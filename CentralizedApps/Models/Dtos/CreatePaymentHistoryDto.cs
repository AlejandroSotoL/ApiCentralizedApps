using System;
using System.Collections.Generic;
using System.Linq;

namespace CentralizedApps.Models.Dtos
{
    public class CreatePaymentHistoryDto
    {
        public int? UserId { get; set; }

        public decimal? Amount { get; set; }

        public DateOnly? PaymentDate { get; set; }

        public bool? Status { get; set; }

        public int? MunicipalityId { get; set; }

        public int? MunicipalityProceduresId { get; set; }

        public int? StatusType { get; set; }

    }
}