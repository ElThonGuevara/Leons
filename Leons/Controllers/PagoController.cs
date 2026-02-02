using Leons.Data;
using Leons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leons.Controllers
{
    [Authorize(Roles = "cliente")]
    public class PagoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public PagoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Crear()
        {
            int idUsuario = ObtenerIdUsuarioActual();

            // Obtén el carrito y sus productos
            var carrito = _appDBContext.Carritos
                .Include(c => c.detalleCarrito)
                .ThenInclude(dc => dc.Producto)
                .FirstOrDefault(c => c.idUsuario == idUsuario);

            if (carrito == null || !carrito.detalleCarrito.Any())
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction("Index", "Carrito");
            }

            ViewBag.MontoTotal = carrito.detalleCarrito.Sum(dc => dc.cantidad * dc.Producto.precio);
            ViewBag.Carrito = carrito;
            //----------------------------------
            //var pedido = _appDBContext.Pedidos
            //    .Include(p => p.detallePedidos)
            //    .ThenInclude(dp => dp.Producto)
            //    .FirstOrDefault(p => p.idPedido == idPedido);

            //if (pedido == null)
            //    return NotFound();

            //ViewBag.IdPedido = idPedido;
            //ViewBag.MontoTotal = pedido.detallePedidos.Sum(p => p.cantidad * p.precioUnitario);

            return View();
        }
        [HttpPost]
        public IActionResult Crear(string metodoPago)
        {
            int idUsuario = ObtenerIdUsuarioActual(); // Simulación hasta tener sesión

            // Obtener el carrito con sus productos
            var carrito = _appDBContext.Carritos
                .Include(c => c.detalleCarrito)
                    .ThenInclude(dc => dc.Producto)
                .FirstOrDefault(c => c.idUsuario == idUsuario);

            if (carrito == null || !carrito.detalleCarrito.Any())
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction("Index");
            }
            //Calcula el total
            var total = carrito.detalleCarrito.Sum(dc => dc.cantidad * dc.Producto.precio);

            // Simula confirmación de pago aquí (debería ser tu lógica real de pago)
            bool pagoExitoso = true; // Cambia esto según tu lógica real

            if (pagoExitoso)
            {
                // Crear el pedido
                var pedido = new Pedido
                {
                    fechaPedido = DateOnly.FromDateTime(DateTime.Now),
                    estado = "Pendiente",
                    idUsuario = idUsuario
                };

                _appDBContext.Pedidos.Add(pedido);
                _appDBContext.SaveChanges();

                // Crear los detalles del pedido
                foreach (var item in carrito.detalleCarrito)
                {
                    var detalle = new DetallePedido
                    {
                        idPedido = pedido.idPedido,
                        idProducto = item.idProducto,
                        cantidad = item.cantidad,
                        precioUnitario = item.Producto.precio
                    };

                    _appDBContext.DetallePedidos.Add(detalle);
                }

                _appDBContext.SaveChanges();

                //Registrar pago
                var pago = new Pago
                {
                    idPedido = pedido.idPedido,
                    metodoPago = metodoPago,
                    montoTotal = total,
                    estado = "Completado",
                    fechaPago = DateTime.Now
                };

                _appDBContext.Pagos.Add(pago);
                _appDBContext.SaveChanges();

                // Eliminar los ítems del carrito
                _appDBContext.DetalleCarritos.RemoveRange(carrito.detalleCarrito);
                _appDBContext.SaveChanges();

                return RedirectToAction("Confirmacion");
            }
            else
            {
                TempData["Error"] = "El pago no se pudo procesar.";
                return RedirectToAction("Crear");
            }
            //----------------------------------------
            //var pedido = _appDBContext.Pedidos.FirstOrDefault(p => p.idPedido == idPedido);
            //if (pedido == null) return NotFound();

            //var total = _appDBContext.DetallePedidos
            //    .Where(dp => dp.idPedido == idPedido)
            //    .Sum(dp => dp.cantidad * dp.precioUnitario);
        }

        private int ObtenerIdUsuarioActual()
        {
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }

        public IActionResult Confirmacion()
        {
            return View();
        }

    }
}
