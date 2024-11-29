using Microsoft.AspNetCore.Mvc;

namespace EcomemerceASP_NET.Controllers
{
    public class AdminController : Controller
    {
        [Route("/Admin")]
        public IActionResult Admin()
        {
            ViewBag.Title = "Admin";
            return View();
        }
    }
}
