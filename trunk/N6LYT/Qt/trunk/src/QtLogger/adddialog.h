/**
* \file addialog.h
* \brief Header files for the addDialog functions as part of the QtLogger program
* \author David R. Larsen, KV0S
* \version 1.0.2
* \date August 21, 2011
*/



/* Copyright (C) 2011 - David R. Larsen, KV0S
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

#ifndef ADDDIALOG_H
#define ADDDIALOG_H

#include <QDialog>
#include <QTime>
#include <QDate>
#include <QTimer>
#include <QHash>
#include <QCompleter>
#include <QHashIterator>
#include <QSettings>
#include <QSortFilterProxyModel>

#include "data.h"
#include "range.h"
#include "ctyxmlreader.h"


namespace Ui {
    class addDialog;
}

class addDialog : public QDialog
{
    Q_OBJECT

public:
    explicit addDialog(QWidget *parent = 0 );
    ~addDialog();
    QDate *date;
    QTime *time;
    QHash <QString, QString> record;
    void setFrequency( QString freq );
    void setMode( QString modestr );
    QString getBand( QString fstr );
    QStandardItemModel *model;
    QSortFilterProxyModel *proxyModel;
    void loadmodeComboBox(QStringList *modes);
    void loadcountryComboBox(QStringList *country);
    void loadsubdivisionsComboBox(QStringList *subdivisions);
    void loadbandsData( QHash <QString, Range> band );
    QHash <QString, Range> bandData;
    void callreset();
    bool callsign_filter;

private:
    Ui::addDialog *ui;
    QSettings settings;



signals:
    void newdata();
    void ownerChanged();
    void prefixChanged( QString );

private slots:
    void reset();
    void addContact();
    void updateTime();
    void updateCall();
    void updateOwner();
    void updateFilter( QString str );

};

#endif // ADDDIALOG_H
