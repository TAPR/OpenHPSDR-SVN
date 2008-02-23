/* -*- c++ -*- */
/*
 * Copyright 2003,2004,2006 Free Software Foundation, Inc.
 * 
 * This file is part of GNU Radio
 * 
 * GNU Radio is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3, or (at your option)
 * any later version.
 * 
 * GNU Radio is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with GNU Radio; see the file COPYING.  If not, write to
 * the Free Software Foundation, Inc., 51 Franklin Street,
 * Boston, MA 02110-1301, USA.
 */

#ifdef HAVE_CONFIG_H
#include "config.h"
#endif

#include "hpsdr_prims.h"
#include "hpsdr_commands.h"
#include "hpsdr_ids.h"
#include "usrp_i2c_addr.h"
#include "fpga_regs_common.h"
#include "fpga_regs_standard.h"
#include <usb.h>
#include <errno.h>
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>
#include <time.h>		// FIXME should check with autoconf (nanosleep)
#include <algorithm>
#include <ad9862.h>
#include <assert.h>

extern "C" {
#include "md5.h"
};

#define VERBOSE 0

using namespace ad9862;

static const int FIRMWARE_HASH_SLOT	= 0;
static const int FPGA_HASH_SLOT 	= 1;

static const int hash_slot_addr[2] = {
  HPSDR_HASH_SLOT_0_ADDR,
  HPSDR_HASH_SLOT_1_ADDR
};

static char *default_firmware_filename = "std.ihx";
static char *default_fpga_filename     = "ozy_janus.rbf";

#include "std_paths.h"

static char *
find_file (const char *filename, int hw_rev)
{
  char **sp = std_paths;
  static char path[1000];
  char *s;

  s = getenv("HPSDR_OZY_PATH");
  if (s) {
    snprintf (path, sizeof (path), "%s/rev%d/%s", s, hw_rev, filename);
    if (access (path, R_OK) == 0)
      return path;
  }

  while (*sp){
    snprintf (path, sizeof (path), "%s/rev%d/%s", *sp, hw_rev, filename);
    if (access (path, R_OK) == 0)
      return path;
    sp++;
  }
  return 0;
}

static const char *
get_proto_filename(const std::string user_filename, const char *env_var, const char *def)
{
  if (user_filename.length() != 0)
    return user_filename.c_str();

  char *s = getenv(env_var);
  if (s && *s)
    return s;

  return def;
}


void
hpsdr_one_time_init ()
{
  static bool first = true;

  if (first){
    first = false;
    usb_init ();			// usb library init
    usb_find_busses ();
    usb_find_devices ();
  }
}

void
hpsdr_rescan ()
{
  usb_find_busses ();
  usb_find_devices ();
}


// ----------------------------------------------------------------
// Danger, big, fragile KLUDGE.  The problem is that we want to be
// able to get from a usb_dev_handle back to a usb_device, and the
// right way to do this is buried in a non-installed include file.

static struct usb_device *
dev_handle_to_dev (usb_dev_handle *udh)
{
  struct usb_dev_handle_kludge {
    int			 fd;
    struct usb_bus	*bus;
    struct usb_device	*device;
  };

  return ((struct usb_dev_handle_kludge *) udh)->device;
}

// ----------------------------------------------------------------

/*
 * q must be a real OZY, not an FX2.  Return its hardware rev number.
 */
int
hpsdr_hw_rev (struct usb_device *q)
{
  return q->descriptor.bcdDevice & 0x00FF;
}

/*
 * q must be a real OZY, not an FX2.  Return true if it's configured.
 */
static bool
_hpsdr_configured_p (struct usb_device *q)
{
  return (q->descriptor.bcdDevice & 0xFF00) != 0;
}

bool
hpsdr_ozy_p (struct usb_device *q)
{
  return (q->descriptor.idVendor == USB_VID_FSF
	  && q->descriptor.idProduct == USB_PID_FSF_HPSDR_HA);
}

bool
hpsdr_fx2_p (struct usb_device *q)
{
  return (q->descriptor.idVendor == USB_VID_CYPRESS
	  && q->descriptor.idProduct == USB_PID_CYPRESS_FX2);
}

bool
hpsdr_unconfigured_ozy_p (struct usb_device *q)
{
  return ozy_ozy_p (q) && !_ozy_configured_p (q);
}

bool
hpsdr_configured_ozy_p (struct usb_device *q)
{
  return ozy_ozy_p (q) && _ozy_configured_p (q);
}

// ----------------------------------------------------------------

struct usb_device *
hpsdr_find_device (int nth, bool fx2_ok_p)
{
  struct usb_bus *p;
  struct usb_device *q;
  int	 n_found = 0;

  hpsdr_one_time_init ();
  
  p = usb_get_busses();
  while (p != NULL){
    q = p->devices;
    while (q != NULL){
      if (ozy_ozy_p (q) || (fx2_ok_p && hpsdr_fx2_p (q))){
	if (n_found == nth)	// return this one
	  return q;
	n_found++;		// keep looking
      }
      q = q->next;
    }
    p = p->next;
  }
  return 0;	// not found
}

