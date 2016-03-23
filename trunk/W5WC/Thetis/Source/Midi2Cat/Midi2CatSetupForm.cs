using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Midi2Cat.IO;

namespace Thetis.Midi2Cat
{
    public partial class Midi2CatSetupForm : Form
    {
        private string DbFile;
        private List<MidiDeviceSetup> Setups;
        int startDelay = 5;
        int MsgToShow = 0;
        // The messages are spoof... we need all the time to ensure the consoles are synchronised with setup, otherwise bad things happen...
        string[] startupMessages = {"Closing Thetis Midi Input","Closing Thetis Midi Output","Attaching Midi Input and Output to Midi Setup","Midi Setup is opening Midi input and output", "Synchronising Console Controls with Midi Setup", "Initialising Database."};

        public Midi2CatSetupForm(string DbFile)
        {
            this.DbFile = DbFile;
            InitializeComponent();
        }

        private void Midi2CatSetupForm_Load(object sender, EventArgs e)
        {
            startTimer.Enabled = true;
            LoadSetup();
        }


        private void LoadSetup()
        {
            Setups = new List<MidiDeviceSetup>();
            MidiDevices devices = new MidiDevices();
            int idx = 0;
            foreach (var InDevice in devices.InDevices)
            {
                MidiDeviceSetup ctrl = new MidiDeviceSetup(DbFile, InDevice, idx++);
                Setups.Add(ctrl);
                TabPage page = new TabPage(InDevice);
                page.Controls.Add(ctrl);
                ctrl.Dock= DockStyle.Fill;
                devicesTabControl.TabPages.Add(page);
            }
        }

        private void Midi2CatSetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (MidiDeviceSetup setup in Setups)
            {
                setup.Parent = null;
                setup.Dispose();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            if (startDelay >= 0)
            {
                progressLabel.Text = startupMessages[MsgToShow++];
                startDelay--;
            }
            else
            {
                startTimer.Enabled = false;
                startupPanel.Visible = false;
                devicesTabControl.Visible = true;
            }
        }

    }
}
