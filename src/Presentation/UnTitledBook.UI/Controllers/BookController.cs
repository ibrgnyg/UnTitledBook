using Microsoft.AspNetCore.Mvc;

namespace UnTitledBook.UI.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult SharedBookNotes()
        {
            return View();
        }
    }
}
