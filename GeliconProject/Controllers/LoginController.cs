using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GeliconProject.Models;
using GeliconProject.Utils.JWTValidationParameters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GeliconProject.Utils.Claims;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Repositories.User;
using GeliconProject.Models.Validation;

namespace GeliconProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IJWTValidationParameters validationParameters;
        private IStorage storage;

        public LoginController(IJWTValidationParameters validationParameters, IStorage storage)
        {
            this.validationParameters = validationParameters;
            this.storage = storage;
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

        private List<Claim> getLoginClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.email!),
                new Claim(Claims.UserID, user.userID.ToString())
            };
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            LoginValidation userValidation = UserValidation(email, password);

            if (userValidation.EmailValid && userValidation.PasswordValid)
            {
                User? user = storage.GetRepository<IUserRepository>()?.GetUserByEmail(email);
                JwtSecurityToken jwt = new JwtSecurityToken
                (
                    issuer: IJWTValidationParameters.issuer,
                    audience: IJWTValidationParameters.audience,
                    claims: getLoginClaims(user),
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(IJWTValidationParameters.expires)),
                    signingCredentials: new SigningCredentials(validationParameters.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
                ControllerContext.HttpContext.Response.Cookies.Append("Authorization", new JwtSecurityTokenHandler().WriteToken(jwt),
                    new CookieOptions { MaxAge = TimeSpan.FromMinutes(IJWTValidationParameters.expires) });
                return Ok(new {});
            }
            return Ok(userValidation);
        }
    }
}
