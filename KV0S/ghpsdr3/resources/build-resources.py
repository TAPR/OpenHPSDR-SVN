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
print "<resources>"

data = urlopen( TextFileName ).readlines()
idx = 0
for line in data:
   idx = idx + 1
   if idx > StartReadLines and idx < LastReadLines: 
      nline = string.rstrip( string.expandtabs(line))
      # print nline
      temp = string.strip(nline[0:PrefixFieldEnd])
      prefix = string.strip(temp,'*#') 
      pref = re.split( ',', prefix )
      #print "comma string", pref
      country = string.strip(nline[PrefixFieldEnd:EntityFieldEnd])
      cont = string.strip(nline[EntityFieldEnd:ContFieldEnd])
      ITU = string.strip(nline[ContFieldEnd:ITUFieldEnd])
      CQ = string.strip(nline[ITUFieldEnd:CQFieldEnd])
      code = string.strip(nline[CQFieldEnd:CodeFieldEnd])
      print "   <country>"
      for p in pref:
	 if( string.find( p, "-" ) > 0 ):
	   print "hyphen string"
	   if( re.match( "([A-Z]+)", p ) > 0 ):
	     slist = re.split( '-', p )
	     end = ord(slist[1][1]) - ord(slist[0][1])
	     print "Two Character", end
	   elif( re.match( "([A-Z][0-9])", p ) > 0 ):
	     print "Character Number"
	   print "      <prefix>%s</prefix>" % string.strip( p )
	 else:
           print "      <prefix>%s</prefix>" % string.strip( p )
         
      print "      <name>%s</name>" % country
      print "      <cont>%s</cont>" % cont
      print "      <ITU>%s</ITU>" % ITU
      print "      <CQ>%s</CQ>" % CQ
      print "      <code>%s</code>" % code
      print "   </country>"

print "</resources>"