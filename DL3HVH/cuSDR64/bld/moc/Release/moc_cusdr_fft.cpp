/****************************************************************************
** Meta object code from reading C++ file 'cusdr_fft.h'
**
** Created: Sun 22. Jul 16:54:10 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_fft.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_fft.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_QFFT[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       3,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
      18,    6,    5,    5, 0x0a,
      47,    6,    5,    5, 0x0a,
     108,   76,    5,    5, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_QFFT[] = {
    "QFFT\0\0in,out,size\0DoFFTWForward(CPX*,CPX*,int)\0"
    "DoFFTWInverse(CPX*,CPX*,int)\0"
    "in,size,baseline,correction,fbr\0"
    "DoFFTWMagnForward(CPX*,int,float,float,float*)\0"
};

void QFFT::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        QFFT *_t = static_cast<QFFT *>(_o);
        switch (_id) {
        case 0: _t->DoFFTWForward((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< CPX*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 1: _t->DoFFTWInverse((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< CPX*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 2: _t->DoFFTWMagnForward((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< float(*)>(_a[3])),(*reinterpret_cast< float(*)>(_a[4])),(*reinterpret_cast< float*(*)>(_a[5]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData QFFT::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject QFFT::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_QFFT,
      qt_meta_data_QFFT, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &QFFT::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *QFFT::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *QFFT::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_QFFT))
        return static_cast<void*>(const_cast< QFFT*>(this));
    return QObject::qt_metacast(_clname);
}

int QFFT::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 3)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 3;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
