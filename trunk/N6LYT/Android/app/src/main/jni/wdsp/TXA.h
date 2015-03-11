/*  TXA.h

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

#ifndef _txa_h
#define _txa_h
#include "comm.h"

enum txaMode
{
	TXA_LSB,
	TXA_USB,
	TXA_DSB,
	TXA_CWL,
	TXA_CWU,
	TXA_FM,
	TXA_AM,
	TXA_DIGU,
	TXA_SPEC,
	TXA_DIGL,
	TXA_SAM,
	TXA_DRM
};

enum txaMeterType
{
	TXA_MIC_PK,
	TXA_MIC_AV,
	TXA_EQ_PK,
	TXA_EQ_AV,
	TXA_LVLR_PK,
	TXA_LVLR_AV,
	TXA_LVLR_GAIN,
	TXA_COMP_PK,
	TXA_COMP_AV,
	TXA_ALC_PK,
	TXA_ALC_AV,
	TXA_ALC_GAIN,
	TXA_OUT_PK,
	TXA_OUT_AV,
	TXA_METERTYPE_LAST
};

struct _txa
{
	double* inbuff;
	double* outbuff;
	double* midbuff;
	int mode;
	double meter[TXA_METERTYPE_LAST];
	CRITICAL_SECTION* pmtupdate[TXA_METERTYPE_LAST];
	struct
	{
		METER p;
	} micmeter, eqmeter, lvlrmeter, compmeter, alcmeter, outmeter;
	struct
	{
		RESAMPLE p;
	} rsmpin, rsmpout;
	struct
	{
		PANEL p;
	} panel;
	struct
	{
		AMSQ p;
	} amsq;
	struct
	{
		EQ p;
	} eq;
	struct
	{
		COMPRESSOR p;
	} compressor;
	struct
	{
		BANDPASS p;
	} bp0, bp1;
	struct
	{
		GAIN p;
	} pfgain;
	struct
	{
		WCPAGC p;
	} leveler, alc;
	struct
	{
		AMMOD p;
	} ammod;
	struct
	{
		EMPH p;
	} preemph;
	struct
	{
		FMMOD p;
	} fmmod;
	struct
	{
		SIPHON p;
	} sip1;
	struct
	{
		GEN p;
	} gen0, gen1;
	struct
	{
		USLEW p;
	} uslew;
	struct
	{
		CALCC p;
		CRITICAL_SECTION cs_update;
	} calcc;
	struct
	{
		IQC p0, p1;
		// p0 for dsp-synchronized reference, p1 for other
	} iqc;
} txa[MAX_CHANNELS];

extern void create_txa (int channel);

extern void destroy_txa (int channel);

extern void flush_txa (int channel);

extern void xtxa (int channel);

extern int TXAUslewCheck (int channel);

// TXA Properties

extern __declspec (dllexport) void SetTXAMode (int channel, int mode);

extern void TXAResCheck (int channel);

#endif