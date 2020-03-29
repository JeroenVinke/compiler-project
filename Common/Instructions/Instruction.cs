namespace Compiler.Parser.Common
{
    public abstract class Instruction
    {
        public int Id { get; set; }
        public static int MaxId = 0;
        public Address Address1 { get; set; }
        public Address Address2 { get; set; }
        public Address Address3 { get; set; }

        public Instruction()
        {
            Id = MaxId++;
        }

        protected Instruction(Address address1)
        {
            Address1 = address1;
        }

        protected Instruction(Address address1, Address address2)
        {
            Address1 = address1;
            Address2 = address2;
        }

        protected Instruction(Address address1, Address address2, Address address3)
        {
            Address1 = address1;
            Address2 = address2;
            Address3 = address3;
        }

        public abstract string GenerateCodeString();

        public override string ToString()
        {
            return GenerateCodeString();
        }
    }
}
