This folder contains the Ozy FX2 firmware modified for configuring the FPGA
devices on the fly via JTAG. There is one new unit, jrun.c / jrun.h. A few
small changes in the existing units are marked with the //VE3NEA comment.

The jrun unit is written from scratch and is released under MPL.

One new vendor request, VRQ_JTAG_FUNC, is used for all JTAG functions.
The following functions are implemented:

JF_PORT         read/write JTAG signals
JF_LIST_DEV     list device ID's in the JTAG chain
JF_START_CFG    start FPGA configuration
JF_XFER_CFG     transfer FPGA data
JF_END_CFG      end FPGA configuration


The JF_PORT function provides direct access to the JTAG signals and can be used
to implement some other JTAG functions in the PC software.

The JF_LIST_DEV function can be used to discover the number and order of the
modules on the Atlas board.



Configuring the FPGA

JF_START_CFG starts FPGA configuration. The following example uses libusb to
communicate with FX2 from a Delphi program:

    rc := usb_control_msg(FHandle, VRT_VENDOR_OUT, VRQ_JTAG_FUNC, MercuryDeviceIdx, JF_START_CFG, Dummy, 0, 1000);
    if rc < 0 then Err('Mercury JF_START_CFG failed');

Once JF_START_CFG succeeded, call JF_XFER_CFG repeatedly and pass the contents
of the RBF file in 64-byte chunks. The RBF file must be uncompressed.

    rc := usb_control_msg(FHandle, VRT_VENDOR_OUT, VRQ_JTAG_FUNC, MercuryDeviceIdx, JF_XFER_CFG, Data[0], Length(Data), 1000);

After sending the whole file, call JF_END_CFG:

    rc := usb_control_msg(FHandle, VRT_VENDOR_OUT, VRQ_JTAG_FUNC, MercuryDeviceIdx, JF_END_CFG, Dummy, 0, 1000);

This will put the FPGA in the User mode with the new configuration.


END



73 Alex VE3NEA
