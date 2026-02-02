namespace Leons.ViewModel
{
    public class ProductoVM
    {
        public string nombre { get; set; }
        public string? imagen { get; set; } // URL o ruta de la imagen del producto
        public string? descripcion { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public string talla { get; set; }
        public int idCategoria { get; set; }
    }
}
