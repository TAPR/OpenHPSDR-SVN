//------------------------------------------------------------------------------
//The contents of this file are subject to the Mozilla Public License
//Version 1.1 (the "License"); you may not use this file except in compliance
//with the License. You may obtain a copy of the License at
//http://www.mozilla.org/MPL/ Software distributed under the License is
//distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express
//or implied. See the License for the specific language governing rights and
//limitations under the License.
//
//The Original Code is jrun.c.
//
//The Initial Developer of the Original Code is Alex Shovkoplyas, VE3NEA.
//Portions created by Alex Shovkoplyas are
//Copyright (C) 2008 Alex Shovkoplyas. All Rights Reserved.
//------------------------------------------------------------------------------

#include "../include/jrun.h"
#include "../include/hpsdr_rev1_regs.h"

#define MAX_DEVICE_COUNT 5
#define INIT_COUNT 200
#define CMD_LENGTH 10

#define JC_PROGRAM       0x002
#define JC_STARTUP       0x003
#define JC_CHECK_STATUS  0x004
#define JC_BYPASS        0x3FF


int jr_dev_cnt = 0;
int jr_dev_idx = 0;

long jr_bytes_written;
long jr_device_id;

//currently Cyclone II and III are supported. Set to 1 for (III) or 0 for (II)
Byte jr_is_CycloneIII;






//------------------------------------------------------------------------------
//                       read/write functions
//------------------------------------------------------------------------------

//write error code to the FX2 EP0 buffer
int jr_set_err(Byte err_code)
{
  EP0BUF[0] = err_code;
  EP0BCH = 0;
  EP0BCL = 1;
  return 1;
}


//set TDO bit, with optional TMS, and stropbe it with TCK
void jr_write_bit(BYTE signals)
{
  BYTE port_value = (IOE & 0xF0) | signals;
  IOE = port_value;
  IOE = port_value | bmTCK;
}


//write TDO/TMS as above, then read TDI
BYTE jr_rw_bit(BYTE signals)
{
  jr_write_bit(signals);
  if (IOE & bmTDI) return 1;
  else return 0;
}


//write <count> bits to TDO. If <is_last>, assert TMS when writing the last bit
void jr_write_bits(Word out_bits, Int8 count, Byte is_last)
{
  Int8 i;
  Byte signals;

  for (i=0; i < count; i++)
    {
	  signals = ((out_bits >> i) & 1)? bmTDO : 0;
    if (is_last && (i == (count-1))) signals |= bmTMS;
    jr_write_bit(signals);
    }
 }


//write TDO / read TDI multiple times, LSB first
Word jr_rw_bits(Word out_bits, Int8 count)
{
  Int8 i;
  Byte signals, in_bits = 0;

	for(i=0; i < count; i++)
	{
	  signals = ((out_bits >> i) & 1)? bmTDO : 0;
    in_bits |= jr_rw_bit(signals) << i;
  }
  return in_bits;
}


void jr_write_byte_fast(Byte out_byte)
{
  (void) out_byte;

  _asm
    mov r2, #0x01
    mov r3, #0x04
    mov r4, dpl

    mov a, r4
    anl a, r2
    mov	_IOE, a
    orl a, r3
    mov	_IOE, a

    mov a, r4
    rr a
    mov r4, a
    anl a, r2
    mov	_IOE, a
    orl a, r3
    mov	_IOE, a

    mov a, r4
    rr a
    mov r4, a
    anl a, r2
    mov	_IOE, a
    orl a, r3
    mov	_IOE, a

    mov a, r4
    rr a
    mov r4, a
    anl a, r2
    mov	_IOE, a
    orl a, r3
    mov	_IOE, a

    mov a, r4
    rr a
    mov r4, a
    anl a, r2
    mov	_IOE, a
    orl a, r3
    mov	_IOE, a

    mov a, r4
    rr a
    mov r4, a
    anl a, r2
    mov	_IOE, a
    orl a, r3
    mov	_IOE, a

    mov a, r4
    rr a
    mov r4, a
    anl a, r2
    mov	_IOE, a
    orl a, r3
    mov	_IOE, a

    mov a, r4
    rr a
    mov r4, a
    anl a, r2
    mov	_IOE, a
    orl a, r3
    mov	_IOE, a
  _endasm;
}






