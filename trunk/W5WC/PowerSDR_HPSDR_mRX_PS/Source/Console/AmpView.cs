﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PowerSDR
{
    public unsafe partial class AmpView : Form
    {
        private PSForm psform;
        public AmpView(PSForm ps)
        {
            InitializeComponent();
            psform = ps;
        }

        GCHandle hx, hym, hyc, hys, hcm, hcc, hcs;
        const int ints = 16;
        const int spi = 256;
        const int np = 512;
        double[] x  = new double[ints * spi];
        double[] ym = new double[ints * spi];
        double[] yc = new double[ints * spi];
        double[] ys = new double[ints * spi];
        double[] cm = new double[4 * ints];
        double[] cc = new double[4 * ints];
        double[] cs = new double[4 * ints];
        double[] t  = new double[ints + 1];
        int skip = 1;
        bool showgain = false;


        private void AmpView_Load(object sender, EventArgs e)
        {
            Common.RestoreForm(this, "AmpView", false);
            hx  = GCHandle.Alloc(x,  GCHandleType.Pinned);
            hym = GCHandle.Alloc(ym, GCHandleType.Pinned);
            hyc = GCHandle.Alloc(yc, GCHandleType.Pinned);
            hys = GCHandle.Alloc(ys, GCHandleType.Pinned);
            hcm = GCHandle.Alloc(cm, GCHandleType.Pinned);
            hcc = GCHandle.Alloc(cc, GCHandleType.Pinned);
            hcs = GCHandle.Alloc(cs, GCHandleType.Pinned);
            double delta = 1.0 / (double)ints;
            t[0] = 0.0;
            for (int i = 1; i <= ints; i++)
                t[i] = t[i - 1] + delta;
            EventArgs ex = EventArgs.Empty;
            chkAVShowGain_CheckedChanged(this, ex);
            chkAVLowRes_CheckedChanged(this, ex);
            chkAVPhaseZoom_CheckedChanged(this, ex);
        }

        private void disp_setup()
        {
            chart1.ChartAreas[0].AxisX.Minimum = 0.0;
            chart1.ChartAreas[0].AxisX.Maximum = 1.0;
            chart1.ChartAreas[0].AxisY.Minimum = 0.0;
            chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.LightSalmon;
            chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.LightSalmon;
            chart1.ChartAreas[0].AxisY2.LabelStyle.ForeColor = Color.LightSalmon;
            chart1.ChartAreas[0].AxisX.Title = "Input Magnitude";
            chart1.ChartAreas[0].AxisX.TitleForeColor = Color.LightSalmon;
            chart1.ChartAreas[0].AxisY.TitleForeColor = Color.LightSalmon;
            chart1.ChartAreas[0].AxisY2.Title = "Phase";
            chart1.ChartAreas[0].AxisY2.TitleForeColor = Color.LightSalmon;
        }

        private void disp_data()
        {
            double delta = 1.0 / (double)np;
            double qx = delta;
            double dx;
            double qym, qyc, qys, phs;
            double phs_base;
            int k;
            chart1.Series["Ref"].Points.Clear();
            chart1.Series["MagCorr"].Points.Clear();
            chart1.Series["PhsCorr"].Points.Clear();
            chart1.Series["MagAmp"].Points.Clear();
            chart1.Series["PhsAmp"].Points.Clear();
            if (!showgain)
            {
                chart1.Series["Ref"].Points.AddXY(0.0, 0.0);
                chart1.Series["Ref"].Points.AddXY(1.0, 1.0);
                chart1.Series["Ref"].Points.AddXY(1.0, 0.5);
                chart1.Series["Ref"].Points.AddXY(0.0, 0.5);
            }
            else
            {
                chart1.Series["Ref"].Points.AddXY(0.0, 1.0);
                chart1.Series["Ref"].Points.AddXY(1.0, 1.0);
            }
            chart1.Series["MagCorr"].Points.AddXY(0.0, 0.0);
            k = ints - 1;
            dx = t[ints] - t[ints - 1];
            qyc = cc[4 * k + 0] + dx * (cc[4 * k + 1] + dx * (cc[4 * k + 2] + dx * cc[4 * k + 3]));
            qys = cs[4 * k + 0] + dx * (cs[4 * k + 1] + dx * (cs[4 * k + 2] + dx * cs[4 * k + 3]));
            phs_base = 180.0 / Math.PI * Math.Atan2(qys, qyc);
            for (int i = 1; i <= np; i++)
            {
                if ((k = (int)(qx * ints)) > ints - 1) k = ints - 1;
                dx = qx - t[k];
                qym = cm[4 * k + 0] + dx * (cm[4 * k + 1] + dx * (cm[4 * k + 2] + dx * cm[4 * k + 3]));
                qyc = cc[4 * k + 0] + dx * (cc[4 * k + 1] + dx * (cc[4 * k + 2] + dx * cc[4 * k + 3]));
                qys = cs[4 * k + 0] + dx * (cs[4 * k + 1] + dx * (cs[4 * k + 2] + dx * cs[4 * k + 3]));
                if (!showgain)
                    chart1.Series["MagCorr"].Points.AddXY(qx, qym * qx);
                else
                    chart1.Series["MagCorr"].Points.AddXY(qx, qym);
                phs = 180.0 / Math.PI * Math.Atan2(qys, qyc) - phs_base;
                if (phs > -180.0 && phs < +180.0)
                    chart1.Series["PhsCorr"].Points.AddXY(qx, phs);
                qx += delta;
            }
            k = ints * spi - 1;
            phs_base = 180.0 / Math.PI * Math.Atan2(yc[k], ys[k]);
            for (int i = 0; i < ints * spi; i += skip)
            {
                if (!showgain)
                    chart1.Series["MagAmp"].Points.AddXY(ym[i] * x[i], x[i]);
                else
                    chart1.Series["MagAmp"].Points.AddXY(ym[i] * x[i], 1.0 / ym[i]);
                phs = 180.0 / Math.PI * Math.Atan2(yc[i], ys[i]) - phs_base;
                chart1.Series["PhsAmp"].Points.AddXY(x[i], phs);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (psform.DismissAmpv)
            {
                Common.SaveForm(this, "AmpView");
                Application.ExitThread();
            }
            disp_setup();
            puresignal.GetPSDisp(wdsp.id(1, 0), 
                hx.AddrOfPinnedObject(),
                hym.AddrOfPinnedObject(),
                hyc.AddrOfPinnedObject(),
                hys.AddrOfPinnedObject(),
                hcm.AddrOfPinnedObject(), 
                hcc.AddrOfPinnedObject(), 
                hcs.AddrOfPinnedObject());
            disp_data();
            if (psform.DismissAmpv)
            {
                Common.SaveForm(this, "AmpView");
                Application.ExitThread();
            }
        }

        private void chkAVShowGain_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAVShowGain.Checked)
            {
                chart1.ChartAreas[0].AxisY.Title = "Gain";
                chart1.ChartAreas[0].AxisY.Maximum = 2.0;
                chart1.Series["MagCorr"].LegendText = "Gain Corr";
                chart1.Series["_magamp"].LegendText = "Gain Amp";
                showgain = true;
            }
            else
            {
                chart1.ChartAreas[0].AxisY.Title = "Magnitude";
                chart1.ChartAreas[0].AxisY.Maximum = 1.0;
                chart1.Series["MagCorr"].LegendText = "Mag Corr";
                chart1.Series["_magamp"].LegendText = "Mag Amp";
                showgain = false;
            }
        }

        private void chkAVLowRes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAVLowRes.Checked)
                skip = 4;
            else
                skip = 1;
        }

        private void AmpView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Common.SaveForm(this, "AmpView");
        }

        private void chkAVPhaseZoom_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAVPhaseZoom.Checked)
            {
                chart1.ChartAreas[0].AxisY2.Minimum = -45.0;
                chart1.ChartAreas[0].AxisY2.Maximum = +45.0;
            }
            else
            {
                chart1.ChartAreas[0].AxisY2.Minimum = -180.0;
                chart1.ChartAreas[0].AxisY2.Maximum = +180.0;
            }
        }
    }
}
