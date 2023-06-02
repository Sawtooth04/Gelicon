using GeliconProject.Models;
using GeliconProject.Repositories;
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
        private IRepository repository;
        private JsonSerializerOptions serializerOptions;

        public RoomController(IRepository repository)
        {
            this.repository = repository;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.MaxDepth = 8;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        [HttpPost]
        public async Task<ActionResult> Join()
        {
            int roomID = await Request.ReadFromJsonAsync<int>();
            Room? room = repository.GetRoomByID(roomID);
            Claim? userIDClaim = Request.HttpContext.User.FindFirst(Claims.UserID);
            User? user;

            if (userIDClaim == null)
                return NotFound();
            user = repository.GetUserByID(int.Parse(userIDClaim.Value));
            if (user != null && user.rooms != null && room != null && user.rooms.Find(r => r.roomID == room.roomID) != null)
                return Json(room, serializerOptions);
            return NotFound();
        }
    }
}
