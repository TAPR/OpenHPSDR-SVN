/*
 *  connection.h
 *  SDR
 *
 *  Created by John Melton on 02/07/2009.
 *  Copyright 2009 G0ORX. All rights reserved.
 *
 */

char host[64];
int port;

int currentBand;
int displayedBand;

void setHost(char * id);
void setPort(int p);
void makeConnection();
void disconnect();
void newConnection(const char *newHost,const char *newPort);
int isConnected();
void sendCommand(char *command);


