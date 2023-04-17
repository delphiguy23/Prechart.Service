using System.Collections.Generic;
using System.Security.Claims;

namespace Prechart.Service.Core.Authorization;

public interface ITokenHelper
{
    string GenerateToken(IList<Claim> claims, int expiresIn);
    bool TryParseToken(string token, out IEnumerable<Claim> claims);
}