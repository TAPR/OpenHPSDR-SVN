/*
 * OzyII  HPSDR Ethernet interface board - Test code 
 *
 * 
 * Copyright (C) 2009 Phil Harman, VK6APH
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
 * 
 * 
 */

/*
 * Change log 
 * 
 *  31 Aug  2009 - Started coding - send MAC frames  i.e. Ethernet Type II Frames
 *  
 * NOTE:  data to libUSB must be a multiple of 512 bytes, receive buffer the same.
 */




using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HPSDR_USB_LIB_V1;
using System.Threading;
using System.Diagnostics;   // use View > Output to see debug messages

namespace Test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        IntPtr hdev = IntPtr.Zero;                                  // USB Ozy device handle
        byte[] Ethernet_Preamble = new byte[8] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0xD5 };
        byte[] To_Ethernet_Address = new byte[4];
        byte[] From_Ethernet_Address = new byte[4];
        byte[] To_MAC_address = new byte[6];
        byte[] From_MAC_address = new byte[6];
        //byte[] UDP_payload = new byte[18] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11};
        //byte[] IP_header = new byte[9] { 0x45, 0x00, 0x00, 0x2E, 0xB3, 0xFE, 0x00, 0x00, 0x80 };
        byte[] Ethernet_type = new byte[2] {0xEF, 0xFE};           // Type is MAC, use 0x0800 for IP 
        byte[] CRC32 = new byte[4]{0x00, 0x01, 0x02, 0x03};        // CRC for MAC frames
        //byte IP_protocol = 0x11;                                 // Protocol is UDP
        //short IP_checksum = 0x0000;
        //short UDP_checksum = 0x0000;
        //short Ethernet_checksum = 0x0000;
        //short UDP_source_port = 1024;
        //short UDP_destination_port = 1024;



        private void Form1_Load(object sender, EventArgs e)
        {

            ReceiveData.ReadOnly = true; // receive window is receive only 
            
            // convert the Ethernet address we are sending to into a byte array
            string[] address = ToIP.Text.Split('.');
            int x = 0;
            foreach (string word in address)
            {
                Console.WriteLine(word);
                To_Ethernet_Address[x] = Convert.ToByte(word);
                x++;
            }

            // convert the Ethernet address we are receiving from into a byte array
            string[] address1 = FromIP.Text.Split('.');
            int i = 0;
            foreach (string word in address1)
            {
                Console.WriteLine(word);
                From_Ethernet_Address[i] = Convert.ToByte(word);
                i++;
            }

            // convert the MAC address we are sending to into a byte array
            string[] MAC = ToMAC.Text.Split(' ');
            int y = 0;
            foreach (string word in MAC)
            {
                Console.WriteLine(word);
                To_MAC_address[y] = (byte)Convert.ToInt16(word, 16);
                // convert back to string for later
                //string test = To_MAC_address[y].ToString("X");
                //Console.WriteLine(test);
                y++;
            }

            // convert the MAC address we are receiving from to into a byte array
            string[] MAC1 = FromMAC.Text.Split(' ');
            int j = 0;
            foreach (string word in MAC1)
            {
                Console.WriteLine(word);
                From_MAC_address[j] = (byte)Convert.ToInt16(word, 16);
                j++;
            }
            
            // Start the USB interface
            if (!start_USB())
            {
                MessageBox.Show("No Ozy board found  - Check HPSDR is connected and powered");
                return;
            }

            // Check that FX2 has been loaded with software, if not call initozy11.bat to load it
            string Ozy_version = getOzyFirmwareString(); // Get ozy firmware version string - 8 bytes,  returns null for error

            if (Ozy_version == null)
            {
                this.Cursor = Cursors.WaitCursor;  // show hour glass cursor whilst we load Ozy FPGA
                // call a process to run initozy11.bat in current directory
                ProcessStartInfo start_info = new ProcessStartInfo();
                start_info.FileName = "initozy11.bat";
                start_info.UseShellExecute = true;
                Process p = new Process();
                p.StartInfo = start_info;
                bool rc = p.Start();
                System.Console.WriteLine("start returned: " + rc);
                p.WaitForExit();
                System.Console.WriteLine("OzyInit completes");
                stop_USB();  // need to close and re-open USB port since it renumerated
                start_USB();
                this.Cursor = Cursors.Default;  // revert to normal cursor

                // get ozy code version now
                Ozy_version = getOzyFirmwareString();
            }

            // check that we are using the correct version of FX2 code
            if (Ozy_version != "20090201")
            {
                MessageBox.Show(" Wrong version of Ozy code found " + (Ozy_version == null ? "(Not Found!)" : Ozy_version.ToString()) + "\n-should be 20090524");
                stop_USB();
                return;
            }
        }


        private void SendButton_Click(object sender, EventArgs e)
        {
            ReceiveData.Text = null;
            // Format data ready to send via USB
            // get all data to send into an array - format is:
            // Ethernet_Preamble, To_MAC_address, From_MAC_address, Ethernet_type, IP_header, IP_protocol, *****
            
            // For MAC frames (i.e. Ethernet Type II Frame) the format is:
            // Ethernet_Preamble, To_MAC_address, From_MAC_address, Ethernet_type (0xEFFE),Payload (46 - 1500bytes), CRC32 (4 bytes)
           
            // create a tempory array to hold the Payload - do this each time so that it initialises to null
            byte[] Temp_Payload = new byte[4096];       // length will be a minimum of 46 and a maximim of 1500 bytes
            int Payload_length = 0;

            //Convert the data we are going to send into an array
            foreach (char word in SendData.Text)
            {
                Temp_Payload[Payload_length] = Convert.ToByte(word);
                Payload_length++;
            }

            if (Payload_length < 998) Payload_length = 998;  // minimum payload is 46 bytes, send 1024 for now

            // now make an array of this length to hold the Payload data
            byte[] Payload = new byte[Payload_length];

            // Now we know the length of the Payload data to use we can copy the temp data into it
            Array.Copy(Temp_Payload, Payload, Payload_length);

            // first calculate the length of the byte array we are going to send, this is the sum of all the array lengths
            int array_lenght = Ethernet_Preamble.Length + To_MAC_address.Length + From_MAC_address.Length + Ethernet_type.Length + Payload.Length + CRC32.Length;
            // establish an array of the appropriate size
            byte[] data_to_send = new byte[array_lenght];

            // now concatinate the various elements of the Ethernet frame
            Buffer.BlockCopy(Ethernet_Preamble, 0, data_to_send, 0, Ethernet_Preamble.Length);
            Buffer.BlockCopy(To_MAC_address, 0, data_to_send, Ethernet_Preamble.Length, To_MAC_address.Length);
            Buffer.BlockCopy(From_MAC_address, 0, data_to_send, To_MAC_address.Length + Ethernet_Preamble.Length, From_MAC_address.Length);
            Buffer.BlockCopy(Ethernet_type, 0, data_to_send, From_MAC_address.Length + To_MAC_address.Length + Ethernet_Preamble.Length, Ethernet_type.Length);
            Buffer.BlockCopy(Payload, 0, data_to_send, Ethernet_type.Length + From_MAC_address.Length + To_MAC_address.Length
                                                        + Ethernet_Preamble.Length, Payload.Length);
            Buffer.BlockCopy(CRC32, 0, data_to_send, Payload.Length + Ethernet_type.Length + From_MAC_address.Length + To_MAC_address.Length
                                                        + Ethernet_Preamble.Length, CRC32.Length);
           
            // now send this to the USB

            int ret;
            ret = libUSB_Interface.usb_bulk_write(hdev, 0x02, data_to_send, data_to_send.Length, 100);


            if (ret != data_to_send.Length)
            {
                Debug.WriteLine("Write to Ozy failed - returned \t" + ret);
                return;
            }

            // now look to see if we get any data back

            byte[] rbuf = new byte[data_to_send.Length];

            ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 1000); // rbuf.Length = 1024
            if (ret > 0)
            {
                // Display in the receive text box.

                int received_length = ret;
                string received_data = null;

                if (chkRaw.Checked)  // display all bytes in the Ethernet frame
                {
                    // convert from Hex to text
                    for (int x = 0; x < received_length; x++)
                    {
                        received_data = received_data + rbuf[x].ToString("X");
                    }

                    ReceiveData.Text = received_data;
                }
                else  // filter out just the payload
                {

                    // calculate the size of array needed to hold the Payload. This will be the length of the received buffer
                    // less the sum of the known Ethernet components

                    int length = received_length - (Ethernet_type.Length + From_MAC_address.Length + To_MAC_address.Length
                                                            + Ethernet_Preamble.Length + CRC32.Length);
                    // build an array to receive this 
                    byte[] Payload_data = new byte[1024];

                    // search the array and strip out the payload 

                    int i = 0;
                    for ( i=0; i<rbuf.Length-2; i++)
                    {
                        if (rbuf[i] == 0x55 && rbuf[i + 1] == 0x55 && rbuf[i + 2] == 0x55 && rbuf[i + 3] == 0x55 &&
                            rbuf[i + 4] == 0x55 && rbuf[i + 5] == 0x55 && rbuf[i + 6] == 0x55 && rbuf[i + 7] == 0xD5)
                        {
                            Console.WriteLine("Found sync at \t" + i);
                            break;
                        }
                    }


                    //strip out everything but the Payload
                    Buffer.BlockCopy(rbuf, i + To_MAC_address.Length + From_MAC_address.Length + Ethernet_type.Length, Payload_data, 0, 100);

                    // convert from Hex to a char string
                    for (int x = 0; x < Payload_data.Length; x++)
                    {
                        received_data += Convert.ToChar(Payload_data[x]);
                    }

                    ReceiveData.Text = received_data;
                }
            }
            else
            {
                ReceiveData.Text = "No data received \n";
            }        
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ReceiveData.Text = null;
        }

        private void Frame1_Click(object sender, EventArgs e)
        {
            SendData.Text = FromIP.Text + " " + FromMAC.Text + " " + "12345";
        }

        private void Frame2_Click(object sender, EventArgs e)
        {
            SendData.Text = "54321";
        }

        private void Frame3_Click(object sender, EventArgs e)
        {
            SendData.Text = "ABCDEF" + Convert.ToString(Ethernet_Preamble);
        }

        private bool start_USB()
        {
            // look for USB connection to Ozy
            hdev = USB.InitFindAndOpenDevice(0xfffe, 0x0007);
            if (hdev == IntPtr.Zero)
                return false;

            int ret;
            ret = libUSB_Interface.usb_set_configuration(hdev, 1);
            ret = libUSB_Interface.usb_claim_interface(hdev, 0);
            ret = libUSB_Interface.usb_set_altinterface(hdev, 0);
            ret = libUSB_Interface.usb_clear_halt(hdev, 0x02);
            ret = libUSB_Interface.usb_clear_halt(hdev, 0x86);
            return true;
        }

        // Get ozy firmware version string - 8 bytes,  returns null for error - thanks to Bill KD5TFD for this code
        private string getOzyFirmwareString()
        {
            if (hdev == IntPtr.Zero)
            {
                return null;
            }

            // the following set of declarations MUST match the values used in the FX2 code - hpsdr_commands.h
            byte VRQ_SDR1K_CTL = 0x0d;
            byte SDR1KCTRL_READ_VERSION = 0x7;
            byte VRT_VENDOR_IN = 0xC0;
            byte[] buf = new byte[8];
            int rc = libUSB_Interface.usb_control_msg(hdev, VRT_VENDOR_IN, VRQ_SDR1K_CTL, SDR1KCTRL_READ_VERSION, 0, buf, buf.Length, 1000);
            Debug.WriteLine("read version rc: " + rc);
            Debug.WriteLine("read version hdev = \t" + hdev);

            string result = null;

            if (rc == 8)    // got length we expected
            {
                char[] cbuf = new char[8];
                for (int i = 0; i < 8; i++)
                {
                    cbuf[i] = (char)(buf[i]);
                }
                result = new string(cbuf);
                Debug.WriteLine("version: >" + result + "<");
            }
            return result;
        }

        // this is called when we exit the program - terminate the USB connection 
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (USB_thread_running)
            //    USB_thread.Abort();  // stop USB thread

            if (hdev != IntPtr.Zero)  // check we have an active USB port
                stop_USB();
        }

        // stop USB interface when we exit the program
        private void stop_USB()
        {
            if (hdev != IntPtr.Zero) // check we have an open USB port 
            {
                try
                {
                    libUSB_Interface.usb_release_interface(hdev, 0);
                    libUSB_Interface.usb_close(hdev);
                }
                catch
                {
                }
            }
        }

    }
}
