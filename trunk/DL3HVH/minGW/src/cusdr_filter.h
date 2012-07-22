/**
* @file  cusdr_filter.h
* @brief Filter header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-02-18
*/

/*   
 *   Copyright (C) 2007, 2008, 2009, 2010 Philip A Covington, N8VB
 *
 *	 adapted for cuSDR by (C) 2011 Hermann von Hasseln, DL3HVH
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

#ifndef _CUSDR_FILTER_H
#define	_CUSDR_FILTER_H

//#include <cstdlib>
//#include <cmath>
//#include <vector>
#include <string.h>
//#include <math.h>
#include "cusdr_fft.h"
#include "cusdr_complex.h"
#include "cusdr_invsinc_coeff.h"
#include "cusdr_settings.h"

#include <QObject>
#include <QMutex>



#define BLACKMANHARRIS_WINDOW   12

class QFilter : public QObject {

	Q_OBJECT 

public:
    //QFilter(float samplerate, int size);
	QFilter(QObject *parent = 0, int size = 0, const int ftype = 2, const int wtype = 12);
	~QFilter();

private:
	Settings	*m_settings;

	QMutex		mutex;

    int			m_size;
	int			m_stream_mode;
	int			m_ftype;
	int			m_wtype;

    float		m_samplerate;
	float		m_filter_lo;
    float		m_filter_hi;

    CPX		*ovlp;
    CPX		*filter;
    CPX		*tmpfilt0;
    CPX		*tmpfilt1;
	CPX		*tmp;
    CPX		*tmp0;
    CPX		*tmp1;

    QFFT	*ovlpfft;
    QFFT	*filtfft;

private slots:
    void LoadFilter(CPX * taps);
    void MakeFirLowpass(float cutoff, 
                        float samplerate, 
                        int wtype, 
                        float * taps_re, 
                        float * taps_im,
                        int length);

    void MakeFirLowpass(float cutoff,
                        float samplerate,
                        int wtype,
                        CPX * taps,
                        int length);
    
	void MakeFirBandpass(float lo,
                        float hi,
                        float samplerate,
                        int wtype,
                        float * taps_re,
                        float * taps_im,
                        int length);
    
	void MakeFirBandpass(float lo,
                        float hi,
                        float samplerate,
                        int wtype,
                        CPX * taps,
                        int length);
    
	void MakeFirBandstop(float lo,
                        float hi,
                        float samplerate,
                        CPX * taps,
                        int length);
    
	//void DoConvolutionCPX();
    void DoOverlapAddCPX();

public slots:
	void	setSampleRate(QObject *sender, int value);

    //void		MakeFilter(const float lo, const float hi, const int ftype);
	void	MakeFilter(const float lo, const float hi, const int ftype, const int wtype);
    static void MakeWindow(int wtype, int size, float * window);

    void	ProcessFilter(CPX* in, CPX* out, int bsize);
	void	ProcessForwardFilter(CPX* in, CPX* out, int bsize);
	void	ProcessChirpFilter(CPX* in, CPX* out, int bsize);
    int		ProcessAndDecimate(CPX* in, CPX* out, int bsize);
	void	Decimate(CPX* in, CPX* out, int downrate);
	void	DoConvolutionCPX();
	void	Normalize(CPX* in, CPX* out, int size);

    float filterLo() const ;
    float filterHi() const ;
    int isStreamMode() const;
    void setFilterLo(const float value);
    void setFilterHi(const float value);
    void setFilter(const float loval, const float hival);
    void setStreamMode(int value);
};

#endif	// _CUSDR_FILTER_H

