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
		
		$admissions = $db->get();

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
				//var_dump($admission);

				try {
				//insert to IMS!
					$ins = DB::table('ims_adpats')->insert(['adp_adm' => $id]);

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
//			return $error;
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
}
