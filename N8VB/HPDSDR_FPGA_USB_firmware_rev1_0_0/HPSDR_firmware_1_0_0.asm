;--------------------------------------------------------
; File Created by SDCC : FreeWare ANSI-C Compiler
; Version 2.5.0 #1020 (Oct  3 2005)
; This file generated Fri Oct  7 14:51:19 2005
;--------------------------------------------------------
	.module HPSDR_firmware_1_0_0
	.optsdcc -mmcs51 --model-small
	
;--------------------------------------------------------
; Public variables in this module
;--------------------------------------------------------
	.globl _EPCS_Offset_Lookup_Table
	.globl _ISR_Ep1out
	.globl _ISR_Highspeed
	.globl _ISR_Usb_Reset
	.globl _ISR_Sutok
	.globl _ISR_Sudav
	.globl _main
	.globl _init_fx2
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
	.globl _SBU
	.globl _SCO
	.globl _MPA
	.globl _EXIF
	.globl _IOB
	.globl _SPC_
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
	.globl _pStringDscr
	.globl _pOtherConfigDscr
	.globl _pConfigDscr
	.globl _pFullSpeedConfigDscr
	.globl _pHighSpeedConfigDscr
	.globl _pDeviceQualDscr
	.globl _pDeviceDscr
	.globl _test_extern_mem2
	.globl _test_extern_mem1
	.globl _AlternateSetting
	.globl _Configuration
	.globl _wascalled
	.globl _got_vend_cmd
	.globl _EP1HASDATA
	.globl _SELFPWR
	.globl _RWUEN
	.globl _RxSUD
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
	.globl _pExtByteMemory
	.globl _pExtMemory
	.globl _FX2_Delay1ms
	.globl _FX2_Delay
	.globl _FX2_Discon
	.globl _setup_io
	.globl _setup_interrupts
	.globl _setup_descriptor_pointers
	.globl _clear_ext_ram
	.globl _process_ep1_data
	.globl _process_ep0_data
	.globl _main_loop
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
_SPC_	=	0x008f
_IOB	=	0x0090
_EXIF	=	0x0091
_MPA	=	0x0092
_SCO	=	0x0098
_SBU	=	0x0099
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
;--------------------------------------------------------
; overlayable register banks 
;--------------------------------------------------------
	.area REG_BANK_0	(REL,OVR,DATA)
	.ds 8
;--------------------------------------------------------
; internal ram data
;--------------------------------------------------------
	.area DSEG    (DATA)
_pExtMemory::
	.ds 2
_pExtByteMemory::
	.ds 2
;--------------------------------------------------------
; overlayable items in internal ram 
;--------------------------------------------------------
	.area	OSEG    (OVR,DATA)
	.area	OSEG    (OVR,DATA)
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
_RxSUD::
	.ds 1
_RWUEN::
	.ds 1
_SELFPWR::
	.ds 1
_EP1HASDATA::
	.ds 1
_got_vend_cmd::
	.ds 1
_wascalled::
	.ds 1
_Configuration::
	.ds 1
_AlternateSetting::
	.ds 1
_test_extern_mem1	=	0x4002
_test_extern_mem2	=	0x4004
_pDeviceDscr::
	.ds 2
_pDeviceQualDscr::
	.ds 2
_pHighSpeedConfigDscr::
	.ds 2
_pFullSpeedConfigDscr::
	.ds 2
_pConfigDscr::
	.ds 2
_pOtherConfigDscr::
	.ds 2
_pStringDscr::
	.ds 2
;--------------------------------------------------------
; external initialized ram data
;--------------------------------------------------------
	.area XISEG   (XDATA)
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
	.globl __mcs51_genXINIT
	.globl __mcs51_genXRAMCLEAR
	.globl __mcs51_genRAMCLEAR
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
;Allocation info for local variables in function 'FX2_Delay1ms'
;------------------------------------------------------------
;------------------------------------------------------------
;fx2_utils.h:37: void FX2_Delay1ms (void)
;	-----------------------------------------
;	 function FX2_Delay1ms
;	-----------------------------------------
_FX2_Delay1ms:
	ar2 = 0x02
	ar3 = 0x03
	ar4 = 0x04
	ar5 = 0x05
	ar6 = 0x06
	ar7 = 0x07
	ar0 = 0x00
	ar1 = 0x01
;fx2_utils.h:55: _endasm;
;     genInline
	        mov A, #0
	        mov _DPS,A
	        mov DPTR,#(0xffff-602)
	        mov R4,#5
        loop:
	        inc DPTR
	        mov A,DPL
	        orl A,DPH
	        jnz loop
        er_end:
	        ret
00101$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'FX2_Delay'
;------------------------------------------------------------
;ms                        Allocated to registers r2 r3 
;------------------------------------------------------------
;fx2_utils.h:60: void FX2_Delay(WORD ms)
;	-----------------------------------------
;	 function FX2_Delay
;	-----------------------------------------
_FX2_Delay:
;     genReceive
	mov	r2,dpl
	mov	r3,dph
;fx2_utils.h:66: if ((CPUCS & bmCLKSPD) == 0)			  // 12Mhz
;     genAssign
	mov	dptr,#_CPUCS
	movx	a,@dptr
	mov	r4,a
;     genAnd
	anl	ar4,#0x18
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r4,#0x00,00104$
;00115$:
;	Peephole 200	removed redundant sjmp
00116$:
;fx2_utils.h:67: ms = (ms + 1) / 2;					   // Round up before dividing so we can accept 1.
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
	mov	r4,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r3 instead of ar3
	addc	a,r3
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
;	Peephole 177.d	removed redundant move
	mov	r5,a
	mov	ar2,r4
	clr	c
	rrc	a
	xch	a,r2
	rrc	a
	xch	a,r2
	mov	r3,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00114$
00104$:
;fx2_utils.h:68: else if ((CPUCS & bmCLKSPD) == bmCLKSPD1)	// 48Mhz
;     genAssign
	mov	dptr,#_CPUCS
	movx	a,@dptr
	mov	r4,a
;     genAnd
	anl	ar4,#0x18
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r4,#0x10,00114$
;00117$:
;	Peephole 200	removed redundant sjmp
00118$:
;fx2_utils.h:69: ms = ms * 2;
;     genLeftShift
;     genLeftShiftLiteral
;     genlshTwo
	mov	a,r3
	xch	a,r2
	add	a,acc
	xch	a,r2
	rlc	a
	mov	r3,a
;fx2_utils.h:71: while (ms--)
00114$:
;     genAssign
00106$:
;     genAssign
	mov	ar4,r2
	mov	ar5,r3
;     genMinus
;     genMinusDec
	dec	r2
	cjne	r2,#0xff,00119$
	dec	r3
00119$:
;     genIfx
	mov	a,r4
	orl	a,r5
;     genIfxJump
;	Peephole 110	removed ljmp by inverse jump logic
	jz	00108$
00120$:
;fx2_utils.h:72: FX2_Delay1ms();
;     genCall
	push	ar2
	push	ar3
	lcall	_FX2_Delay1ms
	pop	ar3
	pop	ar2
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00106$
00108$:
;fx2_utils.h:74: return;
;     genRet
00109$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'FX2_Discon'
;------------------------------------------------------------
;renum                     Allocated to registers r2 
;------------------------------------------------------------
;fx2_utils.h:80: void FX2_Discon(BOOL renum)
;	-----------------------------------------
;	 function FX2_Discon
;	-----------------------------------------
_FX2_Discon:
;     genReceive
;fx2_utils.h:82: if(renum)                                // If renumerate (i.e. 8051 will handle SETUP commands)
;     genIfx
;	peephole 177.g	optimized mov sequence
	mov	a,dpl
	mov	r2,a
