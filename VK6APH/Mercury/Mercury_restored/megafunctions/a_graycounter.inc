------------------------------------------------------------------
-- a_graycounter parameterized megafunction include file
-- Generated with 'clearbox' loader - do not edit
------------------------------------------------------------------
FUNCTION a_graycounter (
	aclr,
	clk_en,
	clock,
	cnt_en,
	sclr,
	updown
)
WITH (
	PVALUE,
	WIDTH
)
RETURNS (
	q[width-1..0],
	qbin[width-1..0]
);
