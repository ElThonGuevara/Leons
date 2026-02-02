using Leons.Data;
using Leons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Leons.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public UsuarioController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public async Task<IActionResult> Lista()
        {
            var usuarios = await _appDBContext.Usuarios.Include(u=>u.rol).ToListAsync(); // Fetching all users from the database
            return View(usuarios);
        }
        public async Task<IActionResult> Editar(int id)
        {
            ViewBag.Roles = await _appDBContext.Roles.ToListAsync(); // Fetching roles for the dropdown in the edit view
            var usuario = await _appDBContext.Usuarios.FirstOrDefaultAsync(u => u.idUsuario == id); // Fetching user details by ID
            if (usuario == null)
            {
                return NotFound(); // Return 404 if user not found
            }
            return View(usuario);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Usuario usuario)
        {
            if (usuario!=null)
            {
                _appDBContext.Usuarios.Update(usuario); // Update user details in the database
                await _appDBContext.SaveChangesAsync(); // Save changes
                return RedirectToAction("Lista","Usuario"); // Redirect to the user list after successful update
            }
            else
            {
                ModelState.AddModelError("", "El usuario no puede ser null"); // Add an error message if the user is null
                ViewBag.Roles = await _appDBContext.Roles.ToListAsync();
                return View(usuario); // Return the view with the current user data if model state is invalid
            }
                
        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var usuario = _appDBContext.Usuarios.FirstOrDefault(u => u.idUsuario==id);
            _appDBContext.Usuarios.Remove(usuario);
            _appDBContext.SaveChanges();
            return RedirectToAction("Lista", "Usuario");
        }
    }
}
