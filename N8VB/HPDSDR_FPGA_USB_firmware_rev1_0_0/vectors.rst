                              1 ;/*
                              2 ; * Copyright (c) 2005 Philip A Covington, N8VB
                              3 ; *
                              4 ; * This program is free software; you can redistribute it and/or modify
                              5 ; * it under the terms of the GNU General Public License as published by
                              6 ; * the Free Software Foundation; either version 2 of the License, or
                              7 ; * (at your option) any later version.
                              8 ; *
                              9 ; * This program is distributed in the hope that it will be useful,
                             10 ; * but WITHOUT ANY WARRANTY; without even the implied warranty of
                             11 ; * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
                             12 ; * GNU General Public License for more details.
                             13 ; *
                             14 ; * You should have received a copy of the GNU General Public License
                             15 ; * along with this program; if not, write to the Free Software
                             16 ; * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
                             17 ; *
                             18 ; */
                             19 
                             20 ; Interrupt vectors for FX2 when using SDCC
                             21 ;
                             22 ; assemble with: asx8051 -plosgff vectors.asm
                             23 ; put the resulting object module first in the list of object modules
                             24 ; compile like this: sdcc -Wl -bVECT=0x0000 --code-loc 0x0200 <your_file_name>.rel vectors.rel
                             25 ; to start code at 0x0200 and place VECT at 0x0000 in code
                             26 
                             27 	.module vectors
                             28 
                             29 ; ----------------------------------------------------------------
                             30 ;		  FX2 interrupt vectors
                             31 ; ----------------------------------------------------------------
                             32 
                             33 	.area CSEG (CODE)
                             34 	.area GSINIT (CODE)
                             35 	.area VECT (CODE)
                             36 
   0000                      37 __standard_interrupt_vector::
   0000                      38 __reset_vector::
                             39 
   0000 02 09 83             40 	ljmp	s_GSINIT		
                             41 					
   0003 02 01 80             42 	ljmp	_ISR_Ext0 		;INT0 (EXT0)
   0006                      43 	.ds	5			
   000B 02 01 80             44 	ljmp	_ISR_Tmr0		;INT1 (TMR0)
   000E                      45 	.ds	5
   0013 02 01 80             46 	ljmp	_ISR_Ext1		;INT2 (EXT1)
   0016                      47 	.ds	5
   001B 02 01 80             48 	ljmp	_ISR_Tmr1		;INT3	(TMR1)
   001E                      49 	.ds	5
   0023 02 01 80             50 	ljmp	_ISR_Ser0		;INT4 (SER0)
   0026                      51 	.ds	5
   002B 02 01 80             52 	ljmp	_ISR_Tmr2		;INT5 (TMR2)
   002E                      53 	.ds	5
   0033 02 01 80             54 	ljmp	_ISR_Resume		;INT6 (RESUME)
   0036                      55 	.ds	5
   003B 02 01 80             56 	ljmp	_ISR_Ser1		;INT7	(SER1)
   003E                      57 	.ds	5
   0043 02 01 00             58 	ljmp	__usb_autovector	;INT8 (USB)
   0046                      59 	.ds	5
   004B 02 01 80             60 	ljmp	_ISR_I2c		;INT9 (I2C)
   004E                      61 	.ds	5
   0053 02 00 80             62 	ljmp	__fifo_gpif_autovector	;INT10 (GPIF/FIFO)
   0056                      63 	.ds	5
   005B 02 01 80             64 	ljmp	_ISR_Int5pin		;INT11 (INT5PIN)
   005E                      65 	.ds	5
   0063 02 01 80             66 	ljmp	_ISR_Int6pin		;INT12 (INT6PIN)
   0066                      67 	.ds	5
                             68 
                             69 ; ----------------------------------------------------------------
                             70 ; the FIFO/GPIF autovector.  14 4-byte entries.
                             71 ; must start on a 128 byte boundary.
                             72 ; ----------------------------------------------------------------
                             73 	
                    0080     74 	. = __reset_vector + 0x0080
                             75 		
   0080                      76 __fifo_gpif_autovector::		
                             77 
   0080 02 01 80             78 	ljmp	_ISR_Ep2pf
   0083 00                   79 	nop	
   0084 02 01 80             80 	ljmp	_ISR_Ep4pf
   0087 00                   81 	nop	
   0088 02 01 80             82 	ljmp	_ISR_Ep6pf
   008B 00                   83 	nop	
   008C 02 01 80             84 	ljmp	_ISR_Ep8pf
   008F 00                   85 	nop	
   0090 02 01 80             86 	ljmp	_ISR_Ep2ef
   0093 00                   87 	nop	
   0094 02 01 80             88 	ljmp	_ISR_Ep4ef
   0097 00                   89 	nop	
   0098 02 01 80             90 	ljmp	_ISR_Ep6ef
   009B 00                   91 	nop	
   009C 02 01 80             92 	ljmp	_ISR_Ep8ef
   009F 00                   93 	nop	
   00A0 02 01 80             94 	ljmp	_ISR_Ep2ff
   00A3 00                   95 	nop	
   00A4 02 01 80             96 	ljmp	_ISR_Ep4ff
   00A7 00                   97 	nop	
   00A8 02 01 80             98 	ljmp	_ISR_Ep6ff
   00AB 00                   99 	nop	
   00AC 02 01 80            100 	ljmp	_ISR_Ep8ff
   00AF 00                  101 	nop	
   00B0 02 01 80            102 	ljmp	_ISR_Gpifdone
   00B3 00                  103 	nop	
   00B4 02 01 80            104 	ljmp	_ISR_Gpifwf
   00B7 00                  105 	nop	
                            106 
                            107 	
                            108 ; ----------------------------------------------------------------
                            109 ; the USB autovector.  32 4-byte entries.
                            110 ; must start on a 256 byte boundary.
                            111 ; ----------------------------------------------------------------
                            112 
                    0100    113 	. = __reset_vector + 0x0100
                            114 	
   0100                     115 __usb_autovector::  
                            116 					
   0100 02 08 4A            117 	ljmp	_ISR_Sudav		;SUDAV
   0103 00                  118 	nop
   0104 02 01 80            119 	ljmp	_ISR_Sof		;SOF
   0107 00                  120 	nop
   0108 02 08 66            121 	ljmp	_ISR_Sutok		;SUTOK
   010B 00                  122 	nop
   010C 02 01 80            123 	ljmp	_ISR_Suspend		;SUSPEND
   010F 00                  124 	nop
   0110 02 08 7C            125 	ljmp	_ISR_Usb_Reset		;RESET
   0113 00                  126 	nop
   0114 02 08 BA            127 	ljmp	_ISR_Highspeed		;HISPEED
   0117 00                  128 	nop
   0118 02 01 80            129 	ljmp	_ISR_Ep0ack		;EP0ACK
   011B 00                  130 	nop
   011C 02 01 80            131 	ljmp	__isr_nop		;RESERVED
   011F 00                  132 	nop
   0120 02 01 80            133 	ljmp	_ISR_Ep0in		;EP0-IN
   0123 00                  134 	nop
   0124 02 01 80            135 	ljmp	_ISR_Ep0out		;EP0-OUT
   0127 00                  136 	nop
   0128 02 01 80            137 	ljmp	_ISR_Ep1in		;EP1-IN
   012B 00                  138 	nop
   012C 02 09 00            139 	ljmp	_ISR_Ep1out		;EP1-OUT	
   012F 00                  140 	nop
   0130 02 01 80            141 	ljmp	_ISR_Ep2inout		;EP2 IN-OUT
   0133 00                  142 	nop
   0134 02 01 80            143 	ljmp	_ISR_Ep4inout		;EP4 IN-OUT
   0137 00                  144 	nop
   0138 02 01 80            145 	ljmp	_ISR_Ep6inout		;EP6 IN-OUT
   013B 00                  146 	nop
   013C 02 01 80            147 	ljmp	_ISR_Ep8inout		;EP8 IN-OUT
   013F 00                  148 	nop
   0140 02 01 80            149 	ljmp	_ISR_Ibn		;IBN IN-Bulk-NAK
   0143 00                  150 	nop
   0144 02 01 80            151 	ljmp	__isr_nop		;RESERVED
   0147 00                  152 	nop
   0148 02 01 80            153 	ljmp	_ISR_Ep0ping		;EP0PING
   014B 00                  154 	nop
   014C 02 01 80            155 	ljmp	_ISR_Ep1ping		;EP1PING
   014F 00                  156 	nop
   0150 02 01 80            157 	ljmp	_ISR_Ep2ping		;EP2PING
   0153 00                  158 	nop
   0154 02 01 80            159 	ljmp	_ISR_Ep4ping		;EP4PING
   0157 00                  160 	nop
   0158 02 01 80            161 	ljmp	_ISR_Ep6ping		;EP6PING
   015B 00                  162 	nop
   015C 02 01 80            163 	ljmp	_ISR_Ep8ping		;EP8PING
   015F 00                  164 	nop
   0160 02 01 80            165 	ljmp	_ISR_Errlimit		;ERRLIMIT
   0163 00                  166 	nop
   0164 02 01 80            167 	ljmp	__isr_nop		;RESERVED
   0167 00                  168 	nop
   0168 02 01 80            169 	ljmp	__isr_nop		;RESERVED
   016B 00                  170 	nop
   016C 02 01 80            171 	ljmp	__isr_nop		;RESERVED
   016F 00                  172 	nop
   0170 02 01 80            173 	ljmp	_ISR_Ep2isoerr		;EP2ISOERR
   0173 00                  174 	nop
   0174 02 01 80            175 	ljmp	_ISR_Ep4isoerr		;EP4ISOERR
   0177 00                  176 	nop
   0178 02 01 80            177 	ljmp	_ISR_Ep6isoerr		;EP6ISOERR
   017B 00                  178 	nop
   017C 02 01 80            179 	ljmp	_ISR_Ep8isoerr		;EP8ISOERR
   017F 00                  180 	nop
                            181 
                            182 ; Comment out used ISRs that
                            183 ; are used so they don't point to reti
                            184 ; below.
                            185 
   0180                     186 _ISR_Ext0::
   0180                     187 _ISR_Tmr0::
   0180                     188 _ISR_Ext1::
   0180                     189 _ISR_Tmr1::
   0180                     190 _ISR_Ser0::
   0180                     191 _ISR_Tmr2::
   0180                     192 _ISR_Resume::
   0180                     193 _ISR_Ser1::
   0180                     194 _ISR_I2c::
   0180                     195 _ISR_Int5pin::
   0180                     196 _ISR_Int6pin::
                            197 
   0180                     198 _ISR_Ep2pf::
   0180                     199 _ISR_Ep4pf::
   0180                     200 _ISR_Ep6pf::
   0180                     201 _ISR_Ep8pf::
   0180                     202 _ISR_Ep2ef::
   0180                     203 _ISR_Ep4ef::
   0180                     204 _ISR_Ep6ef::
   0180                     205 _ISR_Ep8ef::
   0180                     206 _ISR_Ep2ff::
   0180                     207 _ISR_Ep4ff::
   0180                     208 _ISR_Ep6ff::
   0180                     209 _ISR_Ep8ff::
   0180                     210 _ISR_Gpifdone::
   0180                     211 _ISR_Gpifwf::
                            212 
                            213 ;_ISR_Sudav::
   0180                     214 _ISR_Sof::
                            215 ;_ISR_Sutok::
   0180                     216 _ISR_Suspend::
                            217 ;_ISR_Usb_Reset::
                            218 ;_ISR_Highspeed::
   0180                     219 _ISR_Ep0ack::
   0180                     220 _ISR_Ep0in::
   0180                     221 _ISR_Ep0out::
   0180                     222 _ISR_Ep1in::
                            223 ;_ISR_Ep1out::
   0180                     224 _ISR_Ep2inout::
   0180                     225 _ISR_Ep4inout::
   0180                     226 _ISR_Ep6inout::
   0180                     227 _ISR_Ep8inout::
   0180                     228 _ISR_Ibn::
   0180                     229 _ISR_Ep0ping::
   0180                     230 _ISR_Ep1ping::
   0180                     231 _ISR_Ep2ping::
   0180                     232 _ISR_Ep4ping::
   0180                     233 _ISR_Ep6ping::
   0180                     234 _ISR_Ep8ping::
   0180                     235 _ISR_Errlimit::
   0180                     236 _ISR_Ep2isoerr::
   0180                     237 _ISR_Ep4isoerr::
   0180                     238 _ISR_Ep6isoerr::
   0180                     239 _ISR_Ep8isoerr::
   0180                     240 __isr_nop::
   0180 32                  241 	reti
   0181                     242 __end_of_vector_table::
   0181 00                  243 	nop
