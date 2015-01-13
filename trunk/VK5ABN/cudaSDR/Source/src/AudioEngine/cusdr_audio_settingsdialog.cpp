/**
* @file cusdr_audio_settingsdialog.cpp
* @brief cuSDR audio settings dialogue class
* @author adaptation for cuSDR by Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2011-04-02
*/

/****************************************************************************
**
** Copyright (C) 2011 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** This file is part of the examples of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:BSD$
** You may use this file under the terms of the BSD license as follows:
**
** "Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions are
** met:
**   * Redistributions of source code must retain the above copyright
**     notice, this list of conditions and the following disclaimer.
**   * Redistributions in binary form must reproduce the above copyright
**     notice, this list of conditions and the following disclaimer in
**     the documentation and/or other materials provided with the
**     distribution.
**   * Neither the name of Nokia Corporation and its Subsidiary(-ies) nor
**     the names of its contributors may be used to endorse or promote
**     products derived from this software without specific prior written
**     permission.
**
** THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
** "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
** LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
** A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
** OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
** SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
** LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
** OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE."
** $QT_END_LICENSE$
**
****************************************************************************/

#include "cusdr_audio_settingsdialog.h"
#include "Util/cusdr_buttons.h"


#include <QComboBox>
#include <QDialogButtonBox>
#include <QLabel>
#include <QPushButton>
#include <QVBoxLayout>
#include <QCheckBox>
#include <QSlider>
#include <QSpinBox>

#define	btn_height		18
#define	btn_width		74

