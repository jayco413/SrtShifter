namespace SrtShifter.Models
{
    public record AppSettings
    {
        public string? VideoFilePath { get; init; }
        public string? SrtFilePath { get; init; }
    }
}
