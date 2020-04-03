﻿namespace Compiler.Common
{
    public enum TokenType
    {
        Public,
        Integer,
        Class,
        Identifier,
        Semicolon,
        BracketOpen,
        BracketClose,
        ParenthesisOpen,
        ParenthesisClose,
        Nothing,
        EndOfFile,
        Plus,
        Minus,
        Division,
        Multiplication,
        EmptyString,
        EndMarker,
        TypeDeclaration,
        Boolean,
        Assignment,
        While,
        Or,
        And,
        RelOp,
        If,
        Else
    }
}