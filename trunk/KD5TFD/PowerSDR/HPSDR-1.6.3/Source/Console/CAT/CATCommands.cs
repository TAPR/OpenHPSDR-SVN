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
				return AddLeadingZeros(console.AF);		// Get the console setting
			}
			else
			{
				return parser.Error1;	// return a ?
			}
		}

		// Moves one band down from the currently selected band
		// write only
		public string BD()
		{
			BandDown();
			return "";
		}

		// Moves one band up from the currently selected band
		// write only
		public string BU()
		{
			BandUp();
			return "";
		}

		// Sets or reads the frequency of VFO A
		public string FA(string s)
		{
			if(s.Length == parser.nSet)
			{
				s = s.Insert(5,separator);		//reinsert the global numeric separator
				console.VFOAFreq = double.Parse(s);
				return "";
			}
			else if(s.Length == parser.nGet)
				return StrVFOFreq("A");
			else
				return parser.Error1;
		}

		// Sets or reads the frequency of VFO B
		public string FB(string s)
		{
			if(s.Length == parser.nSet)
			{
				s = s.Insert(5,separator);
				console.VFOBFreq = double.Parse(s);
				return "";
			}
			else if(s.Length == parser.nGet)
				return StrVFOFreq("B");
			else
				return parser.Error1;
		}

		// Sets VFO A to control rx
		// this is a dummy command to keep other software happy
		// since the SDR-1000 always uses VFO A for rx
		public string FR(string s)
		{
			if(s.Length == parser.nSet)
				return "0";
			else if(s.Length == parser.nGet)
				return "0";
			else
				return parser.Error1;
		}

		// Sets or reads VFO B to control tx
		// another "happiness" command
		public string FT(string s)
		{
			if(s.Length == parser.nSet && (s == "0" || s == "1"))
				return ZZSP(s);
			else if(s.Length == parser.nGet)
				return ZZSP(s);
			else
				return parser.Error1;
		}

		// Sets or reads the DSP filter width
		public string FW(string s)
		{
			if(s.Length == parser.nSet)
			{
				console.CurrentFilter = String2Filter(s);
				return "";
			}
			else if(s.Length == parser.nGet)
				return Filter2String(console.CurrentFilter);
			else
				return parser.Error1;
		}

		#endregion Standard CAT Methods A-F

		#region Standard CAT Methods G-M

		// Sets or reads the AGC constant
		// this is a wrapper that calls ZZGT
		public string GT(string s)
		{
			return ZZGT(s);
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
			// Initalize the command word
			string cmd_string = "";
			string temp;

			// Get VFOA's frequency (P1 - 11 bytes)
			cmd_string += StrVFOFreq("A");

			// Get the step size index (P2 - 4 bytes)
			cmd_string += ZZST();

			// Determine which incremental tuning control is active
			// and get the value for the active control 
			string rit = "0";
			string xit = "0";
			int ITValue = 0;

			if(console.RITOn)
				rit = "1";
			else if(console.XITOn)
				xit = "1";

			if(rit == "1")
				ITValue = console.RITValue;
			else if(xit == "1")
				ITValue = console.XITValue;

			// Add the ITValue to the command string (P3 - 6 bytes
			if(ITValue < 0)
				cmd_string += "-"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');
			else
				cmd_string += "+"+Convert.ToString(Math.Abs(ITValue)).PadLeft(5,'0');

			// Add the RIT/XIT status bits (P4 and P5, one byte each)
			cmd_string += rit+xit;
				
			// Skip the memory channel stuff, the SDR1K doesn't use banks and channels per se
			// (P6 - 1 byte, P7 - 2 bytes)
			cmd_string += "000";

			// Set the current MOX state (P8 - 1 byte)(what the heck is this for?)
			if(console.MOX)
				cmd_string += "1";
			else
				cmd_string += "0";

			// Get the SDR mode.  (P9 - 1 byte)
			temp = Mode2KString(console.CurrentDSPMode);
			if(temp.Length == 1)	// if the answer is not an error message ?;
				cmd_string += temp;
			else
				cmd_string += " ";	// return a blank if it's an error

			// Set the FR/FT commands which determines the transmit and receive
			// VFO's. VFO A = 0, VFO B = 1. (P10 - 1 byte)
			//			if(console.VFOSplit)
			//				cmd_string += "1";
			//			else
			cmd_string += "0";

			// Set the Scan code to 0 
			// The Scan code might be implemented but the frequency range would
			// have to be manually entered. (P11 - 1 byte)
			cmd_string += "0";

			// Set the Split operation code (P12 - 1 byte)
			cmd_string += ZZSP("");

			// Set the remaining CTCSS tone and shift bits to 0 (P13 - P15, 4 bytes)
			cmd_string += "0000";

			return cmd_string;
		}

		// Sets or reads the transceiver mode
		public string MD(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(Convert.ToInt32(s) > 0 && Convert.ToInt32(s) <= 7)
				{
					KString2Mode(s);
					return "";
				}
				else
					return parser.Error1;
			}
			else if(s.Length == parser.nGet)
			{

				return Mode2KString(console.CurrentDSPMode);

			}
			else
				return parser.Error1;
		}

		// Sets or reads the Mic Gain thumbwheel
		public string MG(string s)
		{
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

		// Sets or reads the Monitor status
		public string MO(string s)
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

		#endregion Standard CAT Methods G-M

		#region Standard CAT Methods N-Q

		// Sets or reads the Noise Blanker 1 status
		public string NB(string s)
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

		// Sets or reads the Automatic Notch Filter status
		public string NT(string s)
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

		// Sets or reads the PA output thumbwheel
		public string PC(string s)
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

		// Sets or reads the Speech Compressor status
		public string PR(string s)
		{
			if(s.Length == parser.nSet)
			{
				if(s == "0")
					console.COMP = false;
				else if(s == "1")
					console.COMP = true;
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				bool comp = console.COMP;
				if(comp)
					return "1";
				else
					return "0";
			}
			else
			{
				return "";
			}
		}

		// Sets or reads the console power on/off status
		public string PS(string s)
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

		// Sets the Quick Memory with the current contents of VFO A
		public string QI()
		{
			console.CATMemoryQS();
			return "";
		}

		#endregion Standard CAT Methods N-Q

		#region Standard CAT Methods R-Z

		// Clears the RIT value
		// write only
		public string RC()
		{
			console.RITValue = 0;
			return "";
		}

		// Sets or reads the RIT status (on/off)
		public string RT(string s)
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

		// Sets or reads the transceiver receive mode status
		// write only but spec shows an answer parameter for a read???
		public string RX(string s)
		{
			console.MOX = false;
			return "";
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
				switch(console.CurrentDSPMode)
				{
					case DSPMode.AM:
					case DSPMode.CWU:
					case DSPMode.DRM:
					case DSPMode.DSB:
					case DSPMode.FMN:
					case DSPMode.SAM:
					case DSPMode.USB:
						return Frequency2Code(console.FilterHighValue,"SH");
					case DSPMode.CWL:
					case DSPMode.LSB:
						return Frequency2Code(console.FilterLowValue,"SH");
					default:
						return Frequency2Code(console.FilterHighValue,"SH");
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
				switch(console.CurrentDSPMode)
				{
					case DSPMode.AM:
					case DSPMode.CWU:
					case DSPMode.DRM:
					case DSPMode.DSB:
					case DSPMode.FMN:
					case DSPMode.SAM:
					case DSPMode.USB:
						return Frequency2Code(console.FilterLowValue,"SL");
					case DSPMode.CWL:
					case DSPMode.LSB:
						return Frequency2Code(console.FilterHighValue,"SL");
					default:
						return Frequency2Code(console.FilterLowValue,"SL");
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
					num = DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
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
			console.MOX = true;
			return "";
		}

		// Sets or reads the transceiver XIT status (on/off)
		public string XT(string s)
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


		//Sets or reads the current band setting
		public string ZZBS(string s)
		{
			return GetBand(s);
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
				if(n > (int) DisplayMode.FIRST && n < (int) DisplayMode.LAST)
					Display.CurrentDisplayMode = (DisplayMode) n;
				else
					return parser.Error1;

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

		//Sets or reads the current filter index number
		public string ZZFI(string s)
		{
			int n = 0;

			if(s != "")
				n = Convert.ToInt32(s);

			if(s.Length == parser.nSet)
			{
				if(n < (int) Filter.LAST)
					console.CurrentFilter = (Filter) n;
				else
					return parser.Error1;

				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros((int) console.CurrentFilter);
			}
			else
			{
				return parser.Error1;
			}
		}

		#endregion Extended CAT Methods ZZA-ZZF

		#region Extended CAT Methods ZZG-ZZM

		// Sets or reads the AGC constant
		public string ZZGT(string s)
		{
			if(s.Length == parser.nSet)
			{
				if((Convert.ToInt32(s) > (int) AGCMode.FIRST && Convert.ToInt32(s) < (int) AGCMode.LAST))
					console.CurrentAGCMode = (AGCMode) Convert.ToInt32(s);
				else
					return parser.Error1;

				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return ((int) console.CurrentAGCMode).ToString();
			}
			else
			{
				return parser.Error1;
			}
		}

		// Sets the CAT Rig Type to SDR-1000
		public string ZZID()
		{
			console.SetupForm.CATSetRig("SDR-1000");
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

			switch(s)
			{
				case "0":
					rtn += StrVFOFreq("A");						// VFO A frequency			11 bytes
					rtn += stepsize;							// Console step frequency	 4 bytes
					rtn += incr;								// incremental tuning value	 6 bytes
					rtn += rit;									// RIT status				 1 byte
					rtn += xit;									// XIT status				 1 byte
					rtn += "000";								// dummy for memory bank	 3 bytes
					rtn += tx;									// tx-rx status				 1 byte
					rtn += Mode2String(console.CurrentDSPMode);	// current mode				 2 bytes
					rtn += "0";									// dummy for FR/FT			 1 byte
					rtn += "0";									// dummy for scan status	 1 byte
					rtn += split;								// VFO Split status			 1 byte
					rtn += "0000";								// dummy for the balance	 4 bytes
					break;										// total bytes				36
				default:
					rtn = parser.Error1;
					break;

			}
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
			console.CATFilterShiftReset = 1;
			return "";
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
				return Mode2String(console.CurrentDSPMode);
			}
			else
			{
				return parser.Error1;
			}
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
				return TXMeter2String();
			}
			else
			{
				return parser.Error1;
			}
		}


		// Sets or reads the Mic Preamp Gain
		//		public string ZZMG(string s)
		//		{
		//			int n = 0;
		//			int x = 0;
		//			string sign;
		//
		//			if(s != "")
		//			{
		//				n = Convert.ToInt32(s);
		//				n = Math.Max(-30, n);
		//				n = Math.Min(70, n);
		//			}
		//
		//			if(s.Length == parser.nSet)
		//			{
		//				console.SetupForm.CATTXPreGain = n;
		//				return "";
		//			}
		//			else if(s.Length == parser.nGet)
		//			{
		//				x = console.SetupForm.CATTXPreGain;
		//				if(x >= 0)
		//					sign = "+";
		//				else
		//					sign = "-";
		//				// we have to remove the leading zero and replace it with the sign.
		//				return sign+AddLeadingZeros(Math.Abs(x)).Substring(1);
		//			}
		//			else
		//			{
		//				return parser.Error1;
		//			}
		//	}

		#endregion Extended CAT Methods ZZG-ZZM

		#region Extended CAT Methods ZZN-ZZQ

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

		// Sets or reads the Preamp thumbwheel
		public string ZZPA(string s)
		{
			if(s.Length == parser.nSet)
			{
				if((Convert.ToInt32(s) > (int) PreampMode.FIRST && Convert.ToInt32(s) < (int) PreampMode.LAST))
				{
					console.CATPreamp = Convert.ToInt32(s);
					return "";
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


		// Sets or reads the Speech Compressor threshold
		public string ZZPL(string s)
		{
			if(s.Length == parser.nSet)
			{
				console.SetupForm.CATCompThreshold = Convert.ToInt32(s);
				return "";
			}
			else if(s.Length == parser.nGet)
			{
				return AddLeadingZeros(console.SetupForm.CATCompThreshold);
			}
			else
			{
				return parser.Error1;
			}

		}


		#endregion Extended CAT Methods ZZN-ZZQ

		#region Extended CAT Methods ZZR-ZZZ

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

		// Reads the Console Multimeter
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


		// ZZSFccccwwww  Set Filter, cccc=center freq www=width both in hz 
		public string ZZSF(string s)
		{
			int center = Convert.ToInt32(s.Substring(0,4), 10); 
			int width = Convert.ToInt32(s.Substring(4), 10); 
			SetFilterCenterAndWidth(center, width); 
			return "";
		}

		// Reads the S Meter value
		public string ZZSM(string s)
		{
			int sm = 0;

			if(s == "0")	// read the main transceiver s meter
			{
				float num = 0f;
				if(console.PowerOn)
					num = DttSP.CalculateMeter(DttSP.MeterType.SIGNAL_STRENGTH);
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

		// Reads the current console step size (read-only property)
		public string ZZST()
		{
			int step = console.StepSize;
			return Step2String(step);
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
				console.TXFilterHigh = th;		
				return "";
			}
			else if(s.Length == parser.nGet)	// if this is a read command
			{
				return AddLeadingZeros(console.TXFilterHigh);
			}
			else
			{
				return parser.Error1;	// return a ?
			}
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
				console.TXFilterLow = tl;		
				return "";
			}
			else if(s.Length == parser.nGet)	// if this is a read command
			{
				return AddLeadingZeros(console.TXFilterLow);
			}
			else
			{
				return parser.Error1;	// return a ?
			}
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
			if(s.Length == parser.nSet & Convert.ToInt32(s) <= 2)
			{
				console.CATVFOSwap(s);
				return "";
			}
			else
			{
				return parser.Error1;
			}

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



		#region VFO Methods
		// Converts a vfo frequency to a proper CAT frequency string
		private string StrVFOFreq(string vfo)
		{
			double freq = 0;
			string cmd_string = "";

			if(vfo == "A")
				freq = console.VFOAFreq;
			else if(vfo == "B")
				freq = console.VFOBFreq;
			else if(vfo == "C")
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
			else
			{
				cmd_string += "0"+freq.ToString();
			}
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
					retval = Math.Abs(console.FilterHighValue-console.FilterLowValue);
					strfilt = AddLeadingZeros(retval);
					break;
				case "VAR2":
					retval = Math.Abs(console.FilterHighValue-console.FilterLowValue);
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
			switch ( console.CurrentDSPMode ) 
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
			console.SelectVarFilter();
			console.UpdateFilters(new_lo, new_hi); 	

			return; 
		} 

		// Converts interger filter frequency into Kenwood SL/SH codes
		private string Frequency2Code(int f, string n)
		{
			f = Math.Abs(f);
			string code = "";
			switch(console.CurrentDSPMode)
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
			switch(console.CurrentDSPMode)
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
//			switch(console.CurrentDSPMode)
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
			console.CurrentFilter = Filter.VAR1;
			int freq = 0;
			int offset = 0;
			string code;

			switch(console.CurrentDSPMode)
			{
				case DSPMode.USB:
				case DSPMode.CWU:
					freq = Code2Frequency(c, n);
					if(n == "SH")
						console.FilterHighValue = freq;	//split the bandwidth at the center frequency
					else
						console.FilterLowValue = freq;
					break;
				case DSPMode.LSB:
				case DSPMode.CWL:
					if(n == "SH")
					{
						freq = Code2Frequency(c, "SH");	// get the upper limit from the lower value set
						console.FilterLowValue = -freq;	// since we need the more positive value
					}										// closest to the center freq in lsb modes
					else
					{
						freq = Code2Frequency(c, "SL");	// do the reverse here, the less positive value
						console.FilterHighValue = -freq; // is away from the center freq
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
						console.FilterHighValue = freq/2;
						console.FilterLowValue = -freq/2;
					}
					else
					{
						// reset the frequency to the nominal value (in case it's been changed)
						freq = console.FilterHighValue*2;	
						code = Frequency2Code(freq, "SH");
						freq = Code2Frequency(code, "SH");
						console.FilterHighValue = freq/2;
						console.FilterLowValue = -freq/2;
						// subtract the SL value from the lower half of the bandwidth
						offset = Code2Frequency(c, "SL");
						console.FilterLowValue = console.FilterLowValue + offset;			
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
					console.CurrentDSPMode = DSPMode.LSB;
					break;
				case "01":
					console.CurrentDSPMode = DSPMode.USB;
					break;
				case "02":
					console.CurrentDSPMode = DSPMode.DSB;
					break;
				case "03":
					console.CurrentDSPMode = DSPMode.CWL;
					break;
				case "04":
					console.CurrentDSPMode = DSPMode.CWU;
					break;
				case "05":
					console.CurrentDSPMode = DSPMode.FMN;
					break;
				case "06":
					console.CurrentDSPMode = DSPMode.AM;
					break;
				case "07":
					console.CurrentDSPMode = DSPMode.DIGU;
					break;
				case "08":
					console.CurrentDSPMode = DSPMode.SPEC;
					break;
				case "09":
					console.CurrentDSPMode = DSPMode.DIGL;
					break;
				case "10":
					console.CurrentDSPMode = DSPMode.SAM;
					break;
				case "11":
					console.CurrentDSPMode = DSPMode.DRM;
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
					console.CurrentDSPMode = DSPMode.LSB;
					break;
				case "2":
					console.CurrentDSPMode = DSPMode.USB;
					break;
				case "3":
					console.CurrentDSPMode = DSPMode.CWU;
					break;
				case "4":
					console.CurrentDSPMode = DSPMode.FMN;
					break;
				case "5":
					console.CurrentDSPMode = DSPMode.AM;
					break;
				case "6":
					console.CurrentDSPMode = DSPMode.DIGL;
					break;
				case "7":
					console.CurrentDSPMode = DSPMode.CWL;
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
				case DSPMode.DIGU:
					retval = "2";	
					break;
				case DSPMode.CWU:
					retval = "3";	
					break;
				case DSPMode.FMN:
					retval = "4";	
					break;
				case DSPMode.AM:
					retval = "5";	
					break;
				case DSPMode.DIGL:
					retval = "6";	
					break;
				case DSPMode.CWL:
					retval = "7";	
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


		private string GetBand(string b)
		{
			if(b.Length == parser.nSet)
			{
				console.SetCATBand(String2Band(b));
				return "";
			}
			else if(b.Length == parser.nGet)
			{
				return Band2String(console.CurrentBand);
			}
			else
			{
				return parser.Error1;
			}


		}

		private void BandUp()
		{
			Band nextband;
			Band current = console.CurrentBand;
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
			Band current = console.CurrentBand;
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
					stepval = "0010";	//10e2 = 100 hz
					break;
				case 3:
					stepval = "0011";	//10e3 = 1 kHz
					break;
				case 4:
					stepval = "0100";	//10e4 = 10 kHz
					break;
				case 5:
					stepval = "0101";	//10e5 = 100 kHz
					break;
				case 6:
					stepval = "0110";	//10e6 = 1 mHz
					break;
				case 7:
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

		#endregion Helper methods
	}	
}

