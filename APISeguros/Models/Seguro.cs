using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace APISeguros.Models
{
    public class Seguro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string Nombre {  get; set; }

        public string? SegundoNombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string SegundoApellido { get; set; }

        [Required]
        public string NumeroIdentificacion { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public double Valor {  get; set; }

        [AllowNull]
        public string Observacion { get; set; }

    }
}
