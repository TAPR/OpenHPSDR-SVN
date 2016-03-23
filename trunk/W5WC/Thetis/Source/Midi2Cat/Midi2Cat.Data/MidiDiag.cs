using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Midi2Cat.Data
{
    public class MidiDiagItem
    {
        public int SeqNum { get; set; }
        public string Device { get; set; }
        public string ControlId { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
        public string Voice { get; set; }
        public string Channel { get; set; }
    }

    public class MidiDiagList : BindingList<MidiDiagItem>
    {
        int seqnum;
        public MidiDiagList()
        {
        }

        public new void Add(MidiDiagItem item)
        {
            item.SeqNum = seqnum++;
            base.Add(item);
        }
    }
}
