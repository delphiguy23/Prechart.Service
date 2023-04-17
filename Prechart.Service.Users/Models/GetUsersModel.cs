namespace Prechart.Service.Users.Models;

public class GetUsersModel
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string[] Roles { get; set; }
}