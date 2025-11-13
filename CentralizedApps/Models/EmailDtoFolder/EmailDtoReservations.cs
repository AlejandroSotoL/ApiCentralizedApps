using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.EmailDto
{
    public class EmailDtoReservations
    {

        public string Id { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string IdUser { get; set; }
        public string NameUser { get; set; }
        public string EmailUser { get; set; }
        public string PhoneUser { get; set; }
        public DateTime? DateReservation { get; set; }
        public string Type { get; set; }
    }
}