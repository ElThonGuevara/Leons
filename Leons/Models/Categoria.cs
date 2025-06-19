using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leons.Models
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCategoria { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        // Relación con productos
        public ICollection<Producto> productos { get; set; } 
    }
}
