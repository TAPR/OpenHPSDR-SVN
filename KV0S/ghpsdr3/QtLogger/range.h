#ifndef RANGE_H
#define RANGE_H

#include <QString>

class Range
{
public:
    Range();
    Range( QString lower, QString upper );

    QString lower;
    QString upper;

};

#endif // RANGE_H