;     genIfxJump
;	Peephole 110	removed ljmp by inverse jump logic
	jz	00102$
00107$:
;fx2_utils.h:83: FX2_USB_DISCONNECT_AND_RENUM();       // disconnect from USB and set the renumerate bit
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_USBCS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x0A
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00103$
00102$:
;fx2_utils.h:85: FX2_USB_DISCONNECT();                 // just disconnect from USB
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_USBCS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x08
	movx	@dptr,a
00103$:
;fx2_utils.h:87: FX2_Delay(1500);      					// Wait 1500 ms
;     genCall
;	Peephole 182.b	used 16 bit load of dptr
	mov	dptr,#0x05DC
	lcall	_FX2_Delay
;fx2_utils.h:89: USBIRQ = 0xff;          					// Clear any pending USB interrupt requests.
;     genAssign
	mov	dptr,#_USBIRQ
	mov	a,#0xFF
	movx	@dptr,a
;fx2_utils.h:90: EPIRQ = 0xff;
;     genAssign
	mov	dptr,#_EPIRQ
	mov	a,#0xFF
	movx	@dptr,a
;fx2_utils.h:92: FX2_IRQ_CLEAR();
;     genAnd
	anl	_EXIF,#0xEF
;fx2_utils.h:93: FX2_USB_CONNECT();      					// reconnect USB
;     genAssign
;     genAnd
;	Peephole 248.b	optimized and to xdata
	mov	dptr,#_USBCS
	movx	a,@dptr
	mov	r2,a
	anl	a,#0xF7
	movx	@dptr,a
00104$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'init_fx2'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:29: void init_fx2 (void)
;	-----------------------------------------
;	 function init_fx2
;	-----------------------------------------
_init_fx2:
;HPSDR_firmware_1_0_0.c:31: FX2_SET_CLKSPD_48();			// CPU runs at 48 MHz
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_CPUCS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x10
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:32: FX2_SET_REVCTL();				// setup as per TRM recommendation
;     genInline
	 nop; 
;     genInline
	 nop; 
;     genInline
	 nop; 
;     genAssign
	mov	dptr,#_REVCTL
	mov	a,#0x03
	movx	@dptr,a
;     genInline
	 nop; 
;     genInline
	 nop; 
;     genInline
	 nop; 
;HPSDR_firmware_1_0_0.c:33: FX2_SPDAUTO_ENABLE();			// setup autopointer enabled
;     genAssign
	mov	dptr,#_SUDPTRCTL
	mov	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:34: FX2_RESET_EP1OUT_STALL(); 		// reset stall bit on EP1 OUT 
;     genAssign
;     genAnd
;	Peephole 248.b	optimized and to xdata
	mov	dptr,#_EP1OUTCS
	movx	a,@dptr
	mov	r2,a
	anl	a,#0xFE
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:35: FX2_ARM_EP1OUT();				// re-arm for EP1 out transfer
;     genAssign
	mov	dptr,#_EP1OUTBC
	mov	a,#0x40
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:36: FX2_RESET_EP1IN_STALL();		// reset if EP1 is stalled
;     genAssign
;     genAnd
;	Peephole 248.b	optimized and to xdata
	mov	dptr,#_EP1INCS
	movx	a,@dptr
	mov	r2,a
	anl	a,#0xFE
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:37: FX2_CLR_IFCONFIG();				// clear ifclock config
;     genAssign
;HPSDR_firmware_1_0_0.c:38: FX2_SET_IFCLKSRC_INT();			// set to internal ifclock source
;     genAssign
;	Peephole 180.a	removed redundant mov to dptr
	mov	dptr,#_IFCONFIG
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
	movx	a,@dptr
	mov	r2,a
;     genOr
	mov	dptr,#_IFCONFIG
	mov	a,#0x80
	orl	a,r2
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:39: FX2_SET_FIFOGPIF_CLK48();		// set ifclock speed to 48 Mhz
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_IFCONFIG
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x40
	movx	@dptr,a
00101$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'setup_io'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:42: void setup_io (void)
;	-----------------------------------------
;	 function setup_io
;	-----------------------------------------
_setup_io:
;HPSDR_firmware_1_0_0.c:44: FX2_CLR_PORTACFG();				// port a all gp io
;     genAssign
	mov	dptr,#_PORTACFG
;	Peephole 181	changed mov to clr
;HPSDR_firmware_1_0_0.c:45: FX2_CLR_PORTECFG();				// port e all gp io
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_PORTECFG
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:47: OEA=0x03;						// pins 1 and 2 output
;     genAssign
	mov	_OEA,#0x03
;HPSDR_firmware_1_0_0.c:48: IOA=0x03;
;     genAssign
	mov	_IOA,#0x03
;HPSDR_firmware_1_0_0.c:50: OEE=0x02;						// pin 2 output
;     genAssign
	mov	_OEE,#0x02
;HPSDR_firmware_1_0_0.c:51: IOE=0x02;
;     genAssign
	mov	_IOE,#0x02
00101$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'setup_interrupts'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:54: void setup_interrupts (void)
;	-----------------------------------------
;	 function setup_interrupts
;	-----------------------------------------
_setup_interrupts:
;HPSDR_firmware_1_0_0.c:56: FX2_USB_INT_ENABLE();  								// enable USB interrupts
;     genOr
	orl	_EIE,#0x01
;HPSDR_firmware_1_0_0.c:57: FX2_INT2_AV_ENABLE();  								// USB autovector enable
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_INTSETUP
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x08
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:58: FX2_USB_INT_MASK(bmSUDAV | bmSUTOK | bmHSGRANT);	// enable SUDAV, SUTOK, and HSGRANT interrupts
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_USBIE
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x25
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:59: FX2_USB_EPIE_MASK(bmEP1OUT);						// enalbe EP1OUT interrupt
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EPIE
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x08
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:60: FX2_8051_INT_ENABLE();								// enable 8051 interrupts
;     genOr
	orl	_IE,#0x80
00101$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'setup_descriptor_pointers'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:63: void setup_descriptor_pointers (void)
;	-----------------------------------------
;	 function setup_descriptor_pointers
;	-----------------------------------------
_setup_descriptor_pointers:
;HPSDR_firmware_1_0_0.c:65: pDeviceDscr = (WORD)&DeviceDscr;
;     genCast
	mov	dptr,#_pDeviceDscr
	mov	a,#_DeviceDscr
	movx	@dptr,a
	inc	dptr
	mov	a,#(_DeviceDscr >> 8)
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:66: pDeviceQualDscr = (WORD)&DeviceQualDscr;
;     genCast
	mov	dptr,#_pDeviceQualDscr
	mov	a,#_DeviceQualDscr
	movx	@dptr,a
	inc	dptr
	mov	a,#(_DeviceQualDscr >> 8)
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:67: pHighSpeedConfigDscr = (WORD)&HighSpeedConfigDscr;
;     genCast
	mov	r2,#_HighSpeedConfigDscr
	mov	r3,#(_HighSpeedConfigDscr >> 8)
