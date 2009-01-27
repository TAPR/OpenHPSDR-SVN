/* 
 * File:   ozy.h
 * Author: John Melton G0ORX/N6LYT
 *
 * Created on 23 December 2008, 14:32
 */

#ifndef _OZY_H
#define	_OZY_H

#ifdef	__cplusplus
extern "C" {
#endif

#define DEFAULT_COMMAND_PORT 19601

// ozy command and control
#define MOX_DISABLED    0x00
#define MOX_ENABLED     0x01

#define MIC_SOURCE_JANUS 0x00
#define MIC_SOURCE_PENELOPE 0x80
#define CONFIG_NONE     0x00
#define CONFIG_PENELOPE 0x20
#define CONFIG_MERCURY  0x40
#define CONFIG_BOTH     0x60
#define PENELOPE_122_88MHZ_SOURCE 0x00
#define MERCURY_122_88MHZ_SOURCE  0x10
#define ATLAS_10MHZ_SOURCE        0x00
#define PENELOPE_10MHZ_SOURCE     0x04
#define MERCURY_10MHZ_SOURCE      0x08
#define SPEED_48KHZ               0x00
#define SPEED_96KHZ               0x01
#define SPEED_192KHZ              0x02

#define MODE_CLASS_E              0x01
#define MODE_OTHERS               0x00

#define ALEX_ATTENUATION_0DB      0x00
#define ALEX_ATTENUATION_10DB     0x01
#define ALEX_ATTENUATION_20DB     0x02
#define ALEX_ATTENUATION_30DB     0x03
#define LT2208_GAIN_OFF           0x00
#define LT2208_GAIN_ON            0x04
#define LT2208_DITHER_OFF         0x00
#define LT2208_DITHER_ON          0x08
#define LT2208_RANDOM_OFF         0x00
#define LT2208_RANDOM_ON          0x10
    
extern void setMOX(int state);
extern void setSpeed(int speed);
extern void set10MHzSource(int source);
extern void set122MHzSource(int source);
extern void setConfig(int config);
extern void setMicSource(int source);
extern void setMode(int mode);
extern void setOCOutputs(int outputs);
extern void setAlexAttenuator(int attenuator);
extern void setLT2208PreampGain(int gain);
extern void setLT2208Dither(int dither);
extern void setLT2208Random(int random);
extern void setFrequency(double f);

#ifdef	__cplusplus
}
#endif

#endif	/* _OZY_H */

