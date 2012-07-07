/****************************************************************************
** Meta object code from reading C++ file 'cusdr_oglWidget.h'
**
** Created: Wed 4. Jul 21:10:20 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_oglWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_oglWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_OGLWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       6,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
      40,   11,   10,   10, 0x08,
     159,  146,   10,   10, 0x08,
     199,  194,   10,   10, 0x08,
     229,  219,   10,   10, 0x08,
     286,  265,   10,   10, 0x08,
     329,  323,   10,   10, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_OGLWidget[] = {
    "OGLWidget\0\0sender,err,hwmode,mode,state\0"
    "systemStateChanged(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::"
    "_ServerMode,QSDR::_DataEngineState)\0"
    "sender,value\0setNumberOfReceivers(QObject*,int)\0"
    "view\0setMultiRxView(int)\0rx,buffer\0"
    "setSpectrumBuffer(int,const float*)\0"
    "sender,value,rx,freq\0"
    "setFrequency(QObject*,bool,int,long)\0"
    "value\0setWideBandStatus(bool)\0"
};

void OGLWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        OGLWidget *_t = static_cast<OGLWidget *>(_o);
        switch (_id) {
        case 0: _t->systemStateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 1: _t->setNumberOfReceivers((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 2: _t->setMultiRxView((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 3: _t->setSpectrumBuffer((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< const float*(*)>(_a[2]))); break;
        case 4: _t->setFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])),(*reinterpret_cast< long(*)>(_a[4]))); break;
        case 5: _t->setWideBandStatus((*reinterpret_cast< bool(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData OGLWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject OGLWidget::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_OGLWidget,
      qt_meta_data_OGLWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &OGLWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *OGLWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *OGLWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_OGLWidget))
        return static_cast<void*>(const_cast< OGLWidget*>(this));
    return QWidget::qt_metacast(_clname);
}

int OGLWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 6)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 6;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
