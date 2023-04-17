USE [Prechart]
GO

DROP TABLE [dbo].[SocialeVerzekeringen]
GO

CREATE TABLE [dbo].[SocialeVerzekeringen]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Jaar] [int] NOT NULL, -- Jaar
	[PremiePercentage_Algemene_Ouderdoms_Wet] [decimal](18, 2) NOT NULL,						-- PPAOW
	[PremiePercentage_Nabestaanden] [decimal](18, 2) NOT NULL,									-- PPNB
	[PremiePercentage_Wet_Langdurige_Zorg] [decimal] (18, 2) NOT NULL,							-- PPWlz
	[Maximaal_Sociaalverzekering_Premieloon] [decimal](18, 2) NOT NULL,							-- MaxSVLn
	[PremiePercentage_Algemeen_Werkloosheids_Fonds_Laag] [decimal](18, 2) NOT NULL,				-- AwfLaag
	[PremiePercentage_Algemeen_Werkloosheids_Fonds_Hoog] [decimal](18, 2) NOT NULL,				-- AwfHoog
	[PremiePercentage_Uitvoeringsdfonds_voor_de_Overheid] [decimal](18, 2) NOT NULL,			-- UFO 
	[PremiePercentage_Wet_Arbeidsongeschikheid_Laag] [decimal](18, 2) NOT NULL,					-- WAOLaag
	[PremiePercentage_Wet_Arbeidsongeschikheid_Hoog] [decimal](18, 2) NOT NULL,					-- WAOHoog
	[PremiePercentage_Ziektekostenverzekering_Werkgeversbijdrage] [decimal](18, 2) NOT NULL,	-- PPZVW_Wgr
	[PremiePercentage_Ziektekostenverzekering_Werknemersbijdrage] [decimal](18, 2) NOT NULL,	-- PPZVW_Wnr
	[Maximaal_Ziektekostenverzekering_Loon] [decimal](18, 2) NOT NULL,							-- Max_ZVW
	[Sociale_Verzekeringen_Record_Actief] [int] NOT NULL,										-- SVActief
	[Actief_Vanaf] [date] NOT NULL,																-- SV_Vanaf
	[Actief_Tot] [date] NOT NULL																-- SV_Tot
)

GO

Insert into SocialeVerzekeringen
(
	[Jaar] ,																					-- Jaar
	[PremiePercentage_Algemene_Ouderdoms_Wet],													-- PPAOW
	[PremiePercentage_Nabestaanden],															-- PPNB
	[PremiePercentage_Wet_Langdurige_Zorg],														-- PPWlz
	[Maximaal_Sociaalverzekering_Premieloon],													-- MaxSVLn
	[PremiePercentage_Algemeen_Werkloosheids_Fonds_Laag],										-- AwfLaag
	[PremiePercentage_Algemeen_Werkloosheids_Fonds_Hoog],										-- AwfHoog
	[PremiePercentage_Uitvoeringsdfonds_voor_de_Overheid],										-- UFO 
	[PremiePercentage_Wet_Arbeidsongeschikheid_Laag],											-- WAOLaag
	[PremiePercentage_Wet_Arbeidsongeschikheid_Hoog],											-- WAOHoog
	[PremiePercentage_Ziektekostenverzekering_Werkgeversbijdrage],								-- PPZVW_Wgr
	[PremiePercentage_Ziektekostenverzekering_Werknemersbijdrage],								-- PPZVW_Wnr
	[Maximaal_Ziektekostenverzekering_Loon],													-- Max_ZVW
	[Sociale_Verzekeringen_Record_Actief],														-- SVActief
	[Actief_Vanaf],																				-- SV_Vanaf
	[Actief_Tot]																				-- SV_Tot
)
Values
( 
	2022,		--	[Jaar] ,																	-- Jaar
	17.90,		--	[PremiePercentage_Algemene_Ouderdoms_Wet],									-- PPAOW
	0.10,		--	[PremiePercentage_Nabestaanden],											-- PPNB
	9.65,		--	[PremiePercentage_Wet_Langdurige_Zorg],										-- PPWlz
	59706,		--	[Maximaal_Sociaalverzekering_Premieloon],									-- MaxSVLn
	2.7,		--	[PremiePercentage_Algemeen_Werkloosheids_Fonds_Laag],						-- AwfLaag
	7.7,		--	[PremiePercentage_Algemeen_Werkloosheids_Fonds_Hoog],						-- AwfHoog
	0.68,		--	[PremiePercentage_Uitvoeringsdfonds_voor_de_Overheid],						-- UFO 
	5.49,		--	[PremiePercentage_Wet_Arbeidsongeschikheid_Laag],							-- WAOLaag
	7.05,		--	[PremiePercentage_Wet_Arbeidsongeschikheid_Hoog],							-- WAOHoog
	6.75,		--	[PremiePercentage_Ziektekostenverzekering_Werkgeversbijdrage],				-- PPZVW_Wgr
	5.50,		--	[PremiePercentage_Ziektekostenverzekering_Werknemersbijdrage],				-- PPZVW_Wnr
	59706,		--	[Maximaal_Ziektekostenverzekering_Loon],									-- Max_ZVW
	1,			--	[Sociale_Verzekeringen_Record_Actief],										-- SVActief
	'20220101', --  [Actief_Vanaf],																-- SV_Vanaf
	'20221231'	--  [Actief_Tot]																-- SV_Tot
)


select * from SocialeVerzekeringen
