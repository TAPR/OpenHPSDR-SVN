FUNCTION a_fefifo (usedw_in[lpm_widthad-1..0], wreq, rreq, threshlevel[lpm_widthad-1..0], clock, aclr, sclr, usedw_is_1)
    WITH (LPM_WIDTHAD, LPM_NUMWORDS, ALLOW_RWCYCLE_WHEN_FULL, UNDERFLOW_CHECKING, OVERFLOW_CHECKING, USEDW_IN_DELAY)
    RETURNS (usedw_out[lpm_widthad-1..0], empty, full, threshold);
