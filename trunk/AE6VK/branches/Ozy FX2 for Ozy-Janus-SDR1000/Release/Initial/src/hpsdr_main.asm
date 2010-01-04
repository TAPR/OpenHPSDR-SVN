;--------------------------------------------------------
; File Created by SDCC : free open source ANSI-C Compiler
; Version 2.9.0 #5416 (Mar 22 2009) (MINGW32)
; This file was generated Mon Jan 04 09:33:55 2010
;--------------------------------------------------------
	.module hpsdr_main
	.optsdcc -mmcs51 --model-small
	
;--------------------------------------------------------
; Public variables in this module
;--------------------------------------------------------
	.globl _main
	.globl _patch_usb_descriptors
	.globl _isr_tick
	.globl _app_vendor_IN_cmd
	.globl _app_vendor_OUT_cmd
	.globl _bitALTERA_DCLK
	.globl _bitALTERA_DATA0
	.globl _bitS_IN
	.globl _bitS_OUT
	.globl _bitS_CLK
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
_bitS_CLK	=	0x0080
_bitS_OUT	=	0x0081
_bitS_IN	=	0x0083
_bitALTERA_DATA0	=	0x00a0
_bitALTERA_DCLK	=	0x00a2
;--------------------------------------------------------
; overlayable register banks
;--------------------------------------------------------
	.area REG_BANK_0	(REL,OVR,DATA)
	.ds 8
;--------------------------------------------------------
; internal ram data
;--------------------------------------------------------
	.area DSEG    (DATA)
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
; absolute internal ram data
;--------------------------------------------------------
	.area IABS    (ABS,DATA)
	.area IABS    (ABS,DATA)
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
_patch_usb_descriptors_hw_rev_1_1:
	.ds 1
_patch_usb_descriptors_serial_no_1_1:
	.ds 8
;--------------------------------------------------------
; absolute external ram data
;--------------------------------------------------------
	.area XABS    (ABS,XDATA)
;--------------------------------------------------------
; external initialized ram data
;--------------------------------------------------------
	.area HOME    (CODE)
	.area GSINIT0 (CODE)
	.area GSINIT1 (CODE)
	.area GSINIT2 (CODE)
	.area GSINIT3 (CODE)
	.area GSINIT4 (CODE)
	.area GSINIT5 (CODE)
	.area GSINIT  (CODE)
	.area GSFINAL (CODE)
	.area CSEG    (CODE)
;--------------------------------------------------------
; interrupt vector 
;--------------------------------------------------------
	.area HOME    (CODE)
__interrupt_vect:
	ljmp	__sdcc_gsinit_startup
;--------------------------------------------------------
; global & static initialisations
;--------------------------------------------------------
	.area HOME    (CODE)
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
;	../Initial/src/hpsdr_main.c:266: static unsigned char	count = 1;  
	mov	_isr_tick_count_1_1,#0x01
	.area GSFINAL (CODE)
	ljmp	__sdcc_program_startup
;--------------------------------------------------------
; Home
;--------------------------------------------------------
	.area HOME    (CODE)
	.area HOME    (CODE)
__sdcc_program_startup:
	lcall	_main
;	return from main will lock up
	sjmp .
;--------------------------------------------------------
; code
;--------------------------------------------------------
	.area CSEG    (CODE)
;------------------------------------------------------------
;Allocation info for local variables in function 'get_ep0_data'
;------------------------------------------------------------
;------------------------------------------------------------
;	../Initial/src/hpsdr_main.c:56: get_ep0_data (void)
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
;	../Initial/src/hpsdr_main.c:58: EP0BCL = 0;			// arm EP0 for OUT xfer.  This sets the busy bit
	mov	dptr,#_EP0BCL
	clr	a
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:59: while (EP0CS & bmEPBUSY);	// wait for busy to clear
00101$:
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	jb	acc.1,00101$
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'app_vendor_OUT_cmd'
;------------------------------------------------------------
;------------------------------------------------------------
;	../Initial/src/hpsdr_main.c:69: unsigned char app_vendor_OUT_cmd(void)
;	-----------------------------------------
;	 function app_vendor_OUT_cmd
;	-----------------------------------------
_app_vendor_OUT_cmd:
;	../Initial/src/hpsdr_main.c:83: switch (bRequest)
	mov	dptr,#(_SETUPDAT + 0x0001)
	movx	a,@dptr
	mov	r2,a
	cjne	r2,#0x01,00154$
	sjmp	00101$
