<?php

use Illuminate\Database\Seeder;

class MachineSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
    	$mmas = [
    		'mmas_machine' => 'tpt-mmas-1a',
    		'mmas_ipa' => '192.168.17.211',
    	];

    	DB::table('ims_mmas')->insert($mmas);
    }
}
