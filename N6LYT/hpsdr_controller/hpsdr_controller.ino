/* Copyright (C)
* 2015 - John Melton, G0ORX/N6LYT
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License
* as published by the Free Software Foundation; either version 2
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*
*/

/*
*  HPSDR radio controller using Arduino UNO
*  using step endcoders and push buttons.
*  
*  sends CAT comands over USB serial port
* 
*  initial test version
* 
*  currently supports:
* 
*     Tuning up/down
*     AF gain
*     RF gain
*     Band up/down
*     
*     
*  Uses the encoder library from http://www.pjrc.com/teensy/td_libs_Encoder.html
*/


#include <Encoder.h>

Encoder tuningEnc(2, 3);
Encoder afEnc(4,5);
Encoder rfEnc(6,7);

int bandPin = 8;
int bandState;
int lastBandState = 0;
unsigned long startTime;
unsigned long endTime;


int afGain = 50;
int rfGain = 50;

void setup() {
  pinMode(bandPin, INPUT);
  bandState = digitalRead(bandPin);
  tuningEnc.write(0);
  afEnc.write(0);
  rfEnc.write(0);
  Serial.begin(9600);
}

void loop() {
  long tunePosition = tuningEnc.read();
  if (tunePosition != 0) {
    tuningEnc.write(0);
    if(tunePosition<0) {
      if(bandState==0) {
          Serial.print("ZZBD;");
      } else {
          Serial.print("ZZSB;");
      }
    } else {
      if(bandState==0) {
          Serial.print("ZZBU;");
      } else {
          Serial.print("ZZSA;");
      }
    }
  }

  long afPosition = afEnc.read();
  if (afPosition != 0) {
    afEnc.write(0);
    if(afPosition<0) {
      if(afGain>0) {
        afGain--;
      }
    } else {
      if(afGain<100) {
        afGain++;
      }
    }
    Serial.print("ZZLA");
    Serial.print(afGain/100);
    Serial.print((afGain%100)/10);
    Serial.print(afGain%10);
    Serial.print(";");
  }

  long rfPosition = rfEnc.read();
  if (rfPosition != 0) {
    rfEnc.write(0);
    if(rfPosition<0) {
      if(rfGain>0) {
        rfGain--;
      }
    } else {
      if(rfGain<100) {
        rfGain++;
      }
    }
    Serial.print("ZZPC");
    Serial.print(rfGain/100);
    Serial.print((rfGain%100)/10);
    Serial.print(rfGain%10);
    Serial.print(";");
  }

  bandState = digitalRead(bandPin);
    
}
