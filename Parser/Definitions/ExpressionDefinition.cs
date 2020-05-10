namespace Compiler.Parser
{
    public abstract class ExpressionDefinition
    {
        public abstract ExpressionSet First();
        public int SubProductionId { get; set; }
        public SubProduction SubProduction
        {
            get
            {
                return SubProduction.AllSubProductions[SubProductionId];
            }
        }
        public string Key { get; set; }

        public abstract bool IsEqualTo(ExpressionDefinition definition);
    }
}