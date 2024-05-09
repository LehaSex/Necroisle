using System.Collections;

namespace Necroisle.DevConsole.Serializers
{
    public class DictionaryEntrySerializer : BasicDevConsoleSerializer<DictionaryEntry>
    {
        public override string SerializeFormatted(DictionaryEntry value, QuantumTheme theme)
        {
            string innerKey = SerializeRecursive(value.Key, theme);
            string innerValue = SerializeRecursive(value.Value, theme);

            return $"{innerKey}: {innerValue}";
        }
    }
}
