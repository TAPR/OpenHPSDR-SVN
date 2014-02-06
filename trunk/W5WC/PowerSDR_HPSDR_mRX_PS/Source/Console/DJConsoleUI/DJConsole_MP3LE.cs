﻿//=====================================================================
// DJConsole_MP3LE.cs
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

namespace PowerSDR.DJConsoleUI
{
    public class DJConsole_MP3LE
    {
        private readonly DJConsole m_parent;

        public DJConsole_MP3LE(DJConsole frm1)
        {
            m_parent = frm1;
        }


        #region variables

        private int trebleA = 0;
        private int trebleB = 0;
        private int mediumA = 0;
        private int mediumB = 0;
        private int bassA = 0;
        private int bassB = 0;
        private int volumeA = 0;
        private int volumeB = 0;
        private int crossfader = 0;
        private int pitchA = 0;
        private int pitchB = 0;
        private int jogWheelA = 0;
        private int jogWheelB = 0;
        private int playA = 0;
        private int playB = 0;
        private int cueA = 0;
        private int cueB = 0;
        private int trackNextA = 0;
        private int trackPrevA = 0;
        private int trackNextB = 0;
        private int trackPrevB = 0;
        private int pitchDownA = 0;
        private int pitchUpA = 0;
        private int pitchUpB = 0;
        private int pitchDownB = 0;
        private int oneA = 0;
        private int oneB = 0;
        private int twoA = 0;
        private int twoB = 0;
        private int threeA = 0;
        private int threeB = 0;
        private int fourA = 0;
        private int fourB = 0;
        private int listenA = 0;
        private int listenB = 0;
        private int syncA = 0;
        private int syncB = 0;
        private int up = 0;
        private int down = 0;
        private int pitchResetA = 0;
        private int pitchResetB = 0;
        private int loadA = 0;
        private int loadB = 0;
        private int files = 0;
        private int folders = 0;
        private int vinyl = 0;
        private int magic = 0;

        #endregion

        #region Properties

        public int TrebleA
        {
            get { return trebleA; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    trebleA = value;
                }
                else
                {
                    trebleA = 0;
                }
            }
        }

