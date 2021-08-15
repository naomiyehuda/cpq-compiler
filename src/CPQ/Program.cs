using System.IO;

namespace CPQ
{
    class Program
    {
        private static string filePath = null;
        private static string directory = null;
        private static string fileName = null;

        static void Main(string[] args)
        {
            // Get file name
            if (args != null && args.Length > 0)
            {
                filePath = args[0];
            }

            if (!IsValidExtension(filePath, out directory, out fileName))
            {
                System.Console.WriteLine("Input file extension is not correct");
            }

            // Compile CPL input file
            var compiler = new CPLCompiler(directory, fileName);
            compiler.Compile(File.ReadAllText(filePath));

            System.Console.WriteLine("End of Compiler :-)");
        }

        private static bool IsValidExtension(string filePath, out string directory, out string fileName)
        {
            directory = null;
            fileName = null;

            if (!File.Exists(filePath))
            {
                System.Console.WriteLine("File path doesn't exist. File path is: {0}", filePath);
                return false;
            }

            directory = Path.GetDirectoryName(filePath);
            fileName = Path.GetFileNameWithoutExtension(filePath);

            return Path.GetExtension(filePath) == Constants.CPL_EXTENSION;
        }
    }
}