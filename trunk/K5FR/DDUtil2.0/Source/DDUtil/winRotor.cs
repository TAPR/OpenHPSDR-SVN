using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataDecoder.Properties;

namespace DataDecoder
{
    public partial class winRotor : Form
    {
        #region Vars & Init
        Setup s;
        public bool init;
        Settings set = Settings.Default;
        public string PFfile = "";

        public winRotor(Setup setup)
        {
            InitializeComponent();
            s = setup;
            // Restore window size and position 
            // this is the default
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.WindowsDefaultBounds;

            // check if the saved bounds are nonzero and visible on any screen
            if (set.WindowPositionwinRotor != Rectangle.Empty &&
                IsVisibleOnAnyScreen(set.WindowPositionwinRotor))
            {
                // first set the bounds
                this.StartPosition = FormStartPosition.Manual;
                this.DesktopBounds = set.WindowPositionwinRotor;

                // afterwards set the window state to the saved value (which could be Maximized)
                this.WindowState = set.WindowStatewinRotor;
            }
            else
            {
                // this resets the upper left corner of the window to windows standards
                this.StartPosition = FormStartPosition.WindowsDefaultLocation;

                // we can still apply the saved size
                if (set.WindowPositionwinRotor != Rectangle.Empty)
                {
                    this.Size = set.WindowPositionwinRotor.Size;
                }
            }
            windowInitialized = true;
        }

        #endregion Vars & Init

        #region Events

