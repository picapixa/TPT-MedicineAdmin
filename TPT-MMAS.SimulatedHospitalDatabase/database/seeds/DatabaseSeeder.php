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
        $this->call(PatientsTableSeeder::class);
        $this->call(HrTableSeeder::class);
        $this->call(AdmissionsTableSeeder::class);
        $this->call(AdmissionAttendantsSeeder::class);
        $this->call(FindingsTableSeeder::class);
    }
}
