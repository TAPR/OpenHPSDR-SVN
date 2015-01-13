#ifndef CUSDR_CPULOAD_H
#define CUSDR_CPULOAD_H

#include <QThread>
#include <QTimer>

class cusdr_cpuUsage : public QThread
{
    Q_OBJECT
public:
    clock_t ptick;
    double ptime;
    QTimer *timer;

    cusdr_cpuUsage();

private:
    int CLOCK_TICK;

private slots:
    void getCPUUsage();
};

#endif // CUSDR_CPULOAD_H
