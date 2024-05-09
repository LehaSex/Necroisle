using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Necroisle.DevConsole.Suggestors
{
    public class GameObjectSuggestor : BasicCachedDevConsoleSuggestor<string>
    {
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.TargetType == typeof(GameObject);
        }

        protected override IDevConsoleSuggestion ItemToSuggestion(string name)
        {
            return new RawSuggestion(name, true);
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            return Object.FindObjectsOfType<GameObject>()
                .Select(obj => obj.name);
        }
    }
}