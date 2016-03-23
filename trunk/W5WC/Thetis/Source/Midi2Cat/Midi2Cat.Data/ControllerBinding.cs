using Midi2Cat.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midi2Cat.Data
{
    public class MidMessageHandler
    {
        public ProcessMidiMessageHandler CmdHandler;
        public ProcessMidiMessageToggleHandler ToggleCmdHandler;
        public string MidiOutCmdDown;
        public string MidiOutCmdUp;
        public string MidiOutCmdSetValue;
    }

    public class ControllerBinding
    {
        public string DeviceName;
        public int DeviceIndex;
        public MidiDevice Device;
        public Dictionary<int, MidMessageHandler> CmdBindings;
    }
}
