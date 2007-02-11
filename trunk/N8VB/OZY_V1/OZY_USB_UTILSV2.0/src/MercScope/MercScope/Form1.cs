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
using SharpDSP;

namespace MercScope
{
    public partial class Form1 : Form
    {
        Rectangle scoperect;
        Random rnd;
        const int bsize = 2048;
        byte[] adcbuf = new byte[bsize];
        const int isize = 512;
        int[] ivalbuf = new int[isize];
        const int qsize = 512;
        int[] qvalbuf = new int[qsize];
        IntPtr hdev = IntPtr.Zero;
        bool adcon = false;
        PowerSpectrum ps = new PowerSpectrum(isize * 2, WindowType.BLACKMAN4_WINDOW);
        IQCorrection iqcor = new IQCorrection(512);

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
            this.Width = 631;
            this.Height = 770;
            pictureBox1.Width = 512;
            pictureBox1.Height = 512;
            scoperect = pictureBox1.ClientRectangle;
            timer1.Interval = 10;
            timer1.Enabled = true;
            hScrollBar2.Maximum = (int)(Int32.MaxValue - 1);
            hScrollBar2.Minimum = 0;
            hScrollBar2.SmallChange = (int)Int32.MaxValue / 100000;
            hScrollBar2.LargeChange = (int)Int32.MaxValue / 10000;            
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
            int gridspacing = (int)pictureBox1.Height/8;
            int tickspacing = (int)gridspacing/8;
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
            int xmax = 512; // number of sample points
            int ymax = 512;  // maximum y value
            int xlast = -1;
            int ylastI = 0;
            int ylastQ = 0;
            int ylastPS = 0;
            int yDrawI = 0;
            int yDrawQ = 0;
            int yDrawPS = 0;

            int yscale = (int)numericUpDown1.Value;
            int ps_scale = (int)nudSpecScale.Value;
            int yposI = vScrollBar1.Value;
            int yposQ = vScrollBar2.Value;
            int yposPS = vScrollBar3.Value;
            int xtscale = (int)numericUpDown2.Value;
            int xtpos = hScrollBar1.Value;
            int xDraw = xtpos;

            if (!read_adc(ref adcbuf))
                return;
                        
            for (int i = 0, j = 0; i < adcbuf.Length; i += 4, j++)
            {
                ivalbuf[j] = (int)BitConverter.ToInt16(adcbuf, i);                
                qvalbuf[j] = (int)BitConverter.ToInt16(adcbuf, i + 2);                
            }

            int maxval = 0;
            foreach (int val in ivalbuf)
            {
                if (val > maxval)
                    maxval = val;
            }

            txtMax.Text = maxval.ToString();

            int minval = 0;
            foreach (int val in ivalbuf)
            {
                if (val < minval)
                    minval = val;
            }

            txtMin.Text = minval.ToString();

            int Qmaxval = 0;
            foreach (int val in qvalbuf)
            {
                if (val > Qmaxval)
                    Qmaxval = val;
            }

            txtQMax.Text = Qmaxval.ToString();

            int Qminval = 0;
            foreach (int val in qvalbuf)
            {
                if (val < Qminval)
                    Qminval = val;
            }

            txtQMin.Text = Qminval.ToString();

            double[] d_i = new double[ivalbuf.Length];
            double[] d_q = new double[qvalbuf.Length];
            
            DataConvert.IntToDouble(ivalbuf, (int)Math.Pow(2,15), ref d_i);
            DataConvert.IntToDouble(qvalbuf, (int)Math.Pow(2, 15), ref d_q);

            //Gains gain;
            //gain.IQGainValue = (double)hscrollGain.Value / 10;
            //gain.IQPhaseValue = (double)hscrollPhase.Value / 10;

            //iqcor.DoIQCorrection(ref d_i, ref d_q, gain);

            double[] d_i_b = new double[ivalbuf.Length * 2];
            double[] d_q_b = new double[qvalbuf.Length * 2];

            for (int i = 0; i < ivalbuf.Length; i++)
            {
                d_i_b[i] = d_i[i];
                d_q_b[i] = d_q[i];
            }

            double[] ps_result = new double[d_i_b.Length];

            ps.PowerSpectrumSignal(ref d_i_b, ref d_q_b, ref ps_result);

            double PSminval = 0;
            foreach (double val in ps_result)
            {
                if (val < PSminval)
                    PSminval = val;
            }

            txtPSMin.Text = PSminval.ToString("E");

            double PSmaxval = double.MinValue;
            foreach (double val in ps_result)
            {
                if (val > PSmaxval)
                    PSmaxval = val;
            }

            txtPSMax.Text = PSmaxval.ToString("E");

