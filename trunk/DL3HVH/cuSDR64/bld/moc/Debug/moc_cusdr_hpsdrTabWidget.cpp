/****************************************************************************
** Meta object code from reading C++ file 'cusdr_hpsdrTabWidget.h'
**
** Created: Mon 30. Jul 19:03:56 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_hpsdrTabWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_hpsdrTabWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_HPSDRTabWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       8,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       3,       // signalCount

 // signals: signature, parameters, type, tag, flags
      23,   16,   15,   15, 0x05,
      43,   16,   15,   15, 0x05,
      72,   64,   15,   15, 0x05,

 // slots: signature, parameters, type, tag, flags
     100,   15,   94,   15, 0x0a,
     111,   15,   94,   15, 0x0a,
     129,   15,   15,   15, 0x0a,
     161,  155,   15,   15, 0x08,
     183,  155,   15,   15, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_HPSDRTabWidget[] = {
    "HPSDRTabWidget\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0message\0"
    "messageEvent(QString)\0QSize\0sizeHint()\0"
    "minimumSizeHint()\0addNICChangedConnection()\0"
    "value\0setAlexPresence(bool)\0"
    "setPennyPresence(bool)\0"
};

void HPSDRTabWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        HPSDRTabWidget *_t = static_cast<HPSDRTabWidget *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: { QSize _r = _t->sizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 4: { QSize _r = _t->minimumSizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 5: _t->addNICChangedConnection(); break;
        case 6: _t->setAlexPresence((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 7: _t->setPennyPresence((*reinterpret_cast< bool(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData HPSDRTabWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject HPSDRTabWidget::staticMetaObject = {
    { &QTabWidget::staticMetaObject, qt_meta_stringdata_HPSDRTabWidget,
      qt_meta_data_HPSDRTabWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &HPSDRTabWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *HPSDRTabWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *HPSDRTabWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_HPSDRTabWidget))
        return static_cast<void*>(const_cast< HPSDRTabWidget*>(this));
    return QTabWidget::qt_metacast(_clname);
}

int HPSDRTabWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QTabWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 8)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 8;
    }
    return _id;
}

// SIGNAL 0
void HPSDRTabWidget::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void HPSDRTabWidget::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void HPSDRTabWidget::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}
QT_END_MOC_NAMESPACE