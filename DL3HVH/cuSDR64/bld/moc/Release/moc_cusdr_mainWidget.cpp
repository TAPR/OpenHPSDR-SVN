/****************************************************************************
** Meta object code from reading C++ file 'cusdr_mainWidget.h'
**
** Created: Mon 16. Jul 22:21:51 2012
**      by: The Qt Meta Object Compiler version 63 (Qt 4.8.0)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../../../src/cusdr_mainWidget.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'cusdr_mainWidget.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 63
#error "This file was generated using the moc from 4.8.0. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_MainWindow[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
      45,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       1,       // signalCount

 // signals: signature, parameters, type, tag, flags
      20,   12,   11,   11, 0x05,

 // slots: signature, parameters, type, tag, flags
      42,   11,   11,   11, 0x0a,
      64,   51,   11,   11, 0x0a,
      99,   11,   11,   11, 0x0a,
     125,   11,   11,   11, 0x0a,
     151,   11,   11,   11, 0x0a,
     175,   11,   11,   11, 0x0a,
     201,   11,   11,   11, 0x0a,
     222,   11,   11,   11, 0x0a,
     244,   11,   11,   11, 0x0a,
     270,   11,   11,   11, 0x0a,
     292,  285,   11,   11, 0x0a,
     318,  285,   11,   11, 0x0a,
     361,  345,   11,   11, 0x0a,
     391,  285,   11,   11, 0x0a,
     415,  285,   11,   11, 0x0a,
     445,  285,   11,   11, 0x0a,
     509,  480,   11,   11, 0x08,
     615,   11,   11,   11, 0x08,
     644,   11,   11,   11, 0x08,
     662,   11,   11,   11, 0x08,
     683,   11,   11,   11, 0x08,
     707,   11,   11,   11, 0x08,
     721,   11,   11,   11, 0x08,
     738,   12,   11,   11, 0x08,
     759,   11,   11,   11, 0x08,
     775,   11,   11,   11, 0x08,
     792,  787,   11,   11, 0x08,
     825,   11,   11,   11, 0x08,
     842,  839,   11,   11, 0x08,
     872,  859,   11,   11, 0x08,
     907,   11,   11,   11, 0x08,
     928,   11,   11,   11, 0x08,
     943,   11,   11,   11, 0x08,
     958,   11,   11,   11, 0x08,
     979,   11,   11,   11, 0x08,
     998,   11,   11,   11, 0x08,
    1015,   11,   11,   11, 0x08,
    1039,   11,   11,   11, 0x08,
    1068, 1062,   11,   11, 0x08,
    1087, 1062,   11,   11, 0x08,
    1103,   11,   11,   11, 0x08,
    1122,   11,   11,   11, 0x08,
    1148, 1144,   11,   11, 0x08,
    1183,   11,   11,   11, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_MainWindow[] = {
    "MainWindow\0\0message\0messageEvent(QString)\0"
    "update()\0sender,power\0"
    "masterSwitchChanged(QObject*,bool)\0"
    "startButtonClickedEvent()\0"
    "messagesBtnClickedEvent()\0"
    "widgetBtnClickedEvent()\0"
    "wideBandBtnClickedEvent()\0"
    "avgBtnClickedEvent()\0gridBtnClickedEvent()\0"
    "peakHoldBtnClickedEvent()\0resizeWidget()\0"
    "sender\0showWidgetEvent(QObject*)\0"
    "closeWidgetEvent(QObject*)\0sender,fileName\0"
    "loadWavFile(QObject*,QString)\0"
    "suspendSignal(QObject*)\0"
    "startPlaybackSignal(QObject*)\0"
    "showSettingsDialogSignal(QObject*)\0"
    "sender,err,hwmode,mode,state\0"
    "systemStateChanged(QObject*,QSDR::_Error,QSDR::_HWInterfaceMode,QSDR::"
    "_ServerMode,QSDR::_DataEngineState)\0"
    "ctrlDisplayBtnClickedEvent()\0"
    "closeMainWindow()\0maximizeMainWindow()\0"
    "setMainWindowGeometry()\0updateTitle()\0"
    "initialMessage()\0showMessage(QString)\0"
    "setFullScreen()\0getRegion()\0mode\0"
    "setServerMode(QSDR::_ServerMode)\0"
    "setReceiver()\0rx\0setReceiver(int)\0"
    "sender,value\0setNumberOfReceivers(QObject*,int)\0"
    "setInternalDSPMode()\0setDttSPMode()\0"
    "setQtDSPMode()\0setExternalDSPMode()\0"
    "setChirpWSPRMode()\0setCudaDSPMode()\0"
    "setPeakHoldStatus(bool)\0getNetworkInterfaces()\0"
    "value\0setMainVolume(int)\0setAGCGain(int)\0"
    "getLastFrequency()\0showNetworkIODialog()\0"
    "str\0addNetworkIOComboBoxEntry(QString)\0"
    "clearNetworkIOComboBoxEntry()\0"
};

void MainWindow::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        MainWindow *_t = static_cast<MainWindow *>(_o);
        switch (_id) {
        case 0: _t->messageEvent((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 1: _t->update(); break;
        case 2: _t->masterSwitchChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2]))); break;
        case 3: _t->startButtonClickedEvent(); break;
        case 4: _t->messagesBtnClickedEvent(); break;
        case 5: _t->widgetBtnClickedEvent(); break;
        case 6: _t->wideBandBtnClickedEvent(); break;
        case 7: _t->avgBtnClickedEvent(); break;
        case 8: _t->gridBtnClickedEvent(); break;
        case 9: _t->peakHoldBtnClickedEvent(); break;
        case 10: _t->resizeWidget(); break;
        case 11: _t->showWidgetEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 12: _t->closeWidgetEvent((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 13: _t->loadWavFile((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< const QString(*)>(_a[2]))); break;
        case 14: _t->suspendSignal((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 15: _t->startPlaybackSignal((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 16: _t->showSettingsDialogSignal((*reinterpret_cast< QObject*(*)>(_a[1]))); break;
        case 17: _t->systemStateChanged((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< QSDR::_Error(*)>(_a[2])),(*reinterpret_cast< QSDR::_HWInterfaceMode(*)>(_a[3])),(*reinterpret_cast< QSDR::_ServerMode(*)>(_a[4])),(*reinterpret_cast< QSDR::_DataEngineState(*)>(_a[5]))); break;
        case 18: _t->ctrlDisplayBtnClickedEvent(); break;
        case 19: _t->closeMainWindow(); break;
        case 20: _t->maximizeMainWindow(); break;
        case 21: _t->setMainWindowGeometry(); break;
        case 22: _t->updateTitle(); break;
        case 23: _t->initialMessage(); break;
        case 24: _t->showMessage((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 25: _t->setFullScreen(); break;
        case 26: _t->getRegion(); break;
        case 27: _t->setServerMode((*reinterpret_cast< QSDR::_ServerMode(*)>(_a[1]))); break;
        case 28: _t->setReceiver(); break;
        case 29: _t->setReceiver((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 30: _t->setNumberOfReceivers((*reinterpret_cast< QObject*(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 31: _t->setInternalDSPMode(); break;
        case 32: _t->setDttSPMode(); break;
        case 33: _t->setQtDSPMode(); break;
        case 34: _t->setExternalDSPMode(); break;
        case 35: _t->setChirpWSPRMode(); break;
        case 36: _t->setCudaDSPMode(); break;
        case 37: _t->setPeakHoldStatus((*reinterpret_cast< bool(*)>(_a[1]))); break;
        case 38: _t->getNetworkInterfaces(); break;
        case 39: _t->setMainVolume((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 40: _t->setAGCGain((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 41: _t->getLastFrequency(); break;
        case 42: _t->showNetworkIODialog(); break;
        case 43: _t->addNetworkIOComboBoxEntry((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 44: _t->clearNetworkIOComboBoxEntry(); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData MainWindow::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject MainWindow::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_MainWindow,
      qt_meta_data_MainWindow, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &MainWindow::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *MainWindow::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *MainWindow::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_MainWindow))
        return static_cast<void*>(const_cast< MainWindow*>(this));
    return QWidget::qt_metacast(_clname);
}

int MainWindow::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 45)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 45;
    }
    return _id;
}

// SIGNAL 0
void MainWindow::messageEvent(QString _t1)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
static const uint qt_meta_data_NetworkIODialog[] = {

 // content:
       6,       // revision
       0,       // classname
       0,    0, // classinfo
       3,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
      21,   17,   16,   16, 0x0a,
      52,   16,   16,   16, 0x0a,
      78,   16,   16,   16, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_NetworkIODialog[] = {
    "NetworkIODialog\0\0str\0"
    "addDeviceComboBoxItem(QString)\0"
    "clearDeviceComboBoxItem()\0okBtnClicked()\0"
};

void NetworkIODialog::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        Q_ASSERT(staticMetaObject.cast(_o));
        NetworkIODialog *_t = static_cast<NetworkIODialog *>(_o);
        switch (_id) {
        case 0: _t->addDeviceComboBoxItem((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 1: _t->clearDeviceComboBoxItem(); break;
        case 2: _t->okBtnClicked(); break;
        default: ;
        }
    }
}

const QMetaObjectExtraData NetworkIODialog::staticMetaObjectExtraData = {
    0,  qt_static_metacall 
};

const QMetaObject NetworkIODialog::staticMetaObject = {
    { &QDialog::staticMetaObject, qt_meta_stringdata_NetworkIODialog,
      qt_meta_data_NetworkIODialog, &staticMetaObjectExtraData }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &NetworkIODialog::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *NetworkIODialog::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *NetworkIODialog::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_NetworkIODialog))
        return static_cast<void*>(const_cast< NetworkIODialog*>(this));
    return QDialog::qt_metacast(_clname);
}

int NetworkIODialog::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QDialog::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 3)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 3;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
