/** 
* @file bandstack.h
* @brief Bandstack definition files
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-11
*/
// bandstack.h

/* Copyright (C) 
* This program is free software; you can redistribute it and/or2009 - John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
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

/* --------------------------------------------------------------------------*/
/** 
* @brief Bandstack definition
*/
struct _BANDSTACK_ENTRY {
    long long frequencyA;
    int mode;
    int filter;
    int var1Low;
    int var1High;
    int var2Low;
    int var2High;
    int step;
    int preamp;
    int spectrumHigh;
    int spectrumLow;
    int spectrumStep;
    int waterfallHigh;
    int waterfallLow;
};

typedef struct _BANDSTACK_ENTRY BANDSTACK_ENTRY;

struct _BANDSTACK {
    int entries;
    int current_entry;
    BANDSTACK_ENTRY* entry;
};

typedef struct _BANDSTACK BANDSTACK;

