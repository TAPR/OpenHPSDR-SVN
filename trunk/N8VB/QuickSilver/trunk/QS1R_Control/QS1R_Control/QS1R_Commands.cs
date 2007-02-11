/*
 * Created by SharpDevelop.
 * User: Phil
 * Date: 12/25/2006
 * Time: 4:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace QS1R_Control
{
	/// <summary>
	/// Description of QS1R_Commands.
	/// </summary>
	public static class QS1R_Commands
	{
		/* Vendor Request Types */
        public const int VENDOR_REQ_TYPE_IN = 0xc0;
        public const int VENDOR_REQ_TYPE_OUT = 0x40;
        
        /* Vendor OUT Requests */
        public const int DEBUG_LED_REG_WRITE = 0x01;
        public const int ADC_CONTROL_REG_WRITE = 0x0A;
        public const int ATTN_REG_WRITE = 0x0D;
        public const int RFFE_REG_WRITE = 0x0E;
        public const int DDC_CNTRL_WRITE = 0xF;
        public const int CPLD_GPIO_WRITE = 0x10;
        public const int DDC0_DR0_WRITE = 0x11;
        public const int DDC0_DR1_WRITE = 0x12;
        public const int DDC0_DR2_WRITE = 0x13;
        public const int DDC0_DR3_WRITE = 0x14;
        public const int DDC0_DR4_WRITE = 0x15;
        public const int DDC0_LAR_WRITE = 0x16;
        public const int DDC0_AMR_WRITE = 0x17;
        public const int DDC1_DR0_WRITE = 0x18;
        public const int DDC1_DR1_WRITE = 0x19;
        public const int DDC1_DR2_WRITE = 0x1A;
        public const int DDC1_DR3_WRITE = 0x1B;
        public const int DDC1_DR4_WRITE = 0x1C;
        public const int DDC1_LAR_WRITE = 0x1D;
        public const int DDC1_AMR_WRITE = 0x1E;
        public const int DDC_MCR_WRITE = 0x1F;
        public const int DDC_NCR_WRITE = 0x20;
        public const int DDC_NSCR_WRITE = 0x21;
        public const int DDC_NFRE_WRITE = 0x22;
        public const int DDC_NPO_WRITE = 0x23;
        public const int DDC_C2SR_WRITE = 0x24;
        public const int DDC_C2DEC_WRITE = 0x25;
        public const int DDC_C5SR_WRITE = 0x26;
        public const int DDC_C5DEC_WRITE = 0x27;
        public const int DDC_RCR_WRITE = 0x28;
        public const int DDC_RDEC_WRITE = 0x29;
        public const int DDC_RAO_WRITE = 0x2A;
        public const int DDC_RTAPS_WRITE = 0x2B;
        public const int DDC_RSVR_WRITE = 0x2C;
        public const int DDC_RAM_CLR = 0x2D;
        public const int DDC_COEF_CLR = 0x2E;
        public const int DDC_COEF_WRITE = 0x2F;
        
        
        /* Vendor IN Requests */
        public const int DEBUG_LED_REG_READ = 0x86;		
		public const int ADC_CONTROL_REG_READ = 0x87;
		public const int ATTN_REG_READ = 0x88;
		public const int RFFE_REG_READ = 0x89;
		public const int DDC_CNTRL_READ = 0x8A;
		public const int CPLD_GPIO_READ = 0x8B;
		public const int DDC0_DR0_READ = 0x8C;
        public const int DDC0_DR1_READ = 0x8D;
        public const int DDC0_DR2_READ = 0x8E;
        public const int DDC0_DR3_READ = 0x8F;
        public const int DDC0_DR4_READ = 0x90;
        public const int DDC0_LAR_READ = 0x91;
        public const int DDC0_AMR_READ = 0x92;
        public const int DDC1_DR0_READ = 0x93;
        public const int DDC1_DR1_READ = 0x94;
        public const int DDC1_DR2_READ = 0x95;
        public const int DDC1_DR3_READ = 0x96;
        public const int DDC1_DR4_READ = 0x97;
        public const int DDC1_LAR_READ = 0x98;
        public const int DDC1_AMR_READ = 0x99;
        public const int DDC_MCR_READ = 0x9A;
        public const int DDC_NCR_READ = 0x9B;
        public const int DDC_NSCR_READ = 0x9C;
        public const int DDC_NFRE_READ = 0x9D;
        public const int DDC_NPO_READ = 0x9E;
        public const int DDC_C2SR_READ = 0x9F;
        public const int DDC_C2DEC_READ = 0xA0;
        public const int DDC_C5SR_READ = 0xA1;
        public const int DDC_C5DEC_READ = 0xA2;
        public const int DDC_RCR_READ = 0xA3;
        public const int DDC_RDEC_READ = 0xA4;
        public const int DDC_RAO_READ = 0xA5;
        public const int DDC_RTAPS_READ = 0xA6;
        public const int DDC_RSVR_READ = 0xA7;
        public const int DDC_COEF_READ = 0xA8;
	}
}
