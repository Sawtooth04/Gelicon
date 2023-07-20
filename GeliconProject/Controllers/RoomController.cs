using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.Room;
using GeliconProject.Storage.Abstractions.Repositories.RoomUser;
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

        private List<int> GetEditableUsers(Room room, int userID)
        {
            if (room.ownerID == userID)
                return storage.GetRepository<IRoomUserRepository>().GetRoomUsers(room.roomID)!.ConvertAll(u => u.userID);
            else
                return new List<int> { userID };
        }

        [HttpPost]
        [ActionName("add-room")]
        public async Task<ActionResult> AddRoom([FromBody] Room room)
        {
            room.owner = storage.GetRepository<IUserRepository>()?.GetUserByID(int.Parse(Request.HttpContext.User.FindFirst(Claims.UserID)!.Value));
            room.users = new List<User>();
            room.users.Add(room.owner!);
            room.roomUsersColors = new List<RoomUserColor>();
            room.roomUsersColors.Add(new RoomUserColor()
            {
                user = room.owner,
                color = room.defaultColor
            });
            storage.GetRepository<IRoomRepository>()?.AddRoom(room);
            storage.Save();
            return Json(room, serializerOptions);
        }

        [HttpPost]
        public async Task<ActionResult> Join([FromBody]JsonElement[] args)
        {
            Room? room = storage.GetRepository<IRoomRepository>().GetRoomByID(args[0].GetInt32()!);
            int userID = int.Parse(Request.HttpContext.User.FindFirst(Claims.UserID)!.Value);
            User? user;

            if (room == null)
                return NotFound();
            user = storage.GetRepository<IUserRepository>().GetUserByID(userID);
            if (user != null && user.rooms != null && room != null && user.rooms.Find(r => r.roomID == room.roomID) != null)
                return Json(new { room = room, editableUsers = GetEditableUsers(room, userID) }, serializerOptions);
            return NotFound();
        }
    }
}
