#ifndef XVTR_H
#define XVTR_H

#include <QObject>
#include <QAction>
#include <QString>
#include <QSettings>
#include <QtXml/QDomDocument>
#include <QtXml/QDomNodeList>

#include "XvtrEntry.h"

class Xvtr : public QObject {
    Q_OBJECT

public:
    Xvtr();
    void buildMenu(QMenu* menu);
    void add(int,QString,quint64,quint64,quint64,quint64,int,int);
    void del(int index);
    void select(QAction* action);
    void deselect();
    int getEntry();
    QString getTitle();
    quint64 getMinFrequency();
    quint64 getMaxFrequency();
    quint64 getIFFrequency();

    quint64 getFrequency();
    int getMode();
    int getFilter();
    void setFrequency(quint64 f);
    void setMode(int m);
    void setFilter(int f);

    int count();
    XvtrEntry* getXvtrAt(int index);

    void configure(QDomDocument* configuration);
    void loadSettings(QSettings* settings);
    void saveSettings(QSettings* settings);

public slots:
    void triggered();

signals:
    void xvtrSelected(QAction*);

private:
    QVector<XvtrEntry*> xvtrs;     // list of XVTR entries
    QAction *currentXvtrAction;    // action for current entry
    XvtrEntry* currentXvtr;        // currently selected
};

#endif // XVTR_H