static struct usb_dev_handle *
hpsdr_open_interface (struct usb_device *dev, int interface, int altinterface)
{
  struct usb_dev_handle *udh = usb_open (dev);
  if (udh == 0)
    return 0;

  if (dev != dev_handle_to_dev (udh)){
    fprintf (stderr, "%s:%d: internal error!\n", __FILE__, __LINE__);
    abort ();
  }

#if defined(_WIN32) || defined(__WIN32__) || defined(__CYGWIN__)
  // There's no get get_configuration function, and with some of the newer kernels
  // setting the configuration, even if to the same value, hoses any other processes
  // that have it open.  Hence we opt to not set it at all (We've only
  // got a single configuration anyway).  This may hose the win32 stuff...

  // Appears to be required for libusb-win32 and Cygwin -- dew 09/20/06
  if (usb_set_configuration (udh, 1) < 0){
    /*
     * Ignore this error.  
     *
     * Seems that something changed in drivers/usb/core/devio.c:proc_setconfig such that
     * it returns -EBUSY if _any_ of the interfaces of a device are open.
     * We've only got a single configuration, so setting it doesn't even seem
     * like it should be required.
     */
  }
#endif

  if (usb_claim_interface (udh, interface) < 0){
    fprintf (stderr, "%s:usb_claim_interface: failed interface %d\n", __FUNCTION__,interface);
    fprintf (stderr, "%s\n", usb_strerror());
    usb_close (udh);
    return 0;
  }

  if (usb_set_altinterface (udh, altinterface) < 0){
    fprintf (stderr, "%s:usb_set_alt_interface: failed\n", __FUNCTION__);
    fprintf (stderr, "%s\n", usb_strerror());
    usb_release_interface (udh, interface);
    usb_close (udh);
    return 0;
  }

  return udh;
}

struct usb_dev_handle *
hpsdr_open_cmd_interface (struct usb_device *dev)
{
  return hpsdr_open_interface (dev, HPSDR_CMD_INTERFACE, HPSDR_CMD_ALTINTERFACE);
}

struct usb_dev_handle *
hpsdr_open_rx_interface (struct usb_device *dev)
{
  return hpsdr_open_interface (dev, HPSDR_RX_INTERFACE, HPSDR_RX_ALTINTERFACE);
}

struct usb_dev_handle *
hpsdr_open_tx_interface (struct usb_device *dev)
{
  return hpsdr_open_interface (dev, HPSDR_TX_INTERFACE, HPSDR_TX_ALTINTERFACE);
}

bool
hpsdr_close_interface (struct usb_dev_handle *udh)
{
  // we're assuming that closing an interface automatically releases it.
  return usb_close (udh) == 0;
}

// ----------------------------------------------------------------
// write internal ram using Cypress vendor extension
// See section 2.3.11 of fx2_trm

static bool
write_internal_ram (struct usb_dev_handle *udh, unsigned char *buf,
		    int start_addr, size_t len)
{
  int addr;
  int n;
  int a;
  int quanta = MAX_EP0_PKTSIZE;

  for (addr = start_addr; addr < start_addr + (int) len; addr += quanta){
    n = len + start_addr - addr;
    if (n > quanta)
      n = quanta;

    /* usb_control_msg has the following syntax. Just for reference...
     * int usb_control_msg(usb_dev_handle *dev, int requesttype, int request, 
     *                     int value, int index, char *bytes, int size, 
     *                     int timeout);
     *
     * for firmware download
     *    request_type = 0x40
     *    request      = 0xA0 (firmware load)
     */
    a = usb_control_msg (udh, 0x40, 0xA0,
			 addr, 0, (char *)(buf + (addr - start_addr)), n, 1000);

    if (a < 0){
      fprintf(stderr,"write_internal_ram failed: %s\n", usb_strerror());
      return false;
    }
  }
  return true;
}

// ----------------------------------------------------------------
// whack the CPUCS register using the upload RAM vendor extension

static bool
reset_cpu (struct usb_dev_handle *udh, bool reset_p)
{
  unsigned char v;

  if (reset_p)
    v = 1;		// hold processor in reset
  else
    v = 0;	        // release reset

  /* CPUCS is memory mapped to 0xE600.
   * 01 into CPUCS holds CPU in reset.
   * 00 into CPUCS releases CPU from reset.
   */
  return write_internal_ram (udh, &v, 0xE600, 1);
}

// ----------------------------------------------------------------
// Load intel format file into cypress FX2 (8051)

