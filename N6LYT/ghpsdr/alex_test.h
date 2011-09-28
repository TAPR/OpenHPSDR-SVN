/** 
* @file alex_test.h
* @brief Alex Test
* @author John Melton, G0ORX/N6LYT, Doxygen Comments Dave Larsen, KV0S
* @version 0.1
* @date 2009-04-12
*/


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


//
// alex_test.h
//

struct _ALEX_TEST {
    int id;
    long long frequency;
    int rf_det_level;
    int rf_fwd_level;
    int rf_rev_level;
    char rf_source[8];
    char rf_min_level[64];
    char rf_max_level[64];
    char description[64];
    GtkWidget* testWidget;
    GtkWidget* frequencyWidget;
    GtkWidget* levelWidget;
    char pause[64];
};

typedef struct _ALEX_TEST ALEX_TEST;

