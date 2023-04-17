using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Prechart.Service.Core.Configuration;

namespace Prechart.Service.Core.Authorization;

    public class TokenHelper : ITokenHelper
    {
        private readonly byte[] _symmetricKey;
        private readonly string _issuer;

        public TokenHelper(IOptions<GeneralConfiguration> generalConfiguration)
        {
            var generalConfig = generalConfiguration.Value;
            _symmetricKey = Encoding.UTF8.GetBytes(generalConfig.JwtKey);
            _issuer = $"Prechart_{generalConfig.Environment}".ToLowerInvariant();
        }

        public string GenerateToken(IList<Claim> claims, int expiresIn)
        {
            var jwtToken = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _issuer,
                    claims: claims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_symmetricKey), SecurityAlgorithms.HmacSha256),
                    expires: DateTime.Now.AddMinutes(expiresIn));

            var tokenHandler = new JwtSecurityTokenHandler();
            IEnumerable<Claim> claims1 ;
            var a = TryParseToken(tokenHandler.WriteToken(jwtToken), out claims1);
            
            return tokenHandler.WriteToken(jwtToken);
        }

        public bool TryParseToken(string token, out IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _issuer,
                        ValidateAudience = true,
                        ValidAudience = _issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_symmetricKey),
                    },
                    out var validatedToken);
                claims = tokenHandler.ReadJwtToken(token).Claims;
                return validatedToken.ValidTo > DateTime.UtcNow;
            }
            catch (SecurityTokenException)
            {
                claims = Array.Empty<Claim>();
            }

            return false;
        }
    }

