/**
* @file cusdr_audio_waveform.h
* @brief cuSDR waveform graphics header file
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

#ifndef _CUSDR_WAVEFORM_H
#define _CUSDR_WAVEFORM_H

#include <QObject>
#include <QPixmap>
#include <QScopedPointer>

#include "cusdr_settings.h"

#ifdef LOG_WAVEFORM
#   define WAVEFORM_DEBUG qDebug().nospace() << "WaveForm::\t"
#else
#   define WAVEFORM_DEBUG nullDebug()
#endif

#ifdef LOG_PAINT_EVENT
#   define WAVEFORM_PAINT_DEBUG qDebug().nospace() << "WaveFormPaint::\t"
#else
#   define WAVEFORM_PAINT_DEBUG nullDebug()
#endif


QT_FORWARD_DECLARE_CLASS(QByteArray)

/**
 * QObject which draws a section of the audio waveform.
 *
 * The waveform is rendered on a set of QPixmaps which form a group of tiles
 * whose extent covers the widget.  As the audio position is updated, these
 * tiles are scrolled from left to right; when the left-most tile scrolls
 * outside the widget, it is moved to the right end of the tile array and
 * painted with the next section of the waveform.
 */

class Waveform : public QObject {

    Q_OBJECT

public:
	Waveform(QObject *parent = 0);
    ~Waveform();

    void initialize(
			const QAudioFormat &format, 
			qint64 audioBufferSize, 
			qint64 windowDurationUs,
			const QRect &rect);

    void reset();

    void setAutoUpdatePosition(bool enabled);

public slots:
    void bufferChanged(QObject *sender, qint64 position, qint64 length, const QByteArray &buffer);
    void audioPositionChanged(QObject *sender, qint64 position);

	QImage*		createWaveformImage(const QRect &rect);

private:
    static const int NullIndex = -1;

    void deletePixmaps();

    /*
     * (Re)create all pixmaps, repaint and update the display.
     * Triggers an update();
     */
    void createPixmaps(const QSize &newSize);

    /*
     * Update window position.
     * Triggers an update().
     */
    void setWindowPosition(qint64 position);

    /*
     * Base position of tile
     */
    qint64 tilePosition(int index) const;

    /*
     * Structure which identifies a point within a given
     * tile.
     */
    struct TilePoint
    {
        TilePoint(int idx = 0, qint64 pos = 0, qint64 pix = 0)
        :   index(idx), positionOffset(pos), pixelOffset(pix)
        { }

        // Index of tile
        int     index;

        // Number of bytes from start of tile
        qint64  positionOffset;

        // Number of pixels from left of corresponding pixmap
        int     pixelOffset;
    };

    /*
     * Convert position in m_buffer into a tile index and an offset in pixels
     * into the corresponding pixmap.
     *
     * \param position  Offset into m_buffer, in bytes

     * If position is outside the tile array, index is NullIndex and
     * offset is zero.
     */
    TilePoint tilePoint(qint64 position) const;

    /*
     * Convert offset in bytes into a tile into an offset in pixels
     * within that tile.
     */
    int tilePixelOffset(qint64 positionOffset) const;

    /*
     * Convert offset in bytes into the window into an offset in pixels
     * within the widget rect().
     */
    int windowPixelOffset(qint64 positionOffset, const QRect &rect) const;

    /*
     * Paint all tiles which can be painted.
     * \return true iff update() was called
     */
    bool paintTiles();

    /*
     * Paint the specified tile
     *
     * \pre Sufficient data is available to completely paint the tile, i.e.
     *      m_dataLength is greater than the upper bound of the tile.
     */
    void paintTile(int index);

    /*
     * Move the first n tiles to the end of the array, and mark them as not
     * painted.
     */
    void shuffleTiles(int n);

    /*
     * Reset tile array
     */
    void resetTiles(qint64 newStartPos);

private:
	Settings*				set;

	QRect					m_audioRect;
    qint64                  m_bufferPosition;
    qint64                  m_bufferLength;
    QByteArray              m_buffer;

    qint64                  m_audioPosition;
    QAudioFormat            m_format;

    bool                    m_active;

    QSize                   m_pixmapSize;
    QVector<QPixmap*>       m_pixmaps;

    struct Tile {
        // Pointer into parent m_pixmaps array
        QPixmap*            pixmap;

        // Flag indicating whether this tile has been painted
        bool                painted;
    };

    QVector<Tile>           m_tiles;

    // Length of audio data in bytes depicted by each tile
    qint64                  m_tileLength;

    // Position in bytes of the first tile, relative to m_buffer
    qint64                  m_tileArrayStart;

    qint64		m_windowPosition;
    qint64		m_windowLength;
	int			m_waveformDisplayWidth;

signals:
	void		waveformImageChanged(bool value);
};

#endif // _CUSDR_WAVEFORM_H
