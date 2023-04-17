using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using Prechart.Service.Core.Authorization;
using Prechart.Service.Core.Configuration;

namespace Prechart.Service.Core.Controllers;

[Route("/platform/account")]
[AllowAnonymous]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IOptions<GeneralConfiguration> _generalConfiguration;
    private readonly IOptions<TestConfiguration> _testConfiguration;
    private readonly ITokenHelper _tokenHelper;

    public AccountController(IWebHostEnvironment hostingEnvironment, IOptions<GeneralConfiguration> generalConfiguration, IOptions<TestConfiguration> testConfiguration, ITokenHelper tokenHelper)
    {
        _hostingEnvironment = hostingEnvironment;
        _generalConfiguration = generalConfiguration;
        _testConfiguration = testConfiguration;
        _tokenHelper = tokenHelper;
        _generalConfiguration = generalConfiguration;
        _testConfiguration = testConfiguration;
        _tokenHelper = tokenHelper;
    }

    [HttpGet]
    [Route("token")]
    [Produces("application/json")]
    public ActionResult TestToken()
    {
        if (_hostingEnvironment.IsDevelopment())
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _testConfiguration.Value.Email),
                new Claim(ClaimTypes.Surname, _testConfiguration.Value.Surname),
                new Claim(ClaimTypes.GivenName, _testConfiguration.Value.GivenName),
                new Claim(ClaimTypes.Name, _testConfiguration.Value.UserName),
            };

            claims.SetUserId(_testConfiguration.Value.UserId);
            claims.SetUserType(_testConfiguration.Value.UserType);
            claims.SetRoles(_testConfiguration.Value.Roles);
            claims.SetAccessRoles(_testConfiguration.Value.AccessRoles);
            claims.SetUserRights(_testConfiguration.Value.UserRights);
            claims.SetServiceRights(_testConfiguration.Value.ServiceRights);

            return new OkObjectResult(_tokenHelper.GenerateToken(claims, 60 * 12));
        }

        return Forbid();
    }
}