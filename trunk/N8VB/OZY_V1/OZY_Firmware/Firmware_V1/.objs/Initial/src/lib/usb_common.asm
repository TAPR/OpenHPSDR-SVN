;--------------------------------------------------------
; File Created by SDCC : FreeWare ANSI-C Compiler
; Version 2.5.0 #1020 (May  8 2005)
; This file generated Mon Aug 28 11:37:53 2006
;--------------------------------------------------------
	.module usb_common
	.optsdcc -mmcs51 --model-small
	
;--------------------------------------------------------
; Public variables in this module
;--------------------------------------------------------
	.globl _epcs
	.globl _plausible_endpoint
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
	.globl __usb_got_SUDAV
	.globl _other_config_descr
	.globl _current_config_descr
	.globl _current_devqual_descr
	.globl _current_device_descr
	.globl __usb_alt_setting
	.globl __usb_config
	.globl _usb_install_handlers
	.globl _usb_handle_set_feature
	.globl _usb_handle_clear_feature
	.globl _usb_handle_get_status
	.globl _usb_handle_get_descriptor
	.globl _usb_handle_IN_std_requests
	.globl _usb_handle_OUT_std_requests
	.globl _usb_handle_std_requests
	.globl _usb_handle_setup_packet
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
__usb_config::
	.ds 1
__usb_alt_setting::
	.ds 1
_current_device_descr::
	.ds 2
_current_devqual_descr::
	.ds 2
_current_config_descr::
	.ds 2
_other_config_descr::
	.ds 2
;--------------------------------------------------------
; overlayable items in internal ram 
;--------------------------------------------------------
	.area	OSEG    (OVR,DATA)
	.area	OSEG    (OVR,DATA)
;--------------------------------------------------------
; indirectly addressable internal ram data
;--------------------------------------------------------
	.area ISEG    (DATA)
;--------------------------------------------------------
; bit data
;--------------------------------------------------------
	.area BSEG    (BIT)
__usb_got_SUDAV::
	.ds 1
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
;Initial/src/lib/usb_common.c:56: unsigned char	_usb_config = 0;
;     genAssign
	mov	__usb_config,#0x00
;Initial/src/lib/usb_common.c:57: unsigned char	_usb_alt_setting = 0;	// FIXME really 1/interface
;     genAssign
	mov	__usb_alt_setting,#0x00
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
;Allocation info for local variables in function 'setup_descriptors'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:65: setup_descriptors (void)
;	-----------------------------------------
;	 function setup_descriptors
;	-----------------------------------------
_setup_descriptors:
	ar2 = 0x02
	ar3 = 0x03
	ar4 = 0x04
	ar5 = 0x05
	ar6 = 0x06
	ar7 = 0x07
	ar0 = 0x00
	ar1 = 0x01
;Initial/src/lib/usb_common.c:67: if (USBCS & bmHSM)
;     genAssign
	mov	dptr,#_USBCS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
;	Peephole 111	removed ljmp by inverse jump logic
	jnb	acc.7,00102$
00107$:
;Initial/src/lib/usb_common.c:69: current_device_descr  = high_speed_device_descr;
;     genAddrOf
	mov	_current_device_descr,#_high_speed_device_descr
	mov	(_current_device_descr + 1),#(_high_speed_device_descr >> 8)
;Initial/src/lib/usb_common.c:70: current_devqual_descr = high_speed_devqual_descr;
;     genAddrOf
	mov	_current_devqual_descr,#_high_speed_devqual_descr
	mov	(_current_devqual_descr + 1),#(_high_speed_devqual_descr >> 8)
;Initial/src/lib/usb_common.c:71: current_config_descr  = high_speed_config_descr;
;     genAddrOf
	mov	_current_config_descr,#_high_speed_config_descr
	mov	(_current_config_descr + 1),#(_high_speed_config_descr >> 8)
;Initial/src/lib/usb_common.c:72: other_config_descr    = full_speed_config_descr;
;     genAddrOf
	mov	_other_config_descr,#_full_speed_config_descr
	mov	(_other_config_descr + 1),#(_full_speed_config_descr >> 8)
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;Initial/src/lib/usb_common.c:76: current_device_descr  = full_speed_device_descr;
;     genAddrOf
	mov	_current_device_descr,#_full_speed_device_descr
	mov	(_current_device_descr + 1),#(_full_speed_device_descr >> 8)
