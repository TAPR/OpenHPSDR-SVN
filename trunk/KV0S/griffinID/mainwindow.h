#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QFile>
#include <QFileDialog>
#include <QSettings>
#include <QByteArray>
#include "about.h"

namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    QSettings settings;
    void loadGridBox();
    void loadPowerBox();
    QString morsechar( QChar ch );
    QString encodeCW( QStringList message );
    int wsprcallchar( QChar ch );
    int wsprlocchar( QChar ch );
    long encodeWSPR( QStringList message );
    QString displayCW( QString message );

private:
    Ui::MainWindow *ui;

private slots:
    void writeString();
};

#endif // MAINWINDOW_H
