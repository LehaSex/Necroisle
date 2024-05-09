using System;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// Serializer for a single type.
    /// </summary>
    /// <typeparam name="T">The type to serialize.</typeparam>
    public abstract class BasicDevConsoleSerializer<T> : IDevConsoleSerializer
    {
        private Func<object, QuantumTheme, string> _recursiveSerializer;

        public virtual int Priority => 0;

        public bool CanSerialize(Type type)
        {
            return type == typeof(T);
        }

        string IDevConsoleSerializer.SerializeFormatted(object value, QuantumTheme theme, Func<object, QuantumTheme, string> recursiveSerializer)
        {
            _recursiveSerializer = recursiveSerializer;
            return SerializeFormatted((T)value, theme);
        }

        protected string SerializeRecursive(object value, QuantumTheme theme)
        {
            return _recursiveSerializer(value, theme);
        }

        public abstract string SerializeFormatted(T value, QuantumTheme theme);
    }
}
