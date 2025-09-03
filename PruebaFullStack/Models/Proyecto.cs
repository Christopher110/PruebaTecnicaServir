using System.ComponentModel.DataAnnotations;

namespace PruebaFullstack.Models
{
    public class Proyecto
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Nombre { get; set; } = "";

        [Required, StringLength(100)]
        public string Municipio { get; set; } = "";

        [Required, StringLength(100)]
        public string Departamento { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        public string Codigo => $"P-{Id:D4}";
    }
}
