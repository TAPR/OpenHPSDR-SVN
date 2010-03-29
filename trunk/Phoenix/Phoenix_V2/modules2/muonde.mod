PCBNEW-LibModule-V1  5/4/2008-02:35:43
$INDEX
ALIM
C7812
CR12CC
SMA
CR12CM
CR12ZM
CR12ZZ
CC12CM
CC12CC
CC12ZM
CC12ZZ
CC08CM
CC08ZM
CC08CC
CC08ZZ
C41485CC
C41485MM
POS
RAID
STUBCC
STUBCO
TROU
GAP
SOT23EBC
$EndINDEX
$MODULE ALIM
Po 38500 21250 0 15 00200000 00000000 ~~
Li ALIM
Sc 00000000
Op 0 0 0
T0 0 0 600 600 0 150 N I 21 "U**"
T1 0 1500 600 600 0 150 N V 21 "ALIM"
DS -2000 -1000 2000 -1000 150 21
DS 2000 -1000 2000 1000 150 21
DS 2000 1000 -2000 1000 150 21
DS -2000 1000 -2000 -1000 150 21
$PAD
Sh "1" R 1000 1000 0 0 0
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po -1000 0
$EndPAD
$PAD
Sh "2" C 1200 1200 0 0 0
Dr 320 0 0
At STD N 0000FFFF
Ne 1 "GND"
Po 1000 0
$EndPAD
$EndMODULE  ALIM
$MODULE C7812
Po 41000 18000 900 15 00200000 00000000 ~~
Li C7812
Sc 00000000
Op 0 0 0
T1 -1500 0 450 402 1800 120 N V 21 "C7812"
T0 1400 0 350 350 0 120 N I 21 "U**"
DS -1050 700 -1050 750 150 21
DS -1050 750 1050 750 150 21
DS 1050 -750 -1050 -750 150 21
DS -1050 -750 -1050 700 150 21
DS -1050 -200 -850 -200 150 21
DS -850 -200 -850 200 150 21
DS -850 200 -1050 200 150 21
DS 1050 -750 1050 750 150 21
$PAD
Sh "8" R 236 748 0 0 900
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po -750 -1100
$EndPAD
$PAD
Sh "1" R 236 748 0 0 900
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po -750 1100
$EndPAD
$PAD
Sh "7" R 236 748 0 0 900
Dr 0 0 0
At SMD N 00888000
Ne 1 "GND"
Po -250 -1100
$EndPAD
$PAD
Sh "6" R 236 748 0 0 900
Dr 0 0 0
At SMD N 00888000
Ne 1 "GND"
Po 250 -1100
$EndPAD
$PAD
Sh "5" R 236 748 0 0 900
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po 750 -1100
$EndPAD
$PAD
Sh "2" R 236 748 0 0 900
Dr 0 0 0
At SMD N 00888000
Ne 1 "GND"
Po -250 1100
$EndPAD
$PAD
Sh "3" R 236 748 0 0 900
Dr 0 0 0
At SMD N 00888000
Ne 1 "GND"
Po 250 1100
$EndPAD
$PAD
Sh "4" R 236 748 0 0 900
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po 750 1100
$EndPAD
$EndMODULE  C7812
$MODULE CR12CC
Po 22000 28500 0 15 00200000 00000000 ~~
Li CR12CC
Sc 00000000
Op 0 0 0
T0 150 -900 400 400 0 120 N I 21 "R**"
T1 50 900 400 400 0 120 N V 21 "CR12CC"
DS -1000 -500 1250 -500 80 21
DS 1250 -500 1250 500 80 21
DS 1250 500 -1000 500 80 21
DS -1000 500 -1000 -500 80 21
$PAD
Sh "1" R 591 748 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -500 0
$EndPAD
$PAD
Sh "2" R 591 748 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 770 0
$EndPAD
$EndMODULE  CR12CC
$MODULE SMA
Po 37500 18000 0 15 00200000 00000000 ~~
Li SMA
Sc 00000000
Op 0 0 0
T0 0 -1050 400 400 0 120 N I 21 "J**"
T1 0 1200 400 400 0 120 N V 21 "SMA"
DS -1100 -700 1100 -700 150 21
DS 1100 -700 1100 700 150 21
DS 1100 700 -1100 700 150 21
DS -1100 700 -1100 -700 150 21
$PAD
Sh "1" R 1970 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 0 0
$EndPAD
$PAD
Sh "2" R 1970 1142 0 0 0
Dr 0 0 0
At SMD N 00A80001
Ne 1 "GND"
Po 0 0
$EndPAD
$EndMODULE  SMA
$MODULE CR12CM
Po 21750 32250 0 15 00200000 00000000 ~~
Li CR12CM
Sc 00000000
Op 0 0 0
T0 200 -950 400 400 0 120 N I 21 "R**"
T1 0 1100 400 400 0 120 N V 21 "CR12CM"
DS 1250 -500 1250 500 80 21
DS -1000 500 -1000 -500 80 21
DS -1000 -500 500 -500 80 21
DS 500 -500 1500 -750 80 21
DS 1500 -750 1500 750 80 21
DS -1000 500 500 500 80 21
DS 500 500 1500 750 80 21
DS 1500 750 1500 500 80 21
$PAD
Sh "1" R 591 748 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -500 0
$EndPAD
$PAD
Sh "2" T 591 942 -197 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 760 0
$EndPAD
$PAD
Sh "2" R 394 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 1240 0
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 1210 -280
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 1210 270
$EndPAD
$EndMODULE  CR12CM
$MODULE CR12ZM
Po 28000 29000 0 15 00200000 00000000 ~~
Li CR12ZM
Sc 00000000
Op 0 0 0
T0 -400 -900 400 400 0 120 N I 21 "R**"
T1 -250 1100 400 400 0 120 N V 21 "CR12ZM"
DS -1750 -500 -1750 500 80 21
DS -1750 500 500 500 80 21
DS 500 500 1250 750 80 21
DS 1250 750 1250 -500 80 21
DS -1750 -500 500 -500 80 21
DS 500 -500 1250 -750 80 21
DS 1250 -750 1250 -500 80 21
$PAD
Sh "1" R 1142 748 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -1000 0
$EndPAD
$PAD
Sh "2" T 591 942 -197 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 540 0
$EndPAD
$PAD
Sh "2" R 394 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 1020 0
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 1030 -280
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 1030 270
$EndPAD
$EndMODULE  CR12ZM
$MODULE CR12ZZ
Po 27500 32500 0 15 00200000 00000000 ~~
Li CR12ZZ
Sc 00000000
Op 0 0 0
T0 50 -1100 400 400 0 120 N I 21 "R**"
T1 150 1150 400 400 0 120 N V 21 "CR12ZZ"
DS 1250 -500 1250 500 80 21
DS -1000 500 -1000 -500 80 21
DS -1000 -750 1250 -750 80 21
DS 1250 -750 1250 750 80 21
DS 1250 750 -1000 750 80 21
DS -1000 750 -1000 -750 80 21
$PAD
Sh "1" T 591 942 197 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -500 0
$EndPAD
$PAD
Sh "2" T 591 942 -197 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 760 0
$EndPAD
$PAD
Sh "2" R 197 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 1140 0
$EndPAD
$PAD
Sh "1" R 197 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -880 0
$EndPAD
$EndMODULE  CR12ZZ
$MODULE CC12CM
Po 21750 39250 0 15 00200000 00000000 ~~
Li CC12CM
Sc 00000000
Op 0 0 0
T0 100 -900 400 400 0 120 N I 21 "C**"
T1 250 1000 400 400 0 120 N V 21 "CC12CM"
DS 1250 -500 1250 500 80 21
DS -1000 500 -1000 -500 80 21
DS -1000 -500 500 -500 80 21
DS 500 -500 1500 -750 80 21
DS 1500 -750 1500 750 80 21
DS -1000 500 500 500 80 21
DS 500 500 1500 750 80 21
DS 1500 750 1500 500 80 21
$PAD
Sh "1" R 591 748 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -500 0
$EndPAD
$PAD
Sh "2" T 591 942 -197 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 760 0
$EndPAD
$PAD
Sh "2" R 394 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 1240 0
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 1210 -280
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 1210 270
$EndPAD
$EndMODULE  CC12CM
$MODULE CC12ZM
Po 28250 39250 0 15 00200000 00000000 ~~
Li CC12ZM
Sc 00000000
Op 0 0 0
DS 1250 -750 1250 750 80 21
T0 -300 -900 400 400 0 120 N I 21 "C**"
T1 -400 1000 400 400 0 120 N V 21 "CC12ZM"
DS -1750 -500 -1750 500 80 21
DS -1750 500 500 500 80 21
DS 500 500 1250 750 80 21
DS -1750 -500 500 -500 80 21
DS 500 -500 1250 -750 80 21
$PAD
Sh "1" R 1142 748 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -1000 0
$EndPAD
$PAD
Sh "2" T 591 942 -197 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 540 0
$EndPAD
$PAD
Sh "2" R 394 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 1020 0
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 1030 -280
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 1030 270
$EndPAD
$EndMODULE  CC12ZM
$MODULE CC08CM
Po 37000 39250 0 15 00200000 00000000 ~~
Li CC08CM
Sc 00000000
Op 0 0 0
T0 -50 -1000 400 400 0 120 N I 21 "C**"
T1 0 1000 400 400 0 120 N V 21 "CC08CM"
DS -750 -500 -750 500 80 21
DS 250 500 1000 750 80 21
DS 1000 750 1000 -750 80 21
DS 1000 -750 250 -500 80 21
DS 250 -500 -750 -500 80 21
DS -750 500 250 500 80 21
$PAD
Sh "1" R 472 591 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -500 0
$EndPAD
$PAD
Sh "2" T 472 866 -275 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 260 0
$EndPAD
$PAD
Sh "2" R 394 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 680 0
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 670 -290
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 670 270
$EndPAD
$EndMODULE  CC08CM
$MODULE CC08ZM
Po 44750 39250 0 15 00200000 00000000 ~~
Li CC08ZM
Sc 00000000
Op 0 0 0
T0 -600 -950 400 400 0 120 N I 21 "C**"
T1 -300 1150 400 400 0 120 N V 21 "CC08ZM"
DS -1750 -500 -1750 500 80 21
DS 0 -500 750 -750 80 21
DS 750 -750 750 750 80 21
DS 750 750 0 500 80 21
DS -1750 500 0 500 80 21
DS -1750 -500 0 -500 80 21
$PAD
Sh "1" R 1142 591 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -1000 0
$EndPAD
$PAD
Sh "2" T 472 866 -275 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 100 0
$EndPAD
$PAD
Sh "2" R 394 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 1 "GND"
Po 530 0
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 480 -280
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 480 270
$EndPAD
$EndMODULE  CC08ZM
$MODULE CC08CC
Po 37250 43000 0 15 00200000 00000000 ~~
Li CC08CC
Sc 00000000
Op 0 0 0
T0 250 750 400 400 0 120 N I 21 ""
T1 -350 900 400 400 0 120 N V 21 "CC08CC"
T0 -200 -950 400 400 0 120 N I 21 "C**"
DS -1000 500 -1000 -500 80 21
DS 750 -500 750 500 80 21
DS 750 500 -1000 500 80 21
DS 750 -500 -1000 -500 80 21
$PAD
Sh "1" R 472 591 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -500 0
$EndPAD
$PAD
Sh "2" R 472 591 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 260 0
$EndPAD
$EndMODULE  CC08CC
$MODULE CC08ZZ
Po 44250 43250 0 15 00200000 00000000 ~~
Li CC08ZZ
Sc 00000000
Op 0 0 0
T0 -150 -1050 400 400 0 120 N I 21 "C**"
T1 -100 1050 400 400 0 120 N V 21 "CC08ZZ"
DS -1000 500 -1000 -500 80 21
DS -1000 750 -1000 -750 80 21
DS 750 -750 750 750 80 21
DS -1000 750 750 750 80 21
DS -1000 -750 750 -750 80 21
$PAD
Sh "1" T 472 866 275 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -500 0
$EndPAD
$PAD
Sh "2" T 472 866 -275 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 260 0
$EndPAD
$PAD
Sh "2" R 197 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 590 0
$EndPAD
$PAD
Sh "1" R 197 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -830 0
$EndPAD
$EndMODULE  CC08ZZ
$MODULE C41485CC
Po 23000 20250 0 15 00200000 00000000 ~~
Li C41485CC
Sc 00000000
Op 0 0 0
T1 0 3000 400 400 0 120 N V 21 "C41485CC"
T0 -1650 -1800 400 400 0 120 N I 21 "Q**"
DS -2750 -750 -2750 750 150 21
DS -2750 750 -750 750 150 21
DS -750 750 -750 2750 150 21
DS -750 2750 750 2750 150 21
DS 750 2750 750 750 150 21
DS 750 750 2750 750 150 21
DS 2750 750 2750 -750 150 21
DS 2750 -750 750 -750 150 21
DS 750 -750 750 -2750 150 21
DS 750 -2750 -750 -2750 150 21
DS -750 -2750 -750 -750 150 21
DS -750 -750 -2750 -750 150 21
$PAD
Sh "1" R 1182 1142 0 0 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po -1950 0
$EndPAD
$PAD
Sh "3" R 1182 1142 0 0 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po 1960 0
$EndPAD
$PAD
Sh "1" T 985 768 374 0 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po -890 0
$EndPAD
$PAD
Sh "3" T 985 768 -374 0 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po 890 0
$EndPAD
$PAD
Sh "4" T 768 985 0 -374 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po -10 -890
$EndPAD
$PAD
Sh "2" T 768 985 0 374 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po -10 900
$EndPAD
$PAD
Sh "4" R 1142 1182 0 0 0
Dr 0 0 0
At SMD N 00A88026
Ne 0 ""
Po -10 -1940
$EndPAD
$PAD
Sh "2" R 1142 1182 0 0 0
Dr 0 0 0
At SMD N 00A88026
Ne 0 ""
Po -10 1970
$EndPAD
$PAD
Sh "" C 906 906 0 0 0
Dr 0 0 0
At MECA N 00C0FFFF
Ne 0 ""
Po 0 0
$EndPAD
$EndMODULE  C41485CC
$MODULE C41485MM
Po 29000 20250 0 15 00200000 00000000 ~~
Li C41485MM
Sc 00000000
Op 0 0 0
T1 250 3100 400 400 0 120 N V 21 "C41485MM"
T0 -1800 -1900 400 400 0 120 N I 21 "Q**"
DS -2750 -750 -2750 750 150 21
DS -2750 750 -750 750 150 21
DS -750 750 -750 2750 150 21
DS -750 2750 750 2750 150 21
DS 750 2750 750 750 150 21
DS 750 750 2750 750 150 21
DS 2750 750 2750 -750 150 21
DS 2750 -750 750 -750 150 21
DS 750 -750 750 -2750 150 21
DS 750 -2750 -750 -2750 150 21
DS -750 -2750 -750 -750 150 21
DS -750 -750 -2750 -750 150 21
$PAD
Sh "1" R 1182 1142 0 0 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po -1950 0
$EndPAD
$PAD
Sh "3" R 1182 1142 0 0 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po 1960 0
$EndPAD
$PAD
Sh "1" T 985 768 374 0 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po -890 0
$EndPAD
$PAD
Sh "3" T 985 768 -374 0 0
Dr 0 0 0
At SMD N 00888002
Ne 0 ""
Po 890 0
$EndPAD
$PAD
Sh "4" T 768 985 0 -374 0
Dr 0 0 0
At SMD N 00888002
Ne 1 "GND"
Po -10 -890
$EndPAD
$PAD
Sh "2" T 768 985 0 374 0
Dr 0 0 0
At SMD N 00888002
Ne 1 "GND"
Po -10 900
$EndPAD
$PAD
Sh "4" R 1142 1182 0 0 0
Dr 0 0 0
At SMD N 00A88026
Ne 1 "GND"
Po -10 -1940
$EndPAD
$PAD
Sh "2" R 1142 1182 0 0 0
Dr 0 0 0
At SMD N 00A88026
Ne 1 "GND"
Po -10 1970
$EndPAD
$PAD
Sh "" C 906 906 0 0 0
Dr 0 0 0
At MECA N 00C0FFFF
Ne 0 ""
Po 0 0
$EndPAD
$PAD
Sh "4" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 0 -1000
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 0 1000
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po -250 1500
$EndPAD
$PAD
Sh "2" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 250 1500
$EndPAD
$PAD
Sh "4" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po 250 -1500
$EndPAD
$PAD
Sh "4" C 315 315 0 0 0
Dr 0 0 0
At STD N 00C0FFFF
Ne 1 "GND"
Po -250 -1500
$EndPAD
$EndMODULE  C41485MM
$MODULE POS
Po 43250 21750 0 15 00200000 00000000 ~~
Li POS
Sc 00000000
Op 0 0 0
T0 -50 1800 600 600 0 150 N V 21 "POS"
T1 0 0 600 600 0 150 N I 21 "***"
DC 0 0 1000 750 150 21
$PAD
Sh "" R 2000 100 0 0 0
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po 0 0
$EndPAD
$PAD
Sh "" R 100 2000 0 0 0
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po 0 0
$EndPAD
$PAD
Sh "" C 100 100 0 0 0
Dr 20 0 0
At STD N 00C0FFFF
Ne 0 ""
Po 0 0
$EndPAD
$EndMODULE  POS
$MODULE RAID
Po 39250 26000 0 15 00200000 00000000 ~~
Li RAID
Sc 00000000
Op 0 0 0
T0 -250 750 400 400 0 120 N V 21 "RAID"
T1 -250 -500 400 400 0 120 N I 21 "***"
DS -3040 -750 -3040 -1930 150 21
DS -3040 -1930 2470 -1930 150 21
DS 2470 -1930 2470 2010 150 21
DS 2470 2010 -3040 2010 150 21
DS -3040 2010 -3040 -760 150 21
$PAD
Sh "" C 788 788 0 0 0
Dr 400 0 0
At MECA N 00E0FFFF
Ne 1 "GND"
Po -2250 -750
$EndPAD
$PAD
Sh "" C 788 788 0 0 0
Dr 400 0 0
At MECA N 00E0FFFF
Ne 1 "GND"
Po 1690 -750
$EndPAD
$EndMODULE  RAID
$MODULE STUBCC
Po 40250 32900 0 15 00200000 00000000 ~~
Li STUBCC
Sc 00000000
Op 0 0 0
T1 1650 1150 400 400 0 120 N I 21 "Val*"
T0 -1900 1150 400 400 0 120 N V 21 "STUBCC"
DS -3700 -700 -3700 700 80 21
DS -3700 700 3650 700 80 21
DS 3650 -700 -3700 -700 80 21
DS 3650 -700 3650 700 80 21
$PAD
Sh "2" R 5910 788 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 500 0
$EndPAD
$PAD
Sh "2" C 394 394 0 0 0
Dr 400 0 0
At STD N 00C0FFFF
Ne 0 ""
Po 3250 -200
$EndPAD
$PAD
Sh "2" C 394 394 0 0 0
Dr 400 0 0
At STD N 00C0FFFF
Ne 0 ""
Po 3050 200
$EndPAD
$PAD
Sh "2" C 394 394 0 0 0
Dr 400 0 0
At STD N 00C0FFFF
Ne 0 ""
Po 2600 200
$EndPAD
$PAD
Sh "2" C 394 394 0 0 0
Dr 400 0 0
At STD N 00C0FFFF
Ne 0 ""
Po 2350 -200
$EndPAD
$PAD
Sh "2" C 394 394 0 0 0
Dr 400 0 0
At STD N 00C0FFFF
Ne 0 ""
Po 2800 -200
$EndPAD
$PAD
Sh "1" R 1142 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -3000 0
$EndPAD
$EndMODULE  STUBCC
$MODULE STUBCO
Po 40250 29750 0 15 00200000 00000000 ~~
Li STUBCO
Sc 00000000
Op 0 0 0
T1 1900 1100 600 600 0 120 N I 21 "Val*"
T0 0 0 600 454 0 0 N V 21 ""
T0 -1950 1100 400 400 0 120 N V 21 "STUBCO"
DS -3700 -700 -3700 700 80 21
DS -3700 700 3650 700 80 21
DS 3650 -700 -3700 -700 80 21
DS 3650 -700 3650 700 80 21
$PAD
Sh "1" R 5910 788 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 500 0
$EndPAD
$PAD
Sh "1" R 1142 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 2 "N00005"
Po -3000 0
$EndPAD
$EndMODULE  STUBCO
$MODULE TROU
Po 44500 25750 0 15 00200000 00000000 ~~
Li TROU
Sc 00000000
Op 0 0 0
T0 150 1650 600 600 0 150 N V 21 "TROU"
T1 0 0 600 600 0 150 N I 21 "**"
DC 0 0 750 750 150 21
$PAD
Sh "" C 1550 1550 0 0 0
Dr 400 0 0
At MECA N 00E0FFFF
Ne 1 "GND"
Po 0 0
$EndPAD
$EndMODULE  TROU
$MODULE CC12CC
Po 22100 42500 0 15 00200000 00000000 ~~
Li CC12CC
Sc 00000000
Op 0 0 0
T0 50 -900 400 400 0 120 N I 21 "C**"
T1 0 900 400 400 0 120 N V 21 "CC12CC"
DS -1100 -500 1150 -500 80 21
DS 1150 -500 1150 500 80 21
DS 1150 500 -1100 500 80 21
DS -1100 500 -1100 -500 80 21
$PAD
Sh "1" R 591 748 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -600 0
$EndPAD
$PAD
Sh "2" R 591 748 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 600 0
$EndPAD
$EndMODULE  CC12CC
$MODULE CC12ZZ
Po 28200 42700 0 15 00200000 00000000 ~~
Li CC12ZZ
Sc 00000000
Op 0 0 0
T0 -50 -1100 400 400 0 120 N I 21 "C**"
T1 0 1200 400 400 0 120 N V 21 "CC12ZZ"
DS -1100 -750 1150 -750 80 21
DS 1150 -750 1150 750 80 21
DS 1150 750 -1100 750 80 21
DS -1100 750 -1100 -750 80 21
$PAD
Sh "1" T 591 942 197 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -600 0
$EndPAD
$PAD
Sh "2" T 591 942 -197 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 600 0
$EndPAD
$PAD
Sh "2" R 197 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 1000 0
$EndPAD
$PAD
Sh "1" R 197 1142 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -980 0
$EndPAD
$EndMODULE  CC12ZZ
$MODULE GAP
Po 0 0 0 15 4230402A 00000000 ~~
Li GAP
Sc 00000000
Op 0 0 0
T0 0 -500 200 200 0 50 N I 21 "GAP"
T1 100 500 200 200 0 50 N I 21 "VAL*"
DS -900 -300 900 -300 50 21
DS 900 -300 900 300 50 21
DS 900 300 -900 300 50 21
DS -900 300 -900 -300 50 21
$PAD
Sh "1" R 200 200 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po -600 0
$EndPAD
$PAD
Sh "2" R 200 200 0 0 0
Dr 0 0 0
At SMD N 00A88000
Ne 0 ""
Po 600 0
$EndPAD
$EndMODULE  GAP
$MODULE SOT23EBC
Po 0 0 0 15 47F6E570 00000000 ~~
Li SOT23EBC
Cd Module CMS SOT23 Transistore EBC
Kw CMS SOT
Sc 00000000
Op 0 0 0
At SMD 
T0 0 -950 300 300 0 80 N V 21 "SOT23EBC"
T1 0 0 300 300 0 80 N I 21 "VAL**"
DS -600 -150 600 -150 50 21
DS 600 -150 600 150 50 21
DS 600 150 -600 150 50 21
DS -600 150 -600 -150 50 21
$PAD
Sh "1" R 360 360 0 0 0
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po -350 -400
$EndPAD
$PAD
Sh "2" R 360 360 0 0 0
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po 350 -400
$EndPAD
$PAD
Sh "3" R 256 315 0 0 0
Dr 0 0 0
At SMD N 00888000
Ne 0 ""
Po 0 551
$EndPAD
$SHAPE3D
Na "smd/cms_sot23.wrl"
Sc 0.130000 0.150000 0.150000
Of 0.000000 0.000000 0.000000
Ro 0.000000 0.000000 0.000000
$EndSHAPE3D
$EndMODULE  SOT23EBC
$EndLIBRARY
