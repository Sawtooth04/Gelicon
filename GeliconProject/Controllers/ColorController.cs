using GeliconProject.Models;
using GeliconProject.Utils.ApplicationContexts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeliconProject.Controllers
{
    public class ColorController : Controller
    {
        private ApplicationContext context;
        private JsonSerializerOptions serializerOptions;

        public ColorController(ApplicationContext context)
        {
            this.context = context;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.MaxDepth = 8;
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        [HttpGet]
        [ActionName("get-colors")]
        public IActionResult GetColors()
        {
            return Json(context.Colors.ToList(), serializerOptions);
        }
    }
}
