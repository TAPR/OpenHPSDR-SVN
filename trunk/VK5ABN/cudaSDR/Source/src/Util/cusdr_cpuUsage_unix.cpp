#include <unistd.h>
#include <sys/resource.h>
#include <sys/times.h>

#include "cusdr_cpuUsage_unix.h"
#include "cusdr_settings.h"

cusdr_cpuUsage::cusdr_cpuUsage()
{
    ptick=0;
    ptime=0.0;
    CLOCK_TICK = sysconf(_SC_CLK_TCK);

    QTimer *timer = new QTimer();
    cusdr_cpuUsage::connect(timer, SIGNAL(timeout()), this, SLOT(getCPUUsage()));

    timer->start(1000);
}

void cusdr_cpuUsage::getCPUUsage()
{
    clock_t tick;
    double time;
    double load;

    struct rusage usage;
    struct tms systime;

    tick = times(&systime);
    getrusage(RUSAGE_SELF, &usage);

    time = usage.ru_utime.tv_sec + usage.ru_utime.tv_usec * 1e-6 +
           usage.ru_stime.tv_sec + usage.ru_stime.tv_usec * 1e-6;

    load = ((time-ptime)/(tick-ptick)) * CLOCK_TICK * 100;

    if(ptick && ptime)
        Settings::instance()->setCPULoad((int) load);

    ptick=tick;
    ptime=time;
}