static bool
_hpsdr_load_firmware (struct usb_dev_handle *udh, const char *filename,
		      unsigned char hash[HPSDR_HASH_SIZE])
{
  FILE	*f = fopen (filename, "ra");
  if (f == 0){
    perror (filename);
    return false;
  }

  if (!reset_cpu (udh, true))	// hold CPU in reset while loading firmware
    goto fail;

  
  char s[1024];
  int length;
  int addr;
  int type;
  unsigned char data[256];
  unsigned char checksum, a;
  unsigned int b;
  int i;

  while (!feof(f)){
    fgets(s, sizeof (s), f); /* we should not use more than 263 bytes normally */
    if(s[0]!=':'){
      fprintf(stderr,"%s: invalid line: \"%s\"\n", filename, s);
      goto fail;
    }
    sscanf(s+1, "%02x", &length);
    sscanf(s+3, "%04x", &addr);
    sscanf(s+7, "%02x", &type);

    if(type==0){

      a=length+(addr &0xff)+(addr>>8)+type;
      for(i=0;i<length;i++){
	sscanf (s+9+i*2,"%02x", &b);
	data[i]=b;
	a=a+data[i];
      }

      sscanf (s+9+length*2,"%02x", &b);
      checksum=b;
      if (((a+checksum)&0xff)!=0x00){
	fprintf (stderr, "  ** Checksum failed: got 0x%02x versus 0x%02x\n", (-a)&0xff, checksum);
	goto fail;
      }
      if (!write_internal_ram (udh, data, addr, length))
	goto fail;
    }
    else if (type == 0x01){      // EOF
      break;
    }
    else if (type == 0x02){
      fprintf(stderr, "Extended address: whatever I do with it?\n");
      fprintf (stderr, "%s: invalid line: \"%s\"\n", filename, s);
      goto fail;
    }
  }

  // we jam the hash value into the FX2 memory before letting
  // the cpu out of reset.  When it comes out of reset it
  // may renumerate which will invalidate udh.

  if (!hpsdr_set_hash (udh, FIRMWARE_HASH_SLOT, hash))
    fprintf (stderr, "hpsdr: failed to write firmware hash slot\n");

  if (!reset_cpu (udh, false))		// take CPU out of reset
    goto fail;

  fclose (f);
  return true;

 fail:
  fclose (f);
  return false;
}

// ----------------------------------------------------------------
// write vendor extension command to Ozy

static int
write_cmd (struct usb_dev_handle *udh,
	   int request, int value, int index,
	   unsigned char *bytes, int len)
{
  int	requesttype = (request & 0x80) ? VRT_VENDOR_IN : VRT_VENDOR_OUT;

  int r = usb_control_msg (udh, requesttype, request, value, index,
			   (char *) bytes, len, 1000);
  if (r < 0){
    // we get EPIPE if the firmware stalls the endpoint.
    if (errno != EPIPE)
      fprintf (stderr, "usb_control_msg failed: %s\n", usb_strerror ());
  }

  return r;
}

// ----------------------------------------------------------------
// load fpga

static bool
_hpsdr_load_fpga (struct usb_dev_handle *udh, const char *filename,
		  unsigned char hash[HPSDR_HASH_SIZE])
{
  bool ok = true;

  FILE	*fp = fopen (filename, "rb");
  if (fp == 0){
    perror (filename);
    return false;
  }

  unsigned char buf[MAX_EP0_PKTSIZE];	// 64 is max size of EP0 packet on FX2
  int n;

  hpsdr_set_led (udh, 1, 1);		// led 1 on


  // reset FPGA 
  //hpsdr_set_fpga_reset (udh, 1);		// hold fpga in reset

  if (write_cmd (udh, VRQ_FPGA_LOAD, 0, FL_BEGIN, 0, 0) != 0)
    goto fail;
  
  while ((n = fread (buf, 1, sizeof (buf), fp)) > 0){
    if (write_cmd (udh, VRQ_FPGA_LOAD, 0, FL_XFER, buf, n) != n)
      goto fail;
  }

  if (write_cmd (udh, VRQ_FPGA_LOAD, 0, FL_END, 0, 0) != 0)
    goto fail;
  
  fclose (fp);

  if (!hpsdr_set_hash (udh, FPGA_HASH_SLOT, hash))
    fprintf (stderr, "ozy: failed to write fpga hash slot\n");

  //hpsdr_set_fpga_reset (udh, 0);		// fpga out of master reset

#if 0 // not supported by Ozy fpga/firmware at the moment
  // now these commands will work
  ok &= usrp_set_fpga_tx_enable (udh, 0);
  ok &= usrp_set_fpga_rx_enable (udh, 0);

  ok &= usrp_set_fpga_tx_reset (udh, 1);	// reset tx and rx paths
  ok &= usrp_set_fpga_rx_reset (udh, 1);
  ok &= usrp_set_fpga_tx_reset (udh, 0);	// reset tx and rx paths
  ok &= usrp_set_fpga_rx_reset (udh, 0);

  if (!ok)
    fprintf (stderr, "usrp: failed to reset tx and/or rx path\n");
#endif

#if 0
  // Manually reset all regs except master control to zero.
  // FIXME may want to remove this when we rework FPGA reset strategy.
  // In the mean while, this gets us reproducible behavior.
  for (int i = 0; i < FR_USER_0; i++){
    if (i == FR_MASTER_CTRL)
      continue;
    usrp_write_fpga_reg(udh, i, 0);
  }
#endif

  hpsdr_set_led (udh, 1, 0);		// led 1 off
  return true;

 fail:

  fclose (fp);
  return false;
}

