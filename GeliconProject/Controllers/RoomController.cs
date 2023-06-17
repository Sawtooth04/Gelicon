using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.Room;
using GeliconProject.Storage.Repositories.User;
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
        private IStorage storage;
        private JsonSerializerOptions serializerOptions;

        public RoomController(IStorage storage)
        {
            this.storage = storage;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.MaxDepth = 12;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        [HttpPost]
        public async Task<ActionResult> Join()
        {
            int roomID = await Request.ReadFromJsonAsync<int>();
            Room? room = storage.GetRepository<IRoomRepository>()?.GetRoomByID(roomID);
            Claim? userIDClaim = Request.HttpContext.User.FindFirst(Claims.UserID);
            User? user;

            if (userIDClaim == null)
                return NotFound();
            user = storage.GetRepository<IUserRepository>()?.GetUserByID(int.Parse(userIDClaim.Value));
            if (user != null && user.rooms != null && room != null && user.rooms.Find(r => r.roomID == room.roomID) != null)
                return Json(room, serializerOptions);
            return NotFound();
        }
    }
}
