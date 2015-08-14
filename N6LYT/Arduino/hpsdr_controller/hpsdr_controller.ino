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
*     Tuning step up
*     
*     
*  Uses the encoder library from http://www.pjrc.com/teensy/td_libs_Encoder.html
*/

#include <Scheduler.h>
#include <U8glib.h>
#include <Encoder.h>
#include <Bounce2.h>


U8GLIB_SH1106_128X64 u8g(U8G_I2C_OPT_NONE);

Encoder tuningEnc(2, 3);
Encoder rfEnc(4,5);
Encoder afEnc(6,7);

#define stepPin 8
Bounce stepSwitch = Bounce();

#define step9Pin 9
Bounce step9Switch = Bounce();

#define step10Pin 10
Bounce step10Switch = Bounce();

#define bandPin 11
Bounce bandSwitch = Bounce();

#define modePin 12
Bounce modeSwitch = Bounce();

#define filterPin 22
Bounce filterSwitch = Bounce();

#define nrPin 23
Bounce nrSwitch = Bounce();

#define naPin 24
Bounce naSwitch = Bounce();

#define functionPin 25
Bounce functionSwitch = Bounce();

int afGain = -1;
int rfGain = -1;

int programMode = 0;

char message[32];
char messageIndex=0;
long frequency=0;

int counter=0;

int function = 0;

void setup() {

  // setup band down pin
  pinMode(bandPin, INPUT);
  bandSwitch.attach(bandPin);
  bandSwitch.interval(20);
  digitalWrite(bandPin, HIGH);
  
  // setup mode pin
  pinMode(modePin, INPUT);
  modeSwitch.attach(modePin);
  modeSwitch.interval(20);
  digitalWrite(modePin, HIGH);

  // setup filter pin
  pinMode(filterPin, INPUT);
  filterSwitch.attach(filterPin);
  filterSwitch.interval(20);
  digitalWrite(filterPin, HIGH);

  // setup NR pin
  pinMode(nrPin, INPUT);
  nrSwitch.attach(nrPin);
  nrSwitch.interval(20);
  digitalWrite(nrPin, HIGH);

  // setup NA pin
  pinMode(naPin, INPUT);
  naSwitch.attach(naPin);
  naSwitch.interval(20);
  digitalWrite(naPin, HIGH);
  
  // setup function up pin
  pinMode(functionPin, INPUT);
  functionSwitch.attach(functionPin);
  functionSwitch.interval(20);
  digitalWrite(functionPin, HIGH);
  
  // setup tuning step encoder
  tuningEnc.write(0);
  afEnc.write(0);
  rfEnc.write(0);
  Serial.begin(9600);
  
  //setup tuning step pin
  pinMode(stepPin, INPUT);
  stepSwitch.attach( stepPin );
  stepSwitch.interval(20);
  digitalWrite(stepPin,HIGH);

   //setup tuning step pin
  pinMode(step9Pin, INPUT);
  step9Switch.attach( step9Pin );
  step9Switch.interval(20);
  digitalWrite(step9Pin,HIGH);

   //setup tuning step pin
  pinMode(step10Pin, INPUT);
  step10Switch.attach( step10Pin );
  step10Switch.interval(20);
  digitalWrite(step10Pin,HIGH);


  initDisplay();
  //delay(2000);
  //Scheduler.startLoop(displayLoop);
}

