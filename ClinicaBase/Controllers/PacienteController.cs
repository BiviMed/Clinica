using AutoMapper;
using Azure.Core;
using ClinicaBase.Data;
using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;
using ClinicaBase.Services.ServicioPacientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml;

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
            return RedirectToAction("Index", "Home", response);
        }

        [Authorize(Roles = "Admin, Enfermeria")]
        [HttpGet]
        public IActionResult BuscarPaciente()
        {            
            return View();
        }

        [Authorize(Roles = "Admin, Enfermeria")]
        [HttpPost]
        public async Task<IActionResult> BuscarPaciente(BuscarPacienteViewModel request)
        {
            if (request.Documento == null && request.Nombres == null && request.Apellidos == null)
            {
                request.Succeeded = 0;
                request.Message = "Debe llenar por lo menos un campo para buscar al paciente.";
                return View(request);
            }

            GeneralResponse response = await _servicioPaciente.BuscarPacientes(request);
            if (response.Succeed == 0)
            {
                request.Succeeded = response.Succeed;
                request.Message = response.Message;
            }
            else
            {
                request.Succeeded = response.Succeed;
                request.Pacientes = (List<Patient>)response.Data!;
            }
            return View(request);
        }


        [Authorize(Roles = "Admin, Medico, Enfermeria")]
        [HttpGet]
        public async Task<IActionResult> ActualizarInfo(int documento)
        {
            GeneralResponse response = await _servicioPaciente.BuscarPaciente(documento);
            if (response.Succeed == 0)
            {
                return RedirectToAction("BuscarPaciente", "Paciente");
            }

            Patient paciente = (Patient)response.Data!;

            _servicioPaciente.MapearPacienteAEditarPaciente(paciente, out EditarPacieteViewModel editarPaciente);
            
            return View(editarPaciente);
        }

        [Authorize(Roles = "Admin, Medico, Enfermeria")]
        [HttpPost]
        public async Task<IActionResult> ActualizarInfo(EditarPacieteViewModel editarRequest)
        {
            GeneralResponse response = new();
            response = await _servicioPaciente.BuscarPaciente(editarRequest.Documento);

            if (response.Succeed == 0)
            {
                editarRequest.Succeeded = response.Succeed;
                editarRequest.Message = response.Message;
                return View(editarRequest);
            }
            Patient paciente = (Patient)response.Data!;
            _servicioPaciente.MapearEditarPacienteAPaciente(paciente, editarRequest, out Patient pacienteActualizado);

            if (pacienteActualizado == null)
            {
                editarRequest.Succeeded = 0;
                editarRequest.Message = "Se ha generado un error inesperado";
                return View(editarRequest);
            }
            response = await _servicioPaciente.ActializarInformacion(pacienteActualizado);

            if (response.Succeed == 0)
            {
                editarRequest.Succeeded = response.Succeed;
                editarRequest.Message = response.Message;
                return View(editarRequest);
            }
            return RedirectToAction("Index", "Home", response);
        }

    }
}
