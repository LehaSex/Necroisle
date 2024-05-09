using System.Collections.Generic;
using System.Linq;

namespace Necroisle.DevConsole.Suggestors
{
    public class MacroSuggestor : BasicCachedDevConsoleSuggestor<string>
    {
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.Prompt.StartsWith("#");
        }

        protected override IDevConsoleSuggestion ItemToSuggestion(string macro)
        {
            return new RawSuggestion($"#{macro}");
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            return QuantumMacros.GetMacros()
                .Select(x => x.Key);
        }
    }
}