//------------------------------------------------------------------------------
//                       state machine functions
//------------------------------------------------------------------------------

//switch state machine to the Reset state
void jr_goto_Reset()
{
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
}


void jr_goto_Idle()
{
  jr_goto_Reset();
	jr_write_bit(0);
}

//switch state machine to the Shift-IR state
void jr_goto_ShiftIR()
{
  jr_goto_Idle();
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
	jr_write_bit(0);
	jr_write_bit(0);
}


void jr_goto_ShiftDR()
{
  jr_goto_Idle();
	jr_write_bit(bmTMS);
	jr_write_bit(0);
	jr_write_bit(0);
}


//load <cmd> in the device # <dev_idx>, load JC_BYPASS in all other devices
//switch to the Update-IR state
void jr_load_command(Word cmd, Int8 dev_idx)
{
  Int8 i;

  for(i=jr_dev_cnt-1; i >= 0; i--)
    jr_write_bits((i == dev_idx)? cmd : JC_BYPASS, CMD_LENGTH, i==0);

  //switch from Exit1-IR to Update-IR
	jr_write_bit(bmTMS);
}






//------------------------------------------------------------------------------
//                        chain analysis functions
//------------------------------------------------------------------------------

#define PROBE_PATTERN 0x0073


int jr_count_devices()
{
  int i;
  Word recv_bits;

  //load JC_BYPASS in all devices
	jr_dev_cnt = MAX_DEVICE_COUNT;
  jr_goto_ShiftIR();
  jr_load_command(JC_BYPASS, 0);

	//switch from Update-IR to Shift-DR
	jr_write_bit(bmTMS);
	jr_write_bit(0);
	jr_write_bit(0);

	//shift-in zeros to clear data registers in all devices
	for(i=0; i < (MAX_DEVICE_COUNT * 32); i++) jr_write_bit(0);

  //shift-in the probe pattern, shift-out recv_bits
	recv_bits = jr_rw_bits(PROBE_PATTERN, 8 + MAX_DEVICE_COUNT);

  //count LSB zeros in recv_bits
	jr_dev_cnt = -1;

	for(i=0; i < MAX_DEVICE_COUNT; i++)
    if (recv_bits & (1 << i)) {jr_dev_cnt = i; break;}

  //validate result
  if ((recv_bits >> jr_dev_cnt) != PROBE_PATTERN) jr_dev_cnt = -1;
  if (jr_dev_cnt >= 0) return 1; else return 0;
}


//read IDCODE from the selected device
void jr_read_device_id(Int8 dev_idx)
{
  Int8 d, b;
  Byte id[4];

  jr_goto_ShiftDR();

  //read device ID's, statring from the last device in the chain to <dev_idx>
  for (d=jr_dev_cnt-1; d >= dev_idx; d--)
    for (b=0; b < 4; b++)
      id[b] = jr_rw_bits(0, 8);

  //C is not my language, sorry
  jr_device_id = id[3];
  jr_device_id = (jr_device_id << 8) | id[2];
  jr_device_id = (jr_device_id << 8) | id[1];
  jr_device_id = (jr_device_id << 8) | id[0];
}


//validate device ID

//see the list of ID's in the jb_device.h file at
//https://www.altera.com/support/software/download/programming/jrunner/dnl-jrunner.html
Byte jr_validate_device_id()
{
  Byte jr_device_model;

  //only Cyclone II and Cyclone III are currently supported
  if ((jr_device_id & 0xFFF00FFF) != 0x020000DD) return 0;

  jr_device_model = jr_device_id >> 12;

  switch (jr_device_model & 0xF0)
  {
    case 0xB0: jr_is_CycloneIII = 0; break;  //Cyclone II
    case 0xF0: jr_is_CycloneIII = 1; break;  //Cyclone III
    default: return 0;
  }

  return 1;
}




//------------------------------------------------------------------------------
//                       published functions, query
//------------------------------------------------------------------------------

//read port E, return result in the FX2 EP0 buffer
int jr_read_port()
{
  EP0BUF[0] = IOE;
  EP0BCH = 0;
  EP0BCL = 1;
  return 1;
}


