﻿sub rsp,0x48
mov rax,[0x{0:X2}]
mov rcx,[rax+0x18]
mov rdx,[rax+0x8]
mov eax, 0x{1:X2}
mov r8d,eax
call 0x{2:X2}
add rsp,0x48
ret