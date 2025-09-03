using System.ComponentModel.DataAnnotations;

namespace PruebaFullstack.Models
{
    public class Donacion
    {
        public int Id { get; set; }
        [Required] public int ProyectoId { get; set; }
        public int? RubroId { get; set; }
        [Required, DataType(DataType.Date)] public DateTime Fecha { get; set; } = DateTime.Today;
        [Required, StringLength(200)] public string Donante { get; set; } = "";
        [Range(0, double.MaxValue)] public decimal Monto { get; set; }
        public Proyecto? Proyecto { get; set; }
        public Rubro? Rubro { get; set; }
    }
}
