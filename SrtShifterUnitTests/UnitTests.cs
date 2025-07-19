using Microsoft.VisualStudio.TestTools.UnitTesting;
using SrtShifter.Models;
using System;
using System.IO;
using System.Linq;

namespace SrtShifterUnitTests
{
    [TestClass]
    public class UnitTests
    {
        private static string GetSamplePath(string fileName)
        {
            var dir = Path.GetDirectoryName(typeof(UnitTests).Assembly.Location) ?? string.Empty;
            return Path.Combine(dir, fileName);
        }

        [TestMethod]
        public void MovParser_ReturnsExpectedDuration()
        {
            var movPath = GetSamplePath("sample.mov");
            var duration = SrtShifter.MovParser.GetDuration(movPath);
            var expected = TimeSpan.FromSeconds(3.7120416);
            Assert.IsTrue(Math.Abs((duration - expected).TotalMilliseconds) <= 1, $"Duration {duration} not within tolerance");
        }

        [TestMethod]
        public void SrtFile_LoadsEntries()
        {
            var srtPath = GetSamplePath("sample.srt");
            var srt = SrtFile.Load(srtPath);
            Assert.IsTrue(srt.Entries.Count > 0, "No entries parsed");
            var first = srt.Entries.First();
            Assert.AreEqual(1, first.Index);
            Assert.AreEqual(TimeSpan.Parse("00:00:00.367"), first.Start);
            Assert.AreEqual(TimeSpan.Parse("00:00:03.534"), first.End);
            Assert.IsTrue(first.Lines.Count > 0);
        }

        [TestMethod]
        public void SrtFile_ShiftAndSave_RoundTrips()
        {
            var srtPath = GetSamplePath("sample.srt");
            var temp = Path.GetTempFileName();
            try
            {
                var srt = SrtFile.Load(srtPath);
                var offset = TimeSpan.FromSeconds(5);
                srt.Shift(offset);
                srt.Save(temp);

                var shifted = SrtFile.Load(temp);
                Assert.AreEqual(srt.Entries.First().Start, shifted.Entries.First().Start);
            }
            finally
            {
                File.Delete(temp);
            }
        }

    }
}
