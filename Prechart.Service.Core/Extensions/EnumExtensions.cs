using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace Prechart.Service.Core.Extensions;

public static class EnumExtensions
{
    public static string ToEnumDescription<T>(this int? id)
        where T : struct, Enum
    {
        if (!id.HasValue)
        {
            return string.Empty;
        }

        return ((T) (object) id.Value).ToEnumDescription();
    }

    public static string ToEnumDescription<T>(this string val)
        where T : struct, Enum
    {
        if (val == null)
        {
            return string.Empty;
        }

        if (!Enum.IsDefined(typeof(T), val))
        {
            return val;
        }

        return Enum.Parse<T>(val, true).ToEnumDescription();
    }

    public static string ToEnumDescription<T>(this T enumValue)
        where T : struct, Enum
    {
        var type = typeof(T);
        var enumName = type.GetEnumName(enumValue);
        if (!string.IsNullOrEmpty(enumName))
        {
            var memInfo = type.GetMember(enumName);
            var descriptionAttribute = memInfo[0]
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
        }

        return enumValue.ToString();
    }

    public static Dictionary<int, string> ToDictionary<T>()
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .ToDictionary(t => (int) (object) t, t => t.ToString());
    }

    public static string XmlEnumToString<TEnum>(this TEnum value) where TEnum : struct, IConvertible
    {
        var enumType = typeof(TEnum);

        if (!enumType.IsEnum)
        {
            return null;
        }

        var member = enumType.GetMember(value.ToString() ?? string.Empty).FirstOrDefault();

        if (member == null)
        {
            return null;
        }

        var attribute = member.GetCustomAttributes(false).OfType<XmlEnumAttribute>().FirstOrDefault();

        return attribute == null ? member.Name : attribute.Name; // Fallback to the member name when there's no attribute
    }
}