;     genAssign
	mov	dptr,#_pHighSpeedConfigDscr
	mov	a,r2
	movx	@dptr,a
	inc	dptr
	mov	a,r3
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:68: pFullSpeedConfigDscr = (WORD)&FullSpeedConfigDscr;
;     genCast
	mov	r4,#_FullSpeedConfigDscr
	mov	r5,#(_FullSpeedConfigDscr >> 8)
;     genAssign
	mov	dptr,#_pFullSpeedConfigDscr
	mov	a,r4
	movx	@dptr,a
	inc	dptr
	mov	a,r5
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:69: pStringDscr = (WORD)&StringDscr0;
;     genCast
	mov	dptr,#_pStringDscr
	mov	a,#_StringDscr0
	movx	@dptr,a
	inc	dptr
	mov	a,#(_StringDscr0 >> 8)
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:70: pConfigDscr = pHighSpeedConfigDscr;
;     genAssign
	mov	dptr,#_pConfigDscr
	mov	a,r2
	movx	@dptr,a
	inc	dptr
	mov	a,r3
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:71: pOtherConfigDscr = pFullSpeedConfigDscr;
;     genAssign
	mov	dptr,#_pOtherConfigDscr
	mov	a,r4
	movx	@dptr,a
	inc	dptr
	mov	a,r5
	movx	@dptr,a
00101$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'clear_ext_ram'
;------------------------------------------------------------
;i                         Allocated to registers r2 r3 
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:74: void clear_ext_ram(void)
;	-----------------------------------------
;	 function clear_ext_ram
;	-----------------------------------------
_clear_ext_ram:
;HPSDR_firmware_1_0_0.c:78: pExtMemory = (WORD xdata *)0x4000;
;     genAssign
	mov	_pExtMemory,#0x00
	mov	(_pExtMemory + 1),#0x40
;HPSDR_firmware_1_0_0.c:80: for (i=0; i < 0x1000; i++)
;     genAssign
	mov	r2,#0x00
	mov	r3,#0x10
00103$:
;HPSDR_firmware_1_0_0.c:82: *(pExtMemory++) = 0xFFFF; 
;     genAssign
	mov	dpl,_pExtMemory
	mov	dph,(_pExtMemory + 1)
;     genPointerSet
;     genFarPointerSet
;	Peephole 101	removed redundant mov
	mov	a,#0xFF
	movx	@dptr,a
	inc	dptr
	movx	@dptr,a
;     genPlus
;     genPlusIncr
	mov	a,#0x02
	add	a,_pExtMemory
	mov	_pExtMemory,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,(_pExtMemory + 1)
	mov	(_pExtMemory + 1),a
;     genDjnz
;     genMinus
;     genMinusDec
	dec	r2
	cjne	r2,#0xff,00108$
	dec	r3
00108$:
;HPSDR_firmware_1_0_0.c:80: for (i=0; i < 0x1000; i++)
;     genIfx
	mov	a,r2
	orl	a,r3
;     genIfxJump
;	Peephole 109	removed ljmp by inverse jump logic
	jnz	00103$
00109$:
00104$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'process_ep1_data'
;------------------------------------------------------------
;i                         Allocated with name '_process_ep1_data_i_1_1'
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:87: void process_ep1_data(void)
;	-----------------------------------------
;	 function process_ep1_data
;	-----------------------------------------
_process_ep1_data:
;HPSDR_firmware_1_0_0.c:91: if (!(EP1INCS & bmEPBUSY)) 		// can we send on EP1 ?
;     genAssign
	mov	dptr,#_EP1INCS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
	jnb	acc.1,00133$
	ljmp	00113$
00133$:
;HPSDR_firmware_1_0_0.c:93: if (EP1OUTBUF[0] == 1)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP1OUTBUF
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00110$
;00134$:
;	Peephole 200	removed redundant sjmp
00135$:
;HPSDR_firmware_1_0_0.c:95: if (EP1OUTBUF[1] == 1)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP1OUTBUF + 0x0001)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00102$
;00136$:
;	Peephole 200	removed redundant sjmp
00137$:
;HPSDR_firmware_1_0_0.c:97: IOA = 0x03;
;     genAssign
	mov	_IOA,#0x03
	ljmp	00113$
00102$:
;HPSDR_firmware_1_0_0.c:101: IOA = 0x02;
;     genAssign
	mov	_IOA,#0x02
	ljmp	00113$
00110$:
;HPSDR_firmware_1_0_0.c:104: else if (EP1OUTBUF[0] == 2)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP1OUTBUF
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x02,00107$
;00138$:
;	Peephole 200	removed redundant sjmp
00139$:
;HPSDR_firmware_1_0_0.c:106: pExtMemory = (WORD xdata *)(EP1OUTBUF[1] + (EP1OUTBUF[2]<<8));
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP1OUTBUF + 0x0001)
	movx	a,@dptr
	mov	r2,a
;     genCast
	mov	r3,#0x00
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP1OUTBUF + 0x0002)
	movx	a,@dptr
	mov	r4,a
;     genCast
;     genLeftShift
;     genLeftShiftLiteral
;     genlshTwo
;	peephole 177.e	removed redundant move
	mov	ar5,r4
;     genPlus
;	Peephole 236.g	used r4 instead of ar4
;	peephole 177.g	optimized mov sequence
;	Peephole 181	changed mov to clr
	clr	a
	mov	r4,a
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
	mov	r2,a
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
;	Peephole 236.b	used r3 instead of ar3
	addc	a,r3
	mov	r3,a
;     genCast
	mov	_pExtMemory,r2
	mov	(_pExtMemory + 1),r3
;HPSDR_firmware_1_0_0.c:107: pExtByteMemory = (BYTE xdata *)pExtMemory;
;     genAssign
	mov	_pExtByteMemory,_pExtMemory
	mov	(_pExtByteMemory + 1),(_pExtMemory + 1)
;HPSDR_firmware_1_0_0.c:109: for (i=0; i < EP1OUTBUF[3]; i++)
;     genAssign
	mov	r2,#0x00
00114$:
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP1OUTBUF + 0x0003)
	movx	a,@dptr
	mov	r3,a
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,r3
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00117$
00140$:
;HPSDR_firmware_1_0_0.c:111: EP1INBUF[i] = *(pExtByteMemory++); 
;     genPlus
;	Peephole 236.g	used r2 instead of ar2
	mov	a,r2
	add	a,#_EP1INBUF
	mov	r3,a
;	Peephole 181	changed mov to clr
	clr	a
	addc	a,#(_EP1INBUF >> 8)
	mov	r4,a
;     genAssign
	mov	dpl,_pExtByteMemory
	mov	dph,(_pExtByteMemory + 1)
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
	mov	r5,a
;     genPlus
;     genPlusIncr
	inc	_pExtByteMemory
	clr	a
	cjne	a,_pExtByteMemory,00141$
	inc	(_pExtByteMemory + 1)
