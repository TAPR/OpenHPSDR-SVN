/****************************************************************************
** Meta object code from reading C++ file 'cusdr_server.h'
**
** Created: Sun 22. Jul 10:30:56 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_server.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_server.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_HPSDRServer[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      27,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
      10,       // signalCount

 // signals: signature, parameters, type, tag, flags
      13,   12,   12,   12, 0x05,
      41,   33,   12,   12, 0x05,
      71,   63,   12,   12, 0x05,
     139,   95,   12,   12, 0x05,
     196,  185,   12,   12, 0x05,
     237,  234,   12,   12, 0x05,
     270,  263,   12,   12, 0x05,
     318,  299,   12,   12, 0x05,
     355,  234,   12,   12, 0x05,
     375,  234,   12,   12, 0x05,

 // slots: signature, parameters, type, tag, flags
     404,   12,  399,   12, 0x0a,
     418,   12,   12,   12, 0x0a,
     431,   12,   12,   12, 0x0a,
     448,   12,  444,   12, 0x0a,
     496,  467,   12,   12, 0x08,
     611,  598,   12,   12, 0x08,
     653,  646,   12,   12, 0x08,
     690,   12,   12,   12, 0x08,
     709,   12,   12,   12, 0x08,
     731,   12,   12,   12, 0x08,
     752,   33,   12,   12, 0x08,
     772,   12,   12,   12, 0x08,
     798,   12,   12,   12, 0x08,
     828,  818,  812,   12, 0x08,
     852,  818,  812,   12, 0x08,
     891,  876,  812,   12, 0x08,
     932,  915,  812,   12, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_HPSDRServer[] = {
    "HPSDRServer\0\0masterSwitchEvent()\0"
    "message\0messageEvent(QString)\0rx,port\0"
    "setIQPortEvent(int,int)\0"
    "socketErrorCode,socketErrorString,addr,port\0"
    "socketErrorEvent(int,QString,QString,quint16)\0"
    "rx,address\0setPeerAddressEvent(int,QHostAddress)\0"
    "rx\0clientConnectedEvent(int)\0client\0"
    "clientDisconnectedEvent(int)\0"
    "value,rx,frequency\0"
    "frequencyChangedEvent(bool,int,long)\0"
    "newClientEvent(int)\0audioReceiverEvent(int)\0"
    "bool\0startServer()\0stopServer()\0"
    "serverStop()\0int\0getAudioReceiver()\0"
    "sender,err,hwmode,mode,state\0"
    "setSystemState(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::_Ser"
    "verMode,QSDR::_DataEngineState)\0"
    "sender,power\0masterSwitchChanged(QObject*,bool)\0"
    "rxList\0rxListChanged(QList<HPSDRReceiver*>)\0"
    "setupConnections()\0handleNewConnection()\0"
    "clientDisconnected()\0newMessage(QString)\0"
    "sendMessageToAllClients()\0readCommand()\0"
    "char*\0rx,client\0attachReceiver(int,int)\0"
    "detachReceiver(int,int)\0command,client\0"
    "parseCommand(char*,int)\0frequency,client\0"
    "setFrequency(long,int)\0"
};

void HPSDRServer::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        HPSDRServer *_t = static_cast<HPSDRServer *>(_o);
        switch (_id) {
        case 0: _t->masterSwitchEvent(); break;
        case 1: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 2: _t->setIQPortEvent((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 3: _t->socketErrorEvent((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< QString(*)>(_a[2])),(*reinterpret_cast< QString(*)>(_a[3])),(*reinterpret_cast< quint16(*)>(_a[4]))); break;
        case 4: _t->setPeerAddressEvent((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< QHostAddress(*)>(_a[2]))); break;
        case 5: _t->clientConnectedEvent((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 6: _t->clientDisconnectedEvent((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 7: _t->frequencyChangedEvent((*reinterpret_cast< bool(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])),(*reinterpret_cast< long(*)>(_a[3]))); break;
        case 8: _t->newClientEvent((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 9: _t->audioReceiverEvent((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 10: { bool _r = _t->startServer();
            if (_a[0]) *reinterpret_cast< bool*>(_a[0]) = _r; }  break;
        case 11: _t->stopServer(); break;
        case 12: _t->serverStop(); break;
        case 13: { int _r = _t->getAudioReceiver();
            if (_a[0]) *reinterpret_cast< int*>(_a[0]) = _r; }  break;
        case 14: _t->setSystemState((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 15: _t->masterSwitchChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2]))); break;
        case 16: _t->rxListChanged((*reinterpret_cast< QList<HPSDRReceiver*>(*)>(_a[1]))); break;
        case 17: _t->setupConnections(); break;
        case 18: _t->handleNewConnection(); break;
        case 19: _t->clientDisconnected(); break;
        case 20: _t->newMessage((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 21: _t->sendMessageToAllClients(); break;
        case 22: _t->readCommand(); break;
        case 23: { char* _r = _t->attachReceiver((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])));
            if (_a[0]) *reinterpret_cast< char**>(_a[0]) = _r; }  break;
        case 24: { char* _r = _t->detachReceiver((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])));
            if (_a[0]) *reinterpret_cast< char**>(_a[0]) = _r; }  break;
        case 25: { char* _r = _t->parseCommand((*reinterpret_cast< char*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])));
            if (_a[0]) *reinterpret_cast< char**>(_a[0]) = _r; }  break;
        case 26: { char* _r = _t->setFrequency((*reinterpret_cast< long(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2])));
            if (_a[0]) *reinterpret_cast< char**>(_a[0]) = _r; }  break;
        default: ;
        }
    }
}

const QMetaObjectExtraData HPSDRServer::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject HPSDRServer::staticMetaObject = {
    { &QTcpServer::staticMetaObject, qt_meta_stringdata_HPSDRServer,
      qt_meta_data_HPSDRServer, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &HPSDRServer::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *HPSDRServer::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *HPSDRServer::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_HPSDRServer))
        return static_cast<void*>(const_cast< HPSDRServer*>(this));
    return QTcpServer::qt_metacast(_clname);
}

int HPSDRServer::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QTcpServer::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 27)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 27;
    }
    return _id;
}

// SIGNAL 0
void HPSDRServer::masterSwitchEvent()
{
    QMetaObject::activate(this, &staticMetaObject, 0, 0);
}

// SIGNAL 1
void HPSDRServer::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 1, _a);
}

// SIGNAL 2
void HPSDRServer::setIQPortEvent(int _t1, int _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 2, _a);
}

// SIGNAL 3
void HPSDRServer::socketErrorEvent(int _t1, QString _t2, QString _t3, quint16 _t4)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)), const_cast<void*>(reinterpret_cast<const void*>(&_t3)), const_cast<void*>(reinterpret_cast<const void*>(&_t4)) };
    QMetaObject::activate(this, &staticMetaObject, 3, _a);
}

// SIGNAL 4
void HPSDRServer::setPeerAddressEvent(int _t1, QHostAddress _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 4, _a);
}

// SIGNAL 5
void HPSDRServer::clientConnectedEvent(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 5, _a);
}

// SIGNAL 6
void HPSDRServer::clientDisconnectedEvent(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 6, _a);
}

// SIGNAL 7
void HPSDRServer::frequencyChangedEvent(bool _t1, int _t2, long _t3)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)), const_cast<void*>(reinterpret_cast<const void*>(&_t3)) };
    QMetaObject::activate(this, &staticMetaObject, 7, _a);
}

// SIGNAL 8
void HPSDRServer::newClientEvent(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 8, _a);
}

// SIGNAL 9
void HPSDRServer::audioReceiverEvent(int _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 9, _a);
}
QT_END_MOC_NAMESPACE
