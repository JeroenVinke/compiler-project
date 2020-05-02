using Compiler.Common;
using Compiler.Common.Instructions;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public abstract class FactorASTNode : SyntaxTreeNode
    {
        public FactorASTNode(SyntaxTreeNodeType type) : base(type)
        {
        }
    }
}
