using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UnTitledBook.UI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        
        public IActionResult Profile()
        {
            return View();
        }

        
        public IActionResult Friends()
        {
            return View();
        }

    }
}
