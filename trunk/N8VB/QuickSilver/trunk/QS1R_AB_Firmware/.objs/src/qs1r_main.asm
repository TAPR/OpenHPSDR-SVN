;--------------------------------------------------------
; File Created by SDCC : FreeWare ANSI-C Compiler
; Version 2.5.0 #1020 (May  8 2005)
; This file generated Fri Jan 05 10:32:59 2007
;--------------------------------------------------------
	.module qs1r_main
	.optsdcc -mmcs51 --model-small
	
;--------------------------------------------------------
; Public variables in this module
;--------------------------------------------------------
	.globl _main
	.globl _patch_usb_descriptors
	.globl _isr_tick
	.globl _app_vendor_IN_cmd
	.globl _app_vendor_OUT_cmd
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
	.globl _app_vendor_cmd
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
_app_vendor_IN_cmd_offset_1_1::
	.ds 1
_isr_tick_count_1_1:
	.ds 1
;--------------------------------------------------------
; overlayable items in internal ram 
;--------------------------------------------------------
	.area OSEG    (OVR,DATA)
;--------------------------------------------------------
; Stack segment in internal ram 
;--------------------------------------------------------
	.area	SSEG	(DATA)
__start__stack:
	.ds	1

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
_patch_usb_descriptors_hw_rev_1_1:
	.ds 1
_patch_usb_descriptors_serial_no_1_1:
	.ds 8
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
; interrupt vector 
;--------------------------------------------------------
	.area CSEG    (CODE)
__interrupt_vect:
	ljmp	__sdcc_gsinit_startup
;--------------------------------------------------------
; global & static initialisations
;--------------------------------------------------------
	.area CSEG    (CODE)
	.area GSINIT  (CODE)
	.area GSFINAL (CODE)
	.area GSINIT  (CODE)
	.globl __sdcc_gsinit_startup
	.globl __sdcc_program_startup
	.globl __start__stack
	.globl __mcs51_genRAMCLEAR
;------------------------------------------------------------
;Allocation info for local variables in function 'isr_tick'
;------------------------------------------------------------
;count                     Allocated with name '_isr_tick_count_1_1'
;------------------------------------------------------------
;src/qs1r_main.c:931: static unsigned char	count = 1;
;     genAssign
	mov	_isr_tick_count_1_1,#0x01
	.area GSFINAL (CODE)
	ljmp	__sdcc_program_startup
;--------------------------------------------------------
; Home
;--------------------------------------------------------
	.area HOME    (CODE)
	.area CSEG    (CODE)
;--------------------------------------------------------
; code
;--------------------------------------------------------
	.area CSEG    (CODE)
__sdcc_program_startup:
	lcall	_main
;	return from main will lock up
	sjmp .
;------------------------------------------------------------
;Allocation info for local variables in function 'get_ep0_data'
;------------------------------------------------------------
;------------------------------------------------------------
;src/qs1r_main.c:54: get_ep0_data (void)
;	-----------------------------------------
;	 function get_ep0_data
;	-----------------------------------------
_get_ep0_data:
	ar2 = 0x02
	ar3 = 0x03
	ar4 = 0x04
	ar5 = 0x05
	ar6 = 0x06
	ar7 = 0x07
	ar0 = 0x00
	ar1 = 0x01
;src/qs1r_main.c:56: EP0BCL = 0;			// arm EP0 for OUT xfer.  This sets the busy bit
;     genAssign
	mov	dptr,#_EP0BCL
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:57: while (EP0CS & bmEPBUSY);	// wait for busy to clear
00101$:
;     genAssign
	mov	dptr,#_EP0CS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
;	Peephole 112.a	removed ljmp by inverse jump logic
	jb	acc.1,00101$
00107$:
00104$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'app_vendor_OUT_cmd'
;------------------------------------------------------------
;i                         Allocated to registers r2 r3 
;j                         Allocated to registers r5 r6 
;offset                    Allocated to registers r2 
;------------------------------------------------------------
;src/qs1r_main.c:65: unsigned char app_vendor_OUT_cmd(void)
;	-----------------------------------------
;	 function app_vendor_OUT_cmd
;	-----------------------------------------
_app_vendor_OUT_cmd:
;src/qs1r_main.c:70: switch (bRequest)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0001)
	movx	a,@dptr
;     genCmpGt
;     genCmp
;     genIfxJump
;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
;	Peephole 177.a	removed redundant mov
	mov  r2,a
	add	a,#0xff - 0x2F
	jnc	00349$
	ljmp	00265$
00349$:
;     genJumpTab
	mov	a,r2
	add	a,#(00350$-3-.)
	movc	a,@a+pc
	push	acc
	mov	a,r2
	add	a,#(00351$-3-.)
	movc	a,@a+pc
	push	acc
	ret
00350$:
	.db	00265$
	.db	00101$
	.db	00121$
	.db	00265$
	.db	00265$
	.db	00265$
	.db	00265$
	.db	00265$
	.db	00127$
	.db	00130$
	.db	00102$
	.db	00133$
	.db	00137$
	.db	00103$
	.db	00104$
	.db	00105$
	.db	00106$
	.db	00107$
	.db	00108$
	.db	00109$
	.db	00110$
	.db	00111$
	.db	00112$
	.db	00113$
	.db	00114$
	.db	00115$
	.db	00116$
	.db	00117$
	.db	00118$
	.db	00119$
	.db	00120$
	.db	00144$
	.db	00151$
	.db	00158$
	.db	00165$
	.db	00172$
	.db	00179$
	.db	00186$
	.db	00193$
	.db	00200$
	.db	00207$
	.db	00214$
	.db	00221$
	.db	00228$
	.db	00235$
	.db	00258$
	.db	00251$
	.db	00242$
00351$:
	.db	00265$>>8
	.db	00101$>>8
	.db	00121$>>8
	.db	00265$>>8
	.db	00265$>>8
	.db	00265$>>8
	.db	00265$>>8
	.db	00265$>>8
	.db	00127$>>8
	.db	00130$>>8
	.db	00102$>>8
	.db	00133$>>8
	.db	00137$>>8
	.db	00103$>>8
	.db	00104$>>8
	.db	00105$>>8
	.db	00106$>>8
	.db	00107$>>8
	.db	00108$>>8
	.db	00109$>>8
	.db	00110$>>8
	.db	00111$>>8
	.db	00112$>>8
	.db	00113$>>8
	.db	00114$>>8
	.db	00115$>>8
	.db	00116$>>8
	.db	00117$>>8
	.db	00118$>>8
	.db	00119$>>8
	.db	00120$>>8
	.db	00144$>>8
	.db	00151$>>8
	.db	00158$>>8
	.db	00165$>>8
	.db	00172$>>8
	.db	00179$>>8
	.db	00186$>>8
	.db	00193$>>8
	.db	00200$>>8
	.db	00207$>>8
	.db	00214$>>8
	.db	00221$>>8
	.db	00228$>>8
	.db	00235$>>8
	.db	00258$>>8
	.db	00251$>>8
	.db	00242$>>8
;src/qs1r_main.c:73: case VRQ_DEBUG_LED_SET:
00101$:
;src/qs1r_main.c:75: DEBUG_LED_REG = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DEBUG_LED_REG
	movx	@dptr,a
;src/qs1r_main.c:76: break;
	ljmp	00266$
;src/qs1r_main.c:78: case VRQ_LTC2208_CNTRL_SET:
00102$:
;src/qs1r_main.c:80: LTC2208_CNTRL_REG = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_LTC2208_CNTRL_REG
	movx	@dptr,a
;src/qs1r_main.c:81: break;
	ljmp	00266$
;src/qs1r_main.c:83: case VRQ_ATTN_SET:
00103$:
;src/qs1r_main.c:85: ATTN = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_ATTN
	movx	@dptr,a
;src/qs1r_main.c:86: break;
	ljmp	00266$
;src/qs1r_main.c:88: case VRQ_RFFE_SET:
00104$:
;src/qs1r_main.c:90: RFFE_CNTRL = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_RFFE_CNTRL
	movx	@dptr,a
;src/qs1r_main.c:91: break;
	ljmp	00266$
;src/qs1r_main.c:93: case VRQ_DDC_CNTRL_SET:
00105$:
;src/qs1r_main.c:95: DDC_CNTRL = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC_CNTRL
	movx	@dptr,a
;src/qs1r_main.c:96: break;
	ljmp	00266$
;src/qs1r_main.c:98: case VRQ_CPLD_GPIO_SET:
00106$:
;src/qs1r_main.c:100: CPLD_GPIO = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_CPLD_GPIO
	movx	@dptr,a
;src/qs1r_main.c:101: break;
	ljmp	00266$
;src/qs1r_main.c:103: case VRQ_DDC0_DR0_SET:
00107$:
;src/qs1r_main.c:105: DDC0_DR0 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/qs1r_main.c:106: break;
	ljmp	00266$
;src/qs1r_main.c:108: case VRQ_DDC0_DR1_SET:
00108$:
;src/qs1r_main.c:110: DDC0_DR1 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/qs1r_main.c:111: break;
	ljmp	00266$
;src/qs1r_main.c:113: case VRQ_DDC0_DR2_SET:
00109$:
;src/qs1r_main.c:115: DDC0_DR2 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
;src/qs1r_main.c:116: break;
	ljmp	00266$
;src/qs1r_main.c:118: case VRQ_DDC0_DR3_SET:
00110$:
;src/qs1r_main.c:120: DDC0_DR3 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR3
	movx	@dptr,a
;src/qs1r_main.c:121: break;
	ljmp	00266$
;src/qs1r_main.c:123: case VRQ_DDC0_DR4_SET:
00111$:
;src/qs1r_main.c:125: DDC0_DR4 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR4
	movx	@dptr,a
;src/qs1r_main.c:126: break;
	ljmp	00266$
;src/qs1r_main.c:128: case VRQ_DDC0_LAR_SET:
00112$:
;src/qs1r_main.c:130: DDC0_LAR = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_LAR
	movx	@dptr,a
;src/qs1r_main.c:131: break;
	ljmp	00266$
