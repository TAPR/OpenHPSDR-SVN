/* 
 * File:   global.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 16 December 2008, 17:38
 */

#ifndef _GLOBAL_H
#define	_GLOBAL_H

#ifdef	__cplusplus
extern "C" {
#endif

extern int debug;
extern int debug_buffers;
extern int debug_commands;
extern int debug_mic_samples;
extern int debug_rx_samples;
extern int debug_spectrum;

extern int mox_state_changed;

extern int total_samples_in;
extern int total_samples_out;


#ifdef	__cplusplus
}
#endif

#endif	/* _GLOBAL_H */

