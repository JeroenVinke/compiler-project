using Compiler.Parser.Common;

namespace Compiler.Parser.Instructions
{
    public class IfJumpInstruction : JumpInstruction
    {
        public string RelOp { get; set; }

        public IfJumpInstruction(Address address1, string relop, Address address2, Label label) : base(label)
        {
            Address1 = address1;
            Address2 = address2;
            RelOp = relop;
            Label = label;
        }

        public override string GenerateCodeString()
        {
            if (Label == null)
            {
                return "";
            }

            return $"if {Address1.ToString()} {RelOp} {Address2.ToString()} goto {Label.ToString()}";
        }
    }
}
