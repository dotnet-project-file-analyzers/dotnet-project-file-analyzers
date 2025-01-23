grammar Ini;

file    : line (CRLF+ line)+ EOF;
line	: WS? key WS? ASSIGN WS? value WS? #KeyValuePair
		;

key     : KEY;
value   : VALUE;


TEXT       : ([A-Za-z0-9] | '/' | '\\' | '*' | '.' | ',' | '@' | '-' | '_')+;
ASSIGN      : COLON | EQUAL;

WS			 : ( SPACE | TAB )+					;//-> channel(WHITESPACE);
CRLF		 : '\r'? '\n'						;//-> channel(WHITESPACE);

fragment COLON		: ':';
fragment EQUAL		: '=';
fragment SPACE		: ' ';
fragment TAB		: '\t';

// section : TEXT;

// ws      : WHITE_SPACE?;
// 
// 
// 
//		| 'not implemented'							#LineComment
//		| 'some space'				                #EmptyLine
//		| 'some header'							    #SectionHeader
// KEY           : ~(SPACE | RESERVED)+;
// TEXT          : ( [A-Za-z0-9] | '/' | '\\' | '*' | '.' | ',' | '@' )+;
// ASSIGN        : COLON | EQUAL;
// LBRACK	      : '[';
// RBRACK	      : ']';
// OPEN_ACOLADE  : '{$';
// CLOSE_ACOLADE : '$}';
// 
// header        : 'HEADER' ~(LBRACK | RBRACK)+;
// 
// // Trivia
// 
// WHITE_SPACE   : SPACE+							-> channel(HIDDEN);
// CRLF		  : '\r'? '\n'						-> channel(HIDDEN);
// 
// //COMMENT       : ('#' | ';')	(~('\r'? '\n'))*	-> channel(HIDDEN);
// 
// fragment RESERVED	: COLON | EQUAL | HASH | SCOLON;
// fragment COLON		: ':';
// fragment EQUAL		: '=';
// fragment HASH		: '#';
// fragment SCOLON		: ';';
// fragment SPACE      : ( ' ' | '\t' );
