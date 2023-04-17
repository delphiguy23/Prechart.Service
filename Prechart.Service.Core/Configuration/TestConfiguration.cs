using System.Collections.Generic;

namespace Prechart.Service.Core.Configuration;

public class TestConfiguration
{
    public bool RecurringTasks { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }
    public string Surname { get; set; }
    public string GivenName { get; set; }
    public string UserName { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public IEnumerable<string> UserRights { get; set; }
    public IEnumerable<string> AccessRoles { get; set; }
    public IEnumerable<string> ServiceRights { get; set; }
    public string UserType { get; set; }
}