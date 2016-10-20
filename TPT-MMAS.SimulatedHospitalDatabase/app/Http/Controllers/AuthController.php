<?php

namespace App\Http\Controllers;

use DB;
use Illuminate\Http\Request;

use App\Http\Requests;

class AuthController extends Controller
{
	public function verify(Request $request)
	{
		$username = $request->input('user');
		$key = $request->input('key');

		$user = DB::table('hpt_hr')->where('hr_uname', $username)->first();

		if (is_null($user)) {
			abort(404,'The user is not found');
		}

		if (password_verify($key, $user->hr_key)) {
			unset($user->hr_key, $user->deleted_at);
			return response()->json($user);
		}
		else {
			abort(400, 'Incorrect password');
		}
	}
}
