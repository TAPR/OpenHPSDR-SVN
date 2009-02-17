/* 
 * File:   ozy_to_jack.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 31 December 2008, 11:25
 */

#ifndef _OZY_TO_JACK_H
#define	_OZY_TO_JACK_H

#ifdef	__cplusplus
extern "C" {
#endif


// sync character
#define SYNC 0x7F

#define OZY_SAMPLES_PER_BUFFER 63

extern int frequency;
extern int frequency_changed;
extern unsigned char control_in[5];
extern unsigned char control_out[5];
extern int output_sample_increment;

#ifdef	__cplusplus
}
#endif

#endif	/* _OZY_TO_JACK_H */

