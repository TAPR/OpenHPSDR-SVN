FUNCTION altdpram
(
	wren,
	data[WIDTH-1..0],
	wraddress[WIDTHAD-1..0],
	inclock,
	inclocken,
	rden,
	rdaddress[WIDTHAD-1..0],
	outclock,
	outclocken,
	aclr
)

WITH
(
	WIDTH,
	WIDTHAD,
	NUMWORDS,
	FILE,
	INDATA_REG,
	INDATA_ACLR,
	WRADDRESS_REG,
	WRADDRESS_ACLR,
	WRCONTROL_REG,
	WRCONTROL_ACLR,
	RDADDRESS_REG,
	RDADDRESS_ACLR,
	RDCONTROL_REG,
	RDCONTROL_ACLR,
	OUTDATA_REG,
	OUTDATA_ACLR,
	USE_EAB,
	MAXIMUM_DEPTH
)
RETURNS (q[WIDTH-1..0]);

