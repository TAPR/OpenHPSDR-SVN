/*
 *	(c) 2009 Ben Watson
 *
 *	taken from:
 *	http://www.philosophicalgeek.com/2009/01/03/determine-cpu-usage-of-current-process-c-and-c/
 *
 */


#pragma once

//#define _WIN32_WINNT 0×0501
#ifndef _WIN32_WINNT            // Specifies that the minimum required platform is Windows Vista.
#define _WIN32_WINNT 0x0600     // Change this to the appropriate value to target other versions of Windows.
#endif

#include <windows.h>

class CpuUsage {

public:
	CpuUsage(void);
	
	short  GetUsage();

private:
	ULONGLONG SubtractTimes(const FILETIME& ftA, const FILETIME& ftB);
	bool EnoughTimePassed();
	inline bool IsFirstRun() const { return (m_dwLastRun == 0); }
	
	//system total times
	FILETIME m_ftPrevSysKernel;
	FILETIME m_ftPrevSysUser;

	//process times
	FILETIME m_ftPrevProcKernel;
	FILETIME m_ftPrevProcUser;

	short m_nCpuUsage;
	ULONGLONG m_dwLastRun;
	
	volatile LONG m_lRunCount;
};
