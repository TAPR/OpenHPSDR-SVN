                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Tue Jul 11 16:42:01 2006
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
                            697 	.area XISEG   (XDATA)
                            698 	.area CSEG    (CODE)
                            699 	.area GSINIT0 (CODE)
                            700 	.area GSINIT1 (CODE)
                            701 	.area GSINIT2 (CODE)
                            702 	.area GSINIT3 (CODE)
                            703 	.area GSINIT4 (CODE)
                            704 	.area GSINIT5 (CODE)
                            705 ;--------------------------------------------------------
                            706 ; global & static initialisations
                            707 ;--------------------------------------------------------
                            708 	.area CSEG    (CODE)
                            709 	.area GSINIT  (CODE)
                            710 	.area GSFINAL (CODE)
                            711 	.area GSINIT  (CODE)
                            712 ;Initial/src/lib/usb_common.c:56: unsigned char	_usb_config = 0;
                            713 ;     genAssign
   0F5D 75 21 00            714 	mov	__usb_config,#0x00
                            715 ;Initial/src/lib/usb_common.c:57: unsigned char	_usb_alt_setting = 0;	// FIXME really 1/interface
                            716 ;     genAssign
   0F60 75 22 00            717 	mov	__usb_alt_setting,#0x00
                            718 ;--------------------------------------------------------
                            719 ; Home
                            720 ;--------------------------------------------------------
                            721 	.area HOME    (CODE)
                            722 	.area CSEG    (CODE)
                            723 ;--------------------------------------------------------
                            724 ; code
                            725 ;--------------------------------------------------------
                            726 	.area CSEG    (CODE)
                            727 ;------------------------------------------------------------
                            728 ;Allocation info for local variables in function 'setup_descriptors'
                            729 ;------------------------------------------------------------
                            730 ;------------------------------------------------------------
                            731 ;Initial/src/lib/usb_common.c:65: setup_descriptors (void)
                            732 ;	-----------------------------------------
                            733 ;	 function setup_descriptors
                            734 ;	-----------------------------------------
   0AD0                     735 _setup_descriptors:
                    0002    736 	ar2 = 0x02
                    0003    737 	ar3 = 0x03
                    0004    738 	ar4 = 0x04
                    0005    739 	ar5 = 0x05
                    0006    740 	ar6 = 0x06
                    0007    741 	ar7 = 0x07
                    0000    742 	ar0 = 0x00
                    0001    743 	ar1 = 0x01
                            744 ;Initial/src/lib/usb_common.c:67: if (USBCS & bmHSM)
                            745 ;     genAssign
   0AD0 90 E6 80            746 	mov	dptr,#_USBCS
   0AD3 E0                  747 	movx	a,@dptr
                            748 ;     genAnd
                            749 ;	Peephole 105	removed redundant mov
   0AD4 FA                  750 	mov	r2,a
                            751 ;     genIfxJump
                            752 ;	Peephole 111	removed ljmp by inverse jump logic
   0AD5 30 E7 19            753 	jnb	acc.7,00102$
   0AD8                     754 00107$:
                            755 ;Initial/src/lib/usb_common.c:69: current_device_descr  = high_speed_device_descr;
                            756 ;     genAddrOf
   0AD8 75 23 0A            757 	mov	_current_device_descr,#_high_speed_device_descr
   0ADB 75 24 20            758 	mov	(_current_device_descr + 1),#(_high_speed_device_descr >> 8)
                            759 ;Initial/src/lib/usb_common.c:70: current_devqual_descr = high_speed_devqual_descr;
                            760 ;     genAddrOf
   0ADE 75 25 1C            761 	mov	_current_devqual_descr,#_high_speed_devqual_descr
   0AE1 75 26 20            762 	mov	(_current_devqual_descr + 1),#(_high_speed_devqual_descr >> 8)
                            763 ;Initial/src/lib/usb_common.c:71: current_config_descr  = high_speed_config_descr;
                            764 ;     genAddrOf
   0AE4 75 27 26            765 	mov	_current_config_descr,#_high_speed_config_descr
   0AE7 75 28 20            766 	mov	(_current_config_descr + 1),#(_high_speed_config_descr >> 8)
                            767 ;Initial/src/lib/usb_common.c:72: other_config_descr    = full_speed_config_descr;
                            768 ;     genAddrOf
   0AEA 75 29 74            769 	mov	_other_config_descr,#_full_speed_config_descr
   0AED 75 2A 20            770 	mov	(_other_config_descr + 1),#(_full_speed_config_descr >> 8)
                            771 ;	Peephole 112.b	changed ljmp to sjmp
                            772 ;	Peephole 251.b	replaced sjmp to ret with ret
   0AF0 22                  773 	ret
   0AF1                     774 00102$:
                            775 ;Initial/src/lib/usb_common.c:76: current_device_descr  = full_speed_device_descr;
                            776 ;     genAddrOf
   0AF1 75 23 58            777 	mov	_current_device_descr,#_full_speed_device_descr
   0AF4 75 24 20            778 	mov	(_current_device_descr + 1),#(_full_speed_device_descr >> 8)
                            779 ;Initial/src/lib/usb_common.c:77: current_devqual_descr = full_speed_devqual_descr;
                            780 ;     genAddrOf
   0AF7 75 25 6A            781 	mov	_current_devqual_descr,#_full_speed_devqual_descr
   0AFA 75 26 20            782 	mov	(_current_devqual_descr + 1),#(_full_speed_devqual_descr >> 8)
                            783 ;Initial/src/lib/usb_common.c:78: current_config_descr  = full_speed_config_descr;
                            784 ;     genAddrOf
   0AFD 75 27 74            785 	mov	_current_config_descr,#_full_speed_config_descr
   0B00 75 28 20            786 	mov	(_current_config_descr + 1),#(_full_speed_config_descr >> 8)
                            787 ;Initial/src/lib/usb_common.c:79: other_config_descr    = high_speed_config_descr;
                            788 ;     genAddrOf
   0B03 75 29 26            789 	mov	_other_config_descr,#_high_speed_config_descr
   0B06 75 2A 20            790 	mov	(_other_config_descr + 1),#(_high_speed_config_descr >> 8)
   0B09                     791 00104$:
   0B09 22                  792 	ret
                            793 ;------------------------------------------------------------
                            794 ;Allocation info for local variables in function 'isr_SUDAV'
                            795 ;------------------------------------------------------------
                            796 ;------------------------------------------------------------
                            797 ;Initial/src/lib/usb_common.c:84: isr_SUDAV (void) interrupt
                            798 ;	-----------------------------------------
                            799 ;	 function isr_SUDAV
                            800 ;	-----------------------------------------
   0B0A                     801 _isr_SUDAV:
                            802 ;Initial/src/lib/usb_common.c:86: clear_usb_irq ();
                            803 ;     genAnd
   0B0A 53 91 EF            804 	anl	_EXIF,#0xEF
                            805 ;     genAssign
   0B0D 75 A1 00            806 	mov	_INT2CLR,#0x00
                            807 ;Initial/src/lib/usb_common.c:87: _usb_got_SUDAV = 1;
                            808 ;     genAssign
   0B10 D2 03               809 	setb	__usb_got_SUDAV
   0B12                     810 00101$:
   0B12 32                  811 	reti
                            812 ;	eliminated unneeded push/pop psw
                            813 ;	eliminated unneeded push/pop dpl
                            814 ;	eliminated unneeded push/pop dph
                            815 ;	eliminated unneeded push/pop b
                            816 ;	eliminated unneeded push/pop acc
                            817 ;------------------------------------------------------------
                            818 ;Allocation info for local variables in function 'isr_USBRESET'
                            819 ;------------------------------------------------------------
                            820 ;------------------------------------------------------------
                            821 ;Initial/src/lib/usb_common.c:91: isr_USBRESET (void) interrupt
                            822 ;	-----------------------------------------
                            823 ;	 function isr_USBRESET
                            824 ;	-----------------------------------------
   0B13                     825 _isr_USBRESET:
   0B13 C0 E0               826 	push	acc
   0B15 C0 F0               827 	push	b
   0B17 C0 82               828 	push	dpl
   0B19 C0 83               829 	push	dph
   0B1B C0 02               830 	push	(0+2)
   0B1D C0 03               831 	push	(0+3)
   0B1F C0 04               832 	push	(0+4)
   0B21 C0 05               833 	push	(0+5)
   0B23 C0 06               834 	push	(0+6)
   0B25 C0 07               835 	push	(0+7)
   0B27 C0 00               836 	push	(0+0)
   0B29 C0 01               837 	push	(0+1)
   0B2B C0 D0               838 	push	psw
   0B2D 75 D0 00            839 	mov	psw,#0x00
                            840 ;Initial/src/lib/usb_common.c:93: clear_usb_irq ();
                            841 ;     genAnd
   0B30 53 91 EF            842 	anl	_EXIF,#0xEF
                            843 ;     genAssign
   0B33 75 A1 00            844 	mov	_INT2CLR,#0x00
                            845 ;Initial/src/lib/usb_common.c:94: setup_descriptors ();
                            846 ;     genCall
   0B36 12 0A D0            847 	lcall	_setup_descriptors
   0B39                     848 00101$:
   0B39 D0 D0               849 	pop	psw
   0B3B D0 01               850 	pop	(0+1)
   0B3D D0 00               851 	pop	(0+0)
   0B3F D0 07               852 	pop	(0+7)
   0B41 D0 06               853 	pop	(0+6)
   0B43 D0 05               854 	pop	(0+5)
   0B45 D0 04               855 	pop	(0+4)
   0B47 D0 03               856 	pop	(0+3)
   0B49 D0 02               857 	pop	(0+2)
   0B4B D0 83               858 	pop	dph
   0B4D D0 82               859 	pop	dpl
   0B4F D0 F0               860 	pop	b
   0B51 D0 E0               861 	pop	acc
   0B53 32                  862 	reti
                            863 ;------------------------------------------------------------
                            864 ;Allocation info for local variables in function 'isr_HIGHSPEED'
                            865 ;------------------------------------------------------------
                            866 ;------------------------------------------------------------
                            867 ;Initial/src/lib/usb_common.c:98: isr_HIGHSPEED (void) interrupt
                            868 ;	-----------------------------------------
                            869 ;	 function isr_HIGHSPEED
                            870 ;	-----------------------------------------
   0B54                     871 _isr_HIGHSPEED:
   0B54 C0 E0               872 	push	acc
   0B56 C0 F0               873 	push	b
   0B58 C0 82               874 	push	dpl
   0B5A C0 83               875 	push	dph
   0B5C C0 02               876 	push	(0+2)
   0B5E C0 03               877 	push	(0+3)
   0B60 C0 04               878 	push	(0+4)
   0B62 C0 05               879 	push	(0+5)
   0B64 C0 06               880 	push	(0+6)
   0B66 C0 07               881 	push	(0+7)
   0B68 C0 00               882 	push	(0+0)
   0B6A C0 01               883 	push	(0+1)
   0B6C C0 D0               884 	push	psw
   0B6E 75 D0 00            885 	mov	psw,#0x00
                            886 ;Initial/src/lib/usb_common.c:100: clear_usb_irq ();
                            887 ;     genAnd
   0B71 53 91 EF            888 	anl	_EXIF,#0xEF
                            889 ;     genAssign
   0B74 75 A1 00            890 	mov	_INT2CLR,#0x00
                            891 ;Initial/src/lib/usb_common.c:101: setup_descriptors ();
                            892 ;     genCall
   0B77 12 0A D0            893 	lcall	_setup_descriptors
   0B7A                     894 00101$:
   0B7A D0 D0               895 	pop	psw
   0B7C D0 01               896 	pop	(0+1)
   0B7E D0 00               897 	pop	(0+0)
   0B80 D0 07               898 	pop	(0+7)
   0B82 D0 06               899 	pop	(0+6)
   0B84 D0 05               900 	pop	(0+5)
   0B86 D0 04               901 	pop	(0+4)
   0B88 D0 03               902 	pop	(0+3)
   0B8A D0 02               903 	pop	(0+2)
   0B8C D0 83               904 	pop	dph
   0B8E D0 82               905 	pop	dpl
   0B90 D0 F0               906 	pop	b
   0B92 D0 E0               907 	pop	acc
   0B94 32                  908 	reti
                            909 ;------------------------------------------------------------
                            910 ;Allocation info for local variables in function 'usb_install_handlers'
                            911 ;------------------------------------------------------------
                            912 ;------------------------------------------------------------
                            913 ;Initial/src/lib/usb_common.c:105: usb_install_handlers (void)
                            914 ;	-----------------------------------------
                            915 ;	 function usb_install_handlers
                            916 ;	-----------------------------------------
   0B95                     917 _usb_install_handlers:
                            918 ;Initial/src/lib/usb_common.c:107: setup_descriptors ();	    // ensure that they're set before use
                            919 ;     genCall
   0B95 12 0A D0            920 	lcall	_setup_descriptors
                            921 ;Initial/src/lib/usb_common.c:109: hook_uv (UV_SUDAV,     (unsigned short) isr_SUDAV);
                            922 ;     genCast
   0B98 75 2B 0A            923 	mov	_hook_uv_PARM_2,#_isr_SUDAV
   0B9B 75 2C 0B            924 	mov	(_hook_uv_PARM_2 + 1),#(_isr_SUDAV >> 8)
                            925 ;     genCall
   0B9E 75 82 00            926 	mov	dpl,#0x00
   0BA1 12 07 5D            927 	lcall	_hook_uv
                            928 ;Initial/src/lib/usb_common.c:110: hook_uv (UV_USBRESET,  (unsigned short) isr_USBRESET);
                            929 ;     genCast
   0BA4 75 2B 13            930 	mov	_hook_uv_PARM_2,#_isr_USBRESET
   0BA7 75 2C 0B            931 	mov	(_hook_uv_PARM_2 + 1),#(_isr_USBRESET >> 8)
                            932 ;     genCall
   0BAA 75 82 10            933 	mov	dpl,#0x10
   0BAD 12 07 5D            934 	lcall	_hook_uv
                            935 ;Initial/src/lib/usb_common.c:111: hook_uv (UV_HIGHSPEED, (unsigned short) isr_HIGHSPEED);
                            936 ;     genCast
   0BB0 75 2B 54            937 	mov	_hook_uv_PARM_2,#_isr_HIGHSPEED
   0BB3 75 2C 0B            938 	mov	(_hook_uv_PARM_2 + 1),#(_isr_HIGHSPEED >> 8)
                            939 ;     genCall
   0BB6 75 82 14            940 	mov	dpl,#0x14
   0BB9 12 07 5D            941 	lcall	_hook_uv
                            942 ;Initial/src/lib/usb_common.c:113: USBIE = bmSUDAV | bmURES | bmHSGRANT;
                            943 ;     genAssign
   0BBC 90 E6 5C            944 	mov	dptr,#_USBIE
   0BBF 74 31               945 	mov	a,#0x31
   0BC1 F0                  946 	movx	@dptr,a
   0BC2                     947 00101$:
   0BC2 22                  948 	ret
                            949 ;------------------------------------------------------------
                            950 ;Allocation info for local variables in function 'plausible_endpoint'
                            951 ;------------------------------------------------------------
                            952 ;ep                        Allocated to registers r2 
                            953 ;------------------------------------------------------------
                            954 ;Initial/src/lib/usb_common.c:120: plausible_endpoint (unsigned char ep)
                            955 ;	-----------------------------------------
                            956 ;	 function plausible_endpoint
                            957 ;	-----------------------------------------
   0BC3                     958 _plausible_endpoint:
                            959 ;     genReceive
                            960 ;Initial/src/lib/usb_common.c:122: ep &= ~0x80;	// ignore direction bit
                            961 ;     genAnd
                            962 ;Initial/src/lib/usb_common.c:124: if (ep > 8)
                            963 ;     genCmpGt
                            964 ;     genCmp
                            965 ;     genIfxJump
                            966 ;	Peephole 108	removed ljmp by inverse jump logic
                            967 ;	Peephole 132.b	optimized genCmpGt by inverse logic (acc differs)
                            968 ;	Peephole 187	used a instead of ar2 for anl
   0BC3 E5 82               969 	mov	a,dpl
   0BC5 54 7F               970 	anl	a,#0x7F
   0BC7 FA                  971 	mov	r2,a
   0BC8 24 F7               972 	add	a,#0xff - 0x08
   0BCA 50 04               973 	jnc	00102$
   0BCC                     974 00109$:
                            975 ;Initial/src/lib/usb_common.c:125: return 0;
                            976 ;     genRet
   0BCC 75 82 00            977 	mov	dpl,#0x00
                            978 ;	Peephole 112.b	changed ljmp to sjmp
                            979 ;	Peephole 251.b	replaced sjmp to ret with ret
   0BCF 22                  980 	ret
   0BD0                     981 00102$:
                            982 ;Initial/src/lib/usb_common.c:127: if (ep == 1)
                            983 ;     genCmpEq
                            984 ;	Peephole 112.b	changed ljmp to sjmp
                            985 ;	Peephole 199	optimized misc jump sequence
   0BD0 BA 01 04            986 	cjne	r2,#0x01,00104$
                            987 ;00110$:
                            988 ;	Peephole 200	removed redundant sjmp
   0BD3                     989 00111$:
                            990 ;Initial/src/lib/usb_common.c:128: return 1;
                            991 ;     genRet
   0BD3 75 82 01            992 	mov	dpl,#0x01
                            993 ;	Peephole 112.b	changed ljmp to sjmp
                            994 ;	Peephole 251.b	replaced sjmp to ret with ret
   0BD6 22                  995 	ret
   0BD7                     996 00104$:
                            997 ;Initial/src/lib/usb_common.c:130: return (ep & 0x1) == 0;	// must be even
                            998 ;     genAnd
   0BD7 53 02 01            999 	anl	ar2,#0x01
                           1000 ;     genCmpEq
                           1001 ;	Peephole 241.c	optimized compare
   0BDA E4                 1002 	clr	a
   0BDB BA 00 01           1003 	cjne	r2,#0x00,00112$
   0BDE 04                 1004 	inc	a
   0BDF                    1005 00112$:
   0BDF                    1006 00113$:
   0BDF F5 82              1007 	mov	dpl,a
                           1008 ;     genRet
   0BE1                    1009 00105$:
   0BE1 22                 1010 	ret
                           1011 ;------------------------------------------------------------
                           1012 ;Allocation info for local variables in function 'epcs'
                           1013 ;------------------------------------------------------------
                           1014 ;ep                        Allocated to registers r2 
                           1015 ;------------------------------------------------------------
                           1016 ;Initial/src/lib/usb_common.c:137: epcs (unsigned char ep)
                           1017 ;	-----------------------------------------
                           1018 ;	 function epcs
                           1019 ;	-----------------------------------------
   0BE2                    1020 _epcs:
                           1021 ;     genReceive
   0BE2 AA 82              1022 	mov	r2,dpl
                           1023 ;Initial/src/lib/usb_common.c:139: if (ep == 0x01)		// ep1 has different in and out CS regs
                           1024 ;     genCmpEq
                           1025 ;	Peephole 112.b	changed ljmp to sjmp
                           1026 ;	Peephole 199	optimized misc jump sequence
   0BE4 BA 01 0C           1027 	cjne	r2,#0x01,00102$
                           1028 ;00112$:
                           1029 ;	Peephole 200	removed redundant sjmp
   0BE7                    1030 00113$:
                           1031 ;Initial/src/lib/usb_common.c:140: return EP1OUTCS;
                           1032 ;     genAssign
   0BE7 90 E6 A1           1033 	mov	dptr,#_EP1OUTCS
   0BEA E0                 1034 	movx	a,@dptr
   0BEB FB                 1035 	mov	r3,a
                           1036 ;     genCast
   0BEC 7C 00              1037 	mov	r4,#0x00
                           1038 ;     genRet
   0BEE 8B 82              1039 	mov	dpl,r3
   0BF0 8C 83              1040 	mov	dph,r4
                           1041 ;	Peephole 112.b	changed ljmp to sjmp
                           1042 ;	Peephole 251.b	replaced sjmp to ret with ret
   0BF2 22                 1043 	ret
   0BF3                    1044 00102$:
                           1045 ;Initial/src/lib/usb_common.c:142: if (ep == 0x81)
                           1046 ;     genCmpEq
                           1047 ;	Peephole 112.b	changed ljmp to sjmp
                           1048 ;	Peephole 199	optimized misc jump sequence
   0BF3 BA 81 0C           1049 	cjne	r2,#0x81,00104$
                           1050 ;00114$:
                           1051 ;	Peephole 200	removed redundant sjmp
   0BF6                    1052 00115$:
                           1053 ;Initial/src/lib/usb_common.c:143: return EP1INCS;
                           1054 ;     genAssign
   0BF6 90 E6 A2           1055 	mov	dptr,#_EP1INCS
   0BF9 E0                 1056 	movx	a,@dptr
   0BFA FB                 1057 	mov	r3,a
                           1058 ;     genCast
   0BFB 7C 00              1059 	mov	r4,#0x00
                           1060 ;     genRet
   0BFD 8B 82              1061 	mov	dpl,r3
   0BFF 8C 83              1062 	mov	dph,r4
                           1063 ;	Peephole 112.b	changed ljmp to sjmp
                           1064 ;	Peephole 251.b	replaced sjmp to ret with ret
   0C01 22                 1065 	ret
   0C02                    1066 00104$:
                           1067 ;Initial/src/lib/usb_common.c:145: ep &= ~0x80;			// ignore direction bit
                           1068 ;     genAnd
   0C02 53 02 7F           1069 	anl	ar2,#0x7F
                           1070 ;Initial/src/lib/usb_common.c:147: if (ep == 0x00)		// ep0
                           1071 ;     genCmpEq
                           1072 ;	Peephole 112.b	changed ljmp to sjmp
                           1073 ;	Peephole 199	optimized misc jump sequence
   0C05 BA 00 0C           1074 	cjne	r2,#0x00,00106$
                           1075 ;00116$:
                           1076 ;	Peephole 200	removed redundant sjmp
   0C08                    1077 00117$:
                           1078 ;Initial/src/lib/usb_common.c:148: return EP0CS;
                           1079 ;     genAssign
   0C08 90 E6 A0           1080 	mov	dptr,#_EP0CS
   0C0B E0                 1081 	movx	a,@dptr
   0C0C FB                 1082 	mov	r3,a
                           1083 ;     genCast
   0C0D 7C 00              1084 	mov	r4,#0x00
                           1085 ;     genRet
   0C0F 8B 82              1086 	mov	dpl,r3
   0C11 8C 83              1087 	mov	dph,r4
                           1088 ;	Peephole 112.b	changed ljmp to sjmp
                           1089 ;	Peephole 251.b	replaced sjmp to ret with ret
   0C13 22                 1090 	ret
   0C14                    1091 00106$:
                           1092 ;Initial/src/lib/usb_common.c:150: return EP2CS + (ep >> 1);	// 2, 4, 6, 8 are consecutive
                           1093 ;     genAssign
   0C14 90 E6 A3           1094 	mov	dptr,#_EP2CS
   0C17 E0                 1095 	movx	a,@dptr
   0C18 FB                 1096 	mov	r3,a
                           1097 ;     genCast
   0C19 7C 00              1098 	mov	r4,#0x00
                           1099 ;     genRightShift
                           1100 ;     genRightShiftLiteral
                           1101 ;     genrshOne
   0C1B EA                 1102 	mov	a,r2
   0C1C C3                 1103 	clr	c
   0C1D 13                 1104 	rrc	a
   0C1E FA                 1105 	mov	r2,a
                           1106 ;     genCast
   0C1F 7D 00              1107 	mov	r5,#0x00
                           1108 ;     genPlus
                           1109 ;	Peephole 236.g	used r2 instead of ar2
   0C21 EA                 1110 	mov	a,r2
                           1111 ;	Peephole 236.a	used r3 instead of ar3
   0C22 2B                 1112 	add	a,r3
   0C23 FB                 1113 	mov	r3,a
                           1114 ;	Peephole 236.g	used r5 instead of ar5
   0C24 ED                 1115 	mov	a,r5
                           1116 ;	Peephole 236.b	used r4 instead of ar4
   0C25 3C                 1117 	addc	a,r4
                           1118 ;     genCast
                           1119 ;     genRet
                           1120 ;	Peephole 235	loading dph directly from a(ccumulator), r4 not set
   0C26 8B 82              1121 	mov	dpl,r3
   0C28 F5 83              1122 	mov	dph,a
   0C2A                    1123 00107$:
   0C2A 22                 1124 	ret
                           1125 ;------------------------------------------------------------
                           1126 ;Allocation info for local variables in function 'usb_handle_set_feature'
                           1127 ;------------------------------------------------------------
                           1128 ;------------------------------------------------------------
                           1129 ;Initial/src/lib/usb_common.c:153: void usb_handle_set_feature(void)
                           1130 ;	-----------------------------------------
                           1131 ;	 function usb_handle_set_feature
                           1132 ;	-----------------------------------------
   0C2B                    1133 _usb_handle_set_feature:
                           1134 ;Initial/src/lib/usb_common.c:155: switch (bRequestType & bmRT_RECIP_MASK)
                           1135 ;     genPointerGet
                           1136 ;     genFarPointerGet
   0C2B 90 E6 B8           1137 	mov	dptr,#_SETUPDAT
   0C2E E0                 1138 	movx	a,@dptr
   0C2F FA                 1139 	mov	r2,a
                           1140 ;     genAnd
   0C30 53 02 1F           1141 	anl	ar2,#0x1F
                           1142 ;     genCmpEq
   0C33 BA 00 02           1143 	cjne	r2,#0x00,00121$
                           1144 ;	Peephole 112.b	changed ljmp to sjmp
   0C36 80 05              1145 	sjmp	00101$
   0C38                    1146 00121$:
                           1147 ;     genCmpEq
                           1148 ;	Peephole 112.b	changed ljmp to sjmp
                           1149 ;Initial/src/lib/usb_common.c:157: case bmRT_RECIP_DEVICE:
                           1150 ;	Peephole 112.b	changed ljmp to sjmp
                           1151 ;	Peephole 199	optimized misc jump sequence
   0C38 BA 02 5B           1152 	cjne	r2,#0x02,00112$
   0C3B 80 12              1153 	sjmp	00106$
                           1154 ;00122$:
   0C3D                    1155 00101$:
                           1156 ;Initial/src/lib/usb_common.c:158: switch (wValueL)
                           1157 ;     genPointerGet
                           1158 ;     genFarPointerGet
   0C3D 90 E6 BA           1159 	mov	dptr,#(_SETUPDAT + 0x0002)
   0C40 E0                 1160 	movx	a,@dptr
   0C41 FA                 1161 	mov	r2,a
                           1162 ;     genCmpEq
   0C42 BA 01 02           1163 	cjne	r2,#0x01,00123$
                           1164 ;	Peephole 112.b	changed ljmp to sjmp
   0C45 80 05              1165 	sjmp	00104$
   0C47                    1166 00123$:
                           1167 ;     genCmpEq
   0C47 BA 02 02           1168 	cjne	r2,#0x02,00124$
                           1169 ;	Peephole 112.b	changed ljmp to sjmp
   0C4A 80 03              1170 	sjmp	00106$
   0C4C                    1171 00124$:
                           1172 ;Initial/src/lib/usb_common.c:165: default:
   0C4C                    1173 00104$:
                           1174 ;Initial/src/lib/usb_common.c:166: fx2_stall_ep0 ();
                           1175 ;     genCall
   0C4C 12 05 47           1176 	lcall	_fx2_stall_ep0
                           1177 ;Initial/src/lib/usb_common.c:170: case bmRT_RECIP_ENDPOINT:
   0C4F                    1178 00106$:
                           1179 ;Initial/src/lib/usb_common.c:171: switch (wValueL)
                           1180 ;     genPointerGet
                           1181 ;     genFarPointerGet
   0C4F 90 E6 BA           1182 	mov	dptr,#(_SETUPDAT + 0x0002)
   0C52 E0                 1183 	movx	a,@dptr
                           1184 ;     genCmpEq
                           1185 ;	Peephole 115.b	jump optimization
   0C53 FA                 1186 	mov	r2,a
   0C54 60 02              1187 	jz	00126$
   0C56                    1188 00125$:
                           1189 ;	Peephole 112.b	changed ljmp to sjmp
   0C56 80 3E              1190 	sjmp	00112$
   0C58                    1191 00126$:
                           1192 ;Initial/src/lib/usb_common.c:174: if (plausible_endpoint (wIndexL))
                           1193 ;     genPointerGet
                           1194 ;     genFarPointerGet
   0C58 90 E6 BC           1195 	mov	dptr,#(_SETUPDAT + 0x0004)
   0C5B E0                 1196 	movx	a,@dptr
                           1197 ;     genCall
                           1198 ;	Peephole 244.c	loading dpl from a instead of r2
   0C5C FA                 1199 	mov	r2,a
   0C5D F5 82              1200 	mov	dpl,a
   0C5F 12 0B C3           1201 	lcall	_plausible_endpoint
   0C62 E5 82              1202 	mov	a,dpl
                           1203 ;     genIfx
                           1204 ;     genIfxJump
                           1205 ;	Peephole 110	removed ljmp by inverse jump logic
   0C64 60 2D              1206 	jz	00109$
   0C66                    1207 00127$:
                           1208 ;Initial/src/lib/usb_common.c:175: *epcs (wIndexL) |= bmEPSTALL;
                           1209 ;     genPointerGet
                           1210 ;     genFarPointerGet
   0C66 90 E6 BC           1211 	mov	dptr,#(_SETUPDAT + 0x0004)
   0C69 E0                 1212 	movx	a,@dptr
                           1213 ;     genCall
                           1214 ;	Peephole 244.c	loading dpl from a instead of r2
   0C6A FA                 1215 	mov	r2,a
   0C6B F5 82              1216 	mov	dpl,a
   0C6D 12 0B E2           1217 	lcall	_epcs
   0C70 AA 82              1218 	mov	r2,dpl
   0C72 AB 83              1219 	mov	r3,dph
                           1220 ;     genPointerGet
                           1221 ;     genFarPointerGet
   0C74 90 E6 BC           1222 	mov	dptr,#(_SETUPDAT + 0x0004)
   0C77 E0                 1223 	movx	a,@dptr
                           1224 ;     genCall
                           1225 ;	Peephole 244.c	loading dpl from a instead of r4
   0C78 FC                 1226 	mov	r4,a
   0C79 F5 82              1227 	mov	dpl,a
   0C7B C0 02              1228 	push	ar2
   0C7D C0 03              1229 	push	ar3
   0C7F 12 0B E2           1230 	lcall	_epcs
   0C82 D0 03              1231 	pop	ar3
   0C84 D0 02              1232 	pop	ar2
                           1233 ;     genPointerGet
                           1234 ;     genFarPointerGet
   0C86 E0                 1235 	movx	a,@dptr
   0C87 FC                 1236 	mov	r4,a
                           1237 ;     genOr
   0C88 43 04 01           1238 	orl	ar4,#0x01
                           1239 ;     genPointerSet
                           1240 ;     genFarPointerSet
   0C8B 8A 82              1241 	mov	dpl,r2
   0C8D 8B 83              1242 	mov	dph,r3
   0C8F EC                 1243 	mov	a,r4
   0C90 F0                 1244 	movx	@dptr,a
                           1245 ;	Peephole 112.b	changed ljmp to sjmp
   0C91 80 03              1246 	sjmp	00112$
   0C93                    1247 00109$:
                           1248 ;Initial/src/lib/usb_common.c:177: fx2_stall_ep0 ();
                           1249 ;     genCall
   0C93 12 05 47           1250 	lcall	_fx2_stall_ep0
                           1251 ;Initial/src/lib/usb_common.c:181: default:
   0C96                    1252 00112$:
                           1253 ;Initial/src/lib/usb_common.c:182: fx2_stall_ep0 ();
                           1254 ;     genCall
                           1255 ;Initial/src/lib/usb_common.c:184: }
                           1256 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0C96 02 05 47           1257 	ljmp	_fx2_stall_ep0
                           1258 ;------------------------------------------------------------
                           1259 ;Allocation info for local variables in function 'usb_handle_clear_feature'
                           1260 ;------------------------------------------------------------
                           1261 ;------------------------------------------------------------
                           1262 ;Initial/src/lib/usb_common.c:187: void usb_handle_clear_feature(void)
                           1263 ;	-----------------------------------------
                           1264 ;	 function usb_handle_clear_feature
                           1265 ;	-----------------------------------------
   0C99                    1266 _usb_handle_clear_feature:
                           1267 ;Initial/src/lib/usb_common.c:189: switch (bRequestType & bmRT_RECIP_MASK)
                           1268 ;     genPointerGet
                           1269 ;     genFarPointerGet
   0C99 90 E6 B8           1270 	mov	dptr,#_SETUPDAT
   0C9C E0                 1271 	movx	a,@dptr
   0C9D FA                 1272 	mov	r2,a
                           1273 ;     genAnd
   0C9E 53 02 1F           1274 	anl	ar2,#0x1F
                           1275 ;     genCmpEq
   0CA1 BA 00 02           1276 	cjne	r2,#0x00,00118$
                           1277 ;	Peephole 112.b	changed ljmp to sjmp
   0CA4 80 05              1278 	sjmp	00101$
   0CA6                    1279 00118$:
                           1280 ;     genCmpEq
                           1281 ;	Peephole 112.b	changed ljmp to sjmp
                           1282 ;Initial/src/lib/usb_common.c:192: case bmRT_RECIP_DEVICE:
                           1283 ;	Peephole 112.b	changed ljmp to sjmp
                           1284 ;	Peephole 199	optimized misc jump sequence
   0CA6 BA 02 58           1285 	cjne	r2,#0x02,00110$
   0CA9 80 07              1286 	sjmp	00105$
                           1287 ;00119$:
   0CAB                    1288 00101$:
                           1289 ;Initial/src/lib/usb_common.c:193: switch (wValueL)
                           1290 ;     genPointerGet
                           1291 ;     genFarPointerGet
   0CAB 90 E6 BA           1292 	mov	dptr,#(_SETUPDAT + 0x0002)
   0CAE E0                 1293 	movx	a,@dptr
                           1294 ;Initial/src/lib/usb_common.c:197: fx2_stall_ep0 ();
                           1295 ;     genCall
                           1296 ;Initial/src/lib/usb_common.c:199: break;
                           1297 ;Initial/src/lib/usb_common.c:201: case bmRT_RECIP_ENDPOINT:
                           1298 ;	Peephole 112.b	changed ljmp to sjmp
                           1299 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1300 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0CAF 02 05 47           1301 	ljmp	_fx2_stall_ep0
   0CB2                    1302 00105$:
                           1303 ;Initial/src/lib/usb_common.c:202: if (wValueL == FS_ENDPOINT_HALT && plausible_endpoint (wIndexL))
                           1304 ;     genPointerGet
                           1305 ;     genFarPointerGet
   0CB2 90 E6 BA           1306 	mov	dptr,#(_SETUPDAT + 0x0002)
   0CB5 E0                 1307 	movx	a,@dptr
                           1308 ;     genCmpEq
                           1309 ;	Peephole 115.b	jump optimization
   0CB6 FA                 1310 	mov	r2,a
   0CB7 60 02              1311 	jz	00121$
   0CB9                    1312 00120$:
                           1313 ;	Peephole 112.b	changed ljmp to sjmp
   0CB9 80 43              1314 	sjmp	00107$
   0CBB                    1315 00121$:
                           1316 ;     genPointerGet
                           1317 ;     genFarPointerGet
   0CBB 90 E6 BC           1318 	mov	dptr,#(_SETUPDAT + 0x0004)
   0CBE E0                 1319 	movx	a,@dptr
                           1320 ;     genCall
                           1321 ;	Peephole 244.c	loading dpl from a instead of r2
   0CBF FA                 1322 	mov	r2,a
   0CC0 F5 82              1323 	mov	dpl,a
   0CC2 12 0B C3           1324 	lcall	_plausible_endpoint
   0CC5 E5 82              1325 	mov	a,dpl
                           1326 ;     genIfx
                           1327 ;     genIfxJump
                           1328 ;	Peephole 110	removed ljmp by inverse jump logic
   0CC7 60 35              1329 	jz	00107$
   0CC9                    1330 00122$:
                           1331 ;Initial/src/lib/usb_common.c:204: *epcs (wIndexL) &= ~bmEPSTALL;
                           1332 ;     genPointerGet
                           1333 ;     genFarPointerGet
   0CC9 90 E6 BC           1334 	mov	dptr,#(_SETUPDAT + 0x0004)
   0CCC E0                 1335 	movx	a,@dptr
                           1336 ;     genCall
                           1337 ;	Peephole 244.c	loading dpl from a instead of r2
   0CCD FA                 1338 	mov	r2,a
   0CCE F5 82              1339 	mov	dpl,a
   0CD0 12 0B E2           1340 	lcall	_epcs
   0CD3 AA 82              1341 	mov	r2,dpl
   0CD5 AB 83              1342 	mov	r3,dph
                           1343 ;     genPointerGet
                           1344 ;     genFarPointerGet
   0CD7 90 E6 BC           1345 	mov	dptr,#(_SETUPDAT + 0x0004)
   0CDA E0                 1346 	movx	a,@dptr
                           1347 ;     genCall
                           1348 ;	Peephole 244.c	loading dpl from a instead of r4
   0CDB FC                 1349 	mov	r4,a
   0CDC F5 82              1350 	mov	dpl,a
   0CDE C0 02              1351 	push	ar2
   0CE0 C0 03              1352 	push	ar3
   0CE2 12 0B E2           1353 	lcall	_epcs
   0CE5 D0 03              1354 	pop	ar3
   0CE7 D0 02              1355 	pop	ar2
                           1356 ;     genPointerGet
                           1357 ;     genFarPointerGet
   0CE9 E0                 1358 	movx	a,@dptr
   0CEA FC                 1359 	mov	r4,a
                           1360 ;     genAnd
   0CEB 53 04 FE           1361 	anl	ar4,#0xFE
                           1362 ;     genPointerSet
                           1363 ;     genFarPointerSet
   0CEE 8A 82              1364 	mov	dpl,r2
   0CF0 8B 83              1365 	mov	dph,r3
   0CF2 EC                 1366 	mov	a,r4
   0CF3 F0                 1367 	movx	@dptr,a
                           1368 ;Initial/src/lib/usb_common.c:205: fx2_reset_data_toggle (wIndexL);
                           1369 ;     genPointerGet
                           1370 ;     genFarPointerGet
   0CF4 90 E6 BC           1371 	mov	dptr,#(_SETUPDAT + 0x0004)
   0CF7 E0                 1372 	movx	a,@dptr
                           1373 ;     genCall
                           1374 ;	Peephole 244.c	loading dpl from a instead of r2
   0CF8 FA                 1375 	mov	r2,a
   0CF9 F5 82              1376 	mov	dpl,a
                           1377 ;	Peephole 112.b	changed ljmp to sjmp
                           1378 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1379 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0CFB 02 05 50           1380 	ljmp	_fx2_reset_data_toggle
   0CFE                    1381 00107$:
                           1382 ;Initial/src/lib/usb_common.c:208: fx2_stall_ep0 ();
                           1383 ;     genCall
                           1384 ;Initial/src/lib/usb_common.c:209: break;
                           1385 ;Initial/src/lib/usb_common.c:211: default:
                           1386 ;	Peephole 112.b	changed ljmp to sjmp
                           1387 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1388 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0CFE 02 05 47           1389 	ljmp	_fx2_stall_ep0
   0D01                    1390 00110$:
                           1391 ;Initial/src/lib/usb_common.c:212: fx2_stall_ep0 ();
                           1392 ;     genCall
                           1393 ;Initial/src/lib/usb_common.c:214: }
                           1394 ;	Peephole 253.c	replaced lcall with ljmp
   0D01 02 05 47           1395 	ljmp	_fx2_stall_ep0
   0D04                    1396 00112$:
   0D04 22                 1397 	ret
                           1398 ;------------------------------------------------------------
                           1399 ;Allocation info for local variables in function 'usb_handle_get_status'
                           1400 ;------------------------------------------------------------
                           1401 ;------------------------------------------------------------
                           1402 ;Initial/src/lib/usb_common.c:217: void usb_handle_get_status(void)
                           1403 ;	-----------------------------------------
                           1404 ;	 function usb_handle_get_status
                           1405 ;	-----------------------------------------
   0D05                    1406 _usb_handle_get_status:
                           1407 ;Initial/src/lib/usb_common.c:219: switch (bRequestType & bmRT_RECIP_MASK)
                           1408 ;     genPointerGet
                           1409 ;     genFarPointerGet
   0D05 90 E6 B8           1410 	mov	dptr,#_SETUPDAT
   0D08 E0                 1411 	movx	a,@dptr
   0D09 FA                 1412 	mov	r2,a
                           1413 ;     genAnd
   0D0A 53 02 1F           1414 	anl	ar2,#0x1F
                           1415 ;     genCmpEq
   0D0D BA 00 02           1416 	cjne	r2,#0x00,00115$
                           1417 ;	Peephole 112.b	changed ljmp to sjmp
   0D10 80 0A              1418 	sjmp	00101$
   0D12                    1419 00115$:
                           1420 ;     genCmpEq
   0D12 BA 01 02           1421 	cjne	r2,#0x01,00116$
                           1422 ;	Peephole 112.b	changed ljmp to sjmp
   0D15 80 1B              1423 	sjmp	00102$
   0D17                    1424 00116$:
                           1425 ;     genCmpEq
                           1426 ;	Peephole 112.b	changed ljmp to sjmp
                           1427 ;Initial/src/lib/usb_common.c:221: case bmRT_RECIP_DEVICE:
                           1428 ;	Peephole 112.b	changed ljmp to sjmp
                           1429 ;	Peephole 199	optimized misc jump sequence
   0D17 BA 02 61           1430 	cjne	r2,#0x02,00107$
   0D1A 80 2A              1431 	sjmp	00103$
                           1432 ;00117$:
   0D1C                    1433 00101$:
                           1434 ;Initial/src/lib/usb_common.c:222: EP0BUF[0] = bmGSDA_SELF_POWERED;	// FIXME app should handle
                           1435 ;     genPointerSet
                           1436 ;     genFarPointerSet
   0D1C 90 E7 40           1437 	mov	dptr,#_EP0BUF
   0D1F 74 01              1438 	mov	a,#0x01
   0D21 F0                 1439 	movx	@dptr,a
                           1440 ;Initial/src/lib/usb_common.c:223: EP0BUF[1] = 0;
                           1441 ;     genPointerSet
                           1442 ;     genFarPointerSet
   0D22 90 E7 41           1443 	mov	dptr,#(_EP0BUF + 0x0001)
                           1444 ;	Peephole 181	changed mov to clr
                           1445 ;Initial/src/lib/usb_common.c:224: EP0BCH = 0;
                           1446 ;     genAssign
                           1447 ;	Peephole 181	changed mov to clr
                           1448 ;	Peephole 219	removed redundant clear
   0D25 E4                 1449 	clr	a
   0D26 F0                 1450 	movx	@dptr,a
   0D27 90 E6 8A           1451 	mov	dptr,#_EP0BCH
   0D2A F0                 1452 	movx	@dptr,a
                           1453 ;Initial/src/lib/usb_common.c:225: EP0BCL = 2;
                           1454 ;     genAssign
   0D2B 90 E6 8B           1455 	mov	dptr,#_EP0BCL
   0D2E 74 02              1456 	mov	a,#0x02
   0D30 F0                 1457 	movx	@dptr,a
                           1458 ;Initial/src/lib/usb_common.c:226: break;
                           1459 ;Initial/src/lib/usb_common.c:228: case bmRT_RECIP_INTERFACE:
                           1460 ;	Peephole 112.b	changed ljmp to sjmp
                           1461 ;	Peephole 251.b	replaced sjmp to ret with ret
   0D31 22                 1462 	ret
   0D32                    1463 00102$:
                           1464 ;Initial/src/lib/usb_common.c:229: EP0BUF[0] = 0;
                           1465 ;     genPointerSet
                           1466 ;     genFarPointerSet
   0D32 90 E7 40           1467 	mov	dptr,#_EP0BUF
                           1468 ;	Peephole 181	changed mov to clr
                           1469 ;Initial/src/lib/usb_common.c:230: EP0BUF[1] = 0;
                           1470 ;     genPointerSet
                           1471 ;     genFarPointerSet
                           1472 ;	Peephole 181	changed mov to clr
                           1473 ;	Peephole 219	removed redundant clear
                           1474 ;Initial/src/lib/usb_common.c:231: EP0BCH = 0;
                           1475 ;     genAssign
                           1476 ;	Peephole 181	changed mov to clr
                           1477 ;	Peephole 219.a	removed redundant clear
   0D35 E4                 1478 	clr	a
   0D36 F0                 1479 	movx	@dptr,a
   0D37 90 E7 41           1480 	mov	dptr,#(_EP0BUF + 0x0001)
   0D3A F0                 1481 	movx	@dptr,a
   0D3B 90 E6 8A           1482 	mov	dptr,#_EP0BCH
   0D3E F0                 1483 	movx	@dptr,a
                           1484 ;Initial/src/lib/usb_common.c:232: EP0BCL = 2;
                           1485 ;     genAssign
   0D3F 90 E6 8B           1486 	mov	dptr,#_EP0BCL
   0D42 74 02              1487 	mov	a,#0x02
   0D44 F0                 1488 	movx	@dptr,a
                           1489 ;Initial/src/lib/usb_common.c:233: break;
                           1490 ;Initial/src/lib/usb_common.c:235: case bmRT_RECIP_ENDPOINT:
                           1491 ;	Peephole 112.b	changed ljmp to sjmp
                           1492 ;	Peephole 251.b	replaced sjmp to ret with ret
   0D45 22                 1493 	ret
   0D46                    1494 00103$:
                           1495 ;Initial/src/lib/usb_common.c:236: if (plausible_endpoint (wIndexL))
                           1496 ;     genPointerGet
                           1497 ;     genFarPointerGet
   0D46 90 E6 BC           1498 	mov	dptr,#(_SETUPDAT + 0x0004)
   0D49 E0                 1499 	movx	a,@dptr
                           1500 ;     genCall
                           1501 ;	Peephole 244.c	loading dpl from a instead of r2
   0D4A FA                 1502 	mov	r2,a
   0D4B F5 82              1503 	mov	dpl,a
   0D4D 12 0B C3           1504 	lcall	_plausible_endpoint
   0D50 E5 82              1505 	mov	a,dpl
                           1506 ;     genIfx
                           1507 ;     genIfxJump
                           1508 ;	Peephole 110	removed ljmp by inverse jump logic
   0D52 60 24              1509 	jz	00105$
   0D54                    1510 00118$:
                           1511 ;Initial/src/lib/usb_common.c:238: EP0BUF[0] = *epcs (wIndexL) & bmEPSTALL;
                           1512 ;     genPointerGet
                           1513 ;     genFarPointerGet
   0D54 90 E6 BC           1514 	mov	dptr,#(_SETUPDAT + 0x0004)
   0D57 E0                 1515 	movx	a,@dptr
                           1516 ;     genCall
                           1517 ;	Peephole 244.c	loading dpl from a instead of r2
   0D58 FA                 1518 	mov	r2,a
   0D59 F5 82              1519 	mov	dpl,a
   0D5B 12 0B E2           1520 	lcall	_epcs
                           1521 ;     genPointerGet
                           1522 ;     genFarPointerGet
   0D5E E0                 1523 	movx	a,@dptr
   0D5F FA                 1524 	mov	r2,a
                           1525 ;     genAnd
   0D60 53 02 01           1526 	anl	ar2,#0x01
                           1527 ;     genPointerSet
                           1528 ;     genFarPointerSet
   0D63 90 E7 40           1529 	mov	dptr,#_EP0BUF
   0D66 EA                 1530 	mov	a,r2
   0D67 F0                 1531 	movx	@dptr,a
                           1532 ;Initial/src/lib/usb_common.c:239: EP0BUF[1] = 0;
                           1533 ;     genPointerSet
                           1534 ;     genFarPointerSet
   0D68 90 E7 41           1535 	mov	dptr,#(_EP0BUF + 0x0001)
                           1536 ;	Peephole 181	changed mov to clr
                           1537 ;Initial/src/lib/usb_common.c:240: EP0BCH = 0;
                           1538 ;     genAssign
                           1539 ;	Peephole 181	changed mov to clr
                           1540 ;	Peephole 219	removed redundant clear
   0D6B E4                 1541 	clr	a
   0D6C F0                 1542 	movx	@dptr,a
   0D6D 90 E6 8A           1543 	mov	dptr,#_EP0BCH
   0D70 F0                 1544 	movx	@dptr,a
                           1545 ;Initial/src/lib/usb_common.c:241: EP0BCL = 2;
                           1546 ;     genAssign
   0D71 90 E6 8B           1547 	mov	dptr,#_EP0BCL
   0D74 74 02              1548 	mov	a,#0x02
   0D76 F0                 1549 	movx	@dptr,a
                           1550 ;	Peephole 112.b	changed ljmp to sjmp
                           1551 ;	Peephole 251.b	replaced sjmp to ret with ret
   0D77 22                 1552 	ret
   0D78                    1553 00105$:
                           1554 ;Initial/src/lib/usb_common.c:244: fx2_stall_ep0 ();
                           1555 ;     genCall
                           1556 ;Initial/src/lib/usb_common.c:245: break;
                           1557 ;Initial/src/lib/usb_common.c:247: default:
                           1558 ;	Peephole 112.b	changed ljmp to sjmp
                           1559 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1560 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0D78 02 05 47           1561 	ljmp	_fx2_stall_ep0
   0D7B                    1562 00107$:
                           1563 ;Initial/src/lib/usb_common.c:248: fx2_stall_ep0 ();
                           1564 ;     genCall
                           1565 ;Initial/src/lib/usb_common.c:250: }
                           1566 ;	Peephole 253.c	replaced lcall with ljmp
   0D7B 02 05 47           1567 	ljmp	_fx2_stall_ep0
   0D7E                    1568 00109$:
   0D7E 22                 1569 	ret
                           1570 ;------------------------------------------------------------
                           1571 ;Allocation info for local variables in function 'usb_handle_get_descriptor'
                           1572 ;------------------------------------------------------------
                           1573 ;p                         Allocated to registers r2 r3 
                           1574 ;------------------------------------------------------------
                           1575 ;Initial/src/lib/usb_common.c:253: void usb_handle_get_descriptor(void)
                           1576 ;	-----------------------------------------
                           1577 ;	 function usb_handle_get_descriptor
                           1578 ;	-----------------------------------------
   0D7F                    1579 _usb_handle_get_descriptor:
                           1580 ;Initial/src/lib/usb_common.c:255: switch (wValueH)
                           1581 ;     genPointerGet
                           1582 ;     genFarPointerGet
   0D7F 90 E6 BB           1583 	mov	dptr,#(_SETUPDAT + 0x0003)
   0D82 E0                 1584 	movx	a,@dptr
   0D83 FA                 1585 	mov	r2,a
                           1586 ;     genCmpEq
   0D84 BA 01 02           1587 	cjne	r2,#0x01,00127$
                           1588 ;	Peephole 112.b	changed ljmp to sjmp
   0D87 80 17              1589 	sjmp	00101$
   0D89                    1590 00127$:
                           1591 ;     genCmpEq
   0D89 BA 02 02           1592 	cjne	r2,#0x02,00128$
                           1593 ;	Peephole 112.b	changed ljmp to sjmp
   0D8C 80 3C              1594 	sjmp	00105$
   0D8E                    1595 00128$:
                           1596 ;     genCmpEq
   0D8E BA 03 02           1597 	cjne	r2,#0x03,00129$
                           1598 ;	Peephole 112.b	changed ljmp to sjmp
   0D91 80 61              1599 	sjmp	00113$
   0D93                    1600 00129$:
                           1601 ;     genCmpEq
   0D93 BA 06 02           1602 	cjne	r2,#0x06,00130$
                           1603 ;	Peephole 112.b	changed ljmp to sjmp
   0D96 80 1D              1604 	sjmp	00102$
   0D98                    1605 00130$:
                           1606 ;     genCmpEq
   0D98 BA 07 02           1607 	cjne	r2,#0x07,00131$
                           1608 ;	Peephole 112.b	changed ljmp to sjmp
   0D9B 80 42              1609 	sjmp	00110$
   0D9D                    1610 00131$:
   0D9D 02 0E 2F           1611 	ljmp	00117$
                           1612 ;Initial/src/lib/usb_common.c:258: case DT_DEVICE:
   0DA0                    1613 00101$:
                           1614 ;Initial/src/lib/usb_common.c:259: SUDPTRH = MSB (current_device_descr);
                           1615 ;     genCast
   0DA0 AA 23              1616 	mov	r2,_current_device_descr
   0DA2 AB 24              1617 	mov	r3,(_current_device_descr + 1)
                           1618 ;     genRightShift
                           1619 ;     genRightShiftLiteral
                           1620 ;     genrshTwo
   0DA4 8B 04              1621 	mov	ar4,r3
   0DA6 7D 00              1622 	mov	r5,#0x00
                           1623 ;     genCast
   0DA8 90 E6 B3           1624 	mov	dptr,#_SUDPTRH
   0DAB EC                 1625 	mov	a,r4
   0DAC F0                 1626 	movx	@dptr,a
                           1627 ;Initial/src/lib/usb_common.c:260: SUDPTRL = LSB (current_device_descr);
                           1628 ;     genAnd
   0DAD 7B 00              1629 	mov	r3,#0x00
                           1630 ;     genCast
   0DAF 90 E6 B4           1631 	mov	dptr,#_SUDPTRL
   0DB2 EA                 1632 	mov	a,r2
   0DB3 F0                 1633 	movx	@dptr,a
                           1634 ;Initial/src/lib/usb_common.c:261: break;
                           1635 ;	Peephole 251.a	replaced ljmp to ret with ret
   0DB4 22                 1636 	ret
                           1637 ;Initial/src/lib/usb_common.c:263: case DT_DEVQUAL:
   0DB5                    1638 00102$:
                           1639 ;Initial/src/lib/usb_common.c:264: SUDPTRH = MSB (current_devqual_descr);
                           1640 ;     genCast
   0DB5 AA 25              1641 	mov	r2,_current_devqual_descr
   0DB7 AB 26              1642 	mov	r3,(_current_devqual_descr + 1)
                           1643 ;     genRightShift
                           1644 ;     genRightShiftLiteral
                           1645 ;     genrshTwo
   0DB9 8B 04              1646 	mov	ar4,r3
   0DBB 7D 00              1647 	mov	r5,#0x00
                           1648 ;     genCast
   0DBD 90 E6 B3           1649 	mov	dptr,#_SUDPTRH
   0DC0 EC                 1650 	mov	a,r4
   0DC1 F0                 1651 	movx	@dptr,a
                           1652 ;Initial/src/lib/usb_common.c:265: SUDPTRL = LSB (current_devqual_descr);
                           1653 ;     genAnd
   0DC2 7B 00              1654 	mov	r3,#0x00
                           1655 ;     genCast
   0DC4 90 E6 B4           1656 	mov	dptr,#_SUDPTRL
   0DC7 EA                 1657 	mov	a,r2
   0DC8 F0                 1658 	movx	@dptr,a
                           1659 ;Initial/src/lib/usb_common.c:266: break;
                           1660 ;Initial/src/lib/usb_common.c:270: fx2_stall_ep0 ();
                           1661 ;	Peephole 112.b	changed ljmp to sjmp
                           1662 ;	Peephole 251.b	replaced sjmp to ret with ret
   0DC9 22                 1663 	ret
   0DCA                    1664 00105$:
                           1665 ;Initial/src/lib/usb_common.c:273: SUDPTRH = MSB (current_config_descr);
                           1666 ;     genCast
   0DCA AA 27              1667 	mov	r2,_current_config_descr
   0DCC AB 28              1668 	mov	r3,(_current_config_descr + 1)
                           1669 ;     genRightShift
                           1670 ;     genRightShiftLiteral
                           1671 ;     genrshTwo
   0DCE 8B 04              1672 	mov	ar4,r3
   0DD0 7D 00              1673 	mov	r5,#0x00
                           1674 ;     genCast
   0DD2 90 E6 B3           1675 	mov	dptr,#_SUDPTRH
   0DD5 EC                 1676 	mov	a,r4
   0DD6 F0                 1677 	movx	@dptr,a
                           1678 ;Initial/src/lib/usb_common.c:274: SUDPTRL = LSB (current_config_descr);
                           1679 ;     genAnd
   0DD7 7B 00              1680 	mov	r3,#0x00
                           1681 ;     genCast
   0DD9 90 E6 B4           1682 	mov	dptr,#_SUDPTRL
   0DDC EA                 1683 	mov	a,r2
   0DDD F0                 1684 	movx	@dptr,a
                           1685 ;Initial/src/lib/usb_common.c:276: break;
                           1686 ;Initial/src/lib/usb_common.c:280: fx2_stall_ep0 ();
                           1687 ;	Peephole 112.b	changed ljmp to sjmp
                           1688 ;	Peephole 251.b	replaced sjmp to ret with ret
   0DDE 22                 1689 	ret
   0DDF                    1690 00110$:
                           1691 ;Initial/src/lib/usb_common.c:283: SUDPTRH = MSB (other_config_descr);
                           1692 ;     genCast
   0DDF AA 29              1693 	mov	r2,_other_config_descr
   0DE1 AB 2A              1694 	mov	r3,(_other_config_descr + 1)
                           1695 ;     genRightShift
                           1696 ;     genRightShiftLiteral
                           1697 ;     genrshTwo
   0DE3 8B 04              1698 	mov	ar4,r3
   0DE5 7D 00              1699 	mov	r5,#0x00
                           1700 ;     genCast
   0DE7 90 E6 B3           1701 	mov	dptr,#_SUDPTRH
   0DEA EC                 1702 	mov	a,r4
   0DEB F0                 1703 	movx	@dptr,a
                           1704 ;Initial/src/lib/usb_common.c:284: SUDPTRL = LSB (other_config_descr);
                           1705 ;     genAnd
   0DEC 7B 00              1706 	mov	r3,#0x00
                           1707 ;     genCast
   0DEE 90 E6 B4           1708 	mov	dptr,#_SUDPTRL
   0DF1 EA                 1709 	mov	a,r2
   0DF2 F0                 1710 	movx	@dptr,a
                           1711 ;Initial/src/lib/usb_common.c:286: break;
                           1712 ;Initial/src/lib/usb_common.c:288: case DT_STRING:
                           1713 ;	Peephole 112.b	changed ljmp to sjmp
                           1714 ;	Peephole 251.b	replaced sjmp to ret with ret
   0DF3 22                 1715 	ret
   0DF4                    1716 00113$:
                           1717 ;Initial/src/lib/usb_common.c:289: if (wValueL >= nstring_descriptors)
                           1718 ;     genPointerGet
                           1719 ;     genFarPointerGet
   0DF4 90 E6 BA           1720 	mov	dptr,#(_SETUPDAT + 0x0002)
   0DF7 E0                 1721 	movx	a,@dptr
   0DF8 FA                 1722 	mov	r2,a
                           1723 ;     genAssign
   0DF9 90 20 86           1724 	mov	dptr,#_nstring_descriptors
   0DFC E0                 1725 	movx	a,@dptr
   0DFD FB                 1726 	mov	r3,a
                           1727 ;     genCmpLt
                           1728 ;     genCmp
   0DFE C3                 1729 	clr	c
   0DFF EA                 1730 	mov	a,r2
   0E00 9B                 1731 	subb	a,r3
                           1732 ;     genIfxJump
                           1733 ;	Peephole 112.b	changed ljmp to sjmp
                           1734 ;	Peephole 160	removed sjmp by inverse jump logic
   0E01 40 03              1735 	jc	00115$
   0E03                    1736 00132$:
                           1737 ;Initial/src/lib/usb_common.c:290: fx2_stall_ep0 ();
                           1738 ;     genCall
                           1739 ;	Peephole 112.b	changed ljmp to sjmp
                           1740 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1741 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0E03 02 05 47           1742 	ljmp	_fx2_stall_ep0
   0E06                    1743 00115$:
                           1744 ;Initial/src/lib/usb_common.c:293: xdata char *p = string_descriptors[wValueL];
                           1745 ;     genPointerGet
                           1746 ;     genFarPointerGet
   0E06 90 E6 BA           1747 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E09 E0                 1748 	movx	a,@dptr
                           1749 ;     genMult
                           1750 ;     genMultOneByte
                           1751 ;	Peephole 105	removed redundant mov
   0E0A FA                 1752 	mov	r2,a
   0E0B 75 F0 02           1753 	mov	b,#0x02
   0E0E A4                 1754 	mul	ab
                           1755 ;     genPlus
   0E0F 24 87              1756 	add	a,#_string_descriptors
   0E11 F5 82              1757 	mov	dpl,a
   0E13 74 20              1758 	mov	a,#(_string_descriptors >> 8)
   0E15 35 F0              1759 	addc	a,b
   0E17 F5 83              1760 	mov	dph,a
                           1761 ;     genPointerGet
                           1762 ;     genFarPointerGet
   0E19 E0                 1763 	movx	a,@dptr
   0E1A FA                 1764 	mov	r2,a
   0E1B A3                 1765 	inc	dptr
   0E1C E0                 1766 	movx	a,@dptr
   0E1D FB                 1767 	mov	r3,a
                           1768 ;Initial/src/lib/usb_common.c:294: SUDPTRH = MSB (p);
                           1769 ;     genCast
                           1770 ;     genRightShift
                           1771 ;     genRightShiftLiteral
                           1772 ;     genrshTwo
   0E1E 8B 04              1773 	mov	ar4,r3
   0E20 7D 00              1774 	mov	r5,#0x00
                           1775 ;     genCast
   0E22 90 E6 B3           1776 	mov	dptr,#_SUDPTRH
   0E25 EC                 1777 	mov	a,r4
   0E26 F0                 1778 	movx	@dptr,a
                           1779 ;Initial/src/lib/usb_common.c:295: SUDPTRL = LSB (p);
                           1780 ;     genAnd
   0E27 7B 00              1781 	mov	r3,#0x00
                           1782 ;     genCast
   0E29 90 E6 B4           1783 	mov	dptr,#_SUDPTRL
   0E2C EA                 1784 	mov	a,r2
   0E2D F0                 1785 	movx	@dptr,a
                           1786 ;Initial/src/lib/usb_common.c:297: break;
                           1787 ;Initial/src/lib/usb_common.c:299: default:
                           1788 ;	Peephole 112.b	changed ljmp to sjmp
                           1789 ;	Peephole 251.b	replaced sjmp to ret with ret
   0E2E 22                 1790 	ret
   0E2F                    1791 00117$:
                           1792 ;Initial/src/lib/usb_common.c:300: fx2_stall_ep0 ();	// invalid request
                           1793 ;     genCall
                           1794 ;Initial/src/lib/usb_common.c:302: }
                           1795 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0E2F 02 05 47           1796 	ljmp	_fx2_stall_ep0
                           1797 ;------------------------------------------------------------
                           1798 ;Allocation info for local variables in function 'usb_handle_IN_std_requests'
                           1799 ;------------------------------------------------------------
                           1800 ;------------------------------------------------------------
                           1801 ;Initial/src/lib/usb_common.c:305: void usb_handle_IN_std_requests(void)
                           1802 ;	-----------------------------------------
                           1803 ;	 function usb_handle_IN_std_requests
                           1804 ;	-----------------------------------------
   0E32                    1805 _usb_handle_IN_std_requests:
                           1806 ;Initial/src/lib/usb_common.c:307: switch (bRequest)
                           1807 ;     genPointerGet
                           1808 ;     genFarPointerGet
   0E32 90 E6 B9           1809 	mov	dptr,#(_SETUPDAT + 0x0001)
   0E35 E0                 1810 	movx	a,@dptr
                           1811 ;     genCmpEq
                           1812 ;	Peephole 112.b	changed ljmp to sjmp
                           1813 ;	Peephole 115.b	jump optimization
   0E36 FA                 1814 	mov	r2,a
   0E37 60 36              1815 	jz	00104$
   0E39                    1816 00114$:
                           1817 ;     genCmpEq
   0E39 BA 06 02           1818 	cjne	r2,#0x06,00115$
                           1819 ;	Peephole 112.b	changed ljmp to sjmp
   0E3C 80 2E              1820 	sjmp	00103$
   0E3E                    1821 00115$:
                           1822 ;     genCmpEq
   0E3E BA 08 02           1823 	cjne	r2,#0x08,00116$
                           1824 ;	Peephole 112.b	changed ljmp to sjmp
   0E41 80 05              1825 	sjmp	00101$
   0E43                    1826 00116$:
                           1827 ;     genCmpEq
                           1828 ;	Peephole 112.b	changed ljmp to sjmp
                           1829 ;Initial/src/lib/usb_common.c:310: case RQ_GET_CONFIG:
                           1830 ;	Peephole 112.b	changed ljmp to sjmp
                           1831 ;	Peephole 199	optimized misc jump sequence
   0E43 BA 0A 2C           1832 	cjne	r2,#0x0A,00106$
   0E46 80 12              1833 	sjmp	00102$
                           1834 ;00117$:
   0E48                    1835 00101$:
                           1836 ;Initial/src/lib/usb_common.c:311: EP0BUF[0] = _usb_config;	// FIXME app should handle
                           1837 ;     genPointerSet
                           1838 ;     genFarPointerSet
   0E48 90 E7 40           1839 	mov	dptr,#_EP0BUF
   0E4B E5 21              1840 	mov	a,__usb_config
   0E4D F0                 1841 	movx	@dptr,a
                           1842 ;Initial/src/lib/usb_common.c:312: EP0BCH = 0;
                           1843 ;     genAssign
   0E4E 90 E6 8A           1844 	mov	dptr,#_EP0BCH
                           1845 ;	Peephole 181	changed mov to clr
   0E51 E4                 1846 	clr	a
   0E52 F0                 1847 	movx	@dptr,a
                           1848 ;Initial/src/lib/usb_common.c:313: EP0BCL = 1;
                           1849 ;     genAssign
   0E53 90 E6 8B           1850 	mov	dptr,#_EP0BCL
   0E56 74 01              1851 	mov	a,#0x01
   0E58 F0                 1852 	movx	@dptr,a
                           1853 ;Initial/src/lib/usb_common.c:314: break;
                           1854 ;Initial/src/lib/usb_common.c:316: case RQ_GET_INTERFACE:
                           1855 ;	Peephole 112.b	changed ljmp to sjmp
                           1856 ;	Peephole 251.b	replaced sjmp to ret with ret
   0E59 22                 1857 	ret
   0E5A                    1858 00102$:
                           1859 ;Initial/src/lib/usb_common.c:317: EP0BUF[0] = _usb_alt_setting;	// FIXME app should handle
                           1860 ;     genPointerSet
                           1861 ;     genFarPointerSet
   0E5A 90 E7 40           1862 	mov	dptr,#_EP0BUF
   0E5D E5 22              1863 	mov	a,__usb_alt_setting
   0E5F F0                 1864 	movx	@dptr,a
                           1865 ;Initial/src/lib/usb_common.c:318: EP0BCH = 0;
                           1866 ;     genAssign
   0E60 90 E6 8A           1867 	mov	dptr,#_EP0BCH
                           1868 ;	Peephole 181	changed mov to clr
   0E63 E4                 1869 	clr	a
   0E64 F0                 1870 	movx	@dptr,a
                           1871 ;Initial/src/lib/usb_common.c:319: EP0BCL = 1;
                           1872 ;     genAssign
   0E65 90 E6 8B           1873 	mov	dptr,#_EP0BCL
   0E68 74 01              1874 	mov	a,#0x01
   0E6A F0                 1875 	movx	@dptr,a
                           1876 ;Initial/src/lib/usb_common.c:320: break;
                           1877 ;Initial/src/lib/usb_common.c:322: case RQ_GET_DESCR:
                           1878 ;	Peephole 112.b	changed ljmp to sjmp
                           1879 ;	Peephole 251.b	replaced sjmp to ret with ret
   0E6B 22                 1880 	ret
   0E6C                    1881 00103$:
                           1882 ;Initial/src/lib/usb_common.c:323: usb_handle_get_descriptor();
                           1883 ;     genCall
                           1884 ;Initial/src/lib/usb_common.c:324: break;
                           1885 ;Initial/src/lib/usb_common.c:326: case RQ_GET_STATUS:
                           1886 ;	Peephole 112.b	changed ljmp to sjmp
                           1887 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1888 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0E6C 02 0D 7F           1889 	ljmp	_usb_handle_get_descriptor
   0E6F                    1890 00104$:
                           1891 ;Initial/src/lib/usb_common.c:327: usb_handle_get_status();
                           1892 ;     genCall
                           1893 ;Initial/src/lib/usb_common.c:328: break;
                           1894 ;Initial/src/lib/usb_common.c:331: default:
                           1895 ;	Peephole 112.b	changed ljmp to sjmp
                           1896 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1897 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0E6F 02 0D 05           1898 	ljmp	_usb_handle_get_status
   0E72                    1899 00106$:
                           1900 ;Initial/src/lib/usb_common.c:332: fx2_stall_ep0 ();
                           1901 ;     genCall
                           1902 ;Initial/src/lib/usb_common.c:334: }
                           1903 ;	Peephole 253.c	replaced lcall with ljmp
   0E72 02 05 47           1904 	ljmp	_fx2_stall_ep0
   0E75                    1905 00108$:
   0E75 22                 1906 	ret
                           1907 ;------------------------------------------------------------
                           1908 ;Allocation info for local variables in function 'usb_handle_OUT_std_requests'
                           1909 ;------------------------------------------------------------
                           1910 ;------------------------------------------------------------
                           1911 ;Initial/src/lib/usb_common.c:337: void usb_handle_OUT_std_requests(void)
                           1912 ;	-----------------------------------------
                           1913 ;	 function usb_handle_OUT_std_requests
                           1914 ;	-----------------------------------------
   0E76                    1915 _usb_handle_OUT_std_requests:
                           1916 ;Initial/src/lib/usb_common.c:339: switch (bRequest)
                           1917 ;     genPointerGet
                           1918 ;     genFarPointerGet
   0E76 90 E6 B9           1919 	mov	dptr,#(_SETUPDAT + 0x0001)
   0E79 E0                 1920 	movx	a,@dptr
   0E7A FA                 1921 	mov	r2,a
                           1922 ;     genCmpEq
   0E7B BA 01 02           1923 	cjne	r2,#0x01,00117$
                           1924 ;	Peephole 112.b	changed ljmp to sjmp
   0E7E 80 27              1925 	sjmp	00103$
   0E80                    1926 00117$:
                           1927 ;     genCmpEq
   0E80 BA 03 02           1928 	cjne	r2,#0x03,00118$
                           1929 ;	Peephole 112.b	changed ljmp to sjmp
   0E83 80 25              1930 	sjmp	00104$
   0E85                    1931 00118$:
                           1932 ;     genCmpEq
   0E85 BA 05 02           1933 	cjne	r2,#0x05,00119$
                           1934 ;	Peephole 112.b	changed ljmp to sjmp
   0E88 80 23              1935 	sjmp	00107$
   0E8A                    1936 00119$:
                           1937 ;     genCmpEq
   0E8A BA 07 02           1938 	cjne	r2,#0x07,00120$
                           1939 ;	Peephole 112.b	changed ljmp to sjmp
   0E8D 80 1E              1940 	sjmp	00107$
   0E8F                    1941 00120$:
                           1942 ;     genCmpEq
   0E8F BA 09 02           1943 	cjne	r2,#0x09,00121$
                           1944 ;	Peephole 112.b	changed ljmp to sjmp
   0E92 80 05              1945 	sjmp	00101$
   0E94                    1946 00121$:
                           1947 ;     genCmpEq
                           1948 ;	Peephole 112.b	changed ljmp to sjmp
                           1949 ;Initial/src/lib/usb_common.c:342: case RQ_SET_CONFIG:
                           1950 ;	Peephole 112.b	changed ljmp to sjmp
                           1951 ;	Peephole 199	optimized misc jump sequence
   0E94 BA 0B 16           1952 	cjne	r2,#0x0B,00107$
   0E97 80 07              1953 	sjmp	00102$
                           1954 ;00122$:
   0E99                    1955 00101$:
                           1956 ;Initial/src/lib/usb_common.c:343: _usb_config = wValueL;		// FIXME app should handle
                           1957 ;     genPointerGet
                           1958 ;     genFarPointerGet
   0E99 90 E6 BA           1959 	mov	dptr,#(_SETUPDAT + 0x0002)
   0E9C E0                 1960 	movx	a,@dptr
   0E9D F5 21              1961 	mov	__usb_config,a
                           1962 ;Initial/src/lib/usb_common.c:344: break;
                           1963 ;Initial/src/lib/usb_common.c:346: case RQ_SET_INTERFACE:
                           1964 ;	Peephole 112.b	changed ljmp to sjmp
                           1965 ;	Peephole 251.b	replaced sjmp to ret with ret
   0E9F 22                 1966 	ret
   0EA0                    1967 00102$:
                           1968 ;Initial/src/lib/usb_common.c:347: _usb_alt_setting = wValueL;	// FIXME app should handle
                           1969 ;     genPointerGet
                           1970 ;     genFarPointerGet
   0EA0 90 E6 BA           1971 	mov	dptr,#(_SETUPDAT + 0x0002)
   0EA3 E0                 1972 	movx	a,@dptr
   0EA4 F5 22              1973 	mov	__usb_alt_setting,a
                           1974 ;Initial/src/lib/usb_common.c:348: break;
                           1975 ;Initial/src/lib/usb_common.c:350: case RQ_CLEAR_FEATURE:
                           1976 ;	Peephole 112.b	changed ljmp to sjmp
                           1977 ;	Peephole 251.b	replaced sjmp to ret with ret
   0EA6 22                 1978 	ret
   0EA7                    1979 00103$:
                           1980 ;Initial/src/lib/usb_common.c:351: usb_handle_clear_feature();
                           1981 ;     genCall
                           1982 ;Initial/src/lib/usb_common.c:352: break;
                           1983 ;Initial/src/lib/usb_common.c:354: case RQ_SET_FEATURE:
                           1984 ;	Peephole 112.b	changed ljmp to sjmp
                           1985 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1986 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0EA7 02 0C 99           1987 	ljmp	_usb_handle_clear_feature
   0EAA                    1988 00104$:
                           1989 ;Initial/src/lib/usb_common.c:355: usb_handle_set_feature();
                           1990 ;     genCall
                           1991 ;Initial/src/lib/usb_common.c:356: break;
                           1992 ;Initial/src/lib/usb_common.c:360: default:
                           1993 ;	Peephole 112.b	changed ljmp to sjmp
                           1994 ;	Peephole 251.b	replaced sjmp to ret with ret
                           1995 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0EAA 02 0C 2B           1996 	ljmp	_usb_handle_set_feature
   0EAD                    1997 00107$:
                           1998 ;Initial/src/lib/usb_common.c:361: fx2_stall_ep0 ();
                           1999 ;     genCall
                           2000 ;Initial/src/lib/usb_common.c:362: }
                           2001 ;	Peephole 253.c	replaced lcall with ljmp
   0EAD 02 05 47           2002 	ljmp	_fx2_stall_ep0
   0EB0                    2003 00109$:
   0EB0 22                 2004 	ret
                           2005 ;------------------------------------------------------------
                           2006 ;Allocation info for local variables in function 'usb_handle_std_requests'
                           2007 ;------------------------------------------------------------
                           2008 ;------------------------------------------------------------
                           2009 ;Initial/src/lib/usb_common.c:365: void usb_handle_std_requests(void)
                           2010 ;	-----------------------------------------
                           2011 ;	 function usb_handle_std_requests
                           2012 ;	-----------------------------------------
   0EB1                    2013 _usb_handle_std_requests:
                           2014 ;Initial/src/lib/usb_common.c:367: if ((bRequestType & bmRT_DIR_MASK) == bmRT_DIR_IN)
                           2015 ;     genPointerGet
                           2016 ;     genFarPointerGet
   0EB1 90 E6 B8           2017 	mov	dptr,#_SETUPDAT
   0EB4 E0                 2018 	movx	a,@dptr
   0EB5 FA                 2019 	mov	r2,a
                           2020 ;     genAnd
   0EB6 53 02 80           2021 	anl	ar2,#0x80
                           2022 ;     genCmpEq
                           2023 ;	Peephole 112.b	changed ljmp to sjmp
                           2024 ;	Peephole 199	optimized misc jump sequence
   0EB9 BA 80 03           2025 	cjne	r2,#0x80,00102$
                           2026 ;00107$:
                           2027 ;	Peephole 200	removed redundant sjmp
   0EBC                    2028 00108$:
                           2029 ;Initial/src/lib/usb_common.c:369: usb_handle_IN_std_requests();
                           2030 ;     genCall
                           2031 ;	Peephole 112.b	changed ljmp to sjmp
                           2032 ;	Peephole 251.b	replaced sjmp to ret with ret
                           2033 ;	Peephole 253.a	replaced lcall/ret with ljmp
   0EBC 02 0E 32           2034 	ljmp	_usb_handle_IN_std_requests
   0EBF                    2035 00102$:
                           2036 ;Initial/src/lib/usb_common.c:372: usb_handle_OUT_std_requests();
                           2037 ;     genCall
                           2038 ;	Peephole 253.c	replaced lcall with ljmp
   0EBF 02 0E 76           2039 	ljmp	_usb_handle_OUT_std_requests
   0EC2                    2040 00104$:
   0EC2 22                 2041 	ret
                           2042 ;------------------------------------------------------------
                           2043 ;Allocation info for local variables in function 'usb_handle_setup_packet'
                           2044 ;------------------------------------------------------------
                           2045 ;------------------------------------------------------------
                           2046 ;Initial/src/lib/usb_common.c:376: usb_handle_setup_packet (void)
                           2047 ;	-----------------------------------------
                           2048 ;	 function usb_handle_setup_packet
                           2049 ;	-----------------------------------------
   0EC3                    2050 _usb_handle_setup_packet:
                           2051 ;Initial/src/lib/usb_common.c:378: _usb_got_SUDAV = 0;
                           2052 ;     genAssign
   0EC3 C2 03              2053 	clr	__usb_got_SUDAV
                           2054 ;Initial/src/lib/usb_common.c:382: switch (bRequestType & bmRT_TYPE_MASK)
                           2055 ;     genPointerGet
                           2056 ;     genFarPointerGet
   0EC5 90 E6 B8           2057 	mov	dptr,#_SETUPDAT
   0EC8 E0                 2058 	movx	a,@dptr
   0EC9 FA                 2059 	mov	r2,a
                           2060 ;     genAnd
   0ECA 53 02 60           2061 	anl	ar2,#0x60
                           2062 ;     genCmpEq
   0ECD BA 00 02           2063 	cjne	r2,#0x00,00115$
                           2064 ;	Peephole 112.b	changed ljmp to sjmp
   0ED0 80 1E              2065 	sjmp	00106$
   0ED2                    2066 00115$:
                           2067 ;     genCmpEq
   0ED2 BA 20 02           2068 	cjne	r2,#0x20,00116$
                           2069 ;	Peephole 112.b	changed ljmp to sjmp
   0ED5 80 08              2070 	sjmp	00102$
   0ED7                    2071 00116$:
                           2072 ;     genCmpEq
   0ED7 BA 40 02           2073 	cjne	r2,#0x40,00117$
                           2074 ;	Peephole 112.b	changed ljmp to sjmp
   0EDA 80 08              2075 	sjmp	00103$
   0EDC                    2076 00117$:
                           2077 ;     genCmpEq
                           2078 ;	Peephole 112.b	changed ljmp to sjmp
                           2079 ;	Peephole 199	optimized misc jump sequence
   0EDC BA 60 16           2080 	cjne	r2,#0x60,00107$
                           2081 ;00118$:
                           2082 ;	Peephole 200	removed redundant sjmp
   0EDF                    2083 00119$:
                           2084 ;Initial/src/lib/usb_common.c:386: case bmRT_TYPE_RESERVED:
   0EDF                    2085 00102$:
                           2086 ;Initial/src/lib/usb_common.c:387: fx2_stall_ep0 ();		// we don't handle these.  indicate error
                           2087 ;     genCall
   0EDF 12 05 47           2088 	lcall	_fx2_stall_ep0
                           2089 ;Initial/src/lib/usb_common.c:388: break;
                           2090 ;Initial/src/lib/usb_common.c:390: case bmRT_TYPE_VENDOR:
                           2091 ;	Peephole 112.b	changed ljmp to sjmp
   0EE2 80 14              2092 	sjmp	00108$
   0EE4                    2093 00103$:
                           2094 ;Initial/src/lib/usb_common.c:392: if (!app_vendor_cmd ())
                           2095 ;     genCall
   0EE4 12 01 65           2096 	lcall	_app_vendor_cmd
   0EE7 E5 82              2097 	mov	a,dpl
                           2098 ;     genIfx
                           2099 ;     genIfxJump
                           2100 ;	Peephole 109	removed ljmp by inverse jump logic
   0EE9 70 0D              2101 	jnz	00108$
   0EEB                    2102 00120$:
                           2103 ;Initial/src/lib/usb_common.c:393: fx2_stall_ep0 ();
                           2104 ;     genCall
   0EEB 12 05 47           2105 	lcall	_fx2_stall_ep0
                           2106 ;Initial/src/lib/usb_common.c:394: break;
                           2107 ;Initial/src/lib/usb_common.c:396: case bmRT_TYPE_STD:
                           2108 ;	Peephole 112.b	changed ljmp to sjmp
   0EEE 80 08              2109 	sjmp	00108$
   0EF0                    2110 00106$:
                           2111 ;Initial/src/lib/usb_common.c:398: usb_handle_std_requests();
                           2112 ;     genCall
   0EF0 12 0E B1           2113 	lcall	_usb_handle_std_requests
                           2114 ;Initial/src/lib/usb_common.c:399: break;
                           2115 ;Initial/src/lib/usb_common.c:401: default:
                           2116 ;	Peephole 112.b	changed ljmp to sjmp
   0EF3 80 03              2117 	sjmp	00108$
   0EF5                    2118 00107$:
                           2119 ;Initial/src/lib/usb_common.c:402: fx2_stall_ep0 ();		// indicate error
                           2120 ;     genCall
   0EF5 12 05 47           2121 	lcall	_fx2_stall_ep0
                           2122 ;Initial/src/lib/usb_common.c:405: }
   0EF8                    2123 00108$:
                           2124 ;Initial/src/lib/usb_common.c:408: EP0CS |= bmHSNAK;
                           2125 ;     genAssign
                           2126 ;     genOr
                           2127 ;	Peephole 248.a	optimized or to xdata
   0EF8 90 E6 A0           2128 	mov	dptr,#_EP0CS
   0EFB E0                 2129 	movx	a,@dptr
   0EFC FA                 2130 	mov	r2,a
   0EFD 44 80              2131 	orl	a,#0x80
   0EFF F0                 2132 	movx	@dptr,a
   0F00                    2133 00109$:
   0F00 22                 2134 	ret
                           2135 	.area CSEG    (CODE)
                           2136 	.area XINIT   (CODE)
