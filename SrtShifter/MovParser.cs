using System;
using System.IO;

namespace SrtShifter
{
    internal static class MovParser
    {
        public static TimeSpan GetDuration(string path)
        {
            using var fs = File.OpenRead(path);
            return GetDuration(fs);
        }

        public static TimeSpan GetDuration(Stream stream)
        {
            using var br = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            long fileSize = stream.Length;
            while (stream.Position < fileSize)
            {
                long size = ReadUInt32BE(br);
                string type = new string(br.ReadChars(4));
                if (size == 1)
                {
                    size = (long)ReadUInt64BE(br);
                }
                long payloadStart = stream.Position;
                if (type == "mvhd")
                {
                    byte version = br.ReadByte();
                    br.ReadBytes(3); // flags
                    if (version == 0)
                    {
                        br.ReadUInt32(); // creation
                        br.ReadUInt32(); // modification
                        uint timescale = ReadUInt32BE(br);
                        uint duration = ReadUInt32BE(br);
                        return TimeSpan.FromSeconds(duration / (double)timescale);
                    }
                    else if (version == 1)
                    {
                        br.ReadUInt64();
                        br.ReadUInt64();
                        uint timescale = ReadUInt32BE(br);
                        ulong duration = ReadUInt64BE(br);
                        return TimeSpan.FromSeconds(duration / (double)timescale);
                    }
                }
                // skip to next box
                stream.Position = payloadStart + size - 8;
            }
            throw new InvalidDataException("mvhd box not found");
        }

        private static uint ReadUInt32BE(BinaryReader br)
        {
            var bytes = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private static ulong ReadUInt64BE(BinaryReader br)
        {
            var bytes = br.ReadBytes(8);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}
