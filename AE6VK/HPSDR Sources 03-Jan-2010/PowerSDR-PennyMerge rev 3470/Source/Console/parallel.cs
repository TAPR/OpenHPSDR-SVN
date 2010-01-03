//=================================================================
// parallel.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2008  FlexRadio Systems
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
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PortTalk
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Parallel
	{
		#region Dll Variable and Method Definition
// ======================================================
// DLL Variable and Method Definition
// ======================================================

		[DllImport("PortTalk.dll", EntryPoint="InitPortTalk")]
		public static extern void InitPortTalkDll();

		private static PowerSDR.Console PowerSDRconsole = null; 

		public static void InitPortTalk()
		{
			PowerSDRconsole = PowerSDR.Console.getConsole(); 
			if ( PowerSDRconsole != null && PowerSDRconsole.neuterPP ) 
			{ 
				System.Console.WriteLine("Parallel Port force neutered"); 
				return; 
			} 
			else 
			{ 
				System.Console.WriteLine("Parallel Port live!"); 
			} 

			if(Environment.OSVersion.ToString() == "Microsoft Windows NT 5.1.2600.0")
			{
				// check for driver file and copy if needed
				if(!File.Exists(Environment.SystemDirectory+"\\drivers\\porttalk.sys"))
					File.Copy(Application.StartupPath+"\\porttalk.sys", Environment.SystemDirectory+"\\drivers\\porttalk.sys");

				// check for registry entry
				RegistryKey rk = null;
				RegistryKey hklm = Registry.LocalMachine.OpenSubKey("SYSTEM", true);
				if(hklm != null) hklm = hklm.OpenSubKey("CurrentControlSet", true);
				if(hklm != null) hklm = hklm.OpenSubKey("Services", true);
				if(hklm != null) rk = hklm.OpenSubKey("PortTalk", true);
				if(rk == null) rk = hklm.CreateSubKey("PortTalk");

				if(rk != null)
				{
					string[] names = rk.GetValueNames();
					int val;
					foreach(string s in names)
					{
						switch(s)
						{
							case "ErrorControl":
								val = (int)rk.GetValue("ErrorControl");
								if(val != 1) rk.SetValue("ErrorControl", 1);
								break;
							case "Start":
								val = (int)rk.GetValue("Start");
								if(val != 2) rk.SetValue("Start", 2);
								break;
							case "Type":
								val = (int)rk.GetValue("Type");
								if(val != 1) rk.SetValue("Type", 1);
								break;
						}
					}
				}
				else // key not found, could not create it
				{
					MessageBox.Show("Error initializing PortTalk");
				}
			}
				
			InitPortTalkDll();
		}

		[DllImport("PortTalk.dll")]
		public static extern void ExitPortTalk();

		[DllImport("PortTalk.dll", EntryPoint="outport")]
		public static extern void outport_dll(ushort addr, byte val);

		public static void outport(ushort addr, byte val)
		{
			System.Console.WriteLine("outport addr=" + addr); 
			if ( PowerSDRconsole != null && PowerSDRconsole.neuterPP ) 
			{ 
				System.Console.WriteLine("outport: parallel Port force neutered"); 
				return; 
			} 
			Debug.WriteLine("outport: "+addr.ToString("X")+"-"+val.ToString("X"));
			outport_dll(addr, val);
		}

		public static byte inport(ushort addr) 
		{
			System.Console.WriteLine("inport addr=" + addr); 
			if ( PowerSDRconsole != null && PowerSDRconsole.neuterPP ) 
			{ 
				System.Console.WriteLine("inport: parallel Port force neutered"); 
				return 0; 
			} 
			return inport_dll(addr); 
		} 

		[DllImport("PortTalk.dll", EntryPoint="inport")]
		public static extern byte inport_dll(ushort addr);

		#endregion
	}
}