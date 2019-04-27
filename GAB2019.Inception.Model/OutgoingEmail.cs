using System;
using System.Collections.Generic;
using System.Text;

namespace GAB2019.Inception.Model
{
    public class OutgoingEmail
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
