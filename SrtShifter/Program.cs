namespace SrtShifter
{
    /// <summary>
    /// Application entry point class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Starts the Windows Forms application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMain());
        }
    }
}