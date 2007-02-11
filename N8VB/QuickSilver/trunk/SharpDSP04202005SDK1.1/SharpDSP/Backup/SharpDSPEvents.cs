//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General Public License
//==============================================================
//SharpDSPEvents.cs
//DSP event generator for display update
//
//==============================================================

using System;
using System.Runtime.InteropServices;

namespace SharpDSP
{
    public delegate void SharpDSPEventHandler(object source, SharpDSPEvent e);

    unsafe public class SharpDSPEvent : EventArgs
    {
        public float* buffer = null;

        public SharpDSPEvent(float* buffer)
		{
            this.buffer = buffer;
        }
    }

}