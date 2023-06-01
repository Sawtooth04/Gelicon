using GeliconProject.Models;
using GeliconProject.Utils.ApplicationContexts;
using GeliconProject.Utils.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeliconProject.Controllers
{
    public class RoomsController : Controller
    {
        private ApplicationContext context;
        private JsonSerializerOptions serializerOptions;

        public RoomsController(ApplicationContext context)
        {
            this.context = context;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.MaxDepth = 10;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        [HttpGet]
        [ActionName("get-rooms")]
        public IActionResult GetRooms()
        {
            Claim? userIDClaim = Request.HttpContext.User.FindFirst(Claims.UserID);
            if (userIDClaim == null)
                return NotFound();
            int userID = int.Parse(userIDClaim.Value);
            User? user = context.Users.Where(u => u.userID == userID).Include(u => u.rooms).ThenInclude(r => r.owner).FirstOrDefault();
            return Json(user?.rooms, serializerOptions);
        }
    }
}
