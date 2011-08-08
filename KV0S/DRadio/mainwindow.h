#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "sender.h"

namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

    Sender *sender;

private:
    Ui::MainWindow *ui;

private slots:
     void updateFreq( int );
     void updateMode(QString value);
};

#endif // MAINWINDOW_H
