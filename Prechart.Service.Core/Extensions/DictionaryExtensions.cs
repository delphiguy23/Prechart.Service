using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Prechart.Service.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static T ExtractKeyParameter<T>(this Dictionary<string, string> parameters, string key)
        {
            var value = parameters.FirstOrDefault(o => string.Equals(o.Key, key, StringComparison.OrdinalIgnoreCase)).Value;
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
