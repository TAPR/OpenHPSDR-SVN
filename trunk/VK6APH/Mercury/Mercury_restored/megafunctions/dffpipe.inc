FUNCTION dffpipe( clock, d[width-1..0], ena, clrn, prn, sclr )
    WITH( WIDTH, DELAY )
    RETURNS( q[width-1..0], pq[delay..0][width-1..0] );
