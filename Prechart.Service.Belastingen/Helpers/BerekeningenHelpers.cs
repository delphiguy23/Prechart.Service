using System;
using System.Linq;
using MassTransit;
using Prechart.Service.Belastingen.Models.Berekeningen;
using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Belastingen.Services.Berekeningen;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Globals.Interfaces.PremieBedrag;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models.Belastingen;

namespace Prechart.Service.Belastingen.Helpers;

public static class BerekeningenHelpers
{
    public static BerekeningenModel ApplyWhkWerkgeverDataToBerekiningen(BerekeningenModel berekeningen, Response<IMongoWerkgevers> whkWerkgever, DateTime processDate)
    {
        var werkgever = whkWerkgever?.Message.Werkgevers.FirstOrDefault();
        var whk = werkgever?.WhkPremies.FirstOrDefault( w=> processDate >= w.ActiefVanaf && processDate <= w.ActiefTot && w.Actief);
        
        if (whk == null)
        {
            return berekeningen;
        }
        
        berekeningen.WerkgeverWHKPremieBedragWGAVastWerkgever = Math.Round(berekeningen.SociaalVerzekeringsloon * whk.WgaVastWerkgever.ConvertToRawPercentage(), 2);
        berekeningen.WerkgeverWHKPremieBedragWGAVastWerknemer = Math.Round(berekeningen.SociaalVerzekeringsloon * whk.WgaVastWerknemer.ConvertToRawPercentage(), 2);
        berekeningen.WerkgeverWHKPremieBedragFlexWerkgever = Math.Round(berekeningen.SociaalVerzekeringsloon * whk.FlexWerkgever.ConvertToRawPercentage(), 2);
        berekeningen.WerkgeverWHKPremieBedragFlexWerknemer = Math.Round(berekeningen.SociaalVerzekeringsloon *whk.FlexWerknemer.ConvertToRawPercentage(), 2);
        berekeningen.WerkgeverWHKPremieBedragZWFlex = Math.Round(berekeningen.SociaalVerzekeringsloon * whk.ZwFlex.ConvertToRawPercentage(), 2);
        berekeningen.WerkgeverWHKPremieBedragTotaal = Math.Round(berekeningen.SociaalVerzekeringsloon * whk.Totaal.ConvertToRawPercentage(), 2);

        berekeningen.NettoTeBetalenEindTotaal = Math.Round(berekeningen.NettoTeBetalenSubTotaal -
                                                           (
                                                               berekeningen.WerkgeverWHKPremieBedragFlexWerknemer +
                                                               berekeningen.WerkgeverWHKPremieBedragWGAVastWerknemer +
                                                               berekeningen.PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage
                                                           ), 2);
        berekeningen.Werkgever = werkgever;
        return berekeningen;
    }
    
    public static BerekeningenModel ApplyPremieBedragDataToBerekiningen(BerekeningenService.CalculateBerekenen request, BerekeningenModel berekeningen,
        Response<IPremieBedrag> premieBedrag)
    {
        berekeningen.PremieBedragAlgemeenWerkloosheIdsFondsLaag =
            berekeningen.PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog == HighLowEnum.Laag
                ? premieBedrag.Message.PremieBedragAlgemeenWerkloosheidsFondsLaag
                : 0m;
        berekeningen.PremieBedragAlgemeenWerkloosheIdsFondsHoog =
            berekeningen.PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog == HighLowEnum.Hoog
                ? premieBedrag.Message.PremieBedragAlgemeenWerkloosheidsFondsHoog
                : 0m;

        berekeningen.PremieBedragUitvoeringsFondsvoordeOverheId =
            berekeningen.IsPremieBedragUitvoeringsFondsvoordeOverheId
                ? premieBedrag.Message.PremieBedragUitvoeringsFondsvoordeOverheid
                : 0m;

        berekeningen.PremieBedragWetArbeIdsOngeschikheIdLaag =
            request.Parameters.IsPremieBedragUitvoeringsFondsvoordeOverheId
                ? 0m
                : (request.Parameters.PremieBedragWetArbeIdsOngeschikheIdLaagHoog == HighLowEnum.Laag
                    ? premieBedrag.Message.PremieBedragWetArbeidsOngeschikheidLaag
                    : 0m);

        berekeningen.PremieBedragWetArbeIdsOngeschikheIdHoog =
            request.Parameters.IsPremieBedragUitvoeringsFondsvoordeOverheId
                ? 0m
                : (request.Parameters.PremieBedragWetArbeIdsOngeschikheIdLaagHoog == HighLowEnum.Hoog
                    ? premieBedrag.Message.PremieBedragWetArbeidsOngeschikheidLaag
                    : 0m);

        berekeningen.PremieBedragWetKinderopvang = premieBedrag.Message.PremieBedragWetKinderopvang;
        berekeningen.PremieBedragZiektekostenVerzekeringsWetLoon =
            premieBedrag.Message.PremieBedragZiektekostenVerzekeringsWetLoon;

        berekeningen.PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage = berekeningen.Payee == PayeeEnum.Werkgever
            ? premieBedrag.Message.PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage
            : 0m;
        berekeningen.PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage = berekeningen.Payee == PayeeEnum.Werknemer
            ? premieBedrag.Message.PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage
            : 0m;

        berekeningen.PremieBedrag = premieBedrag.Message;
        return berekeningen;
    }
    
