using Antlr4.Runtime.Misc;
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
            base.VisitProgram(context);
            EmitOutputCode();
            EmitErrors();

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
            VisitBoolexpr(context.boolexpr());

            // Save relevant parameters for JUMPZ command
            int currLineIdx = quadCode.Length;
            int jumpzLineNo = lineNo;
            string argument = expressions.Pop().Key;

            lineNo++;

            var trueBlock = context.stmt()[0];
            VisitStmt(trueBlock);

            // Insert JUMPZ command
            var jmpzCommand = QuadTokens.JMPZ + " " + lineNo + " " + argument + Environment.NewLine;
            var jmpzWithLineNo = jumpzLineNo + " " + jmpzCommand;
            quadCode.Insert(currLineIdx, jmpzWithLineNo);

            var elseBlock = context.stmt()[1];
            VisitStmt(elseBlock);

            return null;
        }

        public override object VisitWhile_stmt(While_stmtContext context)
        {
            int jumpLineNo = lineNo;
            VisitBoolexpr(context.boolexpr());

            // Save relevant parameters for JUMPZ and JUMP commands
            int currLineIdx = quadCode.Length;
            int jumpzLineNo = lineNo;
            string argument = expressions.Pop().Key;

            lineNo++;

            VisitStmt(context.stmt());

            // It means one of the statements is BREAK statement
            if (breakIdx > 0)
            {
                // Add JUMP command
                var endOfWhileLine = lineNo;
                var breakJump = breakLineNo + " " + QuadTokens.JUMP + " " + endOfWhileLine + Environment.NewLine;
                quadCode.Insert(breakIdx, breakJump);
            }
            else
            {
                // Add JUMP command
                var jumpCommand = lineNo++ + " " + QuadTokens.JUMP + " " + jumpLineNo;
                quadCode.AppendLine(jumpCommand);
            }

            // Insert JMPZ command
            var jmpzCommand = QuadTokens.JMPZ + " " + lineNo + " " + argument + Environment.NewLine;
            var jmpzWithLineNo = jumpzLineNo + " " + jmpzCommand;
            quadCode.Insert(currLineIdx, jmpzWithLineNo);


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
            base.VisitExpression(context);

            if (context.ADDOP() != null)
            {
                HandleADDOP(context);
            }

            return null;
        }

        public override object VisitTerm(TermContext context)
        {
            base.VisitTerm(context);

            if (context.MULOP() != null)
            {
                HandleMULOP(context);
            }

            return null;
        }

        public override object VisitFactor(FactorContext context)
        {
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
