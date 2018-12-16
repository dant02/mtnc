using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Content("default action", "text/plain");
        }

        [HttpGet]
        public ActionResult Double()
        {
            return Content("another action", "text/plain");
        }
    }
}