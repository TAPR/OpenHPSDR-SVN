using Midi2Cat.Data;
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
    public partial class OrganiseDialog : Form
    {
        Midi2CatDatabase DB = null;
        string DeviceName = null;

        public OrganiseDialog(Midi2CatDatabase DB,string DeviceName)
        {
            this.DB = DB;
            this.DeviceName = DeviceName;
            InitializeComponent();
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

        private void mappingsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            EndRename();
            if (mappingsLB.SelectedIndex >= 0)
            {
                renameButton.Enabled = true;
                deleteButton.Enabled = true;
            }
            else
            {
                renameButton.Enabled = false;
                deleteButton.Enabled = false;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if ( mappingsLB.SelectedItem!=null)
            {
                DB.RemoveSavedMapping(DeviceName, ((string)mappingsLB.SelectedItem));
                ExistingMappings = DB.GetSavedMappings();
            }
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            StartRename();
        }

        private void mappingsLB_DoubleClick(object sender, EventArgs e)
        {
            StartRename();
        }

        private void StartRename()
        {
            renameButton.Enabled = false;
            deleteButton.Enabled = false;
            if (mappingsLB.SelectedItem != null)
            {
                mappingsLB.BackColor = Color.Silver;
                renameTB.Left = mappingsLB.Left + 4;
                renameTB.Width = mappingsLB.Width-6;
                renameTB.Top = mappingsLB.Top + ((mappingsLB.SelectedIndex - mappingsLB.TopIndex) * mappingsLB.ItemHeight) + 2;
                renameTB.Text = ((string)mappingsLB.SelectedItem);
                renameTB.Visible = true;
                renameTB.Focus();
                renameTB.SelectAll();
            }
        }

        private void EndRename()
        {
            renameTB.Visible = false;
            mappingsLB.BackColor = Color.White;

        }

        private void renameTB_Leave(object sender, EventArgs e)
        {
            EndRename();
        }

        private void renameTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                EndRename();
            }
            else if (e.KeyCode == Keys.Return)
            {
                string OldName=((string)mappingsLB.SelectedItem);
                string NewName = renameTB.Text.Trim();
                if ( NewName.Length> 0 && OldName.ToLower() != NewName.ToLower())
                {
                    if (DB.GetSavedMappings().Contains(NewName) == false)
                    {
                        DB.RenameSavedMapping(DeviceName, OldName, NewName);
                    }
                }
                EndRename();
                ExistingMappings = DB.GetSavedMappings();
            }
        }
      

    }
}
