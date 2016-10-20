<?php

use Faker\Factory as Faker;
use Illuminate\Database\Seeder;

class PatientsTableSeeder extends Seeder
{
	private $seeder_count = 2000;

	/**
	 * Run the database seeds.
	 * guidance: http://stackoverflow.com/questions/26143315/laravel-5-artisan-seed-reflectionexception-class-songstableseeder-does-not-e
	 *
	 * @return void
	 */
	public function run()
	{
		$faker = Faker::create('en_PH');
		$patients = [];
		
		for ($i = 0; $i < $this->seeder_count; $i++) { 

			$patients[$i] = [
				'pat_fname' => $faker->firstName,
				'pat_mname' => $faker->lastName,
				'pat_lname' => $faker->lastName,
				'pat_birthday' => $faker->dateTime,
				'pat_address' => $faker->address,
				'pat_contact' => $faker->phoneNumber,
				'pat_guardian' => $faker->name
			];
		}
		
		DB::table('hpt_pat')->insert($patients);
	}
}
