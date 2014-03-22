#include <QDebug>

#include <QAction>
#include <QMenu>

#include "Xvtr.h"
#include "Mode.h"

Xvtr::Xvtr() {
    currentXvtr=NULL;
    currentXvtrAction=NULL;
}

// SLOTS
void Xvtr::triggered() {
    QAction *action = qobject_cast<QAction *>(sender());
    emit xvtrSelected(action);
}

// public functions

void Xvtr::loadSettings(QSettings* settings) {
    /*
    int count;
    QString s;
    QString title;
    quint64 minFrequency;
    quint64 maxFrequency;
    quint64 ifFrequency;
    quint64 frequency;
    int mode;
    int filter;

    settings->beginGroup("XVTR");
    if(settings->contains("count")) {
        count=settings->value("count").toInt();
        for(int i=0;i<count;i++) {
            s.sprintf("title.%d",i);
            title=settings->value(s).toString();
            s.sprintf("minFrequency.%d",i);
            minFrequency=settings->value(s).toLongLong();
            s.sprintf("maxFrequency.%d",i);
            maxFrequency=settings->value(s).toLongLong();
            s.sprintf("ifFrequency.%d",i);
            ifFrequency=settings->value(s).toLongLong();
            s.sprintf("frequency.%d",i);
            if(settings->contains(s)) {
                frequency=settings->value(s).toLongLong();
            } else {
                frequency=minFrequency;
            }
            s.sprintf("mode.%d",i);
            if(settings->contains(s)) {
                mode=settings->value(s).toLongLong();
            } else {
                mode=MODE_USB;
            }
            s.sprintf("filter.%d",i);
            if(settings->contains(s)) {
                filter=settings->value(s).toLongLong();
            } else {
                filter=5;
            }
            add(title,minFrequency,maxFrequency,ifFrequency,frequency,mode,filter);
        }
    }
    settings->endGroup();

    */
}

void Xvtr::saveSettings(QSettings* settings) {
    /*
    QString s;
    XvtrEntry* entry;

    settings->beginGroup("XVTR");
    settings->setValue("count",xvtrs.count());
    for(int i=0;i<xvtrs.count();i++) {
        entry=xvtrs.at(i);
        s.sprintf("title.%d",i);
        settings->setValue(s,entry->getTitle());
        s.sprintf("minFrequency.%d",i);
        settings->setValue(s,entry->getMinFrequency());
        s.sprintf("maxFrequency.%d",i);
        settings->setValue(s,entry->getMaxFrequency());
        s.sprintf("ifFrequency.%d",i);
        settings->setValue(s,entry->getIFFrequency());
    }
    settings->endGroup();
    */
}

