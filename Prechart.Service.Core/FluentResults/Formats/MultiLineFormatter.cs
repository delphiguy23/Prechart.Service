using System.Collections.Generic;
using System.Text;

namespace Prechart.Service.Core.FluentResults.Formats;

public static class MultiLineFormatter
{
    public static string ToMultiLine(string delimiter, List<string> messages)
    {
        var result = new StringBuilder();

        foreach (var message in messages)
        {
            if (delimiter == null)
            {
                HandleNullDelimiter(result, message);
            }
            else
            {
                result.AppendFormat("{0}{1}", message, delimiter);
            }
        }

        return result.ToString();
    }

    private static void HandleNullDelimiter(StringBuilder builder, string nextMessage)
    {
        //If builder is empty, there's nothing to do.
        if (builder.Length == 0)
        {
            builder.Append(nextMessage);
            return;
        }

        if (builder[builder.Length - 1] == ' ' || nextMessage.StartsWith(" "))
        {
            builder.Append(nextMessage);
            return;
        }

        builder.Append(" ");
        builder.Append(nextMessage);
    }
}