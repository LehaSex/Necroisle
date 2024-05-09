using System;

namespace Necroisle.DevConsole.Parsers
{
    public class TypeParser : BasicCachedDevConsoleParser<Type>
    {
        public override Type Parse(string value)
        {
            return QuantumParser.ParseType(value);
        }
    }
}
