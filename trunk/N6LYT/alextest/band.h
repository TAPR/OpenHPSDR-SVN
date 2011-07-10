/** 
* @file band.h
* @brief Header files for the Amateur Radio band stack.
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-11
*/
// band.h

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

#define band160 0
#define band80 1
#define band60 2
#define band40 3
#define band30 4
#define band20 5
#define band17 6
#define band15 7
#define band12 8
#define band10 9
#define band6 10
#define bandGen 11
#define bandWWV 12
#define bandXVTR 13

#define BANDS 14

struct _BAND_LIMITS {
    long long minFrequency;
    long long maxFrequency;
};

typedef struct _BAND_LIMITS BAND_LIMITS;

int band;
int xvtr_band;
gboolean displayHF;

void bandSaveState();
void bandRestoreState();
void forceBand(int band);
void configureXVTRButton();
GtkWidget* buildBandUI();

int remoteSetBand(gpointer *data);

BAND_LIMITS* getBandLimits(long long minDisplay,long long maxDisplay);
XVTR_ENTRY* getXvtrEntry(int i);
