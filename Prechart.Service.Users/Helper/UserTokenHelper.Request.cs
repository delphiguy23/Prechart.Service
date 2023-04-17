using Prechart.Service.Users.Database.Models;

namespace Prechart.Service.Users.Helper;

public partial class UserTokenHelper
{
    public record CreateTokens
    {
        public ServiceUsers User { get; set; }
    }

    public record GenerateKey;

    public record CreateTemporaryTokens
    {
        public string PersonId { get; set; }
        public string UserName { get; set; }
    }
}
