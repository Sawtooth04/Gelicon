using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GeliconProject.Utils.JwtValidationParameters
{
    public class JwtValidationParameters : IJwtValidationParameters
    {
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IJwtValidationParameters.key));
        }

        public void SetJwtOptionsToken(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = IJwtValidationParameters.validateIssuer,
                ValidIssuer = IJwtValidationParameters.issuer,
                ValidateAudience = IJwtValidationParameters.validateAudience,
                ValidAudience = IJwtValidationParameters.audience,
                ValidateLifetime = IJwtValidationParameters.validateLifetime,
                IssuerSigningKey = GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = IJwtValidationParameters.validateIssuerSigningKey,
            };
        }
    }
}
