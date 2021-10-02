using System;
using System.IO;
using static CPQ.CPLParser;

namespace CPQ
{
    internal partial class CPLVisitor : CPLBaseVisitor<object>
    {
        private string type = null;
        string arg = null;
        int whileContextCounter = 0;
        int switchContextCounter = 0;

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

            whileContextCounter++;

            int jumpLineNo = GetNextLineNo();

            VisitBoolexpr(context.boolexpr());

            var arg = expressions.Pop().Key;

            // Add JMPZ command
            AddCodeLine(QuadTokens.JMPZ + " " + arg);

            // Save pointer where to add line no
            int jmpzCmdIdx = GetCurrentLineIndex();

            // Translate while body
            VisitStmt(context.stmt());

            // Add JUMP command
            AddCodeLine(QuadTokens.JUMP + " " + jumpLineNo);

            // Update JMPZ command with line no
            quadCode[jmpzCmdIdx] = quadCode[jmpzCmdIdx].Insert(QuadTokens.JMPZ.Length, " " + GetNextLineNo());

            HandleWhileBREAK();

            whileContextCounter--;

            return null;
        }

        public override object VisitSwitch_stmt(Switch_stmtContext context)
        {
            // For full explanation of expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.gi979o709n06
            switchContextCounter++;

            // Translate expression
            VisitExpression(context.expression());

            var element = expressions.Pop();
            var type = element.Value;
            if(type.GetType() == typeof(FloatType))
            {
                semanticErrorHandler.EmitSemanticError(context.Start.Line, "Expression in Switch statement cannot be of 'float' type");
                return null;
            }

            arg = element.Key;

            // Translate caselist
            VisitCaselist(context.caselist());

            // Translate DEFAULT stmtlist
            VisitStmtlist(context.stmtlist());

            HandleSwitchBREAK();

            switchContextCounter--;

            return null;
        }

        public override object VisitCaselist(CaselistContext context)
        {
            if (context != null)
            {
                VisitCaselist(context.caselist());

                if (context.NUM() != null)
                {
                    // Add IEQL command
                    var num = context.NUM().GetText();

                    var tmpVar = GetTmpVar();

                    AddCodeLine(intType.EmitEQL(tmpVar, arg, num));

                    // Add JMPZ command
                    AddCodeLine(QuadTokens.JMPZ + " " + tmpVar);

                    // Save pointer where to add line no
                    int jmpzCmdIdx = GetCurrentLineIndex();

                    // Translate stmtlist
                    VisitStmtlist(context.stmtlist());

                    // Update JMPZ command with line no
                    quadCode[jmpzCmdIdx] = quadCode[jmpzCmdIdx].Insert(QuadTokens.JMPZ.Length, " " + GetNextLineNo());
                }
            }

            return null;
        }

        public override object VisitBreak_stmt(Break_stmtContext context)
        {
            // For full explanation of expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.me0l0oh2n0he

            // Add JUMP command
            AddCodeLine(QuadTokens.JUMP);

            bool isValidContext = IsValidContext(context, out bool isWhileContext);

            if(!isValidContext)
            {
                semanticErrorHandler.EmitSemanticError(context.Start.Line, "Break statement can appear only in Switch\\While statement");
                return null;
            }

            if(isWhileContext)
            {
                // Save pointer where to add line no in while stack
                breakWhileIndexes.Push(new Tuple<int, int>(
                    whileContextCounter,
                    GetCurrentLineIndex()));
            }
            else
            {
                // Save pointer where to add line no in switch stack
                breakSwitchIndexes.Push(new Tuple<int, int>(
                    switchContextCounter,
                    GetCurrentLineIndex()));
            }

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
