                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Fri Aug 04 13:42:03 2006
                              5 ;--------------------------------------------------------
                              6 	.module usb_common
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _epcs
                             13 	.globl _plausible_endpoint
                             14 	.globl _EIPX6
                             15 	.globl _EIPX5
                             16 	.globl _EIPX4
                             17 	.globl _PI2C
                             18 	.globl _PUSB
                             19 	.globl _EIEX6
                             20 	.globl _EIEX5
                             21 	.globl _EIEX4
                             22 	.globl _EI2C
                             23 	.globl _EIUSB
                             24 	.globl _SMOD1
                             25 	.globl _ERESI
                             26 	.globl _RESI
                             27 	.globl _INT6
                             28 	.globl _CY
                             29 	.globl _AC
                             30 	.globl _F0
                             31 	.globl _RS1
                             32 	.globl _RS0
                             33 	.globl _OV
                             34 	.globl _FL
                             35 	.globl _P
                             36 	.globl _TF2
                             37 	.globl _EXF2
                             38 	.globl _RCLK
                             39 	.globl _TCLK
                             40 	.globl _EXEN2
                             41 	.globl _TR2
                             42 	.globl _C_T2
                             43 	.globl _CP_RL2
                             44 	.globl _SM01
                             45 	.globl _SM11
                             46 	.globl _SM21
                             47 	.globl _REN1
                             48 	.globl _TB81
                             49 	.globl _RB81
                             50 	.globl _TI1
                             51 	.globl _RI1
                             52 	.globl _PS1
                             53 	.globl _PT2
                             54 	.globl _PS0
                             55 	.globl _PT1
                             56 	.globl _PX1
                             57 	.globl _PT0
                             58 	.globl _PX0
                             59 	.globl _EA
                             60 	.globl _ES1
                             61 	.globl _ET2
                             62 	.globl _ES0
                             63 	.globl _ET1
                             64 	.globl _EX1
                             65 	.globl _ET0
                             66 	.globl _EX0
                             67 	.globl _SM0
                             68 	.globl _SM1
                             69 	.globl _SM2
                             70 	.globl _REN
                             71 	.globl _TB8
                             72 	.globl _RB8
                             73 	.globl _TI
                             74 	.globl _RI
                             75 	.globl _TF1
                             76 	.globl _TR1
                             77 	.globl _TF0
                             78 	.globl _TR0
                             79 	.globl _IE1
                             80 	.globl _IT1
                             81 	.globl _IE0
                             82 	.globl _IT0
                             83 	.globl _SEL
                             84 	.globl _EIP
                             85 	.globl _B
                             86 	.globl _EIE
                             87 	.globl _ACC
                             88 	.globl _EICON
                             89 	.globl _PSW
                             90 	.globl _TH2
                             91 	.globl _TL2
                             92 	.globl _RCAP2H
                             93 	.globl _RCAP2L
                             94 	.globl _T2CON
                             95 	.globl _SBUF1
                             96 	.globl _SCON1
                             97 	.globl _GPIFSGLDATLNOX
                             98 	.globl _GPIFSGLDATLX
                             99 	.globl _GPIFSGLDATH
                            100 	.globl _GPIFTRIG
                            101 	.globl _EP01STAT
                            102 	.globl _IP
                            103 	.globl _OEE
                            104 	.globl _OED
                            105 	.globl _OEC
                            106 	.globl _OEB
                            107 	.globl _OEA
                            108 	.globl _IOE
                            109 	.globl _IOD
                            110 	.globl _AUTOPTRSETUP
                            111 	.globl _EP68FIFOFLGS
                            112 	.globl _EP24FIFOFLGS
                            113 	.globl _EP2468STAT
                            114 	.globl _IE
                            115 	.globl _INT4CLR
                            116 	.globl _INT2CLR
                            117 	.globl _IOC
                            118 	.globl _AUTODAT2
                            119 	.globl _AUTOPTRL2
                            120 	.globl _AUTOPTRH2
                            121 	.globl _AUTODAT1
                            122 	.globl _APTR1L
                            123 	.globl _APTR1H
                            124 	.globl _SBUF0
                            125 	.globl _SCON0
                            126 	.globl _MPAGE
                            127 	.globl _EXIF
                            128 	.globl _IOB
                            129 	.globl _CKCON
                            130 	.globl _TH1
                            131 	.globl _TH0
                            132 	.globl _TL1
                            133 	.globl _TL0
                            134 	.globl _TMOD
                            135 	.globl _TCON
                            136 	.globl _PCON
                            137 	.globl _DPS
                            138 	.globl _DPH1
                            139 	.globl _DPL1
                            140 	.globl _DPH
                            141 	.globl _DPL
                            142 	.globl _SP
                            143 	.globl _IOA
                            144 	.globl _EP8FIFOBUF
                            145 	.globl _EP6FIFOBUF
                            146 	.globl _EP4FIFOBUF
                            147 	.globl _EP2FIFOBUF
                            148 	.globl _EP1INBUF
                            149 	.globl _EP1OUTBUF
                            150 	.globl _EP0BUF
                            151 	.globl _CT4
                            152 	.globl _CT3
                            153 	.globl _CT2
                            154 	.globl _CT1
                            155 	.globl _USBTEST
                            156 	.globl _TESTCFG
                            157 	.globl _DBUG
                            158 	.globl _UDMACRCQUAL
                            159 	.globl _UDMACRCL
                            160 	.globl _UDMACRCH
                            161 	.globl _GPIFHOLDAMOUNT
                            162 	.globl _FLOWSTBHPERIOD
                            163 	.globl _FLOWSTBEDGE
                            164 	.globl _FLOWSTB
                            165 	.globl _FLOWHOLDOFF
                            166 	.globl _FLOWEQ1CTL
                            167 	.globl _FLOWEQ0CTL
                            168 	.globl _FLOWLOGIC
                            169 	.globl _FLOWSTATE
                            170 	.globl _GPIFABORT
                            171 	.globl _GPIFREADYSTAT
                            172 	.globl _GPIFREADYCFG
                            173 	.globl _XGPIFSGLDATLNOX
                            174 	.globl _XGPIFSGLDATLX
                            175 	.globl _XGPIFSGLDATH
                            176 	.globl _EP8GPIFTRIG
                            177 	.globl _EP8GPIFPFSTOP
                            178 	.globl _EP8GPIFFLGSEL
                            179 	.globl _EP6GPIFTRIG
                            180 	.globl _EP6GPIFPFSTOP
                            181 	.globl _EP6GPIFFLGSEL
                            182 	.globl _EP4GPIFTRIG
                            183 	.globl _EP4GPIFPFSTOP
                            184 	.globl _EP4GPIFFLGSEL
                            185 	.globl _EP2GPIFTRIG
                            186 	.globl _EP2GPIFPFSTOP
                            187 	.globl _EP2GPIFFLGSEL
                            188 	.globl _GPIFTCB0
                            189 	.globl _GPIFTCB1
                            190 	.globl _GPIFTCB2
                            191 	.globl _GPIFTCB3
                            192 	.globl _GPIFADRL
                            193 	.globl _GPIFADRH
                            194 	.globl _GPIFCTLCFG
                            195 	.globl _GPIFIDLECTL
                            196 	.globl _GPIFIDLECS
                            197 	.globl _GPIFWFSELECT
                            198 	.globl _SETUPDAT
                            199 	.globl _SUDPTRCTL
                            200 	.globl _SUDPTRL
                            201 	.globl _SUDPTRH
                            202 	.globl _EP8FIFOBCL
                            203 	.globl _EP8FIFOBCH
                            204 	.globl _EP6FIFOBCL
                            205 	.globl _EP6FIFOBCH
                            206 	.globl _EP4FIFOBCL
                            207 	.globl _EP4FIFOBCH
                            208 	.globl _EP2FIFOBCL
                            209 	.globl _EP2FIFOBCH
                            210 	.globl _EP8FIFOFLGS
                            211 	.globl _EP6FIFOFLGS
                            212 	.globl _EP4FIFOFLGS
                            213 	.globl _EP2FIFOFLGS
                            214 	.globl _EP8CS
                            215 	.globl _EP6CS
                            216 	.globl _EP4CS
                            217 	.globl _EP2CS
                            218 	.globl _EP1INCS
                            219 	.globl _EP1OUTCS
                            220 	.globl _EP0CS
                            221 	.globl _EP8BCL
                            222 	.globl _EP8BCH
                            223 	.globl _EP6BCL
                            224 	.globl _EP6BCH
                            225 	.globl _EP4BCL
                            226 	.globl _EP4BCH
                            227 	.globl _EP2BCL
                            228 	.globl _EP2BCH
                            229 	.globl _EP1INBC
                            230 	.globl _EP1OUTBC
                            231 	.globl _EP0BCL
                            232 	.globl _EP0BCH
                            233 	.globl _FNADDR
                            234 	.globl _MICROFRAME
                            235 	.globl _USBFRAMEL
                            236 	.globl _USBFRAMEH
                            237 	.globl _TOGCTL
                            238 	.globl _WAKEUPCS
                            239 	.globl _SUSPEND
                            240 	.globl _USBCS
                            241 	.globl _XAUTODAT2
                            242 	.globl _XAUTODAT1
                            243 	.globl _I2CTL
                            244 	.globl _I2DAT
                            245 	.globl _I2CS
                            246 	.globl _PORTECFG
                            247 	.globl _PORTCCFG
                            248 	.globl _PORTACFG
                            249 	.globl _INTSETUP
                            250 	.globl _INT4IVEC
                            251 	.globl _INT2IVEC
                            252 	.globl _CLRERRCNT
                            253 	.globl _ERRCNTLIM
                            254 	.globl _USBERRIRQ
                            255 	.globl _USBERRIE
                            256 	.globl _GPIFIRQ
                            257 	.globl _GPIFIE
                            258 	.globl _EPIRQ
                            259 	.globl _EPIE
                            260 	.globl _USBIRQ
                            261 	.globl _USBIE
                            262 	.globl _NAKIRQ
                            263 	.globl _NAKIE
                            264 	.globl _IBNIRQ
                            265 	.globl _IBNIE
                            266 	.globl _EP8FIFOIRQ
                            267 	.globl _EP8FIFOIE
                            268 	.globl _EP6FIFOIRQ
                            269 	.globl _EP6FIFOIE
                            270 	.globl _EP4FIFOIRQ
                            271 	.globl _EP4FIFOIE
                            272 	.globl _EP2FIFOIRQ
                            273 	.globl _EP2FIFOIE
                            274 	.globl _OUTPKTEND
                            275 	.globl _INPKTEND
                            276 	.globl _EP8ISOINPKTS
                            277 	.globl _EP6ISOINPKTS
                            278 	.globl _EP4ISOINPKTS
                            279 	.globl _EP2ISOINPKTS
                            280 	.globl _EP8FIFOPFL
                            281 	.globl _EP8FIFOPFH
                            282 	.globl _EP6FIFOPFL
                            283 	.globl _EP6FIFOPFH
                            284 	.globl _EP4FIFOPFL
                            285 	.globl _EP4FIFOPFH
                            286 	.globl _EP2FIFOPFL
                            287 	.globl _EP2FIFOPFH
                            288 	.globl _EP8AUTOINLENL
                            289 	.globl _EP8AUTOINLENH
                            290 	.globl _EP6AUTOINLENL
                            291 	.globl _EP6AUTOINLENH
                            292 	.globl _EP4AUTOINLENL
                            293 	.globl _EP4AUTOINLENH
                            294 	.globl _EP2AUTOINLENL
                            295 	.globl _EP2AUTOINLENH
                            296 	.globl _EP8FIFOCFG
                            297 	.globl _EP6FIFOCFG
                            298 	.globl _EP4FIFOCFG
                            299 	.globl _EP2FIFOCFG
                            300 	.globl _EP8CFG
                            301 	.globl _EP6CFG
                            302 	.globl _EP4CFG
                            303 	.globl _EP2CFG
                            304 	.globl _EP1INCFG
                            305 	.globl _EP1OUTCFG
                            306 	.globl _REVCTL
                            307 	.globl _REVID
                            308 	.globl _FIFOPINPOLAR
                            309 	.globl _UART230
                            310 	.globl _BPADDRL
                            311 	.globl _BPADDRH
                            312 	.globl _BREAKPT
                            313 	.globl _FIFORESET
                            314 	.globl _PINFLAGSCD
                            315 	.globl _PINFLAGSAB
                            316 	.globl _IFCONFIG
                            317 	.globl _CPUCS
                            318 	.globl _RES_WAVEDATA_END
                            319 	.globl _GPIF_WAVE_DATA
                            320 	.globl __usb_got_SUDAV
                            321 	.globl _other_config_descr
                            322 	.globl _current_config_descr
                            323 	.globl _current_devqual_descr
                            324 	.globl _current_device_descr
                            325 	.globl __usb_alt_setting
                            326 	.globl __usb_config
                            327 	.globl _usb_install_handlers
                            328 	.globl _usb_handle_set_feature
                            329 	.globl _usb_handle_clear_feature
                            330 	.globl _usb_handle_get_status
                            331 	.globl _usb_handle_get_descriptor
                            332 	.globl _usb_handle_IN_std_requests
                            333 	.globl _usb_handle_OUT_std_requests
                            334 	.globl _usb_handle_std_requests
                            335 	.globl _usb_handle_setup_packet
                            336 ;--------------------------------------------------------
                            337 ; special function registers
                            338 ;--------------------------------------------------------
                            339 	.area RSEG    (DATA)
                    0080    340 _IOA	=	0x0080
                    0081    341 _SP	=	0x0081
                    0082    342 _DPL	=	0x0082
                    0083    343 _DPH	=	0x0083
                    0084    344 _DPL1	=	0x0084
                    0085    345 _DPH1	=	0x0085
                    0086    346 _DPS	=	0x0086
                    0087    347 _PCON	=	0x0087
                    0088    348 _TCON	=	0x0088
                    0089    349 _TMOD	=	0x0089
                    008A    350 _TL0	=	0x008a
                    008B    351 _TL1	=	0x008b
                    008C    352 _TH0	=	0x008c
                    008D    353 _TH1	=	0x008d
                    008E    354 _CKCON	=	0x008e
                    0090    355 _IOB	=	0x0090
                    0091    356 _EXIF	=	0x0091
                    0092    357 _MPAGE	=	0x0092
                    0098    358 _SCON0	=	0x0098
                    0099    359 _SBUF0	=	0x0099
                    009A    360 _APTR1H	=	0x009a
                    009B    361 _APTR1L	=	0x009b
                    009C    362 _AUTODAT1	=	0x009c
                    009D    363 _AUTOPTRH2	=	0x009d
                    009E    364 _AUTOPTRL2	=	0x009e
                    009F    365 _AUTODAT2	=	0x009f
                    00A0    366 _IOC	=	0x00a0
                    00A1    367 _INT2CLR	=	0x00a1
                    00A2    368 _INT4CLR	=	0x00a2
                    00A8    369 _IE	=	0x00a8
                    00AA    370 _EP2468STAT	=	0x00aa
                    00AB    371 _EP24FIFOFLGS	=	0x00ab
                    00AC    372 _EP68FIFOFLGS	=	0x00ac
                    00AF    373 _AUTOPTRSETUP	=	0x00af
                    00B0    374 _IOD	=	0x00b0
                    00B1    375 _IOE	=	0x00b1
                    00B2    376 _OEA	=	0x00b2
                    00B3    377 _OEB	=	0x00b3
                    00B4    378 _OEC	=	0x00b4
                    00B5    379 _OED	=	0x00b5
                    00B6    380 _OEE	=	0x00b6
                    00B8    381 _IP	=	0x00b8
                    00BA    382 _EP01STAT	=	0x00ba
                    00BB    383 _GPIFTRIG	=	0x00bb
                    00BD    384 _GPIFSGLDATH	=	0x00bd
                    00BE    385 _GPIFSGLDATLX	=	0x00be
                    00BF    386 _GPIFSGLDATLNOX	=	0x00bf
                    00C0    387 _SCON1	=	0x00c0
                    00C1    388 _SBUF1	=	0x00c1
                    00C8    389 _T2CON	=	0x00c8
                    00CA    390 _RCAP2L	=	0x00ca
                    00CB    391 _RCAP2H	=	0x00cb
                    00CC    392 _TL2	=	0x00cc
                    00CD    393 _TH2	=	0x00cd
                    00D0    394 _PSW	=	0x00d0
                    00D8    395 _EICON	=	0x00d8
                    00E0    396 _ACC	=	0x00e0
                    00E8    397 _EIE	=	0x00e8
                    00F0    398 _B	=	0x00f0
                    00F8    399 _EIP	=	0x00f8
                            400 ;--------------------------------------------------------
                            401 ; special function bits 
                            402 ;--------------------------------------------------------
                            403 	.area RSEG    (DATA)
                    0086    404 _SEL	=	0x0086
                    0088    405 _IT0	=	0x0088
                    0089    406 _IE0	=	0x0089
                    008A    407 _IT1	=	0x008a
                    008B    408 _IE1	=	0x008b
                    008C    409 _TR0	=	0x008c
                    008D    410 _TF0	=	0x008d
                    008E    411 _TR1	=	0x008e
                    008F    412 _TF1	=	0x008f
                    0098    413 _RI	=	0x0098
                    0099    414 _TI	=	0x0099
                    009A    415 _RB8	=	0x009a
                    009B    416 _TB8	=	0x009b
                    009C    417 _REN	=	0x009c
                    009D    418 _SM2	=	0x009d
                    009E    419 _SM1	=	0x009e
                    009F    420 _SM0	=	0x009f
                    00A8    421 _EX0	=	0x00a8
                    00A9    422 _ET0	=	0x00a9
                    00AA    423 _EX1	=	0x00aa
                    00AB    424 _ET1	=	0x00ab
                    00AC    425 _ES0	=	0x00ac
                    00AD    426 _ET2	=	0x00ad
                    00AE    427 _ES1	=	0x00ae
                    00AF    428 _EA	=	0x00af
                    00B8    429 _PX0	=	0x00b8
                    00B9    430 _PT0	=	0x00b9
                    00BA    431 _PX1	=	0x00ba
                    00BB    432 _PT1	=	0x00bb
                    00BC    433 _PS0	=	0x00bc
                    00BD    434 _PT2	=	0x00bd
                    00BE    435 _PS1	=	0x00be
                    00C0    436 _RI1	=	0x00c0
                    00C1    437 _TI1	=	0x00c1
                    00C2    438 _RB81	=	0x00c2
                    00C3    439 _TB81	=	0x00c3
                    00C4    440 _REN1	=	0x00c4
                    00C5    441 _SM21	=	0x00c5
                    00C6    442 _SM11	=	0x00c6
                    00C7    443 _SM01	=	0x00c7
                    00C8    444 _CP_RL2	=	0x00c8
                    00C9    445 _C_T2	=	0x00c9
                    00CA    446 _TR2	=	0x00ca
                    00CB    447 _EXEN2	=	0x00cb
                    00CC    448 _TCLK	=	0x00cc
                    00CD    449 _RCLK	=	0x00cd
                    00CE    450 _EXF2	=	0x00ce
                    00CF    451 _TF2	=	0x00cf
                    00D0    452 _P	=	0x00d0
                    00D1    453 _FL	=	0x00d1
                    00D2    454 _OV	=	0x00d2
                    00D3    455 _RS0	=	0x00d3
                    00D4    456 _RS1	=	0x00d4
                    00D5    457 _F0	=	0x00d5
                    00D6    458 _AC	=	0x00d6
                    00D7    459 _CY	=	0x00d7
                    00DB    460 _INT6	=	0x00db
                    00DC    461 _RESI	=	0x00dc
                    00DD    462 _ERESI	=	0x00dd
                    00DF    463 _SMOD1	=	0x00df
                    00E8    464 _EIUSB	=	0x00e8
                    00E9    465 _EI2C	=	0x00e9
                    00EA    466 _EIEX4	=	0x00ea
                    00EB    467 _EIEX5	=	0x00eb
                    00EC    468 _EIEX6	=	0x00ec
                    00F8    469 _PUSB	=	0x00f8
                    00F9    470 _PI2C	=	0x00f9
                    00FA    471 _EIPX4	=	0x00fa
                    00FB    472 _EIPX5	=	0x00fb
                    00FC    473 _EIPX6	=	0x00fc
                            474 ;--------------------------------------------------------
                            475 ; overlayable register banks 
                            476 ;--------------------------------------------------------
                            477 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                     478 	.ds 8
                            479 ;--------------------------------------------------------
                            480 ; internal ram data
                            481 ;--------------------------------------------------------
                            482 	.area DSEG    (DATA)
   0021                     483 __usb_config::
   0021                     484 	.ds 1
   0022                     485 __usb_alt_setting::
   0022                     486 	.ds 1
   0023                     487 _current_device_descr::
   0023                     488 	.ds 2
   0025                     489 _current_devqual_descr::
   0025                     490 	.ds 2
   0027                     491 _current_config_descr::
   0027                     492 	.ds 2
   0029                     493 _other_config_descr::
   0029                     494 	.ds 2
                            495 ;--------------------------------------------------------
                            496 ; overlayable items in internal ram 
                            497 ;--------------------------------------------------------
                            498 	.area	OSEG    (OVR,DATA)
                            499 	.area	OSEG    (OVR,DATA)
                            500 ;--------------------------------------------------------
                            501 ; indirectly addressable internal ram data
                            502 ;--------------------------------------------------------
                            503 	.area ISEG    (DATA)
                            504 ;--------------------------------------------------------
                            505 ; bit data
                            506 ;--------------------------------------------------------
                            507 	.area BSEG    (BIT)
   0003                     508 __usb_got_SUDAV::
   0003                     509 	.ds 1
                            510 ;--------------------------------------------------------
                            511 ; paged external ram data
                            512 ;--------------------------------------------------------
                            513 	.area PSEG    (PAG,XDATA)
                            514 ;--------------------------------------------------------
                            515 ; external ram data
                            516 ;--------------------------------------------------------
                            517 	.area XSEG    (XDATA)
                    E400    518 _GPIF_WAVE_DATA	=	0xe400
                    E480    519 _RES_WAVEDATA_END	=	0xe480
                    E600    520 _CPUCS	=	0xe600
                    E601    521 _IFCONFIG	=	0xe601
                    E602    522 _PINFLAGSAB	=	0xe602
                    E603    523 _PINFLAGSCD	=	0xe603
                    E604    524 _FIFORESET	=	0xe604
                    E605    525 _BREAKPT	=	0xe605
                    E606    526 _BPADDRH	=	0xe606
                    E607    527 _BPADDRL	=	0xe607
                    E608    528 _UART230	=	0xe608
                    E609    529 _FIFOPINPOLAR	=	0xe609
                    E60A    530 _REVID	=	0xe60a
                    E60B    531 _REVCTL	=	0xe60b
                    E610    532 _EP1OUTCFG	=	0xe610
                    E611    533 _EP1INCFG	=	0xe611
                    E612    534 _EP2CFG	=	0xe612
                    E613    535 _EP4CFG	=	0xe613
                    E614    536 _EP6CFG	=	0xe614
                    E615    537 _EP8CFG	=	0xe615
                    E618    538 _EP2FIFOCFG	=	0xe618
                    E619    539 _EP4FIFOCFG	=	0xe619
                    E61A    540 _EP6FIFOCFG	=	0xe61a
                    E61B    541 _EP8FIFOCFG	=	0xe61b
                    E620    542 _EP2AUTOINLENH	=	0xe620
                    E621    543 _EP2AUTOINLENL	=	0xe621
                    E622    544 _EP4AUTOINLENH	=	0xe622
                    E623    545 _EP4AUTOINLENL	=	0xe623
                    E624    546 _EP6AUTOINLENH	=	0xe624
                    E625    547 _EP6AUTOINLENL	=	0xe625
                    E626    548 _EP8AUTOINLENH	=	0xe626
                    E627    549 _EP8AUTOINLENL	=	0xe627
                    E630    550 _EP2FIFOPFH	=	0xe630
                    E631    551 _EP2FIFOPFL	=	0xe631
                    E632    552 _EP4FIFOPFH	=	0xe632
                    E633    553 _EP4FIFOPFL	=	0xe633
                    E634    554 _EP6FIFOPFH	=	0xe634
                    E635    555 _EP6FIFOPFL	=	0xe635
                    E636    556 _EP8FIFOPFH	=	0xe636
                    E637    557 _EP8FIFOPFL	=	0xe637
                    E640    558 _EP2ISOINPKTS	=	0xe640
                    E641    559 _EP4ISOINPKTS	=	0xe641
                    E642    560 _EP6ISOINPKTS	=	0xe642
                    E643    561 _EP8ISOINPKTS	=	0xe643
                    E648    562 _INPKTEND	=	0xe648
                    E649    563 _OUTPKTEND	=	0xe649
                    E650    564 _EP2FIFOIE	=	0xe650
                    E651    565 _EP2FIFOIRQ	=	0xe651
                    E652    566 _EP4FIFOIE	=	0xe652
                    E653    567 _EP4FIFOIRQ	=	0xe653
                    E654    568 _EP6FIFOIE	=	0xe654
                    E655    569 _EP6FIFOIRQ	=	0xe655
                    E656    570 _EP8FIFOIE	=	0xe656
                    E657    571 _EP8FIFOIRQ	=	0xe657
                    E658    572 _IBNIE	=	0xe658
                    E659    573 _IBNIRQ	=	0xe659
                    E65A    574 _NAKIE	=	0xe65a
                    E65B    575 _NAKIRQ	=	0xe65b
                    E65C    576 _USBIE	=	0xe65c
                    E65D    577 _USBIRQ	=	0xe65d
                    E65E    578 _EPIE	=	0xe65e
                    E65F    579 _EPIRQ	=	0xe65f
                    E660    580 _GPIFIE	=	0xe660
                    E661    581 _GPIFIRQ	=	0xe661
                    E662    582 _USBERRIE	=	0xe662
                    E663    583 _USBERRIRQ	=	0xe663
                    E664    584 _ERRCNTLIM	=	0xe664
                    E665    585 _CLRERRCNT	=	0xe665
                    E666    586 _INT2IVEC	=	0xe666
                    E667    587 _INT4IVEC	=	0xe667
                    E668    588 _INTSETUP	=	0xe668
                    E670    589 _PORTACFG	=	0xe670
                    E671    590 _PORTCCFG	=	0xe671
                    E672    591 _PORTECFG	=	0xe672
                    E678    592 _I2CS	=	0xe678
                    E679    593 _I2DAT	=	0xe679
                    E67A    594 _I2CTL	=	0xe67a
                    E67B    595 _XAUTODAT1	=	0xe67b
                    E67C    596 _XAUTODAT2	=	0xe67c
                    E680    597 _USBCS	=	0xe680
                    E681    598 _SUSPEND	=	0xe681
                    E682    599 _WAKEUPCS	=	0xe682
                    E683    600 _TOGCTL	=	0xe683
                    E684    601 _USBFRAMEH	=	0xe684
                    E685    602 _USBFRAMEL	=	0xe685
                    E686    603 _MICROFRAME	=	0xe686
                    E687    604 _FNADDR	=	0xe687
                    E68A    605 _EP0BCH	=	0xe68a
                    E68B    606 _EP0BCL	=	0xe68b
                    E68D    607 _EP1OUTBC	=	0xe68d
                    E68F    608 _EP1INBC	=	0xe68f
                    E690    609 _EP2BCH	=	0xe690
                    E691    610 _EP2BCL	=	0xe691
                    E694    611 _EP4BCH	=	0xe694
                    E695    612 _EP4BCL	=	0xe695
                    E698    613 _EP6BCH	=	0xe698
                    E699    614 _EP6BCL	=	0xe699
                    E69C    615 _EP8BCH	=	0xe69c
                    E69D    616 _EP8BCL	=	0xe69d
                    E6A0    617 _EP0CS	=	0xe6a0
                    E6A1    618 _EP1OUTCS	=	0xe6a1
                    E6A2    619 _EP1INCS	=	0xe6a2
                    E6A3    620 _EP2CS	=	0xe6a3
                    E6A4    621 _EP4CS	=	0xe6a4
                    E6A5    622 _EP6CS	=	0xe6a5
                    E6A6    623 _EP8CS	=	0xe6a6
                    E6A7    624 _EP2FIFOFLGS	=	0xe6a7
                    E6A8    625 _EP4FIFOFLGS	=	0xe6a8
                    E6A9    626 _EP6FIFOFLGS	=	0xe6a9
                    E6AA    627 _EP8FIFOFLGS	=	0xe6aa
                    E6AB    628 _EP2FIFOBCH	=	0xe6ab
                    E6AC    629 _EP2FIFOBCL	=	0xe6ac
                    E6AD    630 _EP4FIFOBCH	=	0xe6ad
                    E6AE    631 _EP4FIFOBCL	=	0xe6ae
                    E6AF    632 _EP6FIFOBCH	=	0xe6af
                    E6B0    633 _EP6FIFOBCL	=	0xe6b0
                    E6B1    634 _EP8FIFOBCH	=	0xe6b1
                    E6B2    635 _EP8FIFOBCL	=	0xe6b2
                    E6B3    636 _SUDPTRH	=	0xe6b3
                    E6B4    637 _SUDPTRL	=	0xe6b4
                    E6B5    638 _SUDPTRCTL	=	0xe6b5
                    E6B8    639 _SETUPDAT	=	0xe6b8
                    E6C0    640 _GPIFWFSELECT	=	0xe6c0
                    E6C1    641 _GPIFIDLECS	=	0xe6c1
                    E6C2    642 _GPIFIDLECTL	=	0xe6c2
                    E6C3    643 _GPIFCTLCFG	=	0xe6c3
                    E6C4    644 _GPIFADRH	=	0xe6c4
                    E6C5    645 _GPIFADRL	=	0xe6c5
                    E6CE    646 _GPIFTCB3	=	0xe6ce
                    E6CF    647 _GPIFTCB2	=	0xe6cf
                    E6D0    648 _GPIFTCB1	=	0xe6d0
                    E6D1    649 _GPIFTCB0	=	0xe6d1
                    E6D2    650 _EP2GPIFFLGSEL	=	0xe6d2
                    E6D3    651 _EP2GPIFPFSTOP	=	0xe6d3
                    E6D4    652 _EP2GPIFTRIG	=	0xe6d4
                    E6DA    653 _EP4GPIFFLGSEL	=	0xe6da
                    E6DB    654 _EP4GPIFPFSTOP	=	0xe6db
                    E6DC    655 _EP4GPIFTRIG	=	0xe6dc
                    E6E2    656 _EP6GPIFFLGSEL	=	0xe6e2
                    E6E3    657 _EP6GPIFPFSTOP	=	0xe6e3
                    E6E4    658 _EP6GPIFTRIG	=	0xe6e4
                    E6EA    659 _EP8GPIFFLGSEL	=	0xe6ea
                    E6EB    660 _EP8GPIFPFSTOP	=	0xe6eb
                    E6EC    661 _EP8GPIFTRIG	=	0xe6ec
                    E6F0    662 _XGPIFSGLDATH	=	0xe6f0
                    E6F1    663 _XGPIFSGLDATLX	=	0xe6f1
                    E6F2    664 _XGPIFSGLDATLNOX	=	0xe6f2
                    E6F3    665 _GPIFREADYCFG	=	0xe6f3
                    E6F4    666 _GPIFREADYSTAT	=	0xe6f4
                    E6F5    667 _GPIFABORT	=	0xe6f5
                    E6C6    668 _FLOWSTATE	=	0xe6c6
                    E6C7    669 _FLOWLOGIC	=	0xe6c7
                    E6C8    670 _FLOWEQ0CTL	=	0xe6c8
                    E6C9    671 _FLOWEQ1CTL	=	0xe6c9
                    E6CA    672 _FLOWHOLDOFF	=	0xe6ca
                    E6CB    673 _FLOWSTB	=	0xe6cb
                    E6CC    674 _FLOWSTBEDGE	=	0xe6cc
                    E6CD    675 _FLOWSTBHPERIOD	=	0xe6cd
                    E60C    676 _GPIFHOLDAMOUNT	=	0xe60c
                    E67D    677 _UDMACRCH	=	0xe67d
                    E67E    678 _UDMACRCL	=	0xe67e
                    E67F    679 _UDMACRCQUAL	=	0xe67f
                    E6F8    680 _DBUG	=	0xe6f8
                    E6F9    681 _TESTCFG	=	0xe6f9
                    E6FA    682 _USBTEST	=	0xe6fa
                    E6FB    683 _CT1	=	0xe6fb
                    E6FC    684 _CT2	=	0xe6fc
                    E6FD    685 _CT3	=	0xe6fd
                    E6FE    686 _CT4	=	0xe6fe
                    E740    687 _EP0BUF	=	0xe740
                    E780    688 _EP1OUTBUF	=	0xe780
                    E7C0    689 _EP1INBUF	=	0xe7c0
                    F000    690 _EP2FIFOBUF	=	0xf000
                    F400    691 _EP4FIFOBUF	=	0xf400
                    F800    692 _EP6FIFOBUF	=	0xf800
                    FC00    693 _EP8FIFOBUF	=	0xfc00
                            694 ;--------------------------------------------------------
                            695 ; external initialized ram data
                            696 ;--------------------------------------------------------
                            697 	.area CSEG    (CODE)
                            698 	.area GSINIT0 (CODE)
                            699 	.area GSINIT1 (CODE)
                            700 	.area GSINIT2 (CODE)
                            701 	.area GSINIT3 (CODE)
                            702 	.area GSINIT4 (CODE)
                            703 	.area GSINIT5 (CODE)
                            704 ;--------------------------------------------------------
                            705 ; global & static initialisations
                            706 ;--------------------------------------------------------
                            707 	.area CSEG    (CODE)
                            708 	.area GSINIT  (CODE)
                            709 	.area GSFINAL (CODE)
                            710 	.area GSINIT  (CODE)
                            711 ;Initial/src/lib/usb_common.c:56: unsigned char	_usb_config = 0;
                            712 ;     genAssign
   104F 75 21 00            713 	mov	__usb_config,#0x00
                            714 ;Initial/src/lib/usb_common.c:57: unsigned char	_usb_alt_setting = 0;	// FIXME really 1/interface
                            715 ;     genAssign
   1052 75 22 00            716 	mov	__usb_alt_setting,#0x00
                            717 ;--------------------------------------------------------
                            718 ; Home
                            719 ;--------------------------------------------------------
                            720 	.area HOME    (CODE)
                            721 	.area CSEG    (CODE)
                            722 ;--------------------------------------------------------
                            723 ; code
                            724 ;--------------------------------------------------------
                            725 	.area CSEG    (CODE)
                            726 ;------------------------------------------------------------
                            727 ;Allocation info for local variables in function 'setup_descriptors'
                            728 ;------------------------------------------------------------
                            729 ;------------------------------------------------------------
                            730 ;Initial/src/lib/usb_common.c:65: setup_descriptors (void)
                            731 ;	-----------------------------------------
                            732 ;	 function setup_descriptors
                            733 ;	-----------------------------------------
   0BF3                     734 _setup_descriptors:
                    0002    735 	ar2 = 0x02
                    0003    736 	ar3 = 0x03
                    0004    737 	ar4 = 0x04
                    0005    738 	ar5 = 0x05
                    0006    739 	ar6 = 0x06
                    0007    740 	ar7 = 0x07
                    0000    741 	ar0 = 0x00
                    0001    742 	ar1 = 0x01
                            743 ;Initial/src/lib/usb_common.c:67: if (USBCS & bmHSM)
                            744 ;     genAssign
   0BF3 90 E6 80            745 	mov	dptr,#_USBCS
   0BF6 E0                  746 	movx	a,@dptr
                            747 ;     genAnd
                            748 ;	Peephole 105	removed redundant mov
   0BF7 FA                  749 	mov	r2,a
                            750 ;     genIfxJump
                            751 ;	Peephole 111	removed ljmp by inverse jump logic
   0BF8 30 E7 19            752 	jnb	acc.7,00102$
   0BFB                     753 00107$:
                            754 ;Initial/src/lib/usb_common.c:69: current_device_descr  = high_speed_device_descr;
                            755 ;     genAddrOf
   0BFB 75 23 00            756 	mov	_current_device_descr,#_high_speed_device_descr
   0BFE 75 24 E0            757 	mov	(_current_device_descr + 1),#(_high_speed_device_descr >> 8)
                            758 ;Initial/src/lib/usb_common.c:70: current_devqual_descr = high_speed_devqual_descr;
                            759 ;     genAddrOf
   0C01 75 25 12            760 	mov	_current_devqual_descr,#_high_speed_devqual_descr
   0C04 75 26 E0            761 	mov	(_current_devqual_descr + 1),#(_high_speed_devqual_descr >> 8)
                            762 ;Initial/src/lib/usb_common.c:71: current_config_descr  = high_speed_config_descr;
                            763 ;     genAddrOf
   0C07 75 27 1C            764 	mov	_current_config_descr,#_high_speed_config_descr
   0C0A 75 28 E0            765 	mov	(_current_config_descr + 1),#(_high_speed_config_descr >> 8)
                            766 ;Initial/src/lib/usb_common.c:72: other_config_descr    = full_speed_config_descr;
                            767 ;     genAddrOf
   0C0D 75 29 66            768 	mov	_other_config_descr,#_full_speed_config_descr
   0C10 75 2A E0            769 	mov	(_other_config_descr + 1),#(_full_speed_config_descr >> 8)
                            770 ;	Peephole 112.b	changed ljmp to sjmp
                            771 ;	Peephole 251.b	replaced sjmp to ret with ret
   0C13 22                  772 	ret
   0C14                     773 00102$:
                            774 ;Initial/src/lib/usb_common.c:76: current_device_descr  = full_speed_device_descr;
                            775 ;     genAddrOf
   0C14 75 23 4A            776 	mov	_current_device_descr,#_full_speed_device_descr
   0C17 75 24 E0            777 	mov	(_current_device_descr + 1),#(_full_speed_device_descr >> 8)
                            778 ;Initial/src/lib/usb_common.c:77: current_devqual_descr = full_speed_devqual_descr;
                            779 ;     genAddrOf
   0C1A 75 25 5C            780 	mov	_current_devqual_descr,#_full_speed_devqual_descr
   0C1D 75 26 E0            781 	mov	(_current_devqual_descr + 1),#(_full_speed_devqual_descr >> 8)
                            782 ;Initial/src/lib/usb_common.c:78: current_config_descr  = full_speed_config_descr;
                            783 ;     genAddrOf
   0C20 75 27 66            784 	mov	_current_config_descr,#_full_speed_config_descr
   0C23 75 28 E0            785 	mov	(_current_config_descr + 1),#(_full_speed_config_descr >> 8)
                            786 ;Initial/src/lib/usb_common.c:79: other_config_descr    = high_speed_config_descr;
                            787 ;     genAddrOf
   0C26 75 29 1C            788 	mov	_other_config_descr,#_high_speed_config_descr
   0C29 75 2A E0            789 	mov	(_other_config_descr + 1),#(_high_speed_config_descr >> 8)
   0C2C                     790 00104$:
   0C2C 22                  791 	ret
                            792 ;------------------------------------------------------------
                            793 ;Allocation info for local variables in function 'isr_SUDAV'
                            794 ;------------------------------------------------------------
                            795 ;------------------------------------------------------------
                            796 ;Initial/src/lib/usb_common.c:84: isr_SUDAV (void) interrupt
                            797 ;	-----------------------------------------
                            798 ;	 function isr_SUDAV
                            799 ;	-----------------------------------------
   0C2D                     800 _isr_SUDAV:
                            801 ;Initial/src/lib/usb_common.c:86: clear_usb_irq ();
                            802 ;     genAnd
   0C2D 53 91 EF            803 	anl	_EXIF,#0xEF
                            804 ;     genAssign
   0C30 75 A1 00            805 	mov	_INT2CLR,#0x00
                            806 ;Initial/src/lib/usb_common.c:87: _usb_got_SUDAV = 1;
                            807 ;     genAssign
   0C33 D2 03               808 	setb	__usb_got_SUDAV
   0C35                     809 00101$:
   0C35 32                  810 	reti
                            811 ;	eliminated unneeded push/pop psw
                            812 ;	eliminated unneeded push/pop dpl
                            813 ;	eliminated unneeded push/pop dph
                            814 ;	eliminated unneeded push/pop b
                            815 ;	eliminated unneeded push/pop acc
                            816 ;------------------------------------------------------------
                            817 ;Allocation info for local variables in function 'isr_USBRESET'
                            818 ;------------------------------------------------------------
                            819 ;------------------------------------------------------------
                            820 ;Initial/src/lib/usb_common.c:91: isr_USBRESET (void) interrupt
                            821 ;	-----------------------------------------
                            822 ;	 function isr_USBRESET
                            823 ;	-----------------------------------------
   0C36                     824 _isr_USBRESET:
   0C36 C0 E0               825 	push	acc
   0C38 C0 F0               826 	push	b
   0C3A C0 82               827 	push	dpl
   0C3C C0 83               828 	push	dph
   0C3E C0 02               829 	push	(0+2)
   0C40 C0 03               830 	push	(0+3)
   0C42 C0 04               831 	push	(0+4)
   0C44 C0 05               832 	push	(0+5)
   0C46 C0 06               833 	push	(0+6)
   0C48 C0 07               834 	push	(0+7)
   0C4A C0 00               835 	push	(0+0)
   0C4C C0 01               836 	push	(0+1)
   0C4E C0 D0               837 	push	psw
   0C50 75 D0 00            838 	mov	psw,#0x00
                            839 ;Initial/src/lib/usb_common.c:93: clear_usb_irq ();
                            840 ;     genAnd
   0C53 53 91 EF            841 	anl	_EXIF,#0xEF
                            842 ;     genAssign
   0C56 75 A1 00            843 	mov	_INT2CLR,#0x00
                            844 ;Initial/src/lib/usb_common.c:94: setup_descriptors ();
                            845 ;     genCall
   0C59 12 0B F3            846 	lcall	_setup_descriptors
   0C5C                     847 00101$:
   0C5C D0 D0               848 	pop	psw
   0C5E D0 01               849 	pop	(0+1)
   0C60 D0 00               850 	pop	(0+0)
   0C62 D0 07               851 	pop	(0+7)
   0C64 D0 06               852 	pop	(0+6)
   0C66 D0 05               853 	pop	(0+5)
   0C68 D0 04               854 	pop	(0+4)
   0C6A D0 03               855 	pop	(0+3)
   0C6C D0 02               856 	pop	(0+2)
   0C6E D0 83               857 	pop	dph
   0C70 D0 82               858 	pop	dpl
   0C72 D0 F0               859 	pop	b
   0C74 D0 E0               860 	pop	acc
   0C76 32                  861 	reti
                            862 ;------------------------------------------------------------
                            863 ;Allocation info for local variables in function 'isr_HIGHSPEED'
                            864 ;------------------------------------------------------------
                            865 ;------------------------------------------------------------
                            866 ;Initial/src/lib/usb_common.c:98: isr_HIGHSPEED (void) interrupt
                            867 ;	-----------------------------------------
                            868 ;	 function isr_HIGHSPEED
                            869 ;	-----------------------------------------
   0C77                     870 _isr_HIGHSPEED:
   0C77 C0 E0               871 	push	acc
   0C79 C0 F0               872 	push	b
   0C7B C0 82               873 	push	dpl
   0C7D C0 83               874 	push	dph
   0C7F C0 02               875 	push	(0+2)
   0C81 C0 03               876 	push	(0+3)
   0C83 C0 04               877 	push	(0+4)
   0C85 C0 05               878 	push	(0+5)
   0C87 C0 06               879 	push	(0+6)
   0C89 C0 07               880 	push	(0+7)
   0C8B C0 00               881 	push	(0+0)
   0C8D C0 01               882 	push	(0+1)
   0C8F C0 D0               883 	push	psw
   0C91 75 D0 00            884 	mov	psw,#0x00
                            885 ;Initial/src/lib/usb_common.c:100: clear_usb_irq ();
                            886 ;     genAnd
   0C94 53 91 EF            887 	anl	_EXIF,#0xEF
                            888 ;     genAssign
   0C97 75 A1 00            889 	mov	_INT2CLR,#0x00
                            890 ;Initial/src/lib/usb_common.c:101: setup_descriptors ();
                            891 ;     genCall
   0C9A 12 0B F3            892 	lcall	_setup_descriptors
   0C9D                     893 00101$:
   0C9D D0 D0               894 	pop	psw
   0C9F D0 01               895 	pop	(0+1)
   0CA1 D0 00               896 	pop	(0+0)
   0CA3 D0 07               897 	pop	(0+7)
   0CA5 D0 06               898 	pop	(0+6)
   0CA7 D0 05               899 	pop	(0+5)
   0CA9 D0 04               900 	pop	(0+4)
   0CAB D0 03               901 	pop	(0+3)
   0CAD D0 02               902 	pop	(0+2)
   0CAF D0 83               903 	pop	dph
   0CB1 D0 82               904 	pop	dpl
   0CB3 D0 F0               905 	pop	b
   0CB5 D0 E0               906 	pop	acc
   0CB7 32                  907 	reti
                            908 ;------------------------------------------------------------
                            909 ;Allocation info for local variables in function 'usb_install_handlers'
                            910 ;------------------------------------------------------------
                            911 ;------------------------------------------------------------
                            912 ;Initial/src/lib/usb_common.c:105: usb_install_handlers (void)
                            913 ;	-----------------------------------------
                            914 ;	 function usb_install_handlers
                            915 ;	-----------------------------------------
   0CB8                     916 _usb_install_handlers:
                            917 ;Initial/src/lib/usb_common.c:107: setup_descriptors ();	    // ensure that they're set before use
                            918 ;     genCall
   0CB8 12 0B F3            919 	lcall	_setup_descriptors
                            920 ;Initial/src/lib/usb_common.c:109: hook_uv (UV_SUDAV,     (unsigned short) isr_SUDAV);
                            921 ;     genCast
   0CBB 75 2B 2D            922 	mov	_hook_uv_PARM_2,#_isr_SUDAV
   0CBE 75 2C 0C            923 	mov	(_hook_uv_PARM_2 + 1),#(_isr_SUDAV >> 8)
                            924 ;     genCall
   0CC1 75 82 00            925 	mov	dpl,#0x00
   0CC4 12 08 9B            926 	lcall	_hook_uv
                            927 ;Initial/src/lib/usb_common.c:110: hook_uv (UV_USBRESET,  (unsigned short) isr_USBRESET);
                            928 ;     genCast
   0CC7 75 2B 36            929 	mov	_hook_uv_PARM_2,#_isr_USBRESET
   0CCA 75 2C 0C            930 	mov	(_hook_uv_PARM_2 + 1),#(_isr_USBRESET >> 8)
                            931 ;     genCall
   0CCD 75 82 10            932 	mov	dpl,#0x10
   0CD0 12 08 9B            933 	lcall	_hook_uv
                            934 ;Initial/src/lib/usb_common.c:111: hook_uv (UV_HIGHSPEED, (unsigned short) isr_HIGHSPEED);
                            935 ;     genCast
   0CD3 75 2B 77            936 	mov	_hook_uv_PARM_2,#_isr_HIGHSPEED
   0CD6 75 2C 0C            937 	mov	(_hook_uv_PARM_2 + 1),#(_isr_HIGHSPEED >> 8)
                            938 ;     genCall
   0CD9 75 82 14            939 	mov	dpl,#0x14
   0CDC 12 08 9B            940 	lcall	_hook_uv
                            941 ;Initial/src/lib/usb_common.c:113: USBIE = bmSUDAV | bmURES | bmHSGRANT;
                            942 ;     genAssign
   0CDF 90 E6 5C            943 	mov	dptr,#_USBIE
   0CE2 74 31               944 	mov	a,#0x31
   0CE4 F0                  945 	movx	@dptr,a
   0CE5                     946 00101$:
   0CE5 22                  947 	ret
                            948 ;------------------------------------------------------------
                            949 ;Allocation info for local variables in function 'plausible_endpoint'
                            950 ;------------------------------------------------------------
                            951 ;ep                        Allocated to registers r2 
                            952 ;------------------------------------------------------------
                            953 ;Initial/src/lib/usb_common.c:120: plausible_endpoint (unsigned char ep)
                            954 ;	-----------------------------------------
                            955 ;	 function plausible_endpoint
                            956 ;	-----------------------------------------
   0CE6                     957 _plausible_endpoint:
                            958 ;     genReceive
                            959 ;Initial/src/lib/usb_common.c:122: ep &= ~0x80;	// ignore direction bit
                            960 ;     genAnd
                            961 ;Initial/src/lib/usb_common.c:124: if (ep > 8)
                            962 ;     genCmpGt
                            963 ;     genCmp
                            964 ;     genIfxJump
                            965 ;	Peephole 108	removed ljmp by inverse jump logic
                            966 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
                            967 ;	Peephole 187	used a instead of ar2 for anl
   0CE6 E5 82               968 	mov	a,dpl
   0CE8 54 7F               969 	anl	a,#0x7F
   0CEA FA                  970 	mov	r2,a
   0CEB 24 F7               971 	add	a,#0xff - 0x08
   0CED 50 04               972 	jnc	00102$
   0CEF                     973 00109$:
                            974 ;Initial/src/lib/usb_common.c:125: return 0;
                            975 ;     genRet
   0CEF 75 82 00            976 	mov	dpl,#0x00
                            977 ;	Peephole 112.b	changed ljmp to sjmp
                            978 ;	Peephole 251.b	replaced sjmp to ret with ret
   0CF2 22                  979 	ret
   0CF3                     980 00102$:
                            981 ;Initial/src/lib/usb_common.c:127: if (ep == 1)
                            982 ;     genCmpEq
                            983 ;	Peephole 112.b	changed ljmp to sjmp
                            984 ;	Peephole 199	optimized misc jump sequence
   0CF3 BA 01 04            985 	cjne	r2,#0x01,00104$
                            986 ;00110$:
                            987 ;	Peephole 200	removed redundant sjmp
   0CF6                     988 00111$:
                            989 ;Initial/src/lib/usb_common.c:128: return 1;
                            990 ;     genRet
   0CF6 75 82 01            991 	mov	dpl,#0x01
                            992 ;	Peephole 112.b	changed ljmp to sjmp
                            993 ;	Peephole 251.b	replaced sjmp to ret with ret
   0CF9 22                  994 	ret
   0CFA                     995 00104$:
                            996 ;Initial/src/lib/usb_common.c:130: return (ep & 0x1) == 0;	// must be even
                            997 ;     genAnd
   0CFA 53 02 01            998 	anl	ar2,#0x01
                            999 ;     genCmpEq
                           1000 ;	Peephole 241.c	optimized compare
   0CFD E4                 1001 	clr	a
   0CFE BA 00 01           1002 	cjne	r2,#0x00,00112$
   0D01 04                 1003 	inc	a
   0D02                    1004 00112$:
   0D02                    1005 00113$:
   0D02 F5 82              1006 	mov	dpl,a
                           1007 ;     genRet
   0D04                    1008 00105$:
   0D04 22                 1009 	ret
                           1010 ;------------------------------------------------------------
                           1011 ;Allocation info for local variables in function 'epcs'
                           1012 ;------------------------------------------------------------
                           1013 ;ep                        Allocated to registers r2 
                           1014 ;------------------------------------------------------------
                           1015 ;Initial/src/lib/usb_common.c:137: epcs (unsigned char ep)
                           1016 ;	-----------------------------------------
                           1017 ;	 function epcs
                           1018 ;	-----------------------------------------
   0D05                    1019 _epcs:
                           1020 ;     genReceive
   0D05 AA 82              1021 	mov	r2,dpl
                           1022 ;Initial/src/lib/usb_common.c:139: if (ep == 0x01)		// ep1 has different in and out CS regs
                           1023 ;     genCmpEq
                           1024 ;	Peephole 112.b	changed ljmp to sjmp
                           1025 ;	Peephole 199	optimized misc jump sequence
   0D07 BA 01 0C           1026 	cjne	r2,#0x01,00102$
                           1027 ;00112$:
                           1028 ;	Peephole 200	removed redundant sjmp
   0D0A                    1029 00113$:
                           1030 ;Initial/src/lib/usb_common.c:140: return EP1OUTCS;
                           1031 ;     genAssign
   0D0A 90 E6 A1           1032 	mov	dptr,#_EP1OUTCS
   0D0D E0                 1033 	movx	a,@dptr
   0D0E FB                 1034 	mov	r3,a
                           1035 ;     genCast
   0D0F 7C 00              1036 	mov	r4,#0x00
                           1037 ;     genRet
   0D11 8B 82              1038 	mov	dpl,r3
   0D13 8C 83              1039 	mov	dph,r4
                           1040 ;	Peephole 112.b	changed ljmp to sjmp
                           1041 ;	Peephole 251.b	replaced sjmp to ret with ret
   0D15 22                 1042 	ret
   0D16                    1043 00102$:
                           1044 ;Initial/src/lib/usb_common.c:142: if (ep == 0x81)
                           1045 ;     genCmpEq
                           1046 ;	Peephole 112.b	changed ljmp to sjmp
                           1047 ;	Peephole 199	optimized misc jump sequence
   0D16 BA 81 0C           1048 	cjne	r2,#0x81,00104$
                           1049 ;00114$:
                           1050 ;	Peephole 200	removed redundant sjmp
   0D19                    1051 00115$:
                           1052 ;Initial/src/lib/usb_common.c:143: return EP1INCS;
                           1053 ;     genAssign
   0D19 90 E6 A2           1054 	mov	dptr,#_EP1INCS
   0D1C E0                 1055 	movx	a,@dptr
   0D1D FB                 1056 	mov	r3,a
                           1057 ;     genCast
   0D1E 7C 00              1058 	mov	r4,#0x00
                           1059 ;     genRet
   0D20 8B 82              1060 	mov	dpl,r3
   0D22 8C 83              1061 	mov	dph,r4
                           1062 ;	Peephole 112.b	changed ljmp to sjmp
                           1063 ;	Peephole 251.b	replaced sjmp to ret with ret
   0D24 22                 1064 	ret
   0D25                    1065 00104$:
                           1066 ;Initial/src/lib/usb_common.c:145: ep &= ~0x80;			// ignore direction bit
                           1067 ;     genAnd
   0D25 53 02 7F           1068 	anl	ar2,#0x7F
                           1069 ;Initial/src/lib/usb_common.c:147: if (ep == 0x00)		// ep0
                           1070 ;     genCmpEq
                           1071 ;	Peephole 112.b	changed ljmp to sjmp
                           1072 ;	Peephole 199	optimized misc jump sequence
   0D28 BA 00 0C           1073 	cjne	r2,#0x00,00106$
                           1074 ;00116$:
                           1075 ;	Peephole 200	removed redundant sjmp
   0D2B                    1076 00117$:
                           1077 ;Initial/src/lib/usb_common.c:148: return EP0CS;
                           1078 ;     genAssign
   0D2B 90 E6 A0           1079 	mov	dptr,#_EP0CS
   0D2E E0                 1080 	movx	a,@dptr
   0D2F FB                 1081 	mov	r3,a
                           1082 ;     genCast
   0D30 7C 00              1083 	mov	r4,#0x00
                           1084 ;     genRet
   0D32 8B 82              1085 	mov	dpl,r3
   0D34 8C 83              1086 	mov	dph,r4
                           1087 ;	Peephole 112.b	changed ljmp to sjmp
                           1088 ;	Peephole 251.b	replaced sjmp to ret with ret
   0D36 22                 1089 	ret
   0D37                    1090 00106$:
                           1091 ;Initial/src/lib/usb_common.c:150: return EP2CS + (ep >> 1);	// 2, 4, 6, 8 are consecutive
                           1092 ;     genAssign
   0D37 90 E6 A3           1093 	mov	dptr,#_EP2CS
   0D3A E0                 1094 	movx	a,@dptr
   0D3B FB                 1095 	mov	r3,a
                           1096 ;     genCast
   0D3C 7C 00              1097 	mov	r4,#0x00
                           1098 ;     genRightShift
                           1099 ;     genRightShiftLiteral
                           1100 ;     genrshOne
   0D3E EA                 1101 	mov	a,r2
   0D3F C3                 1102 	clr	c
   0D40 13                 1103 	rrc	a
   0D41 FA                 1104 	mov	r2,a
                           1105 ;     genCast
   0D42 7D 00              1106 	mov	r5,#0x00
                           1107 ;     genPlus
                           1108 ;	Peephole 236.g	used r2 instead of ar2
   0D44 EA                 1109 	mov	a,r2
                           1110 ;	Peephole 236.a	used r3 instead of ar3
   0D45 2B                 1111 	add	a,r3
   0D46 FB                 1112 	mov	r3,a
                           1113 ;	Peephole 236.g	used r5 instead of ar5
   0D47 ED                 1114 	mov	a,r5
                           1115 ;	Peephole 236.b	used r4 instead of ar4
   0D48 3C                 1116 	addc	a,r4
                           1117 ;     genCast
                           1118 ;     genRet
                           1119 ;	Peephole 235	loading dph directly from a(ccumulator), r4 not set
   0D49 8B 82              1120 	mov	dpl,r3
   0D4B F5 83              1121 	mov	dph,a
   0D4D                    1122 00107$:
   0D4D 22                 1123 	ret
                           1124 ;------------------------------------------------------------
                           1125 ;Allocation info for local variables in function 'usb_handle_set_feature'
                           1126 ;------------------------------------------------------------
                           1127 ;------------------------------------------------------------
                           1128 ;Initial/src/lib/usb_common.c:153: void usb_handle_set_feature(void)
                           1129 ;	-----------------------------------------
                           1130 ;	 function usb_handle_set_feature
                           1131 ;	-----------------------------------------
   0D4E                    1132 _usb_handle_set_feature:
                           1133 ;Initial/src/lib/usb_common.c:155: switch (bRequestType & bmRT_RECIP_MASK)
                           1134 ;     genPointerGet
                           1135 ;     genFarPointerGet
   0D4E 90 E6 B8           1136 	mov	dptr,#_SETUPDAT
   0D51 E0                 1137 	movx	a,@dptr
   0D52 FA                 1138 	mov	r2,a
                           1139 ;     genAnd
   0D53 53 02 1F           1140 	anl	ar2,#0x1F
                           1141 ;     genCmpEq
   0D56 BA 00 02           1142 	cjne	r2,#0x00,00121$
                           1143 ;	Peephole 112.b	changed ljmp to sjmp
   0D59 80 05              1144 	sjmp	00101$
   0D5B                    1145 00121$:
                           1146 ;     genCmpEq
                           1147 ;	Peephole 112.b	changed ljmp to sjmp
                           1148 ;Initial/src/lib/usb_common.c:157: case bmRT_RECIP_DEVICE:
                           1149 ;	Peephole 112.b	changed ljmp to sjmp
                           1150 ;	Peephole 199	optimized misc jump sequence
   0D5B BA 02 5B           1151 	cjne	r2,#0x02,00112$
   0D5E 80 12              1152 	sjmp	00106$
                           1153 ;00122$:
   0D60                    1154 00101$:
                           1155 ;Initial/src/lib/usb_common.c:158: switch (wValueL)
                           1156 ;     genPointerGet
                           1157 ;     genFarPointerGet
   0D60 90 E6 BA           1158 	mov	dptr,#(_SETUPDAT + 0x0002)
   0D63 E0                 1159 	movx	a,@dptr
   0D64 FA                 1160 	mov	r2,a
                           1161 ;     genCmpEq
   0D65 BA 01 02           1162 	cjne	r2,#0x01,00123$
                           1163 ;	Peephole 112.b	changed ljmp to sjmp
   0D68 80 05              1164 	sjmp	00104$
   0D6A                    1165 00123$:
                           1166 ;     genCmpEq
   0D6A BA 02 02           1167 	cjne	r2,#0x02,00124$
                           1168 ;	Peephole 112.b	changed ljmp to sjmp
   0D6D 80 03              1169 	sjmp	00106$
   0D6F                    1170 00124$:
                           1171 ;Initial/src/lib/usb_common.c:165: default:
   0D6F                    1172 00104$:
                           1173 ;Initial/src/lib/usb_common.c:166: fx2_stall_ep0 ();
                           1174 ;     genCall
   0D6F 12 06 85           1175 	lcall	_fx2_stall_ep0
                           1176 ;Initial/src/lib/usb_common.c:170: case bmRT_RECIP_ENDPOINT:
   0D72                    1177 00106$:
                           1178 ;Initial/src/lib/usb_common.c:171: switch (wValueL)
                           1179 ;     genPointerGet
                           1180 ;     genFarPointerGet
   0D72 90 E6 BA           1181 	mov	dptr,#(_SETUPDAT + 0x0002)
   0D75 E0                 1182 	movx	a,@dptr
                           1183 ;     genCmpEq
                           1184 ;	Peephole 115.b	jump optimization
   0D76 FA                 1185 	mov	r2,a
   0D77 60 02              1186 	jz	00126$
   0D79                    1187 00125$:
                           1188 ;	Peephole 112.b	changed ljmp to sjmp
   0D79 80 3E              1189 	sjmp	00112$
   0D7B                    1190 00126$:
                           1191 ;Initial/src/lib/usb_common.c:174: if (plausible_endpoint (wIndexL))
                           1192 ;     genPointerGet
                           1193 ;     genFarPointerGet
   0D7B 90 E6 BC           1194 	mov	dptr,#(_SETUPDAT + 0x0004)
   0D7E E0                 1195 	movx	a,@dptr
                           1196 ;     genCall
                           1197 ;	Peephole 244.c	loading dpl from a instead of r2
   0D7F FA                 1198 	mov	r2,a
   0D80 F5 82              1199 	mov	dpl,a
   0D82 12 0C E6           1200 	lcall	_plausible_endpoint
   0D85 E5 82              1201 	mov	a,dpl
                           1202 ;     genIfx
                           1203 ;     genIfxJump
                           1204 ;	Peephole 110	removed ljmp by inverse jump logic
   0D87 60 2D              1205 	jz	00109$
   0D89                    1206 00127$:
                           1207 ;Initial/src/lib/usb_common.c:175: *epcs (wIndexL) |= bmEPSTALL;
                           1208 ;     genPointerGet
                           1209 ;     genFarPointerGet
   0D89 90 E6 BC           1210 	mov	dptr,#(_SETUPDAT + 0x0004)
   0D8C E0                 1211 	movx	a,@dptr
                           1212 ;     genCall
                           1213 ;	Peephole 244.c	loading dpl from a instead of r2
   0D8D FA                 1214 	mov	r2,a
   0D8E F5 82              1215 	mov	dpl,a
   0D90 12 0D 05           1216 	lcall	_epcs
   0D93 AA 82              1217 	mov	r2,dpl
   0D95 AB 83              1218 	mov	r3,dph
                           1219 ;     genPointerGet
                           1220 ;     genFarPointerGet
   0D97 90 E6 BC           1221 	mov	dptr,#(_SETUPDAT + 0x0004)
   0D9A E0                 1222 	movx	a,@dptr
                           1223 ;     genCall
                           1224 ;	Peephole 244.c	loading dpl from a instead of r4
   0D9B FC                 1225 	mov	r4,a
   0D9C F5 82              1226 	mov	dpl,a
   0D9E C0 02              1227 	push	ar2
   0DA0 C0 03              1228 	push	ar3
   0DA2 12 0D 05           1229 	lcall	_epcs
   0DA5 D0 03              1230 	pop	ar3
   0DA7 D0 02              1231 	pop	ar2
                           1232 ;     genPointerGet
                           1233 ;     genFarPointerGet
   0DA9 E0                 1234 	movx	a,@dptr
   0DAA FC                 1235 	mov	r4,a
                           1236 ;     genOr
   0DAB 43 04 01           1237 	orl	ar4,#0x01
                           1238 ;     genPointerSet
                           1239 ;     genFarPointerSet
   0DAE 8A 82              1240 	mov	dpl,r2
   0DB0 8B 83              1241 	mov	dph,r3
   0DB2 EC                 1242 	mov	a,r4
   0DB3 F0                 1243 	movx	@dptr,a
                           1244 ;	Peephole 112.b	changed ljmp to sjmp
   0DB4 80 03              1245 	sjmp	00112$
   0DB6                    1246 00109$:
                           1247 ;Initial/src/lib/usb_common.c:177: fx2_stall_ep0 ();
                           1248 ;     genCall
   0DB6 12 06 85           1249 	lcall	_fx2_stall_ep0
                           1250 ;Initial/src/lib/usb_common.c:181: default:
   0DB9                    1251 00112$:
                           1252 ;Initial/src/lib/usb_common.c:182: fx2_stall_ep0 ();
                           1253 ;     genCall
                           1254 ;Initial/src/lib/usb_common.c:184: }
                           1255 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0DB9 02 06 85           1256 	ljmp	_fx2_stall_ep0
                           1257 ;------------------------------------------------------------
                           1258 ;Allocation info for local variables in function 'usb_handle_clear_feature'
                           1259 ;------------------------------------------------------------
                           1260 ;------------------------------------------------------------
                           1261 ;Initial/src/lib/usb_common.c:187: void usb_handle_clear_feature(void)
                           1262 ;	-----------------------------------------
                           1263 ;	 function usb_handle_clear_feature
                           1264 ;	-----------------------------------------
   0DBC                    1265 _usb_handle_clear_feature:
                           1266 ;Initial/src/lib/usb_common.c:189: switch (bRequestType & bmRT_RECIP_MASK)
                           1267 ;     genPointerGet
                           1268 ;     genFarPointerGet
   0DBC 90 E6 B8           1269 	mov	dptr,#_SETUPDAT
   0DBF E0                 1270 	movx	a,@dptr
   0DC0 FA                 1271 	mov	r2,a
                           1272 ;     genAnd
   0DC1 53 02 1F           1273 	anl	ar2,#0x1F
                           1274 ;     genCmpEq
   0DC4 BA 00 02           1275 	cjne	r2,#0x00,00118$
                           1276 ;	Peephole 112.b	changed ljmp to sjmp
   0DC7 80 05              1277 	sjmp	00101$
   0DC9                    1278 00118$:
                           1279 ;     genCmpEq
                           1280 ;	Peephole 112.b	changed ljmp to sjmp
                           1281 ;Initial/src/lib/usb_common.c:192: case bmRT_RECIP_DEVICE:
                           1282 ;	Peephole 112.b	changed ljmp to sjmp
                           1283 ;	Peephole 199	optimized misc jump sequence
   0DC9 BA 02 58           1284 	cjne	r2,#0x02,00110$
   0DCC 80 07              1285 	sjmp	00105$
                           1286 ;00119$:
   0DCE                    1287 00101$:
                           1288 ;Initial/src/lib/usb_common.c:193: switch (wValueL)
                           1289 ;     genPointerGet
                           1290 ;     genFarPointerGet
   0DCE 90 E6 BA           1291 	mov	dptr,#(_SETUPDAT + 0x0002)
   0DD1 E0                 1292 	movx	a,@dptr
                           1293 ;Initial/src/lib/usb_common.c:197: fx2_stall_ep0 ();
                           1294 ;     genCall
                           1295 ;Initial/src/lib/usb_common.c:199: break;
                           1296 ;Initial/src/lib/usb_common.c:201: case bmRT_RECIP_ENDPOINT:
                           1297 ;	Peephole 112.b	changed ljmp to sjmp
                           1298 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1299 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0DD2 02 06 85           1300 	ljmp	_fx2_stall_ep0
   0DD5                    1301 00105$:
                           1302 ;Initial/src/lib/usb_common.c:202: if (wValueL == FS_ENDPOINT_HALT && plausible_endpoint (wIndexL))
                           1303 ;     genPointerGet
                           1304 ;     genFarPointerGet
   0DD5 90 E6 BA           1305 	mov	dptr,#(_SETUPDAT + 0x0002)
   0DD8 E0                 1306 	movx	a,@dptr
                           1307 ;     genCmpEq
                           1308 ;	Peephole 115.b	jump optimization
   0DD9 FA                 1309 	mov	r2,a
   0DDA 60 02              1310 	jz	00121$
   0DDC                    1311 00120$:
                           1312 ;	Peephole 112.b	changed ljmp to sjmp
   0DDC 80 43              1313 	sjmp	00107$
   0DDE                    1314 00121$:
                           1315 ;     genPointerGet
                           1316 ;     genFarPointerGet
   0DDE 90 E6 BC           1317 	mov	dptr,#(_SETUPDAT + 0x0004)
   0DE1 E0                 1318 	movx	a,@dptr
                           1319 ;     genCall
                           1320 ;	Peephole 244.c	loading dpl from a instead of r2
   0DE2 FA                 1321 	mov	r2,a
   0DE3 F5 82              1322 	mov	dpl,a
   0DE5 12 0C E6           1323 	lcall	_plausible_endpoint
   0DE8 E5 82              1324 	mov	a,dpl
                           1325 ;     genIfx
                           1326 ;     genIfxJump
                           1327 ;	Peephole 110	removed ljmp by inverse jump logic
   0DEA 60 35              1328 	jz	00107$
   0DEC                    1329 00122$:
                           1330 ;Initial/src/lib/usb_common.c:204: *epcs (wIndexL) &= ~bmEPSTALL;
                           1331 ;     genPointerGet
                           1332 ;     genFarPointerGet
   0DEC 90 E6 BC           1333 	mov	dptr,#(_SETUPDAT + 0x0004)
   0DEF E0                 1334 	movx	a,@dptr
                           1335 ;     genCall
                           1336 ;	Peephole 244.c	loading dpl from a instead of r2
   0DF0 FA                 1337 	mov	r2,a
   0DF1 F5 82              1338 	mov	dpl,a
   0DF3 12 0D 05           1339 	lcall	_epcs
   0DF6 AA 82              1340 	mov	r2,dpl
   0DF8 AB 83              1341 	mov	r3,dph
                           1342 ;     genPointerGet
                           1343 ;     genFarPointerGet
   0DFA 90 E6 BC           1344 	mov	dptr,#(_SETUPDAT + 0x0004)
   0DFD E0                 1345 	movx	a,@dptr
                           1346 ;     genCall
                           1347 ;	Peephole 244.c	loading dpl from a instead of r4
   0DFE FC                 1348 	mov	r4,a
   0DFF F5 82              1349 	mov	dpl,a
   0E01 C0 02              1350 	push	ar2
   0E03 C0 03              1351 	push	ar3
   0E05 12 0D 05           1352 	lcall	_epcs
   0E08 D0 03              1353 	pop	ar3
   0E0A D0 02              1354 	pop	ar2
                           1355 ;     genPointerGet
                           1356 ;     genFarPointerGet
   0E0C E0                 1357 	movx	a,@dptr
   0E0D FC                 1358 	mov	r4,a
                           1359 ;     genAnd
   0E0E 53 04 FE           1360 	anl	ar4,#0xFE
                           1361 ;     genPointerSet
                           1362 ;     genFarPointerSet
   0E11 8A 82              1363 	mov	dpl,r2
   0E13 8B 83              1364 	mov	dph,r3
   0E15 EC                 1365 	mov	a,r4
   0E16 F0                 1366 	movx	@dptr,a
                           1367 ;Initial/src/lib/usb_common.c:205: fx2_reset_data_toggle (wIndexL);
                           1368 ;     genPointerGet
                           1369 ;     genFarPointerGet
   0E17 90 E6 BC           1370 	mov	dptr,#(_SETUPDAT + 0x0004)
   0E1A E0                 1371 	movx	a,@dptr
                           1372 ;     genCall
                           1373 ;	Peephole 244.c	loading dpl from a instead of r2
   0E1B FA                 1374 	mov	r2,a
   0E1C F5 82              1375 	mov	dpl,a
                           1376 ;	Peephole 112.b	changed ljmp to sjmp
                           1377 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1378 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0E1E 02 06 8E           1379 	ljmp	_fx2_reset_data_toggle
   0E21                    1380 00107$:
                           1381 ;Initial/src/lib/usb_common.c:208: fx2_stall_ep0 ();
                           1382 ;     genCall
                           1383 ;Initial/src/lib/usb_common.c:209: break;
                           1384 ;Initial/src/lib/usb_common.c:211: default:
                           1385 ;	Peephole 112.b	changed ljmp to sjmp
                           1386 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1387 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0E21 02 06 85           1388 	ljmp	_fx2_stall_ep0
   0E24                    1389 00110$:
                           1390 ;Initial/src/lib/usb_common.c:212: fx2_stall_ep0 ();
                           1391 ;     genCall
                           1392 ;Initial/src/lib/usb_common.c:214: }
                           1393 ;	Peephole 253.c	replaced lcall with ljmp
   0E24 02 06 85           1394 	ljmp	_fx2_stall_ep0
   0E27                    1395 00112$:
   0E27 22                 1396 	ret
                           1397 ;------------------------------------------------------------
                           1398 ;Allocation info for local variables in function 'usb_handle_get_status'
                           1399 ;------------------------------------------------------------
                           1400 ;------------------------------------------------------------
                           1401 ;Initial/src/lib/usb_common.c:217: void usb_handle_get_status(void)
                           1402 ;	-----------------------------------------
                           1403 ;	 function usb_handle_get_status
                           1404 ;	-----------------------------------------
   0E28                    1405 _usb_handle_get_status:
                           1406 ;Initial/src/lib/usb_common.c:219: switch (bRequestType & bmRT_RECIP_MASK)
                           1407 ;     genPointerGet
                           1408 ;     genFarPointerGet
   0E28 90 E6 B8           1409 	mov	dptr,#_SETUPDAT
   0E2B E0                 1410 	movx	a,@dptr
   0E2C FA                 1411 	mov	r2,a
                           1412 ;     genAnd
   0E2D 53 02 1F           1413 	anl	ar2,#0x1F
                           1414 ;     genCmpEq
   0E30 BA 00 02           1415 	cjne	r2,#0x00,00115$
                           1416 ;	Peephole 112.b	changed ljmp to sjmp
   0E33 80 0A              1417 	sjmp	00101$
   0E35                    1418 00115$:
                           1419 ;     genCmpEq
   0E35 BA 01 02           1420 	cjne	r2,#0x01,00116$
                           1421 ;	Peephole 112.b	changed ljmp to sjmp
   0E38 80 1B              1422 	sjmp	00102$
   0E3A                    1423 00116$:
                           1424 ;     genCmpEq
                           1425 ;	Peephole 112.b	changed ljmp to sjmp
                           1426 ;Initial/src/lib/usb_common.c:221: case bmRT_RECIP_DEVICE:
                           1427 ;	Peephole 112.b	changed ljmp to sjmp
                           1428 ;	Peephole 199	optimized misc jump sequence
   0E3A BA 02 61           1429 	cjne	r2,#0x02,00107$
   0E3D 80 2A              1430 	sjmp	00103$
                           1431 ;00117$:
   0E3F                    1432 00101$:
                           1433 ;Initial/src/lib/usb_common.c:222: EP0BUF[0] = bmGSDA_SELF_POWERED;	// FIXME app should handle
                           1434 ;     genPointerSet
                           1435 ;     genFarPointerSet
   0E3F 90 E7 40           1436 	mov	dptr,#_EP0BUF
   0E42 74 01              1437 	mov	a,#0x01
   0E44 F0                 1438 	movx	@dptr,a
                           1439 ;Initial/src/lib/usb_common.c:223: EP0BUF[1] = 0;
                           1440 ;     genPointerSet
                           1441 ;     genFarPointerSet
   0E45 90 E7 41           1442 	mov	dptr,#(_EP0BUF + 0x0001)
                           1443 ;	Peephole 181	changed mov to clr
                           1444 ;Initial/src/lib/usb_common.c:224: EP0BCH = 0;
                           1445 ;     genAssign
                           1446 ;	Peephole 181	changed mov to clr
                           1447 ;	Peephole 219	removed redundant clear
   0E48 E4                 1448 	clr	a
   0E49 F0                 1449 	movx	@dptr,a
   0E4A 90 E6 8A           1450 	mov	dptr,#_EP0BCH
   0E4D F0                 1451 	movx	@dptr,a
                           1452 ;Initial/src/lib/usb_common.c:225: EP0BCL = 2;
                           1453 ;     genAssign
   0E4E 90 E6 8B           1454 	mov	dptr,#_EP0BCL
   0E51 74 02              1455 	mov	a,#0x02
   0E53 F0                 1456 	movx	@dptr,a
                           1457 ;Initial/src/lib/usb_common.c:226: break;
                           1458 ;Initial/src/lib/usb_common.c:228: case bmRT_RECIP_INTERFACE:
                           1459 ;	Peephole 112.b	changed ljmp to sjmp
                           1460 ;	Peephole 251.b	replaced sjmp to ret with ret
   0E54 22                 1461 	ret
   0E55                    1462 00102$:
                           1463 ;Initial/src/lib/usb_common.c:229: EP0BUF[0] = 0;
                           1464 ;     genPointerSet
                           1465 ;     genFarPointerSet
   0E55 90 E7 40           1466 	mov	dptr,#_EP0BUF
                           1467 ;	Peephole 181	changed mov to clr
                           1468 ;Initial/src/lib/usb_common.c:230: EP0BUF[1] = 0;
                           1469 ;     genPointerSet
                           1470 ;     genFarPointerSet
                           1471 ;	Peephole 181	changed mov to clr
                           1472 ;	Peephole 219	removed redundant clear
                           1473 ;Initial/src/lib/usb_common.c:231: EP0BCH = 0;
                           1474 ;     genAssign
                           1475 ;	Peephole 181	changed mov to clr
                           1476 ;	Peephole 219.a	removed redundant clear
   0E58 E4                 1477 	clr	a
   0E59 F0                 1478 	movx	@dptr,a
   0E5A 90 E7 41           1479 	mov	dptr,#(_EP0BUF + 0x0001)
   0E5D F0                 1480 	movx	@dptr,a
   0E5E 90 E6 8A           1481 	mov	dptr,#_EP0BCH
   0E61 F0                 1482 	movx	@dptr,a
                           1483 ;Initial/src/lib/usb_common.c:232: EP0BCL = 2;
                           1484 ;     genAssign
   0E62 90 E6 8B           1485 	mov	dptr,#_EP0BCL
   0E65 74 02              1486 	mov	a,#0x02
   0E67 F0                 1487 	movx	@dptr,a
                           1488 ;Initial/src/lib/usb_common.c:233: break;
                           1489 ;Initial/src/lib/usb_common.c:235: case bmRT_RECIP_ENDPOINT:
                           1490 ;	Peephole 112.b	changed ljmp to sjmp
                           1491 ;	Peephole 251.b	replaced sjmp to ret with ret
   0E68 22                 1492 	ret
   0E69                    1493 00103$:
                           1494 ;Initial/src/lib/usb_common.c:236: if (plausible_endpoint (wIndexL))
                           1495 ;     genPointerGet
                           1496 ;     genFarPointerGet
   0E69 90 E6 BC           1497 	mov	dptr,#(_SETUPDAT + 0x0004)
   0E6C E0                 1498 	movx	a,@dptr
                           1499 ;     genCall
                           1500 ;	Peephole 244.c	loading dpl from a instead of r2
   0E6D FA                 1501 	mov	r2,a
   0E6E F5 82              1502 	mov	dpl,a
   0E70 12 0C E6           1503 	lcall	_plausible_endpoint
   0E73 E5 82              1504 	mov	a,dpl
                           1505 ;     genIfx
                           1506 ;     genIfxJump
                           1507 ;	Peephole 110	removed ljmp by inverse jump logic
   0E75 60 24              1508 	jz	00105$
   0E77                    1509 00118$:
                           1510 ;Initial/src/lib/usb_common.c:238: EP0BUF[0] = *epcs (wIndexL) & bmEPSTALL;
                           1511 ;     genPointerGet
                           1512 ;     genFarPointerGet
   0E77 90 E6 BC           1513 	mov	dptr,#(_SETUPDAT + 0x0004)
   0E7A E0                 1514 	movx	a,@dptr
                           1515 ;     genCall
                           1516 ;	Peephole 244.c	loading dpl from a instead of r2
   0E7B FA                 1517 	mov	r2,a
   0E7C F5 82              1518 	mov	dpl,a
   0E7E 12 0D 05           1519 	lcall	_epcs
                           1520 ;     genPointerGet
                           1521 ;     genFarPointerGet
   0E81 E0                 1522 	movx	a,@dptr
   0E82 FA                 1523 	mov	r2,a
                           1524 ;     genAnd
   0E83 53 02 01           1525 	anl	ar2,#0x01
                           1526 ;     genPointerSet
                           1527 ;     genFarPointerSet
   0E86 90 E7 40           1528 	mov	dptr,#_EP0BUF
   0E89 EA                 1529 	mov	a,r2
   0E8A F0                 1530 	movx	@dptr,a
                           1531 ;Initial/src/lib/usb_common.c:239: EP0BUF[1] = 0;
                           1532 ;     genPointerSet
                           1533 ;     genFarPointerSet
   0E8B 90 E7 41           1534 	mov	dptr,#(_EP0BUF + 0x0001)
                           1535 ;	Peephole 181	changed mov to clr
                           1536 ;Initial/src/lib/usb_common.c:240: EP0BCH = 0;
                           1537 ;     genAssign
                           1538 ;	Peephole 181	changed mov to clr
                           1539 ;	Peephole 219	removed redundant clear
   0E8E E4                 1540 	clr	a
   0E8F F0                 1541 	movx	@dptr,a
   0E90 90 E6 8A           1542 	mov	dptr,#_EP0BCH
   0E93 F0                 1543 	movx	@dptr,a
                           1544 ;Initial/src/lib/usb_common.c:241: EP0BCL = 2;
                           1545 ;     genAssign
   0E94 90 E6 8B           1546 	mov	dptr,#_EP0BCL
   0E97 74 02              1547 	mov	a,#0x02
   0E99 F0                 1548 	movx	@dptr,a
                           1549 ;	Peephole 112.b	changed ljmp to sjmp
                           1550 ;	Peephole 251.b	replaced sjmp to ret with ret
   0E9A 22                 1551 	ret
   0E9B                    1552 00105$:
                           1553 ;Initial/src/lib/usb_common.c:244: fx2_stall_ep0 ();
                           1554 ;     genCall
                           1555 ;Initial/src/lib/usb_common.c:245: break;
                           1556 ;Initial/src/lib/usb_common.c:247: default:
                           1557 ;	Peephole 112.b	changed ljmp to sjmp
                           1558 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1559 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0E9B 02 06 85           1560 	ljmp	_fx2_stall_ep0
   0E9E                    1561 00107$:
                           1562 ;Initial/src/lib/usb_common.c:248: fx2_stall_ep0 ();
                           1563 ;     genCall
                           1564 ;Initial/src/lib/usb_common.c:250: }
                           1565 ;	Peephole 253.c	replaced lcall with ljmp
   0E9E 02 06 85           1566 	ljmp	_fx2_stall_ep0
   0EA1                    1567 00109$:
   0EA1 22                 1568 	ret
                           1569 ;------------------------------------------------------------
                           1570 ;Allocation info for local variables in function 'usb_handle_get_descriptor'
                           1571 ;------------------------------------------------------------
                           1572 ;p                         Allocated to registers r2 r3 
                           1573 ;------------------------------------------------------------
                           1574 ;Initial/src/lib/usb_common.c:253: void usb_handle_get_descriptor(void)
                           1575 ;	-----------------------------------------
                           1576 ;	 function usb_handle_get_descriptor
                           1577 ;	-----------------------------------------
   0EA2                    1578 _usb_handle_get_descriptor:
                           1579 ;Initial/src/lib/usb_common.c:255: switch (wValueH)
                           1580 ;     genPointerGet
                           1581 ;     genFarPointerGet
   0EA2 90 E6 BB           1582 	mov	dptr,#(_SETUPDAT + 0x0003)
   0EA5 E0                 1583 	movx	a,@dptr
   0EA6 FA                 1584 	mov	r2,a
                           1585 ;     genCmpEq
   0EA7 BA 01 02           1586 	cjne	r2,#0x01,00127$
                           1587 ;	Peephole 112.b	changed ljmp to sjmp
   0EAA 80 17              1588 	sjmp	00101$
   0EAC                    1589 00127$:
                           1590 ;     genCmpEq
   0EAC BA 02 02           1591 	cjne	r2,#0x02,00128$
                           1592 ;	Peephole 112.b	changed ljmp to sjmp
   0EAF 80 3C              1593 	sjmp	00105$
   0EB1                    1594 00128$:
                           1595 ;     genCmpEq
   0EB1 BA 03 02           1596 	cjne	r2,#0x03,00129$
                           1597 ;	Peephole 112.b	changed ljmp to sjmp
   0EB4 80 61              1598 	sjmp	00113$
   0EB6                    1599 00129$:
                           1600 ;     genCmpEq
   0EB6 BA 06 02           1601 	cjne	r2,#0x06,00130$
                           1602 ;	Peephole 112.b	changed ljmp to sjmp
   0EB9 80 1D              1603 	sjmp	00102$
   0EBB                    1604 00130$:
                           1605 ;     genCmpEq
   0EBB BA 07 02           1606 	cjne	r2,#0x07,00131$
                           1607 ;	Peephole 112.b	changed ljmp to sjmp
   0EBE 80 42              1608 	sjmp	00110$
   0EC0                    1609 00131$:
   0EC0 02 0F 52           1610 	ljmp	00117$
                           1611 ;Initial/src/lib/usb_common.c:258: case DT_DEVICE:
   0EC3                    1612 00101$:
                           1613 ;Initial/src/lib/usb_common.c:259: SUDPTRH = MSB (current_device_descr);
                           1614 ;     genCast
   0EC3 AA 23              1615 	mov	r2,_current_device_descr
   0EC5 AB 24              1616 	mov	r3,(_current_device_descr + 1)
                           1617 ;     genRightShift
                           1618 ;     genRightShiftLiteral
                           1619 ;     genrshTwo
   0EC7 8B 04              1620 	mov	ar4,r3
   0EC9 7D 00              1621 	mov	r5,#0x00
                           1622 ;     genCast
   0ECB 90 E6 B3           1623 	mov	dptr,#_SUDPTRH
   0ECE EC                 1624 	mov	a,r4
   0ECF F0                 1625 	movx	@dptr,a
                           1626 ;Initial/src/lib/usb_common.c:260: SUDPTRL = LSB (current_device_descr);
                           1627 ;     genAnd
   0ED0 7B 00              1628 	mov	r3,#0x00
                           1629 ;     genCast
   0ED2 90 E6 B4           1630 	mov	dptr,#_SUDPTRL
   0ED5 EA                 1631 	mov	a,r2
   0ED6 F0                 1632 	movx	@dptr,a
                           1633 ;Initial/src/lib/usb_common.c:261: break;
                           1634 ;	Peephole 251.a	replaced ljmp to ret with ret
   0ED7 22                 1635 	ret
                           1636 ;Initial/src/lib/usb_common.c:263: case DT_DEVQUAL:
   0ED8                    1637 00102$:
                           1638 ;Initial/src/lib/usb_common.c:264: SUDPTRH = MSB (current_devqual_descr);
                           1639 ;     genCast
   0ED8 AA 25              1640 	mov	r2,_current_devqual_descr
   0EDA AB 26              1641 	mov	r3,(_current_devqual_descr + 1)
                           1642 ;     genRightShift
                           1643 ;     genRightShiftLiteral
                           1644 ;     genrshTwo
   0EDC 8B 04              1645 	mov	ar4,r3
   0EDE 7D 00              1646 	mov	r5,#0x00
                           1647 ;     genCast
   0EE0 90 E6 B3           1648 	mov	dptr,#_SUDPTRH
   0EE3 EC                 1649 	mov	a,r4
   0EE4 F0                 1650 	movx	@dptr,a
                           1651 ;Initial/src/lib/usb_common.c:265: SUDPTRL = LSB (current_devqual_descr);
                           1652 ;     genAnd
   0EE5 7B 00              1653 	mov	r3,#0x00
                           1654 ;     genCast
   0EE7 90 E6 B4           1655 	mov	dptr,#_SUDPTRL
   0EEA EA                 1656 	mov	a,r2
   0EEB F0                 1657 	movx	@dptr,a
                           1658 ;Initial/src/lib/usb_common.c:266: break;
                           1659 ;Initial/src/lib/usb_common.c:270: fx2_stall_ep0 ();
                           1660 ;	Peephole 112.b	changed ljmp to sjmp
                           1661 ;	Peephole 251.b	replaced sjmp to ret with ret
   0EEC 22                 1662 	ret
   0EED                    1663 00105$:
                           1664 ;Initial/src/lib/usb_common.c:273: SUDPTRH = MSB (current_config_descr);
                           1665 ;     genCast
   0EED AA 27              1666 	mov	r2,_current_config_descr
   0EEF AB 28              1667 	mov	r3,(_current_config_descr + 1)
                           1668 ;     genRightShift
                           1669 ;     genRightShiftLiteral
                           1670 ;     genrshTwo
   0EF1 8B 04              1671 	mov	ar4,r3
   0EF3 7D 00              1672 	mov	r5,#0x00
                           1673 ;     genCast
   0EF5 90 E6 B3           1674 	mov	dptr,#_SUDPTRH
   0EF8 EC                 1675 	mov	a,r4
   0EF9 F0                 1676 	movx	@dptr,a
                           1677 ;Initial/src/lib/usb_common.c:274: SUDPTRL = LSB (current_config_descr);
                           1678 ;     genAnd
   0EFA 7B 00              1679 	mov	r3,#0x00
                           1680 ;     genCast
   0EFC 90 E6 B4           1681 	mov	dptr,#_SUDPTRL
   0EFF EA                 1682 	mov	a,r2
   0F00 F0                 1683 	movx	@dptr,a
                           1684 ;Initial/src/lib/usb_common.c:276: break;
                           1685 ;Initial/src/lib/usb_common.c:280: fx2_stall_ep0 ();
                           1686 ;	Peephole 112.b	changed ljmp to sjmp
                           1687 ;	Peephole 251.b	replaced sjmp to ret with ret
   0F01 22                 1688 	ret
   0F02                    1689 00110$:
                           1690 ;Initial/src/lib/usb_common.c:283: SUDPTRH = MSB (other_config_descr);
                           1691 ;     genCast
   0F02 AA 29              1692 	mov	r2,_other_config_descr
   0F04 AB 2A              1693 	mov	r3,(_other_config_descr + 1)
                           1694 ;     genRightShift
                           1695 ;     genRightShiftLiteral
                           1696 ;     genrshTwo
   0F06 8B 04              1697 	mov	ar4,r3
   0F08 7D 00              1698 	mov	r5,#0x00
                           1699 ;     genCast
   0F0A 90 E6 B3           1700 	mov	dptr,#_SUDPTRH
   0F0D EC                 1701 	mov	a,r4
   0F0E F0                 1702 	movx	@dptr,a
                           1703 ;Initial/src/lib/usb_common.c:284: SUDPTRL = LSB (other_config_descr);
                           1704 ;     genAnd
   0F0F 7B 00              1705 	mov	r3,#0x00
                           1706 ;     genCast
   0F11 90 E6 B4           1707 	mov	dptr,#_SUDPTRL
   0F14 EA                 1708 	mov	a,r2
   0F15 F0                 1709 	movx	@dptr,a
                           1710 ;Initial/src/lib/usb_common.c:286: break;
                           1711 ;Initial/src/lib/usb_common.c:288: case DT_STRING:
                           1712 ;	Peephole 112.b	changed ljmp to sjmp
                           1713 ;	Peephole 251.b	replaced sjmp to ret with ret
   0F16 22                 1714 	ret
   0F17                    1715 00113$:
                           1716 ;Initial/src/lib/usb_common.c:289: if (wValueL >= nstring_descriptors)
                           1717 ;     genPointerGet
                           1718 ;     genFarPointerGet
   0F17 90 E6 BA           1719 	mov	dptr,#(_SETUPDAT + 0x0002)
   0F1A E0                 1720 	movx	a,@dptr
   0F1B FA                 1721 	mov	r2,a
                           1722 ;     genAssign
   0F1C 90 E0 78           1723 	mov	dptr,#_nstring_descriptors
   0F1F E0                 1724 	movx	a,@dptr
   0F20 FB                 1725 	mov	r3,a
                           1726 ;     genCmpLt
                           1727 ;     genCmp
   0F21 C3                 1728 	clr	c
   0F22 EA                 1729 	mov	a,r2
   0F23 9B                 1730 	subb	a,r3
                           1731 ;     genIfxJump
                           1732 ;	Peephole 112.b	changed ljmp to sjmp
                           1733 ;	Peephole 160	removed sjmp by inverse jump logic
   0F24 40 03              1734 	jc	00115$
   0F26                    1735 00132$:
                           1736 ;Initial/src/lib/usb_common.c:290: fx2_stall_ep0 ();
                           1737 ;     genCall
                           1738 ;	Peephole 112.b	changed ljmp to sjmp
                           1739 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1740 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0F26 02 06 85           1741 	ljmp	_fx2_stall_ep0
   0F29                    1742 00115$:
                           1743 ;Initial/src/lib/usb_common.c:293: xdata char *p = string_descriptors[wValueL];
                           1744 ;     genPointerGet
                           1745 ;     genFarPointerGet
   0F29 90 E6 BA           1746 	mov	dptr,#(_SETUPDAT + 0x0002)
   0F2C E0                 1747 	movx	a,@dptr
                           1748 ;     genMult
                           1749 ;     genMultOneByte
                           1750 ;	Peephole 105	removed redundant mov
   0F2D FA                 1751 	mov	r2,a
   0F2E 75 F0 02           1752 	mov	b,#0x02
   0F31 A4                 1753 	mul	ab
                           1754 ;     genPlus
   0F32 24 79              1755 	add	a,#_string_descriptors
   0F34 F5 82              1756 	mov	dpl,a
   0F36 74 E0              1757 	mov	a,#(_string_descriptors >> 8)
   0F38 35 F0              1758 	addc	a,b
   0F3A F5 83              1759 	mov	dph,a
                           1760 ;     genPointerGet
                           1761 ;     genFarPointerGet
   0F3C E0                 1762 	movx	a,@dptr
   0F3D FA                 1763 	mov	r2,a
   0F3E A3                 1764 	inc	dptr
   0F3F E0                 1765 	movx	a,@dptr
   0F40 FB                 1766 	mov	r3,a
                           1767 ;Initial/src/lib/usb_common.c:294: SUDPTRH = MSB (p);
                           1768 ;     genCast
                           1769 ;     genRightShift
                           1770 ;     genRightShiftLiteral
                           1771 ;     genrshTwo
   0F41 8B 04              1772 	mov	ar4,r3
   0F43 7D 00              1773 	mov	r5,#0x00
                           1774 ;     genCast
   0F45 90 E6 B3           1775 	mov	dptr,#_SUDPTRH
   0F48 EC                 1776 	mov	a,r4
   0F49 F0                 1777 	movx	@dptr,a
                           1778 ;Initial/src/lib/usb_common.c:295: SUDPTRL = LSB (p);
                           1779 ;     genAnd
   0F4A 7B 00              1780 	mov	r3,#0x00
                           1781 ;     genCast
   0F4C 90 E6 B4           1782 	mov	dptr,#_SUDPTRL
   0F4F EA                 1783 	mov	a,r2
   0F50 F0                 1784 	movx	@dptr,a
                           1785 ;Initial/src/lib/usb_common.c:297: break;
                           1786 ;Initial/src/lib/usb_common.c:299: default:
                           1787 ;	Peephole 112.b	changed ljmp to sjmp
                           1788 ;	Peephole 251.b	replaced sjmp to ret with ret
   0F51 22                 1789 	ret
   0F52                    1790 00117$:
                           1791 ;Initial/src/lib/usb_common.c:300: fx2_stall_ep0 ();	// invalid request
                           1792 ;     genCall
                           1793 ;Initial/src/lib/usb_common.c:302: }
                           1794 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0F52 02 06 85           1795 	ljmp	_fx2_stall_ep0
                           1796 ;------------------------------------------------------------
                           1797 ;Allocation info for local variables in function 'usb_handle_IN_std_requests'
                           1798 ;------------------------------------------------------------
                           1799 ;------------------------------------------------------------
                           1800 ;Initial/src/lib/usb_common.c:305: void usb_handle_IN_std_requests(void)
                           1801 ;	-----------------------------------------
                           1802 ;	 function usb_handle_IN_std_requests
                           1803 ;	-----------------------------------------
   0F55                    1804 _usb_handle_IN_std_requests:
                           1805 ;Initial/src/lib/usb_common.c:307: switch (bRequest)
                           1806 ;     genPointerGet
                           1807 ;     genFarPointerGet
   0F55 90 E6 B9           1808 	mov	dptr,#(_SETUPDAT + 0x0001)
   0F58 E0                 1809 	movx	a,@dptr
                           1810 ;     genCmpEq
                           1811 ;	Peephole 112.b	changed ljmp to sjmp
                           1812 ;	Peephole 115.b	jump optimization
   0F59 FA                 1813 	mov	r2,a
   0F5A 60 36              1814 	jz	00104$
   0F5C                    1815 00114$:
                           1816 ;     genCmpEq
   0F5C BA 06 02           1817 	cjne	r2,#0x06,00115$
                           1818 ;	Peephole 112.b	changed ljmp to sjmp
   0F5F 80 2E              1819 	sjmp	00103$
   0F61                    1820 00115$:
                           1821 ;     genCmpEq
   0F61 BA 08 02           1822 	cjne	r2,#0x08,00116$
                           1823 ;	Peephole 112.b	changed ljmp to sjmp
   0F64 80 05              1824 	sjmp	00101$
   0F66                    1825 00116$:
                           1826 ;     genCmpEq
                           1827 ;	Peephole 112.b	changed ljmp to sjmp
                           1828 ;Initial/src/lib/usb_common.c:310: case RQ_GET_CONFIG:
                           1829 ;	Peephole 112.b	changed ljmp to sjmp
                           1830 ;	Peephole 199	optimized misc jump sequence
   0F66 BA 0A 2C           1831 	cjne	r2,#0x0A,00106$
   0F69 80 12              1832 	sjmp	00102$
                           1833 ;00117$:
   0F6B                    1834 00101$:
                           1835 ;Initial/src/lib/usb_common.c:311: EP0BUF[0] = _usb_config;	// FIXME app should handle
                           1836 ;     genPointerSet
                           1837 ;     genFarPointerSet
   0F6B 90 E7 40           1838 	mov	dptr,#_EP0BUF
   0F6E E5 21              1839 	mov	a,__usb_config
   0F70 F0                 1840 	movx	@dptr,a
                           1841 ;Initial/src/lib/usb_common.c:312: EP0BCH = 0;
                           1842 ;     genAssign
   0F71 90 E6 8A           1843 	mov	dptr,#_EP0BCH
                           1844 ;	Peephole 181	changed mov to clr
   0F74 E4                 1845 	clr	a
   0F75 F0                 1846 	movx	@dptr,a
                           1847 ;Initial/src/lib/usb_common.c:313: EP0BCL = 1;
                           1848 ;     genAssign
   0F76 90 E6 8B           1849 	mov	dptr,#_EP0BCL
   0F79 74 01              1850 	mov	a,#0x01
   0F7B F0                 1851 	movx	@dptr,a
                           1852 ;Initial/src/lib/usb_common.c:314: break;
                           1853 ;Initial/src/lib/usb_common.c:316: case RQ_GET_INTERFACE:
                           1854 ;	Peephole 112.b	changed ljmp to sjmp
                           1855 ;	Peephole 251.b	replaced sjmp to ret with ret
   0F7C 22                 1856 	ret
   0F7D                    1857 00102$:
                           1858 ;Initial/src/lib/usb_common.c:317: EP0BUF[0] = _usb_alt_setting;	// FIXME app should handle
                           1859 ;     genPointerSet
                           1860 ;     genFarPointerSet
   0F7D 90 E7 40           1861 	mov	dptr,#_EP0BUF
   0F80 E5 22              1862 	mov	a,__usb_alt_setting
   0F82 F0                 1863 	movx	@dptr,a
                           1864 ;Initial/src/lib/usb_common.c:318: EP0BCH = 0;
                           1865 ;     genAssign
   0F83 90 E6 8A           1866 	mov	dptr,#_EP0BCH
                           1867 ;	Peephole 181	changed mov to clr
   0F86 E4                 1868 	clr	a
   0F87 F0                 1869 	movx	@dptr,a
                           1870 ;Initial/src/lib/usb_common.c:319: EP0BCL = 1;
                           1871 ;     genAssign
   0F88 90 E6 8B           1872 	mov	dptr,#_EP0BCL
   0F8B 74 01              1873 	mov	a,#0x01
   0F8D F0                 1874 	movx	@dptr,a
                           1875 ;Initial/src/lib/usb_common.c:320: break;
                           1876 ;Initial/src/lib/usb_common.c:322: case RQ_GET_DESCR:
                           1877 ;	Peephole 112.b	changed ljmp to sjmp
                           1878 ;	Peephole 251.b	replaced sjmp to ret with ret
   0F8E 22                 1879 	ret
   0F8F                    1880 00103$:
                           1881 ;Initial/src/lib/usb_common.c:323: usb_handle_get_descriptor();
                           1882 ;     genCall
                           1883 ;Initial/src/lib/usb_common.c:324: break;
                           1884 ;Initial/src/lib/usb_common.c:326: case RQ_GET_STATUS:
                           1885 ;	Peephole 112.b	changed ljmp to sjmp
                           1886 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1887 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0F8F 02 0E A2           1888 	ljmp	_usb_handle_get_descriptor
   0F92                    1889 00104$:
                           1890 ;Initial/src/lib/usb_common.c:327: usb_handle_get_status();
                           1891 ;     genCall
                           1892 ;Initial/src/lib/usb_common.c:328: break;
                           1893 ;Initial/src/lib/usb_common.c:331: default:
                           1894 ;	Peephole 112.b	changed ljmp to sjmp
                           1895 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1896 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0F92 02 0E 28           1897 	ljmp	_usb_handle_get_status
   0F95                    1898 00106$:
                           1899 ;Initial/src/lib/usb_common.c:332: fx2_stall_ep0 ();
                           1900 ;     genCall
                           1901 ;Initial/src/lib/usb_common.c:334: }
                           1902 ;	Peephole 253.c	replaced lcall with ljmp
   0F95 02 06 85           1903 	ljmp	_fx2_stall_ep0
   0F98                    1904 00108$:
   0F98 22                 1905 	ret
                           1906 ;------------------------------------------------------------
                           1907 ;Allocation info for local variables in function 'usb_handle_OUT_std_requests'
                           1908 ;------------------------------------------------------------
                           1909 ;------------------------------------------------------------
                           1910 ;Initial/src/lib/usb_common.c:337: void usb_handle_OUT_std_requests(void)
                           1911 ;	-----------------------------------------
                           1912 ;	 function usb_handle_OUT_std_requests
                           1913 ;	-----------------------------------------
   0F99                    1914 _usb_handle_OUT_std_requests:
                           1915 ;Initial/src/lib/usb_common.c:339: switch (bRequest)
                           1916 ;     genPointerGet
                           1917 ;     genFarPointerGet
   0F99 90 E6 B9           1918 	mov	dptr,#(_SETUPDAT + 0x0001)
   0F9C E0                 1919 	movx	a,@dptr
   0F9D FA                 1920 	mov	r2,a
                           1921 ;     genCmpEq
   0F9E BA 01 02           1922 	cjne	r2,#0x01,00117$
                           1923 ;	Peephole 112.b	changed ljmp to sjmp
   0FA1 80 27              1924 	sjmp	00103$
   0FA3                    1925 00117$:
                           1926 ;     genCmpEq
   0FA3 BA 03 02           1927 	cjne	r2,#0x03,00118$
                           1928 ;	Peephole 112.b	changed ljmp to sjmp
   0FA6 80 25              1929 	sjmp	00104$
   0FA8                    1930 00118$:
                           1931 ;     genCmpEq
   0FA8 BA 05 02           1932 	cjne	r2,#0x05,00119$
                           1933 ;	Peephole 112.b	changed ljmp to sjmp
   0FAB 80 23              1934 	sjmp	00107$
   0FAD                    1935 00119$:
                           1936 ;     genCmpEq
   0FAD BA 07 02           1937 	cjne	r2,#0x07,00120$
                           1938 ;	Peephole 112.b	changed ljmp to sjmp
   0FB0 80 1E              1939 	sjmp	00107$
   0FB2                    1940 00120$:
                           1941 ;     genCmpEq
   0FB2 BA 09 02           1942 	cjne	r2,#0x09,00121$
                           1943 ;	Peephole 112.b	changed ljmp to sjmp
   0FB5 80 05              1944 	sjmp	00101$
   0FB7                    1945 00121$:
                           1946 ;     genCmpEq
                           1947 ;	Peephole 112.b	changed ljmp to sjmp
                           1948 ;Initial/src/lib/usb_common.c:342: case RQ_SET_CONFIG:
                           1949 ;	Peephole 112.b	changed ljmp to sjmp
                           1950 ;	Peephole 199	optimized misc jump sequence
   0FB7 BA 0B 16           1951 	cjne	r2,#0x0B,00107$
   0FBA 80 07              1952 	sjmp	00102$
                           1953 ;00122$:
   0FBC                    1954 00101$:
                           1955 ;Initial/src/lib/usb_common.c:343: _usb_config = wValueL;		// FIXME app should handle
                           1956 ;     genPointerGet
                           1957 ;     genFarPointerGet
   0FBC 90 E6 BA           1958 	mov	dptr,#(_SETUPDAT + 0x0002)
   0FBF E0                 1959 	movx	a,@dptr
   0FC0 F5 21              1960 	mov	__usb_config,a
                           1961 ;Initial/src/lib/usb_common.c:344: break;
                           1962 ;Initial/src/lib/usb_common.c:346: case RQ_SET_INTERFACE:
                           1963 ;	Peephole 112.b	changed ljmp to sjmp
                           1964 ;	Peephole 251.b	replaced sjmp to ret with ret
   0FC2 22                 1965 	ret
   0FC3                    1966 00102$:
                           1967 ;Initial/src/lib/usb_common.c:347: _usb_alt_setting = wValueL;	// FIXME app should handle
                           1968 ;     genPointerGet
                           1969 ;     genFarPointerGet
   0FC3 90 E6 BA           1970 	mov	dptr,#(_SETUPDAT + 0x0002)
   0FC6 E0                 1971 	movx	a,@dptr
   0FC7 F5 22              1972 	mov	__usb_alt_setting,a
                           1973 ;Initial/src/lib/usb_common.c:348: break;
                           1974 ;Initial/src/lib/usb_common.c:350: case RQ_CLEAR_FEATURE:
                           1975 ;	Peephole 112.b	changed ljmp to sjmp
                           1976 ;	Peephole 251.b	replaced sjmp to ret with ret
   0FC9 22                 1977 	ret
   0FCA                    1978 00103$:
                           1979 ;Initial/src/lib/usb_common.c:351: usb_handle_clear_feature();
                           1980 ;     genCall
                           1981 ;Initial/src/lib/usb_common.c:352: break;
                           1982 ;Initial/src/lib/usb_common.c:354: case RQ_SET_FEATURE:
                           1983 ;	Peephole 112.b	changed ljmp to sjmp
                           1984 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1985 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0FCA 02 0D BC           1986 	ljmp	_usb_handle_clear_feature
   0FCD                    1987 00104$:
                           1988 ;Initial/src/lib/usb_common.c:355: usb_handle_set_feature();
                           1989 ;     genCall
                           1990 ;Initial/src/lib/usb_common.c:356: break;
                           1991 ;Initial/src/lib/usb_common.c:360: default:
                           1992 ;	Peephole 112.b	changed ljmp to sjmp
                           1993 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1994 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0FCD 02 0D 4E           1995 	ljmp	_usb_handle_set_feature
   0FD0                    1996 00107$:
                           1997 ;Initial/src/lib/usb_common.c:361: fx2_stall_ep0 ();
                           1998 ;     genCall
                           1999 ;Initial/src/lib/usb_common.c:362: }
                           2000 ;	Peephole 253.c	replaced lcall with ljmp
   0FD0 02 06 85           2001 	ljmp	_fx2_stall_ep0
   0FD3                    2002 00109$:
   0FD3 22                 2003 	ret
                           2004 ;------------------------------------------------------------
                           2005 ;Allocation info for local variables in function 'usb_handle_std_requests'
                           2006 ;------------------------------------------------------------
                           2007 ;------------------------------------------------------------
                           2008 ;Initial/src/lib/usb_common.c:365: void usb_handle_std_requests(void)
                           2009 ;	-----------------------------------------
                           2010 ;	 function usb_handle_std_requests
                           2011 ;	-----------------------------------------
   0FD4                    2012 _usb_handle_std_requests:
                           2013 ;Initial/src/lib/usb_common.c:367: if ((bRequestType & bmRT_DIR_MASK) == bmRT_DIR_IN)
                           2014 ;     genPointerGet
                           2015 ;     genFarPointerGet
   0FD4 90 E6 B8           2016 	mov	dptr,#_SETUPDAT
   0FD7 E0                 2017 	movx	a,@dptr
   0FD8 FA                 2018 	mov	r2,a
                           2019 ;     genAnd
   0FD9 53 02 80           2020 	anl	ar2,#0x80
                           2021 ;     genCmpEq
                           2022 ;	Peephole 112.b	changed ljmp to sjmp
                           2023 ;	Peephole 199	optimized misc jump sequence
   0FDC BA 80 03           2024 	cjne	r2,#0x80,00102$
                           2025 ;00107$:
                           2026 ;	Peephole 200	removed redundant sjmp
   0FDF                    2027 00108$:
                           2028 ;Initial/src/lib/usb_common.c:369: usb_handle_IN_std_requests();
                           2029 ;     genCall
                           2030 ;	Peephole 112.b	changed ljmp to sjmp
                           2031 ;	Peephole 251.b	replaced sjmp to ret with ret
                           2032 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0FDF 02 0F 55           2033 	ljmp	_usb_handle_IN_std_requests
   0FE2                    2034 00102$:
                           2035 ;Initial/src/lib/usb_common.c:372: usb_handle_OUT_std_requests();
                           2036 ;     genCall
                           2037 ;	Peephole 253.c	replaced lcall with ljmp
   0FE2 02 0F 99           2038 	ljmp	_usb_handle_OUT_std_requests
   0FE5                    2039 00104$:
   0FE5 22                 2040 	ret
                           2041 ;------------------------------------------------------------
                           2042 ;Allocation info for local variables in function 'usb_handle_setup_packet'
                           2043 ;------------------------------------------------------------
                           2044 ;------------------------------------------------------------
                           2045 ;Initial/src/lib/usb_common.c:376: usb_handle_setup_packet (void)
                           2046 ;	-----------------------------------------
                           2047 ;	 function usb_handle_setup_packet
                           2048 ;	-----------------------------------------
   0FE6                    2049 _usb_handle_setup_packet:
                           2050 ;Initial/src/lib/usb_common.c:378: _usb_got_SUDAV = 0;
                           2051 ;     genAssign
   0FE6 C2 03              2052 	clr	__usb_got_SUDAV
                           2053 ;Initial/src/lib/usb_common.c:382: switch (bRequestType & bmRT_TYPE_MASK)
                           2054 ;     genPointerGet
                           2055 ;     genFarPointerGet
   0FE8 90 E6 B8           2056 	mov	dptr,#_SETUPDAT
   0FEB E0                 2057 	movx	a,@dptr
   0FEC FA                 2058 	mov	r2,a
                           2059 ;     genAnd
   0FED 53 02 60           2060 	anl	ar2,#0x60
                           2061 ;     genCmpEq
   0FF0 BA 00 02           2062 	cjne	r2,#0x00,00115$
                           2063 ;	Peephole 112.b	changed ljmp to sjmp
   0FF3 80 1E              2064 	sjmp	00106$
   0FF5                    2065 00115$:
                           2066 ;     genCmpEq
   0FF5 BA 20 02           2067 	cjne	r2,#0x20,00116$
                           2068 ;	Peephole 112.b	changed ljmp to sjmp
   0FF8 80 08              2069 	sjmp	00102$
   0FFA                    2070 00116$:
                           2071 ;     genCmpEq
   0FFA BA 40 02           2072 	cjne	r2,#0x40,00117$
                           2073 ;	Peephole 112.b	changed ljmp to sjmp
   0FFD 80 08              2074 	sjmp	00103$
   0FFF                    2075 00117$:
                           2076 ;     genCmpEq
                           2077 ;	Peephole 112.b	changed ljmp to sjmp
                           2078 ;	Peephole 199	optimized misc jump sequence
   0FFF BA 60 16           2079 	cjne	r2,#0x60,00107$
                           2080 ;00118$:
                           2081 ;	Peephole 200	removed redundant sjmp
   1002                    2082 00119$:
                           2083 ;Initial/src/lib/usb_common.c:386: case bmRT_TYPE_RESERVED:
   1002                    2084 00102$:
                           2085 ;Initial/src/lib/usb_common.c:387: fx2_stall_ep0 ();		// we don't handle these.  indicate error
                           2086 ;     genCall
   1002 12 06 85           2087 	lcall	_fx2_stall_ep0
                           2088 ;Initial/src/lib/usb_common.c:388: break;
                           2089 ;Initial/src/lib/usb_common.c:390: case bmRT_TYPE_VENDOR:
                           2090 ;	Peephole 112.b	changed ljmp to sjmp
   1005 80 14              2091 	sjmp	00108$
   1007                    2092 00103$:
                           2093 ;Initial/src/lib/usb_common.c:392: if (!app_vendor_cmd ())
                           2094 ;     genCall
   1007 12 03 6D           2095 	lcall	_app_vendor_cmd
   100A E5 82              2096 	mov	a,dpl
                           2097 ;     genIfx
                           2098 ;     genIfxJump
                           2099 ;	Peephole 109	removed ljmp by inverse jump logic
   100C 70 0D              2100 	jnz	00108$
   100E                    2101 00120$:
                           2102 ;Initial/src/lib/usb_common.c:393: fx2_stall_ep0 ();
                           2103 ;     genCall
   100E 12 06 85           2104 	lcall	_fx2_stall_ep0
                           2105 ;Initial/src/lib/usb_common.c:394: break;
                           2106 ;Initial/src/lib/usb_common.c:396: case bmRT_TYPE_STD:
                           2107 ;	Peephole 112.b	changed ljmp to sjmp
   1011 80 08              2108 	sjmp	00108$
   1013                    2109 00106$:
                           2110 ;Initial/src/lib/usb_common.c:398: usb_handle_std_requests();
                           2111 ;     genCall
   1013 12 0F D4           2112 	lcall	_usb_handle_std_requests
                           2113 ;Initial/src/lib/usb_common.c:399: break;
                           2114 ;Initial/src/lib/usb_common.c:401: default:
                           2115 ;	Peephole 112.b	changed ljmp to sjmp
   1016 80 03              2116 	sjmp	00108$
   1018                    2117 00107$:
                           2118 ;Initial/src/lib/usb_common.c:402: fx2_stall_ep0 ();		// indicate error
                           2119 ;     genCall
   1018 12 06 85           2120 	lcall	_fx2_stall_ep0
                           2121 ;Initial/src/lib/usb_common.c:405: }
   101B                    2122 00108$:
                           2123 ;Initial/src/lib/usb_common.c:408: EP0CS |= bmHSNAK;
                           2124 ;     genAssign
                           2125 ;     genOr
                           2126 ;	Peephole 248.a	optimized or to xdata
   101B 90 E6 A0           2127 	mov	dptr,#_EP0CS
   101E E0                 2128 	movx	a,@dptr
   101F FA                 2129 	mov	r2,a
   1020 44 80              2130 	orl	a,#0x80
   1022 F0                 2131 	movx	@dptr,a
   1023                    2132 00109$:
   1023 22                 2133 	ret
                           2134 	.area CSEG    (CODE)
