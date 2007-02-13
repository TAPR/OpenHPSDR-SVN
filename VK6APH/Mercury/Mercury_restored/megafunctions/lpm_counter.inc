FUNCTION lpm_counter
					(
					-- INPUTS
					data[LPM_WIDTH-1..0],
					clock,
					clk_en,
					cnt_en,
					updown,
					cin,
					aclr,
					aset,
					aconst,
					aload,
					sclr,
					sset,
					sconst,
					sload
					)
					WITH
					(
					-- PARAMETERS
					LPM_WIDTH,
					LPM_DIRECTION,
					LPM_MODULUS,
					LPM_AVALUE,
					LPM_SVALUE,
					LPM_PORT_UPDOWN,
					CARRY_CNT_EN,
					LABWIDE_SCLR,
					USE_NEW_VERSION
					)
					RETURNS
					(
					-- OUPUTS
					q[LPM_WIDTH-1..0],
					cout,
					eq[15..0]%,
					debug_out[6..0]%
					);
