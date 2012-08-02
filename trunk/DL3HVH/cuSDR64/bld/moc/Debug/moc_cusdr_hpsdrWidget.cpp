/****************************************************************************
** Meta object code from reading C++ file 'cusdr_hpsdrWidget.h'
**
** Created: Mon 30. Jul 19:03:51 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_hpsdrWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_hpsdrWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_HPSDRWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      32,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       3,       // signalCount

 // signals: signature, parameters, type, tag, flags
      20,   13,   12,   12, 0x05,
      40,   13,   12,   12, 0x05,
      69,   61,   12,   12, 0x05,

 // slots: signature, parameters, type, tag, flags
     108,   91,   12,   12, 0x0a,
     143,   12,   12,   12, 0x0a,
     181,  169,   12,   12, 0x0a,
     212,   12,   12,   12, 0x0a,
     235,   12,   12,   12, 0x0a,
     256,   12,   12,   12, 0x0a,
     282,   12,   12,   12, 0x0a,
     305,   12,   12,   12, 0x0a,
     330,   12,   12,   12, 0x0a,
     352,   12,   12,   12, 0x0a,
     379,   12,   12,   12, 0x0a,
     433,  404,   12,   12, 0x08,
     539,   12,   12,   12, 0x08,
     558,   12,   12,   12, 0x08,
     579,   12,   12,   12, 0x08,
     601,   12,   12,   12, 0x08,
     631,   12,   12,   12, 0x08,
     663,  657,   12,   12, 0x08,
     701,  688,   12,   12, 0x08,
     735,  688,   12,   12, 0x08,
     769,  763,   12,   12, 0x08,
     792,  787,   12,   12, 0x08,
     845,  840,   12,   12, 0x08,
     889,  657,   12,   12, 0x08,
     915,   12,   12,   12, 0x08,
     929,  657,   12,   12, 0x08,
     959,   12,   12,   12, 0x08,
     976,   12,   12,   12, 0x08,
     992,   12,   12,   12, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_HPSDRWidget[] = {
    "HPSDRWidget\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0message\0"
    "messageEvent(QString)\0niName,ipAddress\0"
    "addDeviceNICEntry(QString,QString)\0"
    "addNICChangedConnection()\0sender,size\0"
    "setSocketBufSize(QObject*,int)\0"
    "hpsdrHardwareChanged()\0hwInterfaceChanged()\0"
    "penelopePresenceChanged()\0"
    "pennyPresenceChanged()\0mercuryPresenceChanged()\0"
    "alexPresenceChanged()\0excaliburPresenceChanged()\0"
    "source122_88MhzChanged()\0"
    "sender,err,hwmode,mode,state\0"
    "systemStateChanged(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::"
    "_ServerMode,QSDR::_DataEngineState)\0"
    "setHPSDRHardware()\0source10MhzChanged()\0"
    "interfaceBtnClicked()\0"
    "searchHPSDRDeviceBtnClicked()\0"
    "socketBufSizeBtnClicked()\0value\0"
    "setSocketBufferSize(int)\0sender,value\0"
    "setClientConnected(QObject*,bool)\0"
    "setConnected(QObject*,bool)\0index\0"
    "setDeviceNIC(int)\0list\0"
    "setNetworkDeviceList(QList<TNetworkDevicecard>)\0"
    "card\0setCurrentNetworkDevice(TNetworkDevicecard)\0"
    "setNumberOfReceivers(int)\0setShow1on2()\0"
    "numberOfReceiversChanged(int)\0"
    "disableButtons()\0enableButtons()\0"
    "setView()\0"
};

void HPSDRWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        HPSDRWidget *_t = static_cast<HPSDRWidget *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: _t->addDeviceNICEntry((*reinterpret_cast< QString(*)>(_a[1])),(*reinterpret_cast< QString(*)>(_a[2]))); break;
        case 4: _t->addNICChangedConnection(); break;
        case 5: _t->setSocketBufSize((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 6: _t->hpsdrHardwareChanged(); break;
        case 7: _t->hwInterfaceChanged(); break;
        case 8: _t->penelopePresenceChanged(); break;
        case 9: _t->pennyPresenceChanged(); break;
        case 10: _t->mercuryPresenceChanged(); break;
        case 11: _t->alexPresenceChanged(); break;
        case 12: _t->excaliburPresenceChanged(); break;
        case 13: _t->source122_88MhzChanged(); break;
        case 14: _t->systemStateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 15: _t->setHPSDRHardware(); break;
        case 16: _t->source10MhzChanged(); break;
        case 17: _t->interfaceBtnClicked(); break;
        case 18: _t->searchHPSDRDeviceBtnClicked(); break;
        case 19: _t->socketBufSizeBtnClicked(); break;
        case 20: _t->setSocketBufferSize((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 21: _t->setClientConnected((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2]))); break;
        case 22: _t->setConnected((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2]))); break;
        case 23: _t->setDeviceNIC((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 24: _t->setNetworkDeviceList((*reinterpret_cast< QList<TNetworkDevicecard>(*)>(_a[1]))); break;
        case 25: _t->setCurrentNetworkDevice((*reinterpret_cast< TNetworkDevicecard(*)>(_a[1]))); break;
        case 26: _t->setNumberOfReceivers((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 27: _t->setShow1on2(); break;
        case 28: _t->numberOfReceiversChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 29: _t->disableButtons(); break;
        case 30: _t->enableButtons(); break;
        case 31: _t->setView(); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData HPSDRWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject HPSDRWidget::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_HPSDRWidget,
      qt_meta_data_HPSDRWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &HPSDRWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *HPSDRWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *HPSDRWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_HPSDRWidget))
        return static_cast<void*>(const_cast< HPSDRWidget*>(this));
    return QWidget::qt_metacast(_clname);
}

int HPSDRWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 32)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 32;
    }
    return _id;
}

// SIGNAL 0
void HPSDRWidget::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void HPSDRWidget::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void HPSDRWidget::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}
QT_END_MOC_NAMESPACE
