                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Thu Aug 17 13:31:57 2006
                              5 ;--------------------------------------------------------
                              6 	.module spi
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
                            323 	.globl _read_bytes_msb_PARM_2
                            324 	.globl _write_bytes_msb_PARM_2
                            325 	.globl _spi_write_PARM_6
                            326 	.globl _spi_write_PARM_5
                            327 	.globl _spi_write_PARM_4
                            328 	.globl _spi_write_PARM_3
                            329 	.globl _spi_write_PARM_2
                            330 	.globl _spi_read_PARM_6
                            331 	.globl _spi_read_PARM_5
                            332 	.globl _spi_read_PARM_4
                            333 	.globl _spi_read_PARM_3
                            334 	.globl _spi_read_PARM_2
                            335 	.globl _init_spi
                            336 	.globl _spi_read
                            337 	.globl _spi_write
                            338 ;--------------------------------------------------------
                            339 ; special function registers
                            340 ;--------------------------------------------------------
                            341 	.area RSEG    (DATA)
                    0080    342 _IOA	=	0x0080
                    0081    343 _SP	=	0x0081
                    0082    344 _DPL	=	0x0082
                    0083    345 _DPH	=	0x0083
                    0084    346 _DPL1	=	0x0084
                    0085    347 _DPH1	=	0x0085
                    0086    348 _DPS	=	0x0086
                    0087    349 _PCON	=	0x0087
                    0088    350 _TCON	=	0x0088
                    0089    351 _TMOD	=	0x0089
                    008A    352 _TL0	=	0x008a
                    008B    353 _TL1	=	0x008b
                    008C    354 _TH0	=	0x008c
                    008D    355 _TH1	=	0x008d
                    008E    356 _CKCON	=	0x008e
                    0090    357 _IOB	=	0x0090
                    0091    358 _EXIF	=	0x0091
                    0092    359 _MPAGE	=	0x0092
                    0098    360 _SCON0	=	0x0098
                    0099    361 _SBUF0	=	0x0099
                    009A    362 _APTR1H	=	0x009a
                    009B    363 _APTR1L	=	0x009b
                    009C    364 _AUTODAT1	=	0x009c
                    009D    365 _AUTOPTRH2	=	0x009d
                    009E    366 _AUTOPTRL2	=	0x009e
                    009F    367 _AUTODAT2	=	0x009f
                    00A0    368 _IOC	=	0x00a0
                    00A1    369 _INT2CLR	=	0x00a1
                    00A2    370 _INT4CLR	=	0x00a2
                    00A8    371 _IE	=	0x00a8
                    00AA    372 _EP2468STAT	=	0x00aa
                    00AB    373 _EP24FIFOFLGS	=	0x00ab
                    00AC    374 _EP68FIFOFLGS	=	0x00ac
                    00AF    375 _AUTOPTRSETUP	=	0x00af
                    00B0    376 _IOD	=	0x00b0
                    00B1    377 _IOE	=	0x00b1
                    00B2    378 _OEA	=	0x00b2
                    00B3    379 _OEB	=	0x00b3
                    00B4    380 _OEC	=	0x00b4
                    00B5    381 _OED	=	0x00b5
                    00B6    382 _OEE	=	0x00b6
                    00B8    383 _IP	=	0x00b8
                    00BA    384 _EP01STAT	=	0x00ba
                    00BB    385 _GPIFTRIG	=	0x00bb
                    00BD    386 _GPIFSGLDATH	=	0x00bd
                    00BE    387 _GPIFSGLDATLX	=	0x00be
                    00BF    388 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    389 _SCON1	=	0x00c0
                    00C1    390 _SBUF1	=	0x00c1
                    00C8    391 _T2CON	=	0x00c8
                    00CA    392 _RCAP2L	=	0x00ca
                    00CB    393 _RCAP2H	=	0x00cb
                    00CC    394 _TL2	=	0x00cc
                    00CD    395 _TH2	=	0x00cd
                    00D0    396 _PSW	=	0x00d0
                    00D8    397 _EICON	=	0x00d8
                    00E0    398 _ACC	=	0x00e0
                    00E8    399 _EIE	=	0x00e8
                    00F0    400 _B	=	0x00f0
                    00F8    401 _EIP	=	0x00f8
                            402 ;--------------------------------------------------------
                            403 ; special function bits 
                            404 ;--------------------------------------------------------
                            405 	.area RSEG    (DATA)
                    0086    406 _SEL	=	0x0086
                    0088    407 _IT0	=	0x0088
                    0089    408 _IE0	=	0x0089
                    008A    409 _IT1	=	0x008a
                    008B    410 _IE1	=	0x008b
                    008C    411 _TR0	=	0x008c
                    008D    412 _TF0	=	0x008d
                    008E    413 _TR1	=	0x008e
                    008F    414 _TF1	=	0x008f
                    0098    415 _RI	=	0x0098
                    0099    416 _TI	=	0x0099
                    009A    417 _RB8	=	0x009a
                    009B    418 _TB8	=	0x009b
                    009C    419 _REN	=	0x009c
                    009D    420 _SM2	=	0x009d
                    009E    421 _SM1	=	0x009e
                    009F    422 _SM0	=	0x009f
                    00A8    423 _EX0	=	0x00a8
                    00A9    424 _ET0	=	0x00a9
                    00AA    425 _EX1	=	0x00aa
                    00AB    426 _ET1	=	0x00ab
                    00AC    427 _ES0	=	0x00ac
                    00AD    428 _ET2	=	0x00ad
                    00AE    429 _ES1	=	0x00ae
                    00AF    430 _EA	=	0x00af
                    00B8    431 _PX0	=	0x00b8
                    00B9    432 _PT0	=	0x00b9
                    00BA    433 _PX1	=	0x00ba
                    00BB    434 _PT1	=	0x00bb
                    00BC    435 _PS0	=	0x00bc
                    00BD    436 _PT2	=	0x00bd
                    00BE    437 _PS1	=	0x00be
                    00C0    438 _RI1	=	0x00c0
                    00C1    439 _TI1	=	0x00c1
                    00C2    440 _RB81	=	0x00c2
                    00C3    441 _TB81	=	0x00c3
                    00C4    442 _REN1	=	0x00c4
                    00C5    443 _SM21	=	0x00c5
                    00C6    444 _SM11	=	0x00c6
                    00C7    445 _SM01	=	0x00c7
                    00C8    446 _CP_RL2	=	0x00c8
                    00C9    447 _C_T2	=	0x00c9
                    00CA    448 _TR2	=	0x00ca
                    00CB    449 _EXEN2	=	0x00cb
                    00CC    450 _TCLK	=	0x00cc
                    00CD    451 _RCLK	=	0x00cd
                    00CE    452 _EXF2	=	0x00ce
                    00CF    453 _TF2	=	0x00cf
                    00D0    454 _P	=	0x00d0
                    00D1    455 _FL	=	0x00d1
                    00D2    456 _OV	=	0x00d2
                    00D3    457 _RS0	=	0x00d3
                    00D4    458 _RS1	=	0x00d4
                    00D5    459 _F0	=	0x00d5
                    00D6    460 _AC	=	0x00d6
                    00D7    461 _CY	=	0x00d7
                    00DB    462 _INT6	=	0x00db
                    00DC    463 _RESI	=	0x00dc
                    00DD    464 _ERESI	=	0x00dd
                    00DF    465 _SMOD1	=	0x00df
                    00E8    466 _EIUSB	=	0x00e8
                    00E9    467 _EI2C	=	0x00e9
                    00EA    468 _EIEX4	=	0x00ea
                    00EB    469 _EIEX5	=	0x00eb
                    00EC    470 _EIEX6	=	0x00ec
                    00F8    471 _PUSB	=	0x00f8
                    00F9    472 _PI2C	=	0x00f9
                    00FA    473 _EIPX4	=	0x00fa
                    00FB    474 _EIPX5	=	0x00fb
                    00FC    475 _EIPX6	=	0x00fc
                    0080    476 _bitS_CLK	=	0x0080
                    0081    477 _bitS_OUT	=	0x0081
                    0083    478 _bitS_IN	=	0x0083
                    00A0    479 _bitALTERA_DATA0	=	0x00a0
                    00A2    480 _bitALTERA_DCLK	=	0x00a2
                            481 ;--------------------------------------------------------
                            482 ; overlayable register banks 
                            483 ;--------------------------------------------------------
                            484 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     485 	.ds 8
                            486 ;--------------------------------------------------------
                            487 ; internal ram data
                            488 ;--------------------------------------------------------
                            489 	.area DSEG    (DATA)
   0010                     490 _spi_read_PARM_2::
   0010                     491 	.ds 1
   0011                     492 _spi_read_PARM_3::
   0011                     493 	.ds 1
   0012                     494 _spi_read_PARM_4::
   0012                     495 	.ds 1
   0013                     496 _spi_read_PARM_5::
   0013                     497 	.ds 2
   0015                     498 _spi_read_PARM_6::
   0015                     499 	.ds 1
   0016                     500 _spi_write_PARM_2::
   0016                     501 	.ds 1
   0017                     502 _spi_write_PARM_3::
   0017                     503 	.ds 1
   0018                     504 _spi_write_PARM_4::
   0018                     505 	.ds 1
   0019                     506 _spi_write_PARM_5::
   0019                     507 	.ds 2
   001B                     508 _spi_write_PARM_6::
   001B                     509 	.ds 1
   001C                     510 _write_bytes_msb_PARM_2::
   001C                     511 	.ds 1
   001D                     512 _read_bytes_msb_PARM_2::
   001D                     513 	.ds 1
                            514 ;--------------------------------------------------------
                            515 ; overlayable items in internal ram 
                            516 ;--------------------------------------------------------
                            517 	.area	OSEG    (OVR,DATA)
                            518 	.area	OSEG    (OVR,DATA)
                            519 	.area	OSEG    (OVR,DATA)
                            520 ;--------------------------------------------------------
                            521 ; indirectly addressable internal ram data
                            522 ;--------------------------------------------------------
                            523 	.area ISEG    (DATA)
                            524 ;--------------------------------------------------------
                            525 ; bit data
                            526 ;--------------------------------------------------------
                            527 	.area BSEG    (BIT)
                            528 ;--------------------------------------------------------
                            529 ; paged external ram data
                            530 ;--------------------------------------------------------
                            531 	.area PSEG    (PAG,XDATA)
                            532 ;--------------------------------------------------------
                            533 ; external ram data
                            534 ;--------------------------------------------------------
                            535 	.area XSEG    (XDATA)
                    E400    536 _GPIF_WAVE_DATA	=	0xe400
                    E480    537 _RES_WAVEDATA_END	=	0xe480
                    E600    538 _CPUCS	=	0xe600
                    E601    539 _IFCONFIG	=	0xe601
                    E602    540 _PINFLAGSAB	=	0xe602
                    E603    541 _PINFLAGSCD	=	0xe603
                    E604    542 _FIFORESET	=	0xe604
                    E605    543 _BREAKPT	=	0xe605
                    E606    544 _BPADDRH	=	0xe606
                    E607    545 _BPADDRL	=	0xe607
                    E608    546 _UART230	=	0xe608
                    E609    547 _FIFOPINPOLAR	=	0xe609
                    E60A    548 _REVID	=	0xe60a
                    E60B    549 _REVCTL	=	0xe60b
                    E610    550 _EP1OUTCFG	=	0xe610
                    E611    551 _EP1INCFG	=	0xe611
                    E612    552 _EP2CFG	=	0xe612
                    E613    553 _EP4CFG	=	0xe613
                    E614    554 _EP6CFG	=	0xe614
                    E615    555 _EP8CFG	=	0xe615
                    E618    556 _EP2FIFOCFG	=	0xe618
                    E619    557 _EP4FIFOCFG	=	0xe619
                    E61A    558 _EP6FIFOCFG	=	0xe61a
                    E61B    559 _EP8FIFOCFG	=	0xe61b
                    E620    560 _EP2AUTOINLENH	=	0xe620
                    E621    561 _EP2AUTOINLENL	=	0xe621
                    E622    562 _EP4AUTOINLENH	=	0xe622
                    E623    563 _EP4AUTOINLENL	=	0xe623
                    E624    564 _EP6AUTOINLENH	=	0xe624
                    E625    565 _EP6AUTOINLENL	=	0xe625
                    E626    566 _EP8AUTOINLENH	=	0xe626
                    E627    567 _EP8AUTOINLENL	=	0xe627
                    E630    568 _EP2FIFOPFH	=	0xe630
                    E631    569 _EP2FIFOPFL	=	0xe631
                    E632    570 _EP4FIFOPFH	=	0xe632
                    E633    571 _EP4FIFOPFL	=	0xe633
                    E634    572 _EP6FIFOPFH	=	0xe634
                    E635    573 _EP6FIFOPFL	=	0xe635
                    E636    574 _EP8FIFOPFH	=	0xe636
                    E637    575 _EP8FIFOPFL	=	0xe637
                    E640    576 _EP2ISOINPKTS	=	0xe640
                    E641    577 _EP4ISOINPKTS	=	0xe641
                    E642    578 _EP6ISOINPKTS	=	0xe642
                    E643    579 _EP8ISOINPKTS	=	0xe643
                    E648    580 _INPKTEND	=	0xe648
                    E649    581 _OUTPKTEND	=	0xe649
                    E650    582 _EP2FIFOIE	=	0xe650
                    E651    583 _EP2FIFOIRQ	=	0xe651
                    E652    584 _EP4FIFOIE	=	0xe652
                    E653    585 _EP4FIFOIRQ	=	0xe653
                    E654    586 _EP6FIFOIE	=	0xe654
                    E655    587 _EP6FIFOIRQ	=	0xe655
                    E656    588 _EP8FIFOIE	=	0xe656
                    E657    589 _EP8FIFOIRQ	=	0xe657
                    E658    590 _IBNIE	=	0xe658
                    E659    591 _IBNIRQ	=	0xe659
                    E65A    592 _NAKIE	=	0xe65a
                    E65B    593 _NAKIRQ	=	0xe65b
                    E65C    594 _USBIE	=	0xe65c
                    E65D    595 _USBIRQ	=	0xe65d
                    E65E    596 _EPIE	=	0xe65e
                    E65F    597 _EPIRQ	=	0xe65f
                    E660    598 _GPIFIE	=	0xe660
                    E661    599 _GPIFIRQ	=	0xe661
                    E662    600 _USBERRIE	=	0xe662
                    E663    601 _USBERRIRQ	=	0xe663
                    E664    602 _ERRCNTLIM	=	0xe664
                    E665    603 _CLRERRCNT	=	0xe665
                    E666    604 _INT2IVEC	=	0xe666
                    E667    605 _INT4IVEC	=	0xe667
                    E668    606 _INTSETUP	=	0xe668
                    E670    607 _PORTACFG	=	0xe670
                    E671    608 _PORTCCFG	=	0xe671
                    E672    609 _PORTECFG	=	0xe672
                    E678    610 _I2CS	=	0xe678
                    E679    611 _I2DAT	=	0xe679
                    E67A    612 _I2CTL	=	0xe67a
                    E67B    613 _XAUTODAT1	=	0xe67b
                    E67C    614 _XAUTODAT2	=	0xe67c
                    E680    615 _USBCS	=	0xe680
                    E681    616 _SUSPEND	=	0xe681
                    E682    617 _WAKEUPCS	=	0xe682
                    E683    618 _TOGCTL	=	0xe683
                    E684    619 _USBFRAMEH	=	0xe684
                    E685    620 _USBFRAMEL	=	0xe685
                    E686    621 _MICROFRAME	=	0xe686
                    E687    622 _FNADDR	=	0xe687
                    E68A    623 _EP0BCH	=	0xe68a
                    E68B    624 _EP0BCL	=	0xe68b
                    E68D    625 _EP1OUTBC	=	0xe68d
                    E68F    626 _EP1INBC	=	0xe68f
                    E690    627 _EP2BCH	=	0xe690
                    E691    628 _EP2BCL	=	0xe691
                    E694    629 _EP4BCH	=	0xe694
                    E695    630 _EP4BCL	=	0xe695
                    E698    631 _EP6BCH	=	0xe698
                    E699    632 _EP6BCL	=	0xe699
                    E69C    633 _EP8BCH	=	0xe69c
                    E69D    634 _EP8BCL	=	0xe69d
                    E6A0    635 _EP0CS	=	0xe6a0
                    E6A1    636 _EP1OUTCS	=	0xe6a1
                    E6A2    637 _EP1INCS	=	0xe6a2
                    E6A3    638 _EP2CS	=	0xe6a3
                    E6A4    639 _EP4CS	=	0xe6a4
                    E6A5    640 _EP6CS	=	0xe6a5
                    E6A6    641 _EP8CS	=	0xe6a6
                    E6A7    642 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    643 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    644 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    645 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    646 _EP2FIFOBCH	=	0xe6ab
                    E6AC    647 _EP2FIFOBCL	=	0xe6ac
                    E6AD    648 _EP4FIFOBCH	=	0xe6ad
                    E6AE    649 _EP4FIFOBCL	=	0xe6ae
                    E6AF    650 _EP6FIFOBCH	=	0xe6af
                    E6B0    651 _EP6FIFOBCL	=	0xe6b0
                    E6B1    652 _EP8FIFOBCH	=	0xe6b1
                    E6B2    653 _EP8FIFOBCL	=	0xe6b2
                    E6B3    654 _SUDPTRH	=	0xe6b3
                    E6B4    655 _SUDPTRL	=	0xe6b4
                    E6B5    656 _SUDPTRCTL	=	0xe6b5
                    E6B8    657 _SETUPDAT	=	0xe6b8
                    E6C0    658 _GPIFWFSELECT	=	0xe6c0
                    E6C1    659 _GPIFIDLECS	=	0xe6c1
                    E6C2    660 _GPIFIDLECTL	=	0xe6c2
                    E6C3    661 _GPIFCTLCFG	=	0xe6c3
                    E6C4    662 _GPIFADRH	=	0xe6c4
                    E6C5    663 _GPIFADRL	=	0xe6c5
                    E6CE    664 _GPIFTCB3	=	0xe6ce
                    E6CF    665 _GPIFTCB2	=	0xe6cf
                    E6D0    666 _GPIFTCB1	=	0xe6d0
                    E6D1    667 _GPIFTCB0	=	0xe6d1
                    E6D2    668 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    669 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    670 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    671 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    672 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    673 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    674 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    675 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    676 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    677 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    678 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    679 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    680 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    681 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    682 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    683 _GPIFREADYCFG	=	0xe6f3
                    E6F4    684 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    685 _GPIFABORT	=	0xe6f5
                    E6C6    686 _FLOWSTATE	=	0xe6c6
                    E6C7    687 _FLOWLOGIC	=	0xe6c7
                    E6C8    688 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    689 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    690 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    691 _FLOWSTB	=	0xe6cb
                    E6CC    692 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    693 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    694 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    695 _UDMACRCH	=	0xe67d
                    E67E    696 _UDMACRCL	=	0xe67e
                    E67F    697 _UDMACRCQUAL	=	0xe67f
                    E6F8    698 _DBUG	=	0xe6f8
                    E6F9    699 _TESTCFG	=	0xe6f9
                    E6FA    700 _USBTEST	=	0xe6fa
                    E6FB    701 _CT1	=	0xe6fb
                    E6FC    702 _CT2	=	0xe6fc
                    E6FD    703 _CT3	=	0xe6fd
                    E6FE    704 _CT4	=	0xe6fe
                    E740    705 _EP0BUF	=	0xe740
                    E780    706 _EP1OUTBUF	=	0xe780
                    E7C0    707 _EP1INBUF	=	0xe7c0
                    F000    708 _EP2FIFOBUF	=	0xf000
                    F400    709 _EP4FIFOBUF	=	0xf400
                    F800    710 _EP6FIFOBUF	=	0xf800
                    FC00    711 _EP8FIFOBUF	=	0xfc00
                            712 ;--------------------------------------------------------
                            713 ; external initialized ram data
                            714 ;--------------------------------------------------------
                            715 	.area CSEG    (CODE)
                            716 	.area GSINIT0 (CODE)
                            717 	.area GSINIT1 (CODE)
                            718 	.area GSINIT2 (CODE)
                            719 	.area GSINIT3 (CODE)
                            720 	.area GSINIT4 (CODE)
                            721 	.area GSINIT5 (CODE)
                            722 ;--------------------------------------------------------
                            723 ; global & static initialisations
                            724 ;--------------------------------------------------------
                            725 	.area CSEG    (CODE)
                            726 	.area GSINIT  (CODE)
                            727 	.area GSFINAL (CODE)
                            728 	.area GSINIT  (CODE)
                            729 ;--------------------------------------------------------
                            730 ; Home
                            731 ;--------------------------------------------------------
                            732 	.area HOME    (CODE)
                            733 	.area CSEG    (CODE)
                            734 ;--------------------------------------------------------
                            735 ; code
                            736 ;--------------------------------------------------------
                            737 	.area CSEG    (CODE)
                            738 ;------------------------------------------------------------
                            739 ;Allocation info for local variables in function 'setup_enables'
                            740 ;------------------------------------------------------------
                            741 ;enables                   Allocated to registers r2 
                            742 ;------------------------------------------------------------
                            743 ;Initial/src/lib/spi.c:30: setup_enables (unsigned char enables)
                            744 ;	-----------------------------------------
                            745 ;	 function setup_enables
                            746 ;	-----------------------------------------
   09F3                     747 _setup_enables:
                    0002    748 	ar2 = 0x02
                    0003    749 	ar3 = 0x03
                    0004    750 	ar4 = 0x04
                    0005    751 	ar5 = 0x05
                    0006    752 	ar6 = 0x06
                    0007    753 	ar7 = 0x07
                    0000    754 	ar0 = 0x00
                    0001    755 	ar1 = 0x01
                            756 ;     genReceive
   09F3 AA 82               757 	mov	r2,dpl
                            758 ;Initial/src/lib/spi.c:32: if (enables == 0)
                            759 ;     genCmpEq
                            760 ;	Peephole 112.b	changed ljmp to sjmp
                            761 ;	Peephole 199	optimized misc jump sequence
   09F5 BA 00 04            762 	cjne	r2,#0x00,00102$
                            763 ;00107$:
                            764 ;	Peephole 200	removed redundant sjmp
   09F8                     765 00108$:
                            766 ;Initial/src/lib/spi.c:33: HPSDR_PE = 0x0;
                            767 ;     genAssign
   09F8 75 B1 00            768 	mov	_IOE,#0x00
                            769 ;	Peephole 112.b	changed ljmp to sjmp
                            770 ;	Peephole 251.b	replaced sjmp to ret with ret
   09FB 22                  771 	ret
   09FC                     772 00102$:
                            773 ;Initial/src/lib/spi.c:35: HPSDR_PE = HPSDR_PE | enables;
                            774 ;     genOr
   09FC EA                  775 	mov	a,r2
   09FD 42 B1               776 	orl	_IOE,a
   09FF                     777 00104$:
   09FF 22                  778 	ret
                            779 ;------------------------------------------------------------
                            780 ;Allocation info for local variables in function 'init_spi'
                            781 ;------------------------------------------------------------
                            782 ;------------------------------------------------------------
                            783 ;Initial/src/lib/spi.c:41: init_spi (void)
                            784 ;	-----------------------------------------
                            785 ;	 function init_spi
                            786 ;	-----------------------------------------
   0A00                     787 _init_spi:
                            788 ;Initial/src/lib/spi.c:43: disable_all ();		/* disable all devs	  */
                            789 ;     genCall
   0A00 75 82 00            790 	mov	dpl,#0x00
   0A03 12 09 F3            791 	lcall	_setup_enables
                            792 ;Initial/src/lib/spi.c:44: bitS_OUT = 0;			/* idle state has CLK = 0 */
                            793 ;     genAssign
   0A06 C2 81               794 	clr	_bitS_OUT
   0A08                     795 00101$:
   0A08 22                  796 	ret
                            797 ;------------------------------------------------------------
                            798 ;Allocation info for local variables in function 'count_bits8'
                            799 ;------------------------------------------------------------
                            800 ;v                         Allocated to registers r2 
                            801 ;count                     Allocated to registers r3 
                            802 ;------------------------------------------------------------
                            803 ;Initial/src/lib/spi.c:75: count_bits8 (unsigned char v)
                            804 ;	-----------------------------------------
                            805 ;	 function count_bits8
                            806 ;	-----------------------------------------
   0A09                     807 _count_bits8:
                            808 ;     genReceive
   0A09 AA 82               809 	mov	r2,dpl
                            810 ;Initial/src/lib/spi.c:77: unsigned char count = 0;
                            811 ;     genAssign
   0A0B 7B 00               812 	mov	r3,#0x00
                            813 ;Initial/src/lib/spi.c:78: if (v & (1 << 0)) count++;
                            814 ;     genAnd
   0A0D EA                  815 	mov	a,r2
                            816 ;     genIfxJump
                            817 ;	Peephole 111	removed ljmp by inverse jump logic
   0A0E 30 E0 02            818 	jnb	acc.0,00102$
   0A11                     819 00127$:
                            820 ;     genAssign
   0A11 7B 01               821 	mov	r3,#0x01
   0A13                     822 00102$:
                            823 ;Initial/src/lib/spi.c:79: if (v & (1 << 1)) count++;
                            824 ;     genAnd
   0A13 EA                  825 	mov	a,r2
                            826 ;     genIfxJump
                            827 ;	Peephole 111	removed ljmp by inverse jump logic
   0A14 30 E1 01            828 	jnb	acc.1,00104$
   0A17                     829 00128$:
                            830 ;     genPlus
                            831 ;     genPlusIncr
   0A17 0B                  832 	inc	r3
   0A18                     833 00104$:
                            834 ;Initial/src/lib/spi.c:80: if (v & (1 << 2)) count++;
                            835 ;     genAnd
   0A18 EA                  836 	mov	a,r2
                            837 ;     genIfxJump
                            838 ;	Peephole 111	removed ljmp by inverse jump logic
   0A19 30 E2 01            839 	jnb	acc.2,00106$
   0A1C                     840 00129$:
                            841 ;     genPlus
                            842 ;     genPlusIncr
   0A1C 0B                  843 	inc	r3
   0A1D                     844 00106$:
                            845 ;Initial/src/lib/spi.c:81: if (v & (1 << 3)) count++;
                            846 ;     genAnd
   0A1D EA                  847 	mov	a,r2
                            848 ;     genIfxJump
                            849 ;	Peephole 111	removed ljmp by inverse jump logic
   0A1E 30 E3 01            850 	jnb	acc.3,00108$
   0A21                     851 00130$:
                            852 ;     genPlus
                            853 ;     genPlusIncr
   0A21 0B                  854 	inc	r3
   0A22                     855 00108$:
                            856 ;Initial/src/lib/spi.c:82: if (v & (1 << 4)) count++;
                            857 ;     genAnd
   0A22 EA                  858 	mov	a,r2
                            859 ;     genIfxJump
                            860 ;	Peephole 111	removed ljmp by inverse jump logic
   0A23 30 E4 01            861 	jnb	acc.4,00110$
   0A26                     862 00131$:
                            863 ;     genPlus
                            864 ;     genPlusIncr
   0A26 0B                  865 	inc	r3
   0A27                     866 00110$:
                            867 ;Initial/src/lib/spi.c:83: if (v & (1 << 5)) count++;
                            868 ;     genAnd
   0A27 EA                  869 	mov	a,r2
                            870 ;     genIfxJump
                            871 ;	Peephole 111	removed ljmp by inverse jump logic
   0A28 30 E5 01            872 	jnb	acc.5,00112$
   0A2B                     873 00132$:
                            874 ;     genPlus
                            875 ;     genPlusIncr
   0A2B 0B                  876 	inc	r3
   0A2C                     877 00112$:
                            878 ;Initial/src/lib/spi.c:84: if (v & (1 << 6)) count++;
                            879 ;     genAnd
   0A2C EA                  880 	mov	a,r2
                            881 ;     genIfxJump
                            882 ;	Peephole 111	removed ljmp by inverse jump logic
   0A2D 30 E6 01            883 	jnb	acc.6,00114$
   0A30                     884 00133$:
                            885 ;     genPlus
                            886 ;     genPlusIncr
   0A30 0B                  887 	inc	r3
   0A31                     888 00114$:
                            889 ;Initial/src/lib/spi.c:85: if (v & (1 << 7)) count++;
                            890 ;     genAnd
   0A31 EA                  891 	mov	a,r2
                            892 ;     genIfxJump
                            893 ;	Peephole 111	removed ljmp by inverse jump logic
   0A32 30 E7 01            894 	jnb	acc.7,00116$
   0A35                     895 00134$:
                            896 ;     genPlus
                            897 ;     genPlusIncr
   0A35 0B                  898 	inc	r3
   0A36                     899 00116$:
                            900 ;Initial/src/lib/spi.c:86: return count;
                            901 ;     genRet
   0A36 8B 82               902 	mov	dpl,r3
   0A38                     903 00117$:
   0A38 22                  904 	ret
                            905 ;------------------------------------------------------------
                            906 ;Allocation info for local variables in function 'spi_read'
                            907 ;------------------------------------------------------------
                            908 ;header_lo                 Allocated with name '_spi_read_PARM_2'
                            909 ;enables                   Allocated with name '_spi_read_PARM_3'
                            910 ;format                    Allocated with name '_spi_read_PARM_4'
                            911 ;buf                       Allocated with name '_spi_read_PARM_5'
                            912 ;len                       Allocated with name '_spi_read_PARM_6'
                            913 ;header_hi                 Allocated to registers r2 
                            914 ;------------------------------------------------------------
                            915 ;Initial/src/lib/spi.c:102: spi_read (unsigned char header_hi, unsigned char header_lo,
                            916 ;	-----------------------------------------
                            917 ;	 function spi_read
                            918 ;	-----------------------------------------
   0A39                     919 _spi_read:
                            920 ;     genReceive
   0A39 AA 82               921 	mov	r2,dpl
                            922 ;Initial/src/lib/spi.c:106: if (count_bits8 (enables) > 1)
                            923 ;     genCall
   0A3B 85 11 82            924 	mov	dpl,_spi_read_PARM_3
   0A3E C0 02               925 	push	ar2
   0A40 12 0A 09            926 	lcall	_count_bits8
   0A43 AB 82               927 	mov	r3,dpl
   0A45 D0 02               928 	pop	ar2
                            929 ;     genCmpGt
                            930 ;     genCmp
                            931 ;     genIfxJump
                            932 ;	Peephole 108	removed ljmp by inverse jump logic
                            933 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   0A47 EB                  934 	mov	a,r3
   0A48 24 FE               935 	add	a,#0xff - 0x01
   0A4A 50 04               936 	jnc	00102$
   0A4C                     937 00121$:
                            938 ;Initial/src/lib/spi.c:107: return 0;		// error, too many enables set
                            939 ;     genRet
   0A4C 75 82 00            940 	mov	dpl,#0x00
                            941 ;	Peephole 112.b	changed ljmp to sjmp
                            942 ;	Peephole 251.b	replaced sjmp to ret with ret
   0A4F 22                  943 	ret
   0A50                     944 00102$:
                            945 ;Initial/src/lib/spi.c:109: setup_enables (enables);
                            946 ;     genCall
   0A50 85 11 82            947 	mov	dpl,_spi_read_PARM_3
   0A53 C0 02               948 	push	ar2
   0A55 12 09 F3            949 	lcall	_setup_enables
   0A58 D0 02               950 	pop	ar2
                            951 ;Initial/src/lib/spi.c:111: if (format & SPI_FMT_LSB){		// order: LSB
                            952 ;     genAnd
   0A5A E5 12               953 	mov	a,_spi_read_PARM_4
                            954 ;     genIfxJump
                            955 ;	Peephole 111	removed ljmp by inverse jump logic
   0A5C 30 E7 04            956 	jnb	acc.7,00111$
   0A5F                     957 00122$:
                            958 ;Initial/src/lib/spi.c:113: return 0;		// error, not implemented
                            959 ;     genRet
   0A5F 75 82 00            960 	mov	dpl,#0x00
                            961 ;	Peephole 112.b	changed ljmp to sjmp
                            962 ;	Peephole 251.b	replaced sjmp to ret with ret
   0A62 22                  963 	ret
   0A63                     964 00111$:
                            965 ;Initial/src/lib/spi.c:135: switch (format & SPI_FMT_HDR_MASK){
                            966 ;     genAnd
   0A63 74 60               967 	mov	a,#0x60
   0A65 55 12               968 	anl	a,_spi_read_PARM_4
                            969 ;     genCmpEq
                            970 ;	Peephole 112.b	changed ljmp to sjmp
                            971 ;	Peephole 115.b	jump optimization
   0A67 FB                  972 	mov	r3,a
   0A68 60 23               973 	jz	00107$
   0A6A                     974 00123$:
                            975 ;     genCmpEq
   0A6A BB 20 02            976 	cjne	r3,#0x20,00124$
                            977 ;	Peephole 112.b	changed ljmp to sjmp
   0A6D 80 05               978 	sjmp	00104$
   0A6F                     979 00124$:
                            980 ;     genCmpEq
                            981 ;	Peephole 112.b	changed ljmp to sjmp
                            982 ;Initial/src/lib/spi.c:138: case SPI_FMT_HDR_1:
                            983 ;	Peephole 112.b	changed ljmp to sjmp
                            984 ;	Peephole 199	optimized misc jump sequence
   0A6F BB 40 17            985 	cjne	r3,#0x40,00106$
   0A72 80 08               986 	sjmp	00105$
                            987 ;00125$:
   0A74                     988 00104$:
                            989 ;Initial/src/lib/spi.c:139: write_byte_msb (header_lo);
                            990 ;     genCall
   0A74 85 10 82            991 	mov	dpl,_spi_read_PARM_2
   0A77 12 0B 00            992 	lcall	_write_byte_msb
                            993 ;Initial/src/lib/spi.c:140: break;
                            994 ;Initial/src/lib/spi.c:141: case SPI_FMT_HDR_2:
                            995 ;	Peephole 112.b	changed ljmp to sjmp
   0A7A 80 11               996 	sjmp	00107$
   0A7C                     997 00105$:
                            998 ;Initial/src/lib/spi.c:142: write_byte_msb (header_hi);
                            999 ;     genCall
   0A7C 8A 82              1000 	mov	dpl,r2
   0A7E 12 0B 00           1001 	lcall	_write_byte_msb
                           1002 ;Initial/src/lib/spi.c:143: write_byte_msb (header_lo);
                           1003 ;     genCall
   0A81 85 10 82           1004 	mov	dpl,_spi_read_PARM_2
   0A84 12 0B 00           1005 	lcall	_write_byte_msb
                           1006 ;Initial/src/lib/spi.c:144: break;
                           1007 ;Initial/src/lib/spi.c:145: default:
                           1008 ;	Peephole 112.b	changed ljmp to sjmp
   0A87 80 04              1009 	sjmp	00107$
   0A89                    1010 00106$:
                           1011 ;Initial/src/lib/spi.c:146: return 0;		// error
                           1012 ;     genRet
   0A89 75 82 00           1013 	mov	dpl,#0x00
                           1014 ;Initial/src/lib/spi.c:147: }
                           1015 ;	Peephole 112.b	changed ljmp to sjmp
                           1016 ;	Peephole 251.b	replaced sjmp to ret with ret
   0A8C 22                 1017 	ret
   0A8D                    1018 00107$:
                           1019 ;Initial/src/lib/spi.c:148: if (len != 0)
                           1020 ;     genCmpEq
   0A8D E5 15              1021 	mov	a,_spi_read_PARM_6
                           1022 ;	Peephole 110	removed ljmp by inverse jump logic
   0A8F 60 0C              1023 	jz	00112$
   0A91                    1024 00126$:
                           1025 ;Initial/src/lib/spi.c:149: read_bytes_msb (buf, len);
                           1026 ;     genAssign
   0A91 85 15 1D           1027 	mov	_read_bytes_msb_PARM_2,_spi_read_PARM_6
                           1028 ;     genCall
   0A94 85 13 82           1029 	mov	dpl,_spi_read_PARM_5
   0A97 85 14 83           1030 	mov	dph,(_spi_read_PARM_5 + 1)
   0A9A 12 0B BB           1031 	lcall	_read_bytes_msb
   0A9D                    1032 00112$:
                           1033 ;Initial/src/lib/spi.c:152: disable_all ();
                           1034 ;     genCall
   0A9D 75 82 00           1035 	mov	dpl,#0x00
   0AA0 12 09 F3           1036 	lcall	_setup_enables
                           1037 ;Initial/src/lib/spi.c:153: return 1;		// success
                           1038 ;     genRet
   0AA3 75 82 01           1039 	mov	dpl,#0x01
   0AA6                    1040 00113$:
   0AA6 22                 1041 	ret
                           1042 ;------------------------------------------------------------
                           1043 ;Allocation info for local variables in function 'spi_write'
                           1044 ;------------------------------------------------------------
                           1045 ;header_lo                 Allocated with name '_spi_write_PARM_2'
                           1046 ;enables                   Allocated with name '_spi_write_PARM_3'
                           1047 ;format                    Allocated with name '_spi_write_PARM_4'
                           1048 ;buf                       Allocated with name '_spi_write_PARM_5'
                           1049 ;len                       Allocated with name '_spi_write_PARM_6'
                           1050 ;header_hi                 Allocated to registers r2 
                           1051 ;------------------------------------------------------------
                           1052 ;Initial/src/lib/spi.c:159: spi_write (unsigned char header_hi, unsigned char header_lo,
                           1053 ;	-----------------------------------------
                           1054 ;	 function spi_write
                           1055 ;	-----------------------------------------
   0AA7                    1056 _spi_write:
                           1057 ;     genReceive
   0AA7 AA 82              1058 	mov	r2,dpl
                           1059 ;Initial/src/lib/spi.c:163: setup_enables (enables);
                           1060 ;     genCall
   0AA9 85 17 82           1061 	mov	dpl,_spi_write_PARM_3
   0AAC C0 02              1062 	push	ar2
   0AAE 12 09 F3           1063 	lcall	_setup_enables
   0AB1 D0 02              1064 	pop	ar2
                           1065 ;Initial/src/lib/spi.c:165: if (format & SPI_FMT_LSB){		// order: LSB
                           1066 ;     genAnd
   0AB3 E5 18              1067 	mov	a,_spi_write_PARM_4
                           1068 ;     genIfxJump
                           1069 ;	Peephole 111	removed ljmp by inverse jump logic
   0AB5 30 E7 04           1070 	jnb	acc.7,00109$
   0AB8                    1071 00118$:
                           1072 ;Initial/src/lib/spi.c:167: return 0;		// error, not implemented
                           1073 ;     genRet
   0AB8 75 82 00           1074 	mov	dpl,#0x00
                           1075 ;	Peephole 112.b	changed ljmp to sjmp
                           1076 ;	Peephole 251.b	replaced sjmp to ret with ret
   0ABB 22                 1077 	ret
   0ABC                    1078 00109$:
                           1079 ;Initial/src/lib/spi.c:189: switch (format & SPI_FMT_HDR_MASK){
                           1080 ;     genAnd
   0ABC 74 60              1081 	mov	a,#0x60
   0ABE 55 18              1082 	anl	a,_spi_write_PARM_4
                           1083 ;     genCmpEq
                           1084 ;	Peephole 112.b	changed ljmp to sjmp
                           1085 ;	Peephole 115.b	jump optimization
   0AC0 FB                 1086 	mov	r3,a
   0AC1 60 23              1087 	jz	00105$
   0AC3                    1088 00119$:
                           1089 ;     genCmpEq
   0AC3 BB 20 02           1090 	cjne	r3,#0x20,00120$
                           1091 ;	Peephole 112.b	changed ljmp to sjmp
   0AC6 80 05              1092 	sjmp	00102$
   0AC8                    1093 00120$:
                           1094 ;     genCmpEq
                           1095 ;	Peephole 112.b	changed ljmp to sjmp
                           1096 ;Initial/src/lib/spi.c:192: case SPI_FMT_HDR_1:
                           1097 ;	Peephole 112.b	changed ljmp to sjmp
                           1098 ;	Peephole 199	optimized misc jump sequence
   0AC8 BB 40 17           1099 	cjne	r3,#0x40,00104$
   0ACB 80 08              1100 	sjmp	00103$
                           1101 ;00121$:
   0ACD                    1102 00102$:
                           1103 ;Initial/src/lib/spi.c:193: write_byte_msb (header_lo);
                           1104 ;     genCall
   0ACD 85 16 82           1105 	mov	dpl,_spi_write_PARM_2
   0AD0 12 0B 00           1106 	lcall	_write_byte_msb
                           1107 ;Initial/src/lib/spi.c:194: break;
                           1108 ;Initial/src/lib/spi.c:195: case SPI_FMT_HDR_2:
                           1109 ;	Peephole 112.b	changed ljmp to sjmp
   0AD3 80 11              1110 	sjmp	00105$
   0AD5                    1111 00103$:
                           1112 ;Initial/src/lib/spi.c:196: write_byte_msb (header_hi);
                           1113 ;     genCall
   0AD5 8A 82              1114 	mov	dpl,r2
   0AD7 12 0B 00           1115 	lcall	_write_byte_msb
                           1116 ;Initial/src/lib/spi.c:197: write_byte_msb (header_lo);
                           1117 ;     genCall
   0ADA 85 16 82           1118 	mov	dpl,_spi_write_PARM_2
   0ADD 12 0B 00           1119 	lcall	_write_byte_msb
                           1120 ;Initial/src/lib/spi.c:198: break;
                           1121 ;Initial/src/lib/spi.c:199: default:
                           1122 ;	Peephole 112.b	changed ljmp to sjmp
   0AE0 80 04              1123 	sjmp	00105$
   0AE2                    1124 00104$:
                           1125 ;Initial/src/lib/spi.c:200: return 0;		// error
                           1126 ;     genRet
   0AE2 75 82 00           1127 	mov	dpl,#0x00
                           1128 ;Initial/src/lib/spi.c:201: }
                           1129 ;	Peephole 112.b	changed ljmp to sjmp
                           1130 ;	Peephole 251.b	replaced sjmp to ret with ret
   0AE5 22                 1131 	ret
   0AE6                    1132 00105$:
                           1133 ;Initial/src/lib/spi.c:202: if (len != 0)
                           1134 ;     genCmpEq
   0AE6 E5 1B              1135 	mov	a,_spi_write_PARM_6
                           1136 ;	Peephole 110	removed ljmp by inverse jump logic
   0AE8 60 0C              1137 	jz	00110$
   0AEA                    1138 00122$:
                           1139 ;Initial/src/lib/spi.c:203: write_bytes_msb (buf, len);
                           1140 ;     genAssign
   0AEA 85 1B 1C           1141 	mov	_write_bytes_msb_PARM_2,_spi_write_PARM_6
                           1142 ;     genCall
   0AED 85 19 82           1143 	mov	dpl,_spi_write_PARM_5
   0AF0 85 1A 83           1144 	mov	dph,(_spi_write_PARM_5 + 1)
   0AF3 12 0B 53           1145 	lcall	_write_bytes_msb
   0AF6                    1146 00110$:
                           1147 ;Initial/src/lib/spi.c:206: disable_all ();
                           1148 ;     genCall
   0AF6 75 82 00           1149 	mov	dpl,#0x00
   0AF9 12 09 F3           1150 	lcall	_setup_enables
                           1151 ;Initial/src/lib/spi.c:207: return 1;		// success
                           1152 ;     genRet
   0AFC 75 82 01           1153 	mov	dpl,#0x01
   0AFF                    1154 00111$:
   0AFF 22                 1155 	ret
                           1156 ;------------------------------------------------------------
                           1157 ;Allocation info for local variables in function 'write_byte_msb'
                           1158 ;------------------------------------------------------------
                           1159 ;v                         Allocated to registers r2 
                           1160 ;------------------------------------------------------------
                           1161 ;Initial/src/lib/spi.c:213: write_byte_msb (unsigned char v)
                           1162 ;	-----------------------------------------
                           1163 ;	 function write_byte_msb
                           1164 ;	-----------------------------------------
   0B00                    1165 _write_byte_msb:
                           1166 ;     genReceive
                           1167 ;Initial/src/lib/spi.c:215: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1168 ;     genRLC
                           1169 ;	peephole 177.g	optimized mov sequence
   0B00 E5 82              1170 	mov	a,dpl
   0B02 FA                 1171 	mov	r2,a
   0B03 23                 1172 	rl	a
                           1173 ;Initial/src/lib/spi.c:216: bitS_OUT = v & 0x1;
                           1174 ;     genAnd
                           1175 ;	Peephole 105	removed redundant mov
   0B04 FA                 1176 	mov	r2,a
   0B05 13                 1177 	rrc	a
   0B06 92 81              1178 	mov	_bitS_OUT,c
                           1179 ;Initial/src/lib/spi.c:217: bitS_CLK = 1;
                           1180 ;     genAssign
   0B08 D2 80              1181 	setb	_bitS_CLK
                           1182 ;Initial/src/lib/spi.c:218: bitS_CLK = 0;
                           1183 ;     genAssign
   0B0A C2 80              1184 	clr	_bitS_CLK
                           1185 ;Initial/src/lib/spi.c:220: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1186 ;     genRLC
   0B0C EA                 1187 	mov	a,r2
   0B0D 23                 1188 	rl	a
                           1189 ;Initial/src/lib/spi.c:221: bitS_OUT = v & 0x1;
                           1190 ;     genAnd
                           1191 ;	Peephole 105	removed redundant mov
   0B0E FA                 1192 	mov	r2,a
   0B0F 13                 1193 	rrc	a
   0B10 92 81              1194 	mov	_bitS_OUT,c
                           1195 ;Initial/src/lib/spi.c:222: bitS_CLK = 1;
                           1196 ;     genAssign
   0B12 D2 80              1197 	setb	_bitS_CLK
                           1198 ;Initial/src/lib/spi.c:223: bitS_CLK = 0;
                           1199 ;     genAssign
   0B14 C2 80              1200 	clr	_bitS_CLK
                           1201 ;Initial/src/lib/spi.c:225: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1202 ;     genRLC
   0B16 EA                 1203 	mov	a,r2
   0B17 23                 1204 	rl	a
                           1205 ;Initial/src/lib/spi.c:226: bitS_OUT = v & 0x1;
                           1206 ;     genAnd
                           1207 ;	Peephole 105	removed redundant mov
   0B18 FA                 1208 	mov	r2,a
   0B19 13                 1209 	rrc	a
   0B1A 92 81              1210 	mov	_bitS_OUT,c
                           1211 ;Initial/src/lib/spi.c:227: bitS_CLK = 1;
                           1212 ;     genAssign
   0B1C D2 80              1213 	setb	_bitS_CLK
                           1214 ;Initial/src/lib/spi.c:228: bitS_CLK = 0;
                           1215 ;     genAssign
   0B1E C2 80              1216 	clr	_bitS_CLK
                           1217 ;Initial/src/lib/spi.c:230: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1218 ;     genRLC
   0B20 EA                 1219 	mov	a,r2
   0B21 23                 1220 	rl	a
                           1221 ;Initial/src/lib/spi.c:231: bitS_OUT = v & 0x1;
                           1222 ;     genAnd
                           1223 ;	Peephole 105	removed redundant mov
   0B22 FA                 1224 	mov	r2,a
   0B23 13                 1225 	rrc	a
   0B24 92 81              1226 	mov	_bitS_OUT,c
                           1227 ;Initial/src/lib/spi.c:232: bitS_CLK = 1;
                           1228 ;     genAssign
   0B26 D2 80              1229 	setb	_bitS_CLK
                           1230 ;Initial/src/lib/spi.c:233: bitS_CLK = 0;
                           1231 ;     genAssign
   0B28 C2 80              1232 	clr	_bitS_CLK
                           1233 ;Initial/src/lib/spi.c:235: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1234 ;     genRLC
   0B2A EA                 1235 	mov	a,r2
   0B2B 23                 1236 	rl	a
                           1237 ;Initial/src/lib/spi.c:236: bitS_OUT = v & 0x1;
                           1238 ;     genAnd
                           1239 ;	Peephole 105	removed redundant mov
   0B2C FA                 1240 	mov	r2,a
   0B2D 13                 1241 	rrc	a
   0B2E 92 81              1242 	mov	_bitS_OUT,c
                           1243 ;Initial/src/lib/spi.c:237: bitS_CLK = 1;
                           1244 ;     genAssign
   0B30 D2 80              1245 	setb	_bitS_CLK
                           1246 ;Initial/src/lib/spi.c:238: bitS_CLK = 0;
                           1247 ;     genAssign
   0B32 C2 80              1248 	clr	_bitS_CLK
                           1249 ;Initial/src/lib/spi.c:240: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1250 ;     genRLC
   0B34 EA                 1251 	mov	a,r2
   0B35 23                 1252 	rl	a
                           1253 ;Initial/src/lib/spi.c:241: bitS_OUT = v & 0x1;
                           1254 ;     genAnd
                           1255 ;	Peephole 105	removed redundant mov
   0B36 FA                 1256 	mov	r2,a
   0B37 13                 1257 	rrc	a
   0B38 92 81              1258 	mov	_bitS_OUT,c
                           1259 ;Initial/src/lib/spi.c:242: bitS_CLK = 1;
                           1260 ;     genAssign
   0B3A D2 80              1261 	setb	_bitS_CLK
                           1262 ;Initial/src/lib/spi.c:243: bitS_CLK = 0;
                           1263 ;     genAssign
   0B3C C2 80              1264 	clr	_bitS_CLK
                           1265 ;Initial/src/lib/spi.c:245: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1266 ;     genRLC
   0B3E EA                 1267 	mov	a,r2
   0B3F 23                 1268 	rl	a
                           1269 ;Initial/src/lib/spi.c:246: bitS_OUT = v & 0x1;
                           1270 ;     genAnd
                           1271 ;	Peephole 105	removed redundant mov
   0B40 FA                 1272 	mov	r2,a
   0B41 13                 1273 	rrc	a
   0B42 92 81              1274 	mov	_bitS_OUT,c
                           1275 ;Initial/src/lib/spi.c:247: bitS_CLK = 1;
                           1276 ;     genAssign
   0B44 D2 80              1277 	setb	_bitS_CLK
                           1278 ;Initial/src/lib/spi.c:248: bitS_CLK = 0;
                           1279 ;     genAssign
   0B46 C2 80              1280 	clr	_bitS_CLK
                           1281 ;Initial/src/lib/spi.c:250: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1282 ;     genRLC
   0B48 EA                 1283 	mov	a,r2
   0B49 23                 1284 	rl	a
                           1285 ;Initial/src/lib/spi.c:251: bitS_OUT = v & 0x1;
                           1286 ;     genAnd
                           1287 ;	Peephole 105	removed redundant mov
   0B4A FA                 1288 	mov	r2,a
   0B4B 13                 1289 	rrc	a
   0B4C 92 81              1290 	mov	_bitS_OUT,c
                           1291 ;Initial/src/lib/spi.c:252: bitS_CLK = 1;
                           1292 ;     genAssign
   0B4E D2 80              1293 	setb	_bitS_CLK
                           1294 ;Initial/src/lib/spi.c:253: bitS_CLK = 0;
                           1295 ;     genAssign
   0B50 C2 80              1296 	clr	_bitS_CLK
   0B52                    1297 00101$:
   0B52 22                 1298 	ret
                           1299 ;------------------------------------------------------------
                           1300 ;Allocation info for local variables in function 'write_bytes_msb'
                           1301 ;------------------------------------------------------------
                           1302 ;len                       Allocated with name '_write_bytes_msb_PARM_2'
                           1303 ;buf                       Allocated to registers r2 r3 
                           1304 ;------------------------------------------------------------
                           1305 ;Initial/src/lib/spi.c:257: write_bytes_msb (const xdata unsigned char *buf, unsigned char len)
                           1306 ;	-----------------------------------------
                           1307 ;	 function write_bytes_msb
                           1308 ;	-----------------------------------------
   0B53                    1309 _write_bytes_msb:
                           1310 ;     genReceive
   0B53 AA 82              1311 	mov	r2,dpl
   0B55 AB 83              1312 	mov	r3,dph
                           1313 ;Initial/src/lib/spi.c:259: while (len-- != 0){
                           1314 ;     genAssign
                           1315 ;     genAssign
   0B57 AC 1C              1316 	mov	r4,_write_bytes_msb_PARM_2
   0B59                    1317 00101$:
                           1318 ;     genAssign
   0B59 8C 05              1319 	mov	ar5,r4
                           1320 ;     genMinus
                           1321 ;     genMinusDec
   0B5B 1C                 1322 	dec	r4
                           1323 ;     genCmpEq
   0B5C BD 00 01           1324 	cjne	r5,#0x00,00108$
                           1325 ;	Peephole 112.b	changed ljmp to sjmp
                           1326 ;	Peephole 251.b	replaced sjmp to ret with ret
   0B5F 22                 1327 	ret
   0B60                    1328 00108$:
                           1329 ;Initial/src/lib/spi.c:260: write_byte_msb (*buf++);
                           1330 ;     genPointerGet
                           1331 ;     genFarPointerGet
   0B60 8A 82              1332 	mov	dpl,r2
   0B62 8B 83              1333 	mov	dph,r3
   0B64 E0                 1334 	movx	a,@dptr
   0B65 FD                 1335 	mov	r5,a
   0B66 A3                 1336 	inc	dptr
   0B67 AA 82              1337 	mov	r2,dpl
   0B69 AB 83              1338 	mov	r3,dph
                           1339 ;     genCall
   0B6B 8D 82              1340 	mov	dpl,r5
   0B6D C0 02              1341 	push	ar2
   0B6F C0 03              1342 	push	ar3
   0B71 C0 04              1343 	push	ar4
   0B73 12 0B 00           1344 	lcall	_write_byte_msb
   0B76 D0 04              1345 	pop	ar4
   0B78 D0 03              1346 	pop	ar3
   0B7A D0 02              1347 	pop	ar2
                           1348 ;	Peephole 112.b	changed ljmp to sjmp
   0B7C 80 DB              1349 	sjmp	00101$
   0B7E                    1350 00104$:
   0B7E 22                 1351 	ret
                           1352 ;------------------------------------------------------------
                           1353 ;Allocation info for local variables in function 'read_byte_msb'
                           1354 ;------------------------------------------------------------
                           1355 ;------------------------------------------------------------
                           1356 ;Initial/src/lib/spi.c:316: read_byte_msb (void) _naked
                           1357 ;	-----------------------------------------
                           1358 ;	 function read_byte_msb
                           1359 ;	-----------------------------------------
   0B7F                    1360 _read_byte_msb:
                           1361 ;	naked function: no prologue.
                           1362 ;Initial/src/lib/spi.c:363: _endasm;
                           1363 ;     genInline
   0B7F E4                 1364 	        clr a
   0B80 D2 80              1365 	        setb _bitS_CLK
   0B82 A2 83              1366 	        mov c, _bitS_IN
   0B84 33                 1367 	        rlc a
   0B85 C2 80              1368 	        clr _bitS_CLK
   0B87 D2 80              1369 	        setb _bitS_CLK
   0B89 A2 83              1370 	        mov c, _bitS_IN
   0B8B 33                 1371 	        rlc a
   0B8C C2 80              1372 	        clr _bitS_CLK
   0B8E D2 80              1373 	        setb _bitS_CLK
   0B90 A2 83              1374 	        mov c, _bitS_IN
   0B92 33                 1375 	        rlc a
   0B93 C2 80              1376 	        clr _bitS_CLK
   0B95 D2 80              1377 	        setb _bitS_CLK
   0B97 A2 83              1378 	        mov c, _bitS_IN
   0B99 33                 1379 	        rlc a
   0B9A C2 80              1380 	        clr _bitS_CLK
   0B9C D2 80              1381 	        setb _bitS_CLK
   0B9E A2 83              1382 	        mov c, _bitS_IN
   0BA0 33                 1383 	        rlc a
   0BA1 C2 80              1384 	        clr _bitS_CLK
   0BA3 D2 80              1385 	        setb _bitS_CLK
   0BA5 A2 83              1386 	        mov c, _bitS_IN
   0BA7 33                 1387 	        rlc a
   0BA8 C2 80              1388 	        clr _bitS_CLK
   0BAA D2 80              1389 	        setb _bitS_CLK
   0BAC A2 83              1390 	        mov c, _bitS_IN
   0BAE 33                 1391 	        rlc a
   0BAF C2 80              1392 	        clr _bitS_CLK
   0BB1 D2 80              1393 	        setb _bitS_CLK
   0BB3 A2 83              1394 	        mov c, _bitS_IN
   0BB5 33                 1395 	        rlc a
   0BB6 C2 80              1396 	        clr _bitS_CLK
   0BB8 F5 82              1397 	        mov dpl,a
   0BBA 22                 1398 	        ret
   0BBB                    1399 00101$:
                           1400 ;	naked function: no epilogue.
                           1401 ;------------------------------------------------------------
                           1402 ;Allocation info for local variables in function 'read_bytes_msb'
                           1403 ;------------------------------------------------------------
                           1404 ;len                       Allocated with name '_read_bytes_msb_PARM_2'
                           1405 ;buf                       Allocated to registers r2 r3 
                           1406 ;------------------------------------------------------------
                           1407 ;Initial/src/lib/spi.c:368: read_bytes_msb (xdata unsigned char *buf, unsigned char len)
                           1408 ;	-----------------------------------------
                           1409 ;	 function read_bytes_msb
                           1410 ;	-----------------------------------------
   0BBB                    1411 _read_bytes_msb:
                           1412 ;     genReceive
   0BBB AA 82              1413 	mov	r2,dpl
   0BBD AB 83              1414 	mov	r3,dph
                           1415 ;Initial/src/lib/spi.c:370: while (len-- != 0){
                           1416 ;     genAssign
                           1417 ;     genAssign
   0BBF AC 1D              1418 	mov	r4,_read_bytes_msb_PARM_2
   0BC1                    1419 00101$:
                           1420 ;     genAssign
   0BC1 8C 05              1421 	mov	ar5,r4
                           1422 ;     genMinus
                           1423 ;     genMinusDec
   0BC3 1C                 1424 	dec	r4
                           1425 ;     genCmpEq
   0BC4 BD 00 01           1426 	cjne	r5,#0x00,00108$
                           1427 ;	Peephole 112.b	changed ljmp to sjmp
                           1428 ;	Peephole 251.b	replaced sjmp to ret with ret
   0BC7 22                 1429 	ret
   0BC8                    1430 00108$:
                           1431 ;Initial/src/lib/spi.c:371: *buf++ = read_byte_msb ();
                           1432 ;     genCall
   0BC8 12 0B 7F           1433 	lcall	_read_byte_msb
   0BCB AD 82              1434 	mov	r5,dpl
                           1435 ;     genPointerSet
                           1436 ;     genFarPointerSet
   0BCD 8A 82              1437 	mov	dpl,r2
   0BCF 8B 83              1438 	mov	dph,r3
   0BD1 ED                 1439 	mov	a,r5
   0BD2 F0                 1440 	movx	@dptr,a
   0BD3 A3                 1441 	inc	dptr
   0BD4 AA 82              1442 	mov	r2,dpl
   0BD6 AB 83              1443 	mov	r3,dph
                           1444 ;	Peephole 112.b	changed ljmp to sjmp
   0BD8 80 E7              1445 	sjmp	00101$
   0BDA                    1446 00104$:
   0BDA 22                 1447 	ret
                           1448 	.area CSEG    (CODE)
