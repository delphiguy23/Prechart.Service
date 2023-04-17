namespace Prechart.Service.Core.Extensions;

public static class PercentageExtension
{
    public static decimal ConvertToRawPercentage(this decimal percentage)
    {
        var value = 0M;
        if (percentage is not 0)
        {
            value = percentage / 100;
        }

        return value;
    }

    public static decimal ConvertToPercentage(this decimal rawPercentage)
    {
        var value = 0M;
        if (rawPercentage is not 0)
        {
            value = rawPercentage * 100;
        }

        return value;
    }
}