;src/qs1r_main.c:133: case VRQ_DDC0_AMR_SET:
00113$:
;src/qs1r_main.c:135: DDC0_AMR = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_AMR
	movx	@dptr,a
;src/qs1r_main.c:136: break;
	ljmp	00266$
;src/qs1r_main.c:138: case VRQ_DDC1_DR0_SET:
00114$:
;src/qs1r_main.c:140: DDC1_DR0 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/qs1r_main.c:141: break;
	ljmp	00266$
;src/qs1r_main.c:143: case VRQ_DDC1_DR1_SET:
00115$:
;src/qs1r_main.c:145: DDC1_DR1 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/qs1r_main.c:146: break;
	ljmp	00266$
;src/qs1r_main.c:148: case VRQ_DDC1_DR2_SET:
00116$:
;src/qs1r_main.c:150: DDC1_DR2 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
;src/qs1r_main.c:151: break;
	ljmp	00266$
;src/qs1r_main.c:153: case VRQ_DDC1_DR3_SET:
00117$:
;src/qs1r_main.c:155: DDC1_DR3 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR3
	movx	@dptr,a
;src/qs1r_main.c:156: break;
	ljmp	00266$
;src/qs1r_main.c:158: case VRQ_DDC1_DR4_SET:
00118$:
;src/qs1r_main.c:160: DDC1_DR4 = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR4
	movx	@dptr,a
;src/qs1r_main.c:161: break;
	ljmp	00266$
;src/qs1r_main.c:163: case VRQ_DDC1_LAR_SET:
00119$:
;src/qs1r_main.c:165: DDC1_LAR = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_LAR
	movx	@dptr,a
;src/qs1r_main.c:166: break;
	ljmp	00266$
;src/qs1r_main.c:168: case VRQ_DDC1_AMR_SET:
00120$:
;src/qs1r_main.c:170: DDC1_AMR = (BYTE)wValueL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_AMR
	movx	@dptr,a
;src/qs1r_main.c:171: break;
	ljmp	00266$
;src/qs1r_main.c:173: case VRQ_FPGA_LOAD:
00121$:
;src/qs1r_main.c:175: switch (wIndexL)			// sub-command
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00122$
00352$:
;     genCmpEq
	cjne	r2,#0x01,00353$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00123$
00353$:
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;src/qs1r_main.c:177: case FL_BEGIN:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x02,00125$
	sjmp	00124$
;00354$:
00122$:
;src/qs1r_main.c:178: return fpga_load_begin ();
;     genCall
;     genRet
;	Peephole 251.a	replaced ljmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_fpga_load_begin
;src/qs1r_main.c:180: case FL_XFER:
00123$:
;src/qs1r_main.c:181: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:182: return fpga_load_xfer (EP0BUF, EP0BCL);
;     genAssign
	mov	dptr,#_EP0BCL
	movx	a,@dptr
	mov	_fpga_load_xfer_PARM_2,a
;     genCall
;	Peephole 182.a	used 16 bit load of DPTR
	mov	dptr,#_EP0BUF
;     genRet
;	Peephole 251.a	replaced ljmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_fpga_load_xfer
;src/qs1r_main.c:184: case FL_END:
00124$:
;src/qs1r_main.c:185: return fpga_load_end ();
;     genCall
;     genRet
;	Peephole 251.a	replaced ljmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_fpga_load_end
;src/qs1r_main.c:187: default:
00125$:
;src/qs1r_main.c:188: return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:192: case VRQ_I2C_WRITE:
00127$:
;src/qs1r_main.c:194: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:195: if (!i2c_write (wValueL, EP0BUF, EP0BCL))
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genAddrOf
	mov	_i2c_write_PARM_2,#_EP0BUF
	mov	(_i2c_write_PARM_2 + 1),#(_EP0BUF >> 8)
;     genAssign
	mov	dptr,#_EP0BCL
	movx	a,@dptr
	mov	_i2c_write_PARM_3,a
;     genCall
	mov	dpl,r2
	lcall	_i2c_write
	mov	a,dpl
;     genIfx
;     genIfxJump
	jz	00355$
	ljmp	00266$
00355$:
;src/qs1r_main.c:196: return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:199: case VRQ_SPI_WRITE:
00130$:
;src/qs1r_main.c:201: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:202: if (!spi_write (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, EP0BCL))
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0003)
	movx	a,@dptr
	mov	r2,a
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	_spi_write_PARM_2,a
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0005)
	movx	a,@dptr
	mov	_spi_write_PARM_3,a
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	_spi_write_PARM_4,a
;     genAddrOf
	mov	_spi_write_PARM_5,#_EP0BUF
	mov	(_spi_write_PARM_5 + 1),#(_EP0BUF >> 8)
;     genAssign
	mov	dptr,#_EP0BCL
	movx	a,@dptr
	mov	_spi_write_PARM_6,a
;     genCall
	mov	dpl,r2
	lcall	_spi_write
	mov	a,dpl
;     genIfx
;     genIfxJump
	jz	00356$
	ljmp	00266$
00356$:
;src/qs1r_main.c:203: return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:206: case VRQ_I2C_SPEED_SET:
00133$:
;src/qs1r_main.c:207: if (wValueL == 1)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00135$
;00357$:
;	Peephole 200	removed redundant sjmp
00358$:
;src/qs1r_main.c:208: I2CTL |= bm400KHZ;
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_I2CTL
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
	ljmp	00266$
00135$:
;src/qs1r_main.c:210: I2CTL &= ~bm400KHZ;
;     genAssign
;     genAnd
;	Peephole 248.b	optimized and to xdata
	mov	dptr,#_I2CTL
	movx	a,@dptr
	mov	r2,a
	anl	a,#0xFE
	movx	@dptr,a
;src/qs1r_main.c:211: break;
	ljmp	00266$
;src/qs1r_main.c:213: case VRQ_CPU_SPEED_SET:
00137$:
;src/qs1r_main.c:214: if (wValueL == 0)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00360$
00359$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00142$
00360$:
;src/qs1r_main.c:215: CPUCS = bmCLKOE; // 12 MHz
;     genAssign
	mov	dptr,#_CPUCS
	mov	a,#0x02
	movx	@dptr,a
	ljmp	00266$
00142$:
;src/qs1r_main.c:216: else if (wValueL == 1)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00139$
;00361$:
;	Peephole 200	removed redundant sjmp
00362$:
;src/qs1r_main.c:217: CPUCS = bmCLKSPD0 | bmCLKOE; // 24 MHz
;     genAssign
	mov	dptr,#_CPUCS
	mov	a,#0x0A
	movx	@dptr,a
	ljmp	00266$
00139$:
;src/qs1r_main.c:219: CPUCS = bmCLKSPD1 | bmCLKOE; // 48 MHz
;     genAssign
	mov	dptr,#_CPUCS
	mov	a,#0x12
	movx	@dptr,a
;src/qs1r_main.c:220: break;
	ljmp	00266$
