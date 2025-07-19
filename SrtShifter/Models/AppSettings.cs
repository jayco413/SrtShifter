namespace SrtShifter.Models
{
    /// <summary>
    /// Represents persisted application settings.
    /// </summary>
    public record AppSettings
    {
        /// <summary>Gets or initializes the last used video file path.</summary>
        public string? VideoFilePath { get; init; }

        /// <summary>Gets or initializes the last used subtitle file path.</summary>
        public string? SrtFilePath { get; init; }
    }
}
