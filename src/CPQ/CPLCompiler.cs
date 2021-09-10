using Antlr4.Runtime;

namespace CPQ
{
    class CPLCompiler
    {
        private readonly string directory;
        private readonly string fileName;

        public CPLCompiler(string directory, string fileName)
        {
            this.directory = directory;
            this.fileName = fileName;
        }

        internal void Compile(string input)
        {
            var parser = GetParser(input);
            var parserContext = parser.Parse();

            if (parser.NumberOfSyntaxErrors == 0)
            {
                // Translate code to QUAD language
                parserContext.Accept(new CPLVisitor(directory, fileName));
            }

            System.Console.ForegroundColor = System.ConsoleColor.White;
        }

        private AstParser GetParser(string input)
        {
            AntlrInputStream AntlrStream = new AntlrInputStream(input);
            CommonTokenStream tokenStream = new CommonTokenStream(new CPLLexer(AntlrStream));
            return new AstParser(new CPLParser(tokenStream));
        }
    }
}
