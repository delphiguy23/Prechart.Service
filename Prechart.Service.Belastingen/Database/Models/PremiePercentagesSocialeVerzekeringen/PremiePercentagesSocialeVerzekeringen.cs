using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Prechart.Service.Belastingen.Database.Models;

public class PremiePercentagesSocialeVerzekeringen
{
    public int Id { get; set; }
    [JsonProperty(PropertyName = "Jaar")] public int Jaar { get; set; }

    [JsonProperty(PropertyName = "PPAOW")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageAlgemeneOuderdomsWet { get; set; }

    [JsonProperty(PropertyName = "PPNB")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageNabestaanden { get; set; }

    [JsonProperty(PropertyName = "PPWlz")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageWetLangdurigeZorg { get; set; }

    [JsonProperty(PropertyName = "MaxSVLn")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal MaximaalSociaalverzekeringPremieloon { get; set; }

    [JsonProperty(PropertyName = "PPAwfLaag")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageAlgemeenWerkloosheidsFondsLaag { get; set; }

    [JsonProperty(PropertyName = "PPAwfHoog")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageAlgemeenWerkloosheidsFondsHoog { get; set; }

    [JsonProperty(PropertyName = "PPUFO")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageUitvoeringsdfondsvoordeOverheid { get; set; }

    [JsonProperty(PropertyName = "PPWAOLaag")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageWetArbeidsongeschikheidLaag { get; set; }

    [JsonProperty(PropertyName = "PPWAOHoog")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageWetArbeidsongeschikheidHoog { get; set; }

    [JsonProperty(PropertyName = "PPWko")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageWetKinderopvang { get; set; }

    [JsonProperty(PropertyName = "PPZVWWgr")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageZiektekostenverzekeringWerkgeversbijdrage { get; set; }

    [JsonProperty(PropertyName = "PPZVWWnr")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiePercentageZiektekostenverzekeringWerknemersbijdrage { get; set; }

    [JsonProperty(PropertyName = "MaxZVW")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal MaximaalZiektekostenverzekeringLoon { get; set; }

    [JsonProperty(PropertyName = "SVActief")]
    public bool SocialeVerzekeringenRecordActief { get; set; }

    [JsonProperty(PropertyName = "SVActiefVanaf")]
    public DateTime SocialeVerzekeringenRecordActiefVanaf { get; set; }

    [JsonProperty(PropertyName = "SVActiefTot")]
    public DateTime SocialeVerzekeringenRecordActiefTot { get; set; }
}

/*
 *
 *-------------------------------------------------------------------------------------------
--
-- Creation of the table PremiePercentages_SocialeVerzekeringen met PPWko
--
-- Author	: Peter Everaert
-- Date		: 21-06-2022
-- Version	: 1.0
--
--------------------------------------------------------------------------------------------


USE [Prechart]
GO

DROP TABLE [dbo].[PremiePercentages_SocialeVerzekeringen]
GO

CREATE TABLE [dbo].[PremiePercentages_SocialeVerzekeringen]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Jaar] [int] NOT NULL, -- Jaar
	[PremiePercentage_Algemene_Ouderdoms_Wet] [decimal](18, 2) NOT NULL,						-- PPAOW
	[PremiePercentage_Nabestaanden] [decimal](18, 2) NOT NULL,									-- PPNB
	[PremiePercentage_Wet_Langdurige_Zorg] [decimal] (18, 2) NOT NULL,							-- PPWlz
	[Maximaal_Sociaalverzekering_Premieloon] [decimal](18, 2) NOT NULL,							-- MaxSVLn
	[PremiePercentage_Algemeen_Werkloosheids_Fonds_Laag] [decimal](18, 2) NOT NULL,				-- PPAwfLaag
	[PremiePercentage_Algemeen_Werkloosheids_Fonds_Hoog] [decimal](18, 2) NOT NULL,				-- PPAwfHoog
	[PremiePercentage_Uitvoeringsdfonds_voor_de_Overheid] [decimal](18, 2) NOT NULL,			-- PPUFO 
	[PremiePercentage_Wet_Arbeidsongeschikheid_Laag] [decimal](18, 2) NOT NULL,					-- PPWAOLaag
	[PremiePercentage_Wet_Arbeidsongeschikheid_Hoog] [decimal](18, 2) NOT NULL,					-- PPWAOHoog
	[PremiePercentage_Wet_Kinderopvang] [decimal] (18,2) NOT NULL,								-- PPWko
	[PremiePercentage_Ziektekostenverzekering_Werkgeversbijdrage] [decimal](18, 2) NOT NULL,	-- PPZVW_Wgr
	[PremiePercentage_Ziektekostenverzekering_Werknemersbijdrage] [decimal](18, 2) NOT NULL,	-- PPZVW_Wnr
	[Maximaal_Ziektekostenverzekering_Loon] [decimal](18, 2) NOT NULL,							-- Max_ZVW
	[Sociale_Verzekeringen_Record_Actief] [int] NOT NULL,										-- SVActief
	[Sociale_Verzekeringen_Record_Actief_Vanaf] [date] NOT NULL,								-- SVActiefVanaf
	[Sociale_Verzekeringen_Record_Actief_Tot] [date] NOT NULL									-- SVActiefTot
)

GO


select * from PremiePercentages_SocialeVerzekeringen

 * 
 */