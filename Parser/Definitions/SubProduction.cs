using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public class SubProduction : List<ExpressionDefinition>
    {
        public static int MaxId = 0;
        public int Id { get; set; }
        public static Dictionary<int, SubProduction> AllSubProductions { get; set; } = new Dictionary<int, SubProduction>();

        public SubProduction(IEnumerable<ExpressionDefinition> collection) : base(collection)
        {
            Id = ++MaxId;
            ForEach(x => x.SubProductionId = this.Id);

            AllSubProductions.Add(Id, this);
        }

        public Production Production { get; internal set; }

        public override string ToString()
        {
            return string.Join(" ", this.Select(y => y.ToString()));
        }
    }
}
