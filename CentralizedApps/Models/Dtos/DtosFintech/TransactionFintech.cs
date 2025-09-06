using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos.DtosFintech
{
    public class TransactionFintech
    {
    public int IdTramite { get; set; }
    public PayerDto Pagador { get; set; }
    public int FuentePago { get; set; }
    public int TipoImplementacion { get; set; }
    public bool Estado_Url { get; set; }
    public string Url { get; set; }
    public int ValorPagar { get; set; }
    public string Factura { get; set; }
    public string referencia { get; set; }
    public string Descripcion { get; set; }
    }
}