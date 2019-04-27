using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAB2019.Inception.Web.Models.Configuration
{
    public class CosmosSettings
    {
        public string EndPoint { get; set; }
        public string PrimaryKey { get; set; }
        public string Database { get; set; }
        public string NotificationsCollection { get; set; }
    }
}
