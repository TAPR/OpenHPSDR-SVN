using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi2Cat.Data
{
    public delegate void ProcessMidiMessageHandler(int msg);
    public delegate CmdState ProcessMidiMessageToggleHandler(int msg);

    public class ControllerMapping
    {
        public int MidiControlId { get; set; }
        public string MidiControlName { get; set; }
        public ControlType MidiControlType { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public CatCmd CatCmdId { get; set; }
        public string MidiOutCmdDown { get; set; }
        public string MidiOutCmdUp { get; set; }
        public string MidiOutCmdSetValue { get; set; }

        // Not stored in the DB properies/events
        public CatCommandAttribute CatCmd { get; set; }
        public ProcessMidiMessageHandler onProcessMidiMessage;

        public override string ToString()
        {
            return MidiControlName + " -> " + CatCmd.Desc;
        }
    }
}