00141$:
;     genPointerSet
;     genFarPointerSet
	mov	dpl,r3
	mov	dph,r4
	mov	a,r5
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:109: for (i=0; i < EP1OUTBUF[3]; i++)
;     genPlus
;     genPlusIncr
	inc	r2
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00114$
00117$:
;HPSDR_firmware_1_0_0.c:113: FX2_RESET_EP1IN_STALL();
;     genAssign
;     genAnd
;	Peephole 248.b	optimized and to xdata
	mov	dptr,#_EP1INCS
	movx	a,@dptr
	mov	r3,a
	anl	a,#0xFE
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:114: EP1INBC = i + 1; 			// bytes to transfer, initates send on EP1 IN
;     genPlus
	mov	dptr,#_EP1INBC
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00113$
00107$:
;HPSDR_firmware_1_0_0.c:116: else if (EP1OUTBUF[0] == 3)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_EP1OUTBUF
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x03,00113$
;00142$:
;	Peephole 200	removed redundant sjmp
00143$:
;HPSDR_firmware_1_0_0.c:118: pExtMemory = (WORD xdata *)(EP1OUTBUF[1] + (EP1OUTBUF[2]<<8));
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP1OUTBUF + 0x0001)
	movx	a,@dptr
	mov	r2,a
;     genCast
	mov	r3,#0x00
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP1OUTBUF + 0x0002)
	movx	a,@dptr
	mov	r4,a
;     genCast
;     genLeftShift
;     genLeftShiftLiteral
;     genlshTwo
;	peephole 177.e	removed redundant move
	mov	ar5,r4
;     genPlus
;	Peephole 236.g	used r4 instead of ar4
;	peephole 177.g	optimized mov sequence
;	Peephole 181	changed mov to clr
	clr	a
	mov	r4,a
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
	mov	r2,a
;	Peephole 236.g	used r5 instead of ar5
	mov	a,r5
;	Peephole 236.b	used r3 instead of ar3
	addc	a,r3
	mov	r3,a
;     genCast
	mov	_pExtMemory,r2
	mov	(_pExtMemory + 1),r3
;HPSDR_firmware_1_0_0.c:119: pExtByteMemory = (BYTE xdata *)pExtMemory;
;     genAssign
	mov	_pExtByteMemory,_pExtMemory
	mov	(_pExtByteMemory + 1),(_pExtMemory + 1)
;HPSDR_firmware_1_0_0.c:121: for (i=0; i < EP1OUTBUF[3]; i++)
;     genAssign
	mov	r2,#0x00
00118$:
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_EP1OUTBUF + 0x0003)
	movx	a,@dptr
	mov	r3,a
;     genCmpLt
;     genCmp
	clr	c
	mov	a,r2
	subb	a,r3
;     genIfxJump
;	Peephole 108	removed ljmp by inverse jump logic
	jnc	00113$
00144$:
;HPSDR_firmware_1_0_0.c:123: *(pExtByteMemory++) = (BYTE)EP1OUTBUF[i+4]; 
;     genAssign
	mov	r3,_pExtByteMemory
	mov	r4,(_pExtByteMemory + 1)
;     genPlus
;     genPlusIncr
	mov	a,#0x04
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
;     genPlus
	add	a,#_EP1OUTBUF
	mov	dpl,a
;	Peephole 240	use clr instead of addc a,#0
	clr	a
	addc	a,#(_EP1OUTBUF >> 8)
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 136	removed redundant moves
	mov	r5,a
	mov	dpl,r3
	mov	dph,r4
	movx	@dptr,a
;     genPlus
;     genPlusIncr
	inc	_pExtByteMemory
	clr	a
	cjne	a,_pExtByteMemory,00145$
	inc	(_pExtByteMemory + 1)
00145$:
;HPSDR_firmware_1_0_0.c:121: for (i=0; i < EP1OUTBUF[3]; i++)
;     genPlus
;     genPlusIncr
	inc	r2
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00118$
00113$:
;HPSDR_firmware_1_0_0.c:128: FX2_RESET_EP1OUT_STALL();	; 	//reset stall bit on EP1 OUT
;     genAssign
;     genAnd
;	Peephole 248.b	optimized and to xdata
	mov	dptr,#_EP1OUTCS
	movx	a,@dptr
	mov	r2,a
	anl	a,#0xFE
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:129: FX2_ARM_EP1OUT();				//re-arm for EP1 out transfer
;     genAssign
	mov	dptr,#_EP1OUTBC
	mov	a,#0x40
	movx	@dptr,a
00122$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'process_ep0_data'
;------------------------------------------------------------
;dscr                      Allocated to registers r4 r5 
;found                     Allocated to registers r2 
;index                     Allocated to registers r3 
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:133: void process_ep0_data(void)
;	-----------------------------------------
;	 function process_ep0_data
;	-----------------------------------------
_process_ep0_data:
;HPSDR_firmware_1_0_0.c:139: switch(SETUPDAT[1])
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
	add	a,#0xff - 0x0B
	jnc	00171$
	ljmp	00146$
00171$:
;     genJumpTab
;	Peephole 254	optimized left shift
	mov	a,r2
	add	a,r2
	add	a,r2
	mov	dptr,#00172$
	jmp	@a+dptr
00172$:
	ljmp	00120$
	ljmp	00126$
	ljmp	00146$
	ljmp	00136$
	ljmp	00146$
	ljmp	00146$
	ljmp	00101$
	ljmp	00146$
	ljmp	00119$
	ljmp	00118$
	ljmp	00116$
	ljmp	00117$
;HPSDR_firmware_1_0_0.c:141: case SC_GET_DESCRIPTOR:                  // *** Get Descriptor
00101$:
;HPSDR_firmware_1_0_0.c:143: switch(SETUPDAT[3])         
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0003)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
	cjne	r2,#0x01,00173$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00102$
00173$:
;     genCmpEq
	cjne	r2,#0x02,00174$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00104$
00174$:
;     genCmpEq
	cjne	r2,#0x03,00175$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00106$
00175$:
;     genCmpEq
	cjne	r2,#0x06,00176$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00103$
00176$:
;     genCmpEq
	cjne	r2,#0x07,00177$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00105$
00177$:
	ljmp	00114$
;HPSDR_firmware_1_0_0.c:145: case GD_DEVICE:            			// Device
00102$:
;HPSDR_firmware_1_0_0.c:146: SUDPTRH = MSB(pDeviceDscr);
;     genAssign
	mov	dptr,#_pDeviceDscr
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
;     genAnd
;	Peephole 177.c	removed redundant move
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:147: SUDPTRL = LSB(pDeviceDscr);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:148: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:149: case GD_DEVICE_QUALIFIER:            // Device Qualifier
00103$:
;HPSDR_firmware_1_0_0.c:150: SUDPTRH = MSB(pDeviceQualDscr);
;     genAssign
	mov	dptr,#_pDeviceQualDscr
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
;     genAnd
;	Peephole 177.c	removed redundant move
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:151: SUDPTRL = LSB(pDeviceQualDscr);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:152: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:153: case GD_CONFIGURATION:         		// Configuration
00104$:
;HPSDR_firmware_1_0_0.c:154: SUDPTRH = MSB(pConfigDscr);
;     genAssign
	mov	dptr,#_pConfigDscr
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
;     genAnd
;	Peephole 177.c	removed redundant move
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:155: SUDPTRL = LSB(pConfigDscr);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:156: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:157: case GD_OTHER_SPEED_CONFIGURATION:  	// Other Speed Configuration
00105$:
;HPSDR_firmware_1_0_0.c:158: SUDPTRH = MSB(pOtherConfigDscr);
;     genAssign
	mov	dptr,#_pOtherConfigDscr
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
	mov	ar4,r3
