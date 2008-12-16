#ifndef _JRUN_H_
#define _JRUN_H_

#define Int8 char
#define Byte unsigned char
#define Word unsigned int

#define JR_ERR_OK 0
#define JR_ERR_BROKEN_CHAIN   1



extern int jr_dev_cnt;
extern int jr_dev_idx;

int jr_read_port();
int jr_write_port(Byte signals);
int jr_list_devices();
int jr_start_config(Int8 dev_idx);
int jr_xfer_config();
int jr_end_config();


#endif

