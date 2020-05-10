using System;

namespace Compiler.Common
{
    public class Address
    {
        public static int MaxId { get; set; }
        public int Id { get; set; }
        public string Prefix { get; set; }
        public string DisplayValue { get; set; }
        public Register Register { get; set; }
        public virtual int Size { get; set; } = 4;
        public int? RelativeAddress { get; set; }
        public bool Spilled { get; set; }

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

        public virtual string ToMemoryLocation()
        {
            if (this is ConstantValue cv)
            {
                return cv.ToString();
            }

            if (Register != null)
            {
                return Register.ToString();
            }

            return $"dword ptr [rbp - {RelativeAddress}]";
        }

        public virtual void CalculateRelativeAddress(ref int relativeAddress)
        {
            if (!RelativeAddress.HasValue)
            {
                RelativeAddress = relativeAddress + Size;
                relativeAddress += Size;
            }
        }
    }
}
