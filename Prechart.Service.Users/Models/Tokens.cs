namespace Prechart.Service.Users.Models;

public class Tokens
{
    public string BearerToken { get; set; }
    public string RefreshToken { get; set; }
    public string ApiKey { get; set; }
    public string UserName { get; set; }
}
