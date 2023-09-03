using Microsoft.AspNetCore.Mvc;

namespace ArtHive.Controllers
{
    public class UnitTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
