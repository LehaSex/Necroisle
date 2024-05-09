using Necroisle.DevConsole.Utilities;
using UnityEngine;

namespace Necroisle.DevConsole.Parsers
{
    public class GameObjectParser : BasicDevConsoleParser<GameObject>
    {
        public override GameObject Parse(string value)
        {
            string name = ParseRecursive<string>(value);
            GameObject obj = GameObjectExtensions.Find(name, true);

            if (!obj)
            {
                throw new ParserInputException($"Could not find GameObject of name {value}.");
            }

            return obj;
        }
    }
}
