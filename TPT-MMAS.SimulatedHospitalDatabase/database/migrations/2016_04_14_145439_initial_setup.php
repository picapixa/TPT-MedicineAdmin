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
		Schema::create('hpt_pat', function (Blueprint $table) {
			$table->bigIncrements('pat_id');
			$table->string('pat_fname');
			$table->string('pat_mname')->nullable();
			$table->string('pat_lname');
			$table->text('pat_address');
			$table->dateTime('pat_birthday');
			$table->string('pat_contact');
			$table->string('pat_guardian')->nullable();
			$table->string('pat_gcontact')->nullable();
			$table->string('pat_photo')->nullable(); //default value will be handled on-client and not on server
 		});
		
		Schema::create('hpt_hr', function (Blueprint $table) {
			$table->mediumIncrements('hr_id');
			$table->string('hr_fname');
			$table->string('hr_mname')->nullable();
			$table->string('hr_lname');
			$table->string('hr_uname')->unique();
			$table->string('hr_title');
			$table->enum('hr_level', ['admin', 'dr', 'rn', 'hr']);
			$table->string('hr_photo')->nullable();
			$table->string('hr_key');
			$table->softDeletes();
		});
		
		Schema::create('hpt_adm', function (Blueprint $table) {
			$table->bigIncrements('adm_id');
			$table->bigInteger('adm_patient')->unsigned();
			$table->dateTime('adm_datestart')->default(DB::raw('CURRENT_TIMESTAMP'));
			$table->dateTime('adm_dateend')->nullable();
			$table->string('adm_room', 10);
			$table->string('adm_stn');
			$table->enum('adm_remark', ['ADMITTED', 'DISCHARGED'])->default('ADMITTED');
			$table->string('adm_rfkey')->nullable(); //enable cross-IMS transactions and even outside in-patient dept

			$table->softDeletes();
			
			$table->foreign('adm_patient')->references('pat_id')->on('hpt_pat')->onUpdate('cascade');
		});
		
		Schema::create('itr_dradms', function (Blueprint $table) {
			$table->bigInteger('ref_adm')->unsigned();
			$table->mediumInteger('ref_dr')->unsigned();

			$table->foreign('ref_adm')->references('adm_id')->on('hpt_adm')->onUpdate('cascade');
			$table->foreign('ref_dr')->references('hr_id')->on('hpt_hr')->onUpdate('cascade');
		});
		
		Schema::create('hpt_findings', function (Blueprint $table) {
			$table->bigIncrements('find_id');
			$table->bigInteger('ref_adm')->unsigned();
			$table->mediumInteger('ref_dr')->unsigned();
			$table->string('find_diag');
			$table->string('find_icd10',10)->nullable();
			$table->text('find_notes')->nullable();
			$table->timestamp('find_createdon')->default(DB::raw('CURRENT_TIMESTAMP'));

			$table->foreign('ref_adm')->references('adm_id')->on('hpt_adm')->onUpdate('cascade');
			$table->foreign('ref_dr')->references('hr_id')->on('hpt_hr')->onUpdate('cascade');
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
		Schema::drop('itr_dradms');
		Schema::drop('hpt_findings');

		Schema::drop('hpt_adm');
		Schema::drop('hpt_pat');
		Schema::drop('hpt_hr');
	}
}
