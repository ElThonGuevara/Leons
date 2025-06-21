using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public string talla { get; set; }


        public int idCategoria { get; set; }
        public Categoria categoria { get; set; }
        public ICollection<DetallePedido> detallePedidos { get; set; } 
        public ICollection<DetalleCarrito> detalleCarrito { get; set; }
        
    }
}
