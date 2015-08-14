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
*  Basic version:
*  
*  1 Encoder
*  3 SPST Push to make switches
* 
*  currently supports:
* 
*     Tuning up/down
*     Band up/down
*     Mode up/down
*     AF Gain up/down
*     RF Gain up/down
*     
*  Uses the encoder library from http://www.pjrc.com/teensy/td_libs_Encoder.html
*  Uses the switch bebounce library from http://playground.arduino.cc/Code/Bounce
*/

#include <Encoder.h>
#include <Bounce2.h>


Encoder tuningEnc(2, 3);

#define encoderPin 4
Bounce encoderSwitch = Bounce();

#define bandPin 5
Bounce bandSwitch = Bounce();

#define modePin 6
Bounce modeSwitch = Bounce();

#define functionPin 7
Bounce functionSwitch = Bounce();



int encoder=0;
int function=0;

int afGain = -1;
int rfGain = -1;

char message[8];
int messageIndex=0;

void setup() {
  

  // setup function pin
  pinMode(functionPin, INPUT);
  functionSwitch.attach(functionPin);
  functionSwitch.interval(20);
  digitalWrite(functionPin, HIGH);
  
  // setup band pin
  pinMode(bandPin, INPUT);
  bandSwitch.attach(bandPin);
  bandSwitch.interval(20);
  digitalWrite(bandPin, HIGH);

  // setup mode pin
  pinMode(modePin, INPUT);
  modeSwitch.attach(modePin);
  modeSwitch.interval(20);
  digitalWrite(modePin, HIGH);
  
  //setup encoder switch
  pinMode(encoderPin, INPUT);
  encoderSwitch.attach( encoderPin );
  encoderSwitch.interval(20);
  digitalWrite(encoderPin,HIGH);

  Serial.begin(9600);
}

void loop() {
   checkSerialData();

   if(functionSwitch.update()) {
      if(functionSwitch.read()==0) {
        function=1;
      } else {
        function=0;
      }
    }

    if(encoderSwitch.update()) {
      if(encoderSwitch.read()==0) {
        encoder=1;
      } else {
        encoder=0;
      }
    }

    long tunePosition = tuningEnc.read();
    if (tunePosition != 0) {
      tuningEnc.write(0);
      if(function) {
        if(afGain==-1) {
          Serial.print("ZZAG;");
        } else {
          if(tunePosition<0) {
            afGain--;
            if(afGain<0) {
              afGain=0;
            }
          } else {
            afGain++;
            if(afGain>100) {
              afGain=100;
            }
          }
          Serial.print("ZZAG");
          Serial.print(afGain/100);
          Serial.print((afGain%100)/10);
          Serial.print(afGain%10);
          Serial.print(";");
        }
      } else if(encoder) {
        if(rfGain==-1) {
          Serial.print("ZZPC;");
        } else {
          if(tunePosition<0) {
            rfGain--;
            if(rfGain<0) {
              rfGain=0;
            }
          } else {
            rfGain++;
            if(rfGain>100) {
              rfGain=100;
            }
          }
          Serial.print("ZZPC");
          Serial.print(rfGain/100);
          Serial.print((rfGain%100)/10);
          Serial.print(rfGain%10);
          Serial.print(";");
        }
      } else {
        if(tunePosition<0) {
              Serial.print("ZZSB;");
        } else {
              Serial.print("ZZSA;");
        }
      }
    }

  if(bandSwitch.update()) {
      // note this switch has no pullup resistor
      if(bandSwitch.read()==0) {
        if(function) {
          Serial.print("ZZBD;");
        } else {
          Serial.print("ZZBU;");
        }
      }
    }

    if(modeSwitch.update()) {
      // note this switch has no pullup resistor
      if(modeSwitch.read()==0) {
        Serial.print("ZZMD;"); // get the mode
      }
    }

}

void checkSerialData() {
   while(Serial.available() > 0) {
      // read the incoming byte:
      char c=Serial.read();
      if(c==';') {
        if(strncmp(message,"ZZAG",4)==0 && messageIndex==7) {
            int gain=((message[4]-'0')*100)+
                     ((message[5]-'0')*10)+
                     (message[6]-'0');
            afGain=gain;
        } else if(strncmp(message,"ZZPC",4)==0 && messageIndex==7) {
            int gain=((message[4]-'0')*100)+
                     ((message[5]-'0')*10)+
                     (message[6]-'0');
            rfGain=gain;
        } else if(strncmp(message,"ZZMD",4)==0 && messageIndex==6) {
          int mode=((message[4]-'0')*10)+
                   (message[5]-'0');
          if(function) {
            mode--;
            if(mode<0) {
              mode=11;
            }
          } else {
            mode++;
            if(mode>11) {
              mode=0;
            }
          }
          Serial.print("ZZMD");
          Serial.print(mode/10);
          Serial.print(mode%10);
          Serial.print(";");
        } else {
            // unhandled message;
        }
        messageIndex=0;
      } else {
        message[messageIndex++]=c;
      }
   }
}
