namespace CentralizedApps.Models.Dtos
{
    public class CreateCourseDto
    {
        public int? MunicipalityId { get; set; }
        public string? Name { get; set; }

        public string? Get { get; set; }

        public string? Post { get; set; }
        public bool? IsActive { get; set; }
    }
}