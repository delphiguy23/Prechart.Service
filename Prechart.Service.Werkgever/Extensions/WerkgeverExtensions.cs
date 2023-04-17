using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Werkgever.Models;

namespace Prechart.Service.Werkgever.Extensions;

public static class WerkgeverExtensions
{
    public static List<TaxGroupedCollectieveAangifteModel> GroupByTaxNumber(this List<CollectieveAangifteModel> collectieve)
    {
        if (collectieve is null)
        {
            return new List<TaxGroupedCollectieveAangifteModel>();
        }

        var taxNumbers = collectieve.Select(t => t.TaxNo).Distinct().ToList();
        var results = new List<TaxGroupedCollectieveAangifteModel>();

        foreach (var t in taxNumbers)
        {
            var collectieves = collectieve
                .Where(c => c.TaxNo == t)
                .Select(s => new ForGrouopingCollectieveAangifteModel
                {
                    Periode = s.Periode,
                    ProcessedDate = s.ProcessedDate,
                    CollectieveType = s.CollectieveType,
                    TotLnLbPh = s.TotLnLbPh,
                    TotLnSv = s.TotLnSv,
                    TotPrlnAofAnwLg = s.TotPrlnAofAnwLg,
                    TotPrlnAofAnwHg = s.TotPrlnAofAnwHg,
                    TotPrlnAofAnwUit = s.TotPrlnAofAnwUit,
                    TotPrlnAwfAnwLg = s.TotPrlnAwfAnwLg,
                    TotPrlnAwfAnwHg = s.TotPrlnAwfAnwHg,
                    TotPrlnAwfAnwHz = s.TotPrlnAwfAnwHz,
                    PrLnUfo = s.PrLnUfo,
                    IngLbPh = s.IngLbPh,
                    EhPubUitk = s.EhPubUitk,
                    EhGebrAuto = s.EhGebrAuto,
                    EhVut = s.EhVut,
                    EhOvsFrfWrkkstrg = s.EhOvsFrfWrkkstrg,
                    AvZeev = s.AvZeev,
                    VrlAvso = s.VrlAvso,
                    TotPrAofLg = s.TotPrAofLg,
                    TotPrAofHg = s.TotPrAofHg,
                    TotPrAofUit = s.TotPrAofUit,
                    TotOpslWko = s.TotOpslWko,
                    TotPrGediffWhk = s.TotPrGediffWhk,
                    TotPrAwfLg = s.TotPrAwfLg,
                    TotPrAwfHg = s.TotPrAwfHg,
                    TotPrAwfHz = s.TotPrAwfHz,
                    PrUfo = s.PrUfo,
                    IngBijdrZvw = s.IngBijdrZvw,
                    TotWghZvw = s.TotWghZvw,
                    TotTeBet = s.TotTeBet,
                    TotGen = s.TotGen,
                    TotSaldo = s.TotSaldo,
                    SaldoCorrectiesVoorgaandTijdvak = s.SaldoCorrectiesVoorgaandTijdvak,
                }).ToList();

            results.Add(new TaxGroupedCollectieveAangifteModel
            {
                TaxNo = t,
                CollectieveAangifte = collectieves,
            });
        }

        return results;
    }
}
