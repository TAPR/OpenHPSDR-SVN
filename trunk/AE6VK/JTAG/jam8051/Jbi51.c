/****************************************************************************/
/*																			*/
/*	Module:			jbi51.c													*/
/*																			*/
/*					Copyright (C) Altera Corporation 1998					*/
/*																			*/
/*	Description:	Main source file for JAM Byte Code Player for 8051		*/
/*																			*/
/****************************************************************************/

/* header file with register definitions for Dallas DS87C520 microprocessor */
#include <ds8xc520.h>

/* header files needed for console I/O functions (like printf) */
#include <stdio.h>
#include <stdlib.h>

/****************************************************************************/
/*																			*/
/*	Port-specific Definitions												*/
/*																			*/
/****************************************************************************/

/*
*	Modifiers to select addressing mode for data pointers for the 8051
*	microprocessor.  These keywords (code, xdata, pdata, idata, data) are
*	compatible with the Keil C51 compiler for the 8051.
*
*	Note that the pointer for program storage is not passed as a parameter,
*	but is provided as a global variable.
*/
#define CONSTANT_AREA code
#define XDATA_AREA xdata /* external RAM accessed by 16-bit pointer */
#define PDATA_AREA pdata /* external RAM accessed by 8-bit pointer */
#define IDATA_AREA idata /* internal RAM accessed by 8-bit pointer */
#define RDATA_AREA data  /* internal RAM registers with direct access */
#define BIT bit          /* internal RAM single bit data type */

/*
*	By default, the JBC file is located in the code memory.  To locate the
*	JBC file in the external data (xdata) area, define JBC_FILE_IN_RAM=1.
*/
#ifdef JBC_FILE_IN_RAM
#define FILE_AREA XDATA_AREA
#define FILE_OR_XDATA_AREA XDATA_AREA
#else
#define FILE_AREA CONSTANT_AREA
#define FILE_OR_XDATA_AREA
#endif /* JBC_FILE_IN_RAM */

/****************************************************************************/
/*																			*/
/*	Constant definitions													*/
/*																			*/
/****************************************************************************/

#define JBI_MAX_PROGRAM_SIZE (48 * 1024)	/* when JBC file is in RAM */

#define JBI_STACK_SIZE 128

#define JBI_HEAP_SIZE (3 * 1024)

#define JBI_MESSAGE_LENGTH 255

#define JBI_VARIABLE_COUNT 150

/* maximum JTAG IR and DR lengths (in bits) */
#define JBIC_MAX_JTAG_IR_PREAMBLE   128
#define JBIC_MAX_JTAG_IR_POSTAMBLE  128
#define JBIC_MAX_JTAG_IR_LENGTH     256
#define JBIC_MAX_JTAG_DR_PREAMBLE   128
#define JBIC_MAX_JTAG_DR_POSTAMBLE  128
#define JBIC_MAX_JTAG_DR_LENGTH    2048

#define JBI_RETURN_TYPE unsigned char

#define JBIC_SUCCESS            0
#define JBIC_OUT_OF_MEMORY      1
#define JBIC_STACK_OVERFLOW     2
#define JBIC_IO_ERROR           3
#define JBIC_UNEXPECTED_END     4
#define JBIC_ILLEGAL_OPCODE     5
#define JBIC_INTEGER_OVERFLOW   6
#define JBIC_DIVIDE_BY_ZERO     7
#define JBIC_CRC_ERROR          8
#define JBIC_INTERNAL_ERROR     9
#define JBIC_BOUNDS_ERROR      10
#define JBIC_VECTOR_MAP_FAILED 11
#define JBIC_USER_ABORT        12
#define JBIC_UNDEFINED_SYMBOL  13

#define JBI_JTAG_STATE unsigned char

#define RESET      0
#define IDLE       1
#define DRSELECT   2
#define DRCAPTURE  3
#define DRSHIFT    4
#define DREXIT1    5
#define DRPAUSE    6
#define DREXIT2    7
#define DRUPDATE   8
#define IRSELECT   9
#define IRCAPTURE 10
#define IRSHIFT   11
#define IREXIT1   12
#define IRPAUSE   13
#define IREXIT2   14
#define IRUPDATE  15
#define ILLEGAL_JTAG_STATE 16

/* flag bits for jbi_jtag_io() function */
#define TMS_HIGH   1
#define TMS_LOW    0
#define TDI_HIGH   1
#define TDI_LOW    0
#define READ_TDO   1
#define IGNORE_TDO 0

/*
*	ACA data decompression constants
*/
#define	SHORT_BITS			16
#define	CHAR_BITS			8
#define	DATA_BLOB_LENGTH	3
#define	MATCH_DATA_LENGTH	8192
#define JBI_ACA_REQUEST_SIZE 1024
#define JBI_ACA_BUFFER_SIZE	(MATCH_DATA_LENGTH + JBI_ACA_REQUEST_SIZE)

/****************************************************************************/
/*																			*/
/*	Type definitions														*/
/*																			*/
/****************************************************************************/

/*
*	This structure shows, for each JTAG state, which state is reached after
*	a single TCK clock cycle with TMS high or TMS low, respectively.  This
*	describes all possible state transitions in the JTAG state machine.
*/
struct JBIS_JTAG_MACHINE
{
	JBI_JTAG_STATE tms_high;
	JBI_JTAG_STATE tms_low;
}
CONSTANT_AREA jbi_jtag_state_transitions[] =
{
/* RESET     */	{ RESET,	IDLE },
/* IDLE      */	{ DRSELECT,	IDLE },
/* DRSELECT  */	{ IRSELECT,	DRCAPTURE },
/* DRCAPTURE */	{ DREXIT1,	DRSHIFT },
/* DRSHIFT   */	{ DREXIT1,	DRSHIFT },
/* DREXIT1   */	{ DRUPDATE,	DRPAUSE },
/* DRPAUSE   */	{ DREXIT2,	DRPAUSE },
/* DREXIT2   */	{ DRUPDATE,	DRSHIFT },
/* DRUPDATE  */	{ DRSELECT,	IDLE },
/* IRSELECT  */	{ RESET,	IRCAPTURE },
/* IRCAPTURE */	{ IREXIT1,	IRSHIFT },
/* IRSHIFT   */	{ IREXIT1,	IRSHIFT },
/* IREXIT1   */	{ IRUPDATE,	IRPAUSE },
/* IRPAUSE   */	{ IREXIT2,	IRPAUSE },
/* IREXIT2   */	{ IRUPDATE,	IRSHIFT },
/* IRUPDATE  */	{ DRSELECT,	IDLE }
};

/*
*	This table contains the TMS value to be used to take the NEXT STEP on
*	the path to the desired state.  The array index is the current state,
*	and the bit position is the desired endstate.  To find out which state
*	is used as the intermediate state, look up the TMS value in the
*	jbi_jtag_state_transitions[] table.
*/
unsigned int CONSTANT_AREA jbi_jtag_path_map[16] =
{
	0x0001, 0xFFFD, 0xFE01, 0xFFE7, 0xFFEF, 0xFF0F, 0xFFBF, 0xFF0F,
	0xFEFD, 0x0001, 0xF3FF, 0xF7FF, 0x87FF, 0xDFFF, 0x87FF, 0x7FFD
};

/****************************************************************************/
/*																			*/
/*	Macros																	*/
/*																			*/
/****************************************************************************/

#define GET_WORD(x) jbi_get_program_word(x)

#define GET_DWORD(x) jbi_get_program_dword(x)

/*
*	This macro checks if enough parameters are available on the stack. The
*	argument is the number of parameters needed.
*/
#ifdef DEBUG

#define IF_CHECK_STACK(x) \
	if (jbi_stack_ptr < (x)) \
	{ \
		done = 1; \
		status = JBIC_STACK_OVERFLOW; \
	} \
	else

#else
/* suppress stack checking */
#define IF_CHECK_STACK(x)
#endif /* DEBUG */

/*
*	This macro checks if a code address is inside the code section
*/
#ifdef DEBUG

#define CHECK_PC \
	if ((pc < code_section) || (pc >= debug_section)) \
	{ \
		done = 1; \
		status = JBIC_BOUNDS_ERROR; \
	}

#else
/* suppress PC checking */
#define CHECK_PC
#endif /* DEBUG */

/****************************************************************************/
/*																			*/
/*	Global variables														*/
/*																			*/
/****************************************************************************/

#ifdef JBC_FILE_IN_RAM
unsigned char FILE_AREA jbi_program[JBI_MAX_PROGRAM_SIZE];
#else
extern unsigned char FILE_AREA jbi_program[];
#endif /* JBC_FILE_IN_RAM */

/*
*	Initialization list to control program execution
*/
char CONSTANT_AREA jbi_do_erase_0[]      = "DO_ERASE=0";
char CONSTANT_AREA jbi_do_erase_1[]      = "DO_ERASE=1";
char CONSTANT_AREA jbi_do_blankcheck_0[] = "DO_BLANKCHECK=0";
char CONSTANT_AREA jbi_do_blankcheck_1[] = "DO_BLANKCHECK=1";
char CONSTANT_AREA jbi_do_program_0[]    = "DO_PROGRAM=0";
char CONSTANT_AREA jbi_do_program_1[]    = "DO_PROGRAM=1";
char CONSTANT_AREA jbi_do_verify_0[]     = "DO_VERIFY=0";
char CONSTANT_AREA jbi_do_verify_1[]     = "DO_VERIFY=1";
char CONSTANT_AREA jbi_do_secure_0[]     = "DO_SECURE=0";
char CONSTANT_AREA jbi_do_secure_all_0[] = "DO_SECURE_ALL=0";
char CONSTANT_AREA jbi_do_configure_0[]  = "DO_CONFIGURE=0";

char CONSTANT_AREA * CONSTANT_AREA jbi_init_list_no_op[] =
{
	jbi_do_erase_0,
	jbi_do_blankcheck_0,
	jbi_do_program_0,
	jbi_do_verify_0,
	jbi_do_secure_0,
	jbi_do_secure_all_0,
	jbi_do_configure_0,
	NULL
};

char CONSTANT_AREA * CONSTANT_AREA jbi_init_list_erase[] =
{
	jbi_do_erase_1,
	jbi_do_blankcheck_0,
	jbi_do_program_0,
	jbi_do_verify_0,
	jbi_do_secure_0,
	jbi_do_secure_all_0,
	jbi_do_configure_0,
	NULL
};

char CONSTANT_AREA * CONSTANT_AREA jbi_init_list_blankcheck[] =
{
	jbi_do_erase_0,
	jbi_do_blankcheck_1,
	jbi_do_program_0,
	jbi_do_verify_0,
	jbi_do_secure_0,
	jbi_do_secure_all_0,
	jbi_do_configure_0,
	NULL
};

char CONSTANT_AREA * CONSTANT_AREA jbi_init_list_program[] =
{
	jbi_do_erase_1,
	jbi_do_blankcheck_0,
	jbi_do_program_1,
	jbi_do_verify_0,
	jbi_do_secure_0,
	jbi_do_secure_all_0,
	jbi_do_configure_0,
	NULL
};

char CONSTANT_AREA * CONSTANT_AREA jbi_init_list_verify[] =
{
	jbi_do_erase_0,
	jbi_do_blankcheck_0,
	jbi_do_program_0,
	jbi_do_verify_1,
	jbi_do_secure_0,
	jbi_do_secure_all_0,
	jbi_do_configure_0,
	NULL
};

char CONSTANT_AREA * CONSTANT_AREA jbi_init_list_all[] =
{
	jbi_do_erase_1,
	jbi_do_blankcheck_1,
	jbi_do_program_1,
	jbi_do_verify_1,
	jbi_do_secure_0,
	jbi_do_secure_all_0,
	jbi_do_configure_0,
	NULL
};

/*
*	Main stack for program execution
*/
unsigned char RDATA_AREA jbi_stack_ptr;
long XDATA_AREA jbi_stack[JBI_STACK_SIZE];

/*
*	Heap area for dynamic allocation
*/
unsigned int XDATA_AREA jbi_current_heap_ptr;
char XDATA_AREA jbi_heap[JBI_HEAP_SIZE];

/*
*	ACA data decompression variables and output buffer
*/
int XDATA_AREA jbi_current_variable_id;
int XDATA_AREA jbi_current_page;
unsigned char XDATA_AREA jbi_bits_avail;
unsigned int  XDATA_AREA jbi_in_length;
unsigned long XDATA_AREA jbi_out_length;
unsigned int  XDATA_AREA jbi_in_index;	/* byte index into compressed array */
unsigned int  XDATA_AREA jbi_out_index;	/* byte index into jbi_aca_out_buffer */
unsigned char FILE_AREA * XDATA_AREA jbi_aca_in_buffer;
unsigned char XDATA_AREA jbi_aca_out_buffer[JBI_ACA_BUFFER_SIZE];

/*
*	Global variable to store the current JTAG state
*/
JBI_JTAG_STATE jbi_jtag_state;

/*
*	Store current stop-state for DR and IR scan commands
*/
JBI_JTAG_STATE jbi_drstop_state;
JBI_JTAG_STATE jbi_irstop_state;

/*
*	Store current padding values
*/
unsigned int XDATA_AREA jbi_dr_preamble_count;
unsigned int XDATA_AREA jbi_dr_postamble_count;
unsigned int XDATA_AREA jbi_ir_preamble_count;
unsigned int XDATA_AREA jbi_ir_postamble_count;

unsigned char XDATA_AREA jbi_dr_preamble_data[JBIC_MAX_JTAG_DR_PREAMBLE / 8];
unsigned char XDATA_AREA jbi_dr_postamble_data[JBIC_MAX_JTAG_DR_POSTAMBLE / 8];
unsigned char XDATA_AREA jbi_ir_preamble_data[JBIC_MAX_JTAG_IR_PREAMBLE / 8];
unsigned char XDATA_AREA jbi_ir_postamble_data[JBIC_MAX_JTAG_IR_POSTAMBLE / 8];
unsigned char XDATA_AREA jbi_dr_buffer[JBIC_MAX_JTAG_DR_LENGTH / 8];
unsigned char XDATA_AREA jbi_ir_buffer[JBIC_MAX_JTAG_IR_LENGTH / 8];

