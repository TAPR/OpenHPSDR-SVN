//=====================================================================
// Database.cs
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
using System.IO;
using System.Data;


namespace Thetis.DJConsoleUI
{
    static class Database
    {
        public static DataSet ds;

        private static string file_name = "";

        public static string FileName
        {
            set { file_name = value;}
        }

        public static void Init()
        {
            ds = new DataSet("Data");
            try
            {
                if (File.Exists(file_name))
                {
                    ds.ReadXml(file_name);
                }
            }
            catch
            {
            }
            VerifyTables();
            return;
        }

        public static void Update()
        {
            try
            {
                ds.WriteXml(file_name, XmlWriteMode.WriteSchema);
            }
            catch
            {
                return;
            }
        }

        public static void Exit()
        {
            Update();
            ds = null;
        }

        private static void VerifyTables()
        {
            bool _new = false;

            if (!ds.Tables.Contains("DJConsoleMK2"))
            {
                AddDJConsoleMK2();
                _new = true;
            }

            if (!ds.Tables.Contains("DJConsoleMP3e2"))
            {
                AddDJConsoleMP3e2();
                _new = true;
            }

            if (!ds.Tables.Contains("DJConsoleMP3LE"))
            {
                AddDJConsoleMP3LE();
                _new = true;
            }

            if (!ds.Tables.Contains("Configuration"))
            {
                AddSelectedConsole();
                _new = true;
            }

            if (_new == true)
            {
                Update();
            }
        }

        private static void AddSelectedConsole()
        {
            ds.Tables.Add("Configuration");
            DataTable t = ds.Tables["Configuration"];
            t.Columns.Add("SelectedConsole", typeof(int));
            DataRow dr = t.NewRow();
            dr["SelectedConsole"] = 0;
            t.Rows.Add(dr);
        }

        private static void AddDJConsoleMK2()
        {
            ds.Tables.Add("DJConsoleMK2");
            DataTable t = ds.Tables["DJConsoleMK2"];

            t.Columns.Add("TrebleA", typeof(int));
            t.Columns.Add("TrebleB", typeof(int));
            t.Columns.Add("MediumA", typeof(int));
            t.Columns.Add("MediumB", typeof(int));
            t.Columns.Add("BassA", typeof(int));
            t.Columns.Add("BassB", typeof(int));
            t.Columns.Add("VolumeA", typeof(int));
            t.Columns.Add("VolumeB", typeof(int));
            t.Columns.Add("Crossfader", typeof(int));
            t.Columns.Add("PitchA", typeof(int));
            t.Columns.Add("PitchB", typeof(int));
            t.Columns.Add("JogWheelA", typeof(int));
            t.Columns.Add("JogWheelB", typeof(int));
            t.Columns.Add("PlayA", typeof(int));
            t.Columns.Add("PlayB", typeof(int));
            t.Columns.Add("CueA", typeof(int));
            t.Columns.Add("CueB", typeof(int));
            t.Columns.Add("AutoBeatA", typeof(int));
            t.Columns.Add("AutoBeatB", typeof(int));
            t.Columns.Add("TrackNextA", typeof(int));
            t.Columns.Add("TrackNextB", typeof(int));
            t.Columns.Add("TrackPrevA", typeof(int));
            t.Columns.Add("TrackPrevB", typeof(int));
            t.Columns.Add("PitchUpA", typeof(int));
            t.Columns.Add("PitchUpB", typeof(int));
            t.Columns.Add("PitchDownA", typeof(int));
            t.Columns.Add("PitchDownB", typeof(int));
            t.Columns.Add("FxA", typeof(int));
            t.Columns.Add("FxB", typeof(int));
            t.Columns.Add("CueFxA", typeof(int));
            t.Columns.Add("CueFxB", typeof(int));
            t.Columns.Add("LoopFxA", typeof(int));
            t.Columns.Add("LoopFxB", typeof(int));
            t.Columns.Add("SelectFxA", typeof(int));
            t.Columns.Add("SelectFxB", typeof(int));

            DataRow dr = t.NewRow();
            dr["TrebleA"] = 0;
            dr["TrebleB"] = 0;
            dr["MediumA"] = 0;
            dr["MediumB"] = 0;
            dr["BassA"] = 0;
            dr["BassB"] = 0;
            dr["VolumeA"] = 0;
            dr["VolumeB"] = 0;
            dr["Crossfader"] = 0;
            dr["PitchA"] = 0;
            dr["PitchB"] = 0;
            dr["JogWheelA"] = 0;
            dr["JogWheelB"] = 0;
            dr["PlayA"] = 0;
            dr["PlayB"] = 0;
            dr["CueA"] = 0;
            dr["CueB"] = 0;
            dr["AutoBeatA"] = 0;
            dr["AutoBeatB"] = 0;
            dr["TrackNextA"] = 0;
            dr["TrackNextB"] = 0;
            dr["TrackPrevA"] = 0;
            dr["TrackPrevB"] = 0;
            dr["PitchUpA"] = 0;
            dr["PitchUpB"] = 0;
            dr["PitchDownA"] = 0;
            dr["PitchDownB"] = 0;
            dr["FxA"] = 0;
            dr["FxB"] = 0;
            dr["CueFxA"] = 0;
            dr["CueFxB"] = 0;
            dr["LoopFxA"] = 0;
            dr["LoopFxB"] = 0;
            dr["SelectFxA"] = 0;
            dr["SelectFxB"] = 0;
            t.Rows.Add(dr);
        }

