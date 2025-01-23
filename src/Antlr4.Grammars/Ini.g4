grammar Ini;

file    : line ( nl+ line )+ EOF ;

line    : ws key ws ASSIGN ws value ws #KeyValuePair
        | ws  #EmpyLine
        ;

key     : KEY;
value   : VALUE;

nl      : CRFL;
ws      : ( SPACE | TAB )*;

TEXT        : ([A-Za-z0-9] | '/' | '\\' | '*' | '.' | ',' | '@' | '-' | '_')+;
ASSIGN      : COLON | EQUAL;

WS          : ( SPACE | TAB )+;
CRLF        : '\r'? '\n';

fragment COLON      : ':';
fragment EQUAL      : '=';
fragment SPACE      : ' ';
fragment TAB        : '\t';
fragment HASH       : '#';
fragment SCOLON     : ';';
