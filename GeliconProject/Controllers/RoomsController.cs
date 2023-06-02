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
    public class RoomsController : Controller
    {
        private IRepository repository;
        private JsonSerializerOptions serializerOptions;

        public RoomsController(IRepository repository)
        {
            this.repository = repository;
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
            User? user = repository.GetUserByID(userID);
            return Json(user?.rooms, serializerOptions);
        }
    }
}
