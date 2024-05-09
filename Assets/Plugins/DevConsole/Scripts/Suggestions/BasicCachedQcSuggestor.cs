using System.Collections.Generic;
using System.Linq;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// An IDevConsoleSuggestor that caches the created IDevConsoleSuggestion objects
    /// </summary>
    /// <typeparam name="TItem">The item type that suggestions are produced from.</typeparam>
    public abstract class BasicCachedDevConsoleSuggestor<TItem> : IDevConsoleSuggestor
    {
        private readonly Dictionary<TItem, IDevConsoleSuggestion> _suggestionCache = new Dictionary<TItem, IDevConsoleSuggestion>();

        /// <summary>
        /// If suggestions can be produced for the provided context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options">Options used by the suggestor.</param>
        /// <returns>If suggestions can be produced.</returns>
        protected abstract bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options);

        /// <summary>
        /// Converts an item to a suggestion.
        /// </summary>
        /// <param name="item">The item to convert to a suggestion.</param>
        /// <returns>The converted suggestion.</returns>
        protected abstract IDevConsoleSuggestion ItemToSuggestion(TItem item);

        /// <summary>
        /// Gets the items for a given context.
        /// </summary>
        /// <param name="context">The context to produce items for.</param>
        /// <param name="options">Options used by the suggestor.</param>
        /// <returns>The items produced.</returns>
        protected abstract IEnumerable<TItem> GetItems(SuggestionContext context, SuggestorOptions options);

        /// <summary>
        /// Determines if the provided suggestion matches the provided context.
        /// Override to remove the filtering or add custom filtering.
        /// </summary>
        /// <param name="context">The context to test the suggestion against.</param>
        /// <param name="suggestion">The suggestion to test.</param>
        /// <param name="options">Options used to test the suggestion.</param>
        /// <returns>If the suggestion matches the context.</returns>
        protected virtual bool IsMatch(SuggestionContext context, IDevConsoleSuggestion suggestion, SuggestorOptions options)
        {
            return SuggestorUtilities.IsCompatible(context.Prompt, suggestion.PrimarySignature, options);
        }

        public IEnumerable<IDevConsoleSuggestion> GetSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            if (!CanProvideSuggestions(context, options))
            {
                return Enumerable.Empty<IDevConsoleSuggestion>();
            }

            return GetItems(context, options)
                .Select(ItemToSuggestionCached)
                .Where(suggestion => IsMatch(context, suggestion, options));
        }

        private IDevConsoleSuggestion ItemToSuggestionCached(TItem item)
        {
            if (_suggestionCache.TryGetValue(item, out IDevConsoleSuggestion suggestion))
            {
                return suggestion;
            }

            return _suggestionCache[item] = ItemToSuggestion(item);
        }
    }
}