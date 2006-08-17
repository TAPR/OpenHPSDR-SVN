                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Thu Aug 17 13:31:55 2006
                              5 ;--------------------------------------------------------
                              6 	.module isr
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _hook_fgv_PARM_2
                             13 	.globl _hook_uv_PARM_2
                             14 	.globl _hook_sv_PARM_2
                             15 	.globl _EIPX6
                             16 	.globl _EIPX5
                             17 	.globl _EIPX4
                             18 	.globl _PI2C
                             19 	.globl _PUSB
                             20 	.globl _EIEX6
                             21 	.globl _EIEX5
                             22 	.globl _EIEX4
                             23 	.globl _EI2C
                             24 	.globl _EIUSB
                             25 	.globl _SMOD1
                             26 	.globl _ERESI
                             27 	.globl _RESI
                             28 	.globl _INT6
                             29 	.globl _CY
                             30 	.globl _AC
                             31 	.globl _F0
                             32 	.globl _RS1
                             33 	.globl _RS0
                             34 	.globl _OV
                             35 	.globl _FL
                             36 	.globl _P
                             37 	.globl _TF2
                             38 	.globl _EXF2
                             39 	.globl _RCLK
                             40 	.globl _TCLK
                             41 	.globl _EXEN2
                             42 	.globl _TR2
                             43 	.globl _C_T2
                             44 	.globl _CP_RL2
                             45 	.globl _SM01
                             46 	.globl _SM11
                             47 	.globl _SM21
                             48 	.globl _REN1
                             49 	.globl _TB81
                             50 	.globl _RB81
                             51 	.globl _TI1
                             52 	.globl _RI1
                             53 	.globl _PS1
                             54 	.globl _PT2
                             55 	.globl _PS0
                             56 	.globl _PT1
                             57 	.globl _PX1
                             58 	.globl _PT0
                             59 	.globl _PX0
                             60 	.globl _EA
                             61 	.globl _ES1
                             62 	.globl _ET2
                             63 	.globl _ES0
                             64 	.globl _ET1
                             65 	.globl _EX1
                             66 	.globl _ET0
                             67 	.globl _EX0
                             68 	.globl _SM0
                             69 	.globl _SM1
                             70 	.globl _SM2
                             71 	.globl _REN
                             72 	.globl _TB8
                             73 	.globl _RB8
                             74 	.globl _TI
                             75 	.globl _RI
                             76 	.globl _TF1
                             77 	.globl _TR1
                             78 	.globl _TF0
                             79 	.globl _TR0
                             80 	.globl _IE1
                             81 	.globl _IT1
                             82 	.globl _IE0
                             83 	.globl _IT0
                             84 	.globl _SEL
                             85 	.globl _EIP
                             86 	.globl _B
                             87 	.globl _EIE
                             88 	.globl _ACC
                             89 	.globl _EICON
                             90 	.globl _PSW
                             91 	.globl _TH2
                             92 	.globl _TL2
                             93 	.globl _RCAP2H
                             94 	.globl _RCAP2L
                             95 	.globl _T2CON
                             96 	.globl _SBUF1
                             97 	.globl _SCON1
                             98 	.globl _GPIFSGLDATLNOX
                             99 	.globl _GPIFSGLDATLX
                            100 	.globl _GPIFSGLDATH
                            101 	.globl _GPIFTRIG
                            102 	.globl _EP01STAT
                            103 	.globl _IP
                            104 	.globl _OEE
                            105 	.globl _OED
                            106 	.globl _OEC
                            107 	.globl _OEB
                            108 	.globl _OEA
                            109 	.globl _IOE
                            110 	.globl _IOD
                            111 	.globl _AUTOPTRSETUP
                            112 	.globl _EP68FIFOFLGS
                            113 	.globl _EP24FIFOFLGS
                            114 	.globl _EP2468STAT
                            115 	.globl _IE
                            116 	.globl _INT4CLR
                            117 	.globl _INT2CLR
                            118 	.globl _IOC
                            119 	.globl _AUTODAT2
                            120 	.globl _AUTOPTRL2
                            121 	.globl _AUTOPTRH2
                            122 	.globl _AUTODAT1
                            123 	.globl _APTR1L
                            124 	.globl _APTR1H
                            125 	.globl _SBUF0
                            126 	.globl _SCON0
                            127 	.globl _MPAGE
                            128 	.globl _EXIF
                            129 	.globl _IOB
                            130 	.globl _CKCON
                            131 	.globl _TH1
                            132 	.globl _TH0
                            133 	.globl _TL1
                            134 	.globl _TL0
                            135 	.globl _TMOD
                            136 	.globl _TCON
                            137 	.globl _PCON
                            138 	.globl _DPS
                            139 	.globl _DPH1
                            140 	.globl _DPL1
                            141 	.globl _DPH
                            142 	.globl _DPL
                            143 	.globl _SP
                            144 	.globl _IOA
                            145 	.globl _EP8FIFOBUF
                            146 	.globl _EP6FIFOBUF
                            147 	.globl _EP4FIFOBUF
                            148 	.globl _EP2FIFOBUF
                            149 	.globl _EP1INBUF
                            150 	.globl _EP1OUTBUF
                            151 	.globl _EP0BUF
                            152 	.globl _CT4
                            153 	.globl _CT3
                            154 	.globl _CT2
                            155 	.globl _CT1
                            156 	.globl _USBTEST
                            157 	.globl _TESTCFG
                            158 	.globl _DBUG
                            159 	.globl _UDMACRCQUAL
                            160 	.globl _UDMACRCL
                            161 	.globl _UDMACRCH
                            162 	.globl _GPIFHOLDAMOUNT
                            163 	.globl _FLOWSTBHPERIOD
                            164 	.globl _FLOWSTBEDGE
                            165 	.globl _FLOWSTB
                            166 	.globl _FLOWHOLDOFF
                            167 	.globl _FLOWEQ1CTL
                            168 	.globl _FLOWEQ0CTL
                            169 	.globl _FLOWLOGIC
                            170 	.globl _FLOWSTATE
                            171 	.globl _GPIFABORT
                            172 	.globl _GPIFREADYSTAT
                            173 	.globl _GPIFREADYCFG
                            174 	.globl _XGPIFSGLDATLNOX
                            175 	.globl _XGPIFSGLDATLX
                            176 	.globl _XGPIFSGLDATH
                            177 	.globl _EP8GPIFTRIG
                            178 	.globl _EP8GPIFPFSTOP
                            179 	.globl _EP8GPIFFLGSEL
                            180 	.globl _EP6GPIFTRIG
                            181 	.globl _EP6GPIFPFSTOP
                            182 	.globl _EP6GPIFFLGSEL
                            183 	.globl _EP4GPIFTRIG
                            184 	.globl _EP4GPIFPFSTOP
                            185 	.globl _EP4GPIFFLGSEL
                            186 	.globl _EP2GPIFTRIG
                            187 	.globl _EP2GPIFPFSTOP
                            188 	.globl _EP2GPIFFLGSEL
                            189 	.globl _GPIFTCB0
                            190 	.globl _GPIFTCB1
                            191 	.globl _GPIFTCB2
                            192 	.globl _GPIFTCB3
                            193 	.globl _GPIFADRL
                            194 	.globl _GPIFADRH
                            195 	.globl _GPIFCTLCFG
                            196 	.globl _GPIFIDLECTL
                            197 	.globl _GPIFIDLECS
                            198 	.globl _GPIFWFSELECT
                            199 	.globl _SETUPDAT
                            200 	.globl _SUDPTRCTL
                            201 	.globl _SUDPTRL
                            202 	.globl _SUDPTRH
                            203 	.globl _EP8FIFOBCL
                            204 	.globl _EP8FIFOBCH
                            205 	.globl _EP6FIFOBCL
                            206 	.globl _EP6FIFOBCH
                            207 	.globl _EP4FIFOBCL
                            208 	.globl _EP4FIFOBCH
                            209 	.globl _EP2FIFOBCL
                            210 	.globl _EP2FIFOBCH
                            211 	.globl _EP8FIFOFLGS
                            212 	.globl _EP6FIFOFLGS
                            213 	.globl _EP4FIFOFLGS
                            214 	.globl _EP2FIFOFLGS
                            215 	.globl _EP8CS
                            216 	.globl _EP6CS
                            217 	.globl _EP4CS
                            218 	.globl _EP2CS
                            219 	.globl _EP1INCS
                            220 	.globl _EP1OUTCS
                            221 	.globl _EP0CS
                            222 	.globl _EP8BCL
                            223 	.globl _EP8BCH
                            224 	.globl _EP6BCL
                            225 	.globl _EP6BCH
                            226 	.globl _EP4BCL
                            227 	.globl _EP4BCH
                            228 	.globl _EP2BCL
                            229 	.globl _EP2BCH
                            230 	.globl _EP1INBC
                            231 	.globl _EP1OUTBC
                            232 	.globl _EP0BCL
                            233 	.globl _EP0BCH
                            234 	.globl _FNADDR
                            235 	.globl _MICROFRAME
                            236 	.globl _USBFRAMEL
                            237 	.globl _USBFRAMEH
                            238 	.globl _TOGCTL
                            239 	.globl _WAKEUPCS
                            240 	.globl _SUSPEND
                            241 	.globl _USBCS
                            242 	.globl _XAUTODAT2
                            243 	.globl _XAUTODAT1
                            244 	.globl _I2CTL
                            245 	.globl _I2DAT
                            246 	.globl _I2CS
                            247 	.globl _PORTECFG
                            248 	.globl _PORTCCFG
                            249 	.globl _PORTACFG
                            250 	.globl _INTSETUP
                            251 	.globl _INT4IVEC
                            252 	.globl _INT2IVEC
                            253 	.globl _CLRERRCNT
                            254 	.globl _ERRCNTLIM
                            255 	.globl _USBERRIRQ
                            256 	.globl _USBERRIE
                            257 	.globl _GPIFIRQ
                            258 	.globl _GPIFIE
                            259 	.globl _EPIRQ
                            260 	.globl _EPIE
                            261 	.globl _USBIRQ
                            262 	.globl _USBIE
                            263 	.globl _NAKIRQ
                            264 	.globl _NAKIE
                            265 	.globl _IBNIRQ
                            266 	.globl _IBNIE
                            267 	.globl _EP8FIFOIRQ
                            268 	.globl _EP8FIFOIE
                            269 	.globl _EP6FIFOIRQ
                            270 	.globl _EP6FIFOIE
                            271 	.globl _EP4FIFOIRQ
                            272 	.globl _EP4FIFOIE
                            273 	.globl _EP2FIFOIRQ
                            274 	.globl _EP2FIFOIE
                            275 	.globl _OUTPKTEND
                            276 	.globl _INPKTEND
                            277 	.globl _EP8ISOINPKTS
                            278 	.globl _EP6ISOINPKTS
                            279 	.globl _EP4ISOINPKTS
                            280 	.globl _EP2ISOINPKTS
                            281 	.globl _EP8FIFOPFL
                            282 	.globl _EP8FIFOPFH
                            283 	.globl _EP6FIFOPFL
                            284 	.globl _EP6FIFOPFH
                            285 	.globl _EP4FIFOPFL
                            286 	.globl _EP4FIFOPFH
                            287 	.globl _EP2FIFOPFL
                            288 	.globl _EP2FIFOPFH
                            289 	.globl _EP8AUTOINLENL
                            290 	.globl _EP8AUTOINLENH
                            291 	.globl _EP6AUTOINLENL
                            292 	.globl _EP6AUTOINLENH
                            293 	.globl _EP4AUTOINLENL
                            294 	.globl _EP4AUTOINLENH
                            295 	.globl _EP2AUTOINLENL
                            296 	.globl _EP2AUTOINLENH
                            297 	.globl _EP8FIFOCFG
                            298 	.globl _EP6FIFOCFG
                            299 	.globl _EP4FIFOCFG
                            300 	.globl _EP2FIFOCFG
                            301 	.globl _EP8CFG
                            302 	.globl _EP6CFG
                            303 	.globl _EP4CFG
                            304 	.globl _EP2CFG
                            305 	.globl _EP1INCFG
                            306 	.globl _EP1OUTCFG
                            307 	.globl _REVCTL
                            308 	.globl _REVID
                            309 	.globl _FIFOPINPOLAR
                            310 	.globl _UART230
                            311 	.globl _BPADDRL
                            312 	.globl _BPADDRH
                            313 	.globl _BREAKPT
                            314 	.globl _FIFORESET
                            315 	.globl _PINFLAGSCD
                            316 	.globl _PINFLAGSAB
                            317 	.globl _IFCONFIG
                            318 	.globl _CPUCS
                            319 	.globl _RES_WAVEDATA_END
                            320 	.globl _GPIF_WAVE_DATA
                            321 	.globl _hook_sv
                            322 	.globl _hook_uv
                            323 	.globl _hook_fgv
                            324 	.globl _setup_autovectors
                            325 ;--------------------------------------------------------
                            326 ; special function registers
                            327 ;--------------------------------------------------------
                            328 	.area RSEG    (DATA)
                    0080    329 _IOA	=	0x0080
                    0081    330 _SP	=	0x0081
                    0082    331 _DPL	=	0x0082
                    0083    332 _DPH	=	0x0083
                    0084    333 _DPL1	=	0x0084
                    0085    334 _DPH1	=	0x0085
                    0086    335 _DPS	=	0x0086
                    0087    336 _PCON	=	0x0087
                    0088    337 _TCON	=	0x0088
                    0089    338 _TMOD	=	0x0089
                    008A    339 _TL0	=	0x008a
                    008B    340 _TL1	=	0x008b
                    008C    341 _TH0	=	0x008c
                    008D    342 _TH1	=	0x008d
                    008E    343 _CKCON	=	0x008e
                    0090    344 _IOB	=	0x0090
                    0091    345 _EXIF	=	0x0091
                    0092    346 _MPAGE	=	0x0092
                    0098    347 _SCON0	=	0x0098
                    0099    348 _SBUF0	=	0x0099
                    009A    349 _APTR1H	=	0x009a
                    009B    350 _APTR1L	=	0x009b
                    009C    351 _AUTODAT1	=	0x009c
                    009D    352 _AUTOPTRH2	=	0x009d
                    009E    353 _AUTOPTRL2	=	0x009e
                    009F    354 _AUTODAT2	=	0x009f
                    00A0    355 _IOC	=	0x00a0
                    00A1    356 _INT2CLR	=	0x00a1
                    00A2    357 _INT4CLR	=	0x00a2
                    00A8    358 _IE	=	0x00a8
                    00AA    359 _EP2468STAT	=	0x00aa
                    00AB    360 _EP24FIFOFLGS	=	0x00ab
                    00AC    361 _EP68FIFOFLGS	=	0x00ac
                    00AF    362 _AUTOPTRSETUP	=	0x00af
                    00B0    363 _IOD	=	0x00b0
                    00B1    364 _IOE	=	0x00b1
                    00B2    365 _OEA	=	0x00b2
                    00B3    366 _OEB	=	0x00b3
                    00B4    367 _OEC	=	0x00b4
                    00B5    368 _OED	=	0x00b5
                    00B6    369 _OEE	=	0x00b6
                    00B8    370 _IP	=	0x00b8
                    00BA    371 _EP01STAT	=	0x00ba
                    00BB    372 _GPIFTRIG	=	0x00bb
                    00BD    373 _GPIFSGLDATH	=	0x00bd
                    00BE    374 _GPIFSGLDATLX	=	0x00be
                    00BF    375 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    376 _SCON1	=	0x00c0
                    00C1    377 _SBUF1	=	0x00c1
                    00C8    378 _T2CON	=	0x00c8
                    00CA    379 _RCAP2L	=	0x00ca
                    00CB    380 _RCAP2H	=	0x00cb
                    00CC    381 _TL2	=	0x00cc
                    00CD    382 _TH2	=	0x00cd
                    00D0    383 _PSW	=	0x00d0
                    00D8    384 _EICON	=	0x00d8
                    00E0    385 _ACC	=	0x00e0
                    00E8    386 _EIE	=	0x00e8
                    00F0    387 _B	=	0x00f0
                    00F8    388 _EIP	=	0x00f8
                            389 ;--------------------------------------------------------
                            390 ; special function bits 
                            391 ;--------------------------------------------------------
                            392 	.area RSEG    (DATA)
                    0086    393 _SEL	=	0x0086
                    0088    394 _IT0	=	0x0088
                    0089    395 _IE0	=	0x0089
                    008A    396 _IT1	=	0x008a
                    008B    397 _IE1	=	0x008b
                    008C    398 _TR0	=	0x008c
                    008D    399 _TF0	=	0x008d
                    008E    400 _TR1	=	0x008e
                    008F    401 _TF1	=	0x008f
                    0098    402 _RI	=	0x0098
                    0099    403 _TI	=	0x0099
                    009A    404 _RB8	=	0x009a
                    009B    405 _TB8	=	0x009b
                    009C    406 _REN	=	0x009c
                    009D    407 _SM2	=	0x009d
                    009E    408 _SM1	=	0x009e
                    009F    409 _SM0	=	0x009f
                    00A8    410 _EX0	=	0x00a8
                    00A9    411 _ET0	=	0x00a9
                    00AA    412 _EX1	=	0x00aa
                    00AB    413 _ET1	=	0x00ab
                    00AC    414 _ES0	=	0x00ac
                    00AD    415 _ET2	=	0x00ad
                    00AE    416 _ES1	=	0x00ae
                    00AF    417 _EA	=	0x00af
                    00B8    418 _PX0	=	0x00b8
                    00B9    419 _PT0	=	0x00b9
                    00BA    420 _PX1	=	0x00ba
                    00BB    421 _PT1	=	0x00bb
                    00BC    422 _PS0	=	0x00bc
                    00BD    423 _PT2	=	0x00bd
                    00BE    424 _PS1	=	0x00be
                    00C0    425 _RI1	=	0x00c0
                    00C1    426 _TI1	=	0x00c1
                    00C2    427 _RB81	=	0x00c2
                    00C3    428 _TB81	=	0x00c3
                    00C4    429 _REN1	=	0x00c4
                    00C5    430 _SM21	=	0x00c5
                    00C6    431 _SM11	=	0x00c6
                    00C7    432 _SM01	=	0x00c7
                    00C8    433 _CP_RL2	=	0x00c8
                    00C9    434 _C_T2	=	0x00c9
                    00CA    435 _TR2	=	0x00ca
                    00CB    436 _EXEN2	=	0x00cb
                    00CC    437 _TCLK	=	0x00cc
                    00CD    438 _RCLK	=	0x00cd
                    00CE    439 _EXF2	=	0x00ce
                    00CF    440 _TF2	=	0x00cf
                    00D0    441 _P	=	0x00d0
                    00D1    442 _FL	=	0x00d1
                    00D2    443 _OV	=	0x00d2
                    00D3    444 _RS0	=	0x00d3
                    00D4    445 _RS1	=	0x00d4
                    00D5    446 _F0	=	0x00d5
                    00D6    447 _AC	=	0x00d6
                    00D7    448 _CY	=	0x00d7
                    00DB    449 _INT6	=	0x00db
                    00DC    450 _RESI	=	0x00dc
                    00DD    451 _ERESI	=	0x00dd
                    00DF    452 _SMOD1	=	0x00df
                    00E8    453 _EIUSB	=	0x00e8
                    00E9    454 _EI2C	=	0x00e9
                    00EA    455 _EIEX4	=	0x00ea
                    00EB    456 _EIEX5	=	0x00eb
                    00EC    457 _EIEX6	=	0x00ec
                    00F8    458 _PUSB	=	0x00f8
                    00F9    459 _PI2C	=	0x00f9
                    00FA    460 _EIPX4	=	0x00fa
                    00FB    461 _EIPX5	=	0x00fb
                    00FC    462 _EIPX6	=	0x00fc
                            463 ;--------------------------------------------------------
                            464 ; overlayable register banks 
                            465 ;--------------------------------------------------------
                            466 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     467 	.ds 8
                            468 ;--------------------------------------------------------
                            469 ; internal ram data
                            470 ;--------------------------------------------------------
                            471 	.area DSEG    (DATA)
                            472 ;--------------------------------------------------------
                            473 ; overlayable items in internal ram 
                            474 ;--------------------------------------------------------
                            475 	.area	OSEG    (OVR,DATA)
   002B                     476 _hook_sv_PARM_2::
   002B                     477 	.ds 2
                            478 	.area	OSEG    (OVR,DATA)
   002B                     479 _hook_uv_PARM_2::
   002B                     480 	.ds 2
                            481 	.area	OSEG    (OVR,DATA)
   002B                     482 _hook_fgv_PARM_2::
   002B                     483 	.ds 2
                            484 ;--------------------------------------------------------
                            485 ; indirectly addressable internal ram data
                            486 ;--------------------------------------------------------
                            487 	.area ISEG    (DATA)
                            488 ;--------------------------------------------------------
                            489 ; bit data
                            490 ;--------------------------------------------------------
                            491 	.area BSEG    (BIT)
   0000                     492 _hook_sv_t_1_1::
   0000                     493 	.ds 1
   0001                     494 _hook_uv_t_1_1::
   0001                     495 	.ds 1
   0002                     496 _hook_fgv_t_1_1::
   0002                     497 	.ds 1
                            498 ;--------------------------------------------------------
                            499 ; paged external ram data
                            500 ;--------------------------------------------------------
                            501 	.area PSEG    (PAG,XDATA)
                            502 ;--------------------------------------------------------
                            503 ; external ram data
                            504 ;--------------------------------------------------------
                            505 	.area XSEG    (XDATA)
                    E400    506 _GPIF_WAVE_DATA	=	0xe400
                    E480    507 _RES_WAVEDATA_END	=	0xe480
                    E600    508 _CPUCS	=	0xe600
                    E601    509 _IFCONFIG	=	0xe601
                    E602    510 _PINFLAGSAB	=	0xe602
                    E603    511 _PINFLAGSCD	=	0xe603
                    E604    512 _FIFORESET	=	0xe604
                    E605    513 _BREAKPT	=	0xe605
                    E606    514 _BPADDRH	=	0xe606
                    E607    515 _BPADDRL	=	0xe607
                    E608    516 _UART230	=	0xe608
                    E609    517 _FIFOPINPOLAR	=	0xe609
                    E60A    518 _REVID	=	0xe60a
                    E60B    519 _REVCTL	=	0xe60b
                    E610    520 _EP1OUTCFG	=	0xe610
                    E611    521 _EP1INCFG	=	0xe611
                    E612    522 _EP2CFG	=	0xe612
                    E613    523 _EP4CFG	=	0xe613
                    E614    524 _EP6CFG	=	0xe614
                    E615    525 _EP8CFG	=	0xe615
                    E618    526 _EP2FIFOCFG	=	0xe618
                    E619    527 _EP4FIFOCFG	=	0xe619
                    E61A    528 _EP6FIFOCFG	=	0xe61a
                    E61B    529 _EP8FIFOCFG	=	0xe61b
                    E620    530 _EP2AUTOINLENH	=	0xe620
                    E621    531 _EP2AUTOINLENL	=	0xe621
                    E622    532 _EP4AUTOINLENH	=	0xe622
                    E623    533 _EP4AUTOINLENL	=	0xe623
                    E624    534 _EP6AUTOINLENH	=	0xe624
                    E625    535 _EP6AUTOINLENL	=	0xe625
                    E626    536 _EP8AUTOINLENH	=	0xe626
                    E627    537 _EP8AUTOINLENL	=	0xe627
                    E630    538 _EP2FIFOPFH	=	0xe630
                    E631    539 _EP2FIFOPFL	=	0xe631
                    E632    540 _EP4FIFOPFH	=	0xe632
                    E633    541 _EP4FIFOPFL	=	0xe633
                    E634    542 _EP6FIFOPFH	=	0xe634
                    E635    543 _EP6FIFOPFL	=	0xe635
                    E636    544 _EP8FIFOPFH	=	0xe636
                    E637    545 _EP8FIFOPFL	=	0xe637
                    E640    546 _EP2ISOINPKTS	=	0xe640
                    E641    547 _EP4ISOINPKTS	=	0xe641
                    E642    548 _EP6ISOINPKTS	=	0xe642
                    E643    549 _EP8ISOINPKTS	=	0xe643
                    E648    550 _INPKTEND	=	0xe648
                    E649    551 _OUTPKTEND	=	0xe649
                    E650    552 _EP2FIFOIE	=	0xe650
                    E651    553 _EP2FIFOIRQ	=	0xe651
                    E652    554 _EP4FIFOIE	=	0xe652
                    E653    555 _EP4FIFOIRQ	=	0xe653
                    E654    556 _EP6FIFOIE	=	0xe654
                    E655    557 _EP6FIFOIRQ	=	0xe655
                    E656    558 _EP8FIFOIE	=	0xe656
                    E657    559 _EP8FIFOIRQ	=	0xe657
                    E658    560 _IBNIE	=	0xe658
                    E659    561 _IBNIRQ	=	0xe659
                    E65A    562 _NAKIE	=	0xe65a
                    E65B    563 _NAKIRQ	=	0xe65b
                    E65C    564 _USBIE	=	0xe65c
                    E65D    565 _USBIRQ	=	0xe65d
                    E65E    566 _EPIE	=	0xe65e
                    E65F    567 _EPIRQ	=	0xe65f
                    E660    568 _GPIFIE	=	0xe660
                    E661    569 _GPIFIRQ	=	0xe661
                    E662    570 _USBERRIE	=	0xe662
                    E663    571 _USBERRIRQ	=	0xe663
                    E664    572 _ERRCNTLIM	=	0xe664
                    E665    573 _CLRERRCNT	=	0xe665
                    E666    574 _INT2IVEC	=	0xe666
                    E667    575 _INT4IVEC	=	0xe667
                    E668    576 _INTSETUP	=	0xe668
                    E670    577 _PORTACFG	=	0xe670
                    E671    578 _PORTCCFG	=	0xe671
                    E672    579 _PORTECFG	=	0xe672
                    E678    580 _I2CS	=	0xe678
                    E679    581 _I2DAT	=	0xe679
                    E67A    582 _I2CTL	=	0xe67a
                    E67B    583 _XAUTODAT1	=	0xe67b
                    E67C    584 _XAUTODAT2	=	0xe67c
                    E680    585 _USBCS	=	0xe680
                    E681    586 _SUSPEND	=	0xe681
                    E682    587 _WAKEUPCS	=	0xe682
                    E683    588 _TOGCTL	=	0xe683
                    E684    589 _USBFRAMEH	=	0xe684
                    E685    590 _USBFRAMEL	=	0xe685
                    E686    591 _MICROFRAME	=	0xe686
                    E687    592 _FNADDR	=	0xe687
                    E68A    593 _EP0BCH	=	0xe68a
                    E68B    594 _EP0BCL	=	0xe68b
                    E68D    595 _EP1OUTBC	=	0xe68d
                    E68F    596 _EP1INBC	=	0xe68f
                    E690    597 _EP2BCH	=	0xe690
                    E691    598 _EP2BCL	=	0xe691
                    E694    599 _EP4BCH	=	0xe694
                    E695    600 _EP4BCL	=	0xe695
                    E698    601 _EP6BCH	=	0xe698
                    E699    602 _EP6BCL	=	0xe699
                    E69C    603 _EP8BCH	=	0xe69c
                    E69D    604 _EP8BCL	=	0xe69d
                    E6A0    605 _EP0CS	=	0xe6a0
                    E6A1    606 _EP1OUTCS	=	0xe6a1
                    E6A2    607 _EP1INCS	=	0xe6a2
                    E6A3    608 _EP2CS	=	0xe6a3
                    E6A4    609 _EP4CS	=	0xe6a4
                    E6A5    610 _EP6CS	=	0xe6a5
                    E6A6    611 _EP8CS	=	0xe6a6
                    E6A7    612 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    613 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    614 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    615 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    616 _EP2FIFOBCH	=	0xe6ab
                    E6AC    617 _EP2FIFOBCL	=	0xe6ac
                    E6AD    618 _EP4FIFOBCH	=	0xe6ad
                    E6AE    619 _EP4FIFOBCL	=	0xe6ae
                    E6AF    620 _EP6FIFOBCH	=	0xe6af
                    E6B0    621 _EP6FIFOBCL	=	0xe6b0
                    E6B1    622 _EP8FIFOBCH	=	0xe6b1
                    E6B2    623 _EP8FIFOBCL	=	0xe6b2
                    E6B3    624 _SUDPTRH	=	0xe6b3
                    E6B4    625 _SUDPTRL	=	0xe6b4
                    E6B5    626 _SUDPTRCTL	=	0xe6b5
                    E6B8    627 _SETUPDAT	=	0xe6b8
                    E6C0    628 _GPIFWFSELECT	=	0xe6c0
                    E6C1    629 _GPIFIDLECS	=	0xe6c1
                    E6C2    630 _GPIFIDLECTL	=	0xe6c2
                    E6C3    631 _GPIFCTLCFG	=	0xe6c3
                    E6C4    632 _GPIFADRH	=	0xe6c4
                    E6C5    633 _GPIFADRL	=	0xe6c5
                    E6CE    634 _GPIFTCB3	=	0xe6ce
                    E6CF    635 _GPIFTCB2	=	0xe6cf
                    E6D0    636 _GPIFTCB1	=	0xe6d0
                    E6D1    637 _GPIFTCB0	=	0xe6d1
                    E6D2    638 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    639 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    640 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    641 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    642 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    643 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    644 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    645 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    646 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    647 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    648 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    649 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    650 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    651 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    652 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    653 _GPIFREADYCFG	=	0xe6f3
                    E6F4    654 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    655 _GPIFABORT	=	0xe6f5
                    E6C6    656 _FLOWSTATE	=	0xe6c6
                    E6C7    657 _FLOWLOGIC	=	0xe6c7
                    E6C8    658 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    659 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    660 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    661 _FLOWSTB	=	0xe6cb
                    E6CC    662 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    663 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    664 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    665 _UDMACRCH	=	0xe67d
                    E67E    666 _UDMACRCL	=	0xe67e
                    E67F    667 _UDMACRCQUAL	=	0xe67f
                    E6F8    668 _DBUG	=	0xe6f8
                    E6F9    669 _TESTCFG	=	0xe6f9
                    E6FA    670 _USBTEST	=	0xe6fa
                    E6FB    671 _CT1	=	0xe6fb
                    E6FC    672 _CT2	=	0xe6fc
                    E6FD    673 _CT3	=	0xe6fd
                    E6FE    674 _CT4	=	0xe6fe
                    E740    675 _EP0BUF	=	0xe740
                    E780    676 _EP1OUTBUF	=	0xe780
                    E7C0    677 _EP1INBUF	=	0xe7c0
                    F000    678 _EP2FIFOBUF	=	0xf000
                    F400    679 _EP4FIFOBUF	=	0xf400
                    F800    680 _EP6FIFOBUF	=	0xf800
                    FC00    681 _EP8FIFOBUF	=	0xfc00
                            682 ;--------------------------------------------------------
                            683 ; external initialized ram data
                            684 ;--------------------------------------------------------
                            685 	.area CSEG    (CODE)
                            686 	.area GSINIT0 (CODE)
                            687 	.area GSINIT1 (CODE)
                            688 	.area GSINIT2 (CODE)
                            689 	.area GSINIT3 (CODE)
                            690 	.area GSINIT4 (CODE)
                            691 	.area GSINIT5 (CODE)
                            692 ;--------------------------------------------------------
                            693 ; global & static initialisations
                            694 ;--------------------------------------------------------
                            695 	.area CSEG    (CODE)
                            696 	.area GSINIT  (CODE)
                            697 	.area GSFINAL (CODE)
                            698 	.area GSINIT  (CODE)
                            699 ;--------------------------------------------------------
                            700 ; Home
                            701 ;--------------------------------------------------------
                            702 	.area HOME    (CODE)
                            703 	.area CSEG    (CODE)
                            704 ;--------------------------------------------------------
                            705 ; code
                            706 ;--------------------------------------------------------
                            707 	.area CSEG    (CODE)
                            708 ;------------------------------------------------------------
                            709 ;Allocation info for local variables in function 'hook_sv'
                            710 ;------------------------------------------------------------
                            711 ;addr                      Allocated with name '_hook_sv_PARM_2'
                            712 ;vector_number             Allocated to registers r2 
                            713 ;------------------------------------------------------------
                            714 ;Initial/src/lib/isr.c:43: hook_sv (unsigned char vector_number, unsigned short addr)
                            715 ;	-----------------------------------------
                            716 ;	 function hook_sv
                            717 ;	-----------------------------------------
   082E                     718 _hook_sv:
                    0002    719 	ar2 = 0x02
                    0003    720 	ar3 = 0x03
                    0004    721 	ar4 = 0x04
                    0005    722 	ar5 = 0x05
                    0006    723 	ar6 = 0x06
                    0007    724 	ar7 = 0x07
                    0000    725 	ar0 = 0x00
                    0001    726 	ar1 = 0x01
                            727 ;     genReceive
   082E AA 82               728 	mov	r2,dpl
                            729 ;Initial/src/lib/isr.c:49: if (vector_number < SV_MIN || vector_number > SV_MAX)
                            730 ;     genCmpLt
                            731 ;     genCmp
   0830 BA 03 00            732 	cjne	r2,#0x03,00112$
   0833                     733 00112$:
                            734 ;     genIfxJump
                            735 ;	Peephole 112.b	changed ljmp to sjmp
                            736 ;	Peephole 160	removed sjmp by inverse jump logic
   0833 40 05               737 	jc	00101$
   0835                     738 00113$:
                            739 ;     genCmpGt
                            740 ;     genCmp
                            741 ;     genIfxJump
                            742 ;	Peephole 108	removed ljmp by inverse jump logic
                            743 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   0835 EA                  744 	mov	a,r2
   0836 24 9C               745 	add	a,#0xff - 0x63
   0838 50 01               746 	jnc	00102$
   083A                     747 00114$:
   083A                     748 00101$:
                            749 ;Initial/src/lib/isr.c:50: return;
                            750 ;     genRet
                            751 ;	Peephole 112.b	changed ljmp to sjmp
                            752 ;	Peephole 251.b	replaced sjmp to ret with ret
   083A 22                  753 	ret
   083B                     754 00102$:
                            755 ;Initial/src/lib/isr.c:52: if ((vector_number & 0x0f) != 0x03 && (vector_number & 0x0f) != 0x0b)
                            756 ;     genAnd
   083B 74 0F               757 	mov	a,#0x0F
   083D 5A                  758 	anl	a,r2
   083E FB                  759 	mov	r3,a
                            760 ;     genCmpEq
   083F BB 03 02            761 	cjne	r3,#0x03,00115$
                            762 ;	Peephole 112.b	changed ljmp to sjmp
   0842 80 07               763 	sjmp	00105$
   0844                     764 00115$:
                            765 ;     genAnd
   0844 74 0F               766 	mov	a,#0x0F
   0846 5A                  767 	anl	a,r2
   0847 FB                  768 	mov	r3,a
                            769 ;     genCmpEq
                            770 ;	Peephole 112.b	changed ljmp to sjmp
                            771 ;Initial/src/lib/isr.c:53: return;
                            772 ;     genRet
                            773 ;	Peephole 112.b	changed ljmp to sjmp
                            774 ;	Peephole 199	optimized misc jump sequence
   0848 BB 0B 47            775 	cjne	r3,#0x0B,00107$
                            776 ;00116$:
                            777 ;	Peephole 200	removed redundant sjmp
   084B                     778 00105$:
                            779 ;Initial/src/lib/isr.c:55: t = EA;
                            780 ;     genAssign
   084B A2 AF               781 	mov	c,_EA
   084D 92 00               782 	mov	_hook_sv_t_1_1,c
                            783 ;Initial/src/lib/isr.c:56: EA = 0;
                            784 ;     genAssign
   084F C2 AF               785 	clr	_EA
                            786 ;Initial/src/lib/isr.c:57: _standard_interrupt_vector[vector_number] = LJMP_OPCODE;
                            787 ;     genPlus
                            788 ;	Peephole 236.g	used r2 instead of ar2
   0851 EA                  789 	mov	a,r2
   0852 24 00               790 	add	a,#__standard_interrupt_vector
   0854 F5 82               791 	mov	dpl,a
                            792 ;	Peephole 181	changed mov to clr
   0856 E4                  793 	clr	a
   0857 34 00               794 	addc	a,#(__standard_interrupt_vector >> 8)
   0859 F5 83               795 	mov	dph,a
                            796 ;     genPointerSet
                            797 ;     genFarPointerSet
   085B 74 02               798 	mov	a,#0x02
   085D F0                  799 	movx	@dptr,a
                            800 ;Initial/src/lib/isr.c:58: _standard_interrupt_vector[vector_number + 1] = addr >> 8;
                            801 ;     genCast
   085E 7B 00               802 	mov	r3,#0x00
                            803 ;     genPlus
                            804 ;     genPlusIncr
   0860 74 01               805 	mov	a,#0x01
                            806 ;	Peephole 236.a	used r2 instead of ar2
   0862 2A                  807 	add	a,r2
   0863 FC                  808 	mov	r4,a
                            809 ;	Peephole 181	changed mov to clr
   0864 E4                  810 	clr	a
                            811 ;	Peephole 236.b	used r3 instead of ar3
   0865 3B                  812 	addc	a,r3
   0866 FD                  813 	mov	r5,a
                            814 ;     genPlus
                            815 ;	Peephole 236.g	used r4 instead of ar4
   0867 EC                  816 	mov	a,r4
   0868 24 00               817 	add	a,#__standard_interrupt_vector
   086A F5 82               818 	mov	dpl,a
                            819 ;	Peephole 236.g	used r5 instead of ar5
   086C ED                  820 	mov	a,r5
   086D 34 00               821 	addc	a,#(__standard_interrupt_vector >> 8)
   086F F5 83               822 	mov	dph,a
                            823 ;     genRightShift
                            824 ;     genRightShiftLiteral
                            825 ;     genrshTwo
   0871 AC 2C               826 	mov	r4,(_hook_sv_PARM_2 + 1)
   0873 7D 00               827 	mov	r5,#0x00
                            828 ;     genCast
                            829 ;     genPointerSet
                            830 ;     genFarPointerSet
   0875 EC                  831 	mov	a,r4
   0876 F0                  832 	movx	@dptr,a
                            833 ;Initial/src/lib/isr.c:59: _standard_interrupt_vector[vector_number + 2] = addr & 0xff;
                            834 ;     genPlus
                            835 ;     genPlusIncr
   0877 74 02               836 	mov	a,#0x02
                            837 ;	Peephole 236.a	used r2 instead of ar2
   0879 2A                  838 	add	a,r2
   087A FA                  839 	mov	r2,a
                            840 ;	Peephole 181	changed mov to clr
   087B E4                  841 	clr	a
                            842 ;	Peephole 236.b	used r3 instead of ar3
   087C 3B                  843 	addc	a,r3
   087D FB                  844 	mov	r3,a
                            845 ;     genPlus
                            846 ;	Peephole 236.g	used r2 instead of ar2
   087E EA                  847 	mov	a,r2
   087F 24 00               848 	add	a,#__standard_interrupt_vector
   0881 F5 82               849 	mov	dpl,a
                            850 ;	Peephole 236.g	used r3 instead of ar3
   0883 EB                  851 	mov	a,r3
   0884 34 00               852 	addc	a,#(__standard_interrupt_vector >> 8)
   0886 F5 83               853 	mov	dph,a
                            854 ;     genAnd
   0888 AA 2B               855 	mov	r2,_hook_sv_PARM_2
   088A 7B 00               856 	mov	r3,#0x00
                            857 ;     genCast
                            858 ;     genPointerSet
                            859 ;     genFarPointerSet
   088C EA                  860 	mov	a,r2
   088D F0                  861 	movx	@dptr,a
                            862 ;Initial/src/lib/isr.c:60: EA = t;
                            863 ;     genAssign
   088E A2 00               864 	mov	c,_hook_sv_t_1_1
   0890 92 AF               865 	mov	_EA,c
   0892                     866 00107$:
   0892 22                  867 	ret
                            868 ;------------------------------------------------------------
                            869 ;Allocation info for local variables in function 'hook_uv'
                            870 ;------------------------------------------------------------
                            871 ;addr                      Allocated with name '_hook_uv_PARM_2'
                            872 ;vector_number             Allocated to registers r2 
                            873 ;------------------------------------------------------------
                            874 ;Initial/src/lib/isr.c:70: hook_uv (unsigned char vector_number, unsigned short addr)
                            875 ;	-----------------------------------------
                            876 ;	 function hook_uv
                            877 ;	-----------------------------------------
   0893                     878 _hook_uv:
                            879 ;     genReceive
   0893 AA 82               880 	mov	r2,dpl
                            881 ;Initial/src/lib/isr.c:76: if (vector_number < UV_MIN || vector_number > UV_MAX)
                            882 ;     genCmpLt
                            883 ;     genCmp
   0895 BA 00 00            884 	cjne	r2,#0x00,00110$
   0898                     885 00110$:
                            886 ;     genIfxJump
                            887 ;	Peephole 112.b	changed ljmp to sjmp
                            888 ;	Peephole 160	removed sjmp by inverse jump logic
   0898 40 05               889 	jc	00101$
   089A                     890 00111$:
                            891 ;     genCmpGt
                            892 ;     genCmp
                            893 ;     genIfxJump
                            894 ;	Peephole 108	removed ljmp by inverse jump logic
                            895 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   089A EA                  896 	mov	a,r2
   089B 24 83               897 	add	a,#0xff - 0x7C
   089D 50 01               898 	jnc	00102$
   089F                     899 00112$:
   089F                     900 00101$:
                            901 ;Initial/src/lib/isr.c:77: return;
                            902 ;     genRet
                            903 ;	Peephole 112.b	changed ljmp to sjmp
                            904 ;	Peephole 251.b	replaced sjmp to ret with ret
   089F 22                  905 	ret
   08A0                     906 00102$:
                            907 ;Initial/src/lib/isr.c:79: if ((vector_number & 0x3) != 0)
                            908 ;     genAnd
   08A0 74 03               909 	mov	a,#0x03
   08A2 5A                  910 	anl	a,r2
                            911 ;     genCmpEq
                            912 ;	Peephole 112.b	changed ljmp to sjmp
                            913 ;	Peephole 115.b	jump optimization
   08A3 FB                  914 	mov	r3,a
   08A4 60 01               915 	jz	00105$
   08A6                     916 00113$:
                            917 ;Initial/src/lib/isr.c:80: return;
                            918 ;     genRet
                            919 ;	Peephole 112.b	changed ljmp to sjmp
                            920 ;	Peephole 251.b	replaced sjmp to ret with ret
   08A6 22                  921 	ret
   08A7                     922 00105$:
                            923 ;Initial/src/lib/isr.c:82: t = EA;
                            924 ;     genAssign
   08A7 A2 AF               925 	mov	c,_EA
   08A9 92 01               926 	mov	_hook_uv_t_1_1,c
                            927 ;Initial/src/lib/isr.c:83: EA = 0;
                            928 ;     genAssign
   08AB C2 AF               929 	clr	_EA
                            930 ;Initial/src/lib/isr.c:84: _usb_autovector[vector_number] = LJMP_OPCODE;
                            931 ;     genPlus
                            932 ;	Peephole 236.g	used r2 instead of ar2
   08AD EA                  933 	mov	a,r2
   08AE 24 00               934 	add	a,#__usb_autovector
   08B0 F5 82               935 	mov	dpl,a
                            936 ;	Peephole 181	changed mov to clr
   08B2 E4                  937 	clr	a
   08B3 34 01               938 	addc	a,#(__usb_autovector >> 8)
   08B5 F5 83               939 	mov	dph,a
                            940 ;     genPointerSet
                            941 ;     genFarPointerSet
   08B7 74 02               942 	mov	a,#0x02
   08B9 F0                  943 	movx	@dptr,a
                            944 ;Initial/src/lib/isr.c:85: _usb_autovector[vector_number + 1] = addr >> 8;
                            945 ;     genCast
   08BA 7B 00               946 	mov	r3,#0x00
                            947 ;     genPlus
                            948 ;     genPlusIncr
   08BC 74 01               949 	mov	a,#0x01
                            950 ;	Peephole 236.a	used r2 instead of ar2
   08BE 2A                  951 	add	a,r2
   08BF FC                  952 	mov	r4,a
                            953 ;	Peephole 181	changed mov to clr
   08C0 E4                  954 	clr	a
                            955 ;	Peephole 236.b	used r3 instead of ar3
   08C1 3B                  956 	addc	a,r3
   08C2 FD                  957 	mov	r5,a
                            958 ;     genPlus
                            959 ;	Peephole 236.g	used r4 instead of ar4
   08C3 EC                  960 	mov	a,r4
   08C4 24 00               961 	add	a,#__usb_autovector
   08C6 F5 82               962 	mov	dpl,a
                            963 ;	Peephole 236.g	used r5 instead of ar5
   08C8 ED                  964 	mov	a,r5
   08C9 34 01               965 	addc	a,#(__usb_autovector >> 8)
   08CB F5 83               966 	mov	dph,a
                            967 ;     genRightShift
                            968 ;     genRightShiftLiteral
                            969 ;     genrshTwo
   08CD AC 2C               970 	mov	r4,(_hook_uv_PARM_2 + 1)
   08CF 7D 00               971 	mov	r5,#0x00
                            972 ;     genCast
                            973 ;     genPointerSet
                            974 ;     genFarPointerSet
   08D1 EC                  975 	mov	a,r4
   08D2 F0                  976 	movx	@dptr,a
                            977 ;Initial/src/lib/isr.c:86: _usb_autovector[vector_number + 2] = addr & 0xff;
                            978 ;     genPlus
                            979 ;     genPlusIncr
   08D3 74 02               980 	mov	a,#0x02
                            981 ;	Peephole 236.a	used r2 instead of ar2
   08D5 2A                  982 	add	a,r2
   08D6 FA                  983 	mov	r2,a
                            984 ;	Peephole 181	changed mov to clr
   08D7 E4                  985 	clr	a
                            986 ;	Peephole 236.b	used r3 instead of ar3
   08D8 3B                  987 	addc	a,r3
   08D9 FB                  988 	mov	r3,a
                            989 ;     genPlus
                            990 ;	Peephole 236.g	used r2 instead of ar2
   08DA EA                  991 	mov	a,r2
   08DB 24 00               992 	add	a,#__usb_autovector
   08DD F5 82               993 	mov	dpl,a
                            994 ;	Peephole 236.g	used r3 instead of ar3
   08DF EB                  995 	mov	a,r3
   08E0 34 01               996 	addc	a,#(__usb_autovector >> 8)
   08E2 F5 83               997 	mov	dph,a
                            998 ;     genAnd
   08E4 AA 2B               999 	mov	r2,_hook_uv_PARM_2
   08E6 7B 00              1000 	mov	r3,#0x00
                           1001 ;     genCast
                           1002 ;     genPointerSet
                           1003 ;     genFarPointerSet
   08E8 EA                 1004 	mov	a,r2
   08E9 F0                 1005 	movx	@dptr,a
                           1006 ;Initial/src/lib/isr.c:87: EA = t;
                           1007 ;     genAssign
   08EA A2 01              1008 	mov	c,_hook_uv_t_1_1
   08EC 92 AF              1009 	mov	_EA,c
   08EE                    1010 00106$:
   08EE 22                 1011 	ret
                           1012 ;------------------------------------------------------------
                           1013 ;Allocation info for local variables in function 'hook_fgv'
                           1014 ;------------------------------------------------------------
                           1015 ;addr                      Allocated with name '_hook_fgv_PARM_2'
                           1016 ;vector_number             Allocated to registers r2 
                           1017 ;------------------------------------------------------------
                           1018 ;Initial/src/lib/isr.c:97: hook_fgv (unsigned char vector_number, unsigned short addr)
                           1019 ;	-----------------------------------------
                           1020 ;	 function hook_fgv
                           1021 ;	-----------------------------------------
   08EF                    1022 _hook_fgv:
                           1023 ;     genReceive
   08EF AA 82              1024 	mov	r2,dpl
                           1025 ;Initial/src/lib/isr.c:103: if (vector_number < FGV_MIN || vector_number > FGV_MAX)
                           1026 ;     genCmpLt
                           1027 ;     genCmp
   08F1 BA 80 00           1028 	cjne	r2,#0x80,00110$
   08F4                    1029 00110$:
                           1030 ;     genIfxJump
                           1031 ;	Peephole 112.b	changed ljmp to sjmp
                           1032 ;	Peephole 160	removed sjmp by inverse jump logic
   08F4 40 05              1033 	jc	00101$
   08F6                    1034 00111$:
                           1035 ;     genCmpGt
                           1036 ;     genCmp
                           1037 ;     genIfxJump
                           1038 ;	Peephole 108	removed ljmp by inverse jump logic
                           1039 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   08F6 EA                 1040 	mov	a,r2
   08F7 24 4B              1041 	add	a,#0xff - 0xB4
   08F9 50 01              1042 	jnc	00102$
   08FB                    1043 00112$:
   08FB                    1044 00101$:
                           1045 ;Initial/src/lib/isr.c:104: return;
                           1046 ;     genRet
                           1047 ;	Peephole 112.b	changed ljmp to sjmp
                           1048 ;	Peephole 251.b	replaced sjmp to ret with ret
   08FB 22                 1049 	ret
   08FC                    1050 00102$:
                           1051 ;Initial/src/lib/isr.c:106: if ((vector_number & 0x3) != 0)
                           1052 ;     genAnd
   08FC 74 03              1053 	mov	a,#0x03
   08FE 5A                 1054 	anl	a,r2
                           1055 ;     genCmpEq
                           1056 ;	Peephole 112.b	changed ljmp to sjmp
                           1057 ;	Peephole 115.b	jump optimization
   08FF FB                 1058 	mov	r3,a
   0900 60 01              1059 	jz	00105$
   0902                    1060 00113$:
                           1061 ;Initial/src/lib/isr.c:107: return;
                           1062 ;     genRet
                           1063 ;	Peephole 112.b	changed ljmp to sjmp
                           1064 ;	Peephole 251.b	replaced sjmp to ret with ret
   0902 22                 1065 	ret
   0903                    1066 00105$:
                           1067 ;Initial/src/lib/isr.c:109: t = EA;
                           1068 ;     genAssign
   0903 A2 AF              1069 	mov	c,_EA
   0905 92 02              1070 	mov	_hook_fgv_t_1_1,c
                           1071 ;Initial/src/lib/isr.c:110: EA = 0;
                           1072 ;     genAssign
   0907 C2 AF              1073 	clr	_EA
                           1074 ;Initial/src/lib/isr.c:111: _fifo_gpif_autovector[vector_number] = LJMP_OPCODE;
                           1075 ;     genPlus
                           1076 ;	Peephole 236.g	used r2 instead of ar2
   0909 EA                 1077 	mov	a,r2
   090A 24 80              1078 	add	a,#__fifo_gpif_autovector
   090C F5 82              1079 	mov	dpl,a
                           1080 ;	Peephole 181	changed mov to clr
   090E E4                 1081 	clr	a
   090F 34 00              1082 	addc	a,#(__fifo_gpif_autovector >> 8)
   0911 F5 83              1083 	mov	dph,a
                           1084 ;     genPointerSet
                           1085 ;     genFarPointerSet
   0913 74 02              1086 	mov	a,#0x02
   0915 F0                 1087 	movx	@dptr,a
                           1088 ;Initial/src/lib/isr.c:112: _fifo_gpif_autovector[vector_number + 1] = addr >> 8;
                           1089 ;     genCast
   0916 7B 00              1090 	mov	r3,#0x00
                           1091 ;     genPlus
                           1092 ;     genPlusIncr
   0918 74 01              1093 	mov	a,#0x01
                           1094 ;	Peephole 236.a	used r2 instead of ar2
   091A 2A                 1095 	add	a,r2
   091B FC                 1096 	mov	r4,a
                           1097 ;	Peephole 181	changed mov to clr
   091C E4                 1098 	clr	a
                           1099 ;	Peephole 236.b	used r3 instead of ar3
   091D 3B                 1100 	addc	a,r3
   091E FD                 1101 	mov	r5,a
                           1102 ;     genPlus
                           1103 ;	Peephole 236.g	used r4 instead of ar4
   091F EC                 1104 	mov	a,r4
   0920 24 80              1105 	add	a,#__fifo_gpif_autovector
   0922 F5 82              1106 	mov	dpl,a
                           1107 ;	Peephole 236.g	used r5 instead of ar5
   0924 ED                 1108 	mov	a,r5
   0925 34 00              1109 	addc	a,#(__fifo_gpif_autovector >> 8)
   0927 F5 83              1110 	mov	dph,a
                           1111 ;     genRightShift
                           1112 ;     genRightShiftLiteral
                           1113 ;     genrshTwo
   0929 AC 2C              1114 	mov	r4,(_hook_fgv_PARM_2 + 1)
   092B 7D 00              1115 	mov	r5,#0x00
                           1116 ;     genCast
                           1117 ;     genPointerSet
                           1118 ;     genFarPointerSet
   092D EC                 1119 	mov	a,r4
   092E F0                 1120 	movx	@dptr,a
                           1121 ;Initial/src/lib/isr.c:113: _fifo_gpif_autovector[vector_number + 2] = addr & 0xff;
                           1122 ;     genPlus
                           1123 ;     genPlusIncr
   092F 74 02              1124 	mov	a,#0x02
                           1125 ;	Peephole 236.a	used r2 instead of ar2
   0931 2A                 1126 	add	a,r2
   0932 FA                 1127 	mov	r2,a
                           1128 ;	Peephole 181	changed mov to clr
   0933 E4                 1129 	clr	a
                           1130 ;	Peephole 236.b	used r3 instead of ar3
   0934 3B                 1131 	addc	a,r3
   0935 FB                 1132 	mov	r3,a
                           1133 ;     genPlus
                           1134 ;	Peephole 236.g	used r2 instead of ar2
   0936 EA                 1135 	mov	a,r2
   0937 24 80              1136 	add	a,#__fifo_gpif_autovector
   0939 F5 82              1137 	mov	dpl,a
                           1138 ;	Peephole 236.g	used r3 instead of ar3
   093B EB                 1139 	mov	a,r3
   093C 34 00              1140 	addc	a,#(__fifo_gpif_autovector >> 8)
   093E F5 83              1141 	mov	dph,a
                           1142 ;     genAnd
   0940 AA 2B              1143 	mov	r2,_hook_fgv_PARM_2
   0942 7B 00              1144 	mov	r3,#0x00
                           1145 ;     genCast
                           1146 ;     genPointerSet
                           1147 ;     genFarPointerSet
   0944 EA                 1148 	mov	a,r2
   0945 F0                 1149 	movx	@dptr,a
                           1150 ;Initial/src/lib/isr.c:114: EA = t;
                           1151 ;     genAssign
   0946 A2 02              1152 	mov	c,_hook_fgv_t_1_1
   0948 92 AF              1153 	mov	_EA,c
   094A                    1154 00106$:
   094A 22                 1155 	ret
                           1156 ;------------------------------------------------------------
                           1157 ;Allocation info for local variables in function 'setup_autovectors'
                           1158 ;------------------------------------------------------------
                           1159 ;------------------------------------------------------------
                           1160 ;Initial/src/lib/isr.c:125: setup_autovectors (void)
                           1161 ;	-----------------------------------------
                           1162 ;	 function setup_autovectors
                           1163 ;	-----------------------------------------
   094B                    1164 _setup_autovectors:
                           1165 ;Initial/src/lib/isr.c:128: EIUSB = 0;
                           1166 ;     genAssign
   094B C2 E8              1167 	clr	_EIUSB
                           1168 ;Initial/src/lib/isr.c:129: EIEX4 = 0;
                           1169 ;     genAssign
   094D C2 EA              1170 	clr	_EIEX4
                           1171 ;Initial/src/lib/isr.c:131: hook_sv (SV_INT_2, (unsigned short) _usb_autovector);
                           1172 ;     genCast
   094F 75 2B 00           1173 	mov	_hook_sv_PARM_2,#__usb_autovector
   0952 75 2C 01           1174 	mov	(_hook_sv_PARM_2 + 1),#(__usb_autovector >> 8)
                           1175 ;     genCall
   0955 75 82 43           1176 	mov	dpl,#0x43
   0958 12 08 2E           1177 	lcall	_hook_sv
                           1178 ;Initial/src/lib/isr.c:132: hook_sv (SV_INT_4, (unsigned short) _fifo_gpif_autovector);
                           1179 ;     genCast
   095B 75 2B 80           1180 	mov	_hook_sv_PARM_2,#__fifo_gpif_autovector
   095E 75 2C 00           1181 	mov	(_hook_sv_PARM_2 + 1),#(__fifo_gpif_autovector >> 8)
                           1182 ;     genCall
   0961 75 82 53           1183 	mov	dpl,#0x53
   0964 12 08 2E           1184 	lcall	_hook_sv
                           1185 ;Initial/src/lib/isr.c:135: SYNCDELAY;
                           1186 ;     genInline
   0967 00                 1187 	 nop; nop; nop; 
                           1188 ;Initial/src/lib/isr.c:136: EP2FIFOIE = 0;	SYNCDELAY;
                           1189 ;     genAssign
   0968 90 E6 50           1190 	mov	dptr,#_EP2FIFOIE
                           1191 ;	Peephole 181	changed mov to clr
   096B E4                 1192 	clr	a
   096C F0                 1193 	movx	@dptr,a
                           1194 ;     genInline
   096D 00                 1195 	 nop; nop; nop; 
                           1196 ;Initial/src/lib/isr.c:137: EP4FIFOIE = 0;	SYNCDELAY;
                           1197 ;     genAssign
   096E 90 E6 52           1198 	mov	dptr,#_EP4FIFOIE
                           1199 ;	Peephole 181	changed mov to clr
   0971 E4                 1200 	clr	a
   0972 F0                 1201 	movx	@dptr,a
                           1202 ;     genInline
   0973 00                 1203 	 nop; nop; nop; 
                           1204 ;Initial/src/lib/isr.c:138: EP6FIFOIE = 0;	SYNCDELAY;
                           1205 ;     genAssign
   0974 90 E6 54           1206 	mov	dptr,#_EP6FIFOIE
                           1207 ;	Peephole 181	changed mov to clr
   0977 E4                 1208 	clr	a
   0978 F0                 1209 	movx	@dptr,a
                           1210 ;     genInline
   0979 00                 1211 	 nop; nop; nop; 
                           1212 ;Initial/src/lib/isr.c:139: EP8FIFOIE = 0;	SYNCDELAY;
                           1213 ;     genAssign
   097A 90 E6 56           1214 	mov	dptr,#_EP8FIFOIE
                           1215 ;	Peephole 181	changed mov to clr
   097D E4                 1216 	clr	a
   097E F0                 1217 	movx	@dptr,a
                           1218 ;     genInline
   097F 00                 1219 	 nop; nop; nop; 
                           1220 ;Initial/src/lib/isr.c:142: EP2FIFOIRQ = 0xff;	SYNCDELAY;
                           1221 ;     genAssign
   0980 90 E6 51           1222 	mov	dptr,#_EP2FIFOIRQ
   0983 74 FF              1223 	mov	a,#0xFF
   0985 F0                 1224 	movx	@dptr,a
                           1225 ;     genInline
   0986 00                 1226 	 nop; nop; nop; 
                           1227 ;Initial/src/lib/isr.c:143: EP4FIFOIRQ = 0xff;	SYNCDELAY;
                           1228 ;     genAssign
   0987 90 E6 53           1229 	mov	dptr,#_EP4FIFOIRQ
   098A 74 FF              1230 	mov	a,#0xFF
   098C F0                 1231 	movx	@dptr,a
                           1232 ;     genInline
   098D 00                 1233 	 nop; nop; nop; 
                           1234 ;Initial/src/lib/isr.c:144: EP6FIFOIRQ = 0xff;	SYNCDELAY;
                           1235 ;     genAssign
   098E 90 E6 55           1236 	mov	dptr,#_EP6FIFOIRQ
   0991 74 FF              1237 	mov	a,#0xFF
   0993 F0                 1238 	movx	@dptr,a
                           1239 ;     genInline
   0994 00                 1240 	 nop; nop; nop; 
                           1241 ;Initial/src/lib/isr.c:145: EP8FIFOIRQ = 0xff;	SYNCDELAY;
                           1242 ;     genAssign
   0995 90 E6 57           1243 	mov	dptr,#_EP8FIFOIRQ
   0998 74 FF              1244 	mov	a,#0xFF
   099A F0                 1245 	movx	@dptr,a
                           1246 ;     genInline
   099B 00                 1247 	 nop; nop; nop; 
                           1248 ;Initial/src/lib/isr.c:147: IBNIE  = 0;
                           1249 ;     genAssign
   099C 90 E6 58           1250 	mov	dptr,#_IBNIE
                           1251 ;	Peephole 181	changed mov to clr
   099F E4                 1252 	clr	a
   09A0 F0                 1253 	movx	@dptr,a
                           1254 ;Initial/src/lib/isr.c:148: IBNIRQ = 0xff;
                           1255 ;     genAssign
   09A1 90 E6 59           1256 	mov	dptr,#_IBNIRQ
   09A4 74 FF              1257 	mov	a,#0xFF
   09A6 F0                 1258 	movx	@dptr,a
                           1259 ;Initial/src/lib/isr.c:149: NAKIE  = 0;
                           1260 ;     genAssign
   09A7 90 E6 5A           1261 	mov	dptr,#_NAKIE
                           1262 ;	Peephole 181	changed mov to clr
   09AA E4                 1263 	clr	a
   09AB F0                 1264 	movx	@dptr,a
                           1265 ;Initial/src/lib/isr.c:150: NAKIRQ = 0xff;
                           1266 ;     genAssign
   09AC 90 E6 5B           1267 	mov	dptr,#_NAKIRQ
   09AF 74 FF              1268 	mov	a,#0xFF
   09B1 F0                 1269 	movx	@dptr,a
                           1270 ;Initial/src/lib/isr.c:151: USBIE  = 0;
                           1271 ;     genAssign
   09B2 90 E6 5C           1272 	mov	dptr,#_USBIE
                           1273 ;	Peephole 181	changed mov to clr
   09B5 E4                 1274 	clr	a
   09B6 F0                 1275 	movx	@dptr,a
                           1276 ;Initial/src/lib/isr.c:152: USBIRQ = 0xff;
                           1277 ;     genAssign
   09B7 90 E6 5D           1278 	mov	dptr,#_USBIRQ
   09BA 74 FF              1279 	mov	a,#0xFF
   09BC F0                 1280 	movx	@dptr,a
                           1281 ;Initial/src/lib/isr.c:153: EPIE   = 0;
                           1282 ;     genAssign
   09BD 90 E6 5E           1283 	mov	dptr,#_EPIE
                           1284 ;	Peephole 181	changed mov to clr
   09C0 E4                 1285 	clr	a
   09C1 F0                 1286 	movx	@dptr,a
                           1287 ;Initial/src/lib/isr.c:154: EPIRQ  = 0xff;
                           1288 ;     genAssign
   09C2 90 E6 5F           1289 	mov	dptr,#_EPIRQ
   09C5 74 FF              1290 	mov	a,#0xFF
   09C7 F0                 1291 	movx	@dptr,a
                           1292 ;Initial/src/lib/isr.c:155: SYNCDELAY;	GPIFIE = 0;
                           1293 ;     genInline
   09C8 00                 1294 	 nop; nop; nop; 
                           1295 ;     genAssign
   09C9 90 E6 60           1296 	mov	dptr,#_GPIFIE
                           1297 ;	Peephole 181	changed mov to clr
   09CC E4                 1298 	clr	a
   09CD F0                 1299 	movx	@dptr,a
                           1300 ;Initial/src/lib/isr.c:156: SYNCDELAY;	GPIFIRQ = 0xff;
                           1301 ;     genInline
   09CE 00                 1302 	 nop; nop; nop; 
                           1303 ;     genAssign
   09CF 90 E6 61           1304 	mov	dptr,#_GPIFIRQ
   09D2 74 FF              1305 	mov	a,#0xFF
   09D4 F0                 1306 	movx	@dptr,a
                           1307 ;Initial/src/lib/isr.c:157: USBERRIE = 0;
                           1308 ;     genAssign
   09D5 90 E6 62           1309 	mov	dptr,#_USBERRIE
                           1310 ;	Peephole 181	changed mov to clr
   09D8 E4                 1311 	clr	a
   09D9 F0                 1312 	movx	@dptr,a
                           1313 ;Initial/src/lib/isr.c:158: USBERRIRQ = 0xff;
                           1314 ;     genAssign
   09DA 90 E6 63           1315 	mov	dptr,#_USBERRIRQ
   09DD 74 FF              1316 	mov	a,#0xFF
   09DF F0                 1317 	movx	@dptr,a
                           1318 ;Initial/src/lib/isr.c:159: CLRERRCNT = 0;
                           1319 ;     genAssign
   09E0 90 E6 65           1320 	mov	dptr,#_CLRERRCNT
                           1321 ;	Peephole 181	changed mov to clr
   09E3 E4                 1322 	clr	a
   09E4 F0                 1323 	movx	@dptr,a
                           1324 ;Initial/src/lib/isr.c:161: INTSETUP = bmAV2EN | bmAV4EN | bmINT4IN;
                           1325 ;     genAssign
   09E5 90 E6 68           1326 	mov	dptr,#_INTSETUP
   09E8 74 0B              1327 	mov	a,#0x0B
   09EA F0                 1328 	movx	@dptr,a
                           1329 ;Initial/src/lib/isr.c:164: EXIF &= ~bmEXIF_USBINT;
                           1330 ;     genAnd
                           1331 ;Initial/src/lib/isr.c:165: EXIF &= ~bmEXIF_IE4;
                           1332 ;     genAnd
                           1333 ;	Peephole 183	avoided anl during execution
   09EB 53 91 AF           1334 	anl	_EXIF,#(0xEF & 0xBF)
                           1335 ;Initial/src/lib/isr.c:168: EIUSB = 1;
                           1336 ;     genAssign
   09EE D2 E8              1337 	setb	_EIUSB
                           1338 ;Initial/src/lib/isr.c:169: EIEX4 = 1;
                           1339 ;     genAssign
   09F0 D2 EA              1340 	setb	_EIEX4
   09F2                    1341 00101$:
   09F2 22                 1342 	ret
                           1343 	.area CSEG    (CODE)
