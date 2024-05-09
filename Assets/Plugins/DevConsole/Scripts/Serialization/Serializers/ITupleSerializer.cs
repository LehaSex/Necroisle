﻿#if !NET_STANDARD_2_0

using System.Runtime.CompilerServices;

namespace Necroisle.DevConsole.Serializers
{
    public class ITupleSerializer : PolymorphicDevConsoleSerializer<ITuple>
    {
        public override string SerializeFormatted(ITuple value, QuantumTheme theme)
        {
            string[] serializedItems = new string[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                serializedItems[i] = SerializeRecursive(value[i], theme);
            }

            return $"({string.Join(", ", serializedItems)})";
        }
    }
}
#endif