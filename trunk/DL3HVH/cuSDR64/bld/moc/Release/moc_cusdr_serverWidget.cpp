/****************************************************************************
** Meta object code from reading C++ file 'cusdr_serverWidget.h'
**
** Created: Wed 11. Jul 22:41:28 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_serverWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_serverWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_ServerWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      11,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       3,       // signalCount

 // signals: signature, parameters, type, tag, flags
      21,   14,   13,   13, 0x05,
      41,   14,   13,   13, 0x05,
      62,   13,   13,   13, 0x05,

 // slots: signature, parameters, type, tag, flags
      90,   13,   84,   13, 0x0a,
     101,   13,   84,   13, 0x0a,
     136,  119,   13,   13, 0x0a,
     170,   13,   13,   13, 0x0a,
     196,   13,   13,   13, 0x08,
     207,   13,   13,   13, 0x08,
     231,  226,   13,   13, 0x08,
     258,  252,   13,   13, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_ServerWidget[] = {
    "ServerWidget\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0messageEvent(QString)\0"
    "QSize\0sizeHint()\0minimumSizeHint()\0"
    "niName,ipAddress\0addServerNIEntry(QString,QString)\0"
    "addNICChangedConnection()\0setPorts()\0"
    "setupConnections()\0text\0portChanged(QString)\0"
    "index\0setServerNIC(int)\0"
};

void ServerWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        ServerWidget *_t = static_cast<ServerWidget *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: { QSize _r = _t->sizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 4: { QSize _r = _t->minimumSizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 5: _t->addServerNIEntry((*reinterpret_cast< QString(*)>(_a[1])),(*reinterpret_cast< QString(*)>(_a[2]))); break;
        case 6: _t->addNICChangedConnection(); break;
        case 7: _t->setPorts(); break;
        case 8: _t->setupConnections(); break;
        case 9: _t->portChanged((*reinterpret_cast< const QString(*)>(_a[1]))); break;
        case 10: _t->setServerNIC((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData ServerWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject ServerWidget::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_ServerWidget,
      qt_meta_data_ServerWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &ServerWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *ServerWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *ServerWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_ServerWidget))
        return static_cast<void*>(const_cast< ServerWidget*>(this));
    return QWidget::qt_metacast(_clname);
}

int ServerWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 11)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 11;
    }
    return _id;
}

// SIGNAL 0
void ServerWidget::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void ServerWidget::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void ServerWidget::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}
QT_END_MOC_NAMESPACE