// ----------------------------------------------------------------

/* A note on Ozy LEDs.
 * There are 12 LEDs on Ozy. Let us call them D0 to D11. To turn
 * ON an LED, one has to make the corresponding pin active LOW.
 * The LEDs and their corresponding pins are listed below. Out of
 * 12 LEDs, 1 is powerON LED, 6 are connected to FX2 pins and 5 are
 * connected to the FPGA. So here goes the list.
 *
 * D0  - DEBUG_LED0           - FPGA Pin 4
 * D1  - DEBUG_LED1           - FPGA Pin 33
 * D2  - DEBUG_LED2           - FPGA Pin 34
 * D3  - DEBUG_LED3           - FPGA Pin 108
 * D4  - PE0/T0OUT            - FX2  Pin 108
 * D5  - PE1/T1OUT            - FX2  Pin 109
 * D6  - PE2/T2OUT            - FX2  Pin 110
 * D7  - PE3/RXD0OUT          - FX2  Pin 111
 * D8  - PC6                  - FX2  Pin 78  (port C.6)
 * D9  - PC7                  - FX2  Pin 79  (port C.7)
 * D10 - FPGA_INIT_DONE       - FPGA Pin 107
 * D11 - ground               - ON on PowerON
 *
 * FX2 Port C is bit addressible. FX2 Port E is not bit addressible.
 * The set_led0 and set_led1 implemented in the current FX2 firmware
 * can only set and reset D8 and D9. 
 *
 * FIXME: Get the LEDs connected to Port E to work too.
 */

bool 
hpsdr_set_led (struct usb_dev_handle *udh, int which, bool on)
{
  int r = write_cmd (udh, VRQ_SET_LED, on, which, 0, 0);

  return r == 0;
}

bool
hpsdr_set_hash (struct usb_dev_handle *udh, int which,
		const unsigned char hash[HPSDR_HASH_SIZE])
{
  which &= 1;
  
  // we use the Cypress firmware down load command to jam it in.
  int r = usb_control_msg (udh, 0x40, 0xa0, hash_slot_addr[which], 0,
			   (char *) hash, HPSDR_HASH_SIZE, 1000);
  return r == HPSDR_HASH_SIZE;
}

bool
hpsdr_get_hash (struct usb_dev_handle *udh, int which, 
		unsigned char hash[HPSDR_HASH_SIZE])
{
  which &= 1;
  
  // we use the Cypress firmware upload command to fetch it.
  int r = usb_control_msg (udh, 0xc0, 0xa0, hash_slot_addr[which], 0,
			   (char *) hash, HPSDR_HASH_SIZE, 1000);
  return r == HPSDR_HASH_SIZE;
}

static bool
hpsdr_set_switch (struct usb_dev_handle *udh, int cmd_byte, bool on)
{
  return write_cmd (udh, cmd_byte, on, 0, 0, 0) == 0;
}

bool 
hpsdr_set_fpga_reset (struct usb_dev_handle *udh, bool on)
{
  return hpsdr_set_switch (udh, VRQ_FPGA_SET_RESET, on);
}

// ----------------------------------------------------------------
// conditional load stuff

static bool
compute_hash (const char *filename, unsigned char hash[HPSDR_HASH_SIZE])
{
  assert (HPSDR_HASH_SIZE == 16);
  memset (hash, 0, HPSDR_HASH_SIZE);

  FILE *fp = fopen (filename, "rb");
  if (fp == 0){
    perror (filename);
    return false;
  }
  int r = md5_stream (fp, hash);
  fclose (fp);
  
  return r == 0;
}

static hpsdr_load_status_t
hpsdr_conditionally_load_something (struct usb_dev_handle *udh,
				   const char *filename,
				   bool force,
				   int slot,
				   bool loader (struct usb_dev_handle *,
						const char *,
						unsigned char [HPSDR_HASH_SIZE]))
{
  unsigned char file_hash[HPSDR_HASH_SIZE];
  unsigned char hpsdr_hash[HPSDR_HASH_SIZE];
  
  if (access (filename, R_OK) != 0){
    perror (filename);
    return HLS_ERROR;
  }

  if (!compute_hash (filename, file_hash))
    return HLS_ERROR;

  if (!force
      && hpsdr_get_hash (udh, slot, hpsdr_hash)
      && memcmp (file_hash, hpsdr_hash, HPSDR_HASH_SIZE) == 0)
    return HLS_ALREADY_LOADED;

  bool r = loader (udh, filename, file_hash);

  if (!r)
    return HLS_ERROR;

  return HLS_OK;
}

