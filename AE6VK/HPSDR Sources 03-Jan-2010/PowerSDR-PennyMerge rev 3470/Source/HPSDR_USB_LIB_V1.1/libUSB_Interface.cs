/*
 * HPSDR/OZY - High Performance Software Defined Radio, OZY_USB_LIB_V1
 *
 * Copyright (C) 2006 Philip A. Covington, N8VB
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace HPSDR_USB_LIB_V1
{
    public class libUSB_Interface
    {
        public const int USB_REQ_GET_STATUS = 0x00;
        public const int USB_REQ_CLEAR_FEATURE = 0x01;
        /* 0x02 is reserved */
        public const int USB_REQ_SET_FEATURE = 0x03;
        /* 0x04 is reserved */
        public const int USB_REQ_SET_ADDRESS = 0x05;
        public const int USB_REQ_GET_DESCRIPTOR = 0x06;
        public const int USB_REQ_SET_DESCRIPTOR = 0x07;
        public const int USB_REQ_GET_CONFIGURATION = 0x08;
        public const int USB_REQ_SET_CONFIGURATION = 0x09;
        public const int USB_REQ_GET_INTERFACE = 0x0A;
        public const int USB_REQ_SET_INTERFACE = 0x0B;
        public const int USB_REQ_SYNCH_FRAME = 0x0C;

        public const int USB_TYPE_STANDARD = (0x00 << 5);
        public const int USB_TYPE_CLASS = (0x01 << 5);
        public const int USB_TYPE_VENDOR = (0x02 << 5);
        public const int USB_TYPE_RESERVED = (0x03 << 5);

        public const int USB_RECIP_DEVICE = 0x00;
        public const int USB_RECIP_INTERFACE = 0x01;
        public const int USB_RECIP_ENDPOINT = 0x02;
        public const int USB_RECIP_OTHER = 0x03;

        public const byte USB_ENDPOINT_ADDRESS_MASK = 0x0f;
        public const byte USB_ENDPOINT_DIR_MASK = 0x80;

        public const byte USB_ENDPOINT_TYPE_MASK = 0x03;
        public const byte USB_ENDPOINT_TYPE_CONTROL = 0;
        public const byte USB_ENDPOINT_TYPE_ISOCHRONOUS = 1;
        public const byte USB_ENDPOINT_TYPE_BULK = 2;
        public const byte USB_ENDPOINT_TYPE_INTERRUPT = 3;
        /*
        * Various libusb API related stuff
        */

        public const int USB_ENDPOINT_IN = 0x80;
        public const int USB_ENDPOINT_OUT = 0x00;

        /* Error codes */
        public const int USB_ERROR_BEGIN = 500000;
        
        // FROM USB.H in libUSB

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_descriptor_header
        {
            public byte bLength;
            public byte bDescriptorType;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_descriptor_header));
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_string_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort wData0;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_string_descriptor));
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_hid_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort bcdHID;
            public byte bCountryCode;
            public byte bNumDescriptors;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_hid_descriptor));
            }
        }
        
        public const byte USB_MAXENDPOINTS = 32;
        // Marshal.Sizeof reports 17 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_endpoint_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public byte bEndpointAddress;
            public byte bmAttributes;
            public ushort wMaxPacketSize;
            public byte bInterval;
            public byte bRefresh;
            public byte bSynchAddress;
            public IntPtr extra;
            public int extralen;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_endpoint_descriptor));
            }
        }
        
        public const byte USB_MAXINTERFACES = 32;
        // Marshal.Sizeof reports 21 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_interface_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public byte bInterfaceNumber;
            public byte bAlternateSetting;
            public byte bNumEndpoints;
            public byte bInterfaceClass;
            public byte bInterfaceSubClass;
            public byte bInterfaceProtocol;
            public byte iInterface;

            public IntPtr endpoint;

            public IntPtr extra;	
            public int extralen;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_interface_descriptor));
            }

            public usb_endpoint_descriptor GetEndpoint(int number)
            {
                if (endpoint == IntPtr.Zero || number > USB_MAXENDPOINTS)
                {
                    return new usb_endpoint_descriptor();
                }
                IntPtr endpointPtr = new IntPtr(endpoint.ToInt64() + number * Marshal.SizeOf(typeof(usb_endpoint_descriptor)));
                return (usb_endpoint_descriptor)Marshal.PtrToStructure(endpointPtr, typeof(usb_endpoint_descriptor));
            }
        }

        public const byte USB_MAXALTSETTING = 128;	/* Hard limit */
        // Marshal.Sizeof reports 8 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_interface
        {
            public IntPtr altsetting;
            public Int32 num_altsetting;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_interface));
            }

            public usb_interface_descriptor GetAltSetting(int number)
            {
                if (altsetting == IntPtr.Zero || number > USB_MAXALTSETTING)
                {
                    return new usb_interface_descriptor();
                }
                IntPtr altPtr = new IntPtr(altsetting.ToInt64() + number * Marshal.SizeOf(typeof(usb_interface_descriptor)));
                return (usb_interface_descriptor)Marshal.PtrToStructure(altPtr, typeof(usb_interface_descriptor));
            }
        }

        public const byte USB_MAXCONFIG = 8;
        // Note Pack = 1 is needed here!
        // Marshal.Sizeof reports 21 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_config_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort wTotalLength;
            public byte bNumInterfaces;
            public byte bConfigurationValue;
            public byte iConfiguration;
            public byte bmAttributes;
            public byte MaxPower;

            public IntPtr uinterface;

            public IntPtr extra;	/* Extra descriptors */
            public int extralen;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_config_descriptor));
            }
            
            public usb_interface GetInterface(int number)
            {
                if (uinterface == IntPtr.Zero || number > USB_MAXINTERFACES)
                {
                    return new usb_interface();
                }
                IntPtr interfacePtr = new IntPtr(uinterface.ToInt64() + number * Marshal.SizeOf(typeof(usb_interface)));
                return (usb_interface)Marshal.PtrToStructure(interfacePtr, typeof(usb_interface));
            }
        }

        // Marshal.Sizeof reports 18 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_device_descriptor
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort bcdUSB;
            public byte bDeviceClass;
            public byte bDeviceSubClass;
            public byte bDeviceProtocol;
            public byte bMaxPacketSize0;
            public ushort idVendor;
            public ushort idProduct;
            public ushort bcdDevice;
            public byte iManufacturer;
            public byte iProduct;
            public byte iSerialNumber;
            public byte bNumConfigurations;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_device_descriptor));
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_ctrl_setup 
        {
            public byte bRequestType;
            public byte bRequest;
            public ushort wValue;
            public ushort wIndex;
            public ushort wLength;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_ctrl_setup));
            }
        }
                
        const int LIBUSB_PATH_MAX = 512;
        // Note Pack = 1 is needed to prevent a invalid memory access
        // Marshal.Sizeof reports 556 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class usb_device
        {
            public IntPtr next;
            public IntPtr prev;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LIBUSB_PATH_MAX)]
            public string filename;
                                    
            public IntPtr bus;
            
            public usb_device_descriptor descriptor;

            public IntPtr config;
                                      
            public IntPtr dev;		/* Darwin support */
            
            public byte devnum;
            
            public byte num_children;
            public IntPtr children;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_device));
            }

            public usb_device NextDevice
            {
                get
                {
                    if (next == IntPtr.Zero)
                    {
                        return null;
                    }
                    return (usb_device)Marshal.PtrToStructure(next, typeof(usb_device));
                }
            }

            public usb_device PrevDevice
            {
                get
                {
                    if (prev == IntPtr.Zero)
                    {
                        return null;
                    }
                    return (usb_device)Marshal.PtrToStructure(prev, typeof(usb_device));
                }
            }

            public usb_bus GetBus()
            {
                if (bus == IntPtr.Zero)
                    return null;
                else
                    return (usb_bus)Marshal.PtrToStructure(bus, typeof(usb_bus));                
            }


            public usb_config_descriptor GetConfig(int number)
            {
                if (config == IntPtr.Zero || number > USB_MAXCONFIG)
                {
                    return new usb_config_descriptor();
                }
                IntPtr configPtr = new IntPtr(config.ToInt64() + number * Marshal.SizeOf(typeof(usb_config_descriptor)));
                return (usb_config_descriptor)Marshal.PtrToStructure(configPtr, typeof(usb_config_descriptor));
            }
        }

        // Marshal.Sizeof reports 532 bytes
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class usb_bus 
        {
            public IntPtr next = IntPtr.Zero;
            public IntPtr prev = IntPtr.Zero;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LIBUSB_PATH_MAX)]
            public string dirname;

            public IntPtr devices = IntPtr.Zero;
            public uint location;

            public IntPtr root_dev = IntPtr.Zero;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_bus));
            }

            public usb_bus NextBus
            {
                get
                {
                    if (next == IntPtr.Zero)
                    {
                        return null;
                    }
                    return (usb_bus)Marshal.PtrToStructure(next, typeof(usb_bus));
                }
            }
            public usb_bus PrevBus
            {
                get
                {
                    if (prev == IntPtr.Zero)
                    {
                        return null;
                    }
                    return (usb_bus)Marshal.PtrToStructure(prev, typeof(usb_bus));
                }
            }

            public usb_device GetDevices()
            {
                if (devices == IntPtr.Zero)
                    return null;
                else
                    return (usb_device)Marshal.PtrToStructure(devices, typeof(usb_device));

            }
        }
                
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_dll_version
        {
            public int major;
            public int minor;
            public int micro;
            public int nano;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_dll_version));
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_driver_version
        {
            public int major;
            public int minor;
            public int micro;
            public int nano;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_driver_version));
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct usb_version
        {
            public IntPtr dll_ver;
            public IntPtr driver_ver;

            public int Size()
            {
                return Marshal.SizeOf(typeof(usb_version));
            }
        }

        const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
        const string LIBUSB_NATIVE_LIBRARY = "libusb0.dll";
        
        // IntPtr helper functions
        public static IntPtr AddPtr(IntPtr a, int b) 
        {
            return (IntPtr) ((long) a + b);
        }

        public static IntPtr AddPtr(IntPtr a, IntPtr b) 
        {
             return (IntPtr) ((long) a + (long)b);
        }

        public static IntPtr AddPtr(int a, IntPtr b) 
        {
             return (IntPtr) ((long) a + (long)b);
        }

        // usb.c
        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr usb_open(usb_device dev);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_close(IntPtr dev);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_get_string(IntPtr dev, int index, int langid, StringBuilder buf, int buflen);
        public static int usb_get_string(IntPtr dev, int index, int langid, StringBuilder buf)
        {
            return usb_get_string(dev, index, langid, buf, buf == null ? 0 : buf.Capacity);
        }

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_get_string_simple(IntPtr dev, int index, StringBuilder buf, int buflen);
        public static int usb_get_string_simple(IntPtr dev, int index, StringBuilder buf)
        {
            return usb_get_string_simple(dev, index, buf, buf == null ? 0 : buf.Capacity);
        }

        // descriptors.c 
        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_get_descriptor_by_endpoint(IntPtr udev, int ep, byte type, byte index, IntPtr buf, int size);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_get_descriptor(IntPtr udev, byte type, byte index, IntPtr buf, int size);

        // <arch>.c 
        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_bulk_write(IntPtr dev, int ep, byte[] bytes, int size, int timeout);
        public static int usb_bulk_write(IntPtr dev, int ep, byte[] bytes, int timeout)
        {
            return usb_bulk_write(dev, ep, bytes, bytes == null ? 0 : bytes.Length, timeout);
        }

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_bulk_read(IntPtr dev, int ep, byte[] bytes, int size, int timeout);
        public static int usb_bulk_read(IntPtr dev, int ep, byte[] bytes, int timeout)
        {
            return usb_bulk_read(dev, ep, bytes, bytes == null ? 0 : bytes.Length, timeout);
        }

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_interrupt_write(IntPtr dev, int ep, byte[] bytes, int size, int timeout);
        public static int usb_interrupt_write(IntPtr dev, int ep, byte[] bytes, int timeout)
        {
            return usb_interrupt_write(dev, ep, bytes, bytes == null ? 0 : bytes.Length, timeout);
        }

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_interrupt_read(IntPtr dev, int ep, byte[] bytes, int size, int timeout);
        public static int usb_interrupt_read(IntPtr dev, int ep, byte[] bytes, int timeout)
        {
            return usb_interrupt_read(dev, ep, bytes, bytes == null ? 0 : bytes.Length, timeout);
        }

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_control_msg(IntPtr dev, int requesttype, int request, int value, int index, byte[] bytes, int size, int timeout);
        public static int usb_control_msg(IntPtr dev, int requesttype, int request, int value, byte[] bytes, int timeout)
        {
            return usb_control_msg(dev, requesttype, request, value, 0, bytes, bytes == null ? 0 : bytes.Length, timeout);
        }

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_set_configuration(IntPtr dev, int configuration);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_claim_interface(IntPtr dev, int interfaceNum);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_release_interface(IntPtr dev, int interfaceNum);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_set_altinterface(IntPtr dev, int alternate);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_resetep(IntPtr dev, uint ep);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_clear_halt(IntPtr dev, uint ep);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_reset(IntPtr dev);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern string usb_strerror();

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern void usb_init();

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern void usb_set_debug(int level);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_find_busses();

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern int usb_find_devices();

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern usb_device usb_device_(IntPtr dev);

        [DllImport(LIBUSB_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, ExactSpelling = true), SuppressUnmanagedCodeSecurity]
        public static extern usb_bus usb_get_busses();
    }
}
