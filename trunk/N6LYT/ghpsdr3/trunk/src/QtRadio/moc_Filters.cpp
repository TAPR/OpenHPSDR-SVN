/****************************************************************************
** Meta object code from reading C++ file 'Filters.h'
**
** Created: Tue Aug 17 20:23:56 2010
**      by: The Qt Meta Object Compiler version 62 (Qt 4.6.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "Filters.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'Filters.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 62
#error "This file was generated using the moc from 4.6.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Filters[] = {

 // content:
       4,       // revision
       0,       // classname
       0,    0, // classinfo
       2,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       2,       // signalCount

 // signals: signature, parameters, type, tag, flags
      36,    9,    8,    8, 0x05,
     103,   78,    8,    8, 0x05,

       0        // eod
};

static const char qt_meta_stringdata_Filters[] = {
    "Filters\0\0previousFilters,newFilters\0"
    "filtersChanged(FiltersBase*,FiltersBase*)\0"
    "previousFitler,newFilter\0"
    "filterChanged(int,int)\0"
};

const QMetaObject Filters::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_Filters,
      qt_meta_data_Filters, 0 }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &Filters::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *Filters::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *Filters::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Filters))
        return static_cast<void*>(const_cast< Filters*>(this));
    return QObject::qt_metacast(_clname);
}

int Filters::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: filtersChanged((*reinterpret_cast< FiltersBase*(*)>(_a[1])),(*reinterpret_cast< FiltersBase*(*)>(_a[2]))); break;
        case 1: filterChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        default: ;
        }
        _id -= 2;
    }
    return _id;
}

// SIGNAL 0
void Filters::filtersChanged(FiltersBase * _t1, FiltersBase * _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void Filters::filterChanged(int _t1, int _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}
QT_END_MOC_NAMESPACE
