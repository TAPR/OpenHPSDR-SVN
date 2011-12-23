<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <h2><xsl:value-of select="AmateurRadioLog/contacts/record/author"/> Radio Log</h2>
  <table border="1">
    <tr bgcolor="#9acd32">
      <th>Call</th>
      <th>Date</th>
      <th>Time</th>
    </tr>
    <xsl:for-each select="AmateurRadioLog/contacts/record">
    <tr>
      <td><xsl:value-of select="call"/></td>
      <td><xsl:value-of select="qso_date"/></td>
      <td><xsl:value-of select="time_on"/></td>
    </tr>
    </xsl:for-each>
  </table>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet> 
