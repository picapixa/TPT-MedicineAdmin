<?php 

return [

    /*
    |--------------------------------------------------------------------------
    | Hospital API
    |--------------------------------------------------------------------------
    |
    | This shows the root URL of the REST API of the hospital.
    |
    */

	'hospital-api' => env('TPT-DB_APIURL','http://localhost/tpt-hospital/public'),

    /*
    |--------------------------------------------------------------------------
    | Hospital API version
    |--------------------------------------------------------------------------
    |
    | Specify the API version here.
    |
    */

	'hospital-apiver' => env('TPT-DB_APIVER', 'v1'),

    /*
    |--------------------------------------------------------------------------
    | Hospital station
    |--------------------------------------------------------------------------
    |
    | Identify the shortcode for the station where this
    | installation of IMS will be.
    |
    */


	'hospital-station' => env('TPT-DB_STN', 'DEFAULT')

];