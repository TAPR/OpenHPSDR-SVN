                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Fri Jan 05 10:13:14 2007
                              5 ;--------------------------------------------------------
                              6 	.module board_specific
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _init_board
                             13 	.globl _bitCPLD_CS
                             14 	.globl _bitFPGA_CS
                             15 	.globl _bitALTERA_DCLK
                             16 	.globl _bitALTERA_DATA0
                             17 	.globl _bitS_CLK
                             18 	.globl _bitS_IN
                             19 	.globl _bitS_OUT
                             20 	.globl _EIPX6
                             21 	.globl _EIPX5
                             22 	.globl _EIPX4
                             23 	.globl _PI2C
                             24 	.globl _PUSB
                             25 	.globl _EIEX6
                             26 	.globl _EIEX5
                             27 	.globl _EIEX4
                             28 	.globl _EI2C
                             29 	.globl _EIUSB
                             30 	.globl _SMOD1
                             31 	.globl _ERESI
                             32 	.globl _RESI
                             33 	.globl _INT6
                             34 	.globl _CY
                             35 	.globl _AC
                             36 	.globl _F0
                             37 	.globl _RS1
                             38 	.globl _RS0
                             39 	.globl _OV
                             40 	.globl _FL
                             41 	.globl _P
                             42 	.globl _TF2
                             43 	.globl _EXF2
                             44 	.globl _RCLK
                             45 	.globl _TCLK
                             46 	.globl _EXEN2
                             47 	.globl _TR2
                             48 	.globl _C_T2
                             49 	.globl _CP_RL2
                             50 	.globl _SM01
                             51 	.globl _SM11
                             52 	.globl _SM21
                             53 	.globl _REN1
                             54 	.globl _TB81
                             55 	.globl _RB81
                             56 	.globl _TI1
                             57 	.globl _RI1
                             58 	.globl _PS1
                             59 	.globl _PT2
                             60 	.globl _PS0
                             61 	.globl _PT1
                             62 	.globl _PX1
                             63 	.globl _PT0
                             64 	.globl _PX0
                             65 	.globl _EA
                             66 	.globl _ES1
                             67 	.globl _ET2
                             68 	.globl _ES0
                             69 	.globl _ET1
                             70 	.globl _EX1
                             71 	.globl _ET0
                             72 	.globl _EX0
                             73 	.globl _SM0
                             74 	.globl _SM1
                             75 	.globl _SM2
                             76 	.globl _REN
                             77 	.globl _TB8
                             78 	.globl _RB8
                             79 	.globl _TI
                             80 	.globl _RI
                             81 	.globl _TF1
                             82 	.globl _TR1
                             83 	.globl _TF0
                             84 	.globl _TR0
                             85 	.globl _IE1
                             86 	.globl _IT1
                             87 	.globl _IE0
                             88 	.globl _IT0
                             89 	.globl _SEL
                             90 	.globl _EIP
                             91 	.globl _B
                             92 	.globl _EIE
                             93 	.globl _ACC
                             94 	.globl _EICON
                             95 	.globl _PSW
                             96 	.globl _TH2
                             97 	.globl _TL2
                             98 	.globl _RCAP2H
                             99 	.globl _RCAP2L
                            100 	.globl _T2CON
                            101 	.globl _SBUF1
                            102 	.globl _SCON1
                            103 	.globl _GPIFSGLDATLNOX
                            104 	.globl _GPIFSGLDATLX
                            105 	.globl _GPIFSGLDATH
                            106 	.globl _GPIFTRIG
                            107 	.globl _EP01STAT
                            108 	.globl _IP
                            109 	.globl _OEE
                            110 	.globl _OED
                            111 	.globl _OEC
                            112 	.globl _OEB
                            113 	.globl _OEA
                            114 	.globl _IOE
                            115 	.globl _IOD
                            116 	.globl _AUTOPTRSETUP
                            117 	.globl _EP68FIFOFLGS
                            118 	.globl _EP24FIFOFLGS
                            119 	.globl _EP2468STAT
                            120 	.globl _IE
                            121 	.globl _INT4CLR
                            122 	.globl _INT2CLR
                            123 	.globl _IOC
                            124 	.globl _AUTODAT2
                            125 	.globl _AUTOPTRL2
                            126 	.globl _AUTOPTRH2
                            127 	.globl _AUTODAT1
                            128 	.globl _APTR1L
                            129 	.globl _APTR1H
                            130 	.globl _SBUF0
                            131 	.globl _SCON0
                            132 	.globl _MPAGE
                            133 	.globl _EXIF
                            134 	.globl _IOB
                            135 	.globl _CKCON
                            136 	.globl _TH1
                            137 	.globl _TH0
                            138 	.globl _TL1
                            139 	.globl _TL0
                            140 	.globl _TMOD
                            141 	.globl _TCON
                            142 	.globl _PCON
                            143 	.globl _DPS
                            144 	.globl _DPH1
                            145 	.globl _DPL1
                            146 	.globl _DPH
                            147 	.globl _DPL
                            148 	.globl _SP
                            149 	.globl _IOA
                            150 	.globl _CPLD_GPIO
                            151 	.globl _RFFE_CNTRL
                            152 	.globl _ATTN
                            153 	.globl _DEBUG_LED_REG
                            154 	.globl _LTC2208_CNTRL_REG
                            155 	.globl _DDC_CNTRL
                            156 	.globl _DDC1_AMR
                            157 	.globl _DDC1_LAR
                            158 	.globl _DDC1_DR4
                            159 	.globl _DDC1_DR3
                            160 	.globl _DDC1_DR2
                            161 	.globl _DDC1_DR1
                            162 	.globl _DDC1_DR0
                            163 	.globl _DDC0_AMR
                            164 	.globl _DDC0_LAR
                            165 	.globl _DDC0_DR4
                            166 	.globl _DDC0_DR3
                            167 	.globl _DDC0_DR2
                            168 	.globl _DDC0_DR1
                            169 	.globl _DDC0_DR0
                            170 	.globl _EP8FIFOBUF
                            171 	.globl _EP6FIFOBUF
                            172 	.globl _EP4FIFOBUF
                            173 	.globl _EP2FIFOBUF
                            174 	.globl _EP1INBUF
                            175 	.globl _EP1OUTBUF
                            176 	.globl _EP0BUF
                            177 	.globl _CT4
                            178 	.globl _CT3
                            179 	.globl _CT2
                            180 	.globl _CT1
                            181 	.globl _USBTEST
                            182 	.globl _TESTCFG
                            183 	.globl _DBUG
                            184 	.globl _UDMACRCQUAL
                            185 	.globl _UDMACRCL
                            186 	.globl _UDMACRCH
                            187 	.globl _GPIFHOLDAMOUNT
                            188 	.globl _FLOWSTBHPERIOD
                            189 	.globl _FLOWSTBEDGE
                            190 	.globl _FLOWSTB
                            191 	.globl _FLOWHOLDOFF
                            192 	.globl _FLOWEQ1CTL
                            193 	.globl _FLOWEQ0CTL
                            194 	.globl _FLOWLOGIC
                            195 	.globl _FLOWSTATE
                            196 	.globl _GPIFABORT
                            197 	.globl _GPIFREADYSTAT
                            198 	.globl _GPIFREADYCFG
                            199 	.globl _XGPIFSGLDATLNOX
                            200 	.globl _XGPIFSGLDATLX
                            201 	.globl _XGPIFSGLDATH
                            202 	.globl _EP8GPIFTRIG
                            203 	.globl _EP8GPIFPFSTOP
                            204 	.globl _EP8GPIFFLGSEL
                            205 	.globl _EP6GPIFTRIG
                            206 	.globl _EP6GPIFPFSTOP
                            207 	.globl _EP6GPIFFLGSEL
                            208 	.globl _EP4GPIFTRIG
                            209 	.globl _EP4GPIFPFSTOP
                            210 	.globl _EP4GPIFFLGSEL
                            211 	.globl _EP2GPIFTRIG
                            212 	.globl _EP2GPIFPFSTOP
                            213 	.globl _EP2GPIFFLGSEL
                            214 	.globl _GPIFTCB0
                            215 	.globl _GPIFTCB1
                            216 	.globl _GPIFTCB2
                            217 	.globl _GPIFTCB3
                            218 	.globl _GPIFADRL
                            219 	.globl _GPIFADRH
                            220 	.globl _GPIFCTLCFG
                            221 	.globl _GPIFIDLECTL
                            222 	.globl _GPIFIDLECS
                            223 	.globl _GPIFWFSELECT
                            224 	.globl _SETUPDAT
                            225 	.globl _SUDPTRCTL
                            226 	.globl _SUDPTRL
                            227 	.globl _SUDPTRH
                            228 	.globl _EP8FIFOBCL
                            229 	.globl _EP8FIFOBCH
                            230 	.globl _EP6FIFOBCL
                            231 	.globl _EP6FIFOBCH
                            232 	.globl _EP4FIFOBCL
                            233 	.globl _EP4FIFOBCH
                            234 	.globl _EP2FIFOBCL
                            235 	.globl _EP2FIFOBCH
                            236 	.globl _EP8FIFOFLGS
                            237 	.globl _EP6FIFOFLGS
                            238 	.globl _EP4FIFOFLGS
                            239 	.globl _EP2FIFOFLGS
                            240 	.globl _EP8CS
                            241 	.globl _EP6CS
                            242 	.globl _EP4CS
                            243 	.globl _EP2CS
                            244 	.globl _EP1INCS
                            245 	.globl _EP1OUTCS
                            246 	.globl _EP0CS
                            247 	.globl _EP8BCL
                            248 	.globl _EP8BCH
                            249 	.globl _EP6BCL
                            250 	.globl _EP6BCH
                            251 	.globl _EP4BCL
                            252 	.globl _EP4BCH
                            253 	.globl _EP2BCL
                            254 	.globl _EP2BCH
                            255 	.globl _EP1INBC
                            256 	.globl _EP1OUTBC
                            257 	.globl _EP0BCL
                            258 	.globl _EP0BCH
                            259 	.globl _FNADDR
                            260 	.globl _MICROFRAME
                            261 	.globl _USBFRAMEL
                            262 	.globl _USBFRAMEH
                            263 	.globl _TOGCTL
                            264 	.globl _WAKEUPCS
                            265 	.globl _SUSPEND
                            266 	.globl _USBCS
                            267 	.globl _XAUTODAT2
                            268 	.globl _XAUTODAT1
                            269 	.globl _I2CTL
                            270 	.globl _I2DAT
                            271 	.globl _I2CS
                            272 	.globl _PORTECFG
                            273 	.globl _PORTCCFG
                            274 	.globl _PORTACFG
                            275 	.globl _INTSETUP
                            276 	.globl _INT4IVEC
                            277 	.globl _INT2IVEC
                            278 	.globl _CLRERRCNT
                            279 	.globl _ERRCNTLIM
                            280 	.globl _USBERRIRQ
                            281 	.globl _USBERRIE
                            282 	.globl _GPIFIRQ
                            283 	.globl _GPIFIE
                            284 	.globl _EPIRQ
                            285 	.globl _EPIE
                            286 	.globl _USBIRQ
                            287 	.globl _USBIE
                            288 	.globl _NAKIRQ
                            289 	.globl _NAKIE
                            290 	.globl _IBNIRQ
                            291 	.globl _IBNIE
                            292 	.globl _EP8FIFOIRQ
                            293 	.globl _EP8FIFOIE
                            294 	.globl _EP6FIFOIRQ
                            295 	.globl _EP6FIFOIE
                            296 	.globl _EP4FIFOIRQ
                            297 	.globl _EP4FIFOIE
                            298 	.globl _EP2FIFOIRQ
                            299 	.globl _EP2FIFOIE
                            300 	.globl _OUTPKTEND
                            301 	.globl _INPKTEND
                            302 	.globl _EP8ISOINPKTS
                            303 	.globl _EP6ISOINPKTS
                            304 	.globl _EP4ISOINPKTS
                            305 	.globl _EP2ISOINPKTS
                            306 	.globl _EP8FIFOPFL
                            307 	.globl _EP8FIFOPFH
                            308 	.globl _EP6FIFOPFL
                            309 	.globl _EP6FIFOPFH
                            310 	.globl _EP4FIFOPFL
                            311 	.globl _EP4FIFOPFH
                            312 	.globl _EP2FIFOPFL
                            313 	.globl _EP2FIFOPFH
                            314 	.globl _EP8AUTOINLENL
                            315 	.globl _EP8AUTOINLENH
                            316 	.globl _EP6AUTOINLENL
                            317 	.globl _EP6AUTOINLENH
                            318 	.globl _EP4AUTOINLENL
                            319 	.globl _EP4AUTOINLENH
                            320 	.globl _EP2AUTOINLENL
                            321 	.globl _EP2AUTOINLENH
                            322 	.globl _EP8FIFOCFG
                            323 	.globl _EP6FIFOCFG
                            324 	.globl _EP4FIFOCFG
                            325 	.globl _EP2FIFOCFG
                            326 	.globl _EP8CFG
                            327 	.globl _EP6CFG
                            328 	.globl _EP4CFG
                            329 	.globl _EP2CFG
                            330 	.globl _EP1INCFG
                            331 	.globl _EP1OUTCFG
                            332 	.globl _REVCTL
                            333 	.globl _REVID
                            334 	.globl _FIFOPINPOLAR
                            335 	.globl _UART230
                            336 	.globl _BPADDRL
                            337 	.globl _BPADDRH
                            338 	.globl _BREAKPT
                            339 	.globl _FIFORESET
                            340 	.globl _PINFLAGSCD
                            341 	.globl _PINFLAGSAB
                            342 	.globl _IFCONFIG
                            343 	.globl _CPUCS
                            344 	.globl _RES_WAVEDATA_END
                            345 	.globl _GPIF_WAVE_DATA
                            346 	.globl _putchar
                            347 	.globl _putstr
                            348 ;--------------------------------------------------------
                            349 ; special function registers
                            350 ;--------------------------------------------------------
                            351 	.area RSEG    (DATA)
                    0080    352 _IOA	=	0x0080
                    0081    353 _SP	=	0x0081
                    0082    354 _DPL	=	0x0082
                    0083    355 _DPH	=	0x0083
                    0084    356 _DPL1	=	0x0084
                    0085    357 _DPH1	=	0x0085
                    0086    358 _DPS	=	0x0086
                    0087    359 _PCON	=	0x0087
                    0088    360 _TCON	=	0x0088
                    0089    361 _TMOD	=	0x0089
                    008A    362 _TL0	=	0x008a
                    008B    363 _TL1	=	0x008b
                    008C    364 _TH0	=	0x008c
                    008D    365 _TH1	=	0x008d
                    008E    366 _CKCON	=	0x008e
                    0090    367 _IOB	=	0x0090
                    0091    368 _EXIF	=	0x0091
                    0092    369 _MPAGE	=	0x0092
                    0098    370 _SCON0	=	0x0098
                    0099    371 _SBUF0	=	0x0099
                    009A    372 _APTR1H	=	0x009a
                    009B    373 _APTR1L	=	0x009b
                    009C    374 _AUTODAT1	=	0x009c
                    009D    375 _AUTOPTRH2	=	0x009d
                    009E    376 _AUTOPTRL2	=	0x009e
                    009F    377 _AUTODAT2	=	0x009f
                    00A0    378 _IOC	=	0x00a0
                    00A1    379 _INT2CLR	=	0x00a1
                    00A2    380 _INT4CLR	=	0x00a2
                    00A8    381 _IE	=	0x00a8
                    00AA    382 _EP2468STAT	=	0x00aa
                    00AB    383 _EP24FIFOFLGS	=	0x00ab
                    00AC    384 _EP68FIFOFLGS	=	0x00ac
                    00AF    385 _AUTOPTRSETUP	=	0x00af
                    00B0    386 _IOD	=	0x00b0
                    00B1    387 _IOE	=	0x00b1
                    00B2    388 _OEA	=	0x00b2
                    00B3    389 _OEB	=	0x00b3
                    00B4    390 _OEC	=	0x00b4
                    00B5    391 _OED	=	0x00b5
                    00B6    392 _OEE	=	0x00b6
                    00B8    393 _IP	=	0x00b8
                    00BA    394 _EP01STAT	=	0x00ba
                    00BB    395 _GPIFTRIG	=	0x00bb
                    00BD    396 _GPIFSGLDATH	=	0x00bd
                    00BE    397 _GPIFSGLDATLX	=	0x00be
                    00BF    398 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    399 _SCON1	=	0x00c0
                    00C1    400 _SBUF1	=	0x00c1
                    00C8    401 _T2CON	=	0x00c8
                    00CA    402 _RCAP2L	=	0x00ca
                    00CB    403 _RCAP2H	=	0x00cb
                    00CC    404 _TL2	=	0x00cc
                    00CD    405 _TH2	=	0x00cd
                    00D0    406 _PSW	=	0x00d0
                    00D8    407 _EICON	=	0x00d8
                    00E0    408 _ACC	=	0x00e0
                    00E8    409 _EIE	=	0x00e8
                    00F0    410 _B	=	0x00f0
                    00F8    411 _EIP	=	0x00f8
                            412 ;--------------------------------------------------------
                            413 ; special function bits 
                            414 ;--------------------------------------------------------
                            415 	.area RSEG    (DATA)
                    0086    416 _SEL	=	0x0086
                    0088    417 _IT0	=	0x0088
                    0089    418 _IE0	=	0x0089
                    008A    419 _IT1	=	0x008a
                    008B    420 _IE1	=	0x008b
                    008C    421 _TR0	=	0x008c
                    008D    422 _TF0	=	0x008d
                    008E    423 _TR1	=	0x008e
                    008F    424 _TF1	=	0x008f
                    0098    425 _RI	=	0x0098
                    0099    426 _TI	=	0x0099
                    009A    427 _RB8	=	0x009a
                    009B    428 _TB8	=	0x009b
                    009C    429 _REN	=	0x009c
                    009D    430 _SM2	=	0x009d
                    009E    431 _SM1	=	0x009e
                    009F    432 _SM0	=	0x009f
                    00A8    433 _EX0	=	0x00a8
                    00A9    434 _ET0	=	0x00a9
                    00AA    435 _EX1	=	0x00aa
                    00AB    436 _ET1	=	0x00ab
                    00AC    437 _ES0	=	0x00ac
                    00AD    438 _ET2	=	0x00ad
                    00AE    439 _ES1	=	0x00ae
                    00AF    440 _EA	=	0x00af
                    00B8    441 _PX0	=	0x00b8
                    00B9    442 _PT0	=	0x00b9
                    00BA    443 _PX1	=	0x00ba
                    00BB    444 _PT1	=	0x00bb
                    00BC    445 _PS0	=	0x00bc
                    00BD    446 _PT2	=	0x00bd
                    00BE    447 _PS1	=	0x00be
                    00C0    448 _RI1	=	0x00c0
                    00C1    449 _TI1	=	0x00c1
                    00C2    450 _RB81	=	0x00c2
                    00C3    451 _TB81	=	0x00c3
                    00C4    452 _REN1	=	0x00c4
                    00C5    453 _SM21	=	0x00c5
                    00C6    454 _SM11	=	0x00c6
                    00C7    455 _SM01	=	0x00c7
                    00C8    456 _CP_RL2	=	0x00c8
                    00C9    457 _C_T2	=	0x00c9
                    00CA    458 _TR2	=	0x00ca
                    00CB    459 _EXEN2	=	0x00cb
                    00CC    460 _TCLK	=	0x00cc
                    00CD    461 _RCLK	=	0x00cd
                    00CE    462 _EXF2	=	0x00ce
                    00CF    463 _TF2	=	0x00cf
                    00D0    464 _P	=	0x00d0
                    00D1    465 _FL	=	0x00d1
                    00D2    466 _OV	=	0x00d2
                    00D3    467 _RS0	=	0x00d3
                    00D4    468 _RS1	=	0x00d4
                    00D5    469 _F0	=	0x00d5
                    00D6    470 _AC	=	0x00d6
                    00D7    471 _CY	=	0x00d7
                    00DB    472 _INT6	=	0x00db
                    00DC    473 _RESI	=	0x00dc
                    00DD    474 _ERESI	=	0x00dd
                    00DF    475 _SMOD1	=	0x00df
                    00E8    476 _EIUSB	=	0x00e8
                    00E9    477 _EI2C	=	0x00e9
                    00EA    478 _EIEX4	=	0x00ea
                    00EB    479 _EIEX5	=	0x00eb
                    00EC    480 _EIEX6	=	0x00ec
                    00F8    481 _PUSB	=	0x00f8
                    00F9    482 _PI2C	=	0x00f9
                    00FA    483 _EIPX4	=	0x00fa
                    00FB    484 _EIPX5	=	0x00fb
                    00FC    485 _EIPX6	=	0x00fc
                    0080    486 _bitS_OUT	=	0x0080
                    0081    487 _bitS_IN	=	0x0081
                    0083    488 _bitS_CLK	=	0x0083
                    00A0    489 _bitALTERA_DATA0	=	0x00a0
                    00A2    490 _bitALTERA_DCLK	=	0x00a2
                    00A6    491 _bitFPGA_CS	=	0x00a6
                    00A7    492 _bitCPLD_CS	=	0x00a7
                            493 ;--------------------------------------------------------
                            494 ; overlayable register banks 
                            495 ;--------------------------------------------------------
                            496 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     497 	.ds 8
                            498 ;--------------------------------------------------------
                            499 ; internal ram data
                            500 ;--------------------------------------------------------
                            501 	.area DSEG    (DATA)
                            502 ;--------------------------------------------------------
                            503 ; overlayable items in internal ram 
                            504 ;--------------------------------------------------------
                            505 	.area	OSEG    (OVR,DATA)
                            506 ;--------------------------------------------------------
                            507 ; indirectly addressable internal ram data
                            508 ;--------------------------------------------------------
                            509 	.area ISEG    (DATA)
                            510 ;--------------------------------------------------------
                            511 ; bit data
                            512 ;--------------------------------------------------------
                            513 	.area BSEG    (BIT)
                            514 ;--------------------------------------------------------
                            515 ; paged external ram data
                            516 ;--------------------------------------------------------
                            517 	.area PSEG    (PAG,XDATA)
                            518 ;--------------------------------------------------------
                            519 ; external ram data
                            520 ;--------------------------------------------------------
                            521 	.area XSEG    (XDATA)
                    E400    522 _GPIF_WAVE_DATA	=	0xe400
                    E480    523 _RES_WAVEDATA_END	=	0xe480
                    E600    524 _CPUCS	=	0xe600
                    E601    525 _IFCONFIG	=	0xe601
                    E602    526 _PINFLAGSAB	=	0xe602
                    E603    527 _PINFLAGSCD	=	0xe603
                    E604    528 _FIFORESET	=	0xe604
                    E605    529 _BREAKPT	=	0xe605
                    E606    530 _BPADDRH	=	0xe606
                    E607    531 _BPADDRL	=	0xe607
                    E608    532 _UART230	=	0xe608
                    E609    533 _FIFOPINPOLAR	=	0xe609
                    E60A    534 _REVID	=	0xe60a
                    E60B    535 _REVCTL	=	0xe60b
                    E610    536 _EP1OUTCFG	=	0xe610
                    E611    537 _EP1INCFG	=	0xe611
                    E612    538 _EP2CFG	=	0xe612
                    E613    539 _EP4CFG	=	0xe613
                    E614    540 _EP6CFG	=	0xe614
                    E615    541 _EP8CFG	=	0xe615
                    E618    542 _EP2FIFOCFG	=	0xe618
                    E619    543 _EP4FIFOCFG	=	0xe619
                    E61A    544 _EP6FIFOCFG	=	0xe61a
                    E61B    545 _EP8FIFOCFG	=	0xe61b
                    E620    546 _EP2AUTOINLENH	=	0xe620
                    E621    547 _EP2AUTOINLENL	=	0xe621
                    E622    548 _EP4AUTOINLENH	=	0xe622
                    E623    549 _EP4AUTOINLENL	=	0xe623
                    E624    550 _EP6AUTOINLENH	=	0xe624
                    E625    551 _EP6AUTOINLENL	=	0xe625
                    E626    552 _EP8AUTOINLENH	=	0xe626
                    E627    553 _EP8AUTOINLENL	=	0xe627
                    E630    554 _EP2FIFOPFH	=	0xe630
                    E631    555 _EP2FIFOPFL	=	0xe631
                    E632    556 _EP4FIFOPFH	=	0xe632
                    E633    557 _EP4FIFOPFL	=	0xe633
                    E634    558 _EP6FIFOPFH	=	0xe634
                    E635    559 _EP6FIFOPFL	=	0xe635
                    E636    560 _EP8FIFOPFH	=	0xe636
                    E637    561 _EP8FIFOPFL	=	0xe637
                    E640    562 _EP2ISOINPKTS	=	0xe640
                    E641    563 _EP4ISOINPKTS	=	0xe641
                    E642    564 _EP6ISOINPKTS	=	0xe642
                    E643    565 _EP8ISOINPKTS	=	0xe643
                    E648    566 _INPKTEND	=	0xe648
                    E649    567 _OUTPKTEND	=	0xe649
                    E650    568 _EP2FIFOIE	=	0xe650
                    E651    569 _EP2FIFOIRQ	=	0xe651
                    E652    570 _EP4FIFOIE	=	0xe652
                    E653    571 _EP4FIFOIRQ	=	0xe653
                    E654    572 _EP6FIFOIE	=	0xe654
                    E655    573 _EP6FIFOIRQ	=	0xe655
                    E656    574 _EP8FIFOIE	=	0xe656
                    E657    575 _EP8FIFOIRQ	=	0xe657
                    E658    576 _IBNIE	=	0xe658
                    E659    577 _IBNIRQ	=	0xe659
                    E65A    578 _NAKIE	=	0xe65a
                    E65B    579 _NAKIRQ	=	0xe65b
                    E65C    580 _USBIE	=	0xe65c
                    E65D    581 _USBIRQ	=	0xe65d
                    E65E    582 _EPIE	=	0xe65e
                    E65F    583 _EPIRQ	=	0xe65f
                    E660    584 _GPIFIE	=	0xe660
                    E661    585 _GPIFIRQ	=	0xe661
                    E662    586 _USBERRIE	=	0xe662
                    E663    587 _USBERRIRQ	=	0xe663
                    E664    588 _ERRCNTLIM	=	0xe664
                    E665    589 _CLRERRCNT	=	0xe665
                    E666    590 _INT2IVEC	=	0xe666
                    E667    591 _INT4IVEC	=	0xe667
                    E668    592 _INTSETUP	=	0xe668
                    E670    593 _PORTACFG	=	0xe670
                    E671    594 _PORTCCFG	=	0xe671
                    E672    595 _PORTECFG	=	0xe672
                    E678    596 _I2CS	=	0xe678
                    E679    597 _I2DAT	=	0xe679
                    E67A    598 _I2CTL	=	0xe67a
                    E67B    599 _XAUTODAT1	=	0xe67b
                    E67C    600 _XAUTODAT2	=	0xe67c
                    E680    601 _USBCS	=	0xe680
                    E681    602 _SUSPEND	=	0xe681
                    E682    603 _WAKEUPCS	=	0xe682
                    E683    604 _TOGCTL	=	0xe683
                    E684    605 _USBFRAMEH	=	0xe684
                    E685    606 _USBFRAMEL	=	0xe685
                    E686    607 _MICROFRAME	=	0xe686
                    E687    608 _FNADDR	=	0xe687
                    E68A    609 _EP0BCH	=	0xe68a
                    E68B    610 _EP0BCL	=	0xe68b
                    E68D    611 _EP1OUTBC	=	0xe68d
                    E68F    612 _EP1INBC	=	0xe68f
                    E690    613 _EP2BCH	=	0xe690
                    E691    614 _EP2BCL	=	0xe691
                    E694    615 _EP4BCH	=	0xe694
                    E695    616 _EP4BCL	=	0xe695
                    E698    617 _EP6BCH	=	0xe698
                    E699    618 _EP6BCL	=	0xe699
                    E69C    619 _EP8BCH	=	0xe69c
                    E69D    620 _EP8BCL	=	0xe69d
                    E6A0    621 _EP0CS	=	0xe6a0
                    E6A1    622 _EP1OUTCS	=	0xe6a1
                    E6A2    623 _EP1INCS	=	0xe6a2
                    E6A3    624 _EP2CS	=	0xe6a3
                    E6A4    625 _EP4CS	=	0xe6a4
                    E6A5    626 _EP6CS	=	0xe6a5
                    E6A6    627 _EP8CS	=	0xe6a6
                    E6A7    628 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    629 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    630 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    631 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    632 _EP2FIFOBCH	=	0xe6ab
                    E6AC    633 _EP2FIFOBCL	=	0xe6ac
                    E6AD    634 _EP4FIFOBCH	=	0xe6ad
                    E6AE    635 _EP4FIFOBCL	=	0xe6ae
                    E6AF    636 _EP6FIFOBCH	=	0xe6af
                    E6B0    637 _EP6FIFOBCL	=	0xe6b0
                    E6B1    638 _EP8FIFOBCH	=	0xe6b1
                    E6B2    639 _EP8FIFOBCL	=	0xe6b2
                    E6B3    640 _SUDPTRH	=	0xe6b3
                    E6B4    641 _SUDPTRL	=	0xe6b4
                    E6B5    642 _SUDPTRCTL	=	0xe6b5
                    E6B8    643 _SETUPDAT	=	0xe6b8
                    E6C0    644 _GPIFWFSELECT	=	0xe6c0
                    E6C1    645 _GPIFIDLECS	=	0xe6c1
                    E6C2    646 _GPIFIDLECTL	=	0xe6c2
                    E6C3    647 _GPIFCTLCFG	=	0xe6c3
                    E6C4    648 _GPIFADRH	=	0xe6c4
                    E6C5    649 _GPIFADRL	=	0xe6c5
                    E6CE    650 _GPIFTCB3	=	0xe6ce
                    E6CF    651 _GPIFTCB2	=	0xe6cf
                    E6D0    652 _GPIFTCB1	=	0xe6d0
                    E6D1    653 _GPIFTCB0	=	0xe6d1
                    E6D2    654 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    655 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    656 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    657 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    658 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    659 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    660 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    661 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    662 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    663 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    664 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    665 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    666 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    667 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    668 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    669 _GPIFREADYCFG	=	0xe6f3
                    E6F4    670 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    671 _GPIFABORT	=	0xe6f5
                    E6C6    672 _FLOWSTATE	=	0xe6c6
                    E6C7    673 _FLOWLOGIC	=	0xe6c7
                    E6C8    674 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    675 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    676 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    677 _FLOWSTB	=	0xe6cb
                    E6CC    678 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    679 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    680 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    681 _UDMACRCH	=	0xe67d
                    E67E    682 _UDMACRCL	=	0xe67e
                    E67F    683 _UDMACRCQUAL	=	0xe67f
                    E6F8    684 _DBUG	=	0xe6f8
                    E6F9    685 _TESTCFG	=	0xe6f9
                    E6FA    686 _USBTEST	=	0xe6fa
                    E6FB    687 _CT1	=	0xe6fb
                    E6FC    688 _CT2	=	0xe6fc
                    E6FD    689 _CT3	=	0xe6fd
                    E6FE    690 _CT4	=	0xe6fe
                    E740    691 _EP0BUF	=	0xe740
                    E780    692 _EP1OUTBUF	=	0xe780
                    E7C0    693 _EP1INBUF	=	0xe7c0
                    F000    694 _EP2FIFOBUF	=	0xf000
                    F400    695 _EP4FIFOBUF	=	0xf400
                    F800    696 _EP6FIFOBUF	=	0xf800
                    FC00    697 _EP8FIFOBUF	=	0xfc00
                    6000    698 _DDC0_DR0	=	0x6000
                    6001    699 _DDC0_DR1	=	0x6001
                    6002    700 _DDC0_DR2	=	0x6002
                    6003    701 _DDC0_DR3	=	0x6003
                    6004    702 _DDC0_DR4	=	0x6004
                    6006    703 _DDC0_LAR	=	0x6006
                    6007    704 _DDC0_AMR	=	0x6007
                    6010    705 _DDC1_DR0	=	0x6010
                    6011    706 _DDC1_DR1	=	0x6011
                    6012    707 _DDC1_DR2	=	0x6012
                    6013    708 _DDC1_DR3	=	0x6013
                    6014    709 _DDC1_DR4	=	0x6014
                    6016    710 _DDC1_LAR	=	0x6016
                    6017    711 _DDC1_AMR	=	0x6017
                    6020    712 _DDC_CNTRL	=	0x6020
                    6021    713 _LTC2208_CNTRL_REG	=	0x6021
                    6022    714 _DEBUG_LED_REG	=	0x6022
                    6023    715 _ATTN	=	0x6023
                    6024    716 _RFFE_CNTRL	=	0x6024
                    6025    717 _CPLD_GPIO	=	0x6025
                            718 ;--------------------------------------------------------
                            719 ; external initialized ram data
                            720 ;--------------------------------------------------------
                            721 	.area CSEG    (CODE)
                            722 	.area GSINIT0 (CODE)
                            723 	.area GSINIT1 (CODE)
                            724 	.area GSINIT2 (CODE)
                            725 	.area GSINIT3 (CODE)
                            726 	.area GSINIT4 (CODE)
                            727 	.area GSINIT5 (CODE)
                            728 ;--------------------------------------------------------
                            729 ; global & static initialisations
                            730 ;--------------------------------------------------------
                            731 	.area CSEG    (CODE)
                            732 	.area GSINIT  (CODE)
                            733 	.area GSFINAL (CODE)
                            734 	.area GSINIT  (CODE)
                            735 ;--------------------------------------------------------
                            736 ; Home
                            737 ;--------------------------------------------------------
                            738 	.area HOME    (CODE)
                            739 	.area CSEG    (CODE)
                            740 ;--------------------------------------------------------
                            741 ; code
                            742 ;--------------------------------------------------------
                            743 	.area CSEG    (CODE)
                            744 ;------------------------------------------------------------
                            745 ;Allocation info for local variables in function 'putchar'
                            746 ;------------------------------------------------------------
                            747 ;c                         Allocated to registers r2 
                            748 ;------------------------------------------------------------
                            749 ;src/board_specific.c:31: putchar(char c)
                            750 ;	-----------------------------------------
                            751 ;	 function putchar
                            752 ;	-----------------------------------------
   01A2                     753 _putchar:
                    0002    754 	ar2 = 0x02
                    0003    755 	ar3 = 0x03
                    0004    756 	ar4 = 0x04
                    0005    757 	ar5 = 0x05
                    0006    758 	ar6 = 0x06
                    0007    759 	ar7 = 0x07
                    0000    760 	ar0 = 0x00
                    0001    761 	ar1 = 0x01
                            762 ;     genReceive
   01A2 AA 82               763 	mov	r2,dpl
                            764 ;src/board_specific.c:33: while(!TI);
   01A4                     765 00101$:
                            766 ;     genIfx
                            767 ;     genIfxJump
                            768 ;	Peephole 111	removed ljmp by inverse jump logic
                            769 ;src/board_specific.c:34: TI=0;
                            770 ;     genAssign
                            771 ;	Peephole 250.a	using atomic test and clear
   01A4 10 99 02            772 	jbc	_TI,00108$
   01A7 80 FB               773 	sjmp	00101$
   01A9                     774 00108$:
                            775 ;src/board_specific.c:35: SBUF0 = c;
                            776 ;     genAssign
   01A9 8A 99               777 	mov	_SBUF0,r2
   01AB                     778 00104$:
   01AB 22                  779 	ret
                            780 ;------------------------------------------------------------
                            781 ;Allocation info for local variables in function 'putstr'
                            782 ;------------------------------------------------------------
                            783 ;s                         Allocated to registers r2 r3 r4 
                            784 ;i                         Allocated to registers r5 
                            785 ;c                         Allocated to registers r7 
                            786 ;------------------------------------------------------------
                            787 ;src/board_specific.c:38: void putstr(char *s)
                            788 ;	-----------------------------------------
                            789 ;	 function putstr
                            790 ;	-----------------------------------------
   01AC                     791 _putstr:
                            792 ;     genReceive
   01AC AA 82               793 	mov	r2,dpl
   01AE AB 83               794 	mov	r3,dph
   01B0 AC F0               795 	mov	r4,b
                            796 ;src/board_specific.c:42: while ((c=*(s+(i++)))!=0) putchar(c);
                            797 ;     genAssign
   01B2 7D 00               798 	mov	r5,#0x00
   01B4                     799 00101$:
                            800 ;     genAssign
   01B4 8D 06               801 	mov	ar6,r5
                            802 ;     genPlus
                            803 ;     genPlusIncr
   01B6 0D                  804 	inc	r5
                            805 ;     genPlus
                            806 ;	Peephole 236.g	used r6 instead of ar6
   01B7 EE                  807 	mov	a,r6
                            808 ;	Peephole 236.a	used r2 instead of ar2
   01B8 2A                  809 	add	a,r2
   01B9 FE                  810 	mov	r6,a
                            811 ;	Peephole 181	changed mov to clr
   01BA E4                  812 	clr	a
                            813 ;	Peephole 236.b	used r3 instead of ar3
   01BB 3B                  814 	addc	a,r3
   01BC FF                  815 	mov	r7,a
   01BD 8C 00               816 	mov	ar0,r4
                            817 ;     genPointerGet
                            818 ;     genGenPointerGet
   01BF 8E 82               819 	mov	dpl,r6
   01C1 8F 83               820 	mov	dph,r7
   01C3 88 F0               821 	mov	b,r0
   01C5 12 1F DD            822 	lcall	__gptrget
   01C8 FE                  823 	mov	r6,a
                            824 ;     genAssign
   01C9 8E 07               825 	mov	ar7,r6
                            826 ;     genCmpEq
   01CB BE 00 01            827 	cjne	r6,#0x00,00108$
                            828 ;	Peephole 112.b	changed ljmp to sjmp
                            829 ;	Peephole 251.b	replaced sjmp to ret with ret
   01CE 22                  830 	ret
   01CF                     831 00108$:
                            832 ;     genCall
   01CF 8F 82               833 	mov	dpl,r7
   01D1 C0 02               834 	push	ar2
   01D3 C0 03               835 	push	ar3
   01D5 C0 04               836 	push	ar4
   01D7 C0 05               837 	push	ar5
   01D9 12 01 A2            838 	lcall	_putchar
   01DC D0 05               839 	pop	ar5
   01DE D0 04               840 	pop	ar4
   01E0 D0 03               841 	pop	ar3
   01E2 D0 02               842 	pop	ar2
                            843 ;	Peephole 112.b	changed ljmp to sjmp
   01E4 80 CE               844 	sjmp	00101$
   01E6                     845 00104$:
   01E6 22                  846 	ret
                            847 ;------------------------------------------------------------
                            848 ;Allocation info for local variables in function 'init_board'
                            849 ;------------------------------------------------------------
                            850 ;------------------------------------------------------------
                            851 ;src/board_specific.c:46: init_board (void)
                            852 ;	-----------------------------------------
                            853 ;	 function init_board
                            854 ;	-----------------------------------------
   01E7                     855 _init_board:
                            856 ;src/board_specific.c:48: init_spi();
                            857 ;     genCall
                            858 ;	Peephole 253.b	replaced lcall/ret with ljmp
   01E7 02 06 82            859 	ljmp	_init_spi
                            860 	.area CSEG    (CODE)
