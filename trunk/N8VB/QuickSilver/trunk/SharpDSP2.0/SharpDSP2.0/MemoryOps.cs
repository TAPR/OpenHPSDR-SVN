//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//memoryops.cs
//memory manipulation
//
//==============================================================

using System;
using System.Runtime.InteropServices;

namespace SharpDSP
{
	class MemoryOps
	{		
		#region Public Members

		public static void ZeroFloatMemory(ref double[] buffer,
		                                   int length)
		{
			for (int i=0; i<length; i++)
			{
				buffer[i] = 0.0f;
			}
			
		}
		
		public static void CopyFloatMemory(ref double[] dest, 
		                                   ref double[] src, 
		                                   int length)
		{
			for (int i=0; i<length; i++)
			{
				dest[i] = src[i];
			}			
		}

		public static void ScaleFloatMemory(ref double[] buffer,
		                                    double scalevalue, 
		                                    int length)
		{
			for (int i = 0; i < length; i++)
			{
				buffer[i] *= scalevalue;
			}
		}

		#endregion
	}
}
