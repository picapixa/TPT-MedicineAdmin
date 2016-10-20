<?php

use Illuminate\Database\Seeder;
use Faker\Factory as Faker;	

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
		$faker = Faker::create('en_PH');
		$meds = [];
		$source = [
			["Paracetamol", "Biogesic"],
			["Metformin", "Glucophage"],
			["Loperamide", "Immodium"],
			["Mefenamic Acid", "Dolfenal"],
			["Amoxicillin", "Amoxil"],
			
		];
		$dosage=["250mg", "500mg"];
		
		for ($i=0; $i < $this->seeder_count; $i++) { 
			$index = mt_rand(0, count($source)-1);
			$med = [
				'med_generic' =>$source[$index][0], //keyvalue
				'med_brand' => $source[$index][1],
				'med_dosage' => $dosage[mt_rand(0, count($dosage)-1)],
				'med_stocks' => mt_rand(0, 50),
				'med_lastadded'=> $faker->dateTimeBetween($startDate = '-15 days', $endDate = 'now', $timezone = date_default_timezone_get()),
			];

			$meds[$i] = $med;

		}

		DB::table('ims_meds')->insert($meds);
	}
}
