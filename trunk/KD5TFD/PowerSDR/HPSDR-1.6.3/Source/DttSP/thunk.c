/* thunk.c */

#include <common.h>
#include <thunk.h>

PRIVATE BOOLEAN
streq (char *p, char *q)
{
	return !strcmp (p, q);
}

/* somewhere along the line
   we'll kick this up a notch
   with gperf */

Thunk
Thunk_lookup (CTB ctb, char *key)
{
	if (key && *key)
	{
		for (;;)
		{
			if (!ctb || !ctb->key || !ctb->thk)
				break;
			if (streq (key, ctb->key))
				return ctb->thk;
			ctb++;
		}
    }
	return (Thunk) 0;
}

#ifdef notdef
unsigned long
hash (unsigned char *str)
{
	unsigned long hash = 5381;
	int c;
	while (c = *str++)
		hash = ((hash << 5) + hash) + c;	// (hash * 33 + c) better
	return hash;
}
#endif