00154$:
	cjne	r2,#0x02,00155$
	sjmp	00106$
00155$:
	cjne	r2,#0x08,00156$
	ljmp	00112$
00156$:
	cjne	r2,#0x09,00157$
	ljmp	00115$
00157$:
	cjne	r2,#0x0B,00158$
	ljmp	00118$
00158$:
	cjne	r2,#0x0C,00159$
	ljmp	00125$
00159$:
	cjne	r2,#0x0D,00160$
	ljmp	00122$
00160$:
	ljmp	00132$
;	../Initial/src/hpsdr_main.c:86: case VRQ_SET_LED:
00101$:
;	../Initial/src/hpsdr_main.c:88: switch (wIndexL)
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
	jz	00102$
;	../Initial/src/hpsdr_main.c:90: case 0:
	cjne	r2,#0x01,00104$
	sjmp	00103$
00102$:
;	../Initial/src/hpsdr_main.c:91: set_led_0 (wValueL);
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	dpl,a
	lcall	_set_led_0
;	../Initial/src/hpsdr_main.c:92: break;
	ljmp	00133$
;	../Initial/src/hpsdr_main.c:94: case 1:
00103$:
;	../Initial/src/hpsdr_main.c:95: set_led_1 (wValueL);
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	dpl,a
	lcall	_set_led_1
;	../Initial/src/hpsdr_main.c:96: break;
	ljmp	00133$
;	../Initial/src/hpsdr_main.c:98: default:
00104$:
;	../Initial/src/hpsdr_main.c:99: return 0;
	mov	dpl,#0x00
	ret
;	../Initial/src/hpsdr_main.c:103: case VRQ_FPGA_LOAD:
00106$:
;	../Initial/src/hpsdr_main.c:105: switch (wIndexL)			// sub-command
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
	jz	00107$
	cjne	r2,#0x01,00164$
	sjmp	00108$
00164$:
;	../Initial/src/hpsdr_main.c:107: case FL_BEGIN:
	cjne	r2,#0x02,00110$
	sjmp	00109$
00107$:
;	../Initial/src/hpsdr_main.c:108: return fpga_load_begin ();
	ljmp	_fpga_load_begin
;	../Initial/src/hpsdr_main.c:110: case FL_XFER:
00108$:
;	../Initial/src/hpsdr_main.c:111: get_ep0_data ();
	lcall	_get_ep0_data
;	../Initial/src/hpsdr_main.c:112: return fpga_load_xfer (EP0BUF, EP0BCL);
	mov	dptr,#_EP0BCL
	movx	a,@dptr
	mov	_fpga_load_xfer_PARM_2,a
	mov	dptr,#_EP0BUF
	ljmp	_fpga_load_xfer
;	../Initial/src/hpsdr_main.c:114: case FL_END:
00109$:
;	../Initial/src/hpsdr_main.c:115: return fpga_load_end ();
	ljmp	_fpga_load_end
;	../Initial/src/hpsdr_main.c:117: default:
00110$:
;	../Initial/src/hpsdr_main.c:118: return 0;
	mov	dpl,#0x00
	ret
;	../Initial/src/hpsdr_main.c:122: case VRQ_I2C_WRITE:
00112$:
;	../Initial/src/hpsdr_main.c:124: get_ep0_data ();
	lcall	_get_ep0_data
