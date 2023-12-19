using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CategoriaDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El número máximo de caracteres es 100!")]
        public string Nombre { get; set; }
    }
}
