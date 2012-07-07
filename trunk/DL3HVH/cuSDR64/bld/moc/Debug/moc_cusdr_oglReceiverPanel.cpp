/****************************************************************************
** Meta object code from reading C++ file 'cusdr_oglReceiverPanel.h'
**
** Created: Sat 7. Jul 09:06:38 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_oglReceiverPanel.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_oglReceiverPanel.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_QGLReceiverPanel[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      24,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       4,       // signalCount

 // signals: signature, parameters, type, tag, flags
      25,   18,   17,   17, 0x05,
      45,   18,   17,   17, 0x05,
      70,   66,   17,   17, 0x05,
      96,   92,   17,   17, 0x05,

 // slots: signature, parameters, type, tag, flags
     124,   17,  118,   17, 0x0a,
     142,  135,   17,   17, 0x0a,
     192,  174,   17,   17, 0x0a,
     254,  225,   17,   17, 0x08,
     387,  360,   17,   17, 0x08,
     478,   17,   17,   17, 0x08,
     503,  497,   17,   17, 0x08,
     543,  527,   17,   17, 0x08,
     603,  590,   17,   17, 0x08,
     632,  627,   17,   17, 0x08,
     659,  497,   17,   17, 0x08,
     686,  497,   17,   17, 0x08,
     715,  497,   17,   17, 0x08,
     738,   17,   17,   17, 0x08,
     762,  760,   17,   17, 0x08,
     787,  780,   17,   17, 0x08,
     823,  590,   17,   17, 0x08,
     864,  855,   17,   17, 0x08,
     890,  855,   17,   17, 0x08,
     921,  855,   17,   17, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_QGLReceiverPanel[] = {
    "QGLReceiverPanel\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0msg\0messageEvent(QString)\0"
    "x,y\0coordChanged(int,int)\0QSize\0"
    "sizeHint()\0buffer\0setSpectrumBuffer(const float*)\0"
    "sender,value,freq\0setFrequency(QObject*,bool,long)\0"
    "sender,err,hwmode,mode,state\0"
    "systemStateChanged(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::"
    "_ServerMode,QSDR::_DataEngineState)\0"
    "sender,panMode,colorScheme\0"
    "graphicModeChanged(QObject*,QSDRGraphics::_Panadapter,QSDRGraphics::_W"
    "aterfallColorScheme)\0"
    "setupConnections()\0value\0"
    "setCurrentReceiver(int)\0sender,rx,lo,hi\0"
    "setFilterFrequencies(QObject*,int,qreal,qreal)\0"
    "sender,value\0setPreamp(QObject*,int)\0"
    "size\0setupDisplayRegions(QSize)\0"
    "setSpectrumAveraging(bool)\0"
    "setSpectrumAveragingCnt(int)\0"
    "setPanGridStatus(bool)\0setPanadapterColors()\0"
    "p\0getRegion(QPoint)\0pos,rx\0"
    "freqRulerPositionChanged(float,int)\0"
    "sampleRateChanged(QObject*,int)\0"
    "rx,value\0setWaterfallTime(int,int)\0"
    "setWaterfallOffesetLo(int,int)\0"
    "setWaterfallOffesetHi(int,int)\0"
};

void QGLReceiverPanel::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        QGLReceiverPanel *_t = static_cast<QGLReceiverPanel *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: _t->coordChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 4: { QSize _r = _t->sizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 5: _t->setSpectrumBuffer((*reinterpret_cast< const float*(*)>(_a[1]))); break;
        case 6: _t->setFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2])),(*reinterpret_cast< long(*)>(_a[3]))); break;
        case 7: _t->systemStateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 8: _t->graphicModeChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDRGraphics::_Panadapter(*)>(_a[2])),(*reinterpret_cast< QSDRGraphics::_WaterfallColorScheme(*)>(_a[3]))); break;
        case 9: _t->setupConnections(); break;
        case 10: _t->setCurrentReceiver((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 11: _t->setFilterFrequencies((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< qreal(*)>(_a[3])),(*reinterpret_cast< qreal(*)>(_a[4]))); break;
        case 12: _t->setPreamp((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 13: _t->setupDisplayRegions((*reinterpret_cast< QSize(*)>(_a[1]))); break;
        case 14: _t->setSpectrumAveraging((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 15: _t->setSpectrumAveragingCnt((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 16: _t->setPanGridStatus((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 17: _t->setPanadapterColors(); break;
        case 18: _t->getRegion((*reinterpret_cast< QPoint(*)>(_a[1]))); break;
        case 19: _t->freqRulerPositionChanged((*reinterpret_cast< float(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 20: _t->sampleRateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 21: _t->setWaterfallTime((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 22: _t->setWaterfallOffesetLo((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 23: _t->setWaterfallOffesetHi((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData QGLReceiverPanel::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject QGLReceiverPanel::staticMetaObject = {
    { &QGLWidget::staticMetaObject, qt_meta_stringdata_QGLReceiverPanel,
      qt_meta_data_QGLReceiverPanel, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &QGLReceiverPanel::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *QGLReceiverPanel::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *QGLReceiverPanel::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_QGLReceiverPanel))
        return static_cast<void*>(const_cast< QGLReceiverPanel*>(this));
    return QGLWidget::qt_metacast(_clname);
}

int QGLReceiverPanel::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QGLWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 24)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 24;
    }
    return _id;
}

// SIGNAL 0
void QGLReceiverPanel::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void QGLReceiverPanel::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void QGLReceiverPanel::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void QGLReceiverPanel::coordChanged(int _t1, int _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}
QT_END_MOC_NAMESPACE