;	../Initial/src/hpsdr_main.c:125: if (!i2c_write (wValueL, EP0BUF, EP0BCL))
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
	mov	_i2c_write_PARM_2,#_EP0BUF
	mov	(_i2c_write_PARM_2 + 1),#(_EP0BUF >> 8)
	mov	dptr,#_EP0BCL
	movx	a,@dptr
	mov	_i2c_write_PARM_3,a
	mov	dpl,r2
	lcall	_i2c_write
	mov	a,dpl
	jz	00166$
	ljmp	00133$
00166$:
;	../Initial/src/hpsdr_main.c:126: return 0;
	mov	dpl,#0x00
	ret
;	../Initial/src/hpsdr_main.c:129: case VRQ_SPI_WRITE:
00115$:
;	../Initial/src/hpsdr_main.c:131: get_ep0_data ();
	lcall	_get_ep0_data
;	../Initial/src/hpsdr_main.c:132: if (!spi_write (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, EP0BCL))
	mov	dptr,#(_SETUPDAT + 0x0003)
	movx	a,@dptr
	mov	r2,a
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	_spi_write_PARM_2,a
	mov	dptr,#(_SETUPDAT + 0x0005)
	movx	a,@dptr
	mov	_spi_write_PARM_3,a
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	_spi_write_PARM_4,a
	mov	_spi_write_PARM_5,#_EP0BUF
	mov	(_spi_write_PARM_5 + 1),#(_EP0BUF >> 8)
	mov	dptr,#_EP0BCL
	movx	a,@dptr
	mov	_spi_write_PARM_6,a
	mov	dpl,r2
	lcall	_spi_write
	mov	a,dpl
	jz	00167$
	ljmp	00133$
00167$:
;	../Initial/src/hpsdr_main.c:133: return 0;
	mov	dpl,#0x00
	ret
;	../Initial/src/hpsdr_main.c:136: case VRQ_I2C_SPEED_SET:
00118$:
;	../Initial/src/hpsdr_main.c:137: if (wValueL == 1)
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
	cjne	r2,#0x01,00120$
;	../Initial/src/hpsdr_main.c:138: I2CTL |= bm400KHZ;
	mov	dptr,#_I2CTL
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
	sjmp	00133$
00120$:
;	../Initial/src/hpsdr_main.c:140: I2CTL &= ~bm400KHZ;
	mov	dptr,#_I2CTL
	movx	a,@dptr
	mov	r2,a
	anl	a,#0xFE
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:141: break;
;	../Initial/src/hpsdr_main.c:144: case VRQ_SDR1K_CTL:
	sjmp	00133$
00122$:
;	../Initial/src/hpsdr_main.c:154: if (!SDR1k_ControlOut(wValueH, wValueL, wIndexH, wIndexL, EP0BUF, EP0BCL)) {
	mov	dptr,#(_SETUPDAT + 0x0003)
	movx	a,@dptr
	mov	r2,a
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	_SDR1k_ControlOut_PARM_2,a
	mov	dptr,#(_SETUPDAT + 0x0005)
	movx	a,@dptr
	mov	_SDR1k_ControlOut_PARM_3,a
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	_SDR1k_ControlOut_PARM_4,a
	mov	_SDR1k_ControlOut_PARM_5,#_EP0BUF
	mov	(_SDR1k_ControlOut_PARM_5 + 1),#(_EP0BUF >> 8)
	mov	dptr,#_EP0BCL
	movx	a,@dptr
	mov	_SDR1k_ControlOut_PARM_6,a
	mov	dpl,r2
	lcall	_SDR1k_ControlOut
	mov	a,dpl
;	../Initial/src/hpsdr_main.c:155: return 0;  
	jnz	00133$
	mov	dpl,a
;	../Initial/src/hpsdr_main.c:161: case VRQ_CPU_SPEED_SET:
	ret
00125$:
;	../Initial/src/hpsdr_main.c:162: if (wValueL == 0)
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	jnz	00130$
;	../Initial/src/hpsdr_main.c:163: CPUCS = bmCLKOE; // 12 MHz
	mov	dptr,#_CPUCS
	mov	a,#0x02
	movx	@dptr,a
	sjmp	00133$
00130$:
;	../Initial/src/hpsdr_main.c:164: else if (wValueL == 1)
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
	cjne	r2,#0x01,00127$
;	../Initial/src/hpsdr_main.c:165: CPUCS = bmCLKSPD0 | bmCLKOE; // 24 MHz
	mov	dptr,#_CPUCS
	mov	a,#0x0A
	movx	@dptr,a
	sjmp	00133$
00127$:
;	../Initial/src/hpsdr_main.c:167: CPUCS = bmCLKSPD1 | bmCLKOE; // 48 MHz
	mov	dptr,#_CPUCS
	mov	a,#0x12
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:168: break;
;	../Initial/src/hpsdr_main.c:170: default:
	sjmp	00133$
00132$:
;	../Initial/src/hpsdr_main.c:171: return 0;
	mov	dpl,#0x00
;	../Initial/src/hpsdr_main.c:172: }
;	../Initial/src/hpsdr_main.c:173: return 1;
	ret
