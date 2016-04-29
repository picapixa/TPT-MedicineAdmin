<?php

use Illuminate\Database\Seeder;

class MedicinesSeeder extends Seeder
{
	private $seeder_count = 10;

    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
    	$meds = [];

    	for ($i=0; $i < $this->seeder_count; $i++) { 
    		$med = [
    			'med_generic' => '', //keyvalue
    			'med_brand' => '',
    			'med_dosage' => '',
    			'med_stocks' => 5,
    		];

    		$med[$i] = $med;

    	}

        DB::table('ims_meds')->insert($meds);
    }
}
