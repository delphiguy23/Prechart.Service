using System;
using System.Text;

namespace Prechart.Service.Core.TestBase
{
    public static class TestExtensions
    {
        public static string WithoutNewLine(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.Replace("\n", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace("\r", string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim(new char[] { '\uFEFF', '\u200B' });
        }

        public static string ToUtf8String(this byte[] input) => Encoding.UTF8.GetString(input);
    }
}
