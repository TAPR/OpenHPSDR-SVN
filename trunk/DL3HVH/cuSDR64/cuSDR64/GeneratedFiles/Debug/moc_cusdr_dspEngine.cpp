/****************************************************************************
** Meta object code from reading C++ file 'cusdr_dspEngine.h'
**
** Created: Wed 4. Jul 21:06:01 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_dspEngine.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_dspEngine.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_QDSPEngine[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       4,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
      17,   11,   12,   11, 0x0a,
      40,   34,   11,   11, 0x0a,
      61,   34,   11,   11, 0x0a,
      83,   78,   11,   11, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_QDSPEngine[] = {
    "QDSPEngine\0\0bool\0getQtDSPStatus()\0"
    "value\0setQtDSPStatus(bool)\0setVolume(float)\0"
    "mode\0setAGCMode(AGCMode)\0"
};

void QDSPEngine::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        QDSPEngine *_t = static_cast<QDSPEngine *>(_o);
        switch (_id) {
        case 0: { bool _r = _t->getQtDSPStatus();
            if (_a[0]) *reinterpret_cast< bool*>(_a[0]) = _r; }  break;
        case 1: _t->setQtDSPStatus((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 2: _t->setVolume((*reinterpret_cast< float(*)>(_a[1]))); break;
        case 3: _t->setAGCMode((*reinterpret_cast< AGCMode(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData QDSPEngine::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject QDSPEngine::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_QDSPEngine,
      qt_meta_data_QDSPEngine, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &QDSPEngine::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *QDSPEngine::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *QDSPEngine::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_QDSPEngine))
        return static_cast<void*>(const_cast< QDSPEngine*>(this));
    return QObject::qt_metacast(_clname);
}

int QDSPEngine::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
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
QT_END_MOC_NAMESPACE
