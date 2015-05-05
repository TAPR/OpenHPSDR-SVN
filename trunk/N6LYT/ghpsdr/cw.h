/** 
* @file cw.h
* @brief Header files to define the CW state.
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-11
*/
// cw.h

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

int cwinternal;
int cwkeysreversed; // 0=disabled 1=enabled
int cwkeyerspeed; // 1-60 WPM
#define KEYER_STRAIGHT  0
#define KEYER_MODE_A  1
#define KEYER_MODE_B  2
int cwkeyermode;

int cwkeyerweight; //0-100
int cwkeyerspacing; // 0=on 1=off

int cwinternal;  // 0=external, 1=internal
int cwsidetonevolume; // 0-127
int cwpttdelay; // 0-255ms
int cwhangtime; // ms
int cwsidetonefrequency; // Hz


int cwPitch;

short cwSin[240];
short cwCos[240];
int cwIndex;

void init_cw();

