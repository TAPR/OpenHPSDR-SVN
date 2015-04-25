/*
*
* Copyright (C) 2008 Bill Tracey, KD5TFD, bill@ewjt.com 
* Copyright (C) 2010-2013  Doug Wigley
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

// this module contains code to support the Alex Filter and Antenna Selection board 
// 
// 
using System;
using System.Threading;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for Penny.
	/// </summary>
	public class Alex
	{
		private static Alex theSingleton = null; 
		
		public  static Alex getAlex() 
		{ 
			lock ( typeof(Alex) ) 
			{
				if ( theSingleton == null ) 
				{ 
					theSingleton = new Alex(); 
				} 
			}
			return theSingleton; 
		} 

		private Alex()
		{		
			for ( int i = 0; i < 12; i++ ) 
			{
				TxAnt[i] = 1; 
				RxAnt[i] = 1; 
				RxOnlyAnt[i] = 0; 
			}
		}


		private byte[] TxAnt = new byte[12]; 
		private byte[] RxAnt = new byte[12]; 
		private byte[] RxOnlyAnt = new byte[12]; // 1 = rx1, 2 = rx2, 3 = xv, 0 = none selected 

        public static bool RxOutOnTx = false;
        public static bool Ext1OutOnTx = false;
        public static bool Ext2OutOnTx = false;
        public static bool init_update = false;
        public static bool rx_out_override = false;

		public void setRxAnt(Band band, byte ant) 
		{ 
			if ( ant > 3 ) 
			{ 
				ant = 1; 
			} 
			int idx = (int)band - (int)Band.B160M; 
			RxAnt[idx] = ant; 
		} 

		public void setRxOnlyAnt(Band band, byte ant) 
		{ 
			if ( ant > 3 ) 
			{ 
				//ant = 0; 
			} 
			int idx = (int)band - (int)Band.B160M; 
			RxOnlyAnt[idx] = ant; 
		} 

		public void setTxAnt(Band band, byte ant) 
		{ 
			if ( ant > 3 ) 
			{ 
				ant = 1; 
			} 
			int idx = (int)band - (int)Band.B160M; 
			TxAnt[idx] = ant;
		} 


		private bool AlexEnableIsStateSaved = false; 
		private bool AlexEnableSavedState; 

		
		public static Band AntBandFromFreq() 
		{

			
			Band result;
 
			Console c = Console.getConsole(); 
			if ( c == null ) 
			{ 
				System.Console.WriteLine("no console"); 
				return Band.LAST; 				
			} 

			double freq  = Console.getConsole().VFOAFreq;   //was = 0.0 Vk4xv Txvr fix.

            if (c.RX1XVTRIndex >= 0)
                freq = c.XVTRForm.TranslateFreq(freq);
            else freq = Console.getConsole().VFOAFreq;

			System.Console.WriteLine("Freq is: " + freq); 


			if ( freq >= 12.075 ) 
			{ 
				if ( freq >= 23.17 ) 
				{
					if ( freq >= 26.465 ) 
					{ 
						result = freq >= 39.85 ? Band.B6M : Band.B10M; 
					}
					else /* >23.17  <26.465 */
					{
						result = Band.B12M;
					}
				}
				else  /* >12.075  <23.17 */ 
				{ 
					if ( freq >= 16.209 ) 
					{ 
						result = freq >= 19.584 ? Band.B15M : Band.B17M;
					}
					else 
					{ 
						result = Band.B20M; 
					} 
				}
			} 
			else  /* < 12.075 */ 
			{
				if ( freq  >= 6.20175 ) 
				{
					result = freq >= 8.7 ? Band.B30M : Band.B40M; 
				}
				else 
				{ 
					if ( freq >= 4.66525 ) 
					{
						result = Band.B60M;
					}
					else 
					{
						result = freq >= 2.75 ? Band.B80M : Band.B160M;
					}
				} 
			}
			return result; 
		}


        public void UpdateAlexAntSelection(Band band, bool tx, bool xvtr)  
		{ 
			UpdateAlexAntSelection(band, tx, true, xvtr); 
		}

		public void UpdateAlexAntSelection(Band band, bool tx, bool alex_enabled, bool xvtr) 
		{

			if ( !alex_enabled ) 
			{ 
				JanusAudio.SetAlexAntBits(0, 0, 0); 
				return;
			}
            

			int rx_only_ant; 
			int trx_ant; 
			int rx_out;
            int xrx_out;

			int idx = (int)band - (int)Band.B160M; 

			if ( idx < 0 || idx > 11 ) 
			{ 
				band = AntBandFromFreq(); 
				idx = (int)band - (int)Band.B160M; 
				if ( idx < 0 || idx > 11 ) 
				{ 
					System.Console.WriteLine("No good ant!"); 
					return; 
				}
			} 
			System.Console.WriteLine("Ant idx: " + idx); 
			

			if ( tx ) 
			{
                if (Ext2OutOnTx) rx_only_ant = 1;
                else if (Ext1OutOnTx) rx_only_ant = 2;
                else rx_only_ant = 0;

                rx_out = RxOutOnTx || Ext1OutOnTx || Ext2OutOnTx ? 1 : 0; 
				trx_ant = TxAnt[idx]; 
			} 
			else 
			{
                rx_only_ant = RxOnlyAnt[idx];
                if (xvtr)
                {
                    if (rx_only_ant >= 3) rx_only_ant = 3;
                    else rx_only_ant = 0;
                }
                else
                {
                    if (rx_only_ant >= 3) rx_only_ant -= 3; // do not use XVTR ant port if not using transverter
                }

                rx_out = rx_only_ant != 0 ? 1 : 0;
                trx_ant = RxAnt[idx];
            }

            if (rx_out_override && rx_out == 1)
            {
                if (!tx) trx_ant = 4;
                rx_out = 0; // disable Rx_Bypass_Out relay
            }

            if (init_update)
            {
                if (rx_out == 0) xrx_out = 1; // workaround for Hermes
                else xrx_out = 0;
                JanusAudio.SetAlexAntBits(rx_only_ant, trx_ant, xrx_out);
                init_update = false;
                Thread.Sleep(10);
            }
			JanusAudio.SetAlexAntBits(rx_only_ant, trx_ant, rx_out); 

			// don't allow changing antenna selections when mox is activated 
		/*	if ( tx )  
			{ 
				AlexEnableSavedState = Console.getConsole().SetupForm.SetAlexAntEnabled(false); 
				AlexEnableIsStateSaved = true; 
			} 
			else if ( AlexEnableIsStateSaved ) 
			{ 
				Console.getConsole().SetupForm.SetAlexAntEnabled(AlexEnableSavedState); 
				AlexEnableIsStateSaved = false; 
			} */

            // Console.getConsole().SetupForm.txtRXAnt.Text = rx_ant.ToString();
           //  Console.getConsole().SetupForm.txtRXOut.Text = rx_out.ToString();
           //  Console.getConsole().SetupForm.txtTXAnt.Text = tx_ant.ToString();
            // Console.getConsole().SetupForm.txtAlexBand.Text = band.ToString();
           //  Console.getConsole().SetupForm.txtAlexEnabled.Text = alex_enabled.ToString();
           //  Console.getConsole().SetupForm.txtAlexBits.Text = Convert.ToString(rc, 2);

			return; 
		}




#if false
		public void setBandBitMask(Band band, byte mask, bool tx) 
		{ 
			int idx = (int)band - (int)Band.B160M; 
			if ( tx ) 
			{ 
				TXBitMasks[idx] = mask;
			} 
			else 
			{ 
				RXBitMasks[idx] = mask;
			} 
			return; 

		} 

		public void ExtCtrlEnable(bool enable, Band band, bool tx ) 
		{
			if ( !enable ) 
			{
				JanusAudio.SetPennyOCBits(0); 
			}
			else 
			{
				UpdateExtCtrl(band, tx);
			}
		}

		public void UpdateExtCtrl(Band band, bool tx) 
		{
			int idx = (int)band - (int)Band.B160M; 
			int bits; 
			if ( idx < 0 || idx > 11 ) 
			{ 
				bits = 0; 
			} 
			else 
			{ 
				if ( tx ) 
				{ 
					bits = TXBitMasks[idx]; 
				}
				else 
				{
					bits = RXBitMasks[idx];
				}
			}
			System.Console.WriteLine("Bits: " + bits + " Band: " + (int)band); 
			JanusAudio.SetPennyOCBits(bits);
	
		}
#endif
	}
}
