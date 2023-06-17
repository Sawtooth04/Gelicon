using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.Color;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeliconProject.Controllers
{
    public class ColorController : Controller
    {
        private IStorage storage;
        private JsonSerializerOptions serializerOptions;

        public ColorController(IStorage storage)
        {
            this.storage = storage;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.MaxDepth = 4;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        [HttpGet]
        [ActionName("get-colors")]
        public IActionResult GetColors()
        {
            return Json(storage.GetRepository<IColorRepository>()?.All(), serializerOptions);
        }
    }
}
