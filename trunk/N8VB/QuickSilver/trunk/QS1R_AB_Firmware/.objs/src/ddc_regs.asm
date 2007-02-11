;--------------------------------------------------------
; File Created by SDCC : FreeWare ANSI-C Compiler
; Version 2.5.0 #1020 (May  8 2005)
; This file generated Wed Jan 03 15:50:41 2007
;--------------------------------------------------------
	.module ddc_regs
	.optsdcc -mmcs51 --model-small
	
;--------------------------------------------------------
; Public variables in this module
;--------------------------------------------------------
	.globl _set_COEF_PARM_4
	.globl _set_COEF_PARM_3
	.globl _set_COEF_PARM_2
	.globl _read_COEF_PARM_4
	.globl _read_COEF_PARM_3
	.globl _read_COEF_PARM_2
	.globl _set_RSRV_PARM_3
	.globl _set_RSRV_PARM_2
	.globl _read_RSRV_PARM_3
	.globl _read_RSRV_PARM_2
	.globl _set_RTAP_PARM_3
	.globl _set_RTAP_PARM_2
	.globl _read_RTAP_PARM_3
	.globl _read_RTAP_PARM_2
	.globl _set_RAO_PARM_3
	.globl _set_RAO_PARM_2
	.globl _read_RAO_PARM_3
	.globl _read_RAO_PARM_2
	.globl _set_RDEC_PARM_3
	.globl _set_RDEC_PARM_2
	.globl _read_RDEC_PARM_3
	.globl _read_RDEC_PARM_2
	.globl _set_RCR_PARM_3
	.globl _set_RCR_PARM_2
	.globl _read_RCR_PARM_3
	.globl _read_RCR_PARM_2
	.globl _set_C5DEC_PARM_3
	.globl _set_C5DEC_PARM_2
	.globl _read_C5DEC_PARM_3
	.globl _read_C5DEC_PARM_2
	.globl _set_C5SR_PARM_3
	.globl _set_C5SR_PARM_2
	.globl _read_C5SR_PARM_3
	.globl _read_C5SR_PARM_2
	.globl _set_C2DEC_PARM_3
	.globl _set_C2DEC_PARM_2
	.globl _read_C2DEC_PARM_3
	.globl _read_C2DEC_PARM_2
	.globl _set_C2SR_PARM_3
	.globl _set_C2SR_PARM_2
	.globl _read_C2SR_PARM_3
	.globl _read_C2SR_PARM_2
	.globl _set_NPHA_PARM_3
	.globl _set_NPHA_PARM_2
	.globl _read_NPHA_PARM_3
	.globl _read_NPHA_PARM_2
	.globl _set_NFRE_PARM_3
	.globl _set_NFRE_PARM_2
	.globl _read_NFRE_PARM_3
	.globl _read_NFRE_PARM_2
	.globl _set_NSCR_PARM_3
	.globl _set_NSCR_PARM_2
	.globl _read_NSCR_PARM_3
	.globl _read_NSCR_PARM_2
	.globl _set_NCR_PARM_3
	.globl _set_NCR_PARM_2
	.globl _read_NCR_PARM_3
	.globl _read_NCR_PARM_2
	.globl _set_MCR_PARM_3
	.globl _set_MCR_PARM_2
	.globl _read_MCR_PARM_3
	.globl _read_MCR_PARM_2
	.globl _bitCPLD_CS
	.globl _bitFPGA_CS
	.globl _bitALTERA_DCLK
	.globl _bitALTERA_DATA0
	.globl _bitS_CLK
	.globl _bitS_IN
	.globl _bitS_OUT
	.globl _EIPX6
	.globl _EIPX5
	.globl _EIPX4
	.globl _PI2C
	.globl _PUSB
	.globl _EIEX6
	.globl _EIEX5
	.globl _EIEX4
	.globl _EI2C
	.globl _EIUSB
	.globl _SMOD1
	.globl _ERESI
	.globl _RESI
	.globl _INT6
	.globl _CY
	.globl _AC
	.globl _F0
	.globl _RS1
	.globl _RS0
	.globl _OV
	.globl _FL
	.globl _P
	.globl _TF2
	.globl _EXF2
	.globl _RCLK
	.globl _TCLK
	.globl _EXEN2
	.globl _TR2
	.globl _C_T2
	.globl _CP_RL2
	.globl _SM01
	.globl _SM11
	.globl _SM21
	.globl _REN1
	.globl _TB81
	.globl _RB81
	.globl _TI1
	.globl _RI1
	.globl _PS1
	.globl _PT2
	.globl _PS0
	.globl _PT1
	.globl _PX1
	.globl _PT0
	.globl _PX0
	.globl _EA
	.globl _ES1
	.globl _ET2
	.globl _ES0
	.globl _ET1
	.globl _EX1
	.globl _ET0
	.globl _EX0
	.globl _SM0
	.globl _SM1
	.globl _SM2
	.globl _REN
	.globl _TB8
	.globl _RB8
	.globl _TI
	.globl _RI
	.globl _TF1
	.globl _TR1
	.globl _TF0
	.globl _TR0
	.globl _IE1
	.globl _IT1
	.globl _IE0
	.globl _IT0
	.globl _SEL
	.globl _EIP
	.globl _B
	.globl _EIE
	.globl _ACC
	.globl _EICON
	.globl _PSW
	.globl _TH2
	.globl _TL2
	.globl _RCAP2H
	.globl _RCAP2L
	.globl _T2CON
	.globl _SBUF1
	.globl _SCON1
	.globl _GPIFSGLDATLNOX
	.globl _GPIFSGLDATLX
	.globl _GPIFSGLDATH
	.globl _GPIFTRIG
	.globl _EP01STAT
	.globl _IP
	.globl _OEE
	.globl _OED
	.globl _OEC
	.globl _OEB
	.globl _OEA
	.globl _IOE
	.globl _IOD
	.globl _AUTOPTRSETUP
	.globl _EP68FIFOFLGS
	.globl _EP24FIFOFLGS
	.globl _EP2468STAT
	.globl _IE
	.globl _INT4CLR
	.globl _INT2CLR
	.globl _IOC
	.globl _AUTODAT2
	.globl _AUTOPTRL2
	.globl _AUTOPTRH2
	.globl _AUTODAT1
	.globl _APTR1L
	.globl _APTR1H
	.globl _SBUF0
	.globl _SCON0
	.globl _MPAGE
	.globl _EXIF
	.globl _IOB
	.globl _CKCON
	.globl _TH1
	.globl _TH0
	.globl _TL1
	.globl _TL0
	.globl _TMOD
	.globl _TCON
	.globl _PCON
	.globl _DPS
	.globl _DPH1
	.globl _DPL1
	.globl _DPH
	.globl _DPL
	.globl _SP
	.globl _IOA
	.globl _CPLD_GPIO
	.globl _RFFE_CNTRL
	.globl _ATTN
	.globl _DEBUG_LED_REG
	.globl _LTC2208_CNTRL_REG
	.globl _DDC_CNTRL
	.globl _DDC1_AMR
	.globl _DDC1_LAR
	.globl _DDC1_DR4
	.globl _DDC1_DR3
	.globl _DDC1_DR2
	.globl _DDC1_DR1
	.globl _DDC1_DR0
	.globl _DDC0_AMR
	.globl _DDC0_LAR
	.globl _DDC0_DR4
	.globl _DDC0_DR3
	.globl _DDC0_DR2
	.globl _DDC0_DR1
	.globl _DDC0_DR0
	.globl _EP8FIFOBUF
	.globl _EP6FIFOBUF
	.globl _EP4FIFOBUF
	.globl _EP2FIFOBUF
	.globl _EP1INBUF
	.globl _EP1OUTBUF
	.globl _EP0BUF
	.globl _CT4
	.globl _CT3
	.globl _CT2
	.globl _CT1
	.globl _USBTEST
	.globl _TESTCFG
	.globl _DBUG
	.globl _UDMACRCQUAL
	.globl _UDMACRCL
	.globl _UDMACRCH
	.globl _GPIFHOLDAMOUNT
	.globl _FLOWSTBHPERIOD
	.globl _FLOWSTBEDGE
	.globl _FLOWSTB
	.globl _FLOWHOLDOFF
	.globl _FLOWEQ1CTL
	.globl _FLOWEQ0CTL
	.globl _FLOWLOGIC
	.globl _FLOWSTATE
	.globl _GPIFABORT
	.globl _GPIFREADYSTAT
	.globl _GPIFREADYCFG
	.globl _XGPIFSGLDATLNOX
	.globl _XGPIFSGLDATLX
	.globl _XGPIFSGLDATH
	.globl _EP8GPIFTRIG
	.globl _EP8GPIFPFSTOP
	.globl _EP8GPIFFLGSEL
	.globl _EP6GPIFTRIG
	.globl _EP6GPIFPFSTOP
	.globl _EP6GPIFFLGSEL
	.globl _EP4GPIFTRIG
	.globl _EP4GPIFPFSTOP
	.globl _EP4GPIFFLGSEL
	.globl _EP2GPIFTRIG
	.globl _EP2GPIFPFSTOP
	.globl _EP2GPIFFLGSEL
	.globl _GPIFTCB0
	.globl _GPIFTCB1
	.globl _GPIFTCB2
	.globl _GPIFTCB3
	.globl _GPIFADRL
	.globl _GPIFADRH
	.globl _GPIFCTLCFG
	.globl _GPIFIDLECTL
	.globl _GPIFIDLECS
	.globl _GPIFWFSELECT
	.globl _SETUPDAT
	.globl _SUDPTRCTL
	.globl _SUDPTRL
	.globl _SUDPTRH
	.globl _EP8FIFOBCL
	.globl _EP8FIFOBCH
	.globl _EP6FIFOBCL
	.globl _EP6FIFOBCH
	.globl _EP4FIFOBCL
	.globl _EP4FIFOBCH
	.globl _EP2FIFOBCL
	.globl _EP2FIFOBCH
	.globl _EP8FIFOFLGS
	.globl _EP6FIFOFLGS
	.globl _EP4FIFOFLGS
	.globl _EP2FIFOFLGS
	.globl _EP8CS
	.globl _EP6CS
	.globl _EP4CS
	.globl _EP2CS
	.globl _EP1INCS
	.globl _EP1OUTCS
	.globl _EP0CS
	.globl _EP8BCL
	.globl _EP8BCH
	.globl _EP6BCL
	.globl _EP6BCH
	.globl _EP4BCL
	.globl _EP4BCH
	.globl _EP2BCL
	.globl _EP2BCH
	.globl _EP1INBC
	.globl _EP1OUTBC
	.globl _EP0BCL
	.globl _EP0BCH
	.globl _FNADDR
	.globl _MICROFRAME
	.globl _USBFRAMEL
	.globl _USBFRAMEH
	.globl _TOGCTL
	.globl _WAKEUPCS
	.globl _SUSPEND
	.globl _USBCS
	.globl _XAUTODAT2
	.globl _XAUTODAT1
	.globl _I2CTL
	.globl _I2DAT
	.globl _I2CS
	.globl _PORTECFG
	.globl _PORTCCFG
	.globl _PORTACFG
	.globl _INTSETUP
	.globl _INT4IVEC
	.globl _INT2IVEC
	.globl _CLRERRCNT
	.globl _ERRCNTLIM
	.globl _USBERRIRQ
	.globl _USBERRIE
	.globl _GPIFIRQ
	.globl _GPIFIE
	.globl _EPIRQ
	.globl _EPIE
	.globl _USBIRQ
	.globl _USBIE
	.globl _NAKIRQ
	.globl _NAKIE
	.globl _IBNIRQ
	.globl _IBNIE
	.globl _EP8FIFOIRQ
	.globl _EP8FIFOIE
	.globl _EP6FIFOIRQ
	.globl _EP6FIFOIE
	.globl _EP4FIFOIRQ
	.globl _EP4FIFOIE
	.globl _EP2FIFOIRQ
	.globl _EP2FIFOIE
	.globl _OUTPKTEND
	.globl _INPKTEND
	.globl _EP8ISOINPKTS
	.globl _EP6ISOINPKTS
	.globl _EP4ISOINPKTS
	.globl _EP2ISOINPKTS
	.globl _EP8FIFOPFL
	.globl _EP8FIFOPFH
	.globl _EP6FIFOPFL
	.globl _EP6FIFOPFH
	.globl _EP4FIFOPFL
	.globl _EP4FIFOPFH
	.globl _EP2FIFOPFL
	.globl _EP2FIFOPFH
	.globl _EP8AUTOINLENL
	.globl _EP8AUTOINLENH
	.globl _EP6AUTOINLENL
	.globl _EP6AUTOINLENH
	.globl _EP4AUTOINLENL
	.globl _EP4AUTOINLENH
	.globl _EP2AUTOINLENL
	.globl _EP2AUTOINLENH
	.globl _EP8FIFOCFG
	.globl _EP6FIFOCFG
	.globl _EP4FIFOCFG
	.globl _EP2FIFOCFG
	.globl _EP8CFG
	.globl _EP6CFG
	.globl _EP4CFG
	.globl _EP2CFG
	.globl _EP1INCFG
	.globl _EP1OUTCFG
	.globl _REVCTL
	.globl _REVID
	.globl _FIFOPINPOLAR
	.globl _UART230
	.globl _BPADDRL
	.globl _BPADDRH
	.globl _BREAKPT
	.globl _FIFORESET
	.globl _PINFLAGSCD
	.globl _PINFLAGSAB
	.globl _IFCONFIG
	.globl _CPUCS
	.globl _RES_WAVEDATA_END
	.globl _GPIF_WAVE_DATA
	.globl _read_MCR
	.globl _set_MCR
	.globl _read_NCR
	.globl _set_NCR
	.globl _read_NSCR
	.globl _set_NSCR
	.globl _read_NFRE
	.globl _set_NFRE
	.globl _read_NPHA
	.globl _set_NPHA
	.globl _read_C2SR
	.globl _set_C2SR
	.globl _read_C2DEC
	.globl _set_C2DEC
	.globl _read_C5SR
	.globl _set_C5SR
	.globl _read_C5DEC
	.globl _set_C5DEC
	.globl _read_RCR
	.globl _set_RCR
	.globl _read_RDEC
	.globl _set_RDEC
	.globl _read_RAO
	.globl _set_RAO
	.globl _read_RTAP
	.globl _set_RTAP
	.globl _read_RSRV
	.globl _set_RSRV
	.globl _clr_RAM
	.globl _clr_COEF
	.globl _read_COEF
	.globl _set_COEF
