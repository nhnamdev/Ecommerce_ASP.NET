using EcomemerceASP_NET.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcomemerceASP_NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }
        [Route("/Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("/Checkout")]
        public IActionResult Checkout()
        {
            return View();
        }
        [Route("/Testimonial")]
        public IActionResult Testimonial()
        {
            return View();
        }
     
    
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
