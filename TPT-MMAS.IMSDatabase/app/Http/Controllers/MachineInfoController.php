<?php

namespace App\Http\Controllers;

use DB;
use Illuminate\Http\Request;

use App\Http\Requests;

class MachineInfoController extends Controller
{
	/**
	 * Display a listing of the resource.
	 *
	 * @return \Illuminate\Http\Response
	 */
	public function index()
	{
		$data = DB::table('ims_mmas')->get();

		return response()->json($data);
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
		//
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
		$deviceName = $id;
		$input = $request->input();
		$db = DB::table('ims_mmas');

		if (isset($input['isOnline'])) {
			$data = [
				'mmas_lastol' => DB::raw('CURRENT_TIMESTAMP')
			];

			$db_update = $db->where('mmas_machine', $deviceName)->update($data);
		}
		else {
			$db_existing = $db->where('mmas_machine', $deviceName)->first();

			if (count($db_existing) == 0) {
				// insert
				$db_insert = $db->insert(['mmas_machine' => $deviceName, 'mmas_ipa', $ipAddress]);
			}
			else {
				// update
				$data = [
					'mmas_ipa' => $input['ipAddress'],
					'mmas_dateupdate' => DB::raw('CURRENT_TIMESTAMP')
				];

				$db_update = $db->where('mmas_machine', $deviceName)->update($data);
			}			
		}

		$db_new = $db->where('mmas_machine', $deviceName)->first();
		return response()->json($db_new);

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
}
