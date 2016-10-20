<?php

namespace App\Http\Controllers;

use DB;
use GuzzleHttp\Client as Guzzle;
use GuzzleHttp\Exception\ClientException;
use Illuminate\Http\Request;

use App\Http\Requests;

class AuthController extends Controller
{
	public function verify(Request $request)
	{
		$user = $request->input('user');
		$key = $request->input('key');

		$client = new Guzzle([
			'base_uri' => config('database-tpt.hospital-api') . '/api/' . config('database-tpt.hospital-apiver') . '/',
			'headers' => ['Accept' => 'application/json', 'Content-Type' => 'application/json']
		]);

		try {			
			$response = $client->post('auth/verify?user=' . $user . '&key=' . $key);
			$admission = (string) $response->getBody();

		} catch (ClientException $e) {
			$errorStatusCode = $e->getResponse()->getStatusCode();
			$message = json_decode((string)$e->getResponse()->getBody());

			return response()->json($message, $errorStatusCode);
		}
	

		//TODO: CREATE A TOKENIZER
		return response()->json(json_decode($admission));
	}
}
