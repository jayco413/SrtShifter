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
            if (TryFindMvhd(br, stream.Length, out var duration))
            {
                return duration;
            }
            throw new InvalidDataException("mvhd box not found");
        }

        private static bool TryFindMvhd(BinaryReader br, long end, out TimeSpan duration)
        {
            duration = default;
            var stream = br.BaseStream;
            while (stream.Position < end)
            {
                long boxStart = stream.Position;
                long size = ReadUInt32BE(br);
                string type = new string(br.ReadChars(4));
                if (size == 1)
                {
                    size = (long)ReadUInt64BE(br);
                }
                long payloadEnd = boxStart + size;

                if (type == "mvhd")
                {
                    byte version = br.ReadByte();
                    br.ReadBytes(3); // flags
                    if (version == 0)
                    {
                        br.ReadUInt32(); // creation
                        br.ReadUInt32(); // modification
                        uint timescale = ReadUInt32BE(br);
                        uint dur = ReadUInt32BE(br);
                        duration = TimeSpan.FromSeconds(dur / (double)timescale);
                        return true;
                    }
                    else if (version == 1)
                    {
                        br.ReadUInt64();
                        br.ReadUInt64();
                        uint timescale = ReadUInt32BE(br);
                        ulong dur = ReadUInt64BE(br);
                        duration = TimeSpan.FromSeconds(dur / (double)timescale);
                        return true;
                    }
                }
                else if (type == "moov")
                {
                    if (TryFindMvhd(br, payloadEnd, out duration))
                    {
                        return true;
                    }
                }

                // skip to next box
                stream.Position = payloadEnd;
            }
            return false;
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
