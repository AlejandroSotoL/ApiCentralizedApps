using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos.DtosFintech
{
    public class TransactionResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public Result result { get; set; }
        public int state { get; set; }
    }
}