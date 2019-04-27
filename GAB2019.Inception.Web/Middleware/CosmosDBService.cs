using GAB2019.Inception.Web.Models.Configuration;
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
        private CosmosSettings cosmosSettings;

        public CosmosDBService(CosmosSettings cosmosSettings)
        {
            this.cosmosSettings = cosmosSettings;
            this.documentDBClient = GetDocumentDBClient();
            this.database = cosmosSettings.Database;
        }

        public DocumentClient GetDocumentDBClient()
        {
            string endpointUrl = cosmosSettings.EndPoint;
            string primaryKey = cosmosSettings.PrimaryKey;
            var client = new DocumentClient(new Uri(endpointUrl), primaryKey);

            return client;
        }

        public List<Notification> GetNotifications(int level)
        {
            string collection = cosmosSettings.NotificationsCollection;
            var collectionLink = UriFactory.CreateDocumentCollectionUri(database, collection);

            var list = this.documentDBClient.CreateDocumentQuery<Notification>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                                            .Where(i => i.Level == level)
                                            .ToList();
            return list;
        }

    }
}
