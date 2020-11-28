using EmailService.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace EmailService.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _mailSettings;

        public EmailSender(IOptions<EmailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<EmailSenderResult> SendEmailAsync(string email, string subject, string message)
        {

            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, false);
                    await client.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }

                return new EmailSenderResult
                {
                    IsSuccessful = true,
                    Message = "Ok",
                };

            }
            catch (Exception exception)
            {
                return new EmailSenderResult
                {
                    IsSuccessful = false,
                    Message = exception.Message,
                };
            }
        }
    }
}

