;;; -*- asm -*-
;;;
;;;-----------------------------------------------------------------------------
;;; USB Descriptor(s)
;;;-----------------------------------------------------------------------------
;;; Copyright 2005..2007 Kolja Waschk, ixo.de
;;;-----------------------------------------------------------------------------
;;; Code based on USRP2 firmware (GNU Radio Project), version 3.0.2,
;;; Copyright 2003 Free Software Foundation, Inc.
;;;-----------------------------------------------------------------------------
;;; This code is part of usbjtag. usbjtag is free software; you can redistribute
;;; it and/or modify it under the terms of the GNU General Public License as
;;; published by the Free Software Foundation; either version 2 of the License,
;;; or (at your option) any later version. usbjtag is distributed in the hope
;;; that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
;;; warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
;;; GNU General Public License for more details.  You should have received a
;;; copy of the GNU General Public License along with this program in the file
;;; COPYING; if not, write to the Free Software Foundation, Inc., 51 Franklin
;;; St, Fifth Floor, Boston, MA  02110-1301  USA
;;;-----------------------------------------------------------------------------

        .module usb_descriptors

        VID              = 0x16C0         ; Vendor ID 0x16C0  
        PID              = 0x06AD         ; Product ID 0x06AD 
        VERSION          = 0x0004         ; Product Version (4 indicates *BM device) 
        USB_VER          = 0x0110         ; Support USB version 1.10 
        USB_ATTR         = 0x80           ; Bus powered, not self-powered, no remote wakeup
        FTD_ATTR         = 0x001C         ; Set USB version, use version string, enable suspend PD
        MAX_POWER        = 75             ; need 2*75 mA max

        DSCR_DEVICE      =   1        ; Descriptor type: Device
        DSCR_CONFIG      =   2        ; Descriptor type: Configuration
        DSCR_STRING      =   3        ; Descriptor type: String
        DSCR_INTRFC      =   4        ; Descriptor type: Interface
        DSCR_ENDPNT      =   5        ; Descriptor type: Endpoint
        DSCR_DEVQUAL     =   6        ; Descriptor type: Device Qualifier
        
        DSCR_DEVICE_LEN  =  18
        DSCR_CONFIG_LEN  =   9
        DSCR_INTRFC_LEN  =   9
        DSCR_ENDPNT_LEN  =   7
        DSCR_DEVQUAL_LEN =  10
        
        ET_CONTROL       =   0        ; Endpoint type: Control
        ET_ISO           =   1        ; Endpoint type: Isochronous
        ET_BULK          =   2        ; Endpoint type: Bulk
        ET_INT           =   3        ; Endpoint type: Interrupt
        
;;; --------------------------------------------------------
;;;        external ram data
;;;--------------------------------------------------------
        
        .area USBDESCSEG    (XDATA)
        
;;; ----------------------------------------------------------------
;;; descriptors used when operating at high speed (480Mb/sec)
;;; ----------------------------------------------------------------
        
        .even        ; descriptors must be 2-byte aligned for SUDPTR{H,L} to work

        ;; The .even directive isn't really honored by the linker.  Bummer!
        ;; (There's no way to specify an alignment requirement for a given area,
        ;; hence when they're concatenated together, even doesn't work.)
        ;; 
        ;; We work around this by telling the linker to put USBDESCSEG
        ;; at 0xE000 absolute.  This means that the maximimum length of this
        ;; segment is 480 bytes, leaving room for the two hash slots 
        ;; at 0xE1EO to 0xE1FF.  
        
_high_speed_device_descr::
        .db        DSCR_DEVICE_LEN
        .db        DSCR_DEVICE
_dscr_usbver::
        .db        <USB_VER         ; Specification version (LSB)
        .db        >USB_VER         ; Specification version (MSB)
        .db        0x00             ; device class (vendor specific)
        .db        0x00             ; device subclass (vendor specific)
        .db        0x00             ; device protocol (vendor specific)
        .db        64               ; bMaxPacketSize0 for endpoint 0
_dscr_vidpidver::
        .db        <VID             ; idVendor
        .db        >VID             ; idVendor
        .db        <PID             ; idProduct
        .db        >PID             ; idProduct
        .db        <VERSION         ; bcdDevice
        .db        >VERSION         ; bcdDevice
