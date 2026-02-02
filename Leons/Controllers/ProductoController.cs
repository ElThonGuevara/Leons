using Leons.Data;
using Leons.Models;
using Leons.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leons.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductoController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public ProductoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public async Task< IActionResult> Lista()
        {
            var productos = await _appDBContext.Productos.Include(p=>p.categoria).ToListAsync(); // Assuming you have a DbSet<Productos> in your AppDBContext
            return View(productos);
        }
        public IActionResult Crear()
        {
            ViewBag.Categorias = _appDBContext.Categoria.ToList(); // Assuming you have a DbSet<Categoria> in your AppDBContext
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Crear(Producto producto)
        {
            if (producto!=null)
            {
                _appDBContext.Productos.Add(producto);
                await _appDBContext.SaveChangesAsync();
                return RedirectToAction("Lista", "Producto");
            }
            else
            {
                ModelState.AddModelError("", "El producto no puede ser nulo.");
                return View(producto);
            }
           
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            ViewBag.Categorias = _appDBContext.Categoria.ToList();
            var producto = _appDBContext.Productos.FirstOrDefault(p => p.idProducto == id);
            if (producto == null)
            {
                return NotFound(); // Return a 404 if the product is not found
            }
            return View(producto);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Producto producto)
        {
            if (producto != null)
            {
                _appDBContext.Productos.Update(producto);
                await _appDBContext.SaveChangesAsync();
                return RedirectToAction("Lista", "Producto");
            }
            else
            {
                ModelState.AddModelError("", "El producto no puede ser nulo.");
                ViewBag.Categorias = _appDBContext.Categoria.ToList();
                return View(producto);
            }
        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var producto = _appDBContext.Productos.FirstOrDefault(r => r.idProducto==id);
            _appDBContext.Productos.Remove(producto);
            _appDBContext.SaveChanges();
            return RedirectToAction("Lista", "Producto");
        }
    }
}
