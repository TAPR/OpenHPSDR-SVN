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
using SerialPorts; 

namespace SDRSerialSupportII
{
	public class SDRSerialPort
	{
		public static event SDRSerialSupportII.SerialRXEventHandler serial_rx_event;
		
		private SerialPort commPort; 		
		private bool isOpen = false; 
		private bool bitBangOnly = false; 
						
		public enum Parity
		{
			FIRST = -1,
			NONE, ODD, EVEN, MARK, SPACE
		}

		public static Parity stringToParity(string s) 
		{
			if ( s == "none" ) return Parity.NONE; 
			if ( s == "odd" ) return Parity.ODD;
			if ( s == "even" ) return Parity.EVEN; 
			if ( s == "space" ) return Parity.SPACE; 
			if ( s == "mark" ) return Parity.MARK; 
			return Parity.NONE;  // error -- default to none
		}
		
		public enum StopBits { FIRST=-1, ONE, ONE_AND_HALF, TWO }

		public static StopBits stringToStopBits(string s) 
		{
			if ( s == "1" ) return StopBits.ONE; 
			if ( s == "1.5" ) return StopBits.ONE_AND_HALF; 
			if ( s == "2" ) return StopBits.TWO; 
			return StopBits.ONE; // error -- default 
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
			commPort.ErrorEvent += new SerialPorts.SerialEventHandler(this.SerialPortErrorEvent);
			commPort.ReceivedEvent += new SerialPorts.SerialEventHandler(this.SerialPortReceivedEvent);
			commPort.PinChangedEvent += new SerialPorts.SerialEventHandler(this.SerialPortPinChangedEvent);

			commPort.PortName = "COM" + portidx.ToString(); 

			commPort.Parity = SerialPorts.Parity.None; 
			commPort.StopBits = SerialPorts.StopBits.One;
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
			
			switch ( p ) 
			{ 
				case Parity.NONE: 
					commPort.Parity = SerialPorts.Parity.None; 
					break; 
				case Parity.ODD: 
					commPort.Parity = SerialPorts.Parity.Odd; 
					break; 
				case Parity.EVEN:
					commPort.Parity = SerialPorts.Parity.Even; 
					break; 
				case Parity.MARK: 
					commPort.Parity = SerialPorts.Parity.Mark; 
					break; 
				case Parity.SPACE:
					commPort.Parity = SerialPorts.Parity.Space; 
					break; 
				default: 
					commPort.Parity = SerialPorts.Parity.None; 
					break; 
			}
									
			switch ( stop ) 
			{
				case StopBits.ONE: 
					commPort.StopBits = SerialPorts.StopBits.One;  
					break; 
				case StopBits.ONE_AND_HALF: 
					commPort.StopBits = SerialPorts.StopBits.OnePointFive; 
					break; 
				case StopBits.TWO: 
					commPort.StopBits = SerialPorts.StopBits.Two; 
					break; 
				default: 
					commPort.StopBits = SerialPorts.StopBits.One; 
					break; 
			}
			
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

		void SerialPortErrorEvent(object source, SerialPorts.SerialEventArgs e)
		{
			
		}
		
		void SerialPortPinChangedEvent(object source, SerialPorts.SerialEventArgs e)
		{
			
		}	
		
		void SerialPortReceivedEvent(object source, SerialPorts.SerialEventArgs e)
		{
			int num_to_read = commPort.InBufferBytes;
			byte[] inbuf = new byte[num_to_read];
			commPort.Read(inbuf, 0, num_to_read);
			serial_rx_event(this, new SDRSerialSupportII.SerialRXEvent(inbuf, (uint)num_to_read));
		}

	}
}
