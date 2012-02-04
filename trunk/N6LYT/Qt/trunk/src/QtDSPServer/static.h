/**
  @file
  @author Stefan Frings
*/

#ifndef STATIC_H
#define STATIC_H

#include <QString>

/**
  This class contains some static resources that are used by the application.
*/

class Static
{
public:

    static QString getConfigFileName();

    static QString getConfigDir();

private:

    /** Directory of the main config file */
    static QString configDir;

};

#endif // STATIC_H