        private static void AddDJConsoleMP3e2()
        {
            ds.Tables.Add("DJConsoleMP3e2");
            DataTable t = ds.Tables["DJConsoleMP3e2"];

            t.Columns.Add("TrebleA", typeof(int));
            t.Columns.Add("TrebleB", typeof(int));
            t.Columns.Add("MediumA", typeof(int));
            t.Columns.Add("MediumB", typeof(int));
            t.Columns.Add("BassA", typeof(int));
            t.Columns.Add("BassB", typeof(int));
            t.Columns.Add("VolumeA", typeof(int));
            t.Columns.Add("VolumeB", typeof(int));
            t.Columns.Add("Crossfader", typeof(int));
            t.Columns.Add("PitchA", typeof(int));
            t.Columns.Add("PitchB", typeof(int));
            t.Columns.Add("JogWheelA", typeof(int));
            t.Columns.Add("JogWheelB", typeof(int));
            t.Columns.Add("PlayA", typeof(int));
            t.Columns.Add("PlayB", typeof(int));
            t.Columns.Add("CueA", typeof(int));
            t.Columns.Add("CueB", typeof(int));
            t.Columns.Add("TrackNextA", typeof(int));
            t.Columns.Add("TrackNextB", typeof(int));
            t.Columns.Add("TrackPrevA", typeof(int));
            t.Columns.Add("TrackPrevB", typeof(int));
            t.Columns.Add("PitchUpA", typeof(int));
            t.Columns.Add("PitchUpB", typeof(int));
            t.Columns.Add("PitchDownA", typeof(int));
            t.Columns.Add("PitchDownB", typeof(int));
            t.Columns.Add("OneA", typeof(int));
            t.Columns.Add("OneB", typeof(int));
            t.Columns.Add("TwoA", typeof(int));
            t.Columns.Add("TwoB", typeof(int));
            t.Columns.Add("ThreeA", typeof(int));
            t.Columns.Add("ThreeB", typeof(int));
            t.Columns.Add("FourA", typeof(int));
            t.Columns.Add("FourB", typeof(int));
            t.Columns.Add("ListenA", typeof(int));
            t.Columns.Add("ListenB", typeof(int));
            t.Columns.Add("SyncA", typeof(int));
            t.Columns.Add("SyncB", typeof(int));
            t.Columns.Add("Up", typeof(int));
            t.Columns.Add("Down", typeof(int));
            t.Columns.Add("MasterTempoA", typeof(int));
            t.Columns.Add("MasterTempoB", typeof(int));
            t.Columns.Add("LoadA", typeof(int));
            t.Columns.Add("LoadB", typeof(int));
            t.Columns.Add("Files", typeof(int));
            t.Columns.Add("Folders", typeof(int));
            t.Columns.Add("Scratch", typeof(int));
            t.Columns.Add("Automix", typeof(int));

            DataRow dr = t.NewRow();
            dr["TrebleA"] = 0;
            dr["TrebleB"] = 0;
            dr["MediumA"] = 0;
            dr["MediumB"] = 0;
            dr["BassA"] = 0;
            dr["BassB"] = 0;
            dr["VolumeA"] = 0;
            dr["VolumeB"] = 0;
            dr["Crossfader"] = 0;
            dr["PitchA"] = 0;
            dr["PitchB"] = 0;
            dr["JogWheelA"] = 0;
            dr["JogWheelB"] = 0;
            dr["PlayA"] = 0;
            dr["PlayB"] = 0;
            dr["CueA"] = 0;
            dr["CueB"] = 0;
            dr["TrackNextA"] = 0;
            dr["TrackNextB"] = 0;
            dr["TrackPrevA"] = 0;
            dr["TrackPrevB"] = 0;
            dr["PitchUpA"] = 0;
            dr["PitchUpB"] = 0;
            dr["PitchDownA"] = 0;
            dr["PitchDownB"] = 0;
            dr["OneA"] = 0;
            dr["OneB"] = 0;
            dr["TwoA"] = 0;
            dr["TwoB"] = 0;
            dr["ThreeA"] = 0;
            dr["ThreeB"] = 0;
            dr["FourA"] = 0;
            dr["FourB"] = 0;
            dr["ListenA"] = 0;
            dr["ListenB"] = 0;
            dr["SyncA"] = 0;
            dr["SyncB"] = 0;
            dr["Up"] = 0;
            dr["Down"] = 0;
            dr["MasterTempoA"] = 0;
            dr["MasterTempoB"] = 0;
            dr["LoadA"] = 0;
            dr["LoadB"] = 0;
            dr["Files"] = 0;
            dr["Folders"] = 0;
            dr["Scratch"] = 0;
            dr["Automix"] = 0;
            t.Rows.Add(dr);

        }

