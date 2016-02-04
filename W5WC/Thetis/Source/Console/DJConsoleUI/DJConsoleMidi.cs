//=====================================================================
// DJConsoleMidi.cs
//=====================================================================
// Copyright (C) 2004-2009  FlexRadio Systems
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
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;

namespace Thetis
{
    public partial class DJConsole
    {
        #region Enums

        private enum Command
        {
            NoteOn = 0x90,
            NoteOff = 0x80,
            Aftertouch0 = 0xA0,
            Aftertouch1 = 0xA1,
            Controller = 0xB0,
            PolyPressure = 0xA0,
            ProgramChange = 0xC0,
            ChannelPressure = 0xD0,
            PitchWheel = 0xE0
        }

        private enum Controller
        {
            HoldPedal1 = 64,
            HoldPedal2 = 69,
        }

        private enum Note
        {
            Mox = 0,
            PAADC = 1,
            Dot = 61,
            Dash = 63,
            MicDown = 66,
            MicUp = 68,
            MicFast = 70,
        }

        #endregion

        // private DJConsole djConsole;
        public const int CALLBACK_FUNCTION = 0x30000;
        public const int MIM_OPEN = 0x3C1; //961
        public const int MIM_CLOSE = 0x3C2; //962
        public const int MIM_DATA = 0x3C3; //963
        public const int MIM_LONGDATA = 0x3C4; //964
        public const int MIM_ERROR = 0x3C5;
        public const int MIM_LONGERROR = 0x3C6;
        public const int MIM_MOREDATA = 0x3C7;

        private MidiInCallback callback;
        private bool resetting = false;
        private int midi_in_handle;
        private int midi_out_handle;
        private object in_lock_obj = new Object();
        private object out_lock_obj = new Object();

        public int DeviceInCount
        {
            get
            {
                return MidiInGetNumDevs();
            }
        }

        public int DeviceOutCount
        {
            get
            {
                return MidiOutGetNumDevs();
            }
        }

