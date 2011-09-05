#ifndef LOGHANDLER_H
#define LOGHANDLER_H

#include <QXmlDefaultHandler>
#include <QStandardItem>
#include <QStandardItemModel>
#include <QList>


class logHandler : public QXmlDefaultHandler
{

public:
    explicit logHandler();
    explicit logHandler( QStandardItemModel *model, QStringList *hdr );
    bool startDocument();
    bool startElement(const QString &namespaceURI, const QString &localName, const QString &qName,  const QXmlAttributes &attrs);
    bool endElement(const QString &namespaceURI, const QString &localName, const QString &qName);
    bool fatalError (const QXmlParseException & exception);
    bool characters(const QString &ch);
    QStringList* getHeaders();
    void setModel( QStandardItemModel *model );
    void setHeader( QStringList* header );
    void readHeader( bool flag );
    QStandardItemModel* getModel();
    QStandardItemModel* localmodel;
    int getRows();

signals:

public slots:

private:
    bool inrecord;
    bool inlabel;
    bool invalue;
    bool headerflag;
    int tablerow;
    int tablecol;
    QString label;
    QString datavalue;
    QStringList *headerlist;
};

#endif // LOGHANDLER_H
