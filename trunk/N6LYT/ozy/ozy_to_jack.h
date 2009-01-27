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

// decode OZY packet
#define SYNC_0       0
#define SYNC_1       1
#define SYNC_2       2
#define CONTROL_0    3
#define CONTROL_1    4
#define CONTROL_2    5
#define CONTROL_3    6
#define CONTROL_4    7
#define SAMPLES      8

#define FREQUENCY_1  9
#define FREQUENCY_2 10
#define FREQUENCY_3 11
#define FREQUENCY_4 12

// data_in_state
#define LEFT_HIGH    0
#define LEFT_MIDDLE  1
#define LEFT_LOW     2
#define RIGHT_HIGH   3
#define RIGHT_MIDDLE 4
#define RIGHT_LOW    5
#define MIC_HIGH     6
#define MIC_LOW      7

// data_out_state
#define LEFT_RX_HIGH  0
#define LEFT_RX_LOW   1
#define RIGHT_RX_HIGH 2
#define RIGHT_RX_LOW  3
#define LEFT_TX_HIGH  4
#define LEFT_TX_LOW   5
#define RIGHT_TX_HIGH 6
#define RIGHT_TX_LOW  7

#define OZY_SAMPLES_PER_BUFFER 63

extern int fifo;

extern int frequency;
extern int frequency_changed;
extern unsigned char control_in[5];
extern unsigned char control_out[5];
extern int output_sample_increment;

#ifdef	__cplusplus
}
#endif

#endif	/* _OZY_TO_JACK_H */

