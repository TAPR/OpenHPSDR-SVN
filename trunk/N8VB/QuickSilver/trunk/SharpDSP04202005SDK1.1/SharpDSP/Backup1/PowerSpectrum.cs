//==============================================================
//Adapted by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//powerspectrum.cs
//
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{   
	[Serializable()]
	public struct PowerSpectrum
    {
		public static event SharpDSPEventHandler sharp_dsp_event;
                
        #region Private members

        private PowerSpectrumPosition position;

        private float[] window;
        private float[] ps_results;     //holds power spectrum results
        private float[] scope_results;  //holds scope results
        private float[] phase_results;  //holds phase results

        private FFT fft;
        private FilterDesigner filt_design;

        #endregion

        #region Properties

		private PowerSpectrumMode mode;
        public PowerSpectrumMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
            }
        }

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

        unsafe public PowerSpectrum(int fft_size)
        {
            this.mode = PowerSpectrumMode.OFF;
            this.position = PowerSpectrumPosition.PREFILTER;
            this.fft_size = fft_size;
            this.results_size = fft_size;
            this.window = new float[fft_size / 2];
            this.ps_results = new float[fft_size]; //don't overrun results buffer
            this.scope_results = new float[fft_size];
            this.phase_results = new float[fft_size * 2];
            this.fft = new FFT(fft_size);
            this.filt_design = new FilterDesigner();
            fixed (float* ptr_window = &window[0])
			{
				filt_design.makewindow(WindowType.BLACKMANHARRIS_WINDOW, fft_size / 2, ptr_window);
			}
        }

        #endregion

        #region Public Methods

        unsafe public void DoPowerSpectrum(float* real, float* imag, PowerSpectrumPosition positon)
        {
            if (mode == PowerSpectrumMode.OFF) { return; }

            switch (positon)
            {   
				case PowerSpectrumPosition.INITIAL:
					switch (mode)
					{						
						case PowerSpectrumMode.PHASE2:
							this.PowerSpectrumPhase2(real, imag);
							break;
						default:
							break;
					}
					break;
				case PowerSpectrumPosition.PREFILTER:
                    switch (mode)
                    {
                        case PowerSpectrumMode.PANADAPTER:
                        case PowerSpectrumMode.SPECMODE:
                            this.PowerSpectrumSignal(real, imag);
                            break;
						default:
							break;
                    }
                    break;
                case PowerSpectrumPosition.POSTFILTER:
                    switch (mode)
                    {
                        case PowerSpectrumMode.HISTOGRAM:
                        case PowerSpectrumMode.WATERFALL:
                        case PowerSpectrumMode.SPECTRUM:
                            this.PowerSpectrumSignal(real, imag);
                            break;
                        case PowerSpectrumMode.SCOPE:
                            this.PowerSpectrumScope(real);
                            break;
						default:
							break;
                    }
                    break;
                case PowerSpectrumPosition.POSTAGC:
                    switch (mode)
                    {
                        case PowerSpectrumMode.PHASE:
                            this.PowerSpectrumPhase(real, imag);
                            break;
						default:
							break;
                    }
                    break;
				case PowerSpectrumPosition.FINAL:
					break;
                default:
                    break;
            }
        }
        
        #endregion

        #region Private Methods
        
		unsafe private void PowerSpectrumSignal(float* real, float* imag)
		{
			float* buffer_real = stackalloc float[this.fft_size];
			float* buffer_imag = stackalloc float[this.fft_size];
			float* sqrMag = stackalloc float[this.fft_size];

			//zero out the buffers
			Memory.ZeroFloatMemory(buffer_real, this.fft_size);
			Memory.ZeroFloatMemory(buffer_imag, this.fft_size);

			// window the data
			for (int i = 0; i < this.fft_size / 2; i++)
			{
				buffer_real[i] = real[i]* this.window[i];
				buffer_imag[i] = imag[i]* this.window[i];
			}	
			
			this.fft.DoComplexDFTForward(buffer_real, buffer_imag);

			int tmp_size = this.fft_size / 2;
			for (int i = 0; i < tmp_size; i++)
			{
				sqrMag[i + tmp_size] = buffer_real[i + tmp_size] * buffer_real[i + tmp_size]
					+ buffer_imag[i + tmp_size] * buffer_imag[i + tmp_size];

				this.ps_results[i] =
					(float)(10.0 * Math.Log10(sqrMag[i + tmp_size] + 1e-180));
                
				sqrMag[i] = buffer_real[i] * buffer_real[i]
					+ buffer_imag[i] * buffer_imag[i];

				this.ps_results[i + tmp_size] =
					(float)(10.0 * Math.Log10(sqrMag[i] + 1e-180));
			}

			if (sharp_dsp_event != null)
			{
				fixed (float* ptr_buffer = &this.ps_results[0])
				{
					sharp_dsp_event(this, new SharpDSPEvent(ptr_buffer)); //raise event to indicate power spectrum complete
				}
			}

		}

        unsafe private void PowerSpectrumScope(float* real)
        {
            for (int i = 0; i < this.fft_size; i++)
                this.scope_results[i] = real[i];

            if (sharp_dsp_event != null)
            {
                fixed(float* ptr_buffer = &this.scope_results[0])
                {
                    sharp_dsp_event(this, new SharpDSPEvent(ptr_buffer)); //raise event to indicate power spectrum scope complete
                }
            }
        }

        unsafe private void PowerSpectrumPhase(float* real, float* imag)
        {
            for (int i = 0, j = 0; i < this.fft_size; i++, j += 2)
            {
                this.phase_results[j] = real[i];
                this.phase_results[j + 1] = imag[i];
            }

            if (sharp_dsp_event != null)
            {
                fixed (float* ptr_buffer = &this.phase_results[0])
                {
                    sharp_dsp_event(this, new SharpDSPEvent(ptr_buffer)); //raise event to indicate power spectrum phase complete
                }
            }

        }

		unsafe private void PowerSpectrumPhase2(float* real, float* imag)
		{
			for (int i = 0, j = 0; i < this.fft_size / 2; i++, j += 2)
			{
				this.phase_results[j] = real[i];
				this.phase_results[j + 1] = imag[i];
			}

			if (sharp_dsp_event != null)
			{
				fixed (float* ptr_buffer = &this.phase_results[0])
				{
					sharp_dsp_event(this, new SharpDSPEvent(ptr_buffer)); //raise event to indicate power spectrum phase complete
				}
			}

		}

        #endregion

    }

}     