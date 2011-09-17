from urllib2 import urlopen
import string

# Program to reasd the arrl.org website for the current DXCC list abd convert it into XML
# by David R. Larsen, Copyright September, 17, 2011

StartReadLines = 24
LastReadLines = 387
TextFileName = 'http://www.arrl.org/files/file/DXCC/dxcclist_2011e.txt'
PrefixFieldEnd = 20
EntityFieldEnd = 47
ContFieldEnd = 53
ITUFieldEnd = 63
CQFieldEnd = 69
CodeFieldEnd = 75

data = urlopen( TextFileName ).readlines()
idx = 0
for line in data:
   idx = idx + 1
   if idx > StartReadLines and idx < LastReadLines: 
      nline = string.rstrip( string.expandtabs(line))
      # print nline
      prefix = string.strip(nline[0:PrefixFieldEnd])
      country = string.strip(nline[PrefixFieldEnd:EntityFieldEnd])
      cont = string.strip(nline[EntityFieldEnd:ContFieldEnd])
      ITU = string.strip(nline[ContFieldEnd:ITUFieldEnd])
      CQ = string.strip(nline[ITUFieldEnd:CQFieldEnd])
      code = string.strip(nline[CQFieldEnd:CodeFieldEnd])
      print "<country>"
      print "  <prefix>%s</prefix>" % prefix
      print "  <name>%s</name>" % country
      print "  <cont>%s</cont>" % cont
      print "  <ITU>%s</ITU>" % ITU
      print "  <CQ>%s</CQ>" % CQ
      print "  <code>%s</code>" % code
      print "</country>"
    