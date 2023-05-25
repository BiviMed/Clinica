using ClinicaBase.Models.DTOs;
using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;
using ClinicaBase.Services.ServicioUsuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicaBase.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IServicioUsuarios _servicioUsuarios;

        public UsuarioController(IServicioUsuarios servicioUsuarios)
        {
            _servicioUsuarios = servicioUsuarios;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public async Task<IActionResult> Index(GeneralResponse? responseRequest)
        {
            UsuariosViewModel model = new();
            GeneralResponse response = await _servicioUsuarios.FindAllUsers();
            if (response.Data != null)
            {
                List<UserDTO> usuarios = (List<UserDTO>)response.Data;                
                model.Usuarios = usuarios;

                if (responseRequest != null)
                {
                    model.Exitoso = responseRequest.Succeed;
                    model.Mensaje = responseRequest.Message;
                }
                return View(model);
            }

            if (responseRequest != null)
            {
                model.Exitoso = responseRequest.Succeed;
                model.Mensaje = responseRequest.Message;
                return View(model);
            }
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public IActionResult Register()
        {
            RegisterViewModel model = new();
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public async Task<ActionResult<GeneralResponse>> Register(RegisterViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
             
            string rolUsuarioAutenticado = User.FindFirstValue(ClaimTypes.Role);
            if (rolUsuarioAutenticado != "Admin" && request.Rol == "Admin")
            {
                request.Succeed = 0;
                request.Message = "No tiene permiso para crear a un Administrador";
                return View(request);
            }
            GeneralResponse response = await _servicioUsuarios.AddUsuario(request);
            if (response.Succeed == 1)
            {
                return RedirectToAction("Index", "Home", response);
            }
            request.Succeed = response.Succeed;
            request.Message = response.Message;
            return View(request);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public async Task<IActionResult> Editar(int documento)
        {
            User? usuario = await _servicioUsuarios.FindById(documento);
            if (usuario != null)
            {
                EditarUsuarioViewModel editarUsuario = new()
                {
                    Documento = usuario.Documento,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Rol = usuario.Rol
                };
                return View(editarUsuario);
            }
            return RedirectToAction("Error");
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public async Task<IActionResult> Editar(EditarUsuarioViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            string rolUsuarioAutenticado = User.FindFirstValue(ClaimTypes.Role);
            GeneralResponse response = await _servicioUsuarios.EditarUsuario(request, rolUsuarioAutenticado);
            if (response.Succeed == 0)
            {
                request.Succeeded = 0;
                request.Message = response.Message;
                return View(request);
            }
            
            return RedirectToAction("Index", response);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public async Task<IActionResult> Eliminar(int documento)
        {
            User? usuario = await _servicioUsuarios.FindById(documento);
            if (usuario == null)
            {
                return RedirectToAction("Error");
            }
            EliminarUsuarioViewModel eliminarUsuario = new()
            {
                Documento = usuario.Documento,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos
            };
            return View(eliminarUsuario);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public async Task<IActionResult> Eliminar(EliminarUsuarioViewModel? request)
        {
            if (request == null || request.Documento == 0)
            {
                return RedirectToAction("Error");
            }
            string rolUsuarioAutenticado = User.FindFirstValue(ClaimTypes.Role);
            GeneralResponse response = await _servicioUsuarios.DeleteUser(request, rolUsuarioAutenticado);
            return RedirectToAction("Index", response);

        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Perfil(GeneralResponse? response)
        {
            int documento = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User? usuarioAutenticado = await _servicioUsuarios.FindById(documento);
            if (usuarioAutenticado == null)
            {
                return RedirectToAction("Error");
            }

            PerfilViewModel perfil = new()
            {
                Documento = usuarioAutenticado.Documento,
                Nombres = usuarioAutenticado.Nombres,
                Apellidos = usuarioAutenticado.Apellidos,
                Correo = usuarioAutenticado.Correo,
                Telefono = usuarioAutenticado.Telefono,
                Rol = usuarioAutenticado.Rol
            };

            if (response != null)
            {
                perfil.Succeeded = response.Succeed;
                perfil.Message = response.Message;
            }
            return View(perfil);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditarPerfil(PerfilViewModel request)
        {
            int documentoUsuarioAutenticado = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            GeneralResponse response = await _servicioUsuarios.EditarPerfil(request, documentoUsuarioAutenticado);
            return RedirectToAction("Perfil", response);
        
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
