/****************************************************************************
** Meta object code from reading C++ file 'Configure.h'
**
** Created: Tue Aug 17 20:23:53 2010
**      by: The Qt Meta Object Compiler version 62 (Qt 4.6.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "Configure.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'Configure.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 62
#error "This file was generated using the moc from 4.6.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_Configure[] = {

 // content:
       4,       // revision
       0,       // classname
       0,    0, // classinfo
       0,    0, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

       0        // eod
};

static const char qt_meta_stringdata_Configure[] = {
    "Configure\0"
};

const QMetaObject Configure::staticMetaObject = {
    { &QDialog::staticMetaObject, qt_meta_stringdata_Configure,
      qt_meta_data_Configure, 0 }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &Configure::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *Configure::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *Configure::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_Configure))
        return static_cast<void*>(const_cast< Configure*>(this));
    return QDialog::qt_metacast(_clname);
}

int Configure::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QDialog::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    return _id;
}
QT_END_MOC_NAMESPACE
