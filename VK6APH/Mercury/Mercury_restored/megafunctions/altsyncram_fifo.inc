FUNCTION altsyncram_fifo	(
								data[LPM_WIDTH - 1..0],
								wrclk,
								rdclk,
								aclr,
								wrreq,
								rdreq
							)
							WITH
							(
								LPM_WIDTH,
								LPM_NUMWORDS,
								LPM_SHOWAHEAD,
								UNDERFLOW_CHECKING,
								OVERFLOW_CHECKING,
								USE_EAB,
								DELAY_RDUSEDW,
								DELAY_WRUSEDW,
								RDSYNC_DELAYPIPE,
								WRSYNC_DELAYPIPE
							)
							RETURNS
							(
								wrempty,
								wrfull,
								wrusedw[CEIL(LOG2(LPM_NUMWORDS)) - 1..0],
								rdempty,
								rdfull,
								rdusedw[CEIL(LOG2(LPM_NUMWORDS)) - 1..0],
								q[LPM_WIDTH - 1..0],
x_fifo_wreq_out,
x_stall_fifo,
x_valid_rreq,
x_ram_output_bus[LPM_WIDTH - 1..0],
x_ramread_address[CEIL(LOG2(LPM_NUMWORDS)) - 1..0],
x_control_pipe[2..0],
x_ram_address[CEIL(LOG2(LPM_NUMWORDS)) - 1..0],
x_ram_output_address[CEIL(LOG2(LPM_NUMWORDS)) - 1..0]
							);
