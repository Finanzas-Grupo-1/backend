using finanzas_project.IAM.Application.Internal.OutboundServices;
using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.IAM.Infrastructure.Tokens.JWT.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace finanzas_project.IAM.Infrastructure.Tokens.JWT.Services
{
    public class TokenService(IOptions<TokenSettings> tokenSettings) : ITokenService
    {
        private readonly TokenSettings _tokenSettings = tokenSettings.Value;
        public string GenerateToken(User user)
        {
            var secret = _tokenSettings.Secret;
            if (string.IsNullOrEmpty(secret)) throw new Exception("Secret is not set");
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)

            };
            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public async Task<int?> ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            var tokenHandler = new JsonWebTokenHandler();
            var secret = _tokenSettings.Secret;
            if (string.IsNullOrEmpty(secret)) return 0;
            var key = Encoding.ASCII.GetBytes(secret);
            try
            {
                var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                });
                var jwtToken = (JsonWebToken)tokenValidationResult.SecurityToken;
                var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
                return userId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
