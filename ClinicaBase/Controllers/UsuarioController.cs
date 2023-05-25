using ClinicaBase.Models.DTOs;
using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;
using ClinicaBase.Services.ServicioUsuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            GeneralResponse response = await _servicioUsuarios.DeleteUser(request);
            return RedirectToAction("Index", response);

        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recursos Humanos")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
