
/*

For demonstration purposes please use WitInkomen = 10 and GroenInkomen = 300. the Prechart tables are right the Klout7 calculations are wrong. 

Test Script For th Calculations in 2022 and to prepare for comparisons in 2023

Emerson this test script is correct. Please put  it in your database. Your Database will be leading. I lose all kinds of things. This routine is leading. 

Ammendments made:
Author	: Peter Everaert
Date	: 15 June 2022 
Version	: 3.0

Author	: Peter Everaert
Date	: 10 june 2022
Version	: 2.1

Last tested June 15 still finding discrepancies between Klout7 code and new Prechart code. 
Initial assessment is that the Prechart code is more accurate. 

This is a script to test if the new calculation engine has the same answers as the original one. The issue here is that the paramaternumbers for the two
queries are different. Here are the description of the differences.

Klout7

LTV 2 = Month
    3 = Week
	All others are day

CountryCode
Woonlandbeginsel
   Id	Code	Name	       DutchName	Active
   0	NL	    Netherlands	    Nederland	   1
   1	LK	    Landenkring	    Landenkring	   1
   2	DL	    DerdeLanden	    DerdeLanden	   1
   3	BE	    Belgium	        Belgie	       1
   4	SA	    SurinameAruba	SurinameAruba  1
	   
[GetTaxpayment_2022] (
  @IncomeWhite DECIMAL(18, 2)
  ,@IncomeGreen DECIMAL(18, 2)
  ,@Basis_Dagen INT
  ,@Birthday DATE
  ,@GenTaxDiscount INT
  ,@LTV_Code INT
  ,@CountryCode INT
  ,@CumulatiefAlgemeneHeffingskorting DECIMAL(18, 2)
  ,@CumulatiefArbeidskorting DECIMAL(18, 2)
  )

Prechart

Loontijdvak (LTV)
   1 = Day
   2 = Week
   3 = 4 Weekly
   4 = Month
   5 = Quarter

Woonlandbeginsel (CountryCode)
   Id	Code	Name	       DutchName	Active
   1	NL	    Netherlands	    Nederland	   1
   2	BE	    Belgium	        Belgie	       1
   3	LK	    Landenkring	    Landenkring	   1
   4	DL	    DerdeLanden	    DerdeLanden	   1
   5	SA	    SurinameAruba	SurinameAruba  1


(  @InkomenWit			DECIMAL(18, 2)
  ,@InkomenGroen		DECIMAL(18, 2)
  ,@Basis_Dagen			INT
  ,@Geboortedatum		DATE
  ,@AHKInd				INT
  ,@Loontijdvak			INT
  ,@Woonlandbeginsel	INT
  )

There are no cumulatives because it is allowed to calculate versus the tables in a Tijdvak without taking into account the yearly maximum.

*/
--select * from country

--select * from type

-- Query to see the values in the tables in Prechart for comparison of the two calculations

DECLARE @PR_InkomenWit 			DECIMAL(18, 2)
DECLARE @PR_InkomenGroen		DECIMAL(18, 2)
DECLARE @PR_Basis_Dagen			INT
DECLARE @PR_Geboortedatum		DATE
DECLARE @PR_AHKInd				INT
DECLARE @PR_Loontijdvak			INT
DECLARE @PR_Woonlandbeginsel	INT


DECLARE @K7_IncomeWhite			DECIMAL(18, 2)
DECLARE @K7_IncomeGreen			DECIMAL(18, 2)
DECLARE @K7_Basis_Dagen			INT
DECLARE @K7_Birthday			DATE
DECLARE @K7_GenTaxDiscount		INT
DECLARE @K7_LTV_Code			INT
DECLARE @K7_CountryCode			INT
DECLARE @K7_CumulatiefAlgemeneHeffingskorting	DECIMAL(18, 2)
DECLARE @K7_CumulatiefArbeidskorting			DECIMAL(18, 2)
 
--------------------------------------------------------------------------------------------- 
--
-- HERE IS WHERE ONE SETS THE PARAMETERS FOR TESTING OF THE TWO APPLICATIONS AGAINST EACHOTHER
--
--

SET @PR_InkomenWit			= 10					-- Inkomen Wit
SET @PR_InkomenGroen		= 300					-- Inkomen Groen
SET @PR_Basis_Dagen			= 3						-- Basis Dagen
SET @PR_Geboortedatum		= '19590323'			-- Geboorte datum
SET @PR_AHKInd				= 0						-- Algemeneheffingskortingindicator 1 = Ja, 0 = Nee
SET @PR_Loontijdvak			= 1						-- Loontijdvak 1 = Dag, 2 = Week, 3 = Vierweken, 4 = Maand, 5 = Kwartaal
SET @PR_Woonlandbeginsel	=1			-- 1 = Nederland, 2 = Belgie, 3 = Landenkring, 4 = Derde Landen, 5 = SurinameAruba

