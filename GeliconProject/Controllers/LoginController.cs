using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GeliconProject.Models;
using GeliconProject.Utils.ApplicationContexts;
using GeliconProject.Utils.JWTValidationParameters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GeliconProject.Utils.Claims;

namespace GeliconProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IJWTValidationParameters validationParameters;
        private ApplicationContext context;

        public LoginController(IJWTValidationParameters validationParameters, ApplicationContext context)
        {
            this.validationParameters = validationParameters;
            this.context = context;
        }

        private User getUserByEmail(string email)
        {
            return context.Users.Where(user => user.email == email).First();
        }

        private bool isPasswordValid(User user, string password)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user, user.passwordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        private bool isUserValid(string email, string password)
        {
            User user = getUserByEmail(email);
            if (user == null)
                return false;
            return isPasswordValid(user, password);
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
            if (isUserValid(email, password))
            {
                User user = context.Users.Where(user => user.email == email).First();
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
                return Ok();
            }
            return NotFound();
        }
    }
}