    public static BerekeningenModel ApplyInhoudingDataToBerekeningen(BerekeningenModel berekeningen, Response<IBerekenenInhouding> inhouding)
    {
        berekeningen.InhoudingOpLoonWit = inhouding.Message.InhoudingWit;
        berekeningen.InhoudingOpLoonGroen = inhouding.Message.InhoudingGroen;
        berekeningen.AlgemeneHeffingskortingBedrag = inhouding.Message.AlgemeneHeffingsKorting;
        berekeningen.VerrekendeArbeIdskorting = inhouding.Message.ArbeidsKorting;
        berekeningen.NettoTeBetalenSubTotaal = inhouding.Message.NettoBetaling;

        berekeningen.Inhouding = inhouding.Message;
        return berekeningen;
    }

    public static BerekeningenModel InitBerekeningen(BerekeningenService.CalculateBerekenen request, BerekeningenModel berekeningen)
    {
        berekeningen.Id = 0;
        berekeningen.ProcessDatum = DateTime.Today;
        berekeningen.WoonlandbeginselId = request.Parameters.InhoudingParameters.WoondlandBeginselId;
        berekeningen.TijdvakId = (int) request.Parameters.InhoudingParameters.Loontijdvak;
        berekeningen.InkomenWit = request.Parameters.InhoudingParameters.InkomenWit;
        berekeningen.InkomenGroen = request.Parameters.InhoudingParameters.InkomenGroen;
        
        berekeningen.LoonOverVanaf = new DateTime(DateTime.Today.Year, 1, 1);
        berekeningen.LoonOverTot = new DateTime(DateTime.Today.Year, 12, 31);
        berekeningen.LoonInVanaf = new DateTime(DateTime.Today.Year, 1, 1);
        berekeningen.LoonInTot = new DateTime(DateTime.Today.Year, 12, 31);
        berekeningen.AlgemeneHeffingskortingToegepast =
            request.Parameters.InhoudingParameters.AlgemeneHeffingsKortingIndicator ? 1 : 0;
        berekeningen.BasisDagen = request.Parameters.InhoudingParameters.Loontijdvak switch
        {
            TaxPeriodEnum.Day => request.Parameters.InhoudingParameters.BasisDagen,
            TaxPeriodEnum.Week => 5m,
            TaxPeriodEnum.FourWeekly => 20m,
            TaxPeriodEnum.Month => 21.75m,
            TaxPeriodEnum.Quarter => 65m,
            _ => 0m,
        };

        berekeningen.SociaalVerzekeringsloon = request.Parameters.InhoudingParameters.InkomenWit;
        berekeningen.PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog =
            request.Parameters.PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog;
        berekeningen.IsPremieBedragUitvoeringsFondsvoordeOverheId =
            request.Parameters.IsPremieBedragUitvoeringsFondsvoordeOverheId;
        berekeningen.PremieBedragWetArbeIdsOngeschikheIdLaagHoog =
            request.Parameters.PremieBedragWetArbeIdsOngeschikheIdLaagHoog;
        berekeningen.Payee = request.Parameters.Payee;

        berekeningen.Deleted = false;
        berekeningen.Actief = true;

        return berekeningen;
    }

    public static GetInhoudingEventModel InitializeGetInhoudingEvent(BerekeningenService.CalculateBerekenen calculateBerekenen)
    {
        return new GetInhoudingEventModel
        {
            InkomenWit = calculateBerekenen.Parameters.InhoudingParameters.InkomenWit,
            InkomenGroen = calculateBerekenen.Parameters.InhoudingParameters.InkomenGroen,
            BasisDagen = calculateBerekenen.Parameters.InhoudingParameters.BasisDagen,
            Geboortedatum = calculateBerekenen.Parameters.InhoudingParameters.Geboortedatum,
            AlgemeneHeffingsKortingIndicator =
                calculateBerekenen.Parameters.InhoudingParameters.AlgemeneHeffingsKortingIndicator,
            Loontijdvak = calculateBerekenen.Parameters.InhoudingParameters.Loontijdvak,
            WoondlandBeginselId = calculateBerekenen.Parameters.InhoudingParameters.WoondlandBeginselId,
            ProcesDatum = calculateBerekenen.Parameters.InhoudingParameters.ProcesDatum,
        };
    }

    public static GetPremieBedragEventModel InitializegetGetPremieBedragEvent(BerekeningenService.CalculateBerekenen calculateBerekenen, DateTime? processdatum = null)
    {
        return new GetPremieBedragEventModel
        {
            LoonSocialVerzekeringen = calculateBerekenen.Parameters.PremieBedragParameters.LoonSocialVerzekeringen,
            LoonZiektekostenVerzekeringsWet = calculateBerekenen.Parameters.InhoudingParameters.InkomenWit +
                                              calculateBerekenen.Parameters.InhoudingParameters.InkomenGroen,
            SocialeVerzekeringenDatum = processdatum ?? DateTime.Today,
        };
    }

    public static GetWhkWerkgeverEventModel InitializeGetWhkWerkgeverEvent(BerekeningenService.CalculateBerekenen calculateBerekenen)
    {
        return new GetWhkWerkgeverEventModel
        {
            Taxno = calculateBerekenen.Parameters.WhkWerkgeverParameters.Taxno,
        };
    }
}
