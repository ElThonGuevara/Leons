using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Pago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPago { get; set; }
        public decimal montoTotal { get; set; }
        public DateTime fechaPago { get; set; }
        public string metodoPago { get; set; } // Ejemplo: "Tarjeta de Crédito", "PayPal", etc.
        public string estado { get; set; } // Ejemplo: "Completado", "Pendiente", "Fallido"
        // Relación con Pedido

        public int idPedido { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}
