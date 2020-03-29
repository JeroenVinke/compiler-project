using Compiler.Parser.Common;
using System;

namespace Compiler.Common
{
    public class SymbolTableEntry
    {
        public string Name { get; set; }
        public SymbolTableEntryType Type { get; set; }
        public object Value { get; set; }
        public int Id { get; set; }
        public static int MaxId { get; set; } = 0;
        private Address _address;
        public Address Address
        {
            get
            {
                if (_address == null)
                {
                    _address = new Address("", Name + Id.ToString());
                }

                return _address;
            }
        }

        public SymbolTableEntry()
        {
            Id = MaxId;
            MaxId++;
        }

        public override string ToString()
        {
            return Name + " (" + GetTypeAsString() + ")";
        }

        public string GetTypeAsString()
        {
            return Enum.GetName(typeof(SymbolTableEntryType), Type);
        }
    }
}
