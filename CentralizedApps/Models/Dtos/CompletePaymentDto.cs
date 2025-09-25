using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;

namespace CentralizedApps.Models.Dtos
{
    public class CompletePaymentDto
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public UserDtoHistory? User { get; set; }
        public AvailibityDto? StatusType { get; set; }
        public MunicipalityProcedureDtoPayment? MunicipalityProcedure { get; set; }
    }

    public class UserDtoHistory
    {
        public int Id { get; set; }
        public string? UserFirtName { get; set; }
        public string? UserLastName { get; set; }
        public string? Email { get; set; }
        public string? NationalId { get; set; }
    }
    public class MunicipalityProcedureDtoPayment
    {
        public int Id { get; set; }
        public string IntegrationType { get; set; }
        public bool? IsActive { get; set; }
        public JustMunicipalitysDto? Municipality { get; set; }
        public ProcedureDto? Procedure { get; set; }
    }
}