00133$:
	mov	dpl,#0x01
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'app_vendor_IN_cmd'
;------------------------------------------------------------
;------------------------------------------------------------
;	../Initial/src/hpsdr_main.c:176: unsigned char app_vendor_IN_cmd(void)
;	-----------------------------------------
;	 function app_vendor_IN_cmd
;	-----------------------------------------
_app_vendor_IN_cmd:
;	../Initial/src/hpsdr_main.c:178: switch (bRequest)
	mov	dptr,#(_SETUPDAT + 0x0001)
	movx	a,@dptr
	mov	r2,a
	cjne	r2,#0x0D,00124$
	ljmp	00107$
00124$:
	cjne	r2,#0x81,00125$
	sjmp	00101$
00125$:
	cjne	r2,#0x82,00126$
	sjmp	00104$
00126$:
	cjne	r2,#0x84,00127$
	ljmp	00110$
00127$:
	cjne	r2,#0x85,00128$
	ljmp	00111$
00128$:
	ljmp	00112$
;	../Initial/src/hpsdr_main.c:180: case VRQ_I2C_READ:
00101$:
;	../Initial/src/hpsdr_main.c:181: if (!i2c_read (wValueL, EP0BUF, wLengthL))
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
	mov	_i2c_read_PARM_2,#_EP0BUF
	mov	(_i2c_read_PARM_2 + 1),#(_EP0BUF >> 8)
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
	mov	_i2c_read_PARM_3,a
	mov	dpl,r2
	lcall	_i2c_read
	mov	a,dpl
;	../Initial/src/hpsdr_main.c:182: return 0;
	jnz	00103$
	mov	dpl,a
	ret
00103$:
;	../Initial/src/hpsdr_main.c:183: EP0BCH = 0;
	mov	dptr,#_EP0BCH
	clr	a
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:184: EP0BCL = wLengthL;
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
	mov	r2,a
	mov	dptr,#_EP0BCL
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:185: break;
	ljmp	00113$
;	../Initial/src/hpsdr_main.c:187: case VRQ_SPI_READ:
00104$:
;	../Initial/src/hpsdr_main.c:188: if (!spi_read (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, wLengthL))
	mov	dptr,#(_SETUPDAT + 0x0003)
	movx	a,@dptr
	mov	r2,a
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	_spi_read_PARM_2,a
	mov	dptr,#(_SETUPDAT + 0x0005)
	movx	a,@dptr
	mov	_spi_read_PARM_3,a
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	_spi_read_PARM_4,a
	mov	_spi_read_PARM_5,#_EP0BUF
	mov	(_spi_read_PARM_5 + 1),#(_EP0BUF >> 8)
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
	mov	_spi_read_PARM_6,a
	mov	dpl,r2
	lcall	_spi_read
	mov	a,dpl
;	../Initial/src/hpsdr_main.c:189: return 0;
	jnz	00106$
	mov	dpl,a
	ret
