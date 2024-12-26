using EcomemerceASP_NET.Models;
namespace EcomemerceASP_NET.Service.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto request); 
    }
}