        // Rotor button was pressed
        public void btnSP_Click(object sender, EventArgs e)
        {
//            txtSP.Focus();
            if (txtSP.Text == null || txtSP.Text == "")
            {
                if (lblSP.Text != "")
                    txtSP.Text = lblSP.Text;
                else
                {
                    MessageBox.Show("A heading is required to use this command.");
                    return;
                }
            }
            s.lblSP.Text = txtSP.Text;
            s.TurnRotor(txtSP.Text);
            rbPre1.Checked = false;
            rbPre2.Checked = false;
            rbPre3.Checked = false;
            rbPre4.Checked = false;
            rbPre5.Checked = false;
            rbPre6.Checked = false;
            rbPre7.Checked = false;
            rbPre8.Checked = false;
            rbPre9.Checked = false;
            rbPre10.Checked = false;
            rbPre11.Checked = false;
            rbPre12.Checked = false;
            rbPre13.Checked = false;
            rbPre14.Checked = false;
            rbPre15.Checked = false;
            rbPre16.Checked = false;
        }
        // the abort button was pressed
        private void btnStop_Click(object sender, EventArgs e)
        {
            s.RotorStop();
        }
        // the rotor heading window was double-clicked
        private void txtSP_DoubleClick(object sender, EventArgs e)
        {
            s.ToggleRtrEnab();
        }
        // one of the SteppIR buttons has changed
        private void grpStepCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (init)
            {
                if (sender == rbFwd) s.StepFwd = true;
                else if (sender == rb180) s.StepRev = true;
                else if (sender == rbBiDir) s.StepBI = true;
            }
            init = true;
        }
        // form is closed
        private void winRotor_FormClosing(object sender, FormClosedEventArgs e)
        {
            //       this.Close();
        }
        // form is closing
        private void winRotor_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
 //           set.miniGeo = DataDecoder.Setup.GeometryToString(this);
            e.Cancel = true;
            this.Hide();
        }
        private void winRotor_FormClosing(object sender, FormClosingEventArgs e)
        {
            s.WindowState = FormWindowState.Normal;
        }
        // One of the rotor presets has been pressed
        private void grpPreset_CheckChanged(object sender, EventArgs e)
        {
            if (rbPre1.Checked)
            { txtSP.Text = rbPre1.Text; btnSP_Click(null, null); }
            else if (rbPre2.Checked)
            { txtSP.Text = rbPre2.Text; btnSP_Click(null, null); }
            else if (rbPre3.Checked)
            { txtSP.Text = rbPre3.Text; btnSP_Click(null, null); }
            else if (rbPre4.Checked)
            { txtSP.Text = rbPre4.Text; btnSP_Click(null, null); }
            else if (rbPre5.Checked)
            { txtSP.Text = rbPre5.Text; btnSP_Click(null, null); }
            else if (rbPre6.Checked)
            { txtSP.Text = rbPre6.Text; btnSP_Click(null, null); }
            else if (rbPre7.Checked)
            { txtSP.Text = rbPre7.Text; btnSP_Click(null, null); }
            else if (rbPre8.Checked)
            { txtSP.Text = rbPre8.Text; btnSP_Click(null, null); }
            else if (rbPre9.Checked)
            { txtSP.Text = rbPre9.Text; btnSP_Click(null, null); }
            else if (rbPre10.Checked)
            { txtSP.Text = rbPre10.Text; btnSP_Click(null, null); }
            else if (rbPre11.Checked)
            { txtSP.Text = rbPre11.Text; btnSP_Click(null, null); }
            else if (rbPre12.Checked)
            { txtSP.Text = rbPre12.Text; btnSP_Click(null, null); }
            else if (rbPre13.Checked)
            { txtSP.Text = rbPre13.Text; btnSP_Click(null, null); }
            else if (rbPre14.Checked)
            { txtSP.Text = rbPre14.Text; btnSP_Click(null, null); }
            else if (rbPre15.Checked)
            { txtSP.Text = rbPre15.Text; btnSP_Click(null, null); }
            else if (rbPre16.Checked)
            { txtSP.Text = rbPre16.Text; btnSP_Click(null, null); }
        }
        // see if a short-cut key was pressed
        private void winRotor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.Down)
            {
                if (txtSP.Text == null || txtSP.Text == "")
                {
                    if (lblSP.Text != "")
                        txtSP.Text = lblSP.Text;
                    else
                    {
                        MessageBox.Show("A heading is required to use this command.");
                        return;
                    }
                }
                int i = (int)Math.Round(Convert.ToDecimal(txtSP.Text));
                if (i >= 5 && i <= 360)
                {
                    i -= 5;
                }
                else if ( i < 5)
                { i = (5 - i) + (360 - i); }

                txtSP.Text = i.ToString();
                btnSP_Click(null, null);
            }
            // Rotor bearing Up 5 degrees
            else if (e.Control && e.Shift && e.KeyCode == Keys.Up)
            {
                if (txtSP.Text == null || txtSP.Text == "")
                {
                    if (lblSP.Text != "")
                        txtSP.Text = lblSP.Text;
                    else
                    {
                        MessageBox.Show("A heading is required to use this command.");
                        return;
                    }
                }
                int i = (int)Math.Round(Convert.ToDecimal(txtSP.Text));
                if (i <= 355)
                { i += 5; }
                else
                { i = 5 - (360 - i); }

                txtSP.Text = i.ToString();
                btnSP_Click(null, null);
            }
        }
        // see if the enter key was pressed
        private void txtSP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSP_Click(null, null);
            }
        }

        #endregion Events

        #region Window Geometry

        private bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(rect))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // only save the WindowState if Normal or Maximized
            switch (this.WindowState)
            {
                case FormWindowState.Normal:
                case FormWindowState.Maximized:
                    set.WindowStatewinRotor = this.WindowState;
                    break;

                default:
                    set.WindowStatewinRotor = FormWindowState.Normal;
                    break;
            }

            set.Save();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            TrackWindowState();
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            TrackWindowState();
        }
        bool windowInitialized;
        // On a move or resize in Normal state, record the new values as they occur.
        // This solves the problem of closing the app when minimized or maximized.
        private void TrackWindowState()
        {
            // Don't record the window setup, otherwise we lose the persistent values!
            if (!windowInitialized) { return; }

            if (WindowState == FormWindowState.Normal)
            {
                set.WindowPositionwinRotor = this.DesktopBounds;
            }
        }

        # endregion Window Geometry

        
    }
}
