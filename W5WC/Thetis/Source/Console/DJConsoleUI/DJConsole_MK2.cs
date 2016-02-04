//=====================================================================
// DJConsole_MK2.cs
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

namespace Thetis.DJConsoleUI
{
    public class DJConsole_MK2
    {
        private readonly DJConsole m_parent;

        public DJConsole_MK2(DJConsole frm1)
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
        private int autobeatA = 0;
        private int autobeatB = 0;
        private int trackNextA = 0;
        private int trackPrevA = 0;
        private int trackNextB = 0;
        private int trackPrevB = 0;
        private int pitchDownA = 0;
        private int pitchUpA = 0;
        private int pitchUpB = 0;
        private int pitchDownB = 0;
        private int fXA = 0;
        private int fXB = 0;
        private int cueFXA = 0;
        private int cueFXB = 0;
        private int loopFxA = 0;
        private int loopFxB = 0;
        private int selectFxA = 0;
        private int selectFxB = 0;

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

        public int AutobeatA
        {
            get { return autobeatA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    autobeatA = value;
                }
                else
                {
                    autobeatA = 0;
                }
            }
        }

        public int AutobeatB
        {
            get { return autobeatB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    autobeatB = value;
                }
                else
                {
                    autobeatB = 0;
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
                    pitchDownA= value;
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

        public int FXA
        {
            get { return fXA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    fXA = value;
                }
                else
                {
                    fXA = 0;
                }
            }
        }

        public int FXB
        {
            get { return fXB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    fXB = value;
                }
                else
                {
                    fXB = 0;
                }
            }
        }


        public int CueFXA
        {
            get { return cueFXA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    cueFXA = value;
                }
                else
                {
                    cueFXA = 0;
                }
            }
        }

        public int CueFXB
        {
            get { return cueFXB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    cueFXB = value;
                }
                else
                {
                    cueFXB = 0;
                }
            }
        }

        public int LoopFxA
        {
            get { return loopFxA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    loopFxA = value;
                }
                else
                {
                    loopFxA = 0;
                }
            }
        }

        public int LoopFxB
        {
            get { return loopFxB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    loopFxB = value;
                }
                else
                {
                    loopFxB = 0;
                }
            }
        }

        public int SelectFxA
        {
            get { return selectFxA; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    selectFxA = value;
                }
                else
                {
                    selectFxA = 0;
                }
            }
        }


        public int SelectFxB
        {
            get { return selectFxB; }
            set
            {
                if (checkValueButton(value) == true)
                {
                    selectFxB = value;
                }
                else
                {
                    selectFxB = 0;
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
