/*
 *	Windows part for CPU usage by (c) 2009 Ben Watson
 *
 *	taken from http://www.philosophicalgeek.com/2009/01/03/determine-cpu-usage-of-current-process-c-and-c/
 *
 *  adapted for cuSDR by (c) 2012  Hermann von Hasseln, DL3HVH
 *
 *
 *  Linux part for CPU usage by (c) by Fabian Holler
 *
 *  taken from https://github.com/fho/code_snippets/blob/master/c/getusage.c
 *
 *  adapted for cuSDR by (c) 2012 Andrea Montefusco, IW0HDV
 *
 */

#include "cusdr_cpuUsage.h"

#if defined(Q_OS_LINUX)
#include <stdlib.h> 
#include <sys/types.h>
#include <stdio.h>
#include <strings.h>   // bzero
#include <unistd.h>    // getpagesize
#include <string.h>    // strncat, strlen

/*
 * read /proc data into the passed struct pstat
 * returns 0 on success, -1 on error
*/
static int get_usage(const pid_t pid, struct pstat* result) {
	
	//convert  pid to string
	char pid_s[20];
    snprintf(pid_s, sizeof(pid_s), "%d", pid);

    char stat_filepath[30] = "/proc/"; strncat(stat_filepath, pid_s,
            sizeof(stat_filepath) - strlen(stat_filepath) -1);
    strncat(stat_filepath, "/stat", sizeof(stat_filepath) -
            strlen(stat_filepath) -1);

    //Open /proc/stat and /proc/$pid/stat fds successive(dont want that cpu
    //ticks increases too much during measurements)
    //TODO: open /proc dir, to lock all files and read the results from the
    //same timefragem
    FILE *fpstat = fopen(stat_filepath, "r");
    
	if (fpstat == NULL) {
		
		perror("FOPEN ERROR ");
		return -1;
    }

    FILE *fstat = fopen("/proc/stat", "r");
    if (fstat == NULL) {
		
		perror("FOPEN ERROR ");
		fclose(fstat);
		return -1;
	}

    //read values from /proc/pid/stat
    bzero(result, sizeof(struct pstat));
    long int rss;
    if (fscanf(fpstat, "%*d %*s %*c %*d %*d %*d %*d %*d %*u %*u %*u %*u %*u %lu"
                "%lu %ld %ld %*d %*d %*d %*d %*u %lu %ld",
                &result->utime_ticks, &result->stime_ticks,
                &result->cutime_ticks, &result->cstime_ticks, &result->vsize,
                &rss) == EOF) {

        fclose(fpstat);
        return -1;
    }

    fclose(fpstat);
    result->rss = rss * getpagesize();

    //read+calc cpu total time from /proc/stat
    long unsigned int cpu_time[10];
    bzero(cpu_time, sizeof(cpu_time));

    if (fscanf(fstat, "%*s %lu %lu %lu %lu %lu %lu %lu %lu %lu %lu",
                &cpu_time[0], &cpu_time[1], &cpu_time[2], &cpu_time[3],
                &cpu_time[4], &cpu_time[5], &cpu_time[6], &cpu_time[7],
                &cpu_time[8], &cpu_time[9]) == EOF) {
					
		fclose(fstat);
		return -1;
	}

    fclose(fstat);

    for (int i = 0; i < 10; i++) {
		
		result->cpu_total_time += cpu_time[i];
	}
    return 0;
}

/*
* calculates the actual CPU usage(cur_usage - last_usage) in percent
* cur_usage, last_usage: both last measured get_usage() results
* ucpu_usage, scpu_usage: result parameters: user and sys cpu usage in %
*/
static void calc_cpu_usage(const struct pstat* cur_usage, const struct pstat*
                    last_usage, double* ucpu_usage, double* scpu_usage) {

    const long unsigned int total_time_diff = cur_usage->cpu_total_time - last_usage->cpu_total_time;

    *ucpu_usage = 100 * (((cur_usage->utime_ticks + cur_usage->cutime_ticks)
                    - (last_usage->utime_ticks + last_usage->cutime_ticks))
                    / (double) total_time_diff);

    *scpu_usage = 100 * ((((cur_usage->stime_ticks + cur_usage->cstime_ticks)
                    - (last_usage->stime_ticks + last_usage->cstime_ticks))) /
                    (double) total_time_diff);
}

CpuUsage :: CpuUsage (void) {

	pid = getpid();
	int rc = get_usage(pid, &cpst);
	
	if (rc < 0) {
		
		printf ("Error, check PID\n");
	} 
	else {
	
		lpst = cpst;
	}
}

short CpuUsage :: GetUsage (void) {

	int rc = get_usage(pid, &cpst);
      
	if (rc < 0) {

		printf ("Error, check PID\n");
	}

	double ucpu_usage;
	double scpu_usage;
	calc_cpu_usage (&cpst, &lpst, &ucpu_usage, &scpu_usage);
	//printf ("usr: %7.2g sys: %7.2g", ucpu_usage, scpu_usage);
	//printf ("  T: %7.0f\n", (float)(ucpu_usage+scpu_usage)/2.0*10.0);

	lpst = cpst;
	return (short) ((ucpu_usage+scpu_usage)/2.0*10.0);
}

