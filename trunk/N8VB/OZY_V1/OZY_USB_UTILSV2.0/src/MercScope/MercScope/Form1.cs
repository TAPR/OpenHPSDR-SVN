using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HPSDR_USB_LIB_V1;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MercScope
{
    public partial class Form1 : Form
    {
        Rectangle scoperect;
        Random rnd;
        const int bsize = 2048;
        byte[] adcbuf = new byte[bsize];
        const int isize = 1024;
        int[] valbuf = new int[isize];
        IntPtr hdev = IntPtr.Zero;
        bool adcon = false;
        bool singleshot = false;

        public Form1()
        {
            InitializeComponent();
            rnd = new Random();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;            
            DrawGrid(g);
            if (adcon) RefreshScope(g);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 1110;
            this.Height = 800;
            pictureBox1.Width = 1000;
            pictureBox1.Height = pictureBox1.Width/2;
            scoperect = pictureBox1.ClientRectangle;
            timer1.Interval = 10;
            timer1.Enabled = true;            
        }

        private void DrawGrid(Graphics g)
        {
            // Draw the background color
            g.FillRectangle(Brushes.DarkGreen, scoperect);
            Font font = new Font(FontFamily.GenericMonospace, 12);
            g.DrawString("Vert: ", font, Brushes.White, 10, 10);
            g.DrawString("Horz: ", font, Brushes.White, 10, 25);

            int tickheight = 4;
            int tickheightLarge = 8;
            int gridspacing = (int)pictureBox1.Height/10;
            int tickspacing = (int)gridspacing/10;
            int largetickspacing = (int)gridspacing/2;

            // draw the Grid Lines
            for (int i = 0; i < pictureBox1.Width; i += gridspacing)
            {
                // draw horizontal line
                g.DrawLine(Pens.DarkGray, 0, i, pictureBox1.Width, i);

                // draw vertical line
                g.DrawLine(Pens.DarkGray, i, 0, i, pictureBox1.Height);

                // draw ticks on horizontal scale
                if (i == pictureBox1.Height/2)
                {
                    for (int j = 0; j < pictureBox1.Width; j += tickspacing)
                    {
                        if (j % largetickspacing != 0)
                        {
                            g.DrawLine(Pens.White, j, (i - tickheight / 2),
                                j, (i + tickheight / 2));                            
                        }
                        else
                        {
                            g.DrawLine(Pens.White, j, (i - tickheightLarge / 2),
                                j, (i + tickheightLarge / 2));                            
                        }
                    }
                }

                // draw ticks on vertical scale
                if (i == pictureBox1.Width/2)
                {
                    for (int j = 0; j < pictureBox1.Width; j += tickspacing)
                    {
                        if (j % largetickspacing != 0)
                        {
                            g.DrawLine(Pens.White, (i - tickheight / 2), j,
                                (i + tickheight / 2), j);
                        }
                        else
                        {
                            g.DrawLine(Pens.White, (i - tickheightLarge / 2), j,
                                (i + tickheightLarge / 2), j);
                        }
                    }
                }
            }                                    
        }

        private void RefreshScope(Graphics g)
        {
            int xmax = 1000; // number of sample points
            int ymax = 500;  // maximum y value
            int xlast = -1;
            int ylast = 0;
            int yDraw = 0;

            int yscale = (int)numericUpDown1.Value;
            int ypos = vScrollBar1.Value;
            int xtscale = (int)numericUpDown2.Value;
            int xtpos = hScrollBar1.Value;
            int xDraw = xtpos;

            if (!read_adc(ref adcbuf))
                return;

            for (int i = 0, j = 0; i < adcbuf.Length; i += 2, j++)
            {
                valbuf[j] = (int)(adcbuf[i + 1] * 256 + adcbuf[i]);
                valbuf[j] = nBitTwosComp(valbuf[j], 16);
            }

            // read each value from the buffer and plot the sample on the scope		
            for (int xpos = 0; xpos < xmax; xpos++)
            {
                //int n = rnd.Next(-32767, 32767);
                int n = valbuf[xpos];

                int yint = (int)(ymax+ypos) / 2 + (int)(n >> yscale);

                yDraw = yint;

                // don't draw the first sample point, use it as
                // a starting sample point
                if (xlast != -1)
                {
                    // scale the x-value based on the TimeScale control
                    // translate the x-position by the x-position congrol
                    if (xtscale > 1)
                        xDraw = xpos * xtscale + xtpos * xtscale;
                    else
                        xDraw = xpos * xtscale;

                    // Draw the scaled sample point by connecting
                    // the previous scaled sample point to the current
                    // scaled sample point
                    if (xDraw < pictureBox1.Width)
                    {
                        g.DrawLine(Pens.Yellow, xlast, ylast, xDraw, yint);
                    }
                }

                // remember the current sample point to allow for connecting
                // the next sample point
                xlast = xDraw;
                ylast = yDraw;
                              
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown3.Value;
            pictureBox1.Invalidate();
        }

        private void start_adc()
        {
            hdev = USB.InitFindAndOpenDevice(0xfffe, 0x0007);
            libUSB_Interface.usb_set_configuration(hdev, 1);
            libUSB_Interface.usb_claim_interface(hdev, 0);
            libUSB_Interface.usb_set_altinterface(hdev, 0);
            libUSB_Interface.usb_clear_halt(hdev, 0x02);
            libUSB_Interface.usb_clear_halt(hdev, 0x86);            
        }

        private void stop_adc()
        {
            libUSB_Interface.usb_release_interface(hdev, 0);
            libUSB_Interface.usb_close(hdev);            
        }

        private bool read_adc(ref byte[] rbuf)
        {
            int ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 500);
            if (ret != rbuf.Length)
                return false;
            else
                return true;
        }

        private static int nBitTwosComp(int data, int numBits)
        {
            System.Collections.BitArray b = new System.Collections.BitArray(new int[] { data });

            int result = -(data & (1 << numBits - 1));
            //int result = -(b[numBits - 1] ? 1 : 0) * (1 << numBits - 1);

            //for (int i = numBits - 2; i >= 0; i--)
            //    result += (b[i] ? 1 : 0) * (1 << i);

            for (int i = numBits - 2; i >= 0; i--)
                result |= (data & (1 << i));

            return result;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            adcon = checkBox1.Checked;
            timer1.Enabled = adcon;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adcon = true;
            pictureBox1.Invalidate();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            listBox1.Items.Add("Starting ADC...");
            start_adc();
            listBox1.Items.Add("ADC running...");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stop_adc();
        }

    }
}