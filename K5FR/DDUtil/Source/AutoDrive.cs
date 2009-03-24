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
    public partial class AutoDrive : Form
    {
        Settings set = Settings.Default;

        public AutoDrive()
        {
            InitializeComponent();
            num1.Value = set.def1;
            num2.Value = set.def2;
            num3.Value = set.def3;
            num4.Value = set.def4;
            num5.Value = set.def5;
            num6.Value = set.def6;
            num7.Value = set.def7;
            num8.Value = set.def8;
            num9.Value = set.def9;
        }

        private void num1_ValueChanged(object sender, EventArgs e)
        {
            set.def1 = num1.Value; set.Save();
        }

        private void num2_ValueChanged(object sender, EventArgs e)
        {
            set.def2 = num2.Value; set.Save();
        }

        private void num3_ValueChanged(object sender, EventArgs e)
        {
            set.def3 = num3.Value; set.Save();
        }

        private void num4_ValueChanged(object sender, EventArgs e)
        {
            set.def4 = num4.Value; set.Save();
        }

        private void num5_ValueChanged(object sender, EventArgs e)
        {
            set.def5 = num5.Value; set.Save();
        }

        private void num6_ValueChanged(object sender, EventArgs e)
        {
            set.def6 = num6.Value; set.Save();
        }

        private void num7_ValueChanged(object sender, EventArgs e)
        {
            set.def7 = num7.Value; set.Save();
        }

        private void num8_ValueChanged(object sender, EventArgs e)
        {
            set.def8 = num8.Value; set.Save();
        }

        private void num9_ValueChanged(object sender, EventArgs e)
        {
            set.def9 = num9.Value; set.Save();
        }

    }
}
