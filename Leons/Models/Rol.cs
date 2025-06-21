using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idRol { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }

        public ICollection<Usuario> usuarios { get; set; }
        
    }
}
