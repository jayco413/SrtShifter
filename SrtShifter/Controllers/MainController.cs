using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using SrtShifter.Models;
using SrtShifter.Views;

namespace SrtShifter.Controllers
{
    public class MainController
    {
        private readonly IMainView _view;
        private readonly string _settingsPath = Path.Combine(Application.StartupPath, "settings.json");

        public MainController(IMainView view)
        {
            _view = view;
            LoadSettings();
        }

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

                ShiftSrtFile(srtPath, newPath, duration);

                _view.AppendLog($"New SRT created: {newPath}");
                _view.AppendLog("Processing finished.");
            }
            catch (Exception ex)
            {
                _view.AppendLog($"Error: {ex.Message}");
            }
        }

        private static void ShiftSrtFile(string sourcePath, string destPath, TimeSpan offset)
        {
            var regex = new Regex(@"(?<start>\d{2}:\d{2}:\d{2},\d{3}) --> (?<end>\d{2}:\d{2}:\d{2},\d{3})");
            using var reader = new StreamReader(sourcePath);
            using var writer = new StreamWriter(destPath, false);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    var start = TimeSpan.ParseExact(match.Groups["start"].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
                    var end = TimeSpan.ParseExact(match.Groups["end"].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);

                    start += offset;
                    end += offset;

                    var startTxt = start.ToString(@"hh\:mm\:ss\,fff");
                    var endTxt = end.ToString(@"hh\:mm\:ss\,fff");
                    line = $"{startTxt} --> {endTxt}";
                }
                writer.WriteLine(line);
            }
        }

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
