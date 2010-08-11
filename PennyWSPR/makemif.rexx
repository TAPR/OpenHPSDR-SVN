/*
 * generate a .mif file for PennyWSPR
 *
 * Copyright Bill Tracey KD5TFD 2010
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


tmpfile = 'makemif.tmp'
ofile = 'symtab.mif'
'del' ofile  '>nul 2>&1'
syms = ''
parse arg args

'genwspr' args  '>' tmpfile
/* read thru line saying 'Encoded symbols' */
do while lines(tmpfile) \= 0
    l = strip(linein(tmpfile))
    if l = 'Encoded Symbols' then leave
end

/* now read the symbols */
do while lines(tmpfile) \= 0
    l = strip(linein(tmpfile))
    if l = '' then leave
    syms = syms l
end

if syms = '' then do
    say 'Error: No syms!!'
    exit 1
end


call lineout ofile, '-- generated with makemif.rexx'
call lineout ofile, '--' args

call lineout ofile, 'WIDTH=2;'
call lineout ofile, 'DEPTH=162;'
call lineout ofile, 'ADDRESS_RADIX=UNS;'
call lineout ofile, 'DATA_RADIX=UNS;'
call lineout ofile, 'CONTENT BEGIN'

do i = 0 to 161
    if syms = '' then leave
    parse var syms this_sym syms
    this_sym = strip(this_sym); syms = strip(syms)
    call lineout ofile, i ':' this_sym || ';'
end

call lineout ofile, 'END;'
