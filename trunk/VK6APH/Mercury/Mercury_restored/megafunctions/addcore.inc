FUNCTION addcore (dataa[(width) - (1)..0], datab[(width) - (1)..0], cin, 
					  add_sub, clock, aclr, clken)
    WITH (WIDTH, DIRECTION, REPRESENTATION, USE_CS_BUFFERS)
    RETURNS (result[(width) - (1)..0], cout, bg_out, bp_out, unreg_result[(width) - (1)..0], unreg_cout, overflow);
