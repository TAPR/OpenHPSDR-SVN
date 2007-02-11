using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HPSDR_USB_LIB_V1;

namespace QS1R_Control
{
    public partial class Main : Form
    {
        /* Vendor Request Types */
        public const int VENDOR_REQ_TYPE_IN = 0xc0;
        public const int VENDOR_REQ_TYPE_OUT = 0x40;

        public IntPtr device = IntPtr.Zero;

        public Main ( )
        {
            InitializeComponent ( );
        }

        private void btnConnect_Click ( object sender, EventArgs e )
        {
            int VID = Int32.Parse ( txtVID.Text, System.Globalization.NumberStyles.HexNumber );
            int PID = Int32.Parse ( txtPID.Text, System.Globalization.NumberStyles.HexNumber );

            listBox1.Items.Add ( "looking for VID: " + VID.ToString("x") + " PID: " + PID.ToString("x") );

            device = USB.InitFindAndOpenDevice ( VID, PID );

            if ( device == IntPtr.Zero )
                listBox1.Items.Add ( "Error" );
            else
                listBox1.Items.Add ( "Found device!" );

        }

        private void btnReadRam_Click ( object sender, EventArgs e )
        {
            int start = Int32.Parse ( txtStart.Text, System.Globalization.NumberStyles.HexNumber );
            int length = Int32.Parse ( txtLength.Text, System.Globalization.NumberStyles.HexNumber );

            if ( device == IntPtr.Zero )
                return;

            byte [ ] buf = new byte [ length ];

            USB.Read_RAM ( device, start, ref buf );

            listBox2.Items.Add("Reading at: " + start);

            for ( int i = 0; i < buf.Length; i++ )
            {
                listBox2.Items.Add ( buf [ i ].ToString ( "x" ) );
            }
        }

        private void btnReadLTCREG_Click ( object sender, EventArgs e )
        {
            byte [ ] buffer = new byte [ 1 ];

            int ret = libUSB_Interface.usb_control_msg (
                device,
                VENDOR_REQ_TYPE_IN,
                0x87,
                0,
                0,
                buffer,
                buffer.Length,
                1000
                );
            
            listBox3.Items.Add ( buffer [ 0 ] );
        }
    }
}