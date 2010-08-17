/****************************************************************************
** Meta object code from reading C++ file 'UI.h'
**
** Created: Tue Aug 17 20:23:54 2010
**      by: The Qt Meta Object Compiler version 62 (Qt 4.6.2)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "UI.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'UI.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 62
#error "This file was generated using the moc from 4.6.2. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_UI[] = {

 // content:
       4,       // revision
       0,       // classname
       0,    0, // classinfo
      54,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
       4,    3,    3,    3, 0x0a,
      22,    3,    3,    3, 0x0a,
      38,    3,    3,    3, 0x0a,
      50,    3,    3,    3, 0x0a,
      61,    3,    3,    3, 0x0a,
      72,    3,    3,    3, 0x0a,
      83,    3,    3,    3, 0x0a,
      94,    3,    3,    3, 0x0a,
     105,    3,    3,    3, 0x0a,
     116,    3,    3,    3, 0x0a,
     127,    3,    3,    3, 0x0a,
     138,    3,    3,    3, 0x0a,
     149,    3,    3,    3, 0x0a,
     159,    3,    3,    3, 0x0a,
     171,    3,    3,    3, 0x0a,
     183,    3,    3,    3, 0x0a,
     195,    3,    3,    3, 0x0a,
     207,    3,    3,    3, 0x0a,
     219,    3,    3,    3, 0x0a,
     231,    3,    3,    3, 0x0a,
     243,    3,    3,    3, 0x0a,
     254,    3,    3,    3, 0x0a,
     266,    3,    3,    3, 0x0a,
     278,    3,    3,    3, 0x0a,
     291,    3,    3,    3, 0x0a,
     304,    3,    3,    3, 0x0a,
     320,    3,    3,    3, 0x0a,
     336,    3,    3,    3, 0x0a,
     352,    3,    3,    3, 0x0a,
     368,    3,    3,    3, 0x0a,
     384,    3,    3,    3, 0x0a,
     400,    3,    3,    3, 0x0a,
     416,    3,    3,    3, 0x0a,
     432,    3,    3,    3, 0x0a,
     448,    3,    3,    3, 0x0a,
     464,    3,    3,    3, 0x0a,
     484,  476,    3,    3, 0x0a,
     513,  506,    3,    3, 0x0a,
     556,  535,    3,    3, 0x0a,
     598,  577,    3,    3, 0x0a,
     646,  619,    3,    3, 0x0a,
     713,  688,    3,    3, 0x0a,
     746,  736,    3,    3, 0x0a,
     774,    3,    3,    3, 0x0a,
     798,  791,    3,    3, 0x0a,
     821,  816,    3,    3, 0x0a,
     834,    3,    3,    3, 0x0a,
     848,  816,    3,    3, 0x0a,
     876,  866,    3,    3, 0x0a,
     901,  896,    3,    3, 0x0a,
     930,  926,    3,    3, 0x0a,
     956,  954,    3,    3, 0x0a,
     972,  896,    3,    3, 0x0a,
     998,  926,    3,    3, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_UI[] = {
    "UI\0\0actionConfigure()\0actionConnect()\0"
    "action160()\0action80()\0action60()\0"
    "action40()\0action30()\0action20()\0"
    "action17()\0action15()\0action12()\0"
    "action10()\0action6()\0actionGen()\0"
    "actionWWV()\0actionCWL()\0actionCWU()\0"
    "actionLSB()\0actionUSB()\0actionDSB()\0"
    "actionAM()\0actionSAM()\0actionFMN()\0"
    "actionDIGL()\0actionDIGU()\0actionFilter0()\0"
    "actionFilter1()\0actionFilter2()\0"
    "actionFilter3()\0actionFilter4()\0"
    "actionFilter5()\0actionFilter6()\0"
    "actionFilter7()\0actionFilter8()\0"
    "actionFilter9()\0connected()\0message\0"
    "disconnected(QString)\0header\0"
    "receivedHeader(char*)\0previousBand,newBand\0"
    "bandChanged(int,int)\0previousMode,newMode\0"
    "modeChanged(int,int)\0previousFilters,newFilters\0"
    "filtersChanged(FiltersBase*,FiltersBase*)\0"
    "previousFilter,newFilter\0"
    "filterChanged(int,int)\0frequency\0"
    "frequencyChanged(long long)\0"
    "updateSpectrum()\0choice\0audioChanged(int)\0"
    "gain\0setGain(int)\0actionSubRx()\0"
    "setSubRxGain(int)\0increment\0"
    "frequencyMoved(int)\0high\0"
    "spectrumHighChanged(int)\0low\0"
    "spectrumLowChanged(int)\0f\0fpsChanged(int)\0"
    "waterfallHighChanged(int)\0"
    "waterfallLowChanged(int)\0"
};

const QMetaObject UI::staticMetaObject = {
    { &QMainWindow::staticMetaObject, qt_meta_stringdata_UI,
      qt_meta_data_UI, 0 }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &UI::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *UI::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *UI::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_UI))
        return static_cast<void*>(const_cast< UI*>(this));
    return QMainWindow::qt_metacast(_clname);
}

int UI::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QMainWindow::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: actionConfigure(); break;
        case 1: actionConnect(); break;
        case 2: action160(); break;
        case 3: action80(); break;
        case 4: action60(); break;
        case 5: action40(); break;
        case 6: action30(); break;
        case 7: action20(); break;
        case 8: action17(); break;
        case 9: action15(); break;
        case 10: action12(); break;
        case 11: action10(); break;
        case 12: action6(); break;
        case 13: actionGen(); break;
        case 14: actionWWV(); break;
        case 15: actionCWL(); break;
        case 16: actionCWU(); break;
        case 17: actionLSB(); break;
        case 18: actionUSB(); break;
        case 19: actionDSB(); break;
        case 20: actionAM(); break;
        case 21: actionSAM(); break;
        case 22: actionFMN(); break;
        case 23: actionDIGL(); break;
        case 24: actionDIGU(); break;
        case 25: actionFilter0(); break;
        case 26: actionFilter1(); break;
        case 27: actionFilter2(); break;
        case 28: actionFilter3(); break;
        case 29: actionFilter4(); break;
        case 30: actionFilter5(); break;
        case 31: actionFilter6(); break;
        case 32: actionFilter7(); break;
        case 33: actionFilter8(); break;
        case 34: actionFilter9(); break;
        case 35: connected(); break;
        case 36: disconnected((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 37: receivedHeader((*reinterpret_cast< char*(*)>(_a[1]))); break;
        case 38: bandChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 39: modeChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 40: filtersChanged((*reinterpret_cast< FiltersBase*(*)>(_a[1])),(*reinterpret_cast< FiltersBase*(*)>(_a[2]))); break;
        case 41: filterChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 42: frequencyChanged((*reinterpret_cast< long long(*)>(_a[1]))); break;
        case 43: updateSpectrum(); break;
        case 44: audioChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 45: setGain((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 46: actionSubRx(); break;
        case 47: setSubRxGain((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 48: frequencyMoved((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 49: spectrumHighChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 50: spectrumLowChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 51: fpsChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 52: waterfallHighChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 53: waterfallLowChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 54;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
