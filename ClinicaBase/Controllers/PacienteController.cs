using ClinicaBase.Data;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;
using ClinicaBase.Services.ServicioPacientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicaBase.Controllers
{
    [Authorize]
    public class PacienteController : Controller
    {
        private readonly ClinicaBase1Context _context;
        private readonly IServicioPaciente _servicioPaciente;        

        public PacienteController(ClinicaBase1Context context,
            IServicioPaciente servicioPaciente)
        {
            _context = context;
            _servicioPaciente = servicioPaciente;
        }


        [Authorize(Roles = "Admin,Medico,Enfermeria")]
        [HttpGet]
        public IActionResult Agregar()
        {
            return View();
        }


        [Authorize(Roles = "Admin,Medico,Enfermeria")]
        [HttpPost]
        public async Task<IActionResult> Agregar(PacienteViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }            
            try
            {
                request.FechaCreacion = DateTime.Now;
                request.UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception )
            {
                request.Succeeded = 0;
                request.Mensaje = "Ha ocurrido un error inesperado";
                return View(request);
            }

            GeneralResponse response = await _servicioPaciente.AgregarHistoriaClinica(request);
            request.Succeeded = response.Succeed;
            request.Mensaje = response.Message;
            if (response.Succeed == 0)
            {                
                return View(request);
            }
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Admin, Enfermeria")]
        [HttpGet]
        public IActionResult ActualizarInfo()
        {
            return View();
        }

    }
}
