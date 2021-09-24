grammar CPL;

program: declarations stmt_block;

declarations: declarations declaration
| /* epsilon */;

declaration: idlist ':' type ';';

type: INT
| FLOAT;

idlist: idlist ',' ID
| ID;

stmt: assignment_stmt
| input_stmt
| output_stmt
| if_stmt
| while_stmt
| switch_stmt
| break_stmt
| stmt_block;

assignment_stmt: ID '=' expression ';';

input_stmt: INPUT '(' ID ')' ';';

output_stmt: OUTPUT '(' expression ')' ';';

if_stmt: IF '(' boolexpr ')' stmt ELSE stmt;

while_stmt: WHILE '(' boolexpr ')' stmt;

switch_stmt: SWITCH '(' expression ')' '{' caselist DEFAULT ':' stmtlist '}';

caselist: caselist CASE NUM ':' stmtlist
| /* epsilon */;

break_stmt: BREAK ';';

stmt_block: '{' stmtlist '}';

stmtlist: stmtlist stmt
| /* epsilon */;

boolexpr: boolexpr OR boolterm
| boolterm;

boolterm: boolterm AND boolfactor
| boolfactor;

boolfactor: NOT '(' boolexpr ')'
| expression RELOP expression;

expression: expression ADDOP term
| term;

term: term MULOP factor
| factor;

factor: '(' expression ')'
| CAST '(' expression ')'
| ID
| NUM;


ADDOP: '+' | '-';
AND: '&&';
BREAK: 'break';
CASE: 'case';
CAST: 'static_cast<int>' | 'static_cast<float>';
DEFAULT: 'default';
ELSE: 'else';
FLOAT: 'float';
IF: 'if';
INPUT: 'input';
INT: 'int';
OUTPUT: 'output';
MULOP: '*' | '/';
NOT: '!';
OR: '||';
RELOP: '==' | '!=' | '<' | '>' | '>=' | '<=';
SWITCH: 'switch';
WHILE: 'while';

ID: IdentifierNondigit (IdentifierNondigit | Digit )*;

NUM: Digit+ ('.' Digit+)?;

DELIMITED_COMMENT:       '/*'  .*? '*/'           -> channel(HIDDEN);

WHITESPACES:   (WS | NL)+ -> channel(HIDDEN);

fragment IdentifierNondigit
    :   Nondigit
    |   UniversalCharacterName;

fragment Nondigit
    :   [a-zA-Z_]
    ;

fragment Digit
    :   [0-9]
    ;

fragment UniversalCharacterName
    :   '\\u' HexQuad
    |   '\\U' HexQuad HexQuad
    ;

fragment HexQuad
    :   HexadecimalDigit HexadecimalDigit HexadecimalDigit HexadecimalDigit
    ;

fragment HexadecimalDigit
    :   [0-9a-fA-F];

fragment NL
	: '\r\n' | '\r' | '\n'
	| '\u0085' // <Next Line CHARACTER (U+0085)>'
	| '\u2028' //'<Line Separator CHARACTER (U+2028)>'
	| '\u2029' //'<Paragraph Separator CHARACTER (U+2029)>'
	;

fragment WS
	: UnicodeClassZS //'<Any Character With Unicode Class Zs>'
	| '\u0009' //'<Horizontal Tab Character (U+0009)>'
	| '\u000B' //'<Vertical Tab Character (U+000B)>'
	| '\u000C' //'<Form Feed Character (U+000C)>'
        ;

fragment UnicodeClassZS
	: '\u0020' // SPACE
	| '\u00A0' // NO_BREAK SPACE
	| '\u1680' // OGHAM SPACE MARK
	| '\u180E' // MONGOLIAN VOWEL SEPARATOR
	| '\u2000' // EN QUAD
	| '\u2001' // EM QUAD
	| '\u2002' // EN SPACE
	| '\u2003' // EM SPACE
	| '\u2004' // THREE_PER_EM SPACE
	| '\u2005' // FOUR_PER_EM SPACE
	| '\u2006' // SIX_PER_EM SPACE
	| '\u2008' // PUNCTUATION SPACE
	| '\u2009' // THIN SPACE
	| '\u200A' // HAIR SPACE
	| '\u202F' // NARROW NO_BREAK SPACE
	| '\u3000' // IDEOGRAPHIC SPACE
	| '\u205F' // MEDIUM MATHEMATICAL SPACE
	;


SINGLE_LINE_COMMENT
 : '/*' ~[\r\n]* -> channel(HIDDEN)
 ;

MULTILINE_COMMENT
 : '/*' .*? ( '*/' | EOF ) -> channel(HIDDEN)
 ;

SPACES
 : [ \u000B\t\r\n] -> channel(HIDDEN)
 ;
