                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Tue Jul 11 16:41:58 2006
                              5 ;--------------------------------------------------------
                              6 	.module hpsdr_common
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _bitALTERA_DCLK
                             13 	.globl _bitALTERA_DATA0
                             14 	.globl _bitS_IN
                             15 	.globl _bitS_OUT
                             16 	.globl _bitS_CLK
                             17 	.globl _EIPX6
                             18 	.globl _EIPX5
                             19 	.globl _EIPX4
                             20 	.globl _PI2C
                             21 	.globl _PUSB
                             22 	.globl _EIEX6
                             23 	.globl _EIEX5
                             24 	.globl _EIEX4
                             25 	.globl _EI2C
                             26 	.globl _EIUSB
                             27 	.globl _SMOD1
                             28 	.globl _ERESI
                             29 	.globl _RESI
                             30 	.globl _INT6
                             31 	.globl _CY
                             32 	.globl _AC
                             33 	.globl _F0
                             34 	.globl _RS1
                             35 	.globl _RS0
                             36 	.globl _OV
                             37 	.globl _FL
                             38 	.globl _P
                             39 	.globl _TF2
                             40 	.globl _EXF2
                             41 	.globl _RCLK
                             42 	.globl _TCLK
                             43 	.globl _EXEN2
                             44 	.globl _TR2
                             45 	.globl _C_T2
                             46 	.globl _CP_RL2
                             47 	.globl _SM01
                             48 	.globl _SM11
                             49 	.globl _SM21
                             50 	.globl _REN1
                             51 	.globl _TB81
                             52 	.globl _RB81
                             53 	.globl _TI1
                             54 	.globl _RI1
                             55 	.globl _PS1
                             56 	.globl _PT2
                             57 	.globl _PS0
                             58 	.globl _PT1
                             59 	.globl _PX1
                             60 	.globl _PT0
                             61 	.globl _PX0
                             62 	.globl _EA
                             63 	.globl _ES1
                             64 	.globl _ET2
                             65 	.globl _ES0
                             66 	.globl _ET1
                             67 	.globl _EX1
                             68 	.globl _ET0
                             69 	.globl _EX0
                             70 	.globl _SM0
                             71 	.globl _SM1
                             72 	.globl _SM2
                             73 	.globl _REN
                             74 	.globl _TB8
                             75 	.globl _RB8
                             76 	.globl _TI
                             77 	.globl _RI
                             78 	.globl _TF1
                             79 	.globl _TR1
                             80 	.globl _TF0
                             81 	.globl _TR0
                             82 	.globl _IE1
                             83 	.globl _IT1
                             84 	.globl _IE0
                             85 	.globl _IT0
                             86 	.globl _SEL
                             87 	.globl _EIP
                             88 	.globl _B
                             89 	.globl _EIE
                             90 	.globl _ACC
                             91 	.globl _EICON
                             92 	.globl _PSW
                             93 	.globl _TH2
                             94 	.globl _TL2
                             95 	.globl _RCAP2H
                             96 	.globl _RCAP2L
                             97 	.globl _T2CON
                             98 	.globl _SBUF1
                             99 	.globl _SCON1
                            100 	.globl _GPIFSGLDATLNOX
                            101 	.globl _GPIFSGLDATLX
                            102 	.globl _GPIFSGLDATH
                            103 	.globl _GPIFTRIG
                            104 	.globl _EP01STAT
                            105 	.globl _IP
                            106 	.globl _OEE
                            107 	.globl _OED
                            108 	.globl _OEC
                            109 	.globl _OEB
                            110 	.globl _OEA
                            111 	.globl _IOE
                            112 	.globl _IOD
                            113 	.globl _AUTOPTRSETUP
                            114 	.globl _EP68FIFOFLGS
                            115 	.globl _EP24FIFOFLGS
                            116 	.globl _EP2468STAT
                            117 	.globl _IE
                            118 	.globl _INT4CLR
                            119 	.globl _INT2CLR
                            120 	.globl _IOC
                            121 	.globl _AUTODAT2
                            122 	.globl _AUTOPTRL2
                            123 	.globl _AUTOPTRH2
                            124 	.globl _AUTODAT1
                            125 	.globl _APTR1L
                            126 	.globl _APTR1H
                            127 	.globl _SBUF0
                            128 	.globl _SCON0
                            129 	.globl _MPAGE
                            130 	.globl _EXIF
                            131 	.globl _IOB
                            132 	.globl _CKCON
                            133 	.globl _TH1
                            134 	.globl _TH0
                            135 	.globl _TL1
                            136 	.globl _TL0
                            137 	.globl _TMOD
                            138 	.globl _TCON
                            139 	.globl _PCON
                            140 	.globl _DPS
                            141 	.globl _DPH1
                            142 	.globl _DPL1
                            143 	.globl _DPH
                            144 	.globl _DPL
                            145 	.globl _SP
                            146 	.globl _IOA
                            147 	.globl _EP8FIFOBUF
                            148 	.globl _EP6FIFOBUF
                            149 	.globl _EP4FIFOBUF
                            150 	.globl _EP2FIFOBUF
                            151 	.globl _EP1INBUF
                            152 	.globl _EP1OUTBUF
                            153 	.globl _EP0BUF
                            154 	.globl _CT4
                            155 	.globl _CT3
                            156 	.globl _CT2
                            157 	.globl _CT1
                            158 	.globl _USBTEST
                            159 	.globl _TESTCFG
                            160 	.globl _DBUG
                            161 	.globl _UDMACRCQUAL
                            162 	.globl _UDMACRCL
                            163 	.globl _UDMACRCH
                            164 	.globl _GPIFHOLDAMOUNT
                            165 	.globl _FLOWSTBHPERIOD
                            166 	.globl _FLOWSTBEDGE
                            167 	.globl _FLOWSTB
                            168 	.globl _FLOWHOLDOFF
                            169 	.globl _FLOWEQ1CTL
                            170 	.globl _FLOWEQ0CTL
                            171 	.globl _FLOWLOGIC
                            172 	.globl _FLOWSTATE
                            173 	.globl _GPIFABORT
                            174 	.globl _GPIFREADYSTAT
                            175 	.globl _GPIFREADYCFG
                            176 	.globl _XGPIFSGLDATLNOX
                            177 	.globl _XGPIFSGLDATLX
                            178 	.globl _XGPIFSGLDATH
                            179 	.globl _EP8GPIFTRIG
                            180 	.globl _EP8GPIFPFSTOP
                            181 	.globl _EP8GPIFFLGSEL
                            182 	.globl _EP6GPIFTRIG
                            183 	.globl _EP6GPIFPFSTOP
                            184 	.globl _EP6GPIFFLGSEL
                            185 	.globl _EP4GPIFTRIG
                            186 	.globl _EP4GPIFPFSTOP
                            187 	.globl _EP4GPIFFLGSEL
                            188 	.globl _EP2GPIFTRIG
                            189 	.globl _EP2GPIFPFSTOP
                            190 	.globl _EP2GPIFFLGSEL
                            191 	.globl _GPIFTCB0
                            192 	.globl _GPIFTCB1
                            193 	.globl _GPIFTCB2
                            194 	.globl _GPIFTCB3
                            195 	.globl _GPIFADRL
                            196 	.globl _GPIFADRH
                            197 	.globl _GPIFCTLCFG
                            198 	.globl _GPIFIDLECTL
                            199 	.globl _GPIFIDLECS
                            200 	.globl _GPIFWFSELECT
                            201 	.globl _SETUPDAT
                            202 	.globl _SUDPTRCTL
                            203 	.globl _SUDPTRL
                            204 	.globl _SUDPTRH
                            205 	.globl _EP8FIFOBCL
                            206 	.globl _EP8FIFOBCH
                            207 	.globl _EP6FIFOBCL
                            208 	.globl _EP6FIFOBCH
                            209 	.globl _EP4FIFOBCL
                            210 	.globl _EP4FIFOBCH
                            211 	.globl _EP2FIFOBCL
                            212 	.globl _EP2FIFOBCH
                            213 	.globl _EP8FIFOFLGS
                            214 	.globl _EP6FIFOFLGS
                            215 	.globl _EP4FIFOFLGS
                            216 	.globl _EP2FIFOFLGS
                            217 	.globl _EP8CS
                            218 	.globl _EP6CS
                            219 	.globl _EP4CS
                            220 	.globl _EP2CS
                            221 	.globl _EP1INCS
                            222 	.globl _EP1OUTCS
                            223 	.globl _EP0CS
                            224 	.globl _EP8BCL
                            225 	.globl _EP8BCH
                            226 	.globl _EP6BCL
                            227 	.globl _EP6BCH
                            228 	.globl _EP4BCL
                            229 	.globl _EP4BCH
                            230 	.globl _EP2BCL
                            231 	.globl _EP2BCH
                            232 	.globl _EP1INBC
                            233 	.globl _EP1OUTBC
                            234 	.globl _EP0BCL
                            235 	.globl _EP0BCH
                            236 	.globl _FNADDR
                            237 	.globl _MICROFRAME
                            238 	.globl _USBFRAMEL
                            239 	.globl _USBFRAMEH
                            240 	.globl _TOGCTL
                            241 	.globl _WAKEUPCS
                            242 	.globl _SUSPEND
                            243 	.globl _USBCS
                            244 	.globl _XAUTODAT2
                            245 	.globl _XAUTODAT1
                            246 	.globl _I2CTL
                            247 	.globl _I2DAT
                            248 	.globl _I2CS
                            249 	.globl _PORTECFG
                            250 	.globl _PORTCCFG
                            251 	.globl _PORTACFG
                            252 	.globl _INTSETUP
                            253 	.globl _INT4IVEC
                            254 	.globl _INT2IVEC
                            255 	.globl _CLRERRCNT
                            256 	.globl _ERRCNTLIM
                            257 	.globl _USBERRIRQ
                            258 	.globl _USBERRIE
                            259 	.globl _GPIFIRQ
                            260 	.globl _GPIFIE
                            261 	.globl _EPIRQ
                            262 	.globl _EPIE
                            263 	.globl _USBIRQ
                            264 	.globl _USBIE
                            265 	.globl _NAKIRQ
                            266 	.globl _NAKIE
                            267 	.globl _IBNIRQ
                            268 	.globl _IBNIE
                            269 	.globl _EP8FIFOIRQ
                            270 	.globl _EP8FIFOIE
                            271 	.globl _EP6FIFOIRQ
                            272 	.globl _EP6FIFOIE
                            273 	.globl _EP4FIFOIRQ
                            274 	.globl _EP4FIFOIE
                            275 	.globl _EP2FIFOIRQ
                            276 	.globl _EP2FIFOIE
                            277 	.globl _OUTPKTEND
                            278 	.globl _INPKTEND
                            279 	.globl _EP8ISOINPKTS
                            280 	.globl _EP6ISOINPKTS
                            281 	.globl _EP4ISOINPKTS
                            282 	.globl _EP2ISOINPKTS
                            283 	.globl _EP8FIFOPFL
                            284 	.globl _EP8FIFOPFH
                            285 	.globl _EP6FIFOPFL
                            286 	.globl _EP6FIFOPFH
                            287 	.globl _EP4FIFOPFL
                            288 	.globl _EP4FIFOPFH
                            289 	.globl _EP2FIFOPFL
                            290 	.globl _EP2FIFOPFH
                            291 	.globl _EP8AUTOINLENL
                            292 	.globl _EP8AUTOINLENH
                            293 	.globl _EP6AUTOINLENL
                            294 	.globl _EP6AUTOINLENH
                            295 	.globl _EP4AUTOINLENL
                            296 	.globl _EP4AUTOINLENH
                            297 	.globl _EP2AUTOINLENL
                            298 	.globl _EP2AUTOINLENH
                            299 	.globl _EP8FIFOCFG
                            300 	.globl _EP6FIFOCFG
                            301 	.globl _EP4FIFOCFG
                            302 	.globl _EP2FIFOCFG
                            303 	.globl _EP8CFG
                            304 	.globl _EP6CFG
                            305 	.globl _EP4CFG
                            306 	.globl _EP2CFG
                            307 	.globl _EP1INCFG
                            308 	.globl _EP1OUTCFG
                            309 	.globl _REVCTL
                            310 	.globl _REVID
                            311 	.globl _FIFOPINPOLAR
                            312 	.globl _UART230
                            313 	.globl _BPADDRL
                            314 	.globl _BPADDRH
                            315 	.globl _BREAKPT
                            316 	.globl _FIFORESET
                            317 	.globl _PINFLAGSCD
                            318 	.globl _PINFLAGSAB
                            319 	.globl _IFCONFIG
                            320 	.globl _CPUCS
                            321 	.globl _RES_WAVEDATA_END
                            322 	.globl _GPIF_WAVE_DATA
                            323 	.globl _init_hpsdr
                            324 ;--------------------------------------------------------
                            325 ; special function registers
                            326 ;--------------------------------------------------------
                            327 	.area RSEG    (DATA)
                    0080    328 _IOA	=	0x0080
                    0081    329 _SP	=	0x0081
                    0082    330 _DPL	=	0x0082
                    0083    331 _DPH	=	0x0083
                    0084    332 _DPL1	=	0x0084
                    0085    333 _DPH1	=	0x0085
                    0086    334 _DPS	=	0x0086
                    0087    335 _PCON	=	0x0087
                    0088    336 _TCON	=	0x0088
                    0089    337 _TMOD	=	0x0089
                    008A    338 _TL0	=	0x008a
                    008B    339 _TL1	=	0x008b
                    008C    340 _TH0	=	0x008c
                    008D    341 _TH1	=	0x008d
                    008E    342 _CKCON	=	0x008e
                    0090    343 _IOB	=	0x0090
                    0091    344 _EXIF	=	0x0091
                    0092    345 _MPAGE	=	0x0092
                    0098    346 _SCON0	=	0x0098
                    0099    347 _SBUF0	=	0x0099
                    009A    348 _APTR1H	=	0x009a
                    009B    349 _APTR1L	=	0x009b
                    009C    350 _AUTODAT1	=	0x009c
                    009D    351 _AUTOPTRH2	=	0x009d
                    009E    352 _AUTOPTRL2	=	0x009e
                    009F    353 _AUTODAT2	=	0x009f
                    00A0    354 _IOC	=	0x00a0
                    00A1    355 _INT2CLR	=	0x00a1
                    00A2    356 _INT4CLR	=	0x00a2
                    00A8    357 _IE	=	0x00a8
                    00AA    358 _EP2468STAT	=	0x00aa
                    00AB    359 _EP24FIFOFLGS	=	0x00ab
                    00AC    360 _EP68FIFOFLGS	=	0x00ac
                    00AF    361 _AUTOPTRSETUP	=	0x00af
                    00B0    362 _IOD	=	0x00b0
                    00B1    363 _IOE	=	0x00b1
                    00B2    364 _OEA	=	0x00b2
                    00B3    365 _OEB	=	0x00b3
                    00B4    366 _OEC	=	0x00b4
                    00B5    367 _OED	=	0x00b5
                    00B6    368 _OEE	=	0x00b6
                    00B8    369 _IP	=	0x00b8
                    00BA    370 _EP01STAT	=	0x00ba
                    00BB    371 _GPIFTRIG	=	0x00bb
                    00BD    372 _GPIFSGLDATH	=	0x00bd
                    00BE    373 _GPIFSGLDATLX	=	0x00be
                    00BF    374 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    375 _SCON1	=	0x00c0
                    00C1    376 _SBUF1	=	0x00c1
                    00C8    377 _T2CON	=	0x00c8
                    00CA    378 _RCAP2L	=	0x00ca
                    00CB    379 _RCAP2H	=	0x00cb
                    00CC    380 _TL2	=	0x00cc
                    00CD    381 _TH2	=	0x00cd
                    00D0    382 _PSW	=	0x00d0
                    00D8    383 _EICON	=	0x00d8
                    00E0    384 _ACC	=	0x00e0
                    00E8    385 _EIE	=	0x00e8
                    00F0    386 _B	=	0x00f0
                    00F8    387 _EIP	=	0x00f8
                            388 ;--------------------------------------------------------
                            389 ; special function bits 
                            390 ;--------------------------------------------------------
                            391 	.area RSEG    (DATA)
                    0086    392 _SEL	=	0x0086
                    0088    393 _IT0	=	0x0088
                    0089    394 _IE0	=	0x0089
                    008A    395 _IT1	=	0x008a
                    008B    396 _IE1	=	0x008b
                    008C    397 _TR0	=	0x008c
                    008D    398 _TF0	=	0x008d
                    008E    399 _TR1	=	0x008e
                    008F    400 _TF1	=	0x008f
                    0098    401 _RI	=	0x0098
                    0099    402 _TI	=	0x0099
                    009A    403 _RB8	=	0x009a
                    009B    404 _TB8	=	0x009b
                    009C    405 _REN	=	0x009c
                    009D    406 _SM2	=	0x009d
                    009E    407 _SM1	=	0x009e
                    009F    408 _SM0	=	0x009f
                    00A8    409 _EX0	=	0x00a8
                    00A9    410 _ET0	=	0x00a9
                    00AA    411 _EX1	=	0x00aa
                    00AB    412 _ET1	=	0x00ab
                    00AC    413 _ES0	=	0x00ac
                    00AD    414 _ET2	=	0x00ad
                    00AE    415 _ES1	=	0x00ae
                    00AF    416 _EA	=	0x00af
                    00B8    417 _PX0	=	0x00b8
                    00B9    418 _PT0	=	0x00b9
                    00BA    419 _PX1	=	0x00ba
                    00BB    420 _PT1	=	0x00bb
                    00BC    421 _PS0	=	0x00bc
                    00BD    422 _PT2	=	0x00bd
                    00BE    423 _PS1	=	0x00be
                    00C0    424 _RI1	=	0x00c0
                    00C1    425 _TI1	=	0x00c1
                    00C2    426 _RB81	=	0x00c2
                    00C3    427 _TB81	=	0x00c3
                    00C4    428 _REN1	=	0x00c4
                    00C5    429 _SM21	=	0x00c5
                    00C6    430 _SM11	=	0x00c6
                    00C7    431 _SM01	=	0x00c7
                    00C8    432 _CP_RL2	=	0x00c8
                    00C9    433 _C_T2	=	0x00c9
                    00CA    434 _TR2	=	0x00ca
                    00CB    435 _EXEN2	=	0x00cb
                    00CC    436 _TCLK	=	0x00cc
                    00CD    437 _RCLK	=	0x00cd
                    00CE    438 _EXF2	=	0x00ce
                    00CF    439 _TF2	=	0x00cf
                    00D0    440 _P	=	0x00d0
                    00D1    441 _FL	=	0x00d1
                    00D2    442 _OV	=	0x00d2
                    00D3    443 _RS0	=	0x00d3
                    00D4    444 _RS1	=	0x00d4
                    00D5    445 _F0	=	0x00d5
                    00D6    446 _AC	=	0x00d6
                    00D7    447 _CY	=	0x00d7
                    00DB    448 _INT6	=	0x00db
                    00DC    449 _RESI	=	0x00dc
                    00DD    450 _ERESI	=	0x00dd
                    00DF    451 _SMOD1	=	0x00df
                    00E8    452 _EIUSB	=	0x00e8
                    00E9    453 _EI2C	=	0x00e9
                    00EA    454 _EIEX4	=	0x00ea
                    00EB    455 _EIEX5	=	0x00eb
                    00EC    456 _EIEX6	=	0x00ec
                    00F8    457 _PUSB	=	0x00f8
                    00F9    458 _PI2C	=	0x00f9
                    00FA    459 _EIPX4	=	0x00fa
                    00FB    460 _EIPX5	=	0x00fb
                    00FC    461 _EIPX6	=	0x00fc
                    0080    462 _bitS_CLK	=	0x0080
                    0081    463 _bitS_OUT	=	0x0081
                    0083    464 _bitS_IN	=	0x0083
                    00A0    465 _bitALTERA_DATA0	=	0x00a0
                    00A2    466 _bitALTERA_DCLK	=	0x00a2
                            467 ;--------------------------------------------------------
                            468 ; overlayable register banks 
                            469 ;--------------------------------------------------------
                            470 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     471 	.ds 8
                            472 ;--------------------------------------------------------
                            473 ; internal ram data
                            474 ;--------------------------------------------------------
                            475 	.area DSEG    (DATA)
                            476 ;--------------------------------------------------------
                            477 ; overlayable items in internal ram 
                            478 ;--------------------------------------------------------
                            479 	.area OSEG    (OVR,DATA)
                            480 ;--------------------------------------------------------
                            481 ; indirectly addressable internal ram data
                            482 ;--------------------------------------------------------
                            483 	.area ISEG    (DATA)
                            484 ;--------------------------------------------------------
                            485 ; bit data
                            486 ;--------------------------------------------------------
                            487 	.area BSEG    (BIT)
                            488 ;--------------------------------------------------------
                            489 ; paged external ram data
                            490 ;--------------------------------------------------------
                            491 	.area PSEG    (PAG,XDATA)
                            492 ;--------------------------------------------------------
                            493 ; external ram data
                            494 ;--------------------------------------------------------
                            495 	.area XSEG    (XDATA)
                    E400    496 _GPIF_WAVE_DATA	=	0xe400
                    E480    497 _RES_WAVEDATA_END	=	0xe480
                    E600    498 _CPUCS	=	0xe600
                    E601    499 _IFCONFIG	=	0xe601
                    E602    500 _PINFLAGSAB	=	0xe602
                    E603    501 _PINFLAGSCD	=	0xe603
                    E604    502 _FIFORESET	=	0xe604
                    E605    503 _BREAKPT	=	0xe605
                    E606    504 _BPADDRH	=	0xe606
                    E607    505 _BPADDRL	=	0xe607
                    E608    506 _UART230	=	0xe608
                    E609    507 _FIFOPINPOLAR	=	0xe609
                    E60A    508 _REVID	=	0xe60a
                    E60B    509 _REVCTL	=	0xe60b
                    E610    510 _EP1OUTCFG	=	0xe610
                    E611    511 _EP1INCFG	=	0xe611
                    E612    512 _EP2CFG	=	0xe612
                    E613    513 _EP4CFG	=	0xe613
                    E614    514 _EP6CFG	=	0xe614
                    E615    515 _EP8CFG	=	0xe615
                    E618    516 _EP2FIFOCFG	=	0xe618
                    E619    517 _EP4FIFOCFG	=	0xe619
                    E61A    518 _EP6FIFOCFG	=	0xe61a
                    E61B    519 _EP8FIFOCFG	=	0xe61b
                    E620    520 _EP2AUTOINLENH	=	0xe620
                    E621    521 _EP2AUTOINLENL	=	0xe621
                    E622    522 _EP4AUTOINLENH	=	0xe622
                    E623    523 _EP4AUTOINLENL	=	0xe623
                    E624    524 _EP6AUTOINLENH	=	0xe624
                    E625    525 _EP6AUTOINLENL	=	0xe625
                    E626    526 _EP8AUTOINLENH	=	0xe626
                    E627    527 _EP8AUTOINLENL	=	0xe627
                    E630    528 _EP2FIFOPFH	=	0xe630
                    E631    529 _EP2FIFOPFL	=	0xe631
                    E632    530 _EP4FIFOPFH	=	0xe632
                    E633    531 _EP4FIFOPFL	=	0xe633
                    E634    532 _EP6FIFOPFH	=	0xe634
                    E635    533 _EP6FIFOPFL	=	0xe635
                    E636    534 _EP8FIFOPFH	=	0xe636
                    E637    535 _EP8FIFOPFL	=	0xe637
                    E640    536 _EP2ISOINPKTS	=	0xe640
                    E641    537 _EP4ISOINPKTS	=	0xe641
                    E642    538 _EP6ISOINPKTS	=	0xe642
                    E643    539 _EP8ISOINPKTS	=	0xe643
                    E648    540 _INPKTEND	=	0xe648
                    E649    541 _OUTPKTEND	=	0xe649
                    E650    542 _EP2FIFOIE	=	0xe650
                    E651    543 _EP2FIFOIRQ	=	0xe651
                    E652    544 _EP4FIFOIE	=	0xe652
                    E653    545 _EP4FIFOIRQ	=	0xe653
                    E654    546 _EP6FIFOIE	=	0xe654
                    E655    547 _EP6FIFOIRQ	=	0xe655
                    E656    548 _EP8FIFOIE	=	0xe656
                    E657    549 _EP8FIFOIRQ	=	0xe657
                    E658    550 _IBNIE	=	0xe658
                    E659    551 _IBNIRQ	=	0xe659
                    E65A    552 _NAKIE	=	0xe65a
                    E65B    553 _NAKIRQ	=	0xe65b
                    E65C    554 _USBIE	=	0xe65c
                    E65D    555 _USBIRQ	=	0xe65d
                    E65E    556 _EPIE	=	0xe65e
                    E65F    557 _EPIRQ	=	0xe65f
                    E660    558 _GPIFIE	=	0xe660
                    E661    559 _GPIFIRQ	=	0xe661
                    E662    560 _USBERRIE	=	0xe662
                    E663    561 _USBERRIRQ	=	0xe663
                    E664    562 _ERRCNTLIM	=	0xe664
                    E665    563 _CLRERRCNT	=	0xe665
                    E666    564 _INT2IVEC	=	0xe666
                    E667    565 _INT4IVEC	=	0xe667
                    E668    566 _INTSETUP	=	0xe668
                    E670    567 _PORTACFG	=	0xe670
                    E671    568 _PORTCCFG	=	0xe671
                    E672    569 _PORTECFG	=	0xe672
                    E678    570 _I2CS	=	0xe678
                    E679    571 _I2DAT	=	0xe679
                    E67A    572 _I2CTL	=	0xe67a
                    E67B    573 _XAUTODAT1	=	0xe67b
                    E67C    574 _XAUTODAT2	=	0xe67c
                    E680    575 _USBCS	=	0xe680
                    E681    576 _SUSPEND	=	0xe681
                    E682    577 _WAKEUPCS	=	0xe682
                    E683    578 _TOGCTL	=	0xe683
                    E684    579 _USBFRAMEH	=	0xe684
                    E685    580 _USBFRAMEL	=	0xe685
                    E686    581 _MICROFRAME	=	0xe686
                    E687    582 _FNADDR	=	0xe687
                    E68A    583 _EP0BCH	=	0xe68a
                    E68B    584 _EP0BCL	=	0xe68b
                    E68D    585 _EP1OUTBC	=	0xe68d
                    E68F    586 _EP1INBC	=	0xe68f
                    E690    587 _EP2BCH	=	0xe690
                    E691    588 _EP2BCL	=	0xe691
                    E694    589 _EP4BCH	=	0xe694
                    E695    590 _EP4BCL	=	0xe695
                    E698    591 _EP6BCH	=	0xe698
                    E699    592 _EP6BCL	=	0xe699
                    E69C    593 _EP8BCH	=	0xe69c
                    E69D    594 _EP8BCL	=	0xe69d
                    E6A0    595 _EP0CS	=	0xe6a0
                    E6A1    596 _EP1OUTCS	=	0xe6a1
                    E6A2    597 _EP1INCS	=	0xe6a2
                    E6A3    598 _EP2CS	=	0xe6a3
                    E6A4    599 _EP4CS	=	0xe6a4
                    E6A5    600 _EP6CS	=	0xe6a5
                    E6A6    601 _EP8CS	=	0xe6a6
                    E6A7    602 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    603 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    604 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    605 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    606 _EP2FIFOBCH	=	0xe6ab
                    E6AC    607 _EP2FIFOBCL	=	0xe6ac
                    E6AD    608 _EP4FIFOBCH	=	0xe6ad
                    E6AE    609 _EP4FIFOBCL	=	0xe6ae
                    E6AF    610 _EP6FIFOBCH	=	0xe6af
                    E6B0    611 _EP6FIFOBCL	=	0xe6b0
                    E6B1    612 _EP8FIFOBCH	=	0xe6b1
                    E6B2    613 _EP8FIFOBCL	=	0xe6b2
                    E6B3    614 _SUDPTRH	=	0xe6b3
                    E6B4    615 _SUDPTRL	=	0xe6b4
                    E6B5    616 _SUDPTRCTL	=	0xe6b5
                    E6B8    617 _SETUPDAT	=	0xe6b8
                    E6C0    618 _GPIFWFSELECT	=	0xe6c0
                    E6C1    619 _GPIFIDLECS	=	0xe6c1
                    E6C2    620 _GPIFIDLECTL	=	0xe6c2
                    E6C3    621 _GPIFCTLCFG	=	0xe6c3
                    E6C4    622 _GPIFADRH	=	0xe6c4
                    E6C5    623 _GPIFADRL	=	0xe6c5
                    E6CE    624 _GPIFTCB3	=	0xe6ce
                    E6CF    625 _GPIFTCB2	=	0xe6cf
                    E6D0    626 _GPIFTCB1	=	0xe6d0
                    E6D1    627 _GPIFTCB0	=	0xe6d1
                    E6D2    628 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    629 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    630 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    631 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    632 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    633 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    634 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    635 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    636 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    637 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    638 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    639 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    640 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    641 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    642 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    643 _GPIFREADYCFG	=	0xe6f3
                    E6F4    644 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    645 _GPIFABORT	=	0xe6f5
                    E6C6    646 _FLOWSTATE	=	0xe6c6
                    E6C7    647 _FLOWLOGIC	=	0xe6c7
                    E6C8    648 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    649 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    650 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    651 _FLOWSTB	=	0xe6cb
                    E6CC    652 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    653 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    654 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    655 _UDMACRCH	=	0xe67d
                    E67E    656 _UDMACRCL	=	0xe67e
                    E67F    657 _UDMACRCQUAL	=	0xe67f
                    E6F8    658 _DBUG	=	0xe6f8
                    E6F9    659 _TESTCFG	=	0xe6f9
                    E6FA    660 _USBTEST	=	0xe6fa
                    E6FB    661 _CT1	=	0xe6fb
                    E6FC    662 _CT2	=	0xe6fc
                    E6FD    663 _CT3	=	0xe6fd
                    E6FE    664 _CT4	=	0xe6fe
                    E740    665 _EP0BUF	=	0xe740
                    E780    666 _EP1OUTBUF	=	0xe780
                    E7C0    667 _EP1INBUF	=	0xe7c0
                    F000    668 _EP2FIFOBUF	=	0xf000
                    F400    669 _EP4FIFOBUF	=	0xf400
                    F800    670 _EP6FIFOBUF	=	0xf800
                    FC00    671 _EP8FIFOBUF	=	0xfc00
                            672 ;--------------------------------------------------------
                            673 ; external initialized ram data
                            674 ;--------------------------------------------------------
                            675 	.area XISEG   (XDATA)
                            676 	.area CSEG    (CODE)
                            677 	.area GSINIT0 (CODE)
                            678 	.area GSINIT1 (CODE)
                            679 	.area GSINIT2 (CODE)
                            680 	.area GSINIT3 (CODE)
                            681 	.area GSINIT4 (CODE)
                            682 	.area GSINIT5 (CODE)
                            683 ;--------------------------------------------------------
                            684 ; global & static initialisations
                            685 ;--------------------------------------------------------
                            686 	.area CSEG    (CODE)
                            687 	.area GSINIT  (CODE)
                            688 	.area GSFINAL (CODE)
                            689 	.area GSINIT  (CODE)
                            690 ;--------------------------------------------------------
                            691 ; Home
                            692 ;--------------------------------------------------------
                            693 	.area HOME    (CODE)
                            694 	.area CSEG    (CODE)
                            695 ;--------------------------------------------------------
                            696 ; code
                            697 ;--------------------------------------------------------
                            698 	.area CSEG    (CODE)
                            699 ;------------------------------------------------------------
                            700 ;Allocation info for local variables in function 'init_hpsdr'
                            701 ;------------------------------------------------------------
                            702 ;------------------------------------------------------------
                            703 ;Initial/src/hpsdr_common.c:33: init_hpsdr (void)
                            704 ;	-----------------------------------------
                            705 ;	 function init_hpsdr
                            706 ;	-----------------------------------------
   0468                     707 _init_hpsdr:
                    0002    708 	ar2 = 0x02
                    0003    709 	ar3 = 0x03
                    0004    710 	ar4 = 0x04
                    0005    711 	ar5 = 0x05
                    0006    712 	ar6 = 0x06
                    0007    713 	ar7 = 0x07
                    0000    714 	ar0 = 0x00
                    0001    715 	ar1 = 0x01
                            716 ;Initial/src/hpsdr_common.c:35: CPUCS = bmCLKSPD1;	// CPU runs @ 48 MHz
                            717 ;     genAssign
   0468 90 E6 00            718 	mov	dptr,#_CPUCS
   046B 74 10               719 	mov	a,#0x10
   046D F0                  720 	movx	@dptr,a
                            721 ;Initial/src/hpsdr_common.c:36: CKCON = 0;		// MOVX takes 2 cycles
                            722 ;     genAssign
   046E 75 8E 00            723 	mov	_CKCON,#0x00
                            724 ;Initial/src/hpsdr_common.c:40: IFCONFIG = bmIFCLKSRC | bm3048MHZ | bmIFCLKOE | bmIFCLKPOL | bmIFFIFO;
                            725 ;     genAssign
   0471 90 E6 01            726 	mov	dptr,#_IFCONFIG
   0474 74 F3               727 	mov	a,#0xF3
   0476 F0                  728 	movx	@dptr,a
                            729 ;Initial/src/hpsdr_common.c:41: SYNCDELAY;
                            730 ;     genInline
   0477 00                  731 	 nop; nop; nop; 
                            732 ;Initial/src/hpsdr_common.c:45: IOA = bmPORT_A_INITIAL;	// Port A initial state
                            733 ;     genAssign
   0478 75 80 02            734 	mov	_IOA,#0x02
                            735 ;Initial/src/hpsdr_common.c:46: OEA = bmPORT_A_OUTPUTS;	// Port A direction register
                            736 ;     genAssign
   047B 75 B2 03            737 	mov	_OEA,#0x03
                            738 ;Initial/src/hpsdr_common.c:48: IOC = bmPORT_C_INITIAL;	// Port C initial state
                            739 ;     genAssign
   047E 75 A0 C0            740 	mov	_IOC,#0xC0
                            741 ;Initial/src/hpsdr_common.c:49: OEC = bmPORT_C_OUTPUTS;	// Port C direction register
                            742 ;     genAssign
   0481 75 B4 E7            743 	mov	_OEC,#0xE7
                            744 ;Initial/src/hpsdr_common.c:51: IOE = bmPORT_E_INITIAL;	// Port E initial state
                            745 ;     genAssign
   0484 75 B1 FF            746 	mov	_IOE,#0xFF
                            747 ;Initial/src/hpsdr_common.c:52: OEE = bmPORT_E_OUTPUTS;	// Port E direction register
                            748 ;     genAssign
   0487 75 B6 FF            749 	mov	_OEE,#0xFF
                            750 ;Initial/src/hpsdr_common.c:57: EP1OUTCFG = bmVALID | bmBULK;				SYNCDELAY;
                            751 ;     genAssign
   048A 90 E6 10            752 	mov	dptr,#_EP1OUTCFG
   048D 74 A0               753 	mov	a,#0xA0
   048F F0                  754 	movx	@dptr,a
                            755 ;     genInline
   0490 00                  756 	 nop; nop; nop; 
                            757 ;Initial/src/hpsdr_common.c:58: EP1INCFG  = bmVALID | bmBULK | bmIN;			SYNCDELAY;
                            758 ;     genAssign
   0491 90 E6 11            759 	mov	dptr,#_EP1INCFG
   0494 74 E0               760 	mov	a,#0xE0
   0496 F0                  761 	movx	@dptr,a
                            762 ;     genInline
   0497 00                  763 	 nop; nop; nop; 
                            764 ;Initial/src/hpsdr_common.c:60: EP2CFG    = bmVALID | bmBULK | bmQUADBUF;		SYNCDELAY;	// 512 quad bulk OUT
                            765 ;     genAssign
   0498 90 E6 12            766 	mov	dptr,#_EP2CFG
   049B 74 A0               767 	mov	a,#0xA0
   049D F0                  768 	movx	@dptr,a
                            769 ;     genInline
   049E 00                  770 	 nop; nop; nop; 
                            771 ;Initial/src/hpsdr_common.c:61: EP4CFG    = 0;					SYNCDELAY;	// disabled
                            772 ;     genAssign
   049F 90 E6 13            773 	mov	dptr,#_EP4CFG
                            774 ;	Peephole 181	changed mov to clr
   04A2 E4                  775 	clr	a
   04A3 F0                  776 	movx	@dptr,a
                            777 ;     genInline
   04A4 00                  778 	 nop; nop; nop; 
                            779 ;Initial/src/hpsdr_common.c:62: EP6CFG    = bmVALID | bmBULK | bmQUADBUF | bmIN;	SYNCDELAY;	// 512 quad bulk IN
                            780 ;     genAssign
   04A5 90 E6 14            781 	mov	dptr,#_EP6CFG
   04A8 74 E0               782 	mov	a,#0xE0
   04AA F0                  783 	movx	@dptr,a
                            784 ;     genInline
   04AB 00                  785 	 nop; nop; nop; 
                            786 ;Initial/src/hpsdr_common.c:63: EP8CFG    = 0;					SYNCDELAY;	// disabled
                            787 ;     genAssign
   04AC 90 E6 15            788 	mov	dptr,#_EP8CFG
                            789 ;	Peephole 181	changed mov to clr
   04AF E4                  790 	clr	a
   04B0 F0                  791 	movx	@dptr,a
                            792 ;     genInline
   04B1 00                  793 	 nop; nop; nop; 
                            794 ;Initial/src/hpsdr_common.c:67: FIFORESET = bmNAKALL;					SYNCDELAY;
                            795 ;     genAssign
   04B2 90 E6 04            796 	mov	dptr,#_FIFORESET
   04B5 74 80               797 	mov	a,#0x80
   04B7 F0                  798 	movx	@dptr,a
                            799 ;     genInline
   04B8 00                  800 	 nop; nop; nop; 
                            801 ;Initial/src/hpsdr_common.c:68: FIFORESET = 2;					SYNCDELAY;
                            802 ;     genAssign
   04B9 90 E6 04            803 	mov	dptr,#_FIFORESET
   04BC 74 02               804 	mov	a,#0x02
   04BE F0                  805 	movx	@dptr,a
                            806 ;     genInline
   04BF 00                  807 	 nop; nop; nop; 
                            808 ;Initial/src/hpsdr_common.c:70: FIFORESET = 6;					SYNCDELAY;
                            809 ;     genAssign
   04C0 90 E6 04            810 	mov	dptr,#_FIFORESET
   04C3 74 06               811 	mov	a,#0x06
   04C5 F0                  812 	movx	@dptr,a
                            813 ;     genInline
   04C6 00                  814 	 nop; nop; nop; 
                            815 ;Initial/src/hpsdr_common.c:72: FIFORESET = 0;					SYNCDELAY;
                            816 ;     genAssign
   04C7 90 E6 04            817 	mov	dptr,#_FIFORESET
                            818 ;	Peephole 181	changed mov to clr
   04CA E4                  819 	clr	a
   04CB F0                  820 	movx	@dptr,a
                            821 ;     genInline
   04CC 00                  822 	 nop; nop; nop; 
                            823 ;Initial/src/hpsdr_common.c:78: EP2FIFOCFG =             bmWORDWIDE;			SYNCDELAY;
                            824 ;     genAssign
   04CD 90 E6 18            825 	mov	dptr,#_EP2FIFOCFG
   04D0 74 01               826 	mov	a,#0x01
   04D2 F0                  827 	movx	@dptr,a
                            828 ;     genInline
   04D3 00                  829 	 nop; nop; nop; 
                            830 ;Initial/src/hpsdr_common.c:79: EP2FIFOCFG = bmAUTOOUT | bmWORDWIDE;			SYNCDELAY;
                            831 ;     genAssign
   04D4 90 E6 18            832 	mov	dptr,#_EP2FIFOCFG
   04D7 74 11               833 	mov	a,#0x11
   04D9 F0                  834 	movx	@dptr,a
                            835 ;     genInline
   04DA 00                  836 	 nop; nop; nop; 
                            837 ;Initial/src/hpsdr_common.c:80: EP6FIFOCFG = bmAUTOIN  | bmWORDWIDE;			SYNCDELAY;
                            838 ;     genAssign
   04DB 90 E6 1A            839 	mov	dptr,#_EP6FIFOCFG
   04DE 74 09               840 	mov	a,#0x09
   04E0 F0                  841 	movx	@dptr,a
                            842 ;     genInline
   04E1 00                  843 	 nop; nop; nop; 
                            844 ;Initial/src/hpsdr_common.c:92: EP0BCH = 0;			SYNCDELAY;
                            845 ;     genAssign
   04E2 90 E6 8A            846 	mov	dptr,#_EP0BCH
                            847 ;	Peephole 181	changed mov to clr
   04E5 E4                  848 	clr	a
   04E6 F0                  849 	movx	@dptr,a
                            850 ;     genInline
   04E7 00                  851 	 nop; nop; nop; 
                            852 ;Initial/src/hpsdr_common.c:96: EP1OUTBC = 0;			SYNCDELAY;
                            853 ;     genAssign
   04E8 90 E6 8D            854 	mov	dptr,#_EP1OUTBC
                            855 ;	Peephole 181	changed mov to clr
   04EB E4                  856 	clr	a
   04EC F0                  857 	movx	@dptr,a
                            858 ;     genInline
   04ED 00                  859 	 nop; nop; nop; 
                            860 ;Initial/src/hpsdr_common.c:104: EP6AUTOINLENH = (512) >> 8;	SYNCDELAY;  // this is the length for high speed
                            861 ;     genAssign
   04EE 90 E6 24            862 	mov	dptr,#_EP6AUTOINLENH
   04F1 74 02               863 	mov	a,#0x02
   04F3 F0                  864 	movx	@dptr,a
                            865 ;     genInline
   04F4 00                  866 	 nop; nop; nop; 
                            867 ;Initial/src/hpsdr_common.c:105: EP6AUTOINLENL = (512) & 0xff; SYNCDELAY;
                            868 ;     genAssign
   04F5 90 E6 25            869 	mov	dptr,#_EP6AUTOINLENL
                            870 ;	Peephole 181	changed mov to clr
   04F8 E4                  871 	clr	a
   04F9 F0                  872 	movx	@dptr,a
                            873 ;     genInline
   04FA 00                  874 	 nop; nop; nop; 
                            875 ;Initial/src/hpsdr_common.c:107: init_board ();
                            876 ;     genCall
                            877 ;	Peephole 253.b	replaced lcall/ret with ljmp
   04FB 02 03 97            878 	ljmp	_init_board
                            879 	.area CSEG    (CODE)
                            880 	.area XINIT   (CODE)
