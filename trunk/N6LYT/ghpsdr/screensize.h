/*
* @file screensize.h
* @brief  definition of netbbok screensize
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-11
*/
// screensize.h

/* Copyright (C) 
* 2009 - John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
* 
*/

// netbook screen size changes from Erich Heinzle vk5hse
// uncomment the following line to build to fit a netbook screen
//#define NETBOOK

#ifdef NETBOOK
#define BUTTON_WIDTH 40
#define BUTTON_HEIGHT 23
#define LARGE_BUTTON_WIDTH 80
#else
#define BUTTON_WIDTH 50
#define BUTTON_HEIGHT 25
#define LARGE_BUTTON_WIDTH 65
#endif