;     genAnd
;	Peephole 177.c	removed redundant move
	mov	r5,#0x00
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r4
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:159: SUDPTRL = LSB(pOtherConfigDscr);
;     genAnd
	mov	r3,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r2
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:160: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:161: case GD_STRING:            			// String
00106$:
;HPSDR_firmware_1_0_0.c:162: found = FALSE;
;     genAssign
	mov	r2,#0x00
;HPSDR_firmware_1_0_0.c:163: index = SETUPDAT[2];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r3,a
;HPSDR_firmware_1_0_0.c:164: dscr = (STRINGDSCR xdata *) pStringDscr;
;     genAssign
	mov	dptr,#_pStringDscr
	movx	a,@dptr
	mov	r4,a
	inc	dptr
	movx	a,@dptr
	mov	r5,a
;     genCast
;HPSDR_firmware_1_0_0.c:165: while(dscr->type == STRING_DSCR)
;     genAssign
00109$:
;     genPlus
;     genPlusIncr
	mov	a,#0x01
;	Peephole 236.a	used r4 instead of ar4
	add	a,r4
	mov	dpl,a
;	Peephole 181	changed mov to clr
	clr	a
;	Peephole 236.b	used r5 instead of ar5
	addc	a,r5
	mov	dph,a
;     genPointerGet
;     genFarPointerGet
	movx	a,@dptr
	mov	r6,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r6,#0x03,00111$
;00178$:
;	Peephole 200	removed redundant sjmp
00179$:
;HPSDR_firmware_1_0_0.c:167: if(!index--)
;     genAssign
	mov	ar6,r3
;     genMinus
;     genMinusDec
	dec	r3
;     genIfx
	mov	a,r6
;     genIfxJump
;	Peephole 109	removed ljmp by inverse jump logic
;HPSDR_firmware_1_0_0.c:169: SUDPTRH = MSB(dscr);
;     genCast
;     genRightShift
;     genRightShiftLiteral
;     genrshTwo
;     genAnd
;	Peephole 177.c	removed redundant move
;	Peephole 256.f	loading 00180$ with zero from a
	jnz	00108$
00180$:
	mov	ar6,r4
	mov	ar7,r5
	mov	ar0,r7
	mov	r1,a
;     genCast
	mov	dptr,#_SUDPTRH
	mov	a,r0
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:170: SUDPTRL = LSB(dscr);
;     genAnd
	mov	r7,#0x00
;     genCast
	mov	dptr,#_SUDPTRL
	mov	a,r6
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:171: found = TRUE;
;     genAssign
	mov	r2,#0x01
;HPSDR_firmware_1_0_0.c:172: break;
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00111$
00108$:
;HPSDR_firmware_1_0_0.c:174: dscr = (STRINGDSCR xdata *)((WORD)dscr + dscr->length);
;     genCast
	mov	ar6,r4
	mov	ar7,r5
;     genPointerGet
;     genFarPointerGet
	mov	dpl,r4
	mov	dph,r5
	movx	a,@dptr
;     genCast
;     genPlus
;	Peephole 236.g	used r0 instead of ar0
;	Peephole 177.d	removed redundant move
	mov	r0,a
	mov	r1,#0x00
;	Peephole 236.a	used r6 instead of ar6
	add	a,r6
	mov	r6,a
;	Peephole 236.g	used r1 instead of ar1
	mov	a,r1
;	Peephole 236.b	used r7 instead of ar7
	addc	a,r7
	mov	r7,a
;     genCast
	mov	ar4,r6
	mov	ar5,r7
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00109$
00111$:
;HPSDR_firmware_1_0_0.c:176: if (!found)
;     genIfx
	mov	a,r2
;     genIfxJump
	jz	00181$
	ljmp	00147$
00181$:
;HPSDR_firmware_1_0_0.c:178: FX2_STALL_EP0();   		// Stall End Point 0
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:180: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:181: default:            				// Invalid request
00114$:
;HPSDR_firmware_1_0_0.c:182: FX2_STALL_EP0();      		// Stall End Point 0
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:184: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:186: case SC_GET_INTERFACE:               // *** Get Interface
00116$:
;HPSDR_firmware_1_0_0.c:187: EP0BUF[0] = AlternateSetting;
;     genAssign
	mov	dptr,#_AlternateSetting
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:188: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:189: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:190: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:192: case SC_SET_INTERFACE:               // *** Set Interface
00117$:
;HPSDR_firmware_1_0_0.c:193: AlternateSetting = SETUPDAT[2];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_AlternateSetting
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:194: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:196: case SC_SET_CONFIGURATION:           // *** Set Configuration
00118$:
;HPSDR_firmware_1_0_0.c:197: Configuration = SETUPDAT[2];
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genAssign
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_Configuration
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:198: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:200: case SC_GET_CONFIGURATION:           // *** Get Configuration
00119$:
;HPSDR_firmware_1_0_0.c:201: EP0BUF[0] = Configuration;
;     genAssign
	mov	dptr,#_Configuration
	movx	a,@dptr
;     genPointerSet
;     genFarPointerSet
;	Peephole 100	removed redundant mov
	mov	r2,a
	mov	dptr,#_EP0BUF
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:202: EP0BCH = 0;
;     genAssign
	mov	dptr,#_EP0BCH
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:203: EP0BCL = 1;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:204: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:206: case SC_GET_STATUS:                  // *** Get Status
00120$:
;HPSDR_firmware_1_0_0.c:208: switch(SETUPDAT[0])
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
	cjne	r2,#0x80,00182$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00121$
00182$:
;     genCmpEq
	cjne	r2,#0x81,00183$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00122$
00183$:
;     genCmpEq
	cjne	r2,#0x82,00184$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00123$
00184$:
	ljmp	00124$
;HPSDR_firmware_1_0_0.c:210: case GS_DEVICE:            		// Device
00121$:
;HPSDR_firmware_1_0_0.c:211: EP0BUF[0] = ((BYTE)RWUEN << 1) | (BYTE)SELFPWR;
;     genAssign
	mov	dptr,#_RWUEN
	movx	a,@dptr
;     genLeftShift
;     genLeftShiftLiteral
;     genlshOne
;	Peephole 105	removed redundant mov
;	Peephole 204	removed redundant mov
	add	a,acc
	mov	r2,a
;     genAssign
	mov	dptr,#_SELFPWR
	movx	a,@dptr
;     genOr
;	Peephole 105	removed redundant mov
	mov	r3,a
	orl	ar2,a
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
	mov	a,r2
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:212: EP0BUF[1] = 0;
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#(_EP0BUF + 0x0001)
;	Peephole 181	changed mov to clr
;HPSDR_firmware_1_0_0.c:213: EP0BCH = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_EP0BCH
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:214: EP0BCL = 2;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x02
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:215: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:216: case GS_INTERFACE:         		// Interface
00122$:
;HPSDR_firmware_1_0_0.c:217: EP0BUF[0] = 0;
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#_EP0BUF
;	Peephole 181	changed mov to clr
;HPSDR_firmware_1_0_0.c:218: EP0BUF[1] = 0;
;     genPointerSet
;     genFarPointerSet
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
;HPSDR_firmware_1_0_0.c:219: EP0BCH = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219.a	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#(_EP0BUF + 0x0001)
	movx	@dptr,a
	mov	dptr,#_EP0BCH
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:220: EP0BCL = 2;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x02
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:221: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:222: case GS_ENDPOINT:         		// End Point
00123$:
;HPSDR_firmware_1_0_0.c:223: EP0BUF[0] = *(BYTE xdata *) epcs(SETUPDAT[4]) & bmEPSTALL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x7E
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r3,a
;     genCmpGt
;     genCmp
	clr	c
	mov	a,#0x80
	subb	a,r3
	clr	a
	rlc	a
