using Leons.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Leons.Controllers
{
    [Authorize(Roles = "cliente, admin")]
    public class TiendaController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public TiendaController(AppDBContext appDBContext)
        {
            _appDBContext=appDBContext;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _appDBContext.Categoria.ToListAsync();
            return View(categorias);
        }
        public IActionResult ProductosPorCategoria(int id)
        {
            var productos = _appDBContext.Productos
                .Include(p => p.categoria)
                .Where(p => p.idCategoria == id)
                .ToList();

            ViewBag.Categoria = _appDBContext.Categoria.Find(id)?.nombre ?? "Productos";
            return View(productos);
        }
        public  IActionResult Hombre()
        {
            var productoHombres =  _appDBContext.Productos
                .Include(p => p.categoria)
                .Where(p => p.genero.ToLower()== "hombre".ToLower()).ToList();
            if (productoHombres == null)
            {
                return NotFound();
            }
            return View(productoHombres);
        }
        public IActionResult Mujer()
        {
            var productoMujeres = _appDBContext.Productos
                .Include(p => p.categoria)
                .Where(p => p.genero.ToLower()== "mujer".ToLower()).ToList();
            if (productoMujeres == null)
            {
                return NotFound();
            }
            return View(productoMujeres);
        }
    }
}
