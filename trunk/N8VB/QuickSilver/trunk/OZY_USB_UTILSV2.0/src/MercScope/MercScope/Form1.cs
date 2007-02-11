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
        const int isize = 512;
        static double[] ibuffer = new double[isize];
        static double[] qbuffer = new double[isize];
        
        static IntPtr hdev = IntPtr.Zero;
        bool adcon = false;
        PowerSpectrum ps = new PowerSpectrum(isize * 2, WindowType.BLACKMANHARRIS_WINDOW);
        static qs1r rx = new qs1r();
        //static ThreadStart ts = new ThreadStart(looper);
        //Thread t = new Thread(ts);
		
        /*
        static public void looper()
        {
        	while (true)
        	{
        		rx.GetBlock(ref ibuffer, ref qbuffer);
        		Thread.Sleep(0);
        	}
        }
        */
       
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
            timer1.Enabled = false;                 
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

        /*
        private void Do_File(ref byte[] adcbuf)
        {
        	System.IO.StreamWriter sr = new StreamWriter("c:\\buffer.txt");
        	        	
        	for (int i = 0, j = 0; i < adcbuf.Length; i += 8, j++)
            {
            	int ivalbuf = (int)(BitConverter.ToInt32(adcbuf, i + 0)) - (int)this.hscrollGain.Value;
            	int qvalbuf = (int)(BitConverter.ToInt32(adcbuf, i + 4)) - (int)this.hscrollGain.Value;
            	sr.WriteLine(j.ToString() + " : " + ivalbuf.ToString() + " : " + qvalbuf.ToString());
            }
        	
        	this.cbFile.Checked = false;
        	sr.Close();
        }
        */
        
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

            rx.GetBlock(ref ibuffer, ref qbuffer);
                     
            double maxval = 0;
            foreach (double val in ibuffer)
            {
                if (val > maxval)
                    maxval = val;
            }

            txtMax.Text = maxval.ToString();

            double minval = 0;
            foreach (double val in ibuffer)
            {
                if (val < minval)
                    minval = val;
            }

            txtMin.Text = minval.ToString();

            double Qmaxval = 0;
            foreach (double val in qbuffer)
            {
                if (val > Qmaxval)
                    Qmaxval = val;
            }

            txtQMax.Text = Qmaxval.ToString();

            double Qminval = 0;
            foreach (double val in qbuffer)
            {
                if (val < Qminval)
                    Qminval = val;
            }

            txtQMin.Text = Qminval.ToString();

            txtIVal.Text = ibuffer[0].ToString();
            txtQVal.Text = qbuffer[0].ToString();
            
            double[] d_i_b = new double[ibuffer.Length * 2];
            double[] d_q_b = new double[ibuffer.Length * 2];

            for (int i = 0; i < ibuffer.Length; i++)
            {
                d_i_b[i] = ibuffer[i];
                d_q_b[i] = qbuffer[i];
            }

            double[] ps_result = new double[d_i_b.Length];

            ps.PowerSpectrumSignal(ref d_i_b, ref d_q_b, ref ps_result);

            double PSminval = double.MaxValue;
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
            
            for (int i=0; i<ps_result.Length; i++)
            {
            	ps_result_int[i] = Convert.ToInt32(ps_result[i] * 100.0);
            }
                       
            int Pminval = int.MaxValue;
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

            int[] ivalbuf = new int[ibuffer.Length];
            int[] qvalbuf = new int[ibuffer.Length];
            
            for (int i=0; i<ibuffer.Length; i++)
            {
            	ivalbuf[i] = (int)(ibuffer[i]);
            	qvalbuf[i] = (int)(qbuffer[i]);
            }
            
                        
            DataConvert.ScaleInt(-0.001, ref ps_result_int);
            DataConvert.ScaleInt(-1, ref ivalbuf);
            DataConvert.ScaleInt(-1, ref qvalbuf);
            
            // read each value from the buffer and plot the sample on the scope		
            for (int xpos = 0; xpos < xmax; xpos++)
            {
                int nI = ivalbuf[xpos];
                int nQ = qvalbuf[xpos];
                int nPS = (int)(((double)ps_result[xpos * 2] * (double)100.0) )* -1;
                
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
        	rx.Connect(0xfffe, 0x00ff);
        	//rx.Start();
        }

        private void stop_adc()
        {
        	//rx.Stop();
        	rx.Close();
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
        	
        }

        private void btnSendSR_Click(object sender, EventArgs e)
        {                      
            
        }

        private void btnRd_Click(object sender, EventArgs e)
        {  
        	
        }
        
        void BtnSendScaleClick(object sender, EventArgs e)
        {                        
            
        }
        
        void Button3Click(object sender, EventArgs e)
        {
        	   
        }
        
        void NudFrequencyValueChanged(object sender, EventArgs e)
        {
        	rx.SetFrequency((double)nudFrequency.Value);
        	textBox1.Text = nudFrequency.Value.ToString();
        }
    }
}