        public int TrebleB
        {
            get { return trebleB; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    trebleB = value;
                }
                else
                {
                    trebleB = 0;
                }
            }
        }

        public int MediumA
        {
            get { return mediumA; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    mediumA = value;
                }
                else
                {
                    mediumA = 0;
                }
            }
        }

        public int MediumB
        {
            get { return mediumB; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    mediumB = value;
                }
                else
                {
                    mediumB = 0;
                }
            }
        }

        public int BassA
        {
            get { return bassA; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    bassA = value;
                }
                else
                {
                    bassA = 0;
                }
            }
        }

        public int BassB
        {
            get { return bassB; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    bassB = value;
                }
                else
                {
                    bassB = 0;
                }
            }
        }

        public int VolumeA
        {
            get { return volumeA; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    volumeA = value;
                }
                else
                {
                    volumeA = 0;
                }
            }
        }

        public int VolumeB
        {
            get { return volumeB; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    volumeB = value;
                }
                else
                {
                    volumeB = 0;
                }
            }
        }

        public int Crossfader
        {
            get { return crossfader; }
            set
            {
                if (checkValueFixKnob(value) == true)
                {
                    crossfader = value;
                }
                else
                {
                    crossfader = 0;
                }
            }
        }

        public int PitchA
        {
            get { return pitchA; }
            set
            {
                if (checkValueIncKnob(value) == true)
                {
                    pitchA = value;
                }
                else
                {
                    pitchA = 0;
                }
            }
        }

        public int PitchB
        {
            get { return pitchB; }
            set
            {
                if (checkValueIncKnob(value) == true)
                {
                    pitchB = value;
                }
                else
                {
                    pitchB = 0;
                }
            }
        }

        public int JogWheelA
        {
            get { return jogWheelA; }
            set
            {
                if (checkValueIncKnob(value) == true)
                {
                    jogWheelA = value;
                }
                else
                {
                    jogWheelA = 0;
                }
            }
        }

        public int JogWheelB
        {
            get { return jogWheelB; }
            set
            {
                if (checkValueIncKnob(value) == true)
                {
                    jogWheelB = value;
                }
                else
                {
                    jogWheelB = 0;
                }
            }
        }

        public int PlayA
        {
            get { return playA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    playA = value;
                }
                else
                {
                    playA = 0;
                }
            }
        }

        public int PlayB
        {
            get { return playB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    playB = value;
                }
                else
                {
                    playB = 0;
                }
            }
        }

        public int CueA
        {
            get { return cueA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    cueA = value;
                }
                else
                {
                    cueA = 0;
                }
            }
        }


        public int CueB
        {
            get { return cueB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    cueB = value;
                }
                else
                {
                    cueB = 0;
                }
            }
        }

        public int TrackNextA
        {
            get { return trackNextA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    trackNextA = value;
                }
                else
                {
                    trackNextA = 0;
                }
            }
        }

        public int TrackPrevA
        {
            get { return trackPrevA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    trackPrevA = value;
                }
                else
                {
                    trackPrevA = 0;
                }
            }
        }

        public int TrackNextB
        {
            get { return trackNextB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    trackNextB = value;
                }
                else
                {
                    trackNextB = 0;
                }
            }
        }
        public int TrackPrevB
        {
            get { return trackPrevB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    trackPrevB = value;
                }
                else
                {
                    trackPrevB = 0;
                }
            }
        }

        public int PitchDownA
        {
            get { return pitchDownA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    pitchDownA = value;
                }
                else
                {
                    pitchDownA = 0;
                }
            }
        }

        public int PitchUpA
        {
            get { return pitchUpA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    pitchUpA = value;
                }
                else
                {
                    pitchUpA = 0;
                }
            }

        }

        public int PitchUpB
        {
            get { return pitchUpB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    pitchUpB = value;
                }
                else
                {
                    pitchUpB = 0;
                }
            }
        }

        public int PitchDownB
        {
            get { return pitchDownB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    pitchDownB = value;
                }
                else
                {
                    pitchDownB = 0;
                }
            }
        }

        public int OneA
        {
            get { return oneA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    oneA = value;
                }
                else
                {
                    oneA = 0;
                }
            }
        }

        public int OneB
        {
            get { return oneB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    oneB = value;
                }
                else
                {
                    oneB = 0;
                }
            }
        }

        public int TwoA
        {
            get { return twoA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    twoA = value;
                }
                else
                {
                    twoA = 0;
                }
            }
        }

        public int TwoB
        {
            get { return twoB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    twoB = value;
                }
                else
                {
                    twoB = 0;
                }
            }
        }

        public int ThreeA
        {
            get { return threeA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    threeA = value;
                }
                else
                {
                    threeA = 0;
                }
            }
        }


        public int ThreeB
        {
            get { return threeB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    threeB = value;
                }
                else
                {
                    threeB = 0;
                }
            }
        }

        public int FourA
        {
            get { return fourA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    fourA = value;
                }
                else
                {
                    fourA = 0;
                }
            }
        }

        public int FourB
        {
            get { return fourB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    fourB = value;
                }
                else
                {
                    fourB = 0;
                }
            }
        }

        public int ListenA
        {
            get { return listenA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    listenA = value;
                }
                else
                {
                    listenA = 0;
                }
            }
        }

        public int ListenB
        {
            get { return listenB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    listenB = value;
                }
                else
                {
                    listenB = 0;
                }
            }
        }

        public int SyncA
        {
            get { return syncA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    syncA = value;
                }
                else
                {
                    syncA = 0;
                }
            }
        }

        public int SyncB
        {
            get { return syncB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    syncB = value;
                }
                else
                {
                    syncB = 0;
                }
            }
        }

        public int Up
        {
            get { return up; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    up = value;
                }
                else
                {
                    up = 0;
                }
            }
        }

        public int Down
        {
            get { return down; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    down = value;
                }
                else
                {
                    down = 0;
                }
            }
        }

        public int PitchResetA
        {
            get { return pitchResetA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    pitchResetA = value;
                }
                else
                {
                    pitchResetA = 0;
                }
            }
        }

        public int PitchResetB
        {
            get { return pitchResetB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    pitchResetB = value;
                }
                else
                {
                    pitchResetB = 0;
                }
            }
        }

        public int LoadA
        {
            get { return loadA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    loadA = value;
                }
                else
                {
                    loadA = 0;
                }
            }
        }

        public int LoadB
        {
            get { return loadB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    loadB = value;
                }
                else
                {
                    loadB = 0;
                }
            }
        }

        public int Files
        {
            get { return files; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    files = value;
                }
                else
                {
                    files = 0;
                }
            }
        }

        public int Folders
        {
            get { return folders; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    folders = value;
                }
                else
                {
                    folders = 0;
                }
            }
        }

        public int Vinyl
        {
            get { return vinyl; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    vinyl = value;
                }
                else
                {
                    vinyl = 0;
                }
            }
        }

        public int Magic
        {
            get { return magic; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    magic = value;
                }
                else
                {
                    magic = 0;
                }
            }
        }


        #endregion

        private bool checkValueButton(int i)
        {
            if (((i > 0) && (i <= 99)) || ((i >= 500) && (i <= 599)))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        private bool checkValueIncKnob(int i)
        {
            if ((i >= 100) && (i <= 199))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        private bool checkValueFixKnob(int i)
        {
            if ((i >= 200) && (i <= 299))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }


    }
}

