using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CPQ.CPLParser;

namespace CPQ
{
    internal partial class CPLVisitor
    {
        private readonly string filePath;
        private Dictionary<string, IType> symbols = new Dictionary<string, IType>();
        private Dictionary<string, string> inversedRelOps = new Dictionary<string, string>();
        private SemanticErrorHandler semanticErrorHandler = new SemanticErrorHandler();
        private List<string> quadCode = new List<string>();
        private IType intType = new IntType();
        private IType floatType = new FloatType();
        private Stack<KeyValuePair<string, IType>> expressions = new Stack<KeyValuePair<string, IType>>();
        private Stack<Tuple<int, int>> breakWhileIndexes = new Stack<Tuple<int, int>>();
        private Stack<Tuple<int, int>> breakSwitchIndexes = new Stack<Tuple<int, int>>();
        private bool notFlag = false;
        private int tmpVarCounter = 0;

        private bool AddVarToSymTbl(string varName, string varType)
        {
            if (symbols.ContainsKey(varName)) return false;

            IType type = GetType(varType);
            symbols.Add(varName, type);

            return true;
        }

        private bool TryGetVariable(string var, out IType type)
        {
            if (symbols.TryGetValue(var, out type))
                return true;

            return false;
        }

        private void EmitOutputCode()
        {
            AddCodeLine(QuadTokens.HALT);
            AddCodeLine("*** Naomi Yehuda ***");

            string outputFilePath = filePath + Constants.QUAD_EXTENSION;
            using var outputStream = new StreamWriter(outputFilePath);
            for (int i = 0; i < quadCode.Count; i++)
            {
                string line = quadCode[i];
                outputStream.WriteLine((i + 1) + " " + line);
            }
        }

        private void FillInversedRelOps()
        {
            inversedRelOps.Add("==", "!=");
            inversedRelOps.Add("!=", "==");
            inversedRelOps.Add("<", ">");
            inversedRelOps.Add(">", "<");
            inversedRelOps.Add(">=", "<");
            inversedRelOps.Add("<=", ">");
            inversedRelOps.Add("||", "&&");
            inversedRelOps.Add("&&", "||");
        }

        private void AddVarToSymbolTbl(IdlistContext context)
        {
            var varName = context.ID().GetText();

            var isSucceedToAdd = AddVarToSymTbl(varName, type);
            if (!isSucceedToAdd)
            {
                semanticErrorHandler.EmitSemanticError(context.Start.Line, string.Format("Variable '{0}' already declared", varName));
            }
        }

        private void HandleADDOP(ExpressionContext context)
        {
            string op = context.ADDOP().GetText();
            var right = expressions.Pop();
            var left = expressions.Pop();

            var tmpVar = GetTmpVar();
            var type = GetType(right, left);

            if (op == "+")
                AddCodeLine(type.EmitADD(tmpVar, left.Key, right.Key));
            else
                AddCodeLine(type.EmitSUB(tmpVar, left.Key, right.Key));

            expressions.Push(new KeyValuePair<string, IType>(tmpVar, type));
        }

        private void HandleINPUT(Input_stmtContext context)
        {
            var var = context.ID().GetText();
            if (!TryGetVariable(var, out IType type))
            {
                semanticErrorHandler.EmitSemanticError(context.Start.Line, string.Format("Variable '{0}' does not exist", var));
            }
            else
                AddCodeLine(type.EmitINP(var));
        }

        private void HandleOUTPUT()
        {
            var expression = expressions.Pop();
            AddCodeLine(expression.Value.EmitPRT(expression.Key));
        }

