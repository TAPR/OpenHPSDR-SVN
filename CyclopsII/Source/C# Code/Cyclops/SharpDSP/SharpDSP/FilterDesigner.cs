/*
This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2007, 2008 Philip A Covington

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The author can be reached by email at

p.covington@gmail.com

*/

using System;

namespace SharpDSP2._1
{
	/// <summary>
	/// Filter Designer
	/// </summary>
	internal class FilterDesigner
    {  
        #region Private members

        private static float TWOPI = Convert.ToSingle(Math.PI * 2);
        private static float ONEPI = Convert.ToSingle(Math.PI);

        #endregion

        #region Public Methods

        internal void MakeFirLowpass(float cutoff_freq, 
                                   float samplerate,
                                   WindowType_e type, 
                                   ref CPX[] taps, 
                                   int length)
        {
        	float[] window = new float[length];

            float fc = cutoff_freq / samplerate;
            
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
                {
                    taps[j].real = (Convert.ToSingle(Math.Sin(TWOPI * (i - midpoint) * fc) / (ONEPI * (i - midpoint)))) * window[j];
                    taps[j].imag = (Convert.ToSingle(Math.Cos(TWOPI * (i - midpoint) * fc) / (ONEPI * (i - midpoint)))) * window[j];
                }
                else
                {
                    taps[midpoint - 1].real = 2.0F * fc;
                    taps[midpoint - 1].imag = 2.0F * fc;
                }
            }

        }

        internal void MakeFirBandpass(float lo, 
                                    float hi, 
                                    float samplerate,
                                    WindowType_e type, 
                                    ref CPX[] taps,
                                    int length)
        {
        	float[] window = new float[length];

            float fl = lo / samplerate;
            float fh = hi / samplerate;
            float fc = (fh - fl) / 2.0F;
            float ff = (fl + fh) * ONEPI;
                                    
            int midpoint = length >> 1;
            
            makewindow(type, length, ref window);

            for (int i = 1; i <= length; i++)
            {
                int j = i - 1;
                int k = i - midpoint;
                float temp = 0F;
                float phase = k * ff * -1F;
                if (i != midpoint)
                    temp = ((float)(Math.Sin(TWOPI * k * fc) / (ONEPI * k))) * window[j];
                else
                    temp = 2.0F * fc;
                temp *= 2.0F;
                taps[j].real = temp * (float)(Math.Cos(phase));
                taps[j].imag = temp * (float)(Math.Sin(phase));
            }

        }

        #endregion
        
        #region Private Methods
        
        internal void makewindow(WindowType_e type, 
                                 int size, 
                                 ref float[] window)
        {
            int i, j, midn, midp1, midm1;
            float freq, rate, sr1, angle, expn, expsum, cx, two_pi;

            midn = size / 2;
            midp1 = (size + 1) / 2;
            midm1 = (size - 1) / 2;
            two_pi = 8.0F * (float)Math.Atan(1.0F);
            freq = two_pi / (float) size;
            rate = 1.0F / (float) midn;
            angle = 0.0F;
            expn = (float)Math.Log(2.0F) / (float) midn + 1.0F;
            expsum = 1.0F;

            switch (type) 
            {
                case WindowType_e.RECTANGULAR_WINDOW:
                    for (i = 0; i < size; i++)
						window[i] = 1.0F;
                    break;
                case WindowType_e.HANNING_WINDOW:	/* Hann would be more accurate */
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq)
						window[j] = (window[i] = 0.5F - 0.5F * (float)Math.Cos(angle));
                    break;
                case WindowType_e.WELCH_WINDOW:
                    for (i = 0, j = size - 1; i <= midn; i++, j--)
						window[j] = (window[i] = 1.0F - (float)Math.Sqrt((float) (i - midm1) / (float) midp1));
                    break;
                case WindowType_e.PARZEN_WINDOW:
                    for (i = 0, j = size - 1; i <= midn; i++, j--)
						window[j] = (window[i] = 1.0F - Math.Abs((float) (i - midm1) / (float) midp1));
                    break;
                case WindowType_e.BARTLETT_WINDOW:
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += rate)
                    window[j] = (window[i] = angle);
                    break;
                case WindowType_e.HAMMING_WINDOW:
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq)
                    window[j] = (window[i] = 0.54F - 0.46F * (float)Math.Cos(angle));
                    break;
                case WindowType_e.BLACKMAN2_WINDOW:	/* using Chebyshev polynomial equivalents here */
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq) {
                    cx = (float)Math.Cos(angle);
                    window[j] = (window[i] = (.34401F + (cx * (-.49755F + (cx * .15844F)))));
                    }
                    break;
                case WindowType_e.BLACKMAN3_WINDOW:
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq) {
                    cx = (float)Math.Cos(angle);
                    window[j] =
	                (window[i] = (.21747F + (cx * (-.45325F + (cx * (.28256F - (cx * .04672F)))))));
                    }
                    break;
                case WindowType_e.BLACKMAN4_WINDOW:
                    for (i = 0, j = size - 1, angle = 0.0F; i <= midn; i++, j--, angle += freq) 
					{                    
						cx = (float)Math.Cos(angle);
                    
						window[j] = (window[i] =
		                (.084037F +
		                    (cx *
		                    (-.29145F +
		                    (cx *
		                    (.375696F + (cx * (-.20762F + (cx * .041194F)))))))));
                    }
                    break;
                case WindowType_e.EXPONENTIAL_WINDOW:
                    for (i = 0, j = size - 1; i <= midn; i++, j--) {
                    window[j] = (window[i] = expsum - 1.0F);
                    expsum *= expn;
                    }
                    break;
                case WindowType_e.RIEMANN_WINDOW:
                    sr1 = two_pi / (float) size;
                    for (i = 0, j = size - 1; i <= midn; i++, j--) {
                    if (i == midn) window[j] = (window[i] = 1.0F);
                    else {
	                /* split out because NeXT C compiler can't handle the full expression */
	                cx = sr1 * (midn - i);
	                window[i] = (float)Math.Sin(cx) / cx;
	                window[j] = window[i];
                    }
                    }
                    break;
                case WindowType_e.BLACKMANHARRIS_WINDOW:
                    {
		                float 
			                a0 = 0.35875F,
			                a1 = 0.48829F,
			                a2 = 0.14128F,
			                a3 = 0.01168F;


		                for (i = 0; i<size;i++) 
                        {
			                window[i] = a0 - a1* (float)Math.Cos(two_pi*(float)(i+0.5)/(float)size) 
				                + a2* (float)Math.Cos(2.0*two_pi*(float)(i+0.5)/(float)size)
				                - a3* (float)Math.Cos(3.0*two_pi*(float)(i+0.5)/(float)size);
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