;--------------------------------------------------------
; special function registers
;--------------------------------------------------------
	.area RSEG    (DATA)
_IOA	=	0x0080
_SP	=	0x0081
_DPL	=	0x0082
_DPH	=	0x0083
_DPL1	=	0x0084
_DPH1	=	0x0085
_DPS	=	0x0086
_PCON	=	0x0087
_TCON	=	0x0088
_TMOD	=	0x0089
_TL0	=	0x008a
_TL1	=	0x008b
_TH0	=	0x008c
_TH1	=	0x008d
_CKCON	=	0x008e
_IOB	=	0x0090
_EXIF	=	0x0091
_MPAGE	=	0x0092
_SCON0	=	0x0098
_SBUF0	=	0x0099
_APTR1H	=	0x009a
_APTR1L	=	0x009b
_AUTODAT1	=	0x009c
_AUTOPTRH2	=	0x009d
_AUTOPTRL2	=	0x009e
_AUTODAT2	=	0x009f
_IOC	=	0x00a0
_INT2CLR	=	0x00a1
_INT4CLR	=	0x00a2
_IE	=	0x00a8
_EP2468STAT	=	0x00aa
_EP24FIFOFLGS	=	0x00ab
_EP68FIFOFLGS	=	0x00ac
_AUTOPTRSETUP	=	0x00af
_IOD	=	0x00b0
_IOE	=	0x00b1
_OEA	=	0x00b2
_OEB	=	0x00b3
_OEC	=	0x00b4
_OED	=	0x00b5
_OEE	=	0x00b6
_IP	=	0x00b8
_EP01STAT	=	0x00ba
_GPIFTRIG	=	0x00bb
_GPIFSGLDATH	=	0x00bd
_GPIFSGLDATLX	=	0x00be
_GPIFSGLDATLNOX	=	0x00bf
_SCON1	=	0x00c0
_SBUF1	=	0x00c1
_T2CON	=	0x00c8
_RCAP2L	=	0x00ca
_RCAP2H	=	0x00cb
_TL2	=	0x00cc
_TH2	=	0x00cd
_PSW	=	0x00d0
_EICON	=	0x00d8
_ACC	=	0x00e0
_EIE	=	0x00e8
_B	=	0x00f0
_EIP	=	0x00f8
;--------------------------------------------------------
; special function bits 
;--------------------------------------------------------
	.area RSEG    (DATA)
_SEL	=	0x0086
_IT0	=	0x0088
_IE0	=	0x0089
_IT1	=	0x008a
_IE1	=	0x008b
_TR0	=	0x008c
_TF0	=	0x008d
_TR1	=	0x008e
_TF1	=	0x008f
_RI	=	0x0098
_TI	=	0x0099
_RB8	=	0x009a
_TB8	=	0x009b
_REN	=	0x009c
_SM2	=	0x009d
_SM1	=	0x009e
_SM0	=	0x009f
_EX0	=	0x00a8
_ET0	=	0x00a9
_EX1	=	0x00aa
_ET1	=	0x00ab
_ES0	=	0x00ac
_ET2	=	0x00ad
_ES1	=	0x00ae
_EA	=	0x00af
_PX0	=	0x00b8
_PT0	=	0x00b9
_PX1	=	0x00ba
_PT1	=	0x00bb
_PS0	=	0x00bc
_PT2	=	0x00bd
_PS1	=	0x00be
_RI1	=	0x00c0
_TI1	=	0x00c1
_RB81	=	0x00c2
_TB81	=	0x00c3
_REN1	=	0x00c4
_SM21	=	0x00c5
_SM11	=	0x00c6
_SM01	=	0x00c7
_CP_RL2	=	0x00c8
_C_T2	=	0x00c9
_TR2	=	0x00ca
_EXEN2	=	0x00cb
_TCLK	=	0x00cc
_RCLK	=	0x00cd
_EXF2	=	0x00ce
_TF2	=	0x00cf
_P	=	0x00d0
_FL	=	0x00d1
_OV	=	0x00d2
_RS0	=	0x00d3
_RS1	=	0x00d4
_F0	=	0x00d5
_AC	=	0x00d6
_CY	=	0x00d7
_INT6	=	0x00db
_RESI	=	0x00dc
_ERESI	=	0x00dd
_SMOD1	=	0x00df
_EIUSB	=	0x00e8
_EI2C	=	0x00e9
_EIEX4	=	0x00ea
_EIEX5	=	0x00eb
_EIEX6	=	0x00ec
_PUSB	=	0x00f8
_PI2C	=	0x00f9
_EIPX4	=	0x00fa
_EIPX5	=	0x00fb
_EIPX6	=	0x00fc
_bitS_OUT	=	0x0080
_bitS_IN	=	0x0081
_bitS_CLK	=	0x0083
_bitALTERA_DATA0	=	0x00a0
_bitALTERA_DCLK	=	0x00a2
_bitFPGA_CS	=	0x00a6
_bitCPLD_CS	=	0x00a7
;--------------------------------------------------------
; overlayable register banks 
;--------------------------------------------------------
	.area REG_BANK_0	(REL,OVR,DATA)
	.ds 8
;--------------------------------------------------------
; internal ram data
;--------------------------------------------------------
	.area DSEG    (DATA)
;--------------------------------------------------------
; overlayable items in internal ram 
;--------------------------------------------------------
	.area	OSEG    (OVR,DATA)
_read_MCR_PARM_2::
	.ds 2
_read_MCR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_MCR_PARM_2::
	.ds 2
_set_MCR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_NCR_PARM_2::
	.ds 2
_read_NCR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_NCR_PARM_2::
	.ds 2
_set_NCR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_NSCR_PARM_2::
	.ds 2
_read_NSCR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_NSCR_PARM_2::
	.ds 2
_set_NSCR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_NFRE_PARM_2::
	.ds 2
_read_NFRE_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_NFRE_PARM_2::
	.ds 2
_set_NFRE_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_NPHA_PARM_2::
	.ds 2
_read_NPHA_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_NPHA_PARM_2::
	.ds 2
_set_NPHA_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_C2SR_PARM_2::
	.ds 2
_read_C2SR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_C2SR_PARM_2::
	.ds 2