        private static void AddDJConsoleMP3LE()
        {
            ds.Tables.Add("DJConsoleMP3LE");
            DataTable t = ds.Tables["DJConsoleMP3LE"];

            t.Columns.Add("TrebleA", typeof(int));
            t.Columns.Add("TrebleB", typeof(int));
            t.Columns.Add("MediumA", typeof(int));
            t.Columns.Add("MediumB", typeof(int));
            t.Columns.Add("BassA", typeof(int));
            t.Columns.Add("BassB", typeof(int));
            t.Columns.Add("VolumeA", typeof(int));
            t.Columns.Add("VolumeB", typeof(int));
            t.Columns.Add("Crossfader", typeof(int));
            t.Columns.Add("PitchA", typeof(int));
            t.Columns.Add("PitchB", typeof(int));
            t.Columns.Add("JogWheelA", typeof(int));
            t.Columns.Add("JogWheelB", typeof(int));
            t.Columns.Add("PlayA", typeof(int));
            t.Columns.Add("PlayB", typeof(int));
            t.Columns.Add("CueA", typeof(int));
            t.Columns.Add("CueB", typeof(int));
            t.Columns.Add("TrackNextA", typeof(int));
            t.Columns.Add("TrackNextB", typeof(int));
            t.Columns.Add("TrackPrevA", typeof(int));
            t.Columns.Add("TrackPrevB", typeof(int));
            t.Columns.Add("PitchUpA", typeof(int));
            t.Columns.Add("PitchUpB", typeof(int));
            t.Columns.Add("PitchDownA", typeof(int));
            t.Columns.Add("PitchDownB", typeof(int));
            t.Columns.Add("OneA", typeof(int));
            t.Columns.Add("OneB", typeof(int));
            t.Columns.Add("TwoA", typeof(int));
            t.Columns.Add("TwoB", typeof(int));
            t.Columns.Add("ThreeA", typeof(int));
            t.Columns.Add("ThreeB", typeof(int));
            t.Columns.Add("FourA", typeof(int));
            t.Columns.Add("FourB", typeof(int));
            t.Columns.Add("ListenA", typeof(int));
            t.Columns.Add("ListenB", typeof(int));
            t.Columns.Add("SyncA", typeof(int));
            t.Columns.Add("SyncB", typeof(int));
            t.Columns.Add("Up", typeof(int));
            t.Columns.Add("Down", typeof(int));
            t.Columns.Add("PitchResetA", typeof(int));
            t.Columns.Add("PitchResetB", typeof(int));
            t.Columns.Add("LoadA", typeof(int));
            t.Columns.Add("LoadB", typeof(int));
            t.Columns.Add("Files", typeof(int));
            t.Columns.Add("Folders", typeof(int));
            t.Columns.Add("Vinyl", typeof(int));
            t.Columns.Add("Magic", typeof(int));

            DataRow dr = t.NewRow();
            dr["TrebleA"] = 0;
            dr["TrebleB"] = 0;
            dr["MediumA"] = 0;
            dr["MediumB"] = 0;
            dr["BassA"] = 0;
            dr["BassB"] = 0;
            dr["VolumeA"] = 0;
            dr["VolumeB"] = 0;
            dr["Crossfader"] = 0;
            dr["PitchA"] = 0;
            dr["PitchB"] = 0;
            dr["JogWheelA"] = 0;
            dr["JogWheelB"] = 0;
            dr["PlayA"] = 0;
            dr["PlayB"] = 0;
            dr["CueA"] = 0;
            dr["CueB"] = 0;
            dr["TrackNextA"] = 0;
            dr["TrackNextB"] = 0;
            dr["TrackPrevA"] = 0;
            dr["TrackPrevB"] = 0;
            dr["PitchUpA"] = 0;
            dr["PitchUpB"] = 0;
            dr["PitchDownA"] = 0;
            dr["PitchDownB"] = 0;
            dr["OneA"] = 0;
            dr["OneB"] = 0;
            dr["TwoA"] = 0;
            dr["TwoB"] = 0;
            dr["ThreeA"] = 0;
            dr["ThreeB"] = 0;
            dr["FourA"] = 0;
            dr["FourB"] = 0;
            dr["ListenA"] = 0;
            dr["ListenB"] = 0;
            dr["SyncA"] = 0;
            dr["SyncB"] = 0;
            dr["Up"] = 0;
            dr["Down"] = 0;
            dr["PitchResetA"] = 0;
            dr["PitchResetB"] = 0;
            dr["LoadA"] = 0;
            dr["LoadB"] = 0;
            dr["Files"] = 0;
            dr["Folders"] = 0;
            dr["Vinyl"] = 0;
            dr["Magic"] = 0;
            t.Rows.Add(dr);

        }

    }
}

