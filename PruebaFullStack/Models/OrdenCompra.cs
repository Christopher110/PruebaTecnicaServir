using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PruebaFullstack.Models
{
    public class OrdenCompra
    {
        public int Id { get; set; }
        [DataType(DataType.Date)] public DateTime Fecha { get; set; } = DateTime.Today;
        [Required, StringLength(200)] public string Proveedor { get; set; } = "";
        public List<OrdenCompraDetalle> Detalles { get; set; } = new();
    }

    public class OrdenCompraDetalle
    {
        public int Id { get; set; }
        public int OrdenCompraId { get; set; }
        [Required] public int ProyectoId { get; set; }
        [Required] public int RubroId { get; set; }
        [Range(0, double.MaxValue)] public decimal Monto { get; set; }
        public OrdenCompra? OrdenCompra { get; set; }
        public Proyecto? Proyecto { get; set; }
        public Rubro? Rubro { get; set; }
    }
}
