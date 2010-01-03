using System;
using System.Collections;
using System.Text;
using System.IO;

namespace HPSDR_USB_LIB_V1
{
    public class OZY
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
        public const int VENDOR_REQ_RS232_READ = 0x83;
        public const int VENDOR_REQ_EEPROM_TYPE_READ = 0x84;
        public const int VENDOR_REQ_I2C_SPEED_READ = 0x85;

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

        public const int VENDOR_REQ_I2C_SPEED_SET = 0x0B; // wValueL 100kHz/400kHz {0,1}

        public const int VENDOR_REQ_CPU_SPEED_SET = 0x0C; // wValueL 100kHz/400kHz {0,1,3}

        static public bool Load_FPGA(IntPtr hdev, string filename)
        {
            if (!(File.Exists(filename)))
            {
                Console.WriteLine(filename + " does not exist!");
                return false;
            }

            // Turn on LED 1 to indicate FPGA programming has started
            Set_LED(hdev, 1, true);

            Console.WriteLine("TS:" + DateTime.Now);

            // start the load
            if ((libUSB_Interface.usb_control_msg
                (hdev, 
                VENDOR_REQ_TYPE_OUT, 
                VENDOR_REQ_FPGA_LOAD, 
                0, 
                FL_BEGIN, 
                new byte[0], 
                0,
                1000
                )) != 0)
            {
                Console.WriteLine("Failed at FL_BEGIN");
                return false;
            }

            Console.WriteLine("TS:" + DateTime.Now + " FL_BEGIN");
            // do the xfer
            // need to read the .rbf file here and just push it over usb in 64 byte chunks.
            FileStream fs = File.OpenRead(filename);
            BinaryReader br = new BinaryReader(fs);

            // start at beginning of file
            br.BaseStream.Seek(0, SeekOrigin.Begin);
                        
            byte[] rbf; // holds our <= 64 byte chunks to write
            int index = 0;

            while (br.BaseStream.Position < br.BaseStream.Length) // check if all bytes read
            {
                rbf = br.ReadBytes(MAX_EP0_PACKETSIZE); // will read up to 64 bytes
                index += rbf.Length;
                                
                if ((libUSB_Interface.usb_control_msg
                    (hdev,
                    VENDOR_REQ_TYPE_OUT,
                    VENDOR_REQ_FPGA_LOAD,
                    0,
                    FL_XFER,
                    rbf,
                    rbf.Length,
                    1000
                    )) != rbf.Length)
                {
                    fs.Close();
                    Console.WriteLine("Failed at FL_XFER");
                    Console.WriteLine(index + " bytes were transfered");
                    return false;
                }
            }
            Console.WriteLine(index + " bytes were transfered");
            fs.Close();

            Console.WriteLine("TS:" + DateTime.Now + " FL_XFER");

            // signal end of load
            if ((libUSB_Interface.usb_control_msg
                (hdev,
                VENDOR_REQ_TYPE_OUT,
                VENDOR_REQ_FPGA_LOAD,
                0,
                FL_END,
                new byte[0],
                0,
                1000
                )) != 0)
            {
                Console.WriteLine("Failed at FL_END");
                return false;
            }

            Console.WriteLine("TS:" + DateTime.Now + " FL_END");
            // turn off LED 1 to indicate successful FPGA Load
            Set_LED(hdev, 1, false);
            return true;
        }

