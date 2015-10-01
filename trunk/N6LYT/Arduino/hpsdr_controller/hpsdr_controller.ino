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
*  HPSDR radio controller using Arduino DUE
*  using step endcoders and push buttons.
*  
*  sends CAT comands over USB serial port
*
*  Uses the encoder library from http://www.pjrc.com/teensy/td_libs_Encoder.html
*  Uses the bounce2 library from https://github.com/thomasfredericks/Bounce2/wiki
*  Uses the UTFT library from http://www.rinkydinkelectronics.com/library.php?id=51
*/

#include <Bounce2.h>
#include <Encoder.h>
#include <UTFT.h>
#include <memorysaver.h>

// declare the display
UTFT myGLCD(ILI9325D_16,25,26,27,28);

// declare the rotary encoders
Encoder tuningEnc(2, 3);
Encoder rfEnc(4,5);
Encoder afEnc(6,7);

// declare the switches
#define stepPin 8
Bounce stepSwitch = Bounce();

#define step9Pin 9
Bounce step9Switch = Bounce();

#define step10Pin 10
Bounce step10Switch = Bounce();

#define functionPin 14
Bounce functionSwitch = Bounce();

#define bandPin 15
Bounce bandSwitch = Bounce();

#define modePin 16
Bounce modeSwitch = Bounce();

#define filterPin 17
Bounce filterSwitch = Bounce();

#define NBPin 18
Bounce NBSwitch = Bounce();

#define pttPin 19
Bounce pttSwitch = Bounce();

// declare the fonts we use
extern uint8_t SmallFont[];
extern uint8_t BigFont[];
extern uint8_t SevenSegmentFull[];

int afGain=-1;
int rfGain=-1;
int agcGain=-200;

int function = 0;

int ptt=0;

char message[32];
int messageIndex=0;

long frequency=0;
int frequencyChanged=0;

unsigned long smetertimer=0;
int dbm=-127;

int mode=1;

int resetting=0;

void setup() {

  // setup serial port
  Serial.begin(9600);

  // setup the display
  myGLCD.InitLCD();
  myGLCD.setFont(SmallFont);

  myGLCD.clrScr();

  myGLCD.setColor(0, 0, 0);
  myGLCD.fillRect(0, 0, 319, 239);

  //myGLCD.setColor(255, 255, 255);
  //myGLCD.print("openHPSDR CAT controller", CENTER, 0);
  //myGLCD.print("by John Melton g0orx/n6lyt", CENTER, 12);

  // initialize the display
  drawFrequency();
  initSMeter();
  initAFGain();
  initRFGain();
  initAGCGain();

  drawMode();
  //myGLCD.setFont(BigFont);
  //myGLCD.print("USB", RIGHT, 60);
  //myGLCD.print("2.4K",RIGHT,80);

  //setup tuning step pin
  pinMode(stepPin, INPUT); // input pin
  stepSwitch.attach( stepPin ); // attach to switch
  stepSwitch.interval(20); // 20ms settle time
  digitalWrite(stepPin,HIGH); // enable internal pull up

  // setup function up pin
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

  // setup filter pin
  pinMode(filterPin, INPUT);
  filterSwitch.attach(filterPin);
  filterSwitch.interval(20);
  digitalWrite(filterPin, HIGH);

  // setup NB pin
  pinMode(NBPin, INPUT);
  NBSwitch.attach(NBPin);
  NBSwitch.interval(20);
  digitalWrite(NBPin, HIGH);

  // setup PTT pin
  pinMode(pttPin, INPUT);
  pttSwitch.attach(pttPin);
  pttSwitch.interval(20);
  digitalWrite(pttPin, HIGH);
  
  reset();

  smetertimer=millis()+100L; // every 100 ms
  
}


