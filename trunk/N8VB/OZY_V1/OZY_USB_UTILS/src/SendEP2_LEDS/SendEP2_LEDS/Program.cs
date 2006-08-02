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
            Console.ReadKey();
            Console.WriteLine("SC:" + libUSB_Interface.usb_set_configuration(hdev, 1));
            Console.WriteLine("CI:" + libUSB_Interface.usb_claim_interface(hdev, 0));
            Console.ReadKey();
            Console.WriteLine("SA:" + libUSB_Interface.usb_set_altinterface(hdev, 0));
            libUSB_Interface.usb_clear_halt(hdev, 0x02);
            Console.ReadKey();            
            byte[] buf = new byte[2];
            int flg = 256;
            while (flg > -1)
            {
                for (int i = 0; i < buf.Length; i++)
                    buf[i] = (byte)flg;
                Console.WriteLine("WRITE: " + libUSB_Interface.usb_bulk_write(hdev, 0x02, buf, 1000) + " " + DateTime.UtcNow.ToBinary().ToString());
                System.Threading.Thread.Sleep(50);                
                flg -= 1;                
            }
            libUSB_Interface.usb_release_interface(hdev, 0);
            libUSB_Interface.usb_close(hdev);
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
