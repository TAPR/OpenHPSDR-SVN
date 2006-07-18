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
            Console.WriteLine("Getting structure sizes...");
            libUSB_Interface.usb_bus usb_bus = new libUSB_Interface.usb_bus();
            Console.WriteLine("Sizeof usb_bus_struct: " + usb_bus.Size());
            libUSB_Interface.usb_device usb_device = new libUSB_Interface.usb_device();
            Console.WriteLine("Sizeof usb_device_struct: " + usb_device.Size());
            libUSB_Interface.usb_config_descriptor usb_conf_descr = new libUSB_Interface.usb_config_descriptor();
            Console.WriteLine("Sizeof usb_config_desriptior_struct: " + usb_conf_descr.Size());
            libUSB_Interface.usb_device_descriptor usb_dev_descr = new libUSB_Interface.usb_device_descriptor();
            Console.WriteLine("Sizeof usb_device_desriptior_struct: " + usb_dev_descr.Size());
            libUSB_Interface.usb_interface usb_interface = new libUSB_Interface.usb_interface();
            Console.WriteLine("Sizeof usb_interface_struct: " + usb_interface.Size());
            libUSB_Interface.usb_interface_descriptor usb_interface_descr = new libUSB_Interface.usb_interface_descriptor();
            Console.WriteLine("Sizeof usb_interface_descriptor_struct: " + usb_interface_descr.Size());
            libUSB_Interface.usb_endpoint_descriptor usb_endpoint_descr = new libUSB_Interface.usb_endpoint_descriptor();
            Console.WriteLine("Sizeof usb_endpoint_descriptor_struct: " + usb_endpoint_descr.Size());

            Console.WriteLine("");
            Console.WriteLine("Initializing libUSB");
            libUSB_Interface.usb_init();
            Console.WriteLine("finding busses...");
            libUSB_Interface.usb_find_busses();
            Console.WriteLine("finding devices...");
            libUSB_Interface.usb_find_devices();
            Console.WriteLine("usb_get_busses...");
            libUSB_Interface.usb_bus bus = libUSB_Interface.usb_get_busses();
            Console.WriteLine("bus location: " + bus.location.ToString());

            USB usb = new USB();
            
            Console.WriteLine("Checking for VID=0xFFFE, PID=0x0007...");
            libUSB_Interface.usb_device fdev = usb.FindDevice(bus, 0xFFFE, 0x0007);
            if (fdev != null)
                Console.WriteLine("found VID PID...");
            else
                Console.WriteLine("did not find VID PID...");

            Console.WriteLine("checking next bus...");

            libUSB_Interface.usb_bus next_bus = bus.NextBus;
            if (next_bus != null)
                Console.WriteLine(bus.dirname);
            else
                Console.WriteLine("oops no next bus!...");

            libUSB_Interface.usb_device dev = bus.GetDevices();
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
                Console.WriteLine("bLength: " + dev.descriptor.bLength.ToString());
                Console.WriteLine("bDescriptorType: " + dev.descriptor.bDescriptorType.ToString());
                Console.WriteLine("bcdUSB: " + dev.descriptor.bcdUSB.ToString("x") + "h");
                Console.WriteLine("bDeviceClass: " + dev.descriptor.bDeviceClass.ToString());
                Console.WriteLine("bDeviceSubClass: " + dev.descriptor.bDeviceSubClass.ToString());
                Console.WriteLine("bDeviceProtocol: " + dev.descriptor.bDeviceProtocol.ToString());
                Console.WriteLine("bMaxPacketSize0: " + dev.descriptor.bMaxPacketSize0.ToString());
                Console.WriteLine("idVendor: " + "0x" + dev.descriptor.idVendor.ToString("x") + "h");
                Console.WriteLine("idProduct: " + "0x" + dev.descriptor.idProduct.ToString("x") + "h");
                Console.WriteLine("bcdDevice: " + "0x" + dev.descriptor.bcdDevice.ToString("x") + "h");
                Console.WriteLine("iManufacturer: " + "0x" + dev.descriptor.iManufacturer.ToString("x") + "h");
                Console.WriteLine("iProduct: " + dev.descriptor.iProduct.ToString());
                Console.WriteLine("iSerialNumber: " + dev.descriptor.iSerialNumber.ToString());
                Console.WriteLine("bNumConfigurations: " +  dev.descriptor.bNumConfigurations.ToString());
                
                StringBuilder buf = new StringBuilder(32);
                int ret;
                if (dev.descriptor.iManufacturer != 0)
                {
                    ret = libUSB_Interface.usb_get_string_simple(usb_dev_handle, dev.descriptor.iManufacturer, buf);
                    if (ret > 0)
                        Console.WriteLine("Manufacturer String: " + buf);
                }
                StringBuilder buf2 = new StringBuilder(32);
                if (dev.descriptor.iProduct != 0)
                {
                    ret = libUSB_Interface.usb_get_string_simple(usb_dev_handle, dev.descriptor.iProduct, buf2);
                    if (ret > 0)
                        Console.WriteLine("Product String: " + buf2);
                }
                StringBuilder buf3 = new StringBuilder(32);
                if (dev.descriptor.iSerialNumber != 0)
                {
                    ret = libUSB_Interface.usb_get_string_simple(usb_dev_handle, dev.descriptor.iSerialNumber, buf3);
                    if (ret > 0)
                        Console.WriteLine("S/N String: " + buf3);
                }
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
                for (int i = 0; i < dev.descriptor.bNumConfigurations; i++)
                {
                    libUSB_Interface.usb_config_descriptor config_descr = dev.GetConfig(i);
                    Console.WriteLine("wTotalLength: " + config_descr.wTotalLength.ToString());
                    Console.WriteLine("bNumInterfaces: " + config_descr.bNumInterfaces.ToString());
                    Console.WriteLine("bConfigurationValue: " + config_descr.bConfigurationValue.ToString());
                    Console.WriteLine("iConfiguration: " + config_descr.iConfiguration.ToString());
                    Console.WriteLine("bmAttributes: " + "0x" + config_descr.bmAttributes.ToString("x") + "h");
                    Console.WriteLine("MaxPower: " + config_descr.MaxPower.ToString());

                    Console.WriteLine("");

                    for (int q = 0; q < config_descr.bNumInterfaces; q++)
                    {
                        libUSB_Interface.usb_interface uinterface = config_descr.GetInterface(q);

                        Console.WriteLine("num_altsetting: " + uinterface.num_altsetting.ToString());

                        for (int j = 0; j < uinterface.num_altsetting; j++)
                        {
                            libUSB_Interface.usb_interface_descriptor altsetting = uinterface.GetAltSetting(j);
                            Console.WriteLine("");
                            Console.WriteLine("*******Interface Descr*******");
                            Console.WriteLine("bLength: " + altsetting.bLength.ToString());
                            Console.WriteLine("bInterfaceNumber: " + altsetting.bInterfaceNumber.ToString());
                            Console.WriteLine("bAlternateSetting: " + altsetting.bAlternateSetting.ToString());
                            Console.WriteLine("bNumEndpoints: " + altsetting.bNumEndpoints.ToString());
                            Console.WriteLine("bInterfaceClass: " + altsetting.bInterfaceClass.ToString());
                            Console.WriteLine("bInterfaceSubClass: " + altsetting.bInterfaceSubClass.ToString());
                            Console.WriteLine("bInterfaceProtocol: " + altsetting.bInterfaceProtocol.ToString());
                            Console.WriteLine("iInterface: " + altsetting.iInterface.ToString());
                            Console.WriteLine("");

                            for (int h = 0; h < altsetting.bNumEndpoints; h++)
                            {
                                libUSB_Interface.usb_endpoint_descriptor endpoint_descr = altsetting.GetEndpoint(h);
                                Console.WriteLine("");
                                Console.WriteLine("-------EndPoint Descr---------");
                                Console.WriteLine("bEndpointAddress: " + endpoint_descr.bEndpointAddress.ToString("x"));
                                Console.WriteLine("bmAttributes: " + endpoint_descr.bmAttributes.ToString("x"));
                                Console.WriteLine("wMaxPacketSize: " + endpoint_descr.wMaxPacketSize.ToString());
                                Console.WriteLine("bInterval: " + endpoint_descr.bInterval.ToString());
                                Console.WriteLine("bRefresh: " + endpoint_descr.bRefresh.ToString());
                                Console.WriteLine("bSynchAddress: " + endpoint_descr.bSynchAddress.ToString());
                            }
                        }
                    }
                }
            }

            //int wrt_size = 200;
            //int rd_size = 200;

            //Console.WriteLine("Resetting CPU...Reset ON...");
            //Console.WriteLine("Result: " + usb.Reset_CPU(usb_dev_handle, true).ToString());
            
            //Console.WriteLine("Attempting to upload RAM at 0xE000...");
            //byte[] input_buf = new byte[wrt_size];
            //for (int i = 0; i < wrt_size; i++)
            //    input_buf[i] = (byte)(i);

            //int a = usb.Write_RAM(usb_dev_handle, 0xE000, input_buf);
            //if (a < 0)
            //    Console.WriteLine("Upload failed...");
            //else
            //    Console.WriteLine("Wrote: " + a.ToString() + " bytes to 0xE000...");

                       
            //Console.WriteLine("Attempting to dump RAM starting at 0xE000...");
            //byte[] read_buffer = new byte[rd_size];

            //int a1 = usb.Read_RAM(usb_dev_handle, 0xE000, ref read_buffer);
            //if (a1 < 0)
            //    Console.WriteLine("Download failed...");
            //else
            //    Console.WriteLine("Read: " + a1.ToString() + " bytes from 0xE000...");


            //for (int i = 0; i < read_buffer.Length; i++)
            //    Console.WriteLine(read_buffer[i].ToString());

            //Console.WriteLine("Trying to open intel hex file...");
            //Console.WriteLine("Result: " + usb.Upload_Firmware(usb_dev_handle, "c:\\testfw.hex").ToString());

            //Console.WriteLine("Taking CPU out of reset...");
            //Console.WriteLine("Result: " + usb.Reset_CPU(usb_dev_handle, false).ToString());

            //OZY.Set_LED(usb_dev_handle, 1, true);
            //System.Threading.Thread.Sleep(3000);
            //OZY.Set_LED(usb_dev_handle, 1, false);

            byte[] bbuf = new byte[64];
            if ((OZY.Read_I2C(usb_dev_handle, 0x51, ref bbuf)))
            {
                for (int i = 0; i < bbuf.Length; i++)
                    Console.Write(bbuf[i].ToString("x") + ":");
            }
            else
                Console.WriteLine("Failed I2C Read...");


            Console.WriteLine("");
            libUSB_Interface.usb_close(usb_dev_handle);
            Console.WriteLine("done...");
            Console.ReadKey();            
        }
    }
}
