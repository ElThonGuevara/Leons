using Leons.Data;
using Leons.Models;
using Leons.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Leons.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public AccesoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Registro()
        {
            ViewBag.Roles = _appDBContext.Roles.ToList(); // Assuming you want to list roles in the view
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroVM registroVM)
        {
            Usuario? usuario = await _appDBContext.Usuarios.FirstOrDefaultAsync(u => u.email == registroVM.email);
            if(usuario == null)
            {
                if (registroVM.password==registroVM.confirmarPassword)
                {
                    usuario=new Usuario
                    {
                        nombreUsuario = registroVM.nombreUsuario,
                        nombre = registroVM.nombre,
                        apellido = registroVM.apellido,
                        email = registroVM.email,
                        password = registroVM.password, // Consider hashing the password before saving
                        telefono = registroVM.telefono,
                        direccion = registroVM.direccion,
                        idRol = registroVM.idRol // Assuming 2 is the ID for a regular user role
                    };
                    _appDBContext.Usuarios.Add(usuario);
                    await _appDBContext.SaveChangesAsync();
                    return RedirectToAction("Login", "Acceso");
                }
                ViewBag.Roles = _appDBContext.Roles.ToList();
                ModelState.AddModelError("", "Las contraseñas no coinciden");
                return View(registroVM);

            }
            else
            {
                ViewBag.Roles = _appDBContext.Roles.ToList();
                ModelState.AddModelError("email", "El email ya está registrado.");
                return View(registroVM);
            }
            
        }
        [HttpGet]
        public IActionResult RegistroCliente()
        {
            return View();
        }
        public async Task<IActionResult> RegistroCliente(RegistroVM registroVM)
        {
            Usuario? usuario = await _appDBContext.Usuarios.FirstOrDefaultAsync(u => u.email == registroVM.email);
            var rolUsuario = await _appDBContext.Roles.FirstOrDefaultAsync(r => r.nombre.ToLower() == "cliente".ToLower()); // Assuming "Usuario" is the role name for regular users
            if (usuario == null)
            {
                if(registroVM.password==registroVM.confirmarPassword)
                {
                    usuario=new Usuario
                    {
                        nombreUsuario = registroVM.nombreUsuario,
                        nombre = registroVM.nombre,
                        apellido = registroVM.apellido,
                        email = registroVM.email,
                        password = registroVM.password, // Consider hashing the password before saving
                        telefono = registroVM.telefono,
                        direccion = registroVM.direccion,
                        idRol = rolUsuario.idRol  // Assuming 2 is the ID for a regular user role
                    };
                    _appDBContext.Usuarios.Add(usuario);
                    await _appDBContext.SaveChangesAsync();
                    return RedirectToAction("Login", "Acceso");
                    
                }
                ModelState.AddModelError("confirmarPassword", "Las contraseñas no coinciden.");
                return View(registroVM);

            }
            else
            {
                ModelState.AddModelError("email", "El email ya está registrado.");
                return View(registroVM);
            }
                //if(rolUsuario== null)
                //{
                //    ModelState.AddModelError("rol", "El rol de usuario no existe.");
                //    return View(registroVM);
                //}
                
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            Usuario? usuario = await _appDBContext.Usuarios.Include(u => u.rol).FirstOrDefaultAsync(u => u.email == loginVM.email && u.password == loginVM.password);
            if (usuario == null)
            {
                ModelState.AddModelError("email", "Email o contraseña incorrectos.");
                return View();
            }
            // Aquí podrías establecer una sesión o cookie para el usuario autenticado
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, usuario.idUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.nombre),
                new Claim(ClaimTypes.Role, usuario.rol.nombre) // Assuming rol has a property 'nombre'
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");

        }
    }
}
