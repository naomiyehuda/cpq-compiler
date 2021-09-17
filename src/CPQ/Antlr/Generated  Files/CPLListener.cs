//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\neomi.yehuda\Msc\Compilers\CPQ\src\CPQ\Antlr\CPL.g4 by ANTLR 4.6.6

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace CPQ {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="CPLParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.6")]
[System.CLSCompliant(false)]
public interface ICPLListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] CPLParser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] CPLParser.ProgramContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.declarations"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeclarations([NotNull] CPLParser.DeclarationsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.declarations"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeclarations([NotNull] CPLParser.DeclarationsContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeclaration([NotNull] CPLParser.DeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeclaration([NotNull] CPLParser.DeclarationContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] CPLParser.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] CPLParser.TypeContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.idlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdlist([NotNull] CPLParser.IdlistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.idlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdlist([NotNull] CPLParser.IdlistContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStmt([NotNull] CPLParser.StmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStmt([NotNull] CPLParser.StmtContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.assignment_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment_stmt([NotNull] CPLParser.Assignment_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.assignment_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment_stmt([NotNull] CPLParser.Assignment_stmtContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.input_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInput_stmt([NotNull] CPLParser.Input_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.input_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInput_stmt([NotNull] CPLParser.Input_stmtContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.output_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOutput_stmt([NotNull] CPLParser.Output_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.output_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOutput_stmt([NotNull] CPLParser.Output_stmtContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.if_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIf_stmt([NotNull] CPLParser.If_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.if_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIf_stmt([NotNull] CPLParser.If_stmtContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.while_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterWhile_stmt([NotNull] CPLParser.While_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.while_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitWhile_stmt([NotNull] CPLParser.While_stmtContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.switch_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSwitch_stmt([NotNull] CPLParser.Switch_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.switch_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSwitch_stmt([NotNull] CPLParser.Switch_stmtContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.caselist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCaselist([NotNull] CPLParser.CaselistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.caselist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCaselist([NotNull] CPLParser.CaselistContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.break_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBreak_stmt([NotNull] CPLParser.Break_stmtContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.break_stmt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBreak_stmt([NotNull] CPLParser.Break_stmtContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.stmt_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStmt_block([NotNull] CPLParser.Stmt_blockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.stmt_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStmt_block([NotNull] CPLParser.Stmt_blockContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.stmtlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStmtlist([NotNull] CPLParser.StmtlistContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.stmtlist"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStmtlist([NotNull] CPLParser.StmtlistContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.boolexpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolexpr([NotNull] CPLParser.BoolexprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.boolexpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolexpr([NotNull] CPLParser.BoolexprContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.boolterm"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolterm([NotNull] CPLParser.BooltermContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.boolterm"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolterm([NotNull] CPLParser.BooltermContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.boolfactor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolfactor([NotNull] CPLParser.BoolfactorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.boolfactor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolfactor([NotNull] CPLParser.BoolfactorContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] CPLParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] CPLParser.ExpressionContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerm([NotNull] CPLParser.TermContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerm([NotNull] CPLParser.TermContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="CPLParser.factor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFactor([NotNull] CPLParser.FactorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CPLParser.factor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFactor([NotNull] CPLParser.FactorContext context);
}
} // namespace CPQ
