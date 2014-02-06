/*  wisdom.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2013 Warren Pratt, NR0V

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at  

warren@wpratt.com

*/

#define _CRT_SECURE_NO_WARNINGS
#include "comm.h"

PORT
void WDSPwisdom (char* directory)
{
	fftw_plan tplan;
	int psize = 64;
	const int maxsize = 32768;						// powers of two
	FILE *stream;
	double* fftin;
	double* fftout;
	char wisdom_file[1024];
	strcpy (wisdom_file, directory);
	strncat (wisdom_file, "wdspWisdom", 16);
	if(!fftw_import_wisdom_from_filename(wisdom_file))
	{
		fftin =  (double *) malloc0 ((maxsize + 1) * 2 * sizeof (double));
		fftout = (double *) malloc0 ((maxsize + 1) * 2 * sizeof (double));
		AllocConsole();								// create console
	    freopen_s(&stream, "conout$", "w", stdout); // redirect output to console
		fprintf(stdout, "Optimizing FFT sizes through %d\n", maxsize);
		fprintf(stdout, "Please do not close this window until wisdom plans are completed.\n");
		while (psize <= maxsize)
		{
			fprintf(stdout, "Planning FORWARD  FFT size %d\n", psize);
			fflush(stdout);
			tplan = fftw_plan_dft_1d(psize, (fftw_complex *)fftin, (fftw_complex *)fftout, FFTW_FORWARD, FFTW_PATIENT);
			fftw_execute (tplan);
			fftw_destroy_plan (tplan);
			fprintf(stdout, "Planning BACKWARD FFT size %d\n", psize);
			fflush(stdout);
			tplan = fftw_plan_dft_1d(psize, (fftw_complex *)fftin, (fftw_complex *)fftout, FFTW_BACKWARD, FFTW_PATIENT);
			fftw_execute (tplan);
			fftw_destroy_plan (tplan);
			fprintf(stdout, "Planning BACKWARD FFT size %d\n", psize + 1);
			fflush(stdout);
			tplan = fftw_plan_dft_1d(psize + 1, (fftw_complex *)fftin, (fftw_complex *)fftout, FFTW_BACKWARD, FFTW_PATIENT);
			fftw_execute (tplan);
			fftw_destroy_plan (tplan);
			psize *= 2;
		}
		fftw_export_wisdom_to_filename(wisdom_file);
		_aligned_free (fftout);
		_aligned_free (fftin);
		FreeConsole();							// dismiss console
	}
}