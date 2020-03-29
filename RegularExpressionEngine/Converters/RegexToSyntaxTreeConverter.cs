using System;

namespace Compiler.RegularExpressionEngine
{
    public class RegexToSyntaxTreeConverter
    {
        private RegexToSyntaxTreeConverter()
        {
        }

        public static SyntaxTreeNode Convert(string regex)
        {
            RegexToSyntaxTreeConverter converter = new RegexToSyntaxTreeConverter();
            SyntaxTreeNode result = converter.DoConvert(regex).SetPositions();
            result.TopParent().CalculateFollowPos();
            return result;
        }

        private SyntaxTreeNode DoConvert(string regex)
        {
            SubExpression subExpression = RegexToSubExpressionConverter.Convert(regex);

            return GetNode(subExpression);
        }

        private SyntaxTreeNode GetNode(SubExpression expression)
        {
            if (expression is RepeatedExpression repeatedExpression)
            {
                if (repeatedExpression.RepetitionOperator == '*')
                {
                    SyntaxTreeNode node = GetNode(repeatedExpression.Expression);
                    SyntaxTreeNode parent = CreateNode();
                    parent.Type = SyntaxTreeNodeType.Star;
                    node.Parent = parent;
                    parent.Children.Add(node);
                    return parent;
                }
                else if (repeatedExpression.RepetitionOperator == '+')
                {
                    SyntaxTreeNode node = GetNode(repeatedExpression.Expression);
                    SyntaxTreeNode parent = CreateNode();
                    parent.Type = SyntaxTreeNodeType.Plus;
                    node.Parent = parent;
                    parent.Children.Add(node);
                    return parent;
                }
            }
            else if (expression is OrExpression orExpression)
            {
                SyntaxTreeNode left = null;
                SyntaxTreeNode right = null;
                SyntaxTreeNode lastParent = null;

                foreach (SubExpression subExpression in orExpression.SubExpressions)
                {
                    if (left == null)
                    {
                        left = GetNode(subExpression);
                        continue;
                    }
                    else if (right == null)
                    {
                        right = GetNode(subExpression);

                        SyntaxTreeNode parent = CreateNode();
                        parent.Type = SyntaxTreeNodeType.Or;
                        left.Parent = parent;
                        right.Parent = parent;
                        parent.Children.Add(left);
                        parent.Children.Add(right);
                        lastParent = parent;
                        left = parent;

                        right = null;
                    }
                }

                return lastParent;
            }
            else if (expression is AndExpression andExpression)
            {
                SyntaxTreeNode left = null;
                SyntaxTreeNode right = null;
                SyntaxTreeNode lastParent = null;

                foreach (SubExpression subExpression in andExpression.SubExpressions)
                {
                    if (left == null)
                    {
                        left = GetNode(subExpression);
                        continue;
                    }
                    else if (right == null)
                    {
                        right = GetNode(subExpression);

                        SyntaxTreeNode parent = CreateNode();
                        parent.Type = SyntaxTreeNodeType.And;
                        left.Parent = parent;
                        right.Parent = parent;
                        parent.Children.Add(left);
                        parent.Children.Add(right);
                        lastParent = parent;
                        left = parent;

                        right = null;
                    }
                }

                return lastParent;
            }
            else if (expression is SingleCharacterExpression singleCharacter)
            {
                return new SyntaxTreeNode { Character = singleCharacter.Character, Type = SyntaxTreeNodeType.Leaf };
            }

            return null;
        }

        private SyntaxTreeNode CreateNode()
        {
            return new SyntaxTreeNode();
        }
    }
}
