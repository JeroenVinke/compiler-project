namespace Compiler.Common
{
    public class ConstantValue : Address
    {
        public object Value { get; set; }

        public ConstantValue(object value)
        {
            Id = MaxId++;
            Value = value;
        }

        public override string ToString()
        {
            if (Value is int v)
            {
                return "" + v;
            }
            if (Value is bool b)
            {
                return b ? "true" : "false";
            }

            return "";
        }
    }
}
