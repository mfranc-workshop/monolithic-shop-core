using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using monolithic_shop_core.EmailHelpers;

namespace monolithic_shop_core.Services
{
    public interface IEmailService
    {
        void SendEmail(string email, EmailType type);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _client;

        public EmailService(SmtpClient client)
        {
            _client = client;
        }

        public void SendEmail(string emailAddress, EmailType type)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("awesome_shop@com.pl"));
            mail.To.Add(new MailboxAddress(emailAddress));

            var emailData = Email.Templates[type];
            mail.Subject = emailData.Item1;
            mail.Body = new TextPart("plain") { 
                Text = emailData.Item2
            };

            try
            {
                _client.Send(mail);
            }
            catch (Exception ex)
            {
                // swallowing errors!! muahahaha
            }
        }
    }
}
