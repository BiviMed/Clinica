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

            var existenciaPaciente = await _context.Patients.FirstOrDefaultAsync();
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

    }
}