//write to port E
int jr_write_port(Byte signals)
{
  IOE = (IOE & 0xF0) | (signals & 0x0F);
  return 1;
}


///read ID codes of all devices
int jr_list_devices()
{
  Int8 i, j;

  //get device count
  if (!jr_count_devices()) return jr_set_err(JR_ERR_BROKEN_CHAIN);

  //read device ID's
  jr_goto_ShiftDR();

  for (i=0; i < jr_dev_cnt; i++)
    for (j=0; j < 4; j++)
      EP0BUF[-2+(jr_dev_cnt-i)*4 + j] = jr_rw_bits(0, 8);

  //return device list in the FX2 EP0 buffer
  EP0BUF[0] = JR_ERR_OK;
  EP0BUF[1] = jr_dev_cnt;
  EP0BCL = 2 + jr_dev_cnt * 4;
  return 1;
}






//------------------------------------------------------------------------------
//                    published functions, configure
//------------------------------------------------------------------------------
int jr_start_config(Int8 dev_idx)
{
  int i, clock_cnt;

  //validate device index
  if (!jr_count_devices()) return 0;
  if (dev_idx >= jr_dev_cnt) return 0;

  //validate device idcode
  jr_read_device_id(dev_idx);
  if (!jr_validate_device_id()) return 0;

  jr_goto_ShiftIR();
  jr_load_command(JC_PROGRAM, dev_idx);

	//switch from Update-IR to Shift-DR, keep TDO high
	jr_write_bit(bmTDO | bmTMS);
	jr_write_bit(bmTDO | 0);
	jr_write_bit(bmTDO | 0);

  //send a sequence of 1's
  if (jr_is_CycloneIII) clock_cnt = 3181; //Cyclone III
  else clock_cnt = 300;                   //Cyclone II

  for (i=0; i < clock_cnt; i++) jr_write_bit(bmTDO);

  jr_bytes_written = 0;
  return 1;
}


//data arrives in 64-byte blocks
int jr_xfer_config()
{
  Int8 bytes_new = EP0BCL;
  Int8 i;

  //skip the first 44 bytes in the rbf file for Cyclone and Cyclone II
  if (jr_is_CycloneIII || (jr_bytes_written >= 44)) i = 0;
  else i = 44 - jr_bytes_written;

  for (; i < bytes_new; i++) jr_write_byte_fast(EP0BUF[i]);

  jr_bytes_written += bytes_new;
  return 1;
}


int jr_end_config()
{
  int i;
  //int status_bit_idx;

  Byte is_ok;

  //write 16 bytes of 0xFF after the RBF data
  for (i=0; i<16; i++) jr_write_bits(0xFF, 8, 0);

  //switch from Shift-DR to Update-DR
	jr_write_bit(bmTDO | bmTMS);
	jr_write_bit(bmTMS);
	//switch to Idle
	jr_write_bit(0);


  //verify status

  is_ok = 1;

/*
  //this code is specific to Cyclone III EP3C25


  //switch from Idle or Reset to Shift-IR
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
	jr_write_bit(0);
	jr_write_bit(0);

  jr_load_command(JC_CHECK_STATUS, jr_dev_idx);

  //shift from Update-IR to Shift-DR
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
	jr_write_bit(0);
	jr_write_bit(0);


	status_bit_idx = ((244-149)*3)+1 + (jr_dev_cnt - jr_dev_idx-1);
  for (i=0; i < status_bit_idx; i++) is_ok = jr_rw_bit(0);

  //shift from Shift-DR to Update-DR
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
	//to Idle
	jr_write_bit(0);
*/



  //switch device to USER mode   ??do this for every device???

  //switch from Idle or Reset to Shift-IR
	jr_write_bit(0);
	jr_write_bit(bmTMS);
	jr_write_bit(bmTMS);
	jr_write_bit(0);
	jr_write_bit(0);

  jr_load_command(JC_STARTUP, jr_dev_idx);

  //switch from Update-IR to Idle
	jr_write_bit(0);

  for (i=0; i < INIT_COUNT; i++) jr_write_bit(bmTDO);
  //reset JSM
  jr_goto_Reset();


  return is_ok;
}
