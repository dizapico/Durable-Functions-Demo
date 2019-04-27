using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GAB2019.Inception.Model;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GAB2019.Inception.DurableFunction
{
    public static partial class InceptionOrchestrator
    {

        [FunctionName("InceptionOrchestrator_StoreNotification")]
        public static async Task StoreNotification([ActivityTrigger] Notification notification, ILogger log)
        {
            string endpointUrl = Environment.GetEnvironmentVariable("Cosmos:Endpoint", EnvironmentVariableTarget.Process);
            string primaryKey = Environment.GetEnvironmentVariable("Cosmos:PrimaryKey", EnvironmentVariableTarget.Process);
            string database = Environment.GetEnvironmentVariable("Cosmos:Database", EnvironmentVariableTarget.Process);
            string collection = Environment.GetEnvironmentVariable("Cosmos:Collection", EnvironmentVariableTarget.Process);
            var client = new DocumentClient(new Uri(endpointUrl), primaryKey);
            var collectionLink = UriFactory.CreateDocumentCollectionUri(database, collection);

            await client.UpsertDocumentAsync(collectionLink, notification);
        }

    }
}