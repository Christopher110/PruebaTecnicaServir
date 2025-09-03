using System.ComponentModel.DataAnnotations;

namespace PruebaFullstack.Models
{
    public class Rubro
    {
        public int Id { get; set; }
        [Required] public int ProyectoId { get; set; }
        [Required, StringLength(150)] public string Nombre { get; set; } = "";
        public Proyecto? Proyecto { get; set; }
    }
}
