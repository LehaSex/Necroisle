using Necroisle.DevConsole.Comparators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// Provides a filtered and sorted list of suggestions for a given context using IDevConsoleSuggestors and IDevConsoleSuggestionFilter 
    /// </summary>
    public class QuantumSuggestor
    {
        private readonly IDevConsoleSuggestor[] _suggestors;
        private readonly IDevConsoleSuggestionFilter[] _suggestionFilters;
        private readonly List<IDevConsoleSuggestion> _suggestionBuffer = new List<IDevConsoleSuggestion>();

        /// <summary>
        /// Creates a Quantum Suggestor with a custom set of suggestors an suggestion filters.
        /// </summary>
        /// <param name="suggestors">The IDevConsoleSuggestors to use in this Quantum Suggestor.</param>
        /// /// <param name="suggestionFilters">The IDevConsoleSuggestionFilters to use in this Quantum Suggestor.</param>
        public QuantumSuggestor(IEnumerable<IDevConsoleSuggestor> suggestors, IEnumerable<IDevConsoleSuggestionFilter> suggestionFilters)
        {
            _suggestors = suggestors.ToArray();
            _suggestionFilters = suggestionFilters.ToArray();
        }

        /// <summary>
        /// Creates a Quantum Suggestor with the default injected suggestors and suggestion filters.
        /// </summary>
        public QuantumSuggestor() : this(
            new InjectionLoader<IDevConsoleSuggestor>().GetInjectedInstances(),
            new InjectionLoader<IDevConsoleSuggestionFilter>().GetInjectedInstances())
        {

        }

        /// <summary>
        /// Gets suggestions for a given context.
        /// </summary>
        /// <param name="context">The context to get suggestions for.</param>
        /// <param name="options">Options for the suggestor.</param>
        /// <returns>The sorted and filtered suggestions for the provided context.</returns>
        public IEnumerable<IDevConsoleSuggestion> GetSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            // Get and filter suggestions
            IEnumerable<IDevConsoleSuggestion> suggestions = 
                _suggestors
                    .SelectMany(x => x.GetSuggestions(context, options))
                    .Where(x => IsSuggestionPermitted(x, context));

            _suggestionBuffer.Clear();
            _suggestionBuffer.AddRange(suggestions);

            // Sort suggestions
            AlphanumComparator comparator = new AlphanumComparator();
            IOrderedEnumerable<IDevConsoleSuggestion> sortedSuggestions =
                _suggestionBuffer
                    .OrderBy(x => x.PrimarySignature.Length)
                    .ThenBy(x => x.PrimarySignature, comparator)
                    .ThenBy(x => x.SecondarySignature.Length)
                    .ThenBy(x => x.SecondarySignature, comparator);

            if (options.Fuzzy)
            {
                StringComparison comparisonType = options.CaseSensitive
                    ? StringComparison.CurrentCulture
                    : StringComparison.CurrentCultureIgnoreCase;

                sortedSuggestions = sortedSuggestions
                        .OrderBy(x => x.PrimarySignature.IndexOf(context.Prompt, comparisonType));
            }

            // Return suggestions to user
            return sortedSuggestions;
        }

        private bool IsSuggestionPermitted(IDevConsoleSuggestion suggestion, SuggestionContext context)
        {
            // LINQ alternative produces too much garbage
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (IDevConsoleSuggestionFilter filter in _suggestionFilters)
            {
                if (!filter.IsSuggestionPermitted(suggestion, context))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
