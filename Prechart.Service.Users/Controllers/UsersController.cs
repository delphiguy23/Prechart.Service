using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Utils;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Users.Database.Models;
using Prechart.Service.Users.Models;
using Prechart.Service.Users.Services;

namespace Prechart.Service.Users.Controllers;

[ApiController]
[Route("/platform/service/api/users/")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUsersService _userService;


    public UsersController(ILogger<UsersController> logger, IUsersService userService, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _userService = userService;
        _publishEndpoint = publishEndpoint;
    }

    [Authorize(Roles = "SuperAdmin, Register, Profile")]
    [HttpPost]
    [Route("register")]
    [LogAuditAction]
    public async Task<ActionResult> RegisterUser(ServiceUserModel userModel)
    {
        if (string.IsNullOrWhiteSpace(userModel.UserName) && string.IsNullOrWhiteSpace(userModel.Password))
        {
            return BadRequest("User name and password are required");
        }

        if (string.IsNullOrWhiteSpace(userModel.Email) || !EmailHelper.IsValidEmail(userModel.Email))
        {
            return BadRequest("Invalid email address format");
        }

        if (await _userService.HandleAsync(new UsersService.IsUserExists {UserName = userModel.UserName}, CancellationToken.None) is { } _result &&
            _result.IsNotFoundOrBadRequest())
        {
            var result = await _userService.HandleAsync(new UsersService.RegisterUser
            {
                User = new ServiceUsers
                {
                    UserName = userModel.UserName.Trim().ToUpperInvariant(),
                    Email = userModel.Email,
                    EmailConfirmed = false,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    MiddleName = userModel.MiddleName,
                },
                Password = userModel.Password,
                Roles = userModel.Roles,
                PersonId = userModel.PersonId,
            }, CancellationToken.None);

            return result.ToActionResult();
        }

        return BadRequest("User already exists");
    }

    [AllowAnonymous]
    [Route("validate")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> ValidateUser([FromBody] ValidateUser validate)
    {
        return (await _userService.HandleAsync(new UsersService.ValidateUser
        {
            UserName = validate.UserName,
            Password = validate.Password,
        }, CancellationToken.None)).ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [Route("roles/add/{rolename}")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> AddRole(string rolename)
    {
        if (string.IsNullOrWhiteSpace(rolename))
        {
            return BadRequest("Role name is required");
        }

        var result = await _userService.HandleAsync(new UsersService.AddRole {RoleName = rolename}, CancellationToken.None);
        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [Route("roles")]
    [HttpGet]
    [LogAuditAction]
    public async Task<ActionResult> GetRoles()
    {
        var result = await _userService.HandleAsync(new UsersService.GetRoles(), CancellationToken.None);
        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [Route("roles/{user}")]
    [HttpGet]
    [LogAuditAction]
    public async Task<ActionResult> GetUserRoles(string user)
    {
        if (string.IsNullOrWhiteSpace(user))
        {
            return BadRequest("User name is required");
        }

        var result = await _userService.HandleAsync(new UsersService.GetUserRoles {User = user}, CancellationToken.None);
        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [Route("roles/delete/{rolename}")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> DeleteRole(string rolename)
    {
        if (string.IsNullOrWhiteSpace(rolename))
        {
            return BadRequest("RoleName is required");
        }

        var result = await _userService.HandleAsync(new UsersService.DeleteRole {RoleName = rolename}, CancellationToken.None);

        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [Route("roles/update/")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> UpdateRole(GetRoles role)
    {
        if (string.IsNullOrWhiteSpace(role.Id))
        {
            return BadRequest("Role Id is required");
        }

        var result = await _userService.HandleAsync(new UsersService.UpdateRole
        {
            Id = role.Id,
            RoleName = role.RoleName,
            RoleNormalizedName = role.RoleNormalizedName,
        }, CancellationToken.None);

        return result.ToActionResult();
    }


    [Authorize(Roles = "SuperAdmin")]
    [Route("all")]
    [HttpGet]
    [LogAuditAction]
    public async Task<ActionResult> GetAllUsers()
    {
        var result = await _userService.HandleAsync(new UsersService.GetUsers(), CancellationToken.None);
        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [Route("delete/{username}")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> DeleteUser(string username)
    {
        var result = await _userService.HandleAsync(new UsersService.DeleteUser {UserName = username}, CancellationToken.None);
        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [Route("removeuserrole/{username}")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> RemoveUserRole(string username)
    {
        var result = await _userService.HandleAsync(new UsersService.RemoveUserRole {UserName = username}, CancellationToken.None);
        return result.ToActionResult();
    }


    [Authorize(Roles = "SuperAdmin")]
    [Route("adduserrole/{username}")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> AddUserRole(string username, string rolename)
    {
        var result = await _userService.HandleAsync(new UsersService.AddUserRole {UserName = username, RoleName = rolename}, CancellationToken.None);
        return result.ToActionResult();
    }

    [AllowAnonymous]
    [Route("refresh")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> RefreshToken([FromBody] Tokens tokens)
    {
        var result = await _userService.HandleAsync(new UsersService.RefreshTokens
        {
            Tokens = tokens,
        }, CancellationToken.None);

        return result.ToActionResult();
    }

    [Authorize(Roles = "SuperAdmin")]
    [Route("testuser")]
    [HttpPost]
    [LogAuditAction]
    public async Task<ActionResult> CreateTestUser()
    {
        try
        {
            if (await _userService.HandleAsync(new UsersService.IsUserExists {UserName = "TEST@USER.COM"}, CancellationToken.None) is { } _result &&
                _result.IsNotFoundOrBadRequest())
            {
                var username = "test@user.com";

                var result = await _userService.HandleAsync(new UsersService.RegisterUser
                {
                    User = new ServiceUsers
                    {
                        UserName = username.Trim().ToUpperInvariant(),
                        Email = username.Trim(),
                        EmailConfirmed = false,
                        FirstName = "Test",
                        LastName = "User",
                        MiddleName = string.Empty,
                    },
                    Password = "T3ster!",
                    Roles = new[] {"Employee"},
                }, CancellationToken.None);
            }

            await _publishEndpoint.Publish<IPersonTestUser>(new {UserName = "test@user.com"});

            return ResultsTo.Something(new
            {
                UserName = "test@user.com",
                Password = "T3ster!",
            }).ToActionResult();
        }
        catch (Exception e)
        {
            return ResultsTo.Failure().FromException(e).ToActionResult();
        }
    }
}
