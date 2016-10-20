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
			$table->timestamp('mmas_dateupdate')->default(DB::raw('CURRENT_TIMESTAMP'));
		});

		Schema::create('ims_adpats', function (Blueprint $table) {
			$table->bigIncrements('adp_imsid');
			$table->bigInteger('adp_adm')->unique()->unsigned();
			$table->enum('adp_remark', ['inactive', 'medicineSelected', 'medicineLoaded', 'medicineAdministered'])->default('Inactive');
			$table->integer('ref_mmas')->unsigned()->nullable();

			$table->foreign('ref_mmas')->references('mmas_id')->on('ims_mmas')->onUpdate('cascade');
		});

		Schema::create('ims_meds', function (Blueprint $table) {
			$table->mediumIncrements('med_id');
			$table->string('med_generic');
			$table->string('med_brand');
			$table->string('med_dosage');
			$table->string('med_stocks');
			$table->timestamp('med_lastadded');
			$table->timestamp('med_lastdisp')->nullable();
		});

		Schema::create('ims_patmeds', function (Blueprint $table) {
			$table->bigIncrements('ipm_id');
			$table->bigInteger('ref_imsid')->unsigned();
			$table->mediumInteger('ref_medid')->unsigned();
			$table->dateTime('ipm_sched');
			$table->integer('ipm_amount'); //how many tablets? this will be subtracted to meds table later
			$table->timestamp('ipm_addedon')->default(DB::raw('CURRENT_TIMESTAMP')); //when the prescription is added to db
			$table->string('ipm_hradder'); //username of hr
			$table->timestamp('ipm_selectedon')->nullable(); // when the patient is selected for administration
			$table->string('ipm_selector')->nullable(); //username of hr
			$table->timestamp('ipm_loadedon')->nullable(); //time when it was added to the mmas
			$table->string('ipm_loadedat')->nullable(); //compname of mmas.
			$table->string('ipm_hrloader')->nullable(); //username of hr
			$table->timestamp('ipm_adminedon')->nullable();
			$table->string('ipm_hradminer')->nullable(); //username of hr
			$table->timestamp('ipm_deletedon')->nullable();

			$table->foreign('ref_imsid')->references('adp_imsid')->on('ims_adpats')->onUpdate('cascade');
			$table->foreign('ref_medid')->references('med_id')->on('ims_meds')->onUpdate('cascade');
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
		Schema::drop('ims_patmeds');

		Schema::drop('ims_meds');
		Schema::drop('ims_adpats');
		Schema::drop('ims_mmas');
	}
}
