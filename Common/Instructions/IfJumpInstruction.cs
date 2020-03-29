namespace Compiler.Parser.Common
{
    public class IfJumpInstruction : Instruction
    {
        public string RelOp { get; set; }
        public Label Label { get; set;  }

        public IfJumpInstruction(Address address1, string relop, Address address2, Label label) : base(address1, address2)
        {
            RelOp = relop;
            Label = label;
        }

        public override string GenerateCodeString()
        {
            return $"if {Address1.ToString()} {RelOp} {Address2.ToString()} goto {Label.ToString()}";
        }
    }
}
