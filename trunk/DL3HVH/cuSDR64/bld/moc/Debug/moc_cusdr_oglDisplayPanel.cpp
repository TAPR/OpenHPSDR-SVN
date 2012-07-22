/****************************************************************************
** Meta object code from reading C++ file 'cusdr_oglDisplayPanel.h'
**
** Created: Sun 22. Jul 13:15:11 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_oglDisplayPanel.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_oglDisplayPanel.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_OGLDisplayPanel[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      37,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       3,       // signalCount

 // signals: signature, parameters, type, tag, flags
      24,   17,   16,   16, 0x05,
      44,   17,   16,   16, 0x05,
      69,   65,   16,   16, 0x05,

 // slots: signature, parameters, type, tag, flags
      97,   16,   91,   16, 0x0a,
     115,   16,   91,   16, 0x0a,
     139,  126,   16,   16, 0x0a,
     188,  167,   16,   16, 0x0a,
     254,  225,   16,   16, 0x08,
     365,  360,   16,   16, 0x08,
     398,  392,   16,   16, 0x08,
     417,  392,   16,   16, 0x08,
     435,  392,   16,   16, 0x08,
     460,  392,   16,   16, 0x08,
     481,  392,   16,   16, 0x08,
     505,  392,   16,   16, 0x08,
     529,  126,   16,   16, 0x08,
     553,  126,   16,   16, 0x08,
     580,  126,   16,   16, 0x08,
     604,  126,   16,   16, 0x08,
     628,  126,   16,   16, 0x08,
     657,  126,   16,   16, 0x08,
     690,  392,   16,   16, 0x08,
     715,  392,   16,   16, 0x08,
     741,  392,   16,   16, 0x08,
     763,  392,   16,   16, 0x08,
     790,  392,   16,   16, 0x08,
     812,  392,   16,   16, 0x08,
     835,  392,   16,   16, 0x08,
     859,  392,   16,   16, 0x08,
     880,  126,   16,   16, 0x08,
     914,  126,   16,   16, 0x08,
     943,  126,   16,   16, 0x08,
     991,  982,   16,   16, 0x08,
    1017,  392,   16,   16, 0x08,
    1040,   16,   16,   16, 0x08,
    1059,   16,   16,   16, 0x08,
    1077,   16,   16,   16, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_OGLDisplayPanel[] = {
    "OGLDisplayPanel\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0msg\0messageEvent(QString)\0"
    "QSize\0minimumSizeHint()\0sizeHint()\0"
    "sender,value\0setSampleRate(QObject*,int)\0"
    "sender,value,rx,freq\0"
    "setFrequency(QObject*,bool,int,long)\0"
    "sender,err,hwmode,mode,state\0"
    "systemStateChanged(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::"
    "_ServerMode,QSDR::_DataEngineState)\0"
    "size\0setupDisplayRegions(QSize)\0value\0"
    "setSyncStatus(int)\0setADCStatus(int)\0"
    "setPacketLossStatus(int)\0setSendIQStatus(int)\0"
    "setRecvAudioStatus(int)\0setCurrentReceiver(int)\0"
    "setPreamp(QObject*,int)\0"
    "setReceivers(QObject*,int)\0"
    "setDither(QObject*,int)\0setRandom(QObject*,int)\0"
    "set10mhzSource(QObject*,int)\0"
    "set122_88mhzSource(QObject*,int)\0"
    "setMercuryPresence(bool)\0"
    "setPenelopePresence(bool)\0"
    "setAlexPresence(bool)\0setExcaliburPresence(bool)\0"
    "setHermesVersion(int)\0setMercuryVersion(int)\0"
    "setPenelopeVersion(int)\0setMetisVersion(int)\0"
    "setExcaliburVersion(QObject*,int)\0"
    "setAlexVersion(QObject*,int)\0"
    "setMouseWheelFreqStep(QObject*,double)\0"
    "rx,value\0setSMeterValue(int,float)\0"
    "setSMeterHoldTime(int)\0updateSyncStatus()\0"
    "updateADCStatus()\0updatePacketLossStatus()\0"
};

void OGLDisplayPanel::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        OGLDisplayPanel *_t = static_cast<OGLDisplayPanel *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: { QSize _r = _t->minimumSizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 4: { QSize _r = _t->sizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 5: _t->setSampleRate((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 6: _t->setFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])),(*reinterpret_cast< long(*)>(_a[4]))); break;
        case 7: _t->systemStateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 8: _t->setupDisplayRegions((*reinterpret_cast< QSize(*)>(_a[1]))); break;
        case 9: _t->setSyncStatus((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 10: _t->setADCStatus((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 11: _t->setPacketLossStatus((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 12: _t->setSendIQStatus((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 13: _t->setRecvAudioStatus((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 14: _t->setCurrentReceiver((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 15: _t->setPreamp((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 16: _t->setReceivers((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 17: _t->setDither((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 18: _t->setRandom((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 19: _t->set10mhzSource((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 20: _t->set122_88mhzSource((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 21: _t->setMercuryPresence((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 22: _t->setPenelopePresence((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 23: _t->setAlexPresence((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 24: _t->setExcaliburPresence((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 25: _t->setHermesVersion((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 26: _t->setMercuryVersion((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 27: _t->setPenelopeVersion((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 28: _t->setMetisVersion((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 29: _t->setExcaliburVersion((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 30: _t->setAlexVersion((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 31: _t->setMouseWheelFreqStep((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< double(*)>(_a[2]))); break;
        case 32: _t->setSMeterValue((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< float(*)>(_a[2]))); break;
        case 33: _t->setSMeterHoldTime((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 34: _t->updateSyncStatus(); break;
        case 35: _t->updateADCStatus(); break;
        case 36: _t->updatePacketLossStatus(); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData OGLDisplayPanel::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject OGLDisplayPanel::staticMetaObject = {
    { &QGLWidget::staticMetaObject, qt_meta_stringdata_OGLDisplayPanel,
      qt_meta_data_OGLDisplayPanel, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &OGLDisplayPanel::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *OGLDisplayPanel::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *OGLDisplayPanel::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_OGLDisplayPanel))
        return static_cast<void*>(const_cast< OGLDisplayPanel*>(this));
    return QGLWidget::qt_metacast(_clname);
}

int OGLDisplayPanel::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QGLWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 37)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 37;
    }
    return _id;
}

// SIGNAL 0
void OGLDisplayPanel::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void OGLDisplayPanel::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void OGLDisplayPanel::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}
QT_END_MOC_NAMESPACE
