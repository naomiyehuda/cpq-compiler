namespace CPQ
{
    class FloatType : IType
    {
        public string EmitINP(string arg0)
        {
            // Emit: RINP arg0
            return QuadTokens.RINP + " " + arg0;
        }

        public string EmitADD(string arg0, string arg1, string arg2)
        {
            // Emit: RADD arg0 arg1 arg2
            return QuadTokens.RADD + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitSUB(string arg0, string arg1, string arg2)
        {
            // Emit: RSUB arg0 arg1 arg2
            return QuadTokens.RSUB + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitMLT(string arg0, string arg1, string arg2)
        {
            // Emit: RMLT arg0 arg1 arg2
            return QuadTokens.RMLT + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitDIV(string arg0, string arg1, string arg2)
        {
            // Emit: RDIV arg0 arg1 arg2
            return QuadTokens.RDIV + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitPRT(string arg0)
        {
            // Emit: RPRT arg0
            return QuadTokens.RPRT + " " + arg0;
        }

        public string EmitCAST(string arg0, string arg1)
        {
            // Emit: RTOI arg0 arg1
            return QuadTokens.RTOI + " " + arg0 + " " + arg1;
        }

        public string EmitASN(string arg0, string arg1)
        {
            // Emit: RASN arg0 arg1
            return QuadTokens.RASN + " " + arg0 + " " + arg1;
        }

        public string EmitEQL(string arg0, string arg1, string arg2)
        {
            // Emit: REQL arg0 arg1 arg2
            return QuadTokens.REQL + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitNQL(string arg0, string arg1, string arg2)
        {
            // Emit: RNQL arg0 arg1 arg2
            return QuadTokens.RNQL + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitGRT(string arg0, string arg1, string arg2)
        {
            // Emit: RGRT arg0 arg1 arg2
            return QuadTokens.RGRT + " " + arg0 + " " + arg1 + " " + arg2;
        }

        public string EmitLSS(string arg0, string arg1, string arg2)
        {
            // Emit: RLSS arg0 arg1 arg2
            return QuadTokens.RLSS + " " + arg0 + " " + arg1 + " " + arg2;
        }
    }
}
