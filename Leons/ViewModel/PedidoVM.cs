using System.ComponentModel.DataAnnotations;

namespace Leons.ViewModel
{
    public class PedidoVM
    {
        [Required]
        public int idProducto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public string Talla { get; set; }

        public string Especificaciones { get; set; }

        public IFormFile ArchivoDiseno { get; set; }

        public int IdUsuario { get; set; } // capturado por sesión/autenticación
    }
}
