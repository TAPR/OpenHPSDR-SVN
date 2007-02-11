/* Quartus II Version 6.1 Build 201 11/27/2006 SJ Web Edition */
JedecChain;
	FileRevision(JESD32A);
	DefaultMfr(6E);

	P ActionCode(Ign)
		Device PartName(EP2C8) MfrSpec(OpMask(0));
	P ActionCode(Cfg)
		Device PartName(EPM240T100) Path("") File("init_load.pof") MfrSpec(OpMask(1) SEC_Device(EPM240T100) Child_OpMask(2 0 0));

ChainEnd;

AlteraBegin;
	ChainType(JTAG);
AlteraEnd;
