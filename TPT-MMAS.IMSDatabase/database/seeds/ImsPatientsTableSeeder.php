<?php

use Faker\Factory as Faker;
use GuzzleHttp\Client as Guzzle;
use Illuminate\Database\Seeder;

class ImsPatientsTableSeeder extends Seeder
{
	private $seeder_count = 15;

	/**
	 * Run the database seeds.
	 *
	 * @return void
	 */
	public function run()
	{
		$api_v = \Config::get('database-tpt.hospital-apiver');
		$uri = \Config::get('database-tpt.hospital-api') . '/api/' . $api_v . '/';
		$stn = \Config::get('database-tpt.hospital-station');

		$admit_ids = [];

		$faker = Faker::create('en_PH');

		//get number of appointments off of hospdb
		$client = new Guzzle([
			'base_uri' => $uri
		]);
		$response = $client->get('admissions?station=' . $stn . '&remark=admitted');
		$body_json = (string) $response->getBody();

		$admissions = json_decode($body_json);

		$counter = 0;
		foreach ($admissions as $a) {
			$adm = [];
			$adm['adp_adm'] = $a->adm_id;

			$ison_machine = $faker->boolean;
			if ($ison_machine === TRUE && $counter < 8) {
				$adm['ref_mmas'] = 1;
				$counter++;
			}
			else
				$adm['ref_mmas'] = null;
			
			array_push($admit_ids, $adm);
		}

		DB::table('ims_adpats')->insert($admit_ids);
	}
}