00106$:
;	../Initial/src/hpsdr_main.c:190: EP0BCH = 0;
	mov	dptr,#_EP0BCH
	clr	a
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:191: EP0BCL = wLengthL;
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
	mov	r2,a
	mov	dptr,#_EP0BCL
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:192: break;
	ljmp	00113$
;	../Initial/src/hpsdr_main.c:195: case VRQ_SDR1K_CTL:
00107$:
;	../Initial/src/hpsdr_main.c:200: if (!SDR1k_ControlIn(wValueH, wValueL, wIndexH, wIndexL, EP0BUF, wLengthL) ) {
	mov	dptr,#(_SETUPDAT + 0x0003)
	movx	a,@dptr
	mov	r2,a
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	_SDR1k_ControlIn_PARM_2,a
	mov	dptr,#(_SETUPDAT + 0x0005)
	movx	a,@dptr
	mov	_SDR1k_ControlIn_PARM_3,a
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	_SDR1k_ControlIn_PARM_4,a
	mov	_SDR1k_ControlIn_PARM_5,#_EP0BUF
	mov	(_SDR1k_ControlIn_PARM_5 + 1),#(_EP0BUF >> 8)
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
	mov	_SDR1k_ControlIn_PARM_6,a
	mov	dpl,r2
	lcall	_SDR1k_ControlIn
	mov	a,dpl
	jnz	00109$
;	../Initial/src/hpsdr_main.c:201: putstr("sdr1kin error bailout\n"); 
	mov	dptr,#__str_0
	mov	b,#0x80
	lcall	_putstr
;	../Initial/src/hpsdr_main.c:202: return 0; 
	mov	dpl,#0x00
	ret
00109$:
;	../Initial/src/hpsdr_main.c:204: EP0BCH = 0;
	mov	dptr,#_EP0BCH
	clr	a
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:205: EP0BCL = wLengthL;
	mov	dptr,#(_SETUPDAT + 0x0006)
	movx	a,@dptr
	mov	r2,a
	mov	dptr,#_EP0BCL
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:206: break;        		                	 		 
;	../Initial/src/hpsdr_main.c:210: case VRQ_EEPROM_TYPE_READ:
	sjmp	00113$
00110$:
;	../Initial/src/hpsdr_main.c:211: EP0BUF[0] = I2CS & bmID; // 16 = 2 byte, 8 = 1 byte
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
	anl	ar2,#0x18
	mov	dptr,#_EP0BUF
	mov	a,r2
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:212: EP0BCH = 0;
	mov	dptr,#_EP0BCH
	clr	a
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:213: EP0BCL = 1;
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:214: break;
;	../Initial/src/hpsdr_main.c:216: case VRQ_I2C_SPEED_READ:
	sjmp	00113$
00111$:
;	../Initial/src/hpsdr_main.c:217: EP0BUF[0] = I2CTL & bm400KHZ; // 0 = 100 kHz, 1 = 400 kHz
	mov	dptr,#_I2CTL
	movx	a,@dptr
	mov	r2,a
	anl	ar2,#0x01
	mov	dptr,#_EP0BUF
	mov	a,r2
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:218: EP0BCH = 0;
	mov	dptr,#_EP0BCH
	clr	a
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:219: EP0BCL = 1;
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:220: break;
;	../Initial/src/hpsdr_main.c:222: default:
	sjmp	00113$
00112$:
;	../Initial/src/hpsdr_main.c:223: return 0;
	mov	dpl,#0x00
;	../Initial/src/hpsdr_main.c:224: }
;	../Initial/src/hpsdr_main.c:225: return 1;
	ret
00113$:
	mov	dpl,#0x01
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'app_vendor_cmd'
;------------------------------------------------------------
;------------------------------------------------------------
;	../Initial/src/hpsdr_main.c:229: app_vendor_cmd (void)
;	-----------------------------------------
;	 function app_vendor_cmd
;	-----------------------------------------
_app_vendor_cmd:
;	../Initial/src/hpsdr_main.c:234: if (bRequestType == VRT_VENDOR_IN)
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
	cjne	r2,#0xC0,00105$
