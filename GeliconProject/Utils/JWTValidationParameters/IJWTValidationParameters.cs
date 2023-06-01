using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GeliconProject.Utils.JWTValidationParameters
{
    public interface IJWTValidationParameters
    {
        public static readonly bool validateIssuer = true, validateAudience = true,
            validateLifetime = true, validateIssuerSigningKey = true;
        public static readonly string issuer = "Gelicon", audience = "GeliconClient", key = "eaRxVXZvjf5tPScaxRAcofre2a1SOhER";
        public static readonly int expires = 60;

        public SymmetricSecurityKey GetSymmetricSecurityKey();

        public void SetJWTOptionsToken(JwtBearerOptions options);
    }
}
