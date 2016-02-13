//=====================================================================
// Setup_Rmx.cs
//=====================================================================
// Copyright (C) 2013 Tobias Wellnitz EA4/DH1TW
// Copyright (C) 2013 Doug Wigley W5WC 
// Copyright (C) 2016 Andrew Mansfield M0YGG 
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
    public partial class DJConsoleRmxConfig : Form
    {
        private Console m_parent;

        public DJConsoleRmxConfig(Console frm1)
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

            cBoxPitchA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxPitchA.DisplayMember = "Value";
            cBoxPitchA.ValueMember = "Key";

            cBoxPitchB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxPitchB.DisplayMember = "Value";
            cBoxPitchB.ValueMember = "Key";

            cBoxBalance.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxBalance.DisplayMember = "Value";
            cBoxBalance.ValueMember = "Key";

            cBoxGainA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxGainA.DisplayMember = "Value";
            cBoxGainA.ValueMember = "Key";

            cBoxGainB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxGainB.DisplayMember = "Value";
            cBoxGainB.ValueMember = "Key";

            cBoxMonSelect.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxMonSelect.DisplayMember = "Value";
            cBoxMonSelect.ValueMember = "Key";

            cBoxVolMain.DataSource = new BindingSource(m_parent.DJConsoleObj.dictFixKnobs, null);
            cBoxVolMain.DisplayMember = "Value";
            cBoxVolMain.ValueMember = "Key";

            //Incremental Knobs

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

            cBoxSourceA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSourceA.DisplayMember = "Value";
            cBoxSourceA.ValueMember = "Key";

            cBoxSourceB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSourceB.DisplayMember = "Value";
            cBoxSourceB.ValueMember = "Key";

            cBoxBeatLockA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxBeatLockA.DisplayMember = "Value";
            cBoxBeatLockA.ValueMember = "Key";

            cBoxBeatLockB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxBeatLockB.DisplayMember = "Value";
            cBoxBeatLockB.ValueMember = "Key";

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

            cBoxSelectA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSelectA.DisplayMember = "Value";
            cBoxSelectA.ValueMember = "Key";

            cBoxSelectB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSelectB.DisplayMember = "Value";
            cBoxSelectB.ValueMember = "Key";

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

            cBoxPitchResetA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxPitchResetA.DisplayMember = "Value";
            cBoxPitchResetA.ValueMember = "Key";

            cBoxPitchResetB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxPitchResetB.DisplayMember = "Value";
            cBoxPitchResetB.ValueMember = "Key";

            cBoxLoadA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxLoadA.DisplayMember = "Value";
            cBoxLoadA.ValueMember = "Key";

            cBoxLoadB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxLoadB.DisplayMember = "Value";
            cBoxLoadB.ValueMember = "Key";

            cBoxSixB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSixB.DisplayMember = "Value";
            cBoxSixB.ValueMember = "Key";

            cBoxFiveB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFiveB.DisplayMember = "Value";
            cBoxFiveB.ValueMember = "Key";

            cBoxFiveA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFiveA.DisplayMember = "Value";
            cBoxFiveA.ValueMember = "Key";

            cBoxSixA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxSixA.DisplayMember = "Value";
            cBoxSixA.ValueMember = "Key";

            cBoxTrebleKillA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxTrebleKillA.DisplayMember = "Value";
            cBoxTrebleKillA.ValueMember = "Key";

            cBoxMediumKillA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxMediumKillA.DisplayMember = "Value";
            cBoxMediumKillA.ValueMember = "Key";

            cBoxBassKillA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxBassKillA.DisplayMember = "Value";
            cBoxBassKillA.ValueMember = "Key";

            cBoxTrebleKillB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxTrebleKillB.DisplayMember = "Value";
            cBoxTrebleKillB.ValueMember = "Key";

            cBoxMediumKillB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxMediumKillB.DisplayMember = "Value";
            cBoxMediumKillB.ValueMember = "Key";

            cBoxBassKillB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxBassKillB.DisplayMember = "Value";
            cBoxBassKillB.ValueMember = "Key";

            cBoxScratch.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxScratch.DisplayMember = "Value";
            cBoxScratch.ValueMember = "Key";

            cBoxLeft.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxLeft.DisplayMember = "Value";
            cBoxLeft.ValueMember = "Key";

            cBoxRight.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxRight.DisplayMember = "Value";
            cBoxRight.ValueMember = "Key";

            cBoxStopA.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxStopA.DisplayMember = "Value";
            cBoxStopA.ValueMember = "Key";

            cBoxStopB.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxStopB.DisplayMember = "Value";
            cBoxStopB.ValueMember = "Key";

            cBoxMikeToggle.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxMikeToggle.DisplayMember = "Value";
            cBoxMikeToggle.ValueMember = "Key";

            cBoxTrebleA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TrebleA;
            cBoxTrebleB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TrebleB;
            cBoxMediumA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.MediumA;
            cBoxMediumB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.MediumB;
            cBoxBassA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.BassA;
            cBoxBassB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.BassB;
            cBoxVolumeA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.VolumeA;
            cBoxVolumeB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.VolumeB;
            cBoxCrossfader.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.Crossfader;
            cBoxPitchA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.PitchA;
            cBoxPitchB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.PitchB;
            cBoxJogWheelA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.JogWheelA;
            cBoxJogWheelB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.JogWheelB;
            cBoxPlayA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.PlayA;
            cBoxPlayB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.PlayB;
            cBoxCueA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.CueA;
            cBoxCueB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.CueB;
            cBoxTrackNextA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TrackNextA;
            cBoxTrackPrevA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TrackPrevA;
            cBoxTrackNextB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TrackNextB;
            cBoxTrackPrevB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TrackPrevB;
            cBoxSourceA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.SourceA;
            cBoxBeatLockA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.BeatLockA;
            cBoxBeatLockB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.BeatLockB;
            cBoxSourceB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.SourceB;
            cBoxOneA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.OneA;
            cBoxOneB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.OneB;
            cBoxTwoA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TwoA;
            cBoxTwoB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TwoB;
            cBoxThreeA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.ThreeA;
            cBoxThreeB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.ThreeB;
            cBoxFourA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.FourA;
            cBoxFourB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.FourB;
            cBoxSelectA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.SelectA;
            cBoxSelectB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.SelectB;
            cBoxSyncA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.SyncA;
            cBoxSyncB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.SyncB;
            cBoxUp.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.Up;
            cBoxDown.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.Down;
            cBoxPitchResetA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.PitchResetA;
            cBoxPitchResetB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.PitchResetB;
            cBoxLoadA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.LoadA;
            cBoxLoadB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.LoadB;
            cBoxSixB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.SixB;
            cBoxFiveB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.FiveB;
            cBoxFiveA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.FiveA;
            cBoxSixA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.FiveA;
            cBoxTrebleKillA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TrebleKillA;
            cBoxMediumKillA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.MediumKillA;
            cBoxBassKillA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.BassKillA;
            cBoxTrebleKillB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.TrebleKillB;
            cBoxMediumKillB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.MediumKillB;
            cBoxBassKillB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.BassKillB;
            cBoxScratch.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.Scratch;
            cBoxLeft.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.Left;
            cBoxRight.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.Right;
            cBoxStopA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.StopA;
            cBoxStopB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.StopB;
            cBoxBalance.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.Balance;
            cBoxGainA.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.GainA;
            cBoxGainB.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.GainB;
            cBoxMonSelect.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.MonSelect;
            cBoxVolMain.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.VolMain;
            cBoxMikeToggle.SelectedValue = m_parent.DJConsoleObj.DJConsole_Rmx.MikeToggle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cBoxTrebleA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TrebleA = (int)cBoxTrebleA.SelectedValue;
            }
            if (cBoxTrebleB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TrebleB = (int)cBoxTrebleB.SelectedValue;
            }
            if (cBoxMediumA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.MediumA = (int)cBoxMediumA.SelectedValue;
            }
            if (cBoxMediumB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.MediumB = (int)cBoxMediumB.SelectedValue;
            }
            if (cBoxBassA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.BassA = (int)cBoxBassA.SelectedValue;
            }
            if (cBoxBassB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.BassB = (int)cBoxBassB.SelectedValue;
            }
            if (cBoxVolumeA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.VolumeA = (int)cBoxVolumeA.SelectedValue;
            }
            if (cBoxVolumeB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.VolumeB = (int)cBoxVolumeB.SelectedValue;
            }
            if (cBoxCrossfader.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.Crossfader = (int)cBoxCrossfader.SelectedValue;
            }
            if (cBoxPitchA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.PitchA = (int)cBoxPitchA.SelectedValue;
            }
            if (cBoxPitchB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.PitchB = (int)cBoxPitchB.SelectedValue;
            }
            if (cBoxJogWheelA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.JogWheelA = (int)cBoxJogWheelA.SelectedValue;
            }
            if (cBoxJogWheelB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.JogWheelB = (int)cBoxJogWheelB.SelectedValue;
            }
            if (cBoxPlayA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.PlayA = (int)cBoxPlayA.SelectedValue;
            }
            if (cBoxPlayB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.PlayB = (int)cBoxPlayB.SelectedValue;
            }
            if (cBoxCueA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.CueA = (int)cBoxCueA.SelectedValue;
            }
            if (cBoxCueB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.CueB = (int)cBoxCueB.SelectedValue;
            }
            if (cBoxTrackNextA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TrackNextA = (int)cBoxTrackNextA.SelectedValue;
            }
            if (cBoxTrackPrevA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TrackPrevA = (int)cBoxTrackPrevA.SelectedValue;
            }
            if (cBoxTrackNextB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TrackNextB = (int)cBoxTrackNextB.SelectedValue;
            }
            if (cBoxTrackPrevB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TrackPrevB = (int)cBoxTrackPrevB.SelectedValue;
            }
            if (cBoxSourceA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.SourceA = (int)cBoxSourceA.SelectedValue;
            }
            if (cBoxBeatLockA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.BeatLockA = (int)cBoxBeatLockA.SelectedValue;
            }
            if (cBoxBeatLockB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.BeatLockB = (int)cBoxBeatLockB.SelectedValue;
            }
            if (cBoxSourceB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.SourceB = (int)cBoxSourceB.SelectedValue;
            }

            if (cBoxOneA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.OneA = (int)cBoxOneA.SelectedValue;
            }

            if (cBoxOneB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.OneB = (int)cBoxOneB.SelectedValue;
            }

            if (cBoxTwoA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TwoA = (int)cBoxTwoA.SelectedValue;
            }

            if (cBoxTwoB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TwoB = (int)cBoxTwoB.SelectedValue;
            }
            if (cBoxThreeA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.ThreeA = (int)cBoxThreeA.SelectedValue;
            }
            if (cBoxThreeB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.ThreeB = (int)cBoxThreeB.SelectedValue;
            }
            if (cBoxFourA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.FourA = (int)cBoxFourA.SelectedValue;
            }
            if (cBoxFourB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.FourB = (int)cBoxFourB.SelectedValue;
            }
            if (cBoxSelectA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.SelectA = (int)cBoxSelectA.SelectedValue;
            }
            if (cBoxSelectB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.SelectB = (int)cBoxSelectB.SelectedValue;
            }
            if (cBoxSyncA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.SyncA = (int)cBoxSyncA.SelectedValue;
            }
            if (cBoxSyncB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.SyncB = (int)cBoxSyncB.SelectedValue;
            }
            if (cBoxUp.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.Up = (int)cBoxUp.SelectedValue;
            }
            if (cBoxDown.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.Down = (int)cBoxDown.SelectedValue;
            }

            if (cBoxLoadA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.LoadA = (int)cBoxLoadA.SelectedValue;
            }

            if (cBoxLoadB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.LoadB = (int)cBoxLoadB.SelectedValue;
            }

            if (cBoxPitchResetA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.PitchResetA = (int)cBoxPitchResetA.SelectedValue;
            }

            if (cBoxPitchResetB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.PitchResetB = (int)cBoxPitchResetB.SelectedValue;
            }

            if (cBoxFiveA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.FiveA = (int)cBoxFiveA.SelectedValue;
            }

            if (cBoxSixA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.SixA = (int)cBoxSixA.SelectedValue;
            }

            if (cBoxSixB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.SixB = (int)cBoxSixB.SelectedValue;
            }

            if (cBoxFiveB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.FiveB = (int)cBoxFiveB.SelectedValue;
            }

            if (cBoxTrebleKillA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TrebleKillA = (int)cBoxTrebleKillA.SelectedValue;
            }

            if (cBoxMediumKillA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.MediumKillA = (int)cBoxMediumKillA.SelectedValue;
            }

            if (cBoxBassKillA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.BassKillA = (int)cBoxBassKillA.SelectedValue;
            }

            if (cBoxTrebleKillB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.TrebleKillB = (int)cBoxTrebleKillB.SelectedValue;
            }

            if (cBoxMediumKillB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.MediumKillB = (int)cBoxMediumKillB.SelectedValue;
            }

            if (cBoxBassKillB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.BassKillB = (int)cBoxBassKillB.SelectedValue;
            }

            if (cBoxScratch.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.Scratch = (int)cBoxScratch.SelectedValue;
            }

            if (cBoxLeft.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.Left = (int)cBoxLeft.SelectedValue;
            }

            if (cBoxRight.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.Right = (int)cBoxRight.SelectedValue;
            }

            if (cBoxStopA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.StopA = (int)cBoxStopA.SelectedValue;
            }

            if (cBoxStopB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.StopB = (int)cBoxStopB.SelectedValue;
            }

            if (cBoxBalance.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.Balance = (int)cBoxBalance.SelectedValue;
            }

            if (cBoxGainA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.GainA = (int)cBoxGainA.SelectedValue;
            }

            if (cBoxGainB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.GainB = (int)cBoxGainB.SelectedValue;
            }

            if (cBoxMonSelect.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.MonSelect = (int)cBoxMonSelect.SelectedValue;
            }

            if (cBoxVolMain.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.VolMain = (int)cBoxVolMain.SelectedValue;
            }

            if (cBoxMikeToggle.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_Rmx.MikeToggle = (int)cBoxMikeToggle.SelectedValue;
            }

            m_parent.DJConsoleObj.SaveSettings();
            //m_parent.DJConsoleObj = null;
            //m_parent.DJConsoleObj = new DJConsole(m_parent);
            this.Close();
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void DropDownGrow(object sender, EventArgs e)
        {
            ComboBox cb=(ComboBox)sender;
            cb.Tag = cb.Width;
            ((ComboBox)sender).Width = 121;
        }

        private void DropDownShrink(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (((int)cb.Tag) > 0)
                cb.Width = ((int)cb.Tag);
            else
                cb.Width = 121;
        }




    }
}
