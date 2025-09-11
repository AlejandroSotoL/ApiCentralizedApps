
namespace CentralizedApps.Models.Dtos
{
    public class PaymentHistoryUserListDto
    {
        public int Id { get; set; }
        public string? UserFirtName { get; set; }
        public decimal? Amount { get; set; }
        public DateOnly? PaymentDate { get; set; }
        public bool? Status { get; set; }
        public string? ProcedureName { get; set; }
        public string? Alcaldia { get; set; }
        public string? StatusType { get; set; }
        public int? IdStatusType { get; set; }
        public string? Idimpuesto { get; set; }
        public string? Factura { get; set; }
        public string? CodigoEntidad { get; set; }
    }
}