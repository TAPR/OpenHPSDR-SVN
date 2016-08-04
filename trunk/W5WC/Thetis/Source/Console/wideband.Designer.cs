namespace Thetis
{
    partial class wideband
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelwbDisplay = new System.Windows.Forms.PanelTS();
            this.wbdisplay = new Thetis.wbDisplay();
            this.panelwbDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wbdisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // panelwbDisplay
            // 
            this.panelwbDisplay.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.panelwbDisplay.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.panelwbDisplay.BackColor = System.Drawing.Color.Transparent;
            this.panelwbDisplay.Controls.Add(this.wbdisplay);
            this.panelwbDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelwbDisplay.Location = new System.Drawing.Point(0, 0);
            this.panelwbDisplay.Name = "panelwbDisplay";
            this.panelwbDisplay.Size = new System.Drawing.Size(801, 302);
            this.panelwbDisplay.TabIndex = 0;
            // 
            // wbdisplay
            // 
            this.wbdisplay.ADC = 0;
            this.wbdisplay.AGCHang = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.wbdisplay.AGCKnee = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.wbdisplay.AlexPreampOffset = 0F;
            this.wbdisplay.AverageOn = true;
            this.wbdisplay.AvTau = 0.12D;
            this.wbdisplay.BackColor = System.Drawing.Color.Black;
            this.wbdisplay.BandEdgeColor = System.Drawing.Color.Red;
            this.wbdisplay.ClickTuneFilter = true;
            this.wbdisplay.ColorSheme = Thetis.ColorSheme.enhanced;
            this.wbdisplay.cOnsole = null;
            this.wbdisplay.CurrentClickTuneMode = Thetis.ClickTuneMode.Off;
            this.wbdisplay.CurrentDisplayMode = Thetis.DisplayMode.PANADAPTER;
            this.wbdisplay.CurrentModel = Thetis.Model.HERMES;
            this.wbdisplay.CurrentRegion = Thetis.FRSRegion.US;
            this.wbdisplay.CWPitch = 600;
            this.wbdisplay.DataLineColor = System.Drawing.Color.White;
            this.wbdisplay.DataReady = false;
            this.wbdisplay.DBMScalePanRect = new System.Drawing.Rectangle(65, 0, 35, 30);
            this.wbdisplay.DisplayAGCHangLine = true;
            this.wbdisplay.DisplayAvgBlocks = 5;
            this.wbdisplay.DisplayBackgroundColor = System.Drawing.Color.Black;
            this.wbdisplay.DisplayCursorX = 0;
            this.wbdisplay.DisplayCursorY = 0;
            this.wbdisplay.DisplayDuplex = false;
            this.wbdisplay.DisplayFilterColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.wbdisplay.DisplayLabelAlign = Thetis.DisplayLabelAlignment.LEFT;
            this.wbdisplay.DisplayLineWidth = 1F;
            this.wbdisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbdisplay.FFTSize = 4096;
            this.wbdisplay.FilterRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.wbdisplay.FrameRate = 15;
            this.wbdisplay.FREQ = 0D;
            this.wbdisplay.FreqDiff = 0;
            this.wbdisplay.FreqRulerPosition = 1F;
            this.wbdisplay.FreqScalePanRect = new System.Drawing.Rectangle(0, 282, 801, 20);
            this.wbdisplay.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.wbdisplay.GridControl = true;
            this.wbdisplay.GridPenDark = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.wbdisplay.GridTextColor = System.Drawing.Color.Yellow;
            this.wbdisplay.GridZeroColor = System.Drawing.Color.Red;
            this.wbdisplay.HGridColor = System.Drawing.Color.White;
            this.wbdisplay.HighSWR = false;
            this.wbdisplay.KaiserPi = 14D;
            this.wbdisplay.LinCor = 2;
            this.wbdisplay.LinLogCor = -14;
            this.wbdisplay.Location = new System.Drawing.Point(0, 0);
            this.wbdisplay.MaxX = 0F;
            this.wbdisplay.MaxY = 0F;
            this.wbdisplay.MOX = false;
            this.wbdisplay.Name = "wbdisplay";
            this.wbdisplay.NReceivers = 2;
            this.wbdisplay.PanFill = true;
            this.wbdisplay.PanFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(127)))));
            this.wbdisplay.PanRect = new System.Drawing.Rectangle(0, 0, 801, 282);
            this.wbdisplay.PanSlider = 0.5D;
            this.wbdisplay.PeakOn = false;
            this.wbdisplay.Pixels = 801;
            this.wbdisplay.PreampOffset = 0F;
            this.wbdisplay.ReverseWaterfall = false;
            this.wbdisplay.RIT = 0;
            this.wbdisplay.RX1HangSpectrumLine = true;
            this.wbdisplay.RXDisplayCalOffset = -40.1F;
            this.wbdisplay.RXDSPMode = Thetis.DSPMode.USB;
            this.wbdisplay.RXFFTSizeOffset = 0F;
            this.wbdisplay.RXFilterHigh = 0;
            this.wbdisplay.RXFilterLow = 0;
            this.wbdisplay.SampleRate = 122880000;
            this.wbdisplay.SecScalePanRect = new System.Drawing.Rectangle(0, 302, 45, 0);
            this.wbdisplay.ShowAGC = true;
            this.wbdisplay.ShowCTHLine = false;
            this.wbdisplay.ShowCWZeroLine = false;
            this.wbdisplay.ShowFreqOffset = false;
            this.wbdisplay.Size = new System.Drawing.Size(801, 302);
            this.wbdisplay.SpectrumGridMax = -50;
            this.wbdisplay.SpectrumGridMin = -170;
            this.wbdisplay.SpectrumGridStep = 10;
            this.wbdisplay.SpectrumLine = true;
            this.wbdisplay.SplitDisplay = false;
            this.wbdisplay.SplitEnabled = false;
            this.wbdisplay.SubRX1Enabled = false;
            this.wbdisplay.SubRXFilterColor = System.Drawing.Color.Blue;
            this.wbdisplay.SubRXZeroLine = System.Drawing.Color.LightSkyBlue;
            this.wbdisplay.TabIndex = 0;
            this.wbdisplay.TabStop = false;
            this.wbdisplay.Target = null;
            this.wbdisplay.TopSize = 0;
            this.wbdisplay.VFOASub = ((long)(0));
            this.wbdisplay.VFOHz = ((long)(10000000));
            this.wbdisplay.WaterfallAGC = false;
            this.wbdisplay.WaterfallAvgBlocks = 18;
            this.wbdisplay.WaterfallDataReady = false;
            this.wbdisplay.WaterfallHighColor = System.Drawing.Color.Yellow;
            this.wbdisplay.WaterfallHighThreshold = -80F;
            this.wbdisplay.WaterfallLowColor = System.Drawing.Color.Black;
            this.wbdisplay.WaterfallLowThreshold = -130F;
            this.wbdisplay.WaterfallMidColor = System.Drawing.Color.Red;
            this.wbdisplay.WaterfallRect = new System.Drawing.Rectangle(0, 302, 801, 1);
            this.wbdisplay.WaterfallUpdatePeriod = 100;
            this.wbdisplay.WindowType = 6;
            this.wbdisplay.ZoomSlider = 0D;
            // 
            // wideband
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 302);
            this.Controls.Add(this.panelwbDisplay);
            this.Name = "wideband";
            this.Text = "wideband";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.wideband_FormClosing);
            this.Resize += new System.EventHandler(this.wideband_Resize);
            this.panelwbDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wbdisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PanelTS panelwbDisplay;
        private wbDisplay wbdisplay;
    }
}