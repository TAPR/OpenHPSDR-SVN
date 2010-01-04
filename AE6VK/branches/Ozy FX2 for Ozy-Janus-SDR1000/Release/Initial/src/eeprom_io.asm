;--------------------------------------------------------
; File Created by SDCC : free open source ANSI-C Compiler
; Version 2.9.0 #5416 (Mar 22 2009) (MINGW32)
; This file was generated Mon Jan 04 09:33:53 2010
;--------------------------------------------------------
	.module eeprom_io
	.optsdcc -mmcs51 --model-small
	
;--------------------------------------------------------
; Public variables in this module
;--------------------------------------------------------
	.globl _eeprom_read_PARM_4
	.globl _eeprom_read_PARM_3
	.globl _eeprom_read_PARM_2
	.globl _eeprom_read
;--------------------------------------------------------
; special function registers
;--------------------------------------------------------
	.area RSEG    (DATA)
;--------------------------------------------------------
; special function bits
;--------------------------------------------------------
	.area RSEG    (DATA)
;--------------------------------------------------------
; overlayable register banks
;--------------------------------------------------------
	.area REG_BANK_0	(REL,OVR,DATA)
	.ds 8
;--------------------------------------------------------
; internal ram data
;--------------------------------------------------------
	.area DSEG    (DATA)
_eeprom_read_PARM_2:
	.ds 1
_eeprom_read_PARM_3:
	.ds 2
_eeprom_read_PARM_4:
	.ds 1
;--------------------------------------------------------
; overlayable items in internal ram 
;--------------------------------------------------------
	.area OSEG    (OVR,DATA)
;--------------------------------------------------------
; indirectly addressable internal ram data
;--------------------------------------------------------
	.area ISEG    (DATA)
;--------------------------------------------------------
; absolute internal ram data
;--------------------------------------------------------
	.area IABS    (ABS,DATA)
	.area IABS    (ABS,DATA)
;--------------------------------------------------------
; bit data
;--------------------------------------------------------
	.area BSEG    (BIT)
;--------------------------------------------------------
; paged external ram data
;--------------------------------------------------------
	.area PSEG    (PAG,XDATA)
;--------------------------------------------------------
; external ram data
;--------------------------------------------------------
	.area XSEG    (XDATA)
_eeprom_read_cmd_1_1:
	.ds 2
;--------------------------------------------------------
; absolute external ram data
;--------------------------------------------------------
	.area XABS    (ABS,XDATA)
;--------------------------------------------------------
; external initialized ram data
;--------------------------------------------------------
	.area HOME    (CODE)
	.area GSINIT0 (CODE)
	.area GSINIT1 (CODE)
	.area GSINIT2 (CODE)
	.area GSINIT3 (CODE)
	.area GSINIT4 (CODE)
	.area GSINIT5 (CODE)
	.area GSINIT  (CODE)
	.area GSFINAL (CODE)
	.area CSEG    (CODE)
;--------------------------------------------------------
; global & static initialisations
;--------------------------------------------------------
	.area HOME    (CODE)
	.area GSINIT  (CODE)
	.area GSFINAL (CODE)
	.area GSINIT  (CODE)
;--------------------------------------------------------
; Home
;--------------------------------------------------------
	.area HOME    (CODE)
	.area HOME    (CODE)
;--------------------------------------------------------
; code
;--------------------------------------------------------
	.area CSEG    (CODE)
;------------------------------------------------------------
;Allocation info for local variables in function 'eeprom_read'
;------------------------------------------------------------
;eeprom_offset             Allocated with name '_eeprom_read_PARM_2'
;buf                       Allocated with name '_eeprom_read_PARM_3'
;len                       Allocated with name '_eeprom_read_PARM_4'
;i2c_addr                  Allocated to registers r2 
;cmd                       Allocated with name '_eeprom_read_cmd_1_1'
;------------------------------------------------------------
;	../Initial/src/eeprom_io.c:32: eeprom_read (unsigned char i2c_addr, unsigned char eeprom_offset,
;	-----------------------------------------
;	 function eeprom_read
;	-----------------------------------------
_eeprom_read:
	ar2 = 0x02
	ar3 = 0x03
	ar4 = 0x04
	ar5 = 0x05
	ar6 = 0x06
	ar7 = 0x07
	ar0 = 0x00
	ar1 = 0x01
	mov	r2,dpl
;	../Initial/src/eeprom_io.c:41: cmd[0] = 0; // <-- address high byte, set to 0 since we are going to always
	mov	dptr,#_eeprom_read_cmd_1_1
	clr	a
	movx	@dptr,a
;	../Initial/src/eeprom_io.c:43: cmd[1] = eeprom_offset; // <-- address low byte
	mov	dptr,#(_eeprom_read_cmd_1_1 + 0x0001)
	mov	a,_eeprom_read_PARM_2
	movx	@dptr,a
;	../Initial/src/eeprom_io.c:44: if (!i2c_write(i2c_addr, cmd, 2))
	mov	_i2c_write_PARM_2,#_eeprom_read_cmd_1_1
	mov	(_i2c_write_PARM_2 + 1),#(_eeprom_read_cmd_1_1 >> 8)
	mov	_i2c_write_PARM_3,#0x02
	mov	dpl,r2
	push	ar2
	lcall	_i2c_write
	mov	a,dpl
	pop	ar2
;	../Initial/src/eeprom_io.c:45: return 0;
	jnz	00102$
	mov	dpl,a
	ret
00102$:
;	../Initial/src/eeprom_io.c:47: return i2c_read(i2c_addr, buf, len);
	mov	_i2c_read_PARM_2,_eeprom_read_PARM_3
	mov	(_i2c_read_PARM_2 + 1),(_eeprom_read_PARM_3 + 1)
	mov	_i2c_read_PARM_3,_eeprom_read_PARM_4
	mov	dpl,r2
	ljmp	_i2c_read
	.area CSEG    (CODE)
	.area CONST   (CODE)
	.area CABS    (ABS,CODE)
