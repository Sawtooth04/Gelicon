using System.IdentityModel.Tokens.Jwt;

namespace GeliconProject.Utils.JwtTokensBuilder
{
    public interface IJwtTokenBuilder
    {
        public JwtSecurityToken GetJwtToken(Models.User user);

        public JwtSecurityToken GetJwtRefreshToken();
    }
}