void loop() {

  checkSerialData();

  if(functionSwitch.update()) {
    if(functionSwitch.read()==1) {
      function=0;
    } else { 
      function=1;
    }
  }
  
  long tunePosition = tuningEnc.read();
    if (tunePosition != 0) {
      tuningEnc.write(0);
      if(tunePosition<0) {
        if(function) {
          Serial.print("ZZSH;");
        } else {
          Serial.print("ZZSB;ZZFA;");
        }
      } else {
        if(function) {
          Serial.print("ZZSG;");
        } else {
          Serial.print("ZZSA;ZZFA;");
        }
      }
    }

    long afPosition = afEnc.read();
    if (afPosition != 0) {
      afEnc.write(0);
      if(function) {
        if(agcGain==-200) {
          Serial.print("ZZAR;");
        } else {
          if(afPosition<0) {
            agcGain--;
            if(agcGain<-20) {
              agcGain=-20;
            }
          } else {
            agcGain++;
            if(agcGain>120) {
              agcGain=120;
            }
          }
          Serial.print("ZZAR");
          int gain=agcGain;
          if(gain<0) {
            Serial.print("-");
            gain=-gain;
          } else {
            Serial.print("+");
          }
          Serial.print(gain/100);
          Serial.print((gain%100)/10);
          Serial.print(gain%10);
          Serial.print(";");
          drawAGCGain();
        }
      } else {
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
          drawAFGain();
        }
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
        drawRFGain();
     }
  }

  if(pttSwitch.update()) {
    if(pttSwitch.read()==0) {
      if(ptt) {
        ptt=0;
        if(function) {
          Serial.print("ZZTU1;");
        } else {
          Serial.print("ZZTX1;");
        }
      }
    } else {
      if(!ptt) {
        ptt=1;
        if(function) {
          Serial.print("ZZTU0;");
        } else {
          Serial.print("ZZTX0;");
        }
      }
    }
  }
  
  if(bandSwitch.update()) {
    if(bandSwitch.read()==1) {
      resetting=1;
      if(function) {
          Serial.print("ZZBD;ZZFA;ZZMD;");
      } else {
        Serial.print("ZZBU;ZZFA;ZZMD;");
      }
    }
  }

  if(modeSwitch.update()) {
    if(modeSwitch.read()==0) {
      Serial.print("ZZMD;"); // get the mode
    }
  }

  if(filterSwitch.update()) {
    if(filterSwitch.read()==0) {
      Serial.print("ZZFI;");
    }
  }

  if(NBSwitch.update()) {
    if(NBSwitch.read()==0) {
      if(function) {
        Serial.print("ZZNT;");
      } else {
        Serial.print("ZZNR;");
      }
    }
  }
    
  if(stepSwitch.update()) {
    if(function) {
      reset();
    } else {
      if(stepSwitch.read()==1) {
        if(function) {
          Serial.print("ZZSD;");
        } else {
          Serial.print("ZZSU;");
        }
      }
    }
  }
   
   if(frequencyChanged) {
     drawFrequency();
     frequencyChanged=0;
   }

   sMeter();
   
}

void reset() {
  resetting=1;
  // initialize frequency
  Serial.print("ZZFA;");
  // intitialize AF Gain
  Serial.print("ZZAG;");
  // initialize RF Gain
  Serial.print("ZZPC;");
  // initialize AGC Gain
  Serial.print("ZZAR;");
  // initialize mode
  Serial.print("ZZMD;");
}

void sMeter() {
  if(smetertimer==0) {
    smetertimer=millis()+100L;
  } else {
    if(millis()>smetertimer) {
      Serial.print("ZZSM0;");
      Serial.print("ZZFA;");
      smetertimer=millis()+100L;
    }
  }
}

void drawFrequency() {
  myGLCD.setFont(SevenSegmentFull);
  myGLCD.setColor(0,255,0);
  char cf[20];
  int mhz=frequency/1000000;
  int khz=(frequency%1000000)/1000;
  int hz=frequency%1000;
  sprintf(cf,"%0d.%03d.%03d", mhz,khz,hz);
  myGLCD.print(cf, RIGHT, 0);
}

void drawMode() {
  char m[8];
  myGLCD.setFont(BigFont);
  myGLCD.setColor(255,255,255);
  switch(mode) {
    case 0:
      strcpy(m,"LSB");
      break;
    case 1:
      strcpy(m,"USB");
      break;
    case 2:
      strcpy(m,"DSB");
      break;
    case 3:
      strcpy(m,"CWL");
      break;
    case 4:
      strcpy(m,"CWU");
      break;
    case 5:
      strcpy(m,"FM");
      break;
    case 6:
      strcpy(m,"AM");
      break;
    case 7:
      strcpy(m,"DIGU");
      break;
    case 8:
      strcpy(m,"SPEC");
      break;
    case 9:
      strcpy(m,"DIGL");
      break;
    case 10:
      strcpy(m,"SAM");
      break;
    case 11:
      strcpy(m,"DRM");
      break;
    default:
      strcpy(m,"???");
      break;
  }
  myGLCD.print(m, RIGHT, 60);
}

void initAFGain() {
  myGLCD.setFont(SmallFont);
  myGLCD.setColor(255,255,255);
  myGLCD.print("AF Gain", 10, 125);
  myGLCD.setColor(255, 255, 255);
  myGLCD.drawLine(80,140,280,140);
  myGLCD.print("0", 70, 135);
  myGLCD.print("100", 285, 135);
}

void drawAFGain() {
  int x=80+(afGain*2);

  myGLCD.setColor(0,0,0);
  myGLCD.fillRect(80, 120, 280, 138);
  myGLCD.setColor(255,255,0);
  myGLCD.fillRect(80, 120, x, 138);
}

void initRFGain() {
  myGLCD.setFont(SmallFont);
  myGLCD.setColor(255,255,255);
  myGLCD.print("RF Gain", 10, 155);
  myGLCD.setColor(255, 255, 255);
  myGLCD.drawLine(80,170,280,170);
  myGLCD.print("0", 70, 165);
  myGLCD.print("100", 285, 165);
}

