using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeliconProject.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        [Authorize]
        public string Hello()
        {
            return "hello!";
        }
    }
}
