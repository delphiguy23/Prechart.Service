using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Globals.Helper;

public static class CumulativeExtension
{
    public static PersonTaxCumulative GetCumulative<T>(this List<PersonTaxCumulative> request)
    {
        if (request is null || !request.Any())
        {
            return new PersonTaxCumulative();
        }

        var taxNumbers = request.Select(r => r.TaxNo).Distinct().ToList();
        var taxPeriodes = request.Select(r => r.TaxPeriode).Distinct().ToList();

        var forCumulatieve = new List<PersonTaxCumulative>();

        foreach (var taxNumber in taxNumbers)
        {
            foreach (var taxPeriode in taxPeriodes)
            {
                forCumulatieve.Add(request
                    .Where(r => r.TaxNo == taxNumber)
                    .Where(r => r.TaxPeriode == taxPeriode)
                    .OrderByDescending(r => r.TaxFileProcessDate)
                    .FirstOrDefault());
            }
        }

        var cumulative = new PersonTaxCumulative
        {
            SofiNr = forCumulatieve.FirstOrDefault()?.SofiNr ?? string.Empty,
            PersonNr = string.Join(",", forCumulatieve.Select(a => a.PersonNr).Distinct().OrderBy(a => a).ToList()),
            NumIv = string.Join(",", forCumulatieve.Select(a => a.NumIv).Distinct().OrderBy(a => a).ToList()),
            TaxNo = string.Join(",", forCumulatieve.Select(a => a.TaxNo).Distinct().OrderBy(a => a).ToList()),
            TaxFileProcessDate = DateTime.Now,
            TaxPeriode = string.Join(",", forCumulatieve.Select(a => a.TaxPeriode).Distinct().OrderBy(a => a).ToList()),
            LnLbPh = forCumulatieve.Sum(r => r.LnLbPh),
            LnSv = forCumulatieve.Sum(r => r.LnSv),
            PrlnAofAnwLg = forCumulatieve.Sum(r => r.PrlnAofAnwLg),
            PrlnAofAnwHg = forCumulatieve.Sum(r => r.PrlnAofAnwHg),
            PrlnAofAnwUit = forCumulatieve.Sum(r => r.PrlnAofAnwUit),
            PrlnAwfAnwLg = forCumulatieve.Sum(r => r.PrlnAwfAnwLg),
            PrlnAwfAnwHg = forCumulatieve.Sum(r => r.PrlnAwfAnwHg),
            PrlnAwfAnwHz = forCumulatieve.Sum(r => r.PrlnAwfAnwHz),
            PrLnUfo = forCumulatieve.Sum(r => r.PrLnUfo),
            LnTabBb = forCumulatieve.Sum(r => r.LnTabBb),
            VakBsl = forCumulatieve.Sum(r => r.VakBsl),
            OpgRchtVakBsl = forCumulatieve.Sum(r => r.OpgRchtVakBsl),
            OpnAvwb = forCumulatieve.Sum(r => r.OpnAvwb),
            OpbAvwb = forCumulatieve.Sum(r => r.OpbAvwb),
            LnInGld = forCumulatieve.Sum(r => r.LnInGld),
            WrdLn = forCumulatieve.Sum(r => r.WrdLn),
            LnOwrk = forCumulatieve.Sum(r => r.LnOwrk),
            VerstrAanv = forCumulatieve.Sum(r => r.VerstrAanv),
            IngLbPh = forCumulatieve.Sum(r => r.IngLbPh),
            PrAofLg = forCumulatieve.Sum(r => r.PrAofLg),
            PrAofHg = forCumulatieve.Sum(r => r.PrAofHg),
            PrAofUit = forCumulatieve.Sum(r => r.PrAofUit),
            OpslWko = forCumulatieve.Sum(r => r.OpslWko),
            PrGediffWhk = forCumulatieve.Sum(r => r.PrGediffWhk),
            PrAwfLg = forCumulatieve.Sum(r => r.PrAwfLg),
            PrAwfHg = forCumulatieve.Sum(r => r.PrAwfHg),
            PrAwfHz = forCumulatieve.Sum(r => r.PrAwfHz),
            PrUfo = forCumulatieve.Sum(r => r.PrUfo),
            BijdrZvw = forCumulatieve.Sum(r => r.BijdrZvw),
            WghZvw = forCumulatieve.Sum(r => r.WghZvw),
            WrdPrGebrAut = forCumulatieve.Sum(r => r.WrdPrGebrAut),
            WrknBijdrAut = forCumulatieve.Sum(r => r.WrknBijdrAut),
            Reisk = forCumulatieve.Sum(r => r.Reisk),
            VerrArbKrt = forCumulatieve.Sum(r => r.VerrArbKrt),
            AantVerlU = forCumulatieve.Sum(r => r.AantVerlU),
            Ctrctln = forCumulatieve.Sum(r => r.Ctrctln),
            AantCtrcturenPWk = forCumulatieve.Sum(r => r.AantCtrcturenPWk),
            BedrRntKstvPersl = forCumulatieve.Sum(r => r.BedrRntKstvPersl),
            BedrAlInWwb = forCumulatieve.Sum(r => r.BedrAlInWwb),
            BedrRchtAl = forCumulatieve.Sum(r => r.BedrRchtAl),
        };

        return cumulative;
    }
}
