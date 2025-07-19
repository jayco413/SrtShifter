namespace SrtShifter.Views
{
    public interface IMainView
    {
        string VideoFilePath { get; set; }
        string SrtFilePath { get; set; }
        void AppendLog(string text);
    }
}
