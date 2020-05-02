using System;
using System.Collections.Generic;

namespace Compiler.Common.Instructions
{
    public abstract class Instruction
    {
        public int Id { get; set; }
        public static int MaxId = 0;
        public Address Address1 { get; set; }
        public Address Address2 { get; set; }
        public Address Address3 { get; set; }
        public List<LiveAnalysisEntry> LiveVariableEntries { get; internal set; }

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

        public List<Address> GetAddresses()
        {
            List<Address> addresses = new List<Address>();
            if (Address1 != null)
            {
                addresses.Add(Address1);
            }
            if (Address2 != null)
            {
                addresses.Add(Address2);
            }
            if (Address3 != null)
            {
                addresses.Add(Address3);
            }
            return addresses;
        }

        public abstract string GenerateCodeString();

        public override string ToString()
        {
            return GenerateCodeString();
        }
    }
}
