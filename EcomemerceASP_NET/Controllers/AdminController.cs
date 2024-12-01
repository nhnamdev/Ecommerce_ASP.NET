using Microsoft.AspNetCore.Mvc;

namespace EcomemerceASP_NET.Controllers
{
    public class AdminController : Controller
    {
        //[Route("/Admin")]
        public IActionResult Index()
        {
            ViewBag.Title = "Admin";
            return View();
        }
        public IActionResult ManageProduct()
        {
            ViewBag.Title = "Product";
            return View();
        }
        public IActionResult ManageUser()
        {
            ViewBag.Title = "User";
            return View();
        }
    }
}
