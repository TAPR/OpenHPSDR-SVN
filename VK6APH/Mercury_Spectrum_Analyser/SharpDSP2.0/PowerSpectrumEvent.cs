//==============================================================
// Copyright (C) 2006 Philip A Covington, N8VB
//
// This software is licensed under the GNU General public License
//==============================================================
// PowerSpectrumEvent.cs
//
//
//
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDSP
{
    public delegate void PowerSpectrumEventHandler(object source, PowerSpectrumEvent e);

    public class PowerSpectrumEvent : EventArgs
    {
        public double[] buffer;

        public PowerSpectrumEvent(ref double[] buffer)
		{
            this.buffer = buffer;
        }
    }
}
