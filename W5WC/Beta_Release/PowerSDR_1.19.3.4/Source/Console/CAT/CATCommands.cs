//=================================================================
// CATCommands.cs
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
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for CATCommands.
	/// </summary>
	public class CATCommands
	{
		#region Variable Definitions

		private Console console;
		private CATParser parser;
		private string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		private Band[] BandList;
		private int LastBandIndex;
		private ASCIIEncoding AE = new ASCIIEncoding();
		private string lastFR = "0";
		private string lastFT = "0";

		//		public static Mutex CATmut = new Mutex();

		#endregion Variable Definitions

		#region Constructors

		public CATCommands()
		{
		}

		public CATCommands(Console c,CATParser p)
		{
			console = c;
			parser = p;
			MakeBandList();
		}

		#endregion Constructors

		// Commands getting this far have been checked for a valid prefix, a correct suffix length,
		// and a terminator.  All we need to do in this class is to decide what kind of command
		// (read or set) and execute it.  Only read commands generate answers.

		#region Standard CAT Methods A-F

		// Sets or reads the Audio Gain control
		public string AG(string s)
		{
			if(s.Length == parser.nSet)	// if the length of the parameter legal for setting this prefix
			{
				int raw = Convert.ToInt32(s.Substring(1));
				int af = (int) Math.Round(raw/2.55,0);	// scale 255:100 (Kenwood vs SDR)
				console.AF = af;		// Set the console control
				return "";
			}
			else if(s.Length == parser.nGet)	// if this is a read command
			{
				int af = (int) Math.Round(console.AF/0.392,0);
//				return AddLeadingZeros(console.AF);		// Get the console setting
				return AddLeadingZeros(af);
			}
			else
			{
				return parser.Error1;	// return a ?
			}
		}

		public string AI(string s)
		{
			return ZZAI(s);
			//			if(console.SetupForm.AllowFreqBroadcast)
			//			{
			//				if(s.Length == parser.nSet)
			//				{
			//					if(s == "0")
			//						console.KWAutoInformation = false;
			//					else 
			//						console.KWAutoInformation = true;
			//					return "";
			//				}
			//				else if(s.Length == parser.nGet)
			//				{
			//					if(console.KWAutoInformation)
			//						return "1";
			//					else
			//						return "0";
			//				}
			//				else
			//					return parser.Error1;
			//			}
			//			else
			//				return parser.Error1;
		}

		// Moves one band down from the currently selected band
		// write only
		public string BD()
		{
			//			BandDown();
			//			return "";
			return ZZBD();
		}

		// Moves one band up from the currently selected band
		// write only
		public string BU()
		{
			//			BandUp();
			//			return "";
			return ZZBU();
		}

		//Moves the VFO A frequency by the step size set on the console
		public string DN()
		{
			//			int step = console.StepSize;
			//			double[] wheel_tune_list;
			//			wheel_tune_list = new double[13];		// initialize wheel tuning list array
			//			wheel_tune_list[0]  =  0.000001;
			//			wheel_tune_list[1]  =  0.000010;
			//			wheel_tune_list[2]  =  0.000050;
			//			wheel_tune_list[3]  =  0.000100;
			//			wheel_tune_list[4]  =  0.000250;
			//			wheel_tune_list[5]  =  0.000500;
			//			wheel_tune_list[6]  =  0.001000;
			//			wheel_tune_list[7]  =  0.005000;
			//			wheel_tune_list[8]  =  0.009000;
			//			wheel_tune_list[9]  =  0.010000;
			//			wheel_tune_list[10] =  0.100000;
			//			wheel_tune_list[11] =  1.000000;
			//			wheel_tune_list[12] = 10.000000;
			//
			//			console.VFOAFreq = console.VFOAFreq - wheel_tune_list[step];
			//			return "";

			return ZZSA();
		}

		// Sets or reads the frequency of VFO A
		public string FA(string s)
		{
			//			if(s.Length == parser.nSet)
			//			{
			//				s = s.Insert(5,separator);		//reinsert the global numeric separator
			//				console.VFOAFreq = double.Parse(s);
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//				return StrVFOFreq("A");
			//			else
			//				return parser.Error1;
			return ZZFA(s);
		}

		// Sets or reads the frequency of VFO B
		public string FB(string s)
		{
			//			if(s.Length == parser.nSet)
			//			{
			//				s = s.Insert(5,separator);
			//				console.VFOBFreq = double.Parse(s);
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//				return StrVFOFreq("B");
			//			else
			//				return parser.Error1;
			return ZZFB(s);
		}

		// Sets VFO A to control rx
		// this is a dummy command to keep other software happy
		// since the SDR-1000 always uses VFO A for rx
		public string FR(string s)
		{
			if(s.Length == parser.nSet)
			{
				return "";
			}
			else if(s.Length == parser.nGet)
				return "0";
			else
				return parser.Error1;
		}

		// Sets or reads VFO B to control tx
		// another "happiness" command
		public string FT(string s)
		{
			//			if(s.Length == parser.nSet)
			//			{
			//				if(s == "1")
			//				{
			//					console.VFOSplit = true;
			//				}
			//				else if(s == "0")
			//				{
			//					console.VFOSplit = false;
			//				}
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				return ZZSP(s);
			//			}
			//			else
			//				return parser.Error1;
			return ZZSP(s);
		}

		// Sets or reads the DSP filter width
		//OBSOLETE
		public string FW(string s)
		{
			if(s.Length == parser.nSet)
			{
				console.RX1Filter = String2Filter(s);
				return "";
			}
			else if(s.Length == parser.nGet)
				return Filter2String(console.RX1Filter);
			else
				return parser.Error1;
		}

		#endregion Standard CAT Methods A-F

		#region Standard CAT Methods G-M

		// Sets or reads the AGC constant
		// this is a wrapper that calls ZZGT
		public string GT(string s)
		{
			if(ZZGT(s).Length > 0)
				return ZZGT(s).PadLeft(3,'0');		//Added padleft fix 4/2/2007 BT
			else
				return "";
		}

		// Reads the transceiver ID number
		// this needs changing when 3rd party folks on line.
		public string ID()
		{
			string id;
			switch(console.CATRigType)
			{
				case 900:
					id = "900";		//SDR-1000
					break;
				case 13:
					id = "013";		//TS-50S
					break;
				case 19:
					id = "019";		//TS-2000
					break;
				case 20:
					id = "020";		//TS-480
					break;
				default:
					id = "019";
					break;
			}
			return(id);
		}

		// Reads the transceiver status
		// needs work in the split area
		public string IF()
		{
			string rtn = "";
			string rit = "0";
			string xit = "0";
			string incr;
			string tx = "0";
			string tempmode = "";
			int ITValue = 0;
			//string temp;

			// Get the rit/xit status
			if(console.RITOn)
				rit = "1";
			else if(console.XITOn)
				xit = "1";
			// Get the incremental tuning value for whichever control is selected
			if(rit == "1")
				ITValue = console.RITValue;
			else if(xit == "1")
				ITValue = console.XITValue;
			// Format the IT value
			if(ITValue < 0)
				incr = "-"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
			else
				incr = "+"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
			// Get the rx - tx status
			if(console.MOX)
				tx = "1";
			// Get the step size
			int step = console.StepSize;
			string stepsize =  Step2String(step);
			// Get the vfo split status
			string split = "0";
			bool retval = console.VFOSplit;
			if(retval)
				split = "1";
			//Get the mode
			//			temp = Mode2KString(console.RX1DSPMode);   //possible fix for SAM problem
			//			if(temp == parser.Error1)
			//				temp = " ";

			string f = ZZFA("");
			if(f.Length > 11)
			{
				f = f.Substring(f.Length-11,11);
			}
			rtn += f;
//			rtn += StrVFOFreq("A");						// VFO A frequency			11 bytes
			rtn += stepsize;							// Console step frequency	 4 bytes
			rtn += incr;								// incremental tuning value	 6 bytes
			rtn += rit;									// RIT status				 1 byte
			rtn += xit;									// XIT status				 1 byte
			rtn += "000";								// dummy for memory bank	 3 bytes
			rtn += tx;									// tx-rx status				 1 byte
			//			rtn += temp;
//			rtn += Mode2KString(console.RX1DSPMode);	// current mode			 1 bytes
			tempmode = Mode2KString(console.RX1DSPMode);
			if(tempmode == "?;")
				rtn += "2";
			else
				rtn += tempmode;
			rtn += "0";									// dummy for FR/FT			 1 byte
			rtn += "0";									// dummy for scan status	 1 byte
			rtn += split;								// VFO Split status			 1 byte
			rtn += "0000";								// dummy for the balance	 4 bytes
			return rtn;									// total bytes				35

			//			// Initalize the command word
			//			string cmd_string = "";
			//			string temp;
			//
			//			// Get VFOA's frequency (P1 - 11 bytes)
			//			if(console.VFOSplit)
			//				cmd_string += StrVFOFreq("B");
			//			else
			//				cmd_string += StrVFOFreq("A");
			//
			//			// Get the step size index (P2 - 4 bytes)
			//			cmd_string += ZZST();
			//
			//			// Determine which incremental tuning control is active
			//			// and get the value for the active control 
			//			string rit = "0";
			//			string xit = "0";
			//			int ITValue = 0;
			//
			//			if(console.RITOn)
			//				rit = "1";
			//			else if(console.XITOn)
			//				xit = "1";
			//
			//			if(rit == "1")
			//				ITValue = console.RITValue;
			//			else if(xit == "1")
			//				ITValue = console.XITValue;
			//
			//			// Add the ITValue to the command string (P3 - 6 bytes
			//			if(ITValue < 0)
			//				cmd_string += "-"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
			//			else
			//				cmd_string += "+"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
			//
			//			// Add the RIT/XIT status bits (P4 and P5, one byte each)
			//			cmd_string += rit+xit;
			//				
			//			// Skip the memory channel stuff, the SDR1K doesn't use banks and channels per se
			//			// (P6 - 1 byte, P7 - 2 bytes)
			//			cmd_string += "000";
			//
			//			// Set the current MOX state (P8 - 1 byte)(what the heck is this for?)
			//			if(console.MOX)
			//				cmd_string += "1";
			//			else
			//				cmd_string += "0";
			//
			//			// Get the SDR mode.  (P9 - 1 byte)
			//			temp = Mode2KString(console.RX1DSPMode);
			//			if(temp.Length == 1)	// if the answer is not an error message ?;
			//				cmd_string += temp;
			//			else
			//				cmd_string += " ";	// return a blank if it's an error
			//
			//			// Set the FR/FT commands which determines the transmit and receive
			//			// VFO's. VFO A = 0, VFO B = 1. (P10 - 1 byte)
			//			if(console.VFOSplit)
			//				cmd_string += "1";
			//			else
			//				cmd_string += "0";
			//
			//
			//			// Set the Scan code to 0 
			//			// The Scan code might be implemented but the frequency range would
			//			// have to be manually entered. (P11 - 1 byte)
			//			cmd_string += "0";
			//
			//			// Set the Split operation code (P12 - 1 byte)
			//			cmd_string += ZZSP("");
			//
			//			// Set the remaining CTCSS tone and shift bits to 0 (P13 - P15, 4 bytes)
			//			cmd_string += "0000";
			//
			//			return cmd_string;
		}

		//Sets or reads the CWX CW speed
		public string KS(string s)
		{
			//			int cws = 0;
			//			// Make sure we have an instance of the form
			//			if(console.CWXForm == null || console.CWXForm.IsDisposed)
			//			{
			//				try
			//				{
			//					console.CWXForm = new CWX(console);
			//				}
			//				catch
			//				{
			//					return parser.Error1;
			//				}
			//			}
			//			if(s.Length == parser.nSet)
			//			{
			//				cws = Convert.ToInt32(s);
			//				cws = Math.Max(1, cws);
			//				cws = Math.Min(99, cws);
			//				console.CWXForm.WPM = cws;
			//				return "";
			//
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				return AddLeadingZeros(console.CWXForm.WPM);
			//			}
			//			else
			//				return parser.Error1;
			return ZZKS(s);
		}

		//Sends text data to CWX for conversion to Morse
		public string KY(string s)
		{
			// Make sure we have an instance of the form
			if(console.CWXForm == null || console.CWXForm.IsDisposed)
			{
				try
				{
					console.CWXForm = new CWX(console);
				}
				catch
				{
					return parser.Error1;
				}
			}

			// Make sure we are in a cw mode.
			switch(console.RX1DSPMode)
			{
				case DSPMode.AM:
				case DSPMode.DRM:
				case DSPMode.DSB:
				case DSPMode.FMN:
				case DSPMode.SAM:
				case DSPMode.SPEC:
				case DSPMode.LSB:
				case DSPMode.USB:
					if(console.RX1Band >= Band.B160M && console.RX1Band <= Band.B40M)
						console.RX1DSPMode = DSPMode.CWL;
					else
						console.RX1DSPMode = DSPMode.CWU;
					break;
                case DSPMode.CWL:
                case DSPMode.CWU:
                    break;
				default:
					console.RX1DSPMode = DSPMode.CWU;
					break;
			}

			if(s.Length == parser.nSet)
			{

				string trms = "";
				byte[] msg;
                string x = s.Trim();

				if(x.Length == 0)
					trms = " ";
				else
					trms = s.TrimEnd();

				if(trms.Length > 1)
				{
					msg = AE.GetBytes(trms);
					return console.CWXForm.RemoteMessage(msg);
				}
				else
				{
					char ss = Convert.ToChar(trms);
					return console.CWXForm.RemoteMessage(ss);
				}
			}
			else if(s.Length == parser.nGet)
			{
				int ch = console.CWXForm.Characters2Send;

				if(ch < 72)
					return "0";
				else
					return "1";
			}
			else
				return parser.Error1;
		}


		// Sets or reads the transceiver mode
		public string MD(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(Convert.ToInt32(s) > 0 && Convert.ToInt32(s) <= 9)
				{
					KString2Mode(s);
					return "";
				}
				else
					return parser.Error1;
			}
			else if(s.Length == parser.nGet)
			{

				return Mode2KString(console.RX1DSPMode);

			}
			else
				return parser.Error1;
		}

		// Sets or reads the Mic Gain thumbwheel
		public string MG(string s)
		{
			int n;
			if(s.Length == parser.nSet)	
			{
				n = Convert.ToInt32(s);
				n = Math.Max(0, n);
				n = Math.Min(100, n);
				int mg = (int) Math.Round(n/1.43,0);	// scale 100:70 (Kenwood vs SDR)
				s = AddLeadingZeros(mg);
				return ZZMG(s);
			}
			else if(s.Length == parser.nGet)
			{
				s = ZZMG("");
				n = Convert.ToInt32(s);
				int mg = (int) Math.Round(n/.7,0);
				s = AddLeadingZeros(mg);
				return s;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the Monitor status
		public string MO(string s)
		{
			//			if(s.Length == parser.nSet)
			//			{
			//				if(s == "0")
			//					console.MON = false;
			//				else if(s == "1")
			//					console.MON = true;
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				bool retval = console.MON;
			//				if(retval)
			//					return "1";
			//				else
			//					return "0";
			//			}
			//			else
			//				return parser.Error1;
			return ZZMO(s);
		}

		#endregion Standard CAT Methods G-M

		#region Standard CAT Methods N-Q

		// Sets or reads the Noise Blanker 1 status
		public string NB(string s)
		{
			//			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			//			{
			//				console.CATNB1 = Convert.ToInt32(s);
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				return console.CATNB1.ToString();
			//			}
			//			else
			//			{
			//				return parser.Error1;
			//			}
			return ZZNA(s);
		}

		// Sets or reads the Automatic Notch Filter status
		public string NT(string s)
		{
			//			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			//			{
			//				console.CATANF = Convert.ToInt32(s);
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				return console.CATANF.ToString();
			//			}
			//			else
			//			{
			//				return parser.Error1;
			//			}
			return ZZNT(s);
		}

		// Sets or reads the PA output thumbwheel
		public string PC(string s)
		{
			//			int pwr = 0;
			//
			//			if(s.Length == parser.nSet)
			//			{
			//				pwr = Convert.ToInt32(s);
			//				console.PWR = pwr;
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				return AddLeadingZeros(console.PWR);
			//			}
			//			else
			//			{
			//				return parser.Error1;
			//			}
			return ZZPC(s);
		}

		// Sets or reads the Speech Compressor status
		/*public string PR(string s)
		{
//			if(s.Length == parser.nSet)
//			{
//				if(s == "0")
//					console.COMP = false;
//				else if(s == "1")
//					console.COMP = true;
//				return "";
//			}
//			else if(s.Length == parser.nGet)
//			{
//				bool comp = console.COMP;
//				if(comp)
//					return "1";
//				else
//					return "0";
//			}
//			else
//			{
//				return "";
//			}
			return ZZPK(s);
		}*/

		// Sets or reads the console power on/off status
		public string PS(string s)
		{
			//			if(s.Length == parser.nSet)
			//			{
			//				if(s == "0")
			//					console.PowerOn = false;
			//				else if(s == "1")
			//					console.PowerOn = true;
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				bool pwr = console.PowerOn;
			//				if(pwr)
			//					return "1";
			//				else
			//					return "0";
			//			}
			//			else
			//			{
			//				return parser.Error1;
			//			}
			return ZZPS(s);
		}

		// Sets the Quick Memory with the current contents of VFO A
		public string QI()
		{
//			console.CATMemoryQS();
//			return "";
			return ZZQS();
		}

		#endregion Standard CAT Methods N-Q

		#region Standard CAT Methods R-Z

		// Clears the RIT value
		// write only
		public string RC()
		{
//			console.RITValue = 0;
//			return "";
			return ZZRC();
		}

		//Decrements RIT
		public string RD(string s)
		{
			return ZZRD(s);
		}


		// Sets or reads the RIT status (on/off)
		public string RT(string s)
		{
			//			if(s.Length == parser.nSet)
			//			{
			//				if(s == "0")
			//					console.RITOn = false;
			//				else if(s == "1") 
			//					console.RITOn = true;
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				bool rit = console.RITOn;
			//				if(rit)
			//					return "1";
			//				else
			//					return "0";
			//			}
			//			else
			//			{
			//				return parser.Error1;
			//			}
			return ZZRT(s);
		}

		//Increments RIT
		public string RU(string s)
		{
			return ZZRU(s);
		}


		// Sets or reads the transceiver receive mode status
		// write only but spec shows an answer parameter for a read???
		public string RX(string s)
		{
			console.CATPTT = false;
			return "";
			//return ZZTX("0");
		}

		// Sets or reads the variable DSP filter high side
		public string SH(string s)
		{
			if(s.Length == parser.nSet)
			{
				SetFilter(s, "SH");
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				switch(console.RX1DSPMode)
				{
					case DSPMode.AM:
					case DSPMode.CWU:
					case DSPMode.DRM:
					case DSPMode.DSB:
					case DSPMode.FMN:
					case DSPMode.SAM:
					case DSPMode.USB:
						return Frequency2Code(console.RX1FilterHigh,"SH");
					case DSPMode.CWL:
					case DSPMode.LSB:
						return Frequency2Code(console.RX1FilterLow,"SH");
					default:
						return Frequency2Code(console.RX1FilterHigh,"SH");
				}
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the variable DSP filter low side
		public string SL(string s)
		{
			if(s.Length == parser.nSet)
			{
				SetFilter(s, "SL");
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				switch(console.RX1DSPMode)
				{
					case DSPMode.AM:
					case DSPMode.CWU:
					case DSPMode.DRM:
					case DSPMode.DSB:
					case DSPMode.FMN:
					case DSPMode.SAM:
					case DSPMode.USB:
						return Frequency2Code(console.RX1FilterLow,"SL");
					case DSPMode.CWL:
					case DSPMode.LSB:
						return Frequency2Code(console.RX1FilterHigh,"SL");
					default:
						return Frequency2Code(console.RX1FilterLow,"SL");
				}
			}
			else
			{
				return parser.Error1;
			}
		}

		// Reads the S Meter value
		public string SM(string s)
		{
			int sm = 0;
			double sx = 0.0;

			if(s == "0" || s == "2")	// read the main transceiver s meter
			{
				float num = 0f;
				if(console.PowerOn)
					num = DttSP.CalculateRXMeter(0, 0,DttSP.MeterType.SIGNAL_STRENGTH);
				num = num+console.MultiMeterCalOffset+console.PreampOffset;

				num = Math.Max(-140, num);
				num = Math.Min(-10, num);

				sx = (num+127)/6;
				if(sx < 0) sx = 0;

				if(sx <= 9.0F)
				{
					sm = Math.Abs((int)(sx * 1.6667));
				}
				else
				{
					double over_s9 = num + 73;
					sm = 15 + (int) over_s9;
				}
				if(sm < 0) sm = 0;
				if(sm > 30) sm = 30;

				return sm.ToString().PadLeft(5,'0');
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the Squelch value
		public string SQ(string s)
		{
			string rx = s.Substring(0,1);
			double level = 0.0;

			//Will need code to select receiver when n Receivers enabled.
			//for now, ignore rx number.
			
			if(s.Length == parser.nSet)
				//convert to a double and add the scale factor (160 = 255)
			{
				level = Convert.ToDouble(s.Substring(1));
				level = Math.Max(0, level);			// lower bound
				level = Math.Min(255, level);		// upper bound
				level = level*0.62745;				// scale factor
				console.Squelch = Convert.ToInt32(Math.Round(level,0));
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return rx+AddLeadingZeros(console.Squelch).Substring(1);
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets the transmitter on, write only
		// will eventually need eiter Commander change or ZZ code
		// since it is not CAT compliant as it is
		public string TX(string s)
		{
			console.CATPTT = true;
			return "";
			//return ZZTX("1");
		}

		//Moves the VFO A frequency up by the step size set on the console
		public string UP()
		{
			//			int step = console.StepSize;
			//			double[] wheel_tune_list;
			//			wheel_tune_list = new double[13];		// initialize wheel tuning list array
			//			wheel_tune_list[0]  =  0.000001;
			//			wheel_tune_list[1]  =  0.000010;
			//			wheel_tune_list[2]  =  0.000050;
			//			wheel_tune_list[3]  =  0.000100;
			//			wheel_tune_list[4]  =  0.000250;
			//			wheel_tune_list[5]  =  0.000500;
			//			wheel_tune_list[6]  =  0.001000;
			//			wheel_tune_list[7]  =  0.005000;
			//			wheel_tune_list[8]  =  0.009000;
			//			wheel_tune_list[9]  =  0.010000;
			//			wheel_tune_list[10] =  0.100000;
			//			wheel_tune_list[11] =  1.000000;
			//			wheel_tune_list[12] = 10.000000;
			//
			//			console.VFOAFreq = console.VFOAFreq + wheel_tune_list[step];
			//			return "";

			return ZZSB();
		}


		// Sets or reads the transceiver XIT status (on/off)
		public string XT(string s)
		{
			//			if(s.Length == parser.nSet)
			//			{
			//				if(s == "0")
			//					console.XITOn = false;
			//				else
			//					if(s == "1") 
			//					console.XITOn = true;
			//				return "";
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				bool xit = console.XITOn;
			//				if(xit)
			//					return "1";
			//				else
			//					return "0";
			//			}
			//			else
			//			{
			//				return parser.Error1;
			//			}
			return ZZXS(s);

		}

		#endregion Standard CAT Methods R-Z

		#region Extended CAT Methods ZZA-ZZF

		// Sets or reads the SDR-1000 Audio Gain control
		public string ZZAG(string s)
		{
			int af = 0;

			if(s.Length == parser.nSet)	// if the length of the parameter legal for setting this prefix
			{
				af = Convert.ToInt32(s);
				af = Math.Max(0, af);
				af = Math.Min(100, af);
				console.AF = af;		// Set the console control
				return "";
			}
			else if(s.Length == parser.nGet)	// if this is a read command
			{
				return AddLeadingZeros(console.AF);		// Get the console setting
			}
			else
			{
				return parser.Error1;	// return a ?
			}

		}

		public string ZZAI(string s)
		{
			if(console.SetupForm.AllowFreqBroadcast)
			{
				if(s.Length == parser.nSet)
				{
					if(s == "0")
						console.KWAutoInformation = false;
					else 
						console.KWAutoInformation = true;
					return "";
				}
				else if(s.Length == parser.nGet)
				{
					if(console.KWAutoInformation)
						return "1";
					else
						return "0";
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the AGC RF gain
		public string ZZAR(string s)
		{
			int n = 0;
			int x = 0;
			string sign;

			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Max(-20, n);
				n = Math.Min(120, n);
			}

			if(s.Length == parser.nSet)
			{
				console.RF = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				x = console.RF;
				if(x >= 0)
					sign = "+";
				else
					sign = "-";
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}

		}

		//Moves the bandswitch down one band
		public string ZZBD()
		{
			BandDown();
			return "";
		}

		// Sets the Band Group (HF/VHF)
		public string ZZBG(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.CATBandGroup = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATBandGroup.ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the BIN button status
		public string ZZBI(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.CATBIN = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATBIN.ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the BCI Rejection button status
		public string ZZBR(string s)
		{
			if(console.CurrentModel == Model.SDR1000)
			{
				int sx = 0;

				if(s != "")
					sx = Convert.ToInt32(s);

				if(s.Length == parser.nSet && (s == "0" || s == "1"))
				{
					console.CATBCIReject = sx;
					return "";
				}
				else if(s.Length == parser.nGet)
				{
					return console.CATBCIReject.ToString();
				}
				else
				{
					return parser.Error1;
				}
			}
			else
				return parser.Error1;

		}


		//Sets or reads the current band setting
		public string ZZBS(string s)
		{
			return GetBand(s);
		}

		//Moves the bandswitch up one band
		public string ZZBU()
		{
			BandUp();
			return "";
		}

		// Sets or reads the CW Break In Enabled checkbox
		public string ZZCB(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "1")
					console.BreakInEnabled = true;
				else
					console.BreakInEnabled = false;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.BreakInEnabled)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}

		}


		// Sets or reads the CW Break In Delay
		public string ZZCD(string s)
		{
			int n = 0;

			if(s != null && s != "")
				n = Convert.ToInt32(s);
			n = Math.Max(150, n);
			n = Math.Min(5000, n);

			if(s.Length == parser.nSet)
			{
				console.SetupForm.BreakInDelay = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros((int) console.SetupForm.BreakInDelay);
			}
			else
			{
				return parser.Error1;
			}

		}

		// Sets or reads the Show CW Frequency checkbox
		public string ZZCF(string s)
		{
			switch(console.RX1DSPMode)
			{
				case DSPMode.CWL:
				case DSPMode.CWU:
					if(s.Length == parser.nSet && (s == "0" || s == "1"))
					{
						if(s == "1")
							console.ShowCWTXFreq = true;
						else
							console.ShowCWTXFreq = false;
						return "";
					}
					else if(s.Length == parser.nGet)
					{
						if(console.ShowCWTXFreq)
							return "1";
						else
							return "0";
					}
					else
					{
						return parser.Error1;
					}
				default:
					return parser.Error1;
			}
		}

		// Sets or reads the CW Iambic checkbox
		public string ZZCI(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "1")
					console.CWIambic = true;
				else
					console.CWIambic = false;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.CWIambic)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}

		}

		// Sets or reads the CW Pitch thumbwheel
		public string ZZCL(string s)
		{
			int n = 0;
			if(s != "")
				n = Convert.ToInt32(s);

			if(s.Length == parser.nSet)
			{
				console.SetupForm.CATCWPitch = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.SetupForm.CATCWPitch);
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the CW Monitor Disable button status
		public string ZZCM(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "1")
					console.CWDisableMonitor = true;
				else
					console.CWDisableMonitor = false;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.CWDisableMonitor)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}

		}

		// Sets or reads the compander button status
		public string ZZCP(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.CATCmpd = 0;
				else if(s == "1")
					console.CATCmpd = 1;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATCmpd.ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the CW Speed thumbwheel
		public string ZZCS(string s)
		{
			int n = 1;

			if(s != "")
				n = Convert.ToInt32(s);

			if(s.Length == parser.nSet)
			{
				console.CATCWSpeed = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.CATCWSpeed);
			}
			else
			{
				return parser.Error1;
			}
		}

		//Reads or sets the compander threshold
		public string ZZCT(string s)
		{
			int n = 0;

			if(s != null && s != "")
				n = Convert.ToInt32(s);
			n = Math.Max(0, n);
			n = Math.Min(10, n);

			if(s.Length == parser.nSet)
			{
				console.CPDRVal = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros((int) console.CPDRVal);
			}
			else
			{
				return parser.Error1;
			}

		}

		// Reads the CPU Usage
		public string ZZCU()
		{
			return console.CpuUsage.ToString("f").PadLeft(6,'0');
		}

		// Sets or reads the Display Average status
		public string ZZDA(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.CATDisplayAvg = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATDisplayAvg.ToString();
			}
			else
			{
				return parser.Error1;
			}

		}

		// Sets or reads the current display mode
		public string ZZDM(string s)
		{
			int n = -1;

			if(s.Length == parser.nSet)
			{
				n = Convert.ToInt32(s);
				switch(n)
				{
					case 0:
						console.DisplayModeText = "Spectrum";
						break;
					case 1:
						console.DisplayModeText = "Panadapter";
						break;
					case 2:
						console.DisplayModeText = "Scope";
						break;
					case 3:
						console.DisplayModeText = "Phase";
						break;
					case 4:
						console.DisplayModeText = "Phase2";
						break;
					case 5:
						console.DisplayModeText = "Waterfall";
						break;
					case 6:
						console.DisplayModeText = "Histogram";
						break;
					case 7:
						console.DisplayModeText = "Off";
						break;

				}

//				if(n > (int) DisplayMode.FIRST && n < (int) DisplayMode.LAST)
//					Display.CurrentDisplayMode = (DisplayMode) n;
//				else
//					return parser.Error1;
//
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return ((int) Display.CurrentDisplayMode).ToString();
			}
			else
			{
				return parser.Error1;
			}

		}

		/// <summary>
		/// Sets or reads the DX button status
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string ZZDX(string s)
		{
			if(s.Length == parser.nSet)
			{
				console.CATPhoneDX = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATPhoneDX;
			}
			else
				return parser.Error1;
		}

		/// <summary>
		/// Reads or sets the RX equalizer.
		/// The CAT suffix string is 36 characters constant.
		/// Each value in the string occupies exactly three characters
		/// starting with the number of bands (003 or 010) followed by
		/// the preamp setting (-12 to 015) followed by 3 or 10 three digit
		/// EQ thumbwheel positions.  If the number of bands is 3, the
		/// last seven positions (21 characters) are all set to zero.
		/// Example:  10 band ZZEA010-09009005000-04-07-09-05000005009;
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string ZZEA(string s)
		{
			if(s.Length == parser.nSet)
			{
				int nb = Int32.Parse(s.Substring(0,3));			//Get the number of bands
				int[] ans = new	int[nb+1];						//Create the integer array
				s = s.Remove(0,3);								//Get rid of the band count

				for(int x = 0; x <= nb;x++)						//Parse the string into the array
				{
					ans[x] = Int32.Parse(s.Substring(0,3));
					s = s.Remove(0,3);							//Remove the last three used
				}
				console.EQForm.RXEQ = ans;						//Send the array to the eq form
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				int[] eqarray = console.EQForm.RXEQ;			//Get the equalizer array
				int nb = console.EQForm.NumBands;				//Get the number of bands in the array
				int val;										//Holds a temporary value
				string ans = nb.ToString().PadLeft(3,'0');		//The return string with the number of bands added

				for (int x = 0; x <= nb; x++)					//Loop thru the array
				{
					if(eqarray[x] < 0)	
					{
						val = Math.Abs(eqarray[x]);					//If the value is negative, format the answer
						ans += "-"+val.ToString().PadLeft(2,'0');
					}
					else
						ans += eqarray[x].ToString().PadLeft(3,'0');
				}
				ans = ans.PadRight(36,'0');							//Add the padding if it's a 3 band eq
				return ans;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the TX EQ settings
		public string ZZEB(string s)
		{
			if(s.Length == parser.nSet)
			{
				int nb = Int32.Parse(s.Substring(0,3));			//Get the number of bands
				int[] ans = new	int[nb+1];						//Create the integer array
				s = s.Remove(0,3);								//Get rid of the band count

				for(int x = 0; x <= nb;x++)						//Parse the string into the array
				{
					ans[x] = Int32.Parse(s.Substring(0,3));
					s = s.Remove(0,3);							//Remove the last three used
				}
				console.EQForm.TXEQ = ans;						//Send the array to the eq form
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				int[] eqarray = console.EQForm.TXEQ;			//Get the equalizer array
				int nb = console.EQForm.NumBands;				//Get the number of bands in the array
				int val;										//Holds a temporary value
				string ans = nb.ToString().PadLeft(3,'0');		//The return string with the number of bands added

				for (int x = 0; x <= nb; x++)					//Loop thru the array
				{
					if(eqarray[x] < 0)	
					{
						val = Math.Abs(eqarray[x]);					//If the value is negative, format the answer
						ans += "-"+val.ToString().PadLeft(2,'0');
					}
					else
						ans += eqarray[x].ToString().PadLeft(3,'0');
				}
				ans = ans.PadRight(36,'0');							//Add the padding if it's a 3 band eq
				return ans;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the RXEQ button statusl
		public string ZZER(string s)
		{
			if(s.Length == parser.nSet  && (s == "1" || s == "0"))
			{
				if(s == "1")
					console.CATRXEQ = "1";
				else if(s == "0")
					console.CATRXEQ = "0";
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATRXEQ;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the TXEQ button status
		public string ZZET(string s)
		{
			if(s.Length == parser.nSet  && (s == "1" || s == "0"))
			{
				if(s == "1")
					console.CATTXEQ = "1";
				else if(s == "0")
					console.CATTXEQ = "0";
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATTXEQ;
			}
			else
				return parser.Error1;
		}


		//Sets or reads VFO A frequency
		public string ZZFA(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(console.SetupForm.RttyOffsetEnabledA && 
					(console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
				{
					int f = int.Parse(s);
					if(console.RX1DSPMode == DSPMode.DIGU)
						f = f - Convert.ToInt32(console.SetupForm.RttyOffsetHigh);
					else if(console.RX1DSPMode == DSPMode.DIGL)
						f = f + Convert.ToInt32(console.SetupForm.RttyOffsetLow);
					s = AddLeadingZeros(f);
					s = s.Insert(5, separator);
				}
				else
					s = s.Insert(5, separator);

				console.VFOAFreq = double.Parse(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.SetupForm.RttyOffsetEnabledA &&
					(console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
				{
                    int f = Convert.ToInt32(Math.Round(console.CATVFOA, 6) * 1e6);
					if(console.RX1DSPMode == DSPMode.DIGU)
						f = f + Convert.ToInt32(console.SetupForm.RttyOffsetHigh);
					else if(console.RX1DSPMode == DSPMode.DIGL)
						f = f - Convert.ToInt32(console.SetupForm.RttyOffsetLow);
					return AddLeadingZeros(f);
				}
				else
					return StrVFOFreq("A");
			}
			else
				return parser.Error1;

		}

		//Sets or reads VFO B frequency
		public string ZZFB(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(console.SetupForm.RttyOffsetEnabledB  && 
					(console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
				{
					int f = int.Parse(s);
					if(console.RX1DSPMode == DSPMode.DIGU)
						f = f - Convert.ToInt32(console.SetupForm.RttyOffsetHigh);
					else if(console.RX1DSPMode == DSPMode.DIGL)
						f = f + Convert.ToInt32(console.SetupForm.RttyOffsetLow);
					s = AddLeadingZeros(f);
					s = s.Insert(5, separator);
				}
				else
					s = s.Insert(5, separator);

				console.VFOBFreq = double.Parse(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.SetupForm.RttyOffsetEnabledB &&
					(console.RX1DSPMode == DSPMode.DIGU || console.RX1DSPMode == DSPMode.DIGL))
				{
                    int f = Convert.ToInt32(Math.Round(console.CATVFOB, 6) * 1e6);
					if(console.RX1DSPMode == DSPMode.DIGU)
						f = f + Convert.ToInt32(console.SetupForm.RttyOffsetHigh);
					else if(console.RX1DSPMode == DSPMode.DIGL)
						f = f - Convert.ToInt32(console.SetupForm.RttyOffsetLow);
					return AddLeadingZeros(f);
				}
				else
					return StrVFOFreq("B");
			}
			else
				return parser.Error1;		
		}


		//Sets or reads the current filter index number
		public string ZZFI(string s)
		{
			int n = 0;

			if(s != "")
				n = Convert.ToInt32(s);

			if(s.Length == parser.nSet)
			{
				if(n < (int) Filter.LAST)
					console.RX1Filter = (Filter) n;
				else
					return parser.Error1;

				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros((int) console.RX1Filter);
			}
			else
			{
				return parser.Error1;
			}
		}


		/// <summary>
		/// Reads or sets the DSP Filter Low value
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string ZZFL(string s)
		{
			string sign;
			int n;

			if(s.Length == parser.nSet)
			{
				n = Convert.ToInt32(s);
				n = Math.Min(9999, n);
				n = Math.Max(-9999, n);
				console.RX1FilterLow = n;
				console.UpdateRX1Filters(n, console.RX1FilterHigh);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				n = console.RX1FilterLow;
				if(n < 0)
					sign = "-";
				else
					sign = "+";

				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(n)).Substring(1);
//				return AddLeadingZeros((int) console.RX1FilterLow);
			}
			else
				return parser.Error1;
		}

		/// <summary>
		/// Reads or sets the DSP Filter High value
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string ZZFH(string s)
		{
			string sign;
			int n;

			if(s.Length == parser.nSet)
				{
					n = Convert.ToInt32(s);
					n = Math.Min(9999, n);
					n = Math.Max(-9999, n);
					console.RX1FilterHigh = n;
					console.UpdateRX1Filters(console.RX1FilterLow, n);
					return "";
				}
				else if(s.Length == parser.nGet)
				{
					n = console.RX1FilterHigh;
					if(n < 0)
						sign = "-";
					else
						sign = "+";

					// we have to remove the leading zero and replace it with the sign.
					return sign+AddLeadingZeros(Math.Abs(n)).Substring(1);
				}
				else
					return parser.Error1;
		}

		public string ZZFM()
		{
			string radio = console.CurrentModel.ToString();
			if(radio == "SDR1000")
				return "0";
			else if (radio == "FLEX5000")
				return "1";
			else
				return parser.Error1;

		}
		//Reads FlexWire single byte value commands
		public string ZZFV(string s)
		{
			if(s.Length == parser.nGet)
			{
				String pattern = "^[a-fA-F0-9][a-fA-F0-9]$";
				Regex sfxpattern = new Regex(pattern);
				if(!sfxpattern.IsMatch(s))
					return parser.Error1;

				byte addr = byte.Parse(s, NumberStyles.HexNumber);
				uint val;
				FWC.FlexWire_ReadValue(addr, out val);
				string ans = String.Format("{0:X2}", (byte)val);
				return ans;
			}
			else
				return parser.Error1;
		}

		//Reds FlexWire double byte value commands
		public string ZZFW(String s)
		{
			if(s.Length == parser.nGet)
			{
				String pattern = "^[a-fA-F0-9][a-fA-F0-9]$";
				Regex sfxpattern = new Regex(pattern);
				if(!sfxpattern.IsMatch(s))
					return parser.Error1;

				byte addr = byte.Parse(s, NumberStyles.HexNumber);
				uint val;
				FWC.FlexWire_Read2Value(addr, out val);
				string ans1 = String.Format("{0:X2}", val>>8);
				string ans2 = String.Format("{0:X2}", (byte) val);
				string ans = String.Concat(ans1,ans2);
				return ans;
			
			}
			else
				return parser.Error1;
		}

		//Sends FlexWire single byte value commands
		public string ZZFX(string s)
		{
			if(s.Length == parser.nSet)
			{
				String pattern = "^[a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9]$";
				Regex sfxpattern = new Regex(pattern);
				if(!sfxpattern.IsMatch(s))
					return parser.Error1;

				byte addr = byte.Parse(s.Substring(0,2),NumberStyles.HexNumber);
				byte val = byte.Parse(s.Substring(2,2),NumberStyles.HexNumber);
				FWC.FlexWire_WriteValue(addr, val);
				return "";
			}
			else
				return parser.Error1;
		}

		//Sends FlexWire double byte value commands
		public string ZZFY(String s)
		{
			if(s.Length == parser.nSet)
			{
				String pattern = "^[a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9]$";
				Regex sfxpattern = new Regex(pattern);
				if(!sfxpattern.IsMatch(s))
					return parser.Error1;

				byte addr = byte.Parse(s.Substring(0,2), NumberStyles.HexNumber);
				byte val1 = byte.Parse(s.Substring(2,2), NumberStyles.HexNumber);
				byte val2 = byte.Parse(s.Substring(4,2), NumberStyles.HexNumber);

				FWC.FlexWire_Write2Value(addr, val1, val2);
				return "";
			
			}
			else
				return parser.Error1;
		}

		#endregion Extended CAT Methods ZZA-ZZF

		#region Extended CAT Methods ZZG-ZZM


		// Sets or reads the noise gate enable button status
		public string ZZGE(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "1")
					console.NoiseGateEnabled = true;
				else
					console.NoiseGateEnabled = false;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.NoiseGateEnabled)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}

		}

		//Sets or reads the noise gate level control
		public string ZZGL(string s)
		{
			int n = 0;
			int x = 0;
			string sign;

			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Max(-160, n);
				n = Math.Min(0, n);
			}

			if(s.Length == parser.nSet)
			{
				console.NoiseGate = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				x = console.NoiseGate;
				if(x >= 0)
					sign = "+";
				else
					sign = "-";
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the AGC constant
		public string ZZGT(string s)
		{
			if(s.Length == parser.nSet)
			{
				if((Convert.ToInt32(s) > (int) AGCMode.FIRST && Convert.ToInt32(s) < (int) AGCMode.LAST))
					console.RX1AGCMode = (AGCMode) Convert.ToInt32(s);
				else
					return parser.Error1;

				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return ((int) console.RX1AGCMode).ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the Audio Buffer Size
		public string ZZHA(string s)
		{
		
			if(s.Length == parser.nSet)
			{
				console.SetupForm.AudioBufferSize = Index2Width(s);
				return "";
			}
			else if (s.Length == parser.nGet)
			{
				return Width2Index(console.SetupForm.AudioBufferSize);
			}
			else
				return parser.Error1;
		}

		//Sets or reads the DSP Phone RX Buffer Size
		public string ZZHR(string s)
		{
			if(s.Length == parser.nSet)
			{
				int width = Index2Width(s);
				console.DSPBufPhoneRX = width;
				console.SetupForm.DSPPhoneRXBuffer = width;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return Width2Index(console.DSPBufPhoneRX);
			}
			else
				return parser.Error1;
		}

		//Sets or reads the DSP Phone TX Buffer Size
		public string ZZHT(string s)
		{
			if(s.Length == parser.nSet)
			{
				int width = Index2Width(s);
				console.DSPBufPhoneTX = width;
				console.SetupForm.DSPPhoneTXBuffer = width;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return Width2Index(console.DSPBufPhoneTX);
			}
			else
				return parser.Error1;
		}

		//Sets or reads the DSP CW RX Buffer Size
		public string ZZHU(string s)
		{
			if(s.Length == parser.nSet)
			{
				int width = Index2Width(s);
				console.DSPBufCWRX = width;
				console.SetupForm.DSPCWRXBuffer = width;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return Width2Index(console.DSPBufCWRX);
			}
			else
				return parser.Error1;
		}

		//Sets or reads the DSP CW TX Buffer Size
		public string ZZHV(string s)
		{
			if(s.Length == parser.nSet)
			{
				int width = Index2Width(s);
				console.DSPBufCWTX = width;
				console.SetupForm.DSPCWTXBuffer = width;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return Width2Index(console.DSPBufCWTX);
			}
			else
				return parser.Error1;
		}

		//Sets or reads the DSP Digital RX Buffer Size
		public string ZZHW(string s)
		{
			if(s.Length == parser.nSet)
			{
				int width = Index2Width(s);
				console.DSPBufDigRX = width;
				console.SetupForm.DSPDigRXBuffer = width;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return Width2Index(console.DSPBufDigRX);
			}
			else
				return parser.Error1;
		}

		//Sets or reads the DSP Digital TX Buffer Size
		public string ZZHX(string s)
		{
			if(s.Length == parser.nSet)
			{
				int width = Index2Width(s);
				console.DSPBufDigTX = width;
				console.SetupForm.DSPDigTXBuffer = width;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return Width2Index(console.DSPBufDigTX);
			}
			else
				return parser.Error1;
		}

		// Sets the CAT Rig Type to SDR-1000
		//Modified 10/12/08 BT changed "SDR-1000" to "PowerSDR"
		public string ZZID()
		{
			//			if(s.Length == parser.nSet)
			//			{
			//				return CAT2RigType(s);
			//			}
			//			else if(s.Length == parser.nGet)
			//			{
			//				return RigType2CAT();
			//			}
			//			else
			//				return parser.Error1;
			console.SetupForm.CATSetRig("PowerSDR");
			return "";
		}

		// Reads the SDR-1000 transceiver status
		public string ZZIF(string s)
		{
			string rtn = "";
			string rit = "0";
			string xit = "0";
			string incr;
			string tx = "0";
			int ITValue = 0;

			// Get the rit/xit status
			if(console.RITOn)
				rit = "1";
			else if(console.XITOn)
				xit = "1";
			// Get the incremental tuning value for whichever control is selected
			if(rit == "1")
				ITValue = console.RITValue;
			else if(xit == "1")
				ITValue = console.XITValue;
			// Format the IT value
			if(ITValue < 0)
				incr = "-"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
			else
				incr = "+"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
			// Get the rx - tx status
			if(console.MOX)
				tx = "1";
			// Get the step size
			int step = console.StepSize;
			string stepsize =  Step2String(step);
			// Get the vfo split status
			string split = "0";
			bool retval = console.VFOSplit;
			if(retval)
				split = "1";

			string f = ZZFA("");
			if(f.Length > 11)
			{
				f = f.Substring(f.Length-11,11);
			}
			rtn += f;
//			rtn += StrVFOFreq("A");						// VFO A frequency			11 bytes
			rtn += stepsize;							// Console step frequency	 4 bytes
			rtn += incr;								// incremental tuning value	 6 bytes
			rtn += rit;									// RIT status				 1 byte
			rtn += xit;									// XIT status				 1 byte
			rtn += "000";								// dummy for memory bank	 3 bytes
			rtn += tx;									// tx-rx status				 1 byte
			rtn += Mode2String(console.RX1DSPMode);	// current mode				 2 bytes
			rtn += "0";									// dummy for FR/FT			 1 byte
			rtn += "0";									// dummy for scan status	 1 byte
			rtn += split;								// VFO Split status			 1 byte
			rtn += "0000";								// dummy for the balance	 4 bytes
			return rtn;
		}

		// Sets or reads the IF width
		public string ZZIS(string s)
		{
			int n = 0;

			if(s != "")
				n = Convert.ToInt32(s);

			if(s.Length == parser.nSet)
			{
				console.CATFilterWidth = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.CATFilterWidth);
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the IF Shift
		public string ZZIT(string s)
		{
			int n = 0;
			string sign = "-";

			if(s != "")
				n = Convert.ToInt32(s);

			if(s.Length == parser.nSet)
			{
				console.CATFilterShift = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				n = console.CATFilterShift;
				if(n >= 0)
				{
					sign = "+";
				}
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(n)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}
		}

		// Resets the Filter Shift to zero.  Write only
		public string ZZIU()
		{
			console.CATFilterShiftReset = 1;	//Fixed XML entry 4/2/2007 to prevent return value.  BT
			return "";
		}

		//Sets or reads the CWX CW speed
		public string ZZKS(string s)
		{
			int cws = 0;
			// Make sure we have an instance of the form
			if(console.CWXForm == null || console.CWXForm.IsDisposed)
			{
				try
				{
					console.CWXForm = new CWX(console);
				}
				catch
				{
					return parser.Error1;
				}
			}

			if(s.Length == parser.nSet)
			{
				cws = Convert.ToInt32(s);
				cws = Math.Max(1, cws);
				cws = Math.Min(99, cws);
				console.CWXForm.WPM = cws;
				return "";

			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.CWXForm.WPM);
			}
			else
				return parser.Error1;
		}

		//Sends text to CWX for conversion to Morse
		public string ZZKY(string s)
		{
			// Make sure we have an instance of the form
			if(console.CWXForm == null || console.CWXForm.IsDisposed)
			{
				try
				{
					console.CWXForm = new CWX(console);
				}
				catch
				{
					return parser.Error1;
				}
			}

			// Make sure we are in a cw mode.
			switch(console.RX1DSPMode)
			{
				case DSPMode.AM:
				case DSPMode.DRM:
				case DSPMode.DSB:
				case DSPMode.FMN:
				case DSPMode.SAM:
				case DSPMode.SPEC:
				case DSPMode.LSB:
				case DSPMode.USB:
					if(console.RX1Band >= Band.B160M && console.RX1Band <= Band.B40M)
						console.RX1DSPMode = DSPMode.CWL;
					else
						console.RX1DSPMode = DSPMode.CWU;
					break;
                case DSPMode.CWL:
                case DSPMode.CWU:
                    break;
				default:
					console.RX1DSPMode = DSPMode.CWU;
					break;
			}

			if(s.Length == parser.nSet)
			{

				string trms = "";
				byte[] msg;
                string x = s.Trim();

				if(x.Length == 0)
					trms = " ";
				else
					trms = s.TrimEnd();

				if(trms.Length > 1)
				{
					msg = AE.GetBytes(trms);
					return console.CWXForm.RemoteMessage(msg);
				}
				else
				{
					char ss = Convert.ToChar(trms);
					return console.CWXForm.RemoteMessage(ss);
				}
			}
			else if(s.Length == parser.nGet)
			{
				int ch = console.CWXForm.Characters2Send;
				if(ch > 0 && ch < 72)
					return "0";
				else if(ch >= 72)
					return "1";
				else if(ch == 0)
					return "2";
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the MUT button on/off status
		public string ZZMA(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "0")
					console.MUT = false;
				else if(s == "1")
					console.MUT = true;

				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool retval = console.MUT;
				if(retval)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}

		}



		// Sets or reads the SDR-1000 DSP mode
		public string ZZMD(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(Convert.ToInt32(s) >= 0 && Convert.ToInt32(s) <= 11)
				{
					String2Mode(s);
					return "";
				}
				else
					return parser.Error1;
			}
			else if(s.Length == parser.nGet)
			{
				return Mode2String(console.RX1DSPMode);
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the Mic gain control
		public string ZZMG(string s)
		{
			int n;
			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Min(70,n);
				n = Math.Max(0,n);
				s = AddLeadingZeros(n);
			}

			if(s.Length == parser.nSet)
			{
				console.CATMIC = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.CATMIC);
			}
			else
				return parser.Error1;
		}

		//Reads the DSP filter presets for filter index (s)
		//Returns 180 character length word for 12 filters x 15 characters each.
		//Format is name high low: ZZMN 5.0k 5150 -160...
		public string ZZMN(string s)
		{
			if(s.Length == parser.nGet)
			{
				return console.GetFilterPresets(Int32.Parse(s));
			}
			else
				return parser.Error1;
		}

		//Sets or reads the Monitor (MON) button status
		public string ZZMO(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.MON = false;
				else if(s == "1")
					console.MON = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool retval = console.MON;
				if(retval)
					return "1";
				else
					return "0";
			}
			else
				return parser.Error1;
		}

		// Sets or reads the RX meter mode
		public string ZZMR(string s)
		{
			int m = -1;
			if(s != "")
				m = Convert.ToInt32(s);

			if(s.Length == parser.nSet && 
				(m > (int) MeterRXMode.FIRST && m < (int) MeterRXMode.LAST))
			{
				String2RXMeter(m);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return RXMeter2String();
			}
			else
			{
				return parser.Error1;
			}

		}	
		
		//Sets or reads the MultiRX Swap checkbox
		public string ZZMS(string s)
		{
			if(s.Length == parser.nSet  && (s == "1" || s == "0"))
			{
				if(s == "1")
					console.CATPanSwap = "1";
				else if(s == "0")
					console.CATPanSwap= "0";
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATPanSwap;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the TX meter mode
		public string ZZMT(string s)
		{
			int m = -1;
			if(s != "")
				m = Convert.ToInt32(s);

			if(s.Length == parser.nSet &&
				(m > (int) MeterTXMode.FIRST && m < (int) MeterTXMode.LAST))
			{
				String2TXMeter(m);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return TXMeter2String().PadLeft(2,'0');		//Added padleft 4/2/2007 BT
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the MultiRX button status
		public string ZZMU(string s)
		{
			if(s.Length == parser.nSet  && (s == "1" || s == "0"))
			{
				if(s == "1")
					console.CATMultRX = "1";
				else if(s == "0")
					console.CATMultRX= "0";
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATMultRX;
			}
			else
				return parser.Error1;
		}


		#endregion Extended CAT Methods ZZG-ZZM

		#region Extended CAT Methods ZZN-ZZQ

		//Sets or reads Noise Blanker 2 status
		public string ZZNA(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.CATNB1 = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATNB1.ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the Noise Blanker 2 status
		public string ZZNB(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.CATNB2 = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATNB2.ToString();
			}
			else
			{
				return parser.Error1;
			}

		}

		// Sets or reads the Noise Blanker 1 threshold
		public string ZZNL(string s)
		{
			if(s.Length == parser.nSet)
			{
				console.SetupForm.CATNB1Threshold = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.SetupForm.CATNB1Threshold);
			}
			else
			{
				return parser.Error1;
			}

		}

		// Sets or reads the Noise Blanker 2 threshold
		public string ZZNM(string s)
		{
			if(s.Length == parser.nSet)
			{
				console.SetupForm.CATNB2Threshold = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.SetupForm.CATNB2Threshold);
			}
			else
			{
				return parser.Error1;
			}

		}


		// Sets or reads the Noise Reduction status
		//		public string ZZNR()
		//		{
		//			int nr = console.CATNR;
		//			return nr.ToString();
		//		}

		public string ZZNR(string s)
		{
			int sx = 0;

			if(s != "")
				sx = Convert.ToInt32(s);

			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.CATNR = sx;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATNR.ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the ANF button status
		public string ZZNT(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.CATANF = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATANF.ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the RX1 antenna
		public string ZZOA(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				if(s.Length > 0)
					n = Int32.Parse(s);

				if(console.fwcAntForm != null)
				{
					if(s.Length == parser.nSet && (n >= 0 && n <= 4))
					{
						console.fwcAntForm.RX1Ant = String2Antenna(s);
						return "";
					}
					else if(s.Length == parser.nGet)
						return Antenna2String(console.fwcAntForm.RX1Ant);
					else
						return parser.Error1;
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the RX2 antenna (if RX2 installed)
		public string ZZOB(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				if(s.Length > 0)
					n = Int32.Parse(s);

				if(console.fwcAntForm != null && console.fwcAntForm.RX2OK)
				{
					if(s.Length == parser.nSet && ((n >= 0 && n <= 1) || (n >= 5 && n <= 6) ))
					{
						console.fwcAntForm.RX2Ant = String2Antenna(s);
						return "";
					}
					else if(s.Length == parser.nGet)
						return Antenna2String(console.fwcAntForm.RX2Ant);
					else
						return parser.Error1;
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the TX antenna
		public string ZZOC(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				if(s.Length > 0)
					n = Int32.Parse(s);

				if(console.fwcAntForm != null)
				{
					if(s.Length == parser.nSet && (n >= 1 && n <= 3))
					{
						console.fwcAntForm.TXAnt = String2Antenna(s);
						return "";
					}
					else if(s.Length == parser.nGet)
						return Antenna2String(console.fwcAntForm.TXAnt);
					else
						return parser.Error1;
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the current Antenna Mode
		public string ZZOD(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				if(s.Length > 0)
					n = Int32.Parse(s);

				if(s.Length == parser.nSet && (n == 0 || n == 1))
				{
					console.fwcAntForm.CurrentAntMode = String2AntMode(s);
					return "";
				}
				else if(s.Length == parser.nGet)
					return AntMode2String(console.fwcAntForm.CurrentAntMode);
				else
					return parser.Error1;
			}
			else
				return parser.Error1;

		}

		//Sets or reads the RX1 External Antenna checkbox
		public string ZZOE(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				if(s.Length == parser.nSet)
				{
					if(s == "0")
						console.fwcAntForm.RX1Loop = false;
					else
						console.fwcAntForm.RX1Loop = true;
					return "";
				}
				else if(s.Length == parser.nGet)
				{
					bool ans = console.fwcAntForm.RX1Loop;
					if(ans)
						return "1";
					else
						return "0";
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the TX relay RCA jack
		public string ZZOF(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				string ans = "";
				bool rly1 = false;
				bool rly2 = false;
				bool rly3 = false;

				if(s.Length == parser.nSet)
				{
					if(s.Substring(0,1) == "1")
						console.fwcAntForm.RCATX1 = true;
					else
						console.fwcAntForm.RCATX1 = false;

					if(s.Substring(1,1) == "1")
						console.fwcAntForm.RCATX2 = true;
					else
						console.fwcAntForm.RCATX2 = false;

					if(s.Substring(2,1) == "1")
						console.fwcAntForm.RCATX3 = true;
					else
						console.fwcAntForm.RCATX3 = false;

					return "";
				}
				else if(s.Length == parser.nGet)
				{
					rly1 = console.fwcAntForm.RCATX1;
					rly2 = console.fwcAntForm.RCATX2;
					rly3 = console.fwcAntForm.RCATX3;

					if(rly1)
						ans += "1";
					else
						ans += "0";
					if(rly2)
						ans += "1";
					else
						ans += "0";
					if(rly3)
						ans += "1";
					else
						ans += "0";

					return ans;
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}


		//Sets or reads the TX Relay Delay enables
		public string ZZOG(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				string ans = "";
				bool rly1 = false;
				bool rly2 = false;
				bool rly3 = false;

				if(s.Length == parser.nSet)
				{
					if(s.Substring(0,1) == "1")
						console.fwcAntForm.TX1DelayEnable = true;
					else
						console.fwcAntForm.TX1DelayEnable = false;

					if(s.Substring(1,1) == "1")
						console.fwcAntForm.TX2DelayEnable = true;
					else
						console.fwcAntForm.TX2DelayEnable = false;

					if(s.Substring(2,1) == "1")
						console.fwcAntForm.TX3DelayEnable = true;
					else
						console.fwcAntForm.TX3DelayEnable = false;

					return "";
				}
				else if(s.Length == parser.nGet)
				{
					rly1 = console.fwcAntForm.TX1DelayEnable;
					rly2 = console.fwcAntForm.TX2DelayEnable;
					rly3 = console.fwcAntForm.TX3DelayEnable;

					if(rly1)
						ans += "1";
					else
						ans += "0";
					if(rly2)
						ans += "1";
					else
						ans += "0";
					if(rly3)
						ans += "1";
					else
						ans += "0";

					return ans;
				}
				else
					return parser.Error1;				
			}else
				return parser.Error1;
		}

		//Sets or reads the TX Relay Delays
		public string ZZOH(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				string ans = "";
				uint delay = 0;
				string relay = s.Substring(0,1);

				if(s.Length == parser.nSet)
				{
					delay = uint.Parse(s.Substring(1,4));

					if(relay == "1")
						console.fwcAntForm.TX1Delay = delay;
					else
					if(relay == "2")
						console.fwcAntForm.TX2Delay = delay;
					else
					if(relay == "3")
						console.fwcAntForm.TX3Delay = delay;

					return "";
				}
				else if(s.Length == parser.nGet)
				{
					if(relay == "1")
						delay = console.fwcAntForm.TX1Delay;
					else
					if(relay == "2")
						delay = console.fwcAntForm.TX2Delay;
					else
					if(relay == "3")
						delay = console.fwcAntForm.TX3Delay;

					ans = AddLeadingZeros((int)delay);
					return " "+ans.Substring(1,4);
				}
				else 
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		public string ZZOJ(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				if(s.Length == parser.nSet)
				{
					if(s == "0")
						console.fwcAntForm.AntLock = false;
					else
						console.fwcAntForm.AntLock = true;
					return "";
				}
				else if(s.Length == parser.nGet)
				{
					bool Lock = console.fwcAntForm.AntLock;
					if(Lock)
						return "1";
					else
						return "0";
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the Preamp thumbwheel
		public string ZZPA(string s)
		{
			int n = 0;
			if(s != "")
				n = Convert.ToInt32(s);

			if(s.Length == parser.nSet)
			{
				if(console.CurrentModel == Model.SDR1000)
				{
					if((n > (int) PreampMode.FIRST && n < (int) PreampMode.LAST))
					{
						console.CATPreamp = n;
						return "";
					}
					else
						return parser.Error1;
				}
				else if(console.CurrentModel == Model.FLEX5000)
				{
					if(n == 0 || n == 1)
					{
						console.CATPreamp = n;
						return "";
					}
					else
						return parser.Error1;
				}
                else if (console.CurrentModel == Model.FLEX3000)
                {
                    if (n <= 3 && n >= 0)
                    {
                        console.CATPreamp = n;
                        return "";
                    }
                    else
                        return parser.Error1;
                }
				else
					return parser.Error1;
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATPreamp.ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the Drive level
		public string ZZPC(string s)
		{
			int pwr = 0;

			if(s.Length == parser.nSet)
			{
				pwr = Convert.ToInt32(s);
				console.PWR = pwr;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.PWR);
			}
			else
			{
				return parser.Error1;
			}
		}

		//Centers the Display Pan scroll
		public string ZZPD()
		{
			console.CATDispCenter = "1";
			return "";
		}

//		//Sets or reads the Speech Compressor button status
//		public string ZZPK(string s)
//		{
//			if(s.Length == parser.nSet)
//			{
//				if(s == "0")
//					console.COMP = false;
//				else if(s == "1")
//					console.COMP = true;
//				return "";
//			}
//			else if(s.Length == parser.nGet)
//			{
//				bool comp = console.COMP;
//				if(comp)
//					return "1";
//				else
//					return "0";
//			}
//			else
//			{
//				return "";
//			}
//		}
//
//		// Sets or reads the Speech Compressor threshold
//		public string ZZPL(string s)
//		{
//			if(s.Length == parser.nSet)
//			{
//				console.SetupForm.CATCompThreshold = Convert.ToInt32(s);
//				return "";
//			}
//			else if(s.Length == parser.nGet)
//			{
//				return AddLeadingZeros(console.SetupForm.CATCompThreshold);
//			}
//			else
//			{
//				return parser.Error1;
//			}
//
//		}

		// Sets or reads the Speech Compressor threshold
//		public string ZZPL(string s)
//		{
//			if(s.Length == parser.nSet)
//			{
//				console.SetupForm.CATCompThreshold = Convert.ToInt32(s);
//				return "";
//			}
//			else if(s.Length == parser.nGet)
//			{
//				return AddLeadingZeros(console.SetupForm.CATCompThreshold);
//			}
//			else
//			{
//				return parser.Error1;
//			}
//
//		}

		//Sets or reads the Display Peak button status
		public string ZZPO(string s)
		{
			if(s.Length == parser.nSet)
			{
				console.CATDispPeak = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATDispPeak;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the Power button status
		public string ZZPS(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.PowerOn = false;
				else if(s == "1")
					console.PowerOn = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool pwr = console.PowerOn;
				if(pwr)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets the Display Zoom buttons
		public string ZZPZ(string s)
		{
			if(s.Length == parser.nSet)
			{
				console.CATDispZoom = s;

				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATDispZoom;
			}
			else
				return parser.Error1;

		}

		// Reads the Quick Memory Save value
		public string ZZQM()
		{
			return StrVFOFreq("C");
		}

		// Recalls Memory Quick Save
		public string ZZQR()
		{
			console.CATMemoryQR();
			return "";
		}

		//Saves Quick Memory value
		public string ZZQS()
		{
			console.CATMemoryQS();
			return "";
		}


		#endregion Extended CAT Methods ZZN-ZZQ

		#region Extended CAT Methods ZZR-ZZZ

		// Sets or reads the RTTY Offset Enable VFO A checkbox
		public string ZZRA(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.SetupForm.RttyOffsetEnabledA = false;
				else if(s == "1") 
					console.SetupForm.RttyOffsetEnabledA = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool ans = console.SetupForm.RttyOffsetEnabledA;
				if(ans)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the RTTY Offset Enable VFO B checkbox
		public string ZZRB(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.SetupForm.RttyOffsetEnabledB = false;
				else if(s == "1") 
					console.SetupForm.RttyOffsetEnabledB = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool ans = console.SetupForm.RttyOffsetEnabledB;
				if(ans)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}

		//Clears the RIT frequency
		public string ZZRC()
		{
			console.RITValue = 0;
			return "";
		}

		//Decrements RIT
		public string ZZRD(string s)
		{
			if(s.Length == parser.nSet)
			{
				return ZZRF(s);
			}
			else if(s.Length == parser.nGet && console.RITOn)
			{
				switch(console.RX1DSPMode)
				{
					case DSPMode.CWL:
					case DSPMode.CWU:
						console.RITValue -= 10;
						break;
					case DSPMode.LSB:
					case DSPMode.USB:
						console.RITValue -= 50;
						break;
				}
				return "";
			}
			else
				return parser.Error1;
		}

		// Sets or reads the RIT frequency value
		public string ZZRF(string s)
		{
			int n = 0;
			int x = 0;
			string sign;

			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Max(-9999, n);
				n = Math.Min(9999, n);
			}

			if(s.Length == parser.nSet)
			{
				console.RITValue = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				x = console.RITValue;
				if(x >= 0)
					sign = "+";
				else
					sign = "-";
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}
		}


		//Sets or reads the RTTY DIGH offset frequency ud counter
		public string ZZRH(string s)
		{
			int n = 0;
			int x = 0;
			string sign;

			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Max(-3000, n);
				n = Math.Min(3000, n);
			}

			if(s.Length == parser.nSet)
			{
				console.SetupForm.RttyOffsetHigh = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				x = console.SetupForm.RttyOffsetHigh;
				if(x >= 0)
					sign = "+";
				else
					sign = "-";
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}

		}

		//Sets or reads the RTTY DIGL offset frequency ud counter
		public string ZZRL(string s)
		{
			int n = 0;
			int x = 0;
			string sign;

			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Max(-3000, n);
				n = Math.Min(3000, n);
			}

			if(s.Length == parser.nSet)
			{
				console.SetupForm.RttyOffsetLow = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				x = console.SetupForm.RttyOffsetLow;
				if(x >= 0)
					sign = "+";
				else
					sign = "-";
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}

		}

		// Reads the Console RX meter
		public string ZZRM(string s)
		{
			string output = parser.Error1;
			if(!console.MOX)
			{
				switch(s)
				{
					case "0":
						output = console.CATReadSigStrength().PadLeft(20);
						break;
					case "1":
						output = console.CATReadAvgStrength().PadLeft(20);
						break;
					case "2":
						output = console.CATReadADC_L().PadLeft(20);
						break;
					case "3":
						output = console.CATReadADC_R().PadLeft(20);
						break;
				}
			}
			else
			{
				switch(s)
				{
					case "4":
						output = console.CATReadALC().PadLeft(20);
						break;
					case "5":
						output = console.CATReadFwdPwr().PadLeft(20);
						break;
					case "6":
						output = console.CATReadPeakPwr().PadLeft(20);
						break;
					case "7":
						output = console.CATReadRevPwr().PadLeft(20);
						break;
					case "8":
						output = console.CATReadSWR().PadLeft(20);
						break;
				}
			}
			return output;
		}
		//Sets or reads the RX2 button status
		public string ZZRS(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.RX2Enabled = false;
				else if(s == "1") 
					console.RX2Enabled = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.RX2Enabled)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}


		//Sets or reads the RIT button status
		public string ZZRT(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.RITOn = false;
				else if(s == "1") 
					console.RITOn = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool rit = console.RITOn;
				if(rit)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}

		//Increments RIT
		public string ZZRU(string s)
		{
			if(s.Length == parser.nSet)
			{
				return ZZRF(s);
			}
			else if(s.Length == parser.nGet && console.RITOn)
			{
				switch(console.RX1DSPMode)
				{
					case DSPMode.CWL:
					case DSPMode.CWU:
						console.RITValue += 10;
						break;
					case DSPMode.LSB:
					case DSPMode.USB:
						console.RITValue += 50;
						break;
				}
				return "";
			}
			else
				return parser.Error1;		}

		//Moves VFO A down one Tune Step
		public string ZZSA()
		{
			int step = console.StepSize;
			double[] wheel_tune_list;
			wheel_tune_list = new double[13];		// initialize wheel tuning list array
			wheel_tune_list[0]  =  0.000001;
			wheel_tune_list[1]  =  0.000010;
			wheel_tune_list[2]  =  0.000050;
			wheel_tune_list[3]  =  0.000100;
			wheel_tune_list[4]  =  0.000250;
			wheel_tune_list[5]  =  0.000500;
			wheel_tune_list[6]  =  0.001000;
			wheel_tune_list[7]  =  0.005000;
			wheel_tune_list[8]  =  0.009000;
			wheel_tune_list[9]  =  0.010000;
			wheel_tune_list[10] =  0.100000;
			wheel_tune_list[11] =  1.000000;
			wheel_tune_list[12] = 10.000000;

			console.VFOAFreq = console.CATVFOA - wheel_tune_list[step];
			return "";
		}

		//Moves VFO A up one Tune Step
		public string ZZSB()
		{
			int step = console.StepSize;
			double[] wheel_tune_list;
			wheel_tune_list = new double[13];		// initialize wheel tuning list array
			wheel_tune_list[0]  =  0.000001;
			wheel_tune_list[1]  =  0.000010;
			wheel_tune_list[2]  =  0.000050;
			wheel_tune_list[3]  =  0.000100;
			wheel_tune_list[4]  =  0.000250;
			wheel_tune_list[5]  =  0.000500;
			wheel_tune_list[6]  =  0.001000;
			wheel_tune_list[7]  =  0.005000;
			wheel_tune_list[8]  =  0.009000;
			wheel_tune_list[9]  =  0.010000;
			wheel_tune_list[10] =  0.100000;
			wheel_tune_list[11] =  1.000000;
			wheel_tune_list[12] = 10.000000;

			console.VFOAFreq = console.CATVFOA + wheel_tune_list[step];
			return "";
		}

		//Moves the mouse wheel tuning step down
		public string ZZSD()
		{
			console.CATTuneStepDown = "1";
			return "";
		}

		// ZZSFccccwwww  Set Filter, cccc=center freq www=width both in hz 
		public string ZZSF(string s)
		{
			int center = Convert.ToInt32(s.Substring(0,4), 10); 
			int width = Convert.ToInt32(s.Substring(4), 10); 
			SetFilterCenterAndWidth(center, width); 
			return "";
		}


        //Moves VFO B down one Tune Step
        public string ZZSG()
        {
            int step = console.StepSize;
            double[] wheel_tune_list;
            wheel_tune_list = new double[13];		// initialize wheel tuning list array
            wheel_tune_list[0] = 0.000001;
            wheel_tune_list[1] = 0.000010;
            wheel_tune_list[2] = 0.000050;
            wheel_tune_list[3] = 0.000100;
            wheel_tune_list[4] = 0.000250;
            wheel_tune_list[5] = 0.000500;
            wheel_tune_list[6] = 0.001000;
            wheel_tune_list[7] = 0.005000;
            wheel_tune_list[8] = 0.009000;
            wheel_tune_list[9] = 0.010000;
            wheel_tune_list[10] = 0.100000;
            wheel_tune_list[11] = 1.000000;
            wheel_tune_list[12] = 10.000000;

            console.VFOBFreq = console.CATVFOB - wheel_tune_list[step];
            return "";
        }

        //Moves VFO B up one Tune Step
        public string ZZSH()
        {
            int step = console.StepSize;
            double[] wheel_tune_list;
            wheel_tune_list = new double[13];		// initialize wheel tuning list array
            wheel_tune_list[0] = 0.000001;
            wheel_tune_list[1] = 0.000010;
            wheel_tune_list[2] = 0.000050;
            wheel_tune_list[3] = 0.000100;
            wheel_tune_list[4] = 0.000250;
            wheel_tune_list[5] = 0.000500;
            wheel_tune_list[6] = 0.001000;
            wheel_tune_list[7] = 0.005000;
            wheel_tune_list[8] = 0.009000;
            wheel_tune_list[9] = 0.010000;
            wheel_tune_list[10] = 0.100000;
            wheel_tune_list[11] = 1.000000;
            wheel_tune_list[12] = 10.000000;

            console.VFOBFreq = console.CATVFOB + wheel_tune_list[step];
            return "";
        }

		// Reads the S Meter value
		public string ZZSM(string s)
		{
			int sm = 0;

			if(s == "0" || s == "1")	// read the main transceiver s meter
			{
				float num = 0f;
                if (console.PowerOn)
                    if (s == "0")
                        num = DttSP.CalculateRXMeter(0, 0, DttSP.MeterType.SIGNAL_STRENGTH);
                    else
                        num = DttSP.CalculateRXMeter(2, 0, DttSP.MeterType.SIGNAL_STRENGTH);

				num = num+console.MultiMeterCalOffset+console.PreampOffset;

				num = Math.Max(-140, num);
				num = Math.Min(-10, num);

				sm = ((int)num+140)*2;

				return sm.ToString().PadLeft(3,'0');
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets or reads the VFO Split status
		public string ZZSP(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "0")
					console.VFOSplit = false;
				else
					console.VFOSplit = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool retval = console.VFOSplit;
				if(!retval)
					return "0";
				else
					return "1";
			}
			else
			{
				return parser.Error1;
			}

		}

		// Sets or reads the Squelch on/off status
		public string ZZSO(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.CATSquelch = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.CATSquelch.ToString();
			}
			else
				return parser.Error1;

		}

		// Sets or reads the SDR-1000 Squelch contorl
		public string ZZSQ(string s)
		{
			int level = 0;

			if(s.Length == parser.nSet)
			{
				level = Convert.ToInt32(s);
				level = Math.Max(0, level);			// lower bound
				level = Math.Min(160, level);		// upper bound
				console.Squelch = level;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.Squelch);
			}
			else
			{
				return parser.Error1;
			}

		}
		
		//Reads or sets the Spur Reduction button status
		public string ZZSR(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "1")
					console.SpurReduction = true;
				else
					console.SpurReduction = false;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.SpurReduction)
					return "1";
				else
					return "0";
			}
			else
				return parser.Error1;


		}

        public string ZZSS()
        {
            console.CWXForm.CWXStop();
            return "";
        }

		// Reads the current console step size (read-only property)
		public string ZZST()
		{
			int step = console.StepSize;
			return Step2String(step);
		}

		// Moves the mouse wheel step tune up
		public string ZZSU()
		{
			console.CATTuneStepUp = "1";
			return "";
		}

        //Swaps VFO A/B TX buttons
        public string ZZSW(string s)
        {
            if (s.Length == parser.nSet && (s == "0" || s == "1"))
            {
                if (s == "0")
                    console.SwapVFOA_BTX = false;
                else if (s == "1")
                    console.SwapVFOA_BTX = true;

                return "";
            }
            else if (s.Length == parser.nGet)
            {
                bool retval = console.SwapVFOA_BTX;
                if (retval)
                    return "1";
                else
                    return "0";
            }
            else
            {
                return parser.Error1;
            }

        }

		// Sets or reads the Show TX Filter checkbox
		public string ZZTF(string s)
		{
			switch(console.RX1DSPMode)
			{
				case DSPMode.CWL:
				case DSPMode.CWU:
				case DSPMode.DIGL:
				case DSPMode.DIGU:
					return parser.Error1;
				default:
					if(s.Length == parser.nSet && (s == "0" || s == "1"))
					{
						if(s == "1")
							console.ShowTXFilter = true;
						else
							console.ShowTXFilter = false;
						return "";
					}
					else if(s.Length == parser.nGet)
					{
						if(console.ShowTXFilter)
							return "1";
						else
							return "0";
					}
					else
						return parser.Error1;
			}
		}


		// Sets or reads the TX filter high setting
		public string ZZTH(string s)
		{
			int th = 0;

			if(s.Length == parser.nSet)	// check the min/max control settings
			{
				th = Convert.ToInt32(s);
				th = Math.Max(500, th);
				th = Math.Min(20000, th);
				console.SetupForm.TXFilterHigh = th;		
				return "";
			}
			else if(s.Length == parser.nGet)	// if this is a read command
			{
				return AddLeadingZeros(console.SetupForm.TXFilterHigh);
			}
			else
			{
				return parser.Error1;	// return a ?
			}
		}

		//Inhibits power output when using external antennas, tuners, etc.
		public string ZZTI(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "0")
				{
					console.RXOnly = false;
				}
				else if(s == "1")
				{
					console.RXOnly = true;
					console.MOX = false;
				}

				return "";
			}
			else
				return parser.Error1;
		}

		// Sets or reads the TX filter low setting
		public string ZZTL(string s)
		{
			int tl = 0;

			if(s.Length == parser.nSet)	// check the min/max control settings
			{
				tl = Convert.ToInt32(s);
				tl = Math.Max(0, tl);
				tl = Math.Min(2000, tl);
				console.SetupForm.TXFilterLow = tl;		
				return "";
			}
			else if(s.Length == parser.nGet)	// if this is a read command
			{
				return AddLeadingZeros(console.SetupForm.TXFilterLow);
			}
			else
			{
				return parser.Error1;	// return a ?
			}
		}

		//Sets or reads the Tune Power level
		public string ZZTO(string s)
		{
			int tl = 0;

			if(s.Length == parser.nSet)	// check the min/max control settings
			{
				tl = Convert.ToInt32(s);
				tl = Math.Max(0, tl);
				tl = Math.Min(100, tl);
				console.SetupForm.TunePower = tl;		
				return "";
			}
			else if(s.Length == parser.nGet)	// if this is a read command
			{
				return AddLeadingZeros(console.SetupForm.TunePower);
			}
			else
			{
				return parser.Error1;	// return a ?
			}
		}


		//Sets or reads the TX Profile
		public string ZZTP(string s)
		{
			int items = console.CATTXProfileCount;
			int cnt = 0;
			if(s != "")
				cnt = Convert.ToInt32(s);

			if(s.Length == parser.nSet && cnt < items)
			{
				console.CATTXProfile = cnt;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.CATTXProfile);
			}
			else
				return parser.Error1;
		}

		// Reads the Flex 5000 temperature sensor
        public string ZZTS()
        {
            Model model = console.CurrentModel;
            if (model != Model.FLEX5000 &&
                model != Model.FLEX3000)
                return parser.Error1;

            int val = 0;
            float volts = 0.0f;
            double temp2 = 0.0f;

            int chan = 4;
            if (model == Model.FLEX3000)
                chan = 3;

            FWC.ReadPAADC(chan, out val);
            volts = (float)val / 4096 * 2.5f;
            double temp = 301 - volts * 1000 / 2.2;

            if (temp >= 100f)
                temp2 = Math.Round(temp, 1);
            else
                temp2 = Math.Round(temp, 2);

            return temp2.ToString().PadLeft(5, '0');
        }

		// Sets or reads the TUN button on/off status
		public string ZZTU(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "0")
					console.TUN = false;
				else if(s == "1")
					console.TUN = true;

				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool retval = console.TUN;
				if(retval)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}

		}

		//Sets or reads the MOX button status
		public string ZZTX(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "0")
					console.CATPTT = false;
				else if(s == "1")
					console.CATPTT = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.CATPTT)
					return "1";
				else
					return "0";
			}
			else
				return parser.Error1;

		}

		//Reads the XVTR Band Names
		public string ZZUA()
		{
			string ans = console.CATGetXVTRBandNames();
			return ans;
		}

		// Reads or sets the VAC Enable checkbox (Setup Form)
		public string ZZVA(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "1")
					console.SetupForm.VACEnable = true;
				else
					console.SetupForm.VACEnable = false;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.SetupForm.VACEnable)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}

		}


		/// <summary>
		/// Sets or reads the VAC RX Gain 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string ZZVB(string s)
		{
			int n = 0;
			int x = 0;
			string sign;

			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Max(-40, n);
				n = Math.Min(20, n);
			}

			if(s.Length == parser.nSet)
			{
				console.VACRXGain = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				x = console.VACRXGain;
				if(x >= 0)
					sign = "+";
				else
					sign = "-";
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}
		}

		/// <summary>
		/// Sets or reads the VAC TX Gain
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string ZZVC(string s)
		{
			int n = 0;
			int x = 0;
			string sign;

			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Max(-40, n);
				n = Math.Min(20, n);
			}

			if(s.Length == parser.nSet)
			{
				console.VACTXGain = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				x = console.VACTXGain;
				if(x >= 0)
					sign = "+";
				else
					sign = "-";
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}
		}

		/// <summary>
		/// Sets or reads the VAC Sample Rate
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string ZZVD(string s)
		{
			int n = -1;

			if(s.Length == parser.nSet)
			{
				n = Convert.ToInt32(s);
				switch(n)
				{
					case 0:
						console.VACSampleRate = "6000";
						break;
					case 1:
						console.VACSampleRate = "8000";
						break;
					case 2:
						console.VACSampleRate = "11025";
						break;
					case 3:
						console.VACSampleRate = "12000";
						break;
					case 4:
						console.VACSampleRate = "24000";
						break;
					case 5:
						console.VACSampleRate = "22050";
						break;
					case 6:
						console.VACSampleRate = "44100";
						break;
					case 7:
						console.VACSampleRate = "48000";
						break;
				}
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				string rate = console.VACSampleRate;
				string ans = "";

				switch(rate)
				{
					case "6000":
						ans = "0";
						break;
					case "8000":
						ans = "1";
						break;
					case "11025":
						ans = "2";
						break;
					case "12000":
						ans = "3";
						break;
					case "24000":
						ans = "4";
						break;
					case "22050":
						ans = "5";
						break;
					case "41000":
						ans = "6";
						break;
					case "48000":
						ans = "7";
						break;
					default:
						ans = parser.Error1;
						break;
				}
				return ans;
			}
			else
			{
				return parser.Error1;
			}
		}

		//Reads or sets the VOX Enable button status
		public string ZZVE(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "1")
					console.VOXEnable = true;
				else
					console.VOXEnable = false;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.VOXEnable)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}


		/// <summary>
		/// Sets or reads the VAC Stereo checkbox
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string ZZVF(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "1")
					console.VACStereo = true;
				else
					console.VACStereo = false;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				if(console.VACStereo)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}

		}

		//Reads or set the VOX Gain control
		public string ZZVG(string s)
		{
			int n = 0;

			if(s != null && s != "")
				n = Convert.ToInt32(s);
			n = Math.Max(0, n);
			n = Math.Min(1000, n);

			if(s.Length == parser.nSet)
			{
				console.VOXSens = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros((int) console.VOXSens);
			}
			else
			{
				return parser.Error1;
			}

		}

		// Reads or sets the I/Q to VAC checkbox on the setup form
		public string ZZVH(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "0")
					console.SetupForm.IQOutToVAC = false;
				else if(s == "1")
					console.SetupForm.IQOutToVAC = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool retval = console.SetupForm.IQOutToVAC;
				if(retval)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}

		// Reads or sets the VFO Lock button status
		public string ZZVL(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				if(s == "0")
					console.CATVFOLock = false;
				else if(s == "1")
					console.CATVFOLock = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool retval = console.CATVFOLock;
				if(retval)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}

		// Returns the version number of the PowerSDR program
		public string ZZVN()
		{
			return console.CATGetVersion().PadLeft(10,'0');
		}

		// Sets the VFO swap status
		// write only
		public string ZZVS(string s)
		{
			if(s.Length == parser.nSet & Convert.ToInt32(s) <= 3)
			{
				console.CATVFOSwap(s);
				return "";
			}
			else
			{
				return parser.Error1;
			}

		}

		//Sets or reads the F5K Mixer Mic Gain
		public string ZZWA(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
					int n = 0;
					int x = 0;
					string sign;

					if(s != "")
					{
						n = Int32.Parse(s);
						n = Math.Max(-128, n);
						n = Math.Min(0, n);
					}
				
					if(s.Length == parser.nSet)
					{
						if(console.fwcMixForm.MicInputSelected == "1")
						{
							console.fwcMixForm.MicInput = n;
							return "";
						}
						else
							return parser.Error1;

					}
					else if(s.Length == parser.nGet)
					{
						x = console.fwcMixForm.MicInput;
						if(x >= 0)
							sign = "+";
						else
							sign = "-";
						return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
					}
					else
						return parser.Error1;
				}
				else
					return parser.Error1;
		}

		//Sets or reads the F5K Line In RCA level
		public string ZZWB(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				int x = 0;
				string sign;

				if(s != "")
				{
					n = Int32.Parse(s);
					n = Math.Max(-128, n);
					n = Math.Min(0, n);
				}
				
				if(s.Length == parser.nSet)
				{
					if(console.fwcMixForm.LineInRCASelected == "1")
					{
						console.fwcMixForm.LineInRCA = n;
						return "";
					}
					else
						return parser.Error1;

				}
				else if(s.Length == parser.nGet)
				{
					x = console.fwcMixForm.LineInRCA;
					if(x >= 0)
						sign = "+";
					else
						sign = "-";
					return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the F5K Line In Phono level
		public string ZZWC(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				int x = 0;
				string sign;

				if(s != "")
				{
					n = Int32.Parse(s);
					n = Math.Max(-128, n);
					n = Math.Min(0, n);
				}
				
				if(s.Length == parser.nSet)
				{
					if(console.fwcMixForm.LineInPhonoSelected == "1")
					{
						console.fwcMixForm.LineInPhono = n;
						return "";
					}
					else
						return parser.Error1;

				}
				else if(s.Length == parser.nGet)
				{
					x = console.fwcMixForm.LineInPhono;
					if(x >= 0)
						sign = "+";
					else
						sign = "-";
					return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the F5K Mixer Line In DB9 level
		public string ZZWD(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				int x = 0;
				string sign;

				if(s != "")
				{
					n = Int32.Parse(s);
					n = Math.Max(-128, n);
					n = Math.Min(0, n);
				}
				
				if(s.Length == parser.nSet)
				{
					if(console.fwcMixForm.LineInDB9Selected == "1")
					{
						console.fwcMixForm.LineInDB9 = n;
						return "";
					}
					else
						return parser.Error1;

				}
				else if(s.Length == parser.nGet)
				{
					x = console.fwcMixForm.LineInDB9;
					if(x >= 0)
						sign = "+";
					else
						sign = "-";
					return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}


		// Sets or reads the F5K Mixer Mic Selected Checkbox
		public string ZZWE(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.MicInputSelected = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.MicInputSelected;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the F5K Mixer Line In RCA Checkbox
		public string ZZWF(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.LineInRCASelected = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.LineInRCASelected;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the F5K Mixer Line In Phono Checkbox
		public string ZZWG(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.LineInPhonoSelected = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.LineInPhonoSelected;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the F5K Mixer Line In DB9 Checkbox
		public string ZZWH(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.LineInDB9Selected = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.LineInDB9Selected;
			}
			else
				return parser.Error1;
		}


		// Sets or reads the F5K Mixer Mute All Checkbox
		public string ZZWJ(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.InputMuteAll = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.InputMuteAll;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the F5K Mixer Internal Speaker level
		public string ZZWK(string s)
		{
//			if(console.CurrentModel == Model.FLEX5000)
			if(FWCEEPROM.Model == 1) // FLEX-5000C
			{
				int n = 0;
				int x = 0;

				if(s != "")
				{
					n = Int32.Parse(s);
					n = Math.Max(128, n);
					n = Math.Min(255, n);
				}
				
				if(s.Length == parser.nSet)
				{
					if(console.fwcMixForm.InternalSpkrSelected == "1")
					{
						console.fwcMixForm.InternalSpkr = n;
						return "";
					}
					else
						return parser.Error1;

				}
				else if(s.Length == parser.nGet)
				{
					x = console.fwcMixForm.InternalSpkr;
					return AddLeadingZeros(x);
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the F5K Mixer External Speaker level
		public string ZZWL(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				int x = 0;

				if(s != "")
				{
					n = Int32.Parse(s);
					n = Math.Max(128, n);
					n = Math.Min(255, n);
				}
				
				if(s.Length == parser.nSet)
				{
					if(console.fwcMixForm.ExternalSpkrSelected == "1")
					{
						console.fwcMixForm.ExternalSpkr = n;
						return "";
					}
					else
						return parser.Error1;

				}
				else if(s.Length == parser.nGet)
				{
					x = console.fwcMixForm.ExternalSpkr;
					return AddLeadingZeros(x);
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the F5K Mixer Headphone level
		public string ZZWM(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				int x = 0;

				if(s != "")
				{
					n = Int32.Parse(s);
					n = Math.Max(128, n);
					n = Math.Min(255, n);
				}
				
				if(s.Length == parser.nSet)
				{
					if(console.fwcMixForm.HeadphoneSelected == "1")
					{
						console.fwcMixForm.Headphone = n;
						return "";
					}
					else
						return parser.Error1;

				}
				else if(s.Length == parser.nGet)
				{
					x = console.fwcMixForm.Headphone;
					return AddLeadingZeros(x);
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		//Sets or reads the F5K Mixer Line Out RCA level
		public string ZZWN(string s)
		{
			if(console.CurrentModel == Model.FLEX5000)
			{
				int n = 0;
				int x = 0;

				if(s != "")
				{
					n = Int32.Parse(s);
					n = Math.Max(128, n);
					n = Math.Min(255, n);
				}
				
				if(s.Length == parser.nSet)
				{
					if(console.fwcMixForm.LineOutRCASelected == "1")
					{
						console.fwcMixForm.LineOutRCA = n;
						return "";
					}
					else
						return parser.Error1;

				}
				else if(s.Length == parser.nGet)
				{
					x = console.fwcMixForm.LineOutRCA;
					return AddLeadingZeros(x);
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the F5K Mixer Internal Speaker Selected Checkbox
		public string ZZWO(string s)
		{
			uint val = FWCEEPROM.Model;
			if(val != 0)
			{
				if(s.Length == parser.nSet && (s == "0" || s == "1"))
				{
					console.fwcMixForm.InternalSpkrSelected = s;
					return "";
				}
				else if(s.Length == parser.nGet)
				{
					return console.fwcMixForm.InternalSpkrSelected;
				}
				else
					return parser.Error1;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the F5K Mixer External Speaker Selected Checkbox
		public string ZZWP(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.ExternalSpkrSelected = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.ExternalSpkrSelected;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the F5K Mixer Headphone Selected Checkbox
		public string ZZWQ(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.HeadphoneSelected = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.HeadphoneSelected;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the F5K Mixer Line Out RCA Selected Checkbox
		public string ZZWR(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.LineOutRCASelected = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.LineOutRCASelected;
			}
			else
				return parser.Error1;
		}

		// Sets or reads the F5K Mixer Output Mute All Checkbox
		public string ZZWS(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
			{
				console.fwcMixForm.OutputMuteAll = s;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return console.fwcMixForm.OutputMuteAll;
			}
			else
				return parser.Error1;
		}






		// Clears the XIT frequency
		// write only
		public string ZZXC()
		{
			console.XITValue = 0;
			return "";
		}

		// Sets or reads the XIT frequency value
		public string ZZXF(string s)
		{
			int n = 0;
			int x = 0;
			string sign;

			if(s != "")
			{
				n = Convert.ToInt32(s);
				n = Math.Max(-9999, n);
				n = Math.Min(9999, n);
			}

			if(s.Length == parser.nSet)
			{
				console.XITValue = n;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				x = console.XITValue;
				if(x >= 0)
					sign = "+";
				else
					sign = "-";
				// we have to remove the leading zero and replace it with the sign.
				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the XIT button status
		public string ZZXS(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.XITOn = false;
				else
					if(s == "1") 
					console.XITOn = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool xit = console.XITOn;
				if(xit)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}

		//Sets or reads the X2TR button status
		public string ZZXT(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.X2TR = false;
				else if(s == "1")
					console.X2TR = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool x2tr = console.X2TR;
				if(x2tr)
					return "1";
				else
					return "0";
			}
			else
			{
				return parser.Error1;
			}
		}

		public string ZZZB()
		{
			if(console.CATDisplayAvg == 1)
				console.CATZB = "1";
			return "";
		}

		public string ZZZZ()
		{
			console.Siolisten.SIO.ClosePort = true;
			return "";
		}
		#endregion Extended CAT Methods ZZR-ZZZ


		#region Helper methods

		#region General Helpers

		private string AddLeadingZeros(int n)
		{
			string num = n.ToString();

			while(num.Length < parser.nAns)
				num = num.Insert(0,"0");
			
			return num;
		}

		#endregion General Helpers

		#region Antenna Methods

		private string Antenna2String(FWCAnt ant)
		{
			string ans = "";
			switch(ant)
			{
				case FWCAnt.NC:
					ans = "0";
					break;
				case FWCAnt.ANT1:
                    ans = "1";
					break;
				case FWCAnt.ANT2:
					ans = "2";
					break;
				case FWCAnt.ANT3:
					ans = "3";
					break;
				case FWCAnt.RX1IN:
					ans = "4";
					break;
				case FWCAnt.RX2IN:
					ans = "5";
					break;
				case FWCAnt.RX1TAP:
					ans = "6";
					break;
				case FWCAnt.SIG_GEN:
					ans = "7";
					break;
			}
			return ans;
		}

		private FWCAnt String2Antenna(string ant)
		{
			FWCAnt ans = FWCAnt.ANT1;
			switch(ant)
			{
				case "0":
					ans = FWCAnt.NC;
					break;
				case "1":
					ans = FWCAnt.ANT1;
					break;
				case "2":
					ans = FWCAnt.ANT2;
					break;
				case "3":
					ans = FWCAnt.ANT3;
					break;
				case "4":
					ans = FWCAnt.RX1IN;
					break;
				case "5":
					ans = FWCAnt.RX2IN;
					break;
				case "6":
					ans = FWCAnt.RX1TAP;
					break;
				case "7":
					ans = FWCAnt.SIG_GEN;
					break;
			}
			return ans;
		}

		private string AntMode2String(AntMode ant)
		{
			string ans = "0";
			switch(ant)
			{
				case AntMode.Simple:
					break;
				case AntMode.Expert:
					ans = "1";
					break;
			}
			return ans;
		}

		private AntMode String2AntMode(string amode)
		{
			AntMode ans = AntMode.Simple;
			switch(amode)
			{
				case "0":
					break;
				case "1":
					ans = AntMode.Expert;
					break;
			}
			return ans;
		}

		#endregion Antenna Methods

		#region Split Methods

		private void SetKWSplitStatus(string s)
		{
			if(s == "0")
			{
				console.VFOSplit = false;
				lastFR = "0";
				lastFT = "0";
			}
			else if(s == "1" && !console.VFOSplit)
			{
				console.VFOSplit = true;
				lastFR = "0";
				lastFT = "1";
			}
			else if(s == "1" && lastFR == "1" && lastFT == "1" && console.VFOSplit)
			{
				console.VFOSplit = false;
				lastFR = "0";
				lastFT = "0";
			}
			Debug.WriteLine("lastFR = "+lastFR+" lastFT = "+lastFT);
		}

		private string GetP10()
		{
			return lastFT;
		}

		private string GetP12()
		{
			return lastFT;
		}

		#endregion Split Methods

		#region VFO Methods
		// Converts a vfo frequency to a proper CAT frequency string
		private string StrVFOFreq(string vfo)
		{
			double freq = 0;
			string cmd_string = "";

            if (vfo == "A")
                freq = Math.Round(console.CATVFOA, 6);
            else if (vfo == "B")
                freq = Math.Round(console.CATVFOB, 6);
            else if (vfo == "C")
                freq = Convert.ToDouble(console.CATQMSValue);

			
			if((int) freq < 10)
			{
				cmd_string += "0000"+freq.ToString();
			}
			else if((int) freq < 100)
			{
				cmd_string += "000"+freq.ToString();
			}
			else if((int) freq < 1000)
			{
				cmd_string += "00"+freq.ToString();
			}
			else if((int) freq < 10000)
			{
				cmd_string += "0"+freq.ToString();
			}
			else
				cmd_string = freq.ToString();

			// Get rid of the decimal separator and pad the right side with 0's 
			// Modified 05/01/05 BT for globalization
			if(cmd_string.IndexOf(separator) > 0)
				cmd_string = cmd_string.Remove(cmd_string.IndexOf(separator),1);
			cmd_string = cmd_string.PadRight(11,'0');
			return cmd_string;
		}
		#endregion VFO Methods

		#region Filter Methods

		public string Filter2String(Filter f)
		{
			string fw = f.ToString();
			string strfilt = "";
			int retval = 0;
			switch(fw)
			{
				case "F6000":
					strfilt = "6000";
					break;
				case "F4000":
					strfilt = "4000";
					break;
				case "F2600":
					strfilt = "2600";
					break;
				case "F2100":
					strfilt = "2100";
					break;
				case "F1000":
					strfilt = "1000";
					break;
				case "F500":
					strfilt = "0500";
					break;
				case "F250":
					strfilt = "0250";
					break;
				case "F100":
					strfilt = "0100";
					break;
				case "F50":
					strfilt = "0050";
					break;
				case "F25":
					strfilt = "0025";
					break;
				case "VAR1":
					retval = Math.Abs(console.RX1FilterHigh-console.RX1FilterLow);
					strfilt = AddLeadingZeros(retval);
					break;
				case "VAR2":
					retval = Math.Abs(console.RX1FilterHigh-console.RX1FilterLow);
					strfilt = AddLeadingZeros(retval);
					break;
			}
			return strfilt;
		}

		public Filter String2Filter(string f)
		{
			Filter filter = Filter.FIRST;
			switch(f)
			{
				case "6000":
					filter = Filter.F1;
					break;
				case "4000":
					filter = Filter.F2;
					break;
				case "2600":
					filter = Filter.F3;
					break;
				case "2100":
					filter = Filter.F4;
					break;
				case "1000":
					filter = Filter.F5;
					break;
				case "0500":
					filter = Filter.F6;
					break;
				case "0250":
					filter = Filter.F7;
					break;
				case "0100":
					filter = Filter.F8;
					break;
				case "0050":
					filter = Filter.F9;
					break;
				case "0025":
					filter = Filter.F10;
					break;
				case "VAR1":
					filter = Filter.VAR1;
					break;
				case "VAR2":
					filter = Filter.VAR2;
					break;
			}
			return filter;
		}

		// set variable filter 1 to indicate center and width 
		// 
		// if either center or width is zero, current value of center or width is 
		// contained 
		// fixme ... what should this thing do for am, fm, dsb ... ignore width? 
		private void SetFilterCenterAndWidth(int center, int width) 
		{ 
			int new_lo; 
			int new_hi; 

			if  ( center == 0 || width == 0 )  // need to get current values 
			{ 
				return; // not implemented yet 
			} 
			else 
			{ 
				// Debug.WriteLine("center: " + center  + " width: " + width); 
				new_lo = center - (width/2); 
				new_hi = center + (width/2); 
				if ( new_lo  < 0 ) new_lo = 0; 				
			} 						
			
			// new_lo and new_hi calculated assuming a USB mode .. do the right thing 
			// for lsb and other modes 
			// fixme -- needs more thinking 
			switch ( console.RX1DSPMode ) 
			{ 
				case DSPMode.LSB: 
					int scratch = new_hi; 
					new_hi = -new_lo; 
					new_lo = -scratch; 
					break; 

				case DSPMode.AM: 
				case DSPMode.SAM: 
					new_lo = -new_hi; 
					break; 
			} 						

			 
			// System.Console.WriteLine("zzsf: " + new_lo + " " + new_hi); 
			console.SelectRX1VarFilter();
			console.UpdateRX1Filters(new_lo, new_hi); 	

			return; 
		} 

		// Converts interger filter frequency into Kenwood SL/SH codes
		private string Frequency2Code(int f, string n)
		{
			f = Math.Abs(f);
			string code = "";
			switch(console.RX1DSPMode)
			{
				case DSPMode.CWL:
				case DSPMode.CWU:
				case DSPMode.LSB:
				case DSPMode.USB:
				switch(n)
				{
					case "SH":
						if(f >= 0 && f <= 1500)
							code = "00";
						else if(f > 1500 && f <= 1700)
							code = "01";
						else if(f > 1700 && f <= 1900)
							code = "02";
						else if(f > 1900 && f <= 2100)
							code = "03";
						else if(f > 2100 && f <= 2300)
							code = "04";
						else if(f > 2300 && f <= 2500)
							code = "05";
						else if(f > 2500 && f <= 2700)
							code = "06";
						else if(f > 2700 && f <= 2900)
							code = "07";
						else if(f > 2900 && f <= 3200)
							code = "08";
						else if(f > 3200 && f <= 3700)
							code = "09";
						else if(f > 3700 && f <= 4500)
							code = "10";
						else if(f > 4500)
							code = "11";
						break;
					case"SL":
						if(f >= 0 && f <= 25)
							code = "00";
						else if(f > 25 && f <= 75)
							code = "01";
						else if(f > 75 && f <= 150)
							code = "02";
						else if(f > 150 && f <= 250)
							code = "03";
						else if(f > 250 && f <= 350)
							code = "04";
						else if(f > 350 && f <= 450)
							code = "05";
						else if(f > 450 && f <= 550)
							code = "06";
						else if(f > 550 && f <= 650)
							code = "07";
						else if(f > 650 && f <= 750)
							code = "08";
						else if(f > 750 && f <= 850)
							code = "09";
						else if(f > 850 && f <= 950)
							code = "10";
						else if(f > 950)
							code = "11";
						break;
				}
				break;
				case DSPMode.AM:
				case DSPMode.DRM:
				case DSPMode.DSB:
				case DSPMode.FMN:
				case DSPMode.SAM:
				switch(n)
				{
					case "SH":
						if(f >= 0 && f <= 2750)
							code = "00";
						else if(f > 2750 && f <= 3500)
							code = "01";
						else if(f > 3500 && f <= 4500)
							code = "02";
						else if(f > 4500)
							code = "03";
						break;
					case "SL":
						if(f >= 0 && f <= 50)
							code = "00";
						else if(f > 50 && f <= 150)
							code = "01";
						else if(f > 150 && f <= 350)
							code = "02";
						else if(f > 350)
							code = "03";
						break;
				}
				break;
			}
			return code;
		}

		// Converts a frequency code pair to frequency in hz according to
		// the Kenwood TS-2000 spec.  Receives code and calling methd as parameters
		private int Code2Frequency(string c, string n)
		{
			int freq = 0;
			string mode = "SSB";
			int fgroup = 0;

			// Get the current console mode
			switch(console.RX1DSPMode)
			{
				case DSPMode.LSB:
				case DSPMode.USB:
					break;
				case DSPMode.AM:
				case DSPMode.DSB:
				case DSPMode.DRM:
				case DSPMode.FMN:
				case DSPMode.SAM:
					mode = "DSB";
					break;
			}
			// Get the frequency group(SSB/SL, SSB/SH, DSB/SL, and DSB/SH)
			switch(n)
			{
				case "SL":
					if(mode == "SSB")
						fgroup = 1;
					else
						fgroup = 3;
					break;
				case "SH":
					if(mode == "SSB")
						fgroup = 2;
					else
						fgroup = 4;
					break;
			}
			// return the frequency for the current DSP mode and calling method
			switch(fgroup)
			{
				case 1:		//SL SSB
				switch(c)
					{
					case "00":
						freq = 0;
						break;
					case "01":
						freq = 50;
						break;
					case "02":
						freq = 100;
						break;
					case "03":
						freq = 200;
						break;
					case "04":
						freq = 300;
						break;
					case "05":
						freq = 400;
						break;
					case "06":
						freq = 500;
						break;
					case "07":
						freq = 600;
						break;
					case "08":
						freq = 700;
						break;
					case "09":
						freq = 800;
						break;
					case "10":
						freq = 900;
						break;
					case "11":
						freq = 1000;
						break;
					}
				break;
				case 2:		//SH SSB
					switch(c)
					{
					case "00":
						freq = 1400;
						break;
					case "01":
						freq = 1600;
						break;
					case "02":
						freq = 1800;
						break;
					case "03":
						freq = 2000;
						break;
					case "04":
						freq = 2200;
						break;
					case "05":
						freq = 2400;
						break;
					case "06":
						freq = 2600;
						break;
					case "07":
						freq = 2800;
						break;
					case "08":
						freq = 3000;
						break;
					case "09":
						freq = 3400;
						break;
					case "10":
						freq = 4000;
						break;
					case "11":
						freq = 5000;
						break;
					}
				break;
				case 3:		//SL DSB
					switch(c)
					{
					case "00":
						freq = 0;
						break;
					case "01":
						freq = 100;
						break;
					case "02":
						freq = 200;
						break;
					case "03":
						freq = 500;
						break;
					}
				break;
				case 4:		//SH DSB
					switch(c)
					{
					case "00":
						freq = 2500;
						break;
					case "01":
						freq = 3000;
						break;
					case "02":
						freq = 4000;
						break;
					case "03":
						freq = 5000;
						break;
					}
				break;
			}
			return freq;
			#region old code

//			int freq = 0;
//			switch(console.RX1DSPMode)
//			{
//				case DSPMode.CWL:
//				case DSPMode.CWU:
//				case DSPMode.LSB:
//				case DSPMode.USB:
//				{
//					switch(c)	//c = filter code, n = SH or SL
//					{
//						case "00":
//							if(n == "SL")
//								freq = 10;
//							else
//								freq = 1400;
//							break;
//						case "01":
//							if(n == "SL")
//								freq = 50;
//							else
//								freq = 1600;
//							break;
//						case "02":
//							if(n == "SL")
//								freq = 100;
//							else
//								freq = 1800;
//							break;
//						case "03":
//							if(n == "SL")
//								freq = 200;
//							else
//								freq = 2000;
//							break;
//						case "04":
//							if(n == "SL")
//								freq = 300;
//							else
//								freq = 2200;
//							break;
//						case "05":
//							if(n == "SL")
//								freq = 400;
//							else
//								freq = 2400;
//							break;
//						case "06":
//							if(n == "SL")
//								freq = 500;
//							else
//								freq = 2600;
//							break;
//						case "07":
//							if(n == "SL")
//								freq = 600;
//							else
//								freq = 2800;
//							break;
//						case "08":
//							if(n == "SL")
//								freq = 700;
//							else
//								freq = 3000;
//							break;
//						case "09":
//							if(n == "SL")
//								freq = 800;
//							else
//								freq = 3400;
//							break;
//						case "10":
//							if(n == "SL")
//								freq = 900;
//							else
//								freq = 4000;
//							break;
//						case "11":
//							if(n == "SL")
//								freq = 1000;
//							else
//								freq = 5000;
//							break;
//						default:
//							break;
//					}
//					break;
//				}
//				case DSPMode.AM:
//				case DSPMode.DRM:
//				case DSPMode.DSB:
//				case DSPMode.FMN:
//				case DSPMode.SAM:
//				{
//					switch(c)
//					{
//						case "00":
//							if(n == "SL")
//								freq = 10;
//							else
//								freq = 2500;
//							break;
//						case "01":
//							if(n == "SL")
//								freq = 100;
//							else
//								freq = 3000;
//							break;
//						case "02":
//							if(n == "SL")
//								freq = 200;
//							else
//								freq = 4000;
//							break;
//						case "03":
//							if(n == "SL")
//								freq = 500;
//							else
//								freq = 5000;
//							break;
//					}
//				}
//				break;
//			}
//			return freq;
			#endregion old code
		}

		private void SetFilter(string c, string n)
		{
			// c = code, n = SH or SL
			console.RX1Filter = Filter.VAR1;
			int freq = 0;
			int offset = 0;
			string code;

			switch(console.RX1DSPMode)
			{
				case DSPMode.USB:
				case DSPMode.CWU:
					freq = Code2Frequency(c, n);
					if(n == "SH")
						console.RX1FilterHigh = freq;	//split the bandwidth at the center frequency
					else
						console.RX1FilterLow = freq;
					break;
				case DSPMode.LSB:
				case DSPMode.CWL:
					if(n == "SH")
					{
						freq = Code2Frequency(c, "SH");	// get the upper limit from the lower value set
						console.RX1FilterLow = -freq;	// since we need the more positive value
					}										// closest to the center freq in lsb modes
					else
					{
						freq = Code2Frequency(c, "SL");	// do the reverse here, the less positive value
						console.RX1FilterHigh = -freq; // is away from the center freq
					}
					break;
				case DSPMode.AM:
				case DSPMode.DRM:
				case DSPMode.DSB:
				case DSPMode.FMN:
				case DSPMode.SAM:
					if(n == "SH")
					{
						// Set the bandwith equally across the center freq
						freq = Code2Frequency(c, "SH");	
						console.RX1FilterHigh = freq/2;
						console.RX1FilterLow = -freq/2;
					}
					else
					{
						// reset the frequency to the nominal value (in case it's been changed)
						freq = console.RX1FilterHigh*2;	
						code = Frequency2Code(freq, "SH");
						freq = Code2Frequency(code, "SH");
						console.RX1FilterHigh = freq/2;
						console.RX1FilterLow = -freq/2;
						// subtract the SL value from the lower half of the bandwidth
						offset = Code2Frequency(c, "SL");
						console.RX1FilterLow = console.RX1FilterLow + offset;			
					}
					break;
			}
		}

		#endregion Filter Methods

		#region Mode Methods

		public void String2Mode(string pIndex)
		{
			string s = pIndex;

			switch(s)
				{
				case "00":								
					console.RX1DSPMode = DSPMode.LSB;
					break;
				case "01":
					console.RX1DSPMode = DSPMode.USB;
					break;
				case "02":
					console.RX1DSPMode = DSPMode.DSB;
					break;
				case "03":
					console.RX1DSPMode = DSPMode.CWL;
					break;
				case "04":
					console.RX1DSPMode = DSPMode.CWU;
					break;
				case "05":
					console.RX1DSPMode = DSPMode.FMN;
					break;
				case "06":
					console.RX1DSPMode = DSPMode.AM;
					break;
				case "07":
					console.RX1DSPMode = DSPMode.DIGU;
					break;
				case "08":
					console.RX1DSPMode = DSPMode.SPEC;
					break;
				case "09":
					console.RX1DSPMode = DSPMode.DIGL;
					break;
				case "10":
					console.RX1DSPMode = DSPMode.SAM;
					break;
				case "11":
					console.RX1DSPMode = DSPMode.DRM;
					break;
				}
		}

		public string Mode2String(DSPMode pMode)
		{
			DSPMode s = pMode;
			string retval = "";

			switch(s)
				{
					case DSPMode.LSB:
						retval = "00";  
						break;
					case DSPMode.USB:
						retval = "01";	
						break;
					case DSPMode.DSB:
						retval = "02";	
						break;
					case DSPMode.CWL:
						retval = "03";	
						break;
					case DSPMode.CWU:
						retval = "04";	
						break;
					case DSPMode.FMN:
						retval = "05";	
						break;
					case DSPMode.AM:
						retval = "06";	
						break;
					case DSPMode.DIGU:
						retval = "07";	
						break;
					case DSPMode.SPEC:
						retval = "08";	
						break;
					case DSPMode.DIGL:
						retval = "09";	
						break;
					case DSPMode.SAM:
						retval = "10";	
						break;
					case DSPMode.DRM:
						retval = "11";	
						break;
					default:
						retval = parser.Error1;
						break;
				}

			return retval;
		}

		// converts Kenwood single digit mode code to SDR mode
		public void KString2Mode(string pIndex)
		{
			string s = pIndex;

			switch(s)
			{
				case "1":
                    if (console.SetupForm.DigUIsUSB)
                        console.RX1DSPMode = DSPMode.DIGL;
                    else
                        console.RX1DSPMode = DSPMode.LSB;
					break;
				case "2":
                    if (console.SetupForm.DigUIsUSB)
                        console.RX1DSPMode = DSPMode.DIGU;
                    else
    					console.RX1DSPMode = DSPMode.USB;
					break;
				case "3":
					console.RX1DSPMode = DSPMode.CWU;
					break;
				case "4":
					console.RX1DSPMode = DSPMode.FMN;
					break;
				case "5":
					console.RX1DSPMode = DSPMode.AM;
					break;
				case "6":
					console.RX1DSPMode = DSPMode.DIGL;
					break;
				case "7":
					console.RX1DSPMode = DSPMode.CWL;
					break;
				case "9":
					console.RX1DSPMode = DSPMode.DIGU;
					break;
				default:
					console.RX1DSPMode = DSPMode.USB;
					break;
			}
		}

		// converts SDR mode to Kenwood single digit mode code
		public string Mode2KString(DSPMode pMode)
		{
			DSPMode s = pMode;
			string retval = "";

			switch(s)
			{
				case DSPMode.LSB:
   					retval = "1";  
					break;
				case DSPMode.USB:
   					retval = "2";	
					break;
				case DSPMode.CWU:
					retval = "3";	
					break;
				case DSPMode.FMN:
					retval = "4";	
					break;
				case DSPMode.AM:
//				case DSPMode.SAM:		//possible fix for SAM problem
					retval = "5";	
					break;
				case DSPMode.DIGL:
					if(console.SetupForm.DigUIsUSB)
						retval = "1";
					else
						retval = "6";	
					break;
				case DSPMode.CWL:
					retval = "7";	
					break;
				case DSPMode.DIGU:
					if(console.SetupForm.DigUIsUSB)
						retval = "2";
					else
						retval = "9";
					break;
				default:
					retval = parser.Error1;
					break;
			}

			return retval;
		}

		#endregion Mode Methods

		#region Band Methods

		private void MakeBandList()
		//Construct an array of the PowerSDR.Band enums.
		//If the 2m xverter is present, set the last index to B2M
		//otherwise, set it to B6M.
		{
			int ndx = 0;
			BandList = new Band[(int)Band.LAST+2];
			foreach(Band b in Enum.GetValues(typeof(Band)))
			{
				BandList.SetValue(b, ndx);
				ndx++;
			}
			if(console.XVTRPresent)
				LastBandIndex = Array.IndexOf(BandList,Band.B2M);
			else
				LastBandIndex = Array.IndexOf(BandList,Band.B6M);
		}

		private void SetBandGroup(int band)
		{
			int oldval = parser.nSet;
			parser.nSet = 1;
			if(band == 0)
				ZZBG("0");
			else
				ZZBG("1");

			parser.nSet = oldval;
		}

		private string GetBand(string b)
		{
			if(b.Length == parser.nSet)
			{
				if(b.StartsWith("V") || b.StartsWith("v"))
					SetBandGroup(1);
				else 
					SetBandGroup(0);
			}

			if(b.Length == parser.nSet)
			{
				console.SetCATBand(String2Band(b));
				return "";
			}
			else if(b.Length == parser.nGet)
			{
				return Band2String(console.RX1Band);
			}
			else
			{
				return parser.Error1;
			}


		}

		private void BandUp()
		{
			Band nextband;
			Band current = console.RX1Band;
			int currndx = Array.IndexOf(BandList,current);
			if(currndx == LastBandIndex)
				nextband = BandList[0];
			else
				nextband = BandList[currndx+1];
			console.SetCATBand(nextband);
		}

		private void BandDown()
		{
			Band nextband;
			Band current = console.RX1Band;
			int currndx = Array.IndexOf(BandList,current);
			if(currndx > 0)
				nextband = BandList[currndx-1];
			else
				nextband = BandList[LastBandIndex];
			console.SetCATBand(nextband);

		}

		private string Band2String(Band pBand)
		{
			Band band = pBand;
			string retval;

			switch(band)
			{
				case Band.GEN:
					retval = "888";
					break;
				case Band.B160M:
					retval = "160";
					break;
				case Band.B60M:
					retval = "060";
					break;
				case Band.B80M:
					retval = "080";
					break;
				case Band.B40M:
					retval = "040";
					break;
				case Band.B30M:
					retval = "030";
					break;
				case Band.B20M:
					retval = "020";
					break;
				case Band.B17M:
					retval = "017";
					break;
				case Band.B15M:
					retval = "015";
					break;
				case Band.B12M:
					retval = "012";
					break;
				case Band.B10M:
					retval = "010";
					break;
				case Band.B6M:
					retval = "006";
					break;
				case Band.B2M:
					retval = "002";
					break;
				case Band.WWV:
					retval = "999";
					break;
				case Band.VHF0:
					retval = "V00";
					break;
				case Band.VHF1:
					retval = "V01";
					break;
				case Band.VHF2:
					retval = "V02";
					break;
				case Band.VHF3:
					retval = "V03";
					break;
				case Band.VHF4:
					retval = "V04";
					break;
				case Band.VHF5:
					retval = "V05";
					break;
				case Band.VHF6:
					retval = "V06";
					break;
				case Band.VHF7:
					retval = "V07";
					break;
				case Band.VHF8:
					retval = "V08";
					break;
				case Band.VHF9:
					retval = "V09";
					break;
				case Band.VHF10:
					retval = "V10";
					break;
				case Band.VHF11:
					retval = "V11";
					break;
				case Band.VHF12:
					retval = "V12";
					break;
				case Band.VHF13:
					retval = "V13";
					break;
				default:
					retval = "888";
					break;
			}
			return retval;
		}

		private Band String2Band(string pBand)
		{
			string band = pBand.ToUpper();;
			Band retval;

			switch(band)
			{
				case "888":
					retval = Band.GEN;
					break;
				case "160":
					retval = Band.B160M;
					break;
				case "060":
					retval = Band.B60M;
					break;
				case "080":
					retval = Band.B80M;
					break;
				case "040":
					retval = Band.B40M;
					break;
				case "030":
					retval = Band.B30M;
					break;
				case "020":
					retval = Band.B20M;
					break;
				case "017":
					retval = Band.B17M;
					break;
				case "015":
					retval = Band.B15M;
					break;
				case "012":
					retval = Band.B12M;
					break;
				case "010":
					retval = Band.B10M;
					break;
				case "006":
					retval = Band.B6M;
					break;
				case "002":
					retval = Band.B2M;
					break;
				case "999":
					retval = Band.WWV;
					break;
				case "V00":
					retval = Band.VHF0;
					break;
				case "V01":
					retval = Band.VHF1;
					break;
				case "V02":
					retval = Band.VHF2;
					break;
				case "V03":
					retval = Band.VHF3;
					break;
				case "V04":
					retval = Band.VHF4;
					break;
				case "V05":
					retval = Band.VHF5;
					break;
				case "V06":
					retval = Band.VHF6;
					break;
				case "V07":
					retval = Band.VHF7;
					break;
				case "V08":
					retval = Band.VHF8;
					break;
				case "V09":
					retval = Band.VHF9;
					break;
				case "V10":
					retval = Band.VHF10;
					break;
				case "V11":
					retval = Band.VHF11;
					break;
				case "V12":
					retval = Band.VHF12;
					break;
				case "V13":
					retval = Band.VHF13;
					break;
				default:
					retval = Band.GEN;
					break;
			}
			return retval;
		}

		#endregion Band Methods

		#region Step Methods

		private string Step2String(int pSize)
		{
			// Modified 2/25/07 to accomodate changes to console where odd step sizes added.  BT
			string stepval = "";
			int step = pSize;
			switch(step)
			{
				case 0:
					stepval = "0000";	//10e0 = 1 hz
					break;
				case 1:
					stepval = "0001";	//10e1 = 10 hz
					break;
				case 2:
					stepval = "1000";	//special default for 50 hz
					break;
				case 3:
					stepval = "0010";	//10e2 = 100 hz
					break;
				case 4:
					stepval = "1001";	//special default for 250 hz
					break;
				case 5:
					stepval = "1010";	//10e3 = 1 kHz default for 500 hz
					break;
				case 6:
					stepval = "0011";	//10e3 = 1 kHz
					break;
				case 7:
					stepval = "1011";	//special default for 5 kHz
					break;
				case 8:
					stepval = "1100";	//special default for 9 kHz
					break;
				case 9:
					stepval = "0100";	//10e4 = 10 khZ
					break;
				case 10:
					stepval = "0101";	//10e5 = 100 kHz
					break;
				case 11:
					stepval = "0110";	//10e6 = 1 mHz
					break;
				case 12:
					stepval = "0111";	//10e7 = 10 mHz
					break;
			}
			return stepval;
		}

		#endregion Step Methods

		#region Meter Methods

		private void String2RXMeter(int m)
		{
			console.CurrentMeterRXMode = (MeterRXMode) m;
		}

		private string RXMeter2String()
		{
			return ((int) console.CurrentMeterRXMode).ToString();
		}

		private void String2TXMeter(int m)
		{
			console.CurrentMeterTXMode = (MeterTXMode) m;
		}

		private string TXMeter2String()
		{
			return ((int) console.CurrentMeterTXMode).ToString();
		}

		#endregion Meter Methods

		#region Rig ID Methods

		private string CAT2RigType()
		{
			return "";
		}

		private string RigType2CAT()
		{
			return "";
		}

		#endregion Rig ID Methods

		#region DSP Filter Size Methods

		private string Width2Index(int txt)
		{
			string ans = "";
			switch(txt)
			{
				case 256:
					ans = "0";
					break;
				case 512:
					ans = "1";
					break;
				case 1024:
					ans = "2";
					break;
				case 2048:
					ans = "3";
					break;
				case 4096:
					ans = "4";
					break;
				default:
					ans = "0";
					break;
			}
			return ans;
		}

		private int Index2Width(string ndx)
		{
			int ans;
			switch(ndx)
			{
				case "0":
					ans = 256;
					break;
				case "1":
					ans = 512;
					break;
				case "2":
					ans = 1024;
					break;
				case "3":
					ans = 2048;
					break;
				case "4":
					ans = 4096;
					break;
				default:
					ans = 256;
					break;
			}
			return ans;
		}

		#endregion DSP Filter Size Methods

		#endregion Helper methods
	}	
}

