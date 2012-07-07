/****************************************************************************
** Meta object code from reading C++ file 'cusdr_filter.h'
**
** Created: Wed 4. Jul 21:06:09 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_filter.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_filter.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_QFilter[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      24,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
      14,    9,    8,    8, 0x08,
      78,   31,    8,    8, 0x08,
     164,  128,    8,    8, 0x08,
     251,  205,    8,    8, 0x08,
     343,  308,    8,    8, 0x08,
     420,  391,    8,    8, 0x08,
     464,    8,    8,    8, 0x08,
     495,  482,    8,    8, 0x0a,
     541,  523,    8,    8, 0x0a,
     591,  573,    8,    8, 0x0a,
     631,  618,    8,    8, 0x0a,
     660,  618,    8,    8, 0x0a,
     696,  618,    8,    8, 0x0a,
     734,  618,  730,    8, 0x0a,
     784,  768,    8,    8, 0x0a,
     808,    8,    8,    8, 0x0a,
     839,  827,    8,    8, 0x0a,
     870,    8,  864,    8, 0x0a,
     881,    8,  864,    8, 0x0a,
     892,    8,  730,    8, 0x0a,
     913,  907,    8,    8, 0x0a,
     932,  907,    8,    8, 0x0a,
     963,  951,    8,    8, 0x0a,
     986,  907,    8,    8, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_QFilter[] = {
    "QFilter\0\0taps\0LoadFilter(CPX*)\0"
    "cutoff,samplerate,wtype,taps_re,taps_im,length\0"
    "MakeFirLowpass(float,float,int,float*,float*,int)\0"
    "cutoff,samplerate,wtype,taps,length\0"
    "MakeFirLowpass(float,float,int,CPX*,int)\0"
    "lo,hi,samplerate,wtype,taps_re,taps_im,length\0"
    "MakeFirBandpass(float,float,float,int,float*,float*,int)\0"
    "lo,hi,samplerate,wtype,taps,length\0"
    "MakeFirBandpass(float,float,float,int,CPX*,int)\0"
    "lo,hi,samplerate,taps,length\0"
    "MakeFirBandstop(float,float,float,CPX*,int)\0"
    "DoOverlapAddCPX()\0sender,value\0"
    "setSampleRate(QObject*,int)\0"
    "lo,hi,ftype,wtype\0MakeFilter(float,float,int,int)\0"
    "wtype,size,window\0MakeWindow(int,int,float*)\0"
    "in,out,bsize\0ProcessFilter(CPX*,CPX*,int)\0"
    "ProcessForwardFilter(CPX*,CPX*,int)\0"
    "ProcessChirpFilter(CPX*,CPX*,int)\0int\0"
    "ProcessAndDecimate(CPX*,CPX*,int)\0"
    "in,out,downrate\0Decimate(CPX*,CPX*,int)\0"
    "DoConvolutionCPX()\0in,out,size\0"
    "Normalize(CPX*,CPX*,int)\0float\0"
    "filterLo()\0filterHi()\0isStreamMode()\0"
    "value\0setFilterLo(float)\0setFilterHi(float)\0"
    "loval,hival\0setFilter(float,float)\0"
    "setStreamMode(int)\0"
};

void QFilter::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        QFilter *_t = static_cast<QFilter *>(_o);
        switch (_id) {
        case 0: _t->LoadFilter((*reinterpret_cast< CPX*(*)>(_a[1]))); break;
        case 1: _t->MakeFirLowpass((*reinterpret_cast< float(*)>(_a[1])),(*reinterpret_cast< float(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])),(*reinterpret_cast< float*(*)>(_a[4])),(*reinterpret_cast< float*(*)>(_a[5])),(*reinterpret_cast< int(*)>(_a[6]))); break;
        case 2: _t->MakeFirLowpass((*reinterpret_cast< float(*)>(_a[1])),(*reinterpret_cast< float(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])),(*reinterpret_cast< CPX*(*)>(_a[4])),(*reinterpret_cast< int(*)>(_a[5]))); break;
        case 3: _t->MakeFirBandpass((*reinterpret_cast< float(*)>(_a[1])),(*reinterpret_cast< float(*)>(_a[2])),(*reinterpret_cast< float(*)>(_a[3])),(*reinterpret_cast< int(*)>(_a[4])),(*reinterpret_cast< float*(*)>(_a[5])),(*reinterpret_cast< float*(*)>(_a[6])),(*reinterpret_cast< int(*)>(_a[7]))); break;
        case 4: _t->MakeFirBandpass((*reinterpret_cast< float(*)>(_a[1])),(*reinterpret_cast< float(*)>(_a[2])),(*reinterpret_cast< float(*)>(_a[3])),(*reinterpret_cast< int(*)>(_a[4])),(*reinterpret_cast< CPX*(*)>(_a[5])),(*reinterpret_cast< int(*)>(_a[6]))); break;
        case 5: _t->MakeFirBandstop((*reinterpret_cast< float(*)>(_a[1])),(*reinterpret_cast< float(*)>(_a[2])),(*reinterpret_cast< float(*)>(_a[3])),(*reinterpret_cast< CPX*(*)>(_a[4])),(*reinterpret_cast< int(*)>(_a[5]))); break;
        case 6: _t->DoOverlapAddCPX(); break;
        case 7: _t->setSampleRate((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 8: _t->MakeFilter((*reinterpret_cast< const float(*)>(_a[1])),(*reinterpret_cast< const float(*)>(_a[2])),(*reinterpret_cast< const int(*)>(_a[3])),(*reinterpret_cast< const int(*)>(_a[4]))); break;
        case 9: _t->MakeWindow((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< float*(*)>(_a[3]))); break;
        case 10: _t->ProcessFilter((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< CPX*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 11: _t->ProcessForwardFilter((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< CPX*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 12: _t->ProcessChirpFilter((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< CPX*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 13: { int _r = _t->ProcessAndDecimate((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< CPX*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])));
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 14: _t->Decimate((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< CPX*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 15: _t->DoConvolutionCPX(); break;
        case 16: _t->Normalize((*reinterpret_cast< CPX*(*)>(_a[1])),(*reinterpret_cast< CPX*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 17: { float _r = _t->filterLo();
            if (_a[0]) *reinterpret_cast< float*>(_a[0]) = _r; }  break;
        case 18: { float _r = _t->filterHi();
            if (_a[0]) *reinterpret_cast< float*>(_a[0]) = _r; }  break;
        case 19: { int _r = _t->isStreamMode();
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 20: _t->setFilterLo((*reinterpret_cast< const float(*)>(_a[1]))); break;
        case 21: _t->setFilterHi((*reinterpret_cast< const float(*)>(_a[1]))); break;
        case 22: _t->setFilter((*reinterpret_cast< const float(*)>(_a[1])),(*reinterpret_cast< const float(*)>(_a[2]))); break;
        case 23: _t->setStreamMode((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData QFilter::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject QFilter::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_QFilter,
      qt_meta_data_QFilter, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &QFilter::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *QFilter::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *QFilter::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_QFilter))
        return static_cast<void*>(const_cast< QFilter*>(this));
    return QObject::qt_metacast(_clname);
}

int QFilter::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 24)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 24;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
