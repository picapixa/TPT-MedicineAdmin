<?php

use Illuminate\Database\Seeder;

class AdmissionAttendantsSeeder extends Seeder
{

	/**
	 * Run the database seeds.
	 *
	 * @return void
	 */
	public function run()
	{
		$admission_count = DB::table('hpt_adm')->count();
		$doctors = DB::table('hpt_hr')->where('hr_level', 'dr')->get();

		$dr_rows = [];
		
		$adm_index = 1;
		while ($adm_index <= $admission_count) { //based on admissions seeder!

			//determine how many doctors will there be
			$count = mt_rand(1,3);
			$appt_drs = [];
			$dr_indices = [];

			for ($i = 0; $i < $count; $i++) { 
				$appt_drs[$i]['ref_adm'] = $adm_index;

				$rand_dr_id;
				do {
					$rand_dr_id = $this->getRandomHr($doctors);
				} while (in_array($rand_dr_id, $dr_indices));

				$appt_drs[$i]['ref_dr'] = $rand_dr_id;
				array_push($dr_indices, $rand_dr_id);
			}

			foreach ($appt_drs as $doctor) {
				array_push($dr_rows, $doctor);
			}

			$adm_index++;
		}
		DB::table('itr_dradms')->insert($dr_rows);
	}

	private function getRandomHr($db)
	{		
		$hr_rand = $db[mt_rand(0, count($db) - 1)];
		return $hr_rand->hr_id;
	}
}
