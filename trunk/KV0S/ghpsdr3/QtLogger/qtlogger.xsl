<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <h2><xsl:value-of select="log/contacts/contact/STATION_CALLSIGN"/> Radio Log</h2>
  <table border="1">
    <tr bgcolor="#9acd32">
      <th>Call</th>
      <th>Date</th>
      <th>Time</th>
    </tr>
    <xsl:for-each select="log/contacts/contact">
    <tr>
      <td><xsl:value-of select="CALL"/></td>
      <td><xsl:value-of select="QSO_DATE"/></td>
      <td><xsl:value-of select="TIME_ON"/></td>
    </tr>
    </xsl:for-each>
  </table>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet> 