;Initial/src/lib/usb_common.c:77: current_devqual_descr = full_speed_devqual_descr;
;     genAddrOf
	mov	_current_devqual_descr,#_full_speed_devqual_descr
	mov	(_current_devqual_descr + 1),#(_full_speed_devqual_descr >> 8)
;Initial/src/lib/usb_common.c:78: current_config_descr  = full_speed_config_descr;
;     genAddrOf
	mov	_current_config_descr,#_full_speed_config_descr
	mov	(_current_config_descr + 1),#(_full_speed_config_descr >> 8)
;Initial/src/lib/usb_common.c:79: other_config_descr    = high_speed_config_descr;
;     genAddrOf
	mov	_other_config_descr,#_high_speed_config_descr
	mov	(_other_config_descr + 1),#(_high_speed_config_descr >> 8)
00104$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'isr_SUDAV'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:84: isr_SUDAV (void) interrupt
;	-----------------------------------------
;	 function isr_SUDAV
;	-----------------------------------------
_isr_SUDAV:
;Initial/src/lib/usb_common.c:86: clear_usb_irq ();
;     genAnd
	anl	_EXIF,#0xEF
;     genAssign
	mov	_INT2CLR,#0x00
;Initial/src/lib/usb_common.c:87: _usb_got_SUDAV = 1;
;     genAssign
	setb	__usb_got_SUDAV
00101$:
	reti
;	eliminated unneeded push/pop psw
;	eliminated unneeded push/pop dpl
;	eliminated unneeded push/pop dph
;	eliminated unneeded push/pop b
;	eliminated unneeded push/pop acc
;------------------------------------------------------------
;Allocation info for local variables in function 'isr_USBRESET'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:91: isr_USBRESET (void) interrupt
;	-----------------------------------------
;	 function isr_USBRESET
;	-----------------------------------------
_isr_USBRESET:
	push	acc
	push	b
	push	dpl
	push	dph
	push	(0+2)
	push	(0+3)
	push	(0+4)
	push	(0+5)
	push	(0+6)
	push	(0+7)
	push	(0+0)
	push	(0+1)
	push	psw
	mov	psw,#0x00
;Initial/src/lib/usb_common.c:93: clear_usb_irq ();
;     genAnd
	anl	_EXIF,#0xEF
;     genAssign
	mov	_INT2CLR,#0x00
;Initial/src/lib/usb_common.c:94: setup_descriptors ();
;     genCall
	lcall	_setup_descriptors
00101$:
	pop	psw
	pop	(0+1)
	pop	(0+0)
	pop	(0+7)
	pop	(0+6)
	pop	(0+5)
	pop	(0+4)
	pop	(0+3)
	pop	(0+2)
	pop	dph
	pop	dpl
	pop	b
	pop	acc
	reti
;------------------------------------------------------------
;Allocation info for local variables in function 'isr_HIGHSPEED'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:98: isr_HIGHSPEED (void) interrupt
;	-----------------------------------------
;	 function isr_HIGHSPEED
;	-----------------------------------------
_isr_HIGHSPEED:
	push	acc
	push	b
	push	dpl
	push	dph
	push	(0+2)
	push	(0+3)
	push	(0+4)
	push	(0+5)
	push	(0+6)
	push	(0+7)
	push	(0+0)
	push	(0+1)
	push	psw
	mov	psw,#0x00
;Initial/src/lib/usb_common.c:100: clear_usb_irq ();
;     genAnd
	anl	_EXIF,#0xEF
;     genAssign
	mov	_INT2CLR,#0x00
;Initial/src/lib/usb_common.c:101: setup_descriptors ();
;     genCall
	lcall	_setup_descriptors
00101$:
	pop	psw
	pop	(0+1)
	pop	(0+0)
	pop	(0+7)
	pop	(0+6)
	pop	(0+5)
	pop	(0+4)
	pop	(0+3)
	pop	(0+2)
	pop	dph
	pop	dpl
	pop	b
	pop	acc
	reti
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_install_handlers'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:105: usb_install_handlers (void)
;	-----------------------------------------
;	 function usb_install_handlers
;	-----------------------------------------
_usb_install_handlers:
;Initial/src/lib/usb_common.c:107: setup_descriptors ();	    // ensure that they're set before use
;     genCall
	lcall	_setup_descriptors
;Initial/src/lib/usb_common.c:109: hook_uv (UV_SUDAV,     (unsigned short) isr_SUDAV);
;     genCast
	mov	_hook_uv_PARM_2,#_isr_SUDAV
	mov	(_hook_uv_PARM_2 + 1),#(_isr_SUDAV >> 8)
