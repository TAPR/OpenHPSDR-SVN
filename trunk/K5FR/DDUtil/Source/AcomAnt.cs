using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataDecoder.Properties;

namespace DataDecoder
{
    public partial class AcomAnt : Form
    {
        Settings set = Settings.Default;

        public AcomAnt()
        {
            InitializeComponent();
            num1.Value = set.asw1;
            num2.Value = set.asw2;
            num3.Value = set.asw3;
            num4.Value = set.asw4;
            num5.Value = set.asw5;
            num6.Value = set.asw6;
            num7.Value = set.asw7;
            num8.Value = set.asw8;
            num9.Value = set.asw9;
            num10.Value = set.asw10;
            chkAntEnab.Checked = set.ChkAntEnab;
        }

        private void num1_ValueChanged(object sender, EventArgs e)
        {
            set.asw1 = num1.Value; set.Save();
        }

        private void num2_ValueChanged(object sender, EventArgs e)
        {
            set.asw2 = num2.Value; set.Save();
        }

        private void num3_ValueChanged(object sender, EventArgs e)
        {
            set.asw3 = num3.Value; set.Save();
        }

        private void num4_ValueChanged(object sender, EventArgs e)
        {
            set.asw4 = num4.Value; set.Save();
        }

        private void num5_ValueChanged(object sender, EventArgs e)
        {
            set.asw5 = num5.Value; set.Save();
        }

        private void num6_ValueChanged(object sender, EventArgs e)
        {
            set.asw6 = num6.Value; set.Save();
        }

        private void num7_ValueChanged(object sender, EventArgs e)
        {
            set.asw7 = num7.Value; set.Save();
        }

        private void num8_ValueChanged(object sender, EventArgs e)
        {
            set.asw8 = num8.Value; set.Save();
        }

        private void num9_ValueChanged(object sender, EventArgs e)
        {
            set.asw9 = num9.Value; set.Save();
        }

        private void num10_ValueChanged(object sender, EventArgs e)
        {
            set.asw10 = num10.Value; set.Save();
        }

        private void chkAntEnab_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAntEnab.Checked) set.ChkAntEnab = true;
            else set.ChkAntEnab = false;
            set.Save();
        }
    }
}
