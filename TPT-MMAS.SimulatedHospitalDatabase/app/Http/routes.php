<?php

/*
|--------------------------------------------------------------------------
| Application Routes
|--------------------------------------------------------------------------
|
| Here is where you can register all of the routes for an application.
| It's a breeze. Simply tell Laravel the URIs it should respond to
| and give it the controller to call when that URI is requested.
|
*/

Route::group(['prefix' => 'api'], function() {
	
	Route::group(['prefix' => 'v1'], function() {
		Route::post('admissions/verify/{id}', 'AdmissionController@verify');
		Route::resource('admissions', 'AdmissionController', ['only' => [
			'index', 'show'
		]]);
		Route::post('auth/verify','AuthController@verify');
	});
	
});
