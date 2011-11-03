//=================================================================
// cwx.cs
//=================================================================
// CWX - new version of the old keyer memory and keyboard stuff
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
//
//         CWX written by Richard Allen, W5SXD
//    with various hooks from Eric Wachsman, KE5DTO, 
//          and Herr Doktor Bob McGwier, N4HY
//            November 2005 - February 2006
//
//=================================================================

#define SAVERESTORE
//#define CWX_DEBUG (Note: Please do not put all Debug.Writeline()under this. Leave them commented off.)

namespace PowerSDR
{
    using System;
    using System.IO;
    using System.Drawing;
    using System.Diagnostics;
    using System.Collections;
    using System.Threading;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

	/// <summary>
	///  CWX is the cw memory and keyboard handler.
	/// </summary>
	public class CWX : System.Windows.Forms.Form
	{
		#region define element codes and constants
		private const byte EL_UNDERFLOW = 0x80;	// underflow flag
		private const byte EL_PTT = 0x10;		// extra ptt delay command
		private const byte EL_PAUSE = 0x08;		// pause flag
		private const byte EL_END = 0x4;		// end flag
		private const byte EL_KEYUP = 0x2;		// key up flag
		private const byte EL_KEYDOWN = 0x3;	// key down

		private const int NKEYS = 120;			// width of the keyboard buffers
		private const int NKPL = 60;			// # keys per line

		private const char EMPTY_CODE = '_';	// empty display loc
		#endregion

		#region Variable Declarations
		private System.ComponentModel.IContainer components;

		private Console console;



		public static Mutex keydisplay = new Mutex();	// around the key display

		private int cwxwpm;						// engine speed
		private uint[] mbits = new uint[64];	// the Morse element maps
		private string[] a2m2 = new string[64]; // in ASCII order from 32-95

		private bool quit, kquit;	// shutdown flags
		private int pause;			// # cycles left to pause

		public static Mutex cwfifo2 = new Mutex();	// around the key fifo
		private byte[] fifo2 = new byte[32768];		// the key fifo
		private int infifo2;		// # entries in the fifo
		private int pin2;			// fifo input pointer
		private int pout2;			// fifo output pointer

		public static Mutex cwfifo = new Mutex();	// around the element fifo
		private byte[] elfifo = new byte[32768];	// the code element fifo
		private int infifo;			// # entries in the fifo
		private int pin;			// fifo input pointer
		private int pout;			// fifo output pointer

		private int tel;			// time of one element in ms
		private int ttx;			// # cycles left 'til ptt drops
		private int ttdel;			// tx timeout; keep ptt up this long after key up
		private int tpause;			// pause time in ms
		private string tqq;			// string currently being sent
		private bool altkey;		// true if alt key is pressed

		private int kkk;			// a counter for key up event handler
		private bool keying;		// key button keying radio
		private bool ptt;			// ptt is active
		private int newptt;			// have a new setting of ptt active timing it down
		private int pttdelay;		// delay from PTT to key down
		private bool pause_checked;	// pause chekbox is checked
		private bool stopThreads;	// tell threads to shut down
		
		// define the position and size of the keyboard area
		private int kylx = 12, kyty = 180;				// ulc of key area
		private int kyysz = 82, kyxsz = 665;			// extents
		private char[] kbufold = new char[NKEYS];		// sent keys
		private char[] kbufnew = new char[NKEYS];		// unsent keys
	

		private System.Windows.Forms.LabelTS label4;
		private System.Windows.Forms.ButtonTS stopButton;
		private System.Windows.Forms.LabelTS label5;
		private System.Windows.Forms.LabelTS label6;								// pulling queue pointer
		private System.Windows.Forms.ButtonTS s1;
		private System.Windows.Forms.TextBoxTS txt1;
		private System.Windows.Forms.ButtonTS s2;
		private System.Windows.Forms.TextBoxTS txt2;
		private System.Windows.Forms.ButtonTS s3;
		private System.Windows.Forms.TextBoxTS txt3;
		private System.Windows.Forms.ButtonTS s4;
		private System.Windows.Forms.TextBoxTS txt4;
		private System.Windows.Forms.ButtonTS s5;
		private System.Windows.Forms.TextBoxTS txt5;
		private System.Windows.Forms.ButtonTS s6;
		private System.Windows.Forms.TextBoxTS txt6;
		private System.Windows.Forms.LabelTS speedLabel;
		private System.Windows.Forms.ButtonTS notesButton;
		private System.Windows.Forms.ComboBoxTS cbMorse;
		private System.Windows.Forms.NumericUpDownTS udDelay;
		private System.Windows.Forms.LabelTS repeatdelayLabel;
		private System.Windows.Forms.TextBoxTS txt7;
		private System.Windows.Forms.ButtonTS s7;
		private System.Windows.Forms.ButtonTS s8;
		private System.Windows.Forms.TextBoxTS txt8;
		private System.Windows.Forms.ButtonTS s9;
		private System.Windows.Forms.TextBoxTS txt9;
		private System.Windows.Forms.LabelTS dropdelaylabel;
		private System.Windows.Forms.NumericUpDownTS udDrop;
		private System.Windows.Forms.ButtonTS keyButton;
		private System.Windows.Forms.LabelTS label7;
		private System.Windows.Forms.TextBoxTS txtdummy1;
		private System.Windows.Forms.CheckBoxTS chkPause;
		private System.Windows.Forms.ButtonTS clearButton;
		private System.Windows.Forms.ButtonTS keyboardButton;
		private System.Windows.Forms.Panel pttLed;
		private System.Windows.Forms.Panel keyLed;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Panel keyboardLed;
		private System.Windows.Forms.ButtonTS expandButton;
		private System.Windows.Forms.NumericUpDownTS udPtt;
		private System.Windows.Forms.LabelTS pttdelaylabel;
		private System.Windows.Forms.CheckBoxTS chkAlwaysOnTop;
		private System.Windows.Forms.NumericUpDownTS udWPM;	
		private ASCIIEncoding AE = new ASCIIEncoding();

		#endregion

		#region Win32 Multimedia Timer Functions

		// Represents the method that is called by Windows when a timer event occurs.
		private delegate void TimeProc(int id, int msg, int user, int param1, int param2);

		/// Specifies constants for multimedia timer event types.

		public enum TimerMode
		{
			OneShot,	// Timer event occurs once.
			Periodic	// Timer event occurs periodically.
		};

		/// Represents information about the timer's capabilities.
		[StructLayout(LayoutKind.Sequential)]
			public struct TimerCaps
		{
			public int periodMin;	// Minimum supported period in milliseconds.
			public int periodMax;	// Maximum supported period in milliseconds.
		}


		// Gets timer capabilities.
		[DllImport("winmm.dll")]
		private static extern int timeGetDevCaps(ref TimerCaps caps,
			int sizeOfTimerCaps);

		// Creates and starts the timer.
		[DllImport("winmm.dll")]
		private static extern int timeSetEvent(int delay, int resolution,
			TimeProc proc, int user, int mode);

		// Stops and destroys the timer.
		[DllImport("winmm.dll")]
		private static extern int timeKillEvent(int id);

		// Indicates that the operation was successful.
		private const int TIMERR_NOERROR = 0;

		// Timer identifier.
		private int timerID;

		// Timer mode.
		//private volatile TimerMode mode;

		// Period between timer events in milliseconds.
		//private volatile int period;

		// Timer resolution in milliseconds.
		//private volatile int resolution;

		// Indicates whether or not the timer is running.
		//private bool running;

		// Called by Windows when a timer periodic event occurs.
		//	private TimeProc timeProcPeriodic;

		// Called by Windows when a timer one shot event occurs.
		//	private TimeProc timeProcOneShot;

		// Indicates whether or not the timer has been disposed.
		//private volatile bool disposed = false;

		// For implementing IComponent.
		//	private ISite site = null;

		// Multimedia timer capabilities.
		//private static TimerCaps caps;
		// Called by Windows when a timer periodic event occurs.
		private TimeProc timeProcPeriodic;
	
		private void setup_timer()
		{
			tel = wpmrate();
#if(CWX_DEBUG)
			Debug.WriteLine(tel+" ms");
#endif
			if (timerID != 0)
			{
				timeKillEvent(timerID);
			}
			timerID = timeSetEvent(tel, 1, timeProcPeriodic, 0, (int)TimerMode.Periodic);
			if (timerID == 0)
			{
				Debug.Fail("Timer creation failed.");
			}
		}
		#endregion

		#region radio interface and fifo functions

