using System.IO;
using System.Text;

namespace CPQ
{
    class SemanticErrorHandler
    {
        readonly StringBuilder errorTracing = new StringBuilder();

        // Called by Antlr in case of error
        public void SemanticError(int line, string msg)
        {
            errorTracing.AppendFormat("line {0}: {1}", line, msg);
            errorTracing.AppendLine();
        }

        public void WriteErrors(string filePath)
        {
            if (errorTracing.Length > 0)
                using (var streamWriter = new StreamWriter(filePath))
                {
                    streamWriter.Write(errorTracing);
                    streamWriter.Flush();
                }
        }
    }
}
