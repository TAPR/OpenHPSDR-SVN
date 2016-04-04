﻿/*  Midi2Cat

Description: A subsystem that facilitates mapping Windows MIDI devices to CAT commands.
 
Copyright (C) 2016 Andrew Mansfield, M0YGG

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at:  midi2cat@cametrix.com

*/

using Midi2Cat.Data;
using Midi2Cat.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Midi2Cat.IO
{
    public delegate void MidiInputEventHandler(MidiDevice Device, int DeviceIdx, int ControlId, int Data, int Status, int Event, int Channel);
    public delegate void DebugMsgEventHandler(int Device, Direction direction, Status deviceStatus, string msg1, string msg2);

    public class ParsedMidiMessage
    {
        public int Event;
        public int Channel;
        public int Data1;
        public int Data2;
        public bool Valid = false;
        public string ErrMsg = null;
    }

    public class MidiDevice
    {
        public event MidiInputEventHandler onMidiInput;
        public event DebugMsgEventHandler onMidiDebugMessage;

        private WinMM.MidiInCallback callback;
        private int midi_in_handle=0;
        private int midi_out_handle=0;
        private object in_lock_obj = new Object();
        private object out_lock_obj = new Object();
        
        public const int CALLBACK_FUNCTION = 0x30000;
        public const int MIM_OPEN = 0x3C1; //961
        public const int MIM_CLOSE = 0x3C2; //962
        public const int MIM_DATA = 0x3C3; //963
        public const int MIM_LONGDATA = 0x3C4; //964
        public const int MIM_ERROR = 0x3C5;
        public const int MIM_LONGERROR = 0x3C6;
        public const int MIM_MOREDATA = 0x3C7;

        #region Midi Setup
        int DeviceIndex = 0;
        string DeviceName;

        public bool OpenMidiIn(int deviceIndex,string deviceName)
        {
            DeviceIndex = deviceIndex;
            this.DeviceName = deviceName;
            callback = new WinMM.MidiInCallback(InCallback);
            int result = WinMM.MidiInOpen(ref midi_in_handle, DeviceIndex, callback, 0, CALLBACK_FUNCTION);
            if (result != 0)
            {
                StringBuilder error_text = new StringBuilder(256);
                WinMM.MidiInGetErrorText(result, error_text, 256);
                DebugMsg(Direction.In, Status.Error, "MidiInOpen Error: ", error_text.ToString());
                return false;
            }
            /*
            for (int i = 0; i < 3; i++)
            {
                result = AddSysExBuffer(midi_in_handle);
                if (result != 0)
                {
                    StringBuilder error_text = new StringBuilder(256);
                    WinMM.MidiInGetErrorText(result, error_text, 256);
                    DebugMsg(Direction.In, Status.Error, "AddSysExBuffer Error:", error_text.ToString());
                    return false;
                }
            }
            */
            result = WinMM.MidiInStart(midi_in_handle);
            if (result != 0)
            {
                StringBuilder error_text = new StringBuilder(256);
                WinMM.MidiInGetErrorText(result, error_text, 256);
                DebugMsg(Direction.In, Status.Error, "MidiInStart Error:", error_text.ToString());
                return false;
            }
            // Try and open Midi Out and send a reset.
            if (OpenMidiOut())
            {
                SendMsg(0x0F, 0x0F, 0, 0);
            }


            DebugMsg(Direction.In, Status.Open);
            return true;
        }

        public bool OpenMidiOut()
        {
            int outDeviceID = -1;
            MidiDevices devices = new MidiDevices();
            int Idx = 0;
            foreach (string devName in devices.OutDevices)
            {
                if (devName == this.DeviceName)
                {
                    outDeviceID = Idx;
                    break;
                }
                Idx++;
            }
            if (outDeviceID == -1)
            {
                DebugMsg(Direction.Out, Status.Error, "MidiOutOpen Error: Unable to find "+DeviceName+" output device.");
                return false;
            }
            else
            {
                int result = WinMM.MidiOutOpen(ref midi_out_handle, outDeviceID, IntPtr.Zero, 0, 0);
                if (result != 0)
                {
                    StringBuilder error_text = new StringBuilder(256);
                    WinMM.MidiInGetErrorText(result, error_text, 256);
                    DebugMsg(Direction.Out, Status.Error, "MidiOutOpen Error:", error_text.ToString());
                    return false;
                }
                DebugMsg(Direction.Out, Status.Open);
                return true;
            }
        }

        public void CloseMidiIn()
        {
            if (midi_in_handle != 0)
            {
                WinMM.MidiInStop(midi_in_handle);
                //resetting = true;
                WinMM.MidiInReset(midi_in_handle);
                WinMM.MidiInClose(midi_in_handle);
                midi_in_handle = 0;
                //resetting = false;
                DebugMsg(Direction.Out, Status.Closed);
            }
        }

        public void CloseMidiOut()
        {
            if (midi_out_handle != 0)
            {
                WinMM.MidiOutClose(midi_out_handle);
                DebugMsg(Direction.Out, Status.Closed);
            }
        }

        private void Reset()
        {
            CloseMidiIn();
            CloseMidiOut();
        }

        /*
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
        */
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

        //private Hashtable midi_in_table = new Hashtable(10);
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
                        byte controlId = (byte)(dwParam1 >> 8);
                        byte data = (byte)(dwParam1 >> 16);
                        byte status = (byte)(dwParam1 & 0xFF);
                        byte Event = (byte)((status & 0xF0)>>4);
                        byte channel = (byte)(status & 0x0F);

                        // if Note off make sure the data is set to 0, its most likely set to 7F;
                        if (Event == (int)MidiEvent.Note_Off)
                            data = 0x00;

                       // Debug.WriteLine("wMsg=MIM_DATA, dwInstance={0}, dwParam1={1}, dwParam2={2}", dwInstance, dwParam1.ToString("X8"), dwParam2.ToString("X8"));
                        Debug.WriteLine("ControlId={0}, byte2={1}, status={2}, Event={3}, channel={4}", controlId.ToString("X2"), data.ToString("X2"), status.ToString("X2"), Event.ToString("X2"), channel.ToString("X2"));

                        inDevice_ChannelMessageReceived(controlId, data, status, Event, channel);
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

                        //if (!resetting && midi_in_handle != 0) // in case device closes, don't send anymore buffers
                        //{
                        //    int result = AddSysExBuffer(midi_in_handle);
                        //    if (result != 0)
                        //    {
                        //        StringBuilder error_text = new StringBuilder(64);
                        //        WinMM.MidiInGetErrorText(result, error_text, 64);
                        //        Debug.WriteLine("AddSysExBuffer Error: " + error_text);
                        //    }
                        //}

                        //IntPtr headerPtr = new IntPtr(dwParam1);
                        //WinMM.MidiHeader header = (WinMM.MidiHeader)Marshal.PtrToStructure(headerPtr, typeof(WinMM.MidiHeader));
                        //byte[] temp = new byte[header.bytesRecorded];
                        //for (int i = 0; i < header.bytesRecorded; i++)
                        //    temp[i] = Marshal.ReadByte(header.data, i);

                        //if (temp.Length > 5)
                        //{
                        //    byte[] temp2 = new byte[temp.Length - 5];
                        //    for (int i = 0; i < temp.Length - 5; i++)
                        //        temp2[i] = temp[i + 4];

                        //    byte[] buf = new byte[temp2.Length / 2];
                        //    DecodeBytes(buf, temp2);
                        //    if (midi_in_table.ContainsKey(BitConverter.ToUInt16(buf, 0)))
                        //        midi_in_table.Remove(BitConverter.ToUInt16(buf, 0));
                        //    midi_in_table.Add(BitConverter.ToUInt16(buf, 0), buf);
                        //}

                        ///*for(int i=0; i<header.bytesRecorded; i++)
                        //    Debug.Write(buf[i].ToString("X")+" ");
                        //Debug.WriteLine("");*/

                        //if (midi_in_handle != 0)
                        //    ReleaseBuffer(midi_in_handle, headerPtr);

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
        /*
        public static int AddSysExBuffer(int handle)
        {
            int result;
            IntPtr headerPtr;
            int size = Marshal.SizeOf(typeof(WinMM.MidiHeader));
            WinMM.MidiHeader header = new WinMM.MidiHeader();
            header.bufferLength = 64;
            header.bytesRecorded = 0;
            header.data = Marshal.AllocHGlobal(64);
            header.flags = 0;

            try
            {
                headerPtr = Marshal.AllocHGlobal(size);
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(header.data);
                throw;
            }

            try
            {
                Marshal.StructureToPtr(header, headerPtr, false);
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(header.data);
                Marshal.FreeHGlobal(headerPtr);

                throw;
            }

            result = WinMM.MidiInPrepareHeader(handle, headerPtr, size);
            if (result != 0) return result;

            result = WinMM.MidiInAddBuffer(handle, headerPtr, size);
            if (result != 0) return result;

            return result;
        }

        public static void ReleaseBuffer(int handle, IntPtr headerPtr)
        {
            int result = WinMM.MidiInUnprepareHeader(handle, headerPtr, Marshal.SizeOf(typeof(WinMM.MidiHeader)));
            if (result != 0)
            {
                StringBuilder error_text = new StringBuilder(64);
                WinMM.MidiInGetErrorText(result, error_text, 64);
                Debug.WriteLine("MidiInUnprepareHeader Error: " + error_text);
                return;
            }

            WinMM.MidiHeader header = (WinMM.MidiHeader)Marshal.PtrToStructure(headerPtr, typeof(WinMM.MidiHeader));

            Marshal.FreeHGlobal(header.data);
            Marshal.FreeHGlobal(headerPtr);
        }
        */
        private static byte[] SwapBytes(byte[] b)
        {
            byte temp;
            for (int i = 0; i < b.Length / 2; i++)
            {
                temp = b[i];
                b[i] = b[b.Length - 1 - i];
                b[b.Length - 1 - i] = temp;
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
        
        public static int SendLongMessage(int handle, byte[] data)
        {
            /*Debug.Write("Midi Out: ");
            for(int i=0; i<data.Length; i++)
                Debug.Write(data[i].ToString("X")+" ");
            Debug.WriteLine("");*/

            int result;
            IntPtr ptr;
            int size = Marshal.SizeOf(typeof(WinMM.MidiHeader));
            WinMM.MidiHeader header = new WinMM.MidiHeader();
            header.data = Marshal.AllocHGlobal(data.Length);
            for (int i = 0; i < data.Length; i++)
                Marshal.WriteByte(header.data, i, data[i]);
            header.bufferLength = data.Length;
            header.bytesRecorded = data.Length;
            header.flags = 0;

            try
            {
                ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinMM.MidiHeader)));
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(header.data);
                throw;
            }

            try
            {
                Marshal.StructureToPtr(header, ptr, false);
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(header.data);
                Marshal.FreeHGlobal(ptr);
                throw;
            }

            result = WinMM.MidiOutPrepareHeader(handle, ptr, size);
            if (result == 0) result = WinMM.MidiOutLongMessage(handle, ptr, size);
            if (result == 0) result = WinMM.MidiOutUnprepareHeader(handle, ptr, size);

            Marshal.FreeHGlobal(header.data);
            Marshal.FreeHGlobal(ptr);

            return result;
        }
        #endregion

        public void inDevice_ChannelMessageReceived(int ControlId, int Data, int Status, int Event, int Channel)
        {
            if (onMidiInput != null)
            {
                try
                {
                    onMidiInput(this, DeviceIndex, ControlId, Data, Status, Event, Channel);
                }
                catch { }
            }
        }

        private void DebugMsg(Direction direction, Status status, string msg1 = "", string msg2 = "")
        {
            if (this.onMidiDebugMessage != null)
            {
                try
                {
                    onMidiDebugMessage(DeviceIndex, direction, status, msg1, msg2);
                }
                catch { }
            }
        }

        public void SendMsg(int Event, int Channel,int Data1, int Data2 )
        {
            if (midi_out_handle == 0)
            {
                OpenMidiOut();            
    
            }
            byte status = (byte)(Event << 4);
            status |= (byte)Channel;
            byte[] bytes = new byte[4];
            bytes[0] = status;
            bytes[1] = (byte)Data1;
            bytes[2] = (byte)Data2;
            bytes[3] = 0;
            uint msg = BitConverter.ToUInt32(bytes,0);
            if (midi_out_handle != 0)
            {
                int Rc = WinMM.MidiOutShortMessage(midi_out_handle, msg);
            }
        }

        public ParsedMidiMessage ParseMsg(int inChannel, int inValue, int inStatus, int inControl, string inMsg)
        {
            string msg = inMsg;
            ParsedMidiMessage Rc = new ParsedMidiMessage();
            if (msg.Length != 6)
            {
                Rc.ErrMsg = string.Format("Msg:{0} {1}", inMsg, "Must be 6 characters long.");
                return Rc;
            }
            if (msg.Contains("SS"))
            {
                if (msg.Substring(0, 2) != "SS")
                {
                    Rc.ErrMsg = string.Format("Msg:{0} {1}", inMsg, "SS must be the 1st and 2nd characters.");
                    return Rc;
                }
                 msg = msg.Replace("SS", inStatus.ToString("X2"));
            }
            if (msg.Contains("YY"))
            {
                if (msg.Substring(2, 2) != "YY")
                {
                    Rc.ErrMsg = string.Format("Msg:{0} {1}", inMsg, "YY must be the 3rd & 4th characters.");
                    return Rc;
                }
                msg = msg.Replace("YY", inControl.ToString("X2"));
            }
            if (msg.Contains("VV"))
            {
                if (msg.Substring(4, 2) != "VV")
                {
                    Rc.ErrMsg = string.Format("Msg:{0} {1}", inMsg, "VV must be the 5th & 6th characters.");
                    return Rc;
                }
                msg = msg.Replace("VV", inValue.ToString("X2"));
            }
            msg = msg.Replace("X", inChannel.ToString());
            string sEvent = msg.Substring(0, 1);
            string sChannel = msg.Substring(1, 1);
            string sData1 = msg.Substring(2, 2);
            string sData2 = msg.Substring(4, 2);
            try
            {
                Rc.Event = Int32.Parse(sEvent, System.Globalization.NumberStyles.HexNumber);
                Rc.Channel = Int32.Parse(sChannel, System.Globalization.NumberStyles.HexNumber);
                Rc.Data1 = Int32.Parse(sData1, System.Globalization.NumberStyles.HexNumber);
                Rc.Data2 = Int32.Parse(sData2, System.Globalization.NumberStyles.HexNumber);
                Rc.Valid = true;
            }
            catch 
            {
                Rc.ErrMsg = string.Format("Msg:{0} {1}", inMsg, "contains invalid hexideciaml characters.");
            }
            return Rc;
        }

        public void SendMsg(int inChannel, int inValue, int inStatus, int inControl, string inMessages)
        {
            char[] parms= {' ', ',', ';', ':'};
            string[] messages = inMessages.Split(parms, StringSplitOptions.RemoveEmptyEntries);
            foreach (string msg in messages)
            {
                ParsedMidiMessage parsedMsg = ParseMsg(inChannel, inValue, inStatus, inControl, msg);
                if (parsedMsg.Valid)
                {
                    SendMsg(parsedMsg.Event, parsedMsg.Channel, parsedMsg.Data1, parsedMsg.Data2);
                }
            }
        }

        public string[] ValidateMidiMessages(string inMessages)
        {
            List<string> Errors=new List<string>();
            char[] parms= {' ', ',', ';', ':'};
            string[] messages = inMessages.Split(parms, StringSplitOptions.RemoveEmptyEntries);
            foreach (string msg in messages)
            {
                ParsedMidiMessage parsedMsg = ParseMsg(0, 0, 0, 0, msg);
                if (parsedMsg.Valid == false)
                {
                    Errors.Add(parsedMsg.ErrMsg);
                }
            }
            return Errors.ToArray();
        }
    }
}