;     genCall
	mov	dpl,#0x00
	lcall	_hook_uv
;Initial/src/lib/usb_common.c:110: hook_uv (UV_USBRESET,  (unsigned short) isr_USBRESET);
;     genCast
	mov	_hook_uv_PARM_2,#_isr_USBRESET
	mov	(_hook_uv_PARM_2 + 1),#(_isr_USBRESET >> 8)
;     genCall
	mov	dpl,#0x10
	lcall	_hook_uv
;Initial/src/lib/usb_common.c:111: hook_uv (UV_HIGHSPEED, (unsigned short) isr_HIGHSPEED);
;     genCast
	mov	_hook_uv_PARM_2,#_isr_HIGHSPEED
	mov	(_hook_uv_PARM_2 + 1),#(_isr_HIGHSPEED >> 8)
;     genCall
	mov	dpl,#0x14
	lcall	_hook_uv
;Initial/src/lib/usb_common.c:113: USBIE = bmSUDAV | bmURES | bmHSGRANT;
;     genAssign
	mov	dptr,#_USBIE
	mov	a,#0x31
	movx	@dptr,a
00101$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'plausible_endpoint'
;------------------------------------------------------------
;ep                        Allocated to registers r2 
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:120: plausible_endpoint (unsigned char ep)
;	-----------------------------------------
;	 function plausible_endpoint
;	-----------------------------------------
_plausible_endpoint:
;     genReceive
;Initial/src/lib/usb_common.c:122: ep &= ~0x80;	// ignore direction bit
;     genAnd
;Initial/src/lib/usb_common.c:124: if (ep > 8)
;     genCmpGt
;     genCmp
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
;	Peephole 187	used a instead of ar2 for anl
	mov	a,dpl
	anl	a,#0x7F
	mov	r2,a
	add	a,#0xff - 0x08
	jnc	00102$
00109$:
;Initial/src/lib/usb_common.c:125: return 0;
;     genRet
	mov	dpl,#0x00
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;Initial/src/lib/usb_common.c:127: if (ep == 1)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00104$
;00110$:
;	Peephole 200	removed redundant sjmp
00111$:
;Initial/src/lib/usb_common.c:128: return 1;
;     genRet
	mov	dpl,#0x01
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00104$:
;Initial/src/lib/usb_common.c:130: return (ep & 0x1) == 0;	// must be even
;     genAnd
	anl	ar2,#0x01
;     genCmpEq
;	Peephole 241.c	optimized compare
	clr	a
	cjne	r2,#0x00,00112$
	inc	a
00112$:
00113$:
	mov	dpl,a
;     genRet
00105$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'epcs'
;------------------------------------------------------------
;ep                        Allocated to registers r2 
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:137: epcs (unsigned char ep)
;	-----------------------------------------
;	 function epcs
;	-----------------------------------------
_epcs:
;     genReceive
	mov	r2,dpl
;Initial/src/lib/usb_common.c:139: if (ep == 0x01)		// ep1 has different in and out CS regs
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00102$
;00112$:
;	Peephole 200	removed redundant sjmp
00113$:
;Initial/src/lib/usb_common.c:140: return EP1OUTCS;
;     genAssign
	mov	dptr,#_EP1OUTCS
	movx	a,@dptr
	mov	r3,a
;     genCast
	mov	r4,#0x00
;     genRet
	mov	dpl,r3
	mov	dph,r4
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;Initial/src/lib/usb_common.c:142: if (ep == 0x81)
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x81,00104$
;00114$:
;	Peephole 200	removed redundant sjmp
00115$:
;Initial/src/lib/usb_common.c:143: return EP1INCS;
;     genAssign
	mov	dptr,#_EP1INCS
	movx	a,@dptr
	mov	r3,a
;     genCast
	mov	r4,#0x00
;     genRet
	mov	dpl,r3
	mov	dph,r4
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00104$:
;Initial/src/lib/usb_common.c:145: ep &= ~0x80;			// ignore direction bit
;     genAnd
	anl	ar2,#0x7F
;Initial/src/lib/usb_common.c:147: if (ep == 0x00)		// ep0
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x00,00106$
;00116$:
;	Peephole 200	removed redundant sjmp
00117$:
;Initial/src/lib/usb_common.c:148: return EP0CS;
;     genAssign
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r3,a
;     genCast
	mov	r4,#0x00
;     genRet
	mov	dpl,r3
	mov	dph,r4
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00106$:
;Initial/src/lib/usb_common.c:150: return EP2CS + (ep >> 1);	// 2, 4, 6, 8 are consecutive
;     genAssign
	mov	dptr,#_EP2CS
	movx	a,@dptr
	mov	r3,a
