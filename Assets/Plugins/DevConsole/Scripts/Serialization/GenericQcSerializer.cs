using Necroisle.DevConsole.Utilities;
using System;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// Serializer for all types that are generic constructions of a single type.
    /// </summary>
    public abstract class GenericDevConsoleSerializer : IDevConsoleSerializer
    {
        /// <summary>
        /// The incomplete generic type of this serializer.
        /// </summary>
        protected abstract Type GenericType { get; }

        private Func<object, QuantumTheme, string> _recursiveSerializer;

        protected GenericDevConsoleSerializer()
        {
            if (!GenericType.IsGenericType)
            {
                throw new ArgumentException($"Generic Serializers must use a generic type as their base");
            }

            if (GenericType.IsConstructedGenericType)
            {
                throw new ArgumentException($"Generic Serializers must use an incomplete generic type as their base");
            }
        }

        public virtual int Priority => -500;

        public bool CanSerialize(Type type)
        {
            return type.IsGenericTypeOf(GenericType);
        }

        string IDevConsoleSerializer.SerializeFormatted(object value, QuantumTheme theme, Func<object, QuantumTheme, string> recursiveSerializer)
        {
            _recursiveSerializer = recursiveSerializer;
            return SerializeFormatted(value, theme);
        }

        protected string SerializeRecursive(object value, QuantumTheme theme)
        {
            return _recursiveSerializer(value, theme);
        }

        public abstract string SerializeFormatted(object value, QuantumTheme theme);
    }
}