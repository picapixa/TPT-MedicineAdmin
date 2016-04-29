<?php

namespace App\Http\Controllers;

use DB;
use Illuminate\Http\Request;

use App\Http\Requests;

class HrController extends Controller
{
	public function getDoctors()
	{
		$doctors = DB::table('hpt_hr')->where('hr_level', 'dr')->get();

		return response()->json($doctors);
	}
}