        public static bool Present()
        {
            int num_in = MidiInGetNumDevs();
            for (int i = 0; i < num_in; i++)
            {

                string s = MidiInGetName(i);
                //if(s == "FLEX 5000" || s == "FLEX-5000 Midi" || s == "FlexRadio Flex-5000" || s == "FlexRadio Flex-5000 MIDI")
                if (s.IndexOf("FLEX") >= 0 || s.IndexOf("Flex") >= 0)
                {
                    goto test_out;
                }
            }

            return false;

        test_out:
            int num_out = MidiOutGetNumDevs();
            for (int i = 0; i < num_out; i++)
            {
                string s = MidiOutGetName(i);
                //if(s == "FLEX 5000" || s == "2- FLEX-5000 Midi" || s == "FlexRadio Flex-5000" || s == "2- FlexRadio Flex-5000 MIDI")
                if (s.IndexOf("FLEX") >= 0 || s.IndexOf("Flex") >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Open()
        {
            FillTables();
            //if(!OpenMidiIn()) return false;
            //if(!OpenMidiOut()) return false;
            return true;
        }

        public bool OpenMidiIn(int deviceID)
        {
            callback = new MidiInCallback(InCallback);
            int result = MidiInOpen(ref midi_in_handle, deviceID, callback, 0, CALLBACK_FUNCTION);
            if (result != 0)
            {
                StringBuilder error_text = new StringBuilder(64);
                MidiInGetErrorText(result, error_text, 64);
                Debug.WriteLine("MidiInOpen Error: " + error_text);
                //MessageBox.Show("Error opening Midi In device");
                return false;
            }

            for (int i = 0; i < 3; i++)
            {
                result = AddSysExBuffer(midi_in_handle);
                if (result != 0)
                {
                    StringBuilder error_text = new StringBuilder(64);
                    MidiInGetErrorText(result, error_text, 64);
                    Debug.WriteLine("AddSysExBuffer Error: " + error_text);
                    //MessageBox.Show("Error adding Midi In device SysEx Buffer");
                    return false;
                }
            }

            result = MidiInStart(midi_in_handle);
            if (result != 0)
            {
                StringBuilder error_text = new StringBuilder(64);
                MidiInGetErrorText(result, error_text, 64);
                Debug.WriteLine("MidiInStart Error: " + error_text);
                //MessageBox.Show("Error starting Midi In device");
                return false;
            }

            return true;
        }

        public bool OpenMidiOut(int deviceID)
        {
            int result = MidiOutOpen(ref midi_out_handle, deviceID, IntPtr.Zero, 0, 0);
            if (result != 0)
            {
                StringBuilder error_text = new StringBuilder(64);
                MidiInGetErrorText(result, error_text, 64);
                Debug.WriteLine("MidiOutOpen Error: " + error_text);
                //MessageBox.Show("Error Opening Midi Out device");
                return false;
            }
            return true;
        }

        private void CloseMidiIn()
        {
            if (midi_in_handle != 0)
            {
                MidiInStop(midi_in_handle);
                resetting = true;
                MidiInReset(midi_in_handle);
                MidiInClose(midi_in_handle);
                midi_in_handle = 0;
                resetting = false;
            }
        }

        private void CloseMidiOut()
        {
            if (midi_out_handle != 0)
            {
                MidiOutClose(midi_out_handle);
            }
        }

        private byte[][] encode_table;
        private byte[][] decode_table;

        private void FillTables()
        {
            encode_table = new byte[256][];
            for (int i = 0; i < 256; i++)
                encode_table[i] = new byte[2];

            for (int i = 0; i < 256; i++)
            {
                byte high_nibble = (byte)(i >> 4); // set 0 byte for high nibble
                if (high_nibble < 0xA) high_nibble += 48; // offset for ascii '0'
                else high_nibble += 55;	// offset for ascii 'A'
                encode_table[i][0] = high_nibble;

                byte low_nibble = (byte)(i & 0xF);
                if (low_nibble < 0xA) low_nibble += 48; // offset for ascii '0'
                else low_nibble += 55; // offset for ascii 'A'
                encode_table[i][1] = low_nibble;
            }


            decode_table = new byte[0x80][];
            for (int i = 0; i < 0x80; i++)
                decode_table[i] = new byte[0x80];

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    if (((i >= 48 && i <= 57) || (i >= 65 && i <= 70)) &&
                        ((j >= 48 && j <= 57) || (j >= 65 && j <= 70)))
                    {
                        byte high_nibble;
                        if (i < 58) high_nibble = (byte)((i - 48) << 4);
                        else high_nibble = (byte)((i - 55) << 4);
                        byte low_nibble;
                        if (j < 58) low_nibble = (byte)(j - 48);
                        else low_nibble = (byte)(j - 55);
                        decode_table[i][j] = (byte)(high_nibble + low_nibble);
                    }
                }
            }
        }

        private int EncodeBytes(byte[] outb, byte[] inb)
        {
            int j = 0;
            for (int i = 0; i < inb.Length; i++)
            {
                outb[j++] = encode_table[inb[i]][0];
                outb[j++] = encode_table[inb[i]][1];
            }
            return j;
        }

        private int DecodeBytes(byte[] outb, byte[] inb)
        {
            int j = 0;
            for (int i = 0; i < inb.Length; i += 2)
            {
                outb[j++] = decode_table[inb[i]][inb[i + 1]];
                //Debug.WriteLine("decode["+inb[i].ToString("X")+"]["+inb[i+1].ToString("X")+"] = "+decode_table[inb[i]][inb[i+1]].ToString("X"));
            }
            return j;
        }

        /*public static byte[] PackBytes(byte[] b1) // leave highest order bit low for MIDI msg
        {
            byte[] b2 = new byte[(int)Math.Ceiling(b1.Length*8/7.0)];
            for(int i=b2.Length-1; i>=0; i--)
            {
                int index1 = (b1.Length-1)-(int)Math.Floor(((b2.Length-1)-i)*7/8.0);
                int index2 = (b1.Length-1)-(int)Math.Ceiling(((b2.Length-1)-i)*7/8.0);
                int shift = (b2.Length-1-i)%8;

                if(shift == 0)
                    b2[i] = (byte)(b1[index1]&0x7F);
                else
                {
                    byte temp1 = (byte)(b1[index1]>>(8-shift));
                    byte temp2;
                    if(index2 < 0) temp2 = 0;
                    else temp2 = (byte)(b1[index2]<<shift);
                    b2[i] = (byte)((temp1 | temp2) & 0x7F);
                }
            }
            DebugByte(b1);
            DebugByte(b2);

            return b2;
        }

        public static byte[] UnpackBytes(byte[] b1)
        {
            byte[] b2 = new byte[(int)Math.Floor(b1.Length*7/8.0)];
            for(int i=b2.Length-1; i>=0; i--)
            {
                int index = (b1.Length-1)-(int)Math.Floor(((b2.Length-1)-i)*8/7.0);
                int shift = (b2.Length-1-i)%7;

                byte temp1 = (byte)(b1[index]>>shift);
                byte temp2 = (byte)(b1[index-1]<<(7-shift));
                b2[i] = (byte)(temp1 | temp2);
            }
            DebugByte(b1);
            DebugByte(b2);
            return b2;
        }*/

        public static void DebugByte(byte[] b)
        {
            for (int i = 0; i < b.Length; i++)
                Debug.Write(b[i].ToString("X") + " ");
            Debug.WriteLine("");
        }


        #region Midi In Callback

        private Hashtable midi_in_table = new Hashtable(10);
        private int InCallback(int hMidiIn, int wMsg, int dwInstance, int dwParam1, int dwParam2)
        {
            lock (in_lock_obj)
            {
                switch (wMsg)
                {
                    case MIM_OPEN:
                        Debug.WriteLine("wMsg=MIM_OPEN");
                        break;
                    case MIM_CLOSE:
                        Debug.WriteLine("wMsg=MIM_CLOSE");
                        break;
                    case MIM_DATA:
                        Command cmd = (Command)((byte)dwParam1);
                        byte byte1 = (byte)(dwParam1 >> 8);
                        byte byte2 = (byte)(dwParam1 >> 16);

                        inDevice_ChannelMessageReceived(byte1, byte2);
                       // Debug.WriteLine("wMsg=MIM_DATA, dwInstance=%08x, dwParam1=%08x, dwParam2=%08x", dwInstance, dwParam1, dwParam2);
                        /*	switch(cmd)
                            {
                                case Command.NoteOn:
                                    switch((Note)byte1)
                                    {
                                        case Note.Dot:
                                            //console.Keyer.FWCDot = true;
                                            //FWC.SetMOX(true);
                                            break;
                                        case Note.Dash:
                                            //console.Keyer.FWCDash = true;
                                            //FWC.SetMOX(true);
                                            break;
                                        case Note.MicDown:
                                            console.MicDown = true;
                                            break;
                                        case Note.MicUp:
                                            console.MicUp = true;
                                            break;
                                        case Note.MicFast:
                                            console.MicFast = !console.MicFast;
                                            break;
                                    }
                                    break;
                                case Command.NoteOff:
                                    switch((Note)byte1)
                                    {
                                        case Note.Dot:
                                            //console.Keyer.FWCDot = false;							
                                            //FWC.SetMOX(false);
                                            break;
                                        case Note.Dash:
                                            //console.Keyer.FWCDash = false;
                                            //FWC.SetMOX(false);
                                            break;
                                        case Note.MicDown:
                                            console.MicDown = false;
                                            break;
                                        case Note.MicUp:
                                            console.MicUp = false;
                                            break;
                                        case Note.MicFast:
                                            break;
                                    }
                                    break;
                                case Command.Controller:
                                    switch((Controller)byte1)
                                    {
                                        case Controller.HoldPedal1:
                                            console.FWCMicPTT = (byte2 > 63);
                                            break;
                                        case Controller.HoldPedal2:
                                            console.FWCRCAPTT = (byte2 > 63);
                                            break;
                                    }
                                    break;
                                case Command.Aftertouch0:
                                case Command.Aftertouch1:
                                    int id = (ushort)((((byte)cmd-(byte)Command.Aftertouch0)<<2)+(byte1>>5));
                                    int data = (int)(((byte1&0x1F)<<7)+byte2);
                                    if(midi_in_table.ContainsKey(id))
                                        midi_in_table.Remove(id);
                                    midi_in_table.Add(id, data);
                                    break;
                            }		*/
                        break;
                    case MIM_LONGDATA:
                        if (!resetting && midi_in_handle != 0) // in case device closes, don't send anymore buffers
                        {
                            int result = AddSysExBuffer(midi_in_handle);
                            if (result != 0)
                            {
                                StringBuilder error_text = new StringBuilder(64);
                                MidiInGetErrorText(result, error_text, 64);
                                Debug.WriteLine("AddSysExBuffer Error: " + error_text);
                            }
                        }

                        IntPtr headerPtr = new IntPtr(dwParam1);
                        MidiHeader header = (MidiHeader)Marshal.PtrToStructure(headerPtr, typeof(MidiHeader));
                        byte[] temp = new byte[header.bytesRecorded];
                        for (int i = 0; i < header.bytesRecorded; i++)
                            temp[i] = Marshal.ReadByte(header.data, i);

                        if (temp.Length > 5)
                        {
                            byte[] temp2 = new byte[temp.Length - 5];
                            for (int i = 0; i < temp.Length - 5; i++)
                                temp2[i] = temp[i + 4];

                            byte[] buf = new byte[temp2.Length / 2];
                            DecodeBytes(buf, temp2);
                            if (midi_in_table.ContainsKey(BitConverter.ToUInt16(buf, 0)))
                                midi_in_table.Remove(BitConverter.ToUInt16(buf, 0));
                            midi_in_table.Add(BitConverter.ToUInt16(buf, 0), buf);
                        }

                        /*for(int i=0; i<header.bytesRecorded; i++)
                            Debug.Write(buf[i].ToString("X")+" ");
                        Debug.WriteLine("");*/

                        if (midi_in_handle != 0)
                            ReleaseBuffer(midi_in_handle, headerPtr);
                        Debug.WriteLine("wMsg=MIM_LONGDATA");
                        break;
                    case MIM_ERROR:
                        Debug.WriteLine("wMsg=MIM_ERROR");
                        break;
                    case MIM_LONGERROR:
                        Debug.WriteLine("wMsg=MIM_LONGERROR");
                        break;
                    case MIM_MOREDATA:
                        Debug.WriteLine("wMsg=MIM_MOREDATA");
                        break;
                }
            }

            return 0;
        }

        #endregion
        public static int AddSysExBuffer(int handle)
		{
			int result;
			IntPtr headerPtr;
			int size = Marshal.SizeOf(typeof(MidiHeader));
			MidiHeader header = new MidiHeader();
			header.bufferLength = 64;
			header.bytesRecorded = 0;
			header.data = Marshal.AllocHGlobal(64);
			header.flags = 0;

			try
			{
				headerPtr = Marshal.AllocHGlobal(size);
			}
			catch(Exception)
			{
				Marshal.FreeHGlobal(header.data);
				throw;
			}

			try
			{
				Marshal.StructureToPtr(header, headerPtr, false);
			}
			catch(Exception)
			{
				Marshal.FreeHGlobal(header.data);
				Marshal.FreeHGlobal(headerPtr);

				throw;
			}

			result = MidiInPrepareHeader(handle, headerPtr, size);
			if(result != 0) return result;

			result = MidiInAddBuffer(handle, headerPtr, size);
			if(result != 0) return result;
		
			return result;
		}

		public static void ReleaseBuffer(int handle, IntPtr headerPtr)
		{
			int result = MidiInUnprepareHeader(handle, headerPtr, Marshal.SizeOf(typeof(MidiHeader)));
			if(result != 0)
			{
				StringBuilder error_text = new StringBuilder(64);
				MidiInGetErrorText(result, error_text, 64);
				Debug.WriteLine("MidiInUnprepareHeader Error: "+error_text);
				return;
			}

			MidiHeader header = (MidiHeader)Marshal.PtrToStructure(headerPtr, typeof(MidiHeader));

			Marshal.FreeHGlobal(header.data);
			Marshal.FreeHGlobal(headerPtr);
		}
		
		private static byte[] SwapBytes(byte[] b)
		{
			byte temp;
			for(int i=0; i<b.Length/2; i++)
			{
				temp = b[i];
				b[i] = b[b.Length-1-i];
				b[b.Length-1-i] = temp;
			}
			return b;
		}

		public static int SendMsg(int handle, ushort msg_id, byte protocol_id, ushort opcode, uint data1, uint data2)
		{
			byte[] bytes = new byte[16];
			bytes[0] = 0xF0;
			bytes[1] = 0x7D;
			SwapBytes(BitConverter.GetBytes(msg_id)).CopyTo(bytes, 2);
			bytes[4] = protocol_id;
			SwapBytes(BitConverter.GetBytes(opcode)).CopyTo(bytes, 5);
			SwapBytes(BitConverter.GetBytes(data1)).CopyTo(bytes, 7);
			SwapBytes(BitConverter.GetBytes(data2)).CopyTo(bytes, 11);
			bytes[15] = 0xF7;

			return SendLongMessage(handle, bytes);
		}

		public static string MidiInGetName(int index)
		{
			MIDIINCAPS caps = new MIDIINCAPS();
			int error = MidiInGetDevCaps(index, ref caps, 44);

			if(error == 0) return caps.szPname;
			else return "";
		}

		public static string MidiOutGetName(int index)
		{
			MIDIOUTCAPS caps = new MIDIOUTCAPS();
			int error = MidiOutGetDevCaps(index, ref caps, 52);
			
			if(error == 0) return caps.szPname;
			else return "";
		}

		/*private static HiPerfTimer t1 = new HiPerfTimer();
		private static double timer1_sum = 0.0;
		private static int timer1_count = 0;*/
		public static int SendLongMessage(int handle, byte[] data)
		{
			/*Debug.Write("Midi Out: ");
			for(int i=0; i<data.Length; i++)
				Debug.Write(data[i].ToString("X")+" ");
			Debug.WriteLine("");*/

			int result;
			IntPtr ptr;
			int size = Marshal.SizeOf(typeof(MidiHeader));
			MidiHeader header = new MidiHeader();
			header.data = Marshal.AllocHGlobal(data.Length);
			for(int i=0; i<data.Length; i++)
				Marshal.WriteByte(header.data, i, data[i]);
			header.bufferLength = data.Length;
			header.bytesRecorded = data.Length;
			header.flags = 0;

			try
			{
				ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MidiHeader)));
			}
			catch(Exception)
			{
				Marshal.FreeHGlobal(header.data);
				throw;
			}

