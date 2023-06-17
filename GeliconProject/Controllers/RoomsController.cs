using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Repositories.User;
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
        private IStorage storage;
        private JsonSerializerOptions serializerOptions;

        public RoomsController(IStorage storage)
        {
            this.storage = storage;
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
            User? user = storage.GetRepository<IUserRepository>()?.GetUserByID(userID);
            return Json(user?.rooms, serializerOptions);
        }
    }
}