void drawRFGain() {
  int x=80+(rfGain*2);
  if(x<80) x=80;
  
  myGLCD.setColor(0,0,0);
  myGLCD.fillRect(80, 150, 280, 168);
  myGLCD.setColor(255, 0, 0);
  myGLCD.fillRect(80, 150, x, 168);
}

void initAGCGain() {
  myGLCD.setFont(SmallFont);
  myGLCD.setColor(255,255,255);
  myGLCD.print("AGC Gain", 10, 185);
  myGLCD.setColor(255, 255, 255);
  myGLCD.drawLine(100,200,240,200);
  myGLCD.print("-20", 70, 195);
  myGLCD.print("120", 245, 195);
}

void drawAGCGain() {
  int x=100+(agcGain+20);

  myGLCD.setColor(0,0,0);
  myGLCD.fillRect(100, 180, 240, 198);
  myGLCD.setColor(0,255,0);
  myGLCD.fillRect(100, 180, x, 198);
}


void initSMeter() {
  myGLCD.setColor(0,255,0);
  myGLCD.drawLine(10,85,64,85);
  myGLCD.drawLine(10,85,10,80); // S0
  myGLCD.drawLine(16,85,16,82); // S1
  myGLCD.drawLine(22,85,22,82); // S2
  myGLCD.drawLine(28,85,28,80); // S3
  myGLCD.drawLine(34,85,34,82); // S4
  myGLCD.drawLine(40,85,40,82); // S5
  myGLCD.drawLine(46,85,46,80); // S6
  myGLCD.drawLine(52,85,52,82); // S7
  myGLCD.drawLine(58,85,58,82); // S8
  
  myGLCD.setColor(255,0,0);
  myGLCD.drawLine(64,85,124,85);
  myGLCD.drawLine(64,85,64,80); // S9
  myGLCD.drawLine(84,85,84,80); // S9+20
  myGLCD.drawLine(104,85,104,80); // S9+40
  myGLCD.drawLine(124,85,124,80); // S9+60
  
  myGLCD.setFont(SmallFont);
  myGLCD.setColor(255,255,255);
  myGLCD.print("3",26,87);
  myGLCD.print("6",44,87);
  myGLCD.setColor(255,0,0);
  myGLCD.print("9",62,87);
  //myGLCD.print("+20",84,87);
  //myGLCD.print("+40",104,87);
  myGLCD.print("+60",120,87);
  
}

void drawSMeter() {
  myGLCD.setColor(0,0,0);
  myGLCD.fillRect(10,75,124,79);
  myGLCD.setColor(255,255,0);
  myGLCD.fillRect(10,75,10+127+dbm,79);
  
  myGLCD.setFont(SmallFont);
  myGLCD.setColor(255,255,255);
  char s[32];
  sprintf(s, "%d dBm", dbm);
  myGLCD.print(s, 130, 75);
}

void checkSerialData() {
   while(Serial.available() > 0) {
      // read the incoming byte:
      char c=Serial.read();
      if(c==';') {
        message[messageIndex]='\0';
        if(strncmp(message,"ZZFA",4)==0 && messageIndex==15) {
          frequency=0;
          for(int i=4;i<15;i++) {
            frequency=(frequency*10L)+(long)(message[i]-'0');
          }
          frequencyChanged=1;
        } else if(strncmp(message,"ZZAG",4)==0 && messageIndex==7) {
            int gain=((message[4]-'0')*100)+
                     ((message[5]-'0')*10)+
                     (message[6]-'0');
            afGain=gain;
            drawAFGain();
        } else if(strncmp(message,"ZZPC",4)==0 && messageIndex==7) {
            int gain=((message[4]-'0')*100)+
                     ((message[5]-'0')*10)+
                     (message[6]-'0');
            rfGain=gain;
            drawRFGain();
        } else if(strncmp(message,"ZZAR",4)==0 && messageIndex==8) {
            int gain=((message[5]-'0')*100)+
                     ((message[6]-'0')*10)+
                     (message[7]-'0');
            if(message[4]=='-') {
              gain=-gain;
            }
            agcGain=gain;
            drawAGCGain();
        } else if(strncmp(message,"ZZMD",4)==0 && messageIndex==6) {
          mode=((message[4]-'0')*10)+
                   (message[5]-'0');
          if(resetting) {
            resetting=0;
          } else {
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
          }
          drawMode();
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
        } else if(strncmp(message,"ZZSM",4)==0 && messageIndex==8) {
          int rx=message[4]-'0';
          if(rx==0) {
            dbm=(((message[5]-'0')*100)+
                     ((message[6]-'0')*10)+
                     (message[7]-'0'))/2 - 140;
            if(dbm>-13) {
              dbm=-13;
            } else if(dbm<-127) {
              dbm=-127;
            }
            
            drawSMeter();
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
