using Antlr4.Runtime;
using System.IO;
using System.Text;

namespace CPQ
{
    public class SyntaxErrorListener : IAntlrErrorListener<IToken>
    {
        readonly StringBuilder errorTracing = new StringBuilder();

        // Called by Antlr in case of error
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            errorTracing.AppendFormat("line {0}:{1} {2}", line, charPositionInLine, msg);
            errorTracing.AppendLine();
        }

        public void WriteErrors(string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                streamWriter.Write(errorTracing);
                streamWriter.Flush();
            }
        }
    }
}