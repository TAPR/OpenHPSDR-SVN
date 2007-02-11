                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Fri Jan 05 10:13:15 2007
                              5 ;--------------------------------------------------------
                              6 	.module i2c
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _i2c_write_PARM_3
                             13 	.globl _i2c_write_PARM_2
                             14 	.globl _i2c_read_PARM_3
                             15 	.globl _i2c_read_PARM_2
                             16 	.globl _EIPX6
                             17 	.globl _EIPX5
                             18 	.globl _EIPX4
                             19 	.globl _PI2C
                             20 	.globl _PUSB
                             21 	.globl _EIEX6
                             22 	.globl _EIEX5
                             23 	.globl _EIEX4
                             24 	.globl _EI2C
                             25 	.globl _EIUSB
                             26 	.globl _SMOD1
                             27 	.globl _ERESI
                             28 	.globl _RESI
                             29 	.globl _INT6
                             30 	.globl _CY
                             31 	.globl _AC
                             32 	.globl _F0
                             33 	.globl _RS1
                             34 	.globl _RS0
                             35 	.globl _OV
                             36 	.globl _FL
                             37 	.globl _P
                             38 	.globl _TF2
                             39 	.globl _EXF2
                             40 	.globl _RCLK
                             41 	.globl _TCLK
                             42 	.globl _EXEN2
                             43 	.globl _TR2
                             44 	.globl _C_T2
                             45 	.globl _CP_RL2
                             46 	.globl _SM01
                             47 	.globl _SM11
                             48 	.globl _SM21
                             49 	.globl _REN1
                             50 	.globl _TB81
                             51 	.globl _RB81
                             52 	.globl _TI1
                             53 	.globl _RI1
                             54 	.globl _PS1
                             55 	.globl _PT2
                             56 	.globl _PS0
                             57 	.globl _PT1
                             58 	.globl _PX1
                             59 	.globl _PT0
                             60 	.globl _PX0
                             61 	.globl _EA
                             62 	.globl _ES1
                             63 	.globl _ET2
                             64 	.globl _ES0
                             65 	.globl _ET1
                             66 	.globl _EX1
                             67 	.globl _ET0
                             68 	.globl _EX0
                             69 	.globl _SM0
                             70 	.globl _SM1
                             71 	.globl _SM2
                             72 	.globl _REN
                             73 	.globl _TB8
                             74 	.globl _RB8
                             75 	.globl _TI
                             76 	.globl _RI
                             77 	.globl _TF1
                             78 	.globl _TR1
                             79 	.globl _TF0
                             80 	.globl _TR0
                             81 	.globl _IE1
                             82 	.globl _IT1
                             83 	.globl _IE0
                             84 	.globl _IT0
                             85 	.globl _SEL
                             86 	.globl _EIP
                             87 	.globl _B
                             88 	.globl _EIE
                             89 	.globl _ACC
                             90 	.globl _EICON
                             91 	.globl _PSW
                             92 	.globl _TH2
                             93 	.globl _TL2
                             94 	.globl _RCAP2H
                             95 	.globl _RCAP2L
                             96 	.globl _T2CON
                             97 	.globl _SBUF1
                             98 	.globl _SCON1
                             99 	.globl _GPIFSGLDATLNOX
                            100 	.globl _GPIFSGLDATLX
                            101 	.globl _GPIFSGLDATH
                            102 	.globl _GPIFTRIG
                            103 	.globl _EP01STAT
                            104 	.globl _IP
                            105 	.globl _OEE
                            106 	.globl _OED
                            107 	.globl _OEC
                            108 	.globl _OEB
                            109 	.globl _OEA
                            110 	.globl _IOE
                            111 	.globl _IOD
                            112 	.globl _AUTOPTRSETUP
                            113 	.globl _EP68FIFOFLGS
                            114 	.globl _EP24FIFOFLGS
                            115 	.globl _EP2468STAT
                            116 	.globl _IE
                            117 	.globl _INT4CLR
                            118 	.globl _INT2CLR
                            119 	.globl _IOC
                            120 	.globl _AUTODAT2
                            121 	.globl _AUTOPTRL2
                            122 	.globl _AUTOPTRH2
                            123 	.globl _AUTODAT1
                            124 	.globl _APTR1L
                            125 	.globl _APTR1H
                            126 	.globl _SBUF0
                            127 	.globl _SCON0
                            128 	.globl _MPAGE
                            129 	.globl _EXIF
                            130 	.globl _IOB
                            131 	.globl _CKCON
                            132 	.globl _TH1
                            133 	.globl _TH0
                            134 	.globl _TL1
                            135 	.globl _TL0
                            136 	.globl _TMOD
                            137 	.globl _TCON
                            138 	.globl _PCON
                            139 	.globl _DPS
                            140 	.globl _DPH1
                            141 	.globl _DPL1
                            142 	.globl _DPH
                            143 	.globl _DPL
                            144 	.globl _SP
                            145 	.globl _IOA
                            146 	.globl _EP8FIFOBUF
                            147 	.globl _EP6FIFOBUF
                            148 	.globl _EP4FIFOBUF
                            149 	.globl _EP2FIFOBUF
                            150 	.globl _EP1INBUF
                            151 	.globl _EP1OUTBUF
                            152 	.globl _EP0BUF
                            153 	.globl _CT4
                            154 	.globl _CT3
                            155 	.globl _CT2
                            156 	.globl _CT1
                            157 	.globl _USBTEST
                            158 	.globl _TESTCFG
                            159 	.globl _DBUG
                            160 	.globl _UDMACRCQUAL
                            161 	.globl _UDMACRCL
                            162 	.globl _UDMACRCH
                            163 	.globl _GPIFHOLDAMOUNT
                            164 	.globl _FLOWSTBHPERIOD
                            165 	.globl _FLOWSTBEDGE
                            166 	.globl _FLOWSTB
                            167 	.globl _FLOWHOLDOFF
                            168 	.globl _FLOWEQ1CTL
                            169 	.globl _FLOWEQ0CTL
                            170 	.globl _FLOWLOGIC
                            171 	.globl _FLOWSTATE
                            172 	.globl _GPIFABORT
                            173 	.globl _GPIFREADYSTAT
                            174 	.globl _GPIFREADYCFG
                            175 	.globl _XGPIFSGLDATLNOX
                            176 	.globl _XGPIFSGLDATLX
                            177 	.globl _XGPIFSGLDATH
                            178 	.globl _EP8GPIFTRIG
                            179 	.globl _EP8GPIFPFSTOP
                            180 	.globl _EP8GPIFFLGSEL
                            181 	.globl _EP6GPIFTRIG
                            182 	.globl _EP6GPIFPFSTOP
                            183 	.globl _EP6GPIFFLGSEL
                            184 	.globl _EP4GPIFTRIG
                            185 	.globl _EP4GPIFPFSTOP
                            186 	.globl _EP4GPIFFLGSEL
                            187 	.globl _EP2GPIFTRIG
                            188 	.globl _EP2GPIFPFSTOP
                            189 	.globl _EP2GPIFFLGSEL
                            190 	.globl _GPIFTCB0
                            191 	.globl _GPIFTCB1
                            192 	.globl _GPIFTCB2
                            193 	.globl _GPIFTCB3
                            194 	.globl _GPIFADRL
                            195 	.globl _GPIFADRH
                            196 	.globl _GPIFCTLCFG
                            197 	.globl _GPIFIDLECTL
                            198 	.globl _GPIFIDLECS
                            199 	.globl _GPIFWFSELECT
                            200 	.globl _SETUPDAT
                            201 	.globl _SUDPTRCTL
                            202 	.globl _SUDPTRL
                            203 	.globl _SUDPTRH
                            204 	.globl _EP8FIFOBCL
                            205 	.globl _EP8FIFOBCH
                            206 	.globl _EP6FIFOBCL
                            207 	.globl _EP6FIFOBCH
                            208 	.globl _EP4FIFOBCL
                            209 	.globl _EP4FIFOBCH
                            210 	.globl _EP2FIFOBCL
                            211 	.globl _EP2FIFOBCH
                            212 	.globl _EP8FIFOFLGS
                            213 	.globl _EP6FIFOFLGS
                            214 	.globl _EP4FIFOFLGS
                            215 	.globl _EP2FIFOFLGS
                            216 	.globl _EP8CS
                            217 	.globl _EP6CS
                            218 	.globl _EP4CS
                            219 	.globl _EP2CS
                            220 	.globl _EP1INCS
                            221 	.globl _EP1OUTCS
                            222 	.globl _EP0CS
                            223 	.globl _EP8BCL
                            224 	.globl _EP8BCH
                            225 	.globl _EP6BCL
                            226 	.globl _EP6BCH
                            227 	.globl _EP4BCL
                            228 	.globl _EP4BCH
                            229 	.globl _EP2BCL
                            230 	.globl _EP2BCH
                            231 	.globl _EP1INBC
                            232 	.globl _EP1OUTBC
                            233 	.globl _EP0BCL
                            234 	.globl _EP0BCH
                            235 	.globl _FNADDR
                            236 	.globl _MICROFRAME
                            237 	.globl _USBFRAMEL
                            238 	.globl _USBFRAMEH
                            239 	.globl _TOGCTL
                            240 	.globl _WAKEUPCS
                            241 	.globl _SUSPEND
                            242 	.globl _USBCS
                            243 	.globl _XAUTODAT2
                            244 	.globl _XAUTODAT1
                            245 	.globl _I2CTL
                            246 	.globl _I2DAT
                            247 	.globl _I2CS
                            248 	.globl _PORTECFG
                            249 	.globl _PORTCCFG
                            250 	.globl _PORTACFG
                            251 	.globl _INTSETUP
                            252 	.globl _INT4IVEC
                            253 	.globl _INT2IVEC
                            254 	.globl _CLRERRCNT
                            255 	.globl _ERRCNTLIM
                            256 	.globl _USBERRIRQ
                            257 	.globl _USBERRIE
                            258 	.globl _GPIFIRQ
                            259 	.globl _GPIFIE
                            260 	.globl _EPIRQ
                            261 	.globl _EPIE
                            262 	.globl _USBIRQ
                            263 	.globl _USBIE
                            264 	.globl _NAKIRQ
                            265 	.globl _NAKIE
                            266 	.globl _IBNIRQ
                            267 	.globl _IBNIE
                            268 	.globl _EP8FIFOIRQ
                            269 	.globl _EP8FIFOIE
                            270 	.globl _EP6FIFOIRQ
                            271 	.globl _EP6FIFOIE
                            272 	.globl _EP4FIFOIRQ
                            273 	.globl _EP4FIFOIE
                            274 	.globl _EP2FIFOIRQ
                            275 	.globl _EP2FIFOIE
                            276 	.globl _OUTPKTEND
                            277 	.globl _INPKTEND
                            278 	.globl _EP8ISOINPKTS
                            279 	.globl _EP6ISOINPKTS
                            280 	.globl _EP4ISOINPKTS
                            281 	.globl _EP2ISOINPKTS
                            282 	.globl _EP8FIFOPFL
                            283 	.globl _EP8FIFOPFH
                            284 	.globl _EP6FIFOPFL
                            285 	.globl _EP6FIFOPFH
                            286 	.globl _EP4FIFOPFL
                            287 	.globl _EP4FIFOPFH
                            288 	.globl _EP2FIFOPFL
                            289 	.globl _EP2FIFOPFH
                            290 	.globl _EP8AUTOINLENL
                            291 	.globl _EP8AUTOINLENH
                            292 	.globl _EP6AUTOINLENL
                            293 	.globl _EP6AUTOINLENH
                            294 	.globl _EP4AUTOINLENL
                            295 	.globl _EP4AUTOINLENH
                            296 	.globl _EP2AUTOINLENL
                            297 	.globl _EP2AUTOINLENH
                            298 	.globl _EP8FIFOCFG
                            299 	.globl _EP6FIFOCFG
                            300 	.globl _EP4FIFOCFG
                            301 	.globl _EP2FIFOCFG
                            302 	.globl _EP8CFG
                            303 	.globl _EP6CFG
                            304 	.globl _EP4CFG
                            305 	.globl _EP2CFG
                            306 	.globl _EP1INCFG
                            307 	.globl _EP1OUTCFG
                            308 	.globl _REVCTL
                            309 	.globl _REVID
                            310 	.globl _FIFOPINPOLAR
                            311 	.globl _UART230
                            312 	.globl _BPADDRL
                            313 	.globl _BPADDRH
                            314 	.globl _BREAKPT
                            315 	.globl _FIFORESET
                            316 	.globl _PINFLAGSCD
                            317 	.globl _PINFLAGSAB
                            318 	.globl _IFCONFIG
                            319 	.globl _CPUCS
                            320 	.globl _RES_WAVEDATA_END
                            321 	.globl _GPIF_WAVE_DATA
                            322 	.globl _i2c_read
                            323 	.globl _i2c_write
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
                            462 ;--------------------------------------------------------
                            463 ; overlayable register banks 
                            464 ;--------------------------------------------------------
                            465 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     466 	.ds 8
                            467 ;--------------------------------------------------------
                            468 ; internal ram data
                            469 ;--------------------------------------------------------
                            470 	.area DSEG    (DATA)
                            471 ;--------------------------------------------------------
                            472 ; overlayable items in internal ram 
                            473 ;--------------------------------------------------------
                            474 	.area	OSEG    (OVR,DATA)
   002B                     475 _i2c_read_PARM_2::
   002B                     476 	.ds 2
   002D                     477 _i2c_read_PARM_3::
   002D                     478 	.ds 1
   002E                     479 _i2c_read_junk_1_1::
   002E                     480 	.ds 1
                            481 	.area	OSEG    (OVR,DATA)
   002B                     482 _i2c_write_PARM_2::
   002B                     483 	.ds 2
   002D                     484 _i2c_write_PARM_3::
   002D                     485 	.ds 1
                            486 ;--------------------------------------------------------
                            487 ; indirectly addressable internal ram data
                            488 ;--------------------------------------------------------
                            489 	.area ISEG    (DATA)
                            490 ;--------------------------------------------------------
                            491 ; bit data
                            492 ;--------------------------------------------------------
                            493 	.area BSEG    (BIT)
                            494 ;--------------------------------------------------------
                            495 ; paged external ram data
                            496 ;--------------------------------------------------------
                            497 	.area PSEG    (PAG,XDATA)
                            498 ;--------------------------------------------------------
                            499 ; external ram data
                            500 ;--------------------------------------------------------
                            501 	.area XSEG    (XDATA)
                    E400    502 _GPIF_WAVE_DATA	=	0xe400
                    E480    503 _RES_WAVEDATA_END	=	0xe480
                    E600    504 _CPUCS	=	0xe600
                    E601    505 _IFCONFIG	=	0xe601
                    E602    506 _PINFLAGSAB	=	0xe602
                    E603    507 _PINFLAGSCD	=	0xe603
                    E604    508 _FIFORESET	=	0xe604
                    E605    509 _BREAKPT	=	0xe605
                    E606    510 _BPADDRH	=	0xe606
                    E607    511 _BPADDRL	=	0xe607
                    E608    512 _UART230	=	0xe608
                    E609    513 _FIFOPINPOLAR	=	0xe609
                    E60A    514 _REVID	=	0xe60a
                    E60B    515 _REVCTL	=	0xe60b
                    E610    516 _EP1OUTCFG	=	0xe610
                    E611    517 _EP1INCFG	=	0xe611
                    E612    518 _EP2CFG	=	0xe612
                    E613    519 _EP4CFG	=	0xe613
                    E614    520 _EP6CFG	=	0xe614
                    E615    521 _EP8CFG	=	0xe615
                    E618    522 _EP2FIFOCFG	=	0xe618
                    E619    523 _EP4FIFOCFG	=	0xe619
                    E61A    524 _EP6FIFOCFG	=	0xe61a
                    E61B    525 _EP8FIFOCFG	=	0xe61b
                    E620    526 _EP2AUTOINLENH	=	0xe620
                    E621    527 _EP2AUTOINLENL	=	0xe621
                    E622    528 _EP4AUTOINLENH	=	0xe622
                    E623    529 _EP4AUTOINLENL	=	0xe623
                    E624    530 _EP6AUTOINLENH	=	0xe624
                    E625    531 _EP6AUTOINLENL	=	0xe625
                    E626    532 _EP8AUTOINLENH	=	0xe626
                    E627    533 _EP8AUTOINLENL	=	0xe627
                    E630    534 _EP2FIFOPFH	=	0xe630
                    E631    535 _EP2FIFOPFL	=	0xe631
                    E632    536 _EP4FIFOPFH	=	0xe632
                    E633    537 _EP4FIFOPFL	=	0xe633
                    E634    538 _EP6FIFOPFH	=	0xe634
                    E635    539 _EP6FIFOPFL	=	0xe635
                    E636    540 _EP8FIFOPFH	=	0xe636
                    E637    541 _EP8FIFOPFL	=	0xe637
                    E640    542 _EP2ISOINPKTS	=	0xe640
                    E641    543 _EP4ISOINPKTS	=	0xe641
                    E642    544 _EP6ISOINPKTS	=	0xe642
                    E643    545 _EP8ISOINPKTS	=	0xe643
                    E648    546 _INPKTEND	=	0xe648
                    E649    547 _OUTPKTEND	=	0xe649
                    E650    548 _EP2FIFOIE	=	0xe650
                    E651    549 _EP2FIFOIRQ	=	0xe651
                    E652    550 _EP4FIFOIE	=	0xe652
                    E653    551 _EP4FIFOIRQ	=	0xe653
                    E654    552 _EP6FIFOIE	=	0xe654
                    E655    553 _EP6FIFOIRQ	=	0xe655
                    E656    554 _EP8FIFOIE	=	0xe656
                    E657    555 _EP8FIFOIRQ	=	0xe657
                    E658    556 _IBNIE	=	0xe658
                    E659    557 _IBNIRQ	=	0xe659
                    E65A    558 _NAKIE	=	0xe65a
                    E65B    559 _NAKIRQ	=	0xe65b
                    E65C    560 _USBIE	=	0xe65c
                    E65D    561 _USBIRQ	=	0xe65d
                    E65E    562 _EPIE	=	0xe65e
                    E65F    563 _EPIRQ	=	0xe65f
                    E660    564 _GPIFIE	=	0xe660
                    E661    565 _GPIFIRQ	=	0xe661
                    E662    566 _USBERRIE	=	0xe662
                    E663    567 _USBERRIRQ	=	0xe663
                    E664    568 _ERRCNTLIM	=	0xe664
                    E665    569 _CLRERRCNT	=	0xe665
                    E666    570 _INT2IVEC	=	0xe666
                    E667    571 _INT4IVEC	=	0xe667
                    E668    572 _INTSETUP	=	0xe668
                    E670    573 _PORTACFG	=	0xe670
                    E671    574 _PORTCCFG	=	0xe671
                    E672    575 _PORTECFG	=	0xe672
                    E678    576 _I2CS	=	0xe678
                    E679    577 _I2DAT	=	0xe679
                    E67A    578 _I2CTL	=	0xe67a
                    E67B    579 _XAUTODAT1	=	0xe67b
                    E67C    580 _XAUTODAT2	=	0xe67c
                    E680    581 _USBCS	=	0xe680
                    E681    582 _SUSPEND	=	0xe681
                    E682    583 _WAKEUPCS	=	0xe682
                    E683    584 _TOGCTL	=	0xe683
                    E684    585 _USBFRAMEH	=	0xe684
                    E685    586 _USBFRAMEL	=	0xe685
                    E686    587 _MICROFRAME	=	0xe686
                    E687    588 _FNADDR	=	0xe687
                    E68A    589 _EP0BCH	=	0xe68a
                    E68B    590 _EP0BCL	=	0xe68b
                    E68D    591 _EP1OUTBC	=	0xe68d
                    E68F    592 _EP1INBC	=	0xe68f
                    E690    593 _EP2BCH	=	0xe690
                    E691    594 _EP2BCL	=	0xe691
                    E694    595 _EP4BCH	=	0xe694
                    E695    596 _EP4BCL	=	0xe695
                    E698    597 _EP6BCH	=	0xe698
                    E699    598 _EP6BCL	=	0xe699
                    E69C    599 _EP8BCH	=	0xe69c
                    E69D    600 _EP8BCL	=	0xe69d
                    E6A0    601 _EP0CS	=	0xe6a0
                    E6A1    602 _EP1OUTCS	=	0xe6a1
                    E6A2    603 _EP1INCS	=	0xe6a2
                    E6A3    604 _EP2CS	=	0xe6a3
                    E6A4    605 _EP4CS	=	0xe6a4
                    E6A5    606 _EP6CS	=	0xe6a5
                    E6A6    607 _EP8CS	=	0xe6a6
                    E6A7    608 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    609 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    610 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    611 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    612 _EP2FIFOBCH	=	0xe6ab
                    E6AC    613 _EP2FIFOBCL	=	0xe6ac
                    E6AD    614 _EP4FIFOBCH	=	0xe6ad
                    E6AE    615 _EP4FIFOBCL	=	0xe6ae
                    E6AF    616 _EP6FIFOBCH	=	0xe6af
                    E6B0    617 _EP6FIFOBCL	=	0xe6b0
                    E6B1    618 _EP8FIFOBCH	=	0xe6b1
                    E6B2    619 _EP8FIFOBCL	=	0xe6b2
                    E6B3    620 _SUDPTRH	=	0xe6b3
                    E6B4    621 _SUDPTRL	=	0xe6b4
                    E6B5    622 _SUDPTRCTL	=	0xe6b5
                    E6B8    623 _SETUPDAT	=	0xe6b8
                    E6C0    624 _GPIFWFSELECT	=	0xe6c0
                    E6C1    625 _GPIFIDLECS	=	0xe6c1
                    E6C2    626 _GPIFIDLECTL	=	0xe6c2
                    E6C3    627 _GPIFCTLCFG	=	0xe6c3
                    E6C4    628 _GPIFADRH	=	0xe6c4
                    E6C5    629 _GPIFADRL	=	0xe6c5
                    E6CE    630 _GPIFTCB3	=	0xe6ce
                    E6CF    631 _GPIFTCB2	=	0xe6cf
                    E6D0    632 _GPIFTCB1	=	0xe6d0
                    E6D1    633 _GPIFTCB0	=	0xe6d1
                    E6D2    634 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    635 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    636 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    637 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    638 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    639 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    640 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    641 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    642 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    643 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    644 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    645 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    646 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    647 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    648 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    649 _GPIFREADYCFG	=	0xe6f3
                    E6F4    650 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    651 _GPIFABORT	=	0xe6f5
                    E6C6    652 _FLOWSTATE	=	0xe6c6
                    E6C7    653 _FLOWLOGIC	=	0xe6c7
                    E6C8    654 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    655 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    656 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    657 _FLOWSTB	=	0xe6cb
                    E6CC    658 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    659 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    660 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    661 _UDMACRCH	=	0xe67d
                    E67E    662 _UDMACRCL	=	0xe67e
                    E67F    663 _UDMACRCQUAL	=	0xe67f
                    E6F8    664 _DBUG	=	0xe6f8
                    E6F9    665 _TESTCFG	=	0xe6f9
                    E6FA    666 _USBTEST	=	0xe6fa
                    E6FB    667 _CT1	=	0xe6fb
                    E6FC    668 _CT2	=	0xe6fc
                    E6FD    669 _CT3	=	0xe6fd
                    E6FE    670 _CT4	=	0xe6fe
                    E740    671 _EP0BUF	=	0xe740
                    E780    672 _EP1OUTBUF	=	0xe780
                    E7C0    673 _EP1INBUF	=	0xe7c0
                    F000    674 _EP2FIFOBUF	=	0xf000
                    F400    675 _EP4FIFOBUF	=	0xf400
                    F800    676 _EP6FIFOBUF	=	0xf800
                    FC00    677 _EP8FIFOBUF	=	0xfc00
                            678 ;--------------------------------------------------------
                            679 ; external initialized ram data
                            680 ;--------------------------------------------------------
                            681 	.area CSEG    (CODE)
                            682 	.area GSINIT0 (CODE)
                            683 	.area GSINIT1 (CODE)
                            684 	.area GSINIT2 (CODE)
                            685 	.area GSINIT3 (CODE)
                            686 	.area GSINIT4 (CODE)
                            687 	.area GSINIT5 (CODE)
                            688 ;--------------------------------------------------------
                            689 ; global & static initialisations
                            690 ;--------------------------------------------------------
                            691 	.area CSEG    (CODE)
                            692 	.area GSINIT  (CODE)
                            693 	.area GSFINAL (CODE)
                            694 	.area GSINIT  (CODE)
                            695 ;--------------------------------------------------------
                            696 ; Home
                            697 ;--------------------------------------------------------
                            698 	.area HOME    (CODE)
                            699 	.area CSEG    (CODE)
                            700 ;--------------------------------------------------------
                            701 ; code
                            702 ;--------------------------------------------------------
                            703 	.area CSEG    (CODE)
                            704 ;------------------------------------------------------------
                            705 ;Allocation info for local variables in function 'i2c_read'
                            706 ;------------------------------------------------------------
                            707 ;buf                       Allocated with name '_i2c_read_PARM_2'
                            708 ;len                       Allocated with name '_i2c_read_PARM_3'
                            709 ;addr                      Allocated to registers r2 
                            710 ;junk                      Allocated with name '_i2c_read_junk_1_1'
                            711 ;------------------------------------------------------------
                            712 ;src/lib/i2c.c:36: i2c_read (unsigned char addr, xdata unsigned char *buf, unsigned char len)
                            713 ;	-----------------------------------------
                            714 ;	 function i2c_read
                            715 ;	-----------------------------------------
   0347                     716 _i2c_read:
                    0002    717 	ar2 = 0x02
                    0003    718 	ar3 = 0x03
                    0004    719 	ar4 = 0x04
                    0005    720 	ar5 = 0x05
                    0006    721 	ar6 = 0x06
                    0007    722 	ar7 = 0x07
                    0000    723 	ar0 = 0x00
                    0001    724 	ar1 = 0x01
                            725 ;     genReceive
   0347 AA 82               726 	mov	r2,dpl
                            727 ;src/lib/i2c.c:40: if (len == 0)			// reading zero bytes always works
                            728 ;     genCmpEq
   0349 E5 2D               729 	mov	a,_i2c_read_PARM_3
                            730 ;	Peephole 162	removed sjmp by inverse jump logic
   034B 60 02               731 	jz	00149$
   034D                     732 00148$:
                            733 ;	Peephole 112.b	changed ljmp to sjmp
   034D 80 04               734 	sjmp	00103$
   034F                     735 00149$:
                            736 ;src/lib/i2c.c:41: return 1;
                            737 ;     genRet
   034F 75 82 01            738 	mov	dpl,#0x01
                            739 ;	Peephole 251.a	replaced ljmp to ret with ret
   0352 22                  740 	ret
                            741 ;src/lib/i2c.c:43: while (I2CS & bmSTOP)		// wait for stop to clear
   0353                     742 00103$:
                            743 ;     genAssign
   0353 90 E6 78            744 	mov	dptr,#_I2CS
   0356 E0                  745 	movx	a,@dptr
                            746 ;     genAnd
                            747 ;	Peephole 105	removed redundant mov
   0357 FB                  748 	mov	r3,a
                            749 ;     genIfxJump
                            750 ;	Peephole 112.a	removed ljmp by inverse jump logic
   0358 20 E6 F8            751 	jb	acc.6,00103$
   035B                     752 00150$:
                            753 ;src/lib/i2c.c:46: I2CS = bmSTART;
                            754 ;     genAssign
   035B 90 E6 78            755 	mov	dptr,#_I2CS
   035E 74 80               756 	mov	a,#0x80
   0360 F0                  757 	movx	@dptr,a
                            758 ;src/lib/i2c.c:47: I2DAT = (addr << 1) | 1;	// write address and direction (1's the read bit)
                            759 ;     genLeftShift
                            760 ;     genLeftShiftLiteral
                            761 ;     genlshOne
                            762 ;	Peephole 254	optimized left shift
   0361 EA                  763 	mov	a,r2
   0362 2A                  764 	add	a,r2
   0363 FA                  765 	mov	r2,a
                            766 ;     genOr
   0364 90 E6 79            767 	mov	dptr,#_I2DAT
   0367 74 01               768 	mov	a,#0x01
   0369 4A                  769 	orl	a,r2
   036A F0                  770 	movx	@dptr,a
                            771 ;src/lib/i2c.c:49: while ((I2CS & bmDONE) == 0)
   036B                     772 00106$:
                            773 ;     genAssign
   036B 90 E6 78            774 	mov	dptr,#_I2CS
   036E E0                  775 	movx	a,@dptr
   036F FA                  776 	mov	r2,a
                            777 ;     genAnd
   0370 53 02 01            778 	anl	ar2,#0x01
                            779 ;     genCmpEq
   0373 BA 00 02            780 	cjne	r2,#0x00,00151$
                            781 ;	Peephole 112.b	changed ljmp to sjmp
   0376 80 F3               782 	sjmp	00106$
   0378                     783 00151$:
                            784 ;src/lib/i2c.c:52: if ((I2CS & bmBERR) || (I2CS & bmACK) == 0)	// no device answered...
                            785 ;     genAssign
   0378 90 E6 78            786 	mov	dptr,#_I2CS
   037B E0                  787 	movx	a,@dptr
                            788 ;     genAnd
                            789 ;	Peephole 105	removed redundant mov
   037C FA                  790 	mov	r2,a
                            791 ;     genIfxJump
   037D 30 E2 03            792 	jnb	acc.2,00152$
   0380 02 04 12            793 	ljmp	00129$
   0383                     794 00152$:
                            795 ;     genAssign
   0383 90 E6 78            796 	mov	dptr,#_I2CS
   0386 E0                  797 	movx	a,@dptr
   0387 FA                  798 	mov	r2,a
                            799 ;     genAnd
   0388 53 02 02            800 	anl	ar2,#0x02
                            801 ;     genCmpEq
   038B BA 00 03            802 	cjne	r2,#0x00,00153$
   038E 02 04 12            803 	ljmp	00129$
   0391                     804 00153$:
                            805 ;src/lib/i2c.c:55: if (len == 1)
                            806 ;     genCmpEq
   0391 E5 2D               807 	mov	a,_i2c_read_PARM_3
                            808 ;	Peephole 112.b	changed ljmp to sjmp
                            809 ;	Peephole 199	optimized misc jump sequence
   0393 B4 01 08            810 	cjne	a,#0x01,00113$
                            811 ;00154$:
                            812 ;	Peephole 200	removed redundant sjmp
   0396                     813 00155$:
                            814 ;src/lib/i2c.c:56: I2CS |= bmLASTRD;
                            815 ;     genAssign
                            816 ;     genOr
                            817 ;	Peephole 248.a	optimized or to xdata
   0396 90 E6 78            818 	mov	dptr,#_I2CS
   0399 E0                  819 	movx	a,@dptr
   039A FA                  820 	mov	r2,a
   039B 44 20               821 	orl	a,#0x20
   039D F0                  822 	movx	@dptr,a
   039E                     823 00113$:
                            824 ;src/lib/i2c.c:58: junk = I2DAT;			// trigger the first read cycle
                            825 ;     genAssign
   039E 90 E6 79            826 	mov	dptr,#_I2DAT
   03A1 E0                  827 	movx	a,@dptr
   03A2 F5 2E               828 	mov	_i2c_read_junk_1_1,a
                            829 ;src/lib/i2c.c:60: while (--len != 0){
                            830 ;     genAssign
   03A4 AA 2B               831 	mov	r2,_i2c_read_PARM_2
   03A6 AB 2C               832 	mov	r3,(_i2c_read_PARM_2 + 1)
                            833 ;     genAssign
   03A8 AC 2D               834 	mov	r4,_i2c_read_PARM_3
   03AA                     835 00121$:
                            836 ;     genMinus
                            837 ;     genMinusDec
                            838 ;     genCmpEq
                            839 ;	Peephole 257	optimized decrement with compare
   03AA DC 02               840 	djnz	r4,00156$
                            841 ;	Peephole 112.b	changed ljmp to sjmp
   03AC 80 35               842 	sjmp	00124$
   03AE                     843 00156$:
                            844 ;src/lib/i2c.c:61: while ((I2CS & bmDONE) == 0)
   03AE                     845 00114$:
                            846 ;     genAssign
   03AE 90 E6 78            847 	mov	dptr,#_I2CS
   03B1 E0                  848 	movx	a,@dptr
   03B2 FD                  849 	mov	r5,a
                            850 ;     genAnd
   03B3 53 05 01            851 	anl	ar5,#0x01
                            852 ;     genCmpEq
   03B6 BD 00 02            853 	cjne	r5,#0x00,00157$
                            854 ;	Peephole 112.b	changed ljmp to sjmp
   03B9 80 F3               855 	sjmp	00114$
   03BB                     856 00157$:
                            857 ;src/lib/i2c.c:64: if (I2CS & bmBERR)
                            858 ;     genAssign
   03BB 90 E6 78            859 	mov	dptr,#_I2CS
   03BE E0                  860 	movx	a,@dptr
                            861 ;     genAnd
                            862 ;	Peephole 105	removed redundant mov
   03BF FD                  863 	mov	r5,a
                            864 ;     genIfxJump
                            865 ;	Peephole 112.a	removed ljmp by inverse jump logic
   03C0 20 E2 4F            866 	jb	acc.2,00129$
   03C3                     867 00158$:
                            868 ;src/lib/i2c.c:67: if (len == 1)
                            869 ;     genCmpEq
                            870 ;	Peephole 112.b	changed ljmp to sjmp
                            871 ;	Peephole 199	optimized misc jump sequence
   03C3 BC 01 08            872 	cjne	r4,#0x01,00120$
                            873 ;00159$:
                            874 ;	Peephole 200	removed redundant sjmp
   03C6                     875 00160$:
                            876 ;src/lib/i2c.c:68: I2CS |= bmLASTRD;
                            877 ;     genAssign
                            878 ;     genOr
                            879 ;	Peephole 248.a	optimized or to xdata
   03C6 90 E6 78            880 	mov	dptr,#_I2CS
   03C9 E0                  881 	movx	a,@dptr
   03CA FD                  882 	mov	r5,a
   03CB 44 20               883 	orl	a,#0x20
   03CD F0                  884 	movx	@dptr,a
   03CE                     885 00120$:
                            886 ;src/lib/i2c.c:70: *buf++ = I2DAT;		// get data, trigger another read
                            887 ;     genAssign
   03CE 90 E6 79            888 	mov	dptr,#_I2DAT
   03D1 E0                  889 	movx	a,@dptr
                            890 ;     genPointerSet
                            891 ;     genFarPointerSet
                            892 ;	Peephole 136	removed redundant moves
   03D2 FD                  893 	mov	r5,a
   03D3 8A 82               894 	mov	dpl,r2
   03D5 8B 83               895 	mov	dph,r3
   03D7 F0                  896 	movx	@dptr,a
   03D8 A3                  897 	inc	dptr
   03D9 AA 82               898 	mov	r2,dpl
   03DB AB 83               899 	mov	r3,dph
                            900 ;     genAssign
   03DD 8A 2B               901 	mov	_i2c_read_PARM_2,r2
   03DF 8B 2C               902 	mov	(_i2c_read_PARM_2 + 1),r3
                            903 ;src/lib/i2c.c:75: while ((I2CS & bmDONE) == 0)
                            904 ;	Peephole 112.b	changed ljmp to sjmp
   03E1 80 C7               905 	sjmp	00121$
   03E3                     906 00124$:
                            907 ;     genAssign
   03E3 90 E6 78            908 	mov	dptr,#_I2CS
   03E6 E0                  909 	movx	a,@dptr
   03E7 FA                  910 	mov	r2,a
                            911 ;     genAnd
   03E8 53 02 01            912 	anl	ar2,#0x01
                            913 ;     genCmpEq
   03EB BA 00 02            914 	cjne	r2,#0x00,00161$
                            915 ;	Peephole 112.b	changed ljmp to sjmp
   03EE 80 F3               916 	sjmp	00124$
   03F0                     917 00161$:
                            918 ;src/lib/i2c.c:78: if (I2CS & bmBERR)
                            919 ;     genAssign
   03F0 90 E6 78            920 	mov	dptr,#_I2CS
   03F3 E0                  921 	movx	a,@dptr
                            922 ;     genAnd
                            923 ;	Peephole 105	removed redundant mov
   03F4 FA                  924 	mov	r2,a
                            925 ;     genIfxJump
                            926 ;	Peephole 112.a	removed ljmp by inverse jump logic
   03F5 20 E2 1A            927 	jb	acc.2,00129$
   03F8                     928 00162$:
                            929 ;src/lib/i2c.c:81: I2CS |= bmSTOP;
                            930 ;     genAssign
                            931 ;     genOr
                            932 ;	Peephole 248.a	optimized or to xdata
   03F8 90 E6 78            933 	mov	dptr,#_I2CS
   03FB E0                  934 	movx	a,@dptr
   03FC FA                  935 	mov	r2,a
   03FD 44 40               936 	orl	a,#0x40
   03FF F0                  937 	movx	@dptr,a
                            938 ;src/lib/i2c.c:82: *buf = I2DAT;
                            939 ;     genAssign
   0400 AA 2B               940 	mov	r2,_i2c_read_PARM_2
   0402 AB 2C               941 	mov	r3,(_i2c_read_PARM_2 + 1)
                            942 ;     genAssign
   0404 90 E6 79            943 	mov	dptr,#_I2DAT
   0407 E0                  944 	movx	a,@dptr
                            945 ;     genPointerSet
                            946 ;     genFarPointerSet
                            947 ;	Peephole 136	removed redundant moves
   0408 FC                  948 	mov	r4,a
   0409 8A 82               949 	mov	dpl,r2
   040B 8B 83               950 	mov	dph,r3
   040D F0                  951 	movx	@dptr,a
                            952 ;src/lib/i2c.c:84: return 1;
                            953 ;     genRet
   040E 75 82 01            954 	mov	dpl,#0x01
                            955 ;src/lib/i2c.c:86: fail:
                            956 ;	Peephole 112.b	changed ljmp to sjmp
                            957 ;	Peephole 251.b	replaced sjmp to ret with ret
   0411 22                  958 	ret
   0412                     959 00129$:
                            960 ;src/lib/i2c.c:87: I2CS |= bmSTOP;
                            961 ;     genAssign
                            962 ;     genOr
                            963 ;	Peephole 248.a	optimized or to xdata
   0412 90 E6 78            964 	mov	dptr,#_I2CS
   0415 E0                  965 	movx	a,@dptr
   0416 FA                  966 	mov	r2,a
   0417 44 40               967 	orl	a,#0x40
   0419 F0                  968 	movx	@dptr,a
                            969 ;src/lib/i2c.c:88: return 0;
                            970 ;     genRet
   041A 75 82 00            971 	mov	dpl,#0x00
   041D                     972 00130$:
   041D 22                  973 	ret
                            974 ;------------------------------------------------------------
                            975 ;Allocation info for local variables in function 'i2c_write'
                            976 ;------------------------------------------------------------
                            977 ;buf                       Allocated with name '_i2c_write_PARM_2'
                            978 ;len                       Allocated with name '_i2c_write_PARM_3'
                            979 ;addr                      Allocated to registers r2 
                            980 ;------------------------------------------------------------
                            981 ;src/lib/i2c.c:95: i2c_write (unsigned char addr, xdata const unsigned char *buf, unsigned char len)
                            982 ;	-----------------------------------------
                            983 ;	 function i2c_write
                            984 ;	-----------------------------------------
   041E                     985 _i2c_write:
                            986 ;     genReceive
   041E AA 82               987 	mov	r2,dpl
                            988 ;src/lib/i2c.c:97: while (I2CS & bmSTOP)		// wait for stop to clear
   0420                     989 00101$:
                            990 ;     genAssign
   0420 90 E6 78            991 	mov	dptr,#_I2CS
   0423 E0                  992 	movx	a,@dptr
                            993 ;     genAnd
                            994 ;	Peephole 105	removed redundant mov
   0424 FB                  995 	mov	r3,a
                            996 ;     genIfxJump
                            997 ;	Peephole 112.a	removed ljmp by inverse jump logic
   0425 20 E6 F8            998 	jb	acc.6,00101$
   0428                     999 00133$:
                           1000 ;src/lib/i2c.c:100: I2CS = bmSTART;
                           1001 ;     genAssign
   0428 90 E6 78           1002 	mov	dptr,#_I2CS
   042B 74 80              1003 	mov	a,#0x80
   042D F0                 1004 	movx	@dptr,a
                           1005 ;src/lib/i2c.c:101: I2DAT = (addr << 1) | 0;	// write address and direction (0's the write bit)
                           1006 ;     genLeftShift
                           1007 ;     genLeftShiftLiteral
                           1008 ;     genlshOne
                           1009 ;	Peephole 254	optimized left shift
   042E EA                 1010 	mov	a,r2
   042F 2A                 1011 	add	a,r2
                           1012 ;     genAssign
                           1013 ;	Peephole 100	removed redundant mov
   0430 FA                 1014 	mov	r2,a
   0431 90 E6 79           1015 	mov	dptr,#_I2DAT
   0434 F0                 1016 	movx	@dptr,a
                           1017 ;src/lib/i2c.c:103: while ((I2CS & bmDONE) == 0)
   0435                    1018 00104$:
                           1019 ;     genAssign
   0435 90 E6 78           1020 	mov	dptr,#_I2CS
   0438 E0                 1021 	movx	a,@dptr
   0439 FA                 1022 	mov	r2,a
                           1023 ;     genAnd
   043A 53 02 01           1024 	anl	ar2,#0x01
                           1025 ;     genCmpEq
   043D BA 00 02           1026 	cjne	r2,#0x00,00134$
                           1027 ;	Peephole 112.b	changed ljmp to sjmp
   0440 80 F3              1028 	sjmp	00104$
   0442                    1029 00134$:
                           1030 ;src/lib/i2c.c:106: if ((I2CS & bmBERR) || (I2CS & bmACK) == 0)	// no device answered...
                           1031 ;     genAssign
   0442 90 E6 78           1032 	mov	dptr,#_I2CS
   0445 E0                 1033 	movx	a,@dptr
                           1034 ;     genAnd
                           1035 ;	Peephole 105	removed redundant mov
   0446 FA                 1036 	mov	r2,a
                           1037 ;     genIfxJump
                           1038 ;	Peephole 112.a	removed ljmp by inverse jump logic
   0447 20 E2 57           1039 	jb	acc.2,00119$
   044A                    1040 00135$:
                           1041 ;     genAssign
   044A 90 E6 78           1042 	mov	dptr,#_I2CS
   044D E0                 1043 	movx	a,@dptr
   044E FA                 1044 	mov	r2,a
                           1045 ;     genAnd
   044F 53 02 02           1046 	anl	ar2,#0x02
                           1047 ;     genCmpEq
   0452 BA 00 02           1048 	cjne	r2,#0x00,00136$
                           1049 ;	Peephole 112.b	changed ljmp to sjmp
   0455 80 4A              1050 	sjmp	00119$
   0457                    1051 00136$:
                           1052 ;src/lib/i2c.c:109: while (len > 0){
                           1053 ;     genAssign
   0457 AA 2B              1054 	mov	r2,_i2c_write_PARM_2
   0459 AB 2C              1055 	mov	r3,(_i2c_write_PARM_2 + 1)
                           1056 ;     genAssign
   045B AC 2D              1057 	mov	r4,_i2c_write_PARM_3
   045D                    1058 00116$:
                           1059 ;     genCmpGt
                           1060 ;     genCmp
                           1061 ;     genIfxJump
                           1062 ;	Peephole 108	removed ljmp by inverse jump logic
                           1063 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   045D EC                 1064 	mov	a,r4
   045E 24 FF              1065 	add	a,#0xff - 0x00
   0460 50 33              1066 	jnc	00118$
   0462                    1067 00137$:
                           1068 ;src/lib/i2c.c:110: I2DAT = *buf++;
                           1069 ;     genPointerGet
                           1070 ;     genFarPointerGet
   0462 8A 82              1071 	mov	dpl,r2
   0464 8B 83              1072 	mov	dph,r3
   0466 E0                 1073 	movx	a,@dptr
   0467 FD                 1074 	mov	r5,a
   0468 A3                 1075 	inc	dptr
   0469 AA 82              1076 	mov	r2,dpl
   046B AB 83              1077 	mov	r3,dph
                           1078 ;     genAssign
   046D 90 E6 79           1079 	mov	dptr,#_I2DAT
   0470 ED                 1080 	mov	a,r5
   0471 F0                 1081 	movx	@dptr,a
                           1082 ;src/lib/i2c.c:111: len--;
                           1083 ;     genMinus
                           1084 ;     genMinusDec
   0472 1C                 1085 	dec	r4
                           1086 ;src/lib/i2c.c:113: while ((I2CS & bmDONE) == 0)
   0473                    1087 00110$:
                           1088 ;     genAssign
   0473 90 E6 78           1089 	mov	dptr,#_I2CS
   0476 E0                 1090 	movx	a,@dptr
   0477 FD                 1091 	mov	r5,a
                           1092 ;     genAnd
   0478 53 05 01           1093 	anl	ar5,#0x01
                           1094 ;     genCmpEq
   047B BD 00 02           1095 	cjne	r5,#0x00,00138$
                           1096 ;	Peephole 112.b	changed ljmp to sjmp
   047E 80 F3              1097 	sjmp	00110$
   0480                    1098 00138$:
                           1099 ;src/lib/i2c.c:116: if ((I2CS & bmBERR) || (I2CS & bmACK) == 0)	// no device answered...
                           1100 ;     genAssign
   0480 90 E6 78           1101 	mov	dptr,#_I2CS
   0483 E0                 1102 	movx	a,@dptr
                           1103 ;     genAnd
                           1104 ;	Peephole 105	removed redundant mov
   0484 FD                 1105 	mov	r5,a
                           1106 ;     genIfxJump
                           1107 ;	Peephole 112.a	removed ljmp by inverse jump logic
   0485 20 E2 19           1108 	jb	acc.2,00119$
   0488                    1109 00139$:
                           1110 ;     genAssign
   0488 90 E6 78           1111 	mov	dptr,#_I2CS
   048B E0                 1112 	movx	a,@dptr
   048C FD                 1113 	mov	r5,a
                           1114 ;     genAnd
   048D 53 05 02           1115 	anl	ar5,#0x02
                           1116 ;     genCmpEq
                           1117 ;	Peephole 112.b	changed ljmp to sjmp
                           1118 ;	Peephole 199	optimized misc jump sequence
   0490 BD 00 CA           1119 	cjne	r5,#0x00,00116$
                           1120 ;00140$:
                           1121 ;	Peephole 200	removed redundant sjmp
   0493                    1122 00141$:
                           1123 ;src/lib/i2c.c:117: goto fail;
                           1124 ;	Peephole 112.b	changed ljmp to sjmp
   0493 80 0C              1125 	sjmp	00119$
   0495                    1126 00118$:
                           1127 ;src/lib/i2c.c:120: I2CS |= bmSTOP;
                           1128 ;     genAssign
                           1129 ;     genOr
                           1130 ;	Peephole 248.a	optimized or to xdata
   0495 90 E6 78           1131 	mov	dptr,#_I2CS
   0498 E0                 1132 	movx	a,@dptr
   0499 FA                 1133 	mov	r2,a
   049A 44 40              1134 	orl	a,#0x40
   049C F0                 1135 	movx	@dptr,a
                           1136 ;src/lib/i2c.c:121: return 1;
                           1137 ;     genRet
   049D 75 82 01           1138 	mov	dpl,#0x01
                           1139 ;src/lib/i2c.c:123: fail:
                           1140 ;	Peephole 112.b	changed ljmp to sjmp
                           1141 ;	Peephole 251.b	replaced sjmp to ret with ret
   04A0 22                 1142 	ret
   04A1                    1143 00119$:
                           1144 ;src/lib/i2c.c:124: I2CS |= bmSTOP;
                           1145 ;     genAssign
                           1146 ;     genOr
                           1147 ;	Peephole 248.a	optimized or to xdata
   04A1 90 E6 78           1148 	mov	dptr,#_I2CS
   04A4 E0                 1149 	movx	a,@dptr
   04A5 FA                 1150 	mov	r2,a
   04A6 44 40              1151 	orl	a,#0x40
   04A8 F0                 1152 	movx	@dptr,a
                           1153 ;src/lib/i2c.c:125: return 0;
                           1154 ;     genRet
   04A9 75 82 00           1155 	mov	dpl,#0x00
   04AC                    1156 00120$:
   04AC 22                 1157 	ret
                           1158 	.area CSEG    (CODE)
