// Philip Covington N8VB
//
// 08/03/2006
//
// Sends random data to EP2 and reads it back on EP6.
// If the data is not identical it will print and error and halt.
// Writes packet sizes of 1024 bytes.
// Use with EP2_LED.rbf dated 08/03/2006 file for FPGA
// Hit Ctrl-C to exit program while running...

// modified by Bill Tracey (kd5tfd) 8 Aug 2006 --
//  use 512 byte blocks 
//  dump contents of block failing compare 
//  set byte3 of buf to 0xaa 
//  set byte4 to frame sequence num 

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;

namespace SendEP2_LEDS
{
    class Program
    {

        static char[] hexits = { '0', '1', '2', '3', '4', '5', '6' , '7', 
                                 '8',  '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        static char[] getHex(byte b)
        {
            int lo = b & 0xf;
            int hi = (b >> 4) & 0xf;
            char[] c = new char[2];
            c[0] = hexits[hi];
            c[1] = hexits[lo];
            return c; 
        } 


        static void dumpBuf(byte[] buf)
        {
            for (int i = 0; i < buf.Length; i++)
            {
                char[] c; 
                c = getHex(buf[i]);
                Console.Write(c);
                if (((i + 1) & 3) == 0)
                {
                    Console.Write(" "); 
                } 
                if ( ((i+1) & 0xf) == 0 ) 
                {
                    Console.WriteLine(); 
                } 
            }
        }


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
            byte fnum = 0; 
            byte[] buf = new byte[512]; // EP@ is double buffered 512 x 2 = 1024
            System.Random rnd = new Random();

            // int dbggate = 0; 
            while (true)
            {
                // ++dbggate; 
                byte[] rbuf = new byte[512];
                buf[0] = (byte)DateTime.Now.Millisecond; // put timestamp in first byte sent
                buf[1] = 0x11;    // timestamp marker in high byte
                buf[2] = 0xaa; // marker 
                buf[3] = ++fnum; // frame number 
                for (int i = 4; i < buf.Length; i += 2)
                    {
                        buf[i] = (byte)rnd.Next(0, 255);  // write random numbers                      
                        buf[i + 1] = (byte)rnd.Next(0, 255);    
                    }
                // dumpBuf(buf); 
                // Console.WriteLine("WRITE: " + libUSB_Interface.usb_bulk_write(hdev, 0x02, buf, 1000) + " " + DateTime.UtcNow.ToBinary().ToString());
                
                int ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 5000);
                Console.WriteLine("READ:  " + ret + " " + DateTime.UtcNow.ToBinary().ToString());
                //if (ret > 0)
                //{
                //    for (int z = 0; z < rbuf.Length; z++)
                //        Console.Write(rbuf[z].ToString("X") + " : ");
                //}
                if (ret > 0)
                {
                    Console.WriteLine("Read:");
                    dumpBuf(rbuf); 
                    //if (rbuf.Length != buf.Length) // compare buffer lengths
                    //{
                    //    Console.WriteLine("Length Error!");
                    //    break;  // end if error
                    //}
                    //else
                    //{
                    //   // test code 
                    //   // if (dbggate == 100)
                    //   // {
                    //   //    Console.WriteLine("forced error"); 
                    //   //     dbggate = 0;
                    //   //     rbuf[5] = 0x77; 
                    //   // }                         

                    //    for (int i = 0; i < rbuf.Length; i++)
                    //    {
                    //        if (rbuf[i] != buf[i]) // compare buffer contents
                    //        {
                    //            Console.WriteLine("Compare Error at: " + i);
                    //            Console.WriteLine("Written:");
                    //            dumpBuf(buf);
                    //            Console.WriteLine("Read:");
                    //            dumpBuf(rbuf); 
                    //            break; // end if error
                    //          }
                    //    }
                    //}
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
