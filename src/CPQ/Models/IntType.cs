namespace CPQ
{
    class IntType : IType
    {
        public string EmitINP(string arg0)
        {
            // Emit: IINP arg0
            return QuadTokens.IINP + " " + arg0;
        }

        public string EmitADD(string arg0, string arg1, string arg2)
        {
            // Emit: IADD arg0 arg1 arg2
            return QuadTokens.IADD + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitSUB(string arg0, string arg1, string arg2)
        {
            // Emit: ISUB arg0 arg1 arg2
            return QuadTokens.ISUB + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitMLT(string arg0, string arg1, string arg2)
        {
            // Emit: IMLT arg0 arg1 arg2
            return QuadTokens.IMLT + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitDIV(string arg0, string arg1, string arg2)
        {
            // Emit: IDIV arg0 arg1 arg2
            return QuadTokens.IDIV + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitPRT(string arg0)
        {
            // Emit: IPRT arg0
            return QuadTokens.IPRT + " " + arg0;
        }

        public string EmitCAST(string arg0, string arg1)
        {
            // Emit: ITOR arg0 arg1
            return QuadTokens.ITOR + " " + arg0 + " " + arg1;
        }

        public string EmitASN(string arg0, string arg1)
        {
            // Emit: IASN arg0 arg1
            return QuadTokens.IASN + " " + arg0 + " " + arg1;
        }

        public string EmitEQL(string arg0, string arg1, string arg2)
        {
            // Emit: IEQL arg0 arg1 arg2
            return QuadTokens.IEQL + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitNQL(string arg0, string arg1, string arg2)
        {
            // Emit: INQL arg0 arg1 arg2
            return QuadTokens.INQL + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitGRT(string arg0, string arg1, string arg2)
        {
            // Emit: IGRT arg0 arg1 arg2
            return QuadTokens.IGRT + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitLSS(string arg0, string arg1, string arg2)
        {
            // Emit: ILSS arg0 arg1 arg2
            return QuadTokens.ILSS + " " + arg0 + " " + arg1 + " " + arg2;
        }
    }
}
