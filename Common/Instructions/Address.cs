namespace Compiler.Parser.Common
{
    public class Address
    {
        public static int MaxId { get; set; }
        public int Id { get; set; }
        public string Prefix { get; set; }

        public Address(string prefix = "t")
        {
            Prefix = prefix;
            Id = MaxId++;
        }

        public override string ToString()
        {
            return Prefix + Id;
        }
    }
}
