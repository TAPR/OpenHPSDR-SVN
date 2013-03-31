#ifndef STATUSDIALOG_H
#define STATUSDIALOG_H

#include <QDialog>
#include <QDebug>

namespace Ui {
class StatusDialog;
}

class StatusDialog : public QDialog
{
    Q_OBJECT
    
public:
    explicit StatusDialog(QWidget *parent = 0);
    ~StatusDialog();
    void status( QString text );
    void clear();

signals:
    void stbar(QString);

private:
    Ui::StatusDialog *ui;
};

#endif // STATUSDIALOG_H