;     genCast
	mov	r4,#0x00
;     genRightShift
;     genRightShiftLiteral
;     genrshOne
	mov	a,r2
	clr	c
	rrc	a
	mov	r2,a
;     genCast
	mov	r5,#0x00
;     genPlus
;	Peephole 236.g	used r2 instead of ar2
	mov	a,r2
;	Peephole 236.a	used r3 instead of ar3
	add	a,r3
	mov	r3,a
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
;	Peephole 236.b	used r4 instead of ar4
	addc	a,r4
;     genCast
;     genRet
;	Peephole 235	loading dph directly from a(ccumulator), r4 not set
	mov	dpl,r3
	mov	dph,a
00107$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_handle_set_feature'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:153: void usb_handle_set_feature(void)
;	-----------------------------------------
;	 function usb_handle_set_feature
;	-----------------------------------------
_usb_handle_set_feature:
;Initial/src/lib/usb_common.c:155: switch (bRequestType & bmRT_RECIP_MASK)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x1F
;     genCmpEq
	cjne	r2,#0x00,00121$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00101$
00121$:
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;Initial/src/lib/usb_common.c:157: case bmRT_RECIP_DEVICE:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x02,00112$
	sjmp	00106$
;00122$:
00101$:
;Initial/src/lib/usb_common.c:158: switch (wValueL)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
	cjne	r2,#0x01,00123$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00104$
00123$:
;     genCmpEq
	cjne	r2,#0x02,00124$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00106$
00124$:
;Initial/src/lib/usb_common.c:165: default:
00104$:
;Initial/src/lib/usb_common.c:166: fx2_stall_ep0 ();
;     genCall
	lcall	_fx2_stall_ep0
;Initial/src/lib/usb_common.c:170: case bmRT_RECIP_ENDPOINT:
00106$:
;Initial/src/lib/usb_common.c:171: switch (wValueL)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00126$
00125$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00112$
00126$:
;Initial/src/lib/usb_common.c:174: if (plausible_endpoint (wIndexL))
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r2
	mov	r2,a
	mov	dpl,a
	lcall	_plausible_endpoint
	mov	a,dpl
;     genIfx
;     genIfxJump
;	Peephole 110	removed ljmp by inverse jump logic
	jz	00109$
00127$:
;Initial/src/lib/usb_common.c:175: *epcs (wIndexL) |= bmEPSTALL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r2
	mov	r2,a
	mov	dpl,a
	lcall	_epcs
	mov	r2,dpl
	mov	r3,dph
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r4
	mov	r4,a
	mov	dpl,a
	push	ar2
	push	ar3
	lcall	_epcs
	pop	ar3
	pop	ar2
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
	mov	r4,a
;     genOr
	orl	ar4,#0x01
;     genPointerSet
;     genFarPointerSet
	mov	dpl,r2
	mov	dph,r3
	mov	a,r4
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00112$
00109$:
;Initial/src/lib/usb_common.c:177: fx2_stall_ep0 ();
;     genCall
	lcall	_fx2_stall_ep0
;Initial/src/lib/usb_common.c:181: default:
00112$:
;Initial/src/lib/usb_common.c:182: fx2_stall_ep0 ();
;     genCall
;Initial/src/lib/usb_common.c:184: }
;	Peephole 253.b	replaced lcall/ret with ljmp
	ljmp	_fx2_stall_ep0
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_handle_clear_feature'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:187: void usb_handle_clear_feature(void)
;	-----------------------------------------
;	 function usb_handle_clear_feature
;	-----------------------------------------
_usb_handle_clear_feature:
;Initial/src/lib/usb_common.c:189: switch (bRequestType & bmRT_RECIP_MASK)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x1F
;     genCmpEq
	cjne	r2,#0x00,00118$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00101$
00118$:
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;Initial/src/lib/usb_common.c:192: case bmRT_RECIP_DEVICE:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x02,00110$
	sjmp	00105$
;00119$:
00101$:
;Initial/src/lib/usb_common.c:193: switch (wValueL)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;Initial/src/lib/usb_common.c:197: fx2_stall_ep0 ();
;     genCall
;Initial/src/lib/usb_common.c:199: break;
;Initial/src/lib/usb_common.c:201: case bmRT_RECIP_ENDPOINT:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_fx2_stall_ep0
00105$:
;Initial/src/lib/usb_common.c:202: if (wValueL == FS_ENDPOINT_HALT && plausible_endpoint (wIndexL))
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00121$
00120$:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00107$
00121$:
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r2
	mov	r2,a
	mov	dpl,a
	lcall	_plausible_endpoint
	mov	a,dpl
