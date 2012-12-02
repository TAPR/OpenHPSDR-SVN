// Phil Harman  VK6APH
//
// 22 Aug 2009 
//
// This program prompts for a user to enter a frequency
// then converts it to a format suitable for loading 
// into the N counter of an ADF4112 and sends it to 
// EP2 in an FX2. 
// Hit Ctrl-C to exit program while running...

using System;
using System.Collections.Generic;
using System.Text;
using HPSDR_USB_LIB_V1;

namespace PLL_Test
{
    class Program
    {
        static void Main(string[] args)
        {

            byte[] buf = new byte[2]; // holds frequency to send to ADF4112, 16 bits
            const ushort IF_FREQUENCY = 1030; // IF is 1030MHz 

            while (true)
            {
                // prompt user to enter centre  frequency in MHz
                Console.WriteLine("Enter centre  freqency (MHz) 0 - 1000MHz");
                string frequency = Console.ReadLine();

                Console.WriteLine("Frequency = {0}" + " MHz", frequency);

                // convert frequency string to integer 
                ushort freq = ushort.Parse(frequency);
                if (freq > 1000)
                {
                    Console.WriteLine("\n \nError - Frequency must be < 1000MHz \nPress Enter to Exit");
                    Console.ReadLine();
                    break;
                }

                // add IF offset of 1030MHz
                freq = (ushort)(freq + IF_FREQUENCY);

                Console.WriteLine("PLL_Frequency {0}", freq);

                // Convert endian before we send to Ozy
                
                buf[1] = (byte)freq; // gets the low byte
                freq = (ushort)(freq >> 8);
                buf[0] = (byte)freq; // gets the high byte
                
                Console.WriteLine("buf[0] = {0} buf[1] = {1}", buf[0], buf[1]);


                // Open USB device 
                Console.WriteLine("Opening Device...");
                IntPtr hdev = USB.InitFindAndOpenDevice(0xfffe, 0x0007);
                if (hdev == IntPtr.Zero)
                {
                    Console.WriteLine("\n \nCan't find Ozy board - is it connected, powered & loaded? \nPress Enter to Exit");
                    Console.ReadLine();
                    break;
                    
                }
                Console.Write("SC:" + libUSB_Interface.usb_set_configuration(hdev, 1));
                Console.Write(" CI:" + libUSB_Interface.usb_claim_interface(hdev, 0));
                Console.WriteLine(" SA:" + libUSB_Interface.usb_set_altinterface(hdev, 0));
                libUSB_Interface.usb_clear_halt(hdev, 0x02);
                libUSB_Interface.usb_clear_halt(hdev, 0x86);
                

                // send data to USB device 
                Console.WriteLine("WRITE: " + libUSB_Interface.usb_bulk_write(hdev, 0x02, buf, 1000)); // + " " + DateTime.UtcNow.ToBinary().ToString());
                
                // close USB device 
                libUSB_Interface.usb_release_interface(hdev, 0);
                libUSB_Interface.usb_close(hdev);
                Console.WriteLine(libUSB_Interface.usb_strerror());
            }
        }

    }
}

