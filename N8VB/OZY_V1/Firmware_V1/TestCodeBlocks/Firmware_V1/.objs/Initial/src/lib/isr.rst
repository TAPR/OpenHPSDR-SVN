                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Tue Jul 11 16:42:00 2006
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
                            685 	.area XISEG   (XDATA)
                            686 	.area CSEG    (CODE)
                            687 	.area GSINIT0 (CODE)
                            688 	.area GSINIT1 (CODE)
                            689 	.area GSINIT2 (CODE)
                            690 	.area GSINIT3 (CODE)
                            691 	.area GSINIT4 (CODE)
                            692 	.area GSINIT5 (CODE)
                            693 ;--------------------------------------------------------
                            694 ; global & static initialisations
                            695 ;--------------------------------------------------------
                            696 	.area CSEG    (CODE)
                            697 	.area GSINIT  (CODE)
                            698 	.area GSFINAL (CODE)
                            699 	.area GSINIT  (CODE)
                            700 ;--------------------------------------------------------
                            701 ; Home
                            702 ;--------------------------------------------------------
                            703 	.area HOME    (CODE)
                            704 	.area CSEG    (CODE)
                            705 ;--------------------------------------------------------
                            706 ; code
                            707 ;--------------------------------------------------------
                            708 	.area CSEG    (CODE)
                            709 ;------------------------------------------------------------
                            710 ;Allocation info for local variables in function 'hook_sv'
                            711 ;------------------------------------------------------------
                            712 ;addr                      Allocated with name '_hook_sv_PARM_2'
                            713 ;vector_number             Allocated to registers r2 
                            714 ;------------------------------------------------------------
                            715 ;Initial/src/lib/isr.c:43: hook_sv (unsigned char vector_number, unsigned short addr)
                            716 ;	-----------------------------------------
                            717 ;	 function hook_sv
                            718 ;	-----------------------------------------
   06F8                     719 _hook_sv:
                    0002    720 	ar2 = 0x02
                    0003    721 	ar3 = 0x03
                    0004    722 	ar4 = 0x04
                    0005    723 	ar5 = 0x05
                    0006    724 	ar6 = 0x06
                    0007    725 	ar7 = 0x07
                    0000    726 	ar0 = 0x00
                    0001    727 	ar1 = 0x01
                            728 ;     genReceive
   06F8 AA 82               729 	mov	r2,dpl
                            730 ;Initial/src/lib/isr.c:49: if (vector_number < SV_MIN || vector_number > SV_MAX)
                            731 ;     genCmpLt
                            732 ;     genCmp
   06FA BA 03 00            733 	cjne	r2,#0x03,00112$
   06FD                     734 00112$:
                            735 ;     genIfxJump
                            736 ;	Peephole 112.b	changed ljmp to sjmp
                            737 ;	Peephole 160	removed sjmp by inverse jump logic
   06FD 40 05               738 	jc	00101$
   06FF                     739 00113$:
                            740 ;     genCmpGt
                            741 ;     genCmp
                            742 ;     genIfxJump
                            743 ;	Peephole 108	removed ljmp by inverse jump logic
                            744 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   06FF EA                  745 	mov	a,r2
   0700 24 9C               746 	add	a,#0xff - 0x63
   0702 50 01               747 	jnc	00102$
   0704                     748 00114$:
   0704                     749 00101$:
                            750 ;Initial/src/lib/isr.c:50: return;
                            751 ;     genRet
                            752 ;	Peephole 112.b	changed ljmp to sjmp
                            753 ;	Peephole 251.b	replaced sjmp to ret with ret
   0704 22                  754 	ret
   0705                     755 00102$:
                            756 ;Initial/src/lib/isr.c:52: if ((vector_number & 0x0f) != 0x03 && (vector_number & 0x0f) != 0x0b)
                            757 ;     genAnd
   0705 74 0F               758 	mov	a,#0x0F
   0707 5A                  759 	anl	a,r2
   0708 FB                  760 	mov	r3,a
                            761 ;     genCmpEq
   0709 BB 03 02            762 	cjne	r3,#0x03,00115$
                            763 ;	Peephole 112.b	changed ljmp to sjmp
   070C 80 07               764 	sjmp	00105$
   070E                     765 00115$:
                            766 ;     genAnd
   070E 74 0F               767 	mov	a,#0x0F
   0710 5A                  768 	anl	a,r2
   0711 FB                  769 	mov	r3,a
                            770 ;     genCmpEq
                            771 ;	Peephole 112.b	changed ljmp to sjmp
                            772 ;Initial/src/lib/isr.c:53: return;
                            773 ;     genRet
                            774 ;	Peephole 112.b	changed ljmp to sjmp
                            775 ;	Peephole 199	optimized misc jump sequence
   0712 BB 0B 47            776 	cjne	r3,#0x0B,00107$
                            777 ;00116$:
                            778 ;	Peephole 200	removed redundant sjmp
   0715                     779 00105$:
                            780 ;Initial/src/lib/isr.c:55: t = EA;
                            781 ;     genAssign
   0715 A2 AF               782 	mov	c,_EA
   0717 92 00               783 	mov	_hook_sv_t_1_1,c
                            784 ;Initial/src/lib/isr.c:56: EA = 0;
                            785 ;     genAssign
   0719 C2 AF               786 	clr	_EA
                            787 ;Initial/src/lib/isr.c:57: _standard_interrupt_vector[vector_number] = LJMP_OPCODE;
                            788 ;     genPlus
                            789 ;	Peephole 236.g	used r2 instead of ar2
   071B EA                  790 	mov	a,r2
   071C 24 F5               791 	add	a,#__standard_interrupt_vector
   071E F5 82               792 	mov	dpl,a
                            793 ;	Peephole 181	changed mov to clr
   0720 E4                  794 	clr	a
   0721 34 01               795 	addc	a,#(__standard_interrupt_vector >> 8)
   0723 F5 83               796 	mov	dph,a
                            797 ;     genPointerSet
                            798 ;     genFarPointerSet
   0725 74 02               799 	mov	a,#0x02
   0727 F0                  800 	movx	@dptr,a
                            801 ;Initial/src/lib/isr.c:58: _standard_interrupt_vector[vector_number + 1] = addr >> 8;
                            802 ;     genCast
   0728 7B 00               803 	mov	r3,#0x00
                            804 ;     genPlus
                            805 ;     genPlusIncr
   072A 74 01               806 	mov	a,#0x01
                            807 ;	Peephole 236.a	used r2 instead of ar2
   072C 2A                  808 	add	a,r2
   072D FC                  809 	mov	r4,a
                            810 ;	Peephole 181	changed mov to clr
   072E E4                  811 	clr	a
                            812 ;	Peephole 236.b	used r3 instead of ar3
   072F 3B                  813 	addc	a,r3
   0730 FD                  814 	mov	r5,a
                            815 ;     genPlus
                            816 ;	Peephole 236.g	used r4 instead of ar4
   0731 EC                  817 	mov	a,r4
   0732 24 F5               818 	add	a,#__standard_interrupt_vector
   0734 F5 82               819 	mov	dpl,a
                            820 ;	Peephole 236.g	used r5 instead of ar5
   0736 ED                  821 	mov	a,r5
   0737 34 01               822 	addc	a,#(__standard_interrupt_vector >> 8)
   0739 F5 83               823 	mov	dph,a
                            824 ;     genRightShift
                            825 ;     genRightShiftLiteral
                            826 ;     genrshTwo
   073B AC 2C               827 	mov	r4,(_hook_sv_PARM_2 + 1)
   073D 7D 00               828 	mov	r5,#0x00
                            829 ;     genCast
                            830 ;     genPointerSet
                            831 ;     genFarPointerSet
   073F EC                  832 	mov	a,r4
   0740 F0                  833 	movx	@dptr,a
                            834 ;Initial/src/lib/isr.c:59: _standard_interrupt_vector[vector_number + 2] = addr & 0xff;
                            835 ;     genPlus
                            836 ;     genPlusIncr
   0741 74 02               837 	mov	a,#0x02
                            838 ;	Peephole 236.a	used r2 instead of ar2
   0743 2A                  839 	add	a,r2
   0744 FA                  840 	mov	r2,a
                            841 ;	Peephole 181	changed mov to clr
   0745 E4                  842 	clr	a
                            843 ;	Peephole 236.b	used r3 instead of ar3
   0746 3B                  844 	addc	a,r3
   0747 FB                  845 	mov	r3,a
                            846 ;     genPlus
                            847 ;	Peephole 236.g	used r2 instead of ar2
   0748 EA                  848 	mov	a,r2
   0749 24 F5               849 	add	a,#__standard_interrupt_vector
   074B F5 82               850 	mov	dpl,a
                            851 ;	Peephole 236.g	used r3 instead of ar3
   074D EB                  852 	mov	a,r3
   074E 34 01               853 	addc	a,#(__standard_interrupt_vector >> 8)
   0750 F5 83               854 	mov	dph,a
                            855 ;     genAnd
   0752 AA 2B               856 	mov	r2,_hook_sv_PARM_2
   0754 7B 00               857 	mov	r3,#0x00
                            858 ;     genCast
                            859 ;     genPointerSet
                            860 ;     genFarPointerSet
   0756 EA                  861 	mov	a,r2
   0757 F0                  862 	movx	@dptr,a
                            863 ;Initial/src/lib/isr.c:60: EA = t;
                            864 ;     genAssign
   0758 A2 00               865 	mov	c,_hook_sv_t_1_1
   075A 92 AF               866 	mov	_EA,c
   075C                     867 00107$:
   075C 22                  868 	ret
                            869 ;------------------------------------------------------------
                            870 ;Allocation info for local variables in function 'hook_uv'
                            871 ;------------------------------------------------------------
                            872 ;addr                      Allocated with name '_hook_uv_PARM_2'
                            873 ;vector_number             Allocated to registers r2 
                            874 ;------------------------------------------------------------
                            875 ;Initial/src/lib/isr.c:70: hook_uv (unsigned char vector_number, unsigned short addr)
                            876 ;	-----------------------------------------
                            877 ;	 function hook_uv
                            878 ;	-----------------------------------------
   075D                     879 _hook_uv:
                            880 ;     genReceive
   075D AA 82               881 	mov	r2,dpl
                            882 ;Initial/src/lib/isr.c:76: if (vector_number < UV_MIN || vector_number > UV_MAX)
                            883 ;     genCmpLt
                            884 ;     genCmp
   075F BA 00 00            885 	cjne	r2,#0x00,00110$
   0762                     886 00110$:
                            887 ;     genIfxJump
                            888 ;	Peephole 112.b	changed ljmp to sjmp
                            889 ;	Peephole 160	removed sjmp by inverse jump logic
   0762 40 05               890 	jc	00101$
   0764                     891 00111$:
                            892 ;     genCmpGt
                            893 ;     genCmp
                            894 ;     genIfxJump
                            895 ;	Peephole 108	removed ljmp by inverse jump logic
                            896 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   0764 EA                  897 	mov	a,r2
   0765 24 83               898 	add	a,#0xff - 0x7C
   0767 50 01               899 	jnc	00102$
   0769                     900 00112$:
   0769                     901 00101$:
                            902 ;Initial/src/lib/isr.c:77: return;
                            903 ;     genRet
                            904 ;	Peephole 112.b	changed ljmp to sjmp
                            905 ;	Peephole 251.b	replaced sjmp to ret with ret
   0769 22                  906 	ret
   076A                     907 00102$:
                            908 ;Initial/src/lib/isr.c:79: if ((vector_number & 0x3) != 0)
                            909 ;     genAnd
   076A 74 03               910 	mov	a,#0x03
   076C 5A                  911 	anl	a,r2
                            912 ;     genCmpEq
                            913 ;	Peephole 112.b	changed ljmp to sjmp
                            914 ;	Peephole 115.b	jump optimization
   076D FB                  915 	mov	r3,a
   076E 60 01               916 	jz	00105$
   0770                     917 00113$:
                            918 ;Initial/src/lib/isr.c:80: return;
                            919 ;     genRet
                            920 ;	Peephole 112.b	changed ljmp to sjmp
                            921 ;	Peephole 251.b	replaced sjmp to ret with ret
   0770 22                  922 	ret
   0771                     923 00105$:
                            924 ;Initial/src/lib/isr.c:82: t = EA;
                            925 ;     genAssign
   0771 A2 AF               926 	mov	c,_EA
   0773 92 01               927 	mov	_hook_uv_t_1_1,c
                            928 ;Initial/src/lib/isr.c:83: EA = 0;
                            929 ;     genAssign
   0775 C2 AF               930 	clr	_EA
                            931 ;Initial/src/lib/isr.c:84: _usb_autovector[vector_number] = LJMP_OPCODE;
                            932 ;     genPlus
                            933 ;	Peephole 236.g	used r2 instead of ar2
   0777 EA                  934 	mov	a,r2
   0778 24 F5               935 	add	a,#__usb_autovector
   077A F5 82               936 	mov	dpl,a
                            937 ;	Peephole 181	changed mov to clr
   077C E4                  938 	clr	a
   077D 34 02               939 	addc	a,#(__usb_autovector >> 8)
   077F F5 83               940 	mov	dph,a
                            941 ;     genPointerSet
                            942 ;     genFarPointerSet
   0781 74 02               943 	mov	a,#0x02
   0783 F0                  944 	movx	@dptr,a
                            945 ;Initial/src/lib/isr.c:85: _usb_autovector[vector_number + 1] = addr >> 8;
                            946 ;     genCast
   0784 7B 00               947 	mov	r3,#0x00
                            948 ;     genPlus
                            949 ;     genPlusIncr
   0786 74 01               950 	mov	a,#0x01
                            951 ;	Peephole 236.a	used r2 instead of ar2
   0788 2A                  952 	add	a,r2
   0789 FC                  953 	mov	r4,a
                            954 ;	Peephole 181	changed mov to clr
   078A E4                  955 	clr	a
                            956 ;	Peephole 236.b	used r3 instead of ar3
   078B 3B                  957 	addc	a,r3
   078C FD                  958 	mov	r5,a
                            959 ;     genPlus
                            960 ;	Peephole 236.g	used r4 instead of ar4
   078D EC                  961 	mov	a,r4
   078E 24 F5               962 	add	a,#__usb_autovector
   0790 F5 82               963 	mov	dpl,a
                            964 ;	Peephole 236.g	used r5 instead of ar5
   0792 ED                  965 	mov	a,r5
   0793 34 02               966 	addc	a,#(__usb_autovector >> 8)
   0795 F5 83               967 	mov	dph,a
                            968 ;     genRightShift
                            969 ;     genRightShiftLiteral
                            970 ;     genrshTwo
   0797 AC 2C               971 	mov	r4,(_hook_uv_PARM_2 + 1)
   0799 7D 00               972 	mov	r5,#0x00
                            973 ;     genCast
                            974 ;     genPointerSet
                            975 ;     genFarPointerSet
   079B EC                  976 	mov	a,r4
   079C F0                  977 	movx	@dptr,a
                            978 ;Initial/src/lib/isr.c:86: _usb_autovector[vector_number + 2] = addr & 0xff;
                            979 ;     genPlus
                            980 ;     genPlusIncr
   079D 74 02               981 	mov	a,#0x02
                            982 ;	Peephole 236.a	used r2 instead of ar2
   079F 2A                  983 	add	a,r2
   07A0 FA                  984 	mov	r2,a
                            985 ;	Peephole 181	changed mov to clr
   07A1 E4                  986 	clr	a
                            987 ;	Peephole 236.b	used r3 instead of ar3
   07A2 3B                  988 	addc	a,r3
   07A3 FB                  989 	mov	r3,a
                            990 ;     genPlus
                            991 ;	Peephole 236.g	used r2 instead of ar2
   07A4 EA                  992 	mov	a,r2
   07A5 24 F5               993 	add	a,#__usb_autovector
   07A7 F5 82               994 	mov	dpl,a
                            995 ;	Peephole 236.g	used r3 instead of ar3
   07A9 EB                  996 	mov	a,r3
   07AA 34 02               997 	addc	a,#(__usb_autovector >> 8)
   07AC F5 83               998 	mov	dph,a
                            999 ;     genAnd
   07AE AA 2B              1000 	mov	r2,_hook_uv_PARM_2
   07B0 7B 00              1001 	mov	r3,#0x00
                           1002 ;     genCast
                           1003 ;     genPointerSet
                           1004 ;     genFarPointerSet
   07B2 EA                 1005 	mov	a,r2
   07B3 F0                 1006 	movx	@dptr,a
                           1007 ;Initial/src/lib/isr.c:87: EA = t;
                           1008 ;     genAssign
   07B4 A2 01              1009 	mov	c,_hook_uv_t_1_1
   07B6 92 AF              1010 	mov	_EA,c
   07B8                    1011 00106$:
   07B8 22                 1012 	ret
                           1013 ;------------------------------------------------------------
                           1014 ;Allocation info for local variables in function 'hook_fgv'
                           1015 ;------------------------------------------------------------
                           1016 ;addr                      Allocated with name '_hook_fgv_PARM_2'
                           1017 ;vector_number             Allocated to registers r2 
                           1018 ;------------------------------------------------------------
                           1019 ;Initial/src/lib/isr.c:97: hook_fgv (unsigned char vector_number, unsigned short addr)
                           1020 ;	-----------------------------------------
                           1021 ;	 function hook_fgv
                           1022 ;	-----------------------------------------
   07B9                    1023 _hook_fgv:
                           1024 ;     genReceive
   07B9 AA 82              1025 	mov	r2,dpl
                           1026 ;Initial/src/lib/isr.c:103: if (vector_number < FGV_MIN || vector_number > FGV_MAX)
                           1027 ;     genCmpLt
                           1028 ;     genCmp
   07BB BA 80 00           1029 	cjne	r2,#0x80,00110$
   07BE                    1030 00110$:
                           1031 ;     genIfxJump
                           1032 ;	Peephole 112.b	changed ljmp to sjmp
                           1033 ;	Peephole 160	removed sjmp by inverse jump logic
   07BE 40 05              1034 	jc	00101$
   07C0                    1035 00111$:
                           1036 ;     genCmpGt
                           1037 ;     genCmp
                           1038 ;     genIfxJump
                           1039 ;	Peephole 108	removed ljmp by inverse jump logic
                           1040 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   07C0 EA                 1041 	mov	a,r2
   07C1 24 4B              1042 	add	a,#0xff - 0xB4
   07C3 50 01              1043 	jnc	00102$
   07C5                    1044 00112$:
   07C5                    1045 00101$:
                           1046 ;Initial/src/lib/isr.c:104: return;
                           1047 ;     genRet
                           1048 ;	Peephole 112.b	changed ljmp to sjmp
                           1049 ;	Peephole 251.b	replaced sjmp to ret with ret
   07C5 22                 1050 	ret
   07C6                    1051 00102$:
                           1052 ;Initial/src/lib/isr.c:106: if ((vector_number & 0x3) != 0)
                           1053 ;     genAnd
   07C6 74 03              1054 	mov	a,#0x03
   07C8 5A                 1055 	anl	a,r2
                           1056 ;     genCmpEq
                           1057 ;	Peephole 112.b	changed ljmp to sjmp
                           1058 ;	Peephole 115.b	jump optimization
   07C9 FB                 1059 	mov	r3,a
   07CA 60 01              1060 	jz	00105$
   07CC                    1061 00113$:
                           1062 ;Initial/src/lib/isr.c:107: return;
                           1063 ;     genRet
                           1064 ;	Peephole 112.b	changed ljmp to sjmp
                           1065 ;	Peephole 251.b	replaced sjmp to ret with ret
   07CC 22                 1066 	ret
   07CD                    1067 00105$:
                           1068 ;Initial/src/lib/isr.c:109: t = EA;
                           1069 ;     genAssign
   07CD A2 AF              1070 	mov	c,_EA
   07CF 92 02              1071 	mov	_hook_fgv_t_1_1,c
                           1072 ;Initial/src/lib/isr.c:110: EA = 0;
                           1073 ;     genAssign
   07D1 C2 AF              1074 	clr	_EA
                           1075 ;Initial/src/lib/isr.c:111: _fifo_gpif_autovector[vector_number] = LJMP_OPCODE;
                           1076 ;     genPlus
                           1077 ;	Peephole 236.g	used r2 instead of ar2
   07D3 EA                 1078 	mov	a,r2
   07D4 24 75              1079 	add	a,#__fifo_gpif_autovector
   07D6 F5 82              1080 	mov	dpl,a
                           1081 ;	Peephole 181	changed mov to clr
   07D8 E4                 1082 	clr	a
   07D9 34 02              1083 	addc	a,#(__fifo_gpif_autovector >> 8)
   07DB F5 83              1084 	mov	dph,a
                           1085 ;     genPointerSet
                           1086 ;     genFarPointerSet
   07DD 74 02              1087 	mov	a,#0x02
   07DF F0                 1088 	movx	@dptr,a
                           1089 ;Initial/src/lib/isr.c:112: _fifo_gpif_autovector[vector_number + 1] = addr >> 8;
                           1090 ;     genCast
   07E0 7B 00              1091 	mov	r3,#0x00
                           1092 ;     genPlus
                           1093 ;     genPlusIncr
   07E2 74 01              1094 	mov	a,#0x01
                           1095 ;	Peephole 236.a	used r2 instead of ar2
   07E4 2A                 1096 	add	a,r2
   07E5 FC                 1097 	mov	r4,a
                           1098 ;	Peephole 181	changed mov to clr
   07E6 E4                 1099 	clr	a
                           1100 ;	Peephole 236.b	used r3 instead of ar3
   07E7 3B                 1101 	addc	a,r3
   07E8 FD                 1102 	mov	r5,a
                           1103 ;     genPlus
                           1104 ;	Peephole 236.g	used r4 instead of ar4
   07E9 EC                 1105 	mov	a,r4
   07EA 24 75              1106 	add	a,#__fifo_gpif_autovector
   07EC F5 82              1107 	mov	dpl,a
                           1108 ;	Peephole 236.g	used r5 instead of ar5
   07EE ED                 1109 	mov	a,r5
   07EF 34 02              1110 	addc	a,#(__fifo_gpif_autovector >> 8)
   07F1 F5 83              1111 	mov	dph,a
                           1112 ;     genRightShift
                           1113 ;     genRightShiftLiteral
                           1114 ;     genrshTwo
   07F3 AC 2C              1115 	mov	r4,(_hook_fgv_PARM_2 + 1)
   07F5 7D 00              1116 	mov	r5,#0x00
                           1117 ;     genCast
                           1118 ;     genPointerSet
                           1119 ;     genFarPointerSet
   07F7 EC                 1120 	mov	a,r4
   07F8 F0                 1121 	movx	@dptr,a
                           1122 ;Initial/src/lib/isr.c:113: _fifo_gpif_autovector[vector_number + 2] = addr & 0xff;
                           1123 ;     genPlus
                           1124 ;     genPlusIncr
   07F9 74 02              1125 	mov	a,#0x02
                           1126 ;	Peephole 236.a	used r2 instead of ar2
   07FB 2A                 1127 	add	a,r2
   07FC FA                 1128 	mov	r2,a
                           1129 ;	Peephole 181	changed mov to clr
   07FD E4                 1130 	clr	a
                           1131 ;	Peephole 236.b	used r3 instead of ar3
   07FE 3B                 1132 	addc	a,r3
   07FF FB                 1133 	mov	r3,a
                           1134 ;     genPlus
                           1135 ;	Peephole 236.g	used r2 instead of ar2
   0800 EA                 1136 	mov	a,r2
   0801 24 75              1137 	add	a,#__fifo_gpif_autovector
   0803 F5 82              1138 	mov	dpl,a
                           1139 ;	Peephole 236.g	used r3 instead of ar3
   0805 EB                 1140 	mov	a,r3
   0806 34 02              1141 	addc	a,#(__fifo_gpif_autovector >> 8)
   0808 F5 83              1142 	mov	dph,a
                           1143 ;     genAnd
   080A AA 2B              1144 	mov	r2,_hook_fgv_PARM_2
   080C 7B 00              1145 	mov	r3,#0x00
                           1146 ;     genCast
                           1147 ;     genPointerSet
                           1148 ;     genFarPointerSet
   080E EA                 1149 	mov	a,r2
   080F F0                 1150 	movx	@dptr,a
                           1151 ;Initial/src/lib/isr.c:114: EA = t;
                           1152 ;     genAssign
   0810 A2 02              1153 	mov	c,_hook_fgv_t_1_1
   0812 92 AF              1154 	mov	_EA,c
   0814                    1155 00106$:
   0814 22                 1156 	ret
                           1157 ;------------------------------------------------------------
                           1158 ;Allocation info for local variables in function 'setup_autovectors'
                           1159 ;------------------------------------------------------------
                           1160 ;------------------------------------------------------------
                           1161 ;Initial/src/lib/isr.c:125: setup_autovectors (void)
                           1162 ;	-----------------------------------------
                           1163 ;	 function setup_autovectors
                           1164 ;	-----------------------------------------
   0815                    1165 _setup_autovectors:
                           1166 ;Initial/src/lib/isr.c:128: EIUSB = 0;
                           1167 ;     genAssign
   0815 C2 E8              1168 	clr	_EIUSB
                           1169 ;Initial/src/lib/isr.c:129: EIEX4 = 0;
                           1170 ;     genAssign
   0817 C2 EA              1171 	clr	_EIEX4
                           1172 ;Initial/src/lib/isr.c:131: hook_sv (SV_INT_2, (unsigned short) _usb_autovector);
                           1173 ;     genCast
   0819 75 2B F5           1174 	mov	_hook_sv_PARM_2,#__usb_autovector
   081C 75 2C 02           1175 	mov	(_hook_sv_PARM_2 + 1),#(__usb_autovector >> 8)
                           1176 ;     genCall
   081F 75 82 43           1177 	mov	dpl,#0x43
   0822 12 06 F8           1178 	lcall	_hook_sv
                           1179 ;Initial/src/lib/isr.c:132: hook_sv (SV_INT_4, (unsigned short) _fifo_gpif_autovector);
                           1180 ;     genCast
   0825 75 2B 75           1181 	mov	_hook_sv_PARM_2,#__fifo_gpif_autovector
   0828 75 2C 02           1182 	mov	(_hook_sv_PARM_2 + 1),#(__fifo_gpif_autovector >> 8)
                           1183 ;     genCall
   082B 75 82 53           1184 	mov	dpl,#0x53
   082E 12 06 F8           1185 	lcall	_hook_sv
                           1186 ;Initial/src/lib/isr.c:135: SYNCDELAY;
                           1187 ;     genInline
   0831 00                 1188 	 nop; nop; nop; 
                           1189 ;Initial/src/lib/isr.c:136: EP2FIFOIE = 0;	SYNCDELAY;
                           1190 ;     genAssign
   0832 90 E6 50           1191 	mov	dptr,#_EP2FIFOIE
                           1192 ;	Peephole 181	changed mov to clr
   0835 E4                 1193 	clr	a
   0836 F0                 1194 	movx	@dptr,a
                           1195 ;     genInline
   0837 00                 1196 	 nop; nop; nop; 
                           1197 ;Initial/src/lib/isr.c:137: EP4FIFOIE = 0;	SYNCDELAY;
                           1198 ;     genAssign
   0838 90 E6 52           1199 	mov	dptr,#_EP4FIFOIE
                           1200 ;	Peephole 181	changed mov to clr
   083B E4                 1201 	clr	a
   083C F0                 1202 	movx	@dptr,a
                           1203 ;     genInline
   083D 00                 1204 	 nop; nop; nop; 
                           1205 ;Initial/src/lib/isr.c:138: EP6FIFOIE = 0;	SYNCDELAY;
                           1206 ;     genAssign
   083E 90 E6 54           1207 	mov	dptr,#_EP6FIFOIE
                           1208 ;	Peephole 181	changed mov to clr
   0841 E4                 1209 	clr	a
   0842 F0                 1210 	movx	@dptr,a
                           1211 ;     genInline
   0843 00                 1212 	 nop; nop; nop; 
                           1213 ;Initial/src/lib/isr.c:139: EP8FIFOIE = 0;	SYNCDELAY;
                           1214 ;     genAssign
   0844 90 E6 56           1215 	mov	dptr,#_EP8FIFOIE
                           1216 ;	Peephole 181	changed mov to clr
   0847 E4                 1217 	clr	a
   0848 F0                 1218 	movx	@dptr,a
                           1219 ;     genInline
   0849 00                 1220 	 nop; nop; nop; 
                           1221 ;Initial/src/lib/isr.c:142: EP2FIFOIRQ = 0xff;	SYNCDELAY;
                           1222 ;     genAssign
   084A 90 E6 51           1223 	mov	dptr,#_EP2FIFOIRQ
   084D 74 FF              1224 	mov	a,#0xFF
   084F F0                 1225 	movx	@dptr,a
                           1226 ;     genInline
   0850 00                 1227 	 nop; nop; nop; 
                           1228 ;Initial/src/lib/isr.c:143: EP4FIFOIRQ = 0xff;	SYNCDELAY;
                           1229 ;     genAssign
   0851 90 E6 53           1230 	mov	dptr,#_EP4FIFOIRQ
   0854 74 FF              1231 	mov	a,#0xFF
   0856 F0                 1232 	movx	@dptr,a
                           1233 ;     genInline
   0857 00                 1234 	 nop; nop; nop; 
                           1235 ;Initial/src/lib/isr.c:144: EP6FIFOIRQ = 0xff;	SYNCDELAY;
                           1236 ;     genAssign
   0858 90 E6 55           1237 	mov	dptr,#_EP6FIFOIRQ
   085B 74 FF              1238 	mov	a,#0xFF
   085D F0                 1239 	movx	@dptr,a
                           1240 ;     genInline
   085E 00                 1241 	 nop; nop; nop; 
                           1242 ;Initial/src/lib/isr.c:145: EP8FIFOIRQ = 0xff;	SYNCDELAY;
                           1243 ;     genAssign
   085F 90 E6 57           1244 	mov	dptr,#_EP8FIFOIRQ
   0862 74 FF              1245 	mov	a,#0xFF
   0864 F0                 1246 	movx	@dptr,a
                           1247 ;     genInline
   0865 00                 1248 	 nop; nop; nop; 
                           1249 ;Initial/src/lib/isr.c:147: IBNIE  = 0;
                           1250 ;     genAssign
   0866 90 E6 58           1251 	mov	dptr,#_IBNIE
                           1252 ;	Peephole 181	changed mov to clr
   0869 E4                 1253 	clr	a
   086A F0                 1254 	movx	@dptr,a
                           1255 ;Initial/src/lib/isr.c:148: IBNIRQ = 0xff;
                           1256 ;     genAssign
   086B 90 E6 59           1257 	mov	dptr,#_IBNIRQ
   086E 74 FF              1258 	mov	a,#0xFF
   0870 F0                 1259 	movx	@dptr,a
                           1260 ;Initial/src/lib/isr.c:149: NAKIE  = 0;
                           1261 ;     genAssign
   0871 90 E6 5A           1262 	mov	dptr,#_NAKIE
                           1263 ;	Peephole 181	changed mov to clr
   0874 E4                 1264 	clr	a
   0875 F0                 1265 	movx	@dptr,a
                           1266 ;Initial/src/lib/isr.c:150: NAKIRQ = 0xff;
                           1267 ;     genAssign
   0876 90 E6 5B           1268 	mov	dptr,#_NAKIRQ
   0879 74 FF              1269 	mov	a,#0xFF
   087B F0                 1270 	movx	@dptr,a
                           1271 ;Initial/src/lib/isr.c:151: USBIE  = 0;
                           1272 ;     genAssign
   087C 90 E6 5C           1273 	mov	dptr,#_USBIE
                           1274 ;	Peephole 181	changed mov to clr
   087F E4                 1275 	clr	a
   0880 F0                 1276 	movx	@dptr,a
                           1277 ;Initial/src/lib/isr.c:152: USBIRQ = 0xff;
                           1278 ;     genAssign
   0881 90 E6 5D           1279 	mov	dptr,#_USBIRQ
   0884 74 FF              1280 	mov	a,#0xFF
   0886 F0                 1281 	movx	@dptr,a
                           1282 ;Initial/src/lib/isr.c:153: EPIE   = 0;
                           1283 ;     genAssign
   0887 90 E6 5E           1284 	mov	dptr,#_EPIE
                           1285 ;	Peephole 181	changed mov to clr
   088A E4                 1286 	clr	a
   088B F0                 1287 	movx	@dptr,a
                           1288 ;Initial/src/lib/isr.c:154: EPIRQ  = 0xff;
                           1289 ;     genAssign
   088C 90 E6 5F           1290 	mov	dptr,#_EPIRQ
   088F 74 FF              1291 	mov	a,#0xFF
   0891 F0                 1292 	movx	@dptr,a
                           1293 ;Initial/src/lib/isr.c:155: SYNCDELAY;	GPIFIE = 0;
                           1294 ;     genInline
   0892 00                 1295 	 nop; nop; nop; 
                           1296 ;     genAssign
   0893 90 E6 60           1297 	mov	dptr,#_GPIFIE
                           1298 ;	Peephole 181	changed mov to clr
   0896 E4                 1299 	clr	a
   0897 F0                 1300 	movx	@dptr,a
                           1301 ;Initial/src/lib/isr.c:156: SYNCDELAY;	GPIFIRQ = 0xff;
                           1302 ;     genInline
   0898 00                 1303 	 nop; nop; nop; 
                           1304 ;     genAssign
   0899 90 E6 61           1305 	mov	dptr,#_GPIFIRQ
   089C 74 FF              1306 	mov	a,#0xFF
   089E F0                 1307 	movx	@dptr,a
                           1308 ;Initial/src/lib/isr.c:157: USBERRIE = 0;
                           1309 ;     genAssign
   089F 90 E6 62           1310 	mov	dptr,#_USBERRIE
                           1311 ;	Peephole 181	changed mov to clr
   08A2 E4                 1312 	clr	a
   08A3 F0                 1313 	movx	@dptr,a
                           1314 ;Initial/src/lib/isr.c:158: USBERRIRQ = 0xff;
                           1315 ;     genAssign
   08A4 90 E6 63           1316 	mov	dptr,#_USBERRIRQ
   08A7 74 FF              1317 	mov	a,#0xFF
   08A9 F0                 1318 	movx	@dptr,a
                           1319 ;Initial/src/lib/isr.c:159: CLRERRCNT = 0;
                           1320 ;     genAssign
   08AA 90 E6 65           1321 	mov	dptr,#_CLRERRCNT
                           1322 ;	Peephole 181	changed mov to clr
   08AD E4                 1323 	clr	a
   08AE F0                 1324 	movx	@dptr,a
                           1325 ;Initial/src/lib/isr.c:161: INTSETUP = bmAV2EN | bmAV4EN | bmINT4IN;
                           1326 ;     genAssign
   08AF 90 E6 68           1327 	mov	dptr,#_INTSETUP
   08B2 74 0B              1328 	mov	a,#0x0B
   08B4 F0                 1329 	movx	@dptr,a
                           1330 ;Initial/src/lib/isr.c:164: EXIF &= ~bmEXIF_USBINT;
                           1331 ;     genAnd
                           1332 ;Initial/src/lib/isr.c:165: EXIF &= ~bmEXIF_IE4;
                           1333 ;     genAnd
                           1334 ;	Peephole 183	avoided anl during execution
   08B5 53 91 AF           1335 	anl	_EXIF,#(0xEF & 0xBF)
                           1336 ;Initial/src/lib/isr.c:168: EIUSB = 1;
                           1337 ;     genAssign
   08B8 D2 E8              1338 	setb	_EIUSB
                           1339 ;Initial/src/lib/isr.c:169: EIEX4 = 1;
                           1340 ;     genAssign
   08BA D2 EA              1341 	setb	_EIEX4
   08BC                    1342 00101$:
   08BC 22                 1343 	ret
                           1344 	.area CSEG    (CODE)
                           1345 	.area XINIT   (CODE)
