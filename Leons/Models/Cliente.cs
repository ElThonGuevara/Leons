using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCliente { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        // Otros campos específicos del cliente pueden ser añadidos aquí
        public int idUsuario { get; set; }
        public Usuario usuario { get; set; }

        public ICollection<Pedido> pedidos { get; set; } 
        public ICollection<Carrito> carritos { get; set; }
    }
}
