//==============================================================
// Copyright (C) 2006 Philip A Covington, N8VB
//
// This software is licensed under the GNU General public License
//==============================================================
//FilterDesign.cs
//window design code based on Bill Schottstaedt's clm.c
//adapted to floating point
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{    
	[Serializable()]
    public class FilterDesigner
    {       

        #region Private members

        private static double TWOPI = Math.PI * 2;
        private static double ONEPI = Math.PI;

        #endregion

        #region Public Methods

        public void MakeFirLowpass(double cutoff_freq, double samplerate, WindowType type, ref double[] taps, int length)
        {
            double[] window = new double[length];

            double fc = cutoff_freq / samplerate;
            
            if (fc > 0.5)
            {
                return;
            }
            
            int midpoint = length >> 1;

            makewindow(type, length, ref window);

            for (int i = 1; i <= length; i++)
            {
                int j = i - 1;
                if (i != midpoint)
                    taps[j] = (Math.Sin(TWOPI * (i - midpoint) * fc) / (ONEPI * (i - midpoint))) * window[j];
                else
                    taps[midpoint - 1] = 2.0F * fc;
            }

        }

        public void MakeFirBandpass(double lo, double hi, double samplerate, WindowType type, ref double[] taps_real, ref double[] taps_imag, int length)
        {
            double[] window = new double[length];

            double fl = lo / samplerate;
            double fh = hi / samplerate;
            double fc = (fh - fl) / 2.0F;
            double ff = (fl + fh) * ONEPI;
                                    
            int midpoint = length >> 1;
            makewindow(type, length, ref window);

            for (int i = 1; i <= length; i++)
            {
                int j = i - 1;
                int k = i - midpoint;
                double temp = 0F;
                double phase = k * ff * -1F;
                if (i != midpoint)
                    temp = ((Math.Sin(TWOPI * k * fc) / (ONEPI * k))) * window[j];
                else
                    temp = 2.0F * fc;
                temp *= 2.0F;
                taps_real[j] = temp * (Math.Cos(phase));
                taps_imag[j] = temp * (Math.Sin(phase));
            }

        }

        public void makewindow(WindowType type, int size, ref double[] window) 
        {
            int i, j, midn, midp1, midm1;
            double freq, rate, sr1, angle, expn, expsum, cx, two_pi;

            midn = size / 2;
            midp1 = (size + 1) / 2;
            midm1 = (size - 1) / 2;
            two_pi = 8.0F * Math.Atan(1.0F);
            freq = two_pi / (double) size;
            rate = 1.0F / (double) midn;
            angle = 0.0F;
            expn = Math.Log(2.0F) / (double) midn + 1.0F;
            expsum = 1.0F;

            switch (type) 
            {
                case WindowType.RECTANGULAR_WINDOW:
                    for (i = 0; i < size; i++)
						window[i] = 1.0F;
                    break;
                case WindowType.HANNING_WINDOW:	/* Hann would be more accurate */
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq)
						window[j] = (window[i] = 0.5F - 0.5F * Math.Cos(angle));
                    break;
                case WindowType.WELCH_WINDOW:
                    for (i = 0, j = size - 1; i <= midn; i++, j--)
						window[j] = (window[i] = 1.0F - Math.Sqrt((double) (i - midm1) / (double) midp1));
                    break;
                case WindowType.PARZEN_WINDOW:
                    for (i = 0, j = size - 1; i <= midn; i++, j--)
						window[j] = (window[i] = 1.0F - Math.Abs((double) (i - midm1) / (double) midp1));
                    break;
                case WindowType.BARTLETT_WINDOW:
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += rate)
                    window[j] = (window[i] = angle);
                    break;
                case WindowType.HAMMING_WINDOW:
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq)
                    window[j] = (window[i] = 0.54F - 0.46F * Math.Cos(angle));
                    break;
                case WindowType.BLACKMAN2_WINDOW:	/* using Chebyshev polynomial equivalents here */
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq) {
                    cx = Math.Cos(angle);
                    window[j] = (window[i] = (.34401F + (cx * (-.49755F + (cx * .15844F)))));
                    }
                    break;
                case WindowType.BLACKMAN3_WINDOW:
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq) {
                    cx = Math.Cos(angle);
                    window[j] =
	                (window[i] = (.21747F + (cx * (-.45325F + (cx * (.28256F - (cx * .04672F)))))));
                    }
                    break;
                case WindowType.BLACKMAN4_WINDOW:
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq) 
					{                    
						cx = Math.Cos(angle);
                    
						window[j] = (window[i] =
		                (.084037F +
		                    (cx *
		                    (-.29145F +
		                    (cx *
		                    (.375696F + (cx * (-.20762F + (cx * .041194F)))))))));
                    }
                    break;
                case WindowType.EXPONENTIAL_WINDOW:
                    for (i = 0, j = size - 1; i <= midn; i++, j--) {
                    window[j] = (window[i] = expsum - 1.0F);
                    expsum *= expn;
                    }
                    break;
                case WindowType.RIEMANN_WINDOW:
                    sr1 = two_pi / (double) size;
                    for (i = 0, j = size - 1; i <= midn; i++, j--) {
                    if (i == midn) window[j] = (window[i] = 1.0F);
                    else {
	                /* split out because NeXT C compiler can't handle the full expression */
	                cx = sr1 * (midn - i);
	                window[i] = Math.Sin(cx) / cx;
	                window[j] = window[i];
                    }
                    }
                    break;
                case WindowType.BLACKMANHARRIS_WINDOW:
                    {
		                float 
			                a0 = 0.35875F,
			                a1 = 0.48829F,
			                a2 = 0.14128F,
			                a3 = 0.01168F;


		                for (i = 0; i<size;i++) 
                        {
                            window[i] = a0 - a1 * Math.Cos(two_pi * (double)(i + 0.5) / (double)size)
                                + a2 * Math.Cos(2.0 * two_pi * (double)(i + 0.5) / (double)size)
                                - a3 * Math.Cos(3.0 * two_pi * (double)(i + 0.5) / (double)size);
		                }
	                }
                    break;
                default:
                    return;
                    
            }
        }

        #endregion

    }
}

    