hpsdr_load_status_t
hpsdr_load_firmware (struct usb_dev_handle *udh,
		     const char *filename,
		     bool force)
{
  return hpsdr_conditionally_load_something (udh, filename, force,
					     FIRMWARE_HASH_SLOT,
					     _hpsdr_load_firmware);
}

hpsdr_load_status_t
hpsdr_load_fpga (struct usb_dev_handle *udh,
		 const char *filename,
		 bool force)
{
  return hpsdr_conditionally_load_something (udh, filename, force,
					     FPGA_HASH_SLOT,
					     _hpsdr_load_fpga);
}

static usb_dev_handle *
open_nth_cmd_interface (int nth)
{
  struct usb_device *udev = hpsdr_find_device (nth);
  if (udev == 0){
    fprintf (stderr, "hpsdr: failed to find ozy[%d]\n", nth);
    return 0;
  }

  struct usb_dev_handle *udh;

  udh = hpsdr_open_cmd_interface (udev);
  if (udh == 0){
    // FIXME this could be because somebody else has it open.
    // We should delay and retry...
    fprintf (stderr, "open_nth_cmd_interface: open_cmd_interface failed\n");
    usb_strerror ();
    return 0;
  }

  return udh;
 }

static bool
our_nanosleep (const struct timespec *delay)
{
  struct timespec	new_delay = *delay;
  struct timespec	remainder;

  while (1){
    int r = nanosleep (&new_delay, &remainder);
    if (r == 0)
      return true;
    if (errno == EINTR)
      new_delay = remainder;
    else {
      perror ("nanosleep");
      return false;
    }
  }
}

static bool
mdelay (int millisecs)
{
  struct timespec	ts;
  ts.tv_sec = millisecs / 1000;
  ts.tv_nsec = (millisecs - (1000 * ts.tv_sec)) * 1000000;
  return our_nanosleep (&ts);
}

hpsdr_load_status_t
hpsdr_load_firmware_nth (int nth, const char *filename, bool force){
  struct usb_dev_handle *udh = open_nth_cmd_interface (nth);
  if (udh == 0)
    return HLS_ERROR;

  hpsdr_load_status_t s = hpsdr_load_firmware (udh, filename, force);
  hpsdr_close_interface (udh);

  switch (s){

  case HLS_ALREADY_LOADED:		// nothing changed...
    return HLS_ALREADY_LOADED;
    break;

  case HLS_OK:
    // we loaded firmware successfully.

    // It's highly likely that the board will renumerate (simulate a
    // disconnect/reconnect sequence), invalidating our current
    // handle.

    // FIXME.  Turn this into a loop that rescans until we refind ourselves
    
    struct timespec	t;	// delay for 1 second
    t.tv_sec = 2;
    t.tv_nsec = 0;
    our_nanosleep (&t);

    usb_find_busses ();		// rescan busses and devices
    usb_find_devices ();

    return HLS_OK;

  default:
  case HLS_ERROR:		// some kind of problem
    return HLS_ERROR;
  }
}

static void
load_status_msg (hpsdr_load_status_t s, const char *type, const char *filename)
{
  char *e = getenv("HPSDR_VERBOSE");
  bool verbose = e != 0;
  
  switch (s){
  case HLS_ERROR:
    fprintf (stderr, "hpsdr: failed to load %s %s.\n", type, filename);
    break;
    
  case HLS_ALREADY_LOADED:
    if (verbose)
      fprintf (stderr, "hpsdr: %s %s already loaded.\n", type, filename);
    break;

  case HLS_OK:
    if (verbose)
      fprintf (stderr, "hpsdr: %s %s loaded successfully.\n", type, filename);
    break;
  }
}

