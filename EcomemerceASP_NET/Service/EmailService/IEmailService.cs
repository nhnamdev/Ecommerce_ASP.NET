namespace final.Service.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto request); 
    }
}
