<?php

use Faker\Factory as Faker;
use Illuminate\Database\Seeder;

class AdmissionsTableSeeder extends Seeder
{
	private $seeder_count = 50;

	/**
	 * Run the database seeds. 
	 *
	 * @return void
	 */
	public function run()
	{
		$admissions = [];

		$faker = Faker::create('en_PH');
		
		$stns = [
			'MEDICAL1', 
			'MEDICAL2', 
			'SURGICAL',
			'MEDSURGI',
			'PEDIA',
			'OB-MATER'
		];

		$rmrks = ['ADMITTED', 'DISCHARGED'];

		$pat_ids = [];

		$current_rooms = [];

		$p_count = DB::table('hpt_pat')->count();

		for ($i = 0; $i < $this->seeder_count; $i++) { 
			$admissions[$i]['adm_stn'] = $stns[mt_rand(0, count($stns) - 1)];

			if (count($current_rooms) > 75) {
				$status = "DISCHARGED";
			}
			else
			{
				$status = $rmrks[mt_rand(0, 1)];				
			}

			//assign patient id
			//try to evaluate uniqueness of id
			$id;
			do {
				$id = mt_rand(1,$p_count);
			} while (in_array($id, $pat_ids));

			$admissions[$i]['adm_patient'] = $id;
			array_push($pat_ids, $id);
 			

 			if ($status == "ADMITTED") {
				$admissions[$i]['adm_datestart'] = $faker->dateTimeBetween('-3 days', 'now');

				$is_there_an_end = $faker->boolean;
	 			if($is_there_an_end == true)
	 				$admissions[$i]['adm_dateend'] = $faker->dateTimeBetween('+20 days', '+25 days');
	 			else
	 				$admissions[$i]['adm_dateend'] = null;

 				// assign a room
 				// make sure it's available
 				$rand_room;
 				do {
					if ($admissions[$i]['adm_stn'] === "MEDICAL1") {
						$rand_room = mt_rand(201,210);
					}
					elseif ($admissions[$i]['adm_stn'] === "MEDICAL2") {
						$rand_room = mt_rand(211,230);				
					}
					elseif ($admissions[$i]['adm_stn'] === "SURGICAL") {
						$rand_room = mt_rand(301,305);				
					}
					elseif ($admissions[$i]['adm_stn'] === "MEDSURGI") {
						$rand_room = mt_rand(306,320);				
					}
					elseif ($admissions[$i]['adm_stn'] === "PEDIA") {
						$rand_room = mt_rand(320,330);				
					}
					elseif ($admissions[$i]['adm_stn'] === "OB-MATER") {
						$rand_room = mt_rand(401,415);				
					}
					else
						$rand_room = 100;			
 				} while (in_array($rand_room, $current_rooms));

				$admissions[$i]['adm_room'] = $rand_room;
				array_push($current_rooms, $rand_room);
 			}
 			elseif ($status == "DISCHARGED")
 			{
				$admissions[$i]['adm_datestart'] = $faker->dateTimeBetween('-35 days', '-30 days');
				$admissions[$i]['adm_dateend'] = $faker->dateTimeBetween('-29 days', '-7 days');

				//assign a room based on station
				if ($admissions[$i]['adm_stn'] == "MEDICAL1")
				{
					$admissions[$i]['adm_room'] = mt_rand(201,210);
				}
				elseif ($admissions[$i]['adm_stn'] == "MEDICAL2") {
					$admissions[$i]['adm_room'] = mt_rand(211,230);				
				}
				elseif ($admissions[$i]['adm_stn'] == 'SURGICAL') {
					$admissions[$i]['adm_room'] = mt_rand(301,305);				
				}
				elseif ($admissions[$i]['adm_stn'] == 'MEDSURGI') {
					$admissions[$i]['adm_room'] = mt_rand(306,320);				
				}
				elseif ($admissions[$i]['adm_stn'] == 'PEDIA') {
					$admissions[$i]['adm_room'] = mt_rand(320,330);				
				}
				elseif ($admissions[$i]['adm_stn'] == 'OB-MATER') {
					$admissions[$i]['adm_room'] = mt_rand(401,415);				
				}
 			}

 			$admissions[$i]['adm_remark'] = $status;

		}

		DB::table('hpt_adm')->insert($admissions);
	}
}
