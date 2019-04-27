using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using GAB2019.Inception.Model;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GAB2019.Inception.DurableFunction
{
    public static partial class InceptionOrchestrator
    {
        [FunctionName("InceptionOrchestrator_NotifyUser")]
        public static async Task NotifyUser([ActivityTrigger] Notification notification, ILogger log)
        {
            string apikey = Environment.GetEnvironmentVariable("SendGridApiKey");
            string subject = string.Empty;

            if (notification.Level == 0)
            {
                subject = "[ALERTA] Se te están intentando colar";
            }
            else
            {
                subject = "[ALERTA] Se te están intentando colar";
            }

            var client = new SendGridClient(apikey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("rodolfo@encamina.com"),
                Subject = subject,
                HtmlContent = $"<br>{notification.Message}</br>"
            };

            msg.AddTo("sporras@encamina.com");
            msg.AddTo("dzapico@encamina.com");

            await client.SendEmailAsync(msg);
        }

    }
}