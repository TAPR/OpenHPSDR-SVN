#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QLabel>
#include <QDebug>
#include <QTimer>
#include <QFileDialog>
#include <QFileInfo>
#include <QDir>
#include <QMessageBox>
#include <QStringList>
#include <QSettings>
#include <QUdpSocket>


#include "../Help/aboutdialog.h"
#include "../Status/statusdialog.h"
#include "./hpsdr/addressdialogprog.h"
#include "./hpsdr/interfaces.h"
#include "./hpsdr/writeboard.h"
#include "version.h"


namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    // private function to display message in the status window
    void status(QString text);


private:
    Ui::MainWindow *ui;
    QLabel *deviceIndicator;

    AboutDialog *ab;
    StatusDialog *stat;
    AddressDialog *add;
    Interfaces interfaces;
    QUdpSocket *socket;
    WriteBoard *wb;
    QString fileName;

private slots:
    void interfaceSelected(int id);
    void discover();
    void discoveryUpdate();
    void browse();
    void program();
};

#endif // MAINWINDOW_H
