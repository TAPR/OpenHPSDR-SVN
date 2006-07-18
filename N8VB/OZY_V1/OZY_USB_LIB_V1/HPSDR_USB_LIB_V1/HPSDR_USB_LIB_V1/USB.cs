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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace HPSDR_USB_LIB_V1
{
    public class USB
    {
        public const byte MAX_DEVICES = 255;
        public const byte MAX_EP0_PACKET_SIZE = 64;

        public libUSB_Interface.usb_device FindDevice(libUSB_Interface.usb_bus bus, int VID, int PID)
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

        public int Reset_CPU(IntPtr hdev, bool reset)
        {
            byte[] write_buffer = new byte[1];
            
            if (reset)
                write_buffer[0] = 1;
            else
                write_buffer[0] = 0;

            return Write_RAM(hdev, 0xE600, write_buffer);
        }
                        
        public int Write_RAM(IntPtr hdev, int start_addr, byte[] write_buffer)
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

        public int Read_RAM(IntPtr hdev, int start_addr, ref byte[] read_buffer)
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

        public int Upload_Firmware(IntPtr hdev, string filename)
        {
            int linecounter = 0;
            StreamReader reader = new StreamReader(filename, Encoding.ASCII);

            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine();
                linecounter++;

                if (str.Substring(0, 1) != ":")
                    return -1;
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
                                return -2;
                            else
                            {                                
                                byte[] tmp_buf = new byte[length];
                                Buffer.BlockCopy(data, 0, tmp_buf, 0, length);
                                if ((Write_RAM(hdev, address, tmp_buf)) < 1)
                                    return -1;
                                break;
                            }

                        case 1: //EOF
                            break;
                        default: // Invalid
                            return -1;
                    }
                }
            }
            Console.WriteLine("Processed " + linecounter.ToString() + " lines...");
            return 1;
        }

    }
}
