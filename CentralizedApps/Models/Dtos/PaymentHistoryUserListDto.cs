using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos
{
    public class PaymentHistoryUserListDto
    {
        public int Id { get; set; }

        public String? UserFirtName { get; set; }

        public decimal? Amount { get; set; }

        public DateOnly? PaymentDate { get; set; }

        public bool? Status { get; set; }

        public String? MunicipalityName { get; set; }

        public String? ProcedureName { get; set; }

        public String? StatusType { get; set; }

    }
}