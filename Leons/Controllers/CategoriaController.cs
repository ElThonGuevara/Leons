using Leons.Data;
using Leons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leons.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoriaController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public CategoriaController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Lista()
        {
            var categorias = _appDBContext.Categoria.ToList(); // Assuming you have a DbSet<Categoria> in your AppDBContext
            return View(categorias);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (categoria != null)
            {
                _appDBContext.Categoria.Add(categoria);
                await _appDBContext.SaveChangesAsync();
                return RedirectToAction("Lista", "Categoria");
            }
            else
            {
                ModelState.AddModelError("", "La categoría no puede ser nula.");
                return View(categoria);
            }
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var categoria = _appDBContext.Categoria.FirstOrDefault(c => c.idCategoria == id);
            if (categoria == null)
            {
                return NotFound(); // Return a 404 if the category is not found
            }
            return View(categoria);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            if (categoria != null)
            {
                _appDBContext.Categoria.Update(categoria);
                await _appDBContext.SaveChangesAsync();
                return RedirectToAction("Lista", "Categoria");
            }
            else
            {
                ModelState.AddModelError("", "La categoría no puede ser nula.");
                return View(categoria);
            }
        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var categoria = _appDBContext.Categoria.FirstOrDefault(c => c.idCategoria == id);
             _appDBContext.Categoria.Remove(categoria);
            _appDBContext.SaveChanges();
            return RedirectToAction("Lista", "Categoria");
        }
    }
}
