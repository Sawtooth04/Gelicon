using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.RefreshToken;
using GeliconProject.Storage.Repositories.User;
using GeliconProject.Utils.Claims;
using GeliconProject.Utils.JwtTokensBuilder;
using GeliconProject.Utils.JwtValidationParameters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GeliconProject.Middlewares
{
    public class JwtRefreshTokenMiddleware
    {
        private RequestDelegate next;

        public JwtRefreshTokenMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStorage storage, IJwtTokenBuilder tokenBuilder)
        {
            IUserRepository userRepository = storage.GetRepository<IUserRepository>();
            IRefreshTokenRepository refreshTokenRepository = storage.GetRepository<IRefreshTokenRepository>();
            string? refreshToken = context.Request.Cookies["Refresh"], token;
            int? userID;
            User? user;

            if (!context.Request.Headers.ContainsKey("Authorization") && refreshToken != null)
            {
                userID = refreshTokenRepository.GetUserIDByRefreshToken(refreshToken);
                if (userID != null)
                {
                    user = userRepository.GetUserByID((int)userID);
                    refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenBuilder.GetJwtRefreshToken());
                    token = new JwtSecurityTokenHandler().WriteToken(tokenBuilder.GetJwtToken(user));
                    refreshTokenRepository.AddOrReplaceRefreshToken(refreshToken, (int)userID);
                    storage.Save();
                    Console.WriteLine("Saved: " + refreshToken);
                    context.Response.Cookies.Append("Authorization", token,
                        new CookieOptions { MaxAge = TimeSpan.FromMinutes(IJwtValidationParameters.expires), HttpOnly = true });
                    context.Response.Cookies.Append("Refresh", refreshToken,
                        new CookieOptions { MaxAge = TimeSpan.FromDays(IJwtValidationParameters.refreshTokenExpires), HttpOnly = true });
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
            }
            await next.Invoke(context);
        }
    }
}
