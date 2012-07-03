/****************************************************************************
** Meta object code from reading C++ file 'cusdr_radioWidget.h'
**
** Created: Tue 3. Jul 21:43:42 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_radioWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_radioWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_RadioWidget[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      30,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       3,       // signalCount

 // signals: signature, parameters, type, tag, flags
      20,   13,   12,   12, 0x05,
      40,   13,   12,   12, 0x05,
      65,   61,   12,   12, 0x05,

 // slots: signature, parameters, type, tag, flags
      91,   12,   85,   12, 0x0a,
     102,   12,   85,   12, 0x0a,
     149,  120,   12,   12, 0x0a,
     255,   12,   12,   12, 0x08,
     276,   12,   12,   12, 0x08,
     297,   12,   12,   12, 0x08,
     317,   12,   12,   12, 0x08,
     341,   12,   12,   12, 0x08,
     365,   12,   12,   12, 0x08,
     402,  397,  389,   12, 0x08,
     429,  423,   12,   12, 0x08,
     479,  453,   12,   12, 0x08,
     520,   12,   12,   12, 0x08,
     554,  539,   12,   12, 0x08,
     588,   12,   12,   12, 0x08,
     625,  610,   12,   12, 0x08,
     662,   12,   12,   12, 0x08,
     684,  610,   12,   12, 0x08,
     721,   12,   12,   12, 0x08,
     761,  742,   12,   12, 0x08,
     806,  801,   12,   12, 0x08,
     834,   12,   12,   12, 0x08,
     850,   12,   12,   12, 0x08,
     866,   12,   12,   12, 0x08,
     882,   12,   12,   12, 0x08,
     906,   12,   12,   12, 0x08,
     930,   12,   12,   12, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_RadioWidget[] = {
    "RadioWidget\0\0sender\0showEvent(QObject*)\0"
    "closeEvent(QObject*)\0msg\0newMessage(QString)\0"
    "QSize\0sizeHint()\0minimumSizeHint()\0"
    "sender,err,hwmode,mode,state\0"
    "systemStateChanged(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::"
    "_ServerMode,QSDR::_DataEngineState)\0"
    "createBandBtnGroup()\0createModeBtnGroup()\0"
    "createAgcBtnGroup()\0createFilterBtnGroupA()\0"
    "createFilterBtnGroupB()\0createFilterBtnGroupC()\0"
    "QLabel*\0text\0createLabel(QString)\0"
    "value\0setCurrentReceiver(int)\0"
    "sender,value,rx,frequency\0"
    "frequencyChanged(QObject*,bool,int,long)\0"
    "bandChangedByBtn()\0sender,rx,band\0"
    "bandChanged(QObject*,int,HamBand)\0"
    "dspModeChangedByBtn()\0sender,rx,mode\0"
    "dspModeChanged(QObject*,int,DSPMode)\0"
    "agcModeChangedByBtn()\0"
    "agcModeChanged(QObject*,int,AGCMode)\0"
    "filterChangedByBtn()\0sender,rx,low,high\0"
    "filterChanged(QObject*,int,qreal,qreal)\0"
    "mode\0filterGroupChanged(DSPMode)\0"
    "preampChanged()\0ditherChanged()\0"
    "randomChanged()\0sampleRateChangedTo48()\0"
    "sampleRateChangedTo96()\0"
    "sampleRateChangedTo192()\0"
};

void RadioWidget::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        RadioWidget *_t = static_cast<RadioWidget *>(_o);
        switch (_id) {
        case 0: _t->showEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 1: _t->closeEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 2: _t->newMessage((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 3: { QSize _r = _t->sizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 4: { QSize _r = _t->minimumSizeHint();
            if (_a[0]) *reinterpret_cast< QSize*>(_a[0]) = _r; }  break;
        case 5: _t->systemStateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 6: _t->createBandBtnGroup(); break;
        case 7: _t->createModeBtnGroup(); break;
        case 8: _t->createAgcBtnGroup(); break;
        case 9: _t->createFilterBtnGroupA(); break;
        case 10: _t->createFilterBtnGroupB(); break;
        case 11: _t->createFilterBtnGroupC(); break;
        case 12: { QLabel* _r = _t->createLabel((*reinterpret_cast< const QString(*)>(_a[1])));
            if (_a[0]) *reinterpret_cast< QLabel**>(_a[0]) = _r; }  break;
        case 13: _t->setCurrentReceiver((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 14: _t->frequencyChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])),(*reinterpret_cast< long(*)>(_a[4]))); break;
        case 15: _t->bandChangedByBtn(); break;
        case 16: _t->bandChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< HamBand(*)>(_a[3]))); break;
        case 17: _t->dspModeChangedByBtn(); break;
        case 18: _t->dspModeChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< DSPMode(*)>(_a[3]))); break;
        case 19: _t->agcModeChangedByBtn(); break;
        case 20: _t->agcModeChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< AGCMode(*)>(_a[3]))); break;
        case 21: _t->filterChangedByBtn(); break;
        case 22: _t->filterChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< qreal(*)>(_a[3])),(*reinterpret_cast< qreal(*)>(_a[4]))); break;
        case 23: _t->filterGroupChanged((*reinterpret_cast< DSPMode(*)>(_a[1]))); break;
        case 24: _t->preampChanged(); break;
        case 25: _t->ditherChanged(); break;
        case 26: _t->randomChanged(); break;
        case 27: _t->sampleRateChangedTo48(); break;
        case 28: _t->sampleRateChangedTo96(); break;
        case 29: _t->sampleRateChangedTo192(); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData RadioWidget::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject RadioWidget::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_RadioWidget,
      qt_meta_data_RadioWidget, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &RadioWidget::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *RadioWidget::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *RadioWidget::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_RadioWidget))
        return static_cast<void*>(const_cast< RadioWidget*>(this));
    return QWidget::qt_metacast(_clname);
}

int RadioWidget::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 30)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 30;
    }
    return _id;
}

// SIGNAL 0
void RadioWidget::showEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void RadioWidget::closeEvent(QObject * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void RadioWidget::newMessage(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}
QT_END_MOC_NAMESPACE
