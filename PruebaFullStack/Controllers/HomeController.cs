using Microsoft.AspNetCore.Mvc;

namespace PruebaFullstack.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
