using GAB2019.Inception.Web.Models.CosmosDb;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAB2019.Inception.Web.Middleware
{
    public class CosmosDBService
    {
        private DocumentClient documentDBClient;
        private string database;

        public CosmosDBService()
        {
            this.documentDBClient = GetDocumentDBClient();
            this.database = Environment.GetEnvironmentVariable("Cosmos:Database", EnvironmentVariableTarget.Process);
        }

        public DocumentClient GetDocumentDBClient()
        {
            string endpointUrl = Environment.GetEnvironmentVariable("Cosmos:Endpoint", EnvironmentVariableTarget.Process);
            string primaryKey = Environment.GetEnvironmentVariable("Cosmos:PrimaryKey", EnvironmentVariableTarget.Process);
            var client = new DocumentClient(new Uri(endpointUrl), primaryKey);

            return client;
        }

        public List<Notification> GetNotifications(int level)
        {
            string collection = Environment.GetEnvironmentVariable("Cosmos:NotificationsCollection", EnvironmentVariableTarget.Process);
            var collectionLink = UriFactory.CreateDocumentCollectionUri(database, collection);

            var list = this.documentDBClient.CreateDocumentQuery<Notification>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                                            .Where(i => i.Level == level)
                                            .ToList();
            return list;
        }

    }
}
