                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Wed Jul 12 14:50:26 2006
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
   08BD                     747 _setup_enables:
                    0002    748 	ar2 = 0x02
                    0003    749 	ar3 = 0x03
                    0004    750 	ar4 = 0x04
                    0005    751 	ar5 = 0x05
                    0006    752 	ar6 = 0x06
                    0007    753 	ar7 = 0x07
                    0000    754 	ar0 = 0x00
                    0001    755 	ar1 = 0x01
                            756 ;     genReceive
   08BD AA 82               757 	mov	r2,dpl
                            758 ;Initial/src/lib/spi.c:36: enables ^= SPI_ENABLE_FPGA;
                            759 ;     genXor
   08BF 63 02 01            760 	xrl	ar2,#0x01
                            761 ;Initial/src/lib/spi.c:40: HPSDR_PA = HPSDR_PA | (0x7 << 3);	// disable FPGA, CODEC_A, CODEC_B
                            762 ;     genOr
   08C2 43 80 38            763 	orl	_IOA,#0x38
                            764 ;Initial/src/lib/spi.c:41: HPSDR_PA ^= (enables & 0x7) << 3;	// enable specified devs
                            765 ;     genAnd
   08C5 74 07               766 	mov	a,#0x07
   08C7 5A                  767 	anl	a,r2
                            768 ;     genLeftShift
                            769 ;     genLeftShiftLiteral
                            770 ;     genlshOne
                            771 ;	Peephole 105	removed redundant mov
   08C8 FB                  772 	mov	r3,a
   08C9 C4                  773 	swap	a
   08CA 03                  774 	rr	a
   08CB 54 F8               775 	anl	a,#0xf8
                            776 ;     genXor
                            777 ;	Peephole 105	removed redundant mov
   08CD FB                  778 	mov	r3,a
   08CE 62 80               779 	xrl	_IOA,a
                            780 ;Initial/src/lib/spi.c:44: USRP_PE = USRP_PE | (0xf << 4);	// disable TX_A, RX_A, TX_B, RX_B
                            781 ;     genOr
   08D0 43 B1 F0            782 	orl	_IOE,#0xF0
                            783 ;Initial/src/lib/spi.c:45: USRP_PE ^= (enables & 0xf0);		// enable specified devs
                            784 ;     genAnd
   08D3 74 F0               785 	mov	a,#0xF0
   08D5 5A                  786 	anl	a,r2
                            787 ;     genXor
   08D6 62 B1               788 	xrl	_IOE,a
   08D8                     789 00101$:
   08D8 22                  790 	ret
                            791 ;------------------------------------------------------------
                            792 ;Allocation info for local variables in function 'init_spi'
                            793 ;------------------------------------------------------------
                            794 ;------------------------------------------------------------
                            795 ;Initial/src/lib/spi.c:51: init_spi (void)
                            796 ;	-----------------------------------------
                            797 ;	 function init_spi
                            798 ;	-----------------------------------------
   08D9                     799 _init_spi:
                            800 ;Initial/src/lib/spi.c:53: disable_all ();		/* disable all devs	  */
                            801 ;     genCall
   08D9 75 82 00            802 	mov	dpl,#0x00
   08DC 12 08 BD            803 	lcall	_setup_enables
                            804 ;Initial/src/lib/spi.c:54: bitS_OUT = 0;			/* idle state has CLK = 0 */
                            805 ;     genAssign
   08DF C2 81               806 	clr	_bitS_OUT
   08E1                     807 00101$:
   08E1 22                  808 	ret
                            809 ;------------------------------------------------------------
                            810 ;Allocation info for local variables in function 'count_bits8'
                            811 ;------------------------------------------------------------
                            812 ;v                         Allocated to registers r2 
                            813 ;count                     Allocated to registers r3 
                            814 ;------------------------------------------------------------
                            815 ;Initial/src/lib/spi.c:85: count_bits8 (unsigned char v)
                            816 ;	-----------------------------------------
                            817 ;	 function count_bits8
                            818 ;	-----------------------------------------
   08E2                     819 _count_bits8:
                            820 ;     genReceive
   08E2 AA 82               821 	mov	r2,dpl
                            822 ;Initial/src/lib/spi.c:87: unsigned char count = 0;
                            823 ;     genAssign
   08E4 7B 00               824 	mov	r3,#0x00
                            825 ;Initial/src/lib/spi.c:88: if (v & (1 << 0)) count++;
                            826 ;     genAnd
   08E6 EA                  827 	mov	a,r2
                            828 ;     genIfxJump
                            829 ;	Peephole 111	removed ljmp by inverse jump logic
   08E7 30 E0 02            830 	jnb	acc.0,00102$
   08EA                     831 00127$:
                            832 ;     genAssign
   08EA 7B 01               833 	mov	r3,#0x01
   08EC                     834 00102$:
                            835 ;Initial/src/lib/spi.c:89: if (v & (1 << 1)) count++;
                            836 ;     genAnd
   08EC EA                  837 	mov	a,r2
                            838 ;     genIfxJump
                            839 ;	Peephole 111	removed ljmp by inverse jump logic
   08ED 30 E1 01            840 	jnb	acc.1,00104$
   08F0                     841 00128$:
                            842 ;     genPlus
                            843 ;     genPlusIncr
   08F0 0B                  844 	inc	r3
   08F1                     845 00104$:
                            846 ;Initial/src/lib/spi.c:90: if (v & (1 << 2)) count++;
                            847 ;     genAnd
   08F1 EA                  848 	mov	a,r2
                            849 ;     genIfxJump
                            850 ;	Peephole 111	removed ljmp by inverse jump logic
   08F2 30 E2 01            851 	jnb	acc.2,00106$
   08F5                     852 00129$:
                            853 ;     genPlus
                            854 ;     genPlusIncr
   08F5 0B                  855 	inc	r3
   08F6                     856 00106$:
                            857 ;Initial/src/lib/spi.c:91: if (v & (1 << 3)) count++;
                            858 ;     genAnd
   08F6 EA                  859 	mov	a,r2
                            860 ;     genIfxJump
                            861 ;	Peephole 111	removed ljmp by inverse jump logic
   08F7 30 E3 01            862 	jnb	acc.3,00108$
   08FA                     863 00130$:
                            864 ;     genPlus
                            865 ;     genPlusIncr
   08FA 0B                  866 	inc	r3
   08FB                     867 00108$:
                            868 ;Initial/src/lib/spi.c:92: if (v & (1 << 4)) count++;
                            869 ;     genAnd
   08FB EA                  870 	mov	a,r2
                            871 ;     genIfxJump
                            872 ;	Peephole 111	removed ljmp by inverse jump logic
   08FC 30 E4 01            873 	jnb	acc.4,00110$
   08FF                     874 00131$:
                            875 ;     genPlus
                            876 ;     genPlusIncr
   08FF 0B                  877 	inc	r3
   0900                     878 00110$:
                            879 ;Initial/src/lib/spi.c:93: if (v & (1 << 5)) count++;
                            880 ;     genAnd
   0900 EA                  881 	mov	a,r2
                            882 ;     genIfxJump
                            883 ;	Peephole 111	removed ljmp by inverse jump logic
   0901 30 E5 01            884 	jnb	acc.5,00112$
   0904                     885 00132$:
                            886 ;     genPlus
                            887 ;     genPlusIncr
   0904 0B                  888 	inc	r3
   0905                     889 00112$:
                            890 ;Initial/src/lib/spi.c:94: if (v & (1 << 6)) count++;
                            891 ;     genAnd
   0905 EA                  892 	mov	a,r2
                            893 ;     genIfxJump
                            894 ;	Peephole 111	removed ljmp by inverse jump logic
   0906 30 E6 01            895 	jnb	acc.6,00114$
   0909                     896 00133$:
                            897 ;     genPlus
                            898 ;     genPlusIncr
   0909 0B                  899 	inc	r3
   090A                     900 00114$:
                            901 ;Initial/src/lib/spi.c:95: if (v & (1 << 7)) count++;
                            902 ;     genAnd
   090A EA                  903 	mov	a,r2
                            904 ;     genIfxJump
                            905 ;	Peephole 111	removed ljmp by inverse jump logic
   090B 30 E7 01            906 	jnb	acc.7,00116$
   090E                     907 00134$:
                            908 ;     genPlus
                            909 ;     genPlusIncr
   090E 0B                  910 	inc	r3
   090F                     911 00116$:
                            912 ;Initial/src/lib/spi.c:96: return count;
                            913 ;     genRet
   090F 8B 82               914 	mov	dpl,r3
   0911                     915 00117$:
   0911 22                  916 	ret
                            917 ;------------------------------------------------------------
                            918 ;Allocation info for local variables in function 'spi_read'
                            919 ;------------------------------------------------------------
                            920 ;header_lo                 Allocated with name '_spi_read_PARM_2'
                            921 ;enables                   Allocated with name '_spi_read_PARM_3'
                            922 ;format                    Allocated with name '_spi_read_PARM_4'
                            923 ;buf                       Allocated with name '_spi_read_PARM_5'
                            924 ;len                       Allocated with name '_spi_read_PARM_6'
                            925 ;header_hi                 Allocated to registers r2 
                            926 ;------------------------------------------------------------
                            927 ;Initial/src/lib/spi.c:112: spi_read (unsigned char header_hi, unsigned char header_lo,
                            928 ;	-----------------------------------------
                            929 ;	 function spi_read
                            930 ;	-----------------------------------------
   0912                     931 _spi_read:
                            932 ;     genReceive
   0912 AA 82               933 	mov	r2,dpl
                            934 ;Initial/src/lib/spi.c:116: if (count_bits8 (enables) > 1)
                            935 ;     genCall
   0914 85 11 82            936 	mov	dpl,_spi_read_PARM_3
   0917 C0 02               937 	push	ar2
   0919 12 08 E2            938 	lcall	_count_bits8
   091C AB 82               939 	mov	r3,dpl
   091E D0 02               940 	pop	ar2
                            941 ;     genCmpGt
                            942 ;     genCmp
                            943 ;     genIfxJump
                            944 ;	Peephole 108	removed ljmp by inverse jump logic
                            945 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   0920 EB                  946 	mov	a,r3
   0921 24 FE               947 	add	a,#0xff - 0x01
   0923 50 04               948 	jnc	00102$
   0925                     949 00121$:
                            950 ;Initial/src/lib/spi.c:117: return 0;		// error, too many enables set
                            951 ;     genRet
   0925 75 82 00            952 	mov	dpl,#0x00
                            953 ;	Peephole 112.b	changed ljmp to sjmp
                            954 ;	Peephole 251.b	replaced sjmp to ret with ret
   0928 22                  955 	ret
   0929                     956 00102$:
                            957 ;Initial/src/lib/spi.c:119: setup_enables (enables);
                            958 ;     genCall
   0929 85 11 82            959 	mov	dpl,_spi_read_PARM_3
   092C C0 02               960 	push	ar2
   092E 12 08 BD            961 	lcall	_setup_enables
   0931 D0 02               962 	pop	ar2
                            963 ;Initial/src/lib/spi.c:121: if (format & SPI_FMT_LSB){		// order: LSB
                            964 ;     genAnd
   0933 E5 12               965 	mov	a,_spi_read_PARM_4
                            966 ;     genIfxJump
                            967 ;	Peephole 111	removed ljmp by inverse jump logic
   0935 30 E7 04            968 	jnb	acc.7,00111$
   0938                     969 00122$:
                            970 ;Initial/src/lib/spi.c:123: return 0;		// error, not implemented
                            971 ;     genRet
   0938 75 82 00            972 	mov	dpl,#0x00
                            973 ;	Peephole 112.b	changed ljmp to sjmp
                            974 ;	Peephole 251.b	replaced sjmp to ret with ret
   093B 22                  975 	ret
   093C                     976 00111$:
                            977 ;Initial/src/lib/spi.c:145: switch (format & SPI_FMT_HDR_MASK){
                            978 ;     genAnd
   093C 74 60               979 	mov	a,#0x60
   093E 55 12               980 	anl	a,_spi_read_PARM_4
                            981 ;     genCmpEq
                            982 ;	Peephole 112.b	changed ljmp to sjmp
                            983 ;	Peephole 115.b	jump optimization
   0940 FB                  984 	mov	r3,a
   0941 60 23               985 	jz	00107$
   0943                     986 00123$:
                            987 ;     genCmpEq
   0943 BB 20 02            988 	cjne	r3,#0x20,00124$
                            989 ;	Peephole 112.b	changed ljmp to sjmp
   0946 80 05               990 	sjmp	00104$
   0948                     991 00124$:
                            992 ;     genCmpEq
                            993 ;	Peephole 112.b	changed ljmp to sjmp
                            994 ;Initial/src/lib/spi.c:148: case SPI_FMT_HDR_1:
                            995 ;	Peephole 112.b	changed ljmp to sjmp
                            996 ;	Peephole 199	optimized misc jump sequence
   0948 BB 40 17            997 	cjne	r3,#0x40,00106$
   094B 80 08               998 	sjmp	00105$
                            999 ;00125$:
   094D                    1000 00104$:
                           1001 ;Initial/src/lib/spi.c:149: write_byte_msb (header_lo);
                           1002 ;     genCall
   094D 85 10 82           1003 	mov	dpl,_spi_read_PARM_2
   0950 12 09 D9           1004 	lcall	_write_byte_msb
                           1005 ;Initial/src/lib/spi.c:150: break;
                           1006 ;Initial/src/lib/spi.c:151: case SPI_FMT_HDR_2:
                           1007 ;	Peephole 112.b	changed ljmp to sjmp
   0953 80 11              1008 	sjmp	00107$
   0955                    1009 00105$:
                           1010 ;Initial/src/lib/spi.c:152: write_byte_msb (header_hi);
                           1011 ;     genCall
   0955 8A 82              1012 	mov	dpl,r2
   0957 12 09 D9           1013 	lcall	_write_byte_msb
                           1014 ;Initial/src/lib/spi.c:153: write_byte_msb (header_lo);
                           1015 ;     genCall
   095A 85 10 82           1016 	mov	dpl,_spi_read_PARM_2
   095D 12 09 D9           1017 	lcall	_write_byte_msb
                           1018 ;Initial/src/lib/spi.c:154: break;
                           1019 ;Initial/src/lib/spi.c:155: default:
                           1020 ;	Peephole 112.b	changed ljmp to sjmp
   0960 80 04              1021 	sjmp	00107$
   0962                    1022 00106$:
                           1023 ;Initial/src/lib/spi.c:156: return 0;		// error
                           1024 ;     genRet
   0962 75 82 00           1025 	mov	dpl,#0x00
                           1026 ;Initial/src/lib/spi.c:157: }
                           1027 ;	Peephole 112.b	changed ljmp to sjmp
                           1028 ;	Peephole 251.b	replaced sjmp to ret with ret
   0965 22                 1029 	ret
   0966                    1030 00107$:
                           1031 ;Initial/src/lib/spi.c:158: if (len != 0)
                           1032 ;     genCmpEq
   0966 E5 15              1033 	mov	a,_spi_read_PARM_6
                           1034 ;	Peephole 110	removed ljmp by inverse jump logic
   0968 60 0C              1035 	jz	00112$
   096A                    1036 00126$:
                           1037 ;Initial/src/lib/spi.c:159: read_bytes_msb (buf, len);
                           1038 ;     genAssign
   096A 85 15 1D           1039 	mov	_read_bytes_msb_PARM_2,_spi_read_PARM_6
                           1040 ;     genCall
   096D 85 13 82           1041 	mov	dpl,_spi_read_PARM_5
   0970 85 14 83           1042 	mov	dph,(_spi_read_PARM_5 + 1)
   0973 12 0A 94           1043 	lcall	_read_bytes_msb
   0976                    1044 00112$:
                           1045 ;Initial/src/lib/spi.c:162: disable_all ();
                           1046 ;     genCall
   0976 75 82 00           1047 	mov	dpl,#0x00
   0979 12 08 BD           1048 	lcall	_setup_enables
                           1049 ;Initial/src/lib/spi.c:163: return 1;		// success
                           1050 ;     genRet
   097C 75 82 01           1051 	mov	dpl,#0x01
   097F                    1052 00113$:
   097F 22                 1053 	ret
                           1054 ;------------------------------------------------------------
                           1055 ;Allocation info for local variables in function 'spi_write'
                           1056 ;------------------------------------------------------------
                           1057 ;header_lo                 Allocated with name '_spi_write_PARM_2'
                           1058 ;enables                   Allocated with name '_spi_write_PARM_3'
                           1059 ;format                    Allocated with name '_spi_write_PARM_4'
                           1060 ;buf                       Allocated with name '_spi_write_PARM_5'
                           1061 ;len                       Allocated with name '_spi_write_PARM_6'
                           1062 ;header_hi                 Allocated to registers r2 
                           1063 ;------------------------------------------------------------
                           1064 ;Initial/src/lib/spi.c:169: spi_write (unsigned char header_hi, unsigned char header_lo,
                           1065 ;	-----------------------------------------
                           1066 ;	 function spi_write
                           1067 ;	-----------------------------------------
   0980                    1068 _spi_write:
                           1069 ;     genReceive
   0980 AA 82              1070 	mov	r2,dpl
                           1071 ;Initial/src/lib/spi.c:173: setup_enables (enables);
                           1072 ;     genCall
   0982 85 17 82           1073 	mov	dpl,_spi_write_PARM_3
   0985 C0 02              1074 	push	ar2
   0987 12 08 BD           1075 	lcall	_setup_enables
   098A D0 02              1076 	pop	ar2
                           1077 ;Initial/src/lib/spi.c:175: if (format & SPI_FMT_LSB){		// order: LSB
                           1078 ;     genAnd
   098C E5 18              1079 	mov	a,_spi_write_PARM_4
                           1080 ;     genIfxJump
                           1081 ;	Peephole 111	removed ljmp by inverse jump logic
   098E 30 E7 04           1082 	jnb	acc.7,00109$
   0991                    1083 00118$:
                           1084 ;Initial/src/lib/spi.c:177: return 0;		// error, not implemented
                           1085 ;     genRet
   0991 75 82 00           1086 	mov	dpl,#0x00
                           1087 ;	Peephole 112.b	changed ljmp to sjmp
                           1088 ;	Peephole 251.b	replaced sjmp to ret with ret
   0994 22                 1089 	ret
   0995                    1090 00109$:
                           1091 ;Initial/src/lib/spi.c:199: switch (format & SPI_FMT_HDR_MASK){
                           1092 ;     genAnd
   0995 74 60              1093 	mov	a,#0x60
   0997 55 18              1094 	anl	a,_spi_write_PARM_4
                           1095 ;     genCmpEq
                           1096 ;	Peephole 112.b	changed ljmp to sjmp
                           1097 ;	Peephole 115.b	jump optimization
   0999 FB                 1098 	mov	r3,a
   099A 60 23              1099 	jz	00105$
   099C                    1100 00119$:
                           1101 ;     genCmpEq
   099C BB 20 02           1102 	cjne	r3,#0x20,00120$
                           1103 ;	Peephole 112.b	changed ljmp to sjmp
   099F 80 05              1104 	sjmp	00102$
   09A1                    1105 00120$:
                           1106 ;     genCmpEq
                           1107 ;	Peephole 112.b	changed ljmp to sjmp
                           1108 ;Initial/src/lib/spi.c:202: case SPI_FMT_HDR_1:
                           1109 ;	Peephole 112.b	changed ljmp to sjmp
                           1110 ;	Peephole 199	optimized misc jump sequence
   09A1 BB 40 17           1111 	cjne	r3,#0x40,00104$
   09A4 80 08              1112 	sjmp	00103$
                           1113 ;00121$:
   09A6                    1114 00102$:
                           1115 ;Initial/src/lib/spi.c:203: write_byte_msb (header_lo);
                           1116 ;     genCall
   09A6 85 16 82           1117 	mov	dpl,_spi_write_PARM_2
   09A9 12 09 D9           1118 	lcall	_write_byte_msb
                           1119 ;Initial/src/lib/spi.c:204: break;
                           1120 ;Initial/src/lib/spi.c:205: case SPI_FMT_HDR_2:
                           1121 ;	Peephole 112.b	changed ljmp to sjmp
   09AC 80 11              1122 	sjmp	00105$
   09AE                    1123 00103$:
                           1124 ;Initial/src/lib/spi.c:206: write_byte_msb (header_hi);
                           1125 ;     genCall
   09AE 8A 82              1126 	mov	dpl,r2
   09B0 12 09 D9           1127 	lcall	_write_byte_msb
                           1128 ;Initial/src/lib/spi.c:207: write_byte_msb (header_lo);
                           1129 ;     genCall
   09B3 85 16 82           1130 	mov	dpl,_spi_write_PARM_2
   09B6 12 09 D9           1131 	lcall	_write_byte_msb
                           1132 ;Initial/src/lib/spi.c:208: break;
                           1133 ;Initial/src/lib/spi.c:209: default:
                           1134 ;	Peephole 112.b	changed ljmp to sjmp
   09B9 80 04              1135 	sjmp	00105$
   09BB                    1136 00104$:
                           1137 ;Initial/src/lib/spi.c:210: return 0;		// error
                           1138 ;     genRet
   09BB 75 82 00           1139 	mov	dpl,#0x00
                           1140 ;Initial/src/lib/spi.c:211: }
                           1141 ;	Peephole 112.b	changed ljmp to sjmp
                           1142 ;	Peephole 251.b	replaced sjmp to ret with ret
   09BE 22                 1143 	ret
   09BF                    1144 00105$:
                           1145 ;Initial/src/lib/spi.c:212: if (len != 0)
                           1146 ;     genCmpEq
   09BF E5 1B              1147 	mov	a,_spi_write_PARM_6
                           1148 ;	Peephole 110	removed ljmp by inverse jump logic
   09C1 60 0C              1149 	jz	00110$
   09C3                    1150 00122$:
                           1151 ;Initial/src/lib/spi.c:213: write_bytes_msb (buf, len);
                           1152 ;     genAssign
   09C3 85 1B 1C           1153 	mov	_write_bytes_msb_PARM_2,_spi_write_PARM_6
                           1154 ;     genCall
   09C6 85 19 82           1155 	mov	dpl,_spi_write_PARM_5
   09C9 85 1A 83           1156 	mov	dph,(_spi_write_PARM_5 + 1)
   09CC 12 0A 2C           1157 	lcall	_write_bytes_msb
   09CF                    1158 00110$:
                           1159 ;Initial/src/lib/spi.c:216: disable_all ();
                           1160 ;     genCall
   09CF 75 82 00           1161 	mov	dpl,#0x00
   09D2 12 08 BD           1162 	lcall	_setup_enables
                           1163 ;Initial/src/lib/spi.c:217: return 1;		// success
                           1164 ;     genRet
   09D5 75 82 01           1165 	mov	dpl,#0x01
   09D8                    1166 00111$:
   09D8 22                 1167 	ret
                           1168 ;------------------------------------------------------------
                           1169 ;Allocation info for local variables in function 'write_byte_msb'
                           1170 ;------------------------------------------------------------
                           1171 ;v                         Allocated to registers r2 
                           1172 ;------------------------------------------------------------
                           1173 ;Initial/src/lib/spi.c:223: write_byte_msb (unsigned char v)
                           1174 ;	-----------------------------------------
                           1175 ;	 function write_byte_msb
                           1176 ;	-----------------------------------------
   09D9                    1177 _write_byte_msb:
                           1178 ;     genReceive
                           1179 ;Initial/src/lib/spi.c:225: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1180 ;     genRLC
                           1181 ;	peephole 177.g	optimized mov sequence
   09D9 E5 82              1182 	mov	a,dpl
   09DB FA                 1183 	mov	r2,a
   09DC 23                 1184 	rl	a
                           1185 ;Initial/src/lib/spi.c:226: bitS_OUT = v & 0x1;
                           1186 ;     genAnd
                           1187 ;	Peephole 105	removed redundant mov
   09DD FA                 1188 	mov	r2,a
   09DE 13                 1189 	rrc	a
   09DF 92 81              1190 	mov	_bitS_OUT,c
                           1191 ;Initial/src/lib/spi.c:227: bitS_CLK = 1;
                           1192 ;     genAssign
   09E1 D2 80              1193 	setb	_bitS_CLK
                           1194 ;Initial/src/lib/spi.c:228: bitS_CLK = 0;
                           1195 ;     genAssign
   09E3 C2 80              1196 	clr	_bitS_CLK
                           1197 ;Initial/src/lib/spi.c:230: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1198 ;     genRLC
   09E5 EA                 1199 	mov	a,r2
   09E6 23                 1200 	rl	a
                           1201 ;Initial/src/lib/spi.c:231: bitS_OUT = v & 0x1;
                           1202 ;     genAnd
                           1203 ;	Peephole 105	removed redundant mov
   09E7 FA                 1204 	mov	r2,a
   09E8 13                 1205 	rrc	a
   09E9 92 81              1206 	mov	_bitS_OUT,c
                           1207 ;Initial/src/lib/spi.c:232: bitS_CLK = 1;
                           1208 ;     genAssign
   09EB D2 80              1209 	setb	_bitS_CLK
                           1210 ;Initial/src/lib/spi.c:233: bitS_CLK = 0;
                           1211 ;     genAssign
   09ED C2 80              1212 	clr	_bitS_CLK
                           1213 ;Initial/src/lib/spi.c:235: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1214 ;     genRLC
   09EF EA                 1215 	mov	a,r2
   09F0 23                 1216 	rl	a
                           1217 ;Initial/src/lib/spi.c:236: bitS_OUT = v & 0x1;
                           1218 ;     genAnd
                           1219 ;	Peephole 105	removed redundant mov
   09F1 FA                 1220 	mov	r2,a
   09F2 13                 1221 	rrc	a
   09F3 92 81              1222 	mov	_bitS_OUT,c
                           1223 ;Initial/src/lib/spi.c:237: bitS_CLK = 1;
                           1224 ;     genAssign
   09F5 D2 80              1225 	setb	_bitS_CLK
                           1226 ;Initial/src/lib/spi.c:238: bitS_CLK = 0;
                           1227 ;     genAssign
   09F7 C2 80              1228 	clr	_bitS_CLK
                           1229 ;Initial/src/lib/spi.c:240: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1230 ;     genRLC
   09F9 EA                 1231 	mov	a,r2
   09FA 23                 1232 	rl	a
                           1233 ;Initial/src/lib/spi.c:241: bitS_OUT = v & 0x1;
                           1234 ;     genAnd
                           1235 ;	Peephole 105	removed redundant mov
   09FB FA                 1236 	mov	r2,a
   09FC 13                 1237 	rrc	a
   09FD 92 81              1238 	mov	_bitS_OUT,c
                           1239 ;Initial/src/lib/spi.c:242: bitS_CLK = 1;
                           1240 ;     genAssign
   09FF D2 80              1241 	setb	_bitS_CLK
                           1242 ;Initial/src/lib/spi.c:243: bitS_CLK = 0;
                           1243 ;     genAssign
   0A01 C2 80              1244 	clr	_bitS_CLK
                           1245 ;Initial/src/lib/spi.c:245: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1246 ;     genRLC
   0A03 EA                 1247 	mov	a,r2
   0A04 23                 1248 	rl	a
                           1249 ;Initial/src/lib/spi.c:246: bitS_OUT = v & 0x1;
                           1250 ;     genAnd
                           1251 ;	Peephole 105	removed redundant mov
   0A05 FA                 1252 	mov	r2,a
   0A06 13                 1253 	rrc	a
   0A07 92 81              1254 	mov	_bitS_OUT,c
                           1255 ;Initial/src/lib/spi.c:247: bitS_CLK = 1;
                           1256 ;     genAssign
   0A09 D2 80              1257 	setb	_bitS_CLK
                           1258 ;Initial/src/lib/spi.c:248: bitS_CLK = 0;
                           1259 ;     genAssign
   0A0B C2 80              1260 	clr	_bitS_CLK
                           1261 ;Initial/src/lib/spi.c:250: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1262 ;     genRLC
   0A0D EA                 1263 	mov	a,r2
   0A0E 23                 1264 	rl	a
                           1265 ;Initial/src/lib/spi.c:251: bitS_OUT = v & 0x1;
                           1266 ;     genAnd
                           1267 ;	Peephole 105	removed redundant mov
   0A0F FA                 1268 	mov	r2,a
   0A10 13                 1269 	rrc	a
   0A11 92 81              1270 	mov	_bitS_OUT,c
                           1271 ;Initial/src/lib/spi.c:252: bitS_CLK = 1;
                           1272 ;     genAssign
   0A13 D2 80              1273 	setb	_bitS_CLK
                           1274 ;Initial/src/lib/spi.c:253: bitS_CLK = 0;
                           1275 ;     genAssign
   0A15 C2 80              1276 	clr	_bitS_CLK
                           1277 ;Initial/src/lib/spi.c:255: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1278 ;     genRLC
   0A17 EA                 1279 	mov	a,r2
   0A18 23                 1280 	rl	a
                           1281 ;Initial/src/lib/spi.c:256: bitS_OUT = v & 0x1;
                           1282 ;     genAnd
                           1283 ;	Peephole 105	removed redundant mov
   0A19 FA                 1284 	mov	r2,a
   0A1A 13                 1285 	rrc	a
   0A1B 92 81              1286 	mov	_bitS_OUT,c
                           1287 ;Initial/src/lib/spi.c:257: bitS_CLK = 1;
                           1288 ;     genAssign
   0A1D D2 80              1289 	setb	_bitS_CLK
                           1290 ;Initial/src/lib/spi.c:258: bitS_CLK = 0;
                           1291 ;     genAssign
   0A1F C2 80              1292 	clr	_bitS_CLK
                           1293 ;Initial/src/lib/spi.c:260: v = (v << 1) | (v >> 7);	// rotate left (MSB into bottom bit)
                           1294 ;     genRLC
   0A21 EA                 1295 	mov	a,r2
   0A22 23                 1296 	rl	a
                           1297 ;Initial/src/lib/spi.c:261: bitS_OUT = v & 0x1;
                           1298 ;     genAnd
                           1299 ;	Peephole 105	removed redundant mov
   0A23 FA                 1300 	mov	r2,a
   0A24 13                 1301 	rrc	a
   0A25 92 81              1302 	mov	_bitS_OUT,c
                           1303 ;Initial/src/lib/spi.c:262: bitS_CLK = 1;
                           1304 ;     genAssign
   0A27 D2 80              1305 	setb	_bitS_CLK
                           1306 ;Initial/src/lib/spi.c:263: bitS_CLK = 0;
                           1307 ;     genAssign
   0A29 C2 80              1308 	clr	_bitS_CLK
   0A2B                    1309 00101$:
   0A2B 22                 1310 	ret
                           1311 ;------------------------------------------------------------
                           1312 ;Allocation info for local variables in function 'write_bytes_msb'
                           1313 ;------------------------------------------------------------
                           1314 ;len                       Allocated with name '_write_bytes_msb_PARM_2'
                           1315 ;buf                       Allocated to registers r2 r3 
                           1316 ;------------------------------------------------------------
                           1317 ;Initial/src/lib/spi.c:267: write_bytes_msb (const xdata unsigned char *buf, unsigned char len)
                           1318 ;	-----------------------------------------
                           1319 ;	 function write_bytes_msb
                           1320 ;	-----------------------------------------
   0A2C                    1321 _write_bytes_msb:
                           1322 ;     genReceive
   0A2C AA 82              1323 	mov	r2,dpl
   0A2E AB 83              1324 	mov	r3,dph
                           1325 ;Initial/src/lib/spi.c:269: while (len-- != 0){
                           1326 ;     genAssign
                           1327 ;     genAssign
   0A30 AC 1C              1328 	mov	r4,_write_bytes_msb_PARM_2
   0A32                    1329 00101$:
                           1330 ;     genAssign
   0A32 8C 05              1331 	mov	ar5,r4
                           1332 ;     genMinus
                           1333 ;     genMinusDec
   0A34 1C                 1334 	dec	r4
                           1335 ;     genCmpEq
   0A35 BD 00 01           1336 	cjne	r5,#0x00,00108$
                           1337 ;	Peephole 112.b	changed ljmp to sjmp
                           1338 ;	Peephole 251.b	replaced sjmp to ret with ret
   0A38 22                 1339 	ret
   0A39                    1340 00108$:
                           1341 ;Initial/src/lib/spi.c:270: write_byte_msb (*buf++);
                           1342 ;     genPointerGet
                           1343 ;     genFarPointerGet
   0A39 8A 82              1344 	mov	dpl,r2
   0A3B 8B 83              1345 	mov	dph,r3
   0A3D E0                 1346 	movx	a,@dptr
   0A3E FD                 1347 	mov	r5,a
   0A3F A3                 1348 	inc	dptr
   0A40 AA 82              1349 	mov	r2,dpl
   0A42 AB 83              1350 	mov	r3,dph
                           1351 ;     genCall
   0A44 8D 82              1352 	mov	dpl,r5
   0A46 C0 02              1353 	push	ar2
   0A48 C0 03              1354 	push	ar3
   0A4A C0 04              1355 	push	ar4
   0A4C 12 09 D9           1356 	lcall	_write_byte_msb
   0A4F D0 04              1357 	pop	ar4
   0A51 D0 03              1358 	pop	ar3
   0A53 D0 02              1359 	pop	ar2
                           1360 ;	Peephole 112.b	changed ljmp to sjmp
   0A55 80 DB              1361 	sjmp	00101$
   0A57                    1362 00104$:
   0A57 22                 1363 	ret
                           1364 ;------------------------------------------------------------
                           1365 ;Allocation info for local variables in function 'read_byte_msb'
                           1366 ;------------------------------------------------------------
                           1367 ;------------------------------------------------------------
                           1368 ;Initial/src/lib/spi.c:326: read_byte_msb (void) _naked
                           1369 ;	-----------------------------------------
                           1370 ;	 function read_byte_msb
                           1371 ;	-----------------------------------------
   0A58                    1372 _read_byte_msb:
                           1373 ;	naked function: no prologue.
                           1374 ;Initial/src/lib/spi.c:373: _endasm;
                           1375 ;     genInline
   0A58 E4                 1376 	        clr a
   0A59 D2 80              1377 	        setb _bitS_CLK
   0A5B A2 83              1378 	        mov c, _bitS_IN
   0A5D 33                 1379 	        rlc a
   0A5E C2 80              1380 	        clr _bitS_CLK
   0A60 D2 80              1381 	        setb _bitS_CLK
   0A62 A2 83              1382 	        mov c, _bitS_IN
   0A64 33                 1383 	        rlc a
   0A65 C2 80              1384 	        clr _bitS_CLK
   0A67 D2 80              1385 	        setb _bitS_CLK
   0A69 A2 83              1386 	        mov c, _bitS_IN
   0A6B 33                 1387 	        rlc a
   0A6C C2 80              1388 	        clr _bitS_CLK
   0A6E D2 80              1389 	        setb _bitS_CLK
   0A70 A2 83              1390 	        mov c, _bitS_IN
   0A72 33                 1391 	        rlc a
   0A73 C2 80              1392 	        clr _bitS_CLK
   0A75 D2 80              1393 	        setb _bitS_CLK
   0A77 A2 83              1394 	        mov c, _bitS_IN
   0A79 33                 1395 	        rlc a
   0A7A C2 80              1396 	        clr _bitS_CLK
   0A7C D2 80              1397 	        setb _bitS_CLK
   0A7E A2 83              1398 	        mov c, _bitS_IN
   0A80 33                 1399 	        rlc a
   0A81 C2 80              1400 	        clr _bitS_CLK
   0A83 D2 80              1401 	        setb _bitS_CLK
   0A85 A2 83              1402 	        mov c, _bitS_IN
   0A87 33                 1403 	        rlc a
   0A88 C2 80              1404 	        clr _bitS_CLK
   0A8A D2 80              1405 	        setb _bitS_CLK
   0A8C A2 83              1406 	        mov c, _bitS_IN
   0A8E 33                 1407 	        rlc a
   0A8F C2 80              1408 	        clr _bitS_CLK
   0A91 F5 82              1409 	        mov dpl,a
   0A93 22                 1410 	        ret
   0A94                    1411 00101$:
                           1412 ;	naked function: no epilogue.
                           1413 ;------------------------------------------------------------
                           1414 ;Allocation info for local variables in function 'read_bytes_msb'
                           1415 ;------------------------------------------------------------
                           1416 ;len                       Allocated with name '_read_bytes_msb_PARM_2'
                           1417 ;buf                       Allocated to registers r2 r3 
                           1418 ;------------------------------------------------------------
                           1419 ;Initial/src/lib/spi.c:378: read_bytes_msb (xdata unsigned char *buf, unsigned char len)
                           1420 ;	-----------------------------------------
                           1421 ;	 function read_bytes_msb
                           1422 ;	-----------------------------------------
   0A94                    1423 _read_bytes_msb:
                           1424 ;     genReceive
   0A94 AA 82              1425 	mov	r2,dpl
   0A96 AB 83              1426 	mov	r3,dph
                           1427 ;Initial/src/lib/spi.c:380: while (len-- != 0){
                           1428 ;     genAssign
                           1429 ;     genAssign
   0A98 AC 1D              1430 	mov	r4,_read_bytes_msb_PARM_2
   0A9A                    1431 00101$:
                           1432 ;     genAssign
   0A9A 8C 05              1433 	mov	ar5,r4
                           1434 ;     genMinus
                           1435 ;     genMinusDec
   0A9C 1C                 1436 	dec	r4
                           1437 ;     genCmpEq
   0A9D BD 00 01           1438 	cjne	r5,#0x00,00108$
                           1439 ;	Peephole 112.b	changed ljmp to sjmp
                           1440 ;	Peephole 251.b	replaced sjmp to ret with ret
   0AA0 22                 1441 	ret
   0AA1                    1442 00108$:
                           1443 ;Initial/src/lib/spi.c:381: *buf++ = read_byte_msb ();
                           1444 ;     genCall
   0AA1 12 0A 58           1445 	lcall	_read_byte_msb
   0AA4 AD 82              1446 	mov	r5,dpl
                           1447 ;     genPointerSet
                           1448 ;     genFarPointerSet
   0AA6 8A 82              1449 	mov	dpl,r2
   0AA8 8B 83              1450 	mov	dph,r3
   0AAA ED                 1451 	mov	a,r5
   0AAB F0                 1452 	movx	@dptr,a
   0AAC A3                 1453 	inc	dptr
   0AAD AA 82              1454 	mov	r2,dpl
   0AAF AB 83              1455 	mov	r3,dph
                           1456 ;	Peephole 112.b	changed ljmp to sjmp
   0AB1 80 E7              1457 	sjmp	00101$
   0AB3                    1458 00104$:
   0AB3 22                 1459 	ret
                           1460 	.area CSEG    (CODE)
