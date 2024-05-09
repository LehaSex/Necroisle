using UnityEngine;

namespace Necroisle.DevConsole.Serializers
{
    public class Vector3Serializer : BasicDevConsoleSerializer<Vector3>
    {
        public override string SerializeFormatted(Vector3 value, QuantumTheme theme)
        {
            return $"({value.x}, {value.y}, {value.z})";
        }
    }
}