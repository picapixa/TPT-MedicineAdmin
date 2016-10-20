<?php

namespace App\Http\Middleware;

use Illuminate\Foundation\Http\Middleware\VerifyCsrfToken as BaseVerifier;

class VerifyCsrfToken extends BaseVerifier
{
	/**
	 * The URIs that should be excluded from CSRF verification.
	 *
	 * @var array
	 */
	protected $except = [
		'api/v1/admissions*',
		'api/v1/machines*',
		'api/v1/inventory*',
		'api/v1/medicines*'
	];
}
