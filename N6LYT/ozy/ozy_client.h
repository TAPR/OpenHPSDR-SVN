/* 
 * File:   ozy_client.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 16 December 2008, 18:05
 */

#ifndef _OZY_CLIENT_H
#define	_OZY_CLIENT_H

#ifdef	__cplusplus
extern "C" {
#endif
    
extern int state_in;
extern int data_state_in;
extern int samples_in;
extern int total_samples_in;


extern int state_out;
extern int data_state_out;
extern int samples_out;
extern int total_samples_out;

extern int output_sample_increment;

#ifdef	__cplusplus
}
#endif

#endif	/* _OZY_CLIENT_H */

