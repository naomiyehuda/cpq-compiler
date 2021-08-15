using System.IO;
using static CPQ.CPLParser;

namespace CPQ
{
    class AstParser
    {
        private readonly CPLParser parser;
        private readonly SyntaxErrorListener errorListener;

        public AstParser(CPLParser parser)
        {
            this.parser = parser;
            errorListener = new SyntaxErrorListener();
            this.parser.AddErrorListener(errorListener);
        }

        public ProgramContext Parse()
        {
            return parser.program();
        }

        public int NumberOfSyntaxErrors
        {
            get { return parser.NumberOfSyntaxErrors; }
        }

        public void WriteErrors(string filePath)
        {
            errorListener.WriteErrors(filePath);
        }
    }
}
