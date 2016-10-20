<?php

use Faker\Factory as Faker;
use Illuminate\Database\Seeder;

class AdmissionsTableSeeder extends Seeder
{
	private $seeder_count = 2000;

	/**
	 * Run the database seeds. 
	 *
	 * @return void
	 */
	public function run()
	{
		$this->command->info("This is it. Starting admissions seeding...");

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
			$no = $i + 1;
			$this->command->info("PATIENT # " . $no . ":");

			$admissions[$i]['adm_stn'] = $stns[mt_rand(0, count($stns) - 1)];
			$this->command->info("Station: " . $admissions[$i]['adm_stn']);

			if (count($current_rooms) > 75) {
				$status = "DISCHARGED";
			}
			else
			{
				$status = $rmrks[mt_rand(0, 1)];				
			}
			$this->command->info("Status: " . $status);

			//assign patient id
			//try to evaluate uniqueness of id
			$id;
			do {
				$id = mt_rand(1,$p_count);
			} while (in_array($id, $pat_ids));

			$admissions[$i]['adm_patient'] = $id;
			array_push($pat_ids, $id);
			$this->command->info("Patient ID: " . $id);


 			if ($status == "ADMITTED") {
				$admissions[$i]['adm_datestart'] = $faker->dateTimeBetween('-3 days', 'now');

				$is_there_an_end = $faker->boolean;
	 			if($is_there_an_end == true)
	 				$admissions[$i]['adm_dateend'] = $faker->dateTimeBetween('+20 days', '+25 days');
	 			else
	 				$admissions[$i]['adm_dateend'] = null;

 				// assign a room
 				// make sure it's available
 				$rm_start;
 				$rm_end;

				if ($admissions[$i]['adm_stn'] === "MEDICAL1") {
					$rm_start = 201;
					$rm_end = 210;
				}
				elseif ($admissions[$i]['adm_stn'] === "MEDICAL2") {
					$rm_start = 211;
					$rm_end = 230;			
				}
				elseif ($admissions[$i]['adm_stn'] === "SURGICAL") {
					$rm_start = 301;
					$rm_end = 305;		
				}
				elseif ($admissions[$i]['adm_stn'] === "MEDSURGI") {
					$rm_start = 306;
					$rm_end = 320;

				}
				elseif ($admissions[$i]['adm_stn'] === "PEDIA") {
					$rm_start = 320;
					$rm_end = 330;
				}
				elseif ($admissions[$i]['adm_stn'] === "OB-MATER") {
					$rm_start = 401;
					$rm_end = 415;
				}

				$rand_room = $this->set_admitted_room($current_rooms, $rm_start, $rm_end);
				if ($rand_room != -1) {
					$admissions[$i]['adm_room'] = $rand_room;
					array_push($current_rooms, $rand_room);
					$this->command->info("Current room: " . $rand_room);
				}
				else
				{
					$status = "DISCHARGED";
				}				
 			}
 			elseif ($status == "DISCHARGED")
 			{
				$admissions[$i]['adm_datestart'] = $faker->dateTimeBetween('-35 days', '-30 days');
				$admissions[$i]['adm_dateend'] = $faker->dateTimeBetween('-29 days', '-7 days');

				$room;
				//assign a room based on station
				if ($admissions[$i]['adm_stn'] == "MEDICAL1")
				{
					$room = mt_rand(201,210);
				}
				elseif ($admissions[$i]['adm_stn'] == "MEDICAL2") {
					$room = mt_rand(211,230);				
				}
				elseif ($admissions[$i]['adm_stn'] == 'SURGICAL') {
					$room = mt_rand(301,305);				
				}
				elseif ($admissions[$i]['adm_stn'] == 'MEDSURGI') {
					$room = mt_rand(306,320);				
				}
				elseif ($admissions[$i]['adm_stn'] == 'PEDIA') {
					$room = mt_rand(320,330);				
				}
				elseif ($admissions[$i]['adm_stn'] == 'OB-MATER') {
					$room = mt_rand(401,415);				
				}

				$admissions[$i]['adm_room'] = $room;
				$this->command->info("Previous room: " . $room);
 			}

 			$admissions[$i]['adm_remark'] = $status;

			$this->command->info("----------------------------------------");
		}

		DB::table('hpt_adm')->insert($admissions);
	}

	private function set_admitted_room($set_rooms, $room_start, $room_end)
	{
		$selected_room = -1;
		$room_count = $room_end - $room_start + 1;

		for ($i = 0; $i < $room_count; $i++) { 
			$selected_room = mt_rand($room_start, $room_end);
			if (!in_array($selected_room, $set_rooms))
				break;
		}

		return $selected_room;
	}
}
