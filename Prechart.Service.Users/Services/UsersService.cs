using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prechart.Service.AuditLog.Events.Users;
using Prechart.Service.AuditLog.Models.Users;
using Prechart.Service.Core.Authorization;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Interfaces;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Users.Database.Models;
using Prechart.Service.Users.Helper;
using Prechart.Service.Users.Models;

namespace Prechart.Service.Users.Services;

public partial class UsersService : IUsersService
{
    private readonly IRequestClient<IPersonUserCredentials> _getPersonEvent;
    private readonly IRequestClient<IResetPersonUserCredentials> _getResetPersonEvent;
    private readonly IRequestClient<IUpsertPersonUserCredentials> _getUpsertPersonEvent;
    private readonly ILogger<UsersService> _logger;
    private readonly IPublishEndpoint _publishendpoint;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ServiceUsers> _signInManager;
    private readonly IOptions<TestConfiguration> _testConfiguration;
    private readonly ITokenHelper _tokenHelper;
    private readonly UserManager<ServiceUsers> _userManager;
    private readonly IUserTokenHelper _userTokenHelper;

    public UsersService(ILogger<UsersService> logger,
        ITokenHelper tokenHelper,
        IOptions<TestConfiguration> testConfiguration,
        IPublishEndpoint publishendpoint,
        UserManager<ServiceUsers> userManager,
        SignInManager<ServiceUsers> signInManager,
        RoleManager<IdentityRole> roleManager,
        IUserTokenHelper userTokenHelper,
        IRequestClient<IPersonUserCredentials> getPersonEvent,
        IRequestClient<IUpsertPersonUserCredentials> getUpsertPersonEvent,
        IRequestClient<IResetPersonUserCredentials> getResetPersonEvent
    )
    {
        _logger = logger;
        _tokenHelper = tokenHelper;
        _testConfiguration = testConfiguration;
        _publishendpoint = publishendpoint;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userTokenHelper = userTokenHelper;
        _getPersonEvent = getPersonEvent;
        _getUpsertPersonEvent = getUpsertPersonEvent;
        _getResetPersonEvent = getResetPersonEvent;
    }

