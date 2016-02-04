//
/* Change log:

            1 Feb 2014  - initial release of RA v1.0

            2 Feb 2014  - fixed "write file" bug that caused a zero data file to be written after acquistion has been stopped
                        - added user warning if a data file containing no valid data is opened
                        - changed version number to RA v1.1
 
            3 Feb 2014  - added code to close the data file when max points have been collected
                        - disabled window resize option (temporarily)
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Thetis
{
    public partial class RAForm : System.Windows.Forms.Form
    {

        public RAForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            RArecordCheckBox.BackColor = Color.DarkGreen;
            RArecordCheckBox.ForeColor = Color.White;
            RArecordCheckBox.Text = "Start";
            labelTS10.Text = "";
            labelTS14.ForeColor = System.Drawing.Color.Yellow;
            labelTS14.Text = "";
            max_count = 1;
        }
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                 {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private System.IO.BinaryWriter writer;
        //private System.Windows.Forms.FileDialog reader;
        private byte[] header_K5SO;
        private byte[] RA_data;
        private double time0;
        private byte[] CRLF;
        private int RA_count;
        private int max_count;
        private int data_points;
        private float sig_level;
        private float sig_level_avg;
        private float sig_max;
        private float sig_min;
        private float initial_sig;
        private float display_y_max = 0;
        private float display_y_min = -150;
        private float display_x_max = 1000; // seconds
        private float display_x_min = 0;
        private int Y_range;
        private int X_range;
        private bool rescale;
        private float RA_cal;
        private float[] sig_data = new float[10000];
        private float[] time_data = new float[10000]; 
        private double time_elapsed;
        double ref_power = 0;
        String s2 = "";

        private void RArecordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RArecordCheckBox.Checked)
            {
                RArecordCheckBox.BackColor = Color.LimeGreen;
                RArecordCheckBox.ForeColor = Color.Black;
                RArecordCheckBox.Text = "Stop";
                try
                {
                    writer = new System.IO.BinaryWriter(System.IO.File.Open("RA_data.csv", System.IO.FileMode.Create));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                header_K5SO = new byte[] { 0x74, 0x69, 0x6D, 0x65, 0x20, 
                                           0x28, 0x73, 0x65, 0x63, 0x6F, 
                                           0x6E, 0x64, 0x73, 0x29, 0x2C, 
                                           0x20, 0x73, 0x69, 0x67, 0x6E, 
                                           0x61, 0x6C, 0x20, 0x28, 0x64, 
                                           0x42, 0x6D, 0x29, 0x0D, 0x0A 
                                          };

                writer.Write(header_K5SO, 0, 30);
                time0 = Environment.TickCount;
                CRLF = new byte[] { 0x0D, 0x0A };
                // collect initial data point
                RA_cal = -4.23f;
                sig_level = wdsp.CalculateRXMeter(0, 0, wdsp.MeterType.SIGNAL_STRENGTH);
                initial_sig = sig_level;
                sig_max = sig_level + RA_cal + 10;           // initial value for sig_max
                sig_min = sig_max - 10;
                rescale = true;
                RA_count = 1;
                for (int i = 0; i < 10000; i++)      // clear any previously acquired data
                {
                    sig_data[i] = 0;
                    time_data[i] = 0;
                }
                data_points = 1;
                picRAGraph.Invalidate();            // draw data display
                labelTS10.Text = "";                // data saved msg
                button_readFile.Enabled = false;
                RA_timer.Enabled = true;
            }
            else
            {
                RA_timer.Enabled = false;
                RArecordCheckBox.BackColor = Color.DarkGreen;
                RArecordCheckBox.ForeColor = Color.White;
                RArecordCheckBox.Text = "Start";
                writer.Flush();
                writer.Close();
                labelTS14.Text = "";
                labelTS10.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
                labelTS10.ForeColor = System.Drawing.SystemColors.Highlight;
                labelTS10.Text = "data written to: RA_data.csv";
                button_readFile.Enabled = true;
            }              
        }

        private void numericUpDownTS1_ValueChanged(object sender, EventArgs e)
        {
            max_count = (int) numericUpDownTS1.Value;
        }

        private void RA_timer_Tick(object sender, EventArgs e)
        {
            RA_data = new byte[30];
            int cnt_string;
            string time_string;
            string signal_string;
            string line_string;
            double time_now;
            string format = "f1";

            if (RA_count == max_count)
               {
                   RA_timer.Enabled = false;
                   data_points += 1;
                   textBox_pts_collected.Text = data_points.ToString();
                   time_now = Environment.TickCount;
                   time_elapsed = 0.001 * (time_now - time0);   // in seconds
                   time_string = time_elapsed.ToString("f3") + ", ";
                   sig_level_avg = (sig_level / RA_count) + RA_cal;   // approximate signal level in dBm
                   sig_data[data_points] = sig_level_avg;
                   time_data[data_points] = (float) time_elapsed;
                   if (sig_level_avg > sig_max)
                   {
                       sig_max = sig_level_avg;
                       rescale = true;
                   }
                   if (sig_min > sig_level_avg )
                   {
                       sig_min = sig_level_avg;
                       rescale = true;
                   }
                   textBoxTS3.Text = time_elapsed.ToString(format);
                   textBoxTS2.Text = sig_level_avg.ToString(format);
                   signal_string = sig_level_avg.ToString("f3"); // System.Convert.ToString(sig_level_avg);
                   line_string = time_string + signal_string;
                   RA_data = StrToByteArray(line_string);
                   cnt_string = RA_data.Length;
                   writer.Write(RA_data, 0, cnt_string);
                   writer.Write(CRLF, 0, 2);
                   //get current value of signal and go accumulate a new average
                   sig_level = wdsp.CalculateRXMeter(0, 0, wdsp.MeterType.SIGNAL_STRENGTH);
                   RA_count = 1;

                   // update by plotting new graphics display
                   picRAGraph.Invalidate();
                   if (RArecordCheckBox.Checked) RA_timer.Enabled = true;  // continue w/data collection
               }
            else
            {
                sig_level += wdsp.CalculateRXMeter(0, 0, wdsp.MeterType.SIGNAL_STRENGTH);
                RA_count = RA_count + 1;
            }       
        }

        private void numericUpDownTS2_ValueChanged(object sender, EventArgs e)
        {
            RA_timer.Interval = (int) numericUpDownTS2.Value;
        }

        // RA graphics plot
        private void picRAGraph_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {            
            // capital "X" or "Y" denotes pixel coordinates whereas "x" or "y" in a variable name denotes user coordinates
            int i;
                Graphics g = e.Graphics;
                Rectangle rect = new Rectangle(0, 0, picRAGraph.Width, picRAGraph.Height);
                int size = Math.Min(picRAGraph.Width, picRAGraph.Height);
                Pen pen = new Pen(Brushes.Black);
                Pen pen2 = new Pen(Brushes.Yellow);
                Pen pen3 = new Pen(Brushes.White);
                Pen pen4 = new Pen(Brushes.LightGray);
                Pen pen5 = new Pen(Brushes.Red);
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //g.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(new Point((int)(size / 2), 0), new Point((int)(size / 2), size - 1), topColor, bottomColor), rect);
                g.FillRectangle(Brushes.AliceBlue, 0, 0, (float) picRAGraph.Width, (float) picRAGraph.Height);

                //draw outside boundary
                g.DrawRectangle(pen, 0, 0, picRAGraph.Width-1, picRAGraph.Height-1);

                //draw and label grid lines
                //
                // find corner points of the plot and the y-range and x-range in pixels

                Point top_left = new Point(picRAGraph.Left, picRAGraph.Top);
                Point bottom_left = new Point(picRAGraph.Left, picRAGraph.Bottom);
                Point top_right = new Point(picRAGraph.Right, picRAGraph.Top);
                Point bottom_right = new Point(picRAGraph.Right, picRAGraph.Bottom);
                Point p1 = new Point();
                Point p2 = new Point();
                int X_left = bottom_left.X;
                int X_right = bottom_right.X;
                int Y_top = top_right.Y;
                int Y_bottom = bottom_right.Y;
                int n_lines = 0;
                float display_y_range;
                float display_x_range;
                float y_step = 0;
                float x_step = 0;
                double y_first_value = 0;
                double x_first_value = 0;
                int Y_first = 0;
                int X_first = 0;

                Font font = new Font("Arial", 10.0f);
                SolidBrush brush = new SolidBrush(Color.Black);
                Y_range = Y_bottom - Y_top;

                if (button_dBm.Checked)     // dBm display mode
                {
                    labelTS12.Text = "                  Signal (dBm)";
                    labelTS14.Text = "";
                    // rescale plot if necessary
                    if (auto_rescale.Checked)
                    {
                        groupBox_scaling.Enabled = false;

                        if (rescale)
                        {
                            display_y_max = sig_max + 10;
                            display_y_min = sig_min - 10;
                        }
                    }
                    else
                    {
                        groupBox_scaling.Enabled = true;
                        display_y_max = (float)manual_ymax.Value;
                        display_y_min = (float)manual_ymin.Value;
                    }
                    // calculate y position for first horizonal grid line...log mode 
                    display_y_range = display_y_max - display_y_min;
                    if (display_y_range > 30)
                    {
                        y_first_value = Math.Truncate(display_y_min / 10) * 10;
                        y_step = 10;
                    }
                    else
                    {
                        if (display_y_range > 15)
                        {
                            y_first_value = Math.Truncate(display_y_min / 5) * 5;
                            y_step = 5;
                        }
                        else
                        {
                            y_first_value = Math.Truncate(display_y_min);
                            y_step = 1;
                        }
                    }
                    n_lines = (int)(Math.Truncate((double)display_y_range / (double)y_step)) + 1;
                    Y_first = Y_range + (int)(((display_y_min - y_first_value) / display_y_range) * Y_range);
                }
                else    // linear display mode   
                {
                    //ref_power =  Math.Pow(10, (double) zeroRefAdjust.Value / 10);               // ref power in Watts
                    ref_power = Math.Pow(10, (double)sig_max / 10);
                    // convert dBm sig_max to linear value in units of ref_power
                    s2 = ref_power.ToString("E1");
                    labelTS12.Text = "Signal ( x " + s2 + " mW)";
                    //s2 = " ( x " + s2 + " mW)";
                    //groupBox_signal.Text = "signal" + s2;
                    // convert linear signal into units of ref_power
                    float linear_sig_max = (float)Math.Pow(10, sig_max / 10) / (float)ref_power; // in units of ref_power
                    float linear_sig_min = (float)Math.Pow(10, sig_min / 10) / (float)ref_power;
                    // rescale plot if necessary
                    if (auto_rescale.Checked)
                    {
                        groupBox_scaling.Enabled = false;

                        if (rescale)
                        {
                            display_y_max = 1.1f * linear_sig_max;
                            display_y_min = 0;
                        }
                    }
                    else
                    {
                        groupBox_scaling.Enabled = true;
                        display_y_max = (float)manual_ymax.Value;
                        display_y_min = (float)manual_ymin.Value;
                    }
                    if (display_y_max <= 0) display_y_max = 100;
                    if (display_y_min < 0) display_y_min = 0;

                    // calculate y position for first horizonal grid line and step size between grid lines...linear mode
                    display_y_range = display_y_max - display_y_min;

                    if (display_y_range > 500)
                    {
                        y_first_value = Math.Truncate(display_y_min / 100) * 100;
                        y_step = 100;
                    }
                    else if (display_y_range > 300)
                    {
                        y_first_value = Math.Truncate(display_y_min / 50) * 50;
                        y_step = 50;
                    }
                    else if (display_y_range > 200)
                    {
                        y_first_value = Math.Truncate(display_y_min / 20) * 20;
                        y_step = 20;
                    }
                    else if (display_y_range > 30)
                    {
                        y_first_value = Math.Truncate(display_y_min / 10) * 10;
                        y_step = 10;
                    }
                    else
                    {
                        if (display_y_range > 15)
                        {
                            y_first_value = Math.Truncate(display_y_min / 5) * 5;
                            y_step = 5;
                        }
                        else
                        {
                            y_first_value = Math.Truncate(display_y_min);
                            y_step = 1;
                        }
                    }
                    n_lines = (int)(Math.Truncate((double)display_y_range / (double)y_step)) + 1;
                    Y_first = Y_range + (int)(((display_y_min - y_first_value) / display_y_range) * Y_range);
                }
                // draw and label horizontal grid lines
                float y_label = (float)y_first_value;
                int Y_value;
                String s;
                for (i = 0; i < n_lines; i++)
                {
                    Y_value = Y_first - (int)(((i * y_step) / (display_y_range)) * Y_range);
                    p1.X = 0;
                    p1.Y = Y_value;
                    p2.X = X_right;
                    p2.Y = Y_value;
                    g.DrawLine(pen4, p1, p2);
                    p1.Y -= (int)(0.027 * Y_range);
                    s = y_label.ToString("f0");
                    g.DrawString(s, font, brush, p1);
                    y_label += y_step;
                }
                    
            
                // ******************************************************************************************************
                // draw and label vertical grid lines
                // calculate y position for first horizonal grid line and step size between grid lines...linear mode
                display_x_range = display_x_max - display_x_min;
                X_range = X_right - X_left;

                if (display_x_range > 6000)
                {
                    x_first_value = Math.Truncate(display_x_min / 1000) * 1000;
                    x_step = 1000;
                }

                else if (display_x_range > 3000)
                {
                    x_first_value = Math.Truncate(display_x_min / 500) * 500;
                    x_step = 500;
                }

                else if (display_x_range > 1500)
                {
                    x_first_value = Math.Truncate(display_x_min / 100) * 100;
                    x_step = 200;
                }

                else if (display_x_range > 500)
                {
                    x_first_value = Math.Truncate(display_x_min / 100) * 100;
                    x_step = 100;
                }
                else if (display_x_range > 300)
                {
                    x_first_value = Math.Truncate(display_x_min / 50) * 50;
                    x_step = 50;
                }
                else if (display_x_range > 200)
                {
                    x_first_value = Math.Truncate(display_x_min / 20) * 20;
                    x_step = 20;
                }
                else if (display_x_range > 60)
                {
                    x_first_value = Math.Truncate(display_x_min / 10) * 10;
                    x_step = 10;
                }
                else
                {
                    if (display_x_range > 15)
                    {
                        x_first_value = Math.Truncate(display_x_min / 5) * 5;
                        x_step = 5;
                    }
                    else
                    {
                        x_first_value = Math.Truncate(display_x_min);
                        x_step = 1;
                    }
                }
                n_lines = (int)(Math.Truncate((double)display_x_range / (double)x_step)) + 1;
                X_first = (int)(((x_first_value - display_x_min) / display_x_range) * X_range);
 
            
            
                    float x_label = (float) x_first_value;
                    int X_value;
                    String s3;
                    for (i = 0; i < n_lines; i++)
                    {
                        X_value = X_first + (int)(((i * x_step) / (display_x_range)) * X_range);
                        p1.X = X_value; // 0;
                        p1.Y = 0;
                        p2.X = X_value;
                        p2.Y = Y_bottom;
                        g.DrawLine(pen4, p1, p2);
                        p1.X -= (int)(0.015 * X_range);
                        p1.Y = Y_range - (int)(0.05 * Y_range);                        
                        s3 = x_label.ToString("f0");
                        g.DrawString(s3, font, brush, p1);
                        x_label += x_step;
                    }

                    // update the corner labels on the display plot
                    labelTS4.Text = display_y_max.ToString("f1");
                    labelTS5.Text = display_y_min.ToString("f1");
                    labelTS8.Text = display_x_min.ToString("f0");
                    labelTS9.Text = display_x_max.ToString("f0");
                    float time = (float)((time_elapsed / 600d) * (X_right - X_left));
                    float data1 = 0;
                    float data2 = 0;

                    // plot the accumulated data
                    for (i = 1; i <= data_points; i++)
                    {
                        if (button_dBm.Checked)          // log mode
                        {
                            data1 = sig_data[i];
                            data2 = sig_data[i - 1];
                        }
                        else                             // linear mode
                        {
                            if (sig_data[i] == 0)
                            {
                                data1 = 0;
                                data2 = 0;
                            }
                            else
                            {
                                if (sig_data[2] == 0) sig_data[2] = sig_data[i];
                                data1 = 100 * (float)((Math.Pow(10, sig_data[i] / 10)) / ref_power);
                                data2 = 100 * (float)(Math.Pow(10, sig_data[i - 1] / 10) / ref_power);
                            }
                        }

                        p2.Y = (int)(((display_y_max - data1) / (display_y_max - display_y_min)) * Y_range);

                        if (button_linear.Checked)
                        {
                            //display_y_min = (float) manual_ymin.Value;
                            if (manual_ymin.Value < 0) manual_ymin.BackColor = System.Drawing.Color.Yellow;
                            else manual_ymin.BackColor = System.Drawing.SystemColors.Window;

                            if (manual_ymin.Value < 0 | manual_ymax.Value <= manual_ymin.Value)
                            {
                                manual_ymin.BackColor = System.Drawing.Color.Yellow;
                                manual_ymax.BackColor = System.Drawing.SystemColors.Window;
                            }
                            else manual_ymin.BackColor = System.Drawing.SystemColors.Window;
                        }

                        if (p2.Y >= Y_range) p2.Y = Y_range - 1;
                        if (p2.Y <= 0) p2.Y = 1;
                        p2.X = (int)(((time_data[i] - display_x_min)/ (display_x_max - display_x_min) * (X_range)));
                        p1.Y = (int)(((display_y_max - data2) / (display_y_max - display_y_min)) * Y_range);
                        if (p1.Y > Y_range) p1.Y = Y_range - 1;
                        if (p1.Y < 0) p1.Y = 1;
                        p1.X = (int)(((time_data[i - 1] -  display_x_min) / (display_x_max - display_x_min)) * (X_range));
                        g.DrawLine(pen, p1, p2);                        
                    }
                    if (data_points >= 9999)
                    {
                        RA_timer.Enabled = false;
                        s = "Maximum # data points collected...acquisition halted";
                        font = new Font("Arial", 18.0f);
                        brush = new SolidBrush(Color.Red);
                        Point p = new Point(70, 150);
                        g.DrawString(s, font, brush, p);
                        RArecordCheckBox.Checked = false;
                    }
                    rescale = false;
        }

        private void button_dBm_CheckedChanged(object sender, EventArgs e)
        {
            if (button_dBm.Checked)     // log mode selected
            {
                labelTS3.Text = "manual Ymax (dBm)";
                labelTS2.Text = "manual Ymin (dBm)";
                manual_ymax.BackColor = System.Drawing.SystemColors.Window;
                manual_ymin.BackColor = System.Drawing.SystemColors.Window;
                manual_ymax.Value = 0;
                manual_ymin.Value = -150;
            }
            else   // linear mode selected
            {
                labelTS3.Text = "manual Ymax";
                labelTS2.Text = "manual Ymin";
                manual_ymax.Value = 110;
                manual_ymin.Value = 0;
            }
            picRAGraph.Invalidate();

        }

        private void button_linear_CheckedChanged(object sender, EventArgs e)
        {
            s2 = ref_power.ToString("E1");
            picRAGraph.Invalidate();
        }

        private void picRAGraph_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            float power;
            float time;
            power = (float)e.Y / (float)Y_range * (display_y_min - display_y_max) + display_y_max;
            time = (float)e.X / (float)X_range * (display_x_max - display_x_min) + display_x_min;
            if (button_dBm.Checked) txtCursorPower.Text = power.ToString("f1") + " dBm";
            else txtCursorPower.Text = power.ToString("f1") + " x " + s2 + " mW";
            txtCursorTime.Text = time.ToString("f1") + " seconds";
        }

        private void RAForm_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtCursorPower.Text = "";
            txtCursorTime.Text = "";
        }

        private void button_readFile_Click(object sender, EventArgs e)
        {
            String path = Environment.CurrentDirectory;
            openFileDialog3.InitialDirectory = path;
            openFileDialog3.FileName = "RA_data.csv";
            openFileDialog3.Filter = "RA data files (*.csv) | *.csv|All files (*.*)|*.*";
            DialogResult result = openFileDialog3.ShowDialog();
            if (result == DialogResult.OK)
            {
                String file = openFileDialog3.FileName;
                System.IO.StreamReader reader = new System.IO.StreamReader(file);
                int i_max = 0;
                while (!reader.EndOfStream)
                {
                    i_max += 1;
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (i_max > 1)
                    {
                        float.TryParse(values[0], out time_data[i_max]);
                        float.TryParse(values[1], out sig_data[i_max]);
                    }
                }
                reader.Close();
                data_points = i_max - 1;
                         labelTS10.Text = "";
                         labelTS10.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
                         labelTS10.ForeColor = System.Drawing.SystemColors.Highlight;
                         textBox_pts_collected.Text = data_points.ToString("f0");
                         if (data_points > 1) manual_xmax.Value = (decimal)time_data[i_max];
                         else
                         {
                             labelTS10.ForeColor = Color.White;
                             labelTS10.BackColor = Color.Red;
                             labelTS10.Text = "NO VALID DATA IN THE FILE";
                         }
                         sig_max = -140;
                         for (int i = 2; i < data_points; i++) if (sig_data[i] > sig_max & sig_data[i] < 0) sig_max = sig_data[i];
                         picRAGraph.Invalidate();
            }
        }

        private void button_writeFile_Click(object sender, EventArgs e)
        {
            String text_line;
            String path = Environment.CurrentDirectory;
            saveFileDialog1.InitialDirectory = path;
            saveFileDialog1.Filter = "RA data file (*.csv) | *.csv";
            DialogResult result = saveFileDialog1.ShowDialog();
            String file = saveFileDialog1.FileName;
            if (result == DialogResult.OK)
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(file);
                for (int i = 0; i <= data_points; i++)
                {
                    if (i < 1) text_line = "time (seconds), signal (dBm)";
                    else text_line = time_data[i].ToString("f3") + ", " + sig_data[i].ToString("f3");
                    writer.WriteLine(text_line);
                }
                writer.Close();
            }
        }

        private void manual_ymax_ValueChanged(object sender, EventArgs e)
        {
            if (manual_ymax.Value <= manual_ymin.Value) manual_ymax.Value = manual_ymin.Value + 1;
            display_y_max = (float)manual_ymax.Value;
            picRAGraph.Invalidate();
        }

        private void manual_ymin_ValueChanged(object sender, EventArgs e)
        {
            if (button_linear.Checked & manual_ymin.Value < 0) manual_ymin.Value = 0;
            if (manual_ymin.Value >= manual_ymax.Value) manual_ymin.Value = manual_ymax.Value - 1;
            display_y_min = (float)manual_ymin.Value;
            picRAGraph.Invalidate();
        }

        private void manual_xmax_ValueChanged(object sender, EventArgs e)
        {
            if (manual_xmax.Value <= manual_xmin.Value) manual_xmax.Value = manual_xmin.Value + 1;
            display_x_max = (float)manual_xmax.Value;
            picRAGraph.Invalidate();
        }

        private void manual_xmin_ValueChanged(object sender, EventArgs e)
        {
            if (manual_xmin.Value >= manual_xmax.Value) manual_xmin.Value = manual_xmax.Value - 1;
            display_x_min = (float)manual_xmin.Value;
            picRAGraph.Invalidate();
        }

        private byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        private void RAForm_Load(object sender, EventArgs e)
        {

        }
    }
}
