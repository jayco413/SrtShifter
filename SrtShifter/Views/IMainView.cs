namespace SrtShifter.Views
{
    /// <summary>
    /// Defines the user interface for the main form.
    /// </summary>
    public interface IMainView
    {
        /// <summary>Gets or sets the selected video file path.</summary>
        string VideoFilePath { get; set; }

        /// <summary>Gets or sets the selected subtitle file path.</summary>
        string SrtFilePath { get; set; }

        /// <summary>Appends a line of text to the log display.</summary>
        /// <param name="text">The text to append.</param>
        void AppendLog(string text);
    }
}