;	../Initial/src/hpsdr_main.c:235: return app_vendor_IN_cmd();
	ljmp	_app_vendor_IN_cmd
00105$:
;	../Initial/src/hpsdr_main.c:236: else if (bRequestType == VRT_VENDOR_OUT)
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
	cjne	r2,#0x40,00102$
;	../Initial/src/hpsdr_main.c:237: return app_vendor_OUT_cmd();
;	../Initial/src/hpsdr_main.c:239: return 0;    // invalid bRequestType
	ljmp	_app_vendor_OUT_cmd
00102$:
	mov	dpl,#0x00
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'main_loop'
;------------------------------------------------------------
;------------------------------------------------------------
;	../Initial/src/hpsdr_main.c:243: main_loop (void)
;	-----------------------------------------
;	 function main_loop
;	-----------------------------------------
_main_loop:
;	../Initial/src/hpsdr_main.c:246: while (1)
00104$:
;	../Initial/src/hpsdr_main.c:248: if (usb_setup_packet_avail ())
	jnb	__usb_got_SUDAV,00104$
;	../Initial/src/hpsdr_main.c:249: usb_handle_setup_packet ();
	lcall	_usb_handle_setup_packet
	sjmp	00104$
;------------------------------------------------------------
;Allocation info for local variables in function 'isr_tick'
;------------------------------------------------------------
;count                     Allocated with name '_isr_tick_count_1_1'
;------------------------------------------------------------
;	../Initial/src/hpsdr_main.c:264: isr_tick (void) interrupt
;	-----------------------------------------
;	 function isr_tick
;	-----------------------------------------
_isr_tick:
	push	acc
;	../Initial/src/hpsdr_main.c:267: if (--count == 0)
	djnz	_isr_tick_count_1_1,00102$
;	../Initial/src/hpsdr_main.c:269: count = 50;     		
	mov	_isr_tick_count_1_1,#0x32
;	../Initial/src/hpsdr_main.c:270: HPSDR_LED_REG ^= bmLED0;
	xrl	_IOC,#0x40
00102$:
;	../Initial/src/hpsdr_main.c:281: clear_timer_irq ();
	clr	_TF2
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
;	../Initial/src/hpsdr_main.c:289: patch_usb_descriptors(void)
;	-----------------------------------------
;	 function patch_usb_descriptors
;	-----------------------------------------
_patch_usb_descriptors:
;	../Initial/src/hpsdr_main.c:294: eeprom_read(I2C_ADDR_BOOT, HW_REV_OFFSET, &hw_rev, 1);	// LSB of device id
	mov	_eeprom_read_PARM_3,#_patch_usb_descriptors_hw_rev_1_1
	mov	(_eeprom_read_PARM_3 + 1),#(_patch_usb_descriptors_hw_rev_1_1 >> 8)
	mov	_eeprom_read_PARM_2,#0x05
	mov	_eeprom_read_PARM_4,#0x01
	mov	dpl,#0x51
	lcall	_eeprom_read
;	../Initial/src/hpsdr_main.c:296: usb_desc_hw_rev_binary_patch_location_0[0] = hw_rev;
	mov	dptr,#_patch_usb_descriptors_hw_rev_1_1
	movx	a,@dptr
	mov	r2,a
	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_0
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:297: usb_desc_hw_rev_binary_patch_location_1[0] = hw_rev;
	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_1
	mov	a,r2
	movx	@dptr,a
;	../Initial/src/hpsdr_main.c:298: usb_desc_hw_rev_ascii_patch_location_0[0] = hw_rev + '0';     // FIXME if we get > 9
	mov	a,#0x30
	add	a,r2
	mov	dptr,#_usb_desc_hw_rev_ascii_patch_location_0
	movx	@dptr,a
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'main'
;------------------------------------------------------------
;------------------------------------------------------------
;	../Initial/src/hpsdr_main.c:309: main (void)
;	-----------------------------------------
;	 function main
;	-----------------------------------------
_main:
;	../Initial/src/hpsdr_main.c:311: init_hpsdr();
	lcall	_init_hpsdr