/*
*	Buffers for note key and value strings
*/
char XDATA_AREA jbi_note_key[33];
char XDATA_AREA jbi_note_value[257];

/*
*	Timer interrupt counters for "time elapsed" message
*/
unsigned int RDATA_AREA jbi_timer_count;
unsigned int XDATA_AREA jbi_timer_seconds;

/*
*	Serial communication interrupt variables
*/
char XDATA_AREA jbi_comm_buffer[256];
unsigned char RDATA_AREA jbi_comm_in_position;
unsigned char RDATA_AREA jbi_comm_out_position;
unsigned char XDATA_AREA jbi_comm_overrun;

/****************************************************************************/
/*																			*/
/*	Interrupt service routines												*/
/*																			*/
/****************************************************************************/

void jbi_timer_interrupt(void) interrupt 1 using 2
{
	/* We get 339 interrupts per second:  339 = 33.333 MHz / 12 / 8192 */
	if (++jbi_timer_count >= 339)
	{
		jbi_timer_count = 0;
		++jbi_timer_seconds;
	}
}

void jbi_comm_interrupt(void) interrupt 4 using 2
{
	/* received data interrupt */
	if (RI_0 != 0)
	{
		RI_0 = 0;		/* clear the interrupt */

		if ((jbi_comm_in_position + 1) != jbi_comm_out_position)
		{
			/* add character to the FIFO buffer */
			jbi_comm_buffer[jbi_comm_in_position++] = SBUF;
		}
		else
		{
			/* flag an error if the FIFO buffer overflows */
			jbi_comm_overrun = 1;
		}
	}
}

/****************************************************************************/
/*																			*/
/*	Code																	*/
/*																			*/
/****************************************************************************/

/*
*	The putchar function manages low-level output to the console device.  It
*	is called by run-time library functions (like printf), but can also be
*	called directly to output one character to the console device.
*/
char putchar(char c)
{
	/* if character was a line-feed, send a carriage-return character first */
	if (c == '\n')
	{
		while (!TI_0);
		TI_0 = 0;
		SBUF0 = '\r';
	}

	while (!TI_0);
	TI_0 = 0;
	SBUF0 = c;

	return (c);
}

/*
*	The _getkey function manages low-level input from the console device.
*	It is called by run-time library functions (like gets) but can also be
*	called directly to input one character from the console device.
*/
char _getkey(void)
{
	char c;
	while (!RI_0);
	c = SBUF0;
	RI_0 = 0;
	return (c);
}

/*
*	The jbi_comm_getchar() function is used to receive characters from the
*	input queue used by the jbi_comm_interrupt() interrupt service routine.
*	This function should only be used when COMM interrupts are enabled.
*/
#ifdef JBC_FILE_IN_RAM
char jbi_comm_getchar(void)
{
	char ch;

	/*
	*	Wait until a character is available in the FIFO buffer
	*/
	while (jbi_comm_in_position == jbi_comm_out_position);

	/*
	*	Get the next character from the FIFO buffer
	*/
	EA = 0;		/* disable interrupts */
	ch = jbi_comm_buffer[jbi_comm_out_position++];
	EA = 1;		/* enable interrupts */

	return (ch);
}
#endif /* JBC_FILE_IN_RAM */

/*
*	IMPORTANT!  The JBI_DELAY function must be adjusted to provide an accurate
*	real-time delay on the target microprocessor.  On a 33MHz DS87C520
*	microprocessor, dividing by eight then counting down long integers to zero
*	seems to give a pretty accurate result.  Other processors will vary.
*/
void jbi_delay(unsigned long microseconds)
{
	unsigned long count = microseconds >> 3;

	while (count != 0L) count--;
}

/*
*	IMPORTANT!  The JBI_JTAG_IO function must be modified to generate JTAG I/O
*	signals on the appropriate port channels of the target system hardware.
*	The mapping used here is:
*
*		TDI -> P1.0
*		TMS -> P1.1
*		TCK -> P1.7
*		TDO <- P3.5
*/
BIT jbi_jtag_io(BIT tms, BIT tdi, BIT read_tdo)
{
	BIT tdo = 0;

	P1_1 = tms;		/* set TMS signal */
	P1_0 = tdi;		/* set TDI signal */

	if (read_tdo)
	{
		tdo = P3_5;	/* read TDO signal */
	}

	P1_7 = 1;		/* strobe TCK signal */
	P1_7 = 0;

	return (tdo);
}

void jbi_jtag_drscan
(
	unsigned char start_state,
	unsigned int count,
	unsigned char XDATA_AREA *tdi,
	unsigned char XDATA_AREA *tdo
)
{
	unsigned int i = 0;
	BIT tdo_bit = 0;

	/*
	*	First go to DRSHIFT state
	*/
	switch (start_state)
	{
	case 0:						/* IDLE */
		jbi_jtag_io(1, 0, 0);	/* DRSELECT */
		jbi_jtag_io(0, 0, 0);	/* DRCAPTURE */
		jbi_jtag_io(0, 0, 0);	/* DRSHIFT */
		break;

	case 1:						/* DRPAUSE */
		jbi_jtag_io(1, 0, 0);	/* DREXIT2 */
		jbi_jtag_io(0, 0, 0);	/* DRSHIFT */
		break;

	case 2:						/* IRPAUSE */
		jbi_jtag_io(1, 0, 0);	/* IREXIT2 */
		jbi_jtag_io(1, 0, 0);	/* IRUPDATE */
		jbi_jtag_io(1, 0, 0);	/* DRSELECT */
		jbi_jtag_io(0, 0, 0);	/* DRCAPTURE */
		jbi_jtag_io(0, 0, 0);	/* DRSHIFT */
		break;

	default:
		break;
	}

	/* loop in the SHIFT-DR state */
	for (i = 0; i < count; i++)
	{
		tdo_bit = jbi_jtag_io(
			(i == count - 1) ? 1 : 0,
			tdi[i >> 3] & (1 << (i & 7)) ? 1 : 0,
			(tdo != NULL) ? 1 : 0);

		if (tdo != NULL)
		{
			if (tdo_bit == 1)
			{
				tdo[i >> 3] |= (1 << (i & 7));
			}
			else
			{
				tdo[i >> 3] &= ~(unsigned char) (1 << (i & 7));
			}
		}
	}

	jbi_jtag_io(0, 0, 0);	/* DRPAUSE */
}

void jbi_jtag_irscan
(
	unsigned char start_state,
	unsigned int count,
	unsigned char XDATA_AREA *tdi,
	unsigned char XDATA_AREA *tdo
)
{
	unsigned int i = 0;
	BIT tdo_bit = 0;

	/*
	*	First go to IRSHIFT state
	*/
	switch (start_state)
	{
	case 0:						/* IDLE */
		jbi_jtag_io(1, 0, 0);	/* DRSELECT */
		jbi_jtag_io(1, 0, 0);	/* IRSELECT */
		jbi_jtag_io(0, 0, 0);	/* IRCAPTURE */
		jbi_jtag_io(0, 0, 0);	/* IRSHIFT */
		break;

	case 1:						/* DRPAUSE */
		jbi_jtag_io(1, 0, 0);	/* DREXIT2 */
		jbi_jtag_io(1, 0, 0);	/* DRUPDATE */
		jbi_jtag_io(1, 0, 0);	/* DRSELECT */
		jbi_jtag_io(1, 0, 0);	/* IRSELECT */
		jbi_jtag_io(0, 0, 0);	/* IRCAPTURE */
		jbi_jtag_io(0, 0, 0);	/* IRSHIFT */
		break;

	case 2:						/* IRPAUSE */
		jbi_jtag_io(1, 0, 0);	/* IREXIT2 */
		jbi_jtag_io(0, 0, 0);	/* IRSHIFT */
		break;

	default:
		break;
	}

	/* loop in the SHIFT-IR state */
	for (i = 0; i < count; i++)
	{
		tdo_bit = jbi_jtag_io(
			(i == count - 1) ? 1 : 0,
			tdi[i >> 3] & (1 << (i & 7)) ? 1 : 0,
			(tdo != NULL) ? 1 : 0);

		if (tdo != NULL)
		{
			if (tdo_bit == 1)
			{
				tdo[i >> 3] |= (1 << (i & 7));
			}
			else
			{
				tdo[i >> 3] &= ~(unsigned char) (1 << (i & 7));
			}
		}
	}

	jbi_jtag_io(0, 0, 0);	/* IRPAUSE */
}

void jbi_message(char XDATA_AREA *message_text)
{
	puts(message_text);
}

void jbi_export(char FILE_AREA *key, long value)
{
	printf("Export: key = \"%s\", value = %ld\n", key, value);
}

