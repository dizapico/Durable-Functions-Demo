using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GAB2019.Inception.Model;
using SendGrid.Helpers.Mail;
using System;
using SendGrid;
using System.Threading.Tasks;

namespace GAB2019.Inception.Functions
{
    public static class SendGridNotificationFunction
    {
        [FunctionName("SendGridNotificationFunction")]
        public static async Task Run([ActivityTrigger] string AlertToSend, ILogger log)
        {

            string apikey = Environment.GetEnvironmentVariable("SendGridApiKey");
            SendGridClient client = new SendGridClient(apikey);
            EmailAddress from = new EmailAddress("rodolfo@encamina.com", "Rodolfo segurata");
            string subject = "Hemos detectado algo";
            EmailAddress to = new EmailAddress("dzapico@encamina.com", "Diego Zapico");
            string text = "Ha saltado una alerta";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, text,text);
            var response = await client.SendEmailAsync(msg);

           
        }
    }

}
