using Leons.Data;
using Leons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leons.Controllers
{
    [Authorize(Roles = "cliente, admin")]
    public class CarritoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public CarritoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Index()
        {
            int idUsuario = ObtenerIdUsuarioActual();

            var carrito = _appDBContext.Carritos
                .Include(c => c.detalleCarrito)
                .ThenInclude(dc => dc.Producto)
                .FirstOrDefault(c => c.idUsuario == idUsuario);
            return View(carrito);
        }
        // Agregar producto al carrito
        [HttpPost]
        public JsonResult Agregar(int idProducto, int cantidad)
        {
            int idUsuario = ObtenerIdUsuarioActual();
            var carrito = _appDBContext.Carritos.FirstOrDefault(c => c.idUsuario == idUsuario);

            if (carrito == null)
            {
                carrito = new Carrito { idUsuario = idUsuario };
                _appDBContext.Carritos.Add(carrito);
                _appDBContext.SaveChanges();
            }

            var detalle = _appDBContext.DetalleCarritos
                .FirstOrDefault(dc => dc.idCarrito == carrito.idCarrito && dc.idProducto == idProducto);

            if (detalle != null)
            {
                detalle.cantidad+=cantidad;
            }
            else
            {
                detalle = new DetalleCarrito
                {
                    idCarrito = carrito.idCarrito,
                    idProducto = idProducto,
                    cantidad = cantidad
                };
                _appDBContext.DetalleCarritos.Add(detalle);
            }

            _appDBContext.SaveChanges();

            return Json(new { success = true, message = "Producto agregado al carrito." });
        }
        // Eliminar producto
        public IActionResult Eliminar(int idDetalleCarrito)
        {
            var item = _appDBContext.DetalleCarritos.Find(idDetalleCarrito);
            if (item != null)
            {
                _appDBContext.DetalleCarritos.Remove(item);
                _appDBContext.SaveChanges();
            }

            return RedirectToAction("Index", "Carrito");
        }

        [HttpPost]
        public IActionResult FinalizarPedido()
        {

            //int idUsuario = ObtenerIdUsuarioActual(); // Simulación hasta tener sesión

            //// Obtener el carrito con sus productos
            //var carrito = _appDBContext.Carritos
            //    .Include(c => c.detalleCarrito)
            //        .ThenInclude(dc => dc.Producto)
            //    .FirstOrDefault(c => c.idUsuario == idUsuario);

            //if (carrito == null || !carrito.detalleCarrito.Any())
            //{
            //    TempData["Error"] = "El carrito está vacío.";
            //    return RedirectToAction("Index");
            //}

            //// Crear el pedido
            //var pedido = new Pedido
            //{
            //    fechaPedido = DateOnly.FromDateTime( DateTime.Now),
            //    estado = "Pendiente",
            //    idUsuario = idUsuario
            //};

            //_appDBContext.Pedidos.Add(pedido);
            //_appDBContext.SaveChanges();

            //// Crear los detalles del pedido
            //foreach (var item in carrito.detalleCarrito)
            //{
            //    var detalle = new DetallePedido
            //    {
            //        idPedido = pedido.idPedido,
            //        idProducto = item.idProducto,
            //        cantidad = item.cantidad,
            //        precioUnitario = item.Producto.precio
            //    };

            //    _appDBContext.DetallePedidos.Add(detalle);
            //}

            //_appDBContext.SaveChanges();

            //// Eliminar los ítems del carrito
            //_appDBContext.DetalleCarritos.RemoveRange(carrito.detalleCarrito);
            //_appDBContext.SaveChanges();

            // Redirigir al pago
            return RedirectToAction("Crear", "Pago");
        }


        private int ObtenerIdUsuarioActual()
        {
            // Simulado. Luego integrar con sesión o login real
            return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }
    }
}
