/*
 * Copyright (c) 2003, 2007-11 Matteo Frigo
 * Copyright (c) 2003, 2007-11 Massachusetts Institute of Technology
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 *
 */

/* This file was automatically generated --- DO NOT EDIT */
/* Generated on Sun Nov 25 07:40:50 EST 2012 */

#include "codelet-rdft.h"

#ifdef HAVE_FMA

/* Generated by: ../../../genfft/gen_hc2cdft.native -fma -reorder-insns -schedule-for-pipeline -compact -variables 4 -pipeline-latency 4 -twiddle-log3 -precompute-twiddles -n 8 -dit -name hc2cfdft2_8 -include hc2cf.h */

/*
 * This function contains 90 FP additions, 66 FP multiplications,
 * (or, 60 additions, 36 multiplications, 30 fused multiply/add),
 * 68 stack variables, 2 constants, and 32 memory accesses
 */
#include "hc2cf.h"

static void hc2cfdft2_8(R *Rp, R *Ip, R *Rm, R *Im, const R *W, stride rs, INT mb, INT me, INT ms)
{
     DK(KP707106781, +0.707106781186547524400844362104849039284835938);
     DK(KP500000000, +0.500000000000000000000000000000000000000000000);
     {
	  INT m;
	  for (m = mb, W = W + ((mb - 1) * 6); m < me; m = m + 1, Rp = Rp + ms, Ip = Ip + ms, Rm = Rm - ms, Im = Im - ms, W = W + 6, MAKE_VOLATILE_STRIDE(32, rs)) {
	       E T1G, T1F, T1C, T1D, T1N, T1B, T1R, T1L;
	       {
		    E T1, T2, Th, Tj, T4, T3, Ti, Tp, T5;
		    T1 = W[0];
		    T2 = W[2];
		    Th = W[4];
		    Tj = W[5];
		    T4 = W[1];
		    T3 = T1 * T2;
		    Ti = T1 * Th;
		    Tp = T1 * Tj;
		    T5 = W[3];
		    {
			 E Tk, Tq, TI, T1a, T1u, TY, TF, TS, T1s, T1c, Tr, T1n, Tg, T16, Tn;
			 E T13, T1f, Ts, To, T1o;
			 {
			      E T6, Tw, Tc, TB, TQ, TM, TC, TR, Tz, TD, TA;
			      {
				   E TX, TV, TT, TU;
				   {
					E TG, Tb, TH, TP, TL;
					TG = Ip[0];
					Tk = FMA(T4, Tj, Ti);
					Tq = FNMS(T4, Th, Tp);
					T6 = FMA(T4, T5, T3);
					Tw = FNMS(T4, T5, T3);
					Tb = T1 * T5;
					TH = Im[0];
					TT = Rm[0];
					TP = T6 * Tj;
					TL = T6 * Th;
					Tc = FNMS(T4, T2, Tb);
					TB = FMA(T4, T2, Tb);
					TX = TG + TH;
					TI = TG - TH;
					TU = Rp[0];
					TQ = FNMS(Tc, Th, TP);
					TM = FMA(Tc, Tj, TL);
				   }
				   T1a = TU + TT;
				   TV = TT - TU;
				   {
					E Tx, Ty, T1t, TW;
					Tx = Ip[WS(rs, 2)];
					Ty = Im[WS(rs, 2)];
					T1t = T4 * TV;
					TW = T1 * TV;
					TC = Rp[WS(rs, 2)];
					TR = Tx + Ty;
					Tz = Tx - Ty;
					T1u = FMA(T1, TX, T1t);
					TY = FNMS(T4, TX, TW);
					TD = Rm[WS(rs, 2)];
				   }
				   TA = Tw * Tz;
			      }
			      {
				   E Td, T9, T12, Te, Ta, T1m;
				   {
					E T7, T8, TN, TE, TO, T1r, T1b;
					T7 = Ip[WS(rs, 1)];
					T8 = Im[WS(rs, 1)];
					TN = TD - TC;
					TE = TC + TD;
					Td = Rp[WS(rs, 1)];
					T9 = T7 - T8;
					T12 = T7 + T8;
					TO = TM * TN;
					T1r = TQ * TN;
					T1b = Tw * TE;
					TF = FNMS(TB, TE, TA);
					TS = FNMS(TQ, TR, TO);
					T1s = FMA(TM, TR, T1r);
					T1c = FMA(TB, Tz, T1b);
					Te = Rm[WS(rs, 1)];
				   }
				   Ta = T6 * T9;
				   T1m = T2 * T12;
				   {
					E Tl, T10, Tf, Tm, T11, T1e;
					Tl = Ip[WS(rs, 3)];
					T10 = Td - Te;
					Tf = Td + Te;
					Tm = Im[WS(rs, 3)];
					Tr = Rp[WS(rs, 3)];
					T11 = T2 * T10;
					T1n = FNMS(T5, T10, T1m);
					T1e = T6 * Tf;
					Tg = FNMS(Tc, Tf, Ta);
					T16 = Tl + Tm;
					Tn = Tl - Tm;
					T13 = FMA(T5, T12, T11);
					T1f = FMA(Tc, T9, T1e);
					Ts = Rm[WS(rs, 3)];
				   }
				   To = Tk * Tn;
				   T1o = Th * T16;
			      }
			 }
			 {
			      E T1z, T1K, T1y, T1k, T1J, T1A, T1x, T1j;
			      {
				   E T1w, TK, T1l, T19, T1d, T1i;
				   {
					E TJ, T14, Tt, T1v, T1h;
					T1z = TI - TF;
					TJ = TF + TI;
					T14 = Tr - Ts;
					Tt = Tr + Ts;
					T1v = T1s + T1u;
					T1G = T1u - T1s;
					{
					     E TZ, T1q, Tv, T18, T15;
					     T1F = TY - TS;
					     TZ = TS + TY;
					     T15 = Th * T14;
					     {
						  E T1p, T1g, Tu, T17;
						  T1p = FNMS(Tj, T14, T1o);
						  T1g = Tk * Tt;
						  Tu = FNMS(Tq, Tt, To);
						  T17 = FMA(Tj, T16, T15);
						  T1C = T1p - T1n;
						  T1q = T1n + T1p;
						  T1h = FMA(Tq, Tn, T1g);
						  T1K = Tg - Tu;
						  Tv = Tg + Tu;
						  T18 = T13 + T17;
						  T1D = T13 - T17;
					     }
					     T1w = T1q - T1v;
					     T1y = T1q + T1v;
					     TK = Tv + TJ;
					     T1l = TJ - Tv;
					     T1k = T18 + TZ;
					     T19 = TZ - T18;
					}
					T1J = T1a - T1c;
					T1d = T1a + T1c;
					T1i = T1f + T1h;
					T1A = T1f - T1h;
				   }
				   Ip[0] = KP500000000 * (TK + T19);
				   Im[WS(rs, 3)] = KP500000000 * (T19 - TK);
				   Im[WS(rs, 1)] = KP500000000 * (T1w - T1l);
				   T1x = T1d + T1i;
				   T1j = T1d - T1i;
				   Ip[WS(rs, 2)] = KP500000000 * (T1l + T1w);
			      }
			      Rm[WS(rs, 3)] = KP500000000 * (T1x - T1y);
			      Rp[0] = KP500000000 * (T1x + T1y);
			      Rp[WS(rs, 2)] = KP500000000 * (T1j + T1k);
			      Rm[WS(rs, 1)] = KP500000000 * (T1j - T1k);
			      T1N = T1A + T1z;
			      T1B = T1z - T1A;
			      T1R = T1J + T1K;
			      T1L = T1J - T1K;
			 }
		    }
	       }
	       {
		    E T1E, T1O, T1H, T1P;
		    T1E = T1C + T1D;
		    T1O = T1C - T1D;
		    T1H = T1F - T1G;
		    T1P = T1F + T1G;
		    {
			 E T1S, T1Q, T1I, T1M;
			 T1S = T1O + T1P;
			 T1Q = T1O - T1P;
			 T1I = T1E + T1H;
			 T1M = T1H - T1E;
			 Im[0] = -(KP500000000 * (FNMS(KP707106781, T1Q, T1N)));
			 Ip[WS(rs, 3)] = KP500000000 * (FMA(KP707106781, T1Q, T1N));
			 Rp[WS(rs, 1)] = KP500000000 * (FMA(KP707106781, T1S, T1R));
			 Rm[WS(rs, 2)] = KP500000000 * (FNMS(KP707106781, T1S, T1R));
			 Rp[WS(rs, 3)] = KP500000000 * (FMA(KP707106781, T1M, T1L));
			 Rm[0] = KP500000000 * (FNMS(KP707106781, T1M, T1L));
			 Im[WS(rs, 2)] = -(KP500000000 * (FNMS(KP707106781, T1I, T1B)));
			 Ip[WS(rs, 1)] = KP500000000 * (FMA(KP707106781, T1I, T1B));
		    }
	       }
	  }
     }
}

