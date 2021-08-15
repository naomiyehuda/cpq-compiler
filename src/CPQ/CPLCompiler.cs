using Antlr4.Runtime;
using System.IO;

namespace CPQ
{
    class CPLCompiler
    {
        private string directory;
        private string fileName;

        public CPLCompiler(string directory, string fileName)
        {
            this.directory = directory;
            this.fileName = fileName;
        }

        internal void Compile(string input)
        {
            Parse(input);
        }

        private void Parse(string input)
        {
            var parser = GetParser(input);
            var parserContext = parser.Parse();

            if (parser.NumberOfSyntaxErrors == 0)
            {
                // Translate code to QUAD language
                parserContext.Accept(new CPLVisitor(directory, fileName));
            }
            else
            {
                string fullPath = Path.Combine(directory, fileName + Constants.ERROR_EXTENSION);
                parser.WriteErrors(fullPath);
            }
        }

        private AstParser GetParser(string input)
        {
            AntlrInputStream AntlrStream = new AntlrInputStream(input);
            CommonTokenStream tokenStream = new CommonTokenStream(new CPLLexer(AntlrStream));
            return new AstParser(new CPLParser(tokenStream));
        }
    }
}
