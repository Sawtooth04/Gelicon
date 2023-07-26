using GeliconProject.Utils.JwtValidationParameters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GeliconProject.Utils.JwtTokensBuilder
{
    public class JwtTokenBuilder : IJwtTokenBuilder
    {
        private IJwtValidationParameters validationParameters;

        public JwtTokenBuilder(IJwtValidationParameters validationParameters)
        {
            this.validationParameters = validationParameters;
        }

        private List<Claim> GetLoginClaims(Models.User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.email!),
                new Claim(Claims.Claims.UserID, user.userID.ToString())
            };
        }

        public JwtSecurityToken GetJwtToken(Models.User user)
        {
            return new JwtSecurityToken
                (
                    issuer: IJwtValidationParameters.issuer,
                    audience: IJwtValidationParameters.audience,
                    claims: GetLoginClaims(user),
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(IJwtValidationParameters.expires)),
                    signingCredentials: new SigningCredentials(validationParameters.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
        }

        public JwtSecurityToken GetJwtRefreshToken()
        {
            Random random = new Random();

            return new JwtSecurityToken
                (
                    issuer: IJwtValidationParameters.issuer,
                    audience: IJwtValidationParameters.audience,
                    claims: new Claim[] { new Claim("str", random.NextInt64().ToString()) },
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(IJwtValidationParameters.refreshTokenExpires)),
                    signingCredentials: new SigningCredentials(validationParameters.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
        }
    }
}
