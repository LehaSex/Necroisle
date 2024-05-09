using UnityEngine;

namespace Necroisle.DevConsole.Parsers
{
    public class Vector2IntParser : BasicCachedDevConsoleParser<Vector2Int>
    {
        public override Vector2Int Parse(string value)
        {
            return (Vector2Int)ParseRecursive<Vector3Int>(value);
        }
    }
}