		private void setptt(bool state)
		{
			console.Keyer.MemoryPTT = state;
			ptt = state;
			if (state) pttLed.BackColor = System.Drawing.Color.Red;
			else pttLed.BackColor = System.Drawing.Color.Black;
//			if (newptt) Thread.Sleep(200);
		}
		private void setkey(bool state)
		{
			console.Keyer.MemoryKey = state;
			if (state) keyLed.BackColor = System.Drawing.Color.Yellow;
			else keyLed.BackColor = System.Drawing.Color.Black;
		}
		private void quitshut()
		{
			clear_fifo();
			clear_fifo2();
			setptt(false);
			setkey(false);
			ttx = 0;  pause = 0; newptt = 0;
			keying = false;
		}
		private void clear_fifo()
		{
			cwfifo.WaitOne();
			infifo = 0;
			pin = 0; 
			pout = 0;
			cwfifo.ReleaseMutex();
		}
		private void push_fifo(byte data)
		{
			cwfifo.WaitOne();
			elfifo.SetValue(data,pin);
			pin++; 
			if (pin >= elfifo.Length) pin = 0;
			infifo++;
			cwfifo.ReleaseMutex();

//			Debug.WriteLine("push " + data);
		}
		private byte pop_fifo()
		{
			byte data;

			if (infifo < 1) data = EL_UNDERFLOW;
			else
			{
				cwfifo.WaitOne();
				data = (byte)elfifo.GetValue(pout);
				pout++; 
				if (pout >= elfifo.Length) pout = 0;
				infifo--;
				cwfifo.ReleaseMutex();
			}
#if(CWX_DEBUG)
			Debug.WriteLine("pop " + data);
#endif

			return data;
		}

		private void clear_fifo2()
		{
			cwfifo2.WaitOne();
			infifo2 = 0;
			pin2 = 0; 
			pout2 = 0;
			cwfifo2.ReleaseMutex();
		}
		private void push_fifo2(byte data)
		{
			cwfifo2.WaitOne();
			fifo2.SetValue(data,pin2);
			pin2++; 
			if (pin2 >= fifo2.Length) pin2 = 0;
			infifo2++;
			cwfifo2.ReleaseMutex();
#if(CWX_DEBUG)
			Debug.WriteLine("push " + data);
#endif
		}
		private byte pop_fifo2()
		{
			byte data;

			if (infifo2 < 1) data = EL_UNDERFLOW;
			else
			{
				cwfifo2.WaitOne();
				data = (byte)fifo2.GetValue(pout2);
				pout2++; 
				if (pout2 >= fifo2.Length) pout2 = 0;
				infifo2--;
				cwfifo2.ReleaseMutex();
			}
#if(CWX_DEBUG)
			Debug.WriteLine("pop " + data);
#endif

			return data;
		}
		#endregion

		#region Morse definition, table builders, and help display
		private int wpmrate()	// Tel in ms from wpm (based on PARIS method
		{
			return	(1200/cwxwpm);
		}
		private void help()
		{
			string t;

			t = "                  Memory and Keyboard Keyer Notes\n";
			t += "\n";
			t += "Radio must be running in a valid cw mode and frequency.\n";
			t += "Speed is for this form only (may change later).\n";
			t += "PTT Delay (ms) is time from PTT to first key down.\n";
			t += "Drop Delay (ms) is time for radio to drop out of transmit when keying stops.\n";
			t += "Drop Delay cannot be set less than PTT Delay * 1.5 and should\n";
			t += "be kept high enough so that PTT does not drop out between words.\n";
			t += "Note that weight settings on Setup Form do not affect CWX at this time.\n";
			t += "\n";
			t += "Messages may be edited while sending but take effect after restarting the message.\n";
			t += "\n";
			t += "\n";
			t += "In the keyer memories special characters are:\n\n";
			t += "    # sends a long dash a bit longer than a zero\n";
			t += "    $ send a long space as above\n";
			t += "    \" at the message end means loop it continuously\n";
			t += "      and Repeat Delay sets time between repeats in seconds\n";
			t += "\n";

			t += "    + is AR        ( is KN         * is SK\n";
			t += "    ! is SN        = is BT         \\ is BK\n\n";
			t += "The other specials can be changed in morsedef.txt.\n";
			t += "They are & ' ) : ; < > [  ] and ^\n";
			t += "All others without regular Morse defs send a space.\n";
			t += "\nPress button in lower right corner to show keyboard and more memories.\n\n";
			t += "Keyboard button must have the focus for characters\n";
			t += "to be entered in the keyboard buffer. Cyan indicator on.\n\n";
			t += "Keyboard sending will pause if the checkbox is checked.\n";
			t += "F1 flips the pause state.  F2 clears the keyboard.\n";
			t += "Alt 1 thru Alt 9 will load memories 1 to 9 into keyboard buffer.\n";
			t += "Right click on a message button will also load it into the keyboard\n";
			t += "\n";
			t += "Esc stops any output and clears the keyboard buffer.\n";
			t += "\n";
			t += "\n";
			t += "    << Sugar Land, Texas 2006-02-16 - Richard Allen, W5SXD >>\n";
	
			MessageBox.Show(t,"  CWX Notes ...");
		}
	
		private void notesButton_Click(object sender, System.EventArgs e)
		{
			Thread t = new Thread(new ThreadStart(help));
			t.Name = "help thread";
			t.IsBackground = true;					// if app closes, kill this thread
			t.Priority = ThreadPriority.Normal;
			t.Start();
		}



		private void build_mbits2()
		{
			uint els;
			uint nel;
			uint mask;
			int ndd;
			string s, st;

			for (int i = 0; i < 64; i++)	// for each code
			{
				nel = 0;  els = 0; mask = 0x80000000;
				/*
				012345678901234
				64|@|.--.-.   |
				*/
				
				st = (string)a2m2.GetValue(i);
				s = st.Substring(5,9);

				ndd = s.Length;
#if(CWX_DEBUG)
				Debug.WriteLine(s);
#endif

				for (int k = 0; k < ndd; k++)
				{				
					if (String.CompareOrdinal(s,k,"-",0,1) == 0)
					{
						nel += 4;
						els |= mask; mask >>= 1;
						els |= mask; mask >>= 1;
						els |= mask; mask >>= 1;
						mask >>= 1;
					}
					else if (String.CompareOrdinal(s,k,".",0,1) == 0)
					{
						nel += 2;
						els |= mask; mask >>= 1;
						mask >>= 1;
					}
					else continue;

				}
	// a space will have a 4 element count + 1 element space + 2 letter spaces
	// making a total of 7 elements in a word space

				if (i == 0) nel = 4;	// value for ' ' character
				else nel += 2;			// add letter space

				els &= 0xffffffe0;
				els += nel & 0x1f;
#if(CWX_DEBUG)
				Debug.WriteLine(ndd+": "+s+" "+sbin(els));
#endif
				mbits.SetValue(els,i);
			}
		}

		private string sfile = "morsedef.txt";

