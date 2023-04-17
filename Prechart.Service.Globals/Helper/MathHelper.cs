namespace Prechart.Service.Globals.Helper;

public static class MathHelper
{
    public static decimal Round(this decimal value, int decimals)
    {
        return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
    }
}
