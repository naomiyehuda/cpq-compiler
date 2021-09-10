namespace CPQ
{
    class SemanticErrorHandler
    {
        int numOfSemanticError = 0;

        public SemanticErrorHandler()
        {
            System.Console.ForegroundColor = System.ConsoleColor.Red;
        }

        public void EmitSemanticError(int line, string msg)
        {
            numOfSemanticError++;
            System.Console.WriteLine("line {0}: {1}", line, msg);
        }

        public bool HasSemanticErrors
        {
            get
            {
                return numOfSemanticError > 0;
            }
        }
    }
}