;	../Initial/src/hpsdr_main.c:314: set_led_0 (1);
	mov	dpl,#0x01
	lcall	_set_led_0
;	../Initial/src/hpsdr_main.c:315: set_led_1 (0);
	mov	dpl,#0x00
	lcall	_set_led_1
;	../Initial/src/hpsdr_main.c:317: EA = 0;		// disable all interrupts
	clr	_EA
;	../Initial/src/hpsdr_main.c:319: patch_usb_descriptors();
	lcall	_patch_usb_descriptors
;	../Initial/src/hpsdr_main.c:321: setup_autovectors ();
	lcall	_setup_autovectors
;	../Initial/src/hpsdr_main.c:322: usb_install_handlers ();
	lcall	_usb_install_handlers
;	../Initial/src/hpsdr_main.c:323: hook_timer_tick ((unsigned short) isr_tick);
	mov	r2,#_isr_tick
	mov	r3,#(_isr_tick >> 8)
	mov	dpl,r2
	mov	dph,r3
	lcall	_hook_timer_tick
;	../Initial/src/hpsdr_main.c:325: EIEX4 = 1;	// disable INT4 FIXME
	setb	_EIEX4
;	../Initial/src/hpsdr_main.c:326: EA = 1;			// global interrupt enable
	setb	_EA
;	../Initial/src/hpsdr_main.c:328: fx2_renumerate ();	// simulates disconnect / reconnect
	lcall	_fx2_renumerate
;	../Initial/src/hpsdr_main.c:331: putstr("\n\nHPSDR Ozy Firmware. ");
	mov	dptr,#__str_1
	mov	b,#0x80
	lcall	_putstr
;	../Initial/src/hpsdr_main.c:332: putstr(__DATE__); 
	mov	dptr,#__str_2
	mov	b,#0x80
	lcall	_putstr
;	../Initial/src/hpsdr_main.c:333: putstr(" "); 
	mov	dptr,#__str_3
	mov	b,#0x80
	lcall	_putstr
;	../Initial/src/hpsdr_main.c:334: putstr(__TIME__);
	mov	dptr,#__str_4
	mov	b,#0x80
	lcall	_putstr
;	../Initial/src/hpsdr_main.c:335: putstr("\nCopyright 2003, 2004, 2006, 2007 Phil Covington (N8VB), Bill Tracey (KD5TFD),\nand Free Software Foundataion, Inc.\n\n");
	mov	dptr,#__str_5
	mov	b,#0x80
	lcall	_putstr
;	../Initial/src/hpsdr_main.c:336: putstr("This code is licensed under the GNU General Public License Version 2\n\n");   
	mov	dptr,#__str_6
	mov	b,#0x80
	lcall	_putstr
;	../Initial/src/hpsdr_main.c:338: main_loop ();
	ljmp	_main_loop
	.area CSEG    (CODE)
	.area CONST   (CODE)
__str_0:
	.ascii "sdr1kin error bailout"
	.db 0x0A
	.db 0x00
__str_1:
	.db 0x0A
	.db 0x0A
	.ascii "HPSDR Ozy Firmware. "
	.db 0x00
__str_2:
	.ascii "Jan  4 2010"
	.db 0x00
__str_3:
	.ascii " "
	.db 0x00
__str_4:
	.ascii "09:33:55"
	.db 0x00
__str_5:
	.db 0x0A
	.ascii "Copyright 2003, 2004, 2006, 2007 Phil Covington (N8VB), Bil"
	.ascii "l Tracey (KD5TFD),"
	.db 0x0A
	.ascii "and Free Software Foundataion, Inc."
	.db 0x0A
	.db 0x0A
	.db 0x00
__str_6:
	.ascii "This code is licensed under the GNU General Public License V"
	.ascii "ersion 2"
	.db 0x0A
	.db 0x0A
	.db 0x00
	.area CABS    (ABS,CODE)
