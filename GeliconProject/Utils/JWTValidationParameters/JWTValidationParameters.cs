using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GeliconProject.Utils.JWTValidationParameters
{
    public class JWTValidationParameters : IJWTValidationParameters
    {
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IJWTValidationParameters.key));
        }

        public void SetJWTOptionsToken(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = IJWTValidationParameters.validateIssuer,
                ValidIssuer = IJWTValidationParameters.issuer,
                ValidateAudience = IJWTValidationParameters.validateAudience,
                ValidAudience = IJWTValidationParameters.audience,
                ValidateLifetime = IJWTValidationParameters.validateLifetime,
                IssuerSigningKey = GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = IJWTValidationParameters.validateIssuerSigningKey,
            };
        }
    }
}
