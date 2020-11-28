using System.Threading.Tasks;

namespace EmailService.Service
{
    public interface IEmailSender
    {
        public Task<EmailSenderResult> SendEmailAsync(string email, string subject, string message);

    }
}
