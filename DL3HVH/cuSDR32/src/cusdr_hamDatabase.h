/**
* @file  cusdr_hamDatabase.h
* @brief Ham database header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2012-01-27
*/

/*   
 *   Copyright 2011, 2012 Hermann von Hasseln, DL3HVH
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
 
#ifndef CUSDR_HAMDATABASE_H
#define CUSDR_HAMDATABASE_H

#include <QList>

typedef enum _iaruRegion {

  region1,		//  0
  region2,		//  1
  region3,		//  2

} IARURegion;

Q_DECLARE_METATYPE (_iaruRegion)

typedef enum _hamBand {

  m160,			//  0
  m80,			//  1
  m60,			//  2
  m40,			//  3
  m30,			//  4
  m20,			//  5
  m17,			//  6
  m15,			//  7
  m12,			//  8
  m10,			//  9
  m6,			// 10
  gen			// 11

} HamBand;

Q_DECLARE_METATYPE (_hamBand)

typedef enum _dspMode {

  LSB,			//  0
  USB,			//  1
  DSB,			//  2
  CWL,			//  3
  CWU,			//  4
  FMN,			//  5
  AM,			//  6
  DIGU,			//  7
  SPEC,			//  8
  DIGL,			//  9
  SAM,			// 10
  DRM			// 11

} DSPMode;

Q_DECLARE_METATYPE (_dspMode)

typedef enum _agcMode {

	agcOFF, 
	agcLONG, 
	agcSLOW, 
	agcMED, 
	agcFAST,
	agcUser

} AGCMode;

Q_DECLARE_METATYPE (_agcMode)

typedef enum _defaultFilterMode {

	filterLSB,
	filterUSB,
	filterDSB,
	filterCWL,
	filterCWU,
	filterFMN,
	filterAM,
	filterDIGU,
	filterSPEC,
	filterDIGL,
	filterSAM,
	filterDRM
	
} TDefaultFilterMode;

Q_DECLARE_METATYPE (_defaultFilterMode)

typedef struct _filter {

	DSPMode dspMode;
	TDefaultFilterMode defaultFilterMode;
	qreal filterLo;
	qreal filterHi;

} TDefaultFilter;

typedef struct _hamBandFrequencies {

	HamBand		hamBand;
	IARURegion	region;
	
	long frequencyLo;
	long frequencyHi;

} THamBandFrequencies;

typedef struct _hamBandText {

	HamBand		hamBand;
	IARURegion	region;

	long	frequencyLo;
	long	frequencyHi;
	int		maxBandwith;

	QString		text;

} THamBandText;

//***********************************************************************

inline QList<THamBandFrequencies> getHamBandFrequencies() {

	QList<THamBandFrequencies> hamBandFreqList;

	THamBandFrequencies hamBandFreq;

	hamBandFreq.frequencyLo = 1810000;
	hamBandFreq.frequencyHi = 2000000;
	hamBandFreq.hamBand = (HamBand) m160;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 3500000;
	hamBandFreq.frequencyHi = 3800000;
	hamBandFreq.hamBand = (HamBand) m80;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 5260000;
	hamBandFreq.frequencyHi = 5410000;
	hamBandFreq.hamBand = (HamBand) m60;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 7000000;
	hamBandFreq.frequencyHi = 7200000;
	hamBandFreq.hamBand = (HamBand) m40;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 10100000;
	hamBandFreq.frequencyHi = 10150000;
	hamBandFreq.hamBand = (HamBand) m30;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 14000000;
	hamBandFreq.frequencyHi = 14350000;
	hamBandFreq.hamBand = (HamBand) m20;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 18068000;
	hamBandFreq.frequencyHi = 18168000;
	hamBandFreq.hamBand = (HamBand) m17;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 21000000;
	hamBandFreq.frequencyHi = 21450000;
	hamBandFreq.hamBand = (HamBand) m15;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 24890000;
	hamBandFreq.frequencyHi = 24990000;
	hamBandFreq.hamBand = (HamBand) m12;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 28000000;
	hamBandFreq.frequencyHi = 29700000;
	hamBandFreq.hamBand = (HamBand) m10;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 50000000;
	hamBandFreq.frequencyHi = 54000000;
	hamBandFreq.hamBand = (HamBand) m6;
	hamBandFreq.region = (IARURegion) region1;

	hamBandFreqList << hamBandFreq;

	hamBandFreq.frequencyLo = 0;
	hamBandFreq.frequencyHi = 61440000;
	hamBandFreq.hamBand = (HamBand) gen;

	hamBandFreqList << hamBandFreq;

	return hamBandFreqList;
}

inline QList<THamBandText> getHamBandText() {

	QList<THamBandText> hamBandTextList;

	THamBandText hamBandText;

	hamBandText.frequencyLo = 1810000;
	hamBandText.frequencyHi = 1838000;
	hamBandText.hamBand = (HamBand) m160;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, 1836 kHz - QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 1838000;
	hamBandText.frequencyHi = 1840000;
	hamBandText.hamBand = (HamBand) m160;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 1840000;
	hamBandText.frequencyHi = 1843000;
	hamBandText.hamBand = (HamBand) m160;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes – digimodes, Lowest dial setting for LSB Voice mode: 1843, 3603 and 7053 kHz";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 1843000;
	hamBandText.frequencyHi = 2000000;
	hamBandText.hamBand = (HamBand) m160;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, Lowest dial setting for LSB Voice mode: 1843, 3603 and 7053 kHz";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3500000;
	hamBandText.frequencyHi = 3510000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, priority for intercontinental operation";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3510000;
	hamBandText.frequencyHi = 3560000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, contest preferred, 3555 kHz - QRS Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3560000;
	hamBandText.frequencyHi = 3580000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, 3560 kHz - QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3580000;
	hamBandText.frequencyHi = 3590000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3590000;
	hamBandText.frequencyHi = 3600000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3600000;
	hamBandText.frequencyHi = 3620000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "All modes - digimodes, automatically controlled data station (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3620000;
	hamBandText.frequencyHi = 3650000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, 3630 kHz - Digital Voice Centre of Activity, SSB contest preferred";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3650000;
	hamBandText.frequencyHi = 3700000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, 3690 kHz - SSB QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3700000;
	hamBandText.frequencyHi = 3775000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, SSB contest preferred, 3735 kHz: Image Centre of Activity, 3760 kHz: Region 1 Emergency Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 3775000;
	hamBandText.frequencyHi = 3800000;
	hamBandText.hamBand = (HamBand) m80;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, priority for intercontinental operation";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7000000;
	hamBandText.frequencyHi = 7040000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, 7030 kHz - QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7040000;
	hamBandText.frequencyHi = 7047000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7047000;
	hamBandText.frequencyHi = 7050000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes – digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7050000;
	hamBandText.frequencyHi = 7053000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7053000;
	hamBandText.frequencyHi = 7060000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes - digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7060000;
	hamBandText.frequencyHi = 7100000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, SSB contest preferred, 7070 kHz: Digital Voice Centre of Activity, 7090 kHz: SSB QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7100000;
	hamBandText.frequencyHi = 7130000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, 7110 kHz: Region 1 Emergency Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7130000;
	hamBandText.frequencyHi = 7175000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, SSB contest preferred, 7165 kHz: Image Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 7175000;
	hamBandText.frequencyHi = 7200000;
	hamBandText.hamBand = (HamBand) m40;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, priority for intercontinental operation";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 10100000;
	hamBandText.frequencyHi = 10140000;
	hamBandText.hamBand = (HamBand) m30;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW,   10116 kHz - QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 10140000;
	hamBandText.frequencyHi = 10150000;
	hamBandText.hamBand = (HamBand) m30;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes – digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14000000;
	hamBandText.frequencyHi = 14060000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, contest preferred, 14055 kHz - QRS Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14060000;
	hamBandText.frequencyHi = 14070000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, 14060 kHz - QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14070000;
	hamBandText.frequencyHi = 14089000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14089000;
	hamBandText.frequencyHi = 14099000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14099000;
	hamBandText.frequencyHi = 14101000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 0;
	hamBandText.text = "IBP, exclusively for beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14101000;
	hamBandText.frequencyHi = 14112000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14112000;
	hamBandText.frequencyHi = 14125000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14125000;
	hamBandText.frequencyHi = 14300000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, SSB contest preferred, 14130 kHz: Digital Voice Centre of Activity, 14195 kHz ± 5 kHz: Priority for Dxpeditions, 14230 kHz: Image Centre of Activity, 14285 kHz: SSB QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 14300000;
	hamBandText.frequencyHi = 14350000;
	hamBandText.hamBand = (HamBand) m20;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, 14300 kHz - Global Emergency centre of activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 18068000;
	hamBandText.frequencyHi = 18095000;
	hamBandText.hamBand = (HamBand) m17;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, 18086 kHz - QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 18095000;
	hamBandText.frequencyHi = 18105000;
	hamBandText.hamBand = (HamBand) m17;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 18105000;
	hamBandText.frequencyHi = 18109000;
	hamBandText.hamBand = (HamBand) m17;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 18109000;
	hamBandText.frequencyHi = 18111000;
	hamBandText.hamBand = (HamBand) m17;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 0;
	hamBandText.text = "IBP, exclusively for beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 18111000;
	hamBandText.frequencyHi = 18120000;
	hamBandText.hamBand = (HamBand) m17;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "IBP, exclusively for beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 18120000;
	hamBandText.frequencyHi = 18168000;
	hamBandText.hamBand = (HamBand) m17;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, 18130 kHz: SSB QRP Centre of Activity, 18150 kHz: Digital Voice Centre of Activity, 18160 kHz: Global Emergency Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 21000000;
	hamBandText.frequencyHi = 21070000;
	hamBandText.hamBand = (HamBand) m15;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, 21055 kHz: QRS Centre of Activity, 21060 kHz: QRP Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 21070000;
	hamBandText.frequencyHi = 21090000;
	hamBandText.hamBand = (HamBand) m15;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 21090000;
	hamBandText.frequencyHi = 21110000;
	hamBandText.hamBand = (HamBand) m15;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 21110000;
	hamBandText.frequencyHi = 21120000;
	hamBandText.hamBand = (HamBand) m15;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes (excluding SSB) - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 21120000;
	hamBandText.frequencyHi = 21149000;
	hamBandText.hamBand = (HamBand) m15;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 21149000;
	hamBandText.frequencyHi = 21151000;
	hamBandText.hamBand = (HamBand) m15;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 0;
	hamBandText.text = "IBP, exclusively for beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 21151000;
	hamBandText.frequencyHi = 21450000;
	hamBandText.hamBand = (HamBand) m15;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, 21180 kHz - Digital Voice Centre of Activity, 21285 kHz: SSB QRP Centre of Activity, 21340 kHz: Image Centre of Activity, 21360 kHz: Global Emergency Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 24890000;
	hamBandText.frequencyHi = 24915000;
	hamBandText.hamBand = (HamBand) m12;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, 24906 kHz - QRP centre of activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 24915000;
	hamBandText.frequencyHi = 24925000;
	hamBandText.hamBand = (HamBand) m12;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 24925000;
	hamBandText.frequencyHi = 24929000;
	hamBandText.hamBand = (HamBand) m12;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 24929000;
	hamBandText.frequencyHi = 24931000;
	hamBandText.hamBand = (HamBand) m12;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 0;
	hamBandText.text = "IBP, exclusively for beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 24931000;
	hamBandText.frequencyHi = 24940000;
	hamBandText.hamBand = (HamBand) m12;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 24940000;
	hamBandText.frequencyHi = 24990000;
	hamBandText.hamBand = (HamBand) m12;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, 24960 kHz - Digital Voice Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28000000;
	hamBandText.frequencyHi = 28070000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 200;
	hamBandText.text = "CW, 28055 kHz: QRS Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28070000;
	hamBandText.frequencyHi = 28120000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28120000;
	hamBandText.frequencyHi = 28150000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28150000;
	hamBandText.frequencyHi = 28190000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 500;
	hamBandText.text = "Narrow band modes";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28190000;
	hamBandText.frequencyHi = 28199000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 0;
	hamBandText.text = "IBP, regional time shared beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28199000;
	hamBandText.frequencyHi = 28201000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 0;
	hamBandText.text = "IBP, worldwide time shared beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28201000;
	hamBandText.frequencyHi = 28225000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 0;
	hamBandText.text = "IBP, continuous duty beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28225000;
	hamBandText.frequencyHi = 28300000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes - beacons";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28300000;
	hamBandText.frequencyHi = 28320000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 28320000;
	hamBandText.frequencyHi = 29100000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 2700;
	hamBandText.text = "All modes, 28330 kHz: Digital Voice Centre of Activity, 28360 kHz: SSB QRP Centre of Activity, 28680 kHz: Image Centre of Activity";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 29100000;
	hamBandText.frequencyHi = 29200000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 6000;
	hamBandText.text = "All modes - FM simplex – 10 kHz channels";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 29200000;
	hamBandText.frequencyHi = 29300000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 6000;
	hamBandText.text = "All modes - digimodes, automatically controlled data stations (unattended)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 29300000;
	hamBandText.frequencyHi = 29510000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 6000;
	hamBandText.text = "Satellite-downlink";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 29510000;
	hamBandText.frequencyHi = 29520000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 0;
	hamBandText.text = "Guard channel";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 29520000;
	hamBandText.frequencyHi = 29590000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 6000;
	hamBandText.text = "All modes - FM repeater input (RH1 – RH8)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 29590000;
	hamBandText.frequencyHi = 29600000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 6000;
	hamBandText.text = "All modes - FM calling channel";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 29600000;
	hamBandText.frequencyHi = 29610000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 6000;
	hamBandText.text = "All modes - FM simplex repeater (parrot - input and output)";

	hamBandTextList << hamBandText;

	hamBandText.frequencyLo = 29610000;
	hamBandText.frequencyHi = 29700000;
	hamBandText.hamBand = (HamBand) m10;
	hamBandText.region = (IARURegion) region1;
	hamBandText.maxBandwith = 6000;
	hamBandText.text = "All modes - FM repeater outputs (RH1 – RH8)";

	hamBandTextList << hamBandText;

	return hamBandTextList;
}

inline QList<TDefaultFilter> getDefaultFilterFrequencies() {

	QList<TDefaultFilter> defaultFilters;

	TDefaultFilter defaultFilter;

	defaultFilter.dspMode = (DSPMode) LSB;
	defaultFilter.defaultFilterMode = filterLSB;
	defaultFilter.filterLo = -3050.0f;
	defaultFilter.filterHi = -150.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) USB;
	defaultFilter.defaultFilterMode = filterUSB;
	defaultFilter.filterLo = 150.0f;
	defaultFilter.filterHi = 3050.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) DSB;
	defaultFilter.defaultFilterMode = filterDSB;
	defaultFilter.filterLo = -3300.0f;
	defaultFilter.filterHi = 3300.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) CWL;
	defaultFilter.defaultFilterMode = filterCWL;
	defaultFilter.filterLo = -1100.0f;
	defaultFilter.filterHi = -100.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) CWU;
	defaultFilter.defaultFilterMode = filterCWU;
	defaultFilter.filterLo = 100.0f;
	defaultFilter.filterHi = 1100.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) FMN;
	defaultFilter.defaultFilterMode = filterFMN;
	defaultFilter.filterLo = -2000.0f;
	defaultFilter.filterHi = 2000.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) AM;
	defaultFilter.defaultFilterMode = filterAM;
	defaultFilter.filterLo = -4000.0f;
	defaultFilter.filterHi = 4000.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) DIGU;
	defaultFilter.defaultFilterMode = filterDIGU;
	defaultFilter.filterLo = 150.0f;
	defaultFilter.filterHi = 3050.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) SPEC;
	defaultFilter.defaultFilterMode = filterSPEC;
	defaultFilter.filterLo = -6000.0f;
	defaultFilter.filterHi = 6000.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) DIGL;
	defaultFilter.defaultFilterMode = filterDIGL;
	defaultFilter.filterLo = -3050.0f;
	defaultFilter.filterHi = -150.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) SAM;
	defaultFilter.defaultFilterMode = filterSAM;
	defaultFilter.filterLo = -3300.0f;
	defaultFilter.filterHi = 3300.0f;

	defaultFilters << defaultFilter;

	defaultFilter.dspMode = (DSPMode) DRM;
	defaultFilter.defaultFilterMode = filterDRM;
	defaultFilter.filterLo = -6000.0f;
	defaultFilter.filterHi = 6000.0f;

	defaultFilters << defaultFilter;

	return defaultFilters;
}

inline HamBand getBandFromFrequency(const QList<THamBandFrequencies> bandList, long frequency) {

	HamBand band;

	for (int i = 0; i < bandList.size(); ++i) {
		
		if (bandList.at(i).frequencyLo <= frequency && bandList.at(i).frequencyHi >= frequency) {

			band = bandList.at(i).hamBand;
			return band;
		}
	}
	return (HamBand) gen;
	
}

inline TDefaultFilter getFilterFromDSPMode(const QList<TDefaultFilter> filterList, DSPMode mode) {

	TDefaultFilter filter;

	for (int i = 0; i < filterList.size(); ++i) {

		if (filterList.at(i).dspMode == mode) {

			filter = filterList.at(i);
			return filter;
		}
	}
	return filterList.at(0);
}

inline QString getHamBandTextString(const QList<THamBandText> textList, long frequency) {

	QString str = "";

	for (int i = 0; i < textList.size(); ++i) {

		if (textList.at(i).frequencyLo <= frequency && textList.at(i).frequencyHi >= frequency) {

			str = textList.at(i).text;
			return str;
		}
	}
	return str;
}

 
#endif // CUSDR_HAMDATABASE_H
