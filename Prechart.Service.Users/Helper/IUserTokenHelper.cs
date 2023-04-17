using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Users.Models;

namespace Prechart.Service.Users.Helper;

public interface IUserTokenHelper :
    IHandlerAsync<UserTokenHelper.CreateTokens, IFluentResults<Tokens>>,
    IHandlerAsync<UserTokenHelper.GenerateKey, IFluentResults<string>>,
    IHandlerAsync<UserTokenHelper.CreateTemporaryTokens, IFluentResults<Tokens>>
{
}
