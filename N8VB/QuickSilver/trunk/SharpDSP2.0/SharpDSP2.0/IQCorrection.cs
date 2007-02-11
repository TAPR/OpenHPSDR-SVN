//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//IQCorrection.cs
//IQ correction code
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct IQCorrection
    {
        #region Private members

        private int size;  //size of IQ correction buffer = blocksize

        #endregion

        #region Constructor

        public IQCorrection(int size)
        {
            this.size = size;
        }

        #endregion

        #region Public Methods

        public void DoIQCorrection(ref double[] real, ref double[] imag, Receiver state)
        {
            for (int i = 0; i < this.size; i++)
            {
                imag[i] += real[i] * state.IQPhaseValue * 0.001F;
                real[i] *= (1.0F + state.IQGainValue * 0.001F);
            }
        }

		//public void DoIQCorrection(ref double[] real, ref double[] imag, Transmitter state)
//		{
//			for (int i = 0; i < this.size; i++)
//			{
//				imag[i] += real[i] * state.IQPhaseValue * 0.001F;
//				real[i] *= (1.0F + state.IQGainValue * 0.001F);
//			}
		//}

        #endregion

    }
}
