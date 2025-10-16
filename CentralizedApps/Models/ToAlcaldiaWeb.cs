using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models
{
    public class ToAlcaldiaWeb
    {
        public Municipality Alcaldia { get; set; }
        public List<Department> Departamentos { get; set; }
        public List<Bank> Bancos { get; set; }
        public List<ShieldMunicipality> Escudos { get; set; }
        public List<Theme> Themas { get; set; }

    }
}
