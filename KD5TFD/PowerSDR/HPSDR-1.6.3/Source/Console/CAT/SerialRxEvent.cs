//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General Public License
//==============================================================
//SerialRXEvent.cs
//
//==============================================================

using System;
using System.Runtime.InteropServices;

namespace SDRSerialSupportII
{
	public delegate void SerialRXEventHandler(object source, SerialRXEvent e);

	public class SerialRXEvent : EventArgs
	{
		internal byte[] buffer = null;
		internal uint count = 0;

		public SerialRXEvent(byte[] buffer, uint count)
		{
			this.buffer = buffer;
			this.count = count;
		}
	}
}
