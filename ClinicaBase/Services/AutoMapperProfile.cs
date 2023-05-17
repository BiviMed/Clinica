using AutoMapper;
using ClinicaBase.Models.Entities;
using ClinicaBase.Models.ViewModels;

namespace ClinicaBase.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PacienteViewModel, Patient>();
        }
    }
}
