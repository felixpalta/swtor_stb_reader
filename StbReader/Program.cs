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
                Console.WriteLine("Please enter path to input file and (optionally) path to output txt file as arguments");
                Console.WriteLine("Usage: StbReader <input-stb-file> [<output-txt-file>]");
                return 1;
            }
            string inFilePath = "";
            string outFilePath = "";
            try
            {
                inFilePath = args[0];
                Console.WriteLine("InputFile: {0}", inFilePath);
                if (args.Length == 2)
                {
                    outFilePath = args[1];
                    Console.WriteLine("OutputFile: {0}", outFilePath);
                }
            } catch (System.IndexOutOfRangeException)
            {
                Console.Error.WriteLine("Invalid number of arguments");
            }

            List<CharacterManagerTests.Entry> entries = new List<CharacterManagerTests.Entry>();
            try
            {
                using (FileStream fs = File.OpenRead(inFilePath))
                {
                    entries = CharacterManagerTests.StbReader.Read(fs);

                }
                Console.WriteLine();
                if (outFilePath.Length != 0)
                {
                    FileStream filestream = new FileStream(outFilePath, FileMode.Create);
                    var streamwriter = new StreamWriter(filestream);
                    streamwriter.AutoFlush = true;
                    Console.SetOut(streamwriter);
                }
                foreach (CharacterManagerTests.Entry e in entries)
                {
                    Console.WriteLine("ID:{0}\t{1}", e.ID, e.Value);
                    Console.WriteLine();
                }
                return 0;
            } catch (System.IO.FileNotFoundException e)
            {
                Console.Error.WriteLine("Error: {0}", e.Message);
            }
            return 1;
        }
    }
}
