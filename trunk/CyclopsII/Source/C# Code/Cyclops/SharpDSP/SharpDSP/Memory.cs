//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//memory.cs
//memory manipulation
//
//==============================================================

/*
This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2007, 2008 Philip A Covington

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The author can be reached by email at

p.covington@gmail.com

*/

using System;
using System.Runtime.InteropServices;

namespace SharpDSP2._1
{
	unsafe public class Memory
	{
		#region Private Members

		[DllImport("msvcrt.dll", EntryPoint="memcpy")]
		private static extern void Copy_Memory(void *ptr_dest, void *ptr_src, int byte_length);

		[DllImport("msvcrt.dll", EntryPoint="memset")]
		private static extern void SetMemory(void *ptr_address, byte byte_value, int byte_length);

		#endregion

		#region Public Members

		public static void ZeroFloatMemory(void *ptr_address, int length)
		{
			SetMemory(ptr_address, 0, length * sizeof(float));			
		}
		
		public static void CopyFloatMemory(void *ptr_dest, void *ptr_src, int length)
		{
			Copy_Memory(ptr_dest, ptr_src, length * sizeof(float));			
		}

		public static void ScaleFloatMemory(float *ptr_address, float scalevalue, int length)
		{
			for (int i = 0; i < length; i++)
			{
				ptr_address[i] *= scalevalue;
			}
		}

		#endregion
	}
}