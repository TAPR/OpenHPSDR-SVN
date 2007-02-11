//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//Filter.cs
//filter code
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    unsafe public struct Filter
    {        
        #region Private members

        private float[] filter_real;    //holds real part of filter
        private float[] filter_imag;    //holds imaginary part of filter

        private float[] i_overlap;      //real part of overlap for overlap-add
        private float[] q_overlap;      //imaginary part of overlap for overlap-add

        private int size;               //size of filter (must be FFT size or 2x blocksize)

        private FilterDesigner filter_designer;
        private FFT fft;

        #endregion

        #region Constructor

        public Filter(int size)
        {
            this.filter_real = new float[size];
            this.filter_imag = new float[size];
            this.i_overlap = new float[size / 2];
            this.q_overlap = new float[size / 2];
            this.size = size;
            this.fft = new FFT(size);
            this.filter_designer = new FilterDesigner();
        }

        #endregion

        #region Public Methods

        public void MakeFilter(float freq_lo, float freq_hi, float samplerate, FilterType filter_type, WindowType window_type)
        {
            this.filter_real = new float[size];
            this.filter_imag = new float[size];
            
            switch (filter_type)
            {                
                case FilterType.LowPass:
                    fixed (float* ptr_filter_real = &this.filter_real[0])
                    fixed (float* ptr_filter_imag = &this.filter_imag[0])
                    {
                        this.filter_designer.MakeFirLowpass(freq_hi, samplerate, window_type, ptr_filter_real, this.size / 2);
                        this.fft.DoComplexDFTForward(ptr_filter_real, ptr_filter_imag);
                    }
                    break;
                case FilterType.BandPass:
                    fixed (float* ptr_filter_real = &this.filter_real[0])
                    fixed (float* ptr_filter_imag = &this.filter_imag[0])
                    {
                        this.filter_designer.MakeFirBandpass(freq_lo, freq_hi, samplerate, window_type, ptr_filter_real, ptr_filter_imag, this.size / 2);
                        this.fft.DoComplexDFTForward(ptr_filter_real, ptr_filter_imag);
                    }
                    break;
                default:
                    break;
            }
        }

        public void DoFilter(float* real, float* imag)
        {
            float* tempbuffer_l = stackalloc float[this.size];
            float* tempbuffer_r = stackalloc float[this.size];

            this.fft.DoComplexDFTForward(real, imag);

            for (int i = 0; i < this.size; i++)  //convolution in frequency here
            {
                tempbuffer_l[i] = (this.filter_real[i] * real[i]) - (this.filter_imag[i] * imag[i]);
                tempbuffer_r[i] = (this.filter_real[i] * imag[i]) + (this.filter_imag[i] * real[i]);
                real[i] = tempbuffer_l[i];
                imag[i] = tempbuffer_r[i];
            }

            this.fft.DoComplexDFTInverse(real, imag);
        }

        public void DoOverlapAdd(float* real, float* imag)
        {
            // overlap add

            for (int i = 0; i < this.size / 2; i++)
            {
                real[i] += this.i_overlap[i];
                imag[i] += this.q_overlap[i];
                this.i_overlap[i] = real[i + this.size / 2];  //save overlap for next pass
                this.q_overlap[i] = imag[i + this.size / 2];  //save overlap for next pass
            }

        }
        #endregion
    }

}