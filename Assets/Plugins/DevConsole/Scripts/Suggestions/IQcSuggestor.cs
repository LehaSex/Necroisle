using System.Collections.Generic;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// A suggestor that is loaded by the QuantumSuggestor to suggest IDevConsoleSuggestions
    /// </summary>
    public interface IDevConsoleSuggestor
    {
        /// <summary>
        /// Gets the suggestions for a given context.
        /// </summary>
        /// <param name="context">The context to provide suggestions for.</param>
        /// <param name="options">Options used by the suggestor.</param>
        /// <returns>The suggestions produced for the context.</returns>
        IEnumerable<IDevConsoleSuggestion> GetSuggestions(SuggestionContext context, SuggestorOptions options);
    }
}