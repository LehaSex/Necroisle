using Necroisle.DevConsole.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Necroisle.DevConsole.Suggestors
{
    public class SceneNameSuggestor : BasicCachedDevConsoleSuggestor<string>
    {
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.HasTag<Tags.SceneNameTag>();
        }

        protected override IDevConsoleSuggestion ItemToSuggestion(string sceneName)
        {
            return new RawSuggestion(sceneName, true);
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            if (context.GetTag<Tags.SceneNameTag>().LoadedOnly)
            {
                return SceneUtilities.GetLoadedScenes()
                    .Select(x => x.name);
            }

            return SceneUtilities.GetAllSceneNames();
        }
    }
}