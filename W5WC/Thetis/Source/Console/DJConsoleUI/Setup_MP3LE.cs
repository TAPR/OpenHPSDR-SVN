//=====================================================================
// Setup_MP3LE.cs
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
    public partial class DJConsoleMP3LEConfig : Form
    {
        private Console m_parent;

        public DJConsoleMP3LEConfig(Console frm1)
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

            cBoxFiles.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFiles.DisplayMember = "Value";
            cBoxFiles.ValueMember = "Key";

            cBoxFolders.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxFolders.DisplayMember = "Value";
            cBoxFolders.ValueMember = "Key";

            cBoxVinyl.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxVinyl.DisplayMember = "Value";
            cBoxVinyl.ValueMember = "Key";

            cBoxMagic.DataSource = new BindingSource(m_parent.DJConsoleObj.dictButtons, null);
            cBoxMagic.DisplayMember = "Value";
            cBoxMagic.ValueMember = "Key";

            cBoxTrebleA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.TrebleA;
            cBoxTrebleB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.TrebleB;
            cBoxMediumA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.MediumA;
            cBoxMediumB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.MediumB;
            cBoxBassA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.BassA;
            cBoxBassB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.BassB;
            cBoxVolumeA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.VolumeA;
            cBoxVolumeB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.VolumeB;
            cBoxCrossfader.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.Crossfader;
            cBoxPitchA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchA;
            cBoxPitchB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchB;
            cBoxJogWheelA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.JogWheelA;
            cBoxJogWheelB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.JogWheelB;
            cBoxPlayA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PlayA;
            cBoxPlayB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PlayB;
            cBoxCueA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.CueA;
            cBoxCueB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.CueB;
            cBoxTrackNextA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.TrackNextA;
            cBoxTrackPrevA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.TrackPrevA;
            cBoxTrackNextB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.TrackNextB;
            cBoxTrackPrevB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.TrackPrevB;
            cBoxPitchDownA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchDownA;
            cBoxPitchUpA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchUpA;
            cBoxPitchUpB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchUpB;
            cBoxPitchDownB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchDownB;
            cBoxOneA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.OneA;
            cBoxOneB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.OneB;
            cBoxTwoA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.TwoA;
            cBoxTwoB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.TwoB;
            cBoxThreeA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.ThreeA;
            cBoxThreeB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.ThreeB;
            cBoxFourA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.FourA;
            cBoxFourB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.FourB;
            cBoxListenA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.ListenA;
            cBoxListenB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.ListenB;
            cBoxSyncA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.SyncA;
            cBoxSyncB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.SyncB;
            cBoxUp.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.Up;
            cBoxDown.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.Down;
            cBoxPitchResetA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchResetA;
            cBoxPitchResetB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchResetB;
            cBoxLoadA.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.LoadA;
            cBoxLoadB.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.LoadB;
            cBoxFiles.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.Files;
            cBoxFolders.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.Folders;
            cBoxVinyl.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.Vinyl;
            cBoxMagic.SelectedValue = m_parent.DJConsoleObj.DJConsole_MP3LE.Vinyl;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cBoxTrebleA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.TrebleA = (int)cBoxTrebleA.SelectedValue;
            }
            if (cBoxTrebleB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.TrebleB = (int)cBoxTrebleB.SelectedValue;
            }
            if (cBoxMediumA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.MediumA = (int)cBoxMediumA.SelectedValue;
            }
            if (cBoxMediumB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.MediumB = (int)cBoxMediumB.SelectedValue;
            }
            if (cBoxBassA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.BassA = (int)cBoxBassA.SelectedValue;
            }
            if (cBoxBassB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.BassB = (int)cBoxBassB.SelectedValue;
            }
            if (cBoxVolumeA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.VolumeA = (int)cBoxVolumeA.SelectedValue;
            }
            if (cBoxVolumeB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.VolumeB = (int)cBoxVolumeB.SelectedValue;
            }
            if (cBoxCrossfader.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.Crossfader = (int)cBoxCrossfader.SelectedValue;
            }
            if (cBoxPitchA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PitchA = (int)cBoxPitchA.SelectedValue;
            }
            if (cBoxPitchB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PitchB = (int)cBoxPitchB.SelectedValue;
            }
            if (cBoxJogWheelA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.JogWheelA = (int)cBoxJogWheelA.SelectedValue;
            }
            if (cBoxJogWheelB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.JogWheelB = (int)cBoxJogWheelB.SelectedValue;
            }
            if (cBoxPlayA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PlayA = (int)cBoxPlayA.SelectedValue;
            }
            if (cBoxPlayB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PlayB = (int)cBoxPlayB.SelectedValue;
            }
            if (cBoxCueA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.CueA = (int)cBoxCueA.SelectedValue;
            }
            if (cBoxCueB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.CueB = (int)cBoxCueB.SelectedValue;
            }
            if (cBoxTrackNextA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.TrackNextA = (int)cBoxTrackNextA.SelectedValue;
            }
            if (cBoxTrackPrevA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.TrackPrevA = (int)cBoxTrackPrevA.SelectedValue;
            }
            if (cBoxTrackNextB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.TrackNextB = (int)cBoxTrackNextB.SelectedValue;
            }
            if (cBoxTrackPrevB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.TrackPrevB = (int)cBoxTrackPrevB.SelectedValue;
            }
            if (cBoxPitchDownA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PitchDownA = (int)cBoxPitchDownA.SelectedValue;
            }
            if (cBoxPitchUpA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PitchUpA = (int)cBoxPitchUpA.SelectedValue;
            }
            if (cBoxPitchUpB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PitchUpB = (int)cBoxPitchUpB.SelectedValue;
            }
            if (cBoxPitchDownB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PitchDownB = (int)cBoxPitchDownB.SelectedValue;
            }

            if (cBoxOneA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.OneA = (int)cBoxOneA.SelectedValue;
            }

            if (cBoxOneB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.OneB = (int)cBoxOneB.SelectedValue;
            }

            if (cBoxTwoA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.TwoA = (int)cBoxTwoA.SelectedValue;
            }

            if (cBoxTwoB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.TwoB = (int)cBoxTwoB.SelectedValue;
            }
            if (cBoxThreeA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.ThreeA = (int)cBoxThreeA.SelectedValue;
            }
            if (cBoxThreeB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.ThreeB = (int)cBoxThreeB.SelectedValue;
            }
            if (cBoxFourA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.FourA = (int)cBoxFourA.SelectedValue;
            }
            if (cBoxFourB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.FourB = (int)cBoxFourB.SelectedValue;
            }
            if (cBoxListenA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.ListenA = (int)cBoxListenA.SelectedValue;
            }
            if (cBoxListenB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.ListenB = (int)cBoxListenB.SelectedValue;
            }
            if (cBoxSyncA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.SyncA = (int)cBoxSyncA.SelectedValue;
            }
            if (cBoxSyncB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.SyncB = (int)cBoxSyncB.SelectedValue;
            }
            if (cBoxUp.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.Up = (int)cBoxUp.SelectedValue;
            }
            if (cBoxDown.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.Down = (int)cBoxDown.SelectedValue;
            }

            if (cBoxLoadA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.LoadA = (int)cBoxLoadA.SelectedValue;
            }

            if (cBoxLoadB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.LoadB = (int)cBoxLoadB.SelectedValue;
            }

            if (cBoxPitchResetA.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PitchResetA = (int)cBoxPitchResetA.SelectedValue;
            }

            if (cBoxPitchResetB.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.PitchResetB = (int)cBoxPitchResetB.SelectedValue;
            }

            if (cBoxVinyl.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.Vinyl = (int)cBoxVinyl.SelectedValue;
            }

            if (cBoxMagic.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.Magic = (int)cBoxMagic.SelectedValue;
            }

            if (cBoxFiles.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.Files = (int)cBoxFiles.SelectedValue;
            }

            if (cBoxFolders.SelectedValue != null)
            {
                m_parent.DJConsoleObj.DJConsole_MP3LE.Folders = (int)cBoxFolders.SelectedValue;
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }





    }
}
