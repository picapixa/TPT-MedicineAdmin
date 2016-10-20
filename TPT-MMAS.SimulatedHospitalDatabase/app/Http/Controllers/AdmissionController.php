<?php

namespace App\Http\Controllers;

use DB;
use Illuminate\Http\Request;

use App\Http\Requests;

class AdmissionController extends Controller
{
	/**
	 * Display a listing of the resource.
	 *
	 * @return \Illuminate\Http\Response
	 */
	public function index(Request $request)
	{
		$params = $request->input();

		$admissions_db = DB::table('hpt_adm')
					->where([
						['adm_stn', $params['station']],
						['adm_remark', $params['remark']]
					])
					->get();

		if (empty($admissions_db)) {
			abort(404, "The records that you were looking for are not found.");
		}

		$admissions = [];

		//filter out the ones soft-deleted
		foreach ($admissions_db as $admission => $value) {
			if (is_null($value->deleted_at)) {
				array_push($admissions, $value);
			}
		}

		foreach ($admissions as $admission) {

			//get the patient object
			$id = $admission->adm_patient;
			$p = DB::table('hpt_pat')
				->where('pat_id', $id)
				->first();
			unset($p->pat_rfkey);
			$admission->adm_patient = $p;

			//attach attending physicians
			$physicians = [];
			$physicians_db = DB::table('itr_dradms')
							->where('ref_adm', $admission->adm_id)
							->get();
			
			foreach ($physicians_db as $physician) {
				$p_id = $physician->ref_dr;
				$phy_db = DB::table('hpt_hr')->where('hr_id', $p_id)->first();
				unset($phy_db->hr_uname, $phy_db->hr_level, $phy_db->hr_key, $phy_db->deleted_at);
				array_push($physicians, $phy_db);
			}
			$admission->adm_physicians = $physicians;

			//attach findings history
			$findings = [];
			$findings_db = DB::table('hpt_findings')
						->where('ref_adm', $admission->adm_id)
						->get();
			foreach ($findings_db as $finding) {
				unset($finding->ref_adm);

				$f_id = $finding->ref_dr;
				$phy_db = DB::table('hpt_hr')->where('hr_id', $f_id)->first();
				unset($phy_db->hr_uname, $phy_db->hr_level, $phy_db->hr_key, $phy_db->deleted_at);
				$finding->ref_dr = $phy_db;

				array_push($findings, $finding);
			}
			$admission->adm_findings = $findings;

			//remove the deleted_at property
			unset($admission->deleted_at);

		}

		return response()->json($admissions); 
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
		//
	}

	/**
	 * Display the specified resource.
	 *
	 * @param  int  $id
	 * @return \Illuminate\Http\Response
	 */
	public function show($id)
	{
		$admission = DB::table('hpt_adm')->where('adm_id', $id)->first();

		if (empty($admission)) {
			abort(404, "The records that you were looking for are not found.");
		}

		if (!is_null($admission->deleted_at)) {
			abort(404, "The record that you were looking for was already deleted.");
		}

		//get the patient object
		$pat_id = $admission->adm_patient;
		$p = DB::table('hpt_pat')
			->where('pat_id', $pat_id)
			->first();
		unset($p->pat_rfkey);
		$admission->adm_patient = $p;

		//attach attending physicians
		$physicians = [];
		$physicians_db = DB::table('itr_dradms')
						->where('ref_adm', $admission->adm_id)
						->get();
		
		foreach ($physicians_db as $physician) {
			$p_id = $physician->ref_dr;
			$phy_db = DB::table('hpt_hr')->where('hr_id', $p_id)->first();
			unset($phy_db->hr_uname, $phy_db->hr_level, $phy_db->hr_key, $phy_db->deleted_at);
			array_push($physicians, $phy_db);
		}
		$admission->adm_physicians = $physicians;

		//attach findings history
		$findings = [];
		$findings_db = DB::table('hpt_findings')
					->where('ref_adm', $admission->adm_id)
					->get();
		foreach ($findings_db as $finding) {
			unset($finding->ref_adm);

			$f_id = $finding->ref_dr;
			$phy_db = DB::table('hpt_hr')->where('hr_id', $f_id)->first();
			unset($phy_db->hr_uname, $phy_db->hr_level, $phy_db->hr_key, $phy_db->deleted_at);
			$finding->ref_dr = $phy_db;

			array_push($findings, $finding);
		}
		$admission->adm_findings = $findings;

		unset($admission->deleted_at, $admission->adm_rfkey);
		return response()->json($admission);
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
		//
	}

	/**
	 * Remove the specified resource from storage.
	 *
	 * @param  int  $id
	 * @return \Illuminate\Http\Response
	 */
	public function destroy($id)
	{
		//
	}

	public function verify(Request $request, $id)
	{
		$input = $request->input('key');
		$key = DB::table("hpt_adm")->where("adm_id", $id)->first()->adm_rfkey;

		if (password_verify($input, $key)) {
			return response(null);
		}
		else
			abort(400, "Invalid RF key");
	}

}
