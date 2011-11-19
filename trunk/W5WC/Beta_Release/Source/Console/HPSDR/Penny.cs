/*
*
* Copyright (C) 2008 Bill Tracey, KD5TFD, bill@ewjt.com 
*
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

//
// this module contains code to support the Penelope Transmitter board 
// 
// 



namespace PowerSDR
{
	/// <summary>
	/// Summary description for Penny.
	/// </summary>
	public class Penny
	{
		private static Penny theSingleton = null; 
		
		public  static Penny getPenny() 
		{ 
			lock ( typeof(Penny) ) 
			{
				if ( theSingleton == null ) 
				{ 
					theSingleton = new Penny(); 
				} 
			}
			return theSingleton; 
		} 

		private Penny()
		{		
		}


		private byte[] TXBitMasks = new byte[25]; 
		private byte[] RXBitMasks = new byte[25]; 


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
			if ( !tx && (int)band < 12)  // if !tx ignore given band and round off to nearest band based on freq 
			{ 
				band = Alex.AntBandFromFreq();
			} 

			int idx = (int)band - (int)Band.B160M; 
			int bits; 
			if ( idx < 0 || idx > 26 ) 
			{ 
				bits = 0; 
			} 
			else 
			{ 
				bits = tx ? TXBitMasks[idx] : RXBitMasks[idx];
			}
			System.Console.WriteLine("Bits: " + bits + " Band: " + (int)band); 
			JanusAudio.SetPennyOCBits(bits);
		}
	}
}
