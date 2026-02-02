using Leons.Data;
using Leons.Models;
using Leons.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leons.Controllers
{
    
    public class PedidoController : Controller
    {
        private readonly AppDBContext appDBContext;
        private readonly IWebHostEnvironment _env;


        public PedidoController(AppDBContext appDBContext, IWebHostEnvironment env)
        {
            this.appDBContext = appDBContext;
            _env = env;

        }
        public IActionResult Crear()
        {
            ViewBag.Productos = appDBContext.Productos.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PedidoVM pedidoVM)
        {
            if (ModelState.IsValid)
            {
                var pedido = new Pedido
                {
                    fechaPedido = DateOnly.FromDateTime(DateTime.Now),
                    estado = "Pendiente",
                    idUsuario = pedidoVM.IdUsuario
                };
                appDBContext.Pedidos.Add(pedido);
                await appDBContext.SaveChangesAsync();

                var detalle = new DetallePedido
                {
                    idPedido = pedido.idPedido,
                    idProducto = pedidoVM.idProducto,
                    cantidad = pedidoVM.Cantidad,
                    precioUnitario = appDBContext.Productos.Find(pedidoVM.idProducto)?.precio ?? 0
                };

                appDBContext.DetallePedidos.Add(detalle);
                await appDBContext.SaveChangesAsync();

                // Guardar archivo de diseño si se envía
                if (pedidoVM.ArchivoDiseno != null)
                {
                    var folderPath = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, pedidoVM.ArchivoDiseno.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await pedidoVM.ArchivoDiseno.CopyToAsync(stream);
                    }
                    // Aquí podrías guardar la ruta en la base de datos si deseas
                }

                TempData["mensaje"] = "Pedido enviado correctamente.";
                return RedirectToAction("Crear");
            }

            ViewBag.Productos = appDBContext.Productos.ToList();
            return View(pedidoVM);
        }

        public IActionResult Historial()
        {
            int idUsuario = ObtenerIdUsuarioActual(); // ← Reemplazar con ID real del usuario logueado

            var pedidos = appDBContext.Pedidos
                .Where(p => p.idUsuario == idUsuario)
                .Include(p => p.detallePedidos)
                    .ThenInclude(dp => dp.Producto)
                .OrderByDescending(p => p.fechaPedido)
                .ToList();

            return View(pedidos);
        }

        public IActionResult Detalles(int id)
        {
            var pedido = appDBContext.Pedidos
                .Include(p => p.detallePedidos)
                    .ThenInclude(dp => dp.Producto)
                .FirstOrDefault(p => p.idPedido == id);

            if (pedido == null)
                return NotFound();

            return View(pedido);
        }

        public int ObtenerIdUsuarioActual()
        {
            //lógica para obtener el id mediante el login
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value); ;
        }
    }
}
