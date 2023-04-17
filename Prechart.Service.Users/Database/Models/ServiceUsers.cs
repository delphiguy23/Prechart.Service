using Microsoft.AspNetCore.Identity;
using Prechart.Service.AuditLog.Models.Users;

namespace Prechart.Service.Users.Database.Models;

public class ServiceUsers : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string RefreshToken { get; set; }
    public string ApiToken { get; set; }
    public bool Active { get; set; }
}
