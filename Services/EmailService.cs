using System;
using monolithic_shop_core.EmailHelpers;
using NLog;
using RestEase;
using System.Threading.Tasks;

namespace monolithic_shop_core.Services
{
    public interface IEmailService
    {
        void SendEmail(string email, EmailType type);
    }

    public class FakeEmailService: IEmailService
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public void SendEmail(string emailAddress, EmailType type)
        {
            _logger.Info($"Sending email to - {emailAddress} of type - {type}");
        }
    }

    public class EmailMessage 
    {
        public string EmailAddress { get; set; }
        public string Message { get; set; }

        public EmailMessage(string emailAddress, string message)
        {
           EmailAddress = emailAddress;
           Message = message;
        }
    }

    public interface IExternalEmailService
    {
        [Post("email")]
        Task<string> Send([Body]EmailMessage message);
    }

    public class ExternalEmailService : IEmailService
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public async void SendEmail(string emailAddress, EmailType type)
        {
            try
            {
                var client = RestClient.For<IExternalEmailService>("http://localhost:5001");
                var result = await client.Send(new EmailMessage(emailAddress, Email.Templates[type].Item2));
                _logger.Info($"Email request sent - {emailAddress} of type - {type} - with result {result}");
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Encountered a problem when sending Email");
            }
        }
    }
}
