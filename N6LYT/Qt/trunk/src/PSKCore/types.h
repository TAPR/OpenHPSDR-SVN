#ifndef TYPES_H
#define TYPES_H

//#include <complex.h>
#include <QMutex>
#include <QString>

#define COREDLL_API
#define __stdcall

#define INT qint32
#define UINT quint32
#define BOOL bool
#define LONG quint32
#define DWORD quint32
#define PDWORD DWORD*
#define BYTE unsigned char
#define WORD quint16
#define SHORT qint16
#define PCHAR char*
#define LPCTSTR char*
#define PINT int*
#define CString char*
#define LONGLONG qint64
#define LARGE_INTEGER qint64

#define LPVOID void*

//#define HANDLE long

#define CRITICAL_SECTION QMutex
#define EnterCriticalSection(x) x.lock()
#define LeaveCriticalSection(x) x.unlock()
#define InitializeCriticalSection(x)
#define DeleteCriticalSection(x)

//#define HWND int

#define _T(x) x

typedef struct {
    double x;
    double y;
} _complex;

//#define FALSE 0
//#define TRUE 1

//#define NULL 0

#define INVALID_HANDLE_VALUE -1

#endif // TYPES_H
