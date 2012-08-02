/****************************************************************************
** Meta object code from reading C++ file 'cusdr_chirpProcessor.h'
**
** Created: Mon 30. Jul 19:03:59 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_chirpProcessor.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_chirpProcessor.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_ChirpProcessor[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      12,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       1,       // signalCount

 // signals: signature, parameters, type, tag, flags
      24,   16,   15,   15, 0x05,

 // slots: signature, parameters, type, tag, flags
      46,   15,   15,   15, 0x0a,
      53,   15,   15,   15, 0x0a,
      72,   15,   15,   15, 0x0a,
      98,   93,   15,   15, 0x08,
     148,  135,   15,   15, 0x08,
     200,  182,   15,   15, 0x08,
     245,  239,   15,   15, 0x08,
     275,  239,   15,   15, 0x08,
     297,  239,   15,   15, 0x08,
     320,  239,   15,   15, 0x08,
     349,  239,   15,   15, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_ChirpProcessor[] = {
    "ChirpProcessor\0\0message\0messageEvent(QString)\0"
    "stop()\0processChirpData()\0"
    "generateLocalChirp()\0data\0"
    "matchedFilterFIRFilter(QList<qreal>)\0"
    "sender,value\0samplingRateChanged(QObject*,int)\0"
    "distance,chirpfft\0"
    "setSpectras(const float*,const float*)\0"
    "value\0setDistSpectrumAvgLength(int)\0"
    "setChirpFFTShow(bool)\0setChirpSideband(bool)\0"
    "setFilterLowerFrequency(int)\0"
    "setFilterUpperFrequency(int)\0"
};

void ChirpProcessor::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        ChirpProcessor *_t = static_cast<ChirpProcessor *>(_o);
        switch (_id) {
        case 0: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 1: _t->stop(); break;
        case 2: _t->processChirpData(); break;
        case 3: _t->generateLocalChirp(); break;
        case 4: _t->matchedFilterFIRFilter((*reinterpret_cast< QList<qreal>(*)>(_a[1]))); break;
        case 5: _t->samplingRateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 6: _t->setSpectras((*reinterpret_cast< const float*(*)>(_a[1])),(*reinterpret_cast< const float*(*)>(_a[2]))); break;
        case 7: _t->setDistSpectrumAvgLength((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 8: _t->setChirpFFTShow((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 9: _t->setChirpSideband((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 10: _t->setFilterLowerFrequency((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 11: _t->setFilterUpperFrequency((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData ChirpProcessor::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject ChirpProcessor::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_ChirpProcessor,
      qt_meta_data_ChirpProcessor, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &ChirpProcessor::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *ChirpProcessor::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *ChirpProcessor::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_ChirpProcessor))
        return static_cast<void*>(const_cast< ChirpProcessor*>(this));
    return QObject::qt_metacast(_clname);
}

int ChirpProcessor::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 12)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 12;
    }
    return _id;
}

// SIGNAL 0
void ChirpProcessor::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
