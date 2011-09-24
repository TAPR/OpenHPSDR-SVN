/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY_USB_LIB_V1
 *
 * Copyright (C) 2006 Philip A. Covington, N8VB
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
using System.Collections;
using System.Text;
using System.IO;
using System.Globalization;

namespace HPSDR_USB_LIB_V1
{
    public class USB
    {
        public const byte MAX_DEVICES = 255;
        public const byte MAX_EP0_PACKET_SIZE = 64;

        static public libUSB_Interface.usb_device FindDevice(libUSB_Interface.usb_bus bus, int VID, int PID)
        {
            libUSB_Interface.usb_device dev = bus.GetDevices();
            for (int i = 0; i < MAX_DEVICES; i++)
            {
                if ((dev != null) && (dev.descriptor.idVendor == VID))
                {
                    if (dev.descriptor.idProduct == PID)
                    {
                        return dev;
                    }                            
                }
                else
                {
                    dev = dev.NextDevice;
                    continue;
                }
            }
            return null;
        }

        static public IntPtr InitFindAndOpenDevice(int VID, int PID)
        {
            try
            {
                libUSB_Interface.usb_init();
                Console.WriteLine("finding busses...");
                libUSB_Interface.usb_find_busses();
                Console.WriteLine("finding devices...");
                libUSB_Interface.usb_find_devices();
                Console.WriteLine("usb_get_busses...");
                libUSB_Interface.usb_bus bus = libUSB_Interface.usb_get_busses();
                Console.WriteLine("bus location: " + bus.location.ToString());
                libUSB_Interface.usb_device dev = bus.GetDevices();

                for (int i = 0; i < MAX_DEVICES; i++)
                {
                    if ((dev != null) && (dev.descriptor.idVendor == VID))
                    {
                        if (dev.descriptor.idProduct == PID)
                        {
                            Console.WriteLine("Found VID PID: " + VID.ToString("x") + " " + PID.ToString("x"));
                            return libUSB_Interface.usb_open(dev);
                        }
                    }
                    else
                    {
                        Console.WriteLine("did not find VID PID: " + VID.ToString("x") + " " + PID.ToString("x"));
                        dev = dev.NextDevice;
                        continue;
                    }
                }
                return IntPtr.Zero;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return IntPtr.Zero;
            }
        }

        static public bool Reset_CPU(IntPtr hdev, bool reset)
        {
            byte[] write_buffer = new byte[1];
            
            if (reset)
                write_buffer[0] = 1;
            else
                write_buffer[0] = 0;

            if ((Write_RAM(hdev, 0xE600, write_buffer)) != 1)
                return false;
            else
                return true;
        }
                        
        static public int Write_RAM(IntPtr hdev, int start_addr, byte[] write_buffer)
        {
            int pkt_size = MAX_EP0_PACKET_SIZE;
            int len = write_buffer.Length;
            int bytes_written = 0;
                        
            for (int addr = start_addr; addr < start_addr + len; addr += pkt_size)
            {
                int nsize = len + start_addr - addr;
                if (nsize > pkt_size) nsize = pkt_size;
                byte[] packet_buf = new byte[pkt_size];
                Array.Copy(write_buffer, addr - start_addr, packet_buf, 0, nsize);

                int a = libUSB_Interface.usb_control_msg(hdev, 0x40, 0xa0, addr, 0, packet_buf, nsize, 1000);
                if (a < 0)
                    return a;
                else
                    bytes_written += a;
            }
            return bytes_written;
        }

        static public int Read_RAM(IntPtr hdev, int start_addr, ref byte[] read_buffer)
        {
            int pkt_size = MAX_EP0_PACKET_SIZE;
            int len = read_buffer.Length;
            int bytes_read = 0;
                        
            for (int addr = start_addr; addr < start_addr + len; addr += pkt_size)
            {
                int nsize = len + start_addr - addr;
                if (nsize > pkt_size) nsize = pkt_size;
                byte[] packet_buf = new byte[pkt_size];

                int a = libUSB_Interface.usb_control_msg(hdev, 0xc0, 0xa0, addr, 0, packet_buf, nsize, 1000);
                if (a < 0)
                    return a;
                else
                {
                    Array.Copy(packet_buf, 0, read_buffer, addr - start_addr, a);
                    bytes_read += a;
                }
            }
            return bytes_read;
        }

        static public bool Upload_Firmware(IntPtr hdev, string filename)
        {
            if (!(File.Exists(filename)))
            {
                Console.WriteLine(filename + " does not exist!");
                return false;
            }

            int linecounter = 0;
            StreamReader reader = new StreamReader(filename, Encoding.ASCII);

			// line below does not read to end of file -- apparently the base reader 
			// gets out in front of the StreamReader on top of it. (kd5tfd) 
            // while (reader.BaseStream.Position < reader.BaseStream.Length )
			while ( true ) 
            {
                string str = reader.ReadLine();
				if ( str == null ) break; // end of file - bail out 
                linecounter++;

                if (str.Substring(0, 1) != ":")
                    return false;
                else
                {
                    int length = int.Parse(str.Substring(1, 2), NumberStyles.HexNumber);
                    int address = int.Parse(str.Substring(3, 4), NumberStyles.HexNumber);
                    int type = int.Parse(str.Substring(7, 2), NumberStyles.HexNumber);

                    switch (type)
                    {
                        case 0: // record

                            byte[] data = new byte[256];
                            uint b = 0;

                            byte a = (byte)(length + (address & 0xff) + (address >> 8 + type));
                            for (int i = 0; i < length; i++)
                            {
                                b = uint.Parse(str.Substring(9 + i * 2, 2), NumberStyles.HexNumber);
                                data[i] = (byte)b;
                                a += data[i];
                            }
                            
                            b = uint.Parse(str.Substring(9 + length * 2, 2), NumberStyles.HexNumber);
                            byte checksum = (byte)b;
                            if (((a + checksum) & 0xff) != 0x00)
                                return false;
                            else
                            {                                
                                byte[] tmp_buf = new byte[length];
                                Buffer.BlockCopy(data, 0, tmp_buf, 0, length);
                                if ((Write_RAM(hdev, address, tmp_buf)) < 1)
                                    return false;
                                break;
                            }

                        case 1: //EOF
                            break;
                        default: //Invalid
                            return false;
                    }
                }
            }
            Console.WriteLine("Processed " + linecounter.ToString() + " lines...");
            return true;
        }

    }
}
