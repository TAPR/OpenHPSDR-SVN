//=====================================================================
// DJConsoleSelect.cs
//=====================================================================
// Copyright (C) 2013 Tobias Wellnitz EA4/DH1TW
// Copyright (C) 2013 Doug Wigley W5WC 
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//======================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Thetis.DJConsoleUI
{
    public partial class DJConsoleSelect : Form
    {
        private Console m_parent;

        private DJConsoleMK2Config ConfigWindowMK2;
        private DJConsoleMP3e2Config ConfigWindowMP3e2;
        private DJConsoleMP3LEConfig ConfigWindowMP3LE;

        public DJConsoleSelect(Console console)
        {
            m_parent = console;
            InitializeComponent(); 
            Init();
        }

        private void Init()
        {
            if (m_parent.DJConsoleObj.connectedConsoles.Count > 0)
            {
                cbConsoleSelect.DataSource = new BindingSource(m_parent.DJConsoleObj.connectedConsoles, null);
                cbConsoleSelect.DisplayMember = "Value";
                cbConsoleSelect.ValueMember = "Key";
                this.cbConsoleSelect.SelectedIndexChanged += new System.EventHandler(this.cbConsoleSelect_SelectedIndexChanged);
                //                cbConsoleSelect.SelectedValue = m_parent.DJConsoleObj.SelectedConsole;
                this.Show();
            }
            else
            {
                MessageBox.Show("Sorry, no compatible device detected", "Error");
                this.Dispose();
            }

            //if (m_parent.DJConsoleObj.connectedConsoles.Count > 0)
            //{
            //    cbConsoleSelect.DataSource = new BindingSource(m_parent.DJConsoleObj.connectedConsoles, null);
            //    cbConsoleSelect.DisplayMember = "Value";
            //    cbConsoleSelect.ValueMember = "Key";
            //    this.cbConsoleSelect.SelectedIndexChanged += new System.EventHandler(this.cbConsoleSelect_SelectedIndexChanged);
            //    //cbConsoleSelect.Items.Contains(m_parent.DJConsoleObj.SelectedConsole);
            //    cbConsoleSelect.SelectedValue= m_parent.DJConsoleObj.SelectedConsole;
            //    this.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Sorry, no compatible device detected", "Error");
            //    this.Dispose();
            //}
        }


        private void btnConfigure_Click(object sender, EventArgs e)
        {

            if (m_parent.DJConsoleObj.SelectedConsole == 0)
            {
                if (ConfigWindowMP3e2 == null)
                {
                    ConfigWindowMP3e2 = new DJConsoleMP3e2Config(m_parent);
                    ConfigWindowMP3e2.Show();
                    ConfigWindowMP3e2.Focus();
                    ConfigWindowMP3e2.FormClosed += new FormClosedEventHandler(ConfigWindowMP3e2Closed);
                }
                return;
            }


            if (m_parent.DJConsoleObj.SelectedConsole == 1)
            {
                if (ConfigWindowMK2 == null)
                {
                    ConfigWindowMK2 = new DJConsoleMK2Config(m_parent);
                    ConfigWindowMK2.Show();
                    ConfigWindowMK2.Focus();
                    ConfigWindowMK2.FormClosed += new FormClosedEventHandler(ConfigWindowMK2Closed);
                }
                return;
            }

            if (m_parent.DJConsoleObj.SelectedConsole == 2)
            {
                if (ConfigWindowMP3LE == null)
                {
                    ConfigWindowMP3LE = new DJConsoleMP3LEConfig(m_parent);
                    ConfigWindowMP3LE.Show();
                    ConfigWindowMP3LE.Focus();
                    ConfigWindowMP3LE.FormClosed += new FormClosedEventHandler(ConfigWindowMP3LEClosed);
                }
                return;

            }

            else
            {
                MessageBox.Show("Please select a Console", "Error");
            }


            //if ((m_parent.DJConsoleObj.SelectedConsole == 2) | (m_parent.DJConsoleObj.SelectedConsole == 3))
            //{
            //    if (ConfigWindowMK2 == null)
            //    {
            //        ConfigWindowMK2 = new DJConsoleMK2Config(m_parent);
            //        ConfigWindowMK2.Show();
            //        ConfigWindowMK2.Focus();
            //        ConfigWindowMK2.FormClosed += new FormClosedEventHandler(ConfigWindowMK2Closed);
            //    }
            //    return;
            //}

            //if ((m_parent.DJConsoleObj.SelectedConsole == 0) | (m_parent.DJConsoleObj.SelectedConsole == 1))
            //{
            //    if (ConfigWindowMP3e2 == null)
            //    {
            //        ConfigWindowMP3e2 = new DJConsoleMP3e2Config(m_parent);
            //        ConfigWindowMP3e2.Show();
            //        ConfigWindowMP3e2.Focus();
            //        ConfigWindowMP3e2.FormClosed += new FormClosedEventHandler(ConfigWindowMP3e2Closed);
            //    }
            //    return;
            //}

            //else
            //{
            //    MessageBox.Show("Please select a Console","Error");
            //}

        }

        private void ConfigWindowMK2Closed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMK2 != null)
            {
                ConfigWindowMK2 = null;
            }
        }

        private void ConfigWindowMP3e2Closed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMP3e2 != null)
            {
                ConfigWindowMP3e2 = null;
            }
        }

        private void ConfigWindowMP3LEClosed(object sender, FormClosedEventArgs e)
        {
            if (ConfigWindowMP3e2 != null)
            {
                ConfigWindowMP3e2 = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbConsoleSelect.SelectedItem != null)
            {
                m_parent.DJConsoleObj.SelectedConsole = (int)cbConsoleSelect.SelectedValue;
                m_parent.DJConsoleObj.Reload();
            }
            this.Close();
        }

        private void cbConsoleSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbConsoleSelect.SelectedItem != null)
            {
                m_parent.DJConsoleObj.SelectedConsole = (int)cbConsoleSelect.SelectedValue;
                m_parent.DJConsoleObj.Reload();
            }
        }


    }
}
