                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Mon Aug 28 11:37:51 2006
                              5 ;--------------------------------------------------------
                              6 	.module hpsdr_main
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _main
                             13 	.globl _patch_usb_descriptors
                             14 	.globl _isr_tick
                             15 	.globl _app_vendor_IN_cmd
                             16 	.globl _app_vendor_OUT_cmd
                             17 	.globl _bitALTERA_DCLK
                             18 	.globl _bitALTERA_DATA0
                             19 	.globl _bitS_IN
                             20 	.globl _bitS_OUT
                             21 	.globl _bitS_CLK
                             22 	.globl _EIPX6
                             23 	.globl _EIPX5
                             24 	.globl _EIPX4
                             25 	.globl _PI2C
                             26 	.globl _PUSB
                             27 	.globl _EIEX6
                             28 	.globl _EIEX5
                             29 	.globl _EIEX4
                             30 	.globl _EI2C
                             31 	.globl _EIUSB
                             32 	.globl _SMOD1
                             33 	.globl _ERESI
                             34 	.globl _RESI
                             35 	.globl _INT6
                             36 	.globl _CY
                             37 	.globl _AC
                             38 	.globl _F0
                             39 	.globl _RS1
                             40 	.globl _RS0
                             41 	.globl _OV
                             42 	.globl _FL
                             43 	.globl _P
                             44 	.globl _TF2
                             45 	.globl _EXF2
                             46 	.globl _RCLK
                             47 	.globl _TCLK
                             48 	.globl _EXEN2
                             49 	.globl _TR2
                             50 	.globl _C_T2
                             51 	.globl _CP_RL2
                             52 	.globl _SM01
                             53 	.globl _SM11
                             54 	.globl _SM21
                             55 	.globl _REN1
                             56 	.globl _TB81
                             57 	.globl _RB81
                             58 	.globl _TI1
                             59 	.globl _RI1
                             60 	.globl _PS1
                             61 	.globl _PT2
                             62 	.globl _PS0
                             63 	.globl _PT1
                             64 	.globl _PX1
                             65 	.globl _PT0
                             66 	.globl _PX0
                             67 	.globl _EA
                             68 	.globl _ES1
                             69 	.globl _ET2
                             70 	.globl _ES0
                             71 	.globl _ET1
                             72 	.globl _EX1
                             73 	.globl _ET0
                             74 	.globl _EX0
                             75 	.globl _SM0
                             76 	.globl _SM1
                             77 	.globl _SM2
                             78 	.globl _REN
                             79 	.globl _TB8
                             80 	.globl _RB8
                             81 	.globl _TI
                             82 	.globl _RI
                             83 	.globl _TF1
                             84 	.globl _TR1
                             85 	.globl _TF0
                             86 	.globl _TR0
                             87 	.globl _IE1
                             88 	.globl _IT1
                             89 	.globl _IE0
                             90 	.globl _IT0
                             91 	.globl _SEL
                             92 	.globl _EIP
                             93 	.globl _B
                             94 	.globl _EIE
                             95 	.globl _ACC
                             96 	.globl _EICON
                             97 	.globl _PSW
                             98 	.globl _TH2
                             99 	.globl _TL2
                            100 	.globl _RCAP2H
                            101 	.globl _RCAP2L
                            102 	.globl _T2CON
                            103 	.globl _SBUF1
                            104 	.globl _SCON1
                            105 	.globl _GPIFSGLDATLNOX
                            106 	.globl _GPIFSGLDATLX
                            107 	.globl _GPIFSGLDATH
                            108 	.globl _GPIFTRIG
                            109 	.globl _EP01STAT
                            110 	.globl _IP
                            111 	.globl _OEE
                            112 	.globl _OED
                            113 	.globl _OEC
                            114 	.globl _OEB
                            115 	.globl _OEA
                            116 	.globl _IOE
                            117 	.globl _IOD
                            118 	.globl _AUTOPTRSETUP
                            119 	.globl _EP68FIFOFLGS
                            120 	.globl _EP24FIFOFLGS
                            121 	.globl _EP2468STAT
                            122 	.globl _IE
                            123 	.globl _INT4CLR
                            124 	.globl _INT2CLR
                            125 	.globl _IOC
                            126 	.globl _AUTODAT2
                            127 	.globl _AUTOPTRL2
                            128 	.globl _AUTOPTRH2
                            129 	.globl _AUTODAT1
                            130 	.globl _APTR1L
                            131 	.globl _APTR1H
                            132 	.globl _SBUF0
                            133 	.globl _SCON0
                            134 	.globl _MPAGE
                            135 	.globl _EXIF
                            136 	.globl _IOB
                            137 	.globl _CKCON
                            138 	.globl _TH1
                            139 	.globl _TH0
                            140 	.globl _TL1
                            141 	.globl _TL0
                            142 	.globl _TMOD
                            143 	.globl _TCON
                            144 	.globl _PCON
                            145 	.globl _DPS
                            146 	.globl _DPH1
                            147 	.globl _DPL1
                            148 	.globl _DPH
                            149 	.globl _DPL
                            150 	.globl _SP
                            151 	.globl _IOA
                            152 	.globl _EP8FIFOBUF
                            153 	.globl _EP6FIFOBUF
                            154 	.globl _EP4FIFOBUF
                            155 	.globl _EP2FIFOBUF
                            156 	.globl _EP1INBUF
                            157 	.globl _EP1OUTBUF
                            158 	.globl _EP0BUF
                            159 	.globl _CT4
                            160 	.globl _CT3
                            161 	.globl _CT2
                            162 	.globl _CT1
                            163 	.globl _USBTEST
                            164 	.globl _TESTCFG
                            165 	.globl _DBUG
                            166 	.globl _UDMACRCQUAL
                            167 	.globl _UDMACRCL
                            168 	.globl _UDMACRCH
                            169 	.globl _GPIFHOLDAMOUNT
                            170 	.globl _FLOWSTBHPERIOD
                            171 	.globl _FLOWSTBEDGE
                            172 	.globl _FLOWSTB
                            173 	.globl _FLOWHOLDOFF
                            174 	.globl _FLOWEQ1CTL
                            175 	.globl _FLOWEQ0CTL
                            176 	.globl _FLOWLOGIC
                            177 	.globl _FLOWSTATE
                            178 	.globl _GPIFABORT
                            179 	.globl _GPIFREADYSTAT
                            180 	.globl _GPIFREADYCFG
                            181 	.globl _XGPIFSGLDATLNOX
                            182 	.globl _XGPIFSGLDATLX
                            183 	.globl _XGPIFSGLDATH
                            184 	.globl _EP8GPIFTRIG
                            185 	.globl _EP8GPIFPFSTOP
                            186 	.globl _EP8GPIFFLGSEL
                            187 	.globl _EP6GPIFTRIG
                            188 	.globl _EP6GPIFPFSTOP
                            189 	.globl _EP6GPIFFLGSEL
                            190 	.globl _EP4GPIFTRIG
                            191 	.globl _EP4GPIFPFSTOP
                            192 	.globl _EP4GPIFFLGSEL
                            193 	.globl _EP2GPIFTRIG
                            194 	.globl _EP2GPIFPFSTOP
                            195 	.globl _EP2GPIFFLGSEL
                            196 	.globl _GPIFTCB0
                            197 	.globl _GPIFTCB1
                            198 	.globl _GPIFTCB2
                            199 	.globl _GPIFTCB3
                            200 	.globl _GPIFADRL
                            201 	.globl _GPIFADRH
                            202 	.globl _GPIFCTLCFG
                            203 	.globl _GPIFIDLECTL
                            204 	.globl _GPIFIDLECS
                            205 	.globl _GPIFWFSELECT
                            206 	.globl _SETUPDAT
                            207 	.globl _SUDPTRCTL
                            208 	.globl _SUDPTRL
                            209 	.globl _SUDPTRH
                            210 	.globl _EP8FIFOBCL
                            211 	.globl _EP8FIFOBCH
                            212 	.globl _EP6FIFOBCL
                            213 	.globl _EP6FIFOBCH
                            214 	.globl _EP4FIFOBCL
                            215 	.globl _EP4FIFOBCH
                            216 	.globl _EP2FIFOBCL
                            217 	.globl _EP2FIFOBCH
                            218 	.globl _EP8FIFOFLGS
                            219 	.globl _EP6FIFOFLGS
                            220 	.globl _EP4FIFOFLGS
                            221 	.globl _EP2FIFOFLGS
                            222 	.globl _EP8CS
                            223 	.globl _EP6CS
                            224 	.globl _EP4CS
                            225 	.globl _EP2CS
                            226 	.globl _EP1INCS
                            227 	.globl _EP1OUTCS
                            228 	.globl _EP0CS
                            229 	.globl _EP8BCL
                            230 	.globl _EP8BCH
                            231 	.globl _EP6BCL
                            232 	.globl _EP6BCH
                            233 	.globl _EP4BCL
                            234 	.globl _EP4BCH
                            235 	.globl _EP2BCL
                            236 	.globl _EP2BCH
                            237 	.globl _EP1INBC
                            238 	.globl _EP1OUTBC
                            239 	.globl _EP0BCL
                            240 	.globl _EP0BCH
                            241 	.globl _FNADDR
                            242 	.globl _MICROFRAME
                            243 	.globl _USBFRAMEL
                            244 	.globl _USBFRAMEH
                            245 	.globl _TOGCTL
                            246 	.globl _WAKEUPCS
                            247 	.globl _SUSPEND
                            248 	.globl _USBCS
                            249 	.globl _XAUTODAT2
                            250 	.globl _XAUTODAT1
                            251 	.globl _I2CTL
                            252 	.globl _I2DAT
                            253 	.globl _I2CS
                            254 	.globl _PORTECFG
                            255 	.globl _PORTCCFG
                            256 	.globl _PORTACFG
                            257 	.globl _INTSETUP
                            258 	.globl _INT4IVEC
                            259 	.globl _INT2IVEC
                            260 	.globl _CLRERRCNT
                            261 	.globl _ERRCNTLIM
                            262 	.globl _USBERRIRQ
                            263 	.globl _USBERRIE
                            264 	.globl _GPIFIRQ
                            265 	.globl _GPIFIE
                            266 	.globl _EPIRQ
                            267 	.globl _EPIE
                            268 	.globl _USBIRQ
                            269 	.globl _USBIE
                            270 	.globl _NAKIRQ
                            271 	.globl _NAKIE
                            272 	.globl _IBNIRQ
                            273 	.globl _IBNIE
                            274 	.globl _EP8FIFOIRQ
                            275 	.globl _EP8FIFOIE
                            276 	.globl _EP6FIFOIRQ
                            277 	.globl _EP6FIFOIE
                            278 	.globl _EP4FIFOIRQ
                            279 	.globl _EP4FIFOIE
                            280 	.globl _EP2FIFOIRQ
                            281 	.globl _EP2FIFOIE
                            282 	.globl _OUTPKTEND
                            283 	.globl _INPKTEND
                            284 	.globl _EP8ISOINPKTS
                            285 	.globl _EP6ISOINPKTS
                            286 	.globl _EP4ISOINPKTS
                            287 	.globl _EP2ISOINPKTS
                            288 	.globl _EP8FIFOPFL
                            289 	.globl _EP8FIFOPFH
                            290 	.globl _EP6FIFOPFL
                            291 	.globl _EP6FIFOPFH
                            292 	.globl _EP4FIFOPFL
                            293 	.globl _EP4FIFOPFH
                            294 	.globl _EP2FIFOPFL
                            295 	.globl _EP2FIFOPFH
                            296 	.globl _EP8AUTOINLENL
                            297 	.globl _EP8AUTOINLENH
                            298 	.globl _EP6AUTOINLENL
                            299 	.globl _EP6AUTOINLENH
                            300 	.globl _EP4AUTOINLENL
                            301 	.globl _EP4AUTOINLENH
                            302 	.globl _EP2AUTOINLENL
                            303 	.globl _EP2AUTOINLENH
                            304 	.globl _EP8FIFOCFG
                            305 	.globl _EP6FIFOCFG
                            306 	.globl _EP4FIFOCFG
                            307 	.globl _EP2FIFOCFG
                            308 	.globl _EP8CFG
                            309 	.globl _EP6CFG
                            310 	.globl _EP4CFG
                            311 	.globl _EP2CFG
                            312 	.globl _EP1INCFG
                            313 	.globl _EP1OUTCFG
                            314 	.globl _REVCTL
                            315 	.globl _REVID
                            316 	.globl _FIFOPINPOLAR
                            317 	.globl _UART230
                            318 	.globl _BPADDRL
                            319 	.globl _BPADDRH
                            320 	.globl _BREAKPT
                            321 	.globl _FIFORESET
                            322 	.globl _PINFLAGSCD
                            323 	.globl _PINFLAGSAB
                            324 	.globl _IFCONFIG
                            325 	.globl _CPUCS
                            326 	.globl _RES_WAVEDATA_END
                            327 	.globl _GPIF_WAVE_DATA
                            328 	.globl _app_vendor_cmd
                            329 ;--------------------------------------------------------
                            330 ; special function registers
                            331 ;--------------------------------------------------------
                            332 	.area RSEG    (DATA)
                    0080    333 _IOA	=	0x0080
                    0081    334 _SP	=	0x0081
                    0082    335 _DPL	=	0x0082
                    0083    336 _DPH	=	0x0083
                    0084    337 _DPL1	=	0x0084
                    0085    338 _DPH1	=	0x0085
                    0086    339 _DPS	=	0x0086
                    0087    340 _PCON	=	0x0087
                    0088    341 _TCON	=	0x0088
                    0089    342 _TMOD	=	0x0089
                    008A    343 _TL0	=	0x008a
                    008B    344 _TL1	=	0x008b
                    008C    345 _TH0	=	0x008c
                    008D    346 _TH1	=	0x008d
                    008E    347 _CKCON	=	0x008e
                    0090    348 _IOB	=	0x0090
                    0091    349 _EXIF	=	0x0091
                    0092    350 _MPAGE	=	0x0092
                    0098    351 _SCON0	=	0x0098
                    0099    352 _SBUF0	=	0x0099
                    009A    353 _APTR1H	=	0x009a
                    009B    354 _APTR1L	=	0x009b
                    009C    355 _AUTODAT1	=	0x009c
                    009D    356 _AUTOPTRH2	=	0x009d
                    009E    357 _AUTOPTRL2	=	0x009e
                    009F    358 _AUTODAT2	=	0x009f
                    00A0    359 _IOC	=	0x00a0
                    00A1    360 _INT2CLR	=	0x00a1
                    00A2    361 _INT4CLR	=	0x00a2
                    00A8    362 _IE	=	0x00a8
                    00AA    363 _EP2468STAT	=	0x00aa
                    00AB    364 _EP24FIFOFLGS	=	0x00ab
                    00AC    365 _EP68FIFOFLGS	=	0x00ac
                    00AF    366 _AUTOPTRSETUP	=	0x00af
                    00B0    367 _IOD	=	0x00b0
                    00B1    368 _IOE	=	0x00b1
                    00B2    369 _OEA	=	0x00b2
                    00B3    370 _OEB	=	0x00b3
                    00B4    371 _OEC	=	0x00b4
                    00B5    372 _OED	=	0x00b5
                    00B6    373 _OEE	=	0x00b6
                    00B8    374 _IP	=	0x00b8
                    00BA    375 _EP01STAT	=	0x00ba
                    00BB    376 _GPIFTRIG	=	0x00bb
                    00BD    377 _GPIFSGLDATH	=	0x00bd
                    00BE    378 _GPIFSGLDATLX	=	0x00be
                    00BF    379 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    380 _SCON1	=	0x00c0
                    00C1    381 _SBUF1	=	0x00c1
                    00C8    382 _T2CON	=	0x00c8
                    00CA    383 _RCAP2L	=	0x00ca
                    00CB    384 _RCAP2H	=	0x00cb
                    00CC    385 _TL2	=	0x00cc
                    00CD    386 _TH2	=	0x00cd
                    00D0    387 _PSW	=	0x00d0
                    00D8    388 _EICON	=	0x00d8
                    00E0    389 _ACC	=	0x00e0
                    00E8    390 _EIE	=	0x00e8
                    00F0    391 _B	=	0x00f0
                    00F8    392 _EIP	=	0x00f8
                            393 ;--------------------------------------------------------
                            394 ; special function bits 
                            395 ;--------------------------------------------------------
                            396 	.area RSEG    (DATA)
                    0086    397 _SEL	=	0x0086
                    0088    398 _IT0	=	0x0088
                    0089    399 _IE0	=	0x0089
                    008A    400 _IT1	=	0x008a
                    008B    401 _IE1	=	0x008b
                    008C    402 _TR0	=	0x008c
                    008D    403 _TF0	=	0x008d
                    008E    404 _TR1	=	0x008e
                    008F    405 _TF1	=	0x008f
                    0098    406 _RI	=	0x0098
                    0099    407 _TI	=	0x0099
                    009A    408 _RB8	=	0x009a
                    009B    409 _TB8	=	0x009b
                    009C    410 _REN	=	0x009c
                    009D    411 _SM2	=	0x009d
                    009E    412 _SM1	=	0x009e
                    009F    413 _SM0	=	0x009f
                    00A8    414 _EX0	=	0x00a8
                    00A9    415 _ET0	=	0x00a9
                    00AA    416 _EX1	=	0x00aa
                    00AB    417 _ET1	=	0x00ab
                    00AC    418 _ES0	=	0x00ac
                    00AD    419 _ET2	=	0x00ad
                    00AE    420 _ES1	=	0x00ae
                    00AF    421 _EA	=	0x00af
                    00B8    422 _PX0	=	0x00b8
                    00B9    423 _PT0	=	0x00b9
                    00BA    424 _PX1	=	0x00ba
                    00BB    425 _PT1	=	0x00bb
                    00BC    426 _PS0	=	0x00bc
                    00BD    427 _PT2	=	0x00bd
                    00BE    428 _PS1	=	0x00be
                    00C0    429 _RI1	=	0x00c0
                    00C1    430 _TI1	=	0x00c1
                    00C2    431 _RB81	=	0x00c2
                    00C3    432 _TB81	=	0x00c3
                    00C4    433 _REN1	=	0x00c4
                    00C5    434 _SM21	=	0x00c5
                    00C6    435 _SM11	=	0x00c6
                    00C7    436 _SM01	=	0x00c7
                    00C8    437 _CP_RL2	=	0x00c8
                    00C9    438 _C_T2	=	0x00c9
                    00CA    439 _TR2	=	0x00ca
                    00CB    440 _EXEN2	=	0x00cb
                    00CC    441 _TCLK	=	0x00cc
                    00CD    442 _RCLK	=	0x00cd
                    00CE    443 _EXF2	=	0x00ce
                    00CF    444 _TF2	=	0x00cf
                    00D0    445 _P	=	0x00d0
                    00D1    446 _FL	=	0x00d1
                    00D2    447 _OV	=	0x00d2
                    00D3    448 _RS0	=	0x00d3
                    00D4    449 _RS1	=	0x00d4
                    00D5    450 _F0	=	0x00d5
                    00D6    451 _AC	=	0x00d6
                    00D7    452 _CY	=	0x00d7
                    00DB    453 _INT6	=	0x00db
                    00DC    454 _RESI	=	0x00dc
                    00DD    455 _ERESI	=	0x00dd
                    00DF    456 _SMOD1	=	0x00df
                    00E8    457 _EIUSB	=	0x00e8
                    00E9    458 _EI2C	=	0x00e9
                    00EA    459 _EIEX4	=	0x00ea
                    00EB    460 _EIEX5	=	0x00eb
                    00EC    461 _EIEX6	=	0x00ec
                    00F8    462 _PUSB	=	0x00f8
                    00F9    463 _PI2C	=	0x00f9
                    00FA    464 _EIPX4	=	0x00fa
                    00FB    465 _EIPX5	=	0x00fb
                    00FC    466 _EIPX6	=	0x00fc
                    0080    467 _bitS_CLK	=	0x0080
                    0081    468 _bitS_OUT	=	0x0081
                    0083    469 _bitS_IN	=	0x0083
                    00A0    470 _bitALTERA_DATA0	=	0x00a0
                    00A2    471 _bitALTERA_DCLK	=	0x00a2
                            472 ;--------------------------------------------------------
                            473 ; overlayable register banks 
                            474 ;--------------------------------------------------------
                            475 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     476 	.ds 8
                            477 ;--------------------------------------------------------
                            478 ; internal ram data
                            479 ;--------------------------------------------------------
                            480 	.area DSEG    (DATA)
   0008                     481 _isr_tick_count_1_1:
   0008                     482 	.ds 1
                            483 ;--------------------------------------------------------
                            484 ; overlayable items in internal ram 
                            485 ;--------------------------------------------------------
                            486 	.area OSEG    (OVR,DATA)
                            487 ;--------------------------------------------------------
                            488 ; Stack segment in internal ram 
                            489 ;--------------------------------------------------------
                            490 	.area	SSEG	(DATA)
   002F                     491 __start__stack:
   002F                     492 	.ds	1
                            493 
                            494 ;--------------------------------------------------------
                            495 ; indirectly addressable internal ram data
                            496 ;--------------------------------------------------------
                            497 	.area ISEG    (DATA)
                            498 ;--------------------------------------------------------
                            499 ; bit data
                            500 ;--------------------------------------------------------
                            501 	.area BSEG    (BIT)
                            502 ;--------------------------------------------------------
                            503 ; paged external ram data
                            504 ;--------------------------------------------------------
                            505 	.area PSEG    (PAG,XDATA)
                            506 ;--------------------------------------------------------
                            507 ; external ram data
                            508 ;--------------------------------------------------------
                            509 	.area XSEG    (XDATA)
                    E400    510 _GPIF_WAVE_DATA	=	0xe400
                    E480    511 _RES_WAVEDATA_END	=	0xe480
                    E600    512 _CPUCS	=	0xe600
                    E601    513 _IFCONFIG	=	0xe601
                    E602    514 _PINFLAGSAB	=	0xe602
                    E603    515 _PINFLAGSCD	=	0xe603
                    E604    516 _FIFORESET	=	0xe604
                    E605    517 _BREAKPT	=	0xe605
                    E606    518 _BPADDRH	=	0xe606
                    E607    519 _BPADDRL	=	0xe607
                    E608    520 _UART230	=	0xe608
                    E609    521 _FIFOPINPOLAR	=	0xe609
                    E60A    522 _REVID	=	0xe60a
                    E60B    523 _REVCTL	=	0xe60b
                    E610    524 _EP1OUTCFG	=	0xe610
                    E611    525 _EP1INCFG	=	0xe611
                    E612    526 _EP2CFG	=	0xe612
                    E613    527 _EP4CFG	=	0xe613
                    E614    528 _EP6CFG	=	0xe614
                    E615    529 _EP8CFG	=	0xe615
                    E618    530 _EP2FIFOCFG	=	0xe618
                    E619    531 _EP4FIFOCFG	=	0xe619
                    E61A    532 _EP6FIFOCFG	=	0xe61a
                    E61B    533 _EP8FIFOCFG	=	0xe61b
                    E620    534 _EP2AUTOINLENH	=	0xe620
                    E621    535 _EP2AUTOINLENL	=	0xe621
                    E622    536 _EP4AUTOINLENH	=	0xe622
                    E623    537 _EP4AUTOINLENL	=	0xe623
                    E624    538 _EP6AUTOINLENH	=	0xe624
                    E625    539 _EP6AUTOINLENL	=	0xe625
                    E626    540 _EP8AUTOINLENH	=	0xe626
                    E627    541 _EP8AUTOINLENL	=	0xe627
                    E630    542 _EP2FIFOPFH	=	0xe630
                    E631    543 _EP2FIFOPFL	=	0xe631
                    E632    544 _EP4FIFOPFH	=	0xe632
                    E633    545 _EP4FIFOPFL	=	0xe633
                    E634    546 _EP6FIFOPFH	=	0xe634
                    E635    547 _EP6FIFOPFL	=	0xe635
                    E636    548 _EP8FIFOPFH	=	0xe636
                    E637    549 _EP8FIFOPFL	=	0xe637
                    E640    550 _EP2ISOINPKTS	=	0xe640
                    E641    551 _EP4ISOINPKTS	=	0xe641
                    E642    552 _EP6ISOINPKTS	=	0xe642
                    E643    553 _EP8ISOINPKTS	=	0xe643
                    E648    554 _INPKTEND	=	0xe648
                    E649    555 _OUTPKTEND	=	0xe649
                    E650    556 _EP2FIFOIE	=	0xe650
                    E651    557 _EP2FIFOIRQ	=	0xe651
                    E652    558 _EP4FIFOIE	=	0xe652
                    E653    559 _EP4FIFOIRQ	=	0xe653
                    E654    560 _EP6FIFOIE	=	0xe654
                    E655    561 _EP6FIFOIRQ	=	0xe655
                    E656    562 _EP8FIFOIE	=	0xe656
                    E657    563 _EP8FIFOIRQ	=	0xe657
                    E658    564 _IBNIE	=	0xe658
                    E659    565 _IBNIRQ	=	0xe659
                    E65A    566 _NAKIE	=	0xe65a
                    E65B    567 _NAKIRQ	=	0xe65b
                    E65C    568 _USBIE	=	0xe65c
                    E65D    569 _USBIRQ	=	0xe65d
                    E65E    570 _EPIE	=	0xe65e
                    E65F    571 _EPIRQ	=	0xe65f
                    E660    572 _GPIFIE	=	0xe660
                    E661    573 _GPIFIRQ	=	0xe661
                    E662    574 _USBERRIE	=	0xe662
                    E663    575 _USBERRIRQ	=	0xe663
                    E664    576 _ERRCNTLIM	=	0xe664
                    E665    577 _CLRERRCNT	=	0xe665
                    E666    578 _INT2IVEC	=	0xe666
                    E667    579 _INT4IVEC	=	0xe667
                    E668    580 _INTSETUP	=	0xe668
                    E670    581 _PORTACFG	=	0xe670
                    E671    582 _PORTCCFG	=	0xe671
                    E672    583 _PORTECFG	=	0xe672
                    E678    584 _I2CS	=	0xe678
                    E679    585 _I2DAT	=	0xe679
                    E67A    586 _I2CTL	=	0xe67a
                    E67B    587 _XAUTODAT1	=	0xe67b
                    E67C    588 _XAUTODAT2	=	0xe67c
                    E680    589 _USBCS	=	0xe680
                    E681    590 _SUSPEND	=	0xe681
                    E682    591 _WAKEUPCS	=	0xe682
                    E683    592 _TOGCTL	=	0xe683
                    E684    593 _USBFRAMEH	=	0xe684
                    E685    594 _USBFRAMEL	=	0xe685
                    E686    595 _MICROFRAME	=	0xe686
                    E687    596 _FNADDR	=	0xe687
                    E68A    597 _EP0BCH	=	0xe68a
                    E68B    598 _EP0BCL	=	0xe68b
                    E68D    599 _EP1OUTBC	=	0xe68d
                    E68F    600 _EP1INBC	=	0xe68f
                    E690    601 _EP2BCH	=	0xe690
                    E691    602 _EP2BCL	=	0xe691
                    E694    603 _EP4BCH	=	0xe694
                    E695    604 _EP4BCL	=	0xe695
                    E698    605 _EP6BCH	=	0xe698
                    E699    606 _EP6BCL	=	0xe699
                    E69C    607 _EP8BCH	=	0xe69c
                    E69D    608 _EP8BCL	=	0xe69d
                    E6A0    609 _EP0CS	=	0xe6a0
                    E6A1    610 _EP1OUTCS	=	0xe6a1
                    E6A2    611 _EP1INCS	=	0xe6a2
                    E6A3    612 _EP2CS	=	0xe6a3
                    E6A4    613 _EP4CS	=	0xe6a4
                    E6A5    614 _EP6CS	=	0xe6a5
                    E6A6    615 _EP8CS	=	0xe6a6
                    E6A7    616 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    617 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    618 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    619 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    620 _EP2FIFOBCH	=	0xe6ab
                    E6AC    621 _EP2FIFOBCL	=	0xe6ac
                    E6AD    622 _EP4FIFOBCH	=	0xe6ad
                    E6AE    623 _EP4FIFOBCL	=	0xe6ae
                    E6AF    624 _EP6FIFOBCH	=	0xe6af
                    E6B0    625 _EP6FIFOBCL	=	0xe6b0
                    E6B1    626 _EP8FIFOBCH	=	0xe6b1
                    E6B2    627 _EP8FIFOBCL	=	0xe6b2
                    E6B3    628 _SUDPTRH	=	0xe6b3
                    E6B4    629 _SUDPTRL	=	0xe6b4
                    E6B5    630 _SUDPTRCTL	=	0xe6b5
                    E6B8    631 _SETUPDAT	=	0xe6b8
                    E6C0    632 _GPIFWFSELECT	=	0xe6c0
                    E6C1    633 _GPIFIDLECS	=	0xe6c1
                    E6C2    634 _GPIFIDLECTL	=	0xe6c2
                    E6C3    635 _GPIFCTLCFG	=	0xe6c3
                    E6C4    636 _GPIFADRH	=	0xe6c4
                    E6C5    637 _GPIFADRL	=	0xe6c5
                    E6CE    638 _GPIFTCB3	=	0xe6ce
                    E6CF    639 _GPIFTCB2	=	0xe6cf
                    E6D0    640 _GPIFTCB1	=	0xe6d0
                    E6D1    641 _GPIFTCB0	=	0xe6d1
                    E6D2    642 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    643 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    644 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    645 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    646 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    647 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    648 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    649 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    650 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    651 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    652 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    653 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    654 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    655 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    656 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    657 _GPIFREADYCFG	=	0xe6f3
                    E6F4    658 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    659 _GPIFABORT	=	0xe6f5
                    E6C6    660 _FLOWSTATE	=	0xe6c6
                    E6C7    661 _FLOWLOGIC	=	0xe6c7
                    E6C8    662 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    663 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    664 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    665 _FLOWSTB	=	0xe6cb
                    E6CC    666 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    667 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    668 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    669 _UDMACRCH	=	0xe67d
                    E67E    670 _UDMACRCL	=	0xe67e
                    E67F    671 _UDMACRCQUAL	=	0xe67f
                    E6F8    672 _DBUG	=	0xe6f8
                    E6F9    673 _TESTCFG	=	0xe6f9
                    E6FA    674 _USBTEST	=	0xe6fa
                    E6FB    675 _CT1	=	0xe6fb
                    E6FC    676 _CT2	=	0xe6fc
                    E6FD    677 _CT3	=	0xe6fd
                    E6FE    678 _CT4	=	0xe6fe
                    E740    679 _EP0BUF	=	0xe740
                    E780    680 _EP1OUTBUF	=	0xe780
                    E7C0    681 _EP1INBUF	=	0xe7c0
                    F000    682 _EP2FIFOBUF	=	0xf000
                    F400    683 _EP4FIFOBUF	=	0xf400
                    F800    684 _EP6FIFOBUF	=	0xf800
                    FC00    685 _EP8FIFOBUF	=	0xfc00
   2000                     686 _patch_usb_descriptors_hw_rev_1_1:
   2000                     687 	.ds 1
   2001                     688 _patch_usb_descriptors_serial_no_1_1:
   2001                     689 	.ds 8
                            690 ;--------------------------------------------------------
                            691 ; external initialized ram data
                            692 ;--------------------------------------------------------
                            693 	.area CSEG    (CODE)
                            694 	.area GSINIT0 (CODE)
                            695 	.area GSINIT1 (CODE)
                            696 	.area GSINIT2 (CODE)
                            697 	.area GSINIT3 (CODE)
                            698 	.area GSINIT4 (CODE)
                            699 	.area GSINIT5 (CODE)
                            700 ;--------------------------------------------------------
                            701 ; interrupt vector 
                            702 ;--------------------------------------------------------
                            703 	.area CSEG    (CODE)
   01A2                     704 __interrupt_vect:
   01A2 02 10 4C            705 	ljmp	__sdcc_gsinit_startup
                            706 ;--------------------------------------------------------
                            707 ; global & static initialisations
                            708 ;--------------------------------------------------------
                            709 	.area CSEG    (CODE)
                            710 	.area GSINIT  (CODE)
                            711 	.area GSFINAL (CODE)
                            712 	.area GSINIT  (CODE)
                            713 	.globl __sdcc_gsinit_startup
                            714 	.globl __sdcc_program_startup
                            715 	.globl __start__stack
                            716 	.globl __mcs51_genRAMCLEAR
                            717 ;------------------------------------------------------------
                            718 ;Allocation info for local variables in function 'isr_tick'
                            719 ;------------------------------------------------------------
                            720 ;count                     Allocated with name '_isr_tick_count_1_1'
                            721 ;------------------------------------------------------------
                            722 ;Initial/src/hpsdr_main.c:210: static unsigned char	count = 1;
                            723 ;     genAssign
   1043 75 08 01            724 	mov	_isr_tick_count_1_1,#0x01
                            725 	.area GSFINAL (CODE)
   105F 02 01 A5            726 	ljmp	__sdcc_program_startup
                            727 ;--------------------------------------------------------
                            728 ; Home
                            729 ;--------------------------------------------------------
                            730 	.area HOME    (CODE)
                            731 	.area CSEG    (CODE)
                            732 ;--------------------------------------------------------
                            733 ; code
                            734 ;--------------------------------------------------------
                            735 	.area CSEG    (CODE)
   01A5                     736 __sdcc_program_startup:
   01A5 12 04 09            737 	lcall	_main
                            738 ;	return from main will lock up
   01A8 80 FE               739 	sjmp .
                            740 ;------------------------------------------------------------
                            741 ;Allocation info for local variables in function 'get_ep0_data'
                            742 ;------------------------------------------------------------
                            743 ;------------------------------------------------------------
                            744 ;Initial/src/hpsdr_main.c:53: get_ep0_data (void)
                            745 ;	-----------------------------------------
                            746 ;	 function get_ep0_data
                            747 ;	-----------------------------------------
   01AA                     748 _get_ep0_data:
                    0002    749 	ar2 = 0x02
                    0003    750 	ar3 = 0x03
                    0004    751 	ar4 = 0x04
                    0005    752 	ar5 = 0x05
                    0006    753 	ar6 = 0x06
                    0007    754 	ar7 = 0x07
                    0000    755 	ar0 = 0x00
                    0001    756 	ar1 = 0x01
                            757 ;Initial/src/hpsdr_main.c:55: EP0BCL = 0;			// arm EP0 for OUT xfer.  This sets the busy bit
                            758 ;     genAssign
   01AA 90 E6 8B            759 	mov	dptr,#_EP0BCL
                            760 ;	Peephole 181	changed mov to clr
   01AD E4                  761 	clr	a
   01AE F0                  762 	movx	@dptr,a
                            763 ;Initial/src/hpsdr_main.c:56: while (EP0CS & bmEPBUSY);	// wait for busy to clear
   01AF                     764 00101$:
                            765 ;     genAssign
   01AF 90 E6 A0            766 	mov	dptr,#_EP0CS
   01B2 E0                  767 	movx	a,@dptr
                            768 ;     genAnd
                            769 ;	Peephole 105	removed redundant mov
   01B3 FA                  770 	mov	r2,a
                            771 ;     genIfxJump
                            772 ;	Peephole 112.a	removed ljmp by inverse jump logic
   01B4 20 E1 F8            773 	jb	acc.1,00101$
   01B7                     774 00107$:
   01B7                     775 00104$:
   01B7 22                  776 	ret
                            777 ;------------------------------------------------------------
                            778 ;Allocation info for local variables in function 'app_vendor_OUT_cmd'
                            779 ;------------------------------------------------------------
                            780 ;------------------------------------------------------------
                            781 ;Initial/src/hpsdr_main.c:64: unsigned char app_vendor_OUT_cmd(void)
                            782 ;	-----------------------------------------
                            783 ;	 function app_vendor_OUT_cmd
                            784 ;	-----------------------------------------
   01B8                     785 _app_vendor_OUT_cmd:
                            786 ;Initial/src/hpsdr_main.c:66: switch (bRequest)
                            787 ;     genPointerGet
                            788 ;     genFarPointerGet
   01B8 90 E6 B9            789 	mov	dptr,#(_SETUPDAT + 0x0001)
   01BB E0                  790 	movx	a,@dptr
   01BC FA                  791 	mov	r2,a
                            792 ;     genCmpEq
   01BD BA 01 02            793 	cjne	r2,#0x01,00149$
                            794 ;	Peephole 112.b	changed ljmp to sjmp
   01C0 80 20               795 	sjmp	00101$
   01C2                     796 00149$:
                            797 ;     genCmpEq
   01C2 BA 02 02            798 	cjne	r2,#0x02,00150$
                            799 ;	Peephole 112.b	changed ljmp to sjmp
   01C5 80 45               800 	sjmp	00106$
   01C7                     801 00150$:
                            802 ;     genCmpEq
   01C7 BA 08 03            803 	cjne	r2,#0x08,00151$
   01CA 02 02 36            804 	ljmp	00112$
   01CD                     805 00151$:
                            806 ;     genCmpEq
   01CD BA 09 03            807 	cjne	r2,#0x09,00152$
   01D0 02 02 5A            808 	ljmp	00115$
   01D3                     809 00152$:
                            810 ;     genCmpEq
   01D3 BA 0B 03            811 	cjne	r2,#0x0B,00153$
   01D6 02 02 8C            812 	ljmp	00118$
   01D9                     813 00153$:
                            814 ;     genCmpEq
   01D9 BA 0C 03            815 	cjne	r2,#0x0C,00154$
   01DC 02 02 A8            816 	ljmp	00122$
   01DF                     817 00154$:
   01DF 02 02 D1            818 	ljmp	00129$
                            819 ;Initial/src/hpsdr_main.c:69: case VRQ_SET_LED:
   01E2                     820 00101$:
                            821 ;Initial/src/hpsdr_main.c:71: switch (wIndexL)
                            822 ;     genPointerGet
                            823 ;     genFarPointerGet
   01E2 90 E6 BC            824 	mov	dptr,#(_SETUPDAT + 0x0004)
   01E5 E0                  825 	movx	a,@dptr
                            826 ;     genCmpEq
                            827 ;	Peephole 112.b	changed ljmp to sjmp
                            828 ;	Peephole 115.b	jump optimization
   01E6 FA                  829 	mov	r2,a
   01E7 60 05               830 	jz	00102$
   01E9                     831 00155$:
                            832 ;     genCmpEq
                            833 ;	Peephole 112.b	changed ljmp to sjmp
                            834 ;Initial/src/hpsdr_main.c:73: case 0:
                            835 ;	Peephole 112.b	changed ljmp to sjmp
                            836 ;	Peephole 199	optimized misc jump sequence
   01E9 BA 01 1C            837 	cjne	r2,#0x01,00104$
   01EC 80 0D               838 	sjmp	00103$
                            839 ;00156$:
   01EE                     840 00102$:
                            841 ;Initial/src/hpsdr_main.c:74: set_led_0 (wValueL);
                            842 ;     genPointerGet
                            843 ;     genFarPointerGet
   01EE 90 E6 BA            844 	mov	dptr,#(_SETUPDAT + 0x0002)
   01F1 E0                  845 	movx	a,@dptr
                            846 ;     genCall
                            847 ;	Peephole 244.c	loading dpl from a instead of r2
   01F2 FA                  848 	mov	r2,a
   01F3 F5 82               849 	mov	dpl,a
   01F5 12 06 24            850 	lcall	_set_led_0
                            851 ;Initial/src/hpsdr_main.c:75: break;
   01F8 02 02 D5            852 	ljmp	00130$
                            853 ;Initial/src/hpsdr_main.c:77: case 1:
   01FB                     854 00103$:
                            855 ;Initial/src/hpsdr_main.c:78: set_led_1 (wValueL);
                            856 ;     genPointerGet
                            857 ;     genFarPointerGet
   01FB 90 E6 BA            858 	mov	dptr,#(_SETUPDAT + 0x0002)
   01FE E0                  859 	movx	a,@dptr
                            860 ;     genCall
                            861 ;	Peephole 244.c	loading dpl from a instead of r2
   01FF FA                  862 	mov	r2,a
   0200 F5 82               863 	mov	dpl,a
   0202 12 06 31            864 	lcall	_set_led_1
                            865 ;Initial/src/hpsdr_main.c:79: break;
   0205 02 02 D5            866 	ljmp	00130$
                            867 ;Initial/src/hpsdr_main.c:81: default:
   0208                     868 00104$:
                            869 ;Initial/src/hpsdr_main.c:82: return 0;
                            870 ;     genRet
   0208 75 82 00            871 	mov	dpl,#0x00
                            872 ;	Peephole 251.a	replaced ljmp to ret with ret
   020B 22                  873 	ret
                            874 ;Initial/src/hpsdr_main.c:86: case VRQ_FPGA_LOAD:
   020C                     875 00106$:
                            876 ;Initial/src/hpsdr_main.c:88: switch (wIndexL)			// sub-command
                            877 ;     genPointerGet
                            878 ;     genFarPointerGet
   020C 90 E6 BC            879 	mov	dptr,#(_SETUPDAT + 0x0004)
   020F E0                  880 	movx	a,@dptr
                            881 ;     genCmpEq
                            882 ;	Peephole 112.b	changed ljmp to sjmp
                            883 ;	Peephole 115.b	jump optimization
   0210 FA                  884 	mov	r2,a
   0211 60 0A               885 	jz	00107$
   0213                     886 00157$:
                            887 ;     genCmpEq
   0213 BA 01 02            888 	cjne	r2,#0x01,00158$
                            889 ;	Peephole 112.b	changed ljmp to sjmp
   0216 80 08               890 	sjmp	00108$
   0218                     891 00158$:
                            892 ;     genCmpEq
                            893 ;	Peephole 112.b	changed ljmp to sjmp
                            894 ;Initial/src/hpsdr_main.c:90: case FL_BEGIN:
                            895 ;	Peephole 112.b	changed ljmp to sjmp
                            896 ;	Peephole 199	optimized misc jump sequence
   0218 BA 02 17            897 	cjne	r2,#0x02,00110$
   021B 80 12               898 	sjmp	00109$
                            899 ;00159$:
   021D                     900 00107$:
                            901 ;Initial/src/hpsdr_main.c:91: return fpga_load_begin ();
                            902 ;     genCall
                            903 ;     genRet
                            904 ;	Peephole 251.a	replaced ljmp to ret with ret
                            905 ;	Peephole 253.a	replaced lcall/ret with ljmp
   021D 02 04 6C            906 	ljmp	_fpga_load_begin
                            907 ;Initial/src/hpsdr_main.c:93: case FL_XFER:
   0220                     908 00108$:
                            909 ;Initial/src/hpsdr_main.c:94: get_ep0_data ();
                            910 ;     genCall
   0220 12 01 AA            911 	lcall	_get_ep0_data
                            912 ;Initial/src/hpsdr_main.c:95: return fpga_load_xfer (EP0BUF, EP0BCL);
                            913 ;     genAssign
   0223 90 E6 8B            914 	mov	dptr,#_EP0BCL
   0226 E0                  915 	movx	a,@dptr
   0227 F5 0F               916 	mov	_fpga_load_xfer_PARM_2,a
                            917 ;     genCall
                            918 ;	Peephole 182.a	used 16 bit load of DPTR
   0229 90 E7 40            919 	mov	dptr,#_EP0BUF
                            920 ;     genRet
                            921 ;	Peephole 251.a	replaced ljmp to ret with ret
                            922 ;	Peephole 253.a	replaced lcall/ret with ljmp
   022C 02 04 F7            923 	ljmp	_fpga_load_xfer
                            924 ;Initial/src/hpsdr_main.c:97: case FL_END:
   022F                     925 00109$:
                            926 ;Initial/src/hpsdr_main.c:98: return fpga_load_end ();
                            927 ;     genCall
                            928 ;     genRet
                            929 ;	Peephole 251.a	replaced ljmp to ret with ret
                            930 ;	Peephole 253.a	replaced lcall/ret with ljmp
   022F 02 05 07            931 	ljmp	_fpga_load_end
                            932 ;Initial/src/hpsdr_main.c:100: default:
   0232                     933 00110$:
                            934 ;Initial/src/hpsdr_main.c:101: return 0;
                            935 ;     genRet
   0232 75 82 00            936 	mov	dpl,#0x00
                            937 ;	Peephole 251.a	replaced ljmp to ret with ret
   0235 22                  938 	ret
                            939 ;Initial/src/hpsdr_main.c:105: case VRQ_I2C_WRITE:
   0236                     940 00112$:
                            941 ;Initial/src/hpsdr_main.c:107: get_ep0_data ();
                            942 ;     genCall
   0236 12 01 AA            943 	lcall	_get_ep0_data
                            944 ;Initial/src/hpsdr_main.c:108: if (!i2c_write (wValueL, EP0BUF, EP0BCL))
                            945 ;     genPointerGet
                            946 ;     genFarPointerGet
   0239 90 E6 BA            947 	mov	dptr,#(_SETUPDAT + 0x0002)
   023C E0                  948 	movx	a,@dptr
   023D FA                  949 	mov	r2,a
                            950 ;     genAddrOf
   023E 75 2B 40            951 	mov	_i2c_write_PARM_2,#_EP0BUF
   0241 75 2C E7            952 	mov	(_i2c_write_PARM_2 + 1),#(_EP0BUF >> 8)
                            953 ;     genAssign
   0244 90 E6 8B            954 	mov	dptr,#_EP0BCL
   0247 E0                  955 	movx	a,@dptr
   0248 F5 2D               956 	mov	_i2c_write_PARM_3,a
                            957 ;     genCall
   024A 8A 82               958 	mov	dpl,r2
   024C 12 07 92            959 	lcall	_i2c_write
   024F E5 82               960 	mov	a,dpl
                            961 ;     genIfx
                            962 ;     genIfxJump
   0251 60 03               963 	jz	00160$
   0253 02 02 D5            964 	ljmp	00130$
   0256                     965 00160$:
                            966 ;Initial/src/hpsdr_main.c:109: return 0;
                            967 ;     genRet
   0256 75 82 00            968 	mov	dpl,#0x00
                            969 ;	Peephole 251.a	replaced ljmp to ret with ret
   0259 22                  970 	ret
                            971 ;Initial/src/hpsdr_main.c:112: case VRQ_SPI_WRITE:
   025A                     972 00115$:
                            973 ;Initial/src/hpsdr_main.c:114: get_ep0_data ();
                            974 ;     genCall
   025A 12 01 AA            975 	lcall	_get_ep0_data
                            976 ;Initial/src/hpsdr_main.c:115: if (!spi_write (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, EP0BCL))
                            977 ;     genPointerGet
                            978 ;     genFarPointerGet
   025D 90 E6 BB            979 	mov	dptr,#(_SETUPDAT + 0x0003)
   0260 E0                  980 	movx	a,@dptr
   0261 FA                  981 	mov	r2,a
                            982 ;     genPointerGet
                            983 ;     genFarPointerGet
   0262 90 E6 BA            984 	mov	dptr,#(_SETUPDAT + 0x0002)
   0265 E0                  985 	movx	a,@dptr
   0266 F5 16               986 	mov	_spi_write_PARM_2,a
                            987 ;     genPointerGet
                            988 ;     genFarPointerGet
   0268 90 E6 BD            989 	mov	dptr,#(_SETUPDAT + 0x0005)
   026B E0                  990 	movx	a,@dptr
   026C F5 17               991 	mov	_spi_write_PARM_3,a
                            992 ;     genPointerGet
                            993 ;     genFarPointerGet
   026E 90 E6 BC            994 	mov	dptr,#(_SETUPDAT + 0x0004)
   0271 E0                  995 	movx	a,@dptr
   0272 F5 18               996 	mov	_spi_write_PARM_4,a
                            997 ;     genAddrOf
   0274 75 19 40            998 	mov	_spi_write_PARM_5,#_EP0BUF
   0277 75 1A E7            999 	mov	(_spi_write_PARM_5 + 1),#(_EP0BUF >> 8)
                           1000 ;     genAssign
   027A 90 E6 8B           1001 	mov	dptr,#_EP0BCL
   027D E0                 1002 	movx	a,@dptr
   027E F5 1B              1003 	mov	_spi_write_PARM_6,a
                           1004 ;     genCall
   0280 8A 82              1005 	mov	dpl,r2
   0282 12 0A 9A           1006 	lcall	_spi_write
   0285 E5 82              1007 	mov	a,dpl
                           1008 ;     genIfx
                           1009 ;     genIfxJump
                           1010 ;	Peephole 109	removed ljmp by inverse jump logic
                           1011 ;Initial/src/hpsdr_main.c:116: return 0;
                           1012 ;     genRet
                           1013 ;	Peephole 256.c	loading dpl with zero from a
   0287 70 4C              1014 	jnz	00130$
   0289                    1015 00161$:
   0289 F5 82              1016 	mov	dpl,a
                           1017 ;Initial/src/hpsdr_main.c:119: case VRQ_I2C_SPEED_SET:
                           1018 ;	Peephole 112.b	changed ljmp to sjmp
                           1019 ;	Peephole 251.b	replaced sjmp to ret with ret
   028B 22                 1020 	ret
   028C                    1021 00118$:
                           1022 ;Initial/src/hpsdr_main.c:120: if (wValueL == 1)
                           1023 ;     genPointerGet
                           1024 ;     genFarPointerGet
   028C 90 E6 BA           1025 	mov	dptr,#(_SETUPDAT + 0x0002)
   028F E0                 1026 	movx	a,@dptr
   0290 FA                 1027 	mov	r2,a
                           1028 ;     genCmpEq
                           1029 ;	Peephole 112.b	changed ljmp to sjmp
                           1030 ;	Peephole 199	optimized misc jump sequence
   0291 BA 01 0A           1031 	cjne	r2,#0x01,00120$
                           1032 ;00162$:
                           1033 ;	Peephole 200	removed redundant sjmp
   0294                    1034 00163$:
                           1035 ;Initial/src/hpsdr_main.c:121: I2CTL |= bm400KHZ;
                           1036 ;     genAssign
                           1037 ;     genOr
                           1038 ;	Peephole 248.a	optimized or to xdata
   0294 90 E6 7A           1039 	mov	dptr,#_I2CTL
   0297 E0                 1040 	movx	a,@dptr
   0298 FA                 1041 	mov	r2,a
   0299 44 01              1042 	orl	a,#0x01
   029B F0                 1043 	movx	@dptr,a
                           1044 ;	Peephole 112.b	changed ljmp to sjmp
   029C 80 37              1045 	sjmp	00130$
   029E                    1046 00120$:
                           1047 ;Initial/src/hpsdr_main.c:123: I2CTL &= ~bm400KHZ;
                           1048 ;     genAssign
                           1049 ;     genAnd
                           1050 ;	Peephole 248.b	optimized and to xdata
   029E 90 E6 7A           1051 	mov	dptr,#_I2CTL
   02A1 E0                 1052 	movx	a,@dptr
   02A2 FA                 1053 	mov	r2,a
   02A3 54 FE              1054 	anl	a,#0xFE
   02A5 F0                 1055 	movx	@dptr,a
                           1056 ;Initial/src/hpsdr_main.c:124: break;
                           1057 ;Initial/src/hpsdr_main.c:126: case VRQ_CPU_SPEED_SET:
                           1058 ;	Peephole 112.b	changed ljmp to sjmp
   02A6 80 2D              1059 	sjmp	00130$
   02A8                    1060 00122$:
                           1061 ;Initial/src/hpsdr_main.c:127: if (wValueL == 0)
                           1062 ;     genPointerGet
                           1063 ;     genFarPointerGet
   02A8 90 E6 BA           1064 	mov	dptr,#(_SETUPDAT + 0x0002)
   02AB E0                 1065 	movx	a,@dptr
                           1066 ;     genCmpEq
                           1067 ;	Peephole 115.b	jump optimization
   02AC FA                 1068 	mov	r2,a
   02AD 60 02              1069 	jz	00165$
   02AF                    1070 00164$:
                           1071 ;	Peephole 112.b	changed ljmp to sjmp
   02AF 80 08              1072 	sjmp	00127$
   02B1                    1073 00165$:
                           1074 ;Initial/src/hpsdr_main.c:128: CPUCS = bmCLKOE; // 12 MHz
                           1075 ;     genAssign
   02B1 90 E6 00           1076 	mov	dptr,#_CPUCS
   02B4 74 02              1077 	mov	a,#0x02
   02B6 F0                 1078 	movx	@dptr,a
                           1079 ;	Peephole 112.b	changed ljmp to sjmp
   02B7 80 1C              1080 	sjmp	00130$
   02B9                    1081 00127$:
                           1082 ;Initial/src/hpsdr_main.c:129: else if (wValueL == 1)
                           1083 ;     genPointerGet
                           1084 ;     genFarPointerGet
   02B9 90 E6 BA           1085 	mov	dptr,#(_SETUPDAT + 0x0002)
   02BC E0                 1086 	movx	a,@dptr
   02BD FA                 1087 	mov	r2,a
                           1088 ;     genCmpEq
                           1089 ;	Peephole 112.b	changed ljmp to sjmp
                           1090 ;	Peephole 199	optimized misc jump sequence
   02BE BA 01 08           1091 	cjne	r2,#0x01,00124$
                           1092 ;00166$:
                           1093 ;	Peephole 200	removed redundant sjmp
   02C1                    1094 00167$:
                           1095 ;Initial/src/hpsdr_main.c:130: CPUCS = bmCLKSPD0 | bmCLKOE; // 24 MHz
                           1096 ;     genAssign
   02C1 90 E6 00           1097 	mov	dptr,#_CPUCS
   02C4 74 0A              1098 	mov	a,#0x0A
   02C6 F0                 1099 	movx	@dptr,a
                           1100 ;	Peephole 112.b	changed ljmp to sjmp
   02C7 80 0C              1101 	sjmp	00130$
   02C9                    1102 00124$:
                           1103 ;Initial/src/hpsdr_main.c:132: CPUCS = bmCLKSPD1 | bmCLKOE; // 48 MHz
                           1104 ;     genAssign
   02C9 90 E6 00           1105 	mov	dptr,#_CPUCS
   02CC 74 12              1106 	mov	a,#0x12
   02CE F0                 1107 	movx	@dptr,a
                           1108 ;Initial/src/hpsdr_main.c:133: break;
                           1109 ;Initial/src/hpsdr_main.c:135: default:
                           1110 ;	Peephole 112.b	changed ljmp to sjmp
   02CF 80 04              1111 	sjmp	00130$
   02D1                    1112 00129$:
                           1113 ;Initial/src/hpsdr_main.c:136: return 0;
                           1114 ;     genRet
   02D1 75 82 00           1115 	mov	dpl,#0x00
                           1116 ;Initial/src/hpsdr_main.c:137: }
                           1117 ;	Peephole 112.b	changed ljmp to sjmp
                           1118 ;Initial/src/hpsdr_main.c:138: return 1;
                           1119 ;     genRet
                           1120 ;	Peephole 237.a	removed sjmp to ret
   02D4 22                 1121 	ret
   02D5                    1122 00130$:
   02D5 75 82 01           1123 	mov	dpl,#0x01
   02D8                    1124 00131$:
   02D8 22                 1125 	ret
                           1126 ;------------------------------------------------------------
                           1127 ;Allocation info for local variables in function 'app_vendor_IN_cmd'
                           1128 ;------------------------------------------------------------
                           1129 ;------------------------------------------------------------
                           1130 ;Initial/src/hpsdr_main.c:141: unsigned char app_vendor_IN_cmd(void)
                           1131 ;	-----------------------------------------
                           1132 ;	 function app_vendor_IN_cmd
                           1133 ;	-----------------------------------------
   02D9                    1134 _app_vendor_IN_cmd:
                           1135 ;Initial/src/hpsdr_main.c:143: switch (bRequest)
                           1136 ;     genPointerGet
                           1137 ;     genFarPointerGet
   02D9 90 E6 B9           1138 	mov	dptr,#(_SETUPDAT + 0x0001)
   02DC E0                 1139 	movx	a,@dptr
   02DD FA                 1140 	mov	r2,a
                           1141 ;     genCmpEq
   02DE BA 81 02           1142 	cjne	r2,#0x81,00119$
                           1143 ;	Peephole 112.b	changed ljmp to sjmp
   02E1 80 14              1144 	sjmp	00101$
   02E3                    1145 00119$:
                           1146 ;     genCmpEq
   02E3 BA 82 02           1147 	cjne	r2,#0x82,00120$
                           1148 ;	Peephole 112.b	changed ljmp to sjmp
   02E6 80 3D              1149 	sjmp	00104$
   02E8                    1150 00120$:
                           1151 ;     genCmpEq
   02E8 BA 84 03           1152 	cjne	r2,#0x84,00121$
   02EB 02 03 64           1153 	ljmp	00107$
   02EE                    1154 00121$:
                           1155 ;     genCmpEq
   02EE BA 85 03           1156 	cjne	r2,#0x85,00122$
   02F1 02 03 7E           1157 	ljmp	00108$
   02F4                    1158 00122$:
   02F4 02 03 98           1159 	ljmp	00109$
                           1160 ;Initial/src/hpsdr_main.c:145: case VRQ_I2C_READ:
   02F7                    1161 00101$:
                           1162 ;Initial/src/hpsdr_main.c:146: if (!i2c_read (wValueL, EP0BUF, wLengthL))
                           1163 ;     genPointerGet
                           1164 ;     genFarPointerGet
   02F7 90 E6 BA           1165 	mov	dptr,#(_SETUPDAT + 0x0002)
   02FA E0                 1166 	movx	a,@dptr
   02FB FA                 1167 	mov	r2,a
                           1168 ;     genAddrOf
   02FC 75 2B 40           1169 	mov	_i2c_read_PARM_2,#_EP0BUF
   02FF 75 2C E7           1170 	mov	(_i2c_read_PARM_2 + 1),#(_EP0BUF >> 8)
                           1171 ;     genPointerGet
                           1172 ;     genFarPointerGet
   0302 90 E6 BE           1173 	mov	dptr,#(_SETUPDAT + 0x0006)
   0305 E0                 1174 	movx	a,@dptr
   0306 F5 2D              1175 	mov	_i2c_read_PARM_3,a
                           1176 ;     genCall
   0308 8A 82              1177 	mov	dpl,r2
   030A 12 06 BB           1178 	lcall	_i2c_read
   030D E5 82              1179 	mov	a,dpl
                           1180 ;     genIfx
                           1181 ;     genIfxJump
                           1182 ;	Peephole 109	removed ljmp by inverse jump logic
                           1183 ;Initial/src/hpsdr_main.c:147: return 0;
                           1184 ;     genRet
                           1185 ;	Peephole 256.c	loading dpl with zero from a
   030F 70 03              1186 	jnz	00103$
   0311                    1187 00123$:
   0311 F5 82              1188 	mov	dpl,a
                           1189 ;	Peephole 251.a	replaced ljmp to ret with ret
   0313 22                 1190 	ret
   0314                    1191 00103$:
                           1192 ;Initial/src/hpsdr_main.c:148: EP0BCH = 0;
                           1193 ;     genAssign
   0314 90 E6 8A           1194 	mov	dptr,#_EP0BCH
                           1195 ;	Peephole 181	changed mov to clr
   0317 E4                 1196 	clr	a
   0318 F0                 1197 	movx	@dptr,a
                           1198 ;Initial/src/hpsdr_main.c:149: EP0BCL = wLengthL;
                           1199 ;     genPointerGet
                           1200 ;     genFarPointerGet
   0319 90 E6 BE           1201 	mov	dptr,#(_SETUPDAT + 0x0006)
   031C E0                 1202 	movx	a,@dptr
                           1203 ;     genAssign
                           1204 ;	Peephole 100	removed redundant mov
   031D FA                 1205 	mov	r2,a
   031E 90 E6 8B           1206 	mov	dptr,#_EP0BCL
   0321 F0                 1207 	movx	@dptr,a
                           1208 ;Initial/src/hpsdr_main.c:150: break;
   0322 02 03 9C           1209 	ljmp	00110$
                           1210 ;Initial/src/hpsdr_main.c:152: case VRQ_SPI_READ:
   0325                    1211 00104$:
                           1212 ;Initial/src/hpsdr_main.c:153: if (!spi_read (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, wLengthL))
                           1213 ;     genPointerGet
                           1214 ;     genFarPointerGet
   0325 90 E6 BB           1215 	mov	dptr,#(_SETUPDAT + 0x0003)
   0328 E0                 1216 	movx	a,@dptr
   0329 FA                 1217 	mov	r2,a
                           1218 ;     genPointerGet
                           1219 ;     genFarPointerGet
   032A 90 E6 BA           1220 	mov	dptr,#(_SETUPDAT + 0x0002)
   032D E0                 1221 	movx	a,@dptr
   032E F5 10              1222 	mov	_spi_read_PARM_2,a
                           1223 ;     genPointerGet
                           1224 ;     genFarPointerGet
   0330 90 E6 BD           1225 	mov	dptr,#(_SETUPDAT + 0x0005)
   0333 E0                 1226 	movx	a,@dptr
   0334 F5 11              1227 	mov	_spi_read_PARM_3,a
                           1228 ;     genPointerGet
                           1229 ;     genFarPointerGet
   0336 90 E6 BC           1230 	mov	dptr,#(_SETUPDAT + 0x0004)
   0339 E0                 1231 	movx	a,@dptr
   033A F5 12              1232 	mov	_spi_read_PARM_4,a
                           1233 ;     genAddrOf
   033C 75 13 40           1234 	mov	_spi_read_PARM_5,#_EP0BUF
   033F 75 14 E7           1235 	mov	(_spi_read_PARM_5 + 1),#(_EP0BUF >> 8)
                           1236 ;     genPointerGet
                           1237 ;     genFarPointerGet
   0342 90 E6 BE           1238 	mov	dptr,#(_SETUPDAT + 0x0006)
   0345 E0                 1239 	movx	a,@dptr
   0346 F5 15              1240 	mov	_spi_read_PARM_6,a
                           1241 ;     genCall
   0348 8A 82              1242 	mov	dpl,r2
   034A 12 0A 2C           1243 	lcall	_spi_read
   034D E5 82              1244 	mov	a,dpl
                           1245 ;     genIfx
                           1246 ;     genIfxJump
                           1247 ;	Peephole 109	removed ljmp by inverse jump logic
                           1248 ;Initial/src/hpsdr_main.c:154: return 0;
                           1249 ;     genRet
                           1250 ;	Peephole 256.c	loading dpl with zero from a
   034F 70 03              1251 	jnz	00106$
   0351                    1252 00124$:
   0351 F5 82              1253 	mov	dpl,a
                           1254 ;	Peephole 112.b	changed ljmp to sjmp
                           1255 ;	Peephole 251.b	replaced sjmp to ret with ret
   0353 22                 1256 	ret
   0354                    1257 00106$:
                           1258 ;Initial/src/hpsdr_main.c:155: EP0BCH = 0;
                           1259 ;     genAssign
   0354 90 E6 8A           1260 	mov	dptr,#_EP0BCH
                           1261 ;	Peephole 181	changed mov to clr
   0357 E4                 1262 	clr	a
   0358 F0                 1263 	movx	@dptr,a
                           1264 ;Initial/src/hpsdr_main.c:156: EP0BCL = wLengthL;
                           1265 ;     genPointerGet
                           1266 ;     genFarPointerGet
   0359 90 E6 BE           1267 	mov	dptr,#(_SETUPDAT + 0x0006)
   035C E0                 1268 	movx	a,@dptr
                           1269 ;     genAssign
                           1270 ;	Peephole 100	removed redundant mov
   035D FA                 1271 	mov	r2,a
   035E 90 E6 8B           1272 	mov	dptr,#_EP0BCL
   0361 F0                 1273 	movx	@dptr,a
                           1274 ;Initial/src/hpsdr_main.c:157: break;
                           1275 ;Initial/src/hpsdr_main.c:159: case VRQ_EEPROM_TYPE_READ:
                           1276 ;	Peephole 112.b	changed ljmp to sjmp
   0362 80 38              1277 	sjmp	00110$
   0364                    1278 00107$:
                           1279 ;Initial/src/hpsdr_main.c:160: EP0BUF[0] = I2CS & bmID; // 16 = 2 byte, 8 = 1 byte
                           1280 ;     genAssign
   0364 90 E6 78           1281 	mov	dptr,#_I2CS
   0367 E0                 1282 	movx	a,@dptr
   0368 FA                 1283 	mov	r2,a
                           1284 ;     genAnd
   0369 53 02 18           1285 	anl	ar2,#0x18
                           1286 ;     genPointerSet
                           1287 ;     genFarPointerSet
   036C 90 E7 40           1288 	mov	dptr,#_EP0BUF
   036F EA                 1289 	mov	a,r2
   0370 F0                 1290 	movx	@dptr,a
                           1291 ;Initial/src/hpsdr_main.c:161: EP0BCH = 0;
                           1292 ;     genAssign
   0371 90 E6 8A           1293 	mov	dptr,#_EP0BCH
                           1294 ;	Peephole 181	changed mov to clr
   0374 E4                 1295 	clr	a
   0375 F0                 1296 	movx	@dptr,a
                           1297 ;Initial/src/hpsdr_main.c:162: EP0BCL = 1;
                           1298 ;     genAssign
   0376 90 E6 8B           1299 	mov	dptr,#_EP0BCL
   0379 74 01              1300 	mov	a,#0x01
   037B F0                 1301 	movx	@dptr,a
                           1302 ;Initial/src/hpsdr_main.c:163: break;
                           1303 ;Initial/src/hpsdr_main.c:165: case VRQ_I2C_SPEED_READ:
                           1304 ;	Peephole 112.b	changed ljmp to sjmp
   037C 80 1E              1305 	sjmp	00110$
   037E                    1306 00108$:
                           1307 ;Initial/src/hpsdr_main.c:166: EP0BUF[0] = I2CTL & bm400KHZ; // 0 = 100 kHz, 1 = 400 kHz
                           1308 ;     genAssign
   037E 90 E6 7A           1309 	mov	dptr,#_I2CTL
   0381 E0                 1310 	movx	a,@dptr
   0382 FA                 1311 	mov	r2,a
                           1312 ;     genAnd
   0383 53 02 01           1313 	anl	ar2,#0x01
                           1314 ;     genPointerSet
                           1315 ;     genFarPointerSet
   0386 90 E7 40           1316 	mov	dptr,#_EP0BUF
   0389 EA                 1317 	mov	a,r2
   038A F0                 1318 	movx	@dptr,a
                           1319 ;Initial/src/hpsdr_main.c:167: EP0BCH = 0;
                           1320 ;     genAssign
   038B 90 E6 8A           1321 	mov	dptr,#_EP0BCH
                           1322 ;	Peephole 181	changed mov to clr
   038E E4                 1323 	clr	a
   038F F0                 1324 	movx	@dptr,a
                           1325 ;Initial/src/hpsdr_main.c:168: EP0BCL = 1;
                           1326 ;     genAssign
   0390 90 E6 8B           1327 	mov	dptr,#_EP0BCL
   0393 74 01              1328 	mov	a,#0x01
   0395 F0                 1329 	movx	@dptr,a
                           1330 ;Initial/src/hpsdr_main.c:169: break;
                           1331 ;Initial/src/hpsdr_main.c:171: default:
                           1332 ;	Peephole 112.b	changed ljmp to sjmp
   0396 80 04              1333 	sjmp	00110$
   0398                    1334 00109$:
                           1335 ;Initial/src/hpsdr_main.c:172: return 0;
                           1336 ;     genRet
   0398 75 82 00           1337 	mov	dpl,#0x00
                           1338 ;Initial/src/hpsdr_main.c:173: }
                           1339 ;	Peephole 112.b	changed ljmp to sjmp
                           1340 ;Initial/src/hpsdr_main.c:174: return 1;
                           1341 ;     genRet
                           1342 ;	Peephole 237.a	removed sjmp to ret
   039B 22                 1343 	ret
   039C                    1344 00110$:
   039C 75 82 01           1345 	mov	dpl,#0x01
   039F                    1346 00111$:
   039F 22                 1347 	ret
                           1348 ;------------------------------------------------------------
                           1349 ;Allocation info for local variables in function 'app_vendor_cmd'
                           1350 ;------------------------------------------------------------
                           1351 ;------------------------------------------------------------
                           1352 ;Initial/src/hpsdr_main.c:178: app_vendor_cmd (void)
                           1353 ;	-----------------------------------------
                           1354 ;	 function app_vendor_cmd
                           1355 ;	-----------------------------------------
   03A0                    1356 _app_vendor_cmd:
                           1357 ;Initial/src/hpsdr_main.c:180: if (bRequestType == VRT_VENDOR_IN)
                           1358 ;     genPointerGet
                           1359 ;     genFarPointerGet
   03A0 90 E6 B8           1360 	mov	dptr,#_SETUPDAT
   03A3 E0                 1361 	movx	a,@dptr
   03A4 FA                 1362 	mov	r2,a
                           1363 ;     genCmpEq
                           1364 ;	Peephole 112.b	changed ljmp to sjmp
                           1365 ;	Peephole 199	optimized misc jump sequence
   03A5 BA C0 03           1366 	cjne	r2,#0xC0,00105$
                           1367 ;00111$:
                           1368 ;	Peephole 200	removed redundant sjmp
   03A8                    1369 00112$:
                           1370 ;Initial/src/hpsdr_main.c:181: return app_vendor_IN_cmd();
                           1371 ;     genCall
                           1372 ;     genRet
                           1373 ;	Peephole 112.b	changed ljmp to sjmp
                           1374 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1375 ;	Peephole 253.a	replaced lcall/ret with ljmp
   03A8 02 02 D9           1376 	ljmp	_app_vendor_IN_cmd
   03AB                    1377 00105$:
                           1378 ;Initial/src/hpsdr_main.c:182: else if (bRequestType == VRT_VENDOR_OUT)
                           1379 ;     genPointerGet
                           1380 ;     genFarPointerGet
   03AB 90 E6 B8           1381 	mov	dptr,#_SETUPDAT
   03AE E0                 1382 	movx	a,@dptr
   03AF FA                 1383 	mov	r2,a
                           1384 ;     genCmpEq
                           1385 ;	Peephole 112.b	changed ljmp to sjmp
                           1386 ;	Peephole 199	optimized misc jump sequence
   03B0 BA 40 03           1387 	cjne	r2,#0x40,00102$
                           1388 ;00113$:
                           1389 ;	Peephole 200	removed redundant sjmp
   03B3                    1390 00114$:
                           1391 ;Initial/src/hpsdr_main.c:183: return app_vendor_OUT_cmd();
                           1392 ;     genCall
                           1393 ;     genRet
                           1394 ;	Peephole 112.b	changed ljmp to sjmp
                           1395 ;Initial/src/hpsdr_main.c:185: return 0;    // invalid bRequestType
                           1396 ;     genRet
                           1397 ;	Peephole 237.a	removed sjmp to ret
                           1398 ;	Peephole 253.a	replaced lcall/ret with ljmp
   03B3 02 01 B8           1399 	ljmp	_app_vendor_OUT_cmd
   03B6                    1400 00102$:
   03B6 75 82 00           1401 	mov	dpl,#0x00
   03B9                    1402 00107$:
   03B9 22                 1403 	ret
                           1404 ;------------------------------------------------------------
                           1405 ;Allocation info for local variables in function 'main_loop'
                           1406 ;------------------------------------------------------------
                           1407 ;------------------------------------------------------------
                           1408 ;Initial/src/hpsdr_main.c:189: main_loop (void)
                           1409 ;	-----------------------------------------
                           1410 ;	 function main_loop
                           1411 ;	-----------------------------------------
   03BA                    1412 _main_loop:
                           1413 ;Initial/src/hpsdr_main.c:192: while (1)
   03BA                    1414 00105$:
                           1415 ;Initial/src/hpsdr_main.c:194: if (usb_setup_packet_avail ())
                           1416 ;     genIfx
                           1417 ;     genIfxJump
                           1418 ;	Peephole 111	removed ljmp by inverse jump logic
   03BA 30 03 05           1419 	jnb	__usb_got_SUDAV,00102$
   03BD                    1420 00111$:
                           1421 ;Initial/src/hpsdr_main.c:195: usb_handle_setup_packet ();
                           1422 ;     genCall
   03BD 12 0F DD           1423 	lcall	_usb_handle_setup_packet
                           1424 ;	Peephole 112.b	changed ljmp to sjmp
   03C0 80 F8              1425 	sjmp	00105$
   03C2                    1426 00102$:
                           1427 ;Initial/src/hpsdr_main.c:197: putchar(32);
                           1428 ;     genCall
   03C2 75 82 20           1429 	mov	dpl,#0x20
   03C5 12 05 DF           1430 	lcall	_putchar
                           1431 ;	Peephole 112.b	changed ljmp to sjmp
   03C8 80 F0              1432 	sjmp	00105$
   03CA                    1433 00107$:
   03CA 22                 1434 	ret
                           1435 ;------------------------------------------------------------
                           1436 ;Allocation info for local variables in function 'isr_tick'
                           1437 ;------------------------------------------------------------
                           1438 ;count                     Allocated with name '_isr_tick_count_1_1'
                           1439 ;------------------------------------------------------------
                           1440 ;Initial/src/hpsdr_main.c:208: isr_tick (void) interrupt
                           1441 ;	-----------------------------------------
                           1442 ;	 function isr_tick
                           1443 ;	-----------------------------------------
   03CB                    1444 _isr_tick:
   03CB C0 E0              1445 	push	acc
                           1446 ;Initial/src/hpsdr_main.c:211: if (--count == 0)
                           1447 ;     genMinus
                           1448 ;     genMinusDec
   03CD 15 08              1449 	dec	_isr_tick_count_1_1
                           1450 ;     genCmpEq
   03CF E5 08              1451 	mov	a,_isr_tick_count_1_1
                           1452 ;	Peephole 162	removed sjmp by inverse jump logic
   03D1 60 02              1453 	jz	00107$
   03D3                    1454 00106$:
                           1455 ;	Peephole 112.b	changed ljmp to sjmp
   03D3 80 06              1456 	sjmp	00102$
   03D5                    1457 00107$:
                           1458 ;Initial/src/hpsdr_main.c:213: count = 50;
                           1459 ;     genAssign
   03D5 75 08 32           1460 	mov	_isr_tick_count_1_1,#0x32
                           1461 ;Initial/src/hpsdr_main.c:214: HPSDR_LED_REG ^= bmLED0;
                           1462 ;     genXor
   03D8 63 A0 40           1463 	xrl	_IOC,#0x40
   03DB                    1464 00102$:
                           1465 ;Initial/src/hpsdr_main.c:216: clear_timer_irq ();
                           1466 ;     genAssign
   03DB C2 CF              1467 	clr	_TF2
   03DD                    1468 00103$:
   03DD D0 E0              1469 	pop	acc
   03DF 32                 1470 	reti
                           1471 ;	eliminated unneeded push/pop psw
                           1472 ;	eliminated unneeded push/pop dpl
                           1473 ;	eliminated unneeded push/pop dph
                           1474 ;	eliminated unneeded push/pop b
                           1475 ;------------------------------------------------------------
                           1476 ;Allocation info for local variables in function 'patch_usb_descriptors'
                           1477 ;------------------------------------------------------------
                           1478 ;hw_rev                    Allocated with name '_patch_usb_descriptors_hw_rev_1_1'
                           1479 ;serial_no                 Allocated with name '_patch_usb_descriptors_serial_no_1_1'
                           1480 ;------------------------------------------------------------
                           1481 ;Initial/src/hpsdr_main.c:224: patch_usb_descriptors(void)
                           1482 ;	-----------------------------------------
                           1483 ;	 function patch_usb_descriptors
                           1484 ;	-----------------------------------------
   03E0                    1485 _patch_usb_descriptors:
                           1486 ;Initial/src/hpsdr_main.c:229: eeprom_read(I2C_ADDR_BOOT, HW_REV_OFFSET, &hw_rev, 1);	// LSB of device id
                           1487 ;     genAddrOf
   03E0 75 0A 00           1488 	mov	_eeprom_read_PARM_3,#_patch_usb_descriptors_hw_rev_1_1
   03E3 75 0B 20           1489 	mov	(_eeprom_read_PARM_3 + 1),#(_patch_usb_descriptors_hw_rev_1_1 >> 8)
                           1490 ;     genAssign
   03E6 75 09 05           1491 	mov	_eeprom_read_PARM_2,#0x05
                           1492 ;     genAssign
   03E9 75 0C 01           1493 	mov	_eeprom_read_PARM_4,#0x01
                           1494 ;     genCall
   03EC 75 82 51           1495 	mov	dpl,#0x51
   03EF 12 04 38           1496 	lcall	_eeprom_read
                           1497 ;Initial/src/hpsdr_main.c:231: usb_desc_hw_rev_binary_patch_location_0[0] = hw_rev;
                           1498 ;     genAssign
   03F2 90 20 00           1499 	mov	dptr,#_patch_usb_descriptors_hw_rev_1_1
   03F5 E0                 1500 	movx	a,@dptr
                           1501 ;     genPointerSet
                           1502 ;     genFarPointerSet
                           1503 ;	Peephole 100	removed redundant mov
   03F6 FA                 1504 	mov	r2,a
   03F7 90 E0 0C           1505 	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_0
   03FA F0                 1506 	movx	@dptr,a
                           1507 ;Initial/src/hpsdr_main.c:232: usb_desc_hw_rev_binary_patch_location_1[0] = hw_rev;
                           1508 ;     genPointerSet
                           1509 ;     genFarPointerSet
   03FB 90 E0 48           1510 	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_1
   03FE EA                 1511 	mov	a,r2
   03FF F0                 1512 	movx	@dptr,a
                           1513 ;Initial/src/hpsdr_main.c:233: usb_desc_hw_rev_ascii_patch_location_0[0] = hw_rev + '0';     // FIXME if we get > 9
                           1514 ;     genPlus
   0400 74 30              1515 	mov	a,#0x30
                           1516 ;	Peephole 236.a	used r2 instead of ar2
   0402 2A                 1517 	add	a,r2
                           1518 ;     genPointerSet
                           1519 ;     genFarPointerSet
                           1520 ;	Peephole 100	removed redundant mov
   0403 FA                 1521 	mov	r2,a
   0404 90 E0 C2           1522 	mov	dptr,#_usb_desc_hw_rev_ascii_patch_location_0
   0407 F0                 1523 	movx	@dptr,a
   0408                    1524 00101$:
   0408 22                 1525 	ret
                           1526 ;------------------------------------------------------------
                           1527 ;Allocation info for local variables in function 'main'
                           1528 ;------------------------------------------------------------
                           1529 ;------------------------------------------------------------
                           1530 ;Initial/src/hpsdr_main.c:244: main (void)
                           1531 ;	-----------------------------------------
                           1532 ;	 function main
                           1533 ;	-----------------------------------------
   0409                    1534 _main:
                           1535 ;Initial/src/hpsdr_main.c:246: init_hpsdr();
                           1536 ;     genCall
   0409 12 05 23           1537 	lcall	_init_hpsdr
                           1538 ;Initial/src/hpsdr_main.c:248: set_led_0 (1);
                           1539 ;     genCall
   040C 75 82 01           1540 	mov	dpl,#0x01
   040F 12 06 24           1541 	lcall	_set_led_0
                           1542 ;Initial/src/hpsdr_main.c:249: set_led_1 (0);
                           1543 ;     genCall
   0412 75 82 00           1544 	mov	dpl,#0x00
   0415 12 06 31           1545 	lcall	_set_led_1
                           1546 ;Initial/src/hpsdr_main.c:251: EA = 0;		// disable all interrupts
                           1547 ;     genAssign
   0418 C2 AF              1548 	clr	_EA
                           1549 ;Initial/src/hpsdr_main.c:253: patch_usb_descriptors();
                           1550 ;     genCall
   041A 12 03 E0           1551 	lcall	_patch_usb_descriptors
                           1552 ;Initial/src/hpsdr_main.c:255: setup_autovectors ();
                           1553 ;     genCall
   041D 12 09 3E           1554 	lcall	_setup_autovectors
                           1555 ;Initial/src/hpsdr_main.c:256: usb_install_handlers ();
                           1556 ;     genCall
   0420 12 0C AF           1557 	lcall	_usb_install_handlers
                           1558 ;Initial/src/hpsdr_main.c:257: hook_timer_tick ((unsigned short) isr_tick);
                           1559 ;     genCast
   0423 7A CB              1560 	mov	r2,#_isr_tick
   0425 7B 03              1561 	mov	r3,#(_isr_tick >> 8)
                           1562 ;     genCall
   0427 8A 82              1563 	mov	dpl,r2
   0429 8B 83              1564 	mov	dph,r3
   042B 12 0B CE           1565 	lcall	_hook_timer_tick
                           1566 ;Initial/src/hpsdr_main.c:259: EIEX4 = 1;	// disable INT4 FIXME
                           1567 ;     genAssign
   042E D2 EA              1568 	setb	_EIEX4
                           1569 ;Initial/src/hpsdr_main.c:260: EA = 1;			// global interrupt enable
                           1570 ;     genAssign
   0430 D2 AF              1571 	setb	_EA
                           1572 ;Initial/src/hpsdr_main.c:262: fx2_renumerate ();	// simulates disconnect / reconnect
                           1573 ;     genCall
   0432 12 06 95           1574 	lcall	_fx2_renumerate
                           1575 ;Initial/src/hpsdr_main.c:264: main_loop ();
                           1576 ;     genCall
                           1577 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0435 02 03 BA           1578 	ljmp	_main_loop
                           1579 	.area CSEG    (CODE)
