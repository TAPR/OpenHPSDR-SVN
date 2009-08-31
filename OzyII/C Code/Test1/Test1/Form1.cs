using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[] Ethernet_Preamble = new byte[8] { 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0xD5 };
        byte[] To_Ethernet_Address = new byte[4];
        byte[] From_Ethernet_Address = new byte[4];
        byte[] To_MAC_address = new byte[6];
        byte[] From_MAC_address = new byte[6];
        //byte[] UDP_payload = new byte[18] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11};
        //byte[] IP_header = new byte[9] { 0x45, 0x00, 0x00, 0x2E, 0xB3, 0xFE, 0x00, 0x00, 0x80 };
        byte[] Ethernet_type = new byte[2] {0xEF, 0xFE};           // Type is MAC, use 0x0800 for IP 
        byte[] CRC32 = new byte[4]{0x00, 0x01, 0x02, 0x03};        // CRC for MAC frames
        //byte IP_protocol = 0x11;                                   // Protocol is UDP
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

        }


        private void SendButton_Click(object sender, EventArgs e)
        {
            ReceiveData.Text = null;
            //ReceiveData.Text = SendData.Text;
            // Format data ready to send via USB
            // get all data to send into an array - format is:
            // Ethernet_Preamble, To_MAC_address, From_MAC_address, Ethernet_type, IP_header, IP_protocol, *****
            
            // For MAC frames (i.e. Ethernet Type II Frame) the format is:
            // Ethernet_Preamble, To_MAC_address, From_MAC_address, Ethernet_type (0xEFFE),Payload (46 - 1500bytes), CRC32 (4 bytes)
           
            // create a tempory array to hold the Payload - do this each time so that it initialises to null
            byte[] Temp_Payload = new byte[1500];       // length will be a minimum of 46 and a maximim of 1500 bytes
            int Payload_length = 0;

            //Convert the data we are going to send into an array
            foreach (char word in SendData.Text)
            {
                Temp_Payload[Payload_length] = Convert.ToByte(word);
                Payload_length++;
            }

            if (Payload_length < 46) Payload_length = 46;  // minimum payload is 46 bytes

            // now make an array of this length to hold the Payload data
            byte[] Payload = new byte[Payload_length];

            // Now we know the length of the Payload data to use we can copy the temp datat into it
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
            // data_to_send



            // Assume we get the data back - so display in the receive text box.

            int received_length = data_to_send.Length;
            string received_data = null;

            // convert from Hex to text
            for (int x = 0; x < received_length; x++)
            {
                received_data = received_data + data_to_send[x].ToString("X");
            }

            //ReceiveData.Text = received_data;

            byte[] test = new  byte[10];

            //strip out everything but the Payload
            Buffer.BlockCopy(data_to_send, Ethernet_Preamble.Length + To_MAC_address.Length + From_MAC_address.Length + Ethernet_type.Length, test, 0, test.Length);

            // convert from Hex to text
            for (int x = 0; x < test.Length; x++)
            {
                received_data = test + test[x].ToString("X");
            }

            ReceiveData.Text = received_data;
        
        
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


    }
}
