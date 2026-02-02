using Leons.Data;
using Leons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leons.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public RolesController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public IActionResult Lista()
        {
            var roles = _appDBContext.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Rol rol)
        {
            if (rol!=null)
            {
                _appDBContext.Roles.Add(rol);
                await _appDBContext.SaveChangesAsync();
                return RedirectToAction("Lista","Roles");
            }
            else
            {
                ModelState.AddModelError("", "El rol no puede ser nulo.");
                return View(rol);
            }
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var rol = _appDBContext.Roles.FirstOrDefault(r=>r.idRol == id);
            return View(rol);
        }
            
        [HttpPost]
        public async Task< IActionResult> Editar(Rol rol)
        {
            if (rol!=null)
            {
                _appDBContext.Roles.Update(rol);
                await _appDBContext.SaveChangesAsync();
                return RedirectToAction("Lista","Roles");
            }
            else
            {
                ModelState.AddModelError("","El rol no puede ser nulo.");
                return View(rol);

            }

        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var rol = _appDBContext.Roles.FirstOrDefault(r => r.idRol==id);
            _appDBContext.Roles.Remove(rol);
            _appDBContext.SaveChanges();
            return RedirectToAction("Lista","Roles");
        }

    }
}
