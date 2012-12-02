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
using System.Collections.Generic;
using System.Text;

namespace SharpDSP2._1
{
    class Resampler
    {
        private CPX[] filter_memory;
        private CPX[] filter_coeff;
        private int filter_memory_index = 0;
        private int interpolation_factor = 1;
        private int decimation_factor = 1;
        private int phase_count = 0;
        private int output_index = 0;
        private int mask;
        private int filter_size = 0;
        private int filter_coeff_sz = 0;
        
        public Resampler(int filtersz, int interpfactor, int decifactor)
        {
            this.interpolation_factor = interpfactor;
            this.decimation_factor = decifactor;
            this.filter_coeff = new CPX[this.interpolation_factor * 31];            
            this.filter_size = filtersz;            
            this.mask = this.filter_size - 1;
            this.filter_coeff_sz = 31 * this.interpolation_factor;
            this.filter_memory = new CPX[this.filter_size];
            FilterDesigner fdes = new FilterDesigner();
            fdes.MakeFirLowpass(0.45f / (float)this.decimation_factor,
                                this.interpolation_factor,
                                WindowType_e.BLACKMANHARRIS_WINDOW,
                                ref this.filter_coeff,
                                31 * this.interpolation_factor);
            //fdes.MakeFirBandpass(0.0001f, 0.45f / (float)this.decimation_factor,
            //                    this.interpolation_factor, WindowType_e.BLACKMANHARRIS_WINDOW,
            //                    ref this.filter_coeff, 33 * this.interpolation_factor);
        }

        public int DoResample(CPX[] input, int insz, CPX[] output)
        {
            int j, jj;

            this.output_index = 0;            

            for (int i=0; i < insz; i++)
            {
                // save data in the circuilar buffer
                this.filter_memory[this.filter_memory_index].real = input[i].real;
                this.filter_memory[this.filter_memory_index].imag = input[i].imag;
                j = this.filter_memory_index;
                jj = j;

                // circular addressing
                this.filter_memory_index = (this.filter_memory_index + 1) & this.mask;

                // interpolate then decimate
                while (this.phase_count < this.interpolation_factor)
                {
                    j = jj;
                    output[this.output_index].real = 0.0f;
                    output[this.output_index].imag = 0.0f;

                    // convolution
                    for (int k = this.phase_count; k < this.filter_coeff_sz; k += this.interpolation_factor)
                    {
                        output[this.output_index].real = output[this.output_index].real + (this.filter_memory[j].real * this.filter_coeff[k].real);
                        output[this.output_index].imag = output[this.output_index].imag + (this.filter_memory[j].imag * this.filter_coeff[k].imag);
                        
                        // circular addressing
                        j = (j + this.mask) & this.mask;
                    }

                    // scale output
                    output[this.output_index].real = output[this.output_index].real * this.interpolation_factor;
                    output[this.output_index].imag = output[this.output_index].imag * this.interpolation_factor;
                    this.output_index += 1;

                    // increment the interploation phase number by decimation factor
                    this.phase_count += this.decimation_factor;
                }
                this.phase_count -= this.interpolation_factor;
            }
            return output_index;
        }

        public int DoResample(CPX[] input, int insz, float[] output_r, float[] output_i)
        {
            int j, jj;

            this.output_index = 0;
            
            for (int i = 0; i < insz; i++)
            {
                // save data in the circuilar buffer
                this.filter_memory[this.filter_memory_index].real = input[i].real;
                this.filter_memory[this.filter_memory_index].imag = input[i].imag;
                j = this.filter_memory_index;
                jj = j;

                // circular addressing
                this.filter_memory_index = (this.filter_memory_index + 1) & this.mask;

                // interpolate then decimate
                while (this.phase_count < this.interpolation_factor)
                {
                    j = jj;
                    output_r[this.output_index] = 0.0f;
                    output_i[this.output_index]= 0.0f;

                    // convolution
                    for (int k = this.phase_count; k < this.filter_coeff.Length; k += this.interpolation_factor)
                    {
                        output_r[this.output_index] = output_r[this.output_index] + (this.filter_memory[j].real * this.filter_coeff[k].real);
                        output_i[this.output_index] = output_i[this.output_index] + (this.filter_memory[j].imag * this.filter_coeff[k].imag);

                        // circular addressing
                        j = (j + this.mask) & this.mask;
                    }

                    // scale output
                    output_r[this.output_index] = output_r[this.output_index] * this.interpolation_factor;
                    output_i[this.output_index] = output_i[this.output_index] * this.interpolation_factor;
                    this.output_index += 1;

                    // increment the interploation phase number by decimation factor
                    this.phase_count += this.decimation_factor;
                }
                this.phase_count -= this.interpolation_factor;
            }
            return output_index;
        }

        public int DoResampleReal(CPX[] input, int insz, float[] output)
        {
            int j, jj;

            this.output_index = 0;
            
            for (int i = 0; i < insz; i++)
            {
                // save data in the circuilar buffer
                this.filter_memory[this.filter_memory_index].real = input[i].real;
                j = this.filter_memory_index;
                jj = j;

                // circular addressing
                this.filter_memory_index = (this.filter_memory_index + 1) & this.mask;

                // interpolate then decimate
                while (this.phase_count < this.interpolation_factor)
                {
                    j = jj;
                    output[this.output_index] = 0.0f;                    

                    // convolution
                    for (int k = this.phase_count; k < this.filter_coeff.Length; k += this.interpolation_factor)
                    {
                        output[this.output_index] = output[this.output_index] + (this.filter_memory[j].real * this.filter_coeff[k].real);
                        
                        // circular addressing
                        j = (j + this.mask) & this.mask;
                    }

                    // scale output
                    output[this.output_index] = output[this.output_index] * this.interpolation_factor * this.interpolation_factor;
                    this.output_index += 1;

                    // increment the interploation phase number by decimation factor
                    this.phase_count += this.decimation_factor;
                }
                this.phase_count -= this.interpolation_factor;
            }
            return output_index;
        }

        public int DoResampleImag(CPX[] input, int insz, float[] output)
        {
            int j, jj;

            this.output_index = 0;

            for (int i = 0; i < insz; i++)
            {
                // save data in the circuilar buffer
                this.filter_memory[this.filter_memory_index].real = input[i].imag;
                j = this.filter_memory_index;
                jj = j;

                // circular addressing
                this.filter_memory_index = (this.filter_memory_index + 1) & this.mask;

                // interpolate then decimate
                while (this.phase_count < this.interpolation_factor)
                {
                    j = jj;
                    output[this.output_index] = 0.0f;

                    // convolution
                    for (int k = this.phase_count; k < this.filter_coeff.Length; k += this.interpolation_factor)
                    {
                        output[this.output_index] = output[this.output_index] + (this.filter_memory[j].real * this.filter_coeff[k].real);

                        // circular addressing
                        j = (j + this.mask) & this.mask;
                    }

                    // scale output
                    output[this.output_index] = output[this.output_index] * this.interpolation_factor * this.interpolation_factor; 
                    this.output_index += 1;

                    // increment the interploation phase number by decimation factor
                    this.phase_count += this.decimation_factor;
                }
                this.phase_count -= this.interpolation_factor;
            }
            return output_index;
        }
    }
}