    public async Task<IFluentResults<IdentityResult>> HandleAsync(RegisterUser request, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var role in request.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return ResultsTo.BadRequest<IdentityResult>("Role does not exist").WithMessage("Invalid argument provided.");
                }
            }

            var newUser = request.User;

            newUser.RefreshToken = (await _userTokenHelper.HandleAsync(new UserTokenHelper.GenerateKey(), cancellationToken))?.Value ?? string.Empty;
            newUser.ApiToken = (await _userTokenHelper.HandleAsync(new UserTokenHelper.GenerateKey(), cancellationToken))?.Value ?? string.Empty;

            request.User.Active = true;

            var result = await _userManager.CreateAsync(request.User, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(m => m.Description).ToList();
                errors.ForEach(m => _logger.LogError(m));
                return ResultsTo.Failure<IdentityResult>("Failed to create user").WithMessage(errors);
            }

            var user = await _userManager.FindByNameAsync(request.User.UserName);

            await _userManager.AddToRolesAsync(user, request.Roles);

            await PublishUserEvent(user, UserEventType.UserCreated);

            if (request.PersonId is not null && request.Roles.Contains(PersonType.Employee.ToString()))
            {
                var response = await _getUpsertPersonEvent.GetResponse<Status, NotFound>(new
                {
                    request.PersonId,
                    request.User.UserName,
                    EmailAddress = request.User.Email,
                });

                if ((!response.Is(out Response<Status> upsertPersonUserResponse)) || upsertPersonUserResponse.Message.Result == false)
                {
                    await HandleAsync(new DeleteUser {UserName = request.User.UserName}, cancellationToken);
                    return ResultsTo.Failure<IdentityResult>("Person does not exist");
                }

                _logger.LogInformation($"User: {user.UserName}, Event: UpsertPersonUserCredentials");

                if (!string.IsNullOrEmpty(request.User.Email))
                {
                    await _publishendpoint.Publish<IEmailEvent>(
                        new
                        {
                            To = new List<string> { request.User.Email },
                            Subject = "User Registration",
                            Body = "Your account is successfully registered.",
                            EmailEventType = Core.Models.EmailEventType.Normal
                        });
                }
            }

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            await HandleAsync(new DeleteUser {UserName = request.User.UserName}, cancellationToken);

            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<IdentityResult>().FromException(e);
        }
    }

    public async Task<IFluentResults<ServiceUsers>> HandleAsync(IsUserExists request, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.FindByNameAsync(request.UserName);

        if (result is null || result.Active == false)
        {
            return ResultsTo.NotFound<ServiceUsers>().WithMessage("User does not exist");
        }

        return ResultsTo.Success(result);
    }

    public async Task<IFluentResults<bool>> HandleAsync(IsPasswordOk request, CancellationToken cancellationToken = default)
    {
        return ResultsTo.Success(await _userManager.CheckPasswordAsync(request.User, request.Password));
    }

    public async Task<IFluentResults<Tokens>> HandleAsync(ValidateUser request, CancellationToken cancellationToken = default)
    {
        if (await HandleAsync(new IsUserExists {UserName = request.UserName}, cancellationToken) is { } user && user.Value is not null &&
            await HandleAsync(new IsPasswordOk {User = user.Value, Password = request.Password}, cancellationToken) is { } isOk && isOk.Value)
        {
            var tokens = await _userTokenHelper.HandleAsync(new UserTokenHelper.CreateTokens {User = user.Value}, cancellationToken);

            if (tokens.Status == FluentResultsStatus.Success)
            {
                await PublishUserEvent(user.Value, UserEventType.UserLogin);
            }

            return ResultsTo.Something(tokens);
        }

        var response = await _getPersonEvent.GetResponse<IPersonCredentialCheck, NotFound>(new
        {
            request.UserName,
            request.Password,
        });

        if (response.Is(out Response<IPersonCredentialCheck> personResponse))
        {
            var tokens = await _userTokenHelper.HandleAsync(new UserTokenHelper.CreateTemporaryTokens
            {
                PersonId = personResponse.Message.Id,
                UserName = request.UserName,
            }, cancellationToken);

            _logger.LogInformation("Generic user login check");

            return ResultsTo.Something(tokens);
        }

        return ResultsTo.NotFound<Tokens>();
    }

    public async Task<IFluentResults<bool>> HandleAsync(AddRole request, CancellationToken cancellationToken = default)
    {
        var result = await _roleManager.FindByNameAsync(request.RoleName);

        if (result is not null)
        {
            return ResultsTo.BadRequest<bool>("Role already exists").WithMessage("Invalid argument provided.");
        }

        return ResultsTo.Success((await _roleManager.CreateAsync(new IdentityRole {Name = request.RoleName})).Succeeded);
    }

    public async Task<IFluentResults<List<string>>> HandleAsync(GetRoles request, CancellationToken cancellationToken = default)
    {
        var result = await _roleManager.Roles.ToListAsync();

        if (result is null || !result.Any())
        {
            return ResultsTo.NotFound<List<string>>().WithMessage("No roles found");
        }

        return ResultsTo.Success(result.Select(r => r.Name).ToList());
    }

    public async Task<IFluentResults<List<string>>> HandleAsync(GetUserRoles request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.User))
            {
                return ResultsTo.BadRequest<List<string>>("User is null or empty").WithMessage("Invalid argument provided.");
            }

            var getUser = await _userManager.FindByNameAsync(request.User);

            var result = getUser == null ? null : await _userManager.GetRolesAsync(getUser) as List<string>;

            if (result is null || !result.Any())
            {
                return ResultsTo.NotFound<List<string>>("User does not have any roles").WithMessage("No roles found");
            }

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<List<string>>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<GetUsersModel>>> HandleAsync(GetUsers request, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.Users.ToListAsync();

        if (result is null || !result.Any())
        {
            return ResultsTo.NotFound<List<GetUsersModel>>("No users found").WithMessage("No users found");
        }

        var users = result.Select(u => new GetUsersModel
        {
            UserName = u.UserName,
            FirstName = u.FirstName,
            LastName = u.LastName,
            MiddleName = u.MiddleName,
        }).ToList();

        foreach (var user in users)
        {
            var roles = await HandleAsync(new GetUserRoles {User = user.UserName}, cancellationToken);

            if (roles is null || !roles.Value.Any())
            {
                continue;
            }

            user.Roles = roles.Value.ToArray();
        }

        return ResultsTo.Success(users);
    }

    public async Task<IFluentResults<bool>> HandleAsync(DeleteRole request, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role is null)
            {
                return ResultsTo.BadRequest<bool>("Role does not exist").WithMessage("Invalid argument provided.");
            }

            var getUsersRoles = await _userManager.GetUsersInRoleAsync(role.Name);

            if (getUsersRoles.Any(x => x.Active))
            {
                return ResultsTo.BadRequest<bool>("Role has active users").WithMessage("Invalid argument provided.");
            }

            var result = await _roleManager.DeleteAsync(role);

            return ResultsTo.Success(result.Succeeded);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateRole request, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(request.Id);

            if (role is null)
            {
                return ResultsTo.BadRequest<bool>("Role does not exist").WithMessage("Invalid argument provided.");
            }

            role.Name = request.RoleName;
            role.NormalizedName = request.RoleNormalizedName;

            var result = await _roleManager.UpdateAsync(role);

            return ResultsTo.Success(result.Succeeded);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }


    public async Task<IFluentResults<bool>> HandleAsync(DeleteUser request, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.FindByNameAsync(request.UserName);

        if (result is null)
        {
            return ResultsTo.BadRequest<bool>("User does not exist").WithMessage("Invalid argument provided.");
        }

        var roles = await HandleAsync(new GetUserRoles {User = request.UserName}, cancellationToken);

        if (roles is not null)
        {
            await _userManager.RemoveFromRolesAsync(result, roles.Value);

            if (roles.Value.Contains(PersonType.Employee.ToString()))
            {
                var response = await _getResetPersonEvent.GetResponse<Status, NotFound>(new
                {
                    request.UserName,
                });
                _logger.LogInformation("Reset Person User Credential");
            }
        }

        return ResultsTo.Success((await _userManager.DeleteAsync(result)).Succeeded);
    }


    public async Task<IFluentResults<bool>> HandleAsync(RemoveUserRole request, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.FindByNameAsync(request.UserName);

        if (result is null)
        {
            return ResultsTo.BadRequest<bool>("User does not exist").WithMessage("Invalid argument provided.");
        }

        var roles = await HandleAsync(new GetUserRoles {User = request.UserName}, cancellationToken);

        if (roles is null || !roles.Value.Any())
        {
            return ResultsTo.NotFound<bool>().WithMessage("No roles found");
        }

        return ResultsTo.Success((await _userManager.RemoveFromRolesAsync(result, roles.Value)).Succeeded);
    }

    public async Task<IFluentResults<bool>> HandleAsync(AddUserRole request, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.FindByNameAsync(request.UserName);
        var roles = new List<string>();

        if (result is null)
        {
            return ResultsTo.BadRequest<bool>("User does not exist").WithMessage("Invalid argument provided.");
        }

        var getRoles = await _roleManager.FindByNameAsync(request.RoleName);

        if (getRoles is not null)
        {
            roles.Add(getRoles.Name);
        }

        if (roles is null || !roles.Any())
        {
            return ResultsTo.NotFound<bool>("No roles found.").WithMessage("No roles found");
        }

        return ResultsTo.Success((await _userManager.AddToRolesAsync(result, roles)).Succeeded);
    }

    public async Task<IFluentResults<Tokens>> HandleAsync(RefreshTokens request, CancellationToken cancellationToken = default)
    {
        var jwtBearerToken = request.Tokens.BearerToken;
        var jwtHandler = new JwtSecurityTokenHandler();
        var claims = jwtHandler.ReadJwtToken(jwtBearerToken).Claims.ToList();

        if (claims is null || !claims.Any())
        {
            return ResultsTo.BadRequest<Tokens>("Invalid tokens provided").WithMessage("Invalid tokens provided");
        }

        if (claims.FirstOrDefault(c => c.Type.Contains("/name", StringComparison.InvariantCultureIgnoreCase))?.Value != request.Tokens.UserName)
        {
            return ResultsTo.BadRequest<Tokens>("Invalid tokens provided").WithMessage("Invalid tokens provided");
        }

        if (await HandleAsync(new IsUserExists {UserName = request.Tokens.UserName}, cancellationToken) is IsSome<ServiceUsers> userForRefresh)
        {
            if (claims.FirstOrDefault(c => c.Type.Contains("/userid", StringComparison.InvariantCultureIgnoreCase))?.Value != userForRefresh.Value.Id ||
                userForRefresh.Value.RefreshToken != request.Tokens.RefreshToken ||
                userForRefresh.Value.ApiToken != request.Tokens.ApiKey)
            {
                return ResultsTo.BadRequest<Tokens>("Invalid tokens provided").WithMessage("Invalid tokens provided");
            }

            var tokens = await _userTokenHelper.HandleAsync(new UserTokenHelper.CreateTokens
            {
                User = userForRefresh.Value,
            }, cancellationToken);

            return ResultsTo.Something(tokens);
        }

        if (claims.Any(c => c.Type.Contains("/usertype", StringComparison.InvariantCultureIgnoreCase) && c.Value == "employee"))
        {
            if (claims.FirstOrDefault(c => c.Type.Contains("/userid", StringComparison.InvariantCultureIgnoreCase)) is not null)
            {
                var tokens = await _userTokenHelper.HandleAsync(new UserTokenHelper.CreateTemporaryTokens
                {
                    PersonId = claims.FirstOrDefault(c => c.Type.Contains("/userid", StringComparison.InvariantCultureIgnoreCase)).Value,
                    UserName = request.Tokens.UserName,
                }, cancellationToken);

                return ResultsTo.Something(tokens);
            }
        }

        return ResultsTo.Failure<Tokens>("No tokens created.");
    }
    
    private async Task PublishUserEvent(ServiceUsers user, UserEventType eventType)
    {
        await _publishendpoint.Publish<IUserEvent>(new
        {
            EventType = eventType,
            user.UserName,
            user.Id,
        });

        _logger.LogInformation($"User: {user.UserName}, Event: {Enum.GetName(typeof(UserEventType), eventType)}");
    }
}
