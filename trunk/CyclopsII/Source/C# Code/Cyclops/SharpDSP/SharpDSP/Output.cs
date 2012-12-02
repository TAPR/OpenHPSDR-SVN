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
	/// Output Handling
	/// </summary>
	public class Output : IDSPObject
    {
        #region Private members

        private DSPBuffer d = null;
        private DSPState s = null;
				
        #endregion
        
        #region Constructor

        public Output(ref DSPBuffer dsp_buffer_obj)
        {
            this.d = dsp_buffer_obj;
            this.s = d.State;        	     	
        }
        
        #endregion

        #region Public Methods

        public void Process()
        {          	        	        	
        	switch (this.oroute)
			{
				case RXOutputRoute_e.Both:
					DoOutputBoth();
					break;
				case RXOutputRoute_e.Right:
					DoOutputRight();
					break;
				case RXOutputRoute_e.Left:
					DoOutputLeft();
					break;
				case RXOutputRoute_e.Mixer:
					DoOutputMix();
					break;
			} 
        }
		
		#endregion

		#region Private Methods

		private void DoOutputBoth()
		{				
			switch (s.DSPMode)
			{
				case DSPMode_e.AM:
				case DSPMode_e.SAM:
				case DSPMode_e.FMN:
					
					// real & imag contain the same magnitude data after PLL
					d.Scale(this.volume_left, this.volume_right);
					
					break;

				case DSPMode_e.DSB:
				case DSPMode_e.USB:
				case DSPMode_e.LSB:
				case DSPMode_e.CWL:
				case DSPMode_e.CWU:
					
					if (this.binaural)
					{
                        d.Scale(this.volume_left, this.volume_right);										
					}
					else
					{
						d.CopyImagToReal();
                        d.Scale(this.volume_left, this.volume_right);
																		
					}
					
					break;

				case DSPMode_e.SPEC:
				default:

                    d.Scale(this.volume_left, this.volume_right);

					break;
			}

		}

		private void DoOutputRight()
		{
            d.Scale(0.0f, this.volume_right);
		}
		
		private void DoOutputLeft()
		{
            d.Scale(this.volume_left, 0.0f);
		}

		private void DoOutputMix()
		{	
			int blocksize = s.DSPBlockSize;
			
			for (int i = 0; i < blocksize; i++)
			{
                d.tmp_cpx_1[i].real = d.cpx[i].real * this.volume_left + d.cpx[i].imag * this.volume_right;
                d.tmp_cpx_1[i].imag = d.cpx[i].real * this.volume_right + d.cpx[i].imag * this.volume_left;
			}
			d.CopyTemp1BotToMainBot();			
		}
										
        #endregion

        #region Properties

        private RXOutputRoute_e oroute = RXOutputRoute_e.Both;
        public RXOutputRoute_e RXOutputRoute
        {
            get { return oroute; }
            set { oroute = value; }
        }

        private float volume_left = 0.5f;
        public float VolumeLeft
        {
            get { return volume_left; }
            set { volume_left = value; }
        }

        private float volume_right = 0.5f;
        public float VolumeRight
        {
            get { return volume_right; }
            set { volume_right = value; }
        }

        private bool binaural = false;
        public bool BinAuralMode
        {
            get { return binaural; }
            set { binaural = value; }
        }	

        #endregion

    }
}