-- After setting the Prechart settings the following part is to translate this to the settings in Klout7 for comparison

SET @K7_IncomeWhite = @PR_InkomenWit
SET @K7_IncomeGreen	= @PR_InkomenGroen
SET @K7_Basis_Dagen	= @PR_Basis_Dagen
SET @K7_Birthday = @PR_Geboortedatum	
SET @K7_GenTaxDiscount = @PR_AHKInd
SET @K7_CumulatiefAlgemeneHeffingskorting = 0
SET @K7_CumulatiefArbeidskorting = 0


if @PR_Loontijdvak = 2 
begin set @K7_LTV_Code = 3 
end
else if @PR_Loontijdvak = 4 
     begin set @K7_LTV_Code = 2
	 end
	 else begin set  @K7_LTV_Code = 1
	 end

if @PR_Woonlandbeginsel in (1,2,5) 
begin 
SET @K7_CountryCode = @PR_Woonlandbeginsel - 1
end
else
begin
SET @K7_CountryCode = @PR_Woonlandbeginsel - 2
end

SET @K7_CumulatiefAlgemeneHeffingskorting = 0
SET @K7_CumulatiefArbeidskorting = 0

-- Starts the testing

use prechart

If @PR_InkomenWit > 0 
begin 
select top 1 * from white
where Tabellon < @PR_InkomenWit
and year = 2022
and typeid = @PR_Loontijdvak
and countryid = @PR_Woonlandbeginsel
order by tabellon desc
end
else 
begin
if @PR_InkomenGroen > 0
select top 1 * from green
where Tabellon < @PR_InkomenGroen
and year = 2022
and typeid = @PR_Loontijdvak
and countryid = @PR_Woonlandbeginsel
order by tabellon desc
end

-- The Prechart calculations

select * from BerekenInhouding(
@PR_InkomenWit,				-- InkomenWit
@PR_InkomenGroen,			-- InkomenGroen
@PR_Basis_Dagen,			-- BasisDagen
@PR_Geboortedatum,			-- Geboortedatum
@PR_AHKInd,					-- AHKInd 1 = Ja, 0 is Nee
@PR_Loontijdvak,			-- Loontijdvak TypeId 1 = dag, 2 = week, 3 = vierweken, 4 = maand, 5 = kwartaal
@PR_Woonlandbeginsel)		-- Woonlandbeginsel 1 = Nederland, 2 = Belgie, 3 = Landenkring, 4 = Andere Landen, 5 = Suriname en Aruba

use ActureProd_HotFix

select * from GetTaxpayment_2022(
@K7_IncomeWhite	,			-- IncomeWhite
@K7_IncomeGreen,			-- IncomeGreen
@K7_Basis_Dagen,			-- BasisDagen
@K7_Birthday,				-- Birthday
@K7_GenTaxDiscount,			-- Algemeneheffingskorting
@K7_LTV_Code,				-- LTV 2 = Month, 3 Week, the rest is Day
@K7_CountryCode,			-- CountryCode 0 = Netherlands, 1 = Belgium, 2 = Landenkring, 3 = Other countries, 4 = SurinameAruba
0,
0)

Use Prechart

select * from BerekenInhouding_Test(
@PR_InkomenWit,				-- InkomenWit
@PR_InkomenGroen,			-- InkomenGroen
@PR_Basis_Dagen,			-- BasisDagen
@PR_Geboortedatum,			-- Geboortedatum
@PR_AHKInd,					-- AHKInd 1 = Ja, 0 is Nee
@PR_Loontijdvak,			-- Loontijdvak TypeId 1 = dag, 2 = week, 3 = vierweken, 4 = maand, 5 = kwartaal
@PR_Woonlandbeginsel)		-- Woonlandbeginsel 1 = Nederland, 2 = Belgie, 3 = Landenkring, 4 = Andere Landen, 5 = Suriname en Aruba

use ActureProd_HotFix

select * from GetTaxpayment_2022_Test(
@K7_IncomeWhite	,			-- IncomeWhite
@K7_IncomeGreen,			-- IncomeGreen
@K7_Basis_Dagen,			-- BasisDagen
@K7_Birthday,				-- Birthday
@K7_GenTaxDiscount,			-- Algemeneheffingskorting
@K7_LTV_Code,				-- LTV 2 = Month, 3 Week, the rest is Day
@K7_CountryCode,			-- CountryCode 0 = Netherlands, 1 = Belgium, 2 = Landenkring, 3 = Other countries, 4 = SurinameAruba
0,
0)
