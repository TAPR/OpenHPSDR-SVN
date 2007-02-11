//==============================================================
// Copyright (C) 2006 Philip A Covington, N8VB
//
// This software is licensed under the GNU General public License
//==============================================================
// PowerSpectrum.cs
//
//
//
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDSP
{
    public class PowerSpectrum
    {
        public static event PowerSpectrumEventHandler sharp_dsp_event;

        #region Private members
                
        private double[] window;
        private double[] ps_results;     //holds power spectrum results
        
        private FFT fft;
        private FilterDesigner filt_design;

        #endregion

        #region Properties

        private int fft_size;
        public int FFTSize
        {
            get { return fft_size; }
        }

        private int results_size;
        public int ResultsBufferSize
        {
            get { return results_size; }
        }

        #endregion

        #region Constructor

        public PowerSpectrum(int fft_size, WindowType wt)
        {
            this.fft_size = fft_size;
            this.results_size = fft_size;
            this.window = new double[fft_size / 2];
            this.ps_results = new double[fft_size]; //don't overrun results buffer
            this.fft = new FFT(fft_size);
            this.filt_design = new FilterDesigner();
            
            filt_design.makewindow(wt, fft_size / 2, ref window);            
        }

        #endregion

        public void PowerSpectrumSignal(ref double[] real, ref double[] imag)
        {
            double[] buffer_real = new double[this.fft_size];
            double[] buffer_imag = new double[this.fft_size];
            double[] sqrMag = new double[this.fft_size];
                        
            // window the data
            for (int i = 0; i < this.fft_size / 2; i++)
            {
                buffer_real[i] = real[i] * this.window[i];
                buffer_imag[i] = imag[i] * this.window[i];
            }

            this.fft.DoComplexDFTForward(ref buffer_real, ref buffer_imag);

            int tmp_size = this.fft_size / 2;
            for (int i = 0; i < tmp_size; i++)
            {
                sqrMag[i + tmp_size] = buffer_real[i + tmp_size] * buffer_real[i + tmp_size]
                    + buffer_imag[i + tmp_size] * buffer_imag[i + tmp_size];

                this.ps_results[i] =
                    (double)(10.0 * Math.Log10(sqrMag[i + tmp_size] + 1e-180));

                sqrMag[i] = buffer_real[i] * buffer_real[i]
                    + buffer_imag[i] * buffer_imag[i];

                this.ps_results[i + tmp_size] =
                    (double)(10.0 * Math.Log10(sqrMag[i] + 1e-180));
            }

            if (sharp_dsp_event != null)
            {
                sharp_dsp_event(this, new PowerSpectrumEvent(ref ps_results)); //raise event to indicate power spectrum complete                
            }

        }

        public void PowerSpectrumSignal(ref double[] real, ref double[] imag, ref double[] ps_result)
        {
            double[] buffer_real = new double[this.fft_size];
            double[] buffer_imag = new double[this.fft_size];
            double[] sqrMag = new double[this.fft_size];

            // window the data
            for (int i = 0; i < this.fft_size / 2; i++)
            {
                buffer_real[i] = real[i] * this.window[i];
                buffer_imag[i] = imag[i] * this.window[i];
            }

            this.fft.DoComplexDFTForward(ref buffer_real, ref buffer_imag);

            int tmp_size = this.fft_size / 2;
            for (int i = 0; i < tmp_size; i++)
            {
                sqrMag[i + tmp_size] = buffer_real[i + tmp_size] * buffer_real[i + tmp_size]
                    + buffer_imag[i + tmp_size] * buffer_imag[i + tmp_size];

                this.ps_results[i] =
                    (double)(10.0 * Math.Log10(sqrMag[i + tmp_size] + 1e-180));

                sqrMag[i] = buffer_real[i] * buffer_real[i]
                    + buffer_imag[i] * buffer_imag[i];

                this.ps_results[i + tmp_size] =
                    (double)(10.0 * Math.Log10(sqrMag[i] + 1e-180));
            }
            ps_result = ps_results;
        }
    }
}
