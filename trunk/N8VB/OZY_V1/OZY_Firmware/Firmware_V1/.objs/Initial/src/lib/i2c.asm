;--------------------------------------------------------
; File Created by SDCC : FreeWare ANSI-C Compiler
; Version 2.5.0 #1020 (May  8 2005)
; This file generated Mon Jul 31 08:57:59 2006
;--------------------------------------------------------
	.module i2c
	.optsdcc -mmcs51 --model-small
	
;--------------------------------------------------------
; Public variables in this module
;--------------------------------------------------------
	.globl _i2c_write_PARM_3
	.globl _i2c_write_PARM_2
	.globl _i2c_read_PARM_3
	.globl _i2c_read_PARM_2
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
	.globl _i2c_read
	.globl _i2c_write
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
_i2c_read_PARM_2::
	.ds 2
_i2c_read_PARM_3::
	.ds 1
_i2c_read_junk_1_1::
	.ds 1
	.area	OSEG    (OVR,DATA)
_i2c_write_PARM_2::
	.ds 2
_i2c_write_PARM_3::
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
;Allocation info for local variables in function 'i2c_read'
;------------------------------------------------------------
;buf                       Allocated with name '_i2c_read_PARM_2'
;len                       Allocated with name '_i2c_read_PARM_3'
;addr                      Allocated to registers r2 
;junk                      Allocated with name '_i2c_read_junk_1_1'
;------------------------------------------------------------
;Initial/src/lib/i2c.c:36: i2c_read (unsigned char addr, xdata unsigned char *buf, unsigned char len)
;	-----------------------------------------
;	 function i2c_read
;	-----------------------------------------
_i2c_read:
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
;Initial/src/lib/i2c.c:40: if (len == 0)			// reading zero bytes always works
;     genCmpEq
	mov	a,_i2c_read_PARM_3
;	Peephole 162	removed sjmp by inverse jump logic
	jz	00149$
00148$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00103$
00149$:
;Initial/src/lib/i2c.c:41: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;Initial/src/lib/i2c.c:43: while (I2CS & bmSTOP)		// wait for stop to clear
00103$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r3,a
;     genIfxJump
;	Peephole 112.a	removed ljmp by inverse jump logic
	jb	acc.6,00103$
00150$:
;Initial/src/lib/i2c.c:46: I2CS = bmSTART;
;     genAssign
	mov	dptr,#_I2CS
	mov	a,#0x80
	movx	@dptr,a
;Initial/src/lib/i2c.c:47: I2DAT = (addr << 1) | 1;	// write address and direction (1's the read bit)
;     genLeftShift
;     genLeftShiftLiteral
;     genlshOne
;	Peephole 254	optimized left shift
	mov	a,r2
	add	a,r2
	mov	r2,a
;     genOr
	mov	dptr,#_I2DAT
	mov	a,#0x01
	orl	a,r2
	movx	@dptr,a
;Initial/src/lib/i2c.c:49: while ((I2CS & bmDONE) == 0)
00106$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x01
;     genCmpEq
	cjne	r2,#0x00,00151$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00106$
00151$:
;Initial/src/lib/i2c.c:52: if ((I2CS & bmBERR) || (I2CS & bmACK) == 0)	// no device answered...
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
	jnb	acc.2,00152$
	ljmp	00129$
00152$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x02
;     genCmpEq
	cjne	r2,#0x00,00153$
	ljmp	00129$
00153$:
;Initial/src/lib/i2c.c:55: if (len == 1)
;     genCmpEq
	mov	a,_i2c_read_PARM_3
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	a,#0x01,00113$
;00154$:
;	Peephole 200	removed redundant sjmp
00155$:
;Initial/src/lib/i2c.c:56: I2CS |= bmLASTRD;
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x20
	movx	@dptr,a
00113$:
;Initial/src/lib/i2c.c:58: junk = I2DAT;			// trigger the first read cycle
;     genAssign
	mov	dptr,#_I2DAT
	movx	a,@dptr
	mov	_i2c_read_junk_1_1,a