		private void load_alpha()
		{

			if (!File.Exists(console.AppDataPath+"\\"+sfile))	// create default morsedef.txt
			{
#if(CWX_DEBUG)
				MessageBox.Show(sfile+" not found, creating ...");
#endif
                using (StreamWriter sw = new StreamWriter(console.AppDataPath + "\\" + sfile)) 
				{
					sw.WriteLine("32| |*        | space     ");
					sw.WriteLine("33|!|...-.    | [SN]      ");
					sw.WriteLine("34|\"|*        | loop      ");
					sw.WriteLine("35|#|*        | long dash ");
					sw.WriteLine("36|$|*        | long space");
					sw.WriteLine("37|%|.-...    | [AS]      ");
					sw.WriteLine("38|&|.........| 0123456789");
					sw.WriteLine("39|'|         |           ");
					sw.WriteLine("40|(|-.--.    | [KN]      ");
					sw.WriteLine("41|)|         |           ");
					sw.WriteLine("42|*|...-.-   | [SK]      ");
					sw.WriteLine("43|+|.-.-.    | [AR]      ");
					sw.WriteLine("44|,|--..--   |           ");
					sw.WriteLine("45|-|-....-   |           ");
					sw.WriteLine("46|.|.-.-.-   |           ");
					sw.WriteLine("47|/|-..-.    |           ");
					sw.WriteLine("48|0|-----    |           ");
					sw.WriteLine("49|1|.----    |           ");
					sw.WriteLine("50|2|..---    |           ");
					sw.WriteLine("51|3|...--    |           ");
					sw.WriteLine("52|4|....-    |           ");
					sw.WriteLine("53|5|.....    |           ");
					sw.WriteLine("54|6|-....    |           ");
					sw.WriteLine("55|7|--...    |           ");
					sw.WriteLine("56|8|---..    |           ");
					sw.WriteLine("57|9|----.    |           ");
					sw.WriteLine("58|:|         |           ");
					sw.WriteLine("59|;|         |           ");
					sw.WriteLine("60|<|         |           ");
					sw.WriteLine("61|=|-...-    | [BT]      ");
					sw.WriteLine("62|>|         |           ");
					sw.WriteLine("63|?|..--..   |           ");
					sw.WriteLine("64|@|.--.-.   |           ");
					sw.WriteLine("65|A|.-       |           ");
					sw.WriteLine("66|B|-...     |           ");
					sw.WriteLine("67|C|-.-.     |           ");
					sw.WriteLine("68|D|-..      |           ");
					sw.WriteLine("69|E|.        |           ");
					sw.WriteLine("70|F|..-.     |           ");
					sw.WriteLine("71|G|--.      |           ");
					sw.WriteLine("72|H|....     |           ");
					sw.WriteLine("73|I|..       |           ");
					sw.WriteLine("74|J|.---     |           ");
					sw.WriteLine("75|K|-.-      |           ");
					sw.WriteLine("76|L|.-..     |           ");
					sw.WriteLine("77|M|--       |           ");
					sw.WriteLine("78|N|-.       |           ");
					sw.WriteLine("79|O|---      |           ");
					sw.WriteLine("80|P|.--.     |           ");
					sw.WriteLine("81|Q|--.-     |           ");
					sw.WriteLine("82|R|.-.      |           ");
					sw.WriteLine("83|S|...      |           ");
					sw.WriteLine("84|T|-        |           ");
					sw.WriteLine("85|U|..-      |           ");
					sw.WriteLine("86|V|...-     |           ");
					sw.WriteLine("87|W|.--      |           ");
					sw.WriteLine("88|X|-..-     |           ");
					sw.WriteLine("89|Y|-.--     |           ");
					sw.WriteLine("90|Z|--..     |           ");
					sw.WriteLine("91|[|         |           ");
					sw.WriteLine("92|\\|-...-.-  | [BK]      ");
					sw.WriteLine("93|]|         |           ");
					sw.WriteLine("94|^|         |           ");
					sw.WriteLine("95|_|*        | reserved  ");
				}
			}
			//MessageBox.Show("reading ",sfile);
            using (StreamReader sr = new StreamReader(console.AppDataPath + "\\" + sfile)) 
			{
				String line;
				String t;
				int nl = 0;

				cbMorse.Items.Clear();
				while ((line = sr.ReadLine()) != null) 
				{
					cbMorse.Items.Add(line);
					a2m2.SetValue(line,nl);
					nl++;
				}
				if (nl != 64)
				{
					t = sfile + " has incorrect length and may be corrupt\n";
					t += "delete it and let it be rebuilt ...";
					MessageBox.Show(t);
				}
				cbMorse.SelectedIndex = 0;
			}
		}

		#endregion

		#region CAT Interface
		// Added by Bob Tracy July, 2007 to implement a remote interface to send
		// a CW message via the CAT KY command.

		PowerSDR.RingBufferByte rb = new RingBufferByte(2048);

		public string RemoteMessage(byte[] msg)
		{
			rb.Write(msg, msg.Length);
			return "";
		}

		public string RemoteMessage(char msg)
		{
			loadchar(msg);
			return "";
		}


		private void SendBufferMessage()				//CAT Read Thread
		{
			while(true)									//do forever
			{
				byte[] buffer;							//holds the bytes read from the ringbuffer
				char chr2send;

				if(rb.ReadSpace() > 0)					//if we have data in the ringbuffer
				{
					buffer = new byte[1];				//an array to hold one byte of rb data
					rb.Read(buffer, buffer.Length);		//read the ringbuffer
					chr2send = (char) buffer[0];
					loadchar(chr2send);
					while (infifo > 2)					//number of elements left in the element fifo
					{
						Thread.Sleep(2);				//wait for the element fifo to catch up
					}
				}
				Thread.Sleep(2);						// give the cpu a rest
			}
		}


		public int WPM
		{
			get{return cwxwpm;}
			set{udWPM.Value = value;}
		}

		public int Characters2Send
		{
			get{return infifo;}
		}

        public void CWXStop()
        {
            stopButton.PerformClick();
        }
		
       public int StartQueue
        {
            set
            {
                queue_start(value);
            }
        }
		
		#endregion CAT Interface

		#region startup/shutdown stuff
		public CWX(Console c)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			console = c;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			txtdummy1.Hide();
			clear_keys();

			txt1.Text = "### test de w5sxd/b el29ep.$$\"";
			txt2.Text = "cq cq test w5sxd test";
			txt3.Text = "5nn stx";
			txt4.Text = "k5sdr de w5sxd (";
			txt5.Text = "cq cq cq de w5sxd w5sxd w5sxd +k";
			txt6.Text = "The quick brown fox jumped over the lazy dog. 0123456789 ";
			txt7.Text = "?";
			txt8.Text = "agn";
			txt9.Text = "n6vs";

			//RestoreSettings();
			Common.RestoreForm(this, "CWX", true);

			//		cwxwpm = 20;
			//		udWPM.Value = cwxwpm;
			cwxwpm = (int)udWPM.Value;

			tpause = (int)udDelay.Value * 1000;
			if (tpause < 1) tpause = tel;
			ttdel = (int)udDrop.Value;
			pttdelay = (int)udPtt.Value;
			//udDrop.Minimum = pttdelay + pttdelay/2;



//			RestoreSettings();

#if(CWX_DEBUG)
			Debug.WriteLine("CWX entry");
#endif

			timeProcPeriodic = new TimeProc(TimerPeriodicEventCallback);
			setup_timer();
			load_alpha();
			// build the mbits array from a2m2
			build_mbits2();

			stopThreads = false;

			Thread keyFifoThread = new Thread(new ThreadStart(keyboardFifo));
			keyFifoThread.Name = "keyboard fifo pop thread";
			keyFifoThread.IsBackground = true;					// if app closes, kill this thread
			keyFifoThread.Priority = ThreadPriority.Normal;
			keyFifoThread.Start();
			
			Thread keyDisplayThread = new Thread(new ThreadStart(keyboardDisplay));
			keyDisplayThread.Name = "keyboard edit box handler thread";
			keyDisplayThread.IsBackground = true;					// if app closes, kill this thread
			keyDisplayThread.Priority = ThreadPriority.Normal;
			keyDisplayThread.Start();

