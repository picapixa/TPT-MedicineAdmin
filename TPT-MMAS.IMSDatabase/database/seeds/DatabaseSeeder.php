<?php

use Illuminate\Database\Seeder;

class DatabaseSeeder extends Seeder
{
	/**
	 * Run the database seeds.
	 *
	 * @return void
	 */
	public function run()
	{
		// $this->call(UsersTableSeeder::class);
		$this->call(MachineSeeder::class);
		$this->call(ImsPatientsTableSeeder::class);
		$this->call(MedicinesSeeder::class);
		$this->call(PatientMedicinesSeeder::class);
	}
}
