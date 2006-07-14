using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using HPSDR_USB_LIB_V1;

namespace Test_HPSDR_USB_LIB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing libUSB");
            libUSB_Interface.usb_init();
            Console.WriteLine("finding busses...");
            libUSB_Interface.usb_find_busses();
            Console.WriteLine("finding devices...");
            libUSB_Interface.usb_find_devices();
            Console.WriteLine("usb_get_busses...");
            libUSB_Interface.usb_bus bus = libUSB_Interface.usb_get_busses();
            Console.WriteLine("bus location: " + bus.location.ToString());
            libUSB_Interface.usb_bus next_bus;
            Console.WriteLine("checking next bus...");
            if (bus.next != IntPtr.Zero)
                next_bus = (libUSB_Interface.usb_bus)Marshal.PtrToStructure(bus.next, typeof(libUSB_Interface.usb_bus));
            else
                Console.WriteLine("oops bus is null! No next bus...");
            libUSB_Interface.usb_device dev = (libUSB_Interface.usb_device)Marshal.PtrToStructure(bus.devices, typeof(libUSB_Interface.usb_device)); ;
            Console.WriteLine("device filename: " + dev.filename);
            Console.WriteLine("Trying to open device...");
            IntPtr usb_dev_handle = libUSB_Interface.usb_open(dev);
            Console.WriteLine("Device handle is: " + usb_dev_handle.ToString());
            if (usb_dev_handle != IntPtr.Zero)
                Console.WriteLine(dev.descriptor.iManufacturer.ToString());
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");
            Console.ReadKey();            
        }
    }
}
