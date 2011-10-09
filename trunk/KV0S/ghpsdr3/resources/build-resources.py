#!/usr/bin/python

from urllib2 import urlopen
import string
import re

# Program to reasd the arrl.org website for the current DXCC list abd convert it into XML
# by David R. Larsen, Copyright September, 17, 2011

# DXCC is currently stored as fixed format text file on the www.arrl.org

StartReadLines = 24
LastReadLines = 387
TextFileName = 'http://www.arrl.org/files/file/DXCC/dxcclist_2011e.txt'
PrefixFieldEnd = 20
EntityFieldEnd = 47
ContFieldEnd = 53
ITUFieldEnd = 63
CQFieldEnd = 69
CodeFieldEnd = 75

print "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>"
print "<?xml-stylesheet type=\"text/xsl\" href=\"country.xsl\"?>"
print "<resources>"

data = urlopen( TextFileName ).readlines()
idx = 0
for line in data:
   idx = idx + 1
   if idx > StartReadLines and idx < LastReadLines: 
      nline = string.rstrip( string.expandtabs(line))
      temp = string.strip(nline[0:PrefixFieldEnd])
      temp1 = string.strip(temp,'*#') 
      prefix = string.strip(temp1, ' ') 
      pstr = re.sub( '\([0-9]+\)', '', prefix )
      pref = re.split( ',', pstr )
      country = string.strip(nline[PrefixFieldEnd:EntityFieldEnd])
      cont = string.strip(nline[EntityFieldEnd:ContFieldEnd])
      ITU = string.strip(nline[ContFieldEnd:ITUFieldEnd])
      CQ = string.strip(nline[ITUFieldEnd:CQFieldEnd])
      code = string.strip(nline[CQFieldEnd:CodeFieldEnd])
      pstr_lst = []  
      for p in pref:
	 if( string.find( p, "-" ) > 0 ):
	   if( re.match( "[A-Z][A-Z]", p ) > 0 ):
	     slist = re.split( '-', p )
	     end = ord(slist[1][1]) - ord(slist[0][1])
	     for i in range(ord(slist[0][1]), ord(slist[1][1])):
                pstr_lst.append(slist[0][0] + chr(i))
	    
	     pstr_lst.append(slist[1])
           elif( re.match( "([0-9][A-Z])", p ) > 0 ):
	     slist = re.split( '-', p )
	     end = ord(slist[1][1]) - ord(slist[0][1])
	     for i in range(ord(slist[0][1]), ord(slist[1][1])):
	        pstr_lst.append(slist[0][0] + chr(i))
	    
	     pstr_lst.append(slist[1])
	   elif( re.match( "([A-Z][0-9])", p ) > 0 ):
	     slist = re.split( '-', p )
	     end = ord(slist[1]) - ord(slist[0][1])
	     for i in range(ord(slist[0][1]), ord(slist[1])):
	        pstr_lst.append(slist[0][0] + chr(i))
	    
	     pstr_lst.append(slist[0][0] + slist[1])
         else:
           pstr_lst.append( p )
      print "   <country code=\"%s\" prefix=\"%s\">" % (code, ','.join( pstr_lst ))
      print "      <name>%s</name>" % string.replace(country, '&', '&amp;' )
      print "      <cont>%s</cont>" % cont
      print "      <ITU>%s</ITU>" % ITU
      print "      <CQ>%s</CQ>" % CQ
      print "   </country>"

print "</resources>"
