using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class DetallePedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idDetallePedido { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; } // Precio del producto al momento del pedido

        public int idPedido { get; set; }
        public Pedido pedido { get; set; }

        public int idProducto { get; set; }
        public Producto Producto { get; set; }


    }
}
