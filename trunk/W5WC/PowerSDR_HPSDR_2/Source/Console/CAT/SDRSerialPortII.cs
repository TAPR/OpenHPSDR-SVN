//=================================================================
// SDRSerialPort.cs
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
// Serial port support for PowerSDR support of CAT and serial port control  
//=================================================================

#define DBG_PRINT

using System;
using System.Threading;
using System.IO.Ports; 

namespace SDRSerialSupportII
{
	public class SDRSerialPort
	{
		public static event SDRSerialSupportII.SerialRXEventHandler serial_rx_event;
		
		private SerialPort commPort; 		
		private bool isOpen = false; 
		private bool bitBangOnly = false; 

		//Added 2/14/2008 BT
		public bool PortIsOpen
		{
			get{return commPort.IsOpen;}
		}

		public bool OpenPort
		{
			set
			{
				if(value)
					commPort.Open();
			}
		}

		public bool ClosePort
		{
			set
			{
				if(value)
					commPort.Close();
			}
		}

		public static Parity stringToParity(string s) 
		{
			if (s == "none") return Parity.None; 
			if (s == "odd") return Parity.Odd;
			if (s == "even") return Parity.Even; 
			if (s == "space") return Parity.Space; 
			if (s == "mark") return Parity.Mark; 
			return Parity.None;  // error -- default to none
		}

		public static StopBits stringToStopBits(string s) 
		{
            if (s == "0") return StopBits.None;
			if (s == "1") return StopBits.One; 
			if (s == "1.5") return StopBits.OnePointFive; 
			if (s == "2") return StopBits.Two; 
			return StopBits.One; // error -- default 
		}

		public enum DataBits { FIRST=-1, EIGHT, SEVEN, SIX } 

		public static DataBits stringToDataBits(string s) 
		{
			if ( s == "8" ) return DataBits.EIGHT; 
			if ( s == "7" ) return DataBits.SEVEN; 
			if ( s == "6" ) return DataBits.SIX; 
			return DataBits.EIGHT; 
		}

		public SDRSerialPort(int portidx)
		{
			commPort = new SerialPort();
			commPort.Encoding = System.Text.Encoding.ASCII;
			commPort.RtsEnable = true; // kd5tfd hack for soft rock ptt 
			commPort.DtrEnable = false; // set dtr off 
            //commPort.ErrorReceived += new SerialErrorReceivedEventHandler(this.SerialErrorReceived);
            commPort.DataReceived += new SerialDataReceivedEventHandler(this.SerialReceivedData);
			commPort.PinChanged += new SerialPinChangedEventHandler(this.SerialPinChanged);

			commPort.PortName = "COM" + portidx.ToString(); 

			commPort.Parity = Parity.None; 
			commPort.StopBits = StopBits.One;
			commPort.DataBits = 8; 
			commPort.BaudRate = 9600; 
			commPort.ReadTimeout = 5000;
			commPort.WriteTimeout = 500;	
			commPort.ReceivedBytesThreshold = 1;
		}
		// set the comm parms ... can only be done if port is not open -- silently fails if port is open (fixme -- add some error checking) 
		// 
		public void setCommParms(int baudrate, Parity p, DataBits data, StopBits stop)  
		{ 
			if ( commPort.IsOpen ) return; // bail out if it's already open 
			
			commPort.BaudRate = baudrate;
            commPort.Parity = p;
            commPort.StopBits = stop;						
			
			switch ( data ) 
			{
				case DataBits.EIGHT: 
					commPort.DataBits = 8; 
					break;
				case DataBits.SEVEN:
					commPort.DataBits = 7; 
					break; 
				case DataBits.SIX: 
					commPort.DataBits = 6; 
					break; 
				default: 
					commPort.DataBits = 8; 
					break; 
			}			
		}
		
		public uint put(byte[] b, uint count) 
		{
			if ( bitBangOnly ) return 0;  // fixme -- throw exception?			
			commPort.Write(b, 0, (int)count);			
			return count; // wjt fixme -- hack -- we don't know if we actually wrote things 			
		}

		public int Create()
		{
			return Create(false); 
		}

		// create port 
		public int Create(bool bit_bang_only) 
		{ 
			bitBangOnly = bit_bang_only; 
			if ( isOpen ){ return -1; }
			commPort.Open();  
			isOpen = commPort.IsOpen; 			
			if ( isOpen )
                return 0; // all is well
			else
				return -1;  //error
		}				  
				  
		public void Destroy()
		{
			try 
			{
				commPort.Close(); 
			}
			catch(Exception)
			{

			}
			isOpen = false;
		}

		public bool isCTS() 
		{ 		
			if ( !isOpen ) return false; // fixme error check 
			return commPort.CtsHolding; 			
		}

		public bool isDSR() 
		{
			if ( !isOpen ) return false; // fixme error check 
			return commPort.DsrHolding; 
			
		}
		public bool isRI()
		{
			if ( !isOpen ) return false; // fixme error check 
			return false; 
		}

		public bool isRLSD() 
		{
			if ( !isOpen ) return false; // fixme error check 
			return commPort.CDHolding; 
		}

		public void setDTR(bool v) 
		{ 
			if ( !isOpen ) return; 
			commPort.DtrEnable = v; 
		}	

		void SerialErrorReceived(object source, SerialErrorReceivedEventArgs e)
		{
			
		}

        void SerialPinChanged(object source, SerialPinChangedEventArgs e)
		{
			
		}	
		
		void SerialReceivedData(object source, SerialDataReceivedEventArgs e)
		{
			int num_to_read = commPort.BytesToRead;
			byte[] inbuf = new byte[num_to_read];
			commPort.Read(inbuf, 0, num_to_read);
			serial_rx_event(this, new SDRSerialSupportII.SerialRXEvent(inbuf, (uint)num_to_read));
		}

	}
}
