using UnityEngine;

namespace Necroisle.DevConsole.Serializers
{
    public class Vector2Serializer : BasicDevConsoleSerializer<Vector2>
    {
        public override string SerializeFormatted(Vector2 value, QuantumTheme theme)
        {
            return $"({value.x}, {value.y})";
        }
    }
}