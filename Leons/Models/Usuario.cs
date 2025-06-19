using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }
        public string ? nombreUsuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string ? password { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }

        public int idRol {  get; set; }
        public Rol rol { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
        public ICollection<Carrito> Carritos { get; set; }
    
    }
}
