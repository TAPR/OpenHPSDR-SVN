/****************************************************************************
** Meta object code from reading C++ file 'cusdr_receiver.h'
**
** Created: Tue 3. Jul 21:43:42 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_receiver.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_receiver.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_HPSDRReceiver[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      21,   14, // methods
       1,  119, // properties
       1,  122, // enums/sets
       0,    0, // constructors
       0,       // flags
       1,       // signalCount

 // signals: signature, parameters, type, tag, flags
      19,   15,   14,   14, 0x05,

 // slots: signature, parameters, type, tag, flags
      46,   41,   14,   14, 0x0a,
      81,   75,   14,   14, 0x0a,
     106,   75,   14,   14, 0x0a,
     123,   75,   14,   14, 0x0a,
     138,   75,   14,   14, 0x0a,
     153,   75,   14,   14, 0x0a,
     168,   75,   14,   14, 0x0a,
     193,   75,   14,   14, 0x0a,
     204,   75,   14,   14, 0x0a,
     238,  223,   14,   14, 0x0a,
     286,  271,   14,   14, 0x0a,
     319,  271,   14,   14, 0x0a,
     368,  352,   14,   14, 0x0a,
     397,  352,   14,   14, 0x0a,
     442,  432,   14,   14, 0x0a,
     480,  461,   14,   14, 0x0a,
     527,   14,   14,   14, 0x0a,
     561,   75,   14,   14, 0x0a,
     586,   75,   14,   14, 0x0a,
     640,  611,   14,   14, 0x08,

 // properties: name, type, flags
     754,  742, 0x0009510b,

 // enums: name, flags, count, data
     742, 0x0,    2,  126,

 // enum data: key, value
     766, uint(HPSDRReceiver::RECEIVER_DETACHED),
     784, uint(HPSDRReceiver::RECEIVER_ATTACHED),

       0        // eod
};

static const char qt_meta_stringdata_HPSDRReceiver[] = {
    "HPSDRReceiver\0\0msg\0messageEvent(QString)\0"
    "addr\0setPeerAddress(QHostAddress)\0"
    "value\0setSocketDescriptor(int)\0"
    "setReceiver(int)\0setClient(int)\0"
    "setIQPort(int)\0setBSPort(int)\0"
    "setConnectedStatus(bool)\0setID(int)\0"
    "setSampleRate(int)\0sender,rx,band\0"
    "setHamBand(QObject*,int,HamBand)\0"
    "sender,rx,mode\0setDspMode(QObject*,int,DSPMode)\0"
    "setAGCMode(QObject*,int,AGCMode)\0"
    "sender,rx,value\0setAGCGain(QObject*,int,int)\0"
    "setAudioVolume(QObject*,int,float)\0"
    "frequency\0setFrequency(long)\0"
    "sender,rx,low,high\0"
    "setFilterFrequencies(QObject*,int,qreal,qreal)\0"
    "setLastFrequencyList(QList<long>)\0"
    "setdBmPanScaleMin(qreal)\0"
    "setdBmPanScaleMax(qreal)\0"
    "sender,err,hwmode,mode,state\0"
    "setSystemState(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::_Ser"
    "verMode,QSDR::_DataEngineState)\0"
    "SocketState\0socketState\0RECEIVER_DETACHED\0"
    "RECEIVER_ATTACHED\0"
};

void HPSDRReceiver::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        HPSDRReceiver *_t = static_cast<HPSDRReceiver *>(_o);
        switch (_id) {
        case 0: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 1: _t->setPeerAddress((*reinterpret_cast< QHostAddress(*)>(_a[1]))); break;
        case 2: _t->setSocketDescriptor((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 3: _t->setReceiver((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 4: _t->setClient((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 5: _t->setIQPort((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 6: _t->setBSPort((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 7: _t->setConnectedStatus((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 8: _t->setID((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 9: _t->setSampleRate((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 10: _t->setHamBand((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< HamBand(*)>(_a[3]))); break;
        case 11: _t->setDspMode((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< DSPMode(*)>(_a[3]))); break;
        case 12: _t->setAGCMode((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< AGCMode(*)>(_a[3]))); break;
        case 13: _t->setAGCGain((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 14: _t->setAudioVolume((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< float(*)>(_a[3]))); break;
        case 15: _t->setFrequency((*reinterpret_cast< long(*)>(_a[1]))); break;
        case 16: _t->setFilterFrequencies((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< qreal(*)>(_a[3])),(*reinterpret_cast< qreal(*)>(_a[4]))); break;
        case 17: _t->setLastFrequencyList((*reinterpret_cast< const QList<long>(*)>(_a[1]))); break;
        case 18: _t->setdBmPanScaleMin((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 19: _t->setdBmPanScaleMax((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 20: _t->setSystemState((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData HPSDRReceiver::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject HPSDRReceiver::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_HPSDRReceiver,
      qt_meta_data_HPSDRReceiver, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &HPSDRReceiver::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *HPSDRReceiver::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *HPSDRReceiver::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_HPSDRReceiver))
        return static_cast<void*>(const_cast< HPSDRReceiver*>(this));
    return QObject::qt_metacast(_clname);
}

int HPSDRReceiver::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 21)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 21;
    }
#ifndef QT_NO_PROPERTIES
      else if (_c == QMetaObject::ReadProperty) {
        void *_v = _a[0];
        switch (_id) {
        case 0: *reinterpret_cast< SocketState*>(_v) = socketState(); break;
        }
        _id -= 1;
    } else if (_c == QMetaObject::WriteProperty) {
        void *_v = _a[0];
        switch (_id) {
        case 0: setSocketState(*reinterpret_cast< SocketState*>(_v)); break;
        }
        _id -= 1;
    } else if (_c == QMetaObject::ResetProperty) {
        _id -= 1;
    } else if (_c == QMetaObject::QueryPropertyDesignable) {
        _id -= 1;
    } else if (_c == QMetaObject::QueryPropertyScriptable) {
        _id -= 1;
    } else if (_c == QMetaObject::QueryPropertyStored) {
        _id -= 1;
    } else if (_c == QMetaObject::QueryPropertyEditable) {
        _id -= 1;
    } else if (_c == QMetaObject::QueryPropertyUser) {
        _id -= 1;
    }
#endif // QT_NO_PROPERTIES
    return _id;
}

// SIGNAL 0
void HPSDRReceiver::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
