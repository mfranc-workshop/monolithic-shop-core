using System;
using monolithic_shop_core.EmailHelpers;
using NLog;

namespace monolithic_shop_core.Services
{
    public interface IEmailService
    {
        void SendEmail(string email, EmailType type);
    }

    public class FakeEmailService : IEmailService
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public void SendEmail(string emailAddress, EmailType type)
        {
            _logger.Info($"Sending email to - {emailAddress} of type - {type}");
        }
    }
}
