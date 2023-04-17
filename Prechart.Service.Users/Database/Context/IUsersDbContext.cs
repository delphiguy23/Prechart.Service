using Microsoft.EntityFrameworkCore;
using Prechart.Service.Core.Database;
using Prechart.Service.Core.Service;
using Prechart.Service.Users.Database.Models;
using Prechart.Service.Users.Models;

namespace Prechart.Service.Users.Database.Context;

public interface IUsersDbContext : IDbContext, IInitializable
{
    DbSet<ServiceUsers> ServiceUsers { get; set; }
}