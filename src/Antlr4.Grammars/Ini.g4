grammar Ini;

// Rules
file    : line ( NL+ line )+ EOF ;

line    : WS? key WS? ASSIGN WS? value WS? COMMENT?     #KeyValuePair
        | WS? COMMENT                                   #LineComment
        | WS                                            #EmpyLine
        ;

key     : KEY;
value   : VALUE;


// Lexer
TEXT        : ([A-Za-z0-9] | '/' | '\\' | '*' | '.' | ',' | '@' | '-' | '_')+;
ASSIGN      : '=' | ':';

// Trivia
COMMENT : ( '#' | ';' ) ~[\r\n]*;
NL      : '\r'? '\n';
WS      : ( ' ' | '\t' )+;
