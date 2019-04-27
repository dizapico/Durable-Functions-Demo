using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAB2019.Inception.Web.Models.CosmosDb
{
    public class Notification
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public int Level { get; set; }
        public string ImageUrl { get; set; }
    }
}
