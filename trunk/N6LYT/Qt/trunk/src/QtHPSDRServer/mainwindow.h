#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QSettings>
#include <QDir>
#include <QFile>
#include <QStandardItemModel>

#include <hpsdr/interfaces.h>
#include <hpsdr/server.h>
#include <hpsdr/alex.h>

#include <client/clientlistener.h>


namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    void closeEvent(QCloseEvent* event);

public slots:
    void actionAbout();
    void actionQuit();

    void actionStartStop();

    void saveConfiguration();

    void hpsdrSelected();
    void hermesSelected();
    void ozySelected();
    void metisSelected();

    void mercurySelected();
    void penelopeSelected();
    void pennylaneSelected();
    void excaliburSelected();
    void alexSelected();

    void autoStartSelected();
    void autoStartDSPSelected();

    void clock10Selected(QString);
    void clock122_88Selected(QString);

    void sampleRateSelected(QString);

    void interfaceSelected(QString);
    void actionDiscover();
    void metisCardSelected(QString);

    void alexSaveConfiguration();

    void clientConnected();
    void clientDisconnected();
    void updateClientList();

    void firmwareVersionChanged();


private:
    Ui::MainWindow *ui;

    QStandardItemModel model;
    QStringList labels;

    void restoreSettings();
    void saveSettings();

    QString getConfigDir();
    QString configDir;
    QString configFileName;

    QSettings* settings;

    void update10MHzClockSources();
    void update122_88MHzClockSources();

    bool running;

    //bool hermes;
    //bool hpsdr;

    //bool ozy;
    //bool metis;
    //bool mercury;
    //bool penelope;
    //bool pennylane;
    //bool excalibur;
    //bool alex;

    int sampleRate;
    bool random;
    bool dither;
    bool classE;
    bool lineIn;
    bool micBoost;

    QString clock10MHz;
    QString clock122_88MHz;

    int receivers;

    int metisPort;

    bool autoStart;

    bool autoStartDSPServer;
    int dspServerPort;
    int dspServerClientPort;

    Server* server;
    ClientListener* clientListener;
    Alex* alex;

    void updateInterfaces();

};

#endif // MAINWINDOW_H