        private void HandleAssignment(Assignment_stmtContext context)
        {
            // For full explanation of assignment handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.giiki5f7q1ma

            var left = context.ID().GetText();
            if (!TryGetVariable(left, out IType leftType))
            {
                semanticErrorHandler.EmitSemanticError(context.Start.Line, string.Format("Variable '{0}' does not exist", left));
            }
            else
            {
                var right = expressions.Pop();
                if (leftType.GetType() == typeof(IntType) && right.Value.GetType() == typeof(FloatType))
                {
                    semanticErrorHandler.EmitSemanticError(context.Start.Line, "Cannot implicitly convert 'float' to 'int'");
                }
                else if (leftType.GetType() == typeof(FloatType) && right.Value.GetType() == typeof(IntType))
                {
                    // Create ITOR command
                    var tmpVar = GetTmpVar();
                    AddCodeLine(intType.EmitCAST(tmpVar, right.Key));
                    AddCodeLine(leftType.EmitASN(left, tmpVar));
                }
                else
                {
                    AddCodeLine(leftType.EmitASN(left, right.Key));
                }
            }
        }

        private void HandleNUM(FactorContext context)
        {
            var num = context.NUM().GetText();
            if (num.IndexOf(".") != -1)
            {
                expressions.Push(new KeyValuePair<string, IType>(num, floatType));
            }
            else
            {
                expressions.Push(new KeyValuePair<string, IType>(num, intType));
            }
        }

        private void HandleID(FactorContext context)
        {
            var var = context.ID().GetText();
            if (!TryGetVariable(var, out IType type))
            {
                semanticErrorHandler.EmitSemanticError(context.Start.Line, string.Format("Variable '{0}' does not exist", var));
            }
            else
            {
                expressions.Push(new KeyValuePair<string, IType>(var, type));
            }
        }

        private void HandleCAST()
        {
            var expression = expressions.Pop();
            var tmpVar = GetTmpVar();
            AddCodeLine(expression.Value.EmitCAST(tmpVar, expression.Key));

            // Add temp var with type as the expression's type
            expressions.Push(new KeyValuePair<string, IType>(tmpVar, expression.Value));
        }

        private void HandleMULOP(TermContext context)
        {
            string op = context.MULOP().GetText();
            var right = expressions.Pop();
            var left = expressions.Pop();

            var tmpVar = GetTmpVar();
            var type = GetType(right, left);

            if (op == "*")
                AddCodeLine(type.EmitMLT(tmpVar, left.Key, right.Key));
            else
                AddCodeLine(type.EmitDIV(tmpVar, left.Key, right.Key));

            expressions.Push(new KeyValuePair<string, IType>(tmpVar, type));
        }

        private void HandleRELOP(string relOp)
        {
            var right = expressions.Pop();
            var left = expressions.Pop();

            var tmpVar = GetTmpVar();
            var type = GetType(right, left);

            if (notFlag)
            {
                // Get inversed relop
                relOp = inversedRelOps[relOp];
            }

            string tmpVar2;
            switch (relOp)
            {
                case "==":
                    AddCodeLine(type.EmitEQL(tmpVar, left.Key, right.Key));
                    break;
                case "!=":
                    AddCodeLine(type.EmitNQL(tmpVar, left.Key, right.Key));
                    break;
                case ">":
                    AddCodeLine(type.EmitGRT(tmpVar, left.Key, right.Key));
                    break;
                case "<":
                    AddCodeLine(type.EmitLSS(tmpVar, left.Key, right.Key));
                    break;
                case ">=":
                    AddCodeLine(type.EmitGRT(tmpVar, left.Key, right.Key));
                    tmpVar2 = GetTmpVar();
                    AddCodeLine(type.EmitEQL(tmpVar2, left.Key, right.Key));
                    AddCodeLine(type.EmitADD(tmpVar, tmpVar2, tmpVar));
                    break;
                case "<=":
                    AddCodeLine(type.EmitLSS(tmpVar, left.Key, right.Key));
                    tmpVar2 = GetTmpVar();
                    AddCodeLine(type.EmitEQL(tmpVar2, left.Key, right.Key));
                    AddCodeLine(type.EmitADD(tmpVar, tmpVar2, tmpVar));
                    break;
            }

            expressions.Push(new KeyValuePair<string, IType>(tmpVar, type));
        }

