using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Prechart.Service.Core.Extensions;

public static class RequestExtension
{
    public static string BodyToString(this HttpRequest request)
    {
        var returnValue = string.Empty;

        try
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            using (var stream = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                returnValue = stream.ReadToEnd();
            }

            request.Body.Position = 0;
            return Regex.Unescape(returnValue);
        }
        catch (Exception)
        {
            return returnValue;
        }
    }

    public static object BodyToObject(this HttpRequest request)
    {
        try
        {
            var bodyString = request.BodyToString();
            return string.IsNullOrWhiteSpace(bodyString) ? null : JsonConvert.DeserializeObject<object>(Regex.Unescape(bodyString));
        }
        catch (Exception)
        {
            return null;
        }
    }
}