;     genOr
;	Peephole 105	removed redundant mov
	mov	r3,a
	orl	a,r2
;     genPlus
;	Peephole 240	use clr instead of addc a,#0
;     genPointerGet
;     genCodePointerGet
;	Peephole 181	changed mov to clr
;	Peephole 186.d	optimized movc sequence
	mov	dptr,#_EPCS_Offset_Lookup_Table
	movc	a,@a+dptr
;     genCast
;	Peephole 105	removed redundant mov
	mov	r2,a
	rlc	a
	subb	a,acc
	mov	r3,a
;     genPlus
	mov	a,#0xA1
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
	mov	r2,a
	mov	a,#0xE6
;	Peephole 236.b	used r3 instead of ar3
	addc	a,r3
	mov	r3,a
;     genCast
	mov	dpl,r2
	mov	dph,r3
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
;HPSDR_firmware_1_0_0.c:224: EP0BUF[1] = 0;
;     genPointerSet
;     genFarPointerSet
	mov	dptr,#(_EP0BUF + 0x0001)
;	Peephole 181	changed mov to clr
;HPSDR_firmware_1_0_0.c:225: EP0BCH = 0;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_EP0BCH
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:226: EP0BCL = 2;
;     genAssign
	mov	dptr,#_EP0BCL
	mov	a,#0x02
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:227: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:228: default:            				// Invalid Command
00124$:
;HPSDR_firmware_1_0_0.c:229: FX2_STALL_EP0();      		// Stall End Point 0
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:231: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:233: case SC_CLEAR_FEATURE:                // *** Clear Feature
00126$:
;HPSDR_firmware_1_0_0.c:235: switch(SETUPDAT[0])
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00127$
00185$:
;     genCmpEq
	cjne	r2,#0x02,00186$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00131$
00186$:
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:237: case FT_DEVICE:            	// Device
00127$:
;HPSDR_firmware_1_0_0.c:238: if(SETUPDAT[2] == 1)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00129$
;00187$:
;	Peephole 200	removed redundant sjmp
00188$:
;HPSDR_firmware_1_0_0.c:239: RWUEN = FALSE;       	// Disable Remote Wakeup
;     genAssign
	mov	dptr,#_RWUEN
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
	ljmp	00147$
00129$:
;HPSDR_firmware_1_0_0.c:241: FX2_STALL_EP0();   	// Stall End Point 0
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:242: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:243: case FT_ENDPOINT:         	// End Point
00131$:
;HPSDR_firmware_1_0_0.c:244: if(SETUPDAT[2] == 0)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
;     genCmpEq
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00190$
00189$:
	ljmp	00133$
00190$:
;HPSDR_firmware_1_0_0.c:246: *(BYTE xdata *) epcs(SETUPDAT[4]) &= ~bmEPSTALL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x7E
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r3,a
;     genCmpGt
;     genCmp
	clr	c
	mov	a,#0x80
	subb	a,r3
	clr	a
	rlc	a
;     genOr
;	Peephole 105	removed redundant mov
	mov	r3,a
	orl	a,r2
;     genPlus
;	Peephole 240	use clr instead of addc a,#0
;     genPointerGet
;     genCodePointerGet
;	Peephole 181	changed mov to clr
;	Peephole 186.d	optimized movc sequence
	mov	dptr,#_EPCS_Offset_Lookup_Table
	movc	a,@a+dptr
;     genCast
;	Peephole 105	removed redundant mov
	mov	r2,a
	rlc	a
	subb	a,acc
	mov	r3,a
;     genPlus
	mov	a,#0xA1
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
	mov	r2,a
	mov	a,#0xE6
;	Peephole 236.b	used r3 instead of ar3
	addc	a,r3
	mov	r3,a
;     genCast
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r4,a
;     genAnd
	anl	ar4,#0x7E
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r5,a
;     genCmpGt
;     genCmp
	clr	c
	mov	a,#0x80
	subb	a,r5
	clr	a
	rlc	a
;     genOr
;	Peephole 105	removed redundant mov
	mov	r5,a
	orl	a,r4
;     genPlus
;	Peephole 240	use clr instead of addc a,#0
;     genPointerGet
;     genCodePointerGet
;	Peephole 181	changed mov to clr
;	Peephole 186.d	optimized movc sequence
	mov	dptr,#_EPCS_Offset_Lookup_Table
	movc	a,@a+dptr
;     genCast
;	Peephole 105	removed redundant mov
	mov	r4,a
	rlc	a
	subb	a,acc
	mov	r5,a
;     genPlus
	mov	a,#0xA1
;	Peephole 236.a	used r4 instead of ar4
	add	a,r4
	mov	r4,a
	mov	a,#0xE6
;	Peephole 236.b	used r5 instead of ar5
	addc	a,r5
	mov	r5,a
;     genCast
	mov	dpl,r4
	mov	dph,r5
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
;HPSDR_firmware_1_0_0.c:247: FX2_RESET_DATA_TOGGLE( SETUPDAT[4] );
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
;     genAnd
;     genRightShift
;     genRightShiftLiteral
;     genrshOne
;	Peephole 139	removed redundant mov
	anl	a,#0x80
	mov	r2,a
	swap	a
	rl	a
	anl	a,#0x1f
	mov	r2,a
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r3,a
;     genAnd
	mov	a,#0x0F
	anl	a,r3
;     genPlus
	mov	dptr,#_TOGCTL
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
	movx	@dptr,a
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_TOGCTL
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x20
	movx	@dptr,a
	ljmp	00147$
00133$:
;HPSDR_firmware_1_0_0.c:250: FX2_STALL_EP0();   	// Stall End Point 0
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:253: break;
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:255: case SC_SET_FEATURE:                  // *** Set Feature
00136$:
;HPSDR_firmware_1_0_0.c:257: switch(SETUPDAT[0])
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#_SETUPDAT
	movx	a,@dptr
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 115.b	jump optimization
	mov	r2,a
	jz	00137$
00191$:
;     genCmpEq
	cjne	r2,#0x02,00192$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00144$
00192$:
	ljmp	00147$
;HPSDR_firmware_1_0_0.c:259: case FT_DEVICE:            // Device
00137$:
;HPSDR_firmware_1_0_0.c:261: if(SETUPDAT[2] == 1)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
;	Peephole 112.b	changed ljmp to sjmp
;	Peephole 199	optimized misc jump sequence
	cjne	r2,#0x01,00142$
;00193$:
;	Peephole 200	removed redundant sjmp
00194$:
;HPSDR_firmware_1_0_0.c:262: RWUEN = TRUE;      // Enable Remote Wakeup
;     genAssign
	mov	dptr,#_RWUEN
	mov	a,#0x01
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00147$
00142$:
;HPSDR_firmware_1_0_0.c:263: else if(SETUPDAT[2] == 2)
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0002)
	movx	a,@dptr
	mov	r2,a