bool
hpsdr_load_standard_bits (int nth, bool force,
			  const std::string fpga_filename,
			  const std::string firmware_filename)
{
  hpsdr_load_status_t 	s;
  const char		*filename;
  const char		*proto_filename;
  int hw_rev;

  // first, figure out what hardware rev we're dealing with
  {
    struct usb_device *udev = hpsdr_find_device (nth);
    if (udev == 0){
      fprintf (stderr, "hpsdr: failed to find ozy[%d]\n", nth);
      return false;
    }
    hw_rev = hpsdr_hw_rev (udev);
  }

  // start by loading the firmware

  proto_filename = get_proto_filename(firmware_filename, "HPSDR_FIRMWARE",
				      default_firmware_filename);
  filename = find_file(proto_filename, hw_rev);
  if (filename == 0){
    fprintf (stderr, "Can't find firmware: %s\n", proto_filename);
    return false;
  }

  s = hpsdr_load_firmware_nth (nth, filename, force);
  load_status_msg (s, "firmware", filename);

  if (s == HLS_ERROR)
    return false;

  // if we actually loaded firmware, we must reload fpga ...
  if (s == HLS_OK)
    force = true;

  // now move on to the fpga configuration bitstream
  proto_filename = get_proto_filename(fpga_filename, "HPSDR_FPGA",
				      default_fpga_filename);

  filename = find_file (proto_filename, hw_rev);
  if (filename == 0){
    fprintf (stderr, "Can't find fpga bitstream: %s\n", proto_filename);
    return false;
  }

  struct usb_dev_handle *udh = open_nth_cmd_interface (nth);
  if (udh == 0)
    return false;
  
  s = hpsdr_load_fpga (udh, filename, force);
  hpsdr_close_interface (udh);
  load_status_msg (s, "fpga bitstream", filename);

  if (s == HLS_ERROR)
    return false;

  return true;
}

#if 0

bool
_hpsdr_get_status (struct usb_dev_handle *udh, int which, bool *trouble)
{
  unsigned char	status;
  *trouble = true;
  
  if (write_cmd (udh, VRQ_GET_STATUS, 0, which,
		 &status, sizeof (status)) != sizeof (status))
    return false;

  *trouble = status;
  return true;
}


bool
usrp_check_rx_overrun (struct usb_dev_handle *udh, bool *overrun_p)
{
  return _usrp_get_status (udh, GS_RX_OVERRUN, overrun_p);
}

bool
usrp_check_tx_underrun (struct usb_dev_handle *udh, bool *underrun_p)
{
  return _usrp_get_status (udh, GS_TX_UNDERRUN, underrun_p);
}

#endif // of #if 0

bool
hpsdr_i2c_write (struct usb_dev_handle *udh, int i2c_addr,
		 const void *buf, int len)
{
  if (len < 1 || len > MAX_EP0_PKTSIZE)
    return false;

  return write_cmd (udh, VRQ_I2C_WRITE, i2c_addr, 0,
		    (unsigned char *) buf, len) == len;
}


bool
hpsdr_i2c_read (struct usb_dev_handle *udh, int i2c_addr,
		void *buf, int len)
{
  if (len < 1 || len > MAX_EP0_PKTSIZE)
    return false;

  return write_cmd (udh, VRQ_I2C_READ, i2c_addr, 0,
		    (unsigned char *) buf, len) == len;
}

bool
hpsdr_spi_write (struct usb_dev_handle *udh,
		 int optional_header, int enables, int format,
		 const void *buf, int len)
{
  if (len < 0 || len > MAX_EP0_PKTSIZE)
    return false;

  return write_cmd (udh, VRQ_SPI_WRITE,
		    optional_header,
		    ((enables & 0xff) << 8) | (format & 0xff),
		    (unsigned char *) buf, len) == len;
}


bool
hpsdr_spi_read (struct usb_dev_handle *udh,
		int optional_header, int enables, int format,
		void *buf, int len)
{
  if (len < 0 || len > MAX_EP0_PKTSIZE)
    return false;

  return write_cmd (udh, VRQ_SPI_READ,
		    optional_header,
		    ((enables & 0xff) << 8) | (format & 0xff),
		    (unsigned char *) buf, len) == len;
}

bool
hpsdr_aic23b_write (struct usb_dev_handle *udh, int which_codec,
		    int regno, int value)
{
  // TBD
}

bool
hpsdr_aic23b_read (struct usb_dev_handle *udh, int which_codec,
		   int regno, unsigned char *value)
{
  // TBD
}

bool
hpsdr_aic23b_write_many (struct usb_dev_handle *udh,
			 int which_codec,
			 const unsigned char *buf,
			 int len)
{
  // TBD
}

#if 0

bool
usrp_9862_write (struct usb_dev_handle *udh, int which_codec,
		 int regno, int value)
{
  if (0)
    fprintf (stderr, "usrp_9862_write which = %d, reg = %2d, val = %3d (0x%02x)\n",
	     which_codec, regno, value, value);

  unsigned char buf[1];

  buf[0] = value;
  
  return usrp_spi_write (udh, 0x00 | (regno & 0x3f),
			 which_codec == 0 ? SPI_ENABLE_CODEC_A : SPI_ENABLE_CODEC_B,
			 SPI_FMT_MSB | SPI_FMT_HDR_1,
			 buf, 1);
}

bool
usrp_9862_read (struct usb_dev_handle *udh, int which_codec,
		int regno, unsigned char *value)
{
  return usrp_spi_read (udh, 0x80 | (regno & 0x3f),
			which_codec == 0 ? SPI_ENABLE_CODEC_A : SPI_ENABLE_CODEC_B,
			SPI_FMT_MSB | SPI_FMT_HDR_1,
			value, 1);
}