        static public bool Load_FPGA_Fast(IntPtr hdev, string filename)
        {
            if (!(File.Exists(filename)))
            {
                Console.WriteLine(filename + " does not exist!");
                return false;
            }

            // Turn on LED 1 to indicate FPGA programming has started
            Set_LED(hdev, 1, true);

            Console.WriteLine("TS:" + DateTime.Now);

            // start the load
            if ((libUSB_Interface.usb_control_msg
                (hdev,
                VENDOR_REQ_TYPE_OUT,
                VENDOR_REQ_FPGA_LOAD,
                0,
                FL_BEGIN,
                new byte[0],
                0,
                1000
                )) != 0)
            {
                Console.WriteLine("Failed at FL_BEGIN");
                return false;
            }

            Console.WriteLine("TS:" + DateTime.Now + " FL_BEGIN");
            // do the xfer
            // need to read the .rbf file here and just push it over usb in 64 byte chunks.
            FileStream fs = File.OpenRead(filename);
            BinaryReader br = new BinaryReader(fs);

            // start at beginning of file
            br.BaseStream.Seek(0, SeekOrigin.Begin);

            byte[] rbf; // holds our <= 128 byte chunks to write
            int index = 0;

            while (br.BaseStream.Position < br.BaseStream.Length) // check if all bytes read
            {
                rbf = br.ReadBytes(128); // will read up to 128 bytes
                index += rbf.Length;

                if ((libUSB_Interface.usb_control_msg
                    (hdev,
                    VENDOR_REQ_TYPE_OUT,
                    VENDOR_REQ_FPGA_LOAD,
                    0,
                    FL_XFER,
                    rbf,
                    rbf.Length,
                    1000
                    )) != rbf.Length)
                {
                    fs.Close();
                    Console.WriteLine("Failed at FL_XFER");
                    Console.WriteLine(index + " bytes were transfered");
                    return false;
                }
            }
            Console.WriteLine(index + " bytes were transfered");
            fs.Close();

            Console.WriteLine("TS:" + DateTime.Now + " FL_XFER");

            // signal end of load
            if ((libUSB_Interface.usb_control_msg
                (hdev,
                VENDOR_REQ_TYPE_OUT,
                VENDOR_REQ_FPGA_LOAD,
                0,
                FL_END,
                new byte[0],
                0,
                1000
                )) != 0)
            {
                Console.WriteLine("Failed at FL_END");
                return false;
            }

            Console.WriteLine("TS:" + DateTime.Now + " FL_END");
            // turn off LED 1 to indicate successful FPGA Load
            Set_LED(hdev, 1, false);
            return true;
        }

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

        static public int Read_EEPROM_Type(IntPtr hdev)
        {
            byte[] ans = new byte[1];

            libUSB_Interface.usb_control_msg
                (
                hdev,
                VENDOR_REQ_TYPE_IN,
                VENDOR_REQ_EEPROM_TYPE_READ,
                0x00,
                0x00,
                ans,
                1,
                1000
                );
            return (int)ans[0];
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
                cmd[1] = (byte)offset; // < -- this is the low byte of the address in EEPROM
                cmd[2] = buffer[i]; // <-- this is the value to be written
                if (!(Write_I2C(hdev, i2c_addr, cmd)))
                {
                    Console.WriteLine("Programming failed at offset: " + offset);
                    return false;
                }
                System.Threading.Thread.Sleep(50);
                offset++;
            }
            return true;
        }

        static public int Read_I2C_Speed(IntPtr hdev)
        {
            byte[] ans = new byte[1];

            libUSB_Interface.usb_control_msg
                (
                hdev,
                VENDOR_REQ_TYPE_IN,
                VENDOR_REQ_I2C_SPEED_READ,
                0x00,
                0x00,
                ans,
                1,
                1000
                );
            return (int)ans[0];
        }

        static public bool Set_I2C_Speed(IntPtr hdev, int value)
        {
            if (value < 0 || value > 1)
            {
                Console.WriteLine("Value must be 0 or 1");
                return false;
            }
                        
            int ret = libUSB_Interface.usb_control_msg
                (
                hdev,
                VENDOR_REQ_TYPE_OUT,
                VENDOR_REQ_I2C_SPEED_SET,
                value,
                0x00,
                new byte[0],
                0,
                1000
                );
            if (ret < 0)
                return false;
            else
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
                if (ret > 0)
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

        static public bool Set_CPU_Speed(IntPtr hdev, int value)
        {
            if (value < 0 || value > 2)
            {
                Console.WriteLine("Value must be 0, 1, or 2");
                return false;
            }
                        
            int ret = libUSB_Interface.usb_control_msg
                (
                hdev,
                VENDOR_REQ_TYPE_OUT,
                VENDOR_REQ_CPU_SPEED_SET,
                value,
                0x00,
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
