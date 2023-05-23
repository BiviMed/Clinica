using ClinicaBase.Models.Entities;

namespace ClinicaBase.Models.ViewModels
{
    public class ControlesViewModel
    {
        public int Documento { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public List<Control>? Controles { get; set; }
    }
}
