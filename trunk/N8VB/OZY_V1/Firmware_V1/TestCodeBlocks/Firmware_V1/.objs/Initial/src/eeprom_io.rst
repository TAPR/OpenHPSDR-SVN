                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Tue Jul 11 16:41:58 2006
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
   2009                      60 	.ds 1
                             61 ;--------------------------------------------------------
                             62 ; external initialized ram data
                             63 ;--------------------------------------------------------
                             64 	.area XISEG   (XDATA)
                             65 	.area CSEG    (CODE)
                             66 	.area GSINIT0 (CODE)
                             67 	.area GSINIT1 (CODE)
                             68 	.area GSINIT2 (CODE)
                             69 	.area GSINIT3 (CODE)
                             70 	.area GSINIT4 (CODE)
                             71 	.area GSINIT5 (CODE)
                             72 ;--------------------------------------------------------
                             73 ; global & static initialisations
                             74 ;--------------------------------------------------------
                             75 	.area CSEG    (CODE)
                             76 	.area GSINIT  (CODE)
                             77 	.area GSFINAL (CODE)
                             78 	.area GSINIT  (CODE)
                             79 ;--------------------------------------------------------
                             80 ; Home
                             81 ;--------------------------------------------------------
                             82 	.area HOME    (CODE)
                             83 	.area CSEG    (CODE)
                             84 ;--------------------------------------------------------
                             85 ; code
                             86 ;--------------------------------------------------------
                             87 	.area CSEG    (CODE)
                             88 ;------------------------------------------------------------
                             89 ;Allocation info for local variables in function 'eeprom_read'
                             90 ;------------------------------------------------------------
                             91 ;eeprom_offset             Allocated with name '_eeprom_read_PARM_2'
                             92 ;buf                       Allocated with name '_eeprom_read_PARM_3'
                             93 ;len                       Allocated with name '_eeprom_read_PARM_4'
                             94 ;i2c_addr                  Allocated to registers r2 
                             95 ;cmd                       Allocated with name '_eeprom_read_cmd_1_1'
                             96 ;------------------------------------------------------------
                             97 ;Initial/src/eeprom_io.c:32: eeprom_read (unsigned char i2c_addr, unsigned char eeprom_offset,
                             98 ;	-----------------------------------------
                             99 ;	 function eeprom_read
                            100 ;	-----------------------------------------
   039A                     101 _eeprom_read:
                    0002    102 	ar2 = 0x02
                    0003    103 	ar3 = 0x03
                    0004    104 	ar4 = 0x04
                    0005    105 	ar5 = 0x05
                    0006    106 	ar6 = 0x06
                    0007    107 	ar7 = 0x07
                    0000    108 	ar0 = 0x00
                    0001    109 	ar1 = 0x01
                            110 ;     genReceive
   039A AA 82               111 	mov	r2,dpl
                            112 ;Initial/src/eeprom_io.c:39: cmd[0] = eeprom_offset;
                            113 ;     genPointerSet
                            114 ;     genFarPointerSet
   039C 90 20 09            115 	mov	dptr,#_eeprom_read_cmd_1_1
   039F E5 09               116 	mov	a,_eeprom_read_PARM_2
   03A1 F0                  117 	movx	@dptr,a
                            118 ;Initial/src/eeprom_io.c:40: if (!i2c_write(i2c_addr, cmd, 1))
                            119 ;     genAssign
   03A2 75 2B 09            120 	mov	_i2c_write_PARM_2,#_eeprom_read_cmd_1_1
   03A5 75 2C 20            121 	mov	(_i2c_write_PARM_2 + 1),#(_eeprom_read_cmd_1_1 >> 8)
                            122 ;     genAssign
   03A8 75 2D 01            123 	mov	_i2c_write_PARM_3,#0x01
                            124 ;     genCall
   03AB 8A 82               125 	mov	dpl,r2
   03AD C0 02               126 	push	ar2
   03AF 12 06 69            127 	lcall	_i2c_write
   03B2 E5 82               128 	mov	a,dpl
   03B4 D0 02               129 	pop	ar2
                            130 ;     genIfx
                            131 ;     genIfxJump
                            132 ;	Peephole 109	removed ljmp by inverse jump logic
                            133 ;Initial/src/eeprom_io.c:41: return 0;
                            134 ;     genRet
                            135 ;	Peephole 256.c	loading dpl with zero from a
   03B6 70 03               136 	jnz	00102$
   03B8                     137 00106$:
   03B8 F5 82               138 	mov	dpl,a
                            139 ;	Peephole 112.b	changed ljmp to sjmp
                            140 ;	Peephole 251.b	replaced sjmp to ret with ret
   03BA 22                  141 	ret
   03BB                     142 00102$:
                            143 ;Initial/src/eeprom_io.c:43: return i2c_read(i2c_addr, buf, len);
                            144 ;     genAssign
   03BB 85 0A 2B            145 	mov	_i2c_read_PARM_2,_eeprom_read_PARM_3
   03BE 85 0B 2C            146 	mov	(_i2c_read_PARM_2 + 1),(_eeprom_read_PARM_3 + 1)
                            147 ;     genAssign
   03C1 85 0C 2D            148 	mov	_i2c_read_PARM_3,_eeprom_read_PARM_4
                            149 ;     genCall
   03C4 8A 82               150 	mov	dpl,r2
                            151 ;     genRet
                            152 ;	Peephole 253.b	replaced lcall/ret with ljmp
   03C6 02 05 92            153 	ljmp	_i2c_read
                            154 	.area CSEG    (CODE)
                            155 	.area XINIT   (CODE)
