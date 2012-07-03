/****************************************************************************
** Meta object code from reading C++ file 'cusdr_audioReceiver.h'
**
** Created: Tue 3. Jul 21:43:47 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../src/cusdr_audioReceiver.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_audioReceiver.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_AudioReceiver[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       9,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       6,       // signalCount

 // signals: signature, parameters, type, tag, flags
      23,   15,   14,   14, 0x05,
      58,   45,   14,   14, 0x05,
      94,   84,   14,   14, 0x05,
     134,  128,   14,   14, 0x05,
     161,   14,   14,   14, 0x05,
     171,   14,   14,   14, 0x05,

 // slots: signature, parameters, type, tag, flags
     186,   14,   14,   14, 0x0a,
     205,  199,   14,   14, 0x08,
     263,   14,   14,   14, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_AudioReceiver[] = {
    "AudioReceiver\0\0message\0messageEvent(QString)\0"
    "sender,value\0rcveIQEvent(QObject*,int)\0"
    "outbuffer\0outputBufferEvent(unsigned char*)\0"
    "value\0clientConnectedEvent(bool)\0"
    "newData()\0newAudioData()\0initClient()\0"
    "error\0displayAudioRcvrSocketError(QAbstractSocket::SocketError)\0"
    "readPendingAudioRcvrData()\0"
};

void AudioReceiver::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        AudioReceiver *_t = static_cast<AudioReceiver *>(_o);
        switch (_id) {
        case 0: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 1: _t->rcveIQEvent((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 2: _t->outputBufferEvent((*reinterpret_cast< unsigned char*(*)>(_a[1]))); break;
        case 3: _t->clientConnectedEvent((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 4: _t->newData(); break;
        case 5: _t->newAudioData(); break;
        case 6: _t->initClient(); break;
        case 7: _t->displayAudioRcvrSocketError((*reinterpret_cast< QAbstractSocket::SocketError(*)>(_a[1]))); break;
        case 8: _t->readPendingAudioRcvrData(); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData AudioReceiver::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject AudioReceiver::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_AudioReceiver,
      qt_meta_data_AudioReceiver, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &AudioReceiver::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *AudioReceiver::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *AudioReceiver::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_AudioReceiver))
        return static_cast<void*>(const_cast< AudioReceiver*>(this));
    return QObject::qt_metacast(_clname);
}

int AudioReceiver::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 9)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 9;
    }
    return _id;
}

// SIGNAL 0
void AudioReceiver::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}

// SIGNAL 1
void AudioReceiver::rcveIQEvent(QObject * _t1, int _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void AudioReceiver::outputBufferEvent(unsigned char * _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void AudioReceiver::clientConnectedEvent(bool _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}

// SIGNAL 4
void AudioReceiver::newData()
{
    QMetaObject::activate(this, &staticMetaObject, 4, 0);
}

// SIGNAL 5
void AudioReceiver::newAudioData()
{
    QMetaObject::activate(this, &staticMetaObject, 5, 0);
}
QT_END_MOC_NAMESPACE
