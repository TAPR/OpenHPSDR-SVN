/****************************************************************************
** Meta object code from reading C++ file 'cusdr_colorTriangle.h'
**
** Created: Wed 11. Jul 22:41:33 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_colorTriangle.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_colorTriangle.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_QtColorTriangle[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       3,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       1,       // signalCount

 // signals: signature, parameters, type, tag, flags
      21,   17,   16,   16, 0x05,

 // slots: signature, parameters, type, tag, flags
      42,   17,   16,   16, 0x0a,
      66,   16,   59,   16, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_QtColorTriangle[] = {
    "QtColorTriangle\0\0col\0colorChanged(QColor)\0"
    "setColor(QColor)\0QColor\0getColor()\0"
};

void QtColorTriangle::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        QtColorTriangle *_t = static_cast<QtColorTriangle *>(_o);
        switch (_id) {
        case 0: _t->colorChanged((*reinterpret_cast< const QColor(*)>(_a[1]))); break;
        case 1: _t->setColor((*reinterpret_cast< const QColor(*)>(_a[1]))); break;
        case 2: { QColor _r = _t->getColor();
            if (_a[0]) *reinterpret_cast< QColor*>(_a[0]) = _r; }  break;
        default: ;
        }
    }
}

const QMetaObjectExtraData QtColorTriangle::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject QtColorTriangle::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_QtColorTriangle,
      qt_meta_data_QtColorTriangle, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &QtColorTriangle::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *QtColorTriangle::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *QtColorTriangle::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_QtColorTriangle))
        return static_cast<void*>(const_cast< QtColorTriangle*>(this));
    return QWidget::qt_metacast(_clname);
}

int QtColorTriangle::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 3)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 3;
    }
    return _id;
}

// SIGNAL 0
void QtColorTriangle::colorChanged(const QColor & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
