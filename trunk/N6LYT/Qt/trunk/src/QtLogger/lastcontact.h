#ifndef LASTCONTACT_H
#define LASTCONTACT_H

#include <QDialog>
#include <QStandardItemModel>
#include <QSortFilterProxyModel>

namespace Ui {
class lastContact;
}

class lastContact : public QDialog
{
    Q_OBJECT
    
public:
    explicit lastContact(QWidget *parent = 0);
    ~lastContact();
    QStandardItemModel *contactModel;
    QSortFilterProxyModel *proxyModel;
    void setModel( QStandardItemModel *model );

    
private:
    Ui::lastContact *ui;

private slots:
    void setCallLabel( QString call );
};

#endif // LASTCONTACT_H
