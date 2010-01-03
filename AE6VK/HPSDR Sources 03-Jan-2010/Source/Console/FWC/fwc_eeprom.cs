//=================================================================
// fwc_eeprom.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2008  FlexRadio Systems
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
// You may contact us via email at: sales@flex-radio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    8900 Marybank Dr.
//    Austin, TX 78750
//    USA
//=================================================================

using System;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
	public class FWCEEPROM
	{
		#region Misc Routines

		public static void Init()
		{
			FWC.GetSerialNum(out serial_number); // get serial number
			Thread.Sleep(40);
			FWC.GetTRXOK(out trx_ok);
			Thread.Sleep(40);
			FWC.GetTRXSN(out trx_serial);
			Thread.Sleep(40);
			FWC.GetPAOK(out pa_ok);
			Thread.Sleep(40);
			FWC.GetPASN(out pa_serial);
			Thread.Sleep(40);
			FWC.GetPARev(out pa_rev);
			Thread.Sleep(40);
			FWC.GetRFIOOK(out rfio_ok);
			Thread.Sleep(40);
			FWC.GetRFIOSN(out rfio_serial);
			Thread.Sleep(40);
			FWC.GetATUOK(out atu_ok);
			Thread.Sleep(40);
			FWC.GetRX2OK(out rx2_ok);
			Thread.Sleep(40);

			uint data; // get last cal date/time
			FWC.ReadTRXEEPROMUint(0x40, out data);
			Thread.Sleep(40);
			last_cal_date_time = (long)data<<32;
			FWC.ReadTRXEEPROMUint(0x44, out data);
			Thread.Sleep(40);
			last_cal_date_time += data;
		}

		public static string SerialToString(uint serial)
		{
			string s = "";
			s += ((byte)(serial)).ToString("00");
			s += ((byte)(serial>>8)).ToString("00")+"-";
			s += ((ushort)(serial>>16)).ToString("0000");
			return s;
		}

		private static void WriteCalDateTime()
		{
			long l = DateTime.Now.ToFileTimeUtc();
			FWC.WriteTRXEEPROMUint(0x40, (uint)(l>>32));
			FWC.WriteTRXEEPROMUint(0x44, (uint)l);
			last_cal_date_time = l;
		}

		public static bool CheckAll()
		{
			bool b = true;
			b = CheckRXLevel(); if(!b) return b;
			b = CheckRXImage(); if(!b) return b;
			b = CheckTXImage(); if(!b) return b;
			b = CheckTXCarrier(); if(!b) return b;
			b = CheckPABias(); if(!b) return b;
			b = CheckPABridge(); if(!b) return b;
			b = CheckPAPower(); if(!b) return b;
			b = CheckPASWR(); if(!b) return b;
			return b;
		}

		#endregion

		#region Properties

		private static long last_cal_date_time = 0;
		public static long LastCalDateTime
		{
			get { return last_cal_date_time; }
			set { last_cal_date_time = value; }
		}

		private static uint serial_number;
		public static uint SerialNumber
		{
			get { return serial_number; }
			//set { serial_number = value; }
		}

		private static bool trx_ok;
		public static bool TRXOK
		{
			get { return trx_ok; }
		}

		private static uint trx_serial;
		public static uint TRXSerial
		{
			get { return trx_serial; }
			//set { trx_serial = value; }
		}

		private static bool pa_ok;
		public static bool PAOK
		{
			get { return pa_ok; }
		}

		private static uint pa_serial;
		public static uint PASerial
		{
			get { return pa_serial; }
			//set { pa_serial = value; }
		}

		private static uint pa_rev;
		public static uint PARev
		{
			get { return pa_rev; }
			//set { pa_rev = value; }
		}

		private static bool rfio_ok;
		public static bool RFIOOK
		{
			get { return rfio_ok; }
		}

		private static uint rfio_serial;
		public static uint RFIOSerial
		{
			get { return rfio_serial; }
		}

		private static bool atu_ok;
		public static bool ATUOK
		{
			get { return atu_ok; }
		}

		private static bool rx2_ok;
		public static bool RX2OK
		{
			get { return rx2_ok; }
		}

		#endregion

		#region RX Level

		public static bool CheckRXLevel()
		{
			Random rand = new Random();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			float[][] rx_level_table = new float[(int)Band.LAST][];
			for(int i=0; i<(int)Band.LAST; i++)
				rx_level_table[i] = new float[3];

			for(int i=0; i<bands.Length; i++)
				for(int j=0; j<3; j++)
					rx_level_table[(int)bands[i]][j] = (float)rand.NextDouble();

			WriteRXLevel(rx_level_table);

			float[][] rx_level_check = new float[(int)Band.LAST][];
			for(int i=0; i<(int)Band.LAST; i++)
				rx_level_check[i] = new float[3];
			
			ReadRXLevel(rx_level_check);

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<3; j++)
				{
					if(rx_level_table[(int)bands[i]][j] != rx_level_check[(int)bands[i]][j])
						return false;
				}
			}
			return true;
		}

		public static void WriteRXLevel(float[][] table)
		{
			WriteCalDateTime();			
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort offset;
			//FWC.ReadTRXEEPROMUshort(0x48, out offset);
			//if(offset == 0 || offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x48, 0x0100);
				Thread.Sleep(10);
				offset = 0x0100;
			}

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<3; j++)
				{
					int error_count = 0;
					float test = -99.99f;
					do
					{
						FWC.WriteTRXEEPROMFloat((uint)(offset+i*12+j*4), table[(int)bands[i]][j]);
						Thread.Sleep(10);
						
						FWC.ReadTRXEEPROMFloat((uint)(offset+i*12+j*4), out test);
						Thread.Sleep(40);
                        
						if(test != table[(int)bands[i]][j])
						{
							if(error_count++ > 5)
							{
								MessageBox.Show("Error writing RX Level value to EEPROM.\n"+
									"Tried to write "+table[(int)bands[i]][j].ToString("f4")+", but read back "+test.ToString("f4"));
								return;
							}
						}
					} while(test != table[(int)bands[i]][j]);
				}
			}
		}

		public static void ReadRXLevel(float[][] table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort offset;
			FWC.ReadTRXEEPROMUshort(0x48, out offset);
			Thread.Sleep(40);
			if(offset == 0 || offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(offset, out data);
			Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<3; j++)
				{
					float f;
					FWC.ReadTRXEEPROMFloat((uint)(offset+i*12+j*4), out f);
					Thread.Sleep(40);
					table[(int)bands[i]][j] = f;
				}
			}
		}

		#endregion

		#region RX Image

		public static bool CheckRXImage()
		{
			Random rand = new Random();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			float[] gain_table = new float[(int)Band.LAST];
			float[] phase_table = new float[(int)Band.LAST];

			for(int i=0; i<bands.Length; i++)
			{
				gain_table[(int)bands[i]] = (float)rand.NextDouble();
				phase_table[(int)bands[i]] = (float)rand.NextDouble();
			}

			WriteRXImage(gain_table, phase_table);

			float[] gain_check = new float[(int)Band.LAST];
			float[] phase_check = new float[(int)Band.LAST];

            ReadRXImage(gain_check, phase_check);

			for(int i=0; i<bands.Length; i++)
			{
				if(gain_table[(int)bands[i]] != gain_check[(int)bands[i]] ||
					phase_table[(int)bands[i]] != phase_check[(int)bands[i]])
					return false;
			}
			return true;
		}

		public static void WriteRXImage(float[] gain_table, float[] phase_table)
		{
			WriteCalDateTime();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			
			ushort gain_offset;
			//FWC.ReadTRXEEPROMUshort(0x4A, out gain_offset);
			//if(gain_offset == 0 || gain_offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x4A, 0x0190);
				Thread.Sleep(10);
				gain_offset = 0x0190;
			}

			ushort phase_offset;
			//FWC.ReadTRXEEPROMUshort(0x4C, out phase_offset);
			//if(phase_offset == 0 || phase_offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x4C, 0x01C0);
				Thread.Sleep(10);
				phase_offset = 0x01C0;
			}

			for(uint i=0; i<bands.Length; i++)
			{
				if(gain_table[(int)bands[i]] > 500.0f || gain_table[(int)bands[i]] < -500.0f)
				{
					MessageBox.Show("Error writing RX Image Gain value to EEPROM.\n"+
						bands[i].ToString()+" - Value out of range [-500.0, 500.0] ("+gain_table[(int)bands[i]].ToString("f4")+").");
					gain_table[(int)bands[i]] = 0.0f;
				}
				int error_count = 0;
				float test = -999.99f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(gain_offset+i*4), gain_table[(int)bands[i]]);
					Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(gain_offset+i*4), out test);
					Thread.Sleep(40);

					if(test != gain_table[(int)bands[i]])
					{
						if(error_count++ > 5)
						{
							MessageBox.Show("Error writing RX Image value to EEPROM.\n"+
								"Tried to write "+gain_table[(int)bands[i]].ToString("f4")+", but read back "+test.ToString("f4"));
							return;
						}
					}
				} while(test != gain_table[(int)bands[i]]);

				if(phase_table[(int)bands[i]] > 400.0f || phase_table[(int)bands[i]] < -400.0f)
				{
					MessageBox.Show("Error writing RX Image Phase value to EEPROM.\n"+
						bands[i].ToString()+" - Value out of range [-400.0, 400.0] ("+gain_table[(int)bands[i]].ToString("f4")+").");
					gain_table[(int)bands[i]] = 0.0f;
				}
				error_count = 0;
				test = -999.99f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(phase_offset+i*4), phase_table[(int)bands[i]]);
					Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(phase_offset+i*4), out test);
					Thread.Sleep(40);

					if(test != phase_table[(int)bands[i]])
					{
						if(error_count++ > 5)
						{
							MessageBox.Show("Error writing RX Image value to EEPROM.\n"+
								"Tried to write "+gain_table[(int)bands[i]].ToString("f4")+", but read back "+test.ToString("f4"));
							return;
						}
					}
				} while(test != phase_table[(int)bands[i]]);
			}
		}

		public static void ReadRXImage(float[] gain_table, float[] phase_table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort gain_offset;
			FWC.ReadTRXEEPROMUshort(0x4A, out gain_offset);
			Thread.Sleep(40);
			if(gain_offset == 0 || gain_offset == 0xFFFF) return;

			ushort phase_offset;
			FWC.ReadTRXEEPROMUshort(0x4C, out phase_offset);
			Thread.Sleep(40);
			if(phase_offset == 0 || phase_offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(gain_offset, out data);
			Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(uint i=0; i<bands.Length; i++)
			{
				float f;
				FWC.ReadTRXEEPROMFloat((uint)(gain_offset+i*4), out f);
				Thread.Sleep(40);
				if(f > 500.0f || f < -500.0f)
				{
					MessageBox.Show("Bad data detected in EEPROM.\n"+
						"RX Image Gain ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					f = 0.0f;
				}
				gain_table[(int)bands[i]] = f;
				FWC.ReadTRXEEPROMFloat((uint)(phase_offset+i*4), out f);
				Thread.Sleep(40);
				if(f > 400.0f || f < -400.0f)
				{
					MessageBox.Show("Bad data detected in EEPROM.\n"+
						"RX Image Phase ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					f = 0.0f;
				}
				phase_table[(int)bands[i]] = f;
			}
		}

		#endregion

		#region TX Image

		public static bool CheckTXImage()
		{
			Random rand = new Random();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			float[] gain_table = new float[(int)Band.LAST];
			float[] phase_table = new float[(int)Band.LAST];

			for(int i=0; i<bands.Length; i++)
			{
				gain_table[(int)bands[i]] = (float)rand.NextDouble();
				phase_table[(int)bands[i]] = (float)rand.NextDouble();
			}

			WriteTXImage(gain_table, phase_table);

			float[] gain_check = new float[(int)Band.LAST];
			float[] phase_check = new float[(int)Band.LAST];

			ReadTXImage(gain_check, phase_check);

			for(int i=0; i<bands.Length; i++)
			{
				if(gain_table[(int)bands[i]] != gain_check[(int)bands[i]] ||
					phase_table[(int)bands[i]] != phase_check[(int)bands[i]])
					return false;
			}
			return true;
		}

		public static void WriteTXImage(float[] gain_table, float[] phase_table)
		{
			WriteCalDateTime();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			
			ushort gain_offset;
			//FWC.ReadTRXEEPROMUshort(0x4E, out gain_offset);
			//if(gain_offset == 0 || gain_offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x4E, 0x01F0);
				Thread.Sleep(10);
				gain_offset = 0x01F0;
			}

			ushort phase_offset;
			//FWC.ReadTRXEEPROMUshort(0x50, out phase_offset);
			//if(phase_offset == 0 || phase_offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x50, 0x0220);
				Thread.Sleep(10);
				phase_offset = 0x0220;
			}

			for(uint i=0; i<bands.Length; i++)
			{
				if(gain_table[(int)bands[i]] > 500.0f || gain_table[(int)bands[i]] < -500.0f)
				{
					MessageBox.Show("Error writing TX Image Gain value to EEPROM.\n"+
						bands[i].ToString()+" - Value out of range [-500.0, 500.0] ("+gain_table[(int)bands[i]].ToString("f4")+").");
					gain_table[(int)bands[i]] = 0.0f;
				}
				int error_count = 0;
				float test = -999.99f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(gain_offset+i*4), gain_table[(int)bands[i]]);
					Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(gain_offset+i*4), out test);
					Thread.Sleep(40);

					if(test != gain_table[(int)bands[i]])
					{
						if(error_count++ > 5)
						{
							MessageBox.Show("Error writing TX Image value to EEPROM.\n"+
								"Tried to write "+gain_table[(int)bands[i]].ToString("f4")+", but read back "+test.ToString("f4"));
							return;
						}
					}
				} while(test != gain_table[(int)bands[i]]);

				if(phase_table[(int)bands[i]] > 400.0f || phase_table[(int)bands[i]] < -400.0f)
				{
					MessageBox.Show("Error writing TX Image Phase value to EEPROM.\n"+
						bands[i].ToString()+" - Value out of range [-400.0, 400.0] ("+gain_table[(int)bands[i]].ToString("f4")+").");
					gain_table[(int)bands[i]] = 0.0f;
				}
				error_count = 0;
				test = -999.99f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(phase_offset+i*4), phase_table[(int)bands[i]]);
					Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(phase_offset+i*4), out test);
					Thread.Sleep(40);

					if(test != phase_table[(int)bands[i]])
					{
						if(error_count++ > 5)
						{
							MessageBox.Show("Error writing TX Image value to EEPROM.\n"+
								"Tried to write "+phase_table[(int)bands[i]].ToString("f4")+", but read back "+test.ToString("f4"));
							return;
						}
					}
				} while(test != phase_table[(int)bands[i]]);
			}
		}

		public static void ReadTXImage(float[] gain_table, float[] phase_table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort gain_offset;
			FWC.ReadTRXEEPROMUshort(0x4E, out gain_offset);
			Thread.Sleep(40);
			if(gain_offset == 0 || gain_offset == 0xFFFF) return;

			ushort phase_offset;
			FWC.ReadTRXEEPROMUshort(0x50, out phase_offset);
			if(phase_offset == 0 || phase_offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(gain_offset, out data);
			Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(uint i=0; i<bands.Length; i++)
			{
				float f;
				FWC.ReadTRXEEPROMFloat((uint)(gain_offset+i*4), out f);
				Thread.Sleep(40);
				if(f > 500.0f || f < -500.0f)
				{
					MessageBox.Show("Bad data detected in EEPROM.\n"+
						"TX Image Gain ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					f = 0.0f;
				}
				gain_table[(int)bands[i]] = f;
				FWC.ReadTRXEEPROMFloat((uint)(phase_offset+i*4), out f);
				Thread.Sleep(40);
				if(f > 400.0f || f < -400.0f)
				{
					MessageBox.Show("Bad data detected in EEPROM.\n"+
						"TX Image Phase ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					f = 0.0f;
				}
				phase_table[(int)bands[i]] = f;
			}
		}

		#endregion

		#region TX Carrier

		public static bool CheckTXCarrier()
		{
			Random rand = new Random();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			int[][] tx_carrier_table = new int[(int)Band.LAST][];
			for(int i=0; i<(int)Band.LAST; i++)
				tx_carrier_table[i] = new int[4];

			for(int i=0; i<bands.Length; i++)
				for(int j=0; j<4; j++)
					tx_carrier_table[(int)bands[i]][j] = rand.Next(255);

			WriteTXCarrier(tx_carrier_table);

			int[][] tx_carrier_check = new int[(int)Band.LAST][];
			for(int i=0; i<(int)Band.LAST; i++)
				tx_carrier_check[i] = new int[4];

			ReadTXCarrier(tx_carrier_check);

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<4; j++)
				{
					if(tx_carrier_table[(int)bands[i]][j] != tx_carrier_check[(int)bands[i]][j])
						return false;
				}
			}
			return true;
		}

		public static void WriteTXCarrier(int[][] table)
		{
			WriteCalDateTime();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			
			ushort offset;
			//FWC.ReadTRXEEPROMUshort(0x52, out offset);
			//if(offset == 0 || offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x52, 0x0250);
				Thread.Sleep(40);
				offset = 0x0250;
			}

			for(int i=0; i<bands.Length; i++)
			{
				uint data = 0;
				for(int j=0; j<4; j++)
				{
					data += (uint)table[(int)bands[i]][j]<<(j*8);
				}

				uint temp = 0;
				int error_count = 0;
				do
				{
					FWC.WriteTRXEEPROMUint((uint)(offset+i*4), data);
					Thread.Sleep(10);

					FWC.ReadTRXEEPROMUint((uint)(offset+i*4), out temp);
					Thread.Sleep(40);

					if(temp != data)
					{
						if(error_count++ > 5)
						{
							MessageBox.Show("Error writing TX Carrier value to EEPROM.\n"+
								"Tried to write "+data+", but read back "+temp);
							return;
						}
					}
				} while(temp != data);
			}
		}

		public static void ReadTXCarrier(int[][] table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort offset;
			FWC.ReadTRXEEPROMUshort(0x52, out offset);
			Thread.Sleep(40);
			if(offset == 0 || offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(offset, out data);
			Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(int i=0; i<bands.Length; i++)
			{
				FWC.ReadTRXEEPROMUint((uint)(offset+i*4), out data);
				Thread.Sleep(40);
				for(int j=0; j<4; j++)
					table[(int)bands[i]][j] = (byte)(data>>(j*8));
			}
		}

		#endregion

		#region PA Bias

		public static bool CheckPABias()
		{
			Random rand = new Random();

			int[][] pa_bias_table = new int[4][];
			for(int i=0; i<4; i++)
				pa_bias_table[i] = new int[8];

			for(int i=0; i<4; i++)
				for(int j=0; j<8; j++)
					pa_bias_table[i][j] = rand.Next(255);

			WritePABias(pa_bias_table);

			int[][] pa_bias_check = new int[4][];
			for(int i=0; i<4; i++)
				pa_bias_check[i] = new int[8];

			ReadPABias(pa_bias_check);

			for(int i=0; i<4; i++)
			{
				for(int j=0; j<8; j++)
				{
					if(pa_bias_table[i][j] != pa_bias_check[i][j])
						return false;
				}
			}
			return true;
		}

		public static void WritePABias(int[][] table)
		{
			WriteCalDateTime();
			
			ushort offset;
			//FWC.ReadTRXEEPROMUshort(0x54, out offset);
			//if(offset == 0 || offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x54, 0x02C0);
				Thread.Sleep(40);
				offset = 0x02C0;
			}

			for(int i=0; i<4; i++)
			{
				uint data = 0;
				for(int j=0; j<4; j++)
					data += (uint)table[i][j]<<(j*8);

				uint temp = 0;
				int error_count = 0;
				do
				{
					FWC.WriteTRXEEPROMUint((uint)(offset+i*8), data);
					Thread.Sleep(10);

					FWC.ReadTRXEEPROMUint((uint)(offset+i*8), out temp);
					Thread.Sleep(40);

					if(temp != data)
					{
						if(error_count++ > 5)
						{
							MessageBox.Show("Error writing PA Bias value to EEPROM.\n"+
								"Tried to write "+data+", but read back "+temp);
							return;
						}
					}
				} while(temp != data);

				data = 0;
				for(int j=4; j<8; j++)
					data += (uint)table[i][j]<<((j-4)*8);

				error_count = 0;
				temp = 0;
				do
				{
					FWC.WriteTRXEEPROMUint((uint)(offset+i*8+4), data);
					Thread.Sleep(10);

					FWC.ReadTRXEEPROMUint((uint)(offset+i*8+4), out temp);
					Thread.Sleep(40);

					if(temp != data)
					{
						if(error_count++ > 5)
						{
							MessageBox.Show("Error writing PA Bias value to EEPROM.\n"+
								"Tried to write "+data+", but read back "+temp);
							return;
						}
					}
				} while(temp != data);
			}
		}

		public static void ReadPABias(int[][] table)
		{
			ushort offset;
			FWC.ReadTRXEEPROMUshort(0x54, out offset);
			Thread.Sleep(40);
			if(offset == 0 || offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(offset, out data);
			Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(int i=0; i<4; i++)
			{
				FWC.ReadTRXEEPROMUint((uint)(offset+i*8), out data);
				Thread.Sleep(40);
				for(int j=0; j<4; j++)
					table[i][j] = (byte)(data>>(j*8));

				FWC.ReadTRXEEPROMUint((uint)(offset+i*8+4), out data);
				Thread.Sleep(40);
				for(int j=4; j<8; j++)
					table[i][j] = (byte)(data>>((j-4)*8));
			}
		}

		#endregion

		#region PA Bridge

		public static bool CheckPABridge()
		{
			Random rand = new Random();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			float[][] pa_bridge_table = new float[(int)Band.LAST][];
			for(int i=0; i<(int)Band.LAST; i++)
				pa_bridge_table[i] = new float[6];

			for(int i=0; i<bands.Length; i++)
				for(int j=0; j<4; j++)
					pa_bridge_table[(int)bands[i]][j] = (float)rand.NextDouble();

			WritePABridge(pa_bridge_table);

			float[][] pa_bridge_check = new float[(int)Band.LAST][];
			for(int i=0; i<(int)Band.LAST; i++)
				pa_bridge_check[i] = new float[6];

			ReadPABridge(pa_bridge_check);

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<6; j++)
				{
					if(pa_bridge_table[(int)bands[i]][j] != pa_bridge_check[(int)bands[i]][j])
						return false;
				}
			}
			return true;
		}

		public static void WritePABridge(float[][] table)
		{
			WriteCalDateTime();			
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort offset;
			//FWC.ReadTRXEEPROMUshort(0x56, out offset);
			//if(offset == 0 || offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x56, 0x02E0);
				Thread.Sleep(10);
				offset = 0x02E0;
			}

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<6; j++)
				{
					int error_count = 0;
					float temp = 0.0f;
					do
					{
						FWC.WriteTRXEEPROMFloat((uint)(offset+i*24+j*4), table[(int)bands[i]][j]);
						Thread.Sleep(10);

						FWC.ReadTRXEEPROMFloat((uint)(offset+i*24+j*4), out temp);
						Thread.Sleep(40);

						if(temp != table[(int)bands[i]][j])
						{
							if(error_count++ > 5)
							{
								MessageBox.Show("Error writing PA Bridge value to EEPROM.\n"+
									"Tried to write "+table[(int)bands[i]][j].ToString("f4")+", but read back "+temp.ToString("f4"));
								return;
							}
						}
					} while(temp != table[(int)bands[i]][j]);
				}
			}
		}

		public static void ReadPABridge(float[][] table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort offset;
			FWC.ReadTRXEEPROMUshort(0x56, out offset);
			Thread.Sleep(40);
			if(offset == 0 || offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(offset, out data);
			Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(int i=0; i<bands.Length; i++)
			{
				float f;
				for(int j=0; j<6; j++)
				{
					FWC.ReadTRXEEPROMFloat((uint)(offset+i*24+j*4), out f);
					Thread.Sleep(40);
					table[(int)bands[i]][j] = f;
				}
			}
		}

		#endregion PA Bridge

		#region PA Power

		public static bool CheckPAPower()
		{
			Random rand = new Random();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			float[][] pa_power_table = new float[(int)Band.LAST][];
			for(int i=0; i<(int)Band.LAST; i++)
				pa_power_table[i] = new float[13];

			for(int i=0; i<bands.Length; i++)
				for(int j=0; j<13; j++)
					pa_power_table[(int)bands[i]][j] = (float)rand.NextDouble();

			WritePAPower(pa_power_table);

			float[][] pa_power_check = new float[(int)Band.LAST][];
			for(int i=0; i<(int)Band.LAST; i++)
				pa_power_check[i] = new float[13];

			ReadPAPower(pa_power_check);

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<13; j++)
				{
					if(pa_power_table[(int)bands[i]][j] != pa_power_check[(int)bands[i]][j])
						return false;
				}
			}
			return true;
		}

		public static void WritePAPower(float[][] table)
		{
			WriteCalDateTime();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			
			ushort offset;
			//FWC.ReadTRXEEPROMUshort(0x58, out offset);
			//if(offset == 0 || offset == 0xFFFF)
			{
				FWC.WriteTRXEEPROMUshort(0x58, 0x03F0);
				Thread.Sleep(10);
				offset = 0x03F0;
			}

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<13; j++)
				{
					if(table[(int)bands[i]][j] > 1.0f || table[(int)bands[i]][j] < 0.0f)
					{
						MessageBox.Show("Error writing PA Power value to EEPROM.\n"+
							"Value out of range 0.0 to 1.0 ("+table[(int)bands[i]][j].ToString("f4")+").");
						table[(int)bands[i]][j] = 0.0f;
					}
					int error_count = 0;
					float temp = 0.0f;
					do
					{
						FWC.WriteTRXEEPROMFloat((uint)(offset+i*52+j*4), table[(int)bands[i]][j]);
						Thread.Sleep(10);

						FWC.ReadTRXEEPROMFloat((uint)(offset+i*52+j*4), out temp);
						Thread.Sleep(40);

						if(temp != table[(int)bands[i]][j])
						{
							if(error_count++ > 5)
							{
								MessageBox.Show("Error writing PA Power value to EEPROM.\n"+
									"Tried to write "+table[(int)bands[i]][j].ToString("f4")+", but read back "+temp.ToString("f4"));
								return;
							}
						}
					} while(temp != table[(int)bands[i]][j]);
				}
			}
		}

		public static void ReadPAPower(float[][] table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort offset;
			FWC.ReadTRXEEPROMUshort(0x58, out offset);
			Thread.Sleep(40);
			if(offset == 0 || offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(offset, out data);
			Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<13; j++)
				{
					float f;
					FWC.ReadTRXEEPROMFloat((uint)(offset+i*52+j*4), out f);
					Thread.Sleep(40);
					if(f > 1.0f || f < 0.0f)
					{
						MessageBox.Show("Bad data detected in EEPROM.\n"+
							"PA Power ("+bands[i].ToString()+", "+j+") = "+f.ToString("f4"));
						table[(int)bands[i]][j] = 0.0f;
					}
					else table[(int)bands[i]][j] = f;
				}
			}
		}

		#endregion

		#region PA SWR

		public static bool CheckPASWR()
		{
			Random rand = new Random();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			float[] pa_swr_table = new float[(int)Band.LAST];

			for(int i=0; i<bands.Length; i++)
				pa_swr_table[(int)bands[i]] = (float)rand.NextDouble();

			WritePASWR(pa_swr_table);

			float[] pa_swr_check = new float[(int)Band.LAST];
			
			ReadPASWR(pa_swr_check);

			for(int i=0; i<bands.Length; i++)
			{
				if(pa_swr_table[(int)bands[i]] != pa_swr_check[(int)bands[i]])
					return false;
			}
			return true;
		}

		public static void WritePASWR(float[] table)
		{
			WriteCalDateTime();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			
			ushort offset;
			//FWC.ReadTRXEEPROMUshort(0x5A, out offset);
			//if(offset == 0 || offset == 0xFFFF)
			//{
			FWC.WriteTRXEEPROMUshort(0x5A, 0x0630);
			Thread.Sleep(10);
			offset = 0x0630;
			//}

			for(int i=0; i<bands.Length; i++)
			{
				int error_count = 0;
				float temp = 0.0f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(offset+i*4), (float)table[(int)bands[i]]);
					Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(offset+i*4), out temp);
					Thread.Sleep(40);

					if(temp != table[(int)bands[i]])
					{
						if(error_count++ > 5)
						{
							MessageBox.Show("Error writing PA SWR value to EEPROM.\n"+
								"Tried to write "+table[(int)bands[i]].ToString("f4")+", but read back "+temp.ToString("f4"));
							return;
						}
					}
				} while(temp != table[(int)bands[i]]);
			}
		}

		public static void ReadPASWR(float[] table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort offset;
			FWC.ReadTRXEEPROMUshort(0x5A, out offset);
			Thread.Sleep(40);
			if(offset == 0 || offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(offset, out data);
			Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(int i=0; i<bands.Length; i++)
			{
				float f;
				FWC.ReadTRXEEPROMFloat((uint)(offset+i*4), out f);
				Thread.Sleep(40);
				table[(int)bands[i]] = f;
			}
		}

		#endregion
	}
}