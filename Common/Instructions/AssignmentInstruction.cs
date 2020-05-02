namespace Compiler.Common.Instructions
{
    public class AssignmentInstruction : Instruction
    {
        public AssignmentInstruction(Address target, Address value)
        {
            Address1 = value;
            Address3 = target;
        }

        public override string GenerateCodeString()
        {
            string value = "";

            if (Address1 is Address addr)
            {
                value = addr.ToString();
            }
            else if (Address1 is ConstantValue cv)
            {
                value = cv.ToString();
            }

            return $"{Address3.ToString()} = {value}";
        }
    }
}
