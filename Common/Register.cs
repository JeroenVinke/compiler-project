namespace Compiler.Common
{
    public class Register
    {
        public string Name { get; set; }
        public Address Address { get; set; }

        public void Clear()
        {
            Address.Register = null;
            Address = null;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool InUse => Address != null;
    }
}