#endif


#ifdef TEST_MODULE

int main (int argc, char **argv) {
	
	int pid;
	struct pstat cpst;
	struct pstat lpst;
	
	if (argc > 1 && sscanf (argv[1], "%d", &pid) == 1) {
		
		int rc = get_usage(pid, &cpst);
		
		if (rc < 0) {
			
			printf ("Error, check PID\n");
			return 254;
		}
		else {
			
			lpst = cpst;
		}
	}
	else {

       return 255;
	}
	
	for (;;) {
		
		sleep (1);
		
		int rc = get_usage(pid, &cpst);
		
		if (rc < 0) {
			
			printf ("Error, check PID\n");
			return 254;
		}
		
		double ucpu_usage;
		double scpu_usage;
		calc_cpu_usage (&cpst, &lpst, &ucpu_usage, &scpu_usage);
		printf ("usr: %7.2g sys: %7.2g", ucpu_usage, scpu_usage);
		printf ("  T: %7.0f\n", (float)(ucpu_usage+scpu_usage)/2.0*10.0);
		
		lpst = cpst;
	}
}

#endif


#if defined(Q_OS_WIN32)
//#error WWWWWWWW

CpuUsage::CpuUsage(void)
	:m_nCpuUsage(-1)
	,m_dwLastRun(0)
	,m_lRunCount(0)
{
	ZeroMemory(&m_ftPrevSysKernel, sizeof(FILETIME));
	ZeroMemory(&m_ftPrevSysUser, sizeof(FILETIME));

	ZeroMemory(&m_ftPrevProcKernel, sizeof(FILETIME));
	ZeroMemory(&m_ftPrevProcUser, sizeof(FILETIME));
}


/**********************************************
* CpuUsage::GetUsage
* returns the percent of the CPU that this process
* has used since the last time the method was called.
* If there is not enough information, -1 is returned.
* If the method is recalled to quickly, the previous value
* is returned.
***********************************************/
short CpuUsage::GetUsage() {

	//create a local copy to protect against race conditions in setting the 
	//member variable
	short nCpuCopy = m_nCpuUsage;
	if (::InterlockedIncrement(&m_lRunCount) == 1) {

		/*
		If this is called too often, the measurement itself will greatly affect the
		results.
		*/

		if (!EnoughTimePassed()) {

			::InterlockedDecrement(&m_lRunCount);
			return nCpuCopy;
		}

		FILETIME ftSysIdle, ftSysKernel, ftSysUser;
		FILETIME ftProcCreation, ftProcExit, ftProcKernel, ftProcUser;

		if (!GetSystemTimes(&ftSysIdle, &ftSysKernel, &ftSysUser) ||
			!GetProcessTimes(GetCurrentProcess(), &ftProcCreation, &ftProcExit, &ftProcKernel, &ftProcUser))
		{
			::InterlockedDecrement(&m_lRunCount);
			return nCpuCopy;
		}

		if (!IsFirstRun()) {
			/*
			CPU usage is calculated by getting the total amount of time the system has operated
			since the last measurement (made up of kernel + user) and the total
			amount of time the process has run (kernel + user).
			*/
			ULONGLONG ftSysKernelDiff = SubtractTimes(ftSysKernel, m_ftPrevSysKernel);
			ULONGLONG ftSysUserDiff = SubtractTimes(ftSysUser, m_ftPrevSysUser);

			ULONGLONG ftProcKernelDiff = SubtractTimes(ftProcKernel, m_ftPrevProcKernel);
			ULONGLONG ftProcUserDiff = SubtractTimes(ftProcUser, m_ftPrevProcUser);

			ULONGLONG nTotalSys =  ftSysKernelDiff + ftSysUserDiff;
			ULONGLONG nTotalProc = ftProcKernelDiff + ftProcUserDiff;

			if (nTotalSys > 0) {

				m_nCpuUsage = (short)((100.0 * nTotalProc) / nTotalSys);
			}
		}
		
		m_ftPrevSysKernel = ftSysKernel;
		m_ftPrevSysUser = ftSysUser;
		m_ftPrevProcKernel = ftProcKernel;
		m_ftPrevProcUser = ftProcUser;
		
		m_dwLastRun = GetTickCount();

		nCpuCopy = m_nCpuUsage;
	}
	
	::InterlockedDecrement(&m_lRunCount);

	return nCpuCopy;
}

ULONGLONG CpuUsage::SubtractTimes(const FILETIME& ftA, const FILETIME& ftB) {

	LARGE_INTEGER a, b;
	a.LowPart = ftA.dwLowDateTime;
	a.HighPart = ftA.dwHighDateTime;

	b.LowPart = ftB.dwLowDateTime;
	b.HighPart = ftB.dwHighDateTime;

	return a.QuadPart - b.QuadPart;
}

bool CpuUsage::EnoughTimePassed() {

	const int minElapsedMS = 250;//milliseconds

	ULONGLONG dwCurrentTickCount = GetTickCount();
	return ((int)(dwCurrentTickCount - m_dwLastRun)) > minElapsedMS;
}

#endif