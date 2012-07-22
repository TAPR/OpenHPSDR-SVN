/****************************************************************************
** Meta object code from reading C++ file 'cusdr_audio_spectrumanalyser.h'
**
** Created: Sun 22. Jul 16:54:13 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_audio_spectrumanalyser.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_audio_spectrumanalyser.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_SpectrumAnalyserThread[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       5,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       2,       // signalCount

 // signals: signature, parameters, type, tag, flags
      33,   24,   23,   23, 0x05,
      85,   72,   23,   23, 0x05,

 // slots: signature, parameters, type, tag, flags
     141,  136,   23,   23, 0x0a,
     212,  175,   23,   23, 0x0a,
     250,  175,   23,   23, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_SpectrumAnalyserThread[] = {
    "SpectrumAnalyserThread\0\0spectrum\0"
    "calculationComplete(FrequencySpectrum)\0"
    "spectrumList\0"
    "calculationTotalComplete(QList<FrequencySpectrum>)\0"
    "type\0setWindowFunction(WindowFunction)\0"
    "buffer,inputFrequency,bytesPerSample\0"
    "calculateSpectrum(QByteArray,int,int)\0"
    "calculateTotalSpectrum(QByteArray,int,int)\0"
};

void SpectrumAnalyserThread::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        SpectrumAnalyserThread *_t = static_cast<SpectrumAnalyserThread *>(_o);
        switch (_id) {
        case 0: _t->calculationComplete((*reinterpret_cast< const FrequencySpectrum(*)>(_a[1]))); break;
        case 1: _t->calculationTotalComplete((*reinterpret_cast< const QList<FrequencySpectrum>(*)>(_a[1]))); break;
        case 2: _t->setWindowFunction((*reinterpret_cast< WindowFunction(*)>(_a[1]))); break;
        case 3: _t->calculateSpectrum((*reinterpret_cast< const QByteArray(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        case 4: _t->calculateTotalSpectrum((*reinterpret_cast< const QByteArray(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< int(*)>(_a[3]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData SpectrumAnalyserThread::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject SpectrumAnalyserThread::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_SpectrumAnalyserThread,
      qt_meta_data_SpectrumAnalyserThread, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &SpectrumAnalyserThread::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *SpectrumAnalyserThread::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *SpectrumAnalyserThread::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_SpectrumAnalyserThread))
        return static_cast<void*>(const_cast< SpectrumAnalyserThread*>(this));
    return QObject::qt_metacast(_clname);
}

int SpectrumAnalyserThread::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 5)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 5;
    }
    return _id;
}

// SIGNAL 0
void SpectrumAnalyserThread::calculationComplete(const FrequencySpectrum & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void SpectrumAnalyserThread::calculationTotalComplete(const QList<FrequencySpectrum> & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}
static const uint qt_meta_data_SpectrumAnalyser[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       4,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       2,       // signalCount

 // signals: signature, parameters, type, tag, flags
      27,   18,   17,   17, 0x05,
      75,   62,   17,   17, 0x05,

 // slots: signature, parameters, type, tag, flags
     121,   18,   17,   17, 0x08,
     175,  160,   17,   17, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_SpectrumAnalyser[] = {
    "SpectrumAnalyser\0\0spectrum\0"
    "spectrumChanged(FrequencySpectrum)\0"
    "spectrumList\0spectrumListChanged(QList<FrequencySpectrum>)\0"
    "calculationComplete(FrequencySpectrum)\0"
    "m_spectrumList\0"
    "calculationTotalComplete(QList<FrequencySpectrum>)\0"
};

void SpectrumAnalyser::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        SpectrumAnalyser *_t = static_cast<SpectrumAnalyser *>(_o);
        switch (_id) {
        case 0: _t->spectrumChanged((*reinterpret_cast< const FrequencySpectrum(*)>(_a[1]))); break;
        case 1: _t->spectrumListChanged((*reinterpret_cast< const QList<FrequencySpectrum>(*)>(_a[1]))); break;
        case 2: _t->calculationComplete((*reinterpret_cast< const FrequencySpectrum(*)>(_a[1]))); break;
        case 3: _t->calculationTotalComplete((*reinterpret_cast< const QList<FrequencySpectrum>(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData SpectrumAnalyser::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject SpectrumAnalyser::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_SpectrumAnalyser,
      qt_meta_data_SpectrumAnalyser, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &SpectrumAnalyser::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *SpectrumAnalyser::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *SpectrumAnalyser::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_SpectrumAnalyser))
        return static_cast<void*>(const_cast< SpectrumAnalyser*>(this));
    return QObject::qt_metacast(_clname);
}

int SpectrumAnalyser::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 4)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 4;
    }
    return _id;
}

// SIGNAL 0
void SpectrumAnalyser::spectrumChanged(const FrequencySpectrum & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void SpectrumAnalyser::spectrumListChanged(const QList<FrequencySpectrum> & _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}
QT_END_MOC_NAMESPACE
