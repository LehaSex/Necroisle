using UnityEngine;

namespace Necroisle.DevConsole.Serializers
{
    public class UnityObjectSerializer : PolymorphicDevConsoleSerializer<Object>
    {
        public override string SerializeFormatted(Object value, QuantumTheme theme)
        {
            return value.name;
        }
    }
}
