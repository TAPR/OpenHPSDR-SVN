#include <stdio.h>

int main(int argc, char **argv)
{
	if (argc != 2)
	{
		fprintf(stderr, "Usage:  jbc2data <filename>\n");
	}
	else
	{
		FILE *fp = fopen(argv[1], "rb");

		if (fp == NULL)
		{
			fprintf(stderr, "Error: can't open file \"%s\"\n", argv[1]);
		}
		else
		{
			int c, i = 0;

			printf("#ifndef JBC_FILE_IN_RAM\n");
			printf("unsigned char CONSTANT_AREA jbi_program[] =\n");
			printf("{\n");

			while ((c = fgetc(fp)) != EOF)
			{
				if ((i % 12) == 0) printf("\t");	/* tab char */
				printf("0x%02X, ", c);
				if ((i % 12) == 11) printf("\n");
				++i;
			}

			if ((i % 12) != 0) printf("\n");
			printf("};\n");
			printf("#endif /* JBC_FILE_IN_RAM */\n");

			fclose(fp);
		}
	}

	return 0;
}