_dscr_strorder::
        .db        SI_VENDOR        ; iManufacturer (string index)
        .db        SI_PRODUCT       ; iProduct (string index)
        .db        SI_SERIAL        ; iSerial number (string index)
        .db        1                ; bNumConfigurations
        
        .even
_high_speed_devqual_descr::
        .db        DSCR_DEVQUAL_LEN
        .db        DSCR_DEVQUAL
        .db        <USB_VER         ; bcdUSB (LSB)
        .db        >USB_VER         ; bcdUSB (MSB)
        .db        0xFF             ; bDeviceClass
        .db        0xFF             ; bDeviceSubClass
        .db        0xFF             ; bDeviceProtocol
        .db        64               ; bMaxPacketSize0
        .db        1                ; bNumConfigurations (one config at 12Mb/sec)
        .db        0                ; bReserved
        
        .even
_high_speed_config_descr::        
        .db        DSCR_CONFIG_LEN
        .db        DSCR_CONFIG
        .db        <(_high_speed_config_descr_end - _high_speed_config_descr) ; LSB
        .db        >(_high_speed_config_descr_end - _high_speed_config_descr) ; MSB
        .db        1                ; bNumInterfaces
        .db        1                ; bConfigurationValue
        .db        0                ; iConfiguration
_dscr_attrpow::
        .db        USB_ATTR         ; bmAttributes
        .db        MAX_POWER        ; bMaxPower [Unit: 0.5 mA]

        ;; interface descriptor
        
        .db        DSCR_INTRFC_LEN
        .db        DSCR_INTRFC
        .db        0                ; bInterfaceNumber (zero based)
        .db        0                ; bAlternateSetting
        .db        2                ; bNumEndpoints
        .db        0xFF             ; bInterfaceClass (vendor specific)
        .db        0xFF             ; bInterfaceSubClass (vendor specific)
        .db        0xFF             ; bInterfaceProtocol (vendor specific)
        .db        SI_PRODUCT       ; iInterface (description)

        ;; endpoint descriptor

        .db        DSCR_ENDPNT_LEN
        .db        DSCR_ENDPNT
        .db        0x81             ; bEndpointAddress (ep 1 IN)
        .db        ET_BULK          ; bmAttributes
        .db        <64              ; wMaxPacketSize (LSB)
        .db        >64              ; wMaxPacketSize (MSB)
        .db        0                ; bInterval (iso only)

        ;; endpoint descriptor

        .db        DSCR_ENDPNT_LEN
        .db        DSCR_ENDPNT
        .db        0x02             ; bEndpointAddress (ep 1 IN)
        .db        ET_BULK          ; bmAttributes
        .db        <64              ; wMaxPacketSize (LSB)
        .db        >64              ; wMaxPacketSize (MSB)
        .db        0                ; bInterval (iso only)

_high_speed_config_descr_end:                

;;; ----------------------------------------------------------------
;;; descriptors used when operating at full speed (12Mb/sec)
;;; ----------------------------------------------------------------

        .even
_full_speed_device_descr::        
        .db        DSCR_DEVICE_LEN
        .db        DSCR_DEVICE
        .db        <USB_VER         ; Specification version (LSB)
        .db        >USB_VER         ; Specification version (MSB)
        .db        0x00             ; device class (vendor specific)
        .db        0x00             ; device subclass (vendor specific)
        .db        0x00             ; device protocol (vendor specific)
        .db        64               ; bMaxPacketSize0 for endpoint 0
        .db        <VID             ; idVendor
        .db        >VID             ; idVendor
        .db        <PID             ; idProduct
        .db        >PID             ; idProduct
        .db        <VERSION         ; bcdDevice
        .db        >VERSION         ; bcdDevice
        .db        SI_VENDOR        ; iManufacturer (string index)
        .db        SI_PRODUCT       ; iProduct (string index)
        .db        SI_SERIAL        ; iSerial number (None)
        .db        1                ; bNumConfigurations
        
;;; describes the other speed (480Mb/sec)
        .even