;     genIfx
;     genIfxJump
;	Peephole 110	removed ljmp by inverse jump logic
	jz	00107$
00122$:
;Initial/src/lib/usb_common.c:204: *epcs (wIndexL) &= ~bmEPSTALL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r2
	mov	r2,a
	mov	dpl,a
	lcall	_epcs
	mov	r2,dpl
	mov	r3,dph
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r4
	mov	r4,a
	mov	dpl,a
	push	ar2
	push	ar3
	lcall	_epcs
	pop	ar3
	pop	ar2
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
	mov	r4,a
;     genAnd
	anl	ar4,#0xFE
;     genPointerSet
;     genFarPointerSet
	mov	dpl,r2
	mov	dph,r3
	mov	a,r4
	movx	@dptr,a
;Initial/src/lib/usb_common.c:205: fx2_reset_data_toggle (wIndexL);
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r2
	mov	r2,a
	mov	dpl,a
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_fx2_reset_data_toggle
00107$:
;Initial/src/lib/usb_common.c:208: fx2_stall_ep0 ();
;     genCall
;Initial/src/lib/usb_common.c:209: break;
;Initial/src/lib/usb_common.c:211: default:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_fx2_stall_ep0
00110$:
;Initial/src/lib/usb_common.c:212: fx2_stall_ep0 ();
;     genCall
;Initial/src/lib/usb_common.c:214: }
;	Peephole 253.c	replaced lcall with ljmp
	ljmp	_fx2_stall_ep0
00112$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_handle_get_status'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:217: void usb_handle_get_status(void)
;	-----------------------------------------
;	 function usb_handle_get_status
;	-----------------------------------------
_usb_handle_get_status:
;Initial/src/lib/usb_common.c:219: switch (bRequestType & bmRT_RECIP_MASK)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x1F
;     genCmpEq
	cjne	r2,#0x00,00115$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00101$
00115$:
;     genCmpEq
	cjne	r2,#0x01,00116$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00102$
00116$:
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;Initial/src/lib/usb_common.c:221: case bmRT_RECIP_DEVICE:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x02,00107$
	sjmp	00103$
;00117$:
00101$:
;Initial/src/lib/usb_common.c:222: EP0BUF[0] = bmGSDA_SELF_POWERED;	// FIXME app should handle
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
	mov	a,#0x01
	movx	@dptr,a
;Initial/src/lib/usb_common.c:223: EP0BUF[1] = 0;
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#(_EP0BUF + 0x0001)
;	Peephole 181	changed mov to clr
;Initial/src/lib/usb_common.c:224: EP0BCH = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_EP0BCH
	movx	@dptr,a
;Initial/src/lib/usb_common.c:225: EP0BCL = 2;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x02
	movx	@dptr,a
;Initial/src/lib/usb_common.c:226: break;
;Initial/src/lib/usb_common.c:228: case bmRT_RECIP_INTERFACE:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;Initial/src/lib/usb_common.c:229: EP0BUF[0] = 0;
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
;	Peephole 181	changed mov to clr
;Initial/src/lib/usb_common.c:230: EP0BUF[1] = 0;
;     genPointerSet
;     genFarPointerSet
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;Initial/src/lib/usb_common.c:231: EP0BCH = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219.a	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	@dptr,a
	mov	dptr,#_EP0BCH
	movx	@dptr,a
;Initial/src/lib/usb_common.c:232: EP0BCL = 2;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x02
	movx	@dptr,a
;Initial/src/lib/usb_common.c:233: break;
;Initial/src/lib/usb_common.c:235: case bmRT_RECIP_ENDPOINT:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00103$:
;Initial/src/lib/usb_common.c:236: if (plausible_endpoint (wIndexL))
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r2
	mov	r2,a
	mov	dpl,a
	lcall	_plausible_endpoint
	mov	a,dpl
;     genIfx
;     genIfxJump
;	Peephole 110	removed ljmp by inverse jump logic
	jz	00105$
00118$:
;Initial/src/lib/usb_common.c:238: EP0BUF[0] = *epcs (wIndexL) & bmEPSTALL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genCall
;	Peephole 244.c	loading dpl from a instead of r2
	mov	r2,a
	mov	dpl,a
	lcall	_epcs
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x01
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
	mov	a,r2
	movx	@dptr,a
