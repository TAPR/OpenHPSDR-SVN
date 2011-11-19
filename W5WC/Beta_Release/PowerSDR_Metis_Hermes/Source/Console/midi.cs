//=================================================================
// midi.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2009  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: sales@flex-radio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    8900 Marybank Dr.
//    Austin, TX 78750
//    USA
//=================================================================

using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using PowerSDR;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for midi.
	/// </summary>
	unsafe public class Midi
	{
		public const int CALLBACK_FUNCTION = 0x00030000;
		public const int MIM_OPEN = 0x3C1;
		public const int MIM_CLOSE = 0x3C2;
		public const int MIM_DATA = 0x3C3;
		public const int MIM_LONGDATA = 0x3C4;

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

			result = Midi.MidiInPrepareHeader(handle, headerPtr, size);
			if(result != 0) return result;

			result = Midi.MidiInAddBuffer(handle, headerPtr, size);
			if(result != 0) return result;
		
			return result;
		}

		public static void ReleaseBuffer(int handle, IntPtr headerPtr)
		{
			int result = MidiInUnprepareHeader(handle, headerPtr, Marshal.SizeOf(typeof(MidiHeader)));
			if(result != 0)
			{
				StringBuilder error_text = new StringBuilder(64);
				Midi.MidiInGetErrorText(result, error_text, 64);
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
