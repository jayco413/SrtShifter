using System.Globalization;
using System.Text.RegularExpressions;

namespace SrtShifter.Models
{
    /// <summary>
    /// Represents a single subtitle entry in an SRT file.
    /// </summary>
    public class SrtEntry
    {
        /// <summary>Gets or sets the sequential index of the entry.</summary>
        public int Index { get; set; }

        /// <summary>Gets or sets the time when the subtitle starts.</summary>
        public TimeSpan Start { get; set; }

        /// <summary>Gets or sets the time when the subtitle ends.</summary>
        public TimeSpan End { get; set; }

        /// <summary>Gets the subtitle text lines.</summary>
        public List<string> Lines { get; } = new();
    }

    /// <summary>
    /// Provides functionality to load, manipulate and save SRT files.
    /// </summary>
    public class SrtFile
    {
        private static readonly Regex _timeRegex = new(@"(?<start>\d{2}:\d{2}:\d{2},\d{3}) --> (?<end>\d{2}:\d{2}:\d{2},\d{3})", RegexOptions.Compiled);

        /// <summary>Gets the collection of subtitle entries.</summary>
        public List<SrtEntry> Entries { get; } = new();

        /// <summary>
        /// Loads an SRT file from the specified path.
        /// </summary>
        /// <param name="path">The path to the SRT file.</param>
        /// <returns>A populated <see cref="SrtFile"/> instance.</returns>
        public static SrtFile Load(string path)
        {
            using var reader = new StreamReader(path);
            var file = new SrtFile();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (!int.TryParse(line.Trim(), out var index))
                {
                    throw new InvalidDataException($"Invalid index line: '{line}'.");
                }

                var timeLine = reader.ReadLine() ?? throw new InvalidDataException("Unexpected end of file.");
                var match = _timeRegex.Match(timeLine);
                if (!match.Success)
                {
                    throw new InvalidDataException($"Invalid time line: '{timeLine}'.");
                }

                var entry = new SrtEntry
                {
                    Index = index,
                    Start = TimeSpan.ParseExact(match.Groups["start"].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture),
                    End = TimeSpan.ParseExact(match.Groups["end"].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture)
                };

                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    entry.Lines.Add(line);
                }

                file.Entries.Add(entry);
            }

            return file;
        }

        /// <summary>
        /// Saves the SRT file to the specified path.
        /// </summary>
        /// <param name="path">The destination path.</param>
        public void Save(string path)
        {
            using var writer = new StreamWriter(path, false);
            foreach (var entry in Entries)
            {
                writer.WriteLine(entry.Index);
                writer.WriteLine($"{entry.Start:hh\\:mm\\:ss\\,fff} --> {entry.End:hh\\:mm\\:ss\\,fff}");
                foreach (var line in entry.Lines)
                {
                    writer.WriteLine(line);
                }
                writer.WriteLine();
            }
        }

        /// <summary>
        /// Applies a time offset to all subtitle entries.
        /// </summary>
        /// <param name="offset">The amount of time to add to each entry.</param>
        public void Shift(TimeSpan offset)
        {
            foreach (var entry in Entries)
            {
                entry.Start += offset;
                entry.End += offset;
            }
        }
    }
}