;Initial/src/lib/usb_common.c:239: EP0BUF[1] = 0;
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#(_EP0BUF + 0x0001)
;	Peephole 181	changed mov to clr
;Initial/src/lib/usb_common.c:240: EP0BCH = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_EP0BCH
	movx	@dptr,a
;Initial/src/lib/usb_common.c:241: EP0BCL = 2;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x02
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00105$:
;Initial/src/lib/usb_common.c:244: fx2_stall_ep0 ();
;     genCall
;Initial/src/lib/usb_common.c:245: break;
;Initial/src/lib/usb_common.c:247: default:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_fx2_stall_ep0
00107$:
;Initial/src/lib/usb_common.c:248: fx2_stall_ep0 ();
;     genCall
;Initial/src/lib/usb_common.c:250: }
;	Peephole 253.c	replaced lcall with ljmp
	ljmp	_fx2_stall_ep0
00109$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_handle_get_descriptor'
;------------------------------------------------------------
;p                         Allocated to registers r2 r3 
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:253: void usb_handle_get_descriptor(void)
;	-----------------------------------------
;	 function usb_handle_get_descriptor
;	-----------------------------------------
_usb_handle_get_descriptor:
;Initial/src/lib/usb_common.c:255: switch (wValueH)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0003)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
	cjne	r2,#0x01,00127$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00101$
00127$:
;     genCmpEq
	cjne	r2,#0x02,00128$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00105$
00128$:
;     genCmpEq
	cjne	r2,#0x03,00129$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00113$
00129$:
;     genCmpEq
	cjne	r2,#0x06,00130$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00102$
00130$:
;     genCmpEq
	cjne	r2,#0x07,00131$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00110$
00131$:
	ljmp	00117$
;Initial/src/lib/usb_common.c:258: case DT_DEVICE:
00101$:
;Initial/src/lib/usb_common.c:259: SUDPTRH = MSB (current_device_descr);
;     genCast
	mov	r2,_current_device_descr
	mov	r3,(_current_device_descr + 1)
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;Initial/src/lib/usb_common.c:260: SUDPTRL = LSB (current_device_descr);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;Initial/src/lib/usb_common.c:261: break;
;	Peephole 251.a	replaced ljmp to ret with ret
	ret
;Initial/src/lib/usb_common.c:263: case DT_DEVQUAL:
00102$:
;Initial/src/lib/usb_common.c:264: SUDPTRH = MSB (current_devqual_descr);
;     genCast
	mov	r2,_current_devqual_descr
	mov	r3,(_current_devqual_descr + 1)
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;Initial/src/lib/usb_common.c:265: SUDPTRL = LSB (current_devqual_descr);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;Initial/src/lib/usb_common.c:266: break;
;Initial/src/lib/usb_common.c:270: fx2_stall_ep0 ();
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00105$:
;Initial/src/lib/usb_common.c:273: SUDPTRH = MSB (current_config_descr);
;     genCast
	mov	r2,_current_config_descr
	mov	r3,(_current_config_descr + 1)
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;Initial/src/lib/usb_common.c:274: SUDPTRL = LSB (current_config_descr);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;Initial/src/lib/usb_common.c:276: break;
;Initial/src/lib/usb_common.c:280: fx2_stall_ep0 ();
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00110$:
;Initial/src/lib/usb_common.c:283: SUDPTRH = MSB (other_config_descr);
;     genCast
	mov	r2,_other_config_descr
	mov	r3,(_other_config_descr + 1)
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;Initial/src/lib/usb_common.c:284: SUDPTRL = LSB (other_config_descr);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;Initial/src/lib/usb_common.c:286: break;
;Initial/src/lib/usb_common.c:288: case DT_STRING:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00113$:
;Initial/src/lib/usb_common.c:289: if (wValueL >= nstring_descriptors)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genAssign
	mov	dptr,#_nstring_descriptors
	movx	a,@dptr
	mov	r3,a
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,r3
;     genIfxJump
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 160	removed sjmp by inverse jump logic
	jc	00115$
00132$:
;Initial/src/lib/usb_common.c:290: fx2_stall_ep0 ();
;     genCall
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_fx2_stall_ep0
00115$:
;Initial/src/lib/usb_common.c:293: xdata char *p = string_descriptors[wValueL];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genMult
;     genMultOneByte
;	Peephole 105	removed redundant mov
	mov	r2,a
	mov	b,#0x02
	mul	ab
