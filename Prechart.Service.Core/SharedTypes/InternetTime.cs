using System;
using System.Globalization;
using System.Net;

namespace Prechart.Service.Core.SharedTypes;

public static class InternetTime
{
#pragma warning disable S1075 // URIs should not be hardcoded
        private const string TimeCheckUrl = "https://google.com";
#pragma warning restore S1075 // URIs should not be hardcoded
        private static System.DateTime checkDate = System.DateTime.Now.AddDays(-10);
        private static TimeSpan diff;
        private static object diffLocker = new object();
        private static bool isChecking;
        public static TimeSpan GetTimeOffset()
        {
            if (ShouldCheck())
            {
                lock (diffLocker)
                {
                    if (ShouldCheck())
                    {
                        isChecking = true;
                        try
                        {
#pragma warning disable SYSLIB0014
                            var request = WebRequest.CreateHttp(new Uri(TimeCheckUrl));
#pragma warning restore SYSLIB0014
                            request.Method = "GET";

                            using (var response = (HttpWebResponse)request.GetResponse())
                            {
                                var date = System.DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
                                var calcDiff = date - System.DateTime.Now;
                                diff = Math.Abs(calcDiff.TotalMinutes) > 1 ? calcDiff : default;
                            }
                        }
                        catch
                        {
                            // ignore
                        }

                        isChecking = false;
                        checkDate = System.DateTime.Now;
                    }
                }
            }

            return diff;
        }

        private static bool ShouldCheck()
        {
            return Math.Abs((System.DateTime.Now - checkDate).TotalDays) >= 1 && !isChecking;
        }
}