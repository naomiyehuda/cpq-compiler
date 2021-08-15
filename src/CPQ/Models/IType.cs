namespace CPQ
{
    public interface IType
    {
        string EmitINP(string arg0);
        string EmitADD(string arg0, string arg1, string arg2);
        string EmitSUB(string arg0, string arg1, string arg2);
        string EmitMLT(string arg0, string arg1, string arg2);
        string EmitDIV(string arg0, string arg1, string arg2);
        string EmitPRT(string arg0);
        string EmitCAST(string arg0, string arg1);
        string EmitASN(string arg0, string arg1);
        string EmitEQL(string arg0, string arg1, string arg2);
        string EmitNQL(string arg0, string arg1, string arg2);
        string EmitGRT(string arg0, string arg1, string arg2);
        string EmitLSS(string arg0, string arg1, string arg2);
    }
}