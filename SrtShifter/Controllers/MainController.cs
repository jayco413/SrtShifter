using System.Text.Json;
using System.Globalization;
using SrtShifter.Models;
using SrtShifter.Views;

namespace SrtShifter.Controllers
{
    /// <summary>
    /// Coordinates the interaction between the user interface and the business logic.
    /// </summary>
    public class MainController
    {
        private readonly IMainView _view;
        private readonly string _settingsPath = Path.Combine(Application.StartupPath, "settings.json");

        /// <summary>
        /// Initializes a new instance of the <see cref="MainController"/> class.
        /// </summary>
        /// <param name="view">The view used to interact with the user.</param>
        public MainController(IMainView view)
        {
            _view = view;
            LoadSettings();
        }

        /// <summary>
        /// Opens a dialog for the user to select a video file.
        /// </summary>
        public void SelectVideoFile()
        {
            using OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "MOV files (*.mov)|*.mov|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _view.VideoFilePath = dlg.FileName;
                _view.AppendLog($"Selected video file: {dlg.FileName}");
                SaveSettings();
            }
        }

        /// <summary>
        /// Opens a dialog for the user to select an SRT subtitle file.
        /// </summary>
        public void SelectSrtFile()
        {
            using OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SRT files (*.srt)|*.srt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _view.SrtFilePath = dlg.FileName;
                _view.AppendLog($"Selected SRT file: {dlg.FileName}");
                SaveSettings();
            }
        }

        /// <summary>
        /// Processes the selected files and creates a shifted subtitle file.
        /// </summary>
        public void ProcessFiles()
        {
            try
            {
                _view.AppendLog("Processing started...");

                var videoPath = _view.VideoFilePath;
                var srtPath = _view.SrtFilePath;

                if (!File.Exists(videoPath))
                {
                    _view.AppendLog("Video file not found.");
                    return;
                }
                if (!File.Exists(srtPath))
                {
                    _view.AppendLog("SRT file not found.");
                    return;
                }

                var duration = MovParser.GetDuration(videoPath);
                _view.AppendLog($"Video duration: {duration}");

                var dir = Path.GetDirectoryName(srtPath) ?? string.Empty;
                var newPath = Path.Combine(dir, Path.GetFileNameWithoutExtension(srtPath) + "_shifted.srt");

                var srt = SrtFile.Load(srtPath);
                srt.Shift(duration);
                srt.Save(newPath);

                _view.AppendLog($"New SRT created: {newPath}");
                _view.AppendLog("Processing finished.");
            }
            catch (Exception ex)
            {
                _view.AppendLog($"Error: {ex}");
            }
        }


        /// <summary>
        /// Loads persisted application settings.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    if (settings != null)
                    {
                        _view.VideoFilePath = settings.VideoFilePath ?? string.Empty;
                        _view.SrtFilePath = settings.SrtFilePath ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                _view.AppendLog($"Error loading settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Persists the current application settings to disk.
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                var settings = new AppSettings
                {
                    VideoFilePath = _view.VideoFilePath,
                    SrtFilePath = _view.SrtFilePath
                };
                var json = JsonSerializer.Serialize(settings);
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                _view.AppendLog($"Error saving settings: {ex.Message}");
            }
        }
    }
}