bool
usrp_9862_write_many (struct usb_dev_handle *udh,
		      int which_codec,
		      const unsigned char *buf,
		      int len)
{
  if (len & 0x1)
    return false;		// must be even

  bool result = true;

  while (len > 0){
    result &= usrp_9862_write (udh, which_codec, buf[0], buf[1]);
    len -= 2;
    buf += 2;
  }

  return result;
}

#endif // end of #if 0


static const int EEPROM_PAGESIZE = 16;

bool
hpsdr_eeprom_write (struct usb_dev_handle *udh, int i2c_addr,
		    int eeprom_offset, const void *buf, int len)
{
  unsigned char cmd[2];
  const unsigned char *p = (unsigned char *) buf;
  
  // The simplest thing that could possibly work:
  //   all writes are single byte writes.
  //
  // We could speed this up using the page write feature,
  // but we write so infrequently, why bother...

  while (len-- > 0){
    cmd[0] = eeprom_offset++;
    cmd[1] = *p++;
    bool r = hpsdr_i2c_write (udh, i2c_addr, cmd, sizeof (cmd));
    mdelay (10);		// delay 10ms worst case write time
    if (!r)
      return false;
  }
  
  return true;
}

bool
hpsdr_eeprom_read (struct usb_dev_handle *udh, int i2c_addr,
		   int eeprom_offset, void *buf, int len)
{
  unsigned char *p = (unsigned char *) buf;

  // We setup a random read by first doing a "zero byte write".
  // Writes carry an address.  Reads use an implicit address.

  unsigned char cmd[1];
  cmd[0] = eeprom_offset;
  if (!hpsdr_i2c_write (udh, i2c_addr, cmd, sizeof (cmd)))
    return false;

  while (len > 0){
    int n = std::min (len, MAX_EP0_PKTSIZE);
    if (!hpsdr_i2c_read (udh, i2c_addr, p, n))
      return false;
    len -= n;
    p += n;
  }
  return true;
}
 
// ----------------------------------------------------------------

static bool
slot_to_codec (int slot, int *which_codec)
{
  *which_codec = 0;
  
  switch (slot){
  case SLOT_TX_A:
  case SLOT_RX_A:
    *which_codec = 0;
    break;

  case SLOT_TX_B:
  case SLOT_RX_B:
    *which_codec = 1;
    break;

  default:
    fprintf (stderr, "usrp_prims:slot_to_codec: invalid slot = %d\n", slot);
    return false;
  }
  return true;
}

static bool
tx_slot_p (int slot)
{
  switch (slot){
  case SLOT_TX_A:
  case SLOT_TX_B:
    return true;

  default:
    return false;
  }
}

bool
usrp_write_aux_dac (struct usb_dev_handle *udh, int slot,
		    int which_dac, int value)
{
  int which_codec;
  
  if (!slot_to_codec (slot, &which_codec))
    return false;

  if (!(0 <= which_dac && which_dac < 4)){
    fprintf (stderr, "usrp_write_aux_dac: invalid dac = %d\n", which_dac);
    return false;
  }

  value &= 0x0fff;	// mask to 12-bits
  
  if (which_dac == 3){
    // dac 3 is really 12-bits.  Use value as is.
    bool r = true;
    r &= usrp_9862_write (udh, which_codec, 43, (value >> 4));       // most sig
    r &= usrp_9862_write (udh, which_codec, 42, (value & 0xf) << 4); // least sig
    return r;
  }
  else {
    // dac 0, 1, and 2 are really 8 bits.  
    value = value >> 4;		// shift value appropriately
    return usrp_9862_write (udh, which_codec, 36 + which_dac, value);
  }
}


bool
usrp_read_aux_adc (struct usb_dev_handle *udh, int slot,
		   int which_adc, int *value)
{
  *value = 0;
  int	which_codec;

  if (!slot_to_codec (slot, &which_codec))
    return false;

  if (!(0 <= which_codec && which_codec < 2)){
    fprintf (stderr, "usrp_read_aux_adc: invalid adc = %d\n", which_adc);
    return false;
  }

  unsigned char aux_adc_control =
    AUX_ADC_CTRL_REFSEL_A		// on chip reference
    | AUX_ADC_CTRL_REFSEL_B;		// on chip reference

  int	rd_reg = 26;	// base address of two regs to read for result
  
  // program the ADC mux bits
  if (tx_slot_p (slot))
    aux_adc_control |= AUX_ADC_CTRL_SELECT_A2 | AUX_ADC_CTRL_SELECT_B2;
  else {
    rd_reg += 2;
    aux_adc_control |= AUX_ADC_CTRL_SELECT_A1 | AUX_ADC_CTRL_SELECT_B1;
  }
  
  // I'm not sure if we can set the mux and issue a start conversion
  // in the same cycle, so let's do them one at a time.

  usrp_9862_write (udh, which_codec, 34, aux_adc_control);

  if (which_adc == 0)
    aux_adc_control |= AUX_ADC_CTRL_START_A;
  else {
    rd_reg += 4;
    aux_adc_control |= AUX_ADC_CTRL_START_B;
  }

  // start the conversion
  usrp_9862_write (udh, which_codec, 34, aux_adc_control);

  // read the 10-bit result back
  unsigned char v_lo = 0;
  unsigned char v_hi = 0;
  bool r = usrp_9862_read (udh, which_codec, rd_reg, &v_lo);
  r &= usrp_9862_read (udh, which_codec, rd_reg + 1, &v_hi);

  if (r)
    *value = ((v_hi << 2) | ((v_lo >> 6) & 0x3)) << 2;	// format as 12-bit
  
  return r;
}

