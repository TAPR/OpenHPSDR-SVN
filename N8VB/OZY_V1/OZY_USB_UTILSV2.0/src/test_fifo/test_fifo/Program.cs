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
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;

namespace test_fifo
{
    internal class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        private long startTime, stopTime;
        private long freq;

        // Constructor
        public HiPerfTimer()
        {
            startTime = 0;
            stopTime = 0;

            if (QueryPerformanceFrequency(out freq) == false)
            {
                // high-performance counter not supported
                throw new Win32Exception();
            }
        }

        // Start the timer
        public void Start()
        {
            // lets do the waiting threads their work
            Thread.Sleep(0);

            QueryPerformanceCounter(out startTime);
        }

        // Stop the timer
        public void Stop()
        {
            QueryPerformanceCounter(out stopTime);
        }

        // Returns the duration of the timer (in seconds)
        public double Duration
        {
            get
            {
                return (double)(stopTime - startTime) / (double)freq;
            }
        }
    }

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
            System.Random rnd = new Random();
            int size = 2048;
            byte[] rbuf = new byte[size];
            HiPerfTimer pt = new HiPerfTimer();


            //while (true)
            //{                
            //    int ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 5000);
            //    if (ret != rbuf.Length)
            //    {
            //        break;
            //    }                
            //    //System.Threading.Thread.Sleep(10); // sleep a bit so we can at least see LEDS flashing...
            //    //if (Console.ReadKey().Key != ConsoleKey.F1)
            //    //    break;
            //}

            //while (true)
            //{
                //pt.Start();
            int counter = 0;
                int ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 500);
                //if (ret != rbuf.Length)
                //    break;
                FileStream fs = File.Create(".\\dump.txt");
                StreamWriter sw = new StreamWriter(fs);
                for (int i = 0; i < rbuf.Length; i++)
                {
                    sw.Write(rbuf[i].ToString("X") + " : ");
                    Console.Write(rbuf[i].ToString("X") + " : ");
                    if (++counter >= 8)
                    {
                        counter = 0;
                        Console.WriteLine();
                        sw.WriteLine();
                    }
                }
                System.Threading.Thread.Sleep(100);
                sw.Close();
                fs.Close();                
                
                //pt.Stop();
            //}

            //Console.WriteLine("RET: " + ret + " Time: " + pt.Duration);
            
            //Console.WriteLine("MBYTES/SEC: " + ((ret / pt.Duration)/System.Math.Pow(2, 20)));

            libUSB_Interface.usb_release_interface(hdev, 0);
            libUSB_Interface.usb_close(hdev);
            Console.WriteLine(libUSB_Interface.usb_strerror());
            Console.WriteLine("Error stop at time: " + DateTime.Now);
            Console.WriteLine("Press return...");
            Console.ReadKey();
        }
    }
}
