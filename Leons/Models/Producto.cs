using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace Leons.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idProducto { get; set; }
        [Required]
        public string nombre { get; set; }
        public string? imagenLocal { get; set; }
        public string? imagen { get; set; } // URL o ruta de la imagen del producto
        public string? descripcion { get; set; }
        [Required]
        public decimal precio { get; set; }
        [Required]
        public int stock { get; set; }
        [Required]
        public string talla { get; set; }
        public string? genero { get; set; } // Masculino, Femenino, Unisex, etc.


        [Required]
        public int idCategoria { get; set; }
        public Categoria? categoria { get; set; }


        [BindNever]
        public ICollection<DetallePedido> detallePedidos { get; set; } = new List<DetallePedido>();
        [BindNever]
        public ICollection<DetalleCarrito> detalleCarrito { get; set; } = new List<DetalleCarrito>();

        [NotMapped]
        public string? ImagenSeleccionada { get; set; }


        // 🔁 Imagen final (NO se guarda en BD)
        [NotMapped]
        public string ImagenFinal =>
            !string.IsNullOrEmpty(imagenLocal)
                ? $"/img/productos/{imagenLocal}"
                : imagen ?? "/img/no-image.png";



    }
}