			try
			{
				Marshal.StructureToPtr(header, ptr, false);
			}
			catch(Exception)
			{
				Marshal.FreeHGlobal(header.data);
				Marshal.FreeHGlobal(ptr);
				throw;
			}
			
			result = MidiOutPrepareHeader(handle, ptr, size);
			if(result == 0) result = MidiOutLongMessage(handle, ptr, size);
			if(result == 0) result = MidiOutUnprepareHeader(handle, ptr, size);

			Marshal.FreeHGlobal(header.data);
			Marshal.FreeHGlobal(ptr);

			return result;
		}

		#region Misc Declarations

		public const int MAXPNAMELEN = 32;

		[StructLayout(LayoutKind.Sequential)]
		public struct MidiHeader
		{			
			public IntPtr	data; 
			public int		bufferLength; 
			public int		bytesRecorded; 
			public int		user; 
			public int		flags; 
			public IntPtr	lpNext; 
			public int		reserved; 
			public int		offset;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
			public int[]	dwReserved;
		}

		#endregion

		#region MidiIn Declarations

		[DllImport("winmm.dll", EntryPoint="midiInGetNumDevs", CharSet=CharSet.Ansi)]
		public static extern int MidiInGetNumDevs();

		[DllImport("winmm.dll", EntryPoint="midiInGetDevCaps", CharSet=CharSet.Ansi)]
		public static extern int MidiInGetDevCaps(int uDeviceID, ref MIDIINCAPS caps, int cbMidiInCaps);

		[DllImport("winmm.dll", EntryPoint="midiInOpen", CharSet=CharSet.Ansi)]
		public static extern int MidiInOpen(ref int lphMidiIn, int uDeviceID, MidiInCallback dwCallback, int dwInstance, int dwFlags);

		[DllImport("winmm.dll", EntryPoint="midiInClose", CharSet=CharSet.Ansi)]
		public static extern int MidiInClose(int hMidiIn);

		[DllImport("winmm.dll", EntryPoint="midiInReset", CharSet=CharSet.Ansi)]
		public static extern int MidiInReset(int hMidiIn);

		[DllImport("winmm.dll", EntryPoint="midiInStart", CharSet=CharSet.Ansi)]
		public static extern int MidiInStart(int hMidiIn);

		[DllImport("winmm.dll", EntryPoint="midiInStop", CharSet=CharSet.Ansi)]
		public static extern int MidiInStop(int hMidiIn);

		[DllImport("winmm.dll", EntryPoint="midiInAddBuffer")]
		public static extern int MidiInAddBuffer(int hMidiIn, IntPtr headerPtr, int cbMidiInHdr);

		[DllImport("winmm.dll", EntryPoint="midiInPrepareHeader")]
		public static extern int MidiInPrepareHeader(int hMidiIn, IntPtr headerPtr, int cbMidiInHdr);

		[DllImport("winmm.dll", EntryPoint="midiInUnprepareHeader")]
		public static extern int MidiInUnprepareHeader(int hMidiIn, IntPtr headerPtr, int cbMidiInHdr);

		[DllImport("winmm.dll", EntryPoint="midiInGetErrorText")]
		public static extern int MidiInGetErrorText(int wError, StringBuilder lpText, int cchText);

		unsafe public delegate int MidiInCallback(int hMidiIn, int wMsg, int dwInstance, int dwParam1, int dwParam2); 

		public struct MIDIINCAPS
		{ 
			public short wMid; 
			public short wPid; 
			public int vDriverVersion; 
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAXPNAMELEN)]
			public string szPname; 
			public int dwSupport; 
		} 

		#endregion

		#region MidiOut Declarations

		[DllImport("winmm.dll", EntryPoint="midiOutGetNumDevs")]
		public static extern int MidiOutGetNumDevs();

		[DllImport("winmm.dll", EntryPoint="midiOutGetDevCaps")]
		public static extern int MidiOutGetDevCaps(int uDeviceID, ref MIDIOUTCAPS caps, int cbMidiOutCaps);

		[DllImport("winmm.dll", EntryPoint="midiOutOpen")]
		public static extern int MidiOutOpen(ref int lphMidiOut, int uDeviceID, IntPtr dwCallback, int dwInstance, int dwFlags);

		[DllImport("winmm.dll", EntryPoint="midiOutClose")]
		public static extern int MidiOutClose(int hMidiOut);

		[DllImport("winmm.dll", EntryPoint="midiOutShortMsg")]
		public static extern int MidiOutShortMessage(int hMidiOut, int dwMsg);

		[DllImport("winmm.dll", EntryPoint="midiOutLongMsg")]
		public static extern int MidiOutLongMessage(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

		[DllImport("winmm.dll", EntryPoint="midiOutPrepareHeader")]
		public static extern int MidiOutPrepareHeader(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

		[DllImport("winmm.dll", EntryPoint="midiOutUnprepareHeader")]
		public static extern int MidiOutUnprepareHeader(int handle, IntPtr headerPtr, int sizeOfMidiHeader);

		public struct MIDIOUTCAPS
		{ 
			public short wMid; 
			public short wPid; 
			public int vDriverVersion; 
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=MAXPNAMELEN)]
			public string szPname; 
			public short wTechnology; 
			public short wVoices; 
			public short wNotes; 
			public short wChannelMask; 
			public int dwSupport; 
		}

		#endregion

    }
}
