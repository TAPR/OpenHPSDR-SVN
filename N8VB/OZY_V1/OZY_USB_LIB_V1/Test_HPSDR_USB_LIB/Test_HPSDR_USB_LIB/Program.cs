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
            if (dev == null)
            {
                Console.WriteLine("Dev is null, done...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("device filename: " + dev.filename);
            Console.WriteLine("Trying to open device...");
            
            IntPtr usb_dev_handle = libUSB_Interface.usb_open(dev);
            Console.WriteLine("Device handle is: " + usb_dev_handle.ToString());
            Console.WriteLine(""); 
            if (usb_dev_handle != IntPtr.Zero)
            {
                Console.WriteLine("iManufacturer: " + "0x" + dev.descriptor.iManufacturer.ToString("x") + "h");
                Console.WriteLine("idVendor: " + "0x" + dev.descriptor.idVendor.ToString("x") + "h");
                Console.WriteLine("idProduct: " + "0x" + dev.descriptor.idProduct.ToString("x") + "h");
                Console.WriteLine("bNumConfigurations: " +  dev.descriptor.bNumConfigurations.ToString());
                Console.WriteLine("bLength: " + dev.descriptor.bLength.ToString());
                Console.WriteLine("bDescriptorType: " + dev.descriptor.bDescriptorType.ToString());
                Console.WriteLine("bDeviceClass: " + dev.descriptor.bDeviceClass.ToString());
                Console.WriteLine("bDeviceProtocol: " + dev.descriptor.bDeviceProtocol.ToString());
                Console.WriteLine("bDeviceSubClass: " + dev.descriptor.bDeviceSubClass.ToString());
            }
            Console.WriteLine("");                       
            if (dev.config == IntPtr.Zero)
            {
                Console.WriteLine("Config was null, done...");
                Console.ReadKey();
                return;
            }
            else
            {
                libUSB_Interface.usb_config_descriptor config_descr = (libUSB_Interface.usb_config_descriptor)Marshal.PtrToStructure(dev.config, typeof(libUSB_Interface.usb_config_descriptor));
                for (int i = 0; i < dev.descriptor.bNumConfigurations; i++)
                {
                    Console.WriteLine("wTotalLength: " + config_descr.wTotalLength.ToString());
                    Console.WriteLine("bNumInterfaces: " + config_descr.bNumInterfaces.ToString());
                    Console.WriteLine("bConfigurationValue: " + config_descr.bConfigurationValue.ToString());
                    Console.WriteLine("iConfiguration: " + config_descr.iConfiguration.ToString());
                    Console.WriteLine("bmAttributes: " + "0x" + config_descr.bmAttributes.ToString("x") + "h");
                    Console.WriteLine("MaxPower: " + config_descr.MaxPower.ToString());

                    Console.WriteLine("");
                    libUSB_Interface.usb_interface uinterface = (libUSB_Interface.usb_interface)Marshal.PtrToStructure(config_descr.uinterface, typeof(libUSB_Interface.usb_interface));
                    Console.WriteLine("num_altsetting: " + uinterface.num_altsetting.ToString());
                    IntPtr ptr_to_altsetting;
                    for (int j = 0; j < uinterface.num_altsetting; j++)
                    {
                        int foo = (int)uinterface.altsetting;
                        foo = foo + j * 4;
                        ptr_to_altsetting = (IntPtr)foo;
                        libUSB_Interface.usb_interface_descriptor altsetting = (libUSB_Interface.usb_interface_descriptor)Marshal.PtrToStructure(ptr_to_altsetting, typeof(libUSB_Interface.usb_interface_descriptor));
                        Console.WriteLine("bLength: " + altsetting.bLength.ToString());
                        Console.WriteLine("bInterfaceNumber: " + altsetting.bInterfaceNumber.ToString());
                        Console.WriteLine("bAlternateSetting: " + altsetting.bAlternateSetting.ToString());
                        Console.WriteLine("bNumEndpoints: " + altsetting.bNumEndpoints.ToString());
                        Console.WriteLine("bInterfaceClass: " + altsetting.bInterfaceClass.ToString());
                        Console.WriteLine("bInterfaceSubClass: " + altsetting.bInterfaceSubClass.ToString());
                        Console.WriteLine("bInterfaceProtocol: " + altsetting.bInterfaceProtocol.ToString());
                        Console.WriteLine("iInterface: " + altsetting.iInterface.ToString());
                    }
                }
            }

            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");
            Console.ReadKey();            
        }
    }
}