_full_speed_devqual_descr::
        .db        DSCR_DEVQUAL_LEN
        .db        DSCR_DEVQUAL
        .db        <USB_VER         ; bcdUSB
        .db        >USB_VER         ; bcdUSB
        .db        0xFF             ; bDeviceClass
        .db        0xFF             ; bDeviceSubClass
        .db        0xFF             ; bDeviceProtocol
        .db        64               ; bMaxPacketSize0
        .db        1                ; bNumConfigurations (one config at 480Mb/sec)
        .db        0                ; bReserved
        
        .even
_full_speed_config_descr::        
        .db        DSCR_CONFIG_LEN
        .db        DSCR_CONFIG
        .db        <(_full_speed_config_descr_end - _full_speed_config_descr) ; LSB
        .db        >(_full_speed_config_descr_end - _full_speed_config_descr) ; MSB
        .db        1                ; bNumInterfaces
        .db        1                ; bConfigurationValue
        .db        0                ; iConfiguration
        .db        USB_ATTR         ; bmAttributes
        .db        MAX_POWER        ; bMaxPower [Unit: 0.5 mA]

        ;; interface descriptor
        
        .db        DSCR_INTRFC_LEN
        .db        DSCR_INTRFC
        .db        0                ; bInterfaceNumber (zero based)
        .db        0                ; bAlternateSetting
        .db        2                ; bNumEndpoints
        .db        0xFF             ; bInterfaceClass (vendor specific)
        .db        0xFF             ; bInterfaceSubClass (vendor specific)
        .db        0xFF             ; bInterfaceProtocol (vendor specific)
        .db        SI_PRODUCT       ; iInterface (description)

        ;; endpoint descriptor

        .db        DSCR_ENDPNT_LEN
        .db        DSCR_ENDPNT
        .db        0x81             ; bEndpointAddress (ep 1 IN)
        .db        ET_BULK          ; bmAttributes
        .db        <64              ; wMaxPacketSize (LSB)
        .db        >64              ; wMaxPacketSize (MSB)
        .db        0                ; bInterval (iso only)

        ;; endpoint descriptor

        .db        DSCR_ENDPNT_LEN
        .db        DSCR_ENDPNT
        .db        0x02             ; bEndpointAddress (ep 1 IN)
        .db        ET_BULK          ; bmAttributes
        .db        <64              ; wMaxPacketSize (LSB)
        .db        >64              ; wMaxPacketSize (MSB)
        .db        0                ; bInterval (iso only)
        
_full_speed_config_descr_end:        
        
;;; ----------------------------------------------------------------
;;;                        string descriptors
;;; ----------------------------------------------------------------

_nstring_descriptors::
        .db        (_string_descriptors_end - _string_descriptors) / 2

_string_descriptors::
        .db        <str0, >str0
        .db        <str1, >str1
        .db        <str2, >str2
        .db        <str3, >str3
_string_descriptors_end:

        SI_NONE = 0
        ;; str0 contains the language ID's.
        .even
_str0::
str0:   .db        str0_end - str0
        .db        DSCR_STRING
        .db        0
        .db        0
        .db        <0x0409          ; magic code for US English (LSB)
        .db        >0x0409          ; magic code for US English (MSB)
str0_end:

        SI_VENDOR = 1
        .even
_str1::
str1:   .db        str1_end - str1
        .db        DSCR_STRING
        .db        'i, 0            ; 16-bit unicode
        .db        'x, 0
        .db        'o, 0
        .db        '., 0
        .db        'd, 0
        .db        'e, 0
str1_end:

        SI_PRODUCT = 2
        .even
_str2::
str2:   .db        str2_end - str2
        .db        DSCR_STRING
        .db        'U, 0
        .db        'S, 0
        .db        'B, 0
        .db        '-, 0
        .db        'J, 0
        .db        'T, 0
        .db        'A, 0
        .db        'G, 0
        .db        '-, 0
        .db        'I, 0
        .db        'F, 0
str2_end:

        SI_SERIAL = 3
        .even
_str3::
str3:   .db        str3_end - str3
        .db        DSCR_STRING
        .db        '0, 0
        .db        '0, 0
        .db        '0, 0
        .db        '0, 0
        .db        '0, 0
        .db        '0, 0
        .db        '0, 0
        .db        '0, 0
str3_end:

