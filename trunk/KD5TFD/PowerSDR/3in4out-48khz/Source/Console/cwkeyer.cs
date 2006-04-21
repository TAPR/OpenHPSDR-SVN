/* 	This file is part of a program that implements a Software-Defined Radio.
	The code in this file is derived from routines originally written by
	Pierre-Philippe Coupard for his CWirc X-chat program. That program
	is issued under the GPL and is
	Copyright (C) Pierre-Philippe Coupard - 18/06/2003
	This derived version is
	Copyright (C) 2004 by Frank Brickle, AB2KT and Bob McGwier, N4HY

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

	The authors can be reached by email at

	ab2kt@arrl.net
	or
	rwmcgwier@comcast.net

	or by paper mail at

	The DTTS Microwave Society
	6 Kathleen Place
	Bridgewater, NJ 08807
*/

using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace PowerSDR
{
	public class CWKeyer2
	{
		#region Variables and Properties

		public Thread Keyer,CWTone;
		private HiPerfTimer timer;
		private float msdel;
		private Console console;
		//private bool comport = false;
		//private bool comport2 = false;

		private KeyerLine secondary_ptt_line = KeyerLine.NONE;
		public KeyerLine SecondaryPTTLine
		{ 
			get { return secondary_ptt_line; }
			set
			{
				secondary_ptt_line = value;
				//				switch(secondary_ptt_line)
				//				{
				//					case KeyerLine.NONE:
				//						keyerptt = false;
				//						break;
				//					case KeyerLine.DTR:
				//						keyerptt = true;
				//						break;
				//					case KeyerLine.RTS:
				//						keyerptt = true;
				//						break;
				//				}
			}
		}
		private SIOListenerII siolisten;
		public SIOListenerII Siolisten 
		{
			get { return siolisten; }
			set 
			{
				siolisten = value;
			}
		}
		private bool cat_enabled= false; 
		public bool CATEnabled 
		{
			set { cat_enabled = value; }
		}

		private KeyerLine secondary_key_line = KeyerLine.NONE;
		public KeyerLine SecondaryKeyLine
		{
			get { return secondary_key_line; }
			set
			{
				secondary_key_line = value;
				switch(secondary_key_line)
				{
					case KeyerLine.NONE:
						break;
					case KeyerLine.DTR:
						DttSP.SetWhichKey(1);
						SP2DotKey = true;
						break;
					case KeyerLine.RTS:
						SP2DotKey = false;
						DttSP.SetWhichKey(0);
						break;
				}
			}
		}

		private bool memorykey = false;
		public bool MemoryKey
		{
			get { return memorykey; }
			set {memorykey = value; }
		}

		private bool memoryptt = false;
		public bool MemoryPTT
		{
			get { return memoryptt; }
			set {memoryptt = value;}
		}
		private HW hw;
		public HW Hdw 
		{
			set { hw = value; }
			get { return hw ; }
		}

		private string primary_conn_port = "SDR";
		public string PrimaryConnPort
		{
			get { return primary_conn_port; }
			set
			{
				primary_conn_port = value;
				switch(primary_conn_port)
				{
					case "SDR":
						if (sp.IsOpen) sp.Close();	
						break;
					case "FPGA":  // fpga does not need to be opened or closed 
						break; 
					default:
						if (sp.IsOpen) sp.Close();
						sp.PortName = primary_conn_port;
						try
						{
							sp.Open();
							sp.DtrEnable=true;
							sp.RtsEnable=true;
						}
						catch(Exception) 
						{
							MessageBox.Show("Primary Keyer Port ["+primary_conn_port+"] could not be opened.");	
							primary_conn_port = "SDR";
						}
						break;
				}
			}
		}

		private string secondary_conn_port = "None";
		public string SecondaryConnPort
		{
			get { return secondary_conn_port; }
			set
			{
				secondary_conn_port = value;
				switch(secondary_conn_port)
				{
					case "None":
						if (sp2.IsOpen) sp2.Close();
						break;
					case "CAT":
						if(!cat_enabled)
							MessageBox.Show("CAT was selected for the Keyer Secondary Port, but CAT is not enabled.");

						break;
					default: // COMx
						if(sp2.IsOpen) sp2.Close();
						sp2.PortName = secondary_conn_port;
						try
						{
							sp2.Open();
							sp2.DtrEnable=true;
							sp2.RtsEnable=true;
						}
						catch(Exception) 
						{
							MessageBox.Show("Comport for keyer program could not be opened\n");
							secondary_conn_port = "None";
						}
						break;
				}
			}
		}
		
		private bool sp2dotkey = false;
		public bool SP2DotKey
		{
			set {sp2dotkey = value;}
			get {return sp2dotkey;}
		}

		private bool keyerptt = false;
		public bool KeyerPTT
		{
			get { return keyerptt; }
		}

		public SerialPorts.SerialPort sp = new SerialPorts.SerialPort(); 
		public SerialPorts.SerialPort sp2 = new SerialPorts.SerialPort();

		#endregion

		#region Constructor and Destructor

		public CWKeyer2(Console c)
		{
			console = c;
			hw = console.Hdw;
			siolisten = console.Siolisten;
			Thread.Sleep(50);
			DttSP.NewKeyer(600.0f, 1, 0.0f, 3.0f, 25.0f, (float)Audio.SampleRate1);
			Thread.Sleep(50);


			CWTone = new Thread(new ThreadStart(DttSP.KeyerSoundThread));
			CWTone.Name = "CW Sound Thread";
			CWTone.Priority = ThreadPriority.Highest;
			CWTone.IsBackground = true;
			CWTone.Start();
			
			Keyer  = new Thread(new ThreadStart(  KeyThread));
			Keyer.Name = "CW KeyThread";
			Keyer.Priority = ThreadPriority.Highest;
			Keyer.IsBackground = true;
			Keyer.Start();

			timer = new HiPerfTimer();			
		}
		
		 ~CWKeyer2()
		{
			// Destructor logic here, make sure threads cleaned up
			DttSP.StopKeyer();
			Thread.Sleep(10);
			CWTone.Suspend();
			Keyer.Suspend();
			Thread.Sleep(50);
			DttSP.DeleteKeyer();
		}

		#endregion

		#region Thread Functions

		public void KeyThread()
		{
			byte extkey_dash, extkey_dot;
			
			do 
			{
				DttSP.KeyerStartedWait();
				for(;DttSP.KeyerRunning()!=0;) 
				{
					byte keyprog=0;
					timer.Start();
					DttSP.PollTimerWait();
					switch(primary_conn_port)
					{
						case "SDR":
							byte b = hw.StatusPort();
							extkey_dash = (byte)(((b & (byte)StatusPin.Dash ) != 0) ? 1:0);
							extkey_dot  = (byte)(((b & (byte)StatusPin.Dot ) != 0) ? 1:0);
							break;

						case "FPGA": 
							int tmp = TFDAPHaudio.GetDotDash(); 
							// System.Console.WriteLine("dd: " + tmp); 
							extkey_dot = (byte)(tmp & 0x1); 
							extkey_dash = (byte)((tmp & 0x2) != 0 ? 1:0); 
							break; 

						default: // COM port
							extkey_dash = System.Convert.ToByte(sp.CtsHolding);
							extkey_dot  = System.Convert.ToByte(sp.DsrHolding);
							break;
					}

					if (memoryptt) 
					{
						//console ptt on
						keyprog = 1;
						extkey_dot = extkey_dash = System.Convert.ToByte(memorykey);
					} 
					else 
					{
						keyprog = 0;
						//console ptt off
					}
					switch(secondary_conn_port)
					{
						case "None":
							break;
						case "CAT":
							if ((extkey_dash==0) && (extkey_dot == 0)) // don't override primary
							{
								switch(secondary_ptt_line)
								{
									case KeyerLine.NONE:
										if (sp2dotkey) extkey_dash  = System.Convert.ToByte(siolisten.SIO.isDSR());
										else extkey_dot = System.Convert.ToByte(siolisten.SIO.isCTS());
										break;
									case KeyerLine.DTR: // look at DSR since we are on the other side of the null modem cable
										keyerptt = siolisten.SIO.isDSR();
										//										extkey_dot = System.Convert.ToByte(sp2.CtsHolding);
										break;
									case KeyerLine.RTS: // look at CTS since we are on the other side of the null modem cable
										keyerptt = siolisten.SIO.isCTS();
										//										extkey_dash  = System.Convert.ToByte(sp2.DsrHolding);
										break;								
								}

								switch(secondary_key_line)
								{
									case KeyerLine.NONE:
										if (sp2dotkey) extkey_dash  = System.Convert.ToByte(siolisten.SIO.isDSR());
										else extkey_dot = System.Convert.ToByte(siolisten.SIO.isCTS());
										break;
									case KeyerLine.DTR: // look at DSR since we are on the other side of the null modem cable
										extkey_dot = System.Convert.ToByte(siolisten.SIO.isDSR());
										//										Debug.WriteLine("extkey_dot: "+extkey_dot);
										break;
									case KeyerLine.RTS: // look at CTS since we are on the other side of the null modem cable
										extkey_dash = System.Convert.ToByte(siolisten.SIO.isCTS());
										break;
								}
								
								if ((extkey_dash+extkey_dot) != 0)
									keyprog=1;
								else keyprog=0;
								//								Debug.WriteLine("keyprog: "+keyprog);
							} 
							else keyprog = 0;

							break;

						default: // comm port
							if ((extkey_dash==0) && (extkey_dot == 0)) // don't override primary
							{
								switch(secondary_ptt_line)
								{
									case KeyerLine.NONE:
										if (sp2dotkey) extkey_dash  = System.Convert.ToByte(sp2.DsrHolding);
										else extkey_dot = System.Convert.ToByte(sp2.CtsHolding);
										break;
									case KeyerLine.DTR: // look at DSR since we are on the other side of the null modem cable
										keyerptt = sp2.DsrHolding;
//										extkey_dot = System.Convert.ToByte(sp2.CtsHolding);
										break;
									case KeyerLine.RTS: // look at CTS since we are on the other side of the null modem cable
										keyerptt = sp2.CtsHolding;
//										extkey_dash  = System.Convert.ToByte(sp2.DsrHolding);
										break;								
								}

								switch(secondary_key_line)
								{
									case KeyerLine.NONE:
										if (sp2dotkey) extkey_dash  = System.Convert.ToByte(sp2.DsrHolding);
										else extkey_dot = System.Convert.ToByte(sp2.CtsHolding);
										break;
									case KeyerLine.DTR: // look at DSR since we are on the other side of the null modem cable
										extkey_dot = System.Convert.ToByte(sp2.DsrHolding);
//										Debug.WriteLine("extkey_dot: "+extkey_dot);
										break;
									case KeyerLine.RTS: // look at CTS since we are on the other side of the null modem cable
										extkey_dash = System.Convert.ToByte(sp2.CtsHolding);
										break;
								}
								
								if ((extkey_dash+extkey_dot) != 0)
									keyprog=1;
								else keyprog=0;
//								Debug.WriteLine("keyprog: "+keyprog);
							} else keyprog = 0;

							break;
					}
					timer.Stop();
					msdel = (float)timer.DurationMsec;
					DttSP.KeyValue(msdel, extkey_dash, extkey_dot, keyprog);
				}
			} while(true);
		}

		#endregion
	}
}
