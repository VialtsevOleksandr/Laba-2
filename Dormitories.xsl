<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<html>
			<head>
				<title>Список студентів у гуртожитках</title>
			</head>
			<body>
				<h1>Список студентів у гуртожитках</h1>
				<table border="1">
					<tr>
						<th>П.І.П.</th>
						<th>Номер гуртожитку</th>
						<th>Поверх</th>
						<th>Номер кімнати</th>
						<th>Дата закінчення договору</th>
						<th>Чи проживає у гуртожитку</th>
						<th>Факультет</th>
						<th>Кафедра</th>
						<th>Курс</th>
					</tr>
					<xsl:apply-templates select="//dormitory"/>
				</table>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="dormitory">
		<tr>
			<td>
				<xsl:value-of select="PersonalInformation/FullName"/>
			</td>
			<td>
				<xsl:value-of select="PersonalInformation/DormitoryNumber"/>
			</td>
			<td>
				<xsl:value-of select="PersonalInformation/Floor"/>
			</td>
			<td>
				<xsl:value-of select="PersonalInformation/RoomNumber"/>
			</td>
			<td>
				<xsl:value-of select="PersonalInformation/ContractEndDate"/>
			</td>
			<td>
				<xsl:value-of select="PersonalInformation/IsResidingInDormitory"/>
			</td>
			<td>
				<xsl:value-of select="PersonalInformation/AcademicDetails/Faculty"/>
			</td>
			<td>
				<xsl:value-of select="PersonalInformation/AcademicDetails/Department"/>
			</td>
			<td>
				<xsl:value-of select="PersonalInformation/AcademicDetails/Course"/>
			</td>
		</tr>
	</xsl:template>

</xsl:stylesheet>
