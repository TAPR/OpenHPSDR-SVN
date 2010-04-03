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

    public partial class WN2Matrix : Form
    {
    #region # Enums, Vars & Initialization #

        Setup s;
        public bool init;
        Settings set = Settings.Default;

        public WN2Matrix(Setup setup)
        {
            InitializeComponent();
            s = setup;
            // load the matrix
            switch (set.bs160)
            {
                case 1: rbSens160a.Checked = true; break;
                case 2: rbSens160b.Checked = true; break;
                case 3: rbSens160c.Checked = true; break;
                case 4: rbSens160d.Checked = true; break;
            }
            switch (set.bs80)
            {
                case 1: rbSens80a.Checked = true; break;
                case 2: rbSens80b.Checked = true; break;
                case 3: rbSens80c.Checked = true; break;
                case 4: rbSens80d.Checked = true; break;
            }
            switch (set.bs60)
            {
                case 1: rbSens60a.Checked = true; break;
                case 2: rbSens60b.Checked = true; break;
                case 3: rbSens60c.Checked = true; break;
                case 4: rbSens60d.Checked = true; break;
            }
            switch (set.bs40)
            {
                case 1: rbSens40a.Checked = true; break;
                case 2: rbSens40b.Checked = true; break;
                case 3: rbSens40c.Checked = true; break;
                case 4: rbSens40d.Checked = true; break;
            }
            switch (set.bs30)
            {
                case 1: rbSens30a.Checked = true; break;
                case 2: rbSens30b.Checked = true; break;
                case 3: rbSens30c.Checked = true; break;
                case 4: rbSens30d.Checked = true; break;
            }
            switch (set.bs20)
            {
                case 1: rbSens20a.Checked = true; break;
                case 2: rbSens20b.Checked = true; break;
                case 3: rbSens20c.Checked = true; break;
                case 4: rbSens20d.Checked = true; break;
            }
            switch (set.bs17)
            {
                case 1: rbSens17a.Checked = true; break;
                case 2: rbSens17b.Checked = true; break;
                case 3: rbSens17c.Checked = true; break;
                case 4: rbSens17d.Checked = true; break;
            }
            switch (set.bs15)
            {
                case 1: rbSens15a.Checked = true; break;
                case 2: rbSens15b.Checked = true; break;
                case 3: rbSens15c.Checked = true; break;
                case 4: rbSens15d.Checked = true; break;
            }
            switch (set.bs12)
            {
                case 1: rbSens12a.Checked = true; break;
                case 2: rbSens12b.Checked = true; break;
                case 3: rbSens12c.Checked = true; break;
                case 4: rbSens12d.Checked = true; break;
            }
            switch (set.bs10)
            {
                case 1: rbSens10a.Checked = true; break;
                case 2: rbSens10b.Checked = true; break;
                case 3: rbSens10c.Checked = true; break;
                case 4: rbSens10d.Checked = true; break;
            }
            switch (set.bs6)
            {
                case 1: rbSens6a.Checked = true; break;
                case 2: rbSens6b.Checked = true; break;
                case 3: rbSens6c.Checked = true; break;
                case 4: rbSens6d.Checked = true; break;
            }
            chkEnab.Checked = set.EnabMatrix;
        }
    #endregion Enums, Vars & Initialization

    #region # Events #
        public int wn160;  // global var for 160 meter sensor
        private void grp160_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens160a.Checked) {set.bs160 = 1; wn160 = 1;}
            else if (rbSens160b.Checked) {set.bs160 = 2; wn160 = 2;}
            else if (rbSens160c.Checked) {set.bs160 = 3; wn160 = 3;}
            else if (rbSens160d.Checked) {set.bs160 = 4; wn160 = 4;}
            set.Save();
        }
        public int wn80;  // global var for 80 meter sensor
        private void grp80_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens80a.Checked) { set.bs80 = 1; wn80 = 1; }
            else if (rbSens80b.Checked) { set.bs80 = 2; wn80 = 2; }
            else if (rbSens80c.Checked) { set.bs80 = 3; wn80 = 3; }
            else if (rbSens80d.Checked) { set.bs80 = 4; wn80 = 4; }
            set.Save();
        }
        public int wn60;  // global var for 60 meter sensor
        private void grp60_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens60a.Checked) { set.bs60 = 1; wn60 = 1; }
            else if (rbSens60b.Checked) { set.bs60 = 2; wn60 = 2; }
            else if (rbSens60c.Checked) { set.bs60 = 3; wn60 = 3; }
            else if (rbSens60d.Checked) { set.bs60 = 4; wn60 = 4; }
            set.Save();
        }
        public int wn40;  // global var for 40 meter sensor
        private void grp40_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens40a.Checked) { set.bs40 = 1; wn40 = 1; }
            else if (rbSens40b.Checked) { set.bs40 = 2; wn40 = 2; }
            else if (rbSens40c.Checked) { set.bs40 = 3; wn40 = 3; }
            else if (rbSens40d.Checked) { set.bs40 = 4; wn40 = 4; }
            set.Save();
        }
        public int wn30;  // global var for 30 meter sensor
        private void grp30_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens30a.Checked) { set.bs30 = 1; wn30 = 1; }
            else if (rbSens30b.Checked) { set.bs30 = 2; wn30 = 2; }
            else if (rbSens30c.Checked) { set.bs30 = 3; wn30 = 3; }
            else if (rbSens30d.Checked) { set.bs30 = 4; wn30 = 4; }
            set.Save();
        }
        public int wn20;  // global var for 20 meter sensor
        private void grp20_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens20a.Checked) { set.bs20 = 1; wn20 = 1; }
            else if (rbSens20b.Checked) { set.bs20 = 2; wn20 = 2; }
            else if (rbSens20c.Checked) { set.bs20 = 3; wn20 = 3; }
            else if (rbSens20d.Checked) { set.bs20 = 4; wn20 = 4; }
            set.Save();
        }
        public int wn17;  // global var for 17 meter sensor
        private void grp17_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens17a.Checked) { set.bs17 = 1; wn17 = 1; }
            else if (rbSens17b.Checked) { set.bs17 = 2; wn17 = 2; }
            else if (rbSens17c.Checked) { set.bs17 = 3; wn17 = 3; }
            else if (rbSens17d.Checked) { set.bs17 = 4; wn17 = 4; }
            set.Save();
        }
        public int wn15;  // global var for 15 meter sensor
        private void grp15_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens15a.Checked) { set.bs15 = 1; wn15 = 1; }
            else if (rbSens15b.Checked) { set.bs15 = 2; wn15 = 2; }
            else if (rbSens15c.Checked) { set.bs15 = 3; wn15 = 3; }
            else if (rbSens15d.Checked) { set.bs15 = 4; wn15 = 4; }
            set.Save();
        }
        public int wn12;  // global var for 12 meter sensor
        private void grp12_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens12a.Checked) { set.bs12 = 1; wn12 = 1; }
            else if (rbSens12b.Checked) { set.bs12 = 2; wn12 = 2; }
            else if (rbSens12c.Checked) { set.bs12 = 3; wn12 = 3; }
            else if (rbSens12d.Checked) { set.bs12 = 4; wn12 = 4; }
            set.Save();
        }
        public int wn10;  // global var for 10 meter sensor
        private void grp10_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens10a.Checked) { set.bs10 = 1; wn10 = 1; }
            else if (rbSens10b.Checked) { set.bs10 = 2; wn10 = 2; }
            else if (rbSens10c.Checked) { set.bs10 = 3; wn10 = 3; }
            else if (rbSens10d.Checked) { set.bs10 = 4; wn10 = 4; }
            set.Save();
        }
        public int wn6;  // global var for 6 meter sensor
        private void grp6_CheckChanged(object sender, EventArgs e)
        {
                 if (rbSens6a.Checked) { set.bs6 = 1; wn6 = 1; }
            else if (rbSens6b.Checked) { set.bs6 = 2; wn6 = 2; }
            else if (rbSens6c.Checked) { set.bs6 = 3; wn6 = 3; }
            else if (rbSens6d.Checked) { set.bs6 = 4; wn6 = 4; }
            set.Save();
        }

        // form is closing
        private void WN2Matrix_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void WN2Matrix_FormClosing(object sender, FormClosedEventArgs e)
        {

        }
        private void chkEnab_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnab.Checked) { set.EnabMatrix = true; }
            else { set.EnabMatrix = false; }
            set.Save();
        }

    #endregion Events

    }

}
