using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Carrito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCarrito { get; set; }

        public int idCliente { get; set; }  
        public Cliente cliente { get; set; }

        public ICollection<DetalleCarrito> detalleCarrito { get; set; }
    }
}
