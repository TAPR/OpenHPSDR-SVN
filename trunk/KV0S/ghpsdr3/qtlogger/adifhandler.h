#ifndef ADIFHANDLER_H
#define ADIFHANDLER_H

#include <QXmlDefaultHandler>
#include <QHash>

#include "range.h"


class AdifHandler : public QXmlDefaultHandler
{

public:
    explicit AdifHandler();
    bool startDocument();
    bool startElement(const QString &namespaceURI, const QString &localName, const QString &qName,  const QXmlAttributes &attrs);
    bool endElement(const QString &namespaceURI, const QString &localName, const QString &qName);
    bool fatalError (const QXmlParseException & exception);
    bool characters(const QString &ch);
    QString abbr;
    QString atName;
    QString dataString;
    QStringList mode;
    QStringList country;
    QHash <QString, QString> ARRLSections;
    QStringList subdivisions;
    QHash <QString, Range> band;


signals:

public slots:

private:
    bool inchoice;
    bool initem;
    bool inlabel;
    bool invalue;
    bool inlower;
    bool inupper;
    QString ename;
    QString iname;
    QString label;
    QString value;
    QString upper;
    QString lower;
    QHash <QString,QString> attr;
};

#endif // ADIFHANDLER_H
