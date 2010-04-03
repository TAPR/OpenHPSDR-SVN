using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataDecoder
{
    public partial class SetupWiz : Form
    {
        public SetupWiz()
        {
            InitializeComponent();
        }

        private void wizardControl1_FinishButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}