using System;

namespace GAB2019.Inception.Model
{
    public class Alert
    {
        public string Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string ImageURL { get; set; }
        public string AlertTo { get; set; }
    }
}
