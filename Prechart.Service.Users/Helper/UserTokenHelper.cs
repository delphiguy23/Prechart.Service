using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.Authorization;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Users.Database.Models;
using Prechart.Service.Users.Models;

namespace Prechart.Service.Users.Helper;

public partial class UserTokenHelper : IUserTokenHelper
{
    private readonly ILogger<UserTokenHelper> _logger;
    private readonly ITokenHelper _tokenHelper;
    private readonly UserManager<ServiceUsers> _userManager;

    public UserTokenHelper(ILogger<UserTokenHelper> logger, UserManager<ServiceUsers> userManager, ITokenHelper tokenHelper)
    {
        _logger = logger;
        _userManager = userManager;
        _tokenHelper = tokenHelper;
    }

    public async Task<IFluentResults<Tokens>> HandleAsync(CreateTokens request, CancellationToken cancellationToken = default)
    {
        if (request?.User is null || string.IsNullOrWhiteSpace(request?.User.UserName))
        {
            return ResultsTo.BadRequest<Tokens>("Invalid argument provided.");
        }

        var user = request.User;
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Surname, user.LastName ?? ""),
            new(ClaimTypes.GivenName, user.FirstName ?? ""),
            new(ClaimTypes.Name, user.UserName ?? ""),
        };

        claims.SetUserId($"{user.Id}");
        claims.SetUserType("user");
        claims.SetRoles(userRoles);

        string[] accessRoles = {"read", "write", "delete"};
        string[] serviceRights =
        {
            "svc-belastingen",
            "svc-documents",
            "svc-klant",
            "svc-loonheffings",
            "svc-person",
            "svc-users",
            "svc-werkgever",
            "svc-batch",
        };

        claims.SetAccessRoles(accessRoles);
        claims.SetUserRights(accessRoles);
        claims.SetServiceRights(serviceRights);

        var tokens = new Tokens
        {
            BearerToken = _tokenHelper.GenerateToken(claims, 60),
            RefreshToken = user.RefreshToken,
            ApiKey = user.ApiToken,
            UserName = user.UserName,
        };

        if (string.IsNullOrWhiteSpace(tokens.BearerToken) || string.IsNullOrWhiteSpace(tokens.RefreshToken) || string.IsNullOrWhiteSpace(tokens.ApiKey))
        {
            _logger.LogCritical("User {0} is missing some tokens.", user.UserName);
            return ResultsTo.Failure<Tokens>().WithMessage($"User {user} is missing some tokens.").WithMessage("Missing tokens");
        }

        return ResultsTo.Success(tokens);
    }

    public async Task<IFluentResults<string>> HandleAsync(GenerateKey request, CancellationToken cancellationToken = default)
    {
        var randomNumber = new byte[64];
        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            var key = string.Empty;

            await Task.Run(() =>
            {
                randomNumberGenerator.GetBytes(randomNumber);
                key = Convert.ToBase64String(randomNumber);
            });

            return ResultsTo.Success(key);
        }
    }

    public async Task<IFluentResults<Tokens>> HandleAsync(CreateTemporaryTokens request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.PersonId) || string.IsNullOrWhiteSpace(request.UserName))
        {
            return ResultsTo.BadRequest<Tokens>("Invalid argument provided.");
        }

        var user = request.UserName;

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user), //input username
        };

        claims.SetUserId($"{request.PersonId}");
        claims.SetUserType("employee");
        claims.SetRoles(new List<string> {"Register", "Profile"});

        string[] accessRoles = {"read", "write"};
        string[] serviceRights =
        {
            "svc-person",
        };

        claims.SetAccessRoles(accessRoles);
        claims.SetUserRights(accessRoles);
        claims.SetServiceRights(serviceRights);

        var tokens = new Tokens
        {
            BearerToken = _tokenHelper.GenerateToken(claims, 30),
            RefreshToken = (await HandleAsync(new GenerateKey()))?.Value ?? string.Empty,
            ApiKey = (await HandleAsync(new GenerateKey()))?.Value ?? string.Empty,
            UserName = user,
        };

        if (string.IsNullOrWhiteSpace(tokens.BearerToken))
        {
            _logger.LogCritical("User {0} is missing some tokens.", user);
            return ResultsTo.Failure<Tokens>().WithMessage($"User {user} is missing some tokens.").WithMessage("Missing tokens");
        }

        return ResultsTo.Success(tokens);
    }
}
