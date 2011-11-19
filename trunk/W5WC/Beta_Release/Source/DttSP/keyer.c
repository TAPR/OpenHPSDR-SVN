/* keyer.c */
/*
This file is part of a program that implements a Software-Defined Radio.

The code in this file is derived from routines originally written by
Pierre-Philippe Coupard for his CWirc X-chat program. That program
is issued under the GPL and is
Copyright (C) Pierre-Philippe Coupard - 18/06/2003

This derived version is
Copyright (C) 2004, 2005, 2006 by Frank Brickle, AB2KT and Bob McGwier, N4HY

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The authors can be reached by email at

ab2kt@arrl.net
or
rwmcgwier@comcast.net

or by paper mail at

The DTTS Microwave Society
6 Kathleen Place
Bridgewater, NJ 08807
*/

// see below for places where serial port is read
// and needs replacement for parallel port hookup

#include <keyer.h>

//========================================================================
// nothing affected by physical port connection here

#define KEYERSTATE_IDLE (0)
#define KEYERSTATE_DIT (1)
#define KEYERSTATE_DAH (2)
#define KEYERSTATE_TIMEOUT_DIT (3)
#define KEYERSTATE_TIMEOUT_DAH (4)
#define KEYERSTATE_TIMEOUT_CHARACTER_SPACE (5)



