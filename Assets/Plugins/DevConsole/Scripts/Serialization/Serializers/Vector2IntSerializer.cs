using UnityEngine;

namespace Necroisle.DevConsole.Serializers
{
    public class Vector2IntSerializer : BasicDevConsoleSerializer<Vector2Int>
    {
        public override string SerializeFormatted(Vector2Int value, QuantumTheme theme)
        {
            return $"({value.x}, {value.y})";
        }
    }
}