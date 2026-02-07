using Microsoft.AspNetCore.Mvc;

namespace E_TicaretWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
