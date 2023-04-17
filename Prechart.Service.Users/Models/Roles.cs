namespace Prechart.Service.Users.Models;

public class GetRoles
{
    public string Id { get; set; }
    public string RoleName { get; set; }
    public string RoleNormalizedName { get; set; }
    public string RoleConcurrencyStamp { get; set; }
}