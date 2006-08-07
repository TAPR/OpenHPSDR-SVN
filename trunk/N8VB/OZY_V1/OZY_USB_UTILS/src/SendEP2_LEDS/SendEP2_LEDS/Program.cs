// Philip Covington N8VB
//
// 08/03/2006
//
// Sends random data to EP2 and reads it back on EP6.
// If the data is not identical it will print and error and halt.
// Writes packet sizes of 1024 bytes.
// Use with EP2_LED.rbf dated 08/03/2006 file for FPGA
// Hit Ctrl-C to exit program while running...

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;

namespace SendEP2_LEDS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Opening Device...");
            IntPtr hdev = USB.InitFindAndOpenDevice(0xfffe, 0x0007);
            Console.WriteLine("Press return...");
            Console.ReadKey();
            Console.WriteLine("SC:" + libUSB_Interface.usb_set_configuration(hdev, 1));
            Console.WriteLine("CI:" + libUSB_Interface.usb_claim_interface(hdev, 0));
            Console.WriteLine("Press return...");
            Console.ReadKey();
            Console.WriteLine("SA:" + libUSB_Interface.usb_set_altinterface(hdev, 0));
            libUSB_Interface.usb_clear_halt(hdev, 0x02);
            libUSB_Interface.usb_clear_halt(hdev, 0x86);
            Console.WriteLine("Press return...");
            Console.ReadKey();            
            byte[] buf = new byte[1024]; // EP@ is double buffered 512 x 2 = 1024
            System.Random rnd = new Random();
            
            while (true)
            {
                byte[] rbuf = new byte[1024];
                buf[0] = (byte)DateTime.Now.Millisecond; // put timestamp in first byte sent
                buf[1] = 0x11;    // timestamp marker in high byte
                for (int i = 2; i < buf.Length; i += 2)
                    {
                        buf[i] = (byte)rnd.Next(0, 255);  // write random numbers                      
                        buf[i + 1] = (byte)rnd.Next(0, 255);    
                    }
                Console.WriteLine("WRITE: " + libUSB_Interface.usb_bulk_write(hdev, 0x02, buf, 1000) + " " + DateTime.UtcNow.ToBinary().ToString());
                
                int ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 5000);
                Console.WriteLine("READ:  " + ret + " " + DateTime.UtcNow.ToBinary().ToString());
                //if (ret > 0)
                //{
                //    for (int z = 0; z < rbuf.Length; z++)
                //        Console.Write(rbuf[z].ToString("X") + " : ");
                //}
                if (ret > 0)
                {
                    if (rbuf.Length != buf.Length) // compare buffer lengths
                    {
                        Console.WriteLine("Length Error!");
                        break;  // end if error
                    }
                    else
                    {
                        for (int i = 0; i < rbuf.Length; i++)
                        {
                            if (rbuf[i] != buf[i]) // compare buffer contents
                            {
                                Console.WriteLine("Compare Error at: " + i);
                                break; // end if error
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(50); // sleep a bit so we can at least see LEDS flashing...
                //if (Console.ReadKey().Key != ConsoleKey.F1)
                //    break;
            }
            libUSB_Interface.usb_release_interface(hdev, 0);
            libUSB_Interface.usb_close(hdev);
            Console.WriteLine(libUSB_Interface.usb_strerror());
            Console.WriteLine("Error stop at time: " + DateTime.Now);
            Console.WriteLine("Press return...");
            Console.ReadKey();
        }
    }
}
