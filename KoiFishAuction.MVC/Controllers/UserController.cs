using Microsoft.AspNetCore.Mvc;

namespace KoiFishAuction.MVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
