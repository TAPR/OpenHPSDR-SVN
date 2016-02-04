//=====================================================================
// Setup_MP2.cs
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
using System.Linq;
using System.Windows.Forms;

namespace Thetis
{
    public partial class DJConsoleMK2Config : Form
    {
        private Console m_parent;

        public DJConsoleMK2Config(Console frm1)
        {
            m_parent = frm1;
            InitializeComponent();
            InitDJConsoleConfig();
        }
        
        private void InitDJConsoleConfig()
        {
            // Fixed Value Range Knobs

            cBoxTrebleA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxTrebleA.DisplayMember = "Value";
            cBoxTrebleA.ValueMember = "Key";

            cBoxTrebleB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxTrebleB.DisplayMember = "Value";
            cBoxTrebleB.ValueMember = "Key";

            cBoxMediumA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxMediumA.DisplayMember = "Value";
            cBoxMediumA.ValueMember = "Key";

            cBoxMediumB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxMediumB.DisplayMember = "Value";
            cBoxMediumB.ValueMember = "Key";

            cBoxBassA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxBassA.DisplayMember = "Value";
            cBoxBassA.ValueMember = "Key";

            cBoxBassB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxBassB.DisplayMember = "Value";
            cBoxBassB.ValueMember = "Key";

            cBoxVolumeA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxVolumeA.DisplayMember = "Value";
            cBoxVolumeA.ValueMember = "Key";

            cBoxVolumeB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxVolumeB.DisplayMember = "Value";
            cBoxVolumeB.ValueMember = "Key";

            cBoxCrossfader.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxCrossfader.DisplayMember = "Value";
            cBoxCrossfader.ValueMember = "Key";


            //Incremental Knobs

            cBoxPitchA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictIncKnobs, null);
            cBoxPitchA.DisplayMember = "Value";
            cBoxPitchA.ValueMember = "Key";

            cBoxPitchB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictIncKnobs, null);
            cBoxPitchB.DisplayMember = "Value";
            cBoxPitchB.ValueMember = "Key";

