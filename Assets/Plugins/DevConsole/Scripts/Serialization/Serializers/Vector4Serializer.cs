using UnityEngine;

namespace Necroisle.DevConsole.Serializers
{
    public class Vector4Serializer : BasicDevConsoleSerializer<Vector4>
    {
        public override string SerializeFormatted(Vector4 value, QuantumTheme theme)
        {
            return $"({value.x}, {value.y}, {value.z}, {value.w})";
        }
    }
}