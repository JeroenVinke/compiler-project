namespace Compiler.Parser.Common
{
    public class Address
    {
        public static int MaxId { get; set; }
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string DisplayValue { get; set; }

        public Address(string prefix = "t", string display = "")
        {
            Prefix = prefix;
            Id = MaxId++;
            DisplayValue = display;
        }

        public override string ToString()
        {
            return Prefix + (!string.IsNullOrEmpty(DisplayValue) ? DisplayValue : "" + Id);
        }
    }
}
