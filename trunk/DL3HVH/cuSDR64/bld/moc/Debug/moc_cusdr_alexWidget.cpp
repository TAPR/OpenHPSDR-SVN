/****************************************************************************
** Meta object code from reading C++ file 'cusdr_alexWidget.h'
**
** Created: Mon 30. Jul 19:03:49 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_alexWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_alexWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_AlexWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      25,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       3,       // signalCount

 // signals: signature, parameters, type, tag, flags
      19,   12,   11,   11, 0x05,
      39,   12,   11,   11, 0x05,
      60,   11,   11,   11, 0x05,

 // slots: signature, parameters, type, tag, flags
      82,   11,   11,   11, 0x08,
     103,   11,   11,   11, 0x08,
     123,   11,   11,   11, 0x08,
     143,   11,   11,   11, 0x08,
     163,   11,   11,   11, 0x08,
     183,   11,   11,   11, 0x08,
     203,   11,   11,   11, 0x08,
     223,   11,   11,   11, 0x08,
     243,   11,   11,   11, 0x08,
     263,   11,   11,   11, 0x08,
     283,   11,   11,   11, 0x08,
     302,   11,   11,   11, 0x08,
     325,   11,   11,   11, 0x08,
     347,   11,   11,   11, 0x08,
     369,   11,   11,   11, 0x08,
     391,   11,   11,   11, 0x08,
     413,   11,   11,   11, 0x08,
     435,   11,   11,   11, 0x08,
     457,   11,   11,   11, 0x08,
     479,   11,   11,   11, 0x08,
     501,   11,   11,   11, 0x08,
     523,   11,   11,   11, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_AlexWidget[] = {
    "AlexWidget\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0messageEvent(QString)\0"
    "band160mBtnClicked()\0band80mBtnClicked()\0"
    "band60mBtnClicked()\0band40mBtnClicked()\0"
    "band30mBtnClicked()\0band20mBtnClicked()\0"
    "band17mBtnClicked()\0band15mBtnClicked()\0"
    "band12mBtnClicked()\0band10mBtnClicked()\0"
    "band6mBtnClicked()\0band160mBtnRxClicked()\0"
    "band80mBtnRxClicked()\0band60mBtnRxClicked()\0"
    "band40mBtnRxClicked()\0band30mBtnRxClicked()\0"
    "band20mBtnRxClicked()\0band17mBtnRxClicked()\0"
    "band15mBtnRxClicked()\0band12mBtnRxClicked()\0"
    "band10mBtnRxClicked()\0band6mBtnRxClicked()\0"
};

void AlexWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        AlexWidget *_t = static_cast<AlexWidget *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: _t->band160mBtnClicked(); break;
        case 4: _t->band80mBtnClicked(); break;
        case 5: _t->band60mBtnClicked(); break;
        case 6: _t->band40mBtnClicked(); break;
        case 7: _t->band30mBtnClicked(); break;
        case 8: _t->band20mBtnClicked(); break;
        case 9: _t->band17mBtnClicked(); break;
        case 10: _t->band15mBtnClicked(); break;
        case 11: _t->band12mBtnClicked(); break;
        case 12: _t->band10mBtnClicked(); break;
        case 13: _t->band6mBtnClicked(); break;
        case 14: _t->band160mBtnRxClicked(); break;
        case 15: _t->band80mBtnRxClicked(); break;
        case 16: _t->band60mBtnRxClicked(); break;
        case 17: _t->band40mBtnRxClicked(); break;
        case 18: _t->band30mBtnRxClicked(); break;
        case 19: _t->band20mBtnRxClicked(); break;
        case 20: _t->band17mBtnRxClicked(); break;
        case 21: _t->band15mBtnRxClicked(); break;
        case 22: _t->band12mBtnRxClicked(); break;
        case 23: _t->band10mBtnRxClicked(); break;
        case 24: _t->band6mBtnRxClicked(); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData AlexWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject AlexWidget::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_AlexWidget,
      qt_meta_data_AlexWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &AlexWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *AlexWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *AlexWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_AlexWidget))
        return static_cast<void*>(const_cast< AlexWidget*>(this));
    return QWidget::qt_metacast(_clname);
}

int AlexWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 25)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 25;
    }
    return _id;
}

// SIGNAL 0
void AlexWidget::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void AlexWidget::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void AlexWidget::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}
QT_END_MOC_NAMESPACE
