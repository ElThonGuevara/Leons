using Leons.Data;
using Leons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Leons.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _appDBContext;

        public HomeController(ILogger<HomeController> logger, AppDBContext appDBContext)
        {
            _logger = logger;
            _appDBContext=appDBContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        //public async Task<IActionResult> Tienda()
        //{
        //    var productos = await _appDBContext.Productos.Include(p => p.categoria).ToListAsync();
        //    return View(productos);
        //}
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
