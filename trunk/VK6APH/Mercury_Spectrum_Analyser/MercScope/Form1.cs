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
        const int bsize = 1024;
        byte[] adcbuf = new byte[bsize];
        const int isize = 512;
        int[] ivalbuf = new int[isize];
        const int qsize = 512;
        int[] qvalbuf = new int[qsize];
        double[] ADC_sample = new double[bsize];
        IntPtr hdev = IntPtr.Zero;
        bool adcon = false;
        //PowerSpectrum ps = new PowerSpectrum(isize * 2, WindowType.BLACKMAN4_WINDOW);
        PowerSpectrum ps = new PowerSpectrum(isize * 2, WindowType.BLACKMANHARRIS_WINDOW);
        static readonly float cutoff_f = 0.1f;  // corner frequecy  = cutoff_f * clock
        Resampler r_i = new Resampler(cutoff_f);
        Resampler r_q = new Resampler(cutoff_f);
       

        public Form1()
        {
            InitializeComponent();
            rnd = new Random();
            start_adc();
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
            pictureBox1.Width = 1024; //512;
            pictureBox1.Height = 512;
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

            float lo_phase_angle = 0;

            if (!read_adc(ref adcbuf)) // check we have ADC data, if not return
                return;

            int[] int_adcbuf = new int[isize];

            // convert adc samples from bytes to int (32 bits)
            for (int i = 0, j = 0; i < adcbuf.Length; i += 2, j++)
            {
                int_adcbuf[j] =(int)BitConverter.ToInt16(adcbuf, i);
                //ivalbuf[j] = int_adcbuf[j]; // display adc samples on screen
            }
            // convert adc samples from int to double 
            double[] d_adc = new double[isize];
            DataConvert.IntToDouble(int_adcbuf, (int)Math.Pow(2,15), ref d_adc);

            // create arrays to hold the I&Q values from the complex mixer
            double[] d_i_b = new double[isize]; 
            double[] d_q_b = new double[isize];

            // multiply  real input by sin & cos to create I&Q
            for (int i = 0; i < isize; i++)
            {
                d_i_b[i] = d_adc[i] * Math.Sin(lo_phase_angle);
                d_q_b[i] = d_adc[i]  *Math.Cos(lo_phase_angle);
                lo_phase_angle += 0.6F;  // this is the local oscillator, frequecy = angle * clock/2pi
            }

            // Pass I&Q samples through FIR LPF

            float[] input_data_i = new float[isize];
            float[] input_data_q = new float[isize];

            float[] output_data_i = new float[isize];
            float[] output_data_q = new float[isize];

            //convert double to float
            for (int i = 0; i < isize; i++)
            {
                input_data_i[i] = (float)d_i_b[i]; 
                input_data_q[i] = (float)d_q_b[i];
            }

            // apply FIR LPF

            r_i.doFilter(ref input_data_i, ref output_data_i); // I data
            r_q.doFilter(ref input_data_q, ref output_data_q); // Q data


            // convert float to double
            double[] out_data_i = new double[isize];
            double[] out_data_q = new double[isize];

            for (int i = 0; i < isize; i++)
            {
                out_data_i[i] = (double)output_data_i[i];
                out_data_q[i] = (double)output_data_q[i];
            }


            DataConvert.DoubleToInt(out_data_i, 32768, ref ivalbuf); // I channel on 'scope has FIR output
            DataConvert.DoubleToInt(d_i_b, 32768, ref qvalbuf); // Q channel on 'scope has FIR input 
            // DataConvert.DoubleToInt(out_data_q, 30000, ref qvalbuf); // display Q channel on 'scope

            double[] ps_result = new double[d_i_b.Length];

            ps.PowerSpectrumSignal(ref d_i_b, ref d_q_b, ref ps_result);  // I & Q before LPF 
            //ps.PowerSpectrumSignal(ref out_data_i, ref out_data_q, ref ps_result);

            int[] ps_result_int = new int[ps_result.Length];
            
            DataConvert.DoubleToInt(ps_result, 32768, ref ps_result_int);
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
                    // translate the x-position by the x-position control
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


        double phase_angle = 0;
      
        private bool read_adc(ref byte[] rbuf)
        {
            if (checkBox2.Checked) 
            {
                // Fill rbuf with 10 samples (i.e. 12.5MHz when sampling at 125MHz)
                // of a  16 bit sine wave to simulate input from Mercury. 
                // Add some noise for extra realism!

                rnd.NextBytes(rbuf);
                for (int j = 0; j < rbuf.Length; j += 2) // rbuf.Length = 1024
                {  
                    double i; 
                    short i16;
                    byte[] tbuf; 
                    i = Math.Sin(phase_angle); 
                    //phase_angle += 0.0368;
                    phase_angle += 0.5;  
                    i16 = (short)(32767.0 * i);
                    tbuf = BitConverter.GetBytes(i16);
                    byte noise_bits = (byte)(rbuf[j] & 0xf); 
                    //noise_bits = 0; 
                    rbuf[j] = (byte)(tbuf[0] ^ noise_bits);
                    rbuf[j + 1] = tbuf[1];
                }
                return true;
            }
            else
            {

                //int ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, 500);
                //if (ret != rbuf.Length)
                //    return false;
                //else
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
           // start_adc();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //stop_adc();
        }
    }
}

