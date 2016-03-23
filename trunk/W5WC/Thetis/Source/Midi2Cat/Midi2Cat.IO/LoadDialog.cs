using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Midi2Cat.IO
{
    public partial class LoadDialog : Form
    {
        public LoadDialog()
        {
            InitializeComponent();
        }

        public string MappingName
        {
            get
            {
                return textBox1.Text;
            }
        }

        public string[] ExistingMappings
        {
            set
            {
                mappingsLB.Items.Clear();
                foreach (string mapping in value)
                {
                    mappingsLB.Items.Add(mapping);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {           
            loadButton.Enabled = (textBox1.Text.Trim().Length > 0);
        }

        private void mappingsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = (string)mappingsLB.SelectedItem;
        }

        private void mappingsLB_DoubleClick(object sender, EventArgs e)
        {
            if (mappingsLB.SelectedItem != null)
            {
                textBox1.Text = (string)mappingsLB.SelectedItem;
                this.DialogResult = DialogResult.OK;
            }
        }        
    }
}
