//=================================================================
// hid_eeprom.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2009  FlexRadio Systems
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
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PowerSDR
{
	public class HIDEEPROM
	{
		#region Misc Routines

		public static void Init()
		{
			USBHID.GetSerialNum(out serial_number); // get serial number

            USBHID.GetTRXSN(out trx_serial);
            USBHID.GetTRXRev(out trx_rev);

            USBHID.GetPASN(out pa_serial);
            USBHID.GetPARev(out pa_rev);
			
			byte[] data = new byte[8]; // get last cal date/time
            USBHID.ReadEEPROM(0x1820, 8, out data);
            last_cal_date_time = BitConverter.ToInt64(data, 0);
            
            /* USBHID.ReadTRXEEPROMByte(0x1BE, out temp);
			rx1_image_ver = temp;*/

            USBHID.GetRegion(out region);
		}

		public static bool NeedDump()
		{
			if(File.Exists(Application.StartupPath+"\\nobackup")) return false;  // for production
			uint data;
			
			StringBuilder s = new StringBuilder("F1.5K_");

            USBHID.ReadTRXEEPROMUint(0x180C, out data);
			s.Append(((byte)(data)).ToString("00"));
			s.Append(((byte)(data>>8)).ToString("00"));
			s.Append("-"+((ushort)(data>>16)).ToString("0000"));
			if(File.Exists(app_data_path+"Backup\\"+s+" backup.csv"))
				return false;
			return true;
		}

		public static void StartDump()
		{
			Thread t = new Thread(new ThreadStart(Dump));
			t.Name = "EEPROM Dump Thread";
			t.IsBackground = true;
			t.Priority = ThreadPriority.Normal;
			t.Start();
			while(t.IsAlive)
			{
				Thread.Sleep(5);
			}
		}

		private static void Dump()
		{
			if(File.Exists(Application.StartupPath+"\\nobackup")) return;  // for production
			uint data;
			
			StringBuilder s = new StringBuilder("F1.5K_");

            USBHID.ReadTRXEEPROMUint(0x180C, out data);
			s.Append(((byte)(data)).ToString("00"));
			s.Append(((byte)(data>>8)).ToString("00"));
			s.Append("-"+((ushort)(data>>16)).ToString("0000"));
            if(!Directory.Exists(app_data_path+"\\Backup\\"))
                Directory.CreateDirectory(app_data_path + "\\Backup\\");
            if (File.Exists(app_data_path + "\\Backup\\" + s + " backup.csv"))
			{
				return;
			}
			StreamWriter writer = new StreamWriter(app_data_path+"\\Backup\\"+s+" backup.csv");

			s = new StringBuilder(",");
			for(int i=0; i<16; i++)
				s.Append(i.ToString("X")+",");
			writer.WriteLine(s);

            byte[] buf = new byte[64];
			for(int i=0; i<64; i++) // 64*32 = 2048
			{
                USBHID.ReadEEPROM((ushort)(0x1800 + i*32), 32, out buf);

                s = new StringBuilder((0x1800 + i * 2).ToString("X") + ",");

				for(int j=0; j<16; j++)
                    s.Append(buf[j].ToString("X")+",");

                writer.WriteLine(s);
                Application.DoEvents();

                s = new StringBuilder((0x1800 + i * 2 + 1).ToString("X") + ",");

                for (int j = 0; j < 16; j++)
                     s.Append(buf[j+16].ToString("X") + ",");

                writer.WriteLine(s);
			}

			writer.Close();
		}

		public static string SerialToString(uint serial)
		{
			string s = "";
			s += ((byte)(serial>>24)).ToString("00");
			s += ((byte)(serial>>16)).ToString("00")+"-";
			s += ((ushort)(serial)).ToString("0000");
			return s;
		}

		private static void WriteCalDateTime()
		{
			long l = DateTime.Now.ToFileTimeUtc();
            byte[] buf = BitConverter.GetBytes(l);
            CheckedWrite(0x1820, buf, 8);
			last_cal_date_time = l;
		}

		/*public static bool CheckAll()
		{
			bool b = true;
			b = CheckRXLevel(); if(!b) return b;
			b = CheckRXImage(); if(!b) return b;
			b = CheckTXImage(); if(!b) return b;
			
			return b;
		}*/

        private static bool CheckedWrite(ushort addr, byte val)
        {
            byte[] buf = new byte[1];
            buf[0] = val;
            return CheckedWrite(addr, buf, 1);
        }

        private static bool CheckedWrite(ushort addr, byte[] buf, byte num_bytes)
        {
            int error_count = 0;
            bool validated = true;
            byte[] test = new byte[num_bytes];

            do
            {
                int val = USBHID.WriteEEPROM(addr, num_bytes, buf);
                Thread.Sleep(10);

                val = USBHID.ReadEEPROM(addr, num_bytes, out test);

                validated = true;
                for (int j = 0; j < num_bytes; j++)
                {
                    if (test[j] != buf[j])
                    {
                        validated = false;
                        break;
                    }
                }

                if (!validated) error_count++;
                if (error_count > NUM_WRITES_TO_TRY)
                    return false;

            } while (!validated);

            return true;
        }

		#endregion

		#region Properties

        private static string app_data_path = "";
        public static string AppDataPath
        {
            set { app_data_path = value; }
        }

		private static FRSRegion region = FRSRegion.US;
		public static FRSRegion Region
		{ 
			get { return region; }
		}

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

		private static uint trx_serial;
		public static uint TRXSerial
		{
			get { return trx_serial; }
			//set { trx_serial = value; }
		}

		private static uint trx_rev;
		public static uint TRXRev
		{
			get { return trx_rev; }
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

		private static int rx1_image_ver = 0;
		public static int RX1ImageVer
		{
			get { return rx1_image_ver; }
		}

		#endregion
        
		#region RX

		#region RX Level

		private const int NUM_WRITES_TO_TRY = 5;
        public static bool CheckRXLevel()
        {
            Random rand = new Random();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            float[][] rx_level_table = new float[(int)Band.LAST][];
            for (int i = 0; i < (int)Band.LAST; i++)
                rx_level_table[i] = new float[3];

            for (int i = 0; i < bands.Length; i++)
                for (int j = 0; j < 3; j++)
                    rx_level_table[(int)bands[i]][j] = (float)rand.NextDouble();

            byte temp;
            WriteRXLevel(rx_level_table, out temp);

            float[][] rx_level_check = new float[(int)Band.LAST][];
            for (int i = 0; i < (int)Band.LAST; i++)
                rx_level_check[i] = new float[3];

            ReadRXLevel(rx_level_check);

            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (rx_level_table[(int)bands[i]][j] != rx_level_check[(int)bands[i]][j])
                        return false;
                }
            }
            return true;
        }

		public static void WriteRXLevel(float[][] table, out byte checksum)
		{
			WriteCalDateTime();			
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort addr = 0x3000;
            byte[] buf = new byte[32];
            int length = 0;

			for(int i=0; i<bands.Length; i++)
			{
				for(int j=0; j<2; j++)
				{
                    short val = (short)(Math.Round(table[(int)bands[i]][j], 1)*10);
                    BitConverter.GetBytes(val).CopyTo(buf, length);
                    length += 2;
                }

                if(length == 32)
                {
                    if (!CheckedWrite(addr, buf, (byte)length))
                    {
                        MessageBox.Show("Error writing RX Level value to EEPROM.");
                        checksum = 0xFF;
                        return;
                    }

                    length = 0;
                    addr += 32;
				}
			}

            if (length > 0)
            {
                if (!CheckedWrite(addr, buf, (byte)length))
                {
                    MessageBox.Show("Error writing RX Level value to EEPROM.");
                    checksum = 0xFF;
                    return;
                }
            }

			// calculate and write checksum
            byte sum = Checksum.Calc(table);
			WriteRXLevelChecksum(sum);
			checksum = sum;
		}

		public static void WriteRXLevelChecksum(byte sum)
		{
			if(!CheckedWrite(0x302F, sum))
			    MessageBox.Show("Error writing RX Level checksum to EEPROM.");
		}

		public static void ReadRXLevel(float[][] table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort addr = 0x3000;
            byte[] buf1 = new byte[32];
            byte[] buf2 = new byte[32];
			USBHID.ReadEEPROM(addr, 32, out buf1);
            addr += 32;
            USBHID.ReadEEPROM(addr, 32, out buf2);
            int index = 0;
			
			if(buf1[0] == 0xFF && buf1[1] == 0xFF) return;

            for (int i = 0; i < bands.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    short val = 0;
                    if (index < 32)
                        val = BitConverter.ToInt16(buf1, index);
                    else val = BitConverter.ToInt16(buf2, index - 32);
                    index += 2;

                    table[(int)bands[i]][j] = (float)(val / 10.0);
                }
            }
		}

		public static byte ReadRXLevelChecksum()
		{
			byte read;
			USBHID.ReadTRXEEPROMByte(0x302F, out read);
			return read;
		}

		#endregion
        /*
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

			byte gain_sum = Checksum.Calc(gain_table);
			byte phase_sum = Checksum.Calc(phase_table);

			byte temp;
			WriteRXImage(gain_table, phase_table, out temp, out temp);

			float[] gain_check = new float[(int)Band.LAST];
			float[] phase_check = new float[(int)Band.LAST];

			ReadRXImage(gain_check, phase_check);

			byte gain_sum_check = Checksum.Calc(gain_check);
			byte phase_sum_check = Checksum.Calc(phase_check);

			if((gain_sum_check != gain_sum) ||
				(phase_sum_check != phase_sum))
				return false;

			for(int i=0; i<bands.Length; i++)
			{
				if(gain_table[(int)bands[i]] != gain_check[(int)bands[i]] ||
					phase_table[(int)bands[i]] != phase_check[(int)bands[i]])
					return false;
			}
			return true;
		}

		public static void WriteRXImage(float[] gain_table, float[] phase_table, out byte gain_sum, out byte phase_sum)
		{
			WriteCalDateTime();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			
			uint addr = 0x4A;
			ushort gain_offset = 0x0190;
			ushort temp;
			int error_count = 0;			
			do
			{
				FWC.WriteTRXEEPROMUshort(addr, gain_offset);
				//Thread.Sleep(10);
				FWC.ReadTRXEEPROMUshort(addr, out temp);
				//Thread.Sleep(40);

				if(temp != gain_offset)
				{
					if(error_count++ > NUM_WRITES_TO_TRY)
					{
						MessageBox.Show("Error writing RX Image Gain pointer to EEPROM.\n"+
							"Tried to write "+gain_offset.ToString()+", but read back "+temp.ToString());
						gain_sum = phase_sum = 0xFF;
						return;
					}
				}
			} while(temp != gain_offset);
			
			FWC.WriteTRXEEPROMByte(0x1BE, 4);
			rx1_image_ver = 4;

			addr = 0x4C;
			ushort phase_offset = 0x01C0;
			error_count = 0;
			do
			{
				FWC.WriteTRXEEPROMUshort(addr, phase_offset);
				//Thread.Sleep(10);
				FWC.ReadTRXEEPROMUshort(addr, out temp);
				//Thread.Sleep(40);

				if(temp != phase_offset)
				{
					if(error_count++ > NUM_WRITES_TO_TRY)
					{
						MessageBox.Show("Error writing RX Image Phase pointer to EEPROM.\n"+
							"Tried to write "+phase_offset.ToString()+", but read back "+temp.ToString());
						gain_sum = phase_sum = 0xFF;
						return;
					}
				}
			} while(temp != phase_offset);

			for(uint i=0; i<bands.Length; i++)
			{
				if(gain_table[(int)bands[i]] > 500.0f || gain_table[(int)bands[i]] < -500.0f)
				{
					//MessageBox.Show("Error writing RX Image Gain value to EEPROM.\n"+
					//	bands[i].ToString()+" - Value out of range [-500.0, 500.0] ("+gain_table[(int)bands[i]].ToString("f4")+").\n"+
					//	"Recalibrate RX Image on this band.");
					TextWriter writer = new StreamWriter(app_data_path+"\\eeprom_error.log", true);
					writer.WriteLine(DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString()+" "+
						"Error writing RX Image Gain value to EEPROM -- "+
						bands[i].ToString()+" - Value out of range [-500.0, 500.0] ("+gain_table[(int)bands[i]].ToString("f4")+").");
					writer.Close();
					gain_table[(int)bands[i]] = 0.0f;
				}
				error_count = 0;
				float test = -999.99f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(gain_offset+i*4), gain_table[(int)bands[i]]);
					//Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(gain_offset+i*4), out test);
					//Thread.Sleep(40);

					if(test != gain_table[(int)bands[i]])
					{
						if(error_count++ > NUM_WRITES_TO_TRY)
						{
							MessageBox.Show("Error writing RX Image value to EEPROM.\n"+
								"Tried to write "+gain_table[(int)bands[i]].ToString("f4")+", but read back "+test.ToString("f4"));
							gain_sum = phase_sum = 0xFF;
							return;
						}
					}
				} while(test != gain_table[(int)bands[i]]);

				if(phase_table[(int)bands[i]] > 400.0f || phase_table[(int)bands[i]] < -400.0f)
				{
					//MessageBox.Show("Error writing RX Image Phase value to EEPROM.\n"+
					//	bands[i].ToString()+" - Value out of range [-400.0, 400.0] ("+gain_table[(int)bands[i]].ToString("f4")+").\n"+
					//	"Recalibrate RX Image on this band.");
					TextWriter writer = new StreamWriter(app_data_path+"\\eeprom_error.log", true);
					writer.WriteLine(DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString()+" "+
						"Error writing RX Image Phase value to EEPROM -- "+
						bands[i].ToString()+" - Value out of range [-400.0, 400.0] ("+gain_table[(int)bands[i]].ToString("f4")+").");
					writer.Close();
					gain_table[(int)bands[i]] = 0.0f;
				}
				error_count = 0;
				test = -999.99f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(phase_offset+i*4), phase_table[(int)bands[i]]);
					//Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(phase_offset+i*4), out test);
					//Thread.Sleep(40);

					if(test != phase_table[(int)bands[i]])
					{
						if(error_count++ > NUM_WRITES_TO_TRY)
						{
							MessageBox.Show("Error writing RX Image value to EEPROM.\n"+
								"Tried to write "+gain_table[(int)bands[i]].ToString("f4")+", but read back "+test.ToString("f4"));
							gain_sum = phase_sum = 0xFF;
							return;
						}
					}
				} while(test != phase_table[(int)bands[i]]);
			}

			// calculate and write checksums
			byte sum = Checksum.Calc(gain_table);
			WriteRXImageGainChecksum(sum);
			gain_sum = sum;

			sum = Checksum.Calc(phase_table);
			WriteRXImagePhaseChecksum(sum);
			phase_sum = sum;
		}

		public static void WriteRXImageGainChecksum(byte sum)
		{
			byte read, errors=0;
			do
			{				
				FWC.WriteTRXEEPROMByte(0x1BF, sum);
				//Thread.Sleep(10);

				FWC.ReadTRXEEPROMByte(0x1BF, out read);
				if(read != sum)
				{
					if(errors++ > NUM_WRITES_TO_TRY)
					{
						MessageBox.Show("Error writing RX Image Gain checksum to EEPROM.\n"+
							"Tried to write "+sum+", but read back "+read);
						return;
					}
				}
			} while(read != sum);
		}

		public static void WriteRXImagePhaseChecksum(byte sum)
		{
			byte read, errors=0;
			do
			{				
				FWC.WriteTRXEEPROMByte(0x1EF, sum);
				//Thread.Sleep(10);

				FWC.ReadTRXEEPROMByte(0x1EF, out read);
				if(read != sum)
				{
					if(errors++ > NUM_WRITES_TO_TRY)
					{
						MessageBox.Show("Error writing RX Image Phase checksum to EEPROM.\n"+
							"Tried to write "+sum+", but read back "+read);
						return;
					}
				}
			} while(read != sum);
		}

		public static void ReadRXImage(float[] gain_table, float[] phase_table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort gain_offset;
			FWC.ReadTRXEEPROMUshort(0x4A, out gain_offset);
			//Thread.Sleep(40);
			if(gain_offset == 0 || gain_offset == 0xFFFF) return;

			ushort phase_offset;
			FWC.ReadTRXEEPROMUshort(0x4C, out phase_offset);
			//Thread.Sleep(40);
			if(phase_offset == 0 || phase_offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(gain_offset, out data);
			//Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(uint i=0; i<bands.Length; i++)
			{
				float f;
				FWC.ReadTRXEEPROMFloat((uint)(gain_offset+i*4), out f);
				//Thread.Sleep(40);
				if(f > 500.0f || f < -500.0f)
				{
					MessageBox.Show("Bad data detected in EEPROM.\n"+
						"RX Image Gain ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					f = 0.0f;
				}
				gain_table[(int)bands[i]] = f;
				FWC.ReadTRXEEPROMFloat((uint)(phase_offset+i*4), out f);
				//Thread.Sleep(40);
				if(f > 400.0f || f < -400.0f)
				{
					MessageBox.Show("Bad data detected in EEPROM.\n"+
						"RX Image Phase ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					f = 0.0f;
				}
				phase_table[(int)bands[i]] = f;
			}
		}

		public static byte ReadRXImageGainChecksum()
		{
			byte read;
			FWC.ReadTRXEEPROMByte(0x1BF, out read);
			return read;
		}

		public static byte ReadRXImagePhaseChecksum()
		{
			byte read;
			FWC.ReadTRXEEPROMByte(0x1EF, out read);
			return read;
		}

		#endregion

		#endregion

		#region TX

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

			byte temp;
			WriteTXImage(gain_table, phase_table, out temp, out temp);

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

		public static void WriteTXImage(float[] gain_table, float[] phase_table, out byte gain_sum, out byte phase_sum)
		{
			WriteCalDateTime();
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };
			
			uint addr = 0x4E;
			ushort gain_offset = 0x01F0;
			ushort temp;
			int error_count = 0;			
			do
			{
				FWC.WriteTRXEEPROMUshort(addr, gain_offset);
				//Thread.Sleep(10);
				FWC.ReadTRXEEPROMUshort(addr, out temp);
				//Thread.Sleep(40);

				if(temp != gain_offset)
				{
					if(error_count++ > NUM_WRITES_TO_TRY)
					{
						MessageBox.Show("Error writing TX Image Gain pointer to EEPROM.\n"+
							"Tried to write "+gain_offset.ToString()+", but read back "+temp.ToString());
						gain_sum = phase_sum = 0xFF;
						return;
					}
				}
			} while(temp != gain_offset);

			addr = 0x50;
			ushort phase_offset = 0x0220;
			error_count = 0;
			do
			{
				FWC.WriteTRXEEPROMUshort(addr, phase_offset);
				//Thread.Sleep(10);
				FWC.ReadTRXEEPROMUshort(addr, out temp);
				//Thread.Sleep(40);

				if(temp != phase_offset)
				{
					if(error_count++ > NUM_WRITES_TO_TRY)
					{
						MessageBox.Show("Error writing TX Image Phase pointer to EEPROM.\n"+
							"Tried to write "+phase_offset.ToString()+", but read back "+temp.ToString());
						gain_sum = phase_sum = 0xFF;
						return;
					}
				}
			} while(temp != phase_offset);

			for(uint i=0; i<bands.Length; i++)
			{
				if(gain_table[(int)bands[i]] > 500.0f || gain_table[(int)bands[i]] < -500.0f)
				{
					//MessageBox.Show("Error writing TX Image Gain value to EEPROM.\n"+
					//	bands[i].ToString()+" - Value out of range [-500.0, 500.0] ("+gain_table[(int)bands[i]].ToString("f4")+").\n"+
					//	"Recalibrate TX Image on this band.");
					TextWriter writer = new StreamWriter(app_data_path+"\\eeprom_error.log", true);
					writer.WriteLine(DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString()+" "+
						"Error writing TX Image Gain value to EEPROM -- "+
						bands[i].ToString()+" - Value out of range [-500.0, 500.0] ("+gain_table[(int)bands[i]].ToString("f4")+").");
					writer.Close();
					gain_table[(int)bands[i]] = 0.0f;
				}
				error_count = 0;
				float test = -999.99f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(gain_offset+i*4), gain_table[(int)bands[i]]);
					//Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(gain_offset+i*4), out test);
					//Thread.Sleep(40);

					if(test != gain_table[(int)bands[i]])
					{
						if(error_count++ > NUM_WRITES_TO_TRY)
						{
							MessageBox.Show("Error writing TX Image value to EEPROM.\n"+
								"Tried to write "+gain_table[(int)bands[i]].ToString("f4")+", but read back "+test.ToString("f4"));
							gain_sum = phase_sum = 0xFF;
							return;
						}
					}
				} while(test != gain_table[(int)bands[i]]);

				if(phase_table[(int)bands[i]] > 400.0f || phase_table[(int)bands[i]] < -400.0f)
				{
					MessageBox.Show("Error writing TX Image Phase value to EEPROM.\n"+
						bands[i].ToString()+" - Value out of range [-400.0, 400.0] ("+gain_table[(int)bands[i]].ToString("f4")+").\n"+
						"Recalibrate TX Image on this band.");
					gain_table[(int)bands[i]] = 0.0f;
				}
				error_count = 0;
				test = -999.99f;
				do
				{
					FWC.WriteTRXEEPROMFloat((uint)(phase_offset+i*4), phase_table[(int)bands[i]]);
					//Thread.Sleep(10);

					FWC.ReadTRXEEPROMFloat((uint)(phase_offset+i*4), out test);
					//Thread.Sleep(40);

					if(test != phase_table[(int)bands[i]])
					{
						if(error_count++ > NUM_WRITES_TO_TRY)
						{
							MessageBox.Show("Error writing TX Image value to EEPROM.\n"+
								"Tried to write "+phase_table[(int)bands[i]].ToString("f4")+", but read back "+test.ToString("f4"));
							gain_sum = phase_sum = 0xFF;
							return;
						}
					}
				} while(test != phase_table[(int)bands[i]]);
			}

			// calculate and write checksums
			byte sum = Checksum.Calc(gain_table);
			WriteTXImageGainChecksum(sum);
			gain_sum = sum;

			sum = Checksum.Calc(phase_table);
			WriteTXImagePhaseChecksum(sum);
			phase_sum = sum;
		}

		public static void WriteTXImageGainChecksum(byte sum)
		{
			byte read, errors=0;
			do
			{				
				FWC.WriteTRXEEPROMByte(0x21F, sum);
				//Thread.Sleep(10);

				FWC.ReadTRXEEPROMByte(0x21F, out read);
				if(read != sum)
				{
					if(errors++ > NUM_WRITES_TO_TRY)
					{
						MessageBox.Show("Error writing TX Image Gain checksum to EEPROM.\n"+
							"Tried to write "+sum+", but read back "+read);
						return;
					}
				}
			} while(read != sum);
		}

		public static void WriteTXImagePhaseChecksum(byte sum)
		{
			byte read, errors=0;
			do
			{				
				FWC.WriteTRXEEPROMByte(0x24F, sum);
				//Thread.Sleep(10);

				FWC.ReadTRXEEPROMByte(0x24F, out read);
				if(read != sum)
				{
					if(errors++ > NUM_WRITES_TO_TRY)
					{
						MessageBox.Show("Error writing TX Image Phase checksum to EEPROM.\n"+
							"Tried to write "+sum+", but read back "+read);
						return;
					}
				}
			} while(read != sum);
		}

		public static void ReadTXImage(float[] gain_table, float[] phase_table)
		{
			Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

			ushort gain_offset;
			FWC.ReadTRXEEPROMUshort(0x4E, out gain_offset);
			//Thread.Sleep(40);
			if(gain_offset == 0 || gain_offset == 0xFFFF) return;

			ushort phase_offset;
			FWC.ReadTRXEEPROMUshort(0x50, out phase_offset);
			if(phase_offset == 0 || phase_offset == 0xFFFF) return;

			uint data;
			FWC.ReadTRXEEPROMUint(gain_offset, out data);
			//Thread.Sleep(40);
			if(data == 0xFFFFFFFF) return;

			for(uint i=0; i<bands.Length; i++)
			{
				float f;
				FWC.ReadTRXEEPROMFloat((uint)(gain_offset+i*4), out f);
				//Thread.Sleep(40);
				if(f > 500.0f || f < -500.0f)
				{
					//MessageBox.Show("Bad data detected in EEPROM.\n"+
					//	"TX Image Gain ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					TextWriter writer = new StreamWriter(app_data_path+"\\eeprom_error.log", true);
					writer.WriteLine(DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString()+" "+
						"Bad data detected in EEPROM -- "+
						"TX Image Gain ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					writer.Close();
					f = 0.0f;
				}
				gain_table[(int)bands[i]] = f;
				FWC.ReadTRXEEPROMFloat((uint)(phase_offset+i*4), out f);
				//Thread.Sleep(40);
				if(f > 400.0f || f < -400.0f)
				{
					//MessageBox.Show("Bad data detected in EEPROM.\n"+
					//	"TX Image Phase ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					TextWriter writer = new StreamWriter(app_data_path+"\\eeprom_error.log", true);
					writer.WriteLine(DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString()+" "+
						"Bad data detected in EEPROM -- "+
						"TX Image Phase ("+bands[i].ToString()+" = "+f.ToString("f2")+")");
					writer.Close();
					f = 0.0f;
				}
				phase_table[(int)bands[i]] = f;
			}
		}

		public static byte ReadTXImageGainChecksum()
		{
			byte read;
			FWC.ReadTRXEEPROMByte(0x21F, out read);
			return read;
		}

		public static byte ReadTXImagePhaseChecksum()
		{
			byte read;
			FWC.ReadTRXEEPROMByte(0x24F, out read);
			return read;
		}

		#endregion
        */
        #endregion

        #region PA

        #region PA Power

        public static bool CheckPAPower()
        {
            Random rand = new Random();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            float[][] pa_power_table = new float[(int)Band.LAST][];
            for (int i = 0; i < (int)Band.LAST; i++)
                pa_power_table[i] = new float[13];

            for (int i = 0; i < bands.Length; i++)
                pa_power_table[(int)bands[i]][0] = (float)rand.NextDouble();

            byte temp;
            WritePAPower(pa_power_table, out temp);

            float[][] pa_power_check = new float[(int)Band.LAST][];
            for (int i = 0; i < (int)Band.LAST; i++)
                pa_power_check[i] = new float[13];

            ReadPAPower(pa_power_check);

            for (int i = 0; i < bands.Length; i++)
            {
                if (pa_power_table[(int)bands[i]][0] != pa_power_check[(int)bands[i]][0])
                    return false;
            }
            return true;
        }

        public static void WritePAPower(float[][] table, out byte checksum)
        {
            WriteCalDateTime();
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x3040;
            byte[] buf = new byte[32];
            int length = 0;
            
            for(int i=0; i<bands.Length; i++)
            {
                short val = (short)(Math.Round(table[(int)bands[i]][0], 4)*10000);
                    BitConverter.GetBytes(val).CopyTo(buf, length);
                    length += 2;
            }

            if (length > 0)
            {
                if (!CheckedWrite(addr, buf, (byte)length))
                {
                    MessageBox.Show("Error writing PA Power value to EEPROM.");
                    checksum = 0xFF;
                    return;
                }
            }

            // calculate and write checksum
            byte sum = Checksum.Calc(table);
            WritePAPowerChecksum(sum);
            checksum = sum;
        }

        public static void WritePAPowerChecksum(byte sum)
        {
            if (!CheckedWrite(0x304F, sum))
                MessageBox.Show("Error writing PA Power checksum to EEPROM.");
        }

        public static void ReadPAPower(float[][] table)
        {
            Band[] bands = { Band.B160M, Band.B80M, Band.B60M, Band.B40M, Band.B30M, Band.B20M,
							   Band.B17M, Band.B15M, Band.B12M, Band.B10M, Band.B6M };

            ushort addr = 0x3040;
            byte[] buf = new byte[32];
            USBHID.ReadEEPROM(addr, 22, out buf);
            int index = 0;

            if (buf[0] == 0xFF && buf[1] == 0xFF) return;

            for (int i = 0; i < bands.Length; i++)
            {
                short val = 0;
                val = BitConverter.ToInt16(buf, index);
                index += 2;

                table[(int)bands[i]][0] = (float)(val / 10000.0);
            }
        }

        public static byte ReadPAPowerChecksum()
        {
            byte read;
            USBHID.ReadTRXEEPROMByte(0x304F, out read);
            return read;
        }

        #endregion

        #endregion

    }
}