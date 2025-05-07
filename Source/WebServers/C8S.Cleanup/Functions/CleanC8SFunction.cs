using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SC.SendGrid.Services;

namespace C8S.Functions.Functions
{
    public class CleanC8SFunction(
        ILoggerFactory loggerFactory,
        EmailService emailService)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CleanC8SFunction>();

        [Function("CleanC8SFunction")]
        public void Run([TimerTrigger("0 30 5 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
