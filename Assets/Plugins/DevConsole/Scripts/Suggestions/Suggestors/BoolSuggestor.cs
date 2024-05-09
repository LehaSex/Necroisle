using System.Collections.Generic;

namespace Necroisle.DevConsole.Suggestors
{
    public class BoolSuggestor : BasicCachedDevConsoleSuggestor<string>
    {
        private readonly string[] _values =
        {
            "true",
            "false"
        };

        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.TargetType == typeof(bool);
        }

        protected override IDevConsoleSuggestion ItemToSuggestion(string value)
        {
            return new RawSuggestion(value);
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            return _values;
        }
    }
}