;src/qs1r_main.c:222: case VRQ_DDC_MCR_SET:
00144$:
;src/qs1r_main.c:223: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:224: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00364$
00363$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00149$
00364$:
;src/qs1r_main.c:225: DDC0_AMR = (MCR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:226: DDC0_LAR = (MCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:227: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00149$:
;src/qs1r_main.c:228: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00146$
;00365$:
;	Peephole 200	removed redundant sjmp
00366$:
;src/qs1r_main.c:229: DDC1_AMR = (MCR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:230: DDC1_LAR = (MCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:231: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00146$:
;src/qs1r_main.c:232: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:235: case VRQ_DDC_NCR_SET:
00151$:
;src/qs1r_main.c:236: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:237: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00368$
00367$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00156$
00368$:
;src/qs1r_main.c:238: DDC0_AMR = (NCR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:239: DDC0_LAR = (NCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:240: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00156$:
;src/qs1r_main.c:241: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00153$
;00369$:
;	Peephole 200	removed redundant sjmp
00370$:
;src/qs1r_main.c:242: DDC1_AMR = (NCR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:243: DDC1_LAR = (NCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:244: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00153$:
;src/qs1r_main.c:245: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:248: case VRQ_DDC_NSCR_SET:
00158$:
;src/qs1r_main.c:249: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:250: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00372$
00371$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00163$
00372$:
;src/qs1r_main.c:251: DDC0_AMR = (NSCR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:252: DDC0_LAR = (NSCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x02
	movx	@dptr,a
;src/qs1r_main.c:253: DDC0_DR3 = EP0BUF[3];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0003)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR3
	movx	@dptr,a
;src/qs1r_main.c:254: DDC0_DR2 = EP0BUF[2];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
;src/qs1r_main.c:255: DDC0_DR1 = EP0BUF[1];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/qs1r_main.c:256: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00163$:
;src/qs1r_main.c:257: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00160$
;00373$:
;	Peephole 200	removed redundant sjmp
00374$:
;src/qs1r_main.c:258: DDC1_AMR = (NSCR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:259: DDC1_LAR = (NSCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x02
	movx	@dptr,a
;src/qs1r_main.c:260: DDC1_DR3 = EP0BUF[3];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0003)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR3
	movx	@dptr,a
;src/qs1r_main.c:261: DDC1_DR2 = EP0BUF[2];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
;src/qs1r_main.c:262: DDC1_DR1 = EP0BUF[1];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/qs1r_main.c:263: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00160$:
;src/qs1r_main.c:264: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:267: case VRQ_DDC_NFRE_SET:
00165$:
;src/qs1r_main.c:268: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:269: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00376$
00375$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00170$
00376$:
;src/qs1r_main.c:270: DDC0_AMR = (NFRE >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:271: DDC0_LAR = (NFRE & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:272: DDC0_DR3 = EP0BUF[3];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0003)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR3
	movx	@dptr,a
;src/qs1r_main.c:273: DDC0_DR2 = EP0BUF[2];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
;src/qs1r_main.c:274: DDC0_DR1 = EP0BUF[1];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/qs1r_main.c:275: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00170$:
;src/qs1r_main.c:276: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00167$
;00377$:
;	Peephole 200	removed redundant sjmp
00378$:
;src/qs1r_main.c:277: DDC1_AMR = (NFRE >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:278: DDC1_LAR = (NFRE & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:279: DDC1_DR3 = EP0BUF[3];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0003)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR3
	movx	@dptr,a
;src/qs1r_main.c:280: DDC1_DR2 = EP0BUF[2];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
;src/qs1r_main.c:281: DDC1_DR1 = EP0BUF[1];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/qs1r_main.c:282: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00167$:
;src/qs1r_main.c:283: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:286: case VRQ_DDC_NPO_SET:
00172$:
;src/qs1r_main.c:287: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:288: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00380$
00379$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00177$
00380$:
;src/qs1r_main.c:289: DDC0_AMR = (NPHA >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:290: DDC0_LAR = (NPHA & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x04
	movx	@dptr,a
;src/qs1r_main.c:291: DDC0_DR1 = EP0BUF[1];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/qs1r_main.c:292: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00177$:
;src/qs1r_main.c:293: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00174$
;00381$:
;	Peephole 200	removed redundant sjmp
00382$:
;src/qs1r_main.c:294: DDC1_AMR = (NPHA >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:295: DDC1_LAR = (NPHA & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x04
	movx	@dptr,a
;src/qs1r_main.c:296: DDC1_DR1 = EP0BUF[1];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/qs1r_main.c:297: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00174$:
;src/qs1r_main.c:298: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:301: case VRQ_DDC_C2SR_SET:
00179$:
;src/qs1r_main.c:302: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:303: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00384$
00383$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00184$
00384$:
;src/qs1r_main.c:304: DDC0_AMR = (C2SR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:305: DDC0_LAR = (C2SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x05
	movx	@dptr,a
;src/qs1r_main.c:306: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00184$:
;src/qs1r_main.c:307: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00181$
;00385$:
;	Peephole 200	removed redundant sjmp
00386$:
;src/qs1r_main.c:308: DDC1_AMR = (C2SR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:309: DDC1_LAR = (C2SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x05
	movx	@dptr,a
;src/qs1r_main.c:310: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00181$:
;src/qs1r_main.c:311: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:314: case VRQ_DDC_C2DEC_SET:
00186$:
;src/qs1r_main.c:315: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:316: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00388$
00387$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00191$
00388$:
;src/qs1r_main.c:317: DDC0_AMR = (C2DEC >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:318: DDC0_LAR = (C2DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x06
	movx	@dptr,a
;src/qs1r_main.c:319: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00191$:
;src/qs1r_main.c:320: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00188$
;00389$:
;	Peephole 200	removed redundant sjmp
00390$:
;src/qs1r_main.c:321: DDC1_AMR = (C2DEC >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:322: DDC1_LAR = (C2DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x06
	movx	@dptr,a
;src/qs1r_main.c:323: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00188$:
;src/qs1r_main.c:324: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:327: case VRQ_DDC_C5SR_SET:
00193$:
;src/qs1r_main.c:328: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:329: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00392$
00391$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00198$
00392$:
;src/qs1r_main.c:330: DDC0_AMR = (C5SR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:331: DDC0_LAR = (C5SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x07
	movx	@dptr,a
;src/qs1r_main.c:332: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00198$:
;src/qs1r_main.c:333: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00195$
;00393$:
;	Peephole 200	removed redundant sjmp
00394$:
;src/qs1r_main.c:334: DDC1_AMR = (C5SR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:335: DDC1_LAR = (C5SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x07
	movx	@dptr,a
;src/qs1r_main.c:336: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00195$:
;src/qs1r_main.c:337: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:340: case VRQ_DDC_C5DEC_SET:
00200$:
;src/qs1r_main.c:341: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:342: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00396$
00395$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00205$
00396$:
;src/qs1r_main.c:343: DDC0_AMR = (C5DEC >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:344: DDC0_LAR = (C5DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x08
	movx	@dptr,a
;src/qs1r_main.c:345: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00205$:
;src/qs1r_main.c:346: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00202$
;00397$:
;	Peephole 200	removed redundant sjmp
00398$:
;src/qs1r_main.c:347: DDC1_AMR = (C5DEC >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:348: DDC1_LAR = (C5DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x08
	movx	@dptr,a
;src/qs1r_main.c:349: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00202$:
;src/qs1r_main.c:350: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:353: case VRQ_DDC_RCR_SET:
00207$:
;src/qs1r_main.c:354: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:355: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00400$
00399$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00212$
00400$:
;src/qs1r_main.c:356: DDC0_AMR = (RCR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:357: DDC0_LAR = (RCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x09
	movx	@dptr,a
;src/qs1r_main.c:358: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00212$:
;src/qs1r_main.c:359: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00209$
;00401$:
;	Peephole 200	removed redundant sjmp
00402$:
;src/qs1r_main.c:360: DDC1_AMR = (RCR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:361: DDC1_LAR = (RCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x09
	movx	@dptr,a
;src/qs1r_main.c:362: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00209$:
;src/qs1r_main.c:363: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:366: case VRQ_DDC_RDEC_SET:
00214$:
;src/qs1r_main.c:367: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:368: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00404$
00403$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00219$
00404$:
;src/qs1r_main.c:369: DDC0_AMR = (RDEC >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:370: DDC0_LAR = (RDEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0A
	movx	@dptr,a
;src/qs1r_main.c:371: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00219$:
;src/qs1r_main.c:372: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00216$
;00405$:
;	Peephole 200	removed redundant sjmp
00406$:
;src/qs1r_main.c:373: DDC1_AMR = (RDEC >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:374: DDC1_LAR = (RDEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0A
	movx	@dptr,a
;src/qs1r_main.c:375: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00216$:
;src/qs1r_main.c:376: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:379: case VRQ_DDC_RAO_SET:
00221$:
;src/qs1r_main.c:380: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:381: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00408$
00407$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00226$
00408$:
;src/qs1r_main.c:382: DDC0_AMR = (RAO >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:383: DDC0_LAR = (RAO & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0B
	movx	@dptr,a
;src/qs1r_main.c:384: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00226$:
;src/qs1r_main.c:385: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00223$
;00409$:
;	Peephole 200	removed redundant sjmp
00410$:
;src/qs1r_main.c:386: DDC1_AMR = (RAO >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:387: DDC1_LAR = (RAO & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0B
	movx	@dptr,a
;src/qs1r_main.c:388: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00223$:
;src/qs1r_main.c:389: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:392: case VRQ_DDC_RTAPS_SET:
00228$:
;src/qs1r_main.c:393: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:394: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00412$
00411$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00233$
00412$:
;src/qs1r_main.c:395: DDC0_AMR = (RTAP >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:396: DDC0_LAR = (RTAP & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0C
	movx	@dptr,a
;src/qs1r_main.c:397: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00233$:
;src/qs1r_main.c:398: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00230$
;00413$:
;	Peephole 200	removed redundant sjmp
00414$:
;src/qs1r_main.c:399: DDC1_AMR = (RTAP >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:400: DDC1_LAR = (RTAP & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0C
	movx	@dptr,a
;src/qs1r_main.c:401: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00230$:
;src/qs1r_main.c:402: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:405: case VRQ_DDC_RSVR_SET:
00235$:
;src/qs1r_main.c:406: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:407: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00416$
00415$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00240$
00416$:
;src/qs1r_main.c:408: DDC0_AMR = (RSRV >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:409: DDC0_LAR = (RSRV & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0D
	movx	@dptr,a
;src/qs1r_main.c:410: DDC0_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
	ljmp	00266$
00240$:
;src/qs1r_main.c:411: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00237$
;00417$:
;	Peephole 200	removed redundant sjmp
00418$:
;src/qs1r_main.c:412: DDC1_AMR = (RSRV >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:413: DDC1_LAR = (RSRV & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0D
	movx	@dptr,a
;src/qs1r_main.c:414: DDC1_DR0 = EP0BUF[0];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP0BUF
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
	ljmp	00266$
00237$:
;src/qs1r_main.c:415: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:418: case VRQ_DDC_COEF_SET:
00242$:
;src/qs1r_main.c:419: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:420: if (wValueL > 0x0F) return 0;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genCmpGt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
;	Peephole 177.a	removed redundant mov
	mov  r2,a
	add	a,#0xff - 0x0F
	jnc	00244$
00419$:
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00244$:
;src/qs1r_main.c:421: offset = wValueL << 4; // advance by 16
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genLeftShift
;     genLeftShiftLiteral
;     genlshOne
;	Peephole 105	removed redundant mov
	mov	r2,a
	swap	a
	anl	a,#0xf0
	mov	r2,a
;src/qs1r_main.c:422: if (wIndexL == 0)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r3,a
	jz	00421$
00420$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00249$
00421$:
;src/qs1r_main.c:424: for (i=0, j=0; i < 16; i++, j+=3) {
;     genAssign
	mov	r3,#0x00
	mov	r4,#0x00
;     genAssign
	mov	r5,#0x00
	mov	r6,#0x00
00267$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r3
	subb	a,#0x10
	mov	a,r4
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
	jc	00422$
	ljmp	00266$
00422$:
;src/qs1r_main.c:425: DDC0_AMR = 0;
;     genAssign
	mov	dptr,#_DDC0_AMR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:426: DDC0_LAR = ((DDC_COEF + i + offset) & 0xFF);
;     genCast
	mov	ar7,r3
;     genPlus
	mov	dptr,#_DDC0_LAR
;	Peephole 236.g	used r2 instead of ar2
	mov	a,r2
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
	movx	@dptr,a
;src/qs1r_main.c:427: DDC0_DR2 = (EP0BUF[j+2] & 0xFF);
;     genCast
	mov	ar7,r5
;     genPlus
;     genPlusIncr
	mov	a,#0x02
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
;     genPlus
	add	a,#_EP0BUF
	mov	dpl,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP0BUF >> 8)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r0,a
	mov	dptr,#_DDC0_DR2
	movx	@dptr,a
;src/qs1r_main.c:428: DDC0_DR1 = (EP0BUF[j+1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
;     genPlus
	add	a,#_EP0BUF
	mov	dpl,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP0BUF >> 8)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r7,a
	mov	dptr,#_DDC0_DR1
	movx	@dptr,a
;src/qs1r_main.c:429: DDC0_DR0 = (EP0BUF[j] & 0xFF);
;     genPlus
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
	add	a,#_EP0BUF
	mov	dpl,a
;	Peephole 236.g	used r6 instead of ar6
	mov	a,r6
	addc	a,#(_EP0BUF >> 8)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r7,a
	mov	dptr,#_DDC0_DR0
	movx	@dptr,a
;src/qs1r_main.c:424: for (i=0, j=0; i < 16; i++, j+=3) {
;     genPlus
;     genPlusIncr
	inc	r3
	cjne	r3,#0x00,00423$
	inc	r4
00423$:
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
	sjmp	00267$
00249$:
;src/qs1r_main.c:431: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r3,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r3,#0x01,00246$
;00424$:
;	Peephole 200	removed redundant sjmp
00425$:
;src/qs1r_main.c:432: for (i=0, j=0; i < 16; i++, j+=3) {
;     genAssign
	mov	r3,#0x00
	mov	r4,#0x00
;     genAssign
	mov	r5,#0x00
	mov	r6,#0x00
00271$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r3
	subb	a,#0x10
	mov	a,r4
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
	jc	00426$
	ljmp	00266$
00426$:
;src/qs1r_main.c:433: DDC1_AMR = 0;
;     genAssign
	mov	dptr,#_DDC1_AMR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:434: DDC1_LAR = ((DDC_COEF + i + offset) & 0xFF);
;     genCast
	mov	ar7,r3
;     genPlus
	mov	dptr,#_DDC1_LAR
;	Peephole 236.g	used r2 instead of ar2
	mov	a,r2
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
	movx	@dptr,a
;src/qs1r_main.c:435: DDC1_DR2 = (EP0BUF[j+2] & 0xFF);
;     genCast
	mov	ar7,r5
;     genPlus
;     genPlusIncr
	mov	a,#0x02
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
;     genPlus
	add	a,#_EP0BUF
	mov	dpl,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP0BUF >> 8)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r0,a
	mov	dptr,#_DDC1_DR2
	movx	@dptr,a
;src/qs1r_main.c:436: DDC1_DR1 = (EP0BUF[j+1] & 0xFF);
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
;     genPlus
	add	a,#_EP0BUF
	mov	dpl,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP0BUF >> 8)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r7,a
	mov	dptr,#_DDC1_DR1
	movx	@dptr,a
;src/qs1r_main.c:437: DDC1_DR0 = (EP0BUF[j] & 0xFF);
;     genPlus
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
	add	a,#_EP0BUF
	mov	dpl,a
;	Peephole 236.g	used r6 instead of ar6
	mov	a,r6
	addc	a,#(_EP0BUF >> 8)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r7,a
	mov	dptr,#_DDC1_DR0
	movx	@dptr,a
;src/qs1r_main.c:432: for (i=0, j=0; i < 16; i++, j+=3) {
;     genPlus
;     genPlusIncr
	inc	r3
	cjne	r3,#0x00,00427$
	inc	r4
00427$:
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
	sjmp	00271$
00246$:
;src/qs1r_main.c:439: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:442: case VRQ_DDC_COEF_CLR:
00251$:
;src/qs1r_main.c:443: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:444: if (wIndexL == 0)  {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00429$
00428$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00256$
00429$:
;src/qs1r_main.c:445: for (i=0; i < 0x100; i++) {
;     genAssign
	mov	r2,#0x00
	mov	r3,#0x00
00275$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,#0x00
	mov	a,r3
	xrl	a,#0x80
	subb	a,#0x81
;     genIfxJump
	jc	00430$
	ljmp	00266$
00430$:
;src/qs1r_main.c:446: DDC0_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
;     genAnd
	mov	r4,#0x00
	mov	ar5,r3
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r5
	mov	r5,#0x00
;     genCast
	mov	dptr,#_DDC0_AMR
	mov	a,r4
	movx	@dptr,a
;src/qs1r_main.c:447: DDC0_LAR = ((DDC_COEF + i) & 0xFF);
;     genCast
	mov	dptr,#_DDC0_LAR
	mov	a,r2
	movx	@dptr,a
;src/qs1r_main.c:448: DDC0_DR4 = 0;
;     genAssign
	mov	dptr,#_DDC0_DR4
;	Peephole 181	changed mov to clr
;src/qs1r_main.c:449: DDC0_DR3 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;src/qs1r_main.c:450: DDC0_DR2 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;src/qs1r_main.c:451: DDC0_DR1 = 0;
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
;src/qs1r_main.c:452: DDC0_DR0 = 0;
;     genAssign
	mov	dptr,#_DDC0_DR0
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:445: for (i=0; i < 0x100; i++) {
;     genPlus
;     genPlusIncr
	inc	r2
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 243	avoided branch to sjmp
	cjne	r2,#0x00,00275$
	inc	r3
00431$:
	sjmp	00275$
00256$:
;src/qs1r_main.c:454: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00253$
;00432$:
;	Peephole 200	removed redundant sjmp
00433$:
;src/qs1r_main.c:455: for (i=0; i < 0x100; i++) {
;     genAssign
	mov	r2,#0x00
	mov	r3,#0x00
00279$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,#0x00
	mov	a,r3
	xrl	a,#0x80
	subb	a,#0x81
;     genIfxJump
	jc	00434$
	ljmp	00266$
00434$:
;src/qs1r_main.c:456: DDC1_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
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
;src/qs1r_main.c:457: DDC1_LAR = ((DDC_COEF + i) & 0xFF);
;     genCast
	mov	dptr,#_DDC1_LAR
	mov	a,r2
	movx	@dptr,a
;src/qs1r_main.c:458: DDC1_DR4 = 0;
;     genAssign
	mov	dptr,#_DDC1_DR4
;	Peephole 181	changed mov to clr
;src/qs1r_main.c:459: DDC1_DR3 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;src/qs1r_main.c:460: DDC1_DR2 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;src/qs1r_main.c:461: DDC1_DR1 = 0;
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
;src/qs1r_main.c:462: DDC1_DR0 = 0;
;     genAssign
	mov	dptr,#_DDC1_DR0
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:455: for (i=0; i < 0x100; i++) {
;     genPlus
;     genPlusIncr
	inc	r2
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 243	avoided branch to sjmp
	cjne	r2,#0x00,00279$
	inc	r3
00435$:
	sjmp	00279$
00253$:
;src/qs1r_main.c:464: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;src/qs1r_main.c:467: case VRQ_DDC_RAM_CLR:
00258$:
;src/qs1r_main.c:468: get_ep0_data ();
;     genCall
	lcall	_get_ep0_data
;src/qs1r_main.c:469: if (wIndexL == 0)  {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00437$
00436$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00263$
00437$:
;src/qs1r_main.c:470: for (i=0; i < 0x100; i++) {
;     genAssign
	mov	r2,#0x00
	mov	r3,#0x00
00283$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,#0x00
	mov	a,r3
	xrl	a,#0x80
	subb	a,#0x81
;     genIfxJump
	jc	00438$
	ljmp	00266$
00438$:
;src/qs1r_main.c:471: DDC0_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
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
	mov	dptr,#_DDC0_AMR
	mov	a,r4
	movx	@dptr,a
;src/qs1r_main.c:472: DDC0_LAR = ((DDC_RAM + i) & 0xFF);
;     genCast
	mov	dptr,#_DDC0_LAR
	mov	a,r2
	movx	@dptr,a
;src/qs1r_main.c:473: DDC0_DR4 = 0;
;     genAssign
	mov	dptr,#_DDC0_DR4
;	Peephole 181	changed mov to clr
;src/qs1r_main.c:474: DDC0_DR3 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;src/qs1r_main.c:475: DDC0_DR2 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;src/qs1r_main.c:476: DDC0_DR1 = 0;
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
;src/qs1r_main.c:477: DDC0_DR0 = 0;
;     genAssign
	mov	dptr,#_DDC0_DR0
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:470: for (i=0; i < 0x100; i++) {
;     genPlus
;     genPlusIncr
	inc	r2
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 243	avoided branch to sjmp
	cjne	r2,#0x00,00283$
	inc	r3
00439$:
	sjmp	00283$
00263$:
;src/qs1r_main.c:479: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00260$
;00440$:
;	Peephole 200	removed redundant sjmp
00441$:
;src/qs1r_main.c:480: for (i=0; i < 0x100; i++) {
;     genAssign
	mov	r2,#0x00
	mov	r3,#0x00
00287$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,#0x00
	mov	a,r3
	xrl	a,#0x80
	subb	a,#0x81
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00266$
00442$:
;src/qs1r_main.c:481: DDC1_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
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
;src/qs1r_main.c:482: DDC1_LAR = ((DDC_RAM + i) & 0xFF);
;     genCast
	mov	dptr,#_DDC1_LAR
	mov	a,r2
	movx	@dptr,a
;src/qs1r_main.c:483: DDC1_DR4 = 0;
;     genAssign
	mov	dptr,#_DDC1_DR4
;	Peephole 181	changed mov to clr
;src/qs1r_main.c:484: DDC1_DR3 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;src/qs1r_main.c:485: DDC1_DR2 = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;src/qs1r_main.c:486: DDC1_DR1 = 0;
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
;src/qs1r_main.c:487: DDC1_DR0 = 0;
;     genAssign
	mov	dptr,#_DDC1_DR0
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:480: for (i=0; i < 0x100; i++) {
;     genPlus
;     genPlusIncr
	inc	r2
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 243	avoided branch to sjmp
	cjne	r2,#0x00,00287$
	inc	r3
00443$:
	sjmp	00287$
00260$:
;src/qs1r_main.c:489: } else return 0;
;     genRet
	mov	dpl,#0x00
;src/qs1r_main.c:492: default:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00265$:
;src/qs1r_main.c:493: return 0;
;     genRet
	mov	dpl,#0x00
;src/qs1r_main.c:494: }
;	Peephole 112.b	changed ljmp to sjmp
;src/qs1r_main.c:495: return 1;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00266$:
	mov	dpl,#0x01
00291$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'app_vendor_IN_cmd'
;------------------------------------------------------------
;i                         Allocated to registers r3 r4 
;j                         Allocated to registers r5 r6 
;offset                    Allocated with name '_app_vendor_IN_cmd_offset_1_1'
;foop                      Allocated to registers 
;------------------------------------------------------------
;src/qs1r_main.c:498: unsigned char app_vendor_IN_cmd(void)
;	-----------------------------------------
;	 function app_vendor_IN_cmd
;	-----------------------------------------
_app_vendor_IN_cmd:
;src/qs1r_main.c:504: switch (bRequest)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0001)
	movx	a,@dptr
	mov	r2,a
;     genCmpLt
;     genCmp
	cjne	r2,#0x81,00288$
00288$:
;     genIfxJump
	jnc	00289$
	ljmp	00237$
00289$:
;     genCmpGt
;     genCmp
;     genIfxJump
;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
	mov	a,r2
	add	a,#0xff - 0xA8
	jnc	00290$
	ljmp	00237$
00290$:
;     genMinus
	mov	a,r2
	add	a,#0x7f
;     genJumpTab
;	Peephole 105	removed redundant mov
	mov	r2,a
	add	a,#(00291$-3-.)
	movc	a,@a+pc
	push	acc
	mov	a,r2
	add	a,#(00292$-3-.)
	movc	a,@a+pc
	push	acc
	ret
00291$:
	.db	00101$
	.db	00104$
	.db	00109$
	.db	00107$
	.db	00108$
	.db	00110$
	.db	00111$
	.db	00112$
	.db	00113$
	.db	00114$
	.db	00115$
	.db	00116$
	.db	00117$
	.db	00118$
	.db	00119$
	.db	00120$
	.db	00121$
	.db	00122$
	.db	00123$
	.db	00124$
	.db	00125$
	.db	00126$
	.db	00127$
	.db	00128$
	.db	00129$
	.db	00130$
	.db	00137$
	.db	00144$
	.db	00151$
	.db	00158$
	.db	00165$
	.db	00172$
	.db	00179$
	.db	00186$
	.db	00193$
	.db	00200$
	.db	00207$
	.db	00214$
	.db	00221$
	.db	00228$
00292$:
	.db	00101$>>8
	.db	00104$>>8
	.db	00109$>>8
	.db	00107$>>8
	.db	00108$>>8
	.db	00110$>>8
	.db	00111$>>8
	.db	00112$>>8
	.db	00113$>>8
	.db	00114$>>8
	.db	00115$>>8
	.db	00116$>>8
	.db	00117$>>8
	.db	00118$>>8
	.db	00119$>>8
	.db	00120$>>8
	.db	00121$>>8
	.db	00122$>>8
	.db	00123$>>8
	.db	00124$>>8
	.db	00125$>>8
	.db	00126$>>8
	.db	00127$>>8
	.db	00128$>>8
	.db	00129$>>8
	.db	00130$>>8
	.db	00137$>>8
	.db	00144$>>8
	.db	00151$>>8
	.db	00158$>>8
	.db	00165$>>8
	.db	00172$>>8
	.db	00179$>>8
	.db	00186$>>8
	.db	00193$>>8
	.db	00200$>>8
	.db	00207$>>8
	.db	00214$>>8
	.db	00221$>>8
	.db	00228$>>8
;src/qs1r_main.c:506: case VRQ_I2C_READ:
00101$:
;src/qs1r_main.c:507: if (!i2c_read (wValueL, EP0BUF, wLengthL))
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genAddrOf
	mov	_i2c_read_PARM_2,#_EP0BUF
	mov	(_i2c_read_PARM_2 + 1),#(_EP0BUF >> 8)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
	mov	_i2c_read_PARM_3,a
;     genCall
	mov	dpl,r2
	lcall	_i2c_read
	mov	a,dpl
;     genIfx
;     genIfxJump
;	Peephole 109	removed ljmp by inverse jump logic
;src/qs1r_main.c:508: return 0;
;     genRet
;	Peephole 256.c	loading dpl with zero from a
	jnz	00103$
00293$:
	mov	dpl,a
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00103$:
;src/qs1r_main.c:509: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:510: EP0BCL = wLengthL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BCL
	movx	@dptr,a
;src/qs1r_main.c:511: break;
	ljmp	00238$
;src/qs1r_main.c:513: case VRQ_SPI_READ:
00104$:
;src/qs1r_main.c:514: if (!spi_read (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, wLengthL))
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0003)
	movx	a,@dptr
	mov	r2,a
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	_spi_read_PARM_2,a
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0005)
	movx	a,@dptr
	mov	_spi_read_PARM_3,a
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	_spi_read_PARM_4,a
;     genAddrOf
	mov	_spi_read_PARM_5,#_EP0BUF
	mov	(_spi_read_PARM_5 + 1),#(_EP0BUF >> 8)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
	mov	_spi_read_PARM_6,a
;     genCall
	mov	dpl,r2
	lcall	_spi_read
	mov	a,dpl
;     genIfx
;     genIfxJump
;	Peephole 109	removed ljmp by inverse jump logic
;src/qs1r_main.c:515: return 0;
;     genRet
;	Peephole 256.c	loading dpl with zero from a
	jnz	00106$
00294$:
	mov	dpl,a
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00106$:
;src/qs1r_main.c:516: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:517: EP0BCL = wLengthL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BCL
	movx	@dptr,a
;src/qs1r_main.c:518: break;
	ljmp	00238$
;src/qs1r_main.c:520: case VRQ_EEPROM_TYPE_READ:
00107$:
;src/qs1r_main.c:521: EP0BUF[0] = I2CS & bmID; // 16 = 2 byte, 8 = 1 byte
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x18
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
	mov	a,r2
	movx	@dptr,a
;src/qs1r_main.c:522: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:523: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:524: break;
	ljmp	00238$
;src/qs1r_main.c:526: case VRQ_I2C_SPEED_READ:
00108$:
;src/qs1r_main.c:527: EP0BUF[0] = I2CTL & bm400KHZ; // 0 = 100 kHz, 1 = 400 kHz
;     genAssign
	mov	dptr,#_I2CTL
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x01
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
	mov	a,r2
	movx	@dptr,a
;src/qs1r_main.c:528: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:529: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:530: break;
	ljmp	00238$
;src/qs1r_main.c:532: case VRQ_SN_READ:
00109$:
;src/qs1r_main.c:533: break;
	ljmp	00238$
;src/qs1r_main.c:535: case VRQ_DEBUG_LED_READ:
00110$:
;src/qs1r_main.c:536: EP0BUF[0] = DEBUG_LED_REG;
;     genAssign
	mov	dptr,#_DEBUG_LED_REG
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:537: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:538: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:539: break;
	ljmp	00238$
;src/qs1r_main.c:541: case VRQ_LTC2208_CNTRL_READ:
00111$:
;src/qs1r_main.c:542: EP0BUF[0] = LTC2208_CNTRL_REG;
;     genAssign
	mov	dptr,#_LTC2208_CNTRL_REG
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:543: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:544: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:545: break;
	ljmp	00238$
;src/qs1r_main.c:547: case VRQ_ATTN_READ:
00112$:
;src/qs1r_main.c:548: EP0BUF[0] = ATTN;
;     genAssign
	mov	dptr,#_ATTN
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:549: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:550: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:551: break;
	ljmp	00238$
;src/qs1r_main.c:553: case VRQ_RFFE_READ:
00113$:
;src/qs1r_main.c:554: EP0BUF[0] = RFFE_CNTRL;
;     genAssign
	mov	dptr,#_RFFE_CNTRL
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:555: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:556: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:557: break;
	ljmp	00238$
;src/qs1r_main.c:559: case VRQ_DDC_CNTRL_READ:
00114$:
;src/qs1r_main.c:560: EP0BUF[0] = DDC_CNTRL;
;     genAssign
	mov	dptr,#_DDC_CNTRL
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:561: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:562: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:563: break;
	ljmp	00238$
;src/qs1r_main.c:565: case VRQ_CPLD_GPIO_READ:
00115$:
;src/qs1r_main.c:566: EP0BUF[0] = CPLD_GPIO;
;     genAssign
	mov	dptr,#_CPLD_GPIO
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:567: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:568: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:569: break;
	ljmp	00238$
;src/qs1r_main.c:571: case VRQ_DDC0_DR0_READ:
00116$:
;src/qs1r_main.c:572: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:573: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:574: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:575: break;
	ljmp	00238$
;src/qs1r_main.c:577: case VRQ_DDC0_DR1_READ:
00117$:
;src/qs1r_main.c:578: EP0BUF[0] = DDC0_DR1;
;     genAssign
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:579: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:580: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:581: break;
	ljmp	00238$
;src/qs1r_main.c:583: case VRQ_DDC0_DR2_READ:
00118$:
;src/qs1r_main.c:584: EP0BUF[0] = DDC0_DR2;
;     genAssign
	mov	dptr,#_DDC0_DR2
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:585: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:586: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:587: break;
	ljmp	00238$
;src/qs1r_main.c:589: case VRQ_DDC0_DR3_READ:
00119$:
;src/qs1r_main.c:590: EP0BUF[0] = DDC0_DR3;
;     genAssign
	mov	dptr,#_DDC0_DR3
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:591: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:592: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:593: break;
	ljmp	00238$
;src/qs1r_main.c:595: case VRQ_DDC0_DR4_READ:
00120$:
;src/qs1r_main.c:596: EP0BUF[0] = DDC0_DR4;
;     genAssign
	mov	dptr,#_DDC0_DR4
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:597: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:598: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:599: break;
	ljmp	00238$
;src/qs1r_main.c:601: case VRQ_DDC0_LAR_READ:
00121$:
;src/qs1r_main.c:602: EP0BUF[0] = DDC0_LAR;
;     genAssign
	mov	dptr,#_DDC0_LAR
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:603: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:604: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:605: break;
	ljmp	00238$
;src/qs1r_main.c:607: case VRQ_DDC0_AMR_READ:
00122$:
;src/qs1r_main.c:608: EP0BUF[0] = DDC0_AMR;
;     genAssign
	mov	dptr,#_DDC0_AMR
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:609: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:610: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:611: break;
	ljmp	00238$
;src/qs1r_main.c:613: case VRQ_DDC1_DR0_READ:
00123$:
;src/qs1r_main.c:614: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:615: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:616: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:617: break;
	ljmp	00238$
;src/qs1r_main.c:619: case VRQ_DDC1_DR1_READ:
00124$:
;src/qs1r_main.c:620: EP0BUF[0] = DDC1_DR1;
;     genAssign
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:621: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:622: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:623: break;
	ljmp	00238$
;src/qs1r_main.c:625: case VRQ_DDC1_DR2_READ:
00125$:
;src/qs1r_main.c:626: EP0BUF[0] = DDC1_DR2;
;     genAssign
	mov	dptr,#_DDC1_DR2
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:627: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:628: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:629: break;
	ljmp	00238$
;src/qs1r_main.c:631: case VRQ_DDC1_DR3_READ:
00126$:
;src/qs1r_main.c:632: EP0BUF[0] = DDC1_DR3;
;     genAssign
	mov	dptr,#_DDC1_DR3
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:633: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:634: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:635: break;
	ljmp	00238$
;src/qs1r_main.c:637: case VRQ_DDC1_DR4_READ:
00127$:
;src/qs1r_main.c:638: EP0BUF[0] = DDC1_DR4;
;     genAssign
	mov	dptr,#_DDC1_DR4
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:639: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:640: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:641: break;
	ljmp	00238$
;src/qs1r_main.c:643: case VRQ_DDC1_LAR_READ:
00128$:
;src/qs1r_main.c:644: EP0BUF[0] = DDC1_LAR;
;     genAssign
	mov	dptr,#_DDC1_LAR
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:645: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:646: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:647: break;
	ljmp	00238$
;src/qs1r_main.c:649: case VRQ_DDC1_AMR_READ:
00129$:
;src/qs1r_main.c:650: EP0BUF[0] = DDC1_AMR;
;     genAssign
	mov	dptr,#_DDC1_AMR
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:651: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:652: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:653: break;
	ljmp	00238$
;src/qs1r_main.c:655: case VRQ_DDC_MCR_READ:
00130$:
;src/qs1r_main.c:656: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00296$
00295$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00135$
00296$:
;src/qs1r_main.c:657: DDC0_AMR = (MCR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:658: DDC0_LAR = (MCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:659: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00136$
00135$:
;src/qs1r_main.c:660: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00132$
;00297$:
;	Peephole 200	removed redundant sjmp
00298$:
;src/qs1r_main.c:661: DDC1_AMR = (MCR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:662: DDC1_LAR = (MCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:663: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00136$
00132$:
;src/qs1r_main.c:664: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00136$:
;src/qs1r_main.c:665: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:666: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:667: break;
	ljmp	00238$
;src/qs1r_main.c:669: case VRQ_DDC_NCR_READ:
00137$:
;src/qs1r_main.c:670: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00300$
00299$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00142$
00300$:
;src/qs1r_main.c:671: DDC0_AMR = (NCR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:672: DDC0_LAR = (NCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:673: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00143$
00142$:
;src/qs1r_main.c:674: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00139$
;00301$:
;	Peephole 200	removed redundant sjmp
00302$:
;src/qs1r_main.c:675: DDC1_AMR = (NCR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:676: DDC1_LAR = (NCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:677: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00143$
00139$:
;src/qs1r_main.c:678: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00143$:
;src/qs1r_main.c:679: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:680: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:681: break;
	ljmp	00238$
;src/qs1r_main.c:683: case VRQ_DDC_NSCR_READ:
00144$:
;src/qs1r_main.c:684: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00304$
00303$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00149$
00304$:
;src/qs1r_main.c:685: DDC0_AMR = (NSCR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:686: DDC0_LAR = (NSCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x02
	movx	@dptr,a
;src/qs1r_main.c:687: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:688: EP0BUF[1] = DDC0_DR1;
;     genAssign
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	@dptr,a
;src/qs1r_main.c:689: EP0BUF[2] = DDC0_DR2;
;     genAssign
	mov	dptr,#_DDC0_DR2
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0002)
	movx	@dptr,a
;src/qs1r_main.c:690: EP0BUF[3] = DDC0_DR3;
;     genAssign
	mov	dptr,#_DDC0_DR3
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0003)
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00150$
00149$:
;src/qs1r_main.c:691: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00146$
;00305$:
;	Peephole 200	removed redundant sjmp
00306$:
;src/qs1r_main.c:692: DDC1_AMR = (NSCR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:693: DDC1_LAR = (NSCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x02
	movx	@dptr,a
;src/qs1r_main.c:694: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:695: EP0BUF[1] = DDC1_DR1;
;     genAssign
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	@dptr,a
;src/qs1r_main.c:696: EP0BUF[2] = DDC1_DR2;
;     genAssign
	mov	dptr,#_DDC1_DR2
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0002)
	movx	@dptr,a
;src/qs1r_main.c:697: EP0BUF[3] = DDC1_DR3;
;     genAssign
	mov	dptr,#_DDC1_DR3
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0003)
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00150$
00146$:
;src/qs1r_main.c:698: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00150$:
;src/qs1r_main.c:699: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:700: EP0BCL = 4;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x04
	movx	@dptr,a
;src/qs1r_main.c:701: break;
	ljmp	00238$
;src/qs1r_main.c:703: case VRQ_DDC_NFRE_READ:
00151$:
;src/qs1r_main.c:704: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00308$
00307$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00156$
00308$:
;src/qs1r_main.c:705: DDC0_AMR = (NFRE >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:706: DDC0_LAR = (NFRE & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:707: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:708: EP0BUF[1] = DDC0_DR1;
;     genAssign
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	@dptr,a
;src/qs1r_main.c:709: EP0BUF[2] = DDC0_DR2;
;     genAssign
	mov	dptr,#_DDC0_DR2
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0002)
	movx	@dptr,a
;src/qs1r_main.c:710: EP0BUF[3] = DDC0_DR3;
;     genAssign
	mov	dptr,#_DDC0_DR3
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0003)
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00157$
00156$:
;src/qs1r_main.c:711: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00153$
;00309$:
;	Peephole 200	removed redundant sjmp
00310$:
;src/qs1r_main.c:712: DDC1_AMR = (NFRE >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:713: DDC1_LAR = (NFRE & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:714: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:715: EP0BUF[1] = DDC1_DR1;
;     genAssign
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	@dptr,a
;src/qs1r_main.c:716: EP0BUF[2] = DDC1_DR2;
;     genAssign
	mov	dptr,#_DDC1_DR2
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0002)
	movx	@dptr,a
;src/qs1r_main.c:717: EP0BUF[3] = DDC1_DR3;
;     genAssign
	mov	dptr,#_DDC1_DR3
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0003)
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00157$
00153$:
;src/qs1r_main.c:718: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00157$:
;src/qs1r_main.c:719: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:720: EP0BCL = 4;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x04
	movx	@dptr,a
;src/qs1r_main.c:721: break;
	ljmp	00238$
;src/qs1r_main.c:723: case VRQ_DDC_NPO_READ:
00158$:
;src/qs1r_main.c:724: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00312$
00311$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00163$
00312$:
;src/qs1r_main.c:725: DDC0_AMR = (NPHA >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:726: DDC0_LAR = (NPHA & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x04
	movx	@dptr,a
;src/qs1r_main.c:727: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:728: EP0BUF[1] = DDC0_DR1;
;     genAssign
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00164$
00163$:
;src/qs1r_main.c:729: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00160$
;00313$:
;	Peephole 200	removed redundant sjmp
00314$:
;src/qs1r_main.c:730: DDC1_AMR = (NPHA >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:731: DDC1_LAR = (NPHA & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x04
	movx	@dptr,a
;src/qs1r_main.c:732: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;src/qs1r_main.c:733: EP0BUF[1] = DDC1_DR1;
;     genAssign
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00164$
00160$:
;src/qs1r_main.c:734: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00164$:
;src/qs1r_main.c:735: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:736: EP0BCL = 2;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x02
	movx	@dptr,a
;src/qs1r_main.c:737: break;
	ljmp	00238$
;src/qs1r_main.c:739: case VRQ_DDC_C2SR_READ:
00165$:
;src/qs1r_main.c:740: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00316$
00315$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00170$
00316$:
;src/qs1r_main.c:741: DDC0_AMR = (C2SR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:742: DDC0_LAR = (C2SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x05
	movx	@dptr,a
;src/qs1r_main.c:743: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00171$
00170$:
;src/qs1r_main.c:744: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00167$
;00317$:
;	Peephole 200	removed redundant sjmp
00318$:
;src/qs1r_main.c:745: DDC1_AMR = (C2SR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:746: DDC1_LAR = (C2SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x05
	movx	@dptr,a
;src/qs1r_main.c:747: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00171$
00167$:
;src/qs1r_main.c:748: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00171$:
;src/qs1r_main.c:749: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:750: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:751: break;
	ljmp	00238$
;src/qs1r_main.c:753: case VRQ_DDC_C2DEC_READ:
00172$:
;src/qs1r_main.c:754: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00320$
00319$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00177$
00320$:
;src/qs1r_main.c:755: DDC0_AMR = ((C2DEC & 0xFF00) >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:756: DDC0_LAR = (C2DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x06
	movx	@dptr,a
;src/qs1r_main.c:757: foop = DDC0_DR1;
;     genDummyRead
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;src/qs1r_main.c:758: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00178$
00177$:
;src/qs1r_main.c:759: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00174$
;00321$:
;	Peephole 200	removed redundant sjmp
00322$:
;src/qs1r_main.c:760: DDC1_AMR = ((C2DEC & 0xFF00) >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:761: DDC1_LAR = (C2DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x06
	movx	@dptr,a
;src/qs1r_main.c:762: foop = DDC1_DR1;
;     genDummyRead
	mov	dptr,#_DDC1_DR1
	movx	a,@dptr
;src/qs1r_main.c:763: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00178$
00174$:
;src/qs1r_main.c:764: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00178$:
;src/qs1r_main.c:765: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:766: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:767: break;
	ljmp	00238$
;src/qs1r_main.c:769: case VRQ_DDC_C5SR_READ:
00179$:
;src/qs1r_main.c:770: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00324$
00323$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00184$
00324$:
;src/qs1r_main.c:771: DDC0_AMR = (C5SR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:772: DDC0_LAR = (C5SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x07
	movx	@dptr,a
;src/qs1r_main.c:773: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00185$
00184$:
;src/qs1r_main.c:774: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00181$
;00325$:
;	Peephole 200	removed redundant sjmp
00326$:
;src/qs1r_main.c:775: DDC1_AMR = (C5SR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:776: DDC1_LAR = (C5SR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x07
	movx	@dptr,a
;src/qs1r_main.c:777: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00185$
00181$:
;src/qs1r_main.c:778: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00185$:
;src/qs1r_main.c:779: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:780: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:781: break;
	ljmp	00238$
;src/qs1r_main.c:783: case VRQ_DDC_C5DEC_READ:
00186$:
;src/qs1r_main.c:784: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00328$
00327$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00191$
00328$:
;src/qs1r_main.c:785: DDC0_AMR = (C5DEC >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:786: DDC0_LAR = (C5DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x08
	movx	@dptr,a
;src/qs1r_main.c:787: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00192$
00191$:
;src/qs1r_main.c:788: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00188$
;00329$:
;	Peephole 200	removed redundant sjmp
00330$:
;src/qs1r_main.c:789: DDC1_AMR = (C5DEC >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:790: DDC1_LAR = (C5DEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x08
	movx	@dptr,a
;src/qs1r_main.c:791: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00192$
00188$:
;src/qs1r_main.c:792: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00192$:
;src/qs1r_main.c:793: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:794: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:795: break;
	ljmp	00238$
;src/qs1r_main.c:797: case VRQ_DDC_RCR_READ:
00193$:
;src/qs1r_main.c:798: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00332$
00331$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00198$
00332$:
;src/qs1r_main.c:799: DDC0_AMR = (RCR >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:800: DDC0_LAR = (RCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x09
	movx	@dptr,a
;src/qs1r_main.c:801: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00199$
00198$:
;src/qs1r_main.c:802: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00195$
;00333$:
;	Peephole 200	removed redundant sjmp
00334$:
;src/qs1r_main.c:803: DDC1_AMR = (RCR >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:804: DDC1_LAR = (RCR & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x09
	movx	@dptr,a
;src/qs1r_main.c:805: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00199$
00195$:
;src/qs1r_main.c:806: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00199$:
;src/qs1r_main.c:807: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:808: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:809: break;
	ljmp	00238$
;src/qs1r_main.c:811: case VRQ_DDC_RDEC_READ:
00200$:
;src/qs1r_main.c:812: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00336$
00335$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00205$
00336$:
;src/qs1r_main.c:813: DDC0_AMR = (RDEC >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:814: DDC0_LAR = (RDEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0A
	movx	@dptr,a
;src/qs1r_main.c:815: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00206$
00205$:
;src/qs1r_main.c:816: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00202$
;00337$:
;	Peephole 200	removed redundant sjmp
00338$:
;src/qs1r_main.c:817: DDC1_AMR = (RDEC >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:818: DDC1_LAR = (RDEC & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0A
	movx	@dptr,a
;src/qs1r_main.c:819: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00206$
00202$:
;src/qs1r_main.c:820: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00206$:
;src/qs1r_main.c:821: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:822: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:823: break;
	ljmp	00238$
;src/qs1r_main.c:825: case VRQ_DDC_RAO_READ:
00207$:
;src/qs1r_main.c:826: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00340$
00339$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00212$
00340$:
;src/qs1r_main.c:827: DDC0_AMR = (RAO >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:828: DDC0_LAR = (RAO & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0B
	movx	@dptr,a
;src/qs1r_main.c:829: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00213$
00212$:
;src/qs1r_main.c:830: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00209$
;00341$:
;	Peephole 200	removed redundant sjmp
00342$:
;src/qs1r_main.c:831: DDC1_AMR = (RAO >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:832: DDC1_LAR = (RAO & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0B
	movx	@dptr,a
;src/qs1r_main.c:833: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00213$
00209$:
;src/qs1r_main.c:834: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00213$:
;src/qs1r_main.c:835: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:836: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:837: break;
	ljmp	00238$
;src/qs1r_main.c:839: case VRQ_DDC_RTAPS_READ:
00214$:
;src/qs1r_main.c:840: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00344$
00343$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00219$
00344$:
;src/qs1r_main.c:841: DDC0_AMR = (RTAP >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:842: DDC0_LAR = (RTAP & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0C
	movx	@dptr,a
;src/qs1r_main.c:843: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00220$
00219$:
;src/qs1r_main.c:844: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00216$
;00345$:
;	Peephole 200	removed redundant sjmp
00346$:
;src/qs1r_main.c:845: DDC1_AMR = (RTAP >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:846: DDC1_LAR = (RTAP & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0C
	movx	@dptr,a
;src/qs1r_main.c:847: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00220$
00216$:
;src/qs1r_main.c:848: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00220$:
;src/qs1r_main.c:849: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:850: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:851: break;
	ljmp	00238$
;src/qs1r_main.c:853: case VRQ_DDC_RSVR_READ:
00221$:
;src/qs1r_main.c:854: if (wIndexL == 0) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00348$
00347$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00226$
00348$:
;src/qs1r_main.c:855: DDC0_AMR = (RSRV >> 8);
;     genAssign
	mov	dptr,#_DDC0_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:856: DDC0_LAR = (RSRV & 0xFF);
;     genAssign
	mov	dptr,#_DDC0_LAR
	mov	a,#0x0D
	movx	@dptr,a
;src/qs1r_main.c:857: EP0BUF[0] = DDC0_DR0;
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00227$
00226$:
;src/qs1r_main.c:858: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00223$
;00349$:
;	Peephole 200	removed redundant sjmp
00350$:
;src/qs1r_main.c:859: DDC1_AMR = (RSRV >> 8);
;     genAssign
	mov	dptr,#_DDC1_AMR
	mov	a,#0x03
	movx	@dptr,a
;src/qs1r_main.c:860: DDC1_LAR = (RSRV & 0xFF);
;     genAssign
	mov	dptr,#_DDC1_LAR
	mov	a,#0x0D
	movx	@dptr,a
;src/qs1r_main.c:861: EP0BUF[0] = DDC1_DR0;
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00227$
00223$:
;src/qs1r_main.c:862: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00227$:
;src/qs1r_main.c:863: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:864: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;src/qs1r_main.c:865: break;
	ljmp	00238$
;src/qs1r_main.c:867: case VRQ_DDC_COEF_READ:
00228$:
;src/qs1r_main.c:868: if (wValueL > 0x0F) return 0;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genCmpGt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
;	Peephole 177.a	removed redundant mov
	mov  r2,a
	add	a,#0xff - 0x0F
	jnc	00230$
00351$:
;     genRet
	mov	dpl,#0x00
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
00230$:
;src/qs1r_main.c:869: offset = wValueL << 4; // advance by 16
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genLeftShift
;     genLeftShiftLiteral
;     genlshOne
;	Peephole 105	removed redundant mov
	mov	r2,a
	swap	a
	anl	a,#0xf0
	mov	_app_vendor_IN_cmd_offset_1_1,a
;src/qs1r_main.c:870: if (wIndexL == 0)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r3,a
	jz	00353$
00352$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00235$
00353$:
;src/qs1r_main.c:872: for (i=0, j=0; i < 16; i++, j+=3) {
;     genAssign
	mov	r3,#0x00
	mov	r4,#0x00
;     genAssign
	mov	r5,#0x00
	mov	r6,#0x00
00239$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r3
	subb	a,#0x10
	mov	a,r4
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
	jc	00354$
	ljmp	00236$
00354$:
;src/qs1r_main.c:873: DDC0_AMR = 0;
;     genAssign
	mov	dptr,#_DDC0_AMR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:874: DDC0_LAR = ((i + offset) & 0xFF);
;     genCast
	mov	ar7,r3
;     genPlus
	mov	dptr,#_DDC0_LAR
	mov	a,_app_vendor_IN_cmd_offset_1_1
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
	movx	@dptr,a
;src/qs1r_main.c:875: EP0BUF[j] = DDC0_DR0;
;     genPlus
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
	add	a,#_EP0BUF
	mov	r7,a
;	Peephole 236.g	used r6 instead of ar6
	mov	a,r6
	addc	a,#(_EP0BUF >> 8)
	mov	r0,a
;     genAssign
	mov	dptr,#_DDC0_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r1,a
	mov	dpl,r7
	mov	dph,r0
	movx	@dptr,a
;src/qs1r_main.c:876: EP0BUF[j+1] = DDC0_DR1;
;     genCast
	mov	ar7,r5
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
;     genPlus
	add	a,#_EP0BUF
	mov	r0,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP0BUF >> 8)
	mov	r1,a
;     genAssign
	mov	dptr,#_DDC0_DR1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r2,a
	mov	dpl,r0
	mov	dph,r1
	movx	@dptr,a
;src/qs1r_main.c:877: EP0BUF[j+2] = (DDC0_DR2 & 0x0F);
;     genPlus
;     genPlusIncr
	mov	a,#0x02
;	Peephole 236.a	used r7 instead of ar7
	add	a,r7
;     genPlus
	add	a,#_EP0BUF
	mov	r2,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP0BUF >> 8)
	mov	r7,a
;     genAssign
	mov	dptr,#_DDC0_DR2
	movx	a,@dptr
	mov	r0,a
;     genAnd
	anl	ar0,#0x0F
;     genPointerSet
;     genFarPointerSet
	mov	dpl,r2
	mov	dph,r7
	mov	a,r0
	movx	@dptr,a
;src/qs1r_main.c:872: for (i=0, j=0; i < 16; i++, j+=3) {
;     genPlus
;     genPlusIncr
	inc	r3
	cjne	r3,#0x00,00355$
	inc	r4
00355$:
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
	sjmp	00239$
00235$:
;src/qs1r_main.c:879: } else if (wIndexL == 1) {
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00232$
;00356$:
;	Peephole 200	removed redundant sjmp
00357$:
;src/qs1r_main.c:880: for (i=0, j=0; i < 16; i++, j+=3) {
;     genAssign
	mov	r2,#0x00
	mov	r3,#0x00
;     genAssign
	mov	r4,#0x00
	mov	r5,#0x00
00243$:
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,#0x10
	mov	a,r3
	xrl	a,#0x80
	subb	a,#0x80
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00236$
00358$:
;src/qs1r_main.c:881: DDC1_AMR = 0;
;     genAssign
	mov	dptr,#_DDC1_AMR
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:882: DDC1_LAR = ((i + offset) & 0xFF);
;     genCast
	mov	ar6,r2
;     genPlus
	mov	dptr,#_DDC1_LAR
	mov	a,_app_vendor_IN_cmd_offset_1_1
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
	movx	@dptr,a
;src/qs1r_main.c:883: EP0BUF[j] = DDC1_DR0;
;     genPlus
;	Peephole 236.g	used r4 instead of ar4
	mov	a,r4
	add	a,#_EP0BUF
	mov	r6,a
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
	addc	a,#(_EP0BUF >> 8)
	mov	r7,a
;     genAssign
	mov	dptr,#_DDC1_DR0
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r0,a
	mov	dpl,r6
	mov	dph,r7
	movx	@dptr,a
;src/qs1r_main.c:884: EP0BUF[j+1] = DDC1_DR1;
;     genCast
	mov	ar6,r4
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
;     genPlus
	add	a,#_EP0BUF
	mov	r7,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP0BUF >> 8)
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
;src/qs1r_main.c:885: EP0BUF[j+2] = (DDC1_DR2 & 0x0F);
;     genPlus
;     genPlusIncr
	mov	a,#0x02
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
;     genPlus
	add	a,#_EP0BUF
	mov	r6,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP0BUF >> 8)
	mov	r7,a
;     genAssign
	mov	dptr,#_DDC1_DR2
	movx	a,@dptr
	mov	r0,a
;     genAnd
	anl	ar0,#0x0F
;     genPointerSet
;     genFarPointerSet
	mov	dpl,r6
	mov	dph,r7
	mov	a,r0
	movx	@dptr,a
;src/qs1r_main.c:880: for (i=0, j=0; i < 16; i++, j+=3) {
;     genPlus
;     genPlusIncr
	inc	r2
	cjne	r2,#0x00,00359$
	inc	r3
00359$:
;     genPlus
;     genPlusIncr
	mov	a,#0x03
;	Peephole 236.a	used r4 instead of ar4
	add	a,r4
	mov	r4,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r5 instead of ar5
	addc	a,r5
	mov	r5,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00243$
00232$:
;src/qs1r_main.c:887: } else return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00236$:
;src/qs1r_main.c:888: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;src/qs1r_main.c:889: EP0BCL = 48;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x30
	movx	@dptr,a
;src/qs1r_main.c:890: break;
;src/qs1r_main.c:892: default:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00238$
00237$:
;src/qs1r_main.c:893: return 0;
;     genRet
	mov	dpl,#0x00
;src/qs1r_main.c:894: }
;	Peephole 112.b	changed ljmp to sjmp
;src/qs1r_main.c:895: return 1;
;     genRet
;	Peephole 237.a	removed sjmp to ret
	ret
00238$:
	mov	dpl,#0x01
00247$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'app_vendor_cmd'
;------------------------------------------------------------
;------------------------------------------------------------
;src/qs1r_main.c:899: app_vendor_cmd (void)
;	-----------------------------------------
;	 function app_vendor_cmd
;	-----------------------------------------
_app_vendor_cmd:
;src/qs1r_main.c:901: if (bRequestType == VRT_VENDOR_IN)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0xC0,00105$
;00111$:
;	Peephole 200	removed redundant sjmp
00112$:
;src/qs1r_main.c:902: return app_vendor_IN_cmd();
;     genCall
;     genRet
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_app_vendor_IN_cmd
00105$:
;src/qs1r_main.c:903: else if (bRequestType == VRT_VENDOR_OUT)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x40,00102$
;00113$:
;	Peephole 200	removed redundant sjmp
00114$:
;src/qs1r_main.c:904: return app_vendor_OUT_cmd();
;     genCall
;     genRet
;	Peephole 112.b	changed ljmp to sjmp
;src/qs1r_main.c:906: return 0;    // invalid bRequestType
;     genRet
;	Peephole 237.a	removed sjmp to ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_app_vendor_OUT_cmd
00102$:
	mov	dpl,#0x00
00107$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'main_loop'
;------------------------------------------------------------
;------------------------------------------------------------
;src/qs1r_main.c:910: main_loop (void)
;	-----------------------------------------
;	 function main_loop
;	-----------------------------------------
_main_loop:
;src/qs1r_main.c:913: while (1)
00105$:
;src/qs1r_main.c:915: if (usb_setup_packet_avail ())
;     genIfx
;     genIfxJump
;	Peephole 111	removed ljmp by inverse jump logic
	jnb	__usb_got_SUDAV,00102$
00111$:
;src/qs1r_main.c:916: usb_handle_setup_packet ();
;     genCall
	lcall	_usb_handle_setup_packet
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00105$
00102$:
;src/qs1r_main.c:918: putchar(32);
;     genCall
	mov	dpl,#0x20
	lcall	_putchar
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00105$
00107$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'isr_tick'
;------------------------------------------------------------
;count                     Allocated with name '_isr_tick_count_1_1'
;------------------------------------------------------------
;src/qs1r_main.c:929: isr_tick (void) interrupt
;	-----------------------------------------
;	 function isr_tick
;	-----------------------------------------
_isr_tick:
	push	acc
;src/qs1r_main.c:932: if (--count == 0)
;     genMinus
;     genMinusDec
	dec	_isr_tick_count_1_1
;     genCmpEq
	mov	a,_isr_tick_count_1_1
;	Peephole 162	removed sjmp by inverse jump logic
	jz	00107$
00106$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00102$
00107$:
;src/qs1r_main.c:934: count = 50;
;     genAssign
	mov	_isr_tick_count_1_1,#0x32
00102$:
;src/qs1r_main.c:937: clear_timer_irq ();
;     genAssign
	clr	_TF2
00103$:
	pop	acc
	reti
;	eliminated unneeded push/pop psw
;	eliminated unneeded push/pop dpl
;	eliminated unneeded push/pop dph
;	eliminated unneeded push/pop b
;------------------------------------------------------------
;Allocation info for local variables in function 'patch_usb_descriptors'
;------------------------------------------------------------
;hw_rev                    Allocated with name '_patch_usb_descriptors_hw_rev_1_1'
;serial_no                 Allocated with name '_patch_usb_descriptors_serial_no_1_1'
;------------------------------------------------------------
;src/qs1r_main.c:945: patch_usb_descriptors(void)
;	-----------------------------------------
;	 function patch_usb_descriptors
;	-----------------------------------------
_patch_usb_descriptors:
;src/qs1r_main.c:950: eeprom_read(I2C_ADDR_BOOT, HW_REV_OFFSET, &hw_rev, 1);	// LSB of device id
;     genAddrOf
	mov	_eeprom_read_PARM_3,#_patch_usb_descriptors_hw_rev_1_1
	mov	(_eeprom_read_PARM_3 + 1),#(_patch_usb_descriptors_hw_rev_1_1 >> 8)
;     genAssign
	mov	_eeprom_read_PARM_2,#0x05
;     genAssign
	mov	_eeprom_read_PARM_4,#0x01
;     genCall
	mov	dpl,#0x51
	lcall	_eeprom_read
;src/qs1r_main.c:952: usb_desc_hw_rev_binary_patch_location_0[0] = hw_rev;
;     genAssign
	mov	dptr,#_patch_usb_descriptors_hw_rev_1_1
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_0
	movx	@dptr,a
;src/qs1r_main.c:953: usb_desc_hw_rev_binary_patch_location_1[0] = hw_rev;
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_1
	mov	a,r2
	movx	@dptr,a
;src/qs1r_main.c:954: usb_desc_hw_rev_ascii_patch_location_0[0] = hw_rev + '0';     // FIXME if we get > 9
;     genPlus
	mov	a,#0x30
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_usb_desc_hw_rev_ascii_patch_location_0
	movx	@dptr,a
00101$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'main'
;------------------------------------------------------------
;------------------------------------------------------------
;src/qs1r_main.c:965: main (void)
;	-----------------------------------------
;	 function main
;	-----------------------------------------
_main:
;src/qs1r_main.c:967: init_qs1r();
;     genCall
	lcall	_init_qs1r
;src/qs1r_main.c:969: EA = 0;		// disable all interrupts
;     genAssign
	clr	_EA
;src/qs1r_main.c:971: patch_usb_descriptors();
;     genCall
	lcall	_patch_usb_descriptors
;src/qs1r_main.c:973: setup_autovectors ();
;     genCall
	lcall	_setup_autovectors
;src/qs1r_main.c:974: usb_install_handlers ();
;     genCall
	lcall	_usb_install_handlers
;src/qs1r_main.c:975: hook_timer_tick ((unsigned short) isr_tick);
;     genCast
	mov	r2,#_isr_tick
	mov	r3,#(_isr_tick >> 8)
;     genCall
	mov	dpl,r2
	mov	dph,r3
	lcall	_hook_timer_tick
;src/qs1r_main.c:977: EIEX4 = 1;	// disable INT4 FIXME
;     genAssign
	setb	_EIEX4
;src/qs1r_main.c:978: EA = 1;			// global interrupt enable
;     genAssign
	setb	_EA
;src/qs1r_main.c:980: fx2_renumerate ();	// simulates disconnect / reconnect
;     genCall
	lcall	_fx2_renumerate
;src/qs1r_main.c:982: main_loop ();
;     genCall
;	Peephole 253.b	replaced lcall/ret with ljmp
	ljmp	_main_loop
	.area CSEG    (CODE)
