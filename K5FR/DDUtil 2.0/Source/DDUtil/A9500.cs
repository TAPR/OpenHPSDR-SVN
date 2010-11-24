//==
// A9500.cs
//==
// Copyright (C) 2007-2010  Steve Nance - K5FR
//

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
    public partial class A9500 : Form
    {
        #region Vars & Init
        Setup s;
        public bool init;
        Settings set = Settings.Default;
        public string PFfile = "";

        public A9500(Setup setup)
        {
            InitializeComponent();
            s = setup;
            // Restore window size and position 
            // this is the default
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.WindowsDefaultBounds;

            // check if the saved bounds are nonzero and visible on any screen
            if (set.WindowPositionA95 != Rectangle.Empty &&
                IsVisibleOnAnyScreen(set.WindowPositionA95))
            {
                // first set the bounds
                this.StartPosition = FormStartPosition.Manual;
                this.DesktopBounds = set.WindowPositionA95;

                // afterwards set the window state to the saved value (which could be Maximized)
                this.WindowState = set.WindowStateA95;
            }
            else
            {
                // this resets the upper left corner of the window to windows standards
                this.StartPosition = FormStartPosition.WindowsDefaultLocation;

                // we can still apply the saved size
                if (set.WindowPositionA95 != Rectangle.Empty)
                {
                    this.Size = set.WindowPositionA95.Size;
                }
            }
            windowInitialized = true;
        }

        #endregion Vars & Init

        #region Events

        // form is closed
        private void A9500_FormClosing(object sender, FormClosedEventArgs e)
        {
            //       this.Close();
        }
        // form is closing
        private void A9500_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //           set.miniGeo = DataDecoder.Setup.GeometryToString(this);
            e.Cancel = true;
            this.Hide();
        }
        private void A9500_FormClosing(object sender, FormClosingEventArgs e)
        {
            s.WindowState = FormWindowState.Normal;
        }
        // the Pwr button was pressed
        private void btnA95on_Click(object sender, EventArgs e)
        {
            s.btnA95on_Click(null, null);
        }
        // the Aux button was pressed
        private void btnA95aux_Click(object sender, EventArgs e)
        {
            s.btnA95aux_Click(null, null);
        }
        // the Mode button was pressed
        private void btnA95mode_Click(object sender, EventArgs e)
        {
            s.btnA95mode_Click(null, null);
        }
        // the Ant button was pressed
        private void btnA95ant_Click(object sender, EventArgs e)
        {
            s.btnA95ant_Click(null, null);
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
                    set.WindowStateA95 = this.WindowState;
                    break;

                default:
                    set.WindowStateA95 = FormWindowState.Normal;
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
                set.WindowPositionA95 = this.DesktopBounds;
            }
        }

        # endregion Window Geometry

    }
}
