/*  analyzer.c

This file is part of a program that implements a Spectrum Analyzer
used in conjunction with software-defined-radio hardware.

Copyright (C) 2012, 2013, 2014 Warren Pratt, NR0V
Copyright (C) 2012 David McQuate, WA8YWQ - Kaiser window & Bessel function added.

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

#include "comm.h"

double bessi0(double x)
{
	double ax,ans;
	double y;

	if ((ax=fabs(x)) < 3.75) {
		y = x / 3.75,  y = y * y;
		ans = 1.0 + y * (3.5156229 + y * (3.0899424 +  y* (1.2067492
			+ y * (0.2659732 + y * (0.360768e-1 + y * 0.45813e-2)))));
	} else {
		y = 3.75 / ax;
		ans = (exp(ax) / sqrt(ax)) * (0.39894228 + y * (0.1328592e-1
			+ y * (0.225319e-2 + y * (-0.157565e-2 + y * (0.916281e-2
			+ y * (-0.2057706e-1 + y * (0.2635537e-1 + y * (-0.1647633e-1
			+ y * 0.392377e-2))))))));
	}
	return ans;
}

void new_window(int disp, int type, int size, double PiAlpha)
{
	DP a = pdisp[disp];
	int i;
	double arg0, arg1, sum;
	double inv_coherent_gain;
	switch (type)
	{
	case 0:					// rectangular window
		{
			inv_coherent_gain = 1.0;
			for (i = 0; i < size; i++)
				a->window[i] = inv_coherent_gain * 1.0;
			break;
		}
	case 1:					// blackman-harris window
		{
			arg0 = 2.0 * PI / ((double)size - 1.0);
			sum = 0.0;
			for (i = 0; i < size; i++)
			{
				arg1 = arg0 * (double)i;
				a->window[i] = 0.35875 - 0.48829 * cos(arg1) + 0.14128 * cos(2.0 * arg1) - 0.01168 * cos(3.0 * arg1);
				sum += a->window[i];
			}
			inv_coherent_gain = (double)size / sum;
			for (i = 0; i < size; i++)
				a->window[i] *= inv_coherent_gain;
			break;
		}
	case 2:					// hann window
		{
			arg0 = 2.0 * PI / ((double)size - 1.0);
			sum = 0.0;
			for (i = 0; i < size; i++)
			{
				a->window[i] = 0.5 * (1.0 - cos((double)i * arg0));
				sum += a->window[i];
			}
			inv_coherent_gain = (double)size / sum;
			for (i = 0; i < size; i++)
				a->window[i] *= inv_coherent_gain;
			break;
		}
	case 3:					// flat-top window
		{
			arg0 = 2.0 * PI / ((double)size - 1.0);
			sum = 0.0;
			for (i = 0; i < size; i++)
			{
				arg1 = arg0 * (double)i;
				a->window[i] = 0.21557895 - 0.41663158 * cos(arg1) + 0.277263158 * cos(2.0 * arg1) - 0.083578947 * cos(3.0 * arg1) + 0.006947368 * cos (4.0 * arg1);
				sum += a->window[i];
			}
			inv_coherent_gain = (double)size / sum;
			for (i = 0; i < size; i++)
				a->window[i] *= inv_coherent_gain;
			break;
		}
	case 4:					// hamming window
		{
			arg0 = 2.0 * PI / ((double)size - 1.0);
			sum = 0.0;
			for (i = 0; i < size; i++)
			{
				a->window[i] = (0.54 - 0.46 * cos((double)i * arg0));
				sum += a->window[i];
			}
			inv_coherent_gain = (double)size / sum;
			for (i = 0; i < size; i++)
				a->window[i] *= inv_coherent_gain;
			break;
		}
	case 5:					// Kaiser window
		{	arg0 = bessi0(a->PiAlpha);
			arg1 = (double)(size - 1);
			sum = 0.0;
			for (i = 0; i < size; ++i)
			{
				a->window[i] = bessi0(a->PiAlpha * sqrt(1.0 - pow(2.0 * (double)i / arg1 - 1.0, 2))) / arg0;
				sum += a->window[i];
			}
			inv_coherent_gain = (double)size / sum;
			for (i = 0; i < size; i++)
				a->window[i] *= inv_coherent_gain;
			break;
		}
	}
}

void eliminate(int disp, int ss, int LO)
{
	DP a = pdisp[disp];
	int i, k, begin, end, ilim;
	double mag;

	if (ss == a->begin_ss)
		begin = a->fscL + a->clip;
	else
		begin = a->clip;
	if (ss == a->end_ss)
		end = a->out_size - a->clip - a->fscH;
	else
		end = a->out_size - a->clip;

	ilim = a->out_size - 1;

	if (a->flip[LO])
		for (i = ilim - begin, k = 0; i > ilim - end; i--, k++)
		{
			mag = (a->fft_out[ss][LO])[i][0] * (a->fft_out[ss][LO])[i][0] + (a->fft_out[ss][LO])[i][1] * (a->fft_out[ss][LO])[i][1];
			if ((a->spec_flag[ss] == 0) || (mag < (a->result[ss])[k]))
				(a->result[ss])[k] = mag;
		}
	else
		for (i = begin, k = 0; i < end; i++, k++)
		{
			mag = (a->fft_out[ss][LO])[i][0] * (a->fft_out[ss][LO])[i][0] + (a->fft_out[ss][LO])[i][1] * (a->fft_out[ss][LO])[i][1];
			if ((a->spec_flag[ss] == 0) || (mag < (a->result[ss])[k]))
				(a->result[ss])[k] = mag;
		}
		a->ss_bins[ss] = k;
}

void Celiminate(int disp, int ss, int LO)
{
	DP a = pdisp[disp];
	int i, k, begin0, end0, begin1, end1, ilim;
	double mag;

	if (ss == a->begin_ss)
	{
		begin0 = a->out_size / 2 + a->clip + a->fscL;
		if (begin0 > a->out_size)
			begin1 = begin0 - a->out_size;
		else
			begin1 = 0;
	}
	else
	{
		begin0 = a->out_size / 2 + a->clip;
		begin1 = 0;
	}
	if (ss == a->end_ss)
	{
		end1 = a->out_size / 2 - a->clip - a->fscH;
		if (end1 < 0)
			end0 = a->out_size + end1;
		else
			end0 = a->out_size;
	}
	else
	{
		end0 = a->out_size;
		end1 = a->out_size / 2 - a->clip;
	}

	ilim = a->out_size - 1;

	if (a->flip[LO])
	{
		for (i = ilim - begin0, k = 0; i > ilim - end0; i--, k++)
		{
			mag = (a->fft_out[ss][LO])[i][0] * (a->fft_out[ss][LO])[i][0] + (a->fft_out[ss][LO])[i][1] * (a->fft_out[ss][LO])[i][1];
			if ((a->spec_flag[ss] == 0) || (mag < (a->result[ss])[k]))
				(a->result[ss])[k] = mag;
		}
		for (i = ilim - begin1; i > ilim - end1; i--, k++)
		{
			mag = (a->fft_out[ss][LO])[i][0] * (a->fft_out[ss][LO])[i][0] + (a->fft_out[ss][LO])[i][1] * (a->fft_out[ss][LO])[i][1];
			if ((a->spec_flag[ss] == 0) || (mag < (a->result[ss])[k]))
				(a->result[ss])[k] = mag;
		}
	}
	else
	{
		for (i = begin0, k = 0; i < end0; i++, k++)
		{
			mag = (a->fft_out[ss][LO])[i][0] * (a->fft_out[ss][LO])[i][0] + (a->fft_out[ss][LO])[i][1] * (a->fft_out[ss][LO])[i][1];
			if ((a->spec_flag[ss] == 0) || (mag < (a->result[ss])[k]))
				(a->result[ss])[k] = mag;
		}
		for (i = begin1; i < end1; i++, k++)
		{
			mag = (a->fft_out[ss][LO])[i][0] * (a->fft_out[ss][LO])[i][0] + (a->fft_out[ss][LO])[i][1] * (a->fft_out[ss][LO])[i][1];
			if ((a->spec_flag[ss] == 0) || (mag < (a->result[ss])[k]))
				(a->result[ss])[k] = mag;
		}
	}
	a->ss_bins[ss] = k;
}

void stitch(int disp)
{
	DP a = pdisp[disp];
	int i, n, m, mna;
	int pix_count = 0;
	double factor;

	EnterCriticalSection(&a->ResampleSection);
	{			// for non-averaged data
		mna = 0;
		for (n = a->begin_ss; n <= a->end_ss; n++)
			for (i = 0; i < a->ss_bins[n]; i++, mna++)
			{
				a->na_out[mna] = 10.0 * mlog10(a->scale * (a->result[n])[i] + 1.0e-60);
			}
	}

	m = 0;
	switch (a->av_mode)
	{
	case 5:		// weighted averaging of linear data (low noise floor)
		{
			double onem_avb = 1.0 - a->av_backmult;
			for (n = a->begin_ss; n <= a->end_ss; n++)
				for (i = 0; i < a->ss_bins[n]; i++, m++)
				{
					a->pre_av_sum[m] = a->av_backmult * a->pre_av_sum[m] + onem_avb * a->scale * (a->result[n])[i];
					a->pre_av_out[m] = 10.0 * mlog10(a->pre_av_sum[m] + 1e-60);
				}
			break;
		}
	case 6:		// weighted averaging of log data (low noise floor)
		{
			double onem_avb = 1.0 - a->av_backmult;
			for (n = a->begin_ss; n <= a->end_ss; n++)
				for (i = 0; i < a->ss_bins[n]; i++, m++)
				{
					a->pre_av_sum[m] = a->av_backmult * a->pre_av_sum[m] + onem_avb * 10.0 * mlog10(a->scale * (a->result[n])[i] + 1e-60);
					a->pre_av_out[m] = a->pre_av_sum[m];
				}
			break;
		}
	default:
		{
			double *ptr = a->pre_av_out;
			for (n = a->begin_ss; n <= a->end_ss; n++)
			{
				memcpy(ptr, a->result[n], a->ss_bins[n] * sizeof(double));
				ptr += a->ss_bins[n];
				m += a->ss_bins[n];
			}
			break;
		}
	}
	
	for (i = 0; i < a->num_pixels; i++)
	{
		a->t_pixels[i]   = -1000.0;
		a->na_tpixels[i] = -1000.0;
	}

	if (a->pix_per_bin <= 1.0)
	{
		for (i = 0; i < m; i++)
		{
			pix_count = (int)((double)i * a->pix_per_bin);
			if (a->pre_av_out[i] > a->t_pixels[pix_count])
				a->t_pixels[pix_count] = a->pre_av_out[i];
			if (a->na_out[i] > a->na_tpixels[pix_count])
				a->na_tpixels[pix_count] = a->na_out[i];
		}
	}
	else
	{
		double frac;
		double pix_pos = 0;
		for (i = 1; i < m; i++)
		{
			while (pix_pos < (double)i)
			{
				frac = pix_pos - (double)(i - 1);
				a->t_pixels[pix_count]   = a->pre_av_out[i - 1] * (1.0 - frac) + a->pre_av_out[i] * frac;
				a->na_tpixels[pix_count] = a->na_out[i - 1]     * (1.0 - frac) + a->na_out[i]     * frac;
				pix_count++;
				pix_pos += a->bin_per_pix;
			}
		}
	}
	
	for (i = 0; i < a->num_pixels; i++)
		(a->napixels[a->na_wpixbuff])[i] = (dOUTREAL)(a->na_tpixels[i] + 10.0 * mlog10(a->cd[i]));

	switch (a->av_mode)
	{
	case -1:	// peak detection
		{
			for (i = 0; i < a->num_pixels; i++)
			{
				if (a->t_pixels[i] > a->av_sum[i])
					a->av_sum[i] = a->t_pixels[i];
				(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(10.0 * mlog10(a->scale * a->cd[i] * a->av_sum[i] + 1e-60));
			}
			break;
		}
	case 0:		// no averaging
		{
			for (i = 0; i < a->num_pixels; i++)
				(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(10.0 * mlog10(a->scale * a->cd[i] * a->t_pixels[i] + 1e-60));
			break;
		}
	case 1:		// weighted averaging of linear data
		{
			double onem_avb = 1.0 - a->av_backmult;
			for (i = 0; i < a->num_pixels; i++)
			{
				a->av_sum[i] = a->av_backmult * a->av_sum[i] + onem_avb * a->t_pixels[i];
				(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(10.0 * mlog10(a->scale * a->cd[i] * a->av_sum[i] + 1e-60));
			}
			break;
		}
	case 2:		// weighted averaging of log data
		{
			double onem_avb = 1.0 - a->av_backmult;
			for (i = 0; i < a->num_pixels; i++)
			{
				a->av_sum[i] = a->av_backmult * a->av_sum[i] + onem_avb * (10.0 * mlog10(a->scale * a->cd[i] * a->t_pixels[i] + 1e-60));
				(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)a->av_sum[i];
			}
			break;
		}
	case 3:		// window averaging of linear data
		{
			if (a->avail_frames < a->num_average)
			{
				factor = a->scale / (double)++a->avail_frames;
				for (i = 0; i < a->num_pixels; i++)
				{
					a->av_sum[i] += a->t_pixels[i];
					(a->av_buff[a->av_in_idx])[i] = a->t_pixels[i];
					(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(10.0 * mlog10(a->cd[i] * a->av_sum[i] * factor + 1e-60));
				}
			}
			else
			{
				factor = a->scale / (double)a->avail_frames;
				for (i = 0; i < a->num_pixels; i++)
				{
					a->av_sum[i] += a->t_pixels[i] - (a->av_buff[a->av_out_idx])[i];
					(a->av_buff[a->av_in_idx])[i] = a->t_pixels[i];
					(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(10.0 * mlog10(a->cd[i] * a->av_sum[i] * factor + 1e-60));
				}
				if (++a->av_out_idx == dMAX_AVERAGE)
						a->av_out_idx = 0;
			}
			if (++a->av_in_idx == dMAX_AVERAGE)
				a->av_in_idx = 0;
			break;
		}
	case 4:		// window averaging of log data
		{
			if (a->avail_frames < a->num_average)
			{
				factor = 1.0 / (double)++a->avail_frames;
				for (i = 0; i < a->num_pixels; i++)
				{
					(a->av_buff[a->av_in_idx])[i] = 10.0 * mlog10(a->cd[i] * a->t_pixels[i] * a->scale + 1e-60);
					a->av_sum[i] += (a->av_buff[a->av_in_idx])[i];
					(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(a->av_sum[i] * factor);
				}
			}
			else
			{
				factor = 1.0 / (double)a->avail_frames;
				for (i = 0; i < a->num_pixels; i++)
				{
					(a->av_buff[a->av_in_idx])[i] = 10.0 * mlog10(a->cd[i] * a->t_pixels[i] * a->scale + 1e-60);
					a->av_sum[i] += (a->av_buff[a->av_in_idx])[i] - (a->av_buff[a->av_out_idx])[i];
					(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(a->av_sum[i] * factor);
				}
				if (++a->av_out_idx == dMAX_AVERAGE)
						a->av_out_idx = 0;
			}
			if (++a->av_in_idx == dMAX_AVERAGE)
				a->av_in_idx = 0;
			break;
		}
	case 5:
		{
			for (i = 0; i < a->num_pixels; i++)
				(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(a->t_pixels[i] + 10.0 * mlog10(a->cd[i]));
			break;
		}
	case 6:
		{
			for (i = 0; i < a->num_pixels; i++)
				(a->pixels[a->w_pix_buff])[i] = (dOUTREAL)(a->t_pixels[i] + 10.0 * mlog10(a->cd[i]));
			break;
		}
	}
	LeaveCriticalSection(&a->ResampleSection);

	EnterCriticalSection(&a->PB_ControlsSection);
		a->last_pix_buff = a->w_pix_buff;	
		while ((a->w_pix_buff = (a->w_pix_buff + 1) % dNUM_PIXEL_BUFFS) == a->r_pix_buff);
	LeaveCriticalSection(&a->PB_ControlsSection);
	InterlockedBitTestAndSet(&(a->pb_ready[a->last_pix_buff]), 0);

	EnterCriticalSection(&a->NA_PBControlsSection);
		a->na_lastpixbuff = a->na_wpixbuff;
		while ((a->na_wpixbuff = (a->na_wpixbuff + 1) % dNUM_PIXEL_BUFFS) == a->na_rpixbuff);
	LeaveCriticalSection(&a->NA_PBControlsSection);
	InterlockedBitTestAndSet(&(a->na_pbready[a->na_lastpixbuff]), 0);
}

DWORD WINAPI spectra (void *pargs)
{
	int i, j;
	int disp = ((int)pargs) >> 12;
	int ss = (((int)pargs) >> 4) & 255;
	int LO = ((int)pargs) & 15;
	DP a = pdisp[disp];

	if (a->stop)
	{
		InterlockedDecrement(a->pnum_threads);
		return 0;
	}

	if ((ss >= a->begin_ss) && (ss <= a->end_ss))
	{
		for (i = 0; i < a->size; i++)
		{
			(a->fft_in[ss][LO])[i] = a->window[i] * (double)((a->I_samples[ss][LO])[a->IQO_idx[ss][LO]]);
			if(++a->IQO_idx[ss][LO] >= a->bsize)
				 a->IQO_idx[ss][LO] -= a->bsize;
		}

		if (a->stop)
		{
			InterlockedDecrement(a->pnum_threads);
			return 0;
		}
		fftw_execute (a->plan[ss][LO]);
	}
	if (a->stop)
	{
		InterlockedDecrement(a->pnum_threads);
		return 0;
	}

	EnterCriticalSection(&(a->EliminateSection[ss]));
	if ((ss >= a->begin_ss) && (ss <= a->end_ss))
		eliminate(disp, ss, LO);
	a->spec_flag[ss] |= 1 << LO;

	if (a->spec_flag[ss] == ((1 << a->num_fft) - 1))
	{
		a->spec_flag[ss] = 0;
		LeaveCriticalSection (&(a->EliminateSection[ss]));

		EnterCriticalSection (&a->StitchSection);
		a->stitch_flag |= 1i64 << ss;

		if (a->stitch_flag == ((1i64 << a->num_stitch) - 1))
		{
			a->stitch_flag = 0;
			LeaveCriticalSection(&a->StitchSection);
			for (j = 0; j < dMAX_STITCH; j++)
				for (i = 0; i < dMAX_NUM_FFT; i++)
					InterlockedBitTestAndReset(&(a->input_busy[j][i]), 0);
			stitch(disp);
		}
		else
			LeaveCriticalSection(&a->StitchSection);
	}
	else
		LeaveCriticalSection (&(a->EliminateSection[ss]));

		InterlockedDecrement(a->pnum_threads);
		return 1;
}

DWORD WINAPI Cspectra (void *pargs)
{
	int i, j;
	int disp = ((int)pargs) >> 12;
	int ss = (((int)pargs) >> 4) & 255;
	int LO = ((int)pargs) & 15;
	DP a = pdisp[disp];
	int trans_size = a->size * sizeof(double);

	if (a->stop)
	{
		InterlockedDecrement(a->pnum_threads);
		return 0;
	}

	if ((ss >= a->begin_ss) && (ss <= a->end_ss))
	{
		for (i = 0; i < a->size; i++)
		{
			(a->Cfft_in[ss][LO])[i][0] = a->window[i] * (double)((a->I_samples[ss][LO])[a->IQO_idx[ss][LO]]);
			(a->Cfft_in[ss][LO])[i][1] = a->window[i] * (double)((a->Q_samples[ss][LO])[a->IQO_idx[ss][LO]]);
			if(++a->IQO_idx[ss][LO] >= a->bsize)
				 a->IQO_idx[ss][LO] -= a->bsize;
		}

		if (a->stop)
		{
			InterlockedDecrement(a->pnum_threads);
			return 0;
		}
		fftw_execute (a->Cplan[ss][LO]);
	}
	if (a->stop)
	{
		InterlockedDecrement(a->pnum_threads);
		return 0;
	}

	if (InterlockedBitTestAndReset(&(a->snap[ss][LO]), 0))
	{
		memcpy((char *)(a->snap_buff[ss][LO]), (char *)(a->fft_out[ss][LO]) + trans_size, trans_size);
		memcpy((char *)(a->snap_buff[ss][LO]) + trans_size, (char *)(a->fft_out[ss][LO]), trans_size);
		SetEvent(a->hSnapEvent[ss][LO]);
	}

	EnterCriticalSection(&(a->EliminateSection[ss]));
	if ((ss >= a->begin_ss) && (ss <= a->end_ss))
		Celiminate(disp, ss, LO);
	a->spec_flag[ss] |= 1 << LO;

	if (a->spec_flag[ss] == ((1 << a->num_fft) - 1))
	{
		a->spec_flag[ss] = 0;
		LeaveCriticalSection (&(a->EliminateSection[ss]));

		EnterCriticalSection (&a->StitchSection);
		a->stitch_flag |= 1i64 << ss;

		if (a->stitch_flag == ((1i64 << a->num_stitch) - 1))
		{
			a->stitch_flag = 0;
			LeaveCriticalSection(&a->StitchSection);
			for (j = 0; j < dMAX_STITCH; j++)
				for (i = 0; i < dMAX_NUM_FFT; i++)
					InterlockedBitTestAndReset(&(a->input_busy[j][i]), 0);
			stitch(disp);
		}
		else
			LeaveCriticalSection(&a->StitchSection);
	}
	else
		LeaveCriticalSection (&(a->EliminateSection[ss]));

		InterlockedDecrement(a->pnum_threads);
		return 1;
}

void interpolate(int disp, int set, double fmin, double fmax, int num_pixels)
{
	DP a = pdisp[disp];
	int i; 
	double f;
	int n = a->n_freqs[set];
	int k;
	int kmin = 0;
	int kmax = n - 1;
	int kdelta;
	double dx;
	double mag;

	for (i = 0; i < num_pixels; i++)
    {
		f = fmin + (double)i * (fmax - fmin) / (double)(num_pixels - 1);
		
		if (f < (a->freqs[set])[0])
            k = 0;
        else if (f > (a->freqs[set])[n - 1])
            k = n - 2;
        else
		{
			kdelta = 1;

			while (f < (a->freqs[set])[kmin])
			{
				kmin = max(0, kmin - kdelta);
				kdelta += kdelta;
			}
			while (f > (a->freqs[set])[kmax])
			{
				kmax = min(n - 1, kmax + kdelta);
				kdelta += kdelta;
			}

			while ((kmax - kmin) > 1)
			{
				k = (kmin + kmax) / 2;
				if (f > (a->freqs[set])[k])
					kmin = k;
				else
					kmax = k--;
			}
		}

        dx = f - (a->freqs[set])[k];

        mag = (((a->ac3[set][0])[k] * dx + (a->ac2[set][0])[k]) * dx + (a->ac1[set][0])[k]) * dx + (a->ac0[set][0])[k];
		a->cd[i] = mag * mag;
	}
}

int build_interpolants(int disp, int set, int n, int m, double *x, double (*y)[dMAX_M])
{
	DP a = pdisp[disp];
	double dx[dMAX_N];
	double idx[dMAX_N];
	double dmain[dMAX_N];
	double dsub[dMAX_N];
	double dsup[dMAX_N];
	double d[dMAX_N][dMAX_M];
	double S[dMAX_N][dMAX_M];
	double b[dMAX_N];
	double v[dMAX_N][dMAX_M];
	double tmp;
	int i, j;

    for (i = 0; i < n - 1; i++)
    {
        dx[i] = x[i + 1] - x[i];

        if (dx[i] < 1e-30)
            return -1;
        idx[i] = 1.0 / dx[i];
    }

    for (i = 1; i <= n - 2; i++)
    {
        if (i == 1)
        {
            dsub[i] = 0.0;
            dmain[i] = 3.0 * dx[i - 1] + 2.0 * dx[i];
            dsup[i] = dx[i];
        }
        else if (i == (n - 2))
        {
            dsub[i] = dx[i - 1];
            dmain[i] = 2.0 * dx[i - 1] + 3.0 * dx[i];
            dsup[i] = 0.0;
        }
        else
        {
            dsub[i] = dx[i - 1];
            dmain[i] = 2.0 * (dx[i - 1] + dx[i]);
            dsup[i] = dx[i];
        }
		for (j = 0; j < m; j++)
            d[i][j] = 6.0 * ((y[i + 1][j] - y[i][j]) * idx[i] - (y[i][j] - y[i - 1][j]) * idx[i - 1]);
    }

    b[1] = dmain[1];
    for (j = 0; j < m; j++)
        v[1][j] = d[1][j];

    for (i = 2; i <= n - 2; i++)
    {
        tmp = dsub[i] / b[i - 1];
        b[i] = dmain[i] - tmp * dsup[i - 1];
        for (j = 0; j < m; j++)
            v[i][j] = d[i][j] - tmp * v[i - 1][j];
    }

    for (j = 0; j < m; j++)
        S[n - 2][j] = v[n - 2][j] / b[n - 2];

    for (i = n - 3; i >= 1; i--)
        for (j = 0; j < m; j++)
            S[i][j] = (v[i][j] - dsup[i] * S[i + 1][j]) / b[i];

    for (j = 0; j < m; j++)
    {
        S[0][j] = S[1][j];
        S[n - 1][j] = S[n - 2][j];
    }

    for (i = 0; i < n - 1; i++)
        for (j = 0; j < m; j++)
        {
            (a->ac3[set][j])[i] = (S[i + 1][j] - S[i][j]) / (6.0 * dx[i]);
            (a->ac2[set][j])[i] = 0.5 * S[i][j];
            (a->ac1[set][j])[i] = (y[i + 1][j] - y[i][j]) * idx[i] - (2.0 * dx[i] * S[i][j] + dx[i] * S[i + 1][j]) / 6.0;
            (a->ac0[set][j])[i] = y[i][j];
        }
    return 0;
}

void __cdecl sendbuf(void *arg)
{
	DP a = pdisp[(int)arg];
	while(!a->end_dispatcher)
	{
		for (a->ss = 0; a->ss < a->num_stitch; a->ss++)
			for (a->LO = 0; a->LO < a->num_fft; a->LO++)
			{
				if (!_InterlockedAnd(&(a->input_busy[a->ss][a->LO]), 1) && _InterlockedAnd(&(a->buff_ready[a->ss][a->LO]), 1))
				{
					InterlockedBitTestAndSet(&(a->input_busy[a->ss][a->LO]), 0);

					a->IQO_idx[a->ss][a->LO] = a->IQout_index[a->ss][a->LO];
					
					InterlockedIncrement(a->pnum_threads);
					if (a->type == 0)
						QueueUserWorkItem(spectra, (void *)(((int)arg << 12) + (a->ss << 4) + a->LO), 0);
					else
						QueueUserWorkItem(Cspectra, (void *)(((int)arg << 12) + (a->ss << 4) + a->LO), 0);

					if((a->IQout_index[a->ss][a->LO] += a->incr) >= a->bsize)
						a->IQout_index[a->ss][a->LO] -= a->bsize;

					EnterCriticalSection(&(a->BufferControlSection[a->ss][a->LO]));
					if ((a->have_samples[a->ss][a->LO] -= a->incr) < a->size)
						InterlockedBitTestAndReset(&(a->buff_ready[a->ss][a->LO]), 0);
					LeaveCriticalSection(&(a->BufferControlSection[a->ss][a->LO]));
				}
			}
			Sleep(1);
	}
	a->dispatcher = 0;
	_endthread();
}

PORT    
void SetAnalyzer (	int disp,
					int n_fft,			//number of LO frequencies = number of ffts used in elimination
					int typ,			//0 for real input data (I only); 1 for complex input data (I & Q)
					int *flp,			//vector with one elt for each LO frequency, 1 if high-side LO, 0 otherwise 
					int sz,				//size of the fft, i.e., number of input samples
					int bf_sz,			//number of samples transferred for each OpenBuffer()/CloseBuffer()
					int win_type,		//integer specifying which window function to use
					double pi,			//PiAlpha parameter for Kaiser window
					int ovrlp,			//number of samples each fft (other than the first) is to re-use from the previous 
					int clp,			//number of fft output bins to be clipped from EACH side of each sub-span
					int fscLin,			//number of bins to clip from low end of entire span
					int fscHin,			//number of bins to clip from high end of entire span
					int n_pix,			//number of pixel values to return.  may be either <= or > number of bins 
					int n_stch,			//number of sub-spans to concatenate to form a complete span 
					int av_m,			//averaging mode
					int n_av,			//number of spans to (moving) average for pixel result 
					double av_b,		//back multiplier for weighted averaging
					int calset,			//identifier of which set of calibration data to use 
					double fmin,		//frequency at first pixel value 
					double fmax,		//frequency at last pixel value
					int max_w
				 )
{
	DP a = pdisp[disp];
	int i, j;

	EnterCriticalSection(&a->SetAnalyzerSection);
	a->end_dispatcher = 1;
	while (a->dispatcher)
		Sleep(1);
	a->stop = 1;
	while (_InterlockedAnd(a->pnum_threads, 1023))
		Sleep(1);

	a->num_fft = n_fft;
	a->type = typ;
	a->buff_size = bf_sz;
	for (i = 0; i < a->num_fft; i++)
		a->flip[i] = *(flp + i);
	a->overlap = ovrlp;
	a->clip = clp;
	a->fsclipL = fscLin;
	a->fsclipH = fscHin;
	a->num_stitch = n_stch;
	a->num_average = n_av;
	a->av_backmult = av_b;

	if (av_m != a->av_mode)
	{
		a->av_mode = av_m;
		switch (av_m)
		{
		case 1:
			{
				for (i = 0; i < dMAX_PIXELS; i++)
					a->av_sum[i] = 1.0e-12;
				break;
			}
		case 2:
			{
				for (i = 0; i < dMAX_PIXELS; i++)
					a->av_sum[i] = -120;
				break;
			}
		case 5:
			{
				for (i = 0; i < a->max_size * a->max_stitch; i++)
					a->pre_av_sum[i] = 1.0e-12;
				break;
			}
		case 6:
			{
				for (i = 0; i < a->max_size * a->max_stitch; i++)
					a->pre_av_sum[i] = -120.0;
				break;
			}
		default:
			{
				memset ((void *)a->av_sum, 0, sizeof(double) * dMAX_PIXELS);
				break;
			}
		}
	}
	else
	{
		switch (av_m)
		{
		case 0:
			{
				break;
			}
		case 1:
			{
				break;
			}
		case 2:
			{
				break;
			}
		case 5:
			{
				break;
			}
		case 6:
			{
				break;
			}
		default:
			{
				memset ((void *)a->av_sum, 0, sizeof(double) * dMAX_PIXELS);
				break;
			}
		}
	}

	if (sz != a->size)
	{
		for (i = 0; i < a->max_stitch; i++)
			for (j = 0; j < a->max_num_fft; j++)
			{
				if (a->plan[i][j])		fftw_destroy_plan (a->plan[i][j]);
				if (a->Cplan[i][j])		fftw_destroy_plan (a->Cplan[i][j]);
				a->plan[i][j] = fftw_plan_dft_r2c_1d(sz, a->fft_in[i][j], a->fft_out[i][j], FFTW_PATIENT);
				a->Cplan[i][j] = fftw_plan_dft_1d(sz, a->Cfft_in[i][j], a->fft_out[i][j], FFTW_FORWARD, FFTW_PATIENT);
			}
	}

	if ((sz != a->size) || (win_type != a->window_type) || (pi != a->PiAlpha))
		new_window(disp, win_type, sz, pi);

	a->size = sz;
	a->window_type = win_type;
	a->PiAlpha = pi;
	a->max_writeahead = max_w;

	if (((fmin != a->f_min) || (fmax != a->f_max)) && ((fmin == 0.0) && (fmax == 0.0)))
		for (i = 0; i < dMAX_PIXELS; i++)
			a->cd[i] = 1.0;

	if (((fmax != 0.0) || (fmin != 0.0)) && ((n_pix != a->num_pixels) || (fmin != a->f_min) || (fmax != a->f_max) || (calset != a->cal_set) || a->cal_changed))
		interpolate(disp, calset, fmin, fmax, n_pix);

	a->incr = a->size - a->overlap;
	a->num_pixels = n_pix;
	a->f_min = fmin;
	a->f_max = fmax;
	a->cal_set = calset;
	a->cal_changed = 0;

	if (a->type == 0)
	{
		a->out_size = a->size / 2 + 1;
		a->scale = 4.0 / ((double)a->size * (double)a->size);
	}
	else
	{
		a->out_size = a->size;
		a->scale = 1.0 / ((double)a->size * (double)a->size);
	}

	a->begin_ss = 0;
	a->end_ss = a->num_stitch - 1;
	a->fscL = a->fsclipL;
	a->fscH = a->fsclipH;
	while (a->fscL >= (a->out_size - 2 * a->clip))
	{
		a->fscL -= a->out_size - 2 * a->clip;
		a->ss_bins[a->begin_ss] = 0;
		a->begin_ss++;
	}
	while (a->fscH >= (a->out_size - 2 * a->clip))
	{
		a->fscH -= a->out_size - 2 * a->clip;
		a->ss_bins[a->end_ss] = 0;
		a->end_ss--;
	}

	a->pix_per_bin = (double)a->num_pixels / (double)(a->num_stitch * (a->out_size - 2 * a->clip) - a->fsclipL - a->fsclipH);
	a->bin_per_pix = (double)(a->num_stitch * (a->out_size - 2 * a->clip) - 1 - a->fsclipL - a->fsclipH) / (double)a->num_pixels;

	for (i = 0; i < dMAX_STITCH; i++)
		for (j = 0; j < dMAX_NUM_FFT; j++)
			a->input_busy[i][j] = 0;

	for (i = 0; i < dMAX_STITCH; i++)
		a->spec_flag[i] = 0;
	a->avail_frames = 0;
	a->stitch_flag = 0;
	a->av_in_idx = 0;
	a->av_out_idx = 0;
	a->w_pix_buff = 0;
	a->r_pix_buff = 0;
	a->last_pix_buff = 0;
	a->na_wpixbuff = 0;
	a->na_rpixbuff = 0;
	a->na_lastpixbuff = 0;
	for (i = 0; i < dNUM_PIXEL_BUFFS; i++)
	{
		a->pb_ready[i] = 0;
		a->na_pbready[i] = 0;
	}

	a->ss = 0;
	a->LO = 0;
	for (i = 0; i < dMAX_STITCH; i++)
		for (j = 0; j < dMAX_NUM_FFT; j++)
		{
			a->buff_ready[i][j] = 0;
			a->have_samples[i][j] = 0;
			a->IQin_index[i][j] = 0;
			a->IQout_index[i][j] = 0;
		}

	a->stop = 0;
	a->end_dispatcher = 0;
	LeaveCriticalSection(&a->SetAnalyzerSection);
}

PORT
void XCreateAnalyzer(	int disp,
						int *success,
						int m_size,
						int m_num_fft,
						int m_stitch,
						char *app_data_path
						)
{

	int i, j, flag;
	DP a = (DP) malloc0 (sizeof(dp));
	pdisp[disp] = a;
	*success = 0;
	flag = 0;

	a->max_size = m_size;
	a->max_num_fft = m_num_fft;
	a->max_stitch = m_stitch;
	
	a->pnum_threads = (LONG*) malloc0 (sizeof (LONG));

	for (i = 0; i < a->max_stitch; i++)
		for (j = 0; j < a->max_num_fft; j++)
		{
			a->hSnapEvent[i][j] = CreateEvent(NULL, FALSE, FALSE, TEXT("snap"));
			a->snap[i][j] = 0;
		}
	InitializeCriticalSectionAndSpinCount(&a->ResampleSection, 0);
	InitializeCriticalSectionAndSpinCount(&a->PB_ControlsSection, 0);
	InitializeCriticalSectionAndSpinCount(&a->NA_PBControlsSection, 0);
	InitializeCriticalSectionAndSpinCount(&a->SetAnalyzerSection, 0);
	InitializeCriticalSectionAndSpinCount(&a->StitchSection, 0);
	for (i = 0; i < dMAX_STITCH; i++)
	{
		InitializeCriticalSectionAndSpinCount(&(a->EliminateSection[i]), 0);
		for (j = 0; j < dMAX_NUM_FFT; j++)
			InitializeCriticalSectionAndSpinCount(&(a->BufferControlSection[i][j]), 0);
	}

	a->window = (double*) malloc0 (sizeof(double) * a->max_size);
	if (a->window == 0)
		*success += -1;
	for (i = 0; i < a->max_stitch; i++)
	{
		a->result[i] = (double*) malloc0 (sizeof(double) * a->max_size);
		if (a->result[i] == 0)
			flag = 1;
	}
	if (flag) *success += -2, flag = 0;
	for (i = 0; i < a->max_stitch; i++)
		for (j = 0; j < a->max_num_fft; j++)
		{
			a->plan[i][j] = 0;
			a->Cplan[i][j] = 0;
			a->fft_in[i][j]   = (double*) malloc0 (sizeof(double) * a->max_size);
			a->Cfft_in[i][j]  = (fftw_complex*) fftw_malloc(sizeof(fftw_complex) * a->max_size);
			a->fft_out[i][j]  = (fftw_complex*) fftw_malloc(sizeof(fftw_complex) * a->max_size);
			if ((a->fft_in[i][j] == 0) || (a->fft_out[i][j] == 0) || (a->Cfft_in[i][j] == 0))
				flag = 1;
		}
	if (flag) *success += -4, flag = 0;
	a->pre_av_sum = (double*) malloc0 (sizeof(double) * a->max_size * a->max_stitch);
	a->pre_av_out = (double*) malloc0 (sizeof(double) * a->max_size * a->max_stitch);
	a->na_out     = (double*) malloc0 (sizeof(double) * a->max_size * a->max_stitch);
	a->av_sum     = (double*) malloc0 (sizeof(double) * dMAX_PIXELS);
	if ((a->av_sum == 0) || (a->pre_av_sum == 0) || (a->pre_av_out == 0) || (a->na_out == 0))
		*success += -8;

	for (i = 0; i < dMAX_AVERAGE; i++)
	{
		a->av_buff[i] = (double*) malloc0 (sizeof(double) * dMAX_PIXELS);
		if (a->av_buff[i] == 0)
			flag = 1;
	}
	if (flag) *success += -16, flag = 0;
	for (i = 0; i < dNUM_PIXEL_BUFFS; i++)
	{
		a->pixels[i]   = (dOUTREAL*) malloc0 (sizeof(dOUTREAL) * dMAX_PIXELS);
		a->napixels[i] = (dOUTREAL*) malloc0 (sizeof(dOUTREAL) * dMAX_PIXELS);
		if (a->pixels[i] == 0 || a->napixels[i] == 0)
			flag = 1;
	}
	if (flag) *success += -32, flag = 0;
	a->cd = (double*) malloc0 (sizeof(double) * dMAX_PIXELS);
		if (a->cd == 0)
			*success += -64;
		else
			for (j = 0; j < dMAX_PIXELS; j++)
				a->cd[j] = 1.0;
	for (i = 0; i < dMAX_CAL_SETS; i++)
	{
		a->freqs[i] = (double*) malloc0 (sizeof(double) * dMAX_N);
		for (j = 0; j < dMAX_M; j++)
		{
			a->ac3[i][j] = (double*) malloc0 (sizeof(double) * dMAX_N);
			a->ac2[i][j] = (double*) malloc0 (sizeof(double) * dMAX_N);
			a->ac1[i][j] = (double*) malloc0 (sizeof(double) * dMAX_N);
			a->ac0[i][j] = (double*) malloc0 (sizeof(double) * dMAX_N);
			if((a->freqs[i] == 0) || (a->ac3[i][j] == 0) || (a->ac2[i][j] == 0) || (a->ac1[i][j] == 0) || (a->ac0[i][j] == 0))
				flag = 1;
		}
	}
	if (flag) *success += -128, flag = 0;
	a->t_pixels   = (double*) malloc0 (sizeof(double) * dMAX_PIXELS);
	a->na_tpixels = (double*) malloc0 (sizeof(double) * dMAX_PIXELS);
	if (a->t_pixels == 0 || a->na_tpixels == 0)
		*success += -256;

	a->size = -1;
	a->window_type = -1;
	a->num_pixels = -1;
	a->cal_set = -1;
	a->f_min = -1.0;
	a->f_max = -1.0;

	a->bsize = a->max_size * dSAMP_BUFF_MULT;
	for (i = 0; i < a->max_stitch; i++)
		for (j = 0; j < a->max_num_fft; j++)
		{
			a->I_samples[i][j] = (dINREAL*) malloc0 (sizeof(dINREAL) * a->bsize);
			a->Q_samples[i][j] = (dINREAL*) malloc0 (sizeof(dINREAL) * a->bsize);
			if((a->I_samples[i][j] == 0) || (a->Q_samples[i][j] == 0))
				flag = 1;
		}
	if (flag) *success += -512, flag = 0;
}

PORT   
void DestroyAnalyzer(int disp)
{
	DP a = pdisp[disp];
	int i, j;

	a->end_dispatcher = 1;
	while (a->dispatcher)
		Sleep(1);

	for (i = 0; i < a->max_stitch; i++)
		for (j = 0; j < a->max_num_fft; j++)
		{
			_aligned_free  (a->I_samples[i][j]);
			_aligned_free  (a->Q_samples[i][j]);
		}
	_aligned_free  (a->na_tpixels);
	_aligned_free  (a->t_pixels);
	
	for (i = 0; i < dMAX_CAL_SETS; i++)
	{
		_aligned_free (a->freqs[i]);
		for (j = 0; j < dMAX_M; j++)
		{
			_aligned_free  (a->ac3[i][j]);
			_aligned_free  (a->ac2[i][j]);
			_aligned_free  (a->ac1[i][j]);
			_aligned_free  (a->ac0[i][j]);
		}
	}
	_aligned_free (a->cd);
	for (i = 0; i < dNUM_PIXEL_BUFFS; i++)
	{
		_aligned_free (a->pixels[i]);
		_aligned_free (a->napixels[i]);
	}
	for (i = 0; i < dMAX_AVERAGE; i++)
		_aligned_free (a->av_buff[i]);
	_aligned_free (a->av_sum);
	_aligned_free (a->pre_av_sum);
	_aligned_free (a->na_out);
	_aligned_free (a->pre_av_out);
	for (i = 0; i < a->max_stitch; i++)
		for (j = 0; j < a->max_num_fft; j++)
		{
			fftw_destroy_plan (a->plan[i][j]);
			fftw_destroy_plan (a->Cplan[i][j]);
			fftw_free (a->Cfft_in[i][j]);
			_aligned_free (a->fft_in[i][j]);
			fftw_free (a->fft_out[i][j]);
		}
	_aligned_free (a->window);
	for (i = 0; i < a->max_stitch; i++)
		_aligned_free (a->result[i]);

	DeleteCriticalSection(&a->StitchSection);
	for (i = 0; i < dMAX_STITCH; i++)
	{
		DeleteCriticalSection(&(a->EliminateSection[i]));
		for (j = 0; j < dMAX_NUM_FFT; j++)
			DeleteCriticalSection(&(a->BufferControlSection[i][j]));
	}

	DeleteCriticalSection(&a->PB_ControlsSection);
	DeleteCriticalSection(&a->SetAnalyzerSection);
	DeleteCriticalSection(&a->ResampleSection);

	for (i = 0; i < a->max_stitch; i++)
		for (j = 0; j < a->max_num_fft; j++)
			CloseHandle(a->hSnapEvent[i][j]);

	_aligned_free ((void *) a->pnum_threads);

	_aligned_free (a);
}

PORT   
void GetPixels	(	int disp,
					dOUTREAL *pix,		//if new pixel values avail, copies to pix and sets flag = 1
					int *flag			//else, returns 0 (try again later)
				)
{
	DP a = pdisp[disp];
	EnterCriticalSection(&a->PB_ControlsSection);
		a->r_pix_buff = a->last_pix_buff;
	LeaveCriticalSection(&a->PB_ControlsSection);

	if (_InterlockedAnd(&(a->pb_ready[a->r_pix_buff]), 1))
	{
		memcpy (pix, a->pixels[a->r_pix_buff], a->num_pixels * sizeof(dOUTREAL));
		*flag = 1;
		InterlockedBitTestAndReset(&(a->pb_ready[a->r_pix_buff]), 0);
	}
	else
		*flag = 0;
}

PORT   
void GetNAPixels	(	int disp,
						dOUTREAL *pix,		//if new pixel values avail, copies to pix and sets flag = 1
						int *flag			//else, returns 0 (try again later)
					)
{
	DP a = pdisp[disp];
	EnterCriticalSection(&a->NA_PBControlsSection);
		a->na_rpixbuff = a->na_lastpixbuff;
	LeaveCriticalSection(&a->NA_PBControlsSection);

	if (_InterlockedAnd(&(a->na_pbready[a->na_rpixbuff]), 1))
	{
		memcpy (pix, a->napixels[a->na_rpixbuff], a->num_pixels * sizeof(dOUTREAL));
		*flag = 1;
		InterlockedBitTestAndReset(&(a->na_pbready[a->na_rpixbuff]), 0);
	}
	else
		*flag = 0;
}

PORT
void SnapSpectrum(	int disp,
					int ss,
					int LO,
					double *snap_buff)
{
	DP a = pdisp[disp];
	a->snap_buff[ss][LO] = snap_buff;
	InterlockedBitTestAndSet(&(a->snap[ss][LO]), 0);
	WaitForSingleObject(a->hSnapEvent[ss][LO], INFINITE);
}

int calcompare (const void * a, const void * b)
{
	if (*(double*)a < *(double*)b)
		return -1;
	else if (*(double*)a == *(double*)b)
		return 0;
	else
		return 1;
}

PORT   
void SetCalibration (	int disp,
						int set_num,				//identifier for this calibration data set
						int n_points,				//number of calibration points in the set
						double (*cal)[dMAX_M+1]		//pointer to the calibration table, first
					)								//   column is frequency, add'l columns are
													//	 data for variables being calibrated
{
	DP a = pdisp[disp];
	int i, j;
	int k = 0;
	double y [dMAX_N][dMAX_M];

	qsort (cal, n_points, (dMAX_M+1) * sizeof(double), calcompare);

	for (i = 0; i < n_points; i++)
	{
		if ((i == n_points - 1) || (cal[i][0] != cal[i + 1][0]))
		{
			(a->freqs[set_num])[k] = cal[i][0];
			for (j = 0; j < dMAX_M; j++)
				y[k][j] = cal[i][j + 1];
			k++;
		}
	}

	a->n_freqs[set_num] = k;

	build_interpolants(disp, set_num, a->n_freqs[set_num], dMAX_M, a->freqs[set_num], y);
	a->cal_changed = 1;
}

PORT   
void OpenBuffer(int disp, int ss, int LO, void **Ipointer, void **Qpointer)
{
	DP a = pdisp[disp];
	EnterCriticalSection(&a->SetAnalyzerSection);
	*Ipointer = &((a->I_samples[ss][LO])[a->IQin_index[ss][LO]]);
	*Qpointer = &((a->Q_samples[ss][LO])[a->IQin_index[ss][LO]]);
	LeaveCriticalSection(&a->SetAnalyzerSection);
}

PORT   
void CloseBuffer(int disp, int ss, int LO)
{
	DP a = pdisp[disp];
	EnterCriticalSection(&a->SetAnalyzerSection);
	EnterCriticalSection(&(a->BufferControlSection[ss][LO]));
		if (a->have_samples[ss][LO] > a->max_writeahead)
			{
				//if we're receiving samples too much faster than we're consuming them, skip some
				if ((a->IQout_index[ss][LO] += a->have_samples[ss][LO] - a->max_writeahead) >= a->bsize)
						a->IQout_index[ss][LO] -= a->bsize;
				a->have_samples[ss][LO] = a->max_writeahead;
			}
		if ((a->have_samples[ss][LO] += a->buff_size) >= a->size)
			InterlockedBitTestAndSet(&(a->buff_ready[ss][LO]), 0);
	LeaveCriticalSection(&(a->BufferControlSection[ss][LO]));
	if((a->IQin_index[ss][LO] += a->buff_size) >= a->bsize)	//REQUIRES buff_size IS A SUB-MULTIPLE OF SIZE OF INPUT SAMPLE BUFFS!
		a->IQin_index[ss][LO] = 0;

	if (!a->dispatcher)
	{
		a->dispatcher = 1;
		LeaveCriticalSection(&a->SetAnalyzerSection);
		_beginthread(sendbuf, 0, (void *)disp);
	}
	else
		LeaveCriticalSection(&a->SetAnalyzerSection);
}

PORT
void Spectrum(int disp, int ss, int LO, dINREAL* pI, dINREAL* pQ)
{
	dINREAL *Ipointer;
	dINREAL *Qpointer;
	DP a = pdisp[disp];
	EnterCriticalSection(&a->SetAnalyzerSection);
	Ipointer = &((a->I_samples[ss][LO])[a->IQin_index[ss][LO]]);
	Qpointer = &((a->Q_samples[ss][LO])[a->IQin_index[ss][LO]]);
	LeaveCriticalSection(&a->SetAnalyzerSection);

	memcpy(Ipointer, pI, a->buff_size * sizeof(dINREAL));
	memcpy(Qpointer, pQ, a->buff_size * sizeof(dINREAL));

	EnterCriticalSection(&a->SetAnalyzerSection);
	EnterCriticalSection(&(a->BufferControlSection[ss][LO]));
		if (a->have_samples[ss][LO] > a->max_writeahead)
			{
				//if we're receiving samples too much faster than we're consuming them, skip some
				if ((a->IQout_index[ss][LO] += a->have_samples[ss][LO] - a->max_writeahead) >= a->bsize)
						a->IQout_index[ss][LO] -= a->bsize;
				a->have_samples[ss][LO] = a->max_writeahead;
			}
		if ((a->have_samples[ss][LO] += a->buff_size) >= a->size)
			InterlockedBitTestAndSet(&(a->buff_ready[ss][LO]), 0);
	LeaveCriticalSection(&(a->BufferControlSection[ss][LO]));
	if((a->IQin_index[ss][LO] += a->buff_size) >= a->bsize)	//REQUIRES buff_size IS A SUB-MULTIPLE OF SIZE OF INPUT SAMPLE BUFFS!
		a->IQin_index[ss][LO] = 0;

	if (!a->dispatcher)
	{
		a->dispatcher = 1;
		LeaveCriticalSection(&a->SetAnalyzerSection);
		_beginthread(sendbuf, 0, (void *)disp);
	}
	else
		LeaveCriticalSection(&a->SetAnalyzerSection);
}

PORT
void Spectrum2(int disp, int ss, int LO, dINREAL* pbuff)
{
	int i;
	dINREAL *Ipointer;
	dINREAL *Qpointer;
	DP a = pdisp[disp];
	EnterCriticalSection(&a->SetAnalyzerSection);
	Ipointer = &((a->I_samples[ss][LO])[a->IQin_index[ss][LO]]);
	Qpointer = &((a->Q_samples[ss][LO])[a->IQin_index[ss][LO]]);
	LeaveCriticalSection(&a->SetAnalyzerSection);

	for (i = 0; i < a->buff_size; i++)
	{
		Ipointer[i] = pbuff[2 * i];
		Qpointer[i] = pbuff[2 * i + 1];
	}

	EnterCriticalSection(&a->SetAnalyzerSection);
	EnterCriticalSection(&(a->BufferControlSection[ss][LO]));
		if (a->have_samples[ss][LO] > a->max_writeahead)
			{
				//if we're receiving samples too much faster than we're consuming them, skip some
				if ((a->IQout_index[ss][LO] += a->have_samples[ss][LO] - a->max_writeahead) >= a->bsize)
						a->IQout_index[ss][LO] -= a->bsize;
				a->have_samples[ss][LO] = a->max_writeahead;
			}
		if ((a->have_samples[ss][LO] += a->buff_size) >= a->size)
			InterlockedBitTestAndSet(&(a->buff_ready[ss][LO]), 0);
	LeaveCriticalSection(&(a->BufferControlSection[ss][LO]));
	if((a->IQin_index[ss][LO] += a->buff_size) >= a->bsize)	//REQUIRES buff_size IS A SUB-MULTIPLE OF SIZE OF INPUT SAMPLE BUFFS!
		a->IQin_index[ss][LO] = 0;

	if (!a->dispatcher)
	{
		a->dispatcher = 1;
		LeaveCriticalSection(&a->SetAnalyzerSection);
		_beginthread(sendbuf, 0, (void *)disp);
	}
	else
		LeaveCriticalSection(&a->SetAnalyzerSection);
}
