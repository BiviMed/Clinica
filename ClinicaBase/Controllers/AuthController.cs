using ClinicaBase.Data;
using ClinicaBase.Models.DTOs;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;
using ClinicaBase.Services.ServicioUsuarios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicaBase.Controllers
{
    public class AuthController : Controller
    {
        private readonly IServicioUsuarios _servicioUsuarios;
        private readonly ClinicaBase1Context _context;

        public AuthController(IServicioUsuarios servicioUsuarios,ClinicaBase1Context context)
        {
            _servicioUsuarios = servicioUsuarios;
            _context = context;
        }

        [HttpGet]
        public IActionResult Inicio()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UsuarioAuthViewModel request)
        {
            GeneralResponse response = new();

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            response = await _servicioUsuarios.Auth(request);

            if (response.Succeed == 0)
            {
                request.Succeed = response.Succeed;
                request.Message = response.Message;
                return View(request);
            }

            var userClaims = (UserClaimsDTO)response.Data!;            

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userClaims.Documento.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, userClaims.Nombres.Trim() + " " + userClaims.Apellidos.Trim()));
            identity.AddClaim(new Claim(ClaimTypes.Role, userClaims.Rol));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddHours(1),
                    IsPersistent = true
                });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {            
            if (User.Identity!.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login");
            }
            return RedirectToAction("Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult<GeneralResponse>> Register(RegisterViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            GeneralResponse response = new();
            response = await _servicioUsuarios.AddUsuario(request);

            if (response.Succeed == 1)
            {
                return RedirectToAction("Index", "Home", response);
            }            
            request.Succeed = response.Succeed;
            request.Message = response.Message;
            return View(request);
        }
    }
}
