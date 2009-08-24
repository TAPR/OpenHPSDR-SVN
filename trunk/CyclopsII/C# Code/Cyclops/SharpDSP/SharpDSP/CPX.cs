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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDSP2._1
{	
	#region The Complex Type
	// The Complex Type
	// maintain alignment for SSE
	[StructLayout(LayoutKind.Explicit)]
	public struct CPX
	{		
		[FieldOffset(0)] public float real;		
		[FieldOffset(4)] public float imag;	
		
		public CPX(float real, float imag)
		{
			this.real = real;
			this.imag = imag;
		}
		
		public static CPX operator +(CPX c1, CPX c2)
		{
			return new CPX(c1.real + c2.real, c1.imag + c2.imag);
		}
		
		public static CPX operator *(CPX c1, CPX c2)
		{
			return new CPX(c1.real * c2.real, c1.imag * c2.imag);
		}

        public static CPX[] ToCPX(float[] real, float[] imag)
        {
            CPX[] mycpx = new CPX[real.Length];

            for (int i = 0; i < real.Length; i++)
            {
                mycpx[i].real = real[i];
                mycpx[i].imag = imag[i];
            }

            return mycpx;
        }

        public override string ToString()
		{
			return (System.String.Format("{0} + {1}i", real, imag));
		}
	}
	
	#endregion
}
