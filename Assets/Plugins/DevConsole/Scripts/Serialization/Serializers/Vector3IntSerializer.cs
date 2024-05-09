using UnityEngine;

namespace Necroisle.DevConsole.Serializers
{
    public class Vector3IntSerializer : BasicDevConsoleSerializer<Vector3Int>
    {
        public override string SerializeFormatted(Vector3Int value, QuantumTheme theme)
        {
            return $"({value.x}, {value.y}, {value.z})";
        }
    }
}