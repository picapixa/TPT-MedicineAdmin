<?php

use Illuminate\Database\Schema\Blueprint;
use Illuminate\Database\Migrations\Migration;

class InitialSetup extends Migration
{
	/**
	 * Run the migrations.
	 *
	 * @return void
	 */
	public function up()
	{

		Schema::create('ims_mmas', function (Blueprint $table) {
			$table->increments('mmas_id');
			$table->string('mmas_machine'); //computername
			$table->ipAddress('mmas_ipa'); //ip add
			$table->timestamp('mmas_datereg')->default(DB::raw('CURRENT_TIMESTAMP'));
			$table->timestamp('mmas_lastol')->nullable();
		});

		Schema::create('ims_adpats', function (Blueprint $table) {
			$table->bigIncrements('adp_imsid');
			 //admin id must be unique and should only be there when the machine is to be used
			$table->bigInteger('adp_adm')->unique()->unsigned();
			$table->integer('ref_mmas')->unsigned()->nullable();

			$table->foreign('ref_mmas')->references('mmas_id')->on('ims_mmas')->onUpdate('cascade');
		});

		Schema::create('ims_meds', function (Blueprint $table) {
			$table->mediumIncrements('med_id');
			$table->string('med_generic');
			$table->string('med_brand');
			$table->string('med_dosage');
			$table->string('med_stocks');
			$table->string('med_lastadded');
			$table->string('med_lastdisp')->nullable();
		});

		Schema::create('itr_patmeds', function (Blueprint $table) {
			$table->bigIncrements('ipm_id');
			$table->bigInteger('ref_imsid')->unsigned();
			$table->bigInteger('ref_medid')->unsigned();
			$table->dateTime('ipm_sched');
			$table->integer('ipm_amount'); //how many tablets? this will be subtracted to meds table later
			$table->string('ipm_mmas'); //compname of mmas.
			$table->timestamp('ipm_addedon'); //when the prescription is added to db
			$table->mediumInteger('ipm_hradder');
			$table->timestamp('ipm_loadedon'); //time when it was added to the mmas
			$table->mediumInteger('ipm_hrloader');
			$table->timestamp('ipm_adminedon')->nullable();
			$table->mediumInteger('ipm_hradminer')->nullable();
		
		});

	}

	/**
	 * Reverse the migrations.
	 *
	 * @return void
	 */
	public function down()
	{
		//drop intermediary tables first (ones with fkeys)
		Schema::drop('itr_patmeds');

		Schema::drop('ims_meds');
		Schema::drop('ims_adpats');
		Schema::drop('ims_mmas');
	}
}
