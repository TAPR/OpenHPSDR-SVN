                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (Oct  3 2005)
                              4 ; This file generated Fri Oct  7 14:51:19 2005
                              5 ;--------------------------------------------------------
                              6 	.module HPSDR_firmware_1_0_0
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _EPCS_Offset_Lookup_Table
                             13 	.globl _ISR_Ep1out
                             14 	.globl _ISR_Highspeed
                             15 	.globl _ISR_Usb_Reset
                             16 	.globl _ISR_Sutok
                             17 	.globl _ISR_Sudav
                             18 	.globl _main
                             19 	.globl _init_fx2
                             20 	.globl _EIP
                             21 	.globl _B
                             22 	.globl _EIE
                             23 	.globl _ACC
                             24 	.globl _EICON
                             25 	.globl _PSW
                             26 	.globl _TH2
                             27 	.globl _TL2
                             28 	.globl _RCAP2H
                             29 	.globl _RCAP2L
                             30 	.globl _T2CON
                             31 	.globl _SBUF1
                             32 	.globl _SCON1
                             33 	.globl _GPIFSGLDATLNOX
                             34 	.globl _GPIFSGLDATLX
                             35 	.globl _GPIFSGLDATH
                             36 	.globl _GPIFTRIG
                             37 	.globl _EP01STAT
                             38 	.globl _IP
                             39 	.globl _OEE
                             40 	.globl _OED
                             41 	.globl _OEC
                             42 	.globl _OEB
                             43 	.globl _OEA
                             44 	.globl _IOE
                             45 	.globl _IOD
                             46 	.globl _AUTOPTRSETUP
                             47 	.globl _EP68FIFOFLGS
                             48 	.globl _EP24FIFOFLGS
                             49 	.globl _EP2468STAT
                             50 	.globl _IE
                             51 	.globl _INT4CLR
                             52 	.globl _INT2CLR
                             53 	.globl _IOC
                             54 	.globl _AUTODAT2
                             55 	.globl _AUTOPTRL2
                             56 	.globl _AUTOPTRH2
                             57 	.globl _AUTODAT1
                             58 	.globl _APTR1L
                             59 	.globl _APTR1H
                             60 	.globl _SBU
                             61 	.globl _SCO
                             62 	.globl _MPA
                             63 	.globl _EXIF
                             64 	.globl _IOB
                             65 	.globl _SPC_
                             66 	.globl _CKCON
                             67 	.globl _TH1
                             68 	.globl _TH0
                             69 	.globl _TL1
                             70 	.globl _TL0
                             71 	.globl _TMOD
                             72 	.globl _TCON
                             73 	.globl _PCON
                             74 	.globl _DPS
                             75 	.globl _DPH1
                             76 	.globl _DPL1
                             77 	.globl _DPH
                             78 	.globl _DPL
                             79 	.globl _SP
                             80 	.globl _IOA
                             81 	.globl _pStringDscr
                             82 	.globl _pOtherConfigDscr
                             83 	.globl _pConfigDscr
                             84 	.globl _pFullSpeedConfigDscr
                             85 	.globl _pHighSpeedConfigDscr
                             86 	.globl _pDeviceQualDscr
                             87 	.globl _pDeviceDscr
                             88 	.globl _test_extern_mem2
                             89 	.globl _test_extern_mem1
                             90 	.globl _AlternateSetting
                             91 	.globl _Configuration
                             92 	.globl _wascalled
                             93 	.globl _got_vend_cmd
                             94 	.globl _EP1HASDATA
                             95 	.globl _SELFPWR
                             96 	.globl _RWUEN
                             97 	.globl _RxSUD
                             98 	.globl _EP8FIFOBUF
                             99 	.globl _EP6FIFOBUF
                            100 	.globl _EP4FIFOBUF
                            101 	.globl _EP2FIFOBUF
                            102 	.globl _EP1INBUF
                            103 	.globl _EP1OUTBUF
                            104 	.globl _EP0BUF
                            105 	.globl _CT4
                            106 	.globl _CT3
                            107 	.globl _CT2
                            108 	.globl _CT1
                            109 	.globl _USBTEST
                            110 	.globl _TESTCFG
                            111 	.globl _DBUG
                            112 	.globl _UDMACRCQUAL
                            113 	.globl _UDMACRCL
                            114 	.globl _UDMACRCH
                            115 	.globl _GPIFHOLDAMOUNT
                            116 	.globl _FLOWSTBHPERIOD
                            117 	.globl _FLOWSTBEDGE
                            118 	.globl _FLOWSTB
                            119 	.globl _FLOWHOLDOFF
                            120 	.globl _FLOWEQ1CTL
                            121 	.globl _FLOWEQ0CTL
                            122 	.globl _FLOWLOGIC
                            123 	.globl _FLOWSTATE
                            124 	.globl _GPIFABORT
                            125 	.globl _GPIFREADYSTAT
                            126 	.globl _GPIFREADYCFG
                            127 	.globl _XGPIFSGLDATLNOX
                            128 	.globl _XGPIFSGLDATLX
                            129 	.globl _XGPIFSGLDATH
                            130 	.globl _EP8GPIFTRIG
                            131 	.globl _EP8GPIFPFSTOP
                            132 	.globl _EP8GPIFFLGSEL
                            133 	.globl _EP6GPIFTRIG
                            134 	.globl _EP6GPIFPFSTOP
                            135 	.globl _EP6GPIFFLGSEL
                            136 	.globl _EP4GPIFTRIG
                            137 	.globl _EP4GPIFPFSTOP
                            138 	.globl _EP4GPIFFLGSEL
                            139 	.globl _EP2GPIFTRIG
                            140 	.globl _EP2GPIFPFSTOP
                            141 	.globl _EP2GPIFFLGSEL
                            142 	.globl _GPIFTCB0
                            143 	.globl _GPIFTCB1
                            144 	.globl _GPIFTCB2
                            145 	.globl _GPIFTCB3
                            146 	.globl _GPIFADRL
                            147 	.globl _GPIFADRH
                            148 	.globl _GPIFCTLCFG
                            149 	.globl _GPIFIDLECTL
                            150 	.globl _GPIFIDLECS
                            151 	.globl _GPIFWFSELECT
                            152 	.globl _SETUPDAT
                            153 	.globl _SUDPTRCTL
                            154 	.globl _SUDPTRL
                            155 	.globl _SUDPTRH
                            156 	.globl _EP8FIFOBCL
                            157 	.globl _EP8FIFOBCH
                            158 	.globl _EP6FIFOBCL
                            159 	.globl _EP6FIFOBCH
                            160 	.globl _EP4FIFOBCL
                            161 	.globl _EP4FIFOBCH
                            162 	.globl _EP2FIFOBCL
                            163 	.globl _EP2FIFOBCH
                            164 	.globl _EP8FIFOFLGS
                            165 	.globl _EP6FIFOFLGS
                            166 	.globl _EP4FIFOFLGS
                            167 	.globl _EP2FIFOFLGS
                            168 	.globl _EP8CS
                            169 	.globl _EP6CS
                            170 	.globl _EP4CS
                            171 	.globl _EP2CS
                            172 	.globl _EP1INCS
                            173 	.globl _EP1OUTCS
                            174 	.globl _EP0CS
                            175 	.globl _EP8BCL
                            176 	.globl _EP8BCH
                            177 	.globl _EP6BCL
                            178 	.globl _EP6BCH
                            179 	.globl _EP4BCL
                            180 	.globl _EP4BCH
                            181 	.globl _EP2BCL
                            182 	.globl _EP2BCH
                            183 	.globl _EP1INBC
                            184 	.globl _EP1OUTBC
                            185 	.globl _EP0BCL
                            186 	.globl _EP0BCH
                            187 	.globl _FNADDR
                            188 	.globl _MICROFRAME
                            189 	.globl _USBFRAMEL
                            190 	.globl _USBFRAMEH
                            191 	.globl _TOGCTL
                            192 	.globl _WAKEUPCS
                            193 	.globl _SUSPEND
                            194 	.globl _USBCS
                            195 	.globl _XAUTODAT2
                            196 	.globl _XAUTODAT1
                            197 	.globl _I2CTL
                            198 	.globl _I2DAT
                            199 	.globl _I2CS
                            200 	.globl _PORTECFG
                            201 	.globl _PORTCCFG
                            202 	.globl _PORTACFG
                            203 	.globl _INTSETUP
                            204 	.globl _INT4IVEC
                            205 	.globl _INT2IVEC
                            206 	.globl _CLRERRCNT
                            207 	.globl _ERRCNTLIM
                            208 	.globl _USBERRIRQ
                            209 	.globl _USBERRIE
                            210 	.globl _GPIFIRQ
                            211 	.globl _GPIFIE
                            212 	.globl _EPIRQ
                            213 	.globl _EPIE
                            214 	.globl _USBIRQ
                            215 	.globl _USBIE
                            216 	.globl _NAKIRQ
                            217 	.globl _NAKIE
                            218 	.globl _IBNIRQ
                            219 	.globl _IBNIE
                            220 	.globl _EP8FIFOIRQ
                            221 	.globl _EP8FIFOIE
                            222 	.globl _EP6FIFOIRQ
                            223 	.globl _EP6FIFOIE
                            224 	.globl _EP4FIFOIRQ
                            225 	.globl _EP4FIFOIE
                            226 	.globl _EP2FIFOIRQ
                            227 	.globl _EP2FIFOIE
                            228 	.globl _OUTPKTEND
                            229 	.globl _INPKTEND
                            230 	.globl _EP8ISOINPKTS
                            231 	.globl _EP6ISOINPKTS
                            232 	.globl _EP4ISOINPKTS
                            233 	.globl _EP2ISOINPKTS
                            234 	.globl _EP8FIFOPFL
                            235 	.globl _EP8FIFOPFH
                            236 	.globl _EP6FIFOPFL
                            237 	.globl _EP6FIFOPFH
                            238 	.globl _EP4FIFOPFL
                            239 	.globl _EP4FIFOPFH
                            240 	.globl _EP2FIFOPFL
                            241 	.globl _EP2FIFOPFH
                            242 	.globl _EP8AUTOINLENL
                            243 	.globl _EP8AUTOINLENH
                            244 	.globl _EP6AUTOINLENL
                            245 	.globl _EP6AUTOINLENH
                            246 	.globl _EP4AUTOINLENL
                            247 	.globl _EP4AUTOINLENH
                            248 	.globl _EP2AUTOINLENL
                            249 	.globl _EP2AUTOINLENH
                            250 	.globl _EP8FIFOCFG
                            251 	.globl _EP6FIFOCFG
                            252 	.globl _EP4FIFOCFG
                            253 	.globl _EP2FIFOCFG
                            254 	.globl _EP8CFG
                            255 	.globl _EP6CFG
                            256 	.globl _EP4CFG
                            257 	.globl _EP2CFG
                            258 	.globl _EP1INCFG
                            259 	.globl _EP1OUTCFG
                            260 	.globl _REVCTL
                            261 	.globl _REVID
                            262 	.globl _FIFOPINPOLAR
                            263 	.globl _UART230
                            264 	.globl _BPADDRL
                            265 	.globl _BPADDRH
                            266 	.globl _BREAKPT
                            267 	.globl _FIFORESET
                            268 	.globl _PINFLAGSCD
                            269 	.globl _PINFLAGSAB
                            270 	.globl _IFCONFIG
                            271 	.globl _CPUCS
                            272 	.globl _RES_WAVEDATA_END
                            273 	.globl _GPIF_WAVE_DATA
                            274 	.globl _pExtByteMemory
                            275 	.globl _pExtMemory
                            276 	.globl _FX2_Delay1ms
                            277 	.globl _FX2_Delay
                            278 	.globl _FX2_Discon
                            279 	.globl _setup_io
                            280 	.globl _setup_interrupts
                            281 	.globl _setup_descriptor_pointers
                            282 	.globl _clear_ext_ram
                            283 	.globl _process_ep1_data
                            284 	.globl _process_ep0_data
                            285 	.globl _main_loop
                            286 ;--------------------------------------------------------
                            287 ; special function registers
                            288 ;--------------------------------------------------------
                            289 	.area RSEG    (DATA)
                    0080    290 _IOA	=	0x0080
                    0081    291 _SP	=	0x0081
                    0082    292 _DPL	=	0x0082
                    0083    293 _DPH	=	0x0083
                    0084    294 _DPL1	=	0x0084
                    0085    295 _DPH1	=	0x0085
                    0086    296 _DPS	=	0x0086
                    0087    297 _PCON	=	0x0087
                    0088    298 _TCON	=	0x0088
                    0089    299 _TMOD	=	0x0089
                    008A    300 _TL0	=	0x008a
                    008B    301 _TL1	=	0x008b
                    008C    302 _TH0	=	0x008c
                    008D    303 _TH1	=	0x008d
                    008E    304 _CKCON	=	0x008e
                    008F    305 _SPC_	=	0x008f
                    0090    306 _IOB	=	0x0090
                    0091    307 _EXIF	=	0x0091
                    0092    308 _MPA	=	0x0092
                    0098    309 _SCO	=	0x0098
                    0099    310 _SBU	=	0x0099
                    009A    311 _APTR1H	=	0x009a
                    009B    312 _APTR1L	=	0x009b
                    009C    313 _AUTODAT1	=	0x009c
                    009D    314 _AUTOPTRH2	=	0x009d
                    009E    315 _AUTOPTRL2	=	0x009e
                    009F    316 _AUTODAT2	=	0x009f
                    00A0    317 _IOC	=	0x00a0
                    00A1    318 _INT2CLR	=	0x00a1
                    00A2    319 _INT4CLR	=	0x00a2
                    00A8    320 _IE	=	0x00a8
                    00AA    321 _EP2468STAT	=	0x00aa
                    00AB    322 _EP24FIFOFLGS	=	0x00ab
                    00AC    323 _EP68FIFOFLGS	=	0x00ac
                    00AF    324 _AUTOPTRSETUP	=	0x00af
                    00B0    325 _IOD	=	0x00b0
                    00B1    326 _IOE	=	0x00b1
                    00B2    327 _OEA	=	0x00b2
                    00B3    328 _OEB	=	0x00b3
                    00B4    329 _OEC	=	0x00b4
                    00B5    330 _OED	=	0x00b5
                    00B6    331 _OEE	=	0x00b6
                    00B8    332 _IP	=	0x00b8
                    00BA    333 _EP01STAT	=	0x00ba
                    00BB    334 _GPIFTRIG	=	0x00bb
                    00BD    335 _GPIFSGLDATH	=	0x00bd
                    00BE    336 _GPIFSGLDATLX	=	0x00be
                    00BF    337 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    338 _SCON1	=	0x00c0
                    00C1    339 _SBUF1	=	0x00c1
                    00C8    340 _T2CON	=	0x00c8
                    00CA    341 _RCAP2L	=	0x00ca
                    00CB    342 _RCAP2H	=	0x00cb
                    00CC    343 _TL2	=	0x00cc
                    00CD    344 _TH2	=	0x00cd
                    00D0    345 _PSW	=	0x00d0
                    00D8    346 _EICON	=	0x00d8
                    00E0    347 _ACC	=	0x00e0
                    00E8    348 _EIE	=	0x00e8
                    00F0    349 _B	=	0x00f0
                    00F8    350 _EIP	=	0x00f8
                            351 ;--------------------------------------------------------
                            352 ; special function bits 
                            353 ;--------------------------------------------------------
                            354 	.area RSEG    (DATA)
                            355 ;--------------------------------------------------------
                            356 ; overlayable register banks 
                            357 ;--------------------------------------------------------
                            358 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     359 	.ds 8
                            360 ;--------------------------------------------------------
                            361 ; internal ram data
                            362 ;--------------------------------------------------------
                            363 	.area DSEG    (DATA)
   0008                     364 _pExtMemory::
   0008                     365 	.ds 2
   000A                     366 _pExtByteMemory::
   000A                     367 	.ds 2
                            368 ;--------------------------------------------------------
                            369 ; overlayable items in internal ram 
                            370 ;--------------------------------------------------------
                            371 	.area	OSEG    (OVR,DATA)
                            372 	.area	OSEG    (OVR,DATA)
                            373 ;--------------------------------------------------------
                            374 ; Stack segment in internal ram 
                            375 ;--------------------------------------------------------
                            376 	.area	SSEG	(DATA)
   000C                     377 __start__stack:
   000C                     378 	.ds	1
                            379 
                            380 ;--------------------------------------------------------
                            381 ; indirectly addressable internal ram data
                            382 ;--------------------------------------------------------
                            383 	.area ISEG    (DATA)
                            384 ;--------------------------------------------------------
                            385 ; bit data
                            386 ;--------------------------------------------------------
                            387 	.area BSEG    (BIT)
                            388 ;--------------------------------------------------------
                            389 ; paged external ram data
                            390 ;--------------------------------------------------------
                            391 	.area PSEG    (PAG,XDATA)
                            392 ;--------------------------------------------------------
                            393 ; external ram data
                            394 ;--------------------------------------------------------
                            395 	.area XSEG    (XDATA)
                    E400    396 _GPIF_WAVE_DATA	=	0xe400
                    E480    397 _RES_WAVEDATA_END	=	0xe480
                    E600    398 _CPUCS	=	0xe600
                    E601    399 _IFCONFIG	=	0xe601
                    E602    400 _PINFLAGSAB	=	0xe602
                    E603    401 _PINFLAGSCD	=	0xe603
                    E604    402 _FIFORESET	=	0xe604
                    E605    403 _BREAKPT	=	0xe605
                    E606    404 _BPADDRH	=	0xe606
                    E607    405 _BPADDRL	=	0xe607
                    E608    406 _UART230	=	0xe608
                    E609    407 _FIFOPINPOLAR	=	0xe609
                    E60A    408 _REVID	=	0xe60a
                    E60B    409 _REVCTL	=	0xe60b
                    E610    410 _EP1OUTCFG	=	0xe610
                    E611    411 _EP1INCFG	=	0xe611
                    E612    412 _EP2CFG	=	0xe612
                    E613    413 _EP4CFG	=	0xe613
                    E614    414 _EP6CFG	=	0xe614
                    E615    415 _EP8CFG	=	0xe615
                    E618    416 _EP2FIFOCFG	=	0xe618
                    E619    417 _EP4FIFOCFG	=	0xe619
                    E61A    418 _EP6FIFOCFG	=	0xe61a
                    E61B    419 _EP8FIFOCFG	=	0xe61b
                    E620    420 _EP2AUTOINLENH	=	0xe620
                    E621    421 _EP2AUTOINLENL	=	0xe621
                    E622    422 _EP4AUTOINLENH	=	0xe622
                    E623    423 _EP4AUTOINLENL	=	0xe623
                    E624    424 _EP6AUTOINLENH	=	0xe624
                    E625    425 _EP6AUTOINLENL	=	0xe625
                    E626    426 _EP8AUTOINLENH	=	0xe626
                    E627    427 _EP8AUTOINLENL	=	0xe627
                    E630    428 _EP2FIFOPFH	=	0xe630
                    E631    429 _EP2FIFOPFL	=	0xe631
                    E632    430 _EP4FIFOPFH	=	0xe632
                    E633    431 _EP4FIFOPFL	=	0xe633
                    E634    432 _EP6FIFOPFH	=	0xe634
                    E635    433 _EP6FIFOPFL	=	0xe635
                    E636    434 _EP8FIFOPFH	=	0xe636
                    E637    435 _EP8FIFOPFL	=	0xe637
                    E640    436 _EP2ISOINPKTS	=	0xe640
                    E641    437 _EP4ISOINPKTS	=	0xe641
                    E642    438 _EP6ISOINPKTS	=	0xe642
                    E643    439 _EP8ISOINPKTS	=	0xe643
                    E648    440 _INPKTEND	=	0xe648
                    E649    441 _OUTPKTEND	=	0xe649
                    E650    442 _EP2FIFOIE	=	0xe650
                    E651    443 _EP2FIFOIRQ	=	0xe651
                    E652    444 _EP4FIFOIE	=	0xe652
                    E653    445 _EP4FIFOIRQ	=	0xe653
                    E654    446 _EP6FIFOIE	=	0xe654
                    E655    447 _EP6FIFOIRQ	=	0xe655
                    E656    448 _EP8FIFOIE	=	0xe656
                    E657    449 _EP8FIFOIRQ	=	0xe657
                    E658    450 _IBNIE	=	0xe658
                    E659    451 _IBNIRQ	=	0xe659
                    E65A    452 _NAKIE	=	0xe65a
                    E65B    453 _NAKIRQ	=	0xe65b
                    E65C    454 _USBIE	=	0xe65c
                    E65D    455 _USBIRQ	=	0xe65d
                    E65E    456 _EPIE	=	0xe65e
                    E65F    457 _EPIRQ	=	0xe65f
                    E660    458 _GPIFIE	=	0xe660
                    E661    459 _GPIFIRQ	=	0xe661
                    E662    460 _USBERRIE	=	0xe662
                    E663    461 _USBERRIRQ	=	0xe663
                    E664    462 _ERRCNTLIM	=	0xe664
                    E665    463 _CLRERRCNT	=	0xe665
                    E666    464 _INT2IVEC	=	0xe666
                    E667    465 _INT4IVEC	=	0xe667
                    E668    466 _INTSETUP	=	0xe668
                    E670    467 _PORTACFG	=	0xe670
                    E671    468 _PORTCCFG	=	0xe671
                    E672    469 _PORTECFG	=	0xe672
                    E678    470 _I2CS	=	0xe678
                    E679    471 _I2DAT	=	0xe679
                    E67A    472 _I2CTL	=	0xe67a
                    E67B    473 _XAUTODAT1	=	0xe67b
                    E67C    474 _XAUTODAT2	=	0xe67c
                    E680    475 _USBCS	=	0xe680
                    E681    476 _SUSPEND	=	0xe681
                    E682    477 _WAKEUPCS	=	0xe682
                    E683    478 _TOGCTL	=	0xe683
                    E684    479 _USBFRAMEH	=	0xe684
                    E685    480 _USBFRAMEL	=	0xe685
                    E686    481 _MICROFRAME	=	0xe686
                    E687    482 _FNADDR	=	0xe687
                    E68A    483 _EP0BCH	=	0xe68a
                    E68B    484 _EP0BCL	=	0xe68b
                    E68D    485 _EP1OUTBC	=	0xe68d
                    E68F    486 _EP1INBC	=	0xe68f
                    E690    487 _EP2BCH	=	0xe690
                    E691    488 _EP2BCL	=	0xe691
                    E694    489 _EP4BCH	=	0xe694
                    E695    490 _EP4BCL	=	0xe695
                    E698    491 _EP6BCH	=	0xe698
                    E699    492 _EP6BCL	=	0xe699
                    E69C    493 _EP8BCH	=	0xe69c
                    E69D    494 _EP8BCL	=	0xe69d
                    E6A0    495 _EP0CS	=	0xe6a0
                    E6A1    496 _EP1OUTCS	=	0xe6a1
                    E6A2    497 _EP1INCS	=	0xe6a2
                    E6A3    498 _EP2CS	=	0xe6a3
                    E6A4    499 _EP4CS	=	0xe6a4
                    E6A5    500 _EP6CS	=	0xe6a5
                    E6A6    501 _EP8CS	=	0xe6a6
                    E6A7    502 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    503 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    504 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    505 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    506 _EP2FIFOBCH	=	0xe6ab
                    E6AC    507 _EP2FIFOBCL	=	0xe6ac
                    E6AD    508 _EP4FIFOBCH	=	0xe6ad
                    E6AE    509 _EP4FIFOBCL	=	0xe6ae
                    E6AF    510 _EP6FIFOBCH	=	0xe6af
                    E6B0    511 _EP6FIFOBCL	=	0xe6b0
                    E6B1    512 _EP8FIFOBCH	=	0xe6b1
                    E6B2    513 _EP8FIFOBCL	=	0xe6b2
                    E6B3    514 _SUDPTRH	=	0xe6b3
                    E6B4    515 _SUDPTRL	=	0xe6b4
                    E6B5    516 _SUDPTRCTL	=	0xe6b5
                    E6B8    517 _SETUPDAT	=	0xe6b8
                    E6C0    518 _GPIFWFSELECT	=	0xe6c0
                    E6C1    519 _GPIFIDLECS	=	0xe6c1
                    E6C2    520 _GPIFIDLECTL	=	0xe6c2
                    E6C3    521 _GPIFCTLCFG	=	0xe6c3
                    E6C4    522 _GPIFADRH	=	0xe6c4
                    E6C5    523 _GPIFADRL	=	0xe6c5
                    E6CE    524 _GPIFTCB3	=	0xe6ce
                    E6CF    525 _GPIFTCB2	=	0xe6cf
                    E6D0    526 _GPIFTCB1	=	0xe6d0
                    E6D1    527 _GPIFTCB0	=	0xe6d1
                    E6D2    528 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    529 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    530 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    531 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    532 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    533 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    534 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    535 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    536 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    537 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    538 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    539 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    540 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    541 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    542 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    543 _GPIFREADYCFG	=	0xe6f3
                    E6F4    544 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    545 _GPIFABORT	=	0xe6f5
                    E6C6    546 _FLOWSTATE	=	0xe6c6
                    E6C7    547 _FLOWLOGIC	=	0xe6c7
                    E6C8    548 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    549 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    550 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    551 _FLOWSTB	=	0xe6cb
                    E6CC    552 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    553 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    554 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    555 _UDMACRCH	=	0xe67d
                    E67E    556 _UDMACRCL	=	0xe67e
                    E67F    557 _UDMACRCQUAL	=	0xe67f
                    E6F8    558 _DBUG	=	0xe6f8
                    E6F9    559 _TESTCFG	=	0xe6f9
                    E6FA    560 _USBTEST	=	0xe6fa
                    E6FB    561 _CT1	=	0xe6fb
                    E6FC    562 _CT2	=	0xe6fc
                    E6FD    563 _CT3	=	0xe6fd
                    E6FE    564 _CT4	=	0xe6fe
                    E740    565 _EP0BUF	=	0xe740
                    E780    566 _EP1OUTBUF	=	0xe780
                    E7C0    567 _EP1INBUF	=	0xe7c0
                    F000    568 _EP2FIFOBUF	=	0xf000
                    F400    569 _EP4FIFOBUF	=	0xf400
                    F800    570 _EP6FIFOBUF	=	0xf800
                    FC00    571 _EP8FIFOBUF	=	0xfc00
   2200                     572 _RxSUD::
   2200                     573 	.ds 1
   2201                     574 _RWUEN::
   2201                     575 	.ds 1
   2202                     576 _SELFPWR::
   2202                     577 	.ds 1
   2203                     578 _EP1HASDATA::
   2203                     579 	.ds 1
   2204                     580 _got_vend_cmd::
   2204                     581 	.ds 1
   2205                     582 _wascalled::
   2205                     583 	.ds 1
   2206                     584 _Configuration::
   2206                     585 	.ds 1
   2207                     586 _AlternateSetting::
   2207                     587 	.ds 1
                    4002    588 _test_extern_mem1	=	0x4002
                    4004    589 _test_extern_mem2	=	0x4004
   2208                     590 _pDeviceDscr::
   2208                     591 	.ds 2
   220A                     592 _pDeviceQualDscr::
   220A                     593 	.ds 2
   220C                     594 _pHighSpeedConfigDscr::
   220C                     595 	.ds 2
   220E                     596 _pFullSpeedConfigDscr::
   220E                     597 	.ds 2
   2210                     598 _pConfigDscr::
   2210                     599 	.ds 2
   2212                     600 _pOtherConfigDscr::
   2212                     601 	.ds 2
   2214                     602 _pStringDscr::
   2214                     603 	.ds 2
                            604 ;--------------------------------------------------------
                            605 ; external initialized ram data
                            606 ;--------------------------------------------------------
                            607 	.area XISEG   (XDATA)
                            608 	.area CSEG    (CODE)
                            609 	.area GSINIT0 (CODE)
                            610 	.area GSINIT1 (CODE)
                            611 	.area GSINIT2 (CODE)
                            612 	.area GSINIT3 (CODE)
                            613 	.area GSINIT4 (CODE)
                            614 	.area GSINIT5 (CODE)
                            615 ;--------------------------------------------------------
                            616 ; interrupt vector 
                            617 ;--------------------------------------------------------
                            618 	.area CSEG    (CODE)
   0200                     619 __interrupt_vect:
   0200 02 09 2A            620 	ljmp	__sdcc_gsinit_startup
                            621 ;--------------------------------------------------------
                            622 ; global & static initialisations
                            623 ;--------------------------------------------------------
                            624 	.area CSEG    (CODE)
                            625 	.area GSINIT  (CODE)
                            626 	.area GSFINAL (CODE)
                            627 	.area GSINIT  (CODE)
                            628 	.globl __sdcc_gsinit_startup
                            629 	.globl __sdcc_program_startup
                            630 	.globl __start__stack
                            631 	.globl __mcs51_genXINIT
                            632 	.globl __mcs51_genXRAMCLEAR
                            633 	.globl __mcs51_genRAMCLEAR
                            634 	.area GSFINAL (CODE)
   0983 02 02 03            635 	ljmp	__sdcc_program_startup
                            636 ;--------------------------------------------------------
                            637 ; Home
                            638 ;--------------------------------------------------------
                            639 	.area HOME    (CODE)
                            640 	.area CSEG    (CODE)
                            641 ;--------------------------------------------------------
                            642 ; code
                            643 ;--------------------------------------------------------
                            644 	.area CSEG    (CODE)
   0203                     645 __sdcc_program_startup:
   0203 12 08 13            646 	lcall	_main
                            647 ;	return from main will lock up
   0206 80 FE               648 	sjmp .
                            649 ;------------------------------------------------------------
                            650 ;Allocation info for local variables in function 'FX2_Delay1ms'
                            651 ;------------------------------------------------------------
                            652 ;------------------------------------------------------------
                            653 ;fx2_utils.h:37: void FX2_Delay1ms (void)
                            654 ;	-----------------------------------------
                            655 ;	 function FX2_Delay1ms
                            656 ;	-----------------------------------------
   0208                     657 _FX2_Delay1ms:
                    0002    658 	ar2 = 0x02
                    0003    659 	ar3 = 0x03
                    0004    660 	ar4 = 0x04
                    0005    661 	ar5 = 0x05
                    0006    662 	ar6 = 0x06
                    0007    663 	ar7 = 0x07
                    0000    664 	ar0 = 0x00
                    0001    665 	ar1 = 0x01
                            666 ;fx2_utils.h:55: _endasm;
                            667 ;     genInline
   0208 74 00               668 	        mov A, #0
   020A F5 86               669 	        mov _DPS,A
   020C 90 FD A5            670 	        mov DPTR,#(0xffff-602)
   020F 7C 05               671 	        mov R4,#5
   0211                     672         loop:
   0211 A3                  673 	        inc DPTR
   0212 E5 82               674 	        mov A,DPL
   0214 45 83               675 	        orl A,DPH
   0216 70 F9               676 	        jnz loop
   0218                     677         er_end:
   0218 22                  678 	        ret
   0219                     679 00101$:
   0219 22                  680 	ret
                            681 ;------------------------------------------------------------
                            682 ;Allocation info for local variables in function 'FX2_Delay'
                            683 ;------------------------------------------------------------
                            684 ;ms                        Allocated to registers r2 r3 
                            685 ;------------------------------------------------------------
                            686 ;fx2_utils.h:60: void FX2_Delay(WORD ms)
                            687 ;	-----------------------------------------
                            688 ;	 function FX2_Delay
                            689 ;	-----------------------------------------
   021A                     690 _FX2_Delay:
                            691 ;     genReceive
   021A AA 82               692 	mov	r2,dpl
   021C AB 83               693 	mov	r3,dph
                            694 ;fx2_utils.h:66: if ((CPUCS & bmCLKSPD) == 0)			  // 12Mhz
                            695 ;     genAssign
   021E 90 E6 00            696 	mov	dptr,#_CPUCS
   0221 E0                  697 	movx	a,@dptr
   0222 FC                  698 	mov	r4,a
                            699 ;     genAnd
   0223 53 04 18            700 	anl	ar4,#0x18
                            701 ;     genCmpEq
                            702 ;	Peephole 112.b	changed ljmp to sjmp
                            703 ;	Peephole 199	optimized misc jump sequence
   0226 BC 00 11            704 	cjne	r4,#0x00,00104$
                            705 ;00115$:
                            706 ;	Peephole 200	removed redundant sjmp
   0229                     707 00116$:
                            708 ;fx2_utils.h:67: ms = (ms + 1) / 2;					   // Round up before dividing so we can accept 1.
                            709 ;     genPlus
                            710 ;     genPlusIncr
   0229 74 01               711 	mov	a,#0x01
                            712 ;	Peephole 236.a	used r2 instead of ar2
   022B 2A                  713 	add	a,r2
   022C FC                  714 	mov	r4,a
                            715 ;	Peephole 181	changed mov to clr
   022D E4                  716 	clr	a
                            717 ;	Peephole 236.b	used r3 instead of ar3
   022E 3B                  718 	addc	a,r3
                            719 ;     genRightShift
                            720 ;     genRightShiftLiteral
                            721 ;     genrshTwo
                            722 ;	Peephole 177.d	removed redundant move
   022F FD                  723 	mov	r5,a
   0230 8C 02               724 	mov	ar2,r4
   0232 C3                  725 	clr	c
   0233 13                  726 	rrc	a
   0234 CA                  727 	xch	a,r2
   0235 13                  728 	rrc	a
   0236 CA                  729 	xch	a,r2
   0237 FB                  730 	mov	r3,a
                            731 ;	Peephole 112.b	changed ljmp to sjmp
   0238 80 12               732 	sjmp	00114$
   023A                     733 00104$:
                            734 ;fx2_utils.h:68: else if ((CPUCS & bmCLKSPD) == bmCLKSPD1)	// 48Mhz
                            735 ;     genAssign
   023A 90 E6 00            736 	mov	dptr,#_CPUCS
   023D E0                  737 	movx	a,@dptr
   023E FC                  738 	mov	r4,a
                            739 ;     genAnd
   023F 53 04 18            740 	anl	ar4,#0x18
                            741 ;     genCmpEq
                            742 ;	Peephole 112.b	changed ljmp to sjmp
                            743 ;	Peephole 199	optimized misc jump sequence
   0242 BC 10 07            744 	cjne	r4,#0x10,00114$
                            745 ;00117$:
                            746 ;	Peephole 200	removed redundant sjmp
   0245                     747 00118$:
                            748 ;fx2_utils.h:69: ms = ms * 2;
                            749 ;     genLeftShift
                            750 ;     genLeftShiftLiteral
                            751 ;     genlshTwo
   0245 EB                  752 	mov	a,r3
   0246 CA                  753 	xch	a,r2
   0247 25 E0               754 	add	a,acc
   0249 CA                  755 	xch	a,r2
   024A 33                  756 	rlc	a
   024B FB                  757 	mov	r3,a
                            758 ;fx2_utils.h:71: while (ms--)
   024C                     759 00114$:
                            760 ;     genAssign
   024C                     761 00106$:
                            762 ;     genAssign
   024C 8A 04               763 	mov	ar4,r2
   024E 8B 05               764 	mov	ar5,r3
                            765 ;     genMinus
                            766 ;     genMinusDec
   0250 1A                  767 	dec	r2
   0251 BA FF 01            768 	cjne	r2,#0xff,00119$
   0254 1B                  769 	dec	r3
   0255                     770 00119$:
                            771 ;     genIfx
   0255 EC                  772 	mov	a,r4
   0256 4D                  773 	orl	a,r5
                            774 ;     genIfxJump
                            775 ;	Peephole 110	removed ljmp by inverse jump logic
   0257 60 0D               776 	jz	00108$
   0259                     777 00120$:
                            778 ;fx2_utils.h:72: FX2_Delay1ms();
                            779 ;     genCall
   0259 C0 02               780 	push	ar2
   025B C0 03               781 	push	ar3
   025D 12 02 08            782 	lcall	_FX2_Delay1ms
   0260 D0 03               783 	pop	ar3
   0262 D0 02               784 	pop	ar2
                            785 ;	Peephole 112.b	changed ljmp to sjmp
   0264 80 E6               786 	sjmp	00106$
   0266                     787 00108$:
                            788 ;fx2_utils.h:74: return;
                            789 ;     genRet
   0266                     790 00109$:
   0266 22                  791 	ret
                            792 ;------------------------------------------------------------
                            793 ;Allocation info for local variables in function 'FX2_Discon'
                            794 ;------------------------------------------------------------
                            795 ;renum                     Allocated to registers r2 
                            796 ;------------------------------------------------------------
                            797 ;fx2_utils.h:80: void FX2_Discon(BOOL renum)
                            798 ;	-----------------------------------------
                            799 ;	 function FX2_Discon
                            800 ;	-----------------------------------------
   0267                     801 _FX2_Discon:
                            802 ;     genReceive
                            803 ;fx2_utils.h:82: if(renum)                                // If renumerate (i.e. 8051 will handle SETUP commands)
                            804 ;     genIfx
                            805 ;	peephole 177.g	optimized mov sequence
   0267 E5 82               806 	mov	a,dpl
   0269 FA                  807 	mov	r2,a
                            808 ;     genIfxJump
                            809 ;	Peephole 110	removed ljmp by inverse jump logic
   026A 60 0A               810 	jz	00102$
   026C                     811 00107$:
                            812 ;fx2_utils.h:83: FX2_USB_DISCONNECT_AND_RENUM();       // disconnect from USB and set the renumerate bit
                            813 ;     genAssign
                            814 ;     genOr
                            815 ;	Peephole 248.a	optimized or to xdata
   026C 90 E6 80            816 	mov	dptr,#_USBCS
   026F E0                  817 	movx	a,@dptr
   0270 FA                  818 	mov	r2,a
   0271 44 0A               819 	orl	a,#0x0A
   0273 F0                  820 	movx	@dptr,a
                            821 ;	Peephole 112.b	changed ljmp to sjmp
   0274 80 08               822 	sjmp	00103$
   0276                     823 00102$:
                            824 ;fx2_utils.h:85: FX2_USB_DISCONNECT();                 // just disconnect from USB
                            825 ;     genAssign
                            826 ;     genOr
                            827 ;	Peephole 248.a	optimized or to xdata
   0276 90 E6 80            828 	mov	dptr,#_USBCS
   0279 E0                  829 	movx	a,@dptr
   027A FA                  830 	mov	r2,a
   027B 44 08               831 	orl	a,#0x08
   027D F0                  832 	movx	@dptr,a
   027E                     833 00103$:
                            834 ;fx2_utils.h:87: FX2_Delay(1500);      					// Wait 1500 ms
                            835 ;     genCall
                            836 ;	Peephole 182.b	used 16 bit load of dptr
   027E 90 05 DC            837 	mov	dptr,#0x05DC
   0281 12 02 1A            838 	lcall	_FX2_Delay
                            839 ;fx2_utils.h:89: USBIRQ = 0xff;          					// Clear any pending USB interrupt requests.
                            840 ;     genAssign
   0284 90 E6 5D            841 	mov	dptr,#_USBIRQ
   0287 74 FF               842 	mov	a,#0xFF
   0289 F0                  843 	movx	@dptr,a
                            844 ;fx2_utils.h:90: EPIRQ = 0xff;
                            845 ;     genAssign
   028A 90 E6 5F            846 	mov	dptr,#_EPIRQ
   028D 74 FF               847 	mov	a,#0xFF
   028F F0                  848 	movx	@dptr,a
                            849 ;fx2_utils.h:92: FX2_IRQ_CLEAR();
                            850 ;     genAnd
   0290 53 91 EF            851 	anl	_EXIF,#0xEF
                            852 ;fx2_utils.h:93: FX2_USB_CONNECT();      					// reconnect USB
                            853 ;     genAssign
                            854 ;     genAnd
                            855 ;	Peephole 248.b	optimized and to xdata
   0293 90 E6 80            856 	mov	dptr,#_USBCS
   0296 E0                  857 	movx	a,@dptr
   0297 FA                  858 	mov	r2,a
   0298 54 F7               859 	anl	a,#0xF7
   029A F0                  860 	movx	@dptr,a
   029B                     861 00104$:
   029B 22                  862 	ret
                            863 ;------------------------------------------------------------
                            864 ;Allocation info for local variables in function 'init_fx2'
                            865 ;------------------------------------------------------------
                            866 ;------------------------------------------------------------
                            867 ;HPSDR_firmware_1_0_0.c:29: void init_fx2 (void)
                            868 ;	-----------------------------------------
                            869 ;	 function init_fx2
                            870 ;	-----------------------------------------
   029C                     871 _init_fx2:
                            872 ;HPSDR_firmware_1_0_0.c:31: FX2_SET_CLKSPD_48();			// CPU runs at 48 MHz
                            873 ;     genAssign
                            874 ;     genOr
                            875 ;	Peephole 248.a	optimized or to xdata
   029C 90 E6 00            876 	mov	dptr,#_CPUCS
   029F E0                  877 	movx	a,@dptr
   02A0 FA                  878 	mov	r2,a
   02A1 44 10               879 	orl	a,#0x10
   02A3 F0                  880 	movx	@dptr,a
                            881 ;HPSDR_firmware_1_0_0.c:32: FX2_SET_REVCTL();				// setup as per TRM recommendation
                            882 ;     genInline
   02A4 00                  883 	 nop; 
                            884 ;     genInline
   02A5 00                  885 	 nop; 
                            886 ;     genInline
   02A6 00                  887 	 nop; 
                            888 ;     genAssign
   02A7 90 E6 0B            889 	mov	dptr,#_REVCTL
   02AA 74 03               890 	mov	a,#0x03
   02AC F0                  891 	movx	@dptr,a
                            892 ;     genInline
   02AD 00                  893 	 nop; 
                            894 ;     genInline
   02AE 00                  895 	 nop; 
                            896 ;     genInline
   02AF 00                  897 	 nop; 
                            898 ;HPSDR_firmware_1_0_0.c:33: FX2_SPDAUTO_ENABLE();			// setup autopointer enabled
                            899 ;     genAssign
   02B0 90 E6 B5            900 	mov	dptr,#_SUDPTRCTL
   02B3 74 01               901 	mov	a,#0x01
   02B5 F0                  902 	movx	@dptr,a
                            903 ;HPSDR_firmware_1_0_0.c:34: FX2_RESET_EP1OUT_STALL(); 		// reset stall bit on EP1 OUT 
                            904 ;     genAssign
                            905 ;     genAnd
                            906 ;	Peephole 248.b	optimized and to xdata
   02B6 90 E6 A1            907 	mov	dptr,#_EP1OUTCS
   02B9 E0                  908 	movx	a,@dptr
   02BA FA                  909 	mov	r2,a
   02BB 54 FE               910 	anl	a,#0xFE
   02BD F0                  911 	movx	@dptr,a
                            912 ;HPSDR_firmware_1_0_0.c:35: FX2_ARM_EP1OUT();				// re-arm for EP1 out transfer
                            913 ;     genAssign
   02BE 90 E6 8D            914 	mov	dptr,#_EP1OUTBC
   02C1 74 40               915 	mov	a,#0x40
   02C3 F0                  916 	movx	@dptr,a
                            917 ;HPSDR_firmware_1_0_0.c:36: FX2_RESET_EP1IN_STALL();		// reset if EP1 is stalled
                            918 ;     genAssign
                            919 ;     genAnd
                            920 ;	Peephole 248.b	optimized and to xdata
   02C4 90 E6 A2            921 	mov	dptr,#_EP1INCS
   02C7 E0                  922 	movx	a,@dptr
   02C8 FA                  923 	mov	r2,a
   02C9 54 FE               924 	anl	a,#0xFE
   02CB F0                  925 	movx	@dptr,a
                            926 ;HPSDR_firmware_1_0_0.c:37: FX2_CLR_IFCONFIG();				// clear ifclock config
                            927 ;     genAssign
                            928 ;HPSDR_firmware_1_0_0.c:38: FX2_SET_IFCLKSRC_INT();			// set to internal ifclock source
                            929 ;     genAssign
                            930 ;	Peephole 180.a	removed redundant mov to dptr
   02CC 90 E6 01            931 	mov	dptr,#_IFCONFIG
                            932 ;	Peephole 181	changed mov to clr
   02CF E4                  933 	clr	a
   02D0 F0                  934 	movx	@dptr,a
   02D1 E0                  935 	movx	a,@dptr
   02D2 FA                  936 	mov	r2,a
                            937 ;     genOr
   02D3 90 E6 01            938 	mov	dptr,#_IFCONFIG
   02D6 74 80               939 	mov	a,#0x80
   02D8 4A                  940 	orl	a,r2
   02D9 F0                  941 	movx	@dptr,a
                            942 ;HPSDR_firmware_1_0_0.c:39: FX2_SET_FIFOGPIF_CLK48();		// set ifclock speed to 48 Mhz
                            943 ;     genAssign
                            944 ;     genOr
                            945 ;	Peephole 248.a	optimized or to xdata
   02DA 90 E6 01            946 	mov	dptr,#_IFCONFIG
   02DD E0                  947 	movx	a,@dptr
   02DE FA                  948 	mov	r2,a
   02DF 44 40               949 	orl	a,#0x40
   02E1 F0                  950 	movx	@dptr,a
   02E2                     951 00101$:
   02E2 22                  952 	ret
                            953 ;------------------------------------------------------------
                            954 ;Allocation info for local variables in function 'setup_io'
                            955 ;------------------------------------------------------------
                            956 ;------------------------------------------------------------
                            957 ;HPSDR_firmware_1_0_0.c:42: void setup_io (void)
                            958 ;	-----------------------------------------
                            959 ;	 function setup_io
                            960 ;	-----------------------------------------
   02E3                     961 _setup_io:
                            962 ;HPSDR_firmware_1_0_0.c:44: FX2_CLR_PORTACFG();				// port a all gp io
                            963 ;     genAssign
   02E3 90 E6 70            964 	mov	dptr,#_PORTACFG
                            965 ;	Peephole 181	changed mov to clr
                            966 ;HPSDR_firmware_1_0_0.c:45: FX2_CLR_PORTECFG();				// port e all gp io
                            967 ;     genAssign
                            968 ;	Peephole 181	changed mov to clr
                            969 ;	Peephole 219	removed redundant clear
   02E6 E4                  970 	clr	a
   02E7 F0                  971 	movx	@dptr,a
   02E8 90 E6 72            972 	mov	dptr,#_PORTECFG
   02EB F0                  973 	movx	@dptr,a
                            974 ;HPSDR_firmware_1_0_0.c:47: OEA=0x03;						// pins 1 and 2 output
                            975 ;     genAssign
   02EC 75 B2 03            976 	mov	_OEA,#0x03
                            977 ;HPSDR_firmware_1_0_0.c:48: IOA=0x03;
                            978 ;     genAssign
   02EF 75 80 03            979 	mov	_IOA,#0x03
                            980 ;HPSDR_firmware_1_0_0.c:50: OEE=0x02;						// pin 2 output
                            981 ;     genAssign
   02F2 75 B6 02            982 	mov	_OEE,#0x02
                            983 ;HPSDR_firmware_1_0_0.c:51: IOE=0x02;
                            984 ;     genAssign
   02F5 75 B1 02            985 	mov	_IOE,#0x02
   02F8                     986 00101$:
   02F8 22                  987 	ret
                            988 ;------------------------------------------------------------
                            989 ;Allocation info for local variables in function 'setup_interrupts'
                            990 ;------------------------------------------------------------
                            991 ;------------------------------------------------------------
                            992 ;HPSDR_firmware_1_0_0.c:54: void setup_interrupts (void)
                            993 ;	-----------------------------------------
                            994 ;	 function setup_interrupts
                            995 ;	-----------------------------------------
   02F9                     996 _setup_interrupts:
                            997 ;HPSDR_firmware_1_0_0.c:56: FX2_USB_INT_ENABLE();  								// enable USB interrupts
                            998 ;     genOr
   02F9 43 E8 01            999 	orl	_EIE,#0x01
                           1000 ;HPSDR_firmware_1_0_0.c:57: FX2_INT2_AV_ENABLE();  								// USB autovector enable
                           1001 ;     genAssign
                           1002 ;     genOr
                           1003 ;	Peephole 248.a	optimized or to xdata
   02FC 90 E6 68           1004 	mov	dptr,#_INTSETUP
   02FF E0                 1005 	movx	a,@dptr
   0300 FA                 1006 	mov	r2,a
   0301 44 08              1007 	orl	a,#0x08
   0303 F0                 1008 	movx	@dptr,a
                           1009 ;HPSDR_firmware_1_0_0.c:58: FX2_USB_INT_MASK(bmSUDAV | bmSUTOK | bmHSGRANT);	// enable SUDAV, SUTOK, and HSGRANT interrupts
                           1010 ;     genAssign
                           1011 ;     genOr
                           1012 ;	Peephole 248.a	optimized or to xdata
   0304 90 E6 5C           1013 	mov	dptr,#_USBIE
   0307 E0                 1014 	movx	a,@dptr
   0308 FA                 1015 	mov	r2,a
   0309 44 25              1016 	orl	a,#0x25
   030B F0                 1017 	movx	@dptr,a
                           1018 ;HPSDR_firmware_1_0_0.c:59: FX2_USB_EPIE_MASK(bmEP1OUT);						// enalbe EP1OUT interrupt
                           1019 ;     genAssign
                           1020 ;     genOr
                           1021 ;	Peephole 248.a	optimized or to xdata
   030C 90 E6 5E           1022 	mov	dptr,#_EPIE
   030F E0                 1023 	movx	a,@dptr
   0310 FA                 1024 	mov	r2,a
   0311 44 08              1025 	orl	a,#0x08
   0313 F0                 1026 	movx	@dptr,a
                           1027 ;HPSDR_firmware_1_0_0.c:60: FX2_8051_INT_ENABLE();								// enable 8051 interrupts
                           1028 ;     genOr
   0314 43 A8 80           1029 	orl	_IE,#0x80
   0317                    1030 00101$:
   0317 22                 1031 	ret
                           1032 ;------------------------------------------------------------
                           1033 ;Allocation info for local variables in function 'setup_descriptor_pointers'
                           1034 ;------------------------------------------------------------
                           1035 ;------------------------------------------------------------
                           1036 ;HPSDR_firmware_1_0_0.c:63: void setup_descriptor_pointers (void)
                           1037 ;	-----------------------------------------
                           1038 ;	 function setup_descriptor_pointers
                           1039 ;	-----------------------------------------
   0318                    1040 _setup_descriptor_pointers:
                           1041 ;HPSDR_firmware_1_0_0.c:65: pDeviceDscr = (WORD)&DeviceDscr;
                           1042 ;     genCast
   0318 90 22 08           1043 	mov	dptr,#_pDeviceDscr
   031B 74 00              1044 	mov	a,#_DeviceDscr
   031D F0                 1045 	movx	@dptr,a
   031E A3                 1046 	inc	dptr
   031F 74 E0              1047 	mov	a,#(_DeviceDscr >> 8)
   0321 F0                 1048 	movx	@dptr,a
                           1049 ;HPSDR_firmware_1_0_0.c:66: pDeviceQualDscr = (WORD)&DeviceQualDscr;
                           1050 ;     genCast
   0322 90 22 0A           1051 	mov	dptr,#_pDeviceQualDscr
   0325 74 12              1052 	mov	a,#_DeviceQualDscr
   0327 F0                 1053 	movx	@dptr,a
   0328 A3                 1054 	inc	dptr
   0329 74 E0              1055 	mov	a,#(_DeviceQualDscr >> 8)
   032B F0                 1056 	movx	@dptr,a
                           1057 ;HPSDR_firmware_1_0_0.c:67: pHighSpeedConfigDscr = (WORD)&HighSpeedConfigDscr;
                           1058 ;     genCast
   032C 7A 1C              1059 	mov	r2,#_HighSpeedConfigDscr
   032E 7B E0              1060 	mov	r3,#(_HighSpeedConfigDscr >> 8)
                           1061 ;     genAssign
   0330 90 22 0C           1062 	mov	dptr,#_pHighSpeedConfigDscr
   0333 EA                 1063 	mov	a,r2
   0334 F0                 1064 	movx	@dptr,a
   0335 A3                 1065 	inc	dptr
   0336 EB                 1066 	mov	a,r3
   0337 F0                 1067 	movx	@dptr,a
                           1068 ;HPSDR_firmware_1_0_0.c:68: pFullSpeedConfigDscr = (WORD)&FullSpeedConfigDscr;
                           1069 ;     genCast
   0338 7C 1C              1070 	mov	r4,#_FullSpeedConfigDscr
   033A 7D E0              1071 	mov	r5,#(_FullSpeedConfigDscr >> 8)
                           1072 ;     genAssign
   033C 90 22 0E           1073 	mov	dptr,#_pFullSpeedConfigDscr
   033F EC                 1074 	mov	a,r4
   0340 F0                 1075 	movx	@dptr,a
   0341 A3                 1076 	inc	dptr
   0342 ED                 1077 	mov	a,r5
   0343 F0                 1078 	movx	@dptr,a
                           1079 ;HPSDR_firmware_1_0_0.c:69: pStringDscr = (WORD)&StringDscr0;
                           1080 ;     genCast
   0344 90 22 14           1081 	mov	dptr,#_pStringDscr
   0347 74 3C              1082 	mov	a,#_StringDscr0
   0349 F0                 1083 	movx	@dptr,a
   034A A3                 1084 	inc	dptr
   034B 74 E0              1085 	mov	a,#(_StringDscr0 >> 8)
   034D F0                 1086 	movx	@dptr,a
                           1087 ;HPSDR_firmware_1_0_0.c:70: pConfigDscr = pHighSpeedConfigDscr;
                           1088 ;     genAssign
   034E 90 22 10           1089 	mov	dptr,#_pConfigDscr
   0351 EA                 1090 	mov	a,r2
   0352 F0                 1091 	movx	@dptr,a
   0353 A3                 1092 	inc	dptr
   0354 EB                 1093 	mov	a,r3
   0355 F0                 1094 	movx	@dptr,a
                           1095 ;HPSDR_firmware_1_0_0.c:71: pOtherConfigDscr = pFullSpeedConfigDscr;
                           1096 ;     genAssign
   0356 90 22 12           1097 	mov	dptr,#_pOtherConfigDscr
   0359 EC                 1098 	mov	a,r4
   035A F0                 1099 	movx	@dptr,a
   035B A3                 1100 	inc	dptr
   035C ED                 1101 	mov	a,r5
   035D F0                 1102 	movx	@dptr,a
   035E                    1103 00101$:
   035E 22                 1104 	ret
                           1105 ;------------------------------------------------------------
                           1106 ;Allocation info for local variables in function 'clear_ext_ram'
                           1107 ;------------------------------------------------------------
                           1108 ;i                         Allocated to registers r2 r3 
                           1109 ;------------------------------------------------------------
                           1110 ;HPSDR_firmware_1_0_0.c:74: void clear_ext_ram(void)
                           1111 ;	-----------------------------------------
                           1112 ;	 function clear_ext_ram
                           1113 ;	-----------------------------------------
   035F                    1114 _clear_ext_ram:
                           1115 ;HPSDR_firmware_1_0_0.c:78: pExtMemory = (WORD xdata *)0x4000;
                           1116 ;     genAssign
   035F 75 08 00           1117 	mov	_pExtMemory,#0x00
   0362 75 09 40           1118 	mov	(_pExtMemory + 1),#0x40
                           1119 ;HPSDR_firmware_1_0_0.c:80: for (i=0; i < 0x1000; i++)
                           1120 ;     genAssign
   0365 7A 00              1121 	mov	r2,#0x00
   0367 7B 10              1122 	mov	r3,#0x10
   0369                    1123 00103$:
                           1124 ;HPSDR_firmware_1_0_0.c:82: *(pExtMemory++) = 0xFFFF; 
                           1125 ;     genAssign
   0369 85 08 82           1126 	mov	dpl,_pExtMemory
   036C 85 09 83           1127 	mov	dph,(_pExtMemory + 1)
                           1128 ;     genPointerSet
                           1129 ;     genFarPointerSet
                           1130 ;	Peephole 101	removed redundant mov
   036F 74 FF              1131 	mov	a,#0xFF
   0371 F0                 1132 	movx	@dptr,a
   0372 A3                 1133 	inc	dptr
   0373 F0                 1134 	movx	@dptr,a
                           1135 ;     genPlus
                           1136 ;     genPlusIncr
   0374 74 02              1137 	mov	a,#0x02
   0376 25 08              1138 	add	a,_pExtMemory
   0378 F5 08              1139 	mov	_pExtMemory,a
                           1140 ;	Peephole 181	changed mov to clr
   037A E4                 1141 	clr	a
   037B 35 09              1142 	addc	a,(_pExtMemory + 1)
   037D F5 09              1143 	mov	(_pExtMemory + 1),a
                           1144 ;     genDjnz
                           1145 ;     genMinus
                           1146 ;     genMinusDec
   037F 1A                 1147 	dec	r2
   0380 BA FF 01           1148 	cjne	r2,#0xff,00108$
   0383 1B                 1149 	dec	r3
   0384                    1150 00108$:
                           1151 ;HPSDR_firmware_1_0_0.c:80: for (i=0; i < 0x1000; i++)
                           1152 ;     genIfx
   0384 EA                 1153 	mov	a,r2
   0385 4B                 1154 	orl	a,r3
                           1155 ;     genIfxJump
                           1156 ;	Peephole 109	removed ljmp by inverse jump logic
   0386 70 E1              1157 	jnz	00103$
   0388                    1158 00109$:
   0388                    1159 00104$:
   0388 22                 1160 	ret
                           1161 ;------------------------------------------------------------
                           1162 ;Allocation info for local variables in function 'process_ep1_data'
                           1163 ;------------------------------------------------------------
                           1164 ;i                         Allocated with name '_process_ep1_data_i_1_1'
                           1165 ;------------------------------------------------------------
                           1166 ;HPSDR_firmware_1_0_0.c:87: void process_ep1_data(void)
                           1167 ;	-----------------------------------------
                           1168 ;	 function process_ep1_data
                           1169 ;	-----------------------------------------
   0389                    1170 _process_ep1_data:
                           1171 ;HPSDR_firmware_1_0_0.c:91: if (!(EP1INCS & bmEPBUSY)) 		// can we send on EP1 ?
                           1172 ;     genAssign
   0389 90 E6 A2           1173 	mov	dptr,#_EP1INCS
   038C E0                 1174 	movx	a,@dptr
                           1175 ;     genAnd
                           1176 ;	Peephole 105	removed redundant mov
   038D FA                 1177 	mov	r2,a
                           1178 ;     genIfxJump
   038E 30 E1 03           1179 	jnb	acc.1,00133$
   0391 02 04 6A           1180 	ljmp	00113$
   0394                    1181 00133$:
                           1182 ;HPSDR_firmware_1_0_0.c:93: if (EP1OUTBUF[0] == 1)
                           1183 ;     genPointerGet
                           1184 ;     genFarPointerGet
   0394 90 E7 80           1185 	mov	dptr,#_EP1OUTBUF
   0397 E0                 1186 	movx	a,@dptr
   0398 FA                 1187 	mov	r2,a
                           1188 ;     genCmpEq
                           1189 ;	Peephole 112.b	changed ljmp to sjmp
                           1190 ;	Peephole 199	optimized misc jump sequence
   0399 BA 01 14           1191 	cjne	r2,#0x01,00110$
                           1192 ;00134$:
                           1193 ;	Peephole 200	removed redundant sjmp
   039C                    1194 00135$:
                           1195 ;HPSDR_firmware_1_0_0.c:95: if (EP1OUTBUF[1] == 1)
                           1196 ;     genPointerGet
                           1197 ;     genFarPointerGet
   039C 90 E7 81           1198 	mov	dptr,#(_EP1OUTBUF + 0x0001)
   039F E0                 1199 	movx	a,@dptr
   03A0 FA                 1200 	mov	r2,a
                           1201 ;     genCmpEq
                           1202 ;	Peephole 112.b	changed ljmp to sjmp
                           1203 ;	Peephole 199	optimized misc jump sequence
   03A1 BA 01 06           1204 	cjne	r2,#0x01,00102$
                           1205 ;00136$:
                           1206 ;	Peephole 200	removed redundant sjmp
   03A4                    1207 00137$:
                           1208 ;HPSDR_firmware_1_0_0.c:97: IOA = 0x03;
                           1209 ;     genAssign
   03A4 75 80 03           1210 	mov	_IOA,#0x03
   03A7 02 04 6A           1211 	ljmp	00113$
   03AA                    1212 00102$:
                           1213 ;HPSDR_firmware_1_0_0.c:101: IOA = 0x02;
                           1214 ;     genAssign
   03AA 75 80 02           1215 	mov	_IOA,#0x02
   03AD 02 04 6A           1216 	ljmp	00113$
   03B0                    1217 00110$:
                           1218 ;HPSDR_firmware_1_0_0.c:104: else if (EP1OUTBUF[0] == 2)
                           1219 ;     genPointerGet
                           1220 ;     genFarPointerGet
   03B0 90 E7 80           1221 	mov	dptr,#_EP1OUTBUF
   03B3 E0                 1222 	movx	a,@dptr
   03B4 FA                 1223 	mov	r2,a
                           1224 ;     genCmpEq
                           1225 ;	Peephole 112.b	changed ljmp to sjmp
                           1226 ;	Peephole 199	optimized misc jump sequence
   03B5 BA 02 5D           1227 	cjne	r2,#0x02,00107$
                           1228 ;00138$:
                           1229 ;	Peephole 200	removed redundant sjmp
   03B8                    1230 00139$:
                           1231 ;HPSDR_firmware_1_0_0.c:106: pExtMemory = (WORD xdata *)(EP1OUTBUF[1] + (EP1OUTBUF[2]<<8));
                           1232 ;     genPointerGet
                           1233 ;     genFarPointerGet
   03B8 90 E7 81           1234 	mov	dptr,#(_EP1OUTBUF + 0x0001)
   03BB E0                 1235 	movx	a,@dptr
   03BC FA                 1236 	mov	r2,a
                           1237 ;     genCast
   03BD 7B 00              1238 	mov	r3,#0x00
                           1239 ;     genPointerGet
                           1240 ;     genFarPointerGet
   03BF 90 E7 82           1241 	mov	dptr,#(_EP1OUTBUF + 0x0002)
   03C2 E0                 1242 	movx	a,@dptr
   03C3 FC                 1243 	mov	r4,a
                           1244 ;     genCast
                           1245 ;     genLeftShift
                           1246 ;     genLeftShiftLiteral
                           1247 ;     genlshTwo
                           1248 ;	peephole 177.e	removed redundant move
   03C4 8C 05              1249 	mov	ar5,r4
                           1250 ;     genPlus
                           1251 ;	Peephole 236.g	used r4 instead of ar4
                           1252 ;	peephole 177.g	optimized mov sequence
                           1253 ;	Peephole 181	changed mov to clr
   03C6 E4                 1254 	clr	a
   03C7 FC                 1255 	mov	r4,a
                           1256 ;	Peephole 236.a	used r2 instead of ar2
   03C8 2A                 1257 	add	a,r2
   03C9 FA                 1258 	mov	r2,a
                           1259 ;	Peephole 236.g	used r5 instead of ar5
   03CA ED                 1260 	mov	a,r5
                           1261 ;	Peephole 236.b	used r3 instead of ar3
   03CB 3B                 1262 	addc	a,r3
   03CC FB                 1263 	mov	r3,a
                           1264 ;     genCast
   03CD 8A 08              1265 	mov	_pExtMemory,r2
   03CF 8B 09              1266 	mov	(_pExtMemory + 1),r3
                           1267 ;HPSDR_firmware_1_0_0.c:107: pExtByteMemory = (BYTE xdata *)pExtMemory;
                           1268 ;     genAssign
   03D1 85 08 0A           1269 	mov	_pExtByteMemory,_pExtMemory
   03D4 85 09 0B           1270 	mov	(_pExtByteMemory + 1),(_pExtMemory + 1)
                           1271 ;HPSDR_firmware_1_0_0.c:109: for (i=0; i < EP1OUTBUF[3]; i++)
                           1272 ;     genAssign
   03D7 7A 00              1273 	mov	r2,#0x00
   03D9                    1274 00114$:
                           1275 ;     genPointerGet
                           1276 ;     genFarPointerGet
   03D9 90 E7 83           1277 	mov	dptr,#(_EP1OUTBUF + 0x0003)
   03DC E0                 1278 	movx	a,@dptr
   03DD FB                 1279 	mov	r3,a
                           1280 ;     genCmpLt
                           1281 ;     genCmp
   03DE C3                 1282 	clr	c
   03DF EA                 1283 	mov	a,r2
   03E0 9B                 1284 	subb	a,r3
                           1285 ;     genIfxJump
                           1286 ;	Peephole 108	removed ljmp by inverse jump logic
   03E1 50 21              1287 	jnc	00117$
   03E3                    1288 00140$:
                           1289 ;HPSDR_firmware_1_0_0.c:111: EP1INBUF[i] = *(pExtByteMemory++); 
                           1290 ;     genPlus
                           1291 ;	Peephole 236.g	used r2 instead of ar2
   03E3 EA                 1292 	mov	a,r2
   03E4 24 C0              1293 	add	a,#_EP1INBUF
   03E6 FB                 1294 	mov	r3,a
                           1295 ;	Peephole 181	changed mov to clr
   03E7 E4                 1296 	clr	a
   03E8 34 E7              1297 	addc	a,#(_EP1INBUF >> 8)
   03EA FC                 1298 	mov	r4,a
                           1299 ;     genAssign
   03EB 85 0A 82           1300 	mov	dpl,_pExtByteMemory
   03EE 85 0B 83           1301 	mov	dph,(_pExtByteMemory + 1)
                           1302 ;     genPointerGet
                           1303 ;     genFarPointerGet
   03F1 E0                 1304 	movx	a,@dptr
   03F2 FD                 1305 	mov	r5,a
                           1306 ;     genPlus
                           1307 ;     genPlusIncr
   03F3 05 0A              1308 	inc	_pExtByteMemory
   03F5 E4                 1309 	clr	a
   03F6 B5 0A 02           1310 	cjne	a,_pExtByteMemory,00141$
   03F9 05 0B              1311 	inc	(_pExtByteMemory + 1)
   03FB                    1312 00141$:
                           1313 ;     genPointerSet
                           1314 ;     genFarPointerSet
   03FB 8B 82              1315 	mov	dpl,r3
   03FD 8C 83              1316 	mov	dph,r4
   03FF ED                 1317 	mov	a,r5
   0400 F0                 1318 	movx	@dptr,a
                           1319 ;HPSDR_firmware_1_0_0.c:109: for (i=0; i < EP1OUTBUF[3]; i++)
                           1320 ;     genPlus
                           1321 ;     genPlusIncr
   0401 0A                 1322 	inc	r2
                           1323 ;	Peephole 112.b	changed ljmp to sjmp
   0402 80 D5              1324 	sjmp	00114$
   0404                    1325 00117$:
                           1326 ;HPSDR_firmware_1_0_0.c:113: FX2_RESET_EP1IN_STALL();
                           1327 ;     genAssign
                           1328 ;     genAnd
                           1329 ;	Peephole 248.b	optimized and to xdata
   0404 90 E6 A2           1330 	mov	dptr,#_EP1INCS
   0407 E0                 1331 	movx	a,@dptr
   0408 FB                 1332 	mov	r3,a
   0409 54 FE              1333 	anl	a,#0xFE
   040B F0                 1334 	movx	@dptr,a
                           1335 ;HPSDR_firmware_1_0_0.c:114: EP1INBC = i + 1; 			// bytes to transfer, initates send on EP1 IN
                           1336 ;     genPlus
   040C 90 E6 8F           1337 	mov	dptr,#_EP1INBC
                           1338 ;     genPlusIncr
   040F 74 01              1339 	mov	a,#0x01
                           1340 ;	Peephole 236.a	used r2 instead of ar2
   0411 2A                 1341 	add	a,r2
   0412 F0                 1342 	movx	@dptr,a
                           1343 ;	Peephole 112.b	changed ljmp to sjmp
   0413 80 55              1344 	sjmp	00113$
   0415                    1345 00107$:
                           1346 ;HPSDR_firmware_1_0_0.c:116: else if (EP1OUTBUF[0] == 3)
                           1347 ;     genPointerGet
                           1348 ;     genFarPointerGet
   0415 90 E7 80           1349 	mov	dptr,#_EP1OUTBUF
   0418 E0                 1350 	movx	a,@dptr
   0419 FA                 1351 	mov	r2,a
                           1352 ;     genCmpEq
                           1353 ;	Peephole 112.b	changed ljmp to sjmp
                           1354 ;	Peephole 199	optimized misc jump sequence
   041A BA 03 4D           1355 	cjne	r2,#0x03,00113$
                           1356 ;00142$:
                           1357 ;	Peephole 200	removed redundant sjmp
   041D                    1358 00143$:
                           1359 ;HPSDR_firmware_1_0_0.c:118: pExtMemory = (WORD xdata *)(EP1OUTBUF[1] + (EP1OUTBUF[2]<<8));
                           1360 ;     genPointerGet
                           1361 ;     genFarPointerGet
   041D 90 E7 81           1362 	mov	dptr,#(_EP1OUTBUF + 0x0001)
   0420 E0                 1363 	movx	a,@dptr
   0421 FA                 1364 	mov	r2,a
                           1365 ;     genCast
   0422 7B 00              1366 	mov	r3,#0x00
                           1367 ;     genPointerGet
                           1368 ;     genFarPointerGet
   0424 90 E7 82           1369 	mov	dptr,#(_EP1OUTBUF + 0x0002)
   0427 E0                 1370 	movx	a,@dptr
   0428 FC                 1371 	mov	r4,a
                           1372 ;     genCast
                           1373 ;     genLeftShift
                           1374 ;     genLeftShiftLiteral
                           1375 ;     genlshTwo
                           1376 ;	peephole 177.e	removed redundant move
   0429 8C 05              1377 	mov	ar5,r4
                           1378 ;     genPlus
                           1379 ;	Peephole 236.g	used r4 instead of ar4
                           1380 ;	peephole 177.g	optimized mov sequence
                           1381 ;	Peephole 181	changed mov to clr
   042B E4                 1382 	clr	a
   042C FC                 1383 	mov	r4,a
                           1384 ;	Peephole 236.a	used r2 instead of ar2
   042D 2A                 1385 	add	a,r2
   042E FA                 1386 	mov	r2,a
                           1387 ;	Peephole 236.g	used r5 instead of ar5
   042F ED                 1388 	mov	a,r5
                           1389 ;	Peephole 236.b	used r3 instead of ar3
   0430 3B                 1390 	addc	a,r3
   0431 FB                 1391 	mov	r3,a
                           1392 ;     genCast
   0432 8A 08              1393 	mov	_pExtMemory,r2
   0434 8B 09              1394 	mov	(_pExtMemory + 1),r3
                           1395 ;HPSDR_firmware_1_0_0.c:119: pExtByteMemory = (BYTE xdata *)pExtMemory;
                           1396 ;     genAssign
   0436 85 08 0A           1397 	mov	_pExtByteMemory,_pExtMemory
   0439 85 09 0B           1398 	mov	(_pExtByteMemory + 1),(_pExtMemory + 1)
                           1399 ;HPSDR_firmware_1_0_0.c:121: for (i=0; i < EP1OUTBUF[3]; i++)
                           1400 ;     genAssign
   043C 7A 00              1401 	mov	r2,#0x00
   043E                    1402 00118$:
                           1403 ;     genPointerGet
                           1404 ;     genFarPointerGet
   043E 90 E7 83           1405 	mov	dptr,#(_EP1OUTBUF + 0x0003)
   0441 E0                 1406 	movx	a,@dptr
   0442 FB                 1407 	mov	r3,a
                           1408 ;     genCmpLt
                           1409 ;     genCmp
   0443 C3                 1410 	clr	c
   0444 EA                 1411 	mov	a,r2
   0445 9B                 1412 	subb	a,r3
                           1413 ;     genIfxJump
                           1414 ;	Peephole 108	removed ljmp by inverse jump logic
   0446 50 22              1415 	jnc	00113$
   0448                    1416 00144$:
                           1417 ;HPSDR_firmware_1_0_0.c:123: *(pExtByteMemory++) = (BYTE)EP1OUTBUF[i+4]; 
                           1418 ;     genAssign
   0448 AB 0A              1419 	mov	r3,_pExtByteMemory
   044A AC 0B              1420 	mov	r4,(_pExtByteMemory + 1)
                           1421 ;     genPlus
                           1422 ;     genPlusIncr
   044C 74 04              1423 	mov	a,#0x04
                           1424 ;	Peephole 236.a	used r2 instead of ar2
   044E 2A                 1425 	add	a,r2
                           1426 ;     genPlus
   044F 24 80              1427 	add	a,#_EP1OUTBUF
   0451 F5 82              1428 	mov	dpl,a
                           1429 ;	Peephole 240	use clr instead of addc a,#0
   0453 E4                 1430 	clr	a
   0454 34 E7              1431 	addc	a,#(_EP1OUTBUF >> 8)
   0456 F5 83              1432 	mov	dph,a
                           1433 ;     genPointerGet
                           1434 ;     genFarPointerGet
   0458 E0                 1435 	movx	a,@dptr
                           1436 ;     genPointerSet
                           1437 ;     genFarPointerSet
                           1438 ;	Peephole 136	removed redundant moves
   0459 FD                 1439 	mov	r5,a
   045A 8B 82              1440 	mov	dpl,r3
   045C 8C 83              1441 	mov	dph,r4
   045E F0                 1442 	movx	@dptr,a
                           1443 ;     genPlus
                           1444 ;     genPlusIncr
   045F 05 0A              1445 	inc	_pExtByteMemory
   0461 E4                 1446 	clr	a
   0462 B5 0A 02           1447 	cjne	a,_pExtByteMemory,00145$
   0465 05 0B              1448 	inc	(_pExtByteMemory + 1)
   0467                    1449 00145$:
                           1450 ;HPSDR_firmware_1_0_0.c:121: for (i=0; i < EP1OUTBUF[3]; i++)
                           1451 ;     genPlus
                           1452 ;     genPlusIncr
   0467 0A                 1453 	inc	r2
                           1454 ;	Peephole 112.b	changed ljmp to sjmp
   0468 80 D4              1455 	sjmp	00118$
   046A                    1456 00113$:
                           1457 ;HPSDR_firmware_1_0_0.c:128: FX2_RESET_EP1OUT_STALL();	; 	//reset stall bit on EP1 OUT
                           1458 ;     genAssign
                           1459 ;     genAnd
                           1460 ;	Peephole 248.b	optimized and to xdata
   046A 90 E6 A1           1461 	mov	dptr,#_EP1OUTCS
   046D E0                 1462 	movx	a,@dptr
   046E FA                 1463 	mov	r2,a
   046F 54 FE              1464 	anl	a,#0xFE
   0471 F0                 1465 	movx	@dptr,a
                           1466 ;HPSDR_firmware_1_0_0.c:129: FX2_ARM_EP1OUT();				//re-arm for EP1 out transfer
                           1467 ;     genAssign
   0472 90 E6 8D           1468 	mov	dptr,#_EP1OUTBC
   0475 74 40              1469 	mov	a,#0x40
   0477 F0                 1470 	movx	@dptr,a
   0478                    1471 00122$:
   0478 22                 1472 	ret
                           1473 ;------------------------------------------------------------
                           1474 ;Allocation info for local variables in function 'process_ep0_data'
                           1475 ;------------------------------------------------------------
                           1476 ;dscr                      Allocated to registers r4 r5 
                           1477 ;found                     Allocated to registers r2 
                           1478 ;index                     Allocated to registers r3 
                           1479 ;------------------------------------------------------------
                           1480 ;HPSDR_firmware_1_0_0.c:133: void process_ep0_data(void)
                           1481 ;	-----------------------------------------
                           1482 ;	 function process_ep0_data
                           1483 ;	-----------------------------------------
   0479                    1484 _process_ep0_data:
                           1485 ;HPSDR_firmware_1_0_0.c:139: switch(SETUPDAT[1])
                           1486 ;     genPointerGet
                           1487 ;     genFarPointerGet
   0479 90 E6 B9           1488 	mov	dptr,#(_SETUPDAT + 0x0001)
   047C E0                 1489 	movx	a,@dptr
                           1490 ;     genCmpGt
                           1491 ;     genCmp
                           1492 ;     genIfxJump
                           1493 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
                           1494 ;	Peephole 177.a	removed redundant mov
   047D FA                 1495 	mov  r2,a
   047E 24 F4              1496 	add	a,#0xff - 0x0B
   0480 50 03              1497 	jnc	00171$
   0482 02 07 E1           1498 	ljmp	00146$
   0485                    1499 00171$:
                           1500 ;     genJumpTab
                           1501 ;	Peephole 254	optimized left shift
   0485 EA                 1502 	mov	a,r2
   0486 2A                 1503 	add	a,r2
   0487 2A                 1504 	add	a,r2
   0488 90 04 8C           1505 	mov	dptr,#00172$
   048B 73                 1506 	jmp	@a+dptr
   048C                    1507 00172$:
   048C 02 05 F0           1508 	ljmp	00120$
   048F 02 06 93           1509 	ljmp	00126$
   0492 02 07 E1           1510 	ljmp	00146$
   0495 02 07 51           1511 	ljmp	00136$
   0498 02 07 E1           1512 	ljmp	00146$
   049B 02 07 E1           1513 	ljmp	00146$
   049E 02 04 B0           1514 	ljmp	00101$
   04A1 02 07 E1           1515 	ljmp	00146$
   04A4 02 05 D9           1516 	ljmp	00119$
   04A7 02 05 CD           1517 	ljmp	00118$
   04AA 02 05 AA           1518 	ljmp	00116$
   04AD 02 05 C1           1519 	ljmp	00117$
                           1520 ;HPSDR_firmware_1_0_0.c:141: case SC_GET_DESCRIPTOR:                  // *** Get Descriptor
   04B0                    1521 00101$:
                           1522 ;HPSDR_firmware_1_0_0.c:143: switch(SETUPDAT[3])         
                           1523 ;     genPointerGet
                           1524 ;     genFarPointerGet
   04B0 90 E6 BB           1525 	mov	dptr,#(_SETUPDAT + 0x0003)
   04B3 E0                 1526 	movx	a,@dptr
   04B4 FA                 1527 	mov	r2,a
                           1528 ;     genCmpEq
   04B5 BA 01 02           1529 	cjne	r2,#0x01,00173$
                           1530 ;	Peephole 112.b	changed ljmp to sjmp
   04B8 80 17              1531 	sjmp	00102$
   04BA                    1532 00173$:
                           1533 ;     genCmpEq
   04BA BA 02 02           1534 	cjne	r2,#0x02,00174$
                           1535 ;	Peephole 112.b	changed ljmp to sjmp
   04BD 80 48              1536 	sjmp	00104$
   04BF                    1537 00174$:
                           1538 ;     genCmpEq
   04BF BA 03 02           1539 	cjne	r2,#0x03,00175$
                           1540 ;	Peephole 112.b	changed ljmp to sjmp
   04C2 80 79              1541 	sjmp	00106$
   04C4                    1542 00175$:
                           1543 ;     genCmpEq
   04C4 BA 06 02           1544 	cjne	r2,#0x06,00176$
                           1545 ;	Peephole 112.b	changed ljmp to sjmp
   04C7 80 23              1546 	sjmp	00103$
   04C9                    1547 00176$:
                           1548 ;     genCmpEq
   04C9 BA 07 02           1549 	cjne	r2,#0x07,00177$
                           1550 ;	Peephole 112.b	changed ljmp to sjmp
   04CC 80 54              1551 	sjmp	00105$
   04CE                    1552 00177$:
   04CE 02 05 9F           1553 	ljmp	00114$
                           1554 ;HPSDR_firmware_1_0_0.c:145: case GD_DEVICE:            			// Device
   04D1                    1555 00102$:
                           1556 ;HPSDR_firmware_1_0_0.c:146: SUDPTRH = MSB(pDeviceDscr);
                           1557 ;     genAssign
   04D1 90 22 08           1558 	mov	dptr,#_pDeviceDscr
   04D4 E0                 1559 	movx	a,@dptr
   04D5 FA                 1560 	mov	r2,a
   04D6 A3                 1561 	inc	dptr
   04D7 E0                 1562 	movx	a,@dptr
   04D8 FB                 1563 	mov	r3,a
                           1564 ;     genRightShift
                           1565 ;     genRightShiftLiteral
                           1566 ;     genrshTwo
   04D9 8B 04              1567 	mov	ar4,r3
                           1568 ;     genAnd
                           1569 ;	Peephole 177.c	removed redundant move
   04DB 7D 00              1570 	mov	r5,#0x00
                           1571 ;     genCast
   04DD 90 E6 B3           1572 	mov	dptr,#_SUDPTRH
   04E0 EC                 1573 	mov	a,r4
   04E1 F0                 1574 	movx	@dptr,a
                           1575 ;HPSDR_firmware_1_0_0.c:147: SUDPTRL = LSB(pDeviceDscr);
                           1576 ;     genAnd
   04E2 7B 00              1577 	mov	r3,#0x00
                           1578 ;     genCast
   04E4 90 E6 B4           1579 	mov	dptr,#_SUDPTRL
   04E7 EA                 1580 	mov	a,r2
   04E8 F0                 1581 	movx	@dptr,a
                           1582 ;HPSDR_firmware_1_0_0.c:148: break;
   04E9 02 07 E9           1583 	ljmp	00147$
                           1584 ;HPSDR_firmware_1_0_0.c:149: case GD_DEVICE_QUALIFIER:            // Device Qualifier
   04EC                    1585 00103$:
                           1586 ;HPSDR_firmware_1_0_0.c:150: SUDPTRH = MSB(pDeviceQualDscr);
                           1587 ;     genAssign
   04EC 90 22 0A           1588 	mov	dptr,#_pDeviceQualDscr
   04EF E0                 1589 	movx	a,@dptr
   04F0 FA                 1590 	mov	r2,a
   04F1 A3                 1591 	inc	dptr
   04F2 E0                 1592 	movx	a,@dptr
   04F3 FB                 1593 	mov	r3,a
                           1594 ;     genRightShift
                           1595 ;     genRightShiftLiteral
                           1596 ;     genrshTwo
   04F4 8B 04              1597 	mov	ar4,r3
                           1598 ;     genAnd
                           1599 ;	Peephole 177.c	removed redundant move
   04F6 7D 00              1600 	mov	r5,#0x00
                           1601 ;     genCast
   04F8 90 E6 B3           1602 	mov	dptr,#_SUDPTRH
   04FB EC                 1603 	mov	a,r4
   04FC F0                 1604 	movx	@dptr,a
                           1605 ;HPSDR_firmware_1_0_0.c:151: SUDPTRL = LSB(pDeviceQualDscr);
                           1606 ;     genAnd
   04FD 7B 00              1607 	mov	r3,#0x00
                           1608 ;     genCast
   04FF 90 E6 B4           1609 	mov	dptr,#_SUDPTRL
   0502 EA                 1610 	mov	a,r2
   0503 F0                 1611 	movx	@dptr,a
                           1612 ;HPSDR_firmware_1_0_0.c:152: break;
   0504 02 07 E9           1613 	ljmp	00147$
                           1614 ;HPSDR_firmware_1_0_0.c:153: case GD_CONFIGURATION:         		// Configuration
   0507                    1615 00104$:
                           1616 ;HPSDR_firmware_1_0_0.c:154: SUDPTRH = MSB(pConfigDscr);
                           1617 ;     genAssign
   0507 90 22 10           1618 	mov	dptr,#_pConfigDscr
   050A E0                 1619 	movx	a,@dptr
   050B FA                 1620 	mov	r2,a
   050C A3                 1621 	inc	dptr
   050D E0                 1622 	movx	a,@dptr
   050E FB                 1623 	mov	r3,a
                           1624 ;     genRightShift
                           1625 ;     genRightShiftLiteral
                           1626 ;     genrshTwo
   050F 8B 04              1627 	mov	ar4,r3
                           1628 ;     genAnd
                           1629 ;	Peephole 177.c	removed redundant move
   0511 7D 00              1630 	mov	r5,#0x00
                           1631 ;     genCast
   0513 90 E6 B3           1632 	mov	dptr,#_SUDPTRH
   0516 EC                 1633 	mov	a,r4
   0517 F0                 1634 	movx	@dptr,a
                           1635 ;HPSDR_firmware_1_0_0.c:155: SUDPTRL = LSB(pConfigDscr);
                           1636 ;     genAnd
   0518 7B 00              1637 	mov	r3,#0x00
                           1638 ;     genCast
   051A 90 E6 B4           1639 	mov	dptr,#_SUDPTRL
   051D EA                 1640 	mov	a,r2
   051E F0                 1641 	movx	@dptr,a
                           1642 ;HPSDR_firmware_1_0_0.c:156: break;
   051F 02 07 E9           1643 	ljmp	00147$
                           1644 ;HPSDR_firmware_1_0_0.c:157: case GD_OTHER_SPEED_CONFIGURATION:  	// Other Speed Configuration
   0522                    1645 00105$:
                           1646 ;HPSDR_firmware_1_0_0.c:158: SUDPTRH = MSB(pOtherConfigDscr);
                           1647 ;     genAssign
   0522 90 22 12           1648 	mov	dptr,#_pOtherConfigDscr
   0525 E0                 1649 	movx	a,@dptr
   0526 FA                 1650 	mov	r2,a
   0527 A3                 1651 	inc	dptr
   0528 E0                 1652 	movx	a,@dptr
   0529 FB                 1653 	mov	r3,a
                           1654 ;     genRightShift
                           1655 ;     genRightShiftLiteral
                           1656 ;     genrshTwo
   052A 8B 04              1657 	mov	ar4,r3
                           1658 ;     genAnd
                           1659 ;	Peephole 177.c	removed redundant move
   052C 7D 00              1660 	mov	r5,#0x00
                           1661 ;     genCast
   052E 90 E6 B3           1662 	mov	dptr,#_SUDPTRH
   0531 EC                 1663 	mov	a,r4
   0532 F0                 1664 	movx	@dptr,a
                           1665 ;HPSDR_firmware_1_0_0.c:159: SUDPTRL = LSB(pOtherConfigDscr);
                           1666 ;     genAnd
   0533 7B 00              1667 	mov	r3,#0x00
                           1668 ;     genCast
   0535 90 E6 B4           1669 	mov	dptr,#_SUDPTRL
   0538 EA                 1670 	mov	a,r2
   0539 F0                 1671 	movx	@dptr,a
                           1672 ;HPSDR_firmware_1_0_0.c:160: break;
   053A 02 07 E9           1673 	ljmp	00147$
                           1674 ;HPSDR_firmware_1_0_0.c:161: case GD_STRING:            			// String
   053D                    1675 00106$:
                           1676 ;HPSDR_firmware_1_0_0.c:162: found = FALSE;
                           1677 ;     genAssign
   053D 7A 00              1678 	mov	r2,#0x00
                           1679 ;HPSDR_firmware_1_0_0.c:163: index = SETUPDAT[2];
                           1680 ;     genPointerGet
                           1681 ;     genFarPointerGet
   053F 90 E6 BA           1682 	mov	dptr,#(_SETUPDAT + 0x0002)
   0542 E0                 1683 	movx	a,@dptr
   0543 FB                 1684 	mov	r3,a
                           1685 ;HPSDR_firmware_1_0_0.c:164: dscr = (STRINGDSCR xdata *) pStringDscr;
                           1686 ;     genAssign
   0544 90 22 14           1687 	mov	dptr,#_pStringDscr
   0547 E0                 1688 	movx	a,@dptr
   0548 FC                 1689 	mov	r4,a
   0549 A3                 1690 	inc	dptr
   054A E0                 1691 	movx	a,@dptr
   054B FD                 1692 	mov	r5,a
                           1693 ;     genCast
                           1694 ;HPSDR_firmware_1_0_0.c:165: while(dscr->type == STRING_DSCR)
                           1695 ;     genAssign
   054C                    1696 00109$:
                           1697 ;     genPlus
                           1698 ;     genPlusIncr
   054C 74 01              1699 	mov	a,#0x01
                           1700 ;	Peephole 236.a	used r4 instead of ar4
   054E 2C                 1701 	add	a,r4
   054F F5 82              1702 	mov	dpl,a
                           1703 ;	Peephole 181	changed mov to clr
   0551 E4                 1704 	clr	a
                           1705 ;	Peephole 236.b	used r5 instead of ar5
   0552 3D                 1706 	addc	a,r5
   0553 F5 83              1707 	mov	dph,a
                           1708 ;     genPointerGet
                           1709 ;     genFarPointerGet
   0555 E0                 1710 	movx	a,@dptr
   0556 FE                 1711 	mov	r6,a
                           1712 ;     genCmpEq
                           1713 ;	Peephole 112.b	changed ljmp to sjmp
                           1714 ;	Peephole 199	optimized misc jump sequence
   0557 BE 03 34           1715 	cjne	r6,#0x03,00111$
                           1716 ;00178$:
                           1717 ;	Peephole 200	removed redundant sjmp
   055A                    1718 00179$:
                           1719 ;HPSDR_firmware_1_0_0.c:167: if(!index--)
                           1720 ;     genAssign
   055A 8B 06              1721 	mov	ar6,r3
                           1722 ;     genMinus
                           1723 ;     genMinusDec
   055C 1B                 1724 	dec	r3
                           1725 ;     genIfx
   055D EE                 1726 	mov	a,r6
                           1727 ;     genIfxJump
                           1728 ;	Peephole 109	removed ljmp by inverse jump logic
                           1729 ;HPSDR_firmware_1_0_0.c:169: SUDPTRH = MSB(dscr);
                           1730 ;     genCast
                           1731 ;     genRightShift
                           1732 ;     genRightShiftLiteral
                           1733 ;     genrshTwo
                           1734 ;     genAnd
                           1735 ;	Peephole 177.c	removed redundant move
                           1736 ;	Peephole 256.f	loading 00180$ with zero from a
   055E 70 17              1737 	jnz	00108$
   0560                    1738 00180$:
   0560 8C 06              1739 	mov	ar6,r4
   0562 8D 07              1740 	mov	ar7,r5
   0564 8F 00              1741 	mov	ar0,r7
   0566 F9                 1742 	mov	r1,a
                           1743 ;     genCast
   0567 90 E6 B3           1744 	mov	dptr,#_SUDPTRH
   056A E8                 1745 	mov	a,r0
   056B F0                 1746 	movx	@dptr,a
                           1747 ;HPSDR_firmware_1_0_0.c:170: SUDPTRL = LSB(dscr);
                           1748 ;     genAnd
   056C 7F 00              1749 	mov	r7,#0x00
                           1750 ;     genCast
   056E 90 E6 B4           1751 	mov	dptr,#_SUDPTRL
   0571 EE                 1752 	mov	a,r6
   0572 F0                 1753 	movx	@dptr,a
                           1754 ;HPSDR_firmware_1_0_0.c:171: found = TRUE;
                           1755 ;     genAssign
   0573 7A 01              1756 	mov	r2,#0x01
                           1757 ;HPSDR_firmware_1_0_0.c:172: break;
                           1758 ;	Peephole 112.b	changed ljmp to sjmp
   0575 80 17              1759 	sjmp	00111$
   0577                    1760 00108$:
                           1761 ;HPSDR_firmware_1_0_0.c:174: dscr = (STRINGDSCR xdata *)((WORD)dscr + dscr->length);
                           1762 ;     genCast
   0577 8C 06              1763 	mov	ar6,r4
   0579 8D 07              1764 	mov	ar7,r5
                           1765 ;     genPointerGet
                           1766 ;     genFarPointerGet
   057B 8C 82              1767 	mov	dpl,r4
   057D 8D 83              1768 	mov	dph,r5
   057F E0                 1769 	movx	a,@dptr
                           1770 ;     genCast
                           1771 ;     genPlus
                           1772 ;	Peephole 236.g	used r0 instead of ar0
                           1773 ;	Peephole 177.d	removed redundant move
   0580 F8                 1774 	mov	r0,a
   0581 79 00              1775 	mov	r1,#0x00
                           1776 ;	Peephole 236.a	used r6 instead of ar6
   0583 2E                 1777 	add	a,r6
   0584 FE                 1778 	mov	r6,a
                           1779 ;	Peephole 236.g	used r1 instead of ar1
   0585 E9                 1780 	mov	a,r1
                           1781 ;	Peephole 236.b	used r7 instead of ar7
   0586 3F                 1782 	addc	a,r7
   0587 FF                 1783 	mov	r7,a
                           1784 ;     genCast
   0588 8E 04              1785 	mov	ar4,r6
   058A 8F 05              1786 	mov	ar5,r7
                           1787 ;	Peephole 112.b	changed ljmp to sjmp
   058C 80 BE              1788 	sjmp	00109$
   058E                    1789 00111$:
                           1790 ;HPSDR_firmware_1_0_0.c:176: if (!found)
                           1791 ;     genIfx
   058E EA                 1792 	mov	a,r2
                           1793 ;     genIfxJump
   058F 60 03              1794 	jz	00181$
   0591 02 07 E9           1795 	ljmp	00147$
   0594                    1796 00181$:
                           1797 ;HPSDR_firmware_1_0_0.c:178: FX2_STALL_EP0();   		// Stall End Point 0
                           1798 ;     genAssign
                           1799 ;     genOr
                           1800 ;	Peephole 248.a	optimized or to xdata
   0594 90 E6 A0           1801 	mov	dptr,#_EP0CS
   0597 E0                 1802 	movx	a,@dptr
   0598 FA                 1803 	mov	r2,a
   0599 44 01              1804 	orl	a,#0x01
   059B F0                 1805 	movx	@dptr,a
                           1806 ;HPSDR_firmware_1_0_0.c:180: break;
   059C 02 07 E9           1807 	ljmp	00147$
                           1808 ;HPSDR_firmware_1_0_0.c:181: default:            				// Invalid request
   059F                    1809 00114$:
                           1810 ;HPSDR_firmware_1_0_0.c:182: FX2_STALL_EP0();      		// Stall End Point 0
                           1811 ;     genAssign
                           1812 ;     genOr
                           1813 ;	Peephole 248.a	optimized or to xdata
   059F 90 E6 A0           1814 	mov	dptr,#_EP0CS
   05A2 E0                 1815 	movx	a,@dptr
   05A3 FA                 1816 	mov	r2,a
   05A4 44 01              1817 	orl	a,#0x01
   05A6 F0                 1818 	movx	@dptr,a
                           1819 ;HPSDR_firmware_1_0_0.c:184: break;
   05A7 02 07 E9           1820 	ljmp	00147$
                           1821 ;HPSDR_firmware_1_0_0.c:186: case SC_GET_INTERFACE:               // *** Get Interface
   05AA                    1822 00116$:
                           1823 ;HPSDR_firmware_1_0_0.c:187: EP0BUF[0] = AlternateSetting;
                           1824 ;     genAssign
   05AA 90 22 07           1825 	mov	dptr,#_AlternateSetting
   05AD E0                 1826 	movx	a,@dptr
                           1827 ;     genPointerSet
                           1828 ;     genFarPointerSet
                           1829 ;	Peephole 100	removed redundant mov
   05AE FA                 1830 	mov	r2,a
   05AF 90 E7 40           1831 	mov	dptr,#_EP0BUF
   05B2 F0                 1832 	movx	@dptr,a
                           1833 ;HPSDR_firmware_1_0_0.c:188: EP0BCH = 0;
                           1834 ;     genAssign
   05B3 90 E6 8A           1835 	mov	dptr,#_EP0BCH
                           1836 ;	Peephole 181	changed mov to clr
   05B6 E4                 1837 	clr	a
   05B7 F0                 1838 	movx	@dptr,a
                           1839 ;HPSDR_firmware_1_0_0.c:189: EP0BCL = 1;
                           1840 ;     genAssign
   05B8 90 E6 8B           1841 	mov	dptr,#_EP0BCL
   05BB 74 01              1842 	mov	a,#0x01
   05BD F0                 1843 	movx	@dptr,a
                           1844 ;HPSDR_firmware_1_0_0.c:190: break;
   05BE 02 07 E9           1845 	ljmp	00147$
                           1846 ;HPSDR_firmware_1_0_0.c:192: case SC_SET_INTERFACE:               // *** Set Interface
   05C1                    1847 00117$:
                           1848 ;HPSDR_firmware_1_0_0.c:193: AlternateSetting = SETUPDAT[2];
                           1849 ;     genPointerGet
                           1850 ;     genFarPointerGet
   05C1 90 E6 BA           1851 	mov	dptr,#(_SETUPDAT + 0x0002)
   05C4 E0                 1852 	movx	a,@dptr
                           1853 ;     genAssign
                           1854 ;	Peephole 100	removed redundant mov
   05C5 FA                 1855 	mov	r2,a
   05C6 90 22 07           1856 	mov	dptr,#_AlternateSetting
   05C9 F0                 1857 	movx	@dptr,a
                           1858 ;HPSDR_firmware_1_0_0.c:194: break;
   05CA 02 07 E9           1859 	ljmp	00147$
                           1860 ;HPSDR_firmware_1_0_0.c:196: case SC_SET_CONFIGURATION:           // *** Set Configuration
   05CD                    1861 00118$:
                           1862 ;HPSDR_firmware_1_0_0.c:197: Configuration = SETUPDAT[2];
                           1863 ;     genPointerGet
                           1864 ;     genFarPointerGet
   05CD 90 E6 BA           1865 	mov	dptr,#(_SETUPDAT + 0x0002)
   05D0 E0                 1866 	movx	a,@dptr
                           1867 ;     genAssign
                           1868 ;	Peephole 100	removed redundant mov
   05D1 FA                 1869 	mov	r2,a
   05D2 90 22 06           1870 	mov	dptr,#_Configuration
   05D5 F0                 1871 	movx	@dptr,a
                           1872 ;HPSDR_firmware_1_0_0.c:198: break;
   05D6 02 07 E9           1873 	ljmp	00147$
                           1874 ;HPSDR_firmware_1_0_0.c:200: case SC_GET_CONFIGURATION:           // *** Get Configuration
   05D9                    1875 00119$:
                           1876 ;HPSDR_firmware_1_0_0.c:201: EP0BUF[0] = Configuration;
                           1877 ;     genAssign
   05D9 90 22 06           1878 	mov	dptr,#_Configuration
   05DC E0                 1879 	movx	a,@dptr
                           1880 ;     genPointerSet
                           1881 ;     genFarPointerSet
                           1882 ;	Peephole 100	removed redundant mov
   05DD FA                 1883 	mov	r2,a
   05DE 90 E7 40           1884 	mov	dptr,#_EP0BUF
   05E1 F0                 1885 	movx	@dptr,a
                           1886 ;HPSDR_firmware_1_0_0.c:202: EP0BCH = 0;
                           1887 ;     genAssign
   05E2 90 E6 8A           1888 	mov	dptr,#_EP0BCH
                           1889 ;	Peephole 181	changed mov to clr
   05E5 E4                 1890 	clr	a
   05E6 F0                 1891 	movx	@dptr,a
                           1892 ;HPSDR_firmware_1_0_0.c:203: EP0BCL = 1;
                           1893 ;     genAssign
   05E7 90 E6 8B           1894 	mov	dptr,#_EP0BCL
   05EA 74 01              1895 	mov	a,#0x01
   05EC F0                 1896 	movx	@dptr,a
                           1897 ;HPSDR_firmware_1_0_0.c:204: break;
   05ED 02 07 E9           1898 	ljmp	00147$
                           1899 ;HPSDR_firmware_1_0_0.c:206: case SC_GET_STATUS:                  // *** Get Status
   05F0                    1900 00120$:
                           1901 ;HPSDR_firmware_1_0_0.c:208: switch(SETUPDAT[0])
                           1902 ;     genPointerGet
                           1903 ;     genFarPointerGet
   05F0 90 E6 B8           1904 	mov	dptr,#_SETUPDAT
   05F3 E0                 1905 	movx	a,@dptr
   05F4 FA                 1906 	mov	r2,a
                           1907 ;     genCmpEq
   05F5 BA 80 02           1908 	cjne	r2,#0x80,00182$
                           1909 ;	Peephole 112.b	changed ljmp to sjmp
   05F8 80 0D              1910 	sjmp	00121$
   05FA                    1911 00182$:
                           1912 ;     genCmpEq
   05FA BA 81 02           1913 	cjne	r2,#0x81,00183$
                           1914 ;	Peephole 112.b	changed ljmp to sjmp
   05FD 80 2D              1915 	sjmp	00122$
   05FF                    1916 00183$:
                           1917 ;     genCmpEq
   05FF BA 82 02           1918 	cjne	r2,#0x82,00184$
                           1919 ;	Peephole 112.b	changed ljmp to sjmp
   0602 80 3E              1920 	sjmp	00123$
   0604                    1921 00184$:
   0604 02 06 88           1922 	ljmp	00124$
                           1923 ;HPSDR_firmware_1_0_0.c:210: case GS_DEVICE:            		// Device
   0607                    1924 00121$:
                           1925 ;HPSDR_firmware_1_0_0.c:211: EP0BUF[0] = ((BYTE)RWUEN << 1) | (BYTE)SELFPWR;
                           1926 ;     genAssign
   0607 90 22 01           1927 	mov	dptr,#_RWUEN
   060A E0                 1928 	movx	a,@dptr
                           1929 ;     genLeftShift
                           1930 ;     genLeftShiftLiteral
                           1931 ;     genlshOne
                           1932 ;	Peephole 105	removed redundant mov
                           1933 ;	Peephole 204	removed redundant mov
   060B 25 E0              1934 	add	a,acc
   060D FA                 1935 	mov	r2,a
                           1936 ;     genAssign
   060E 90 22 02           1937 	mov	dptr,#_SELFPWR
   0611 E0                 1938 	movx	a,@dptr
                           1939 ;     genOr
                           1940 ;	Peephole 105	removed redundant mov
   0612 FB                 1941 	mov	r3,a
   0613 42 02              1942 	orl	ar2,a
                           1943 ;     genPointerSet
                           1944 ;     genFarPointerSet
   0615 90 E7 40           1945 	mov	dptr,#_EP0BUF
   0618 EA                 1946 	mov	a,r2
   0619 F0                 1947 	movx	@dptr,a
                           1948 ;HPSDR_firmware_1_0_0.c:212: EP0BUF[1] = 0;
                           1949 ;     genPointerSet
                           1950 ;     genFarPointerSet
   061A 90 E7 41           1951 	mov	dptr,#(_EP0BUF + 0x0001)
                           1952 ;	Peephole 181	changed mov to clr
                           1953 ;HPSDR_firmware_1_0_0.c:213: EP0BCH = 0;
                           1954 ;     genAssign
                           1955 ;	Peephole 181	changed mov to clr
                           1956 ;	Peephole 219	removed redundant clear
   061D E4                 1957 	clr	a
   061E F0                 1958 	movx	@dptr,a
   061F 90 E6 8A           1959 	mov	dptr,#_EP0BCH
   0622 F0                 1960 	movx	@dptr,a
                           1961 ;HPSDR_firmware_1_0_0.c:214: EP0BCL = 2;
                           1962 ;     genAssign
   0623 90 E6 8B           1963 	mov	dptr,#_EP0BCL
   0626 74 02              1964 	mov	a,#0x02
   0628 F0                 1965 	movx	@dptr,a
                           1966 ;HPSDR_firmware_1_0_0.c:215: break;
   0629 02 07 E9           1967 	ljmp	00147$
                           1968 ;HPSDR_firmware_1_0_0.c:216: case GS_INTERFACE:         		// Interface
   062C                    1969 00122$:
                           1970 ;HPSDR_firmware_1_0_0.c:217: EP0BUF[0] = 0;
                           1971 ;     genPointerSet
                           1972 ;     genFarPointerSet
   062C 90 E7 40           1973 	mov	dptr,#_EP0BUF
                           1974 ;	Peephole 181	changed mov to clr
                           1975 ;HPSDR_firmware_1_0_0.c:218: EP0BUF[1] = 0;
                           1976 ;     genPointerSet
                           1977 ;     genFarPointerSet
                           1978 ;	Peephole 181	changed mov to clr
                           1979 ;	Peephole 219	removed redundant clear
                           1980 ;HPSDR_firmware_1_0_0.c:219: EP0BCH = 0;
                           1981 ;     genAssign
                           1982 ;	Peephole 181	changed mov to clr
                           1983 ;	Peephole 219.a	removed redundant clear
   062F E4                 1984 	clr	a
   0630 F0                 1985 	movx	@dptr,a
   0631 90 E7 41           1986 	mov	dptr,#(_EP0BUF + 0x0001)
   0634 F0                 1987 	movx	@dptr,a
   0635 90 E6 8A           1988 	mov	dptr,#_EP0BCH
   0638 F0                 1989 	movx	@dptr,a
                           1990 ;HPSDR_firmware_1_0_0.c:220: EP0BCL = 2;
                           1991 ;     genAssign
   0639 90 E6 8B           1992 	mov	dptr,#_EP0BCL
   063C 74 02              1993 	mov	a,#0x02
   063E F0                 1994 	movx	@dptr,a
                           1995 ;HPSDR_firmware_1_0_0.c:221: break;
   063F 02 07 E9           1996 	ljmp	00147$
                           1997 ;HPSDR_firmware_1_0_0.c:222: case GS_ENDPOINT:         		// End Point
   0642                    1998 00123$:
                           1999 ;HPSDR_firmware_1_0_0.c:223: EP0BUF[0] = *(BYTE xdata *) epcs(SETUPDAT[4]) & bmEPSTALL;
                           2000 ;     genPointerGet
                           2001 ;     genFarPointerGet
   0642 90 E6 BC           2002 	mov	dptr,#(_SETUPDAT + 0x0004)
   0645 E0                 2003 	movx	a,@dptr
   0646 FA                 2004 	mov	r2,a
                           2005 ;     genAnd
   0647 53 02 7E           2006 	anl	ar2,#0x7E
                           2007 ;     genPointerGet
                           2008 ;     genFarPointerGet
   064A 90 E6 BC           2009 	mov	dptr,#(_SETUPDAT + 0x0004)
   064D E0                 2010 	movx	a,@dptr
   064E FB                 2011 	mov	r3,a
                           2012 ;     genCmpGt
                           2013 ;     genCmp
   064F C3                 2014 	clr	c
   0650 74 80              2015 	mov	a,#0x80
   0652 9B                 2016 	subb	a,r3
   0653 E4                 2017 	clr	a
   0654 33                 2018 	rlc	a
                           2019 ;     genOr
                           2020 ;	Peephole 105	removed redundant mov
   0655 FB                 2021 	mov	r3,a
   0656 4A                 2022 	orl	a,r2
                           2023 ;     genPlus
                           2024 ;	Peephole 240	use clr instead of addc a,#0
                           2025 ;     genPointerGet
                           2026 ;     genCodePointerGet
                           2027 ;	Peephole 181	changed mov to clr
                           2028 ;	Peephole 186.d	optimized movc sequence
   0657 90 09 1C           2029 	mov	dptr,#_EPCS_Offset_Lookup_Table
   065A 93                 2030 	movc	a,@a+dptr
                           2031 ;     genCast
                           2032 ;	Peephole 105	removed redundant mov
   065B FA                 2033 	mov	r2,a
   065C 33                 2034 	rlc	a
   065D 95 E0              2035 	subb	a,acc
   065F FB                 2036 	mov	r3,a
                           2037 ;     genPlus
   0660 74 A1              2038 	mov	a,#0xA1
                           2039 ;	Peephole 236.a	used r2 instead of ar2
   0662 2A                 2040 	add	a,r2
   0663 FA                 2041 	mov	r2,a
   0664 74 E6              2042 	mov	a,#0xE6
                           2043 ;	Peephole 236.b	used r3 instead of ar3
   0666 3B                 2044 	addc	a,r3
   0667 FB                 2045 	mov	r3,a
                           2046 ;     genCast
   0668 8A 82              2047 	mov	dpl,r2
   066A 8B 83              2048 	mov	dph,r3
                           2049 ;     genPointerGet
                           2050 ;     genFarPointerGet
   066C E0                 2051 	movx	a,@dptr
   066D FA                 2052 	mov	r2,a
                           2053 ;     genAnd
   066E 53 02 01           2054 	anl	ar2,#0x01
                           2055 ;     genPointerSet
                           2056 ;     genFarPointerSet
   0671 90 E7 40           2057 	mov	dptr,#_EP0BUF
   0674 EA                 2058 	mov	a,r2
   0675 F0                 2059 	movx	@dptr,a
                           2060 ;HPSDR_firmware_1_0_0.c:224: EP0BUF[1] = 0;
                           2061 ;     genPointerSet
                           2062 ;     genFarPointerSet
   0676 90 E7 41           2063 	mov	dptr,#(_EP0BUF + 0x0001)
                           2064 ;	Peephole 181	changed mov to clr
                           2065 ;HPSDR_firmware_1_0_0.c:225: EP0BCH = 0;
                           2066 ;     genAssign
                           2067 ;	Peephole 181	changed mov to clr
                           2068 ;	Peephole 219	removed redundant clear
   0679 E4                 2069 	clr	a
   067A F0                 2070 	movx	@dptr,a
   067B 90 E6 8A           2071 	mov	dptr,#_EP0BCH
   067E F0                 2072 	movx	@dptr,a
                           2073 ;HPSDR_firmware_1_0_0.c:226: EP0BCL = 2;
                           2074 ;     genAssign
   067F 90 E6 8B           2075 	mov	dptr,#_EP0BCL
   0682 74 02              2076 	mov	a,#0x02
   0684 F0                 2077 	movx	@dptr,a
                           2078 ;HPSDR_firmware_1_0_0.c:227: break;
   0685 02 07 E9           2079 	ljmp	00147$
                           2080 ;HPSDR_firmware_1_0_0.c:228: default:            				// Invalid Command
   0688                    2081 00124$:
                           2082 ;HPSDR_firmware_1_0_0.c:229: FX2_STALL_EP0();      		// Stall End Point 0
                           2083 ;     genAssign
                           2084 ;     genOr
                           2085 ;	Peephole 248.a	optimized or to xdata
   0688 90 E6 A0           2086 	mov	dptr,#_EP0CS
   068B E0                 2087 	movx	a,@dptr
   068C FA                 2088 	mov	r2,a
   068D 44 01              2089 	orl	a,#0x01
   068F F0                 2090 	movx	@dptr,a
                           2091 ;HPSDR_firmware_1_0_0.c:231: break;
   0690 02 07 E9           2092 	ljmp	00147$
                           2093 ;HPSDR_firmware_1_0_0.c:233: case SC_CLEAR_FEATURE:                // *** Clear Feature
   0693                    2094 00126$:
                           2095 ;HPSDR_firmware_1_0_0.c:235: switch(SETUPDAT[0])
                           2096 ;     genPointerGet
                           2097 ;     genFarPointerGet
   0693 90 E6 B8           2098 	mov	dptr,#_SETUPDAT
   0696 E0                 2099 	movx	a,@dptr
                           2100 ;     genCmpEq
                           2101 ;	Peephole 112.b	changed ljmp to sjmp
                           2102 ;	Peephole 115.b	jump optimization
   0697 FA                 2103 	mov	r2,a
   0698 60 08              2104 	jz	00127$
   069A                    2105 00185$:
                           2106 ;     genCmpEq
   069A BA 02 02           2107 	cjne	r2,#0x02,00186$
                           2108 ;	Peephole 112.b	changed ljmp to sjmp
   069D 80 1E              2109 	sjmp	00131$
   069F                    2110 00186$:
   069F 02 07 E9           2111 	ljmp	00147$
                           2112 ;HPSDR_firmware_1_0_0.c:237: case FT_DEVICE:            	// Device
   06A2                    2113 00127$:
                           2114 ;HPSDR_firmware_1_0_0.c:238: if(SETUPDAT[2] == 1)
                           2115 ;     genPointerGet
                           2116 ;     genFarPointerGet
   06A2 90 E6 BA           2117 	mov	dptr,#(_SETUPDAT + 0x0002)
   06A5 E0                 2118 	movx	a,@dptr
   06A6 FA                 2119 	mov	r2,a
                           2120 ;     genCmpEq
                           2121 ;	Peephole 112.b	changed ljmp to sjmp
                           2122 ;	Peephole 199	optimized misc jump sequence
   06A7 BA 01 08           2123 	cjne	r2,#0x01,00129$
                           2124 ;00187$:
                           2125 ;	Peephole 200	removed redundant sjmp
   06AA                    2126 00188$:
                           2127 ;HPSDR_firmware_1_0_0.c:239: RWUEN = FALSE;       	// Disable Remote Wakeup
                           2128 ;     genAssign
   06AA 90 22 01           2129 	mov	dptr,#_RWUEN
                           2130 ;	Peephole 181	changed mov to clr
   06AD E4                 2131 	clr	a
   06AE F0                 2132 	movx	@dptr,a
   06AF 02 07 E9           2133 	ljmp	00147$
   06B2                    2134 00129$:
                           2135 ;HPSDR_firmware_1_0_0.c:241: FX2_STALL_EP0();   	// Stall End Point 0
                           2136 ;     genAssign
                           2137 ;     genOr
                           2138 ;	Peephole 248.a	optimized or to xdata
   06B2 90 E6 A0           2139 	mov	dptr,#_EP0CS
   06B5 E0                 2140 	movx	a,@dptr
   06B6 FA                 2141 	mov	r2,a
   06B7 44 01              2142 	orl	a,#0x01
   06B9 F0                 2143 	movx	@dptr,a
                           2144 ;HPSDR_firmware_1_0_0.c:242: break;
   06BA 02 07 E9           2145 	ljmp	00147$
                           2146 ;HPSDR_firmware_1_0_0.c:243: case FT_ENDPOINT:         	// End Point
   06BD                    2147 00131$:
                           2148 ;HPSDR_firmware_1_0_0.c:244: if(SETUPDAT[2] == 0)
                           2149 ;     genPointerGet
                           2150 ;     genFarPointerGet
   06BD 90 E6 BA           2151 	mov	dptr,#(_SETUPDAT + 0x0002)
   06C0 E0                 2152 	movx	a,@dptr
                           2153 ;     genCmpEq
                           2154 ;	Peephole 115.b	jump optimization
   06C1 FA                 2155 	mov	r2,a
   06C2 60 03              2156 	jz	00190$
   06C4                    2157 00189$:
   06C4 02 07 46           2158 	ljmp	00133$
   06C7                    2159 00190$:
                           2160 ;HPSDR_firmware_1_0_0.c:246: *(BYTE xdata *) epcs(SETUPDAT[4]) &= ~bmEPSTALL;
                           2161 ;     genPointerGet
                           2162 ;     genFarPointerGet
   06C7 90 E6 BC           2163 	mov	dptr,#(_SETUPDAT + 0x0004)
   06CA E0                 2164 	movx	a,@dptr
   06CB FA                 2165 	mov	r2,a
                           2166 ;     genAnd
   06CC 53 02 7E           2167 	anl	ar2,#0x7E
                           2168 ;     genPointerGet
                           2169 ;     genFarPointerGet
   06CF 90 E6 BC           2170 	mov	dptr,#(_SETUPDAT + 0x0004)
   06D2 E0                 2171 	movx	a,@dptr
   06D3 FB                 2172 	mov	r3,a
                           2173 ;     genCmpGt
                           2174 ;     genCmp
   06D4 C3                 2175 	clr	c
   06D5 74 80              2176 	mov	a,#0x80
   06D7 9B                 2177 	subb	a,r3
   06D8 E4                 2178 	clr	a
   06D9 33                 2179 	rlc	a
                           2180 ;     genOr
                           2181 ;	Peephole 105	removed redundant mov
   06DA FB                 2182 	mov	r3,a
   06DB 4A                 2183 	orl	a,r2
                           2184 ;     genPlus
                           2185 ;	Peephole 240	use clr instead of addc a,#0
                           2186 ;     genPointerGet
                           2187 ;     genCodePointerGet
                           2188 ;	Peephole 181	changed mov to clr
                           2189 ;	Peephole 186.d	optimized movc sequence
   06DC 90 09 1C           2190 	mov	dptr,#_EPCS_Offset_Lookup_Table
   06DF 93                 2191 	movc	a,@a+dptr
                           2192 ;     genCast
                           2193 ;	Peephole 105	removed redundant mov
   06E0 FA                 2194 	mov	r2,a
   06E1 33                 2195 	rlc	a
   06E2 95 E0              2196 	subb	a,acc
   06E4 FB                 2197 	mov	r3,a
                           2198 ;     genPlus
   06E5 74 A1              2199 	mov	a,#0xA1
                           2200 ;	Peephole 236.a	used r2 instead of ar2
   06E7 2A                 2201 	add	a,r2
   06E8 FA                 2202 	mov	r2,a
   06E9 74 E6              2203 	mov	a,#0xE6
                           2204 ;	Peephole 236.b	used r3 instead of ar3
   06EB 3B                 2205 	addc	a,r3
   06EC FB                 2206 	mov	r3,a
                           2207 ;     genCast
                           2208 ;     genPointerGet
                           2209 ;     genFarPointerGet
   06ED 90 E6 BC           2210 	mov	dptr,#(_SETUPDAT + 0x0004)
   06F0 E0                 2211 	movx	a,@dptr
   06F1 FC                 2212 	mov	r4,a
                           2213 ;     genAnd
   06F2 53 04 7E           2214 	anl	ar4,#0x7E
                           2215 ;     genPointerGet
                           2216 ;     genFarPointerGet
   06F5 90 E6 BC           2217 	mov	dptr,#(_SETUPDAT + 0x0004)
   06F8 E0                 2218 	movx	a,@dptr
   06F9 FD                 2219 	mov	r5,a
                           2220 ;     genCmpGt
                           2221 ;     genCmp
   06FA C3                 2222 	clr	c
   06FB 74 80              2223 	mov	a,#0x80
   06FD 9D                 2224 	subb	a,r5
   06FE E4                 2225 	clr	a
   06FF 33                 2226 	rlc	a
                           2227 ;     genOr
                           2228 ;	Peephole 105	removed redundant mov
   0700 FD                 2229 	mov	r5,a
   0701 4C                 2230 	orl	a,r4
                           2231 ;     genPlus
                           2232 ;	Peephole 240	use clr instead of addc a,#0
                           2233 ;     genPointerGet
                           2234 ;     genCodePointerGet
                           2235 ;	Peephole 181	changed mov to clr
                           2236 ;	Peephole 186.d	optimized movc sequence
   0702 90 09 1C           2237 	mov	dptr,#_EPCS_Offset_Lookup_Table
   0705 93                 2238 	movc	a,@a+dptr
                           2239 ;     genCast
                           2240 ;	Peephole 105	removed redundant mov
   0706 FC                 2241 	mov	r4,a
   0707 33                 2242 	rlc	a
   0708 95 E0              2243 	subb	a,acc
   070A FD                 2244 	mov	r5,a
                           2245 ;     genPlus
   070B 74 A1              2246 	mov	a,#0xA1
                           2247 ;	Peephole 236.a	used r4 instead of ar4
   070D 2C                 2248 	add	a,r4
   070E FC                 2249 	mov	r4,a
   070F 74 E6              2250 	mov	a,#0xE6
                           2251 ;	Peephole 236.b	used r5 instead of ar5
   0711 3D                 2252 	addc	a,r5
   0712 FD                 2253 	mov	r5,a
                           2254 ;     genCast
   0713 8C 82              2255 	mov	dpl,r4
   0715 8D 83              2256 	mov	dph,r5
                           2257 ;     genPointerGet
                           2258 ;     genFarPointerGet
   0717 E0                 2259 	movx	a,@dptr
   0718 FC                 2260 	mov	r4,a
                           2261 ;     genAnd
   0719 53 04 FE           2262 	anl	ar4,#0xFE
                           2263 ;     genPointerSet
                           2264 ;     genFarPointerSet
   071C 8A 82              2265 	mov	dpl,r2
   071E 8B 83              2266 	mov	dph,r3
   0720 EC                 2267 	mov	a,r4
   0721 F0                 2268 	movx	@dptr,a
                           2269 ;HPSDR_firmware_1_0_0.c:247: FX2_RESET_DATA_TOGGLE( SETUPDAT[4] );
                           2270 ;     genPointerGet
                           2271 ;     genFarPointerGet
   0722 90 E6 BC           2272 	mov	dptr,#(_SETUPDAT + 0x0004)
   0725 E0                 2273 	movx	a,@dptr
                           2274 ;     genAnd
                           2275 ;     genRightShift
                           2276 ;     genRightShiftLiteral
                           2277 ;     genrshOne
                           2278 ;	Peephole 139	removed redundant mov
   0726 54 80              2279 	anl	a,#0x80
   0728 FA                 2280 	mov	r2,a
   0729 C4                 2281 	swap	a
   072A 23                 2282 	rl	a
   072B 54 1F              2283 	anl	a,#0x1f
   072D FA                 2284 	mov	r2,a
                           2285 ;     genPointerGet
                           2286 ;     genFarPointerGet
   072E 90 E6 BC           2287 	mov	dptr,#(_SETUPDAT + 0x0004)
   0731 E0                 2288 	movx	a,@dptr
   0732 FB                 2289 	mov	r3,a
                           2290 ;     genAnd
   0733 74 0F              2291 	mov	a,#0x0F
   0735 5B                 2292 	anl	a,r3
                           2293 ;     genPlus
   0736 90 E6 83           2294 	mov	dptr,#_TOGCTL
                           2295 ;	Peephole 236.a	used r2 instead of ar2
   0739 2A                 2296 	add	a,r2
   073A F0                 2297 	movx	@dptr,a
                           2298 ;     genAssign
                           2299 ;     genOr
                           2300 ;	Peephole 248.a	optimized or to xdata
   073B 90 E6 83           2301 	mov	dptr,#_TOGCTL
   073E E0                 2302 	movx	a,@dptr
   073F FA                 2303 	mov	r2,a
   0740 44 20              2304 	orl	a,#0x20
   0742 F0                 2305 	movx	@dptr,a
   0743 02 07 E9           2306 	ljmp	00147$
   0746                    2307 00133$:
                           2308 ;HPSDR_firmware_1_0_0.c:250: FX2_STALL_EP0();   	// Stall End Point 0
                           2309 ;     genAssign
                           2310 ;     genOr
                           2311 ;	Peephole 248.a	optimized or to xdata
   0746 90 E6 A0           2312 	mov	dptr,#_EP0CS
   0749 E0                 2313 	movx	a,@dptr
   074A FA                 2314 	mov	r2,a
   074B 44 01              2315 	orl	a,#0x01
   074D F0                 2316 	movx	@dptr,a
                           2317 ;HPSDR_firmware_1_0_0.c:253: break;
   074E 02 07 E9           2318 	ljmp	00147$
                           2319 ;HPSDR_firmware_1_0_0.c:255: case SC_SET_FEATURE:                  // *** Set Feature
   0751                    2320 00136$:
                           2321 ;HPSDR_firmware_1_0_0.c:257: switch(SETUPDAT[0])
                           2322 ;     genPointerGet
                           2323 ;     genFarPointerGet
   0751 90 E6 B8           2324 	mov	dptr,#_SETUPDAT
   0754 E0                 2325 	movx	a,@dptr
                           2326 ;     genCmpEq
                           2327 ;	Peephole 112.b	changed ljmp to sjmp
                           2328 ;	Peephole 115.b	jump optimization
   0755 FA                 2329 	mov	r2,a
   0756 60 08              2330 	jz	00137$
   0758                    2331 00191$:
                           2332 ;     genCmpEq
   0758 BA 02 02           2333 	cjne	r2,#0x02,00192$
                           2334 ;	Peephole 112.b	changed ljmp to sjmp
   075B 80 27              2335 	sjmp	00144$
   075D                    2336 00192$:
   075D 02 07 E9           2337 	ljmp	00147$
                           2338 ;HPSDR_firmware_1_0_0.c:259: case FT_DEVICE:            // Device
   0760                    2339 00137$:
                           2340 ;HPSDR_firmware_1_0_0.c:261: if(SETUPDAT[2] == 1)
                           2341 ;     genPointerGet
                           2342 ;     genFarPointerGet
   0760 90 E6 BA           2343 	mov	dptr,#(_SETUPDAT + 0x0002)
   0763 E0                 2344 	movx	a,@dptr
   0764 FA                 2345 	mov	r2,a
                           2346 ;     genCmpEq
                           2347 ;	Peephole 112.b	changed ljmp to sjmp
                           2348 ;	Peephole 199	optimized misc jump sequence
   0765 BA 01 08           2349 	cjne	r2,#0x01,00142$
                           2350 ;00193$:
                           2351 ;	Peephole 200	removed redundant sjmp
   0768                    2352 00194$:
                           2353 ;HPSDR_firmware_1_0_0.c:262: RWUEN = TRUE;      // Enable Remote Wakeup
                           2354 ;     genAssign
   0768 90 22 01           2355 	mov	dptr,#_RWUEN
   076B 74 01              2356 	mov	a,#0x01
   076D F0                 2357 	movx	@dptr,a
                           2358 ;	Peephole 112.b	changed ljmp to sjmp
   076E 80 79              2359 	sjmp	00147$
   0770                    2360 00142$:
                           2361 ;HPSDR_firmware_1_0_0.c:263: else if(SETUPDAT[2] == 2)
                           2362 ;     genPointerGet
                           2363 ;     genFarPointerGet
   0770 90 E6 BA           2364 	mov	dptr,#(_SETUPDAT + 0x0002)
   0773 E0                 2365 	movx	a,@dptr
   0774 FA                 2366 	mov	r2,a
                           2367 ;     genCmpEq
   0775 BA 02 02           2368 	cjne	r2,#0x02,00195$
                           2369 ;	Peephole 112.b	changed ljmp to sjmp
   0778 80 6F              2370 	sjmp	00147$
   077A                    2371 00195$:
                           2372 ;HPSDR_firmware_1_0_0.c:267: FX2_STALL_EP0();   // Stall End Point 0
                           2373 ;     genAssign
                           2374 ;     genOr
                           2375 ;	Peephole 248.a	optimized or to xdata
   077A 90 E6 A0           2376 	mov	dptr,#_EP0CS
   077D E0                 2377 	movx	a,@dptr
   077E FA                 2378 	mov	r2,a
   077F 44 01              2379 	orl	a,#0x01
   0781 F0                 2380 	movx	@dptr,a
                           2381 ;HPSDR_firmware_1_0_0.c:268: break;
                           2382 ;HPSDR_firmware_1_0_0.c:270: case FT_ENDPOINT:         // End Point
                           2383 ;	Peephole 112.b	changed ljmp to sjmp
   0782 80 65              2384 	sjmp	00147$
   0784                    2385 00144$:
                           2386 ;HPSDR_firmware_1_0_0.c:271: *(BYTE xdata *) epcs(SETUPDAT[4]) |= bmEPSTALL;
                           2387 ;     genPointerGet
                           2388 ;     genFarPointerGet
   0784 90 E6 BC           2389 	mov	dptr,#(_SETUPDAT + 0x0004)
   0787 E0                 2390 	movx	a,@dptr
   0788 FA                 2391 	mov	r2,a
                           2392 ;     genAnd
   0789 53 02 7E           2393 	anl	ar2,#0x7E
                           2394 ;     genPointerGet
                           2395 ;     genFarPointerGet
   078C 90 E6 BC           2396 	mov	dptr,#(_SETUPDAT + 0x0004)
   078F E0                 2397 	movx	a,@dptr
   0790 FB                 2398 	mov	r3,a
                           2399 ;     genCmpGt
                           2400 ;     genCmp
   0791 C3                 2401 	clr	c
   0792 74 80              2402 	mov	a,#0x80
   0794 9B                 2403 	subb	a,r3
   0795 E4                 2404 	clr	a
   0796 33                 2405 	rlc	a
                           2406 ;     genOr
                           2407 ;	Peephole 105	removed redundant mov
   0797 FB                 2408 	mov	r3,a
   0798 4A                 2409 	orl	a,r2
                           2410 ;     genPlus
                           2411 ;	Peephole 240	use clr instead of addc a,#0
                           2412 ;     genPointerGet
                           2413 ;     genCodePointerGet
                           2414 ;	Peephole 181	changed mov to clr
                           2415 ;	Peephole 186.d	optimized movc sequence
   0799 90 09 1C           2416 	mov	dptr,#_EPCS_Offset_Lookup_Table
   079C 93                 2417 	movc	a,@a+dptr
                           2418 ;     genCast
                           2419 ;	Peephole 105	removed redundant mov
   079D FA                 2420 	mov	r2,a
   079E 33                 2421 	rlc	a
   079F 95 E0              2422 	subb	a,acc
   07A1 FB                 2423 	mov	r3,a
                           2424 ;     genPlus
   07A2 74 A1              2425 	mov	a,#0xA1
                           2426 ;	Peephole 236.a	used r2 instead of ar2
   07A4 2A                 2427 	add	a,r2
   07A5 FA                 2428 	mov	r2,a
   07A6 74 E6              2429 	mov	a,#0xE6
                           2430 ;	Peephole 236.b	used r3 instead of ar3
   07A8 3B                 2431 	addc	a,r3
   07A9 FB                 2432 	mov	r3,a
                           2433 ;     genCast
                           2434 ;     genPointerGet
                           2435 ;     genFarPointerGet
   07AA 90 E6 BC           2436 	mov	dptr,#(_SETUPDAT + 0x0004)
   07AD E0                 2437 	movx	a,@dptr
   07AE FC                 2438 	mov	r4,a
                           2439 ;     genAnd
   07AF 53 04 7E           2440 	anl	ar4,#0x7E
                           2441 ;     genPointerGet
                           2442 ;     genFarPointerGet
   07B2 90 E6 BC           2443 	mov	dptr,#(_SETUPDAT + 0x0004)
   07B5 E0                 2444 	movx	a,@dptr
   07B6 FD                 2445 	mov	r5,a
                           2446 ;     genCmpGt
                           2447 ;     genCmp
   07B7 C3                 2448 	clr	c
   07B8 74 80              2449 	mov	a,#0x80
   07BA 9D                 2450 	subb	a,r5
   07BB E4                 2451 	clr	a
   07BC 33                 2452 	rlc	a
                           2453 ;     genOr
                           2454 ;	Peephole 105	removed redundant mov
   07BD FD                 2455 	mov	r5,a
   07BE 4C                 2456 	orl	a,r4
                           2457 ;     genPlus
                           2458 ;	Peephole 240	use clr instead of addc a,#0
                           2459 ;     genPointerGet
                           2460 ;     genCodePointerGet
                           2461 ;	Peephole 181	changed mov to clr
                           2462 ;	Peephole 186.d	optimized movc sequence
   07BF 90 09 1C           2463 	mov	dptr,#_EPCS_Offset_Lookup_Table
   07C2 93                 2464 	movc	a,@a+dptr
                           2465 ;     genCast
                           2466 ;	Peephole 105	removed redundant mov
   07C3 FC                 2467 	mov	r4,a
   07C4 33                 2468 	rlc	a
   07C5 95 E0              2469 	subb	a,acc
   07C7 FD                 2470 	mov	r5,a
                           2471 ;     genPlus
   07C8 74 A1              2472 	mov	a,#0xA1
                           2473 ;	Peephole 236.a	used r4 instead of ar4
   07CA 2C                 2474 	add	a,r4
   07CB FC                 2475 	mov	r4,a
   07CC 74 E6              2476 	mov	a,#0xE6
                           2477 ;	Peephole 236.b	used r5 instead of ar5
   07CE 3D                 2478 	addc	a,r5
   07CF FD                 2479 	mov	r5,a
                           2480 ;     genCast
   07D0 8C 82              2481 	mov	dpl,r4
   07D2 8D 83              2482 	mov	dph,r5
                           2483 ;     genPointerGet
                           2484 ;     genFarPointerGet
   07D4 E0                 2485 	movx	a,@dptr
   07D5 FC                 2486 	mov	r4,a
                           2487 ;     genOr
   07D6 43 04 01           2488 	orl	ar4,#0x01
                           2489 ;     genPointerSet
                           2490 ;     genFarPointerSet
   07D9 8A 82              2491 	mov	dpl,r2
   07DB 8B 83              2492 	mov	dph,r3
   07DD EC                 2493 	mov	a,r4
   07DE F0                 2494 	movx	@dptr,a
                           2495 ;HPSDR_firmware_1_0_0.c:274: break;
                           2496 ;HPSDR_firmware_1_0_0.c:276: default:                     // *** Invalid Command
                           2497 ;	Peephole 112.b	changed ljmp to sjmp
   07DF 80 08              2498 	sjmp	00147$
   07E1                    2499 00146$:
                           2500 ;HPSDR_firmware_1_0_0.c:277: FX2_STALL_EP0();         // Stall End Point 0
                           2501 ;     genAssign
                           2502 ;     genOr
                           2503 ;	Peephole 248.a	optimized or to xdata
   07E1 90 E6 A0           2504 	mov	dptr,#_EP0CS
   07E4 E0                 2505 	movx	a,@dptr
   07E5 FA                 2506 	mov	r2,a
   07E6 44 01              2507 	orl	a,#0x01
   07E8 F0                 2508 	movx	@dptr,a
                           2509 ;HPSDR_firmware_1_0_0.c:279: }
   07E9                    2510 00147$:
                           2511 ;HPSDR_firmware_1_0_0.c:281: FX2_ACK_HANDSHAKE();	// Acknowledge handshake phase of device request
                           2512 ;     genAssign
                           2513 ;     genOr
                           2514 ;	Peephole 248.a	optimized or to xdata
   07E9 90 E6 A0           2515 	mov	dptr,#_EP0CS
   07EC E0                 2516 	movx	a,@dptr
   07ED FA                 2517 	mov	r2,a
   07EE 44 80              2518 	orl	a,#0x80
   07F0 F0                 2519 	movx	@dptr,a
   07F1                    2520 00148$:
   07F1 22                 2521 	ret
                           2522 ;------------------------------------------------------------
                           2523 ;Allocation info for local variables in function 'main_loop'
                           2524 ;------------------------------------------------------------
                           2525 ;------------------------------------------------------------
                           2526 ;HPSDR_firmware_1_0_0.c:286: void main_loop (void)
                           2527 ;	-----------------------------------------
                           2528 ;	 function main_loop
                           2529 ;	-----------------------------------------
   07F2                    2530 _main_loop:
                           2531 ;HPSDR_firmware_1_0_0.c:288: while (TRUE)
   07F2                    2532 00106$:
                           2533 ;HPSDR_firmware_1_0_0.c:290: if(RxSUD)            			
                           2534 ;     genAssign
   07F2 90 22 00           2535 	mov	dptr,#_RxSUD
   07F5 E0                 2536 	movx	a,@dptr
                           2537 ;     genIfx
                           2538 ;	Peephole 105	removed redundant mov
   07F6 FA                 2539 	mov	r2,a
                           2540 ;     genIfxJump
                           2541 ;	Peephole 110	removed ljmp by inverse jump logic
   07F7 60 08              2542 	jz	00102$
   07F9                    2543 00113$:
                           2544 ;HPSDR_firmware_1_0_0.c:292: process_ep0_data();         // do setup command
                           2545 ;     genCall
   07F9 12 04 79           2546 	lcall	_process_ep0_data
                           2547 ;HPSDR_firmware_1_0_0.c:293: RxSUD = FALSE;        		// clear SUDAV flag
                           2548 ;     genAssign
   07FC 90 22 00           2549 	mov	dptr,#_RxSUD
                           2550 ;	Peephole 181	changed mov to clr
   07FF E4                 2551 	clr	a
   0800 F0                 2552 	movx	@dptr,a
   0801                    2553 00102$:
                           2554 ;HPSDR_firmware_1_0_0.c:296: if (EP1HASDATA)
                           2555 ;     genAssign
   0801 90 22 03           2556 	mov	dptr,#_EP1HASDATA
   0804 E0                 2557 	movx	a,@dptr
                           2558 ;     genIfx
                           2559 ;	Peephole 105	removed redundant mov
   0805 FA                 2560 	mov	r2,a
                           2561 ;     genIfxJump
                           2562 ;	Peephole 110	removed ljmp by inverse jump logic
   0806 60 EA              2563 	jz	00106$
   0808                    2564 00114$:
                           2565 ;HPSDR_firmware_1_0_0.c:298: process_ep1_data();			// do ep1 data
                           2566 ;     genCall
   0808 12 03 89           2567 	lcall	_process_ep1_data
                           2568 ;HPSDR_firmware_1_0_0.c:299: EP1HASDATA = FALSE;			// clear EP1 flag
                           2569 ;     genAssign
   080B 90 22 03           2570 	mov	dptr,#_EP1HASDATA
                           2571 ;	Peephole 181	changed mov to clr
   080E E4                 2572 	clr	a
   080F F0                 2573 	movx	@dptr,a
                           2574 ;	Peephole 112.b	changed ljmp to sjmp
   0810 80 E0              2575 	sjmp	00106$
   0812                    2576 00108$:
   0812 22                 2577 	ret
                           2578 ;------------------------------------------------------------
                           2579 ;Allocation info for local variables in function 'main'
                           2580 ;------------------------------------------------------------
                           2581 ;------------------------------------------------------------
                           2582 ;HPSDR_firmware_1_0_0.c:306: void main (void)
                           2583 ;	-----------------------------------------
                           2584 ;	 function main
                           2585 ;	-----------------------------------------
   0813                    2586 _main:
                           2587 ;HPSDR_firmware_1_0_0.c:308: RxSUD = FALSE;
                           2588 ;     genAssign
   0813 90 22 00           2589 	mov	dptr,#_RxSUD
                           2590 ;	Peephole 181	changed mov to clr
                           2591 ;HPSDR_firmware_1_0_0.c:309: EP1HASDATA = FALSE;
                           2592 ;     genAssign
                           2593 ;	Peephole 181	changed mov to clr
                           2594 ;	Peephole 219	removed redundant clear
   0816 E4                 2595 	clr	a
   0817 F0                 2596 	movx	@dptr,a
   0818 90 22 03           2597 	mov	dptr,#_EP1HASDATA
   081B F0                 2598 	movx	@dptr,a
                           2599 ;HPSDR_firmware_1_0_0.c:311: init_fx2();
                           2600 ;     genCall
   081C 12 02 9C           2601 	lcall	_init_fx2
                           2602 ;HPSDR_firmware_1_0_0.c:312: setup_io();
                           2603 ;     genCall
   081F 12 02 E3           2604 	lcall	_setup_io
                           2605 ;HPSDR_firmware_1_0_0.c:313: setup_descriptor_pointers();
                           2606 ;     genCall
   0822 12 03 18           2607 	lcall	_setup_descriptor_pointers
                           2608 ;HPSDR_firmware_1_0_0.c:314: setup_interrupts();
                           2609 ;     genCall
   0825 12 02 F9           2610 	lcall	_setup_interrupts
                           2611 ;HPSDR_firmware_1_0_0.c:316: if(FX2_NEED_TO_RENUM())
                           2612 ;     genAssign
   0828 90 E6 80           2613 	mov	dptr,#_USBCS
   082B E0                 2614 	movx	a,@dptr
                           2615 ;     genAnd
                           2616 ;	Peephole 105	removed redundant mov
   082C FA                 2617 	mov	r2,a
                           2618 ;     genIfxJump
                           2619 ;	Peephole 112.a	removed ljmp by inverse jump logic
   082D 20 E1 06           2620 	jb	acc.1,00102$
   0830                    2621 00106$:
                           2622 ;HPSDR_firmware_1_0_0.c:318: FX2_Discon(TRUE);   			// renumerate
                           2623 ;     genCall
   0830 75 82 01           2624 	mov	dpl,#0x01
   0833 12 02 67           2625 	lcall	_FX2_Discon
   0836                    2626 00102$:
                           2627 ;HPSDR_firmware_1_0_0.c:321: FX2_USB_CONNECT();					// Connect USB
                           2628 ;     genAssign
                           2629 ;     genAnd
                           2630 ;	Peephole 248.b	optimized and to xdata
   0836 90 E6 80           2631 	mov	dptr,#_USBCS
   0839 E0                 2632 	movx	a,@dptr
   083A FA                 2633 	mov	r2,a
   083B 54 F7              2634 	anl	a,#0xF7
   083D F0                 2635 	movx	@dptr,a
                           2636 ;HPSDR_firmware_1_0_0.c:322: FX2_DATA_WAIT_STATE(0); 			// MOVX takes 2 cycles (valid 0-7)
                           2637 ;     genAssign
   083E 75 8E 00           2638 	mov	_CKCON,#0x00
                           2639 ;HPSDR_firmware_1_0_0.c:324: IOA = 0x02;
                           2640 ;     genAssign
   0841 75 80 02           2641 	mov	_IOA,#0x02
                           2642 ;HPSDR_firmware_1_0_0.c:325: IOE = 0x02;
                           2643 ;     genAssign
   0844 75 B1 02           2644 	mov	_IOE,#0x02
                           2645 ;HPSDR_firmware_1_0_0.c:327: main_loop();
                           2646 ;     genCall
                           2647 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0847 02 07 F2           2648 	ljmp	_main_loop
                           2649 ;------------------------------------------------------------
                           2650 ;Allocation info for local variables in function 'ISR_Sudav'
                           2651 ;------------------------------------------------------------
                           2652 ;------------------------------------------------------------
                           2653 ;HPSDR_firmware_1_0_0.c:333: void ISR_Sudav(void) interrupt  
                           2654 ;	-----------------------------------------
                           2655 ;	 function ISR_Sudav
                           2656 ;	-----------------------------------------
   084A                    2657 _ISR_Sudav:
   084A C0 E0              2658 	push	acc
   084C C0 82              2659 	push	dpl
   084E C0 83              2660 	push	dph
                           2661 ;HPSDR_firmware_1_0_0.c:335: RxSUD = TRUE;            // Set flag
                           2662 ;     genAssign
   0850 90 22 00           2663 	mov	dptr,#_RxSUD
   0853 74 01              2664 	mov	a,#0x01
   0855 F0                 2665 	movx	@dptr,a
                           2666 ;HPSDR_firmware_1_0_0.c:336: FX2_IRQ_CLEAR();
                           2667 ;     genAnd
   0856 53 91 EF           2668 	anl	_EXIF,#0xEF
                           2669 ;HPSDR_firmware_1_0_0.c:337: FX2_CLEAR_USB_IRQ(bmSUDAV);	// Clear SUDAV IRQ
                           2670 ;     genAssign
   0859 90 E6 5D           2671 	mov	dptr,#_USBIRQ
   085C 74 01              2672 	mov	a,#0x01
   085E F0                 2673 	movx	@dptr,a
   085F                    2674 00101$:
   085F D0 83              2675 	pop	dph
   0861 D0 82              2676 	pop	dpl
   0863 D0 E0              2677 	pop	acc
   0865 32                 2678 	reti
                           2679 ;	eliminated unneeded push/pop psw
                           2680 ;	eliminated unneeded push/pop b
                           2681 ;------------------------------------------------------------
                           2682 ;Allocation info for local variables in function 'ISR_Sutok'
                           2683 ;------------------------------------------------------------
                           2684 ;------------------------------------------------------------
                           2685 ;HPSDR_firmware_1_0_0.c:341: void ISR_Sutok(void) interrupt  
                           2686 ;	-----------------------------------------
                           2687 ;	 function ISR_Sutok
                           2688 ;	-----------------------------------------
   0866                    2689 _ISR_Sutok:
   0866 C0 E0              2690 	push	acc
   0868 C0 82              2691 	push	dpl
   086A C0 83              2692 	push	dph
                           2693 ;HPSDR_firmware_1_0_0.c:343: FX2_IRQ_CLEAR();
                           2694 ;     genAnd
   086C 53 91 EF           2695 	anl	_EXIF,#0xEF
                           2696 ;HPSDR_firmware_1_0_0.c:344: FX2_CLEAR_USB_IRQ(bmSUTOK);	// Clear SUTOK IRQ
                           2697 ;     genAssign
   086F 90 E6 5D           2698 	mov	dptr,#_USBIRQ
   0872 74 04              2699 	mov	a,#0x04
   0874 F0                 2700 	movx	@dptr,a
   0875                    2701 00101$:
   0875 D0 83              2702 	pop	dph
   0877 D0 82              2703 	pop	dpl
   0879 D0 E0              2704 	pop	acc
   087B 32                 2705 	reti
                           2706 ;	eliminated unneeded push/pop psw
                           2707 ;	eliminated unneeded push/pop b
                           2708 ;------------------------------------------------------------
                           2709 ;Allocation info for local variables in function 'ISR_Usb_Reset'
                           2710 ;------------------------------------------------------------
                           2711 ;------------------------------------------------------------
                           2712 ;HPSDR_firmware_1_0_0.c:347: void ISR_Usb_Reset(void) interrupt  
                           2713 ;	-----------------------------------------
                           2714 ;	 function ISR_Usb_Reset
                           2715 ;	-----------------------------------------
   087C                    2716 _ISR_Usb_Reset:
   087C C0 E0              2717 	push	acc
   087E C0 82              2718 	push	dpl
   0880 C0 83              2719 	push	dph
   0882 C0 02              2720 	push	ar2
   0884 C0 03              2721 	push	ar3
                           2722 ;HPSDR_firmware_1_0_0.c:350: pConfigDscr = pFullSpeedConfigDscr;
                           2723 ;     genAssign
   0886 90 22 0E           2724 	mov	dptr,#_pFullSpeedConfigDscr
   0889 E0                 2725 	movx	a,@dptr
   088A FA                 2726 	mov	r2,a
   088B A3                 2727 	inc	dptr
   088C E0                 2728 	movx	a,@dptr
   088D FB                 2729 	mov	r3,a
                           2730 ;     genAssign
   088E 90 22 10           2731 	mov	dptr,#_pConfigDscr
   0891 EA                 2732 	mov	a,r2
   0892 F0                 2733 	movx	@dptr,a
   0893 A3                 2734 	inc	dptr
   0894 EB                 2735 	mov	a,r3
   0895 F0                 2736 	movx	@dptr,a
                           2737 ;HPSDR_firmware_1_0_0.c:351: pOtherConfigDscr = pHighSpeedConfigDscr;
                           2738 ;     genAssign
   0896 90 22 0C           2739 	mov	dptr,#_pHighSpeedConfigDscr
   0899 E0                 2740 	movx	a,@dptr
   089A FA                 2741 	mov	r2,a
   089B A3                 2742 	inc	dptr
   089C E0                 2743 	movx	a,@dptr
   089D FB                 2744 	mov	r3,a
                           2745 ;     genAssign
   089E 90 22 12           2746 	mov	dptr,#_pOtherConfigDscr
   08A1 EA                 2747 	mov	a,r2
   08A2 F0                 2748 	movx	@dptr,a
   08A3 A3                 2749 	inc	dptr
   08A4 EB                 2750 	mov	a,r3
   08A5 F0                 2751 	movx	@dptr,a
                           2752 ;HPSDR_firmware_1_0_0.c:353: FX2_IRQ_CLEAR();
                           2753 ;     genAnd
   08A6 53 91 EF           2754 	anl	_EXIF,#0xEF
                           2755 ;HPSDR_firmware_1_0_0.c:354: FX2_CLEAR_USB_IRQ(bmURES);	// Clear URES IRQ
                           2756 ;     genAssign
   08A9 90 E6 5D           2757 	mov	dptr,#_USBIRQ
   08AC 74 10              2758 	mov	a,#0x10
   08AE F0                 2759 	movx	@dptr,a
   08AF                    2760 00101$:
   08AF D0 03              2761 	pop	ar3
   08B1 D0 02              2762 	pop	ar2
   08B3 D0 83              2763 	pop	dph
   08B5 D0 82              2764 	pop	dpl
   08B7 D0 E0              2765 	pop	acc
   08B9 32                 2766 	reti
                           2767 ;	eliminated unneeded push/pop psw
                           2768 ;	eliminated unneeded push/pop b
                           2769 ;------------------------------------------------------------
                           2770 ;Allocation info for local variables in function 'ISR_Highspeed'
                           2771 ;------------------------------------------------------------
                           2772 ;------------------------------------------------------------
                           2773 ;HPSDR_firmware_1_0_0.c:357: void ISR_Highspeed(void) interrupt  
                           2774 ;	-----------------------------------------
                           2775 ;	 function ISR_Highspeed
                           2776 ;	-----------------------------------------
   08BA                    2777 _ISR_Highspeed:
   08BA C0 E0              2778 	push	acc
   08BC C0 82              2779 	push	dpl
   08BE C0 83              2780 	push	dph
   08C0 C0 02              2781 	push	ar2
   08C2 C0 03              2782 	push	ar3
                           2783 ;HPSDR_firmware_1_0_0.c:359: if (FX2_HIGHSPEED())
                           2784 ;     genAssign
   08C4 90 E6 80           2785 	mov	dptr,#_USBCS
   08C7 E0                 2786 	movx	a,@dptr
                           2787 ;     genAnd
                           2788 ;	Peephole 105	removed redundant mov
   08C8 FA                 2789 	mov	r2,a
                           2790 ;     genIfxJump
                           2791 ;	Peephole 111	removed ljmp by inverse jump logic
   08C9 30 E7 20           2792 	jnb	acc.7,00102$
   08CC                    2793 00106$:
                           2794 ;HPSDR_firmware_1_0_0.c:361: pConfigDscr = pHighSpeedConfigDscr;
                           2795 ;     genAssign
   08CC 90 22 0C           2796 	mov	dptr,#_pHighSpeedConfigDscr
   08CF E0                 2797 	movx	a,@dptr
   08D0 FA                 2798 	mov	r2,a
   08D1 A3                 2799 	inc	dptr
   08D2 E0                 2800 	movx	a,@dptr
   08D3 FB                 2801 	mov	r3,a
                           2802 ;     genAssign
   08D4 90 22 10           2803 	mov	dptr,#_pConfigDscr
   08D7 EA                 2804 	mov	a,r2
   08D8 F0                 2805 	movx	@dptr,a
   08D9 A3                 2806 	inc	dptr
   08DA EB                 2807 	mov	a,r3
   08DB F0                 2808 	movx	@dptr,a
                           2809 ;HPSDR_firmware_1_0_0.c:362: pOtherConfigDscr = pFullSpeedConfigDscr;
                           2810 ;     genAssign
   08DC 90 22 0E           2811 	mov	dptr,#_pFullSpeedConfigDscr
   08DF E0                 2812 	movx	a,@dptr
   08E0 FA                 2813 	mov	r2,a
   08E1 A3                 2814 	inc	dptr
   08E2 E0                 2815 	movx	a,@dptr
   08E3 FB                 2816 	mov	r3,a
                           2817 ;     genAssign
   08E4 90 22 12           2818 	mov	dptr,#_pOtherConfigDscr
   08E7 EA                 2819 	mov	a,r2
   08E8 F0                 2820 	movx	@dptr,a
   08E9 A3                 2821 	inc	dptr
   08EA EB                 2822 	mov	a,r3
   08EB F0                 2823 	movx	@dptr,a
   08EC                    2824 00102$:
                           2825 ;HPSDR_firmware_1_0_0.c:365: FX2_IRQ_CLEAR();
                           2826 ;     genAnd
   08EC 53 91 EF           2827 	anl	_EXIF,#0xEF
                           2828 ;HPSDR_firmware_1_0_0.c:366: FX2_CLEAR_USB_IRQ(bmHSGRANT);	// Clear HSGrant IRQ
                           2829 ;     genAssign
   08EF 90 E6 5D           2830 	mov	dptr,#_USBIRQ
   08F2 74 20              2831 	mov	a,#0x20
   08F4 F0                 2832 	movx	@dptr,a
   08F5                    2833 00103$:
   08F5 D0 03              2834 	pop	ar3
   08F7 D0 02              2835 	pop	ar2
   08F9 D0 83              2836 	pop	dph
   08FB D0 82              2837 	pop	dpl
   08FD D0 E0              2838 	pop	acc
   08FF 32                 2839 	reti
                           2840 ;	eliminated unneeded push/pop psw
                           2841 ;	eliminated unneeded push/pop b
                           2842 ;------------------------------------------------------------
                           2843 ;Allocation info for local variables in function 'ISR_Ep1out'
                           2844 ;------------------------------------------------------------
                           2845 ;------------------------------------------------------------
                           2846 ;HPSDR_firmware_1_0_0.c:369: void ISR_Ep1out(void) interrupt
                           2847 ;	-----------------------------------------
                           2848 ;	 function ISR_Ep1out
                           2849 ;	-----------------------------------------
   0900                    2850 _ISR_Ep1out:
   0900 C0 E0              2851 	push	acc
   0902 C0 82              2852 	push	dpl
   0904 C0 83              2853 	push	dph
                           2854 ;HPSDR_firmware_1_0_0.c:371: EP1HASDATA = TRUE;            // Set flag
                           2855 ;     genAssign
   0906 90 22 03           2856 	mov	dptr,#_EP1HASDATA
   0909 74 01              2857 	mov	a,#0x01
   090B F0                 2858 	movx	@dptr,a
                           2859 ;HPSDR_firmware_1_0_0.c:372: FX2_IRQ_CLEAR();
                           2860 ;     genAnd
   090C 53 91 EF           2861 	anl	_EXIF,#0xEF
                           2862 ;HPSDR_firmware_1_0_0.c:373: FX2_CLEAR_USB_EPIRQ(bmEP1OUT);	// Clear EP1OUT IRQ
                           2863 ;     genAssign
   090F 90 E6 5F           2864 	mov	dptr,#_EPIRQ
   0912 74 08              2865 	mov	a,#0x08
   0914 F0                 2866 	movx	@dptr,a
   0915                    2867 00101$:
   0915 D0 83              2868 	pop	dph
   0917 D0 82              2869 	pop	dpl
   0919 D0 E0              2870 	pop	acc
   091B 32                 2871 	reti
                           2872 ;	eliminated unneeded push/pop psw
                           2873 ;	eliminated unneeded push/pop b
                           2874 	.area CSEG    (CODE)
   091C                    2875 _EPCS_Offset_Lookup_Table:
   091C 00                 2876 	.db #0x00
   091D 01                 2877 	.db #0x01
   091E 02                 2878 	.db #0x02
   091F 02                 2879 	.db #0x02
   0920 03                 2880 	.db #0x03
   0921 03                 2881 	.db #0x03
   0922 04                 2882 	.db #0x04
   0923 04                 2883 	.db #0x04
   0924 05                 2884 	.db #0x05
   0925 05                 2885 	.db #0x05
                           2886 	.area XINIT   (CODE)
