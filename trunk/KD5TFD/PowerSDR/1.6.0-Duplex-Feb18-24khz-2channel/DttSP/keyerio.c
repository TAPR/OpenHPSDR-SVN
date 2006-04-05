#include <keyer.h>

//========================================================================

/* Read a straight key connected to a serial port, do debouncing, then
   return the key state */

BOOLEAN
read_straight_key (KeyerState ks, BOOLEAN keyed)
{
  int i, j;
  static BOOLEAN keystate = 0;
  static int debounce_buf_i = 0, debounce_buf[DEBOUNCE_BUF_MAX_SIZE];
  debounce_buf[debounce_buf_i] = keyed;
  debounce_buf_i++;

  //
  //***************************************************
  // back to business as usual
  //***************************************************

  /* If the debounce buffer is full, determine the state of the key */
  if (debounce_buf_i >= ks->debounce)
    {
      debounce_buf_i = 0;

      j = 0;
      for (i = 0; i < ks->debounce; i++)
	if (debounce_buf[i])
	  j++;
      keystate = (j > ks->debounce / 2) ? 1 : 0;
    }

  return keystate;
}

//------------------------------------------------------------------------

/* Read an iambic key connected to a serial port, do debouncing, emulate a
   straight key, then return the emulated key state */

BOOLEAN
read_iambic_key (KeyerState ks, BOOLEAN dash, BOOLEAN dot, KeyerLogic kl,
		 REAL ticklen)
{
  int i, j;
  static BOOLEAN dah_debounce_buf[DEBOUNCE_BUF_MAX_SIZE],
    dit_debounce_buf[DEBOUNCE_BUF_MAX_SIZE];
  static int dah = 0, debounce_buf_i = 0, dit = 0;

  if (ks->flag.revpdl)
    {
      dah_debounce_buf[debounce_buf_i] = dot;
      dit_debounce_buf[debounce_buf_i] = dash;
    }
  else
    {
      dah_debounce_buf[debounce_buf_i] = dash;
      dit_debounce_buf[debounce_buf_i] = dot;
    }
  debounce_buf_i++;

  //
  //***************************************************
  // back to business as usual
  //***************************************************

  /* If the debounce buffer is full, determine the state of the keys */
  if (debounce_buf_i >= ks->debounce)
    {
      debounce_buf_i = 0;

      j = 0;
      for (i = 0; i < ks->debounce; i++)
	if (dah_debounce_buf[i])
	  j++;
      dah = (j > ks->debounce / 2) ? 1 : 0;

      j = 0;
      for (i = 0; i < ks->debounce; i++)
	if (dit_debounce_buf[i])
	  j++;
      dit = (j > ks->debounce / 2) ? 1 : 0;
    }

  return klogic (kl,
		 dit,
		 dah,
		 ks->wpm,
		 ks->mode,
		 ks->flag.mdlmdB,
		 ks->flag.memory.dit,
		 ks->flag.memory.dah,
		 ks->flag.autospace.khar,
		 ks->flag.autospace.word, ks->weight, ticklen);
}

//========================================================================
