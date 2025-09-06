using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos.DtosFintech
{
    public class PayerDto
    {
        public string Documento { get; set; }
        public int TipoDocumento { get; set; }
        public string Nombre_Completo { get; set; }
        public int Dv { get; set; }
        public string PRIMERNOMBRE { get; set; }
        public string SEGUNDONOMBRE { get; set; }
        public string PRIMERAPELLIDO { get; set; }
        public string SEGUNDOAPELLIDO { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
    }
}