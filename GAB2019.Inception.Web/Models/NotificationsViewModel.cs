using GAB2019.Inception.Web.Middleware;
using GAB2019.Inception.Web.Models.CosmosDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAB2019.Inception.Web.Models
{
    public class NotificationsViewModel
    {
        public List<Notification> Notifications { get; set; }
        public List<Notification> Alerts { get; set; }

        public NotificationsViewModel()
        {
            var levelNotification = Int32.Parse(Environment.GetEnvironmentVariable("Notifications:Level:Notification", EnvironmentVariableTarget.Process));
            var levelAlert = Int32.Parse(Environment.GetEnvironmentVariable("Notifications:Level:Alert", EnvironmentVariableTarget.Process));
            var cosmosDBService = new CosmosDBService();

            this.Notifications = cosmosDBService.GetNotifications(levelNotification);
            this.Alerts = cosmosDBService.GetNotifications(levelAlert);
        }
    }
}
