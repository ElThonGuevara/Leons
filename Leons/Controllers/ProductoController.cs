using Leons.Data;
using Leons.Models;
using Leons.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Leons.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        private readonly IWebHostEnvironment _env;

        public ProductoController(AppDBContext appDBContext, IWebHostEnvironment env)
        {
            _appDBContext = appDBContext;
            _env = env;
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
        public async Task< IActionResult> Crear(Producto producto, IFormFile imagen)
        {

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(errores);
            }

            if (imagen != null && imagen.Length > 0)
            {
                string extension = Path.GetExtension(imagen.FileName);
                string nombreProducto = LimpiarTexto(producto.nombre);
                string genero = LimpiarTexto(producto.genero ?? "general");

                string nombreArchivo = $"{nombreProducto}-{genero}-{DateTime.Now.Ticks}{extension}";

                string carpeta = Path.Combine(
                    _env.WebRootPath,
                    "img/productos"
                );

                // ✔ Crear carpeta si no existe
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                string ruta = Path.Combine(carpeta, nombreArchivo);

                using var stream = new FileStream(ruta, FileMode.Create);
                await imagen.CopyToAsync(stream);

                producto.imagenLocal = nombreArchivo;
            }

            _appDBContext.Productos.Add(producto);
            await _appDBContext.SaveChangesAsync();

            return RedirectToAction("Lista", "Producto");
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

            string ruta = Path.Combine(_env.WebRootPath, "img/productos");

            ViewBag.Imagenes = Directory
                .GetFiles(ruta)
                .Select(Path.GetFileName)
                .ToList();

            return View(producto);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Producto producto, IFormFile? imagenNueva)
        {
            var productoDB = await _appDBContext.Productos.FirstOrDefaultAsync(p => p.idProducto == producto.idProducto);

            if (productoDB == null) return NotFound();

            // Campos normales
            productoDB.nombre = producto.nombre;
            productoDB.precio = producto.precio;
            productoDB.stock = producto.stock;
            productoDB.talla = producto.talla;
            productoDB.genero = producto.genero;
            productoDB.descripcion = producto.descripcion;
            productoDB.idCategoria = producto.idCategoria;

            // 🖼️ CASO 1: sube nueva imagen
            if (imagenNueva != null && imagenNueva.Length > 0)
            {
                string ext = Path.GetExtension(imagenNueva.FileName);
                string nombreProducto = LimpiarTexto(producto.nombre);
                string genero = LimpiarTexto(producto.genero ?? "general");

                string nombreArchivo = $"{nombreProducto}-{genero}-{DateTime.Now.Ticks}{ext}";

                string ruta = Path.Combine(
                    _env.WebRootPath,
                    "img/productos",
                    nombreArchivo
                );

                using var stream = new FileStream(ruta, FileMode.Create);
                await imagenNueva.CopyToAsync(stream);

                producto.imagenLocal = nombreArchivo;
            }
            // 🖼️ CASO 2: elige imagen existente
            else if (!string.IsNullOrEmpty(producto.ImagenSeleccionada))
            {
                productoDB.imagenLocal = producto.ImagenSeleccionada;
            }
            // 🖼️ CASO 3: no cambia imagen → no se toca nada

            await _appDBContext.SaveChangesAsync();
            return RedirectToAction("Lista");
        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var producto = _appDBContext.Productos.FirstOrDefault(r => r.idProducto==id);
            _appDBContext.Productos.Remove(producto);
            _appDBContext.SaveChanges();
            return RedirectToAction("Lista", "Producto");
        }

        string LimpiarTexto(string texto)
        {
                texto = texto.ToLower();
                texto = Regex.Replace(texto, @"[^a-z0-9\s-]", "");
                texto = texto.Replace(" ", "-");
                return texto;
        }

    }
}
