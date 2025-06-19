using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class DetalleCarrito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idDetalleCarrito { get; set; }
        public int cantidad { get; set; }

        public int idCarrito { get; set; }
        public Carrito carrito { get; set; }

        public int idProducto { get; set; }
        public Producto Producto { get; set; }
    }
}
