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
            if(IsValidateArguments(args))
            {
                // Compile CPL input file
                var compiler = new CPLCompiler(directory, fileName);
                compiler.Compile(File.ReadAllText(filePath));
            }
        }

        private static bool IsValidateArguments(string[] args)
        {
            try
            {
                System.Console.ForegroundColor = System.ConsoleColor.Red;

                // Get file name
                if (args != null && args.Length > 0)
                {
                    filePath = args[0];
                }
                else
                {
                    System.Console.WriteLine("Missing required argument 'filename'");
                    return false;
                }

                if (!File.Exists(filePath))
                {
                    System.Console.WriteLine("File path doesn't exist. File path is: {0}", filePath);
                    return false;
                }

                if (!IsValidExtension(filePath, out directory, out fileName))
                {
                    System.Console.WriteLine("Input file extension is incorrect. File Extension should be 'ou'");
                }
            }
            finally
            {
                System.Console.ForegroundColor = System.ConsoleColor.White;
            }

            return true;
        }

        private static bool IsValidExtension(string filePath, out string directory, out string fileName)
        {
            directory = Path.GetDirectoryName(filePath);
            fileName = Path.GetFileNameWithoutExtension(filePath);

            return Path.GetExtension(filePath) == Constants.CPL_EXTENSION;
        }
    }
}