using System;
using System.Collections.Generic;
using System.Text;

namespace HPSDR_USB_LIB_V1
{
    public static class OZY
    {
        public const int MAX_EP0_PACKETSIZE = 64;

        /* OZY VID PID */
        public const int OZY_VID = 0xFFFE;
        public const int OZY_PID = 0x0007;

        /* Vendor Request Types */
        public const int VENDOR_REQ_TYPE_IN = 0xc0;
        public const int VENDOR_REQ_TYPE_OUT = 0x40;

        /* Vendor In Commands */
        public const int VENDOR_REQ_I2C_READ = 0x81;    // wValueL: i2c address; length: how much to read
        public const int VENDOR_REQ_SPI_READ = 0x82;    // wValue: optional header bytes
                                                        // wIndexH:	enables
                                                        // wIndexL:	format
                                                        // len: how much to read

        /* Vendor Out Commands */
        public const int VENDOR_REQ_SET_LED = 0x01;     // wValueL off/on {0,1}; wIndexL: which {0,1}
        public const int VENDOR_REQ_FPGA_LOAD = 0x02;
        public const int FL_BEGIN = 0;	                // wIndexL: begin fpga programming cycle.  stalls if trouble.
        public const int FL_XFER = 1;	                // wIndexL: xfer up to 64 bytes of data
        public const int FL_END = 2;	                // wIndexL: end programming cycle, check for success.
									                    // stalls endpoint if trouble.

        public const int VENDOR_REQ_I2C_WRITE = 0x08;	// wValueL: i2c address; data: data

        public const int VENDOR_REQ_SPI_WRITE = 0x09;	// wValue: optional header bytes
							                            // wIndexH:	enables
							                            // wIndexL:	format
							                            // len: how much to write

        static public bool Read_EEPROM(IntPtr hdev, int i2c_addr, int offset, ref byte[] buffer)
        {
            // We have to set up the EEPROM from random read by writing
            // two bytes to it first to set the read address.  
            // See 24LC128 datasheet for more info.
            // This would need to be changed if using a smaller EEPROM that only 
            // requires a 1 byte address.

            byte[] cmd = new byte[2];
            cmd[0] = 0; // < -- this is the high byte of the address in EEPROM
            cmd[1] = (byte)offset; // < -- this is the low byte of the address in EEPROM
            if (Write_I2C(hdev, i2c_addr, cmd))
            {
                return Read_I2C(hdev, i2c_addr, ref buffer);                
            }
            else
                return false;
            
        }

        static public bool Write_EEPROM(IntPtr hdev, int i2c_addr, int offset, byte[] buffer)
        {
            // Each write to EEPROM consists of two address bytes followed by the value to 
            // be written.  The first byte is the high byte of the address, the second byte
            // is the low byte of the address, the third byte is the value to be written.

            byte[] cmd = new byte[3];
            
            for (int i = 0; i < buffer.Length; i++)
            {
                cmd[0] = 0; // < -- this is the high byte of the address in EEPROM
                cmd[1] = (byte)offset++; // < -- this is the low byte of the address in EEPROM
                cmd[2] = buffer[i]; // <-- this is the value to be written
                if (!(Write_I2C(hdev, i2c_addr, cmd)))
                {
                    return false;
                }
            }
            return true;
        }

        static public bool Read_I2C(IntPtr hdev, int i2c_addr, ref byte[] buffer)
        {
            if (buffer.Length < 1 || buffer.Length > MAX_EP0_PACKETSIZE)
                return false;
            else
            {
                int ret = libUSB_Interface.usb_control_msg(
                    hdev,
                    VENDOR_REQ_TYPE_IN,
                    VENDOR_REQ_I2C_READ,
                    i2c_addr,
                    0,
                    buffer,
                    buffer.Length,
                    1000
                    );
                if (ret == buffer.Length)
                    return true;
                else
                    return false;
            }
        }

        static public bool Write_I2C(IntPtr hdev, int i2c_addr, byte[] buffer)
        {
            if (buffer.Length < 1 || buffer.Length > MAX_EP0_PACKETSIZE)
                return false;
            else
            {
                int ret = libUSB_Interface.usb_control_msg(
                    hdev,
                    VENDOR_REQ_TYPE_OUT,
                    VENDOR_REQ_I2C_WRITE,
                    i2c_addr,
                    0,
                    buffer,
                    buffer.Length,
                    1000
                    );
                if (ret == buffer.Length)
                    return true;
                else
                    return false;
            }
        }

        static public bool Set_LED(IntPtr hdev, int which, bool on)
        {
            if (on)
            {
                int ret = libUSB_Interface.usb_control_msg(
                    hdev,
                    VENDOR_REQ_TYPE_OUT,
                    VENDOR_REQ_SET_LED,
                    1,
                    which,
                    new byte[0],
                    0,
                    1000
                    );
                if (ret < 0)
                    return false;
                else
                    return true;
            }
            else
            {
                int ret = libUSB_Interface.usb_control_msg(
                    hdev,
                    VENDOR_REQ_TYPE_OUT,
                    VENDOR_REQ_SET_LED,
                    0,
                    which,
                    new byte[0],
                    0,
                    1000
                    );
                if (ret < 0)
                    return false;
                else
                    return true;
            }            
        }

    }
}
