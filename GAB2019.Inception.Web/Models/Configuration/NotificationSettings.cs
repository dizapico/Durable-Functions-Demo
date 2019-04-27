using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAB2019.Inception.Web.Models.Configuration
{
    public class NotificationSettings
    {
        public NotificationLevels Level { get; set; }
    }

    public class NotificationLevels
    {
        public int Notification { get; set; }
        public int Alert { get; set; }
    }
}
