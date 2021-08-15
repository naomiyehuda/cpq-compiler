﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using static CPQ.CPLParser;

namespace CPQ
{
    internal partial class CPLVisitor
    {
        private int lineNo = 1;
        private readonly string filePath;
        private Dictionary<string, IType> symbols = new Dictionary<string, IType>();
        private Dictionary<string, string> inversedRelOps = new Dictionary<string, string>();
        private SemanticErrorHandler semanticErrorHandler = new SemanticErrorHandler();
        private StringBuilder quadCode = new StringBuilder();
        private IType intType = new IntType();
        private IType floatType = new FloatType();
        private Stack<KeyValuePair<string, IType>> expressions = new Stack<KeyValuePair<string, IType>>();
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
            using (var outputStream = new StreamWriter(outputFilePath))
            {
                outputStream.Write(quadCode);
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

        private void EmitErrors()
        {
            string errorFilePath = filePath + Constants.ERROR_EXTENSION;
            semanticErrorHandler.WriteErrors(errorFilePath);
        }

        private void AddVarToSymbolTbl(IdlistContext context)
        {
            var var = context.ID().GetText();

            var isSucceedToAdd = AddVarToSymTbl(var, type);
            if (!isSucceedToAdd)
            {
                semanticErrorHandler.SemanticError(context.Start.Line, string.Format("Variable '{0}' already declared", var));
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
                semanticErrorHandler.SemanticError(context.Start.Line, string.Format("Variable '{0}' does not exist", var));
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
            var left = context.ID().GetText();
            if (!TryGetVariable(left, out IType leftType))
            {
                semanticErrorHandler.SemanticError(context.Start.Line, string.Format("Variable '{0}' does not exist", left));
            }
            else
            {
                var right = expressions.Pop();
                if (leftType.GetType() == typeof(IntType) && right.Value.GetType() == typeof(FloatType))
                {
                    semanticErrorHandler.SemanticError(context.Start.Line, "Cannot implicitly convert 'float' to 'int'");
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
                semanticErrorHandler.SemanticError(context.Start.Line, string.Format("Variable '{0}' does not exist", var));
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

            if (expression.Value.GetType() == typeof(FloatType))
            {
                expressions.Push(new KeyValuePair<string, IType>(tmpVar, intType));
            }
            else
            {
                expressions.Push(new KeyValuePair<string, IType>(tmpVar, floatType));
            }
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
            if (string.Compare(type, Constants.INT_TYPE) == 0)
                return intType;

            return floatType;
        }

        private string GetTmpVar()
        {
            return "t_" + tmpVarCounter++;
        }

        private IType GetType(KeyValuePair<string, IType> right, KeyValuePair<string, IType> left)
        {
            return right.Value.GetType() == typeof(FloatType) ? right.Value : left.Value;
        }

        private void AddCodeLine(string code)
        {
            quadCode.AppendLine(lineNo++ + " " + code);
        }
    }
}