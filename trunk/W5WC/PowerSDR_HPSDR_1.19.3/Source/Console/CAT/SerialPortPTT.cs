//=================================================================
// SerialPortPTT.cs
//=================================================================
// Copyright (C) 2005  Bill Tracey
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//=================================================================
// This class is used to implement a PTT using RTS or DTS 
//=================================================================

#define DBG_PRINT

using System;
using SDRSerialSupportII; 

namespace PowerSDR
{
	/// <summary>
	/// Summary description for SerialPortPTT.
	/// </summary>
	public class SerialPortPTT
	{

		// instance vars 
		// 
		private int portNum = 0; 
		private bool rtsIsPTT = false; 
		public bool RTSIsPTT 
		{
			get {return rtsIsPTT;}
			set {rtsIsPTT = value;}
		}
		private bool dtrIsPTT = false; 

		public bool DTRIsPTT 
		{
			get {return dtrIsPTT;}
			set {dtrIsPTT = value;}
		}
		private SDRSerialPort commPort = null; 
		private bool Initialized = false; 

		//
		// 
		//

		public SerialPortPTT(int portidx, bool rts_is_ptt, bool dtr_is_ptt)
		{
			portNum = portidx; 
			rtsIsPTT = rts_is_ptt; 
			dtrIsPTT = dtr_is_ptt; 
		}

		public void Init() 
		{ 
			lock ( this )  // do this only once -- keep the lock until we're ready to go less we hose up the poll ptt thread 
			{
				if ( Initialized ) return;  							
				if ( portNum == 0 ) return; // bail out 
				commPort = new SDRSerialPort(portNum); 
				commPort.Create(true); // true says to create bit bang only port  -- fixme needs error checking! 
				Initialized = true; 
			}
			return; 
		}

		public bool isPTT() 
		{
			if ( !Initialized ) return false; 
			if ( rtsIsPTT && commPort.isCTS() ) return true; 
			if ( dtrIsPTT && commPort.isDSR() ) return true; 
			return false; 
		}

		public bool isCTS() 
		{
			return commPort.isCTS(); 
		}

		public bool isDSR() 
		{ 
			return commPort.isDSR(); 
		}

		public void setDTR(bool v) 
		{
			commPort.setDTR(v); 
		}
		
		public void Destroy() 
		{ 
			lock ( this )  // we only get in here once 
			{ 
				if ( !Initialized ) return; 
				Initialized = false; 
			}
			if ( commPort != null ) 
			{ 
				commPort.Destroy(); 
				commPort = null; 
			}
		}
	}
}