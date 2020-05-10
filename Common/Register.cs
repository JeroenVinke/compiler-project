namespace Compiler.Common
{
    public class Register : Address
    {
        public string Name { get; set; }
        public Address AddressInRegister { get; set; }
        public override int Size { get => 0; }

        public void Clear()
        {
            if (AddressInRegister != null)
            {
                AddressInRegister.Register = null;
            }
            AddressInRegister = null;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool InUse => AddressInRegister != null;

        public override void CalculateRelativeAddress(ref int relativeAddress)
        {
        }

        public override string ToMemoryLocation()
        {
            return Name;
        }
    }
}
