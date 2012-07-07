/**
* @file  cusdr_chirpWidget.h
* @brief Chirp signal settings widget header file for cuSDR
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-05-21
*/

/*
 *   
 *   Copyright 2010, 2011 Hermann von Hasseln, DL3HVH
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
 
#ifndef _CUSDR_CHIRP_WIDGET_H
#define _CUSDR_CHIRP_WIDGET_H

#include <QWidget>
#include <QComboBox>
#include <QGroupBox>
#include <QSpinBox>
#include <QLineEdit>
#include <QtMultimedia/qaudio.h>

#include "cusdr_buttons.h"
#include "cusdr_settings.h"
//#include "audioengine/frequencyspectrum.h"


class ChirpWidget : public QWidget {

	Q_OBJECT

public:
	ChirpWidget(QWidget *parent = 0);
	~ChirpWidget();

protected:
	void resizeEvent(QResizeEvent *event);

public slots:
	QSize	sizeHint() const;
	QSize	minimumSizeHint() const;
	
private:
	void	setupConnections();
	void	createChirpModeGroup();
	void	createPlayFileGroup();
	void	createChirpSignalGroup();
	void	createChirpSpectrumGroup();
	void	createMatchedFilterControlGroup();
	void	createSpectrum();
	
private:
	Settings*	m_settings;

	QSDR::_ServerMode			m_serverMode;
	QSDR::_HWInterfaceMode	m_hwInterface;
	QSDR::_DataEngineState	m_dataEngineState;

	QString			m_menu_style;
	QFont			m_titleFont;
	QFont			m_smallFont;

	QString			lineedit_style;
	QLabel			*labelAudioPortLabel;
	QLabel			*labelAudioPortText;
	QLabel			*spectrumLabel;
	QLabel			*controlLabel;
	
	QGroupBox		*chirpModeGroupBox;
	QGroupBox		*playFileGroupBox;
	QGroupBox		*chirpSignalGroupBox;
	QGroupBox		*chirpSpectrumGroupBox;
	QGroupBox		*matchedFilterControlGroupBox;
	
	QComboBox		*m_inputDeviceComboBox;
    QComboBox		*m_outputDeviceComboBox;

	QSpinBox		*lowerFreqSpinBox;
	QSpinBox		*upperFreqSpinBox;
	QSpinBox		*timeSpinBox;
	//QSpinBox		*repetitionSpinBox;
	QSpinBox		*avgSpinBox;
	QSpinBox		*filterLowerFreqSpinBox;
	QSpinBox		*filterUpperFreqSpinBox;

	AeroButton		*chirpModeBtn;
	AeroButton		*loadFileBtn;
	AeroButton		*rcveBtn;
	AeroButton		*pauseBtn;
	AeroButton		*playBtn;
	AeroButton		*settingsBtn;
	AeroButton		*playChirpBtn;
	AeroButton		*avgBtn;
	AeroButton		*chirpLSBBtn;
	AeroButton		*chirpUSBBtn;
	AeroButton		*chirpForwardBtn;
	AeroButton		*chirpBackwardBtn;
	AeroButton		*showChirpFFTBtn;

    QIcon			pauseIcon;
    QIcon			playIcon;
    QIcon			settingsIcon;

	QRect			m_spectrumRect;
	QImage			m_sourceImage;
    QImage			m_destinationImage;
	QImage			m_resultImage;

	QImage*			createSpectrumImage(const QRect& rect);

	//QList<QImage*>	m_specImages;

	struct Stripe {
        Stripe() : value(0.0), clipped(false) { }
        qreal   value;
		bool    clipped;
    };

	QVector<Stripe>     m_stripes;
    int                 m_stripeSelected;
    int                 m_timerId;
    qreal               m_lowFreq;
    qreal               m_highFreq;
	FrequencySpectrum   m_spectrum;

	QList<FrequencySpectrum>	m_spectrumList;
	QList<QVector<Stripe>>		m_stripeList;

	int stripeIndex(qreal frequency) const;
    QPair<qreal, qreal> stripeRange(int stripeIndex) const;
	void stripeFreqRange();
	

	qreal		m_scale;
	qreal		m_timeScale;
	int			m_timeScaleMax;
	int			m_fontHeight;
	int			m_maxFontWidth;
	int			m_graphWidth;
	int			m_graphHeight;
	int			m_graphScaleX;

	qint64		m_chirpPosition;
	qint64		m_chirpLength;

	QVector<qreal>	m_freq;

	qint64		m_chirpBufferPosition;
    qint64		m_chirpBufferLength;
	QByteArray	m_chirpBuffer;

	bool		m_antialiased;
	bool		m_mouseOver;
	bool		m_showChirpFFT;
	bool		m_chirpUSB;
	int			m_minimumWidgetWidth;
	int			m_minimumGroupBoxWidth;
	int			m_btnSpacing;

	int			m_cnt;

private slots:
	/*void	systemStateChanged(
					QObject *sender, 
					QSDR::_Error err, 
					QSDR::_HWInterfaceMode hwmode, 
					QSDR::_ServerMode mode, 
					QSDR::_DataEngineState state);*/

	void	initChirpDecoder();
	void	showFileDialog();
	void	setReceiver();
	void	suspend();
	void	startPlayback();
	void	showSettingsDialog();
	void	showChirpFFT();
	void	switchSideband();

	void	setChirpLowerFrequency(QObject* sender, int lo);
	void	setChirpUpperFrequency(QObject* sender, int lo);
	void	setChirpAmplitude(QObject* sender, qreal amp);
	void	setChirpBufferDurationUs(QObject* sender, qint64 value);
	void	setChirpRepetitionTimes(QObject* sender, int value);
	void	setFilterLowerFrequency(QObject* sender, int lo);
	void	setFilterUpperFrequency(QObject* sender, int lo);

	//void	chirpBufferChanged(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer);
	void	chirpSpectrumChanged(qint64 position, qint64 length, const FrequencySpectrum &spectrum);
	void	chirpSpectrumListChanged(const QList<FrequencySpectrum> &spectrumList);
	void	resetSpectrum();
	void	updateStripes();
	void	stripeResize(int scale);
	//void	updateChirpSpectrum();
	//void selectBar(int index);
    
	
signals:
	void	showEvent(QObject *sender);
	void	closeEvent(QObject *sender);
	void	messageEvent(QString );
	void	initChirpDecoderEvent(QObject *sender, bool value);
	void	loadFileEvent(QObject *sender, const QString &fileName);
	void	suspendSignal(QObject *sender);
	void	startPlaybackSignal(QObject *sender);
	void	showSettingsDialogSignal(QObject *sender);
};

#endif // _CUSDR_CHIRP_WIDGET_H
