namespace CentralizedApps.Models.Dtos
{
    public class CreateSportsFacilityDto
    {
        public int? MunicipalityId { get; set; }
        public string? Name { get; set; }
        public string? Get { get; set; }
        public bool? IsActive { get; set; }
        public string? CalendaryPost { get; set; }

        public string? ReservationPost { get; set; }
    }
}