using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Users.Database.Models;
using Prechart.Service.Users.Models;

namespace Prechart.Service.Users.Services;

public interface IUsersService :
    IHandlerAsync<UsersService.RegisterUser, IFluentResults<IdentityResult>>,
    IHandlerAsync<UsersService.IsUserExists, IFluentResults<ServiceUsers>>,
    IHandlerAsync<UsersService.IsPasswordOk, IFluentResults<bool>>,
    IHandlerAsync<UsersService.AddRole, IFluentResults<bool>>,
    IHandlerAsync<UsersService.DeleteRole, IFluentResults<bool>>,
    IHandlerAsync<UsersService.UpdateRole, IFluentResults<bool>>,
    IHandlerAsync<UsersService.GetRoles, IFluentResults<List<string>>>,
    IHandlerAsync<UsersService.GetUserRoles, IFluentResults<List<string>>>,
    IHandlerAsync<UsersService.ValidateUser, IFluentResults<Tokens>>,
    IHandlerAsync<UsersService.GetUsers, IFluentResults<List<GetUsersModel>>>,
    IHandlerAsync<UsersService.DeleteUser, IFluentResults<bool>>,
    IHandlerAsync<UsersService.RemoveUserRole, IFluentResults<bool>>,
    IHandlerAsync<UsersService.AddUserRole, IFluentResults<bool>>,
    IHandlerAsync<UsersService.RefreshTokens, IFluentResults<Tokens>>
{
}
