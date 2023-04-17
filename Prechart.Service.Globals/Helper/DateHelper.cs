namespace Prechart.Service.Globals.Helper;

public static class DateHelper
{
    public static string ToTaxPeriode(this DateTime date)
    {
        return date.ToString("yyyy-MM");
    }

    public static DateTime FromTaxPeriodeToDate(this string periode, int day = 1)
    {
        return new DateTime(int.Parse(periode.Substring(0, 4)), int.Parse(periode.Substring(5, 2)), day);
    }

    public static (DateTime startReference, DateTime endReference) ToReferencePeriod(this DateTime date)
    {
        var currentDate = date.AddMonths(-1);
        var endReference = new DateTime(currentDate.Year, currentDate.Month, currentDate.DaysInMonth());
        var startReference = new DateTime(endReference.Year - 1, endReference.Month, 1);
        return (startReference, endReference);
    }

    public static int DaysInMonth(this DateTime date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    public static List<string> ToPeriodList(this (DateTime startReference, DateTime endReference) referenceDates)
    {
        var periodList = new List<string>();
        var currentDate = referenceDates.startReference;

        while (currentDate <= referenceDates.endReference)
        {
            periodList.Add(currentDate.ToTaxPeriode());
            currentDate = currentDate.AddMonths(1);
        }

        return periodList;
    }
}
