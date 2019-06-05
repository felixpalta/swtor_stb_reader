using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CharacterManagerTests
{
    public static class StbReader
    {
        public static List<Entry> Read(Stream stream)
        {
            byte[] buffer;
            List<Entry> entries = new List<Entry>();
            if (ValidateHeader(stream))
            {
                //Read length (4 bytes)
                buffer = stream.ReadBuffer(4);
                int size = BitConverter.ToInt32(buffer, 0);

                //Parse definitions
                
                for (int i = 0; i < size; i++)
                {
                    ParseDefinition(stream, entries);
                }

                //Read values
                foreach (Entry e in entries)
                {
                    ReadValue(stream, e);
                }
                //Entry r = entries.Where(o => o.ID == 2187718901628928).SingleOrDefault();
            }
            return entries;
        }

        private static bool ValidateHeader(Stream stream)
        {
            byte[] buffer;

            buffer = stream.ReadBuffer(3);

            return buffer.ToHexString() == "010000"; ;
        }

        private static void ParseDefinition(Stream stream, List<Entry> entries)
        {
            Entry entry;
            byte[] mainBuffer = new byte[4+4+2+4+4+4+4];
            byte[] b;
            int read = stream.Read(mainBuffer, 0, mainBuffer.Length);

            if (read > 0)
            {
                //Check middle range
                //if ((mainBuffer[8] == 0x41 || mainBuffer[8] == 0x41) && mainBuffer[9] == 0x01)
                {
                    //Middle range ok
                    entry = new Entry();

                    b = mainBuffer.SubArray(0, 7);
                    entry.ID = (ulong)BitConverter.ToInt64(b, 0);

                    b = mainBuffer.SubArray(10, 13);
                    entry.Unknown = BitConverter.ToInt32(b, 0);

                    b = mainBuffer.SubArray(14, 17);
                    entry.strLength = BitConverter.ToInt32(b, 0);

                    b = mainBuffer.SubArray(18, 21);
                    entry.strOffset = BitConverter.ToInt32(b, 0);
                    
                    if (entry.strLength != 0)
                        entries.Add(entry);
                }
            }
        }

        private static void ReadValue(Stream stream, Entry entry)
        {
            stream.Seek(entry.strOffset, SeekOrigin.Begin);
            byte[] b = stream.ReadBuffer(entry.strLength);
            entry.Value = System.Text.Encoding.UTF8.GetString(b);
        }

        #region Helpers
        public static byte[] ReadBuffer(this Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public static T[] SubArray<T>(this T[] data, int start, int end)
        {
            int length = end - start + 1;
            T[] result = new T[length];
            Array.Copy(data, start, result, 0, length);
            return result;
        } 

        private static string ToHexString(this byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "");
        }
        #endregion
    }

    public class Entry
    {
        public ulong ID { get; set; }
        public int Unknown { get; set; }
        public int strLength { get; set; }
        public int strOffset { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("Entry {0}: {1}", ID, Value);
        }
    }
}
