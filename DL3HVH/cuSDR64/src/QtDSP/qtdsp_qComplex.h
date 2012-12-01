/**
* @file  qtdsp_qComplex.h
* @brief qComplex type header for QtDSP
* @author by Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-09-22
*/

/*   
 *   Copyright 2011 Hermann von Hasseln, DL3HVH
 *
 *   Based on the complex type CPX by Philip A Covington, p.covington@gmail.com
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

#ifndef _QTDSP_QCOMPLEX_H
#define _QTDSP_QCOMPLEX_H

#include <cmath>
#include <limits>
#include <QVector>
#include <QVector2D>

using namespace std;

typedef struct _QCOMPLEX {

	float re;
	float im;

} cpx;

Q_DECLARE_METATYPE (cpx)
Q_DECLARE_TYPEINFO(cpx, Q_MOVABLE_TYPE );

typedef QVector<cpx> CPX;



inline void InitCPX(CPX &vec, int size, float value) {

	cpx zero;
	zero.re = value; zero.im = value;

	vec.resize(size);
	vec.fill(zero);
}

inline cpx ToCPX(qreal x, qreal y) {

	cpx z;
	z.re = x;
	z.im = y;

	return z;
}

inline cpx ScaleCPX(const cpx &c, float a) {

	cpx z;
	z.re = a * c.re;
	z.im = a * c.im;

	return z;
}

inline cpx AddCPX(cpx x, cpx y) {

	cpx z;
	z.re = x.re + y.re;
	z.im = x.im + y.im;

	return z;
}

inline void PlusCPX(CPX &a, CPX &b, CPX &c) {

	CPX z;
	z.resize(0);

	int sa = a.size();
	int sb = b.size();

	if (sa != sb) return;

	z.resize(sa);
	for (int i = 0; i < sa; i++) {

		z[i].re = a.at(i).re + b.at(i).re;
		z[i].im = a.at(i).im + b.at(i).im;

		c[i].re = z.at(i).re;
		c[i].im = z.at(i).im;
	}
}

inline cpx MultCPX(cpx x, cpx y) {

	cpx z;
	z.re = x.re * y.re - x.im * y.im;
	z.im = x.im * y.re + x.re * y.im;
	return z;
}

inline float MagCPX(cpx z) {

	return (float) (z.re * z.re + z.im * z.im);
}

inline float SqrMagCPX(cpx z) {

	return (float) sqrt(z.re * z.re + z.im * z.im);
}

inline QString ValidQReal(qreal value) {

    if (value != value) {
        return "NaN";
    }
    else if (value > std::numeric_limits<qreal>::max()){
        return "+Inf";
    }
    else if (value < -std::numeric_limits<qreal>::max()){
        return "-Inf";
    }
    else
        return "";
}

#endif // _QTDSP_QCOMPLEX_H
