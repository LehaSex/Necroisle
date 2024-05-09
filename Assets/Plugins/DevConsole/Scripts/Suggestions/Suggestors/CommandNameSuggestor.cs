﻿using System.Collections.Generic;
using System.Linq;

namespace Necroisle.DevConsole.Suggestors
{
    public class CommandNameSuggestor : BasicCachedDevConsoleSuggestor<string>
    {
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.HasTag<Tags.CommandNameTag>()
                && !string.IsNullOrWhiteSpace(context.Prompt);
        }

        protected override IDevConsoleSuggestion ItemToSuggestion(string commandName)
        {
            return new RawSuggestion(commandName);
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            string incompleteCommandName =
                context.Prompt
                    .SplitScopedFirst(' ')
                    .SplitFirst('<');

            return QuantumConsoleProcessor.GetUniqueCommands()
                .Select(command => command.CommandName);
        }
    }
}