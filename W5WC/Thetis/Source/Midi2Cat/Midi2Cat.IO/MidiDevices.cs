using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi2Cat.IO
{
    public class MidiDevices
    {
        public int DeviceInCount
        {
            get
            {
                return WinMM.MidiInGetNumDevs();
            }
        }

        public int DeviceOutCount
        {
            get
            {
                return WinMM.MidiOutGetNumDevs();
            }
        }

        public Collection<string> InDevices
        {
            get
            {
                Collection<string> inDevices = new Collection<string>();
                int numInDevices = DeviceInCount;
                for (int i = 0; i < numInDevices; i++)
                {
                    inDevices.Add(MidiInGetName(i));
                }
                return inDevices;
            }
        }

        public Collection<string> OutDevices
        {
            get
            {
                Collection<string> outDevices = new Collection<string>();
                int numOutDevices = DeviceOutCount;
                for (int i = 0; i < numOutDevices; i++)
                {
                    outDevices.Add(MidiOutGetName(i));
                }
                return outDevices;
            }
        }

        public static string MidiInGetName(int index)
        {
            WinMM.MIDIINCAPS caps = new WinMM.MIDIINCAPS();
            int error = WinMM.MidiInGetDevCaps(index, ref caps, 44);

            if (error == 0) return caps.szPname;
            else return "";
        }

        public static string MidiOutGetName(int index)
        {
            WinMM.MIDIOUTCAPS caps = new WinMM.MIDIOUTCAPS();
            int error = WinMM.MidiOutGetDevCaps(index, ref caps, 52);

            if (error == 0) return caps.szPname;
            else return "";
        }
    }
}
