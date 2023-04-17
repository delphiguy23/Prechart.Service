using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prechart.Service.Core.Configuration;

namespace Prechart.Service.Core.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtSetup(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               var general = new GeneralConfiguration();
               configuration.Bind("General", general);

               var issuer = $"Prechart_{general.Environment}".ToLowerInvariant();

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = issuer,
                   ValidAudience = issuer,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(general.JwtKey)),
               };
           });

            return services;
        }
    }
}
