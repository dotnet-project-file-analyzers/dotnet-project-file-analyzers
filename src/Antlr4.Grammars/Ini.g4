grammar Ini;

file : (section)+ EOF;

section
    : ( line (END)+ )+
    ;

line
	: ws key ws ASSIGN ws value ws (comment)?  #KeyValuePair
    | ws comment                               #LineComment
    | SPACE                                    #EmptyLine
	;

comment
    : COMMENT_START ~END*
    ;

ws
    :
    | (SPACE)?
    ;

SPACE
    : ( ' ' | '\t' )+
    ;

key :
    | (~ASSIGN)+
    ;

value
    :
    |  (~COMMENT_START)+
    ;
    
ASSIGN
    : '='
    | ':'
    ;

COMMENT_START
	: '#'
    | ';'
    ;

OPEN_ACOLADE
    : '{$'
    ;

CLOSE_ACOLADE
    : '$}'
    ;

END
    : '\r'? '\n'
    ;