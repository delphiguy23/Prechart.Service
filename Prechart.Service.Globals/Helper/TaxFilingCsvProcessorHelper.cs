using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Prechart.Service.Globals.Interfaces.Belastingen;
using Prechart.Service.Globals.Models.Belastingen;

namespace Prechart.Service.Globals.Helper;

public static class TaxFilingCsvProcessorHelper
{
    public static List<ITaxRecord> TaxFilingCsvProcessor(MemoryStream stream, int countryId, int TypeId, int year, string taxType, string delimiter = ";")
    {
        var CsvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = delimiter,
        };

        var cultureInfo = new CultureInfo("nl-NL");
        var records = new List<ITaxRecord>();

        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CsvConfig);
        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            var record = new TaxTableModel
            {
                Year = year,
                CountryId = countryId,
                TypeId = TypeId,
                Tabelloon = Convert.ToDecimal(csv.GetField("Tabelloon"), cultureInfo),
                ZonderLoonheffingskorting =
                    Convert.ToDecimal(
                        csv.GetField<string>("Jonger dan AOW-leeftijd zonder loonheffingskorting"),
                        cultureInfo),
                MetLoonheffingskorting =
                    Convert.ToDecimal(csv.GetField<string>("Jonger dan AOW-leeftijd met loonheffingskorting"),
                        cultureInfo),
                VerrekendeArbeidskorting = taxType == "groen"
                    ? 0.0M
                    : Convert.ToDecimal(
                        csv.GetField<string>("Jonger dan AOW-leeftijd verrekende arbeidskorting"), cultureInfo),
                EerderZonderLoonheffingskorting = Convert.ToDecimal(
                    csv.GetField<string>(
                        "AOW-leeftijd en ouder en geboren in 1945 of eerder zonder loonheffingskorting"),
                    cultureInfo),
                EerderMetLoonheffingskorting = Convert.ToDecimal(
                    csv.GetField<string>(
                        "AOW-leeftijd en ouder en geboren in 1945 of eerder met loonheffingskorting"),
                    cultureInfo),
                EerderVerrekendeArbeidskorting = taxType == "groen"
                    ? 0.0M
                    : Convert.ToDecimal(
                        csv.GetField<string>(
                            "AOW-leeftijd en ouder en geboren in 1945 of eerder verrekende arbeidskorting"),
                        cultureInfo),
                LaterZonderLoonheffingskorting = Convert.ToDecimal(
                    csv.GetField<string>(
                        "AOW-leeftijd en ouder en geboren in 1946 of later zonder loonheffingskorting"),
                    cultureInfo),
                LaterMetLoonheffingskorting = Convert.ToDecimal(
                    csv.GetField<string>(
                        "AOW-leeftijd en ouder en geboren in 1946 of later met loonheffingskorting"),
                    cultureInfo),
                LaterVerrekendeArbeidskorting = taxType == "groen"
                    ? 0.0M
                    : Convert.ToDecimal(
                        csv.GetField<string>(
                            "AOW-leeftijd en ouder en geboren in 1946 of later verrekende arbeidskorting"),
                        cultureInfo),
                Active = true,
            };

            records.Add(record);
        }

        return records.OrderBy(t => t.Tabelloon).ToList();
    }

    public static bool CsvFileCheck(string[] filename)
    {
        if (!filename.Any() || filename.Count() != 6 || string.IsNullOrWhiteSpace(filename[0]) || string.IsNullOrWhiteSpace(filename[1]) || string.IsNullOrWhiteSpace(filename[2]))
        {
            return false;
        }

        var taxType = filename[0].ToUpper();

        return new[] {TaxTypeEnum.Groen.ToString().ToUpper(), TaxTypeEnum.Wit.ToString().ToUpper()}.Contains(taxType);
    }

    public static bool IsCsvValid(Stream stream)
    {
        var badCsvRecords = new List<string>();
        var CsvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            BadDataFound = context => badCsvRecords.Add(context.RawRecord),
        };

        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CsvConfig);
        csv.Read();
        csv.ReadHeader();

        if (badCsvRecords.Any())
        {
            return false;
        }

        return true;
    }

    public static Stream ValidateRemoveHeader(Stream csv, string getFirstString)
    {
        var streamWriter = new StreamReader(csv, Encoding.UTF8);
        var text = streamWriter.ReadToEnd();
        var result = string.Empty;

        if (text.Substring(0, 9) != getFirstString)
        {
            result = text.Split(Environment.NewLine.ToCharArray(), 2).Skip(1).FirstOrDefault().Replace("\n", "");
        }
        else
        {
            result = text;
        }

        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(result);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