void loop() {
  if(programMode==1) {
    
  } else {

    checkSerialData();

    long tunePosition = tuningEnc.read();
    if (tunePosition != 0) {
      tuningEnc.write(0);
      if(tunePosition<0) {
        if(function) {
          Serial.print("ZZSG;");
        } else {
          Serial.print("ZZSA;");
        }
      } else {
        if(function) {
          Serial.print("ZZSH;");
        } else {
          Serial.print("ZZSB;");
        }
      }
    }
  
    long afPosition = afEnc.read();
    if (afPosition != 0) {
      afEnc.write(0);
      if(afGain==-1) {
        Serial.print("ZZAG;");
      } else {
        if(afPosition<0) {
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
    }
  
    long rfPosition = rfEnc.read();
    if (rfPosition != 0) {
      rfEnc.write(0);
      if(rfGain==-1) {
        Serial.print("ZZPC;");
      } else {
        if(rfPosition<0) {
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
    }

    if(functionSwitch.update()) {
      if(functionSwitch.read()==1) {
        function=0;
      } else {
        function=1;
      }
    }
  
    if(stepSwitch.update()) {
      // note this switch has no pullup resistor
      if(stepSwitch.read()==1) {
        if(function) {
          Serial.print("ZZSD;");
        } else {
          Serial.print("ZZSU;");
        }
      }
    }
    
    if(bandSwitch.update()) {
      // note this switch has no pullup resistor
      if(bandSwitch.read()==1) {
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

    if(filterSwitch.update()) {
      if(filterSwitch.read()==0) {
        Serial.print("ZZFI;");
      }
    }

    if(nrSwitch.update()) {
      if(nrSwitch.read()==0) {
        if(function) {
          Serial.print("ZZNT;");
        } else {
          Serial.print("ZZNR;");
        }
      }
    }

    if(naSwitch.update()) {
      if(naSwitch.read()==0) {
        if(function) {
          Serial.print("ZZNB;");
        } else {
          Serial.print("ZZNA;");
        }
      }
    }
/*
    counter++;
    if(counter==100) {
      Serial.print("ZZFA;"); // get VFO A frequency
      //updateDisplay();
      counter=0;
    }
*/
    //yield();
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
        } else if(strncmp(message,"ZZFI",4)==0 && messageIndex==6) {
          int filter=((message[4]-'0')*10)+
                   (message[5]-'0');
          if(function) {
            filter--;
            if(filter<0) {
              filter=11;
            }
          } else {
            filter++;
            if(filter>11) {
              filter=0;
            }
          }
          Serial.print("ZZFI");
          Serial.print(filter/10);
          Serial.print(filter%10);
          Serial.print(";");
        } else if(strncmp(message,"ZZNR",4)==0 && messageIndex==5) {
          int state=message[4]-'0';
          if(state) {
            Serial.print("ZZNR0;");
          } else {
            Serial.print("ZZNR1;");
          }
        } else if(strncmp(message,"ZZNT",4)==0 && messageIndex==5) {
          int state=message[4]-'0';
          if(state) {
            Serial.print("ZZNT0;");
          } else {
            Serial.print("ZZNT1;");
          }
        } else if(strncmp(message,"ZZNA",4)==0 && messageIndex==5) {
          int state=message[4]-'0';
          if(state) {
            Serial.print("ZZNA0;");
          } else {
            Serial.print("ZZNA1;");
          }
        } else if(strncmp(message,"ZZNB",4)==0 && messageIndex==5) {
          int state=message[4]-'0';
          if(state) {
            Serial.print("ZZNB0;");
          } else {
            Serial.print("ZZNB1;");
          }
        } else {
            // unhandled message;
        }
        messageIndex=0;
      } else {
        message[messageIndex++]=c;
      }
   }
}

void displayLoop() {
  delay(1000);
  updateDisplay();
}


void initDisplay() {
  u8g.firstPage();
  do {
    u8g.setFont(u8g_font_profont11);
    u8g.setPrintPos(0,10);
    u8g.print("openHPSDR CAT Control");
    u8g.setFont(u8g_font_profont11);
    u8g.setPrintPos(0,30);
    u8g.print("Version 0.1");
    u8g.setPrintPos(0,40);
    u8g.print("by John Melton");
    u8g.setPrintPos(0,50);
    u8g.print("   g0orx/n6lyt");
    
  } while( u8g.nextPage());
}

void updateDisplay() {
  char f[32];
  u8g.firstPage();
  do {
    u8g.setFont(u8g_font_profont11);
    u8g.setPrintPos(0,10);
    u8g.print("openHPSDR CAT Control");
    u8g.setPrintPos(0,30);
    u8g.setFont(u8g_font_profont22);
    sprintf(f,"%03d.%03d.%03d",frequency/1000000, (frequency%1000000)/1000, frequency%1000);
    u8g.print(f);;
    u8g.setFont(u8g_font_profont11);
    u8g.setPrintPos(0,50);
    u8g.print("AF Gain: "); u8g.print(afGain);
    u8g.setFont(u8g_font_profont11);
    u8g.setPrintPos(0,60);
    u8g.print("RF Gain: "); u8g.print(rfGain);
  } while( u8g.nextPage());
}

void programDisplay() {
  u8g.firstPage();
  do {
    u8g.setFont(u8g_font_profont11);
    u8g.setPrintPos(0,10);
    u8g.print("Menu:");
  } while( u8g.nextPage()); 
}


