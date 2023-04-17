using Prechart.Service.Users.Database.Models;
using Prechart.Service.Users.Models;

namespace Prechart.Service.Users.Services;

public partial class UsersService
{
    public record RegisterUser
    {
        public ServiceUsers User { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }
        public string PersonId { get; set; }
    }

    public record IsUserExists
    {
        public string UserName { get; set; }
    }

    public record IsPasswordOk
    {
        public ServiceUsers User { get; set; }
        public string Password { get; set; }
    }

    public record AddRole
    {
        public string RoleName { get; set; }
    }

    public record DeleteRole
    {
        public string RoleName { get; set; }
    }

    public record UpdateRole
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string RoleNormalizedName { get; set; }
        public string RoleConcurrencyStamp { get; set; }
    }

    public record GetRoles;

    public record GetUserRoles
    {
        public string User { get; set; }
    }

    public record ValidateUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public record GetUsers;

    public record DeleteUser
    {
        public string UserName { get; set; }
    }

    public record RemoveUserRole
    {
        public string UserName { get; set; }
    }
    public record AddUserRole
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
    public record RefreshTokens
    {
        public Tokens Tokens { get; set; }
    }

}
