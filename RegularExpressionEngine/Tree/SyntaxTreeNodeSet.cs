using System.Collections.Generic;
using System.Linq;

namespace Compiler.RegularExpressionEngine
{
    public class SyntaxTreeNodeSet : List<SyntaxTreeNode>
    {
        public SyntaxTreeNodeSet(IEnumerable<SyntaxTreeNode> collection) : base(collection)
        {
        }
        public SyntaxTreeNodeSet(List<SyntaxTreeNode> collection) : base(collection)
        {
        }

        public SyntaxTreeNodeSet()
        {
        }

        public override string ToString()
        {
            return $"{string.Join(",", this.Select(x => x.Position).ToList())}";
        }

        public void AddIfNotExists(SyntaxTreeNode to)
        {
            if (!Contains(to))
            {
                Add(to);
            }
        }

        public bool IsEqualTo(List<SyntaxTreeNode> other)
        {
            return string.Join(",", this.OrderBy(x => x.Id).Select(x => x.Id).ToList()) ==
                string.Join(",", other.OrderBy(x => x.Id).Select(x => x.Id).ToList());
        }
    }
}
