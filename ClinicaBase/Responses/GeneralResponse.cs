namespace ClinicaBase.Responses
{
    public class GeneralResponse
    {
        public int Succeed { get; set; } = 0;

        public string? Message { get; set; } = null;

        public object? Data { get; set; } = null;
    }
}