            cBoxJogWheelA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictIncKnobs, null);
            cBoxJogWheelA.DisplayMember = "Value";
            cBoxJogWheelA.ValueMember = "Key";

            cBoxJogWheelB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictIncKnobs, null);
            cBoxJogWheelB.DisplayMember = "Value";
            cBoxJogWheelB.ValueMember = "Key";

            //Buttons

            cBoxPlayA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxPlayA.DisplayMember = "Value";
            cBoxPlayA.ValueMember = "Key";

            cBoxPlayB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxPlayB.DisplayMember = "Value";
            cBoxPlayB.ValueMember = "Key";

            cBoxCueA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxCueA.DisplayMember = "Value";
            cBoxCueA.ValueMember = "Key";

            cBoxCueB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxCueB.DisplayMember = "Value";
            cBoxCueB.ValueMember = "Key";

            cBoxTrackNextA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxTrackNextA.DisplayMember = "Value";
            cBoxTrackNextA.ValueMember = "Key";

            cBoxTrackNextB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxTrackNextB.DisplayMember = "Value";
            cBoxTrackNextB.ValueMember = "Key";

            cBoxTrackPrevB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxTrackPrevB.DisplayMember = "Value";
            cBoxTrackPrevB.ValueMember = "Key";

            cBoxTrackPrevA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxTrackPrevA.DisplayMember = "Value";
            cBoxTrackPrevA.ValueMember = "Key";

            cBoxPitchDownA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxPitchDownA.DisplayMember = "Value";
            cBoxPitchDownA.ValueMember = "Key";

            cBoxPitchDownB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxPitchDownB.DisplayMember = "Value";
            cBoxPitchDownB.ValueMember = "Key";

            cBoxPitchUpA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxPitchUpA.DisplayMember = "Value";
            cBoxPitchUpA.ValueMember = "Key";

            cBoxPitchUpB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxPitchUpB.DisplayMember = "Value";
            cBoxPitchUpB.ValueMember = "Key";

            cBoxAutoBeatA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxAutoBeatA.DisplayMember = "Value";
            cBoxAutoBeatA.ValueMember = "Key";

            cBoxAutoBeatB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxAutoBeatB.DisplayMember = "Value";
            cBoxAutoBeatB.ValueMember = "Key";

            cBoxFxA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFxA.DisplayMember = "Value";
            cBoxFxA.ValueMember = "Key";

            cBoxFxB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFxB.DisplayMember = "Value";
            cBoxFxB.ValueMember = "Key";

            cBoxCueFxA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxCueFxA.DisplayMember = "Value";
            cBoxCueFxA.ValueMember = "Key";

            cBoxCueFxB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxCueFxB.DisplayMember = "Value";
            cBoxCueFxB.ValueMember = "Key";

            cBoxLoopFxA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxLoopFxA.DisplayMember = "Value";
            cBoxLoopFxA.ValueMember = "Key";

            cBoxLoopFxB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxLoopFxB.DisplayMember = "Value";
            cBoxLoopFxB.ValueMember = "Key";

            cBoxSelectFxA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSelectFxA.DisplayMember = "Value";
            cBoxSelectFxA.ValueMember = "Key";

            cBoxSelectFxB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSelectFxB.DisplayMember = "Value";
            cBoxSelectFxB.ValueMember = "Key";

            cBoxTrebleA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.TrebleA;
            cBoxTrebleB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.TrebleB;
            cBoxMediumA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.MediumA;
            cBoxMediumB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.MediumB;
            cBoxBassA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.BassA;
            cBoxBassB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.BassB;
            cBoxVolumeA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.VolumeA;
            cBoxVolumeB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.VolumeB;
            cBoxCrossfader.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.Crossfader;
            cBoxPitchA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.PitchA;
            cBoxPitchB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.PitchB;
            cBoxJogWheelA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.JogWheelA;
            cBoxJogWheelB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.JogWheelB;
            cBoxPlayA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.PlayA;
            cBoxPlayB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.PlayB;
            cBoxCueA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.CueA;
            cBoxCueB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.CueB;
            cBoxAutoBeatA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.AutobeatA;
            cBoxAutoBeatB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.AutobeatB;
            cBoxTrackNextA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.TrackNextA;
            cBoxTrackPrevA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.TrackPrevA;
            cBoxTrackNextB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.TrackNextB;
            cBoxTrackPrevB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.TrackPrevB;
            cBoxPitchDownA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.PitchDownA;
            cBoxPitchUpA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.PitchUpA;
            cBoxPitchUpB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.PitchUpB;
            cBoxPitchDownB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.PitchDownB;
            cBoxFxA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.FXA;
            cBoxFxB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.FXB;
            cBoxCueFxA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.CueFXA;
            cBoxCueFxB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.CueFXB;
            cBoxLoopFxA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.LoopFxA;
            cBoxLoopFxB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.LoopFxB;
            cBoxSelectFxA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.SelectFxA;
            cBoxSelectFxB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MK2.SelectFxB;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cBoxTrebleA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.TrebleA = (int)cBoxTrebleA.SelectedValue;
            }
            if (cBoxTrebleB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.TrebleB = (int)cBoxTrebleB.SelectedValue;
            }
            if (cBoxMediumA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.MediumA = (int)cBoxMediumA.SelectedValue;
            }
            if (cBoxMediumB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.MediumB = (int)cBoxMediumB.SelectedValue;
            }
            if (cBoxBassA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.BassA = (int)cBoxBassA.SelectedValue;
            }
            if (cBoxBassB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.BassB = (int)cBoxBassB.SelectedValue;
            }
            if (cBoxVolumeA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.VolumeA = (int)cBoxVolumeA.SelectedValue;
            }
            if (cBoxVolumeB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.VolumeB = (int)cBoxVolumeB.SelectedValue;
            }
            if (cBoxCrossfader.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.Crossfader = (int)cBoxCrossfader.SelectedValue;
            }
            if (cBoxPitchA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.PitchA = (int)cBoxPitchA.SelectedValue;
            }
            if (cBoxPitchB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.PitchB = (int)cBoxPitchB.SelectedValue;
            }
            if (cBoxJogWheelA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.JogWheelA = (int)cBoxJogWheelA.SelectedValue;
            }
            if (cBoxJogWheelB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.JogWheelB = (int)cBoxJogWheelB.SelectedValue;
            }
            if (cBoxPlayA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.PlayA = (int)cBoxPlayA.SelectedValue;
            }
            if (cBoxPlayB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.PlayB = (int)cBoxPlayB.SelectedValue;
            }
            if (cBoxCueA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.CueA = (int)cBoxCueA.SelectedValue;
            }
            if (cBoxCueB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.CueB = (int)cBoxCueB.SelectedValue;
            }
            if (cBoxAutoBeatA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.AutobeatA = (int)cBoxAutoBeatA.SelectedValue;
            }
            if (cBoxAutoBeatB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.AutobeatB = (int)cBoxAutoBeatB.SelectedValue;
            }
            if (cBoxTrackNextA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.TrackNextA = (int)cBoxTrackNextA.SelectedValue;
            }
            if (cBoxTrackPrevA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.TrackPrevA = (int)cBoxTrackPrevA.SelectedValue;
            }
            if (cBoxTrackNextB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.TrackNextB = (int)cBoxTrackNextB.SelectedValue;
            }
            if (cBoxTrackPrevB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.TrackPrevB = (int)cBoxTrackPrevB.SelectedValue;
            }
            if (cBoxPitchDownA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.PitchDownA = (int)cBoxPitchDownA.SelectedValue;
            }
            if (cBoxPitchUpA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.PitchUpA = (int)cBoxPitchUpA.SelectedValue;
            }
            if (cBoxPitchUpB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.PitchUpB = (int)cBoxPitchUpB.SelectedValue;
            }
            if (cBoxPitchDownB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.PitchDownB = (int)cBoxPitchDownB.SelectedValue;
            }
            if (cBoxFxA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.FXA = (int)cBoxFxA.SelectedValue;
            }
            if (cBoxFxB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.FXB = (int)cBoxFxB.SelectedValue;
            }
            if (cBoxCueFxA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.CueFXA = (int)cBoxCueFxA.SelectedValue;
            }
            if (cBoxCueFxB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.CueFXB = (int)cBoxCueFxB.SelectedValue;
            }
            if (cBoxLoopFxA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.LoopFxA = (int)cBoxLoopFxA.SelectedValue;
            }
            if (cBoxLoopFxB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.LoopFxB = (int)cBoxLoopFxB.SelectedValue;
            }
            if (cBoxSelectFxA.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.SelectFxA = (int)cBoxSelectFxA.SelectedValue;
            }
            if (cBoxSelectFxB.SelectedItem != null)
            {
                m_parent.DJConsoleObj.DJConsole_MK2.SelectFxB = (int)cBoxSelectFxB.SelectedValue;
            }

            m_parent.DJConsoleObj.SaveSettings();
            //m_parent.DJConsoleObj = null;
            //m_parent.DJConsoleObj = new DJConsole(m_parent);
            this.Close();

        }

        private void DJConsoleConfig_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