char XDATA_AREA *jbi_malloc(unsigned int size)
{
	char XDATA_AREA *ptr;

	ptr = &jbi_heap[jbi_current_heap_ptr];
	jbi_current_heap_ptr += size;

	if (jbi_current_heap_ptr >= JBI_HEAP_SIZE)
	{
		/* out of memory -- return NULL */
		ptr = NULL;
	}

	return (ptr);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_set_dr_preamble
(
	unsigned int count,
	unsigned int start_index,
	unsigned char FILE_OR_XDATA_AREA *preamble_data
)

/*																			*/
/****************************************************************************/
{
	JBI_RETURN_TYPE status = JBIC_SUCCESS;
	unsigned int i;
	unsigned int j;

	if (count >= 0)
	{
		if (count > JBIC_MAX_JTAG_DR_PREAMBLE)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
		else
		{
			jbi_dr_preamble_count = count;
		}

		if (status == JBIC_SUCCESS)
		{
			for (i = 0; i < count; ++i)
			{
				j = i + start_index;

				if (preamble_data == NULL)
				{
					jbi_dr_preamble_data[i >> 3] |= (1 << (j & 7));
				}
				else
				{
					if (preamble_data[j >> 3] & (1 << (j & 7)))
					{
						jbi_dr_preamble_data[i >> 3] |= (1 << (j & 7));
					}
					else
					{
						jbi_dr_preamble_data[i >> 3] &=
							~(unsigned char) (1 << (j & 7));
					}
				}
			}
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_set_ir_preamble
(
	unsigned int count,
	unsigned int start_index,
	unsigned char FILE_OR_XDATA_AREA *preamble_data
)

/*																			*/
/****************************************************************************/
{
	JBI_RETURN_TYPE status = JBIC_SUCCESS;
	unsigned int i;
	unsigned int j;

	if (count >= 0)
	{
		if (count > JBIC_MAX_JTAG_IR_PREAMBLE)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
		else
		{
			jbi_ir_preamble_count = count;
		}

		if (status == JBIC_SUCCESS)
		{
			for (i = 0; i < count; ++i)
			{
				j = i + start_index;

				if (preamble_data == NULL)
				{
					jbi_ir_preamble_data[i >> 3] |= (1 << (j & 7));
				}
				else
				{
					if (preamble_data[j >> 3] & (1 << (j & 7)))
					{
						jbi_ir_preamble_data[i >> 3] |= (1 << (j & 7));
					}
					else
					{
						jbi_ir_preamble_data[i >> 3] &=
							~(unsigned char) (1 << (j & 7));
					}
				}
			}
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_set_dr_postamble
(
	unsigned int count,
	unsigned int start_index,
	unsigned char FILE_OR_XDATA_AREA *postamble_data
)

/*																			*/
/****************************************************************************/
{
	JBI_RETURN_TYPE status = JBIC_SUCCESS;
	unsigned int i;
	unsigned int j;

	if (count >= 0)
	{
		if (count > JBIC_MAX_JTAG_DR_POSTAMBLE)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
		else
		{
			jbi_dr_postamble_count = count;
		}

		if (status == JBIC_SUCCESS)
		{
			for (i = 0; i < count; ++i)
			{
				j = i + start_index;

				if (postamble_data == NULL)
				{
					jbi_dr_postamble_data[i >> 3] |= (1 << (j & 7));
				}
				else
				{
					if (postamble_data[j >> 3] & (1 << (j & 7)))
					{
						jbi_dr_postamble_data[i >> 3] |= (1 << (j & 7));
					}
					else
					{
						jbi_dr_postamble_data[i >> 3] &=
							~(unsigned char) (1 << (j & 7));
					}
				}
			}
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_set_ir_postamble
(
	unsigned int count,
	unsigned int start_index,
	unsigned char FILE_OR_XDATA_AREA *postamble_data
)

/*																			*/
/****************************************************************************/
{
	JBI_RETURN_TYPE status = JBIC_SUCCESS;
	unsigned int i;
	unsigned int j;

	if (count >= 0)
	{
		if (count > JBIC_MAX_JTAG_IR_POSTAMBLE)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
		else
		{
			jbi_ir_postamble_count = count;
		}

		if (status == JBIC_SUCCESS)
		{
			for (i = 0; i < count; ++i)
			{
				j = i + start_index;

				if (postamble_data == NULL)
				{
					jbi_ir_postamble_data[i >> 3] |= (1 << (j & 7));
				}
				else
				{
					if (postamble_data[j >> 3] & (1 << (j & 7)))
					{
						jbi_ir_postamble_data[i >> 3] |= (1 << (j & 7));
					}
					else
					{
						jbi_ir_postamble_data[i >> 3] &=
							~(unsigned char) (1 << (j & 7));
					}
				}
			}
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

void jbi_jtag_reset_idle(void)

/*																			*/
/****************************************************************************/
{
	unsigned char i;

	/*
	*	Go to Test Logic Reset (no matter what the starting state may be)
	*/
	for (i = 0; i < 5; ++i)
	{
		jbi_jtag_io(TMS_HIGH, TDI_LOW, IGNORE_TDO);
	}

	/*
	*	Now step to Run Test / Idle
	*/
	jbi_jtag_io(TMS_LOW, TDI_LOW, IGNORE_TDO);

	jbi_jtag_state = IDLE;
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_goto_jtag_state
(
	JBI_JTAG_STATE state
)

/*																			*/
/****************************************************************************/
{
	BIT tms;
	unsigned int count = 0;
	JBI_RETURN_TYPE status = JBIC_SUCCESS;

	if (jbi_jtag_state == ILLEGAL_JTAG_STATE)
	{
		/* initialize JTAG chain to known state */
		jbi_jtag_reset_idle();
	}

	if (jbi_jtag_state == state)
	{
		/*
		*	We are already in the desired state.  If it is a stable state,
		*	loop here.  Otherwise do nothing (no clock cycles).
		*/
		if ((state == IDLE) ||
			(state == DRSHIFT) ||
			(state == DRPAUSE) ||
			(state == IRSHIFT) ||
			(state == IRPAUSE))
		{
			jbi_jtag_io(TMS_LOW, TDI_LOW, IGNORE_TDO);
		}
		else if (state == RESET)
		{
			jbi_jtag_io(TMS_HIGH, TDI_LOW, IGNORE_TDO);
		}
	}
	else
	{
		while ((jbi_jtag_state != state) && (count < 9))
		{
			/*
			*	Get TMS value to take a step toward desired state
			*/
			tms = (jbi_jtag_path_map[jbi_jtag_state] & (1 << state)) ?
				TMS_HIGH : TMS_LOW;

			/*
			*	Take a step
			*/
			jbi_jtag_io(tms, TDI_LOW, IGNORE_TDO);

			if (tms)
			{
				jbi_jtag_state =
					jbi_jtag_state_transitions[jbi_jtag_state].tms_high;
			}
			else
			{
				jbi_jtag_state =
					jbi_jtag_state_transitions[jbi_jtag_state].tms_low;
			}

			++count;
		}
	}

	if (jbi_jtag_state != state)
	{
		status = JBIC_INTERNAL_ERROR;
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_do_wait_cycles
(
	unsigned long cycles,
	JBI_JTAG_STATE wait_state
)

/*																			*/
/*	Description:	Causes JTAG hardware to loop in the specified stable	*/
/*					state for the specified number of TCK clock cycles.		*/
/*																			*/
/*	Returns:		JBIC_SUCCESS for success, else appropriate error code	*/
/*																			*/
/****************************************************************************/
{
	BIT tms;
	unsigned long count;
	JBI_RETURN_TYPE status = JBIC_SUCCESS;

	if (jbi_jtag_state != wait_state)
	{
		status = jbi_goto_jtag_state(wait_state);
	}

	if (status == JBIC_SUCCESS)
	{
		/*
		*	Set TMS high to loop in RESET state
		*	Set TMS low to loop in any other stable state
		*/
		tms = (wait_state == RESET) ? TMS_HIGH : TMS_LOW;

		for (count = 0L; count < cycles; count++)
		{
			jbi_jtag_io(tms, TDI_LOW, IGNORE_TDO);
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_do_wait_microseconds
(
	unsigned long microseconds,
	JBI_JTAG_STATE wait_state
)

/*																			*/
/*	Description:	Causes JTAG hardware to sit in the specified stable		*/
/*					state for the specified duration of real time.  If		*/
/*					no JTAG operations have been performed yet, then only	*/
/*					a delay is performed.  This permits the WAIT USECS		*/
/*					statement to be used in VECTOR programs without causing	*/
/*					any JTAG operations.									*/
/*																			*/
/*	Returns:		JBIC_SUCCESS for success, else appropriate error code	*/
/*																			*/
/****************************************************************************/
{
	JBI_RETURN_TYPE status = JBIC_SUCCESS;

	if ((jbi_jtag_state != ILLEGAL_JTAG_STATE) &&
		(jbi_jtag_state != wait_state))
	{
		status = jbi_goto_jtag_state(wait_state);
	}

	if (status == JBIC_SUCCESS)
	{
		/*
		*	Wait for specified time interval
		*/
		jbi_delay(microseconds);
	}

	return (status);
}

/****************************************************************************/
/*																			*/

void jbi_jtag_concatenate_data
(
	unsigned char XDATA_AREA *buffer,
	unsigned char XDATA_AREA *preamble_data,
	unsigned int preamble_count,
	unsigned char FILE_OR_XDATA_AREA *target_data,
	unsigned long start_index,
	unsigned int target_count,
	unsigned char XDATA_AREA *postamble_data,
	unsigned int postamble_count
)

/*																			*/
/*	Description:	Copies preamble data, target data, and postamble data	*/
/*					into one buffer for IR or DR scans.						*/
/*																			*/
/*	Returns:		nothing													*/
/*																			*/
/****************************************************************************/
{
	unsigned int i;
	unsigned int j;
	unsigned int stop;
	unsigned long XDATA_AREA l;

	for (i = 0; i < preamble_count; ++i)
	{
		if (preamble_data[i >> 3] & (1 << (i & 7)))
		{
			buffer[i >> 3] |= (1 << (i & 7));
		}
		else
		{
			buffer[i >> 3] &= ~(unsigned char) (1 << (i & 7));
		}
	}

	l = start_index;
	stop = preamble_count + target_count;
	for (; i < stop; ++i, ++l)
	{
		if (target_data[l >> 3] & (1 << (l & 7)))
		{
			buffer[i >> 3] |= (1 << (i & 7));
		}
		else
		{
			buffer[i >> 3] &= ~(unsigned char) (1 << (i & 7));
		}
	}

	j = 0;
	stop = preamble_count + target_count + postamble_count;
	for (; i < stop; ++i, ++j)
	{
		if (postamble_data[j >> 3] & (1 << (j & 7)))
		{
			buffer[i >> 3] |= (1 << (i & 7));
		}
		else
		{
			buffer[i >> 3] &= ~(unsigned char) (1 << (i & 7));
		}
	}
}

/****************************************************************************/
/*																			*/

void jbi_jtag_extract_target_data
(
	unsigned char XDATA_AREA *buffer,
	unsigned char XDATA_AREA *target_data,
	unsigned int start_index,
	unsigned int preamble_count,
	unsigned int target_count
)

/*																			*/
/*	Description:	Copies target data from scan buffer, filtering out		*/
/*					preamble and postamble data.							*/
/*																			*/
/*	Returns:		nothing													*/
/*																			*/
/****************************************************************************/
{
	unsigned int i;
	unsigned int j;
	unsigned int stop;

	j = preamble_count;
	stop = start_index + target_count;
	for (i = start_index; i < stop; ++i, ++j)
	{
		if (buffer[j >> 3] & (1 << (j & 7)))
		{
			target_data[i >> 3] |= (1 << (i & 7));
		}
		else
		{
			target_data[i >> 3] &= ~(unsigned char) (1 << (i & 7));
		}
	}
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_do_irscan
(
	unsigned int count,
	unsigned char FILE_OR_XDATA_AREA *in_data,
	unsigned int in_index
)

/*																			*/
/*	Description:	Shifts data into instruction register					*/
/*																			*/
/*	Returns:		JBIC_SUCCESS for success, else appropriate error code	*/
/*																			*/
/****************************************************************************/
{
	unsigned char start_code = 0;
	unsigned int shift_count = jbi_ir_preamble_count + count + jbi_ir_postamble_count;
	JBI_RETURN_TYPE status = JBIC_SUCCESS;
	JBI_JTAG_STATE start_state = ILLEGAL_JTAG_STATE;

	switch (jbi_jtag_state)
	{
	case ILLEGAL_JTAG_STATE:
	case RESET:
	case IDLE:
		start_code = 0;
		start_state = IDLE;
		break;

	case DRSELECT:
	case DRCAPTURE:
	case DRSHIFT:
	case DREXIT1:
	case DRPAUSE:
	case DREXIT2:
	case DRUPDATE:
		start_code = 1;
		start_state = DRPAUSE;
		break;

	case IRSELECT:
	case IRCAPTURE:
	case IRSHIFT:
	case IREXIT1:
	case IRPAUSE:
	case IREXIT2:
	case IRUPDATE:
		start_code = 2;
		start_state = IRPAUSE;
		break;

	default:
		status = JBIC_INTERNAL_ERROR;
		break;
	}

	if (status == JBIC_SUCCESS)
	{
		if (jbi_jtag_state != start_state)
		{
			status = jbi_goto_jtag_state(start_state);
		}
	}

	if (status == JBIC_SUCCESS)
	{
		if (shift_count > JBIC_MAX_JTAG_IR_LENGTH)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
	}

	if (status == JBIC_SUCCESS)
	{
		/*
		*	Copy preamble data, IR data, and postamble data into a buffer
		*/
		jbi_jtag_concatenate_data
		(
			jbi_ir_buffer,
			jbi_ir_preamble_data,
			jbi_ir_preamble_count,
			in_data,
			in_index,
			count,
			jbi_ir_postamble_data,
			jbi_ir_postamble_count
		);

		/*
		*	Do the IRSCAN
		*/
		jbi_jtag_irscan
		(
			start_code,
			shift_count,
			jbi_ir_buffer,
			NULL
		);

		/* jbi_jtag_irscan() always ends in IRPAUSE state */
		jbi_jtag_state = IRPAUSE;
	}

	if (status == JBIC_SUCCESS)
	{
		if (jbi_irstop_state != IRPAUSE)
		{
			status = jbi_goto_jtag_state(jbi_irstop_state);
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_swap_ir
(
	unsigned int count,
	unsigned char FILE_OR_XDATA_AREA *in_data,
	unsigned int in_index,
	unsigned char XDATA_AREA *out_data,
	unsigned int out_index
)

/*																			*/
/*	Description:	Shifts data into instruction register, capturing output	*/
/*					data													*/
/*																			*/
/*	Returns:		JBIC_SUCCESS for success, else appropriate error code	*/
/*																			*/
/****************************************************************************/
{
	unsigned char start_code = 0;
	unsigned int shift_count = jbi_ir_preamble_count + count + jbi_ir_postamble_count;
	JBI_RETURN_TYPE status = JBIC_SUCCESS;
	JBI_JTAG_STATE start_state = ILLEGAL_JTAG_STATE;

	switch (jbi_jtag_state)
	{
	case ILLEGAL_JTAG_STATE:
	case RESET:
	case IDLE:
		start_code = 0;
		start_state = IDLE;
		break;

	case DRSELECT:
	case DRCAPTURE:
	case DRSHIFT:
	case DREXIT1:
	case DRPAUSE:
	case DREXIT2:
	case DRUPDATE:
		start_code = 1;
		start_state = DRPAUSE;
		break;

	case IRSELECT:
	case IRCAPTURE:
	case IRSHIFT:
	case IREXIT1:
	case IRPAUSE:
	case IREXIT2:
	case IRUPDATE:
		start_code = 2;
		start_state = IRPAUSE;
		break;

	default:
		status = JBIC_INTERNAL_ERROR;
		break;
	}

	if (status == JBIC_SUCCESS)
	{
		if (jbi_jtag_state != start_state)
		{
			status = jbi_goto_jtag_state(start_state);
		}
	}

	if (status == JBIC_SUCCESS)
	{
		if (shift_count > JBIC_MAX_JTAG_IR_LENGTH)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
	}

	if (status == JBIC_SUCCESS)
	{
		/*
		*	Copy preamble data, IR data, and postamble data into a buffer
		*/
		jbi_jtag_concatenate_data
		(
			jbi_ir_buffer,
			jbi_ir_preamble_data,
			jbi_ir_preamble_count,
			in_data,
			in_index,
			count,
			jbi_ir_postamble_data,
			jbi_ir_postamble_count
		);

		/*
		*	Do the IRSCAN
		*/
		jbi_jtag_irscan
		(
			start_code,
			shift_count,
			jbi_ir_buffer,
			jbi_ir_buffer
		);

		/* jbi_jtag_irscan() always ends in IRPAUSE state */
		jbi_jtag_state = IRPAUSE;
	}

	if (status == JBIC_SUCCESS)
	{
		if (jbi_irstop_state != IRPAUSE)
		{
			status = jbi_goto_jtag_state(jbi_irstop_state);
		}
	}

	if (status == JBIC_SUCCESS)
	{
		/*
		*	Now extract the returned data from the buffer
		*/
		jbi_jtag_extract_target_data
		(
			jbi_ir_buffer,
			out_data,
			out_index,
			jbi_ir_preamble_count,
			count
		);
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_do_drscan
(
	unsigned int count,
	unsigned char FILE_OR_XDATA_AREA *in_data,
	unsigned long in_index
)

/*																			*/
/*	Description:	Shifts data into data register (ignoring output data)	*/
/*																			*/
/*	Returns:		JBIC_SUCCESS for success, else appropriate error code	*/
/*																			*/
/****************************************************************************/
{
	unsigned char start_code = 0;
	unsigned int shift_count = jbi_dr_preamble_count + count + jbi_dr_postamble_count;
	JBI_RETURN_TYPE status = JBIC_SUCCESS;
	JBI_JTAG_STATE start_state = ILLEGAL_JTAG_STATE;

	switch (jbi_jtag_state)
	{
	case ILLEGAL_JTAG_STATE:
	case RESET:
	case IDLE:
		start_code = 0;
		start_state = IDLE;
		break;

	case DRSELECT:
	case DRCAPTURE:
	case DRSHIFT:
	case DREXIT1:
	case DRPAUSE:
	case DREXIT2:
	case DRUPDATE:
		start_code = 1;
		start_state = DRPAUSE;
		break;

	case IRSELECT:
	case IRCAPTURE:
	case IRSHIFT:
	case IREXIT1:
	case IRPAUSE:
	case IREXIT2:
	case IRUPDATE:
		start_code = 2;
		start_state = IRPAUSE;
		break;

	default:
		status = JBIC_INTERNAL_ERROR;
		break;
	}

	if (status == JBIC_SUCCESS)
	{
		if (jbi_jtag_state != start_state)
		{
			status = jbi_goto_jtag_state(start_state);
		}
	}

	if (status == JBIC_SUCCESS)
	{
		if (shift_count > JBIC_MAX_JTAG_DR_LENGTH)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
	}

	if (status == JBIC_SUCCESS)
	{
		/*
		*	Copy preamble data, DR data, and postamble data into a buffer
		*/
		jbi_jtag_concatenate_data
		(
			jbi_dr_buffer,
			jbi_dr_preamble_data,
			jbi_dr_preamble_count,
			in_data,
			in_index,
			count,
			jbi_dr_postamble_data,
			jbi_dr_postamble_count
		);

		/*
		*	Do the DRSCAN
		*/
		jbi_jtag_drscan
		(
			start_code,
			shift_count,
			jbi_dr_buffer,
			NULL
		);

		/* jbi_jtag_drscan() always ends in DRPAUSE state */
		jbi_jtag_state = DRPAUSE;
	}

	if (status == JBIC_SUCCESS)
	{
		if (jbi_drstop_state != DRPAUSE)
		{
			status = jbi_goto_jtag_state(jbi_drstop_state);
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_swap_dr
(
	unsigned int count,
	unsigned char FILE_OR_XDATA_AREA *in_data,
	unsigned long in_index,
	unsigned char XDATA_AREA *out_data,
	unsigned int out_index
)

/*																			*/
/*	Description:	Shifts data into data register, capturing output data	*/
/*																			*/
/*	Returns:		JBIC_SUCCESS for success, else appropriate error code	*/
/*																			*/
/****************************************************************************/
{
	unsigned char start_code = 0;
	unsigned int shift_count = jbi_dr_preamble_count + count + jbi_dr_postamble_count;
	JBI_RETURN_TYPE status = JBIC_SUCCESS;
	JBI_JTAG_STATE start_state = ILLEGAL_JTAG_STATE;

	switch (jbi_jtag_state)
	{
	case ILLEGAL_JTAG_STATE:
	case RESET:
	case IDLE:
		start_code = 0;
		start_state = IDLE;
		break;

	case DRSELECT:
	case DRCAPTURE:
	case DRSHIFT:
	case DREXIT1:
	case DRPAUSE:
	case DREXIT2:
	case DRUPDATE:
		start_code = 1;
		start_state = DRPAUSE;
		break;

	case IRSELECT:
	case IRCAPTURE:
	case IRSHIFT:
	case IREXIT1:
	case IRPAUSE:
	case IREXIT2:
	case IRUPDATE:
		start_code = 2;
		start_state = IRPAUSE;
		break;

	default:
		status = JBIC_INTERNAL_ERROR;
		break;
	}

	if (status == JBIC_SUCCESS)
	{
		if (jbi_jtag_state != start_state)
		{
			status = jbi_goto_jtag_state(start_state);
		}
	}

	if (status == JBIC_SUCCESS)
	{
		if (shift_count > JBIC_MAX_JTAG_DR_LENGTH)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
	}

	if (status == JBIC_SUCCESS)
	{
		/*
		*	Copy preamble data, DR data, and postamble data into a buffer
		*/
		jbi_jtag_concatenate_data
		(
			jbi_dr_buffer,
			jbi_dr_preamble_data,
			jbi_dr_preamble_count,
			in_data,
			in_index,
			count,
			jbi_dr_postamble_data,
			jbi_dr_postamble_count
		);

		/*
		*	Do the DRSCAN
		*/
		jbi_jtag_drscan
		(
			start_code,
			shift_count,
			jbi_dr_buffer,
			jbi_dr_buffer
		);

		/* jbi_jtag_drscan() always ends in DRPAUSE state */
		jbi_jtag_state = DRPAUSE;
	}

	if (status == JBIC_SUCCESS)
	{
		if (jbi_drstop_state != DRPAUSE)
		{
			status = jbi_goto_jtag_state(jbi_drstop_state);
		}
	}

	if (status == JBIC_SUCCESS)
	{
		/*
		*	Now extract the returned data from the buffer
		*/
		jbi_jtag_extract_target_data
		(
			jbi_dr_buffer,
			out_data,
			out_index,
			jbi_dr_preamble_count,
			count
		);
	}

	return (status);
}

/****************************************************************************/
/*																			*/
/*	UTILITY FUNCTIONS														*/
/*																			*/
/****************************************************************************/

int jbi_strlen(char XDATA_AREA *string)
{
	int len = 0;

	while (string[len] != '\0') ++len;

	return (len);
}

long jbi_atol(char CONSTANT_AREA *buffer)
{
	long result = 0L;
	int index = 0;

	while ((buffer[index] >= '0') && (buffer[index] <= '9'))
	{
		result = (result * 10) + (buffer[index] - '0');
		++index;
	}

	return (result);
}

void jbi_ltoa(char XDATA_AREA *buffer, long number)
{
	int index = 0;
	int rev_index = 0;
	char XDATA_AREA reverse[32];

	if (number < 0L)
	{
		buffer[index++] = '-';
		number = 0 - number;
	}
	else if (number == 0)
	{
		buffer[index++] = '0';
	}

	while (number != 0)
	{
		reverse[rev_index++] = (char) ((number % 10) + '0');
		number /= 10;
	}

	while (rev_index > 0)
	{
		buffer[index++] = reverse[--rev_index];
	}

	buffer[index] = '\0';
}

char jbi_toupper(char ch)
{
	return ((char) (((ch >= 'a') && (ch <= 'z')) ? (ch + 'A' - 'a') : ch));
}

int jbi_stricmp(char XDATA_AREA *left, char FILE_AREA *right)
{
	int result = 0;
	char l, r;

	do
	{
		l = jbi_toupper(*left);
		r = jbi_toupper(*right);
		result = l - r;
		++left;
		++right;
	}
	while ((result == 0) && (l != '\0') && (r != '\0'));

	return (result);
}

void jbi_strncpy(char XDATA_AREA *left, char *right, int count)
{
	char ch;

	do
	{
		*left = *right;
		ch = *right;
		++left;
		++right;
		--count;
	}
	while ((ch != '\0') && (count != 0));
}

void jbi_make_dword(unsigned char XDATA_AREA *buf, unsigned long num)
{
	buf[0] = (unsigned char) num;
	buf[1] = (unsigned char) (num >> 8);
	buf[2] = (unsigned char) (num >> 16);
	buf[3] = (unsigned char) (num >> 24);
}

unsigned long jbi_get_dword(unsigned char FILE_OR_XDATA_AREA *buf)
{
	return
		(((unsigned long) buf[0]) |
		(((unsigned long) buf[1]) << 8) |
		(((unsigned long) buf[2]) << 16) |
		(((unsigned long) buf[3]) << 24));
}

unsigned int jbi_get_program_word(unsigned int x)
{
	unsigned int FILE_AREA *ptr = (unsigned int FILE_AREA *) &jbi_program[x];
	return (*ptr);
}

unsigned long jbi_get_program_dword(unsigned int x)
{
	unsigned long FILE_AREA *ptr = (unsigned long FILE_AREA *) &jbi_program[x];
	return (*ptr);
}

/*
*	Functions for measurement of elapsed real-time
*/
void jbi_start_timer(void)
{
	EA = 0;
	jbi_timer_count = 0;
	jbi_timer_seconds = 0;
	ET0 = 1;
	EA = 1;
	TR0 = 1;
}

void jbi_stop_timer(void)
{
	EA = 0;
	ET0 = 0;
	TR0 = 0;
}

/****************************************************************************/
/*																			*/
/*	The following functions implement incremental decompression of Boolean	*/
/*	array data, using a small memory window.								*/
/*																			*/
/*	This algorithm works by searching previous bytes in the data that match	*/
/*	the current data. If a match is found, then the offset and length of	*/
/*	the matching data can replace the actual data in the output.			*/
/*																			*/
/*	Memory usage is reduced by maintaining a "window" buffer which contains	*/
/*	the uncompressed data for one 8K page, plus some extra amount specified	*/
/*	by JBI_ACA_REQUEST_SIZE.  The function jbi_uncompress_page() is used to	*/
/*	request a subrange of the uncompressed data, starting at a particular	*/
/*	bit position and extending a maximum of JBI_ACA_REQUEST_SIZE bytes.		*/
/*																			*/
/****************************************************************************/

/****************************************************************************/
/*																			*/

unsigned char jbi_bits_required(int n)

/*																			*/
/*	Description:	Calculate the minimum number of bits required to		*/
/*					represent n.											*/
/*																			*/
/*	Returns:		Number of bits.											*/
/*																			*/
/****************************************************************************/
{
	unsigned char result = SHORT_BITS;

	if (n == 0)
	{
		result = 1;
	}
	else
	{
		/* Look for the highest non-zero bit position */
		while ((n & (1 << (SHORT_BITS - 1))) == 0)
		{
			n <<= 1;
			--result;
		}
	}

	return (result);
}

/****************************************************************************/
/*																			*/

unsigned int jbi_read_packed(unsigned char bits)

/*																			*/
/*	Description:	Read the next value from the input array "buffer".		*/
/*					Read only "bits" bits from the array. The amount of		*/
/*					bits that have already been read from "buffer" is		*/
/*					stored internally to this function.					 	*/
/*																			*/
/*	Returns:		Up to 16 bit value. -1 if buffer overrun.				*/
/*																			*/
/****************************************************************************/
{
	unsigned int XDATA_AREA result = 0;
	unsigned char shift = 0;

	while (result != -1 && bits > 0)
	{
		result |= (((jbi_aca_in_buffer[jbi_in_index] >> (CHAR_BITS - jbi_bits_avail))
			& (0xFF >> (CHAR_BITS - jbi_bits_avail))) << shift);

		if (bits <= jbi_bits_avail)
		{
			result &= (0xFFFF >> (SHORT_BITS - (bits + shift)));
			jbi_bits_avail -= bits;
			bits = 0;
		}
		else
		{
			++jbi_in_index;
			shift += jbi_bits_avail;
			bits -= jbi_bits_avail;
			jbi_bits_avail = CHAR_BITS;
		}
	}

	return (result);
}

/****************************************************************************/
/*																			*/

void jbi_uncompress_next_page(void)

/*																			*/
/*	Description:	Uncompresses one page of compressed data, using			*/
/*					data page as reference for repeated sections.			*/
/*					Overwrites previous page of data in buffer.				*/
/*																			*/
/*	Returns:		nothing													*/
/*																			*/
/****************************************************************************/
{
	unsigned int RDATA_AREA i;
	unsigned int XDATA_AREA j, offset, length;
	unsigned int XDATA_AREA end_index;
	unsigned int XDATA_AREA tmp_in_index;
	unsigned int XDATA_AREA tmp_out_index;
	unsigned char XDATA_AREA tmp_bits_avail;
	unsigned int XDATA_AREA prev[3];
	unsigned long XDATA_AREA long_end;

	if (jbi_current_page < 0)
	{
		/* this is the first page of the array */
		jbi_current_page = 0;
		jbi_in_index = 4;	/* skip over length field */
		jbi_out_index = 0;
		end_index = (jbi_out_length < JBI_ACA_BUFFER_SIZE) ?
			(unsigned int) jbi_out_length : JBI_ACA_BUFFER_SIZE;
	}
	else
	{
		/* this is not the first page */
		++jbi_current_page;
		jbi_out_index -= MATCH_DATA_LENGTH;
		long_end = jbi_out_length -
			((long) jbi_current_page * (long) MATCH_DATA_LENGTH);
		end_index = (long_end < JBI_ACA_BUFFER_SIZE) ?
			(unsigned int) long_end : JBI_ACA_BUFFER_SIZE;

		/* copy extra data from end of circular buffer to beginning */
		for (i = 0; i < jbi_out_index; ++i)
		{
			jbi_aca_out_buffer[i] = jbi_aca_out_buffer[i + MATCH_DATA_LENGTH];
		}
	}

	while (jbi_out_index < end_index)
	{
		/* save state so we can undo the last packet when we reach the end */
		tmp_in_index = jbi_in_index;
		tmp_out_index = jbi_out_index;
		tmp_bits_avail = jbi_bits_avail;

		/* A 0 bit indicates literal data. */
		if (jbi_read_packed(1) == 0)
		{
			for (i = 0; i < DATA_BLOB_LENGTH; ++i)
			{
				if (jbi_out_index < end_index)
				{
					prev[i] = jbi_aca_out_buffer[jbi_out_index] & 0xff;
					jbi_aca_out_buffer[jbi_out_index++] =
						(unsigned char) jbi_read_packed(CHAR_BITS);
				}
			}
		}
		else
		{
			/* A 1 bit indicates offset/length to follow. */
			offset = jbi_read_packed(jbi_bits_required(
				(jbi_current_page > 0) ? MATCH_DATA_LENGTH :
				(jbi_out_index > MATCH_DATA_LENGTH ? MATCH_DATA_LENGTH :
				jbi_out_index)));
			length = jbi_read_packed(CHAR_BITS);

			if (offset == MATCH_DATA_LENGTH + 3)
			{
				jbi_aca_out_buffer[jbi_out_index++] = prev[0];
				jbi_aca_out_buffer[jbi_out_index++] = prev[1];
				jbi_aca_out_buffer[jbi_out_index++] = prev[2];
				length -= 3;
			}

			for (i = 0; i < length; ++i)
			{
				if (jbi_out_index < end_index)
				{
					if (offset > jbi_out_index)
					{
						j = jbi_out_index + MATCH_DATA_LENGTH - offset;
					}
					else j = jbi_out_index - offset;
					jbi_aca_out_buffer[jbi_out_index] = jbi_aca_out_buffer[j];
					++jbi_out_index;
				}
			}

			prev[0] = jbi_aca_out_buffer[jbi_out_index - 3] & 0xff;
			prev[1] = jbi_aca_out_buffer[jbi_out_index - 2] & 0xff;
			prev[2] = jbi_aca_out_buffer[jbi_out_index - 1] & 0xff;
		}
	}

	/* restore the state before the previous packet */
	jbi_in_index = tmp_in_index;
	jbi_out_index = tmp_out_index;
	jbi_bits_avail = tmp_bits_avail;
}

/****************************************************************************/
/*																			*/

void jbi_uncompress_page
(
	int variable_id,
	int page
)

/*																			*/
/*	Description:	Uncompress requested page of variable data.  Stores		*/
/*					uncompressed data in jbi_aca_out_buffer[].				*/
/*																			*/
/*	Returns:		nothing													*/
/*																			*/
/****************************************************************************/
{
	unsigned int XDATA_AREA symbol_table;
	unsigned int XDATA_AREA data_section;
	unsigned int XDATA_AREA offset;
	unsigned int XDATA_AREA value;

	if (variable_id != jbi_current_variable_id)
	{
		/* initialize to uncompress the desired variable */
		symbol_table = (unsigned int) GET_DWORD(16);
		data_section = (unsigned int) GET_DWORD(20);
		offset = symbol_table + (11 * variable_id);
		value = GET_DWORD(offset + 3);
		jbi_current_variable_id = variable_id;
		jbi_current_page = -1;
		jbi_bits_avail = CHAR_BITS;
		jbi_in_length = GET_DWORD(offset + 7);
		jbi_out_length = jbi_get_dword(&jbi_program[data_section + value]);
		jbi_in_index = 4;	/* skip over length field */
		jbi_out_index = 0;
		jbi_aca_in_buffer = &jbi_program[data_section + value];
	}

	/* to look back at an earlier page, start over at the beginning */
	if (page < jbi_current_page)
	{
		jbi_current_page = -1;
		jbi_in_index = 4;	/* skip over length field */
		jbi_bits_avail = CHAR_BITS;
	}

	/* uncompress sequentially up to the desired page */
	while (page > jbi_current_page)
	{
		jbi_uncompress_next_page();
	}
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_execute
(
	char CONSTANT_AREA **init_list,
	unsigned int XDATA_AREA *error_address,
	int XDATA_AREA *exit_code
)

/*																			*/
/*	Description:															*/
/*																			*/
/*	Returns:																*/
/*																			*/
/****************************************************************************/
{
	JBI_RETURN_TYPE XDATA_AREA status = JBIC_SUCCESS;
	unsigned long XDATA_AREA first_word = 0L;
	unsigned long XDATA_AREA string_table = 0L;
	unsigned long XDATA_AREA symbol_table = 0L;
	unsigned long XDATA_AREA data_section = 0L;
	unsigned long XDATA_AREA code_section = 0L;
	unsigned long XDATA_AREA debug_section = 0L;
	unsigned long XDATA_AREA ram_required = 0L;
	unsigned long XDATA_AREA symbol_count = 0L;
	char XDATA_AREA message_buffer[JBI_MESSAGE_LENGTH + 1];
	long XDATA_AREA variables[JBI_VARIABLE_COUNT];
	unsigned char XDATA_AREA * XDATA_AREA variables_x[JBI_VARIABLE_COUNT];
	unsigned char FILE_AREA  * XDATA_AREA variables_c[JBI_VARIABLE_COUNT];
	long XDATA_AREA variable_size[JBI_VARIABLE_COUNT];
	unsigned char XDATA_AREA attributes[JBI_VARIABLE_COUNT];
	unsigned char XDATA_AREA *array_data = NULL;
	unsigned int RDATA_AREA pc;
	unsigned int XDATA_AREA opcode_address;
	unsigned long XDATA_AREA args[3];
	unsigned char RDATA_AREA opcode;
	unsigned char XDATA_AREA charbuf[4];
	long XDATA_AREA long_temp;
	unsigned long XDATA_AREA long_index;
	unsigned int XDATA_AREA variable_id;
	unsigned char XDATA_AREA * XDATA_AREA charptr_temp;
	long XDATA_AREA * XDATA_AREA longptr_temp;
	unsigned char FILE_AREA * XDATA_AREA ccptr;
	unsigned int XDATA_AREA arg_count;
	BIT done = 0;
	unsigned int XDATA_AREA count;
	unsigned int XDATA_AREA index;
	unsigned int RDATA_AREA i;
	unsigned char XDATA_AREA length;
	char CONSTANT_AREA * XDATA_AREA equal_ptr;
	long XDATA_AREA value;
	unsigned int XDATA_AREA offset;
	unsigned int XDATA_AREA name_id;
	char FILE_AREA * XDATA_AREA name;
	long XDATA_AREA c, d;
	unsigned char FILE_OR_XDATA_AREA *source1;
	unsigned char FILE_OR_XDATA_AREA *source2;
	unsigned char FILE_OR_XDATA_AREA *mask;
	long XDATA_AREA step;
	long XDATA_AREA end;
	long XDATA_AREA top;
	long XDATA_AREA iterator;
	BIT break_out;
	unsigned int XDATA_AREA index2;
	unsigned char FILE_OR_XDATA_AREA * XDATA_AREA unknown_ptr;
	unsigned char ch;

	/*
	*	Initialize heap
	*/
	jbi_current_heap_ptr = 0;
	for (i = 0; i < JBI_HEAP_SIZE; ++i) jbi_heap[i] = 0;

	/*
	*	Initialize stack
	*/
	jbi_stack_ptr = 0;
	for (i = 0; i < JBI_STACK_SIZE; ++i) jbi_stack[i] = 0L;

	/*
	*	Initialize JTAG interface
	*/
	jbi_jtag_state = ILLEGAL_JTAG_STATE;
	jbi_drstop_state = IDLE;
	jbi_irstop_state = IDLE;
	jbi_dr_preamble_count  = 0;
	jbi_dr_postamble_count = 0;
	jbi_ir_preamble_count  = 0;
	jbi_ir_postamble_count = 0;

	/*
	*	Initialize ACA data compression variables
	*/
	jbi_current_variable_id = -1;
	jbi_current_page = -1;
	jbi_bits_avail = CHAR_BITS;
	jbi_in_length = 0;
	jbi_out_length = 0L;
	jbi_in_index = 0;
	jbi_out_index = 0;
	jbi_aca_in_buffer = NULL;

	/*
	*	Read header information
	*/
	first_word    = GET_DWORD(0);
	string_table  = GET_DWORD(4);
	symbol_table  = GET_DWORD(16);
	data_section  = GET_DWORD(20);
	code_section  = GET_DWORD(24);
	debug_section = GET_DWORD(28);
	ram_required  = GET_DWORD(36);
	symbol_count  = GET_DWORD(48);

	if (first_word != 0x4A414D00L)
	{
		done = 1;
		status = JBIC_IO_ERROR;
	}

	if ((status == JBIC_SUCCESS) && (symbol_count > JBI_VARIABLE_COUNT))
	{
		/* too many symbols in symbol table */
		status = JBIC_OUT_OF_MEMORY;
	}

	if ((status == JBIC_SUCCESS) && (symbol_count > 0L))
	{
		array_data = jbi_malloc((unsigned int) ram_required);

		if (array_data == NULL)
		{
			status = JBIC_OUT_OF_MEMORY;
		}
		else
		{
			for (i = 0; i < (unsigned int) symbol_count; ++i)
			{
				offset = (unsigned int) (symbol_table + (11 * i));

				value = GET_DWORD(offset + 3);

				attributes[i] = jbi_program[offset];

				variable_size[i] = GET_DWORD(offset + 7);

				/*
				*	Attribute bits:
				*	bit 0:	0 = read-only, 1 = read-write
				*	bit 1:	0 = not compressed, 1 = compressed
				*	bit 2:	0 = not initialized, 1 = initialized
				*	bit 3:	0 = scalar, 1 = array
				*	bit 4:	0 = Boolean, 1 = integer
				*	bit 5:	0 = declared variable,
				*			1 = compiler created temporary variable
				*/

				if ((attributes[i] & 0x0c) == 0x04)
				{
					/* initialized scalar variable */
					variables[i] = value;
				}
				else if ((attributes[i] & 0x1e) == 0x0e)
				{
					/* initialized compressed Boolean array */
					variable_size[i] = jbi_get_dword(
						&jbi_program[data_section + value]);
				}
				else if ((attributes[i] & 0x1e) == 0x0c)
				{
					/* initialized Boolean array */
					variables_c[i] = &jbi_program[data_section + value];
				}
				else if ((attributes[i] & 0x1c) == 0x1c)
				{
					/* initialized integer array */
					variables[i] = value + data_section;
				}
				else if ((attributes[i] & 0x0c) == 0x08)
				{
					/* uninitialized array */
					variables_x[i] = &array_data[value];
				}
				else
				{
					variables[i] = 0;
				}
			}

			for (i = 0; i < (unsigned int) ram_required; ++i)
			{
				array_data[i] = 0;
			}
		}
	}

	/*
	*	Initialize variables listed in init_list
	*/
	if ((status == JBIC_SUCCESS) && (init_list != NULL))
	{
		count = 0;
		while (init_list[count] != NULL)
		{
			equal_ptr = init_list[count];
			length = 0;
			while ((*equal_ptr != '=') && (*equal_ptr != '\0'))
			{
				++equal_ptr;
				++length;
			}
			if (*equal_ptr == '=')
			{
				++equal_ptr;
				value = jbi_atol(equal_ptr);
				jbi_strncpy(message_buffer, init_list[count], length);
				message_buffer[length] = '\0';
				for (i = 0; i < (unsigned int) symbol_count; ++i)
				{
					offset = (unsigned int) (symbol_table + (11 * i));
					name_id = GET_WORD(offset + 1);
					name = &jbi_program[string_table + name_id];

					if (jbi_stricmp(message_buffer, name) == 0)
					{
						variables[i] = value;
					}
				}
			}

			++count;
		}
	}

	if (status == JBIC_SUCCESS)
	{
		pc = code_section;
		message_buffer[0] = '\0';
	}
	else
	{
		done = 1;
	}

	while (!done)
	{
		opcode = jbi_program[pc];
		opcode_address = pc;
		++pc;

		arg_count = (opcode >> 6) & 3;
		for (i = 0; i < arg_count; ++i)
		{
			args[i] = GET_DWORD(pc);
			pc += 4;
		}

		switch (opcode)
		{
		case 0x00: /* NOP  */
			/* do nothing */
			break;

		case 0x01: /* DUP  */
			IF_CHECK_STACK(1)
			{
				jbi_stack[jbi_stack_ptr] = jbi_stack[jbi_stack_ptr - 1];
				++jbi_stack_ptr;
			}
			break;

		case 0x02: /* SWP  */
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[jbi_stack_ptr - 2];
				jbi_stack[jbi_stack_ptr - 2] = jbi_stack[jbi_stack_ptr - 1];
				jbi_stack[jbi_stack_ptr - 1] = long_temp;
			}
			break;

		case 0x03: /* ADD  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] += jbi_stack[jbi_stack_ptr];
			}
			break;

		case 0x04: /* SUB  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] -= jbi_stack[jbi_stack_ptr];
			}
			break;

		case 0x05: /* MULT */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] *= jbi_stack[jbi_stack_ptr];
			}
			break;

		case 0x06: /* DIV  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				if (jbi_stack[jbi_stack_ptr] == 0)
				{
					status = JBIC_DIVIDE_BY_ZERO;
				}
				else
				{
					jbi_stack[jbi_stack_ptr - 1] /= jbi_stack[jbi_stack_ptr];
				}
			}
			break;

		case 0x07: /* MOD  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] %= jbi_stack[jbi_stack_ptr];
			}
			break;

		case 0x08: /* SHL  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] <<= jbi_stack[jbi_stack_ptr];
			}
			break;

		case 0x09: /* SHR  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] >>= jbi_stack[jbi_stack_ptr];;
			}
			break;

		case 0x0A: /* NOT  */
			IF_CHECK_STACK(1)
			{
				jbi_stack[jbi_stack_ptr - 1] ^= (-1L);
			}
			break;

		case 0x0B: /* AND  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] &= jbi_stack[jbi_stack_ptr];
			}
			break;

		case 0x0C: /* OR   */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] |= jbi_stack[jbi_stack_ptr];
			}
			break;

		case 0x0D: /* XOR  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] ^= jbi_stack[jbi_stack_ptr];
			}
			break;

		case 0x0E: /* INV */
			IF_CHECK_STACK(1)
			{
				jbi_stack[jbi_stack_ptr - 1] = jbi_stack[jbi_stack_ptr - 1] ? 0L : 1L;
			}
			break;

		case 0x0F: /* GT   */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] =
					(jbi_stack[jbi_stack_ptr - 1] > jbi_stack[jbi_stack_ptr]) ? 1L : 0L;
			}
			break;

		case 0x10: /* LT   */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] =
					(jbi_stack[jbi_stack_ptr - 1] < jbi_stack[jbi_stack_ptr]) ? 1L : 0L;
			}
			break;

		case 0x11: /* RET  */
			IF_CHECK_STACK(1)
			{
				pc = jbi_stack[--jbi_stack_ptr] + code_section;
				CHECK_PC;
				if (pc == code_section)
				{
					status = JBIC_BOUNDS_ERROR;
				}
			}
			break;

		case 0x12: /* CMPS */
			/*
			*	Array short compare
			*	...stack 0 is source 1 value
			*	...stack 1 is source 2 value
			*	...stack 2 is mask value
			*	...stack 3 is count
			*/
			IF_CHECK_STACK(4)
			{
				c = jbi_stack[--jbi_stack_ptr];
				d = jbi_stack[--jbi_stack_ptr];
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[jbi_stack_ptr - 1];

				if (count > 32)
				{
					status = JBIC_BOUNDS_ERROR;
				}
				else
				{
					long_temp &= ((-1L) >> (32 - count));

					jbi_stack[jbi_stack_ptr - 1] =
						((c & long_temp) == (d & long_temp)) ? 1L : 0L;
				}
			}
			break;

		case 0x13: /* PINT */
			/*
			*	PRINT add integer
			*	...stack 0 is integer value
			*/
			IF_CHECK_STACK(1)
			{
				jbi_ltoa(&message_buffer[jbi_strlen(message_buffer)],
					jbi_stack[--jbi_stack_ptr]);
			}
			break;

		case 0x14: /* PRNT */
			/*
			*	PRINT finish
			*/
			jbi_message(message_buffer);
			message_buffer[0] = '\0';
			break;

		case 0x15: /* DSS  */
			/*
			*	DRSCAN short
			*	...stack 0 is scan data
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				jbi_make_dword(charbuf, long_temp);
				status = jbi_do_drscan(count, charbuf, 0L);
			}
			break;

		case 0x16: /* DSSC */
			/*
			*	DRSCAN short with capture
			*	...stack 0 is scan data
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[jbi_stack_ptr - 1];
				jbi_make_dword(charbuf, long_temp);
				status = jbi_swap_dr(count, charbuf, 0L, charbuf, 0);
				jbi_stack[jbi_stack_ptr - 1] = jbi_get_dword(charbuf);
			}
			break;

		case 0x17: /* ISS  */
			/*
			*	IRSCAN short
			*	...stack 0 is scan data
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				jbi_make_dword(charbuf, long_temp);
				status = jbi_do_irscan(count, charbuf, 0);
			}
			break;

		case 0x18: /* ISSC */
			/*
			*	IRSCAN short with capture
			*	...stack 0 is scan data
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[jbi_stack_ptr - 1];
				jbi_make_dword(charbuf, long_temp);
				status = jbi_swap_ir(count, charbuf, 0, charbuf, 0);
				jbi_stack[jbi_stack_ptr - 1] = jbi_get_dword(charbuf);
			}
			break;

		case 0x19: /* VSS  */
			/*
			*	VECTOR short
			*	...stack 0 is scan data
			*	...stack 1 is count
			*/
			status = JBIC_ILLEGAL_OPCODE;
			break;

		case 0x1A: /* VSSC */
			/*
			*	VECTOR short with capture
			*	...stack 0 is scan data
			*	...stack 1 is count
			*/
			status = JBIC_ILLEGAL_OPCODE;
			break;

		case 0x1B: /* VMPF */
			/*
			*	VMAP finish
			*/
			status = JBIC_ILLEGAL_OPCODE;
			break;

		case 0x1C: /* DPR  */
			IF_CHECK_STACK(1)
			{
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				status = jbi_set_dr_preamble(count, 0, NULL);
			}
			break;

		case 0x1D: /* DPRL */
			/*
			*	DRPRE with literal data
			*	...stack 0 is count
			*	...stack 1 is literal data
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				jbi_make_dword(charbuf, long_temp);
				status = jbi_set_dr_preamble(count, 0, charbuf);
			}
			break;

		case 0x1E: /* DPO  */
			/*
			*	DRPOST
			*	...stack 0 is count
			*/
			IF_CHECK_STACK(1)
			{
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				status = jbi_set_dr_postamble(count, 0, NULL);
			}
			break;

		case 0x1F: /* DPOL */
			/*
			*	DRPOST with literal data
			*	...stack 0 is count
			*	...stack 1 is literal data
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				jbi_make_dword(charbuf, long_temp);
				status = jbi_set_dr_postamble(count, 0, charbuf);
			}
			break;

		case 0x20: /* IPR  */
			IF_CHECK_STACK(1)
			{
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				status = jbi_set_ir_preamble(count, 0, NULL);
			}
			break;

		case 0x21: /* IPRL */
			/*
			*	IRPRE with literal data
			*	...stack 0 is count
			*	...stack 1 is literal data
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				jbi_make_dword(charbuf, long_temp);
				status = jbi_set_ir_preamble(count, 0, charbuf);
			}
			break;

		case 0x22: /* IPO  */
			/*
			*	IRPOST
			*	...stack 0 is count
			*/
			IF_CHECK_STACK(1)
			{
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				status = jbi_set_ir_postamble(count, 0, NULL);
			}
			break;

		case 0x23: /* IPOL */
			/*
			*	IRPOST with literal data
			*	...stack 0 is count
			*	...stack 1 is literal data
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				jbi_make_dword(charbuf, long_temp);
				status = jbi_set_ir_postamble(count, 0, charbuf);
			}
			break;

		case 0x24: /* PCHR */
			IF_CHECK_STACK(1)
			{
				count = jbi_strlen(message_buffer);
				ch = (char) jbi_stack[--jbi_stack_ptr];
				if ((ch < 1) || (ch > 127))
				{
					/* character code out of range */
					/* instead of flagging an error, force the value to 127 */
					ch = 127;
				}
				message_buffer[count] = ch;
				message_buffer[count + 1] = '\0';
			}
			break;

		case 0x25: /* EXIT */
			IF_CHECK_STACK(1)
			{
				*exit_code = (int) jbi_stack[--jbi_stack_ptr];
			}
			done = 1;
			break;

		case 0x26: /* EQU  */
			IF_CHECK_STACK(2)
			{
				--jbi_stack_ptr;
				jbi_stack[jbi_stack_ptr - 1] =
					(jbi_stack[jbi_stack_ptr - 1] == jbi_stack[jbi_stack_ptr])
					? 1L : 0L;
			}
			break;

		case 0x27: /* POPT  */
			IF_CHECK_STACK(1)
			{
				--jbi_stack_ptr;
			}
			break;

		case 0x40: /* PSHL */
			jbi_stack[jbi_stack_ptr++] = args[0];
			break;

		case 0x41: /* PSHV */
			jbi_stack[jbi_stack_ptr++] = variables[args[0]];
			break;

		case 0x42: /* JMP  */
			pc = args[0] + code_section;
			CHECK_PC;
			break;

		case 0x43: /* CALL */
			jbi_stack[jbi_stack_ptr++] = pc;
			pc = args[0] + code_section;
			CHECK_PC;
			break;

		case 0x44: /* NEXT */
			/*
			*	Process FOR / NEXT loop
			*	...argument 0 is variable ID
			*	...stack 0 is step value
			*	...stack 1 is end value
			*	...stack 2 is top address
			*/
			IF_CHECK_STACK(3)
			{
				variable_id = (unsigned int) args[0];
				step = jbi_stack[jbi_stack_ptr - 1];
				end = jbi_stack[jbi_stack_ptr - 2];
				top = jbi_stack[jbi_stack_ptr - 3];
				iterator = variables[variable_id];
				break_out = 0;

				if (step < 0)
				{
					if (iterator <= end) break_out = 1;
				}
				else
				{
					if (iterator >= end) break_out = 1;
				}

				if (break_out)
				{
					jbi_stack_ptr -= 3;
				}
				else
				{
					variables[variable_id] = iterator + step;
					pc = top + code_section;
					CHECK_PC;
				}
			}
			break;

		case 0x45: /* PSTR */
			/*
			*	PRINT add string
			*	...argument 0 is string ID
			*/
			count = jbi_strlen(message_buffer);
			jbi_strncpy(&message_buffer[count],
				&jbi_program[string_table + args[0]],
				JBI_MESSAGE_LENGTH - count);
			message_buffer[JBI_MESSAGE_LENGTH] = '\0';
			break;

		case 0x46: /* VMAP */
			/*
			*	VMAP add signal name
			*	...argument 0 is string ID
			*/
			status = JBIC_ILLEGAL_OPCODE;
			break;

		case 0x47: /* SINT */
			/*
			*	STATE intermediate state
			*	...argument 0 is state code
			*/
			status = jbi_goto_jtag_state((JBI_JTAG_STATE) args[0]);
			break;

		case 0x48: /* ST   */
			/*
			*	STATE final state
			*	...argument 0 is state code
			*/
			status = jbi_goto_jtag_state((JBI_JTAG_STATE) args[0]);
			break;

		case 0x49: /* ISTP */
			/*
			*	IRSTOP state
			*	...argument 0 is state code
			*/
			jbi_irstop_state = (JBI_JTAG_STATE) args[0];
			break;

		case 0x4A: /* DSTP */
			/*
			*	DRSTOP state
			*	...argument 0 is state code
			*/
			jbi_drstop_state = (JBI_JTAG_STATE) args[0];
			break;

		case 0x4B: /* SWPN */
			/*
			*	Exchange top with Nth stack value
			*	...argument 0 is 0-based stack entry to swap with top element
			*/
			index = ((unsigned int) args[0]) + 1;
			IF_CHECK_STACK(index)
			{
				long_temp = jbi_stack[jbi_stack_ptr - index];
				jbi_stack[jbi_stack_ptr - index] = jbi_stack[jbi_stack_ptr - 1];
				jbi_stack[jbi_stack_ptr - 1] = long_temp;
			}
			break;

		case 0x4C: /* DUPN */
			/*
			*	Duplicate Nth stack value
			*	...argument 0 is 0-based stack entry to duplicate
			*/
			index = ((unsigned int) args[0]) + 1;
			IF_CHECK_STACK(index)
			{
				jbi_stack[jbi_stack_ptr] = jbi_stack[jbi_stack_ptr - index];
				++jbi_stack_ptr;
			}
			break;

		case 0x4D: /* POPV */
			/*
			*	Pop stack into scalar variable
			*	...argument 0 is variable ID
			*	...stack 0 is value
			*/
			IF_CHECK_STACK(1)
			{
				variables[args[0]] = jbi_stack[--jbi_stack_ptr];
			}
			break;

		case 0x4E: /* POPE */
			/*
			*	Pop stack into integer array element
			*	...argument 0 is variable ID
			*	...stack 0 is array index
			*	...stack 1 is value
			*/
			IF_CHECK_STACK(2)
			{
				variable_id = (unsigned int) args[0];

				/* check that variable is a writable integer array */
				if ((attributes[variable_id] & 0x1c) != 0x18)
				{
					status = JBIC_BOUNDS_ERROR;
				}
				else
				{
					longptr_temp = (long XDATA_AREA *) variables_x[variable_id];

					/* pop the array index */
					index = (unsigned int) jbi_stack[--jbi_stack_ptr];

					/* pop the value and store it into the array */
					longptr_temp[index] = jbi_stack[--jbi_stack_ptr];
				}
			}
			break;

		case 0x4F: /* POPA */
			/*
			*	Pop stack into Boolean array
			*	...argument 0 is variable ID
			*	...stack 0 is count
			*	...stack 1 is array index
			*	...stack 2 is value
			*/
			IF_CHECK_STACK(3)
			{
				variable_id = (unsigned int) args[0];

				/* check that variable is a writable Boolean array */
				if ((attributes[variable_id] & 0x1c) != 0x08)
				{
					status = JBIC_BOUNDS_ERROR;
				}
				else
				{
					charptr_temp = variables_x[variable_id];

					/* pop the count (number of bits to copy) */
					count = (unsigned int) jbi_stack[--jbi_stack_ptr];

					/* pop the array index */
					index = (unsigned int) jbi_stack[--jbi_stack_ptr];

					/* pop the data */
					long_temp = jbi_stack[--jbi_stack_ptr];

					if (count < 1)
					{
						status = JBIC_BOUNDS_ERROR;
					}
					else
					{
						for (i = 0; i < count; ++i)
						{
							if (long_temp & (1L << i))
							{
								charptr_temp[index >> 3] |= (1 << (index & 7));
							}
							else
							{
								charptr_temp[index >> 3] &=
									~ (unsigned char) (1 << (index & 7));
							}
							++index;
						}
					}
				}
			}
			break;

		case 0x50: /* JMPZ */
			/*
			*	Pop stack and branch if zero
			*	...argument 0 is address
			*	...stack 0 is condition value
			*/
			IF_CHECK_STACK(1)
			{
				if (jbi_stack[--jbi_stack_ptr] == 0)
				{
					pc = args[0] + code_section;
					CHECK_PC;
				}
			}
			break;

		case 0x51: /* DS   */
			/*
			*	DRSCAN
			*	...argument 0 is scan data variable ID
			*	...stack 0 is array index
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				long_index = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1e) == 0x0e)
				{
					/* initialized compressed Boolean array */
					jbi_uncompress_page(variable_id, (int) (long_index >> 16));
					long_index &= 0x0000ffff;
					unknown_ptr = jbi_aca_out_buffer;
				}
				else if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					/* initialized Boolean array */
					unknown_ptr = variables_c[variable_id];
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					unknown_ptr = variables_x[variable_id];
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}

				status = jbi_do_drscan(count, unknown_ptr, long_index);
			}
			break;

		case 0x52: /* IS   */
			/*
			*	IRSCAN
			*	...argument 0 is scan data variable ID
			*	...stack 0 is array index
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				index = (unsigned int) jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					status = jbi_do_irscan(count, variables_c[variable_id],
						index);
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					status = jbi_do_irscan(count, variables_x[variable_id],
						index);
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}
			}
			break;

		case 0x53: /* DPRA */
			/*
			*	DRPRE with array data
			*	...argument 0 is variable ID
			*	...stack 0 is array index
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				index = (unsigned int) jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					unknown_ptr = variables_c[variable_id];
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					unknown_ptr = variables_x[variable_id];
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}

				status = jbi_set_dr_preamble(count, index, unknown_ptr);
			}
			break;

		case 0x54: /* DPOA */
			/*
			*	DRPOST with array data
			*	...argument 0 is variable ID
			*	...stack 0 is array index
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				index = (unsigned int) jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					unknown_ptr = variables_c[variable_id];
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					unknown_ptr = variables_x[variable_id];
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}

				status = jbi_set_dr_postamble(count, index, unknown_ptr);
			}
			break;

		case 0x55: /* IPRA */
			/*
			*	IRPRE with array data
			*	...argument 0 is variable ID
			*	...stack 0 is array index
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				index = (unsigned int) jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					unknown_ptr = variables_c[variable_id];
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					unknown_ptr = variables_x[variable_id];
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}

				status = jbi_set_ir_preamble(count, index, unknown_ptr);
			}
			break;

		case 0x56: /* IPOA */
			/*
			*	IRPOST with array data
			*	...argument 0 is variable ID
			*	...stack 0 is array index
			*	...stack 1 is count
			*/
			IF_CHECK_STACK(2)
			{
				index = (unsigned int) jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					unknown_ptr = variables_c[variable_id];
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					unknown_ptr = variables_x[variable_id];
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}

				status = jbi_set_ir_postamble(count, index, unknown_ptr);
			}
			break;

		case 0x57: /* EXPT */
			/*
			*	EXPORT
			*	...argument 0 is string ID
			*	...stack 0 is integer expression
			*/
			IF_CHECK_STACK(1)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];
				jbi_export(&jbi_program[string_table + args[0]], long_temp);
			}
			break;

		case 0x58: /* PSHE */
			/*
			*	Push integer array element
			*	...argument 0 is variable ID
			*	...stack 0 is array index
			*/
			IF_CHECK_STACK(1)
			{
				variable_id = (unsigned int) args[0];
				index = (unsigned int) jbi_stack[jbi_stack_ptr - 1];

				/* check variable type */
				if ((attributes[variable_id] & 0x1f) == 0x19)
				{
					/* writable integer array */
					longptr_temp = (long XDATA_AREA *) variables_x[variable_id];
					jbi_stack[jbi_stack_ptr - 1] = longptr_temp[index];
				}
				else if ((attributes[variable_id] & 0x1f) == 0x1c)
				{
					/* read-only integer array */
					long_temp = variables[variable_id] + (4L * index);
					jbi_stack[jbi_stack_ptr - 1] = GET_DWORD(long_temp);
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}
			}
			break;

		case 0x59: /* PSHA */
			/*
			*	Push Boolean array
			*	...argument 0 is variable ID
			*	...stack 0 is count
			*	...stack 1 is array index
			*/
			IF_CHECK_STACK(2)
			{
				/* pop the count (number of bits to copy) */
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];

				/* pop the array index */
				index = (unsigned int) jbi_stack[jbi_stack_ptr - 1];

				variable_id = (unsigned int) args[0];

				if ((count < 1) || (count > 32))
				{
					status = JBIC_BOUNDS_ERROR;
				}
				else if ((attributes[variable_id] & 0x1e) == 0x0e)
				{
					/* initialized compressed Boolean array */
					jbi_uncompress_page(variable_id,
						(int) (jbi_stack[jbi_stack_ptr - 1] >> 16));

					long_temp = 0L;

					for (i = 0; i < count; ++i)
					{
						if (jbi_aca_out_buffer[(i + index) >> 3] &
							(1 << ((i + index) & 7)))
						{
							long_temp |= (1L << i);
						}
					}

					jbi_stack[jbi_stack_ptr - 1] = long_temp;
				}
				else if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					/* variable is an initialized Boolean array */
					ccptr = variables_c[variable_id];

					long_temp = 0L;

					for (i = 0; i < count; ++i)
					{
						if (ccptr[(i + index) >> 3] & (1 << ((i + index) & 7)))
						{
							long_temp |= (1L << i);
						}
					}

					jbi_stack[jbi_stack_ptr - 1] = long_temp;
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					/* variable is a writable Boolean array */
					charptr_temp = variables_x[variable_id];

					long_temp = 0L;

					for (i = 0; i < count; ++i)
					{
						if (charptr_temp[(i + index) >> 3] &
							(1 << ((i + index) & 7)))
						{
							long_temp |= (1L << i);
						}
					}

					jbi_stack[jbi_stack_ptr - 1] = long_temp;
				}
				else
				{
					/* variable is not a Boolean array */
					status = JBIC_BOUNDS_ERROR;
				}
			}
			break;

		case 0x5A: /* DYNA */
			/*
			*	Dynamically change size of array
			*	...argument 0 is variable ID
			*	...stack 0 is new size
			*/
			IF_CHECK_STACK(1)
			{
				variable_id = (unsigned int) args[0];
				long_temp = jbi_stack[--jbi_stack_ptr];

				if (long_temp > variable_size[variable_id])
				{
					variable_size[variable_id] = long_temp;

					if (attributes[variable_id] & 0x10)
					{
						/* allocate integer array */
						long_temp *= 4;
					}
					else
					{
						/* allocate Boolean array */
						long_temp = (long_temp + 7) >> 3;
					}

					variables_x[variable_id] =
						jbi_malloc((unsigned int) long_temp);

					if (variables_x[variable_id] == NULL)
					{
						status = JBIC_OUT_OF_MEMORY;
					}
				}
			}
			break;

		case 0x80: /* COPY */
			/*
			*	Array copy
			*	...argument 0 is dest ID
			*	...argument 1 is source ID
			*	...stack 0 is count
			*	...stack 1 is dest index
			*	...stack 2 is source index
			*/
			IF_CHECK_STACK(3)
			{
				unsigned long long_index2;
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];
				long_index = jbi_stack[--jbi_stack_ptr];
				long_index2 = jbi_stack[--jbi_stack_ptr];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1e) == 0x0e)
				{
					/* initialized compressed Boolean array */
					jbi_uncompress_page(variable_id, (int) (long_index >> 16));
					long_index &= 0x0000ffff;
					unknown_ptr = jbi_aca_out_buffer;
				}
				else if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					unknown_ptr = variables_c[variable_id];
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					unknown_ptr = variables_x[variable_id];
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}

				charptr_temp = variables_x[args[1]];

				for (i = 0; i < count; ++i)
				{
					if (unknown_ptr[long_index >> 3] & (1 << (long_index & 7)))
					{
						charptr_temp[long_index2 >> 3] |= (1 << (long_index2 & 7));
					}
					else
					{
						charptr_temp[long_index2 >> 3] &=
							~(unsigned char) (1 << (long_index2 & 7));
					}
					++long_index;
					++long_index2;
				}
			}
			break;

		case 0x81: /* REVA */
			/*
			*	ARRAY COPY reversing bit order
			*	...argument 0 is dest ID
			*	...argument 1 is source ID
			*	...stack 0 is dest index
			*	...stack 1 is source index
			*	...stack 2 is count
			*/
			status = JBIC_ILLEGAL_OPCODE;
			break;

		case 0x82: /* DSC  */
			/*
			*	DRSCAN with capture
			*	...argument 0 is scan data variable ID
			*	...argument 1 is capture variable ID
			*	...stack 0 is capture index
			*	...stack 1 is scan data index
			*	...stack 2 is count
			*/
			IF_CHECK_STACK(3)
			{
				index = (unsigned int) jbi_stack[--jbi_stack_ptr];
				long_index = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1e) == 0x0e)
				{
					/* initialized compressed Boolean array */
					jbi_uncompress_page(variable_id, (int) (long_index >> 16));
					long_index &= 0x0000ffff;
					unknown_ptr = jbi_aca_out_buffer;
				}
				else if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					unknown_ptr = variables_c[variable_id];
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					unknown_ptr = variables_x[variable_id];
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}

				charptr_temp = variables_x[args[1]];

				status = jbi_swap_dr(count, unknown_ptr, long_index,
					charptr_temp, index);
			}
			break;

		case 0x83: /* ISC  */
			/*
			*	IRSCAN with capture
			*	...argument 0 is scan data variable ID
			*	...argument 1 is capture variable ID
			*	...stack 0 is scan data index
			*	...stack 1 is capture index
			*	...stack 2 is count
			*/
			IF_CHECK_STACK(3)
			{
				index = (unsigned int) jbi_stack[--jbi_stack_ptr];
				index2 = (unsigned int) jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[--jbi_stack_ptr];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1c) == 0x0c)
				{
					unknown_ptr = variables_c[variable_id];
				}
				else if ((attributes[variable_id] & 0x1c) == 0x08)
				{
					unknown_ptr = variables_x[variable_id];
				}
				else
				{
					status = JBIC_BOUNDS_ERROR;
				}

				charptr_temp = variables_x[args[1]];

				status = jbi_swap_ir(count, unknown_ptr, index2,
					charptr_temp, index);
			}
			break;

		case 0x84: /* WAIT */
			/*
			*	WAIT
			*	...argument 0 is wait state
			*	...argument 1 is end state
			*	...stack 0 is cycles
			*	...stack 1 is microseconds
			*/
			IF_CHECK_STACK(2)
			{
				long_temp = jbi_stack[--jbi_stack_ptr];

				if (long_temp != 0L)
				{
					status = jbi_do_wait_cycles(long_temp,
						(JBI_JTAG_STATE) args[0]);
				}

				long_temp = jbi_stack[--jbi_stack_ptr];

				if ((status == JBIC_SUCCESS) && (long_temp != 0L))
				{
					status = jbi_do_wait_microseconds(long_temp,
						(JBI_JTAG_STATE) args[0]);
				}

				if ((status == JBIC_SUCCESS) && (args[1] != args[0]))
				{
					status = jbi_goto_jtag_state((JBI_JTAG_STATE) args[1]);
				}
			}
			break;

		case 0x85: /* VS   */
			/*
			*	VECTOR
			*	...argument 0 is dir data variable ID
			*	...argument 1 is scan data variable ID
			*	...stack 0 is dir array index
			*	...stack 1 is scan array index
			*	...stack 2 is count
			*/
			status = JBIC_ILLEGAL_OPCODE;
			break;

		case 0xC0: /* CMPA */
			/*
			*	Array compare
			*	...argument 0 is source 1 ID
			*	...argument 1 is source 2 ID
			*	...argument 2 is mask ID
			*	...stack 0 is source 1 index
			*	...stack 1 is source 2 index
			*	...stack 2 is mask index
			*	...stack 3 is count
			*/
			IF_CHECK_STACK(4)
			{
				long XDATA_AREA source1_index, source2_index, mask_index;
				unsigned char XDATA_AREA a, b;

				source1_index = jbi_stack[--jbi_stack_ptr];
				source2_index = jbi_stack[--jbi_stack_ptr];
				mask_index = jbi_stack[--jbi_stack_ptr];
				count = (unsigned int) jbi_stack[jbi_stack_ptr - 1];

				variable_id = (unsigned int) args[0];
				if ((attributes[variable_id] & 0x1e) == 0x0e)
				{
					jbi_uncompress_page(variable_id,
						(int) (source1_index >> 16));
					source1_index &= 0x0000ffff;
					source1 = jbi_aca_out_buffer;
				}
				else if (attributes[variable_id] & 0x04)
				{
					source1 = variables_c[variable_id];
				}
				else source1 = variables_x[variable_id];

				variable_id = (unsigned int) args[1];
				if ((attributes[variable_id] & 0x1e) == 0x0e)
				{
					jbi_uncompress_page(variable_id,
						(int) (source2_index >> 16));
					source2_index &= 0x0000ffff;
					source2 = jbi_aca_out_buffer;
				}
				else if (attributes[variable_id] & 0x04)
				{
					source2 = variables_c[variable_id];
				}
				else source2 = variables_x[variable_id];

				variable_id = (unsigned int) args[2];
				if (attributes[variable_id] & 0x04)
				{
					mask = variables_c[variable_id];
				}
				else mask = variables_x[variable_id];

				long_temp = 1L;

				if (count < 1)
				{
					status = JBIC_BOUNDS_ERROR;
				}
				else
				{
					for (i = 0; i < count; ++i)
					{
						if (mask[mask_index >> 3] & (1 << (mask_index & 7)))
						{
							a = source1[source1_index >> 3] &
								(1 << (source1_index & 7)) ? 1 : 0;
							b = source2[source2_index >> 3] &
								(1 << (source2_index & 7)) ? 1 : 0;

							if (a != b) long_temp = 0L;	/* failure */
						}
						++source1_index;
						++source2_index;
						++mask_index;
					}
				}

				jbi_stack[jbi_stack_ptr - 1] = long_temp;
			}
			break;

		case 0xC1: /* VSC  */
			/*
			*	VECTOR with capture
			*	...argument 0 is dir data variable ID
			*	...argument 1 is scan data variable ID
			*	...argument 2 is capture variable ID
			*	...stack 0 is capture index
			*	...stack 1 is scan data index
			*	...stack 2 is dir data index
			*	...stack 3 is count
			*/
			status = JBIC_ILLEGAL_OPCODE;
			break;

		default:
			/*
			*	Unrecognized opcode -- ERROR!
			*/
			status = JBIC_ILLEGAL_OPCODE;
			break;
		}

  		if ((jbi_stack_ptr < 0) || (jbi_stack_ptr >= JBI_STACK_SIZE))
		{
			status = JBIC_STACK_OVERFLOW;
		}

		if (status != JBIC_SUCCESS)
		{
			done = 1;
			*error_address = (unsigned int) (opcode_address - code_section);
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_get_note
(
	unsigned int XDATA_AREA *offset,
	char XDATA_AREA *key,
	char XDATA_AREA *value,
	unsigned int length
)

/*																			*/
/*	Description:	Gets key and value of NOTE fields in the JBC file.		*/
/*					Can be called in two modes:  if offset pointer is NULL,	*/
/*					then the function searches for note fields which match	*/
/*					the key string provided.  If offset is not NULL, then	*/
/*					the function finds the next note field of any key,		*/
/*					starting at the offset specified by the offset pointer.	*/
/*																			*/
/*	Returns:		JBIC_SUCCESS for success, else appropriate error code	*/
/*																			*/
/****************************************************************************/
{
	JBI_RETURN_TYPE status = JBIC_UNEXPECTED_END;
	unsigned int XDATA_AREA note_strings = 0L;
	unsigned int XDATA_AREA note_table = 0L;
	unsigned int XDATA_AREA note_count = 0L;
	char FILE_AREA *note_ptr;
	unsigned int i;

	/*
	*	Read header information
	*/
	note_strings  = (unsigned int) GET_DWORD(8);
	note_table    = (unsigned int) GET_DWORD(12);
	note_count    = (unsigned int) GET_DWORD(44);

	if (note_count > 0)
	{
		note_ptr = &jbi_program[note_strings];

		if (offset == NULL)
		{
			/*
			*	We will search for the first note with a specific key, and
			*	return only the value
			*/
			for (i = 0; (i < note_count) && (status != JBIC_SUCCESS); ++i)
			{
				if ((key != NULL) && (jbi_stricmp(key,
					&note_ptr[GET_DWORD(note_table + (4 * i))]) == 0))
				{
					status = JBIC_SUCCESS;

					if (value != NULL)
					{
						jbi_strncpy(value,
							&note_ptr[GET_DWORD(note_table + (4 * i) + 4)],
							length);
					}
				}
			}
		}
		else
		{
			/*
			*	We will search for the next note, regardless of the key, and
			*	return both the value and the key
			*/

			i = *offset;

			if ((i >= 0) && (i < note_count))
			{
				status = JBIC_SUCCESS;

				if (key != NULL)
				{
					jbi_strncpy(key,
						&note_ptr[GET_DWORD(note_table + (8 * i))],
						length);
				}

				if (value != NULL)
				{
					jbi_strncpy(value,
						&note_ptr[GET_DWORD(note_table + (8 * i) + 4)],
						length);
				}

				*offset = i + 1;
			}
		}
	}

	return (status);
}

/****************************************************************************/
/*																			*/

JBI_RETURN_TYPE jbi_check_crc
(
	unsigned int XDATA_AREA *expected_crc,
	unsigned int XDATA_AREA *actual_crc
)

/*																			*/
/*	Description:	This function reads the entire input file and computes	*/
/*					the CRC of everything up to the CRC field.				*/
/*																			*/
/*	Returns:		JBIC_SUCCESS for success, JBIC_CRC_ERROR for failure	*/
/*																			*/
/****************************************************************************/
{
	unsigned int XDATA_AREA local_expected, local_actual;
	unsigned int shift_reg = 0xffff;
	unsigned char j, feedback, databyte;
	unsigned int XDATA_AREA crc_section;
	unsigned int i;

	crc_section = (unsigned int) GET_DWORD(32);
	local_expected = GET_WORD(crc_section);
	if (expected_crc != NULL) *expected_crc = local_expected;

	for (i = 0; i < crc_section; ++i)
	{
		databyte = jbi_program[i];
		for (j = 0; j < 8; j++)	/* compute for each bit */
		{
			feedback = (databyte ^ shift_reg) & 0x01;
			shift_reg >>= 1;	/* shift the shift register */
			if (feedback) shift_reg ^= 0x8408;	/* invert selected bits */
			databyte >>= 1;		/* get the next bit of input_byte */
		}
	}

	local_actual = ~shift_reg;
	if (actual_crc != NULL) *actual_crc = local_actual;

	return ((local_expected == local_actual) ? JBIC_SUCCESS : JBIC_CRC_ERROR);
}

/****************************************************************************/
/*																			*/
/*	The following function implements a HEX file loader.  This function		*/
/*	uses interrupt-driven serial communications with a 256-byte FIFO queue.	*/
/*																			*/
/****************************************************************************/

#ifdef JBC_FILE_IN_RAM

JBI_RETURN_TYPE jbi_load_jbc_file(void)
{
	JBI_RETURN_TYPE XDATA_AREA crc_result = JBIC_IO_ERROR;
	unsigned int XDATA_AREA expected_crc;
	unsigned int XDATA_AREA actual_crc;
	unsigned int XDATA_AREA note_offset = 0;
	unsigned int XDATA_AREA address;
	unsigned int XDATA_AREA count;
	char ch, value, pos, size, type, offset;
	char databyte, checksum, error, done;

	pos = 0;
	type = 0;
	done = 0;
	error = 0;
	count = 0;
	checksum = 0;

	EA = 0;			/* disable interrupts */

	/* reset the FIFO indicies */
	jbi_comm_in_position = 0;
	jbi_comm_out_position = 0;
	jbi_comm_overrun = 0;

	/* set up the serial interface registers */
	TI_0 = 0;		/* clear transmit interrupt */
	RI_0 = 0;		/* clear receiver interrupt */
	ES = 1;			/* enable serial interrupts */
	PS = 0;			/* set serial interrupts to low priority */
	EA = 1;			/* enable interrupts */

	/* get data and interpret according to Intel HEX file format */
	while (!done)
	{
		ch = jbi_comm_getchar();

		if ((ch >= '0') && (ch <= '9'))
		{
			value = ch - '0';
		}
		else if ((ch >= 'A') && (ch <= 'F'))
		{
			value = ch - ('A' - 10);
		}
		else if ((ch >= 'a') && (ch <= 'f'))
		{
			value = ch - ('a' - 10);
		}
		else
		{
			value = -1;
		}

		if (value >= 0)
		{
			/* valid data character */
			if (pos & 1) checksum += (value << 4);
			else checksum += value;
		}
		else if (ch == ':')
		{
			/* beginning of a HEX record */
		}
		else
		{
			/* end of a HEX record */

			if (checksum != 0)
			{
				printf("Checksum Error: %02bX\n", checksum);
				error = 1;
				done = 1;
			}
			if (type != 0)
			{
				done = 1;
			}

			checksum = 0;
			pos = 0;
		}
	
		switch(pos)
		{
		case 0:
			if (ch != ':') --pos;
			break;

		case 1:		/* MSB size */
			size = value << 4;
			break;

		case 2:		/* LSB size */
			size |= value;
			break;

		case 3:		/* MSB address */
			address = value << 12;
			break;

		case 4:
			address |= value << 8;
			break;

		case 5:
			address |= value << 4;
			break;

		case 6:		/* LSB address */
			address |= value;
			break;

		case 7:		/* MSB type */
			type = value << 4;
			break;

		case 8:		/* LSB type */
			type |= value;
			break;

		default:
			if (type == 0)
			{
				offset = (pos - 9) / 2;

				if (pos & 1)
				{
					databyte = value << 4;
				}
				else
				{
					databyte |= value;

					if (offset < size)
					{
						if ((address + offset) < JBI_MAX_PROGRAM_SIZE)
						{
							jbi_program[address + offset] = databyte;
							++count;
						}
						else
						{
							printf("\nError: memory buffer size exceeded\n");
							error = 1;
							done = 1;
						}
					}
				}
			}
		}

		if (jbi_comm_overrun)
		{
			printf("\nError: communication buffer overrun\n");
			error = 1;
			done = 1;
		}

		++pos;
	}

	EA = 0;			/* disable interrupts */
	ES = 0;

	if (!error)
	{
		printf("\nLoaded %u bytes\n", count);

		if ((long) count == (jbi_get_program_dword(32) + 2L))
		{
			crc_result = jbi_check_crc(&expected_crc, &actual_crc);

			if (crc_result == JBIC_SUCCESS)
			{
				printf("CRC matched: expected %04X, actual %04X\n",
					expected_crc, actual_crc);
			}
			else
			{
				printf("CRC mismatch: expected %04X, actual %04X\n",
					expected_crc, actual_crc);
			}

			/*
			*	Dump out NOTE fields
			*/
			if (crc_result == JBIC_SUCCESS)
			{
				while (jbi_get_note(&note_offset,
					jbi_note_key, jbi_note_value, 256) == 0)
				{
					printf("NOTE \"%s\" = \"%s\"\n",
						jbi_note_key, jbi_note_value);
				}
			}
		}
	}

	return (crc_result);
}

#endif /* JBC_FILE_IN_RAM */

/****************************************************************************/
/*																			*/
/*	Main program															*/
/*																			*/
/****************************************************************************/

void main(void)
{
	unsigned int XDATA_AREA note_offset = 0;
	unsigned int XDATA_AREA error_address = 0;
	JBI_RETURN_TYPE XDATA_AREA crc_result = JBIC_SUCCESS;
	JBI_RETURN_TYPE XDATA_AREA exec_result = JBIC_SUCCESS;
	unsigned int XDATA_AREA expected_crc = 0;
	unsigned int XDATA_AREA actual_crc = 0;
	int XDATA_AREA exit_status = 0;
	int XDATA_AREA exit_code = 0;
	char CONSTANT_AREA *exit_string = NULL;
	char XDATA_AREA buffer[32];
	char CONSTANT_AREA **init_list;

	char CONSTANT_AREA banner[] =
		"Jam Byte Code Player Version 1.0\n"
		"Copyright (C) 1998 Altera Corporation\n";

#ifdef JBC_FILE_IN_RAM
	char CONSTANT_AREA usage_message[] =
		"Commands: Load, CRC, Notes, ID, Erase, Blankcheck, Program, Verify, All";
#else
	/* load command is not available when JBC file is in ROM area */
	char CONSTANT_AREA usage_message[] =
		"Commands: CRC, Notes, ID, Erase, Blankcheck, Program, Verify, All";
#endif /* JBC_FILE_IN_RAM */

	/*
	*	Initialize internal registers
	*/
	SCON0 = 0x50;
	SCON1 = 0x5C;
	TCON = 0xC0;
	TMOD = 0x20;
	PCON = 0xB0;
	TL0 = 0x00;
	TL1 = 0xD2;
	TH0 = 0x00;
	TH1 = 0xCA;
	CKCON = 0x11;
	WDCON = 0xC8;
	TR1 = 1;
	TI_0 = 1;
	TI_1 = 1;

	/* print out the version string and copyright message */
	puts(banner);
	puts(usage_message);

	while (1)
	{
		printf("> ");
		gets(buffer, 32);

		if ((buffer[0] == 'C') || (buffer[0] == 'c'))
		{
			/*
			*	Check CRC
			*/
			crc_result = jbi_check_crc(&expected_crc, &actual_crc);

			if (crc_result == JBIC_SUCCESS)
			{
				printf("CRC matched: expected %04X, actual %04X\n",
					expected_crc, actual_crc);
			}
			else
			{
				printf("CRC mismatch: expected %04X, actual %04X\n",
					expected_crc, actual_crc);
			}
		}
		else if ((buffer[0] == 'N') || (buffer[0] == 'n'))
		{
			/*
			*	Dump out NOTE fields
			*/
			if (crc_result == JBIC_SUCCESS)
			{
				note_offset = 0;
				while (jbi_get_note(&note_offset,
					jbi_note_key, jbi_note_value, 256) == 0)
				{
					printf("NOTE \"%s\" = \"%s\"\n",
						jbi_note_key, jbi_note_value);
				}
			}
		}
#ifdef JBC_FILE_IN_RAM
		else if ((buffer[0] == 'L') || (buffer[0] == 'l'))
		{
			/*
			*	Load a JBC program in Intel HEX format
			*/
			crc_result = jbi_load_jbc_file();
		}
#endif /* JBC_FILE_IN_RAM */
		else if ((buffer[0] == 'I') || (buffer[0] == 'i') ||
				 (buffer[0] == 'A') || (buffer[0] == 'a') ||
				 (buffer[0] == 'E') || (buffer[0] == 'e') ||
				 (buffer[0] == 'B') || (buffer[0] == 'b') ||
				 (buffer[0] == 'P') || (buffer[0] == 'p') ||
				 (buffer[0] == 'V') || (buffer[0] == 'v'))
		{
			/*
			*	Execute the JAM Bytecode program
			*/
			if ((buffer[0] == 'I') || (buffer[0] == 'i'))
				init_list = jbi_init_list_no_op;
			else if ((buffer[0] == 'A') || (buffer[0] == 'a'))
				init_list = jbi_init_list_all;
			else if ((buffer[0] == 'E') || (buffer[0] == 'e'))
				init_list = jbi_init_list_erase;
			else if ((buffer[0] == 'B') || (buffer[0] == 'b'))
				init_list = jbi_init_list_blankcheck;
			else if ((buffer[0] == 'P') || (buffer[0] == 'p'))
				init_list = jbi_init_list_program;
			else if ((buffer[0] == 'V') || (buffer[0] == 'v'))
				init_list = jbi_init_list_verify;
			else init_list = NULL;

			if (crc_result == JBIC_SUCCESS)
			{
				jbi_start_timer();

				exec_result = jbi_execute(init_list, &error_address, &exit_code);

				jbi_stop_timer();

				if (exec_result == JBIC_SUCCESS)
				{
					switch (exit_code)
					{
					case 0: exit_string = "Success"; break;
					case 1: exit_string = "Illegal initialization values"; break;
					case 2: exit_string = "Unrecognized device"; break;
					case 3: exit_string = "Device revision is not supported"; break;
					case 4: exit_string = "Device programming failure"; break;
					case 5: exit_string = "Device is not blank"; break;
					case 6: exit_string = "Device verify failure"; break;
					case 7: exit_string = "SRAM configuration failure"; break;
					default: exit_string = "Unknown exit code"; break;
					}

					printf("Exit code = %d... %s\n", exit_code, exit_string);
				}
				else
				{
					switch (exec_result)
					{
					case JBIC_OUT_OF_MEMORY:
						printf("Error: out of memory at address %04X\n",
							error_address);
						break;

					case JBIC_STACK_OVERFLOW:
						printf("Error: stack overflow at address %04X\n",
							error_address);
						break;

					case JBIC_IO_ERROR:
						printf("Error: invalid or corrupt JBC file\n");
						break;

					case JBIC_ILLEGAL_OPCODE:
						printf("Error: invalid instruction at address %04X\n",
							error_address);
						break;

					case JBIC_DIVIDE_BY_ZERO:
						printf("Error: division by zero at address %04X\n",
							error_address);
						break;

					case JBIC_INTERNAL_ERROR:
						printf("Error: internal error at address %04X\n",
							error_address);
						break;

					case JBIC_BOUNDS_ERROR:
						printf("Error: array access error at address %04X\n",
							error_address);
						break;

					default:
						printf("Unknown error code %u at address %04X\n",
							(unsigned int) exec_result, error_address);
						break;
					}
				}

				/*
				*	Print out the elapsed time in hours : minutes : seconds
				*/
				printf("Elapsed time = %02u:%02u:%02u\n",
					(jbi_timer_seconds / (60 * 60)),
					((jbi_timer_seconds / 60) % 60),
					(jbi_timer_seconds % 60));
			}
		}
		else if (buffer[0] >= ' ')
		{
			printf("Unknown command: %s\n", buffer);
			puts(usage_message);
		}
	}
}

#ifndef JBC_FILE_IN_RAM
unsigned char CONSTANT_AREA jbi_program[] =
{
	0	/* insert JBC program data here */
};
#endif /* JBC_FILE_IN_RAM */