;     genPlus
	add	a,#_string_descriptors
	mov	dpl,a
	mov	a,#(_string_descriptors >> 8)
	addc	a,b
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;Initial/src/lib/usb_common.c:294: SUDPTRH = MSB (p);
;     genCast
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;Initial/src/lib/usb_common.c:295: SUDPTRL = LSB (p);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;Initial/src/lib/usb_common.c:297: break;
;Initial/src/lib/usb_common.c:299: default:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00117$:
;Initial/src/lib/usb_common.c:300: fx2_stall_ep0 ();	// invalid request
;     genCall
;Initial/src/lib/usb_common.c:302: }
;	Peephole 253.b	replaced lcall/ret with ljmp
	ljmp	_fx2_stall_ep0
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_handle_IN_std_requests'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:305: void usb_handle_IN_std_requests(void)
;	-----------------------------------------
;	 function usb_handle_IN_std_requests
;	-----------------------------------------
_usb_handle_IN_std_requests:
;Initial/src/lib/usb_common.c:307: switch (bRequest)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0001)
	movx	a,@dptr
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00104$
00114$:
;     genCmpEq
	cjne	r2,#0x06,00115$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00103$
00115$:
;     genCmpEq
	cjne	r2,#0x08,00116$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00101$
00116$:
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;Initial/src/lib/usb_common.c:310: case RQ_GET_CONFIG:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x0A,00106$
	sjmp	00102$
;00117$:
00101$:
;Initial/src/lib/usb_common.c:311: EP0BUF[0] = _usb_config;	// FIXME app should handle
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
	mov	a,__usb_config
	movx	@dptr,a
;Initial/src/lib/usb_common.c:312: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;Initial/src/lib/usb_common.c:313: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;Initial/src/lib/usb_common.c:314: break;
;Initial/src/lib/usb_common.c:316: case RQ_GET_INTERFACE:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;Initial/src/lib/usb_common.c:317: EP0BUF[0] = _usb_alt_setting;	// FIXME app should handle
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
	mov	a,__usb_alt_setting
	movx	@dptr,a
;Initial/src/lib/usb_common.c:318: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;Initial/src/lib/usb_common.c:319: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;Initial/src/lib/usb_common.c:320: break;
;Initial/src/lib/usb_common.c:322: case RQ_GET_DESCR:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00103$:
;Initial/src/lib/usb_common.c:323: usb_handle_get_descriptor();
;     genCall
;Initial/src/lib/usb_common.c:324: break;
;Initial/src/lib/usb_common.c:326: case RQ_GET_STATUS:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_usb_handle_get_descriptor
00104$:
;Initial/src/lib/usb_common.c:327: usb_handle_get_status();
;     genCall
;Initial/src/lib/usb_common.c:328: break;
;Initial/src/lib/usb_common.c:331: default:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_usb_handle_get_status
00106$:
;Initial/src/lib/usb_common.c:332: fx2_stall_ep0 ();
;     genCall
;Initial/src/lib/usb_common.c:334: }
;	Peephole 253.c	replaced lcall with ljmp
	ljmp	_fx2_stall_ep0
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_handle_OUT_std_requests'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:337: void usb_handle_OUT_std_requests(void)
;	-----------------------------------------
;	 function usb_handle_OUT_std_requests
;	-----------------------------------------
_usb_handle_OUT_std_requests:
;Initial/src/lib/usb_common.c:339: switch (bRequest)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0001)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
	cjne	r2,#0x01,00117$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00103$
00117$:
;     genCmpEq
	cjne	r2,#0x03,00118$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00104$
00118$:
;     genCmpEq
	cjne	r2,#0x05,00119$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00107$
00119$:
;     genCmpEq
	cjne	r2,#0x07,00120$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00107$
00120$:
;     genCmpEq
	cjne	r2,#0x09,00121$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00101$
00121$:
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;Initial/src/lib/usb_common.c:342: case RQ_SET_CONFIG:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x0B,00107$
	sjmp	00102$
;00122$:
00101$:
;Initial/src/lib/usb_common.c:343: _usb_config = wValueL;		// FIXME app should handle
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	__usb_config,a
;Initial/src/lib/usb_common.c:344: break;
;Initial/src/lib/usb_common.c:346: case RQ_SET_INTERFACE:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00102$:
;Initial/src/lib/usb_common.c:347: _usb_alt_setting = wValueL;	// FIXME app should handle
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	__usb_alt_setting,a
;Initial/src/lib/usb_common.c:348: break;
;Initial/src/lib/usb_common.c:350: case RQ_CLEAR_FEATURE:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
	ret
