/** 
* @file filter.h
* @brief Header files to define the filters.
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-11
*/
// filter.h
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

#define filter6000 0
#define filter4000 1
#define filter2600 2
#define filter2100 3
#define filter1000 4
#define filter500 5
#define filter250 6
#define filter100 7
#define filter50 8
#define filter25 9
#define filterVar1 10
#define filterVar2 11

int filter;

int filterLow;
int filterHigh;

int txFilterLowCut;
int txFilterHighCut;

int filterVar1Low;
int filterVar1High;
int filterVar2Low;
int filterVar2High;


GtkWidget* buildFilterUI();
void filterSaveState();
void filterRestoreState();
void setFilter(int filter);
void setTxFilters();
