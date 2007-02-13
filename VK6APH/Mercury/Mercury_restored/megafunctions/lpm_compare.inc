FUNCTION lpm_compare (dataa[LPM_WIDTH-1..0], datab[LPM_WIDTH-1..0],
						clock, aclr, clken)
    WITH (LPM_WIDTH, LPM_REPRESENTATION, LPM_PIPELINE,
			CHAIN_SIZE, ONE_INPUT_IS_CONSTANT)
    RETURNS (alb, aeb, agb, ageb, aneb, aleb);
