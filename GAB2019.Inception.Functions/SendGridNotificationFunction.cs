using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GAB2019.Inception.Model;
using SendGrid.Helpers.Mail;

namespace GAB2019.Inception.Functions
{
    public static class SendGridNotificationFunction
    {
        [FunctionName("SendGridNotificationFunction")]
        public static void Run([CosmosDBTrigger(
            databaseName: "Inception-FunctionsDB",
            collectionName: "AlertsToSend",
            ConnectionStringSetting = "StorageSettings:CosmosDBInception_FunctionsDB",
            LeaseCollectionName = "lease",
            CreateLeaseCollectionIfNotExists = true
            )]IReadOnlyList<Document> input, ILogger log,[SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message)
        {
            Alert alert = JsonConvert.DeserializeObject<Alert>(input[0].ToString());

            //Warning
            if (alert.Level.Equals("0"))
            {
                message = new SendGridMessage();
                message.AddTo(alert.AlertTo);
                message.AddContent("text/html", alert.Message);
                message.SetFrom(new EmailAddress("DomCobb@ingenieriaCobol.com"));
                message.SetSubject("Alerta nivel:" + alert.Level);
            }
            else
            {
                message = null;
            }
           
        }
    }

}
