using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GeliconProject.Utils.JwtValidationParameters
{
    public interface IJwtValidationParameters
    {
        public static readonly bool validateIssuer = true, validateAudience = true,
            validateLifetime = true, validateIssuerSigningKey = true;
        public static readonly string issuer = "Gelicon", audience = "GeliconClient", key = "eaRxVXZvjf5tPScaxRAcofre2a1SOhER";
        public static readonly int expires = 1, refreshTokenExpires = 28;

        public SymmetricSecurityKey GetSymmetricSecurityKey();

        public void SetJwtOptionsToken(JwtBearerOptions options);
    }
}
