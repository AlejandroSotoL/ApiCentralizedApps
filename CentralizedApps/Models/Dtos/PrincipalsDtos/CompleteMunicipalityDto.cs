
namespace CentralizedApps.Models.Dtos.PrincipalsDtos
{
    public class CompleteMunicipalityDto
    {
        //Municipality Dtos
        public string? Name { get; set; }
        public string? EntityCode { get; set; }
        public string? Domain { get; set; }
        public string? UserFintech { get; set; }
        public string? PasswordFintech { get; set; }
        public bool? IsActive { get; set; }
        //Departament Dtoc
        public string? Department { get; set; }
        //Theme Dto
        public string? Theme { get; set; }
        public string? NameEscudo { get; set; }
        public string? NameBank { get; set; }
        public string? DataPrivacy { get; set; }
        public string? DataProcessingPrivacy { get; set; }

    }
}