_set_C2SR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_C2DEC_PARM_2::
	.ds 2
_read_C2DEC_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_C2DEC_PARM_2::
	.ds 2
_set_C2DEC_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_C5SR_PARM_2::
	.ds 2
_read_C5SR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_C5SR_PARM_2::
	.ds 2
_set_C5SR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_C5DEC_PARM_2::
	.ds 2
_read_C5DEC_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_C5DEC_PARM_2::
	.ds 2
_set_C5DEC_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_RCR_PARM_2::
	.ds 2
_read_RCR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_RCR_PARM_2::
	.ds 2
_set_RCR_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_RDEC_PARM_2::
	.ds 2
_read_RDEC_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_RDEC_PARM_2::
	.ds 2
_set_RDEC_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_RAO_PARM_2::
	.ds 2
_read_RAO_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_RAO_PARM_2::
	.ds 2
_set_RAO_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_RTAP_PARM_2::
	.ds 2
_read_RTAP_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_RTAP_PARM_2::
	.ds 2
_set_RTAP_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_read_RSRV_PARM_2::
	.ds 2
_read_RSRV_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
_set_RSRV_PARM_2::
	.ds 2
_set_RSRV_PARM_3::
	.ds 1
	.area	OSEG    (OVR,DATA)
	.area	OSEG    (OVR,DATA)
	.area	OSEG    (OVR,DATA)
_read_COEF_PARM_2::
	.ds 1
_read_COEF_PARM_3::
	.ds 2
_read_COEF_PARM_4::
	.ds 1
_read_COEF_i_1_1::
	.ds 2
	.area	OSEG    (OVR,DATA)
_set_COEF_PARM_2::
	.ds 1
_set_COEF_PARM_3::
	.ds 2
_set_COEF_PARM_4::
	.ds 1
;--------------------------------------------------------
; indirectly addressable internal ram data
;--------------------------------------------------------
	.area ISEG    (DATA)
;--------------------------------------------------------
; bit data
;--------------------------------------------------------
	.area BSEG    (BIT)
;--------------------------------------------------------
; paged external ram data
;--------------------------------------------------------
	.area PSEG    (PAG,XDATA)
;--------------------------------------------------------
; external ram data
;--------------------------------------------------------
	.area XSEG    (XDATA)
_GPIF_WAVE_DATA	=	0xe400
_RES_WAVEDATA_END	=	0xe480
_CPUCS	=	0xe600
_IFCONFIG	=	0xe601
_PINFLAGSAB	=	0xe602
_PINFLAGSCD	=	0xe603
_FIFORESET	=	0xe604
_BREAKPT	=	0xe605
_BPADDRH	=	0xe606
_BPADDRL	=	0xe607
_UART230	=	0xe608
_FIFOPINPOLAR	=	0xe609
_REVID	=	0xe60a
_REVCTL	=	0xe60b
_EP1OUTCFG	=	0xe610
_EP1INCFG	=	0xe611
_EP2CFG	=	0xe612
_EP4CFG	=	0xe613
_EP6CFG	=	0xe614
_EP8CFG	=	0xe615
_EP2FIFOCFG	=	0xe618
_EP4FIFOCFG	=	0xe619
_EP6FIFOCFG	=	0xe61a
_EP8FIFOCFG	=	0xe61b
_EP2AUTOINLENH	=	0xe620
_EP2AUTOINLENL	=	0xe621
_EP4AUTOINLENH	=	0xe622
_EP4AUTOINLENL	=	0xe623
_EP6AUTOINLENH	=	0xe624
_EP6AUTOINLENL	=	0xe625
_EP8AUTOINLENH	=	0xe626
_EP8AUTOINLENL	=	0xe627
_EP2FIFOPFH	=	0xe630
_EP2FIFOPFL	=	0xe631
_EP4FIFOPFH	=	0xe632
_EP4FIFOPFL	=	0xe633
_EP6FIFOPFH	=	0xe634
_EP6FIFOPFL	=	0xe635
_EP8FIFOPFH	=	0xe636
_EP8FIFOPFL	=	0xe637
_EP2ISOINPKTS	=	0xe640
_EP4ISOINPKTS	=	0xe641
_EP6ISOINPKTS	=	0xe642
_EP8ISOINPKTS	=	0xe643
_INPKTEND	=	0xe648
_OUTPKTEND	=	0xe649
_EP2FIFOIE	=	0xe650
_EP2FIFOIRQ	=	0xe651
_EP4FIFOIE	=	0xe652
_EP4FIFOIRQ	=	0xe653
_EP6FIFOIE	=	0xe654
_EP6FIFOIRQ	=	0xe655
_EP8FIFOIE	=	0xe656
_EP8FIFOIRQ	=	0xe657
_IBNIE	=	0xe658
_IBNIRQ	=	0xe659
_NAKIE	=	0xe65a
_NAKIRQ	=	0xe65b
_USBIE	=	0xe65c
_USBIRQ	=	0xe65d
_EPIE	=	0xe65e
_EPIRQ	=	0xe65f
_GPIFIE	=	0xe660
_GPIFIRQ	=	0xe661
_USBERRIE	=	0xe662
_USBERRIRQ	=	0xe663
_ERRCNTLIM	=	0xe664
_CLRERRCNT	=	0xe665
_INT2IVEC	=	0xe666
_INT4IVEC	=	0xe667
_INTSETUP	=	0xe668
_PORTACFG	=	0xe670
_PORTCCFG	=	0xe671
_PORTECFG	=	0xe672
_I2CS	=	0xe678
_I2DAT	=	0xe679
_I2CTL	=	0xe67a
_XAUTODAT1	=	0xe67b
_XAUTODAT2	=	0xe67c
_USBCS	=	0xe680
_SUSPEND	=	0xe681
_WAKEUPCS	=	0xe682
_TOGCTL	=	0xe683
_USBFRAMEH	=	0xe684
_USBFRAMEL	=	0xe685
_MICROFRAME	=	0xe686
_FNADDR	=	0xe687
_EP0BCH	=	0xe68a
_EP0BCL	=	0xe68b
_EP1OUTBC	=	0xe68d
_EP1INBC	=	0xe68f
_EP2BCH	=	0xe690
_EP2BCL	=	0xe691
_EP4BCH	=	0xe694
_EP4BCL	=	0xe695
_EP6BCH	=	0xe698
_EP6BCL	=	0xe699
_EP8BCH	=	0xe69c
_EP8BCL	=	0xe69d
_EP0CS	=	0xe6a0
_EP1OUTCS	=	0xe6a1
_EP1INCS	=	0xe6a2
_EP2CS	=	0xe6a3
_EP4CS	=	0xe6a4
_EP6CS	=	0xe6a5
_EP8CS	=	0xe6a6
_EP2FIFOFLGS	=	0xe6a7
_EP4FIFOFLGS	=	0xe6a8
_EP6FIFOFLGS	=	0xe6a9
_EP8FIFOFLGS	=	0xe6aa
_EP2FIFOBCH	=	0xe6ab
_EP2FIFOBCL	=	0xe6ac
_EP4FIFOBCH	=	0xe6ad
_EP4FIFOBCL	=	0xe6ae
_EP6FIFOBCH	=	0xe6af
_EP6FIFOBCL	=	0xe6b0
_EP8FIFOBCH	=	0xe6b1
_EP8FIFOBCL	=	0xe6b2
_SUDPTRH	=	0xe6b3
_SUDPTRL	=	0xe6b4
_SUDPTRCTL	=	0xe6b5
_SETUPDAT	=	0xe6b8
_GPIFWFSELECT	=	0xe6c0
_GPIFIDLECS	=	0xe6c1
_GPIFIDLECTL	=	0xe6c2
_GPIFCTLCFG	=	0xe6c3
_GPIFADRH	=	0xe6c4
_GPIFADRL	=	0xe6c5
_GPIFTCB3	=	0xe6ce
_GPIFTCB2	=	0xe6cf
_GPIFTCB1	=	0xe6d0
_GPIFTCB0	=	0xe6d1
_EP2GPIFFLGSEL	=	0xe6d2
_EP2GPIFPFSTOP	=	0xe6d3
_EP2GPIFTRIG	=	0xe6d4
_EP4GPIFFLGSEL	=	0xe6da
_EP4GPIFPFSTOP	=	0xe6db
_EP4GPIFTRIG	=	0xe6dc
_EP6GPIFFLGSEL	=	0xe6e2
_EP6GPIFPFSTOP	=	0xe6e3
_EP6GPIFTRIG	=	0xe6e4
_EP8GPIFFLGSEL	=	0xe6ea
_EP8GPIFPFSTOP	=	0xe6eb
_EP8GPIFTRIG	=	0xe6ec
_XGPIFSGLDATH	=	0xe6f0
_XGPIFSGLDATLX	=	0xe6f1
_XGPIFSGLDATLNOX	=	0xe6f2
_GPIFREADYCFG	=	0xe6f3
_GPIFREADYSTAT	=	0xe6f4
_GPIFABORT	=	0xe6f5
_FLOWSTATE	=	0xe6c6
_FLOWLOGIC	=	0xe6c7
_FLOWEQ0CTL	=	0xe6c8
_FLOWEQ1CTL	=	0xe6c9
_FLOWHOLDOFF	=	0xe6ca
_FLOWSTB	=	0xe6cb
_FLOWSTBEDGE	=	0xe6cc
_FLOWSTBHPERIOD	=	0xe6cd
_GPIFHOLDAMOUNT	=	0xe60c
_UDMACRCH	=	0xe67d
_UDMACRCL	=	0xe67e
_UDMACRCQUAL	=	0xe67f
_DBUG	=	0xe6f8
_TESTCFG	=	0xe6f9
_USBTEST	=	0xe6fa
_CT1	=	0xe6fb
_CT2	=	0xe6fc
_CT3	=	0xe6fd
_CT4	=	0xe6fe
_EP0BUF	=	0xe740
_EP1OUTBUF	=	0xe780
_EP1INBUF	=	0xe7c0
_EP2FIFOBUF	=	0xf000
_EP4FIFOBUF	=	0xf400
_EP6FIFOBUF	=	0xf800
_EP8FIFOBUF	=	0xfc00
_DDC0_DR0	=	0x6000
_DDC0_DR1	=	0x6001
_DDC0_DR2	=	0x6002
_DDC0_DR3	=	0x6003
_DDC0_DR4	=	0x6004
_DDC0_LAR	=	0x6006
_DDC0_AMR	=	0x6007
_DDC1_DR0	=	0x6010
_DDC1_DR1	=	0x6011
_DDC1_DR2	=	0x6012
_DDC1_DR3	=	0x6013
_DDC1_DR4	=	0x6014
_DDC1_LAR	=	0x6016
_DDC1_AMR	=	0x6017
_DDC_CNTRL	=	0x6020
_LTC2208_CNTRL_REG	=	0x6021
_DEBUG_LED_REG	=	0x6022
_ATTN	=	0x6023
_RFFE_CNTRL	=	0x6024
_CPLD_GPIO	=	0x6025
;--------------------------------------------------------
; external initialized ram data
;--------------------------------------------------------
	.area CSEG    (CODE)
	.area GSINIT0 (CODE)
	.area GSINIT1 (CODE)
	.area GSINIT2 (CODE)
	.area GSINIT3 (CODE)
	.area GSINIT4 (CODE)
	.area GSINIT5 (CODE)
