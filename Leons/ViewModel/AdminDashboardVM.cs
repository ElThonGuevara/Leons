using Leons.Models;

namespace Leons.ViewModel
{
    public class AdminDashboardVM
    {
        public int TotalPedidos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalProductos { get; set; }
        public List<Pedido> UltimosPedidos { get; set; }
    }
}
