<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="text()"/>
	<xsl:template match="Wrestlers">
		<html>
			<Document xmlns="Aspose.Pdf" IsAutoHyphenated="false">
				<PageInfo>
					<DefaultTextState BackgroundColor="#000000" Font="Helvetica" FontSize="12" LineSpacing="4"/>
					<Margin Left="5cm" Right="5cm" Top="3cm" Bottom="15cm" />
				</PageInfo>
				<Page id="wwe">
					<Image>
						<xsl:attribute name="File">
							<xsl:text>Input/wwe.jpg</xsl:text>
						</xsl:attribute>
					</Image>
				</Page>
				<Page id="survivor">
					<Table ColumnWidths="50% 50%">
						<Row>
							<Cell>
								<Table>
									<xsl:apply-templates select="Raw/Wrestler"/>
								</Table>
							</Cell>
							<Cell>
								<Table>
									<xsl:apply-templates select="Smackdown/Wrestler"/>
								</Table>
							</Cell>
						</Row>
					</Table>
				</Page>
			</Document>
		</html>
	</xsl:template>

	<xsl:template match="Raw/Wrestler">		
			<Row>
				<Cell ColSpan="30">
					<TextFragment>
						<TextSegment>
							<xsl:value-of select="Name"/>
						</TextSegment>
					</TextFragment>
				</Cell>
			</Row>
			<Row>
				<Cell>
					<TextFragment>
						<TextSegment>
							<xsl:value-of select="Finisher"/>
							<xsl:text>&#xA;</xsl:text>
						</TextSegment>
					</TextFragment>
				</Cell>
			</Row>
	</xsl:template>

	<xsl:template match="Smackdown/Wrestler">
		<Row>
				<Cell ColSpan="30">
					<TextFragment>
						<TextSegment>
							<xsl:value-of select="Name"/>
						</TextSegment>
					</TextFragment>
				</Cell>
			</Row>
			<Row>
				<Cell>
					<TextFragment>
						<TextSegment>
							<xsl:value-of select="Finisher"/>
							<xsl:text>&#xA;</xsl:text>
						</TextSegment>
					</TextFragment>
				</Cell>
			</Row>
	</xsl:template>
</xsl:stylesheet>