00103$:
;Initial/src/lib/usb_common.c:351: usb_handle_clear_feature();
;     genCall
;Initial/src/lib/usb_common.c:352: break;
;Initial/src/lib/usb_common.c:354: case RQ_SET_FEATURE:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_usb_handle_clear_feature
00104$:
;Initial/src/lib/usb_common.c:355: usb_handle_set_feature();
;     genCall
;Initial/src/lib/usb_common.c:356: break;
;Initial/src/lib/usb_common.c:360: default:
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_usb_handle_set_feature
00107$:
;Initial/src/lib/usb_common.c:361: fx2_stall_ep0 ();
;     genCall
;Initial/src/lib/usb_common.c:362: }
;	Peephole 253.c	replaced lcall with ljmp
	ljmp	_fx2_stall_ep0
00109$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_handle_std_requests'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:365: void usb_handle_std_requests(void)
;	-----------------------------------------
;	 function usb_handle_std_requests
;	-----------------------------------------
_usb_handle_std_requests:
;Initial/src/lib/usb_common.c:367: if ((bRequestType & bmRT_DIR_MASK) == bmRT_DIR_IN)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x80
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x80,00102$
;00107$:
;	Peephole 200	removed redundant sjmp
00108$:
;Initial/src/lib/usb_common.c:369: usb_handle_IN_std_requests();
;     genCall
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 251.b	replaced sjmp to ret with ret
;	Peephole 253.a	replaced lcall/ret with ljmp
	ljmp	_usb_handle_IN_std_requests
00102$:
;Initial/src/lib/usb_common.c:372: usb_handle_OUT_std_requests();
;     genCall
;	Peephole 253.c	replaced lcall with ljmp
	ljmp	_usb_handle_OUT_std_requests
00104$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'usb_handle_setup_packet'
;------------------------------------------------------------
;------------------------------------------------------------
;Initial/src/lib/usb_common.c:376: usb_handle_setup_packet (void)
;	-----------------------------------------
;	 function usb_handle_setup_packet
;	-----------------------------------------
_usb_handle_setup_packet:
;Initial/src/lib/usb_common.c:378: _usb_got_SUDAV = 0;
;     genAssign
	clr	__usb_got_SUDAV
;Initial/src/lib/usb_common.c:382: switch (bRequestType & bmRT_TYPE_MASK)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x60
;     genCmpEq
	cjne	r2,#0x00,00115$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00106$
00115$:
;     genCmpEq
	cjne	r2,#0x20,00116$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00102$
00116$:
;     genCmpEq
	cjne	r2,#0x40,00117$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00103$
00117$:
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x60,00107$
;00118$:
;	Peephole 200	removed redundant sjmp
00119$:
;Initial/src/lib/usb_common.c:386: case bmRT_TYPE_RESERVED:
00102$:
;Initial/src/lib/usb_common.c:387: fx2_stall_ep0 ();		// we don't handle these.  indicate error
;     genCall
	lcall	_fx2_stall_ep0
;Initial/src/lib/usb_common.c:388: break;
;Initial/src/lib/usb_common.c:390: case bmRT_TYPE_VENDOR:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00108$
00103$:
;Initial/src/lib/usb_common.c:392: if (!app_vendor_cmd ())
;     genCall
	lcall	_app_vendor_cmd
	mov	a,dpl
;     genIfx
;     genIfxJump
;	Peephole 109	removed ljmp by inverse jump logic
	jnz	00108$
00120$:
;Initial/src/lib/usb_common.c:393: fx2_stall_ep0 ();
;     genCall
	lcall	_fx2_stall_ep0
;Initial/src/lib/usb_common.c:394: break;
;Initial/src/lib/usb_common.c:396: case bmRT_TYPE_STD:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00108$
00106$:
;Initial/src/lib/usb_common.c:398: usb_handle_std_requests();
;     genCall
	lcall	_usb_handle_std_requests
;Initial/src/lib/usb_common.c:399: break;
;Initial/src/lib/usb_common.c:401: default:
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00108$
00107$:
;Initial/src/lib/usb_common.c:402: fx2_stall_ep0 ();		// indicate error
;     genCall
	lcall	_fx2_stall_ep0
;Initial/src/lib/usb_common.c:405: }
00108$:
;Initial/src/lib/usb_common.c:408: EP0CS |= bmHSNAK;
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x80
	movx	@dptr,a
00109$:
	ret
	.area CSEG    (CODE)
