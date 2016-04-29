<?php

use Faker\Factory as Faker;
use Illuminate\Database\Seeder;

class FindingsTableSeeder extends Seeder
{

	/**
	 * Run the database seeds.
	 *
	 * @return void
	 */
	public function run()
	{
		$admissions = DB::table('hpt_adm')->get();
		$admission_count = count($admissions);
		$doctors = DB::table('hpt_hr')->where('hr_level', 'dr')->get();

		$faker = Faker::create('en_PH');

		$findings = [];

		// // WHY NOT A SOURCE?
		// http://www.healthphilippines.net/2011/05/illnesses-philippines-philhealth-claims/
		$diags = [
			'Pneumonia',
			'Dengue',
			'Flu',
			'Diarrhoea',
			'Gastroenteritis',
			'Hypertension',
			'Asthma',
			'Amoebiasis',
			'Viral infection',
			'Acute upper respiratory infection'
		];

		$fn_index = 1;
		while ($fn_index <= $admission_count) {
			//determine how many findings per patient
			$count = mt_rand(1,3);
			$adm_findings = [];
			$hr_indices = [];

			$diagnosed_on = [];

			for ($i = 0; $i < $count; $i++) {
				$adm_findings[$i]['ref_adm'] = $fn_index;

				//doctor id
				$rand_hrid;
				do {
					$rand_hrid = $this->getRandomHr($doctors);
				} while (in_array($rand_hrid, $hr_indices));
				$adm_findings[$i]['ref_dr'] = $rand_hrid;
				array_push($hr_indices, $rand_hrid);

				//diagnosis
				$adm_findings[$i]['find_diag'] = $diags[mt_rand(0, count($diags) - 1)];


				//create fake diagnosis history                             
				$adm_rem = $admissions[$fn_index - 1]->adm_remark;
				$adm_dst = $admissions[$fn_index - 1]->adm_datestart;
				$adm_dnd = $admissions[$fn_index - 1]->adm_dateend;

				$adm_dst = date("Y-m-d H:i:s", strtotime($adm_dst));
				$adm_dnd = date("Y-m-d H:i:s", strtotime($adm_dnd));

				// $adm_dst0 = new \DateTime($adm_dst);
				// $adm_dst4 = $adm_dst0->modify('+4 days')->format('Y-m-d H:i:s');

				// var_dump($adm_dst4); exit();

				if ($i == 0) {
					$adm_findings[$i]['find_createdon'] = $adm_dst;
					array_push($diagnosed_on, $adm_dst);
				}
				elseif ($i == 1) {
					if ((($count - 1) == 1) && ($adm_rem == 'DISCHARGED')) {
						$adm_findings[$i]['find_createdon'] = $adm_dnd;
					}
					else {
						$prev = new \DateTime($diagnosed_on[0]);
						$prev_date = $prev->modify('+4 days')->format('Y-m-d H:i:s');

						$next_date = $faker->dateTimeBetween($diagnosed_on[0], $prev_date);
						$adm_findings[$i]['find_createdon'] = $next_date->format('Y-m-d H:i:s');

						//if there's a next round
						if (($count - 1) !== 1)
							array_push($diagnosed_on, $next_date->format('Y-m-d H:i:s'));
					}
				}
				elseif ($i == 2) {
					$prev = new \DateTime($diagnosed_on[1]);
					$prev_date = $prev->modify('+4 days')->format('Y-m-d H:i:s');

					$next_date = $faker->dateTimeBetween($diagnosed_on[0], $prev_date);
					$adm_findings[$i]['find_createdon'] = $next_date->format('Y-m-d H:i:s');

				}
			}


			foreach ($adm_findings as $finding) {
				array_push($findings, $finding);
			}



			$fn_index++;
		}

		DB::table('hpt_findings')->insert($findings);


	}

	private function getRandomHr($db)
	{		
		$hr_rand = $db[mt_rand(0, count($db) - 1)];
		return $hr_rand->hr_id;
	}
}
