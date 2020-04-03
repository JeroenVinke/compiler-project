using Compiler.Parser.Rules;
using System;
using System.Collections.Generic;

namespace Compiler.Parser
{
    public class Grammar : List<Production>
    {
        private static Grammar _instance;
        public static Grammar Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Grammar();

                    //CodeblockRule.Initialize(ref _instance);
                    //StatementsRule.Initialize(ref _instance);
                    //StatementRule.Initialize(ref _instance);
                    //DeclarationRule.Initialize(ref _instance);
                    //BooleanExpressionRule.Initialize(ref _instance);
                    //BooleanRule.Initialize(ref _instance);
                    NumericExpressionRule.Initialize(ref _instance);
                    ////FactorRule.Initialize(ref _instance);
                }

                return _instance;
            }
        }
    }
}
