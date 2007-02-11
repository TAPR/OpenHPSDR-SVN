                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Fri Jan 05 10:13:17 2007
                              5 ;--------------------------------------------------------
                              6 	.module qs1r_common
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _bitCPLD_CS
                             13 	.globl _bitFPGA_CS
                             14 	.globl _bitALTERA_DCLK
                             15 	.globl _bitALTERA_DATA0
                             16 	.globl _bitS_CLK
                             17 	.globl _bitS_IN
                             18 	.globl _bitS_OUT
                             19 	.globl _EIPX6
                             20 	.globl _EIPX5
                             21 	.globl _EIPX4
                             22 	.globl _PI2C
                             23 	.globl _PUSB
                             24 	.globl _EIEX6
                             25 	.globl _EIEX5
                             26 	.globl _EIEX4
                             27 	.globl _EI2C
                             28 	.globl _EIUSB
                             29 	.globl _SMOD1
                             30 	.globl _ERESI
                             31 	.globl _RESI
                             32 	.globl _INT6
                             33 	.globl _CY
                             34 	.globl _AC
                             35 	.globl _F0
                             36 	.globl _RS1
                             37 	.globl _RS0
                             38 	.globl _OV
                             39 	.globl _FL
                             40 	.globl _P
                             41 	.globl _TF2
                             42 	.globl _EXF2
                             43 	.globl _RCLK
                             44 	.globl _TCLK
                             45 	.globl _EXEN2
                             46 	.globl _TR2
                             47 	.globl _C_T2
                             48 	.globl _CP_RL2
                             49 	.globl _SM01
                             50 	.globl _SM11
                             51 	.globl _SM21
                             52 	.globl _REN1
                             53 	.globl _TB81
                             54 	.globl _RB81
                             55 	.globl _TI1
                             56 	.globl _RI1
                             57 	.globl _PS1
                             58 	.globl _PT2
                             59 	.globl _PS0
                             60 	.globl _PT1
                             61 	.globl _PX1
                             62 	.globl _PT0
                             63 	.globl _PX0
                             64 	.globl _EA
                             65 	.globl _ES1
                             66 	.globl _ET2
                             67 	.globl _ES0
                             68 	.globl _ET1
                             69 	.globl _EX1
                             70 	.globl _ET0
                             71 	.globl _EX0
                             72 	.globl _SM0
                             73 	.globl _SM1
                             74 	.globl _SM2
                             75 	.globl _REN
                             76 	.globl _TB8
                             77 	.globl _RB8
                             78 	.globl _TI
                             79 	.globl _RI
                             80 	.globl _TF1
                             81 	.globl _TR1
                             82 	.globl _TF0
                             83 	.globl _TR0
                             84 	.globl _IE1
                             85 	.globl _IT1
                             86 	.globl _IE0
                             87 	.globl _IT0
                             88 	.globl _SEL
                             89 	.globl _EIP
                             90 	.globl _B
                             91 	.globl _EIE
                             92 	.globl _ACC
                             93 	.globl _EICON
                             94 	.globl _PSW
                             95 	.globl _TH2
                             96 	.globl _TL2
                             97 	.globl _RCAP2H
                             98 	.globl _RCAP2L
                             99 	.globl _T2CON
                            100 	.globl _SBUF1
                            101 	.globl _SCON1
                            102 	.globl _GPIFSGLDATLNOX
                            103 	.globl _GPIFSGLDATLX
                            104 	.globl _GPIFSGLDATH
                            105 	.globl _GPIFTRIG
                            106 	.globl _EP01STAT
                            107 	.globl _IP
                            108 	.globl _OEE
                            109 	.globl _OED
                            110 	.globl _OEC
                            111 	.globl _OEB
                            112 	.globl _OEA
                            113 	.globl _IOE
                            114 	.globl _IOD
                            115 	.globl _AUTOPTRSETUP
                            116 	.globl _EP68FIFOFLGS
                            117 	.globl _EP24FIFOFLGS
                            118 	.globl _EP2468STAT
                            119 	.globl _IE
                            120 	.globl _INT4CLR
                            121 	.globl _INT2CLR
                            122 	.globl _IOC
                            123 	.globl _AUTODAT2
                            124 	.globl _AUTOPTRL2
                            125 	.globl _AUTOPTRH2
                            126 	.globl _AUTODAT1
                            127 	.globl _APTR1L
                            128 	.globl _APTR1H
                            129 	.globl _SBUF0
                            130 	.globl _SCON0
                            131 	.globl _MPAGE
                            132 	.globl _EXIF
                            133 	.globl _IOB
                            134 	.globl _CKCON
                            135 	.globl _TH1
                            136 	.globl _TH0
                            137 	.globl _TL1
                            138 	.globl _TL0
                            139 	.globl _TMOD
                            140 	.globl _TCON
                            141 	.globl _PCON
                            142 	.globl _DPS
                            143 	.globl _DPH1
                            144 	.globl _DPL1
                            145 	.globl _DPH
                            146 	.globl _DPL
                            147 	.globl _SP
                            148 	.globl _IOA
                            149 	.globl _CPLD_GPIO
                            150 	.globl _RFFE_CNTRL
                            151 	.globl _ATTN
                            152 	.globl _DEBUG_LED_REG
                            153 	.globl _LTC2208_CNTRL_REG
                            154 	.globl _DDC_CNTRL
                            155 	.globl _DDC1_AMR
                            156 	.globl _DDC1_LAR
                            157 	.globl _DDC1_DR4
                            158 	.globl _DDC1_DR3
                            159 	.globl _DDC1_DR2
                            160 	.globl _DDC1_DR1
                            161 	.globl _DDC1_DR0
                            162 	.globl _DDC0_AMR
                            163 	.globl _DDC0_LAR
                            164 	.globl _DDC0_DR4
                            165 	.globl _DDC0_DR3
                            166 	.globl _DDC0_DR2
                            167 	.globl _DDC0_DR1
                            168 	.globl _DDC0_DR0
                            169 	.globl _EP8FIFOBUF
                            170 	.globl _EP6FIFOBUF
                            171 	.globl _EP4FIFOBUF
                            172 	.globl _EP2FIFOBUF
                            173 	.globl _EP1INBUF
                            174 	.globl _EP1OUTBUF
                            175 	.globl _EP0BUF
                            176 	.globl _CT4
                            177 	.globl _CT3
                            178 	.globl _CT2
                            179 	.globl _CT1
                            180 	.globl _USBTEST
                            181 	.globl _TESTCFG
                            182 	.globl _DBUG
                            183 	.globl _UDMACRCQUAL
                            184 	.globl _UDMACRCL
                            185 	.globl _UDMACRCH
                            186 	.globl _GPIFHOLDAMOUNT
                            187 	.globl _FLOWSTBHPERIOD
                            188 	.globl _FLOWSTBEDGE
                            189 	.globl _FLOWSTB
                            190 	.globl _FLOWHOLDOFF
                            191 	.globl _FLOWEQ1CTL
                            192 	.globl _FLOWEQ0CTL
                            193 	.globl _FLOWLOGIC
                            194 	.globl _FLOWSTATE
                            195 	.globl _GPIFABORT
                            196 	.globl _GPIFREADYSTAT
                            197 	.globl _GPIFREADYCFG
                            198 	.globl _XGPIFSGLDATLNOX
                            199 	.globl _XGPIFSGLDATLX
                            200 	.globl _XGPIFSGLDATH
                            201 	.globl _EP8GPIFTRIG
                            202 	.globl _EP8GPIFPFSTOP
                            203 	.globl _EP8GPIFFLGSEL
                            204 	.globl _EP6GPIFTRIG
                            205 	.globl _EP6GPIFPFSTOP
                            206 	.globl _EP6GPIFFLGSEL
                            207 	.globl _EP4GPIFTRIG
                            208 	.globl _EP4GPIFPFSTOP
                            209 	.globl _EP4GPIFFLGSEL
                            210 	.globl _EP2GPIFTRIG
                            211 	.globl _EP2GPIFPFSTOP
                            212 	.globl _EP2GPIFFLGSEL
                            213 	.globl _GPIFTCB0
                            214 	.globl _GPIFTCB1
                            215 	.globl _GPIFTCB2
                            216 	.globl _GPIFTCB3
                            217 	.globl _GPIFADRL
                            218 	.globl _GPIFADRH
                            219 	.globl _GPIFCTLCFG
                            220 	.globl _GPIFIDLECTL
                            221 	.globl _GPIFIDLECS
                            222 	.globl _GPIFWFSELECT
                            223 	.globl _SETUPDAT
                            224 	.globl _SUDPTRCTL
                            225 	.globl _SUDPTRL
                            226 	.globl _SUDPTRH
                            227 	.globl _EP8FIFOBCL
                            228 	.globl _EP8FIFOBCH
                            229 	.globl _EP6FIFOBCL
                            230 	.globl _EP6FIFOBCH
                            231 	.globl _EP4FIFOBCL
                            232 	.globl _EP4FIFOBCH
                            233 	.globl _EP2FIFOBCL
                            234 	.globl _EP2FIFOBCH
                            235 	.globl _EP8FIFOFLGS
                            236 	.globl _EP6FIFOFLGS
                            237 	.globl _EP4FIFOFLGS
                            238 	.globl _EP2FIFOFLGS
                            239 	.globl _EP8CS
                            240 	.globl _EP6CS
                            241 	.globl _EP4CS
                            242 	.globl _EP2CS
                            243 	.globl _EP1INCS
                            244 	.globl _EP1OUTCS
                            245 	.globl _EP0CS
                            246 	.globl _EP8BCL
                            247 	.globl _EP8BCH
                            248 	.globl _EP6BCL
                            249 	.globl _EP6BCH
                            250 	.globl _EP4BCL
                            251 	.globl _EP4BCH
                            252 	.globl _EP2BCL
                            253 	.globl _EP2BCH
                            254 	.globl _EP1INBC
                            255 	.globl _EP1OUTBC
                            256 	.globl _EP0BCL
                            257 	.globl _EP0BCH
                            258 	.globl _FNADDR
                            259 	.globl _MICROFRAME
                            260 	.globl _USBFRAMEL
                            261 	.globl _USBFRAMEH
                            262 	.globl _TOGCTL
                            263 	.globl _WAKEUPCS
                            264 	.globl _SUSPEND
                            265 	.globl _USBCS
                            266 	.globl _XAUTODAT2
                            267 	.globl _XAUTODAT1
                            268 	.globl _I2CTL
                            269 	.globl _I2DAT
                            270 	.globl _I2CS
                            271 	.globl _PORTECFG
                            272 	.globl _PORTCCFG
                            273 	.globl _PORTACFG
                            274 	.globl _INTSETUP
                            275 	.globl _INT4IVEC
                            276 	.globl _INT2IVEC
                            277 	.globl _CLRERRCNT
                            278 	.globl _ERRCNTLIM
                            279 	.globl _USBERRIRQ
                            280 	.globl _USBERRIE
                            281 	.globl _GPIFIRQ
                            282 	.globl _GPIFIE
                            283 	.globl _EPIRQ
                            284 	.globl _EPIE
                            285 	.globl _USBIRQ
                            286 	.globl _USBIE
                            287 	.globl _NAKIRQ
                            288 	.globl _NAKIE
                            289 	.globl _IBNIRQ
                            290 	.globl _IBNIE
                            291 	.globl _EP8FIFOIRQ
                            292 	.globl _EP8FIFOIE
                            293 	.globl _EP6FIFOIRQ
                            294 	.globl _EP6FIFOIE
                            295 	.globl _EP4FIFOIRQ
                            296 	.globl _EP4FIFOIE
                            297 	.globl _EP2FIFOIRQ
                            298 	.globl _EP2FIFOIE
                            299 	.globl _OUTPKTEND
                            300 	.globl _INPKTEND
                            301 	.globl _EP8ISOINPKTS
                            302 	.globl _EP6ISOINPKTS
                            303 	.globl _EP4ISOINPKTS
                            304 	.globl _EP2ISOINPKTS
                            305 	.globl _EP8FIFOPFL
                            306 	.globl _EP8FIFOPFH
                            307 	.globl _EP6FIFOPFL
                            308 	.globl _EP6FIFOPFH
                            309 	.globl _EP4FIFOPFL
                            310 	.globl _EP4FIFOPFH
                            311 	.globl _EP2FIFOPFL
                            312 	.globl _EP2FIFOPFH
                            313 	.globl _EP8AUTOINLENL
                            314 	.globl _EP8AUTOINLENH
                            315 	.globl _EP6AUTOINLENL
                            316 	.globl _EP6AUTOINLENH
                            317 	.globl _EP4AUTOINLENL
                            318 	.globl _EP4AUTOINLENH
                            319 	.globl _EP2AUTOINLENL
                            320 	.globl _EP2AUTOINLENH
                            321 	.globl _EP8FIFOCFG
                            322 	.globl _EP6FIFOCFG
                            323 	.globl _EP4FIFOCFG
                            324 	.globl _EP2FIFOCFG
                            325 	.globl _EP8CFG
                            326 	.globl _EP6CFG
                            327 	.globl _EP4CFG
                            328 	.globl _EP2CFG
                            329 	.globl _EP1INCFG
                            330 	.globl _EP1OUTCFG
                            331 	.globl _REVCTL
                            332 	.globl _REVID
                            333 	.globl _FIFOPINPOLAR
                            334 	.globl _UART230
                            335 	.globl _BPADDRL
                            336 	.globl _BPADDRH
                            337 	.globl _BREAKPT
                            338 	.globl _FIFORESET
                            339 	.globl _PINFLAGSCD
                            340 	.globl _PINFLAGSAB
                            341 	.globl _IFCONFIG
                            342 	.globl _CPUCS
                            343 	.globl _RES_WAVEDATA_END
                            344 	.globl _GPIF_WAVE_DATA
                            345 	.globl _init_qs1r
                            346 ;--------------------------------------------------------
                            347 ; special function registers
                            348 ;--------------------------------------------------------
                            349 	.area RSEG    (DATA)
                    0080    350 _IOA	=	0x0080
                    0081    351 _SP	=	0x0081
                    0082    352 _DPL	=	0x0082
                    0083    353 _DPH	=	0x0083
                    0084    354 _DPL1	=	0x0084
                    0085    355 _DPH1	=	0x0085
                    0086    356 _DPS	=	0x0086
                    0087    357 _PCON	=	0x0087
                    0088    358 _TCON	=	0x0088
                    0089    359 _TMOD	=	0x0089
                    008A    360 _TL0	=	0x008a
                    008B    361 _TL1	=	0x008b
                    008C    362 _TH0	=	0x008c
                    008D    363 _TH1	=	0x008d
                    008E    364 _CKCON	=	0x008e
                    0090    365 _IOB	=	0x0090
                    0091    366 _EXIF	=	0x0091
                    0092    367 _MPAGE	=	0x0092
                    0098    368 _SCON0	=	0x0098
                    0099    369 _SBUF0	=	0x0099
                    009A    370 _APTR1H	=	0x009a
                    009B    371 _APTR1L	=	0x009b
                    009C    372 _AUTODAT1	=	0x009c
                    009D    373 _AUTOPTRH2	=	0x009d
                    009E    374 _AUTOPTRL2	=	0x009e
                    009F    375 _AUTODAT2	=	0x009f
                    00A0    376 _IOC	=	0x00a0
                    00A1    377 _INT2CLR	=	0x00a1
                    00A2    378 _INT4CLR	=	0x00a2
                    00A8    379 _IE	=	0x00a8
                    00AA    380 _EP2468STAT	=	0x00aa
                    00AB    381 _EP24FIFOFLGS	=	0x00ab
                    00AC    382 _EP68FIFOFLGS	=	0x00ac
                    00AF    383 _AUTOPTRSETUP	=	0x00af
                    00B0    384 _IOD	=	0x00b0
                    00B1    385 _IOE	=	0x00b1
                    00B2    386 _OEA	=	0x00b2
                    00B3    387 _OEB	=	0x00b3
                    00B4    388 _OEC	=	0x00b4
                    00B5    389 _OED	=	0x00b5
                    00B6    390 _OEE	=	0x00b6
                    00B8    391 _IP	=	0x00b8
                    00BA    392 _EP01STAT	=	0x00ba
                    00BB    393 _GPIFTRIG	=	0x00bb
                    00BD    394 _GPIFSGLDATH	=	0x00bd
                    00BE    395 _GPIFSGLDATLX	=	0x00be
                    00BF    396 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    397 _SCON1	=	0x00c0
                    00C1    398 _SBUF1	=	0x00c1
                    00C8    399 _T2CON	=	0x00c8
                    00CA    400 _RCAP2L	=	0x00ca
                    00CB    401 _RCAP2H	=	0x00cb
                    00CC    402 _TL2	=	0x00cc
                    00CD    403 _TH2	=	0x00cd
                    00D0    404 _PSW	=	0x00d0
                    00D8    405 _EICON	=	0x00d8
                    00E0    406 _ACC	=	0x00e0
                    00E8    407 _EIE	=	0x00e8
                    00F0    408 _B	=	0x00f0
                    00F8    409 _EIP	=	0x00f8
                            410 ;--------------------------------------------------------
                            411 ; special function bits 
                            412 ;--------------------------------------------------------
                            413 	.area RSEG    (DATA)
                    0086    414 _SEL	=	0x0086
                    0088    415 _IT0	=	0x0088
                    0089    416 _IE0	=	0x0089
                    008A    417 _IT1	=	0x008a
                    008B    418 _IE1	=	0x008b
                    008C    419 _TR0	=	0x008c
                    008D    420 _TF0	=	0x008d
                    008E    421 _TR1	=	0x008e
                    008F    422 _TF1	=	0x008f
                    0098    423 _RI	=	0x0098
                    0099    424 _TI	=	0x0099
                    009A    425 _RB8	=	0x009a
                    009B    426 _TB8	=	0x009b
                    009C    427 _REN	=	0x009c
                    009D    428 _SM2	=	0x009d
                    009E    429 _SM1	=	0x009e
                    009F    430 _SM0	=	0x009f
                    00A8    431 _EX0	=	0x00a8
                    00A9    432 _ET0	=	0x00a9
                    00AA    433 _EX1	=	0x00aa
                    00AB    434 _ET1	=	0x00ab
                    00AC    435 _ES0	=	0x00ac
                    00AD    436 _ET2	=	0x00ad
                    00AE    437 _ES1	=	0x00ae
                    00AF    438 _EA	=	0x00af
                    00B8    439 _PX0	=	0x00b8
                    00B9    440 _PT0	=	0x00b9
                    00BA    441 _PX1	=	0x00ba
                    00BB    442 _PT1	=	0x00bb
                    00BC    443 _PS0	=	0x00bc
                    00BD    444 _PT2	=	0x00bd
                    00BE    445 _PS1	=	0x00be
                    00C0    446 _RI1	=	0x00c0
                    00C1    447 _TI1	=	0x00c1
                    00C2    448 _RB81	=	0x00c2
                    00C3    449 _TB81	=	0x00c3
                    00C4    450 _REN1	=	0x00c4
                    00C5    451 _SM21	=	0x00c5
                    00C6    452 _SM11	=	0x00c6
                    00C7    453 _SM01	=	0x00c7
                    00C8    454 _CP_RL2	=	0x00c8
                    00C9    455 _C_T2	=	0x00c9
                    00CA    456 _TR2	=	0x00ca
                    00CB    457 _EXEN2	=	0x00cb
                    00CC    458 _TCLK	=	0x00cc
                    00CD    459 _RCLK	=	0x00cd
                    00CE    460 _EXF2	=	0x00ce
                    00CF    461 _TF2	=	0x00cf
                    00D0    462 _P	=	0x00d0
                    00D1    463 _FL	=	0x00d1
                    00D2    464 _OV	=	0x00d2
                    00D3    465 _RS0	=	0x00d3
                    00D4    466 _RS1	=	0x00d4
                    00D5    467 _F0	=	0x00d5
                    00D6    468 _AC	=	0x00d6
                    00D7    469 _CY	=	0x00d7
                    00DB    470 _INT6	=	0x00db
                    00DC    471 _RESI	=	0x00dc
                    00DD    472 _ERESI	=	0x00dd
                    00DF    473 _SMOD1	=	0x00df
                    00E8    474 _EIUSB	=	0x00e8
                    00E9    475 _EI2C	=	0x00e9
                    00EA    476 _EIEX4	=	0x00ea
                    00EB    477 _EIEX5	=	0x00eb
                    00EC    478 _EIEX6	=	0x00ec
                    00F8    479 _PUSB	=	0x00f8
                    00F9    480 _PI2C	=	0x00f9
                    00FA    481 _EIPX4	=	0x00fa
                    00FB    482 _EIPX5	=	0x00fb
                    00FC    483 _EIPX6	=	0x00fc
                    0080    484 _bitS_OUT	=	0x0080
                    0081    485 _bitS_IN	=	0x0081
                    0083    486 _bitS_CLK	=	0x0083
                    00A0    487 _bitALTERA_DATA0	=	0x00a0
                    00A2    488 _bitALTERA_DCLK	=	0x00a2
                    00A6    489 _bitFPGA_CS	=	0x00a6
                    00A7    490 _bitCPLD_CS	=	0x00a7
                            491 ;--------------------------------------------------------
                            492 ; overlayable register banks 
                            493 ;--------------------------------------------------------
                            494 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     495 	.ds 8
                            496 ;--------------------------------------------------------
                            497 ; internal ram data
                            498 ;--------------------------------------------------------
                            499 	.area DSEG    (DATA)
                            500 ;--------------------------------------------------------
                            501 ; overlayable items in internal ram 
                            502 ;--------------------------------------------------------
                            503 	.area OSEG    (OVR,DATA)
                            504 ;--------------------------------------------------------
                            505 ; indirectly addressable internal ram data
                            506 ;--------------------------------------------------------
                            507 	.area ISEG    (DATA)
                            508 ;--------------------------------------------------------
                            509 ; bit data
                            510 ;--------------------------------------------------------
                            511 	.area BSEG    (BIT)
                            512 ;--------------------------------------------------------
                            513 ; paged external ram data
                            514 ;--------------------------------------------------------
                            515 	.area PSEG    (PAG,XDATA)
                            516 ;--------------------------------------------------------
                            517 ; external ram data
                            518 ;--------------------------------------------------------
                            519 	.area XSEG    (XDATA)
                    E400    520 _GPIF_WAVE_DATA	=	0xe400
                    E480    521 _RES_WAVEDATA_END	=	0xe480
                    E600    522 _CPUCS	=	0xe600
                    E601    523 _IFCONFIG	=	0xe601
                    E602    524 _PINFLAGSAB	=	0xe602
                    E603    525 _PINFLAGSCD	=	0xe603
                    E604    526 _FIFORESET	=	0xe604
                    E605    527 _BREAKPT	=	0xe605
                    E606    528 _BPADDRH	=	0xe606
                    E607    529 _BPADDRL	=	0xe607
                    E608    530 _UART230	=	0xe608
                    E609    531 _FIFOPINPOLAR	=	0xe609
                    E60A    532 _REVID	=	0xe60a
                    E60B    533 _REVCTL	=	0xe60b
                    E610    534 _EP1OUTCFG	=	0xe610
                    E611    535 _EP1INCFG	=	0xe611
                    E612    536 _EP2CFG	=	0xe612
                    E613    537 _EP4CFG	=	0xe613
                    E614    538 _EP6CFG	=	0xe614
                    E615    539 _EP8CFG	=	0xe615
                    E618    540 _EP2FIFOCFG	=	0xe618
                    E619    541 _EP4FIFOCFG	=	0xe619
                    E61A    542 _EP6FIFOCFG	=	0xe61a
                    E61B    543 _EP8FIFOCFG	=	0xe61b
                    E620    544 _EP2AUTOINLENH	=	0xe620
                    E621    545 _EP2AUTOINLENL	=	0xe621
                    E622    546 _EP4AUTOINLENH	=	0xe622
                    E623    547 _EP4AUTOINLENL	=	0xe623
                    E624    548 _EP6AUTOINLENH	=	0xe624
                    E625    549 _EP6AUTOINLENL	=	0xe625
                    E626    550 _EP8AUTOINLENH	=	0xe626
                    E627    551 _EP8AUTOINLENL	=	0xe627
                    E630    552 _EP2FIFOPFH	=	0xe630
                    E631    553 _EP2FIFOPFL	=	0xe631
                    E632    554 _EP4FIFOPFH	=	0xe632
                    E633    555 _EP4FIFOPFL	=	0xe633
                    E634    556 _EP6FIFOPFH	=	0xe634
                    E635    557 _EP6FIFOPFL	=	0xe635
                    E636    558 _EP8FIFOPFH	=	0xe636
                    E637    559 _EP8FIFOPFL	=	0xe637
                    E640    560 _EP2ISOINPKTS	=	0xe640
                    E641    561 _EP4ISOINPKTS	=	0xe641
                    E642    562 _EP6ISOINPKTS	=	0xe642
                    E643    563 _EP8ISOINPKTS	=	0xe643
                    E648    564 _INPKTEND	=	0xe648
                    E649    565 _OUTPKTEND	=	0xe649
                    E650    566 _EP2FIFOIE	=	0xe650
                    E651    567 _EP2FIFOIRQ	=	0xe651
                    E652    568 _EP4FIFOIE	=	0xe652
                    E653    569 _EP4FIFOIRQ	=	0xe653
                    E654    570 _EP6FIFOIE	=	0xe654
                    E655    571 _EP6FIFOIRQ	=	0xe655
                    E656    572 _EP8FIFOIE	=	0xe656
                    E657    573 _EP8FIFOIRQ	=	0xe657
                    E658    574 _IBNIE	=	0xe658
                    E659    575 _IBNIRQ	=	0xe659
                    E65A    576 _NAKIE	=	0xe65a
                    E65B    577 _NAKIRQ	=	0xe65b
                    E65C    578 _USBIE	=	0xe65c
                    E65D    579 _USBIRQ	=	0xe65d
                    E65E    580 _EPIE	=	0xe65e
                    E65F    581 _EPIRQ	=	0xe65f
                    E660    582 _GPIFIE	=	0xe660
                    E661    583 _GPIFIRQ	=	0xe661
                    E662    584 _USBERRIE	=	0xe662
                    E663    585 _USBERRIRQ	=	0xe663
                    E664    586 _ERRCNTLIM	=	0xe664
                    E665    587 _CLRERRCNT	=	0xe665
                    E666    588 _INT2IVEC	=	0xe666
                    E667    589 _INT4IVEC	=	0xe667
                    E668    590 _INTSETUP	=	0xe668
                    E670    591 _PORTACFG	=	0xe670
                    E671    592 _PORTCCFG	=	0xe671
                    E672    593 _PORTECFG	=	0xe672
                    E678    594 _I2CS	=	0xe678
                    E679    595 _I2DAT	=	0xe679
                    E67A    596 _I2CTL	=	0xe67a
                    E67B    597 _XAUTODAT1	=	0xe67b
                    E67C    598 _XAUTODAT2	=	0xe67c
                    E680    599 _USBCS	=	0xe680
                    E681    600 _SUSPEND	=	0xe681
                    E682    601 _WAKEUPCS	=	0xe682
                    E683    602 _TOGCTL	=	0xe683
                    E684    603 _USBFRAMEH	=	0xe684
                    E685    604 _USBFRAMEL	=	0xe685
                    E686    605 _MICROFRAME	=	0xe686
                    E687    606 _FNADDR	=	0xe687
                    E68A    607 _EP0BCH	=	0xe68a
                    E68B    608 _EP0BCL	=	0xe68b
                    E68D    609 _EP1OUTBC	=	0xe68d
                    E68F    610 _EP1INBC	=	0xe68f
                    E690    611 _EP2BCH	=	0xe690
                    E691    612 _EP2BCL	=	0xe691
                    E694    613 _EP4BCH	=	0xe694
                    E695    614 _EP4BCL	=	0xe695
                    E698    615 _EP6BCH	=	0xe698
                    E699    616 _EP6BCL	=	0xe699
                    E69C    617 _EP8BCH	=	0xe69c
                    E69D    618 _EP8BCL	=	0xe69d
                    E6A0    619 _EP0CS	=	0xe6a0
                    E6A1    620 _EP1OUTCS	=	0xe6a1
                    E6A2    621 _EP1INCS	=	0xe6a2
                    E6A3    622 _EP2CS	=	0xe6a3
                    E6A4    623 _EP4CS	=	0xe6a4
                    E6A5    624 _EP6CS	=	0xe6a5
                    E6A6    625 _EP8CS	=	0xe6a6
                    E6A7    626 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    627 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    628 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    629 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    630 _EP2FIFOBCH	=	0xe6ab
                    E6AC    631 _EP2FIFOBCL	=	0xe6ac
                    E6AD    632 _EP4FIFOBCH	=	0xe6ad
                    E6AE    633 _EP4FIFOBCL	=	0xe6ae
                    E6AF    634 _EP6FIFOBCH	=	0xe6af
                    E6B0    635 _EP6FIFOBCL	=	0xe6b0
                    E6B1    636 _EP8FIFOBCH	=	0xe6b1
                    E6B2    637 _EP8FIFOBCL	=	0xe6b2
                    E6B3    638 _SUDPTRH	=	0xe6b3
                    E6B4    639 _SUDPTRL	=	0xe6b4
                    E6B5    640 _SUDPTRCTL	=	0xe6b5
                    E6B8    641 _SETUPDAT	=	0xe6b8
                    E6C0    642 _GPIFWFSELECT	=	0xe6c0
                    E6C1    643 _GPIFIDLECS	=	0xe6c1
                    E6C2    644 _GPIFIDLECTL	=	0xe6c2
                    E6C3    645 _GPIFCTLCFG	=	0xe6c3
                    E6C4    646 _GPIFADRH	=	0xe6c4
                    E6C5    647 _GPIFADRL	=	0xe6c5
                    E6CE    648 _GPIFTCB3	=	0xe6ce
                    E6CF    649 _GPIFTCB2	=	0xe6cf
                    E6D0    650 _GPIFTCB1	=	0xe6d0
                    E6D1    651 _GPIFTCB0	=	0xe6d1
                    E6D2    652 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    653 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    654 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    655 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    656 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    657 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    658 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    659 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    660 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    661 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    662 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    663 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    664 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    665 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    666 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    667 _GPIFREADYCFG	=	0xe6f3
                    E6F4    668 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    669 _GPIFABORT	=	0xe6f5
                    E6C6    670 _FLOWSTATE	=	0xe6c6
                    E6C7    671 _FLOWLOGIC	=	0xe6c7
                    E6C8    672 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    673 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    674 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    675 _FLOWSTB	=	0xe6cb
                    E6CC    676 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    677 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    678 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    679 _UDMACRCH	=	0xe67d
                    E67E    680 _UDMACRCL	=	0xe67e
                    E67F    681 _UDMACRCQUAL	=	0xe67f
                    E6F8    682 _DBUG	=	0xe6f8
                    E6F9    683 _TESTCFG	=	0xe6f9
                    E6FA    684 _USBTEST	=	0xe6fa
                    E6FB    685 _CT1	=	0xe6fb
                    E6FC    686 _CT2	=	0xe6fc
                    E6FD    687 _CT3	=	0xe6fd
                    E6FE    688 _CT4	=	0xe6fe
                    E740    689 _EP0BUF	=	0xe740
                    E780    690 _EP1OUTBUF	=	0xe780
                    E7C0    691 _EP1INBUF	=	0xe7c0
                    F000    692 _EP2FIFOBUF	=	0xf000
                    F400    693 _EP4FIFOBUF	=	0xf400
                    F800    694 _EP6FIFOBUF	=	0xf800
                    FC00    695 _EP8FIFOBUF	=	0xfc00
                    6000    696 _DDC0_DR0	=	0x6000
                    6001    697 _DDC0_DR1	=	0x6001
                    6002    698 _DDC0_DR2	=	0x6002
                    6003    699 _DDC0_DR3	=	0x6003
                    6004    700 _DDC0_DR4	=	0x6004
                    6006    701 _DDC0_LAR	=	0x6006
                    6007    702 _DDC0_AMR	=	0x6007
                    6010    703 _DDC1_DR0	=	0x6010
                    6011    704 _DDC1_DR1	=	0x6011
                    6012    705 _DDC1_DR2	=	0x6012
                    6013    706 _DDC1_DR3	=	0x6013
                    6014    707 _DDC1_DR4	=	0x6014
                    6016    708 _DDC1_LAR	=	0x6016
                    6017    709 _DDC1_AMR	=	0x6017
                    6020    710 _DDC_CNTRL	=	0x6020
                    6021    711 _LTC2208_CNTRL_REG	=	0x6021
                    6022    712 _DEBUG_LED_REG	=	0x6022
                    6023    713 _ATTN	=	0x6023
                    6024    714 _RFFE_CNTRL	=	0x6024
                    6025    715 _CPLD_GPIO	=	0x6025
                            716 ;--------------------------------------------------------
                            717 ; external initialized ram data
                            718 ;--------------------------------------------------------
                            719 	.area CSEG    (CODE)
                            720 	.area GSINIT0 (CODE)
                            721 	.area GSINIT1 (CODE)
                            722 	.area GSINIT2 (CODE)
                            723 	.area GSINIT3 (CODE)
                            724 	.area GSINIT4 (CODE)
                            725 	.area GSINIT5 (CODE)
                            726 ;--------------------------------------------------------
                            727 ; global & static initialisations
                            728 ;--------------------------------------------------------
                            729 	.area CSEG    (CODE)
                            730 	.area GSINIT  (CODE)
                            731 	.area GSFINAL (CODE)
                            732 	.area GSINIT  (CODE)
                            733 ;--------------------------------------------------------
                            734 ; Home
                            735 ;--------------------------------------------------------
                            736 	.area HOME    (CODE)
                            737 	.area CSEG    (CODE)
                            738 ;--------------------------------------------------------
                            739 ; code
                            740 ;--------------------------------------------------------
                            741 	.area CSEG    (CODE)
                            742 ;------------------------------------------------------------
                            743 ;Allocation info for local variables in function 'init_qs1r'
                            744 ;------------------------------------------------------------
                            745 ;------------------------------------------------------------
                            746 ;src/qs1r_common.c:33: init_qs1r (void)
                            747 ;	-----------------------------------------
                            748 ;	 function init_qs1r
                            749 ;	-----------------------------------------
   0CAA                     750 _init_qs1r:
                    0002    751 	ar2 = 0x02
                    0003    752 	ar3 = 0x03
                    0004    753 	ar4 = 0x04
                    0005    754 	ar5 = 0x05
                    0006    755 	ar6 = 0x06
                    0007    756 	ar7 = 0x07
                    0000    757 	ar0 = 0x00
                    0001    758 	ar1 = 0x01
                            759 ;src/qs1r_common.c:35: CPUCS = bmCLKSPD1 | bmCLKOE;	// CPU runs @ 48 MHz  Changed 08/04/2006
                            760 ;     genAssign
   0CAA 90 E6 00            761 	mov	dptr,#_CPUCS
   0CAD 74 12               762 	mov	a,#0x12
   0CAF F0                  763 	movx	@dptr,a
                            764 ;src/qs1r_common.c:41: CKCON = 4;		// MOVX takes 6 cycles, needed for U25 MMIO CPLD on QS1R - PAC 12/27/2006
                            765 ;     genAssign
   0CB0 75 8E 04            766 	mov	_CKCON,#0x04
                            767 ;src/qs1r_common.c:44: IFCONFIG = bmIFCLKSRC | bm3048MHZ | bmIFCLKOE | bmIFFIFO;
                            768 ;     genAssign
   0CB3 90 E6 01            769 	mov	dptr,#_IFCONFIG
   0CB6 74 E3               770 	mov	a,#0xE3
   0CB8 F0                  771 	movx	@dptr,a
                            772 ;src/qs1r_common.c:47: SYNCDELAY;
                            773 ;     genInline
   0CB9 00                  774 	 nop; nop; nop; 
                            775 ;src/qs1r_common.c:48: FIFORESET = 0x80;
                            776 ;     genAssign
   0CBA 90 E6 04            777 	mov	dptr,#_FIFORESET
   0CBD 74 80               778 	mov	a,#0x80
   0CBF F0                  779 	movx	@dptr,a
                            780 ;src/qs1r_common.c:49: SYNCDELAY;
                            781 ;     genInline
   0CC0 00                  782 	 nop; nop; nop; 
                            783 ;src/qs1r_common.c:50: FIFORESET = 0x02;
                            784 ;     genAssign
   0CC1 90 E6 04            785 	mov	dptr,#_FIFORESET
   0CC4 74 02               786 	mov	a,#0x02
   0CC6 F0                  787 	movx	@dptr,a
                            788 ;src/qs1r_common.c:51: SYNCDELAY;
                            789 ;     genInline
   0CC7 00                  790 	 nop; nop; nop; 
                            791 ;src/qs1r_common.c:52: FIFORESET = 0x04;
                            792 ;     genAssign
   0CC8 90 E6 04            793 	mov	dptr,#_FIFORESET
   0CCB 74 04               794 	mov	a,#0x04
   0CCD F0                  795 	movx	@dptr,a
                            796 ;src/qs1r_common.c:53: SYNCDELAY;
                            797 ;     genInline
   0CCE 00                  798 	 nop; nop; nop; 
                            799 ;src/qs1r_common.c:54: FIFORESET = 0x06;
                            800 ;     genAssign
   0CCF 90 E6 04            801 	mov	dptr,#_FIFORESET
   0CD2 74 06               802 	mov	a,#0x06
   0CD4 F0                  803 	movx	@dptr,a
                            804 ;src/qs1r_common.c:55: SYNCDELAY;
                            805 ;     genInline
   0CD5 00                  806 	 nop; nop; nop; 
                            807 ;src/qs1r_common.c:56: FIFORESET = 0x08;
                            808 ;     genAssign
   0CD6 90 E6 04            809 	mov	dptr,#_FIFORESET
   0CD9 74 08               810 	mov	a,#0x08
   0CDB F0                  811 	movx	@dptr,a
                            812 ;src/qs1r_common.c:57: SYNCDELAY;
                            813 ;     genInline
   0CDC 00                  814 	 nop; nop; nop; 
                            815 ;src/qs1r_common.c:58: FIFORESET = 0x00;
                            816 ;     genAssign
   0CDD 90 E6 04            817 	mov	dptr,#_FIFORESET
                            818 ;	Peephole 181	changed mov to clr
   0CE0 E4                  819 	clr	a
   0CE1 F0                  820 	movx	@dptr,a
                            821 ;src/qs1r_common.c:59: SYNCDELAY;
                            822 ;     genInline
   0CE2 00                  823 	 nop; nop; nop; 
                            824 ;src/qs1r_common.c:63: IOA = bmPORT_A_INITIAL;	// Port A initial state
                            825 ;     genAssign
   0CE3 75 80 00            826 	mov	_IOA,#0x00
                            827 ;src/qs1r_common.c:64: OEA = bmPORT_A_OUTPUTS;	// Port A direction register
                            828 ;     genAssign
   0CE6 75 B2 09            829 	mov	_OEA,#0x09
                            830 ;src/qs1r_common.c:66: IOC = bmPORT_C_INITIAL;	// Port C initial state
                            831 ;     genAssign
   0CE9 75 A0 00            832 	mov	_IOC,#0x00
                            833 ;src/qs1r_common.c:67: OEC = bmPORT_C_OUTPUTS;	// Port C direction register
                            834 ;     genAssign
   0CEC 75 B4 E7            835 	mov	_OEC,#0xE7
                            836 ;src/qs1r_common.c:69: IOE = bmPORT_E_INITIAL;	// Port E initial state
                            837 ;     genAssign
   0CEF 75 B1 00            838 	mov	_IOE,#0x00
                            839 ;src/qs1r_common.c:70: OEE = bmPORT_E_OUTPUTS;	// Port E direction register
                            840 ;     genAssign
   0CF2 75 B6 FF            841 	mov	_OEE,#0xFF
                            842 ;src/qs1r_common.c:75: EP1OUTCFG = bmVALID | bmBULK;
                            843 ;     genAssign
   0CF5 90 E6 10            844 	mov	dptr,#_EP1OUTCFG
   0CF8 74 A0               845 	mov	a,#0xA0
   0CFA F0                  846 	movx	@dptr,a
                            847 ;src/qs1r_common.c:76: SYNCDELAY;
                            848 ;     genInline
   0CFB 00                  849 	 nop; nop; nop; 
                            850 ;src/qs1r_common.c:77: EP1INCFG = bmVALID | bmBULK;
                            851 ;     genAssign
   0CFC 90 E6 11            852 	mov	dptr,#_EP1INCFG
   0CFF 74 A0               853 	mov	a,#0xA0
   0D01 F0                  854 	movx	@dptr,a
                            855 ;src/qs1r_common.c:78: SYNCDELAY;
                            856 ;     genInline
   0D02 00                  857 	 nop; nop; nop; 
                            858 ;src/qs1r_common.c:79: EP2CFG = bmVALID | bmBULK | bmQUADBUF;
                            859 ;     genAssign
   0D03 90 E6 12            860 	mov	dptr,#_EP2CFG
   0D06 74 A0               861 	mov	a,#0xA0
   0D08 F0                  862 	movx	@dptr,a
                            863 ;src/qs1r_common.c:80: SYNCDELAY;
                            864 ;     genInline
   0D09 00                  865 	 nop; nop; nop; 
                            866 ;src/qs1r_common.c:81: EP6CFG = bmVALID | bmBULK | bmQUADBUF | bmIN;
                            867 ;     genAssign
   0D0A 90 E6 14            868 	mov	dptr,#_EP6CFG
   0D0D 74 E0               869 	mov	a,#0xE0
   0D0F F0                  870 	movx	@dptr,a
                            871 ;src/qs1r_common.c:82: SYNCDELAY;
                            872 ;     genInline
   0D10 00                  873 	 nop; nop; nop; 
                            874 ;src/qs1r_common.c:84: FIFOPINPOLAR=0x00; //default polarities: SLWR active low
                            875 ;     genAssign
   0D11 90 E6 09            876 	mov	dptr,#_FIFOPINPOLAR
                            877 ;	Peephole 181	changed mov to clr
   0D14 E4                  878 	clr	a
   0D15 F0                  879 	movx	@dptr,a
                            880 ;src/qs1r_common.c:86: SYNCDELAY;
                            881 ;     genInline
   0D16 00                  882 	 nop; nop; nop; 
                            883 ;src/qs1r_common.c:89: EP2FIFOCFG = bmWORDWIDE; //core needs to see 0 to 1 transistion of AUTOOUT
                            884 ;     genAssign
   0D17 90 E6 18            885 	mov	dptr,#_EP2FIFOCFG
   0D1A 74 01               886 	mov	a,#0x01
   0D1C F0                  887 	movx	@dptr,a
                            888 ;src/qs1r_common.c:90: SYNCDELAY;
                            889 ;     genInline
   0D1D 00                  890 	 nop; nop; nop; 
                            891 ;src/qs1r_common.c:91: EP2FIFOCFG = bmAUTOOUT | bmWORDWIDE;
                            892 ;     genAssign
   0D1E 90 E6 18            893 	mov	dptr,#_EP2FIFOCFG
   0D21 74 11               894 	mov	a,#0x11
   0D23 F0                  895 	movx	@dptr,a
                            896 ;src/qs1r_common.c:92: SYNCDELAY;
                            897 ;     genInline
   0D24 00                  898 	 nop; nop; nop; 
                            899 ;src/qs1r_common.c:93: EP2AUTOINLENH = 0x02; //MSB
                            900 ;     genAssign
   0D25 90 E6 20            901 	mov	dptr,#_EP2AUTOINLENH
   0D28 74 02               902 	mov	a,#0x02
   0D2A F0                  903 	movx	@dptr,a
                            904 ;src/qs1r_common.c:94: SYNCDELAY;
                            905 ;     genInline
   0D2B 00                  906 	 nop; nop; nop; 
                            907 ;src/qs1r_common.c:95: EP2AUTOINLENL = 0x00; //LSB
                            908 ;     genAssign
   0D2C 90 E6 21            909 	mov	dptr,#_EP2AUTOINLENL
                            910 ;	Peephole 181	changed mov to clr
   0D2F E4                  911 	clr	a
   0D30 F0                  912 	movx	@dptr,a
                            913 ;src/qs1r_common.c:96: SYNCDELAY;
                            914 ;     genInline
   0D31 00                  915 	 nop; nop; nop; 
                            916 ;src/qs1r_common.c:99: EP6FIFOCFG = bmAUTOIN | bmWORDWIDE;
                            917 ;     genAssign
   0D32 90 E6 1A            918 	mov	dptr,#_EP6FIFOCFG
   0D35 74 09               919 	mov	a,#0x09
   0D37 F0                  920 	movx	@dptr,a
                            921 ;src/qs1r_common.c:100: SYNCDELAY;
                            922 ;     genInline
   0D38 00                  923 	 nop; nop; nop; 
                            924 ;src/qs1r_common.c:101: EP6AUTOINLENH = 0x02; //MSB
                            925 ;     genAssign
   0D39 90 E6 24            926 	mov	dptr,#_EP6AUTOINLENH
   0D3C 74 02               927 	mov	a,#0x02
   0D3E F0                  928 	movx	@dptr,a
                            929 ;src/qs1r_common.c:102: SYNCDELAY;
                            930 ;     genInline
   0D3F 00                  931 	 nop; nop; nop; 
                            932 ;src/qs1r_common.c:103: EP6AUTOINLENL = 0x00; //LSB
                            933 ;     genAssign
   0D40 90 E6 25            934 	mov	dptr,#_EP6AUTOINLENL
                            935 ;	Peephole 181	changed mov to clr
   0D43 E4                  936 	clr	a
   0D44 F0                  937 	movx	@dptr,a
                            938 ;src/qs1r_common.c:104: SYNCDELAY;
                            939 ;     genInline
   0D45 00                  940 	 nop; nop; nop; 
                            941 ;src/qs1r_common.c:107: AUTOPTRSETUP |= 0x01;
                            942 ;     genOr
   0D46 43 AF 01            943 	orl	_AUTOPTRSETUP,#0x01
                            944 ;src/qs1r_common.c:108: SYNCDELAY;
                            945 ;     genInline
   0D49 00                  946 	 nop; nop; nop; 
                            947 ;src/qs1r_common.c:110: EP0BCH = 0;
                            948 ;     genAssign
   0D4A 90 E6 8A            949 	mov	dptr,#_EP0BCH
                            950 ;	Peephole 181	changed mov to clr
   0D4D E4                  951 	clr	a
   0D4E F0                  952 	movx	@dptr,a
                            953 ;src/qs1r_common.c:111: SYNCDELAY;
                            954 ;     genInline
   0D4F 00                  955 	 nop; nop; nop; 
                            956 ;src/qs1r_common.c:115: EP1OUTBC = 0;
                            957 ;     genAssign
   0D50 90 E6 8D            958 	mov	dptr,#_EP1OUTBC
                            959 ;	Peephole 181	changed mov to clr
   0D53 E4                  960 	clr	a
   0D54 F0                  961 	movx	@dptr,a
                            962 ;src/qs1r_common.c:116: SYNCDELAY;
                            963 ;     genInline
   0D55 00                  964 	 nop; nop; nop; 
                            965 ;src/qs1r_common.c:120: SCON0 = 0x50;
                            966 ;     genAssign
   0D56 75 98 50            967 	mov	_SCON0,#0x50
                            968 ;src/qs1r_common.c:121: TMOD |= 0x20;
                            969 ;     genOr
   0D59 43 89 20            970 	orl	_TMOD,#0x20
                            971 ;src/qs1r_common.c:122: TH1 = 0xB2;
                            972 ;     genAssign
   0D5C 75 8D B2            973 	mov	_TH1,#0xB2
                            974 ;src/qs1r_common.c:123: TR1 = 1;
                            975 ;     genAssign
   0D5F D2 8E               976 	setb	_TR1
                            977 ;src/qs1r_common.c:124: TI = 1;
                            978 ;     genAssign
   0D61 D2 99               979 	setb	_TI
                            980 ;src/qs1r_common.c:126: init_board ();
                            981 ;     genCall
                            982 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0D63 02 01 E7            983 	ljmp	_init_board
                            984 	.area CSEG    (CODE)
