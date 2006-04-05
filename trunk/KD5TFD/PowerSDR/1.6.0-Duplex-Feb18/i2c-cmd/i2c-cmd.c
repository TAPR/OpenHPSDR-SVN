#include <stdio.h> 
#include <stdlib.h> 
#include <KD5TFD-VK6APH-Audio.h> 

void doHelp() { 
	printf("i2c-cmd i2c_addr cmdbytes\n  all values in hex\n"); 
} 



void print_command(FILE *f, unsigned char addr, unsigned char cmdbuf[], unsigned char buflen)  { 
	int i; 
	printf("addr: 0x%02x cmd:", addr); 
	for ( i = 0; i < buflen; i++ ) { 
		printf(" 0x%02x", cmdbuf[i]); 
	} 
	return; 
} 

int main(int argc, char *argv[]) { 
	unsigned char addr; 
	unsigned long l; 
	unsigned char cmdbuf[65]; 
	int i; 
	int rc;

	if ( argc <= 2 ) { 
		doHelp(); 
		exit(101); 
	}

	l = strtoul(argv[1], NULL, 16); 
	if ( l > 255 ) { 
		printf("bad address: %s\n", argv[1]); 
		doHelp(); 
		exit(102); 
	} 
	/* else */ 
	addr = (unsigned char)l; 

	for ( i = 0; i < argc - 2; i++ ) { 
		l = strtoul(argv[2+i], NULL, 16); 
		if ( l > 255 ) { 
			printf("bad command byte: %s\n", argv[2+i]); 
			doHelp(); 
			exit(103); 
		} 
		/* else */ 
		cmdbuf[i] = (unsigned char)l; 
	} 
	printf("i2c-cmd "); 
	print_command(stdout, addr, cmdbuf, argc-2); 
	printf("..."); 
	rc = I2C_Cmd(addr, cmdbuf, argc-2); 
	printf(" %s\n", I2C_RCtoString(rc)); 
	exit(rc); 
}