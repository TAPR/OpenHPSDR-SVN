/**********************************************************
 * datatypes.h: Common data type declarations
 *
 * History:
 *      2010-09-15  Initial creation MSW
 *      2011-03-27  Initial release
 *********************************************************/
#ifndef DATATYPES_H
#define DATATYPES_H

#include <math.h>
#include <QtGlobal>


//define single or double precision reals and complex types
typedef float tSReal;
typedef double tDReal;

typedef struct _sCplx
{
	tSReal re;
	tSReal im;
} tSComplex;

typedef struct _dCplx
{
	tDReal re;
	tDReal im;
} tDComplex;

typedef struct _isCplx
{
	qint16 re;
	qint16 im;
} tStereo16;

#define TYPEREAL tDReal
#define TYPECPX	tDComplex
#define TYPESTEREO16 tStereo16
#define TYPEMONO16 qint16

#endif // DATATYPES_H
