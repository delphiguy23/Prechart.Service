using System.Text.RegularExpressions;

namespace Prechart.Service.Globals.Helper;

public static class StringHelper
{
    public static string RemoveSpecialCharacters(string text)
    {
        return Regex.Replace(text ?? string.Empty, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
    }

    public static string FirstCharacterUpperCase(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var result = text.ToArray();
        result[0] = char.ToUpper(result[0]);

        return new string(result);
    }

    public static bool ContainsNewLineCharacter(this string value)
    {
        foreach (char character in value)
        {
            if ((character == (char)13) || (character == (char)10))
            {
                return true;
            }
        }
        return false;
    }
}
