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
    }
}
