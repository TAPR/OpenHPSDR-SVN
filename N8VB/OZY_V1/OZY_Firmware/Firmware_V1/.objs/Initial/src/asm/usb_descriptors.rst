                              1 ;;; -*- asm -*-
                              2 ;;;
                              3 ;;; HPSDR/OZY - High Performance Software Defined Radio, OZY Firmware
                              4 ;;;
                              5 ;;; Adapted from USRP firmware 07/10/2006 by Phil Covington N8VB
                              6 ;;;
                              7 ;;; Copyright 2003 Free Software Foundation, Inc.
                              8 ;;; 
                              9 ;;; This file is part of GNU Radio
                             10 ;;; 
                             11 ;;; GNU Radio is free software; you can redistribute it and/or modify
                             12 ;;; it under the terms of the GNU General Public License as published by
                             13 ;;; the Free Software Foundation; either version 2, or (at your option)
                             14 ;;; any later version.
                             15 ;;; 
                             16 ;;; GNU Radio is distributed in the hope that it will be useful,
                             17 ;;; but WITHOUT ANY WARRANTY; without even the implied warranty of
                             18 ;;; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
                             19 ;;; GNU General Public License for more details.
                             20 ;;; 
                             21 ;;; You should have received a copy of the GNU General Public License
                             22 ;;; along with GNU Radio; see the file COPYING.  If not, write to
                             23 ;;; the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
                             24 ;;; Boston, MA 02111-1307, USA.
                             25 ;;;
                             26 	
                             27 ;;; USB Descriptor table for the HPSDR
                             28 ;;; 
                             29 ;;; We're a high-speed only device (480 Mb/sec) with 1 configuration
                             30 ;;; and 3 interfaces.  
                             31 ;;; 
                             32 ;;;	interface 0:	command and status (ep0 COMMAND)
                             33 ;;;	interface 1:	Transmit path (ep2 OUT BULK)
                             34 ;;;	interface 2:	Receive path (ep6 IN BULK)
                             35 
                             36 	.module usb_descriptors
                             37 	
                    FFFE     38 	VID_FREE	 = 0xfffe	; Free Software Folks
                    0007     39 	PID_USRP	 = 0x0007	; HPSDR
                             40 
                             41 	;; We distinguish configured from unconfigured USRPs using the Device ID.
                             42 	;; If the MSB of the DID is 0, the device is unconfigured.
                             43 	;; The LSB of the DID is reserved for hardware revs.
                             44 	
                    0100     45 	DID_USRP	 = 0x0100	; Device ID (bcd)
                             46 
                             47 	
                    0001     48 	DSCR_DEVICE	 =   1	; Descriptor type: Device
                    0002     49 	DSCR_CONFIG	 =   2	; Descriptor type: Configuration
                    0003     50 	DSCR_STRING	 =   3	; Descriptor type: String
                    0004     51 	DSCR_INTRFC	 =   4	; Descriptor type: Interface
                    0005     52 	DSCR_ENDPNT	 =   5	; Descriptor type: Endpoint
                    0006     53 	DSCR_DEVQUAL	 =   6	; Descriptor type: Device Qualifier
                             54 	
                    0012     55 	DSCR_DEVICE_LEN	 =  18
                    0009     56 	DSCR_CONFIG_LEN  =   9
                    0009     57 	DSCR_INTRFC_LEN  =   9
                    0007     58 	DSCR_ENDPNT_LEN  =   7
                    000A     59 	DSCR_DEVQUAL_LEN =  10
                             60 	
                    0000     61 	ET_CONTROL	 =   0	; Endpoint type: Control
                    0001     62 	ET_ISO		 =   1	; Endpoint type: Isochronous
                    0002     63 	ET_BULK		 =   2	; Endpoint type: Bulk
                    0003     64 	ET_INT		 =   3	; Endpoint type: Interrupt
                             65 	
                             66 	
                             67 	;; configuration attributes
                    0040     68 	bmSELF_POWERED	=	1 << 6
                             69 
                             70 ;;; --------------------------------------------------------
                             71 ;;;	external ram data
                             72 ;;;--------------------------------------------------------
                             73 	
                             74 	.area USBDESCSEG    (XDATA)
                             75 	
                             76 ;;; ----------------------------------------------------------------
                             77 ;;; descriptors used when operating at high speed (480Mb/sec)
                             78 ;;; ----------------------------------------------------------------
                             79 	
   E000                      80 	.even	; descriptors must be 2-byte aligned for SUDPTR{H,L} to work
                             81 
                             82 	;; The .even directive isn't really honored by the linker.  Bummer!
                             83 	;; (There's no way to specify an alignment requirement for a given area,
                             84 	;; hence when they're concatenated together, even doesn't work.)
                             85 	;; 
                             86 	;; We work around this by telling the linker to put USBDESCSEG
                             87 	;; at 0xE000 absolute.  This means that the maximimum length of this
                             88 	;; segment is 480 bytes, leaving room for the two hash slots 
                             89 	;; at 0xE1EO to 0xE1FF.  
                             90 	;; 
                             91 	;; As of July 7, 2004, this segment is 326 bytes long
                             92 	
   E000                      93 _high_speed_device_descr::
   E000 12                   94 	.db	DSCR_DEVICE_LEN
   E001 01                   95 	.db	DSCR_DEVICE
   E002 00                   96 	.db	<0x0200		; Specification version (LSB)
   E003 02                   97 	.db	>0x0200		; Specification version (MSB)
   E004 FF                   98 	.db	0xff		; device class (vendor specific)
   E005 FF                   99 	.db	0xff		; device subclass (vendor specific)
   E006 FF                  100 	.db	0xff		; device protocol (vendor specific)
   E007 40                  101 	.db	64		; bMaxPacketSize0 for endpoint 0
   E008 FE                  102 	.db	<VID_FREE	; idVendor
   E009 FF                  103 	.db	>VID_FREE	; idVendor
   E00A 07                  104 	.db	<PID_USRP	; idProduct
   E00B 00                  105 	.db	>PID_USRP	; idProduct
   E00C                     106 _usb_desc_hw_rev_binary_patch_location_0::
   E00C 00                  107 	.db	<DID_USRP	; bcdDevice
   E00D 01                  108 	.db	>DID_USRP	; bcdDevice
   E00E 01                  109 	.db	SI_VENDOR	; iManufacturer (string index)
   E00F 02                  110 	.db	SI_PRODUCT	; iProduct (string index)
   E010 06                  111 	.db	SI_SERIAL	; iSerial number (string index)
   E011 01                  112 	.db	1		; bNumConfigurations
                            113 	
                            114 ;;; describes the other speed (12Mb/sec)
   E012                     115 	.even
   E012                     116 _high_speed_devqual_descr::
   E012 0A                  117 	.db	DSCR_DEVQUAL_LEN
   E013 06                  118 	.db	DSCR_DEVQUAL
   E014 00                  119 	.db	<0x0200		; bcdUSB (LSB)
   E015 02                  120 	.db	>0x0200		; bcdUSB (MSB)
   E016 FF                  121 	.db	0xff		; bDeviceClass
   E017 FF                  122 	.db	0xff		; bDeviceSubClass
   E018 FF                  123 	.db	0xff		; bDeviceProtocol
   E019 40                  124 	.db	64		; bMaxPacketSize0
   E01A 01                  125 	.db	1		; bNumConfigurations (one config at 12Mb/sec)
   E01B 00                  126 	.db	0		; bReserved
                            127 	
   E01C                     128 	.even
   E01C                     129 _high_speed_config_descr::	
   E01C 09                  130 	.db	DSCR_CONFIG_LEN
   E01D 02                  131 	.db	DSCR_CONFIG
   E01E 32                  132 	.db	<(_high_speed_config_descr_end - _high_speed_config_descr) ; LSB
   E01F 00                  133 	.db	>(_high_speed_config_descr_end - _high_speed_config_descr) ; MSB
   E020 03                  134 	.db	3		; bNumInterfaces
   E021 01                  135 	.db	1		; bConfigurationValue
   E022 00                  136 	.db	0		; iConfiguration
   E023 C0                  137 	.db	0x80 | bmSELF_POWERED ; bmAttributes
   E024 00                  138 	.db	0		; bMaxPower
                            139 
                            140 	;; interface descriptor 0 (command & status, ep0 COMMAND)
                            141 	
   E025 09                  142 	.db	DSCR_INTRFC_LEN
   E026 04                  143 	.db	DSCR_INTRFC
   E027 00                  144 	.db	0		; bInterfaceNumber (zero based)
   E028 00                  145 	.db	0		; bAlternateSetting
   E029 00                  146 	.db	0		; bNumEndpoints
   E02A FF                  147 	.db	0xff		; bInterfaceClass (vendor specific)
   E02B FF                  148 	.db	0xff		; bInterfaceSubClass (vendor specific)
   E02C FF                  149 	.db	0xff		; bInterfaceProtocol (vendor specific)
   E02D 03                  150 	.db	SI_COMMAND_AND_STATUS	; iInterface (description)
                            151 
                            152 	;; interface descriptor 1 (transmit path, ep2 OUT BULK)
                            153 	
   E02E 09                  154 	.db	DSCR_INTRFC_LEN
   E02F 04                  155 	.db	DSCR_INTRFC
   E030 01                  156 	.db	1		; bInterfaceNumber (zero based)
   E031 00                  157 	.db	0		; bAlternateSetting
   E032 01                  158 	.db	1		; bNumEndpoints
   E033 FF                  159 	.db	0xff		; bInterfaceClass (vendor specific)
   E034 FF                  160 	.db	0xff		; bInterfaceSubClass (vendor specific)
   E035 FF                  161 	.db	0xff		; bInterfaceProtocol (vendor specific)
   E036 04                  162 	.db	SI_TX_PATH	; iInterface (description)
                            163 
                            164 	;; interface 1's end point
                            165 
   E037 07                  166 	.db	DSCR_ENDPNT_LEN
   E038 05                  167 	.db	DSCR_ENDPNT
   E039 02                  168 	.db	0x02		; bEndpointAddress (ep 2 OUT)
   E03A 02                  169 	.db	ET_BULK		; bmAttributes
   E03B 00                  170 	.db	<512		; wMaxPacketSize (LSB)
   E03C 02                  171 	.db	>512		; wMaxPacketSize (MSB)
   E03D 00                  172 	.db	0		; bInterval (iso only)
                            173 
                            174 	;; interface descriptor 2 (receive path, ep6 IN BULK)
                            175 	
   E03E 09                  176 	.db	DSCR_INTRFC_LEN
   E03F 04                  177 	.db	DSCR_INTRFC
   E040 02                  178 	.db	2		; bInterfaceNumber (zero based)
   E041 00                  179 	.db	0		; bAlternateSetting
   E042 01                  180 	.db	1		; bNumEndpoints
   E043 FF                  181 	.db	0xff		; bInterfaceClass (vendor specific)
   E044 FF                  182 	.db	0xff		; bInterfaceSubClass (vendor specific)
   E045 FF                  183 	.db	0xff		; bInterfaceProtocol (vendor specific)
   E046 05                  184 	.db	SI_RX_PATH	; iInterface (description)
                            185 
                            186 	;; interface 2's end point
                            187 
   E047 07                  188 	.db	DSCR_ENDPNT_LEN
   E048 05                  189 	.db	DSCR_ENDPNT
   E049 86                  190 	.db	0x86		; bEndpointAddress (ep 6 IN)
   E04A 02                  191 	.db	ET_BULK		; bmAttributes
   E04B 00                  192 	.db	<512		; wMaxPacketSize (LSB)
   E04C 02                  193 	.db	>512		; wMaxPacketSize (MSB)
   E04D 00                  194 	.db	0		; bInterval (iso only)
                            195 
   E04E                     196 _high_speed_config_descr_end:		
                            197 
                            198 ;;; ----------------------------------------------------------------
                            199 ;;; descriptors used when operating at full speed (12Mb/sec)
                            200 ;;; ----------------------------------------------------------------
                            201 
   E04E                     202 	.even
   E04E                     203 _full_speed_device_descr::	
   E04E 12                  204 	.db	DSCR_DEVICE_LEN
   E04F 01                  205 	.db	DSCR_DEVICE
   E050 00                  206 	.db	<0x0200		; Specification version (LSB)
   E051 02                  207 	.db	>0x0200		; Specification version (MSB)
   E052 FF                  208 	.db	0xff		; device class (vendor specific)
   E053 FF                  209 	.db	0xff		; device subclass (vendor specific)
   E054 FF                  210 	.db	0xff		; device protocol (vendor specific)
   E055 40                  211 	.db	64		; bMaxPacketSize0 for endpoint 0
   E056 FE                  212 	.db	<VID_FREE	; idVendor
   E057 FF                  213 	.db	>VID_FREE	; idVendor
   E058 07                  214 	.db	<PID_USRP	; idProduct
   E059 00                  215 	.db	>PID_USRP	; idProduct
   E05A                     216 _usb_desc_hw_rev_binary_patch_location_1::
   E05A 00                  217 	.db	<DID_USRP	; bcdDevice
   E05B 01                  218 	.db	>DID_USRP	; bcdDevice
   E05C 01                  219 	.db	SI_VENDOR	; iManufacturer (string index)
   E05D 02                  220 	.db	SI_PRODUCT	; iProduct (string index)
   E05E 00                  221 	.db	SI_NONE		; iSerial number (None)
   E05F 01                  222 	.db	1		; bNumConfigurations
                            223 	
                            224 	
                            225 ;;; describes the other speed (480Mb/sec)
   E060                     226 	.even
   E060                     227 _full_speed_devqual_descr::
   E060 0A                  228 	.db	DSCR_DEVQUAL_LEN
   E061 06                  229 	.db	DSCR_DEVQUAL
   E062 00                  230 	.db	<0x0200		; bcdUSB
   E063 02                  231 	.db	>0x0200		; bcdUSB
   E064 FF                  232 	.db	0xff		; bDeviceClass
   E065 FF                  233 	.db	0xff		; bDeviceSubClass
   E066 FF                  234 	.db	0xff		; bDeviceProtocol
   E067 40                  235 	.db	64		; bMaxPacketSize0
   E068 01                  236 	.db	1		; bNumConfigurations (one config at 480Mb/sec)
   E069 00                  237 	.db	0		; bReserved
                            238 	
   E06A                     239 	.even
   E06A                     240 _full_speed_config_descr::	
   E06A 09                  241 	.db	DSCR_CONFIG_LEN
   E06B 02                  242 	.db	DSCR_CONFIG
   E06C 12                  243 	.db	<(_full_speed_config_descr_end - _full_speed_config_descr) ; LSB
   E06D 00                  244 	.db	>(_full_speed_config_descr_end - _full_speed_config_descr) ; MSB
   E06E 01                  245 	.db	1		; bNumInterfaces
   E06F 01                  246 	.db	1		; bConfigurationValue
   E070 00                  247 	.db	0		; iConfiguration
   E071 C0                  248 	.db	0x80 | bmSELF_POWERED ; bmAttributes
   E072 00                  249 	.db	0		; bMaxPower
                            250 
                            251 	;; interface descriptor 0 (command & status, ep0 COMMAND)
                            252 	
   E073 09                  253 	.db	DSCR_INTRFC_LEN
   E074 04                  254 	.db	DSCR_INTRFC
   E075 00                  255 	.db	0		; bInterfaceNumber (zero based)
   E076 00                  256 	.db	0		; bAlternateSetting
   E077 00                  257 	.db	0		; bNumEndpoints
   E078 FF                  258 	.db	0xff		; bInterfaceClass (vendor specific)
   E079 FF                  259 	.db	0xff		; bInterfaceSubClass (vendor specific)
   E07A FF                  260 	.db	0xff		; bInterfaceProtocol (vendor specific)
   E07B 03                  261 	.db	SI_COMMAND_AND_STATUS	; iInterface (description)
                            262 	
   E07C                     263 _full_speed_config_descr_end:	
                            264 	
                            265 ;;; ----------------------------------------------------------------
                            266 ;;;			string descriptors
                            267 ;;; ----------------------------------------------------------------
                            268 
   E07C                     269 _nstring_descriptors::
   E07C 07                  270 	.db	(_string_descriptors_end - _string_descriptors) / 2
                            271 
   E07D                     272 _string_descriptors::
   E07D 8C E0               273 	.db	<str0, >str0
   E07F 92 E0               274 	.db	<str1, >str1
   E081 BA E0               275 	.db	<str2, >str2
   E083 D2 E0               276 	.db	<str3, >str3
   E085 F4 E0               277 	.db	<str4, >str4
   E087 10 E1               278 	.db	<str5, >str5
   E089 2A E1               279 	.db	<str6, >str6
   E08B                     280 _string_descriptors_end:
                            281 
                    0000    282 	SI_NONE = 0
                            283 	;; str0 contains the language ID's.
   E08C                     284 	.even
   E08C 06                  285 str0:	.db	str0_end - str0
   E08D 03                  286 	.db	DSCR_STRING
   E08E 00                  287 	.db	0
   E08F 00                  288 	.db	0
   E090 09                  289 	.db	<0x0409		; magic code for US English (LSB)
   E091 04                  290 	.db	>0x0409		; magic code for US English (MSB)
   E092                     291 str0_end:
                            292 
                    0001    293 	SI_VENDOR = 1
   E092                     294 	.even
   E092 28                  295 str1:	.db	str1_end - str1
   E093 03                  296 	.db	DSCR_STRING
   E094 46 00               297 	.db	'F, 0		; 16-bit unicode
   E096 72 00               298 	.db	'r, 0
   E098 65 00               299 	.db	'e, 0
   E09A 65 00               300 	.db	'e, 0
   E09C 20 00               301 	.db	' , 0
   E09E 53 00               302 	.db	'S, 0
   E0A0 6F 00               303 	.db	'o, 0
   E0A2 66 00               304 	.db	'f, 0
   E0A4 74 00               305 	.db	't, 0
   E0A6 77 00               306 	.db	'w, 0
   E0A8 61 00               307 	.db	'a, 0
   E0AA 72 00               308 	.db	'r, 0
   E0AC 65 00               309 	.db	'e, 0
   E0AE 20 00               310 	.db	' , 0
   E0B0 46 00               311 	.db	'F, 0
   E0B2 6F 00               312 	.db	'o, 0
   E0B4 6C 00               313 	.db	'l, 0
   E0B6 6B 00               314 	.db	'k, 0
   E0B8 73 00               315 	.db	's, 0
   E0BA                     316 str1_end:
                            317 
                    0002    318 	SI_PRODUCT = 2
   E0BA                     319 	.even
   E0BA 18                  320 str2:	.db	str2_end - str2
   E0BB 03                  321 	.db	DSCR_STRING
   E0BC 48 00               322 	.db	'H, 0
   E0BE 50 00               323 	.db	'P, 0
   E0C0 53 00               324 	.db	'S, 0
   E0C2 44 00               325 	.db	'D, 0
   E0C4 52 00               326 	.db	'R, 0
   E0C6 20 00               327 	.db	' , 0
   E0C8 52 00               328 	.db	'R, 0
   E0CA 65 00               329 	.db	'e, 0
   E0CC 76 00               330 	.db	'v, 0
   E0CE 20 00               331 	.db	' , 0
   E0D0                     332 _usb_desc_hw_rev_ascii_patch_location_0::
   E0D0 3F 00               333 	.db	'?, 0
   E0D2                     334 str2_end:
                            335 
                    0003    336 	SI_COMMAND_AND_STATUS = 3
   E0D2                     337 	.even
   E0D2 22                  338 str3:	.db	str3_end - str3
   E0D3 03                  339 	.db	DSCR_STRING
   E0D4 43 00               340 	.db	'C, 0
   E0D6 6F 00               341 	.db	'o, 0
   E0D8 6D 00               342 	.db	'm, 0
   E0DA 6D 00               343 	.db	'm, 0
   E0DC 61 00               344 	.db	'a, 0
   E0DE 6E 00               345 	.db	'n, 0
   E0E0 64 00               346 	.db	'd, 0
   E0E2 20 00               347 	.db	' , 0
   E0E4 26 00               348 	.db	'&, 0
   E0E6 20 00               349 	.db	' , 0
   E0E8 53 00               350 	.db	'S, 0
   E0EA 74 00               351 	.db	't, 0
   E0EC 61 00               352 	.db	'a, 0
   E0EE 74 00               353 	.db	't, 0
   E0F0 75 00               354 	.db	'u, 0
   E0F2 73 00               355 	.db	's, 0
   E0F4                     356 str3_end:
                            357 
                    0004    358 	SI_TX_PATH = 4
   E0F4                     359 	.even
   E0F4 1C                  360 str4:	.db	str4_end - str4
   E0F5 03                  361 	.db	DSCR_STRING
   E0F6 54 00               362 	.db	'T, 0
   E0F8 72 00               363 	.db	'r, 0
   E0FA 61 00               364 	.db	'a, 0
   E0FC 6E 00               365 	.db	'n, 0
   E0FE 73 00               366 	.db	's, 0
   E100 6D 00               367 	.db	'm, 0
   E102 69 00               368 	.db	'i, 0
   E104 74 00               369 	.db	't, 0
   E106 20 00               370 	.db	' , 0
   E108 50 00               371 	.db	'P, 0
   E10A 61 00               372 	.db	'a, 0
   E10C 74 00               373 	.db	't, 0
   E10E 68 00               374 	.db	'h, 0
   E110                     375 str4_end:
                            376 
                    0005    377 	SI_RX_PATH = 5
   E110                     378 	.even
   E110 1A                  379 str5:	.db	str5_end - str5
   E111 03                  380 	.db	DSCR_STRING
   E112 52 00               381 	.db	'R, 0
   E114 65 00               382 	.db	'e, 0
   E116 63 00               383 	.db	'c, 0
   E118 65 00               384 	.db	'e, 0
   E11A 69 00               385 	.db	'i, 0
   E11C 76 00               386 	.db	'v, 0
   E11E 65 00               387 	.db	'e, 0
   E120 20 00               388 	.db	' , 0
   E122 50 00               389 	.db	'P, 0
   E124 61 00               390 	.db	'a, 0
   E126 74 00               391 	.db	't, 0
   E128 68 00               392 	.db	'h, 0
   E12A                     393 str5_end:
                            394 
                    0006    395 	SI_SERIAL = 6
   E12A                     396 	.even
   E12A 12                  397 str6:	.db	str6_end - str6
   E12B 03                  398 	.db	DSCR_STRING
   E12C                     399 _usb_desc_serial_number_ascii::
   E12C 33 00               400 	.db	'3, 0
   E12E 2E 00               401 	.db	'., 0
   E130 31 00               402 	.db	'1, 0
   E132 34 00               403 	.db	'4, 0
   E134 31 00               404 	.db	'1, 0
   E136 35 00               405 	.db	'5, 0
   E138 39 00               406 	.db	'9, 0
   E13A 33 00               407 	.db	'3, 0
   E13C                     408 str6_end:
                            409 