        private void HandleBoolterm(string op)
        {
            // For full explanation of boolian expressions handling see here https://docs.google.com/document/d/1ztou5S87E3qKKMlAbFuv7m3ow-E-c4Lw7q8khP37Fxk/edit#heading=h.geh772q1xjsl

            var right = expressions.Pop();
            var left = expressions.Pop();

            var tmpVar = GetTmpVar();
            var type = GetType(right, left);

            if (notFlag)
            {
                op = inversedRelOps[op];
            }

            string tmpVar2;
            if (op == "&&")
            {
                AddCodeLine(type.EmitADD(tmpVar, left.Key, right.Key));
                tmpVar2 = GetTmpVar();
                AddCodeLine(type.EmitGRT(tmpVar2, tmpVar, "1"));
            }
            else
            {
                AddCodeLine(type.EmitADD(tmpVar, left.Key, right.Key));
                tmpVar2 = GetTmpVar();
                AddCodeLine(type.EmitGRT(tmpVar2, tmpVar, "0"));
            }

            expressions.Push(new KeyValuePair<string, IType>(tmpVar2, type));
        }

        private IType GetType(string type)
        {
            if (string.Compare(type.ToLower(), Constants.INT_TYPE) == 0)
                return intType;

            return floatType;
        }

        private string GetTmpVar()
        {
            return "t_" + tmpVarCounter++;
        }

        private IType GetType(KeyValuePair<string, IType> right, KeyValuePair<string, IType> left)
        {
            /*
             * f op i = f
             * i op f = f
             * f op f = f
             * i op i = i
             */
            return right.Value.GetType() == typeof(FloatType) ? right.Value : left.Value;
        }

        private void HandleJMPZ(If_stmtContext context)
        {
            var arg = expressions.Pop().Key;

            // Add JMPZ command
            AddCodeLine(QuadTokens.JMPZ + " " + arg);

            // Save pointer where to add line no
            int jumpCmdIdx = GetCurrentLineIndex();

            // Translating true block
            var trueBlock = context.stmt()[0];
            VisitStmt(trueBlock);

            // Update JMPZ command with line no. Adding 1 because we also take JUMP command in account
            quadCode[jumpCmdIdx] = quadCode[jumpCmdIdx].Insert(QuadTokens.JMPZ.Length, " " + (GetNextLineNo() + 1));
        }

        private void HandleJUMP(If_stmtContext context)
        {
            // Add JUMP command
            AddCodeLine(QuadTokens.JUMP);

            // Save pointer where to add line no
            int jumpCmdIdx = GetCurrentLineIndex();

            // Translating else block
            var elseBlock = context.stmt()[1];
            VisitStmt(elseBlock);

            // Update JUMP command with line no
            quadCode[jumpCmdIdx] = quadCode[jumpCmdIdx].Insert(QuadTokens.JUMP.Length, " " + GetNextLineNo());
        }

        private void HandleWhileBREAK()
        {
            while (breakWhileIndexes.Count > 0 && breakWhileIndexes.Peek().Item1 == whileContextCounter)
            {
                var index = breakWhileIndexes.Pop().Item2;

                // Update JUMP command with line no
                quadCode[index] = quadCode[index].Insert(QuadTokens.JUMP.Length, " " + GetNextLineNo());
            }
        }

        private void HandleSwitchBREAK()
        {
            while (breakSwitchIndexes.Count > 0 && breakSwitchIndexes.Peek().Item1 == switchContextCounter)
            {
                var index = breakSwitchIndexes.Pop().Item2;

                // Update JUMP command with line no
                quadCode[index] = quadCode[index].Insert(QuadTokens.JUMP.Length, " " + GetNextLineNo());
            }
        }

        private bool IsValidContext(Antlr4.Runtime.RuleContext context, out bool isWhileContext)
        {
            isWhileContext = false;

            while (context != null)
            {
                if (context.GetType() == typeof(While_stmtContext))
                {
                    isWhileContext = true;
                    return true;
                }

                if (context.GetType() == typeof(CaselistContext) || context.GetType() == typeof(Switch_stmtContext))
                {
                    return true;
                }

                context = context.Parent;
            }

            return false;
        }

        private void AddCodeLine(string code)
        {
            quadCode.Add(code);
        }

        private int GetCurrentLineIndex()
        {
            return quadCode.Count - 1;
        }

        private int GetNextLineNo()
        {
            return quadCode.Count + 1;
        }
    }
}