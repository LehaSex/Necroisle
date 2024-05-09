using System;

namespace Necroisle.DevConsole.Parsers
{
    public class NullableParser : GenericDevConsoleParser
    {
        protected override Type GenericType => typeof(Nullable<>);

        public override object Parse(string value, Type type)
        {
            if (value == "null")
            {
                return null;
            }

            Type innerType = type.GetGenericArguments()[0];
            return ParseRecursive(value, innerType);
        }
    }
}