using Necroisle.DevConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Necroisle.DevConsole.Suggestors
{
    public class ComponentSuggestor : BasicCachedDevConsoleSuggestor<string>
    {
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            Type targetType = context.TargetType;
            return targetType != null
                && targetType.IsDerivedTypeOf(typeof(Component))
                && !targetType.IsGenericParameter;
        }

        protected override IDevConsoleSuggestion ItemToSuggestion(string name)
        {
            return new RawSuggestion(name, true);
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            return Object.FindObjectsOfType(context.TargetType)
                .Select(cmp => (Component) cmp)
                .Select(cmp => cmp.gameObject.name);
        }
    }
}