			Thread CATReadThread = new Thread(new ThreadStart(SendBufferMessage));
			CATReadThread.Name = "CAT Read Thread";
			CATReadThread.IsBackground = true;
			CATReadThread.Priority = ThreadPriority.Highest;
			CATReadThread.Start();

//			ttdel = 50;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
#if(CWX_DEBUG)
			Debug.WriteLine("dispose cwx");
#endif
			timeKillEvent(timerID);
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CWX));
            this.txt1 = new System.Windows.Forms.TextBoxTS();
            this.s1 = new System.Windows.Forms.ButtonTS();
            this.s2 = new System.Windows.Forms.ButtonTS();
            this.txt2 = new System.Windows.Forms.TextBoxTS();
            this.s3 = new System.Windows.Forms.ButtonTS();
            this.txt3 = new System.Windows.Forms.TextBoxTS();
            this.s4 = new System.Windows.Forms.ButtonTS();
            this.txt4 = new System.Windows.Forms.TextBoxTS();
            this.s5 = new System.Windows.Forms.ButtonTS();
            this.txt5 = new System.Windows.Forms.TextBoxTS();
            this.s6 = new System.Windows.Forms.ButtonTS();
            this.txt6 = new System.Windows.Forms.TextBoxTS();
            this.speedLabel = new System.Windows.Forms.LabelTS();
            this.notesButton = new System.Windows.Forms.ButtonTS();
            this.cbMorse = new System.Windows.Forms.ComboBoxTS();
            this.udDelay = new System.Windows.Forms.NumericUpDownTS();
            this.repeatdelayLabel = new System.Windows.Forms.LabelTS();
            this.label4 = new System.Windows.Forms.LabelTS();
            this.stopButton = new System.Windows.Forms.ButtonTS();
            this.label5 = new System.Windows.Forms.LabelTS();
            this.label6 = new System.Windows.Forms.LabelTS();
            this.txt7 = new System.Windows.Forms.TextBoxTS();
            this.s7 = new System.Windows.Forms.ButtonTS();
            this.s8 = new System.Windows.Forms.ButtonTS();
            this.txt8 = new System.Windows.Forms.TextBoxTS();
            this.s9 = new System.Windows.Forms.ButtonTS();
            this.txt9 = new System.Windows.Forms.TextBoxTS();
            this.udDrop = new System.Windows.Forms.NumericUpDownTS();
            this.dropdelaylabel = new System.Windows.Forms.LabelTS();
            this.keyButton = new System.Windows.Forms.ButtonTS();
            this.label7 = new System.Windows.Forms.LabelTS();
            this.txtdummy1 = new System.Windows.Forms.TextBoxTS();
            this.chkPause = new System.Windows.Forms.CheckBoxTS();
            this.clearButton = new System.Windows.Forms.ButtonTS();
            this.keyboardButton = new System.Windows.Forms.ButtonTS();
            this.pttLed = new System.Windows.Forms.Panel();
            this.keyLed = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.keyboardLed = new System.Windows.Forms.Panel();
            this.expandButton = new System.Windows.Forms.ButtonTS();
            this.pttdelaylabel = new System.Windows.Forms.LabelTS();
            this.udPtt = new System.Windows.Forms.NumericUpDownTS();
            this.udWPM = new System.Windows.Forms.NumericUpDownTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.udDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDrop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPtt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udWPM)).BeginInit();
            this.SuspendLayout();
            // 
            // txt1
            // 
            this.txt1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt1.Location = new System.Drawing.Point(40, 56);
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(172, 20);
            this.txt1.TabIndex = 3;
            this.txt1.Text = "cq cq test w5sxd test";
            this.toolTip1.SetToolTip(this.txt1, "Message edit box.");
            // 
            // s1
            // 
            this.s1.Image = null;
            this.s1.Location = new System.Drawing.Point(8, 56);
            this.s1.Name = "s1";
            this.s1.Size = new System.Drawing.Size(24, 20);
            this.s1.TabIndex = 4;
            this.s1.Text = "1";
            this.toolTip1.SetToolTip(this.s1, "Start message 1.");
            this.s1.Click += new System.EventHandler(this.s1_Click);
            this.s1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s1_MouseDown);
            // 
            // s2
            // 
            this.s2.Image = null;
            this.s2.Location = new System.Drawing.Point(8, 88);
            this.s2.Name = "s2";
            this.s2.Size = new System.Drawing.Size(24, 20);
            this.s2.TabIndex = 6;
            this.s2.Text = "2";
            this.toolTip1.SetToolTip(this.s2, "Start message 2.");
            this.s2.Click += new System.EventHandler(this.s2_Click);
            this.s2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s2_MouseDown);
            // 
            // txt2
            // 
            this.txt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt2.Location = new System.Drawing.Point(40, 88);
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(172, 20);
            this.txt2.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txt2, "Message edit box.");
            // 
            // s3
            // 
            this.s3.Image = null;
            this.s3.Location = new System.Drawing.Point(8, 120);
            this.s3.Name = "s3";
            this.s3.Size = new System.Drawing.Size(24, 20);
            this.s3.TabIndex = 8;
            this.s3.Text = "3";
            this.toolTip1.SetToolTip(this.s3, "Start message 3.");
            this.s3.Click += new System.EventHandler(this.s3_Click);
            this.s3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s3_MouseDown);
            // 
            // txt3
            // 
            this.txt3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt3.Location = new System.Drawing.Point(40, 120);
            this.txt3.Name = "txt3";
            this.txt3.Size = new System.Drawing.Size(172, 20);
            this.txt3.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txt3, "Message edit box.");
            // 
            // s4
            // 
            this.s4.Image = null;
            this.s4.Location = new System.Drawing.Point(232, 56);
            this.s4.Name = "s4";
            this.s4.Size = new System.Drawing.Size(24, 20);
            this.s4.TabIndex = 10;
            this.s4.Text = "4";
            this.toolTip1.SetToolTip(this.s4, "Start message 4.");
            this.s4.Click += new System.EventHandler(this.s4_Click);
            this.s4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s4_MouseDown);
            // 
            // txt4
            // 
            this.txt4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt4.Location = new System.Drawing.Point(264, 56);
            this.txt4.Name = "txt4";
            this.txt4.Size = new System.Drawing.Size(176, 20);
            this.txt4.TabIndex = 9;
            this.toolTip1.SetToolTip(this.txt4, "Message edit box.");
            // 
            // s5
            // 
            this.s5.Image = null;
            this.s5.Location = new System.Drawing.Point(232, 88);
            this.s5.Name = "s5";
            this.s5.Size = new System.Drawing.Size(24, 20);
            this.s5.TabIndex = 12;
            this.s5.Text = "5";
            this.toolTip1.SetToolTip(this.s5, "Start message 5.");
            this.s5.Click += new System.EventHandler(this.s5_Click);
            this.s5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s5_MouseDown);
            // 
            // txt5
            // 
            this.txt5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt5.Location = new System.Drawing.Point(264, 88);
            this.txt5.Name = "txt5";
            this.txt5.Size = new System.Drawing.Size(176, 20);
            this.txt5.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txt5, "Message edit box.");
            // 
            // s6
            // 
            this.s6.Image = null;
            this.s6.Location = new System.Drawing.Point(232, 120);
            this.s6.Name = "s6";
            this.s6.Size = new System.Drawing.Size(24, 20);
            this.s6.TabIndex = 14;
            this.s6.Text = "6";
            this.toolTip1.SetToolTip(this.s6, "Start message 6.");
            this.s6.Click += new System.EventHandler(this.s6_Click);
            this.s6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s6_MouseDown);
            // 
            // txt6
            // 
            this.txt6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt6.Location = new System.Drawing.Point(264, 120);
            this.txt6.Name = "txt6";
            this.txt6.Size = new System.Drawing.Size(160, 20);
            this.txt6.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txt6, "Message edit box.");
            // 
            // speedLabel
            // 
            this.speedLabel.Image = null;
            this.speedLabel.Location = new System.Drawing.Point(232, 32);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(72, 16);
            this.speedLabel.TabIndex = 15;
            this.speedLabel.Text = "Speed WPM";
            this.toolTip1.SetToolTip(this.speedLabel, " Set memory keyer (not paddle) speed in words per minute. (PARIS method)");
            // 
            // notesButton
            // 
            this.notesButton.Image = null;
            this.notesButton.Location = new System.Drawing.Point(176, 8);
            this.notesButton.Name = "notesButton";
            this.notesButton.Size = new System.Drawing.Size(48, 24);
            this.notesButton.TabIndex = 17;
            this.notesButton.Text = "Notes";
            this.toolTip1.SetToolTip(this.notesButton, "Show program notes.");
            this.notesButton.Click += new System.EventHandler(this.notesButton_Click);
            // 
            // cbMorse
            // 
            this.cbMorse.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbMorse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMorse.DropDownWidth = 208;
            this.cbMorse.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMorse.Location = new System.Drawing.Point(472, 152);
            this.cbMorse.Name = "cbMorse";
            this.cbMorse.Size = new System.Drawing.Size(208, 23);
            this.cbMorse.TabIndex = 19;
            this.toolTip1.SetToolTip(this.cbMorse, " View and right click to edit Morse definition table.");
            this.cbMorse.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cbMorse_MouseDown);
            // 
            // udDelay
            // 
            this.udDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDelay.Location = new System.Drawing.Point(312, 8);
            this.udDelay.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.udDelay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDelay.Name = "udDelay";
            this.udDelay.Size = new System.Drawing.Size(56, 20);
            this.udDelay.TabIndex = 20;
            this.udDelay.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.udDelay.ValueChanged += new System.EventHandler(this.udDelay_ValueChanged);
            this.udDelay.LostFocus += new System.EventHandler(this.udDelay_LostFocus);
            // 
            // repeatdelayLabel
            // 
            this.repeatdelayLabel.Image = null;
            this.repeatdelayLabel.Location = new System.Drawing.Point(304, 32);
            this.repeatdelayLabel.Name = "repeatdelayLabel";
            this.repeatdelayLabel.Size = new System.Drawing.Size(80, 16);
            this.repeatdelayLabel.TabIndex = 48;
            this.repeatdelayLabel.Text = "Repeat Delay";
            this.toolTip1.SetToolTip(this.repeatdelayLabel, " Set repeat message delay in seconds.");
            // 
            // label4
            // 
            this.label4.Image = null;
            this.label4.Location = new System.Drawing.Point(56, 304);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(256, 32);
            this.label4.TabIndex = 25;
            this.label4.Text = "label4";
            // 
            // stopButton
            // 
            this.stopButton.Image = null;
            this.stopButton.Location = new System.Drawing.Point(48, 8);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(72, 24);
            this.stopButton.TabIndex = 26;
            this.stopButton.Text = "Stop (Esc)";
            this.toolTip1.SetToolTip(this.stopButton, "Stop all keying.");
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // label5
            // 
            this.label5.Image = null;
            this.label5.Location = new System.Drawing.Point(376, 304);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(256, 32);
            this.label5.TabIndex = 27;
            this.label5.Text = "label5";
            // 
            // label6
            // 
            this.label6.Image = null;
            this.label6.Location = new System.Drawing.Point(56, 352);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(256, 32);
            this.label6.TabIndex = 28;
            this.label6.Text = "label6";
            // 
            // txt7
            // 
            this.txt7.Location = new System.Drawing.Point(488, 56);
            this.txt7.Name = "txt7";
            this.txt7.Size = new System.Drawing.Size(176, 20);
            this.txt7.TabIndex = 29;
            this.toolTip1.SetToolTip(this.txt7, "Message edit box.");
            // 
            // s7
            // 
            this.s7.Image = null;
            this.s7.Location = new System.Drawing.Point(456, 56);
            this.s7.Name = "s7";
            this.s7.Size = new System.Drawing.Size(24, 20);
            this.s7.TabIndex = 30;
            this.s7.Text = "7";
            this.toolTip1.SetToolTip(this.s7, "Start message 7.");
            this.s7.Click += new System.EventHandler(this.s7_Click);
            this.s7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s7_MouseDown);
            // 
            // s8
            // 
            this.s8.Image = null;
            this.s8.Location = new System.Drawing.Point(456, 88);
            this.s8.Name = "s8";
            this.s8.Size = new System.Drawing.Size(24, 20);
            this.s8.TabIndex = 31;
            this.s8.Text = "8";
            this.toolTip1.SetToolTip(this.s8, "Start message 8.");
            this.s8.Click += new System.EventHandler(this.s8_Click);
            this.s8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s8_MouseDown);
            // 
            // txt8
            // 
            this.txt8.Location = new System.Drawing.Point(488, 88);
            this.txt8.Name = "txt8";
            this.txt8.Size = new System.Drawing.Size(176, 20);
            this.txt8.TabIndex = 32;
            this.toolTip1.SetToolTip(this.txt8, "Message edit box.");
            // 
            // s9
            // 
            this.s9.Image = null;
            this.s9.Location = new System.Drawing.Point(456, 120);
            this.s9.Name = "s9";
            this.s9.Size = new System.Drawing.Size(24, 20);
            this.s9.TabIndex = 33;
            this.s9.Text = "9";
            this.toolTip1.SetToolTip(this.s9, "Start message 9.");
            this.s9.Click += new System.EventHandler(this.s9_Click);
            this.s9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.s9_MouseDown);
            // 
            // txt9
            // 
            this.txt9.Location = new System.Drawing.Point(488, 120);
            this.txt9.Name = "txt9";
            this.txt9.Size = new System.Drawing.Size(176, 20);
            this.txt9.TabIndex = 34;
            this.toolTip1.SetToolTip(this.txt9, "Message edit box.");
            // 
            // udDrop
            // 
            this.udDrop.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDrop.Location = new System.Drawing.Point(384, 8);
            this.udDrop.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.udDrop.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDrop.Name = "udDrop";
            this.udDrop.Size = new System.Drawing.Size(56, 20);
            this.udDrop.TabIndex = 35;
            this.udDrop.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.udDrop.ValueChanged += new System.EventHandler(this.udDrop_ValueChanged);
            this.udDrop.LostFocus += new System.EventHandler(this.udDrop_LostFocus);
            // 
            // dropdelaylabel
            // 
            this.dropdelaylabel.Image = null;
            this.dropdelaylabel.Location = new System.Drawing.Point(384, 32);
            this.dropdelaylabel.Name = "dropdelaylabel";
            this.dropdelaylabel.Size = new System.Drawing.Size(64, 16);
            this.dropdelaylabel.TabIndex = 36;
            this.dropdelaylabel.Text = "Drop Delay";
            this.toolTip1.SetToolTip(this.dropdelaylabel, " Set break in drop out in milliseconds. Minimum allowed is PTT Delay * 1.5 .");
            // 
            // keyButton
            // 
            this.keyButton.Image = null;
            this.keyButton.Location = new System.Drawing.Point(128, 8);
            this.keyButton.Name = "keyButton";
            this.keyButton.Size = new System.Drawing.Size(40, 24);
            this.keyButton.TabIndex = 37;
            this.keyButton.Text = "Key";
            this.toolTip1.SetToolTip(this.keyButton, "Turn on transmitter and key it. (60 second timeout)");
            this.keyButton.Click += new System.EventHandler(this.keyButton_Click);
            // 
            // label7
            // 
            this.label7.Image = null;
            this.label7.Location = new System.Drawing.Point(376, 352);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(256, 32);
            this.label7.TabIndex = 47;
            this.label7.Text = "label7";
            // 
            // txtdummy1
            // 
            this.txtdummy1.Location = new System.Drawing.Point(12, 180);
            this.txtdummy1.Multiline = true;
            this.txtdummy1.Name = "txtdummy1";
            this.txtdummy1.Size = new System.Drawing.Size(665, 82);
            this.txtdummy1.TabIndex = 42;
            this.txtdummy1.Text = "the actual text box will be a graphic here and this one disabled";
            // 
            // chkPause
            // 
            this.chkPause.Image = null;
            this.chkPause.Location = new System.Drawing.Point(16, 156);
            this.chkPause.Name = "chkPause";
            this.chkPause.Size = new System.Drawing.Size(80, 16);
            this.chkPause.TabIndex = 43;
            this.chkPause.Text = "Pause (F1)";
            this.toolTip1.SetToolTip(this.chkPause, " Pause keyboard transmission.");
            this.chkPause.CheckedChanged += new System.EventHandler(this.chkPause_CheckedChanged);
            // 
            // clearButton
            // 
            this.clearButton.Image = null;
            this.clearButton.Location = new System.Drawing.Point(120, 152);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 46;
            this.clearButton.Text = "Clear (F2)";
            this.toolTip1.SetToolTip(this.clearButton, " Clear the keyboard buffer.");
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // keyboardButton
            // 
            this.keyboardButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keyboardButton.Image = null;
            this.keyboardButton.Location = new System.Drawing.Point(216, 152);
            this.keyboardButton.Name = "keyboardButton";
            this.keyboardButton.Size = new System.Drawing.Size(112, 23);
            this.keyboardButton.TabIndex = 45;
            this.keyboardButton.Text = "Keyboard";
            this.toolTip1.SetToolTip(this.keyboardButton, " Enable keyboard.  This must be selected for keyboard to work.");
            this.keyboardButton.Leave += new System.EventHandler(this.keyboardButton_Leave);
            this.keyboardButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyboardButton_KeyPress);
            this.keyboardButton.Enter += new System.EventHandler(this.keyboardButton_Enter);
            // 
            // pttLed
            // 
            this.pttLed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pttLed.Location = new System.Drawing.Point(10, 8);
            this.pttLed.Name = "pttLed";
            this.pttLed.Size = new System.Drawing.Size(24, 13);
            this.pttLed.TabIndex = 49;
            this.toolTip1.SetToolTip(this.pttLed, " PTT status");
            // 
            // keyLed
            // 
            this.keyLed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keyLed.Location = new System.Drawing.Point(10, 24);
            this.keyLed.Name = "keyLed";
            this.keyLed.Size = new System.Drawing.Size(24, 13);
            this.keyLed.TabIndex = 50;
            this.toolTip1.SetToolTip(this.keyLed, "Key status");
            // 
            // keyboardLed
            // 
            this.keyboardLed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keyboardLed.Location = new System.Drawing.Point(344, 157);
            this.keyboardLed.Name = "keyboardLed";
            this.keyboardLed.Size = new System.Drawing.Size(24, 13);
            this.keyboardLed.TabIndex = 52;
            this.toolTip1.SetToolTip(this.keyboardLed, " Keyboard active indicator.");
            // 
            // expandButton
            // 
            this.expandButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.expandButton.Image = null;
            this.expandButton.Location = new System.Drawing.Point(432, 136);
            this.expandButton.Name = "expandButton";
            this.expandButton.Size = new System.Drawing.Size(8, 8);
            this.expandButton.TabIndex = 53;
            this.toolTip1.SetToolTip(this.expandButton, " Expand form.");
            this.expandButton.UseVisualStyleBackColor = false;
            this.expandButton.Click += new System.EventHandler(this.expandButton_Click);
            // 
            // pttdelaylabel
            // 
            this.pttdelaylabel.Image = null;
            this.pttdelaylabel.Location = new System.Drawing.Point(456, 32);
            this.pttdelaylabel.Name = "pttdelaylabel";
            this.pttdelaylabel.Size = new System.Drawing.Size(64, 16);
            this.pttdelaylabel.TabIndex = 55;
            this.pttdelaylabel.Text = "PTT Delay";
            this.toolTip1.SetToolTip(this.pttdelaylabel, "Set delay from PTT to key down in milliseconds.");
            // 
            // udPtt
            // 
            this.udPtt.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udPtt.Location = new System.Drawing.Point(456, 8);
            this.udPtt.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.udPtt.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udPtt.Name = "udPtt";
            this.udPtt.Size = new System.Drawing.Size(56, 20);
            this.udPtt.TabIndex = 54;
            this.udPtt.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.udPtt.ValueChanged += new System.EventHandler(this.udPtt_ValueChanged);
            this.udPtt.LostFocus += new System.EventHandler(this.udPtt_LostFocus);
            // 
            // udWPM
            // 
            this.udWPM.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.udWPM.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udWPM.Location = new System.Drawing.Point(240, 8);
            this.udWPM.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.udWPM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udWPM.Name = "udWPM";
            this.udWPM.Size = new System.Drawing.Size(56, 20);
            this.udWPM.TabIndex = 56;
            this.udWPM.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            this.udWPM.ValueChanged += new System.EventHandler(this.udWPM_ValueChanged);
            this.udWPM.LostFocus += new System.EventHandler(this.udWPM_LostFocus);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(528, 8);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(104, 24);
            this.chkAlwaysOnTop.TabIndex = 57;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // CWX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5, 13);
            this.ClientSize = new System.Drawing.Size(448, 150);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.udWPM);
            this.Controls.Add(this.pttdelaylabel);
            this.Controls.Add(this.udPtt);
            this.Controls.Add(this.expandButton);
            this.Controls.Add(this.keyboardLed);
            this.Controls.Add(this.keyLed);
            this.Controls.Add(this.pttLed);
            this.Controls.Add(this.keyboardButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.chkPause);
            this.Controls.Add(this.txtdummy1);
            this.Controls.Add(this.txt9);
            this.Controls.Add(this.txt8);
            this.Controls.Add(this.txt7);
            this.Controls.Add(this.txt6);
            this.Controls.Add(this.txt5);
            this.Controls.Add(this.txt4);
            this.Controls.Add(this.txt3);
            this.Controls.Add(this.txt2);
            this.Controls.Add(this.txt1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.keyButton);
            this.Controls.Add(this.dropdelaylabel);
            this.Controls.Add(this.udDrop);
            this.Controls.Add(this.s9);
            this.Controls.Add(this.s8);
            this.Controls.Add(this.s7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.repeatdelayLabel);
            this.Controls.Add(this.udDelay);
            this.Controls.Add(this.cbMorse);
            this.Controls.Add(this.notesButton);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.s6);
            this.Controls.Add(this.s5);
            this.Controls.Add(this.s4);
            this.Controls.Add(this.s3);
            this.Controls.Add(this.s2);
            this.Controls.Add(this.s1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CWX";
            this.Text = "   CW Memories and Keyboard ...";
            this.Load += new System.EventHandler(this.CWX_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CWX_Paint);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.CWX_Closing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CWX_KeyUp_1);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CWX_MouseMove);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CWX_KeyDown_1);
            ((System.ComponentModel.ISupportInitialize)(this.udDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDrop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udPtt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udWPM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region event handlers and callbacks

		private void expandButton_Click(object sender, System.EventArgs e)
		{
			if (CWX.ActiveForm.Width > 550)
			{
				CWX.ActiveForm.Width = 456;
				CWX.ActiveForm.Height = 176;
				expandButton.Left = 432;
				expandButton.Top = 136;
				toolTip1.SetToolTip(expandButton, " Expand form.");
			}
			else
			{
				CWX.ActiveForm.Width = 704;
				CWX.ActiveForm.Height = 304;
				expandButton.Left = 680;
				expandButton.Top = 264;
				toolTip1.SetToolTip(expandButton, " Compress form.");
			}
		}
		
		private void keyboardButton_Leave(object sender, System.EventArgs e)
		{
			keyboardButton.ForeColor = System.Drawing.Color.Gray;
			keyboardButton.Text = "Keys Off";
			keyboardLed.BackColor = System.Drawing.Color.Black;

		}

		private void keyboardButton_Enter(object sender, System.EventArgs e)
		{
			keyboardButton.ForeColor = System.Drawing.Color.Black;
			keyboardButton.Text = "KEYS ACTIVE";
			keyboardLed.BackColor = System.Drawing.Color.Cyan;

		}

		// this guy checks for the release of the Alt key
		private void CWX_KeyUp_1(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			kkk++;
			label6.Text = kkk.ToString() + " " + 
				e.KeyCode.ToString()+ " " + 
				e.KeyData.ToString() + " " + 
				e.KeyValue.ToString("x");

			if (e.KeyCode.ToString().Equals("Menu")) altkey = false;
		}

		// the Esc, F1, F2, and Alt 1 thru Alt 9 are handled anywhere on the form
		// the Alt key press is detected here and altkey set to true
		private void CWX_KeyDown_1(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			char key = (char)e.KeyValue;

			label5.Text = "KeyDown " + key + " " +
				e.KeyCode.ToString()+ " " + 
				e.KeyData.ToString() + " " + 
				e.KeyValue.ToString("x");

			if (key == 0x70)			// F1
			{
				chkPause.Checked = !chkPause.Checked;
			}
			else if (key == 0x71)		// F2
			{
				clear_show();
			}
			else if (key == 27)			// Esc
			{
				clear_show();
				quit = true;
				kquit = true;
			}
			
			else if (e.KeyCode.ToString().Equals("Menu")) altkey = true;

			if (altkey)
			{	// Alt 1 thru 9 load messages 1-9 into the keyboard buffer
				if (e.KeyCode.ToString().Equals("D1")) msg2keys(1);
				else if (e.KeyCode.ToString().Equals("D2")) msg2keys(2);
				else if (e.KeyCode.ToString().Equals("D3")) msg2keys(3);
				else if (e.KeyCode.ToString().Equals("D4")) msg2keys(4);
				else if (e.KeyCode.ToString().Equals("D5")) msg2keys(5);
				else if (e.KeyCode.ToString().Equals("D6")) msg2keys(6);
				else if (e.KeyCode.ToString().Equals("D7")) msg2keys(7);
				else if (e.KeyCode.ToString().Equals("D8")) msg2keys(8);
				else if (e.KeyCode.ToString().Equals("D9")) msg2keys(9);
			}

		}
	
		private void keyboardButton_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			process_key(e.KeyChar);	
		}

		// process the 'Key' button which start transmitter with key down
		private void keyButton_Click(object sender, System.EventArgs e)	// the 'Key' button
		{
			if (keying)
			{
				quitshut();
				return;
			}

			if(console.RX1DSPMode != DSPMode.CWL &&
				console.RX1DSPMode != DSPMode.CWU)
			{
				MessageBox.Show("Console is not in CW mode.  Please switch to either CWL or CWU and try again.",
					"CWX Error: Wrong Mode",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			quit = true;
			kquit = true;
			while (quit) Thread.Sleep(10);
			pause = 60000/tel;
			tqq = " . ";
			setptt(true);
			setkey(true);
			keying = true;
		}

		private void CWX_Load(object sender, System.EventArgs e)
		{		
#if(CWX_DEBUG)
			Debug.WriteLine("load cwx, queue is " + elfifo.Length);
#endif
		}
		private void CWX_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			quitshut();
			Thread.Sleep(100);
			
			// shut downs
			// savesettings, close threads, kill mmtimer
			stopThreads = true;
			if (timerID != 0)
			{
				timeKillEvent(timerID);		// kill the mmtimer
			}
			Thread.Sleep(200);		// let it all stop

			//SaveSettings();
			Common.SaveForm(this, "CWX");

		//	Debug.WriteLine("CWX_Closing()");

			// don't do next two lines so we will shut down the form completely

		//	this.Hide();
		//	e.Cancel = true;
		}
		// Callback method called by the Win32 multimedia timer when a timer
		// periodic event occurs.
		private void TimerPeriodicEventCallback(int id, int msg, int user, int param1, int param2)
		{
			process_element();
		}
		
		private void s1_Click(object sender, System.EventArgs e)
		{
			queue_start(1);
		}

		private void s2_Click(object sender, System.EventArgs e)
		{	
			queue_start(2);
		}

		private void s3_Click(object sender, System.EventArgs e)
		{
			queue_start(3);
		}

		private void s4_Click(object sender, System.EventArgs e)
		{
			queue_start(4);
		}

		private void s5_Click(object sender, System.EventArgs e)
		{
			queue_start(5);
		}

		private void s6_Click(object sender, System.EventArgs e)
		{
			queue_start(6);
		}

		private void s7_Click(object sender, System.EventArgs e)
		{
			queue_start(7);	
		}

		private void s8_Click(object sender, System.EventArgs e)
		{
			queue_start(8);
		}

		private void s9_Click(object sender, System.EventArgs e)
		{
			queue_start(9);
		}

		private void cbMorse_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) editit();
		}


		private void s1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(1);
		}
		private void s2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(2);
		}
		private void s3_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(3);
		}
		private void s4_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(4);
		}
		private void s5_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(5);
		}
		private void s6_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(6);
		}
		private void s7_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(7);
		}
		private void s8_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(8);
		}
		private void s9_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Right)) msg2keys(9);
		}

		// stop button clicked
		private void stopButton_Click(object sender, System.EventArgs e)
		{
			clear_show();
			quit = true;
			kquit = true;
		}
		private void udWPM_ValueChanged(object sender, System.EventArgs e)
		{
			cwxwpm = (int)udWPM.Value;
			setup_timer();
		}
		private void udWPM_LostFocus(object sender, EventArgs e)
		{
			udWPM_ValueChanged(sender, e);
		}

		private void udDelay_ValueChanged(object sender, System.EventArgs e)
		{
			tpause = (int)udDelay.Value * 1000;
			if (tpause < 1) tpause = tel;
		}
		private void udDelay_LostFocus(object sender, EventArgs e)
		{
			udDelay_ValueChanged(sender, e);
		}

		private void udDrop_ValueChanged(object sender, System.EventArgs e)
		{
			ttdel = (int)udDrop.Value;
		}
		private void udDrop_LostFocus(object sender, EventArgs e)
		{
			udDrop_ValueChanged(sender, e);
		}

		private void udPtt_ValueChanged(object sender, System.EventArgs e)
		{
			pttdelay = (int)udPtt.Value;
			//udDrop.Minimum = pttdelay + pttdelay/2;
		}
		private void udPtt_LostFocus(object sender, EventArgs e)
		{
			udPtt_ValueChanged(sender, e);
		}

		private void CWX_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			label5.Text = ((int)e.X + " " + (int)e.Y);	// a tool for screen coords
		}

		private void CWX_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			show_keys();		// since this is not a regular control
		}

		private void chkPause_CheckedChanged(object sender, System.EventArgs e)
		{
			pause_checked = chkPause.Checked;
		}

		private void chkAlwaysOnTop_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkAlwaysOnTop.Checked)
			{
				Win32.SetWindowPos(this.Handle.ToInt32(),
					-1, this.Left, this.Top, this.Width, this.Height, 0);
			}
			else
			{
				Win32.SetWindowPos(this.Handle.ToInt32(),
					-2, this.Left, this.Top, this.Width, this.Height, 0);
			}
		}

		#endregion

		#region keyboard graphic display generator

		private void clear_keys()
		{
			int i;

			keydisplay.WaitOne();
			for (i = 0; i < NKEYS; i++)
			{
				kbufnew.SetValue(EMPTY_CODE,i);
				kbufold.SetValue(EMPTY_CODE,i);
			}
			keydisplay.ReleaseMutex();
		}

		private void show_keys()
		{
			string s;
			int i;
			int x, y, dx, dy;
			int kyrx = kylx + kyxsz + 1;
			int kyby = kyty + kyysz + 1;
			
			lock(this)
			{
				y = kyty + 2;
				dx = 11; dy = 19;

				System.Drawing.Graphics formGraphics = this.CreateGraphics();
		
				System.Drawing.Font drawFont = new System.Drawing.Font("Courier New", 14,FontStyle.Bold);
				System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
				System.Drawing.SolidBrush grayBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray);
				System.Drawing.SolidBrush whiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
				formGraphics.FillRectangle(whiteBrush, new Rectangle(kylx,kyty,kyxsz+1,kyysz+1));

				// draw a box around the area
				Pen myPen = new Pen(Color.Gray, 1);
				formGraphics.DrawLine(myPen, kylx,kyty,kyrx,kyty);
				formGraphics.DrawLine(myPen, kyrx,kyty,kyrx,kyby);
				formGraphics.DrawLine(myPen, kyrx,kyby,kylx,kyby);
				formGraphics.DrawLine(myPen, kylx,kyby,kylx,kyty);
				myPen.Dispose();

				keydisplay.WaitOne();
				x = kylx;
				for (i = 0; i < NKEYS; i++)
				{
					s = kbufold.GetValue(i).ToString();
					formGraphics.DrawString(s, drawFont, grayBrush, (float)x, (float)y);
					if ((i % NKPL) == (NKPL-1))
					{
						x = kylx;  y += dy;
					}
					else x += dx;
				}
			
				x = kylx;
				for (i = 0; i < NKEYS; i++)
				{
					s = kbufnew.GetValue(i).ToString();
					formGraphics.DrawString(s, drawFont, drawBrush, (float)x, (float)y);
					if ((i % NKPL) == (NKPL-1))
					{
						x = kylx;  y += dy;
					}
					else x += dx;
				}
				keydisplay.ReleaseMutex();

				drawFont.Dispose();
				whiteBrush.Dispose();
				drawBrush.Dispose();
				grayBrush.Dispose();
				formGraphics.Dispose();
			}
		}

		private void clearButton_Click(object sender, System.EventArgs e)
		{
			clear_show();
		}
		private void clear_show()
		{
			clear_keys();
			show_keys();
		}
	

		#endregion

		#region Morse code definition editor interface

		public string editline;		// for passing definition line to the editor
	
		private void editit()
		{
			clear_show();
			quitshut();
			
			editline = cbMorse.Text;
#if(CWX_DEBUG)
			Debug.WriteLine(editline);
			Debug.WriteLine(editline.Length);
#endif
			if (editline[5] == '*') //.Substring(5,1).Equals("*"))
			{
				MessageBox.Show("Definitions that start with '*' cannot be edited");
			}
			else if (editline.Length != 26)
			{
				MessageBox.Show("Selected line has invalid length");
			}
			else
			{
				cwedit cweditDialog = new cwedit(console);
				cweditDialog.ShowDialog();
#if(CWX_DEBUG)
				Debug.WriteLine(editline);
				Debug.WriteLine(editline.Length);
#endif
				if (editline.Length == 26) insert_and_reload(editline);
				else if (editline.Length > 0)
				{
					MessageBox.Show("Edited line has invalid length and is not saved.");
				}
			}
		}

		private void insert_and_reload(string s)	// replace line s in a2m2 with s then re-write file
		{
			int id;
#if true	
			//id = int.Parse("q");		// see an exception here
			id = int.Parse(s.Substring(0,2));
			id -= 32;
			if (id < 0 || id > 63)
			{
				MessageBox.Show("Edited line cannot be found in a2m2.");
				return;
			}
			a2m2[id] = s;		// replace with the new lines
			write_a2m2();		// write new files
			load_alpha();		// read it back in
			build_mbits2();		// and rebuild the mbits array
#else
			for (id = 0; id < 64; id++)		// find the old one in a2m2
			{
				if (a2m2[id].StartsWith(s.Substring(0,2)))
				{
					a2m2[id] = s;		// replace with the new lines
					write_a2m2();		// write new files
					load_alpha();		// read it back in
					build_mbits2();		// and rebuild the mbits array
					return;
				}
			}
			MessageBox.Show("Edited line cannot be found in a2m2.");
#endif
		}
		private void write_a2m2()
		{
            if (File.Exists(console.AppDataPath + "\\" + sfile))
                File.Delete(console.AppDataPath + "\\" + sfile);			// out withe the old
            using (StreamWriter sw = new StreamWriter(console.AppDataPath + "\\" + sfile))	// and in with the new
			{
				for (int i = 0; i < 64; i++)
				{
					sw.WriteLine(a2m2[i]);
				}
			}
		}
		#endregion

		#region where all the fun work is done

		// the following three procedures are the only non-gui thread sections
		// process_element() is called by the mmtimer at element rate
		// keyboardFifo() pops keystrokes and set into Morse elements
		// keyboardDisplay() watches and keeps the keyboard display going
		// all three are started by the constructor and killed in CWX_Closing


		// process_element is called at the element rate (width of dot)
		// and pulls commands out of the element fifo.  These are basically
		// to determine the state of the key during the next element time.
		// The timeout pause is also processed here.

		private void process_element()		// called at the element rate
		{
			byte data;
				
			if (quit)		// shut 'er all down
			{
				quitshut();
				quit = false;
				return;
			}
			if (newptt > 0)
			{
				newptt--;
				ttx = ttdel/tel;
				if (newptt > 0) return;
//				Debug.WriteLine("newppt delay over");
				setkey(true);				// this was the defered key down
				return;
			}
			if (pause > 0)	// time out the pause
			{
				pause--;
				if (pause > 0) return;		// not yet done
				// pause ended, load 'er up again
				loadmsg(tqq);
				push_fifo(EL_END);			// end
				return;
			}
			while (true)		// used while for control; only once ever thru
			{
				if (infifo < 1) break;
				data = pop_fifo();
				if (data == EL_UNDERFLOW) return;	// underflow
				if (data == EL_END)		// end command
				{
					quitshut();
					return;
				}
				if (data == EL_PAUSE)		// pause command
				{
					ttx = 0;
					pause = tpause/tel;
					if (pause < 1) pause = tel;
					break;
				}
				if (data == EL_PTT)		// ptt only command
				{
					setptt(true);
					ttx = ttdel/tel;
				}
				if ((data == EL_KEYDOWN) || (data == EL_KEYUP))		// key command
				{
					if (data == EL_KEYDOWN)	// key down?
					{
						if (!ptt)	// we're gonna need a ptt->key delay setup
						{
							newptt = pttdelay / tel;
//							Debug.WriteLine("start newptt");
						}
						setptt(true);
						ttx = ttdel/tel;
						if (newptt > 0) return;		// the key will get pressed after newptt
						setkey(true);
					}
					else
					{
						setkey(false);
						break;
					}
				}
				return;		// ignore all others
			}
			// X on flow
			if (ttx > 0) ttx--;			// time out timer down one element
			if (ttx > 0) return;		// not yet timed out
			setptt(false);			// cw timer timed out
			setkey(false);
		}

		// keyboardFifo pops keys from fifo2 and then calls loadchar() to
		// convert to Morse elements in infifo.  The routine will sleep until
		// most of the Morse character has been output by watching infifo.

		private void keyboardFifo()		// thread to watch the keyboard fifo
		{
			byte b;
			char c;

			while (!stopThreads)
			{
				if (infifo2 > 0)
				{
					b = pop_fifo2();
					if ((b >= 'a') && (b <= 'z')) b = (byte)((int)b - 'a' + 'A');
					c = (char)b;
			
					if (kquit)	// escape
					{
						clear_fifo2();
						kquit = false;
					}
					//else 
					if (b >= 32)
					{
						loadchar(c);
						while (infifo > 2)
						{
							Thread.Sleep(10);
						}
					}
					//Thread.Sleep(1000);
				}
				else Thread.Sleep(20);	// this was originally 10 ms
			}
		}

		// the keyboardDisplay() thread pulls keys from the left hand edge (top)
		// of the keyboard display and stuffs them into fifo2 then causes the display
		// to be updated.  The keys are put into the display buffer by the keystroke event
		// handler.

		private void keyboardDisplay()		// watch and maintain the the keyboard display
		{
			char topkey;
			int i;

			while (!stopThreads)
			{
				while (pause_checked) Thread.Sleep(100);

				if ((topkey = (char)kbufnew.GetValue(0)) != EMPTY_CODE)
				{
					keydisplay.WaitOne();
					// shift old left one and insert topkey at the right
					for (i = 0; i < (NKEYS-1); i++)
					{
						kbufold.SetValue(kbufold.GetValue(i+1),i);
					}
					kbufold.SetValue(topkey,NKEYS-1);
					// shift new left one and insert EMPTY_CODE at the right
					for (i = 0; i < (NKEYS-1); i++)
					{
						kbufnew.SetValue(kbufnew.GetValue(i+1),i);
					}
					kbufnew.SetValue(EMPTY_CODE,NKEYS-1);
					keydisplay.ReleaseMutex();
					push_fifo2((byte)topkey);
					show_keys();
					while (infifo > 0) Thread.Sleep(10);
					// somehow wait here 'till character has been sent
				}
				else Thread.Sleep(20);		// this was originally 10 ms
			}
		}

		private void queue_start(int qmsg)			// queue message n for start
		{
			if(console.RX1DSPMode != DSPMode.CWL &&
				console.RX1DSPMode != DSPMode.CWU)
			{
				MessageBox.Show("Console is not in CW mode.  Please switch to either CWL or CWU and try again.",
					"CWX Error: Wrong Mode",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			kquit = true;
			quit = true;
			while (quit) Thread.Sleep(10);

			switch (qmsg)
			{
				case 1: tqq = txt1.Text; break;
				case 2: tqq = txt2.Text; break;
				case 3: tqq = txt3.Text; break;
				case 4: tqq = txt4.Text; break;
				case 5: tqq = txt5.Text; break;
				case 6: tqq = txt6.Text; break;
				case 7: tqq = txt7.Text; break;
				case 8: tqq = txt8.Text; break;
				case 9: tqq = txt9.Text; break;
				default: tqq = "?bad msg?"; break;
			}

			loadmsg(tqq);
			push_fifo(0x4);			// end
			
		}

		private void loadchar(char cc)	// convert and load a single character
		{		// this is the guts of loadmsg and work much the same way
			uint v,n;
			int ic;

			v = 0;
			ic = cc - ' ';

			if ((cc >= 'a') && (cc <= 'z')) ic -= 32;	// toupper case

			if (ic < 0 || ic > 63) return;		// ignore bad codes

			ic &= 0x3f;		// isolate the 0-63 code  (we did that in the previous line>)


			if (ic == 2) return;	// ignore loop back
			if (ic == 3) return;	// ignore long dash
			if (ic == 4) return;	//  and long space

			v = (uint)mbits.GetValue(ic);	// look up code for the character
			// top 24 of v are marks, bottom 5 have count
		
			n = v & 0x1f;		// element count
			while (n > 0)	// push n marks and spaces to the element fifo
			{
				if ((v & 0x80000000) > 0) push_fifo(EL_KEYDOWN);
				else push_fifo(EL_KEYUP);
				v <<= 1;
				n--;
			}
		}

		private void loadmsg(string t)	// load string t to the element fifo
		{
			string s;
			int ii, nc,ic;
			bool npause;
			uint v, n;
			char cc;
	
			nc = t.Length;
			if (nc < 1)			// handle zero length string
			{
				t = "?";
				nc = t.Length;
			}

			s = t.ToUpper();	// convert keys to upper case

			char[] c = new char[nc+1];		// move the string
			s.CopyTo(0,c,0,nc);				//  into a char array

			nc = t.Length;
			clear_fifo();				// clear the element fifo at start of message

			// the following inserts a slight delay between ptt and 1st element
			for (int nptt = pttdelay/tel; nptt > 0; nptt--)
			{
				push_fifo(EL_PTT);	// early ptt
			}
			
			ii = 0;
			npause = false;
			while (nc > 0)
			{
				cc = (char)c.GetValue(ii);		// fetch next character

				v = 0;					// clear element builder
				ic = cc - ' ';			// convert character into
				ic &= 0x3f;				// 0-63 code

				if (ic == 2)	// set loop back
				{
					push_fifo(EL_PAUSE);		// stuff a pause code
					npause = true;
					break;					// all done
				}
				else if (ic == 3)		// a long dash
				{
					v = 0xffffff00 + 23;	// 23 marks
				}
				else if (ic == 4)		// a long space
				{
					v = 23;				// 23 spaces
				}
				else
				{
					v = (uint)mbits.GetValue(ic); // fetch Morse bit pattern for the character
				}

				// top 24 of v are marks, bottom 5 have count
		
				n = v & 0x1f;		// isolate the element count
				while (n > 0)		// push n marks/spaces to the element fifo
				{
					if ((v & 0x80000000) > 0) push_fifo(EL_KEYDOWN);
					else push_fifo(EL_KEYUP);
					v <<= 1;
					n--;
				}
	
				ii++;			// bump fetch index
				nc--;			//  and tally me banana ...
			}

			if (npause == false) push_fifo(EL_END);	// stuff an end command if no
													// pauses in the message
		}





		private void process_key(char key)	// keys from keyboardButton_KeyPress event
		{
#if(CWX_DEBUG)
			Debug.WriteLine((char)key + "key " + (int)key);
#endif

			if (key >= ' ' && key <= '~')	// a possible code
			{
				if (key >= 'a' && key <= 'z')	// convert to upper case
				{
					key -= 'a';
					key += 'A';
				}
				insert_key(key);		// insert into unsent
				show_keys();
			}
			else if (key == 8) backspace();
		}

		private void insert_key(char key)
		{
			int i;

			keydisplay.WaitOne();
			for (i = 0; i < NKEYS; i++)		// find 1st EMPTY_CODE character
			{
				if ((char)kbufnew.GetValue(i) == EMPTY_CODE)
				{
					kbufnew.SetValue(key,i);
					keydisplay.ReleaseMutex();
					return;
				}
			}
			// no empty place, put at the end
			kbufnew.SetValue(key,NKEYS-1);
			keydisplay.ReleaseMutex();
		}
	
		private void backspace()
		{
			int i;

			for (i = NKEYS-1; i >= 0; i--)		// from left find first non empty
			{
				if ((char)kbufnew.GetValue(i) != EMPTY_CODE)
				{
					kbufnew.SetValue(EMPTY_CODE,i);
					show_keys();
					return;
				}
			}
		}

	
		private void msg2keys(int nmsg)
		{
			int i, nc;
			string qq,s;
			
			char cc;
			char[] c = new char[5];
					

			switch (nmsg)
			{
				case 1: qq = txt1.Text; break;
				case 2: qq = txt2.Text; break;
				case 3: qq = txt3.Text; break;
				case 4: qq = txt4.Text; break;
				case 5: qq = txt5.Text; break;
				case 6: qq = txt6.Text; break;
				case 7: qq = txt7.Text; break;
				case 8: qq = txt8.Text; break;
				case 9: qq = txt9.Text; break;
				default: qq = "?bad msg?"; break;
			}

			insert_key(' ');
			nc = qq.Length;
			for (i = 0; i < nc; i++)
			{
				s = qq.Substring(i,1);
				s.CopyTo(0,c,0,1);
				cc = (char)c.GetValue(0);
				insert_key(cc);
			}
			show_keys();
		}

		#endregion
	
	} // end class
} // end namespace
