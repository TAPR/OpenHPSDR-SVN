//=================================================================
// win32.cs
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

namespace PowerSDR
{
	unsafe class Win32
	{
		[DllImport("msvcrt.dll", EntryPoint="memcpy")]
		public static extern void memcpy(void *destptr, void *srcptr, int n);

		[DllImport("kernel32.dll", EntryPoint="EnterCriticalSection")]
		public static extern void EnterCriticalSection(void *cs_ptr);

		[DllImport("kernel32.dll", EntryPoint="LeaveCriticalSection")]
		public static extern void LeaveCriticalSection(void *cs_ptr);

		[DllImport("kernel32.dll", EntryPoint="InitializeCriticalSection")]
		public static extern void InitializeCriticalSection(void *cs_ptr);

		[DllImport("kernel32.dll", EntryPoint="InitializeCriticalSectionAndSpinCount")]
		public static extern int InitializeCriticalSectionAndSpinCount(void *cs_ptr, uint spincount);

		[DllImport("kernel32.dll", EntryPoint="DeleteCriticalSection")]
		public static extern byte DeleteCriticalSection(void *cs_ptr);

		[DllImport("DttSP.dll", EntryPoint="NewCriticalSection")]
		public static extern void *NewCriticalSection();

		[DllImport("DttSP.dll", EntryPoint="DestroyCriticalSection")]
		public static extern void DestroyCriticalSection(void *cs_ptr);

		[DllImport("msvcrt.dll", EntryPoint="memset")]
		public static extern void memset(void *addr, byte val, int n);

		[DllImport("user32.dll", EntryPoint="SetWindowPos")]
		public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
	}
}