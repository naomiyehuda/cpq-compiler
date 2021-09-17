using Antlr4.Runtime;

namespace CPQ
{
    public class SyntaxErrorListener : IAntlrErrorListener<IToken>
    {
        public SyntaxErrorListener()
        {
            System.Console.ForegroundColor = System.ConsoleColor.Red;
        }

        // Called by Antlr in case of error
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            System.Console.WriteLine("line {0}:{1} {2}", line, charPositionInLine, msg);
        }
    }
}