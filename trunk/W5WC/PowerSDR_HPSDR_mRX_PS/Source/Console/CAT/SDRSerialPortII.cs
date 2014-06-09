//=================================================================
// SDRSerialPort.cs
//=================================================================
// Copyright (C) 2005  Bill Tracey
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
//================================================================= 
// Serial port support for PowerSDR support of CAT and serial port control  
//=================================================================

#define DBG_PRINT

using System;
using System.Threading;
using System.IO.Ports;

namespace PowerSDR
{
    public class SDRSerialPort
    {
        public static event SerialRXEventHandler serial_rx_event;

        private SerialPort commPort;
        public SerialPort BasePort
        {
            get { return commPort; }
        }

        private bool isOpen = false;
        private bool bitBangOnly = false;

        //Added 2/14/2008 BT
        public bool IsOpen
        {
            get { return commPort.IsOpen; }
        }

        public void Open()
        {
            commPort.Open();
        }

        public void Close()
        {
            commPort.Close();
        }

        public static Parity StringToParity(string s)
        {
            if (s == "none") return Parity.None;
            if (s == "odd") return Parity.Odd;
            if (s == "even") return Parity.Even;
            if (s == "space") return Parity.Space;
            if (s == "mark") return Parity.Mark;
            return Parity.None;  // error -- default to none
        }

        public static StopBits StringToStopBits(string s)
        {
            if (s == "0") return StopBits.None;
            if (s == "1") return StopBits.One;
            if (s == "1.5") return StopBits.OnePointFive;
            if (s == "2") return StopBits.Two;
            return StopBits.One; // error -- default 
        }

        public SDRSerialPort(int portidx)
        {
            commPort = new SerialPort();
            commPort.Encoding = System.Text.Encoding.ASCII;
            commPort.RtsEnable = true; // hack for soft rock ptt 
            commPort.DtrEnable = true; // set dtr off 
            //commPort.ErrorReceived += new SerialErrorReceivedEventHandler(this.SerialErrorReceived);
            commPort.DataReceived += new SerialDataReceivedEventHandler(this.SerialReceivedData);
            commPort.PinChanged += new SerialPinChangedEventHandler(this.SerialPinChanged);

            commPort.PortName = "COM" + portidx.ToString();

            commPort.Parity = Parity.None;
            commPort.StopBits = StopBits.One;
            commPort.DataBits = 8;
            commPort.BaudRate = 9600;
            commPort.ReadTimeout = 5000;
            commPort.WriteTimeout = 500;
            commPort.ReceivedBytesThreshold = 1;
        }
        // set the comm parms ... can only be done if port is not open -- silently fails if port is open (fixme -- add some error checking) 
        // 
        public void setCommParms(int baudrate, Parity p, int databits, StopBits stop)
        {
            if (commPort.IsOpen) return; // bail out if it's already open 

            commPort.BaudRate = baudrate;
            commPort.Parity = p;
            commPort.StopBits = stop;
            commPort.DataBits = databits;
        }

        public uint put(string s)
        {
            if (bitBangOnly) return 0;  // fixme -- throw exception?			
            commPort.Write(s);
            return (uint)s.Length; // wjt fixme -- hack -- we don't know if we actually wrote things 			
        }

        public int Create()
        {
            return Create(false);
        }

        // create port 
        public int Create(bool bit_bang_only)
        {
            bitBangOnly = bit_bang_only;
            if (isOpen) { return -1; }
            commPort.Open();
            isOpen = commPort.IsOpen;
            if (isOpen)
                return 0; // all is well
            else
                return -1;  //error
        }

        public void Destroy()
        {
            try
            {
                commPort.Close();
            }
            catch (Exception)
            {

            }
            isOpen = false;
        }

        public bool isCTS()
        {
            if (!isOpen) return false; // fixme error check 
            return commPort.CtsHolding;
        }

        public bool isDSR()
        {
            if (!isOpen) return false; // fixme error check 
            return commPort.DsrHolding;

        }
        public bool isRI()
        {
            if (!isOpen) return false; // fixme error check 
            return false;
        }

        public bool isRLSD()
        {
            if (!isOpen) return false; // fixme error check 
            return commPort.CDHolding;
        }

        public void setDTR(bool v)
        {
            if (!isOpen) return;
            commPort.DtrEnable = v;
        }

        void SerialErrorReceived(object source, SerialErrorReceivedEventArgs e)
        {

        }

        private bool use_for_keyptt = false;
        public bool UseForKeyPTT
        {
            get { return use_for_keyptt; }
            set { use_for_keyptt = value; }
        }

        private bool use_for_paddles = false;
        public bool UseForPaddles
        {
            get { return use_for_paddles; }
            set { use_for_paddles = value; }
        }

        private bool ptt_on_dtr = false;
        public bool PTTOnDTR
        {
            get { return ptt_on_dtr; }
            set { ptt_on_dtr = value; }
        }

        private bool ptt_on_rts = false;
        public bool PTTOnRTS
        {
            get { return ptt_on_rts; }
            set { ptt_on_rts = value; }
        }

        private bool key_on_dtr = false;
        public bool KeyOnDTR
        {
            get { return key_on_dtr; }
            set { key_on_dtr = value; }
        }

        private bool key_on_rts = false;
        public bool KeyOnRTS
        {
            get { return key_on_rts; }
            set { key_on_rts = value; }
        }

        void SerialPinChanged(object source, SerialPinChangedEventArgs e)
        {
            if (!use_for_keyptt && !use_for_paddles) return;

            if (use_for_keyptt)
            {
                switch (e.EventType)
                {
                    case SerialPinChange.DsrChanged:

                        if (ptt_on_dtr)
                        {
                            CWInput.KeyerPTT = commPort.DsrHolding;
                        }

                        if (key_on_dtr)
                        {
                            JanusAudio.SetCWX(Convert.ToInt32(commPort.DsrHolding));
                        }
                        break;
                    case SerialPinChange.CtsChanged:

                        if (ptt_on_rts)
                        {
                            CWInput.KeyerPTT = commPort.CtsHolding;
                        }

                        if (key_on_rts)
                        {
                            JanusAudio.SetCWX(Convert.ToInt32(commPort.CtsHolding));
                        }
                        break;
                }
            }
            else if (use_for_paddles)
            {
                switch (e.EventType)
                {
                    case SerialPinChange.DsrChanged:
                        JanusAudio.SetCWDot(Convert.ToInt32(commPort.DsrHolding));
                        break;
                    case SerialPinChange.CtsChanged:
                        JanusAudio.SetCWDash(Convert.ToInt32(commPort.CtsHolding));
                        break;
                }
            }
        }

        void SerialReceivedData(object source, SerialDataReceivedEventArgs e)
        {
            serial_rx_event(this, new SerialRXEvent(commPort.ReadExisting()));
        }

    }
}
