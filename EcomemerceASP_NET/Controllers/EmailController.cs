using final.Models;
using final.Service.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace final.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult SendMail()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailDto request)
        {

            string verificationCode = GenerateRandomCode();


            request.Subject = "Code confirmation !";
            request.Body = $"your verify code: {verificationCode}";


            await _emailService.SendEmailAsync(request);
            return View("EmailSent");
        }

        private string GenerateRandomCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            var randomCode = new char[9];
            for (int i = 0; i < randomCode.Length; i++)
            {
                randomCode[i] = chars[random.Next(chars.Length)];
            }
            if (random != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in randomCode)
                {
                    sb.Append(c);
                }
                HttpContext.Session.SetString("RandomCode", sb.ToString());
            }
            return new string(randomCode);
        }
    }
}
