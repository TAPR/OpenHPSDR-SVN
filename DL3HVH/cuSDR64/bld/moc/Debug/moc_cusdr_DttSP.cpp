/****************************************************************************
** Meta object code from reading C++ file 'cusdr_DttSP.h'
**
** Created: Wed 4. Jul 21:06:09 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_DttSP.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_DttSP.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_QDttSP[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      31,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       1,       // signalCount

 // signals: signature, parameters, type, tag, flags
      16,    8,    7,    7, 0x05,

 // slots: signature, parameters, type, tag, flags
      42,   38,    7,    7, 0x08,
      68,   62,    7,    7, 0x0a,
      94,    7,   89,    7, 0x0a,
     111,    7,    7,    7, 0x0a,
     127,    7,    7,    7, 0x0a,
     148,    7,    7,    7, 0x0a,
     173,  166,    7,    7, 0x0a,
     210,  196,    7,    7, 0x0a,
     252,  233,    7,    7, 0x0a,
     303,  284,    7,    7, 0x0a,
     346,  331,    7,    7, 0x0a,
     400,  389,  385,    7, 0x0a,
     445,  427,  385,    7, 0x0a,
     495,  477,  385,    7, 0x0a,
     549,  534,  385,    7, 0x0a,
     603,  581,  385,    7, 0x0a,
     659,  645,  385,    7, 0x0a,
     694,  233,    7,    7, 0x0a,
     748,  727,    7,    7, 0x0a,
     806,  783,    7,    7, 0x0a,
     890,  841,    7,    7, 0x0a,
     968,  947,    7,    7, 0x0a,
    1025, 1010,    7,    7, 0x0a,
    1058, 1010,    7,    7, 0x0a,
    1118, 1093,    7,    7, 0x0a,
    1152, 1093,    7,    7, 0x0a,
    1199, 1186,    7,    7, 0x0a,
    1248, 1236,    7,    7, 0x0a,
    1292, 1280,    7,    7, 0x0a,
    1355, 1339, 1333,    7, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_QDttSP[] = {
    "QDttSP\0\0message\0messageEvent(QString)\0"
    "msg\0newMessage(QString)\0value\0"
    "setDttSPStatus(bool)\0bool\0getDttSPStatus()\0"
    "dttspSetupSDR()\0dttspReleaseUpdate()\0"
    "dttspDestroySDR()\0thread\0"
    "dttspSetThreadCom(int)\0thread,trx_on\0"
    "dttspSetTRX(uint,bool)\0thread,subrx,setit\0"
    "dttspSetSubRXSt(uint,uint,bool)\0"
    "thread,newBuffSize\0dttspSetDSPBuflen(uint,int)\0"
    "thread,runmode\0dttspSetThreadProcessingMode(uint,int)\0"
    "int\0sampleRate\0dttspSetSampleRate(double)\0"
    "thread,subrx,freq\0dttspSetRXOsc(uint,uint,double)\0"
    "thread,subrx,gain\0"
    "dttspSetRXOutputGain(uint,uint,double)\0"
    "thread,subrx,m\0dttspSetMode(uint,uint,DSPMode)\0"
    "thread,subrx,low,high\0"
    "dttspSetRXFilter(uint,uint,double,double)\0"
    "thread,offset\0dttspSetRingBufferOffset(uint,int)\0"
    "dttspSetRXAGC(uint,uint,AGCMode)\0"
    "thread,subrx,max_agc\0"
    "dttspSetRXAGCTop(uint,uint,double)\0"
    "thread,subrx,fixed_agc\0"
    "dttspSetFixedAGC(uint,uint,double)\0"
    "input_l,input_r,output_l,output_r,nframes,thread\0"
    "dttspAudioCallback(float*,float*,float*,float*,uint,int)\0"
    "input,output,nframes\0"
    "dttspAudioCallback2(float**,float**,uint)\0"
    "thread,results\0dttspProcessSpectrum(int,float*)\0"
    "dttspProcessPanadapter(int,float*)\0"
    "thread,results,numpoints\0"
    "dttspProcessPhase(int,float*,int)\0"
    "dttspProcessScope(int,float*,int)\0"
    "thread,setit\0dttspSetSpectrumPolyphase(uint,bool)\0"
    "proc_thread\0dttspProcessSamplesThread(uint)\0"
    "thread,type\0dttspSetSpectrumWindow(uint,TWindowtype)\0"
    "float\0thread,subrx,mt\0"
    "dttspCalculateRXMeter(int,uint,int)\0"
};

void QDttSP::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        QDttSP *_t = static_cast<QDttSP *>(_o);
        switch (_id) {
        case 0: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 1: _t->newMessage((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 2: _t->setDttSPStatus((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 3: { bool _r = _t->getDttSPStatus();
            if (_a[0]) *reinterpret_cast< bool*>(_a[0]) = _r; }  break;
        case 4: _t->dttspSetupSDR(); break;
        case 5: _t->dttspReleaseUpdate(); break;
        case 6: _t->dttspDestroySDR(); break;
        case 7: _t->dttspSetThreadCom((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 8: _t->dttspSetTRX((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2]))); break;
        case 9: _t->dttspSetSubRXSt((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< bool(*)>(_a[3]))); break;
        case 10: _t->dttspSetDSPBuflen((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 11: _t->dttspSetThreadProcessingMode((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 12: { int _r = _t->dttspSetSampleRate((*reinterpret_cast< double(*)>(_a[1])));
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 13: { int _r = _t->dttspSetRXOsc((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< double(*)>(_a[3])));
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 14: { int _r = _t->dttspSetRXOutputGain((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< double(*)>(_a[3])));
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 15: { int _r = _t->dttspSetMode((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< DSPMode(*)>(_a[3])));
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 16: { int _r = _t->dttspSetRXFilter((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< double(*)>(_a[3])),(*reinterpret_cast< double(*)>(_a[4])));
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 17: { int _r = _t->dttspSetRingBufferOffset((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])));
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 18: _t->dttspSetRXAGC((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< AGCMode(*)>(_a[3]))); break;
        case 19: _t->dttspSetRXAGCTop((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< double(*)>(_a[3]))); break;
        case 20: _t->dttspSetFixedAGC((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< double(*)>(_a[3]))); break;
        case 21: _t->dttspAudioCallback((*reinterpret_cast< float*(*)>(_a[1])),(*reinterpret_cast< float*(*)>(_a[2])),(*reinterpret_cast< float*(*)>(_a[3])),(*reinterpret_cast< float*(*)>(_a[4])),(*reinterpret_cast< uint(*)>(_a[5])),(*reinterpret_cast< int(*)>(_a[6]))); break;
        case 22: _t->dttspAudioCallback2((*reinterpret_cast< float**(*)>(_a[1])),(*reinterpret_cast< float**(*)>(_a[2])),(*reinterpret_cast< uint(*)>(_a[3]))); break;
        case 23: _t->dttspProcessSpectrum((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< float*(*)>(_a[2]))); break;
        case 24: _t->dttspProcessPanadapter((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< float*(*)>(_a[2]))); break;
        case 25: _t->dttspProcessPhase((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< float*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 26: _t->dttspProcessScope((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< float*(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 27: _t->dttspSetSpectrumPolyphase((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2]))); break;
        case 28: _t->dttspProcessSamplesThread((*reinterpret_cast< uint(*)>(_a[1]))); break;
        case 29: _t->dttspSetSpectrumWindow((*reinterpret_cast< uint(*)>(_a[1])),(*reinterpret_cast< TWindowtype(*)>(_a[2]))); break;
        case 30: { float _r = _t->dttspCalculateRXMeter((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< uint(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3])));
            if (_a[0]) *reinterpret_cast< float*>(_a[0]) = _r; }  break;
        default: ;
        }
    }
}

const QMetaObjectExtraData QDttSP::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject QDttSP::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_QDttSP,
      qt_meta_data_QDttSP, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &QDttSP::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *QDttSP::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *QDttSP::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_QDttSP))
        return static_cast<void*>(const_cast< QDttSP*>(this));
    return QObject::qt_metacast(_clname);
}

int QDttSP::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 31)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 31;
    }
    return _id;
}

// SIGNAL 0
void QDttSP::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
