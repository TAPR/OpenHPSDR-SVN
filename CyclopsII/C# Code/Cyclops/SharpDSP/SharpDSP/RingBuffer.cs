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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SharpDSP2._1
{     
    public class RingBuffer
    {
        int size = 0;
        CPX[] buffer;
        int readPos = 0;
        int writePos = 0;

        public RingBuffer(int nsize)
        {
            buffer = new CPX[nsize];
            this.size = nsize;
        }

        public bool isEmpty()
        {
            if (readPos == writePos)
                return true;
            else
                return false;
        }

        public int Count
        {
            get
            {
                int len = writePos - readPos;
                if (len < 0)
                    len += size;
                return len;
            }
        }

        public void Clear()
        {
            writePos = 0;
            readPos = 0;
        }
                
        public void Write(CPX data)
        {
            buffer[writePos] = data;
            writePos = (writePos + 1) % size;            
        }

        public void Write(float real, float imag)
        {
            buffer[writePos].real = real;
            buffer[writePos].imag = imag;
            writePos = (writePos + 1) % size;
        }

        public CPX Peek()
        {
            return buffer[readPos];
        }

        public CPX Read()
        {
            int pos = readPos;
            readPos = (readPos + 1) % size;
            return buffer[pos];
        }

        public void Read(ref float real, ref float imag)
        {
            int pos = readPos;
            readPos = (readPos + 1) % size;
            real = buffer[pos].real;
            imag = buffer[pos].imag;
        }  

        public void Discard(int num)
        {
            if (Count < num)
                num = Count;

            readPos = (readPos + num) % size;
        }
    }
}