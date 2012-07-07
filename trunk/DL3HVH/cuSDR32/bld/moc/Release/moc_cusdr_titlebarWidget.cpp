/****************************************************************************
** Meta object code from reading C++ file 'cusdr_titlebarWidget.h'
**
** Created: Sat 7. Jul 18:04:42 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_titlebarWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_titlebarWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_TitleBarWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       9,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       3,       // signalCount

 // signals: signature, parameters, type, tag, flags
      16,   15,   15,   15, 0x05,
      28,   15,   15,   15, 0x05,
      43,   15,   15,   15, 0x05,

 // slots: signature, parameters, type, tag, flags
      74,   70,   15,   15, 0x0a,
      93,   87,   15,   15, 0x0a,
     122,  110,   15,   15, 0x0a,
     143,   15,   15,   15, 0x08,
     161,   15,   15,   15, 0x08,
     182,   15,   15,   15, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_TitleBarWidget[] = {
    "TitleBarWidget\0\0mainClose()\0mainMaximize()\0"
    "mainMaximizeHideTitlebar()\0pen\0"
    "setPen(QPen)\0brush\0setBrush(QBrush)\0"
    "antialiased\0setAntialiased(bool)\0"
    "mainWindowClose()\0maximizeMainWindow()\0"
    "hideTitlebar()\0"
};

void TitleBarWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        TitleBarWidget *_t = static_cast<TitleBarWidget *>(_o);
        switch (_id) {
        case 0: _t->mainClose(); break;
        case 1: _t->mainMaximize(); break;
        case 2: _t->mainMaximizeHideTitlebar(); break;
        case 3: _t->setPen((*reinterpret_cast< const QPen(*)>(_a[1]))); break;
        case 4: _t->setBrush((*reinterpret_cast< const QBrush(*)>(_a[1]))); break;
        case 5: _t->setAntialiased((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 6: _t->mainWindowClose(); break;
        case 7: _t->maximizeMainWindow(); break;
        case 8: _t->hideTitlebar(); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData TitleBarWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject TitleBarWidget::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_TitleBarWidget,
      qt_meta_data_TitleBarWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &TitleBarWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *TitleBarWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *TitleBarWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_TitleBarWidget))
        return static_cast<void*>(const_cast< TitleBarWidget*>(this));
    return QWidget::qt_metacast(_clname);
}

int TitleBarWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 9)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 9;
    }
    return _id;
}

// SIGNAL 0
void TitleBarWidget::mainClose()
{
    QMetaObject::activate(this, &staticMetaObject, 0, 0);
}

// SIGNAL 1
void TitleBarWidget::mainMaximize()
{
    QMetaObject::activate(this, &staticMetaObject, 1, 0);
}

// SIGNAL 2
void TitleBarWidget::mainMaximizeHideTitlebar()
{
    QMetaObject::activate(this, &staticMetaObject, 2, 0);
}
QT_END_MOC_NAMESPACE
