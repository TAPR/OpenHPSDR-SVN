/****************************************************************************
** Meta object code from reading C++ file 'UI.h'
**
** Created: Mon Aug 16 18:19:46 2010
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
      48,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
       4,    3,    3,    3, 0x0a,
      20,    3,    3,    3, 0x0a,
      32,    3,    3,    3, 0x0a,
      43,    3,    3,    3, 0x0a,
      54,    3,    3,    3, 0x0a,
      65,    3,    3,    3, 0x0a,
      76,    3,    3,    3, 0x0a,
      87,    3,    3,    3, 0x0a,
      98,    3,    3,    3, 0x0a,
     109,    3,    3,    3, 0x0a,
     120,    3,    3,    3, 0x0a,
     131,    3,    3,    3, 0x0a,
     141,    3,    3,    3, 0x0a,
     153,    3,    3,    3, 0x0a,
     165,    3,    3,    3, 0x0a,
     177,    3,    3,    3, 0x0a,
     189,    3,    3,    3, 0x0a,
     201,    3,    3,    3, 0x0a,
     213,    3,    3,    3, 0x0a,
     225,    3,    3,    3, 0x0a,
     236,    3,    3,    3, 0x0a,
     248,    3,    3,    3, 0x0a,
     260,    3,    3,    3, 0x0a,
     273,    3,    3,    3, 0x0a,
     286,    3,    3,    3, 0x0a,
     302,    3,    3,    3, 0x0a,
     318,    3,    3,    3, 0x0a,
     334,    3,    3,    3, 0x0a,
     350,    3,    3,    3, 0x0a,
     366,    3,    3,    3, 0x0a,
     382,    3,    3,    3, 0x0a,
     398,    3,    3,    3, 0x0a,
     414,    3,    3,    3, 0x0a,
     430,    3,    3,    3, 0x0a,
     446,    3,    3,    3, 0x0a,
     466,  458,    3,    3, 0x0a,
     495,  488,    3,    3, 0x0a,
     538,  517,    3,    3, 0x0a,
     580,  559,    3,    3, 0x0a,
     628,  601,    3,    3, 0x0a,
     695,  670,    3,    3, 0x0a,
     728,  718,    3,    3, 0x0a,
     756,    3,    3,    3, 0x0a,
     772,  765,    3,    3, 0x0a,
     795,  790,    3,    3, 0x0a,
     808,    3,    3,    3, 0x0a,
     822,  790,    3,    3, 0x0a,
     850,  840,    3,    3, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_UI[] = {
    "UI\0\0actionConnect()\0action160()\0"
    "action80()\0action60()\0action40()\0"
    "action30()\0action20()\0action17()\0"
    "action15()\0action12()\0action10()\0"
    "action6()\0actionGen()\0actionWWV()\0"
    "actionCWL()\0actionCWU()\0actionLSB()\0"
    "actionUSB()\0actionDSB()\0actionAM()\0"
    "actionSAM()\0actionFMN()\0actionDIGL()\0"
    "actionDIGU()\0actionFilter0()\0"
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
    "frequencyChanged(long long)\0update()\0"
    "choice\0audioChanged(int)\0gain\0"
    "setGain(int)\0actionSubRx()\0setSubRxGain(int)\0"
    "increment\0frequencyMoved(int)\0"
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
        case 0: actionConnect(); break;
        case 1: action160(); break;
        case 2: action80(); break;
        case 3: action60(); break;
        case 4: action40(); break;
        case 5: action30(); break;
        case 6: action20(); break;
        case 7: action17(); break;
        case 8: action15(); break;
        case 9: action12(); break;
        case 10: action10(); break;
        case 11: action6(); break;
        case 12: actionGen(); break;
        case 13: actionWWV(); break;
        case 14: actionCWL(); break;
        case 15: actionCWU(); break;
        case 16: actionLSB(); break;
        case 17: actionUSB(); break;
        case 18: actionDSB(); break;
        case 19: actionAM(); break;
        case 20: actionSAM(); break;
        case 21: actionFMN(); break;
        case 22: actionDIGL(); break;
        case 23: actionDIGU(); break;
        case 24: actionFilter0(); break;
        case 25: actionFilter1(); break;
        case 26: actionFilter2(); break;
        case 27: actionFilter3(); break;
        case 28: actionFilter4(); break;
        case 29: actionFilter5(); break;
        case 30: actionFilter6(); break;
        case 31: actionFilter7(); break;
        case 32: actionFilter8(); break;
        case 33: actionFilter9(); break;
        case 34: connected(); break;
        case 35: disconnected((*reinterpret_cast< QString(*)>(_a[1]))); break;
        case 36: receivedHeader((*reinterpret_cast< char*(*)>(_a[1]))); break;
        case 37: bandChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 38: modeChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 39: filtersChanged((*reinterpret_cast< FiltersBase*(*)>(_a[1])),(*reinterpret_cast< FiltersBase*(*)>(_a[2]))); break;
        case 40: filterChanged((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< int(*)>(_a[2]))); break;
        case 41: frequencyChanged((*reinterpret_cast< long long(*)>(_a[1]))); break;
        case 42: update(); break;
        case 43: audioChanged((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 44: setGain((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 45: actionSubRx(); break;
        case 46: setSubRxGain((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 47: frequencyMoved((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 48;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
