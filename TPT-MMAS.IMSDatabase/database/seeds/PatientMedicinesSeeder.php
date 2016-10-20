<?php

use Illuminate\Database\Seeder;
use Carbon\Carbon;
use Faker\Factory as Faker;	

class PatientMedicinesSeeder extends Seeder
{
	/**
	 * Run the database seeds.
	 *
	 * @return void
	 */
	public function run()
	{
		$patients = [];

		$ims_recs = DB::table('ims_adpats')->get();
		$med_recs = DB::table('ims_meds')->get();

		foreach ($ims_recs as $record) {
			$patient_sessions = array();

			//determine how many medicines do a patient have
			// and set that at a permanent array
			$prescribed_meds = [];
			$n_meds_prescribed = mt_rand(1,4);

			for ($i = 0; $i < $n_meds_prescribed; $i++) { 
				do {
					$pm = $med_recs[mt_rand(0, count($med_recs) - 1)];
				} while (in_array($pm, $prescribed_meds));
				$prescribed_meds[$i] = $pm;
			}

			//pick between 3, 4, 6 hour intervals for each
			$med_intervals = [3,4,6];
			$med_interval = $med_intervals[mt_rand(0, count($med_intervals) - 1)];

			//determine how many med admin sessions a patient will have
			$admin_n = mt_rand(1,6);

			//this session will run at what time?
			$current_sked = Carbon::now()->addDays(mt_rand(3,5))->hour(0)->minute(0)->second(0);

			//for each admin session
			for ($i = 0; $i < $admin_n; $i++) {
				$session_medicines = [];

				//how many medicines to take
				//at admin session $i
				$n_meds = mt_rand(1, count($prescribed_meds));

				//randomly select a subset of prescribed medicines
				$meds = $prescribed_meds;
				shuffle($meds);
				$selected_meds = array_slice($meds, 0, $n_meds);

				foreach ($selected_meds as $medicine) {
					$med['ref_imsid'] = $record->adp_imsid;
					$med['ref_medid'] = $medicine->med_id;					
					$med['ipm_sched'] = (string)$current_sked;
					$med['ipm_amount'] = mt_rand(1,3);
					$med['ipm_addedon'] = date('Y-m-d H:i:s');
					$med['ipm_hradder'] = "jrpalcoriza"; //dummy username

					array_push($session_medicines, $med);
				}

				$current_sked = $current_sked->addHours($med_interval);

				$patient_sessions = array_merge_recursive($patient_sessions, $session_medicines);
			}

			$patients = array_merge($patients, $patient_sessions);
		}

		DB::table('ims_patmeds')->insert($patients);
	}
}
