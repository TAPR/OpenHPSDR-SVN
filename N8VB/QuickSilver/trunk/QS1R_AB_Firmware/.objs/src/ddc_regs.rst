                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Wed Jan 03 15:50:41 2007
                              5 ;--------------------------------------------------------
                              6 	.module ddc_regs
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _set_COEF_PARM_4
                             13 	.globl _set_COEF_PARM_3
                             14 	.globl _set_COEF_PARM_2
                             15 	.globl _read_COEF_PARM_4
                             16 	.globl _read_COEF_PARM_3
                             17 	.globl _read_COEF_PARM_2
                             18 	.globl _set_RSRV_PARM_3
                             19 	.globl _set_RSRV_PARM_2
                             20 	.globl _read_RSRV_PARM_3
                             21 	.globl _read_RSRV_PARM_2
                             22 	.globl _set_RTAP_PARM_3
                             23 	.globl _set_RTAP_PARM_2
                             24 	.globl _read_RTAP_PARM_3
                             25 	.globl _read_RTAP_PARM_2
                             26 	.globl _set_RAO_PARM_3
                             27 	.globl _set_RAO_PARM_2
                             28 	.globl _read_RAO_PARM_3
                             29 	.globl _read_RAO_PARM_2
                             30 	.globl _set_RDEC_PARM_3
                             31 	.globl _set_RDEC_PARM_2
                             32 	.globl _read_RDEC_PARM_3
                             33 	.globl _read_RDEC_PARM_2
                             34 	.globl _set_RCR_PARM_3
                             35 	.globl _set_RCR_PARM_2
                             36 	.globl _read_RCR_PARM_3
                             37 	.globl _read_RCR_PARM_2
                             38 	.globl _set_C5DEC_PARM_3
                             39 	.globl _set_C5DEC_PARM_2
                             40 	.globl _read_C5DEC_PARM_3
                             41 	.globl _read_C5DEC_PARM_2
                             42 	.globl _set_C5SR_PARM_3
                             43 	.globl _set_C5SR_PARM_2
                             44 	.globl _read_C5SR_PARM_3
                             45 	.globl _read_C5SR_PARM_2
                             46 	.globl _set_C2DEC_PARM_3
                             47 	.globl _set_C2DEC_PARM_2
                             48 	.globl _read_C2DEC_PARM_3
                             49 	.globl _read_C2DEC_PARM_2
                             50 	.globl _set_C2SR_PARM_3
                             51 	.globl _set_C2SR_PARM_2
                             52 	.globl _read_C2SR_PARM_3
                             53 	.globl _read_C2SR_PARM_2
                             54 	.globl _set_NPHA_PARM_3
                             55 	.globl _set_NPHA_PARM_2
                             56 	.globl _read_NPHA_PARM_3
                             57 	.globl _read_NPHA_PARM_2
                             58 	.globl _set_NFRE_PARM_3
                             59 	.globl _set_NFRE_PARM_2
                             60 	.globl _read_NFRE_PARM_3
                             61 	.globl _read_NFRE_PARM_2
                             62 	.globl _set_NSCR_PARM_3
                             63 	.globl _set_NSCR_PARM_2
                             64 	.globl _read_NSCR_PARM_3
                             65 	.globl _read_NSCR_PARM_2
                             66 	.globl _set_NCR_PARM_3
                             67 	.globl _set_NCR_PARM_2
                             68 	.globl _read_NCR_PARM_3
                             69 	.globl _read_NCR_PARM_2
                             70 	.globl _set_MCR_PARM_3
                             71 	.globl _set_MCR_PARM_2
                             72 	.globl _read_MCR_PARM_3
                             73 	.globl _read_MCR_PARM_2
                             74 	.globl _bitCPLD_CS
                             75 	.globl _bitFPGA_CS
                             76 	.globl _bitALTERA_DCLK
                             77 	.globl _bitALTERA_DATA0
                             78 	.globl _bitS_CLK
                             79 	.globl _bitS_IN
                             80 	.globl _bitS_OUT
                             81 	.globl _EIPX6
                             82 	.globl _EIPX5
                             83 	.globl _EIPX4
                             84 	.globl _PI2C
                             85 	.globl _PUSB
                             86 	.globl _EIEX6
                             87 	.globl _EIEX5
                             88 	.globl _EIEX4
                             89 	.globl _EI2C
                             90 	.globl _EIUSB
                             91 	.globl _SMOD1
                             92 	.globl _ERESI
                             93 	.globl _RESI
                             94 	.globl _INT6
                             95 	.globl _CY
                             96 	.globl _AC
                             97 	.globl _F0
                             98 	.globl _RS1
                             99 	.globl _RS0
                            100 	.globl _OV
                            101 	.globl _FL
                            102 	.globl _P
                            103 	.globl _TF2
                            104 	.globl _EXF2
                            105 	.globl _RCLK
                            106 	.globl _TCLK
                            107 	.globl _EXEN2
                            108 	.globl _TR2
                            109 	.globl _C_T2
                            110 	.globl _CP_RL2
                            111 	.globl _SM01
                            112 	.globl _SM11
                            113 	.globl _SM21
                            114 	.globl _REN1
                            115 	.globl _TB81
                            116 	.globl _RB81
                            117 	.globl _TI1
                            118 	.globl _RI1
                            119 	.globl _PS1
                            120 	.globl _PT2
                            121 	.globl _PS0
                            122 	.globl _PT1
                            123 	.globl _PX1
                            124 	.globl _PT0
                            125 	.globl _PX0
                            126 	.globl _EA
                            127 	.globl _ES1
                            128 	.globl _ET2
                            129 	.globl _ES0
                            130 	.globl _ET1
                            131 	.globl _EX1
                            132 	.globl _ET0
                            133 	.globl _EX0
                            134 	.globl _SM0
                            135 	.globl _SM1
                            136 	.globl _SM2
                            137 	.globl _REN
                            138 	.globl _TB8
                            139 	.globl _RB8
                            140 	.globl _TI
                            141 	.globl _RI
                            142 	.globl _TF1
                            143 	.globl _TR1
                            144 	.globl _TF0
                            145 	.globl _TR0
                            146 	.globl _IE1
                            147 	.globl _IT1
                            148 	.globl _IE0
                            149 	.globl _IT0
                            150 	.globl _SEL
                            151 	.globl _EIP
                            152 	.globl _B
                            153 	.globl _EIE
                            154 	.globl _ACC
                            155 	.globl _EICON
                            156 	.globl _PSW
                            157 	.globl _TH2
                            158 	.globl _TL2
                            159 	.globl _RCAP2H
                            160 	.globl _RCAP2L
                            161 	.globl _T2CON
                            162 	.globl _SBUF1
                            163 	.globl _SCON1
                            164 	.globl _GPIFSGLDATLNOX
                            165 	.globl _GPIFSGLDATLX
                            166 	.globl _GPIFSGLDATH
                            167 	.globl _GPIFTRIG
                            168 	.globl _EP01STAT
                            169 	.globl _IP
                            170 	.globl _OEE
                            171 	.globl _OED
                            172 	.globl _OEC
                            173 	.globl _OEB
                            174 	.globl _OEA
                            175 	.globl _IOE
                            176 	.globl _IOD
                            177 	.globl _AUTOPTRSETUP
                            178 	.globl _EP68FIFOFLGS
                            179 	.globl _EP24FIFOFLGS
                            180 	.globl _EP2468STAT
                            181 	.globl _IE
                            182 	.globl _INT4CLR
                            183 	.globl _INT2CLR
                            184 	.globl _IOC
                            185 	.globl _AUTODAT2
                            186 	.globl _AUTOPTRL2
                            187 	.globl _AUTOPTRH2
                            188 	.globl _AUTODAT1
                            189 	.globl _APTR1L
                            190 	.globl _APTR1H
                            191 	.globl _SBUF0
                            192 	.globl _SCON0
                            193 	.globl _MPAGE
                            194 	.globl _EXIF
                            195 	.globl _IOB
                            196 	.globl _CKCON
                            197 	.globl _TH1
                            198 	.globl _TH0
                            199 	.globl _TL1
                            200 	.globl _TL0
                            201 	.globl _TMOD
                            202 	.globl _TCON
                            203 	.globl _PCON
                            204 	.globl _DPS
                            205 	.globl _DPH1
                            206 	.globl _DPL1
                            207 	.globl _DPH
                            208 	.globl _DPL
                            209 	.globl _SP
                            210 	.globl _IOA
                            211 	.globl _CPLD_GPIO
                            212 	.globl _RFFE_CNTRL
                            213 	.globl _ATTN
                            214 	.globl _DEBUG_LED_REG
                            215 	.globl _LTC2208_CNTRL_REG
                            216 	.globl _DDC_CNTRL
                            217 	.globl _DDC1_AMR
                            218 	.globl _DDC1_LAR
                            219 	.globl _DDC1_DR4
                            220 	.globl _DDC1_DR3
                            221 	.globl _DDC1_DR2
                            222 	.globl _DDC1_DR1
                            223 	.globl _DDC1_DR0
                            224 	.globl _DDC0_AMR
                            225 	.globl _DDC0_LAR
                            226 	.globl _DDC0_DR4
                            227 	.globl _DDC0_DR3
                            228 	.globl _DDC0_DR2
                            229 	.globl _DDC0_DR1
                            230 	.globl _DDC0_DR0
                            231 	.globl _EP8FIFOBUF
                            232 	.globl _EP6FIFOBUF
                            233 	.globl _EP4FIFOBUF
                            234 	.globl _EP2FIFOBUF
                            235 	.globl _EP1INBUF
                            236 	.globl _EP1OUTBUF
                            237 	.globl _EP0BUF
                            238 	.globl _CT4
                            239 	.globl _CT3
                            240 	.globl _CT2
                            241 	.globl _CT1
                            242 	.globl _USBTEST
                            243 	.globl _TESTCFG
                            244 	.globl _DBUG
                            245 	.globl _UDMACRCQUAL
                            246 	.globl _UDMACRCL
                            247 	.globl _UDMACRCH
                            248 	.globl _GPIFHOLDAMOUNT
                            249 	.globl _FLOWSTBHPERIOD
                            250 	.globl _FLOWSTBEDGE
                            251 	.globl _FLOWSTB
                            252 	.globl _FLOWHOLDOFF
                            253 	.globl _FLOWEQ1CTL
                            254 	.globl _FLOWEQ0CTL
                            255 	.globl _FLOWLOGIC
                            256 	.globl _FLOWSTATE
                            257 	.globl _GPIFABORT
                            258 	.globl _GPIFREADYSTAT
                            259 	.globl _GPIFREADYCFG
                            260 	.globl _XGPIFSGLDATLNOX
                            261 	.globl _XGPIFSGLDATLX
                            262 	.globl _XGPIFSGLDATH
                            263 	.globl _EP8GPIFTRIG
                            264 	.globl _EP8GPIFPFSTOP
                            265 	.globl _EP8GPIFFLGSEL
                            266 	.globl _EP6GPIFTRIG
                            267 	.globl _EP6GPIFPFSTOP
                            268 	.globl _EP6GPIFFLGSEL
                            269 	.globl _EP4GPIFTRIG
                            270 	.globl _EP4GPIFPFSTOP
                            271 	.globl _EP4GPIFFLGSEL
                            272 	.globl _EP2GPIFTRIG
                            273 	.globl _EP2GPIFPFSTOP
                            274 	.globl _EP2GPIFFLGSEL
                            275 	.globl _GPIFTCB0
                            276 	.globl _GPIFTCB1
                            277 	.globl _GPIFTCB2
                            278 	.globl _GPIFTCB3
                            279 	.globl _GPIFADRL
                            280 	.globl _GPIFADRH
                            281 	.globl _GPIFCTLCFG
                            282 	.globl _GPIFIDLECTL
                            283 	.globl _GPIFIDLECS
                            284 	.globl _GPIFWFSELECT
                            285 	.globl _SETUPDAT
                            286 	.globl _SUDPTRCTL
                            287 	.globl _SUDPTRL
                            288 	.globl _SUDPTRH
                            289 	.globl _EP8FIFOBCL
                            290 	.globl _EP8FIFOBCH
                            291 	.globl _EP6FIFOBCL
                            292 	.globl _EP6FIFOBCH
                            293 	.globl _EP4FIFOBCL
                            294 	.globl _EP4FIFOBCH
                            295 	.globl _EP2FIFOBCL
                            296 	.globl _EP2FIFOBCH
                            297 	.globl _EP8FIFOFLGS
                            298 	.globl _EP6FIFOFLGS
                            299 	.globl _EP4FIFOFLGS
                            300 	.globl _EP2FIFOFLGS
                            301 	.globl _EP8CS
                            302 	.globl _EP6CS
                            303 	.globl _EP4CS
                            304 	.globl _EP2CS
                            305 	.globl _EP1INCS
                            306 	.globl _EP1OUTCS
                            307 	.globl _EP0CS
                            308 	.globl _EP8BCL
                            309 	.globl _EP8BCH
                            310 	.globl _EP6BCL
                            311 	.globl _EP6BCH
                            312 	.globl _EP4BCL
                            313 	.globl _EP4BCH
                            314 	.globl _EP2BCL
                            315 	.globl _EP2BCH
                            316 	.globl _EP1INBC
                            317 	.globl _EP1OUTBC
                            318 	.globl _EP0BCL
                            319 	.globl _EP0BCH
                            320 	.globl _FNADDR
                            321 	.globl _MICROFRAME
                            322 	.globl _USBFRAMEL
                            323 	.globl _USBFRAMEH
                            324 	.globl _TOGCTL
                            325 	.globl _WAKEUPCS
                            326 	.globl _SUSPEND
                            327 	.globl _USBCS
                            328 	.globl _XAUTODAT2
                            329 	.globl _XAUTODAT1
                            330 	.globl _I2CTL
                            331 	.globl _I2DAT
                            332 	.globl _I2CS
                            333 	.globl _PORTECFG
                            334 	.globl _PORTCCFG
                            335 	.globl _PORTACFG
                            336 	.globl _INTSETUP
                            337 	.globl _INT4IVEC
                            338 	.globl _INT2IVEC
                            339 	.globl _CLRERRCNT
                            340 	.globl _ERRCNTLIM
                            341 	.globl _USBERRIRQ
                            342 	.globl _USBERRIE
                            343 	.globl _GPIFIRQ
                            344 	.globl _GPIFIE
                            345 	.globl _EPIRQ
                            346 	.globl _EPIE
                            347 	.globl _USBIRQ
                            348 	.globl _USBIE
                            349 	.globl _NAKIRQ
                            350 	.globl _NAKIE
                            351 	.globl _IBNIRQ
                            352 	.globl _IBNIE
                            353 	.globl _EP8FIFOIRQ
                            354 	.globl _EP8FIFOIE
                            355 	.globl _EP6FIFOIRQ
                            356 	.globl _EP6FIFOIE
                            357 	.globl _EP4FIFOIRQ
                            358 	.globl _EP4FIFOIE
                            359 	.globl _EP2FIFOIRQ
                            360 	.globl _EP2FIFOIE
                            361 	.globl _OUTPKTEND
                            362 	.globl _INPKTEND
                            363 	.globl _EP8ISOINPKTS
                            364 	.globl _EP6ISOINPKTS
                            365 	.globl _EP4ISOINPKTS
                            366 	.globl _EP2ISOINPKTS
                            367 	.globl _EP8FIFOPFL
                            368 	.globl _EP8FIFOPFH
                            369 	.globl _EP6FIFOPFL
                            370 	.globl _EP6FIFOPFH
                            371 	.globl _EP4FIFOPFL
                            372 	.globl _EP4FIFOPFH
                            373 	.globl _EP2FIFOPFL
                            374 	.globl _EP2FIFOPFH
                            375 	.globl _EP8AUTOINLENL
                            376 	.globl _EP8AUTOINLENH
                            377 	.globl _EP6AUTOINLENL
                            378 	.globl _EP6AUTOINLENH
                            379 	.globl _EP4AUTOINLENL
                            380 	.globl _EP4AUTOINLENH
                            381 	.globl _EP2AUTOINLENL
                            382 	.globl _EP2AUTOINLENH
                            383 	.globl _EP8FIFOCFG
                            384 	.globl _EP6FIFOCFG
                            385 	.globl _EP4FIFOCFG
                            386 	.globl _EP2FIFOCFG
                            387 	.globl _EP8CFG
                            388 	.globl _EP6CFG
                            389 	.globl _EP4CFG
                            390 	.globl _EP2CFG
                            391 	.globl _EP1INCFG
                            392 	.globl _EP1OUTCFG
                            393 	.globl _REVCTL
                            394 	.globl _REVID
                            395 	.globl _FIFOPINPOLAR
                            396 	.globl _UART230
                            397 	.globl _BPADDRL
                            398 	.globl _BPADDRH
                            399 	.globl _BREAKPT
                            400 	.globl _FIFORESET
                            401 	.globl _PINFLAGSCD
                            402 	.globl _PINFLAGSAB
                            403 	.globl _IFCONFIG
                            404 	.globl _CPUCS
                            405 	.globl _RES_WAVEDATA_END
                            406 	.globl _GPIF_WAVE_DATA
                            407 	.globl _read_MCR
                            408 	.globl _set_MCR
                            409 	.globl _read_NCR
                            410 	.globl _set_NCR
                            411 	.globl _read_NSCR
                            412 	.globl _set_NSCR
                            413 	.globl _read_NFRE
                            414 	.globl _set_NFRE
                            415 	.globl _read_NPHA
                            416 	.globl _set_NPHA
                            417 	.globl _read_C2SR
                            418 	.globl _set_C2SR
                            419 	.globl _read_C2DEC
                            420 	.globl _set_C2DEC
                            421 	.globl _read_C5SR
                            422 	.globl _set_C5SR
                            423 	.globl _read_C5DEC
                            424 	.globl _set_C5DEC
                            425 	.globl _read_RCR
                            426 	.globl _set_RCR
                            427 	.globl _read_RDEC
                            428 	.globl _set_RDEC
                            429 	.globl _read_RAO
                            430 	.globl _set_RAO
                            431 	.globl _read_RTAP
                            432 	.globl _set_RTAP
                            433 	.globl _read_RSRV
                            434 	.globl _set_RSRV
                            435 	.globl _clr_RAM
                            436 	.globl _clr_COEF
                            437 	.globl _read_COEF
                            438 	.globl _set_COEF
                            439 ;--------------------------------------------------------
                            440 ; special function registers
                            441 ;--------------------------------------------------------
                            442 	.area RSEG    (DATA)
                    0080    443 _IOA	=	0x0080
                    0081    444 _SP	=	0x0081
                    0082    445 _DPL	=	0x0082
                    0083    446 _DPH	=	0x0083
                    0084    447 _DPL1	=	0x0084
                    0085    448 _DPH1	=	0x0085
                    0086    449 _DPS	=	0x0086
                    0087    450 _PCON	=	0x0087
                    0088    451 _TCON	=	0x0088
                    0089    452 _TMOD	=	0x0089
                    008A    453 _TL0	=	0x008a
                    008B    454 _TL1	=	0x008b
                    008C    455 _TH0	=	0x008c
                    008D    456 _TH1	=	0x008d
                    008E    457 _CKCON	=	0x008e
                    0090    458 _IOB	=	0x0090
                    0091    459 _EXIF	=	0x0091
                    0092    460 _MPAGE	=	0x0092
                    0098    461 _SCON0	=	0x0098
                    0099    462 _SBUF0	=	0x0099
                    009A    463 _APTR1H	=	0x009a
                    009B    464 _APTR1L	=	0x009b
                    009C    465 _AUTODAT1	=	0x009c
                    009D    466 _AUTOPTRH2	=	0x009d
                    009E    467 _AUTOPTRL2	=	0x009e
                    009F    468 _AUTODAT2	=	0x009f
                    00A0    469 _IOC	=	0x00a0
                    00A1    470 _INT2CLR	=	0x00a1
                    00A2    471 _INT4CLR	=	0x00a2
                    00A8    472 _IE	=	0x00a8
                    00AA    473 _EP2468STAT	=	0x00aa
                    00AB    474 _EP24FIFOFLGS	=	0x00ab
                    00AC    475 _EP68FIFOFLGS	=	0x00ac
                    00AF    476 _AUTOPTRSETUP	=	0x00af
                    00B0    477 _IOD	=	0x00b0
                    00B1    478 _IOE	=	0x00b1
                    00B2    479 _OEA	=	0x00b2
                    00B3    480 _OEB	=	0x00b3
                    00B4    481 _OEC	=	0x00b4
                    00B5    482 _OED	=	0x00b5
                    00B6    483 _OEE	=	0x00b6
                    00B8    484 _IP	=	0x00b8
                    00BA    485 _EP01STAT	=	0x00ba
                    00BB    486 _GPIFTRIG	=	0x00bb
                    00BD    487 _GPIFSGLDATH	=	0x00bd
                    00BE    488 _GPIFSGLDATLX	=	0x00be
                    00BF    489 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    490 _SCON1	=	0x00c0
                    00C1    491 _SBUF1	=	0x00c1
                    00C8    492 _T2CON	=	0x00c8
                    00CA    493 _RCAP2L	=	0x00ca
                    00CB    494 _RCAP2H	=	0x00cb
                    00CC    495 _TL2	=	0x00cc
                    00CD    496 _TH2	=	0x00cd
                    00D0    497 _PSW	=	0x00d0
                    00D8    498 _EICON	=	0x00d8
                    00E0    499 _ACC	=	0x00e0
                    00E8    500 _EIE	=	0x00e8
                    00F0    501 _B	=	0x00f0
                    00F8    502 _EIP	=	0x00f8
                            503 ;--------------------------------------------------------
                            504 ; special function bits 
                            505 ;--------------------------------------------------------
                            506 	.area RSEG    (DATA)
                    0086    507 _SEL	=	0x0086
                    0088    508 _IT0	=	0x0088
                    0089    509 _IE0	=	0x0089
                    008A    510 _IT1	=	0x008a
                    008B    511 _IE1	=	0x008b
                    008C    512 _TR0	=	0x008c
                    008D    513 _TF0	=	0x008d
                    008E    514 _TR1	=	0x008e
                    008F    515 _TF1	=	0x008f
                    0098    516 _RI	=	0x0098
                    0099    517 _TI	=	0x0099
                    009A    518 _RB8	=	0x009a
                    009B    519 _TB8	=	0x009b
                    009C    520 _REN	=	0x009c
                    009D    521 _SM2	=	0x009d
                    009E    522 _SM1	=	0x009e
                    009F    523 _SM0	=	0x009f
                    00A8    524 _EX0	=	0x00a8
                    00A9    525 _ET0	=	0x00a9
                    00AA    526 _EX1	=	0x00aa
                    00AB    527 _ET1	=	0x00ab
                    00AC    528 _ES0	=	0x00ac
                    00AD    529 _ET2	=	0x00ad
                    00AE    530 _ES1	=	0x00ae
                    00AF    531 _EA	=	0x00af
                    00B8    532 _PX0	=	0x00b8
                    00B9    533 _PT0	=	0x00b9
                    00BA    534 _PX1	=	0x00ba
                    00BB    535 _PT1	=	0x00bb
                    00BC    536 _PS0	=	0x00bc
                    00BD    537 _PT2	=	0x00bd
                    00BE    538 _PS1	=	0x00be
                    00C0    539 _RI1	=	0x00c0
                    00C1    540 _TI1	=	0x00c1
                    00C2    541 _RB81	=	0x00c2
                    00C3    542 _TB81	=	0x00c3
                    00C4    543 _REN1	=	0x00c4
                    00C5    544 _SM21	=	0x00c5
                    00C6    545 _SM11	=	0x00c6
                    00C7    546 _SM01	=	0x00c7
                    00C8    547 _CP_RL2	=	0x00c8
                    00C9    548 _C_T2	=	0x00c9
                    00CA    549 _TR2	=	0x00ca
                    00CB    550 _EXEN2	=	0x00cb
                    00CC    551 _TCLK	=	0x00cc
                    00CD    552 _RCLK	=	0x00cd
                    00CE    553 _EXF2	=	0x00ce
                    00CF    554 _TF2	=	0x00cf
                    00D0    555 _P	=	0x00d0
                    00D1    556 _FL	=	0x00d1
                    00D2    557 _OV	=	0x00d2
                    00D3    558 _RS0	=	0x00d3
                    00D4    559 _RS1	=	0x00d4
                    00D5    560 _F0	=	0x00d5
                    00D6    561 _AC	=	0x00d6
                    00D7    562 _CY	=	0x00d7
                    00DB    563 _INT6	=	0x00db
                    00DC    564 _RESI	=	0x00dc
                    00DD    565 _ERESI	=	0x00dd
                    00DF    566 _SMOD1	=	0x00df
                    00E8    567 _EIUSB	=	0x00e8
                    00E9    568 _EI2C	=	0x00e9
                    00EA    569 _EIEX4	=	0x00ea
                    00EB    570 _EIEX5	=	0x00eb
                    00EC    571 _EIEX6	=	0x00ec
                    00F8    572 _PUSB	=	0x00f8
                    00F9    573 _PI2C	=	0x00f9
                    00FA    574 _EIPX4	=	0x00fa
                    00FB    575 _EIPX5	=	0x00fb
                    00FC    576 _EIPX6	=	0x00fc
                    0080    577 _bitS_OUT	=	0x0080
                    0081    578 _bitS_IN	=	0x0081
                    0083    579 _bitS_CLK	=	0x0083
                    00A0    580 _bitALTERA_DATA0	=	0x00a0
                    00A2    581 _bitALTERA_DCLK	=	0x00a2
                    00A6    582 _bitFPGA_CS	=	0x00a6
                    00A7    583 _bitCPLD_CS	=	0x00a7
                            584 ;--------------------------------------------------------
                            585 ; overlayable register banks 
                            586 ;--------------------------------------------------------
                            587 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     588 	.ds 8
                            589 ;--------------------------------------------------------
                            590 ; internal ram data
                            591 ;--------------------------------------------------------
                            592 	.area DSEG    (DATA)
                            593 ;--------------------------------------------------------
                            594 ; overlayable items in internal ram 
                            595 ;--------------------------------------------------------
                            596 	.area	OSEG    (OVR,DATA)
   002B                     597 _read_MCR_PARM_2::
   002B                     598 	.ds 2
   002D                     599 _read_MCR_PARM_3::
   002D                     600 	.ds 1
                            601 	.area	OSEG    (OVR,DATA)
   002B                     602 _set_MCR_PARM_2::
   002B                     603 	.ds 2
   002D                     604 _set_MCR_PARM_3::
   002D                     605 	.ds 1
                            606 	.area	OSEG    (OVR,DATA)
   002B                     607 _read_NCR_PARM_2::
   002B                     608 	.ds 2
   002D                     609 _read_NCR_PARM_3::
   002D                     610 	.ds 1
                            611 	.area	OSEG    (OVR,DATA)
   002B                     612 _set_NCR_PARM_2::
   002B                     613 	.ds 2
   002D                     614 _set_NCR_PARM_3::
   002D                     615 	.ds 1
                            616 	.area	OSEG    (OVR,DATA)
   002B                     617 _read_NSCR_PARM_2::
   002B                     618 	.ds 2
   002D                     619 _read_NSCR_PARM_3::
   002D                     620 	.ds 1
                            621 	.area	OSEG    (OVR,DATA)
   002B                     622 _set_NSCR_PARM_2::
   002B                     623 	.ds 2
   002D                     624 _set_NSCR_PARM_3::
   002D                     625 	.ds 1
                            626 	.area	OSEG    (OVR,DATA)
   002B                     627 _read_NFRE_PARM_2::
   002B                     628 	.ds 2
   002D                     629 _read_NFRE_PARM_3::
   002D                     630 	.ds 1
                            631 	.area	OSEG    (OVR,DATA)
   002B                     632 _set_NFRE_PARM_2::
   002B                     633 	.ds 2
   002D                     634 _set_NFRE_PARM_3::
   002D                     635 	.ds 1
                            636 	.area	OSEG    (OVR,DATA)
   002B                     637 _read_NPHA_PARM_2::
   002B                     638 	.ds 2
   002D                     639 _read_NPHA_PARM_3::
   002D                     640 	.ds 1
                            641 	.area	OSEG    (OVR,DATA)
   002B                     642 _set_NPHA_PARM_2::
   002B                     643 	.ds 2
   002D                     644 _set_NPHA_PARM_3::
   002D                     645 	.ds 1
                            646 	.area	OSEG    (OVR,DATA)
   002B                     647 _read_C2SR_PARM_2::
   002B                     648 	.ds 2
   002D                     649 _read_C2SR_PARM_3::
   002D                     650 	.ds 1
                            651 	.area	OSEG    (OVR,DATA)
   002B                     652 _set_C2SR_PARM_2::
   002B                     653 	.ds 2
   002D                     654 _set_C2SR_PARM_3::
   002D                     655 	.ds 1
                            656 	.area	OSEG    (OVR,DATA)
   002B                     657 _read_C2DEC_PARM_2::
   002B                     658 	.ds 2
   002D                     659 _read_C2DEC_PARM_3::
   002D                     660 	.ds 1
                            661 	.area	OSEG    (OVR,DATA)
   002B                     662 _set_C2DEC_PARM_2::
   002B                     663 	.ds 2
   002D                     664 _set_C2DEC_PARM_3::
   002D                     665 	.ds 1
                            666 	.area	OSEG    (OVR,DATA)
   002B                     667 _read_C5SR_PARM_2::
   002B                     668 	.ds 2
   002D                     669 _read_C5SR_PARM_3::
   002D                     670 	.ds 1
                            671 	.area	OSEG    (OVR,DATA)
   002B                     672 _set_C5SR_PARM_2::
   002B                     673 	.ds 2
   002D                     674 _set_C5SR_PARM_3::
   002D                     675 	.ds 1
                            676 	.area	OSEG    (OVR,DATA)
   002B                     677 _read_C5DEC_PARM_2::
   002B                     678 	.ds 2
   002D                     679 _read_C5DEC_PARM_3::
   002D                     680 	.ds 1
                            681 	.area	OSEG    (OVR,DATA)
   002B                     682 _set_C5DEC_PARM_2::
   002B                     683 	.ds 2
   002D                     684 _set_C5DEC_PARM_3::
   002D                     685 	.ds 1
                            686 	.area	OSEG    (OVR,DATA)
   002B                     687 _read_RCR_PARM_2::
   002B                     688 	.ds 2
   002D                     689 _read_RCR_PARM_3::
   002D                     690 	.ds 1
                            691 	.area	OSEG    (OVR,DATA)
   002B                     692 _set_RCR_PARM_2::
   002B                     693 	.ds 2
   002D                     694 _set_RCR_PARM_3::
   002D                     695 	.ds 1
                            696 	.area	OSEG    (OVR,DATA)
   002B                     697 _read_RDEC_PARM_2::
   002B                     698 	.ds 2
   002D                     699 _read_RDEC_PARM_3::
   002D                     700 	.ds 1
                            701 	.area	OSEG    (OVR,DATA)
   002B                     702 _set_RDEC_PARM_2::
   002B                     703 	.ds 2
   002D                     704 _set_RDEC_PARM_3::
   002D                     705 	.ds 1
                            706 	.area	OSEG    (OVR,DATA)
   002B                     707 _read_RAO_PARM_2::
   002B                     708 	.ds 2
   002D                     709 _read_RAO_PARM_3::
   002D                     710 	.ds 1
                            711 	.area	OSEG    (OVR,DATA)
   002B                     712 _set_RAO_PARM_2::
   002B                     713 	.ds 2
   002D                     714 _set_RAO_PARM_3::
   002D                     715 	.ds 1
                            716 	.area	OSEG    (OVR,DATA)
   002B                     717 _read_RTAP_PARM_2::
   002B                     718 	.ds 2
   002D                     719 _read_RTAP_PARM_3::
   002D                     720 	.ds 1
                            721 	.area	OSEG    (OVR,DATA)
   002B                     722 _set_RTAP_PARM_2::
   002B                     723 	.ds 2
   002D                     724 _set_RTAP_PARM_3::
   002D                     725 	.ds 1
                            726 	.area	OSEG    (OVR,DATA)
   002B                     727 _read_RSRV_PARM_2::
   002B                     728 	.ds 2
   002D                     729 _read_RSRV_PARM_3::
   002D                     730 	.ds 1
                            731 	.area	OSEG    (OVR,DATA)
   002B                     732 _set_RSRV_PARM_2::
   002B                     733 	.ds 2
   002D                     734 _set_RSRV_PARM_3::
   002D                     735 	.ds 1
                            736 	.area	OSEG    (OVR,DATA)
                            737 	.area	OSEG    (OVR,DATA)
                            738 	.area	OSEG    (OVR,DATA)
   002B                     739 _read_COEF_PARM_2::
   002B                     740 	.ds 1
   002C                     741 _read_COEF_PARM_3::
   002C                     742 	.ds 2
   002E                     743 _read_COEF_PARM_4::
   002E                     744 	.ds 1
   002F                     745 _read_COEF_i_1_1::
   002F                     746 	.ds 2
                            747 	.area	OSEG    (OVR,DATA)
   002B                     748 _set_COEF_PARM_2::
   002B                     749 	.ds 1
   002C                     750 _set_COEF_PARM_3::
   002C                     751 	.ds 2
   002E                     752 _set_COEF_PARM_4::
   002E                     753 	.ds 1
                            754 ;--------------------------------------------------------
                            755 ; indirectly addressable internal ram data
                            756 ;--------------------------------------------------------
                            757 	.area ISEG    (DATA)
                            758 ;--------------------------------------------------------
                            759 ; bit data
                            760 ;--------------------------------------------------------
                            761 	.area BSEG    (BIT)
                            762 ;--------------------------------------------------------
                            763 ; paged external ram data
                            764 ;--------------------------------------------------------
                            765 	.area PSEG    (PAG,XDATA)
                            766 ;--------------------------------------------------------
                            767 ; external ram data
                            768 ;--------------------------------------------------------
                            769 	.area XSEG    (XDATA)
                    E400    770 _GPIF_WAVE_DATA	=	0xe400
                    E480    771 _RES_WAVEDATA_END	=	0xe480
                    E600    772 _CPUCS	=	0xe600
                    E601    773 _IFCONFIG	=	0xe601
                    E602    774 _PINFLAGSAB	=	0xe602
                    E603    775 _PINFLAGSCD	=	0xe603
                    E604    776 _FIFORESET	=	0xe604
                    E605    777 _BREAKPT	=	0xe605
                    E606    778 _BPADDRH	=	0xe606
                    E607    779 _BPADDRL	=	0xe607
                    E608    780 _UART230	=	0xe608
                    E609    781 _FIFOPINPOLAR	=	0xe609
                    E60A    782 _REVID	=	0xe60a
                    E60B    783 _REVCTL	=	0xe60b
                    E610    784 _EP1OUTCFG	=	0xe610
                    E611    785 _EP1INCFG	=	0xe611
                    E612    786 _EP2CFG	=	0xe612
                    E613    787 _EP4CFG	=	0xe613
                    E614    788 _EP6CFG	=	0xe614
                    E615    789 _EP8CFG	=	0xe615
                    E618    790 _EP2FIFOCFG	=	0xe618
                    E619    791 _EP4FIFOCFG	=	0xe619
                    E61A    792 _EP6FIFOCFG	=	0xe61a
                    E61B    793 _EP8FIFOCFG	=	0xe61b
                    E620    794 _EP2AUTOINLENH	=	0xe620
                    E621    795 _EP2AUTOINLENL	=	0xe621
                    E622    796 _EP4AUTOINLENH	=	0xe622
                    E623    797 _EP4AUTOINLENL	=	0xe623
                    E624    798 _EP6AUTOINLENH	=	0xe624
                    E625    799 _EP6AUTOINLENL	=	0xe625
                    E626    800 _EP8AUTOINLENH	=	0xe626
                    E627    801 _EP8AUTOINLENL	=	0xe627
                    E630    802 _EP2FIFOPFH	=	0xe630
                    E631    803 _EP2FIFOPFL	=	0xe631
                    E632    804 _EP4FIFOPFH	=	0xe632
                    E633    805 _EP4FIFOPFL	=	0xe633
                    E634    806 _EP6FIFOPFH	=	0xe634
                    E635    807 _EP6FIFOPFL	=	0xe635
                    E636    808 _EP8FIFOPFH	=	0xe636
                    E637    809 _EP8FIFOPFL	=	0xe637
                    E640    810 _EP2ISOINPKTS	=	0xe640
                    E641    811 _EP4ISOINPKTS	=	0xe641
                    E642    812 _EP6ISOINPKTS	=	0xe642
                    E643    813 _EP8ISOINPKTS	=	0xe643
                    E648    814 _INPKTEND	=	0xe648
                    E649    815 _OUTPKTEND	=	0xe649
                    E650    816 _EP2FIFOIE	=	0xe650
                    E651    817 _EP2FIFOIRQ	=	0xe651
                    E652    818 _EP4FIFOIE	=	0xe652
                    E653    819 _EP4FIFOIRQ	=	0xe653
                    E654    820 _EP6FIFOIE	=	0xe654
                    E655    821 _EP6FIFOIRQ	=	0xe655
                    E656    822 _EP8FIFOIE	=	0xe656
                    E657    823 _EP8FIFOIRQ	=	0xe657
                    E658    824 _IBNIE	=	0xe658
                    E659    825 _IBNIRQ	=	0xe659
                    E65A    826 _NAKIE	=	0xe65a
                    E65B    827 _NAKIRQ	=	0xe65b
                    E65C    828 _USBIE	=	0xe65c
                    E65D    829 _USBIRQ	=	0xe65d
                    E65E    830 _EPIE	=	0xe65e
                    E65F    831 _EPIRQ	=	0xe65f
                    E660    832 _GPIFIE	=	0xe660
                    E661    833 _GPIFIRQ	=	0xe661
                    E662    834 _USBERRIE	=	0xe662
                    E663    835 _USBERRIRQ	=	0xe663
                    E664    836 _ERRCNTLIM	=	0xe664
                    E665    837 _CLRERRCNT	=	0xe665
                    E666    838 _INT2IVEC	=	0xe666
                    E667    839 _INT4IVEC	=	0xe667
                    E668    840 _INTSETUP	=	0xe668
                    E670    841 _PORTACFG	=	0xe670
                    E671    842 _PORTCCFG	=	0xe671
                    E672    843 _PORTECFG	=	0xe672
                    E678    844 _I2CS	=	0xe678
                    E679    845 _I2DAT	=	0xe679
                    E67A    846 _I2CTL	=	0xe67a
                    E67B    847 _XAUTODAT1	=	0xe67b
                    E67C    848 _XAUTODAT2	=	0xe67c
                    E680    849 _USBCS	=	0xe680
                    E681    850 _SUSPEND	=	0xe681
                    E682    851 _WAKEUPCS	=	0xe682
                    E683    852 _TOGCTL	=	0xe683
                    E684    853 _USBFRAMEH	=	0xe684
                    E685    854 _USBFRAMEL	=	0xe685
                    E686    855 _MICROFRAME	=	0xe686
                    E687    856 _FNADDR	=	0xe687
                    E68A    857 _EP0BCH	=	0xe68a
                    E68B    858 _EP0BCL	=	0xe68b
                    E68D    859 _EP1OUTBC	=	0xe68d
                    E68F    860 _EP1INBC	=	0xe68f
                    E690    861 _EP2BCH	=	0xe690
                    E691    862 _EP2BCL	=	0xe691
                    E694    863 _EP4BCH	=	0xe694
                    E695    864 _EP4BCL	=	0xe695
                    E698    865 _EP6BCH	=	0xe698
                    E699    866 _EP6BCL	=	0xe699
                    E69C    867 _EP8BCH	=	0xe69c
                    E69D    868 _EP8BCL	=	0xe69d
                    E6A0    869 _EP0CS	=	0xe6a0
                    E6A1    870 _EP1OUTCS	=	0xe6a1
                    E6A2    871 _EP1INCS	=	0xe6a2
                    E6A3    872 _EP2CS	=	0xe6a3
                    E6A4    873 _EP4CS	=	0xe6a4
                    E6A5    874 _EP6CS	=	0xe6a5
                    E6A6    875 _EP8CS	=	0xe6a6
                    E6A7    876 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    877 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    878 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    879 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    880 _EP2FIFOBCH	=	0xe6ab
                    E6AC    881 _EP2FIFOBCL	=	0xe6ac
                    E6AD    882 _EP4FIFOBCH	=	0xe6ad
                    E6AE    883 _EP4FIFOBCL	=	0xe6ae
                    E6AF    884 _EP6FIFOBCH	=	0xe6af
                    E6B0    885 _EP6FIFOBCL	=	0xe6b0
                    E6B1    886 _EP8FIFOBCH	=	0xe6b1
                    E6B2    887 _EP8FIFOBCL	=	0xe6b2
                    E6B3    888 _SUDPTRH	=	0xe6b3
                    E6B4    889 _SUDPTRL	=	0xe6b4
                    E6B5    890 _SUDPTRCTL	=	0xe6b5
                    E6B8    891 _SETUPDAT	=	0xe6b8
                    E6C0    892 _GPIFWFSELECT	=	0xe6c0
                    E6C1    893 _GPIFIDLECS	=	0xe6c1
                    E6C2    894 _GPIFIDLECTL	=	0xe6c2
                    E6C3    895 _GPIFCTLCFG	=	0xe6c3
                    E6C4    896 _GPIFADRH	=	0xe6c4
                    E6C5    897 _GPIFADRL	=	0xe6c5
                    E6CE    898 _GPIFTCB3	=	0xe6ce
                    E6CF    899 _GPIFTCB2	=	0xe6cf
                    E6D0    900 _GPIFTCB1	=	0xe6d0
                    E6D1    901 _GPIFTCB0	=	0xe6d1
                    E6D2    902 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    903 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    904 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    905 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    906 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    907 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    908 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    909 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    910 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    911 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    912 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    913 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    914 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    915 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    916 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    917 _GPIFREADYCFG	=	0xe6f3
                    E6F4    918 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    919 _GPIFABORT	=	0xe6f5
                    E6C6    920 _FLOWSTATE	=	0xe6c6
                    E6C7    921 _FLOWLOGIC	=	0xe6c7
                    E6C8    922 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    923 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    924 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    925 _FLOWSTB	=	0xe6cb
                    E6CC    926 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    927 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    928 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    929 _UDMACRCH	=	0xe67d
                    E67E    930 _UDMACRCL	=	0xe67e
                    E67F    931 _UDMACRCQUAL	=	0xe67f
                    E6F8    932 _DBUG	=	0xe6f8
                    E6F9    933 _TESTCFG	=	0xe6f9
                    E6FA    934 _USBTEST	=	0xe6fa
                    E6FB    935 _CT1	=	0xe6fb
                    E6FC    936 _CT2	=	0xe6fc
                    E6FD    937 _CT3	=	0xe6fd
                    E6FE    938 _CT4	=	0xe6fe
                    E740    939 _EP0BUF	=	0xe740
                    E780    940 _EP1OUTBUF	=	0xe780
                    E7C0    941 _EP1INBUF	=	0xe7c0
                    F000    942 _EP2FIFOBUF	=	0xf000
                    F400    943 _EP4FIFOBUF	=	0xf400
                    F800    944 _EP6FIFOBUF	=	0xf800
                    FC00    945 _EP8FIFOBUF	=	0xfc00
                    6000    946 _DDC0_DR0	=	0x6000
                    6001    947 _DDC0_DR1	=	0x6001
                    6002    948 _DDC0_DR2	=	0x6002
                    6003    949 _DDC0_DR3	=	0x6003
                    6004    950 _DDC0_DR4	=	0x6004
                    6006    951 _DDC0_LAR	=	0x6006
                    6007    952 _DDC0_AMR	=	0x6007
                    6010    953 _DDC1_DR0	=	0x6010
                    6011    954 _DDC1_DR1	=	0x6011
                    6012    955 _DDC1_DR2	=	0x6012
                    6013    956 _DDC1_DR3	=	0x6013
                    6014    957 _DDC1_DR4	=	0x6014
                    6016    958 _DDC1_LAR	=	0x6016
                    6017    959 _DDC1_AMR	=	0x6017
                    6020    960 _DDC_CNTRL	=	0x6020
                    6021    961 _LTC2208_CNTRL_REG	=	0x6021
                    6022    962 _DEBUG_LED_REG	=	0x6022
                    6023    963 _ATTN	=	0x6023
                    6024    964 _RFFE_CNTRL	=	0x6024
                    6025    965 _CPLD_GPIO	=	0x6025
                            966 ;--------------------------------------------------------
                            967 ; external initialized ram data
                            968 ;--------------------------------------------------------
                            969 	.area CSEG    (CODE)
                            970 	.area GSINIT0 (CODE)
                            971 	.area GSINIT1 (CODE)
                            972 	.area GSINIT2 (CODE)
                            973 	.area GSINIT3 (CODE)
                            974 	.area GSINIT4 (CODE)
                            975 	.area GSINIT5 (CODE)
                            976 ;--------------------------------------------------------
                            977 ; global & static initialisations
                            978 ;--------------------------------------------------------
                            979 	.area CSEG    (CODE)
                            980 	.area GSINIT  (CODE)
                            981 	.area GSFINAL (CODE)
                            982 	.area GSINIT  (CODE)
                            983 ;--------------------------------------------------------
                            984 ; Home
                            985 ;--------------------------------------------------------
                            986 	.area HOME    (CODE)
                            987 	.area CSEG    (CODE)
                            988 ;--------------------------------------------------------
                            989 ; code
                            990 ;--------------------------------------------------------
                            991 	.area CSEG    (CODE)
                            992 ;------------------------------------------------------------
                            993 ;Allocation info for local variables in function 'read_MCR'
                            994 ;------------------------------------------------------------
                            995 ;buf                       Allocated with name '_read_MCR_PARM_2'
                            996 ;len                       Allocated with name '_read_MCR_PARM_3'
                            997 ;index                     Allocated to registers r2 
                            998 ;------------------------------------------------------------
                            999 ;src/ddc_regs.c:17: read_MCR (unsigned char index, xdata unsigned char *buf,
                           1000 ;	-----------------------------------------
                           1001 ;	 function read_MCR
                           1002 ;	-----------------------------------------
   01EA                    1003 _read_MCR:
                    0002   1004 	ar2 = 0x02
                    0003   1005 	ar3 = 0x03
                    0004   1006 	ar4 = 0x04
                    0005   1007 	ar5 = 0x05
                    0006   1008 	ar6 = 0x06
                    0007   1009 	ar7 = 0x07
                    0000   1010 	ar0 = 0x00
                    0001   1011 	ar1 = 0x01
                           1012 ;     genReceive
   01EA AA 82              1013 	mov	r2,dpl
                           1014 ;src/ddc_regs.c:20: if (index == 0)
                           1015 ;     genCmpEq
                           1016 ;	Peephole 112.b	changed ljmp to sjmp
                           1017 ;	Peephole 199	optimized misc jump sequence
   01EC BA 00 13           1018 	cjne	r2,#0x00,00105$
                           1019 ;00111$:
                           1020 ;	Peephole 200	removed redundant sjmp
   01EF                    1021 00112$:
                           1022 ;src/ddc_regs.c:22: DDC0_AMR = (MCR & 0xFF00) >> 8;
                           1023 ;     genAssign
   01EF 90 60 07           1024 	mov	dptr,#_DDC0_AMR
   01F2 74 03              1025 	mov	a,#0x03
   01F4 F0                 1026 	movx	@dptr,a
                           1027 ;src/ddc_regs.c:23: DDC0_LAR = (MCR & 0xFF);
                           1028 ;     genAssign
   01F5 90 60 06           1029 	mov	dptr,#_DDC0_LAR
                           1030 ;	Peephole 181	changed mov to clr
   01F8 E4                 1031 	clr	a
   01F9 F0                 1032 	movx	@dptr,a
                           1033 ;src/ddc_regs.c:24: *buf == DDC0_DR0;
                           1034 ;     genDummyRead
   01FA 90 60 00           1035 	mov	dptr,#_DDC0_DR0
   01FD E0                 1036 	movx	a,@dptr
                           1037 ;src/ddc_regs.c:25: return 1;
                           1038 ;     genRet
   01FE 75 82 01           1039 	mov	dpl,#0x01
                           1040 ;	Peephole 112.b	changed ljmp to sjmp
                           1041 ;	Peephole 251.b	replaced sjmp to ret with ret
   0201 22                 1042 	ret
   0202                    1043 00105$:
                           1044 ;src/ddc_regs.c:26: } else if (index == 1) {
                           1045 ;     genCmpEq
                           1046 ;	Peephole 112.b	changed ljmp to sjmp
                           1047 ;	Peephole 199	optimized misc jump sequence
   0202 BA 01 13           1048 	cjne	r2,#0x01,00102$
                           1049 ;00113$:
                           1050 ;	Peephole 200	removed redundant sjmp
   0205                    1051 00114$:
                           1052 ;src/ddc_regs.c:27: DDC1_AMR = (MCR & 0xFF00) >> 8;
                           1053 ;     genAssign
   0205 90 60 17           1054 	mov	dptr,#_DDC1_AMR
   0208 74 03              1055 	mov	a,#0x03
   020A F0                 1056 	movx	@dptr,a
                           1057 ;src/ddc_regs.c:28: DDC1_LAR = (MCR & 0xFF);
                           1058 ;     genAssign
   020B 90 60 16           1059 	mov	dptr,#_DDC1_LAR
                           1060 ;	Peephole 181	changed mov to clr
   020E E4                 1061 	clr	a
   020F F0                 1062 	movx	@dptr,a
                           1063 ;src/ddc_regs.c:29: *buf == DDC1_DR0;
                           1064 ;     genDummyRead
   0210 90 60 10           1065 	mov	dptr,#_DDC1_DR0
   0213 E0                 1066 	movx	a,@dptr
                           1067 ;src/ddc_regs.c:30: return 1;
                           1068 ;     genRet
   0214 75 82 01           1069 	mov	dpl,#0x01
                           1070 ;	Peephole 112.b	changed ljmp to sjmp
                           1071 ;src/ddc_regs.c:31: } else return 0;
                           1072 ;     genRet
                           1073 ;	Peephole 237.a	removed sjmp to ret
   0217 22                 1074 	ret
   0218                    1075 00102$:
   0218 75 82 00           1076 	mov	dpl,#0x00
   021B                    1077 00107$:
   021B 22                 1078 	ret
                           1079 ;------------------------------------------------------------
                           1080 ;Allocation info for local variables in function 'set_MCR'
                           1081 ;------------------------------------------------------------
                           1082 ;buf                       Allocated with name '_set_MCR_PARM_2'
                           1083 ;len                       Allocated with name '_set_MCR_PARM_3'
                           1084 ;index                     Allocated to registers r2 
                           1085 ;------------------------------------------------------------
                           1086 ;src/ddc_regs.c:36: set_MCR (unsigned char index, xdata unsigned char *buf,
                           1087 ;	-----------------------------------------
                           1088 ;	 function set_MCR
                           1089 ;	-----------------------------------------
   021C                    1090 _set_MCR:
                           1091 ;     genReceive
   021C AA 82              1092 	mov	r2,dpl
                           1093 ;src/ddc_regs.c:39: if (len < 1) return 0; // 8 bits
                           1094 ;     genCmpLt
                           1095 ;     genCmp
                           1096 ;     genIfxJump
                           1097 ;	Peephole 108	removed ljmp by inverse jump logic
                           1098 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   021E 74 FF              1099 	mov	a,#0x100 - 0x01
   0220 25 2D              1100 	add	a,_set_MCR_PARM_3
   0222 40 04              1101 	jc	00102$
   0224                    1102 00113$:
                           1103 ;     genRet
   0224 75 82 00           1104 	mov	dpl,#0x00
                           1105 ;	Peephole 112.b	changed ljmp to sjmp
                           1106 ;	Peephole 251.b	replaced sjmp to ret with ret
   0227 22                 1107 	ret
   0228                    1108 00102$:
                           1109 ;src/ddc_regs.c:41: if (index == 0)
                           1110 ;     genCmpEq
                           1111 ;	Peephole 112.b	changed ljmp to sjmp
                           1112 ;	Peephole 199	optimized misc jump sequence
   0228 BA 00 1B           1113 	cjne	r2,#0x00,00106$
                           1114 ;00114$:
                           1115 ;	Peephole 200	removed redundant sjmp
   022B                    1116 00115$:
                           1117 ;src/ddc_regs.c:43: DDC0_AMR = (MCR & 0xFF00) >> 8;
                           1118 ;     genAssign
   022B 90 60 07           1119 	mov	dptr,#_DDC0_AMR
   022E 74 03              1120 	mov	a,#0x03
   0230 F0                 1121 	movx	@dptr,a
                           1122 ;src/ddc_regs.c:44: DDC0_LAR = (MCR & 0xFF);
                           1123 ;     genAssign
   0231 90 60 06           1124 	mov	dptr,#_DDC0_LAR
                           1125 ;	Peephole 181	changed mov to clr
   0234 E4                 1126 	clr	a
   0235 F0                 1127 	movx	@dptr,a
                           1128 ;src/ddc_regs.c:45: DDC0_DR0 = (buf[0] & 0xFF);
                           1129 ;     genAssign
   0236 85 2B 82           1130 	mov	dpl,_set_MCR_PARM_2
   0239 85 2C 83           1131 	mov	dph,(_set_MCR_PARM_2 + 1)
                           1132 ;     genPointerGet
                           1133 ;     genFarPointerGet
   023C E0                 1134 	movx	a,@dptr
                           1135 ;     genAssign
                           1136 ;	Peephole 100	removed redundant mov
   023D FB                 1137 	mov	r3,a
   023E 90 60 00           1138 	mov	dptr,#_DDC0_DR0
   0241 F0                 1139 	movx	@dptr,a
                           1140 ;src/ddc_regs.c:46: return 1;
                           1141 ;     genRet
   0242 75 82 01           1142 	mov	dpl,#0x01
                           1143 ;	Peephole 112.b	changed ljmp to sjmp
                           1144 ;	Peephole 251.b	replaced sjmp to ret with ret
   0245 22                 1145 	ret
   0246                    1146 00106$:
                           1147 ;src/ddc_regs.c:47: } else if (index == 1) {
                           1148 ;     genCmpEq
                           1149 ;	Peephole 112.b	changed ljmp to sjmp
                           1150 ;	Peephole 199	optimized misc jump sequence
   0246 BA 01 1B           1151 	cjne	r2,#0x01,00107$
                           1152 ;00116$:
                           1153 ;	Peephole 200	removed redundant sjmp
   0249                    1154 00117$:
                           1155 ;src/ddc_regs.c:48: DDC1_AMR = (MCR & 0xFF00) >> 8;
                           1156 ;     genAssign
   0249 90 60 17           1157 	mov	dptr,#_DDC1_AMR
   024C 74 03              1158 	mov	a,#0x03
   024E F0                 1159 	movx	@dptr,a
                           1160 ;src/ddc_regs.c:49: DDC1_LAR = (MCR & 0xFF);
                           1161 ;     genAssign
   024F 90 60 16           1162 	mov	dptr,#_DDC1_LAR
                           1163 ;	Peephole 181	changed mov to clr
   0252 E4                 1164 	clr	a
   0253 F0                 1165 	movx	@dptr,a
                           1166 ;src/ddc_regs.c:50: DDC0_DR0 = (buf[0] & 0xFF);
                           1167 ;     genAssign
   0254 85 2B 82           1168 	mov	dpl,_set_MCR_PARM_2
   0257 85 2C 83           1169 	mov	dph,(_set_MCR_PARM_2 + 1)
                           1170 ;     genPointerGet
                           1171 ;     genFarPointerGet
   025A E0                 1172 	movx	a,@dptr
                           1173 ;     genAssign
                           1174 ;	Peephole 100	removed redundant mov
   025B FA                 1175 	mov	r2,a
   025C 90 60 00           1176 	mov	dptr,#_DDC0_DR0
   025F F0                 1177 	movx	@dptr,a
                           1178 ;src/ddc_regs.c:51: return 1;
                           1179 ;     genRet
   0260 75 82 01           1180 	mov	dpl,#0x01
                           1181 ;	Peephole 112.b	changed ljmp to sjmp
                           1182 ;src/ddc_regs.c:53: return 0;
                           1183 ;     genRet
                           1184 ;	Peephole 237.a	removed sjmp to ret
   0263 22                 1185 	ret
   0264                    1186 00107$:
   0264 75 82 00           1187 	mov	dpl,#0x00
   0267                    1188 00108$:
   0267 22                 1189 	ret
                           1190 ;------------------------------------------------------------
                           1191 ;Allocation info for local variables in function 'read_NCR'
                           1192 ;------------------------------------------------------------
                           1193 ;buf                       Allocated with name '_read_NCR_PARM_2'
                           1194 ;len                       Allocated with name '_read_NCR_PARM_3'
                           1195 ;index                     Allocated to registers r2 
                           1196 ;------------------------------------------------------------
                           1197 ;src/ddc_regs.c:58: read_NCR (unsigned char index, xdata unsigned char *buf,
                           1198 ;	-----------------------------------------
                           1199 ;	 function read_NCR
                           1200 ;	-----------------------------------------
   0268                    1201 _read_NCR:
                           1202 ;     genReceive
   0268 AA 82              1203 	mov	r2,dpl
                           1204 ;src/ddc_regs.c:61: if (len < 1) return 0; // 8 bits
                           1205 ;     genCmpLt
                           1206 ;     genCmp
                           1207 ;     genIfxJump
                           1208 ;	Peephole 108	removed ljmp by inverse jump logic
                           1209 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   026A 74 FF              1210 	mov	a,#0x100 - 0x01
   026C 25 2D              1211 	add	a,_read_NCR_PARM_3
   026E 40 04              1212 	jc	00102$
   0270                    1213 00113$:
                           1214 ;     genRet
   0270 75 82 00           1215 	mov	dpl,#0x00
                           1216 ;	Peephole 112.b	changed ljmp to sjmp
                           1217 ;	Peephole 251.b	replaced sjmp to ret with ret
   0273 22                 1218 	ret
   0274                    1219 00102$:
                           1220 ;src/ddc_regs.c:63: if (index == 0)
                           1221 ;     genCmpEq
                           1222 ;	Peephole 112.b	changed ljmp to sjmp
                           1223 ;	Peephole 199	optimized misc jump sequence
   0274 BA 00 14           1224 	cjne	r2,#0x00,00106$
                           1225 ;00114$:
                           1226 ;	Peephole 200	removed redundant sjmp
   0277                    1227 00115$:
                           1228 ;src/ddc_regs.c:65: DDC0_AMR = (NCR & 0xFF00) >> 8;
                           1229 ;     genAssign
   0277 90 60 07           1230 	mov	dptr,#_DDC0_AMR
   027A 74 03              1231 	mov	a,#0x03
   027C F0                 1232 	movx	@dptr,a
                           1233 ;src/ddc_regs.c:66: DDC0_LAR = (NCR & 0xFF);
                           1234 ;     genAssign
   027D 90 60 06           1235 	mov	dptr,#_DDC0_LAR
   0280 74 01              1236 	mov	a,#0x01
   0282 F0                 1237 	movx	@dptr,a
                           1238 ;src/ddc_regs.c:67: buf[0] == (DDC0_DR0 & 0xFF);
                           1239 ;     genDummyRead
   0283 90 60 00           1240 	mov	dptr,#_DDC0_DR0
   0286 E0                 1241 	movx	a,@dptr
                           1242 ;src/ddc_regs.c:68: return 1;
                           1243 ;     genRet
   0287 75 82 01           1244 	mov	dpl,#0x01
                           1245 ;	Peephole 112.b	changed ljmp to sjmp
                           1246 ;	Peephole 251.b	replaced sjmp to ret with ret
   028A 22                 1247 	ret
   028B                    1248 00106$:
                           1249 ;src/ddc_regs.c:69: } else if (index == 1) {
                           1250 ;     genCmpEq
                           1251 ;	Peephole 112.b	changed ljmp to sjmp
                           1252 ;	Peephole 199	optimized misc jump sequence
   028B BA 01 14           1253 	cjne	r2,#0x01,00107$
                           1254 ;00116$:
                           1255 ;	Peephole 200	removed redundant sjmp
   028E                    1256 00117$:
                           1257 ;src/ddc_regs.c:70: DDC1_AMR = (NCR & 0xFF00) >> 8;
                           1258 ;     genAssign
   028E 90 60 17           1259 	mov	dptr,#_DDC1_AMR
   0291 74 03              1260 	mov	a,#0x03
   0293 F0                 1261 	movx	@dptr,a
                           1262 ;src/ddc_regs.c:71: DDC1_LAR = (NCR & 0xFF);
                           1263 ;     genAssign
   0294 90 60 16           1264 	mov	dptr,#_DDC1_LAR
   0297 74 01              1265 	mov	a,#0x01
   0299 F0                 1266 	movx	@dptr,a
                           1267 ;src/ddc_regs.c:72: buf[0] == (DDC1_DR0 & 0xFF);
                           1268 ;     genDummyRead
   029A 90 60 10           1269 	mov	dptr,#_DDC1_DR0
   029D E0                 1270 	movx	a,@dptr
                           1271 ;src/ddc_regs.c:73: return 1;
                           1272 ;     genRet
   029E 75 82 01           1273 	mov	dpl,#0x01
                           1274 ;	Peephole 112.b	changed ljmp to sjmp
                           1275 ;src/ddc_regs.c:75: return 0;
                           1276 ;     genRet
                           1277 ;	Peephole 237.a	removed sjmp to ret
   02A1 22                 1278 	ret
   02A2                    1279 00107$:
   02A2 75 82 00           1280 	mov	dpl,#0x00
   02A5                    1281 00108$:
   02A5 22                 1282 	ret
                           1283 ;------------------------------------------------------------
                           1284 ;Allocation info for local variables in function 'set_NCR'
                           1285 ;------------------------------------------------------------
                           1286 ;buf                       Allocated with name '_set_NCR_PARM_2'
                           1287 ;len                       Allocated with name '_set_NCR_PARM_3'
                           1288 ;index                     Allocated to registers r2 
                           1289 ;------------------------------------------------------------
                           1290 ;src/ddc_regs.c:80: set_NCR (unsigned char index, xdata unsigned char *buf,
                           1291 ;	-----------------------------------------
                           1292 ;	 function set_NCR
                           1293 ;	-----------------------------------------
   02A6                    1294 _set_NCR:
                           1295 ;     genReceive
   02A6 AA 82              1296 	mov	r2,dpl
                           1297 ;src/ddc_regs.c:83: if (len < 1) return 0; // 8 bits
                           1298 ;     genCmpLt
                           1299 ;     genCmp
                           1300 ;     genIfxJump
                           1301 ;	Peephole 108	removed ljmp by inverse jump logic
                           1302 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   02A8 74 FF              1303 	mov	a,#0x100 - 0x01
   02AA 25 2D              1304 	add	a,_set_NCR_PARM_3
   02AC 40 04              1305 	jc	00102$
   02AE                    1306 00113$:
                           1307 ;     genRet
   02AE 75 82 00           1308 	mov	dpl,#0x00
                           1309 ;	Peephole 112.b	changed ljmp to sjmp
                           1310 ;	Peephole 251.b	replaced sjmp to ret with ret
   02B1 22                 1311 	ret
   02B2                    1312 00102$:
                           1313 ;src/ddc_regs.c:85: if (index == 0)
                           1314 ;     genCmpEq
                           1315 ;	Peephole 112.b	changed ljmp to sjmp
                           1316 ;	Peephole 199	optimized misc jump sequence
   02B2 BA 00 1C           1317 	cjne	r2,#0x00,00106$
                           1318 ;00114$:
                           1319 ;	Peephole 200	removed redundant sjmp
   02B5                    1320 00115$:
                           1321 ;src/ddc_regs.c:87: DDC0_AMR = (NCR & 0xFF00) >> 8;
                           1322 ;     genAssign
   02B5 90 60 07           1323 	mov	dptr,#_DDC0_AMR
   02B8 74 03              1324 	mov	a,#0x03
   02BA F0                 1325 	movx	@dptr,a
                           1326 ;src/ddc_regs.c:88: DDC0_LAR = (NCR & 0xFF);
                           1327 ;     genAssign
   02BB 90 60 06           1328 	mov	dptr,#_DDC0_LAR
   02BE 74 01              1329 	mov	a,#0x01
   02C0 F0                 1330 	movx	@dptr,a
                           1331 ;src/ddc_regs.c:89: DDC0_DR0 = (buf[0] & 0xFF);
                           1332 ;     genAssign
   02C1 85 2B 82           1333 	mov	dpl,_set_NCR_PARM_2
   02C4 85 2C 83           1334 	mov	dph,(_set_NCR_PARM_2 + 1)
                           1335 ;     genPointerGet
                           1336 ;     genFarPointerGet
   02C7 E0                 1337 	movx	a,@dptr
                           1338 ;     genAssign
                           1339 ;	Peephole 100	removed redundant mov
   02C8 FB                 1340 	mov	r3,a
   02C9 90 60 00           1341 	mov	dptr,#_DDC0_DR0
   02CC F0                 1342 	movx	@dptr,a
                           1343 ;src/ddc_regs.c:90: return 1;
                           1344 ;     genRet
   02CD 75 82 01           1345 	mov	dpl,#0x01
                           1346 ;	Peephole 112.b	changed ljmp to sjmp
                           1347 ;	Peephole 251.b	replaced sjmp to ret with ret
   02D0 22                 1348 	ret
   02D1                    1349 00106$:
                           1350 ;src/ddc_regs.c:91: } else if (index == 1) {
                           1351 ;     genCmpEq
                           1352 ;	Peephole 112.b	changed ljmp to sjmp
                           1353 ;	Peephole 199	optimized misc jump sequence
   02D1 BA 01 1C           1354 	cjne	r2,#0x01,00107$
                           1355 ;00116$:
                           1356 ;	Peephole 200	removed redundant sjmp
   02D4                    1357 00117$:
                           1358 ;src/ddc_regs.c:92: DDC1_AMR = (NCR & 0xFF00) >> 8;
                           1359 ;     genAssign
   02D4 90 60 17           1360 	mov	dptr,#_DDC1_AMR
   02D7 74 03              1361 	mov	a,#0x03
   02D9 F0                 1362 	movx	@dptr,a
                           1363 ;src/ddc_regs.c:93: DDC1_LAR = (NCR & 0xFF);
                           1364 ;     genAssign
   02DA 90 60 16           1365 	mov	dptr,#_DDC1_LAR
   02DD 74 01              1366 	mov	a,#0x01
   02DF F0                 1367 	movx	@dptr,a
                           1368 ;src/ddc_regs.c:94: DDC0_DR0 = (buf[0] & 0xFF);
                           1369 ;     genAssign
   02E0 85 2B 82           1370 	mov	dpl,_set_NCR_PARM_2
   02E3 85 2C 83           1371 	mov	dph,(_set_NCR_PARM_2 + 1)
                           1372 ;     genPointerGet
                           1373 ;     genFarPointerGet
   02E6 E0                 1374 	movx	a,@dptr
                           1375 ;     genAssign
                           1376 ;	Peephole 100	removed redundant mov
   02E7 FA                 1377 	mov	r2,a
   02E8 90 60 00           1378 	mov	dptr,#_DDC0_DR0
   02EB F0                 1379 	movx	@dptr,a
                           1380 ;src/ddc_regs.c:95: return 1;
                           1381 ;     genRet
   02EC 75 82 01           1382 	mov	dpl,#0x01
                           1383 ;	Peephole 112.b	changed ljmp to sjmp
                           1384 ;src/ddc_regs.c:97: return 0;
                           1385 ;     genRet
                           1386 ;	Peephole 237.a	removed sjmp to ret
   02EF 22                 1387 	ret
   02F0                    1388 00107$:
   02F0 75 82 00           1389 	mov	dpl,#0x00
   02F3                    1390 00108$:
   02F3 22                 1391 	ret
                           1392 ;------------------------------------------------------------
                           1393 ;Allocation info for local variables in function 'read_NSCR'
                           1394 ;------------------------------------------------------------
                           1395 ;buf                       Allocated with name '_read_NSCR_PARM_2'
                           1396 ;len                       Allocated with name '_read_NSCR_PARM_3'
                           1397 ;index                     Allocated to registers r2 
                           1398 ;------------------------------------------------------------
                           1399 ;src/ddc_regs.c:102: read_NSCR (unsigned char index, xdata unsigned char *buf,
                           1400 ;	-----------------------------------------
                           1401 ;	 function read_NSCR
                           1402 ;	-----------------------------------------
   02F4                    1403 _read_NSCR:
                           1404 ;     genReceive
   02F4 AA 82              1405 	mov	r2,dpl
                           1406 ;src/ddc_regs.c:105: if (len < 4) return 0; // 32 bits
                           1407 ;     genCmpLt
                           1408 ;     genCmp
                           1409 ;     genIfxJump
                           1410 ;	Peephole 108	removed ljmp by inverse jump logic
                           1411 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   02F6 74 FC              1412 	mov	a,#0x100 - 0x04
   02F8 25 2D              1413 	add	a,_read_NSCR_PARM_3
   02FA 40 04              1414 	jc	00102$
   02FC                    1415 00113$:
                           1416 ;     genRet
   02FC 75 82 00           1417 	mov	dpl,#0x00
                           1418 ;	Peephole 112.b	changed ljmp to sjmp
                           1419 ;	Peephole 251.b	replaced sjmp to ret with ret
   02FF 22                 1420 	ret
   0300                    1421 00102$:
                           1422 ;src/ddc_regs.c:107: if (index == 0)
                           1423 ;     genCmpEq
                           1424 ;	Peephole 112.b	changed ljmp to sjmp
                           1425 ;	Peephole 199	optimized misc jump sequence
   0300 BA 00 20           1426 	cjne	r2,#0x00,00106$
                           1427 ;00114$:
                           1428 ;	Peephole 200	removed redundant sjmp
   0303                    1429 00115$:
                           1430 ;src/ddc_regs.c:109: DDC0_AMR = (NSCR & 0xFF00) >> 8;
                           1431 ;     genAssign
   0303 90 60 07           1432 	mov	dptr,#_DDC0_AMR
   0306 74 03              1433 	mov	a,#0x03
   0308 F0                 1434 	movx	@dptr,a
                           1435 ;src/ddc_regs.c:110: DDC0_LAR = (NSCR & 0xFF);
                           1436 ;     genAssign
   0309 90 60 06           1437 	mov	dptr,#_DDC0_LAR
   030C 74 02              1438 	mov	a,#0x02
   030E F0                 1439 	movx	@dptr,a
                           1440 ;src/ddc_regs.c:111: buf[0] == (DDC0_DR0 & 0xFF);
                           1441 ;     genDummyRead
   030F 90 60 00           1442 	mov	dptr,#_DDC0_DR0
   0312 E0                 1443 	movx	a,@dptr
                           1444 ;src/ddc_regs.c:112: buf[1] == (DDC0_DR1 & 0xFF);
                           1445 ;     genDummyRead
   0313 90 60 01           1446 	mov	dptr,#_DDC0_DR1
   0316 E0                 1447 	movx	a,@dptr
                           1448 ;src/ddc_regs.c:113: buf[2] == (DDC0_DR2 & 0xFF);
                           1449 ;     genDummyRead
   0317 90 60 02           1450 	mov	dptr,#_DDC0_DR2
   031A E0                 1451 	movx	a,@dptr
                           1452 ;src/ddc_regs.c:114: buf[3] == (DDC0_DR3 & 0xFF);
                           1453 ;     genDummyRead
   031B 90 60 03           1454 	mov	dptr,#_DDC0_DR3
   031E E0                 1455 	movx	a,@dptr
                           1456 ;src/ddc_regs.c:115: return 4;
                           1457 ;     genRet
   031F 75 82 04           1458 	mov	dpl,#0x04
                           1459 ;	Peephole 112.b	changed ljmp to sjmp
                           1460 ;	Peephole 251.b	replaced sjmp to ret with ret
   0322 22                 1461 	ret
   0323                    1462 00106$:
                           1463 ;src/ddc_regs.c:116: } else if (index == 1) {
                           1464 ;     genCmpEq
                           1465 ;	Peephole 112.b	changed ljmp to sjmp
                           1466 ;	Peephole 199	optimized misc jump sequence
   0323 BA 01 20           1467 	cjne	r2,#0x01,00107$
                           1468 ;00116$:
                           1469 ;	Peephole 200	removed redundant sjmp
   0326                    1470 00117$:
                           1471 ;src/ddc_regs.c:117: DDC1_AMR = (NSCR & 0xFF00) >> 8;
                           1472 ;     genAssign
   0326 90 60 17           1473 	mov	dptr,#_DDC1_AMR
   0329 74 03              1474 	mov	a,#0x03
   032B F0                 1475 	movx	@dptr,a
                           1476 ;src/ddc_regs.c:118: DDC1_LAR = (NSCR & 0xFF);
                           1477 ;     genAssign
   032C 90 60 16           1478 	mov	dptr,#_DDC1_LAR
   032F 74 02              1479 	mov	a,#0x02
   0331 F0                 1480 	movx	@dptr,a
                           1481 ;src/ddc_regs.c:119: buf[0] == (DDC1_DR0 & 0xFF);
                           1482 ;     genDummyRead
   0332 90 60 10           1483 	mov	dptr,#_DDC1_DR0
   0335 E0                 1484 	movx	a,@dptr
                           1485 ;src/ddc_regs.c:120: buf[1] == (DDC1_DR1 & 0xFF);
                           1486 ;     genDummyRead
   0336 90 60 11           1487 	mov	dptr,#_DDC1_DR1
   0339 E0                 1488 	movx	a,@dptr
                           1489 ;src/ddc_regs.c:121: buf[2] == (DDC1_DR2 & 0xFF);
                           1490 ;     genDummyRead
   033A 90 60 12           1491 	mov	dptr,#_DDC1_DR2
   033D E0                 1492 	movx	a,@dptr
                           1493 ;src/ddc_regs.c:122: buf[3] == (DDC1_DR3 & 0xFF);
                           1494 ;     genDummyRead
   033E 90 60 13           1495 	mov	dptr,#_DDC1_DR3
   0341 E0                 1496 	movx	a,@dptr
                           1497 ;src/ddc_regs.c:123: return 4;
                           1498 ;     genRet
   0342 75 82 04           1499 	mov	dpl,#0x04
                           1500 ;	Peephole 112.b	changed ljmp to sjmp
                           1501 ;src/ddc_regs.c:125: return 0;
                           1502 ;     genRet
                           1503 ;	Peephole 237.a	removed sjmp to ret
   0345 22                 1504 	ret
   0346                    1505 00107$:
   0346 75 82 00           1506 	mov	dpl,#0x00
   0349                    1507 00108$:
   0349 22                 1508 	ret
                           1509 ;------------------------------------------------------------
                           1510 ;Allocation info for local variables in function 'set_NSCR'
                           1511 ;------------------------------------------------------------
                           1512 ;buf                       Allocated with name '_set_NSCR_PARM_2'
                           1513 ;len                       Allocated with name '_set_NSCR_PARM_3'
                           1514 ;index                     Allocated to registers r2 
                           1515 ;------------------------------------------------------------
                           1516 ;src/ddc_regs.c:130: set_NSCR (unsigned char index, xdata unsigned char *buf,
                           1517 ;	-----------------------------------------
                           1518 ;	 function set_NSCR
                           1519 ;	-----------------------------------------
   034A                    1520 _set_NSCR:
                           1521 ;     genReceive
   034A AA 82              1522 	mov	r2,dpl
                           1523 ;src/ddc_regs.c:133: if (len < 4) return 0; // 32 bits
                           1524 ;     genCmpLt
                           1525 ;     genCmp
                           1526 ;     genIfxJump
                           1527 ;	Peephole 108	removed ljmp by inverse jump logic
                           1528 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   034C 74 FC              1529 	mov	a,#0x100 - 0x04
   034E 25 2D              1530 	add	a,_set_NSCR_PARM_3
   0350 40 04              1531 	jc	00102$
   0352                    1532 00113$:
                           1533 ;     genRet
   0352 75 82 00           1534 	mov	dpl,#0x00
                           1535 ;	Peephole 251.a	replaced ljmp to ret with ret
   0355 22                 1536 	ret
   0356                    1537 00102$:
                           1538 ;src/ddc_regs.c:135: if (index == 0)
                           1539 ;     genCmpEq
                           1540 ;	Peephole 112.b	changed ljmp to sjmp
                           1541 ;	Peephole 199	optimized misc jump sequence
   0356 BA 00 4F           1542 	cjne	r2,#0x00,00106$
                           1543 ;00114$:
                           1544 ;	Peephole 200	removed redundant sjmp
   0359                    1545 00115$:
                           1546 ;src/ddc_regs.c:137: DDC0_AMR = (NSCR & 0xFF00) >> 8;
                           1547 ;     genAssign
   0359 90 60 07           1548 	mov	dptr,#_DDC0_AMR
   035C 74 03              1549 	mov	a,#0x03
   035E F0                 1550 	movx	@dptr,a
                           1551 ;src/ddc_regs.c:138: DDC0_LAR = (NSCR & 0xFF);
                           1552 ;     genAssign
   035F 90 60 06           1553 	mov	dptr,#_DDC0_LAR
   0362 74 02              1554 	mov	a,#0x02
   0364 F0                 1555 	movx	@dptr,a
                           1556 ;src/ddc_regs.c:139: DDC0_DR3 = (buf[3] & 0xFF);
                           1557 ;     genPlus
                           1558 ;     genPlusIncr
   0365 74 03              1559 	mov	a,#0x03
   0367 25 2B              1560 	add	a,_set_NSCR_PARM_2
   0369 F5 82              1561 	mov	dpl,a
                           1562 ;	Peephole 181	changed mov to clr
   036B E4                 1563 	clr	a
   036C 35 2C              1564 	addc	a,(_set_NSCR_PARM_2 + 1)
   036E F5 83              1565 	mov	dph,a
                           1566 ;     genPointerGet
                           1567 ;     genFarPointerGet
   0370 E0                 1568 	movx	a,@dptr
                           1569 ;     genAssign
                           1570 ;	Peephole 100	removed redundant mov
   0371 FB                 1571 	mov	r3,a
   0372 90 60 03           1572 	mov	dptr,#_DDC0_DR3
   0375 F0                 1573 	movx	@dptr,a
                           1574 ;src/ddc_regs.c:140: DDC0_DR2 = (buf[2] & 0xFF);
                           1575 ;     genPlus
                           1576 ;     genPlusIncr
   0376 74 02              1577 	mov	a,#0x02
   0378 25 2B              1578 	add	a,_set_NSCR_PARM_2
   037A F5 82              1579 	mov	dpl,a
                           1580 ;	Peephole 181	changed mov to clr
   037C E4                 1581 	clr	a
   037D 35 2C              1582 	addc	a,(_set_NSCR_PARM_2 + 1)
   037F F5 83              1583 	mov	dph,a
                           1584 ;     genPointerGet
                           1585 ;     genFarPointerGet
   0381 E0                 1586 	movx	a,@dptr
                           1587 ;     genAssign
                           1588 ;	Peephole 100	removed redundant mov
   0382 FB                 1589 	mov	r3,a
   0383 90 60 02           1590 	mov	dptr,#_DDC0_DR2
   0386 F0                 1591 	movx	@dptr,a
                           1592 ;src/ddc_regs.c:141: DDC0_DR1 = (buf[1] & 0xFF);
                           1593 ;     genPlus
                           1594 ;     genPlusIncr
   0387 74 01              1595 	mov	a,#0x01
   0389 25 2B              1596 	add	a,_set_NSCR_PARM_2
   038B F5 82              1597 	mov	dpl,a
                           1598 ;	Peephole 181	changed mov to clr
   038D E4                 1599 	clr	a
   038E 35 2C              1600 	addc	a,(_set_NSCR_PARM_2 + 1)
   0390 F5 83              1601 	mov	dph,a
                           1602 ;     genPointerGet
                           1603 ;     genFarPointerGet
   0392 E0                 1604 	movx	a,@dptr
                           1605 ;     genAssign
                           1606 ;	Peephole 100	removed redundant mov
   0393 FB                 1607 	mov	r3,a
   0394 90 60 01           1608 	mov	dptr,#_DDC0_DR1
   0397 F0                 1609 	movx	@dptr,a
                           1610 ;src/ddc_regs.c:142: DDC0_DR0 = (buf[0] & 0xFF);
                           1611 ;     genAssign
   0398 85 2B 82           1612 	mov	dpl,_set_NSCR_PARM_2
   039B 85 2C 83           1613 	mov	dph,(_set_NSCR_PARM_2 + 1)
                           1614 ;     genPointerGet
                           1615 ;     genFarPointerGet
   039E E0                 1616 	movx	a,@dptr
                           1617 ;     genAssign
                           1618 ;	Peephole 100	removed redundant mov
   039F FB                 1619 	mov	r3,a
   03A0 90 60 00           1620 	mov	dptr,#_DDC0_DR0
   03A3 F0                 1621 	movx	@dptr,a
                           1622 ;src/ddc_regs.c:143: return 4;
                           1623 ;     genRet
   03A4 75 82 04           1624 	mov	dpl,#0x04
                           1625 ;	Peephole 112.b	changed ljmp to sjmp
                           1626 ;	Peephole 251.b	replaced sjmp to ret with ret
   03A7 22                 1627 	ret
   03A8                    1628 00106$:
                           1629 ;src/ddc_regs.c:144: } else if (index == 1) {
                           1630 ;     genCmpEq
                           1631 ;	Peephole 112.b	changed ljmp to sjmp
                           1632 ;	Peephole 199	optimized misc jump sequence
   03A8 BA 01 4F           1633 	cjne	r2,#0x01,00107$
                           1634 ;00116$:
                           1635 ;	Peephole 200	removed redundant sjmp
   03AB                    1636 00117$:
                           1637 ;src/ddc_regs.c:145: DDC1_AMR = (NSCR & 0xFF00) >> 8;
                           1638 ;     genAssign
   03AB 90 60 17           1639 	mov	dptr,#_DDC1_AMR
   03AE 74 03              1640 	mov	a,#0x03
   03B0 F0                 1641 	movx	@dptr,a
                           1642 ;src/ddc_regs.c:146: DDC1_LAR = (NSCR & 0xFF);
                           1643 ;     genAssign
   03B1 90 60 16           1644 	mov	dptr,#_DDC1_LAR
   03B4 74 02              1645 	mov	a,#0x02
   03B6 F0                 1646 	movx	@dptr,a
                           1647 ;src/ddc_regs.c:147: DDC1_DR3 = (buf[3] & 0xFF);
                           1648 ;     genPlus
                           1649 ;     genPlusIncr
   03B7 74 03              1650 	mov	a,#0x03
   03B9 25 2B              1651 	add	a,_set_NSCR_PARM_2
   03BB F5 82              1652 	mov	dpl,a
                           1653 ;	Peephole 181	changed mov to clr
   03BD E4                 1654 	clr	a
   03BE 35 2C              1655 	addc	a,(_set_NSCR_PARM_2 + 1)
   03C0 F5 83              1656 	mov	dph,a
                           1657 ;     genPointerGet
                           1658 ;     genFarPointerGet
   03C2 E0                 1659 	movx	a,@dptr
                           1660 ;     genAssign
                           1661 ;	Peephole 100	removed redundant mov
   03C3 FA                 1662 	mov	r2,a
   03C4 90 60 13           1663 	mov	dptr,#_DDC1_DR3
   03C7 F0                 1664 	movx	@dptr,a
                           1665 ;src/ddc_regs.c:148: DDC1_DR2 = (buf[2] & 0xFF);
                           1666 ;     genPlus
                           1667 ;     genPlusIncr
   03C8 74 02              1668 	mov	a,#0x02
   03CA 25 2B              1669 	add	a,_set_NSCR_PARM_2
   03CC F5 82              1670 	mov	dpl,a
                           1671 ;	Peephole 181	changed mov to clr
   03CE E4                 1672 	clr	a
   03CF 35 2C              1673 	addc	a,(_set_NSCR_PARM_2 + 1)
   03D1 F5 83              1674 	mov	dph,a
                           1675 ;     genPointerGet
                           1676 ;     genFarPointerGet
   03D3 E0                 1677 	movx	a,@dptr
                           1678 ;     genAssign
                           1679 ;	Peephole 100	removed redundant mov
   03D4 FA                 1680 	mov	r2,a
   03D5 90 60 12           1681 	mov	dptr,#_DDC1_DR2
   03D8 F0                 1682 	movx	@dptr,a
                           1683 ;src/ddc_regs.c:149: DDC1_DR1 = (buf[1] & 0xFF);
                           1684 ;     genPlus
                           1685 ;     genPlusIncr
   03D9 74 01              1686 	mov	a,#0x01
   03DB 25 2B              1687 	add	a,_set_NSCR_PARM_2
   03DD F5 82              1688 	mov	dpl,a
                           1689 ;	Peephole 181	changed mov to clr
   03DF E4                 1690 	clr	a
   03E0 35 2C              1691 	addc	a,(_set_NSCR_PARM_2 + 1)
   03E2 F5 83              1692 	mov	dph,a
                           1693 ;     genPointerGet
                           1694 ;     genFarPointerGet
   03E4 E0                 1695 	movx	a,@dptr
                           1696 ;     genAssign
                           1697 ;	Peephole 100	removed redundant mov
   03E5 FA                 1698 	mov	r2,a
   03E6 90 60 11           1699 	mov	dptr,#_DDC1_DR1
   03E9 F0                 1700 	movx	@dptr,a
                           1701 ;src/ddc_regs.c:150: DDC1_DR0 = (buf[0] & 0xFF);
                           1702 ;     genAssign
   03EA 85 2B 82           1703 	mov	dpl,_set_NSCR_PARM_2
   03ED 85 2C 83           1704 	mov	dph,(_set_NSCR_PARM_2 + 1)
                           1705 ;     genPointerGet
                           1706 ;     genFarPointerGet
   03F0 E0                 1707 	movx	a,@dptr
                           1708 ;     genAssign
                           1709 ;	Peephole 100	removed redundant mov
   03F1 FA                 1710 	mov	r2,a
   03F2 90 60 10           1711 	mov	dptr,#_DDC1_DR0
   03F5 F0                 1712 	movx	@dptr,a
                           1713 ;src/ddc_regs.c:151: return 4;
                           1714 ;     genRet
   03F6 75 82 04           1715 	mov	dpl,#0x04
                           1716 ;	Peephole 112.b	changed ljmp to sjmp
                           1717 ;src/ddc_regs.c:153: return 0;
                           1718 ;     genRet
                           1719 ;	Peephole 237.a	removed sjmp to ret
   03F9 22                 1720 	ret
   03FA                    1721 00107$:
   03FA 75 82 00           1722 	mov	dpl,#0x00
   03FD                    1723 00108$:
   03FD 22                 1724 	ret
                           1725 ;------------------------------------------------------------
                           1726 ;Allocation info for local variables in function 'read_NFRE'
                           1727 ;------------------------------------------------------------
                           1728 ;buf                       Allocated with name '_read_NFRE_PARM_2'
                           1729 ;len                       Allocated with name '_read_NFRE_PARM_3'
                           1730 ;index                     Allocated to registers r2 
                           1731 ;------------------------------------------------------------
                           1732 ;src/ddc_regs.c:159: read_NFRE (unsigned char index, xdata unsigned char *buf,
                           1733 ;	-----------------------------------------
                           1734 ;	 function read_NFRE
                           1735 ;	-----------------------------------------
   03FE                    1736 _read_NFRE:
                           1737 ;     genReceive
   03FE AA 82              1738 	mov	r2,dpl
                           1739 ;src/ddc_regs.c:162: if (len < 4) return 0; // 32 bits
                           1740 ;     genCmpLt
                           1741 ;     genCmp
                           1742 ;     genIfxJump
                           1743 ;	Peephole 108	removed ljmp by inverse jump logic
                           1744 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0400 74 FC              1745 	mov	a,#0x100 - 0x04
   0402 25 2D              1746 	add	a,_read_NFRE_PARM_3
   0404 40 04              1747 	jc	00102$
   0406                    1748 00113$:
                           1749 ;     genRet
   0406 75 82 00           1750 	mov	dpl,#0x00
                           1751 ;	Peephole 112.b	changed ljmp to sjmp
                           1752 ;	Peephole 251.b	replaced sjmp to ret with ret
   0409 22                 1753 	ret
   040A                    1754 00102$:
                           1755 ;src/ddc_regs.c:164: if (index == 0)
                           1756 ;     genCmpEq
                           1757 ;	Peephole 112.b	changed ljmp to sjmp
                           1758 ;	Peephole 199	optimized misc jump sequence
   040A BA 00 20           1759 	cjne	r2,#0x00,00106$
                           1760 ;00114$:
                           1761 ;	Peephole 200	removed redundant sjmp
   040D                    1762 00115$:
                           1763 ;src/ddc_regs.c:166: DDC0_AMR = (NFRE & 0xFF00) >> 8;
                           1764 ;     genAssign
   040D 90 60 07           1765 	mov	dptr,#_DDC0_AMR
   0410 74 03              1766 	mov	a,#0x03
   0412 F0                 1767 	movx	@dptr,a
                           1768 ;src/ddc_regs.c:167: DDC0_LAR = (NFRE & 0xFF);
                           1769 ;     genAssign
   0413 90 60 06           1770 	mov	dptr,#_DDC0_LAR
   0416 74 03              1771 	mov	a,#0x03
   0418 F0                 1772 	movx	@dptr,a
                           1773 ;src/ddc_regs.c:168: buf[0] == (DDC0_DR0 & 0xFF);
                           1774 ;     genDummyRead
   0419 90 60 00           1775 	mov	dptr,#_DDC0_DR0
   041C E0                 1776 	movx	a,@dptr
                           1777 ;src/ddc_regs.c:169: buf[1] == (DDC0_DR1 & 0xFF);
                           1778 ;     genDummyRead
   041D 90 60 01           1779 	mov	dptr,#_DDC0_DR1
   0420 E0                 1780 	movx	a,@dptr
                           1781 ;src/ddc_regs.c:170: buf[2] == (DDC0_DR2 & 0xFF);
                           1782 ;     genDummyRead
   0421 90 60 02           1783 	mov	dptr,#_DDC0_DR2
   0424 E0                 1784 	movx	a,@dptr
                           1785 ;src/ddc_regs.c:171: buf[3] == (DDC0_DR3 & 0xFF);
                           1786 ;     genDummyRead
   0425 90 60 03           1787 	mov	dptr,#_DDC0_DR3
   0428 E0                 1788 	movx	a,@dptr
                           1789 ;src/ddc_regs.c:172: return 4;
                           1790 ;     genRet
   0429 75 82 04           1791 	mov	dpl,#0x04
                           1792 ;	Peephole 112.b	changed ljmp to sjmp
                           1793 ;	Peephole 251.b	replaced sjmp to ret with ret
   042C 22                 1794 	ret
   042D                    1795 00106$:
                           1796 ;src/ddc_regs.c:173: } else if (index == 1) {
                           1797 ;     genCmpEq
                           1798 ;	Peephole 112.b	changed ljmp to sjmp
                           1799 ;	Peephole 199	optimized misc jump sequence
   042D BA 01 20           1800 	cjne	r2,#0x01,00107$
                           1801 ;00116$:
                           1802 ;	Peephole 200	removed redundant sjmp
   0430                    1803 00117$:
                           1804 ;src/ddc_regs.c:174: DDC1_AMR = (NFRE & 0xFF00) >> 8;
                           1805 ;     genAssign
   0430 90 60 17           1806 	mov	dptr,#_DDC1_AMR
   0433 74 03              1807 	mov	a,#0x03
   0435 F0                 1808 	movx	@dptr,a
                           1809 ;src/ddc_regs.c:175: DDC1_LAR = (NFRE & 0xFF);
                           1810 ;     genAssign
   0436 90 60 16           1811 	mov	dptr,#_DDC1_LAR
   0439 74 03              1812 	mov	a,#0x03
   043B F0                 1813 	movx	@dptr,a
                           1814 ;src/ddc_regs.c:176: buf[0] == (DDC1_DR0 & 0xFF);
                           1815 ;     genDummyRead
   043C 90 60 10           1816 	mov	dptr,#_DDC1_DR0
   043F E0                 1817 	movx	a,@dptr
                           1818 ;src/ddc_regs.c:177: buf[1] == (DDC1_DR1 & 0xFF);
                           1819 ;     genDummyRead
   0440 90 60 11           1820 	mov	dptr,#_DDC1_DR1
   0443 E0                 1821 	movx	a,@dptr
                           1822 ;src/ddc_regs.c:178: buf[2] == (DDC1_DR2 & 0xFF);
                           1823 ;     genDummyRead
   0444 90 60 12           1824 	mov	dptr,#_DDC1_DR2
   0447 E0                 1825 	movx	a,@dptr
                           1826 ;src/ddc_regs.c:179: buf[3] == (DDC1_DR3 & 0xFF);
                           1827 ;     genDummyRead
   0448 90 60 13           1828 	mov	dptr,#_DDC1_DR3
   044B E0                 1829 	movx	a,@dptr
                           1830 ;src/ddc_regs.c:180: return 4;
                           1831 ;     genRet
   044C 75 82 04           1832 	mov	dpl,#0x04
                           1833 ;	Peephole 112.b	changed ljmp to sjmp
                           1834 ;src/ddc_regs.c:182: return 0;
                           1835 ;     genRet
                           1836 ;	Peephole 237.a	removed sjmp to ret
   044F 22                 1837 	ret
   0450                    1838 00107$:
   0450 75 82 00           1839 	mov	dpl,#0x00
   0453                    1840 00108$:
   0453 22                 1841 	ret
                           1842 ;------------------------------------------------------------
                           1843 ;Allocation info for local variables in function 'set_NFRE'
                           1844 ;------------------------------------------------------------
                           1845 ;buf                       Allocated with name '_set_NFRE_PARM_2'
                           1846 ;len                       Allocated with name '_set_NFRE_PARM_3'
                           1847 ;index                     Allocated to registers r2 
                           1848 ;------------------------------------------------------------
                           1849 ;src/ddc_regs.c:187: set_NFRE (unsigned char index, xdata unsigned char *buf,
                           1850 ;	-----------------------------------------
                           1851 ;	 function set_NFRE
                           1852 ;	-----------------------------------------
   0454                    1853 _set_NFRE:
                           1854 ;     genReceive
   0454 AA 82              1855 	mov	r2,dpl
                           1856 ;src/ddc_regs.c:190: if (len < 4) return 0; // 32 bits
                           1857 ;     genCmpLt
                           1858 ;     genCmp
                           1859 ;     genIfxJump
                           1860 ;	Peephole 108	removed ljmp by inverse jump logic
                           1861 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0456 74 FC              1862 	mov	a,#0x100 - 0x04
   0458 25 2D              1863 	add	a,_set_NFRE_PARM_3
   045A 40 04              1864 	jc	00102$
   045C                    1865 00113$:
                           1866 ;     genRet
   045C 75 82 00           1867 	mov	dpl,#0x00
                           1868 ;	Peephole 251.a	replaced ljmp to ret with ret
   045F 22                 1869 	ret
   0460                    1870 00102$:
                           1871 ;src/ddc_regs.c:192: if (index == 0)
                           1872 ;     genCmpEq
                           1873 ;	Peephole 112.b	changed ljmp to sjmp
                           1874 ;	Peephole 199	optimized misc jump sequence
   0460 BA 00 4F           1875 	cjne	r2,#0x00,00106$
                           1876 ;00114$:
                           1877 ;	Peephole 200	removed redundant sjmp
   0463                    1878 00115$:
                           1879 ;src/ddc_regs.c:194: DDC0_AMR = (NFRE & 0xFF00) >> 8;
                           1880 ;     genAssign
   0463 90 60 07           1881 	mov	dptr,#_DDC0_AMR
   0466 74 03              1882 	mov	a,#0x03
   0468 F0                 1883 	movx	@dptr,a
                           1884 ;src/ddc_regs.c:195: DDC0_LAR = (NFRE & 0xFF);
                           1885 ;     genAssign
   0469 90 60 06           1886 	mov	dptr,#_DDC0_LAR
   046C 74 03              1887 	mov	a,#0x03
   046E F0                 1888 	movx	@dptr,a
                           1889 ;src/ddc_regs.c:196: DDC0_DR3 = (buf[3] & 0xFF);
                           1890 ;     genPlus
                           1891 ;     genPlusIncr
   046F 74 03              1892 	mov	a,#0x03
   0471 25 2B              1893 	add	a,_set_NFRE_PARM_2
   0473 F5 82              1894 	mov	dpl,a
                           1895 ;	Peephole 181	changed mov to clr
   0475 E4                 1896 	clr	a
   0476 35 2C              1897 	addc	a,(_set_NFRE_PARM_2 + 1)
   0478 F5 83              1898 	mov	dph,a
                           1899 ;     genPointerGet
                           1900 ;     genFarPointerGet
   047A E0                 1901 	movx	a,@dptr
                           1902 ;     genAssign
                           1903 ;	Peephole 100	removed redundant mov
   047B FB                 1904 	mov	r3,a
   047C 90 60 03           1905 	mov	dptr,#_DDC0_DR3
   047F F0                 1906 	movx	@dptr,a
                           1907 ;src/ddc_regs.c:197: DDC0_DR2 = (buf[2] & 0xFF);
                           1908 ;     genPlus
                           1909 ;     genPlusIncr
   0480 74 02              1910 	mov	a,#0x02
   0482 25 2B              1911 	add	a,_set_NFRE_PARM_2
   0484 F5 82              1912 	mov	dpl,a
                           1913 ;	Peephole 181	changed mov to clr
   0486 E4                 1914 	clr	a
   0487 35 2C              1915 	addc	a,(_set_NFRE_PARM_2 + 1)
   0489 F5 83              1916 	mov	dph,a
                           1917 ;     genPointerGet
                           1918 ;     genFarPointerGet
   048B E0                 1919 	movx	a,@dptr
                           1920 ;     genAssign
                           1921 ;	Peephole 100	removed redundant mov
   048C FB                 1922 	mov	r3,a
   048D 90 60 02           1923 	mov	dptr,#_DDC0_DR2
   0490 F0                 1924 	movx	@dptr,a
                           1925 ;src/ddc_regs.c:198: DDC0_DR1 = (buf[1] & 0xFF);
                           1926 ;     genPlus
                           1927 ;     genPlusIncr
   0491 74 01              1928 	mov	a,#0x01
   0493 25 2B              1929 	add	a,_set_NFRE_PARM_2
   0495 F5 82              1930 	mov	dpl,a
                           1931 ;	Peephole 181	changed mov to clr
   0497 E4                 1932 	clr	a
   0498 35 2C              1933 	addc	a,(_set_NFRE_PARM_2 + 1)
   049A F5 83              1934 	mov	dph,a
                           1935 ;     genPointerGet
                           1936 ;     genFarPointerGet
   049C E0                 1937 	movx	a,@dptr
                           1938 ;     genAssign
                           1939 ;	Peephole 100	removed redundant mov
   049D FB                 1940 	mov	r3,a
   049E 90 60 01           1941 	mov	dptr,#_DDC0_DR1
   04A1 F0                 1942 	movx	@dptr,a
                           1943 ;src/ddc_regs.c:199: DDC0_DR0 = (buf[0] & 0xFF);
                           1944 ;     genAssign
   04A2 85 2B 82           1945 	mov	dpl,_set_NFRE_PARM_2
   04A5 85 2C 83           1946 	mov	dph,(_set_NFRE_PARM_2 + 1)
                           1947 ;     genPointerGet
                           1948 ;     genFarPointerGet
   04A8 E0                 1949 	movx	a,@dptr
                           1950 ;     genAssign
                           1951 ;	Peephole 100	removed redundant mov
   04A9 FB                 1952 	mov	r3,a
   04AA 90 60 00           1953 	mov	dptr,#_DDC0_DR0
   04AD F0                 1954 	movx	@dptr,a
                           1955 ;src/ddc_regs.c:200: return 4;
                           1956 ;     genRet
   04AE 75 82 04           1957 	mov	dpl,#0x04
                           1958 ;	Peephole 112.b	changed ljmp to sjmp
                           1959 ;	Peephole 251.b	replaced sjmp to ret with ret
   04B1 22                 1960 	ret
   04B2                    1961 00106$:
                           1962 ;src/ddc_regs.c:201: } else if (index == 1) {
                           1963 ;     genCmpEq
                           1964 ;	Peephole 112.b	changed ljmp to sjmp
                           1965 ;	Peephole 199	optimized misc jump sequence
   04B2 BA 01 4F           1966 	cjne	r2,#0x01,00107$
                           1967 ;00116$:
                           1968 ;	Peephole 200	removed redundant sjmp
   04B5                    1969 00117$:
                           1970 ;src/ddc_regs.c:202: DDC1_AMR = (NFRE & 0xFF00) >> 8;
                           1971 ;     genAssign
   04B5 90 60 17           1972 	mov	dptr,#_DDC1_AMR
   04B8 74 03              1973 	mov	a,#0x03
   04BA F0                 1974 	movx	@dptr,a
                           1975 ;src/ddc_regs.c:203: DDC1_LAR = (NFRE & 0xFF);
                           1976 ;     genAssign
   04BB 90 60 16           1977 	mov	dptr,#_DDC1_LAR
   04BE 74 03              1978 	mov	a,#0x03
   04C0 F0                 1979 	movx	@dptr,a
                           1980 ;src/ddc_regs.c:204: DDC1_DR3 = (buf[3] & 0xFF);
                           1981 ;     genPlus
                           1982 ;     genPlusIncr
   04C1 74 03              1983 	mov	a,#0x03
   04C3 25 2B              1984 	add	a,_set_NFRE_PARM_2
   04C5 F5 82              1985 	mov	dpl,a
                           1986 ;	Peephole 181	changed mov to clr
   04C7 E4                 1987 	clr	a
   04C8 35 2C              1988 	addc	a,(_set_NFRE_PARM_2 + 1)
   04CA F5 83              1989 	mov	dph,a
                           1990 ;     genPointerGet
                           1991 ;     genFarPointerGet
   04CC E0                 1992 	movx	a,@dptr
                           1993 ;     genAssign
                           1994 ;	Peephole 100	removed redundant mov
   04CD FA                 1995 	mov	r2,a
   04CE 90 60 13           1996 	mov	dptr,#_DDC1_DR3
   04D1 F0                 1997 	movx	@dptr,a
                           1998 ;src/ddc_regs.c:205: DDC1_DR2 = (buf[2] & 0xFF);
                           1999 ;     genPlus
                           2000 ;     genPlusIncr
   04D2 74 02              2001 	mov	a,#0x02
   04D4 25 2B              2002 	add	a,_set_NFRE_PARM_2
   04D6 F5 82              2003 	mov	dpl,a
                           2004 ;	Peephole 181	changed mov to clr
   04D8 E4                 2005 	clr	a
   04D9 35 2C              2006 	addc	a,(_set_NFRE_PARM_2 + 1)
   04DB F5 83              2007 	mov	dph,a
                           2008 ;     genPointerGet
                           2009 ;     genFarPointerGet
   04DD E0                 2010 	movx	a,@dptr
                           2011 ;     genAssign
                           2012 ;	Peephole 100	removed redundant mov
   04DE FA                 2013 	mov	r2,a
   04DF 90 60 12           2014 	mov	dptr,#_DDC1_DR2
   04E2 F0                 2015 	movx	@dptr,a
                           2016 ;src/ddc_regs.c:206: DDC1_DR1 = (buf[1] & 0xFF);
                           2017 ;     genPlus
                           2018 ;     genPlusIncr
   04E3 74 01              2019 	mov	a,#0x01
   04E5 25 2B              2020 	add	a,_set_NFRE_PARM_2
   04E7 F5 82              2021 	mov	dpl,a
                           2022 ;	Peephole 181	changed mov to clr
   04E9 E4                 2023 	clr	a
   04EA 35 2C              2024 	addc	a,(_set_NFRE_PARM_2 + 1)
   04EC F5 83              2025 	mov	dph,a
                           2026 ;     genPointerGet
                           2027 ;     genFarPointerGet
   04EE E0                 2028 	movx	a,@dptr
                           2029 ;     genAssign
                           2030 ;	Peephole 100	removed redundant mov
   04EF FA                 2031 	mov	r2,a
   04F0 90 60 11           2032 	mov	dptr,#_DDC1_DR1
   04F3 F0                 2033 	movx	@dptr,a
                           2034 ;src/ddc_regs.c:207: DDC1_DR0 = (buf[0] & 0xFF);
                           2035 ;     genAssign
   04F4 85 2B 82           2036 	mov	dpl,_set_NFRE_PARM_2
   04F7 85 2C 83           2037 	mov	dph,(_set_NFRE_PARM_2 + 1)
                           2038 ;     genPointerGet
                           2039 ;     genFarPointerGet
   04FA E0                 2040 	movx	a,@dptr
                           2041 ;     genAssign
                           2042 ;	Peephole 100	removed redundant mov
   04FB FA                 2043 	mov	r2,a
   04FC 90 60 10           2044 	mov	dptr,#_DDC1_DR0
   04FF F0                 2045 	movx	@dptr,a
                           2046 ;src/ddc_regs.c:208: return 4;
                           2047 ;     genRet
   0500 75 82 04           2048 	mov	dpl,#0x04
                           2049 ;	Peephole 112.b	changed ljmp to sjmp
                           2050 ;src/ddc_regs.c:210: return 0;
                           2051 ;     genRet
                           2052 ;	Peephole 237.a	removed sjmp to ret
   0503 22                 2053 	ret
   0504                    2054 00107$:
   0504 75 82 00           2055 	mov	dpl,#0x00
   0507                    2056 00108$:
   0507 22                 2057 	ret
                           2058 ;------------------------------------------------------------
                           2059 ;Allocation info for local variables in function 'read_NPHA'
                           2060 ;------------------------------------------------------------
                           2061 ;buf                       Allocated with name '_read_NPHA_PARM_2'
                           2062 ;len                       Allocated with name '_read_NPHA_PARM_3'
                           2063 ;index                     Allocated to registers r2 
                           2064 ;------------------------------------------------------------
                           2065 ;src/ddc_regs.c:216: read_NPHA (unsigned char index, xdata unsigned char *buf,
                           2066 ;	-----------------------------------------
                           2067 ;	 function read_NPHA
                           2068 ;	-----------------------------------------
   0508                    2069 _read_NPHA:
                           2070 ;     genReceive
   0508 AA 82              2071 	mov	r2,dpl
                           2072 ;src/ddc_regs.c:219: if (len < 2) return 0; // 16 bits
                           2073 ;     genCmpLt
                           2074 ;     genCmp
                           2075 ;     genIfxJump
                           2076 ;	Peephole 108	removed ljmp by inverse jump logic
                           2077 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   050A 74 FE              2078 	mov	a,#0x100 - 0x02
   050C 25 2D              2079 	add	a,_read_NPHA_PARM_3
   050E 40 04              2080 	jc	00102$
   0510                    2081 00113$:
                           2082 ;     genRet
   0510 75 82 00           2083 	mov	dpl,#0x00
                           2084 ;	Peephole 112.b	changed ljmp to sjmp
                           2085 ;	Peephole 251.b	replaced sjmp to ret with ret
   0513 22                 2086 	ret
   0514                    2087 00102$:
                           2088 ;src/ddc_regs.c:221: if (index == 0)
                           2089 ;     genCmpEq
                           2090 ;	Peephole 112.b	changed ljmp to sjmp
                           2091 ;	Peephole 199	optimized misc jump sequence
   0514 BA 00 18           2092 	cjne	r2,#0x00,00106$
                           2093 ;00114$:
                           2094 ;	Peephole 200	removed redundant sjmp
   0517                    2095 00115$:
                           2096 ;src/ddc_regs.c:223: DDC0_AMR = (NPHA & 0xFF00) >> 8;
                           2097 ;     genAssign
   0517 90 60 07           2098 	mov	dptr,#_DDC0_AMR
   051A 74 03              2099 	mov	a,#0x03
   051C F0                 2100 	movx	@dptr,a
                           2101 ;src/ddc_regs.c:224: DDC0_LAR = (NPHA & 0xFF);
                           2102 ;     genAssign
   051D 90 60 06           2103 	mov	dptr,#_DDC0_LAR
   0520 74 04              2104 	mov	a,#0x04
   0522 F0                 2105 	movx	@dptr,a
                           2106 ;src/ddc_regs.c:225: buf[0] == (DDC0_DR0 & 0xFF);
                           2107 ;     genDummyRead
   0523 90 60 00           2108 	mov	dptr,#_DDC0_DR0
   0526 E0                 2109 	movx	a,@dptr
                           2110 ;src/ddc_regs.c:226: buf[1] == (DDC0_DR1 & 0xFF);
                           2111 ;     genDummyRead
   0527 90 60 01           2112 	mov	dptr,#_DDC0_DR1
   052A E0                 2113 	movx	a,@dptr
                           2114 ;src/ddc_regs.c:227: return 2;
                           2115 ;     genRet
   052B 75 82 02           2116 	mov	dpl,#0x02
                           2117 ;	Peephole 112.b	changed ljmp to sjmp
                           2118 ;	Peephole 251.b	replaced sjmp to ret with ret
   052E 22                 2119 	ret
   052F                    2120 00106$:
                           2121 ;src/ddc_regs.c:228: } else if (index == 1) {
                           2122 ;     genCmpEq
                           2123 ;	Peephole 112.b	changed ljmp to sjmp
                           2124 ;	Peephole 199	optimized misc jump sequence
   052F BA 01 18           2125 	cjne	r2,#0x01,00107$
                           2126 ;00116$:
                           2127 ;	Peephole 200	removed redundant sjmp
   0532                    2128 00117$:
                           2129 ;src/ddc_regs.c:229: DDC1_AMR = (NPHA & 0xFF00) >> 8;
                           2130 ;     genAssign
   0532 90 60 17           2131 	mov	dptr,#_DDC1_AMR
   0535 74 03              2132 	mov	a,#0x03
   0537 F0                 2133 	movx	@dptr,a
                           2134 ;src/ddc_regs.c:230: DDC1_LAR = (NPHA & 0xFF);
                           2135 ;     genAssign
   0538 90 60 16           2136 	mov	dptr,#_DDC1_LAR
   053B 74 04              2137 	mov	a,#0x04
   053D F0                 2138 	movx	@dptr,a
                           2139 ;src/ddc_regs.c:231: buf[0] == (DDC1_DR0 & 0xFF);
                           2140 ;     genDummyRead
   053E 90 60 10           2141 	mov	dptr,#_DDC1_DR0
   0541 E0                 2142 	movx	a,@dptr
                           2143 ;src/ddc_regs.c:232: buf[1] == (DDC1_DR1 & 0xFF);
                           2144 ;     genDummyRead
   0542 90 60 11           2145 	mov	dptr,#_DDC1_DR1
   0545 E0                 2146 	movx	a,@dptr
                           2147 ;src/ddc_regs.c:233: return 2;
                           2148 ;     genRet
   0546 75 82 02           2149 	mov	dpl,#0x02
                           2150 ;	Peephole 112.b	changed ljmp to sjmp
                           2151 ;src/ddc_regs.c:235: return 0;
                           2152 ;     genRet
                           2153 ;	Peephole 237.a	removed sjmp to ret
   0549 22                 2154 	ret
   054A                    2155 00107$:
   054A 75 82 00           2156 	mov	dpl,#0x00
   054D                    2157 00108$:
   054D 22                 2158 	ret
                           2159 ;------------------------------------------------------------
                           2160 ;Allocation info for local variables in function 'set_NPHA'
                           2161 ;------------------------------------------------------------
                           2162 ;buf                       Allocated with name '_set_NPHA_PARM_2'
                           2163 ;len                       Allocated with name '_set_NPHA_PARM_3'
                           2164 ;index                     Allocated to registers r2 
                           2165 ;------------------------------------------------------------
                           2166 ;src/ddc_regs.c:240: set_NPHA (unsigned char index, xdata unsigned char *buf,
                           2167 ;	-----------------------------------------
                           2168 ;	 function set_NPHA
                           2169 ;	-----------------------------------------
   054E                    2170 _set_NPHA:
                           2171 ;     genReceive
   054E AA 82              2172 	mov	r2,dpl
                           2173 ;src/ddc_regs.c:243: if (len < 2) return 0; // 16 bits
                           2174 ;     genCmpLt
                           2175 ;     genCmp
                           2176 ;     genIfxJump
                           2177 ;	Peephole 108	removed ljmp by inverse jump logic
                           2178 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0550 74 FE              2179 	mov	a,#0x100 - 0x02
   0552 25 2D              2180 	add	a,_set_NPHA_PARM_3
   0554 40 04              2181 	jc	00102$
   0556                    2182 00113$:
                           2183 ;     genRet
   0556 75 82 00           2184 	mov	dpl,#0x00
                           2185 ;	Peephole 112.b	changed ljmp to sjmp
                           2186 ;	Peephole 251.b	replaced sjmp to ret with ret
   0559 22                 2187 	ret
   055A                    2188 00102$:
                           2189 ;src/ddc_regs.c:245: if (index == 0)
                           2190 ;     genCmpEq
                           2191 ;	Peephole 112.b	changed ljmp to sjmp
                           2192 ;	Peephole 199	optimized misc jump sequence
   055A BA 00 2D           2193 	cjne	r2,#0x00,00106$
                           2194 ;00114$:
                           2195 ;	Peephole 200	removed redundant sjmp
   055D                    2196 00115$:
                           2197 ;src/ddc_regs.c:247: DDC0_AMR = (NPHA & 0xFF00) >> 8;
                           2198 ;     genAssign
   055D 90 60 07           2199 	mov	dptr,#_DDC0_AMR
   0560 74 03              2200 	mov	a,#0x03
   0562 F0                 2201 	movx	@dptr,a
                           2202 ;src/ddc_regs.c:248: DDC0_LAR = (NPHA & 0xFF);
                           2203 ;     genAssign
   0563 90 60 06           2204 	mov	dptr,#_DDC0_LAR
   0566 74 04              2205 	mov	a,#0x04
   0568 F0                 2206 	movx	@dptr,a
                           2207 ;src/ddc_regs.c:249: DDC0_DR1 = (buf[1] & 0xFF);
                           2208 ;     genPlus
                           2209 ;     genPlusIncr
   0569 74 01              2210 	mov	a,#0x01
   056B 25 2B              2211 	add	a,_set_NPHA_PARM_2
   056D F5 82              2212 	mov	dpl,a
                           2213 ;	Peephole 181	changed mov to clr
   056F E4                 2214 	clr	a
   0570 35 2C              2215 	addc	a,(_set_NPHA_PARM_2 + 1)
   0572 F5 83              2216 	mov	dph,a
                           2217 ;     genPointerGet
                           2218 ;     genFarPointerGet
   0574 E0                 2219 	movx	a,@dptr
                           2220 ;     genAssign
                           2221 ;	Peephole 100	removed redundant mov
   0575 FB                 2222 	mov	r3,a
   0576 90 60 01           2223 	mov	dptr,#_DDC0_DR1
   0579 F0                 2224 	movx	@dptr,a
                           2225 ;src/ddc_regs.c:250: DDC0_DR0 = (buf[0] & 0xFF);
                           2226 ;     genAssign
   057A 85 2B 82           2227 	mov	dpl,_set_NPHA_PARM_2
   057D 85 2C 83           2228 	mov	dph,(_set_NPHA_PARM_2 + 1)
                           2229 ;     genPointerGet
                           2230 ;     genFarPointerGet
   0580 E0                 2231 	movx	a,@dptr
                           2232 ;     genAssign
                           2233 ;	Peephole 100	removed redundant mov
   0581 FB                 2234 	mov	r3,a
   0582 90 60 00           2235 	mov	dptr,#_DDC0_DR0
   0585 F0                 2236 	movx	@dptr,a
                           2237 ;src/ddc_regs.c:251: return 2;
                           2238 ;     genRet
   0586 75 82 02           2239 	mov	dpl,#0x02
                           2240 ;	Peephole 112.b	changed ljmp to sjmp
                           2241 ;	Peephole 251.b	replaced sjmp to ret with ret
   0589 22                 2242 	ret
   058A                    2243 00106$:
                           2244 ;src/ddc_regs.c:252: } else if (index == 1) {
                           2245 ;     genCmpEq
                           2246 ;	Peephole 112.b	changed ljmp to sjmp
                           2247 ;	Peephole 199	optimized misc jump sequence
   058A BA 01 2D           2248 	cjne	r2,#0x01,00107$
                           2249 ;00116$:
                           2250 ;	Peephole 200	removed redundant sjmp
   058D                    2251 00117$:
                           2252 ;src/ddc_regs.c:253: DDC1_AMR = (NPHA & 0xFF00) >> 8;
                           2253 ;     genAssign
   058D 90 60 17           2254 	mov	dptr,#_DDC1_AMR
   0590 74 03              2255 	mov	a,#0x03
   0592 F0                 2256 	movx	@dptr,a
                           2257 ;src/ddc_regs.c:254: DDC1_LAR = (NPHA & 0xFF);
                           2258 ;     genAssign
   0593 90 60 16           2259 	mov	dptr,#_DDC1_LAR
   0596 74 04              2260 	mov	a,#0x04
   0598 F0                 2261 	movx	@dptr,a
                           2262 ;src/ddc_regs.c:255: DDC1_DR1 = (buf[1] & 0xFF);
                           2263 ;     genPlus
                           2264 ;     genPlusIncr
   0599 74 01              2265 	mov	a,#0x01
   059B 25 2B              2266 	add	a,_set_NPHA_PARM_2
   059D F5 82              2267 	mov	dpl,a
                           2268 ;	Peephole 181	changed mov to clr
   059F E4                 2269 	clr	a
   05A0 35 2C              2270 	addc	a,(_set_NPHA_PARM_2 + 1)
   05A2 F5 83              2271 	mov	dph,a
                           2272 ;     genPointerGet
                           2273 ;     genFarPointerGet
   05A4 E0                 2274 	movx	a,@dptr
                           2275 ;     genAssign
                           2276 ;	Peephole 100	removed redundant mov
   05A5 FA                 2277 	mov	r2,a
   05A6 90 60 11           2278 	mov	dptr,#_DDC1_DR1
   05A9 F0                 2279 	movx	@dptr,a
                           2280 ;src/ddc_regs.c:256: DDC1_DR0 = (buf[0] & 0xFF);
                           2281 ;     genAssign
   05AA 85 2B 82           2282 	mov	dpl,_set_NPHA_PARM_2
   05AD 85 2C 83           2283 	mov	dph,(_set_NPHA_PARM_2 + 1)
                           2284 ;     genPointerGet
                           2285 ;     genFarPointerGet
   05B0 E0                 2286 	movx	a,@dptr
                           2287 ;     genAssign
                           2288 ;	Peephole 100	removed redundant mov
   05B1 FA                 2289 	mov	r2,a
   05B2 90 60 10           2290 	mov	dptr,#_DDC1_DR0
   05B5 F0                 2291 	movx	@dptr,a
                           2292 ;src/ddc_regs.c:257: return 2;
                           2293 ;     genRet
   05B6 75 82 02           2294 	mov	dpl,#0x02
                           2295 ;	Peephole 112.b	changed ljmp to sjmp
                           2296 ;src/ddc_regs.c:259: return 0;
                           2297 ;     genRet
                           2298 ;	Peephole 237.a	removed sjmp to ret
   05B9 22                 2299 	ret
   05BA                    2300 00107$:
   05BA 75 82 00           2301 	mov	dpl,#0x00
   05BD                    2302 00108$:
   05BD 22                 2303 	ret
                           2304 ;------------------------------------------------------------
                           2305 ;Allocation info for local variables in function 'read_C2SR'
                           2306 ;------------------------------------------------------------
                           2307 ;buf                       Allocated with name '_read_C2SR_PARM_2'
                           2308 ;len                       Allocated with name '_read_C2SR_PARM_3'
                           2309 ;index                     Allocated to registers r2 
                           2310 ;------------------------------------------------------------
                           2311 ;src/ddc_regs.c:264: read_C2SR (unsigned char index, xdata unsigned char *buf,
                           2312 ;	-----------------------------------------
                           2313 ;	 function read_C2SR
                           2314 ;	-----------------------------------------
   05BE                    2315 _read_C2SR:
                           2316 ;     genReceive
   05BE AA 82              2317 	mov	r2,dpl
                           2318 ;src/ddc_regs.c:267: if (len < 1) return 0; // 8 bits
                           2319 ;     genCmpLt
                           2320 ;     genCmp
                           2321 ;     genIfxJump
                           2322 ;	Peephole 108	removed ljmp by inverse jump logic
                           2323 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   05C0 74 FF              2324 	mov	a,#0x100 - 0x01
   05C2 25 2D              2325 	add	a,_read_C2SR_PARM_3
   05C4 40 04              2326 	jc	00102$
   05C6                    2327 00113$:
                           2328 ;     genRet
   05C6 75 82 00           2329 	mov	dpl,#0x00
                           2330 ;	Peephole 112.b	changed ljmp to sjmp
                           2331 ;	Peephole 251.b	replaced sjmp to ret with ret
   05C9 22                 2332 	ret
   05CA                    2333 00102$:
                           2334 ;src/ddc_regs.c:269: if (index == 0)
                           2335 ;     genCmpEq
                           2336 ;	Peephole 112.b	changed ljmp to sjmp
                           2337 ;	Peephole 199	optimized misc jump sequence
   05CA BA 00 14           2338 	cjne	r2,#0x00,00106$
                           2339 ;00114$:
                           2340 ;	Peephole 200	removed redundant sjmp
   05CD                    2341 00115$:
                           2342 ;src/ddc_regs.c:271: DDC0_AMR = (C2SR & 0xFF00) >> 8;
                           2343 ;     genAssign
   05CD 90 60 07           2344 	mov	dptr,#_DDC0_AMR
   05D0 74 03              2345 	mov	a,#0x03
   05D2 F0                 2346 	movx	@dptr,a
                           2347 ;src/ddc_regs.c:272: DDC0_LAR = (C2SR & 0xFF);
                           2348 ;     genAssign
   05D3 90 60 06           2349 	mov	dptr,#_DDC0_LAR
   05D6 74 05              2350 	mov	a,#0x05
   05D8 F0                 2351 	movx	@dptr,a
                           2352 ;src/ddc_regs.c:273: buf[0] == DDC0_DR0;
                           2353 ;     genDummyRead
   05D9 90 60 00           2354 	mov	dptr,#_DDC0_DR0
   05DC E0                 2355 	movx	a,@dptr
                           2356 ;src/ddc_regs.c:274: return 1;
                           2357 ;     genRet
   05DD 75 82 01           2358 	mov	dpl,#0x01
                           2359 ;	Peephole 112.b	changed ljmp to sjmp
                           2360 ;	Peephole 251.b	replaced sjmp to ret with ret
   05E0 22                 2361 	ret
   05E1                    2362 00106$:
                           2363 ;src/ddc_regs.c:275: } else if (index == 1) {
                           2364 ;     genCmpEq
                           2365 ;	Peephole 112.b	changed ljmp to sjmp
                           2366 ;	Peephole 199	optimized misc jump sequence
   05E1 BA 01 14           2367 	cjne	r2,#0x01,00107$
                           2368 ;00116$:
                           2369 ;	Peephole 200	removed redundant sjmp
   05E4                    2370 00117$:
                           2371 ;src/ddc_regs.c:276: DDC1_AMR = (C2SR & 0xFF00) >> 8;
                           2372 ;     genAssign
   05E4 90 60 17           2373 	mov	dptr,#_DDC1_AMR
   05E7 74 03              2374 	mov	a,#0x03
   05E9 F0                 2375 	movx	@dptr,a
                           2376 ;src/ddc_regs.c:277: DDC1_LAR = (C2SR & 0xFF);
                           2377 ;     genAssign
   05EA 90 60 16           2378 	mov	dptr,#_DDC1_LAR
   05ED 74 05              2379 	mov	a,#0x05
   05EF F0                 2380 	movx	@dptr,a
                           2381 ;src/ddc_regs.c:278: buf[0] == DDC1_DR0;
                           2382 ;     genDummyRead
   05F0 90 60 10           2383 	mov	dptr,#_DDC1_DR0
   05F3 E0                 2384 	movx	a,@dptr
                           2385 ;src/ddc_regs.c:279: return 1;
                           2386 ;     genRet
   05F4 75 82 01           2387 	mov	dpl,#0x01
                           2388 ;	Peephole 112.b	changed ljmp to sjmp
                           2389 ;src/ddc_regs.c:281: return 0;
                           2390 ;     genRet
                           2391 ;	Peephole 237.a	removed sjmp to ret
   05F7 22                 2392 	ret
   05F8                    2393 00107$:
   05F8 75 82 00           2394 	mov	dpl,#0x00
   05FB                    2395 00108$:
   05FB 22                 2396 	ret
                           2397 ;------------------------------------------------------------
                           2398 ;Allocation info for local variables in function 'set_C2SR'
                           2399 ;------------------------------------------------------------
                           2400 ;buf                       Allocated with name '_set_C2SR_PARM_2'
                           2401 ;len                       Allocated with name '_set_C2SR_PARM_3'
                           2402 ;index                     Allocated to registers r2 
                           2403 ;------------------------------------------------------------
                           2404 ;src/ddc_regs.c:286: set_C2SR (unsigned char index, xdata unsigned char *buf,
                           2405 ;	-----------------------------------------
                           2406 ;	 function set_C2SR
                           2407 ;	-----------------------------------------
   05FC                    2408 _set_C2SR:
                           2409 ;     genReceive
   05FC AA 82              2410 	mov	r2,dpl
                           2411 ;src/ddc_regs.c:289: if (len < 1) return 0; // 8 bits
                           2412 ;     genCmpLt
                           2413 ;     genCmp
                           2414 ;     genIfxJump
                           2415 ;	Peephole 108	removed ljmp by inverse jump logic
                           2416 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   05FE 74 FF              2417 	mov	a,#0x100 - 0x01
   0600 25 2D              2418 	add	a,_set_C2SR_PARM_3
   0602 40 04              2419 	jc	00102$
   0604                    2420 00113$:
                           2421 ;     genRet
   0604 75 82 00           2422 	mov	dpl,#0x00
                           2423 ;	Peephole 112.b	changed ljmp to sjmp
                           2424 ;	Peephole 251.b	replaced sjmp to ret with ret
   0607 22                 2425 	ret
   0608                    2426 00102$:
                           2427 ;src/ddc_regs.c:291: if (index == 0)
                           2428 ;     genCmpEq
                           2429 ;	Peephole 112.b	changed ljmp to sjmp
                           2430 ;	Peephole 199	optimized misc jump sequence
   0608 BA 00 1C           2431 	cjne	r2,#0x00,00106$
                           2432 ;00114$:
                           2433 ;	Peephole 200	removed redundant sjmp
   060B                    2434 00115$:
                           2435 ;src/ddc_regs.c:293: DDC0_AMR = (C2SR & 0xFF00) >> 8;
                           2436 ;     genAssign
   060B 90 60 07           2437 	mov	dptr,#_DDC0_AMR
   060E 74 03              2438 	mov	a,#0x03
   0610 F0                 2439 	movx	@dptr,a
                           2440 ;src/ddc_regs.c:294: DDC0_LAR = (C2SR & 0xFF);
                           2441 ;     genAssign
   0611 90 60 06           2442 	mov	dptr,#_DDC0_LAR
   0614 74 05              2443 	mov	a,#0x05
   0616 F0                 2444 	movx	@dptr,a
                           2445 ;src/ddc_regs.c:295: DDC0_DR0 = (buf[0] & 0xFF);
                           2446 ;     genAssign
   0617 85 2B 82           2447 	mov	dpl,_set_C2SR_PARM_2
   061A 85 2C 83           2448 	mov	dph,(_set_C2SR_PARM_2 + 1)
                           2449 ;     genPointerGet
                           2450 ;     genFarPointerGet
   061D E0                 2451 	movx	a,@dptr
                           2452 ;     genAssign
                           2453 ;	Peephole 100	removed redundant mov
   061E FB                 2454 	mov	r3,a
   061F 90 60 00           2455 	mov	dptr,#_DDC0_DR0
   0622 F0                 2456 	movx	@dptr,a
                           2457 ;src/ddc_regs.c:296: return 1;
                           2458 ;     genRet
   0623 75 82 01           2459 	mov	dpl,#0x01
                           2460 ;	Peephole 112.b	changed ljmp to sjmp
                           2461 ;	Peephole 251.b	replaced sjmp to ret with ret
   0626 22                 2462 	ret
   0627                    2463 00106$:
                           2464 ;src/ddc_regs.c:297: } else if (index == 1) {
                           2465 ;     genCmpEq
                           2466 ;	Peephole 112.b	changed ljmp to sjmp
                           2467 ;	Peephole 199	optimized misc jump sequence
   0627 BA 01 1C           2468 	cjne	r2,#0x01,00107$
                           2469 ;00116$:
                           2470 ;	Peephole 200	removed redundant sjmp
   062A                    2471 00117$:
                           2472 ;src/ddc_regs.c:298: DDC1_AMR = (C2SR & 0xFF00) >> 8;
                           2473 ;     genAssign
   062A 90 60 17           2474 	mov	dptr,#_DDC1_AMR
   062D 74 03              2475 	mov	a,#0x03
   062F F0                 2476 	movx	@dptr,a
                           2477 ;src/ddc_regs.c:299: DDC1_LAR = (C2SR & 0xFF);
                           2478 ;     genAssign
   0630 90 60 16           2479 	mov	dptr,#_DDC1_LAR
   0633 74 05              2480 	mov	a,#0x05
   0635 F0                 2481 	movx	@dptr,a
                           2482 ;src/ddc_regs.c:300: DDC1_DR0 = (buf[0] & 0xFF);
                           2483 ;     genAssign
   0636 85 2B 82           2484 	mov	dpl,_set_C2SR_PARM_2
   0639 85 2C 83           2485 	mov	dph,(_set_C2SR_PARM_2 + 1)
                           2486 ;     genPointerGet
                           2487 ;     genFarPointerGet
   063C E0                 2488 	movx	a,@dptr
                           2489 ;     genAssign
                           2490 ;	Peephole 100	removed redundant mov
   063D FA                 2491 	mov	r2,a
   063E 90 60 10           2492 	mov	dptr,#_DDC1_DR0
   0641 F0                 2493 	movx	@dptr,a
                           2494 ;src/ddc_regs.c:301: return 1;
                           2495 ;     genRet
   0642 75 82 01           2496 	mov	dpl,#0x01
                           2497 ;	Peephole 112.b	changed ljmp to sjmp
                           2498 ;src/ddc_regs.c:303: return 0;
                           2499 ;     genRet
                           2500 ;	Peephole 237.a	removed sjmp to ret
   0645 22                 2501 	ret
   0646                    2502 00107$:
   0646 75 82 00           2503 	mov	dpl,#0x00
   0649                    2504 00108$:
   0649 22                 2505 	ret
                           2506 ;------------------------------------------------------------
                           2507 ;Allocation info for local variables in function 'read_C2DEC'
                           2508 ;------------------------------------------------------------
                           2509 ;buf                       Allocated with name '_read_C2DEC_PARM_2'
                           2510 ;len                       Allocated with name '_read_C2DEC_PARM_3'
                           2511 ;index                     Allocated to registers r2 
                           2512 ;------------------------------------------------------------
                           2513 ;src/ddc_regs.c:309: read_C2DEC (unsigned char index, xdata unsigned char *buf,
                           2514 ;	-----------------------------------------
                           2515 ;	 function read_C2DEC
                           2516 ;	-----------------------------------------
   064A                    2517 _read_C2DEC:
                           2518 ;     genReceive
   064A AA 82              2519 	mov	r2,dpl
                           2520 ;src/ddc_regs.c:312: if (len < 1) return 0; // 8 bits
                           2521 ;     genCmpLt
                           2522 ;     genCmp
                           2523 ;     genIfxJump
                           2524 ;	Peephole 108	removed ljmp by inverse jump logic
                           2525 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   064C 74 FF              2526 	mov	a,#0x100 - 0x01
   064E 25 2D              2527 	add	a,_read_C2DEC_PARM_3
   0650 40 04              2528 	jc	00102$
   0652                    2529 00113$:
                           2530 ;     genRet
   0652 75 82 00           2531 	mov	dpl,#0x00
                           2532 ;	Peephole 112.b	changed ljmp to sjmp
                           2533 ;	Peephole 251.b	replaced sjmp to ret with ret
   0655 22                 2534 	ret
   0656                    2535 00102$:
                           2536 ;src/ddc_regs.c:314: if (index == 0)
                           2537 ;     genCmpEq
                           2538 ;	Peephole 112.b	changed ljmp to sjmp
                           2539 ;	Peephole 199	optimized misc jump sequence
   0656 BA 00 14           2540 	cjne	r2,#0x00,00106$
                           2541 ;00114$:
                           2542 ;	Peephole 200	removed redundant sjmp
   0659                    2543 00115$:
                           2544 ;src/ddc_regs.c:316: DDC0_AMR = (C2DEC & 0xFF00) >> 8;
                           2545 ;     genAssign
   0659 90 60 07           2546 	mov	dptr,#_DDC0_AMR
   065C 74 03              2547 	mov	a,#0x03
   065E F0                 2548 	movx	@dptr,a
                           2549 ;src/ddc_regs.c:317: DDC0_LAR = (C2DEC & 0xFF);
                           2550 ;     genAssign
   065F 90 60 06           2551 	mov	dptr,#_DDC0_LAR
   0662 74 06              2552 	mov	a,#0x06
   0664 F0                 2553 	movx	@dptr,a
                           2554 ;src/ddc_regs.c:318: buf[0] == DDC0_DR0;
                           2555 ;     genDummyRead
   0665 90 60 00           2556 	mov	dptr,#_DDC0_DR0
   0668 E0                 2557 	movx	a,@dptr
                           2558 ;src/ddc_regs.c:319: return 1;
                           2559 ;     genRet
   0669 75 82 01           2560 	mov	dpl,#0x01
                           2561 ;	Peephole 112.b	changed ljmp to sjmp
                           2562 ;	Peephole 251.b	replaced sjmp to ret with ret
   066C 22                 2563 	ret
   066D                    2564 00106$:
                           2565 ;src/ddc_regs.c:320: } else if (index == 1) {
                           2566 ;     genCmpEq
                           2567 ;	Peephole 112.b	changed ljmp to sjmp
                           2568 ;	Peephole 199	optimized misc jump sequence
   066D BA 01 14           2569 	cjne	r2,#0x01,00107$
                           2570 ;00116$:
                           2571 ;	Peephole 200	removed redundant sjmp
   0670                    2572 00117$:
                           2573 ;src/ddc_regs.c:321: DDC1_AMR = (C2DEC & 0xFF00) >> 8;
                           2574 ;     genAssign
   0670 90 60 17           2575 	mov	dptr,#_DDC1_AMR
   0673 74 03              2576 	mov	a,#0x03
   0675 F0                 2577 	movx	@dptr,a
                           2578 ;src/ddc_regs.c:322: DDC1_LAR = (C2DEC & 0xFF);
                           2579 ;     genAssign
   0676 90 60 16           2580 	mov	dptr,#_DDC1_LAR
   0679 74 06              2581 	mov	a,#0x06
   067B F0                 2582 	movx	@dptr,a
                           2583 ;src/ddc_regs.c:323: buf[0] == DDC1_DR0;
                           2584 ;     genDummyRead
   067C 90 60 10           2585 	mov	dptr,#_DDC1_DR0
   067F E0                 2586 	movx	a,@dptr
                           2587 ;src/ddc_regs.c:324: return 1;
                           2588 ;     genRet
   0680 75 82 01           2589 	mov	dpl,#0x01
                           2590 ;	Peephole 112.b	changed ljmp to sjmp
                           2591 ;src/ddc_regs.c:326: return 0;
                           2592 ;     genRet
                           2593 ;	Peephole 237.a	removed sjmp to ret
   0683 22                 2594 	ret
   0684                    2595 00107$:
   0684 75 82 00           2596 	mov	dpl,#0x00
   0687                    2597 00108$:
   0687 22                 2598 	ret
                           2599 ;------------------------------------------------------------
                           2600 ;Allocation info for local variables in function 'set_C2DEC'
                           2601 ;------------------------------------------------------------
                           2602 ;buf                       Allocated with name '_set_C2DEC_PARM_2'
                           2603 ;len                       Allocated with name '_set_C2DEC_PARM_3'
                           2604 ;index                     Allocated to registers r2 
                           2605 ;------------------------------------------------------------
                           2606 ;src/ddc_regs.c:331: set_C2DEC (unsigned char index, xdata unsigned char *buf,
                           2607 ;	-----------------------------------------
                           2608 ;	 function set_C2DEC
                           2609 ;	-----------------------------------------
   0688                    2610 _set_C2DEC:
                           2611 ;     genReceive
   0688 AA 82              2612 	mov	r2,dpl
                           2613 ;src/ddc_regs.c:334: if (len < 1) return 0; // 8 bits
                           2614 ;     genCmpLt
                           2615 ;     genCmp
                           2616 ;     genIfxJump
                           2617 ;	Peephole 108	removed ljmp by inverse jump logic
                           2618 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   068A 74 FF              2619 	mov	a,#0x100 - 0x01
   068C 25 2D              2620 	add	a,_set_C2DEC_PARM_3
   068E 40 04              2621 	jc	00102$
   0690                    2622 00113$:
                           2623 ;     genRet
   0690 75 82 00           2624 	mov	dpl,#0x00
                           2625 ;	Peephole 112.b	changed ljmp to sjmp
                           2626 ;	Peephole 251.b	replaced sjmp to ret with ret
   0693 22                 2627 	ret
   0694                    2628 00102$:
                           2629 ;src/ddc_regs.c:336: if (index == 0)
                           2630 ;     genCmpEq
                           2631 ;	Peephole 112.b	changed ljmp to sjmp
                           2632 ;	Peephole 199	optimized misc jump sequence
   0694 BA 00 1C           2633 	cjne	r2,#0x00,00106$
                           2634 ;00114$:
                           2635 ;	Peephole 200	removed redundant sjmp
   0697                    2636 00115$:
                           2637 ;src/ddc_regs.c:338: DDC0_AMR = (C2DEC & 0xFF00) >> 8;
                           2638 ;     genAssign
   0697 90 60 07           2639 	mov	dptr,#_DDC0_AMR
   069A 74 03              2640 	mov	a,#0x03
   069C F0                 2641 	movx	@dptr,a
                           2642 ;src/ddc_regs.c:339: DDC0_LAR = (C2DEC & 0xFF);
                           2643 ;     genAssign
   069D 90 60 06           2644 	mov	dptr,#_DDC0_LAR
   06A0 74 06              2645 	mov	a,#0x06
   06A2 F0                 2646 	movx	@dptr,a
                           2647 ;src/ddc_regs.c:340: DDC0_DR0 = (buf[0] & 0xFF);
                           2648 ;     genAssign
   06A3 85 2B 82           2649 	mov	dpl,_set_C2DEC_PARM_2
   06A6 85 2C 83           2650 	mov	dph,(_set_C2DEC_PARM_2 + 1)
                           2651 ;     genPointerGet
                           2652 ;     genFarPointerGet
   06A9 E0                 2653 	movx	a,@dptr
                           2654 ;     genAssign
                           2655 ;	Peephole 100	removed redundant mov
   06AA FB                 2656 	mov	r3,a
   06AB 90 60 00           2657 	mov	dptr,#_DDC0_DR0
   06AE F0                 2658 	movx	@dptr,a
                           2659 ;src/ddc_regs.c:341: return 1;
                           2660 ;     genRet
   06AF 75 82 01           2661 	mov	dpl,#0x01
                           2662 ;	Peephole 112.b	changed ljmp to sjmp
                           2663 ;	Peephole 251.b	replaced sjmp to ret with ret
   06B2 22                 2664 	ret
   06B3                    2665 00106$:
                           2666 ;src/ddc_regs.c:342: } else if (index == 1) {
                           2667 ;     genCmpEq
                           2668 ;	Peephole 112.b	changed ljmp to sjmp
                           2669 ;	Peephole 199	optimized misc jump sequence
   06B3 BA 01 1C           2670 	cjne	r2,#0x01,00107$
                           2671 ;00116$:
                           2672 ;	Peephole 200	removed redundant sjmp
   06B6                    2673 00117$:
                           2674 ;src/ddc_regs.c:343: DDC1_AMR = (C2DEC & 0xFF00) >> 8;
                           2675 ;     genAssign
   06B6 90 60 17           2676 	mov	dptr,#_DDC1_AMR
   06B9 74 03              2677 	mov	a,#0x03
   06BB F0                 2678 	movx	@dptr,a
                           2679 ;src/ddc_regs.c:344: DDC1_LAR = (C2DEC & 0xFF);
                           2680 ;     genAssign
   06BC 90 60 16           2681 	mov	dptr,#_DDC1_LAR
   06BF 74 06              2682 	mov	a,#0x06
   06C1 F0                 2683 	movx	@dptr,a
                           2684 ;src/ddc_regs.c:345: DDC1_DR0 = (buf[0] & 0xFF);
                           2685 ;     genAssign
   06C2 85 2B 82           2686 	mov	dpl,_set_C2DEC_PARM_2
   06C5 85 2C 83           2687 	mov	dph,(_set_C2DEC_PARM_2 + 1)
                           2688 ;     genPointerGet
                           2689 ;     genFarPointerGet
   06C8 E0                 2690 	movx	a,@dptr
                           2691 ;     genAssign
                           2692 ;	Peephole 100	removed redundant mov
   06C9 FA                 2693 	mov	r2,a
   06CA 90 60 10           2694 	mov	dptr,#_DDC1_DR0
   06CD F0                 2695 	movx	@dptr,a
                           2696 ;src/ddc_regs.c:346: return 1;
                           2697 ;     genRet
   06CE 75 82 01           2698 	mov	dpl,#0x01
                           2699 ;	Peephole 112.b	changed ljmp to sjmp
                           2700 ;src/ddc_regs.c:348: return 0;
                           2701 ;     genRet
                           2702 ;	Peephole 237.a	removed sjmp to ret
   06D1 22                 2703 	ret
   06D2                    2704 00107$:
   06D2 75 82 00           2705 	mov	dpl,#0x00
   06D5                    2706 00108$:
   06D5 22                 2707 	ret
                           2708 ;------------------------------------------------------------
                           2709 ;Allocation info for local variables in function 'read_C5SR'
                           2710 ;------------------------------------------------------------
                           2711 ;buf                       Allocated with name '_read_C5SR_PARM_2'
                           2712 ;len                       Allocated with name '_read_C5SR_PARM_3'
                           2713 ;index                     Allocated to registers r2 
                           2714 ;------------------------------------------------------------
                           2715 ;src/ddc_regs.c:353: read_C5SR (unsigned char index, xdata unsigned char *buf,
                           2716 ;	-----------------------------------------
                           2717 ;	 function read_C5SR
                           2718 ;	-----------------------------------------
   06D6                    2719 _read_C5SR:
                           2720 ;     genReceive
   06D6 AA 82              2721 	mov	r2,dpl
                           2722 ;src/ddc_regs.c:356: if (len < 1) return 0; // 8 bits
                           2723 ;     genCmpLt
                           2724 ;     genCmp
                           2725 ;     genIfxJump
                           2726 ;	Peephole 108	removed ljmp by inverse jump logic
                           2727 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   06D8 74 FF              2728 	mov	a,#0x100 - 0x01
   06DA 25 2D              2729 	add	a,_read_C5SR_PARM_3
   06DC 40 04              2730 	jc	00102$
   06DE                    2731 00113$:
                           2732 ;     genRet
   06DE 75 82 00           2733 	mov	dpl,#0x00
                           2734 ;	Peephole 112.b	changed ljmp to sjmp
                           2735 ;	Peephole 251.b	replaced sjmp to ret with ret
   06E1 22                 2736 	ret
   06E2                    2737 00102$:
                           2738 ;src/ddc_regs.c:358: if (index == 0)
                           2739 ;     genCmpEq
                           2740 ;	Peephole 112.b	changed ljmp to sjmp
                           2741 ;	Peephole 199	optimized misc jump sequence
   06E2 BA 00 14           2742 	cjne	r2,#0x00,00106$
                           2743 ;00114$:
                           2744 ;	Peephole 200	removed redundant sjmp
   06E5                    2745 00115$:
                           2746 ;src/ddc_regs.c:360: DDC0_AMR = (C5SR & 0xFF00) >> 8;
                           2747 ;     genAssign
   06E5 90 60 07           2748 	mov	dptr,#_DDC0_AMR
   06E8 74 03              2749 	mov	a,#0x03
   06EA F0                 2750 	movx	@dptr,a
                           2751 ;src/ddc_regs.c:361: DDC0_LAR = (C5SR & 0xFF);
                           2752 ;     genAssign
   06EB 90 60 06           2753 	mov	dptr,#_DDC0_LAR
   06EE 74 07              2754 	mov	a,#0x07
   06F0 F0                 2755 	movx	@dptr,a
                           2756 ;src/ddc_regs.c:362: buf[0] == DDC0_DR0;
                           2757 ;     genDummyRead
   06F1 90 60 00           2758 	mov	dptr,#_DDC0_DR0
   06F4 E0                 2759 	movx	a,@dptr
                           2760 ;src/ddc_regs.c:363: return 1;
                           2761 ;     genRet
   06F5 75 82 01           2762 	mov	dpl,#0x01
                           2763 ;	Peephole 112.b	changed ljmp to sjmp
                           2764 ;	Peephole 251.b	replaced sjmp to ret with ret
   06F8 22                 2765 	ret
   06F9                    2766 00106$:
                           2767 ;src/ddc_regs.c:364: } else if (index == 1) {
                           2768 ;     genCmpEq
                           2769 ;	Peephole 112.b	changed ljmp to sjmp
                           2770 ;	Peephole 199	optimized misc jump sequence
   06F9 BA 01 14           2771 	cjne	r2,#0x01,00107$
                           2772 ;00116$:
                           2773 ;	Peephole 200	removed redundant sjmp
   06FC                    2774 00117$:
                           2775 ;src/ddc_regs.c:365: DDC1_AMR = (C5SR & 0xFF00) >> 8;
                           2776 ;     genAssign
   06FC 90 60 17           2777 	mov	dptr,#_DDC1_AMR
   06FF 74 03              2778 	mov	a,#0x03
   0701 F0                 2779 	movx	@dptr,a
                           2780 ;src/ddc_regs.c:366: DDC1_LAR = (C5SR & 0xFF);
                           2781 ;     genAssign
   0702 90 60 16           2782 	mov	dptr,#_DDC1_LAR
   0705 74 07              2783 	mov	a,#0x07
   0707 F0                 2784 	movx	@dptr,a
                           2785 ;src/ddc_regs.c:367: buf[0] == DDC1_DR0;
                           2786 ;     genDummyRead
   0708 90 60 10           2787 	mov	dptr,#_DDC1_DR0
   070B E0                 2788 	movx	a,@dptr
                           2789 ;src/ddc_regs.c:368: return 1;
                           2790 ;     genRet
   070C 75 82 01           2791 	mov	dpl,#0x01
                           2792 ;	Peephole 112.b	changed ljmp to sjmp
                           2793 ;src/ddc_regs.c:370: return 0;
                           2794 ;     genRet
                           2795 ;	Peephole 237.a	removed sjmp to ret
   070F 22                 2796 	ret
   0710                    2797 00107$:
   0710 75 82 00           2798 	mov	dpl,#0x00
   0713                    2799 00108$:
   0713 22                 2800 	ret
                           2801 ;------------------------------------------------------------
                           2802 ;Allocation info for local variables in function 'set_C5SR'
                           2803 ;------------------------------------------------------------
                           2804 ;buf                       Allocated with name '_set_C5SR_PARM_2'
                           2805 ;len                       Allocated with name '_set_C5SR_PARM_3'
                           2806 ;index                     Allocated to registers r2 
                           2807 ;------------------------------------------------------------
                           2808 ;src/ddc_regs.c:375: set_C5SR (unsigned char index, xdata unsigned char *buf,
                           2809 ;	-----------------------------------------
                           2810 ;	 function set_C5SR
                           2811 ;	-----------------------------------------
   0714                    2812 _set_C5SR:
                           2813 ;     genReceive
   0714 AA 82              2814 	mov	r2,dpl
                           2815 ;src/ddc_regs.c:378: if (len < 1) return 0; // 8 bits
                           2816 ;     genCmpLt
                           2817 ;     genCmp
                           2818 ;     genIfxJump
                           2819 ;	Peephole 108	removed ljmp by inverse jump logic
                           2820 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0716 74 FF              2821 	mov	a,#0x100 - 0x01
   0718 25 2D              2822 	add	a,_set_C5SR_PARM_3
   071A 40 04              2823 	jc	00102$
   071C                    2824 00113$:
                           2825 ;     genRet
   071C 75 82 00           2826 	mov	dpl,#0x00
                           2827 ;	Peephole 112.b	changed ljmp to sjmp
                           2828 ;	Peephole 251.b	replaced sjmp to ret with ret
   071F 22                 2829 	ret
   0720                    2830 00102$:
                           2831 ;src/ddc_regs.c:380: if (index == 0)
                           2832 ;     genCmpEq
                           2833 ;	Peephole 112.b	changed ljmp to sjmp
                           2834 ;	Peephole 199	optimized misc jump sequence
   0720 BA 00 1C           2835 	cjne	r2,#0x00,00106$
                           2836 ;00114$:
                           2837 ;	Peephole 200	removed redundant sjmp
   0723                    2838 00115$:
                           2839 ;src/ddc_regs.c:382: DDC0_AMR = (C5SR & 0xFF00) >> 8;
                           2840 ;     genAssign
   0723 90 60 07           2841 	mov	dptr,#_DDC0_AMR
   0726 74 03              2842 	mov	a,#0x03
   0728 F0                 2843 	movx	@dptr,a
                           2844 ;src/ddc_regs.c:383: DDC0_LAR = (C5SR & 0xFF);
                           2845 ;     genAssign
   0729 90 60 06           2846 	mov	dptr,#_DDC0_LAR
   072C 74 07              2847 	mov	a,#0x07
   072E F0                 2848 	movx	@dptr,a
                           2849 ;src/ddc_regs.c:384: DDC0_DR0 = (buf[0] & 0xFF);
                           2850 ;     genAssign
   072F 85 2B 82           2851 	mov	dpl,_set_C5SR_PARM_2
   0732 85 2C 83           2852 	mov	dph,(_set_C5SR_PARM_2 + 1)
                           2853 ;     genPointerGet
                           2854 ;     genFarPointerGet
   0735 E0                 2855 	movx	a,@dptr
                           2856 ;     genAssign
                           2857 ;	Peephole 100	removed redundant mov
   0736 FB                 2858 	mov	r3,a
   0737 90 60 00           2859 	mov	dptr,#_DDC0_DR0
   073A F0                 2860 	movx	@dptr,a
                           2861 ;src/ddc_regs.c:385: return 1;
                           2862 ;     genRet
   073B 75 82 01           2863 	mov	dpl,#0x01
                           2864 ;	Peephole 112.b	changed ljmp to sjmp
                           2865 ;	Peephole 251.b	replaced sjmp to ret with ret
   073E 22                 2866 	ret
   073F                    2867 00106$:
                           2868 ;src/ddc_regs.c:386: } else if (index == 1) {
                           2869 ;     genCmpEq
                           2870 ;	Peephole 112.b	changed ljmp to sjmp
                           2871 ;	Peephole 199	optimized misc jump sequence
   073F BA 01 1C           2872 	cjne	r2,#0x01,00107$
                           2873 ;00116$:
                           2874 ;	Peephole 200	removed redundant sjmp
   0742                    2875 00117$:
                           2876 ;src/ddc_regs.c:387: DDC1_AMR = (C5SR & 0xFF00) >> 8;
                           2877 ;     genAssign
   0742 90 60 17           2878 	mov	dptr,#_DDC1_AMR
   0745 74 03              2879 	mov	a,#0x03
   0747 F0                 2880 	movx	@dptr,a
                           2881 ;src/ddc_regs.c:388: DDC1_LAR = (C5SR & 0xFF);
                           2882 ;     genAssign
   0748 90 60 16           2883 	mov	dptr,#_DDC1_LAR
   074B 74 07              2884 	mov	a,#0x07
   074D F0                 2885 	movx	@dptr,a
                           2886 ;src/ddc_regs.c:389: DDC1_DR0 = (buf[0] & 0xFF);
                           2887 ;     genAssign
   074E 85 2B 82           2888 	mov	dpl,_set_C5SR_PARM_2
   0751 85 2C 83           2889 	mov	dph,(_set_C5SR_PARM_2 + 1)
                           2890 ;     genPointerGet
                           2891 ;     genFarPointerGet
   0754 E0                 2892 	movx	a,@dptr
                           2893 ;     genAssign
                           2894 ;	Peephole 100	removed redundant mov
   0755 FA                 2895 	mov	r2,a
   0756 90 60 10           2896 	mov	dptr,#_DDC1_DR0
   0759 F0                 2897 	movx	@dptr,a
                           2898 ;src/ddc_regs.c:390: return 1;
                           2899 ;     genRet
   075A 75 82 01           2900 	mov	dpl,#0x01
                           2901 ;	Peephole 112.b	changed ljmp to sjmp
                           2902 ;src/ddc_regs.c:392: return 0;
                           2903 ;     genRet
                           2904 ;	Peephole 237.a	removed sjmp to ret
   075D 22                 2905 	ret
   075E                    2906 00107$:
   075E 75 82 00           2907 	mov	dpl,#0x00
   0761                    2908 00108$:
   0761 22                 2909 	ret
                           2910 ;------------------------------------------------------------
                           2911 ;Allocation info for local variables in function 'read_C5DEC'
                           2912 ;------------------------------------------------------------
                           2913 ;buf                       Allocated with name '_read_C5DEC_PARM_2'
                           2914 ;len                       Allocated with name '_read_C5DEC_PARM_3'
                           2915 ;index                     Allocated to registers r2 
                           2916 ;------------------------------------------------------------
                           2917 ;src/ddc_regs.c:397: read_C5DEC (unsigned char index, xdata unsigned char *buf,
                           2918 ;	-----------------------------------------
                           2919 ;	 function read_C5DEC
                           2920 ;	-----------------------------------------
   0762                    2921 _read_C5DEC:
                           2922 ;     genReceive
   0762 AA 82              2923 	mov	r2,dpl
                           2924 ;src/ddc_regs.c:400: if (len < 1) return 0; // 8 bits
                           2925 ;     genCmpLt
                           2926 ;     genCmp
                           2927 ;     genIfxJump
                           2928 ;	Peephole 108	removed ljmp by inverse jump logic
                           2929 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0764 74 FF              2930 	mov	a,#0x100 - 0x01
   0766 25 2D              2931 	add	a,_read_C5DEC_PARM_3
   0768 40 04              2932 	jc	00102$
   076A                    2933 00113$:
                           2934 ;     genRet
   076A 75 82 00           2935 	mov	dpl,#0x00
                           2936 ;	Peephole 112.b	changed ljmp to sjmp
                           2937 ;	Peephole 251.b	replaced sjmp to ret with ret
   076D 22                 2938 	ret
   076E                    2939 00102$:
                           2940 ;src/ddc_regs.c:402: if (index == 0)
                           2941 ;     genCmpEq
                           2942 ;	Peephole 112.b	changed ljmp to sjmp
                           2943 ;	Peephole 199	optimized misc jump sequence
   076E BA 00 14           2944 	cjne	r2,#0x00,00106$
                           2945 ;00114$:
                           2946 ;	Peephole 200	removed redundant sjmp
   0771                    2947 00115$:
                           2948 ;src/ddc_regs.c:404: DDC0_AMR = (C5DEC & 0xFF00) >> 8;
                           2949 ;     genAssign
   0771 90 60 07           2950 	mov	dptr,#_DDC0_AMR
   0774 74 03              2951 	mov	a,#0x03
   0776 F0                 2952 	movx	@dptr,a
                           2953 ;src/ddc_regs.c:405: DDC0_LAR = (C5DEC & 0xFF);
                           2954 ;     genAssign
   0777 90 60 06           2955 	mov	dptr,#_DDC0_LAR
   077A 74 08              2956 	mov	a,#0x08
   077C F0                 2957 	movx	@dptr,a
                           2958 ;src/ddc_regs.c:406: buf[0] == DDC0_DR0;
                           2959 ;     genDummyRead
   077D 90 60 00           2960 	mov	dptr,#_DDC0_DR0
   0780 E0                 2961 	movx	a,@dptr
                           2962 ;src/ddc_regs.c:407: return 1;
                           2963 ;     genRet
   0781 75 82 01           2964 	mov	dpl,#0x01
                           2965 ;	Peephole 112.b	changed ljmp to sjmp
                           2966 ;	Peephole 251.b	replaced sjmp to ret with ret
   0784 22                 2967 	ret
   0785                    2968 00106$:
                           2969 ;src/ddc_regs.c:408: } else if (index == 1) {
                           2970 ;     genCmpEq
                           2971 ;	Peephole 112.b	changed ljmp to sjmp
                           2972 ;	Peephole 199	optimized misc jump sequence
   0785 BA 01 14           2973 	cjne	r2,#0x01,00107$
                           2974 ;00116$:
                           2975 ;	Peephole 200	removed redundant sjmp
   0788                    2976 00117$:
                           2977 ;src/ddc_regs.c:409: DDC1_AMR = (C5DEC & 0xFF00) >> 8;
                           2978 ;     genAssign
   0788 90 60 17           2979 	mov	dptr,#_DDC1_AMR
   078B 74 03              2980 	mov	a,#0x03
   078D F0                 2981 	movx	@dptr,a
                           2982 ;src/ddc_regs.c:410: DDC1_LAR = (C5DEC & 0xFF);
                           2983 ;     genAssign
   078E 90 60 16           2984 	mov	dptr,#_DDC1_LAR
   0791 74 08              2985 	mov	a,#0x08
   0793 F0                 2986 	movx	@dptr,a
                           2987 ;src/ddc_regs.c:411: buf[0] == DDC1_DR0;
                           2988 ;     genDummyRead
   0794 90 60 10           2989 	mov	dptr,#_DDC1_DR0
   0797 E0                 2990 	movx	a,@dptr
                           2991 ;src/ddc_regs.c:412: return 1;
                           2992 ;     genRet
   0798 75 82 01           2993 	mov	dpl,#0x01
                           2994 ;	Peephole 112.b	changed ljmp to sjmp
                           2995 ;src/ddc_regs.c:414: return 0;
                           2996 ;     genRet
                           2997 ;	Peephole 237.a	removed sjmp to ret
   079B 22                 2998 	ret
   079C                    2999 00107$:
   079C 75 82 00           3000 	mov	dpl,#0x00
   079F                    3001 00108$:
   079F 22                 3002 	ret
                           3003 ;------------------------------------------------------------
                           3004 ;Allocation info for local variables in function 'set_C5DEC'
                           3005 ;------------------------------------------------------------
                           3006 ;buf                       Allocated with name '_set_C5DEC_PARM_2'
                           3007 ;len                       Allocated with name '_set_C5DEC_PARM_3'
                           3008 ;index                     Allocated to registers r2 
                           3009 ;------------------------------------------------------------
                           3010 ;src/ddc_regs.c:419: set_C5DEC (unsigned char index, xdata unsigned char *buf,
                           3011 ;	-----------------------------------------
                           3012 ;	 function set_C5DEC
                           3013 ;	-----------------------------------------
   07A0                    3014 _set_C5DEC:
                           3015 ;     genReceive
   07A0 AA 82              3016 	mov	r2,dpl
                           3017 ;src/ddc_regs.c:422: if (len < 1) return 0; // 8 bits
                           3018 ;     genCmpLt
                           3019 ;     genCmp
                           3020 ;     genIfxJump
                           3021 ;	Peephole 108	removed ljmp by inverse jump logic
                           3022 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   07A2 74 FF              3023 	mov	a,#0x100 - 0x01
   07A4 25 2D              3024 	add	a,_set_C5DEC_PARM_3
   07A6 40 04              3025 	jc	00102$
   07A8                    3026 00113$:
                           3027 ;     genRet
   07A8 75 82 00           3028 	mov	dpl,#0x00
                           3029 ;	Peephole 112.b	changed ljmp to sjmp
                           3030 ;	Peephole 251.b	replaced sjmp to ret with ret
   07AB 22                 3031 	ret
   07AC                    3032 00102$:
                           3033 ;src/ddc_regs.c:424: if (index == 0)
                           3034 ;     genCmpEq
                           3035 ;	Peephole 112.b	changed ljmp to sjmp
                           3036 ;	Peephole 199	optimized misc jump sequence
   07AC BA 00 1C           3037 	cjne	r2,#0x00,00106$
                           3038 ;00114$:
                           3039 ;	Peephole 200	removed redundant sjmp
   07AF                    3040 00115$:
                           3041 ;src/ddc_regs.c:426: DDC0_AMR = (C5DEC & 0xFF00) >> 8;
                           3042 ;     genAssign
   07AF 90 60 07           3043 	mov	dptr,#_DDC0_AMR
   07B2 74 03              3044 	mov	a,#0x03
   07B4 F0                 3045 	movx	@dptr,a
                           3046 ;src/ddc_regs.c:427: DDC0_LAR = (C5DEC & 0xFF);
                           3047 ;     genAssign
   07B5 90 60 06           3048 	mov	dptr,#_DDC0_LAR
   07B8 74 08              3049 	mov	a,#0x08
   07BA F0                 3050 	movx	@dptr,a
                           3051 ;src/ddc_regs.c:428: DDC0_DR0 = (buf[0] & 0xFF);
                           3052 ;     genAssign
   07BB 85 2B 82           3053 	mov	dpl,_set_C5DEC_PARM_2
   07BE 85 2C 83           3054 	mov	dph,(_set_C5DEC_PARM_2 + 1)
                           3055 ;     genPointerGet
                           3056 ;     genFarPointerGet
   07C1 E0                 3057 	movx	a,@dptr
                           3058 ;     genAssign
                           3059 ;	Peephole 100	removed redundant mov
   07C2 FB                 3060 	mov	r3,a
   07C3 90 60 00           3061 	mov	dptr,#_DDC0_DR0
   07C6 F0                 3062 	movx	@dptr,a
                           3063 ;src/ddc_regs.c:429: return 1;
                           3064 ;     genRet
   07C7 75 82 01           3065 	mov	dpl,#0x01
                           3066 ;	Peephole 112.b	changed ljmp to sjmp
                           3067 ;	Peephole 251.b	replaced sjmp to ret with ret
   07CA 22                 3068 	ret
   07CB                    3069 00106$:
                           3070 ;src/ddc_regs.c:430: } else if (index == 1) {
                           3071 ;     genCmpEq
                           3072 ;	Peephole 112.b	changed ljmp to sjmp
                           3073 ;	Peephole 199	optimized misc jump sequence
   07CB BA 01 1C           3074 	cjne	r2,#0x01,00107$
                           3075 ;00116$:
                           3076 ;	Peephole 200	removed redundant sjmp
   07CE                    3077 00117$:
                           3078 ;src/ddc_regs.c:431: DDC1_AMR = (C5DEC & 0xFF00) >> 8;
                           3079 ;     genAssign
   07CE 90 60 17           3080 	mov	dptr,#_DDC1_AMR
   07D1 74 03              3081 	mov	a,#0x03
   07D3 F0                 3082 	movx	@dptr,a
                           3083 ;src/ddc_regs.c:432: DDC1_LAR = (C5DEC & 0xFF);
                           3084 ;     genAssign
   07D4 90 60 16           3085 	mov	dptr,#_DDC1_LAR
   07D7 74 08              3086 	mov	a,#0x08
   07D9 F0                 3087 	movx	@dptr,a
                           3088 ;src/ddc_regs.c:433: DDC1_DR0 = (buf[0] & 0xFF);
                           3089 ;     genAssign
   07DA 85 2B 82           3090 	mov	dpl,_set_C5DEC_PARM_2
   07DD 85 2C 83           3091 	mov	dph,(_set_C5DEC_PARM_2 + 1)
                           3092 ;     genPointerGet
                           3093 ;     genFarPointerGet
   07E0 E0                 3094 	movx	a,@dptr
                           3095 ;     genAssign
                           3096 ;	Peephole 100	removed redundant mov
   07E1 FA                 3097 	mov	r2,a
   07E2 90 60 10           3098 	mov	dptr,#_DDC1_DR0
   07E5 F0                 3099 	movx	@dptr,a
                           3100 ;src/ddc_regs.c:434: return 1;
                           3101 ;     genRet
   07E6 75 82 01           3102 	mov	dpl,#0x01
                           3103 ;	Peephole 112.b	changed ljmp to sjmp
                           3104 ;src/ddc_regs.c:436: return 0;
                           3105 ;     genRet
                           3106 ;	Peephole 237.a	removed sjmp to ret
   07E9 22                 3107 	ret
   07EA                    3108 00107$:
   07EA 75 82 00           3109 	mov	dpl,#0x00
   07ED                    3110 00108$:
   07ED 22                 3111 	ret
                           3112 ;------------------------------------------------------------
                           3113 ;Allocation info for local variables in function 'read_RCR'
                           3114 ;------------------------------------------------------------
                           3115 ;buf                       Allocated with name '_read_RCR_PARM_2'
                           3116 ;len                       Allocated with name '_read_RCR_PARM_3'
                           3117 ;index                     Allocated to registers r2 
                           3118 ;------------------------------------------------------------
                           3119 ;src/ddc_regs.c:441: read_RCR (unsigned char index, xdata unsigned char *buf,
                           3120 ;	-----------------------------------------
                           3121 ;	 function read_RCR
                           3122 ;	-----------------------------------------
   07EE                    3123 _read_RCR:
                           3124 ;     genReceive
   07EE AA 82              3125 	mov	r2,dpl
                           3126 ;src/ddc_regs.c:444: if (len < 1) return 0; // 8 bits
                           3127 ;     genCmpLt
                           3128 ;     genCmp
                           3129 ;     genIfxJump
                           3130 ;	Peephole 108	removed ljmp by inverse jump logic
                           3131 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   07F0 74 FF              3132 	mov	a,#0x100 - 0x01
   07F2 25 2D              3133 	add	a,_read_RCR_PARM_3
   07F4 40 04              3134 	jc	00102$
   07F6                    3135 00113$:
                           3136 ;     genRet
   07F6 75 82 00           3137 	mov	dpl,#0x00
                           3138 ;	Peephole 112.b	changed ljmp to sjmp
                           3139 ;	Peephole 251.b	replaced sjmp to ret with ret
   07F9 22                 3140 	ret
   07FA                    3141 00102$:
                           3142 ;src/ddc_regs.c:446: if (index == 0)
                           3143 ;     genCmpEq
                           3144 ;	Peephole 112.b	changed ljmp to sjmp
                           3145 ;	Peephole 199	optimized misc jump sequence
   07FA BA 00 14           3146 	cjne	r2,#0x00,00106$
                           3147 ;00114$:
                           3148 ;	Peephole 200	removed redundant sjmp
   07FD                    3149 00115$:
                           3150 ;src/ddc_regs.c:448: DDC0_AMR = (RCR & 0xFF00) >> 8;
                           3151 ;     genAssign
   07FD 90 60 07           3152 	mov	dptr,#_DDC0_AMR
   0800 74 03              3153 	mov	a,#0x03
   0802 F0                 3154 	movx	@dptr,a
                           3155 ;src/ddc_regs.c:449: DDC0_LAR = (RCR & 0xFF);
                           3156 ;     genAssign
   0803 90 60 06           3157 	mov	dptr,#_DDC0_LAR
   0806 74 09              3158 	mov	a,#0x09
   0808 F0                 3159 	movx	@dptr,a
                           3160 ;src/ddc_regs.c:450: buf[0] == DDC0_DR0;
                           3161 ;     genDummyRead
   0809 90 60 00           3162 	mov	dptr,#_DDC0_DR0
   080C E0                 3163 	movx	a,@dptr
                           3164 ;src/ddc_regs.c:451: return 1;
                           3165 ;     genRet
   080D 75 82 01           3166 	mov	dpl,#0x01
                           3167 ;	Peephole 112.b	changed ljmp to sjmp
                           3168 ;	Peephole 251.b	replaced sjmp to ret with ret
   0810 22                 3169 	ret
   0811                    3170 00106$:
                           3171 ;src/ddc_regs.c:452: } else if (index == 1) {
                           3172 ;     genCmpEq
                           3173 ;	Peephole 112.b	changed ljmp to sjmp
                           3174 ;	Peephole 199	optimized misc jump sequence
   0811 BA 01 14           3175 	cjne	r2,#0x01,00107$
                           3176 ;00116$:
                           3177 ;	Peephole 200	removed redundant sjmp
   0814                    3178 00117$:
                           3179 ;src/ddc_regs.c:453: DDC1_AMR = (RCR & 0xFF00) >> 8;
                           3180 ;     genAssign
   0814 90 60 17           3181 	mov	dptr,#_DDC1_AMR
   0817 74 03              3182 	mov	a,#0x03
   0819 F0                 3183 	movx	@dptr,a
                           3184 ;src/ddc_regs.c:454: DDC1_LAR = (RCR & 0xFF);
                           3185 ;     genAssign
   081A 90 60 16           3186 	mov	dptr,#_DDC1_LAR
   081D 74 09              3187 	mov	a,#0x09
   081F F0                 3188 	movx	@dptr,a
                           3189 ;src/ddc_regs.c:455: buf[0] == DDC1_DR0;
                           3190 ;     genDummyRead
   0820 90 60 10           3191 	mov	dptr,#_DDC1_DR0
   0823 E0                 3192 	movx	a,@dptr
                           3193 ;src/ddc_regs.c:456: return 1;
                           3194 ;     genRet
   0824 75 82 01           3195 	mov	dpl,#0x01
                           3196 ;	Peephole 112.b	changed ljmp to sjmp
                           3197 ;src/ddc_regs.c:458: return 0;
                           3198 ;     genRet
                           3199 ;	Peephole 237.a	removed sjmp to ret
   0827 22                 3200 	ret
   0828                    3201 00107$:
   0828 75 82 00           3202 	mov	dpl,#0x00
   082B                    3203 00108$:
   082B 22                 3204 	ret
                           3205 ;------------------------------------------------------------
                           3206 ;Allocation info for local variables in function 'set_RCR'
                           3207 ;------------------------------------------------------------
                           3208 ;buf                       Allocated with name '_set_RCR_PARM_2'
                           3209 ;len                       Allocated with name '_set_RCR_PARM_3'
                           3210 ;index                     Allocated to registers r2 
                           3211 ;------------------------------------------------------------
                           3212 ;src/ddc_regs.c:463: set_RCR (unsigned char index, xdata unsigned char *buf,
                           3213 ;	-----------------------------------------
                           3214 ;	 function set_RCR
                           3215 ;	-----------------------------------------
   082C                    3216 _set_RCR:
                           3217 ;     genReceive
   082C AA 82              3218 	mov	r2,dpl
                           3219 ;src/ddc_regs.c:466: if (len < 1) return 0; // 8 bits
                           3220 ;     genCmpLt
                           3221 ;     genCmp
                           3222 ;     genIfxJump
                           3223 ;	Peephole 108	removed ljmp by inverse jump logic
                           3224 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   082E 74 FF              3225 	mov	a,#0x100 - 0x01
   0830 25 2D              3226 	add	a,_set_RCR_PARM_3
   0832 40 04              3227 	jc	00102$
   0834                    3228 00113$:
                           3229 ;     genRet
   0834 75 82 00           3230 	mov	dpl,#0x00
                           3231 ;	Peephole 112.b	changed ljmp to sjmp
                           3232 ;	Peephole 251.b	replaced sjmp to ret with ret
   0837 22                 3233 	ret
   0838                    3234 00102$:
                           3235 ;src/ddc_regs.c:468: if (index == 0)
                           3236 ;     genCmpEq
                           3237 ;	Peephole 112.b	changed ljmp to sjmp
                           3238 ;	Peephole 199	optimized misc jump sequence
   0838 BA 00 1C           3239 	cjne	r2,#0x00,00106$
                           3240 ;00114$:
                           3241 ;	Peephole 200	removed redundant sjmp
   083B                    3242 00115$:
                           3243 ;src/ddc_regs.c:470: DDC0_AMR = (RCR & 0xFF00) >> 8;
                           3244 ;     genAssign
   083B 90 60 07           3245 	mov	dptr,#_DDC0_AMR
   083E 74 03              3246 	mov	a,#0x03
   0840 F0                 3247 	movx	@dptr,a
                           3248 ;src/ddc_regs.c:471: DDC0_LAR = (RCR & 0xFF);
                           3249 ;     genAssign
   0841 90 60 06           3250 	mov	dptr,#_DDC0_LAR
   0844 74 09              3251 	mov	a,#0x09
   0846 F0                 3252 	movx	@dptr,a
                           3253 ;src/ddc_regs.c:472: DDC0_DR0 = (buf[0] & 0xFF);
                           3254 ;     genAssign
   0847 85 2B 82           3255 	mov	dpl,_set_RCR_PARM_2
   084A 85 2C 83           3256 	mov	dph,(_set_RCR_PARM_2 + 1)
                           3257 ;     genPointerGet
                           3258 ;     genFarPointerGet
   084D E0                 3259 	movx	a,@dptr
                           3260 ;     genAssign
                           3261 ;	Peephole 100	removed redundant mov
   084E FB                 3262 	mov	r3,a
   084F 90 60 00           3263 	mov	dptr,#_DDC0_DR0
   0852 F0                 3264 	movx	@dptr,a
                           3265 ;src/ddc_regs.c:473: return 1;
                           3266 ;     genRet
   0853 75 82 01           3267 	mov	dpl,#0x01
                           3268 ;	Peephole 112.b	changed ljmp to sjmp
                           3269 ;	Peephole 251.b	replaced sjmp to ret with ret
   0856 22                 3270 	ret
   0857                    3271 00106$:
                           3272 ;src/ddc_regs.c:474: } else if (index == 1) {
                           3273 ;     genCmpEq
                           3274 ;	Peephole 112.b	changed ljmp to sjmp
                           3275 ;	Peephole 199	optimized misc jump sequence
   0857 BA 01 1C           3276 	cjne	r2,#0x01,00107$
                           3277 ;00116$:
                           3278 ;	Peephole 200	removed redundant sjmp
   085A                    3279 00117$:
                           3280 ;src/ddc_regs.c:475: DDC1_AMR = (RCR & 0xFF00) >> 8;
                           3281 ;     genAssign
   085A 90 60 17           3282 	mov	dptr,#_DDC1_AMR
   085D 74 03              3283 	mov	a,#0x03
   085F F0                 3284 	movx	@dptr,a
                           3285 ;src/ddc_regs.c:476: DDC1_LAR = (RCR & 0xFF);
                           3286 ;     genAssign
   0860 90 60 16           3287 	mov	dptr,#_DDC1_LAR
   0863 74 09              3288 	mov	a,#0x09
   0865 F0                 3289 	movx	@dptr,a
                           3290 ;src/ddc_regs.c:477: DDC1_DR0 = (buf[0] & 0xFF);
                           3291 ;     genAssign
   0866 85 2B 82           3292 	mov	dpl,_set_RCR_PARM_2
   0869 85 2C 83           3293 	mov	dph,(_set_RCR_PARM_2 + 1)
                           3294 ;     genPointerGet
                           3295 ;     genFarPointerGet
   086C E0                 3296 	movx	a,@dptr
                           3297 ;     genAssign
                           3298 ;	Peephole 100	removed redundant mov
   086D FA                 3299 	mov	r2,a
   086E 90 60 10           3300 	mov	dptr,#_DDC1_DR0
   0871 F0                 3301 	movx	@dptr,a
                           3302 ;src/ddc_regs.c:478: return 1;
                           3303 ;     genRet
   0872 75 82 01           3304 	mov	dpl,#0x01
                           3305 ;	Peephole 112.b	changed ljmp to sjmp
                           3306 ;src/ddc_regs.c:480: return 0;
                           3307 ;     genRet
                           3308 ;	Peephole 237.a	removed sjmp to ret
   0875 22                 3309 	ret
   0876                    3310 00107$:
   0876 75 82 00           3311 	mov	dpl,#0x00
   0879                    3312 00108$:
   0879 22                 3313 	ret
                           3314 ;------------------------------------------------------------
                           3315 ;Allocation info for local variables in function 'read_RDEC'
                           3316 ;------------------------------------------------------------
                           3317 ;buf                       Allocated with name '_read_RDEC_PARM_2'
                           3318 ;len                       Allocated with name '_read_RDEC_PARM_3'
                           3319 ;index                     Allocated to registers r2 
                           3320 ;------------------------------------------------------------
                           3321 ;src/ddc_regs.c:486: read_RDEC (unsigned char index, xdata unsigned char *buf,
                           3322 ;	-----------------------------------------
                           3323 ;	 function read_RDEC
                           3324 ;	-----------------------------------------
   087A                    3325 _read_RDEC:
                           3326 ;     genReceive
   087A AA 82              3327 	mov	r2,dpl
                           3328 ;src/ddc_regs.c:489: if (len < 1) return 0; // 8 bits
                           3329 ;     genCmpLt
                           3330 ;     genCmp
                           3331 ;     genIfxJump
                           3332 ;	Peephole 108	removed ljmp by inverse jump logic
                           3333 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   087C 74 FF              3334 	mov	a,#0x100 - 0x01
   087E 25 2D              3335 	add	a,_read_RDEC_PARM_3
   0880 40 04              3336 	jc	00102$
   0882                    3337 00113$:
                           3338 ;     genRet
   0882 75 82 00           3339 	mov	dpl,#0x00
                           3340 ;	Peephole 112.b	changed ljmp to sjmp
                           3341 ;	Peephole 251.b	replaced sjmp to ret with ret
   0885 22                 3342 	ret
   0886                    3343 00102$:
                           3344 ;src/ddc_regs.c:491: if (index == 0)
                           3345 ;     genCmpEq
                           3346 ;	Peephole 112.b	changed ljmp to sjmp
                           3347 ;	Peephole 199	optimized misc jump sequence
   0886 BA 00 14           3348 	cjne	r2,#0x00,00106$
                           3349 ;00114$:
                           3350 ;	Peephole 200	removed redundant sjmp
   0889                    3351 00115$:
                           3352 ;src/ddc_regs.c:493: DDC0_AMR = (RDEC & 0xFF00) >> 8;
                           3353 ;     genAssign
   0889 90 60 07           3354 	mov	dptr,#_DDC0_AMR
   088C 74 03              3355 	mov	a,#0x03
   088E F0                 3356 	movx	@dptr,a
                           3357 ;src/ddc_regs.c:494: DDC0_LAR = (RDEC & 0xFF);
                           3358 ;     genAssign
   088F 90 60 06           3359 	mov	dptr,#_DDC0_LAR
   0892 74 0A              3360 	mov	a,#0x0A
   0894 F0                 3361 	movx	@dptr,a
                           3362 ;src/ddc_regs.c:495: buf[0] == DDC0_DR0;
                           3363 ;     genDummyRead
   0895 90 60 00           3364 	mov	dptr,#_DDC0_DR0
   0898 E0                 3365 	movx	a,@dptr
                           3366 ;src/ddc_regs.c:496: return 1;
                           3367 ;     genRet
   0899 75 82 01           3368 	mov	dpl,#0x01
                           3369 ;	Peephole 112.b	changed ljmp to sjmp
                           3370 ;	Peephole 251.b	replaced sjmp to ret with ret
   089C 22                 3371 	ret
   089D                    3372 00106$:
                           3373 ;src/ddc_regs.c:497: } else if (index == 1) {
                           3374 ;     genCmpEq
                           3375 ;	Peephole 112.b	changed ljmp to sjmp
                           3376 ;	Peephole 199	optimized misc jump sequence
   089D BA 01 14           3377 	cjne	r2,#0x01,00107$
                           3378 ;00116$:
                           3379 ;	Peephole 200	removed redundant sjmp
   08A0                    3380 00117$:
                           3381 ;src/ddc_regs.c:498: DDC1_AMR = (RDEC & 0xFF00) >> 8;
                           3382 ;     genAssign
   08A0 90 60 17           3383 	mov	dptr,#_DDC1_AMR
   08A3 74 03              3384 	mov	a,#0x03
   08A5 F0                 3385 	movx	@dptr,a
                           3386 ;src/ddc_regs.c:499: DDC1_LAR = (RDEC & 0xFF);
                           3387 ;     genAssign
   08A6 90 60 16           3388 	mov	dptr,#_DDC1_LAR
   08A9 74 0A              3389 	mov	a,#0x0A
   08AB F0                 3390 	movx	@dptr,a
                           3391 ;src/ddc_regs.c:500: buf[0] == DDC1_DR0;
                           3392 ;     genDummyRead
   08AC 90 60 10           3393 	mov	dptr,#_DDC1_DR0
   08AF E0                 3394 	movx	a,@dptr
                           3395 ;src/ddc_regs.c:501: return 1;
                           3396 ;     genRet
   08B0 75 82 01           3397 	mov	dpl,#0x01
                           3398 ;	Peephole 112.b	changed ljmp to sjmp
                           3399 ;src/ddc_regs.c:503: return 0;
                           3400 ;     genRet
                           3401 ;	Peephole 237.a	removed sjmp to ret
   08B3 22                 3402 	ret
   08B4                    3403 00107$:
   08B4 75 82 00           3404 	mov	dpl,#0x00
   08B7                    3405 00108$:
   08B7 22                 3406 	ret
                           3407 ;------------------------------------------------------------
                           3408 ;Allocation info for local variables in function 'set_RDEC'
                           3409 ;------------------------------------------------------------
                           3410 ;buf                       Allocated with name '_set_RDEC_PARM_2'
                           3411 ;len                       Allocated with name '_set_RDEC_PARM_3'
                           3412 ;index                     Allocated to registers r2 
                           3413 ;------------------------------------------------------------
                           3414 ;src/ddc_regs.c:508: set_RDEC (unsigned char index, xdata unsigned char *buf,
                           3415 ;	-----------------------------------------
                           3416 ;	 function set_RDEC
                           3417 ;	-----------------------------------------
   08B8                    3418 _set_RDEC:
                           3419 ;     genReceive
   08B8 AA 82              3420 	mov	r2,dpl
                           3421 ;src/ddc_regs.c:511: if (len < 1) return 0; // 8 bits
                           3422 ;     genCmpLt
                           3423 ;     genCmp
                           3424 ;     genIfxJump
                           3425 ;	Peephole 108	removed ljmp by inverse jump logic
                           3426 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   08BA 74 FF              3427 	mov	a,#0x100 - 0x01
   08BC 25 2D              3428 	add	a,_set_RDEC_PARM_3
   08BE 40 04              3429 	jc	00102$
   08C0                    3430 00113$:
                           3431 ;     genRet
   08C0 75 82 00           3432 	mov	dpl,#0x00
                           3433 ;	Peephole 112.b	changed ljmp to sjmp
                           3434 ;	Peephole 251.b	replaced sjmp to ret with ret
   08C3 22                 3435 	ret
   08C4                    3436 00102$:
                           3437 ;src/ddc_regs.c:513: if (index == 0)
                           3438 ;     genCmpEq
                           3439 ;	Peephole 112.b	changed ljmp to sjmp
                           3440 ;	Peephole 199	optimized misc jump sequence
   08C4 BA 00 1C           3441 	cjne	r2,#0x00,00106$
                           3442 ;00114$:
                           3443 ;	Peephole 200	removed redundant sjmp
   08C7                    3444 00115$:
                           3445 ;src/ddc_regs.c:515: DDC0_AMR = (RDEC & 0xFF00) >> 8;
                           3446 ;     genAssign
   08C7 90 60 07           3447 	mov	dptr,#_DDC0_AMR
   08CA 74 03              3448 	mov	a,#0x03
   08CC F0                 3449 	movx	@dptr,a
                           3450 ;src/ddc_regs.c:516: DDC0_LAR = (RDEC & 0xFF);
                           3451 ;     genAssign
   08CD 90 60 06           3452 	mov	dptr,#_DDC0_LAR
   08D0 74 0A              3453 	mov	a,#0x0A
   08D2 F0                 3454 	movx	@dptr,a
                           3455 ;src/ddc_regs.c:517: DDC0_DR0 = (buf[0] & 0xFF);
                           3456 ;     genAssign
   08D3 85 2B 82           3457 	mov	dpl,_set_RDEC_PARM_2
   08D6 85 2C 83           3458 	mov	dph,(_set_RDEC_PARM_2 + 1)
                           3459 ;     genPointerGet
                           3460 ;     genFarPointerGet
   08D9 E0                 3461 	movx	a,@dptr
                           3462 ;     genAssign
                           3463 ;	Peephole 100	removed redundant mov
   08DA FB                 3464 	mov	r3,a
   08DB 90 60 00           3465 	mov	dptr,#_DDC0_DR0
   08DE F0                 3466 	movx	@dptr,a
                           3467 ;src/ddc_regs.c:518: return 1;
                           3468 ;     genRet
   08DF 75 82 01           3469 	mov	dpl,#0x01
                           3470 ;	Peephole 112.b	changed ljmp to sjmp
                           3471 ;	Peephole 251.b	replaced sjmp to ret with ret
   08E2 22                 3472 	ret
   08E3                    3473 00106$:
                           3474 ;src/ddc_regs.c:519: } else if (index == 1) {
                           3475 ;     genCmpEq
                           3476 ;	Peephole 112.b	changed ljmp to sjmp
                           3477 ;	Peephole 199	optimized misc jump sequence
   08E3 BA 01 1C           3478 	cjne	r2,#0x01,00107$
                           3479 ;00116$:
                           3480 ;	Peephole 200	removed redundant sjmp
   08E6                    3481 00117$:
                           3482 ;src/ddc_regs.c:520: DDC1_AMR = (RDEC & 0xFF00) >> 8;
                           3483 ;     genAssign
   08E6 90 60 17           3484 	mov	dptr,#_DDC1_AMR
   08E9 74 03              3485 	mov	a,#0x03
   08EB F0                 3486 	movx	@dptr,a
                           3487 ;src/ddc_regs.c:521: DDC1_LAR = (RDEC & 0xFF);
                           3488 ;     genAssign
   08EC 90 60 16           3489 	mov	dptr,#_DDC1_LAR
   08EF 74 0A              3490 	mov	a,#0x0A
   08F1 F0                 3491 	movx	@dptr,a
                           3492 ;src/ddc_regs.c:522: DDC1_DR0 = (buf[0] & 0xFF);
                           3493 ;     genAssign
   08F2 85 2B 82           3494 	mov	dpl,_set_RDEC_PARM_2
   08F5 85 2C 83           3495 	mov	dph,(_set_RDEC_PARM_2 + 1)
                           3496 ;     genPointerGet
                           3497 ;     genFarPointerGet
   08F8 E0                 3498 	movx	a,@dptr
                           3499 ;     genAssign
                           3500 ;	Peephole 100	removed redundant mov
   08F9 FA                 3501 	mov	r2,a
   08FA 90 60 10           3502 	mov	dptr,#_DDC1_DR0
   08FD F0                 3503 	movx	@dptr,a
                           3504 ;src/ddc_regs.c:523: return 1;
                           3505 ;     genRet
   08FE 75 82 01           3506 	mov	dpl,#0x01
                           3507 ;	Peephole 112.b	changed ljmp to sjmp
                           3508 ;src/ddc_regs.c:525: return 0;
                           3509 ;     genRet
                           3510 ;	Peephole 237.a	removed sjmp to ret
   0901 22                 3511 	ret
   0902                    3512 00107$:
   0902 75 82 00           3513 	mov	dpl,#0x00
   0905                    3514 00108$:
   0905 22                 3515 	ret
                           3516 ;------------------------------------------------------------
                           3517 ;Allocation info for local variables in function 'read_RAO'
                           3518 ;------------------------------------------------------------
                           3519 ;buf                       Allocated with name '_read_RAO_PARM_2'
                           3520 ;len                       Allocated with name '_read_RAO_PARM_3'
                           3521 ;index                     Allocated to registers r2 
                           3522 ;------------------------------------------------------------
                           3523 ;src/ddc_regs.c:530: read_RAO (unsigned char index, xdata unsigned char *buf,
                           3524 ;	-----------------------------------------
                           3525 ;	 function read_RAO
                           3526 ;	-----------------------------------------
   0906                    3527 _read_RAO:
                           3528 ;     genReceive
   0906 AA 82              3529 	mov	r2,dpl
                           3530 ;src/ddc_regs.c:533: if (len < 1) return 0; // 8 bits
                           3531 ;     genCmpLt
                           3532 ;     genCmp
                           3533 ;     genIfxJump
                           3534 ;	Peephole 108	removed ljmp by inverse jump logic
                           3535 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0908 74 FF              3536 	mov	a,#0x100 - 0x01
   090A 25 2D              3537 	add	a,_read_RAO_PARM_3
   090C 40 04              3538 	jc	00102$
   090E                    3539 00113$:
                           3540 ;     genRet
   090E 75 82 00           3541 	mov	dpl,#0x00
                           3542 ;	Peephole 112.b	changed ljmp to sjmp
                           3543 ;	Peephole 251.b	replaced sjmp to ret with ret
   0911 22                 3544 	ret
   0912                    3545 00102$:
                           3546 ;src/ddc_regs.c:535: if (index == 0)
                           3547 ;     genCmpEq
                           3548 ;	Peephole 112.b	changed ljmp to sjmp
                           3549 ;	Peephole 199	optimized misc jump sequence
   0912 BA 00 14           3550 	cjne	r2,#0x00,00106$
                           3551 ;00114$:
                           3552 ;	Peephole 200	removed redundant sjmp
   0915                    3553 00115$:
                           3554 ;src/ddc_regs.c:537: DDC0_AMR = (RAO & 0xFF00) >> 8;
                           3555 ;     genAssign
   0915 90 60 07           3556 	mov	dptr,#_DDC0_AMR
   0918 74 03              3557 	mov	a,#0x03
   091A F0                 3558 	movx	@dptr,a
                           3559 ;src/ddc_regs.c:538: DDC0_LAR = (RAO & 0xFF);
                           3560 ;     genAssign
   091B 90 60 06           3561 	mov	dptr,#_DDC0_LAR
   091E 74 0B              3562 	mov	a,#0x0B
   0920 F0                 3563 	movx	@dptr,a
                           3564 ;src/ddc_regs.c:539: buf[0] == DDC0_DR0;
                           3565 ;     genDummyRead
   0921 90 60 00           3566 	mov	dptr,#_DDC0_DR0
   0924 E0                 3567 	movx	a,@dptr
                           3568 ;src/ddc_regs.c:540: return 1;
                           3569 ;     genRet
   0925 75 82 01           3570 	mov	dpl,#0x01
                           3571 ;	Peephole 112.b	changed ljmp to sjmp
                           3572 ;	Peephole 251.b	replaced sjmp to ret with ret
   0928 22                 3573 	ret
   0929                    3574 00106$:
                           3575 ;src/ddc_regs.c:541: } else if (index == 1) {
                           3576 ;     genCmpEq
                           3577 ;	Peephole 112.b	changed ljmp to sjmp
                           3578 ;	Peephole 199	optimized misc jump sequence
   0929 BA 01 14           3579 	cjne	r2,#0x01,00107$
                           3580 ;00116$:
                           3581 ;	Peephole 200	removed redundant sjmp
   092C                    3582 00117$:
                           3583 ;src/ddc_regs.c:542: DDC1_AMR = (RAO & 0xFF00) >> 8;
                           3584 ;     genAssign
   092C 90 60 17           3585 	mov	dptr,#_DDC1_AMR
   092F 74 03              3586 	mov	a,#0x03
   0931 F0                 3587 	movx	@dptr,a
                           3588 ;src/ddc_regs.c:543: DDC1_LAR = (RAO & 0xFF);
                           3589 ;     genAssign
   0932 90 60 16           3590 	mov	dptr,#_DDC1_LAR
   0935 74 0B              3591 	mov	a,#0x0B
   0937 F0                 3592 	movx	@dptr,a
                           3593 ;src/ddc_regs.c:544: buf[0] == DDC1_DR0;
                           3594 ;     genDummyRead
   0938 90 60 10           3595 	mov	dptr,#_DDC1_DR0
   093B E0                 3596 	movx	a,@dptr
                           3597 ;src/ddc_regs.c:545: return 1;
                           3598 ;     genRet
   093C 75 82 01           3599 	mov	dpl,#0x01
                           3600 ;	Peephole 112.b	changed ljmp to sjmp
                           3601 ;src/ddc_regs.c:547: return 0;
                           3602 ;     genRet
                           3603 ;	Peephole 237.a	removed sjmp to ret
   093F 22                 3604 	ret
   0940                    3605 00107$:
   0940 75 82 00           3606 	mov	dpl,#0x00
   0943                    3607 00108$:
   0943 22                 3608 	ret
                           3609 ;------------------------------------------------------------
                           3610 ;Allocation info for local variables in function 'set_RAO'
                           3611 ;------------------------------------------------------------
                           3612 ;buf                       Allocated with name '_set_RAO_PARM_2'
                           3613 ;len                       Allocated with name '_set_RAO_PARM_3'
                           3614 ;index                     Allocated to registers r2 
                           3615 ;------------------------------------------------------------
                           3616 ;src/ddc_regs.c:552: set_RAO (unsigned char index, xdata unsigned char *buf,
                           3617 ;	-----------------------------------------
                           3618 ;	 function set_RAO
                           3619 ;	-----------------------------------------
   0944                    3620 _set_RAO:
                           3621 ;     genReceive
   0944 AA 82              3622 	mov	r2,dpl
                           3623 ;src/ddc_regs.c:555: if (len < 1) return 0; // 8 bits
                           3624 ;     genCmpLt
                           3625 ;     genCmp
                           3626 ;     genIfxJump
                           3627 ;	Peephole 108	removed ljmp by inverse jump logic
                           3628 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0946 74 FF              3629 	mov	a,#0x100 - 0x01
   0948 25 2D              3630 	add	a,_set_RAO_PARM_3
   094A 40 04              3631 	jc	00102$
   094C                    3632 00113$:
                           3633 ;     genRet
   094C 75 82 00           3634 	mov	dpl,#0x00
                           3635 ;	Peephole 112.b	changed ljmp to sjmp
                           3636 ;	Peephole 251.b	replaced sjmp to ret with ret
   094F 22                 3637 	ret
   0950                    3638 00102$:
                           3639 ;src/ddc_regs.c:557: if (index == 0)
                           3640 ;     genCmpEq
                           3641 ;	Peephole 112.b	changed ljmp to sjmp
                           3642 ;	Peephole 199	optimized misc jump sequence
   0950 BA 00 1C           3643 	cjne	r2,#0x00,00106$
                           3644 ;00114$:
                           3645 ;	Peephole 200	removed redundant sjmp
   0953                    3646 00115$:
                           3647 ;src/ddc_regs.c:559: DDC0_AMR = (RAO & 0xFF00) >> 8;
                           3648 ;     genAssign
   0953 90 60 07           3649 	mov	dptr,#_DDC0_AMR
   0956 74 03              3650 	mov	a,#0x03
   0958 F0                 3651 	movx	@dptr,a
                           3652 ;src/ddc_regs.c:560: DDC0_LAR = (RAO & 0xFF);
                           3653 ;     genAssign
   0959 90 60 06           3654 	mov	dptr,#_DDC0_LAR
   095C 74 0B              3655 	mov	a,#0x0B
   095E F0                 3656 	movx	@dptr,a
                           3657 ;src/ddc_regs.c:561: DDC0_DR0 = (buf[0] & 0xFF);
                           3658 ;     genAssign
   095F 85 2B 82           3659 	mov	dpl,_set_RAO_PARM_2
   0962 85 2C 83           3660 	mov	dph,(_set_RAO_PARM_2 + 1)
                           3661 ;     genPointerGet
                           3662 ;     genFarPointerGet
   0965 E0                 3663 	movx	a,@dptr
                           3664 ;     genAssign
                           3665 ;	Peephole 100	removed redundant mov
   0966 FB                 3666 	mov	r3,a
   0967 90 60 00           3667 	mov	dptr,#_DDC0_DR0
   096A F0                 3668 	movx	@dptr,a
                           3669 ;src/ddc_regs.c:562: return 1;
                           3670 ;     genRet
   096B 75 82 01           3671 	mov	dpl,#0x01
                           3672 ;	Peephole 112.b	changed ljmp to sjmp
                           3673 ;	Peephole 251.b	replaced sjmp to ret with ret
   096E 22                 3674 	ret
   096F                    3675 00106$:
                           3676 ;src/ddc_regs.c:563: } else if (index == 1) {
                           3677 ;     genCmpEq
                           3678 ;	Peephole 112.b	changed ljmp to sjmp
                           3679 ;	Peephole 199	optimized misc jump sequence
   096F BA 01 1C           3680 	cjne	r2,#0x01,00107$
                           3681 ;00116$:
                           3682 ;	Peephole 200	removed redundant sjmp
   0972                    3683 00117$:
                           3684 ;src/ddc_regs.c:564: DDC1_AMR = (RAO & 0xFF00) >> 8;
                           3685 ;     genAssign
   0972 90 60 17           3686 	mov	dptr,#_DDC1_AMR
   0975 74 03              3687 	mov	a,#0x03
   0977 F0                 3688 	movx	@dptr,a
                           3689 ;src/ddc_regs.c:565: DDC1_LAR = (RAO & 0xFF);
                           3690 ;     genAssign
   0978 90 60 16           3691 	mov	dptr,#_DDC1_LAR
   097B 74 0B              3692 	mov	a,#0x0B
   097D F0                 3693 	movx	@dptr,a
                           3694 ;src/ddc_regs.c:566: DDC1_DR0 = (buf[0] & 0xFF);
                           3695 ;     genAssign
   097E 85 2B 82           3696 	mov	dpl,_set_RAO_PARM_2
   0981 85 2C 83           3697 	mov	dph,(_set_RAO_PARM_2 + 1)
                           3698 ;     genPointerGet
                           3699 ;     genFarPointerGet
   0984 E0                 3700 	movx	a,@dptr
                           3701 ;     genAssign
                           3702 ;	Peephole 100	removed redundant mov
   0985 FA                 3703 	mov	r2,a
   0986 90 60 10           3704 	mov	dptr,#_DDC1_DR0
   0989 F0                 3705 	movx	@dptr,a
                           3706 ;src/ddc_regs.c:567: return 1;
                           3707 ;     genRet
   098A 75 82 01           3708 	mov	dpl,#0x01
                           3709 ;	Peephole 112.b	changed ljmp to sjmp
                           3710 ;src/ddc_regs.c:569: return 0;
                           3711 ;     genRet
                           3712 ;	Peephole 237.a	removed sjmp to ret
   098D 22                 3713 	ret
   098E                    3714 00107$:
   098E 75 82 00           3715 	mov	dpl,#0x00
   0991                    3716 00108$:
   0991 22                 3717 	ret
                           3718 ;------------------------------------------------------------
                           3719 ;Allocation info for local variables in function 'read_RTAP'
                           3720 ;------------------------------------------------------------
                           3721 ;buf                       Allocated with name '_read_RTAP_PARM_2'
                           3722 ;len                       Allocated with name '_read_RTAP_PARM_3'
                           3723 ;index                     Allocated to registers r2 
                           3724 ;------------------------------------------------------------
                           3725 ;src/ddc_regs.c:574: read_RTAP (unsigned char index, xdata unsigned char *buf,
                           3726 ;	-----------------------------------------
                           3727 ;	 function read_RTAP
                           3728 ;	-----------------------------------------
   0992                    3729 _read_RTAP:
                           3730 ;     genReceive
   0992 AA 82              3731 	mov	r2,dpl
                           3732 ;src/ddc_regs.c:577: if (len < 1) return 0; // 8 bits
                           3733 ;     genCmpLt
                           3734 ;     genCmp
                           3735 ;     genIfxJump
                           3736 ;	Peephole 108	removed ljmp by inverse jump logic
                           3737 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0994 74 FF              3738 	mov	a,#0x100 - 0x01
   0996 25 2D              3739 	add	a,_read_RTAP_PARM_3
   0998 40 04              3740 	jc	00102$
   099A                    3741 00113$:
                           3742 ;     genRet
   099A 75 82 00           3743 	mov	dpl,#0x00
                           3744 ;	Peephole 112.b	changed ljmp to sjmp
                           3745 ;	Peephole 251.b	replaced sjmp to ret with ret
   099D 22                 3746 	ret
   099E                    3747 00102$:
                           3748 ;src/ddc_regs.c:579: if (index == 0)
                           3749 ;     genCmpEq
                           3750 ;	Peephole 112.b	changed ljmp to sjmp
                           3751 ;	Peephole 199	optimized misc jump sequence
   099E BA 00 14           3752 	cjne	r2,#0x00,00106$
                           3753 ;00114$:
                           3754 ;	Peephole 200	removed redundant sjmp
   09A1                    3755 00115$:
                           3756 ;src/ddc_regs.c:581: DDC0_AMR = (RTAP & 0xFF00) >> 8;
                           3757 ;     genAssign
   09A1 90 60 07           3758 	mov	dptr,#_DDC0_AMR
   09A4 74 03              3759 	mov	a,#0x03
   09A6 F0                 3760 	movx	@dptr,a
                           3761 ;src/ddc_regs.c:582: DDC0_LAR = (RTAP & 0xFF);
                           3762 ;     genAssign
   09A7 90 60 06           3763 	mov	dptr,#_DDC0_LAR
   09AA 74 0C              3764 	mov	a,#0x0C
   09AC F0                 3765 	movx	@dptr,a
                           3766 ;src/ddc_regs.c:583: buf[0] == DDC0_DR0;
                           3767 ;     genDummyRead
   09AD 90 60 00           3768 	mov	dptr,#_DDC0_DR0
   09B0 E0                 3769 	movx	a,@dptr
                           3770 ;src/ddc_regs.c:584: return 1;
                           3771 ;     genRet
   09B1 75 82 01           3772 	mov	dpl,#0x01
                           3773 ;	Peephole 112.b	changed ljmp to sjmp
                           3774 ;	Peephole 251.b	replaced sjmp to ret with ret
   09B4 22                 3775 	ret
   09B5                    3776 00106$:
                           3777 ;src/ddc_regs.c:585: } else if (index == 1) {
                           3778 ;     genCmpEq
                           3779 ;	Peephole 112.b	changed ljmp to sjmp
                           3780 ;	Peephole 199	optimized misc jump sequence
   09B5 BA 01 14           3781 	cjne	r2,#0x01,00107$
                           3782 ;00116$:
                           3783 ;	Peephole 200	removed redundant sjmp
   09B8                    3784 00117$:
                           3785 ;src/ddc_regs.c:586: DDC1_AMR = (RTAP & 0xFF00) >> 8;
                           3786 ;     genAssign
   09B8 90 60 17           3787 	mov	dptr,#_DDC1_AMR
   09BB 74 03              3788 	mov	a,#0x03
   09BD F0                 3789 	movx	@dptr,a
                           3790 ;src/ddc_regs.c:587: DDC1_LAR = (RTAP & 0xFF);
                           3791 ;     genAssign
   09BE 90 60 16           3792 	mov	dptr,#_DDC1_LAR
   09C1 74 0C              3793 	mov	a,#0x0C
   09C3 F0                 3794 	movx	@dptr,a
                           3795 ;src/ddc_regs.c:588: buf[0] == DDC1_DR0;
                           3796 ;     genDummyRead
   09C4 90 60 10           3797 	mov	dptr,#_DDC1_DR0
   09C7 E0                 3798 	movx	a,@dptr
                           3799 ;src/ddc_regs.c:589: return 1;
                           3800 ;     genRet
   09C8 75 82 01           3801 	mov	dpl,#0x01
                           3802 ;	Peephole 112.b	changed ljmp to sjmp
                           3803 ;src/ddc_regs.c:591: return 0;
                           3804 ;     genRet
                           3805 ;	Peephole 237.a	removed sjmp to ret
   09CB 22                 3806 	ret
   09CC                    3807 00107$:
   09CC 75 82 00           3808 	mov	dpl,#0x00
   09CF                    3809 00108$:
   09CF 22                 3810 	ret
                           3811 ;------------------------------------------------------------
                           3812 ;Allocation info for local variables in function 'set_RTAP'
                           3813 ;------------------------------------------------------------
                           3814 ;buf                       Allocated with name '_set_RTAP_PARM_2'
                           3815 ;len                       Allocated with name '_set_RTAP_PARM_3'
                           3816 ;index                     Allocated to registers r2 
                           3817 ;------------------------------------------------------------
                           3818 ;src/ddc_regs.c:596: set_RTAP (unsigned char index, xdata unsigned char *buf,
                           3819 ;	-----------------------------------------
                           3820 ;	 function set_RTAP
                           3821 ;	-----------------------------------------
   09D0                    3822 _set_RTAP:
                           3823 ;     genReceive
   09D0 AA 82              3824 	mov	r2,dpl
                           3825 ;src/ddc_regs.c:599: if (len < 1) return 0; // 8 bits
                           3826 ;     genCmpLt
                           3827 ;     genCmp
                           3828 ;     genIfxJump
                           3829 ;	Peephole 108	removed ljmp by inverse jump logic
                           3830 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   09D2 74 FF              3831 	mov	a,#0x100 - 0x01
   09D4 25 2D              3832 	add	a,_set_RTAP_PARM_3
   09D6 40 04              3833 	jc	00102$
   09D8                    3834 00113$:
                           3835 ;     genRet
   09D8 75 82 00           3836 	mov	dpl,#0x00
                           3837 ;	Peephole 112.b	changed ljmp to sjmp
                           3838 ;	Peephole 251.b	replaced sjmp to ret with ret
   09DB 22                 3839 	ret
   09DC                    3840 00102$:
                           3841 ;src/ddc_regs.c:601: if (index == 0)
                           3842 ;     genCmpEq
                           3843 ;	Peephole 112.b	changed ljmp to sjmp
                           3844 ;	Peephole 199	optimized misc jump sequence
   09DC BA 00 1C           3845 	cjne	r2,#0x00,00106$
                           3846 ;00114$:
                           3847 ;	Peephole 200	removed redundant sjmp
   09DF                    3848 00115$:
                           3849 ;src/ddc_regs.c:603: DDC0_AMR = (RTAP & 0xFF00) >> 8;
                           3850 ;     genAssign
   09DF 90 60 07           3851 	mov	dptr,#_DDC0_AMR
   09E2 74 03              3852 	mov	a,#0x03
   09E4 F0                 3853 	movx	@dptr,a
                           3854 ;src/ddc_regs.c:604: DDC0_LAR = (RTAP & 0xFF);
                           3855 ;     genAssign
   09E5 90 60 06           3856 	mov	dptr,#_DDC0_LAR
   09E8 74 0C              3857 	mov	a,#0x0C
   09EA F0                 3858 	movx	@dptr,a
                           3859 ;src/ddc_regs.c:605: DDC0_DR0 = (buf[0] & 0xFF);
                           3860 ;     genAssign
   09EB 85 2B 82           3861 	mov	dpl,_set_RTAP_PARM_2
   09EE 85 2C 83           3862 	mov	dph,(_set_RTAP_PARM_2 + 1)
                           3863 ;     genPointerGet
                           3864 ;     genFarPointerGet
   09F1 E0                 3865 	movx	a,@dptr
                           3866 ;     genAssign
                           3867 ;	Peephole 100	removed redundant mov
   09F2 FB                 3868 	mov	r3,a
   09F3 90 60 00           3869 	mov	dptr,#_DDC0_DR0
   09F6 F0                 3870 	movx	@dptr,a
                           3871 ;src/ddc_regs.c:606: return 1;
                           3872 ;     genRet
   09F7 75 82 01           3873 	mov	dpl,#0x01
                           3874 ;	Peephole 112.b	changed ljmp to sjmp
                           3875 ;	Peephole 251.b	replaced sjmp to ret with ret
   09FA 22                 3876 	ret
   09FB                    3877 00106$:
                           3878 ;src/ddc_regs.c:607: } else if (index == 1) {
                           3879 ;     genCmpEq
                           3880 ;	Peephole 112.b	changed ljmp to sjmp
                           3881 ;	Peephole 199	optimized misc jump sequence
   09FB BA 01 1C           3882 	cjne	r2,#0x01,00107$
                           3883 ;00116$:
                           3884 ;	Peephole 200	removed redundant sjmp
   09FE                    3885 00117$:
                           3886 ;src/ddc_regs.c:608: DDC1_AMR = (RTAP & 0xFF00) >> 8;
                           3887 ;     genAssign
   09FE 90 60 17           3888 	mov	dptr,#_DDC1_AMR
   0A01 74 03              3889 	mov	a,#0x03
   0A03 F0                 3890 	movx	@dptr,a
                           3891 ;src/ddc_regs.c:609: DDC1_LAR = (RTAP & 0xFF);
                           3892 ;     genAssign
   0A04 90 60 16           3893 	mov	dptr,#_DDC1_LAR
   0A07 74 0C              3894 	mov	a,#0x0C
   0A09 F0                 3895 	movx	@dptr,a
                           3896 ;src/ddc_regs.c:610: DDC1_DR0 = (buf[0] & 0xFF);
                           3897 ;     genAssign
   0A0A 85 2B 82           3898 	mov	dpl,_set_RTAP_PARM_2
   0A0D 85 2C 83           3899 	mov	dph,(_set_RTAP_PARM_2 + 1)
                           3900 ;     genPointerGet
                           3901 ;     genFarPointerGet
   0A10 E0                 3902 	movx	a,@dptr
                           3903 ;     genAssign
                           3904 ;	Peephole 100	removed redundant mov
   0A11 FA                 3905 	mov	r2,a
   0A12 90 60 10           3906 	mov	dptr,#_DDC1_DR0
   0A15 F0                 3907 	movx	@dptr,a
                           3908 ;src/ddc_regs.c:611: return 1;
                           3909 ;     genRet
   0A16 75 82 01           3910 	mov	dpl,#0x01
                           3911 ;	Peephole 112.b	changed ljmp to sjmp
                           3912 ;src/ddc_regs.c:613: return 0;
                           3913 ;     genRet
                           3914 ;	Peephole 237.a	removed sjmp to ret
   0A19 22                 3915 	ret
   0A1A                    3916 00107$:
   0A1A 75 82 00           3917 	mov	dpl,#0x00
   0A1D                    3918 00108$:
   0A1D 22                 3919 	ret
                           3920 ;------------------------------------------------------------
                           3921 ;Allocation info for local variables in function 'read_RSRV'
                           3922 ;------------------------------------------------------------
                           3923 ;buf                       Allocated with name '_read_RSRV_PARM_2'
                           3924 ;len                       Allocated with name '_read_RSRV_PARM_3'
                           3925 ;index                     Allocated to registers r2 
                           3926 ;------------------------------------------------------------
                           3927 ;src/ddc_regs.c:618: read_RSRV (unsigned char index, xdata unsigned char *buf,
                           3928 ;	-----------------------------------------
                           3929 ;	 function read_RSRV
                           3930 ;	-----------------------------------------
   0A1E                    3931 _read_RSRV:
                           3932 ;     genReceive
   0A1E AA 82              3933 	mov	r2,dpl
                           3934 ;src/ddc_regs.c:621: if (len < 1) return 0; // 8 bits
                           3935 ;     genCmpLt
                           3936 ;     genCmp
                           3937 ;     genIfxJump
                           3938 ;	Peephole 108	removed ljmp by inverse jump logic
                           3939 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0A20 74 FF              3940 	mov	a,#0x100 - 0x01
   0A22 25 2D              3941 	add	a,_read_RSRV_PARM_3
   0A24 40 04              3942 	jc	00102$
   0A26                    3943 00113$:
                           3944 ;     genRet
   0A26 75 82 00           3945 	mov	dpl,#0x00
                           3946 ;	Peephole 112.b	changed ljmp to sjmp
                           3947 ;	Peephole 251.b	replaced sjmp to ret with ret
   0A29 22                 3948 	ret
   0A2A                    3949 00102$:
                           3950 ;src/ddc_regs.c:623: if (index == 0)
                           3951 ;     genCmpEq
                           3952 ;	Peephole 112.b	changed ljmp to sjmp
                           3953 ;	Peephole 199	optimized misc jump sequence
   0A2A BA 00 14           3954 	cjne	r2,#0x00,00106$
                           3955 ;00114$:
                           3956 ;	Peephole 200	removed redundant sjmp
   0A2D                    3957 00115$:
                           3958 ;src/ddc_regs.c:625: DDC0_AMR = (RSRV & 0xFF00) >> 8;
                           3959 ;     genAssign
   0A2D 90 60 07           3960 	mov	dptr,#_DDC0_AMR
   0A30 74 03              3961 	mov	a,#0x03
   0A32 F0                 3962 	movx	@dptr,a
                           3963 ;src/ddc_regs.c:626: DDC0_LAR = (RSRV & 0xFF);
                           3964 ;     genAssign
   0A33 90 60 06           3965 	mov	dptr,#_DDC0_LAR
   0A36 74 0D              3966 	mov	a,#0x0D
   0A38 F0                 3967 	movx	@dptr,a
                           3968 ;src/ddc_regs.c:627: buf[0] == DDC0_DR0;
                           3969 ;     genDummyRead
   0A39 90 60 00           3970 	mov	dptr,#_DDC0_DR0
   0A3C E0                 3971 	movx	a,@dptr
                           3972 ;src/ddc_regs.c:628: return 1;
                           3973 ;     genRet
   0A3D 75 82 01           3974 	mov	dpl,#0x01
                           3975 ;	Peephole 112.b	changed ljmp to sjmp
                           3976 ;	Peephole 251.b	replaced sjmp to ret with ret
   0A40 22                 3977 	ret
   0A41                    3978 00106$:
                           3979 ;src/ddc_regs.c:629: } else if (index == 1) {
                           3980 ;     genCmpEq
                           3981 ;	Peephole 112.b	changed ljmp to sjmp
                           3982 ;	Peephole 199	optimized misc jump sequence
   0A41 BA 01 14           3983 	cjne	r2,#0x01,00107$
                           3984 ;00116$:
                           3985 ;	Peephole 200	removed redundant sjmp
   0A44                    3986 00117$:
                           3987 ;src/ddc_regs.c:630: DDC1_AMR = (RSRV & 0xFF00) >> 8;
                           3988 ;     genAssign
   0A44 90 60 17           3989 	mov	dptr,#_DDC1_AMR
   0A47 74 03              3990 	mov	a,#0x03
   0A49 F0                 3991 	movx	@dptr,a
                           3992 ;src/ddc_regs.c:631: DDC1_LAR = (RSRV & 0xFF);
                           3993 ;     genAssign
   0A4A 90 60 16           3994 	mov	dptr,#_DDC1_LAR
   0A4D 74 0D              3995 	mov	a,#0x0D
   0A4F F0                 3996 	movx	@dptr,a
                           3997 ;src/ddc_regs.c:632: buf[0] == DDC1_DR0;
                           3998 ;     genDummyRead
   0A50 90 60 10           3999 	mov	dptr,#_DDC1_DR0
   0A53 E0                 4000 	movx	a,@dptr
                           4001 ;src/ddc_regs.c:633: return 1;
                           4002 ;     genRet
   0A54 75 82 01           4003 	mov	dpl,#0x01
                           4004 ;	Peephole 112.b	changed ljmp to sjmp
                           4005 ;src/ddc_regs.c:635: return 0;
                           4006 ;     genRet
                           4007 ;	Peephole 237.a	removed sjmp to ret
   0A57 22                 4008 	ret
   0A58                    4009 00107$:
   0A58 75 82 00           4010 	mov	dpl,#0x00
   0A5B                    4011 00108$:
   0A5B 22                 4012 	ret
                           4013 ;------------------------------------------------------------
                           4014 ;Allocation info for local variables in function 'set_RSRV'
                           4015 ;------------------------------------------------------------
                           4016 ;buf                       Allocated with name '_set_RSRV_PARM_2'
                           4017 ;len                       Allocated with name '_set_RSRV_PARM_3'
                           4018 ;index                     Allocated to registers r2 
                           4019 ;------------------------------------------------------------
                           4020 ;src/ddc_regs.c:640: set_RSRV (unsigned char index, xdata unsigned char *buf,
                           4021 ;	-----------------------------------------
                           4022 ;	 function set_RSRV
                           4023 ;	-----------------------------------------
   0A5C                    4024 _set_RSRV:
                           4025 ;     genReceive
   0A5C AA 82              4026 	mov	r2,dpl
                           4027 ;src/ddc_regs.c:643: if (len < 1) return 0; // 8 bits
                           4028 ;     genCmpLt
                           4029 ;     genCmp
                           4030 ;     genIfxJump
                           4031 ;	Peephole 108	removed ljmp by inverse jump logic
                           4032 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0A5E 74 FF              4033 	mov	a,#0x100 - 0x01
   0A60 25 2D              4034 	add	a,_set_RSRV_PARM_3
   0A62 40 04              4035 	jc	00102$
   0A64                    4036 00113$:
                           4037 ;     genRet
   0A64 75 82 00           4038 	mov	dpl,#0x00
                           4039 ;	Peephole 112.b	changed ljmp to sjmp
                           4040 ;	Peephole 251.b	replaced sjmp to ret with ret
   0A67 22                 4041 	ret
   0A68                    4042 00102$:
                           4043 ;src/ddc_regs.c:645: if (index == 0)
                           4044 ;     genCmpEq
                           4045 ;	Peephole 112.b	changed ljmp to sjmp
                           4046 ;	Peephole 199	optimized misc jump sequence
   0A68 BA 00 1C           4047 	cjne	r2,#0x00,00106$
                           4048 ;00114$:
                           4049 ;	Peephole 200	removed redundant sjmp
   0A6B                    4050 00115$:
                           4051 ;src/ddc_regs.c:647: DDC0_AMR = (RSRV & 0xFF00) >> 8;
                           4052 ;     genAssign
   0A6B 90 60 07           4053 	mov	dptr,#_DDC0_AMR
   0A6E 74 03              4054 	mov	a,#0x03
   0A70 F0                 4055 	movx	@dptr,a
                           4056 ;src/ddc_regs.c:648: DDC0_LAR = (RSRV & 0xFF);
                           4057 ;     genAssign
   0A71 90 60 06           4058 	mov	dptr,#_DDC0_LAR
   0A74 74 0D              4059 	mov	a,#0x0D
   0A76 F0                 4060 	movx	@dptr,a
                           4061 ;src/ddc_regs.c:649: DDC0_DR0 = (buf[0] & 0xFF);
                           4062 ;     genAssign
   0A77 85 2B 82           4063 	mov	dpl,_set_RSRV_PARM_2
   0A7A 85 2C 83           4064 	mov	dph,(_set_RSRV_PARM_2 + 1)
                           4065 ;     genPointerGet
                           4066 ;     genFarPointerGet
   0A7D E0                 4067 	movx	a,@dptr
                           4068 ;     genAssign
                           4069 ;	Peephole 100	removed redundant mov
   0A7E FB                 4070 	mov	r3,a
   0A7F 90 60 00           4071 	mov	dptr,#_DDC0_DR0
   0A82 F0                 4072 	movx	@dptr,a
                           4073 ;src/ddc_regs.c:650: return 1;
                           4074 ;     genRet
   0A83 75 82 01           4075 	mov	dpl,#0x01
                           4076 ;	Peephole 112.b	changed ljmp to sjmp
                           4077 ;	Peephole 251.b	replaced sjmp to ret with ret
   0A86 22                 4078 	ret
   0A87                    4079 00106$:
                           4080 ;src/ddc_regs.c:651: } else if (index == 1) {
                           4081 ;     genCmpEq
                           4082 ;	Peephole 112.b	changed ljmp to sjmp
                           4083 ;	Peephole 199	optimized misc jump sequence
   0A87 BA 01 1C           4084 	cjne	r2,#0x01,00107$
                           4085 ;00116$:
                           4086 ;	Peephole 200	removed redundant sjmp
   0A8A                    4087 00117$:
                           4088 ;src/ddc_regs.c:652: DDC1_AMR = (RSRV & 0xFF00) >> 8;
                           4089 ;     genAssign
   0A8A 90 60 17           4090 	mov	dptr,#_DDC1_AMR
   0A8D 74 03              4091 	mov	a,#0x03
   0A8F F0                 4092 	movx	@dptr,a
                           4093 ;src/ddc_regs.c:653: DDC1_LAR = (RSRV & 0xFF);
                           4094 ;     genAssign
   0A90 90 60 16           4095 	mov	dptr,#_DDC1_LAR
   0A93 74 0D              4096 	mov	a,#0x0D
   0A95 F0                 4097 	movx	@dptr,a
                           4098 ;src/ddc_regs.c:654: DDC1_DR0 = (buf[0] & 0xFF);
                           4099 ;     genAssign
   0A96 85 2B 82           4100 	mov	dpl,_set_RSRV_PARM_2
   0A99 85 2C 83           4101 	mov	dph,(_set_RSRV_PARM_2 + 1)
                           4102 ;     genPointerGet
                           4103 ;     genFarPointerGet
   0A9C E0                 4104 	movx	a,@dptr
                           4105 ;     genAssign
                           4106 ;	Peephole 100	removed redundant mov
   0A9D FA                 4107 	mov	r2,a
   0A9E 90 60 10           4108 	mov	dptr,#_DDC1_DR0
   0AA1 F0                 4109 	movx	@dptr,a
                           4110 ;src/ddc_regs.c:655: return 1;
                           4111 ;     genRet
   0AA2 75 82 01           4112 	mov	dpl,#0x01
                           4113 ;	Peephole 112.b	changed ljmp to sjmp
                           4114 ;src/ddc_regs.c:657: return 0;
                           4115 ;     genRet
                           4116 ;	Peephole 237.a	removed sjmp to ret
   0AA5 22                 4117 	ret
   0AA6                    4118 00107$:
   0AA6 75 82 00           4119 	mov	dpl,#0x00
   0AA9                    4120 00108$:
   0AA9 22                 4121 	ret
                           4122 ;------------------------------------------------------------
                           4123 ;Allocation info for local variables in function 'clr_RAM'
                           4124 ;------------------------------------------------------------
                           4125 ;index                     Allocated to registers r2 
                           4126 ;i                         Allocated to registers r3 r4 
                           4127 ;------------------------------------------------------------
                           4128 ;src/ddc_regs.c:663: clr_RAM (unsigned char index)
                           4129 ;	-----------------------------------------
                           4130 ;	 function clr_RAM
                           4131 ;	-----------------------------------------
   0AAA                    4132 _clr_RAM:
                           4133 ;     genReceive
   0AAA AA 82              4134 	mov	r2,dpl
                           4135 ;src/ddc_regs.c:667: if (index == 0)  {
                           4136 ;     genCmpEq
                           4137 ;	Peephole 112.b	changed ljmp to sjmp
                           4138 ;	Peephole 199	optimized misc jump sequence
   0AAC BA 00 43           4139 	cjne	r2,#0x00,00104$
                           4140 ;00122$:
                           4141 ;	Peephole 200	removed redundant sjmp
   0AAF                    4142 00123$:
                           4143 ;src/ddc_regs.c:668: for (i=0; i < 0xFF; i++) {
                           4144 ;     genAssign
   0AAF 7B 00              4145 	mov	r3,#0x00
   0AB1 7C 00              4146 	mov	r4,#0x00
   0AB3                    4147 00106$:
                           4148 ;     genCmpLt
                           4149 ;     genCmp
   0AB3 C3                 4150 	clr	c
   0AB4 EB                 4151 	mov	a,r3
   0AB5 94 FF              4152 	subb	a,#0xFF
   0AB7 EC                 4153 	mov	a,r4
   0AB8 64 80              4154 	xrl	a,#0x80
   0ABA 94 80              4155 	subb	a,#0x80
                           4156 ;     genIfxJump
                           4157 ;	Peephole 108	removed ljmp by inverse jump logic
   0ABC 50 31              4158 	jnc	00109$
   0ABE                    4159 00124$:
                           4160 ;src/ddc_regs.c:669: DDC0_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
                           4161 ;     genPlus
                           4162 ;     genPlus shortcut
   0ABE 8B 05              4163 	mov	ar5,r3
   0AC0 74 01              4164 	mov	a,#0x01
                           4165 ;	Peephole 236.a	used r4 instead of ar4
   0AC2 2C                 4166 	add	a,r4
   0AC3 FE                 4167 	mov	r6,a
                           4168 ;     genAnd
                           4169 ;     genRightShift
                           4170 ;     genRightShiftLiteral
                           4171 ;     genrshTwo
                           4172 ;	peephole 177.e	removed redundant move
   0AC4 8E 05              4173 	mov	ar5,r6
   0AC6 7E 00              4174 	mov	r6,#0x00
                           4175 ;     genCast
   0AC8 90 60 07           4176 	mov	dptr,#_DDC0_AMR
   0ACB ED                 4177 	mov	a,r5
   0ACC F0                 4178 	movx	@dptr,a
                           4179 ;src/ddc_regs.c:670: DDC0_LAR = ((DDC_RAM + i) & 0xFF);
                           4180 ;     genCast
   0ACD 90 60 06           4181 	mov	dptr,#_DDC0_LAR
   0AD0 EB                 4182 	mov	a,r3
   0AD1 F0                 4183 	movx	@dptr,a
                           4184 ;src/ddc_regs.c:671: DDC0_DR4 = 0;
                           4185 ;     genAssign
   0AD2 90 60 04           4186 	mov	dptr,#_DDC0_DR4
                           4187 ;	Peephole 181	changed mov to clr
                           4188 ;src/ddc_regs.c:672: DDC0_DR3 = 0;
                           4189 ;     genAssign
                           4190 ;	Peephole 181	changed mov to clr
                           4191 ;	Peephole 219	removed redundant clear
                           4192 ;src/ddc_regs.c:673: DDC0_DR2 = 0;
                           4193 ;     genAssign
                           4194 ;	Peephole 181	changed mov to clr
                           4195 ;src/ddc_regs.c:674: DDC0_DR1 = 0;
                           4196 ;     genAssign
                           4197 ;	Peephole 181	changed mov to clr
                           4198 ;	Peephole 219	removed redundant clear
                           4199 ;	Peephole 219.a	removed redundant clear
   0AD5 E4                 4200 	clr	a
   0AD6 F0                 4201 	movx	@dptr,a
   0AD7 90 60 03           4202 	mov	dptr,#_DDC0_DR3
   0ADA F0                 4203 	movx	@dptr,a
   0ADB 90 60 02           4204 	mov	dptr,#_DDC0_DR2
   0ADE F0                 4205 	movx	@dptr,a
   0ADF 90 60 01           4206 	mov	dptr,#_DDC0_DR1
   0AE2 F0                 4207 	movx	@dptr,a
                           4208 ;src/ddc_regs.c:675: DDC0_DR0 = 0;
                           4209 ;     genAssign
   0AE3 90 60 00           4210 	mov	dptr,#_DDC0_DR0
                           4211 ;	Peephole 181	changed mov to clr
   0AE6 E4                 4212 	clr	a
   0AE7 F0                 4213 	movx	@dptr,a
                           4214 ;src/ddc_regs.c:668: for (i=0; i < 0xFF; i++) {
                           4215 ;     genPlus
                           4216 ;     genPlusIncr
   0AE8 0B                 4217 	inc	r3
                           4218 ;	Peephole 112.b	changed ljmp to sjmp
                           4219 ;	Peephole 243	avoided branch to sjmp
   0AE9 BB 00 C7           4220 	cjne	r3,#0x00,00106$
   0AEC 0C                 4221 	inc	r4
   0AED                    4222 00125$:
   0AED 80 C4              4223 	sjmp	00106$
   0AEF                    4224 00109$:
                           4225 ;src/ddc_regs.c:677: return (unsigned char)i;
                           4226 ;     genCast
   0AEF 8B 82              4227 	mov	dpl,r3
                           4228 ;     genRet
                           4229 ;	Peephole 112.b	changed ljmp to sjmp
                           4230 ;	Peephole 251.b	replaced sjmp to ret with ret
   0AF1 22                 4231 	ret
   0AF2                    4232 00104$:
                           4233 ;src/ddc_regs.c:679: else if (index == 1) {
                           4234 ;     genCmpEq
                           4235 ;	Peephole 112.b	changed ljmp to sjmp
                           4236 ;	Peephole 199	optimized misc jump sequence
   0AF2 BA 01 43           4237 	cjne	r2,#0x01,00105$
                           4238 ;00126$:
                           4239 ;	Peephole 200	removed redundant sjmp
   0AF5                    4240 00127$:
                           4241 ;src/ddc_regs.c:680: for (i=0; i < 0xFF; i++) {
                           4242 ;     genAssign
   0AF5 7A 00              4243 	mov	r2,#0x00
   0AF7 7B 00              4244 	mov	r3,#0x00
   0AF9                    4245 00110$:
                           4246 ;     genCmpLt
                           4247 ;     genCmp
   0AF9 C3                 4248 	clr	c
   0AFA EA                 4249 	mov	a,r2
   0AFB 94 FF              4250 	subb	a,#0xFF
   0AFD EB                 4251 	mov	a,r3
   0AFE 64 80              4252 	xrl	a,#0x80
   0B00 94 80              4253 	subb	a,#0x80
                           4254 ;     genIfxJump
                           4255 ;	Peephole 108	removed ljmp by inverse jump logic
   0B02 50 31              4256 	jnc	00113$
   0B04                    4257 00128$:
                           4258 ;src/ddc_regs.c:681: DDC1_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
                           4259 ;     genPlus
                           4260 ;     genPlus shortcut
   0B04 8A 04              4261 	mov	ar4,r2
   0B06 74 01              4262 	mov	a,#0x01
                           4263 ;	Peephole 236.a	used r3 instead of ar3
   0B08 2B                 4264 	add	a,r3
   0B09 FD                 4265 	mov	r5,a
                           4266 ;     genAnd
                           4267 ;     genRightShift
                           4268 ;     genRightShiftLiteral
                           4269 ;     genrshTwo
                           4270 ;	peephole 177.e	removed redundant move
   0B0A 8D 04              4271 	mov	ar4,r5
   0B0C 7D 00              4272 	mov	r5,#0x00
                           4273 ;     genCast
   0B0E 90 60 17           4274 	mov	dptr,#_DDC1_AMR
   0B11 EC                 4275 	mov	a,r4
   0B12 F0                 4276 	movx	@dptr,a
                           4277 ;src/ddc_regs.c:682: DDC1_LAR = ((DDC_RAM + i) & 0xFF);
                           4278 ;     genCast
   0B13 90 60 16           4279 	mov	dptr,#_DDC1_LAR
   0B16 EA                 4280 	mov	a,r2
   0B17 F0                 4281 	movx	@dptr,a
                           4282 ;src/ddc_regs.c:683: DDC1_DR4 = 0;
                           4283 ;     genAssign
   0B18 90 60 14           4284 	mov	dptr,#_DDC1_DR4
                           4285 ;	Peephole 181	changed mov to clr
                           4286 ;src/ddc_regs.c:684: DDC1_DR3 = 0;
                           4287 ;     genAssign
                           4288 ;	Peephole 181	changed mov to clr
                           4289 ;	Peephole 219	removed redundant clear
                           4290 ;src/ddc_regs.c:685: DDC1_DR2 = 0;
                           4291 ;     genAssign
                           4292 ;	Peephole 181	changed mov to clr
                           4293 ;src/ddc_regs.c:686: DDC1_DR1 = 0;
                           4294 ;     genAssign
                           4295 ;	Peephole 181	changed mov to clr
                           4296 ;	Peephole 219	removed redundant clear
                           4297 ;	Peephole 219.a	removed redundant clear
   0B1B E4                 4298 	clr	a
   0B1C F0                 4299 	movx	@dptr,a
   0B1D 90 60 13           4300 	mov	dptr,#_DDC1_DR3
   0B20 F0                 4301 	movx	@dptr,a
   0B21 90 60 12           4302 	mov	dptr,#_DDC1_DR2
   0B24 F0                 4303 	movx	@dptr,a
   0B25 90 60 11           4304 	mov	dptr,#_DDC1_DR1
   0B28 F0                 4305 	movx	@dptr,a
                           4306 ;src/ddc_regs.c:687: DDC1_DR0 = 0;
                           4307 ;     genAssign
   0B29 90 60 10           4308 	mov	dptr,#_DDC1_DR0
                           4309 ;	Peephole 181	changed mov to clr
   0B2C E4                 4310 	clr	a
   0B2D F0                 4311 	movx	@dptr,a
                           4312 ;src/ddc_regs.c:680: for (i=0; i < 0xFF; i++) {
                           4313 ;     genPlus
                           4314 ;     genPlusIncr
   0B2E 0A                 4315 	inc	r2
                           4316 ;	Peephole 112.b	changed ljmp to sjmp
                           4317 ;	Peephole 243	avoided branch to sjmp
   0B2F BA 00 C7           4318 	cjne	r2,#0x00,00110$
   0B32 0B                 4319 	inc	r3
   0B33                    4320 00129$:
   0B33 80 C4              4321 	sjmp	00110$
   0B35                    4322 00113$:
                           4323 ;src/ddc_regs.c:689: return (unsigned char)i;
                           4324 ;     genCast
   0B35 8A 82              4325 	mov	dpl,r2
                           4326 ;     genRet
                           4327 ;	Peephole 112.b	changed ljmp to sjmp
                           4328 ;src/ddc_regs.c:691: return 0;
                           4329 ;     genRet
                           4330 ;	Peephole 237.a	removed sjmp to ret
   0B37 22                 4331 	ret
   0B38                    4332 00105$:
   0B38 75 82 00           4333 	mov	dpl,#0x00
   0B3B                    4334 00114$:
   0B3B 22                 4335 	ret
                           4336 ;------------------------------------------------------------
                           4337 ;Allocation info for local variables in function 'clr_COEF'
                           4338 ;------------------------------------------------------------
                           4339 ;index                     Allocated to registers r2 
                           4340 ;i                         Allocated to registers r3 r4 
                           4341 ;------------------------------------------------------------
                           4342 ;src/ddc_regs.c:697: clr_COEF (unsigned char index)
                           4343 ;	-----------------------------------------
                           4344 ;	 function clr_COEF
                           4345 ;	-----------------------------------------
   0B3C                    4346 _clr_COEF:
                           4347 ;     genReceive
   0B3C AA 82              4348 	mov	r2,dpl
                           4349 ;src/ddc_regs.c:701: if (index == 0)  {
                           4350 ;     genCmpEq
                           4351 ;	Peephole 112.b	changed ljmp to sjmp
                           4352 ;	Peephole 199	optimized misc jump sequence
   0B3E BA 00 41           4353 	cjne	r2,#0x00,00104$
                           4354 ;00122$:
                           4355 ;	Peephole 200	removed redundant sjmp
   0B41                    4356 00123$:
                           4357 ;src/ddc_regs.c:702: for (i=0; i < 0xFF; i++) {
                           4358 ;     genAssign
   0B41 7B 00              4359 	mov	r3,#0x00
   0B43 7C 00              4360 	mov	r4,#0x00
   0B45                    4361 00106$:
                           4362 ;     genCmpLt
                           4363 ;     genCmp
   0B45 C3                 4364 	clr	c
   0B46 EB                 4365 	mov	a,r3
   0B47 94 FF              4366 	subb	a,#0xFF
   0B49 EC                 4367 	mov	a,r4
   0B4A 64 80              4368 	xrl	a,#0x80
   0B4C 94 80              4369 	subb	a,#0x80
                           4370 ;     genIfxJump
                           4371 ;	Peephole 108	removed ljmp by inverse jump logic
   0B4E 50 2F              4372 	jnc	00109$
   0B50                    4373 00124$:
                           4374 ;src/ddc_regs.c:703: DDC0_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
                           4375 ;     genAnd
   0B50 7D 00              4376 	mov	r5,#0x00
   0B52 8C 06              4377 	mov	ar6,r4
                           4378 ;     genRightShift
                           4379 ;     genRightShiftLiteral
                           4380 ;     genrshTwo
   0B54 8E 05              4381 	mov	ar5,r6
   0B56 7E 00              4382 	mov	r6,#0x00
                           4383 ;     genCast
   0B58 90 60 07           4384 	mov	dptr,#_DDC0_AMR
   0B5B ED                 4385 	mov	a,r5
   0B5C F0                 4386 	movx	@dptr,a
                           4387 ;src/ddc_regs.c:704: DDC0_LAR = ((DDC_COEF + i) & 0xFF);
                           4388 ;     genCast
   0B5D 90 60 06           4389 	mov	dptr,#_DDC0_LAR
   0B60 EB                 4390 	mov	a,r3
   0B61 F0                 4391 	movx	@dptr,a
                           4392 ;src/ddc_regs.c:705: DDC0_DR4 = 0;
                           4393 ;     genAssign
   0B62 90 60 04           4394 	mov	dptr,#_DDC0_DR4
                           4395 ;	Peephole 181	changed mov to clr
                           4396 ;src/ddc_regs.c:706: DDC0_DR3 = 0;
                           4397 ;     genAssign
                           4398 ;	Peephole 181	changed mov to clr
                           4399 ;	Peephole 219	removed redundant clear
                           4400 ;src/ddc_regs.c:707: DDC0_DR2 = 0;
                           4401 ;     genAssign
                           4402 ;	Peephole 181	changed mov to clr
                           4403 ;src/ddc_regs.c:708: DDC0_DR1 = 0;
                           4404 ;     genAssign
                           4405 ;	Peephole 181	changed mov to clr
                           4406 ;	Peephole 219	removed redundant clear
                           4407 ;	Peephole 219.a	removed redundant clear
   0B65 E4                 4408 	clr	a
   0B66 F0                 4409 	movx	@dptr,a
   0B67 90 60 03           4410 	mov	dptr,#_DDC0_DR3
   0B6A F0                 4411 	movx	@dptr,a
   0B6B 90 60 02           4412 	mov	dptr,#_DDC0_DR2
   0B6E F0                 4413 	movx	@dptr,a
   0B6F 90 60 01           4414 	mov	dptr,#_DDC0_DR1
   0B72 F0                 4415 	movx	@dptr,a
                           4416 ;src/ddc_regs.c:709: DDC0_DR0 = 0;
                           4417 ;     genAssign
   0B73 90 60 00           4418 	mov	dptr,#_DDC0_DR0
                           4419 ;	Peephole 181	changed mov to clr
   0B76 E4                 4420 	clr	a
   0B77 F0                 4421 	movx	@dptr,a
                           4422 ;src/ddc_regs.c:702: for (i=0; i < 0xFF; i++) {
                           4423 ;     genPlus
                           4424 ;     genPlusIncr
   0B78 0B                 4425 	inc	r3
                           4426 ;	Peephole 112.b	changed ljmp to sjmp
                           4427 ;	Peephole 243	avoided branch to sjmp
   0B79 BB 00 C9           4428 	cjne	r3,#0x00,00106$
   0B7C 0C                 4429 	inc	r4
   0B7D                    4430 00125$:
   0B7D 80 C6              4431 	sjmp	00106$
   0B7F                    4432 00109$:
                           4433 ;src/ddc_regs.c:711: return (unsigned char)i;
                           4434 ;     genCast
   0B7F 8B 82              4435 	mov	dpl,r3
                           4436 ;     genRet
                           4437 ;	Peephole 112.b	changed ljmp to sjmp
                           4438 ;	Peephole 251.b	replaced sjmp to ret with ret
   0B81 22                 4439 	ret
   0B82                    4440 00104$:
                           4441 ;src/ddc_regs.c:713: else if (index == 1) {
                           4442 ;     genCmpEq
                           4443 ;	Peephole 112.b	changed ljmp to sjmp
                           4444 ;	Peephole 199	optimized misc jump sequence
   0B82 BA 01 41           4445 	cjne	r2,#0x01,00105$
                           4446 ;00126$:
                           4447 ;	Peephole 200	removed redundant sjmp
   0B85                    4448 00127$:
                           4449 ;src/ddc_regs.c:714: for (i=0; i < 0xFF; i++) {
                           4450 ;     genAssign
   0B85 7A 00              4451 	mov	r2,#0x00
   0B87 7B 00              4452 	mov	r3,#0x00
   0B89                    4453 00110$:
                           4454 ;     genCmpLt
                           4455 ;     genCmp
   0B89 C3                 4456 	clr	c
   0B8A EA                 4457 	mov	a,r2
   0B8B 94 FF              4458 	subb	a,#0xFF
   0B8D EB                 4459 	mov	a,r3
   0B8E 64 80              4460 	xrl	a,#0x80
   0B90 94 80              4461 	subb	a,#0x80
                           4462 ;     genIfxJump
                           4463 ;	Peephole 108	removed ljmp by inverse jump logic
   0B92 50 2F              4464 	jnc	00113$
   0B94                    4465 00128$:
                           4466 ;src/ddc_regs.c:715: DDC1_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
                           4467 ;     genAnd
   0B94 7C 00              4468 	mov	r4,#0x00
   0B96 8B 05              4469 	mov	ar5,r3
                           4470 ;     genRightShift
                           4471 ;     genRightShiftLiteral
                           4472 ;     genrshTwo
   0B98 8D 04              4473 	mov	ar4,r5
   0B9A 7D 00              4474 	mov	r5,#0x00
                           4475 ;     genCast
   0B9C 90 60 17           4476 	mov	dptr,#_DDC1_AMR
   0B9F EC                 4477 	mov	a,r4
   0BA0 F0                 4478 	movx	@dptr,a
                           4479 ;src/ddc_regs.c:716: DDC1_LAR = ((DDC_COEF + i) & 0xFF);
                           4480 ;     genCast
   0BA1 90 60 16           4481 	mov	dptr,#_DDC1_LAR
   0BA4 EA                 4482 	mov	a,r2
   0BA5 F0                 4483 	movx	@dptr,a
                           4484 ;src/ddc_regs.c:717: DDC1_DR4 = 0;
                           4485 ;     genAssign
   0BA6 90 60 14           4486 	mov	dptr,#_DDC1_DR4
                           4487 ;	Peephole 181	changed mov to clr
                           4488 ;src/ddc_regs.c:718: DDC1_DR3 = 0;
                           4489 ;     genAssign
                           4490 ;	Peephole 181	changed mov to clr
                           4491 ;	Peephole 219	removed redundant clear
                           4492 ;src/ddc_regs.c:719: DDC1_DR2 = 0;
                           4493 ;     genAssign
                           4494 ;	Peephole 181	changed mov to clr
                           4495 ;src/ddc_regs.c:720: DDC1_DR1 = 0;
                           4496 ;     genAssign
                           4497 ;	Peephole 181	changed mov to clr
                           4498 ;	Peephole 219	removed redundant clear
                           4499 ;	Peephole 219.a	removed redundant clear
   0BA9 E4                 4500 	clr	a
   0BAA F0                 4501 	movx	@dptr,a
   0BAB 90 60 13           4502 	mov	dptr,#_DDC1_DR3
   0BAE F0                 4503 	movx	@dptr,a
   0BAF 90 60 12           4504 	mov	dptr,#_DDC1_DR2
   0BB2 F0                 4505 	movx	@dptr,a
   0BB3 90 60 11           4506 	mov	dptr,#_DDC1_DR1
   0BB6 F0                 4507 	movx	@dptr,a
                           4508 ;src/ddc_regs.c:721: DDC1_DR0 = 0;
                           4509 ;     genAssign
   0BB7 90 60 10           4510 	mov	dptr,#_DDC1_DR0
                           4511 ;	Peephole 181	changed mov to clr
   0BBA E4                 4512 	clr	a
   0BBB F0                 4513 	movx	@dptr,a
                           4514 ;src/ddc_regs.c:714: for (i=0; i < 0xFF; i++) {
                           4515 ;     genPlus
                           4516 ;     genPlusIncr
   0BBC 0A                 4517 	inc	r2
                           4518 ;	Peephole 112.b	changed ljmp to sjmp
                           4519 ;	Peephole 243	avoided branch to sjmp
   0BBD BA 00 C9           4520 	cjne	r2,#0x00,00110$
   0BC0 0B                 4521 	inc	r3
   0BC1                    4522 00129$:
   0BC1 80 C6              4523 	sjmp	00110$
   0BC3                    4524 00113$:
                           4525 ;src/ddc_regs.c:723: return (unsigned char)i;
                           4526 ;     genCast
   0BC3 8A 82              4527 	mov	dpl,r2
                           4528 ;     genRet
                           4529 ;	Peephole 112.b	changed ljmp to sjmp
                           4530 ;src/ddc_regs.c:725: return 0;
                           4531 ;     genRet
                           4532 ;	Peephole 237.a	removed sjmp to ret
   0BC5 22                 4533 	ret
   0BC6                    4534 00105$:
   0BC6 75 82 00           4535 	mov	dpl,#0x00
   0BC9                    4536 00114$:
   0BC9 22                 4537 	ret
                           4538 ;------------------------------------------------------------
                           4539 ;Allocation info for local variables in function 'read_COEF'
                           4540 ;------------------------------------------------------------
                           4541 ;offset                    Allocated with name '_read_COEF_PARM_2'
                           4542 ;buf                       Allocated with name '_read_COEF_PARM_3'
                           4543 ;len                       Allocated with name '_read_COEF_PARM_4'
                           4544 ;index                     Allocated to registers r2 
                           4545 ;i                         Allocated with name '_read_COEF_i_1_1'
                           4546 ;j                         Allocated to registers r6 r7 
                           4547 ;------------------------------------------------------------
                           4548 ;src/ddc_regs.c:729: read_COEF (unsigned char index,
                           4549 ;	-----------------------------------------
                           4550 ;	 function read_COEF
                           4551 ;	-----------------------------------------
   0BCA                    4552 _read_COEF:
                           4553 ;     genReceive
   0BCA AA 82              4554 	mov	r2,dpl
                           4555 ;src/ddc_regs.c:736: if (offset > 0x0F) return 0;
                           4556 ;     genCmpGt
                           4557 ;     genCmp
                           4558 ;     genIfxJump
                           4559 ;	Peephole 108	removed ljmp by inverse jump logic
                           4560 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   0BCC E5 2B              4561 	mov	a,_read_COEF_PARM_2
   0BCE 24 F0              4562 	add	a,#0xff - 0x0F
   0BD0 50 04              4563 	jnc	00102$
   0BD2                    4564 00128$:
                           4565 ;     genRet
   0BD2 75 82 00           4566 	mov	dpl,#0x00
                           4567 ;	Peephole 251.a	replaced ljmp to ret with ret
   0BD5 22                 4568 	ret
   0BD6                    4569 00102$:
                           4570 ;src/ddc_regs.c:738: offset = offset << 4; // advance by 16
                           4571 ;     genLeftShift
                           4572 ;     genLeftShiftLiteral
                           4573 ;     genlshOne
   0BD6 E5 2B              4574 	mov	a,_read_COEF_PARM_2
   0BD8 C4                 4575 	swap	a
   0BD9 54 F0              4576 	anl	a,#0xf0
   0BDB F5 2B              4577 	mov	_read_COEF_PARM_2,a
                           4578 ;src/ddc_regs.c:740: if (len < 48) return 0; // need 48 bytes min (3 x 8 bytes) = 16 coeff
                           4579 ;     genCmpLt
                           4580 ;     genCmp
                           4581 ;     genIfxJump
                           4582 ;	Peephole 108	removed ljmp by inverse jump logic
                           4583 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0BDD 74 D0              4584 	mov	a,#0x100 - 0x30
   0BDF 25 2E              4585 	add	a,_read_COEF_PARM_4
   0BE1 40 04              4586 	jc	00104$
   0BE3                    4587 00129$:
                           4588 ;     genRet
   0BE3 75 82 00           4589 	mov	dpl,#0x00
                           4590 ;	Peephole 251.a	replaced ljmp to ret with ret
   0BE6 22                 4591 	ret
   0BE7                    4592 00104$:
                           4593 ;src/ddc_regs.c:742: if (index == 0)
                           4594 ;     genCmpEq
   0BE7 BA 00 02           4595 	cjne	r2,#0x00,00130$
   0BEA 80 03              4596 	sjmp	00131$
   0BEC                    4597 00130$:
   0BEC 02 0C 6B           4598 	ljmp	00108$
   0BEF                    4599 00131$:
                           4600 ;src/ddc_regs.c:744: for (i=0, j=0; i < 16; i++, j+=3) {
                           4601 ;     genAssign
   0BEF AB 2B              4602 	mov	r3,_read_COEF_PARM_2
                           4603 ;     genAssign
                           4604 ;     genAssign
                           4605 ;	Peephole 3.h	changed mov r6,#0x00 to r6,a
   0BF1 E4                 4606 	clr	a
   0BF2 F5 2F              4607 	mov	_read_COEF_i_1_1,a
   0BF4 F5 30              4608 	mov	(_read_COEF_i_1_1 + 1),a
   0BF6 FE                 4609 	mov	r6,a
                           4610 ;	Peephole 3.i	changed mov r7,#0x00 to r7,a
   0BF7 FF                 4611 	mov	r7,a
   0BF8                    4612 00110$:
                           4613 ;     genCmpLt
                           4614 ;     genCmp
   0BF8 C3                 4615 	clr	c
   0BF9 E5 2F              4616 	mov	a,_read_COEF_i_1_1
   0BFB 94 10              4617 	subb	a,#0x10
   0BFD E5 30              4618 	mov	a,(_read_COEF_i_1_1 + 1)
   0BFF 64 80              4619 	xrl	a,#0x80
   0C01 94 80              4620 	subb	a,#0x80
                           4621 ;     genIfxJump
                           4622 ;	Peephole 108	removed ljmp by inverse jump logic
   0C03 50 63              4623 	jnc	00113$
   0C05                    4624 00132$:
                           4625 ;src/ddc_regs.c:745: DDC0_AMR = 0;
                           4626 ;     genAssign
   0C05 90 60 07           4627 	mov	dptr,#_DDC0_AMR
                           4628 ;	Peephole 181	changed mov to clr
   0C08 E4                 4629 	clr	a
   0C09 F0                 4630 	movx	@dptr,a
                           4631 ;src/ddc_regs.c:746: DDC0_LAR = ((DDC_COEF + i + offset) & 0xFF);
                           4632 ;     genCast
   0C0A A8 2F              4633 	mov	r0,_read_COEF_i_1_1
                           4634 ;     genPlus
   0C0C 90 60 06           4635 	mov	dptr,#_DDC0_LAR
                           4636 ;	Peephole 236.g	used r3 instead of ar3
   0C0F EB                 4637 	mov	a,r3
                           4638 ;	Peephole 236.a	used r0 instead of ar0
   0C10 28                 4639 	add	a,r0
   0C11 F0                 4640 	movx	@dptr,a
                           4641 ;src/ddc_regs.c:747: buf[j] = DDC0_DR0;
                           4642 ;     genPlus
                           4643 ;	Peephole 236.g	used r6 instead of ar6
   0C12 EE                 4644 	mov	a,r6
   0C13 25 2C              4645 	add	a,_read_COEF_PARM_3
   0C15 F8                 4646 	mov	r0,a
                           4647 ;	Peephole 236.g	used r7 instead of ar7
   0C16 EF                 4648 	mov	a,r7
   0C17 35 2D              4649 	addc	a,(_read_COEF_PARM_3 + 1)
   0C19 F9                 4650 	mov	r1,a
                           4651 ;     genAssign
   0C1A 90 60 00           4652 	mov	dptr,#_DDC0_DR0
   0C1D E0                 4653 	movx	a,@dptr
                           4654 ;     genPointerSet
                           4655 ;     genFarPointerSet
                           4656 ;	Peephole 136	removed redundant moves
   0C1E FC                 4657 	mov	r4,a
   0C1F 88 82              4658 	mov	dpl,r0
   0C21 89 83              4659 	mov	dph,r1
   0C23 F0                 4660 	movx	@dptr,a
                           4661 ;src/ddc_regs.c:748: buf[j+1] = DDC0_DR1;
                           4662 ;     genPlus
                           4663 ;     genPlusIncr
   0C24 74 01              4664 	mov	a,#0x01
                           4665 ;	Peephole 236.a	used r6 instead of ar6
   0C26 2E                 4666 	add	a,r6
   0C27 FC                 4667 	mov	r4,a
                           4668 ;	Peephole 181	changed mov to clr
   0C28 E4                 4669 	clr	a
                           4670 ;	Peephole 236.b	used r7 instead of ar7
   0C29 3F                 4671 	addc	a,r7
   0C2A FD                 4672 	mov	r5,a
                           4673 ;     genPlus
                           4674 ;	Peephole 236.g	used r4 instead of ar4
   0C2B EC                 4675 	mov	a,r4
   0C2C 25 2C              4676 	add	a,_read_COEF_PARM_3
   0C2E FC                 4677 	mov	r4,a
                           4678 ;	Peephole 236.g	used r5 instead of ar5
   0C2F ED                 4679 	mov	a,r5
   0C30 35 2D              4680 	addc	a,(_read_COEF_PARM_3 + 1)
   0C32 FD                 4681 	mov	r5,a
                           4682 ;     genAssign
   0C33 90 60 01           4683 	mov	dptr,#_DDC0_DR1
   0C36 E0                 4684 	movx	a,@dptr
                           4685 ;     genPointerSet
                           4686 ;     genFarPointerSet
                           4687 ;	Peephole 136	removed redundant moves
   0C37 F8                 4688 	mov	r0,a
   0C38 8C 82              4689 	mov	dpl,r4
   0C3A 8D 83              4690 	mov	dph,r5
   0C3C F0                 4691 	movx	@dptr,a
                           4692 ;src/ddc_regs.c:749: buf[j+2] = DDC0_DR2;
                           4693 ;     genPlus
                           4694 ;     genPlusIncr
   0C3D 74 02              4695 	mov	a,#0x02
                           4696 ;	Peephole 236.a	used r6 instead of ar6
   0C3F 2E                 4697 	add	a,r6
   0C40 FC                 4698 	mov	r4,a
                           4699 ;	Peephole 181	changed mov to clr
   0C41 E4                 4700 	clr	a
                           4701 ;	Peephole 236.b	used r7 instead of ar7
   0C42 3F                 4702 	addc	a,r7
   0C43 FD                 4703 	mov	r5,a
                           4704 ;     genPlus
                           4705 ;	Peephole 236.g	used r4 instead of ar4
   0C44 EC                 4706 	mov	a,r4
   0C45 25 2C              4707 	add	a,_read_COEF_PARM_3
   0C47 FC                 4708 	mov	r4,a
                           4709 ;	Peephole 236.g	used r5 instead of ar5
   0C48 ED                 4710 	mov	a,r5
   0C49 35 2D              4711 	addc	a,(_read_COEF_PARM_3 + 1)
   0C4B FD                 4712 	mov	r5,a
                           4713 ;     genAssign
   0C4C 90 60 02           4714 	mov	dptr,#_DDC0_DR2
   0C4F E0                 4715 	movx	a,@dptr
                           4716 ;     genPointerSet
                           4717 ;     genFarPointerSet
                           4718 ;	Peephole 136	removed redundant moves
   0C50 F8                 4719 	mov	r0,a
   0C51 8C 82              4720 	mov	dpl,r4
   0C53 8D 83              4721 	mov	dph,r5
   0C55 F0                 4722 	movx	@dptr,a
                           4723 ;src/ddc_regs.c:744: for (i=0, j=0; i < 16; i++, j+=3) {
                           4724 ;     genPlus
                           4725 ;     genPlusIncr
   0C56 05 2F              4726 	inc	_read_COEF_i_1_1
   0C58 E4                 4727 	clr	a
   0C59 B5 2F 02           4728 	cjne	a,_read_COEF_i_1_1,00133$
   0C5C 05 30              4729 	inc	(_read_COEF_i_1_1 + 1)
   0C5E                    4730 00133$:
                           4731 ;     genPlus
                           4732 ;     genPlusIncr
   0C5E 74 03              4733 	mov	a,#0x03
                           4734 ;	Peephole 236.a	used r6 instead of ar6
   0C60 2E                 4735 	add	a,r6
   0C61 FE                 4736 	mov	r6,a
                           4737 ;	Peephole 181	changed mov to clr
   0C62 E4                 4738 	clr	a
                           4739 ;	Peephole 236.b	used r7 instead of ar7
   0C63 3F                 4740 	addc	a,r7
   0C64 FF                 4741 	mov	r7,a
   0C65 02 0B F8           4742 	ljmp	00110$
   0C68                    4743 00113$:
                           4744 ;src/ddc_regs.c:751: return (unsigned char)j;
                           4745 ;     genCast
   0C68 8E 82              4746 	mov	dpl,r6
                           4747 ;     genRet
                           4748 ;	Peephole 251.a	replaced ljmp to ret with ret
   0C6A 22                 4749 	ret
   0C6B                    4750 00108$:
                           4751 ;src/ddc_regs.c:752: } else if (index == 1) {
                           4752 ;     genCmpEq
   0C6B BA 01 02           4753 	cjne	r2,#0x01,00134$
   0C6E 80 03              4754 	sjmp	00135$
   0C70                    4755 00134$:
   0C70 02 0C EB           4756 	ljmp	00109$
   0C73                    4757 00135$:
                           4758 ;src/ddc_regs.c:753: for (i=0, j=0; i < 16; i++, j+=3) {
                           4759 ;     genAssign
   0C73 AA 2B              4760 	mov	r2,_read_COEF_PARM_2
                           4761 ;     genAssign
   0C75 7B 00              4762 	mov	r3,#0x00
   0C77 7C 00              4763 	mov	r4,#0x00
                           4764 ;     genAssign
   0C79 7D 00              4765 	mov	r5,#0x00
   0C7B 7E 00              4766 	mov	r6,#0x00
   0C7D                    4767 00114$:
                           4768 ;     genCmpLt
                           4769 ;     genCmp
   0C7D C3                 4770 	clr	c
   0C7E EB                 4771 	mov	a,r3
   0C7F 94 10              4772 	subb	a,#0x10
   0C81 EC                 4773 	mov	a,r4
   0C82 64 80              4774 	xrl	a,#0x80
   0C84 94 80              4775 	subb	a,#0x80
                           4776 ;     genIfxJump
                           4777 ;	Peephole 108	removed ljmp by inverse jump logic
   0C86 50 60              4778 	jnc	00117$
   0C88                    4779 00136$:
                           4780 ;src/ddc_regs.c:754: DDC1_AMR = 0;
                           4781 ;     genAssign
   0C88 90 60 17           4782 	mov	dptr,#_DDC1_AMR
                           4783 ;	Peephole 181	changed mov to clr
   0C8B E4                 4784 	clr	a
   0C8C F0                 4785 	movx	@dptr,a
                           4786 ;src/ddc_regs.c:755: DDC1_LAR = ((DDC_COEF + i + offset) & 0xFF);
                           4787 ;     genCast
   0C8D 8B 07              4788 	mov	ar7,r3
                           4789 ;     genPlus
   0C8F 90 60 16           4790 	mov	dptr,#_DDC1_LAR
                           4791 ;	Peephole 236.g	used r2 instead of ar2
   0C92 EA                 4792 	mov	a,r2
                           4793 ;	Peephole 236.a	used r7 instead of ar7
   0C93 2F                 4794 	add	a,r7
   0C94 F0                 4795 	movx	@dptr,a
                           4796 ;src/ddc_regs.c:756: buf[j] = DDC1_DR0;
                           4797 ;     genPlus
                           4798 ;	Peephole 236.g	used r5 instead of ar5
   0C95 ED                 4799 	mov	a,r5
   0C96 25 2C              4800 	add	a,_read_COEF_PARM_3
   0C98 FF                 4801 	mov	r7,a
                           4802 ;	Peephole 236.g	used r6 instead of ar6
   0C99 EE                 4803 	mov	a,r6
   0C9A 35 2D              4804 	addc	a,(_read_COEF_PARM_3 + 1)
   0C9C F8                 4805 	mov	r0,a
                           4806 ;     genAssign
   0C9D 90 60 10           4807 	mov	dptr,#_DDC1_DR0
   0CA0 E0                 4808 	movx	a,@dptr
                           4809 ;     genPointerSet
                           4810 ;     genFarPointerSet
                           4811 ;	Peephole 136	removed redundant moves
   0CA1 F9                 4812 	mov	r1,a
   0CA2 8F 82              4813 	mov	dpl,r7
   0CA4 88 83              4814 	mov	dph,r0
   0CA6 F0                 4815 	movx	@dptr,a
                           4816 ;src/ddc_regs.c:757: buf[j+1] = DDC1_DR1;
                           4817 ;     genPlus
                           4818 ;     genPlusIncr
   0CA7 74 01              4819 	mov	a,#0x01
                           4820 ;	Peephole 236.a	used r5 instead of ar5
   0CA9 2D                 4821 	add	a,r5
   0CAA FF                 4822 	mov	r7,a
                           4823 ;	Peephole 181	changed mov to clr
   0CAB E4                 4824 	clr	a
                           4825 ;	Peephole 236.b	used r6 instead of ar6
   0CAC 3E                 4826 	addc	a,r6
   0CAD F8                 4827 	mov	r0,a
                           4828 ;     genPlus
                           4829 ;	Peephole 236.g	used r7 instead of ar7
   0CAE EF                 4830 	mov	a,r7
   0CAF 25 2C              4831 	add	a,_read_COEF_PARM_3
   0CB1 FF                 4832 	mov	r7,a
                           4833 ;	Peephole 236.g	used r0 instead of ar0
   0CB2 E8                 4834 	mov	a,r0
   0CB3 35 2D              4835 	addc	a,(_read_COEF_PARM_3 + 1)
   0CB5 F8                 4836 	mov	r0,a
                           4837 ;     genAssign
   0CB6 90 60 11           4838 	mov	dptr,#_DDC1_DR1
   0CB9 E0                 4839 	movx	a,@dptr
                           4840 ;     genPointerSet
                           4841 ;     genFarPointerSet
                           4842 ;	Peephole 136	removed redundant moves
   0CBA F9                 4843 	mov	r1,a
   0CBB 8F 82              4844 	mov	dpl,r7
   0CBD 88 83              4845 	mov	dph,r0
   0CBF F0                 4846 	movx	@dptr,a
                           4847 ;src/ddc_regs.c:758: buf[j+2] = DDC1_DR2;
                           4848 ;     genPlus
                           4849 ;     genPlusIncr
   0CC0 74 02              4850 	mov	a,#0x02
                           4851 ;	Peephole 236.a	used r5 instead of ar5
   0CC2 2D                 4852 	add	a,r5
   0CC3 FF                 4853 	mov	r7,a
                           4854 ;	Peephole 181	changed mov to clr
   0CC4 E4                 4855 	clr	a
                           4856 ;	Peephole 236.b	used r6 instead of ar6
   0CC5 3E                 4857 	addc	a,r6
   0CC6 F8                 4858 	mov	r0,a
                           4859 ;     genPlus
                           4860 ;	Peephole 236.g	used r7 instead of ar7
   0CC7 EF                 4861 	mov	a,r7
   0CC8 25 2C              4862 	add	a,_read_COEF_PARM_3
   0CCA FF                 4863 	mov	r7,a
                           4864 ;	Peephole 236.g	used r0 instead of ar0
   0CCB E8                 4865 	mov	a,r0
   0CCC 35 2D              4866 	addc	a,(_read_COEF_PARM_3 + 1)
   0CCE F8                 4867 	mov	r0,a
                           4868 ;     genAssign
   0CCF 90 60 12           4869 	mov	dptr,#_DDC1_DR2
   0CD2 E0                 4870 	movx	a,@dptr
                           4871 ;     genPointerSet
                           4872 ;     genFarPointerSet
                           4873 ;	Peephole 136	removed redundant moves
   0CD3 F9                 4874 	mov	r1,a
   0CD4 8F 82              4875 	mov	dpl,r7
   0CD6 88 83              4876 	mov	dph,r0
   0CD8 F0                 4877 	movx	@dptr,a
                           4878 ;src/ddc_regs.c:753: for (i=0, j=0; i < 16; i++, j+=3) {
                           4879 ;     genPlus
                           4880 ;     genPlusIncr
   0CD9 0B                 4881 	inc	r3
   0CDA BB 00 01           4882 	cjne	r3,#0x00,00137$
   0CDD 0C                 4883 	inc	r4
   0CDE                    4884 00137$:
                           4885 ;     genPlus
                           4886 ;     genPlusIncr
   0CDE 74 03              4887 	mov	a,#0x03
                           4888 ;	Peephole 236.a	used r5 instead of ar5
   0CE0 2D                 4889 	add	a,r5
   0CE1 FD                 4890 	mov	r5,a
                           4891 ;	Peephole 181	changed mov to clr
   0CE2 E4                 4892 	clr	a
                           4893 ;	Peephole 236.b	used r6 instead of ar6
   0CE3 3E                 4894 	addc	a,r6
   0CE4 FE                 4895 	mov	r6,a
   0CE5 02 0C 7D           4896 	ljmp	00114$
   0CE8                    4897 00117$:
                           4898 ;src/ddc_regs.c:760: return (unsigned char)j;
                           4899 ;     genCast
   0CE8 8D 82              4900 	mov	dpl,r5
                           4901 ;     genRet
                           4902 ;	Peephole 112.b	changed ljmp to sjmp
                           4903 ;src/ddc_regs.c:762: return 0;
                           4904 ;     genRet
                           4905 ;	Peephole 237.a	removed sjmp to ret
   0CEA 22                 4906 	ret
   0CEB                    4907 00109$:
   0CEB 75 82 00           4908 	mov	dpl,#0x00
   0CEE                    4909 00118$:
   0CEE 22                 4910 	ret
                           4911 ;------------------------------------------------------------
                           4912 ;Allocation info for local variables in function 'set_COEF'
                           4913 ;------------------------------------------------------------
                           4914 ;offset                    Allocated with name '_set_COEF_PARM_2'
                           4915 ;buf                       Allocated with name '_set_COEF_PARM_3'
                           4916 ;len                       Allocated with name '_set_COEF_PARM_4'
                           4917 ;index                     Allocated to registers r2 
                           4918 ;i                         Allocated to registers r4 r5 
                           4919 ;j                         Allocated to registers r6 r7 
                           4920 ;------------------------------------------------------------
                           4921 ;src/ddc_regs.c:771: set_COEF (unsigned char index,
                           4922 ;	-----------------------------------------
                           4923 ;	 function set_COEF
                           4924 ;	-----------------------------------------
   0CEF                    4925 _set_COEF:
                           4926 ;     genReceive
   0CEF AA 82              4927 	mov	r2,dpl
                           4928 ;src/ddc_regs.c:778: if (offset > 0x0F)
                           4929 ;     genCmpGt
                           4930 ;     genCmp
                           4931 ;     genIfxJump
                           4932 ;	Peephole 108	removed ljmp by inverse jump logic
                           4933 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   0CF1 E5 2B              4934 	mov	a,_set_COEF_PARM_2
   0CF3 24 F0              4935 	add	a,#0xff - 0x0F
   0CF5 50 04              4936 	jnc	00102$
   0CF7                    4937 00128$:
                           4938 ;src/ddc_regs.c:779: return 0;
                           4939 ;     genRet
   0CF7 75 82 00           4940 	mov	dpl,#0x00
                           4941 ;	Peephole 251.a	replaced ljmp to ret with ret
   0CFA 22                 4942 	ret
   0CFB                    4943 00102$:
                           4944 ;src/ddc_regs.c:781: offset = offset << 4; // advance by 16
                           4945 ;     genLeftShift
                           4946 ;     genLeftShiftLiteral
                           4947 ;     genlshOne
   0CFB E5 2B              4948 	mov	a,_set_COEF_PARM_2
   0CFD C4                 4949 	swap	a
   0CFE 54 F0              4950 	anl	a,#0xf0
   0D00 F5 2B              4951 	mov	_set_COEF_PARM_2,a
                           4952 ;src/ddc_regs.c:783: if (len < 48) return 0; // need 48 bytes min (3 x 8 bytes) = 16 coeff
                           4953 ;     genCmpLt
                           4954 ;     genCmp
                           4955 ;     genIfxJump
                           4956 ;	Peephole 108	removed ljmp by inverse jump logic
                           4957 ;	Peephole 132.e	optimized genCmpLt by inverse logic (carry differs)
   0D02 74 D0              4958 	mov	a,#0x100 - 0x30
   0D04 25 2E              4959 	add	a,_set_COEF_PARM_4
   0D06 40 04              4960 	jc	00104$
   0D08                    4961 00129$:
                           4962 ;     genRet
   0D08 75 82 00           4963 	mov	dpl,#0x00
                           4964 ;	Peephole 251.a	replaced ljmp to ret with ret
   0D0B 22                 4965 	ret
   0D0C                    4966 00104$:
                           4967 ;src/ddc_regs.c:785: if (index == 0)
                           4968 ;     genCmpEq
   0D0C BA 00 02           4969 	cjne	r2,#0x00,00130$
   0D0F 80 03              4970 	sjmp	00131$
   0D11                    4971 00130$:
   0D11 02 0D 85           4972 	ljmp	00108$
   0D14                    4973 00131$:
                           4974 ;src/ddc_regs.c:787: for (i=0, j=0; i < 16; i++, j+=3) {
                           4975 ;     genAssign
   0D14 AB 2B              4976 	mov	r3,_set_COEF_PARM_2
                           4977 ;     genAssign
   0D16 7C 00              4978 	mov	r4,#0x00
   0D18 7D 00              4979 	mov	r5,#0x00
                           4980 ;     genAssign
   0D1A 7E 00              4981 	mov	r6,#0x00
   0D1C 7F 00              4982 	mov	r7,#0x00
   0D1E                    4983 00110$:
                           4984 ;     genCmpLt
                           4985 ;     genCmp
   0D1E C3                 4986 	clr	c
   0D1F EC                 4987 	mov	a,r4
   0D20 94 10              4988 	subb	a,#0x10
   0D22 ED                 4989 	mov	a,r5
   0D23 64 80              4990 	xrl	a,#0x80
   0D25 94 80              4991 	subb	a,#0x80
                           4992 ;     genIfxJump
                           4993 ;	Peephole 108	removed ljmp by inverse jump logic
   0D27 50 59              4994 	jnc	00113$
   0D29                    4995 00132$:
                           4996 ;src/ddc_regs.c:788: DDC0_AMR = 0;
                           4997 ;     genAssign
   0D29 90 60 07           4998 	mov	dptr,#_DDC0_AMR
                           4999 ;	Peephole 181	changed mov to clr
   0D2C E4                 5000 	clr	a
   0D2D F0                 5001 	movx	@dptr,a
                           5002 ;src/ddc_regs.c:789: DDC0_LAR = ((DDC_COEF + i + offset) & 0xFF);
                           5003 ;     genCast
   0D2E 8C 00              5004 	mov	ar0,r4
                           5005 ;     genPlus
   0D30 90 60 06           5006 	mov	dptr,#_DDC0_LAR
                           5007 ;	Peephole 236.g	used r3 instead of ar3
   0D33 EB                 5008 	mov	a,r3
                           5009 ;	Peephole 236.a	used r0 instead of ar0
   0D34 28                 5010 	add	a,r0
   0D35 F0                 5011 	movx	@dptr,a
                           5012 ;src/ddc_regs.c:790: DDC0_DR2 = (buf[j+2] & 0xFF);
                           5013 ;     genPlus
                           5014 ;     genPlusIncr
   0D36 74 02              5015 	mov	a,#0x02
                           5016 ;	Peephole 236.a	used r6 instead of ar6
   0D38 2E                 5017 	add	a,r6
   0D39 F8                 5018 	mov	r0,a
                           5019 ;	Peephole 181	changed mov to clr
   0D3A E4                 5020 	clr	a
                           5021 ;	Peephole 236.b	used r7 instead of ar7
   0D3B 3F                 5022 	addc	a,r7
   0D3C F9                 5023 	mov	r1,a
                           5024 ;     genPlus
                           5025 ;	Peephole 236.g	used r0 instead of ar0
   0D3D E8                 5026 	mov	a,r0
   0D3E 25 2C              5027 	add	a,_set_COEF_PARM_3
   0D40 F5 82              5028 	mov	dpl,a
                           5029 ;	Peephole 236.g	used r1 instead of ar1
   0D42 E9                 5030 	mov	a,r1
   0D43 35 2D              5031 	addc	a,(_set_COEF_PARM_3 + 1)
   0D45 F5 83              5032 	mov	dph,a
                           5033 ;     genPointerGet
                           5034 ;     genFarPointerGet
   0D47 E0                 5035 	movx	a,@dptr
                           5036 ;     genAssign
                           5037 ;	Peephole 100	removed redundant mov
   0D48 F8                 5038 	mov	r0,a
   0D49 90 60 02           5039 	mov	dptr,#_DDC0_DR2
   0D4C F0                 5040 	movx	@dptr,a
                           5041 ;src/ddc_regs.c:791: DDC0_DR1 = (buf[j+1] & 0xFF);
                           5042 ;     genPlus
                           5043 ;     genPlusIncr
   0D4D 74 01              5044 	mov	a,#0x01
                           5045 ;	Peephole 236.a	used r6 instead of ar6
   0D4F 2E                 5046 	add	a,r6
   0D50 F8                 5047 	mov	r0,a
                           5048 ;	Peephole 181	changed mov to clr
   0D51 E4                 5049 	clr	a
                           5050 ;	Peephole 236.b	used r7 instead of ar7
   0D52 3F                 5051 	addc	a,r7
   0D53 F9                 5052 	mov	r1,a
                           5053 ;     genPlus
                           5054 ;	Peephole 236.g	used r0 instead of ar0
   0D54 E8                 5055 	mov	a,r0
   0D55 25 2C              5056 	add	a,_set_COEF_PARM_3
   0D57 F5 82              5057 	mov	dpl,a
                           5058 ;	Peephole 236.g	used r1 instead of ar1
   0D59 E9                 5059 	mov	a,r1
   0D5A 35 2D              5060 	addc	a,(_set_COEF_PARM_3 + 1)
   0D5C F5 83              5061 	mov	dph,a
                           5062 ;     genPointerGet
                           5063 ;     genFarPointerGet
   0D5E E0                 5064 	movx	a,@dptr
                           5065 ;     genAssign
                           5066 ;	Peephole 100	removed redundant mov
   0D5F F8                 5067 	mov	r0,a
   0D60 90 60 01           5068 	mov	dptr,#_DDC0_DR1
   0D63 F0                 5069 	movx	@dptr,a
                           5070 ;src/ddc_regs.c:792: DDC0_DR0 = (buf[j] & 0xFF);
                           5071 ;     genPlus
                           5072 ;	Peephole 236.g	used r6 instead of ar6
   0D64 EE                 5073 	mov	a,r6
   0D65 25 2C              5074 	add	a,_set_COEF_PARM_3
   0D67 F5 82              5075 	mov	dpl,a
                           5076 ;	Peephole 236.g	used r7 instead of ar7
   0D69 EF                 5077 	mov	a,r7
   0D6A 35 2D              5078 	addc	a,(_set_COEF_PARM_3 + 1)
   0D6C F5 83              5079 	mov	dph,a
                           5080 ;     genPointerGet
                           5081 ;     genFarPointerGet
   0D6E E0                 5082 	movx	a,@dptr
                           5083 ;     genAssign
                           5084 ;	Peephole 100	removed redundant mov
   0D6F F8                 5085 	mov	r0,a
   0D70 90 60 00           5086 	mov	dptr,#_DDC0_DR0
   0D73 F0                 5087 	movx	@dptr,a
                           5088 ;src/ddc_regs.c:787: for (i=0, j=0; i < 16; i++, j+=3) {
                           5089 ;     genPlus
                           5090 ;     genPlusIncr
   0D74 0C                 5091 	inc	r4
   0D75 BC 00 01           5092 	cjne	r4,#0x00,00133$
   0D78 0D                 5093 	inc	r5
   0D79                    5094 00133$:
                           5095 ;     genPlus
                           5096 ;     genPlusIncr
   0D79 74 03              5097 	mov	a,#0x03
                           5098 ;	Peephole 236.a	used r6 instead of ar6
   0D7B 2E                 5099 	add	a,r6
   0D7C FE                 5100 	mov	r6,a
                           5101 ;	Peephole 181	changed mov to clr
   0D7D E4                 5102 	clr	a
                           5103 ;	Peephole 236.b	used r7 instead of ar7
   0D7E 3F                 5104 	addc	a,r7
   0D7F FF                 5105 	mov	r7,a
                           5106 ;	Peephole 112.b	changed ljmp to sjmp
   0D80 80 9C              5107 	sjmp	00110$
   0D82                    5108 00113$:
                           5109 ;src/ddc_regs.c:794: return (unsigned char)j;
                           5110 ;     genCast
   0D82 8E 82              5111 	mov	dpl,r6
                           5112 ;     genRet
                           5113 ;	Peephole 251.a	replaced ljmp to ret with ret
   0D84 22                 5114 	ret
   0D85                    5115 00108$:
                           5116 ;src/ddc_regs.c:795: } else if (index == 1) {
                           5117 ;     genCmpEq
   0D85 BA 01 02           5118 	cjne	r2,#0x01,00134$
   0D88 80 03              5119 	sjmp	00135$
   0D8A                    5120 00134$:
   0D8A 02 0D FE           5121 	ljmp	00109$
   0D8D                    5122 00135$:
                           5123 ;src/ddc_regs.c:796: for (i=0, j=0; i < 16; i++, j+=3) {
                           5124 ;     genAssign
   0D8D AA 2B              5125 	mov	r2,_set_COEF_PARM_2
                           5126 ;     genAssign
   0D8F 7B 00              5127 	mov	r3,#0x00
   0D91 7C 00              5128 	mov	r4,#0x00
                           5129 ;     genAssign
   0D93 7D 00              5130 	mov	r5,#0x00
   0D95 7E 00              5131 	mov	r6,#0x00
   0D97                    5132 00114$:
                           5133 ;     genCmpLt
                           5134 ;     genCmp
   0D97 C3                 5135 	clr	c
   0D98 EB                 5136 	mov	a,r3
   0D99 94 10              5137 	subb	a,#0x10
   0D9B EC                 5138 	mov	a,r4
   0D9C 64 80              5139 	xrl	a,#0x80
   0D9E 94 80              5140 	subb	a,#0x80
                           5141 ;     genIfxJump
                           5142 ;	Peephole 108	removed ljmp by inverse jump logic
   0DA0 50 59              5143 	jnc	00117$
   0DA2                    5144 00136$:
                           5145 ;src/ddc_regs.c:797: DDC1_AMR = 0;
                           5146 ;     genAssign
   0DA2 90 60 17           5147 	mov	dptr,#_DDC1_AMR
                           5148 ;	Peephole 181	changed mov to clr
   0DA5 E4                 5149 	clr	a
   0DA6 F0                 5150 	movx	@dptr,a
                           5151 ;src/ddc_regs.c:798: DDC1_LAR = ((DDC_COEF + i + offset) & 0xFF);
                           5152 ;     genCast
   0DA7 8B 07              5153 	mov	ar7,r3
                           5154 ;     genPlus
   0DA9 90 60 16           5155 	mov	dptr,#_DDC1_LAR
                           5156 ;	Peephole 236.g	used r2 instead of ar2
   0DAC EA                 5157 	mov	a,r2
                           5158 ;	Peephole 236.a	used r7 instead of ar7
   0DAD 2F                 5159 	add	a,r7
   0DAE F0                 5160 	movx	@dptr,a
                           5161 ;src/ddc_regs.c:799: DDC1_DR2 = (buf[j+2] & 0xFF);
                           5162 ;     genPlus
                           5163 ;     genPlusIncr
   0DAF 74 02              5164 	mov	a,#0x02
                           5165 ;	Peephole 236.a	used r5 instead of ar5
   0DB1 2D                 5166 	add	a,r5
   0DB2 FF                 5167 	mov	r7,a
                           5168 ;	Peephole 181	changed mov to clr
   0DB3 E4                 5169 	clr	a
                           5170 ;	Peephole 236.b	used r6 instead of ar6
   0DB4 3E                 5171 	addc	a,r6
   0DB5 F8                 5172 	mov	r0,a
                           5173 ;     genPlus
                           5174 ;	Peephole 236.g	used r7 instead of ar7
   0DB6 EF                 5175 	mov	a,r7
   0DB7 25 2C              5176 	add	a,_set_COEF_PARM_3
   0DB9 F5 82              5177 	mov	dpl,a
                           5178 ;	Peephole 236.g	used r0 instead of ar0
   0DBB E8                 5179 	mov	a,r0
   0DBC 35 2D              5180 	addc	a,(_set_COEF_PARM_3 + 1)
   0DBE F5 83              5181 	mov	dph,a
                           5182 ;     genPointerGet
                           5183 ;     genFarPointerGet
   0DC0 E0                 5184 	movx	a,@dptr
                           5185 ;     genAssign
                           5186 ;	Peephole 100	removed redundant mov
   0DC1 FF                 5187 	mov	r7,a
   0DC2 90 60 12           5188 	mov	dptr,#_DDC1_DR2
   0DC5 F0                 5189 	movx	@dptr,a
                           5190 ;src/ddc_regs.c:800: DDC1_DR1 = (buf[j+1] & 0xFF);
                           5191 ;     genPlus
                           5192 ;     genPlusIncr
   0DC6 74 01              5193 	mov	a,#0x01
                           5194 ;	Peephole 236.a	used r5 instead of ar5
   0DC8 2D                 5195 	add	a,r5
   0DC9 FF                 5196 	mov	r7,a
                           5197 ;	Peephole 181	changed mov to clr
   0DCA E4                 5198 	clr	a
                           5199 ;	Peephole 236.b	used r6 instead of ar6
   0DCB 3E                 5200 	addc	a,r6
   0DCC F8                 5201 	mov	r0,a
                           5202 ;     genPlus
                           5203 ;	Peephole 236.g	used r7 instead of ar7
   0DCD EF                 5204 	mov	a,r7
   0DCE 25 2C              5205 	add	a,_set_COEF_PARM_3
   0DD0 F5 82              5206 	mov	dpl,a
                           5207 ;	Peephole 236.g	used r0 instead of ar0
   0DD2 E8                 5208 	mov	a,r0
   0DD3 35 2D              5209 	addc	a,(_set_COEF_PARM_3 + 1)
   0DD5 F5 83              5210 	mov	dph,a
                           5211 ;     genPointerGet
                           5212 ;     genFarPointerGet
   0DD7 E0                 5213 	movx	a,@dptr
                           5214 ;     genAssign
                           5215 ;	Peephole 100	removed redundant mov
   0DD8 FF                 5216 	mov	r7,a
   0DD9 90 60 11           5217 	mov	dptr,#_DDC1_DR1
   0DDC F0                 5218 	movx	@dptr,a
                           5219 ;src/ddc_regs.c:801: DDC1_DR0 = (buf[j] & 0xFF);
                           5220 ;     genPlus
                           5221 ;	Peephole 236.g	used r5 instead of ar5
   0DDD ED                 5222 	mov	a,r5
   0DDE 25 2C              5223 	add	a,_set_COEF_PARM_3
   0DE0 F5 82              5224 	mov	dpl,a
                           5225 ;	Peephole 236.g	used r6 instead of ar6
   0DE2 EE                 5226 	mov	a,r6
   0DE3 35 2D              5227 	addc	a,(_set_COEF_PARM_3 + 1)
   0DE5 F5 83              5228 	mov	dph,a
                           5229 ;     genPointerGet
                           5230 ;     genFarPointerGet
   0DE7 E0                 5231 	movx	a,@dptr
                           5232 ;     genAssign
                           5233 ;	Peephole 100	removed redundant mov
   0DE8 FF                 5234 	mov	r7,a
   0DE9 90 60 10           5235 	mov	dptr,#_DDC1_DR0
   0DEC F0                 5236 	movx	@dptr,a
                           5237 ;src/ddc_regs.c:796: for (i=0, j=0; i < 16; i++, j+=3) {
                           5238 ;     genPlus
                           5239 ;     genPlusIncr
   0DED 0B                 5240 	inc	r3
   0DEE BB 00 01           5241 	cjne	r3,#0x00,00137$
   0DF1 0C                 5242 	inc	r4
   0DF2                    5243 00137$:
                           5244 ;     genPlus
                           5245 ;     genPlusIncr
   0DF2 74 03              5246 	mov	a,#0x03
                           5247 ;	Peephole 236.a	used r5 instead of ar5
   0DF4 2D                 5248 	add	a,r5
   0DF5 FD                 5249 	mov	r5,a
                           5250 ;	Peephole 181	changed mov to clr
   0DF6 E4                 5251 	clr	a
                           5252 ;	Peephole 236.b	used r6 instead of ar6
   0DF7 3E                 5253 	addc	a,r6
   0DF8 FE                 5254 	mov	r6,a
                           5255 ;	Peephole 112.b	changed ljmp to sjmp
   0DF9 80 9C              5256 	sjmp	00114$
   0DFB                    5257 00117$:
                           5258 ;src/ddc_regs.c:803: return (unsigned char)j;
                           5259 ;     genCast
   0DFB 8D 82              5260 	mov	dpl,r5
                           5261 ;     genRet
                           5262 ;	Peephole 112.b	changed ljmp to sjmp
                           5263 ;src/ddc_regs.c:805: return 0;
                           5264 ;     genRet
                           5265 ;	Peephole 237.a	removed sjmp to ret
   0DFD 22                 5266 	ret
   0DFE                    5267 00109$:
   0DFE 75 82 00           5268 	mov	dpl,#0x00
   0E01                    5269 00118$:
   0E01 22                 5270 	ret
                           5271 	.area CSEG    (CODE)
