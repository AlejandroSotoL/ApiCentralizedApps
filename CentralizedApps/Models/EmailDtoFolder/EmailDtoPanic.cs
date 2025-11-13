using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.EmailDto
{
    public class EmailDtoPanic
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public string locationCoordinates { get; set; }
    }
}