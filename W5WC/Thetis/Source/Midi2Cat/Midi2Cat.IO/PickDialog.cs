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
    public partial class PickDialog : Form
    {
        public PickDialog()
        {
            InitializeComponent();
        }

        public string Prompt
        {
            set
            {
                promptLabel.Text = value;
            }
        }

        public string[] Mappings
        {
            get
            {
                List<string> results = new List<string>();
                foreach (var item in mappingsLB.CheckedItems)
                {
                    results.Add((string)item);
                }
                return results.ToArray();
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

        private void mappingsLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int count=mappingsLB.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked)
                count++;
            else
                count--;
            doneButton.Enabled = count > 0;
        }
                   
    }
}
