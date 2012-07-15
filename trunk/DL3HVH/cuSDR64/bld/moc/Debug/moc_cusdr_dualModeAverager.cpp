/****************************************************************************
** Meta object code from reading C++ file 'cusdr_dualModeAverager.h'
**
** Created: Sat 14. Jul 23:28:38 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_dualModeAverager.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_dualModeAverager.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_DualModeAverager[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       1,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
      24,   18,   17,   17, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_DualModeAverager[] = {
    "DualModeAverager\0\0value\0setAveragingLength(int)\0"
};

void DualModeAverager::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        DualModeAverager *_t = static_cast<DualModeAverager *>(_o);
        switch (_id) {
        case 0: _t->setAveragingLength((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData DualModeAverager::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject DualModeAverager::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_DualModeAverager,
      qt_meta_data_DualModeAverager, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &DualModeAverager::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *DualModeAverager::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *DualModeAverager::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_DualModeAverager))
        return static_cast<void*>(const_cast< DualModeAverager*>(this));
    return QObject::qt_metacast(_clname);
}

int DualModeAverager::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 1)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 1;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
