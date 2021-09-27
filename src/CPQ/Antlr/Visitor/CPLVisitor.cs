using System;
using System.IO;
using static CPQ.CPLParser;

namespace CPQ
{
    internal partial class CPLVisitor : CPLBaseVisitor<object>
    {
        private string type = null;
        private int breakIdx;
        private int breakLineNo;

        public CPLVisitor(string directory, string fileName)
        {
            filePath = Path.Combine(directory, fileName);
            FillInversedRelOps();
        }

        #region CPL visitor methods

        public override object VisitProgram(ProgramContext context)
        {
            // Start traversing AST tree
            base.VisitProgram(context);

            if(!semanticErrorHandler.HasSemanticErrors)
                EmitOutputCode();

            return null;
        }

        public override object VisitDeclaration(DeclarationContext context)
        {
            type = context.type().GetText();
            return base.VisitDeclaration(context);
        }

        public override object VisitIdlist(IdlistContext context)
        {
            base.VisitIdlist(context);

            AddVarToSymbolTbl(context);

            return null;
        }

        public override object VisitInput_stmt(Input_stmtContext context)
        {
            base.VisitInput_stmt(context);

            HandleINPUT(context);

            return null;
        }

        public override object VisitOutput_stmt(Output_stmtContext context)
        {
            base.VisitOutput_stmt(context);

            HandleOUTPUT();

            return null;
        }

        public override object VisitAssignment_stmt(Assignment_stmtContext context)
        {
            base.VisitAssignment_stmt(context);

            HandleAssignment(context);

            return null;
        }

        public override object VisitIf_stmt(If_stmtContext context)
        {
            // For full explanation of expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.e8021sqmo5m9

            VisitBoolexpr(context.boolexpr());

            HandleJMPZ(context);

            HandleJUMP(context);

            return null;
        }

        public override object VisitWhile_stmt(While_stmtContext context)
        {
            // For full explanation of expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.persefusho4b
            int jumpLineNo = lineNo;

            VisitBoolexpr(context.boolexpr());

            var arg = expressions.Pop().Key;

            // Add JMPZ command
            AddCodeLine(QuadTokens.JMPZ + " " + arg);

            // Save pointer where to add line no. Should also substract arg.len and 3 charachters: space, \r and \n
            int whereToAddLineNoIdx = quadCode.Length - arg.Length - 3;

            // Translate while body
            VisitStmt(context.stmt());

            // Update JMPZ command with line no
            quadCode.Insert(whereToAddLineNoIdx, " " + (lineNo + 1));

            // Add JUMP command
            AddCodeLine(QuadTokens.JUMP + " " + jumpLineNo);

            return null;
        }

        public override object VisitBreak_stmt(Break_stmtContext context)
        {
            breakIdx = quadCode.Length;
            breakLineNo = lineNo++;
            
            return base.VisitBreak_stmt(context);
        }

        public override object VisitExpression(ExpressionContext context)
        {
            // For full explanation of expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.1ta746ejsxmw

            base.VisitExpression(context);

            if (context.ADDOP() != null)
            {
                HandleADDOP(context);
            }

            return null;
        }

        public override object VisitTerm(TermContext context)
        {
            // For full explanation of expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.1ta746ejsxmw

            base.VisitTerm(context);

            if (context.MULOP() != null)
            {
                HandleMULOP(context);
            }

            return null;
        }

        public override object VisitFactor(FactorContext context)
        {
            // For full explanation of expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.1ta746ejsxmw

            base.VisitFactor(context);

            if (context.ID() != null)
            {
                HandleID(context);
            }
            else if (context.NUM() != null)
            {
                HandleNUM(context);
            }
            else if (context.CAST() != null)
            {
                HandleCAST();
            }

            return null;
        }

        public override object VisitBoolexpr(BoolexprContext context)
        {
            base.VisitBoolexpr(context);

            if (context.OR() != null)
            {
                HandleBoolterm(context.OR().GetText());
            }

            return null;
        }

        public override object VisitBoolterm(BooltermContext context)
        {
            base.VisitBoolterm(context);

            if (context.AND() != null)
            {
                HandleBoolterm(context.AND().GetText());
            }

            return null;
        }

        public override object VisitBoolfactor(BoolfactorContext context)
        {
            // For full explanation of boolian expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.geh772q1xjsl

            if (context.NOT() != null)
            {
                // Toggle notFlag
                notFlag = !notFlag;
            }

            base.VisitBoolfactor(context);

            if (context.RELOP() != null)
            {
                HandleRELOP(context.RELOP().GetText());
            }

            // notFlag will be toggled just after handling boolFactor was completed.
            // e.g.: !(a > 2 && b < 5)
            if (context.NOT() != null)
            {
                // Toggle notFlag
                notFlag = !notFlag;
            }

            return null;
        }

        #endregion
    }
}
