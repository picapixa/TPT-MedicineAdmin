<?php

use Faker\Factory as Faker;
use Illuminate\Database\Seeder;

class HrTableSeeder extends Seeder
{
	private $seed_count = 20;

	/**
	 * Run the database seeds.
	 *
	 * @return void
	 */
	public function run()
	{
		$faker = Faker::create('en_PH');

		$default_pw = 'helloworld';

		$all_personnel = [];

		array_push($all_personnel, [
				'hr_fname' => 'Jann Raphael',
				'hr_mname' => 'Perez',
				'hr_lname' => 'Alcoriza',
				'hr_uname' => 'jrpalcoriza',
				'hr_title' => 'Administrator',
				'hr_level' => 'admin',
				'hr_key'   => password_hash($default_pw, PASSWORD_DEFAULT)
		]);

		array_push($all_personnel, [
				'hr_fname' => 'Jonas Edwin',
				'hr_mname' => 'Colorado',
				'hr_lname' => 'Fonacier',
				'hr_uname' => 'jecfonacier',
				'hr_title' => 'Administrator',
				'hr_level' => 'admin',
				'hr_key'   => password_hash($default_pw, PASSWORD_DEFAULT)
		]);

		array_push($all_personnel, [
				'hr_fname' => 'Glenn Howard',
				'hr_mname' => 'Celerio',
				'hr_lname' => 'Paredes',
				'hr_uname' => 'ghcparedes',
				'hr_title' => 'Administrator',
				'hr_level' => 'admin',
				'hr_key'   => password_hash($default_pw, PASSWORD_DEFAULT)
		]);

		array_push($all_personnel, [
				'hr_fname' => 'Charlemagne Zoilo',
				'hr_mname' => 'Constantino',
				'hr_lname' => 'Rivera',
				'hr_uname' => 'czcrivera',
				'hr_title' => 'Administrator',
				'hr_level' => 'admin',
				'hr_key'   => password_hash($default_pw, PASSWORD_DEFAULT)
		]);

		array_push($all_personnel, [
				'hr_fname' => 'Joven',
				'hr_mname' => 'Ela',
				'hr_lname' => 'Subala',
				'hr_uname' => 'jesubala',
				'hr_title' => 'Administrator',
				'hr_level' => 'admin',
				'hr_key'   => password_hash($default_pw, PASSWORD_DEFAULT)
		]);

		$dummy_medpeeps = [];
		$levels = ['dr', 'dr', 'dr', 'rn', 'rn', 'rn', 'hr'];

		$dr_titles = [
			'Cardiologist',
			'Physician',
			'Pulmonologist',
			'General Surgeon',
			'Obstetrician',
			'Gynecologist',
			'Surgeon'
		];

		for ($i = 0; $i < $this->seed_count; $i++){
			$dummy_medpeeps[$i] = [
				'hr_fname' => $faker->firstName,
				'hr_mname' => $faker->lastName,
				'hr_lname' => $faker->lastName,
				'hr_level' => $levels[mt_rand(0, count($levels) - 1)],
				'hr_key' => password_hash($default_pw, PASSWORD_DEFAULT)
			];

			//set uname
			$fn = strtolower($dummy_medpeeps[$i]['hr_fname'][0]);
			$mn = strtolower($dummy_medpeeps[$i]['hr_mname'][0]);
			$ln = strtolower($dummy_medpeeps[$i]['hr_lname']);
			$ln = preg_replace('/\s+/', '', $ln);	//remove spaces
			$dummy_medpeeps[$i]['hr_uname'] = $fn . $mn . $ln;

			//set title based on level
			if ($dummy_medpeeps[$i]['hr_level'] == 'hr') {
				$dummy_medpeeps[$i]['hr_title'] = "Staff";
			}
			elseif ($dummy_medpeeps[$i]['hr_level'] == 'rn') {
				$dummy_medpeeps[$i]['hr_title'] = "Nurse";
			}
			elseif ($dummy_medpeeps[$i]['hr_level'] == 'dr') {
				$dummy_medpeeps[$i]['hr_title'] = $dr_titles[mt_rand(0, count($dr_titles) - 1)];
			}
		}
		foreach ($dummy_medpeeps as $personnel) {
			array_push($all_personnel, $personnel);
		}

		DB::table('hpt_hr')->insert($all_personnel);
	}
}
