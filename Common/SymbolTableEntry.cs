using Compiler.Parser.Common;
using System;
using System.Collections.Generic;

namespace Compiler.Common
{
    public class SymbolTableEntry
    {
        public string Name { get; set; }
        public SymbolTableEntryType Type { get; set; }
        public SymbolTableEntry SpecificType { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
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

        public Label Label { get; set; }

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

        public T GetMetadata<T>(string key)
        {
            return (T)Metadata[key]; 
        }
    }
}