SettingsDialog::SettingsDialog(
            const QList<QAudioDeviceInfo> &availableInputDevices,
            const QList<QAudioDeviceInfo> &availableOutputDevices,
            QWidget *parent)
    :   QDialog(parent)
	,	set(Settings::instance())
    ,   m_inputDeviceComboBox(new QComboBox(this))
    ,   m_outputDeviceComboBox(new QComboBox(this))
	//,   m_windowFunction(DefaultWindowFunction)
    //,   m_windowFunctionComboBox(new QComboBox(this))
{
	if (parent)
		setWindowFlags(Qt::Tool | Qt::FramelessWindowHint);
	else
		setWindowFlags(Qt::Tool | Qt::FramelessWindowHint | Qt::WindowStaysOnTopHint);

	setWindowModality(Qt::NonModal);
	setWindowOpacity(0.9);
	setStyleSheet(set->getDialogStyle());

	setMouseTracking(true);

	m_titleFont.setStyleStrategy(QFont::PreferAntialias);
	m_titleFont.setFixedPitch(true);
	#ifdef Q_OS_MAC
		m_titleFont.setPixelSize(10);
		m_titleFont.setFamily("Arial");
		//m_titleFont.setBold(true);
	#endif
	#ifdef Q_OS_WIN
		m_titleFont.setPixelSize(13);
		m_titleFont.setFamily("Arial");
		m_titleFont.setBold(true);
		//m_titleFont.setItalic(true);
	#endif

    QVBoxLayout *dialogLayout = new QVBoxLayout(this);

    // Populate combo boxes

    QAudioDeviceInfo device;
    foreach (device, availableInputDevices)
        m_inputDeviceComboBox->addItem(device.deviceName(),
                                       qVariantFromValue(device));
    foreach (device, availableOutputDevices)
        m_outputDeviceComboBox->addItem(device.deviceName(),
                                       qVariantFromValue(device));

    //m_windowFunctionComboBox->addItem(tr("None"), qVariantFromValue(int(NoWindow)));
    //m_windowFunctionComboBox->addItem("Hann", qVariantFromValue(int(HannWindow)));
    //m_windowFunctionComboBox->setCurrentIndex(m_windowFunction);

	m_inputDeviceComboBox->setStyleSheet(set->getComboBoxStyle());
	m_inputDeviceComboBox->setMinimumContentsLength(30);
	m_outputDeviceComboBox->setStyleSheet(set->getComboBoxStyle());
	m_outputDeviceComboBox->setMinimumContentsLength(30);

    // Initialize default devices
    if (!availableInputDevices.empty())
        m_inputDevice = availableInputDevices.front();
    if (!availableOutputDevices.empty())
        m_outputDevice = availableOutputDevices.front();

    // Add widgets to layout

	QScopedPointer<QHBoxLayout> titleLayout(new QHBoxLayout);
	QLabel *titleLabel = new QLabel(tr("Audio Settings:"), this);
	titleLabel->setFont(m_titleFont);
	titleLabel->setStyleSheet(set->getLabelStyle());
    titleLayout->addWidget(titleLabel);
    dialogLayout->addLayout(titleLayout.data());
    titleLayout.take(); // ownership transferred to dialogLayout

    QScopedPointer<QHBoxLayout> inputDeviceLayout(new QHBoxLayout);
    QLabel *inputDeviceLabel = new QLabel(tr("Input device"), this);
	inputDeviceLabel->setStyleSheet(set->getLabelStyle());
    inputDeviceLayout->addWidget(inputDeviceLabel);
    inputDeviceLayout->addWidget(m_inputDeviceComboBox);
    dialogLayout->addLayout(inputDeviceLayout.data());
    inputDeviceLayout.take(); // ownership transferred to dialogLayout

    QScopedPointer<QHBoxLayout> outputDeviceLayout(new QHBoxLayout);
    QLabel *outputDeviceLabel = new QLabel(tr("Output device"), this);
	outputDeviceLabel->setStyleSheet(set->getLabelStyle());
    outputDeviceLayout->addWidget(outputDeviceLabel);
    outputDeviceLayout->addWidget(m_outputDeviceComboBox);
    dialogLayout->addLayout(outputDeviceLayout.data());
    outputDeviceLayout.take(); // ownership transferred to dialogLayout

    //QScopedPointer<QHBoxLayout> windowFunctionLayout(new QHBoxLayout);
    //QLabel *windowFunctionLabel = new QLabel(tr("Window function"), this);
    //windowFunctionLayout->addWidget(windowFunctionLabel);
    //windowFunctionLayout->addWidget(m_windowFunctionComboBox);
    //dialogLayout->addLayout(windowFunctionLayout.data());
    //windowFunctionLayout.take(); // ownership transferred to dialogLayout

    // Connect
    CHECKED_CONNECT(
		m_inputDeviceComboBox, 
		SIGNAL(activated(int)),
        this, 
		SLOT(inputDeviceChanged(int)));

    CHECKED_CONNECT(
		m_outputDeviceComboBox, 
		SIGNAL(activated(int)),
        this, 
		SLOT(outputDeviceChanged(int)));

    /*CHECKED_CONNECT(
		m_windowFunctionComboBox,
		SIGNAL(activated(int)),
        this, 
		SLOT(windowFunctionChanged(int)));*/

	AeroButton* okBtn = new AeroButton("Ok", this);
	okBtn->setRoundness(10);
	okBtn->setFixedSize(btn_width, btn_height);
	CHECKED_CONNECT(
		okBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(accept()));

	AeroButton* cancelBtn = new AeroButton("Cancel", this);
	cancelBtn->setRoundness(10);
	cancelBtn->setFixedSize(btn_width, btn_height);
	CHECKED_CONNECT(
		cancelBtn, 
		SIGNAL(clicked()), 
		this, 
		SLOT(reject()));

	QHBoxLayout *hbox = new QHBoxLayout;
	hbox->setSpacing(1);
	hbox->addWidget(okBtn);
	hbox->addWidget(cancelBtn);

	dialogLayout->addLayout(hbox);
    
    setLayout(dialogLayout);
}

SettingsDialog::~SettingsDialog() {
}

//void SettingsDialog::windowFunctionChanged(int index)
//{
//    m_windowFunction = static_cast<WindowFunction>(
//            m_windowFunctionComboBox->itemData(index).value<int>());
//}

void SettingsDialog::inputDeviceChanged(int index) {

    m_inputDevice = m_inputDeviceComboBox->itemData(index).value<QAudioDeviceInfo>();
}

void SettingsDialog::outputDeviceChanged(int index) {

    m_outputDevice = m_outputDeviceComboBox->itemData(index).value<QAudioDeviceInfo>();
}

