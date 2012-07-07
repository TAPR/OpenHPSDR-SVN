/**
* @file  cusdr_complex.h
* @brief complex types header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-05-14
*/

/*   
 *   Copyright (C) 2007, 2008, 2009, 2010 Philip A Covington, N8VB
 *
 *	 adapted for cuSDR by (C) 2011, 2012 Hermann von Hasseln, DL3HVH
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Library General Public License version 2 as
 *   published by the Free Software Foundation
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details
 *
 *   You should have received a copy of the GNU Library General Public
 *   License along with this program; if not, write to the
 *   Free Software Foundation, Inc.,
 *   51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

#ifndef _CUSDR_COMPLEX_H
#define _CUSDR_COMPLEX_H

#include <cstdlib>
#include <cmath>
#include <vector>

using std::vector;

typedef struct _COMPLEX {
	
	float re;
	float im;

} CPX;

inline void *malloc16(int size) {

    void *p;
    void **p1;

    if((p = malloc(size+31)) == NULL)
        return NULL;

    p1 = (void **)(((long)p + 31) & (~15));
    p1[-1] = p;
    return (void *)p1;
}

inline CPX *mallocCPX(int size)
{
    void *p;
    void **p1;

    int msz = sizeof(CPX) * size;

    if((p = malloc(msz+31)) == NULL)
        return NULL;

    p1 = (void **)(((long)p + 31) & (~15));
    p1[-1] = p;
    return (CPX *)p1;
}

inline void free16(void *memory)
{
    if (memory != NULL)
        free(((void**)memory)[-1]);

}

inline void freeCPX(CPX *memory)
{
    if (memory != NULL)
        free(((void**)memory)[-1]);

}

inline CPX ToCPX(float x, float y) {
	
	CPX z;
	z.re = x, z.im = y;
	return z;
}

inline CPX ScaleCPX(CPX c, float a) {
	
	CPX z;
	z.re = c.re * a;
	z.im = c.im * a;
	
	return z;
}

inline CPX AddCPX(CPX x, CPX y) {
	
	CPX z;
	z.re = x.re + y.re;
	z.im = x.im + y.im;
	return z;
}

inline CPX SubCPX(CPX x, CPX y) {
	
	CPX z;
	z.re = x.re - y.re;
	z.im = x.im - y.im;
	return z;
}

inline CPX MultCPX(CPX x, CPX y) {
	
	CPX z;
	z.re = x.re * y.re - x.im * y.im;
	z.im = x.im * y.re + x.re * y.im;
	return z;
}

inline CPX DivCPX(CPX x, CPX y) {
	
	float d = y.re * y.re + y.im * y.im;
	CPX z;
	z.re = (x.re * y.re + x.im * y.im) / d;
	z.im = (y.re * x.im - y.im * x.re) / d;
	
	return z;
}

inline float MagCPX(CPX z) {
	
	return (float) (z.re * z.re + z.im * z.im);
}

inline float SqrMagCPX(CPX z) {
	
	return (float) sqrt(z.re * z.re + z.im * z.im);
}

#endif // _CUSDR_COMPLEX_H
