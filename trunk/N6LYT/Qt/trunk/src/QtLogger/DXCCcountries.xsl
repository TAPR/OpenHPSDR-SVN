<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
  <html>
  <body>
  <h2>DXCC Countries and Subdivisions</h2>
  <table border="1">
    <tr bgcolor="#9acd32">
      <th>Country</th>
      <th>Code</th>
      <th>Prefix</th>
      <th>Subdivision</th>
      <th>ITU</th>
      <th>CQ</th>
    </tr>
    <xsl:for-each select="resources/country">
   <tr>
      <td><xsl:value-of select="name"/></td>
      <td><xsl:value-of select="@code"/></td>
      <td><xsl:value-of select="subdivision/@prefix"/></td>
      <td><xsl:value-of select="subdivision/name"/></td>
      <td><xsl:value-of select="subdivision/ITU"/></td>
      <td><xsl:value-of select="subdivision/CQ"/></td>

    </tr>
    </xsl:for-each>
  </table>
  </body>
  </html>
</xsl:template>

</xsl:stylesheet> 
