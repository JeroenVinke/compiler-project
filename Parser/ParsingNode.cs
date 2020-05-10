using Compiler.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public class ParsingNode
    {
        public static int MaxId = 0;

        private Expression expression;
        private ParsingNode parent;
        public BottomUpParser Parser { get; set; }
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        private int Id { get; set; }

        public ParsingNode Parent
        {
            get => parent;
            set
            {
                parent = value;
                value.Children.Insert(0, this);
            }
        }

        private SymbolTable _currentSymbolTable;
        public SymbolTable CurrentSymbolTable
        {
            get
            {
                if (_currentSymbolTable == null)
                {
                    _currentSymbolTable = FirstParentWithAttribute(ParserConstants.SymTable)?.GetAttribute<SymbolTable>(ParserConstants.SymTable);

                    if (_currentSymbolTable == null)
                    {
                        _currentSymbolTable = Parser.RootSymbolTable;
                    }
                }

                return _currentSymbolTable;
            }
        }

        private SymbolTable _childSymbolTable;
        public SymbolTable ChildSymbolTable
        {
            get
            {
                if (_childSymbolTable == null)
                {
                    SymbolTable symbolTable = FirstParentWithAttribute(ParserConstants.SymTable)?.GetAttribute<SymbolTable>(ParserConstants.SymTable);

                    if (symbolTable == null)
                    {
                        symbolTable = Parser.RootSymbolTable;
                    }

                    _childSymbolTable = symbolTable.CreateChild();
                }

                return _childSymbolTable;
            }
        }

        public Expression Expression
        {
            get => expression;
            set
            {
                expression = value;
                value.Tree = this;
            }
        }

        public List<ParsingNode> Children { get; set; } = new List<ParsingNode>();
        public SubProduction SubProduction { get; set; }

        public ParsingNode()
        {
            Id = MaxId++;
        }

        public void EvaluateAttributes()
        {
            if (Expression is TerminalExpression)
            {
                return;
            }

            int i = 0;
            foreach(ExpressionDefinition expressionDefinition in SubProduction)
            {
                if (expressionDefinition is SemanticActionDefinition sa)
                {
                    sa.Action(this);
                }
                else
                {
                    if (!(expressionDefinition is TerminalExpressionDefinition ted && ted.TokenType == TokenType.EmptyString))
                    {
                        ParsingNode child1 = Children.First(x => x.Expression.Key == expressionDefinition.Key);
                        ParsingNode child = Children[i];
                        if (child != child1)
                        {
                            ;
                        }

                        if (child.Expression is NonTerminalExpression)
                        {
                            child.EvaluateAttributes();
                        }
                    }
                    i++;
                }
            }
        }

        public ParsingNode GetTopParent()
        {
            ParsingNode tree = this;

            while (true)
            {
                if(tree.Parent != null)
                {
                    tree = tree.Parent;
                }
                else
                {
                    break;
                }
            }

            return tree;
        }

        public T GetAttributeForKey<T>(string key, string attribute)
        {
            return GetNodeForKey(key).GetAttribute<T>(attribute);
        }

        public ParsingNode GetNodeForKey(string key)
        {
            return Children.First(x => x.Expression.Key == key);
        }

        public T GetAttribute<T>(string key)
        {
            return (T)Attributes[key];
        }

        public ParsingNode FirstParentWithAttribute(string v)
        {
            ParsingNode node = this;

            while (node != null)
            {
                if (node.Attributes.ContainsKey(v))
                {
                    return node;
                }

                node = node.Parent;
            }

            return null;
        }

        public string ToDot()
        {
            string result = $"{Id}3333 [label=\"{Expression.ToString()}\"]\r\n";

            foreach (ParsingNode child in Children)
            {
                result += $"{child.Id}3333 -> {Id}3333\r\n";
                result += child.ToDot();
            }

            return result;
        }

        internal T GetAttributeForKey<T>(object identifier, string symbolTableEntry)
        {
            throw new NotImplementedException();
        }
    }
}