;Initial/src/lib/i2c.c:60: while (--len != 0){
;     genAssign
	mov	r2,_i2c_read_PARM_2
	mov	r3,(_i2c_read_PARM_2 + 1)
;     genAssign
	mov	r4,_i2c_read_PARM_3
00121$:
;     genMinus
;     genMinusDec
;     genCmpEq
;	Peephole 257	optimized decrement with compare
	djnz	r4,00156$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00124$
00156$:
;Initial/src/lib/i2c.c:61: while ((I2CS & bmDONE) == 0)
00114$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r5,a
;     genAnd
	anl	ar5,#0x01
;     genCmpEq
	cjne	r5,#0x00,00157$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00114$
00157$:
;Initial/src/lib/i2c.c:64: if (I2CS & bmBERR)
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r5,a
;     genIfxJump
;	Peephole 112.a	removed ljmp by inverse jump logic
	jb	acc.2,00129$
00158$:
;Initial/src/lib/i2c.c:67: if (len == 1)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r4,#0x01,00120$
;00159$:
;	Peephole 200	removed redundant sjmp
00160$:
;Initial/src/lib/i2c.c:68: I2CS |= bmLASTRD;
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r5,a
	orl	a,#0x20
	movx	@dptr,a
00120$:
;Initial/src/lib/i2c.c:70: *buf++ = I2DAT;		// get data, trigger another read
;     genAssign
	mov	dptr,#_I2DAT
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r5,a
	mov	dpl,r2
	mov	dph,r3
	movx	@dptr,a
	inc	dptr
	mov	r2,dpl
	mov	r3,dph
;     genAssign
	mov	_i2c_read_PARM_2,r2
	mov	(_i2c_read_PARM_2 + 1),r3
;Initial/src/lib/i2c.c:75: while ((I2CS & bmDONE) == 0)
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00121$
00124$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x01
;     genCmpEq
	cjne	r2,#0x00,00161$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00124$
00161$:
;Initial/src/lib/i2c.c:78: if (I2CS & bmBERR)
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
;	Peephole 112.a	removed ljmp by inverse jump logic
	jb	acc.2,00129$
00162$:
;Initial/src/lib/i2c.c:81: I2CS |= bmSTOP;
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x40
	movx	@dptr,a
;Initial/src/lib/i2c.c:82: *buf = I2DAT;
;     genAssign
	mov	r2,_i2c_read_PARM_2
	mov	r3,(_i2c_read_PARM_2 + 1)
;     genAssign
	mov	dptr,#_I2DAT
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r4,a
	mov	dpl,r2
	mov	dph,r3
	movx	@dptr,a
;Initial/src/lib/i2c.c:84: return 1;
;     genRet
	mov	dpl,#0x01
;Initial/src/lib/i2c.c:86: fail:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00129$:
;Initial/src/lib/i2c.c:87: I2CS |= bmSTOP;
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x40
	movx	@dptr,a
;Initial/src/lib/i2c.c:88: return 0;
;     genRet
	mov	dpl,#0x00
00130$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'i2c_write'
;------------------------------------------------------------
;buf                       Allocated with name '_i2c_write_PARM_2'
;len                       Allocated with name '_i2c_write_PARM_3'
;addr                      Allocated to registers r2 
;------------------------------------------------------------
;Initial/src/lib/i2c.c:95: i2c_write (unsigned char addr, xdata const unsigned char *buf, unsigned char len)
;	-----------------------------------------
;	 function i2c_write
;	-----------------------------------------
_i2c_write:
;     genReceive
	mov	r2,dpl
;Initial/src/lib/i2c.c:97: while (I2CS & bmSTOP)		// wait for stop to clear
00101$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r3,a
;     genIfxJump
;	Peephole 112.a	removed ljmp by inverse jump logic
	jb	acc.6,00101$
00133$:
;Initial/src/lib/i2c.c:100: I2CS = bmSTART;
;     genAssign
	mov	dptr,#_I2CS
	mov	a,#0x80
	movx	@dptr,a
;Initial/src/lib/i2c.c:101: I2DAT = (addr << 1) | 0;	// write address and direction (0's the write bit)
;     genLeftShift
;     genLeftShiftLiteral
;     genlshOne
;	Peephole 254	optimized left shift
	mov	a,r2
	add	a,r2
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_I2DAT
	movx	@dptr,a
;Initial/src/lib/i2c.c:103: while ((I2CS & bmDONE) == 0)
00104$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x01
;     genCmpEq
	cjne	r2,#0x00,00134$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00104$
00134$:
;Initial/src/lib/i2c.c:106: if ((I2CS & bmBERR) || (I2CS & bmACK) == 0)	// no device answered...
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
;	Peephole 112.a	removed ljmp by inverse jump logic
	jb	acc.2,00119$
00135$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x02
;     genCmpEq
	cjne	r2,#0x00,00136$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00119$
00136$:
;Initial/src/lib/i2c.c:109: while (len > 0){
;     genAssign
	mov	r2,_i2c_write_PARM_2
	mov	r3,(_i2c_write_PARM_2 + 1)
;     genAssign
	mov	r4,_i2c_write_PARM_3
00116$:
;     genCmpGt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
	mov	a,r4
	add	a,#0xff - 0x00
	jnc	00118$
00137$:
;Initial/src/lib/i2c.c:110: I2DAT = *buf++;
;     genPointerGet
;     genFarPointerGet
	mov	dpl,r2
	mov	dph,r3
	movx	a,@dptr
	mov	r5,a
	inc	dptr
	mov	r2,dpl
	mov	r3,dph
;     genAssign
	mov	dptr,#_I2DAT
	mov	a,r5
	movx	@dptr,a
;Initial/src/lib/i2c.c:111: len--;
;     genMinus
;     genMinusDec
	dec	r4
;Initial/src/lib/i2c.c:113: while ((I2CS & bmDONE) == 0)
00110$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r5,a
;     genAnd
	anl	ar5,#0x01
;     genCmpEq
	cjne	r5,#0x00,00138$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00110$
00138$:
;Initial/src/lib/i2c.c:116: if ((I2CS & bmBERR) || (I2CS & bmACK) == 0)	// no device answered...
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r5,a
;     genIfxJump
;	Peephole 112.a	removed ljmp by inverse jump logic
	jb	acc.2,00119$
00139$:
;     genAssign
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r5,a
;     genAnd
	anl	ar5,#0x02
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r5,#0x00,00116$
;00140$:
;	Peephole 200	removed redundant sjmp
00141$:
;Initial/src/lib/i2c.c:117: goto fail;
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00119$
00118$:
;Initial/src/lib/i2c.c:120: I2CS |= bmSTOP;
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x40
	movx	@dptr,a
;Initial/src/lib/i2c.c:121: return 1;
;     genRet
	mov	dpl,#0x01
;Initial/src/lib/i2c.c:123: fail:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00119$:
;Initial/src/lib/i2c.c:124: I2CS |= bmSTOP;
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_I2CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x40
	movx	@dptr,a
;Initial/src/lib/i2c.c:125: return 0;
;     genRet
	mov	dpl,#0x00
00120$:
	ret
	.area CSEG    (CODE)
