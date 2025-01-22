grammar Ini;

file    : section+ EOF;
section : ( line END+ )+;

line
	: key ASSIGN value (comment)?  #KeyValuePair
    | comment                      #LineComment
    | SPACE                        #EmptyLine
	;

comment   : COMMENT_START ~END*;
header    : LBRACK HEADER RBRACK;
key       : (~SPACE)+;
value     : (~SPACE)+;

TEXT          : ( 'a' .. 'z' | 'A' .. 'Z' | '_' | '0' .. '9' | '/' | '\\' | ':' | '*' | '.' | ',' | '@' | ' ')+;
SPACE         : ( ' ' | '\t' )+;
HEADER        : (~']')+;
ASSIGN        : '=' | ':';
COMMENT_START : '#' | ';' ;
LBRACK	      : '[';
RBRACK	      : ']';
OPEN_ACOLADE  : '{$';
CLOSE_ACOLADE : '$}';
END           : '\r\n' | '\n';
