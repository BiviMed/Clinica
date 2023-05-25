using System.ComponentModel.DataAnnotations;

namespace ClinicaBase.Models.ViewModels
{
    public class EliminarUsuarioViewModel
    {        
        public int Documento { get; set; }

        public string? Nombres { get; set; }

        public string? Apellidos { get; set; }
    }
}
