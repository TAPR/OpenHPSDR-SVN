//=====================================================================
// Setup_MP3e2.cs
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
    public partial class DJConsoleMP3e2Config : Form
    {
        private Console m_parent;

        public DJConsoleMP3e2Config(Console frm1)
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

            cBoxOneA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxOneA.DisplayMember = "Value";
            cBoxOneA.ValueMember = "Key";

            cBoxOneB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxOneB.DisplayMember = "Value";
            cBoxOneB.ValueMember = "Key";

            cBoxTwoA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxTwoA.DisplayMember = "Value";
            cBoxTwoA.ValueMember = "Key";

            cBoxTwoB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxTwoB.DisplayMember = "Value";
            cBoxTwoB.ValueMember = "Key";

            cBoxThreeA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxThreeA.DisplayMember = "Value";
            cBoxThreeA.ValueMember = "Key";

            cBoxThreeB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxThreeB.DisplayMember = "Value";
            cBoxThreeB.ValueMember = "Key";

            cBoxFourA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFourA.DisplayMember = "Value";
            cBoxFourA.ValueMember = "Key";

            cBoxFourB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFourB.DisplayMember = "Value";
            cBoxFourB.ValueMember = "Key";

            cBoxListenA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxListenA.DisplayMember = "Value";
            cBoxListenA.ValueMember = "Key";

            cBoxListenB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxListenB.DisplayMember = "Value";
            cBoxListenB.ValueMember = "Key";

            cBoxSyncA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSyncA.DisplayMember = "Value";
            cBoxSyncA.ValueMember = "Key";

            cBoxSyncB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSyncB.DisplayMember = "Value";
            cBoxSyncB.ValueMember = "Key";

            cBoxUp.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxUp.DisplayMember = "Value";
            cBoxUp.ValueMember = "Key";

            cBoxDown.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxDown.DisplayMember = "Value";
            cBoxDown.ValueMember = "Key";

            cBoxMasterTempoA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxMasterTempoA.DisplayMember = "Value";
            cBoxMasterTempoA.ValueMember = "Key";

            cBoxMasterTempoB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxMasterTempoB.DisplayMember = "Value";
            cBoxMasterTempoB.ValueMember = "Key";

            cBoxLoadA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxLoadA.DisplayMember = "Value";
            cBoxLoadA.ValueMember = "Key";

            cBoxLoadB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxLoadB.DisplayMember = "Value";
            cBoxLoadB.ValueMember = "Key";

            cBoxFiles.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFiles.DisplayMember = "Value";
            cBoxFiles.ValueMember = "Key";

            cBoxFolders.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFolders.DisplayMember = "Value";
            cBoxFolders.ValueMember = "Key";

            cBoxScratch.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxScratch.DisplayMember = "Value";
            cBoxScratch.ValueMember = "Key";

            cBoxAutomix.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxAutomix.DisplayMember = "Value";
            cBoxAutomix.ValueMember = "Key";

            cBoxTrebleA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.TrebleA;
            cBoxTrebleB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.TrebleB;
            cBoxMediumA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.MediumA;
            cBoxMediumB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.MediumB;
            cBoxBassA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.BassA;
            cBoxBassB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.BassB;
            cBoxVolumeA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.VolumeA;
            cBoxVolumeB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.VolumeB;
            cBoxCrossfader.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.Crossfader;
            cBoxPitchA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchA;
            cBoxPitchB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchB;
            cBoxJogWheelA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.JogWheelA;
            cBoxJogWheelB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.JogWheelB;
            cBoxPlayA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.PlayA;
            cBoxPlayB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.PlayB;
            cBoxCueA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.CueA;
            cBoxCueB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.CueB;
            cBoxTrackNextA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.TrackNextA;
            cBoxTrackPrevA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.TrackPrevA;
            cBoxTrackNextB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.TrackNextB;
            cBoxTrackPrevB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.TrackPrevB;
            cBoxPitchDownA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchDownA;
            cBoxPitchUpA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchUpA;
            cBoxPitchUpB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchUpB;
            cBoxPitchDownB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchDownB;
            cBoxOneA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.OneA;
            cBoxOneB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.OneB;
            cBoxTwoA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.TwoA;
            cBoxTwoB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.TwoB;
            cBoxThreeA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.ThreeA;
            cBoxThreeB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.ThreeB;
            cBoxFourA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.FourA;
            cBoxFourB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.FourB;
            cBoxListenA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.ListenA;
            cBoxListenB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.ListenB;
            cBoxSyncA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.SyncA;
            cBoxSyncB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.SyncB;
            cBoxUp.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.Up;
            cBoxDown.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.Down;
            cBoxMasterTempoA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.MasterTempoA;
            cBoxMasterTempoB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.MasterTempoB;
            cBoxLoadA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.LoadA;
            cBoxLoadB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.LoadB;
            cBoxFiles.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.Files;
            cBoxFolders.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.Folders;
            cBoxScratch.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.Scratch;
            cBoxAutomix.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3e2.Automix;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cBoxTrebleA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.TrebleA = (int)cBoxTrebleA.SelectedValue;
            }
            if (cBoxTrebleB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.TrebleB = (int)cBoxTrebleB.SelectedValue;
            }
            if (cBoxMediumA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.MediumA = (int)cBoxMediumA.SelectedValue;
            }
            if (cBoxMediumB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.MediumB = (int)cBoxMediumB.SelectedValue;
            }
            if (cBoxBassA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.BassA = (int)cBoxBassA.SelectedValue;
            }
            if (cBoxBassB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.BassB = (int)cBoxBassB.SelectedValue;
            }
            if (cBoxVolumeA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.VolumeA = (int)cBoxVolumeA.SelectedValue;
            }
            if (cBoxVolumeB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.VolumeB = (int)cBoxVolumeB.SelectedValue;
            }
            if (cBoxCrossfader.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.Crossfader = (int)cBoxCrossfader.SelectedValue;
            }
            if (cBoxPitchA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.PitchA = (int)cBoxPitchA.SelectedValue;
            }
            if (cBoxPitchB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.PitchB = (int)cBoxPitchB.SelectedValue;
            }
            if (cBoxJogWheelA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.JogWheelA = (int)cBoxJogWheelA.SelectedValue;
            }
            if (cBoxJogWheelB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.JogWheelB = (int)cBoxJogWheelB.SelectedValue;
            }
            if (cBoxPlayA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.PlayA = (int)cBoxPlayA.SelectedValue;
            }
            if (cBoxPlayB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.PlayB = (int)cBoxPlayB.SelectedValue;
            }
            if (cBoxCueA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.CueA = (int)cBoxCueA.SelectedValue;
            }
            if (cBoxCueB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.CueB = (int)cBoxCueB.SelectedValue;
            }
            if (cBoxTrackNextA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.TrackNextA = (int)cBoxTrackNextA.SelectedValue;
            }
            if (cBoxTrackPrevA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.TrackPrevA = (int)cBoxTrackPrevA.SelectedValue;
            }
            if (cBoxTrackNextB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.TrackNextB = (int)cBoxTrackNextB.SelectedValue;
            }
            if (cBoxTrackPrevB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.TrackPrevB = (int)cBoxTrackPrevB.SelectedValue;
            }
            if (cBoxPitchDownA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.PitchDownA = (int)cBoxPitchDownA.SelectedValue;
            }
            if (cBoxPitchUpA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.PitchUpA = (int)cBoxPitchUpA.SelectedValue;
            }
            if (cBoxPitchUpB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.PitchUpB = (int)cBoxPitchUpB.SelectedValue;
            }
            if (cBoxPitchDownB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.PitchDownB = (int)cBoxPitchDownB.SelectedValue;
            }

            if (cBoxOneA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.OneA = (int)cBoxOneA.SelectedValue;
            }

            if (cBoxOneB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.OneB = (int)cBoxOneB.SelectedValue;
            }

            if (cBoxTwoA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.TwoA = (int)cBoxTwoA.SelectedValue;
            }

            if (cBoxTwoB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.TwoB = (int)cBoxTwoB.SelectedValue;
            }
            if (cBoxThreeA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.ThreeA = (int)cBoxThreeA.SelectedValue;
            }
            if (cBoxThreeB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.ThreeB = (int)cBoxThreeB.SelectedValue;
            }
            if (cBoxFourA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.FourA = (int)cBoxFourA.SelectedValue;
            }
            if (cBoxFourB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.FourB = (int)cBoxFourB.SelectedValue;
            }
            if (cBoxListenA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.ListenA = (int)cBoxListenA.SelectedValue;
            }
            if (cBoxListenB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.ListenB = (int)cBoxListenB.SelectedValue;
            }
            if (cBoxSyncA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.SyncA = (int)cBoxSyncA.SelectedValue;
            }
            if (cBoxSyncB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.SyncB = (int)cBoxSyncB.SelectedValue;
            }
            if (cBoxUp.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.Up = (int)cBoxUp.SelectedValue;
            }
            if (cBoxDown.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.Down = (int)cBoxDown.SelectedValue;
            }

            if (cBoxLoadA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.LoadA = (int)cBoxLoadA.SelectedValue;
            }

            if (cBoxLoadB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.LoadB = (int)cBoxLoadB.SelectedValue;
            }
            
            if (cBoxMasterTempoA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.MasterTempoA = (int)cBoxMasterTempoA.SelectedValue;
            }

            if (cBoxMasterTempoB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.MasterTempoB = (int)cBoxMasterTempoB.SelectedValue;
            }

            if (cBoxScratch.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.Scratch = (int)cBoxScratch.SelectedValue;
            }

            if (cBoxAutomix.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.Automix = (int)cBoxAutomix.SelectedValue;
            }

            if (cBoxFiles.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.Files = (int)cBoxFiles.SelectedValue;
            }

            if (cBoxFolders.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3e2.Folders = (int)cBoxFolders.SelectedValue;
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

        private void cBoxLoadB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }





    }
}
