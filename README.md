# Compiler

This is a hobby project

Techniques from the book Compilers: Principles, Techniques, & Tools by Alfred V. Aho et al

Currently able to generate unoptimized three address code for a very limited language

Input:

```
{
    int bar;

    bar = 0;
    
    while(bar < 10) {
        bar = bar + 1;
    }

    bar = 20;
}
```

Output:
```
t2 = 0
s0 = t2
label L2:
t3 = 10
if s0 < t3 goto label L3
t6 = 1
t7 = s0 + t6
s0 = t7
goto label L2
label L3:
t8 = 20
s0 = t8
```

