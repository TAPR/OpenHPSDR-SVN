//=====================================================================
// DJConsole.cs
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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Thetis
{

    public enum DJConsoleModels
    {
        NotSupported,
        HerculesMK2,
        HerculesMP3e2,
        HerculesMP3LE,
        HerculesRmx
    }

    public partial class DJConsole
    {
        private Console m_parent;

        public DJConsoleUI.DJConsole_MK2 DJConsole_MK2;
        public DJConsoleUI.DJConsole_MP3e2 DJConsole_MP3e2;
        public DJConsoleUI.DJConsole_MP3LE DJConsole_MP3LE;
        public DJConsoleUI.DJConsole_Rmx DJConsole_Rmx;

        // private InputDevice inDevice;
        // private OutputDevice outDevice;
        private CATParser parser;
        private CATCommands commands;
        private int selectedConsole = 0;

        private Dictionary<int, string> DictButtons = new Dictionary<int, string>();
        private Dictionary<int, string> DictIncKnobs = new Dictionary<int, string>();
        private Dictionary<int, string> DictFixKnobs = new Dictionary<int, string>();
        private Dictionary<int, string> SupportedConsoles = new Dictionary<int, string>();
        private Dictionary<int, string> ConnectedConsoles = new Dictionary<int, string>();

        private DateTime LastVfoAUpdateTime;
        private int VfoAStepValue;
        private int NumVfoASteps;
        private int VfoALastSpeed=-1;
        private DateTime VfoALastSpeedChangeTime;
        private int VfoAConsoleStep = 1000;

        #region Properties

        public Dictionary<int, string> dictFixKnobs
        {
            get { return DictFixKnobs; }
            set { }
        }

        public Dictionary<int, string> dictIncKnobs
        {
            get { return DictIncKnobs; }
            set { }
        }

        public Dictionary<int, string> dictButtons
        {
            get { return DictButtons; }
            set { }
        }

        public Dictionary<int, string> supportedConsoles
        {
            get { return SupportedConsoles; }
            set { }
        }

        public Dictionary<int, string> connectedConsoles
        {
            get { return ConnectedConsoles; }
            set { }
        }

        public int SelectedConsole
        {
            get { return selectedConsole; }
            set
            {
                selectedConsole = value;
                SaveSettings();
            }
        }

        #endregion

        #region Constructor

        public DJConsole(Console c)
        {
            m_parent = c;
            LoadDictionaries();
            DetectConnectedConsoles();
            DJConsole_MK2 = new Thetis.DJConsoleUI.DJConsole_MK2(this);
            DJConsole_MP3e2 = new Thetis.DJConsoleUI.DJConsole_MP3e2(this);
            DJConsole_MP3LE = new Thetis.DJConsoleUI.DJConsole_MP3LE(this);
            DJConsole_Rmx = new Thetis.DJConsoleUI.DJConsole_Rmx(this);

            LoadSettings();
            FillTables();

            if ((Load_inDevice() == true) & (Load_outDevice() == true))
            {
                // inDevice.StartRecording();
                // ResetDJConsole();
                parser = new CATParser(m_parent);
                commands = new CATCommands(m_parent, parser);
            }

        }

        #endregion Constructor

        public void Reload()
        {
            // inDevice.StopRecording();
            // inDevice.Close();
            // outDevice.Close();
            Close();
            DJConsoleUI.Database.Exit();
            LoadSettings();
            if ((Load_inDevice() == true) & (Load_outDevice() == true))
            {
                //  inDevice.StartRecording();
                // ResetDJConsole();
                parser = new CATParser(m_parent);
                commands = new CATCommands(m_parent, parser);
            }

        }

        public bool Load_inDevice()
        {
            List<string> ListInDevices = new List<string>();

            //check if a MIDI Device (INPUT) is connected and initiate it

            if (DeviceInCount == 0)
            //if no device connected, just exit
            {
                return (false);
            }
            else
            {
                try
                {
                    for (int i = 0; i < DeviceInCount; i++)
                    {
                        // string a = InputDevice.GetDeviceCapabilities(i).name;
                        string a = MidiInGetName(i);
                        // MessageBox.Show("DJ Console Found: " + a, "Debugging Message"); //Debug Message
                        string[] b = a.Split(new Char[] { '-' });
                        string djconsole_name;
                        if (b.Length > 1)
                        {
                            djconsole_name = b[1];
                            djconsole_name = djconsole_name.Trim();
                        }
                        else
                        {
                            djconsole_name = a;
                        }

                        ListInDevices.Add(djconsole_name);


                        //MessageBox.Show("MID: " + Sanford.Multimedia.Midi.InputDevice.GetDeviceCapabilities(i).mid.ToString() + "\r\n"
                        //    + "PID: " + Sanford.Multimedia.Midi.InputDevice.GetDeviceCapabilities(i).pid.ToString() + "\r\n"
                        //    + "Name: " + Sanford.Multimedia.Midi.InputDevice.GetDeviceCapabilities(i).name.ToString() + "\r\n" 
                        //    + "xx: " + Sanford.Multimedia.Midi.InputDevice.GetDeviceCapabilities(i).support.ToString() + "\r\n"
                        //    + "Driver Version: " + Sanford.Multimedia.Midi.InputDevice.GetDeviceCapabilities(i).driverVersion.ToString());
                    }
                    //try to load the preset Console type

                    if (ConnectedConsoles.ContainsKey(this.SelectedConsole) == true)
                    {
                        //  inDevice = new InputDevice(ListInDevices.IndexOf(ConnectedConsoles[this.SelectedConsole]));
                        OpenMidiIn(ListInDevices.IndexOf(ConnectedConsoles[this.SelectedConsole]));
                        //inDevice = new InputDevice(this.SelectedConsole);
                        // inDevice.ChannelMessageReceived += new EventHandler<ChannelMessageEventArgs>(inDevice_ChannelMessageReceived);
                        return (true);
                    }

                    //if preset console type is isn't connected try to load one of the supported Consoles
                    for (int i = 0; i < supportedConsoles.Count; i++)
                    {
                        if (ListInDevices.Contains(SupportedConsoles[i].ToString()) == true)
                        {
                            OpenMidiIn(ListInDevices.IndexOf(SupportedConsoles[i].ToString()));
                            // inDevice = new InputDevice(ListInDevices.IndexOf(SupportedConsoles[i].ToString()));
                            //inDevice = new InputDevice(i);
                            // inDevice.ChannelMessageReceived += new EventHandler<ChannelMessageEventArgs>(inDevice_ChannelMessageReceived);
                            return (true);
                        }
                    }

                    //if the connected MIDI Device is not supported, return false
                }



                //{
                //    for (int i = 0; i < Sanford.Multimedia.Midi.InputDevice.DeviceCount; i++)
                //    {
                //        ListInDevices.Add(Sanford.Multimedia.Midi.InputDevice.GetDeviceCapabilities(i).name);
                //    }

                //    //try to load the preset Console type

                //    if (ConnectedConsoles.ContainsKey(this.SelectedConsole) == true)
                //    {
                //        inDevice = new InputDevice(ListInDevices.IndexOf(ConnectedConsoles[this.SelectedConsole]));
                //        //inDevice = new InputDevice(this.SelectedConsole);
                //        inDevice.ChannelMessageReceived += new EventHandler<ChannelMessageEventArgs>(inDevice_ChannelMessageReceived);
                //        return (true);
                //    }

                //    //if preset console type is isn't connected try to load one of the supported Consoles
                //    for (int i = 0; i < supportedConsoles.Count; i++)
                //    {
                //        if (ListInDevices.Contains(SupportedConsoles[i].ToString()) == true)
                //        {
                //            inDevice = new InputDevice(ListInDevices.IndexOf(SupportedConsoles[i].ToString()));
                //            //inDevice = new InputDevice(i);
                //            inDevice.ChannelMessageReceived += new EventHandler<ChannelMessageEventArgs>(inDevice_ChannelMessageReceived);
                //            return (true);
                //        }
                //    }

                //    //if the connected MIDI Device is not supported, return false

                //}

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error! There are problems with your MIDI Output Device!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    Close();
                    return (false);
                }
            }
            return (false);
        }

        public bool Load_outDevice()
        {
            List<string> ListOutDevices = new List<string>();

            //check if a MIDI Device (OUTPUT) is connected and initiate it

            if (DeviceOutCount == 0)
            {
                //if no device connected, just leave
                return (false);
            }

            else
            {
                try
                {
                    for (int i = 0; i < DeviceOutCount; i++)
                    {
                        string a = MidiOutGetName(i);
                        string[] b = a.Split(new Char[] { '-' });
                        string djconsole_name;
                        if (b.Length > 1)
                        {
                            djconsole_name = b[1];
                            djconsole_name = djconsole_name.Trim();
                        }
                        else
                        {
                            djconsole_name = a;
                        }

                        ListOutDevices.Add(djconsole_name);
                    }

                    //try to load the preset Console type

                    if (ConnectedConsoles.ContainsKey(this.SelectedConsole) == true)
                    {
                        // outDevice = new OutputDevice(ListOutDevices.IndexOf(ConnectedConsoles[this.SelectedConsole]));
                        OpenMidiOut(ListOutDevices.IndexOf(ConnectedConsoles[this.SelectedConsole]));
                        return (true);
                    }

                    //if preset console type is isn't connected try to load one of the supported Consoles
                    for (int i = 0; i < supportedConsoles.Count; i++)
                    {
                        if (ListOutDevices.Contains(SupportedConsoles[i].ToString()) == true)
                        {
                            // outDevice = new OutputDevice(ListOutDevices.IndexOf(SupportedConsoles[i].ToString()));
                            OpenMidiOut(ListOutDevices.IndexOf(SupportedConsoles[i].ToString()));
                            return (true);
                        }
                    }

                    //if the connected MIDI Device is not supported, return false
                }

                //{
                //    for (int i = 0; i < Sanford.Multimedia.Midi.OutputDevice.DeviceCount; i++)
                //    {
                //        ListOutDevices.Add(Sanford.Multimedia.Midi.OutputDevice.GetDeviceCapabilities(i).name);
                //    }

                //    //try to load the preset Console type

                //    if (ConnectedConsoles.ContainsKey(this.SelectedConsole) == true)
                //    {
                //        outDevice = new OutputDevice(ListOutDevices.IndexOf(ConnectedConsoles[this.SelectedConsole]));
                //        return (true);
                //    }

                //    //if preset console type is isn't connected try to load one of the supported Consoles
                //    for (int i = 0; i < supportedConsoles.Count; i++)
                //    {
                //        if (ListOutDevices.Contains(SupportedConsoles[i].ToString()) == true)
                //        {
                //            outDevice = new OutputDevice(ListOutDevices.IndexOf(SupportedConsoles[i].ToString()));
                //            return (true);
                //        }
                //    }

                //    //if the connected MIDI Device is not supported, return false
                //}

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error! There are problems with your MIDI Output Device!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    Close();
                    return (false);
                }
            }
            return (false);
        }


        //Load the DJ Console settings / mappings
        public void LoadSettings()
        {
            try
            {
                DJConsoleUI.Database.FileName = m_parent.AppDataPath + "DJConsoleSettings.xml";
                DJConsoleUI.Database.Init();
                DataTable t = DJConsoleUI.Database.ds.Tables["DJConsoleMK2"];
                DataRow dr = t.Rows[0];
                this.DJConsole_MK2.TrebleA = (int)dr[0];
                this.DJConsole_MK2.TrebleB = (int)dr[1];
                this.DJConsole_MK2.MediumA = (int)dr[2];
                this.DJConsole_MK2.MediumB = (int)dr[3];
                this.DJConsole_MK2.BassA = (int)dr[4];
                this.DJConsole_MK2.BassB = (int)dr[5];
                this.DJConsole_MK2.VolumeA = (int)dr[6];
                this.DJConsole_MK2.VolumeB = (int)dr[7];
                this.DJConsole_MK2.Crossfader = (int)dr[8];
                this.DJConsole_MK2.PitchA = (int)dr[9];
                this.DJConsole_MK2.PitchB = (int)dr[10];
                this.DJConsole_MK2.JogWheelA = (int)dr[11];
                this.DJConsole_MK2.JogWheelB = (int)dr[12];
                this.DJConsole_MK2.PlayA = (int)dr[13];
                this.DJConsole_MK2.PlayB = (int)dr[14];
                this.DJConsole_MK2.CueA = (int)dr[15];
                this.DJConsole_MK2.CueB = (int)dr[16];
                this.DJConsole_MK2.AutobeatA = (int)dr[17];
                this.DJConsole_MK2.AutobeatB = (int)dr[18];
                this.DJConsole_MK2.TrackNextA = (int)dr[19];
                this.DJConsole_MK2.TrackNextB = (int)dr[20];
                this.DJConsole_MK2.TrackPrevA = (int)dr[21];
                this.DJConsole_MK2.TrackPrevB = (int)dr[22];
                this.DJConsole_MK2.PitchUpA = (int)dr[23];
                this.DJConsole_MK2.PitchUpB = (int)dr[24];
                this.DJConsole_MK2.PitchDownA = (int)dr[25];
                this.DJConsole_MK2.PitchDownB = (int)dr[26];
                this.DJConsole_MK2.FXA = (int)dr[27];
                this.DJConsole_MK2.FXB = (int)dr[28];
                this.DJConsole_MK2.CueFXA = (int)dr[29];
                this.DJConsole_MK2.CueFXB = (int)dr[30];
                this.DJConsole_MK2.LoopFxA = (int)dr[31];
                this.DJConsole_MK2.LoopFxB = (int)dr[32];
                this.DJConsole_MK2.SelectFxA = (int)dr[33];
                this.DJConsole_MK2.SelectFxB = (int)dr[34];

                t = DJConsoleUI.Database.ds.Tables["DJConsoleMP3e2"];
                dr = t.Rows[0];
                this.DJConsole_MP3e2.TrebleA = (int)dr[0];
                this.DJConsole_MP3e2.TrebleB = (int)dr[1];
                this.DJConsole_MP3e2.MediumA = (int)dr[2];
                this.DJConsole_MP3e2.MediumB = (int)dr[3];
                this.DJConsole_MP3e2.BassA = (int)dr[4];
                this.DJConsole_MP3e2.BassB = (int)dr[5];
                this.DJConsole_MP3e2.VolumeA = (int)dr[6];
                this.DJConsole_MP3e2.VolumeB = (int)dr[7];
                this.DJConsole_MP3e2.Crossfader = (int)dr[8];
                this.DJConsole_MP3e2.PitchA = (int)dr[9];
                this.DJConsole_MP3e2.PitchB = (int)dr[10];
                this.DJConsole_MP3e2.JogWheelA = (int)dr[11];
                this.DJConsole_MP3e2.JogWheelB = (int)dr[12];
                this.DJConsole_MP3e2.PlayA = (int)dr[13];
                this.DJConsole_MP3e2.PlayB = (int)dr[14];
                this.DJConsole_MP3e2.CueA = (int)dr[15];
                this.DJConsole_MP3e2.CueB = (int)dr[16];
                this.DJConsole_MP3e2.TrackNextA = (int)dr[17];
                this.DJConsole_MP3e2.TrackNextB = (int)dr[18];
                this.DJConsole_MP3e2.TrackPrevA = (int)dr[19];
                this.DJConsole_MP3e2.TrackPrevB = (int)dr[20];
                this.DJConsole_MP3e2.PitchUpA = (int)dr[21];
                this.DJConsole_MP3e2.PitchUpB = (int)dr[22];
                this.DJConsole_MP3e2.PitchDownA = (int)dr[23];
                this.DJConsole_MP3e2.PitchDownB = (int)dr[24];
                this.DJConsole_MP3e2.OneA = (int)dr[25];
                this.DJConsole_MP3e2.OneB = (int)dr[26];
                this.DJConsole_MP3e2.TwoA = (int)dr[27];
                this.DJConsole_MP3e2.TwoB = (int)dr[28];
                this.DJConsole_MP3e2.ThreeA = (int)dr[29];
                this.DJConsole_MP3e2.ThreeB = (int)dr[30];
                this.DJConsole_MP3e2.FourA = (int)dr[31];
                this.DJConsole_MP3e2.FourB = (int)dr[32];
                this.DJConsole_MP3e2.ListenA = (int)dr[33];
                this.DJConsole_MP3e2.ListenB = (int)dr[34];
                this.DJConsole_MP3e2.SyncA = (int)dr[35];
                this.DJConsole_MP3e2.SyncB = (int)dr[36];
                this.DJConsole_MP3e2.Up = (int)dr[37];
                this.DJConsole_MP3e2.Down = (int)dr[38];
                this.DJConsole_MP3e2.MasterTempoA = (int)dr[39];
                this.DJConsole_MP3e2.MasterTempoB = (int)dr[40];
                this.DJConsole_MP3e2.LoadA = (int)dr[41];
                this.DJConsole_MP3e2.LoadB = (int)dr[42];
                this.DJConsole_MP3e2.Files = (int)dr[43];
                this.DJConsole_MP3e2.Folders = (int)dr[44];
                this.DJConsole_MP3e2.Scratch = (int)dr[45];
                this.DJConsole_MP3e2.Automix = (int)dr[46];

                t = DJConsoleUI.Database.ds.Tables["DJConsoleMP3LE"];
                dr = t.Rows[0];
                this.DJConsole_MP3LE.TrebleA = (int)dr[0];
                this.DJConsole_MP3LE.TrebleB = (int)dr[1];
                this.DJConsole_MP3LE.MediumA = (int)dr[2];
                this.DJConsole_MP3LE.MediumB = (int)dr[3];
                this.DJConsole_MP3LE.BassA = (int)dr[4];
                this.DJConsole_MP3LE.BassB = (int)dr[5];
                this.DJConsole_MP3LE.VolumeA = (int)dr[6];
                this.DJConsole_MP3LE.VolumeB = (int)dr[7];
                this.DJConsole_MP3LE.Crossfader = (int)dr[8];
                this.DJConsole_MP3LE.PitchA = (int)dr[9];
                this.DJConsole_MP3LE.PitchB = (int)dr[10];
                this.DJConsole_MP3LE.JogWheelA = (int)dr[11];
                this.DJConsole_MP3LE.JogWheelB = (int)dr[12];
                this.DJConsole_MP3LE.PlayA = (int)dr[13];
                this.DJConsole_MP3LE.PlayB = (int)dr[14];
                this.DJConsole_MP3LE.CueA = (int)dr[15];
                this.DJConsole_MP3LE.CueB = (int)dr[16];
                this.DJConsole_MP3LE.TrackNextA = (int)dr[17];
                this.DJConsole_MP3LE.TrackNextB = (int)dr[18];
                this.DJConsole_MP3LE.TrackPrevA = (int)dr[19];
                this.DJConsole_MP3LE.TrackPrevB = (int)dr[20];
                this.DJConsole_MP3LE.PitchUpA = (int)dr[21];
                this.DJConsole_MP3LE.PitchUpB = (int)dr[22];
                this.DJConsole_MP3LE.PitchDownA = (int)dr[23];
                this.DJConsole_MP3LE.PitchDownB = (int)dr[24];
                this.DJConsole_MP3LE.OneA = (int)dr[25];
                this.DJConsole_MP3LE.OneB = (int)dr[26];
                this.DJConsole_MP3LE.TwoA = (int)dr[27];
                this.DJConsole_MP3LE.TwoB = (int)dr[28];
                this.DJConsole_MP3LE.ThreeA = (int)dr[29];
                this.DJConsole_MP3LE.ThreeB = (int)dr[30];
                this.DJConsole_MP3LE.FourA = (int)dr[31];
                this.DJConsole_MP3LE.FourB = (int)dr[32];
                this.DJConsole_MP3LE.ListenA = (int)dr[33];
                this.DJConsole_MP3LE.ListenB = (int)dr[34];
                this.DJConsole_MP3LE.SyncA = (int)dr[35];
                this.DJConsole_MP3LE.SyncB = (int)dr[36];
                this.DJConsole_MP3LE.Up = (int)dr[37];
                this.DJConsole_MP3LE.Down = (int)dr[38];
                this.DJConsole_MP3LE.PitchResetA = (int)dr[39];
                this.DJConsole_MP3LE.PitchResetB = (int)dr[40];
                this.DJConsole_MP3LE.LoadA = (int)dr[41];
                this.DJConsole_MP3LE.LoadB = (int)dr[42];
                this.DJConsole_MP3LE.Files = (int)dr[43];
                this.DJConsole_MP3LE.Folders = (int)dr[44];
                this.DJConsole_MP3LE.Vinyl = (int)dr[45];
                this.DJConsole_MP3LE.Magic = (int)dr[46];
                
                t = DJConsoleUI.Database.ds.Tables["DJConsoleRmx"];
                dr = t.Rows[0];
                this.DJConsole_Rmx.TrebleA = (int)dr[0];
                this.DJConsole_Rmx.TrebleB = (int)dr[1];
                this.DJConsole_Rmx.MediumA = (int)dr[2];
                this.DJConsole_Rmx.MediumB = (int)dr[3];
                this.DJConsole_Rmx.BassA = (int)dr[4];
                this.DJConsole_Rmx.BassB = (int)dr[5];
                this.DJConsole_Rmx.VolumeA = (int)dr[6];
                this.DJConsole_Rmx.VolumeB = (int)dr[7];
                this.DJConsole_Rmx.Crossfader = (int)dr[8];
                this.DJConsole_Rmx.PitchA = (int)dr[9];
                this.DJConsole_Rmx.PitchB = (int)dr[10];
                this.DJConsole_Rmx.JogWheelA = (int)dr[11];
                this.DJConsole_Rmx.JogWheelB = (int)dr[12];
                this.DJConsole_Rmx.PlayA = (int)dr[13];
                this.DJConsole_Rmx.PlayB = (int)dr[14];
                this.DJConsole_Rmx.CueA = (int)dr[15];
                this.DJConsole_Rmx.CueB = (int)dr[16];
                this.DJConsole_Rmx.TrackNextA = (int)dr[17];
                this.DJConsole_Rmx.TrackNextB = (int)dr[18];
                this.DJConsole_Rmx.TrackPrevA = (int)dr[19];
                this.DJConsole_Rmx.TrackPrevB = (int)dr[20];
                this.DJConsole_Rmx.BeatLockA = (int)dr[21];
                this.DJConsole_Rmx.BeatLockB = (int)dr[22];
                this.DJConsole_Rmx.SourceA = (int)dr[23];
                this.DJConsole_Rmx.SourceB = (int)dr[24];
                this.DJConsole_Rmx.OneA = (int)dr[25];
                this.DJConsole_Rmx.OneB = (int)dr[26];
                this.DJConsole_Rmx.TwoA = (int)dr[27];
                this.DJConsole_Rmx.TwoB = (int)dr[28];
                this.DJConsole_Rmx.ThreeA = (int)dr[29];
                this.DJConsole_Rmx.ThreeB = (int)dr[30];
                this.DJConsole_Rmx.FourA = (int)dr[31];
                this.DJConsole_Rmx.FourB = (int)dr[32];
                this.DJConsole_Rmx.SelectA = (int)dr[33];
                this.DJConsole_Rmx.SelectB = (int)dr[34];
                this.DJConsole_Rmx.SyncA = (int)dr[35];
                this.DJConsole_Rmx.SyncB = (int)dr[36];
                this.DJConsole_Rmx.Up = (int)dr[37];
                this.DJConsole_Rmx.Down = (int)dr[38];
                this.DJConsole_Rmx.PitchResetA = (int)dr[39];
                this.DJConsole_Rmx.PitchResetB = (int)dr[40];
                this.DJConsole_Rmx.LoadA = (int)dr[41];
                this.DJConsole_Rmx.LoadB = (int)dr[42];
                this.DJConsole_Rmx.SixB = (int)dr[43];
                this.DJConsole_Rmx.FiveB = (int)dr[44];
                this.DJConsole_Rmx.FiveA = (int)dr[45];
                this.DJConsole_Rmx.SixA = (int)dr[46];
                this.DJConsole_Rmx.TrebleKillA = (int)dr[47];
                this.DJConsole_Rmx.MediumKillA = (int)dr[48];
                this.DJConsole_Rmx.BassKillA = (int)dr[49];
                this.DJConsole_Rmx.TrebleKillB = (int)dr[50];
                this.DJConsole_Rmx.MediumKillB = (int)dr[51];
                this.DJConsole_Rmx.BassKillB = (int)dr[52];
                this.DJConsole_Rmx.Scratch = (int)dr[53];
                this.DJConsole_Rmx.Left = (int)dr[54];
                this.DJConsole_Rmx.Right = (int)dr[55];
                this.DJConsole_Rmx.StopA = (int)dr[56];
                this.DJConsole_Rmx.StopB = (int)dr[57];
                this.DJConsole_Rmx.Balance = (int)dr[58];
                this.DJConsole_Rmx.GainA = (int)dr[59];
                this.DJConsole_Rmx.GainB = (int)dr[60];
                this.DJConsole_Rmx.MonSelect = (int)dr[61];
                this.DJConsole_Rmx.VolMain = (int)dr[62];
                this.DJConsole_Rmx.MikeToggle = (int)dr[63];

                t = DJConsoleUI.Database.ds.Tables["Configuration"];
                dr = t.Rows[0];
                this.selectedConsole = (int)dr[0];
            }
            catch
            {
                //MessageBox.Show("DJ Console Settings could not be written to disk", "Disc Writing Error");
                return;
            }
        }


        public void SaveSettings()
        {
            try
            {
                string fileName = m_parent.AppDataPath + "DJConsoleSettings.xml";
                DJConsoleUI.Database.FileName = fileName;
                //DJConsoleUI.Database.Init();
                DataTable t = DJConsoleUI.Database.ds.Tables["DJConsoleMK2"];
                if (t.Rows.Count > 0)
                {
                    t.Rows[0].Delete();
                }
                DataRow dr = t.NewRow();
                dr["TrebleA"] = this.DJConsole_MK2.TrebleA;
                dr["TrebleB"] = this.DJConsole_MK2.TrebleB;
                dr["MediumA"] = this.DJConsole_MK2.MediumA;
                dr["MediumB"] = this.DJConsole_MK2.MediumB;
                dr["BassA"] = this.DJConsole_MK2.BassA;
                dr["BassB"] = this.DJConsole_MK2.BassB;
                dr["VolumeA"] = this.DJConsole_MK2.VolumeA;
                dr["VolumeB"] = this.DJConsole_MK2.VolumeB;
                dr["Crossfader"] = this.DJConsole_MK2.Crossfader;
                dr["PitchA"] = this.DJConsole_MK2.PitchA;
                dr["PitchB"] = this.DJConsole_MK2.PitchB;
                dr["JogWheelA"] = this.DJConsole_MK2.JogWheelA;
                dr["JogWheelB"] = this.DJConsole_MK2.JogWheelB;
                dr["PlayA"] = this.DJConsole_MK2.PlayA;
                dr["PlayB"] = this.DJConsole_MK2.PlayB;
                dr["CueA"] = this.DJConsole_MK2.CueA;
                dr["CueB"] = this.DJConsole_MK2.CueB;
                dr["AutoBeatA"] = this.DJConsole_MK2.AutobeatA;
                dr["AutoBeatB"] = this.DJConsole_MK2.AutobeatB;
                dr["TrackNextA"] = this.DJConsole_MK2.TrackNextA;
                dr["TrackNextB"] = this.DJConsole_MK2.TrackNextB;
                dr["TrackPrevA"] = this.DJConsole_MK2.TrackPrevA;
                dr["TrackPrevB"] = this.DJConsole_MK2.TrackPrevB;
                dr["PitchUpA"] = this.DJConsole_MK2.PitchUpA;
                dr["PitchUpB"] = this.DJConsole_MK2.PitchUpB;
                dr["PitchDownA"] = this.DJConsole_MK2.PitchDownA;
                dr["PitchDownB"] = this.DJConsole_MK2.PitchDownB;
                dr["FxA"] = this.DJConsole_MK2.FXA;
                dr["FxB"] = this.DJConsole_MK2.FXB;
                dr["CueFxA"] = this.DJConsole_MK2.CueFXA;
                dr["CueFxB"] = this.DJConsole_MK2.CueFXB;
                dr["LoopFxA"] = this.DJConsole_MK2.LoopFxA;
                dr["LoopFxB"] = this.DJConsole_MK2.LoopFxB;
                dr["SelectFxA"] = this.DJConsole_MK2.SelectFxA;
                dr["SelectFxB"] = this.DJConsole_MK2.SelectFxB;
                t.Rows.Add(dr);

                t = DJConsoleUI.Database.ds.Tables["DJConsoleMP3e2"];
                if (t.Rows.Count > 0)
                {
                    t.Rows[0].Delete();
                }
                dr = t.NewRow();
                dr["TrebleA"] = this.DJConsole_MP3e2.TrebleA;
                dr["TrebleB"] = this.DJConsole_MP3e2.TrebleB;
                dr["MediumA"] = this.DJConsole_MP3e2.MediumA;
                dr["MediumB"] = this.DJConsole_MP3e2.MediumB;
                dr["BassA"] = this.DJConsole_MP3e2.BassA;
                dr["BassB"] = this.DJConsole_MP3e2.BassB;
                dr["VolumeA"] = this.DJConsole_MP3e2.VolumeA;
                dr["VolumeB"] = this.DJConsole_MP3e2.VolumeB;
                dr["Crossfader"] = this.DJConsole_MP3e2.Crossfader;
                dr["PitchA"] = this.DJConsole_MP3e2.PitchA;
                dr["PitchB"] = this.DJConsole_MP3e2.PitchB;
                dr["JogWheelA"] = this.DJConsole_MP3e2.JogWheelA;
                dr["JogWheelB"] = this.DJConsole_MP3e2.JogWheelB;
                dr["PlayA"] = this.DJConsole_MP3e2.PlayA;
                dr["PlayB"] = this.DJConsole_MP3e2.PlayB;
                dr["CueA"] = this.DJConsole_MP3e2.CueA;
                dr["CueB"] = this.DJConsole_MP3e2.CueB;
                dr["TrackNextA"] = this.DJConsole_MP3e2.TrackNextA;
                dr["TrackNextB"] = this.DJConsole_MP3e2.TrackNextB;
                dr["TrackPrevA"] = this.DJConsole_MP3e2.TrackPrevA;
                dr["TrackPrevB"] = this.DJConsole_MP3e2.TrackPrevB;
                dr["PitchUpA"] = this.DJConsole_MP3e2.PitchUpA;
                dr["PitchUpB"] = this.DJConsole_MP3e2.PitchUpB;
                dr["PitchDownA"] = this.DJConsole_MP3e2.PitchDownA;
                dr["PitchDownB"] = this.DJConsole_MP3e2.PitchDownB;
                dr["OneA"] = this.DJConsole_MP3e2.OneA;
                dr["OneB"] = this.DJConsole_MP3e2.OneB;
                dr["TwoA"] = this.DJConsole_MP3e2.TwoA;
                dr["TwoB"] = this.DJConsole_MP3e2.TwoB;
                dr["ThreeA"] = this.DJConsole_MP3e2.ThreeA;
                dr["ThreeB"] = this.DJConsole_MP3e2.ThreeB;
                dr["FourA"] = this.DJConsole_MP3e2.FourA;
                dr["FourB"] = this.DJConsole_MP3e2.FourB;
                dr["ListenA"] = this.DJConsole_MP3e2.ListenA;
                dr["ListenB"] = this.DJConsole_MP3e2.ListenB;
                dr["SyncA"] = this.DJConsole_MP3e2.SyncA;
                dr["SyncB"] = this.DJConsole_MP3e2.SyncB;
                dr["Up"] = this.DJConsole_MP3e2.Up;
                dr["Down"] = this.DJConsole_MP3e2.Down;
                dr["MasterTempoA"] = this.DJConsole_MP3e2.MasterTempoA;
                dr["MasterTempoB"] = this.DJConsole_MP3e2.MasterTempoB;
                dr["LoadA"] = this.DJConsole_MP3e2.LoadA;
                dr["LoadB"] = this.DJConsole_MP3e2.LoadB;
                dr["Files"] = this.DJConsole_MP3e2.Files;
                dr["Folders"] = this.DJConsole_MP3e2.Folders;
                dr["Scratch"] = this.DJConsole_MP3e2.Scratch;
                dr["Automix"] = this.DJConsole_MP3e2.Automix;
                t.Rows.Add(dr);

                t = DJConsoleUI.Database.ds.Tables["DJConsoleMP3LE"];
                if (t.Rows.Count > 0)
                {
                    t.Rows[0].Delete();
                }
                dr = t.NewRow();
                dr["TrebleA"] = this.DJConsole_MP3LE.TrebleA;
                dr["TrebleB"] = this.DJConsole_MP3LE.TrebleB;
                dr["MediumA"] = this.DJConsole_MP3LE.MediumA;
                dr["MediumB"] = this.DJConsole_MP3LE.MediumB;
                dr["BassA"] = this.DJConsole_MP3LE.BassA;
                dr["BassB"] = this.DJConsole_MP3LE.BassB;
                dr["VolumeA"] = this.DJConsole_MP3LE.VolumeA;
                dr["VolumeB"] = this.DJConsole_MP3LE.VolumeB;
                dr["Crossfader"] = this.DJConsole_MP3LE.Crossfader;
                dr["PitchA"] = this.DJConsole_MP3LE.PitchA;
                dr["PitchB"] = this.DJConsole_MP3LE.PitchB;
                dr["JogWheelA"] = this.DJConsole_MP3LE.JogWheelA;
                dr["JogWheelB"] = this.DJConsole_MP3LE.JogWheelB;
                dr["PlayA"] = this.DJConsole_MP3LE.PlayA;
                dr["PlayB"] = this.DJConsole_MP3LE.PlayB;
                dr["CueA"] = this.DJConsole_MP3LE.CueA;
                dr["CueB"] = this.DJConsole_MP3LE.CueB;
                dr["TrackNextA"] = this.DJConsole_MP3LE.TrackNextA;
                dr["TrackNextB"] = this.DJConsole_MP3LE.TrackNextB;
                dr["TrackPrevA"] = this.DJConsole_MP3LE.TrackPrevA;
                dr["TrackPrevB"] = this.DJConsole_MP3LE.TrackPrevB;
                dr["PitchUpA"] = this.DJConsole_MP3LE.PitchUpA;
                dr["PitchUpB"] = this.DJConsole_MP3LE.PitchUpB;
                dr["PitchDownA"] = this.DJConsole_MP3LE.PitchDownA;
                dr["PitchDownB"] = this.DJConsole_MP3LE.PitchDownB;
                dr["OneA"] = this.DJConsole_MP3LE.OneA;
                dr["OneB"] = this.DJConsole_MP3LE.OneB;
                dr["TwoA"] = this.DJConsole_MP3LE.TwoA;
                dr["TwoB"] = this.DJConsole_MP3LE.TwoB;
                dr["ThreeA"] = this.DJConsole_MP3LE.ThreeA;
                dr["ThreeB"] = this.DJConsole_MP3LE.ThreeB;
                dr["FourA"] = this.DJConsole_MP3LE.FourA;
                dr["FourB"] = this.DJConsole_MP3LE.FourB;
                dr["ListenA"] = this.DJConsole_MP3LE.ListenA;
                dr["ListenB"] = this.DJConsole_MP3LE.ListenB;
                dr["SyncA"] = this.DJConsole_MP3LE.SyncA;
                dr["SyncB"] = this.DJConsole_MP3LE.SyncB;
                dr["Up"] = this.DJConsole_MP3LE.Up;
                dr["Down"] = this.DJConsole_MP3LE.Down;
                dr["PitchResetA"] = this.DJConsole_MP3LE.PitchResetA;
                dr["PitchResetB"] = this.DJConsole_MP3LE.PitchResetB;
                dr["LoadA"] = this.DJConsole_MP3LE.LoadA;
                dr["LoadB"] = this.DJConsole_MP3LE.LoadB;
                dr["Files"] = this.DJConsole_MP3LE.Files;
                dr["Folders"] = this.DJConsole_MP3LE.Folders;
                dr["Vinyl"] = this.DJConsole_MP3LE.Vinyl;
                dr["Magic"] = this.DJConsole_MP3LE.Magic;
                t.Rows.Add(dr);

                t = DJConsoleUI.Database.ds.Tables["DJConsoleRmx"];
                if (t.Rows.Count > 0)
                {
                    t.Rows[0].Delete();
                }
                dr = t.NewRow();
                dr["TrebleA"] = this.DJConsole_Rmx.TrebleA;
                dr["TrebleB"] = this.DJConsole_Rmx.TrebleB;
                dr["MediumA"] = this.DJConsole_Rmx.MediumA;
                dr["MediumB"] = this.DJConsole_Rmx.MediumB;
                dr["BassA"] = this.DJConsole_Rmx.BassA;
                dr["BassB"] = this.DJConsole_Rmx.BassB;
                dr["VolumeA"] = this.DJConsole_Rmx.VolumeA;
                dr["VolumeB"] = this.DJConsole_Rmx.VolumeB;
                dr["Crossfader"] = this.DJConsole_Rmx.Crossfader;
                dr["PitchA"] = this.DJConsole_Rmx.PitchA;
                dr["PitchB"] = this.DJConsole_Rmx.PitchB;
                dr["JogWheelA"] = this.DJConsole_Rmx.JogWheelA;
                dr["JogWheelB"] = this.DJConsole_Rmx.JogWheelB;
                dr["PlayA"] = this.DJConsole_Rmx.PlayA;
                dr["PlayB"] = this.DJConsole_Rmx.PlayB;
                dr["CueA"] = this.DJConsole_Rmx.CueA;
                dr["CueB"] = this.DJConsole_Rmx.CueB;
                dr["TrackNextA"] = this.DJConsole_Rmx.TrackNextA;
                dr["TrackNextB"] = this.DJConsole_Rmx.TrackNextB;
                dr["TrackPrevA"] = this.DJConsole_Rmx.TrackPrevA;
                dr["TrackPrevB"] = this.DJConsole_Rmx.TrackPrevB;
                dr["BeatLockA"] = this.DJConsole_Rmx.BeatLockA;
                dr["BeatLockB"] = this.DJConsole_Rmx.BeatLockB;
                dr["SourceA"] = this.DJConsole_Rmx.SourceA;
                dr["SourceB"] = this.DJConsole_Rmx.SourceB;
                dr["OneA"] = this.DJConsole_Rmx.OneA;
                dr["OneB"] = this.DJConsole_Rmx.OneB;
                dr["TwoA"] = this.DJConsole_Rmx.TwoA;
                dr["TwoB"] = this.DJConsole_Rmx.TwoB;
                dr["ThreeA"] = this.DJConsole_Rmx.ThreeA;
                dr["ThreeB"] = this.DJConsole_Rmx.ThreeB;
                dr["FourA"] = this.DJConsole_Rmx.FourA;
                dr["FourB"] = this.DJConsole_Rmx.FourB;
                dr["SelectA"] = this.DJConsole_Rmx.SelectA;
                dr["SelectB"] = this.DJConsole_Rmx.SelectB;
                dr["SyncA"] = this.DJConsole_Rmx.SyncA;
                dr["SyncB"] = this.DJConsole_Rmx.SyncB;
                dr["Up"] = this.DJConsole_Rmx.Up;
                dr["Down"] = this.DJConsole_Rmx.Down;
                dr["PitchResetA"] = this.DJConsole_Rmx.PitchResetA;
                dr["PitchResetB"] = this.DJConsole_Rmx.PitchResetB;
                dr["LoadA"] = this.DJConsole_Rmx.LoadA;
                dr["LoadB"] = this.DJConsole_Rmx.LoadB;
                dr["SixB"] = this.DJConsole_Rmx.SixB;
                dr["FiveB"] = this.DJConsole_Rmx.FiveB;
                dr["FiveA"] = this.DJConsole_Rmx.FiveA;
                dr["SixA"] = this.DJConsole_Rmx.SixA;
                dr["TrebleKillA"] = this.DJConsole_Rmx.TrebleKillA;
                dr["MediumKillA"] = this.DJConsole_Rmx.MediumKillA;
                dr["BassKillA"] = this.DJConsole_Rmx.BassKillA;
                dr["TrebleKillB"] = this.DJConsole_Rmx.TrebleKillB;
                dr["MediumKillB"] = this.DJConsole_Rmx.MediumKillB;
                dr["BassKillB"] = this.DJConsole_Rmx.BassKillB;
                dr["Scratch"] = this.DJConsole_Rmx.Scratch;
                dr["Left"] = this.DJConsole_Rmx.Left;
                dr["Right"] = this.DJConsole_Rmx.Right;
                dr["StopA"] = this.DJConsole_Rmx.StopA;
                dr["StopB"] = this.DJConsole_Rmx.StopB;
                dr["Balance"] = this.DJConsole_Rmx.Balance;
                dr["GainA"] = this.DJConsole_Rmx.GainA;
                dr["GainB"] = this.DJConsole_Rmx.GainB;
                dr["MonSelect"] = this.DJConsole_Rmx.MonSelect;
                dr["VolMain"] = this.DJConsole_Rmx.VolMain;
                dr["MikeToggle"] = this.DJConsole_Rmx.MikeToggle;
                t.Rows.Add(dr);


                t = DJConsoleUI.Database.ds.Tables["Configuration"];
                if (t.Rows.Count > 0)
                {
                    t.Rows[0].Delete();
                }
                dr = t.NewRow();
                dr["SelectedConsole"] = this.selectedConsole;
                t.Rows.Add(dr);

                DJConsoleUI.Database.Update();

            }
            catch
            {
                MessageBox.Show("DJ Console Settings could not be written to disk", "Disc Writing Error");
                return;
            }
        }


        public void Close()
        {
            // if (inDevice != null)
            {

                // ResetDJConsole();
                // inDevice.ChannelMessageReceived -= new EventHandler<ChannelMessageEventArgs>(inDevice_ChannelMessageReceived);
                // inDevice.StopRecording();
                // inDevice.Close();
                // outDevice.Close();
                CloseMidiIn();
                CloseMidiOut();
                DJConsoleUI.Database.Exit();
            }
        }



        private void ResetDJConsole()
        {
            /*  try
              {
                  ChannelMessageBuilder builder = new ChannelMessageBuilder();
                  for (int i = 0; i <= 100; i++)
                  {
                      builder.Command = ChannelCommand.Controller;
                      builder.MidiChannel = 0;
                      builder.Data1 = i;
                      builder.Data2 = 0;
                      builder.Build();
                     // outDevice.Send(builder.Result);
                  }
              }
              catch
              {
                  return;
              } */
        }

        private void LoadDictionaries()
        {
            //Buttons - 0...99
            DictButtons.Add(0, " ");
            DictButtons.Add(1, "A > B");
            DictButtons.Add(2, "A < B");
            DictButtons.Add(3, "A <> B");
            DictButtons.Add(4, "Split");
            DictButtons.Add(5, "0 Beat");
            DictButtons.Add(6, "RIT");
            DictButtons.Add(7, "XIT");
            DictButtons.Add(8, "Clear RIT");
            DictButtons.Add(9, "Clear XIT");
            DictButtons.Add(10, "MultiRx");
            DictButtons.Add(11, "VFO Sync");
            DictButtons.Add(12, "VFO Lock");
            DictButtons.Add(13, "MOX");
            DictButtons.Add(14, "VOX");
            DictButtons.Add(15, "Mute");
            //DSP
            DictButtons.Add(16, "NB1");
            DictButtons.Add(17, "NB2");
            DictButtons.Add(18, "ANF");
            DictButtons.Add(19, "NR");
            DictButtons.Add(20, "NR2");
            DictButtons.Add(21, "BIN");
            DictButtons.Add(22, "Wider Filter");
            DictButtons.Add(23, "Narrower Filter");
            DictButtons.Add(24, "Next Mode");
            DictButtons.Add(25, "Prev Mode");
            DictButtons.Add(26, "Tuning Step Up");
            DictButtons.Add(27, "Tuning Step Down");
            DictButtons.Add(28, "Band Up");
            DictButtons.Add(29, "Band Down");
            DictButtons.Add(30, "Start");
            // DictButtons.Add(31, "Tuner On/Off");
            //Phone 
            DictButtons.Add(32, "COMP");
            //  DictButtons.Add(33, "DX");
            DictButtons.Add(34, "DEXP");
            //2. RX
            DictButtons.Add(35, "RX2 On/Off");
            DictButtons.Add(36, "RX2 Preamp");
            DictButtons.Add(37, "RX2 NB1");
            DictButtons.Add(38, "RX2 NB2");
            DictButtons.Add(39, "RX2 Band Up");
            DictButtons.Add(40, "RX2 Band Down");
            //
            DictButtons.Add(41, "Enable Rx EQ");
            DictButtons.Add(42, "Enable Tx EQ");
            DictButtons.Add(43, "Squelch");
            DictButtons.Add(44, "SNB");
            DictButtons.Add(45, "AGC Mode Up");
            DictButtons.Add(46, "AGC Mode Down");
            DictButtons.Add(47, "RX2 SNB");
            //display
            DictButtons.Add(48, "AVG");
            DictButtons.Add(49, "Peak");
            DictButtons.Add(50, "Show TX Filter");
            DictButtons.Add(51, "Display Mode Next");
            DictButtons.Add(52, "Display Mode Prev");
            DictButtons.Add(53, "Zoom Step Up");
            DictButtons.Add(54, "Zoom Step Down");
            DictButtons.Add(55, "Quick Mode Save");
            DictButtons.Add(56, "Quick Mode Restore");
            //cw
            DictButtons.Add(57, "CWX Macro 1");
            DictButtons.Add(58, "CWX Macro 2");
            DictButtons.Add(59, "CWX Macro 3");
            DictButtons.Add(60, "CWX Macro 4");
            DictButtons.Add(61, "CWX Macro 5");
            DictButtons.Add(62, "CWX Macro 6");
            DictButtons.Add(63, "CWX Macro 7");
            DictButtons.Add(64, "CWX Macro 8");
            DictButtons.Add(65, "CWX Macro 9");
            DictButtons.Add(66, "Stop CWX Sending");
            DictButtons.Add(67, "MON");
            DictButtons.Add(68, "Pan Center");

            //VAC
            DictButtons.Add(69, "VAC On/Off"); //ZZVA
            DictButtons.Add(70, "I/Q to VAC1"); // ZZVH
            DictButtons.Add(71, "I/Q to VAC1 use RX2"); //ZZVJ
            DictButtons.Add(72, "VAC2 On/Off"); //ZZVK

            DictButtons.Add(73, "CTUN"); //ZZCN
            //DictButtons.Add(73, "ESC On/Off"); //ZZDE
            // DictButtons.Add(74, "ESC Form Open/Close"); //ZZDF
            DictButtons.Add(75, "Mute RX2");
            DictButtons.Add(76, "TUN");
            // DictButtons.Add(77, "Tuner Bypass");

            //Bands
            DictButtons.Add(78, "160m");
            DictButtons.Add(79, "80m");
            DictButtons.Add(80, "60m");
            DictButtons.Add(81, "40m");
            DictButtons.Add(82, "30m");
            DictButtons.Add(83, "20m");
            DictButtons.Add(84, "17m");
            DictButtons.Add(85, "15m");
            DictButtons.Add(86, "12m");
            DictButtons.Add(87, "10m");
            DictButtons.Add(88, "6m");
            //  DictButtons.Add(89, "2m");
            DictButtons.Add(90, "160m RX2");
            DictButtons.Add(91, "80m RX2");
            DictButtons.Add(92, "60m RX2");
            DictButtons.Add(93, "40m RX2");
            DictButtons.Add(94, "30m RX2");
            DictButtons.Add(95, "20m RX2");
            DictButtons.Add(96, "17m RX2");
            DictButtons.Add(97, "15m RX2");
            DictButtons.Add(98, "12m RX2");
            DictButtons.Add(99, "10m RX2");
            DictButtons.Add(500, "6m RX2");
            // DictButtons.Add(501, "2m RX2");
            DictButtons.Add(502, "Mode SSB");
            DictButtons.Add(503, "Mode LSB");
            DictButtons.Add(504, "Mode USB");
            DictButtons.Add(505, "Mode DSB");
            //     DictButtons.Add(506, "Mode CW");
            DictButtons.Add(507, "Mode CWL");
            DictButtons.Add(508, "Mode CWU");
            DictButtons.Add(509, "Mode FM");
            DictButtons.Add(510, "Mode AM");
            DictButtons.Add(511, "Mode DIGU");
            DictButtons.Add(512, "Mode SPEC");
            DictButtons.Add(513, "Mode DIGL");
            DictButtons.Add(514, "Mode SAM");
            DictButtons.Add(515, "Mode DRM");

            DictButtons.Add(520, "VFOA khz Step");
            DictButtons.Add(521, "VFOA mhz Step");



            //Incremental Knobs 100...199
            DictIncKnobs.Add(100, " ");
            DictIncKnobs.Add(101, "Freq VfoA");
            DictIncKnobs.Add(102, "Freq VfoB");
            DictIncKnobs.Add(103, "Filter Bandwidth");
            DictIncKnobs.Add(104, "RIT");
            DictIncKnobs.Add(105, "XIT");
            DictIncKnobs.Add(106, "Zoom Slider");
            DictIncKnobs.Add(107, "Filter Upper Edge");
            DictIncKnobs.Add(108, "Filter Lower Edge");
            // DictIncKnobs.Add(109, "Pan Slider");
            DictIncKnobs.Add(110, "Freq VfoA Accelerating");

            //Fixed Value Range Knobs 200...299
            DictFixKnobs.Add(200, " ");
            DictFixKnobs.Add(201, "RIT");
            DictFixKnobs.Add(202, "XIT");
            DictFixKnobs.Add(203, "SHIFT");
            DictFixKnobs.Add(204, "Volume MainRx");
            DictFixKnobs.Add(205, "Volume SubRx");
            DictFixKnobs.Add(206, "Ratio MainRx/SubRx");
            DictFixKnobs.Add(207, "PreAmp Settings");
            DictFixKnobs.Add(208, "CW Speed");
            DictFixKnobs.Add(209, "AF Gain");
            DictFixKnobs.Add(210, "AGC Threshold");
            DictFixKnobs.Add(211, "Drive Level");
            DictFixKnobs.Add(212, "Mic Gain");
            // DictFixKnobs.Add(213, "DX");
            DictFixKnobs.Add(214, "COMP Threshold");
            DictFixKnobs.Add(215, "Vox Gain");
            DictFixKnobs.Add(216, "DEXP Threshold");
            DictFixKnobs.Add(217, "Squelch Threshold");
            DictFixKnobs.Add(218, "AGC Threshold RX2");
            DictFixKnobs.Add(219, "TX AF Monitor");
            DictFixKnobs.Add(220, "AGC Mode");
            //Display 
            DictFixKnobs.Add(221, "Zoom Slider");
            DictFixKnobs.Add(222, "Volume RX2");
            DictFixKnobs.Add(223, "Pan Slider");
            DictFixKnobs.Add(224, "VAC RX Gain");
            DictFixKnobs.Add(225, "VAC TX Gain");
            DictFixKnobs.Add(226, "VAC2 RX Gain");
            DictFixKnobs.Add(227, "VAC2 TX Gain");
            DictFixKnobs.Add(228, "Waterfall / Grid Low Limit");
            DictFixKnobs.Add(229, "Waterfall / Grid High Limit");
            DictFixKnobs.Add(230, "RX2 Stereo Balance (PAN)");


            //Add supported consoles to Dictionary
            SupportedConsoles.Add(0, "No Supported Console found");
            SupportedConsoles.Add(1, "Hercules DJ Console Mk2 MIDI");
            SupportedConsoles.Add(2, "DJ Control MP3 e2 MIDI");
            SupportedConsoles.Add(3, "DJControl MP3 LE MIDI");
            SupportedConsoles.Add(4, "Hercules DJ Console Rmx MIDI");

        }

        //private void DetectConnectedConsoles()
        //{
        //    if (Sanford.Multimedia.Midi.InputDevice.DeviceCount > 0)
        //    {
        //        ConnectedConsoles = null;
        //        ConnectedConsoles = new Dictionary<int, string>();
        //        for (int i = 0; i < Sanford.Multimedia.Midi.InputDevice.DeviceCount; i++)
        //        {
        //            for (int j = 0; j < SupportedConsoles.Count; j++)
        //            {
        //                string dev = Sanford.Multimedia.Midi.InputDevice.GetDeviceCapabilities(i).name;
        //                //if (dev.Contains("-"))
        //                //{
        //                //    dev = dev.Substring(3);
        //                //}
        //                if (dev == SupportedConsoles[j].ToString())
        //                {
        //                    ConnectedConsoles.Add(j, SupportedConsoles[j].ToString());
        //                }
        //            }
        //        }
        //    }
        //}

        private void DetectConnectedConsoles()
        {
            if (DeviceInCount > 0)
            {
                ConnectedConsoles = null;
                ConnectedConsoles = new Dictionary<int, string>();
                for (int i = 0; i < DeviceInCount; i++)
                {
                    for (int j = 0; j < SupportedConsoles.Count; j++)
                    {
                        // string dev = InputDevice.GetDeviceCapabilities(i).name;
                        string dev = MidiInGetName(i);
                        string[] temp = dev.Split(new char[] { '-' });
                        if (temp.Length > 1)
                        {
                            dev = temp[1];
                            dev = dev.Trim();
                        }


                        if (dev == SupportedConsoles[j].ToString())
                        {
                            ConnectedConsoles.Add(j, SupportedConsoles[j].ToString());
                        }
                    }
                }
            }
        }


        //private void inDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        public void inDevice_ChannelMessageReceived(int Data1, int Data2)
        {
            if (this.SelectedConsole == (int)DJConsoleModels.HerculesMK2)
            {

                //switch (e.Message.Data1)
                switch (Data1)
                {
                    case 1: // SelectFX DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.SelectFxB;
                            {
                                execute(cmd, Data2);
                            }
                            break;
                        }

                    case 2: // Play DeckB 
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.PlayB;
                            {
                                execute(cmd, Data2);
                            }
                            break;
                        }

                    case 3: // Cue DeckB 
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.CueB;
                            {
                                execute(cmd, Data2);
                            }
                            break;
                        }


                    case 4: // AutoBeat DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.AutobeatB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 5: // TrackPrev DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.TrackPrevB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 6: // TrackNext DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.TrackNextB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 7: // SelectFX DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.SelectFxA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 8: // Play DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.PlayA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 9: // Cue DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.CueA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 10: // Autobeat DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.AutobeatA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 11: // TrackPrev DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.TrackPrevA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 12: // TrackNext DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.TrackNextA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 13: // LoopFX DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.LoopFxA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 14: // CueFx DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.CueFXA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 15: // FX DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.FXA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 16: // FX DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.FXB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 17: // CueFx DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.CueFXB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 18: // LoopFX DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.LoopFxB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 19: // PitchUp DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.PitchUpA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 20: // PitchDown DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.PitchDownA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 23: // PitchUp DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.PitchUpB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 24: // PitchDown DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.PitchDownB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 43: //Bass DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.BassB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 44: //Medium DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.MediumB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 45: //Treble DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.TrebleB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 46: //Bass DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.BassA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 47: //Medium DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.MediumA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 48: //Treble DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.TrebleA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 49: //Crossfader
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.Crossfader;
                            execute(cmd, Data2);
                            break;
                        }

                    case 50: //Volume DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.VolumeA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 51: //Volume DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.VolumeB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 52: //Pitch DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.PitchA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 53: //Pitch DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.PitchB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 54: //JogWheel DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.JogWheelA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 55: //JogWheel DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MK2.JogWheelB;
                            execute(cmd, Data2);
                            break;
                        }


                }
            }

            if (this.SelectedConsole == (int)DJConsoleModels.HerculesMP3LE)  //Hercules DJ Console MP3LE
            {
                //Debugging: //MessageBox.Show("Data1: " + e.Message.Data1.ToString() + " Data2: " + e.Message.Data2.ToString());
                switch (Data1)
                {
                    case 1: // Key 1 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.OneA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 2: // Key 2 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TwoA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 3: // Key 3 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.ThreeA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 4: // Key 4 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.FourA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 5: // Key 1 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.OneA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 6: // Key 2 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TwoA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 7: // Key 3 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.ThreeA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 8: // Key 4 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.FourA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 9: // Shift DeckA (deselected)
                        {
                            //int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.FourB;
                            //execute(cmd, Data2);
                            break;
                        }

                    case 20: // Shift DeckA (selected)
                        {
                            //int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.FourB;
                            //execute(cmd, Data2);
                            break;
                        }

                    case 14: // CUE DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.CueA;
                            //Debugging: MessageBox.Show("MP3LE CueA: "+cmd.ToString());
                            execute(cmd, Data2);
                            break;
                        }

                    case 16: // Listen DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.ListenA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 17: // Load DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.LoadA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 18: // Sync DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.SyncA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 19: // Pitch Reset DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchResetA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 21: // Key 1 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.OneB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 22: // Key 2 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TwoB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 23: // Key 3 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.ThreeB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 24: // Key 4 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.FourB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 25: // Key 1 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.OneB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 26: // Key 2 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TwoB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 27: // Key 3 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.ThreeB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 28: // Key 4 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.FourB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 29: // Shift DeckB (deselected)
                        {
                            //int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.ShiftB;
                            //execute(cmd, Data2);
                            break;
                        }

                    case 36: // Listen DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.ListenB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 37: // Load DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.LoadB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 38: // Sync DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.SyncB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 39: // PitchReset DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchResetB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 40: // Shift DeckB (selected)
                        {
                            break;
                        }

                    case 41: // Up
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.Up;
                            execute(cmd, Data2);
                            break;
                        }

                    case 42: // Down
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.Down;
                            execute(cmd, Data2);
                            break;
                        }

                    case 35: // Play DeckB 
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PlayB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 34: // Cue DeckB 
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.CueB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 32: // TrackPrev DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TrackPrevB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 33: // TrackNext DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TrackNextB;
                            execute(cmd, Data2);
                            break;
                        }


                    case 15: // Play DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PlayA;
                            execute(cmd, Data2);
                            break;
                        }


                    case 12: // TrackPrev DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TrackPrevA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 13: // TrackNext DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TrackNextA;
                            execute(cmd, Data2);
                            break;
                        }


                    case 11: // PitchUp DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchUpA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 10: // PitchDown DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchDownA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 31: // PitchUp DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchUpB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 30: // PitchDown DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchDownB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 60: //Bass DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.BassB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 59: //Medium DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.MediumB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 58: //Treble DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TrebleB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 55: //Bass DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.BassA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 54: //Medium DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.MediumA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 53: //Treble DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.TrebleA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 56: //Crossfader
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.Crossfader;
                            execute(cmd, Data2);
                            break;
                        }

                    case 52: //Volume DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.VolumeA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 57: //Volume DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.VolumeB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 50: //Pitch DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 51: //Pitch DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.PitchB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 48: //JogWheel DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.JogWheelA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 49: //JogWheel DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.JogWheelB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 43: // Files
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.Files;
                            execute(cmd, Data2);
                            break;
                        }

                    case 44: // Folders
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.Folders;
                            execute(cmd, Data2);
                            break;
                        }

                    case 45: // Vinyl
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.Vinyl;
                            execute(cmd, Data2);
                            break;
                        }

                    case 46: // Magic
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3LE.Magic;
                            execute(cmd, Data2);
                            break;
                        }

                }

            }



            if (m_parent.DJConsoleObj.SelectedConsole == (int)DJConsoleModels.HerculesMP3e2)
            {

                switch (Data1)
                {
                    case 1: // Key 1 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.OneA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 2: // Key 2 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TwoA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 3: // Key 3 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.ThreeA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 4: // Key 4 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.FourA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 5: // Key 1 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.OneA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 6: // Key 2 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TwoA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 7: // Key 3 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.ThreeA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 8: // Key 4 DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.FourA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 9: // Shift DeckA (deselected)
                        {
                            //int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.FourB;
                            //execute(cmd, Data2);
                            break;
                        }

                    case 20: // Shift DeckA (selected)
                        {
                            //int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.FourB;
                            //execute(cmd, Data2);
                            break;
                        }

                    case 14: // CUE DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.CueA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 16: // Listen DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.ListenA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 17: // Load DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.LoadA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 18: // Sync DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.SyncA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 19: // MasterTempo DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.MasterTempoA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 21: // Key 1 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.OneB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 22: // Key 2 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TwoB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 23: // Key 3 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.ThreeB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 24: // Key 4 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.FourB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 25: // Key 1 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.OneB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 26: // Key 2 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TwoB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 27: // Key 3 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.ThreeB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 28: // Key 4 DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.FourB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 29: // Shift DeckB (deselected)
                        {
                            //int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.ShiftB;
                            //execute(cmd, Data2);
                            break;
                        }

                    case 36: // Listen DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.ListenB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 37: // Load DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.LoadB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 38: // Sync DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.SyncB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 39: // MasterTempo DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.MasterTempoB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 40: // Shift DeckB (selected)
                        {
                            break;
                        }

                    case 41: // Up
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.Up;
                            execute(cmd, Data2);
                            break;
                        }

                    case 42: // Down
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.Down;
                            execute(cmd, Data2);
                            break;
                        }

                    case 35: // Play DeckB 
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.PlayB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 34: // Cue DeckB 
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.CueB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 32: // TrackPrev DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TrackPrevB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 33: // TrackNext DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TrackNextB;
                            execute(cmd, Data2);
                            break;
                        }


                    case 15: // Play DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.PlayA;
                            execute(cmd, Data2);
                            break;
                        }


                    case 12: // TrackPrev DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TrackPrevA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 13: // TrackNext DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TrackNextA;
                            execute(cmd, Data2);
                            break;
                        }


                    case 11: // PitchUp DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchUpA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 10: // PitchDown DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchDownA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 31: // PitchUp DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchUpB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 30: // PitchDown DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchDownB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 60: //Bass DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.BassB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 59: //Medium DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.MediumB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 58: //Treble DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TrebleB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 55: //Bass DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.BassA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 54: //Medium DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.MediumA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 53: //Treble DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.TrebleA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 56: //Crossfader
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.Crossfader;
                            execute(cmd, Data2);
                            break;
                        }

                    case 52: //Volume DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.VolumeA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 57: //Volume DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.VolumeB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 50: //Pitch DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 51: //Pitch DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.PitchB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 48: //JogWheel DeckA
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.JogWheelA;
                            execute(cmd, Data2);
                            break;
                        }

                    case 49: //JogWheel DeckB
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.JogWheelB;
                            execute(cmd, Data2);
                            break;
                        }

                    case 43: // Files
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.Files;
                            execute(cmd, Data2);
                            break;
                        }

                    case 44: // Folders
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.Folders;
                            execute(cmd, Data2);
                            break;
                        }

                    case 45: // Scratch
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.Scratch;
                            execute(cmd, Data2);
                            break;
                        }

                    case 46: // Automix
                        {
                            int cmd = m_parent.DJConsoleObj.DJConsole_MP3e2.Automix;
                            execute(cmd, Data2);
                            break;
                        }

                }
            }

                //*******************************************

                if (this.SelectedConsole == (int)DJConsoleModels.HerculesRmx)  //Hercules DJ Console RMX
                {
                    //Debugging: //MessageBox.Show("Data1: " + e.Message.Data1.ToString() + " Data2: " + e.Message.Data2.ToString());
                    switch (Data1)
                    {
                        case 0x01: // Key 1 DeckA
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.OneA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x02: // Key 2 DeckA
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TwoA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x03: // Key 3 DeckA
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.ThreeA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x04: // Key 4 DeckA
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.FourA;
                                execute(cmd, Data2);
                                break;
                            }
                        case 0x05: // Key 5 DeckA
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.FiveA;
                                execute(cmd, Data2);
                                break;
                            }
                        case 0x06: // Key 6 DeckA
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.SixA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x07: // Left Sync
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.SyncA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x08: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.BeatLockA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x09: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TrackPrevA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x0A: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TrackNextA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x0b: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.PlayA;
                                execute(cmd, Data2);
                                break;
                            }
                        
                        case 0x0c: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.CueA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x0d: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.StopA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x0e:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TrebleKillA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x0f: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.MediumKillA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x10: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.BassKillA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x11: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.PitchResetA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x12:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.LoadA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x13: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.SourceA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x14:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.SelectA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x15:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.BeatLockB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x16: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.LoadB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x17:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.SourceB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x18:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.SelectB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x19:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.OneB;
                                execute(cmd, Data2);
                                break;
                            }
                        case 0x1A:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TwoB;
                                execute(cmd, Data2);
                                break;
                            }
                        case 0x1B:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.ThreeB;
                                execute(cmd, Data2);
                                break;
                            }
                        case 0x1C:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.FourB;
                                execute(cmd, Data2);
                                break;
                            }
                        case 0x1D:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.FiveB;
                                execute(cmd, Data2);
                                break;
                            }
                        case 0x1E:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.SixB;
                                execute(cmd, Data2);
                                break;
                            }
                            
                        case 0x1f: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.SyncB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x20:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.PitchResetB;
                                execute(cmd, Data2);
                                break;
                            }
                            
                        case 0x21:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TrackPrevB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x22: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TrackNextB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x23:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.PlayB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x24: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.CueA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x25: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.StopB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x26:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TrebleKillB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x27:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.MediumKillB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x28:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.BassKillB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x29: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.Scratch;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x2a: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.Up;
                                execute(cmd, Data2);
                                break;
                            }


                        case 0x2b:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.Down;
                                execute(cmd, Data2);
                                break;
                            }


                        case 0x2c: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.Left;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x2d: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.Right;
                                execute(cmd, Data2);
                                break;
                            }
                            
                        case 0x2e: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.MikeToggle;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x2f: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.JogWheelA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x30: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.JogWheelB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x31:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.PitchA;
                                execute(cmd, Data2);
                                break;
                            }
                            
                        case 0x32: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.VolumeA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x33: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.GainA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x34:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TrebleA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x35:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.MediumA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x36:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.BassA;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x37: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.Balance;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x38: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.VolMain;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x39:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.Crossfader;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x3a: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.MonSelect;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x3b:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.PitchB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x3c:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.VolumeB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x3d: 
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.GainB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x3e:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.TrebleB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x3f:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.MediumB;
                                execute(cmd, Data2);
                                break;
                            }

                        case 0x40:
                            {
                                int cmd = m_parent.DJConsoleObj.DJConsole_Rmx.BassB;
                                execute(cmd, Data2);
                                break;
                            }

                        default:
                            {
                                System.Diagnostics.Debug.WriteLine("RMX unhandled MIDI Cmd "+Data1.ToString("H"));
                                break;
                            }
                    }
                }
                //*****************

        }

        private void execute(int cmd, int msg)
        {
            switch (cmd)
            {

                //Buttons

                case 1: //"VfoA->VfoB":
                    {
                        VfoAtoB(msg);
                        break;
                    }


                case 2: //"VfoA<-VfoB":
                    {
                        VfoBtoA(msg);
                        break;
                    }

                case 3:
                    {
                        VfoSwap(msg);
                        break;
                    }
                case 4:
                    {
                        SplitOnOff(msg);
                        break;
                    }
                case 5:
                    {
                        ZeroBeatPress(msg);
                        break;
                    }
                case 6:
                    {
                        RitOnOff(msg);
                        break;
                    }

                case 7:
                    {
                        XitOnOff(msg);
                        break;
                    }

                case 8:
                    {
                        RIT_clear(msg);
                        break;
                    }

                case 9:
                    {
                        XIT_clear(msg);
                        break;
                    }

                case 10:
                    {
                        MultiRxOnOff(msg);
                        break;
                    }
                case 11:
                    {
                        VfoSyncOnOff(msg);
                        break;
                    }
                case 12:
                    {
                        LockVFO(msg);
                        break;
                    }

                case 13:
                    {
                        MOXOnOff(msg);
                        break;
                    }

                case 14:
                    {
                        VOXOnOff(msg);
                        break;
                    }

                case 15:
                    {
                        MuteOnOff(msg);
                        break;
                    }

                case 16:
                    {
                        Rx1NoiseBlanker1OnOff(msg);
                        break;
                    }

                case 17:
                    {
                        Rx1Noiseblanker2OnOff(msg);
                        break;
                    }

                case 18:
                    {
                        AutoNotchOnOff(msg);
                        break;
                    }

                case 19:
                    {
                        NoiseReductionOnOff(msg);
                        break;
                    }

                case 20:
                    {
                        Rx2NoiseReductionOnOff(msg);
                        //SpurReductionOnOff(msg);
                        break;
                    }

                case 21:
                    {
                        BinauralOnOff(msg);
                        break;
                    }

                case 22:
                    {
                        Rx1FilterWider(msg);
                        break;
                    }

                case 23:
                    {
                        Rx1FilterNarrower(msg);
                        break;
                    }

                case 24:
                    {
                        Rx1ModeNext(msg);
                        break;
                    }

                case 25:
                    {
                        Rx1ModePrev(msg);
                        break;
                    }

                case 26:
                    {
                        TuningStepUp(msg);
                        break;
                    }

                case 27:
                    {
                        TuningStepDown(msg);
                        break;
                    }

                case 28:
                    {
                        BandUp(msg);
                        break;
                    }

                case 29:
                    {
                        BandDown(msg);
                        break;
                    }

                case 30:
                    {
                        StartOnOff(msg);
                        break;
                    }

                case 31:
                    {
                        TunerOnOff(msg);
                        break;
                    }

                case 32:
                    {
                        CompanderOnOff(msg);
                        break;
                    }

                case 33:
                    {
                        PhoneDXOnOff(msg);
                        break;
                    }

                case 34:
                    {
                        DEXPOnOff(msg);
                        break;
                    }

                case 35:
                    {
                        RX2OnOff(msg);
                        break;
                    }

                case 36:
                    {
                        Rx2PreAmpOnOff(msg);
                        break;
                    }

                case 37:
                    {
                        Rx2NoiseBlanker1OnOff(msg);
                        break;
                    }

                case 38:
                    {
                        Rx2Noiseblanker2OnOff(msg);
                        break;
                    }

                case 39:
                    {
                        Rx2BandUp(msg);
                        break;
                    }
                case 40:
                    {
                        Rx2BandDown(msg);
                        break;
                    }

                case 41:
                    {
                        RXEQOnOff(msg);
                        break;
                    }
                case 42:
                    {
                        TXEQOnOff(msg);
                        break;
                    }
                case 43:
                    {
                        SquelchOnOff(msg);
                        break;
                    }

                case 44:
                    {
                        SpectralNoiseBlankerOnOff(msg);
                        break;
                    }

                case 45:
                    {
                        AGCModeUp(msg);
                        break;
                    }

                case 46:
                    {
                        AGCModeDown(msg);
                        break;
                    }

                case 47:
                    {
                        SpectralNoiseBlankerRx2OnOff(msg);
                        break;
                    }

                case 48:
                    {
                        DisplayAverage(msg);
                        break;
                    }
                case 49:
                    {
                        DisplayPeak(msg);
                        break;
                    }
                case 50:
                    {
                        DisplayTxFilter(msg);
                        break;
                    }
                case 51:
                    {
                        DisplayModeNext(msg);
                        break;
                    }
                case 52:
                    {
                        DisplayModePrev(msg);
                        break;
                    }
                case 53:
                    {
                        ZoomInc(msg);
                        break;
                    }
                case 54:
                    {
                        ZoomDec(msg);
                        break;
                    }
                case 55:
                    {
                        QuickModeSave(msg);
                        break;
                    }
                case 56:
                    {
                        QuickModeRestore(msg);
                        break;
                    }

                case 57:
                    {
                        CWXMacro1(msg);
                        break;
                    }
                case 58:
                    {
                        CWXMacro2(msg);
                        break;
                    }

                case 59:
                    {
                        CWXMacro3(msg);
                        break;
                    }
                case 60:
                    {
                        CWXMacro4(msg);
                        break;
                    }
                case 61:
                    {
                        CWXMacro5(msg);
                        break;
                    }
                case 62:
                    {
                        CWXMacro6(msg);
                        break;
                    }
                case 63:
                    {
                        CWXMacro7(msg);
                        break;
                    }
                case 64:
                    {
                        CWXMacro8(msg);
                        break;
                    }
                case 65:
                    {
                        CWXMacro9(msg);
                        break;
                    }
                case 66:
                    {
                        CWXStop(msg);
                        break;
                    }
                case 67:
                    {
                        MONOnOff(msg);
                        break;
                    }
                case 68:
                    {
                        PanCenter(msg);
                        break;
                    }
                case 69:
                    {
                        VACOnOff(msg);
                        break;
                    }
                case 70:
                    {
                        IQtoVAC(msg);
                        break;
                    }
                case 71:
                    {
                        IQtoVACRX2(msg);
                        break;
                    }
                case 72:
                    {
                        VAC2OnOff(msg);
                        break;
                    }

                case 73:
                    {
                        CTun(msg);
                        //ESCOnOff(msg);
                        break;
                    }
                case 74:
                    {
                        ESCFormOnOff(msg);
                        break;
                    }

                case 75:
                    {
                        MuteRX2OnOff(msg);
                        break;
                    }

                case 76:
                    {
                        Tun(msg);
                        break;
                    }

                case 77:
                    {
                        TunerBypass(msg);
                        break;
                    }

                case 78:
                    {
                        Band160m(msg);
                        break;
                    }
                case 79:
                    {
                        Band80m(msg);
                        break;
                    }
                case 80:
                    {
                        Band60m(msg);
                        break;
                    }
                case 81:
                    {
                        Band40m(msg);
                        break;
                    }
                case 82:
                    {
                        Band30m(msg);
                        break;
                    }
                case 83:
                    {
                        Band20m(msg);
                        break;
                    }
                case 84:
                    {
                        Band17m(msg);
                        break;
                    }
                case 85:
                    {
                        Band15m(msg);
                        break;
                    }
                case 86:
                    {
                        Band12m(msg);
                        break;
                    }
                case 87:
                    {
                        Band10m(msg);
                        break;
                    }
                case 88:
                    {
                        Band6m(msg);
                        break;
                    }
                case 89:
                    {
                        Band2m(msg);
                        break;
                    }
                case 90:
                    {
                        Band160mRX2(msg);
                        break;
                    }
                case 91:
                    {
                        Band80mRX2(msg);
                        break;
                    }
                case 92:
                    {
                        Band60mRX2(msg);
                        break;
                    }
                case 93:
                    {
                        Band40mRX2(msg);
                        break;
                    }
                case 94:
                    {
                        Band30mRX2(msg);
                        break;
                    }
                case 95:
                    {
                        Band20mRX2(msg);
                        break;
                    }
                case 96:
                    {
                        Band17mRX2(msg);
                        break;
                    }
                case 97:
                    {
                        Band15mRX2(msg);
                        break;
                    }
                case 98:
                    {
                        Band12mRX2(msg);
                        break;
                    }
                case 99:
                    {
                        Band10mRX2(msg);
                        break;
                    }
                case 500:
                    {
                        Band6mRX2(msg);
                        break;
                    }
                case 501:
                    {
                        Band2mRX2(msg);
                        break;
                    }
                case 502:
                    {
                        ModeSSB(msg);
                        break;
                    }
                case 503:
                    {
                        ModeLSB(msg);
                        break;
                    }
                case 504:
                    {
                        ModeUSB(msg);
                        break;
                    }
                case 505:
                    {
                        ModeDSB(msg);
                        break;
                    }
                case 506:
                    {
                        ModeCW(msg);
                        break;
                    }
                case 507:
                    {
                        ModeCWL(msg);
                        break;
                    }
                case 508:
                    {
                        ModeCWU(msg);
                        break;
                    }
                case 509:
                    {
                        ModeFM(msg);
                        break;
                    }
                case 510:
                    {
                        ModeAM(msg);
                        break;
                    }
                case 511:
                    {
                        ModeDIGU(msg);
                        break;
                    }
                case 512:
                    {
                        ModeSPEC(msg);
                        break;
                    }
                case 513:
                    {
                        ModeDIGL(msg);
                        break;
                    }
                case 514:
                    {
                        ModeSAM(msg);
                        break;
                    }
                case 515:
                    {
                        ModeDRM(msg);
                        break;
                    }

                case 520:
                    {
                        SetVfoAConsoleStepKhz(msg);
                        break;
                    }

                case 521:
                    {
                        SetVfoAConsoleStepMhz(msg);
                        break;
                    }



                //Incremental Knobs 100...199

                case 101: //"Freq VfoA":
                    {
                        ChangeFreqVfoA(msg);
                        break;
                    }

                case 102: //"Freq VfoB":
                    {
                        ChangeFreqVfoB(msg);
                        break;
                    }


                case 103: //"Filter Bandwidth":
                    {
                        FilterBandwidth(msg);
                        break;
                    }

                case 104: //RIT
                    {
                        RIT_inc(msg);
                        break;
                    }

                case 105: //XIT
                    {
                        XIT_inc(msg);
                        break;
                    }

                case 106:
                    {
                        ZoomSliderInc(msg);
                        break;
                    }
                case 107:
                    {
                        FilterHigh(msg);
                        break;
                    }
                case 108:
                    {
                        FilterLow(msg);
                        break;
                    }
                case 109:
                    {
                        PanSlider(msg);
                        break;
                    }
                case 110:
                    {
                        MultiStepVfoA(msg);
                        break;
                    }


                //Fixed Value Range Knobs 200...299

                case 201: //"RIT"
                    {
                        RIT(msg);
                        break;
                    }

                case 202: //"XIT":
                    {
                        XIT(msg);
                        break;
                    }

                case 203: //"SHIFT":
                    {
                        FilterShift(msg);
                        break;
                    }

                case 204: //"Volume Rx1":
                    {
                        VolumeVfoA(msg);
                        break;
                    }

                case 205: //"Volume Rx2":
                    {
                        VolumeVfoB(msg);
                        break;
                    }


                case 206: //"Ratio Main/Sub Rx":
                    {
                        RatioMainSubRx(msg);
                        break;
                    }

                case 207:
                    {
                        PreAmpSettingsKnob(msg);
                        break;
                    }

                case 208:
                    {
                        CWSpeed(msg);
                        break;
                    }

                case 209: //"AF Gain":
                    {
                        SetAFGain(msg);
                        break;
                    }

                case 210:
                    {
                        AGCLevel(msg);
                        break;
                    }

                case 211:
                    {
                        DriveLevel(msg);
                        break;
                    }

                case 212:
                    {
                        MicGain(msg);
                        break;
                    }

                case 213:
                    {
                        DXLevel(msg);
                        break;
                    }

                case 214:
                    {
                        CPDRLevel(msg);
                        break;
                    }

                case 215:
                    {
                        VOXGain(msg);
                        break;
                    }

                case 216:
                    {
                        DEXPThreshold(msg);
                        break;
                    }

                case 217:
                    {
                        SquelchControl(msg);
                        break;
                    }

                case 218:
                    {
                        RX2AGCLevel(msg);
                        break;
                    }
                case 219:
                    {
                        TXAFMonitor(msg);
                        break;
                    }

                case 220:
                    {
                        AGCModeKnob(msg);
                        break;
                    }

                case 221:
                    {
                        ZoomSliderFix(msg);
                        break;
                    }

                case 222:
                    {
                        RX2Volume(msg);
                        break;
                    }

                case 223:
                    {
                        PanSlider(msg);
                        break;
                    }
                case 224:
                    {
                        VACGainRX(msg);
                        break;
                    }
                case 225:
                    {
                        VACGainTX(msg);
                        break;
                    }

                case 226:
                    {
                        VAC2GainRX(msg);
                        break;
                    }
                case 227:
                    {
                        VAC2GainTX(msg);
                        break;
                    }
                case 228:
                    {
                        WaterfallLowLimit(msg);
                        break;
                    }
                case 229:
                    {
                        WaterfallHighLimit(msg);
                        break;
                    }
                case 230:
                    {
                        RX2Pan(msg);
                        break;
                    }
            }
        }


        private void MultiRxOnOff(int msg)
        {
            parser.nSet = 1;
            parser.nGet = 0;

            if (msg == 127)
            {
                int MultiRxState = Convert.ToInt16(commands.ZZMU(""));

                if (MultiRxState == 0)
                {
                    commands.ZZMU("1");
                    return;
                }

                if (MultiRxState == 1)
                {
                    commands.ZZMU("0");
                    return;
                }
            }
        }


        private void Rx1ModeNext(int msg)
        {
            parser.nSet = 2;
            parser.nGet = 0;
            int SelectedMode = Convert.ToInt16(commands.ZZMD(""));

            if ((SelectedMode < 11) && (msg == 127))
            {
                commands.ZZMD((SelectedMode + 1).ToString("00"));
            }
        }


        private void Rx1ModePrev(int msg)
        {
            parser.nSet = 2;
            parser.nGet = 0;
            int SelectedMode = Convert.ToInt16(commands.ZZMD(""));

            if ((SelectedMode > 0) && (msg == 127))
            {
                commands.ZZMD((SelectedMode - 1).ToString("00"));
            }
        }

        private void Rx1FilterWider(int msg)
        {
            parser.nSet = 2;
            parser.nGet = 0;
            int SelectedFilter = Convert.ToInt16(commands.ZZFI(""));

            if ((SelectedFilter > 0) && (msg == 127))
            {
                commands.ZZFI((SelectedFilter - 1).ToString("00"));
            }
        }


        private void Rx1FilterNarrower(int msg)
        {
            parser.nSet = 2;
            parser.nGet = 0;
            int SelectedFilter = Convert.ToInt16(commands.ZZFI(""));

            if ((SelectedFilter < 14) && (msg == 127))
            {
                commands.ZZFI((SelectedFilter + 1).ToString("00"));
            }
        }


        private void VfoAtoB(int msg)
        {
            if (msg == 127)
            {
                parser.nSet = 11;
                parser.nGet = 0;

                string FreqA = commands.ZZFA("");
                commands.ZZFB(FreqA);
            }
        }


        private void VfoBtoA(int msg)
        {
            if (msg == 127)
            {
                parser.nSet = 11;
                parser.nGet = 0;

                string FreqB = commands.ZZFB("");
                commands.ZZFA(FreqB);
            }
        }

        private void VfoSwap(int msg)
        {
            if (msg == 127)
            {
                parser.nSet = 11;
                parser.nGet = 0;

                string FreqB = commands.ZZFB("");
                string FreqA = commands.ZZFA("");
                commands.ZZFA(FreqB);
                commands.ZZFB(FreqA);
            }
        }

        private void XIT(int msg)
        {
            parser.nSet = 5;
            parser.nGet = 0;

            if ((msg < 64) & (msg >= 0))
            {
                int XITValue = (-1280 + (msg * 20));
                commands.ZZXF(XITValue.ToString("0000"));
            }
            if ((msg >= 64) & (msg <= 127))
            {
                int XITValue = ((msg - 64) * 20);
                commands.ZZXF("+" + XITValue.ToString("0000"));
            }
            return;
        }


        private void RIT(int msg)
        {
            parser.nSet = 5;
            parser.nGet = 0;

            if ((msg < 64) & (msg >= 0))
            {
                int RITValue = (-1280 + (msg * 20));
                commands.ZZRF(RITValue.ToString("0000"));
            }
            if ((msg >= 64) & (msg <= 127))
            {
                int RITValue = ((msg - 64) * 20);
                commands.ZZRF("+" + RITValue.ToString("0000"));
            }
            return;
        }

        private void RIT_inc(int msg)
        {
            parser.nSet = 2;
            parser.nGet = 0;

            if (msg == 127)
            {
                commands.ZZRD("");
            }

            if (msg == 1)
            {
                commands.ZZRU("");
            }
        }

        private void XIT_inc(int msg)
        {
            parser.nSet = 5;
            parser.nGet = 0;
            parser.nAns = 5;
            long freq = Convert.ToInt32(commands.ZZXF(""));
            int mode = Convert.ToInt16(commands.ZZMD(""));

            if ((msg == 127) && (freq > -99995))
            {
                if ((mode == 0) || (mode == 1)) freq = freq - 50;
                if ((mode == 3) || (mode == 4)) freq = freq - 10;
                if (freq < 0) commands.ZZXF(freq.ToString("D4"));
                if (freq >= 0) commands.ZZXF("+" + freq.ToString("D4"));
            }

            if ((msg == 1) && (freq < 99995))
            {
                if ((mode == 0) || (mode == 1)) freq = freq + 50;
                if ((mode == 3) || (mode == 4)) freq = freq + 10;
                if (freq < 0) commands.ZZXF(freq.ToString("D4"));
                if (freq >= 0) commands.ZZXF("+" + freq.ToString("D4"));
            }
        }

        private void RIT_clear(int msg)
        {
            parser.nSet = 0;
            parser.nGet = 0;

            if (msg == 127)
            {
                commands.ZZRC();
            }
        }

        private void XIT_clear(int msg)
        {
            parser.nSet = 0;
            parser.nGet = 0;

            if (msg == 127)
            {
                commands.ZZXC();
            }
        }

        private void TuningStepUp(int msg)
        {
            parser.nSet = 2;
            parser.nGet = 0;

            if ((Convert.ToInt16(commands.ZZAC("")) < 22) && (msg == 127))
            {
                commands.ZZSU();
            }
        }

        private void TuningStepDown(int msg)
        {
            parser.nSet = 2;
            parser.nGet = 0;

            if ((Convert.ToInt16(commands.ZZAC("")) > 0) && (msg == 127))
            {
                commands.ZZSD();
            }
        }

        //    case 50: // Volume DeckA - Volume

        private void VolumeVfoA(int msg)
        {
            parser.nSet = 3;
            parser.nGet = 0;

            try
            {
                double vol = msg * 0.787;
                commands.ZZLA(vol.ToString("000"));
                return;
            }
            catch
            {
                return;
            }
        }

        //    case 51: // Volume DeckB - Volume

        private void VolumeVfoB(int msg)
        {

            parser.nSet = 3;
            parser.nGet = 0;

            try
            {
                double vol = msg * 0.787;
                commands.ZZLC(vol.ToString("000"));
                return;
            }
            catch
            {
                return;
            }

        }

        private void RX2Volume(int msg)
        {
            parser.nSet = 3;
            parser.nGet = 0;

            try
            {
                double RX2vol = msg * 0.787;
                commands.ZZLE(RX2vol.ToString("000"));
                return;
            }
            catch
            {
                return;
            }
        }

        private void RX2Pan(int msg)
        {
            parser.nSet = 3;
            parser.nGet = 0;

            try
            {
                double RX2vol = msg * 0.787;
                commands.ZZLF(RX2vol.ToString("000"));
                return;
            }
            catch
            {
                return;
            }
        }

        //    case 52: // Pitch DeckA - FilterBandwidth
        private void FilterBandwidth(int msg)
        {
            parser.nSet = 5;
            parser.nGet = 0;
            int FilterBW = Convert.ToInt16(commands.ZZIS(""));

            if (msg == 1)
            {
                FilterBW = FilterBW + 50;
                commands.ZZIS(FilterBW.ToString("00000"));
                return;
            }

            if (msg == 127)
            {
                FilterBW = FilterBW - 50;
                commands.ZZIS(FilterBW.ToString("00000"));
                return;
            }
        }

        private void FilterShift(int msg)
        {
            int ShiftValue = (int)(((msg / 1.27) - 50) * 20);
            parser.nSet = 5;
            parser.nGet = 0;
            if (ShiftValue < 0)
            {
                commands.ZZIT(ShiftValue.ToString("0000"));
            }
            if (ShiftValue >= 0)
            {
                commands.ZZIT("+" + ShiftValue.ToString("0000"));
            }
            return;
        }



        //    case 49: //Crossfader

        private void RatioMainSubRx(int msg)
        {
            parser.nSet = 3;
            parser.nGet = 0;

            try
            {
                double mix = msg * 0.787;
                commands.ZZLB(mix.ToString("000"));
                commands.ZZLD((100 - mix).ToString("000"));
                return;
            }
            catch
            {
                return;
            }

        }


        private void AutoNotchOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int AutoNotchState = Convert.ToInt16(commands.ZZNT(""));

                if (AutoNotchState == 0)
                {
                    commands.ZZNT("1");
                    return;
                }
                if (AutoNotchState == 1)
                {
                    commands.ZZNT("0");
                    return;
                }
            }
        }


        private void Rx1NoiseBlanker1OnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int Rx1NB1State = Convert.ToInt16(commands.ZZNA(""));

                if (Rx1NB1State == 0)
                {
                    commands.ZZNA("1");
                    return;
                }
                if (Rx1NB1State == 1)
                {
                    commands.ZZNA("0");
                    return;
                }
            }
        }



        private void Rx2NoiseBlanker1OnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int Rx2NB1State = Convert.ToInt16(commands.ZZNC(""));

                    if (Rx2NB1State == 0)
                    {
                        commands.ZZNC("1");
                        return;
                    }
                    if (Rx2NB1State == 1)
                    {
                        commands.ZZNC("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void Rx1Noiseblanker2OnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int Rx1NB2State = Convert.ToInt16(commands.ZZNB(""));

                    if (Rx1NB2State == 0)
                    {
                        commands.ZZNB("1");
                        return;
                    }
                    if (Rx1NB2State == 1)
                    {
                        commands.ZZNB("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void Rx2Noiseblanker2OnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int Rx2NB2State = Convert.ToInt16(commands.ZZND(""));

                    if (Rx2NB2State == 0)
                    {
                        commands.ZZND("1");
                        return;
                    }
                    if (Rx2NB2State == 1)
                    {
                        commands.ZZND("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void LockVFO(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int VfoLockState = Convert.ToInt16(commands.ZZVL(""));

                if (VfoLockState == 0)
                {
                    commands.ZZVL("1");
                    return;
                }
                if (VfoLockState == 1)
                {
                    commands.ZZVL("0");
                    return;
                }
            }
        }

        private void RitOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int RitState = Convert.ToInt16(commands.ZZRT(""));

                if (RitState == 0)
                {
                    commands.ZZRT("1");
                    return;
                }
                if (RitState == 1)
                {
                    commands.ZZRT("0");
                    return;
                }
            }
        }

        private void XitOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int XitState = Convert.ToInt16(commands.ZZXS(""));

                if (XitState == 0)
                {
                    commands.ZZXS("1");
                    return;
                }
                if (XitState == 1)
                {
                    commands.ZZXS("0");
                    return;
                }
            }
        }

        private void SetAFGain(int msg)
        {
            int AFGain = (int)(msg / 1.27);
            parser.nSet = 3;
            commands.ZZAG(AFGain.ToString("000"));
            return;
        }

        //private void ChangeFreqVfoA(int msg)
        //{
        //    if (msg == 127)
        //    {
        //        commands.ZZSA();
        //    }
        //    if (msg == 1)
        //    {
        //        commands.ZZSB();
        //    }
        //}

        private int StringToFreq(string s)
        {
            int d = 0;
            int t = Convert.ToInt16(s);
            switch (t)
            {
                case 0:
                    d = 0000001;
                    break;
                case 1:
                    d = 000010;
                    break;
                case 2:
                    d = 000025;
                    break;
                case 3:
                    d = 000050;
                    break;
                case 4:
                    d = 000100;
                    break;
                case 5:
                    d = 000250;
                    break;
                case 6:
                    d = 000500;
                    break;
                case 7:
                    d = 001000;
                    break;
                case 8:
                    d = 002500;
                    break;
                case 9:
                    d = 005000;
                    break;
                case 10:
                    d = 006250;
                    break;
                case 11:
                    d = 009000;
                    break;
                case 12:
                    d = 010000;
                    break;
                case 13:
                    d = 012500;
                    break;
                case 14:
                    d = 015000;
                    break;
                case 15:
                    d = 020000;
                    break;
                case 16:
                    d = 025000;
                    break;
                case 17:
                    d = 030000;
                    break;
                case 18:
                    d = 050000;
                    break;
                case 19:
                    d = 100000;
                    break;
                case 20:
                    d = 250000;
                    break;
                case 21:
                    d = 500000;
                    break;
                case 22:
                    d = 1000000;
                    break;
                case 23:
                    d = 10000000;
                    break;
            }
            return (d);
        }



        private void ChangeFreqVfoA(int msg)
        {
            parser.nSet = 2;
            parser.nGet = 0;
            int step = StringToFreq(commands.ZZAC(""));
            ChangeFreqVfoA(msg, step,true);
        }

        private void ChangeFreqVfoA(int direction, int step, bool RoundToStepSize)
        {
            parser.nGet = 0;
            parser.nSet = 11;
            long freq = Convert.ToInt64(commands.ZZFA(""));
            parser.nAns = 11;
            int mode = Convert.ToInt16(commands.ZZMD(""));
            commands.isMidi = true;
            //System.Diagnostics.Debug.WriteLine("Msg=" + msg);
            switch (mode)
            {
                case 7: //DIGU
                    {
                        if (Convert.ToInt16(commands.ZZRA("")) == 1) //elminate CAT Offset for DIGU in case selected
                        {
                            int offsetDIGU = Convert.ToInt16(commands.ZZRH(""));

                            if (direction == 127)
                            {
                                freq -= offsetDIGU;
                                long x = SnapTune(freq, step, -1, RoundToStepSize) + offsetDIGU;
                                commands.ZZFA(x.ToString("D11"));
                            }
                            if (direction == 1)
                            {
                                freq -= offsetDIGU;
                                long x = SnapTune(freq, step, 1, RoundToStepSize) + offsetDIGU;
                                commands.ZZFA(x.ToString("D11"));
                            }

                        }
                        else
                        {
                            if (direction == 127)
                            {
                                commands.ZZFA((SnapTune(freq, step, -1, RoundToStepSize).ToString("D11")));
                            }
                            if (direction == 1)
                            {
                                commands.ZZFA((SnapTune(freq, step, 1, RoundToStepSize).ToString("D11")));
                            }
                        }
                        break;
                    }
                case 9: //DIGL
                    {
                        if (Convert.ToInt16(commands.ZZRA("")) == 1) //elminate CAT Offset for DIGL in case selected
                        {
                            int offsetDIGL = Convert.ToInt16(commands.ZZRL(""));

                            if (direction == 127)
                            {
                                freq += offsetDIGL;
                                long x = SnapTune(freq, step, -1, RoundToStepSize) - offsetDIGL;
                                commands.ZZFA(x.ToString("D11"));
                            }
                            if (direction == 1)
                            {
                                freq += offsetDIGL;
                                long x = SnapTune(freq, step, 1, RoundToStepSize) - offsetDIGL;
                                commands.ZZFA(x.ToString("D11"));
                            }

                        }
                        else
                        {
                            if (direction == 127)
                            {
                                commands.ZZFA((SnapTune(freq, step, -1, RoundToStepSize).ToString("D11")));
                            }
                            if (direction == 1)
                            {
                                commands.ZZFA((SnapTune(freq, step, 1, RoundToStepSize).ToString("D11")));
                            }
                        }
                        break;
                    }
                default: //for all other modes
                    {

                        if (direction == 127)
                        {
                            commands.ZZFA((SnapTune(freq, step, -1, RoundToStepSize).ToString("D11")));
                        }
                        if (direction == 1)
                        {
                            commands.ZZFA((SnapTune(freq, step, 1, RoundToStepSize).ToString("D11")));
                        }
                        break;
                    }
            }
            commands.isMidi = false;
        }

        private long SnapTune(long freq, int step, int num_steps, bool RoundToStepSize)
        {
            long temp;

            if (step == 0) //catch to avoid division by zero
            {
                return freq;
            }

            if (RoundToStepSize)
            {
                try
                {
                    temp = freq / step; // do integer division to end up on a step size boundary
                }
                catch
                {
                    return freq;
                }

                // handle when starting frequency was already on a step size boundary and tuning down
                if (num_steps < 0 && freq % step != 0)
                    num_steps++; // off boundary -- add one as the divide takes care of one step

                temp += num_steps; // increment by the number of steps (positive or negative)

                freq = temp * step; // multiply back up to get hz
                return freq; // return freq in MHz
            }
            else
            {
                if ( num_steps > 0 )
                    return freq + step;
                else
                    return freq - step;
            }
        }

        private void ChangeFreqVfoB(int msg)
        {
            bool RoundToStepSize = true;
            parser.nSet = 2;
            parser.nGet = 0;
            int mode;
            if (int.TryParse(commands.ZZMD(""), out mode) == false)
                return;
            int step = StringToFreq(commands.ZZAC(""));
            parser.nSet = 11;
            long freq = Convert.ToInt64(commands.ZZFB(""));
            parser.nAns = 11;

            commands.isMidi2 = true;

            switch (mode)
            {
                case 7: //DIGU
                    {
                        if (Convert.ToInt16(commands.ZZRB("")) == 1) //elminate CAT Offset for DIGU in case selected
                        {
                            int offsetDIGU = Convert.ToInt16(commands.ZZRH(""));

                            if (msg == 127)
                            {
                                freq -= offsetDIGU;
                                long x = SnapTune(freq, step, -1, RoundToStepSize) + offsetDIGU;
                                commands.ZZFB(x.ToString("D11"));
                            }
                            if (msg == 1)
                            {
                                freq -= offsetDIGU;
                                long x = SnapTune(freq, step, 1, RoundToStepSize) + offsetDIGU;
                                commands.ZZFB(x.ToString("D11"));
                            }

                        }
                        else
                        {
                            if (msg == 127)
                            {
                                commands.ZZFB((SnapTune(freq, step, -1, RoundToStepSize).ToString("D11")));
                            }
                            if (msg == 1)
                            {
                                commands.ZZFB((SnapTune(freq, step, 1, RoundToStepSize).ToString("D11")));
                            }
                        }
                        break;
                    }
                case 9: //DIGL
                    {
                        if (Convert.ToInt16(commands.ZZRB("")) == 1) //elminate CAT Offset for DIGL in case selected
                        {
                            int offsetDIGL = Convert.ToInt16(commands.ZZRL(""));

                            if (msg == 127)
                            {
                                freq += offsetDIGL;
                                long x = SnapTune(freq, step, -1, RoundToStepSize) - offsetDIGL;
                                commands.ZZFB(x.ToString("D11"));
                            }
                            if (msg == 1)
                            {
                                freq += offsetDIGL;
                                long x = SnapTune(freq, step, 1, RoundToStepSize) - offsetDIGL;
                                commands.ZZFB(x.ToString("D11"));
                            }

                        }
                        else
                        {
                            if (msg == 127)
                            {
                                commands.ZZFB((SnapTune(freq, step, -1, RoundToStepSize).ToString("D11")));
                            }
                            if (msg == 1)
                            {
                                commands.ZZFB((SnapTune(freq, step, 1, RoundToStepSize).ToString("D11")));
                            }
                        }
                        break;
                    }
                default:
                    {

                        if (msg == 127)
                        {
                            commands.ZZFB((SnapTune(freq, step, -1, RoundToStepSize).ToString("D11")));
                        }
                        if (msg == 1)
                        {
                            commands.ZZFB((SnapTune(freq, step, 1, RoundToStepSize).ToString("D11")));
                        }
                        break;
                    }
            }
            commands.isMidi2 = false;
        }

        private void BinauralOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int BINState = Convert.ToInt16(commands.ZZBI(""));

                if (BINState == 0)
                {
                    commands.ZZBI("1");
                    return;
                }
                if (BINState == 1)
                {
                    commands.ZZBI("0");
                    return;
                }
            }
        }

        private void MuteOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int MuteState = Convert.ToInt16(commands.ZZMA(""));

                if (MuteState == 0)
                {
                    commands.ZZMA("1");
                    return;
                }
                if (MuteState == 1)
                {
                    commands.ZZMA("0");
                    return;
                }
            }
        }

        private void SpurReductionOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int SRState = Convert.ToInt16(commands.ZZSR(""));

                if (SRState == 0)
                {
                    commands.ZZSR("1");
                    return;
                }
                if (SRState == 1)
                {
                    commands.ZZSR("0");
                    return;
                }
            }
        }


        private void NoiseReductionOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int NRState = Convert.ToInt16(commands.ZZNR(""));

                if (NRState == 0)
                {
                    commands.ZZNR("1");
                    return;
                }
                if (NRState == 1)
                {
                    commands.ZZNR("0");
                    return;
                }
            }
        }

        private void Rx2NoiseReductionOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int NRState = Convert.ToInt16(commands.ZZNS(""));

                if (NRState == 0)
                {
                    commands.ZZNS("1");
                    return;
                }
                if (NRState == 1)
                {
                    commands.ZZNS("0");
                    return;
                }
            }
        }

        private void Rx2PreAmpOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int PreAmpState = Convert.ToInt16(commands.ZZPB(""));

                    if (PreAmpState == 0)
                    {
                        commands.ZZPB("1");
                        return;
                    }
                    if (PreAmpState == 1)
                    {
                        commands.ZZPB("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void VfoSyncOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int SyncState = Convert.ToInt16(commands.ZZSY(""));

                if (SyncState == 0)
                {
                    commands.ZZSY("1");
                    return;
                }
                if (SyncState == 1)
                {
                    commands.ZZSY("0");
                    return;
                }
            }
        }

        private void SplitOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int SplitState = Convert.ToInt16(commands.ZZSP(""));

                if (SplitState == 0)
                {
                    commands.ZZSP("1");
                    return;
                }
                if (SplitState == 1)
                {
                    commands.ZZSP("0");
                    return;
                }
            }
        }

        private void MOXOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int MOXState = Convert.ToInt16(commands.ZZTX(""));

                if (MOXState == 0)
                {
                    commands.ZZTX("1");
                    return;
                }
                if (MOXState == 1)
                {
                    commands.ZZTX("0");
                    return;
                }
            }
        }

        private void VOXOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int VOXState = Convert.ToInt16(commands.ZZVE(""));

                if (VOXState == 0)
                {
                    commands.ZZVE("1");
                    return;
                }
                if (VOXState == 1)
                {
                    commands.ZZVE("0");
                    return;
                }
            }
        }

        private void CompanderOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int CMPRState = Convert.ToInt16(commands.ZZCP(""));

                if (CMPRState == 0)
                {
                    commands.ZZCP("1");
                    return;
                }
                if (CMPRState == 1)
                {
                    commands.ZZCP("0");
                    return;
                }
            }
        }

        private void PhoneDXOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int DXState = Convert.ToInt16(commands.ZZDX(""));

                if (DXState == 0)
                {
                    commands.ZZDX("1");
                    return;
                }
                if (DXState == 1)
                {
                    commands.ZZDX("0");
                    return;
                }
            }
        }

        private void DEXPOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int DEXPState = Convert.ToInt16(commands.ZZGE(""));

                if (DEXPState == 0)
                {
                    commands.ZZGE("1");
                    return;
                }
                if (DEXPState == 1)
                {
                    commands.ZZGE("0");
                    return;
                }
            }
        }

        private void RX2OnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int RX2State = Convert.ToInt16(commands.ZZRS(""));
                    if (RX2State == 0)
                    {
                        commands.ZZRS("1");
                        return;
                    }
                    if (RX2State == 1)
                    {
                        commands.ZZRS("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }
        private void StartOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;
                try
                {
                    int StartState = Convert.ToInt16(commands.ZZPS(""));

                    parser.nSet = 1;

                    if (StartState == 0)
                    {
                        commands.ZZPS("1");
                        return;
                    }
                    if (StartState == 1)
                    {
                        commands.ZZPS("0");
                        return;
                    }
                }
                catch
                { }
            }
        }

        private void TunerOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int TunerState = Convert.ToInt16(commands.ZZOV(""));

                    if (TunerState == 0)
                    {
                        commands.ZZOV("1");
                        return;
                    }
                    if (TunerState == 1)
                    {
                        commands.ZZOV("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void Tun(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int TunerState = Convert.ToInt16(commands.ZZTU(""));

                    if (TunerState == 0)
                    {
                        commands.ZZTU("1");
                        return;
                    }
                    if (TunerState == 1)
                    {
                        commands.ZZTU("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void TunerBypass(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int TunerState = Convert.ToInt16(commands.ZZOW(""));

                    if (TunerState == 0)
                    {
                        commands.ZZOW("1");
                        return;
                    }
                    if (TunerState == 1)
                    {
                        commands.ZZOW("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }


        private void ZeroBeatPress(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                commands.ZZZB();
            }
        }

        private void BandUp(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 0;

                try
                {
                    commands.ZZBU();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void BandDown(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 0;

                try
                {
                    commands.ZZBD();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Rx2BandUp(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 0;

                try
                {
                    commands.ZZBA();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Rx2BandDown(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 0;

                try
                {
                    commands.ZZBB();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void PreAmpSettingsKnob(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 1;

            try
            {
                switch (Convert.ToInt16(commands.ZZFM()))
                {
                    case 0: // HPSDR HERMES ANAN10
                        {
                            if ((msg >= 0) && (msg < 64))
                            {
                                commands.ZZPA("1");
                                return;
                            }
                            if ((msg >= 64) && (msg < 128))
                            {
                                commands.ZZPA("0");
                                return;
                            }
                            break;
                        }
                    case 1: // w/ALEX ANAN100/D
                        {
                            if ((msg >= 0) && (msg < 16))
                            {
                                commands.ZZPA("1");
                                return;
                            }
                            if ((msg >= 16) && (msg < 32))
                            {
                                commands.ZZPA("0");
                                return;
                            }
                            if ((msg >= 32) && (msg < 48))
                            {
                                commands.ZZPA("2");
                                return;
                            }
                            if ((msg >= 48) && (msg < 64))
                            {
                                commands.ZZPA("3");
                                return;
                            }
                            if ((msg >= 64) && (msg < 80))
                            {
                                commands.ZZPA("4");
                                return;
                            }
                            if ((msg >= 80) && (msg < 96))
                            {
                                commands.ZZPA("5");
                                return;
                            }
                            if ((msg >= 112) && (msg < 128))
                            {
                                commands.ZZPA("6");
                                return;
                            }
                            break;
                        }
                    case 2: //ANAN10
                        {
                            if ((msg >= 0) && (msg < 32))
                            {
                                commands.ZZPA("0");
                                return;
                            }
                            if ((msg >= 32) && (msg < 64))
                            {
                                commands.ZZPA("1");
                                return;
                            }
                            if ((msg >= 64) && (msg < 96))
                            {
                                commands.ZZPA("2");
                                return;
                            }
                            if ((msg >= 96) && (msg < 128))
                            {
                                commands.ZZPA("3");
                                return;
                            }

                            break;
                        }
                    case 3: //ANAN100/D
                        {
                            if ((msg >= 0) && (msg < 25))
                            {
                                commands.ZZPA("0");
                                return;
                            }
                            if ((msg >= 25) && (msg < 51))
                            {
                                commands.ZZPA("1");
                                return;
                            }
                            if ((msg >= 51) && (msg < 77))
                            {
                                commands.ZZPA("2");
                                return;
                            }
                            if ((msg >= 77) && (msg < 102))
                            {
                                commands.ZZPA("3");
                                return;
                            }
                            if ((msg >= 102) && (msg < 128))
                            {
                                commands.ZZPA("4");
                                return;
                            }
                            break;
                        }
                }
            }
            catch
            {
                return;
            }
        }

        private void CWSpeed(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 2;

            try
            {
                if (msg != 0)
                {
                    double cwspeed = msg / 2.1333 + 1;
                    string a = cwspeed.ToString("00");
                    commands.ZZCS(cwspeed.ToString("00"));

                }
                return;
            }
            catch
            {
                return;
            }
        }

        private void AGCLevel(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 4;

            try
            {
                double agclevel = ((msg * 1.099) - 20);
                if (agclevel >= 0)
                {
                    commands.ZZAR("+" + agclevel.ToString("000"));
                }
                if (agclevel < 0)
                {
                    commands.ZZAR(agclevel.ToString("000"));
                }
                return;
            }
            catch
            {
                return;
            }
        }

        private void RX2AGCLevel(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 4;

            try
            {
                double agclevel = ((msg * 1.099) - 20);
                if (agclevel >= 0)
                {
                    commands.ZZAS("+" + agclevel.ToString("000"));
                }
                if (agclevel < 0)
                {
                    commands.ZZAS(agclevel.ToString("000"));
                }
                return;
            }
            catch
            {
                return;
            }
        }

        private void MicGain(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 3;
            parser.nAns = 3;

            try
            {
                double micgain = (msg / 1.82);
                string a = micgain.ToString("000");
                commands.ZZMG(micgain.ToString("000"));

            }
            catch
            {
                return;
            }
        }

        private void SquelchControl(int msg)
        {
            parser.nSet = 0;
            parser.nSet = 3;

            try
            {
                double sqlctrl = 160 - (msg * 1.26);
                commands.ZZSQ(sqlctrl.ToString("000"));
                return;
            }
            catch
            {
                return;
            }
        }

        private void CPDRLevel(int msg)
        {
            parser.nSet = 0;
            parser.nSet = 2;

            try
            {
                double cpdr = msg * 0.078;
                commands.ZZCT(cpdr.ToString("00"));
                return;
            }
            catch
            {
                return;
            }
        }

        private void DXLevel(int msg)
        {
            parser.nSet = 0;
            parser.nSet = 2;

            try
            {
                double dx = msg * 0.078;
                commands.ZZDY(dx.ToString("00"));
                return;
            }
            catch
            {
                return;
            }
        }

        private void VOXGain(int msg)
        {
            parser.nSet = 0;
            parser.nSet = 4;

            try
            {
                double vox = msg * 7.89;
                commands.ZZVG(vox.ToString("0000"));
                return;
            }
            catch
            {
                return;
            }
        }

        private void DEXPThreshold(int msg)
        {
            parser.nSet = 0;
            parser.nSet = 4;

            try
            {
                double dexp = -160 + (msg * 1.26);
                if (dexp < 0)
                {
                    commands.ZZGL(dexp.ToString("000"));
                    return;
                }
                else
                {
                    commands.ZZGL(dexp.ToString("0000"));
                    return;
                }
            }
            catch
            {
                return;
            }
        }

        private void TXAFMonitor(int msg)
        {
            parser.nSet = 3;
            parser.nGet = 0;

            try
            {
                double txaf = msg * 0.787;
                commands.ZZTM(txaf.ToString("000"));
                return;
            }
            catch
            {
                return;
            }
        }

        private void DriveLevel(int msg)
        {
            parser.nSet = 3;
            parser.nGet = 0;

            try
            {
                double drive = msg * 0.787;
                commands.ZZPC(drive.ToString("000"));
                return;
            }
            catch
            {
                return;
            }
        }

        private void RXEQOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int RXEQState = Convert.ToInt16(commands.ZZER(""));

                if (RXEQState == 0)
                {
                    commands.ZZER("1");
                    return;
                }
                if (RXEQState == 1)
                {
                    commands.ZZER("0");
                    return;
                }
            }
        }

        private void TXEQOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int TXEQState = Convert.ToInt16(commands.ZZET(""));

                if (TXEQState == 0)
                {
                    commands.ZZET("1");
                    return;
                }
                if (TXEQState == 1)
                {
                    commands.ZZET("0");
                    return;
                }
            }
        }

        private void SquelchOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int sql = Convert.ToInt16(commands.ZZSO(""));

                    if (sql == 0)
                    {
                        commands.ZZSO("1");
                        return;
                    }
                    if (sql == 1)
                    {
                        commands.ZZSO("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void AGCModeKnob(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 1;

            if ((msg >= 0) && (msg < 22))
            {
                commands.ZZGT("0");
                return;
            }
            if ((msg >= 22) && (msg < 43))
            {
                commands.ZZGT("1");
                return;
            }
            if ((msg >= 43) && (msg < 64))
            {
                commands.ZZGT("2");
                return;
            }
            if ((msg >= 64) && (msg < 85))
            {
                commands.ZZGT("3");
                return;
            }

            if ((msg >= 85) && (msg < 106))
            {
                commands.ZZGT("4");
                return;
            }

            if ((msg >= 106) && (msg < 128))
            {
                commands.ZZGT("5");
                return;
            }

        }

        private void AGCModeUp(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 1;

            if (msg == 127)
            {
                try
                {
                    int agcstate = Convert.ToInt16(commands.ZZGT(""));

                    if ((agcstate > 0) && (agcstate <= 5))
                    {
                        agcstate = agcstate - 1;
                        commands.ZZGT(agcstate.ToString("0"));
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void AGCModeDown(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 1;

            if (msg == 127)
            {
                try
                {
                    int agcstate = Convert.ToInt16(commands.ZZGT(""));

                    if ((agcstate >= 0) && (agcstate < 5))
                    {
                        agcstate = agcstate + 1;
                        commands.ZZGT(agcstate.ToString("0"));
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void PreampFlex5000(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 1;

            if (msg == 127)
            {
                try
                {
                    if (commands.ZZFM() == "1") //check if TRX = Flex5000
                    {
                        if (commands.ZZPA("") == "0")
                        {
                            commands.ZZPA("1");
                            return;
                        }

                        if (commands.ZZPA("") == "1")
                        {
                            commands.ZZPA("0");
                            return;
                        }
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void DisplayAverage(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int avg = Convert.ToInt16(commands.ZZDA(""));

                    if (avg == 0)
                    {
                        commands.ZZDA("1");
                        return;
                    }
                    if (avg == 1)
                    {
                        commands.ZZDA("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void DisplayPeak(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int avg = Convert.ToInt16(commands.ZZPO(""));

                    if (avg == 0)
                    {
                        commands.ZZPO("1");
                        return;
                    }
                    if (avg == 1)
                    {
                        commands.ZZPO("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void DisplayTxFilter(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int txf = Convert.ToInt16(commands.ZZTF(""));

                    if (txf == 0)
                    {
                        commands.ZZTF("1");
                        return;
                    }
                    if (txf == 1)
                    {
                        commands.ZZTF("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void VACOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int vac = Convert.ToInt16(commands.ZZVA(""));

                    if (vac == 0)
                    {
                        commands.ZZVA("1");
                        return;
                    }
                    if (vac == 1)
                    {
                        commands.ZZVA("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void VAC2OnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int vac2 = Convert.ToInt16(commands.ZZVK(""));

                    if (vac2 == 0)
                    {
                        commands.ZZVK("1");
                        return;
                    }
                    if (vac2 == 1)
                    {
                        commands.ZZVK("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void IQtoVAC(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int iq = Convert.ToInt16(commands.ZZVH(""));

                    if (iq == 0)
                    {
                        commands.ZZVH("1");
                        return;
                    }
                    if (iq == 1)
                    {
                        commands.ZZVH("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void IQtoVACRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int iq = Convert.ToInt16(commands.ZZVJ(""));
                    int vac2 = Convert.ToInt16(commands.ZZVH(""));

                    if (iq == 0)
                    {
                        commands.ZZVH("1");
                        commands.ZZVJ("1");
                        return;
                    }
                    if (iq == 1)
                    {
                        commands.ZZVJ("0");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }


        private void DisplayModePrev(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int dpm = Convert.ToInt16(commands.ZZDM(""));

                    if ((dpm > 0) && (dpm <= 7))
                    {
                        dpm = dpm - 1;
                        commands.ZZDM(dpm.ToString("0"));
                        return;
                    }

                    if ((dpm == 9)) //workaround Bug #3661
                    {
                        dpm = 6;
                        commands.ZZDM(dpm.ToString("0"));
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void DisplayModeNext(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int dpm = Convert.ToInt16(commands.ZZDM(""));

                    if ((dpm >= 0) && (dpm < 7))
                    {
                        dpm = dpm + 1;
                        commands.ZZDM(dpm.ToString("0"));
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void ZoomDec(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int zoom = Convert.ToInt16(commands.ZZPZ(""));
                    int zoomf = Convert.ToInt16(commands.ZZPY(""));
                    //if ((zoomf >= 10) && (zoomf <= 49))
                    //{
                    //    commands.ZZPZ("0");
                    //    return;
                    //}
                    if ((zoomf >= 51) && (zoomf <= 150))
                    {
                        commands.ZZPZ("0");
                        return;
                    }
                    if ((zoomf >= 151) && (zoomf <= 200))
                    {
                        commands.ZZPZ("1");
                        return;
                    }
                    if ((zoomf >= 201) && (zoomf <= 225))
                    {
                        commands.ZZPZ("2");
                        return;
                    }
                    if ((zoomf >= 226) && (zoomf <= 240))
                    {
                        commands.ZZPZ("3");
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void ZoomInc(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    int zoom = Convert.ToInt16(commands.ZZPZ(""));
                    int zoomf = Convert.ToInt16(commands.ZZPY("")); //check slider position and select closest step

                    if ((zoomf >= 10) && (zoomf <= 49))
                    {
                        commands.ZZPZ("0"); //050
                        return;
                    }
                    if ((zoomf >= 50) && (zoomf <= 149))
                    {
                        commands.ZZPZ("1"); //150
                        return;
                    }
                    if ((zoomf >= 150) && (zoomf <= 199))
                    {
                        commands.ZZPZ("2"); //200
                        return;
                    }
                    if ((zoomf >= 200) && (zoomf <= 225))
                    {
                        commands.ZZPZ("3"); //225
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        //private void ZoomSliderFix(int msg)
        //{
        //    parser.nGet = 0;
        //    parser.nSet = 3;

        //    double zoom = (msg * 1.797) + 10;
        //    commands.ZZPY(zoom.ToString("000"));
        //    return;
        //}

        private void ZoomSliderInc(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 3;

            try
            {
                int zoom = Convert.ToInt16(commands.ZZPY(""));
                if ((msg == 127) && (zoom >= 15))
                {
                    zoom = zoom - 5;
                    commands.ZZPY(zoom.ToString("000"));
                    return;
                }
                if ((msg == 1) && (zoom <= 235))
                {
                    zoom = zoom + 5;
                    commands.ZZPY(zoom.ToString("000"));
                    return;
                }
            }
            catch
            {
                return;
            }
        }


        private void PanSliderInc(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 4;

            try
            {
                int pan = Convert.ToInt16(commands.ZZPE(""));
                if ((msg == 127) && (pan >= 50))
                {
                    pan = pan - 25;
                    commands.ZZPE(pan.ToString("0000"));
                    return;
                }
                if ((msg == 1) && (pan <= 235))
                {
                    pan = pan + 25;
                    commands.ZZPE(pan.ToString("0000"));
                    return;
                }
            }
            catch
            {
                return;
            }
        }

        private void PanSlider(int msg)
        {
            try
            {
                parser.nSet = 4;
                parser.nGet = 0;

                double PanValue = msg * 7.87;
                commands.ZZPE(PanValue.ToString("0000"));
                return;
            }
            catch
            {
                return;
            }
        }


        private void SpectralNoiseBlankerOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int Rx1SNBState = Convert.ToInt16(commands.ZZNN(""));

                if (Rx1SNBState == 0)
                {
                    commands.ZZNN("1");
                    return;
                }
                if (Rx1SNBState == 1)
                {
                    commands.ZZNN("0");
                    return;
                }
            }
        }

        private void SpectralNoiseBlankerRx2OnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int Rx2SNBState = Convert.ToInt16(commands.ZZNO(""));

                if (Rx2SNBState == 0)
                {
                    commands.ZZNO("1");
                    return;
                }
                if (Rx2SNBState == 1)
                {
                    commands.ZZNO("0");
                    return;
                }
            }
        }

        private void QuickModeSave(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZQS();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro1(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("1");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("2");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro3(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("3");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro4(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("4");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro5(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("5");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro6(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("6");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro7(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("7");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro8(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("8");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXMacro9(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZKM("9");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void CWXStop(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZSS();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void MONOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int MONState = Convert.ToInt16(commands.ZZMO(""));

                if (MONState == 0)
                {
                    commands.ZZMO("1");
                    return;
                }
                if (MONState == 1)
                {
                    commands.ZZMO("0");
                    return;
                }
            }
        }

        private void PanCenter(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZPD();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void QuickModeRestore(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                try
                {
                    commands.ZZQR();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ZoomSliderFix(int msg)
        {
            try
            {
                parser.nSet = 3;
                parser.nGet = 0;

                double ZoomValue = msg * 1.88;
                commands.ZZPY(ZoomValue.ToString("000"));
                return;
            }
            catch
            {
                return;
            }
        }

        private void FilterHigh(int msg)
        {
            int tuningstep = 20;

            try
            {
                parser.nGet = 0;
                parser.nSet = 2;

                string s = commands.ZZMD("");
                int mode = Convert.ToInt32(s);

                if ((mode == 3) || (mode == 4) || (mode == 7) || (mode == 8) || (mode == 9))
                {
                    tuningstep = 20;
                }
                else
                {
                    tuningstep = 50;
                }
            }
            catch
            {
                return;
            }

            try
            {

                parser.nSet = 5;
                parser.nGet = 0;
                parser.nAns = 5;

                string s = commands.ZZFH("");

                int UpperEdge = Convert.ToInt32(s);


                if ((msg == 1) && (UpperEdge >= 0))
                {
                    UpperEdge = UpperEdge + tuningstep;
                    commands.ZZFH(UpperEdge.ToString("00000"));
                    return;
                }

                if ((msg == 1) && (UpperEdge < 0))
                {
                    if (UpperEdge > (-tuningstep - 1))
                    {
                        UpperEdge = UpperEdge + tuningstep;
                        commands.ZZFH(UpperEdge.ToString("00000"));
                        return;
                    }
                    else
                    {
                        UpperEdge = UpperEdge + tuningstep;
                        commands.ZZFH(UpperEdge.ToString("0000"));
                        return;
                    }
                }

                if ((msg == 127) && (UpperEdge >= 0))
                {
                    if (UpperEdge < tuningstep)
                    {
                        UpperEdge = UpperEdge - tuningstep;
                        commands.ZZFH(UpperEdge.ToString("0000"));
                        return;
                    }
                    else
                    {
                        UpperEdge = UpperEdge - tuningstep;
                        commands.ZZFH(UpperEdge.ToString("00000"));
                        return;
                    }
                }

                if ((msg == 127) && (UpperEdge < 0))
                {
                    UpperEdge = UpperEdge - tuningstep;
                    commands.ZZFH(UpperEdge.ToString("0000"));
                    return;
                }
            }
            catch
            {
                return;
            }
        }

        private void FilterLow(int msg)
        {
            int tuningstep = 20;

            try
            {
                parser.nGet = 0;
                parser.nSet = 2;

                string s = commands.ZZMD("");
                int mode = Convert.ToInt32(s);

                if ((mode == 3) || (mode == 4) || (mode == 7) || (mode == 8) || (mode == 9))
                {
                    ; // tuningstep = tuningstep;
                }
                else
                {
                    tuningstep = 50;
                }
            }
            catch
            {
                return;
            }

            try
            {

                parser.nSet = 5;
                parser.nGet = 0;
                parser.nAns = 5;

                string s = commands.ZZFL("");

                int LowerEdge = Convert.ToInt32(s);


                if ((msg == 1) && (LowerEdge >= 0))
                {
                    LowerEdge = LowerEdge + tuningstep;
                    commands.ZZFL(LowerEdge.ToString("00000"));
                    return;
                }

                if ((msg == 1) && (LowerEdge < 0))
                {
                    if (LowerEdge > (-tuningstep - 1))
                    {
                        LowerEdge = LowerEdge + tuningstep;
                        commands.ZZFL(LowerEdge.ToString("00000"));
                        return;
                    }
                    else
                    {
                        LowerEdge = LowerEdge + tuningstep;
                        commands.ZZFL(LowerEdge.ToString("0000"));
                        return;
                    }
                }

                if ((msg == 127) && (LowerEdge >= 0))
                {
                    if (LowerEdge < tuningstep)
                    {
                        LowerEdge = LowerEdge - tuningstep;
                        commands.ZZFL(LowerEdge.ToString("0000"));
                        return;
                    }
                    else
                    {
                        LowerEdge = LowerEdge - tuningstep;
                        commands.ZZFL(LowerEdge.ToString("00000"));
                        return;
                    }
                }

                if ((msg == 127) && (LowerEdge < 0))
                {
                    LowerEdge = LowerEdge - tuningstep;
                    commands.ZZFL(LowerEdge.ToString("0000"));
                    return;
                }
            }
            catch
            {
                return;
            }

        }

        private void VACGainRX(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 3;

            try
            {
                int vac = (int)((msg - 63) * 0.64);
                commands.ZZVB(vac.ToString("000;-00;000"));
                return;

            }
            catch
            {
                return;
            }
        }

        private void VACGainTX(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 3;

            try
            {
                int vac = (int)((msg - 63) * 0.64);
                commands.ZZVC(vac.ToString("000;-00;000"));
                return;

            }
            catch
            {
                return;
            }
        }


        private void VAC2GainRX(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 3;

            try
            {
                int vac = (int)((msg - 63) * 0.64);
                commands.ZZVW(vac.ToString("000;-00;000"));
                return;

            }
            catch
            {
                return;
            }
        }

        private void VAC2GainTX(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 3;

            try
            {
                int vac = (int)((msg - 63) * 0.64);
                commands.ZZVX(vac.ToString("000;-00;000"));
                return;

            }
            catch
            {
                return;
            }
        }



        //private void ESCOnOff(int msg)
        //{
        //    if (msg == 127)
        //    {
        //        parser.nGet = 0;
        //        parser.nSet = 1;

        //        int ESC = Convert.ToInt16(commands.ZZDE(""));

        //        if (ESC == 0)
        //        {
        //            commands.ZZDE("1");
        //            return;
        //        }
        //        if (ESC == 1)
        //        {
        //            commands.ZZDE("0");
        //            return;
        //        }
        //    }
        //}

        private void CTun(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int ESC = Convert.ToInt16(commands.ZZCN(""));

                if (ESC == 0)
                {
                    commands.ZZCN("1");
                    return;
                }
                if (ESC == 1)
                {
                    commands.ZZCN("0");
                    return;
                }
            }
        }

        private void ESCFormOnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int ESC = Convert.ToInt16(commands.ZZDF(""));

                if (ESC == 0)
                {
                    commands.ZZDF("1");
                    return;
                }
                if (ESC == 1)
                {
                    commands.ZZDF("0");
                    return;
                }
            }
        }

        private void WaterfallLowLimit(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 4;

            try
            {
                int wf = (int)((msg - 63) * 3.125);
                commands.ZZDN(wf.ToString("0000;-000;0000"));
                commands.ZZDQ(wf.ToString("0000;-000;0000"));
                return;

            }
            catch
            {
                return;
            }
        }

        private void WaterfallHighLimit(int msg)
        {
            parser.nGet = 0;
            parser.nSet = 4;

            try
            {
                int wf = (int)((msg - 63) * 3.125);
                commands.ZZDO(wf.ToString("0000;-000;0000"));
                commands.ZZDP(wf.ToString("0000;-000;0000"));
                return;

            }
            catch
            {
                return;
            }
        }

        private void MuteRX2OnOff(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 1;

                int MuteState = Convert.ToInt16(commands.ZZMB(""));

                if (MuteState == 0)
                {
                    commands.ZZMB("1");
                    return;
                }
                if (MuteState == 1)
                {
                    commands.ZZMB("0");
                    return;
                }
            }
        }

        private void Band160m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("160");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band80m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("080");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band60m(int msg) //not yet implemented PSDR 2.4.4
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("060");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band40m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("040");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band30m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("030");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band20m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("020");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }


        private void Band17m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("017");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band15m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("015");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band12m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("012");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band10m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("010");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band6m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("006");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band2m(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBS("002");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band160mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("160");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band80mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("080");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band60mRX2(int msg) //not yet implemented PSDR 2.4.4
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("060");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }


        private void Band40mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("040");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band30mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("030");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band20mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("020");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }


        private void Band17mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("017");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band15mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("015");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band12mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("012");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band10mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("010");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band6mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("006");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void Band2mRX2(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 3;

                try
                {
                    commands.ZZBT("002");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeSSB(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;
                parser.nAns = 3;

                int band = 0;

                try
                {
                    band = Convert.ToInt16(commands.ZZBS(""));
                }
                catch
                {
                    band = 0;
                }

                parser.nGet = 0;
                parser.nSet = 2;
                parser.nAns = 2;

                try
                {
                    if (band >= 40) commands.ZZMD("00");
                    if (band < 40) commands.ZZMD("01");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeLSB(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("00");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }
        private void ModeUSB(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("01");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeDSB(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("02");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeCW(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("01");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeCWL(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("03");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeCWU(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("04");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeFM(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("05");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeAM(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("06");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeDIGU(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("07");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }


        private void ModeSPEC(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("08");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeDIGL(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("09");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void ModeSAM(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("10");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }


        private void ModeDRM(int msg)
        {
            if (msg == 127)
            {
                parser.nGet = 0;
                parser.nSet = 2;

                try
                {
                    commands.ZZMD("11");
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        private void MultiStepVfoA(int msg)
        {
            TimeSpan timeDiff=DateTime.Now - LastVfoAUpdateTime;
            NumVfoASteps++;
            if (timeDiff.TotalMilliseconds >= 100 && NumVfoASteps>5)
            {
                if (timeDiff.TotalMilliseconds <= 200 )
                {
                    int digitOffset=-1;
                    int direction = 0;

                    if (VfoAStepValue > 0)
                        direction = 127;
                    else if (VfoAStepValue < 0)
                        direction = 1;
                    /*
                    int rate = Math.Abs((int)timeDiff.TotalMilliseconds / NumVfoASteps);
                    timeDiff=DateTime.Now - VfoALastSpeedChangeTime;
                    if ( VfoALastSpeed==0 || timeDiff.TotalMilliseconds > 1000)
                    {
                        if (rate < 5)     //fast
                        {
                            digitOffset = 100 * VfoAConsoleStep;
                            VfoALastSpeedChangeTime = DateTime.Now;
                            VfoALastSpeed = digitOffset;
                        }
                        else if (rate < 20) //medium
                        {
                            digitOffset = 10 * VfoAConsoleStep;
                            VfoALastSpeedChangeTime = DateTime.Now;
                            VfoALastSpeed = digitOffset;
                        }
                        else
                        {
                            digitOffset = 1 * VfoAConsoleStep; // slow
                            VfoALastSpeedChangeTime = DateTime.Now;
                            VfoALastSpeed = digitOffset;
                        }
                    }
                    else
                    {
                        VfoALastSpeedChangeTime = DateTime.Now;
                        digitOffset = VfoALastSpeed;
                    }

                    if (digitOffset != -1 && ( VfoALastSpeed == digitOffset) || ( VfoALastSpeed==0) )
                    {
                        System.Diagnostics.Debug.WriteLine("rate=" + rate + " direction=" + direction + " step=" + digitOffset);
                        ChangeFreqVfoA(direction, digitOffset, false);
                    }  
                     */
                    parser.nSet = 2;
                    parser.nGet = 0;
                    int step;
                    if ( VfoAConsoleStep > 1 )
                        step=VfoAConsoleStep;
                    else
                        step = StringToFreq(commands.ZZAC(""));
                    ChangeFreqVfoA(direction,step,false);
                }
                NumVfoASteps = 0;
                VfoAStepValue = 0;
                LastVfoAUpdateTime = DateTime.Now;
            }
            else
            {
                if ( msg >=127 )
                    VfoAStepValue++;
                else
                    VfoAStepValue--;
            }
        }

        private void SetVfoAConsoleStepKhz(int msg)
        {
            if (msg == 127)
            {
                if (VfoAConsoleStep == 1)
                    VfoAConsoleStep = 1000;
                else
                    VfoAConsoleStep = 1;
            }
        }

        private void SetVfoAConsoleStepMhz(int msg)
        {
            if (msg == 127)
            {
                if (VfoAConsoleStep == 1)
                    VfoAConsoleStep = 1000000;
                else
                    VfoAConsoleStep = 1;
            }
        }

    } // class
}// namespace