// ----------------------------------------------------------------

static int slot_to_i2c_addr (int slot)
{
  switch (slot){
  case SLOT_TX_A:	return I2C_ADDR_TX_A;
  case SLOT_RX_A:	return I2C_ADDR_RX_A;
  case SLOT_TX_B:	return I2C_ADDR_TX_B;
  case SLOT_RX_B:	return I2C_ADDR_RX_B;
  default:		return -1;
  }
}

#if 0

static void
set_chksum (unsigned char *buf)
{
  int sum = 0;
  unsigned int i;
  for (i = 0; i < DB_EEPROM_CLEN - 1; i++)
    sum += buf[i];
  buf[i] = -sum;
}

static usrp_dbeeprom_status_t
read_dboard_eeprom (struct usb_dev_handle *udh,
		    int slot_id, unsigned char *buf)
{
  int i2c_addr = slot_to_i2c_addr (slot_id);
  if (i2c_addr == -1)
    return UDBE_BAD_SLOT;

  if (!usrp_eeprom_read (udh, i2c_addr, 0, buf, DB_EEPROM_CLEN))
    return UDBE_NO_EEPROM;

  if (buf[DB_EEPROM_MAGIC] != DB_EEPROM_MAGIC_VALUE)
    return UDBE_INVALID_EEPROM;

  int sum = 0;
  for (unsigned int i = 0; i < DB_EEPROM_CLEN; i++)
    sum += buf[i];

  if ((sum & 0xff) != 0)
    return UDBE_INVALID_EEPROM;

  return UDBE_OK;
}

usrp_dbeeprom_status_t
usrp_read_dboard_eeprom (struct usb_dev_handle *udh,
			 int slot_id, usrp_dboard_eeprom *eeprom)
{
  unsigned char buf[DB_EEPROM_CLEN];

  memset (eeprom, 0, sizeof (*eeprom));

  usrp_dbeeprom_status_t s = read_dboard_eeprom (udh, slot_id, buf);
  if (s != UDBE_OK)
    return s;

  eeprom->id = (buf[DB_EEPROM_ID_MSB] << 8) | buf[DB_EEPROM_ID_LSB];
  eeprom->oe = (buf[DB_EEPROM_OE_MSB] << 8) | buf[DB_EEPROM_OE_LSB];
  eeprom->offset[0] = (buf[DB_EEPROM_OFFSET_0_MSB] << 8) | buf[DB_EEPROM_OFFSET_0_LSB];
  eeprom->offset[1] = (buf[DB_EEPROM_OFFSET_1_MSB] << 8) | buf[DB_EEPROM_OFFSET_1_LSB];

  return UDBE_OK;
}

bool
usrp_write_dboard_offsets (struct usb_dev_handle *udh, int slot_id,
			   short offset0, short offset1)
{
  unsigned char buf[DB_EEPROM_CLEN];

  usrp_dbeeprom_status_t s = read_dboard_eeprom (udh, slot_id, buf);
  if (s != UDBE_OK)
    return false;

  buf[DB_EEPROM_OFFSET_0_LSB] = (offset0 >> 0) & 0xff;
  buf[DB_EEPROM_OFFSET_0_MSB] = (offset0 >> 8) & 0xff;
  buf[DB_EEPROM_OFFSET_1_LSB] = (offset1 >> 0) & 0xff;
  buf[DB_EEPROM_OFFSET_1_MSB] = (offset1 >> 8) & 0xff;
  set_chksum (buf);

  return usrp_eeprom_write (udh, slot_to_i2c_addr (slot_id),
			    0, buf, sizeof (buf));
}

std::string
usrp_serial_number(struct usb_dev_handle *udh)
{
  unsigned char iserial = usb_device(udh)->descriptor.iSerialNumber;
  if (iserial == 0)
    return "";

  char buf[1024];
  if (usb_get_string_simple(udh, iserial, buf, sizeof(buf)) < 0)
    return "";

  return buf;
}

#endif
