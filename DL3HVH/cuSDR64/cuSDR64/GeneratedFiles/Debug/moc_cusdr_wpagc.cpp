/****************************************************************************
** Meta object code from reading C++ file 'cusdr_wpagc.h'
**
** Created: Tue 3. Jul 21:47:18 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_wpagc.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_wpagc.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_QWPAGC[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      16,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
      21,    8,    7,    7, 0x0a,
      54,   49,    7,    7, 0x0a,
      77,   71,    7,    7, 0x0a,
     100,   71,    7,    7, 0x0a,
     123,   71,    7,    7, 0x0a,
     148,   71,    7,    7, 0x0a,
     168,   71,    7,    7, 0x0a,
     192,   71,  187,    7, 0x0a,
     214,   71,  187,    7, 0x0a,
     238,   71,    7,    7, 0x0a,
     256,   71,    7,    7, 0x0a,
     276,   71,    7,    7, 0x0a,
     297,   71,    7,    7, 0x0a,
     316,   71,    7,    7, 0x0a,
     374,  338,    7,    7, 0x0a,
     447,  418,  412,    7, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_QWPAGC[] = {
    "QWPAGC\0\0sender,value\0setSampleRate(QObject*,int)\0"
    "mode\0setMode(AGCMode)\0value\0"
    "setAGCHangEnable(bool)\0setAGCFixedGain(qreal)\0"
    "setAGCFixedGainDb(qreal)\0setTauAttack(qreal)\0"
    "setTauDecay(qreal)\0bool\0setMaximumGain(qreal)\0"
    "setMaximumGainDb(qreal)\0setVarGain(qreal)\0"
    "setVarGainDb(qreal)\0setHangThresh(qreal)\0"
    "setHangTime(qreal)\0setHangLevelDb(qreal)\0"
    "filt_high,filt_low,spec_size,thresh\0"
    "setAGCThreshDb(qreal,qreal,int,qreal)\0"
    "qreal\0filt_high,filt_low,spec_size\0"
    "getAGCThreshDb(qreal,qreal,int)\0"
};

void QWPAGC::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        QWPAGC *_t = static_cast<QWPAGC *>(_o);
        switch (_id) {
        case 0: _t->setSampleRate((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 1: _t->setMode((*reinterpret_cast< AGCMode(*)>(_a[1]))); break;
        case 2: _t->setAGCHangEnable((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 3: _t->setAGCFixedGain((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 4: _t->setAGCFixedGainDb((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 5: _t->setTauAttack((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 6: _t->setTauDecay((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 7: { bool _r = _t->setMaximumGain((*reinterpret_cast< qreal(*)>(_a[1])));
            if (_a[0]) *reinterpret_cast< bool*>(_a[0]) = _r; }  break;
        case 8: { bool _r = _t->setMaximumGainDb((*reinterpret_cast< qreal(*)>(_a[1])));
            if (_a[0]) *reinterpret_cast< bool*>(_a[0]) = _r; }  break;
        case 9: _t->setVarGain((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 10: _t->setVarGainDb((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 11: _t->setHangThresh((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 12: _t->setHangTime((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 13: _t->setHangLevelDb((*reinterpret_cast< qreal(*)>(_a[1]))); break;
        case 14: _t->setAGCThreshDb((*reinterpret_cast< qreal(*)>(_a[1])),(*reinterpret_cast< qreal(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])),(*reinterpret_cast< qreal(*)>(_a[4]))); break;
        case 15: { qreal _r = _t->getAGCThreshDb((*reinterpret_cast< qreal(*)>(_a[1])),(*reinterpret_cast< qreal(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])));
            if (_a[0]) *reinterpret_cast< qreal*>(_a[0]) = _r; }  break;
        default: ;
        }
    }
}

const QMetaObjectExtraData QWPAGC::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject QWPAGC::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_QWPAGC,
      qt_meta_data_QWPAGC, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &QWPAGC::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *QWPAGC::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *QWPAGC::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_QWPAGC))
        return static_cast<void*>(const_cast< QWPAGC*>(this));
    return QObject::qt_metacast(_clname);
}

int QWPAGC::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 16)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 16;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
