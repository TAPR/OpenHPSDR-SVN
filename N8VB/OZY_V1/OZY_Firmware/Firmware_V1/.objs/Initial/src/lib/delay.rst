                              1 ;--------------------------------------------------------
                              2 ; File Created by SDCC : FreeWare ANSI-C Compiler
                              3 ; Version 2.5.0 #1020 (May  8 2005)
                              4 ; This file generated Fri Aug 04 15:56:57 2006
                              5 ;--------------------------------------------------------
                              6 	.module delay
                              7 	.optsdcc -mmcs51 --model-small
                              8 	
                              9 ;--------------------------------------------------------
                             10 ; Public variables in this module
                             11 ;--------------------------------------------------------
                             12 	.globl _mdelay
                             13 	.globl _udelay
                             14 ;--------------------------------------------------------
                             15 ; special function registers
                             16 ;--------------------------------------------------------
                             17 	.area RSEG    (DATA)
                             18 ;--------------------------------------------------------
                             19 ; special function bits 
                             20 ;--------------------------------------------------------
                             21 	.area RSEG    (DATA)
                             22 ;--------------------------------------------------------
                             23 ; overlayable register banks 
                             24 ;--------------------------------------------------------
                             25 	.area REG_BANK_0	(REL,OVR,DATA)
   0000                      26 	.ds 8
                             27 ;--------------------------------------------------------
                             28 ; internal ram data
                             29 ;--------------------------------------------------------
                             30 	.area DSEG    (DATA)
                             31 ;--------------------------------------------------------
                             32 ; overlayable items in internal ram 
                             33 ;--------------------------------------------------------
                             34 	.area OSEG    (OVR,DATA)
                             35 ;--------------------------------------------------------
                             36 ; indirectly addressable internal ram data
                             37 ;--------------------------------------------------------
                             38 	.area ISEG    (DATA)
                             39 ;--------------------------------------------------------
                             40 ; bit data
                             41 ;--------------------------------------------------------
                             42 	.area BSEG    (BIT)
                             43 ;--------------------------------------------------------
                             44 ; paged external ram data
                             45 ;--------------------------------------------------------
                             46 	.area PSEG    (PAG,XDATA)
                             47 ;--------------------------------------------------------
                             48 ; external ram data
                             49 ;--------------------------------------------------------
                             50 	.area XSEG    (XDATA)
                             51 ;--------------------------------------------------------
                             52 ; external initialized ram data
                             53 ;--------------------------------------------------------
                             54 	.area CSEG    (CODE)
                             55 	.area GSINIT0 (CODE)
                             56 	.area GSINIT1 (CODE)
                             57 	.area GSINIT2 (CODE)
                             58 	.area GSINIT3 (CODE)
                             59 	.area GSINIT4 (CODE)
                             60 	.area GSINIT5 (CODE)
                             61 ;--------------------------------------------------------
                             62 ; global & static initialisations
                             63 ;--------------------------------------------------------
                             64 	.area CSEG    (CODE)
                             65 	.area GSINIT  (CODE)
                             66 	.area GSFINAL (CODE)
                             67 	.area GSINIT  (CODE)
                             68 ;--------------------------------------------------------
                             69 ; Home
                             70 ;--------------------------------------------------------
                             71 	.area HOME    (CODE)
                             72 	.area CSEG    (CODE)
                             73 ;--------------------------------------------------------
                             74 ; code
                             75 ;--------------------------------------------------------
                             76 	.area CSEG    (CODE)
                             77 ;------------------------------------------------------------
                             78 ;Allocation info for local variables in function 'udelay1'
                             79 ;------------------------------------------------------------
                             80 ;------------------------------------------------------------
                             81 ;Initial/src/lib/delay.c:30: udelay1 (void) _naked
                             82 ;	-----------------------------------------
                             83 ;	 function udelay1
                             84 ;	-----------------------------------------
   0691                      85 _udelay1:
                             86 ;	naked function: no prologue.
                             87 ;Initial/src/lib/delay.c:34: _endasm;
                             88 ;     genInline
                             89 	 ; lcall that got us here took 4 bus cycles
   0691 22                   90 	        ret ; 4 bus cycles
   0692                      91 00101$:
                             92 ;	naked function: no epilogue.
                             93 ;------------------------------------------------------------
                             94 ;Allocation info for local variables in function 'udelay'
                             95 ;------------------------------------------------------------
                             96 ;usecs                     Allocated to registers r2 
                             97 ;------------------------------------------------------------
                             98 ;Initial/src/lib/delay.c:41: udelay (unsigned char usecs)
                             99 ;	-----------------------------------------
                            100 ;	 function udelay
                            101 ;	-----------------------------------------
   0692                     102 _udelay:
                    0002    103 	ar2 = 0x02
                    0003    104 	ar3 = 0x03
                    0004    105 	ar4 = 0x04
                    0005    106 	ar5 = 0x05
                    0006    107 	ar6 = 0x06
                    0007    108 	ar7 = 0x07
                    0000    109 	ar0 = 0x00
                    0001    110 	ar1 = 0x01
                            111 ;     genReceive
   0692 AA 82               112 	mov	r2,dpl
                            113 ;Initial/src/lib/delay.c:43: do {
                            114 ;     genAssign
   0694                     115 00101$:
                            116 ;Initial/src/lib/delay.c:44: udelay1 ();
                            117 ;     genCall
   0694 12 06 91            118 	lcall	_udelay1
                            119 ;Initial/src/lib/delay.c:45: } while (--usecs != 0);
                            120 ;     genMinus
                            121 ;     genMinusDec
                            122 ;     genCmpEq
                            123 ;	Peephole 112.b	changed ljmp to sjmp
                            124 ;	Peephole 199	optimized misc jump sequence
                            125 ;	Peephole 257	optimized decrement with compare
   0697 DA FB               126 	djnz	r2,00101$
                            127 ;00107$:
                            128 ;	Peephole 200	removed redundant sjmp
   0699                     129 00108$:
   0699                     130 00104$:
   0699 22                  131 	ret
                            132 ;------------------------------------------------------------
                            133 ;Allocation info for local variables in function 'mdelay1'
                            134 ;------------------------------------------------------------
                            135 ;------------------------------------------------------------
                            136 ;Initial/src/lib/delay.c:57: mdelay1 (void) _naked
                            137 ;	-----------------------------------------
                            138 ;	 function mdelay1
                            139 ;	-----------------------------------------
   069A                     140 _mdelay1:
                            141 ;	naked function: no prologue.
                            142 ;Initial/src/lib/delay.c:68: _endasm;
                            143 ;     genInline
   069A 90 FB 50            144 	        mov dptr,#(-1200 & 0xffff)
   069D                     145 002$:
   069D A3                  146 	        inc dptr ; 3 bus cycles
   069E E5 82               147 	        mov a, dpl ; 2 bus cycles
   06A0 45 83               148 	        orl a, dph ; 2 bus cycles
   06A2 70 F9               149 	        jnz 002$ ; 3 bus cycles
   06A4 22                  150 	        ret
   06A5                     151 00101$:
                            152 ;	naked function: no epilogue.
                            153 ;------------------------------------------------------------
                            154 ;Allocation info for local variables in function 'mdelay'
                            155 ;------------------------------------------------------------
                            156 ;msecs                     Allocated to registers r2 r3 
                            157 ;------------------------------------------------------------
                            158 ;Initial/src/lib/delay.c:72: mdelay (unsigned int msecs)
                            159 ;	-----------------------------------------
                            160 ;	 function mdelay
                            161 ;	-----------------------------------------
   06A5                     162 _mdelay:
                            163 ;     genReceive
   06A5 AA 82               164 	mov	r2,dpl
   06A7 AB 83               165 	mov	r3,dph
                            166 ;Initial/src/lib/delay.c:74: do {
                            167 ;     genAssign
   06A9                     168 00101$:
                            169 ;Initial/src/lib/delay.c:75: mdelay1 ();
                            170 ;     genCall
   06A9 12 06 9A            171 	lcall	_mdelay1
                            172 ;Initial/src/lib/delay.c:76: } while (--msecs != 0);
                            173 ;     genMinus
                            174 ;     genMinusDec
   06AC 1A                  175 	dec	r2
   06AD BA FF 01            176 	cjne	r2,#0xff,00107$
   06B0 1B                  177 	dec	r3
   06B1                     178 00107$:
                            179 ;     genCmpEq
                            180 ;	Peephole 112.b	changed ljmp to sjmp
                            181 ;	Peephole 198	optimized misc jump sequence
   06B1 BA 00 F5            182 	cjne	r2,#0x00,00101$
   06B4 BB 00 F2            183 	cjne	r3,#0x00,00101$
                            184 ;00108$:
                            185 ;	Peephole 200	removed redundant sjmp
   06B7                     186 00109$:
   06B7                     187 00104$:
   06B7 22                  188 	ret
                            189 	.area CSEG    (CODE)
