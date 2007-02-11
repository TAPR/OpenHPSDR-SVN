//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//memory.cs
//memory manipulation
//
//==============================================================

using System;
using System.Runtime.InteropServices;

namespace SharpDSP
{
	unsafe class Memory
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