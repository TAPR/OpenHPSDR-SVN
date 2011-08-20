#ifndef DIALOG_H
#define DIALOG_H

#include <QDialog>
#include <QTextBrowser>
#include <QStringList>

namespace Ui {
    class Dialog;
}

class Dialog : public QDialog
{
    Q_OBJECT

public:
    explicit Dialog(QWidget *parent = 0);
    ~Dialog();
    //QString *path;

private:
    Ui::Dialog *ui;
    QList <QUrl> oldsource;

private slots:
    void home();
    void back();
    void updatesource( QUrl newsource );
};

#endif // DIALOG_H
