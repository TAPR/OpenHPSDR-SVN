/*
---- Begin Copyright Notice

     This is part of the interfacing DLL between Winrad and the HPSDR, based on WinUSB
     Copyright (C) 2009  Alberto di Bene,  I2PHD

     This library is free software; you can redistribute it and/or
     modify it under the terms of the GNU Lesser General Public
     License as published by the Free Software Foundation; either
     version 2.1 of the License, or (at your option) any later version.

     This library is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
     Lesser General Public License for more details.

     You should have received a copy of the GNU Lesser General Public
     License along with this library; if not, write to the Free Software
     Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, 
     MA  02110-1301  USA


--- End Copyright Notice
*/
#ifndef HpsdrwusbH
  #define HpsdrwusbH

#include <winusb.h>

   struct  HPSDR_WUSB_INFO
   {
     HANDLE deviceHandle;
     WINUSB_INTERFACE_HANDLE winUSBHandle;
     UCHAR  deviceSpeed;
     UCHAR  bulkInPipe;
     UCHAR  rawInPipe;
     UCHAR  bulkOutPipe;
     int    FX2linesloaded;
     int    FPGAbytesloaded;
     int    numEndP;
     char*  FX2fname;
     char*  FPGAfname;
     char   lastMsg[256];
   };


#endif
