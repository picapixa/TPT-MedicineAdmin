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

// Route::get('/', function () {
//     return view('welcome');
// });

Route::group(['prefix' => 'api'], function() {
	
	Route::group(['prefix' => 'v1'], function() {
		Route::group(['prefix' => 'auth'], function() {
		    Route::post('verify', 'AuthController@verify');
		});

		Route::resource('admissions', 'AdmissionsController');
		Route::resource('inventory', 'InventoryController');
		Route::resource('machines', 'MachineInfoController', ['only' => [
			'index', 'update'
		]]);
		Route::resource('medicines', 'MedicinesController');
	});
	
});

