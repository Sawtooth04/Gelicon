using GeliconProject.Utils.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeliconProject.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpGet]
        [ActionName("authorization-check")]
        public ActionResult AuthorizationCheck()
        {
            if (Request.HttpContext.User.FindFirst(Claims.UserID) != null || Request.Headers.ContainsKey("Authorization"))
                return Ok();
            else
                return Unauthorized();
        }
    }
}
