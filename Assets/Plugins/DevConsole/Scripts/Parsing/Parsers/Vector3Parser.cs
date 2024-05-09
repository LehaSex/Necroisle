using UnityEngine;

namespace Necroisle.DevConsole.Parsers
{
    public class Vector3Parser : BasicCachedDevConsoleParser<Vector3>
    {
        public override Vector3 Parse(string value)
        {
            return ParseRecursive<Vector4>(value);
        }
    }
}
