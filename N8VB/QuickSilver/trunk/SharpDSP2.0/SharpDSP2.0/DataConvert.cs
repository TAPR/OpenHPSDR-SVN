//==============================================================
// Copyright (C) 2006 Philip A Covington, N8VB
//
// This software is licensed under the GNU General public License
//==============================================================
// Convert.cs
// Scaled conversion routines
//
// Managed Code Version
//
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDSP
{
    public class DataConvert
    {
        static public void IntToDouble(int[] input, int scaling, ref double[] output)
        {            
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (double)input[i] / (double)scaling;
            }            
        }

        static public void DoubleToInt(double[] input, int scaling, ref int[] output)
        {            
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (int)(input[i] * (double)scaling);
            }            
        }

        static public void ScaleInt(double scalefactor, ref int[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
            	input[i] = (int)((double)(input[i] / scalefactor));
            }   
        }

        static public byte[] intToByteArray(int value) 
        {
            return BitConverter.GetBytes(value);            
        }
    }
}