void Xvtr::configure(QDomDocument* configuration) {
    int xvtrEntry;
    QString xvtrLabel;
    quint64 xvtrMinFrequency;
    quint64 xvtrMaxFrequency;
    quint64 xvtrLOFrequency;

    // walk through the DOM tree and extract the XVTR configuration
    QDomElement element=configuration->documentElement();
    QDomNode n=element.firstChild();
    while(!n.isNull()) {
        //qDebug()<<"Xvtr::confige: node:"<<n.nodeName();
        if(n.nodeName()=="xvtr") {
            QDomNode xvtrNode=n.firstChild();
            QDomNode xvtrElement;
            while(!xvtrNode.isNull()) {
                qDebug()<<"    "<<xvtrNode.nodeName();
                if(xvtrNode.nodeName()=="entry") {
                    xvtrElement=xvtrNode.firstChild();
                    if(!xvtrElement.isNull()) {
                        if(xvtrElement.isText()) {
                            xvtrEntry=xvtrElement.nodeValue().toInt();
                        }
                    }
                } else if(xvtrNode.nodeName()=="label") {
                    xvtrElement=xvtrNode.firstChild();
                    if(!xvtrElement.isNull()) {
                        if(xvtrElement.isText()) {
                            xvtrLabel=xvtrElement.nodeValue();
                        }
                    }
                } else if(xvtrNode.nodeName()=="minfreq") {
                    xvtrElement=xvtrNode.firstChild();
                    if(!xvtrElement.isNull()) {
                        if(xvtrElement.isText()) {
                            xvtrMinFrequency=xvtrElement.nodeValue().toLongLong();
                        }
                    }

                } else if(xvtrNode.nodeName()=="maxfreq") {
                    xvtrElement=xvtrNode.firstChild();
                    if(!xvtrElement.isNull()) {
                        if(xvtrElement.isText()) {
                            xvtrMaxFrequency=xvtrElement.nodeValue().toLongLong();
                        }
                    }
                } else if(xvtrNode.nodeName()=="lofreq") {
                    xvtrElement=xvtrNode.firstChild();
                    if(!xvtrElement.isNull()) {
                        if(xvtrElement.isText()) {
                            xvtrLOFrequency=xvtrElement.nodeValue().toLongLong();
                        }
                    }
                }
                xvtrNode=xvtrNode.nextSibling();
            }
            //qDebug()<<xvtrLabel<<": "<<xvtrMinFrequency<<":"<<xvtrMaxFrequency<<":"<<xvtrLOFrequency;

            add(xvtrEntry,xvtrLabel,xvtrMinFrequency,xvtrMaxFrequency,xvtrLOFrequency,xvtrMinFrequency,0,0);
        }
        n=n.nextSibling();
    }
}

void Xvtr::buildMenu(QMenu* menu) {
    XvtrEntry* xvtr;
    QAction* action;
    //menu->clear();
    for(int i=0;i<xvtrs.size();++i) {
        xvtr=xvtrs.at(i);
        action=new QAction(xvtr->getTitle(),this);
        action->setCheckable(true);
        menu->addAction(action);

        //need an event for the new action
        connect(action,SIGNAL(triggered()),this,SLOT(triggered()));
    }
}

void Xvtr::add(int entry, QString title, quint64 minFrequency, quint64 maxFrequency, quint64 ifFrequency,quint64 frequency,int mode,int filter) {
    qDebug()<<"Xvtr::add"<<title;
    xvtrs.append(new XvtrEntry(entry,title,minFrequency,maxFrequency,ifFrequency,frequency,mode,filter));
}

void Xvtr::del(int index) {
    xvtrs.remove(index);
}

void Xvtr::select(QAction* action) {
    if(currentXvtrAction!=NULL) {
        currentXvtrAction->setChecked(false);
    }

    // find the entry
    for(int i=0;i<xvtrs.size();i++) {
        currentXvtr=xvtrs.at(i);
        if(action->text()==currentXvtr->getTitle()) {
            break;
        } else {
            currentXvtr=NULL;
        }
    }

    if(currentXvtr!=NULL) {
        action->setChecked(true);
        currentXvtrAction=action;
    }
}

void Xvtr::deselect() {
    currentXvtrAction->setChecked(false);
}

int Xvtr::getEntry() {
    return currentXvtr->getEntry();
}

QString Xvtr::getTitle() {
    return currentXvtr->getTitle();
}

quint64 Xvtr::getMinFrequency() {
    return currentXvtr->getMinFrequency();
}

quint64 Xvtr::getMaxFrequency() {
    return currentXvtr->getMaxFrequency();
}

quint64 Xvtr::getIFFrequency() {
    return currentXvtr->getIFFrequency();
}


int Xvtr::count() {
    return xvtrs.count();
}

XvtrEntry* Xvtr::getXvtrAt(int index) {
    return xvtrs.at(index);
}

quint64 Xvtr::getFrequency() {
    return currentXvtr->getFrequency();
}

int Xvtr::getMode() {
    return currentXvtr->getMode();
}

int Xvtr::getFilter() {
    return currentXvtr->getFilter();
}

void Xvtr::setFrequency(quint64 f) {
    currentXvtr->setFrequency(f);
}

void Xvtr::setMode(int m) {
    currentXvtr->setMode(m);
}

void Xvtr::setFilter(int f) {
    currentXvtr->setFilter(f);
}
