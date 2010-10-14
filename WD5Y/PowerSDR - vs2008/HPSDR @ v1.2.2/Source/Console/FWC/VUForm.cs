//=================================================================
// VUForm.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2010  FlexRadio Systems
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
//    8900 Marybank Dr.
//    Austin, TX 78750
//    USA
//=================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerSDR
{
    public partial class VUForm : Form
    {
        public VUForm()
        {
            InitializeComponent();
        }

        private void chkFanHigh_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_FanHigh(chkVUFanHigh.Checked);
        }

        private void chkVUKeyV_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_KeyV(chkVUKeyV.Checked);
        }

        private void chkVUTXIFU_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_TXIFU(chkVUTXIFU.Checked);
        }

        private void chkVUKeyU_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_KeyU(chkVUKeyU.Checked);
        }

        private void chkVURXURX2_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_RXURX2(chkVURXURX2.Checked);
        }

        private void chkVURX2U_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_RX2U(chkVURX2U.Checked);
        }

        private void chkVURXIFU_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_RXIFU(chkVURXIFU.Checked);
        }

        private void chkVURX2V_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_RX2V(chkVURX2V.Checked);
        }

        private void chkVUTXU_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_TXU(chkVUTXU.Checked);
        }

        private void chkVUTXV_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_TXV(chkVUTXV.Checked);
        }

        private void chkVUK15_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_K15(chkVUK15.Checked);
        }

        private void chkVUK18_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_K18(chkVUK18.Checked);
        }

        private void chkVUK16_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_K16(chkVUK16.Checked);
        }

        private void chkVUKeyVU_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_KeyVU(chkVUKeyVU.Checked);
        }

        private void chkVUDrvU_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_DrvU(chkVUDrvU.Checked);
        }

        private void chkVUDrvV_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_DrvV(chkVUDrvV.Checked);
        }

        private void chkVUK12_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_K12(chkVUK12.Checked);
        }

        private void chkVUK13_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_K13(chkVUK13.Checked);
        }

        private void chkVULPwrU_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_LPwrU(chkVULPwrU.Checked);
        }

        private void chkVULPwrV_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_LPwrV(chkVULPwrV.Checked);
        }

        private void VUForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void chkVUUIFHG1_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_UIFHG1(chkVUUIFHG1.Checked);
        }

        private void chkVUVIFHG1_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_VIFHG1(chkVUVIFHG1.Checked);
        }

        private void chkVUUIFHG2_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_UIFHG2(chkVUUIFHG2.Checked);
        }

        private void chkVUVIFHG2_CheckedChanged(object sender, EventArgs e)
        {
            FWC.SetVU_VIFHG2(chkVUVIFHG2.Checked);
        }

        
    }
}
