﻿using EcomemerceASP_NET.Models;
using EcomemerceASP_NET.Service.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace EcomemerceASP_NET.Controllers
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
            // Kiểm tra nếu email trống
            if (string.IsNullOrEmpty(request.To))
            {
                ModelState.AddModelError("Email", "Email không được để trống.");
                return View("SendMail");
            }
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

        #region VerifyCode

        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View("EmailSent");
        }
        [HttpPost]
        public IActionResult VerifyCode(string VerifyCode)
        {
            var storedCode = HttpContext.Session.GetString("RandomCode");
            Console.WriteLine($"Session Code: {storedCode}, Input Code: {VerifyCode}");

            if (!string.IsNullOrEmpty(storedCode) && storedCode.Equals(VerifyCode, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("ChangePassword", "KhachHang");
            }

            ModelState.AddModelError(string.Empty, "Invalid verification code.");
            return View("EmailSent");
        }

        [HttpGet]
        public JsonResult checkNull(string VerifyCode)
        {
            var check = VerifyCode;
            if (check != null || check.IsNullOrEmpty())
            {
                return Json(new { message = "null" });
            }
            else
            {
                return Json(new { message = "OK" });
            }
        }
        #endregion

    }
}
