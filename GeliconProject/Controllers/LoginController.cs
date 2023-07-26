using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GeliconProject.Models;
using GeliconProject.Utils.JwtValidationParameters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GeliconProject.Utils.Claims;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Repositories.User;
using GeliconProject.Models.Validation;
using GeliconProject.Utils.JwtTokensBuilder;
using GeliconProject.Storage.Abstractions.Repositories.RefreshToken;

namespace GeliconProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IJwtValidationParameters validationParameters;
        private IStorage storage;
        private IJwtTokenBuilder tokenBuilder;

        public LoginController(IJwtValidationParameters validationParameters, IStorage storage, IJwtTokenBuilder tokenBuilder)
        {
            this.validationParameters = validationParameters;
            this.storage = storage;
            this.tokenBuilder = tokenBuilder;
        }

        private bool IsPasswordValid(User user, string password)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user, user.passwordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        private LoginValidation UserValidation(string email, string password)
        {
            User? user = storage.GetRepository<IUserRepository>()?.GetUserByEmail(email);
            return new LoginValidation()
            {
                EmailValid = user != null,
                PasswordValid = (user == null) ? true : IsPasswordValid(user, password)
            };
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            LoginValidation userValidation = UserValidation(email, password);

            if (userValidation.EmailValid && userValidation.PasswordValid)
            {
                User user = storage.GetRepository<IUserRepository>().GetUserByEmail(email)!;
                string refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenBuilder.GetJwtRefreshToken());
                IRefreshTokenRepository refreshTokenRepository = storage.GetRepository<IRefreshTokenRepository>();

                ControllerContext.HttpContext.Response.Cookies.Append("Authorization", new JwtSecurityTokenHandler().WriteToken(tokenBuilder.GetJwtToken(user)),
                    new CookieOptions { MaxAge = TimeSpan.FromMinutes(IJwtValidationParameters.expires), HttpOnly = true });
                ControllerContext.HttpContext.Response.Cookies.Append("Refresh", refreshToken,
                    new CookieOptions { MaxAge = TimeSpan.FromDays(IJwtValidationParameters.refreshTokenExpires), HttpOnly = true });
                refreshTokenRepository.AddOrReplaceRefreshToken(refreshToken, user.userID);
                storage.Save();
                return Ok(new {});
            }
            return Ok(userValidation);
        }
    }
}
