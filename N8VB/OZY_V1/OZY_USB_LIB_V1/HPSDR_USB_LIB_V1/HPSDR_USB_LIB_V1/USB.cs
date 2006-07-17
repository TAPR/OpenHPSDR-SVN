using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HPSDR_USB_LIB_V1
{
    public class USB
    {
        public const byte MAX_DEVICES = 255;
        public const byte MAX_EP0_PACKET_SIZE = 64;

        public libUSB_Interface.usb_device FindDevice(libUSB_Interface.usb_bus bus, int VID, int PID, int DID, int SN)
        {
            libUSB_Interface.usb_device dev = bus.GetDevices();
            for (int i = 0; i < MAX_DEVICES; i++)
            {
                if ((dev != null) && (dev.descriptor.idVendor == VID))
                {
                    if (dev.descriptor.idProduct == PID)
                    {
                        if (dev.descriptor.iProduct == DID)
                        {
                            if (dev.descriptor.iSerialNumber == SN)
                                return dev;
                        }                            
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
            int length;
            int address;
            int type;
            byte checksum, a;
            uint b;
            byte[] data = new byte[256];

            StreamReader reader = new StreamReader(filename, Encoding.ASCII);
            
            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine();

                if (str.Substring(0, 1) != ":")
                    return -1;
                else
                {
                    length = Convert.ToInt32(str.Substring(1, 2));
                    address = Convert.ToInt32(str.Substring(3, 4));
                    type = Convert.ToInt32(str.Substring(7, 2));

                    switch (type)
                    {
                        case 0: // record
                            a = (byte)(length + (address & 0xff) + (address >> 8 + type));
                            for (int i = 0; i < length; i++)
                            {
                                b = Convert.ToUInt32(str.Substring(9 + i * 2, 2));
                                data[i] = (byte)b;
                                a += data[i];
                            }
                            b = Convert.ToUInt32(str.Substring(9 + length * 2, 2));
                            checksum = (byte)b;
                            if (((a + checksum) & 0xff) != 0x00)
                                return -2;
                            else
                            {
                                int r = Write_RAM(hdev, address, data);
                                if (r < 1)
                                    return -1;
                            }
                            break;
                        case 1: //EOF
                            return 1;
                        default: // Invalid
                            return -1;
                    }                    
                }                
            }
            return 1;
        }

    }
}
