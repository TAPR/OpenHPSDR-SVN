//=================================================================
// SIOListener.cs
//=================================================================
// Copyright (C) 2005  Bob Tracy
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
//
// You may contact the author via email at: k5kdn@arrl.net
//=================================================================

#define DBG_PRINT

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Forms; // needed for MessageBox (wjt)
using System.Text.RegularExpressions;
using System.Diagnostics;
using SDRSerialSupportII;

namespace PowerSDR
{	
	public class SIOListenerII
	{
		#region Constructor

		public SIOListenerII(Console c)
		{
			console = c;
			console.Activated += new EventHandler(console_Activated);
			console.Closing += new System.ComponentModel.CancelEventHandler(console_Closing);
			parser = new CATParser(console);

			//event handler for Serial RX Events
			SDRSerialSupportII.SDRSerialPort.serial_rx_event += new SDRSerialSupportII.SerialRXEventHandler(SerialRXEventHandler);
		
			if ( console.CATEnabled )  // if CAT is on fire it up 
			{ 
				try 
				{ 
					enableCAT();  
				}
				catch ( Exception ex ) 
				{					
					// fixme??? how cool is to to pop a msg box from an exception handler in a constructor ?? 
					//  seems ugly to me (wjt) 
					console.CATEnabled = false; 
					if ( console.SetupForm != null ) 
					{ 
						console.SetupForm.copyCATPropsToDialogVars(); // need to make sure the props on the setup page get reset 
					}
					MessageBox.Show("Could not initialize CAT control.  Exception was:\n\n " + ex.Message + 
						"\n\nCAT control has been disabled.", "Error Initializing CAT control", 
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
//			SIOMonitor = new System.Timers.Timer();
//			SIOMonitor.Elapsed+=new
//				System.Timers.ElapsedEventHandler(SIOMonitor_Elapsed);
//
//			SIOMonitor.Interval = 60000;
//			SIOMonitor.Start();
//
//			if(!File.Exists("SIOTestLog.txt"))
//				File.Create("SIOTestLog.txt");

		}

//		private void SIOMonitor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
//		{
//			if(!console.MOX) 
//			{
//				SIOMonitorCount++;			// increments the counter when in receive
//				Debug.WriteLine("SIOMonitorCount: "+SIOMonitorCount.ToString());
//				if(SIOMonitorCount < 12)	// if the counter is less than 12 (60 seconds),reinitialize the serial port
//				{
//					Debug.WriteLine("The SIO Timer has elapsed");			
//					if(!SIO.PortIsOpen)
//					{
//						Debug.WriteLine("The port is closed");
//						SIO.OpenPort = true;
//						StreamWriter sw = new StreamWriter("SIOTestLog.txt", true);
//						sw.WriteLine("The serial port was forced open");
//						sw.WriteLine("Time: "+DateTime.Now.ToString());
//						sw.Flush();
//						sw.Close();
//						SIOMonitor.Enabled = true;
//						SIOMonitorCount = 0;
//						Fpass = true;
//						disableCAT();
//						enableCAT();
//					}
//					//Initialize();
//				}
//				else						// consider the remote program on the serial port as being shut down
//				{
//					if(SIO.PortIsOpen)
//					{
//						port_status = "open";
//					}
//					else
//					{
//
//						port_status = "closed";
//					}
//
//
//					SIOMonitorCount = 0;
//					disableCAT();
//					enableCAT();
//	//				SIOMonitor.Stop();
//				}
//			}
//			else
//				SIOMonitorCount = 0;
//		}

		public void enableCAT() 
		{
			lock ( this ) 
			{
				if ( cat_enabled ) return; // nothing to do already enabled 
				cat_enabled = true; 
			}
			int port_num = console.CATPort; 
			SIO = new SDRSerialPort(port_num);
			SIO.setCommParms(console.CATBaudRate, 
							console.CATParity, 
							console.CATDataBits, 
							console.CATStopBits); 
		
			Initialize();	
		}

		// typically called when the end user has disabled CAT control through a UI element ... this 
		// closes the serial port and neutralized the listeners we have in place
		public void disableCAT() 
		{
			lock ( this ) 
			{
				if ( !cat_enabled )  return; /* nothing to do already disabled */ 
				cat_enabled = false; 
			}

			if ( SIO != null ) 
			{
				SIO.Destroy(); 
				SIO = null; 
			}
			Fpass = true; // reset init flag 
			return; 									
		}

		#endregion Constructor

		#region Variables
				
		public SDRSerialSupportII.SDRSerialPort SIO; 
		Console console;
		ASCIIEncoding AE = new ASCIIEncoding();
		private bool Fpass = true;
		private bool cat_enabled = false;  // is cat currently enabled by user? 
//		private System.Timers.Timer SIOMonitor;
		CATParser parser;		
//		private int SIOMonitorCount = 0;

		#endregion variables

		#region Methods

		private static void dbgWriteLine(string s) 
		{ 
#if(!DBG_PRINT) 
			Console.dbgWriteLine("SIOListener: " + s); 
#endif
		}

		// Called when the console is activated for the first time.  
		private void Initialize()
		{	
			if(Fpass)
			{
				SIO.Create();
				Fpass = false;
			}
		}		
#if UseParser
		private char[] ParseLeftover = null; 

		// segment incoming string into CAT commands ... handle leftovers from when we read a parial 
		// 
		private void ParseString(byte[] rxdata, uint count) 
		{ 
			if ( count == 0 ) return;  // nothing to do 
			int cmd_char_count = 0; 
			int left_over_char_count = ( ParseLeftover == null ? 0 : ParseLeftover.Length ); 
			char[] cmd_chars = new char[count + left_over_char_count]; 			
			if ( ParseLeftover != null )  // seed with leftovers from last read 
			{ 
				for ( int j = 0; j < left_over_char_count; j++ )  // wjt fixme ... use C# equiv of System.arraycopy 
				{
					cmd_chars[cmd_char_count] = ParseLeftover[j]; 
					++cmd_char_count; 
				}
				ParseLeftover = null; 
			}
			for ( int j = 0; j < count; j++ )   // while we have chars to play with 
			{ 
				cmd_chars[cmd_char_count] = (char)rxdata[j]; 
				++cmd_char_count; 
				if ( rxdata[j] == ';' )  // end of cmd -- parse it and execute it 
				{ 
					string cmdword = new String(cmd_chars, 0, cmd_char_count); 
					dbgWriteLine("cmdword: >" + cmdword + "<");  
					// BT 06/08
					string answer = parser.Get(cmdword);
					byte[] out_string = AE.GetBytes(answer);
					uint result = SIO.put(out_string, (uint) out_string.Length);

					cmd_char_count = 0; // reset word counter 
				}
			} 
			// when we get here have processed all of the incoming buffer, if there's anyting 
			// in cmd_chars we need to save it as we've not pulled a full command so we stuff 
			// it in leftover for the next time we come through 
			if ( cmd_char_count != 0 ) 
			{ 
				ParseLeftover = new char[cmd_char_count]; 
				for ( int j = 0; j < cmd_char_count; j++ )  // wjt fixme ... C# equiv of Sytsem.arraycopy 
				{
					ParseLeftover[j] = cmd_chars[j]; 
				}
			} 
#if DBG_PRINT
			if ( ParseLeftover != null) 
			{
				dbgWriteLine("Leftover >" + new String(ParseLeftover) + "<"); 
			}
#endif
			return; 
		}

#endif

		#endregion Methods

		#region Events

		private void console_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( SIO != null ) 
			{ 
				SIO.Destroy(); 
			}
		}

		private void console_Activated(object sender, EventArgs e)
		{
			if ( console.CATEnabled ) 
			{ 
				// Initialize();   // wjt enable CAT calls Initialize 
				enableCAT(); 
			}
		}

		string CommBuffer = "";				//holds incoming serial data from the port
		private void SerialRXEventHandler(object source, SDRSerialSupportII.SerialRXEvent e)
		{
//			SIOMonitor.Interval = 5000;		// set the timer for 5 seconds
//			SIOMonitor.Enabled = true;		// start or restart the timer

			CommBuffer += AE.GetString(e.buffer,0,e.buffer.Length);				//put the data in the string
			if(parser != null)													//is the parser instantiated
			{
				try
				{
					Regex rex = new Regex(".*?;");										//accept any string ending in ;
					string answer;
					byte[] out_string;
					uint result;
					for(Match m = rex.Match(CommBuffer); m.Success; m = m.NextMatch())	//loop thru the buffer and find matches
					{
						answer = parser.Get(m.Value);									//send the match to the parser
						out_string = AE.GetBytes(answer);								//get the answer from the parser
						result = SIO.put(out_string, (uint) out_string.Length);			//send the answer to the serial port
						CommBuffer = CommBuffer.Replace(m.Value, "");					//remove the match from the buffer
					}
				}
				catch(Exception)
				{
					//Add ex name to exception above to enable
//					Debug.WriteLine("RX Event:  "+ex.Message);
//					Debug.WriteLine("RX Event:  "+ex.StackTrace);
				}
			}
		}


		#endregion Events
	}
}

