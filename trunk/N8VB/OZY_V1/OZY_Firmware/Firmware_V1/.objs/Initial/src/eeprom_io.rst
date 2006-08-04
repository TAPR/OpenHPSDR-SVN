                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Fri Aug 04 15:56:57 2006
                              5 ;--------------------------------------------------------
                              6 	.module eeprom_io
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _eeprom_read_PARM_4
                             13 	.globl _eeprom_read_PARM_3
                             14 	.globl _eeprom_read_PARM_2
                             15 	.globl _eeprom_read
                             16 ;--------------------------------------------------------
                             17 ; special function registers
                             18 ;--------------------------------------------------------
                             19 	.area RSEG    (DATA)
                             20 ;--------------------------------------------------------
                             21 ; special function bits 
                             22 ;--------------------------------------------------------
                             23 	.area RSEG    (DATA)
                             24 ;--------------------------------------------------------
                             25 ; overlayable register banks 
                             26 ;--------------------------------------------------------
                             27 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                      28 	.ds 8
                             29 ;--------------------------------------------------------
                             30 ; internal ram data
                             31 ;--------------------------------------------------------
                             32 	.area DSEG    (DATA)
   0009                      33 _eeprom_read_PARM_2::
   0009                      34 	.ds 1
   000A                      35 _eeprom_read_PARM_3::
   000A                      36 	.ds 2
   000C                      37 _eeprom_read_PARM_4::
   000C                      38 	.ds 1
                             39 ;--------------------------------------------------------
                             40 ; overlayable items in internal ram 
                             41 ;--------------------------------------------------------
                             42 	.area OSEG    (OVR,DATA)
                             43 ;--------------------------------------------------------
                             44 ; indirectly addressable internal ram data
                             45 ;--------------------------------------------------------
                             46 	.area ISEG    (DATA)
                             47 ;--------------------------------------------------------
                             48 ; bit data
                             49 ;--------------------------------------------------------
                             50 	.area BSEG    (BIT)
                             51 ;--------------------------------------------------------
                             52 ; paged external ram data
                             53 ;--------------------------------------------------------
                             54 	.area PSEG    (PAG,XDATA)
                             55 ;--------------------------------------------------------
                             56 ; external ram data
                             57 ;--------------------------------------------------------
                             58 	.area XSEG    (XDATA)
   2009                      59 _eeprom_read_cmd_1_1:
   2009                      60 	.ds 2
                             61 ;--------------------------------------------------------
                             62 ; external initialized ram data
                             63 ;--------------------------------------------------------
                             64 	.area CSEG    (CODE)
                             65 	.area GSINIT0 (CODE)
                             66 	.area GSINIT1 (CODE)
                             67 	.area GSINIT2 (CODE)
                             68 	.area GSINIT3 (CODE)
                             69 	.area GSINIT4 (CODE)
                             70 	.area GSINIT5 (CODE)
                             71 ;--------------------------------------------------------
                             72 ; global & static initialisations
                             73 ;--------------------------------------------------------
                             74 	.area CSEG    (CODE)
                             75 	.area GSINIT  (CODE)
                             76 	.area GSFINAL (CODE)
                             77 	.area GSINIT  (CODE)
                             78 ;--------------------------------------------------------
                             79 ; Home
                             80 ;--------------------------------------------------------
                             81 	.area HOME    (CODE)
                             82 	.area CSEG    (CODE)
                             83 ;--------------------------------------------------------
                             84 ; code
                             85 ;--------------------------------------------------------
                             86 	.area CSEG    (CODE)
                             87 ;------------------------------------------------------------
                             88 ;Allocation info for local variables in function 'eeprom_read'
                             89 ;------------------------------------------------------------
                             90 ;eeprom_offset             Allocated with name '_eeprom_read_PARM_2'
                             91 ;buf                       Allocated with name '_eeprom_read_PARM_3'
                             92 ;len                       Allocated with name '_eeprom_read_PARM_4'
                             93 ;i2c_addr                  Allocated to registers r2 
                             94 ;cmd                       Allocated with name '_eeprom_read_cmd_1_1'
                             95 ;------------------------------------------------------------
                             96 ;Initial/src/eeprom_io.c:32: eeprom_read (unsigned char i2c_addr, unsigned char eeprom_offset,
                             97 ;	-----------------------------------------
                             98 ;	 function eeprom_read
                             99 ;	-----------------------------------------
   0438                     100 _eeprom_read:
                    0002    101 	ar2 = 0x02
                    0003    102 	ar3 = 0x03
                    0004    103 	ar4 = 0x04
                    0005    104 	ar5 = 0x05
                    0006    105 	ar6 = 0x06
                    0007    106 	ar7 = 0x07
                    0000    107 	ar0 = 0x00
                    0001    108 	ar1 = 0x01
                            109 ;     genReceive
   0438 AA 82               110 	mov	r2,dpl
                            111 ;Initial/src/eeprom_io.c:41: cmd[0] = 0; // <-- address high byte, set to 0 since we are going to always
                            112 ;     genPointerSet
                            113 ;     genFarPointerSet
   043A 90 20 09            114 	mov	dptr,#_eeprom_read_cmd_1_1
                            115 ;	Peephole 181	changed mov to clr
   043D E4                  116 	clr	a
   043E F0                  117 	movx	@dptr,a
                            118 ;Initial/src/eeprom_io.c:43: cmd[1] = eeprom_offset; // <-- address low byte
                            119 ;     genPointerSet
                            120 ;     genFarPointerSet
   043F 90 20 0A            121 	mov	dptr,#(_eeprom_read_cmd_1_1 + 0x0001)
   0442 E5 09               122 	mov	a,_eeprom_read_PARM_2
   0444 F0                  123 	movx	@dptr,a
                            124 ;Initial/src/eeprom_io.c:44: if (!i2c_write(i2c_addr, cmd, 2))
                            125 ;     genAssign
   0445 75 2B 09            126 	mov	_i2c_write_PARM_2,#_eeprom_read_cmd_1_1
   0448 75 2C 20            127 	mov	(_i2c_write_PARM_2 + 1),#(_eeprom_read_cmd_1_1 >> 8)
                            128 ;     genAssign
   044B 75 2D 02            129 	mov	_i2c_write_PARM_3,#0x02
                            130 ;     genCall
   044E 8A 82               131 	mov	dpl,r2
   0450 C0 02               132 	push	ar2
   0452 12 07 DA            133 	lcall	_i2c_write
   0455 E5 82               134 	mov	a,dpl
   0457 D0 02               135 	pop	ar2
                            136 ;     genIfx
                            137 ;     genIfxJump
                            138 ;	Peephole 109	removed ljmp by inverse jump logic
                            139 ;Initial/src/eeprom_io.c:45: return 0;
                            140 ;     genRet
                            141 ;	Peephole 256.c	loading dpl with zero from a
   0459 70 03               142 	jnz	00102$
   045B                     143 00106$:
   045B F5 82               144 	mov	dpl,a
                            145 ;	Peephole 112.b	changed ljmp to sjmp
                            146 ;	Peephole 251.b	replaced sjmp to ret with ret
   045D 22                  147 	ret
   045E                     148 00102$:
                            149 ;Initial/src/eeprom_io.c:47: return i2c_read(i2c_addr, buf, len);
                            150 ;     genAssign
   045E 85 0A 2B            151 	mov	_i2c_read_PARM_2,_eeprom_read_PARM_3
   0461 85 0B 2C            152 	mov	(_i2c_read_PARM_2 + 1),(_eeprom_read_PARM_3 + 1)
                            153 ;     genAssign
   0464 85 0C 2D            154 	mov	_i2c_read_PARM_3,_eeprom_read_PARM_4
                            155 ;     genCall
   0467 8A 82               156 	mov	dpl,r2
                            157 ;     genRet
                            158 ;	Peephole 253.b	replaced lcall/ret with ljmp
   0469 02 07 03            159 	ljmp	_i2c_read
                            160 	.area CSEG    (CODE)
