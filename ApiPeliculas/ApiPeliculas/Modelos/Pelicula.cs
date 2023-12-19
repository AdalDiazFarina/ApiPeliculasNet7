using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre{ get; set; }
        public byte[] RutaImagen { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho}
        public TipoClasificacion Clasificacion {  get; set; }
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("categoriaId")]
        public int categoriaId {  get; set; }
        public Categoria Categoria { get; set; }   
    }
}
