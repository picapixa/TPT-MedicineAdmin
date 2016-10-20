<?php

namespace App\Http\Controllers;

use DB;
use GuzzleHttp\Client as Guzzle;
use Illuminate\Http\Request;

use App\Http\Requests;

class AdmissionsController extends Controller
{
	/**
	 * Display a listing of the resource.
	 *
	 * @return \Illuminate\Http\Response
	 */
	public function index(Request $request)
	{
		$vars = $request->input();

		$db = DB::table('ims_adpats');

		if((isset($vars['inMachine'])) && ($vars['inMachine'] == 'true'))
			$db->whereNotNull('ref_mmas');
		
		$admissions = [];
		$adm = $db->get();

		return response()->json($adm);
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
		$id = $request->input('adm_id');

		try {
			//check if the admission exists
			$client = new Guzzle([
				'base_uri' => config('database-tpt.hospital-api') . '/api/' . config('database-tpt.hospital-apiver') . '/',
				'headers' => ['Accept' => 'application/json', 'Content-Type' => 'application/json']
			]);			
			$response = $client->get('admissions/' . $id);
			$admission = (string) $response->getBody();

			$statusCode = $response->getStatusCode();
			if ($statusCode == 200) {
				$values = [
					'adp_adm' => $id,
					'ref_mmas' => 1
				];

				try {
					
					//check if adp_adm is already in db
					$existing = DB::table('ims_adpats')->where('adp_adm', $id)->first();

					if (count($existing) == 0)
						$insert = DB::table('ims_adpats')->insert($values);
					else 
						$update = DB::table('ims_adpats')->where('adp_adm', $id)->update($values);

					$row = DB::table('ims_adpats')->where('adp_adm', $id)->first();
					return response()->json($row);

				} catch (\Exception $e) {
					$content = json_encode([
						'errors' => "A database error occurred.",
						'exception' => get_class($e),
						'message' => $e->getMessage(),
						'db_errorCode' => $e->getCode()
					]);

					return response($content, 500);					
				}
			}
			else
			{
				return response()->json([
					"errors" => 'Error ' . $statusCode . ' occurred.',
				]);
			}

		} catch (\GuzzleHttp\Exception\ClientException $e) {

			$error = $e->getResponse()->getBody()->getContents();
			return $error;
		}
	}

	/**
	 * Display the specified resource.
	 *
	 * @param  int  $id
	 * @return \Illuminate\Http\Response
	 */
	public function show($id)
	{	
		$db = DB::table('ims_adpats')->where('adp_imsid', $id)->get();
		return resource()->json($db);
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
		$isInMachine = $request->input('inMachine');
		$db = DB::table('ims_adpats')->where('adp_imsid', $id);

		if ($isInMachine == 'true')
			$db->update(['ref_mmas' => 1]);
		elseif ($isInMachine == 'false')
			$db->update(['ref_mmas' => null, 'adp_remark' => 'inactive']);

		$row = DB::table('ims_adpats')->where('adp_adm', $id)->first();
		return response()->json($row);
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

	/**
	* Gets the admissions data from the hospital database.
	*
	*/
	private function getAdmissionsData($id)
	{
		$client = new Guzzle([
			'base_uri' => config('database-tpt.hospital-api') . '/api/' . config('database-tpt.hospital-apiver') . '/',
			'headers' => ['Accept' => 'application/json', 'Content-Type' => 'application/json']
		]);			
		$response = $client->get('admissions/' . $id);
		$a = (string) $response->getBody();

		return json_decode($a);
	}
}
