using Microsoft.AspNetCore.Mvc;

namespace ArtHive.Controllers
{
    public class UnitTestController : Controller
    {
        public IActionResult Index()
        {
            // put a variable in a ViewData (similar to ViewBag)
            ViewData["Message"] = "This is a ViewData message";
            
            return View("Index");
        }
    }
}
