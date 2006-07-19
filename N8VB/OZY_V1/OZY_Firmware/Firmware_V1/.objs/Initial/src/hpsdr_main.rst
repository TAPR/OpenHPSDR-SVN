                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Wed Jul 19 12:32:14 2006
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
   01A2 02 0F 27            705 	ljmp	__sdcc_gsinit_startup
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
                            722 ;Initial/src/hpsdr_main.c:180: static unsigned char	count = 1;
                            723 ;     genAssign
   0F1E 75 08 01            724 	mov	_isr_tick_count_1_1,#0x01
                            725 	.area GSFINAL (CODE)
   0F3A 02 01 A5            726 	ljmp	__sdcc_program_startup
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
   01A5 12 03 68            737 	lcall	_main
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
   01BD BA 01 02            793 	cjne	r2,#0x01,00133$
                            794 ;	Peephole 112.b	changed ljmp to sjmp
   01C0 80 14               795 	sjmp	00101$
   01C2                     796 00133$:
                            797 ;     genCmpEq
   01C2 BA 02 02            798 	cjne	r2,#0x02,00134$
                            799 ;	Peephole 112.b	changed ljmp to sjmp
   01C5 80 39               800 	sjmp	00106$
   01C7                     801 00134$:
                            802 ;     genCmpEq
   01C7 BA 08 03            803 	cjne	r2,#0x08,00135$
   01CA 02 02 2A            804 	ljmp	00112$
   01CD                     805 00135$:
                            806 ;     genCmpEq
   01CD BA 09 03            807 	cjne	r2,#0x09,00136$
   01D0 02 02 4A            808 	ljmp	00115$
   01D3                     809 00136$:
   01D3 02 02 7C            810 	ljmp	00118$
                            811 ;Initial/src/hpsdr_main.c:69: case VRQ_SET_LED:
   01D6                     812 00101$:
                            813 ;Initial/src/hpsdr_main.c:71: switch (wIndexL)
                            814 ;     genPointerGet
                            815 ;     genFarPointerGet
   01D6 90 E6 BC            816 	mov	dptr,#(_SETUPDAT + 0x0004)
   01D9 E0                  817 	movx	a,@dptr
                            818 ;     genCmpEq
                            819 ;	Peephole 112.b	changed ljmp to sjmp
                            820 ;	Peephole 115.b	jump optimization
   01DA FA                  821 	mov	r2,a
   01DB 60 05               822 	jz	00102$
   01DD                     823 00137$:
                            824 ;     genCmpEq
                            825 ;	Peephole 112.b	changed ljmp to sjmp
                            826 ;Initial/src/hpsdr_main.c:73: case 0:
                            827 ;	Peephole 112.b	changed ljmp to sjmp
                            828 ;	Peephole 199	optimized misc jump sequence
   01DD BA 01 1C            829 	cjne	r2,#0x01,00104$
   01E0 80 0D               830 	sjmp	00103$
                            831 ;00138$:
   01E2                     832 00102$:
                            833 ;Initial/src/hpsdr_main.c:74: set_led_0 (wValueL);
                            834 ;     genPointerGet
                            835 ;     genFarPointerGet
   01E2 90 E6 BA            836 	mov	dptr,#(_SETUPDAT + 0x0002)
   01E5 E0                  837 	movx	a,@dptr
                            838 ;     genCall
                            839 ;	Peephole 244.c	loading dpl from a instead of r2
   01E6 FA                  840 	mov	r2,a
   01E7 F5 82               841 	mov	dpl,a
   01E9 12 05 18            842 	lcall	_set_led_0
                            843 ;Initial/src/hpsdr_main.c:75: break;
   01EC 02 02 80            844 	ljmp	00119$
                            845 ;Initial/src/hpsdr_main.c:77: case 1:
   01EF                     846 00103$:
                            847 ;Initial/src/hpsdr_main.c:78: set_led_1 (wValueL);
                            848 ;     genPointerGet
                            849 ;     genFarPointerGet
   01EF 90 E6 BA            850 	mov	dptr,#(_SETUPDAT + 0x0002)
   01F2 E0                  851 	movx	a,@dptr
                            852 ;     genCall
                            853 ;	Peephole 244.c	loading dpl from a instead of r2
   01F3 FA                  854 	mov	r2,a
   01F4 F5 82               855 	mov	dpl,a
   01F6 12 05 25            856 	lcall	_set_led_1
                            857 ;Initial/src/hpsdr_main.c:79: break;
   01F9 02 02 80            858 	ljmp	00119$
                            859 ;Initial/src/hpsdr_main.c:81: default:
   01FC                     860 00104$:
                            861 ;Initial/src/hpsdr_main.c:82: return 0;
                            862 ;     genRet
   01FC 75 82 00            863 	mov	dpl,#0x00
                            864 ;	Peephole 251.a	replaced ljmp to ret with ret
   01FF 22                  865 	ret
                            866 ;Initial/src/hpsdr_main.c:86: case VRQ_FPGA_LOAD:
   0200                     867 00106$:
                            868 ;Initial/src/hpsdr_main.c:88: switch (wIndexL)			// sub-command
                            869 ;     genPointerGet
                            870 ;     genFarPointerGet
   0200 90 E6 BC            871 	mov	dptr,#(_SETUPDAT + 0x0004)
   0203 E0                  872 	movx	a,@dptr
                            873 ;     genCmpEq
                            874 ;	Peephole 112.b	changed ljmp to sjmp
                            875 ;	Peephole 115.b	jump optimization
   0204 FA                  876 	mov	r2,a
   0205 60 0A               877 	jz	00107$
   0207                     878 00139$:
                            879 ;     genCmpEq
   0207 BA 01 02            880 	cjne	r2,#0x01,00140$
                            881 ;	Peephole 112.b	changed ljmp to sjmp
   020A 80 08               882 	sjmp	00108$
   020C                     883 00140$:
                            884 ;     genCmpEq
                            885 ;	Peephole 112.b	changed ljmp to sjmp
                            886 ;Initial/src/hpsdr_main.c:90: case FL_BEGIN:
                            887 ;	Peephole 112.b	changed ljmp to sjmp
                            888 ;	Peephole 199	optimized misc jump sequence
   020C BA 02 17            889 	cjne	r2,#0x02,00110$
   020F 80 12               890 	sjmp	00109$
                            891 ;00141$:
   0211                     892 00107$:
                            893 ;Initial/src/hpsdr_main.c:91: return fpga_load_begin ();
                            894 ;     genCall
                            895 ;     genRet
                            896 ;	Peephole 251.a	replaced ljmp to ret with ret
                            897 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0211 02 03 CB            898 	ljmp	_fpga_load_begin
                            899 ;Initial/src/hpsdr_main.c:93: case FL_XFER:
   0214                     900 00108$:
                            901 ;Initial/src/hpsdr_main.c:94: get_ep0_data ();
                            902 ;     genCall
   0214 12 01 AA            903 	lcall	_get_ep0_data
                            904 ;Initial/src/hpsdr_main.c:95: return fpga_load_xfer (EP0BUF, EP0BCL);
                            905 ;     genAssign
   0217 90 E6 8B            906 	mov	dptr,#_EP0BCL
   021A E0                  907 	movx	a,@dptr
   021B F5 0F               908 	mov	_fpga_load_xfer_PARM_2,a
                            909 ;     genCall
                            910 ;	Peephole 182.a	used 16 bit load of DPTR
   021D 90 E7 40            911 	mov	dptr,#_EP0BUF
                            912 ;     genRet
                            913 ;Initial/src/hpsdr_main.c:97: case FL_END:
                            914 ;	Peephole 112.b	changed ljmp to sjmp
                            915 ;	Peephole 251.b	replaced sjmp to ret with ret
                            916 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0220 02 04 56            917 	ljmp	_fpga_load_xfer
   0223                     918 00109$:
                            919 ;Initial/src/hpsdr_main.c:98: return fpga_load_end ();
                            920 ;     genCall
                            921 ;     genRet
                            922 ;Initial/src/hpsdr_main.c:100: default:
                            923 ;	Peephole 112.b	changed ljmp to sjmp
                            924 ;	Peephole 251.b	replaced sjmp to ret with ret
                            925 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0223 02 04 66            926 	ljmp	_fpga_load_end
   0226                     927 00110$:
                            928 ;Initial/src/hpsdr_main.c:101: return 0;
                            929 ;     genRet
   0226 75 82 00            930 	mov	dpl,#0x00
                            931 ;Initial/src/hpsdr_main.c:105: case VRQ_I2C_WRITE:
                            932 ;	Peephole 112.b	changed ljmp to sjmp
                            933 ;	Peephole 251.b	replaced sjmp to ret with ret
   0229 22                  934 	ret
   022A                     935 00112$:
                            936 ;Initial/src/hpsdr_main.c:107: get_ep0_data ();
                            937 ;     genCall
   022A 12 01 AA            938 	lcall	_get_ep0_data
                            939 ;Initial/src/hpsdr_main.c:108: if (!i2c_write (wValueL, EP0BUF, EP0BCL))
                            940 ;     genPointerGet
                            941 ;     genFarPointerGet
   022D 90 E6 BA            942 	mov	dptr,#(_SETUPDAT + 0x0002)
   0230 E0                  943 	movx	a,@dptr
   0231 FA                  944 	mov	r2,a
                            945 ;     genAddrOf
   0232 75 2B 40            946 	mov	_i2c_write_PARM_2,#_EP0BUF
   0235 75 2C E7            947 	mov	(_i2c_write_PARM_2 + 1),#(_EP0BUF >> 8)
                            948 ;     genAssign
   0238 90 E6 8B            949 	mov	dptr,#_EP0BCL
   023B E0                  950 	movx	a,@dptr
   023C F5 2D               951 	mov	_i2c_write_PARM_3,a
                            952 ;     genCall
   023E 8A 82               953 	mov	dpl,r2
   0240 12 06 86            954 	lcall	_i2c_write
   0243 E5 82               955 	mov	a,dpl
                            956 ;     genIfx
                            957 ;     genIfxJump
                            958 ;	Peephole 109	removed ljmp by inverse jump logic
                            959 ;Initial/src/hpsdr_main.c:109: return 0;
                            960 ;     genRet
                            961 ;	Peephole 256.c	loading dpl with zero from a
   0245 70 39               962 	jnz	00119$
   0247                     963 00142$:
   0247 F5 82               964 	mov	dpl,a
                            965 ;Initial/src/hpsdr_main.c:112: case VRQ_SPI_WRITE:
                            966 ;	Peephole 112.b	changed ljmp to sjmp
                            967 ;	Peephole 251.b	replaced sjmp to ret with ret
   0249 22                  968 	ret
   024A                     969 00115$:
                            970 ;Initial/src/hpsdr_main.c:114: get_ep0_data ();
                            971 ;     genCall
   024A 12 01 AA            972 	lcall	_get_ep0_data
                            973 ;Initial/src/hpsdr_main.c:115: if (!spi_write (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, EP0BCL))
                            974 ;     genPointerGet
                            975 ;     genFarPointerGet
   024D 90 E6 BB            976 	mov	dptr,#(_SETUPDAT + 0x0003)
   0250 E0                  977 	movx	a,@dptr
   0251 FA                  978 	mov	r2,a
                            979 ;     genPointerGet
                            980 ;     genFarPointerGet
   0252 90 E6 BA            981 	mov	dptr,#(_SETUPDAT + 0x0002)
   0255 E0                  982 	movx	a,@dptr
   0256 F5 16               983 	mov	_spi_write_PARM_2,a
                            984 ;     genPointerGet
                            985 ;     genFarPointerGet
   0258 90 E6 BD            986 	mov	dptr,#(_SETUPDAT + 0x0005)
   025B E0                  987 	movx	a,@dptr
   025C F5 17               988 	mov	_spi_write_PARM_3,a
                            989 ;     genPointerGet
                            990 ;     genFarPointerGet
   025E 90 E6 BC            991 	mov	dptr,#(_SETUPDAT + 0x0004)
   0261 E0                  992 	movx	a,@dptr
   0262 F5 18               993 	mov	_spi_write_PARM_4,a
                            994 ;     genAddrOf
   0264 75 19 40            995 	mov	_spi_write_PARM_5,#_EP0BUF
   0267 75 1A E7            996 	mov	(_spi_write_PARM_5 + 1),#(_EP0BUF >> 8)
                            997 ;     genAssign
   026A 90 E6 8B            998 	mov	dptr,#_EP0BCL
   026D E0                  999 	movx	a,@dptr
   026E F5 1B              1000 	mov	_spi_write_PARM_6,a
                           1001 ;     genCall
   0270 8A 82              1002 	mov	dpl,r2
   0272 12 09 9D           1003 	lcall	_spi_write
   0275 E5 82              1004 	mov	a,dpl
                           1005 ;     genIfx
                           1006 ;     genIfxJump
                           1007 ;	Peephole 109	removed ljmp by inverse jump logic
                           1008 ;Initial/src/hpsdr_main.c:116: return 0;
                           1009 ;     genRet
                           1010 ;	Peephole 256.c	loading dpl with zero from a
   0277 70 07              1011 	jnz	00119$
   0279                    1012 00143$:
   0279 F5 82              1013 	mov	dpl,a
                           1014 ;Initial/src/hpsdr_main.c:119: default:
                           1015 ;	Peephole 112.b	changed ljmp to sjmp
                           1016 ;	Peephole 251.b	replaced sjmp to ret with ret
   027B 22                 1017 	ret
   027C                    1018 00118$:
                           1019 ;Initial/src/hpsdr_main.c:120: return 0;
                           1020 ;     genRet
   027C 75 82 00           1021 	mov	dpl,#0x00
                           1022 ;Initial/src/hpsdr_main.c:121: }
                           1023 ;	Peephole 112.b	changed ljmp to sjmp
                           1024 ;Initial/src/hpsdr_main.c:122: return 1;
                           1025 ;     genRet
                           1026 ;	Peephole 237.a	removed sjmp to ret
   027F 22                 1027 	ret
   0280                    1028 00119$:
   0280 75 82 01           1029 	mov	dpl,#0x01
   0283                    1030 00120$:
   0283 22                 1031 	ret
                           1032 ;------------------------------------------------------------
                           1033 ;Allocation info for local variables in function 'app_vendor_IN_cmd'
                           1034 ;------------------------------------------------------------
                           1035 ;------------------------------------------------------------
                           1036 ;Initial/src/hpsdr_main.c:125: unsigned char app_vendor_IN_cmd(void)
                           1037 ;	-----------------------------------------
                           1038 ;	 function app_vendor_IN_cmd
                           1039 ;	-----------------------------------------
   0284                    1040 _app_vendor_IN_cmd:
                           1041 ;Initial/src/hpsdr_main.c:127: switch (bRequest)
                           1042 ;     genPointerGet
                           1043 ;     genFarPointerGet
   0284 90 E6 B9           1044 	mov	dptr,#(_SETUPDAT + 0x0001)
   0287 E0                 1045 	movx	a,@dptr
   0288 FA                 1046 	mov	r2,a
                           1047 ;     genCmpEq
   0289 BA 81 02           1048 	cjne	r2,#0x81,00115$
                           1049 ;	Peephole 112.b	changed ljmp to sjmp
   028C 80 05              1050 	sjmp	00101$
   028E                    1051 00115$:
                           1052 ;     genCmpEq
                           1053 ;	Peephole 112.b	changed ljmp to sjmp
                           1054 ;Initial/src/hpsdr_main.c:129: case VRQ_I2C_READ:
                           1055 ;	Peephole 112.b	changed ljmp to sjmp
                           1056 ;	Peephole 199	optimized misc jump sequence
   028E BA 82 6E           1057 	cjne	r2,#0x82,00107$
   0291 80 2D              1058 	sjmp	00104$
                           1059 ;00116$:
   0293                    1060 00101$:
                           1061 ;Initial/src/hpsdr_main.c:130: if (!i2c_read (wValueL, EP0BUF, wLengthL))
                           1062 ;     genPointerGet
                           1063 ;     genFarPointerGet
   0293 90 E6 BA           1064 	mov	dptr,#(_SETUPDAT + 0x0002)
   0296 E0                 1065 	movx	a,@dptr
   0297 FA                 1066 	mov	r2,a
                           1067 ;     genAddrOf
   0298 75 2B 40           1068 	mov	_i2c_read_PARM_2,#_EP0BUF
   029B 75 2C E7           1069 	mov	(_i2c_read_PARM_2 + 1),#(_EP0BUF >> 8)
                           1070 ;     genPointerGet
                           1071 ;     genFarPointerGet
   029E 90 E6 BE           1072 	mov	dptr,#(_SETUPDAT + 0x0006)
   02A1 E0                 1073 	movx	a,@dptr
   02A2 F5 2D              1074 	mov	_i2c_read_PARM_3,a
                           1075 ;     genCall
   02A4 8A 82              1076 	mov	dpl,r2
   02A6 12 05 AF           1077 	lcall	_i2c_read
   02A9 E5 82              1078 	mov	a,dpl
                           1079 ;     genIfx
                           1080 ;     genIfxJump
                           1081 ;	Peephole 109	removed ljmp by inverse jump logic
                           1082 ;Initial/src/hpsdr_main.c:131: return 0;
                           1083 ;     genRet
                           1084 ;	Peephole 256.c	loading dpl with zero from a
   02AB 70 03              1085 	jnz	00103$
   02AD                    1086 00117$:
   02AD F5 82              1087 	mov	dpl,a
                           1088 ;	Peephole 112.b	changed ljmp to sjmp
                           1089 ;	Peephole 251.b	replaced sjmp to ret with ret
   02AF 22                 1090 	ret
   02B0                    1091 00103$:
                           1092 ;Initial/src/hpsdr_main.c:132: EP0BCH = 0;
                           1093 ;     genAssign
   02B0 90 E6 8A           1094 	mov	dptr,#_EP0BCH
                           1095 ;	Peephole 181	changed mov to clr
   02B3 E4                 1096 	clr	a
   02B4 F0                 1097 	movx	@dptr,a
                           1098 ;Initial/src/hpsdr_main.c:133: EP0BCL = wLengthL;
                           1099 ;     genPointerGet
                           1100 ;     genFarPointerGet
   02B5 90 E6 BE           1101 	mov	dptr,#(_SETUPDAT + 0x0006)
   02B8 E0                 1102 	movx	a,@dptr
                           1103 ;     genAssign
                           1104 ;	Peephole 100	removed redundant mov
   02B9 FA                 1105 	mov	r2,a
   02BA 90 E6 8B           1106 	mov	dptr,#_EP0BCL
   02BD F0                 1107 	movx	@dptr,a
                           1108 ;Initial/src/hpsdr_main.c:134: break;
                           1109 ;Initial/src/hpsdr_main.c:136: case VRQ_SPI_READ:
                           1110 ;	Peephole 112.b	changed ljmp to sjmp
   02BE 80 43              1111 	sjmp	00108$
   02C0                    1112 00104$:
                           1113 ;Initial/src/hpsdr_main.c:137: if (!spi_read (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, wLengthL))
                           1114 ;     genPointerGet
                           1115 ;     genFarPointerGet
   02C0 90 E6 BB           1116 	mov	dptr,#(_SETUPDAT + 0x0003)
   02C3 E0                 1117 	movx	a,@dptr
   02C4 FA                 1118 	mov	r2,a
                           1119 ;     genPointerGet
                           1120 ;     genFarPointerGet
   02C5 90 E6 BA           1121 	mov	dptr,#(_SETUPDAT + 0x0002)
   02C8 E0                 1122 	movx	a,@dptr
   02C9 F5 10              1123 	mov	_spi_read_PARM_2,a
                           1124 ;     genPointerGet
                           1125 ;     genFarPointerGet
   02CB 90 E6 BD           1126 	mov	dptr,#(_SETUPDAT + 0x0005)
   02CE E0                 1127 	movx	a,@dptr
   02CF F5 11              1128 	mov	_spi_read_PARM_3,a
                           1129 ;     genPointerGet
                           1130 ;     genFarPointerGet
   02D1 90 E6 BC           1131 	mov	dptr,#(_SETUPDAT + 0x0004)
   02D4 E0                 1132 	movx	a,@dptr
   02D5 F5 12              1133 	mov	_spi_read_PARM_4,a
                           1134 ;     genAddrOf
   02D7 75 13 40           1135 	mov	_spi_read_PARM_5,#_EP0BUF
   02DA 75 14 E7           1136 	mov	(_spi_read_PARM_5 + 1),#(_EP0BUF >> 8)
                           1137 ;     genPointerGet
                           1138 ;     genFarPointerGet
   02DD 90 E6 BE           1139 	mov	dptr,#(_SETUPDAT + 0x0006)
   02E0 E0                 1140 	movx	a,@dptr
   02E1 F5 15              1141 	mov	_spi_read_PARM_6,a
                           1142 ;     genCall
   02E3 8A 82              1143 	mov	dpl,r2
   02E5 12 09 2F           1144 	lcall	_spi_read
   02E8 E5 82              1145 	mov	a,dpl
                           1146 ;     genIfx
                           1147 ;     genIfxJump
                           1148 ;	Peephole 109	removed ljmp by inverse jump logic
                           1149 ;Initial/src/hpsdr_main.c:138: return 0;
                           1150 ;     genRet
                           1151 ;	Peephole 256.c	loading dpl with zero from a
   02EA 70 03              1152 	jnz	00106$
   02EC                    1153 00118$:
   02EC F5 82              1154 	mov	dpl,a
                           1155 ;	Peephole 112.b	changed ljmp to sjmp
                           1156 ;	Peephole 251.b	replaced sjmp to ret with ret
   02EE 22                 1157 	ret
   02EF                    1158 00106$:
                           1159 ;Initial/src/hpsdr_main.c:139: EP0BCH = 0;
                           1160 ;     genAssign
   02EF 90 E6 8A           1161 	mov	dptr,#_EP0BCH
                           1162 ;	Peephole 181	changed mov to clr
   02F2 E4                 1163 	clr	a
   02F3 F0                 1164 	movx	@dptr,a
                           1165 ;Initial/src/hpsdr_main.c:140: EP0BCL = wLengthL;
                           1166 ;     genPointerGet
                           1167 ;     genFarPointerGet
   02F4 90 E6 BE           1168 	mov	dptr,#(_SETUPDAT + 0x0006)
   02F7 E0                 1169 	movx	a,@dptr
                           1170 ;     genAssign
                           1171 ;	Peephole 100	removed redundant mov
   02F8 FA                 1172 	mov	r2,a
   02F9 90 E6 8B           1173 	mov	dptr,#_EP0BCL
   02FC F0                 1174 	movx	@dptr,a
                           1175 ;Initial/src/hpsdr_main.c:141: break;
                           1176 ;Initial/src/hpsdr_main.c:143: default:
                           1177 ;	Peephole 112.b	changed ljmp to sjmp
   02FD 80 04              1178 	sjmp	00108$
   02FF                    1179 00107$:
                           1180 ;Initial/src/hpsdr_main.c:144: return 0;
                           1181 ;     genRet
   02FF 75 82 00           1182 	mov	dpl,#0x00
                           1183 ;Initial/src/hpsdr_main.c:145: }
                           1184 ;	Peephole 112.b	changed ljmp to sjmp
                           1185 ;Initial/src/hpsdr_main.c:146: return 1;
                           1186 ;     genRet
                           1187 ;	Peephole 237.a	removed sjmp to ret
   0302 22                 1188 	ret
   0303                    1189 00108$:
   0303 75 82 01           1190 	mov	dpl,#0x01
   0306                    1191 00109$:
   0306 22                 1192 	ret
                           1193 ;------------------------------------------------------------
                           1194 ;Allocation info for local variables in function 'app_vendor_cmd'
                           1195 ;------------------------------------------------------------
                           1196 ;------------------------------------------------------------
                           1197 ;Initial/src/hpsdr_main.c:150: app_vendor_cmd (void)
                           1198 ;	-----------------------------------------
                           1199 ;	 function app_vendor_cmd
                           1200 ;	-----------------------------------------
   0307                    1201 _app_vendor_cmd:
                           1202 ;Initial/src/hpsdr_main.c:152: if (bRequestType == VRT_VENDOR_IN)
                           1203 ;     genPointerGet
                           1204 ;     genFarPointerGet
   0307 90 E6 B8           1205 	mov	dptr,#_SETUPDAT
   030A E0                 1206 	movx	a,@dptr
   030B FA                 1207 	mov	r2,a
                           1208 ;     genCmpEq
                           1209 ;	Peephole 112.b	changed ljmp to sjmp
                           1210 ;	Peephole 199	optimized misc jump sequence
   030C BA C0 03           1211 	cjne	r2,#0xC0,00105$
                           1212 ;00111$:
                           1213 ;	Peephole 200	removed redundant sjmp
   030F                    1214 00112$:
                           1215 ;Initial/src/hpsdr_main.c:153: return app_vendor_IN_cmd();
                           1216 ;     genCall
                           1217 ;     genRet
                           1218 ;	Peephole 112.b	changed ljmp to sjmp
                           1219 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1220 ;	Peephole 253.a	replaced lcall/ret with ljmp
   030F 02 02 84           1221 	ljmp	_app_vendor_IN_cmd
   0312                    1222 00105$:
                           1223 ;Initial/src/hpsdr_main.c:154: else if (bRequestType == VRT_VENDOR_OUT)
                           1224 ;     genPointerGet
                           1225 ;     genFarPointerGet
   0312 90 E6 B8           1226 	mov	dptr,#_SETUPDAT
   0315 E0                 1227 	movx	a,@dptr
   0316 FA                 1228 	mov	r2,a
                           1229 ;     genCmpEq
                           1230 ;	Peephole 112.b	changed ljmp to sjmp
                           1231 ;	Peephole 199	optimized misc jump sequence
   0317 BA 40 03           1232 	cjne	r2,#0x40,00102$
                           1233 ;00113$:
                           1234 ;	Peephole 200	removed redundant sjmp
   031A                    1235 00114$:
                           1236 ;Initial/src/hpsdr_main.c:155: return app_vendor_OUT_cmd();
                           1237 ;     genCall
                           1238 ;     genRet
                           1239 ;	Peephole 112.b	changed ljmp to sjmp
                           1240 ;Initial/src/hpsdr_main.c:157: return 0;    // invalid bRequestType
                           1241 ;     genRet
                           1242 ;	Peephole 237.a	removed sjmp to ret
                           1243 ;	Peephole 253.a	replaced lcall/ret with ljmp
   031A 02 01 B8           1244 	ljmp	_app_vendor_OUT_cmd
   031D                    1245 00102$:
   031D 75 82 00           1246 	mov	dpl,#0x00
   0320                    1247 00107$:
   0320 22                 1248 	ret
                           1249 ;------------------------------------------------------------
                           1250 ;Allocation info for local variables in function 'main_loop'
                           1251 ;------------------------------------------------------------
                           1252 ;------------------------------------------------------------
                           1253 ;Initial/src/hpsdr_main.c:161: main_loop (void)
                           1254 ;	-----------------------------------------
                           1255 ;	 function main_loop
                           1256 ;	-----------------------------------------
   0321                    1257 _main_loop:
                           1258 ;Initial/src/hpsdr_main.c:164: while (1)
   0321                    1259 00104$:
                           1260 ;Initial/src/hpsdr_main.c:166: if (usb_setup_packet_avail ())
                           1261 ;     genIfx
                           1262 ;     genIfxJump
                           1263 ;	Peephole 111	removed ljmp by inverse jump logic
   0321 30 03 FD           1264 	jnb	__usb_got_SUDAV,00104$
   0324                    1265 00110$:
                           1266 ;Initial/src/hpsdr_main.c:167: usb_handle_setup_packet ();
                           1267 ;     genCall
   0324 12 0E E0           1268 	lcall	_usb_handle_setup_packet
                           1269 ;	Peephole 112.b	changed ljmp to sjmp
   0327 80 F8              1270 	sjmp	00104$
   0329                    1271 00106$:
   0329 22                 1272 	ret
                           1273 ;------------------------------------------------------------
                           1274 ;Allocation info for local variables in function 'isr_tick'
                           1275 ;------------------------------------------------------------
                           1276 ;count                     Allocated with name '_isr_tick_count_1_1'
                           1277 ;------------------------------------------------------------
                           1278 ;Initial/src/hpsdr_main.c:178: isr_tick (void) interrupt
                           1279 ;	-----------------------------------------
                           1280 ;	 function isr_tick
                           1281 ;	-----------------------------------------
   032A                    1282 _isr_tick:
   032A C0 E0              1283 	push	acc
                           1284 ;Initial/src/hpsdr_main.c:181: if (--count == 0)
                           1285 ;     genMinus
                           1286 ;     genMinusDec
   032C 15 08              1287 	dec	_isr_tick_count_1_1
                           1288 ;     genCmpEq
   032E E5 08              1289 	mov	a,_isr_tick_count_1_1
                           1290 ;	Peephole 162	removed sjmp by inverse jump logic
   0330 60 02              1291 	jz	00107$
   0332                    1292 00106$:
                           1293 ;	Peephole 112.b	changed ljmp to sjmp
   0332 80 06              1294 	sjmp	00102$
   0334                    1295 00107$:
                           1296 ;Initial/src/hpsdr_main.c:183: count = 50;
                           1297 ;     genAssign
   0334 75 08 32           1298 	mov	_isr_tick_count_1_1,#0x32
                           1299 ;Initial/src/hpsdr_main.c:184: HPSDR_LED_REG ^= bmLED0;
                           1300 ;     genXor
   0337 63 A0 40           1301 	xrl	_IOC,#0x40
   033A                    1302 00102$:
                           1303 ;Initial/src/hpsdr_main.c:186: clear_timer_irq ();
                           1304 ;     genAssign
   033A C2 CF              1305 	clr	_TF2
   033C                    1306 00103$:
   033C D0 E0              1307 	pop	acc
   033E 32                 1308 	reti
                           1309 ;	eliminated unneeded push/pop psw
                           1310 ;	eliminated unneeded push/pop dpl
                           1311 ;	eliminated unneeded push/pop dph
                           1312 ;	eliminated unneeded push/pop b
                           1313 ;------------------------------------------------------------
                           1314 ;Allocation info for local variables in function 'patch_usb_descriptors'
                           1315 ;------------------------------------------------------------
                           1316 ;hw_rev                    Allocated with name '_patch_usb_descriptors_hw_rev_1_1'
                           1317 ;serial_no                 Allocated with name '_patch_usb_descriptors_serial_no_1_1'
                           1318 ;------------------------------------------------------------
                           1319 ;Initial/src/hpsdr_main.c:194: patch_usb_descriptors(void)
                           1320 ;	-----------------------------------------
                           1321 ;	 function patch_usb_descriptors
                           1322 ;	-----------------------------------------
   033F                    1323 _patch_usb_descriptors:
                           1324 ;Initial/src/hpsdr_main.c:199: eeprom_read(I2C_ADDR_BOOT, HW_REV_OFFSET, &hw_rev, 1);	// LSB of device id
                           1325 ;     genAddrOf
   033F 75 0A 00           1326 	mov	_eeprom_read_PARM_3,#_patch_usb_descriptors_hw_rev_1_1
   0342 75 0B 20           1327 	mov	(_eeprom_read_PARM_3 + 1),#(_patch_usb_descriptors_hw_rev_1_1 >> 8)
                           1328 ;     genAssign
   0345 75 09 05           1329 	mov	_eeprom_read_PARM_2,#0x05
                           1330 ;     genAssign
   0348 75 0C 01           1331 	mov	_eeprom_read_PARM_4,#0x01
                           1332 ;     genCall
   034B 75 82 51           1333 	mov	dpl,#0x51
   034E 12 03 97           1334 	lcall	_eeprom_read
                           1335 ;Initial/src/hpsdr_main.c:201: usb_desc_hw_rev_binary_patch_location_0[0] = hw_rev;
                           1336 ;     genAssign
   0351 90 20 00           1337 	mov	dptr,#_patch_usb_descriptors_hw_rev_1_1
   0354 E0                 1338 	movx	a,@dptr
                           1339 ;     genPointerSet
                           1340 ;     genFarPointerSet
                           1341 ;	Peephole 100	removed redundant mov
   0355 FA                 1342 	mov	r2,a
   0356 90 E0 0C           1343 	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_0
   0359 F0                 1344 	movx	@dptr,a
                           1345 ;Initial/src/hpsdr_main.c:202: usb_desc_hw_rev_binary_patch_location_1[0] = hw_rev;
                           1346 ;     genPointerSet
                           1347 ;     genFarPointerSet
   035A 90 E0 5A           1348 	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_1
   035D EA                 1349 	mov	a,r2
   035E F0                 1350 	movx	@dptr,a
                           1351 ;Initial/src/hpsdr_main.c:203: usb_desc_hw_rev_ascii_patch_location_0[0] = hw_rev + '0';     // FIXME if we get > 9
                           1352 ;     genPlus
   035F 74 30              1353 	mov	a,#0x30
                           1354 ;	Peephole 236.a	used r2 instead of ar2
   0361 2A                 1355 	add	a,r2
                           1356 ;     genPointerSet
                           1357 ;     genFarPointerSet
                           1358 ;	Peephole 100	removed redundant mov
   0362 FA                 1359 	mov	r2,a
   0363 90 E0 D0           1360 	mov	dptr,#_usb_desc_hw_rev_ascii_patch_location_0
   0366 F0                 1361 	movx	@dptr,a
   0367                    1362 00101$:
   0367 22                 1363 	ret
                           1364 ;------------------------------------------------------------
                           1365 ;Allocation info for local variables in function 'main'
                           1366 ;------------------------------------------------------------
                           1367 ;------------------------------------------------------------
                           1368 ;Initial/src/hpsdr_main.c:214: main (void)
                           1369 ;	-----------------------------------------
                           1370 ;	 function main
                           1371 ;	-----------------------------------------
   0368                    1372 _main:
                           1373 ;Initial/src/hpsdr_main.c:216: init_hpsdr();
                           1374 ;     genCall
   0368 12 04 82           1375 	lcall	_init_hpsdr
                           1376 ;Initial/src/hpsdr_main.c:218: set_led_0 (1);
                           1377 ;     genCall
   036B 75 82 01           1378 	mov	dpl,#0x01
   036E 12 05 18           1379 	lcall	_set_led_0
                           1380 ;Initial/src/hpsdr_main.c:219: set_led_1 (0);
                           1381 ;     genCall
   0371 75 82 00           1382 	mov	dpl,#0x00
   0374 12 05 25           1383 	lcall	_set_led_1
                           1384 ;Initial/src/hpsdr_main.c:221: EA = 0;		// disable all interrupts
                           1385 ;     genAssign
   0377 C2 AF              1386 	clr	_EA
                           1387 ;Initial/src/hpsdr_main.c:223: patch_usb_descriptors();
                           1388 ;     genCall
   0379 12 03 3F           1389 	lcall	_patch_usb_descriptors
                           1390 ;Initial/src/hpsdr_main.c:225: setup_autovectors ();
                           1391 ;     genCall
   037C 12 08 32           1392 	lcall	_setup_autovectors
                           1393 ;Initial/src/hpsdr_main.c:226: usb_install_handlers ();
                           1394 ;     genCall
   037F 12 0B B2           1395 	lcall	_usb_install_handlers
                           1396 ;Initial/src/hpsdr_main.c:227: hook_timer_tick ((unsigned short) isr_tick);
                           1397 ;     genCast
   0382 7A 2A              1398 	mov	r2,#_isr_tick
   0384 7B 03              1399 	mov	r3,#(_isr_tick >> 8)
                           1400 ;     genCall
   0386 8A 82              1401 	mov	dpl,r2
   0388 8B 83              1402 	mov	dph,r3
   038A 12 0A D1           1403 	lcall	_hook_timer_tick
                           1404 ;Initial/src/hpsdr_main.c:229: EIEX4 = 1;	// disable INT4 FIXME
                           1405 ;     genAssign
   038D D2 EA              1406 	setb	_EIEX4
                           1407 ;Initial/src/hpsdr_main.c:230: EA = 1;			// global interrupt enable
                           1408 ;     genAssign
   038F D2 AF              1409 	setb	_EA
                           1410 ;Initial/src/hpsdr_main.c:232: fx2_renumerate ();	// simulates disconnect / reconnect
                           1411 ;     genCall
   0391 12 05 89           1412 	lcall	_fx2_renumerate
                           1413 ;Initial/src/hpsdr_main.c:234: main_loop ();
                           1414 ;     genCall
                           1415 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0394 02 03 21           1416 	ljmp	_main_loop
                           1417 	.area CSEG    (CODE)