;     genCmpEq
	cjne	r2,#0x02,00195$
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00147$
00195$:
;HPSDR_firmware_1_0_0.c:267: FX2_STALL_EP0();   // Stall End Point 0
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:268: break;
;HPSDR_firmware_1_0_0.c:270: case FT_ENDPOINT:         // End Point
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00147$
00144$:
;HPSDR_firmware_1_0_0.c:271: *(BYTE xdata *) epcs(SETUPDAT[4]) |= bmEPSTALL;
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r2,a
;     genAnd
	anl	ar2,#0x7E
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r3,a
;     genCmpGt
;     genCmp
	clr	c
	mov	a,#0x80
	subb	a,r3
	clr	a
	rlc	a
;     genOr
;	Peephole 105	removed redundant mov
	mov	r3,a
	orl	a,r2
;     genPlus
;	Peephole 240	use clr instead of addc a,#0
;     genPointerGet
;     genCodePointerGet
;	Peephole 181	changed mov to clr
;	Peephole 186.d	optimized movc sequence
	mov	dptr,#_EPCS_Offset_Lookup_Table
	movc	a,@a+dptr
;     genCast
;	Peephole 105	removed redundant mov
	mov	r2,a
	rlc	a
	subb	a,acc
	mov	r3,a
;     genPlus
	mov	a,#0xA1
;	Peephole 236.a	used r2 instead of ar2
	add	a,r2
	mov	r2,a
	mov	a,#0xE6
;	Peephole 236.b	used r3 instead of ar3
	addc	a,r3
	mov	r3,a
;     genCast
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r4,a
;     genAnd
	anl	ar4,#0x7E
;     genPointerGet
;     genFarPointerGet
	mov	dptr,#(_SETUPDAT + 0x0004)
	movx	a,@dptr
	mov	r5,a
;     genCmpGt
;     genCmp
	clr	c
	mov	a,#0x80
	subb	a,r5
	clr	a
	rlc	a
;     genOr
;	Peephole 105	removed redundant mov
	mov	r5,a
	orl	a,r4
;     genPlus
;	Peephole 240	use clr instead of addc a,#0
;     genPointerGet
;     genCodePointerGet
;	Peephole 181	changed mov to clr
;	Peephole 186.d	optimized movc sequence
	mov	dptr,#_EPCS_Offset_Lookup_Table
	movc	a,@a+dptr
;     genCast
;	Peephole 105	removed redundant mov
	mov	r4,a
	rlc	a
	subb	a,acc
	mov	r5,a
;     genPlus
	mov	a,#0xA1
;	Peephole 236.a	used r4 instead of ar4
	add	a,r4
	mov	r4,a
	mov	a,#0xE6
;	Peephole 236.b	used r5 instead of ar5
	addc	a,r5
	mov	r5,a
;     genCast
	mov	dpl,r4
	mov	dph,r5
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
;HPSDR_firmware_1_0_0.c:274: break;
;HPSDR_firmware_1_0_0.c:276: default:                     // *** Invalid Command
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00147$
00146$:
;HPSDR_firmware_1_0_0.c:277: FX2_STALL_EP0();         // Stall End Point 0
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:279: }
00147$:
;HPSDR_firmware_1_0_0.c:281: FX2_ACK_HANDSHAKE();	// Acknowledge handshake phase of device request
;     genAssign
;     genOr
;	Peephole 248.a	optimized or to xdata
	mov	dptr,#_EP0CS
	movx	a,@dptr
	mov	r2,a
	orl	a,#0x80
	movx	@dptr,a
00148$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'main_loop'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:286: void main_loop (void)
;	-----------------------------------------
;	 function main_loop
;	-----------------------------------------
_main_loop:
;HPSDR_firmware_1_0_0.c:288: while (TRUE)
00106$:
;HPSDR_firmware_1_0_0.c:290: if(RxSUD)            			
;     genAssign
	mov	dptr,#_RxSUD
	movx	a,@dptr
;     genIfx
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
;	Peephole 110	removed ljmp by inverse jump logic
	jz	00102$
00113$:
;HPSDR_firmware_1_0_0.c:292: process_ep0_data();         // do setup command
;     genCall
	lcall	_process_ep0_data
;HPSDR_firmware_1_0_0.c:293: RxSUD = FALSE;        		// clear SUDAV flag
;     genAssign
	mov	dptr,#_RxSUD
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
00102$:
;HPSDR_firmware_1_0_0.c:296: if (EP1HASDATA)
;     genAssign
	mov	dptr,#_EP1HASDATA
	movx	a,@dptr
;     genIfx
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
;	Peephole 110	removed ljmp by inverse jump logic
	jz	00106$
00114$:
;HPSDR_firmware_1_0_0.c:298: process_ep1_data();			// do ep1 data
;     genCall
	lcall	_process_ep1_data
;HPSDR_firmware_1_0_0.c:299: EP1HASDATA = FALSE;			// clear EP1 flag
;     genAssign
	mov	dptr,#_EP1HASDATA
;	Peephole 181	changed mov to clr
	clr	a
	movx	@dptr,a
;	Peephole 112.b	changed ljmp to sjmp
	sjmp	00106$
00108$:
	ret
;------------------------------------------------------------
;Allocation info for local variables in function 'main'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:306: void main (void)
;	-----------------------------------------
;	 function main
;	-----------------------------------------
_main:
;HPSDR_firmware_1_0_0.c:308: RxSUD = FALSE;
;     genAssign
	mov	dptr,#_RxSUD
;	Peephole 181	changed mov to clr
;HPSDR_firmware_1_0_0.c:309: EP1HASDATA = FALSE;
;     genAssign
;	Peephole 181	changed mov to clr
;	Peephole 219	removed redundant clear
	clr	a
	movx	@dptr,a
	mov	dptr,#_EP1HASDATA
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:311: init_fx2();
;     genCall
	lcall	_init_fx2
;HPSDR_firmware_1_0_0.c:312: setup_io();
;     genCall
	lcall	_setup_io
;HPSDR_firmware_1_0_0.c:313: setup_descriptor_pointers();
;     genCall
	lcall	_setup_descriptor_pointers
;HPSDR_firmware_1_0_0.c:314: setup_interrupts();
;     genCall
	lcall	_setup_interrupts
;HPSDR_firmware_1_0_0.c:316: if(FX2_NEED_TO_RENUM())
;     genAssign
	mov	dptr,#_USBCS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
;	Peephole 112.a	removed ljmp by inverse jump logic
	jb	acc.1,00102$
00106$:
;HPSDR_firmware_1_0_0.c:318: FX2_Discon(TRUE);   			// renumerate
;     genCall
	mov	dpl,#0x01
	lcall	_FX2_Discon
00102$:
;HPSDR_firmware_1_0_0.c:321: FX2_USB_CONNECT();					// Connect USB
;     genAssign
;     genAnd
;	Peephole 248.b	optimized and to xdata
	mov	dptr,#_USBCS
	movx	a,@dptr
	mov	r2,a
	anl	a,#0xF7
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:322: FX2_DATA_WAIT_STATE(0); 			// MOVX takes 2 cycles (valid 0-7)
;     genAssign
	mov	_CKCON,#0x00
;HPSDR_firmware_1_0_0.c:324: IOA = 0x02;
;     genAssign
	mov	_IOA,#0x02
;HPSDR_firmware_1_0_0.c:325: IOE = 0x02;
;     genAssign
	mov	_IOE,#0x02
