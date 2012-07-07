/****************************************************************************
** Meta object code from reading C++ file 'cusdr_audio_engine.h'
**
** Created: Sat 7. Jul 09:06:31 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_audio_engine.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_audio_engine.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_AudioEngine[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      33,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
      14,       // signalCount

 // signals: signature, parameters, type, tag, flags
      24,   13,   12,   12, 0x05,
      69,   65,   12,   12, 0x05,
     110,   91,   12,   12, 0x05,
     150,  135,   12,   12, 0x05,
     194,  180,   12,   12, 0x05,
     240,  231,   12,   12, 0x05,
     268,  231,   12,   12, 0x05,
     303,  294,   12,   12, 0x05,
     349,  333,   12,   12, 0x05,
     416,  386,   12,   12, 0x05,
     471,  446,   12,   12, 0x05,
     550,  520,   12,   12, 0x05,
     599,   12,   12,   12, 0x05,
     627,  620,   12,   12, 0x05,

 // slots: signature, parameters, type, tag, flags
     664,   12,   12,   12, 0x0a,
     681,   12,   12,   12, 0x0a,
     697,   12,   12,   12, 0x0a,
     718,   12,   12,   12, 0x0a,
     735,  728,   12,   12, 0x0a,
     773,  728,   12,   12, 0x0a,
     812,   12,   12,   12, 0x0a,
     860,  841,   12,   12, 0x0a,
     931,  921,   12,   12, 0x0a,
     968,  921,   12,   12, 0x0a,
    1016, 1005,   12,   12, 0x0a,
    1063, 1050,   12,   12, 0x0a,
    1105, 1050,   12,   12, 0x0a,
    1143, 1050,   12,   12, 0x0a,
    1175,   12,   12,   12, 0x08,
    1195, 1189,   12,   12, 0x08,
    1228,   12,   12,   12, 0x08,
    1254, 1245,   12,   12, 0x08,
    1302, 1289,   12,   12, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_AudioEngine[] = {
    "AudioEngine\0\0mode,state\0"
    "stateChanged(QAudio::Mode,QAudio::State)\0"
    "msg\0messageEvent(QString)\0message,durationMs\0"
    "infoMessage(QString,int)\0heading,detail\0"
    "errorMessage(QString,QString)\0"
    "sender,format\0formatChanged(QObject*,QAudioFormat)\0"
    "duration\0bufferLengthChanged(qint64)\0"
    "dataLengthChanged(qint64)\0position\0"
    "recordPositionChanged(qint64)\0"
    "sender,position\0playPositionChanged(QObject*,qint64)\0"
    "rmsLevel,peakLevel,numSamples\0"
    "levelChanged(qreal,qreal,int)\0"
    "position,length,spectrum\0"
    "spectrumChanged(qint64,qint64,FrequencySpectrum)\0"
    "sender,position,length,buffer\0"
    "bufferChanged(QObject*,qint64,qint64,QByteArray)\0"
    "chirpSignalChanged()\0buffer\0"
    "audiofileBufferChanged(QList<qreal>)\0"
    "startRecording()\0startPlayback()\0"
    "showSettingsDialog()\0suspend()\0device\0"
    "setAudioInputDevice(QAudioDeviceInfo)\0"
    "setAudioOutputDevice(QAudioDeviceInfo)\0"
    "setChirpSignalMode(QObject*)\0"
    "tone,format,buffer\0"
    "generateAudioChirpSignal(SweptTone,QAudioFormat,QByteArray&)\0"
    "sender,lo\0setChirpLowerFrequency(QObject*,int)\0"
    "setChirpUpperFrequency(QObject*,int)\0"
    "sender,amp\0setChirpAmplitude(QObject*,qreal)\0"
    "sender,value\0setChirpBufferDurationUs(QObject*,qint64)\0"
    "setChirpRepetitionTimes(QObject*,int)\0"
    "sampleRateChanged(QObject*,int)\0"
    "audioNotify()\0state\0"
    "audioStateChanged(QAudio::State)\0"
    "audioDataReady()\0spectrum\0"
    "spectrumChanged(FrequencySpectrum)\0"
    "spectrumList\0spectrumListChanged(QList<FrequencySpectrum>)\0"
};

void AudioEngine::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        AudioEngine *_t = static_cast<AudioEngine *>(_o);
        switch (_id) {
        case 0: _t->stateChanged((*reinterpret_cast< QAudio::Mode(*)>(_a[1])),(*reinterpret_cast< QAudio::State(*)>(_a[2]))); break;
        case 1: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 2: _t->infoMessage((*reinterpret_cast< const QString(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 3: _t->errorMessage((*reinterpret_cast< const QString(*)>(_a[1])),(*reinterpret_cast< const QString(*)>(_a[2]))); break;
        case 4: _t->formatChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< const QAudioFormat(*)>(_a[2]))); break;
        case 5: _t->bufferLengthChanged((*reinterpret_cast< qint64(*)>(_a[1]))); break;
        case 6: _t->dataLengthChanged((*reinterpret_cast< qint64(*)>(_a[1]))); break;
        case 7: _t->recordPositionChanged((*reinterpret_cast< qint64(*)>(_a[1]))); break;
        case 8: _t->playPositionChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< qint64(*)>(_a[2]))); break;
        case 9: _t->levelChanged((*reinterpret_cast< qreal(*)>(_a[1])),(*reinterpret_cast< qreal(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 10: _t->spectrumChanged((*reinterpret_cast< qint64(*)>(_a[1])),(*reinterpret_cast< qint64(*)>(_a[2])),(*reinterpret_cast< const FrequencySpectrum(*)>(_a[3]))); break;
        case 11: _t->bufferChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< qint64(*)>(_a[2])),(*reinterpret_cast< qint64(*)>(_a[3])),(*reinterpret_cast< const QByteArray(*)>(_a[4]))); break;
        case 12: _t->chirpSignalChanged(); break;
        case 13: _t->audiofileBufferChanged((*reinterpret_cast< const QList<qreal>(*)>(_a[1]))); break;
        case 14: _t->startRecording(); break;
        case 15: _t->startPlayback(); break;
        case 16: _t->showSettingsDialog(); break;
        case 17: _t->suspend(); break;
        case 18: _t->setAudioInputDevice((*reinterpret_cast< const QAudioDeviceInfo(*)>(_a[1]))); break;
        case 19: _t->setAudioOutputDevice((*reinterpret_cast< const QAudioDeviceInfo(*)>(_a[1]))); break;
        case 20: _t->setChirpSignalMode((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 21: _t->generateAudioChirpSignal((*reinterpret_cast< const SweptTone(*)>(_a[1])),(*reinterpret_cast< const QAudioFormat(*)>(_a[2])),(*reinterpret_cast< QByteArray(*)>(_a[3]))); break;
        case 22: _t->setChirpLowerFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 23: _t->setChirpUpperFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 24: _t->setChirpAmplitude((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< qreal(*)>(_a[2]))); break;
        case 25: _t->setChirpBufferDurationUs((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< qint64(*)>(_a[2]))); break;
        case 26: _t->setChirpRepetitionTimes((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 27: _t->sampleRateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 28: _t->audioNotify(); break;
        case 29: _t->audioStateChanged((*reinterpret_cast< QAudio::State(*)>(_a[1]))); break;
        case 30: _t->audioDataReady(); break;
        case 31: _t->spectrumChanged((*reinterpret_cast< const FrequencySpectrum(*)>(_a[1]))); break;
        case 32: _t->spectrumListChanged((*reinterpret_cast< const QList<FrequencySpectrum>(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData AudioEngine::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject AudioEngine::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_AudioEngine,
      qt_meta_data_AudioEngine, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &AudioEngine::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *AudioEngine::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *AudioEngine::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_AudioEngine))
        return static_cast<void*>(const_cast< AudioEngine*>(this));
    return QWidget::qt_metacast(_clname);
}

int AudioEngine::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 33)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 33;
    }
    return _id;
}

// SIGNAL 0
void AudioEngine::stateChanged(QAudio::Mode _t1, QAudio::State _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void AudioEngine::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void AudioEngine::infoMessage(const QString & _t1, int _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void AudioEngine::errorMessage(const QString & _t1, const QString & _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}

// SIGNAL 4
void AudioEngine::formatChanged(QObject * _t1, const QAudioFormat & _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 4, _a);
}

// SIGNAL 5
void AudioEngine::bufferLengthChanged(qint64 _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 5, _a);
}

// SIGNAL 6
void AudioEngine::dataLengthChanged(qint64 _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 6, _a);
}

// SIGNAL 7
void AudioEngine::recordPositionChanged(qint64 _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 7, _a);
}

// SIGNAL 8
void AudioEngine::playPositionChanged(QObject * _t1, qint64 _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 8, _a);
}

// SIGNAL 9
void AudioEngine::levelChanged(qreal _t1, qreal _t2, int _t3)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)), const_cast<void*>(reinterpret_cast<const void*>(&_t3)) };
    QMetaObject::activate(this, &staticMetaObject, 9, _a);
}

// SIGNAL 10
void AudioEngine::spectrumChanged(qint64 _t1, qint64 _t2, const FrequencySpectrum & _t3)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)), const_cast<void*>(reinterpret_cast<const void*>(&_t3)) };
    QMetaObject::activate(this, &staticMetaObject, 10, _a);
}

// SIGNAL 11
void AudioEngine::bufferChanged(QObject * _t1, qint64 _t2, qint64 _t3, const QByteArray & _t4)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)), const_cast<void*>(reinterpret_cast<const void*>(&_t3)), const_cast<void*>(reinterpret_cast<const void*>(&_t4)) };
    QMetaObject::activate(this, &staticMetaObject, 11, _a);
}

// SIGNAL 12
void AudioEngine::chirpSignalChanged()
{
    QMetaObject::activate(this, &staticMetaObject, 12, 0);
}

// SIGNAL 13
void AudioEngine::audiofileBufferChanged(const QList<qreal> & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 13, _a);
}
QT_END_MOC_NAMESPACE
