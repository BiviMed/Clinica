﻿using AutoMapper;
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
                    response.Message = "Información del paciente añadida con éxito.";
                }
                catch (Exception)
                {
                    response.Message = "Ha ocurrido un error al momento de guardar al Paciente.";
                }   
            }
            return response;
        }

        public async Task<GeneralResponse> BuscarPacientes(BuscarPacienteViewModel pacienteViewModel)
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

        public async Task<GeneralResponse> BuscarPaciente(int documentoRequest)
        {
            GeneralResponse response = new();
            try
            {
                var paciente = await _context.Patients.Where(p => p.Documento == documentoRequest).FirstOrDefaultAsync();
                if (paciente == null)
                {
                    response.Succeed = 0;
                    response.Message = "Paciente no encontrado";
                }
                else
                {
                    response.Succeed = 1;
                    response.Data = paciente;
                }                
            }
            catch (Exception)
            {
                response.Succeed = 0;
                response.Message = "Ha ocurrido un error inesperado";
            }
            return response;
        }
        

        public async Task<GeneralResponse> ActializarInformacion(Patient editarPaciente)
        {
            GeneralResponse response = new();
            try
            {
                _context.Patients.Entry(editarPaciente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Succeed = 1;
                response.Message = "Información del paciente actualizada correctamente.";
            }
            catch (Exception)
            {
                response.Message = "Se ha generado un error inesperado al momento de guardar la información";
            }

            return response;
        }


        public void MapearPacienteAEditarPaciente(Patient paciente, out EditarPacieteViewModel editarPaciente)
        {
            editarPaciente = new()
            {
                Documento = paciente.Documento,
                Nombres = paciente.Nombres,
                Apellidos = paciente.Apellidos,
                Antecedentes = paciente.Antecedentes,
                AntecedentesFarmac = paciente.AntecedentesFarmac,
                Correo = paciente.Correo,
                Direccion = paciente.Direccion,
                ExamenFisico = paciente.ExamenFisico,
                Message = null,
                NombreEps = paciente.NombreEps,
                NombreFamiliar = paciente.NombreFamiliar,
                Ocupacion = paciente.Ocupacion,
                Profesion = paciente.Profesion,
                Succeeded = null,
                Telefono = paciente.Telefono,
                TelefonoFamiliar = paciente.TelefonoFamiliar,
                TipoEps = paciente.TipoEps
            };
        }


        public void MapearEditarPacienteAPaciente(Patient paciente, EditarPacieteViewModel editarPaciente, out Patient pacienteActualizado)
        {
            paciente.Nombres = editarPaciente.Nombres;
            paciente.Apellidos = editarPaciente.Apellidos;
            paciente.Correo = editarPaciente.Correo;
            paciente.Direccion = editarPaciente.Direccion;
            paciente.Telefono = editarPaciente.Telefono;
            paciente.Profesion = editarPaciente.Profesion;
            paciente.Ocupacion = editarPaciente.Ocupacion;
            paciente.NombreFamiliar = editarPaciente.NombreFamiliar;
            paciente.TelefonoFamiliar = editarPaciente.TelefonoFamiliar;
            paciente.TipoEps = editarPaciente.TipoEps;
            paciente.NombreEps = editarPaciente.NombreEps;
            paciente.ExamenFisico = editarPaciente.ExamenFisico;
            paciente.Antecedentes = editarPaciente.Antecedentes;
            paciente.AntecedentesFarmac = editarPaciente.AntecedentesFarmac;

            pacienteActualizado = paciente;
        }


        public async Task<GeneralResponse> AgregarControl(int usuarioId, NuevoControleViewModel nuevoControl)
        {
            GeneralResponse response = new();
            response = await VerificarInfoPaciente(usuarioId, nuevoControl.PatientId, nuevoControl.Nombres, nuevoControl.Apellidos);
            if (response.Succeed == 0)
            {
                return response;
            }

            Control control = _mapper.Map<Control>(nuevoControl);
            control.Id = Guid.NewGuid();
            control.UserId = usuarioId;
            control.Fecha = DateTime.Now;

            try
            {
                await _context.Controls.AddAsync(control);
                await _context.SaveChangesAsync();
                response.Succeed = 1;
                response.Message = "Control agregado con éxito.";
            }
            catch (Exception)
            {
                response.Succeed = 0;
                response.Message = "Se ha generado un error al momento de guardar la información, por favor vuelva a intentarlo.";               
            }
            return response;
        }


        private async Task<GeneralResponse> VerificarInfoPaciente(int usuarioId, int pacienteId, string nombres, string apellidos)
        {
            GeneralResponse response = await BuscarPaciente(pacienteId);
            if (response.Succeed == 0)
            {
                response.Message = "Paciente no encontrado";
            }
            else
            {
                var paciente = (Patient)response.Data!;
                if (nombres != paciente.Nombres || apellidos != paciente.Apellidos)
                {
                    response.Succeed = 0;
                    response.Message = "Paciente no encontrado";
                }
                else
                {
                    response.Succeed = 1;
                }
            }
            return response;
        }


        public async Task<GeneralResponse> BuscarControlesById(int documento)
        {
            GeneralResponse response = await BuscarPaciente(documento);
            if (response.Succeed == 0)
            {
                return response;
            }

            try
            {
                Patient paciente = (Patient)response.Data!;
                ControlesViewModel controles = new()
                {
                    Documento = paciente.Documento,
                    Nombres = paciente.Nombres,
                    Apellidos = paciente.Apellidos
                };

                var controlesPaciente = (from control in _context.Controls
                                         where control.PatientId == paciente.Documento
                                         select control
                                       ).ToList();

                controles.Controles = controlesPaciente;
                response.Succeed = 1;
                response.Data = controles;
            }
            catch (Exception)
            {
                response.Succeed = 0;
                response.Message = "Ha sucedido un error inesperado y no se han podido cargar los controles del paciente";
                response.Data = null;
            }
            return response;
        }
    }
}