static const tw_instr twinstr[] = {
     {TW_CEXP, 1, 1},
     {TW_CEXP, 1, 3},
     {TW_CEXP, 1, 7},
     {TW_NEXT, 1, 0}
};

static const hc2c_desc desc = { 8, "hc2cfdft2_8", twinstr, &GENUS, {60, 36, 30, 0} };

void X(codelet_hc2cfdft2_8) (planner *p) {
     X(khc2c_register) (p, hc2cfdft2_8, &desc, HC2C_VIA_DFT);
}
#else				/* HAVE_FMA */

/* Generated by: ../../../genfft/gen_hc2cdft.native -compact -variables 4 -pipeline-latency 4 -twiddle-log3 -precompute-twiddles -n 8 -dit -name hc2cfdft2_8 -include hc2cf.h */

/*
 * This function contains 90 FP additions, 56 FP multiplications,
 * (or, 72 additions, 38 multiplications, 18 fused multiply/add),
 * 51 stack variables, 2 constants, and 32 memory accesses
 */
#include "hc2cf.h"

static void hc2cfdft2_8(R *Rp, R *Ip, R *Rm, R *Im, const R *W, stride rs, INT mb, INT me, INT ms)
{
     DK(KP353553390, +0.353553390593273762200422181052424519642417969);
     DK(KP500000000, +0.500000000000000000000000000000000000000000000);
     {
	  INT m;
	  for (m = mb, W = W + ((mb - 1) * 6); m < me; m = m + 1, Rp = Rp + ms, Ip = Ip + ms, Rm = Rm - ms, Im = Im - ms, W = W + 6, MAKE_VOLATILE_STRIDE(32, rs)) {
	       E T1, T4, T2, T5, Tu, Ty, T7, Td, Ti, Tj, Tk, TP, To, TN;
	       {
		    E T3, Tc, T6, Tb;
		    T1 = W[0];
		    T4 = W[1];
		    T2 = W[2];
		    T5 = W[3];
		    T3 = T1 * T2;
		    Tc = T4 * T2;
		    T6 = T4 * T5;
		    Tb = T1 * T5;
		    Tu = T3 - T6;
		    Ty = Tb + Tc;
		    T7 = T3 + T6;
		    Td = Tb - Tc;
		    Ti = W[4];
		    Tj = W[5];
		    Tk = FMA(T1, Ti, T4 * Tj);
		    TP = FNMS(Td, Ti, T7 * Tj);
		    To = FNMS(T4, Ti, T1 * Tj);
		    TN = FMA(T7, Ti, Td * Tj);
	       }
	       {
		    E TF, T11, TC, T12, T1d, T1e, T1q, TM, TR, T1p, Th, Ts, T15, T14, T1a;
		    E T1b, T1m, TV, TY, T1n;
		    {
			 E TD, TE, TL, TI, TJ, TK, Tx, TQ, TB, TO;
			 TD = Ip[0];
			 TE = Im[0];
			 TL = TD + TE;
			 TI = Rm[0];
			 TJ = Rp[0];
			 TK = TI - TJ;
			 {
			      E Tv, Tw, Tz, TA;
			      Tv = Ip[WS(rs, 2)];
			      Tw = Im[WS(rs, 2)];
			      Tx = Tv - Tw;
			      TQ = Tv + Tw;
			      Tz = Rp[WS(rs, 2)];
			      TA = Rm[WS(rs, 2)];
			      TB = Tz + TA;
			      TO = Tz - TA;
			 }
			 TF = TD - TE;
			 T11 = TJ + TI;
			 TC = FNMS(Ty, TB, Tu * Tx);
			 T12 = FMA(Tu, TB, Ty * Tx);
			 T1d = FNMS(TP, TO, TN * TQ);
			 T1e = FMA(T4, TK, T1 * TL);
			 T1q = T1e - T1d;
			 TM = FNMS(T4, TL, T1 * TK);
			 TR = FMA(TN, TO, TP * TQ);
			 T1p = TR + TM;
		    }
		    {
			 E Ta, TU, Tg, TT, Tn, TX, Tr, TW;
			 {
			      E T8, T9, Te, Tf;
			      T8 = Ip[WS(rs, 1)];
			      T9 = Im[WS(rs, 1)];
			      Ta = T8 - T9;
			      TU = T8 + T9;
			      Te = Rp[WS(rs, 1)];
			      Tf = Rm[WS(rs, 1)];
			      Tg = Te + Tf;
			      TT = Te - Tf;
			 }
			 {
			      E Tl, Tm, Tp, Tq;
			      Tl = Ip[WS(rs, 3)];
			      Tm = Im[WS(rs, 3)];
			      Tn = Tl - Tm;
			      TX = Tl + Tm;
			      Tp = Rp[WS(rs, 3)];
			      Tq = Rm[WS(rs, 3)];
			      Tr = Tp + Tq;
			      TW = Tp - Tq;
			 }
			 Th = FNMS(Td, Tg, T7 * Ta);
			 Ts = FNMS(To, Tr, Tk * Tn);
			 T15 = FMA(Tk, Tr, To * Tn);
			 T14 = FMA(T7, Tg, Td * Ta);
			 T1a = FNMS(T5, TT, T2 * TU);
			 T1b = FNMS(Tj, TW, Ti * TX);
			 T1m = T1b - T1a;
			 TV = FMA(T2, TT, T5 * TU);
			 TY = FMA(Ti, TW, Tj * TX);
			 T1n = TV - TY;
		    }
		    {
			 E T1l, T1x, T1A, T1C, T1s, T1w, T1v, T1B;
			 {
			      E T1j, T1k, T1y, T1z;
			      T1j = TF - TC;
			      T1k = T14 - T15;
			      T1l = KP500000000 * (T1j - T1k);
			      T1x = KP500000000 * (T1k + T1j);
			      T1y = T1m - T1n;
			      T1z = T1p + T1q;
			      T1A = KP353553390 * (T1y - T1z);
			      T1C = KP353553390 * (T1y + T1z);
			 }
			 {
			      E T1o, T1r, T1t, T1u;
			      T1o = T1m + T1n;
			      T1r = T1p - T1q;
			      T1s = KP353553390 * (T1o + T1r);
			      T1w = KP353553390 * (T1r - T1o);
			      T1t = T11 - T12;
			      T1u = Th - Ts;
			      T1v = KP500000000 * (T1t - T1u);
			      T1B = KP500000000 * (T1t + T1u);
			 }
			 Ip[WS(rs, 1)] = T1l + T1s;
			 Rp[WS(rs, 1)] = T1B + T1C;
			 Im[WS(rs, 2)] = T1s - T1l;
			 Rm[WS(rs, 2)] = T1B - T1C;
			 Rm[0] = T1v - T1w;
			 Im[0] = T1A - T1x;
			 Rp[WS(rs, 3)] = T1v + T1w;
			 Ip[WS(rs, 3)] = T1x + T1A;
		    }
		    {
			 E TH, T19, T1g, T1i, T10, T18, T17, T1h;
			 {
			      E Tt, TG, T1c, T1f;
			      Tt = Th + Ts;
			      TG = TC + TF;
			      TH = Tt + TG;
			      T19 = TG - Tt;
			      T1c = T1a + T1b;
			      T1f = T1d + T1e;
			      T1g = T1c - T1f;
			      T1i = T1c + T1f;
			 }
			 {
			      E TS, TZ, T13, T16;
			      TS = TM - TR;
			      TZ = TV + TY;
			      T10 = TS - TZ;
			      T18 = TZ + TS;
			      T13 = T11 + T12;
			      T16 = T14 + T15;
			      T17 = T13 - T16;
			      T1h = T13 + T16;
			 }
			 Ip[0] = KP500000000 * (TH + T10);
			 Rp[0] = KP500000000 * (T1h + T1i);
			 Im[WS(rs, 3)] = KP500000000 * (T10 - TH);
			 Rm[WS(rs, 3)] = KP500000000 * (T1h - T1i);
			 Rm[WS(rs, 1)] = KP500000000 * (T17 - T18);
			 Im[WS(rs, 1)] = KP500000000 * (T1g - T19);
			 Rp[WS(rs, 2)] = KP500000000 * (T17 + T18);
			 Ip[WS(rs, 2)] = KP500000000 * (T19 + T1g);
		    }
	       }
	  }
     }
}

static const tw_instr twinstr[] = {
     {TW_CEXP, 1, 1},
     {TW_CEXP, 1, 3},
     {TW_CEXP, 1, 7},
     {TW_NEXT, 1, 0}
};

static const hc2c_desc desc = { 8, "hc2cfdft2_8", twinstr, &GENUS, {72, 38, 18, 0} };

void X(codelet_hc2cfdft2_8) (planner *p) {
     X(khc2c_register) (p, hc2cfdft2_8, &desc, HC2C_VIA_DFT);
}
#endif				/* HAVE_FMA */
