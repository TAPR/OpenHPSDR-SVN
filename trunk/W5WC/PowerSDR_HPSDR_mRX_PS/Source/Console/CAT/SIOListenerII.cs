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

using System;
using System.Text;
using System.Windows.Forms; // needed for MessageBox (wjt)
using System.Text.RegularExpressions;
using System.IO.Ports;

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
            SDRSerialPort.serial_rx_event += new SerialRXEventHandler(SerialRXEventHandler);

            if (console.CATEnabled)  // if CAT is on fire it up 
            {
                try
                {
                    enableCAT();
                }
                catch (Exception ex)
                {
                    // fixme??? how cool is to to pop a msg box from an exception handler in a constructor ?? 
                    //  seems ugly to me (wjt) 
                    console.CATEnabled = false;
                    if (console.SetupForm != null)
                    {
                        console.SetupForm.copyCATPropsToDialogVars(); // need to make sure the props on the setup page get reset 
                    }
                    MessageBox.Show("Could not initialize CAT control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT control has been disabled.", "Error Initializing CAT control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

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
 
        public bool UseForCATPTT
        {
            set
            {
                if (SIO != null)
                    SIO.UseForCATPTT = value;
            }
        }
        
        public bool UseForKeyPTT
        {
            set
            {
                if (SIO != null)
                    SIO.UseForKeyPTT = value;
            }
        }

        public bool UseForPaddles
        {
            set
            {
                if (SIO != null)
                    SIO.UseForPaddles = value;
            }
        }

        public bool PTTOnDTR
        {
            set
            {
                if (SIO != null)
                    SIO.PTTOnDTR = value;
            }
        }

        public bool PTTOnRTS
        {
            set
            {
                if (SIO != null)
                    SIO.PTTOnRTS = value;
            }
        }

        public bool KeyOnDTR
        {
            set
            {
                if (SIO != null)
                    SIO.KeyOnDTR = value;
            }
        }

        public bool KeyOnRTS
        {
            set
            {
                if (SIO != null)
                    SIO.KeyOnRTS = value;
            }
        }
        
        // typically called when the end user has disabled CAT control through a UI element ... this 
		// closes the serial port and neutralized the listeners we have in place
		public void disableCAT() 
		{
			lock ( this ) 
			{
				if ( !cat_enabled )  return; // nothing to do already disabled  
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
				
		public SDRSerialPort SIO;

		Console console;
		ASCIIEncoding AE = new ASCIIEncoding();
		private bool Fpass = true;
		private bool cat_enabled = false;  // is cat currently enabled by user?

//		private System.Timers.Timer SIOMonitor;
		CATParser parser;		
//		private int SIOMonitorCount = 0;

		#endregion variables

		#region Methods

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

        StringBuilder CommBuffer = new StringBuilder();//"";				//holds incoming serial data from the port
        private void SerialRXEventHandler(object source, SerialRXEvent e)
        {
            //			SIOMonitor.Interval = 5000;		// set the timer for 5 seconds
            //			SIOMonitor.Enabled = true;		// start or restart the timer

            //double T0 = 0.00;
            //double T1 = 0.00;
            //int bufferLen = 0;

            CommBuffer.Append(e.buffer);                                		// put the data in the string
            if (parser != null)													// is the parser instantiated
            {
                //bufferLen = CommBuffer.Length;
                try
                {
                    Regex rex = new Regex(".*?;");										//accept any string ending in ;
                    string answer;
                    uint result;

                    for (Match m = rex.Match(CommBuffer.ToString()); m.Success; m = m.NextMatch())	//loop thru the buffer and find matches
                    {
                        //testTimer1.Start();
                        answer = parser.Get(m.Value);                                   //send the match to the parser
                        //testTimer1.Stop();
                        //T0 = testTimer1.DurationMsec;
                        //testTimer2.Start();
                        if (answer.Length > 0)
                            result = SIO.put(answer);                           		//send the answer to the serial port
                        //testTimer2.Stop();
                        //T1 = testTimer2.DurationMsec;
                        CommBuffer = CommBuffer.Replace(m.Value, "", 0, m.Length);                   //remove the match from the buffer
                        //Debug.WriteLine("Parser decode time for "+m.Value.ToString()+":  "+T0.ToString()+ "ms");
                        //Debug.WriteLine("SIO send answer time:  " + T1.ToString() + "ms");
                        //Debug.WriteLine("CommBuffer Length:  " + bufferLen.ToString());
                        //if (bufferLen > 100)
                        //Debug.WriteLine("Buffer contents:  "+CommBuffer.ToString());
                    }
                }
                catch (Exception)
                {
                    //Add ex name to exception above to enable
                    //Debug.WriteLine("RX Event:  "+ex.Message);
                    //Debug.WriteLine("RX Event:  "+ex.StackTrace);
                }
            }
        }

		#endregion Events
	}

    public class SIO2ListenerII
    {
        #region Constructor

        public SIO2ListenerII(Console c)
        {
            console = c;
            console.Activated += new EventHandler(console_Activated);
            console.Closing += new System.ComponentModel.CancelEventHandler(console_Closing);
            parser = new CATParser(console);

            //event handler for Serial RX Events
            SDRSerialPort2.serial_rx_event += new SerialRXEventHandler(SerialRX2EventHandler);

            if (console.CAT2Enabled)  // if CAT is on fire it up 
            {
                try
                {
                    enableCAT2();
                }
                catch (Exception ex)
                {
                    // fixme??? how cool is to to pop a msg box from an exception handler in a constructor ?? 
                    //  seems ugly to me (wjt) 
                    console.CAT2Enabled = false;
                    if (console.SetupForm != null)
                    {
                        console.SetupForm.copyCAT2PropsToDialogVars(); // need to make sure the props on the setup page get reset 
                    }
                    MessageBox.Show("Could not initialize CAT control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT control has been disabled.", "Error Initializing CAT control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        public void enableCAT2()
        {
            lock (this)
            {
                if (cat2_enabled) return; // nothing to do already enabled 
                cat2_enabled = true;
            }
            int port_num = console.CAT2Port;
            SIO2 = new SDRSerialPort2(port_num);
            SIO2.setCommParms(console.CAT2BaudRate,
                            console.CAT2Parity,
                            console.CAT2DataBits,
                            console.CAT2StopBits);

            Initialize();
        }

        public bool UseForKeyPTT
        {
            set
            {
                if (SIO2 != null)
                    SIO2.UseForKeyPTT = value;
            }
        }

        public bool UseForPaddles
        {
            set
            {
                if (SIO2 != null)
                    SIO2.UseForPaddles = value;
            }
        }

        public bool PTTOnDTR
        {
            set
            {
                if (SIO2 != null)
                    SIO2.PTTOnDTR = value;
            }
        }

        public bool PTTOnRTS
        {
            set
            {
                if (SIO2 != null)
                    SIO2.PTTOnRTS = value;
            }
        }

        public bool KeyOnDTR
        {
            set
            {
                if (SIO2 != null)
                    SIO2.KeyOnDTR = value;
            }
        }

        public bool KeyOnRTS
        {
            set
            {
                if (SIO2 != null)
                    SIO2.KeyOnRTS = value;
            }
        }

        // typically called when the end user has disabled CAT control through a UI element ... this 
        // closes the serial port and neutralized the listeners we have in place
 
        public void disableCAT2()
        {
            lock (this)
            {
                if (!cat2_enabled) return;
                cat2_enabled = false; 
            }

            if (SIO2 != null)
            {
                SIO2.Destroy();
                SIO2 = null;
            }
            Fpass = true; // reset init flag 
            return;
        }

        #endregion Constructor

        #region Variables

        public SDRSerialPort2 SIO2;

        Console console;
        ASCIIEncoding AE = new ASCIIEncoding();
        private bool Fpass = true;
        private bool cat2_enabled = false;
 
        //		private System.Timers.Timer SIOMonitor;
        CATParser parser;
        //		private int SIOMonitorCount = 0;

        #endregion variables

        #region Methods

        // Called when the console is activated for the first time.  
        private void Initialize()
        {
            if (Fpass)
            {
                SIO2.Create();
                Fpass = false;
            }
        }

        #endregion Methods

        #region Events

        private void console_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SIO2 != null)
            {
                SIO2.Destroy();
            }
        }

        private void console_Activated(object sender, EventArgs e)
        {
            if (console.CAT2Enabled)
            {
                // Initialize();   // wjt enable CAT calls Initialize 
                enableCAT2();
            }
        }

        StringBuilder CommBuffer = new StringBuilder();//"";				//holds incoming serial data from the port
        private void SerialRX2EventHandler(object sender, SerialRXEvent e)
        {
            //			SIOMonitor.Interval = 5000;		// set the timer for 5 seconds
            //			SIOMonitor.Enabled = true;		// start or restart the timer

            //double T0 = 0.00;
            //double T1 = 0.00;
            //int bufferLen = 0;
           // SerialPort spL = (SerialPort)sender;
         //   SDRSerialPort2 spL = (SDRSerialPort2)sender;
           // if (!SIO2.IsOpen || spL.BasePort.PortName != SIO2.BasePort.PortName) return;

            CommBuffer.Append(e.buffer);                                		// put the data in the string
            if (parser != null)													// is the parser instantiated
            {
                //bufferLen = CommBuffer.Length;
                try
                {
                    Regex rex = new Regex(".*?;");										//accept any string ending in ;
                    string answer;
                    uint result;

                    for (Match m = rex.Match(CommBuffer.ToString()); m.Success; m = m.NextMatch())	//loop thru the buffer and find matches
                    {
                        //testTimer1.Start();
                        answer = parser.Get(m.Value);                                   //send the match to the parser
                        //testTimer1.Stop();
                        //T0 = testTimer1.DurationMsec;
                        //testTimer2.Start();
                        if (answer.Length > 0)
                            result = SIO2.put(answer);                           		//send the answer to the serial port
                        //testTimer2.Stop();
                        //T1 = testTimer2.DurationMsec;
                        CommBuffer = CommBuffer.Replace(m.Value, "", 0, m.Length);                   //remove the match from the buffer
                        //Debug.WriteLine("Parser decode time for "+m.Value.ToString()+":  "+T0.ToString()+ "ms");
                        //Debug.WriteLine("SIO2 send answer time:  " + T1.ToString() + "ms");
                        //Debug.WriteLine("CommBuffer Length:  " + bufferLen.ToString());
                        //if (bufferLen > 100)
                        //Debug.WriteLine("Buffer contents:  "+CommBuffer.ToString());
                    }
                }
                catch (Exception)
                {
                    //Add ex name to exception above to enable
                    //Debug.WriteLine("RX Event:  "+ex.Message);
                    //Debug.WriteLine("RX Event:  "+ex.StackTrace);
                }
            }
        }

        #endregion Events
    }

    public class SIO3ListenerII
    {
        #region Constructor

        public SIO3ListenerII(Console c)
        {
            console = c;
            console.Activated += new EventHandler(console_Activated);
            console.Closing += new System.ComponentModel.CancelEventHandler(console_Closing);
            parser = new CATParser(console);

            //event handler for Serial RX Events
            SDRSerialPort3.serial_rx_event += new SerialRXEventHandler(SerialRX3EventHandler);

            if (console.CAT3Enabled)  // if CAT is on fire it up 
            {
                try
                {
                    enableCAT3();
                }
                catch (Exception ex)
                {
                    // fixme??? how cool is to to pop a msg box from an exception handler in a constructor ?? 
                    //  seems ugly to me (wjt) 
                    console.CAT3Enabled = false;
                    if (console.SetupForm != null)
                    {
                        console.SetupForm.copyCAT3PropsToDialogVars(); // need to make sure the props on the setup page get reset 
                    }
                    MessageBox.Show("Could not initialize CAT control.  Exception was:\n\n " + ex.Message +
                        "\n\nCAT control has been disabled.", "Error Initializing CAT control",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
 
        }

        public void enableCAT3()
        {
            lock (this)
            {
                if (cat3_enabled) return; // nothing to do already enabled 
                cat3_enabled = true;
            }
            int port_num = console.CAT3Port;
            SIO3 = new SDRSerialPort3(port_num);
            SIO3.setCommParms(console.CAT3BaudRate,
                            console.CAT3Parity,
                            console.CAT3DataBits,
                            console.CAT3StopBits);

            Initialize();
        }

        public bool UseForKeyPTT
        {
            set
            {
                if (SIO3 != null)
                    SIO3.UseForKeyPTT = value;
            }
        }

        public bool UseForPaddles
        {
            set
            {
                if (SIO3 != null)
                    SIO3.UseForPaddles = value;
            }
        }

        public bool PTTOnDTR
        {
            set
            {
                if (SIO3 != null)
                    SIO3.PTTOnDTR = value;
            }
        }

        public bool PTTOnRTS
        {
            set
            {
                if (SIO3 != null)
                    SIO3.PTTOnRTS = value;
            }
        }

        public bool KeyOnDTR
        {
            set
            {
                if (SIO3 != null)
                    SIO3.KeyOnDTR = value;
            }
        }

        public bool KeyOnRTS
        {
            set
            {
                if (SIO3 != null)
                    SIO3.KeyOnRTS = value;
            }
        }

        // typically called when the end user has disabled CAT control through a UI element ... this 
        // closes the serial port and neutralized the listeners we have in place
 
        public void disableCAT3()
        {
            lock (this)
            {
                if (!cat3_enabled) return;
                cat3_enabled = false; 
            }

            if (SIO3 != null)
            {
                SIO3.Destroy();
                SIO3 = null;
            }
            Fpass = true; // reset init flag 
            return;
        }

        #endregion Constructor

        #region Variables

        public SDRSerialPort3 SIO3;

        Console console;
        ASCIIEncoding AE = new ASCIIEncoding();
        private bool Fpass = true;
        private bool cat3_enabled = false;

        //		private System.Timers.Timer SIOMonitor;
        CATParser parser;
        //		private int SIOMonitorCount = 0;

        #endregion variables

        #region Methods

        // Called when the console is activated for the first time.  
        private void Initialize()
        {
            if (Fpass)
            {
                SIO3.Create();
                Fpass = false;
            }
        }

        #endregion Methods

        #region Events

        private void console_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SIO3 != null)
            {
                SIO3.Destroy();
            }
        }

        private void console_Activated(object sender, EventArgs e)
        {
            if (console.CAT3Enabled)
            {
                // Initialize();   // wjt enable CAT calls Initialize 
                enableCAT3();
            }
        }

        StringBuilder CommBuffer = new StringBuilder();//"";			//holds incoming serial data from the port
        private void SerialRX3EventHandler(object source, SerialRXEvent e)
        {
            //			SIOMonitor.Interval = 5000;		// set the timer for 5 seconds
            //			SIOMonitor.Enabled = true;		// start or restart the timer

            //double T0 = 0.00;
            //double T1 = 0.00;
            //int bufferLen = 0;

            CommBuffer.Append(e.buffer);                                		// put the data in the string
            if (parser != null)													// is the parser instantiated
            {
                //bufferLen = CommBuffer.Length;
                try
                {
                    Regex rex = new Regex(".*?;");										//accept any string ending in ;
                    string answer;
                    uint result;

                    for (Match m = rex.Match(CommBuffer.ToString()); m.Success; m = m.NextMatch())	//loop thru the buffer and find matches
                    {
                        //testTimer1.Start();
                        answer = parser.Get(m.Value);                                   //send the match to the parser
                        //testTimer1.Stop();
                        //T0 = testTimer1.DurationMsec;
                        //testTimer2.Start();
                        if (answer.Length > 0)
                            result = SIO3.put(answer);                           		//send the answer to the serial port
                        //testTimer2.Stop();
                        //T1 = testTimer2.DurationMsec;
                        CommBuffer = CommBuffer.Replace(m.Value, "", 0, m.Length);                   //remove the match from the buffer
                        //Debug.WriteLine("Parser decode time for "+m.Value.ToString()+":  "+T0.ToString()+ "ms");
                        //Debug.WriteLine("SIO3 send answer time:  " + T1.ToString() + "ms");
                        //Debug.WriteLine("CommBuffer Length:  " + bufferLen.ToString());
                        //if (bufferLen > 100)
                        //Debug.WriteLine("Buffer contents:  "+CommBuffer.ToString());
                    }
                }
                catch (Exception)
                {
                    //Add ex name to exception above to enable
                    //Debug.WriteLine("RX Event:  "+ex.Message);
                    //Debug.WriteLine("RX Event:  "+ex.StackTrace);
                }
            }
        }

        #endregion Events
    }
    public class SIO4ListenerII
    {
        #region Constructor

        public SIO4ListenerII(Console c)
        {
            console = c;
            console.Activated += new EventHandler(console_Activated);
            console.Closing += new System.ComponentModel.CancelEventHandler(console_Closing);
            parser = new CATParser(console);

            //event handler for Serial RX Events
            SDRSerialPort4.serial_rx_event += new SerialRXEventHandler(SerialRX4EventHandler);

               if (console.CAT4Enabled)  // if CAT is on fire it up 
               {
                   try
                   {
                       enableCAT4();
                   }
                   catch (Exception ex)
                   {
                       // fixme??? how cool is to to pop a msg box from an exception handler in a constructor ?? 
                       //  seems ugly to me (wjt) 
                       console.CAT4Enabled = false;
                       if (console.SetupForm != null)
                       {
                           console.SetupForm.copyCAT4PropsToDialogVars(); // need to make sure the props on the setup page get reset 
                       }
                       MessageBox.Show("Could not initialize CAT control.  Exception was:\n\n " + ex.Message +
                           "\n\nCAT control has been disabled.", "Error Initializing CAT control",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
               } 
        }

            public void enableCAT4()
           {
               lock (this)
               {
                   if (cat4_enabled) return; // nothing to do already enabled 
                   cat4_enabled = true;
               }
               int port_num = console.CAT4Port;
               SIO4 = new SDRSerialPort4(port_num);
               SIO4.setCommParms(console.CAT4BaudRate,
                               console.CAT4Parity,
                               console.CAT4DataBits,
                               console.CAT4StopBits);

               Initialize();
           } 

        public bool UseForKeyPTT
        {
            set
            {
                if (SIO4 != null)
                    SIO4.UseForKeyPTT = value;
            }
        }

        public bool UseForPaddles
        {
            set
            {
                if (SIO4 != null)
                    SIO4.UseForPaddles = value;
            }
        }

        public bool PTTOnDTR
        {
            set
            {
                if (SIO4 != null)
                    SIO4.PTTOnDTR = value;
            }
        }

        public bool PTTOnRTS
        {
            set
            {
                if (SIO4 != null)
                    SIO4.PTTOnRTS = value;
            }
        }

        public bool KeyOnDTR
        {
            set
            {
                if (SIO4 != null)
                    SIO4.KeyOnDTR = value;
            }
        }

        public bool KeyOnRTS
        {
            set
            {
                if (SIO4 != null)
                    SIO4.KeyOnRTS = value;
            }
        }

        // typically called when the end user has disabled CAT control through a UI element ... this 
        // closes the serial port and neutralized the listeners we have in place
  
          public void disableCAT4()
          {
              lock (this)
              {
                  if (!cat4_enabled) return; 
                  cat4_enabled = false;
              }

              if (SIO4 != null)
              {
                  SIO4.Destroy();
                  SIO4 = null;
              }
              Fpass = true; // reset init flag 
              return;
          } 

        #endregion Constructor

        #region Variables

        public SDRSerialPort4 SIO4;

        Console console;
        ASCIIEncoding AE = new ASCIIEncoding();
        private bool Fpass = true;
        private bool cat4_enabled = false;

        //		private System.Timers.Timer SIOMonitor;
        CATParser parser;
        //		private int SIOMonitorCount = 0;

        #endregion variables

        #region Methods

        // Called when the console is activated for the first time.  
        private void Initialize()
        {
            if (Fpass)
            {
                SIO4.Create();
                Fpass = false;
            }
        }

        #endregion Methods

        #region Events

        private void console_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SIO4 != null)
            {
                SIO4.Destroy();
            }
        }

        private void console_Activated(object sender, EventArgs e)
        {
            if (console.CAT4Enabled)
            {
                // Initialize();   // wjt enable CAT calls Initialize 
                enableCAT4();
            }
        }

        StringBuilder CommBuffer = new StringBuilder();//"";				//holds incoming serial data from the port
        private void SerialRX4EventHandler(object source, SerialRXEvent e)
        {
            //			SIOMonitor.Interval = 5000;		// set the timer for 5 seconds
            //			SIOMonitor.Enabled = true;		// start or restart the timer

            //double T0 = 0.00;
            //double T1 = 0.00;
            //int bufferLen = 0;

            CommBuffer.Append(e.buffer);                                		// put the data in the string
            if (parser != null)													// is the parser instantiated
            {
                //bufferLen = CommBuffer.Length;
                try
                {
                    Regex rex = new Regex(".*?;");										//accept any string ending in ;
                    string answer;
                    uint result;

                    for (Match m = rex.Match(CommBuffer.ToString()); m.Success; m = m.NextMatch())	//loop thru the buffer and find matches
                    {
                        //testTimer1.Start();
                        answer = parser.Get(m.Value);                                   //send the match to the parser
                        //testTimer1.Stop();
                        //T0 = testTimer1.DurationMsec;
                        //testTimer2.Start();
                        if (answer.Length > 0)
                            result = SIO4.put(answer);                           		//send the answer to the serial port
                        //testTimer2.Stop();
                        //T1 = testTimer2.DurationMsec;
                        CommBuffer = CommBuffer.Replace(m.Value, "", 0, m.Length);                   //remove the match from the buffer
                        //Debug.WriteLine("Parser decode time for "+m.Value.ToString()+":  "+T0.ToString()+ "ms");
                        //Debug.WriteLine("SIO4 send answer time:  " + T1.ToString() + "ms");
                        //Debug.WriteLine("CommBuffer Length:  " + bufferLen.ToString());
                        //if (bufferLen > 100)
                        //Debug.WriteLine("Buffer contents:  "+CommBuffer.ToString());
                    }
                }
                catch (Exception)
                {
                    //Add ex name to exception above to enable
                    //Debug.WriteLine("RX Event:  "+ex.Message);
                    //Debug.WriteLine("RX Event:  "+ex.StackTrace);
                }
            }
        }

        #endregion Events
    }

}

