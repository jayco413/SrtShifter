using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SrtShifter
{
    public partial class frmMain : Form
    {
        private readonly string _settingsPath = Path.Combine(Application.StartupPath, "settings.json");

        public frmMain()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void btnSelectVideoFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "MOV files (*.mov)|*.mov|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtVideoFilePath.Text = dlg.FileName;
                AppendLog($"Selected video file: {dlg.FileName}");
                SaveSettings();
            }
        }

        private void btnSelectSrtFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SRT files (*.srt)|*.srt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtSrtFilePath.Text = dlg.FileName;
                AppendLog($"Selected SRT file: {dlg.FileName}");
                SaveSettings();
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            ProcessFiles();
        }

        private void ProcessFiles()
        {
            try
            {
                AppendLog("Processing started...");

                var videoPath = txtVideoFilePath.Text;
                var srtPath = txtSrtFilePath.Text;

                if (!File.Exists(videoPath))
                {
                    AppendLog("Video file not found.");
                    return;
                }
                if (!File.Exists(srtPath))
                {
                    AppendLog("SRT file not found.");
                    return;
                }

                var duration = MovParser.GetDuration(videoPath);
                AppendLog($"Video duration: {duration}");

                var dir = Path.GetDirectoryName(srtPath) ?? string.Empty;
                var newPath = Path.Combine(dir, Path.GetFileNameWithoutExtension(srtPath) + "_shifted.srt");

                ShiftSrtFile(srtPath, newPath, duration);

                AppendLog($"New SRT created: {newPath}");
                AppendLog("Processing finished.");
            }
            catch (Exception ex)
            {
                AppendLog($"Error: {ex.Message}");
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
                        txtVideoFilePath.Text = settings.VideoFilePath ?? string.Empty;
                        txtSrtFilePath.Text = settings.SrtFilePath ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error loading settings: {ex.Message}");
            }
        }

        private void SaveSettings()
        {
            try
            {
                var settings = new AppSettings
                {
                    VideoFilePath = txtVideoFilePath.Text,
                    SrtFilePath = txtSrtFilePath.Text
                };
                var json = JsonSerializer.Serialize(settings);
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                AppendLog($"Error saving settings: {ex.Message}");
            }
        }

        private void AppendLog(string text)
        {
            txtLogText.AppendText(text + Environment.NewLine);
        }

        private record AppSettings
        {
            public string? VideoFilePath { get; set; }
            public string? SrtFilePath { get; set; }
        }
    }
}
