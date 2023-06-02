using GeliconProject.Models;
using GeliconProject.Repositories;
using GeliconProject.Utils.ApplicationContexts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeliconProject.Controllers
{
    public class ColorController : Controller
    {
        private IRepository repository;
        private JsonSerializerOptions serializerOptions;

        public ColorController(IRepository repository)
        {
            this.repository = repository;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.MaxDepth = 4;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        [HttpGet]
        [ActionName("get-colors")]
        public IActionResult GetColors()
        {
            return Json(repository.GetColors(), serializerOptions);
        }
    }
}
