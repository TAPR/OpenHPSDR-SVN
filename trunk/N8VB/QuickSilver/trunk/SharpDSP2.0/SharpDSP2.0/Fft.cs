//==============================================================
// Copyright (C) 2006 Philip A Covington, N8VB
//
// This software is licensed under the GNU General public License
//==============================================================
// FFT.cs
// based on Ooura's FFT code - version with work area
// adapted to C#
//
// Managed Code Version
//
//==============================================================

#region Description

/*
Fast Fourier/Cosine/Sine Transform
    dimension   :one
    data length :power of 2
    decimation  :frequency
    radix       :4, 2
    data        :inplace
    table       :use
functions
    cdft: Complex Discrete Fourier Transform
    rdft: Real Discrete Fourier Transform
    ddct: Discrete Cosine Transform
    ddst: Discrete Sine Transform
    dfct: Cosine Transform of RDFT (Real Symmetric DFT)
    dfst: Sine Transform of RDFT (Real Anti-symmetric DFT)
function prototypes
    void cdft(int, int, double *, int *, double *);
    void rdft(int, int, double *, int *, double *);
    void ddct(int, int, double *, int *, double *);
    void ddst(int, int, double *, int *, double *);
    void dfct(int, double *, double *, int *, double *);
    void dfst(int, double *, double *, int *, double *);


-------- Complex DFT (Discrete Fourier Transform) --------
    [definition]
        <case1>
            X[k] = sum_j=0^n-1 x[j]*exp(2*pi*i*j*k/n), 0<=k<n
        <case2>
            X[k] = sum_j=0^n-1 x[j]*exp(-2*pi*i*j*k/n), 0<=k<n
        (notes: sum_j=0^n-1 is a summation from j=0 to n-1)
    [usage]
        <case1>
            ip[0] = 0; // first time only
            cdft(2*n, 1, a, ip, w);
        <case2>
            ip[0] = 0; // first time only
            cdft(2*n, -1, a, ip, w);
    [parameters]
        2*n            :data length (int)
                        n >= 1, n = power of 2
        a[0...2*n-1]   :input/output data (double *)
                        input data
                            a[2*j] = Re(x[j]), 
                            a[2*j+1] = Im(x[j]), 0<=j<n
                        output data
                            a[2*k] = Re(X[k]), 
                            a[2*k+1] = Im(X[k]), 0<=k<n
        ip[0...*]      :work area for bit reversal (int *)
                        length of ip >= 2+sqrt(n)
                        strictly, 
                        length of ip >= 
                            2+(1<<(int)(log(n+0.5)/log(2))/2).
                        ip[0],ip[1] are pointers of the cos/sin table.
        w[0...n/2-1]   :cos/sin table (double *)
                        w[],ip[] are initialized if ip[0] == 0.
    [remark]
        Inverse of 
            cdft(2*n, -1, a, ip, w);
        is 
            cdft(2*n, 1, a, ip, w);
            for (j = 0; j <= 2 * n - 1; j++) {
                a[j] *= 1.0 / n;
            }
        .


-------- Real DFT / Inverse of Real DFT --------
    [definition]
        <case1> RDFT
            R[k] = sum_j=0^n-1 a[j]*cos(2*pi*j*k/n), 0<=k<=n/2
            I[k] = sum_j=0^n-1 a[j]*sin(2*pi*j*k/n), 0<k<n/2
        <case2> IRDFT (excluding scale)
            a[k] = (R[0] + R[n/2]*cos(pi*k))/2 + 
                   sum_j=1^n/2-1 R[j]*cos(2*pi*j*k/n) + 
                   sum_j=1^n/2-1 I[j]*sin(2*pi*j*k/n), 0<=k<n
    [usage]
        <case1>
            ip[0] = 0; // first time only
            rdft(n, 1, a, ip, w);
        <case2>
            ip[0] = 0; // first time only
            rdft(n, -1, a, ip, w);
    [parameters]
        n              :data length (int)
                        n >= 2, n = power of 2
        a[0...n-1]     :input/output data (double *)
                        <case1>
                            output data
                                a[2*k] = R[k], 0<=k<n/2
                                a[2*k+1] = I[k], 0<k<n/2
                                a[1] = R[n/2]
                        <case2>
                            input data
                                a[2*j] = R[j], 0<=j<n/2
                                a[2*j+1] = I[j], 0<j<n/2
                                a[1] = R[n/2]
        ip[0...*]      :work area for bit reversal (int *)
                        length of ip >= 2+sqrt(n/2)
                        strictly, 
                        length of ip >= 
                            2+(1<<(int)(log(n/2+0.5)/log(2))/2).
                        ip[0],ip[1] are pointers of the cos/sin table.
        w[0...n/2-1]   :cos/sin table (double *)
                        w[],ip[] are initialized if ip[0] == 0.
    [remark]
        Inverse of 
            rdft(n, 1, a, ip, w);
        is 
            rdft(n, -1, a, ip, w);
            for (j = 0; j <= n - 1; j++) {
                a[j] *= 2.0 / n;
            }
        .


-------- DCT (Discrete Cosine Transform) / Inverse of DCT --------
    [definition]
        <case1> IDCT (excluding scale)
            C[k] = sum_j=0^n-1 a[j]*cos(pi*j*(k+1/2)/n), 0<=k<n
        <case2> DCT
            C[k] = sum_j=0^n-1 a[j]*cos(pi*(j+1/2)*k/n), 0<=k<n
    [usage]
        <case1>
            ip[0] = 0; // first time only
            ddct(n, 1, a, ip, w);
        <case2>
            ip[0] = 0; // first time only
            ddct(n, -1, a, ip, w);
    [parameters]
        n              :data length (int)
                        n >= 2, n = power of 2
        a[0...n-1]     :input/output data (double *)
                        output data
                            a[k] = C[k], 0<=k<n
        ip[0...*]      :work area for bit reversal (int *)
                        length of ip >= 2+sqrt(n/2)
                        strictly, 
                        length of ip >= 
                            2+(1<<(int)(log(n/2+0.5)/log(2))/2).
                        ip[0],ip[1] are pointers of the cos/sin table.
        w[0...n*5/4-1] :cos/sin table (double *)
                        w[],ip[] are initialized if ip[0] == 0.
    [remark]
        Inverse of 
            ddct(n, -1, a, ip, w);
        is 
            a[0] *= 0.5;
            ddct(n, 1, a, ip, w);
            for (j = 0; j <= n - 1; j++) {
                a[j] *= 2.0 / n;
            }
        .


-------- DST (Discrete Sine Transform) / Inverse of DST --------
    [definition]
        <case1> IDST (excluding scale)
            S[k] = sum_j=1^n A[j]*sin(pi*j*(k+1/2)/n), 0<=k<n
        <case2> DST
            S[k] = sum_j=0^n-1 a[j]*sin(pi*(j+1/2)*k/n), 0<k<=n
    [usage]
        <case1>
            ip[0] = 0; // first time only
            ddst(n, 1, a, ip, w);
        <case2>
            ip[0] = 0; // first time only
            ddst(n, -1, a, ip, w);
    [parameters]
        n              :data length (int)
                        n >= 2, n = power of 2
        a[0...n-1]     :input/output data (double *)
                        <case1>
                            input data
                                a[j] = A[j], 0<j<n
                                a[0] = A[n]
                            output data
                                a[k] = S[k], 0<=k<n
                        <case2>
                            output data
                                a[k] = S[k], 0<k<n
                                a[0] = S[n]
        ip[0...*]      :work area for bit reversal (int *)
                        length of ip >= 2+sqrt(n/2)
                        strictly, 
                        length of ip >= 
                            2+(1<<(int)(log(n/2+0.5)/log(2))/2).
                        ip[0],ip[1] are pointers of the cos/sin table.
        w[0...n*5/4-1] :cos/sin table (double *)
                        w[],ip[] are initialized if ip[0] == 0.
    [remark]
        Inverse of 
            ddst(n, -1, a, ip, w);
        is 
            a[0] *= 0.5;
            ddst(n, 1, a, ip, w);
            for (j = 0; j <= n - 1; j++) {
                a[j] *= 2.0 / n;
            }
        .


-------- Cosine Transform of RDFT (Real Symmetric DFT) --------
    [definition]
        C[k] = sum_j=0^n a[j]*cos(pi*j*k/n), 0<=k<=n
    [usage]
        ip[0] = 0; // first time only
        dfct(n, a, t, ip, w);
    [parameters]
        n              :data length - 1 (int)
                        n >= 2, n = power of 2
        a[0...n]       :input/output data (double *)
                        output data
                            a[k] = C[k], 0<=k<=n
        t[0...n/2]     :work area (double *)
        ip[0...*]      :work area for bit reversal (int *)
                        length of ip >= 2+sqrt(n/4)
                        strictly, 
                        length of ip >= 
                            2+(1<<(int)(log(n/4+0.5)/log(2))/2).
                        ip[0],ip[1] are pointers of the cos/sin table.
        w[0...n*5/8-1] :cos/sin table (double *)
                        w[],ip[] are initialized if ip[0] == 0.
    [remark]
        Inverse of 
            a[0] *= 0.5;
            a[n] *= 0.5;
            dfct(n, a, t, ip, w);
        is 
            a[0] *= 0.5;
            a[n] *= 0.5;
            dfct(n, a, t, ip, w);
            for (j = 0; j <= n; j++) {
                a[j] *= 2.0 / n;
            }
        .


-------- Sine Transform of RDFT (Real Anti-symmetric DFT) --------
    [definition]
        S[k] = sum_j=1^n-1 a[j]*sin(pi*j*k/n), 0<k<n
    [usage]
        ip[0] = 0; // first time only
        dfst(n, a, t, ip, w);
    [parameters]
        n              :data length + 1 (int)
                        n >= 2, n = power of 2
        a[0...n-1]     :input/output data (double *)
                        output data
                            a[k] = S[k], 0<k<n
                        (a[0] is used for work area)
        t[0...n/2-1]   :work area (double *)
        ip[0...*]      :work area for bit reversal (int *)
                        length of ip >= 2+sqrt(n/4)
                        strictly, 
                        length of ip >= 
                            2+(1<<(int)(log(n/4+0.5)/log(2))/2).
                        ip[0],ip[1] are pointers of the cos/sin table.
        w[0...n*5/8-1] :cos/sin table (double *)
                        w[],ip[] are initialized if ip[0] == 0.
    [remark]
        Inverse of 
            dfst(n, a, t, ip, w);
        is 
            dfst(n, a, t, ip, w);
            for (j = 1; j <= n - 1; j++) {
                a[j] *= 2.0 / n;
            }
        .


Appendix :
    The cos/sin table is recalculated when the larger table required.
    w[] and ip[] are compatible with all routines.
*/
#endregion

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct FFT
    {
        #region Private members

        private int ip0;
        private int[] ip_workarea;
        private double[] w_cossintable;
        private int size;

        #endregion

        #region Public Methods

        public FFT(int size)
        {
			this.size = size;
            this.ip0 = 0;
            this.ip_workarea = new int[(int)(Math.Sqrt(size) + 2)];
            this.w_cossintable = new double[size / 2];
			            
            MakeWorkTable(size / 2, ref this.ip_workarea, ref this.w_cossintable);		
        }

        public void DoComplexDFTForward(ref double[] real_in, ref double[] imag_in)
        {
            double[] temp_buffer = new double[this.size * 2];
            for (int i = 0; i < this.size; i++)
            {
                temp_buffer[2 * i] = real_in[i];
                temp_buffer[2 * i + 1] = imag_in[i];
            }

            ComplexDFT_Forward(2 * this.size, ref temp_buffer, ref this.ip_workarea, ref this.w_cossintable);
            			
            for (int i = 0; i < this.size; i++)
            {
                real_in[i] = temp_buffer[2 * i];
                imag_in[i] = temp_buffer[2 * i + 1];
            }

        }
        
        public void DoComplexDFTInverse(ref double[] real_in, ref double[] imag_in)
        {
            double[] temp_buffer = new double[this.size * 2];
            double normalize = (double)(this.size);

            for (int i = 0; i < this.size; i++)
            {
                temp_buffer[2 * i] = real_in[i];
                temp_buffer[2 * i + 1] = imag_in[i];
            }

            ComplexDFT_Reverse(2 * this.size, ref temp_buffer, ref this.ip_workarea, ref this.w_cossintable);
            			
            for (int i = 0; i < this.size; i++)
            {
                real_in[i] = temp_buffer[2 * i] / normalize;
                imag_in[i] = temp_buffer[2 * i + 1] / normalize;
            }
        }

        #endregion

        #region Private Methods

        private void ComplexDFT(int n, int isgn, ref double[] a, ref int[] ip, ref double[] w)
        {            
			if (isgn >= 0) 
			{
				bitrv2(n, ref ip, ref a);
				cftfsub(n, ref a, ref w);
			} 
			else 
			{
				bitrv2conj(n, ref ip, ref a);
				cftbsub(n, ref a, ref w);
			}
            
        }

        private void ComplexDFT_Forward(int n, ref double[] a, ref int[] ip, ref double[] w)
		{   
			bitrv2(n, ref ip, ref a);
			cftfsub(n, ref a, ref w);			           
		}

        private void ComplexDFT_Reverse(int n, ref double[] a, ref int[] ip, ref double[] w)
		{  
			bitrv2conj(n, ref ip, ref a);
			cftbsub(n, ref a, ref w);            
		}        

        /* -------- initializing routines -------- */

        private void MakeWorkTable(int nw, ref int[] ip, ref double[] w)
        {
            int j, nwh;
            double delta, x, y;
            
            ip0 = nw;
            if (nw > 2) {
                nwh = nw >> 1;
                delta = (double)(Math.Atan(1.0F) / nwh);
                w[0] = 1;
                w[1] = 0;
                w[nwh] = (double)(Math.Cos(delta * nwh));
                w[nwh + 1] = w[nwh];
                if (nwh > 2) {
                    for (j = 2; j < nwh; j += 2) {
                        x = (double)(Math.Cos(delta * j));
                        y = (double)(Math.Sin(delta * j));
                        w[j] = x;
                        w[j + 1] = y;
                        w[nw - j] = y;
                        w[nw - j + 1] = x;
                    }
                    bitrv2(nw, ref ip, ref w);
                }
            }
        }
        
        /* -------- child routines -------- */


        private void bitrv2(int n, ref int[] ip, ref double[] a)
        {
            int j, j1, k, k1, l, m, m2;
            double xr, xi, yr, yi;
            
            ip[0] = 0;
            l = n;
            m = 1;
            while ((m << 3) < l) {
                l >>= 1;
                for (j = 0; j < m; j++) {
                    ip[m + j] = ip[j] + l;
                }
                m <<= 1;
            }
            m2 = 2 * m;
            if ((m << 3) == l) {
                for (k = 0; k < m; k++) {
                    for (j = 0; j < k; j++) {
                        j1 = 2 * j + ip[k];
                        k1 = 2 * k + ip[j];
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += 2 * m2;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 -= m2;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += 2 * m2;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                    }
                    j1 = 2 * k + m2 + ip[k];
                    k1 = j1 + m2;
                    xr = a[j1];
                    xi = a[j1 + 1];
                    yr = a[k1];
                    yi = a[k1 + 1];
                    a[j1] = yr;
                    a[j1 + 1] = yi;
                    a[k1] = xr;
                    a[k1 + 1] = xi;
                }
            } else {
                for (k = 1; k < m; k++) {
                    for (j = 0; j < k; j++) {
                        j1 = 2 * j + ip[k];
                        k1 = 2 * k + ip[j];
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += m2;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                    }
                }
            }
        }


        private void bitrv2conj(int n, ref int[] ip, ref double[] a)
        {
            int j, j1, k, k1, l, m, m2;
            double xr, xi, yr, yi;
            
            ip[0] = 0;
            l = n;
            m = 1;
            while ((m << 3) < l) {
                l >>= 1;
                for (j = 0; j < m; j++) {
                    ip[m + j] = ip[j] + l;
                }
                m <<= 1;
            }
            m2 = 2 * m;
            if ((m << 3) == l) {
                for (k = 0; k < m; k++) {
                    for (j = 0; j < k; j++) {
                        j1 = 2 * j + ip[k];
                        k1 = 2 * k + ip[j];
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += 2 * m2;
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 -= m2;
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += 2 * m2;
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                    }
                    k1 = 2 * k + ip[k];
                    a[k1 + 1] = -a[k1 + 1];
                    j1 = k1 + m2;
                    k1 = j1 + m2;
                    xr = a[j1];
                    xi = -a[j1 + 1];
                    yr = a[k1];
                    yi = -a[k1 + 1];
                    a[j1] = yr;
                    a[j1 + 1] = yi;
                    a[k1] = xr;
                    a[k1 + 1] = xi;
                    k1 += m2;
                    a[k1 + 1] = -a[k1 + 1];
                }
            } else {
                a[1] = -a[1];
                a[m2 + 1] = -a[m2 + 1];
                for (k = 1; k < m; k++) {
                    for (j = 0; j < k; j++) {
                        j1 = 2 * j + ip[k];
                        k1 = 2 * k + ip[j];
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += m2;
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                    }
                    k1 = 2 * k + ip[k];
                    a[k1 + 1] = -a[k1 + 1];
                    a[k1 + m2 + 1] = -a[k1 + m2 + 1];
                }
            }
        }


        private void cftfsub(int n, ref double[] a, ref double[] w)
        {
            int j, j1, j2, j3, l;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
            
            l = 2;
            if (n > 8) {
                cft1st(n, ref a, ref w);
                l = 8;
                while ((l << 2) < n) {
                    cftmdl(n, l, ref a, ref w);
                    l <<= 2;
                }
            }
            if ((l << 2) == n) {
                for (j = 0; j < l; j += 2) {
                    j1 = j + l;
                    j2 = j1 + l;
                    j3 = j2 + l;
                    x0r = a[j] + a[j1];
                    x0i = a[j + 1] + a[j1 + 1];
                    x1r = a[j] - a[j1];
                    x1i = a[j + 1] - a[j1 + 1];
                    x2r = a[j2] + a[j3];
                    x2i = a[j2 + 1] + a[j3 + 1];
                    x3r = a[j2] - a[j3];
                    x3i = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0r + x2r;
                    a[j + 1] = x0i + x2i;
                    a[j2] = x0r - x2r;
                    a[j2 + 1] = x0i - x2i;
                    a[j1] = x1r - x3i;
                    a[j1 + 1] = x1i + x3r;
                    a[j3] = x1r + x3i;
                    a[j3 + 1] = x1i - x3r;
                }
            } else {
                for (j = 0; j < l; j += 2) {
                    j1 = j + l;
                    x0r = a[j] - a[j1];
                    x0i = a[j + 1] - a[j1 + 1];
                    a[j] += a[j1];
                    a[j + 1] += a[j1 + 1];
                    a[j1] = x0r;
                    a[j1 + 1] = x0i;
                }
            }
        }


        private void cftbsub(int n, ref double[] a, ref double[] w)
        {
            int j, j1, j2, j3, l;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
            
            l = 2;
            if (n > 8) {
                cft1st(n, ref a, ref w);
                l = 8;
                while ((l << 2) < n) {
                    cftmdl(n, l, ref a, ref w);
                    l <<= 2;
                }
            }
            if ((l << 2) == n) {
                for (j = 0; j < l; j += 2) {
                    j1 = j + l;
                    j2 = j1 + l;
                    j3 = j2 + l;
                    x0r = a[j] + a[j1];
                    x0i = -a[j + 1] - a[j1 + 1];
                    x1r = a[j] - a[j1];
                    x1i = -a[j + 1] + a[j1 + 1];
                    x2r = a[j2] + a[j3];
                    x2i = a[j2 + 1] + a[j3 + 1];
                    x3r = a[j2] - a[j3];
                    x3i = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0r + x2r;
                    a[j + 1] = x0i - x2i;
                    a[j2] = x0r - x2r;
                    a[j2 + 1] = x0i + x2i;
                    a[j1] = x1r - x3i;
                    a[j1 + 1] = x1i - x3r;
                    a[j3] = x1r + x3i;
                    a[j3 + 1] = x1i + x3r;
                }
            } else {
                for (j = 0; j < l; j += 2) {
                    j1 = j + l;
                    x0r = a[j] - a[j1];
                    x0i = -a[j + 1] + a[j1 + 1];
                    a[j] += a[j1];
                    a[j + 1] = -a[j + 1] - a[j1 + 1];
                    a[j1] = x0r;
                    a[j1 + 1] = x0i;
                }
            }
        }


        private void cft1st(int n, ref double[] a, ref double[] w)
        {
            int j, k1, k2;
            double wk1r, wk1i, wk2r, wk2i, wk3r, wk3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
            
            x0r = a[0] + a[2];
            x0i = a[1] + a[3];
            x1r = a[0] - a[2];
            x1i = a[1] - a[3];
            x2r = a[4] + a[6];
            x2i = a[5] + a[7];
            x3r = a[4] - a[6];
            x3i = a[5] - a[7];
            a[0] = x0r + x2r;
            a[1] = x0i + x2i;
            a[4] = x0r - x2r;
            a[5] = x0i - x2i;
            a[2] = x1r - x3i;
            a[3] = x1i + x3r;
            a[6] = x1r + x3i;
            a[7] = x1i - x3r;
            wk1r = w[2];
            x0r = a[8] + a[10];
            x0i = a[9] + a[11];
            x1r = a[8] - a[10];
            x1i = a[9] - a[11];
            x2r = a[12] + a[14];
            x2i = a[13] + a[15];
            x3r = a[12] - a[14];
            x3i = a[13] - a[15];
            a[8] = x0r + x2r;
            a[9] = x0i + x2i;
            a[12] = x2i - x0i;
            a[13] = x0r - x2r;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[10] = wk1r * (x0r - x0i);
            a[11] = wk1r * (x0r + x0i);
            x0r = x3i + x1r;
            x0i = x3r - x1i;
            a[14] = wk1r * (x0i - x0r);
            a[15] = wk1r * (x0i + x0r);
            k1 = 0;
            for (j = 16; j < n; j += 16) {
                k1 += 2;
                k2 = 2 * k1;
                wk2r = w[k1];
                wk2i = w[k1 + 1];
                wk1r = w[k2];
                wk1i = w[k2 + 1];
                wk3r = wk1r - 2 * wk2i * wk1i;
                wk3i = 2 * wk2i * wk1r - wk1i;
                x0r = a[j] + a[j + 2];
                x0i = a[j + 1] + a[j + 3];
                x1r = a[j] - a[j + 2];
                x1i = a[j + 1] - a[j + 3];
                x2r = a[j + 4] + a[j + 6];
                x2i = a[j + 5] + a[j + 7];
                x3r = a[j + 4] - a[j + 6];
                x3i = a[j + 5] - a[j + 7];
                a[j] = x0r + x2r;
                a[j + 1] = x0i + x2i;
                x0r -= x2r;
                x0i -= x2i;
                a[j + 4] = wk2r * x0r - wk2i * x0i;
                a[j + 5] = wk2r * x0i + wk2i * x0r;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[j + 2] = wk1r * x0r - wk1i * x0i;
                a[j + 3] = wk1r * x0i + wk1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[j + 6] = wk3r * x0r - wk3i * x0i;
                a[j + 7] = wk3r * x0i + wk3i * x0r;
                wk1r = w[k2 + 2];
                wk1i = w[k2 + 3];
                wk3r = wk1r - 2 * wk2r * wk1i;
                wk3i = 2 * wk2r * wk1r - wk1i;
                x0r = a[j + 8] + a[j + 10];
                x0i = a[j + 9] + a[j + 11];
                x1r = a[j + 8] - a[j + 10];
                x1i = a[j + 9] - a[j + 11];
                x2r = a[j + 12] + a[j + 14];
                x2i = a[j + 13] + a[j + 15];
                x3r = a[j + 12] - a[j + 14];
                x3i = a[j + 13] - a[j + 15];
                a[j + 8] = x0r + x2r;
                a[j + 9] = x0i + x2i;
                x0r -= x2r;
                x0i -= x2i;
                a[j + 12] = -wk2i * x0r - wk2r * x0i;
                a[j + 13] = -wk2i * x0i + wk2r * x0r;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[j + 10] = wk1r * x0r - wk1i * x0i;
                a[j + 11] = wk1r * x0i + wk1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[j + 14] = wk3r * x0r - wk3i * x0i;
                a[j + 15] = wk3r * x0i + wk3i * x0r;
            }
        }

        private void cftmdl(int n, int l, ref double[] a, ref double[] w)
        {
            int j, j1, j2, j3, k, k1, k2, m, m2;
            double wk1r, wk1i, wk2r, wk2i, wk3r, wk3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
            
            m = l << 2;
            for (j = 0; j < l; j += 2) {
                j1 = j + l;
                j2 = j1 + l;
                j3 = j2 + l;
                x0r = a[j] + a[j1];
                x0i = a[j + 1] + a[j1 + 1];
                x1r = a[j] - a[j1];
                x1i = a[j + 1] - a[j1 + 1];
                x2r = a[j2] + a[j3];
                x2i = a[j2 + 1] + a[j3 + 1];
                x3r = a[j2] - a[j3];
                x3i = a[j2 + 1] - a[j3 + 1];
                a[j] = x0r + x2r;
                a[j + 1] = x0i + x2i;
                a[j2] = x0r - x2r;
                a[j2 + 1] = x0i - x2i;
                a[j1] = x1r - x3i;
                a[j1 + 1] = x1i + x3r;
                a[j3] = x1r + x3i;
                a[j3 + 1] = x1i - x3r;
            }
            wk1r = w[2];
            for (j = m; j < l + m; j += 2) {
                j1 = j + l;
                j2 = j1 + l;
                j3 = j2 + l;
                x0r = a[j] + a[j1];
                x0i = a[j + 1] + a[j1 + 1];
                x1r = a[j] - a[j1];
                x1i = a[j + 1] - a[j1 + 1];
                x2r = a[j2] + a[j3];
                x2i = a[j2 + 1] + a[j3 + 1];
                x3r = a[j2] - a[j3];
                x3i = a[j2 + 1] - a[j3 + 1];
                a[j] = x0r + x2r;
                a[j + 1] = x0i + x2i;
                a[j2] = x2i - x0i;
                a[j2 + 1] = x0r - x2r;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[j1] = wk1r * (x0r - x0i);
                a[j1 + 1] = wk1r * (x0r + x0i);
                x0r = x3i + x1r;
                x0i = x3r - x1i;
                a[j3] = wk1r * (x0i - x0r);
                a[j3 + 1] = wk1r * (x0i + x0r);
            }
            k1 = 0;
            m2 = 2 * m;
            for (k = m2; k < n; k += m2) {
                k1 += 2;
                k2 = 2 * k1;
                wk2r = w[k1];
                wk2i = w[k1 + 1];
                wk1r = w[k2];
                wk1i = w[k2 + 1];
                wk3r = wk1r - 2 * wk2i * wk1i;
                wk3i = 2 * wk2i * wk1r - wk1i;
                for (j = k; j < l + k; j += 2) {
                    j1 = j + l;
                    j2 = j1 + l;
                    j3 = j2 + l;
                    x0r = a[j] + a[j1];
                    x0i = a[j + 1] + a[j1 + 1];
                    x1r = a[j] - a[j1];
                    x1i = a[j + 1] - a[j1 + 1];
                    x2r = a[j2] + a[j3];
                    x2i = a[j2 + 1] + a[j3 + 1];
                    x3r = a[j2] - a[j3];
                    x3i = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0r + x2r;
                    a[j + 1] = x0i + x2i;
                    x0r -= x2r;
                    x0i -= x2i;
                    a[j2] = wk2r * x0r - wk2i * x0i;
                    a[j2 + 1] = wk2r * x0i + wk2i * x0r;
                    x0r = x1r - x3i;
                    x0i = x1i + x3r;
                    a[j1] = wk1r * x0r - wk1i * x0i;
                    a[j1 + 1] = wk1r * x0i + wk1i * x0r;
                    x0r = x1r + x3i;
                    x0i = x1i - x3r;
                    a[j3] = wk3r * x0r - wk3i * x0i;
                    a[j3 + 1] = wk3r * x0i + wk3i * x0r;
                }
                wk1r = w[k2 + 2];
                wk1i = w[k2 + 3];
                wk3r = wk1r - 2 * wk2r * wk1i;
                wk3i = 2 * wk2r * wk1r - wk1i;
                for (j = k + m; j < l + (k + m); j += 2) {
                    j1 = j + l;
                    j2 = j1 + l;
                    j3 = j2 + l;
                    x0r = a[j] + a[j1];
                    x0i = a[j + 1] + a[j1 + 1];
                    x1r = a[j] - a[j1];
                    x1i = a[j + 1] - a[j1 + 1];
                    x2r = a[j2] + a[j3];
                    x2i = a[j2 + 1] + a[j3 + 1];
                    x3r = a[j2] - a[j3];
                    x3i = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0r + x2r;
                    a[j + 1] = x0i + x2i;
                    x0r -= x2r;
                    x0i -= x2i;
                    a[j2] = -wk2i * x0r - wk2r * x0i;
                    a[j2 + 1] = -wk2i * x0i + wk2r * x0r;
                    x0r = x1r - x3i;
                    x0i = x1i + x3r;
                    a[j1] = wk1r * x0r - wk1i * x0i;
                    a[j1 + 1] = wk1r * x0i + wk1i * x0r;
                    x0r = x1r + x3i;
                    x0i = x1i - x3r;
                    a[j3] = wk3r * x0r - wk3i * x0i;
                    a[j3 + 1] = wk3r * x0i + wk3i * x0r;
                }
            }
        }
       
        #endregion
    }
}