;--------------------------------------------------------
; global & static initialisations
;--------------------------------------------------------
	.area CSEG    (CODE)
	.area GSINIT  (CODE)
	.area GSFINAL (CODE)
	.area GSINIT  (CODE)
;--------------------------------------------------------
; Home
;--------------------------------------------------------
	.area HOME    (CODE)
	.area CSEG    (CODE)
;--------------------------------------------------------
; code
;--------------------------------------------------------
	.area CSEG    (CODE)
;------------------------------------------------------------
;Allocation info for local variables in function 'read_MCR'
;------------------------------------------------------------
;buf                       Allocated with name '_read_MCR_PARM_2'
;len                       Allocated with name '_read_MCR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:17: read_MCR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_MCR
;	-----------------------------------------
_read_MCR:
	ar2 = 0x02
	ar3 = 0x03
	ar4 = 0x04
	ar5 = 0x05
	ar6 = 0x06
	ar7 = 0x07
	ar0 = 0x00
	ar1 = 0x01
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:20: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00105$
;00111$:
;	Peephole 200	removed redundant sjmp
00112$:
;src/ddc_regs.c:22: DDC0_AMR = (MCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:23: DDC0_LAR = (MCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:24: *buf == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:25: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00105$:
;src/ddc_regs.c:26: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00102$
;00113$:
;	Peephole 200	removed redundant sjmp
00114$:
;src/ddc_regs.c:27: DDC1_AMR = (MCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:28: DDC1_LAR = (MCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:29: *buf == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:30: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:31: } else return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00102$:
	mov	dpl,#0x00
00107$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_MCR'
;------------------------------------------------------------
;buf                       Allocated with name '_set_MCR_PARM_2'
;len                       Allocated with name '_set_MCR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:36: set_MCR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_MCR
;	-----------------------------------------
_set_MCR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:39: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_MCR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:41: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:43: DDC0_AMR = (MCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:44: DDC0_LAR = (MCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:45: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_MCR_PARM_2
	mov	dph,(_set_MCR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:46: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:47: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:48: DDC1_AMR = (MCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:49: DDC1_LAR = (MCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:50: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_MCR_PARM_2
	mov	dph,(_set_MCR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:51: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:53: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_NCR'
;------------------------------------------------------------
;buf                       Allocated with name '_read_NCR_PARM_2'
;len                       Allocated with name '_read_NCR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:58: read_NCR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_NCR
;	-----------------------------------------
_read_NCR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:61: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_NCR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:63: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:65: DDC0_AMR = (NCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:66: DDC0_LAR = (NCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x01
	movx	@dptr,a
;src/ddc_regs.c:67: buf[0] == (DDC0_DR0 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:68: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:69: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:70: DDC1_AMR = (NCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:71: DDC1_LAR = (NCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x01
	movx	@dptr,a
;src/ddc_regs.c:72: buf[0] == (DDC1_DR0 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:73: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:75: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_NCR'
;------------------------------------------------------------
;buf                       Allocated with name '_set_NCR_PARM_2'
;len                       Allocated with name '_set_NCR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:80: set_NCR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_NCR
;	-----------------------------------------
_set_NCR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:83: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_NCR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:85: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:87: DDC0_AMR = (NCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:88: DDC0_LAR = (NCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x01
	movx	@dptr,a
;src/ddc_regs.c:89: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_NCR_PARM_2
	mov	dph,(_set_NCR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:90: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:91: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:92: DDC1_AMR = (NCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:93: DDC1_LAR = (NCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x01
	movx	@dptr,a
;src/ddc_regs.c:94: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_NCR_PARM_2
	mov	dph,(_set_NCR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:95: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:97: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_NSCR'
;------------------------------------------------------------
;buf                       Allocated with name '_read_NSCR_PARM_2'
;len                       Allocated with name '_read_NSCR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:102: read_NSCR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_NSCR
;	-----------------------------------------
_read_NSCR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:105: if (len < 4) return 0; // 32 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x04
	add	a,_read_NSCR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:107: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:109: DDC0_AMR = (NSCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:110: DDC0_LAR = (NSCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x02
	movx	@dptr,a
;src/ddc_regs.c:111: buf[0] == (DDC0_DR0 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:112: buf[1] == (DDC0_DR1 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;src/ddc_regs.c:113: buf[2] == (DDC0_DR2 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR2
	movx	a,@dptr
;src/ddc_regs.c:114: buf[3] == (DDC0_DR3 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR3
	movx	a,@dptr
;src/ddc_regs.c:115: return 4;
;     genRet
	mov	dpl,#0x04
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:116: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:117: DDC1_AMR = (NSCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:118: DDC1_LAR = (NSCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x02
	movx	@dptr,a
;src/ddc_regs.c:119: buf[0] == (DDC1_DR0 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:120: buf[1] == (DDC1_DR1 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;src/ddc_regs.c:121: buf[2] == (DDC1_DR2 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR2
	movx	a,@dptr
;src/ddc_regs.c:122: buf[3] == (DDC1_DR3 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR3
	movx	a,@dptr
;src/ddc_regs.c:123: return 4;
;     genRet
	mov	dpl,#0x04
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:125: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_NSCR'
;------------------------------------------------------------
;buf                       Allocated with name '_set_NSCR_PARM_2'
;len                       Allocated with name '_set_NSCR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:130: set_NSCR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_NSCR
;	-----------------------------------------
_set_NSCR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:133: if (len < 4) return 0; // 32 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x04
	add	a,_set_NSCR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:135: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:137: DDC0_AMR = (NSCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:138: DDC0_LAR = (NSCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x02
	movx	@dptr,a
;src/ddc_regs.c:139: DDC0_DR3 = (buf[3] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x03
	add	a,_set_NSCR_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NSCR_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR3
	movx	@dptr,a
;src/ddc_regs.c:140: DDC0_DR2 = (buf[2] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x02
	add	a,_set_NSCR_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NSCR_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
;src/ddc_regs.c:141: DDC0_DR1 = (buf[1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
	add	a,_set_NSCR_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NSCR_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/ddc_regs.c:142: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_NSCR_PARM_2
	mov	dph,(_set_NSCR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:143: return 4;
;     genRet
	mov	dpl,#0x04
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:144: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:145: DDC1_AMR = (NSCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:146: DDC1_LAR = (NSCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x02
	movx	@dptr,a
;src/ddc_regs.c:147: DDC1_DR3 = (buf[3] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x03
	add	a,_set_NSCR_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NSCR_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR3
	movx	@dptr,a
;src/ddc_regs.c:148: DDC1_DR2 = (buf[2] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x02
	add	a,_set_NSCR_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NSCR_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
;src/ddc_regs.c:149: DDC1_DR1 = (buf[1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
	add	a,_set_NSCR_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NSCR_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/ddc_regs.c:150: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_NSCR_PARM_2
	mov	dph,(_set_NSCR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:151: return 4;
;     genRet
	mov	dpl,#0x04
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:153: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_NFRE'
;------------------------------------------------------------
;buf                       Allocated with name '_read_NFRE_PARM_2'
;len                       Allocated with name '_read_NFRE_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:159: read_NFRE (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_NFRE
;	-----------------------------------------
_read_NFRE:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:162: if (len < 4) return 0; // 32 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x04
	add	a,_read_NFRE_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:164: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:166: DDC0_AMR = (NFRE & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:167: DDC0_LAR = (NFRE & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:168: buf[0] == (DDC0_DR0 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:169: buf[1] == (DDC0_DR1 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;src/ddc_regs.c:170: buf[2] == (DDC0_DR2 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR2
	movx	a,@dptr
;src/ddc_regs.c:171: buf[3] == (DDC0_DR3 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR3
	movx	a,@dptr
;src/ddc_regs.c:172: return 4;
;     genRet
	mov	dpl,#0x04
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:173: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:174: DDC1_AMR = (NFRE & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:175: DDC1_LAR = (NFRE & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:176: buf[0] == (DDC1_DR0 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:177: buf[1] == (DDC1_DR1 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;src/ddc_regs.c:178: buf[2] == (DDC1_DR2 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR2
	movx	a,@dptr
;src/ddc_regs.c:179: buf[3] == (DDC1_DR3 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR3
	movx	a,@dptr
;src/ddc_regs.c:180: return 4;
;     genRet
	mov	dpl,#0x04
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:182: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_NFRE'
;------------------------------------------------------------
;buf                       Allocated with name '_set_NFRE_PARM_2'
;len                       Allocated with name '_set_NFRE_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:187: set_NFRE (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_NFRE
;	-----------------------------------------
_set_NFRE:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:190: if (len < 4) return 0; // 32 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x04
	add	a,_set_NFRE_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:192: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:194: DDC0_AMR = (NFRE & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:195: DDC0_LAR = (NFRE & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:196: DDC0_DR3 = (buf[3] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x03
	add	a,_set_NFRE_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NFRE_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR3
	movx	@dptr,a
;src/ddc_regs.c:197: DDC0_DR2 = (buf[2] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x02
	add	a,_set_NFRE_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NFRE_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
;src/ddc_regs.c:198: DDC0_DR1 = (buf[1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
	add	a,_set_NFRE_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NFRE_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/ddc_regs.c:199: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_NFRE_PARM_2
	mov	dph,(_set_NFRE_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:200: return 4;
;     genRet
	mov	dpl,#0x04
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:201: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:202: DDC1_AMR = (NFRE & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:203: DDC1_LAR = (NFRE & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:204: DDC1_DR3 = (buf[3] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x03
	add	a,_set_NFRE_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NFRE_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR3
	movx	@dptr,a
;src/ddc_regs.c:205: DDC1_DR2 = (buf[2] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x02
	add	a,_set_NFRE_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NFRE_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
;src/ddc_regs.c:206: DDC1_DR1 = (buf[1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
	add	a,_set_NFRE_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NFRE_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/ddc_regs.c:207: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_NFRE_PARM_2
	mov	dph,(_set_NFRE_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:208: return 4;
;     genRet
	mov	dpl,#0x04
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:210: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_NPHA'
;------------------------------------------------------------
;buf                       Allocated with name '_read_NPHA_PARM_2'
;len                       Allocated with name '_read_NPHA_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:216: read_NPHA (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_NPHA
;	-----------------------------------------
_read_NPHA:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:219: if (len < 2) return 0; // 16 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x02
	add	a,_read_NPHA_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:221: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:223: DDC0_AMR = (NPHA & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:224: DDC0_LAR = (NPHA & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x04
	movx	@dptr,a
;src/ddc_regs.c:225: buf[0] == (DDC0_DR0 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:226: buf[1] == (DDC0_DR1 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;src/ddc_regs.c:227: return 2;
;     genRet
	mov	dpl,#0x02
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:228: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:229: DDC1_AMR = (NPHA & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:230: DDC1_LAR = (NPHA & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x04
	movx	@dptr,a
;src/ddc_regs.c:231: buf[0] == (DDC1_DR0 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:232: buf[1] == (DDC1_DR1 & 0xFF);
;     genDummyRead
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;src/ddc_regs.c:233: return 2;
;     genRet
	mov	dpl,#0x02
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:235: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_NPHA'
;------------------------------------------------------------
;buf                       Allocated with name '_set_NPHA_PARM_2'
;len                       Allocated with name '_set_NPHA_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:240: set_NPHA (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_NPHA
;	-----------------------------------------
_set_NPHA:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:243: if (len < 2) return 0; // 16 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x02
	add	a,_set_NPHA_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:245: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:247: DDC0_AMR = (NPHA & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:248: DDC0_LAR = (NPHA & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x04
	movx	@dptr,a
;src/ddc_regs.c:249: DDC0_DR1 = (buf[1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
	add	a,_set_NPHA_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NPHA_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/ddc_regs.c:250: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_NPHA_PARM_2
	mov	dph,(_set_NPHA_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:251: return 2;
;     genRet
	mov	dpl,#0x02
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:252: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:253: DDC1_AMR = (NPHA & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:254: DDC1_LAR = (NPHA & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x04
	movx	@dptr,a
;src/ddc_regs.c:255: DDC1_DR1 = (buf[1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
	add	a,_set_NPHA_PARM_2
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_set_NPHA_PARM_2 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/ddc_regs.c:256: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_NPHA_PARM_2
	mov	dph,(_set_NPHA_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:257: return 2;
;     genRet
	mov	dpl,#0x02
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:259: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_C2SR'
;------------------------------------------------------------
;buf                       Allocated with name '_read_C2SR_PARM_2'
;len                       Allocated with name '_read_C2SR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:264: read_C2SR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_C2SR
;	-----------------------------------------
_read_C2SR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:267: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_C2SR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:269: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:271: DDC0_AMR = (C2SR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:272: DDC0_LAR = (C2SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x05
	movx	@dptr,a
;src/ddc_regs.c:273: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:274: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:275: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:276: DDC1_AMR = (C2SR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:277: DDC1_LAR = (C2SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x05
	movx	@dptr,a
;src/ddc_regs.c:278: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:279: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:281: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_C2SR'
;------------------------------------------------------------
;buf                       Allocated with name '_set_C2SR_PARM_2'
;len                       Allocated with name '_set_C2SR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:286: set_C2SR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_C2SR
;	-----------------------------------------
_set_C2SR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:289: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_C2SR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:291: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:293: DDC0_AMR = (C2SR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:294: DDC0_LAR = (C2SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x05
	movx	@dptr,a
;src/ddc_regs.c:295: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_C2SR_PARM_2
	mov	dph,(_set_C2SR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:296: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:297: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:298: DDC1_AMR = (C2SR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:299: DDC1_LAR = (C2SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x05
	movx	@dptr,a
;src/ddc_regs.c:300: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_C2SR_PARM_2
	mov	dph,(_set_C2SR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:301: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:303: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_C2DEC'
;------------------------------------------------------------
;buf                       Allocated with name '_read_C2DEC_PARM_2'
;len                       Allocated with name '_read_C2DEC_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:309: read_C2DEC (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_C2DEC
;	-----------------------------------------
_read_C2DEC:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:312: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_C2DEC_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:314: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:316: DDC0_AMR = (C2DEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:317: DDC0_LAR = (C2DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x06
	movx	@dptr,a
;src/ddc_regs.c:318: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:319: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:320: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:321: DDC1_AMR = (C2DEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:322: DDC1_LAR = (C2DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x06
	movx	@dptr,a
;src/ddc_regs.c:323: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:324: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:326: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_C2DEC'
;------------------------------------------------------------
;buf                       Allocated with name '_set_C2DEC_PARM_2'
;len                       Allocated with name '_set_C2DEC_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:331: set_C2DEC (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_C2DEC
;	-----------------------------------------
_set_C2DEC:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:334: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_C2DEC_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:336: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:338: DDC0_AMR = (C2DEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:339: DDC0_LAR = (C2DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x06
	movx	@dptr,a
;src/ddc_regs.c:340: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_C2DEC_PARM_2
	mov	dph,(_set_C2DEC_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:341: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:342: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:343: DDC1_AMR = (C2DEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:344: DDC1_LAR = (C2DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x06
	movx	@dptr,a
;src/ddc_regs.c:345: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_C2DEC_PARM_2
	mov	dph,(_set_C2DEC_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:346: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:348: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_C5SR'
;------------------------------------------------------------
;buf                       Allocated with name '_read_C5SR_PARM_2'
;len                       Allocated with name '_read_C5SR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:353: read_C5SR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_C5SR
;	-----------------------------------------
_read_C5SR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:356: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_C5SR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:358: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:360: DDC0_AMR = (C5SR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:361: DDC0_LAR = (C5SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x07
	movx	@dptr,a
;src/ddc_regs.c:362: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:363: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:364: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:365: DDC1_AMR = (C5SR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:366: DDC1_LAR = (C5SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x07
	movx	@dptr,a
;src/ddc_regs.c:367: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:368: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:370: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_C5SR'
;------------------------------------------------------------
;buf                       Allocated with name '_set_C5SR_PARM_2'
;len                       Allocated with name '_set_C5SR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:375: set_C5SR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_C5SR
;	-----------------------------------------
_set_C5SR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:378: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_C5SR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:380: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:382: DDC0_AMR = (C5SR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:383: DDC0_LAR = (C5SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x07
	movx	@dptr,a
;src/ddc_regs.c:384: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_C5SR_PARM_2
	mov	dph,(_set_C5SR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:385: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:386: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:387: DDC1_AMR = (C5SR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:388: DDC1_LAR = (C5SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x07
	movx	@dptr,a
;src/ddc_regs.c:389: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_C5SR_PARM_2
	mov	dph,(_set_C5SR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:390: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:392: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_C5DEC'
;------------------------------------------------------------
;buf                       Allocated with name '_read_C5DEC_PARM_2'
;len                       Allocated with name '_read_C5DEC_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:397: read_C5DEC (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_C5DEC
;	-----------------------------------------
_read_C5DEC:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:400: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_C5DEC_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:402: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:404: DDC0_AMR = (C5DEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:405: DDC0_LAR = (C5DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x08
	movx	@dptr,a
;src/ddc_regs.c:406: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:407: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:408: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:409: DDC1_AMR = (C5DEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:410: DDC1_LAR = (C5DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x08
	movx	@dptr,a
;src/ddc_regs.c:411: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:412: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:414: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_C5DEC'
;------------------------------------------------------------
;buf                       Allocated with name '_set_C5DEC_PARM_2'
;len                       Allocated with name '_set_C5DEC_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:419: set_C5DEC (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_C5DEC
;	-----------------------------------------
_set_C5DEC:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:422: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_C5DEC_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:424: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:426: DDC0_AMR = (C5DEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:427: DDC0_LAR = (C5DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x08
	movx	@dptr,a
;src/ddc_regs.c:428: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_C5DEC_PARM_2
	mov	dph,(_set_C5DEC_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:429: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:430: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:431: DDC1_AMR = (C5DEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:432: DDC1_LAR = (C5DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x08
	movx	@dptr,a
;src/ddc_regs.c:433: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_C5DEC_PARM_2
	mov	dph,(_set_C5DEC_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:434: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:436: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_RCR'
;------------------------------------------------------------
;buf                       Allocated with name '_read_RCR_PARM_2'
;len                       Allocated with name '_read_RCR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:441: read_RCR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_RCR
;	-----------------------------------------
_read_RCR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:444: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_RCR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:446: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:448: DDC0_AMR = (RCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:449: DDC0_LAR = (RCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x09
	movx	@dptr,a
;src/ddc_regs.c:450: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:451: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:452: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:453: DDC1_AMR = (RCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:454: DDC1_LAR = (RCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x09
	movx	@dptr,a
;src/ddc_regs.c:455: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:456: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:458: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_RCR'
;------------------------------------------------------------
;buf                       Allocated with name '_set_RCR_PARM_2'
;len                       Allocated with name '_set_RCR_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:463: set_RCR (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_RCR
;	-----------------------------------------
_set_RCR:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:466: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_RCR_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:468: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:470: DDC0_AMR = (RCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:471: DDC0_LAR = (RCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x09
	movx	@dptr,a
;src/ddc_regs.c:472: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RCR_PARM_2
	mov	dph,(_set_RCR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:473: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:474: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:475: DDC1_AMR = (RCR & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:476: DDC1_LAR = (RCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x09
	movx	@dptr,a
;src/ddc_regs.c:477: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RCR_PARM_2
	mov	dph,(_set_RCR_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:478: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:480: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_RDEC'
;------------------------------------------------------------
;buf                       Allocated with name '_read_RDEC_PARM_2'
;len                       Allocated with name '_read_RDEC_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:486: read_RDEC (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_RDEC
;	-----------------------------------------
_read_RDEC:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:489: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_RDEC_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:491: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:493: DDC0_AMR = (RDEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:494: DDC0_LAR = (RDEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0A
	movx	@dptr,a
;src/ddc_regs.c:495: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:496: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:497: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:498: DDC1_AMR = (RDEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:499: DDC1_LAR = (RDEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0A
	movx	@dptr,a
;src/ddc_regs.c:500: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:501: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:503: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_RDEC'
;------------------------------------------------------------
;buf                       Allocated with name '_set_RDEC_PARM_2'
;len                       Allocated with name '_set_RDEC_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:508: set_RDEC (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_RDEC
;	-----------------------------------------
_set_RDEC:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:511: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_RDEC_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:513: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:515: DDC0_AMR = (RDEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:516: DDC0_LAR = (RDEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0A
	movx	@dptr,a
;src/ddc_regs.c:517: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RDEC_PARM_2
	mov	dph,(_set_RDEC_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:518: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:519: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:520: DDC1_AMR = (RDEC & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:521: DDC1_LAR = (RDEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0A
	movx	@dptr,a
;src/ddc_regs.c:522: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RDEC_PARM_2
	mov	dph,(_set_RDEC_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:523: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:525: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_RAO'
;------------------------------------------------------------
;buf                       Allocated with name '_read_RAO_PARM_2'
;len                       Allocated with name '_read_RAO_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:530: read_RAO (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_RAO
;	-----------------------------------------
_read_RAO:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:533: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_RAO_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:535: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:537: DDC0_AMR = (RAO & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:538: DDC0_LAR = (RAO & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0B
	movx	@dptr,a
;src/ddc_regs.c:539: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:540: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:541: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:542: DDC1_AMR = (RAO & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:543: DDC1_LAR = (RAO & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0B
	movx	@dptr,a
;src/ddc_regs.c:544: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:545: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:547: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_RAO'
;------------------------------------------------------------
;buf                       Allocated with name '_set_RAO_PARM_2'
;len                       Allocated with name '_set_RAO_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:552: set_RAO (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_RAO
;	-----------------------------------------
_set_RAO:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:555: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_RAO_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:557: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:559: DDC0_AMR = (RAO & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:560: DDC0_LAR = (RAO & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0B
	movx	@dptr,a
;src/ddc_regs.c:561: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RAO_PARM_2
	mov	dph,(_set_RAO_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:562: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:563: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:564: DDC1_AMR = (RAO & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:565: DDC1_LAR = (RAO & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0B
	movx	@dptr,a
;src/ddc_regs.c:566: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RAO_PARM_2
	mov	dph,(_set_RAO_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:567: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:569: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_RTAP'
;------------------------------------------------------------
;buf                       Allocated with name '_read_RTAP_PARM_2'
;len                       Allocated with name '_read_RTAP_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:574: read_RTAP (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_RTAP
;	-----------------------------------------
_read_RTAP:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:577: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_RTAP_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:579: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:581: DDC0_AMR = (RTAP & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:582: DDC0_LAR = (RTAP & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0C
	movx	@dptr,a
;src/ddc_regs.c:583: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:584: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:585: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:586: DDC1_AMR = (RTAP & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:587: DDC1_LAR = (RTAP & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0C
	movx	@dptr,a
;src/ddc_regs.c:588: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:589: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:591: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_RTAP'
;------------------------------------------------------------
;buf                       Allocated with name '_set_RTAP_PARM_2'
;len                       Allocated with name '_set_RTAP_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:596: set_RTAP (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_RTAP
;	-----------------------------------------
_set_RTAP:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:599: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_RTAP_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:601: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:603: DDC0_AMR = (RTAP & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:604: DDC0_LAR = (RTAP & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0C
	movx	@dptr,a
;src/ddc_regs.c:605: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RTAP_PARM_2
	mov	dph,(_set_RTAP_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:606: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:607: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:608: DDC1_AMR = (RTAP & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:609: DDC1_LAR = (RTAP & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0C
	movx	@dptr,a
;src/ddc_regs.c:610: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RTAP_PARM_2
	mov	dph,(_set_RTAP_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:611: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:613: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_RSRV'
;------------------------------------------------------------
;buf                       Allocated with name '_read_RSRV_PARM_2'
;len                       Allocated with name '_read_RSRV_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:618: read_RSRV (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function read_RSRV
;	-----------------------------------------
_read_RSRV:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:621: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_read_RSRV_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:623: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:625: DDC0_AMR = (RSRV & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:626: DDC0_LAR = (RSRV & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0D
	movx	@dptr,a
;src/ddc_regs.c:627: buf[0] == DDC0_DR0;
;     genDummyRead
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;src/ddc_regs.c:628: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:629: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:630: DDC1_AMR = (RSRV & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:631: DDC1_LAR = (RSRV & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0D
	movx	@dptr,a
;src/ddc_regs.c:632: buf[0] == DDC1_DR0;
;     genDummyRead
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;src/ddc_regs.c:633: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:635: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_RSRV'
;------------------------------------------------------------
;buf                       Allocated with name '_set_RSRV_PARM_2'
;len                       Allocated with name '_set_RSRV_PARM_3'
;index                     Allocated to registers r2 
;------------------------------------------------------------
;src/ddc_regs.c:640: set_RSRV (unsigned char index, xdata unsigned char *buf,
;	-----------------------------------------
;	 function set_RSRV
;	-----------------------------------------
_set_RSRV:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:643: if (len < 1) return 0; // 8 bits
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x01
	add	a,_set_RSRV_PARM_3
	jc	00102$
00113$:
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:645: if (index == 0)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;src/ddc_regs.c:647: DDC0_AMR = (RSRV & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:648: DDC0_LAR = (RSRV & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0D
	movx	@dptr,a
;src/ddc_regs.c:649: DDC0_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RSRV_PARM_2
	mov	dph,(_set_RSRV_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r3,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:650: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;src/ddc_regs.c:651: } else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00107$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;src/ddc_regs.c:652: DDC1_AMR = (RSRV & 0xFF00) >> 8;
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/ddc_regs.c:653: DDC1_LAR = (RSRV & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0D
	movx	@dptr,a
;src/ddc_regs.c:654: DDC1_DR0 = (buf[0] & 0xFF);
;     genAssign
	mov	dpl,_set_RSRV_PARM_2
	mov	dph,(_set_RSRV_PARM_2 + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:655: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:657: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00107$:
	mov	dpl,#0x00
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'clr_RAM'
;------------------------------------------------------------
;index                     Allocated to registers r2 
;i                         Allocated to registers r3 r4 
;------------------------------------------------------------
;src/ddc_regs.c:663: clr_RAM (unsigned char index)
;	-----------------------------------------
;	 function clr_RAM
;	-----------------------------------------
_clr_RAM:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:667: if (index == 0)  {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00104$
;00122$:
;	Peephole 200	removed redundant sjmp
00123$:
;src/ddc_regs.c:668: for (i=0; i < 0xFF; i++) {
;     genAssign
	mov	r3,#0x00
	mov	r4,#0x00
00106$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r3
	subb	a,#0xFF
	mov	a,r4
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00109$
00124$:
;src/ddc_regs.c:669: DDC0_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
;     genPlus
;     genPlus shortcut
	mov	ar5,r3
	mov	a,#0x01
;	Peephole 236.a	used r4 instead of ar4
	add	a,r4
	mov	r6,a
;     genAnd
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
;	peephole 177.e	removed redundant move
	mov	ar5,r6
	mov	r6,#0x00
;     genCast
	mov	dptr,#_DDC0_AMR
	mov	a,r5
	movx	@dptr,a
;src/ddc_regs.c:670: DDC0_LAR = ((DDC_RAM + i) & 0xFF);
;     genCast
	mov	dptr,#_DDC0_LAR
	mov	a,r3
	movx	@dptr,a
;src/ddc_regs.c:671: DDC0_DR4 = 0;
;     genAssign
	mov	dptr,#_DDC0_DR4
;	Peephole 181	changed mov to clr
;src/ddc_regs.c:672: DDC0_DR3 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;src/ddc_regs.c:673: DDC0_DR2 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;src/ddc_regs.c:674: DDC0_DR1 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;	Peephole 219.a	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_DDC0_DR3
	movx	@dptr,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/ddc_regs.c:675: DDC0_DR0 = 0;
;     genAssign
	mov	dptr,#_DDC0_DR0
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:668: for (i=0; i < 0xFF; i++) {
;     genPlus
;     genPlusIncr
	inc	r3
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 243	avoided branch to sjmp
	cjne	r3,#0x00,00106$
	inc	r4
00125$:
	sjmp	00106$
00109$:
;src/ddc_regs.c:677: return (unsigned char)i;
;     genCast
	mov	dpl,r3
;     genRet
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00104$:
;src/ddc_regs.c:679: else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00105$
;00126$:
;	Peephole 200	removed redundant sjmp
00127$:
;src/ddc_regs.c:680: for (i=0; i < 0xFF; i++) {
;     genAssign
	mov	r2,#0x00
	mov	r3,#0x00
00110$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,#0xFF
	mov	a,r3
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00113$
00128$:
;src/ddc_regs.c:681: DDC1_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
;     genPlus
;     genPlus shortcut
	mov	ar4,r2
	mov	a,#0x01
;	Peephole 236.a	used r3 instead of ar3
	add	a,r3
	mov	r5,a
;     genAnd
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
;	peephole 177.e	removed redundant move
	mov	ar4,r5
	mov	r5,#0x00
;     genCast
	mov	dptr,#_DDC1_AMR
	mov	a,r4
	movx	@dptr,a
;src/ddc_regs.c:682: DDC1_LAR = ((DDC_RAM + i) & 0xFF);
;     genCast
	mov	dptr,#_DDC1_LAR
	mov	a,r2
	movx	@dptr,a
;src/ddc_regs.c:683: DDC1_DR4 = 0;
;     genAssign
	mov	dptr,#_DDC1_DR4
;	Peephole 181	changed mov to clr
;src/ddc_regs.c:684: DDC1_DR3 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;src/ddc_regs.c:685: DDC1_DR2 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;src/ddc_regs.c:686: DDC1_DR1 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;	Peephole 219.a	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_DDC1_DR3
	movx	@dptr,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/ddc_regs.c:687: DDC1_DR0 = 0;
;     genAssign
	mov	dptr,#_DDC1_DR0
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:680: for (i=0; i < 0xFF; i++) {
;     genPlus
;     genPlusIncr
	inc	r2
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 243	avoided branch to sjmp
	cjne	r2,#0x00,00110$
	inc	r3
00129$:
	sjmp	00110$
00113$:
;src/ddc_regs.c:689: return (unsigned char)i;
;     genCast
	mov	dpl,r2
;     genRet
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:691: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00105$:
	mov	dpl,#0x00
00114$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'clr_COEF'
;------------------------------------------------------------
;index                     Allocated to registers r2 
;i                         Allocated to registers r3 r4 
;------------------------------------------------------------
;src/ddc_regs.c:697: clr_COEF (unsigned char index)
;	-----------------------------------------
;	 function clr_COEF
;	-----------------------------------------
_clr_COEF:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:701: if (index == 0)  {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00104$
;00122$:
;	Peephole 200	removed redundant sjmp
00123$:
;src/ddc_regs.c:702: for (i=0; i < 0xFF; i++) {
;     genAssign
	mov	r3,#0x00
	mov	r4,#0x00
00106$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r3
	subb	a,#0xFF
	mov	a,r4
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00109$
00124$:
;src/ddc_regs.c:703: DDC0_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
;     genAnd
	mov	r5,#0x00
	mov	ar6,r4
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar5,r6
	mov	r6,#0x00
;     genCast
	mov	dptr,#_DDC0_AMR
	mov	a,r5
	movx	@dptr,a
;src/ddc_regs.c:704: DDC0_LAR = ((DDC_COEF + i) & 0xFF);
;     genCast
	mov	dptr,#_DDC0_LAR
	mov	a,r3
	movx	@dptr,a
;src/ddc_regs.c:705: DDC0_DR4 = 0;
;     genAssign
	mov	dptr,#_DDC0_DR4
;	Peephole 181	changed mov to clr
;src/ddc_regs.c:706: DDC0_DR3 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;src/ddc_regs.c:707: DDC0_DR2 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;src/ddc_regs.c:708: DDC0_DR1 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;	Peephole 219.a	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_DDC0_DR3
	movx	@dptr,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/ddc_regs.c:709: DDC0_DR0 = 0;
;     genAssign
	mov	dptr,#_DDC0_DR0
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:702: for (i=0; i < 0xFF; i++) {
;     genPlus
;     genPlusIncr
	inc	r3
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 243	avoided branch to sjmp
	cjne	r3,#0x00,00106$
	inc	r4
00125$:
	sjmp	00106$
00109$:
;src/ddc_regs.c:711: return (unsigned char)i;
;     genCast
	mov	dpl,r3
;     genRet
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00104$:
;src/ddc_regs.c:713: else if (index == 1) {
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00105$
;00126$:
;	Peephole 200	removed redundant sjmp
00127$:
;src/ddc_regs.c:714: for (i=0; i < 0xFF; i++) {
;     genAssign
	mov	r2,#0x00
	mov	r3,#0x00
00110$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,#0xFF
	mov	a,r3
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00113$
00128$:
;src/ddc_regs.c:715: DDC1_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
;     genAnd
	mov	r4,#0x00
	mov	ar5,r3
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r5
	mov	r5,#0x00
;     genCast
	mov	dptr,#_DDC1_AMR
	mov	a,r4
	movx	@dptr,a
;src/ddc_regs.c:716: DDC1_LAR = ((DDC_COEF + i) & 0xFF);
;     genCast
	mov	dptr,#_DDC1_LAR
	mov	a,r2
	movx	@dptr,a
;src/ddc_regs.c:717: DDC1_DR4 = 0;
;     genAssign
	mov	dptr,#_DDC1_DR4
;	Peephole 181	changed mov to clr
;src/ddc_regs.c:718: DDC1_DR3 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;src/ddc_regs.c:719: DDC1_DR2 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;src/ddc_regs.c:720: DDC1_DR1 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;	Peephole 219.a	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_DDC1_DR3
	movx	@dptr,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/ddc_regs.c:721: DDC1_DR0 = 0;
;     genAssign
	mov	dptr,#_DDC1_DR0
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:714: for (i=0; i < 0xFF; i++) {
;     genPlus
;     genPlusIncr
	inc	r2
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 243	avoided branch to sjmp
	cjne	r2,#0x00,00110$
	inc	r3
00129$:
	sjmp	00110$
00113$:
;src/ddc_regs.c:723: return (unsigned char)i;
;     genCast
	mov	dpl,r2
;     genRet
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:725: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00105$:
	mov	dpl,#0x00
00114$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'read_COEF'
;------------------------------------------------------------
;offset                    Allocated with name '_read_COEF_PARM_2'
;buf                       Allocated with name '_read_COEF_PARM_3'
;len                       Allocated with name '_read_COEF_PARM_4'
;index                     Allocated to registers r2 
;i                         Allocated with name '_read_COEF_i_1_1'
;j                         Allocated to registers r6 r7 
;------------------------------------------------------------
;src/ddc_regs.c:729: read_COEF (unsigned char index,
;	-----------------------------------------
;	 function read_COEF
;	-----------------------------------------
_read_COEF:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:736: if (offset > 0x0F) return 0;
;     genCmpGt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
	mov	a,_read_COEF_PARM_2
	add	a,#0xff - 0x0F
	jnc	00102$
00128$:
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:738: offset = offset << 4; // advance by 16
;     genLeftShift
;     genLeftShiftLiteral
;     genlshOne
	mov	a,_read_COEF_PARM_2
	swap	a
	anl	a,#0xf0
	mov	_read_COEF_PARM_2,a
;src/ddc_regs.c:740: if (len < 48) return 0; // need 48 bytes min (3 x 8 bytes) = 16 coeff
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x30
	add	a,_read_COEF_PARM_4
	jc	00104$
00129$:
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00104$:
;src/ddc_regs.c:742: if (index == 0)
;     genCmpEq
	cjne	r2,#0x00,00130$
	sjmp	00131$
00130$:
	ljmp	00108$
00131$:
;src/ddc_regs.c:744: for (i=0, j=0; i < 16; i++, j+=3) {
;     genAssign
	mov	r3,_read_COEF_PARM_2
;     genAssign
;     genAssign
;	Peephole 3.h	changed mov r6,#0x00 to r6,a
	clr	a
	mov	_read_COEF_i_1_1,a
	mov	(_read_COEF_i_1_1 + 1),a
	mov	r6,a
;	Peephole 3.i	changed mov r7,#0x00 to r7,a
	mov	r7,a
00110$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,_read_COEF_i_1_1
	subb	a,#0x10
	mov	a,(_read_COEF_i_1_1 + 1)
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00113$
00132$:
;src/ddc_regs.c:745: DDC0_AMR = 0;
;     genAssign
	mov	dptr,#_DDC0_AMR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:746: DDC0_LAR = ((DDC_COEF + i + offset) & 0xFF);
;     genCast
	mov	r0,_read_COEF_i_1_1
;     genPlus
	mov	dptr,#_DDC0_LAR
;	Peephole 236.g	used r3 instead of ar3
	mov	a,r3
;	Peephole 236.a	used r0 instead of ar0
	add	a,r0
	movx	@dptr,a
;src/ddc_regs.c:747: buf[j] = DDC0_DR0;
;     genPlus
;	Peephole 236.g	used r6 instead of ar6
	mov	a,r6
	add	a,_read_COEF_PARM_3
	mov	r0,a
;	Peephole 236.g	used r7 instead of ar7
	mov	a,r7
	addc	a,(_read_COEF_PARM_3 + 1)
	mov	r1,a
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r4,a
	mov	dpl,r0
	mov	dph,r1
	movx	@dptr,a
;src/ddc_regs.c:748: buf[j+1] = DDC0_DR1;
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
	mov	r4,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r7 instead of ar7
	addc	a,r7
	mov	r5,a
;     genPlus
;	Peephole 236.g	used r4 instead of ar4
	mov	a,r4
	add	a,_read_COEF_PARM_3
	mov	r4,a
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
	addc	a,(_read_COEF_PARM_3 + 1)
	mov	r5,a
;     genAssign
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r0,a
	mov	dpl,r4
	mov	dph,r5
	movx	@dptr,a
;src/ddc_regs.c:749: buf[j+2] = DDC0_DR2;
;     genPlus
;     genPlusIncr
	mov	a,#0x02
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
	mov	r4,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r7 instead of ar7
	addc	a,r7
	mov	r5,a
;     genPlus
;	Peephole 236.g	used r4 instead of ar4
	mov	a,r4
	add	a,_read_COEF_PARM_3
	mov	r4,a
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
	addc	a,(_read_COEF_PARM_3 + 1)
	mov	r5,a
;     genAssign
	mov	dptr,#_DDC0_DR2
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r0,a
	mov	dpl,r4
	mov	dph,r5
	movx	@dptr,a
;src/ddc_regs.c:744: for (i=0, j=0; i < 16; i++, j+=3) {
;     genPlus
;     genPlusIncr
	inc	_read_COEF_i_1_1
	clr	a
	cjne	a,_read_COEF_i_1_1,00133$
	inc	(_read_COEF_i_1_1 + 1)
00133$:
;     genPlus
;     genPlusIncr
	mov	a,#0x03
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
	mov	r6,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r7 instead of ar7
	addc	a,r7
	mov	r7,a
	ljmp	00110$
00113$:
;src/ddc_regs.c:751: return (unsigned char)j;
;     genCast
	mov	dpl,r6
;     genRet
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00108$:
;src/ddc_regs.c:752: } else if (index == 1) {
;     genCmpEq
	cjne	r2,#0x01,00134$
	sjmp	00135$
00134$:
	ljmp	00109$
00135$:
;src/ddc_regs.c:753: for (i=0, j=0; i < 16; i++, j+=3) {
;     genAssign
	mov	r2,_read_COEF_PARM_2
;     genAssign
	mov	r3,#0x00
	mov	r4,#0x00
;     genAssign
	mov	r5,#0x00
	mov	r6,#0x00
00114$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r3
	subb	a,#0x10
	mov	a,r4
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00117$
00136$:
;src/ddc_regs.c:754: DDC1_AMR = 0;
;     genAssign
	mov	dptr,#_DDC1_AMR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:755: DDC1_LAR = ((DDC_COEF + i + offset) & 0xFF);
;     genCast
	mov	ar7,r3
;     genPlus
	mov	dptr,#_DDC1_LAR
;	Peephole 236.g	used r2 instead of ar2
	mov	a,r2
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
	movx	@dptr,a
;src/ddc_regs.c:756: buf[j] = DDC1_DR0;
;     genPlus
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
	add	a,_read_COEF_PARM_3
	mov	r7,a
;	Peephole 236.g	used r6 instead of ar6
	mov	a,r6
	addc	a,(_read_COEF_PARM_3 + 1)
	mov	r0,a
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r1,a
	mov	dpl,r7
	mov	dph,r0
	movx	@dptr,a
;src/ddc_regs.c:757: buf[j+1] = DDC1_DR1;
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r5 instead of ar5
	add	a,r5
	mov	r7,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r6 instead of ar6
	addc	a,r6
	mov	r0,a
;     genPlus
;	Peephole 236.g	used r7 instead of ar7
	mov	a,r7
	add	a,_read_COEF_PARM_3
	mov	r7,a
;	Peephole 236.g	used r0 instead of ar0
	mov	a,r0
	addc	a,(_read_COEF_PARM_3 + 1)
	mov	r0,a
;     genAssign
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r1,a
	mov	dpl,r7
	mov	dph,r0
	movx	@dptr,a
;src/ddc_regs.c:758: buf[j+2] = DDC1_DR2;
;     genPlus
;     genPlusIncr
	mov	a,#0x02
;	Peephole 236.a	used r5 instead of ar5
	add	a,r5
	mov	r7,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r6 instead of ar6
	addc	a,r6
	mov	r0,a
;     genPlus
;	Peephole 236.g	used r7 instead of ar7
	mov	a,r7
	add	a,_read_COEF_PARM_3
	mov	r7,a
;	Peephole 236.g	used r0 instead of ar0
	mov	a,r0
	addc	a,(_read_COEF_PARM_3 + 1)
	mov	r0,a
;     genAssign
	mov	dptr,#_DDC1_DR2
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r1,a
	mov	dpl,r7
	mov	dph,r0
	movx	@dptr,a
;src/ddc_regs.c:753: for (i=0, j=0; i < 16; i++, j+=3) {
;     genPlus
;     genPlusIncr
	inc	r3
	cjne	r3,#0x00,00137$
	inc	r4
00137$:
;     genPlus
;     genPlusIncr
	mov	a,#0x03
;	Peephole 236.a	used r5 instead of ar5
	add	a,r5
	mov	r5,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r6 instead of ar6
	addc	a,r6
	mov	r6,a
	ljmp	00114$
00117$:
;src/ddc_regs.c:760: return (unsigned char)j;
;     genCast
	mov	dpl,r5
;     genRet
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:762: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00109$:
	mov	dpl,#0x00
00118$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'set_COEF'
;------------------------------------------------------------
;offset                    Allocated with name '_set_COEF_PARM_2'
;buf                       Allocated with name '_set_COEF_PARM_3'
;len                       Allocated with name '_set_COEF_PARM_4'
;index                     Allocated to registers r2 
;i                         Allocated to registers r4 r5 
;j                         Allocated to registers r6 r7 
;------------------------------------------------------------
;src/ddc_regs.c:771: set_COEF (unsigned char index,
;	-----------------------------------------
;	 function set_COEF
;	-----------------------------------------
_set_COEF:
;     genReceive
	mov	r2,dpl
;src/ddc_regs.c:778: if (offset > 0x0F)
;     genCmpGt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
	mov	a,_set_COEF_PARM_2
	add	a,#0xff - 0x0F
	jnc	00102$
00128$:
;src/ddc_regs.c:779: return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00102$:
;src/ddc_regs.c:781: offset = offset << 4; // advance by 16
;     genLeftShift
;     genLeftShiftLiteral
;     genlshOne
	mov	a,_set_COEF_PARM_2
	swap	a
	anl	a,#0xf0
	mov	_set_COEF_PARM_2,a
;src/ddc_regs.c:783: if (len < 48) return 0; // need 48 bytes min (3 x 8 bytes) = 16 coeff
;     genCmpLt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
	mov	a,#0x100 - 0x30
	add	a,_set_COEF_PARM_4
	jc	00104$
00129$:
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00104$:
;src/ddc_regs.c:785: if (index == 0)
;     genCmpEq
	cjne	r2,#0x00,00130$
	sjmp	00131$
00130$:
	ljmp	00108$
00131$:
;src/ddc_regs.c:787: for (i=0, j=0; i < 16; i++, j+=3) {
;     genAssign
	mov	r3,_set_COEF_PARM_2
;     genAssign
	mov	r4,#0x00
	mov	r5,#0x00
;     genAssign
	mov	r6,#0x00
	mov	r7,#0x00
00110$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r4
	subb	a,#0x10
	mov	a,r5
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00113$
00132$:
;src/ddc_regs.c:788: DDC0_AMR = 0;
;     genAssign
	mov	dptr,#_DDC0_AMR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:789: DDC0_LAR = ((DDC_COEF + i + offset) & 0xFF);
;     genCast
	mov	ar0,r4
;     genPlus
	mov	dptr,#_DDC0_LAR
;	Peephole 236.g	used r3 instead of ar3
	mov	a,r3
;	Peephole 236.a	used r0 instead of ar0
	add	a,r0
	movx	@dptr,a
;src/ddc_regs.c:790: DDC0_DR2 = (buf[j+2] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x02
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
	mov	r0,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r7 instead of ar7
	addc	a,r7
	mov	r1,a
;     genPlus
;	Peephole 236.g	used r0 instead of ar0
	mov	a,r0
	add	a,_set_COEF_PARM_3
	mov	dpl,a
;	Peephole 236.g	used r1 instead of ar1
	mov	a,r1
	addc	a,(_set_COEF_PARM_3 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r0,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
;src/ddc_regs.c:791: DDC0_DR1 = (buf[j+1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
	mov	r0,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r7 instead of ar7
	addc	a,r7
	mov	r1,a
;     genPlus
;	Peephole 236.g	used r0 instead of ar0
	mov	a,r0
	add	a,_set_COEF_PARM_3
	mov	dpl,a
;	Peephole 236.g	used r1 instead of ar1
	mov	a,r1
	addc	a,(_set_COEF_PARM_3 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r0,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/ddc_regs.c:792: DDC0_DR0 = (buf[j] & 0xFF);
;     genPlus
;	Peephole 236.g	used r6 instead of ar6
	mov	a,r6
	add	a,_set_COEF_PARM_3
	mov	dpl,a
;	Peephole 236.g	used r7 instead of ar7
	mov	a,r7
	addc	a,(_set_COEF_PARM_3 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r0,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/ddc_regs.c:787: for (i=0, j=0; i < 16; i++, j+=3) {
;     genPlus
;     genPlusIncr
	inc	r4
	cjne	r4,#0x00,00133$
	inc	r5
00133$:
;     genPlus
;     genPlusIncr
	mov	a,#0x03
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
	mov	r6,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r7 instead of ar7
	addc	a,r7
	mov	r7,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00110$
00113$:
;src/ddc_regs.c:794: return (unsigned char)j;
;     genCast
	mov	dpl,r6
;     genRet
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00108$:
;src/ddc_regs.c:795: } else if (index == 1) {
;     genCmpEq
	cjne	r2,#0x01,00134$
	sjmp	00135$
00134$:
	ljmp	00109$
00135$:
;src/ddc_regs.c:796: for (i=0, j=0; i < 16; i++, j+=3) {
;     genAssign
	mov	r2,_set_COEF_PARM_2
;     genAssign
	mov	r3,#0x00
	mov	r4,#0x00
;     genAssign
	mov	r5,#0x00
	mov	r6,#0x00
00114$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r3
	subb	a,#0x10
	mov	a,r4
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00117$
00136$:
;src/ddc_regs.c:797: DDC1_AMR = 0;
;     genAssign
	mov	dptr,#_DDC1_AMR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/ddc_regs.c:798: DDC1_LAR = ((DDC_COEF + i + offset) & 0xFF);
;     genCast
	mov	ar7,r3
;     genPlus
	mov	dptr,#_DDC1_LAR
;	Peephole 236.g	used r2 instead of ar2
	mov	a,r2
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
	movx	@dptr,a
;src/ddc_regs.c:799: DDC1_DR2 = (buf[j+2] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x02
;	Peephole 236.a	used r5 instead of ar5
	add	a,r5
	mov	r7,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r6 instead of ar6
	addc	a,r6
	mov	r0,a
;     genPlus
;	Peephole 236.g	used r7 instead of ar7
	mov	a,r7
	add	a,_set_COEF_PARM_3
	mov	dpl,a
;	Peephole 236.g	used r0 instead of ar0
	mov	a,r0
	addc	a,(_set_COEF_PARM_3 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r7,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
;src/ddc_regs.c:800: DDC1_DR1 = (buf[j+1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r5 instead of ar5
	add	a,r5
	mov	r7,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r6 instead of ar6
	addc	a,r6
	mov	r0,a
;     genPlus
;	Peephole 236.g	used r7 instead of ar7
	mov	a,r7
	add	a,_set_COEF_PARM_3
	mov	dpl,a
;	Peephole 236.g	used r0 instead of ar0
	mov	a,r0
	addc	a,(_set_COEF_PARM_3 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r7,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/ddc_regs.c:801: DDC1_DR0 = (buf[j] & 0xFF);
;     genPlus
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
	add	a,_set_COEF_PARM_3
	mov	dpl,a
;	Peephole 236.g	used r6 instead of ar6
	mov	a,r6
	addc	a,(_set_COEF_PARM_3 + 1)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r7,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/ddc_regs.c:796: for (i=0, j=0; i < 16; i++, j+=3) {
;     genPlus
;     genPlusIncr
	inc	r3
	cjne	r3,#0x00,00137$
	inc	r4
00137$:
;     genPlus
;     genPlusIncr
	mov	a,#0x03
;	Peephole 236.a	used r5 instead of ar5
	add	a,r5
	mov	r5,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r6 instead of ar6
	addc	a,r6
	mov	r6,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00114$
00117$:
;src/ddc_regs.c:803: return (unsigned char)j;
;     genCast
	mov	dpl,r5
;     genRet
;	Peephole 112.b	changed ljmp to sjmp
;src/ddc_regs.c:805: return 0;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00109$:
	mov	dpl,#0x00
00118$:
	ret
	.area CSEG    (CODE)
