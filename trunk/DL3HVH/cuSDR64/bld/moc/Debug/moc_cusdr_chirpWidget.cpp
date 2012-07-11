/****************************************************************************
** Meta object code from reading C++ file 'cusdr_chirpWidget.h'
**
** Created: Mon 9. Jul 22:40:32 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_chirpWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_chirpWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_ChirpWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      30,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       8,       // signalCount

 // signals: signature, parameters, type, tag, flags
      20,   13,   12,   12, 0x05,
      40,   13,   12,   12, 0x05,
      61,   12,   12,   12, 0x05,
      96,   83,   12,   12, 0x05,
     149,  133,   12,   12, 0x05,
     181,   13,   12,   12, 0x05,
     205,   13,   12,   12, 0x05,
     235,   13,   12,   12, 0x05,

 // slots: signature, parameters, type, tag, flags
     276,   12,  270,   12, 0x0a,
     287,   12,  270,   12, 0x0a,
     305,   12,   12,   12, 0x08,
     324,   12,   12,   12, 0x08,
     341,   12,   12,   12, 0x08,
     355,   12,   12,   12, 0x08,
     365,   12,   12,   12, 0x08,
     381,   12,   12,   12, 0x08,
     402,   12,   12,   12, 0x08,
     417,   12,   12,   12, 0x08,
     444,  434,   12,   12, 0x08,
     481,  434,   12,   12, 0x08,
     529,  518,   12,   12, 0x08,
     563,   83,   12,   12, 0x08,
     605,   83,   12,   12, 0x08,
     643,  434,   12,   12, 0x08,
     681,  434,   12,   12, 0x08,
     744,  719,   12,   12, 0x08,
     811,  798,   12,   12, 0x08,
     862,   12,   12,   12, 0x08,
     878,   12,   12,   12, 0x08,
     900,  894,   12,   12, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_ChirpWidget[] = {
    "ChirpWidget\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0messageEvent(QString)\0"
    "sender,value\0initChirpDecoderEvent(QObject*,bool)\0"
    "sender,fileName\0loadFileEvent(QObject*,QString)\0"
    "suspendSignal(QObject*)\0"
    "startPlaybackSignal(QObject*)\0"
    "showSettingsDialogSignal(QObject*)\0"
    "QSize\0sizeHint()\0minimumSizeHint()\0"
    "initChirpDecoder()\0showFileDialog()\0"
    "setReceiver()\0suspend()\0startPlayback()\0"
    "showSettingsDialog()\0showChirpFFT()\0"
    "switchSideband()\0sender,lo\0"
    "setChirpLowerFrequency(QObject*,int)\0"
    "setChirpUpperFrequency(QObject*,int)\0"
    "sender,amp\0setChirpAmplitude(QObject*,qreal)\0"
    "setChirpBufferDurationUs(QObject*,qint64)\0"
    "setChirpRepetitionTimes(QObject*,int)\0"
    "setFilterLowerFrequency(QObject*,int)\0"
    "setFilterUpperFrequency(QObject*,int)\0"
    "position,length,spectrum\0"
    "chirpSpectrumChanged(qint64,qint64,FrequencySpectrum)\0"
    "spectrumList\0"
    "chirpSpectrumListChanged(QList<FrequencySpectrum>)\0"
    "resetSpectrum()\0updateStripes()\0scale\0"
    "stripeResize(int)\0"
};

void ChirpWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        ChirpWidget *_t = static_cast<ChirpWidget *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: _t->initChirpDecoderEvent((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2]))); break;
        case 4: _t->loadFileEvent((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< const QString(*)>(_a[2]))); break;
        case 5: _t->suspendSignal((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 6: _t->startPlaybackSignal((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 7: _t->showSettingsDialogSignal((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 8: { QSize _r = _t->sizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 9: { QSize _r = _t->minimumSizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 10: _t->initChirpDecoder(); break;
        case 11: _t->showFileDialog(); break;
        case 12: _t->setReceiver(); break;
        case 13: _t->suspend(); break;
        case 14: _t->startPlayback(); break;
        case 15: _t->showSettingsDialog(); break;
        case 16: _t->showChirpFFT(); break;
        case 17: _t->switchSideband(); break;
        case 18: _t->setChirpLowerFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 19: _t->setChirpUpperFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 20: _t->setChirpAmplitude((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< qreal(*)>(_a[2]))); break;
        case 21: _t->setChirpBufferDurationUs((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< qint64(*)>(_a[2]))); break;
        case 22: _t->setChirpRepetitionTimes((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 23: _t->setFilterLowerFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 24: _t->setFilterUpperFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 25: _t->chirpSpectrumChanged((*reinterpret_cast< qint64(*)>(_a[1])),(*reinterpret_cast< qint64(*)>(_a[2])),(*reinterpret_cast< const FrequencySpectrum(*)>(_a[3]))); break;
        case 26: _t->chirpSpectrumListChanged((*reinterpret_cast< const QList<FrequencySpectrum>(*)>(_a[1]))); break;
        case 27: _t->resetSpectrum(); break;
        case 28: _t->updateStripes(); break;
        case 29: _t->stripeResize((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData ChirpWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject ChirpWidget::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_ChirpWidget,
      qt_meta_data_ChirpWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &ChirpWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *ChirpWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *ChirpWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_ChirpWidget))
        return static_cast<void*>(const_cast< ChirpWidget*>(this));
    return QWidget::qt_metacast(_clname);
}

int ChirpWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 30)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 30;
    }
    return _id;
}

// SIGNAL 0
void ChirpWidget::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void ChirpWidget::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void ChirpWidget::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void ChirpWidget::initChirpDecoderEvent(QObject * _t1, bool _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}

// SIGNAL 4
void ChirpWidget::loadFileEvent(QObject * _t1, const QString & _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 4, _a);
}

// SIGNAL 5
void ChirpWidget::suspendSignal(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 5, _a);
}

// SIGNAL 6
void ChirpWidget::startPlaybackSignal(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 6, _a);
}

// SIGNAL 7
void ChirpWidget::showSettingsDialogSignal(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 7, _a);
}
QT_END_MOC_NAMESPACE
