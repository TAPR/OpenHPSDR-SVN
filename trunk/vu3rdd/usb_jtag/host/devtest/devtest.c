#include <stdio.h>
#include <stdlib.h>
#include <ftdi.h>
#include <stdint.h>
#include <string.h>

struct ftdi_context fc;

#define ALTERA_VID 0x09FB
#define BLASTER_PID 0x6001
#define USBJTAG_VID 0x16C0
#define USBJTAG_PID 0x06AD

void dev_deinit(void)
{ 
  ftdi_deinit(&fc);
}

void dev_close(void)
{
  ftdi_usb_close(&fc);
}

void dev_reset(void)
{
  ftdi_usb_reset(&fc);
}

int dev_error (char *s)
{
  fprintf (stderr, "%s: %s\n", s, ftdi_get_error_string(&fc));
  return -1;
}

int eeprom_test(void)
{
  uint8_t eeprom[128];

  if(ftdi_read_eeprom(&fc, eeprom) < 0)
  {
    (void)dev_error("ftdi_read_eeprom failed");
    return;
  };

  printf("=== EEPROM test ===\n");
  printf("  VID 0x%04X\n", eeprom[2] + (eeprom[3]<<8));
  printf("  PID 0x%04X\n", eeprom[4] + (eeprom[5]<<8));
  printf("  HW Version 0x%04X\n", eeprom[6] + (eeprom[7]<<8));
  printf("  USB Flags 0x%02X\n", eeprom[8]);
  printf("  Power 0x%02X\n", eeprom[9]);
  printf("  FTDI Flags 0x%02X\n", eeprom[10]);

  int i;
  for(i=0;i<128;i++) { printf(" %02X", eeprom[i]); }; printf("\n");

  return 0;
}

int jtag_test(void)
{
  int n;
  unsigned char buf[64];

  printf("=== JTAG test ===\n");

  memset(buf, 0, 64); // ensure bit bang mode in "CPLD"

#if 1
  n = ftdi_write_data(&fc, buf, 64);
  printf("n = %d\n", n);
  if(n < 0) return dev_error("ftdi_write_data(64 zeros) failed");

  buf[0] = 0x33;
  n = ftdi_write_data(&fc, buf, 1);
  printf("n = %d\n", n);
  if(n < 0) return dev_error("ftdi_write_data(0x33) failed");
#endif

#if 0
  n = ftdi_read_data(&fc, buf, 4);
  printf("n = %d, buf=%02X %02X %02X %02X\n", n, buf[0], buf[1], buf[2], buf[3]);
  if(n < 0) (void)dev_error("ftdi_read_data failed");
#endif
}

int asmi_test(void)
{
  printf("=== ASMI test ===\n  not implemented yet\n");
  return 0;
}

int main(int argc, char *argv[])
{
  ftdi_init(&fc);
#if 0
#warning using nonstandard ep numbers
  fc.in_ep = 0x01;
  fc.out_ep = 0x81;
#endif

  atexit(dev_deinit);

  printf("=== Search ixo.de adapter ===\n");

  if(ftdi_usb_open(&fc, USBJTAG_VID, USBJTAG_PID) < 0)
  {
    (void)dev_error("ftdi_usb_open failed");
  
    printf("=== Search Altera adapter ===\n");
    if(ftdi_usb_open(&fc, ALTERA_VID, BLASTER_PID) < 0)
    {
      return dev_error("ftdi_usb_open failed");
    };
  };

  atexit(dev_close);

  if(eeprom_test() < 0) return -1;

  if(ftdi_disable_bitbang(&fc) < 0)
  {
    return dev_error("unable to disable bitbang mode"); 
  };

  if (ftdi_set_latency_timer(&fc, 2) < 0)
  {
    return dev_error("unable to set latency timer");
  };

  {
    unsigned  char latency_timer;
    if (ftdi_get_latency_timer(&fc, &latency_timer) < 0)
    {
      return dev_error("unable to get latency timer");
    }
    printf("current latency timer: %i\n", latency_timer);
  };

  /* TODO: libftdi's ftdi_set_baudrate chokes on high rates, use lowlevel
   * usb function instead! And additionally allow user to throttle. */
  if(ftdi_set_baudrate(&fc, 3000000/4)<0)
  {
    return dev_error("unable to set baud rate to max");
  };

  if(jtag_test() < 0) return -1;
  if(asmi_test() < 0) return -1;

  return 0;
}
