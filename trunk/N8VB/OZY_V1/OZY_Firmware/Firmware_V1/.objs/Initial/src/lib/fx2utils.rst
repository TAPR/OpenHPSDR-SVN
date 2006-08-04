                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Fri Aug 04 15:56:57 2006
                              5 ;--------------------------------------------------------
                              6 	.module fx2utils
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _EIPX6
                             13 	.globl _EIPX5
                             14 	.globl _EIPX4
                             15 	.globl _PI2C
                             16 	.globl _PUSB
                             17 	.globl _EIEX6
                             18 	.globl _EIEX5
                             19 	.globl _EIEX4
                             20 	.globl _EI2C
                             21 	.globl _EIUSB
                             22 	.globl _SMOD1
                             23 	.globl _ERESI
                             24 	.globl _RESI
                             25 	.globl _INT6
                             26 	.globl _CY
                             27 	.globl _AC
                             28 	.globl _F0
                             29 	.globl _RS1
                             30 	.globl _RS0
                             31 	.globl _OV
                             32 	.globl _FL
                             33 	.globl _P
                             34 	.globl _TF2
                             35 	.globl _EXF2
                             36 	.globl _RCLK
                             37 	.globl _TCLK
                             38 	.globl _EXEN2
                             39 	.globl _TR2
                             40 	.globl _C_T2
                             41 	.globl _CP_RL2
                             42 	.globl _SM01
                             43 	.globl _SM11
                             44 	.globl _SM21
                             45 	.globl _REN1
                             46 	.globl _TB81
                             47 	.globl _RB81
                             48 	.globl _TI1
                             49 	.globl _RI1
                             50 	.globl _PS1
                             51 	.globl _PT2
                             52 	.globl _PS0
                             53 	.globl _PT1
                             54 	.globl _PX1
                             55 	.globl _PT0
                             56 	.globl _PX0
                             57 	.globl _EA
                             58 	.globl _ES1
                             59 	.globl _ET2
                             60 	.globl _ES0
                             61 	.globl _ET1
                             62 	.globl _EX1
                             63 	.globl _ET0
                             64 	.globl _EX0
                             65 	.globl _SM0
                             66 	.globl _SM1
                             67 	.globl _SM2
                             68 	.globl _REN
                             69 	.globl _TB8
                             70 	.globl _RB8
                             71 	.globl _TI
                             72 	.globl _RI
                             73 	.globl _TF1
                             74 	.globl _TR1
                             75 	.globl _TF0
                             76 	.globl _TR0
                             77 	.globl _IE1
                             78 	.globl _IT1
                             79 	.globl _IE0
                             80 	.globl _IT0
                             81 	.globl _SEL
                             82 	.globl _EIP
                             83 	.globl _B
                             84 	.globl _EIE
                             85 	.globl _ACC
                             86 	.globl _EICON
                             87 	.globl _PSW
                             88 	.globl _TH2
                             89 	.globl _TL2
                             90 	.globl _RCAP2H
                             91 	.globl _RCAP2L
                             92 	.globl _T2CON
                             93 	.globl _SBUF1
                             94 	.globl _SCON1
                             95 	.globl _GPIFSGLDATLNOX
                             96 	.globl _GPIFSGLDATLX
                             97 	.globl _GPIFSGLDATH
                             98 	.globl _GPIFTRIG
                             99 	.globl _EP01STAT
                            100 	.globl _IP
                            101 	.globl _OEE
                            102 	.globl _OED
                            103 	.globl _OEC
                            104 	.globl _OEB
                            105 	.globl _OEA
                            106 	.globl _IOE
                            107 	.globl _IOD
                            108 	.globl _AUTOPTRSETUP
                            109 	.globl _EP68FIFOFLGS
                            110 	.globl _EP24FIFOFLGS
                            111 	.globl _EP2468STAT
                            112 	.globl _IE
                            113 	.globl _INT4CLR
                            114 	.globl _INT2CLR
                            115 	.globl _IOC
                            116 	.globl _AUTODAT2
                            117 	.globl _AUTOPTRL2
                            118 	.globl _AUTOPTRH2
                            119 	.globl _AUTODAT1
                            120 	.globl _APTR1L
                            121 	.globl _APTR1H
                            122 	.globl _SBUF0
                            123 	.globl _SCON0
                            124 	.globl _MPAGE
                            125 	.globl _EXIF
                            126 	.globl _IOB
                            127 	.globl _CKCON
                            128 	.globl _TH1
                            129 	.globl _TH0
                            130 	.globl _TL1
                            131 	.globl _TL0
                            132 	.globl _TMOD
                            133 	.globl _TCON
                            134 	.globl _PCON
                            135 	.globl _DPS
                            136 	.globl _DPH1
                            137 	.globl _DPL1
                            138 	.globl _DPH
                            139 	.globl _DPL
                            140 	.globl _SP
                            141 	.globl _IOA
                            142 	.globl _EP8FIFOBUF
                            143 	.globl _EP6FIFOBUF
                            144 	.globl _EP4FIFOBUF
                            145 	.globl _EP2FIFOBUF
                            146 	.globl _EP1INBUF
                            147 	.globl _EP1OUTBUF
                            148 	.globl _EP0BUF
                            149 	.globl _CT4
                            150 	.globl _CT3
                            151 	.globl _CT2
                            152 	.globl _CT1
                            153 	.globl _USBTEST
                            154 	.globl _TESTCFG
                            155 	.globl _DBUG
                            156 	.globl _UDMACRCQUAL
                            157 	.globl _UDMACRCL
                            158 	.globl _UDMACRCH
                            159 	.globl _GPIFHOLDAMOUNT
                            160 	.globl _FLOWSTBHPERIOD
                            161 	.globl _FLOWSTBEDGE
                            162 	.globl _FLOWSTB
                            163 	.globl _FLOWHOLDOFF
                            164 	.globl _FLOWEQ1CTL
                            165 	.globl _FLOWEQ0CTL
                            166 	.globl _FLOWLOGIC
                            167 	.globl _FLOWSTATE
                            168 	.globl _GPIFABORT
                            169 	.globl _GPIFREADYSTAT
                            170 	.globl _GPIFREADYCFG
                            171 	.globl _XGPIFSGLDATLNOX
                            172 	.globl _XGPIFSGLDATLX
                            173 	.globl _XGPIFSGLDATH
                            174 	.globl _EP8GPIFTRIG
                            175 	.globl _EP8GPIFPFSTOP
                            176 	.globl _EP8GPIFFLGSEL
                            177 	.globl _EP6GPIFTRIG
                            178 	.globl _EP6GPIFPFSTOP
                            179 	.globl _EP6GPIFFLGSEL
                            180 	.globl _EP4GPIFTRIG
                            181 	.globl _EP4GPIFPFSTOP
                            182 	.globl _EP4GPIFFLGSEL
                            183 	.globl _EP2GPIFTRIG
                            184 	.globl _EP2GPIFPFSTOP
                            185 	.globl _EP2GPIFFLGSEL
                            186 	.globl _GPIFTCB0
                            187 	.globl _GPIFTCB1
                            188 	.globl _GPIFTCB2
                            189 	.globl _GPIFTCB3
                            190 	.globl _GPIFADRL
                            191 	.globl _GPIFADRH
                            192 	.globl _GPIFCTLCFG
                            193 	.globl _GPIFIDLECTL
                            194 	.globl _GPIFIDLECS
                            195 	.globl _GPIFWFSELECT
                            196 	.globl _SETUPDAT
                            197 	.globl _SUDPTRCTL
                            198 	.globl _SUDPTRL
                            199 	.globl _SUDPTRH
                            200 	.globl _EP8FIFOBCL
                            201 	.globl _EP8FIFOBCH
                            202 	.globl _EP6FIFOBCL
                            203 	.globl _EP6FIFOBCH
                            204 	.globl _EP4FIFOBCL
                            205 	.globl _EP4FIFOBCH
                            206 	.globl _EP2FIFOBCL
                            207 	.globl _EP2FIFOBCH
                            208 	.globl _EP8FIFOFLGS
                            209 	.globl _EP6FIFOFLGS
                            210 	.globl _EP4FIFOFLGS
                            211 	.globl _EP2FIFOFLGS
                            212 	.globl _EP8CS
                            213 	.globl _EP6CS
                            214 	.globl _EP4CS
                            215 	.globl _EP2CS
                            216 	.globl _EP1INCS
                            217 	.globl _EP1OUTCS
                            218 	.globl _EP0CS
                            219 	.globl _EP8BCL
                            220 	.globl _EP8BCH
                            221 	.globl _EP6BCL
                            222 	.globl _EP6BCH
                            223 	.globl _EP4BCL
                            224 	.globl _EP4BCH
                            225 	.globl _EP2BCL
                            226 	.globl _EP2BCH
                            227 	.globl _EP1INBC
                            228 	.globl _EP1OUTBC
                            229 	.globl _EP0BCL
                            230 	.globl _EP0BCH
                            231 	.globl _FNADDR
                            232 	.globl _MICROFRAME
                            233 	.globl _USBFRAMEL
                            234 	.globl _USBFRAMEH
                            235 	.globl _TOGCTL
                            236 	.globl _WAKEUPCS
                            237 	.globl _SUSPEND
                            238 	.globl _USBCS
                            239 	.globl _XAUTODAT2
                            240 	.globl _XAUTODAT1
                            241 	.globl _I2CTL
                            242 	.globl _I2DAT
                            243 	.globl _I2CS
                            244 	.globl _PORTECFG
                            245 	.globl _PORTCCFG
                            246 	.globl _PORTACFG
                            247 	.globl _INTSETUP
                            248 	.globl _INT4IVEC
                            249 	.globl _INT2IVEC
                            250 	.globl _CLRERRCNT
                            251 	.globl _ERRCNTLIM
                            252 	.globl _USBERRIRQ
                            253 	.globl _USBERRIE
                            254 	.globl _GPIFIRQ
                            255 	.globl _GPIFIE
                            256 	.globl _EPIRQ
                            257 	.globl _EPIE
                            258 	.globl _USBIRQ
                            259 	.globl _USBIE
                            260 	.globl _NAKIRQ
                            261 	.globl _NAKIE
                            262 	.globl _IBNIRQ
                            263 	.globl _IBNIE
                            264 	.globl _EP8FIFOIRQ
                            265 	.globl _EP8FIFOIE
                            266 	.globl _EP6FIFOIRQ
                            267 	.globl _EP6FIFOIE
                            268 	.globl _EP4FIFOIRQ
                            269 	.globl _EP4FIFOIE
                            270 	.globl _EP2FIFOIRQ
                            271 	.globl _EP2FIFOIE
                            272 	.globl _OUTPKTEND
                            273 	.globl _INPKTEND
                            274 	.globl _EP8ISOINPKTS
                            275 	.globl _EP6ISOINPKTS
                            276 	.globl _EP4ISOINPKTS
                            277 	.globl _EP2ISOINPKTS
                            278 	.globl _EP8FIFOPFL
                            279 	.globl _EP8FIFOPFH
                            280 	.globl _EP6FIFOPFL
                            281 	.globl _EP6FIFOPFH
                            282 	.globl _EP4FIFOPFL
                            283 	.globl _EP4FIFOPFH
                            284 	.globl _EP2FIFOPFL
                            285 	.globl _EP2FIFOPFH
                            286 	.globl _EP8AUTOINLENL
                            287 	.globl _EP8AUTOINLENH
                            288 	.globl _EP6AUTOINLENL
                            289 	.globl _EP6AUTOINLENH
                            290 	.globl _EP4AUTOINLENL
                            291 	.globl _EP4AUTOINLENH
                            292 	.globl _EP2AUTOINLENL
                            293 	.globl _EP2AUTOINLENH
                            294 	.globl _EP8FIFOCFG
                            295 	.globl _EP6FIFOCFG
                            296 	.globl _EP4FIFOCFG
                            297 	.globl _EP2FIFOCFG
                            298 	.globl _EP8CFG
                            299 	.globl _EP6CFG
                            300 	.globl _EP4CFG
                            301 	.globl _EP2CFG
                            302 	.globl _EP1INCFG
                            303 	.globl _EP1OUTCFG
                            304 	.globl _REVCTL
                            305 	.globl _REVID
                            306 	.globl _FIFOPINPOLAR
                            307 	.globl _UART230
                            308 	.globl _BPADDRL
                            309 	.globl _BPADDRH
                            310 	.globl _BREAKPT
                            311 	.globl _FIFORESET
                            312 	.globl _PINFLAGSCD
                            313 	.globl _PINFLAGSAB
                            314 	.globl _IFCONFIG
                            315 	.globl _CPUCS
                            316 	.globl _RES_WAVEDATA_END
                            317 	.globl _GPIF_WAVE_DATA
                            318 	.globl _fx2_stall_ep0
                            319 	.globl _fx2_reset_data_toggle
                            320 	.globl _fx2_renumerate
                            321 ;--------------------------------------------------------
                            322 ; special function registers
                            323 ;--------------------------------------------------------
                            324 	.area RSEG    (DATA)
                    0080    325 _IOA	=	0x0080
                    0081    326 _SP	=	0x0081
                    0082    327 _DPL	=	0x0082
                    0083    328 _DPH	=	0x0083
                    0084    329 _DPL1	=	0x0084
                    0085    330 _DPH1	=	0x0085
                    0086    331 _DPS	=	0x0086
                    0087    332 _PCON	=	0x0087
                    0088    333 _TCON	=	0x0088
                    0089    334 _TMOD	=	0x0089
                    008A    335 _TL0	=	0x008a
                    008B    336 _TL1	=	0x008b
                    008C    337 _TH0	=	0x008c
                    008D    338 _TH1	=	0x008d
                    008E    339 _CKCON	=	0x008e
                    0090    340 _IOB	=	0x0090
                    0091    341 _EXIF	=	0x0091
                    0092    342 _MPAGE	=	0x0092
                    0098    343 _SCON0	=	0x0098
                    0099    344 _SBUF0	=	0x0099
                    009A    345 _APTR1H	=	0x009a
                    009B    346 _APTR1L	=	0x009b
                    009C    347 _AUTODAT1	=	0x009c
                    009D    348 _AUTOPTRH2	=	0x009d
                    009E    349 _AUTOPTRL2	=	0x009e
                    009F    350 _AUTODAT2	=	0x009f
                    00A0    351 _IOC	=	0x00a0
                    00A1    352 _INT2CLR	=	0x00a1
                    00A2    353 _INT4CLR	=	0x00a2
                    00A8    354 _IE	=	0x00a8
                    00AA    355 _EP2468STAT	=	0x00aa
                    00AB    356 _EP24FIFOFLGS	=	0x00ab
                    00AC    357 _EP68FIFOFLGS	=	0x00ac
                    00AF    358 _AUTOPTRSETUP	=	0x00af
                    00B0    359 _IOD	=	0x00b0
                    00B1    360 _IOE	=	0x00b1
                    00B2    361 _OEA	=	0x00b2
                    00B3    362 _OEB	=	0x00b3
                    00B4    363 _OEC	=	0x00b4
                    00B5    364 _OED	=	0x00b5
                    00B6    365 _OEE	=	0x00b6
                    00B8    366 _IP	=	0x00b8
                    00BA    367 _EP01STAT	=	0x00ba
                    00BB    368 _GPIFTRIG	=	0x00bb
                    00BD    369 _GPIFSGLDATH	=	0x00bd
                    00BE    370 _GPIFSGLDATLX	=	0x00be
                    00BF    371 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    372 _SCON1	=	0x00c0
                    00C1    373 _SBUF1	=	0x00c1
                    00C8    374 _T2CON	=	0x00c8
                    00CA    375 _RCAP2L	=	0x00ca
                    00CB    376 _RCAP2H	=	0x00cb
                    00CC    377 _TL2	=	0x00cc
                    00CD    378 _TH2	=	0x00cd
                    00D0    379 _PSW	=	0x00d0
                    00D8    380 _EICON	=	0x00d8
                    00E0    381 _ACC	=	0x00e0
                    00E8    382 _EIE	=	0x00e8
                    00F0    383 _B	=	0x00f0
                    00F8    384 _EIP	=	0x00f8
                            385 ;--------------------------------------------------------
                            386 ; special function bits 
                            387 ;--------------------------------------------------------
                            388 	.area RSEG    (DATA)
                    0086    389 _SEL	=	0x0086
                    0088    390 _IT0	=	0x0088
                    0089    391 _IE0	=	0x0089
                    008A    392 _IT1	=	0x008a
                    008B    393 _IE1	=	0x008b
                    008C    394 _TR0	=	0x008c
                    008D    395 _TF0	=	0x008d
                    008E    396 _TR1	=	0x008e
                    008F    397 _TF1	=	0x008f
                    0098    398 _RI	=	0x0098
                    0099    399 _TI	=	0x0099
                    009A    400 _RB8	=	0x009a
                    009B    401 _TB8	=	0x009b
                    009C    402 _REN	=	0x009c
                    009D    403 _SM2	=	0x009d
                    009E    404 _SM1	=	0x009e
                    009F    405 _SM0	=	0x009f
                    00A8    406 _EX0	=	0x00a8
                    00A9    407 _ET0	=	0x00a9
                    00AA    408 _EX1	=	0x00aa
                    00AB    409 _ET1	=	0x00ab
                    00AC    410 _ES0	=	0x00ac
                    00AD    411 _ET2	=	0x00ad
                    00AE    412 _ES1	=	0x00ae
                    00AF    413 _EA	=	0x00af
                    00B8    414 _PX0	=	0x00b8
                    00B9    415 _PT0	=	0x00b9
                    00BA    416 _PX1	=	0x00ba
                    00BB    417 _PT1	=	0x00bb
                    00BC    418 _PS0	=	0x00bc
                    00BD    419 _PT2	=	0x00bd
                    00BE    420 _PS1	=	0x00be
                    00C0    421 _RI1	=	0x00c0
                    00C1    422 _TI1	=	0x00c1
                    00C2    423 _RB81	=	0x00c2
                    00C3    424 _TB81	=	0x00c3
                    00C4    425 _REN1	=	0x00c4
                    00C5    426 _SM21	=	0x00c5
                    00C6    427 _SM11	=	0x00c6
                    00C7    428 _SM01	=	0x00c7
                    00C8    429 _CP_RL2	=	0x00c8
                    00C9    430 _C_T2	=	0x00c9
                    00CA    431 _TR2	=	0x00ca
                    00CB    432 _EXEN2	=	0x00cb
                    00CC    433 _TCLK	=	0x00cc
                    00CD    434 _RCLK	=	0x00cd
                    00CE    435 _EXF2	=	0x00ce
                    00CF    436 _TF2	=	0x00cf
                    00D0    437 _P	=	0x00d0
                    00D1    438 _FL	=	0x00d1
                    00D2    439 _OV	=	0x00d2
                    00D3    440 _RS0	=	0x00d3
                    00D4    441 _RS1	=	0x00d4
                    00D5    442 _F0	=	0x00d5
                    00D6    443 _AC	=	0x00d6
                    00D7    444 _CY	=	0x00d7
                    00DB    445 _INT6	=	0x00db
                    00DC    446 _RESI	=	0x00dc
                    00DD    447 _ERESI	=	0x00dd
                    00DF    448 _SMOD1	=	0x00df
                    00E8    449 _EIUSB	=	0x00e8
                    00E9    450 _EI2C	=	0x00e9
                    00EA    451 _EIEX4	=	0x00ea
                    00EB    452 _EIEX5	=	0x00eb
                    00EC    453 _EIEX6	=	0x00ec
                    00F8    454 _PUSB	=	0x00f8
                    00F9    455 _PI2C	=	0x00f9
                    00FA    456 _EIPX4	=	0x00fa
                    00FB    457 _EIPX5	=	0x00fb
                    00FC    458 _EIPX6	=	0x00fc
                            459 ;--------------------------------------------------------
                            460 ; overlayable register banks 
                            461 ;--------------------------------------------------------
                            462 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     463 	.ds 8
                            464 ;--------------------------------------------------------
                            465 ; internal ram data
                            466 ;--------------------------------------------------------
                            467 	.area DSEG    (DATA)
                            468 ;--------------------------------------------------------
                            469 ; overlayable items in internal ram 
                            470 ;--------------------------------------------------------
                            471 	.area	OSEG    (OVR,DATA)
                            472 ;--------------------------------------------------------
                            473 ; indirectly addressable internal ram data
                            474 ;--------------------------------------------------------
                            475 	.area ISEG    (DATA)
                            476 ;--------------------------------------------------------
                            477 ; bit data
                            478 ;--------------------------------------------------------
                            479 	.area BSEG    (BIT)
                            480 ;--------------------------------------------------------
                            481 ; paged external ram data
                            482 ;--------------------------------------------------------
                            483 	.area PSEG    (PAG,XDATA)
                            484 ;--------------------------------------------------------
                            485 ; external ram data
                            486 ;--------------------------------------------------------
                            487 	.area XSEG    (XDATA)
                    E400    488 _GPIF_WAVE_DATA	=	0xe400
                    E480    489 _RES_WAVEDATA_END	=	0xe480
                    E600    490 _CPUCS	=	0xe600
                    E601    491 _IFCONFIG	=	0xe601
                    E602    492 _PINFLAGSAB	=	0xe602
                    E603    493 _PINFLAGSCD	=	0xe603
                    E604    494 _FIFORESET	=	0xe604
                    E605    495 _BREAKPT	=	0xe605
                    E606    496 _BPADDRH	=	0xe606
                    E607    497 _BPADDRL	=	0xe607
                    E608    498 _UART230	=	0xe608
                    E609    499 _FIFOPINPOLAR	=	0xe609
                    E60A    500 _REVID	=	0xe60a
                    E60B    501 _REVCTL	=	0xe60b
                    E610    502 _EP1OUTCFG	=	0xe610
                    E611    503 _EP1INCFG	=	0xe611
                    E612    504 _EP2CFG	=	0xe612
                    E613    505 _EP4CFG	=	0xe613
                    E614    506 _EP6CFG	=	0xe614
                    E615    507 _EP8CFG	=	0xe615
                    E618    508 _EP2FIFOCFG	=	0xe618
                    E619    509 _EP4FIFOCFG	=	0xe619
                    E61A    510 _EP6FIFOCFG	=	0xe61a
                    E61B    511 _EP8FIFOCFG	=	0xe61b
                    E620    512 _EP2AUTOINLENH	=	0xe620
                    E621    513 _EP2AUTOINLENL	=	0xe621
                    E622    514 _EP4AUTOINLENH	=	0xe622
                    E623    515 _EP4AUTOINLENL	=	0xe623
                    E624    516 _EP6AUTOINLENH	=	0xe624
                    E625    517 _EP6AUTOINLENL	=	0xe625
                    E626    518 _EP8AUTOINLENH	=	0xe626
                    E627    519 _EP8AUTOINLENL	=	0xe627
                    E630    520 _EP2FIFOPFH	=	0xe630
                    E631    521 _EP2FIFOPFL	=	0xe631
                    E632    522 _EP4FIFOPFH	=	0xe632
                    E633    523 _EP4FIFOPFL	=	0xe633
                    E634    524 _EP6FIFOPFH	=	0xe634
                    E635    525 _EP6FIFOPFL	=	0xe635
                    E636    526 _EP8FIFOPFH	=	0xe636
                    E637    527 _EP8FIFOPFL	=	0xe637
                    E640    528 _EP2ISOINPKTS	=	0xe640
                    E641    529 _EP4ISOINPKTS	=	0xe641
                    E642    530 _EP6ISOINPKTS	=	0xe642
                    E643    531 _EP8ISOINPKTS	=	0xe643
                    E648    532 _INPKTEND	=	0xe648
                    E649    533 _OUTPKTEND	=	0xe649
                    E650    534 _EP2FIFOIE	=	0xe650
                    E651    535 _EP2FIFOIRQ	=	0xe651
                    E652    536 _EP4FIFOIE	=	0xe652
                    E653    537 _EP4FIFOIRQ	=	0xe653
                    E654    538 _EP6FIFOIE	=	0xe654
                    E655    539 _EP6FIFOIRQ	=	0xe655
                    E656    540 _EP8FIFOIE	=	0xe656
                    E657    541 _EP8FIFOIRQ	=	0xe657
                    E658    542 _IBNIE	=	0xe658
                    E659    543 _IBNIRQ	=	0xe659
                    E65A    544 _NAKIE	=	0xe65a
                    E65B    545 _NAKIRQ	=	0xe65b
                    E65C    546 _USBIE	=	0xe65c
                    E65D    547 _USBIRQ	=	0xe65d
                    E65E    548 _EPIE	=	0xe65e
                    E65F    549 _EPIRQ	=	0xe65f
                    E660    550 _GPIFIE	=	0xe660
                    E661    551 _GPIFIRQ	=	0xe661
                    E662    552 _USBERRIE	=	0xe662
                    E663    553 _USBERRIRQ	=	0xe663
                    E664    554 _ERRCNTLIM	=	0xe664
                    E665    555 _CLRERRCNT	=	0xe665
                    E666    556 _INT2IVEC	=	0xe666
                    E667    557 _INT4IVEC	=	0xe667
                    E668    558 _INTSETUP	=	0xe668
                    E670    559 _PORTACFG	=	0xe670
                    E671    560 _PORTCCFG	=	0xe671
                    E672    561 _PORTECFG	=	0xe672
                    E678    562 _I2CS	=	0xe678
                    E679    563 _I2DAT	=	0xe679
                    E67A    564 _I2CTL	=	0xe67a
                    E67B    565 _XAUTODAT1	=	0xe67b
                    E67C    566 _XAUTODAT2	=	0xe67c
                    E680    567 _USBCS	=	0xe680
                    E681    568 _SUSPEND	=	0xe681
                    E682    569 _WAKEUPCS	=	0xe682
                    E683    570 _TOGCTL	=	0xe683
                    E684    571 _USBFRAMEH	=	0xe684
                    E685    572 _USBFRAMEL	=	0xe685
                    E686    573 _MICROFRAME	=	0xe686
                    E687    574 _FNADDR	=	0xe687
                    E68A    575 _EP0BCH	=	0xe68a
                    E68B    576 _EP0BCL	=	0xe68b
                    E68D    577 _EP1OUTBC	=	0xe68d
                    E68F    578 _EP1INBC	=	0xe68f
                    E690    579 _EP2BCH	=	0xe690
                    E691    580 _EP2BCL	=	0xe691
                    E694    581 _EP4BCH	=	0xe694
                    E695    582 _EP4BCL	=	0xe695
                    E698    583 _EP6BCH	=	0xe698
                    E699    584 _EP6BCL	=	0xe699
                    E69C    585 _EP8BCH	=	0xe69c
                    E69D    586 _EP8BCL	=	0xe69d
                    E6A0    587 _EP0CS	=	0xe6a0
                    E6A1    588 _EP1OUTCS	=	0xe6a1
                    E6A2    589 _EP1INCS	=	0xe6a2
                    E6A3    590 _EP2CS	=	0xe6a3
                    E6A4    591 _EP4CS	=	0xe6a4
                    E6A5    592 _EP6CS	=	0xe6a5
                    E6A6    593 _EP8CS	=	0xe6a6
                    E6A7    594 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    595 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    596 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    597 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    598 _EP2FIFOBCH	=	0xe6ab
                    E6AC    599 _EP2FIFOBCL	=	0xe6ac
                    E6AD    600 _EP4FIFOBCH	=	0xe6ad
                    E6AE    601 _EP4FIFOBCL	=	0xe6ae
                    E6AF    602 _EP6FIFOBCH	=	0xe6af
                    E6B0    603 _EP6FIFOBCL	=	0xe6b0
                    E6B1    604 _EP8FIFOBCH	=	0xe6b1
                    E6B2    605 _EP8FIFOBCL	=	0xe6b2
                    E6B3    606 _SUDPTRH	=	0xe6b3
                    E6B4    607 _SUDPTRL	=	0xe6b4
                    E6B5    608 _SUDPTRCTL	=	0xe6b5
                    E6B8    609 _SETUPDAT	=	0xe6b8
                    E6C0    610 _GPIFWFSELECT	=	0xe6c0
                    E6C1    611 _GPIFIDLECS	=	0xe6c1
                    E6C2    612 _GPIFIDLECTL	=	0xe6c2
                    E6C3    613 _GPIFCTLCFG	=	0xe6c3
                    E6C4    614 _GPIFADRH	=	0xe6c4
                    E6C5    615 _GPIFADRL	=	0xe6c5
                    E6CE    616 _GPIFTCB3	=	0xe6ce
                    E6CF    617 _GPIFTCB2	=	0xe6cf
                    E6D0    618 _GPIFTCB1	=	0xe6d0
                    E6D1    619 _GPIFTCB0	=	0xe6d1
                    E6D2    620 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    621 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    622 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    623 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    624 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    625 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    626 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    627 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    628 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    629 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    630 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    631 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    632 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    633 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    634 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    635 _GPIFREADYCFG	=	0xe6f3
                    E6F4    636 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    637 _GPIFABORT	=	0xe6f5
                    E6C6    638 _FLOWSTATE	=	0xe6c6
                    E6C7    639 _FLOWLOGIC	=	0xe6c7
                    E6C8    640 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    641 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    642 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    643 _FLOWSTB	=	0xe6cb
                    E6CC    644 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    645 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    646 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    647 _UDMACRCH	=	0xe67d
                    E67E    648 _UDMACRCL	=	0xe67e
                    E67F    649 _UDMACRCQUAL	=	0xe67f
                    E6F8    650 _DBUG	=	0xe6f8
                    E6F9    651 _TESTCFG	=	0xe6f9
                    E6FA    652 _USBTEST	=	0xe6fa
                    E6FB    653 _CT1	=	0xe6fb
                    E6FC    654 _CT2	=	0xe6fc
                    E6FD    655 _CT3	=	0xe6fd
                    E6FE    656 _CT4	=	0xe6fe
                    E740    657 _EP0BUF	=	0xe740
                    E780    658 _EP1OUTBUF	=	0xe780
                    E7C0    659 _EP1INBUF	=	0xe7c0
                    F000    660 _EP2FIFOBUF	=	0xf000
                    F400    661 _EP4FIFOBUF	=	0xf400
                    F800    662 _EP6FIFOBUF	=	0xf800
                    FC00    663 _EP8FIFOBUF	=	0xfc00
                            664 ;--------------------------------------------------------
                            665 ; external initialized ram data
                            666 ;--------------------------------------------------------
                            667 	.area CSEG    (CODE)
                            668 	.area GSINIT0 (CODE)
                            669 	.area GSINIT1 (CODE)
                            670 	.area GSINIT2 (CODE)
                            671 	.area GSINIT3 (CODE)
                            672 	.area GSINIT4 (CODE)
                            673 	.area GSINIT5 (CODE)
                            674 ;--------------------------------------------------------
                            675 ; global & static initialisations
                            676 ;--------------------------------------------------------
                            677 	.area CSEG    (CODE)
                            678 	.area GSINIT  (CODE)
                            679 	.area GSFINAL (CODE)
                            680 	.area GSINIT  (CODE)
                            681 ;--------------------------------------------------------
                            682 ; Home
                            683 ;--------------------------------------------------------
                            684 	.area HOME    (CODE)
                            685 	.area CSEG    (CODE)
                            686 ;--------------------------------------------------------
                            687 ; code
                            688 ;--------------------------------------------------------
                            689 	.area CSEG    (CODE)
                            690 ;------------------------------------------------------------
                            691 ;Allocation info for local variables in function 'fx2_stall_ep0'
                            692 ;------------------------------------------------------------
                            693 ;------------------------------------------------------------
                            694 ;Initial/src/lib/fx2utils.c:31: fx2_stall_ep0 (void)
                            695 ;	-----------------------------------------
                            696 ;	 function fx2_stall_ep0
                            697 ;	-----------------------------------------
   06B8                     698 _fx2_stall_ep0:
                    0002    699 	ar2 = 0x02
                    0003    700 	ar3 = 0x03
                    0004    701 	ar4 = 0x04
                    0005    702 	ar5 = 0x05
                    0006    703 	ar6 = 0x06
                    0007    704 	ar7 = 0x07
                    0000    705 	ar0 = 0x00
                    0001    706 	ar1 = 0x01
                            707 ;Initial/src/lib/fx2utils.c:33: EP0CS |= bmEPSTALL;
                            708 ;     genAssign
                            709 ;     genOr
                            710 ;	Peephole 248.a	optimized or to xdata
   06B8 90 E6 A0            711 	mov	dptr,#_EP0CS
   06BB E0                  712 	movx	a,@dptr
   06BC FA                  713 	mov	r2,a
   06BD 44 01               714 	orl	a,#0x01
   06BF F0                  715 	movx	@dptr,a
   06C0                     716 00101$:
   06C0 22                  717 	ret
                            718 ;------------------------------------------------------------
                            719 ;Allocation info for local variables in function 'fx2_reset_data_toggle'
                            720 ;------------------------------------------------------------
                            721 ;ep                        Allocated to registers r2 
                            722 ;------------------------------------------------------------
                            723 ;Initial/src/lib/fx2utils.c:37: fx2_reset_data_toggle (unsigned char ep)
                            724 ;	-----------------------------------------
                            725 ;	 function fx2_reset_data_toggle
                            726 ;	-----------------------------------------
   06C1                     727 _fx2_reset_data_toggle:
                            728 ;     genReceive
   06C1 AA 82               729 	mov	r2,dpl
                            730 ;Initial/src/lib/fx2utils.c:39: TOGCTL = ((ep & 0x80) >> 3 | (ep & 0x0f));
                            731 ;     genAnd
   06C3 74 80               732 	mov	a,#0x80
   06C5 5A                  733 	anl	a,r2
                            734 ;     genRightShift
                            735 ;     genRightShiftLiteral
                            736 ;     genrshOne
                            737 ;	Peephole 105	removed redundant mov
   06C6 FB                  738 	mov	r3,a
   06C7 C4                  739 	swap	a
   06C8 23                  740 	rl	a
   06C9 54 1F               741 	anl	a,#0x1f
   06CB FB                  742 	mov	r3,a
                            743 ;     genAnd
   06CC 74 0F               744 	mov	a,#0x0F
   06CE 5A                  745 	anl	a,r2
                            746 ;     genOr
   06CF 90 E6 83            747 	mov	dptr,#_TOGCTL
   06D2 4B                  748 	orl	a,r3
   06D3 F0                  749 	movx	@dptr,a
                            750 ;Initial/src/lib/fx2utils.c:40: TOGCTL |= bmRESETTOGGLE;
                            751 ;     genAssign
                            752 ;     genOr
                            753 ;	Peephole 248.a	optimized or to xdata
   06D4 90 E6 83            754 	mov	dptr,#_TOGCTL
   06D7 E0                  755 	movx	a,@dptr
   06D8 FA                  756 	mov	r2,a
   06D9 44 20               757 	orl	a,#0x20
   06DB F0                  758 	movx	@dptr,a
   06DC                     759 00101$:
   06DC 22                  760 	ret
                            761 ;------------------------------------------------------------
                            762 ;Allocation info for local variables in function 'fx2_renumerate'
                            763 ;------------------------------------------------------------
                            764 ;------------------------------------------------------------
                            765 ;Initial/src/lib/fx2utils.c:44: fx2_renumerate (void)
                            766 ;	-----------------------------------------
                            767 ;	 function fx2_renumerate
                            768 ;	-----------------------------------------
   06DD                     769 _fx2_renumerate:
                            770 ;Initial/src/lib/fx2utils.c:46: USBCS |= bmDISCON | bmRENUM;
                            771 ;     genAssign
                            772 ;     genOr
                            773 ;	Peephole 248.a	optimized or to xdata
   06DD 90 E6 80            774 	mov	dptr,#_USBCS
   06E0 E0                  775 	movx	a,@dptr
   06E1 FA                  776 	mov	r2,a
   06E2 44 0A               777 	orl	a,#0x0A
   06E4 F0                  778 	movx	@dptr,a
                            779 ;Initial/src/lib/fx2utils.c:48: mdelay (250);
                            780 ;     genCall
                            781 ;	Peephole 182.b	used 16 bit load of dptr
   06E5 90 00 FA            782 	mov	dptr,#0x00FA
   06E8 12 06 A5            783 	lcall	_mdelay
                            784 ;Initial/src/lib/fx2utils.c:50: USBIRQ = 0xff;		// clear any pending USB irqs...
                            785 ;     genAssign
   06EB 90 E6 5D            786 	mov	dptr,#_USBIRQ
   06EE 74 FF               787 	mov	a,#0xFF
   06F0 F0                  788 	movx	@dptr,a
                            789 ;Initial/src/lib/fx2utils.c:51: EPIRQ =  0xff;		//   they're from before the renumeration
                            790 ;     genAssign
   06F1 90 E6 5F            791 	mov	dptr,#_EPIRQ
   06F4 74 FF               792 	mov	a,#0xFF
   06F6 F0                  793 	movx	@dptr,a
                            794 ;Initial/src/lib/fx2utils.c:53: EXIF &= ~bmEXIF_USBINT;
                            795 ;     genAnd
   06F7 53 91 EF            796 	anl	_EXIF,#0xEF
                            797 ;Initial/src/lib/fx2utils.c:55: USBCS &= ~bmDISCON;		// reconnect USB
                            798 ;     genAssign
                            799 ;     genAnd
                            800 ;	Peephole 248.b	optimized and to xdata
   06FA 90 E6 80            801 	mov	dptr,#_USBCS
   06FD E0                  802 	movx	a,@dptr
   06FE FA                  803 	mov	r2,a
   06FF 54 F7               804 	anl	a,#0xF7
   0701 F0                  805 	movx	@dptr,a
   0702                     806 00101$:
   0702 22                  807 	ret
                            808 	.area CSEG    (CODE)
