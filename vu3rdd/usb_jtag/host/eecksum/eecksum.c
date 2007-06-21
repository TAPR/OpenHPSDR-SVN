#include <stdio.h>

extern unsigned char PROM[];

int main()
{
  int i = 0;
  unsigned short value, checksum = 0xAAAA;

  while(i<126)
  {
    value = PROM[i++];
    value += PROM[i++]<<8;

    checksum = value^checksum;
    checksum = (checksum << 1) | (checksum >> 15);
  }

  printf("0x%04X / 0x%02X,0x%02X\n", checksum, checksum&0xFF, (checksum>>8)&0xFF);

  return 0;
}

