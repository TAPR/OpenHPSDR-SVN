FUNCTION alt_sync_fifo	(
					data[LPM_WIDTH - 1..0],
					rdclk,
					rdreq,
					wrclk,
					wrreq,
					aclr
				)
			WITH
				(
					LPM_WIDTH,
					LPM_NUMWORDS,
					LPM_SHOWAHEAD,
					UNDERFLOW_CHECKING,
					OVERFLOW_CHECKING,
					USE_EAB
				)
		  RETURNS
		  		(
					q[LPM_WIDTH - 1..0],
					rdempty,
					rdfull,
					wrempty,
					wrfull,
					rdusedw[CEIL(LOG2(LPM_NUMWORDS)) - 1..0],
					wrusedw[CEIL(LOG2(LPM_NUMWORDS)) - 1..0],
db_port[CEIL(LOG2(LPM_NUMWORDS)) - 1..0]
				);
