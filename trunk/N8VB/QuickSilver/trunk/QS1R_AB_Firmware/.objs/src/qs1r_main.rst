                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Fri Jan 05 10:32:59 2007
                              5 ;--------------------------------------------------------
                              6 	.module qs1r_main
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
                             17 	.globl _bitCPLD_CS
                             18 	.globl _bitFPGA_CS
                             19 	.globl _bitALTERA_DCLK
                             20 	.globl _bitALTERA_DATA0
                             21 	.globl _bitS_CLK
                             22 	.globl _bitS_IN
                             23 	.globl _bitS_OUT
                             24 	.globl _EIPX6
                             25 	.globl _EIPX5
                             26 	.globl _EIPX4
                             27 	.globl _PI2C
                             28 	.globl _PUSB
                             29 	.globl _EIEX6
                             30 	.globl _EIEX5
                             31 	.globl _EIEX4
                             32 	.globl _EI2C
                             33 	.globl _EIUSB
                             34 	.globl _SMOD1
                             35 	.globl _ERESI
                             36 	.globl _RESI
                             37 	.globl _INT6
                             38 	.globl _CY
                             39 	.globl _AC
                             40 	.globl _F0
                             41 	.globl _RS1
                             42 	.globl _RS0
                             43 	.globl _OV
                             44 	.globl _FL
                             45 	.globl _P
                             46 	.globl _TF2
                             47 	.globl _EXF2
                             48 	.globl _RCLK
                             49 	.globl _TCLK
                             50 	.globl _EXEN2
                             51 	.globl _TR2
                             52 	.globl _C_T2
                             53 	.globl _CP_RL2
                             54 	.globl _SM01
                             55 	.globl _SM11
                             56 	.globl _SM21
                             57 	.globl _REN1
                             58 	.globl _TB81
                             59 	.globl _RB81
                             60 	.globl _TI1
                             61 	.globl _RI1
                             62 	.globl _PS1
                             63 	.globl _PT2
                             64 	.globl _PS0
                             65 	.globl _PT1
                             66 	.globl _PX1
                             67 	.globl _PT0
                             68 	.globl _PX0
                             69 	.globl _EA
                             70 	.globl _ES1
                             71 	.globl _ET2
                             72 	.globl _ES0
                             73 	.globl _ET1
                             74 	.globl _EX1
                             75 	.globl _ET0
                             76 	.globl _EX0
                             77 	.globl _SM0
                             78 	.globl _SM1
                             79 	.globl _SM2
                             80 	.globl _REN
                             81 	.globl _TB8
                             82 	.globl _RB8
                             83 	.globl _TI
                             84 	.globl _RI
                             85 	.globl _TF1
                             86 	.globl _TR1
                             87 	.globl _TF0
                             88 	.globl _TR0
                             89 	.globl _IE1
                             90 	.globl _IT1
                             91 	.globl _IE0
                             92 	.globl _IT0
                             93 	.globl _SEL
                             94 	.globl _EIP
                             95 	.globl _B
                             96 	.globl _EIE
                             97 	.globl _ACC
                             98 	.globl _EICON
                             99 	.globl _PSW
                            100 	.globl _TH2
                            101 	.globl _TL2
                            102 	.globl _RCAP2H
                            103 	.globl _RCAP2L
                            104 	.globl _T2CON
                            105 	.globl _SBUF1
                            106 	.globl _SCON1
                            107 	.globl _GPIFSGLDATLNOX
                            108 	.globl _GPIFSGLDATLX
                            109 	.globl _GPIFSGLDATH
                            110 	.globl _GPIFTRIG
                            111 	.globl _EP01STAT
                            112 	.globl _IP
                            113 	.globl _OEE
                            114 	.globl _OED
                            115 	.globl _OEC
                            116 	.globl _OEB
                            117 	.globl _OEA
                            118 	.globl _IOE
                            119 	.globl _IOD
                            120 	.globl _AUTOPTRSETUP
                            121 	.globl _EP68FIFOFLGS
                            122 	.globl _EP24FIFOFLGS
                            123 	.globl _EP2468STAT
                            124 	.globl _IE
                            125 	.globl _INT4CLR
                            126 	.globl _INT2CLR
                            127 	.globl _IOC
                            128 	.globl _AUTODAT2
                            129 	.globl _AUTOPTRL2
                            130 	.globl _AUTOPTRH2
                            131 	.globl _AUTODAT1
                            132 	.globl _APTR1L
                            133 	.globl _APTR1H
                            134 	.globl _SBUF0
                            135 	.globl _SCON0
                            136 	.globl _MPAGE
                            137 	.globl _EXIF
                            138 	.globl _IOB
                            139 	.globl _CKCON
                            140 	.globl _TH1
                            141 	.globl _TH0
                            142 	.globl _TL1
                            143 	.globl _TL0
                            144 	.globl _TMOD
                            145 	.globl _TCON
                            146 	.globl _PCON
                            147 	.globl _DPS
                            148 	.globl _DPH1
                            149 	.globl _DPL1
                            150 	.globl _DPH
                            151 	.globl _DPL
                            152 	.globl _SP
                            153 	.globl _IOA
                            154 	.globl _CPLD_GPIO
                            155 	.globl _RFFE_CNTRL
                            156 	.globl _ATTN
                            157 	.globl _DEBUG_LED_REG
                            158 	.globl _LTC2208_CNTRL_REG
                            159 	.globl _DDC_CNTRL
                            160 	.globl _DDC1_AMR
                            161 	.globl _DDC1_LAR
                            162 	.globl _DDC1_DR4
                            163 	.globl _DDC1_DR3
                            164 	.globl _DDC1_DR2
                            165 	.globl _DDC1_DR1
                            166 	.globl _DDC1_DR0
                            167 	.globl _DDC0_AMR
                            168 	.globl _DDC0_LAR
                            169 	.globl _DDC0_DR4
                            170 	.globl _DDC0_DR3
                            171 	.globl _DDC0_DR2
                            172 	.globl _DDC0_DR1
                            173 	.globl _DDC0_DR0
                            174 	.globl _EP8FIFOBUF
                            175 	.globl _EP6FIFOBUF
                            176 	.globl _EP4FIFOBUF
                            177 	.globl _EP2FIFOBUF
                            178 	.globl _EP1INBUF
                            179 	.globl _EP1OUTBUF
                            180 	.globl _EP0BUF
                            181 	.globl _CT4
                            182 	.globl _CT3
                            183 	.globl _CT2
                            184 	.globl _CT1
                            185 	.globl _USBTEST
                            186 	.globl _TESTCFG
                            187 	.globl _DBUG
                            188 	.globl _UDMACRCQUAL
                            189 	.globl _UDMACRCL
                            190 	.globl _UDMACRCH
                            191 	.globl _GPIFHOLDAMOUNT
                            192 	.globl _FLOWSTBHPERIOD
                            193 	.globl _FLOWSTBEDGE
                            194 	.globl _FLOWSTB
                            195 	.globl _FLOWHOLDOFF
                            196 	.globl _FLOWEQ1CTL
                            197 	.globl _FLOWEQ0CTL
                            198 	.globl _FLOWLOGIC
                            199 	.globl _FLOWSTATE
                            200 	.globl _GPIFABORT
                            201 	.globl _GPIFREADYSTAT
                            202 	.globl _GPIFREADYCFG
                            203 	.globl _XGPIFSGLDATLNOX
                            204 	.globl _XGPIFSGLDATLX
                            205 	.globl _XGPIFSGLDATH
                            206 	.globl _EP8GPIFTRIG
                            207 	.globl _EP8GPIFPFSTOP
                            208 	.globl _EP8GPIFFLGSEL
                            209 	.globl _EP6GPIFTRIG
                            210 	.globl _EP6GPIFPFSTOP
                            211 	.globl _EP6GPIFFLGSEL
                            212 	.globl _EP4GPIFTRIG
                            213 	.globl _EP4GPIFPFSTOP
                            214 	.globl _EP4GPIFFLGSEL
                            215 	.globl _EP2GPIFTRIG
                            216 	.globl _EP2GPIFPFSTOP
                            217 	.globl _EP2GPIFFLGSEL
                            218 	.globl _GPIFTCB0
                            219 	.globl _GPIFTCB1
                            220 	.globl _GPIFTCB2
                            221 	.globl _GPIFTCB3
                            222 	.globl _GPIFADRL
                            223 	.globl _GPIFADRH
                            224 	.globl _GPIFCTLCFG
                            225 	.globl _GPIFIDLECTL
                            226 	.globl _GPIFIDLECS
                            227 	.globl _GPIFWFSELECT
                            228 	.globl _SETUPDAT
                            229 	.globl _SUDPTRCTL
                            230 	.globl _SUDPTRL
                            231 	.globl _SUDPTRH
                            232 	.globl _EP8FIFOBCL
                            233 	.globl _EP8FIFOBCH
                            234 	.globl _EP6FIFOBCL
                            235 	.globl _EP6FIFOBCH
                            236 	.globl _EP4FIFOBCL
                            237 	.globl _EP4FIFOBCH
                            238 	.globl _EP2FIFOBCL
                            239 	.globl _EP2FIFOBCH
                            240 	.globl _EP8FIFOFLGS
                            241 	.globl _EP6FIFOFLGS
                            242 	.globl _EP4FIFOFLGS
                            243 	.globl _EP2FIFOFLGS
                            244 	.globl _EP8CS
                            245 	.globl _EP6CS
                            246 	.globl _EP4CS
                            247 	.globl _EP2CS
                            248 	.globl _EP1INCS
                            249 	.globl _EP1OUTCS
                            250 	.globl _EP0CS
                            251 	.globl _EP8BCL
                            252 	.globl _EP8BCH
                            253 	.globl _EP6BCL
                            254 	.globl _EP6BCH
                            255 	.globl _EP4BCL
                            256 	.globl _EP4BCH
                            257 	.globl _EP2BCL
                            258 	.globl _EP2BCH
                            259 	.globl _EP1INBC
                            260 	.globl _EP1OUTBC
                            261 	.globl _EP0BCL
                            262 	.globl _EP0BCH
                            263 	.globl _FNADDR
                            264 	.globl _MICROFRAME
                            265 	.globl _USBFRAMEL
                            266 	.globl _USBFRAMEH
                            267 	.globl _TOGCTL
                            268 	.globl _WAKEUPCS
                            269 	.globl _SUSPEND
                            270 	.globl _USBCS
                            271 	.globl _XAUTODAT2
                            272 	.globl _XAUTODAT1
                            273 	.globl _I2CTL
                            274 	.globl _I2DAT
                            275 	.globl _I2CS
                            276 	.globl _PORTECFG
                            277 	.globl _PORTCCFG
                            278 	.globl _PORTACFG
                            279 	.globl _INTSETUP
                            280 	.globl _INT4IVEC
                            281 	.globl _INT2IVEC
                            282 	.globl _CLRERRCNT
                            283 	.globl _ERRCNTLIM
                            284 	.globl _USBERRIRQ
                            285 	.globl _USBERRIE
                            286 	.globl _GPIFIRQ
                            287 	.globl _GPIFIE
                            288 	.globl _EPIRQ
                            289 	.globl _EPIE
                            290 	.globl _USBIRQ
                            291 	.globl _USBIE
                            292 	.globl _NAKIRQ
                            293 	.globl _NAKIE
                            294 	.globl _IBNIRQ
                            295 	.globl _IBNIE
                            296 	.globl _EP8FIFOIRQ
                            297 	.globl _EP8FIFOIE
                            298 	.globl _EP6FIFOIRQ
                            299 	.globl _EP6FIFOIE
                            300 	.globl _EP4FIFOIRQ
                            301 	.globl _EP4FIFOIE
                            302 	.globl _EP2FIFOIRQ
                            303 	.globl _EP2FIFOIE
                            304 	.globl _OUTPKTEND
                            305 	.globl _INPKTEND
                            306 	.globl _EP8ISOINPKTS
                            307 	.globl _EP6ISOINPKTS
                            308 	.globl _EP4ISOINPKTS
                            309 	.globl _EP2ISOINPKTS
                            310 	.globl _EP8FIFOPFL
                            311 	.globl _EP8FIFOPFH
                            312 	.globl _EP6FIFOPFL
                            313 	.globl _EP6FIFOPFH
                            314 	.globl _EP4FIFOPFL
                            315 	.globl _EP4FIFOPFH
                            316 	.globl _EP2FIFOPFL
                            317 	.globl _EP2FIFOPFH
                            318 	.globl _EP8AUTOINLENL
                            319 	.globl _EP8AUTOINLENH
                            320 	.globl _EP6AUTOINLENL
                            321 	.globl _EP6AUTOINLENH
                            322 	.globl _EP4AUTOINLENL
                            323 	.globl _EP4AUTOINLENH
                            324 	.globl _EP2AUTOINLENL
                            325 	.globl _EP2AUTOINLENH
                            326 	.globl _EP8FIFOCFG
                            327 	.globl _EP6FIFOCFG
                            328 	.globl _EP4FIFOCFG
                            329 	.globl _EP2FIFOCFG
                            330 	.globl _EP8CFG
                            331 	.globl _EP6CFG
                            332 	.globl _EP4CFG
                            333 	.globl _EP2CFG
                            334 	.globl _EP1INCFG
                            335 	.globl _EP1OUTCFG
                            336 	.globl _REVCTL
                            337 	.globl _REVID
                            338 	.globl _FIFOPINPOLAR
                            339 	.globl _UART230
                            340 	.globl _BPADDRL
                            341 	.globl _BPADDRH
                            342 	.globl _BREAKPT
                            343 	.globl _FIFORESET
                            344 	.globl _PINFLAGSCD
                            345 	.globl _PINFLAGSAB
                            346 	.globl _IFCONFIG
                            347 	.globl _CPUCS
                            348 	.globl _RES_WAVEDATA_END
                            349 	.globl _GPIF_WAVE_DATA
                            350 	.globl _app_vendor_cmd
                            351 ;--------------------------------------------------------
                            352 ; special function registers
                            353 ;--------------------------------------------------------
                            354 	.area RSEG    (DATA)
                    0080    355 _IOA	=	0x0080
                    0081    356 _SP	=	0x0081
                    0082    357 _DPL	=	0x0082
                    0083    358 _DPH	=	0x0083
                    0084    359 _DPL1	=	0x0084
                    0085    360 _DPH1	=	0x0085
                    0086    361 _DPS	=	0x0086
                    0087    362 _PCON	=	0x0087
                    0088    363 _TCON	=	0x0088
                    0089    364 _TMOD	=	0x0089
                    008A    365 _TL0	=	0x008a
                    008B    366 _TL1	=	0x008b
                    008C    367 _TH0	=	0x008c
                    008D    368 _TH1	=	0x008d
                    008E    369 _CKCON	=	0x008e
                    0090    370 _IOB	=	0x0090
                    0091    371 _EXIF	=	0x0091
                    0092    372 _MPAGE	=	0x0092
                    0098    373 _SCON0	=	0x0098
                    0099    374 _SBUF0	=	0x0099
                    009A    375 _APTR1H	=	0x009a
                    009B    376 _APTR1L	=	0x009b
                    009C    377 _AUTODAT1	=	0x009c
                    009D    378 _AUTOPTRH2	=	0x009d
                    009E    379 _AUTOPTRL2	=	0x009e
                    009F    380 _AUTODAT2	=	0x009f
                    00A0    381 _IOC	=	0x00a0
                    00A1    382 _INT2CLR	=	0x00a1
                    00A2    383 _INT4CLR	=	0x00a2
                    00A8    384 _IE	=	0x00a8
                    00AA    385 _EP2468STAT	=	0x00aa
                    00AB    386 _EP24FIFOFLGS	=	0x00ab
                    00AC    387 _EP68FIFOFLGS	=	0x00ac
                    00AF    388 _AUTOPTRSETUP	=	0x00af
                    00B0    389 _IOD	=	0x00b0
                    00B1    390 _IOE	=	0x00b1
                    00B2    391 _OEA	=	0x00b2
                    00B3    392 _OEB	=	0x00b3
                    00B4    393 _OEC	=	0x00b4
                    00B5    394 _OED	=	0x00b5
                    00B6    395 _OEE	=	0x00b6
                    00B8    396 _IP	=	0x00b8
                    00BA    397 _EP01STAT	=	0x00ba
                    00BB    398 _GPIFTRIG	=	0x00bb
                    00BD    399 _GPIFSGLDATH	=	0x00bd
                    00BE    400 _GPIFSGLDATLX	=	0x00be
                    00BF    401 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    402 _SCON1	=	0x00c0
                    00C1    403 _SBUF1	=	0x00c1
                    00C8    404 _T2CON	=	0x00c8
                    00CA    405 _RCAP2L	=	0x00ca
                    00CB    406 _RCAP2H	=	0x00cb
                    00CC    407 _TL2	=	0x00cc
                    00CD    408 _TH2	=	0x00cd
                    00D0    409 _PSW	=	0x00d0
                    00D8    410 _EICON	=	0x00d8
                    00E0    411 _ACC	=	0x00e0
                    00E8    412 _EIE	=	0x00e8
                    00F0    413 _B	=	0x00f0
                    00F8    414 _EIP	=	0x00f8
                            415 ;--------------------------------------------------------
                            416 ; special function bits 
                            417 ;--------------------------------------------------------
                            418 	.area RSEG    (DATA)
                    0086    419 _SEL	=	0x0086
                    0088    420 _IT0	=	0x0088
                    0089    421 _IE0	=	0x0089
                    008A    422 _IT1	=	0x008a
                    008B    423 _IE1	=	0x008b
                    008C    424 _TR0	=	0x008c
                    008D    425 _TF0	=	0x008d
                    008E    426 _TR1	=	0x008e
                    008F    427 _TF1	=	0x008f
                    0098    428 _RI	=	0x0098
                    0099    429 _TI	=	0x0099
                    009A    430 _RB8	=	0x009a
                    009B    431 _TB8	=	0x009b
                    009C    432 _REN	=	0x009c
                    009D    433 _SM2	=	0x009d
                    009E    434 _SM1	=	0x009e
                    009F    435 _SM0	=	0x009f
                    00A8    436 _EX0	=	0x00a8
                    00A9    437 _ET0	=	0x00a9
                    00AA    438 _EX1	=	0x00aa
                    00AB    439 _ET1	=	0x00ab
                    00AC    440 _ES0	=	0x00ac
                    00AD    441 _ET2	=	0x00ad
                    00AE    442 _ES1	=	0x00ae
                    00AF    443 _EA	=	0x00af
                    00B8    444 _PX0	=	0x00b8
                    00B9    445 _PT0	=	0x00b9
                    00BA    446 _PX1	=	0x00ba
                    00BB    447 _PT1	=	0x00bb
                    00BC    448 _PS0	=	0x00bc
                    00BD    449 _PT2	=	0x00bd
                    00BE    450 _PS1	=	0x00be
                    00C0    451 _RI1	=	0x00c0
                    00C1    452 _TI1	=	0x00c1
                    00C2    453 _RB81	=	0x00c2
                    00C3    454 _TB81	=	0x00c3
                    00C4    455 _REN1	=	0x00c4
                    00C5    456 _SM21	=	0x00c5
                    00C6    457 _SM11	=	0x00c6
                    00C7    458 _SM01	=	0x00c7
                    00C8    459 _CP_RL2	=	0x00c8
                    00C9    460 _C_T2	=	0x00c9
                    00CA    461 _TR2	=	0x00ca
                    00CB    462 _EXEN2	=	0x00cb
                    00CC    463 _TCLK	=	0x00cc
                    00CD    464 _RCLK	=	0x00cd
                    00CE    465 _EXF2	=	0x00ce
                    00CF    466 _TF2	=	0x00cf
                    00D0    467 _P	=	0x00d0
                    00D1    468 _FL	=	0x00d1
                    00D2    469 _OV	=	0x00d2
                    00D3    470 _RS0	=	0x00d3
                    00D4    471 _RS1	=	0x00d4
                    00D5    472 _F0	=	0x00d5
                    00D6    473 _AC	=	0x00d6
                    00D7    474 _CY	=	0x00d7
                    00DB    475 _INT6	=	0x00db
                    00DC    476 _RESI	=	0x00dc
                    00DD    477 _ERESI	=	0x00dd
                    00DF    478 _SMOD1	=	0x00df
                    00E8    479 _EIUSB	=	0x00e8
                    00E9    480 _EI2C	=	0x00e9
                    00EA    481 _EIEX4	=	0x00ea
                    00EB    482 _EIEX5	=	0x00eb
                    00EC    483 _EIEX6	=	0x00ec
                    00F8    484 _PUSB	=	0x00f8
                    00F9    485 _PI2C	=	0x00f9
                    00FA    486 _EIPX4	=	0x00fa
                    00FB    487 _EIPX5	=	0x00fb
                    00FC    488 _EIPX6	=	0x00fc
                    0080    489 _bitS_OUT	=	0x0080
                    0081    490 _bitS_IN	=	0x0081
                    0083    491 _bitS_CLK	=	0x0083
                    00A0    492 _bitALTERA_DATA0	=	0x00a0
                    00A2    493 _bitALTERA_DCLK	=	0x00a2
                    00A6    494 _bitFPGA_CS	=	0x00a6
                    00A7    495 _bitCPLD_CS	=	0x00a7
                            496 ;--------------------------------------------------------
                            497 ; overlayable register banks 
                            498 ;--------------------------------------------------------
                            499 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     500 	.ds 8
                            501 ;--------------------------------------------------------
                            502 ; internal ram data
                            503 ;--------------------------------------------------------
                            504 	.area DSEG    (DATA)
   001D                     505 _app_vendor_IN_cmd_offset_1_1::
   001D                     506 	.ds 1
   001E                     507 _isr_tick_count_1_1:
   001E                     508 	.ds 1
                            509 ;--------------------------------------------------------
                            510 ; overlayable items in internal ram 
                            511 ;--------------------------------------------------------
                            512 	.area OSEG    (OVR,DATA)
                            513 ;--------------------------------------------------------
                            514 ; Stack segment in internal ram 
                            515 ;--------------------------------------------------------
                            516 	.area	SSEG	(DATA)
   002F                     517 __start__stack:
   002F                     518 	.ds	1
                            519 
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
                    6000    712 _DDC0_DR0	=	0x6000
                    6001    713 _DDC0_DR1	=	0x6001
                    6002    714 _DDC0_DR2	=	0x6002
                    6003    715 _DDC0_DR3	=	0x6003
                    6004    716 _DDC0_DR4	=	0x6004
                    6006    717 _DDC0_LAR	=	0x6006
                    6007    718 _DDC0_AMR	=	0x6007
                    6010    719 _DDC1_DR0	=	0x6010
                    6011    720 _DDC1_DR1	=	0x6011
                    6012    721 _DDC1_DR2	=	0x6012
                    6013    722 _DDC1_DR3	=	0x6013
                    6014    723 _DDC1_DR4	=	0x6014
                    6016    724 _DDC1_LAR	=	0x6016
                    6017    725 _DDC1_AMR	=	0x6017
                    6020    726 _DDC_CNTRL	=	0x6020
                    6021    727 _LTC2208_CNTRL_REG	=	0x6021
                    6022    728 _DEBUG_LED_REG	=	0x6022
                    6023    729 _ATTN	=	0x6023
                    6024    730 _RFFE_CNTRL	=	0x6024
                    6025    731 _CPLD_GPIO	=	0x6025
   4002                     732 _patch_usb_descriptors_hw_rev_1_1:
   4002                     733 	.ds 1
   4003                     734 _patch_usb_descriptors_serial_no_1_1:
   4003                     735 	.ds 8
                            736 ;--------------------------------------------------------
                            737 ; external initialized ram data
                            738 ;--------------------------------------------------------
                            739 	.area CSEG    (CODE)
                            740 	.area GSINIT0 (CODE)
                            741 	.area GSINIT1 (CODE)
                            742 	.area GSINIT2 (CODE)
                            743 	.area GSINIT3 (CODE)
                            744 	.area GSINIT4 (CODE)
                            745 	.area GSINIT5 (CODE)
                            746 ;--------------------------------------------------------
                            747 ; interrupt vector 
                            748 ;--------------------------------------------------------
                            749 	.area CSEG    (CODE)
   0D66                     750 __interrupt_vect:
   0D66 02 20 0E            751 	ljmp	__sdcc_gsinit_startup
                            752 ;--------------------------------------------------------
                            753 ; global & static initialisations
                            754 ;--------------------------------------------------------
                            755 	.area CSEG    (CODE)
                            756 	.area GSINIT  (CODE)
                            757 	.area GSFINAL (CODE)
                            758 	.area GSINIT  (CODE)
                            759 	.globl __sdcc_gsinit_startup
                            760 	.globl __sdcc_program_startup
                            761 	.globl __start__stack
                            762 	.globl __mcs51_genRAMCLEAR
                            763 ;------------------------------------------------------------
                            764 ;Allocation info for local variables in function 'isr_tick'
                            765 ;------------------------------------------------------------
                            766 ;count                     Allocated with name '_isr_tick_count_1_1'
                            767 ;------------------------------------------------------------
                            768 ;src/qs1r_main.c:931: static unsigned char	count = 1;
                            769 ;     genAssign
   200B 75 1E 01            770 	mov	_isr_tick_count_1_1,#0x01
                            771 	.area GSFINAL (CODE)
   2021 02 0D 69            772 	ljmp	__sdcc_program_startup
                            773 ;--------------------------------------------------------
                            774 ; Home
                            775 ;--------------------------------------------------------
                            776 	.area HOME    (CODE)
                            777 	.area CSEG    (CODE)
                            778 ;--------------------------------------------------------
                            779 ; code
                            780 ;--------------------------------------------------------
                            781 	.area CSEG    (CODE)
   0D69                     782 __sdcc_program_startup:
   0D69 12 1F BA            783 	lcall	_main
                            784 ;	return from main will lock up
   0D6C 80 FE               785 	sjmp .
                            786 ;------------------------------------------------------------
                            787 ;Allocation info for local variables in function 'get_ep0_data'
                            788 ;------------------------------------------------------------
                            789 ;------------------------------------------------------------
                            790 ;src/qs1r_main.c:54: get_ep0_data (void)
                            791 ;	-----------------------------------------
                            792 ;	 function get_ep0_data
                            793 ;	-----------------------------------------
   0D6E                     794 _get_ep0_data:
                    0002    795 	ar2 = 0x02
                    0003    796 	ar3 = 0x03
                    0004    797 	ar4 = 0x04
                    0005    798 	ar5 = 0x05
                    0006    799 	ar6 = 0x06
                    0007    800 	ar7 = 0x07
                    0000    801 	ar0 = 0x00
                    0001    802 	ar1 = 0x01
                            803 ;src/qs1r_main.c:56: EP0BCL = 0;			// arm EP0 for OUT xfer.  This sets the busy bit
                            804 ;     genAssign
   0D6E 90 E6 8B            805 	mov	dptr,#_EP0BCL
                            806 ;	Peephole 181	changed mov to clr
   0D71 E4                  807 	clr	a
   0D72 F0                  808 	movx	@dptr,a
                            809 ;src/qs1r_main.c:57: while (EP0CS & bmEPBUSY);	// wait for busy to clear
   0D73                     810 00101$:
                            811 ;     genAssign
   0D73 90 E6 A0            812 	mov	dptr,#_EP0CS
   0D76 E0                  813 	movx	a,@dptr
                            814 ;     genAnd
                            815 ;	Peephole 105	removed redundant mov
   0D77 FA                  816 	mov	r2,a
                            817 ;     genIfxJump
                            818 ;	Peephole 112.a	removed ljmp by inverse jump logic
   0D78 20 E1 F8            819 	jb	acc.1,00101$
   0D7B                     820 00107$:
   0D7B                     821 00104$:
   0D7B 22                  822 	ret
                            823 ;------------------------------------------------------------
                            824 ;Allocation info for local variables in function 'app_vendor_OUT_cmd'
                            825 ;------------------------------------------------------------
                            826 ;i                         Allocated to registers r2 r3 
                            827 ;j                         Allocated to registers r5 r6 
                            828 ;offset                    Allocated to registers r2 
                            829 ;------------------------------------------------------------
                            830 ;src/qs1r_main.c:65: unsigned char app_vendor_OUT_cmd(void)
                            831 ;	-----------------------------------------
                            832 ;	 function app_vendor_OUT_cmd
                            833 ;	-----------------------------------------
   0D7C                     834 _app_vendor_OUT_cmd:
                            835 ;src/qs1r_main.c:70: switch (bRequest)
                            836 ;     genPointerGet
                            837 ;     genFarPointerGet
   0D7C 90 E6 B9            838 	mov	dptr,#(_SETUPDAT + 0x0001)
   0D7F E0                  839 	movx	a,@dptr
                            840 ;     genCmpGt
                            841 ;     genCmp
                            842 ;     genIfxJump
                            843 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
                            844 ;	Peephole 177.a	removed redundant mov
   0D80 FA                  845 	mov  r2,a
   0D81 24 D0               846 	add	a,#0xff - 0x2F
   0D83 50 03               847 	jnc	00349$
   0D85 02 16 50            848 	ljmp	00265$
   0D88                     849 00349$:
                            850 ;     genJumpTab
   0D88 EA                  851 	mov	a,r2
   0D89 24 09               852 	add	a,#(00350$-3-.)
   0D8B 83                  853 	movc	a,@a+pc
   0D8C C0 E0               854 	push	acc
   0D8E EA                  855 	mov	a,r2
   0D8F 24 33               856 	add	a,#(00351$-3-.)
   0D91 83                  857 	movc	a,@a+pc
   0D92 C0 E0               858 	push	acc
   0D94 22                  859 	ret
   0D95                     860 00350$:
   0D95 50                  861 	.db	00265$
   0D96 F5                  862 	.db	00101$
   0D97 E5                  863 	.db	00121$
   0D98 50                  864 	.db	00265$
   0D99 50                  865 	.db	00265$
   0D9A 50                  866 	.db	00265$
   0D9B 50                  867 	.db	00265$
   0D9C 50                  868 	.db	00265$
   0D9D 0F                  869 	.db	00127$
   0D9E 33                  870 	.db	00130$
   0D9F 01                  871 	.db	00102$
   0DA0 69                  872 	.db	00133$
   0DA1 87                  873 	.db	00137$
   0DA2 0D                  874 	.db	00103$
   0DA3 19                  875 	.db	00104$
   0DA4 25                  876 	.db	00105$
   0DA5 31                  877 	.db	00106$
   0DA6 3D                  878 	.db	00107$
   0DA7 49                  879 	.db	00108$
   0DA8 55                  880 	.db	00109$
   0DA9 61                  881 	.db	00110$
   0DAA 6D                  882 	.db	00111$
   0DAB 79                  883 	.db	00112$
   0DAC 85                  884 	.db	00113$
   0DAD 91                  885 	.db	00114$
   0DAE 9D                  886 	.db	00115$
   0DAF A9                  887 	.db	00116$
   0DB0 B5                  888 	.db	00117$
   0DB1 C1                  889 	.db	00118$
   0DB2 CD                  890 	.db	00119$
   0DB3 D9                  891 	.db	00120$
   0DB4 B3                  892 	.db	00144$
   0DB5 F9                  893 	.db	00151$
   0DB6 41                  894 	.db	00158$
   0DB7 BF                  895 	.db	00165$
   0DB8 3D                  896 	.db	00172$
   0DB9 97                  897 	.db	00179$
   0DBA DF                  898 	.db	00186$
   0DBB 27                  899 	.db	00193$
   0DBC 6F                  900 	.db	00200$
   0DBD B7                  901 	.db	00207$
   0DBE FF                  902 	.db	00214$
   0DBF 47                  903 	.db	00221$
   0DC0 8F                  904 	.db	00228$
   0DC1 D7                  905 	.db	00235$
   0DC2 B5                  906 	.db	00258$
   0DC3 1B                  907 	.db	00251$
   0DC4 1F                  908 	.db	00242$
   0DC5                     909 00351$:
   0DC5 16                  910 	.db	00265$>>8
   0DC6 0D                  911 	.db	00101$>>8
   0DC7 0E                  912 	.db	00121$>>8
   0DC8 16                  913 	.db	00265$>>8
   0DC9 16                  914 	.db	00265$>>8
   0DCA 16                  915 	.db	00265$>>8
   0DCB 16                  916 	.db	00265$>>8
   0DCC 16                  917 	.db	00265$>>8
   0DCD 0F                  918 	.db	00127$>>8
   0DCE 0F                  919 	.db	00130$>>8
   0DCF 0E                  920 	.db	00102$>>8
   0DD0 0F                  921 	.db	00133$>>8
   0DD1 0F                  922 	.db	00137$>>8
   0DD2 0E                  923 	.db	00103$>>8
   0DD3 0E                  924 	.db	00104$>>8
   0DD4 0E                  925 	.db	00105$>>8
   0DD5 0E                  926 	.db	00106$>>8
   0DD6 0E                  927 	.db	00107$>>8
   0DD7 0E                  928 	.db	00108$>>8
   0DD8 0E                  929 	.db	00109$>>8
   0DD9 0E                  930 	.db	00110$>>8
   0DDA 0E                  931 	.db	00111$>>8
   0DDB 0E                  932 	.db	00112$>>8
   0DDC 0E                  933 	.db	00113$>>8
   0DDD 0E                  934 	.db	00114$>>8
   0DDE 0E                  935 	.db	00115$>>8
   0DDF 0E                  936 	.db	00116$>>8
   0DE0 0E                  937 	.db	00117$>>8
   0DE1 0E                  938 	.db	00118$>>8
   0DE2 0E                  939 	.db	00119$>>8
   0DE3 0E                  940 	.db	00120$>>8
   0DE4 0F                  941 	.db	00144$>>8
   0DE5 0F                  942 	.db	00151$>>8
   0DE6 10                  943 	.db	00158$>>8
   0DE7 10                  944 	.db	00165$>>8
   0DE8 11                  945 	.db	00172$>>8
   0DE9 11                  946 	.db	00179$>>8
   0DEA 11                  947 	.db	00186$>>8
   0DEB 12                  948 	.db	00193$>>8
   0DEC 12                  949 	.db	00200$>>8
   0DED 12                  950 	.db	00207$>>8
   0DEE 12                  951 	.db	00214$>>8
   0DEF 13                  952 	.db	00221$>>8
   0DF0 13                  953 	.db	00228$>>8
   0DF1 13                  954 	.db	00235$>>8
   0DF2 15                  955 	.db	00258$>>8
   0DF3 15                  956 	.db	00251$>>8
   0DF4 14                  957 	.db	00242$>>8
                            958 ;src/qs1r_main.c:73: case VRQ_DEBUG_LED_SET:
   0DF5                     959 00101$:
                            960 ;src/qs1r_main.c:75: DEBUG_LED_REG = (BYTE)wValueL;
                            961 ;     genPointerGet
                            962 ;     genFarPointerGet
   0DF5 90 E6 BA            963 	mov	dptr,#(_SETUPDAT + 0x0002)
   0DF8 E0                  964 	movx	a,@dptr
                            965 ;     genAssign
                            966 ;	Peephole 100	removed redundant mov
   0DF9 FA                  967 	mov	r2,a
   0DFA 90 60 22            968 	mov	dptr,#_DEBUG_LED_REG
   0DFD F0                  969 	movx	@dptr,a
                            970 ;src/qs1r_main.c:76: break;
   0DFE 02 16 54            971 	ljmp	00266$
                            972 ;src/qs1r_main.c:78: case VRQ_LTC2208_CNTRL_SET:
   0E01                     973 00102$:
                            974 ;src/qs1r_main.c:80: LTC2208_CNTRL_REG = (BYTE)wValueL;
                            975 ;     genPointerGet
                            976 ;     genFarPointerGet
   0E01 90 E6 BA            977 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E04 E0                  978 	movx	a,@dptr
                            979 ;     genAssign
                            980 ;	Peephole 100	removed redundant mov
   0E05 FA                  981 	mov	r2,a
   0E06 90 60 21            982 	mov	dptr,#_LTC2208_CNTRL_REG
   0E09 F0                  983 	movx	@dptr,a
                            984 ;src/qs1r_main.c:81: break;
   0E0A 02 16 54            985 	ljmp	00266$
                            986 ;src/qs1r_main.c:83: case VRQ_ATTN_SET:
   0E0D                     987 00103$:
                            988 ;src/qs1r_main.c:85: ATTN = (BYTE)wValueL;
                            989 ;     genPointerGet
                            990 ;     genFarPointerGet
   0E0D 90 E6 BA            991 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E10 E0                  992 	movx	a,@dptr
                            993 ;     genAssign
                            994 ;	Peephole 100	removed redundant mov
   0E11 FA                  995 	mov	r2,a
   0E12 90 60 23            996 	mov	dptr,#_ATTN
   0E15 F0                  997 	movx	@dptr,a
                            998 ;src/qs1r_main.c:86: break;
   0E16 02 16 54            999 	ljmp	00266$
                           1000 ;src/qs1r_main.c:88: case VRQ_RFFE_SET:
   0E19                    1001 00104$:
                           1002 ;src/qs1r_main.c:90: RFFE_CNTRL = (BYTE)wValueL;
                           1003 ;     genPointerGet
                           1004 ;     genFarPointerGet
   0E19 90 E6 BA           1005 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E1C E0                 1006 	movx	a,@dptr
                           1007 ;     genAssign
                           1008 ;	Peephole 100	removed redundant mov
   0E1D FA                 1009 	mov	r2,a
   0E1E 90 60 24           1010 	mov	dptr,#_RFFE_CNTRL
   0E21 F0                 1011 	movx	@dptr,a
                           1012 ;src/qs1r_main.c:91: break;
   0E22 02 16 54           1013 	ljmp	00266$
                           1014 ;src/qs1r_main.c:93: case VRQ_DDC_CNTRL_SET:
   0E25                    1015 00105$:
                           1016 ;src/qs1r_main.c:95: DDC_CNTRL = (BYTE)wValueL;
                           1017 ;     genPointerGet
                           1018 ;     genFarPointerGet
   0E25 90 E6 BA           1019 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E28 E0                 1020 	movx	a,@dptr
                           1021 ;     genAssign
                           1022 ;	Peephole 100	removed redundant mov
   0E29 FA                 1023 	mov	r2,a
   0E2A 90 60 20           1024 	mov	dptr,#_DDC_CNTRL
   0E2D F0                 1025 	movx	@dptr,a
                           1026 ;src/qs1r_main.c:96: break;
   0E2E 02 16 54           1027 	ljmp	00266$
                           1028 ;src/qs1r_main.c:98: case VRQ_CPLD_GPIO_SET:
   0E31                    1029 00106$:
                           1030 ;src/qs1r_main.c:100: CPLD_GPIO = (BYTE)wValueL;
                           1031 ;     genPointerGet
                           1032 ;     genFarPointerGet
   0E31 90 E6 BA           1033 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E34 E0                 1034 	movx	a,@dptr
                           1035 ;     genAssign
                           1036 ;	Peephole 100	removed redundant mov
   0E35 FA                 1037 	mov	r2,a
   0E36 90 60 25           1038 	mov	dptr,#_CPLD_GPIO
   0E39 F0                 1039 	movx	@dptr,a
                           1040 ;src/qs1r_main.c:101: break;
   0E3A 02 16 54           1041 	ljmp	00266$
                           1042 ;src/qs1r_main.c:103: case VRQ_DDC0_DR0_SET:
   0E3D                    1043 00107$:
                           1044 ;src/qs1r_main.c:105: DDC0_DR0 = (BYTE)wValueL;
                           1045 ;     genPointerGet
                           1046 ;     genFarPointerGet
   0E3D 90 E6 BA           1047 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E40 E0                 1048 	movx	a,@dptr
                           1049 ;     genAssign
                           1050 ;	Peephole 100	removed redundant mov
   0E41 FA                 1051 	mov	r2,a
   0E42 90 60 00           1052 	mov	dptr,#_DDC0_DR0
   0E45 F0                 1053 	movx	@dptr,a
                           1054 ;src/qs1r_main.c:106: break;
   0E46 02 16 54           1055 	ljmp	00266$
                           1056 ;src/qs1r_main.c:108: case VRQ_DDC0_DR1_SET:
   0E49                    1057 00108$:
                           1058 ;src/qs1r_main.c:110: DDC0_DR1 = (BYTE)wValueL;
                           1059 ;     genPointerGet
                           1060 ;     genFarPointerGet
   0E49 90 E6 BA           1061 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E4C E0                 1062 	movx	a,@dptr
                           1063 ;     genAssign
                           1064 ;	Peephole 100	removed redundant mov
   0E4D FA                 1065 	mov	r2,a
   0E4E 90 60 01           1066 	mov	dptr,#_DDC0_DR1
   0E51 F0                 1067 	movx	@dptr,a
                           1068 ;src/qs1r_main.c:111: break;
   0E52 02 16 54           1069 	ljmp	00266$
                           1070 ;src/qs1r_main.c:113: case VRQ_DDC0_DR2_SET:
   0E55                    1071 00109$:
                           1072 ;src/qs1r_main.c:115: DDC0_DR2 = (BYTE)wValueL;
                           1073 ;     genPointerGet
                           1074 ;     genFarPointerGet
   0E55 90 E6 BA           1075 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E58 E0                 1076 	movx	a,@dptr
                           1077 ;     genAssign
                           1078 ;	Peephole 100	removed redundant mov
   0E59 FA                 1079 	mov	r2,a
   0E5A 90 60 02           1080 	mov	dptr,#_DDC0_DR2
   0E5D F0                 1081 	movx	@dptr,a
                           1082 ;src/qs1r_main.c:116: break;
   0E5E 02 16 54           1083 	ljmp	00266$
                           1084 ;src/qs1r_main.c:118: case VRQ_DDC0_DR3_SET:
   0E61                    1085 00110$:
                           1086 ;src/qs1r_main.c:120: DDC0_DR3 = (BYTE)wValueL;
                           1087 ;     genPointerGet
                           1088 ;     genFarPointerGet
   0E61 90 E6 BA           1089 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E64 E0                 1090 	movx	a,@dptr
                           1091 ;     genAssign
                           1092 ;	Peephole 100	removed redundant mov
   0E65 FA                 1093 	mov	r2,a
   0E66 90 60 03           1094 	mov	dptr,#_DDC0_DR3
   0E69 F0                 1095 	movx	@dptr,a
                           1096 ;src/qs1r_main.c:121: break;
   0E6A 02 16 54           1097 	ljmp	00266$
                           1098 ;src/qs1r_main.c:123: case VRQ_DDC0_DR4_SET:
   0E6D                    1099 00111$:
                           1100 ;src/qs1r_main.c:125: DDC0_DR4 = (BYTE)wValueL;
                           1101 ;     genPointerGet
                           1102 ;     genFarPointerGet
   0E6D 90 E6 BA           1103 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E70 E0                 1104 	movx	a,@dptr
                           1105 ;     genAssign
                           1106 ;	Peephole 100	removed redundant mov
   0E71 FA                 1107 	mov	r2,a
   0E72 90 60 04           1108 	mov	dptr,#_DDC0_DR4
   0E75 F0                 1109 	movx	@dptr,a
                           1110 ;src/qs1r_main.c:126: break;
   0E76 02 16 54           1111 	ljmp	00266$
                           1112 ;src/qs1r_main.c:128: case VRQ_DDC0_LAR_SET:
   0E79                    1113 00112$:
                           1114 ;src/qs1r_main.c:130: DDC0_LAR = (BYTE)wValueL;
                           1115 ;     genPointerGet
                           1116 ;     genFarPointerGet
   0E79 90 E6 BA           1117 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E7C E0                 1118 	movx	a,@dptr
                           1119 ;     genAssign
                           1120 ;	Peephole 100	removed redundant mov
   0E7D FA                 1121 	mov	r2,a
   0E7E 90 60 06           1122 	mov	dptr,#_DDC0_LAR
   0E81 F0                 1123 	movx	@dptr,a
                           1124 ;src/qs1r_main.c:131: break;
   0E82 02 16 54           1125 	ljmp	00266$
                           1126 ;src/qs1r_main.c:133: case VRQ_DDC0_AMR_SET:
   0E85                    1127 00113$:
                           1128 ;src/qs1r_main.c:135: DDC0_AMR = (BYTE)wValueL;
                           1129 ;     genPointerGet
                           1130 ;     genFarPointerGet
   0E85 90 E6 BA           1131 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E88 E0                 1132 	movx	a,@dptr
                           1133 ;     genAssign
                           1134 ;	Peephole 100	removed redundant mov
   0E89 FA                 1135 	mov	r2,a
   0E8A 90 60 07           1136 	mov	dptr,#_DDC0_AMR
   0E8D F0                 1137 	movx	@dptr,a
                           1138 ;src/qs1r_main.c:136: break;
   0E8E 02 16 54           1139 	ljmp	00266$
                           1140 ;src/qs1r_main.c:138: case VRQ_DDC1_DR0_SET:
   0E91                    1141 00114$:
                           1142 ;src/qs1r_main.c:140: DDC1_DR0 = (BYTE)wValueL;
                           1143 ;     genPointerGet
                           1144 ;     genFarPointerGet
   0E91 90 E6 BA           1145 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E94 E0                 1146 	movx	a,@dptr
                           1147 ;     genAssign
                           1148 ;	Peephole 100	removed redundant mov
   0E95 FA                 1149 	mov	r2,a
   0E96 90 60 10           1150 	mov	dptr,#_DDC1_DR0
   0E99 F0                 1151 	movx	@dptr,a
                           1152 ;src/qs1r_main.c:141: break;
   0E9A 02 16 54           1153 	ljmp	00266$
                           1154 ;src/qs1r_main.c:143: case VRQ_DDC1_DR1_SET:
   0E9D                    1155 00115$:
                           1156 ;src/qs1r_main.c:145: DDC1_DR1 = (BYTE)wValueL;
                           1157 ;     genPointerGet
                           1158 ;     genFarPointerGet
   0E9D 90 E6 BA           1159 	mov	dptr,#(_SETUPDAT + 0x0002)
   0EA0 E0                 1160 	movx	a,@dptr
                           1161 ;     genAssign
                           1162 ;	Peephole 100	removed redundant mov
   0EA1 FA                 1163 	mov	r2,a
   0EA2 90 60 11           1164 	mov	dptr,#_DDC1_DR1
   0EA5 F0                 1165 	movx	@dptr,a
                           1166 ;src/qs1r_main.c:146: break;
   0EA6 02 16 54           1167 	ljmp	00266$
                           1168 ;src/qs1r_main.c:148: case VRQ_DDC1_DR2_SET:
   0EA9                    1169 00116$:
                           1170 ;src/qs1r_main.c:150: DDC1_DR2 = (BYTE)wValueL;
                           1171 ;     genPointerGet
                           1172 ;     genFarPointerGet
   0EA9 90 E6 BA           1173 	mov	dptr,#(_SETUPDAT + 0x0002)
   0EAC E0                 1174 	movx	a,@dptr
                           1175 ;     genAssign
                           1176 ;	Peephole 100	removed redundant mov
   0EAD FA                 1177 	mov	r2,a
   0EAE 90 60 12           1178 	mov	dptr,#_DDC1_DR2
   0EB1 F0                 1179 	movx	@dptr,a
                           1180 ;src/qs1r_main.c:151: break;
   0EB2 02 16 54           1181 	ljmp	00266$
                           1182 ;src/qs1r_main.c:153: case VRQ_DDC1_DR3_SET:
   0EB5                    1183 00117$:
                           1184 ;src/qs1r_main.c:155: DDC1_DR3 = (BYTE)wValueL;
                           1185 ;     genPointerGet
                           1186 ;     genFarPointerGet
   0EB5 90 E6 BA           1187 	mov	dptr,#(_SETUPDAT + 0x0002)
   0EB8 E0                 1188 	movx	a,@dptr
                           1189 ;     genAssign
                           1190 ;	Peephole 100	removed redundant mov
   0EB9 FA                 1191 	mov	r2,a
   0EBA 90 60 13           1192 	mov	dptr,#_DDC1_DR3
   0EBD F0                 1193 	movx	@dptr,a
                           1194 ;src/qs1r_main.c:156: break;
   0EBE 02 16 54           1195 	ljmp	00266$
                           1196 ;src/qs1r_main.c:158: case VRQ_DDC1_DR4_SET:
   0EC1                    1197 00118$:
                           1198 ;src/qs1r_main.c:160: DDC1_DR4 = (BYTE)wValueL;
                           1199 ;     genPointerGet
                           1200 ;     genFarPointerGet
   0EC1 90 E6 BA           1201 	mov	dptr,#(_SETUPDAT + 0x0002)
   0EC4 E0                 1202 	movx	a,@dptr
                           1203 ;     genAssign
                           1204 ;	Peephole 100	removed redundant mov
   0EC5 FA                 1205 	mov	r2,a
   0EC6 90 60 14           1206 	mov	dptr,#_DDC1_DR4
   0EC9 F0                 1207 	movx	@dptr,a
                           1208 ;src/qs1r_main.c:161: break;
   0ECA 02 16 54           1209 	ljmp	00266$
                           1210 ;src/qs1r_main.c:163: case VRQ_DDC1_LAR_SET:
   0ECD                    1211 00119$:
                           1212 ;src/qs1r_main.c:165: DDC1_LAR = (BYTE)wValueL;
                           1213 ;     genPointerGet
                           1214 ;     genFarPointerGet
   0ECD 90 E6 BA           1215 	mov	dptr,#(_SETUPDAT + 0x0002)
   0ED0 E0                 1216 	movx	a,@dptr
                           1217 ;     genAssign
                           1218 ;	Peephole 100	removed redundant mov
   0ED1 FA                 1219 	mov	r2,a
   0ED2 90 60 16           1220 	mov	dptr,#_DDC1_LAR
   0ED5 F0                 1221 	movx	@dptr,a
                           1222 ;src/qs1r_main.c:166: break;
   0ED6 02 16 54           1223 	ljmp	00266$
                           1224 ;src/qs1r_main.c:168: case VRQ_DDC1_AMR_SET:
   0ED9                    1225 00120$:
                           1226 ;src/qs1r_main.c:170: DDC1_AMR = (BYTE)wValueL;
                           1227 ;     genPointerGet
                           1228 ;     genFarPointerGet
   0ED9 90 E6 BA           1229 	mov	dptr,#(_SETUPDAT + 0x0002)
   0EDC E0                 1230 	movx	a,@dptr
                           1231 ;     genAssign
                           1232 ;	Peephole 100	removed redundant mov
   0EDD FA                 1233 	mov	r2,a
   0EDE 90 60 17           1234 	mov	dptr,#_DDC1_AMR
   0EE1 F0                 1235 	movx	@dptr,a
                           1236 ;src/qs1r_main.c:171: break;
   0EE2 02 16 54           1237 	ljmp	00266$
                           1238 ;src/qs1r_main.c:173: case VRQ_FPGA_LOAD:
   0EE5                    1239 00121$:
                           1240 ;src/qs1r_main.c:175: switch (wIndexL)			// sub-command
                           1241 ;     genPointerGet
                           1242 ;     genFarPointerGet
   0EE5 90 E6 BC           1243 	mov	dptr,#(_SETUPDAT + 0x0004)
   0EE8 E0                 1244 	movx	a,@dptr
                           1245 ;     genCmpEq
                           1246 ;	Peephole 112.b	changed ljmp to sjmp
                           1247 ;	Peephole 115.b	jump optimization
   0EE9 FA                 1248 	mov	r2,a
   0EEA 60 0A              1249 	jz	00122$
   0EEC                    1250 00352$:
                           1251 ;     genCmpEq
   0EEC BA 01 02           1252 	cjne	r2,#0x01,00353$
                           1253 ;	Peephole 112.b	changed ljmp to sjmp
   0EEF 80 08              1254 	sjmp	00123$
   0EF1                    1255 00353$:
                           1256 ;     genCmpEq
                           1257 ;	Peephole 112.b	changed ljmp to sjmp
                           1258 ;src/qs1r_main.c:177: case FL_BEGIN:
                           1259 ;	Peephole 112.b	changed ljmp to sjmp
                           1260 ;	Peephole 199	optimized misc jump sequence
   0EF1 BA 02 17           1261 	cjne	r2,#0x02,00125$
   0EF4 80 12              1262 	sjmp	00124$
                           1263 ;00354$:
   0EF6                    1264 00122$:
                           1265 ;src/qs1r_main.c:178: return fpga_load_begin ();
                           1266 ;     genCall
                           1267 ;     genRet
                           1268 ;	Peephole 251.a	replaced ljmp to ret with ret
                           1269 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0EF6 02 02 1E           1270 	ljmp	_fpga_load_begin
                           1271 ;src/qs1r_main.c:180: case FL_XFER:
   0EF9                    1272 00123$:
                           1273 ;src/qs1r_main.c:181: get_ep0_data ();
                           1274 ;     genCall
   0EF9 12 0D 6E           1275 	lcall	_get_ep0_data
                           1276 ;src/qs1r_main.c:182: return fpga_load_xfer (EP0BUF, EP0BCL);
                           1277 ;     genAssign
   0EFC 90 E6 8B           1278 	mov	dptr,#_EP0BCL
   0EFF E0                 1279 	movx	a,@dptr
   0F00 F5 0E              1280 	mov	_fpga_load_xfer_PARM_2,a
                           1281 ;     genCall
                           1282 ;	Peephole 182.a	used 16 bit load of DPTR
   0F02 90 E7 40           1283 	mov	dptr,#_EP0BUF
                           1284 ;     genRet
                           1285 ;	Peephole 251.a	replaced ljmp to ret with ret
                           1286 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0F05 02 02 A9           1287 	ljmp	_fpga_load_xfer
                           1288 ;src/qs1r_main.c:184: case FL_END:
   0F08                    1289 00124$:
                           1290 ;src/qs1r_main.c:185: return fpga_load_end ();
                           1291 ;     genCall
                           1292 ;     genRet
                           1293 ;	Peephole 251.a	replaced ljmp to ret with ret
                           1294 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0F08 02 02 B9           1295 	ljmp	_fpga_load_end
                           1296 ;src/qs1r_main.c:187: default:
   0F0B                    1297 00125$:
                           1298 ;src/qs1r_main.c:188: return 0;
                           1299 ;     genRet
   0F0B 75 82 00           1300 	mov	dpl,#0x00
                           1301 ;	Peephole 251.a	replaced ljmp to ret with ret
   0F0E 22                 1302 	ret
                           1303 ;src/qs1r_main.c:192: case VRQ_I2C_WRITE:
   0F0F                    1304 00127$:
                           1305 ;src/qs1r_main.c:194: get_ep0_data ();
                           1306 ;     genCall
   0F0F 12 0D 6E           1307 	lcall	_get_ep0_data
                           1308 ;src/qs1r_main.c:195: if (!i2c_write (wValueL, EP0BUF, EP0BCL))
                           1309 ;     genPointerGet
                           1310 ;     genFarPointerGet
   0F12 90 E6 BA           1311 	mov	dptr,#(_SETUPDAT + 0x0002)
   0F15 E0                 1312 	movx	a,@dptr
   0F16 FA                 1313 	mov	r2,a
                           1314 ;     genAddrOf
   0F17 75 2B 40           1315 	mov	_i2c_write_PARM_2,#_EP0BUF
   0F1A 75 2C E7           1316 	mov	(_i2c_write_PARM_2 + 1),#(_EP0BUF >> 8)
                           1317 ;     genAssign
   0F1D 90 E6 8B           1318 	mov	dptr,#_EP0BCL
   0F20 E0                 1319 	movx	a,@dptr
   0F21 F5 2D              1320 	mov	_i2c_write_PARM_3,a
                           1321 ;     genCall
   0F23 8A 82              1322 	mov	dpl,r2
   0F25 12 04 1E           1323 	lcall	_i2c_write
   0F28 E5 82              1324 	mov	a,dpl
                           1325 ;     genIfx
                           1326 ;     genIfxJump
   0F2A 60 03              1327 	jz	00355$
   0F2C 02 16 54           1328 	ljmp	00266$
   0F2F                    1329 00355$:
                           1330 ;src/qs1r_main.c:196: return 0;
                           1331 ;     genRet
   0F2F 75 82 00           1332 	mov	dpl,#0x00
                           1333 ;	Peephole 251.a	replaced ljmp to ret with ret
   0F32 22                 1334 	ret
                           1335 ;src/qs1r_main.c:199: case VRQ_SPI_WRITE:
   0F33                    1336 00130$:
                           1337 ;src/qs1r_main.c:201: get_ep0_data ();
                           1338 ;     genCall
   0F33 12 0D 6E           1339 	lcall	_get_ep0_data
                           1340 ;src/qs1r_main.c:202: if (!spi_write (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, EP0BCL))
                           1341 ;     genPointerGet
                           1342 ;     genFarPointerGet
   0F36 90 E6 BB           1343 	mov	dptr,#(_SETUPDAT + 0x0003)
   0F39 E0                 1344 	movx	a,@dptr
   0F3A FA                 1345 	mov	r2,a
                           1346 ;     genPointerGet
                           1347 ;     genFarPointerGet
   0F3B 90 E6 BA           1348 	mov	dptr,#(_SETUPDAT + 0x0002)
   0F3E E0                 1349 	movx	a,@dptr
   0F3F F5 15              1350 	mov	_spi_write_PARM_2,a
                           1351 ;     genPointerGet
                           1352 ;     genFarPointerGet
   0F41 90 E6 BD           1353 	mov	dptr,#(_SETUPDAT + 0x0005)
   0F44 E0                 1354 	movx	a,@dptr
   0F45 F5 16              1355 	mov	_spi_write_PARM_3,a
                           1356 ;     genPointerGet
                           1357 ;     genFarPointerGet
   0F47 90 E6 BC           1358 	mov	dptr,#(_SETUPDAT + 0x0004)
   0F4A E0                 1359 	movx	a,@dptr
   0F4B F5 17              1360 	mov	_spi_write_PARM_4,a
                           1361 ;     genAddrOf
   0F4D 75 18 40           1362 	mov	_spi_write_PARM_5,#_EP0BUF
   0F50 75 19 E7           1363 	mov	(_spi_write_PARM_5 + 1),#(_EP0BUF >> 8)
                           1364 ;     genAssign
   0F53 90 E6 8B           1365 	mov	dptr,#_EP0BCL
   0F56 E0                 1366 	movx	a,@dptr
   0F57 F5 1A              1367 	mov	_spi_write_PARM_6,a
                           1368 ;     genCall
   0F59 8A 82              1369 	mov	dpl,r2
   0F5B 12 07 29           1370 	lcall	_spi_write
   0F5E E5 82              1371 	mov	a,dpl
                           1372 ;     genIfx
                           1373 ;     genIfxJump
   0F60 60 03              1374 	jz	00356$
   0F62 02 16 54           1375 	ljmp	00266$
   0F65                    1376 00356$:
                           1377 ;src/qs1r_main.c:203: return 0;
                           1378 ;     genRet
   0F65 75 82 00           1379 	mov	dpl,#0x00
                           1380 ;	Peephole 251.a	replaced ljmp to ret with ret
   0F68 22                 1381 	ret
                           1382 ;src/qs1r_main.c:206: case VRQ_I2C_SPEED_SET:
   0F69                    1383 00133$:
                           1384 ;src/qs1r_main.c:207: if (wValueL == 1)
                           1385 ;     genPointerGet
                           1386 ;     genFarPointerGet
   0F69 90 E6 BA           1387 	mov	dptr,#(_SETUPDAT + 0x0002)
   0F6C E0                 1388 	movx	a,@dptr
   0F6D FA                 1389 	mov	r2,a
                           1390 ;     genCmpEq
                           1391 ;	Peephole 112.b	changed ljmp to sjmp
                           1392 ;	Peephole 199	optimized misc jump sequence
   0F6E BA 01 0B           1393 	cjne	r2,#0x01,00135$
                           1394 ;00357$:
                           1395 ;	Peephole 200	removed redundant sjmp
   0F71                    1396 00358$:
                           1397 ;src/qs1r_main.c:208: I2CTL |= bm400KHZ;
                           1398 ;     genAssign
                           1399 ;     genOr
                           1400 ;	Peephole 248.a	optimized or to xdata
   0F71 90 E6 7A           1401 	mov	dptr,#_I2CTL
   0F74 E0                 1402 	movx	a,@dptr
   0F75 FA                 1403 	mov	r2,a
   0F76 44 01              1404 	orl	a,#0x01
   0F78 F0                 1405 	movx	@dptr,a
   0F79 02 16 54           1406 	ljmp	00266$
   0F7C                    1407 00135$:
                           1408 ;src/qs1r_main.c:210: I2CTL &= ~bm400KHZ;
                           1409 ;     genAssign
                           1410 ;     genAnd
                           1411 ;	Peephole 248.b	optimized and to xdata
   0F7C 90 E6 7A           1412 	mov	dptr,#_I2CTL
   0F7F E0                 1413 	movx	a,@dptr
   0F80 FA                 1414 	mov	r2,a
   0F81 54 FE              1415 	anl	a,#0xFE
   0F83 F0                 1416 	movx	@dptr,a
                           1417 ;src/qs1r_main.c:211: break;
   0F84 02 16 54           1418 	ljmp	00266$
                           1419 ;src/qs1r_main.c:213: case VRQ_CPU_SPEED_SET:
   0F87                    1420 00137$:
                           1421 ;src/qs1r_main.c:214: if (wValueL == 0)
                           1422 ;     genPointerGet
                           1423 ;     genFarPointerGet
   0F87 90 E6 BA           1424 	mov	dptr,#(_SETUPDAT + 0x0002)
   0F8A E0                 1425 	movx	a,@dptr
                           1426 ;     genCmpEq
                           1427 ;	Peephole 115.b	jump optimization
   0F8B FA                 1428 	mov	r2,a
   0F8C 60 02              1429 	jz	00360$
   0F8E                    1430 00359$:
                           1431 ;	Peephole 112.b	changed ljmp to sjmp
   0F8E 80 09              1432 	sjmp	00142$
   0F90                    1433 00360$:
                           1434 ;src/qs1r_main.c:215: CPUCS = bmCLKOE; // 12 MHz
                           1435 ;     genAssign
   0F90 90 E6 00           1436 	mov	dptr,#_CPUCS
   0F93 74 02              1437 	mov	a,#0x02
   0F95 F0                 1438 	movx	@dptr,a
   0F96 02 16 54           1439 	ljmp	00266$
   0F99                    1440 00142$:
                           1441 ;src/qs1r_main.c:216: else if (wValueL == 1)
                           1442 ;     genPointerGet
                           1443 ;     genFarPointerGet
   0F99 90 E6 BA           1444 	mov	dptr,#(_SETUPDAT + 0x0002)
   0F9C E0                 1445 	movx	a,@dptr
   0F9D FA                 1446 	mov	r2,a
                           1447 ;     genCmpEq
                           1448 ;	Peephole 112.b	changed ljmp to sjmp
                           1449 ;	Peephole 199	optimized misc jump sequence
   0F9E BA 01 09           1450 	cjne	r2,#0x01,00139$
                           1451 ;00361$:
                           1452 ;	Peephole 200	removed redundant sjmp
   0FA1                    1453 00362$:
                           1454 ;src/qs1r_main.c:217: CPUCS = bmCLKSPD0 | bmCLKOE; // 24 MHz
                           1455 ;     genAssign
   0FA1 90 E6 00           1456 	mov	dptr,#_CPUCS
   0FA4 74 0A              1457 	mov	a,#0x0A
   0FA6 F0                 1458 	movx	@dptr,a
   0FA7 02 16 54           1459 	ljmp	00266$
   0FAA                    1460 00139$:
                           1461 ;src/qs1r_main.c:219: CPUCS = bmCLKSPD1 | bmCLKOE; // 48 MHz
                           1462 ;     genAssign
   0FAA 90 E6 00           1463 	mov	dptr,#_CPUCS
   0FAD 74 12              1464 	mov	a,#0x12
   0FAF F0                 1465 	movx	@dptr,a
                           1466 ;src/qs1r_main.c:220: break;
   0FB0 02 16 54           1467 	ljmp	00266$
                           1468 ;src/qs1r_main.c:222: case VRQ_DDC_MCR_SET:
   0FB3                    1469 00144$:
                           1470 ;src/qs1r_main.c:223: get_ep0_data ();
                           1471 ;     genCall
   0FB3 12 0D 6E           1472 	lcall	_get_ep0_data
                           1473 ;src/qs1r_main.c:224: if (wIndexL == 0) {
                           1474 ;     genPointerGet
                           1475 ;     genFarPointerGet
   0FB6 90 E6 BC           1476 	mov	dptr,#(_SETUPDAT + 0x0004)
   0FB9 E0                 1477 	movx	a,@dptr
                           1478 ;     genCmpEq
                           1479 ;	Peephole 115.b	jump optimization
   0FBA FA                 1480 	mov	r2,a
   0FBB 60 02              1481 	jz	00364$
   0FBD                    1482 00363$:
                           1483 ;	Peephole 112.b	changed ljmp to sjmp
   0FBD 80 17              1484 	sjmp	00149$
   0FBF                    1485 00364$:
                           1486 ;src/qs1r_main.c:225: DDC0_AMR = (MCR >> 8);
                           1487 ;     genAssign
   0FBF 90 60 07           1488 	mov	dptr,#_DDC0_AMR
   0FC2 74 03              1489 	mov	a,#0x03
   0FC4 F0                 1490 	movx	@dptr,a
                           1491 ;src/qs1r_main.c:226: DDC0_LAR = (MCR & 0xFF);
                           1492 ;     genAssign
   0FC5 90 60 06           1493 	mov	dptr,#_DDC0_LAR
                           1494 ;	Peephole 181	changed mov to clr
   0FC8 E4                 1495 	clr	a
   0FC9 F0                 1496 	movx	@dptr,a
                           1497 ;src/qs1r_main.c:227: DDC0_DR0 = EP0BUF[0];
                           1498 ;     genPointerGet
                           1499 ;     genFarPointerGet
   0FCA 90 E7 40           1500 	mov	dptr,#_EP0BUF
   0FCD E0                 1501 	movx	a,@dptr
                           1502 ;     genAssign
                           1503 ;	Peephole 100	removed redundant mov
   0FCE FA                 1504 	mov	r2,a
   0FCF 90 60 00           1505 	mov	dptr,#_DDC0_DR0
   0FD2 F0                 1506 	movx	@dptr,a
   0FD3 02 16 54           1507 	ljmp	00266$
   0FD6                    1508 00149$:
                           1509 ;src/qs1r_main.c:228: } else if (wIndexL == 1) {
                           1510 ;     genPointerGet
                           1511 ;     genFarPointerGet
   0FD6 90 E6 BC           1512 	mov	dptr,#(_SETUPDAT + 0x0004)
   0FD9 E0                 1513 	movx	a,@dptr
   0FDA FA                 1514 	mov	r2,a
                           1515 ;     genCmpEq
                           1516 ;	Peephole 112.b	changed ljmp to sjmp
                           1517 ;	Peephole 199	optimized misc jump sequence
   0FDB BA 01 17           1518 	cjne	r2,#0x01,00146$
                           1519 ;00365$:
                           1520 ;	Peephole 200	removed redundant sjmp
   0FDE                    1521 00366$:
                           1522 ;src/qs1r_main.c:229: DDC1_AMR = (MCR >> 8);
                           1523 ;     genAssign
   0FDE 90 60 17           1524 	mov	dptr,#_DDC1_AMR
   0FE1 74 03              1525 	mov	a,#0x03
   0FE3 F0                 1526 	movx	@dptr,a
                           1527 ;src/qs1r_main.c:230: DDC1_LAR = (MCR & 0xFF);
                           1528 ;     genAssign
   0FE4 90 60 16           1529 	mov	dptr,#_DDC1_LAR
                           1530 ;	Peephole 181	changed mov to clr
   0FE7 E4                 1531 	clr	a
   0FE8 F0                 1532 	movx	@dptr,a
                           1533 ;src/qs1r_main.c:231: DDC1_DR0 = EP0BUF[0];
                           1534 ;     genPointerGet
                           1535 ;     genFarPointerGet
   0FE9 90 E7 40           1536 	mov	dptr,#_EP0BUF
   0FEC E0                 1537 	movx	a,@dptr
                           1538 ;     genAssign
                           1539 ;	Peephole 100	removed redundant mov
   0FED FA                 1540 	mov	r2,a
   0FEE 90 60 10           1541 	mov	dptr,#_DDC1_DR0
   0FF1 F0                 1542 	movx	@dptr,a
   0FF2 02 16 54           1543 	ljmp	00266$
   0FF5                    1544 00146$:
                           1545 ;src/qs1r_main.c:232: } else return 0;
                           1546 ;     genRet
   0FF5 75 82 00           1547 	mov	dpl,#0x00
                           1548 ;	Peephole 251.a	replaced ljmp to ret with ret
   0FF8 22                 1549 	ret
                           1550 ;src/qs1r_main.c:235: case VRQ_DDC_NCR_SET:
   0FF9                    1551 00151$:
                           1552 ;src/qs1r_main.c:236: get_ep0_data ();
                           1553 ;     genCall
   0FF9 12 0D 6E           1554 	lcall	_get_ep0_data
                           1555 ;src/qs1r_main.c:237: if (wIndexL == 0) {
                           1556 ;     genPointerGet
                           1557 ;     genFarPointerGet
   0FFC 90 E6 BC           1558 	mov	dptr,#(_SETUPDAT + 0x0004)
   0FFF E0                 1559 	movx	a,@dptr
                           1560 ;     genCmpEq
                           1561 ;	Peephole 115.b	jump optimization
   1000 FA                 1562 	mov	r2,a
   1001 60 02              1563 	jz	00368$
   1003                    1564 00367$:
                           1565 ;	Peephole 112.b	changed ljmp to sjmp
   1003 80 18              1566 	sjmp	00156$
   1005                    1567 00368$:
                           1568 ;src/qs1r_main.c:238: DDC0_AMR = (NCR >> 8);
                           1569 ;     genAssign
   1005 90 60 07           1570 	mov	dptr,#_DDC0_AMR
   1008 74 03              1571 	mov	a,#0x03
   100A F0                 1572 	movx	@dptr,a
                           1573 ;src/qs1r_main.c:239: DDC0_LAR = (NCR & 0xFF);
                           1574 ;     genAssign
   100B 90 60 06           1575 	mov	dptr,#_DDC0_LAR
   100E 74 01              1576 	mov	a,#0x01
   1010 F0                 1577 	movx	@dptr,a
                           1578 ;src/qs1r_main.c:240: DDC0_DR0 = EP0BUF[0];
                           1579 ;     genPointerGet
                           1580 ;     genFarPointerGet
   1011 90 E7 40           1581 	mov	dptr,#_EP0BUF
   1014 E0                 1582 	movx	a,@dptr
                           1583 ;     genAssign
                           1584 ;	Peephole 100	removed redundant mov
   1015 FA                 1585 	mov	r2,a
   1016 90 60 00           1586 	mov	dptr,#_DDC0_DR0
   1019 F0                 1587 	movx	@dptr,a
   101A 02 16 54           1588 	ljmp	00266$
   101D                    1589 00156$:
                           1590 ;src/qs1r_main.c:241: } else if (wIndexL == 1) {
                           1591 ;     genPointerGet
                           1592 ;     genFarPointerGet
   101D 90 E6 BC           1593 	mov	dptr,#(_SETUPDAT + 0x0004)
   1020 E0                 1594 	movx	a,@dptr
   1021 FA                 1595 	mov	r2,a
                           1596 ;     genCmpEq
                           1597 ;	Peephole 112.b	changed ljmp to sjmp
                           1598 ;	Peephole 199	optimized misc jump sequence
   1022 BA 01 18           1599 	cjne	r2,#0x01,00153$
                           1600 ;00369$:
                           1601 ;	Peephole 200	removed redundant sjmp
   1025                    1602 00370$:
                           1603 ;src/qs1r_main.c:242: DDC1_AMR = (NCR >> 8);
                           1604 ;     genAssign
   1025 90 60 17           1605 	mov	dptr,#_DDC1_AMR
   1028 74 03              1606 	mov	a,#0x03
   102A F0                 1607 	movx	@dptr,a
                           1608 ;src/qs1r_main.c:243: DDC1_LAR = (NCR & 0xFF);
                           1609 ;     genAssign
   102B 90 60 16           1610 	mov	dptr,#_DDC1_LAR
   102E 74 01              1611 	mov	a,#0x01
   1030 F0                 1612 	movx	@dptr,a
                           1613 ;src/qs1r_main.c:244: DDC1_DR0 = EP0BUF[0];
                           1614 ;     genPointerGet
                           1615 ;     genFarPointerGet
   1031 90 E7 40           1616 	mov	dptr,#_EP0BUF
   1034 E0                 1617 	movx	a,@dptr
                           1618 ;     genAssign
                           1619 ;	Peephole 100	removed redundant mov
   1035 FA                 1620 	mov	r2,a
   1036 90 60 10           1621 	mov	dptr,#_DDC1_DR0
   1039 F0                 1622 	movx	@dptr,a
   103A 02 16 54           1623 	ljmp	00266$
   103D                    1624 00153$:
                           1625 ;src/qs1r_main.c:245: } else return 0;
                           1626 ;     genRet
   103D 75 82 00           1627 	mov	dpl,#0x00
                           1628 ;	Peephole 251.a	replaced ljmp to ret with ret
   1040 22                 1629 	ret
                           1630 ;src/qs1r_main.c:248: case VRQ_DDC_NSCR_SET:
   1041                    1631 00158$:
                           1632 ;src/qs1r_main.c:249: get_ep0_data ();
                           1633 ;     genCall
   1041 12 0D 6E           1634 	lcall	_get_ep0_data
                           1635 ;src/qs1r_main.c:250: if (wIndexL == 0) {
                           1636 ;     genPointerGet
                           1637 ;     genFarPointerGet
   1044 90 E6 BC           1638 	mov	dptr,#(_SETUPDAT + 0x0004)
   1047 E0                 1639 	movx	a,@dptr
                           1640 ;     genCmpEq
                           1641 ;	Peephole 115.b	jump optimization
   1048 FA                 1642 	mov	r2,a
   1049 60 02              1643 	jz	00372$
   104B                    1644 00371$:
                           1645 ;	Peephole 112.b	changed ljmp to sjmp
   104B 80 33              1646 	sjmp	00163$
   104D                    1647 00372$:
                           1648 ;src/qs1r_main.c:251: DDC0_AMR = (NSCR >> 8);
                           1649 ;     genAssign
   104D 90 60 07           1650 	mov	dptr,#_DDC0_AMR
   1050 74 03              1651 	mov	a,#0x03
   1052 F0                 1652 	movx	@dptr,a
                           1653 ;src/qs1r_main.c:252: DDC0_LAR = (NSCR & 0xFF);
                           1654 ;     genAssign
   1053 90 60 06           1655 	mov	dptr,#_DDC0_LAR
   1056 74 02              1656 	mov	a,#0x02
   1058 F0                 1657 	movx	@dptr,a
                           1658 ;src/qs1r_main.c:253: DDC0_DR3 = EP0BUF[3];
                           1659 ;     genPointerGet
                           1660 ;     genFarPointerGet
   1059 90 E7 43           1661 	mov	dptr,#(_EP0BUF + 0x0003)
   105C E0                 1662 	movx	a,@dptr
                           1663 ;     genAssign
                           1664 ;	Peephole 100	removed redundant mov
   105D FA                 1665 	mov	r2,a
   105E 90 60 03           1666 	mov	dptr,#_DDC0_DR3
   1061 F0                 1667 	movx	@dptr,a
                           1668 ;src/qs1r_main.c:254: DDC0_DR2 = EP0BUF[2];
                           1669 ;     genPointerGet
                           1670 ;     genFarPointerGet
   1062 90 E7 42           1671 	mov	dptr,#(_EP0BUF + 0x0002)
   1065 E0                 1672 	movx	a,@dptr
                           1673 ;     genAssign
                           1674 ;	Peephole 100	removed redundant mov
   1066 FA                 1675 	mov	r2,a
   1067 90 60 02           1676 	mov	dptr,#_DDC0_DR2
   106A F0                 1677 	movx	@dptr,a
                           1678 ;src/qs1r_main.c:255: DDC0_DR1 = EP0BUF[1];
                           1679 ;     genPointerGet
                           1680 ;     genFarPointerGet
   106B 90 E7 41           1681 	mov	dptr,#(_EP0BUF + 0x0001)
   106E E0                 1682 	movx	a,@dptr
                           1683 ;     genAssign
                           1684 ;	Peephole 100	removed redundant mov
   106F FA                 1685 	mov	r2,a
   1070 90 60 01           1686 	mov	dptr,#_DDC0_DR1
   1073 F0                 1687 	movx	@dptr,a
                           1688 ;src/qs1r_main.c:256: DDC0_DR0 = EP0BUF[0];
                           1689 ;     genPointerGet
                           1690 ;     genFarPointerGet
   1074 90 E7 40           1691 	mov	dptr,#_EP0BUF
   1077 E0                 1692 	movx	a,@dptr
                           1693 ;     genAssign
                           1694 ;	Peephole 100	removed redundant mov
   1078 FA                 1695 	mov	r2,a
   1079 90 60 00           1696 	mov	dptr,#_DDC0_DR0
   107C F0                 1697 	movx	@dptr,a
   107D 02 16 54           1698 	ljmp	00266$
   1080                    1699 00163$:
                           1700 ;src/qs1r_main.c:257: } else if (wIndexL == 1) {
                           1701 ;     genPointerGet
                           1702 ;     genFarPointerGet
   1080 90 E6 BC           1703 	mov	dptr,#(_SETUPDAT + 0x0004)
   1083 E0                 1704 	movx	a,@dptr
   1084 FA                 1705 	mov	r2,a
                           1706 ;     genCmpEq
                           1707 ;	Peephole 112.b	changed ljmp to sjmp
                           1708 ;	Peephole 199	optimized misc jump sequence
   1085 BA 01 33           1709 	cjne	r2,#0x01,00160$
                           1710 ;00373$:
                           1711 ;	Peephole 200	removed redundant sjmp
   1088                    1712 00374$:
                           1713 ;src/qs1r_main.c:258: DDC1_AMR = (NSCR >> 8);
                           1714 ;     genAssign
   1088 90 60 17           1715 	mov	dptr,#_DDC1_AMR
   108B 74 03              1716 	mov	a,#0x03
   108D F0                 1717 	movx	@dptr,a
                           1718 ;src/qs1r_main.c:259: DDC1_LAR = (NSCR & 0xFF);
                           1719 ;     genAssign
   108E 90 60 16           1720 	mov	dptr,#_DDC1_LAR
   1091 74 02              1721 	mov	a,#0x02
   1093 F0                 1722 	movx	@dptr,a
                           1723 ;src/qs1r_main.c:260: DDC1_DR3 = EP0BUF[3];
                           1724 ;     genPointerGet
                           1725 ;     genFarPointerGet
   1094 90 E7 43           1726 	mov	dptr,#(_EP0BUF + 0x0003)
   1097 E0                 1727 	movx	a,@dptr
                           1728 ;     genAssign
                           1729 ;	Peephole 100	removed redundant mov
   1098 FA                 1730 	mov	r2,a
   1099 90 60 13           1731 	mov	dptr,#_DDC1_DR3
   109C F0                 1732 	movx	@dptr,a
                           1733 ;src/qs1r_main.c:261: DDC1_DR2 = EP0BUF[2];
                           1734 ;     genPointerGet
                           1735 ;     genFarPointerGet
   109D 90 E7 42           1736 	mov	dptr,#(_EP0BUF + 0x0002)
   10A0 E0                 1737 	movx	a,@dptr
                           1738 ;     genAssign
                           1739 ;	Peephole 100	removed redundant mov
   10A1 FA                 1740 	mov	r2,a
   10A2 90 60 12           1741 	mov	dptr,#_DDC1_DR2
   10A5 F0                 1742 	movx	@dptr,a
                           1743 ;src/qs1r_main.c:262: DDC1_DR1 = EP0BUF[1];
                           1744 ;     genPointerGet
                           1745 ;     genFarPointerGet
   10A6 90 E7 41           1746 	mov	dptr,#(_EP0BUF + 0x0001)
   10A9 E0                 1747 	movx	a,@dptr
                           1748 ;     genAssign
                           1749 ;	Peephole 100	removed redundant mov
   10AA FA                 1750 	mov	r2,a
   10AB 90 60 11           1751 	mov	dptr,#_DDC1_DR1
   10AE F0                 1752 	movx	@dptr,a
                           1753 ;src/qs1r_main.c:263: DDC1_DR0 = EP0BUF[0];
                           1754 ;     genPointerGet
                           1755 ;     genFarPointerGet
   10AF 90 E7 40           1756 	mov	dptr,#_EP0BUF
   10B2 E0                 1757 	movx	a,@dptr
                           1758 ;     genAssign
                           1759 ;	Peephole 100	removed redundant mov
   10B3 FA                 1760 	mov	r2,a
   10B4 90 60 10           1761 	mov	dptr,#_DDC1_DR0
   10B7 F0                 1762 	movx	@dptr,a
   10B8 02 16 54           1763 	ljmp	00266$
   10BB                    1764 00160$:
                           1765 ;src/qs1r_main.c:264: } else return 0;
                           1766 ;     genRet
   10BB 75 82 00           1767 	mov	dpl,#0x00
                           1768 ;	Peephole 251.a	replaced ljmp to ret with ret
   10BE 22                 1769 	ret
                           1770 ;src/qs1r_main.c:267: case VRQ_DDC_NFRE_SET:
   10BF                    1771 00165$:
                           1772 ;src/qs1r_main.c:268: get_ep0_data ();
                           1773 ;     genCall
   10BF 12 0D 6E           1774 	lcall	_get_ep0_data
                           1775 ;src/qs1r_main.c:269: if (wIndexL == 0) {
                           1776 ;     genPointerGet
                           1777 ;     genFarPointerGet
   10C2 90 E6 BC           1778 	mov	dptr,#(_SETUPDAT + 0x0004)
   10C5 E0                 1779 	movx	a,@dptr
                           1780 ;     genCmpEq
                           1781 ;	Peephole 115.b	jump optimization
   10C6 FA                 1782 	mov	r2,a
   10C7 60 02              1783 	jz	00376$
   10C9                    1784 00375$:
                           1785 ;	Peephole 112.b	changed ljmp to sjmp
   10C9 80 33              1786 	sjmp	00170$
   10CB                    1787 00376$:
                           1788 ;src/qs1r_main.c:270: DDC0_AMR = (NFRE >> 8);
                           1789 ;     genAssign
   10CB 90 60 07           1790 	mov	dptr,#_DDC0_AMR
   10CE 74 03              1791 	mov	a,#0x03
   10D0 F0                 1792 	movx	@dptr,a
                           1793 ;src/qs1r_main.c:271: DDC0_LAR = (NFRE & 0xFF);
                           1794 ;     genAssign
   10D1 90 60 06           1795 	mov	dptr,#_DDC0_LAR
   10D4 74 03              1796 	mov	a,#0x03
   10D6 F0                 1797 	movx	@dptr,a
                           1798 ;src/qs1r_main.c:272: DDC0_DR3 = EP0BUF[3];
                           1799 ;     genPointerGet
                           1800 ;     genFarPointerGet
   10D7 90 E7 43           1801 	mov	dptr,#(_EP0BUF + 0x0003)
   10DA E0                 1802 	movx	a,@dptr
                           1803 ;     genAssign
                           1804 ;	Peephole 100	removed redundant mov
   10DB FA                 1805 	mov	r2,a
   10DC 90 60 03           1806 	mov	dptr,#_DDC0_DR3
   10DF F0                 1807 	movx	@dptr,a
                           1808 ;src/qs1r_main.c:273: DDC0_DR2 = EP0BUF[2];
                           1809 ;     genPointerGet
                           1810 ;     genFarPointerGet
   10E0 90 E7 42           1811 	mov	dptr,#(_EP0BUF + 0x0002)
   10E3 E0                 1812 	movx	a,@dptr
                           1813 ;     genAssign
                           1814 ;	Peephole 100	removed redundant mov
   10E4 FA                 1815 	mov	r2,a
   10E5 90 60 02           1816 	mov	dptr,#_DDC0_DR2
   10E8 F0                 1817 	movx	@dptr,a
                           1818 ;src/qs1r_main.c:274: DDC0_DR1 = EP0BUF[1];
                           1819 ;     genPointerGet
                           1820 ;     genFarPointerGet
   10E9 90 E7 41           1821 	mov	dptr,#(_EP0BUF + 0x0001)
   10EC E0                 1822 	movx	a,@dptr
                           1823 ;     genAssign
                           1824 ;	Peephole 100	removed redundant mov
   10ED FA                 1825 	mov	r2,a
   10EE 90 60 01           1826 	mov	dptr,#_DDC0_DR1
   10F1 F0                 1827 	movx	@dptr,a
                           1828 ;src/qs1r_main.c:275: DDC0_DR0 = EP0BUF[0];
                           1829 ;     genPointerGet
                           1830 ;     genFarPointerGet
   10F2 90 E7 40           1831 	mov	dptr,#_EP0BUF
   10F5 E0                 1832 	movx	a,@dptr
                           1833 ;     genAssign
                           1834 ;	Peephole 100	removed redundant mov
   10F6 FA                 1835 	mov	r2,a
   10F7 90 60 00           1836 	mov	dptr,#_DDC0_DR0
   10FA F0                 1837 	movx	@dptr,a
   10FB 02 16 54           1838 	ljmp	00266$
   10FE                    1839 00170$:
                           1840 ;src/qs1r_main.c:276: } else if (wIndexL == 1) {
                           1841 ;     genPointerGet
                           1842 ;     genFarPointerGet
   10FE 90 E6 BC           1843 	mov	dptr,#(_SETUPDAT + 0x0004)
   1101 E0                 1844 	movx	a,@dptr
   1102 FA                 1845 	mov	r2,a
                           1846 ;     genCmpEq
                           1847 ;	Peephole 112.b	changed ljmp to sjmp
                           1848 ;	Peephole 199	optimized misc jump sequence
   1103 BA 01 33           1849 	cjne	r2,#0x01,00167$
                           1850 ;00377$:
                           1851 ;	Peephole 200	removed redundant sjmp
   1106                    1852 00378$:
                           1853 ;src/qs1r_main.c:277: DDC1_AMR = (NFRE >> 8);
                           1854 ;     genAssign
   1106 90 60 17           1855 	mov	dptr,#_DDC1_AMR
   1109 74 03              1856 	mov	a,#0x03
   110B F0                 1857 	movx	@dptr,a
                           1858 ;src/qs1r_main.c:278: DDC1_LAR = (NFRE & 0xFF);
                           1859 ;     genAssign
   110C 90 60 16           1860 	mov	dptr,#_DDC1_LAR
   110F 74 03              1861 	mov	a,#0x03
   1111 F0                 1862 	movx	@dptr,a
                           1863 ;src/qs1r_main.c:279: DDC1_DR3 = EP0BUF[3];
                           1864 ;     genPointerGet
                           1865 ;     genFarPointerGet
   1112 90 E7 43           1866 	mov	dptr,#(_EP0BUF + 0x0003)
   1115 E0                 1867 	movx	a,@dptr
                           1868 ;     genAssign
                           1869 ;	Peephole 100	removed redundant mov
   1116 FA                 1870 	mov	r2,a
   1117 90 60 13           1871 	mov	dptr,#_DDC1_DR3
   111A F0                 1872 	movx	@dptr,a
                           1873 ;src/qs1r_main.c:280: DDC1_DR2 = EP0BUF[2];
                           1874 ;     genPointerGet
                           1875 ;     genFarPointerGet
   111B 90 E7 42           1876 	mov	dptr,#(_EP0BUF + 0x0002)
   111E E0                 1877 	movx	a,@dptr
                           1878 ;     genAssign
                           1879 ;	Peephole 100	removed redundant mov
   111F FA                 1880 	mov	r2,a
   1120 90 60 12           1881 	mov	dptr,#_DDC1_DR2
   1123 F0                 1882 	movx	@dptr,a
                           1883 ;src/qs1r_main.c:281: DDC1_DR1 = EP0BUF[1];
                           1884 ;     genPointerGet
                           1885 ;     genFarPointerGet
   1124 90 E7 41           1886 	mov	dptr,#(_EP0BUF + 0x0001)
   1127 E0                 1887 	movx	a,@dptr
                           1888 ;     genAssign
                           1889 ;	Peephole 100	removed redundant mov
   1128 FA                 1890 	mov	r2,a
   1129 90 60 11           1891 	mov	dptr,#_DDC1_DR1
   112C F0                 1892 	movx	@dptr,a
                           1893 ;src/qs1r_main.c:282: DDC1_DR0 = EP0BUF[0];
                           1894 ;     genPointerGet
                           1895 ;     genFarPointerGet
   112D 90 E7 40           1896 	mov	dptr,#_EP0BUF
   1130 E0                 1897 	movx	a,@dptr
                           1898 ;     genAssign
                           1899 ;	Peephole 100	removed redundant mov
   1131 FA                 1900 	mov	r2,a
   1132 90 60 10           1901 	mov	dptr,#_DDC1_DR0
   1135 F0                 1902 	movx	@dptr,a
   1136 02 16 54           1903 	ljmp	00266$
   1139                    1904 00167$:
                           1905 ;src/qs1r_main.c:283: } else return 0;
                           1906 ;     genRet
   1139 75 82 00           1907 	mov	dpl,#0x00
                           1908 ;	Peephole 251.a	replaced ljmp to ret with ret
   113C 22                 1909 	ret
                           1910 ;src/qs1r_main.c:286: case VRQ_DDC_NPO_SET:
   113D                    1911 00172$:
                           1912 ;src/qs1r_main.c:287: get_ep0_data ();
                           1913 ;     genCall
   113D 12 0D 6E           1914 	lcall	_get_ep0_data
                           1915 ;src/qs1r_main.c:288: if (wIndexL == 0) {
                           1916 ;     genPointerGet
                           1917 ;     genFarPointerGet
   1140 90 E6 BC           1918 	mov	dptr,#(_SETUPDAT + 0x0004)
   1143 E0                 1919 	movx	a,@dptr
                           1920 ;     genCmpEq
                           1921 ;	Peephole 115.b	jump optimization
   1144 FA                 1922 	mov	r2,a
   1145 60 02              1923 	jz	00380$
   1147                    1924 00379$:
                           1925 ;	Peephole 112.b	changed ljmp to sjmp
   1147 80 21              1926 	sjmp	00177$
   1149                    1927 00380$:
                           1928 ;src/qs1r_main.c:289: DDC0_AMR = (NPHA >> 8);
                           1929 ;     genAssign
   1149 90 60 07           1930 	mov	dptr,#_DDC0_AMR
   114C 74 03              1931 	mov	a,#0x03
   114E F0                 1932 	movx	@dptr,a
                           1933 ;src/qs1r_main.c:290: DDC0_LAR = (NPHA & 0xFF);
                           1934 ;     genAssign
   114F 90 60 06           1935 	mov	dptr,#_DDC0_LAR
   1152 74 04              1936 	mov	a,#0x04
   1154 F0                 1937 	movx	@dptr,a
                           1938 ;src/qs1r_main.c:291: DDC0_DR1 = EP0BUF[1];
                           1939 ;     genPointerGet
                           1940 ;     genFarPointerGet
   1155 90 E7 41           1941 	mov	dptr,#(_EP0BUF + 0x0001)
   1158 E0                 1942 	movx	a,@dptr
                           1943 ;     genAssign
                           1944 ;	Peephole 100	removed redundant mov
   1159 FA                 1945 	mov	r2,a
   115A 90 60 01           1946 	mov	dptr,#_DDC0_DR1
   115D F0                 1947 	movx	@dptr,a
                           1948 ;src/qs1r_main.c:292: DDC0_DR0 = EP0BUF[0];
                           1949 ;     genPointerGet
                           1950 ;     genFarPointerGet
   115E 90 E7 40           1951 	mov	dptr,#_EP0BUF
   1161 E0                 1952 	movx	a,@dptr
                           1953 ;     genAssign
                           1954 ;	Peephole 100	removed redundant mov
   1162 FA                 1955 	mov	r2,a
   1163 90 60 00           1956 	mov	dptr,#_DDC0_DR0
   1166 F0                 1957 	movx	@dptr,a
   1167 02 16 54           1958 	ljmp	00266$
   116A                    1959 00177$:
                           1960 ;src/qs1r_main.c:293: } else if (wIndexL == 1) {
                           1961 ;     genPointerGet
                           1962 ;     genFarPointerGet
   116A 90 E6 BC           1963 	mov	dptr,#(_SETUPDAT + 0x0004)
   116D E0                 1964 	movx	a,@dptr
   116E FA                 1965 	mov	r2,a
                           1966 ;     genCmpEq
                           1967 ;	Peephole 112.b	changed ljmp to sjmp
                           1968 ;	Peephole 199	optimized misc jump sequence
   116F BA 01 21           1969 	cjne	r2,#0x01,00174$
                           1970 ;00381$:
                           1971 ;	Peephole 200	removed redundant sjmp
   1172                    1972 00382$:
                           1973 ;src/qs1r_main.c:294: DDC1_AMR = (NPHA >> 8);
                           1974 ;     genAssign
   1172 90 60 17           1975 	mov	dptr,#_DDC1_AMR
   1175 74 03              1976 	mov	a,#0x03
   1177 F0                 1977 	movx	@dptr,a
                           1978 ;src/qs1r_main.c:295: DDC1_LAR = (NPHA & 0xFF);
                           1979 ;     genAssign
   1178 90 60 16           1980 	mov	dptr,#_DDC1_LAR
   117B 74 04              1981 	mov	a,#0x04
   117D F0                 1982 	movx	@dptr,a
                           1983 ;src/qs1r_main.c:296: DDC1_DR1 = EP0BUF[1];
                           1984 ;     genPointerGet
                           1985 ;     genFarPointerGet
   117E 90 E7 41           1986 	mov	dptr,#(_EP0BUF + 0x0001)
   1181 E0                 1987 	movx	a,@dptr
                           1988 ;     genAssign
                           1989 ;	Peephole 100	removed redundant mov
   1182 FA                 1990 	mov	r2,a
   1183 90 60 11           1991 	mov	dptr,#_DDC1_DR1
   1186 F0                 1992 	movx	@dptr,a
                           1993 ;src/qs1r_main.c:297: DDC1_DR0 = EP0BUF[0];
                           1994 ;     genPointerGet
                           1995 ;     genFarPointerGet
   1187 90 E7 40           1996 	mov	dptr,#_EP0BUF
   118A E0                 1997 	movx	a,@dptr
                           1998 ;     genAssign
                           1999 ;	Peephole 100	removed redundant mov
   118B FA                 2000 	mov	r2,a
   118C 90 60 10           2001 	mov	dptr,#_DDC1_DR0
   118F F0                 2002 	movx	@dptr,a
   1190 02 16 54           2003 	ljmp	00266$
   1193                    2004 00174$:
                           2005 ;src/qs1r_main.c:298: } else return 0;
                           2006 ;     genRet
   1193 75 82 00           2007 	mov	dpl,#0x00
                           2008 ;	Peephole 251.a	replaced ljmp to ret with ret
   1196 22                 2009 	ret
                           2010 ;src/qs1r_main.c:301: case VRQ_DDC_C2SR_SET:
   1197                    2011 00179$:
                           2012 ;src/qs1r_main.c:302: get_ep0_data ();
                           2013 ;     genCall
   1197 12 0D 6E           2014 	lcall	_get_ep0_data
                           2015 ;src/qs1r_main.c:303: if (wIndexL == 0) {
                           2016 ;     genPointerGet
                           2017 ;     genFarPointerGet
   119A 90 E6 BC           2018 	mov	dptr,#(_SETUPDAT + 0x0004)
   119D E0                 2019 	movx	a,@dptr
                           2020 ;     genCmpEq
                           2021 ;	Peephole 115.b	jump optimization
   119E FA                 2022 	mov	r2,a
   119F 60 02              2023 	jz	00384$
   11A1                    2024 00383$:
                           2025 ;	Peephole 112.b	changed ljmp to sjmp
   11A1 80 18              2026 	sjmp	00184$
   11A3                    2027 00384$:
                           2028 ;src/qs1r_main.c:304: DDC0_AMR = (C2SR >> 8);
                           2029 ;     genAssign
   11A3 90 60 07           2030 	mov	dptr,#_DDC0_AMR
   11A6 74 03              2031 	mov	a,#0x03
   11A8 F0                 2032 	movx	@dptr,a
                           2033 ;src/qs1r_main.c:305: DDC0_LAR = (C2SR & 0xFF);
                           2034 ;     genAssign
   11A9 90 60 06           2035 	mov	dptr,#_DDC0_LAR
   11AC 74 05              2036 	mov	a,#0x05
   11AE F0                 2037 	movx	@dptr,a
                           2038 ;src/qs1r_main.c:306: DDC0_DR0 = EP0BUF[0];
                           2039 ;     genPointerGet
                           2040 ;     genFarPointerGet
   11AF 90 E7 40           2041 	mov	dptr,#_EP0BUF
   11B2 E0                 2042 	movx	a,@dptr
                           2043 ;     genAssign
                           2044 ;	Peephole 100	removed redundant mov
   11B3 FA                 2045 	mov	r2,a
   11B4 90 60 00           2046 	mov	dptr,#_DDC0_DR0
   11B7 F0                 2047 	movx	@dptr,a
   11B8 02 16 54           2048 	ljmp	00266$
   11BB                    2049 00184$:
                           2050 ;src/qs1r_main.c:307: } else if (wIndexL == 1) {
                           2051 ;     genPointerGet
                           2052 ;     genFarPointerGet
   11BB 90 E6 BC           2053 	mov	dptr,#(_SETUPDAT + 0x0004)
   11BE E0                 2054 	movx	a,@dptr
   11BF FA                 2055 	mov	r2,a
                           2056 ;     genCmpEq
                           2057 ;	Peephole 112.b	changed ljmp to sjmp
                           2058 ;	Peephole 199	optimized misc jump sequence
   11C0 BA 01 18           2059 	cjne	r2,#0x01,00181$
                           2060 ;00385$:
                           2061 ;	Peephole 200	removed redundant sjmp
   11C3                    2062 00386$:
                           2063 ;src/qs1r_main.c:308: DDC1_AMR = (C2SR >> 8);
                           2064 ;     genAssign
   11C3 90 60 17           2065 	mov	dptr,#_DDC1_AMR
   11C6 74 03              2066 	mov	a,#0x03
   11C8 F0                 2067 	movx	@dptr,a
                           2068 ;src/qs1r_main.c:309: DDC1_LAR = (C2SR & 0xFF);
                           2069 ;     genAssign
   11C9 90 60 16           2070 	mov	dptr,#_DDC1_LAR
   11CC 74 05              2071 	mov	a,#0x05
   11CE F0                 2072 	movx	@dptr,a
                           2073 ;src/qs1r_main.c:310: DDC1_DR0 = EP0BUF[0];
                           2074 ;     genPointerGet
                           2075 ;     genFarPointerGet
   11CF 90 E7 40           2076 	mov	dptr,#_EP0BUF
   11D2 E0                 2077 	movx	a,@dptr
                           2078 ;     genAssign
                           2079 ;	Peephole 100	removed redundant mov
   11D3 FA                 2080 	mov	r2,a
   11D4 90 60 10           2081 	mov	dptr,#_DDC1_DR0
   11D7 F0                 2082 	movx	@dptr,a
   11D8 02 16 54           2083 	ljmp	00266$
   11DB                    2084 00181$:
                           2085 ;src/qs1r_main.c:311: } else return 0;
                           2086 ;     genRet
   11DB 75 82 00           2087 	mov	dpl,#0x00
                           2088 ;	Peephole 251.a	replaced ljmp to ret with ret
   11DE 22                 2089 	ret
                           2090 ;src/qs1r_main.c:314: case VRQ_DDC_C2DEC_SET:
   11DF                    2091 00186$:
                           2092 ;src/qs1r_main.c:315: get_ep0_data ();
                           2093 ;     genCall
   11DF 12 0D 6E           2094 	lcall	_get_ep0_data
                           2095 ;src/qs1r_main.c:316: if (wIndexL == 0) {
                           2096 ;     genPointerGet
                           2097 ;     genFarPointerGet
   11E2 90 E6 BC           2098 	mov	dptr,#(_SETUPDAT + 0x0004)
   11E5 E0                 2099 	movx	a,@dptr
                           2100 ;     genCmpEq
                           2101 ;	Peephole 115.b	jump optimization
   11E6 FA                 2102 	mov	r2,a
   11E7 60 02              2103 	jz	00388$
   11E9                    2104 00387$:
                           2105 ;	Peephole 112.b	changed ljmp to sjmp
   11E9 80 18              2106 	sjmp	00191$
   11EB                    2107 00388$:
                           2108 ;src/qs1r_main.c:317: DDC0_AMR = (C2DEC >> 8);
                           2109 ;     genAssign
   11EB 90 60 07           2110 	mov	dptr,#_DDC0_AMR
   11EE 74 03              2111 	mov	a,#0x03
   11F0 F0                 2112 	movx	@dptr,a
                           2113 ;src/qs1r_main.c:318: DDC0_LAR = (C2DEC & 0xFF);
                           2114 ;     genAssign
   11F1 90 60 06           2115 	mov	dptr,#_DDC0_LAR
   11F4 74 06              2116 	mov	a,#0x06
   11F6 F0                 2117 	movx	@dptr,a
                           2118 ;src/qs1r_main.c:319: DDC0_DR0 = EP0BUF[0];
                           2119 ;     genPointerGet
                           2120 ;     genFarPointerGet
   11F7 90 E7 40           2121 	mov	dptr,#_EP0BUF
   11FA E0                 2122 	movx	a,@dptr
                           2123 ;     genAssign
                           2124 ;	Peephole 100	removed redundant mov
   11FB FA                 2125 	mov	r2,a
   11FC 90 60 00           2126 	mov	dptr,#_DDC0_DR0
   11FF F0                 2127 	movx	@dptr,a
   1200 02 16 54           2128 	ljmp	00266$
   1203                    2129 00191$:
                           2130 ;src/qs1r_main.c:320: } else if (wIndexL == 1) {
                           2131 ;     genPointerGet
                           2132 ;     genFarPointerGet
   1203 90 E6 BC           2133 	mov	dptr,#(_SETUPDAT + 0x0004)
   1206 E0                 2134 	movx	a,@dptr
   1207 FA                 2135 	mov	r2,a
                           2136 ;     genCmpEq
                           2137 ;	Peephole 112.b	changed ljmp to sjmp
                           2138 ;	Peephole 199	optimized misc jump sequence
   1208 BA 01 18           2139 	cjne	r2,#0x01,00188$
                           2140 ;00389$:
                           2141 ;	Peephole 200	removed redundant sjmp
   120B                    2142 00390$:
                           2143 ;src/qs1r_main.c:321: DDC1_AMR = (C2DEC >> 8);
                           2144 ;     genAssign
   120B 90 60 17           2145 	mov	dptr,#_DDC1_AMR
   120E 74 03              2146 	mov	a,#0x03
   1210 F0                 2147 	movx	@dptr,a
                           2148 ;src/qs1r_main.c:322: DDC1_LAR = (C2DEC & 0xFF);
                           2149 ;     genAssign
   1211 90 60 16           2150 	mov	dptr,#_DDC1_LAR
   1214 74 06              2151 	mov	a,#0x06
   1216 F0                 2152 	movx	@dptr,a
                           2153 ;src/qs1r_main.c:323: DDC1_DR0 = EP0BUF[0];
                           2154 ;     genPointerGet
                           2155 ;     genFarPointerGet
   1217 90 E7 40           2156 	mov	dptr,#_EP0BUF
   121A E0                 2157 	movx	a,@dptr
                           2158 ;     genAssign
                           2159 ;	Peephole 100	removed redundant mov
   121B FA                 2160 	mov	r2,a
   121C 90 60 10           2161 	mov	dptr,#_DDC1_DR0
   121F F0                 2162 	movx	@dptr,a
   1220 02 16 54           2163 	ljmp	00266$
   1223                    2164 00188$:
                           2165 ;src/qs1r_main.c:324: } else return 0;
                           2166 ;     genRet
   1223 75 82 00           2167 	mov	dpl,#0x00
                           2168 ;	Peephole 251.a	replaced ljmp to ret with ret
   1226 22                 2169 	ret
                           2170 ;src/qs1r_main.c:327: case VRQ_DDC_C5SR_SET:
   1227                    2171 00193$:
                           2172 ;src/qs1r_main.c:328: get_ep0_data ();
                           2173 ;     genCall
   1227 12 0D 6E           2174 	lcall	_get_ep0_data
                           2175 ;src/qs1r_main.c:329: if (wIndexL == 0) {
                           2176 ;     genPointerGet
                           2177 ;     genFarPointerGet
   122A 90 E6 BC           2178 	mov	dptr,#(_SETUPDAT + 0x0004)
   122D E0                 2179 	movx	a,@dptr
                           2180 ;     genCmpEq
                           2181 ;	Peephole 115.b	jump optimization
   122E FA                 2182 	mov	r2,a
   122F 60 02              2183 	jz	00392$
   1231                    2184 00391$:
                           2185 ;	Peephole 112.b	changed ljmp to sjmp
   1231 80 18              2186 	sjmp	00198$
   1233                    2187 00392$:
                           2188 ;src/qs1r_main.c:330: DDC0_AMR = (C5SR >> 8);
                           2189 ;     genAssign
   1233 90 60 07           2190 	mov	dptr,#_DDC0_AMR
   1236 74 03              2191 	mov	a,#0x03
   1238 F0                 2192 	movx	@dptr,a
                           2193 ;src/qs1r_main.c:331: DDC0_LAR = (C5SR & 0xFF);
                           2194 ;     genAssign
   1239 90 60 06           2195 	mov	dptr,#_DDC0_LAR
   123C 74 07              2196 	mov	a,#0x07
   123E F0                 2197 	movx	@dptr,a
                           2198 ;src/qs1r_main.c:332: DDC0_DR0 = EP0BUF[0];
                           2199 ;     genPointerGet
                           2200 ;     genFarPointerGet
   123F 90 E7 40           2201 	mov	dptr,#_EP0BUF
   1242 E0                 2202 	movx	a,@dptr
                           2203 ;     genAssign
                           2204 ;	Peephole 100	removed redundant mov
   1243 FA                 2205 	mov	r2,a
   1244 90 60 00           2206 	mov	dptr,#_DDC0_DR0
   1247 F0                 2207 	movx	@dptr,a
   1248 02 16 54           2208 	ljmp	00266$
   124B                    2209 00198$:
                           2210 ;src/qs1r_main.c:333: } else if (wIndexL == 1) {
                           2211 ;     genPointerGet
                           2212 ;     genFarPointerGet
   124B 90 E6 BC           2213 	mov	dptr,#(_SETUPDAT + 0x0004)
   124E E0                 2214 	movx	a,@dptr
   124F FA                 2215 	mov	r2,a
                           2216 ;     genCmpEq
                           2217 ;	Peephole 112.b	changed ljmp to sjmp
                           2218 ;	Peephole 199	optimized misc jump sequence
   1250 BA 01 18           2219 	cjne	r2,#0x01,00195$
                           2220 ;00393$:
                           2221 ;	Peephole 200	removed redundant sjmp
   1253                    2222 00394$:
                           2223 ;src/qs1r_main.c:334: DDC1_AMR = (C5SR >> 8);
                           2224 ;     genAssign
   1253 90 60 17           2225 	mov	dptr,#_DDC1_AMR
   1256 74 03              2226 	mov	a,#0x03
   1258 F0                 2227 	movx	@dptr,a
                           2228 ;src/qs1r_main.c:335: DDC1_LAR = (C5SR & 0xFF);
                           2229 ;     genAssign
   1259 90 60 16           2230 	mov	dptr,#_DDC1_LAR
   125C 74 07              2231 	mov	a,#0x07
   125E F0                 2232 	movx	@dptr,a
                           2233 ;src/qs1r_main.c:336: DDC1_DR0 = EP0BUF[0];
                           2234 ;     genPointerGet
                           2235 ;     genFarPointerGet
   125F 90 E7 40           2236 	mov	dptr,#_EP0BUF
   1262 E0                 2237 	movx	a,@dptr
                           2238 ;     genAssign
                           2239 ;	Peephole 100	removed redundant mov
   1263 FA                 2240 	mov	r2,a
   1264 90 60 10           2241 	mov	dptr,#_DDC1_DR0
   1267 F0                 2242 	movx	@dptr,a
   1268 02 16 54           2243 	ljmp	00266$
   126B                    2244 00195$:
                           2245 ;src/qs1r_main.c:337: } else return 0;
                           2246 ;     genRet
   126B 75 82 00           2247 	mov	dpl,#0x00
                           2248 ;	Peephole 251.a	replaced ljmp to ret with ret
   126E 22                 2249 	ret
                           2250 ;src/qs1r_main.c:340: case VRQ_DDC_C5DEC_SET:
   126F                    2251 00200$:
                           2252 ;src/qs1r_main.c:341: get_ep0_data ();
                           2253 ;     genCall
   126F 12 0D 6E           2254 	lcall	_get_ep0_data
                           2255 ;src/qs1r_main.c:342: if (wIndexL == 0) {
                           2256 ;     genPointerGet
                           2257 ;     genFarPointerGet
   1272 90 E6 BC           2258 	mov	dptr,#(_SETUPDAT + 0x0004)
   1275 E0                 2259 	movx	a,@dptr
                           2260 ;     genCmpEq
                           2261 ;	Peephole 115.b	jump optimization
   1276 FA                 2262 	mov	r2,a
   1277 60 02              2263 	jz	00396$
   1279                    2264 00395$:
                           2265 ;	Peephole 112.b	changed ljmp to sjmp
   1279 80 18              2266 	sjmp	00205$
   127B                    2267 00396$:
                           2268 ;src/qs1r_main.c:343: DDC0_AMR = (C5DEC >> 8);
                           2269 ;     genAssign
   127B 90 60 07           2270 	mov	dptr,#_DDC0_AMR
   127E 74 03              2271 	mov	a,#0x03
   1280 F0                 2272 	movx	@dptr,a
                           2273 ;src/qs1r_main.c:344: DDC0_LAR = (C5DEC & 0xFF);
                           2274 ;     genAssign
   1281 90 60 06           2275 	mov	dptr,#_DDC0_LAR
   1284 74 08              2276 	mov	a,#0x08
   1286 F0                 2277 	movx	@dptr,a
                           2278 ;src/qs1r_main.c:345: DDC0_DR0 = EP0BUF[0];
                           2279 ;     genPointerGet
                           2280 ;     genFarPointerGet
   1287 90 E7 40           2281 	mov	dptr,#_EP0BUF
   128A E0                 2282 	movx	a,@dptr
                           2283 ;     genAssign
                           2284 ;	Peephole 100	removed redundant mov
   128B FA                 2285 	mov	r2,a
   128C 90 60 00           2286 	mov	dptr,#_DDC0_DR0
   128F F0                 2287 	movx	@dptr,a
   1290 02 16 54           2288 	ljmp	00266$
   1293                    2289 00205$:
                           2290 ;src/qs1r_main.c:346: } else if (wIndexL == 1) {
                           2291 ;     genPointerGet
                           2292 ;     genFarPointerGet
   1293 90 E6 BC           2293 	mov	dptr,#(_SETUPDAT + 0x0004)
   1296 E0                 2294 	movx	a,@dptr
   1297 FA                 2295 	mov	r2,a
                           2296 ;     genCmpEq
                           2297 ;	Peephole 112.b	changed ljmp to sjmp
                           2298 ;	Peephole 199	optimized misc jump sequence
   1298 BA 01 18           2299 	cjne	r2,#0x01,00202$
                           2300 ;00397$:
                           2301 ;	Peephole 200	removed redundant sjmp
   129B                    2302 00398$:
                           2303 ;src/qs1r_main.c:347: DDC1_AMR = (C5DEC >> 8);
                           2304 ;     genAssign
   129B 90 60 17           2305 	mov	dptr,#_DDC1_AMR
   129E 74 03              2306 	mov	a,#0x03
   12A0 F0                 2307 	movx	@dptr,a
                           2308 ;src/qs1r_main.c:348: DDC1_LAR = (C5DEC & 0xFF);
                           2309 ;     genAssign
   12A1 90 60 16           2310 	mov	dptr,#_DDC1_LAR
   12A4 74 08              2311 	mov	a,#0x08
   12A6 F0                 2312 	movx	@dptr,a
                           2313 ;src/qs1r_main.c:349: DDC1_DR0 = EP0BUF[0];
                           2314 ;     genPointerGet
                           2315 ;     genFarPointerGet
   12A7 90 E7 40           2316 	mov	dptr,#_EP0BUF
   12AA E0                 2317 	movx	a,@dptr
                           2318 ;     genAssign
                           2319 ;	Peephole 100	removed redundant mov
   12AB FA                 2320 	mov	r2,a
   12AC 90 60 10           2321 	mov	dptr,#_DDC1_DR0
   12AF F0                 2322 	movx	@dptr,a
   12B0 02 16 54           2323 	ljmp	00266$
   12B3                    2324 00202$:
                           2325 ;src/qs1r_main.c:350: } else return 0;
                           2326 ;     genRet
   12B3 75 82 00           2327 	mov	dpl,#0x00
                           2328 ;	Peephole 251.a	replaced ljmp to ret with ret
   12B6 22                 2329 	ret
                           2330 ;src/qs1r_main.c:353: case VRQ_DDC_RCR_SET:
   12B7                    2331 00207$:
                           2332 ;src/qs1r_main.c:354: get_ep0_data ();
                           2333 ;     genCall
   12B7 12 0D 6E           2334 	lcall	_get_ep0_data
                           2335 ;src/qs1r_main.c:355: if (wIndexL == 0) {
                           2336 ;     genPointerGet
                           2337 ;     genFarPointerGet
   12BA 90 E6 BC           2338 	mov	dptr,#(_SETUPDAT + 0x0004)
   12BD E0                 2339 	movx	a,@dptr
                           2340 ;     genCmpEq
                           2341 ;	Peephole 115.b	jump optimization
   12BE FA                 2342 	mov	r2,a
   12BF 60 02              2343 	jz	00400$
   12C1                    2344 00399$:
                           2345 ;	Peephole 112.b	changed ljmp to sjmp
   12C1 80 18              2346 	sjmp	00212$
   12C3                    2347 00400$:
                           2348 ;src/qs1r_main.c:356: DDC0_AMR = (RCR >> 8);
                           2349 ;     genAssign
   12C3 90 60 07           2350 	mov	dptr,#_DDC0_AMR
   12C6 74 03              2351 	mov	a,#0x03
   12C8 F0                 2352 	movx	@dptr,a
                           2353 ;src/qs1r_main.c:357: DDC0_LAR = (RCR & 0xFF);
                           2354 ;     genAssign
   12C9 90 60 06           2355 	mov	dptr,#_DDC0_LAR
   12CC 74 09              2356 	mov	a,#0x09
   12CE F0                 2357 	movx	@dptr,a
                           2358 ;src/qs1r_main.c:358: DDC0_DR0 = EP0BUF[0];
                           2359 ;     genPointerGet
                           2360 ;     genFarPointerGet
   12CF 90 E7 40           2361 	mov	dptr,#_EP0BUF
   12D2 E0                 2362 	movx	a,@dptr
                           2363 ;     genAssign
                           2364 ;	Peephole 100	removed redundant mov
   12D3 FA                 2365 	mov	r2,a
   12D4 90 60 00           2366 	mov	dptr,#_DDC0_DR0
   12D7 F0                 2367 	movx	@dptr,a
   12D8 02 16 54           2368 	ljmp	00266$
   12DB                    2369 00212$:
                           2370 ;src/qs1r_main.c:359: } else if (wIndexL == 1) {
                           2371 ;     genPointerGet
                           2372 ;     genFarPointerGet
   12DB 90 E6 BC           2373 	mov	dptr,#(_SETUPDAT + 0x0004)
   12DE E0                 2374 	movx	a,@dptr
   12DF FA                 2375 	mov	r2,a
                           2376 ;     genCmpEq
                           2377 ;	Peephole 112.b	changed ljmp to sjmp
                           2378 ;	Peephole 199	optimized misc jump sequence
   12E0 BA 01 18           2379 	cjne	r2,#0x01,00209$
                           2380 ;00401$:
                           2381 ;	Peephole 200	removed redundant sjmp
   12E3                    2382 00402$:
                           2383 ;src/qs1r_main.c:360: DDC1_AMR = (RCR >> 8);
                           2384 ;     genAssign
   12E3 90 60 17           2385 	mov	dptr,#_DDC1_AMR
   12E6 74 03              2386 	mov	a,#0x03
   12E8 F0                 2387 	movx	@dptr,a
                           2388 ;src/qs1r_main.c:361: DDC1_LAR = (RCR & 0xFF);
                           2389 ;     genAssign
   12E9 90 60 16           2390 	mov	dptr,#_DDC1_LAR
   12EC 74 09              2391 	mov	a,#0x09
   12EE F0                 2392 	movx	@dptr,a
                           2393 ;src/qs1r_main.c:362: DDC1_DR0 = EP0BUF[0];
                           2394 ;     genPointerGet
                           2395 ;     genFarPointerGet
   12EF 90 E7 40           2396 	mov	dptr,#_EP0BUF
   12F2 E0                 2397 	movx	a,@dptr
                           2398 ;     genAssign
                           2399 ;	Peephole 100	removed redundant mov
   12F3 FA                 2400 	mov	r2,a
   12F4 90 60 10           2401 	mov	dptr,#_DDC1_DR0
   12F7 F0                 2402 	movx	@dptr,a
   12F8 02 16 54           2403 	ljmp	00266$
   12FB                    2404 00209$:
                           2405 ;src/qs1r_main.c:363: } else return 0;
                           2406 ;     genRet
   12FB 75 82 00           2407 	mov	dpl,#0x00
                           2408 ;	Peephole 251.a	replaced ljmp to ret with ret
   12FE 22                 2409 	ret
                           2410 ;src/qs1r_main.c:366: case VRQ_DDC_RDEC_SET:
   12FF                    2411 00214$:
                           2412 ;src/qs1r_main.c:367: get_ep0_data ();
                           2413 ;     genCall
   12FF 12 0D 6E           2414 	lcall	_get_ep0_data
                           2415 ;src/qs1r_main.c:368: if (wIndexL == 0) {
                           2416 ;     genPointerGet
                           2417 ;     genFarPointerGet
   1302 90 E6 BC           2418 	mov	dptr,#(_SETUPDAT + 0x0004)
   1305 E0                 2419 	movx	a,@dptr
                           2420 ;     genCmpEq
                           2421 ;	Peephole 115.b	jump optimization
   1306 FA                 2422 	mov	r2,a
   1307 60 02              2423 	jz	00404$
   1309                    2424 00403$:
                           2425 ;	Peephole 112.b	changed ljmp to sjmp
   1309 80 18              2426 	sjmp	00219$
   130B                    2427 00404$:
                           2428 ;src/qs1r_main.c:369: DDC0_AMR = (RDEC >> 8);
                           2429 ;     genAssign
   130B 90 60 07           2430 	mov	dptr,#_DDC0_AMR
   130E 74 03              2431 	mov	a,#0x03
   1310 F0                 2432 	movx	@dptr,a
                           2433 ;src/qs1r_main.c:370: DDC0_LAR = (RDEC & 0xFF);
                           2434 ;     genAssign
   1311 90 60 06           2435 	mov	dptr,#_DDC0_LAR
   1314 74 0A              2436 	mov	a,#0x0A
   1316 F0                 2437 	movx	@dptr,a
                           2438 ;src/qs1r_main.c:371: DDC0_DR0 = EP0BUF[0];
                           2439 ;     genPointerGet
                           2440 ;     genFarPointerGet
   1317 90 E7 40           2441 	mov	dptr,#_EP0BUF
   131A E0                 2442 	movx	a,@dptr
                           2443 ;     genAssign
                           2444 ;	Peephole 100	removed redundant mov
   131B FA                 2445 	mov	r2,a
   131C 90 60 00           2446 	mov	dptr,#_DDC0_DR0
   131F F0                 2447 	movx	@dptr,a
   1320 02 16 54           2448 	ljmp	00266$
   1323                    2449 00219$:
                           2450 ;src/qs1r_main.c:372: } else if (wIndexL == 1) {
                           2451 ;     genPointerGet
                           2452 ;     genFarPointerGet
   1323 90 E6 BC           2453 	mov	dptr,#(_SETUPDAT + 0x0004)
   1326 E0                 2454 	movx	a,@dptr
   1327 FA                 2455 	mov	r2,a
                           2456 ;     genCmpEq
                           2457 ;	Peephole 112.b	changed ljmp to sjmp
                           2458 ;	Peephole 199	optimized misc jump sequence
   1328 BA 01 18           2459 	cjne	r2,#0x01,00216$
                           2460 ;00405$:
                           2461 ;	Peephole 200	removed redundant sjmp
   132B                    2462 00406$:
                           2463 ;src/qs1r_main.c:373: DDC1_AMR = (RDEC >> 8);
                           2464 ;     genAssign
   132B 90 60 17           2465 	mov	dptr,#_DDC1_AMR
   132E 74 03              2466 	mov	a,#0x03
   1330 F0                 2467 	movx	@dptr,a
                           2468 ;src/qs1r_main.c:374: DDC1_LAR = (RDEC & 0xFF);
                           2469 ;     genAssign
   1331 90 60 16           2470 	mov	dptr,#_DDC1_LAR
   1334 74 0A              2471 	mov	a,#0x0A
   1336 F0                 2472 	movx	@dptr,a
                           2473 ;src/qs1r_main.c:375: DDC1_DR0 = EP0BUF[0];
                           2474 ;     genPointerGet
                           2475 ;     genFarPointerGet
   1337 90 E7 40           2476 	mov	dptr,#_EP0BUF
   133A E0                 2477 	movx	a,@dptr
                           2478 ;     genAssign
                           2479 ;	Peephole 100	removed redundant mov
   133B FA                 2480 	mov	r2,a
   133C 90 60 10           2481 	mov	dptr,#_DDC1_DR0
   133F F0                 2482 	movx	@dptr,a
   1340 02 16 54           2483 	ljmp	00266$
   1343                    2484 00216$:
                           2485 ;src/qs1r_main.c:376: } else return 0;
                           2486 ;     genRet
   1343 75 82 00           2487 	mov	dpl,#0x00
                           2488 ;	Peephole 251.a	replaced ljmp to ret with ret
   1346 22                 2489 	ret
                           2490 ;src/qs1r_main.c:379: case VRQ_DDC_RAO_SET:
   1347                    2491 00221$:
                           2492 ;src/qs1r_main.c:380: get_ep0_data ();
                           2493 ;     genCall
   1347 12 0D 6E           2494 	lcall	_get_ep0_data
                           2495 ;src/qs1r_main.c:381: if (wIndexL == 0) {
                           2496 ;     genPointerGet
                           2497 ;     genFarPointerGet
   134A 90 E6 BC           2498 	mov	dptr,#(_SETUPDAT + 0x0004)
   134D E0                 2499 	movx	a,@dptr
                           2500 ;     genCmpEq
                           2501 ;	Peephole 115.b	jump optimization
   134E FA                 2502 	mov	r2,a
   134F 60 02              2503 	jz	00408$
   1351                    2504 00407$:
                           2505 ;	Peephole 112.b	changed ljmp to sjmp
   1351 80 18              2506 	sjmp	00226$
   1353                    2507 00408$:
                           2508 ;src/qs1r_main.c:382: DDC0_AMR = (RAO >> 8);
                           2509 ;     genAssign
   1353 90 60 07           2510 	mov	dptr,#_DDC0_AMR
   1356 74 03              2511 	mov	a,#0x03
   1358 F0                 2512 	movx	@dptr,a
                           2513 ;src/qs1r_main.c:383: DDC0_LAR = (RAO & 0xFF);
                           2514 ;     genAssign
   1359 90 60 06           2515 	mov	dptr,#_DDC0_LAR
   135C 74 0B              2516 	mov	a,#0x0B
   135E F0                 2517 	movx	@dptr,a
                           2518 ;src/qs1r_main.c:384: DDC0_DR0 = EP0BUF[0];
                           2519 ;     genPointerGet
                           2520 ;     genFarPointerGet
   135F 90 E7 40           2521 	mov	dptr,#_EP0BUF
   1362 E0                 2522 	movx	a,@dptr
                           2523 ;     genAssign
                           2524 ;	Peephole 100	removed redundant mov
   1363 FA                 2525 	mov	r2,a
   1364 90 60 00           2526 	mov	dptr,#_DDC0_DR0
   1367 F0                 2527 	movx	@dptr,a
   1368 02 16 54           2528 	ljmp	00266$
   136B                    2529 00226$:
                           2530 ;src/qs1r_main.c:385: } else if (wIndexL == 1) {
                           2531 ;     genPointerGet
                           2532 ;     genFarPointerGet
   136B 90 E6 BC           2533 	mov	dptr,#(_SETUPDAT + 0x0004)
   136E E0                 2534 	movx	a,@dptr
   136F FA                 2535 	mov	r2,a
                           2536 ;     genCmpEq
                           2537 ;	Peephole 112.b	changed ljmp to sjmp
                           2538 ;	Peephole 199	optimized misc jump sequence
   1370 BA 01 18           2539 	cjne	r2,#0x01,00223$
                           2540 ;00409$:
                           2541 ;	Peephole 200	removed redundant sjmp
   1373                    2542 00410$:
                           2543 ;src/qs1r_main.c:386: DDC1_AMR = (RAO >> 8);
                           2544 ;     genAssign
   1373 90 60 17           2545 	mov	dptr,#_DDC1_AMR
   1376 74 03              2546 	mov	a,#0x03
   1378 F0                 2547 	movx	@dptr,a
                           2548 ;src/qs1r_main.c:387: DDC1_LAR = (RAO & 0xFF);
                           2549 ;     genAssign
   1379 90 60 16           2550 	mov	dptr,#_DDC1_LAR
   137C 74 0B              2551 	mov	a,#0x0B
   137E F0                 2552 	movx	@dptr,a
                           2553 ;src/qs1r_main.c:388: DDC1_DR0 = EP0BUF[0];
                           2554 ;     genPointerGet
                           2555 ;     genFarPointerGet
   137F 90 E7 40           2556 	mov	dptr,#_EP0BUF
   1382 E0                 2557 	movx	a,@dptr
                           2558 ;     genAssign
                           2559 ;	Peephole 100	removed redundant mov
   1383 FA                 2560 	mov	r2,a
   1384 90 60 10           2561 	mov	dptr,#_DDC1_DR0
   1387 F0                 2562 	movx	@dptr,a
   1388 02 16 54           2563 	ljmp	00266$
   138B                    2564 00223$:
                           2565 ;src/qs1r_main.c:389: } else return 0;
                           2566 ;     genRet
   138B 75 82 00           2567 	mov	dpl,#0x00
                           2568 ;	Peephole 251.a	replaced ljmp to ret with ret
   138E 22                 2569 	ret
                           2570 ;src/qs1r_main.c:392: case VRQ_DDC_RTAPS_SET:
   138F                    2571 00228$:
                           2572 ;src/qs1r_main.c:393: get_ep0_data ();
                           2573 ;     genCall
   138F 12 0D 6E           2574 	lcall	_get_ep0_data
                           2575 ;src/qs1r_main.c:394: if (wIndexL == 0) {
                           2576 ;     genPointerGet
                           2577 ;     genFarPointerGet
   1392 90 E6 BC           2578 	mov	dptr,#(_SETUPDAT + 0x0004)
   1395 E0                 2579 	movx	a,@dptr
                           2580 ;     genCmpEq
                           2581 ;	Peephole 115.b	jump optimization
   1396 FA                 2582 	mov	r2,a
   1397 60 02              2583 	jz	00412$
   1399                    2584 00411$:
                           2585 ;	Peephole 112.b	changed ljmp to sjmp
   1399 80 18              2586 	sjmp	00233$
   139B                    2587 00412$:
                           2588 ;src/qs1r_main.c:395: DDC0_AMR = (RTAP >> 8);
                           2589 ;     genAssign
   139B 90 60 07           2590 	mov	dptr,#_DDC0_AMR
   139E 74 03              2591 	mov	a,#0x03
   13A0 F0                 2592 	movx	@dptr,a
                           2593 ;src/qs1r_main.c:396: DDC0_LAR = (RTAP & 0xFF);
                           2594 ;     genAssign
   13A1 90 60 06           2595 	mov	dptr,#_DDC0_LAR
   13A4 74 0C              2596 	mov	a,#0x0C
   13A6 F0                 2597 	movx	@dptr,a
                           2598 ;src/qs1r_main.c:397: DDC0_DR0 = EP0BUF[0];
                           2599 ;     genPointerGet
                           2600 ;     genFarPointerGet
   13A7 90 E7 40           2601 	mov	dptr,#_EP0BUF
   13AA E0                 2602 	movx	a,@dptr
                           2603 ;     genAssign
                           2604 ;	Peephole 100	removed redundant mov
   13AB FA                 2605 	mov	r2,a
   13AC 90 60 00           2606 	mov	dptr,#_DDC0_DR0
   13AF F0                 2607 	movx	@dptr,a
   13B0 02 16 54           2608 	ljmp	00266$
   13B3                    2609 00233$:
                           2610 ;src/qs1r_main.c:398: } else if (wIndexL == 1) {
                           2611 ;     genPointerGet
                           2612 ;     genFarPointerGet
   13B3 90 E6 BC           2613 	mov	dptr,#(_SETUPDAT + 0x0004)
   13B6 E0                 2614 	movx	a,@dptr
   13B7 FA                 2615 	mov	r2,a
                           2616 ;     genCmpEq
                           2617 ;	Peephole 112.b	changed ljmp to sjmp
                           2618 ;	Peephole 199	optimized misc jump sequence
   13B8 BA 01 18           2619 	cjne	r2,#0x01,00230$
                           2620 ;00413$:
                           2621 ;	Peephole 200	removed redundant sjmp
   13BB                    2622 00414$:
                           2623 ;src/qs1r_main.c:399: DDC1_AMR = (RTAP >> 8);
                           2624 ;     genAssign
   13BB 90 60 17           2625 	mov	dptr,#_DDC1_AMR
   13BE 74 03              2626 	mov	a,#0x03
   13C0 F0                 2627 	movx	@dptr,a
                           2628 ;src/qs1r_main.c:400: DDC1_LAR = (RTAP & 0xFF);
                           2629 ;     genAssign
   13C1 90 60 16           2630 	mov	dptr,#_DDC1_LAR
   13C4 74 0C              2631 	mov	a,#0x0C
   13C6 F0                 2632 	movx	@dptr,a
                           2633 ;src/qs1r_main.c:401: DDC1_DR0 = EP0BUF[0];
                           2634 ;     genPointerGet
                           2635 ;     genFarPointerGet
   13C7 90 E7 40           2636 	mov	dptr,#_EP0BUF
   13CA E0                 2637 	movx	a,@dptr
                           2638 ;     genAssign
                           2639 ;	Peephole 100	removed redundant mov
   13CB FA                 2640 	mov	r2,a
   13CC 90 60 10           2641 	mov	dptr,#_DDC1_DR0
   13CF F0                 2642 	movx	@dptr,a
   13D0 02 16 54           2643 	ljmp	00266$
   13D3                    2644 00230$:
                           2645 ;src/qs1r_main.c:402: } else return 0;
                           2646 ;     genRet
   13D3 75 82 00           2647 	mov	dpl,#0x00
                           2648 ;	Peephole 251.a	replaced ljmp to ret with ret
   13D6 22                 2649 	ret
                           2650 ;src/qs1r_main.c:405: case VRQ_DDC_RSVR_SET:
   13D7                    2651 00235$:
                           2652 ;src/qs1r_main.c:406: get_ep0_data ();
                           2653 ;     genCall
   13D7 12 0D 6E           2654 	lcall	_get_ep0_data
                           2655 ;src/qs1r_main.c:407: if (wIndexL == 0) {
                           2656 ;     genPointerGet
                           2657 ;     genFarPointerGet
   13DA 90 E6 BC           2658 	mov	dptr,#(_SETUPDAT + 0x0004)
   13DD E0                 2659 	movx	a,@dptr
                           2660 ;     genCmpEq
                           2661 ;	Peephole 115.b	jump optimization
   13DE FA                 2662 	mov	r2,a
   13DF 60 02              2663 	jz	00416$
   13E1                    2664 00415$:
                           2665 ;	Peephole 112.b	changed ljmp to sjmp
   13E1 80 18              2666 	sjmp	00240$
   13E3                    2667 00416$:
                           2668 ;src/qs1r_main.c:408: DDC0_AMR = (RSRV >> 8);
                           2669 ;     genAssign
   13E3 90 60 07           2670 	mov	dptr,#_DDC0_AMR
   13E6 74 03              2671 	mov	a,#0x03
   13E8 F0                 2672 	movx	@dptr,a
                           2673 ;src/qs1r_main.c:409: DDC0_LAR = (RSRV & 0xFF);
                           2674 ;     genAssign
   13E9 90 60 06           2675 	mov	dptr,#_DDC0_LAR
   13EC 74 0D              2676 	mov	a,#0x0D
   13EE F0                 2677 	movx	@dptr,a
                           2678 ;src/qs1r_main.c:410: DDC0_DR0 = EP0BUF[0];
                           2679 ;     genPointerGet
                           2680 ;     genFarPointerGet
   13EF 90 E7 40           2681 	mov	dptr,#_EP0BUF
   13F2 E0                 2682 	movx	a,@dptr
                           2683 ;     genAssign
                           2684 ;	Peephole 100	removed redundant mov
   13F3 FA                 2685 	mov	r2,a
   13F4 90 60 00           2686 	mov	dptr,#_DDC0_DR0
   13F7 F0                 2687 	movx	@dptr,a
   13F8 02 16 54           2688 	ljmp	00266$
   13FB                    2689 00240$:
                           2690 ;src/qs1r_main.c:411: } else if (wIndexL == 1) {
                           2691 ;     genPointerGet
                           2692 ;     genFarPointerGet
   13FB 90 E6 BC           2693 	mov	dptr,#(_SETUPDAT + 0x0004)
   13FE E0                 2694 	movx	a,@dptr
   13FF FA                 2695 	mov	r2,a
                           2696 ;     genCmpEq
                           2697 ;	Peephole 112.b	changed ljmp to sjmp
                           2698 ;	Peephole 199	optimized misc jump sequence
   1400 BA 01 18           2699 	cjne	r2,#0x01,00237$
                           2700 ;00417$:
                           2701 ;	Peephole 200	removed redundant sjmp
   1403                    2702 00418$:
                           2703 ;src/qs1r_main.c:412: DDC1_AMR = (RSRV >> 8);
                           2704 ;     genAssign
   1403 90 60 17           2705 	mov	dptr,#_DDC1_AMR
   1406 74 03              2706 	mov	a,#0x03
   1408 F0                 2707 	movx	@dptr,a
                           2708 ;src/qs1r_main.c:413: DDC1_LAR = (RSRV & 0xFF);
                           2709 ;     genAssign
   1409 90 60 16           2710 	mov	dptr,#_DDC1_LAR
   140C 74 0D              2711 	mov	a,#0x0D
   140E F0                 2712 	movx	@dptr,a
                           2713 ;src/qs1r_main.c:414: DDC1_DR0 = EP0BUF[0];
                           2714 ;     genPointerGet
                           2715 ;     genFarPointerGet
   140F 90 E7 40           2716 	mov	dptr,#_EP0BUF
   1412 E0                 2717 	movx	a,@dptr
                           2718 ;     genAssign
                           2719 ;	Peephole 100	removed redundant mov
   1413 FA                 2720 	mov	r2,a
   1414 90 60 10           2721 	mov	dptr,#_DDC1_DR0
   1417 F0                 2722 	movx	@dptr,a
   1418 02 16 54           2723 	ljmp	00266$
   141B                    2724 00237$:
                           2725 ;src/qs1r_main.c:415: } else return 0;
                           2726 ;     genRet
   141B 75 82 00           2727 	mov	dpl,#0x00
                           2728 ;	Peephole 251.a	replaced ljmp to ret with ret
   141E 22                 2729 	ret
                           2730 ;src/qs1r_main.c:418: case VRQ_DDC_COEF_SET:
   141F                    2731 00242$:
                           2732 ;src/qs1r_main.c:419: get_ep0_data ();
                           2733 ;     genCall
   141F 12 0D 6E           2734 	lcall	_get_ep0_data
                           2735 ;src/qs1r_main.c:420: if (wValueL > 0x0F) return 0;
                           2736 ;     genPointerGet
                           2737 ;     genFarPointerGet
   1422 90 E6 BA           2738 	mov	dptr,#(_SETUPDAT + 0x0002)
   1425 E0                 2739 	movx	a,@dptr
                           2740 ;     genCmpGt
                           2741 ;     genCmp
                           2742 ;     genIfxJump
                           2743 ;	Peephole 108	removed ljmp by inverse jump logic
                           2744 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
                           2745 ;	Peephole 177.a	removed redundant mov
   1426 FA                 2746 	mov  r2,a
   1427 24 F0              2747 	add	a,#0xff - 0x0F
   1429 50 04              2748 	jnc	00244$
   142B                    2749 00419$:
                           2750 ;     genRet
   142B 75 82 00           2751 	mov	dpl,#0x00
                           2752 ;	Peephole 251.a	replaced ljmp to ret with ret
   142E 22                 2753 	ret
   142F                    2754 00244$:
                           2755 ;src/qs1r_main.c:421: offset = wValueL << 4; // advance by 16
                           2756 ;     genPointerGet
                           2757 ;     genFarPointerGet
   142F 90 E6 BA           2758 	mov	dptr,#(_SETUPDAT + 0x0002)
   1432 E0                 2759 	movx	a,@dptr
                           2760 ;     genLeftShift
                           2761 ;     genLeftShiftLiteral
                           2762 ;     genlshOne
                           2763 ;	Peephole 105	removed redundant mov
   1433 FA                 2764 	mov	r2,a
   1434 C4                 2765 	swap	a
   1435 54 F0              2766 	anl	a,#0xf0
   1437 FA                 2767 	mov	r2,a
                           2768 ;src/qs1r_main.c:422: if (wIndexL == 0)
                           2769 ;     genPointerGet
                           2770 ;     genFarPointerGet
   1438 90 E6 BC           2771 	mov	dptr,#(_SETUPDAT + 0x0004)
   143B E0                 2772 	movx	a,@dptr
                           2773 ;     genCmpEq
                           2774 ;	Peephole 115.b	jump optimization
   143C FB                 2775 	mov	r3,a
   143D 60 02              2776 	jz	00421$
   143F                    2777 00420$:
                           2778 ;	Peephole 112.b	changed ljmp to sjmp
   143F 80 67              2779 	sjmp	00249$
   1441                    2780 00421$:
                           2781 ;src/qs1r_main.c:424: for (i=0, j=0; i < 16; i++, j+=3) {
                           2782 ;     genAssign
   1441 7B 00              2783 	mov	r3,#0x00
   1443 7C 00              2784 	mov	r4,#0x00
                           2785 ;     genAssign
   1445 7D 00              2786 	mov	r5,#0x00
   1447 7E 00              2787 	mov	r6,#0x00
   1449                    2788 00267$:
                           2789 ;     genCmpLt
                           2790 ;     genCmp
   1449 C3                 2791 	clr	c
   144A EB                 2792 	mov	a,r3
   144B 94 10              2793 	subb	a,#0x10
   144D EC                 2794 	mov	a,r4
   144E 64 80              2795 	xrl	a,#0x80
   1450 94 80              2796 	subb	a,#0x80
                           2797 ;     genIfxJump
   1452 40 03              2798 	jc	00422$
   1454 02 16 54           2799 	ljmp	00266$
   1457                    2800 00422$:
                           2801 ;src/qs1r_main.c:425: DDC0_AMR = 0;
                           2802 ;     genAssign
   1457 90 60 07           2803 	mov	dptr,#_DDC0_AMR
                           2804 ;	Peephole 181	changed mov to clr
   145A E4                 2805 	clr	a
   145B F0                 2806 	movx	@dptr,a
                           2807 ;src/qs1r_main.c:426: DDC0_LAR = ((DDC_COEF + i + offset) & 0xFF);
                           2808 ;     genCast
   145C 8B 07              2809 	mov	ar7,r3
                           2810 ;     genPlus
   145E 90 60 06           2811 	mov	dptr,#_DDC0_LAR
                           2812 ;	Peephole 236.g	used r2 instead of ar2
   1461 EA                 2813 	mov	a,r2
                           2814 ;	Peephole 236.a	used r7 instead of ar7
   1462 2F                 2815 	add	a,r7
   1463 F0                 2816 	movx	@dptr,a
                           2817 ;src/qs1r_main.c:427: DDC0_DR2 = (EP0BUF[j+2] & 0xFF);
                           2818 ;     genCast
   1464 8D 07              2819 	mov	ar7,r5
                           2820 ;     genPlus
                           2821 ;     genPlusIncr
   1466 74 02              2822 	mov	a,#0x02
                           2823 ;	Peephole 236.a	used r7 instead of ar7
   1468 2F                 2824 	add	a,r7
                           2825 ;     genPlus
   1469 24 40              2826 	add	a,#_EP0BUF
   146B F5 82              2827 	mov	dpl,a
                           2828 ;	Peephole 240	use clr instead of addc a,#0
   146D E4                 2829 	clr	a
   146E 34 E7              2830 	addc	a,#(_EP0BUF >> 8)
   1470 F5 83              2831 	mov	dph,a
                           2832 ;     genPointerGet
                           2833 ;     genFarPointerGet
   1472 E0                 2834 	movx	a,@dptr
                           2835 ;     genAssign
                           2836 ;	Peephole 100	removed redundant mov
   1473 F8                 2837 	mov	r0,a
   1474 90 60 02           2838 	mov	dptr,#_DDC0_DR2
   1477 F0                 2839 	movx	@dptr,a
                           2840 ;src/qs1r_main.c:428: DDC0_DR1 = (EP0BUF[j+1] & 0xFF);
                           2841 ;     genPlus
                           2842 ;     genPlusIncr
   1478 74 01              2843 	mov	a,#0x01
                           2844 ;	Peephole 236.a	used r7 instead of ar7
   147A 2F                 2845 	add	a,r7
                           2846 ;     genPlus
   147B 24 40              2847 	add	a,#_EP0BUF
   147D F5 82              2848 	mov	dpl,a
                           2849 ;	Peephole 240	use clr instead of addc a,#0
   147F E4                 2850 	clr	a
   1480 34 E7              2851 	addc	a,#(_EP0BUF >> 8)
   1482 F5 83              2852 	mov	dph,a
                           2853 ;     genPointerGet
                           2854 ;     genFarPointerGet
   1484 E0                 2855 	movx	a,@dptr
                           2856 ;     genAssign
                           2857 ;	Peephole 100	removed redundant mov
   1485 FF                 2858 	mov	r7,a
   1486 90 60 01           2859 	mov	dptr,#_DDC0_DR1
   1489 F0                 2860 	movx	@dptr,a
                           2861 ;src/qs1r_main.c:429: DDC0_DR0 = (EP0BUF[j] & 0xFF);
                           2862 ;     genPlus
                           2863 ;	Peephole 236.g	used r5 instead of ar5
   148A ED                 2864 	mov	a,r5
   148B 24 40              2865 	add	a,#_EP0BUF
   148D F5 82              2866 	mov	dpl,a
                           2867 ;	Peephole 236.g	used r6 instead of ar6
   148F EE                 2868 	mov	a,r6
   1490 34 E7              2869 	addc	a,#(_EP0BUF >> 8)
   1492 F5 83              2870 	mov	dph,a
                           2871 ;     genPointerGet
                           2872 ;     genFarPointerGet
   1494 E0                 2873 	movx	a,@dptr
                           2874 ;     genAssign
                           2875 ;	Peephole 100	removed redundant mov
   1495 FF                 2876 	mov	r7,a
   1496 90 60 00           2877 	mov	dptr,#_DDC0_DR0
   1499 F0                 2878 	movx	@dptr,a
                           2879 ;src/qs1r_main.c:424: for (i=0, j=0; i < 16; i++, j+=3) {
                           2880 ;     genPlus
                           2881 ;     genPlusIncr
   149A 0B                 2882 	inc	r3
   149B BB 00 01           2883 	cjne	r3,#0x00,00423$
   149E 0C                 2884 	inc	r4
   149F                    2885 00423$:
                           2886 ;     genPlus
                           2887 ;     genPlusIncr
   149F 74 03              2888 	mov	a,#0x03
                           2889 ;	Peephole 236.a	used r5 instead of ar5
   14A1 2D                 2890 	add	a,r5
   14A2 FD                 2891 	mov	r5,a
                           2892 ;	Peephole 181	changed mov to clr
   14A3 E4                 2893 	clr	a
                           2894 ;	Peephole 236.b	used r6 instead of ar6
   14A4 3E                 2895 	addc	a,r6
   14A5 FE                 2896 	mov	r6,a
                           2897 ;	Peephole 112.b	changed ljmp to sjmp
   14A6 80 A1              2898 	sjmp	00267$
   14A8                    2899 00249$:
                           2900 ;src/qs1r_main.c:431: } else if (wIndexL == 1) {
                           2901 ;     genPointerGet
                           2902 ;     genFarPointerGet
   14A8 90 E6 BC           2903 	mov	dptr,#(_SETUPDAT + 0x0004)
   14AB E0                 2904 	movx	a,@dptr
   14AC FB                 2905 	mov	r3,a
                           2906 ;     genCmpEq
                           2907 ;	Peephole 112.b	changed ljmp to sjmp
                           2908 ;	Peephole 199	optimized misc jump sequence
   14AD BB 01 67           2909 	cjne	r3,#0x01,00246$
                           2910 ;00424$:
                           2911 ;	Peephole 200	removed redundant sjmp
   14B0                    2912 00425$:
                           2913 ;src/qs1r_main.c:432: for (i=0, j=0; i < 16; i++, j+=3) {
                           2914 ;     genAssign
   14B0 7B 00              2915 	mov	r3,#0x00
   14B2 7C 00              2916 	mov	r4,#0x00
                           2917 ;     genAssign
   14B4 7D 00              2918 	mov	r5,#0x00
   14B6 7E 00              2919 	mov	r6,#0x00
   14B8                    2920 00271$:
                           2921 ;     genCmpLt
                           2922 ;     genCmp
   14B8 C3                 2923 	clr	c
   14B9 EB                 2924 	mov	a,r3
   14BA 94 10              2925 	subb	a,#0x10
   14BC EC                 2926 	mov	a,r4
   14BD 64 80              2927 	xrl	a,#0x80
   14BF 94 80              2928 	subb	a,#0x80
                           2929 ;     genIfxJump
   14C1 40 03              2930 	jc	00426$
   14C3 02 16 54           2931 	ljmp	00266$
   14C6                    2932 00426$:
                           2933 ;src/qs1r_main.c:433: DDC1_AMR = 0;
                           2934 ;     genAssign
   14C6 90 60 17           2935 	mov	dptr,#_DDC1_AMR
                           2936 ;	Peephole 181	changed mov to clr
   14C9 E4                 2937 	clr	a
   14CA F0                 2938 	movx	@dptr,a
                           2939 ;src/qs1r_main.c:434: DDC1_LAR = ((DDC_COEF + i + offset) & 0xFF);
                           2940 ;     genCast
   14CB 8B 07              2941 	mov	ar7,r3
                           2942 ;     genPlus
   14CD 90 60 16           2943 	mov	dptr,#_DDC1_LAR
                           2944 ;	Peephole 236.g	used r2 instead of ar2
   14D0 EA                 2945 	mov	a,r2
                           2946 ;	Peephole 236.a	used r7 instead of ar7
   14D1 2F                 2947 	add	a,r7
   14D2 F0                 2948 	movx	@dptr,a
                           2949 ;src/qs1r_main.c:435: DDC1_DR2 = (EP0BUF[j+2] & 0xFF);
                           2950 ;     genCast
   14D3 8D 07              2951 	mov	ar7,r5
                           2952 ;     genPlus
                           2953 ;     genPlusIncr
   14D5 74 02              2954 	mov	a,#0x02
                           2955 ;	Peephole 236.a	used r7 instead of ar7
   14D7 2F                 2956 	add	a,r7
                           2957 ;     genPlus
   14D8 24 40              2958 	add	a,#_EP0BUF
   14DA F5 82              2959 	mov	dpl,a
                           2960 ;	Peephole 240	use clr instead of addc a,#0
   14DC E4                 2961 	clr	a
   14DD 34 E7              2962 	addc	a,#(_EP0BUF >> 8)
   14DF F5 83              2963 	mov	dph,a
                           2964 ;     genPointerGet
                           2965 ;     genFarPointerGet
   14E1 E0                 2966 	movx	a,@dptr
                           2967 ;     genAssign
                           2968 ;	Peephole 100	removed redundant mov
   14E2 F8                 2969 	mov	r0,a
   14E3 90 60 12           2970 	mov	dptr,#_DDC1_DR2
   14E6 F0                 2971 	movx	@dptr,a
                           2972 ;src/qs1r_main.c:436: DDC1_DR1 = (EP0BUF[j+1] & 0xFF);
                           2973 ;     genPlus
                           2974 ;     genPlusIncr
   14E7 74 01              2975 	mov	a,#0x01
                           2976 ;	Peephole 236.a	used r7 instead of ar7
   14E9 2F                 2977 	add	a,r7
                           2978 ;     genPlus
   14EA 24 40              2979 	add	a,#_EP0BUF
   14EC F5 82              2980 	mov	dpl,a
                           2981 ;	Peephole 240	use clr instead of addc a,#0
   14EE E4                 2982 	clr	a
   14EF 34 E7              2983 	addc	a,#(_EP0BUF >> 8)
   14F1 F5 83              2984 	mov	dph,a
                           2985 ;     genPointerGet
                           2986 ;     genFarPointerGet
   14F3 E0                 2987 	movx	a,@dptr
                           2988 ;     genAssign
                           2989 ;	Peephole 100	removed redundant mov
   14F4 FF                 2990 	mov	r7,a
   14F5 90 60 11           2991 	mov	dptr,#_DDC1_DR1
   14F8 F0                 2992 	movx	@dptr,a
                           2993 ;src/qs1r_main.c:437: DDC1_DR0 = (EP0BUF[j] & 0xFF);
                           2994 ;     genPlus
                           2995 ;	Peephole 236.g	used r5 instead of ar5
   14F9 ED                 2996 	mov	a,r5
   14FA 24 40              2997 	add	a,#_EP0BUF
   14FC F5 82              2998 	mov	dpl,a
                           2999 ;	Peephole 236.g	used r6 instead of ar6
   14FE EE                 3000 	mov	a,r6
   14FF 34 E7              3001 	addc	a,#(_EP0BUF >> 8)
   1501 F5 83              3002 	mov	dph,a
                           3003 ;     genPointerGet
                           3004 ;     genFarPointerGet
   1503 E0                 3005 	movx	a,@dptr
                           3006 ;     genAssign
                           3007 ;	Peephole 100	removed redundant mov
   1504 FF                 3008 	mov	r7,a
   1505 90 60 10           3009 	mov	dptr,#_DDC1_DR0
   1508 F0                 3010 	movx	@dptr,a
                           3011 ;src/qs1r_main.c:432: for (i=0, j=0; i < 16; i++, j+=3) {
                           3012 ;     genPlus
                           3013 ;     genPlusIncr
   1509 0B                 3014 	inc	r3
   150A BB 00 01           3015 	cjne	r3,#0x00,00427$
   150D 0C                 3016 	inc	r4
   150E                    3017 00427$:
                           3018 ;     genPlus
                           3019 ;     genPlusIncr
   150E 74 03              3020 	mov	a,#0x03
                           3021 ;	Peephole 236.a	used r5 instead of ar5
   1510 2D                 3022 	add	a,r5
   1511 FD                 3023 	mov	r5,a
                           3024 ;	Peephole 181	changed mov to clr
   1512 E4                 3025 	clr	a
                           3026 ;	Peephole 236.b	used r6 instead of ar6
   1513 3E                 3027 	addc	a,r6
   1514 FE                 3028 	mov	r6,a
                           3029 ;	Peephole 112.b	changed ljmp to sjmp
   1515 80 A1              3030 	sjmp	00271$
   1517                    3031 00246$:
                           3032 ;src/qs1r_main.c:439: } else return 0;
                           3033 ;     genRet
   1517 75 82 00           3034 	mov	dpl,#0x00
                           3035 ;	Peephole 251.a	replaced ljmp to ret with ret
   151A 22                 3036 	ret
                           3037 ;src/qs1r_main.c:442: case VRQ_DDC_COEF_CLR:
   151B                    3038 00251$:
                           3039 ;src/qs1r_main.c:443: get_ep0_data ();
                           3040 ;     genCall
   151B 12 0D 6E           3041 	lcall	_get_ep0_data
                           3042 ;src/qs1r_main.c:444: if (wIndexL == 0)  {
                           3043 ;     genPointerGet
                           3044 ;     genFarPointerGet
   151E 90 E6 BC           3045 	mov	dptr,#(_SETUPDAT + 0x0004)
   1521 E0                 3046 	movx	a,@dptr
                           3047 ;     genCmpEq
                           3048 ;	Peephole 115.b	jump optimization
   1522 FA                 3049 	mov	r2,a
   1523 60 02              3050 	jz	00429$
   1525                    3051 00428$:
                           3052 ;	Peephole 112.b	changed ljmp to sjmp
   1525 80 41              3053 	sjmp	00256$
   1527                    3054 00429$:
                           3055 ;src/qs1r_main.c:445: for (i=0; i < 0x100; i++) {
                           3056 ;     genAssign
   1527 7A 00              3057 	mov	r2,#0x00
   1529 7B 00              3058 	mov	r3,#0x00
   152B                    3059 00275$:
                           3060 ;     genCmpLt
                           3061 ;     genCmp
   152B C3                 3062 	clr	c
   152C EA                 3063 	mov	a,r2
   152D 94 00              3064 	subb	a,#0x00
   152F EB                 3065 	mov	a,r3
   1530 64 80              3066 	xrl	a,#0x80
   1532 94 81              3067 	subb	a,#0x81
                           3068 ;     genIfxJump
   1534 40 03              3069 	jc	00430$
   1536 02 16 54           3070 	ljmp	00266$
   1539                    3071 00430$:
                           3072 ;src/qs1r_main.c:446: DDC0_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
                           3073 ;     genAnd
   1539 7C 00              3074 	mov	r4,#0x00
   153B 8B 05              3075 	mov	ar5,r3
                           3076 ;     genRightShift
                           3077 ;     genRightShiftLiteral
                           3078 ;     genrshTwo
   153D 8D 04              3079 	mov	ar4,r5
   153F 7D 00              3080 	mov	r5,#0x00
                           3081 ;     genCast
   1541 90 60 07           3082 	mov	dptr,#_DDC0_AMR
   1544 EC                 3083 	mov	a,r4
   1545 F0                 3084 	movx	@dptr,a
                           3085 ;src/qs1r_main.c:447: DDC0_LAR = ((DDC_COEF + i) & 0xFF);
                           3086 ;     genCast
   1546 90 60 06           3087 	mov	dptr,#_DDC0_LAR
   1549 EA                 3088 	mov	a,r2
   154A F0                 3089 	movx	@dptr,a
                           3090 ;src/qs1r_main.c:448: DDC0_DR4 = 0;
                           3091 ;     genAssign
   154B 90 60 04           3092 	mov	dptr,#_DDC0_DR4
                           3093 ;	Peephole 181	changed mov to clr
                           3094 ;src/qs1r_main.c:449: DDC0_DR3 = 0;
                           3095 ;     genAssign
                           3096 ;	Peephole 181	changed mov to clr
                           3097 ;	Peephole 219	removed redundant clear
                           3098 ;src/qs1r_main.c:450: DDC0_DR2 = 0;
                           3099 ;     genAssign
                           3100 ;	Peephole 181	changed mov to clr
                           3101 ;src/qs1r_main.c:451: DDC0_DR1 = 0;
                           3102 ;     genAssign
                           3103 ;	Peephole 181	changed mov to clr
                           3104 ;	Peephole 219	removed redundant clear
                           3105 ;	Peephole 219.a	removed redundant clear
   154E E4                 3106 	clr	a
   154F F0                 3107 	movx	@dptr,a
   1550 90 60 03           3108 	mov	dptr,#_DDC0_DR3
   1553 F0                 3109 	movx	@dptr,a
   1554 90 60 02           3110 	mov	dptr,#_DDC0_DR2
   1557 F0                 3111 	movx	@dptr,a
   1558 90 60 01           3112 	mov	dptr,#_DDC0_DR1
   155B F0                 3113 	movx	@dptr,a
                           3114 ;src/qs1r_main.c:452: DDC0_DR0 = 0;
                           3115 ;     genAssign
   155C 90 60 00           3116 	mov	dptr,#_DDC0_DR0
                           3117 ;	Peephole 181	changed mov to clr
   155F E4                 3118 	clr	a
   1560 F0                 3119 	movx	@dptr,a
                           3120 ;src/qs1r_main.c:445: for (i=0; i < 0x100; i++) {
                           3121 ;     genPlus
                           3122 ;     genPlusIncr
   1561 0A                 3123 	inc	r2
                           3124 ;	Peephole 112.b	changed ljmp to sjmp
                           3125 ;	Peephole 243	avoided branch to sjmp
   1562 BA 00 C6           3126 	cjne	r2,#0x00,00275$
   1565 0B                 3127 	inc	r3
   1566                    3128 00431$:
   1566 80 C3              3129 	sjmp	00275$
   1568                    3130 00256$:
                           3131 ;src/qs1r_main.c:454: } else if (wIndexL == 1) {
                           3132 ;     genPointerGet
                           3133 ;     genFarPointerGet
   1568 90 E6 BC           3134 	mov	dptr,#(_SETUPDAT + 0x0004)
   156B E0                 3135 	movx	a,@dptr
   156C FA                 3136 	mov	r2,a
                           3137 ;     genCmpEq
                           3138 ;	Peephole 112.b	changed ljmp to sjmp
                           3139 ;	Peephole 199	optimized misc jump sequence
   156D BA 01 41           3140 	cjne	r2,#0x01,00253$
                           3141 ;00432$:
                           3142 ;	Peephole 200	removed redundant sjmp
   1570                    3143 00433$:
                           3144 ;src/qs1r_main.c:455: for (i=0; i < 0x100; i++) {
                           3145 ;     genAssign
   1570 7A 00              3146 	mov	r2,#0x00
   1572 7B 00              3147 	mov	r3,#0x00
   1574                    3148 00279$:
                           3149 ;     genCmpLt
                           3150 ;     genCmp
   1574 C3                 3151 	clr	c
   1575 EA                 3152 	mov	a,r2
   1576 94 00              3153 	subb	a,#0x00
   1578 EB                 3154 	mov	a,r3
   1579 64 80              3155 	xrl	a,#0x80
   157B 94 81              3156 	subb	a,#0x81
                           3157 ;     genIfxJump
   157D 40 03              3158 	jc	00434$
   157F 02 16 54           3159 	ljmp	00266$
   1582                    3160 00434$:
                           3161 ;src/qs1r_main.c:456: DDC1_AMR = ((DDC_COEF + i) & 0xFF00) >> 8;
                           3162 ;     genAnd
   1582 7C 00              3163 	mov	r4,#0x00
   1584 8B 05              3164 	mov	ar5,r3
                           3165 ;     genRightShift
                           3166 ;     genRightShiftLiteral
                           3167 ;     genrshTwo
   1586 8D 04              3168 	mov	ar4,r5
   1588 7D 00              3169 	mov	r5,#0x00
                           3170 ;     genCast
   158A 90 60 17           3171 	mov	dptr,#_DDC1_AMR
   158D EC                 3172 	mov	a,r4
   158E F0                 3173 	movx	@dptr,a
                           3174 ;src/qs1r_main.c:457: DDC1_LAR = ((DDC_COEF + i) & 0xFF);
                           3175 ;     genCast
   158F 90 60 16           3176 	mov	dptr,#_DDC1_LAR
   1592 EA                 3177 	mov	a,r2
   1593 F0                 3178 	movx	@dptr,a
                           3179 ;src/qs1r_main.c:458: DDC1_DR4 = 0;
                           3180 ;     genAssign
   1594 90 60 14           3181 	mov	dptr,#_DDC1_DR4
                           3182 ;	Peephole 181	changed mov to clr
                           3183 ;src/qs1r_main.c:459: DDC1_DR3 = 0;
                           3184 ;     genAssign
                           3185 ;	Peephole 181	changed mov to clr
                           3186 ;	Peephole 219	removed redundant clear
                           3187 ;src/qs1r_main.c:460: DDC1_DR2 = 0;
                           3188 ;     genAssign
                           3189 ;	Peephole 181	changed mov to clr
                           3190 ;src/qs1r_main.c:461: DDC1_DR1 = 0;
                           3191 ;     genAssign
                           3192 ;	Peephole 181	changed mov to clr
                           3193 ;	Peephole 219	removed redundant clear
                           3194 ;	Peephole 219.a	removed redundant clear
   1597 E4                 3195 	clr	a
   1598 F0                 3196 	movx	@dptr,a
   1599 90 60 13           3197 	mov	dptr,#_DDC1_DR3
   159C F0                 3198 	movx	@dptr,a
   159D 90 60 12           3199 	mov	dptr,#_DDC1_DR2
   15A0 F0                 3200 	movx	@dptr,a
   15A1 90 60 11           3201 	mov	dptr,#_DDC1_DR1
   15A4 F0                 3202 	movx	@dptr,a
                           3203 ;src/qs1r_main.c:462: DDC1_DR0 = 0;
                           3204 ;     genAssign
   15A5 90 60 10           3205 	mov	dptr,#_DDC1_DR0
                           3206 ;	Peephole 181	changed mov to clr
   15A8 E4                 3207 	clr	a
   15A9 F0                 3208 	movx	@dptr,a
                           3209 ;src/qs1r_main.c:455: for (i=0; i < 0x100; i++) {
                           3210 ;     genPlus
                           3211 ;     genPlusIncr
   15AA 0A                 3212 	inc	r2
                           3213 ;	Peephole 112.b	changed ljmp to sjmp
                           3214 ;	Peephole 243	avoided branch to sjmp
   15AB BA 00 C6           3215 	cjne	r2,#0x00,00279$
   15AE 0B                 3216 	inc	r3
   15AF                    3217 00435$:
   15AF 80 C3              3218 	sjmp	00279$
   15B1                    3219 00253$:
                           3220 ;src/qs1r_main.c:464: } else return 0;
                           3221 ;     genRet
   15B1 75 82 00           3222 	mov	dpl,#0x00
                           3223 ;	Peephole 251.a	replaced ljmp to ret with ret
   15B4 22                 3224 	ret
                           3225 ;src/qs1r_main.c:467: case VRQ_DDC_RAM_CLR:
   15B5                    3226 00258$:
                           3227 ;src/qs1r_main.c:468: get_ep0_data ();
                           3228 ;     genCall
   15B5 12 0D 6E           3229 	lcall	_get_ep0_data
                           3230 ;src/qs1r_main.c:469: if (wIndexL == 0)  {
                           3231 ;     genPointerGet
                           3232 ;     genFarPointerGet
   15B8 90 E6 BC           3233 	mov	dptr,#(_SETUPDAT + 0x0004)
   15BB E0                 3234 	movx	a,@dptr
                           3235 ;     genCmpEq
                           3236 ;	Peephole 115.b	jump optimization
   15BC FA                 3237 	mov	r2,a
   15BD 60 02              3238 	jz	00437$
   15BF                    3239 00436$:
                           3240 ;	Peephole 112.b	changed ljmp to sjmp
   15BF 80 43              3241 	sjmp	00263$
   15C1                    3242 00437$:
                           3243 ;src/qs1r_main.c:470: for (i=0; i < 0x100; i++) {
                           3244 ;     genAssign
   15C1 7A 00              3245 	mov	r2,#0x00
   15C3 7B 00              3246 	mov	r3,#0x00
   15C5                    3247 00283$:
                           3248 ;     genCmpLt
                           3249 ;     genCmp
   15C5 C3                 3250 	clr	c
   15C6 EA                 3251 	mov	a,r2
   15C7 94 00              3252 	subb	a,#0x00
   15C9 EB                 3253 	mov	a,r3
   15CA 64 80              3254 	xrl	a,#0x80
   15CC 94 81              3255 	subb	a,#0x81
                           3256 ;     genIfxJump
   15CE 40 03              3257 	jc	00438$
   15D0 02 16 54           3258 	ljmp	00266$
   15D3                    3259 00438$:
                           3260 ;src/qs1r_main.c:471: DDC0_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
                           3261 ;     genPlus
                           3262 ;     genPlus shortcut
   15D3 8A 04              3263 	mov	ar4,r2
   15D5 74 01              3264 	mov	a,#0x01
                           3265 ;	Peephole 236.a	used r3 instead of ar3
   15D7 2B                 3266 	add	a,r3
   15D8 FD                 3267 	mov	r5,a
                           3268 ;     genAnd
                           3269 ;     genRightShift
                           3270 ;     genRightShiftLiteral
                           3271 ;     genrshTwo
                           3272 ;	peephole 177.e	removed redundant move
   15D9 8D 04              3273 	mov	ar4,r5
   15DB 7D 00              3274 	mov	r5,#0x00
                           3275 ;     genCast
   15DD 90 60 07           3276 	mov	dptr,#_DDC0_AMR
   15E0 EC                 3277 	mov	a,r4
   15E1 F0                 3278 	movx	@dptr,a
                           3279 ;src/qs1r_main.c:472: DDC0_LAR = ((DDC_RAM + i) & 0xFF);
                           3280 ;     genCast
   15E2 90 60 06           3281 	mov	dptr,#_DDC0_LAR
   15E5 EA                 3282 	mov	a,r2
   15E6 F0                 3283 	movx	@dptr,a
                           3284 ;src/qs1r_main.c:473: DDC0_DR4 = 0;
                           3285 ;     genAssign
   15E7 90 60 04           3286 	mov	dptr,#_DDC0_DR4
                           3287 ;	Peephole 181	changed mov to clr
                           3288 ;src/qs1r_main.c:474: DDC0_DR3 = 0;
                           3289 ;     genAssign
                           3290 ;	Peephole 181	changed mov to clr
                           3291 ;	Peephole 219	removed redundant clear
                           3292 ;src/qs1r_main.c:475: DDC0_DR2 = 0;
                           3293 ;     genAssign
                           3294 ;	Peephole 181	changed mov to clr
                           3295 ;src/qs1r_main.c:476: DDC0_DR1 = 0;
                           3296 ;     genAssign
                           3297 ;	Peephole 181	changed mov to clr
                           3298 ;	Peephole 219	removed redundant clear
                           3299 ;	Peephole 219.a	removed redundant clear
   15EA E4                 3300 	clr	a
   15EB F0                 3301 	movx	@dptr,a
   15EC 90 60 03           3302 	mov	dptr,#_DDC0_DR3
   15EF F0                 3303 	movx	@dptr,a
   15F0 90 60 02           3304 	mov	dptr,#_DDC0_DR2
   15F3 F0                 3305 	movx	@dptr,a
   15F4 90 60 01           3306 	mov	dptr,#_DDC0_DR1
   15F7 F0                 3307 	movx	@dptr,a
                           3308 ;src/qs1r_main.c:477: DDC0_DR0 = 0;
                           3309 ;     genAssign
   15F8 90 60 00           3310 	mov	dptr,#_DDC0_DR0
                           3311 ;	Peephole 181	changed mov to clr
   15FB E4                 3312 	clr	a
   15FC F0                 3313 	movx	@dptr,a
                           3314 ;src/qs1r_main.c:470: for (i=0; i < 0x100; i++) {
                           3315 ;     genPlus
                           3316 ;     genPlusIncr
   15FD 0A                 3317 	inc	r2
                           3318 ;	Peephole 112.b	changed ljmp to sjmp
                           3319 ;	Peephole 243	avoided branch to sjmp
   15FE BA 00 C4           3320 	cjne	r2,#0x00,00283$
   1601 0B                 3321 	inc	r3
   1602                    3322 00439$:
   1602 80 C1              3323 	sjmp	00283$
   1604                    3324 00263$:
                           3325 ;src/qs1r_main.c:479: } else if (wIndexL == 1) {
                           3326 ;     genPointerGet
                           3327 ;     genFarPointerGet
   1604 90 E6 BC           3328 	mov	dptr,#(_SETUPDAT + 0x0004)
   1607 E0                 3329 	movx	a,@dptr
   1608 FA                 3330 	mov	r2,a
                           3331 ;     genCmpEq
                           3332 ;	Peephole 112.b	changed ljmp to sjmp
                           3333 ;	Peephole 199	optimized misc jump sequence
   1609 BA 01 40           3334 	cjne	r2,#0x01,00260$
                           3335 ;00440$:
                           3336 ;	Peephole 200	removed redundant sjmp
   160C                    3337 00441$:
                           3338 ;src/qs1r_main.c:480: for (i=0; i < 0x100; i++) {
                           3339 ;     genAssign
   160C 7A 00              3340 	mov	r2,#0x00
   160E 7B 00              3341 	mov	r3,#0x00
   1610                    3342 00287$:
                           3343 ;     genCmpLt
                           3344 ;     genCmp
   1610 C3                 3345 	clr	c
   1611 EA                 3346 	mov	a,r2
   1612 94 00              3347 	subb	a,#0x00
   1614 EB                 3348 	mov	a,r3
   1615 64 80              3349 	xrl	a,#0x80
   1617 94 81              3350 	subb	a,#0x81
                           3351 ;     genIfxJump
                           3352 ;	Peephole 108	removed ljmp by inverse jump logic
   1619 50 39              3353 	jnc	00266$
   161B                    3354 00442$:
                           3355 ;src/qs1r_main.c:481: DDC1_AMR = ((DDC_RAM + i) & 0xFF00) >> 8;
                           3356 ;     genPlus
                           3357 ;     genPlus shortcut
   161B 8A 04              3358 	mov	ar4,r2
   161D 74 01              3359 	mov	a,#0x01
                           3360 ;	Peephole 236.a	used r3 instead of ar3
   161F 2B                 3361 	add	a,r3
   1620 FD                 3362 	mov	r5,a
                           3363 ;     genAnd
                           3364 ;     genRightShift
                           3365 ;     genRightShiftLiteral
                           3366 ;     genrshTwo
                           3367 ;	peephole 177.e	removed redundant move
   1621 8D 04              3368 	mov	ar4,r5
   1623 7D 00              3369 	mov	r5,#0x00
                           3370 ;     genCast
   1625 90 60 17           3371 	mov	dptr,#_DDC1_AMR
   1628 EC                 3372 	mov	a,r4
   1629 F0                 3373 	movx	@dptr,a
                           3374 ;src/qs1r_main.c:482: DDC1_LAR = ((DDC_RAM + i) & 0xFF);
                           3375 ;     genCast
   162A 90 60 16           3376 	mov	dptr,#_DDC1_LAR
   162D EA                 3377 	mov	a,r2
   162E F0                 3378 	movx	@dptr,a
                           3379 ;src/qs1r_main.c:483: DDC1_DR4 = 0;
                           3380 ;     genAssign
   162F 90 60 14           3381 	mov	dptr,#_DDC1_DR4
                           3382 ;	Peephole 181	changed mov to clr
                           3383 ;src/qs1r_main.c:484: DDC1_DR3 = 0;
                           3384 ;     genAssign
                           3385 ;	Peephole 181	changed mov to clr
                           3386 ;	Peephole 219	removed redundant clear
                           3387 ;src/qs1r_main.c:485: DDC1_DR2 = 0;
                           3388 ;     genAssign
                           3389 ;	Peephole 181	changed mov to clr
                           3390 ;src/qs1r_main.c:486: DDC1_DR1 = 0;
                           3391 ;     genAssign
                           3392 ;	Peephole 181	changed mov to clr
                           3393 ;	Peephole 219	removed redundant clear
                           3394 ;	Peephole 219.a	removed redundant clear
   1632 E4                 3395 	clr	a
   1633 F0                 3396 	movx	@dptr,a
   1634 90 60 13           3397 	mov	dptr,#_DDC1_DR3
   1637 F0                 3398 	movx	@dptr,a
   1638 90 60 12           3399 	mov	dptr,#_DDC1_DR2
   163B F0                 3400 	movx	@dptr,a
   163C 90 60 11           3401 	mov	dptr,#_DDC1_DR1
   163F F0                 3402 	movx	@dptr,a
                           3403 ;src/qs1r_main.c:487: DDC1_DR0 = 0;
                           3404 ;     genAssign
   1640 90 60 10           3405 	mov	dptr,#_DDC1_DR0
                           3406 ;	Peephole 181	changed mov to clr
   1643 E4                 3407 	clr	a
   1644 F0                 3408 	movx	@dptr,a
                           3409 ;src/qs1r_main.c:480: for (i=0; i < 0x100; i++) {
                           3410 ;     genPlus
                           3411 ;     genPlusIncr
   1645 0A                 3412 	inc	r2
                           3413 ;	Peephole 112.b	changed ljmp to sjmp
                           3414 ;	Peephole 243	avoided branch to sjmp
   1646 BA 00 C7           3415 	cjne	r2,#0x00,00287$
   1649 0B                 3416 	inc	r3
   164A                    3417 00443$:
   164A 80 C4              3418 	sjmp	00287$
   164C                    3419 00260$:
                           3420 ;src/qs1r_main.c:489: } else return 0;
                           3421 ;     genRet
   164C 75 82 00           3422 	mov	dpl,#0x00
                           3423 ;src/qs1r_main.c:492: default:
                           3424 ;	Peephole 112.b	changed ljmp to sjmp
                           3425 ;	Peephole 251.b	replaced sjmp to ret with ret
   164F 22                 3426 	ret
   1650                    3427 00265$:
                           3428 ;src/qs1r_main.c:493: return 0;
                           3429 ;     genRet
   1650 75 82 00           3430 	mov	dpl,#0x00
                           3431 ;src/qs1r_main.c:494: }
                           3432 ;	Peephole 112.b	changed ljmp to sjmp
                           3433 ;src/qs1r_main.c:495: return 1;
                           3434 ;     genRet
                           3435 ;	Peephole 237.a	removed sjmp to ret
   1653 22                 3436 	ret
   1654                    3437 00266$:
   1654 75 82 01           3438 	mov	dpl,#0x01
   1657                    3439 00291$:
   1657 22                 3440 	ret
                           3441 ;------------------------------------------------------------
                           3442 ;Allocation info for local variables in function 'app_vendor_IN_cmd'
                           3443 ;------------------------------------------------------------
                           3444 ;i                         Allocated to registers r3 r4 
                           3445 ;j                         Allocated to registers r5 r6 
                           3446 ;offset                    Allocated with name '_app_vendor_IN_cmd_offset_1_1'
                           3447 ;foop                      Allocated to registers 
                           3448 ;------------------------------------------------------------
                           3449 ;src/qs1r_main.c:498: unsigned char app_vendor_IN_cmd(void)
                           3450 ;	-----------------------------------------
                           3451 ;	 function app_vendor_IN_cmd
                           3452 ;	-----------------------------------------
   1658                    3453 _app_vendor_IN_cmd:
                           3454 ;src/qs1r_main.c:504: switch (bRequest)
                           3455 ;     genPointerGet
                           3456 ;     genFarPointerGet
   1658 90 E6 B9           3457 	mov	dptr,#(_SETUPDAT + 0x0001)
   165B E0                 3458 	movx	a,@dptr
   165C FA                 3459 	mov	r2,a
                           3460 ;     genCmpLt
                           3461 ;     genCmp
   165D BA 81 00           3462 	cjne	r2,#0x81,00288$
   1660                    3463 00288$:
                           3464 ;     genIfxJump
   1660 50 03              3465 	jnc	00289$
   1662 02 1F 4C           3466 	ljmp	00237$
   1665                    3467 00289$:
                           3468 ;     genCmpGt
                           3469 ;     genCmp
                           3470 ;     genIfxJump
                           3471 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
   1665 EA                 3472 	mov	a,r2
   1666 24 57              3473 	add	a,#0xff - 0xA8
   1668 50 03              3474 	jnc	00290$
   166A 02 1F 4C           3475 	ljmp	00237$
   166D                    3476 00290$:
                           3477 ;     genMinus
   166D EA                 3478 	mov	a,r2
   166E 24 7F              3479 	add	a,#0x7f
                           3480 ;     genJumpTab
                           3481 ;	Peephole 105	removed redundant mov
   1670 FA                 3482 	mov	r2,a
   1671 24 09              3483 	add	a,#(00291$-3-.)
   1673 83                 3484 	movc	a,@a+pc
   1674 C0 E0              3485 	push	acc
   1676 EA                 3486 	mov	a,r2
   1677 24 2B              3487 	add	a,#(00292$-3-.)
   1679 83                 3488 	movc	a,@a+pc
   167A C0 E0              3489 	push	acc
   167C 22                 3490 	ret
   167D                    3491 00291$:
   167D CD                 3492 	.db	00101$
   167E FB                 3493 	.db	00104$
   167F 71                 3494 	.db	00109$
   1680 3B                 3495 	.db	00107$
   1681 56                 3496 	.db	00108$
   1682 74                 3497 	.db	00110$
   1683 8B                 3498 	.db	00111$
   1684 A2                 3499 	.db	00112$
   1685 B9                 3500 	.db	00113$
   1686 D0                 3501 	.db	00114$
   1687 E7                 3502 	.db	00115$
   1688 FE                 3503 	.db	00116$
   1689 15                 3504 	.db	00117$
   168A 2C                 3505 	.db	00118$
   168B 43                 3506 	.db	00119$
   168C 5A                 3507 	.db	00120$
   168D 71                 3508 	.db	00121$
   168E 88                 3509 	.db	00122$
   168F 9F                 3510 	.db	00123$
   1690 B6                 3511 	.db	00124$
   1691 CD                 3512 	.db	00125$
   1692 E4                 3513 	.db	00126$
   1693 FB                 3514 	.db	00127$
   1694 12                 3515 	.db	00128$
   1695 29                 3516 	.db	00129$
   1696 40                 3517 	.db	00130$
   1697 8F                 3518 	.db	00137$
   1698 E0                 3519 	.db	00144$
   1699 67                 3520 	.db	00151$
   169A EE                 3521 	.db	00158$
   169B 51                 3522 	.db	00165$
   169C A2                 3523 	.db	00172$
   169D FB                 3524 	.db	00179$
   169E 4C                 3525 	.db	00186$
   169F 9D                 3526 	.db	00193$
   16A0 EE                 3527 	.db	00200$
   16A1 3F                 3528 	.db	00207$
   16A2 90                 3529 	.db	00214$
   16A3 E1                 3530 	.db	00221$
   16A4 32                 3531 	.db	00228$
   16A5                    3532 00292$:
   16A5 16                 3533 	.db	00101$>>8
   16A6 16                 3534 	.db	00104$>>8
   16A7 17                 3535 	.db	00109$>>8
   16A8 17                 3536 	.db	00107$>>8
   16A9 17                 3537 	.db	00108$>>8
   16AA 17                 3538 	.db	00110$>>8
   16AB 17                 3539 	.db	00111$>>8
   16AC 17                 3540 	.db	00112$>>8
   16AD 17                 3541 	.db	00113$>>8
   16AE 17                 3542 	.db	00114$>>8
   16AF 17                 3543 	.db	00115$>>8
   16B0 17                 3544 	.db	00116$>>8
   16B1 18                 3545 	.db	00117$>>8
   16B2 18                 3546 	.db	00118$>>8
   16B3 18                 3547 	.db	00119$>>8
   16B4 18                 3548 	.db	00120$>>8
   16B5 18                 3549 	.db	00121$>>8
   16B6 18                 3550 	.db	00122$>>8
   16B7 18                 3551 	.db	00123$>>8
   16B8 18                 3552 	.db	00124$>>8
   16B9 18                 3553 	.db	00125$>>8
   16BA 18                 3554 	.db	00126$>>8
   16BB 18                 3555 	.db	00127$>>8
   16BC 19                 3556 	.db	00128$>>8
   16BD 19                 3557 	.db	00129$>>8
   16BE 19                 3558 	.db	00130$>>8
   16BF 19                 3559 	.db	00137$>>8
   16C0 19                 3560 	.db	00144$>>8
   16C1 1A                 3561 	.db	00151$>>8
   16C2 1A                 3562 	.db	00158$>>8
   16C3 1B                 3563 	.db	00165$>>8
   16C4 1B                 3564 	.db	00172$>>8
   16C5 1B                 3565 	.db	00179$>>8
   16C6 1C                 3566 	.db	00186$>>8
   16C7 1C                 3567 	.db	00193$>>8
   16C8 1C                 3568 	.db	00200$>>8
   16C9 1D                 3569 	.db	00207$>>8
   16CA 1D                 3570 	.db	00214$>>8
   16CB 1D                 3571 	.db	00221$>>8
   16CC 1E                 3572 	.db	00228$>>8
                           3573 ;src/qs1r_main.c:506: case VRQ_I2C_READ:
   16CD                    3574 00101$:
                           3575 ;src/qs1r_main.c:507: if (!i2c_read (wValueL, EP0BUF, wLengthL))
                           3576 ;     genPointerGet
                           3577 ;     genFarPointerGet
   16CD 90 E6 BA           3578 	mov	dptr,#(_SETUPDAT + 0x0002)
   16D0 E0                 3579 	movx	a,@dptr
   16D1 FA                 3580 	mov	r2,a
                           3581 ;     genAddrOf
   16D2 75 2B 40           3582 	mov	_i2c_read_PARM_2,#_EP0BUF
   16D5 75 2C E7           3583 	mov	(_i2c_read_PARM_2 + 1),#(_EP0BUF >> 8)
                           3584 ;     genPointerGet
                           3585 ;     genFarPointerGet
   16D8 90 E6 BE           3586 	mov	dptr,#(_SETUPDAT + 0x0006)
   16DB E0                 3587 	movx	a,@dptr
   16DC F5 2D              3588 	mov	_i2c_read_PARM_3,a
                           3589 ;     genCall
   16DE 8A 82              3590 	mov	dpl,r2
   16E0 12 03 47           3591 	lcall	_i2c_read
   16E3 E5 82              3592 	mov	a,dpl
                           3593 ;     genIfx
                           3594 ;     genIfxJump
                           3595 ;	Peephole 109	removed ljmp by inverse jump logic
                           3596 ;src/qs1r_main.c:508: return 0;
                           3597 ;     genRet
                           3598 ;	Peephole 256.c	loading dpl with zero from a
   16E5 70 03              3599 	jnz	00103$
   16E7                    3600 00293$:
   16E7 F5 82              3601 	mov	dpl,a
                           3602 ;	Peephole 251.a	replaced ljmp to ret with ret
   16E9 22                 3603 	ret
   16EA                    3604 00103$:
                           3605 ;src/qs1r_main.c:509: EP0BCH = 0;
                           3606 ;     genAssign
   16EA 90 E6 8A           3607 	mov	dptr,#_EP0BCH
                           3608 ;	Peephole 181	changed mov to clr
   16ED E4                 3609 	clr	a
   16EE F0                 3610 	movx	@dptr,a
                           3611 ;src/qs1r_main.c:510: EP0BCL = wLengthL;
                           3612 ;     genPointerGet
                           3613 ;     genFarPointerGet
   16EF 90 E6 BE           3614 	mov	dptr,#(_SETUPDAT + 0x0006)
   16F2 E0                 3615 	movx	a,@dptr
                           3616 ;     genAssign
                           3617 ;	Peephole 100	removed redundant mov
   16F3 FA                 3618 	mov	r2,a
   16F4 90 E6 8B           3619 	mov	dptr,#_EP0BCL
   16F7 F0                 3620 	movx	@dptr,a
                           3621 ;src/qs1r_main.c:511: break;
   16F8 02 1F 50           3622 	ljmp	00238$
                           3623 ;src/qs1r_main.c:513: case VRQ_SPI_READ:
   16FB                    3624 00104$:
                           3625 ;src/qs1r_main.c:514: if (!spi_read (wValueH, wValueL, wIndexH, wIndexL, EP0BUF, wLengthL))
                           3626 ;     genPointerGet
                           3627 ;     genFarPointerGet
   16FB 90 E6 BB           3628 	mov	dptr,#(_SETUPDAT + 0x0003)
   16FE E0                 3629 	movx	a,@dptr
   16FF FA                 3630 	mov	r2,a
                           3631 ;     genPointerGet
                           3632 ;     genFarPointerGet
   1700 90 E6 BA           3633 	mov	dptr,#(_SETUPDAT + 0x0002)
   1703 E0                 3634 	movx	a,@dptr
   1704 F5 0F              3635 	mov	_spi_read_PARM_2,a
                           3636 ;     genPointerGet
                           3637 ;     genFarPointerGet
   1706 90 E6 BD           3638 	mov	dptr,#(_SETUPDAT + 0x0005)
   1709 E0                 3639 	movx	a,@dptr
   170A F5 10              3640 	mov	_spi_read_PARM_3,a
                           3641 ;     genPointerGet
                           3642 ;     genFarPointerGet
   170C 90 E6 BC           3643 	mov	dptr,#(_SETUPDAT + 0x0004)
   170F E0                 3644 	movx	a,@dptr
   1710 F5 11              3645 	mov	_spi_read_PARM_4,a
                           3646 ;     genAddrOf
   1712 75 12 40           3647 	mov	_spi_read_PARM_5,#_EP0BUF
   1715 75 13 E7           3648 	mov	(_spi_read_PARM_5 + 1),#(_EP0BUF >> 8)
                           3649 ;     genPointerGet
                           3650 ;     genFarPointerGet
   1718 90 E6 BE           3651 	mov	dptr,#(_SETUPDAT + 0x0006)
   171B E0                 3652 	movx	a,@dptr
   171C F5 14              3653 	mov	_spi_read_PARM_6,a
                           3654 ;     genCall
   171E 8A 82              3655 	mov	dpl,r2
   1720 12 06 BB           3656 	lcall	_spi_read
   1723 E5 82              3657 	mov	a,dpl
                           3658 ;     genIfx
                           3659 ;     genIfxJump
                           3660 ;	Peephole 109	removed ljmp by inverse jump logic
                           3661 ;src/qs1r_main.c:515: return 0;
                           3662 ;     genRet
                           3663 ;	Peephole 256.c	loading dpl with zero from a
   1725 70 03              3664 	jnz	00106$
   1727                    3665 00294$:
   1727 F5 82              3666 	mov	dpl,a
                           3667 ;	Peephole 251.a	replaced ljmp to ret with ret
   1729 22                 3668 	ret
   172A                    3669 00106$:
                           3670 ;src/qs1r_main.c:516: EP0BCH = 0;
                           3671 ;     genAssign
   172A 90 E6 8A           3672 	mov	dptr,#_EP0BCH
                           3673 ;	Peephole 181	changed mov to clr
   172D E4                 3674 	clr	a
   172E F0                 3675 	movx	@dptr,a
                           3676 ;src/qs1r_main.c:517: EP0BCL = wLengthL;
                           3677 ;     genPointerGet
                           3678 ;     genFarPointerGet
   172F 90 E6 BE           3679 	mov	dptr,#(_SETUPDAT + 0x0006)
   1732 E0                 3680 	movx	a,@dptr
                           3681 ;     genAssign
                           3682 ;	Peephole 100	removed redundant mov
   1733 FA                 3683 	mov	r2,a
   1734 90 E6 8B           3684 	mov	dptr,#_EP0BCL
   1737 F0                 3685 	movx	@dptr,a
                           3686 ;src/qs1r_main.c:518: break;
   1738 02 1F 50           3687 	ljmp	00238$
                           3688 ;src/qs1r_main.c:520: case VRQ_EEPROM_TYPE_READ:
   173B                    3689 00107$:
                           3690 ;src/qs1r_main.c:521: EP0BUF[0] = I2CS & bmID; // 16 = 2 byte, 8 = 1 byte
                           3691 ;     genAssign
   173B 90 E6 78           3692 	mov	dptr,#_I2CS
   173E E0                 3693 	movx	a,@dptr
   173F FA                 3694 	mov	r2,a
                           3695 ;     genAnd
   1740 53 02 18           3696 	anl	ar2,#0x18
                           3697 ;     genPointerSet
                           3698 ;     genFarPointerSet
   1743 90 E7 40           3699 	mov	dptr,#_EP0BUF
   1746 EA                 3700 	mov	a,r2
   1747 F0                 3701 	movx	@dptr,a
                           3702 ;src/qs1r_main.c:522: EP0BCH = 0;
                           3703 ;     genAssign
   1748 90 E6 8A           3704 	mov	dptr,#_EP0BCH
                           3705 ;	Peephole 181	changed mov to clr
   174B E4                 3706 	clr	a
   174C F0                 3707 	movx	@dptr,a
                           3708 ;src/qs1r_main.c:523: EP0BCL = 1;
                           3709 ;     genAssign
   174D 90 E6 8B           3710 	mov	dptr,#_EP0BCL
   1750 74 01              3711 	mov	a,#0x01
   1752 F0                 3712 	movx	@dptr,a
                           3713 ;src/qs1r_main.c:524: break;
   1753 02 1F 50           3714 	ljmp	00238$
                           3715 ;src/qs1r_main.c:526: case VRQ_I2C_SPEED_READ:
   1756                    3716 00108$:
                           3717 ;src/qs1r_main.c:527: EP0BUF[0] = I2CTL & bm400KHZ; // 0 = 100 kHz, 1 = 400 kHz
                           3718 ;     genAssign
   1756 90 E6 7A           3719 	mov	dptr,#_I2CTL
   1759 E0                 3720 	movx	a,@dptr
   175A FA                 3721 	mov	r2,a
                           3722 ;     genAnd
   175B 53 02 01           3723 	anl	ar2,#0x01
                           3724 ;     genPointerSet
                           3725 ;     genFarPointerSet
   175E 90 E7 40           3726 	mov	dptr,#_EP0BUF
   1761 EA                 3727 	mov	a,r2
   1762 F0                 3728 	movx	@dptr,a
                           3729 ;src/qs1r_main.c:528: EP0BCH = 0;
                           3730 ;     genAssign
   1763 90 E6 8A           3731 	mov	dptr,#_EP0BCH
                           3732 ;	Peephole 181	changed mov to clr
   1766 E4                 3733 	clr	a
   1767 F0                 3734 	movx	@dptr,a
                           3735 ;src/qs1r_main.c:529: EP0BCL = 1;
                           3736 ;     genAssign
   1768 90 E6 8B           3737 	mov	dptr,#_EP0BCL
   176B 74 01              3738 	mov	a,#0x01
   176D F0                 3739 	movx	@dptr,a
                           3740 ;src/qs1r_main.c:530: break;
   176E 02 1F 50           3741 	ljmp	00238$
                           3742 ;src/qs1r_main.c:532: case VRQ_SN_READ:
   1771                    3743 00109$:
                           3744 ;src/qs1r_main.c:533: break;
   1771 02 1F 50           3745 	ljmp	00238$
                           3746 ;src/qs1r_main.c:535: case VRQ_DEBUG_LED_READ:
   1774                    3747 00110$:
                           3748 ;src/qs1r_main.c:536: EP0BUF[0] = DEBUG_LED_REG;
                           3749 ;     genAssign
   1774 90 60 22           3750 	mov	dptr,#_DEBUG_LED_REG
   1777 E0                 3751 	movx	a,@dptr
                           3752 ;     genPointerSet
                           3753 ;     genFarPointerSet
                           3754 ;	Peephole 100	removed redundant mov
   1778 FA                 3755 	mov	r2,a
   1779 90 E7 40           3756 	mov	dptr,#_EP0BUF
   177C F0                 3757 	movx	@dptr,a
                           3758 ;src/qs1r_main.c:537: EP0BCH = 0;
                           3759 ;     genAssign
   177D 90 E6 8A           3760 	mov	dptr,#_EP0BCH
                           3761 ;	Peephole 181	changed mov to clr
   1780 E4                 3762 	clr	a
   1781 F0                 3763 	movx	@dptr,a
                           3764 ;src/qs1r_main.c:538: EP0BCL = 1;
                           3765 ;     genAssign
   1782 90 E6 8B           3766 	mov	dptr,#_EP0BCL
   1785 74 01              3767 	mov	a,#0x01
   1787 F0                 3768 	movx	@dptr,a
                           3769 ;src/qs1r_main.c:539: break;
   1788 02 1F 50           3770 	ljmp	00238$
                           3771 ;src/qs1r_main.c:541: case VRQ_LTC2208_CNTRL_READ:
   178B                    3772 00111$:
                           3773 ;src/qs1r_main.c:542: EP0BUF[0] = LTC2208_CNTRL_REG;
                           3774 ;     genAssign
   178B 90 60 21           3775 	mov	dptr,#_LTC2208_CNTRL_REG
   178E E0                 3776 	movx	a,@dptr
                           3777 ;     genPointerSet
                           3778 ;     genFarPointerSet
                           3779 ;	Peephole 100	removed redundant mov
   178F FA                 3780 	mov	r2,a
   1790 90 E7 40           3781 	mov	dptr,#_EP0BUF
   1793 F0                 3782 	movx	@dptr,a
                           3783 ;src/qs1r_main.c:543: EP0BCH = 0;
                           3784 ;     genAssign
   1794 90 E6 8A           3785 	mov	dptr,#_EP0BCH
                           3786 ;	Peephole 181	changed mov to clr
   1797 E4                 3787 	clr	a
   1798 F0                 3788 	movx	@dptr,a
                           3789 ;src/qs1r_main.c:544: EP0BCL = 1;
                           3790 ;     genAssign
   1799 90 E6 8B           3791 	mov	dptr,#_EP0BCL
   179C 74 01              3792 	mov	a,#0x01
   179E F0                 3793 	movx	@dptr,a
                           3794 ;src/qs1r_main.c:545: break;
   179F 02 1F 50           3795 	ljmp	00238$
                           3796 ;src/qs1r_main.c:547: case VRQ_ATTN_READ:
   17A2                    3797 00112$:
                           3798 ;src/qs1r_main.c:548: EP0BUF[0] = ATTN;
                           3799 ;     genAssign
   17A2 90 60 23           3800 	mov	dptr,#_ATTN
   17A5 E0                 3801 	movx	a,@dptr
                           3802 ;     genPointerSet
                           3803 ;     genFarPointerSet
                           3804 ;	Peephole 100	removed redundant mov
   17A6 FA                 3805 	mov	r2,a
   17A7 90 E7 40           3806 	mov	dptr,#_EP0BUF
   17AA F0                 3807 	movx	@dptr,a
                           3808 ;src/qs1r_main.c:549: EP0BCH = 0;
                           3809 ;     genAssign
   17AB 90 E6 8A           3810 	mov	dptr,#_EP0BCH
                           3811 ;	Peephole 181	changed mov to clr
   17AE E4                 3812 	clr	a
   17AF F0                 3813 	movx	@dptr,a
                           3814 ;src/qs1r_main.c:550: EP0BCL = 1;
                           3815 ;     genAssign
   17B0 90 E6 8B           3816 	mov	dptr,#_EP0BCL
   17B3 74 01              3817 	mov	a,#0x01
   17B5 F0                 3818 	movx	@dptr,a
                           3819 ;src/qs1r_main.c:551: break;
   17B6 02 1F 50           3820 	ljmp	00238$
                           3821 ;src/qs1r_main.c:553: case VRQ_RFFE_READ:
   17B9                    3822 00113$:
                           3823 ;src/qs1r_main.c:554: EP0BUF[0] = RFFE_CNTRL;
                           3824 ;     genAssign
   17B9 90 60 24           3825 	mov	dptr,#_RFFE_CNTRL
   17BC E0                 3826 	movx	a,@dptr
                           3827 ;     genPointerSet
                           3828 ;     genFarPointerSet
                           3829 ;	Peephole 100	removed redundant mov
   17BD FA                 3830 	mov	r2,a
   17BE 90 E7 40           3831 	mov	dptr,#_EP0BUF
   17C1 F0                 3832 	movx	@dptr,a
                           3833 ;src/qs1r_main.c:555: EP0BCH = 0;
                           3834 ;     genAssign
   17C2 90 E6 8A           3835 	mov	dptr,#_EP0BCH
                           3836 ;	Peephole 181	changed mov to clr
   17C5 E4                 3837 	clr	a
   17C6 F0                 3838 	movx	@dptr,a
                           3839 ;src/qs1r_main.c:556: EP0BCL = 1;
                           3840 ;     genAssign
   17C7 90 E6 8B           3841 	mov	dptr,#_EP0BCL
   17CA 74 01              3842 	mov	a,#0x01
   17CC F0                 3843 	movx	@dptr,a
                           3844 ;src/qs1r_main.c:557: break;
   17CD 02 1F 50           3845 	ljmp	00238$
                           3846 ;src/qs1r_main.c:559: case VRQ_DDC_CNTRL_READ:
   17D0                    3847 00114$:
                           3848 ;src/qs1r_main.c:560: EP0BUF[0] = DDC_CNTRL;
                           3849 ;     genAssign
   17D0 90 60 20           3850 	mov	dptr,#_DDC_CNTRL
   17D3 E0                 3851 	movx	a,@dptr
                           3852 ;     genPointerSet
                           3853 ;     genFarPointerSet
                           3854 ;	Peephole 100	removed redundant mov
   17D4 FA                 3855 	mov	r2,a
   17D5 90 E7 40           3856 	mov	dptr,#_EP0BUF
   17D8 F0                 3857 	movx	@dptr,a
                           3858 ;src/qs1r_main.c:561: EP0BCH = 0;
                           3859 ;     genAssign
   17D9 90 E6 8A           3860 	mov	dptr,#_EP0BCH
                           3861 ;	Peephole 181	changed mov to clr
   17DC E4                 3862 	clr	a
   17DD F0                 3863 	movx	@dptr,a
                           3864 ;src/qs1r_main.c:562: EP0BCL = 1;
                           3865 ;     genAssign
   17DE 90 E6 8B           3866 	mov	dptr,#_EP0BCL
   17E1 74 01              3867 	mov	a,#0x01
   17E3 F0                 3868 	movx	@dptr,a
                           3869 ;src/qs1r_main.c:563: break;
   17E4 02 1F 50           3870 	ljmp	00238$
                           3871 ;src/qs1r_main.c:565: case VRQ_CPLD_GPIO_READ:
   17E7                    3872 00115$:
                           3873 ;src/qs1r_main.c:566: EP0BUF[0] = CPLD_GPIO;
                           3874 ;     genAssign
   17E7 90 60 25           3875 	mov	dptr,#_CPLD_GPIO
   17EA E0                 3876 	movx	a,@dptr
                           3877 ;     genPointerSet
                           3878 ;     genFarPointerSet
                           3879 ;	Peephole 100	removed redundant mov
   17EB FA                 3880 	mov	r2,a
   17EC 90 E7 40           3881 	mov	dptr,#_EP0BUF
   17EF F0                 3882 	movx	@dptr,a
                           3883 ;src/qs1r_main.c:567: EP0BCH = 0;
                           3884 ;     genAssign
   17F0 90 E6 8A           3885 	mov	dptr,#_EP0BCH
                           3886 ;	Peephole 181	changed mov to clr
   17F3 E4                 3887 	clr	a
   17F4 F0                 3888 	movx	@dptr,a
                           3889 ;src/qs1r_main.c:568: EP0BCL = 1;
                           3890 ;     genAssign
   17F5 90 E6 8B           3891 	mov	dptr,#_EP0BCL
   17F8 74 01              3892 	mov	a,#0x01
   17FA F0                 3893 	movx	@dptr,a
                           3894 ;src/qs1r_main.c:569: break;
   17FB 02 1F 50           3895 	ljmp	00238$
                           3896 ;src/qs1r_main.c:571: case VRQ_DDC0_DR0_READ:
   17FE                    3897 00116$:
                           3898 ;src/qs1r_main.c:572: EP0BUF[0] = DDC0_DR0;
                           3899 ;     genAssign
   17FE 90 60 00           3900 	mov	dptr,#_DDC0_DR0
   1801 E0                 3901 	movx	a,@dptr
                           3902 ;     genPointerSet
                           3903 ;     genFarPointerSet
                           3904 ;	Peephole 100	removed redundant mov
   1802 FA                 3905 	mov	r2,a
   1803 90 E7 40           3906 	mov	dptr,#_EP0BUF
   1806 F0                 3907 	movx	@dptr,a
                           3908 ;src/qs1r_main.c:573: EP0BCH = 0;
                           3909 ;     genAssign
   1807 90 E6 8A           3910 	mov	dptr,#_EP0BCH
                           3911 ;	Peephole 181	changed mov to clr
   180A E4                 3912 	clr	a
   180B F0                 3913 	movx	@dptr,a
                           3914 ;src/qs1r_main.c:574: EP0BCL = 1;
                           3915 ;     genAssign
   180C 90 E6 8B           3916 	mov	dptr,#_EP0BCL
   180F 74 01              3917 	mov	a,#0x01
   1811 F0                 3918 	movx	@dptr,a
                           3919 ;src/qs1r_main.c:575: break;
   1812 02 1F 50           3920 	ljmp	00238$
                           3921 ;src/qs1r_main.c:577: case VRQ_DDC0_DR1_READ:
   1815                    3922 00117$:
                           3923 ;src/qs1r_main.c:578: EP0BUF[0] = DDC0_DR1;
                           3924 ;     genAssign
   1815 90 60 01           3925 	mov	dptr,#_DDC0_DR1
   1818 E0                 3926 	movx	a,@dptr
                           3927 ;     genPointerSet
                           3928 ;     genFarPointerSet
                           3929 ;	Peephole 100	removed redundant mov
   1819 FA                 3930 	mov	r2,a
   181A 90 E7 40           3931 	mov	dptr,#_EP0BUF
   181D F0                 3932 	movx	@dptr,a
                           3933 ;src/qs1r_main.c:579: EP0BCH = 0;
                           3934 ;     genAssign
   181E 90 E6 8A           3935 	mov	dptr,#_EP0BCH
                           3936 ;	Peephole 181	changed mov to clr
   1821 E4                 3937 	clr	a
   1822 F0                 3938 	movx	@dptr,a
                           3939 ;src/qs1r_main.c:580: EP0BCL = 1;
                           3940 ;     genAssign
   1823 90 E6 8B           3941 	mov	dptr,#_EP0BCL
   1826 74 01              3942 	mov	a,#0x01
   1828 F0                 3943 	movx	@dptr,a
                           3944 ;src/qs1r_main.c:581: break;
   1829 02 1F 50           3945 	ljmp	00238$
                           3946 ;src/qs1r_main.c:583: case VRQ_DDC0_DR2_READ:
   182C                    3947 00118$:
                           3948 ;src/qs1r_main.c:584: EP0BUF[0] = DDC0_DR2;
                           3949 ;     genAssign
   182C 90 60 02           3950 	mov	dptr,#_DDC0_DR2
   182F E0                 3951 	movx	a,@dptr
                           3952 ;     genPointerSet
                           3953 ;     genFarPointerSet
                           3954 ;	Peephole 100	removed redundant mov
   1830 FA                 3955 	mov	r2,a
   1831 90 E7 40           3956 	mov	dptr,#_EP0BUF
   1834 F0                 3957 	movx	@dptr,a
                           3958 ;src/qs1r_main.c:585: EP0BCH = 0;
                           3959 ;     genAssign
   1835 90 E6 8A           3960 	mov	dptr,#_EP0BCH
                           3961 ;	Peephole 181	changed mov to clr
   1838 E4                 3962 	clr	a
   1839 F0                 3963 	movx	@dptr,a
                           3964 ;src/qs1r_main.c:586: EP0BCL = 1;
                           3965 ;     genAssign
   183A 90 E6 8B           3966 	mov	dptr,#_EP0BCL
   183D 74 01              3967 	mov	a,#0x01
   183F F0                 3968 	movx	@dptr,a
                           3969 ;src/qs1r_main.c:587: break;
   1840 02 1F 50           3970 	ljmp	00238$
                           3971 ;src/qs1r_main.c:589: case VRQ_DDC0_DR3_READ:
   1843                    3972 00119$:
                           3973 ;src/qs1r_main.c:590: EP0BUF[0] = DDC0_DR3;
                           3974 ;     genAssign
   1843 90 60 03           3975 	mov	dptr,#_DDC0_DR3
   1846 E0                 3976 	movx	a,@dptr
                           3977 ;     genPointerSet
                           3978 ;     genFarPointerSet
                           3979 ;	Peephole 100	removed redundant mov
   1847 FA                 3980 	mov	r2,a
   1848 90 E7 40           3981 	mov	dptr,#_EP0BUF
   184B F0                 3982 	movx	@dptr,a
                           3983 ;src/qs1r_main.c:591: EP0BCH = 0;
                           3984 ;     genAssign
   184C 90 E6 8A           3985 	mov	dptr,#_EP0BCH
                           3986 ;	Peephole 181	changed mov to clr
   184F E4                 3987 	clr	a
   1850 F0                 3988 	movx	@dptr,a
                           3989 ;src/qs1r_main.c:592: EP0BCL = 1;
                           3990 ;     genAssign
   1851 90 E6 8B           3991 	mov	dptr,#_EP0BCL
   1854 74 01              3992 	mov	a,#0x01
   1856 F0                 3993 	movx	@dptr,a
                           3994 ;src/qs1r_main.c:593: break;
   1857 02 1F 50           3995 	ljmp	00238$
                           3996 ;src/qs1r_main.c:595: case VRQ_DDC0_DR4_READ:
   185A                    3997 00120$:
                           3998 ;src/qs1r_main.c:596: EP0BUF[0] = DDC0_DR4;
                           3999 ;     genAssign
   185A 90 60 04           4000 	mov	dptr,#_DDC0_DR4
   185D E0                 4001 	movx	a,@dptr
                           4002 ;     genPointerSet
                           4003 ;     genFarPointerSet
                           4004 ;	Peephole 100	removed redundant mov
   185E FA                 4005 	mov	r2,a
   185F 90 E7 40           4006 	mov	dptr,#_EP0BUF
   1862 F0                 4007 	movx	@dptr,a
                           4008 ;src/qs1r_main.c:597: EP0BCH = 0;
                           4009 ;     genAssign
   1863 90 E6 8A           4010 	mov	dptr,#_EP0BCH
                           4011 ;	Peephole 181	changed mov to clr
   1866 E4                 4012 	clr	a
   1867 F0                 4013 	movx	@dptr,a
                           4014 ;src/qs1r_main.c:598: EP0BCL = 1;
                           4015 ;     genAssign
   1868 90 E6 8B           4016 	mov	dptr,#_EP0BCL
   186B 74 01              4017 	mov	a,#0x01
   186D F0                 4018 	movx	@dptr,a
                           4019 ;src/qs1r_main.c:599: break;
   186E 02 1F 50           4020 	ljmp	00238$
                           4021 ;src/qs1r_main.c:601: case VRQ_DDC0_LAR_READ:
   1871                    4022 00121$:
                           4023 ;src/qs1r_main.c:602: EP0BUF[0] = DDC0_LAR;
                           4024 ;     genAssign
   1871 90 60 06           4025 	mov	dptr,#_DDC0_LAR
   1874 E0                 4026 	movx	a,@dptr
                           4027 ;     genPointerSet
                           4028 ;     genFarPointerSet
                           4029 ;	Peephole 100	removed redundant mov
   1875 FA                 4030 	mov	r2,a
   1876 90 E7 40           4031 	mov	dptr,#_EP0BUF
   1879 F0                 4032 	movx	@dptr,a
                           4033 ;src/qs1r_main.c:603: EP0BCH = 0;
                           4034 ;     genAssign
   187A 90 E6 8A           4035 	mov	dptr,#_EP0BCH
                           4036 ;	Peephole 181	changed mov to clr
   187D E4                 4037 	clr	a
   187E F0                 4038 	movx	@dptr,a
                           4039 ;src/qs1r_main.c:604: EP0BCL = 1;
                           4040 ;     genAssign
   187F 90 E6 8B           4041 	mov	dptr,#_EP0BCL
   1882 74 01              4042 	mov	a,#0x01
   1884 F0                 4043 	movx	@dptr,a
                           4044 ;src/qs1r_main.c:605: break;
   1885 02 1F 50           4045 	ljmp	00238$
                           4046 ;src/qs1r_main.c:607: case VRQ_DDC0_AMR_READ:
   1888                    4047 00122$:
                           4048 ;src/qs1r_main.c:608: EP0BUF[0] = DDC0_AMR;
                           4049 ;     genAssign
   1888 90 60 07           4050 	mov	dptr,#_DDC0_AMR
   188B E0                 4051 	movx	a,@dptr
                           4052 ;     genPointerSet
                           4053 ;     genFarPointerSet
                           4054 ;	Peephole 100	removed redundant mov
   188C FA                 4055 	mov	r2,a
   188D 90 E7 40           4056 	mov	dptr,#_EP0BUF
   1890 F0                 4057 	movx	@dptr,a
                           4058 ;src/qs1r_main.c:609: EP0BCH = 0;
                           4059 ;     genAssign
   1891 90 E6 8A           4060 	mov	dptr,#_EP0BCH
                           4061 ;	Peephole 181	changed mov to clr
   1894 E4                 4062 	clr	a
   1895 F0                 4063 	movx	@dptr,a
                           4064 ;src/qs1r_main.c:610: EP0BCL = 1;
                           4065 ;     genAssign
   1896 90 E6 8B           4066 	mov	dptr,#_EP0BCL
   1899 74 01              4067 	mov	a,#0x01
   189B F0                 4068 	movx	@dptr,a
                           4069 ;src/qs1r_main.c:611: break;
   189C 02 1F 50           4070 	ljmp	00238$
                           4071 ;src/qs1r_main.c:613: case VRQ_DDC1_DR0_READ:
   189F                    4072 00123$:
                           4073 ;src/qs1r_main.c:614: EP0BUF[0] = DDC1_DR0;
                           4074 ;     genAssign
   189F 90 60 10           4075 	mov	dptr,#_DDC1_DR0
   18A2 E0                 4076 	movx	a,@dptr
                           4077 ;     genPointerSet
                           4078 ;     genFarPointerSet
                           4079 ;	Peephole 100	removed redundant mov
   18A3 FA                 4080 	mov	r2,a
   18A4 90 E7 40           4081 	mov	dptr,#_EP0BUF
   18A7 F0                 4082 	movx	@dptr,a
                           4083 ;src/qs1r_main.c:615: EP0BCH = 0;
                           4084 ;     genAssign
   18A8 90 E6 8A           4085 	mov	dptr,#_EP0BCH
                           4086 ;	Peephole 181	changed mov to clr
   18AB E4                 4087 	clr	a
   18AC F0                 4088 	movx	@dptr,a
                           4089 ;src/qs1r_main.c:616: EP0BCL = 1;
                           4090 ;     genAssign
   18AD 90 E6 8B           4091 	mov	dptr,#_EP0BCL
   18B0 74 01              4092 	mov	a,#0x01
   18B2 F0                 4093 	movx	@dptr,a
                           4094 ;src/qs1r_main.c:617: break;
   18B3 02 1F 50           4095 	ljmp	00238$
                           4096 ;src/qs1r_main.c:619: case VRQ_DDC1_DR1_READ:
   18B6                    4097 00124$:
                           4098 ;src/qs1r_main.c:620: EP0BUF[0] = DDC1_DR1;
                           4099 ;     genAssign
   18B6 90 60 11           4100 	mov	dptr,#_DDC1_DR1
   18B9 E0                 4101 	movx	a,@dptr
                           4102 ;     genPointerSet
                           4103 ;     genFarPointerSet
                           4104 ;	Peephole 100	removed redundant mov
   18BA FA                 4105 	mov	r2,a
   18BB 90 E7 40           4106 	mov	dptr,#_EP0BUF
   18BE F0                 4107 	movx	@dptr,a
                           4108 ;src/qs1r_main.c:621: EP0BCH = 0;
                           4109 ;     genAssign
   18BF 90 E6 8A           4110 	mov	dptr,#_EP0BCH
                           4111 ;	Peephole 181	changed mov to clr
   18C2 E4                 4112 	clr	a
   18C3 F0                 4113 	movx	@dptr,a
                           4114 ;src/qs1r_main.c:622: EP0BCL = 1;
                           4115 ;     genAssign
   18C4 90 E6 8B           4116 	mov	dptr,#_EP0BCL
   18C7 74 01              4117 	mov	a,#0x01
   18C9 F0                 4118 	movx	@dptr,a
                           4119 ;src/qs1r_main.c:623: break;
   18CA 02 1F 50           4120 	ljmp	00238$
                           4121 ;src/qs1r_main.c:625: case VRQ_DDC1_DR2_READ:
   18CD                    4122 00125$:
                           4123 ;src/qs1r_main.c:626: EP0BUF[0] = DDC1_DR2;
                           4124 ;     genAssign
   18CD 90 60 12           4125 	mov	dptr,#_DDC1_DR2
   18D0 E0                 4126 	movx	a,@dptr
                           4127 ;     genPointerSet
                           4128 ;     genFarPointerSet
                           4129 ;	Peephole 100	removed redundant mov
   18D1 FA                 4130 	mov	r2,a
   18D2 90 E7 40           4131 	mov	dptr,#_EP0BUF
   18D5 F0                 4132 	movx	@dptr,a
                           4133 ;src/qs1r_main.c:627: EP0BCH = 0;
                           4134 ;     genAssign
   18D6 90 E6 8A           4135 	mov	dptr,#_EP0BCH
                           4136 ;	Peephole 181	changed mov to clr
   18D9 E4                 4137 	clr	a
   18DA F0                 4138 	movx	@dptr,a
                           4139 ;src/qs1r_main.c:628: EP0BCL = 1;
                           4140 ;     genAssign
   18DB 90 E6 8B           4141 	mov	dptr,#_EP0BCL
   18DE 74 01              4142 	mov	a,#0x01
   18E0 F0                 4143 	movx	@dptr,a
                           4144 ;src/qs1r_main.c:629: break;
   18E1 02 1F 50           4145 	ljmp	00238$
                           4146 ;src/qs1r_main.c:631: case VRQ_DDC1_DR3_READ:
   18E4                    4147 00126$:
                           4148 ;src/qs1r_main.c:632: EP0BUF[0] = DDC1_DR3;
                           4149 ;     genAssign
   18E4 90 60 13           4150 	mov	dptr,#_DDC1_DR3
   18E7 E0                 4151 	movx	a,@dptr
                           4152 ;     genPointerSet
                           4153 ;     genFarPointerSet
                           4154 ;	Peephole 100	removed redundant mov
   18E8 FA                 4155 	mov	r2,a
   18E9 90 E7 40           4156 	mov	dptr,#_EP0BUF
   18EC F0                 4157 	movx	@dptr,a
                           4158 ;src/qs1r_main.c:633: EP0BCH = 0;
                           4159 ;     genAssign
   18ED 90 E6 8A           4160 	mov	dptr,#_EP0BCH
                           4161 ;	Peephole 181	changed mov to clr
   18F0 E4                 4162 	clr	a
   18F1 F0                 4163 	movx	@dptr,a
                           4164 ;src/qs1r_main.c:634: EP0BCL = 1;
                           4165 ;     genAssign
   18F2 90 E6 8B           4166 	mov	dptr,#_EP0BCL
   18F5 74 01              4167 	mov	a,#0x01
   18F7 F0                 4168 	movx	@dptr,a
                           4169 ;src/qs1r_main.c:635: break;
   18F8 02 1F 50           4170 	ljmp	00238$
                           4171 ;src/qs1r_main.c:637: case VRQ_DDC1_DR4_READ:
   18FB                    4172 00127$:
                           4173 ;src/qs1r_main.c:638: EP0BUF[0] = DDC1_DR4;
                           4174 ;     genAssign
   18FB 90 60 14           4175 	mov	dptr,#_DDC1_DR4
   18FE E0                 4176 	movx	a,@dptr
                           4177 ;     genPointerSet
                           4178 ;     genFarPointerSet
                           4179 ;	Peephole 100	removed redundant mov
   18FF FA                 4180 	mov	r2,a
   1900 90 E7 40           4181 	mov	dptr,#_EP0BUF
   1903 F0                 4182 	movx	@dptr,a
                           4183 ;src/qs1r_main.c:639: EP0BCH = 0;
                           4184 ;     genAssign
   1904 90 E6 8A           4185 	mov	dptr,#_EP0BCH
                           4186 ;	Peephole 181	changed mov to clr
   1907 E4                 4187 	clr	a
   1908 F0                 4188 	movx	@dptr,a
                           4189 ;src/qs1r_main.c:640: EP0BCL = 1;
                           4190 ;     genAssign
   1909 90 E6 8B           4191 	mov	dptr,#_EP0BCL
   190C 74 01              4192 	mov	a,#0x01
   190E F0                 4193 	movx	@dptr,a
                           4194 ;src/qs1r_main.c:641: break;
   190F 02 1F 50           4195 	ljmp	00238$
                           4196 ;src/qs1r_main.c:643: case VRQ_DDC1_LAR_READ:
   1912                    4197 00128$:
                           4198 ;src/qs1r_main.c:644: EP0BUF[0] = DDC1_LAR;
                           4199 ;     genAssign
   1912 90 60 16           4200 	mov	dptr,#_DDC1_LAR
   1915 E0                 4201 	movx	a,@dptr
                           4202 ;     genPointerSet
                           4203 ;     genFarPointerSet
                           4204 ;	Peephole 100	removed redundant mov
   1916 FA                 4205 	mov	r2,a
   1917 90 E7 40           4206 	mov	dptr,#_EP0BUF
   191A F0                 4207 	movx	@dptr,a
                           4208 ;src/qs1r_main.c:645: EP0BCH = 0;
                           4209 ;     genAssign
   191B 90 E6 8A           4210 	mov	dptr,#_EP0BCH
                           4211 ;	Peephole 181	changed mov to clr
   191E E4                 4212 	clr	a
   191F F0                 4213 	movx	@dptr,a
                           4214 ;src/qs1r_main.c:646: EP0BCL = 1;
                           4215 ;     genAssign
   1920 90 E6 8B           4216 	mov	dptr,#_EP0BCL
   1923 74 01              4217 	mov	a,#0x01
   1925 F0                 4218 	movx	@dptr,a
                           4219 ;src/qs1r_main.c:647: break;
   1926 02 1F 50           4220 	ljmp	00238$
                           4221 ;src/qs1r_main.c:649: case VRQ_DDC1_AMR_READ:
   1929                    4222 00129$:
                           4223 ;src/qs1r_main.c:650: EP0BUF[0] = DDC1_AMR;
                           4224 ;     genAssign
   1929 90 60 17           4225 	mov	dptr,#_DDC1_AMR
   192C E0                 4226 	movx	a,@dptr
                           4227 ;     genPointerSet
                           4228 ;     genFarPointerSet
                           4229 ;	Peephole 100	removed redundant mov
   192D FA                 4230 	mov	r2,a
   192E 90 E7 40           4231 	mov	dptr,#_EP0BUF
   1931 F0                 4232 	movx	@dptr,a
                           4233 ;src/qs1r_main.c:651: EP0BCH = 0;
                           4234 ;     genAssign
   1932 90 E6 8A           4235 	mov	dptr,#_EP0BCH
                           4236 ;	Peephole 181	changed mov to clr
   1935 E4                 4237 	clr	a
   1936 F0                 4238 	movx	@dptr,a
                           4239 ;src/qs1r_main.c:652: EP0BCL = 1;
                           4240 ;     genAssign
   1937 90 E6 8B           4241 	mov	dptr,#_EP0BCL
   193A 74 01              4242 	mov	a,#0x01
   193C F0                 4243 	movx	@dptr,a
                           4244 ;src/qs1r_main.c:653: break;
   193D 02 1F 50           4245 	ljmp	00238$
                           4246 ;src/qs1r_main.c:655: case VRQ_DDC_MCR_READ:
   1940                    4247 00130$:
                           4248 ;src/qs1r_main.c:656: if (wIndexL == 0) {
                           4249 ;     genPointerGet
                           4250 ;     genFarPointerGet
   1940 90 E6 BC           4251 	mov	dptr,#(_SETUPDAT + 0x0004)
   1943 E0                 4252 	movx	a,@dptr
                           4253 ;     genCmpEq
                           4254 ;	Peephole 115.b	jump optimization
   1944 FA                 4255 	mov	r2,a
   1945 60 02              4256 	jz	00296$
   1947                    4257 00295$:
                           4258 ;	Peephole 112.b	changed ljmp to sjmp
   1947 80 16              4259 	sjmp	00135$
   1949                    4260 00296$:
                           4261 ;src/qs1r_main.c:657: DDC0_AMR = (MCR >> 8);
                           4262 ;     genAssign
   1949 90 60 07           4263 	mov	dptr,#_DDC0_AMR
   194C 74 03              4264 	mov	a,#0x03
   194E F0                 4265 	movx	@dptr,a
                           4266 ;src/qs1r_main.c:658: DDC0_LAR = (MCR & 0xFF);
                           4267 ;     genAssign
   194F 90 60 06           4268 	mov	dptr,#_DDC0_LAR
                           4269 ;	Peephole 181	changed mov to clr
   1952 E4                 4270 	clr	a
   1953 F0                 4271 	movx	@dptr,a
                           4272 ;src/qs1r_main.c:659: EP0BUF[0] = DDC0_DR0;
                           4273 ;     genAssign
   1954 90 60 00           4274 	mov	dptr,#_DDC0_DR0
   1957 E0                 4275 	movx	a,@dptr
                           4276 ;     genPointerSet
                           4277 ;     genFarPointerSet
                           4278 ;	Peephole 100	removed redundant mov
   1958 FA                 4279 	mov	r2,a
   1959 90 E7 40           4280 	mov	dptr,#_EP0BUF
   195C F0                 4281 	movx	@dptr,a
                           4282 ;	Peephole 112.b	changed ljmp to sjmp
   195D 80 22              4283 	sjmp	00136$
   195F                    4284 00135$:
                           4285 ;src/qs1r_main.c:660: } else if (wIndexL == 1) {
                           4286 ;     genPointerGet
                           4287 ;     genFarPointerGet
   195F 90 E6 BC           4288 	mov	dptr,#(_SETUPDAT + 0x0004)
   1962 E0                 4289 	movx	a,@dptr
   1963 FA                 4290 	mov	r2,a
                           4291 ;     genCmpEq
                           4292 ;	Peephole 112.b	changed ljmp to sjmp
                           4293 ;	Peephole 199	optimized misc jump sequence
   1964 BA 01 16           4294 	cjne	r2,#0x01,00132$
                           4295 ;00297$:
                           4296 ;	Peephole 200	removed redundant sjmp
   1967                    4297 00298$:
                           4298 ;src/qs1r_main.c:661: DDC1_AMR = (MCR >> 8);
                           4299 ;     genAssign
   1967 90 60 17           4300 	mov	dptr,#_DDC1_AMR
   196A 74 03              4301 	mov	a,#0x03
   196C F0                 4302 	movx	@dptr,a
                           4303 ;src/qs1r_main.c:662: DDC1_LAR = (MCR & 0xFF);
                           4304 ;     genAssign
   196D 90 60 16           4305 	mov	dptr,#_DDC1_LAR
                           4306 ;	Peephole 181	changed mov to clr
   1970 E4                 4307 	clr	a
   1971 F0                 4308 	movx	@dptr,a
                           4309 ;src/qs1r_main.c:663: EP0BUF[0] = DDC1_DR0;
                           4310 ;     genAssign
   1972 90 60 10           4311 	mov	dptr,#_DDC1_DR0
   1975 E0                 4312 	movx	a,@dptr
                           4313 ;     genPointerSet
                           4314 ;     genFarPointerSet
                           4315 ;	Peephole 100	removed redundant mov
   1976 FA                 4316 	mov	r2,a
   1977 90 E7 40           4317 	mov	dptr,#_EP0BUF
   197A F0                 4318 	movx	@dptr,a
                           4319 ;	Peephole 112.b	changed ljmp to sjmp
   197B 80 04              4320 	sjmp	00136$
   197D                    4321 00132$:
                           4322 ;src/qs1r_main.c:664: } else return 0;
                           4323 ;     genRet
   197D 75 82 00           4324 	mov	dpl,#0x00
                           4325 ;	Peephole 251.a	replaced ljmp to ret with ret
   1980 22                 4326 	ret
   1981                    4327 00136$:
                           4328 ;src/qs1r_main.c:665: EP0BCH = 0;
                           4329 ;     genAssign
   1981 90 E6 8A           4330 	mov	dptr,#_EP0BCH
                           4331 ;	Peephole 181	changed mov to clr
   1984 E4                 4332 	clr	a
   1985 F0                 4333 	movx	@dptr,a
                           4334 ;src/qs1r_main.c:666: EP0BCL = 1;
                           4335 ;     genAssign
   1986 90 E6 8B           4336 	mov	dptr,#_EP0BCL
   1989 74 01              4337 	mov	a,#0x01
   198B F0                 4338 	movx	@dptr,a
                           4339 ;src/qs1r_main.c:667: break;
   198C 02 1F 50           4340 	ljmp	00238$
                           4341 ;src/qs1r_main.c:669: case VRQ_DDC_NCR_READ:
   198F                    4342 00137$:
                           4343 ;src/qs1r_main.c:670: if (wIndexL == 0) {
                           4344 ;     genPointerGet
                           4345 ;     genFarPointerGet
   198F 90 E6 BC           4346 	mov	dptr,#(_SETUPDAT + 0x0004)
   1992 E0                 4347 	movx	a,@dptr
                           4348 ;     genCmpEq
                           4349 ;	Peephole 115.b	jump optimization
   1993 FA                 4350 	mov	r2,a
   1994 60 02              4351 	jz	00300$
   1996                    4352 00299$:
                           4353 ;	Peephole 112.b	changed ljmp to sjmp
   1996 80 17              4354 	sjmp	00142$
   1998                    4355 00300$:
                           4356 ;src/qs1r_main.c:671: DDC0_AMR = (NCR >> 8);
                           4357 ;     genAssign
   1998 90 60 07           4358 	mov	dptr,#_DDC0_AMR
   199B 74 03              4359 	mov	a,#0x03
   199D F0                 4360 	movx	@dptr,a
                           4361 ;src/qs1r_main.c:672: DDC0_LAR = (NCR & 0xFF);
                           4362 ;     genAssign
   199E 90 60 06           4363 	mov	dptr,#_DDC0_LAR
   19A1 74 01              4364 	mov	a,#0x01
   19A3 F0                 4365 	movx	@dptr,a
                           4366 ;src/qs1r_main.c:673: EP0BUF[0] = DDC0_DR0;
                           4367 ;     genAssign
   19A4 90 60 00           4368 	mov	dptr,#_DDC0_DR0
   19A7 E0                 4369 	movx	a,@dptr
                           4370 ;     genPointerSet
                           4371 ;     genFarPointerSet
                           4372 ;	Peephole 100	removed redundant mov
   19A8 FA                 4373 	mov	r2,a
   19A9 90 E7 40           4374 	mov	dptr,#_EP0BUF
   19AC F0                 4375 	movx	@dptr,a
                           4376 ;	Peephole 112.b	changed ljmp to sjmp
   19AD 80 23              4377 	sjmp	00143$
   19AF                    4378 00142$:
                           4379 ;src/qs1r_main.c:674: } else if (wIndexL == 1) {
                           4380 ;     genPointerGet
                           4381 ;     genFarPointerGet
   19AF 90 E6 BC           4382 	mov	dptr,#(_SETUPDAT + 0x0004)
   19B2 E0                 4383 	movx	a,@dptr
   19B3 FA                 4384 	mov	r2,a
                           4385 ;     genCmpEq
                           4386 ;	Peephole 112.b	changed ljmp to sjmp
                           4387 ;	Peephole 199	optimized misc jump sequence
   19B4 BA 01 17           4388 	cjne	r2,#0x01,00139$
                           4389 ;00301$:
                           4390 ;	Peephole 200	removed redundant sjmp
   19B7                    4391 00302$:
                           4392 ;src/qs1r_main.c:675: DDC1_AMR = (NCR >> 8);
                           4393 ;     genAssign
   19B7 90 60 17           4394 	mov	dptr,#_DDC1_AMR
   19BA 74 03              4395 	mov	a,#0x03
   19BC F0                 4396 	movx	@dptr,a
                           4397 ;src/qs1r_main.c:676: DDC1_LAR = (NCR & 0xFF);
                           4398 ;     genAssign
   19BD 90 60 16           4399 	mov	dptr,#_DDC1_LAR
   19C0 74 01              4400 	mov	a,#0x01
   19C2 F0                 4401 	movx	@dptr,a
                           4402 ;src/qs1r_main.c:677: EP0BUF[0] = DDC1_DR0;
                           4403 ;     genAssign
   19C3 90 60 10           4404 	mov	dptr,#_DDC1_DR0
   19C6 E0                 4405 	movx	a,@dptr
                           4406 ;     genPointerSet
                           4407 ;     genFarPointerSet
                           4408 ;	Peephole 100	removed redundant mov
   19C7 FA                 4409 	mov	r2,a
   19C8 90 E7 40           4410 	mov	dptr,#_EP0BUF
   19CB F0                 4411 	movx	@dptr,a
                           4412 ;	Peephole 112.b	changed ljmp to sjmp
   19CC 80 04              4413 	sjmp	00143$
   19CE                    4414 00139$:
                           4415 ;src/qs1r_main.c:678: } else return 0;
                           4416 ;     genRet
   19CE 75 82 00           4417 	mov	dpl,#0x00
                           4418 ;	Peephole 251.a	replaced ljmp to ret with ret
   19D1 22                 4419 	ret
   19D2                    4420 00143$:
                           4421 ;src/qs1r_main.c:679: EP0BCH = 0;
                           4422 ;     genAssign
   19D2 90 E6 8A           4423 	mov	dptr,#_EP0BCH
                           4424 ;	Peephole 181	changed mov to clr
   19D5 E4                 4425 	clr	a
   19D6 F0                 4426 	movx	@dptr,a
                           4427 ;src/qs1r_main.c:680: EP0BCL = 1;
                           4428 ;     genAssign
   19D7 90 E6 8B           4429 	mov	dptr,#_EP0BCL
   19DA 74 01              4430 	mov	a,#0x01
   19DC F0                 4431 	movx	@dptr,a
                           4432 ;src/qs1r_main.c:681: break;
   19DD 02 1F 50           4433 	ljmp	00238$
                           4434 ;src/qs1r_main.c:683: case VRQ_DDC_NSCR_READ:
   19E0                    4435 00144$:
                           4436 ;src/qs1r_main.c:684: if (wIndexL == 0) {
                           4437 ;     genPointerGet
                           4438 ;     genFarPointerGet
   19E0 90 E6 BC           4439 	mov	dptr,#(_SETUPDAT + 0x0004)
   19E3 E0                 4440 	movx	a,@dptr
                           4441 ;     genCmpEq
                           4442 ;	Peephole 115.b	jump optimization
   19E4 FA                 4443 	mov	r2,a
   19E5 60 02              4444 	jz	00304$
   19E7                    4445 00303$:
                           4446 ;	Peephole 112.b	changed ljmp to sjmp
   19E7 80 32              4447 	sjmp	00149$
   19E9                    4448 00304$:
                           4449 ;src/qs1r_main.c:685: DDC0_AMR = (NSCR >> 8);
                           4450 ;     genAssign
   19E9 90 60 07           4451 	mov	dptr,#_DDC0_AMR
   19EC 74 03              4452 	mov	a,#0x03
   19EE F0                 4453 	movx	@dptr,a
                           4454 ;src/qs1r_main.c:686: DDC0_LAR = (NSCR & 0xFF);
                           4455 ;     genAssign
   19EF 90 60 06           4456 	mov	dptr,#_DDC0_LAR
   19F2 74 02              4457 	mov	a,#0x02
   19F4 F0                 4458 	movx	@dptr,a
                           4459 ;src/qs1r_main.c:687: EP0BUF[0] = DDC0_DR0;
                           4460 ;     genAssign
   19F5 90 60 00           4461 	mov	dptr,#_DDC0_DR0
   19F8 E0                 4462 	movx	a,@dptr
                           4463 ;     genPointerSet
                           4464 ;     genFarPointerSet
                           4465 ;	Peephole 100	removed redundant mov
   19F9 FA                 4466 	mov	r2,a
   19FA 90 E7 40           4467 	mov	dptr,#_EP0BUF
   19FD F0                 4468 	movx	@dptr,a
                           4469 ;src/qs1r_main.c:688: EP0BUF[1] = DDC0_DR1;
                           4470 ;     genAssign
   19FE 90 60 01           4471 	mov	dptr,#_DDC0_DR1
   1A01 E0                 4472 	movx	a,@dptr
                           4473 ;     genPointerSet
                           4474 ;     genFarPointerSet
                           4475 ;	Peephole 100	removed redundant mov
   1A02 FA                 4476 	mov	r2,a
   1A03 90 E7 41           4477 	mov	dptr,#(_EP0BUF + 0x0001)
   1A06 F0                 4478 	movx	@dptr,a
                           4479 ;src/qs1r_main.c:689: EP0BUF[2] = DDC0_DR2;
                           4480 ;     genAssign
   1A07 90 60 02           4481 	mov	dptr,#_DDC0_DR2
   1A0A E0                 4482 	movx	a,@dptr
                           4483 ;     genPointerSet
                           4484 ;     genFarPointerSet
                           4485 ;	Peephole 100	removed redundant mov
   1A0B FA                 4486 	mov	r2,a
   1A0C 90 E7 42           4487 	mov	dptr,#(_EP0BUF + 0x0002)
   1A0F F0                 4488 	movx	@dptr,a
                           4489 ;src/qs1r_main.c:690: EP0BUF[3] = DDC0_DR3;
                           4490 ;     genAssign
   1A10 90 60 03           4491 	mov	dptr,#_DDC0_DR3
   1A13 E0                 4492 	movx	a,@dptr
                           4493 ;     genPointerSet
                           4494 ;     genFarPointerSet
                           4495 ;	Peephole 100	removed redundant mov
   1A14 FA                 4496 	mov	r2,a
   1A15 90 E7 43           4497 	mov	dptr,#(_EP0BUF + 0x0003)
   1A18 F0                 4498 	movx	@dptr,a
                           4499 ;	Peephole 112.b	changed ljmp to sjmp
   1A19 80 3E              4500 	sjmp	00150$
   1A1B                    4501 00149$:
                           4502 ;src/qs1r_main.c:691: } else if (wIndexL == 1) {
                           4503 ;     genPointerGet
                           4504 ;     genFarPointerGet
   1A1B 90 E6 BC           4505 	mov	dptr,#(_SETUPDAT + 0x0004)
   1A1E E0                 4506 	movx	a,@dptr
   1A1F FA                 4507 	mov	r2,a
                           4508 ;     genCmpEq
                           4509 ;	Peephole 112.b	changed ljmp to sjmp
                           4510 ;	Peephole 199	optimized misc jump sequence
   1A20 BA 01 32           4511 	cjne	r2,#0x01,00146$
                           4512 ;00305$:
                           4513 ;	Peephole 200	removed redundant sjmp
   1A23                    4514 00306$:
                           4515 ;src/qs1r_main.c:692: DDC1_AMR = (NSCR >> 8);
                           4516 ;     genAssign
   1A23 90 60 17           4517 	mov	dptr,#_DDC1_AMR
   1A26 74 03              4518 	mov	a,#0x03
   1A28 F0                 4519 	movx	@dptr,a
                           4520 ;src/qs1r_main.c:693: DDC1_LAR = (NSCR & 0xFF);
                           4521 ;     genAssign
   1A29 90 60 16           4522 	mov	dptr,#_DDC1_LAR
   1A2C 74 02              4523 	mov	a,#0x02
   1A2E F0                 4524 	movx	@dptr,a
                           4525 ;src/qs1r_main.c:694: EP0BUF[0] = DDC1_DR0;
                           4526 ;     genAssign
   1A2F 90 60 10           4527 	mov	dptr,#_DDC1_DR0
   1A32 E0                 4528 	movx	a,@dptr
                           4529 ;     genPointerSet
                           4530 ;     genFarPointerSet
                           4531 ;	Peephole 100	removed redundant mov
   1A33 FA                 4532 	mov	r2,a
   1A34 90 E7 40           4533 	mov	dptr,#_EP0BUF
   1A37 F0                 4534 	movx	@dptr,a
                           4535 ;src/qs1r_main.c:695: EP0BUF[1] = DDC1_DR1;
                           4536 ;     genAssign
   1A38 90 60 11           4537 	mov	dptr,#_DDC1_DR1
   1A3B E0                 4538 	movx	a,@dptr
                           4539 ;     genPointerSet
                           4540 ;     genFarPointerSet
                           4541 ;	Peephole 100	removed redundant mov
   1A3C FA                 4542 	mov	r2,a
   1A3D 90 E7 41           4543 	mov	dptr,#(_EP0BUF + 0x0001)
   1A40 F0                 4544 	movx	@dptr,a
                           4545 ;src/qs1r_main.c:696: EP0BUF[2] = DDC1_DR2;
                           4546 ;     genAssign
   1A41 90 60 12           4547 	mov	dptr,#_DDC1_DR2
   1A44 E0                 4548 	movx	a,@dptr
                           4549 ;     genPointerSet
                           4550 ;     genFarPointerSet
                           4551 ;	Peephole 100	removed redundant mov
   1A45 FA                 4552 	mov	r2,a
   1A46 90 E7 42           4553 	mov	dptr,#(_EP0BUF + 0x0002)
   1A49 F0                 4554 	movx	@dptr,a
                           4555 ;src/qs1r_main.c:697: EP0BUF[3] = DDC1_DR3;
                           4556 ;     genAssign
   1A4A 90 60 13           4557 	mov	dptr,#_DDC1_DR3
   1A4D E0                 4558 	movx	a,@dptr
                           4559 ;     genPointerSet
                           4560 ;     genFarPointerSet
                           4561 ;	Peephole 100	removed redundant mov
   1A4E FA                 4562 	mov	r2,a
   1A4F 90 E7 43           4563 	mov	dptr,#(_EP0BUF + 0x0003)
   1A52 F0                 4564 	movx	@dptr,a
                           4565 ;	Peephole 112.b	changed ljmp to sjmp
   1A53 80 04              4566 	sjmp	00150$
   1A55                    4567 00146$:
                           4568 ;src/qs1r_main.c:698: } else return 0;
                           4569 ;     genRet
   1A55 75 82 00           4570 	mov	dpl,#0x00
                           4571 ;	Peephole 251.a	replaced ljmp to ret with ret
   1A58 22                 4572 	ret
   1A59                    4573 00150$:
                           4574 ;src/qs1r_main.c:699: EP0BCH = 0;
                           4575 ;     genAssign
   1A59 90 E6 8A           4576 	mov	dptr,#_EP0BCH
                           4577 ;	Peephole 181	changed mov to clr
   1A5C E4                 4578 	clr	a
   1A5D F0                 4579 	movx	@dptr,a
                           4580 ;src/qs1r_main.c:700: EP0BCL = 4;
                           4581 ;     genAssign
   1A5E 90 E6 8B           4582 	mov	dptr,#_EP0BCL
   1A61 74 04              4583 	mov	a,#0x04
   1A63 F0                 4584 	movx	@dptr,a
                           4585 ;src/qs1r_main.c:701: break;
   1A64 02 1F 50           4586 	ljmp	00238$
                           4587 ;src/qs1r_main.c:703: case VRQ_DDC_NFRE_READ:
   1A67                    4588 00151$:
                           4589 ;src/qs1r_main.c:704: if (wIndexL == 0) {
                           4590 ;     genPointerGet
                           4591 ;     genFarPointerGet
   1A67 90 E6 BC           4592 	mov	dptr,#(_SETUPDAT + 0x0004)
   1A6A E0                 4593 	movx	a,@dptr
                           4594 ;     genCmpEq
                           4595 ;	Peephole 115.b	jump optimization
   1A6B FA                 4596 	mov	r2,a
   1A6C 60 02              4597 	jz	00308$
   1A6E                    4598 00307$:
                           4599 ;	Peephole 112.b	changed ljmp to sjmp
   1A6E 80 32              4600 	sjmp	00156$
   1A70                    4601 00308$:
                           4602 ;src/qs1r_main.c:705: DDC0_AMR = (NFRE >> 8);
                           4603 ;     genAssign
   1A70 90 60 07           4604 	mov	dptr,#_DDC0_AMR
   1A73 74 03              4605 	mov	a,#0x03
   1A75 F0                 4606 	movx	@dptr,a
                           4607 ;src/qs1r_main.c:706: DDC0_LAR = (NFRE & 0xFF);
                           4608 ;     genAssign
   1A76 90 60 06           4609 	mov	dptr,#_DDC0_LAR
   1A79 74 03              4610 	mov	a,#0x03
   1A7B F0                 4611 	movx	@dptr,a
                           4612 ;src/qs1r_main.c:707: EP0BUF[0] = DDC0_DR0;
                           4613 ;     genAssign
   1A7C 90 60 00           4614 	mov	dptr,#_DDC0_DR0
   1A7F E0                 4615 	movx	a,@dptr
                           4616 ;     genPointerSet
                           4617 ;     genFarPointerSet
                           4618 ;	Peephole 100	removed redundant mov
   1A80 FA                 4619 	mov	r2,a
   1A81 90 E7 40           4620 	mov	dptr,#_EP0BUF
   1A84 F0                 4621 	movx	@dptr,a
                           4622 ;src/qs1r_main.c:708: EP0BUF[1] = DDC0_DR1;
                           4623 ;     genAssign
   1A85 90 60 01           4624 	mov	dptr,#_DDC0_DR1
   1A88 E0                 4625 	movx	a,@dptr
                           4626 ;     genPointerSet
                           4627 ;     genFarPointerSet
                           4628 ;	Peephole 100	removed redundant mov
   1A89 FA                 4629 	mov	r2,a
   1A8A 90 E7 41           4630 	mov	dptr,#(_EP0BUF + 0x0001)
   1A8D F0                 4631 	movx	@dptr,a
                           4632 ;src/qs1r_main.c:709: EP0BUF[2] = DDC0_DR2;
                           4633 ;     genAssign
   1A8E 90 60 02           4634 	mov	dptr,#_DDC0_DR2
   1A91 E0                 4635 	movx	a,@dptr
                           4636 ;     genPointerSet
                           4637 ;     genFarPointerSet
                           4638 ;	Peephole 100	removed redundant mov
   1A92 FA                 4639 	mov	r2,a
   1A93 90 E7 42           4640 	mov	dptr,#(_EP0BUF + 0x0002)
   1A96 F0                 4641 	movx	@dptr,a
                           4642 ;src/qs1r_main.c:710: EP0BUF[3] = DDC0_DR3;
                           4643 ;     genAssign
   1A97 90 60 03           4644 	mov	dptr,#_DDC0_DR3
   1A9A E0                 4645 	movx	a,@dptr
                           4646 ;     genPointerSet
                           4647 ;     genFarPointerSet
                           4648 ;	Peephole 100	removed redundant mov
   1A9B FA                 4649 	mov	r2,a
   1A9C 90 E7 43           4650 	mov	dptr,#(_EP0BUF + 0x0003)
   1A9F F0                 4651 	movx	@dptr,a
                           4652 ;	Peephole 112.b	changed ljmp to sjmp
   1AA0 80 3E              4653 	sjmp	00157$
   1AA2                    4654 00156$:
                           4655 ;src/qs1r_main.c:711: } else if (wIndexL == 1) {
                           4656 ;     genPointerGet
                           4657 ;     genFarPointerGet
   1AA2 90 E6 BC           4658 	mov	dptr,#(_SETUPDAT + 0x0004)
   1AA5 E0                 4659 	movx	a,@dptr
   1AA6 FA                 4660 	mov	r2,a
                           4661 ;     genCmpEq
                           4662 ;	Peephole 112.b	changed ljmp to sjmp
                           4663 ;	Peephole 199	optimized misc jump sequence
   1AA7 BA 01 32           4664 	cjne	r2,#0x01,00153$
                           4665 ;00309$:
                           4666 ;	Peephole 200	removed redundant sjmp
   1AAA                    4667 00310$:
                           4668 ;src/qs1r_main.c:712: DDC1_AMR = (NFRE >> 8);
                           4669 ;     genAssign
   1AAA 90 60 17           4670 	mov	dptr,#_DDC1_AMR
   1AAD 74 03              4671 	mov	a,#0x03
   1AAF F0                 4672 	movx	@dptr,a
                           4673 ;src/qs1r_main.c:713: DDC1_LAR = (NFRE & 0xFF);
                           4674 ;     genAssign
   1AB0 90 60 16           4675 	mov	dptr,#_DDC1_LAR
   1AB3 74 03              4676 	mov	a,#0x03
   1AB5 F0                 4677 	movx	@dptr,a
                           4678 ;src/qs1r_main.c:714: EP0BUF[0] = DDC1_DR0;
                           4679 ;     genAssign
   1AB6 90 60 10           4680 	mov	dptr,#_DDC1_DR0
   1AB9 E0                 4681 	movx	a,@dptr
                           4682 ;     genPointerSet
                           4683 ;     genFarPointerSet
                           4684 ;	Peephole 100	removed redundant mov
   1ABA FA                 4685 	mov	r2,a
   1ABB 90 E7 40           4686 	mov	dptr,#_EP0BUF
   1ABE F0                 4687 	movx	@dptr,a
                           4688 ;src/qs1r_main.c:715: EP0BUF[1] = DDC1_DR1;
                           4689 ;     genAssign
   1ABF 90 60 11           4690 	mov	dptr,#_DDC1_DR1
   1AC2 E0                 4691 	movx	a,@dptr
                           4692 ;     genPointerSet
                           4693 ;     genFarPointerSet
                           4694 ;	Peephole 100	removed redundant mov
   1AC3 FA                 4695 	mov	r2,a
   1AC4 90 E7 41           4696 	mov	dptr,#(_EP0BUF + 0x0001)
   1AC7 F0                 4697 	movx	@dptr,a
                           4698 ;src/qs1r_main.c:716: EP0BUF[2] = DDC1_DR2;
                           4699 ;     genAssign
   1AC8 90 60 12           4700 	mov	dptr,#_DDC1_DR2
   1ACB E0                 4701 	movx	a,@dptr
                           4702 ;     genPointerSet
                           4703 ;     genFarPointerSet
                           4704 ;	Peephole 100	removed redundant mov
   1ACC FA                 4705 	mov	r2,a
   1ACD 90 E7 42           4706 	mov	dptr,#(_EP0BUF + 0x0002)
   1AD0 F0                 4707 	movx	@dptr,a
                           4708 ;src/qs1r_main.c:717: EP0BUF[3] = DDC1_DR3;
                           4709 ;     genAssign
   1AD1 90 60 13           4710 	mov	dptr,#_DDC1_DR3
   1AD4 E0                 4711 	movx	a,@dptr
                           4712 ;     genPointerSet
                           4713 ;     genFarPointerSet
                           4714 ;	Peephole 100	removed redundant mov
   1AD5 FA                 4715 	mov	r2,a
   1AD6 90 E7 43           4716 	mov	dptr,#(_EP0BUF + 0x0003)
   1AD9 F0                 4717 	movx	@dptr,a
                           4718 ;	Peephole 112.b	changed ljmp to sjmp
   1ADA 80 04              4719 	sjmp	00157$
   1ADC                    4720 00153$:
                           4721 ;src/qs1r_main.c:718: } else return 0;
                           4722 ;     genRet
   1ADC 75 82 00           4723 	mov	dpl,#0x00
                           4724 ;	Peephole 251.a	replaced ljmp to ret with ret
   1ADF 22                 4725 	ret
   1AE0                    4726 00157$:
                           4727 ;src/qs1r_main.c:719: EP0BCH = 0;
                           4728 ;     genAssign
   1AE0 90 E6 8A           4729 	mov	dptr,#_EP0BCH
                           4730 ;	Peephole 181	changed mov to clr
   1AE3 E4                 4731 	clr	a
   1AE4 F0                 4732 	movx	@dptr,a
                           4733 ;src/qs1r_main.c:720: EP0BCL = 4;
                           4734 ;     genAssign
   1AE5 90 E6 8B           4735 	mov	dptr,#_EP0BCL
   1AE8 74 04              4736 	mov	a,#0x04
   1AEA F0                 4737 	movx	@dptr,a
                           4738 ;src/qs1r_main.c:721: break;
   1AEB 02 1F 50           4739 	ljmp	00238$
                           4740 ;src/qs1r_main.c:723: case VRQ_DDC_NPO_READ:
   1AEE                    4741 00158$:
                           4742 ;src/qs1r_main.c:724: if (wIndexL == 0) {
                           4743 ;     genPointerGet
                           4744 ;     genFarPointerGet
   1AEE 90 E6 BC           4745 	mov	dptr,#(_SETUPDAT + 0x0004)
   1AF1 E0                 4746 	movx	a,@dptr
                           4747 ;     genCmpEq
                           4748 ;	Peephole 115.b	jump optimization
   1AF2 FA                 4749 	mov	r2,a
   1AF3 60 02              4750 	jz	00312$
   1AF5                    4751 00311$:
                           4752 ;	Peephole 112.b	changed ljmp to sjmp
   1AF5 80 20              4753 	sjmp	00163$
   1AF7                    4754 00312$:
                           4755 ;src/qs1r_main.c:725: DDC0_AMR = (NPHA >> 8);
                           4756 ;     genAssign
   1AF7 90 60 07           4757 	mov	dptr,#_DDC0_AMR
   1AFA 74 03              4758 	mov	a,#0x03
   1AFC F0                 4759 	movx	@dptr,a
                           4760 ;src/qs1r_main.c:726: DDC0_LAR = (NPHA & 0xFF);
                           4761 ;     genAssign
   1AFD 90 60 06           4762 	mov	dptr,#_DDC0_LAR
   1B00 74 04              4763 	mov	a,#0x04
   1B02 F0                 4764 	movx	@dptr,a
                           4765 ;src/qs1r_main.c:727: EP0BUF[0] = DDC0_DR0;
                           4766 ;     genAssign
   1B03 90 60 00           4767 	mov	dptr,#_DDC0_DR0
   1B06 E0                 4768 	movx	a,@dptr
                           4769 ;     genPointerSet
                           4770 ;     genFarPointerSet
                           4771 ;	Peephole 100	removed redundant mov
   1B07 FA                 4772 	mov	r2,a
   1B08 90 E7 40           4773 	mov	dptr,#_EP0BUF
   1B0B F0                 4774 	movx	@dptr,a
                           4775 ;src/qs1r_main.c:728: EP0BUF[1] = DDC0_DR1;
                           4776 ;     genAssign
   1B0C 90 60 01           4777 	mov	dptr,#_DDC0_DR1
   1B0F E0                 4778 	movx	a,@dptr
                           4779 ;     genPointerSet
                           4780 ;     genFarPointerSet
                           4781 ;	Peephole 100	removed redundant mov
   1B10 FA                 4782 	mov	r2,a
   1B11 90 E7 41           4783 	mov	dptr,#(_EP0BUF + 0x0001)
   1B14 F0                 4784 	movx	@dptr,a
                           4785 ;	Peephole 112.b	changed ljmp to sjmp
   1B15 80 2C              4786 	sjmp	00164$
   1B17                    4787 00163$:
                           4788 ;src/qs1r_main.c:729: } else if (wIndexL == 1) {
                           4789 ;     genPointerGet
                           4790 ;     genFarPointerGet
   1B17 90 E6 BC           4791 	mov	dptr,#(_SETUPDAT + 0x0004)
   1B1A E0                 4792 	movx	a,@dptr
   1B1B FA                 4793 	mov	r2,a
                           4794 ;     genCmpEq
                           4795 ;	Peephole 112.b	changed ljmp to sjmp
                           4796 ;	Peephole 199	optimized misc jump sequence
   1B1C BA 01 20           4797 	cjne	r2,#0x01,00160$
                           4798 ;00313$:
                           4799 ;	Peephole 200	removed redundant sjmp
   1B1F                    4800 00314$:
                           4801 ;src/qs1r_main.c:730: DDC1_AMR = (NPHA >> 8);
                           4802 ;     genAssign
   1B1F 90 60 17           4803 	mov	dptr,#_DDC1_AMR
   1B22 74 03              4804 	mov	a,#0x03
   1B24 F0                 4805 	movx	@dptr,a
                           4806 ;src/qs1r_main.c:731: DDC1_LAR = (NPHA & 0xFF);
                           4807 ;     genAssign
   1B25 90 60 16           4808 	mov	dptr,#_DDC1_LAR
   1B28 74 04              4809 	mov	a,#0x04
   1B2A F0                 4810 	movx	@dptr,a
                           4811 ;src/qs1r_main.c:732: EP0BUF[0] = DDC1_DR0;
                           4812 ;     genAssign
   1B2B 90 60 10           4813 	mov	dptr,#_DDC1_DR0
   1B2E E0                 4814 	movx	a,@dptr
                           4815 ;     genPointerSet
                           4816 ;     genFarPointerSet
                           4817 ;	Peephole 100	removed redundant mov
   1B2F FA                 4818 	mov	r2,a
   1B30 90 E7 40           4819 	mov	dptr,#_EP0BUF
   1B33 F0                 4820 	movx	@dptr,a
                           4821 ;src/qs1r_main.c:733: EP0BUF[1] = DDC1_DR1;
                           4822 ;     genAssign
   1B34 90 60 11           4823 	mov	dptr,#_DDC1_DR1
   1B37 E0                 4824 	movx	a,@dptr
                           4825 ;     genPointerSet
                           4826 ;     genFarPointerSet
                           4827 ;	Peephole 100	removed redundant mov
   1B38 FA                 4828 	mov	r2,a
   1B39 90 E7 41           4829 	mov	dptr,#(_EP0BUF + 0x0001)
   1B3C F0                 4830 	movx	@dptr,a
                           4831 ;	Peephole 112.b	changed ljmp to sjmp
   1B3D 80 04              4832 	sjmp	00164$
   1B3F                    4833 00160$:
                           4834 ;src/qs1r_main.c:734: } else return 0;
                           4835 ;     genRet
   1B3F 75 82 00           4836 	mov	dpl,#0x00
                           4837 ;	Peephole 251.a	replaced ljmp to ret with ret
   1B42 22                 4838 	ret
   1B43                    4839 00164$:
                           4840 ;src/qs1r_main.c:735: EP0BCH = 0;
                           4841 ;     genAssign
   1B43 90 E6 8A           4842 	mov	dptr,#_EP0BCH
                           4843 ;	Peephole 181	changed mov to clr
   1B46 E4                 4844 	clr	a
   1B47 F0                 4845 	movx	@dptr,a
                           4846 ;src/qs1r_main.c:736: EP0BCL = 2;
                           4847 ;     genAssign
   1B48 90 E6 8B           4848 	mov	dptr,#_EP0BCL
   1B4B 74 02              4849 	mov	a,#0x02
   1B4D F0                 4850 	movx	@dptr,a
                           4851 ;src/qs1r_main.c:737: break;
   1B4E 02 1F 50           4852 	ljmp	00238$
                           4853 ;src/qs1r_main.c:739: case VRQ_DDC_C2SR_READ:
   1B51                    4854 00165$:
                           4855 ;src/qs1r_main.c:740: if (wIndexL == 0) {
                           4856 ;     genPointerGet
                           4857 ;     genFarPointerGet
   1B51 90 E6 BC           4858 	mov	dptr,#(_SETUPDAT + 0x0004)
   1B54 E0                 4859 	movx	a,@dptr
                           4860 ;     genCmpEq
                           4861 ;	Peephole 115.b	jump optimization
   1B55 FA                 4862 	mov	r2,a
   1B56 60 02              4863 	jz	00316$
   1B58                    4864 00315$:
                           4865 ;	Peephole 112.b	changed ljmp to sjmp
   1B58 80 17              4866 	sjmp	00170$
   1B5A                    4867 00316$:
                           4868 ;src/qs1r_main.c:741: DDC0_AMR = (C2SR >> 8);
                           4869 ;     genAssign
   1B5A 90 60 07           4870 	mov	dptr,#_DDC0_AMR
   1B5D 74 03              4871 	mov	a,#0x03
   1B5F F0                 4872 	movx	@dptr,a
                           4873 ;src/qs1r_main.c:742: DDC0_LAR = (C2SR & 0xFF);
                           4874 ;     genAssign
   1B60 90 60 06           4875 	mov	dptr,#_DDC0_LAR
   1B63 74 05              4876 	mov	a,#0x05
   1B65 F0                 4877 	movx	@dptr,a
                           4878 ;src/qs1r_main.c:743: EP0BUF[0] = DDC0_DR0;
                           4879 ;     genAssign
   1B66 90 60 00           4880 	mov	dptr,#_DDC0_DR0
   1B69 E0                 4881 	movx	a,@dptr
                           4882 ;     genPointerSet
                           4883 ;     genFarPointerSet
                           4884 ;	Peephole 100	removed redundant mov
   1B6A FA                 4885 	mov	r2,a
   1B6B 90 E7 40           4886 	mov	dptr,#_EP0BUF
   1B6E F0                 4887 	movx	@dptr,a
                           4888 ;	Peephole 112.b	changed ljmp to sjmp
   1B6F 80 23              4889 	sjmp	00171$
   1B71                    4890 00170$:
                           4891 ;src/qs1r_main.c:744: } else if (wIndexL == 1) {
                           4892 ;     genPointerGet
                           4893 ;     genFarPointerGet
   1B71 90 E6 BC           4894 	mov	dptr,#(_SETUPDAT + 0x0004)
   1B74 E0                 4895 	movx	a,@dptr
   1B75 FA                 4896 	mov	r2,a
                           4897 ;     genCmpEq
                           4898 ;	Peephole 112.b	changed ljmp to sjmp
                           4899 ;	Peephole 199	optimized misc jump sequence
   1B76 BA 01 17           4900 	cjne	r2,#0x01,00167$
                           4901 ;00317$:
                           4902 ;	Peephole 200	removed redundant sjmp
   1B79                    4903 00318$:
                           4904 ;src/qs1r_main.c:745: DDC1_AMR = (C2SR >> 8);
                           4905 ;     genAssign
   1B79 90 60 17           4906 	mov	dptr,#_DDC1_AMR
   1B7C 74 03              4907 	mov	a,#0x03
   1B7E F0                 4908 	movx	@dptr,a
                           4909 ;src/qs1r_main.c:746: DDC1_LAR = (C2SR & 0xFF);
                           4910 ;     genAssign
   1B7F 90 60 16           4911 	mov	dptr,#_DDC1_LAR
   1B82 74 05              4912 	mov	a,#0x05
   1B84 F0                 4913 	movx	@dptr,a
                           4914 ;src/qs1r_main.c:747: EP0BUF[0] = DDC1_DR0;
                           4915 ;     genAssign
   1B85 90 60 10           4916 	mov	dptr,#_DDC1_DR0
   1B88 E0                 4917 	movx	a,@dptr
                           4918 ;     genPointerSet
                           4919 ;     genFarPointerSet
                           4920 ;	Peephole 100	removed redundant mov
   1B89 FA                 4921 	mov	r2,a
   1B8A 90 E7 40           4922 	mov	dptr,#_EP0BUF
   1B8D F0                 4923 	movx	@dptr,a
                           4924 ;	Peephole 112.b	changed ljmp to sjmp
   1B8E 80 04              4925 	sjmp	00171$
   1B90                    4926 00167$:
                           4927 ;src/qs1r_main.c:748: } else return 0;
                           4928 ;     genRet
   1B90 75 82 00           4929 	mov	dpl,#0x00
                           4930 ;	Peephole 251.a	replaced ljmp to ret with ret
   1B93 22                 4931 	ret
   1B94                    4932 00171$:
                           4933 ;src/qs1r_main.c:749: EP0BCH = 0;
                           4934 ;     genAssign
   1B94 90 E6 8A           4935 	mov	dptr,#_EP0BCH
                           4936 ;	Peephole 181	changed mov to clr
   1B97 E4                 4937 	clr	a
   1B98 F0                 4938 	movx	@dptr,a
                           4939 ;src/qs1r_main.c:750: EP0BCL = 1;
                           4940 ;     genAssign
   1B99 90 E6 8B           4941 	mov	dptr,#_EP0BCL
   1B9C 74 01              4942 	mov	a,#0x01
   1B9E F0                 4943 	movx	@dptr,a
                           4944 ;src/qs1r_main.c:751: break;
   1B9F 02 1F 50           4945 	ljmp	00238$
                           4946 ;src/qs1r_main.c:753: case VRQ_DDC_C2DEC_READ:
   1BA2                    4947 00172$:
                           4948 ;src/qs1r_main.c:754: if (wIndexL == 0) {
                           4949 ;     genPointerGet
                           4950 ;     genFarPointerGet
   1BA2 90 E6 BC           4951 	mov	dptr,#(_SETUPDAT + 0x0004)
   1BA5 E0                 4952 	movx	a,@dptr
                           4953 ;     genCmpEq
                           4954 ;	Peephole 115.b	jump optimization
   1BA6 FA                 4955 	mov	r2,a
   1BA7 60 02              4956 	jz	00320$
   1BA9                    4957 00319$:
                           4958 ;	Peephole 112.b	changed ljmp to sjmp
   1BA9 80 1B              4959 	sjmp	00177$
   1BAB                    4960 00320$:
                           4961 ;src/qs1r_main.c:755: DDC0_AMR = ((C2DEC & 0xFF00) >> 8);
                           4962 ;     genAssign
   1BAB 90 60 07           4963 	mov	dptr,#_DDC0_AMR
   1BAE 74 03              4964 	mov	a,#0x03
   1BB0 F0                 4965 	movx	@dptr,a
                           4966 ;src/qs1r_main.c:756: DDC0_LAR = (C2DEC & 0xFF);
                           4967 ;     genAssign
   1BB1 90 60 06           4968 	mov	dptr,#_DDC0_LAR
   1BB4 74 06              4969 	mov	a,#0x06
   1BB6 F0                 4970 	movx	@dptr,a
                           4971 ;src/qs1r_main.c:757: foop = DDC0_DR1;
                           4972 ;     genDummyRead
   1BB7 90 60 01           4973 	mov	dptr,#_DDC0_DR1
   1BBA E0                 4974 	movx	a,@dptr
                           4975 ;src/qs1r_main.c:758: EP0BUF[0] = DDC0_DR0;
                           4976 ;     genAssign
   1BBB 90 60 00           4977 	mov	dptr,#_DDC0_DR0
   1BBE E0                 4978 	movx	a,@dptr
                           4979 ;     genPointerSet
                           4980 ;     genFarPointerSet
                           4981 ;	Peephole 100	removed redundant mov
   1BBF FA                 4982 	mov	r2,a
   1BC0 90 E7 40           4983 	mov	dptr,#_EP0BUF
   1BC3 F0                 4984 	movx	@dptr,a
                           4985 ;	Peephole 112.b	changed ljmp to sjmp
   1BC4 80 27              4986 	sjmp	00178$
   1BC6                    4987 00177$:
                           4988 ;src/qs1r_main.c:759: } else if (wIndexL == 1) {
                           4989 ;     genPointerGet
                           4990 ;     genFarPointerGet
   1BC6 90 E6 BC           4991 	mov	dptr,#(_SETUPDAT + 0x0004)
   1BC9 E0                 4992 	movx	a,@dptr
   1BCA FA                 4993 	mov	r2,a
                           4994 ;     genCmpEq
                           4995 ;	Peephole 112.b	changed ljmp to sjmp
                           4996 ;	Peephole 199	optimized misc jump sequence
   1BCB BA 01 1B           4997 	cjne	r2,#0x01,00174$
                           4998 ;00321$:
                           4999 ;	Peephole 200	removed redundant sjmp
   1BCE                    5000 00322$:
                           5001 ;src/qs1r_main.c:760: DDC1_AMR = ((C2DEC & 0xFF00) >> 8);
                           5002 ;     genAssign
   1BCE 90 60 17           5003 	mov	dptr,#_DDC1_AMR
   1BD1 74 03              5004 	mov	a,#0x03
   1BD3 F0                 5005 	movx	@dptr,a
                           5006 ;src/qs1r_main.c:761: DDC1_LAR = (C2DEC & 0xFF);
                           5007 ;     genAssign
   1BD4 90 60 16           5008 	mov	dptr,#_DDC1_LAR
   1BD7 74 06              5009 	mov	a,#0x06
   1BD9 F0                 5010 	movx	@dptr,a
                           5011 ;src/qs1r_main.c:762: foop = DDC1_DR1;
                           5012 ;     genDummyRead
   1BDA 90 60 11           5013 	mov	dptr,#_DDC1_DR1
   1BDD E0                 5014 	movx	a,@dptr
                           5015 ;src/qs1r_main.c:763: EP0BUF[0] = DDC1_DR0;
                           5016 ;     genAssign
   1BDE 90 60 10           5017 	mov	dptr,#_DDC1_DR0
   1BE1 E0                 5018 	movx	a,@dptr
                           5019 ;     genPointerSet
                           5020 ;     genFarPointerSet
                           5021 ;	Peephole 100	removed redundant mov
   1BE2 FA                 5022 	mov	r2,a
   1BE3 90 E7 40           5023 	mov	dptr,#_EP0BUF
   1BE6 F0                 5024 	movx	@dptr,a
                           5025 ;	Peephole 112.b	changed ljmp to sjmp
   1BE7 80 04              5026 	sjmp	00178$
   1BE9                    5027 00174$:
                           5028 ;src/qs1r_main.c:764: } else return 0;
                           5029 ;     genRet
   1BE9 75 82 00           5030 	mov	dpl,#0x00
                           5031 ;	Peephole 251.a	replaced ljmp to ret with ret
   1BEC 22                 5032 	ret
   1BED                    5033 00178$:
                           5034 ;src/qs1r_main.c:765: EP0BCH = 0;
                           5035 ;     genAssign
   1BED 90 E6 8A           5036 	mov	dptr,#_EP0BCH
                           5037 ;	Peephole 181	changed mov to clr
   1BF0 E4                 5038 	clr	a
   1BF1 F0                 5039 	movx	@dptr,a
                           5040 ;src/qs1r_main.c:766: EP0BCL = 1;
                           5041 ;     genAssign
   1BF2 90 E6 8B           5042 	mov	dptr,#_EP0BCL
   1BF5 74 01              5043 	mov	a,#0x01
   1BF7 F0                 5044 	movx	@dptr,a
                           5045 ;src/qs1r_main.c:767: break;
   1BF8 02 1F 50           5046 	ljmp	00238$
                           5047 ;src/qs1r_main.c:769: case VRQ_DDC_C5SR_READ:
   1BFB                    5048 00179$:
                           5049 ;src/qs1r_main.c:770: if (wIndexL == 0) {
                           5050 ;     genPointerGet
                           5051 ;     genFarPointerGet
   1BFB 90 E6 BC           5052 	mov	dptr,#(_SETUPDAT + 0x0004)
   1BFE E0                 5053 	movx	a,@dptr
                           5054 ;     genCmpEq
                           5055 ;	Peephole 115.b	jump optimization
   1BFF FA                 5056 	mov	r2,a
   1C00 60 02              5057 	jz	00324$
   1C02                    5058 00323$:
                           5059 ;	Peephole 112.b	changed ljmp to sjmp
   1C02 80 17              5060 	sjmp	00184$
   1C04                    5061 00324$:
                           5062 ;src/qs1r_main.c:771: DDC0_AMR = (C5SR >> 8);
                           5063 ;     genAssign
   1C04 90 60 07           5064 	mov	dptr,#_DDC0_AMR
   1C07 74 03              5065 	mov	a,#0x03
   1C09 F0                 5066 	movx	@dptr,a
                           5067 ;src/qs1r_main.c:772: DDC0_LAR = (C5SR & 0xFF);
                           5068 ;     genAssign
   1C0A 90 60 06           5069 	mov	dptr,#_DDC0_LAR
   1C0D 74 07              5070 	mov	a,#0x07
   1C0F F0                 5071 	movx	@dptr,a
                           5072 ;src/qs1r_main.c:773: EP0BUF[0] = DDC0_DR0;
                           5073 ;     genAssign
   1C10 90 60 00           5074 	mov	dptr,#_DDC0_DR0
   1C13 E0                 5075 	movx	a,@dptr
                           5076 ;     genPointerSet
                           5077 ;     genFarPointerSet
                           5078 ;	Peephole 100	removed redundant mov
   1C14 FA                 5079 	mov	r2,a
   1C15 90 E7 40           5080 	mov	dptr,#_EP0BUF
   1C18 F0                 5081 	movx	@dptr,a
                           5082 ;	Peephole 112.b	changed ljmp to sjmp
   1C19 80 23              5083 	sjmp	00185$
   1C1B                    5084 00184$:
                           5085 ;src/qs1r_main.c:774: } else if (wIndexL == 1) {
                           5086 ;     genPointerGet
                           5087 ;     genFarPointerGet
   1C1B 90 E6 BC           5088 	mov	dptr,#(_SETUPDAT + 0x0004)
   1C1E E0                 5089 	movx	a,@dptr
   1C1F FA                 5090 	mov	r2,a
                           5091 ;     genCmpEq
                           5092 ;	Peephole 112.b	changed ljmp to sjmp
                           5093 ;	Peephole 199	optimized misc jump sequence
   1C20 BA 01 17           5094 	cjne	r2,#0x01,00181$
                           5095 ;00325$:
                           5096 ;	Peephole 200	removed redundant sjmp
   1C23                    5097 00326$:
                           5098 ;src/qs1r_main.c:775: DDC1_AMR = (C5SR >> 8);
                           5099 ;     genAssign
   1C23 90 60 17           5100 	mov	dptr,#_DDC1_AMR
   1C26 74 03              5101 	mov	a,#0x03
   1C28 F0                 5102 	movx	@dptr,a
                           5103 ;src/qs1r_main.c:776: DDC1_LAR = (C5SR & 0xFF);
                           5104 ;     genAssign
   1C29 90 60 16           5105 	mov	dptr,#_DDC1_LAR
   1C2C 74 07              5106 	mov	a,#0x07
   1C2E F0                 5107 	movx	@dptr,a
                           5108 ;src/qs1r_main.c:777: EP0BUF[0] = DDC1_DR0;
                           5109 ;     genAssign
   1C2F 90 60 10           5110 	mov	dptr,#_DDC1_DR0
   1C32 E0                 5111 	movx	a,@dptr
                           5112 ;     genPointerSet
                           5113 ;     genFarPointerSet
                           5114 ;	Peephole 100	removed redundant mov
   1C33 FA                 5115 	mov	r2,a
   1C34 90 E7 40           5116 	mov	dptr,#_EP0BUF
   1C37 F0                 5117 	movx	@dptr,a
                           5118 ;	Peephole 112.b	changed ljmp to sjmp
   1C38 80 04              5119 	sjmp	00185$
   1C3A                    5120 00181$:
                           5121 ;src/qs1r_main.c:778: } else return 0;
                           5122 ;     genRet
   1C3A 75 82 00           5123 	mov	dpl,#0x00
                           5124 ;	Peephole 251.a	replaced ljmp to ret with ret
   1C3D 22                 5125 	ret
   1C3E                    5126 00185$:
                           5127 ;src/qs1r_main.c:779: EP0BCH = 0;
                           5128 ;     genAssign
   1C3E 90 E6 8A           5129 	mov	dptr,#_EP0BCH
                           5130 ;	Peephole 181	changed mov to clr
   1C41 E4                 5131 	clr	a
   1C42 F0                 5132 	movx	@dptr,a
                           5133 ;src/qs1r_main.c:780: EP0BCL = 1;
                           5134 ;     genAssign
   1C43 90 E6 8B           5135 	mov	dptr,#_EP0BCL
   1C46 74 01              5136 	mov	a,#0x01
   1C48 F0                 5137 	movx	@dptr,a
                           5138 ;src/qs1r_main.c:781: break;
   1C49 02 1F 50           5139 	ljmp	00238$
                           5140 ;src/qs1r_main.c:783: case VRQ_DDC_C5DEC_READ:
   1C4C                    5141 00186$:
                           5142 ;src/qs1r_main.c:784: if (wIndexL == 0) {
                           5143 ;     genPointerGet
                           5144 ;     genFarPointerGet
   1C4C 90 E6 BC           5145 	mov	dptr,#(_SETUPDAT + 0x0004)
   1C4F E0                 5146 	movx	a,@dptr
                           5147 ;     genCmpEq
                           5148 ;	Peephole 115.b	jump optimization
   1C50 FA                 5149 	mov	r2,a
   1C51 60 02              5150 	jz	00328$
   1C53                    5151 00327$:
                           5152 ;	Peephole 112.b	changed ljmp to sjmp
   1C53 80 17              5153 	sjmp	00191$
   1C55                    5154 00328$:
                           5155 ;src/qs1r_main.c:785: DDC0_AMR = (C5DEC >> 8);
                           5156 ;     genAssign
   1C55 90 60 07           5157 	mov	dptr,#_DDC0_AMR
   1C58 74 03              5158 	mov	a,#0x03
   1C5A F0                 5159 	movx	@dptr,a
                           5160 ;src/qs1r_main.c:786: DDC0_LAR = (C5DEC & 0xFF);
                           5161 ;     genAssign
   1C5B 90 60 06           5162 	mov	dptr,#_DDC0_LAR
   1C5E 74 08              5163 	mov	a,#0x08
   1C60 F0                 5164 	movx	@dptr,a
                           5165 ;src/qs1r_main.c:787: EP0BUF[0] = DDC0_DR0;
                           5166 ;     genAssign
   1C61 90 60 00           5167 	mov	dptr,#_DDC0_DR0
   1C64 E0                 5168 	movx	a,@dptr
                           5169 ;     genPointerSet
                           5170 ;     genFarPointerSet
                           5171 ;	Peephole 100	removed redundant mov
   1C65 FA                 5172 	mov	r2,a
   1C66 90 E7 40           5173 	mov	dptr,#_EP0BUF
   1C69 F0                 5174 	movx	@dptr,a
                           5175 ;	Peephole 112.b	changed ljmp to sjmp
   1C6A 80 23              5176 	sjmp	00192$
   1C6C                    5177 00191$:
                           5178 ;src/qs1r_main.c:788: } else if (wIndexL == 1) {
                           5179 ;     genPointerGet
                           5180 ;     genFarPointerGet
   1C6C 90 E6 BC           5181 	mov	dptr,#(_SETUPDAT + 0x0004)
   1C6F E0                 5182 	movx	a,@dptr
   1C70 FA                 5183 	mov	r2,a
                           5184 ;     genCmpEq
                           5185 ;	Peephole 112.b	changed ljmp to sjmp
                           5186 ;	Peephole 199	optimized misc jump sequence
   1C71 BA 01 17           5187 	cjne	r2,#0x01,00188$
                           5188 ;00329$:
                           5189 ;	Peephole 200	removed redundant sjmp
   1C74                    5190 00330$:
                           5191 ;src/qs1r_main.c:789: DDC1_AMR = (C5DEC >> 8);
                           5192 ;     genAssign
   1C74 90 60 17           5193 	mov	dptr,#_DDC1_AMR
   1C77 74 03              5194 	mov	a,#0x03
   1C79 F0                 5195 	movx	@dptr,a
                           5196 ;src/qs1r_main.c:790: DDC1_LAR = (C5DEC & 0xFF);
                           5197 ;     genAssign
   1C7A 90 60 16           5198 	mov	dptr,#_DDC1_LAR
   1C7D 74 08              5199 	mov	a,#0x08
   1C7F F0                 5200 	movx	@dptr,a
                           5201 ;src/qs1r_main.c:791: EP0BUF[0] = DDC1_DR0;
                           5202 ;     genAssign
   1C80 90 60 10           5203 	mov	dptr,#_DDC1_DR0
   1C83 E0                 5204 	movx	a,@dptr
                           5205 ;     genPointerSet
                           5206 ;     genFarPointerSet
                           5207 ;	Peephole 100	removed redundant mov
   1C84 FA                 5208 	mov	r2,a
   1C85 90 E7 40           5209 	mov	dptr,#_EP0BUF
   1C88 F0                 5210 	movx	@dptr,a
                           5211 ;	Peephole 112.b	changed ljmp to sjmp
   1C89 80 04              5212 	sjmp	00192$
   1C8B                    5213 00188$:
                           5214 ;src/qs1r_main.c:792: } else return 0;
                           5215 ;     genRet
   1C8B 75 82 00           5216 	mov	dpl,#0x00
                           5217 ;	Peephole 251.a	replaced ljmp to ret with ret
   1C8E 22                 5218 	ret
   1C8F                    5219 00192$:
                           5220 ;src/qs1r_main.c:793: EP0BCH = 0;
                           5221 ;     genAssign
   1C8F 90 E6 8A           5222 	mov	dptr,#_EP0BCH
                           5223 ;	Peephole 181	changed mov to clr
   1C92 E4                 5224 	clr	a
   1C93 F0                 5225 	movx	@dptr,a
                           5226 ;src/qs1r_main.c:794: EP0BCL = 1;
                           5227 ;     genAssign
   1C94 90 E6 8B           5228 	mov	dptr,#_EP0BCL
   1C97 74 01              5229 	mov	a,#0x01
   1C99 F0                 5230 	movx	@dptr,a
                           5231 ;src/qs1r_main.c:795: break;
   1C9A 02 1F 50           5232 	ljmp	00238$
                           5233 ;src/qs1r_main.c:797: case VRQ_DDC_RCR_READ:
   1C9D                    5234 00193$:
                           5235 ;src/qs1r_main.c:798: if (wIndexL == 0) {
                           5236 ;     genPointerGet
                           5237 ;     genFarPointerGet
   1C9D 90 E6 BC           5238 	mov	dptr,#(_SETUPDAT + 0x0004)
   1CA0 E0                 5239 	movx	a,@dptr
                           5240 ;     genCmpEq
                           5241 ;	Peephole 115.b	jump optimization
   1CA1 FA                 5242 	mov	r2,a
   1CA2 60 02              5243 	jz	00332$
   1CA4                    5244 00331$:
                           5245 ;	Peephole 112.b	changed ljmp to sjmp
   1CA4 80 17              5246 	sjmp	00198$
   1CA6                    5247 00332$:
                           5248 ;src/qs1r_main.c:799: DDC0_AMR = (RCR >> 8);
                           5249 ;     genAssign
   1CA6 90 60 07           5250 	mov	dptr,#_DDC0_AMR
   1CA9 74 03              5251 	mov	a,#0x03
   1CAB F0                 5252 	movx	@dptr,a
                           5253 ;src/qs1r_main.c:800: DDC0_LAR = (RCR & 0xFF);
                           5254 ;     genAssign
   1CAC 90 60 06           5255 	mov	dptr,#_DDC0_LAR
   1CAF 74 09              5256 	mov	a,#0x09
   1CB1 F0                 5257 	movx	@dptr,a
                           5258 ;src/qs1r_main.c:801: EP0BUF[0] = DDC0_DR0;
                           5259 ;     genAssign
   1CB2 90 60 00           5260 	mov	dptr,#_DDC0_DR0
   1CB5 E0                 5261 	movx	a,@dptr
                           5262 ;     genPointerSet
                           5263 ;     genFarPointerSet
                           5264 ;	Peephole 100	removed redundant mov
   1CB6 FA                 5265 	mov	r2,a
   1CB7 90 E7 40           5266 	mov	dptr,#_EP0BUF
   1CBA F0                 5267 	movx	@dptr,a
                           5268 ;	Peephole 112.b	changed ljmp to sjmp
   1CBB 80 23              5269 	sjmp	00199$
   1CBD                    5270 00198$:
                           5271 ;src/qs1r_main.c:802: } else if (wIndexL == 1) {
                           5272 ;     genPointerGet
                           5273 ;     genFarPointerGet
   1CBD 90 E6 BC           5274 	mov	dptr,#(_SETUPDAT + 0x0004)
   1CC0 E0                 5275 	movx	a,@dptr
   1CC1 FA                 5276 	mov	r2,a
                           5277 ;     genCmpEq
                           5278 ;	Peephole 112.b	changed ljmp to sjmp
                           5279 ;	Peephole 199	optimized misc jump sequence
   1CC2 BA 01 17           5280 	cjne	r2,#0x01,00195$
                           5281 ;00333$:
                           5282 ;	Peephole 200	removed redundant sjmp
   1CC5                    5283 00334$:
                           5284 ;src/qs1r_main.c:803: DDC1_AMR = (RCR >> 8);
                           5285 ;     genAssign
   1CC5 90 60 17           5286 	mov	dptr,#_DDC1_AMR
   1CC8 74 03              5287 	mov	a,#0x03
   1CCA F0                 5288 	movx	@dptr,a
                           5289 ;src/qs1r_main.c:804: DDC1_LAR = (RCR & 0xFF);
                           5290 ;     genAssign
   1CCB 90 60 16           5291 	mov	dptr,#_DDC1_LAR
   1CCE 74 09              5292 	mov	a,#0x09
   1CD0 F0                 5293 	movx	@dptr,a
                           5294 ;src/qs1r_main.c:805: EP0BUF[0] = DDC1_DR0;
                           5295 ;     genAssign
   1CD1 90 60 10           5296 	mov	dptr,#_DDC1_DR0
   1CD4 E0                 5297 	movx	a,@dptr
                           5298 ;     genPointerSet
                           5299 ;     genFarPointerSet
                           5300 ;	Peephole 100	removed redundant mov
   1CD5 FA                 5301 	mov	r2,a
   1CD6 90 E7 40           5302 	mov	dptr,#_EP0BUF
   1CD9 F0                 5303 	movx	@dptr,a
                           5304 ;	Peephole 112.b	changed ljmp to sjmp
   1CDA 80 04              5305 	sjmp	00199$
   1CDC                    5306 00195$:
                           5307 ;src/qs1r_main.c:806: } else return 0;
                           5308 ;     genRet
   1CDC 75 82 00           5309 	mov	dpl,#0x00
                           5310 ;	Peephole 251.a	replaced ljmp to ret with ret
   1CDF 22                 5311 	ret
   1CE0                    5312 00199$:
                           5313 ;src/qs1r_main.c:807: EP0BCH = 0;
                           5314 ;     genAssign
   1CE0 90 E6 8A           5315 	mov	dptr,#_EP0BCH
                           5316 ;	Peephole 181	changed mov to clr
   1CE3 E4                 5317 	clr	a
   1CE4 F0                 5318 	movx	@dptr,a
                           5319 ;src/qs1r_main.c:808: EP0BCL = 1;
                           5320 ;     genAssign
   1CE5 90 E6 8B           5321 	mov	dptr,#_EP0BCL
   1CE8 74 01              5322 	mov	a,#0x01
   1CEA F0                 5323 	movx	@dptr,a
                           5324 ;src/qs1r_main.c:809: break;
   1CEB 02 1F 50           5325 	ljmp	00238$
                           5326 ;src/qs1r_main.c:811: case VRQ_DDC_RDEC_READ:
   1CEE                    5327 00200$:
                           5328 ;src/qs1r_main.c:812: if (wIndexL == 0) {
                           5329 ;     genPointerGet
                           5330 ;     genFarPointerGet
   1CEE 90 E6 BC           5331 	mov	dptr,#(_SETUPDAT + 0x0004)
   1CF1 E0                 5332 	movx	a,@dptr
                           5333 ;     genCmpEq
                           5334 ;	Peephole 115.b	jump optimization
   1CF2 FA                 5335 	mov	r2,a
   1CF3 60 02              5336 	jz	00336$
   1CF5                    5337 00335$:
                           5338 ;	Peephole 112.b	changed ljmp to sjmp
   1CF5 80 17              5339 	sjmp	00205$
   1CF7                    5340 00336$:
                           5341 ;src/qs1r_main.c:813: DDC0_AMR = (RDEC >> 8);
                           5342 ;     genAssign
   1CF7 90 60 07           5343 	mov	dptr,#_DDC0_AMR
   1CFA 74 03              5344 	mov	a,#0x03
   1CFC F0                 5345 	movx	@dptr,a
                           5346 ;src/qs1r_main.c:814: DDC0_LAR = (RDEC & 0xFF);
                           5347 ;     genAssign
   1CFD 90 60 06           5348 	mov	dptr,#_DDC0_LAR
   1D00 74 0A              5349 	mov	a,#0x0A
   1D02 F0                 5350 	movx	@dptr,a
                           5351 ;src/qs1r_main.c:815: EP0BUF[0] = DDC0_DR0;
                           5352 ;     genAssign
   1D03 90 60 00           5353 	mov	dptr,#_DDC0_DR0
   1D06 E0                 5354 	movx	a,@dptr
                           5355 ;     genPointerSet
                           5356 ;     genFarPointerSet
                           5357 ;	Peephole 100	removed redundant mov
   1D07 FA                 5358 	mov	r2,a
   1D08 90 E7 40           5359 	mov	dptr,#_EP0BUF
   1D0B F0                 5360 	movx	@dptr,a
                           5361 ;	Peephole 112.b	changed ljmp to sjmp
   1D0C 80 23              5362 	sjmp	00206$
   1D0E                    5363 00205$:
                           5364 ;src/qs1r_main.c:816: } else if (wIndexL == 1) {
                           5365 ;     genPointerGet
                           5366 ;     genFarPointerGet
   1D0E 90 E6 BC           5367 	mov	dptr,#(_SETUPDAT + 0x0004)
   1D11 E0                 5368 	movx	a,@dptr
   1D12 FA                 5369 	mov	r2,a
                           5370 ;     genCmpEq
                           5371 ;	Peephole 112.b	changed ljmp to sjmp
                           5372 ;	Peephole 199	optimized misc jump sequence
   1D13 BA 01 17           5373 	cjne	r2,#0x01,00202$
                           5374 ;00337$:
                           5375 ;	Peephole 200	removed redundant sjmp
   1D16                    5376 00338$:
                           5377 ;src/qs1r_main.c:817: DDC1_AMR = (RDEC >> 8);
                           5378 ;     genAssign
   1D16 90 60 17           5379 	mov	dptr,#_DDC1_AMR
   1D19 74 03              5380 	mov	a,#0x03
   1D1B F0                 5381 	movx	@dptr,a
                           5382 ;src/qs1r_main.c:818: DDC1_LAR = (RDEC & 0xFF);
                           5383 ;     genAssign
   1D1C 90 60 16           5384 	mov	dptr,#_DDC1_LAR
   1D1F 74 0A              5385 	mov	a,#0x0A
   1D21 F0                 5386 	movx	@dptr,a
                           5387 ;src/qs1r_main.c:819: EP0BUF[0] = DDC1_DR0;
                           5388 ;     genAssign
   1D22 90 60 10           5389 	mov	dptr,#_DDC1_DR0
   1D25 E0                 5390 	movx	a,@dptr
                           5391 ;     genPointerSet
                           5392 ;     genFarPointerSet
                           5393 ;	Peephole 100	removed redundant mov
   1D26 FA                 5394 	mov	r2,a
   1D27 90 E7 40           5395 	mov	dptr,#_EP0BUF
   1D2A F0                 5396 	movx	@dptr,a
                           5397 ;	Peephole 112.b	changed ljmp to sjmp
   1D2B 80 04              5398 	sjmp	00206$
   1D2D                    5399 00202$:
                           5400 ;src/qs1r_main.c:820: } else return 0;
                           5401 ;     genRet
   1D2D 75 82 00           5402 	mov	dpl,#0x00
                           5403 ;	Peephole 251.a	replaced ljmp to ret with ret
   1D30 22                 5404 	ret
   1D31                    5405 00206$:
                           5406 ;src/qs1r_main.c:821: EP0BCH = 0;
                           5407 ;     genAssign
   1D31 90 E6 8A           5408 	mov	dptr,#_EP0BCH
                           5409 ;	Peephole 181	changed mov to clr
   1D34 E4                 5410 	clr	a
   1D35 F0                 5411 	movx	@dptr,a
                           5412 ;src/qs1r_main.c:822: EP0BCL = 1;
                           5413 ;     genAssign
   1D36 90 E6 8B           5414 	mov	dptr,#_EP0BCL
   1D39 74 01              5415 	mov	a,#0x01
   1D3B F0                 5416 	movx	@dptr,a
                           5417 ;src/qs1r_main.c:823: break;
   1D3C 02 1F 50           5418 	ljmp	00238$
                           5419 ;src/qs1r_main.c:825: case VRQ_DDC_RAO_READ:
   1D3F                    5420 00207$:
                           5421 ;src/qs1r_main.c:826: if (wIndexL == 0) {
                           5422 ;     genPointerGet
                           5423 ;     genFarPointerGet
   1D3F 90 E6 BC           5424 	mov	dptr,#(_SETUPDAT + 0x0004)
   1D42 E0                 5425 	movx	a,@dptr
                           5426 ;     genCmpEq
                           5427 ;	Peephole 115.b	jump optimization
   1D43 FA                 5428 	mov	r2,a
   1D44 60 02              5429 	jz	00340$
   1D46                    5430 00339$:
                           5431 ;	Peephole 112.b	changed ljmp to sjmp
   1D46 80 17              5432 	sjmp	00212$
   1D48                    5433 00340$:
                           5434 ;src/qs1r_main.c:827: DDC0_AMR = (RAO >> 8);
                           5435 ;     genAssign
   1D48 90 60 07           5436 	mov	dptr,#_DDC0_AMR
   1D4B 74 03              5437 	mov	a,#0x03
   1D4D F0                 5438 	movx	@dptr,a
                           5439 ;src/qs1r_main.c:828: DDC0_LAR = (RAO & 0xFF);
                           5440 ;     genAssign
   1D4E 90 60 06           5441 	mov	dptr,#_DDC0_LAR
   1D51 74 0B              5442 	mov	a,#0x0B
   1D53 F0                 5443 	movx	@dptr,a
                           5444 ;src/qs1r_main.c:829: EP0BUF[0] = DDC0_DR0;
                           5445 ;     genAssign
   1D54 90 60 00           5446 	mov	dptr,#_DDC0_DR0
   1D57 E0                 5447 	movx	a,@dptr
                           5448 ;     genPointerSet
                           5449 ;     genFarPointerSet
                           5450 ;	Peephole 100	removed redundant mov
   1D58 FA                 5451 	mov	r2,a
   1D59 90 E7 40           5452 	mov	dptr,#_EP0BUF
   1D5C F0                 5453 	movx	@dptr,a
                           5454 ;	Peephole 112.b	changed ljmp to sjmp
   1D5D 80 23              5455 	sjmp	00213$
   1D5F                    5456 00212$:
                           5457 ;src/qs1r_main.c:830: } else if (wIndexL == 1) {
                           5458 ;     genPointerGet
                           5459 ;     genFarPointerGet
   1D5F 90 E6 BC           5460 	mov	dptr,#(_SETUPDAT + 0x0004)
   1D62 E0                 5461 	movx	a,@dptr
   1D63 FA                 5462 	mov	r2,a
                           5463 ;     genCmpEq
                           5464 ;	Peephole 112.b	changed ljmp to sjmp
                           5465 ;	Peephole 199	optimized misc jump sequence
   1D64 BA 01 17           5466 	cjne	r2,#0x01,00209$
                           5467 ;00341$:
                           5468 ;	Peephole 200	removed redundant sjmp
   1D67                    5469 00342$:
                           5470 ;src/qs1r_main.c:831: DDC1_AMR = (RAO >> 8);
                           5471 ;     genAssign
   1D67 90 60 17           5472 	mov	dptr,#_DDC1_AMR
   1D6A 74 03              5473 	mov	a,#0x03
   1D6C F0                 5474 	movx	@dptr,a
                           5475 ;src/qs1r_main.c:832: DDC1_LAR = (RAO & 0xFF);
                           5476 ;     genAssign
   1D6D 90 60 16           5477 	mov	dptr,#_DDC1_LAR
   1D70 74 0B              5478 	mov	a,#0x0B
   1D72 F0                 5479 	movx	@dptr,a
                           5480 ;src/qs1r_main.c:833: EP0BUF[0] = DDC1_DR0;
                           5481 ;     genAssign
   1D73 90 60 10           5482 	mov	dptr,#_DDC1_DR0
   1D76 E0                 5483 	movx	a,@dptr
                           5484 ;     genPointerSet
                           5485 ;     genFarPointerSet
                           5486 ;	Peephole 100	removed redundant mov
   1D77 FA                 5487 	mov	r2,a
   1D78 90 E7 40           5488 	mov	dptr,#_EP0BUF
   1D7B F0                 5489 	movx	@dptr,a
                           5490 ;	Peephole 112.b	changed ljmp to sjmp
   1D7C 80 04              5491 	sjmp	00213$
   1D7E                    5492 00209$:
                           5493 ;src/qs1r_main.c:834: } else return 0;
                           5494 ;     genRet
   1D7E 75 82 00           5495 	mov	dpl,#0x00
                           5496 ;	Peephole 251.a	replaced ljmp to ret with ret
   1D81 22                 5497 	ret
   1D82                    5498 00213$:
                           5499 ;src/qs1r_main.c:835: EP0BCH = 0;
                           5500 ;     genAssign
   1D82 90 E6 8A           5501 	mov	dptr,#_EP0BCH
                           5502 ;	Peephole 181	changed mov to clr
   1D85 E4                 5503 	clr	a
   1D86 F0                 5504 	movx	@dptr,a
                           5505 ;src/qs1r_main.c:836: EP0BCL = 1;
                           5506 ;     genAssign
   1D87 90 E6 8B           5507 	mov	dptr,#_EP0BCL
   1D8A 74 01              5508 	mov	a,#0x01
   1D8C F0                 5509 	movx	@dptr,a
                           5510 ;src/qs1r_main.c:837: break;
   1D8D 02 1F 50           5511 	ljmp	00238$
                           5512 ;src/qs1r_main.c:839: case VRQ_DDC_RTAPS_READ:
   1D90                    5513 00214$:
                           5514 ;src/qs1r_main.c:840: if (wIndexL == 0) {
                           5515 ;     genPointerGet
                           5516 ;     genFarPointerGet
   1D90 90 E6 BC           5517 	mov	dptr,#(_SETUPDAT + 0x0004)
   1D93 E0                 5518 	movx	a,@dptr
                           5519 ;     genCmpEq
                           5520 ;	Peephole 115.b	jump optimization
   1D94 FA                 5521 	mov	r2,a
   1D95 60 02              5522 	jz	00344$
   1D97                    5523 00343$:
                           5524 ;	Peephole 112.b	changed ljmp to sjmp
   1D97 80 17              5525 	sjmp	00219$
   1D99                    5526 00344$:
                           5527 ;src/qs1r_main.c:841: DDC0_AMR = (RTAP >> 8);
                           5528 ;     genAssign
   1D99 90 60 07           5529 	mov	dptr,#_DDC0_AMR
   1D9C 74 03              5530 	mov	a,#0x03
   1D9E F0                 5531 	movx	@dptr,a
                           5532 ;src/qs1r_main.c:842: DDC0_LAR = (RTAP & 0xFF);
                           5533 ;     genAssign
   1D9F 90 60 06           5534 	mov	dptr,#_DDC0_LAR
   1DA2 74 0C              5535 	mov	a,#0x0C
   1DA4 F0                 5536 	movx	@dptr,a
                           5537 ;src/qs1r_main.c:843: EP0BUF[0] = DDC0_DR0;
                           5538 ;     genAssign
   1DA5 90 60 00           5539 	mov	dptr,#_DDC0_DR0
   1DA8 E0                 5540 	movx	a,@dptr
                           5541 ;     genPointerSet
                           5542 ;     genFarPointerSet
                           5543 ;	Peephole 100	removed redundant mov
   1DA9 FA                 5544 	mov	r2,a
   1DAA 90 E7 40           5545 	mov	dptr,#_EP0BUF
   1DAD F0                 5546 	movx	@dptr,a
                           5547 ;	Peephole 112.b	changed ljmp to sjmp
   1DAE 80 23              5548 	sjmp	00220$
   1DB0                    5549 00219$:
                           5550 ;src/qs1r_main.c:844: } else if (wIndexL == 1) {
                           5551 ;     genPointerGet
                           5552 ;     genFarPointerGet
   1DB0 90 E6 BC           5553 	mov	dptr,#(_SETUPDAT + 0x0004)
   1DB3 E0                 5554 	movx	a,@dptr
   1DB4 FA                 5555 	mov	r2,a
                           5556 ;     genCmpEq
                           5557 ;	Peephole 112.b	changed ljmp to sjmp
                           5558 ;	Peephole 199	optimized misc jump sequence
   1DB5 BA 01 17           5559 	cjne	r2,#0x01,00216$
                           5560 ;00345$:
                           5561 ;	Peephole 200	removed redundant sjmp
   1DB8                    5562 00346$:
                           5563 ;src/qs1r_main.c:845: DDC1_AMR = (RTAP >> 8);
                           5564 ;     genAssign
   1DB8 90 60 17           5565 	mov	dptr,#_DDC1_AMR
   1DBB 74 03              5566 	mov	a,#0x03
   1DBD F0                 5567 	movx	@dptr,a
                           5568 ;src/qs1r_main.c:846: DDC1_LAR = (RTAP & 0xFF);
                           5569 ;     genAssign
   1DBE 90 60 16           5570 	mov	dptr,#_DDC1_LAR
   1DC1 74 0C              5571 	mov	a,#0x0C
   1DC3 F0                 5572 	movx	@dptr,a
                           5573 ;src/qs1r_main.c:847: EP0BUF[0] = DDC1_DR0;
                           5574 ;     genAssign
   1DC4 90 60 10           5575 	mov	dptr,#_DDC1_DR0
   1DC7 E0                 5576 	movx	a,@dptr
                           5577 ;     genPointerSet
                           5578 ;     genFarPointerSet
                           5579 ;	Peephole 100	removed redundant mov
   1DC8 FA                 5580 	mov	r2,a
   1DC9 90 E7 40           5581 	mov	dptr,#_EP0BUF
   1DCC F0                 5582 	movx	@dptr,a
                           5583 ;	Peephole 112.b	changed ljmp to sjmp
   1DCD 80 04              5584 	sjmp	00220$
   1DCF                    5585 00216$:
                           5586 ;src/qs1r_main.c:848: } else return 0;
                           5587 ;     genRet
   1DCF 75 82 00           5588 	mov	dpl,#0x00
                           5589 ;	Peephole 251.a	replaced ljmp to ret with ret
   1DD2 22                 5590 	ret
   1DD3                    5591 00220$:
                           5592 ;src/qs1r_main.c:849: EP0BCH = 0;
                           5593 ;     genAssign
   1DD3 90 E6 8A           5594 	mov	dptr,#_EP0BCH
                           5595 ;	Peephole 181	changed mov to clr
   1DD6 E4                 5596 	clr	a
   1DD7 F0                 5597 	movx	@dptr,a
                           5598 ;src/qs1r_main.c:850: EP0BCL = 1;
                           5599 ;     genAssign
   1DD8 90 E6 8B           5600 	mov	dptr,#_EP0BCL
   1DDB 74 01              5601 	mov	a,#0x01
   1DDD F0                 5602 	movx	@dptr,a
                           5603 ;src/qs1r_main.c:851: break;
   1DDE 02 1F 50           5604 	ljmp	00238$
                           5605 ;src/qs1r_main.c:853: case VRQ_DDC_RSVR_READ:
   1DE1                    5606 00221$:
                           5607 ;src/qs1r_main.c:854: if (wIndexL == 0) {
                           5608 ;     genPointerGet
                           5609 ;     genFarPointerGet
   1DE1 90 E6 BC           5610 	mov	dptr,#(_SETUPDAT + 0x0004)
   1DE4 E0                 5611 	movx	a,@dptr
                           5612 ;     genCmpEq
                           5613 ;	Peephole 115.b	jump optimization
   1DE5 FA                 5614 	mov	r2,a
   1DE6 60 02              5615 	jz	00348$
   1DE8                    5616 00347$:
                           5617 ;	Peephole 112.b	changed ljmp to sjmp
   1DE8 80 17              5618 	sjmp	00226$
   1DEA                    5619 00348$:
                           5620 ;src/qs1r_main.c:855: DDC0_AMR = (RSRV >> 8);
                           5621 ;     genAssign
   1DEA 90 60 07           5622 	mov	dptr,#_DDC0_AMR
   1DED 74 03              5623 	mov	a,#0x03
   1DEF F0                 5624 	movx	@dptr,a
                           5625 ;src/qs1r_main.c:856: DDC0_LAR = (RSRV & 0xFF);
                           5626 ;     genAssign
   1DF0 90 60 06           5627 	mov	dptr,#_DDC0_LAR
   1DF3 74 0D              5628 	mov	a,#0x0D
   1DF5 F0                 5629 	movx	@dptr,a
                           5630 ;src/qs1r_main.c:857: EP0BUF[0] = DDC0_DR0;
                           5631 ;     genAssign
   1DF6 90 60 00           5632 	mov	dptr,#_DDC0_DR0
   1DF9 E0                 5633 	movx	a,@dptr
                           5634 ;     genPointerSet
                           5635 ;     genFarPointerSet
                           5636 ;	Peephole 100	removed redundant mov
   1DFA FA                 5637 	mov	r2,a
   1DFB 90 E7 40           5638 	mov	dptr,#_EP0BUF
   1DFE F0                 5639 	movx	@dptr,a
                           5640 ;	Peephole 112.b	changed ljmp to sjmp
   1DFF 80 23              5641 	sjmp	00227$
   1E01                    5642 00226$:
                           5643 ;src/qs1r_main.c:858: } else if (wIndexL == 1) {
                           5644 ;     genPointerGet
                           5645 ;     genFarPointerGet
   1E01 90 E6 BC           5646 	mov	dptr,#(_SETUPDAT + 0x0004)
   1E04 E0                 5647 	movx	a,@dptr
   1E05 FA                 5648 	mov	r2,a
                           5649 ;     genCmpEq
                           5650 ;	Peephole 112.b	changed ljmp to sjmp
                           5651 ;	Peephole 199	optimized misc jump sequence
   1E06 BA 01 17           5652 	cjne	r2,#0x01,00223$
                           5653 ;00349$:
                           5654 ;	Peephole 200	removed redundant sjmp
   1E09                    5655 00350$:
                           5656 ;src/qs1r_main.c:859: DDC1_AMR = (RSRV >> 8);
                           5657 ;     genAssign
   1E09 90 60 17           5658 	mov	dptr,#_DDC1_AMR
   1E0C 74 03              5659 	mov	a,#0x03
   1E0E F0                 5660 	movx	@dptr,a
                           5661 ;src/qs1r_main.c:860: DDC1_LAR = (RSRV & 0xFF);
                           5662 ;     genAssign
   1E0F 90 60 16           5663 	mov	dptr,#_DDC1_LAR
   1E12 74 0D              5664 	mov	a,#0x0D
   1E14 F0                 5665 	movx	@dptr,a
                           5666 ;src/qs1r_main.c:861: EP0BUF[0] = DDC1_DR0;
                           5667 ;     genAssign
   1E15 90 60 10           5668 	mov	dptr,#_DDC1_DR0
   1E18 E0                 5669 	movx	a,@dptr
                           5670 ;     genPointerSet
                           5671 ;     genFarPointerSet
                           5672 ;	Peephole 100	removed redundant mov
   1E19 FA                 5673 	mov	r2,a
   1E1A 90 E7 40           5674 	mov	dptr,#_EP0BUF
   1E1D F0                 5675 	movx	@dptr,a
                           5676 ;	Peephole 112.b	changed ljmp to sjmp
   1E1E 80 04              5677 	sjmp	00227$
   1E20                    5678 00223$:
                           5679 ;src/qs1r_main.c:862: } else return 0;
                           5680 ;     genRet
   1E20 75 82 00           5681 	mov	dpl,#0x00
                           5682 ;	Peephole 251.a	replaced ljmp to ret with ret
   1E23 22                 5683 	ret
   1E24                    5684 00227$:
                           5685 ;src/qs1r_main.c:863: EP0BCH = 0;
                           5686 ;     genAssign
   1E24 90 E6 8A           5687 	mov	dptr,#_EP0BCH
                           5688 ;	Peephole 181	changed mov to clr
   1E27 E4                 5689 	clr	a
   1E28 F0                 5690 	movx	@dptr,a
                           5691 ;src/qs1r_main.c:864: EP0BCL = 1;
                           5692 ;     genAssign
   1E29 90 E6 8B           5693 	mov	dptr,#_EP0BCL
   1E2C 74 01              5694 	mov	a,#0x01
   1E2E F0                 5695 	movx	@dptr,a
                           5696 ;src/qs1r_main.c:865: break;
   1E2F 02 1F 50           5697 	ljmp	00238$
                           5698 ;src/qs1r_main.c:867: case VRQ_DDC_COEF_READ:
   1E32                    5699 00228$:
                           5700 ;src/qs1r_main.c:868: if (wValueL > 0x0F) return 0;
                           5701 ;     genPointerGet
                           5702 ;     genFarPointerGet
   1E32 90 E6 BA           5703 	mov	dptr,#(_SETUPDAT + 0x0002)
   1E35 E0                 5704 	movx	a,@dptr
                           5705 ;     genCmpGt
                           5706 ;     genCmp
                           5707 ;     genIfxJump
                           5708 ;	Peephole 108	removed ljmp by inverse jump logic
                           5709 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
                           5710 ;	Peephole 177.a	removed redundant mov
   1E36 FA                 5711 	mov  r2,a
   1E37 24 F0              5712 	add	a,#0xff - 0x0F
   1E39 50 04              5713 	jnc	00230$
   1E3B                    5714 00351$:
                           5715 ;     genRet
   1E3B 75 82 00           5716 	mov	dpl,#0x00
                           5717 ;	Peephole 251.a	replaced ljmp to ret with ret
   1E3E 22                 5718 	ret
   1E3F                    5719 00230$:
                           5720 ;src/qs1r_main.c:869: offset = wValueL << 4; // advance by 16
                           5721 ;     genPointerGet
                           5722 ;     genFarPointerGet
   1E3F 90 E6 BA           5723 	mov	dptr,#(_SETUPDAT + 0x0002)
   1E42 E0                 5724 	movx	a,@dptr
                           5725 ;     genLeftShift
                           5726 ;     genLeftShiftLiteral
                           5727 ;     genlshOne
                           5728 ;	Peephole 105	removed redundant mov
   1E43 FA                 5729 	mov	r2,a
   1E44 C4                 5730 	swap	a
   1E45 54 F0              5731 	anl	a,#0xf0
   1E47 F5 1D              5732 	mov	_app_vendor_IN_cmd_offset_1_1,a
                           5733 ;src/qs1r_main.c:870: if (wIndexL == 0)
                           5734 ;     genPointerGet
                           5735 ;     genFarPointerGet
   1E49 90 E6 BC           5736 	mov	dptr,#(_SETUPDAT + 0x0004)
   1E4C E0                 5737 	movx	a,@dptr
                           5738 ;     genCmpEq
                           5739 ;	Peephole 115.b	jump optimization
   1E4D FB                 5740 	mov	r3,a
   1E4E 60 02              5741 	jz	00353$
   1E50                    5742 00352$:
                           5743 ;	Peephole 112.b	changed ljmp to sjmp
   1E50 80 72              5744 	sjmp	00235$
   1E52                    5745 00353$:
                           5746 ;src/qs1r_main.c:872: for (i=0, j=0; i < 16; i++, j+=3) {
                           5747 ;     genAssign
   1E52 7B 00              5748 	mov	r3,#0x00
   1E54 7C 00              5749 	mov	r4,#0x00
                           5750 ;     genAssign
   1E56 7D 00              5751 	mov	r5,#0x00
   1E58 7E 00              5752 	mov	r6,#0x00
   1E5A                    5753 00239$:
                           5754 ;     genCmpLt
                           5755 ;     genCmp
   1E5A C3                 5756 	clr	c
   1E5B EB                 5757 	mov	a,r3
   1E5C 94 10              5758 	subb	a,#0x10
   1E5E EC                 5759 	mov	a,r4
   1E5F 64 80              5760 	xrl	a,#0x80
   1E61 94 80              5761 	subb	a,#0x80
                           5762 ;     genIfxJump
   1E63 40 03              5763 	jc	00354$
   1E65 02 1F 3F           5764 	ljmp	00236$
   1E68                    5765 00354$:
                           5766 ;src/qs1r_main.c:873: DDC0_AMR = 0;
                           5767 ;     genAssign
   1E68 90 60 07           5768 	mov	dptr,#_DDC0_AMR
                           5769 ;	Peephole 181	changed mov to clr
   1E6B E4                 5770 	clr	a
   1E6C F0                 5771 	movx	@dptr,a
                           5772 ;src/qs1r_main.c:874: DDC0_LAR = ((i + offset) & 0xFF);
                           5773 ;     genCast
   1E6D 8B 07              5774 	mov	ar7,r3
                           5775 ;     genPlus
   1E6F 90 60 06           5776 	mov	dptr,#_DDC0_LAR
   1E72 E5 1D              5777 	mov	a,_app_vendor_IN_cmd_offset_1_1
                           5778 ;	Peephole 236.a	used r7 instead of ar7
   1E74 2F                 5779 	add	a,r7
   1E75 F0                 5780 	movx	@dptr,a
                           5781 ;src/qs1r_main.c:875: EP0BUF[j] = DDC0_DR0;
                           5782 ;     genPlus
                           5783 ;	Peephole 236.g	used r5 instead of ar5
   1E76 ED                 5784 	mov	a,r5
   1E77 24 40              5785 	add	a,#_EP0BUF
   1E79 FF                 5786 	mov	r7,a
                           5787 ;	Peephole 236.g	used r6 instead of ar6
   1E7A EE                 5788 	mov	a,r6
   1E7B 34 E7              5789 	addc	a,#(_EP0BUF >> 8)
   1E7D F8                 5790 	mov	r0,a
                           5791 ;     genAssign
   1E7E 90 60 00           5792 	mov	dptr,#_DDC0_DR0
   1E81 E0                 5793 	movx	a,@dptr
                           5794 ;     genPointerSet
                           5795 ;     genFarPointerSet
                           5796 ;	Peephole 136	removed redundant moves
   1E82 F9                 5797 	mov	r1,a
   1E83 8F 82              5798 	mov	dpl,r7
   1E85 88 83              5799 	mov	dph,r0
   1E87 F0                 5800 	movx	@dptr,a
                           5801 ;src/qs1r_main.c:876: EP0BUF[j+1] = DDC0_DR1;
                           5802 ;     genCast
   1E88 8D 07              5803 	mov	ar7,r5
                           5804 ;     genPlus
                           5805 ;     genPlusIncr
   1E8A 74 01              5806 	mov	a,#0x01
                           5807 ;	Peephole 236.a	used r7 instead of ar7
   1E8C 2F                 5808 	add	a,r7
                           5809 ;     genPlus
   1E8D 24 40              5810 	add	a,#_EP0BUF
   1E8F F8                 5811 	mov	r0,a
                           5812 ;	Peephole 240	use clr instead of addc a,#0
   1E90 E4                 5813 	clr	a
   1E91 34 E7              5814 	addc	a,#(_EP0BUF >> 8)
   1E93 F9                 5815 	mov	r1,a
                           5816 ;     genAssign
   1E94 90 60 01           5817 	mov	dptr,#_DDC0_DR1
   1E97 E0                 5818 	movx	a,@dptr
                           5819 ;     genPointerSet
                           5820 ;     genFarPointerSet
                           5821 ;	Peephole 136	removed redundant moves
   1E98 FA                 5822 	mov	r2,a
   1E99 88 82              5823 	mov	dpl,r0
   1E9B 89 83              5824 	mov	dph,r1
   1E9D F0                 5825 	movx	@dptr,a
                           5826 ;src/qs1r_main.c:877: EP0BUF[j+2] = (DDC0_DR2 & 0x0F);
                           5827 ;     genPlus
                           5828 ;     genPlusIncr
   1E9E 74 02              5829 	mov	a,#0x02
                           5830 ;	Peephole 236.a	used r7 instead of ar7
   1EA0 2F                 5831 	add	a,r7
                           5832 ;     genPlus
   1EA1 24 40              5833 	add	a,#_EP0BUF
   1EA3 FA                 5834 	mov	r2,a
                           5835 ;	Peephole 240	use clr instead of addc a,#0
   1EA4 E4                 5836 	clr	a
   1EA5 34 E7              5837 	addc	a,#(_EP0BUF >> 8)
   1EA7 FF                 5838 	mov	r7,a
                           5839 ;     genAssign
   1EA8 90 60 02           5840 	mov	dptr,#_DDC0_DR2
   1EAB E0                 5841 	movx	a,@dptr
   1EAC F8                 5842 	mov	r0,a
                           5843 ;     genAnd
   1EAD 53 00 0F           5844 	anl	ar0,#0x0F
                           5845 ;     genPointerSet
                           5846 ;     genFarPointerSet
   1EB0 8A 82              5847 	mov	dpl,r2
   1EB2 8F 83              5848 	mov	dph,r7
   1EB4 E8                 5849 	mov	a,r0
   1EB5 F0                 5850 	movx	@dptr,a
                           5851 ;src/qs1r_main.c:872: for (i=0, j=0; i < 16; i++, j+=3) {
                           5852 ;     genPlus
                           5853 ;     genPlusIncr
   1EB6 0B                 5854 	inc	r3
   1EB7 BB 00 01           5855 	cjne	r3,#0x00,00355$
   1EBA 0C                 5856 	inc	r4
   1EBB                    5857 00355$:
                           5858 ;     genPlus
                           5859 ;     genPlusIncr
   1EBB 74 03              5860 	mov	a,#0x03
                           5861 ;	Peephole 236.a	used r5 instead of ar5
   1EBD 2D                 5862 	add	a,r5
   1EBE FD                 5863 	mov	r5,a
                           5864 ;	Peephole 181	changed mov to clr
   1EBF E4                 5865 	clr	a
                           5866 ;	Peephole 236.b	used r6 instead of ar6
   1EC0 3E                 5867 	addc	a,r6
   1EC1 FE                 5868 	mov	r6,a
                           5869 ;	Peephole 112.b	changed ljmp to sjmp
   1EC2 80 96              5870 	sjmp	00239$
   1EC4                    5871 00235$:
                           5872 ;src/qs1r_main.c:879: } else if (wIndexL == 1) {
                           5873 ;     genPointerGet
                           5874 ;     genFarPointerGet
   1EC4 90 E6 BC           5875 	mov	dptr,#(_SETUPDAT + 0x0004)
   1EC7 E0                 5876 	movx	a,@dptr
   1EC8 FA                 5877 	mov	r2,a
                           5878 ;     genCmpEq
                           5879 ;	Peephole 112.b	changed ljmp to sjmp
                           5880 ;	Peephole 199	optimized misc jump sequence
   1EC9 BA 01 6F           5881 	cjne	r2,#0x01,00232$
                           5882 ;00356$:
                           5883 ;	Peephole 200	removed redundant sjmp
   1ECC                    5884 00357$:
                           5885 ;src/qs1r_main.c:880: for (i=0, j=0; i < 16; i++, j+=3) {
                           5886 ;     genAssign
   1ECC 7A 00              5887 	mov	r2,#0x00
   1ECE 7B 00              5888 	mov	r3,#0x00
                           5889 ;     genAssign
   1ED0 7C 00              5890 	mov	r4,#0x00
   1ED2 7D 00              5891 	mov	r5,#0x00
   1ED4                    5892 00243$:
                           5893 ;     genCmpLt
                           5894 ;     genCmp
   1ED4 C3                 5895 	clr	c
   1ED5 EA                 5896 	mov	a,r2
   1ED6 94 10              5897 	subb	a,#0x10
   1ED8 EB                 5898 	mov	a,r3
   1ED9 64 80              5899 	xrl	a,#0x80
   1EDB 94 80              5900 	subb	a,#0x80
                           5901 ;     genIfxJump
                           5902 ;	Peephole 108	removed ljmp by inverse jump logic
   1EDD 50 60              5903 	jnc	00236$
   1EDF                    5904 00358$:
                           5905 ;src/qs1r_main.c:881: DDC1_AMR = 0;
                           5906 ;     genAssign
   1EDF 90 60 17           5907 	mov	dptr,#_DDC1_AMR
                           5908 ;	Peephole 181	changed mov to clr
   1EE2 E4                 5909 	clr	a
   1EE3 F0                 5910 	movx	@dptr,a
                           5911 ;src/qs1r_main.c:882: DDC1_LAR = ((i + offset) & 0xFF);
                           5912 ;     genCast
   1EE4 8A 06              5913 	mov	ar6,r2
                           5914 ;     genPlus
   1EE6 90 60 16           5915 	mov	dptr,#_DDC1_LAR
   1EE9 E5 1D              5916 	mov	a,_app_vendor_IN_cmd_offset_1_1
                           5917 ;	Peephole 236.a	used r6 instead of ar6
   1EEB 2E                 5918 	add	a,r6
   1EEC F0                 5919 	movx	@dptr,a
                           5920 ;src/qs1r_main.c:883: EP0BUF[j] = DDC1_DR0;
                           5921 ;     genPlus
                           5922 ;	Peephole 236.g	used r4 instead of ar4
   1EED EC                 5923 	mov	a,r4
   1EEE 24 40              5924 	add	a,#_EP0BUF
   1EF0 FE                 5925 	mov	r6,a
                           5926 ;	Peephole 236.g	used r5 instead of ar5
   1EF1 ED                 5927 	mov	a,r5
   1EF2 34 E7              5928 	addc	a,#(_EP0BUF >> 8)
   1EF4 FF                 5929 	mov	r7,a
                           5930 ;     genAssign
   1EF5 90 60 10           5931 	mov	dptr,#_DDC1_DR0
   1EF8 E0                 5932 	movx	a,@dptr
                           5933 ;     genPointerSet
                           5934 ;     genFarPointerSet
                           5935 ;	Peephole 136	removed redundant moves
   1EF9 F8                 5936 	mov	r0,a
   1EFA 8E 82              5937 	mov	dpl,r6
   1EFC 8F 83              5938 	mov	dph,r7
   1EFE F0                 5939 	movx	@dptr,a
                           5940 ;src/qs1r_main.c:884: EP0BUF[j+1] = DDC1_DR1;
                           5941 ;     genCast
   1EFF 8C 06              5942 	mov	ar6,r4
                           5943 ;     genPlus
                           5944 ;     genPlusIncr
   1F01 74 01              5945 	mov	a,#0x01
                           5946 ;	Peephole 236.a	used r6 instead of ar6
   1F03 2E                 5947 	add	a,r6
                           5948 ;     genPlus
   1F04 24 40              5949 	add	a,#_EP0BUF
   1F06 FF                 5950 	mov	r7,a
                           5951 ;	Peephole 240	use clr instead of addc a,#0
   1F07 E4                 5952 	clr	a
   1F08 34 E7              5953 	addc	a,#(_EP0BUF >> 8)
   1F0A F8                 5954 	mov	r0,a
                           5955 ;     genAssign
   1F0B 90 60 11           5956 	mov	dptr,#_DDC1_DR1
   1F0E E0                 5957 	movx	a,@dptr
                           5958 ;     genPointerSet
                           5959 ;     genFarPointerSet
                           5960 ;	Peephole 136	removed redundant moves
   1F0F F9                 5961 	mov	r1,a
   1F10 8F 82              5962 	mov	dpl,r7
   1F12 88 83              5963 	mov	dph,r0
   1F14 F0                 5964 	movx	@dptr,a
                           5965 ;src/qs1r_main.c:885: EP0BUF[j+2] = (DDC1_DR2 & 0x0F);
                           5966 ;     genPlus
                           5967 ;     genPlusIncr
   1F15 74 02              5968 	mov	a,#0x02
                           5969 ;	Peephole 236.a	used r6 instead of ar6
   1F17 2E                 5970 	add	a,r6
                           5971 ;     genPlus
   1F18 24 40              5972 	add	a,#_EP0BUF
   1F1A FE                 5973 	mov	r6,a
                           5974 ;	Peephole 240	use clr instead of addc a,#0
   1F1B E4                 5975 	clr	a
   1F1C 34 E7              5976 	addc	a,#(_EP0BUF >> 8)
   1F1E FF                 5977 	mov	r7,a
                           5978 ;     genAssign
   1F1F 90 60 12           5979 	mov	dptr,#_DDC1_DR2
   1F22 E0                 5980 	movx	a,@dptr
   1F23 F8                 5981 	mov	r0,a
                           5982 ;     genAnd
   1F24 53 00 0F           5983 	anl	ar0,#0x0F
                           5984 ;     genPointerSet
                           5985 ;     genFarPointerSet
   1F27 8E 82              5986 	mov	dpl,r6
   1F29 8F 83              5987 	mov	dph,r7
   1F2B E8                 5988 	mov	a,r0
   1F2C F0                 5989 	movx	@dptr,a
                           5990 ;src/qs1r_main.c:880: for (i=0, j=0; i < 16; i++, j+=3) {
                           5991 ;     genPlus
                           5992 ;     genPlusIncr
   1F2D 0A                 5993 	inc	r2
   1F2E BA 00 01           5994 	cjne	r2,#0x00,00359$
   1F31 0B                 5995 	inc	r3
   1F32                    5996 00359$:
                           5997 ;     genPlus
                           5998 ;     genPlusIncr
   1F32 74 03              5999 	mov	a,#0x03
                           6000 ;	Peephole 236.a	used r4 instead of ar4
   1F34 2C                 6001 	add	a,r4
   1F35 FC                 6002 	mov	r4,a
                           6003 ;	Peephole 181	changed mov to clr
   1F36 E4                 6004 	clr	a
                           6005 ;	Peephole 236.b	used r5 instead of ar5
   1F37 3D                 6006 	addc	a,r5
   1F38 FD                 6007 	mov	r5,a
                           6008 ;	Peephole 112.b	changed ljmp to sjmp
   1F39 80 99              6009 	sjmp	00243$
   1F3B                    6010 00232$:
                           6011 ;src/qs1r_main.c:887: } else return 0;
                           6012 ;     genRet
   1F3B 75 82 00           6013 	mov	dpl,#0x00
                           6014 ;	Peephole 112.b	changed ljmp to sjmp
                           6015 ;	Peephole 251.b	replaced sjmp to ret with ret
   1F3E 22                 6016 	ret
   1F3F                    6017 00236$:
                           6018 ;src/qs1r_main.c:888: EP0BCH = 0;
                           6019 ;     genAssign
   1F3F 90 E6 8A           6020 	mov	dptr,#_EP0BCH
                           6021 ;	Peephole 181	changed mov to clr
   1F42 E4                 6022 	clr	a
   1F43 F0                 6023 	movx	@dptr,a
                           6024 ;src/qs1r_main.c:889: EP0BCL = 48;
                           6025 ;     genAssign
   1F44 90 E6 8B           6026 	mov	dptr,#_EP0BCL
   1F47 74 30              6027 	mov	a,#0x30
   1F49 F0                 6028 	movx	@dptr,a
                           6029 ;src/qs1r_main.c:890: break;
                           6030 ;src/qs1r_main.c:892: default:
                           6031 ;	Peephole 112.b	changed ljmp to sjmp
   1F4A 80 04              6032 	sjmp	00238$
   1F4C                    6033 00237$:
                           6034 ;src/qs1r_main.c:893: return 0;
                           6035 ;     genRet
   1F4C 75 82 00           6036 	mov	dpl,#0x00
                           6037 ;src/qs1r_main.c:894: }
                           6038 ;	Peephole 112.b	changed ljmp to sjmp
                           6039 ;src/qs1r_main.c:895: return 1;
                           6040 ;     genRet
                           6041 ;	Peephole 237.a	removed sjmp to ret
   1F4F 22                 6042 	ret
   1F50                    6043 00238$:
   1F50 75 82 01           6044 	mov	dpl,#0x01
   1F53                    6045 00247$:
   1F53 22                 6046 	ret
                           6047 ;------------------------------------------------------------
                           6048 ;Allocation info for local variables in function 'app_vendor_cmd'
                           6049 ;------------------------------------------------------------
                           6050 ;------------------------------------------------------------
                           6051 ;src/qs1r_main.c:899: app_vendor_cmd (void)
                           6052 ;	-----------------------------------------
                           6053 ;	 function app_vendor_cmd
                           6054 ;	-----------------------------------------
   1F54                    6055 _app_vendor_cmd:
                           6056 ;src/qs1r_main.c:901: if (bRequestType == VRT_VENDOR_IN)
                           6057 ;     genPointerGet
                           6058 ;     genFarPointerGet
   1F54 90 E6 B8           6059 	mov	dptr,#_SETUPDAT
   1F57 E0                 6060 	movx	a,@dptr
   1F58 FA                 6061 	mov	r2,a
                           6062 ;     genCmpEq
                           6063 ;	Peephole 112.b	changed ljmp to sjmp
                           6064 ;	Peephole 199	optimized misc jump sequence
   1F59 BA C0 03           6065 	cjne	r2,#0xC0,00105$
                           6066 ;00111$:
                           6067 ;	Peephole 200	removed redundant sjmp
   1F5C                    6068 00112$:
                           6069 ;src/qs1r_main.c:902: return app_vendor_IN_cmd();
                           6070 ;     genCall
                           6071 ;     genRet
                           6072 ;	Peephole 112.b	changed ljmp to sjmp
                           6073 ;	Peephole 251.b	replaced sjmp to ret with ret
                           6074 ;	Peephole 253.a	replaced lcall/ret with ljmp
   1F5C 02 16 58           6075 	ljmp	_app_vendor_IN_cmd
   1F5F                    6076 00105$:
                           6077 ;src/qs1r_main.c:903: else if (bRequestType == VRT_VENDOR_OUT)
                           6078 ;     genPointerGet
                           6079 ;     genFarPointerGet
   1F5F 90 E6 B8           6080 	mov	dptr,#_SETUPDAT
   1F62 E0                 6081 	movx	a,@dptr
   1F63 FA                 6082 	mov	r2,a
                           6083 ;     genCmpEq
                           6084 ;	Peephole 112.b	changed ljmp to sjmp
                           6085 ;	Peephole 199	optimized misc jump sequence
   1F64 BA 40 03           6086 	cjne	r2,#0x40,00102$
                           6087 ;00113$:
                           6088 ;	Peephole 200	removed redundant sjmp
   1F67                    6089 00114$:
                           6090 ;src/qs1r_main.c:904: return app_vendor_OUT_cmd();
                           6091 ;     genCall
                           6092 ;     genRet
                           6093 ;	Peephole 112.b	changed ljmp to sjmp
                           6094 ;src/qs1r_main.c:906: return 0;    // invalid bRequestType
                           6095 ;     genRet
                           6096 ;	Peephole 237.a	removed sjmp to ret
                           6097 ;	Peephole 253.a	replaced lcall/ret with ljmp
   1F67 02 0D 7C           6098 	ljmp	_app_vendor_OUT_cmd
   1F6A                    6099 00102$:
   1F6A 75 82 00           6100 	mov	dpl,#0x00
   1F6D                    6101 00107$:
   1F6D 22                 6102 	ret
                           6103 ;------------------------------------------------------------
                           6104 ;Allocation info for local variables in function 'main_loop'
                           6105 ;------------------------------------------------------------
                           6106 ;------------------------------------------------------------
                           6107 ;src/qs1r_main.c:910: main_loop (void)
                           6108 ;	-----------------------------------------
                           6109 ;	 function main_loop
                           6110 ;	-----------------------------------------
   1F6E                    6111 _main_loop:
                           6112 ;src/qs1r_main.c:913: while (1)
   1F6E                    6113 00105$:
                           6114 ;src/qs1r_main.c:915: if (usb_setup_packet_avail ())
                           6115 ;     genIfx
                           6116 ;     genIfxJump
                           6117 ;	Peephole 111	removed ljmp by inverse jump logic
   1F6E 30 03 05           6118 	jnb	__usb_got_SUDAV,00102$
   1F71                    6119 00111$:
                           6120 ;src/qs1r_main.c:916: usb_handle_setup_packet ();
                           6121 ;     genCall
   1F71 12 0C 6C           6122 	lcall	_usb_handle_setup_packet
                           6123 ;	Peephole 112.b	changed ljmp to sjmp
   1F74 80 F8              6124 	sjmp	00105$
   1F76                    6125 00102$:
                           6126 ;src/qs1r_main.c:918: putchar(32);
                           6127 ;     genCall
   1F76 75 82 20           6128 	mov	dpl,#0x20
   1F79 12 01 A2           6129 	lcall	_putchar
                           6130 ;	Peephole 112.b	changed ljmp to sjmp
   1F7C 80 F0              6131 	sjmp	00105$
   1F7E                    6132 00107$:
   1F7E 22                 6133 	ret
                           6134 ;------------------------------------------------------------
                           6135 ;Allocation info for local variables in function 'isr_tick'
                           6136 ;------------------------------------------------------------
                           6137 ;count                     Allocated with name '_isr_tick_count_1_1'
                           6138 ;------------------------------------------------------------
                           6139 ;src/qs1r_main.c:929: isr_tick (void) interrupt
                           6140 ;	-----------------------------------------
                           6141 ;	 function isr_tick
                           6142 ;	-----------------------------------------
   1F7F                    6143 _isr_tick:
   1F7F C0 E0              6144 	push	acc
                           6145 ;src/qs1r_main.c:932: if (--count == 0)
                           6146 ;     genMinus
                           6147 ;     genMinusDec
   1F81 15 1E              6148 	dec	_isr_tick_count_1_1
                           6149 ;     genCmpEq
   1F83 E5 1E              6150 	mov	a,_isr_tick_count_1_1
                           6151 ;	Peephole 162	removed sjmp by inverse jump logic
   1F85 60 02              6152 	jz	00107$
   1F87                    6153 00106$:
                           6154 ;	Peephole 112.b	changed ljmp to sjmp
   1F87 80 03              6155 	sjmp	00102$
   1F89                    6156 00107$:
                           6157 ;src/qs1r_main.c:934: count = 50;
                           6158 ;     genAssign
   1F89 75 1E 32           6159 	mov	_isr_tick_count_1_1,#0x32
   1F8C                    6160 00102$:
                           6161 ;src/qs1r_main.c:937: clear_timer_irq ();
                           6162 ;     genAssign
   1F8C C2 CF              6163 	clr	_TF2
   1F8E                    6164 00103$:
   1F8E D0 E0              6165 	pop	acc
   1F90 32                 6166 	reti
                           6167 ;	eliminated unneeded push/pop psw
                           6168 ;	eliminated unneeded push/pop dpl
                           6169 ;	eliminated unneeded push/pop dph
                           6170 ;	eliminated unneeded push/pop b
                           6171 ;------------------------------------------------------------
                           6172 ;Allocation info for local variables in function 'patch_usb_descriptors'
                           6173 ;------------------------------------------------------------
                           6174 ;hw_rev                    Allocated with name '_patch_usb_descriptors_hw_rev_1_1'
                           6175 ;serial_no                 Allocated with name '_patch_usb_descriptors_serial_no_1_1'
                           6176 ;------------------------------------------------------------
                           6177 ;src/qs1r_main.c:945: patch_usb_descriptors(void)
                           6178 ;	-----------------------------------------
                           6179 ;	 function patch_usb_descriptors
                           6180 ;	-----------------------------------------
   1F91                    6181 _patch_usb_descriptors:
                           6182 ;src/qs1r_main.c:950: eeprom_read(I2C_ADDR_BOOT, HW_REV_OFFSET, &hw_rev, 1);	// LSB of device id
                           6183 ;     genAddrOf
   1F91 75 09 02           6184 	mov	_eeprom_read_PARM_3,#_patch_usb_descriptors_hw_rev_1_1
   1F94 75 0A 40           6185 	mov	(_eeprom_read_PARM_3 + 1),#(_patch_usb_descriptors_hw_rev_1_1 >> 8)
                           6186 ;     genAssign
   1F97 75 08 05           6187 	mov	_eeprom_read_PARM_2,#0x05
                           6188 ;     genAssign
   1F9A 75 0B 01           6189 	mov	_eeprom_read_PARM_4,#0x01
                           6190 ;     genCall
   1F9D 75 82 51           6191 	mov	dpl,#0x51
   1FA0 12 01 EA           6192 	lcall	_eeprom_read
                           6193 ;src/qs1r_main.c:952: usb_desc_hw_rev_binary_patch_location_0[0] = hw_rev;
                           6194 ;     genAssign
   1FA3 90 40 02           6195 	mov	dptr,#_patch_usb_descriptors_hw_rev_1_1
   1FA6 E0                 6196 	movx	a,@dptr
                           6197 ;     genPointerSet
                           6198 ;     genFarPointerSet
                           6199 ;	Peephole 100	removed redundant mov
   1FA7 FA                 6200 	mov	r2,a
   1FA8 90 E0 0C           6201 	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_0
   1FAB F0                 6202 	movx	@dptr,a
                           6203 ;src/qs1r_main.c:953: usb_desc_hw_rev_binary_patch_location_1[0] = hw_rev;
                           6204 ;     genPointerSet
                           6205 ;     genFarPointerSet
   1FAC 90 E0 48           6206 	mov	dptr,#_usb_desc_hw_rev_binary_patch_location_1
   1FAF EA                 6207 	mov	a,r2
   1FB0 F0                 6208 	movx	@dptr,a
                           6209 ;src/qs1r_main.c:954: usb_desc_hw_rev_ascii_patch_location_0[0] = hw_rev + '0';     // FIXME if we get > 9
                           6210 ;     genPlus
   1FB1 74 30              6211 	mov	a,#0x30
                           6212 ;	Peephole 236.a	used r2 instead of ar2
   1FB3 2A                 6213 	add	a,r2
                           6214 ;     genPointerSet
                           6215 ;     genFarPointerSet
                           6216 ;	Peephole 100	removed redundant mov
   1FB4 FA                 6217 	mov	r2,a
   1FB5 90 E0 C2           6218 	mov	dptr,#_usb_desc_hw_rev_ascii_patch_location_0
   1FB8 F0                 6219 	movx	@dptr,a
   1FB9                    6220 00101$:
   1FB9 22                 6221 	ret
                           6222 ;------------------------------------------------------------
                           6223 ;Allocation info for local variables in function 'main'
                           6224 ;------------------------------------------------------------
                           6225 ;------------------------------------------------------------
                           6226 ;src/qs1r_main.c:965: main (void)
                           6227 ;	-----------------------------------------
                           6228 ;	 function main
                           6229 ;	-----------------------------------------
   1FBA                    6230 _main:
                           6231 ;src/qs1r_main.c:967: init_qs1r();
                           6232 ;     genCall
   1FBA 12 0C AA           6233 	lcall	_init_qs1r
                           6234 ;src/qs1r_main.c:969: EA = 0;		// disable all interrupts
                           6235 ;     genAssign
   1FBD C2 AF              6236 	clr	_EA
                           6237 ;src/qs1r_main.c:971: patch_usb_descriptors();
                           6238 ;     genCall
   1FBF 12 1F 91           6239 	lcall	_patch_usb_descriptors
                           6240 ;src/qs1r_main.c:973: setup_autovectors ();
                           6241 ;     genCall
   1FC2 12 05 CA           6242 	lcall	_setup_autovectors
                           6243 ;src/qs1r_main.c:974: usb_install_handlers ();
                           6244 ;     genCall
   1FC5 12 09 3E           6245 	lcall	_usb_install_handlers
                           6246 ;src/qs1r_main.c:975: hook_timer_tick ((unsigned short) isr_tick);
                           6247 ;     genCast
   1FC8 7A 7F              6248 	mov	r2,#_isr_tick
   1FCA 7B 1F              6249 	mov	r3,#(_isr_tick >> 8)
                           6250 ;     genCall
   1FCC 8A 82              6251 	mov	dpl,r2
   1FCE 8B 83              6252 	mov	dph,r3
   1FD0 12 08 5D           6253 	lcall	_hook_timer_tick
                           6254 ;src/qs1r_main.c:977: EIEX4 = 1;	// disable INT4 FIXME
                           6255 ;     genAssign
   1FD3 D2 EA              6256 	setb	_EIEX4
                           6257 ;src/qs1r_main.c:978: EA = 1;			// global interrupt enable
                           6258 ;     genAssign
   1FD5 D2 AF              6259 	setb	_EA
                           6260 ;src/qs1r_main.c:980: fx2_renumerate ();	// simulates disconnect / reconnect
                           6261 ;     genCall
   1FD7 12 03 21           6262 	lcall	_fx2_renumerate
                           6263 ;src/qs1r_main.c:982: main_loop ();
                           6264 ;     genCall
                           6265 ;	Peephole 253.b	replaced lcall/ret with ljmp
   1FDA 02 1F 6E           6266 	ljmp	_main_loop
                           6267 	.area CSEG    (CODE)
