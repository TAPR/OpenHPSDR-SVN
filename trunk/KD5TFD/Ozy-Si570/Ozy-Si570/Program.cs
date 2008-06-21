/*
*
* Copyright (C) 2008 Bill Tracey, KD5TFD (bill@ewjt.com) 
*
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/


using System;
using System.Collections.Generic;
using System.Text;

using HPSDR_USB_LIB_V1;


namespace Ozy_Si570
{


    class Program

    
    {

        static readonly int OZY_VID = 0xfffe; 
        static readonly int OZY_PID = 0x7; 
        static readonly int SI570_ADDR = 0x55; 
        static readonly int[] HS_MAP =  { 4, 5, 6, 7, 0, 9, 0, 11 } ; 
        static readonly double TWO_POWER28 = 268435456.0; 
        static readonly double FREQ_NOMINAL = 56.320; 
        static readonly double FOSC_MAX = 5670.0;
        static readonly double FOSC_MIN = 4850.0;


        static void Main(string[] args)
        {
            Console.WriteLine("Hello.");
            Console.WriteLine("arg count: "  + args.Length);
            for (int j = 0; j < args.Length; j++)
            {
                Console.WriteLine(args[j]); 
            } 
            

            double new_freq;

            if (args[0].Equals("RESET"))
            {
                new_freq = 0;
            }
            else
            {
                new_freq = (double)float.Parse(args[0]);
            }

            IntPtr usb_dev_handle = IntPtr.Zero;

            try
            {
                usb_dev_handle = USB.InitFindAndOpenDevice(OZY_VID, OZY_PID);
                Console.WriteLine("Device handle is: " + usb_dev_handle.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return;
            }



            do
            {


                byte[] bytes = new byte[2];



                if (new_freq == 0.0)
                {

                    bytes[0] = 135;  // reset/mem ctl reg 
                    bytes[1] = 0x80; // recall bit 
                    if (!( OZY.Write_I2C(usb_dev_handle, SI570_ADDR, bytes) ))
                    {
                        Console.WriteLine("Failed to write reset to device");
                        break;
                    }
                    Console.WriteLine("Device Reset!");
                    break; 
                }
                bytes[0] = 135;  // reset/mem ctl reg 
                bytes[1] = 0x1; // recall bit 
                if (!( OZY.Write_I2C(usb_dev_handle, SI570_ADDR, bytes) ))
                {
                    Console.WriteLine("Failed to read NVM to RAM!");
                    break;
                }


                bytes = new byte[1];
                bytes[0] = 0x7;  // want to start reading at addr 7; 

                if (!( OZY.Write_I2C(usb_dev_handle, SI570_ADDR, bytes) ))
                {
                    Console.WriteLine("Failed to write to I2C addr 0x55");
                    break;
                }

                bytes = new byte[6];   // need to read 6 bytes 
                bool rc;
                rc = OZY.Read_I2C(usb_dev_handle, SI570_ADDR, ref bytes);
                Console.WriteLine("Read_I2C returned: " + rc);
                for (int i = 0; i < bytes.Length; i++)
                {
                    Console.WriteLine("data[" + i + "]: " + bytes[i]);
                }

                int hs_div = (bytes[0] >> 5) & 7;
                int n1 = (bytes[0] & 0x1f) << 2; 
                n1 |= (bytes[1] >> 6)  & 3; 

                Console.WriteLine("HS_DIV divider: " + HS_MAP[hs_div] + " (" + hs_div + ")" ) ;   
                Console.WriteLine("N1: " + n1); 

                // see the si570 data sheet for info on this -- ref freq is 38 bits -- 10 bits whole number, 28 bits
                // fractional 
                int ref_freq = (bytes[1]  & 0x3f)   << 4; 
                ref_freq |= (bytes[2] >> 4) & 0xf;

                int ref_lo = (bytes[5] & 0xff); 
                ref_lo |= ( bytes[4] & 0xff ) << 8;
                ref_lo |= ( bytes[3] & 0xff ) << 16; 
                ref_lo |= ( bytes[2] & 0xf ) << 24; 


                Console.WriteLine("ref_whole: " + ref_freq + " reff_fractional: " + ref_lo); 
                double reff = ref_lo / TWO_POWER28;
                reff += ref_freq; 

                Console.WriteLine("Ref Freq: " + reff); 

                double fxtal = (FREQ_NOMINAL * (1+n1) * HS_MAP[hs_div])/reff; 


                Console.WriteLine("fxtal: " + fxtal); 


                
                Console.WriteLine("new_freq: " + new_freq); 
                double total_divide_max = FOSC_MAX / new_freq;
                double total_divide_min = FOSC_MIN / new_freq; 

                // given the new freq, find a HS * N1 we can live with 
                // between total_dive_min and total_divide_max 
                
                int min_divide = (int)Math.Ceiling(total_divide_min); 
                int max_divide = (int)Math.Floor(total_divide_max);


                int new_hs = -1; 
                int total_div = 0; 

                for (int i = min_divide; i <= max_divide; i++)
                {
                    total_div = i; 
                    new_hs = findGoodHS(total_div); 
                    if ( new_hs > 0 ) break;                     
                }

                if (new_hs <= 0)
                {
                    Console.WriteLine("could not find good HS divider!!\n"); 
                    break; 
                }

                int new_n1 = total_div / new_hs;
                Console.WriteLine("hs: " + new_hs + " n1: " + new_n1); 
                
                double f_osc = new_freq * new_n1 * new_hs; 

                double new_reff = f_osc / fxtal; 

                Console.WriteLine("f_osc: " + f_osc + " new_reff: " + new_reff); 

                // debug stuff 
                // new_reff = 43.3639223910868;
                // new_hs = 4; 
                // new_n1 = 22; 

                // calculate reffreq integer               
                Console.WriteLine("new_reff (float): " + new_reff); 
                uint new_reff_whole = (uint)Math.Floor(new_reff); 
                Console.WriteLine("new_reff_whole: " + new_reff_whole); 
                ulong reff_bytes = ((ulong)new_reff_whole) << 28;
                Console.WriteLine("reff bytes (whole): " + reff_bytes); 
                double new_reff_fraction = new_reff - (double)new_reff_whole;                
                Console.WriteLine("new_reff_fraction: " + new_reff_fraction); 
                uint fractional_bytes = (uint)Math.Truncate((double)new_reff_fraction * (double)TWO_POWER28);
                Console.WriteLine("fractional bytes: " + fractional_bytes); 
                reff_bytes |= fractional_bytes; 
                Console.WriteLine("reff bytes: " + reff_bytes); 

                

                byte[] new_regs = new byte[6]; 

                new_regs[5] = (byte)(reff_bytes & 0xff); 
                new_regs[4] = (byte)(( reff_bytes >> 8 ) & 0xff);
                new_regs[3] = (byte)(( reff_bytes >> 16 ) & 0xff);
                new_regs[2] = (byte)(( reff_bytes >> 24 ) & 0xff);

                new_regs[1] = (byte)(((ulong)( reff_bytes >> 32 )) & 0x3f);
                --new_n1; 
                new_regs[1] |= (byte)(( new_n1 & 3 ) << 6 );

                new_regs[0] = (byte)((new_n1 >> 2)  & 0x1f); 
                int new_hs_bits = -1;
                for (int j = 0; j < HS_MAP.Length; j++)
                {
                    if (HS_MAP[j] == new_hs)
                    {
                        new_hs_bits = j; 
                        break; 
                    }
                }
                if (new_hs_bits == -1)
                {
                    Console.WriteLine("Bad HS: " + new_hs); 
                    break; 
                }
                new_regs[0] |=  (byte)(new_hs_bits << 5); 

                for (int j = 0; j < new_regs.Length; j++)
                {
                    Console.WriteLine("newRegs[" + j + "]: " + new_regs[j]); 
                }

                bytes = new byte[2]; 
                bytes[0] = 137;  // freezo dco register 
                bytes[1] = 0x10; // assert freeze bit 
                if (!( OZY.Write_I2C(usb_dev_handle, SI570_ADDR, bytes) ))
                {
                    Console.WriteLine("Failed to Freeze DCO!");
                    break;
                }


                bytes = new byte[7]; 
                bytes[0] = 0x7; // starting addr 
                for ( int j = 0; j < 6; j++ )
                { 
                    bytes[1+j] = new_regs[j]; 
                }
                if (!( OZY.Write_I2C(usb_dev_handle, SI570_ADDR, bytes) ))
                {
                    Console.WriteLine("Failed to Write new regs!");
                    break;
                }



                bytes = new byte[2];
                bytes[0] = 137;  // freezo dco register 
                bytes[1] = 0x0; // deassert freeze bit 
                if (!( OZY.Write_I2C(usb_dev_handle, SI570_ADDR, bytes) ))
                {
                    Console.WriteLine("Failed to defrost DCO!");
                    break;
                }

                bytes[0] = 135;  // new freq reg 
                bytes[1] = 0x40; // deassert freeze bit 
                if (!( OZY.Write_I2C(usb_dev_handle, SI570_ADDR, bytes) ))
                {
                    Console.WriteLine("Failed to assert new freq!");
                    break;
                }




            } while (false); 

            Console.WriteLine("Closing USB device...");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");


        }

        // is there a good HS value for this total_div ratio
        // hs need to be 4, 5, 6, 7, 9, 11 
        private static readonly int[] HS_VALUES = { 11, 9, 7, 6, 5, 4 } ; 
        private static int findGoodHS(int total_div)
        {
            for (int j = 0; j < HS_VALUES.Length; j++)
            {
                if (total_div % HS_VALUES[j] == 0)
                {
                    int res = total_div / HS_VALUES[j]; 
                    if ( res == 1 ) return HS_VALUES[j]; 
                    if (( res & 1 ) == 0 ) return HS_VALUES[j]; 
                }
            }
            return -1; 
        } 
    }
}
