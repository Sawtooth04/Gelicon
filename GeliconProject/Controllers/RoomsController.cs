using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.Room;
using GeliconProject.Storage.Abstractions.Repositories.RoomUser;
using GeliconProject.Storage.Abstractions.Repositories.RoomUserColor;
using GeliconProject.Storage.Repositories.User;
using GeliconProject.Utils.Claims;
using GeliconProject.Utils.RoomJoinURLBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeliconProject.Controllers
{
    public class RoomsController : Controller
    {
        private IStorage storage;
        private JsonSerializerOptions serializerOptions;

        public RoomsController(IStorage storage)
        {
            this.storage = storage;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.MaxDepth = 10;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        private bool IsTokenValid(string token, out int roomID)
        {
            string[] tokenParts = token.Split('.');
            string encodedToken = RoomJoinTokenBuilder.Build(tokenParts[0], tokenParts[1]),
                payloadString = Encoding.UTF8.GetString(Convert.FromBase64String(tokenParts[1]));
            RoomJoinURLTokenPayload? payload = JsonSerializer.Deserialize<RoomJoinURLTokenPayload>(payloadString);

            roomID = int.Parse(payload!.roomID!);
            if (encodedToken == token && payload != null && DateTime.UtcNow <= DateTime.Parse(payload.exp!))
                return true;
            return false;
        }

        [HttpGet]
        [ActionName("get-rooms")]
        public IActionResult GetRooms()
        {
            Claim? userIDClaim = Request.HttpContext.User.FindFirst(Claims.UserID);
            if (userIDClaim == null)
                return Unauthorized();
            int userID = int.Parse(userIDClaim.Value);
            User? user = storage.GetRepository<IUserRepository>()?.GetUserByID(userID);
            return Json(user?.rooms, serializerOptions);
        }

        [HttpGet]
        [ActionName("get-join-room-link")]
        public IActionResult GetJoinRoomLink(int roomID)
        {
            return Json(RoomJoinTokenBuilder.Build(roomID), serializerOptions);
        }

        [HttpGet]
        [ActionName("join-room")]
        public IActionResult JoinRoom(string token)
        {
            Claim? userIDClaim = Request.HttpContext.User.FindFirst(Claims.UserID);
            int userID = int.Parse(userIDClaim!.Value);
            Room? room;
            User? user;
            int roomID;

            if (IsTokenValid(token, out roomID))
            {
                room = storage.GetRepository<IRoomRepository>()?.GetRoomByID(roomID);
                user = storage.GetRepository<IUserRepository>()?.GetUserByID(userID);
                if (room != null && user != null)
                {
                    storage.GetRepository<IRoomUserRepository>()?.AddRoomUser(roomID, userID);
                    storage.GetRepository<IRoomUserColorRepository>()?.AddDefaultRoomUserColor(room, user);
                    storage.Save();
                    return Json(roomID, serializerOptions);
                }
            }
            return Forbid();
        }
    }
}
