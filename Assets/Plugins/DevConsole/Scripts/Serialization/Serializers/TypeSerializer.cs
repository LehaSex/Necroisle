using Necroisle.DevConsole.Utilities;
using System;

namespace Necroisle.DevConsole.Serializers
{
    public class TypeSerialiazer : PolymorphicDevConsoleSerializer<Type>
    {
        public override string SerializeFormatted(Type value, QuantumTheme theme)
        {
            return value.GetDisplayName();
        }
    }
}
