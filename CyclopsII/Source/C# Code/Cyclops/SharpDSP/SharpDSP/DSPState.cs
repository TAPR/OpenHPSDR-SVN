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
using System.Xml.Serialization;

namespace SharpDSP2._1
{
    [Serializable]
    public class DSPState
    {
        #region General DSP Properties

//        private QS1RServer.ServerConfiguration server_config = null;
        //public QS1RServer.ServerConfiguration ServerConfigObject
//        {
//            get { return server_config; }
//            set { server_config = value; }
//        }

        private float dsp_sample_rate = 48003f;
        public float DSPSampleRate
        {
            get { return dsp_sample_rate; }
            set { dsp_sample_rate = value; }
        }

        private int block_size = 1024;
        public int DSPBlockSize 
        {
            get { return block_size; }
            set { block_size = value; }
        }

        private int ps_block_size = 1024;
        public int PowerSpectrumBlockSize
        {
            get { return ps_block_size; }
            set { ps_block_size = value; }
        }

        private int spec_block_size = 2048;
        public int SpectrumAquireBlockSize
        {
            get { return spec_block_size; }
            set { spec_block_size = value; }
        }

        private int output_rate_divisor = 1;
        public int OutputRateDivisor
        {
            get { return output_rate_divisor; }
            set { output_rate_divisor = value; }
        }

        private DSPMode_e dsp_mode = DSPMode_e.AM;
        public DSPMode_e DSPMode
        {
            get { return dsp_mode; }
            set { dsp_mode = value; }
        }

        private int ringbuffer_size = 4096 * 8;
        public int RingBufferSize
        {
            get { return ringbuffer_size; }
            set { ringbuffer_size = value; }
        }

        #endregion                               
    }
}
