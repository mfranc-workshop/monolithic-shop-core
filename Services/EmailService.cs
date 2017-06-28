using System;
using monolithic_shop_core.EmailHelpers;
using Microsoft.Extensions.Logging;

namespace monolithic_shop_core.Services
{
    public interface IEmailService
    {
        void SendEmail(string email, EmailType type);
    }

    public class FakeEmailService : IEmailService
    {
        private ILogger _logger;

        public FakeEmailService(ILogger<FakeEmailService> logger)
        {
            _logger = logger;
        }

        public void SendEmail(string emailAddress, EmailType type)
        {
            _logger.LogInformation($"Sending email to - {emailAddress} of type - {type}");
        }
    }
}
