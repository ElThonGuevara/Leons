using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }
        public string ? email { get; set; }
        public string ? password { get; set; }

        public int idRol {  get; set; }
        public Rol rol { get; set; }

        public ICollection<Administrador> Administradores { get; set; }
        public ICollection<Cliente> Clientes { get; set; }
    
    }
}