            int[] ps_result_int = new int[ps_result.Length];
            
            DataConvert.DoubleToInt(ps_result, 32768, ref ps_result_int);
                       
            int Pminval = 0;
            foreach (int val in ps_result_int)
            {
                if (val < Pminval)
                    Pminval = val;
            }

            txtPMin.Text = Pminval.ToString();

            int Pmaxval = int.MinValue;
            foreach (int val in ps_result_int)
            {
                if (val > Pmaxval)
                    Pmaxval = val;
            }

            txtPMax.Text = Pmaxval.ToString();

            DataConvert.ScaleInt(-500, ref ps_result_int);
            DataConvert.ScaleInt(-1, ref ivalbuf);
            DataConvert.ScaleInt(-1, ref qvalbuf);

            // read each value from the buffer and plot the sample on the scope		
            for (int xpos = 0; xpos < xmax; xpos++)
            {
                int nI = ivalbuf[xpos];
                int nQ = qvalbuf[xpos];
                int nPS = ps_result_int[xpos * 2];
                
                int yintI = (int)(ymax+yposI) / 2 + (int)(nI >> yscale);
                int yintQ = (int)(ymax+yposQ) / 2 + (int)(nQ >> yscale);
                int yintPS = (int)(ymax + yposPS) / 2 + (int)(nPS >> ps_scale);

                yDrawI = yintI;
                yDrawQ = yintQ;
                yDrawPS = yintPS;

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
                        if (chkI.Checked) g.DrawLine(Pens.Yellow, xlast, ylastI, xDraw, yintI);
                        if (chkQ.Checked) g.DrawLine(Pens.Red, xlast, ylastQ, xDraw, yintQ);
                        if (chkSpec.Checked) g.DrawLine(Pens.White, xlast, ylastPS, xDraw, yintPS);
                    }
                }

                // remember the current sample point to allow for connecting
                // the next sample point
                xlast = xDraw;
                ylastI = yDrawI;
                ylastQ = yDrawQ;
                ylastPS = yDrawPS;
                              
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown3.Value;
            pictureBox1.Invalidate();
        }

        private void start_adc()
        {
            hdev = USB.InitFindAndOpenDevice(0xfffe, 0x00ff);
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
            if (checkBox2.Checked)
            {
                rnd.NextBytes(rbuf);
                return true;
            }
            else
            {

                int ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 500);
                if (ret != rbuf.Length)
                    return false;
                else
                    return true;
            }
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

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            byte[] buf = BitConverter.GetBytes(hScrollBar2.Value); // gets 4 bytes
            byte[] tempbuf = new byte[4];

            if (buf.Length < 4) return;

            // put bytes in proper order MSB to LSB
            for (int i = 0; i < buf.Length; i++)
            {
                tempbuf[buf.Length - 1 - i] = buf[i];
            }

            buf = tempbuf;

            textBox1.Text = ((((float)hScrollBar2.Value * (float)100))/(float)Math.Pow(2,32)).ToString();
            textBox2.Text = buf[0].ToString("X2") 
                            + ":" + buf[1].ToString("X2") 
                            + ":" + buf[2].ToString("X2") 
                            + ":" + buf[3].ToString("X2");
            
            // send to OZY register 0x01, 32 bits
            if (hdev != IntPtr.Zero)
                OZY.Write_SPI(hdev, 0, 0x01, OZY.SPI_EN_FPGA, (OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1), buf);
        }

        private void btnSendSR_Click(object sender, EventArgs e)
        {
            int cmd = Convert.ToInt32(txtSR.Text);
            byte[] buf = BitConverter.GetBytes(cmd);

            byte[] tempbuf = new byte[4];

            if (buf.Length < 4) return;

            // put bytes in proper order MSB to LSB
            for (int i = 0; i < buf.Length; i++)
            {
                tempbuf[buf.Length - 1 - i] = buf[i];
            }

            for (int i = 0; i < buf.Length; i++)
            {
                listBox1.Items.Add(buf[i]);
            }

            buf = tempbuf;
            // send to OZY register 0x02, 32 bits
            if (hdev != IntPtr.Zero)
                OZY.Write_SPI(hdev, 0, 0x02, OZY.SPI_EN_FPGA, (OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1), buf);            
            
        }

        private void btnRd_Click(object sender, EventArgs e)
        {
            byte[] buf = new byte[4];

            if (hdev != IntPtr.Zero)
                OZY.Read_SPI(hdev, 0, 0x02, OZY.SPI_EN_FPGA, (OZY.SPI_FMT_MSB | OZY.SPI_FMT_HDR_1), ref buf);

            for (int i = 0; i < buf.Length; i++)
            {
                listBox1.Items.Add(buf[i]);
            }
        }


    }
}