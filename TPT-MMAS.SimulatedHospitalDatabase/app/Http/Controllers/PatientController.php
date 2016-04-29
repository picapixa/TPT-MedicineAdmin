<?php

namespace App\Http\Controllers;

use DB;
use Illuminate\Contracts\Routing\ResponseFactory;
use Illuminate\Routing\Controller as BaseController;

class PatientController extends BaseController
{
	public function getPatients()
	{
		//$patients = DB::table('hpt_pat')->paginate(5);
		$patients = DB::table('hpt_pat')->get();
		return response()->json($patients); 
	}
}

