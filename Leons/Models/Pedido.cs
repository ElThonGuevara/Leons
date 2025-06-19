using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPedido { get; set; }
        public DateOnly fechaPedido { get; set; }
        public string estado { get; set; } // Ejemplo: "Pendiente", "Enviado", "Entregado", etc.
        
        public int idUsuario { get; set; }
        public Usuario usuario { get; set; }

        public ICollection<DetallePedido> detallePedidos { get; set; }
    }
}
