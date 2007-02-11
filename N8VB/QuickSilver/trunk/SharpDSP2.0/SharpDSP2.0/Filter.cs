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
    public struct Filter
    {        
        #region Private members

        private double[] filter_real;    //holds real part of filter
        private double[] filter_imag;    //holds imaginary part of filter

        private double[] i_overlap;      //real part of overlap for overlap-add
        private double[] q_overlap;      //imaginary part of overlap for overlap-add

        private int size;               //size of filter (must be FFT size or 2x blocksize)

        private FilterDesigner filter_designer;
        private FFT fft;

        #endregion

        #region Constructor

        public Filter(int size)
        {
            this.filter_real = new double[size];
            this.filter_imag = new double[size];
            this.i_overlap = new double[size / 2];
            this.q_overlap = new double[size / 2];
            this.size = size;
            this.fft = new FFT(size);
            this.filter_designer = new FilterDesigner();
        }

        #endregion

        #region Public Methods

        public void MakeFilter(double freq_lo, double freq_hi, double samplerate, FilterType filter_type, WindowType window_type)
        {
            this.filter_real = new double[size];
            this.filter_imag = new double[size];
            
            switch (filter_type)
            {                
                case FilterType.LowPass:
            		this.filter_designer.MakeFirLowpass(freq_hi, 
            		                                    samplerate, 
            		                                    window_type, 
            		                                    ref this.filter_real, 
            		                                    this.size / 2);
                    this.fft.DoComplexDFTForward(ref this.filter_real, ref this.filter_imag);
            		break;
                case FilterType.BandPass:
            		this.filter_designer.MakeFirBandpass(freq_lo, 
            		                                     freq_hi, 
            		                                     samplerate, 
            		                                     window_type, 
            		                                     ref this.filter_real, 
            		                                     ref this.filter_imag, 
            		                                     this.size / 2);
                    this.fft.DoComplexDFTForward(ref this.filter_real, ref this.filter_imag);
                    break;
                default:
                    break;
            }
        }

        public void MakeFilterFromCoeffArray(double[] coeff)
        {
        	this.filter_real = new double[size];
            this.filter_imag = new double[size];
            
            for (int i=0; i<coeff.Length; i++)
            {
            	this.filter_real[i] = coeff[i];
            }
        	this.fft.DoComplexDFTForward(ref this.filter_real, ref this.filter_imag);
        }
        
        public void DoFilter(ref double[] real, ref double[] imag)
        {
        	double[] tempbuffer_l = new double[this.size];
        	double[] tempbuffer_r = new double[this.size];

            this.fft.DoComplexDFTForward(ref real, ref imag);

            for (int i = 0; i < this.size; i++)  //convolution in frequency here
            {
                tempbuffer_l[i] = (this.filter_real[i] * real[i]) - (this.filter_imag[i] * imag[i]);
                tempbuffer_r[i] = (this.filter_real[i] * imag[i]) + (this.filter_imag[i] * real[i]);
                real[i] = tempbuffer_l[i];
                imag[i] = tempbuffer_r[i];
            }

            this.fft.DoComplexDFTInverse(ref real, ref imag);
        }

        public void DoOverlapAdd(ref double[] real, ref double[] imag)
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
