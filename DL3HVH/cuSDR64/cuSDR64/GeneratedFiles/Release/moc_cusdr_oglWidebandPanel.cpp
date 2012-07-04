/****************************************************************************
** Meta object code from reading C++ file 'cusdr_oglWidebandPanel.h'
**
** Created: Wed 4. Jul 21:10:21 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_oglWidebandPanel.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_oglWidebandPanel.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_QGLWidebandPanel[] = {

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
     203,  174,   17,   17, 0x08,
     336,  309,   17,   17, 0x08,
     427,   17,   17,   17, 0x08,
     452,  446,   17,   17, 0x08,
     497,  476,   17,   17, 0x08,
     539,  534,   17,   17, 0x08,
     566,  135,   17,   17, 0x08,
     606,  446,   17,   17, 0x08,
     635,   17,   17,   17, 0x08,
     657,  446,   17,   17, 0x08,
     682,  680,   17,   17, 0x08,
     713,  700,   17,   17, 0x08,
     745,  446,   17,   17, 0x08,
     767,  446,   17,   17, 0x08,
     788,  446,   17,   17, 0x08,
     809,  446,   17,   17, 0x08,
     829,  446,   17,   17, 0x08,
     849,  446,   17,   17, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_QGLWidebandPanel[] = {
    "QGLWidebandPanel\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0msg\0messageEvent(QString)\0"
    "x,y\0coordChanged(int,int)\0QSize\0"
    "sizeHint()\0buffer\0setSpectrumBuffer(const float*)\0"
    "sender,err,hwmode,mode,state\0"
    "systemStateChanged(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::"
    "_ServerMode,QSDR::_DataEngineState)\0"
    "sender,panMode,colorScheme\0"
    "graphicModeChanged(QObject*,QSDRGraphics::_Panadapter,QSDRGraphics::_W"
    "aterfallColorScheme)\0"
    "setupConnections()\0value\0"
    "setCurrentReceiver(int)\0sender,value,rx,freq\0"
    "setFrequency(QObject*,bool,int,long)\0"
    "size\0setupDisplayRegions(QSize)\0"
    "setWidebandSpectrumBuffer(const float*)\0"
    "setSpectrumAveragingCnt(int)\0"
    "setPanadapterColors()\0setPanGridStatus(bool)\0"
    "p\0getRegion(QPoint)\0sender,value\0"
    "sampleRateChanged(QObject*,int)\0"
    "freqScaleUpdate(bool)\0freqScaleRenew(bool)\0"
    "dBmScaleUpdate(bool)\0dBmScaleRenew(bool)\0"
    "panGridUpdate(bool)\0panGridRenew(bool)\0"
};

void QGLWidebandPanel::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        QGLWidebandPanel *_t = static_cast<QGLWidebandPanel *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: _t->coordChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 4: { QSize _r = _t->sizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 5: _t->setSpectrumBuffer((*reinterpret_cast< const float*(*)>(_a[1]))); break;
        case 6: _t->systemStateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 7: _t->graphicModeChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDRGraphics::_Panadapter(*)>(_a[2])),(*reinterpret_cast< QSDRGraphics::_WaterfallColorScheme(*)>(_a[3]))); break;
        case 8: _t->setupConnections(); break;
        case 9: _t->setCurrentReceiver((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 10: _t->setFrequency((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])),(*reinterpret_cast< long(*)>(_a[4]))); break;
        case 11: _t->setupDisplayRegions((*reinterpret_cast< QSize(*)>(_a[1]))); break;
        case 12: _t->setWidebandSpectrumBuffer((*reinterpret_cast< const float*(*)>(_a[1]))); break;
        case 13: _t->setSpectrumAveragingCnt((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 14: _t->setPanadapterColors(); break;
        case 15: _t->setPanGridStatus((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 16: _t->getRegion((*reinterpret_cast< QPoint(*)>(_a[1]))); break;
        case 17: _t->sampleRateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 18: _t->freqScaleUpdate((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 19: _t->freqScaleRenew((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 20: _t->dBmScaleUpdate((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 21: _t->dBmScaleRenew((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 22: _t->panGridUpdate((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 23: _t->panGridRenew((*reinterpret_cast< bool(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData QGLWidebandPanel::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject QGLWidebandPanel::staticMetaObject = {
    { &QGLWidget::staticMetaObject, qt_meta_stringdata_QGLWidebandPanel,
      qt_meta_data_QGLWidebandPanel, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &QGLWidebandPanel::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *QGLWidebandPanel::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *QGLWidebandPanel::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_QGLWidebandPanel))
        return static_cast<void*>(const_cast< QGLWidebandPanel*>(this));
    return QGLWidget::qt_metacast(_clname);
}

int QGLWidebandPanel::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
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
void QGLWidebandPanel::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void QGLWidebandPanel::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void QGLWidebandPanel::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void QGLWidebandPanel::coordChanged(int _t1, int _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}
QT_END_MOC_NAMESPACE
