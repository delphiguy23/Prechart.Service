using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Prechart.Service.Core.Authorization
{
    public static class ClaimsExtensions
    {
        public static int? GetUserId(this IEnumerable<Claim> claims)
        {
            if (int.TryParse(claims.FirstOrDefault(c => c.Type == "http://prechart.com/userid")?.Value ?? string.Empty, out var result))
            {
                return result;
            }

            return null;
        }

        public static void SetUserId(this IList<Claim> claims, string userId) => claims.Add(new Claim("http://prechart.com/userid", userId));

        public static IEnumerable<string> GetRoles(this IEnumerable<Claim> claims) => claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        public static void SetRoles(this IList<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        public static IEnumerable<AreaUserRight> GetUserRights(this IEnumerable<Claim> claims) => claims.Where(c => c.Type == "http://prechart.com/userrights")
                .Select(r => r.Value.Split('_')).Where(a => a.Length == 2)
                .Select(c => new AreaUserRight
                {
                    Area = c[0].ToLowerInvariant(),
                    Rights = GetRights(c[1]),
                });

        public static void SetUserRights(this IList<Claim> claims, IEnumerable<string> userRights)
        {
            foreach (var userRight in userRights)
            {
                claims.Add(new Claim("http://prechart.com/userrights", userRight));
            }
        }

        public static void SetUserRights(this IList<Claim> claims, IEnumerable<AreaUserRight> userRights)
        {
            foreach (var userRight in userRights)
            {
                claims.Add(new Claim("http://prechart.com/userrights", userRight.ToString()));
            }
        }

        public static IEnumerable<string> GetAccessRoles(this IEnumerable<Claim> claims) => claims.Where(c => c.Type == "http://prechart.com/accessroles")
                .Select(r => r.Value);

        public static void SetAccessRoles(this IList<Claim> claims, IEnumerable<string> accessRoles)
        {
            foreach (var accessRole in accessRoles)
            {
                claims.Add(new Claim("http://prechart.com/accessroles", accessRole));
            }
        }

        public static IEnumerable<string> GetServiceRights(this IEnumerable<Claim> claims) => claims.Where(c => c.Type == "http://prechart.com/servicerights").Select(r => r.Value);

        public static void SetServiceRights(this IList<Claim> claims, IEnumerable<string> serviceRights)
        {
            foreach (var serviceRight in serviceRights)
            {
                claims.Add(new Claim("http://prechart.com/servicerights", serviceRight));
            }
        }

        public static bool IsUser(this IEnumerable<Claim> claims) => claims.Any(c => c.Type == "http://prechart.com/usertype" && c.Value == "user");

        public static void SetUserType(this IList<Claim> claims, string type) => claims.Add(new Claim("http://prechart.com/usertype", type));

        public static bool IsService(this IEnumerable<Claim> claims) => claims.Any(c => c.Type == "http://prechart.com/usertype" && c.Value == "service");

        private static IEnumerable<UserRight> GetRights(string rights)
        {
            if (rights.Contains('1', StringComparison.InvariantCultureIgnoreCase))
            {
                yield return UserRight.Read;
            }

            if (rights.Contains('2', StringComparison.InvariantCultureIgnoreCase))
            {
                yield return UserRight.Update;
            }

            if (rights.Contains('3', StringComparison.InvariantCultureIgnoreCase))
            {
                yield return UserRight.Create;
            }

            if (rights.Contains('4', StringComparison.InvariantCultureIgnoreCase))
            {
                yield return UserRight.Delete;
            }
        }

        public static bool HasRole(this IEnumerable<Claim> claims, string role) => claims.GetRoles().Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));

        public static bool HasAccessRole(this IEnumerable<Claim> claims, string role) => claims.GetAccessRoles().Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));

        public static bool HasServiceRight(this IEnumerable<Claim> claims, string right) => claims.GetServiceRights().Any(r => r.Equals(right, StringComparison.OrdinalIgnoreCase));

        public static bool HasUserRight(this IEnumerable<Claim> claims, string area, UserRight right) => claims.GetUserRights().HasUserRight(area, right);

        public static bool HasUserRight(this IEnumerable<AreaUserRight> areaUserRights, string area, UserRight right) => areaUserRights.Any(a => a.Area.Equals(area, StringComparison.OrdinalIgnoreCase) && a.Rights.Contains(right));

        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePair(this IEnumerable<Claim> claims) => claims?.Select(c => new KeyValuePair<string, string>(c.Type, c.Value)) ?? Array.Empty<KeyValuePair<string, string>>();

        public static IEnumerable<Claim> ToClaims(this IEnumerable<KeyValuePair<string, string>> keyValuePairs) => keyValuePairs?.Select(c => new Claim(c.Key, c.Value)) ?? Array.Empty<Claim>();
    }
}
