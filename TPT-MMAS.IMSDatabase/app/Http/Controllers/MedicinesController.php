<?php

namespace App\Http\Controllers;

use Carbon\Carbon;
use DB;
use Illuminate\Http\Request;

use App\Http\Requests;

class MedicinesController extends Controller
{
	/**
	 * Display a listing of the resource.
	 *
	 * @return \Illuminate\Http\Response
	 */
	public function index()
	{
	}

	/**
	 * Show the form for creating a new resource.
	 *
	 * @return \Illuminate\Http\Response
	 */
	public function create()
	{
		//
	}

	/**
	 * Store a newly created resource in storage.
	 *
	 * @param  \Illuminate\Http\Request  $request
	 * @return \Illuminate\Http\Response
	 */
	public function store(Request $request)
	{
		$body = json_decode($request->getContent());
		$ims_id = $request->input('aid');
		$multiplier = $request->input('multiplier');
		$duration = $request->input('duration');

		if (!is_null($multiplier) && !is_null($duration)) 
		{
			//has multiplier and duration
			$data = [];

			$interval = 24 / $multiplier;
			$n = $multiplier * $duration;
			
			$sched_parsed = Carbon::parse($body->ipm_sched);

			for ($i = 0; $i < $n; $i++) {
				$data[$i] = $this->insert_prescription_to_db($ims_id, $body, $sched_parsed);
				$sched_parsed->addHours($interval);
			}
		}
		else
		{
			$sched_parsed = Carbon::parse($body->ipm_sched);
			$data = $this->insert_prescription_to_db($ims_id, $body, $sched_parsed);
		}

		return response()->json($data);
	}

	/**
	 * Display the specified resource.
	 *
	 * @param  int  $id
	 * @return \Illuminate\Http\Response
	 */
	public function show($id)
	{
		$data = DB::table('ims_patmeds')->where('ref_imsid', $id)->whereNull('ipm_deletedon')->get();

		foreach ($data as $item) {
			$med_id = $item->ref_medid;
			unset($item->ref_medid);

			$med = DB::table('ims_meds')->where('med_id', $med_id)->first();
			
			$item->ipm_medicine = $med;
		}

		return response()->json($data);
	}

	/**
	 * Show the form for editing the specified resource.
	 *
	 * @param  int  $id
	 * @return \Illuminate\Http\Response
	 */
	public function edit($id)
	{
		//
	}

	/**
	 * Update the specified resource in storage.
	 *
	 * @param  \Illuminate\Http\Request  $request
	 * @param  int  $id
	 * @return \Illuminate\Http\Response
	 */
	public function update(Request $request, $id)
	{
		$body = json_decode($request->getContent());

		if (count($body) < 1) {
			abort(400);
		}
		elseif (count($body) == 1) {
			if (is_array($body)) {
				$body = $body[0];
			}

			$body->ref_medid = $body->ipm_medicine->med_id;
			unset($body->ipm_medicine);

			$resp = DB::table("ims_patmeds")->where("ipm_id",$body->ipm_id)->update((array)$body);
			$obj = DB::table("ims_patmeds")->where("ipm_id",$body->ipm_id)->first();
			$this->set_admitted_patient_status($id, $obj);
			return response($resp);
		}
		else
		{
			$resp;

			for ($i = 0; $i < count($body); $i++) { 
				$obj = $body[$i];

				$obj->ref_medid = $obj->ipm_medicine->med_id;
				unset($obj->ipm_medicine);

				$resp = DB::table("ims_patmeds")->where("ipm_id",$obj->ipm_id)->update((array)$obj);
			
				// just get the first one; all medicines have same status anyway
				if ($i == 0) {
					$this->set_admitted_patient_status($id, $obj);
				}
			}

			return response($resp);
		}		
	}

	/**
	 * Remove the specified resource from storage.
	 *
	 * @param  int  $id
	 * @return \Illuminate\Http\Response
	 */
	public function destroy(Request $request,$id)
	{
		//if the resources to be deleted is grouped
		if ($id == "-1") {
			$body = json_decode($request->getContent());
			$count = count($body);

			if ($count > 1) {
				$processed_items = 0;

				foreach ($body as $item) {
					$query = DB::table('ims_patmeds')->where('ipm_id', $item->ipm_id)->update(['ipm_deletedon' => DB::raw('CURRENT_TIMESTAMP')]);
					$processed_items += $query;
				}
				if ($processed_items < $count) {
					abort(500);
				}
			}
			else
			{	
				$query = DB::table('ims_patmeds')->where('ipm_id', $body[0]->ipm_id)->update(['ipm_deletedon' => DB::raw('CURRENT_TIMESTAMP')]);
				if ($query !== 1) {
					abort(500);
				}
			}
		}
		else
		{
			$query = DB::table('ims_patmeds')->where('ipm_id', $id)->update(['ipm_deletedon' => DB::raw('CURRENT_TIMESTAMP')]);
			if ($query !== 1) {
				abort(500);
			}
		}
	}

	private function insert_prescription_to_db($aid, $body, $timestamp)
	{
		$prescription = clone $body;

		$prescription->ipm_sched = $timestamp;
		$prescription->ref_medid = $prescription->ipm_medicine->med_id;
		$prescription->ref_imsid = $aid;

		unset($prescription->ipm_id, $prescription->ipm_medicine);
		
		// insert then retrieve again
		$id = DB::table('ims_patmeds')->insertGetId((array)$prescription);
		$data = DB::table('ims_patmeds')->where('ipm_id', $id)->first();

		//translate medicine data from ID to object
		$med_id = $data->ref_medid;
		unset($data->ref_medid);
		
		$med = DB::table('ims_meds')->where('med_id', $med_id)->first();
		
		$data->ipm_medicine = $med;
		// unset($data->ipm_medicine->med_stocks);

		return $data;
	}

	private function set_admitted_patient_status($id, $body)
	{
		$remark;
		$db = DB::table('ims_adpats')->where('adp_imsid', $id);

		if (!($body->ipm_adminedon === NULL)) 
			$remark = 'medicineAdministered';
		elseif (!($body->ipm_loadedon === NULL)) 
			$remark = 'medicineLoaded';
		elseif (!($body->ipm_selectedon === NULL)) 
			$remark = 'medicineSelected';
		else
			$remark = 'inactive';

		$db->update(['adp_remark' => $remark]);
	}
}
