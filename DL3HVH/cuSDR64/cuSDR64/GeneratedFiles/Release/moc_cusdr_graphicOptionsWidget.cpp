/****************************************************************************
** Meta object code from reading C++ file 'cusdr_graphicOptionsWidget.h'
**
** Created: Wed 4. Jul 21:10:22 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_graphicOptionsWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_graphicOptionsWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_GraphicOptionsWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      26,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       5,       // signalCount

 // signals: signature, parameters, type, tag, flags
      28,   22,   21,   21, 0x05,
      55,   49,   21,   21, 0x05,
      89,   82,   21,   21, 0x05,
     109,   82,   21,   21, 0x05,
     130,   21,   21,   21, 0x05,

 // slots: signature, parameters, type, tag, flags
     158,   21,  152,   21, 0x0a,
     169,   21,  152,   21, 0x0a,
     216,  187,   21,   21, 0x08,
     354,  322,   21,   21, 0x08,
     448,  445,   21,   21, 0x08,
     472,   21,   21,   21, 0x08,
     489,   21,   21,   21, 0x08,
     514,   21,   21,   21, 0x08,
     530,   21,   21,   21, 0x08,
     554,   21,   21,   21, 0x08,
     573,   21,   21,   21, 0x08,
     595,   49,   21,   21, 0x08,
     621,   49,   21,   21, 0x08,
     651,   49,   21,   21, 0x08,
     681,   49,   21,   21, 0x08,
     708,   21,   21,   21, 0x08,
     722,   21,   21,   21, 0x08,
     737,   22,   21,   21, 0x08,
     766,   49,   21,   21, 0x08,
     787,   49,   21,   21, 0x08,
     831,  818,   21,   21, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_GraphicOptionsWidget[] = {
    "GraphicOptionsWidget\0\0color\0"
    "colorChanged(QColor)\0value\0"
    "averagingModeChanged(bool)\0sender\0"
    "showEvent(QObject*)\0closeEvent(QObject*)\0"
    "messageEvent(QString)\0QSize\0sizeHint()\0"
    "minimumSizeHint()\0sender,err,hwmode,mode,state\0"
    "systemStateChanged(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::"
    "_ServerMode,QSDR::_DataEngineState)\0"
    "sender,panMode,waterColorScheme\0"
    "graphicModeChanged(QObject*,QSDRGraphics::_Panadapter,QSDRGraphics::_W"
    "aterfallColorScheme)\0"
    "rx\0setCurrentReceiver(int)\0panModeChanged()\0"
    "waterfallSchemeChanged()\0sMeterChanged()\0"
    "resolutionModeChanged()\0setAveragingMode()\0"
    "colorChooserChanged()\0waterfallTimeChanged(int)\0"
    "waterfallLoOffsetChanged(int)\0"
    "waterfallHiOffsetChanged(int)\0"
    "sMeterHoldTimeChanged(int)\0resetColors()\0"
    "acceptColors()\0triangleColorChanged(QColor)\0"
    "fpsValueChanged(int)\0"
    "averagingFilterCntChanged(int)\0"
    "sender,value\0sampleRateChanged(QObject*,int)\0"
};

void GraphicOptionsWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        GraphicOptionsWidget *_t = static_cast<GraphicOptionsWidget *>(_o);
        switch (_id) {
        case 0: _t->colorChanged((*reinterpret_cast< const QColor(*)>(_a[1]))); break;
        case 1: _t->averagingModeChanged((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 2: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 3: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 4: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 5: { QSize _r = _t->sizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 6: { QSize _r = _t->minimumSizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 7: _t->systemStateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 8: _t->graphicModeChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDRGraphics::_Panadapter(*)>(_a[2])),(*reinterpret_cast< QSDRGraphics::_WaterfallColorScheme(*)>(_a[3]))); break;
        case 9: _t->setCurrentReceiver((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 10: _t->panModeChanged(); break;
        case 11: _t->waterfallSchemeChanged(); break;
        case 12: _t->sMeterChanged(); break;
        case 13: _t->resolutionModeChanged(); break;
        case 14: _t->setAveragingMode(); break;
        case 15: _t->colorChooserChanged(); break;
        case 16: _t->waterfallTimeChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 17: _t->waterfallLoOffsetChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 18: _t->waterfallHiOffsetChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 19: _t->sMeterHoldTimeChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 20: _t->resetColors(); break;
        case 21: _t->acceptColors(); break;
        case 22: _t->triangleColorChanged((*reinterpret_cast< QColor(*)>(_a[1]))); break;
        case 23: _t->fpsValueChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 24: _t->averagingFilterCntChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 25: _t->sampleRateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData GraphicOptionsWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject GraphicOptionsWidget::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_GraphicOptionsWidget,
      qt_meta_data_GraphicOptionsWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &GraphicOptionsWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *GraphicOptionsWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *GraphicOptionsWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_GraphicOptionsWidget))
        return static_cast<void*>(const_cast< GraphicOptionsWidget*>(this));
    return QWidget::qt_metacast(_clname);
}

int GraphicOptionsWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 26)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 26;
    }
    return _id;
}

// SIGNAL 0
void GraphicOptionsWidget::colorChanged(const QColor & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void GraphicOptionsWidget::averagingModeChanged(bool _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void GraphicOptionsWidget::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void GraphicOptionsWidget::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}

// SIGNAL 4
void GraphicOptionsWidget::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 4, _a);
}
QT_END_MOC_NAMESPACE