;HPSDR_firmware_1_0_0.c:327: main_loop();
;     genCall
;	Peephole 253.b	replaced lcall/ret with ljmp
	ljmp	_main_loop
;------------------------------------------------------------
;Allocation info for local variables in function 'ISR_Sudav'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:333: void ISR_Sudav(void) interrupt  
;	-----------------------------------------
;	 function ISR_Sudav
;	-----------------------------------------
_ISR_Sudav:
	push	acc
	push	dpl
	push	dph
;HPSDR_firmware_1_0_0.c:335: RxSUD = TRUE;            // Set flag
;     genAssign
	mov	dptr,#_RxSUD
	mov	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:336: FX2_IRQ_CLEAR();
;     genAnd
	anl	_EXIF,#0xEF
;HPSDR_firmware_1_0_0.c:337: FX2_CLEAR_USB_IRQ(bmSUDAV);	// Clear SUDAV IRQ
;     genAssign
	mov	dptr,#_USBIRQ
	mov	a,#0x01
	movx	@dptr,a
00101$:
	pop	dph
	pop	dpl
	pop	acc
	reti
;	eliminated unneeded push/pop psw
;	eliminated unneeded push/pop b
;------------------------------------------------------------
;Allocation info for local variables in function 'ISR_Sutok'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:341: void ISR_Sutok(void) interrupt  
;	-----------------------------------------
;	 function ISR_Sutok
;	-----------------------------------------
_ISR_Sutok:
	push	acc
	push	dpl
	push	dph
;HPSDR_firmware_1_0_0.c:343: FX2_IRQ_CLEAR();
;     genAnd
	anl	_EXIF,#0xEF
;HPSDR_firmware_1_0_0.c:344: FX2_CLEAR_USB_IRQ(bmSUTOK);	// Clear SUTOK IRQ
;     genAssign
	mov	dptr,#_USBIRQ
	mov	a,#0x04
	movx	@dptr,a
00101$:
	pop	dph
	pop	dpl
	pop	acc
	reti
;	eliminated unneeded push/pop psw
;	eliminated unneeded push/pop b
;------------------------------------------------------------
;Allocation info for local variables in function 'ISR_Usb_Reset'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:347: void ISR_Usb_Reset(void) interrupt  
;	-----------------------------------------
;	 function ISR_Usb_Reset
;	-----------------------------------------
_ISR_Usb_Reset:
	push	acc
	push	dpl
	push	dph
	push	ar2
	push	ar3
;HPSDR_firmware_1_0_0.c:350: pConfigDscr = pFullSpeedConfigDscr;
;     genAssign
	mov	dptr,#_pFullSpeedConfigDscr
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;     genAssign
	mov	dptr,#_pConfigDscr
	mov	a,r2
	movx	@dptr,a
	inc	dptr
	mov	a,r3
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:351: pOtherConfigDscr = pHighSpeedConfigDscr;
;     genAssign
	mov	dptr,#_pHighSpeedConfigDscr
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;     genAssign
	mov	dptr,#_pOtherConfigDscr
	mov	a,r2
	movx	@dptr,a
	inc	dptr
	mov	a,r3
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:353: FX2_IRQ_CLEAR();
;     genAnd
	anl	_EXIF,#0xEF
;HPSDR_firmware_1_0_0.c:354: FX2_CLEAR_USB_IRQ(bmURES);	// Clear URES IRQ
;     genAssign
	mov	dptr,#_USBIRQ
	mov	a,#0x10
	movx	@dptr,a
00101$:
	pop	ar3
	pop	ar2
	pop	dph
	pop	dpl
	pop	acc
	reti
;	eliminated unneeded push/pop psw
;	eliminated unneeded push/pop b
;------------------------------------------------------------
;Allocation info for local variables in function 'ISR_Highspeed'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:357: void ISR_Highspeed(void) interrupt  
;	-----------------------------------------
;	 function ISR_Highspeed
;	-----------------------------------------
_ISR_Highspeed:
	push	acc
	push	dpl
	push	dph
	push	ar2
	push	ar3
;HPSDR_firmware_1_0_0.c:359: if (FX2_HIGHSPEED())
;     genAssign
	mov	dptr,#_USBCS
	movx	a,@dptr
;     genAnd
;	Peephole 105	removed redundant mov
	mov	r2,a
;     genIfxJump
;	Peephole 111	removed ljmp by inverse jump logic
	jnb	acc.7,00102$
00106$:
;HPSDR_firmware_1_0_0.c:361: pConfigDscr = pHighSpeedConfigDscr;
;     genAssign
	mov	dptr,#_pHighSpeedConfigDscr
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;     genAssign
	mov	dptr,#_pConfigDscr
	mov	a,r2
	movx	@dptr,a
	inc	dptr
	mov	a,r3
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:362: pOtherConfigDscr = pFullSpeedConfigDscr;
;     genAssign
	mov	dptr,#_pFullSpeedConfigDscr
	movx	a,@dptr
	mov	r2,a
	inc	dptr
	movx	a,@dptr
	mov	r3,a
;     genAssign
	mov	dptr,#_pOtherConfigDscr
	mov	a,r2
	movx	@dptr,a
	inc	dptr
	mov	a,r3
	movx	@dptr,a
00102$:
;HPSDR_firmware_1_0_0.c:365: FX2_IRQ_CLEAR();
;     genAnd
	anl	_EXIF,#0xEF
;HPSDR_firmware_1_0_0.c:366: FX2_CLEAR_USB_IRQ(bmHSGRANT);	// Clear HSGrant IRQ
;     genAssign
	mov	dptr,#_USBIRQ
	mov	a,#0x20
	movx	@dptr,a
00103$:
	pop	ar3
	pop	ar2
	pop	dph
	pop	dpl
	pop	acc
	reti
;	eliminated unneeded push/pop psw
;	eliminated unneeded push/pop b
;------------------------------------------------------------
;Allocation info for local variables in function 'ISR_Ep1out'
;------------------------------------------------------------
;------------------------------------------------------------
;HPSDR_firmware_1_0_0.c:369: void ISR_Ep1out(void) interrupt
;	-----------------------------------------
;	 function ISR_Ep1out
;	-----------------------------------------
_ISR_Ep1out:
	push	acc
	push	dpl
	push	dph
;HPSDR_firmware_1_0_0.c:371: EP1HASDATA = TRUE;            // Set flag
;     genAssign
	mov	dptr,#_EP1HASDATA
	mov	a,#0x01
	movx	@dptr,a
;HPSDR_firmware_1_0_0.c:372: FX2_IRQ_CLEAR();
;     genAnd
	anl	_EXIF,#0xEF
;HPSDR_firmware_1_0_0.c:373: FX2_CLEAR_USB_EPIRQ(bmEP1OUT);	// Clear EP1OUT IRQ
;     genAssign
	mov	dptr,#_EPIRQ
	mov	a,#0x08
	movx	@dptr,a
00101$:
	pop	dph
	pop	dpl
	pop	acc
	reti
;	eliminated unneeded push/pop psw
;	eliminated unneeded push/pop b
	.area CSEG    (CODE)
_EPCS_Offset_Lookup_Table:
	.db #0x00
	.db #0x01
	.db #0x02
	.db #0x02
	.db #0x03
	.db #0x03
	.db #0x04
	.db #0x04
	.db #0x05
	.db #0x05
	.area XINIT   (CODE)
