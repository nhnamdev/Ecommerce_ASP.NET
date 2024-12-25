using final.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace final.Service.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailSettings:EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            try
            {
                // Kết nối đến Gmail SMTP server
                await smtp.ConnectAsync(_config.GetSection("EmailSettings:SMTPHost").Value,
                                        int.Parse(_config.GetSection("EmailSettings:SMTPPort").Value),
                                        SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config.GetSection("EmailSettings:EmailUsername").Value,
                                              _config.GetSection("EmailSettings:EmailPassword").Value);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Có lỗi khi gửi email: {ex.Message}");
            }
        }

    }
}
