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
    public class RoomController : Controller
    {
        private ApplicationContext context;
        private JsonSerializerOptions serializerOptions;

        public RoomController(ApplicationContext context)
        {
            this.context = context;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.MaxDepth = 8;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        [HttpPost]
        public async Task<ActionResult> Join()
        {
            int roomID = await Request.ReadFromJsonAsync<int>();
            Room? room = context.Rooms.Where(r => r.roomID == roomID).Include(r => r.roomUsersColors!).ThenInclude(r => r.color).Include(r => r.users).FirstOrDefault();
            Claim? userIDClaim = Request.HttpContext.User.FindFirst(Claims.UserID);
            User? user;

            if (userIDClaim == null)
                return NotFound();
            user = context.Users.Where(u => u.userID == int.Parse(userIDClaim.Value)).Include(u => u.rooms).FirstOrDefault();
            if (user != null && user.rooms != null && room != null && user.rooms.Contains(room))
                return Json(room, serializerOptions);
            return NotFound();
        }
    }
}
