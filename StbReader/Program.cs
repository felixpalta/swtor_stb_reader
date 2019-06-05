using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StbReader
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("Please enter path to input file as argument");
                Console.WriteLine("Usage: StbReader <input-stb-file>");
                return 1;
            }
            string inFilePath = "";
            try
            {
                Console.WriteLine("Hello there!");
                inFilePath = args[0];
                Console.WriteLine("InputFile: {0}", inFilePath);
            } catch (System.IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid number of arguments");
            }

            using (FileStream fs = File.OpenRead(inFilePath))
            {
                var entries = CharacterManagerTests.StbReader.Read(fs);
                foreach (CharacterManagerTests.Entry e in entries)
                {
                    Console.WriteLine("ID: {0}\n{1}\n", e.ID, e.Value);
                }
                //byte[] b = new byte[1024];
                //UTF8Encoding temp = new UTF8Encoding(true);
                //while (fs.Read(b, 0, b.Length) > 0)
                //{
                //    Console.WriteLine(temp.GetString(b));
                //}
            }
            return 0;
        }
    }
}
