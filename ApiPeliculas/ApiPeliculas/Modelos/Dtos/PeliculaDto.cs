using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class PeliculaDto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El número máximo de caracteres es 100!")]
        public string Nombre { get; set; }
        public byte[] RutaImagen { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La duración es obligatoria")]
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int categoriaId { get; set; }
    }
}
