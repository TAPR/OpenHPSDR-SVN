//=================================================================
// parallel.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004, 2005, 2006  FlexRadio Systems
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
//    12100 Technology Blvd.
//    Austin, TX 78727
//    USA
//=================================================================

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

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

		[DllImport("PortTalk.dll")]
		public static extern void InitPortTalk();

		[DllImport("PortTalk.dll")]
		public static extern void ExitPortTalk();

		[DllImport("PortTalk.dll", EntryPoint="outport")]
		public static extern void outport(ushort addr, byte val);
//		public static void outport(ushort addr, byte val)
//		{
//			Debug.WriteLine("outport: "+addr.ToString("X")+"-"+val.ToString("X"));
//			outport_dll(addr, val);
//		}

		[DllImport("PortTalk.dll")]
		public static extern byte inport(ushort addr);

		#endregion
	}
}