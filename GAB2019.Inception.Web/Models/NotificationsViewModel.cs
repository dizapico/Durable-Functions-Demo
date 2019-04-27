using GAB2019.Inception.Web.Middleware;
using GAB2019.Inception.Web.Models.Configuration;
using GAB2019.Inception.Web.Models.CosmosDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        public NotificationsViewModel(NotificationSettings notificationSettings, CosmosSettings cosmosSettings)
        {
            var levelNotification = notificationSettings.Level.Notification;
            var levelAlert = notificationSettings.Level.Alert;
            var cosmosDBService = new CosmosDBService(cosmosSettings);

            this.Notifications = cosmosDBService.GetNotifications(levelNotification);
            this.Alerts = cosmosDBService.GetNotifications(levelAlert);
        }
    }
}
