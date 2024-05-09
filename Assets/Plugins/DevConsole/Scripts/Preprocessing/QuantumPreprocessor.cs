using System;
using System.Collections.Generic;
using System.Linq;

namespace Necroisle.DevConsole
{
    /// <summary>
    /// Handles preprocessing of console input.
    /// </summary>
    public class QuantumPreprocessor
    {
        private readonly IDevConsolePreprocessor[] _preprocessors;

        /// <summary>
        /// Creates a Quantum Preprocessor with a custom set of preprocessors.
        /// </summary>
        /// <param name="preprocessors">The IDevConsolePreprocessors to use in this Quantum Preprocessor.</param>
        public QuantumPreprocessor(IEnumerable<IDevConsolePreprocessor> preprocessors)
        {
            _preprocessors = preprocessors.OrderByDescending(x => x.Priority)
                                          .ToArray();
        }

        /// <summary>
        /// Creates a Quantum Preprocessor with the default injected preprocessors
        /// </summary>
        public QuantumPreprocessor() : this(new InjectionLoader<IDevConsolePreprocessor>().GetInjectedInstances())
        {

        }

        /// <summary>
        /// Processes the provided text.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <returns>The processed text.</returns>
        public string Process(string text)
        {
            foreach (IDevConsolePreprocessor preprocessor in _preprocessors)
            {
                try
                {
                    text = preprocessor.Process(text);
                }
                catch (Exception e)
                {
                    throw new Exception($"Preprocessor {preprocessor} failed:\n{e.Message}", e);
                }
            }

            return text;
        }
    }
}