BOOLEAN
klogic (KeyerLogic kl,
	BOOLEAN dit,
	BOOLEAN dah,
	REAL wpm,
	int iambicmode,
	BOOLEAN midelementmodeB,
	BOOLEAN ditmemory,
	BOOLEAN dahmemory,
	BOOLEAN autocharspacing,
	BOOLEAN autowordspacing, int weight, REAL ticklen)
{
	REAL ditlen = 1200 / wpm;
	//REAL element_space = ditlen;
	int static keyerState = 0;
	int static last_element = 0;
	static BOOLEAN ditmem = 0;
	static BOOLEAN dahmem = 0;
	static BOOLEAN paddlesWereSqueezed = 0;
	static BOOLEAN paddlesAreNotSqueezed = 0;
	static BOOLEAN squeezeStartedAfterCurrentElementStartedPlaying = 0;	
	//squeezeStartedAfterCurrentElementStartedPlaying set to 0 after each element is finished playing
	//squeezeStartedAfterCurrentElementStartedPlaying set to 1 if squeezed after element started playing AND squeezeStartedBeforeCurrentElementStartedPlaying = 0
	static BOOLEAN squeezeStartedBeforeCurrentElementStartedPlaying = 0;	
	//squeezeStartedBeforeCurrentElementStartedPlaying set to 1 if ever squeezed during an element space or idle
	//squeezeStartedBeforeCurrentElementStartedPlaying set to 0 if ever not squeezed during an element space or idle
	static BOOLEAN squeezeReleasedBeforeMidpointAndModeBStrict = 0;
	//squeezeReleasedBeforeMidpoint only to be used for situations in which Mode B strict (50% rule) matters
	//squeezeReleasedBeforeMidpoint set to 1 when the early release condition for Mode B strict is met
	//squeezeReleasedBeforeMidpoint set to 0 after current element is completed
	static BOOLEAN afterflagdebug = 0;
	//afterflagdebug used to prevent debug line from being called multiple times

	switch(keyerState)
	{
		case (KEYERSTATE_IDLE):
			//fprintf(stdout, "KEYERSTATE_IDLE\n"), fflush(stdout);
			//fprintf(stdout, "midelementmodeB is %d\n", midelementmodeB), fflush(stdout);

			if(dit && dah)
				squeezeStartedBeforeCurrentElementStartedPlaying = 1;
				//paddlesWereSqueezed = 1;
			else
				squeezeStartedBeforeCurrentElementStartedPlaying = 0;
				//paddlesWereSqueezed = 0;

			if(dit)
			{
				kl->element.curr = DIT;
				kl->timeout.beep = (ditlen * (REAL) weight) / 50;
				kl->timeout.midl = kl->timeout.beep / 2;	//todo: add audio latency to this
				keyerState = KEYERSTATE_DIT;
			}
			else if(dah)
			{
				kl->element.curr = DAH;
				kl->timeout.beep = (ditlen * (REAL) weight) / 50 + ditlen * 2;
				kl->timeout.midl = kl->timeout.beep / 2;	//todo: add audio latency to this
				keyerState = KEYERSTATE_DAH;
			}
			break;

		case (KEYERSTATE_DIT):
			//fprintf(stdout, "KEYERSTATE_DIT\n"), fflush(stdout);

			if(dit && dah)
				squeezeStartedAfterCurrentElementStartedPlaying = 1;

			//decrement beep timer
			kl->timeout.beep = kl->timeout.beep - ticklen;
			kl->timeout.midl = kl->timeout.midl - ticklen;

			if((!dit || !dah) && squeezeStartedBeforeCurrentElementStartedPlaying && midelementmodeB && !(kl->timeout.midl <= 0) && dahmem)
			{
				fprintf(stdout, "KEYERSTATE_DAH(midway = %f):  dahmem = 0 (squeeze released BEFORE midpoint of element) \n", kl->timeout.midl), fflush(stdout);
				dahmem = 0;
				squeezeReleasedBeforeMidpointAndModeBStrict = 1;
			}

			//if squeeze is released past midpoint of current element (Mode B Strict 50% rule)
			if((!dit || !dah) && squeezeStartedBeforeCurrentElementStartedPlaying && !squeezeReleasedBeforeMidpointAndModeBStrict && midelementmodeB && kl->timeout.midl <= 0 && !afterflagdebug)
			{
				fprintf(stdout, "KEYERSTATE_DAH(%f):  dahmem = 1 (squeeze released AFTER midpoint of element) \n", kl->timeout.beep), fflush(stdout);
				dahmem = 1;
				afterflagdebug = 1;
			}
			//if dah is tapped while dit element is playing
			else if(dah && !dit && !dahmem)
			{
				fprintf(stdout, "KEYERSTATE_DAH(%f):  dahmem = 1 (dah paddle tapped while dit element was playing and dit paddle not pressed) \n", kl->timeout.beep), fflush(stdout);
				dahmem = 1;
			}
			//if dah is tapped while dit is playing and dit paddle is being held down
			//this would have triggered a "SQEEZE" but since this SQEEZE is happenning after the 
			//current element has started playing, you don't care about the 50% rule,
			//you WANT to play the dah since the paddle was PRESSED (and maybe released along
			//with the other paddle, but that doesn't matter) while dit was playing
			else if(dah && squeezeStartedAfterCurrentElementStartedPlaying && !dahmem)
			{
				fprintf(stdout, "KEYERSTATE_DAH(%f):  ditmem = 1 (dah paddle tapped while dit element playing and while dit paddle held down) \n", kl->timeout.beep), fflush(stdout);
				dahmem = 1;
			}

			//is beep timer done?
			if(kl->timeout.beep <= 0)	
			{
				kl->element.curr = NO_ELEMENT;
				kl->timeout.elem = ditlen;
				keyerState = KEYERSTATE_TIMEOUT_DIT;	//beep is finished, go to element timeout
				//last_element = DAH;
			}
		
			break;

		case(KEYERSTATE_DAH):	
			//fprintf(stdout, "KEYERSTATE_DAH\n"), fflush(stdout);

			if(dit && dah)
				squeezeStartedAfterCurrentElementStartedPlaying = 1;

			//decrement beep timer
			kl->timeout.beep = kl->timeout.beep - ticklen;
			kl->timeout.midl = kl->timeout.midl - ticklen;

			//if squeeze is released before midpoint of current element (Mode B Strict 50% rule) DO NOT PLAY
			if((!dit || !dah) && squeezeStartedBeforeCurrentElementStartedPlaying && midelementmodeB && !(kl->timeout.midl <= 0) && ditmem)
			{
				fprintf(stdout, "KEYERSTATE_DAH(middle = %f):  ditmem = 0 (squeeze released BEFORE midpoint of element) \n", kl->timeout.midl), fflush(stdout);
				ditmem = 0;
				squeezeReleasedBeforeMidpointAndModeBStrict = 1;
			}

			//if squeeze is released past midpoint of current element (Mode B Strict 50% rule)
			if((!dit || !dah) && squeezeStartedBeforeCurrentElementStartedPlaying && !squeezeReleasedBeforeMidpointAndModeBStrict && midelementmodeB && kl->timeout.midl <= 0 && !afterflagdebug)
			{
				fprintf(stdout, "KEYERSTATE_DAH(%f):  ditmem = 1 (squeeze released AFTER midpoint of element) \n", kl->timeout.beep), fflush(stdout);
				ditmem = 1;
				afterflagdebug = 1;
			}
			//if dit is tapped while dah element is playing
			else if(dit && !dah && !ditmem)
			{
				fprintf(stdout, "KEYERSTATE_DAH(%f):  ditmem = 1 (dit paddle tapped while dah element playing and dah paddle not pressed) \n", kl->timeout.beep), fflush(stdout);
				ditmem = 1;
			}
			//if dit is tapped while dah is playing and dah paddle is being held down
			//this would have triggered a "SQEEZE" but since this SQEEZE is happenning after the 
			//current element has started playing, you don't care about the 50% rule
			//you WANT to play the dit since the paddle was PRESSED (and maybe released along
			//with the other paddle, but that doesn't matter) while dah was playing
			else if(dit && squeezeStartedAfterCurrentElementStartedPlaying && !ditmem)
			{
				fprintf(stdout, "KEYERSTATE_DAH(%f):  ditmem = 1 (dit paddle tapped while dah element playing and while dah paddle held down) \n", kl->timeout.beep), fflush(stdout);
				ditmem = 1;
			}

			//is beep timer done?
			if(kl->timeout.beep <= 0)
			{
				kl->element.curr = NO_ELEMENT;
				kl->timeout.elem = ditlen;				//set the length of the timeout space
				keyerState = KEYERSTATE_TIMEOUT_DAH;	//beep is finished, go to element timeout
				//last_element = DAH;
			}
			break;

		case(KEYERSTATE_TIMEOUT_DAH):
			//fprintf(stdout, "KEYERSTATE_TIMEOUT_DAH\n\n"), fflush(stdout);
			last_element = DAH;
			kl->timeout.elem = kl->timeout.elem - ticklen;
			

			squeezeStartedAfterCurrentElementStartedPlaying = 0;
			squeezeReleasedBeforeMidpointAndModeBStrict = 0;
			afterflagdebug = 0;

			if(dit && !ditmem)
			{
				fprintf(stdout, "KEYERSTATE_TIMEOUT_DAH(%f):  ditmem = 1 (dit pressed during dah element space) \n", kl->timeout.beep), fflush(stdout);
				ditmem = 1;
			}

			if(dit && dah)
				squeezeStartedBeforeCurrentElementStartedPlaying = 1;
			else
				squeezeStartedBeforeCurrentElementStartedPlaying = 0;

			//is end of element timeout finished?
			if(kl->timeout.elem <= 0)
			{
				//squeeze case is handled
				if(dit || ditmem)	//todo: add 'both released after mid' and both squeezed
				{
					kl->element.curr = DIT;					
					kl->timeout.beep = (ditlen * (REAL) weight) / 50;
					kl->timeout.midl = kl->timeout.beep / 2;	//todo: add audio latency to this
					keyerState = KEYERSTATE_DIT;
					ditmem = 0;
				}
				else if(dah)
				{
					kl->element.curr = DAH;
					kl->timeout.beep = (ditlen * (REAL) weight) / 50 + ditlen * 2;
					kl->timeout.midl = kl->timeout.beep / 2;	//todo: add audio latency to this
					keyerState = KEYERSTATE_DAH;
				}
				else if(autocharspacing)
				{
					//Setting character space delay to ditlen*2 instead of ditlen*3 because
					//one ditlen is accounted for in the element space
					kl->timeout.dlay = ditlen * 2;
					keyerState = KEYERSTATE_TIMEOUT_CHARACTER_SPACE;
				}
				else
					keyerState = KEYERSTATE_IDLE;
			}
			break;

		case(KEYERSTATE_TIMEOUT_DIT):
			//fprintf(stdout, "KEYERSTATE_TIMEOUT_DIT\n\n"), fflush(stdout);
			last_element = DIT;
			kl->timeout.elem = kl->timeout.elem - ticklen;

			squeezeStartedAfterCurrentElementStartedPlaying = 0;
			squeezeReleasedBeforeMidpointAndModeBStrict = 0;
			afterflagdebug = 0;

			if(dah && !dahmem)
			{
				fprintf(stdout, "KEYERSTATE_TIMEOUT_DIT(%f):  dahmem = 1 (dah pressed while in dit element space) \n", kl->timeout.beep), fflush(stdout);
				dahmem = 1;
			}

			if(dit && dah)
				squeezeStartedBeforeCurrentElementStartedPlaying = 1;
			else
				squeezeStartedBeforeCurrentElementStartedPlaying = 0;

			//is end of element timeout finished?
			if(kl->timeout.elem <= 0)
			{
				if(dah || dahmem)	//todo: add 'both released after mid' and both squeezed(already handled?)
				{
					kl->element.curr = DAH;
					keyerState = KEYERSTATE_DAH;
					kl->timeout.beep = (ditlen * (REAL) weight) / 50 + ditlen * 2;
					kl->timeout.midl = kl->timeout.beep / 2;	//todo: add audio latency to this
					dahmem = 0;
				}
				else if (dit)
				{
					kl->element.curr = DIT;
					kl->timeout.beep = (ditlen * (REAL) weight) / 50;
					kl->timeout.midl = kl->timeout.beep / 2;	//todo: add audio latency to this
					keyerState = KEYERSTATE_DIT;
				}
				else if(autocharspacing)
				{
					//Setting character space delay to ditlen*2 instead of ditlen*3 because
					//one ditlen is accounted for in the element space
					kl->timeout.dlay = ditlen * 2;
					keyerState = KEYERSTATE_TIMEOUT_CHARACTER_SPACE;
				}
				else
					keyerState = KEYERSTATE_IDLE;
			}
			break;

		case (KEYERSTATE_TIMEOUT_CHARACTER_SPACE):

			//decrement character space timeout
			kl->timeout.dlay = kl->timeout.dlay - ticklen;

			//during a character space, remember only the FIRST dit or dah that is pressed
			if(dit && !ditmem && !dahmem)
			{
				fprintf(stdout, "KEYERSTATE_TIMEOUT_CHARACTER_SPACE(Char space: %f):  ditmem = 1 (dit pressed during auto character space) \n", kl->timeout.dlay), fflush(stdout);
				ditmem = 1;
			}
			else if(dah && !dahmem && !ditmem)
			{
				fprintf(stdout, "KEYERSTATE_TIMEOUT_CHARACTER_SPACE(Char space: %f):  dahmem = 1 (dah pressed during auto character space) \n", kl->timeout.dlay), fflush(stdout);
				dahmem = 1;
			}

			//when character space timout is finished go to IDLE unless dit or dah was pressed during the space
			if(kl->timeout.dlay <=0)
			{
				if(ditmem)
				{
					ditmem = 0;
					kl->element.curr = DIT;
					kl->timeout.beep = (ditlen * (REAL) weight) / 50;
					kl->timeout.midl = kl->timeout.beep / 2;	//todo: add audio latency to this
					keyerState = KEYERSTATE_DIT;
				}
				else if(dahmem)
				{
					dahmem = 0;
					kl->element.curr = DAH;
					kl->timeout.beep = (ditlen * (REAL) weight) / 50 + ditlen * 2;
					kl->timeout.midl = kl->timeout.beep / 2;	//todo: add audio latency to this
					keyerState = KEYERSTATE_DAH;
				}
				else
				keyerState = KEYERSTATE_IDLE;
			}
			break;
	}
	return (kl->timeout.beep > 0) && (kl->timeout.dlay <= 0);
}

KeyerState
newKeyerState (void)
{
	return (KeyerState) safealloc (1, sizeof (KeyerStateInfo), "newKeyerState");
}

void
delKeyerState (KeyerState ks)
{
	safefree ((char *) ks);
}

KeyerLogic
newKeyerLogic (void)
{
	return (KeyerLogic) safealloc (1, sizeof (KeyerLogicInfo), "newKeyerLogic");
}

void
delKeyerLogic (KeyerLogic kl)
{
	safefree ((char *) kl);
}

//========================================================================
