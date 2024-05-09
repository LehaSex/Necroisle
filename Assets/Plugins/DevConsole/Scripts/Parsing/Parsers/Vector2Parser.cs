using UnityEngine;

namespace Necroisle.DevConsole.Parsers
{
    public class Vector2Parser : BasicCachedDevConsoleParser<Vector2>
    {
        public override Vector2 Parse(string value)
        {
            return ParseRecursive<Vector4>(value);
        }
    }
}
