using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos.DtosFintech
{
    public class authenticationResponseFintechDto
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string result { get; set; }
    }
}