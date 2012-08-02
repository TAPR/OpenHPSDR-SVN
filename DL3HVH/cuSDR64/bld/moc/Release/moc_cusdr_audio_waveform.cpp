/****************************************************************************
** Meta object code from reading C++ file 'cusdr_audio_waveform.h'
**
** Created: Mon 30. Jul 19:05:46 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_audio_waveform.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_audio_waveform.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Waveform[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       4,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       1,       // signalCount

 // signals: signature, parameters, type, tag, flags
      16,   10,    9,    9, 0x05,

 // slots: signature, parameters, type, tag, flags
      73,   43,    9,    9, 0x0a,
     138,  122,    9,    9, 0x0a,
     189,  184,  176,    9, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_Waveform[] = {
    "Waveform\0\0value\0waveformImageChanged(bool)\0"
    "sender,position,length,buffer\0"
    "bufferChanged(QObject*,qint64,qint64,QByteArray)\0"
    "sender,position\0audioPositionChanged(QObject*,qint64)\0"
    "QImage*\0rect\0createWaveformImage(QRect)\0"
};

void Waveform::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        Waveform *_t = static_cast<Waveform *>(_o);
        switch (_id) {
        case 0: _t->waveformImageChanged((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 1: _t->bufferChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< qint64(*)>(_a[2])),(*reinterpret_cast< qint64(*)>(_a[3])),(*reinterpret_cast< const QByteArray(*)>(_a[4]))); break;
        case 2: _t->audioPositionChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< qint64(*)>(_a[2]))); break;
        case 3: { QImage* _r = _t->createWaveformImage((*reinterpret_cast< const QRect(*)>(_a[1])));
            if (_a[0]) *reinterpret_cast< QImage**>(_a[0]) = _r; }  break;
        default: ;
        }
    }
}

const QMetaObjectExtraData Waveform::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject Waveform::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_Waveform,
      qt_meta_data_Waveform, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &Waveform::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *Waveform::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *Waveform::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Waveform))
        return static_cast<void*>(const_cast< Waveform*>(this));
    return QObject::qt_metacast(_clname);
}

int Waveform::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 4)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 4;
    }
    return _id;
}

// SIGNAL 0
void Waveform::waveformImageChanged(bool _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
