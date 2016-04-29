<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Patient extends Model
{
    protected $table = "hpt_pat";
    protected $primaryKey = "pat_id";
    public $timestamps = false;
}
