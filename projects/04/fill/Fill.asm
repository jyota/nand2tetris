(MAIN)
(DOBLACKFILL)
@i
M=0
(BLACKLOOP)
@KBD
D=M
@DOWHITEFILL
D;JEQ
@i
D=M
@8192
D=A-D
@BLACKEND
D;JLE
@i
D=M
@SCREEN
A=A+D
M=-1
@i
M=M+1
@BLACKLOOP
0;JMP
(BLACKEND)
@MAIN
0;JMP

(DOWHITEFILL)
@i
M=0
(WHITELOOP)
@KBD
D=M
@DOBLACKFILL
D;JNE
@i
D=M
@8192
D=A-D
@WHITEEND
D;JLE
@i
D=M
@SCREEN
A=A+D
M=0
@i
M=M+1
@WHITELOOP
0;JMP
(WHITEEND)
@MAIN
0;JMP

(TRUEEND)
0;JMP