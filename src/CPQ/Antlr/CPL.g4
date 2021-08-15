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

ID : IdentifierStart (IdentifierPart)?;

NUM: Digit+ ('.' Digit+)?;

DELIMITED_COMMENT:       '/*'  .*? '*/'           -> channel(HIDDEN);

WHITESPACES:   (WS | NL)+ -> channel(HIDDEN);

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

fragment IdentifierStart
        : UnicodeLetter;

fragment IdentifierPart
        : IdentifierStart
        | Digit
        ;
fragment Digit : [0-9];

fragment UnicodeLetter		// Any character in the Unicode categories "Uppercase letter (Lu)", 
	: '\u0041'..'\u005A'	// "Lowercase letter (Ll)", "Titlecase letter (Lt)",
	| '\u0061'..'\u007A'	// "Modifier letter (Lm)", "Other letter (Lo)", or "Letter number (Nl)".
	| '\u00AA'
	| '\u00B5'
	| '\u00BA'
	| '\u00C0'..'\u00D6'
	| '\u00D8'..'\u00F6'
	| '\u00F8'..'\u021F'
	| '\u0222'..'\u0233'
	| '\u0250'..'\u02AD'
	| '\u02B0'..'\u02B8'
	| '\u02BB'..'\u02C1'
	| '\u02D0'..'\u02D1'
	| '\u02E0'..'\u02E4'
	| '\u02EE'
	| '\u037A'
	| '\u0386'
	| '\u0388'..'\u038A'
	| '\u038C'
	| '\u038E'..'\u03A1'
	| '\u03A3'..'\u03CE'
	| '\u03D0'..'\u03D7'
	| '\u03DA'..'\u03F3'
	| '\u0400'..'\u0481'
	| '\u048C'..'\u04C4'
	| '\u04C7'..'\u04C8'
	| '\u04CB'..'\u04CC'
	| '\u04D0'..'\u04F5'
	| '\u04F8'..'\u04F9'
	| '\u0531'..'\u0556'
	| '\u0559'
	| '\u0561'..'\u0587'
	| '\u05D0'..'\u05EA'
	| '\u05F0'..'\u05F2'
	| '\u0621'..'\u063A'
	| '\u0640'..'\u064A'
	| '\u0671'..'\u06D3'
	| '\u06D5'
	| '\u06E5'..'\u06E6'
	| '\u06FA'..'\u06FC'
	| '\u0710'
	| '\u0712'..'\u072C'
	| '\u0780'..'\u07A5'
	| '\u0905'..'\u0939'
	| '\u093D'
	| '\u0950'
	| '\u0958'..'\u0961'
	| '\u0985'..'\u098C'
	| '\u098F'..'\u0990'
	| '\u0993'..'\u09A8'
	| '\u09AA'..'\u09B0'
	| '\u09B2'
	| '\u09B6'..'\u09B9'
	| '\u09DC'..'\u09DD'
	| '\u09DF'..'\u09E1'
	| '\u09F0'..'\u09F1'
	| '\u0A05'..'\u0A0A'
	| '\u0A0F'..'\u0A10'
	| '\u0A13'..'\u0A28'
	| '\u0A2A'..'\u0A30'
	| '\u0A32'..'\u0A33'
	| '\u0A35'..'\u0A36'
	| '\u0A38'..'\u0A39'
	| '\u0A59'..'\u0A5C'
	| '\u0A5E'
	| '\u0A72'..'\u0A74'
	| '\u0A85'..'\u0A8B'
	| '\u0A8D'
	| '\u0A8F'..'\u0A91'
	| '\u0A93'..'\u0AA8'
	| '\u0AAA'..'\u0AB0'
	| '\u0AB2'..'\u0AB3'
	| '\u0AB5'..'\u0AB9'
	| '\u0ABD'
	| '\u0AD0'
	| '\u0AE0'
	| '\u0B05'..'\u0B0C'
	| '\u0B0F'..'\u0B10'
	| '\u0B13'..'\u0B28'
	| '\u0B2A'..'\u0B30'
	| '\u0B32'..'\u0B33'
	| '\u0B36'..'\u0B39'
	| '\u0B3D'
	| '\u0B5C'..'\u0B5D'
	| '\u0B5F'..'\u0B61'
	| '\u0B85'..'\u0B8A'
	| '\u0B8E'..'\u0B90'
	| '\u0B92'..'\u0B95'
	| '\u0B99'..'\u0B9A'
	| '\u0B9C'
	| '\u0B9E'..'\u0B9F'
	| '\u0BA3'..'\u0BA4'
	| '\u0BA8'..'\u0BAA'
	| '\u0BAE'..'\u0BB5'
	| '\u0BB7'..'\u0BB9'
	| '\u0C05'..'\u0C0C'
	| '\u0C0E'..'\u0C10'
	| '\u0C12'..'\u0C28'
	| '\u0C2A'..'\u0C33'
	| '\u0C35'..'\u0C39'
	| '\u0C60'..'\u0C61'
	| '\u0C85'..'\u0C8C'
	| '\u0C8E'..'\u0C90'
	| '\u0C92'..'\u0CA8'
	| '\u0CAA'..'\u0CB3'
	| '\u0CB5'..'\u0CB9'
	| '\u0CDE'
	| '\u0CE0'..'\u0CE1'
	| '\u0D05'..'\u0D0C'
	| '\u0D0E'..'\u0D10'
	| '\u0D12'..'\u0D28'
	| '\u0D2A'..'\u0D39'
	| '\u0D60'..'\u0D61'
	| '\u0D85'..'\u0D96'
	| '\u0D9A'..'\u0DB1'
	| '\u0DB3'..'\u0DBB'
	| '\u0DBD'
	| '\u0DC0'..'\u0DC6'
	| '\u0E01'..'\u0E30'
	| '\u0E32'..'\u0E33'
	| '\u0E40'..'\u0E46'
	| '\u0E81'..'\u0E82'
	| '\u0E84'
	| '\u0E87'..'\u0E88'
	| '\u0E8A'
	| '\u0E8D'
	| '\u0E94'..'\u0E97'
	| '\u0E99'..'\u0E9F'
	| '\u0EA1'..'\u0EA3'
	| '\u0EA5'
	| '\u0EA7'
	| '\u0EAA'..'\u0EAB'
	| '\u0EAD'..'\u0EB0'
	| '\u0EB2'..'\u0EB3'
	| '\u0EBD'..'\u0EC4'
	| '\u0EC6'
	| '\u0EDC'..'\u0EDD'
	| '\u0F00'
	| '\u0F40'..'\u0F6A'
	| '\u0F88'..'\u0F8B'
	| '\u1000'..'\u1021'
	| '\u1023'..'\u1027'
	| '\u1029'..'\u102A'
	| '\u1050'..'\u1055'
	| '\u10A0'..'\u10C5'
	| '\u10D0'..'\u10F6'
	| '\u1100'..'\u1159'
	| '\u115F'..'\u11A2'
	| '\u11A8'..'\u11F9'
	| '\u1200'..'\u1206'
	| '\u1208'..'\u1246'
	| '\u1248'
	| '\u124A'..'\u124D'
	| '\u1250'..'\u1256'
	| '\u1258'
	| '\u125A'..'\u125D'
	| '\u1260'..'\u1286'
	| '\u1288'
	| '\u128A'..'\u128D'
	| '\u1290'..'\u12AE'
	| '\u12B0'
	| '\u12B2'..'\u12B5'
	| '\u12B8'..'\u12BE'
	| '\u12C0'
	| '\u12C2'..'\u12C5'
	| '\u12C8'..'\u12CE'
	| '\u12D0'..'\u12D6'
	| '\u12D8'..'\u12EE'
	| '\u12F0'..'\u130E'
	| '\u1310'
	| '\u1312'..'\u1315'
	| '\u1318'..'\u131E'
	| '\u1320'..'\u1346'
	| '\u1348'..'\u135A'
	| '\u13A0'..'\u13B0'
	| '\u13B1'..'\u13F4'
	| '\u1401'..'\u1676'
	| '\u1681'..'\u169A'
	| '\u16A0'..'\u16EA'
	| '\u1780'..'\u17B3'
	| '\u1820'..'\u1877'
	| '\u1880'..'\u18A8'
	| '\u1E00'..'\u1E9B'
	| '\u1EA0'..'\u1EE0'
	| '\u1EE1'..'\u1EF9'
	| '\u1F00'..'\u1F15'
	| '\u1F18'..'\u1F1D'
	| '\u1F20'..'\u1F39'
	| '\u1F3A'..'\u1F45'
	| '\u1F48'..'\u1F4D'
	| '\u1F50'..'\u1F57'
	| '\u1F59'
	| '\u1F5B'
	| '\u1F5D'
	| '\u1F5F'..'\u1F7D'
	| '\u1F80'..'\u1FB4'
	| '\u1FB6'..'\u1FBC'
	| '\u1FBE'
	| '\u1FC2'..'\u1FC4'
	| '\u1FC6'..'\u1FCC'
	| '\u1FD0'..'\u1FD3'
	| '\u1FD6'..'\u1FDB'
	| '\u1FE0'..'\u1FEC'
	| '\u1FF2'..'\u1FF4'
	| '\u1FF6'..'\u1FFC'
	| '\u207F'
	| '\u2102'
	| '\u2107'
	| '\u210A'..'\u2113'
	| '\u2115'
	| '\u2119'..'\u211D'
	| '\u2124'
	| '\u2126'
	| '\u2128'
	| '\u212A'..'\u212D'
	| '\u212F'..'\u2131'
	| '\u2133'..'\u2139'
	| '\u2160'..'\u2183'
	| '\u3005'..'\u3007'
	| '\u3021'..'\u3029'
	| '\u3031'..'\u3035'
	| '\u3038'..'\u303A'
	| '\u3041'..'\u3094'
	| '\u309D'..'\u309E'
	| '\u30A1'..'\u30FA'
	| '\u30FC'..'\u30FE'
	| '\u3105'..'\u312C'
	| '\u3131'..'\u318E'
	| '\u31A0'..'\u31B7'
	| '\u3400'
	| '\u4DB5'
	| '\u4E00'
	| '\u9FA5'
	| '\uA000'..'\uA48C'
	| '\uAC00'
	| '\uD7A3'
	| '\uF900'..'\uFA2D'
	| '\uFB00'..'\uFB06'
	| '\uFB13'..'\uFB17'
	| '\uFB1D'
	| '\uFB1F'..'\uFB28'
	| '\uFB2A'..'\uFB36'
	| '\uFB38'..'\uFB3C'
	| '\uFB3E'
	| '\uFB40'..'\uFB41'
	| '\uFB43'..'\uFB44'
	| '\uFB46'..'\uFBB1'
	| '\uFBD3'..'\uFD3D'
	| '\uFD50'..'\uFD8F'
	| '\uFD92'..'\uFDC7'
	| '\uFDF0'..'\uFDFB'
	| '\uFE70'..'\uFE72'
	| '\uFE74'
	| '\uFE76'..'\uFEFC'
	| '\uFF21'..'\uFF3A'
	| '\uFF41'..'\uFF5A'
	| '\uFF66'..'\uFFBE'
	| '\uFFC2'..'\uFFC7'
	| '\uFFCA'..'\uFFCF'
	| '\uFFD2'..'\uFFD7'
	| '\uFFDA'..'\uFFDC'
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