using Compiler.Common;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public class ParsingTable : List<ParsingTableEntry>
    {
        public ParsingTable(IEnumerable<ParsingTableEntry> collection) : base(collection)
        {
        }

        public static ParsingTable Create(List<Production> grammar)
        {
            return new ParsingTable(CreateParsingTable(grammar));
        }

        private static List<ParsingTableEntry> CreateParsingTable(List<Production> grammar)
        {
            List<ParsingTableEntry> entries = new List<ParsingTableEntry>();

            foreach (Production production in grammar)
            {
                foreach (SubProduction subProduction in production)
                {
                    ExpressionSet set = subProduction.First(x => !(x is SemanticActionDefinition)).First();

                    foreach (TerminalExpressionDefinition terminalExpression in set)
                    {
                        AddParsingTableEntry(entries, new ParsingTableEntry
                        {
                            Dimension1 = production.Identifier,
                            Dimension2 = terminalExpression,
                            SubProduction = subProduction
                        });
                    }

                    if (set.Any(x => x.TokenType == TokenType.EmptyString))
                    {
                        ExpressionSet f = new NonTerminalExpressionDefinition { Identifier = production.Identifier }.Follow();

                        foreach (TerminalExpressionDefinition nte in f)
                        {
                            AddParsingTableEntry(entries, new ParsingTableEntry
                            {
                                Dimension1 = production.Identifier,
                                Dimension2 = nte,
                                SubProduction = subProduction
                            });
                        }

                        if (f.Any(x => x.TokenType == TokenType.EndMarker))
                        {
                            AddParsingTableEntry(entries, new ParsingTableEntry
                            {
                                Dimension1 = production.Identifier,
                                Dimension2 = f.First(x => x.TokenType == TokenType.EndMarker),
                                SubProduction = subProduction
                            });
                        }
                    }
                }
            }

            return entries;
        }

        private static void AddParsingTableEntry(List<ParsingTableEntry> entries, ParsingTableEntry parsingTableEntry)
        {
            if (!entries.Any(x => x.Dimension1 == parsingTableEntry.Dimension1 && x.Dimension2.TokenType == parsingTableEntry.Dimension2.TokenType && x.SubProduction == parsingTableEntry.SubProduction))
            {
                entries.Add(parsingTableEntry);
            }
        }
    }
}
