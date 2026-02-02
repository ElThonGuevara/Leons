using Leons.Data;
using Leons.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leons.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public AdminController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Dashboard()
        {
            var resumen = new AdminDashboardVM()
            {
                TotalPedidos = _appDBContext.Pedidos.Count(),
                TotalClientes = _appDBContext.Usuarios.Count(u => u.idRol == 2), // su rol de cliente
                TotalProductos = _appDBContext.Productos.Count(),
                UltimosPedidos = _appDBContext.Pedidos.Include(p => p.usuario)
                .OrderByDescending(p => p.fechaPedido)
                .Take(5)
                .ToList()
            };
            return View(resumen);
        }
    }
}
