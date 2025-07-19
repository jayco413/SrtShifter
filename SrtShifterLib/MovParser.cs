using System;
using System.IO;

namespace SrtShifter
{
    /// <summary>
    /// Utility methods for extracting information from MOV files.
    /// </summary>
    public static class MovParser
    {
        /// <summary>
        /// Retrieves the duration of a MOV file from disk.
        /// </summary>
        /// <param name="path">The path to the MOV file.</param>
        /// <returns>The extracted duration.</returns>
        public static TimeSpan GetDuration(string path)
        {
            using var fs = File.OpenRead(path);
            return GetDuration(fs);
        }

        /// <summary>
        /// Retrieves the duration of a MOV file from a stream.
        /// </summary>
        /// <param name="stream">The MOV file stream.</param>
        /// <returns>The extracted duration.</returns>
        public static TimeSpan GetDuration(Stream stream)
        {
            using var br = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            if (TryFindMvhd(br, stream.Length, out var duration))
            {
                return duration;
            }
            throw new InvalidDataException("mvhd box not found");
        }

        /// <summary>
        /// Searches the MOV container for the mvhd box containing duration information.
        /// </summary>
        /// <param name="br">The binary reader positioned at the start of the stream.</param>
        /// <param name="end">The position to stop searching.</param>
        /// <param name="duration">When this method returns, contains the parsed duration if found.</param>
        /// <returns><c>true</c> if the mvhd box was found; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Reads a 32-bit unsigned integer in big-endian order.
        /// </summary>
        /// <param name="br">The binary reader.</param>
        /// <returns>The 32-bit unsigned integer.</returns>
        private static uint ReadUInt32BE(BinaryReader br)
        {
            var bytes = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer in big-endian order.
        /// </summary>
        /// <param name="br">The binary reader.</param>
        /// <returns>The 64-bit unsigned integer.</returns>
        private static ulong ReadUInt64BE(BinaryReader br)
        {
            var bytes = br.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}
