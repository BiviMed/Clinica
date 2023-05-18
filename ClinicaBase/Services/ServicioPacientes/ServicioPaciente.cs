using AutoMapper;
using ClinicaBase.Data;
using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;
using ClinicaBase.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicaBase.Services.ServicioPacientes
{
    public class ServicioPaciente : IServicioPaciente
    {
        private readonly ClinicaBase1Context _context;
        private readonly IMapper _mapper;

        public ServicioPaciente(ClinicaBase1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GeneralResponse> AgregarHistoriaClinica(PacienteViewModel request)
        {
            GeneralResponse response = new();

            var existenciaPaciente = await _context.Patients.Where(p => p.Documento == request.Documento).FirstOrDefaultAsync();
            if (existenciaPaciente != null)
            {
                response.Message = "A este paciente ya se le ha abierto una historia clínica.";
            }
            else
            {                
                try
                {
                    Patient paciente = _mapper.Map<Patient>(request);
                    await _context.Patients.AddAsync(paciente);
                    await _context.SaveChangesAsync();
                    response.Succeed = 1;
                    response.Message = "Historia del paciente añadida con éxito.";
                }
                catch (Exception)
                {
                    response.Message = "Ha ocurrido un error al momento de guardar al Paciente.";
                }   
            }
            return response;
        }

        public async Task<GeneralResponse> BuscarPaciente(BuscarPacienteViewModel pacienteViewModel)
        {
            GeneralResponse response = new();
            List<Patient>? paciente;

            if (pacienteViewModel.Documento != null && pacienteViewModel.Documento != 0)
            {
                paciente = await _context.Patients.Where(p => p.Documento == pacienteViewModel.Documento).ToListAsync();
            }else if (pacienteViewModel.Nombres != null && pacienteViewModel.Apellidos != null)
            {
                paciente = await _context.Patients.Where(p =>
                EF.Functions.Like(p.Nombres, "%" + pacienteViewModel.Nombres + "%") && 
                EF.Functions.Like(p.Apellidos, "%" + pacienteViewModel.Apellidos + "%")).ToListAsync();
            }
            else if (pacienteViewModel.Nombres != null )
            {
                paciente = await _context.Patients.Where(p =>
                EF.Functions.Like(p.Nombres, "%" + pacienteViewModel.Nombres + "%"))
                    .ToListAsync();
            }
            else if (pacienteViewModel.Apellidos != null)
            {
                paciente = await _context.Patients.Where(p =>
                EF.Functions.Like(p.Nombres, "%" + pacienteViewModel.Apellidos + "%"))
                    .ToListAsync();
            }
            else
            {
                paciente = null!;
            }

            if (paciente == null || paciente.Count == 0)
            {
                response.Message = "No se ha encontrado ningún paciente con esta información.";
            }else
            {
                response.Succeed = 1;
                response.Data = paciente;
            }            

            return response;
        }

    }
}
