using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.DtosTables;

namespace CentralizedApps.Models.Dtos
{
    public class RegisterMunicipalityDto
    {
        //Municipality
        public string NameMunicipality { get; set; }
        public int EntityCode { get; set; }
        public int DepartmentId { get; set; }
        public int ThemeId { get; set; }
        public bool IsActive { get; set; }
        public string Domain { get; set; }
        public string UserFintech { get; set; }
        public string PasswordFintech { get; set; }

        //Mores
        public string Departamento { get; set; }
        public string TemaName { get; set; }

        //Theme
        public string Tema { get; set; } // solo nombre del tema
        public string? BackGroundColor { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? OnPrimaryColorDark { get; set; }
        public string? OnPrimaryColorLight { get; set; }
        public string? SecondaryColorBlack { get; set; }
        public string? Shield { get; set; }


        // Trámites asociados
        public List<MunicipalityProceduresDto> Tramites { get; set; } = new();
        // Redes sociales
        public List<SocialMediaMunicipality> RedesSociales { get; set; } = new();

    }
}