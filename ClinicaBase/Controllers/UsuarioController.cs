using ClinicaBase.Data;
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
        public async Task<IActionResult> Index()
        {
            GeneralResponse response = await _servicioUsuarios.FindAllUsers();
            if (response.Data != null)
            {
                List<UserDTO> usuarios = (List<UserDTO>)response.Data;
                UsuariosViewModel model = new()
                {
                    Usuarios = usuarios
                };
                return View(model);
            }
            return View();
        }
    }
}
